using iTextSharp.text;
using iTextSharp.text.pdf.parser.clipper;
using java.io;
//using java.lang;
using java.security;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


using System.Web.UI;
using static EsignAesEncryption;

public class EsignAesEncryption
{
    public static string Encrypt(string plainText, string key)
    {
        // Convert the key and the plain text to byte arrays
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

        // Ensure the key is exactly 32 bytes (AES-256)
        if (keyBytes.Length != 32)
        {
            Array.Resize(ref keyBytes, 32);
        }

        // Create a new AES instance
        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.GenerateIV(); // Generate a random IV for security
            byte[] iv = aes.IV;

            // Create an encryptor
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream())
            {
                // Prepend the IV to the encrypted data
                ms.Write(iv, 0, iv.Length);

                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(plainBytes, 0, plainBytes.Length);
                    cs.FlushFinalBlock();
                }

                // Get the encrypted data
                byte[] encryptedBytes = ms.ToArray();
                // Convert to base64 string for easier handling
                return Convert.ToBase64String(encryptedBytes);
            }
        }
    }

    public static string Decrypt(string encryptedText, string key)
    {
        // Convert the key and encrypted text to byte arrays
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

        // Ensure the key is exactly 32 bytes (AES-256)
        if (keyBytes.Length != 32)
        {
            Array.Resize(ref keyBytes, 32);
        }

        // Create a new AES instance
        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;

            // Extract the IV from the beginning of the encrypted data
            byte[] iv = new byte[aes.BlockSize / 8];
            Array.Copy(encryptedBytes, 0, iv, 0, iv.Length);
            aes.IV = iv;

            // Create a decryptor
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            // Prepare a memory stream with the encrypted data minus the IV
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(encryptedBytes, iv.Length, encryptedBytes.Length - iv.Length);
                ms.Position = 0;

                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (MemoryStream decryptedStream = new MemoryStream())
                    {
                        cs.CopyTo(decryptedStream);
                        byte[] decryptedBytes = decryptedStream.ToArray();

                        // Convert decrypted bytes to a string
                        return Encoding.UTF8.GetString(decryptedBytes);
                    }
                }
            }
        }
    }

    public static void Main()
    {
        string jsonString = "{\"name\":\"John Doe\",\"age\":30}";
        string aesKey = "your-256-bit-key-here-should-be-32-bytes!"; // 32 bytes key for AES-256

        string encrypted = Encrypt(jsonString, aesKey);

    }


    public static EncryptionResult Encrypt_New(string plainText, string key)
    {
        EncryptionResult model = new EncryptionResult();
        try
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            // Ensure the key is exactly 32 bytes (AES-256)
            if (keyBytes.Length != 32)
            {
                Array.Resize(ref keyBytes, 32);
            }

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.GenerateIV();
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    model.IsSuccess = true;
                    model.EncriptedData = msEncrypt.ToArray();
                }
            }
            return model;
        }
        catch (System.Exception ex)
        {
            model.IsSuccess = false;
            model.Message = ex.Message;
            return model;
        }
    }

    public static EncryptionResult Decrypt_New(string encryptedText, string keyString)
    {
        EncryptionResult model = new EncryptionResult();
        byte[] key = Encoding.UTF8.GetBytes(keyString);
        byte[] cipherText = Convert.FromBase64String(encryptedText);
        try
        {
            using (Aes aesAlg = Aes.Create())
            {

                aesAlg.Key = key;
                byte[] iv = new byte[aesAlg.IV.Length];
                Array.Copy(cipherText, 0, iv, 0, iv.Length);
                aesAlg.IV = iv;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(cipherText, iv.Length, cipherText.Length - iv.Length))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            model.IsSuccess = true;
                            model.DecriptedData = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            model.IsSuccess = false;
            model.Message = ex.Message;
        }
        return model;
    }
    public static string ComputeSHA256Hash(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            StringBuilder hex = new StringBuilder(hashBytes.Length * 2);
            foreach (byte b in hashBytes)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }
    }


}
public class EncryptionResult
{
    public bool IsSuccess { get; set; }
    public byte[] EncriptedData { get; set; } = new byte[0];
    public string DecriptedData { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}