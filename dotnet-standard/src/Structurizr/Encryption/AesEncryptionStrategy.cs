using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace Structurizr.Encryption
{
    [DataContract]
    public class AesEncryptionStrategy : EncryptionStrategy
    {

        private const int InitializationVectorSizeInBytes = 16;

        public override string Type
        {
            get
            {
                return "aes";
            }
        }

        [DataMember(Name = "keySize", EmitDefaultValue = false)]
        public int KeySize { get; private set; }

        [DataMember(Name = "iterationCount", EmitDefaultValue = false)]
        public int IterationCount { get; private set; }

        [DataMember(Name = "salt", EmitDefaultValue = false)]
        public string Salt { get; private set; }

        [DataMember(Name = "iv", EmitDefaultValue = false)]
        public string Iv { get; private set; }

        public AesEncryptionStrategy() { }

        public AesEncryptionStrategy(string passphrase) : this(128, 1000, passphrase) { }

        public AesEncryptionStrategy(int keySize, int iterationCount, string passphrase) : base(passphrase)
        {
            this.KeySize = keySize;
            this.IterationCount = iterationCount;

            // create a random salt
            using (var random = RandomNumberGenerator.Create())
            {
                byte[] saltAsBytes = new byte[keySize / 8];
                random.GetBytes(saltAsBytes);
                this.Salt = BitConverter.ToString(saltAsBytes).Replace("-", "");
            }

            // and a random IV
            using (var random = RandomNumberGenerator.Create())
            {
                byte[] ivAsBytes = new byte[InitializationVectorSizeInBytes];
                random.GetBytes(ivAsBytes);
                this.Iv = BitConverter.ToString(ivAsBytes).Replace("-", "");
            }
        }

        public AesEncryptionStrategy(int keySize, int iterationCount, string salt, string iv, string passphrase) : base(passphrase)
        {
            this.KeySize = keySize;
            this.IterationCount = iterationCount;
            this.Salt = salt;
            this.Iv = iv;
        }

        public override string Decrypt(string ciphertext)
        {
            string plaintext = null;
            byte[] decryptedBytes;

            using (MemoryStream ms = new MemoryStream())
            {
                using (var AES = Aes.Create())
                {
                    AES.KeySize = this.KeySize;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(
                        Encoding.UTF8.GetBytes(this.Passphrase),
                        hexStringToByteArray(this.Salt),
                        this.IterationCount);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = hexStringToByteArray(this.Iv);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        byte[] bytesToBeDecrypted = Convert.FromBase64String(ciphertext);
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                    }

                    decryptedBytes = ms.ToArray();
                    plaintext = Encoding.UTF8.GetString(decryptedBytes);
                }
            }

            return plaintext;
        }

        public override string Encrypt(string plaintext)
        {
            string ciphertext = null;
            byte[] encryptedBytes;

            using (MemoryStream ms = new MemoryStream())
            {
                using (var AES = Aes.Create())
                {
                    AES.KeySize = this.KeySize;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(
                        Encoding.UTF8.GetBytes(this.Passphrase),
                        hexStringToByteArray(this.Salt),
                        this.IterationCount);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = hexStringToByteArray(this.Iv);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        byte[] plaintextAsBytes = Encoding.UTF8.GetBytes(plaintext);
                        cs.Write(plaintextAsBytes, 0, plaintextAsBytes.Length);
                    }

                    encryptedBytes = ms.ToArray();
                    ciphertext = Convert.ToBase64String(encryptedBytes);
                }
            }

            return ciphertext;
        }

        private byte[] hexStringToByteArray(string hex)
        {
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                string byteValue = hex.Substring(i * 2, 2);
                bytes[i] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return bytes;
        }

    }
}
