using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Cooperative.Models
{
    #region Class For Encrypton and logging (Security)

    /// <summary>
    /// Class : Containing Pasword Hash
    /// </summary>
    public class EncryptionKey
    {
        public static readonly string PasswordHash = "$*#Sparrow^!0jP"; // String for Hashing Password
    }

    /// <summary>
    /// Encryption (By Hashing)
    /// </summary>
    public class Encryption
    {
        /// <summary>
        /// Encrypt a string by Hashing
        /// </summary>
        /// <param name="toEncrypt">string  which is to be encrypt</param>
        /// <param name="HashingKey">Hash key for encryption</param>
        /// <returns>Encrypted string</returns>
        public static string Encrypt(string toEncrypt, string HashingKey)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(HashingKey));

            //Release the resources and flush data of the Cryptographic service provide
            hashmd5.Clear();


            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;

            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;

            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            //Release resources held by TripleDes Encryptor
            tdes.Clear();

            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cipherString">string  which is to be decrypt</param>
        /// <param name="HashingKey">Hash key for decryption (same as per encrypt for proper decryption)</param>
        /// <returns>Decrypted string</returns>
        public static string Decrypt(string cipherString, string HashingKey)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            //if hashing was used get the hash code with regards to your key
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(HashingKey));

            //release any resource held by the MD5CryptoServiceProvider
            hashmd5.Clear();


            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;

            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;

            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            //Release resources held by TripleDes Encryptor                
            tdes.Clear();

            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }

    /// <summary>
    /// Add Error Log in Database
    /// </summary>
    public class Logging
    {
        /// <summary>
        /// Add Http Message to database which is sent by Error Handler (MVC)
        /// </summary>
        /// <param name="_ErrorMessage">error message which is to be save at database</param>
        public static void Add(string _ErrorMessage)
        {
            try
            {
                //Repository<ErrorLog> _Error = new Repository<ErrorLog>();
                //ErrorLog _ErrLogInst = new ErrorLog();
                //_ErrLogInst.OccuranceDateTime = DateTime.Now;
                //_ErrLogInst.Message = _ErrorMessage;
                //_Error.Add(_ErrLogInst);
            }
            catch (Exception e)
            {
                // If Database connection failes then save to local server resource (In .txt file)
                string path = System.Web.HttpContext.Current.Server.MapPath("~/Content/ErrorLog/" + DateTime.Now.Date.ToString().Substring(0, 10).Replace("/", "").Replace("-", "") + ".txt");

                if (!System.IO.File.Exists(path))
                {
                    FileStream fs1 = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter tw = new StreamWriter(fs1);
                    tw.WriteLine("Stack Trace:- " + e.StackTrace);
                    tw.WriteLine("Message:- " + e.Message + " Time:" + DateTime.Now.ToString());
                    tw.WriteLine("Time:- " + DateTime.Now.ToString());
                    tw.WriteLine("-------------------------------------------------");
                    tw.Close();
                }
                else
                {
                    FileStream fs1 = new FileStream(path, FileMode.Append, FileAccess.Write);
                    StreamWriter tw = new StreamWriter(fs1);
                    tw.WriteLine("Stack Trace:- " + e.StackTrace);
                    tw.WriteLine("Message:- " + e.Message + " Time:" + DateTime.Now.ToString());
                    tw.WriteLine("Time:- " + DateTime.Now.ToString());
                    tw.WriteLine("-------------------------------------------------");
                    tw.Close();
                }

            }
        }
    }
    #endregion
}