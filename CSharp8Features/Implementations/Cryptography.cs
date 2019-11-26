using CSharp8Features.Interfaces;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CSharp8Features.Implementations
{
    public class Cryptography : IExecutable
    {
        public async Task Execute()
        {
            string text = "Encrypt/Decrypt using Aes algorithm";

            using var ars = new AesCng();
            byte[] bytes = await Encrypt(text, ars.Key, ars.IV);

            string sameText = await Decrypt(bytes, ars.Key, ars.IV);

            Console.WriteLine($"Original text = '{text}' & DecryptedText = '{sameText}'");
        }

        static async Task<byte[]> Encrypt(string text, byte[] key, byte[] vector)
        {
            byte[] bytes;
            using (var aes = new AesCng())
            {
                aes.Key = key;
                aes.IV = vector;

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (var writer = new StreamWriter(cryptoStream))
                        {
                            await writer.WriteAsync(text);
                        }
                    }

                    bytes = memoryStream.ToArray();
                }

                return bytes;
            }
        }

        static async Task<string> Decrypt(byte[] bytes, byte[] key, byte[] vector)
        {
            string result;
            using (var aes = new AesCng())
            {
                aes.Key = key;
                aes.IV = vector;
                using (var memoryStream = new MemoryStream(bytes))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (var reader = new StreamReader(cryptoStream))
                        {
                            result = await reader.ReadToEndAsync();
                        }
                    }
                }
            }

            return result;
        }
    }
}