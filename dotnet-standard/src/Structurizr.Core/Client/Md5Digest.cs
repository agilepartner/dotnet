using System;
using System.Security.Cryptography;
using System.Text;

namespace Structurizr.Client
{
    internal class Md5Digest
    {
        internal string Generate(string content)
        {
            if (content == null)
            {
                content = "";
            }

            return MD5Hash(content);
        }

        private static string MD5Hash(string content)
        {
            using (var md5 = MD5.Create())
            {
                byte[] textToHash = Encoding.UTF8.GetBytes(content);
                byte[] result = md5.ComputeHash(textToHash);

                return BitConverter.ToString(result).Replace("-", "").ToLower();
            }
        }
    }
}