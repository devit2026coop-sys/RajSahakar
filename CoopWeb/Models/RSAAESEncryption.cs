using Cooperative.Controllers;
using Microsoft.Ajax.Utilities;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace Cooperative.Models
{
    public class RSAAESEncryption
    {
        public static int AES_KEY_SIZE = 256;
        public static int AES_ENC_KEY_SIZE = 2048;
        public static int GCM_IV_SIZE = 128;
        public static int GCM_TAG_LENGTH = 128;

        public static void Encrypt(string inputfilename, string outputfilename, string Certificate)
        {
            X509Certificate2 cert = new X509Certificate2(Certificate);
            RSACryptoServiceProvider key = cert.PublicKey.Key as RSACryptoServiceProvider;
            RSAParameters publicKey = key.ExportParameters(false);
            byte[] fileBytes = File.ReadAllBytes(inputfilename);
            byte[] secretKey = new byte[AES_KEY_SIZE / 8];
            SecureRandom random = new SecureRandom();
            random.NextBytes(secretKey);
            byte[] aesEncBytes = encryptAES(secretKey, fileBytes);
            byte[] rsaEnckey = encryptRSA(publicKey, secretKey, true);
            using (var combinedStream = new MemoryStream())
            {
                using (var binaryWriter = new BinaryWriter(combinedStream))
                {
                    binaryWriter.Write(rsaEnckey);
                    binaryWriter.Write(aesEncBytes);
                }
                byte[] encryptedData = combinedStream.ToArray();
                String encdata = Convert.ToBase64String(encryptedData);

                File.WriteAllText(outputfilename, encdata);
            }
        }

        public static void Decrypt(string inputfilename, string outputfilename, string privateKeyFile, string privateKeyPassword)
        {
            // get private key from private key file(PKCS12 format, .pfx)
            X509Certificate2 certificate = new
            X509Certificate2(privateKeyFile, privateKeyPassword,
            X509KeyStorageFlags.Exportable |
            X509KeyStorageFlags.PersistKeySet);
            RSACryptoServiceProvider privateKey =
            (RSACryptoServiceProvider)certificate.PrivateKey;
            RSAParameters RSAPrivateKeyInfo =
            privateKey.ExportParameters(true);
            // get file bytes
            string filedata = File.ReadAllText(inputfilename);
            // decode file text to byte array
            byte[] fileBytes = Convert.FromBase64String(filedata);
            int encAESKeySize = AES_ENC_KEY_SIZE / 8;
            byte[] aesEnckey = new byte[encAESKeySize];
            byte[] aesEncBytes = new byte[fileBytes.Length -
            encAESKeySize];
            //get encrypted AES key - First 2048 bit of file bytes
            Array.Copy(fileBytes, aesEnckey, encAESKeySize);
            //get AES encrypted file bytes
            Array.Copy(fileBytes, encAESKeySize, aesEncBytes, 0,
            fileBytes.Length - encAESKeySize);
            //decrypt AES secret key
            byte[] secretKey = decryptRSA(RSAPrivateKeyInfo, aesEnckey,
            true);
            //decrypt file bytes using AES
            byte[] plainBytes = decryptAES(secretKey, aesEncBytes);
            //write decrypted file bytes to a file
            File.WriteAllBytes(outputfilename, plainBytes);
        }
        public static byte[] encryptAES(byte[] key, byte[] fileBytes)
        {
            // initilize a random 128 bit IV
            var iv = new byte[GCM_IV_SIZE / 8];
            SecureRandom random = new SecureRandom();
            random.NextBytes(iv, 0, iv.Length);
            //use GCM Block Cipher
            GcmBlockCipher cipher = new GcmBlockCipher(new AesEngine());
            var parameters = new AeadParameters(new KeyParameter(key),
            GCM_TAG_LENGTH, iv);
            cipher.Init(true, parameters);
            //Generate Cipher Text
            var cipherText = new byte[cipher.GetOutputSize(fileBytes.Length)];
            var len = cipher.ProcessBytes(fileBytes, 0, fileBytes.Length, cipherText, 0);
            cipher.DoFinal(cipherText, len);
            //Assemble Message
            using (var combinedStream = new MemoryStream())
            {
                using (var binaryWriter = new BinaryWriter(combinedStream))
                {
                    //write iv
                    binaryWriter.Write(iv);
                    //Write Cipher Text
                    binaryWriter.Write(cipherText);
                }
                byte[] encryptedData = combinedStream.ToArray();
                return encryptedData;
            }
        }
        public static byte[] decryptAES(byte[] secretkey, byte[] inputBytes)
        {
            using (var cipherStream = new MemoryStream(inputBytes))
            using (var cipherReader = new BinaryReader(cipherStream))
            {//get IV - first bit of file as 128
                var iv = cipherReader.ReadBytes(GCM_IV_SIZE / 8);
                //use GCM Block Cipher
                var cipher = new GcmBlockCipher(new AesEngine());
                var parameters = new AeadParameters(new
                KeyParameter(secretkey), GCM_TAG_LENGTH, iv);
                cipher.Init(false, parameters);
                //Decrypt Cipher Text
                var cipherText = cipherReader.ReadBytes(inputBytes.Length -
               iv.Length);
                var outputBytes = new
                byte[cipher.GetOutputSize(cipherText.Length)];
                var len = cipher.ProcessBytes(cipherText, 0,
                cipherText.Length, outputBytes, 0);
                cipher.DoFinal(outputBytes, len);
                return outputBytes;
            }
        }
        public static byte[] encryptRSA(RSAParameters publicKey, byte[]
        secretKey, bool DoOAEPPadding)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.ImportParameters(publicKey);
            return RSA.Encrypt(secretKey, DoOAEPPadding);
        }
        public static byte[] decryptRSA(RSAParameters RSAPrivateKeyInfo,
        byte[] aesEnckey, bool DoOAEPPadding)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.ImportParameters(RSAPrivateKeyInfo);
            return RSA.Decrypt(aesEnckey, DoOAEPPadding);
        }
    }
}