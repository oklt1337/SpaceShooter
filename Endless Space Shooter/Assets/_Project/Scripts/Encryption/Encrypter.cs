using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace _Project.Scripts.Encryption
{
    public static class Encrypter
    {
        public static string Encrypt(string text)
        {
            var iv = new byte[16];
            byte[] array;
            const string key = "b14ca5898a4e4133bbce2ea2315a1916";

            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(text);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }
    }
}
