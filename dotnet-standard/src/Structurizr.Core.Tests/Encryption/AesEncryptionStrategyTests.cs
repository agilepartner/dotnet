using FluentAssertions;
using Structurizr.Encryption;
using System.Security.Cryptography;
using Xunit;

namespace Structurizr.CoreTests.Encryption
{
    public class AesEncryptionStrategyTests
    {

        private AesEncryptionStrategy strategy;

        [Fact]
        public void Test_Encrypt_EncryptsPlaintext()
        {
            strategy = new AesEncryptionStrategy(128, 1000, "06DC30A48ADEEE72D98E33C2CEAEAD3E", "ED124530AF64A5CAD8EF463CF5628434", "password");

            string ciphertext = strategy.Encrypt("Hello world");

            ciphertext.Should().Be("A/DzjV17WVS6ZAKsLOaC/Q==");
        }

        [Fact]
        public void Test_Decrypt_DecryptsTheCiphertext_WhenTheSameStrategyInstanceIsUsed()
        {
            strategy = new AesEncryptionStrategy(128, 1000, "password");

            string ciphertext = strategy.Encrypt("Hello world");

            strategy.Decrypt(ciphertext).Should().Be("Hello world");
        }

        [Fact]
        public void Test_Decrypt_DecryptsTheCiphertext_WhenTheSameConfigurationIsUsed()
        {
            strategy = new AesEncryptionStrategy(128, 1000, "password");

            string ciphertext = strategy.Encrypt("Hello world");

            strategy = new AesEncryptionStrategy(strategy.KeySize, strategy.IterationCount, strategy.Salt, strategy.Iv, "password");

            strategy.Decrypt(ciphertext).Should().Be("Hello world");
        }

        [Fact]
        public void Test_Decrypt_DoesNotDecryptTheCiphertext_WhenTheIncorrectKeySizeIsUsed()
        {
            strategy = new AesEncryptionStrategy(128, 1000, "password");
            string ciphertext = strategy.Encrypt("Hello world");
            strategy = new AesEncryptionStrategy(256, strategy.IterationCount, strategy.Salt, strategy.Iv, "password");

            Assert.Throws<CryptographicException>(() => strategy.Decrypt(ciphertext));
        }

        [Fact]
        public void Test_Decrypt_DoesNotDecryptTheCiphertext_WhenTheIncorrectIterationCountIsUsed()
        {
            strategy = new AesEncryptionStrategy(128, 1000, "password");
            string ciphertext = strategy.Encrypt("Hello world");
            strategy = new AesEncryptionStrategy(strategy.KeySize, 2000, strategy.Salt, strategy.Iv, "password");

            Assert.Throws<CryptographicException>(() => strategy.Decrypt(ciphertext));
        }

        [Fact]
        public void Test_Decrypt_DoesNotDecryptTheCiphertext_WhenTheIncorrectSaltIsUsed()
        {
            strategy = new AesEncryptionStrategy(128, 1000, "password");
            string ciphertext = strategy.Encrypt("Hello world");
            strategy = new AesEncryptionStrategy(strategy.KeySize, strategy.IterationCount, "133D30C2A658B3081279A97FD3B1F7CDE10C4FB61D39EEA8", strategy.Iv, "password");

            Assert.Throws<CryptographicException>(() => strategy.Decrypt(ciphertext));
        }

        [Fact]
        public void test_decrypt_doesNotDecryptTheCiphertext_WhenTheIncorrectIvIsUsed()
        {
            strategy = new AesEncryptionStrategy(128, 1000, "password");
            string ciphertext = strategy.Encrypt("Hello world");
            strategy = new AesEncryptionStrategy(strategy.KeySize, strategy.IterationCount, strategy.Salt, "1DED89E4FB15F61DC6433E3BADA4A891", "password");

            Assert.Throws<CryptographicException>(() => strategy.Decrypt(ciphertext));
        }

        [Fact]
        public void test_decrypt_doesNotDecryptTheCiphertext_WhenTheIncorrectPassphraseIsUsed()
        {
            strategy = new AesEncryptionStrategy(128, 1000, "password");
            string ciphertext = strategy.Encrypt("Hello world");
            strategy = new AesEncryptionStrategy(strategy.KeySize, strategy.IterationCount, strategy.Salt, strategy.Iv, "The Wrong Password");

            Assert.Throws<CryptographicException>(() => strategy.Decrypt(ciphertext));
        }
    }
}