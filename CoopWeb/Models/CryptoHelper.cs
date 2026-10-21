using Cooperative.Controllers;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Cooperative.JanadhaarClasses
{
	public class CryptoHelper
	{
		private RSA publicKey;   // Jan Aadhaar Public Key (from their portal)
		private RSA privateKey;  // Your Private Key
  
        public CryptoHelper(RSA publicKey, RSA privateKey)
		{
			this.publicKey = publicKey;
			this.privateKey = privateKey;
		}
		public static RSA LoadPublicKey(string certPath)
		{
			string text = File.ReadAllText(certPath).Trim();

            byte[] certBytes;

			if (text.StartsWith("-----BEGIN CERTIFICATE-----"))
			{
				// PEM/Base64 format
				string base64 = text
					.Replace("-----BEGIN CERTIFICATE-----", "")
					.Replace("-----END CERTIFICATE-----", "")
					.Replace("\r", "")
					.Replace("\n", "")
					.Trim();

				certBytes = Convert.FromBase64String(base64);
			}
			else
			{
				// DER/Binary format
				certBytes = File.ReadAllBytes(certPath);
			}

			// Try to load certificate bytes
			try
			{
				var cert = new X509Certificate2(certBytes);
				return cert.GetRSAPublicKey();
			}
			catch (CryptographicException ex)
			{
				throw new Exception("Invalid certificate format. Ensure RJAA.cer is in correct DER or PEM format.", ex);
			}
		}
		public static X509Certificate2 LoadCertificate(string path)
		{
			string text = File.ReadAllText(path);
            if (text.Contains("-----BEGIN CERTIFICATE-----"))
			{
				// PEM format → strip headers/footers
				string base64 = text
					.Replace("-----BEGIN CERTIFICATE-----", "")
					.Replace("-----END CERTIFICATE-----", "")
					.Replace("\r", "")
					.Replace("\n", "")
					.Trim();

				byte[] certBytes = Convert.FromBase64String(base64);
				return new X509Certificate2(certBytes);
			}
			else
			{
				// DER / Binary format
				byte[] certBytes = File.ReadAllBytes(path);
				return new X509Certificate2(certBytes);
			}
		}
		public string EncryptDataWithAES(string data)
		{
			using (var aesKey = GenerateAESKey())
			{
				using (var cipher = Aes.Create())
				{
					cipher.Mode = CipherMode.CBC;
					cipher.Padding = PaddingMode.PKCS7;

					byte[] iv = new byte[16];
					using (var rng = new RNGCryptoServiceProvider())
					{
						rng.GetBytes(iv);
					}
					cipher.IV = iv;
					cipher.Key = aesKey.Key;

					using (var encryptor = cipher.CreateEncryptor())
					{
						byte[] encryptedData = encryptor.TransformFinalBlock(
							Encoding.UTF8.GetBytes(data), 0, data.Length);

						string encryptedDataBase64 = Convert.ToBase64String(encryptedData);
						string ivBase64 = Convert.ToBase64String(iv);
						string encryptedAESKey = EncryptAESKeyWithRSA(aesKey);

						return $"{ivBase64}:{encryptedDataBase64}:{encryptedAESKey}";
					}
				}
			}
		}
		public static Aes DecryptAESKeyWithRSA(string encryptedAESKey)
		{           
            var cert = new X509Certificate2(JanAadhaarConfig.PrivateCertPath, JanAadhaarConfig.PrivateCertPassword, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);

			using (RSA rsa = cert.GetRSAPrivateKey())
			{
				byte[] encryptedAESKeyBytes = Convert.FromBase64String(encryptedAESKey);
				byte[] decryptedAESKeyBytes = rsa.Decrypt(encryptedAESKeyBytes, RSAEncryptionPadding.Pkcs1);

				var aes = Aes.Create();
				aes.Key = decryptedAESKeyBytes;
				aes.Mode = CipherMode.CBC;
				aes.Padding = PaddingMode.PKCS7;

				return aes;
			}
		}
		private string EncryptAESKeyWithRSA(Aes aesKey)
		{
			using (var cipher = RSA.Create())
			{
				cipher.ImportParameters(publicKey.ExportParameters(false));
				byte[] encryptedAESKey = cipher.Encrypt(aesKey.Key, RSAEncryptionPadding.Pkcs1);
				return Convert.ToBase64String(encryptedAESKey);
			}
		}



		private Aes GenerateAESKey()
		{
			var aes = Aes.Create();
			aes.KeySize = 256;
			aes.GenerateKey();
			return aes;
		}

		public string SignData(string data)
		{
			byte[] dataBytes = Encoding.UTF8.GetBytes(data);
			byte[] digitalSignature = privateKey.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
			return Convert.ToBase64String(digitalSignature);
		}
		public static string GetSha256Fingerprint(string certPath)
		{
			var cert = new X509Certificate2(certPath);
			using (var sha256 = SHA256.Create())
			{
				byte[] hashBytes = sha256.ComputeHash(cert.RawData);
				return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
			}
		}

		public string DecryptDataWithAES1(string encryptedData)
		{
			// Split the encrypted string into IV, encrypted data, and encrypted
			//	AES key
			string[] parts = encryptedData.Split(':');
			string ivBase64 = parts[0];
			string encryptedDataBase64 = parts[1];
			string encryptedAESKey = parts[2];
			// Decode the base64-encoded IV, encrypted data, and AES key
			byte[] iv = Convert.FromBase64String(ivBase64);
			byte[] encryptedDataBytes =
			Convert.FromBase64String(encryptedDataBase64);
			// Decrypt the AES key using RSA
			using (var aesKey = DecryptAESKeyWithRSA(encryptedAESKey))
			{
				// Decrypt the data with AES
				using (var cipher = Aes.Create())
				{
					cipher.Mode = CipherMode.CBC;
					cipher.Padding = PaddingMode.PKCS7;
					cipher.IV = iv;
					cipher.Key = aesKey.Key;
					using (var decryptor = cipher.CreateDecryptor())
					{
						byte[] decryptedData =
						decryptor.TransformFinalBlock(encryptedDataBytes, 0,
						encryptedDataBytes.Length);
						return Encoding.UTF8.GetString(decryptedData);
					}
				}
			}
		}

		public static string DecryptDataWithAES(string encryptedData)
		{
			// encryptedData format: "<IV_Base64>:<EncryptedData_Base64>:<EncryptedAESKey_Base64>"
			string[] parts = encryptedData.Split(':');
			if (parts.Length != 3)
				throw new ArgumentException("Invalid encrypted data format.");

			string ivBase64 = parts[0];
			string encryptedDataBase64 = parts[1];
			string encryptedAESKeyBase64 = parts[2];

			// Convert from Base64 strings to byte arrays
			byte[] iv = Convert.FromBase64String(ivBase64);
			byte[] encryptedDataBytes = Convert.FromBase64String(encryptedDataBase64);

			// Decrypt the AES key first using your RSA private key method
			using (var aes = DecryptAESKeyWithRSA(encryptedAESKeyBase64))
			{
				// Setup AES decryptor
				using (var aesAlg = Aes.Create())
				{
					aesAlg.Mode = CipherMode.CBC;
					aesAlg.Padding = PaddingMode.PKCS7;
					aesAlg.IV = iv;
					aesAlg.Key = aes.Key;

					using (var decryptor = aesAlg.CreateDecryptor())
					{
						byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedDataBytes, 0, encryptedDataBytes.Length);
						return Encoding.UTF8.GetString(decryptedBytes);
					}
				}
			}
		}

		public Aes DecryptAESKeyWithRSANew(string encryptedAESKey)
		{
			byte[] encryptedAESKeyBytes =
			Convert.FromBase64String(encryptedAESKey);
			using (var cipher = RSA.Create())
			{
				cipher.ImportParameters(privateKey.ExportParameters(true));
				byte[] decryptedAESKeyBytes =
				cipher.Decrypt(encryptedAESKeyBytes, RSAEncryptionPadding.Pkcs1);
				var aes = Aes.Create();
				aes.Key = decryptedAESKeyBytes;
				return aes;
			}
		}
	}
}



