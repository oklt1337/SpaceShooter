using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace _Project.Scripts.Encryption
{
    public static class Decrypter
    {
        public static string Decrypt(string text)
        {
            var iv = new byte[16];
            var buffer = Convert.FromBase64String(text);
            const string key = "b14ca5898a4e4133bbce2ea2315a1916";

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;
            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var memoryStream = new MemoryStream(buffer);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);
            return streamReader.ReadToEnd();
        }
    }
}
