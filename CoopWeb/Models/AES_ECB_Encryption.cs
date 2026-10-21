using iTextSharp.text.pdf;
using java.security;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Cooperative.Models
{
    /// <summary>
    /// Summary description for AES_ECB_Encryption
    /// </summary>
    public class AES_ECB_Encryption
    {
        private static byte[] key;
        public AES_ECB_Encryption() { }

        private static void SetKey(string myKey)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(myKey);
                byte[] hash = sha1.ComputeHash(keyBytes);
                key = new byte[16];
                Array.Copy(hash, key, 16); // Use first 16 bytes for AES key
            }
        }

        public static string Encrypt(string strToEncrypt, string secret)
        {
            try
            {
                SetKey(secret);
                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.Mode = CipherMode.ECB;
                    aes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform encryptor = aes.CreateEncryptor();
                    byte[] inputBytes = Encoding.UTF8.GetBytes(strToEncrypt);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

                    return Convert.ToBase64String(encryptedBytes);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while encrypting: " + e.ToString());
            }
            return null;
        }

        public static string Decrypt(string strToDecrypt, string secret)
        {
            try
            {
                SetKey(secret);
                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.Mode = CipherMode.ECB;
                    aes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform decryptor = aes.CreateDecryptor();
                    byte[] inputBytes = Convert.FromBase64String(strToDecrypt);
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while decrypting: " + e.ToString());
            }
            return null;
        }
        //public static string Encrypt(string plainText, string key)
        //{
        //    using (Aes aes = Aes.Create())
        //    {
        //        aes.Key = Encoding.UTF8.GetBytes(key);
        //        aes.Mode = CipherMode.ECB;
        //        aes.Padding = PaddingMode.PKCS7;

        //        using (var encryptor = aes.CreateEncryptor())
        //        {
        //            byte[] inputBuffer = Encoding.UTF8.GetBytes(plainText);
        //            byte[] result = encryptor.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
        //            return Convert.ToBase64String(result);
        //        }
        //    }
        //}

        //public static string Decrypt(string base64EncryptedText, string key)
        //{
        //    if (key.Length != 16 && key.Length != 24 && key.Length != 32)
        //        throw new ArgumentException("Key must be 16, 24, or 32 characters for AES.");

        //    using (Aes aes = Aes.Create())
        //    {
        //        aes.Key = Encoding.UTF8.GetBytes(key);
        //        aes.Mode = CipherMode.ECB;
        //        aes.Padding = PaddingMode.PKCS7;

        //        using (ICryptoTransform decryptor = aes.CreateDecryptor())
        //        {
        //            byte[] inputBytes = Convert.FromBase64String(base64EncryptedText);
        //            byte[] decryptedBytes = decryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
        //            return Encoding.UTF8.GetString(decryptedBytes);
        //        }
        //    }
        //}

    }
}