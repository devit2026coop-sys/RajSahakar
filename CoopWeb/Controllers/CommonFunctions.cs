using Cooperative.DBConnector;
using Cooperative.JanadhaarClasses;
using Cooperative.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.css;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.html;
using LovePdf.Core;
using LovePdf.Model.Enums;
using LovePdf.Model.Task;
using LovePdf.Model.TaskParams;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using QRCoder;
using RAJSSO;
using sun.security.util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Util;
using System.Xml;
using static Cooperative.Controllers.ApplicationFlowController;
using static Cooperative.Models.JanAadharModel;
using static javax.net.ssl.SSLEngineResult;


namespace Cooperative.Controllers
{
    public class SessionIdentity
    {
        private SessionIdentity() { }
        public static readonly string Admin = "Session_For_Admin";
        public static readonly string Culture = "_culture";
        public static readonly string PublicUser = "PublicUser";
        public static readonly string SocietyIDEnc = "SocietyIDEnc";
        public static readonly string Identity = "Identity";
        public static readonly string ARequest = "ARequest";
        public static readonly string OditerMobile = "OditerMobile";
        public static readonly string SWCSData = "SWCS";
        public static readonly string GCCERNO = "GCCERNO";
        public static readonly string GCCEId = "GCCEId";
        public static readonly string ISRAJNIVESH = "ISRAJNIVESH";
        public static readonly string SwcToken = "SwcToken";
    }


    public static class JsonModelError
    {
        public static string Get(ICollection<System.Web.Mvc.ModelState> ModelStateMessageCollection)
        {
            if (ModelStateMessageCollection == null)
                return "{}";

            StringBuilder str = new StringBuilder();
            str.Append("{\"Status\":false,\"data\":[");

            foreach (System.Web.Mvc.ModelState modelState in ModelStateMessageCollection)
                foreach (var Message in modelState.Errors)
                    str.Append("\"" + Message.ErrorMessage + "\",");

            str.Append("#@#]}");

            return str.ToString().Replace(",#@#", "");
        }
    }

    public static class NewXmlNode
    {
        public static string sa;
        public static string _txnid;
    }
    public class ENCRYPTER
    {
        private const string ASYMMETRIC_ALGO = "RSA/ECB/PKCS1Padding";
        private const int SYMMETRIC_KEY_SIZE = 256;
        private DateTime certExpiryDate;
        private RsaKeyParameters publicKey;
        public static int IV_SIZE_BITS = 96;
        // Additional authentication data - last 128 bits of ISO format timestamp
        public static int AAD_SIZE_BITS = 128;
        // Authentication tag length - in bits
        public static int AUTH_TAG_SIZE_BITS = 128;

        public ENCRYPTER(string publicKeyFilePath)
        {
            FileStream fileStream = null;
            try
            {
                using (fileStream = new FileStream(publicKeyFilePath, FileMode.Open, FileAccess.Read))
                {
                    Org.BouncyCastle.X509.X509Certificate x509Certificate = new Org.BouncyCastle.X509.X509CertificateParser().ReadCertificate((Stream)fileStream);
                    this.publicKey = (RsaKeyParameters)x509Certificate.GetPublicKey();
                    this.certExpiryDate = x509Certificate.NotAfter;
                    fileStream.Close();
                }
            }
            catch (Exception innerException)
            {
                throw new Exception("Could not intialize encryption module", innerException);
            }
            finally
            {
                if (fileStream != null) { fileStream.Close(); }
            }
        }

        public ENCRYPTER(byte[] publicKeyBytes)
        {
            try
            {
                Org.BouncyCastle.X509.X509Certificate x509Certificate = new Org.BouncyCastle.X509.X509CertificateParser().ReadCertificate(publicKeyBytes);
                this.publicKey = (RsaKeyParameters)x509Certificate.GetPublicKey();
                this.certExpiryDate = x509Certificate.NotAfter;
            }
            catch (Exception innerException)
            {
                throw new Exception("Could not intialize encryption module", innerException);
            }
        }

        public byte[] encryptUsingPublicKey(byte[] data)
        {
            IBufferedCipher cipher = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
            cipher.Init(true, this.publicKey);
            return cipher.DoFinal(data);
        }


        public byte[] encryptUsingSessionKey(byte[] skey, byte[] data)
        {
            PaddedBufferedBlockCipher paddedBufferedBlockCipher = new PaddedBufferedBlockCipher(new AesEngine(), new Pkcs7Padding());
            paddedBufferedBlockCipher.Init(true, new KeyParameter(skey));
            byte[] array = new byte[paddedBufferedBlockCipher.GetOutputSize(data.Length)];
            int num = paddedBufferedBlockCipher.ProcessBytes(data, 0, data.Length, array, 0);
            int num2 = paddedBufferedBlockCipher.DoFinal(array, num);
            byte[] array2 = new byte[num + num2];
            System.Array.Copy(array, 0, array2, 0, array2.Length);
            return array2;
        }

        // AES/GCM ENCRYPTION
        public byte[] encryptUsingSessionKeyNew(byte[] skey, byte[] data, string ts, bool isHmac)
        {
            byte[] tsBytes = null;
            try
            {
                tsBytes = Encoding.UTF8.GetBytes(ts);
                byte[] ivBytes = new byte[IV_SIZE_BITS / 8];
                byte[] aadBytes = new byte[AAD_SIZE_BITS / 8];
                System.Array.Copy(tsBytes, tsBytes.Length - 12, ivBytes, 0, ivBytes.Length);
                System.Array.Copy(tsBytes, tsBytes.Length - 16, aadBytes, 0, aadBytes.Length);

                var cipher = new GcmBlockCipher(new AesFastEngine());
                var parameters = new AeadParameters(new KeyParameter(skey), AUTH_TAG_SIZE_BITS, ivBytes, aadBytes);
                cipher.Init(true, parameters);
                byte[] result = new byte[cipher.GetOutputSize(data.Length)];
                int processLen = cipher.ProcessBytes(data, 0, data.Length, result, 0);
                cipher.DoFinal(result, processLen);
                if (isHmac) return result;
                // With Add for Session Key Encryption
                byte[] packedCipherData = new byte[result.Length + tsBytes.Length];
                System.Array.Copy(tsBytes, 0, packedCipherData, 0, tsBytes.Length);
                System.Array.Copy(result, 0, packedCipherData, tsBytes.Length, result.Length);
                return packedCipherData;
            }
            catch { throw; }
        }

        public byte[] generateSessionKey()
        {
            SecureRandom random = new SecureRandom();
            KeyGenerationParameters parameters = new KeyGenerationParameters(random, 256);
            CipherKeyGenerator keyGenerator = GeneratorUtilities.GetKeyGenerator("AES");
            keyGenerator.Init(parameters);
            return keyGenerator.GenerateKey();
        }

        public byte[] generateSha256Hash(byte[] message)
        {
            IDigest digest = new Sha256Digest();
            digest.Reset();
            byte[] array = new byte[digest.GetDigestSize()];
            digest.BlockUpdate(message, 0, message.Length);
            digest.DoFinal(array, 0);
            return array;
        }

        public string getCertificateIdentifier()
        {
            return this.certExpiryDate.ToString("yyyyMMdd");
        }
    }


    public static class COMMON_FUNCTION
    {
        public static string GenerateQRCode(string Data, int Pixels = 2)
        {
            StringBuilder bcode = new StringBuilder(Data);
            byte[] img = null;
            string QRimage = "";
            string code = bcode.ToString();
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            using (System.Drawing.Bitmap bitMap = qrCode.GetGraphic(Pixels))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    //var aa =new System.Drawing.Imaging.EncoderParameter(;
                    //bitMap.Height = 100;
                    //bitMap.Width = 100;
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);



                    img = new byte[ms.ToArray().Length];
                    img = ms.ToArray();
                    QRimage = Convert.ToBase64String(img);
                    //imgBarCode1.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                }
            }
            return "data:image/png;base64," + QRimage;
        }


        public static string AddStampOnPDF(string sourceFile, string DestinationFile, string base64encodedstring)
        {

            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(base64encodedstring.Split(',')[1]);
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            MemoryStream stream = new MemoryStream(byteArray);

            byte[] imageBytes = Convert.FromBase64String(base64encodedstring.Split(',')[1]);
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageBytes);
            //image.Width = 80;
            //image.Height = 80;
            using (Stream inputPdfStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            //using (Stream inputImageStream = stream) ;//FileStream(Server.MapPath("~") + "/logo/test.jpg", FileMode.Open, FileAccess.Read, FileShare.Read))
            using (Stream outputPdfStream = new FileStream(DestinationFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var reader = new PdfReader(inputPdfStream);
                var stamper = new PdfStamper(reader, outputPdfStream);
                var pdfContentByte = stamper.GetOverContent(1);

                //iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(stream);
                image.SetAbsolutePosition(pdfContentByte.PdfDocument.PageSize.Width - 100, pdfContentByte.PdfDocument.PageSize.Height - 100);

                pdfContentByte.AddImage(image);
                stamper.Close();
            }
            return "";
        }


        public static string AddStampOnPDF(HttpPostedFileBase httpfile, string DestinationFile, string base64encodedstring)
        {

            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(base64encodedstring.Split(',')[1]);
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            MemoryStream stream = new MemoryStream(byteArray);
            //HttpPostedFileBase file=new ;

            byte[] imageBytes = Convert.FromBase64String(base64encodedstring.Split(',')[1]);
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageBytes);
            //image.Width = 80;
            //image.Height = 80;
            Stream inputPdfStream = httpfile.InputStream;

            //using (Stream inputPdfStream = new FileStream(file.InputStream, FileMode.Open, FileAccess.Read, FileShare.Read))
            //{
            //using (Stream inputImageStream = stream) ;//FileStream(Server.MapPath("~") + "/logo/test.jpg", FileMode.Open, FileAccess.Read, FileShare.Read))
            using (Stream outputPdfStream = new FileStream(DestinationFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var reader = new PdfReader(inputPdfStream);
                var stamper = new PdfStamper(reader, outputPdfStream);
                var pdfContentByte = stamper.GetOverContent(1);

                //iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(stream);
                image.SetAbsolutePosition(0, 0);

                pdfContentByte.AddImage(image);
                stamper.Close();
            }
            //}
            return "";
        }

        //public static void SendMSGForAdmin(string MobileNo)
        //{
        //    try
        //    {
        //        //string SMS_MSG = "Total application recieved:5813618 \nTotal Uploaded on PM Kishan:49.48 L \nDisbursement 1st installment:32.81 L\nDisbursement 2nd installment:21.84 L";  //07-08-2019
        //        string SMS_MSG = "Opening Stock of Fertilizer (in MTs) as on : 28/08/2019 \nTOTAL : 953.25 \nUREA : 345.60 \nDAP : 407.37 \nNPKS : 42.94 \nMOP: 24.55 \nSSP: 126.47 \nCOMPOST : 6.29 \nRegards \nDoit&c team";  //08-08-2019
        //        if (MobileNo.Length > 5 && SMS_MSG.Length > 5)
        //        {
        //            EsanchaarController EC = new EsanchaarController();
        //            string ReturnVal = EC.SendSocietySMS(SMS_MSG, MobileNo);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "SendMSG(string SocietyIDEnc ,int StatusId)" }, ex);
        //    }
        //    finally
        //    {
        //        // objDBAccess.CloseConnection();
        //        // dt.Dispose();
        //    }

        //}

        public static string SendSMS(string mobileNo, string message)
        {
            try
            {
                string username = ConfigurationSettings.AppSettings["SMSLogin"];
                string password = ConfigurationSettings.AppSettings["SMSPassword"];
                string senderid = ConfigurationSettings.AppSettings["SMSSenderId"];

                HttpWebRequest request;
                Stream dataStream;
                String secureKey = hashGenerator(username, senderid, message, "SMSKEY");
                //  (HttpWebRequest)WebRequest.Create("https://msdgweb.mgov.gov.in/esms/sendsmsrequest");
                request = (HttpWebRequest)WebRequest.Create("https://msdgweb.mgov.gov.in/esms/sendsmsrequest");
                request.ProtocolVersion = HttpVersion.Version10;
                //((HttpWebRequest)request).UserAgent = ".NET Framework Example Client"; 
                ((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
                request.Method = "POST";
                String smsservicetype = "singlemsg"; //For single message. 
                String query = "username=" + HttpUtility.UrlEncode(username) +
                "&password=" + HttpUtility.UrlEncode(password) +
                "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +
                "&content=" + HttpUtility.UrlEncode(message) +
                "&mobileno=" + HttpUtility.UrlEncode(mobileNo) +
                "&senderid=" + HttpUtility.UrlEncode(senderid) +
                        "&key=" + HttpUtility.UrlEncode(secureKey);

                byte[] byteArray = Encoding.ASCII.GetBytes(query);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;

                dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                String Status = ((HttpWebResponse)response).StatusDescription;
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
                return responseFromServer;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public static void SendMSGForAdmin(string MobileNo)
        {
            // MobileNo = "9461569464";
            try
            {
                string SMS_MSG = "वि. ले. प. दौसा के आदेश दिनांक 19 .11. 2019 अंतर्गत धारा 54 व नियम 73 (8 ) के क्रम में आपके द्वारा की गई ऑडिटर नियुक्ति को निरस्त किया जाता है।";
                //string SMS_MSG = "Total application recieved:5813618 \nTotal Uploaded on PM Kishan:49.48 L \nDisbursement 1st installment:32.81 L\nDisbursement 2nd installment:21.84 L";  //07-08-2019
                //string SMS_MSG = "Total 21368 Complaint (MSCS-24398, SSCS-58) of Rs 54831.94 Lacs. registered for ccs till 25/11/2019 6.30 PM Out of these : -\n 1 : Adarsh Credit : 8282 (27209.79 Lacs) \n2: Sanjivani : 12656 (21623.52 Lacs) \n 3: Navjeevan : 2606 (4433 Lacs)\nRegards Rajsahakar Team DoIT&C";  //08-08-2019
                //string SMS_MSG = "Top 5 Worst Performance in Society Data Uploading \n Jodhpur : 8 ,Jhalawar : 10, Sawai Madhopur :11 ,Jalore: 20 ,Rajsamand : 21 ";


                //string SMS_MSG = "Top 10 Best Performance in Society Data Uploading \n 1 Jaipur DR : 34860 \n 2 Nagaur DR : 8172  \n 3 Bhilwara DR  :6719  \n 4 JHUNJHUNNU DR : 6302 \n 5  Bikaner DR : 5862\n 6 CHURU  DR : 5646   \n 7 Hanumangarh DR : 4480 \n 8  Sri Ganganagar DR : 3258  \n 9  Bikaner (Dairy) : 2884 \n 10 Pali (Dairy)  :2745 ";
                //string SMS_MSG2 = "Top 10 Worst Performance in Society Data Uploading \n 1 Bharatpur DR: 0 \n 2 Dholpur DR : 0 \n 3  Barmer (Dairy) : 0  \n 4 Dungarpur DR  :0  \n 5 Sirohi (Dairy) : 0 \n 6 Tonk (Dairy) : 0 \n 7 Jodhpur DR 8 \n 8 Chittorgarh DR : 104 \n 9 Sirohi DR : 164 \n 10 Dausa DR : 466 ";


                //string SMS_MSG3 = "Top 10 Best Performance in Audit Uploading \n 1 Sri Ganganagar 174 \n 2  JAIPUR : 103  \n 3 Udaipur  : 89  \n 4 Rajsamand : 75 \n 5  Jodhpur : 34   \n 5 Hanumangarh  : 25 \n 6 Jalore  20  \n 7 Pali  : 17  \n 9 Dungarpur: 7  \n 10 Pratapgarh  : 7 ";
                //string SMS_MSG4 = "Top 10 Worst Performance in Audit Uploading \n 1 Bharatpur : 0  \n 2 Karauli : 0  \n 3 Baran 1 \n 4 Barmer : 1 \n 5  Bundi : 1  \n 6 Jaisalmer  :1 \n 7 Sawai Madhopur  : 1 \n 8 Banswara  :2  \n 9 Bhilwara: 2 \n 10 Alwar : 2 \n";

                if (MobileNo.Length > 5 && SMS_MSG.Length > 5)
                {
                    EsanchaarController EC = new EsanchaarController();
                    string ReturnVal = EC.SendSocietySMS(SMS_MSG, MobileNo, "HIN");
                }
            }
            catch (Exception ex)
            {
                // objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "SendMSG(string SocietyIDEnc ,int StatusId)" }, ex);
            }
            finally
            {
                // objDBAccess.CloseConnection();
                // dt.Dispose();
            }

        }

        public static String hashGenerator(String Username, String sender_id, String message, String secure_key)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Username).Append(sender_id).Append(message).Append(secure_key);
            byte[] genkey = Encoding.UTF8.GetBytes(sb.ToString());
            //static byte[] pwd = new byte[encPwd.Length];
            HashAlgorithm sha1 = HashAlgorithm.Create("SHA512");
            byte[] sec_key = sha1.ComputeHash(genkey);
            StringBuilder sb1 = new StringBuilder();
            for (int i = 0; i < sec_key.Length; i++)
            {
                sb1.Append(sec_key[i].ToString("x2"));
            }
            return sb1.ToString();
        }
        public static string SendSMSBULK(string Mobile, string Message)
        {
            try
            {
                string username = ConfigurationSettings.AppSettings["SMSLogin"];
                string password = ConfigurationSettings.AppSettings["SMSPassword"];
                string senderid = ConfigurationSettings.AppSettings["SMSSenderId"];
                bool isUnicode = IsUnicode(Message);
                if (isUnicode)
                    Message = GetUnicodeMessage(Message);

                String secureKey = hashGenerator(username, senderid, Message, "SMSKEY");

                HttpWebRequest request;
                Stream dataStream;

                request = (HttpWebRequest)WebRequest.Create("http://msdgweb.mgov.gov.in/esms/sendsmsrequest");
                request.ProtocolVersion = HttpVersion.Version10;
                //((HttpWebRequest)request).UserAgent = ".NET Framework Example Client"; 
                ((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
                request.Method = "POST";
                String smsservicetype = "bulkmsg"; // for bulk msg
                smsservicetype = isUnicode ? "unicodemsg" : smsservicetype;
                String query = "username=" + HttpUtility.UrlEncode(username) +
                "&password=" + HttpUtility.UrlEncode(password) +
                "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +
                "&content=" + HttpUtility.UrlEncode(Message) +
                "&bulkmobno=" + HttpUtility.UrlEncode(Mobile) +
                "&senderid=" + HttpUtility.UrlEncode(senderid) +
                "&key=" + HttpUtility.UrlEncode(secureKey);

                byte[] byteArray = Encoding.ASCII.GetBytes(query);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;

                dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                String Status = ((HttpWebResponse)response).StatusDescription;
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
                return responseFromServer;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public static bool IsUnicode(string input)
        {
            const int MaxAnsiCode = 255;
            return input.ToCharArray().Any(c => c > MaxAnsiCode) || input.Contains("@") || input.Contains("|"); // @ and | is not sending in simple sms
        }
        public static string GetUnicodeMessage(string input)
        {
            string finalmsg = string.Empty;
            foreach (char c in input)
                finalmsg += "&#" + (int)c + ";";
            return finalmsg;
        }
        public static string DataTableToJSONString(DataTable dt)
        {
            //return DataTableToJSONWithJSONNet(dt);
            if (dt == null)
                return "[]"; // {\"Message\":\"Data table is null.\"} // Change By Jitendra on 22-03-2018 ** Only 0 count No Message for Message rendering through JS **

            if (dt.Rows.Count < 1)
                return "[]"; //  {\"Message\":\"Data table have no record(s).\"} // Change By Jitendra on 22-03-2018 ** Only 0 count No Message for Message rendering through JS **

            StringBuilder JSONString = new StringBuilder();
            int RowsCount = dt.Rows.Count, ColCount = dt.Columns.Count;

            JSONString.Append("[");
            for (int i = 0; i < RowsCount; i++)
            {
                JSONString.Append("{");
                for (int j = 0; j < ColCount; j++)
                {
                    if (j < ColCount - 1)
                        JSONString.Append("\"" + System.Text.RegularExpressions.Regex.Replace(dt.Columns[j].ColumnName.ToString(), @"\t|\n|\r", "") + "\":" + "\"" + System.Text.RegularExpressions.Regex.Replace(dt.Rows[i][j].ToString(), @"\t|\n|\r", "") + "\",");
                    else if (j == ColCount - 1)
                        JSONString.Append("\"" + System.Text.RegularExpressions.Regex.Replace(dt.Columns[j].ColumnName.ToString(), @"\t|\n|\r", "") + "\":" + "\"" + System.Text.RegularExpressions.Regex.Replace(dt.Rows[i][j].ToString(), @"\t|\n|\r", "") + "\"");
                }
                if (i == RowsCount - 1)
                    JSONString.Append("}");
                else
                    JSONString.Append("},");
            }
            JSONString.Append("]");

            return JSONString.ToString();
        }
        public static string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }

        public static string DataTableToJSONWithJSONNet(DataSet table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }

        public static void ssoSignout(string userid)
        {
            RAJSSO.SSO SSO = new RAJSSO.SSO();
            SSO.SSOSignout(userid);
        }

        public static string GetIpAddress()  // Get IP Address
        {
            return GetIp();
        }


        public static string GetLocalIPAddress()
        {
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            return System.Environment.MachineName;


        }

        public static string GetIPAddress1()
        {
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            string iPAddress = "";
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    iPAddress = Convert.ToString(IP);
                }
            }
            return iPAddress;
        }


        public static string GetLocalHostName()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            return host.HostName;
        }

        public static string GetIp()
        {
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }

        public static string GetCompCode()  // Get Computer Name
        {
            string strHostName = "";
            strHostName = Dns.GetHostName();
            return strHostName;
        }
        public static string ImageToBase64(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                return Convert.ToBase64String(ms.ToArray());
            }
        }
        public static string Encrypt(string Text)
        {
            string Key = "681D392DB4FD7B8C6562712DFEEF1";
            string initVector = "tu89geji340t89u2";
            int keysize = 256;

            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(Text);
            PasswordDeriveBytes password = new PasswordDeriveBytes(Key, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] Encrypted = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(Encrypted);
        }
        public static string Decrypt(string EncryptedText)
        {
            string Key = "681D392DB4FD7B8C6562712DFEEF1";
            string initVector = "tu89geji340t89u2";
            int keysize = 256;
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] DeEncryptedText = Convert.FromBase64String(EncryptedText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(Key, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(DeEncryptedText);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[DeEncryptedText.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }
        public static string SSO_AADHAR_OTP_VALIDATION(string SSOID)
        {
            //SSOWSSoap.SSOUserDetail usrdtl = new SSOWSSoap.SSOUserDetail();
            //SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
            //usrdtl = ssoclient.GetUserDetailXML(SSOID, "RAJSAHKAR.TEST", "Rajasthan@12");
            string _txnid = string.Empty;
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser != null)
            {
                _txnid = SendOtpByAadharNo(_LoggedInUser.AadharNo);

            }
            return _txnid;
        }



        public static string GetEncryptedPIDXmlNew(string pidXml, string certPath, string ts, ref string certExpiryDate, ref string encSessionKey, ref string encHmac)
        {
            ENCRYPTER enc = new ENCRYPTER(certPath);
            byte[] pidXmlBytes = Encoding.UTF8.GetBytes(pidXml);
            byte[] session_key = enc.generateSessionKey();
            byte[] encrypted_skey = enc.encryptUsingPublicKey(session_key);
            byte[] encPidXmlBytes = enc.encryptUsingSessionKeyNew(session_key, pidXmlBytes, ts, false);
            byte[] hmac = enc.generateSha256Hash(pidXmlBytes);
            byte[] encHmacBytes = enc.encryptUsingSessionKeyNew(session_key, hmac, ts, true);

            encSessionKey = Convert.ToBase64String(encrypted_skey);
            encHmac = Convert.ToBase64String(encHmacBytes);
            certExpiryDate = enc.getCertificateIdentifier();
            return Convert.ToBase64String(encPidXmlBytes);
        }
        public static DataTable ConvertListToDataTable(List<string[]> list)
        {
            // New table.
            DataTable table = new DataTable();
            // Get max columns.
            int columns = 0;
            foreach (var array in list)
            {
                if (array.Length > columns)
                {
                    columns = array.Length;
                }
            }
            // Add columns.
            for (int i = 0; i < columns; i++)
            {
                table.Columns.Add();
            }
            // Add rows.
            foreach (var array in list)
            {
                table.Rows.Add(array);
            }
            return table;
        }

        public static string GetUniqueFileName(string name, string savePath, string ext)
        {
            name = name.Replace(ext, "").Replace(" ", "_");
            name = System.Text.RegularExpressions.Regex.Replace(name, @"[^\w\s]", "");

            var newName = name;
            var i = 0;
            if (System.IO.File.Exists(savePath + newName + ext))
            {
                do
                {
                    i++;
                    newName = name + "_" + i;

                }
                while (System.IO.File.Exists(savePath + newName + ext));
            }
            return newName;
        }

        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string fn_OTP_EKYC(string strUID, string strOtp)
        {
            string CertificatePath = "~/Content/";
            string Certificatename = "uidai_auth_prod.cer";
            //generate session key
            AesCryptoServiceProvider crypto = new AesCryptoServiceProvider();
            crypto.KeySize = 256;
            crypto.GenerateKey();
            byte[] skey = crypto.Key;

            //read certificate
            CertificatePath = CertificatePath + Certificatename;
            X509Certificate2 certificate = getPublicKey2(CertificatePath);
            RSACryptoServiceProvider rsa = certificate.PublicKey.Key as RSACryptoServiceProvider;
            byte[] encSessionKey = rsa.Encrypt(skey, false);

            string certExpiryDate = certificate.GetExpirationDateString();
            DateTime MyDateTime = new DateTime();
            MyDateTime = Convert.ToDateTime(certExpiryDate);
            string expiryDate = MyDateTime.ToString("yyyyMMdd");

            //Creating PID XML and Generate in UTF-8 Byte of that XML.
            var tts = DateTime.UtcNow.ToString("yyyy-MM-ddTHH\\:mm\\:ss");

            string msg = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Pid ts='" + tts + "' ver='1.0'><Pv otp='" + strOtp + "' /></Pid>";
            byte[] message = Encoding.UTF8.GetBytes(msg);

            //Encrypt PID block by normal session key.
            byte[] encXMLPIDData = EncryptDatabyte(message, skey);

            //Generating hmac by SHA-256 Algo
            byte[] hmac = GetSHA256(message);
            byte[] encryptedHmacBytes = EncryptDatabyte(hmac, skey);

            //string authXML = "<?xml version=\"1.0\" encoding=\"utf-8\"?><authrequest uid='" + strUID + "' tid='public' subaua='STGDOIT006' ip='NA' fdc='NA' idc='NA' udc='15dfs13I846' macadd='78-2B-CB-8D-93-F1' lot='P' lov='110002' lk='MFoFMLF3estsEWOpcX1GwastH6eyU0MbYAgT9e7Q3q6pQRG171lmOuY' rc='Y' ts='" + tts + "'><Skey ci='" + expiryDate + "'>" + Convert.ToBase64String(encSessionKey) + "</Skey><Hmac>" + Convert.ToBase64String(encryptedHmacBytes) + "</Hmac><Data type='X'>" + Convert.ToBase64String(encXMLPIDData) + "</Data></authrequest>";
            string authXML = "<?xml version=\"1.0\" encoding=\"utf-8\"?><authrequest uid='" + strUID + "' tid='public' subaua='STGDOIT026' ip='NA' fdc='NA' idc='NA' udc='RAJSSOAUTH-OTP' macadd='00-50-56-A6-BC-1E' lot='P' lov='302005' lk='MOe2N9kGlDEde9gMtZB59ZjJZpEcVSKkLxRP7ZFUjd4_ozuLn3bCVow' rc='Y' ts='" + tts + "'><Skey ci='" + expiryDate + "'>" + Convert.ToBase64String(encSessionKey) + "</Skey><Hmac>" + Convert.ToBase64String(encryptedHmacBytes) + "</Hmac><Data type='X'>" + Convert.ToBase64String(encXMLPIDData) + "</Data></authrequest>";

            string response = postXMLData("http://10.68.251.97/api/aua/kyc/bio_otp/encr", authXML);
            return response;
        }
        public static X509Certificate2 getPublicKey2(string pth)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            X509Certificate2 cert2 = new X509Certificate2(pth);
            return cert2;
        }
        public static string postXMLData(string destinationUrl, string requestXml)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
            byte[] bytes;
            bytes = System.Text.Encoding.ASCII.GetBytes(requestXml);
            request.ContentType = "Application/xml";
            request.ContentLength = bytes.Length;
            request.Method = "POST";
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                string responseStr = new StreamReader(responseStream).ReadToEnd();
                return responseStr;
            }
            return null;
        }
        public static byte[] EncryptDatabyte(byte[] toEncryptArray, byte[] sessionKey)
        {
            AesCryptoServiceProvider AES = new AesCryptoServiceProvider();
            AES.Key = sessionKey;
            AES.Mode = CipherMode.ECB;
            AES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = AES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return resultArray;
        }
        private static byte[] GetSHA256(byte[] text)
        {
            byte[] hashValue;
            SHA256Managed hashString = new SHA256Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(text);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hashValue;
        }

        static string ComputeSha256Hash(byte[] text)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(text);

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }


        #region ESign
        public static string GenerateOTPForEsingh(string hdn_aadhar)
        {
            if (HttpContext.Current.Request.Url.Host.Contains("localhost") || HttpContext.Current.Request.Url.Host.Contains("devitsandbox"))
            {
                //hdn_aadhar = "9887356163";
                hdn_aadhar = "409032098170";
            }

            //hdn_aadhar = "428434778643";
            string errormsg = string.Empty;
            string succmsg = string.Empty;
            string json = string.Empty;
            string ss = string.Empty;
            string _txnid = "";
            string status = "";
            string responseCode = "";
            if (hdn_aadhar.ToString().Trim().ToString() != "")
            {
                string url = GlobalConstant.SendOTPForAadharAuthEsignURL + "?client_id=" + GlobalConstant.AadharAuthEsignClientId;



                json = "{\"aadharid\":\"" + hdn_aadhar.ToString() + "\", \"departmentname\" : \"Raj Sahakar\"}";
                try
                {
                    ss = WebRequestinJson(url, json, "application/json");
                    //Sp_callTrackingInsert("Response GenerateOTPForEsingh(string hdn_aadhar):- ", ss.ToString());
                    JObject root = (JObject)JObject.Parse(ss);
                    foreach (var item in root)
                    {
                        if (item.Key == "Status")
                            status = item.Value.ToString().Replace("\"", "");
                        if (item.Key == "TransactionId")
                            _txnid = item.Value.ToString().Replace("\"", "");

                        if (item.Key == "UidaiErrorMsg")
                            errormsg += " " + item.Value.ToString().Replace("\"", "");
                        if (item.Key == "ErrorMessage")
                            errormsg += " " + item.Value.ToString().Replace("\"", "");
                        if (item.Key == "ResponseCode")
                            responseCode = item.Value.ToString().Replace("\"", "");
                    }
                    if (!status.Equals("1"))
                    {
                        //errormsg = "status - " + status;
                        //_txnid = "NO" + "#Response from Aadhar Server :- hdn_aadhar=" + hdn_aadhar + ", " + errormsg;
                        if (errormsg.Contains("Problem"))
                        {
                            //errormsg = "status - " + status;
                            succmsg = "You have exceeded limit for OTP attempts or resends, Please try in 10 minutes";
                            _txnid = succmsg;
                        }
                        else if (responseCode == "ES-998")
                        {
                            succmsg = "Please enter valid Aadhaar Id";
                            _txnid = succmsg;
                        }
                        else
                        {
                            _txnid = errormsg;
                        }
                        _txnid = "NO" + "#" + errormsg;
                    }
                    else if (status.Equals("1"))
                    {
                        _txnid.Replace("\"", "");

                    }
                }
                catch (Exception ex)
                {

                    _txnid = "NO" + "#" + ex.Message;
                }
            }
            else
            {
                errormsg = "Fill Aadhar ID!";
            }

            string logid = new csCommon().InsertAadhaarServiceLogs(hdn_aadhar, "GENTERATEOTPForEsign", json, ss, _txnid, status, responseCode, "");
            return _txnid;

        }


        //public static string GenerateOTPForEsingh1(string hdn_aadhar)
        //{
        //    hdn_aadhar = "420928155572";
        //    if (HttpContext.Current.Request.Url.Host.Contains("localhost") || HttpContext.Current.Request.Url.Host.Contains("devitsandbox"))
        //    {
        //        //hdn_aadhar = "9887356163";
        //        hdn_aadhar = "420928155572";
        //    }

        //    //hdn_aadhar = "428434778643";
        //    string errormsg = string.Empty;
        //    string succmsg = string.Empty;
        //    string _txnid = "";
        //    string ResponseCode = "";
        //    if (hdn_aadhar.ToString().Trim().ToString() != "")
        //    {
        //        var SSOID = System.Web.HttpContext.Current.Session.SessionID; //((UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin]);

        //        //string url = GlobalConstant.SendOTPForAadharAuthEsignURL + "?client_id=" + GlobalConstant.AadharAuthEsignClientId;
        //        string url = GlobalConstant.SendOTPForAadharAuthEsignURL_Test;

        //        string status = "";
        //        string AppCode = "PRAVL22868ABFAP021";
        //        string SSOToken = SSOID;
        //        Random rnd = new Random();
        //        var iRnd = rnd.Next(10000000, 99999999);
        //        string RequestId = "PRAVL22868" + iRnd.ToString();
        //        string ReturnURL = HttpContext.Current.Request.Url.ToString();
        //        string Checksum = CreateMD5(RequestId);
        //        //string encAadharNo = EncryptWithKey(hdn_aadhar.ToString(), GlobalConstant.AadharAuthEsigEncKey_Test, GlobalConstant.AadharAuthEsigAppCode_Test);

        //        string json1 = "{\"aadharid\":\"" + hdn_aadhar.ToString() + "\", \"departmentname\" : \"Raj Sahakar\"}";

        //        //string json = "{\"SSOToken\": \""+ SSOToken +"\",\"AppCode\": \""+AppCode+"\",\"RequestId\": \""+RequestId +"\",\"AadhaarNo\": \""+ hdn_aadhar.ToString() + "\",\"RequestType\": \"3\",\"ReturnURL\": \"<ReturnURL>\",\"Checksum\": \"<Checksum >\",\"VerificationType\": \"<VerificationType >\",\"OperatorIdType\": \"<OperatorIdType >\",\"OperatorId\": \"<OperatorId >\",\"Purpose\": \"<Purpose >\",}";

        //        //string json = "{\"SSOToken\": \"" + SSOToken + "\",\"AppCode\": \"" + AppCode + "\",\"RequestId\": \"" + RequestId + "\",\"AadhaarNo\": \"" + hdn_aadhar.ToString() + "\",\"RequestType\": \"3\",\"ReturnURL\": \""+ReturnURL+ "\",\"Purpose\": \"Send OTP For Esign Authentication.\",}";


        //        string json = "{\"SSOToken\":\"" + SSOToken + "\",\"AppCode\":\"" + AppCode + "\",\"RequestId\":\"" + RequestId + "\",\"AadhaarNo\":\"" + hdn_aadhar.ToString() + "\",\"ReturnURL\":\"" + ReturnURL + "\",\"Purpose\":\"Send OTP For Esign Authentication.\",\"RequestType\":3,\"Checksum\":\"" + Checksum + "\",\"VerificationType\":0,\"OperatorIdType\":0,\"OperatorId\":\"2\"}";


        //        string encData = EsignAesEncryption.Encrypt(json, GlobalConstant.AadharAuthEsigEncKey_Test);
        //        //var bytes = System.Text.Encoding.UTF8.GetBytes(encData);
        //        try
        //        {
        //            string ss = WebRequestinJson(url, encData, "application/json");
        //            Sp_callTrackingInsert("Response GenerateOTPForEsingh(string GenerateOTPForEsingh1):- ", ss.ToString());
        //            JObject root = (JObject)JObject.Parse(ss);
        //            foreach (var item in root)
        //            {
        //                if (item.Key == "Status")
        //                    status = item.Value.ToString().Replace("\"", ""); ;
        //                if (item.Key == "TransactionId")
        //                    _txnid = item.Value.ToString().Replace("\"", ""); ;



        //                if (item.Key == "RequestId")
        //                    _txnid = item.Value.ToString().Replace("\"", ""); ;
        //                if (item.Key == "ResponseCode")
        //                    status = item.Value.ToString().Replace("\"", ""); ;




        //                if (item.Key == "UidaiErrorMsg")
        //                    errormsg += " " + item.Value.ToString().Replace("\"", ""); ;
        //                if (item.Key == "ErrorMessage")
        //                    errormsg += " " + item.Value.ToString().Replace("\"", ""); ;



        //            }
        //            if (!status.Equals("1") || !ResponseCode.Equals("200"))
        //            {
        //                //errormsg = "status - " + status;
        //                _txnid = "NO" + "#Response from Aadhar Server :- " + errormsg;
        //            }
        //            else if (status.Equals("1") || ResponseCode.Equals("200"))
        //            {
        //                _txnid.Replace("\"", "");

        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            _txnid = "NO" + "#" + ex.Message;
        //        }
        //    }
        //    else
        //    {
        //        errormsg = "Fill Aadhar ID!";
        //    }
        //    return _txnid;

        //}
        public static ResultMessage VerifyAadhaarOTP(string OTP, string _txnid)
        {
            ResultMessage result = new ResultMessage();

            if (_txnid.Contains("PRAVL2286"))
            {
                result = CommonMessageFactory.GetJsonResponse("1", _txnid);

                return result;
            }

            string responseCode = "";
            string message = string.Empty;
            string status = "";
            string TransactionId = "";
            string succmsg = string.Empty;
            string json = string.Empty;
            string ss = string.Empty;



            if (_txnid != "")
            {
                try
                {
                    string url = GlobalConstant.VerifyOTPForAadharAuthEsignURL + "?client_id=" + GlobalConstant.AadharAuthEsignClientId;
                    json = "{\"otp\":\"" + OTP + "\",\"transactionid\":\"" + _txnid + "\"}";
                    ss = WebRequestinJson(url, json, "application/json");

                    //Sp_callTrackingInsert("Response VerifyAadhaarOTP(string OTP, string _txnid)(:- ", ss.ToString());
                    JObject root = (JObject)JObject.Parse(ss);
                    foreach (var item in root)
                    {
                        if (item.Key.ToLower() == "status")
                            status = item.Value.ToString().Replace("\"", "");
                        if (item.Key.ToLower() == "transactionid")
                            TransactionId = item.Value.ToString().Replace("\"", "");

                    }
                    if (status == "1")
                    {
                        result = CommonMessageFactory.GetJsonResponse("1", TransactionId);
                    }
                    else
                    {
                        result = CommonMessageFactory.GetJsonResponse("0", "Invalid OTP!");

                    }
                    result.responce = ss;
                }
                catch (Exception ex)
                {
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.CommonFunctions", FunctionName = " VerifyAadhaarOTP(string OTP, string _txnid)" }, ex);
                    //result = "Error";
                    result = CommonMessageFactory.GetJsonResponse("0", ex.Message);
                }
            }

            string logid = new csCommon().InsertAadhaarServiceLogs(_txnid, "Verifyesign", json, ss, _txnid, status, responseCode, "");

            return result;
        }

        public static string ConvertToA4InPlace_old(string filePath)
        {
            try
            {
                // Define a temporary file path
                string tempPath = Path.GetTempFileName();

                // Open the original PDF and create a new PDF with A4 pages
                using (PdfReader reader = new PdfReader(filePath))
                using (FileStream fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
                using (Document document = new Document(PageSize.A4))
                using (PdfWriter writer = PdfWriter.GetInstance(document, fs))
                {
                    document.Open();

                    // Loop through each page of the original PDF
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        document.NewPage();

                        // Get the page from the original PDF
                        PdfImportedPage page = writer.GetImportedPage(reader, i);

                        // Check if the page size matches A4
                        Rectangle pageSize = reader.GetPageSizeWithRotation(i);
                        if (pageSize.Width != PageSize.A4.Width || pageSize.Height != PageSize.A4.Height)
                        {
                            // Calculate scaling factors to fit within A4 dimensions
                            float scaleX = PageSize.A4.Width / pageSize.Width;
                            float scaleY = PageSize.A4.Height / pageSize.Height;
                            float scale = Math.Min(scaleX, scaleY);

                            // Apply scaling and center the content on A4 page
                            PdfContentByte content = writer.DirectContent;
                            float offsetX = (PageSize.A4.Width - pageSize.Width * scale) / 2;
                            float offsetY = (PageSize.A4.Height - pageSize.Height * scale) / 2;
                            content.AddTemplate(page, scale, 0, 0, scale, offsetX, offsetY);
                        }
                        else
                        {
                            // If already A4, add the page as is
                            PdfContentByte content = writer.DirectContent;
                            content.AddTemplate(page, 0, 0);
                        }
                    }

                    document.Close();
                }

                // Overwrite the original file with the modified content
                System.IO.File.Delete(filePath);
                System.IO.File.Move(tempPath, filePath);
                return filePath;
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "CommonFunctions#" + ex.Message, FunctionName = "ConvertToA4InPlace(string filePath)" }, ex);
                return filePath;
            }

        }

        public static string ConvertToA4InPlace(string filePath)
        {
            try
            {
                // Define a temporary file path
                string tempPath = Path.GetTempFileName();

                // Open the original PDF and create a new PDF with A4 pages
                using (PdfReader reader = new PdfReader(filePath))
                using (FileStream fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
                using (Document document = new Document(PageSize.A4))
                using (PdfWriter writer = PdfWriter.GetInstance(document, fs))
                {
                    document.Open();

                    // Loop through each page of the original PDF
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        document.NewPage();

                        // Get the page from the original PDF
                        PdfImportedPage page = writer.GetImportedPage(reader, i);

                        // Check if the page size matches A4
                        Rectangle pageSize = reader.GetPageSizeWithRotation(i);
                        if (pageSize.Width != PageSize.A4.Width || pageSize.Height != PageSize.A4.Height)
                        {

                            // Calculate scaling factors
                            float scaleX = PageSize.A4.Width / pageSize.Width;
                            float scaleY = PageSize.A4.Height / pageSize.Height;
                            float scale = Math.Min(scaleX, scaleY);

                            PdfContentByte content = writer.DirectContent;

                            // Determine offsets for centering
                            float offsetX = (PageSize.A4.Width - pageSize.Width * scale) / 2;
                            float offsetY = (PageSize.A4.Height - pageSize.Height * scale) / 2;

                            // Handle page rotation
                            int rotation = reader.GetPageRotation(i);
                            switch (rotation)
                            {
                                case 90:
                                    content.AddTemplate(page, 0, -scale, scale, 0, offsetX, PageSize.A4.Height - offsetY);
                                    break;
                                case 180:
                                    content.AddTemplate(page, -scale, 0, 0, -scale, PageSize.A4.Width - offsetX, PageSize.A4.Height - offsetY);
                                    break;
                                case 270:
                                    content.AddTemplate(page, 0, scale, -scale, 0, PageSize.A4.Width - offsetX, offsetY);
                                    break;
                                default:
                                    content.AddTemplate(page, scale, 0, 0, scale, offsetX, offsetY);
                                    break;
                            }
                        }
                        else
                        {
                            // If already A4, add the page as is
                            PdfContentByte content = writer.DirectContent;
                            content.AddTemplate(page, 0, 0);
                        }
                    }

                    document.Close();
                }

                // Overwrite the original file with the modified content
                System.IO.File.Delete(filePath);
                System.IO.File.Move(tempPath, filePath);
                return filePath;
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "CommonFunctions#" + ex.Message, FunctionName = "ConvertToA4InPlace(string filePath)" + filePath }, ex);
                return filePath;
            }
        }

        public static ResultMessage EsinghPDF(string _txnid, string SocietyIDEncOrFilePath, string Location = "", string Designation = "", string llx = "300", string lly = "-100", string positionX = "", string positionY = "")
        {
            string message = string.Empty;
            string final_status = "";
            string final_document = "";
            string succmsg = string.Empty;
            //string pdfPath = "";
            string openFile = "";
            string CurrentJson = string.Empty;

            ResultMessage result = new ResultMessage();

            if (_txnid != "")
            {
                try
                {
                    ApplicationFlowController OBJAFC = new ApplicationFlowController();
                    SocietyIDEncOrFilePath = SocietyIDEncOrFilePath.Replace("\\", "/");
                    if (SocietyIDEncOrFilePath == "")
                    {
                        result = CommonMessageFactory.GetJsonResponse("0", "Don't have any file for esign");
                        return result;
                    }

                    foreach (string file in SocietyIDEncOrFilePath.Split('#'))
                    {

                        if (System.IO.File.Exists(file))
                        {
                            string ConvertedFile = ConvertToA4InPlace(file);
                        }
                        else
                        {
                            result = CommonMessageFactory.GetJsonResponse("0", "Final Status Failed -" + final_status);
                            succmsg = "Final Status Failed -" + final_status;
                            return result;
                        }
                        // string ConvertedFile = ConvertToA4InPlace(file);  

                        string filename = file.Split('/')[file.Split('/').Length - 1];

                        byte[] pdfBytes = System.IO.File.ReadAllBytes(file);
                        string pdfBase64 = Convert.ToBase64String(pdfBytes);
                        string url2 = GlobalConstant.EsignPDFURL + "?client_id=" + GlobalConstant.AadharAuthEsignClientId;

                        string json2 = "";
                        json2 = "{\"inputJson\":{\"File\":\"" + pdfBase64 + "\"},\"transactionid\":\"" + _txnid + "\",\"docname\":\"" + filename + "\",\"designation\": \"" + Designation + "\",\"status\":\"" + (Designation != "" ? "Approved" : "SelfAttested") + "\",\"llx\":\"" + llx + "\",\"lly\":\"" + lly + "\",\"positionX\":\"" + positionX + "\",\"positionY\":\"" + positionY + "\",\"location\":\"" + Location + "\",\"mode\":\"2\"}";
                        CurrentJson = json2;
                        string ss2 = WebRequestinJson(url2, json2, "application/json");
                        //Sp_callTrackingInsert("Response DataTable EsinghPDF(:- ", ss2.ToString());
                        JObject root2 = (JObject)JObject.Parse(ss2);
                        Task.Run(() => { Sp_callTrackingInsert("Response DataTable EsinghPDF(:- " + file, root2.ToString()); });
                        foreach (var item in root2)
                        {
                            if (item.Key.ToLower() == "status")
                                final_status = item.Value.ToString();
                            if (item.Key.ToLower() == "document")
                            {
                                final_document = item.Value.ToString().Replace("\"", "");
                                byte[] sPDFDecoded = Convert.FromBase64String(final_document);
                                FileStream stream = new FileStream(file, FileMode.Create);
                                BinaryWriter writer = new BinaryWriter(stream);
                                writer.Write(sPDFDecoded, 0, sPDFDecoded.Length);
                                writer.Close();

                                //FileInfo fi = new FileInfo(pdfPath);
                                //if (fi.Exists)
                                //{
                                //    fi.CopyTo(pdfPath, true);
                                //}
                                openFile = "esign";
                                result = CommonMessageFactory.GetJsonResponse("1", "Success");
                            }

                        }
                        /*{ "Status": "0", "ResponseCode": "AE-251", "ErrorMessage": "Some Exception Occured Please try again", "Transaction Id": "b3f89178-f9f1-45d5-83c9-85ea4d2352f1" }*/
                        //if (final_status != "1" && !openFile.Equals("esign")) /*In error case document not find*/
                        if (final_status != "1" || !openFile.Equals("esign"))
                        {
                            result = CommonMessageFactory.GetJsonResponse("0", "Final Status Failed -" + final_status);
                            succmsg = "Final Status Failed -" + final_status;
                            return result;
                        }

                    }
                }
                catch (Exception ex)
                {
                    ILogger objILogger = new DBLogger();
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController#" + ex.Message, FunctionName = " EsinghPDF( string '" + _txnid + "', string '" + SocietyIDEncOrFilePath + "',string '" + Location + "',string '" + Designation + "', string '" + llx + "', string '" + lly + "', string '" + positionX + "' string '" + positionY + "=)" }, ex);
                    //objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController#", FunctionName = CurrentJson }, ex);
                    result = CommonMessageFactory.GetJsonResponse("0", ex.Message);
                    succmsg = ex.Message;
                }
            }
            return result;
        }

        public static ResultMessage EsinghPDF_Test(string _txnid, string SocietyIDEncOrFilePath, string Location = "", string Designation = "", string llx = "300", string lly = "-100", string positionX = "", string positionY = "")
        {
            string message = string.Empty;
            string final_status = "";
            string final_document = "";
            string succmsg = string.Empty;
            //string pdfPath = "";
            string openFile = "";
            string CurrentJson = string.Empty;

            ResultMessage result = new ResultMessage();

            if (_txnid != "")
            {
                try
                {
                    ApplicationFlowController OBJAFC = new ApplicationFlowController();

                    SocietyIDEncOrFilePath = SocietyIDEncOrFilePath.Replace("\\", "/");
                    foreach (string file in SocietyIDEncOrFilePath.Split('#'))
                    {
                        string filename = file.Split('/')[file.Split('/').Length - 1];
                        byte[] pdfBytes = System.IO.File.ReadAllBytes(file);
                        string pdfBase64 = Convert.ToBase64String(pdfBytes);
                        string clientid = "76b3522f-117a-4797-b7fe-342e40ee9f29";
                        // string url2 = GlobalConstant.EsignPDFURL_Test + "?client_id=" + GlobalConstant.AadharAuthEsignClientId;
                        string url2 = GlobalConstant.EsignPDFURL_Test + "?client_id=" + clientid;

                        string json2 = "";
                        json2 = "{\"inputJson\":{\"File\":\"" + pdfBase64 + "\"},\"transactionid\":\"" + _txnid + "\",\"docname\":\"" + filename + "\",\"designation\": \"" + Designation + "\",\"status\":\"" + (Designation != "" ? "Approved" : "SelfAttested") + "\",\"llx\":\"" + llx + "\",\"lly\":\"" + lly + "\",\"positionX\":\"" + positionX + "\",\"positionY\":\"" + positionY + "\",\"mode\":\"3\"}";
                        CurrentJson = json2;
                        string ss2 = WebRequestinJson(url2, json2, "application/json");
                        Sp_callTrackingInsert("Response DataTable EsinghPDFTest(:- ", ss2.ToString());
                        JObject root2 = (JObject)JObject.Parse(ss2);
                        foreach (var item in root2)
                        {
                            if (item.Key.ToLower() == "status")
                                final_status = item.Value.ToString();
                            if (item.Key.ToLower() == "document")
                            {
                                final_document = item.Value.ToString().Replace("\"", "");
                                byte[] sPDFDecoded = Convert.FromBase64String(final_document);
                                FileStream stream = new FileStream(file, FileMode.Create);
                                BinaryWriter writer = new BinaryWriter(stream);
                                writer.Write(sPDFDecoded, 0, sPDFDecoded.Length);
                                writer.Close();

                                //FileInfo fi = new FileInfo(pdfPath);
                                //if (fi.Exists)
                                //{
                                //    fi.CopyTo(pdfPath, true);
                                //}
                                openFile = "esign";
                                result = CommonMessageFactory.GetJsonResponse("1", "Success");

                            }
                        }
                        if (final_status != "1" && !openFile.Equals("esign"))
                        {
                            result = CommonMessageFactory.GetJsonResponse("0", "Final Status Failed -" + final_status);
                            succmsg = "Final Status Failed -" + final_status;
                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ILogger objILogger = new DBLogger();
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController#" + ex.Message, FunctionName = " EsinghPDF_Test( string '" + _txnid + "', string '" + SocietyIDEncOrFilePath + "',string '" + Location + "',string '" + Designation + "', string '" + llx + "', string '" + lly + "', string '" + positionX + "' string '" + positionY + "=)" }, ex);
                    //objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController#", FunctionName = CurrentJson }, ex);
                    result = CommonMessageFactory.GetJsonResponse("0", ex.Message);
                    succmsg = ex.Message;
                }
            }
            return result;
        }

        public static ResultMessage EsignPDF_NEW(string _txnid, string SocietyIDEncOrFilePath, string Location = "", string Designation = "", string llx = "300", string lly = "-100", string positionX = "", string positionY = "")
        {
            string message = string.Empty;
            string final_status = "";
            string final_document = "";
            string succmsg = string.Empty;
            //string pdfPath = "";
            string openFile = "";
            string CurrentJson = string.Empty;
            int step = 1;
            ResultMessage result = new ResultMessage();
            if (_txnid != "")
            {
                try
                {
                    // SocietyIDEncOrFilePath=HttpContext.Current.Server.MapPath(SocietyIDEncOrFilePath);
                    ApplicationFlowController OBJAFC = new ApplicationFlowController();
                    SocietyIDEncOrFilePath = SocietyIDEncOrFilePath.Replace("\\", "/");
                    if (SocietyIDEncOrFilePath == "")
                    {
                        result = CommonMessageFactory.GetJsonResponse("0", "Don't have any file for esign");
                        return result;
                    }
                    foreach (string file in SocietyIDEncOrFilePath.Split('#'))
                    {
                        if (System.IO.File.Exists(file))
                        {
                            string ConvertedFile = ConvertToA4InPlace(file);
                        }
                        else
                        {
                            Sp_callTrackingInsert("Response DataTable EsinghPDF(:- ", "file not exists" + file);
                            result = CommonMessageFactory.GetJsonResponse("0", "Final Status Failed -" + final_status);
                            succmsg = "Final Status Failed -" + final_status;
                            return result;
                        }
                        string filename = file.Split('/')[file.Split('/').Length - 1];


                        byte[] pdfBytes = System.IO.File.ReadAllBytes(file);
                        string pdfBase64 = Convert.ToBase64String(pdfBytes);
                        //string url2 = GlobalConstant.EsignPDFURL_Test + "?client_id=" + GlobalConstant.AadharAuthEsignClientId;
                        string url2 = GlobalConstant.EsignPDFURL + "?client_id=" + GlobalConstant.AadharAuthEsignClientId;
                        step = 6;
                        //string url2 = GlobalConstant.EsignPDFURL + "?client_id=" + GlobalConstant.AadharAuthEsignClientId;

                        if (Location != " ")
                            Designation += " (" + Location + ")";

                        string json2 = "";
                        //    json2 = "{\"inputJson\":{\"File\":\"" + pdfBase64 + "\"},\"transactionid\":\"" + _txnid + "\",\"docname\":\"" + filename + "\",\"designation\": \"" + Designation + "\",\"status\":\"" + (Designation != "" ? "Approved" : "SelfAttested") + "\",\"llx\":\"" + llx + "\",\"lly\":\"" + lly + "\",\"positionX\":\"" + positionX + "\",\"positionY\":\"" + positionY + "\",\"location\":\"" + Location + "\",\"mode\":\"2\"}";
                        //json2 = "{\"inputJson\":{\"File\":\"" + pdfBase64 + "\"},\"transactionid\":\"" + _txnid + "\",\"docname\":\"" + filename + "\",\"designation\": \"" + Designation + "\",\"status\":\"" + (Designation != "" ? "Approved" : "SelfAttested") + "\",\"llx\":\"" + llx + "\",\"lly\":\"" + lly + "\",\"positionX\":\"" + positionX + "\",\"positionY\":\"" + positionY + "\",\"filetype\":\"" + "PDF" + "\",\"mode\":\"2\"}";
                        json2 = "{\"inputJson\":{\"File\":\"" + pdfBase64 + "\"},\"transactionid\":\"" + _txnid + "\",\"docname\":\"" + filename + "\",\"designation\": \"" + Designation + "\",\"status\":\"" + (Designation != "" ? "Approved" : "SelfAttested") + "\",\"llx\":\"" + llx + "\",\"lly\":\"" + lly + "\",\"positionX\":\"" + positionX + "\",\"positionY\":\"" + positionY + "\",\"mode\":\"2\"}";

                        CurrentJson = json2;
                        string ss2 = WebRequestinJson(url2, json2, "application/json");

                        // Sp_callTrackingInsert("Response DataTable EsinghPDFNew(:- ", ss2.ToString());
                        JObject root2 = (JObject)JObject.Parse(ss2);
                        Sp_callTrackingInsert("Response DataTable EsinghPDFNew(:- " + filename + ": ", root2.ToString());
                        foreach (var item in root2)
                        {
                            if (item.Key.ToLower() == "status")
                                final_status = item.Value.ToString();
                            if (item.Key.ToLower() == "document")
                            {
                                final_document = item.Value.ToString().Replace("\"", "");
                                byte[] sPDFDecoded = Convert.FromBase64String(final_document);
                                FileStream stream = new FileStream(file, FileMode.Create);
                                BinaryWriter writer = new BinaryWriter(stream);
                                writer.Write(sPDFDecoded, 0, sPDFDecoded.Length);
                                writer.Close();
                                //FileInfo fi = new FileInfo(pdfPath);
                                //if (fi.Exists)
                                //{
                                //    fi.CopyTo(pdfPath, true);
                                //}
                                openFile = "esign";
                                result = CommonMessageFactory.GetJsonResponse("1", "Success");
                            }
                        }
                        if (final_status != "1" && !openFile.Equals("esign"))
                        {
                            result = CommonMessageFactory.GetJsonResponse("0", "Final Status Failed -" + final_status);
                            succmsg = "Final Status Failed -" + final_status;
                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ILogger objILogger = new DBLogger();
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController#" + ex.Message, FunctionName = "EsignPDF_new step" + step + "EsinghPDF( string '" + _txnid + "', string '" + SocietyIDEncOrFilePath + "',string '" + Location + "',string '" + Designation + "', string '" + llx + "', string '" + lly + "', string '" + positionX + "' string '" + positionY + ")" }, ex);
                    result = CommonMessageFactory.GetJsonResponse("0", ex.Message);
                    succmsg = ex.Message;
                }
            }
            return result;
        }


        //2026-02-12 By Pradeep
        public static ResultMessage EsignPDF_GCC(string _txnid, string filePath, string Location = "", string Designation = "", int pageNumber = 0, string llx = "300", string lly = "-100", string positionX = "", string positionY = "")
        {
            string message = string.Empty;
            string final_status = "";
            string final_document = "";
            string succmsg = string.Empty;
            string openFile = "";
            string CurrentJson = string.Empty;
            int step = 1;
            ResultMessage result = new ResultMessage();
            if (_txnid != "")
            {
                try
                {
                    filePath = filePath.Replace("\\", "/");
                    if (filePath == "")
                    {
                        result = CommonMessageFactory.GetJsonResponse("0", "Don't have any file for esign");
                        return result;
                    }
                    foreach (string file in filePath.Split('#'))
                    {

                        if (File.Exists(file))
                        {
                            string filename = file.Split('/')[file.Split('/').Length - 1];

                            byte[] pdfBytes = File.ReadAllBytes(file);
                            string pdfBase64 = Convert.ToBase64String(pdfBytes);
                            string url2 = GlobalConstant.EsignPDFURL + "?client_id=" + GlobalConstant.AadharAuthEsignClientId;
                            step = 6;

                            if (Location != " ")
                                Designation += " (" + Location + ")";

                            //var jsonModel = new RootEsignModel
                            //{
                            //    inputJson = new InputEsignJson
                            //    {
                            //        File = pdfBase64
                            //    },
                            //    filetype = "PDF",
                            //    transactionid = _txnid,
                            //    docname = filename,
                            //    designation = "",
                            //    status = string.IsNullOrEmpty(Designation) ? "SelfAttested" : "Approved",
                            //    llx = llx,
                            //    lly = lly,
                            //    positionX = "",
                            //    positionY = "",
                            //    mode = "2",
                            //    page = pageNumber == 0 ? "" : pageNumber.ToString()
                            //};

                            string json2 = "";
                            if (pageNumber <= 0)
                                json2 = "{\"inputJson\":{\"File\":\"" + pdfBase64 + "\"},\"transactionid\":\"" + _txnid + "\",\"docname\":\"" + filename + "\",\"designation\": \"" + Designation + "\",\"status\":\"" + (Designation != "" ? "Approved" : "SelfAttested") + "\",\"llx\":\"" + llx + "\",\"lly\":\"" + lly + "\",\"positionX\":\"" + positionX + "\",\"positionY\":\"" + positionY + "\",\"mode\":\"2\"}";
                            else
                                json2 = "{\"inputJson\":{\"File\":\"" + pdfBase64 + "\"},\"transactionid\":\"" + _txnid + "\",\"docname\":\"" + filename + "\",\"designation\": \"" + Designation + "\",\"status\":\"" + (Designation != "" ? "Approved" : "SelfAttested") + "\",\"llx\":\"" + llx + "\",\"lly\":\"" + lly + "\",\"positionX\":\"\",\"positionY\":\"\", \"mode\":\"4\",\"page\":\"" + pageNumber + "\"}";


                            //CurrentJson = JsonConvert.SerializeObject(jsonModel); 
                            //string ss2 = WebRequestinJson(url2, JsonConvert.SerializeObject(jsonModel), "application/json");
                            string ss2 = WebRequestinJson(url2, json2, "application/json");

                            JObject root2 = (JObject)JObject.Parse(ss2);
                            Sp_callTrackingInsert("Response DataTable EsignPDF_GCC(:- " + filename + ": ", root2.ToString() + "json2=" + json2);
                            foreach (var item in root2)
                            {
                                if (item.Key.ToLower() == "status")
                                    final_status = item.Value.ToString();
                                if (item.Key.ToLower() == "document")
                                {
                                    final_document = item.Value.ToString().Replace("\"", "");
                                    byte[] sPDFDecoded = Convert.FromBase64String(final_document);
                                    using (FileStream stream = new FileStream(file, FileMode.Create))
                                    {
                                        using (BinaryWriter writer = new BinaryWriter(stream))
                                        {
                                            writer.Write(sPDFDecoded, 0, sPDFDecoded.Length);
                                            writer.Close();
                                            openFile = "esign";
                                        }
                                    }
                                    result = CommonMessageFactory.GetJsonResponse("1", "Success");
                                }
                            }
                            if (final_status != "1" && !openFile.Equals("esign"))
                            {
                                result = CommonMessageFactory.GetJsonResponse("0", "Final Status Failed -" + final_status);
                                succmsg = "Final Status Failed -" + final_status;
                                return result;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ILogger objILogger = new DBLogger();
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "CommonFinction" + ex.Message, FunctionName = "EsignPDF_GCC step" + step + "EsignPDF_GCC( string '" + _txnid + "', string '" + filePath + "',string '" + Location + "',string '" + Designation + "', string '" + llx + "', string '" + lly + "', string '" + positionX + "' string '" + positionY + ")" }, ex);
                    result = CommonMessageFactory.GetJsonResponse("0", ex.Message);
                    succmsg = ex.Message;
                }
            }
            return result;

        }

        public static string WebRequestinJson(string url, string postData, string contenttype)
        {
            try
            {
                string ret = string.Empty;

                StreamWriter requestWriter;

                var webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
                if (webRequest != null)
                {
                    webRequest.Method = "POST";
                    webRequest.ServicePoint.Expect100Continue = false;
                    webRequest.Timeout = 40000;

                    webRequest.ContentType = contenttype;
                    //POST the data.
                    using (requestWriter = new StreamWriter(webRequest.GetRequestStream()))
                    {
                        requestWriter.Write(postData);
                    }
                }

                HttpWebResponse resp = (HttpWebResponse)webRequest.GetResponse();
                Stream resStream = resp.GetResponseStream();
                StreamReader reader = new StreamReader(resStream);
                ret = reader.ReadToEnd();
                return ret;
            }
            catch (WebException wex)
            {
                throw wex;

            }


        }

        public static string JamaBandi(string _DistrictCode, string _TehsilCode, string _VillageCode, string _Khasra)
        {
            _DistrictCode = "130";
            _TehsilCode = "00688";
            _VillageCode = "106118";
            _Khasra = "1";

            string url = "https://api.sewadwaar.rajasthan.gov.in/app/live/Emitra/Prod/JamaBandi/Post/ROR?client_id=02a79073-c2db-4c84-b810-885fdaa8c54a";
            url = "https://api.sewadwaar.rajasthan.gov.in/app/live/Emitra/Prod/JamaBandi/Post/VillSummary?client_id=02a79073-c2db-4c84-b810-885fdaa8c54a";

            string XMLDATA = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?><authrequest villagecode='" + _VillageCode + "' districtcode='" + _DistrictCode + "'  tehsilcode='" + _TehsilCode + "' khasra='" + _Khasra + "' username='ws_eseva'  password='ws_eseva#789'></authrequest>";
            WebRequest req = null;
            WebResponse rsp = null;
            try
            {
                if (!string.IsNullOrEmpty(url))
                {
                    req = WebRequest.Create(url);
                    req.Method = "POST";
                    req.ContentType = "application/xml";
                    StreamWriter writer = new StreamWriter(req.GetRequestStream());
                    writer.Write(XMLDATA);
                    writer.Close();
                    rsp = req.GetResponse(); //I am getting error over here
                    StreamReader sr = new StreamReader(rsp.GetResponseStream());
                    string results = sr.ReadToEnd();
                    sr.Close();


                }
            }
            catch (Exception ex)
            {


            }
            return "";
        }
        #endregion




        public static String SHA512Generator(string value)
        {
            byte[] genkey = Encoding.UTF8.GetBytes(value);
            //static byte[] pwd = new byte[encPwd.Length];
            HashAlgorithm sha1 = HashAlgorithm.Create("SHA512");
            byte[] sec_key = sha1.ComputeHash(genkey);
            StringBuilder sb1 = new StringBuilder();
            for (int i = 0; i < sec_key.Length; i++)
            {
                sb1.Append(sec_key[i].ToString("x2"));
            }
            return sb1.ToString();
        }


        public static BRNEntity GetBRN(string BRN)
        {

            //string url = "https://br.raj.nic.in/brapi/api/brservices/GetBRNDetailJSON"; //old
            string url = "https://sanapi.rajasthan.gov.in/api/sanservices/GetBRNDetailJSON";
            BRNEntity result = new BRNEntity();
            try
            {
                if (!string.IsNullOrEmpty(url))
                {
                    string sBRNPassword, sEncPwd, sPwdSalt, sEncPwdSalt, sBase64;
                    int iRnd;

                    sBRNPassword = "Coop$3667";
                    Random rnd = new Random();
                    iRnd = rnd.Next(10000000, 99999999);
                    sEncPwd = SHA512Generator(sBRNPassword);
                    sPwdSalt = iRnd.ToString() + sEncPwd;
                    sEncPwdSalt = SHA512Generator(sPwdSalt);
                    sBase64 = GetBase64Encode(sEncPwdSalt);
                    using (WebClient Client = new WebClient())
                    {
                        try
                        {
                            string data = JsonConvert.SerializeObject(new
                            {
                                user_id = "rajcoop",
                                BRN = BRN,
                                salt = iRnd.ToString(),
                                user_pwd = sBase64
                            });

                            //Encode form string to bytes
                            var bytes = System.Text.Encoding.UTF8.GetBytes(data);

                            var http = (HttpWebRequest)WebRequest.Create(new Uri(url));

                            http.Method = "POST";
                            http.ContentType = "application/json";

                            // string parsedContent = <<PUT HERE YOUR JSON PARSED CONTENT>>;
                            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                            //Byte[] bytes = encoding.GetBytes(jsonString);

                            Stream newStream = http.GetRequestStream();
                            newStream.Write(bytes, 0, bytes.Length);
                            newStream.Close();

                            var response = http.GetResponse();

                            var stream = response.GetResponseStream();
                            var sr = new StreamReader(stream);
                            var content = sr.ReadToEnd();
                            // result = JsonConvert.DeserializeObject<BRNStatus>(content.ToString());

                            result = JsonConvert.DeserializeObject<BRNEntity>(JsonConvert.DeserializeObject<object>(content.ToString()).ToString());

                            //Response.Write(content.ToString());
                        }
                        catch (Exception ex)
                        {
                            ILogger objILogger = new DBLogger();
                            objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SOCIETY", FunctionName = "When Convert JSON To BRNEntity" }, ex);
                            return new BRNEntity();
                        }
                    }



                }
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SOCIETY", FunctionName = "When Convert JSON To BRNEntity" }, ex);
                return new BRNEntity();

            }
            return result;
        }
        public static string CreateMD5(string sStr)
        {
            try
            {
                // Use input string to calculate MD5 hash
                using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
                {
                    byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(sStr);
                    byte[] hashBytes = md5.ComputeHash(inputBytes);
                    // Convert the byte array to hexadecimal string
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        sb.Append(hashBytes[i].ToString("X2"));
                    }
                    return sb.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string GetBase64Encode(string sStr)
        {
            var sBytes = Encoding.UTF8.GetBytes(sStr);
            return System.Convert.ToBase64String(sBytes);
        }
        public static bool GeneratePDF1(System.Text.StringBuilder HtmlString, string filepath, string PageOriantation = "")
        {
            Document pdfDoc = new Document(PageOriantation == "" ? iTextSharp.text.PageSize.A4 : iTextSharp.text.PageSize.LETTER_LANDSCAPE, 5, 5, 5, 5);

            if (PageOriantation == "")
            {
                pdfDoc = new Document(iTextSharp.text.PageSize.A4, 5, 5, 5, 5);
            }
            else
            {
                pdfDoc = new Document(iTextSharp.text.PageSize.A4.Rotate());
            }

            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(filepath, FileMode.Create));
            try
            {

                pdfDoc.Open();
                var tagProcessors = (DefaultTagProcessorFactory)Tags.GetHtmlTagProcessorFactory();
                tagProcessors.RemoveProcessor(HTML.Tag.IMG);
                tagProcessors.AddProcessor(HTML.Tag.IMG, new CustomImageTagProcessor());

                // Set up font provider to include Kruti Dev font
                var fontPath = HttpContext.Current.Server.MapPath(@"~\fonts\Kruti-Dev-010\K010.ttf");
                if (!System.IO.File.Exists(fontPath))
                {
                    throw new FileNotFoundException("Font file not found at specified path: " + fontPath);
                }
                var fontProvider = new XMLWorkerFontProvider(XMLWorkerFontProvider.DONTLOOKFORFONTS);
                fontProvider.Register(fontPath, "KrutiDev");

                var cssAppliers = new CssAppliersImpl(fontProvider);
                var context = new HtmlPipelineContext(cssAppliers);
                context.SetAcceptUnknown(true).AutoBookmark(true).SetTagFactory(tagProcessors);

                var cssFiles = new CssFilesImpl();
                cssFiles.Add(XMLWorkerHelper.GetInstance().GetDefaultCSS());
                var cssResolver = new StyleAttrCSSResolver(cssFiles);



                var htmlPipeline = new HtmlPipeline(context, new PdfWriterPipeline(pdfDoc, writer));
                var cssPipeline = new CssResolverPipeline(cssResolver, htmlPipeline);
                var worker = new XMLWorker(cssPipeline, true);
                var charset = Encoding.UTF8;
                var xmlParser = new XMLParser(true, worker, charset);

                using (var sr = new StringReader(HtmlString.ToString()))
                {
                    xmlParser.Parse(sr);
                    pdfDoc.Close();
                    writer.Close();
                }
                return true;
            }
            catch (System.Exception ex)
            {
                pdfDoc.Close();
                writer.Close();
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.COMMON_FUNCTION", FunctionName = " GeneratePDF1(System.Text.StringBuilder HtmlString, string filepath)" }, ex);
                return false;
            }
        }
        public static bool GeneratePDF(System.Text.StringBuilder HtmlString, string filepath, string PageOriantation = "")
        {
            Document pdfDoc = new Document(PageOriantation == "" ? iTextSharp.text.PageSize.A4 : iTextSharp.text.PageSize.LETTER_LANDSCAPE, 5, 5, 5, 5);

            if (PageOriantation == "")
            {
                pdfDoc = new Document(iTextSharp.text.PageSize.A4, 5, 5, 5, 5);
            }
            else
            {
                pdfDoc = new Document(iTextSharp.text.PageSize.A4.Rotate());
            }

            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(filepath, FileMode.Create));
            try
            {

                pdfDoc.Open();
                var tagProcessors = (DefaultTagProcessorFactory)Tags.GetHtmlTagProcessorFactory();
                tagProcessors.RemoveProcessor(HTML.Tag.IMG);
                tagProcessors.AddProcessor(HTML.Tag.IMG, new CustomImageTagProcessor());

                var cssFiles = new CssFilesImpl();
                cssFiles.Add(XMLWorkerHelper.GetInstance().GetDefaultCSS());
                var cssResolver = new StyleAttrCSSResolver(cssFiles);

                var charset = System.Text.Encoding.UTF8;
                var context = new HtmlPipelineContext(new CssAppliersImpl(new XMLWorkerFontProvider()));
                context.SetAcceptUnknown(true).AutoBookmark(true).SetTagFactory(tagProcessors);
                var htmlPipeline = new HtmlPipeline(context, new PdfWriterPipeline(pdfDoc, writer));
                var cssPipeline = new CssResolverPipeline(cssResolver, htmlPipeline);
                var worker = new XMLWorker(cssPipeline, true);
                var xmlParser = new XMLParser(true, worker, charset);

                using (var sr = new StringReader(HtmlString.ToString()))
                {
                    xmlParser.Parse(sr);
                    pdfDoc.Close();
                    writer.Close();
                }
                return true;
            }
            catch (System.Exception ex)
            {
                pdfDoc.Close();
                writer.Close();
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.COMMON_FUNCTION", FunctionName = " GeneratePDF(System.Text.StringBuilder HtmlString, string filepath)" + HtmlString }, ex);
                return false;
            }
        }

        public static Byte[] GePDFBytes(System.Text.StringBuilder HtmlString, string PageType)
        {
            System.Byte[] bytes;
            try
            {

                using (var ms = new MemoryStream())
                {
                    using (var doc = new Document(PageType == "Landscape" ? iTextSharp.text.PageSize.A4_LANDSCAPE : iTextSharp.text.PageSize.A4, 5, 5, 5, 5))
                    {

                        //Create a writer that's bound to our PDF abstraction and our stream
                        using (var writer = PdfWriter.GetInstance(doc, ms))
                        {

                            //Open the document for writing
                            doc.Open();
                            // string path = HttpContext.Current.Server.MapPath("~/fonts/Kruti-Dev-010/k010.TTF");
                            //Our sample HTML and CSS
                            var example_html = HtmlString.ToString();
                            //var example_css = @" @font-face {  font-family: 'HindiFont'; src: url('" + path + "'); } .hindiFont { font-family: 'HindiFont'; }";

                            // using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_css)))
                            // {
                            using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_html)))
                            {
                                // iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, msCss, System.Text.Encoding.UTF8, new UnicodeFontFactory());
                                iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, null, System.Text.Encoding.UTF8);
                            }
                            //}
                            doc.Close();
                        }
                    }

                    bytes = ms.ToArray();

                }
                //System.IO.File.WriteAllBytes(filepath, bytes);
                return bytes;
            }
            catch (System.Exception ex)
            {

                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.COMMON_FUNCTION", FunctionName = " GeneratePDF(System.Text.StringBuilder HtmlString, string filepath, string PageType)" }, ex);
                return null;
            }

        }
        public static void MergePDFs(string outPutFilePath, params string[] filesPath)
        {
            List<PdfReader> readerList = new List<PdfReader>();
            foreach (string filePath in filesPath)
            {
                PdfReader pdfReader = new PdfReader(filePath);
                readerList.Add(pdfReader);
            }

            //Define a new output document and its size, type
            Document document = new Document(PageSize.A4, 0, 0, 0, 0);
            //Create blank output pdf file and get the stream to write on it.
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(outPutFilePath, FileMode.Create));
            document.Open();

            foreach (PdfReader reader in readerList)
            {
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    PdfImportedPage page = writer.GetImportedPage(reader, i);
                    document.Add(iTextSharp.text.Image.GetInstance(page));
                }
            }
            document.Close();
        }
        //public static void CombineMultiplePDFs(string[] fileNames, string outFile)
        //{
        //    // step 1: creation of a document-object
        //    Document document = new Document();
        //    //create newFileStream object which will be disposed at the end
        //    using (FileStream newFileStream = new FileStream(outFile, FileMode.Create))
        //    {
        //        // step 2: we create a writer that listens to the document
        //        PdfCopy writer = new PdfCopy(document, newFileStream);
        //        if (writer == null)
        //        {
        //            return;
        //        }

        //        // step 3: we open the document
        //        document.Open();

        //        foreach (string fileName in fileNames)
        //        {
        //            // we create a reader for a certain document
        //            PdfReader reader = new PdfReader(fileName);
        //            reader.ConsolidateNamedDestinations();

        //            // step 4: we add content
        //            for (int i = 1; i <= reader.NumberOfPages; i++)
        //            {
        //                PdfImportedPage page = writer.GetImportedPage(reader, i);
        //                writer.AddPage(page);
        //            }

        //            PRAcroForm form = reader.AcroForm;

        //            if (form != null)
        //            {
        //                writer.CopyAcroForm(reader);
        //            }

        //            reader.Close();
        //        }

        //        // step 5: we close the document and writer
        //        writer.Close();
        //        document.Close();
        //    }//disposes the newFileStream object
        //}




        public static DataTable ReadExcelData(string ExcelPath, string SheetName = "Sheet1")
        {
            DataTable resultTable = new DataTable();
            string conString = string.Empty;
            string extension = Path.GetExtension(ExcelPath).ToLower();
            if (extension.Trim() == ".xls")
            {
                conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ExcelPath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
            }
            else if (extension.Trim() == ".xlsx")
            {
                conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ExcelPath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
            }
            conString = string.Format(conString, ExcelPath);
            using (OleDbConnection excel_con = new OleDbConnection(conString))
            {
                try
                {
                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + SheetName + "$]", excel_con))
                    {
                        oda.Fill(resultTable);
                    }
                    excel_con.Close();
                    return resultTable;
                }
                catch (Exception ex)
                {
                    excel_con.Close();
                    throw new Exception("\"" + SheetName + "\" sheet name does not exists in upload Excel. OR Error-" + ex.Message.Replace("'", ""));
                }
            }

        }

        private static bool IsDigitFamily(string val)
        {
            int valid = 0;
            if (!int.TryParse(val, out valid)) //|| !double.TryParse(val, out double valid1) || !float.TryParse(val, out float valid2) || !decimal.TryParse(val, out decimal valid3)
                return false;

            return true;
        }

        public static string WhereClauseByParams(List<Params> parameters)
        {
            string whereCase = " ";

            if (parameters.Count > 0)
            {
                foreach (var item in parameters)
                {
                    if (!string.IsNullOrEmpty(item.Key) && !string.IsNullOrEmpty(item.Value))
                    {
                        if (IsDigitFamily(item.Value) && Convert.ToDecimal(item.Value) != default(decimal))
                        {
                            whereCase += " AND " + item.Key + " = " + item.Value;
                        }
                        else if (!IsDigitFamily(item.Value))
                        {
                            whereCase += " AND " + item.Key + " like %" + item.Value + "% ";
                        }
                    }
                }
            }

            return whereCase;
        }

        public static string PDFResize(string OriginalPdfPath)
        {
            string Result = "-1";
            try
            {
                string inPDF = OriginalPdfPath;
                PdfReader pdfr = new PdfReader(inPDF);
                //Document doc = new Document(PageSize.A4, 0, 0, 0, 0);
                //Document.Compress = true;

                //PdfWriter writer = PdfWriter.GetInstance(doc,new FileStream("E:\\NASEWWWW.pdf", FileMode.Create));
                //doc.Open();               
                //PdfContentByte cb = writer.DirectContent;               
                //PdfImportedPage page;

                //for (int i = 1; i < pdfr.NumberOfPages + 1; i++)
                //{
                //    page = writer.GetImportedPage(pdfr, i);                   
                //    cb.AddTemplate(page, PageSize.A7.Width / pdfr.GetPageSize(i).Width, 0, 0, PageSize.A4.Height / pdfr.GetPageSize(i).Height, 0, 0);                   
                //    doc.NewPage();
                //}

                //doc.Close();

                //Result = "1";

                var doc = new Document();
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream("E:\\RAJEEV.pdf", FileMode.Create));
                writer.CompressionLevel = PdfStream.BEST_COMPRESSION;
                doc.Open();
                PdfContentByte cb = writer.DirectContent;
                PdfImportedPage page;

                for (int i = 1; i < pdfr.NumberOfPages + 1; i++)
                {
                    page = writer.GetImportedPage(pdfr, i);

                    cb.AddTemplate(page, PageSize.A4.Width / pdfr.GetPageSize(i).Width, 0, 0, PageSize.A4.Height / pdfr.GetPageSize(i).Height, 0, 0);
                    doc.NewPage();
                }

                doc.Close();

            }
            catch (Exception ex)
            {
                Result = ex.Message;
            }
            return Result;
        }
        public static void DoTask()
        {
            var api = new LovePdfApi("PUBLIC_KEY", "SECRET_KEY");

            var task = api.CreateTask<CompressTask>();

            //add file, and specify rotation
            var file = task.AddFile("/path/to/document.pdf", task.TaskId, Rotate.Degrees90);

            //set compress parameters and process files
            var time = task.Process(new CompressParams
            {
                CompressionLevel = CompressionLevels.Extreme,
                OutputFileName = "extreme_compression"
            });

            //download output file(s) to specific directory
            task.DownloadFile("path/to/download");
        }
        public static string posttopage(string URL, string userdetails, string AppName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");
            sb.AppendFormat(@"<body style='background-color:#F0F0F0;' onload='document.forms[""form""].submit()'>");
            sb.AppendFormat("<form name='form' action='{0}' method='post'>", URL);
            sb.AppendFormat("<div style='float:left; width:100%; height:100%;'>");
            sb.AppendFormat("<div style='float:left; width:100%; height:100%; margin-top:10%;'>	");
            sb.AppendFormat("<div style='float:left; width:100%; text-align:center; font-size:30px; color:#525252; margin:0 0 50px 0;'>Please wait while you are being redirected to <span style='font-weight:bold;'>{0}</span> Application.</div>", AppName.ToUpper());
            sb.AppendFormat("<div style='float:left; width:100%; text-align:center;'>");
            sb.AppendFormat("<img src='/images/loading.gif'  width='350px'/>");
            sb.AppendFormat("</div>");
            sb.AppendFormat("<input type='hidden' name='userdetails' value='{0}'>", userdetails);
            sb.AppendFormat("</div>");
            sb.AppendFormat("<div>");
            sb.Append("</form>");
            sb.Append("</body>");
            sb.Append("</html>");
            return sb.ToString();
        }

        public static void UpdateSWSStatus(string RegistrationNo, int Statuscode)
        {
            try
            {


                //if (HttpContext.Current.Request.Url.Host.Contains("localhost") || HttpContext.Current.Request.Url.Host.Contains("http://10.68.252.116"))
                //{
                SWSService.STATUSUPDATESoapClient obj = new SWSService.STATUSUPDATESoapClient("STATUSUPDATESoap");
                var result = obj.statusupdate(28, 1, RegistrationNo, Statuscode, "", "", "", "1");
                //}
                //else
                //{
                //   SWCLive.STATUSUPDATESoapClient obj = new SWCLive.STATUSUPDATESoapClient("STATUSUPDATESoap1");
                //   var result = obj.statusupdate(28, 1, RegistrationNo, Statuscode, "", "", "", "1");
                //}


            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "UpdateSWSStatus(" + RegistrationNo + ", " + Statuscode + ")" }, ex);
            }
        }

        public static void UpdateSWSPartialSave(string RegistrationNo, int Statuscode)
        {
            try
            {
                string data = System.Web.HttpContext.Current.Session[SessionIdentity.SWCSData].ToString();// JsonConvert.SerializeObject(System.Web.HttpContext.Current.Session[SessionIdentity.SWCSData]) ;
                SWSEntity objSWC = JsonConvert.DeserializeObject<SWSEntity>(data);

                // if (HttpContext.Current.Request.Url.Host.Contains("localhost") || HttpContext.Current.Request.Url.Host.Contains("http://10.68.252.116"))
                // {
                SWSService.STATUSUPDATESoapClient obj = new SWSService.STATUSUPDATESoapClient("STATUSUPDATESoap");
                var result = obj.AppIdUpdate(Convert.ToInt32(objSWC.SWSID), RegistrationNo, Statuscode);
                // }
                // else {
                //     SWCLive.STATUSUPDATESoapClient obj = new SWCLive.STATUSUPDATESoapClient("STATUSUPDATESoap1");
                //     var result = obj.AppIdUpdate(Convert.ToInt32(objSWC.SWSID), RegistrationNo, Statuscode);
                // }



            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "UpdateSWSPartialSave(" + RegistrationNo + ", " + Statuscode + ")" }, ex);
            }
        }


        public static string SendOtpByAadharNo(string _AadharNo)
        {
            if (HttpContext.Current.Request.Url.Host.Contains("localhost"))
            {
                _AadharNo = "432534469557";
                return "DOITRJGOVAUTH202201211037006324723";
            }
            string _txnid = string.Empty;
            string UID = string.Empty;
            string ip = GetIpAddress();
            String timeStamp = DateTime.Now.ToString();

            string auacode = GlobalConstant.AadharAuthSUBAUS;
            string lickey = GlobalConstant.AadharAuthLicenseKey;
            string client_id = GlobalConstant.AadharAuthClientId;

            string XMLDATA = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?><authrequest uid='" + _AadharNo + "' subaua='" + auacode + "'  ip='" + ip + "' fdc='NA' idc='NA' bt='otp' macadd='78-2B-CB-8D-93-F1' lot='P' lov='110002' lk='" + lickey + "' rc='Y' ver='2.5'><otp/></authrequest>";


            XMLDATA.Replace('\'', '\"');
            string url = GlobalConstant.SendOTPForAadharAuthURL + "?client_id=" + client_id;

            System.Net.WebRequest req = null;
            WebResponse rsp = null;
            try
            {
                if (!string.IsNullOrEmpty(_AadharNo) && _AadharNo.Length == 12)
                {
                    req = WebRequest.Create(url);
                    //req.Proxy = WebProxy.GetDefaultProxy(); // Enable if using proxy
                    req.Method = "POST";        // Post method
                    req.ContentType = "application/xml";
                    req.Headers["appname"] = "CO-Operative";
                    // content type
                    // Wrap the request stream with a text-based writer
                    StreamWriter writer = new StreamWriter(req.GetRequestStream());
                    // Write the XML text into the stream
                    writer.Write(XMLDATA);
                    writer.Close();
                    // Send the data to the webserver
                    rsp = req.GetResponse(); //I am getting error over here                  
                                             // Sp_callTrackingInsert("Response SendOtpByAadharNo(string _AadharNo1):- ", rsp.ToString());
                    StreamReader sr = new StreamReader(rsp.GetResponseStream());
                    string results = sr.ReadToEnd();
                    sr.Close();
                    //Sp_callTrackingInsert("Response SendOtpByAadharNo(string _AadharNo2):- ", results);

                    // UpdateIdentifyMessage(results, null);
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(results); // suppose that myXmlString contains "<Names>...</Names>"
                    XmlNodeList xnList = xml.SelectNodes("authresponse/auth");
                    string sa = "";
                    string Errormsg = "";
                    foreach (XmlNode xn in xnList)
                    {
                        sa = xn.Attributes["status"].Value;
                        if (sa.ToUpper() == "Y")
                            _txnid = xn.Attributes["txn"].Value;
                        else
                        {
                            _txnid = xml.InnerText;
                            //_txnid = "Aadhar Service temporarily down Please Try After Some Time";
                        }

                    }
                    if (sa.ToUpper() == "N")
                    {

                        _txnid = "NO" + "#" + _txnid;
                    }
                }
                else
                {
                    _txnid = "NO" + "#" + "AADHAR ID NOT UPDATED IN SSO PROFILE";
                }



            }
            catch (Exception ex)
            {
                _txnid = "NO" + "#SOME ERROR OCCURD IN AADHAAR SERVER. PLEASE TRY AFTER SOME TIME #" + ex.Message;
                //_txnid = "DOITRJGOVAUTH" + "";
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "static string SendOtpByAadharNo(string " + _AadharNo + ")" }, ex);
            }
            return _txnid;
        }

        public static DataTable ValidateOTP(string txn, string adhar, string otp)
        {

            DataTable dtb = new DataTable();
            try
            {


                string auacode = GlobalConstant.AadharAuthSUBAUS;
                string lickey = GlobalConstant.AadharAuthLicenseKey;
                string client_id = GlobalConstant.AadharAuthClientId;

                string Certificatename = "uidai_auth_prod.cer";

                string url = GlobalConstant.VerifyOTPForAadharAuthURL + "?client_id=" + client_id;//new

                System.Web.HttpContext context = System.Web.HttpContext.Current;
                //string CertificatePath = context.Server.MapPath(Certificatename);
                string pathString = "~/Content/";
                string CertificatePath = System.Web.HttpContext.Current.Server.MapPath(pathString + Certificatename);

                //Creating PID XML and Generate in UTF-8 Byte of that XML.
                var tts = DateTime.UtcNow.ToString("yyyy-MM-ddTHH\\:mm\\:ss");
                string msg = "<Pid ts=\"" + tts + "\" ver=\"2.0\" wadh=\"DEL9Vn2tNcxY/T3g9Jf6F/0Ne43ufdO6AaBClCWkCiQ=\"><Pv otp=\"" + otp + "\" /></Pid>";

                string expiryDate = string.Empty;
                string encSessionKey = string.Empty;
                string encryptedHmacBytes = string.Empty;
                string encXMLPIDDataNew = GetEncryptedPIDXmlNew(msg, CertificatePath, tts, ref expiryDate, ref encSessionKey, ref encryptedHmacBytes);

                //V.2.5
                string authXML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><authrequest uid=\"" + adhar + "\" tid=\"\" subaua=\"" + auacode + "\" bt=\"OTP\" lk=\"" + lickey + "\" rc=\"Y\" ra=\"O\" lr=\"Y\" de=\"N\" pfr=\"N\" mec=\"Y\" ver=\"2.5\" txn=\"" + txn + "\" dpID=\"\" rdsID=\"\" rdsVer=\"\" dc=\"\" mi=\"\" mc=\"\" deviceSrNO=\"NA\" deviceError=\"NA\" ip=\"NA\" fdc=\"NA\" idc=\"NC\" macadd=\"NA\" lot=\"P\" lov=\"302005\" ><deviceInfo fType=\"NA\" iCount=\"NA\" pCount=\"NA\" errCodeRDS=\"NA\" errInfoRDS=\"NA\" fCount=\"NA\" nmPoints=\"NA\" qScore=\"NA\" srno=\"NA\" deviceError=\"NA\" /><Skey ci=\"" + expiryDate + "\">" + encSessionKey + "</Skey><Hmac>" + encryptedHmacBytes + "</Hmac><Data type=\"X\">" + encXMLPIDDataNew + "</Data></authrequest>";


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(authXML);
                request.ContentType = "Application/xml";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                request.Headers["appname"] = "CO-Operative";

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();

                //Sp_callTrackingInsert("Response ValidateOTP(string txn, string adhar, string otp):- ", response.ToString());
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    string results = new StreamReader(responseStream).ReadToEnd();
                    // UpdateIdentifyMessage(results, null);

                    //CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
                    //objCitizenModel.InsertAutharServeise(adhar, authXML, results);

                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(results); // suppose that myXmlString contains "<Names>...</Names>"
                    XmlNodeList xnList = xml.SelectNodes("//authresponse/auth");
                    string sa = "";
                    string txnid = "";
                    foreach (XmlNode xn in xnList)
                    {
                        sa = xn.Attributes["status"].Value;
                        //txnid = xn.Attributes["txn"].Value;

                    }
                    if (sa.ToUpper() == "Y")
                    {
                        string SSOIDORAADHAREnc = Encrypt(adhar);
                        List<string[]> listx = new List<string[]>();
                        XmlNodeList xDataList = xml.SelectNodes("//authresponse/UidData/pi");
                        XmlNodeList xDataList1 = xml.SelectNodes("//authresponse/UidData/pht");
                        XmlNodeList xDataList2 = xml.SelectNodes("//authresponse/UidData/LData");
                        XmlNodeList xDataList3 = xml.SelectNodes("//authresponse/UidData/pa");
                        string address = "";// "House : " + xDataList3[0].Attributes["house"].Value + ", LandMark : " + xDataList3[0].Attributes["lm"].Value + ", Street : " + xDataList3[0].Attributes["street"].Value + ", District: " + xDataList3[0].Attributes["dist"].Value + ", PIN: " + xDataList3[0].Attributes["pc"].Value + ", State: " + xDataList3[0].Attributes["state"].Value;

                        //<pa co="S/O Lakhan Singh" dist="Dhaulpur" house="" lm="" loc="old ricco jail road dholpur" ms="" pc="328001" state="Rajasthan" street="" vtc="dholpur" />

                        if (xDataList3[0].Attributes["co"].Value != "" && xDataList3[0].Attributes["co"].Value != null)
                        {
                            address += xDataList3[0].Attributes["co"].Value + ",";
                        }

                        if (xDataList3[0].Attributes["loc"].Value != "" && xDataList3[0].Attributes["loc"].Value != null)
                        {
                            address += xDataList3[0].Attributes["loc"].Value + ",";
                        }

                        if (xDataList3[0].Attributes["house"].Value != "" && xDataList3[0].Attributes["house"].Value != null)
                        {
                            address += xDataList3[0].Attributes["house"].Value + ",";
                        }
                        if (xDataList3[0].Attributes["lm"].Value != "" && xDataList3[0].Attributes["lm"].Value != null)
                        {
                            address += xDataList3[0].Attributes["lm"].Value + ",";
                        }
                        if (xDataList3[0].Attributes["street"].Value != "" && xDataList3[0].Attributes["street"].Value != null)
                        {
                            address += xDataList3[0].Attributes["street"].Value + ",";
                        }
                        if (xDataList3[0].Attributes["dist"].Value != "" && xDataList3[0].Attributes["dist"].Value != null)
                        {
                            address += xDataList3[0].Attributes["dist"].Value + ",";
                        }
                        if (xDataList3[0].Attributes["pc"].Value != "" && xDataList3[0].Attributes["pc"].Value != null)
                        {
                            address += xDataList3[0].Attributes["pc"].Value + ",";
                        }
                        if (xDataList3[0].Attributes["state"].Value != "" && xDataList3[0].Attributes["state"].Value != null)
                        {
                            address += xDataList3[0].Attributes["state"].Value + "";
                        }

                        string address2 = "House : " + xDataList3[0].Attributes["house"].Value + "#LandMark : " + xDataList3[0].Attributes["lm"].Value + "#Street : " + xDataList3[0].Attributes["street"].Value + "#District: " + xDataList3[0].Attributes["dist"].Value + "#PIN: " + xDataList3[0].Attributes["pc"].Value + "#State: " + xDataList3[0].Attributes["state"].Value;
                        listx.Add(new string[] { "Success", xDataList[0].Attributes["name"].Value, xDataList[0].Attributes["gender"].Value, xDataList[0].Attributes["dob"].Value, xDataList1[0].InnerText.ToString(), xDataList2[0].Attributes["name"].Value, SSOIDORAADHAREnc, xDataList3[0].Attributes["co"].Value, xDataList2[0].Attributes["co"].Value, address, address2 });

                        //listx.Add(new string[] { "Success", xDataList[0].Attributes["name"].Value, xDataList[0].Attributes["gender"].Value, xDataList[0].Attributes["dob"].Value, xDataList1[0].InnerText.ToString(), xDataList2[0].Attributes["name"].Value, SSOIDORAADHAREnc, xDataList3[0].Attributes["co"].Value.Split(':')[1], xDataList2[0].Attributes["co"].Value.Split(':')[1] });
                        dtb = ConvertListToDataTable(listx);
                    }
                    else
                    {
                        List<string[]> listx = new List<string[]>();
                        XmlNodeList xDataList = xml.SelectNodes("//authresponse/message");
                        listx.Add(new string[] { "Failed", xDataList[0].InnerText });
                        dtb = ConvertListToDataTable(listx);
                    }
                }
            }
            catch (Exception ex)
            {
                List<string[]> listx = new List<string[]>();
                listx.Add(new string[] { "Failed", ex.ToString() });
                dtb = ConvertListToDataTable(listx);
            }
            return dtb;
        }

        public static APIResponce ValidateAadharOTP(string txn, string adhar, string otp)
        {
            APIResponce apiResponce = new APIResponce();
            AadharResponce Responce = new AadharResponce();
            Responce.UserAddress = new AadharAddressResponce();
            Responce.UserInfo = new AadharUserInfoResponce();
            Responce.UserLocation = new AadharLocationResponce();
            try
            {


                string auacode = GlobalConstant.AadharAuthSUBAUS;
                string lickey = GlobalConstant.AadharAuthLicenseKey;
                string client_id = GlobalConstant.AadharAuthClientId;

                string Certificatename = "uidai_auth_prod.cer";

                string url = GlobalConstant.VerifyOTPForAadharAuthURL + "?client_id=" + client_id;//new

                System.Web.HttpContext context = System.Web.HttpContext.Current;
                //string CertificatePath = context.Server.MapPath(Certificatename);
                string pathString = "~/Content/";
                string CertificatePath = System.Web.HttpContext.Current.Server.MapPath(pathString + Certificatename);

                //Creating PID XML and Generate in UTF-8 Byte of that XML.
                var tts = DateTime.UtcNow.ToString("yyyy-MM-ddTHH\\:mm\\:ss");
                string msg = "<Pid ts=\"" + tts + "\" ver=\"2.0\" wadh=\"DEL9Vn2tNcxY/T3g9Jf6F/0Ne43ufdO6AaBClCWkCiQ=\"><Pv otp=\"" + otp + "\" /></Pid>";

                string expiryDate = string.Empty;
                string encSessionKey = string.Empty;
                string encryptedHmacBytes = string.Empty;
                string encXMLPIDDataNew = GetEncryptedPIDXmlNew(msg, CertificatePath, tts, ref expiryDate, ref encSessionKey, ref encryptedHmacBytes);

                //V.2.5
                string authXML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><authrequest uid=\"" + adhar + "\" tid=\"\" subaua=\"" + auacode + "\" bt=\"OTP\" lk=\"" + lickey + "\" rc=\"Y\" ra=\"O\" lr=\"Y\" de=\"N\" pfr=\"N\" mec=\"Y\" ver=\"2.5\" txn=\"" + txn + "\" dpID=\"\" rdsID=\"\" rdsVer=\"\" dc=\"\" mi=\"\" mc=\"\" deviceSrNO=\"NA\" deviceError=\"NA\" ip=\"NA\" fdc=\"NA\" idc=\"NC\" macadd=\"NA\" lot=\"P\" lov=\"302005\" ><deviceInfo fType=\"NA\" iCount=\"NA\" pCount=\"NA\" errCodeRDS=\"NA\" errInfoRDS=\"NA\" fCount=\"NA\" nmPoints=\"NA\" qScore=\"NA\" srno=\"NA\" deviceError=\"NA\" /><Skey ci=\"" + expiryDate + "\">" + encSessionKey + "</Skey><Hmac>" + encryptedHmacBytes + "</Hmac><Data type=\"X\">" + encXMLPIDDataNew + "</Data></authrequest>";


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(authXML);
                request.ContentType = "Application/xml";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                request.Headers["appname"] = "CO-Operative";

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();
                //Sp_callTrackingInsert("Response DataTable APIResponce ValidateAadharOTP(string txn, string adhar, string otp):- ", response.ToString());
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    string results = new StreamReader(responseStream).ReadToEnd();
                    // UpdateIdentifyMessage(results, null);

                    //CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
                    //objCitizenModel.InsertAutharServeise(adhar, authXML, results);

                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(results); // suppose that myXmlString contains "<Names>...</Names>"
                    XmlNodeList xnList = xml.SelectNodes("//authresponse/auth");
                    string sa = "";
                    string txnid = "";
                    foreach (XmlNode xn in xnList)
                    {
                        sa = xn.Attributes["status"].Value;
                        //txnid = xn.Attributes["txn"].Value;

                    }
                    if (sa.ToUpper() == "Y")
                    {
                        apiResponce.Status = "Success";
                        string SSOIDORAADHAREnc = Encrypt(adhar);
                        List<string[]> listx = new List<string[]>();
                        XmlNodeList xDataList = xml.SelectNodes("//authresponse/UidData/pi");
                        XmlNodeList xDataList1 = xml.SelectNodes("//authresponse/UidData/pht");
                        XmlNodeList xDataList2 = xml.SelectNodes("//authresponse/UidData/LData");
                        XmlNodeList xDataList3 = xml.SelectNodes("//authresponse/UidData/pa");
                        string address = "House : " + xDataList3[0].Attributes["house"].Value + ", LandMark : " + xDataList3[0].Attributes["lm"].Value + ", Street : " + xDataList3[0].Attributes["street"].Value + ", District: " + xDataList3[0].Attributes["dist"].Value + ", PIN: " + xDataList3[0].Attributes["pc"].Value + ", State: " + xDataList3[0].Attributes["state"].Value;
                        string address2 = "House : " + xDataList3[0].Attributes["house"].Value + "#LandMark : " + xDataList3[0].Attributes["lm"].Value + "#Street : " + xDataList3[0].Attributes["street"].Value + "#District: " + xDataList3[0].Attributes["dist"].Value + "#PIN: " + xDataList3[0].Attributes["pc"].Value + "#State: " + xDataList3[0].Attributes["state"].Value;
                        listx.Add(new string[] { "Success", xDataList[0].Attributes["name"].Value, xDataList[0].Attributes["gender"].Value, xDataList[0].Attributes["dob"].Value, xDataList1[0].InnerText.ToString(), xDataList2[0].Attributes["name"].Value, SSOIDORAADHAREnc, xDataList3[0].Attributes["co"].Value, xDataList2[0].Attributes["co"].Value, address, address2 });


                        Responce.UserInfo = new AadharUserInfoResponce() { name = xDataList[0].Attributes["name"].Value, gender = xDataList[0].Attributes["gender"].Value, photo = xDataList1[0].InnerText.ToString(), dob = xDataList[0].Attributes["dob"].Value, email = xDataList[0].Attributes["email"].Value, ms = xDataList[0].Attributes["ms"].Value, phone = xDataList[0].Attributes["phone"].Value };
                        Responce.UserAddress = new AadharAddressResponce() { dist = xDataList3[0].Attributes["dist"].Value, house = xDataList3[0].Attributes["house"].Value, lm = xDataList3[0].Attributes["lm"].Value, pc = xDataList3[0].Attributes["pc"].Value, state = xDataList3[0].Attributes["state"].Value, street = xDataList3[0].Attributes["street"].Value, ms = xDataList3[0].Attributes["ms"].Value, co = xDataList3[0].Attributes["co"].Value, loc = xDataList3[0].Attributes["loc"].Value, vtc = xDataList3[0].Attributes["vtc"].Value };
                        Responce.UserLocation = new AadharLocationResponce() { dist = xDataList2[0].Attributes["dist"].Value, house = xDataList2[0].Attributes["house"].Value, lm = xDataList2[0].Attributes["lm"].Value, pc = xDataList2[0].Attributes["pc"].Value, state = xDataList2[0].Attributes["state"].Value, street = xDataList2[0].Attributes["street"].Value, country = xDataList2[0].Attributes["country"].Value, co = xDataList2[0].Attributes["co"].Value, loc = xDataList2[0].Attributes["loc"].Value, vtc = xDataList2[0].Attributes["vtc"].Value, lang = xDataList2[0].Attributes["lang"].Value, name = xDataList2[0].Attributes["name"].Value, po = xDataList2[0].Attributes["po"].Value, subdist = xDataList2[0].Attributes["subdist"].Value };

                        apiResponce.Data = Responce;

                    }
                    else
                    {
                        XmlNodeList xDataList = xml.SelectNodes("//authresponse/message");
                        apiResponce.Status = "Failed";
                        apiResponce.Message = xDataList[0].InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                apiResponce.Status = "Failed";
                apiResponce.Message = ex.ToString();
            }
            return apiResponce;
        }

        public static string GetAadharFromDSMService(string ReferenceNo)
        {
            string _AadhaarNo = string.Empty;
            string subaua = GlobalConstant.AadharAuthSUBAUS;
            string client_id = GlobalConstant.AadharAuthEsignClientId;
            string url = GlobalConstant.AadhaarDSMDETOKANIZEV2URL + "?client_id=" + client_id;
            try
            {
                if (!string.IsNullOrEmpty(ReferenceNo) && ReferenceNo.Length == 15)
                {
                    string ip = GetIpAddress();
                    string ModifiedData = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><AuthRequest UUID=\"" + ReferenceNo + "\" subaua=\"" + subaua + "\" flagType=\"" + "A" + "\" ver=\"2.5\"></AuthRequest>";
                    System.Net.WebRequest req = null;
                    WebResponse rsp = null;
                    try
                    {
                        req = WebRequest.Create(url);
                        req.Method = "POST";
                        req.ContentType = "application/xml";
                        req.Headers["appname"] = "CO-Operative";
                        StreamWriter writer = new StreamWriter(req.GetRequestStream());
                        writer.Write(ModifiedData);
                        writer.Close();
                        rsp = req.GetResponse();

                        StreamReader sr = new StreamReader(rsp.GetResponseStream());
                        string results = sr.ReadToEnd();
                        sr.Close();
                        //Sp_callTrackingInsert("Response DataTable APIResponce ValidateAadharOTP(string txn, string adhar, string otp):- ", results);
                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(results);
                        XmlNodeList xnList = xml.SelectNodes("/DSMTokanize");
                        string sa = "";
                        foreach (XmlNode xn in xnList)
                        {
                            sa = xn["status"].InnerText;
                            if (sa.ToUpper() == "Y")
                                _AadhaarNo = xn["AadhaarNo"].InnerText;
                            else
                                _AadhaarNo = "NO";
                        }
                        if (sa.ToUpper() == "N")
                        {
                            _AadhaarNo = "NO";
                        }
                    }
                    catch (Exception ex)
                    {
                        _AadhaarNo = "NO#" + ex.Message;
                    }
                }
                else
                {
                    _AadhaarNo = "NO" + "#" + "ReferenceId Not Found.";
                }
            }
            catch (Exception ex)
            {
                _AadhaarNo = "NO" + "#" + "ReferenceId Not Found";
            }
            return _AadhaarNo;
        }
        public static void SendEmail(string strToAddress, string strSubject, string data)
        {
            //System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            //message.To.Add(new System.Net.Mail.MailAddress(strToAddress.ToString()));
            //message.Body = data;
            //message.IsBodyHtml = true;
            //message.From = new System.Net.Mail.MailAddress("donotreply.rajsahakar@raj.gov.in");
            //message.Subject = strSubject;
            //message.BodyEncoding = System.Text.Encoding.ASCII;
            //message.Priority = System.Net.Mail.MailPriority.High;
            //System.Net.Mail.SmtpClient Smtp = new System.Net.Mail.SmtpClient();
            //Smtp.Host = "mail.raj.gov.in";
            //Smtp.Port = 25;
            //Smtp.UseDefaultCredentials = true;
            //Smtp.Credentials = new System.Net.NetworkCredential("donotreply.rajsahakar@raj.gov.in", "Rajasthan@1027");
            //Smtp.EnableSsl = false;
            //Smtp.Send(message);

        }
        public static UserDetailsEntity1 GetUserDetailsBySSOId(string ssoid)
        {
            try
            {
                SSOWSSoap.SSOUserDetail userDetails = new SSOWSSoap.SSOUserDetail();
                SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
                userDetails = ssoclient.GetUserDetailXML(ssoid, "RAJSAHKAR.TEST", "Rajasthan@12");


                UserDetailsEntity1 user = new UserDetailsEntity1();
                if (userDetails != null)
                {
                    user = new UserDetailsEntity1
                    {
                        SSOID = userDetails.SSOID ?? "",
                        AadhaarId = userDetails.aadhaarId ?? "",
                        BhamashahId = userDetails.bhamashahId ?? "",
                        BhamashahMemberId = userDetails.bhamashahMemberId ?? "",
                        DisplayName = userDetails.displayName ?? "",
                        DateOfBirth = userDetails.dateOfBirth ?? "",
                        Gender = userDetails.gender ?? "",
                        Mobile = userDetails.mobile ?? "",
                        TelephoneNumber = userDetails.telephoneNumber ?? "",
                        MailPersonal = userDetails.mailPersonal ?? "",
                        PostalAddress = userDetails.postalAddress ?? "",
                        PostalCode = userDetails.postalCode ?? "",
                        //l = userDetails.l ?? "",
                        ST = userDetails.st ?? "",
                        JpegPhoto = userDetails.jpegPhoto ?? "",
                        Designation = userDetails.designation ?? "",
                        Department = userDetails.department ?? "",
                        MailOfficial = userDetails.mailOfficial ?? "",
                        EmployeeNumber = userDetails.employeeNumber ?? "",
                        DepartmentId = userDetails.departmentId ?? ""
                    };
                }
                return user;
            }
            catch
            {
                return (new UserDetailsEntity1());
            }

        }

        public static string ValidateEmblemsAndName(string Name, int SocietyTypeId)
        {

            List<AdministrativeData> objData = new List<AdministrativeData>();
            List<AmendmentDocuments> amendmentDoc = new List<AmendmentDocuments>();

            /*Remove Special words check from system as request letter recieved from JR rules department*/

            DataTable DT = new SocietyModel().GetLovValuesByListId(1);

            foreach (DataRow Dr in DT.Rows)
            {

                string[] validValue = new string[] { "government", "राजकीय", "state", "केंद्रीय", "central" };
                string LovCode = Dr["LovCode"].ToString().Replace("\n", "").Replace("\r", "").ToUpper();
                string Lovdescription = Dr["Lovdescription"].ToString().Replace("\n", "").Replace("\r", "").ToUpper();
                //string SocietyName = Name.ToUpper();

                string[] SocietyNamekey = Name.ToUpper().Replace("  ", "").Split(' ');
                if ((SocietyTypeId == 5 || SocietyTypeId == 11) && (validValue.Contains(LovCode.ToLower()) || validValue.Contains(Lovdescription.ToLower())))
                {

                }
                else
                 if (SocietyNamekey.Where(x => x == LovCode || x == Lovdescription).Any() && SocietyTypeId == 5 && SocietyTypeId == 11)
                {
                    return "NotValidName:-" + LovCode + "/" + Lovdescription;
                }
            }
            return "";
        }


        #region Janaadhar Details (Gopal Credit Card By Pankaj)
        public static string Masked(string source)
        {
            source = source.Trim();
            string mask = string.Empty;
            if (source.Length > 4)
            {
                int unMaskedEnd = source.Length - 4;
                var firstPart = string.Concat(Enumerable.Repeat("X", source.Length - 4));
                mask = firstPart + source.Substring(source.Length - 4);
            }
            else
            {
                mask = "XXXX";
            }
            return mask;
        }
        public static List<JanAadharModel.JanAaadharFamilyMemberModel> GetJanAadhaarMembersList(string JanaadharNo)
        {
            List<JanAadharModel.JanAaadharFamilyMemberModel> resultData = new List<JanAadharModel.JanAaadharFamilyMemberModel>();
            try
            {
                //string result = CallWebService();
                string client_id = "f6de7747-60d3-4cf0-a0ae-71488abd6e95";
                string URL = "https://api.sewadwaar.rajasthan.gov.in/app/live/Janaadhaar/Prod/Service/action/fetchJayFamily" + "/" + JanaadharNo + "?client_id=" + client_id;

                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                // | SecurityProtocolType.Tls11
                //  | SecurityProtocolType.Tls12;                                 ;
                HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(URL);
                objRequest.Method = "GET";
                objRequest.KeepAlive = false;

                HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    sr.Close();
                }
                //return result;

                //result = "[{\"AADHAR_ID\":null,\"NAME\":\"Munni Devi\",\"DOB\":\"01\\/01\\/1958\",\"JAN_MEMBER_ID\":\"38536525079\",\"SRDR_MID\":\"0\",\"JAN_AADHAR\":\"5058544181\",\"NAME_HINDI\":\"मुन्नी देवी\",\"SRDR_ID\":\"BHNRJJH\",\"JANAADHAARMEMID\":\"132177929\",\"ENR_ID\":\"9999-GW2W-00300\",\"NFSA_STATUS\":\"N\",\"FATHER_NAME_HND\":\"गोरधन लाल सेवदा\",\"MOBILE_NO\":\"9785364768\",\"FATHER_NAME_ENG\":\"Gordhan Lal Sewada\",\"AADHAR_REF_NO\":\"444081981222965\",\"GENDER\":\"Female\",\"MEMBER_TYPE\":\"HOF\"},{\"AADHAR_ID\":null,\"NAME\":\"Sangita Sharma\",\"DOB\":\"09\\/02\\/1984\",\"JAN_MEMBER_ID\":\"86458060011\",\"SRDR_MID\":\"113287472\",\"JAN_AADHAR\":\"5058544181\",\"NAME_HINDI\":\"संगीता शर्मा\",\"SRDR_ID\":\"BHNRJJH\",\"JANAADHAARMEMID\":\"113287472\",\"ENR_ID\":\"9999-GW2W-00300\",\"NFSA_STATUS\":\"N\",\"FATHER_NAME_HND\":\"रामेश्वर लाल\",\"MOBILE_NO\":\"9785364768\",\"FATHER_NAME_ENG\":\"Rameshwar Lal\",\"AADHAR_REF_NO\":\"444500528476207\",\"GENDER\":\"Female\",\"MEMBER_TYPE\":\"MEM\"},{\"AADHAR_ID\":null,\"NAME\":\"Gaurav\",\"DOB\":\"31\\/03\\/2011\",\"JAN_MEMBER_ID\":\"21959611793\",\"SRDR_MID\":\"113287473\",\"JAN_AADHAR\":\"5058544181\",\"NAME_HINDI\":\"गौरव\",\"SRDR_ID\":\"BHNRJJH\",\"JANAADHAARMEMID\":\"113287473\",\"ENR_ID\":\"9999-GW2W-00300\",\"NFSA_STATUS\":\"N\",\"FATHER_NAME_HND\":\"अनिल कुमार\",\"MOBILE_NO\":\"9785364768\",\"FATHER_NAME_ENG\":\"Anil Kumar\",\"AADHAR_REF_NO\":\"444795603265772\",\"GENDER\":\"Male\",\"MEMBER_TYPE\":\"MEM\"},{\"AADHAR_ID\":null,\"NAME\":\"Anil Kumar\",\"DOB\":\"15\\/09\\/1979\",\"JAN_MEMBER_ID\":\"37265441741\",\"SRDR_MID\":\"113287471\",\"JAN_AADHAR\":\"5058544181\",\"NAME_HINDI\":\"अनिल कुमार\",\"SRDR_ID\":\"BHNRJJH\",\"JANAADHAARMEMID\":\"113287471\",\"ENR_ID\":\"9999-GW2W-00300\",\"NFSA_STATUS\":\"N\",\"FATHER_NAME_HND\":\"ओम प्रकाश\",\"MOBILE_NO\":\"9785364768\",\"FATHER_NAME_ENG\":\"Om Prakash\",\"AADHAR_REF_NO\":\"444963065846904\",\"GENDER\":\"Male\",\"MEMBER_TYPE\":\"MEM\"},{\"AADHAR_ID\":null,\"NAME\":\"Om Prakash\",\"DOB\":\"01\\/01\\/1952\",\"JAN_MEMBER_ID\":\"25870361516\",\"SRDR_MID\":\"113287469\",\"JAN_AADHAR\":\"5058544181\",\"NAME_HINDI\":\"ओम प्रकाश\",\"SRDR_ID\":\"BHNRJJH\",\"JANAADHAARMEMID\":\"113287469\",\"ENR_ID\":\"9999-GW2W-00300\",\"NFSA_STATUS\":\"N\",\"FATHER_NAME_HND\":\"रामदेव\",\"MOBILE_NO\":\"9785364768\",\"FATHER_NAME_ENG\":\"Ram Dev\",\"AADHAR_REF_NO\":\"444494900961321\",\"GENDER\":\"Male\",\"MEMBER_TYPE\":\"MEM\"},{\"AADHAR_ID\":null,\"NAME\":\"Sneha\",\"DOB\":\"28\\/04\\/2006\",\"JAN_MEMBER_ID\":\"23798847668\",\"SRDR_MID\":\"113287470\",\"JAN_AADHAR\":\"5058544181\",\"NAME_HINDI\":\"स्नेहा\",\"SRDR_ID\":\"BHNRJJH\",\"JANAADHAARMEMID\":\"113287470\",\"ENR_ID\":\"9999-GW2W-00300\",\"NFSA_STATUS\":\"N\",\"FATHER_NAME_HND\":\"अनिल कुमार\",\"MOBILE_NO\":\"9785364768\",\"FATHER_NAME_ENG\":\"Anil Kumar\",\"AADHAR_REF_NO\":null,\"GENDER\":\"Female\",\"MEMBER_TYPE\":\"MEM\"}]";


                if (!result.Contains("JAN_MEMBER_ID"))
                {
                    return null;
                }
                resultData = JsonConvert.DeserializeObject<List<JanAadharModel.JanAaadharFamilyMemberModel>>(result);

                resultData.ForEach(x =>
                {
                    x.Age = CalculateAge_DDMMYYYY(x.DOB);

                    x.Masked_JAN_MEMBER_ID = !String.IsNullOrEmpty(x.JAN_MEMBER_ID) == true ? Masked(x.JAN_MEMBER_ID) : "";
                    x.Encrypted_JAN_MEMBER_ID = !String.IsNullOrEmpty(x.JAN_MEMBER_ID) == true ? Encrypt(x.JAN_MEMBER_ID) : "";
                    x.Masked_MOBILE_NO = !String.IsNullOrEmpty(x.MOBILE_NO) == true ? Masked(x.MOBILE_NO) : "";
                    x.Encrypted_MOBILE_NO = !String.IsNullOrEmpty(x.MOBILE_NO) == true ? Encrypt(x.MOBILE_NO) : "";
                    x.Encrypted_AADHAR_REF_NO = !String.IsNullOrEmpty(x.AADHAR_REF_NO) == true ? Encrypt(x.AADHAR_REF_NO) : "";
                    x.Encrypted_JAN_AADHAR = !String.IsNullOrEmpty(x.JAN_AADHAR) == true ? Encrypt(x.JAN_AADHAR) : "";
                    x.JAN_AADHAR = !String.IsNullOrEmpty(x.JAN_AADHAR) == true ? Masked(x.JAN_AADHAR) : "";
                    x.Encrypted_ENR_ID = !String.IsNullOrEmpty(x.ENR_ID) == true ? Encrypt(x.ENR_ID) : "";
                });



            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.COMMON_FUNCTION", FunctionName = " GetJanAadhaarMembersList(string JanaadharNo) JanaadharNo: " + JanaadharNo }, ex);
                return null;
            }
            return resultData;
        }
        public static int CalculateAge_DDMMYYYY(string DOB)
        {
            int Age = 0;
            try
            {
                if (!String.IsNullOrEmpty(DOB))
                {
                    string[] dobArr = DOB.Split('/');
                    if (dobArr.Length == 3)
                    {
                        string Day = dobArr[0].ToString();
                        string Month = dobArr[1].ToString();
                        string Year = dobArr[2].ToString();
                        DateTime dobNew = Convert.ToDateTime(Year + "-" + Month + "-" + Day);

                        var today = DateTime.Today;

                        var currentDays = (today.Year * 100 + today.Month) * 100 + today.Day;
                        var dobDays = (dobNew.Year * 100 + dobNew.Month) * 100 + dobNew.Day;

                        Age = (currentDays - dobDays) / 10000;
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.COMMON_FUNCTION", FunctionName = " CalculateAge_DDMMYYYY(string DOB)" }, ex);
                //return null;
            }
            return Age;
        }
        public static DataSet SendJanAadharOTP(string eJanId, string eJanMID)
        {
            string JanAdhaarClientID = "b10d20ae-367c-4cab-8b14-49330d4a392d";
            DataSet objdsotp = new DataSet();
            try
            {
                string JanAadharNo = COMMON_FUNCTION.Decrypt(eJanId);
                string JanMID = COMMON_FUNCTION.Decrypt(eJanMID);

                string Scheme = "CEA";
                var request = (HttpWebRequest)WebRequest.Create("https://api.sewadwaar.rajasthan.gov.in/app/live/Janaadhaar/Prod/Service/Gen/Otp/For?client_id=" + JanAdhaarClientID);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.Method = "POST";
                string postData = "<root><genOtpFor><janaadharId>" + JanAadharNo + "</janaadharId><mid>" + JanMID + "</mid><schemeId>" + Scheme + "</schemeId></genOtpFor></root>";
                byte[] byteArray = Encoding.ASCII.GetBytes(postData);
                request.ContentType = "text/xml;charset=utf-8";
                request.ContentLength = byteArray.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(byteArray, 0, byteArray.Length);
                requestStream.Close();

                Sp_callTrackingInsert("SendJanAadharOTP(string eJanId, string eJanMID) postData- ", request.ToString() + "/" + postData.ToString());
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                //var responseString = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<root><status>true</status><tid>168232615</tid></root>";

                Sp_callTrackingInsert("SendJanAadharOTP(string eJanId, string eJanMID) Response- ", responseString.ToString());

                int foundS1 = responseString.IndexOf("<root>");
                responseString = responseString.Substring(foundS1);
                //responseString = "<root><status>true</status><tid>168232615</tid></root>";

                objdsotp = ConvertXMLResponceToDataSet(responseString);

                StreamReader respStream = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);
                string receivedResponse = respStream.ReadToEnd();
                respStream.Close();
                response.Close();

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.COMMON_FUNCTION", FunctionName = " SendJanAadharOTP(string eJanId, string eJanMID)" }, ex);
                //return null;
            }

            return objdsotp;
        }
        public static DataSet ValidateJanAadharOTP(string otp, string tid)
        {
            string JanAdhaarClientID = "b10d20ae-367c-4cab-8b14-49330d4a392d";
            DataSet objdsotp = new DataSet();
            try
            {
                //196877
                string Scheme = "CEA";
                var request = (HttpWebRequest)WebRequest.Create("https://api.sewadwaar.rajasthan.gov.in/app/live/Janaadhaar/Prod/Service/Validate/Otp?client_id=" + JanAdhaarClientID);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.Method = "POST";
                string postData = "<root><validateOtp><tid>" + tid + "</tid><otp>" + otp + "</otp><schemeId>" + Scheme + "</schemeId></validateOtp></root>";
                byte[] byteArray = Encoding.ASCII.GetBytes(postData);
                request.ContentType = "text/xml;charset=utf-8";
                request.ContentLength = byteArray.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(byteArray, 0, byteArray.Length);
                requestStream.Close();

                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                int foundS1 = responseString.IndexOf("<root>");
                responseString = responseString.Substring(foundS1);

                objdsotp = ConvertXMLResponceToDataSet(responseString);

                StreamReader respStream = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);
                string receivedResponse = respStream.ReadToEnd();
                respStream.Close();
                response.Close();

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.COMMON_FUNCTION", FunctionName = " ValidateJanAadharOTP(string otp, string tid)" }, ex);
                //return null;
            }

            return objdsotp;
        }
        public static JanAadharModel GetJanAadharMembersDetail(string JanAadharid, string enrId, string aadharid, string janmemidselected)
        {
            GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());

            JanAadharModel data = new JanAadharModel();
            try
            {
                data.Status = "";
                JanAadharMemberDetails dt = new JanAadharMemberDetails();
                string scheme = string.Empty;
                string client_id = "f6de7747-60d3-4cf0-a0ae-71488abd6e95";
                string Url = "https://api.sewadwaar.rajasthan.gov.in/app/live/Janaadhaar/Prod/Service/Info/Fetch" + "?client_id=" + client_id;

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                scheme = "HEADM";

                string infoFlag = "PFE";
                string authoMode = "AOTP";
                string dateTime = string.Empty;
                try
                {
                    dateTime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                }
                catch { dateTime = DateTime.Now.ToString(); }
                StringBuilder requestXml = new StringBuilder();
                requestXml.Append("<root>");
                if (JanAadharid.Length == 10)
                {
                    requestXml.Append("<Info><janaadhaarId>" + JanAadharid + "</janaadhaarId>");
                    requestXml.Append("<enrId></enrId>");
                }
                else
                {
                    requestXml.Append("<Info><janaadhaarId></janaadhaarId>");
                    requestXml.Append("<enrId>" + enrId + "</enrId>");

                }
                //requestXml.Append("<aadharId>" + aadharid + "</aadharId>");
                requestXml.Append("<aadharId></aadharId>");
                requestXml.Append("<scheme>" + scheme.TrimEnd() + "</scheme>");
                requestXml.Append("<infoFlg>" + infoFlag + "</infoFlg>");
                requestXml.Append("<authMode>" + authoMode + "</authMode>");
                requestXml.Append("<dateTime>" + dateTime + "</dateTime>");
                requestXml.Append("</Info>");
                requestXml.Append("</root>");



                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(requestXml.ToString());
                string pathStringRequest = "~/JanaadhaarnLog/Request";
                string pathStringResponse = "~/JanaadhaarnLog/Response";
                //new csCommon().CreateTextFile(Path.Combine(System.Web.HttpContext.Current.Server.MapPath(pathStringRequest), aadharid) + ".txt", pathStringRequest, requestXml.ToString());
                var req = (HttpWebRequest)WebRequest.Create(Url);



                req.ContentType = "application/xml";
                req.Method = "POST";
                req.ContentLength = bytes.Length;

                using (Stream os = req.GetRequestStream())
                {
                    os.Write(bytes, 0, bytes.Length);
                }

                string response = "";
                using (WebResponse resp = req.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                    {
                        response = sr.ReadToEnd().Trim();
                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(response);
                        //new csCommon().CreateTextFile(Path.Combine(System.Web.HttpContext.Current.Server.MapPath(pathStringResponse), aadharid) + ".txt", pathStringResponse, response);
                        XmlElement Dtlist1 = xml.DocumentElement;
                        XmlElement Dtlist2 = xml.DocumentElement;
                        List<string[]> listx = new List<string[]>();
                        //XmlNodeList xDataList = xml.SelectNodes("//personalInfo/member");

                        XmlNodeList nodeList = Dtlist1.SelectNodes("//personalInfo/member");
                        string enrid = "", janaadhaarId = "", nameEng = "", nameHnd = "", fnameEng = "", fnameHnd = "", dob = "", aadhar = "", mobile = "", acc = "", bankName = "", ifsc = "", gender = "", bankBranch = "";
                        string addressEng = "", addressHnd = "", pin = "", caste = "", category = "", districtName = "", block_city = "", gp = "", village = "", age = "", snameHnd = "", snameEng = "", maritalStatus = "", relationTyp = "";
                        string micr = "", voterId = "", mnameEng = "", mnameHnd = "", income = "", occupation = "", qualification = "", panNo = "", passport = "", dlNo = "", eid = "", janmemid = "";


                        if (nodeList.Count > 0)
                        {
                            foreach (XmlNode node in nodeList)
                            {
                                janmemid = node.SelectSingleNode("jan_mid").InnerXml;
                                if (janmemid == "0")
                                {
                                    janmemid = node.SelectSingleNode("hof_jan_m_id").InnerXml;
                                }


                                if (janmemidselected == janmemid)
                                {
                                    data.Status = "OK";
                                    dt.JanAadharMID = janmemid;
                                    dt.Encrypted_JanAadharMID = Encrypt(janmemid);
                                    dt.JanAadharEnrollmentId = node.SelectSingleNode("enrId").InnerXml;
                                    dt.JanAadharNo = node.SelectSingleNode("janaadhaarId").InnerXml;
                                    dt.Encrypted_JanAadharNo = Encrypt(node.SelectSingleNode("janaadhaarId").InnerXml);
                                    dt.AadharNo = node.SelectSingleNode("aadhar").InnerXml;
                                    dt.Encrypted_AadharNo = Encrypt(node.SelectSingleNode("aadhar").InnerXml);
                                    dt.Name_ENG = node.SelectSingleNode("nameEng").InnerXml;
                                    dt.Name_HI = node.SelectSingleNode("nameHnd").InnerXml;
                                    dt.FatherName_ENG = node.SelectSingleNode("fnameEng").InnerXml;
                                    dt.FatherName_HI = node.SelectSingleNode("fnameHnd").InnerXml;
                                    dt.SpouseName_ENG = node.SelectSingleNode("snameEng").InnerXml;
                                    dt.SpouseName_HI = node.SelectSingleNode("snameHnd").InnerXml;
                                    dt.MotherName_ENG = node.SelectSingleNode("mnameEng").InnerXml;
                                    dt.MotherName_HI = node.SelectSingleNode("mnameHnd").InnerXml;

                                    dt.MaritalStatus = node.SelectSingleNode("maritalStatus").InnerXml;
                                    dt.RelationType = node.SelectSingleNode("relationTyp").InnerXml;

                                    dt.DOB = node.SelectSingleNode("dob").InnerXml;
                                    dt.Gender = node.SelectSingleNode("gender").InnerXml;
                                    dt.MobileNo = node.SelectSingleNode("mobile").InnerXml;

                                    dt.BankAccountNo = node.SelectSingleNode("acc").InnerXml;
                                    dt.Encrypted_BankAccountNo = Encrypt(node.SelectSingleNode("acc").InnerXml);
                                    dt.BankName = node.SelectSingleNode("bankName").InnerXml;
                                    dt.BankIFSCCode = node.SelectSingleNode("ifsc").InnerXml;
                                    dt.Age = node.SelectSingleNode("age").InnerXml;
                                    dt.BankBranchName = node.SelectSingleNode("bankBranch").InnerXml;
                                    dt.BankMICRCode = node.SelectSingleNode("micr").InnerXml;




                                    //dt.voterId = node.SelectSingleNode("voterId").InnerXml;

                                    dt.Income = node.SelectSingleNode("income").InnerXml;
                                    dt.Occupation = node.SelectSingleNode("occupation").InnerXml;
                                    //dt.qualification = node.SelectSingleNode("qualification").InnerXml;
                                    dt.PANCardNo = node.SelectSingleNode("panNo").InnerXml;
                                    //dt.passport = node.SelectSingleNode("passport").InnerXml;
                                    //dt.dlNo = node.SelectSingleNode("dlNo").InnerXml;
                                    //dt.eid = node.SelectSingleNode("eid").InnerXml;
                                    dt.MemberPhoto_Base64 = JanAadhaarMemberPhoto(JanAadharid, janmemid);

                                }
                            }
                            XmlNodeList nodeList1 = Dtlist2.SelectNodes("//family/familydetail");
                            dt.Address = new JanAadharMemberAddressDetails();
                            foreach (XmlNode node1 in nodeList1)
                            {
                                dt.Caste = node1.SelectSingleNode("caste").InnerXml;
                                dt.Category = node1.SelectSingleNode("category").InnerXml;
                                dt.Address.Address_ENG = node1.SelectSingleNode("addressEng").InnerXml;
                                dt.Address.Address_HI = node1.SelectSingleNode("addressHnd").InnerXml;
                                dt.Address.PinCode = node1.SelectSingleNode("pin").InnerXml;

                                if (node1.SelectSingleNode("isRural").InnerXml == "N")
                                    dt.Address.IsRural = false;
                                if (node1.SelectSingleNode("isRural").InnerXml == "Y")
                                    dt.Address.IsRural = true;
                                dt.Address.DistrictName_ENG = node1.SelectSingleNode("districtName").InnerXml;
                                dt.Address.DistrictName_HI = node1.SelectSingleNode("districtNameHin").InnerXml;
                                dt.Address.District_Code = node1.SelectSingleNode("districtCd").InnerXml;
                                dt.Address.DISTRICT_ID = objGopalCreditCardDBModel.Get_MasterDataIDByCode(dt.Address.District_Code, "DISTRICT_CODE");

                                dt.Address.Block_ENG = node1.SelectSingleNode("block_city").InnerXml;
                                dt.Address.Block_HI = node1.SelectSingleNode("block_city_hin").InnerXml;
                                dt.Address.Block_Code = node1.SelectSingleNode("block_cityCd").InnerXml;
                                dt.Address.BLOCK_ID = objGopalCreditCardDBModel.Get_MasterDataIDByCode(dt.Address.Block_Code, "BLOCK_CODE");

                                dt.Address.City_ENG = node1.SelectSingleNode("block_city").InnerXml;
                                dt.Address.City_HI = node1.SelectSingleNode("block_city_hin").InnerXml;
                                dt.Address.City_Code = node1.SelectSingleNode("block_cityCd").InnerXml;
                                dt.Address.CITY_ID = objGopalCreditCardDBModel.Get_MasterDataIDByCode(dt.Address.City_Code, "CITY_CODE");

                                dt.Address.GP_ENG = node1.SelectSingleNode("gp").InnerXml;
                                dt.Address.GP_HI = node1.SelectSingleNode("gpHin").InnerXml;
                                dt.Address.GP_Code = node1.SelectSingleNode("gpCd").InnerXml;
                                dt.Address.GP_ID = objGopalCreditCardDBModel.Get_MasterDataIDByCode(dt.Address.GP_Code, "GP_CODE");

                                dt.Address.Ward_ENG = node1.SelectSingleNode("ward").InnerXml;
                                dt.Address.Ward_HI = node1.SelectSingleNode("wardHin").InnerXml;

                                dt.Address.Village_ENG = node1.SelectSingleNode("village").InnerXml;
                                dt.Address.Village_HI = node1.SelectSingleNode("villageHin").InnerXml;
                                dt.Address.Village_Code = node1.SelectSingleNode("villageCd").InnerXml;
                                dt.Address.VILLAGE_ID = objGopalCreditCardDBModel.Get_MasterDataIDByCode(dt.Address.Village_Code, "VILLAGE_CODE");
                            }
                        }
                        else
                        {
                            data.Status = "";
                            data.Message = "Member details not found";
                        }
                    }

                }
                if (data.Status == "")
                {
                    data.Message = "Member details not found";
                }
                data.UserDetails = new JanAadharMemberDetails();
                data.UserDetails = dt;
                data.UserDetails.JanAadharNo = Masked(data.UserDetails.JanAadharNo ?? "");
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.COMMON_FUNCTION", FunctionName = " GetJanAadharMembersDetail(string JanAadharid, string enrId, string aadharid, string janmemidselected)" }, ex);
                //return null;
            }
            return data;
        }
        public static string JanAadhaarMemberPhoto(string JanaadharNo, string MemberId)
        {
            string photo = "";
            try
            {
                string client_id = "0cbe6b41-c050-44a5-a3b7-3ec37f27b624";
                // string Url = "f6de7747-60d3-4cf0-a0ae-71488abd6e95" + JanaadharNo + "/" + MemberId + "?client_id=" + client_id;
                string Url = "https://api.sewadwaar.rajasthan.gov.in/app/live/Janaadhaar/Prod/Service/details/hofAndMemberPhoto/" + JanaadharNo + "/" + MemberId + "?client_id=" + client_id;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var request = (HttpWebRequest)WebRequest.Create(Url);
                var response = (HttpWebResponse)request.GetResponse();
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseString);
                photo = ((Newtonsoft.Json.Linq.JContainer)(values["member_Photo"])).First.First.First.ToString();

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.COMMON_FUNCTION", FunctionName = " JanAadhaarMemberPhoto(string JanaadharNo, string MemberId)" }, ex);
                //return null;
            }
            return photo;
        }
        public static DataSet ConvertXMLResponceToDataSet(string xmlData)
        {
            StringReader stream = null;
            XmlTextReader reader = null;
            try
            {
                DataSet xmlDS = new DataSet();
                stream = new StringReader(xmlData);
                // Load the XmlTextReader from the stream
                reader = new XmlTextReader(stream);
                xmlDS.ReadXml(reader);
                return xmlDS;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.COMMON_FUNCTION", FunctionName = " ConvertXMLResponceToDataSet(string xmlData)" }, ex);
                return null;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }
        #endregion

        public static void Sp_callTrackingInsert(string function_name, string msj)
        {

            if (msj == null) { msj = ""; }

            DBFactoryMSSQL conn = new DBFactoryMSSQL();
            SqlConnection sqlConnection = conn.GetConnection;
            try
            {
                sqlConnection.Open();
                SqlCommand cmdstr = new SqlCommand();
                cmdstr.CommandText = "INSERT INTO SPCallingTracker(ProcedureName,Parameter) values (N'" + function_name + "',N'" + msj + "')";
                cmdstr.CommandType = CommandType.Text;
                cmdstr.Connection = sqlConnection;
                cmdstr.ExecuteScalar();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = " Sp_callTrackingInsert(string function_name,string " + msj + ")" }, ex);
            }
            finally
            {
                sqlConnection.Close();
            }

        }



        #region Madhuri JanAadhar service web

        public async static Task<string> GetJanAadhaarDetails(string JanaadhaarNo)
        {
            var mid = "";
            csCommon objComm = new csCommon();
            DataTable dt = new DataTable();
            dt = await objComm.GetJanAadhaarMIDByAadhaar(JanaadhaarNo);


            var JanAadharId = ""; var MemberId = "";
            try
            {
                if (dt != null && dt.Rows.Count > 0 && (dt.Columns.Contains("JAN_MEMBER_ID")))
                {
                    foreach (DataRow dr in dt.Rows) // search whole table
                    {
                        JanAadharId = dr["JAN_AADHAR"].ToString();
                        MemberId = dr["JAN_MEMBER_ID"].ToString();
                    }

                    try
                    {
                        DataSet dtts = new DataSet();
                        dtts = csCommon.SendOtpByAadharNo_KYC(JanAadharId, MemberId);
                        DataTable dtt = new DataTable();

                        if (dtts != null && dtts.Tables.Count > 0)
                            dtt = dtts.Tables[0];

                        if (dtt != null && dtt.Rows.Count > 0)
                        {
                            var encData = Encrypt(dtt.Rows[0]["tid"].ToString() + "@" + JanAadharId + "@" + MemberId);
                            return encData;
                        }
                        else
                        {
                            return "Fail";
                        }
                    }

                    catch (Exception ex)
                    {
                        DBLogger objDBLogger = new DBLogger();
                        objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.CommonFunction", FunctionName = "GetJanAadhaarDetails(string JanaadhaarNo) _1" }, ex);
                        return "Fail";
                    }
                }
            }
            catch (Exception ex)
            {

                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.CommonFunction", FunctionName = "GetJanAadhaarDetails(string JanaadhaarNo)_2" }, ex);
            }
            if (dt.Rows.Count > 0)
            {
                return COMMON_FUNCTION.DataTableToJSONString(dt);

            }
            else
            {
                dt = null;
                return COMMON_FUNCTION.DataTableToJSONString(dt);
            }


        }

        public static APIResponce VerifyRecheckOTP(string tid, string OTP)
        {
            APIResponce apiResponce = new APIResponce();
            AadharResponce Responce = new AadharResponce();
            Responce.UserAddress = new AadharAddressResponce();
            Responce.UserInfo = new AadharUserInfoResponce();
            Responce.UserLocation = new AadharLocationResponce();

            if (tid != null && tid != "")
            {
                tid = Decrypt(tid);
                var a = tid.Split('@');
                var t_id = a[0];
                var JanAadharId = a[1];
                var MemberId = a[2];
                try
                {
                    string message = string.Empty;
                    DataTable dt = new DataTable();
                    dt = csCommon.ValidateAadharOTP(t_id, OTP);

                    dt.Rows[0]["isvalidate"].ToString();
                    if (dt.Rows[0]["isvalidate"].ToString() == "true")
                    {

                        csCommon objComm = new csCommon();
                        DataSet dS = new DataSet();
                        dt = objComm.GetJanAadharMembersDetail(JanAadharId, JanAadharId, MemberId); //Session["JanaadhaarNo"] as aadhar

                        if (dt != null)
                        {
                            Responce.UserInfo = new AadharUserInfoResponce() { name = dt.Rows[0]["Column2"].ToString(), photo = dt.Rows[0]["Column5"].ToString() };
                            Responce.UserAddress = new AadharAddressResponce() { co = dt.Rows[0]["Column8"].ToString(), house = dt.Rows[0]["Column10"].ToString() };
                            Responce.UserLocation = new AadharLocationResponce() { house = dt.Rows[0]["Column10"].ToString() };

                            apiResponce.Status = "Success";
                            apiResponce.Data = Responce;
                        }
                        else
                        {

                            apiResponce.Status = "Failed";
                            apiResponce.Message = "";
                        }
                    }
                    else
                    {
                        apiResponce.Status = "Failed";
                        apiResponce.Message = "";
                    }

                    return apiResponce;
                }
                catch (Exception ex)
                {

                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen", FunctionName = "APIResponce VerifyRecheckOTP(string tid, string OTP)" }, ex);
                    apiResponce.Status = "Failed";
                    apiResponce.Message = "Some error occured.";
                    return apiResponce;
                }
            }
            else
            {
                apiResponce.Status = "Failed";
                apiResponce.Message = "";
            }

            return apiResponce;

        }

        public static string EncryptWithKey(string Text, string EncryptionKey, string AppCode)
        {
            string Key = EncryptionKey;
            string initVector = AppCode;
            int keysize = 256;

            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(Text);
            PasswordDeriveBytes password = new PasswordDeriveBytes(Key, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] Encrypted = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(Encrypted);
        }
        public static string DecryptWithKey(string EncryptedText, string EncryptionKey, string AppCode)
        {
            string Key = EncryptionKey;
            string initVector = AppCode;
            int keysize = 256;
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] DeEncryptedText = Convert.FromBase64String(EncryptedText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(Key, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(DeEncryptedText);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[DeEncryptedText.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }
        #endregion



        #region Madhuri(New Aadhar Esign Service)

        public static ResponseDataEsign IsValidEsignOtp(string RequestId)
        {

            string AppCode = "PRAVL22868ABFAP012";
            String EncryptedData = "";
            ResponseDataEsign responseDataEsign = new ResponseDataEsign();

            string url = "https://aadhaarauth.rajasthan.gov.in/AadhaarFaceAuthAPI/api/service";

            EncryptionResult e = new EncryptionResult();
            if (HttpContext.Current.Request.Url.Host.Contains("10.70.234.208") || HttpContext.Current.Request.Url.Host.Contains("10.68.231.168"))
            {
                AppCode = "PRAVL22868ABFAP021";
                url = "https://aadhaarauthtest.rajasthan.gov.in/GenericFaceAuthApi/api/service";
            }
            string Data = JsonConvert.SerializeObject(new
            {
                AppCode = AppCode,
                RequestId = RequestId,
            });
            string encData = "";

            if (HttpContext.Current.Request.Url.Host.Contains("10.70.234.208") || HttpContext.Current.Request.Url.Host.Contains("10.68.231.168"))
                e = EsignAesEncryption.Encrypt_New(Data, GlobalConstant.AadharAuthEsigEncKey_Test_stagging);
            else
                e = EsignAesEncryption.Encrypt_New(Data, GlobalConstant.AadharAuthEsigEncKey_Test);

            if (e.IsSuccess)
            {
                encData = Convert.ToBase64String(e.EncriptedData);
                string Jsondata = JsonConvert.SerializeObject(new { AppCode = AppCode, Data = encData, });

                try
                {
                    EncryptedData = WebRequestinJson(url, Jsondata, "application/json");

                    JObject jObj = JObject.Parse(EncryptedData);
                    string ResponseEncData = jObj["Data"].ToString();

                    EncryptionResult ResponseDecData = new EncryptionResult();

                    if (HttpContext.Current.Request.Url.Host.Contains("10.70.234.208") || HttpContext.Current.Request.Url.Host.Contains("10.68.231.168"))
                        ResponseDecData = EsignAesEncryption.Decrypt_New(ResponseEncData, GlobalConstant.AadharAuthEsigEncKey_Test_stagging);
                    else
                        ResponseDecData = EsignAesEncryption.Decrypt_New(ResponseEncData, GlobalConstant.AadharAuthEsigEncKey_Test);



                    Sp_callTrackingInsert("GetTxnIdOtp parsedResponse DecriptedData:- " + Data, ResponseDecData.DecriptedData);
                    if (ResponseDecData.IsSuccess)
                    {
                        // decryptedData = ResponseDecData.DecriptedData.ToString();

                        var parsedResponse = JsonConvert.DeserializeObject<ResponseDataEsign>(ResponseDecData.DecriptedData);
                        Sp_callTrackingInsert("GetTxnIdOtp parsedResponse S2s:-", ResponseDecData.DecriptedData);
                        if (parsedResponse.Status != "N")
                        {
                            //JObject root = (JObject)JObject.Parse(response);
                            //ResponseDataEsign responseData = root.ToObject<ResponseDataEsign>();
                            //Sp_callTrackingInsert("GetTxnIdOtp parsedResponse s2s :-", response);
                            return parsedResponse;
                        }

                    }
                    else
                    {
                        return null;
                        //return decryptedData;
                    }



                }
                catch (Exception ex)
                {
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.CommonFunctions", FunctionName = " static ResponseDataEsign IsValidEsignOtp(string RequestId)" }, ex);
                    return null;
                }


            }
            else
            {
                encData = "NO" + "#" + e.Message.ToString();
            }






            return null;
        }



        public static void ImportExcelToSql()
        {
            string excelFilePath = @"Downloads\TT SSO ID MAPPING LIST ALL BRANCH.xlsx";
            string connectionString = "Data Source=ACRUX\\SQL2019;Initial Catalog=Coop_Rajeev;Integrated Security=False;User ID=developer;Password=P@ssw0rd123;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;";

            // Load the Excel file
            FileInfo existingFile = new FileInfo(excelFilePath);
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                // Loop through all worksheets in the Excel file
                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    if (worksheet.Dimension == null) continue; // Skip empty sheets

                    Console.WriteLine($"Importing data from sheet: {worksheet.Name}");

                    // Read data into a DataTable
                    DataTable dataTable = new DataTable(worksheet.Name);

                    // Add columns to DataTable
                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                    {
                        dataTable.Columns.Add(worksheet.Cells[1, col].Text); // Assume the first row has headers
                    }

                    // Add rows to DataTable
                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++) // Start from the second row
                    {
                        var newRow = dataTable.NewRow();
                        for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                        {
                            newRow[col - 1] = worksheet.Cells[row, col].Text;
                        }
                        dataTable.Rows.Add(newRow);
                    }

                    // Insert data into SQL Server
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        // Use SqlBulkCopy to efficiently insert data
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                        {
                            bulkCopy.DestinationTableName = "PacsTestTable"; // Change to your target table name
                            bulkCopy.BatchSize = 1000; // Adjust as necessary

                            try
                            {
                                bulkCopy.WriteToServer(dataTable);
                                Console.WriteLine($"Successfully imported {dataTable.Rows.Count} rows from sheet: {worksheet.Name}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error importing data from sheet: {worksheet.Name}. Exception: {ex.Message}");
                            }
                        }
                    }
                }
            }
        }
        #endregion


        public static DataTable ListToDataTable(object data)
        {
            DataTable table = new DataTable();

            if (data == null)
                return table;

            if (data is Newtonsoft.Json.Linq.JArray)
            {
                var jArray = (Newtonsoft.Json.Linq.JArray)data;
                foreach (var item in jArray)
                {
                    var dict = item.ToObject<Dictionary<string, object>>();
                    if (table.Columns.Count == 0)
                    {
                        foreach (var key in dict.Keys)
                        {
                            table.Columns.Add(key);
                        }
                    }

                    var row = table.NewRow();
                    foreach (var key in dict.Keys)
                    {
                        row[key] = dict[key] ?? DBNull.Value;
                    }

                    table.Rows.Add(row);
                }
            }
            else if (data is Newtonsoft.Json.Linq.JObject)
            {
                var jObject = (Newtonsoft.Json.Linq.JObject)data;
                var dict = jObject.ToObject<Dictionary<string, object>>();
                foreach (var key in dict.Keys)
                {
                    table.Columns.Add(key);
                }

                var row = table.NewRow();
                foreach (var key in dict.Keys)
                {
                    row[key] = dict[key] ?? DBNull.Value;
                }

                table.Rows.Add(row);
            }

            return table;
        }
        public static bool CheckMimeType(this Stream stream, string extension)
        {
            stream.Seek(0, SeekOrigin.Begin);

            List<string> jpg = new List<string> { "FF", "D8" };
            List<string> bmp = new List<string> { "42", "4D" };
            List<string> gif = new List<string> { "47", "49", "46" };
            List<string> png = new List<string> { "89", "50", "4E", "47", "0D", "0A", "1A" };

            List<string> pdf = new List<string> { "25", "50", "44", "46" };
            List<List<string>> imgTypes = new List<List<string>>();

            if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg")
                imgTypes.Add(jpg);
            else if (extension.ToLower() == ".bmp")
                imgTypes.Add(bmp);
            else if (extension.ToLower() == ".gif")
                imgTypes.Add(gif);
            else if (extension.ToLower() == ".png")
                imgTypes.Add(png);
            else if (extension.ToLower() == ".pdf")
                imgTypes.Add(pdf);

            List<string> bytesIterated = new List<string>();

            for (int i = 0; i < 8; i++)
            {
                string bit = stream.ReadByte().ToString("X2");
                bytesIterated.Add(bit);

                bool isImage = imgTypes.Any(img => !img.Except(bytesIterated).Any());
                if (isImage)
                {
                    return true;
                }
            }

            return false;
        }

        public class DecryptedAadhaarResponse
        {
            public string RequestId { get; set; }
            public string Message { get; set; }
            public string ResponseCode { get; set; }
        }

    }



    public static class DateFormatter
    {
        public static DateTime ConvertString_ToValidDate(this string date)
        {
            if (date != string.Empty)
            {
                return Convert.ToDateTime(date, System.Globalization.CultureInfo.GetCultureInfo("en-GB").DateTimeFormat);
            }
            return DateTime.MaxValue;
        }
    }

    public class UnicodeFontFactory : FontFactoryImp
    {
        static string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
            "arialuni.ttf");//Server.MapPath("~/fonts/Kruti-Dev-010/k010.TTF");
        private static readonly string FontPath = "D:\\Coop_App\\fonts\\Kruti-Dev-010\\mangal_0.TTF";//  path;// Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
                                                                                                     //"arialuni.ttf");

        private readonly BaseFont _baseFont;

        public UnicodeFontFactory()
        {
            string font = HttpContext.Current.Server.MapPath("~/fonts/Kruti-Dev-010/k010.TTF");// + "ARIALUNI.TTF"; //"D:\\Coop_App\\fonts\\Kruti-Dev-010\\Sarai_07.TTF";
            _baseFont = BaseFont.CreateFont(font, BaseFont.IDENTITY_H,
                                                false);

        }

        public override iTextSharp.text.Font GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color,
          bool cached)
        {
            return new iTextSharp.text.Font(_baseFont, size, style, color);
        }
    }

    public static class DataFormatter
    {
        public static bool IsValidDateTime(string dateTime)
        {
            if (string.IsNullOrEmpty(dateTime))
                return false;

            string[] formats = { "dd/MM/yyyy" };
            DateTime parsedDateTime;
            return DateTime.TryParseExact(dateTime, formats, new CultureInfo("en-US"), DateTimeStyles.None, out parsedDateTime);
        }

        public static bool IsValidInt(string val)
        {
            if (string.IsNullOrEmpty(val))
                return false;

            int intValue = 0;
            return int.TryParse(val, out intValue);
        }

        public static bool IsValidDecimal(string val)
        {
            if (string.IsNullOrEmpty(val))
                return false;

            decimal decimalValue = 0;
            return decimal.TryParse(val, out decimalValue);
        }

        public static bool IsValidIntOrDecimal(string val)
        {
            if (string.IsNullOrEmpty(val))
                return false;

            int intValue = 0;
            decimal decimalValue = 0;
            return (int.TryParse(val, out intValue) || decimal.TryParse(val, out decimalValue)) ? true : false;
        }

        public static string posttopage(string URL, string userdetails, string AppName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");
            sb.AppendFormat(@"<body style='background-color:#F0F0F0;' onload='document.forms[""form""].submit()'>");
            sb.AppendFormat("<form name='form' action='{0}' method='post'>", URL);
            sb.AppendFormat("<div style='float:left; width:100%; height:100%;'>");
            sb.AppendFormat("<div style='float:left; width:100%; height:100%; margin-top:10%;'>    ");
            sb.AppendFormat("<div style='float:left; width:100%; text-align:center; font-size:30px; color:#525252; margin:0 0 50px 0;'>Please wait while you are being redirected to <span style='font-weight:bold;'>{0}</span> Application.</div>", AppName.ToUpper());
            sb.AppendFormat("<div style='float:left; width:100%; text-align:center;'>");
            sb.AppendFormat("<img src='/images/loading.gif'  width='350px'/>");
            sb.AppendFormat("</div>");
            sb.AppendFormat("<input type='hidden' name='userdetails' value='{0}'>", userdetails);
            sb.AppendFormat("</div>");
            sb.AppendFormat("<div>");
            sb.Append("</form>");
            sb.Append("</body>");
            sb.Append("</html>");
            return sb.ToString();
        }
    }

    //string address2 = "House : " + xDataList3[0].Attributes["house"].Value + "#LandMark : " + xDataList3[0].Attributes["lm"].Value + "#Street : " + xDataList3[0].Attributes["street"].Value + "#District: " + xDataList3[0].Attributes["dist"].Value + "#PIN: " + xDataList3[0].Attributes["pc"].Value + "#State: " + xDataList3[0].Attributes["state"].Value;
    //listx.Add(new string[] { "Success", xDataList[0].Attributes["name"].Value, xDataList[0].Attributes["gender"].Value, xDataList[0].Attributes["dob"].Value, xDataList1[0].InnerText.ToString(), xDataList2[0].Attributes["name"].Value, SSOIDORAADHAREnc, xDataList3[0].Attributes["co"].Value, xDataList2[0].Attributes["co"].Value, address, address2

    public class APIResponce
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
    public class APIPagingResponce : APIResponce
    {
        public Int32 PageNumber { get; set; }
        public Int32 PageSize { get; set; }
        public Int32 Totalpages { get; set; }
    }

    public class AadharResponce
    {
        public AadharUserInfoResponce UserInfo { get; set; }
        public AadharAddressResponce UserAddress { get; set; }
        public AadharLocationResponce UserLocation { get; set; }

    }

    public class AadharLocationResponce
    {
        public string co { get; set; }
        public string country { get; set; }
        public string dist { get; set; }
        public string house { get; set; }
        public string lang { get; set; }
        public string lm { get; set; }
        public string loc { get; set; }
        public string name { get; set; }
        public string pc { get; set; }
        public string po { get; set; }
        public string state { get; set; }
        public string street { get; set; }
        public string subdist { get; set; }
        public string vtc { get; set; }
    }

    public class AadharAddressResponce
    {

        public string co { get; set; }
        public string dist { get; set; }
        public string house { get; set; }
        public string lm { get; set; }
        public string loc { get; set; }
        public string ms { get; set; }
        public string pc { get; set; }
        public string state { get; set; }
        public string street { get; set; }
        public string vtc { get; set; }



    }

    public class AadharUserInfoResponce
    {
        public string dob { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string ms { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string photo { get; set; }
    }

    public class BRNStatus
    {
        public string Status { get; set; }
        public string BRN { get; set; }
        public string Area { get; set; }
        public string District { get; set; }
        public string Tehsil { get; set; }
        public string Village { get; set; }
        public string LocalBody { get; set; }
        public string Ward { get; set; }
        public string BO_Name { get; set; }
        public string BO_HouseNo { get; set; }
        public string BO_Lane { get; set; }
        public string BO_Locality { get; set; }
        public string BO_PinCode { get; set; }
        public string BO_TelNo { get; set; }
        public string BO_Email { get; set; }
        public string BO_PanNo { get; set; }
        public string BO_TanNo { get; set; }
        public string HO_Name { get; set; }
        public string HO_HouseNo { get; set; }
        public string HO_Lane { get; set; }
        public string HO_Locality { get; set; }
        public string HO_PinCode { get; set; }
        public string HO_TelNo { get; set; }
        public string HO_EMail { get; set; }
        public string HO_PanNo { get; set; }
        public string HO_TanNo { get; set; }
        public string NIC_Code { get; set; }
        public string Year { get; set; }
        public string Ownership { get; set; }
        public string Total_Person { get; set; }
        public string ActAuthorityRegNo { get; set; }
        public string Applicant_Name { get; set; }
        public string Applicant_No { get; set; }
        public string Applicant_EMail { get; set; }
        public string Applicant_Address { get; set; }
    }





    public class ResponseDataEsign
    {
        public string RequestId { get; set; }
        public string AppCode { get; set; }
        public string TrnNo { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }
        public string UUID { get; set; }
        public string UidToken { get; set; }
        public string VerificationType { get; set; }
        public string CO { get; set; }
        public string Pht { get; set; }
        public string Dist { get; set; }
        public string Lm { get; set; }
        public string House { get; set; }
        public string Loc { get; set; }
        public string Ms { get; set; }
        public string Pc { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
        public string Vtc { get; set; }
        public string Dob { get; set; }
        public string Gender { get; set; }
        public string Name { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Lm_Hi { get; set; }
        public string Loc_Hi { get; set; }
        public string Vtc_Hi { get; set; }
        public string Dist_Hi { get; set; }
        public string CO_Hi { get; set; }
        public string House_Hi { get; set; }
        public string Pc_Hi { get; set; }
        public string Street_Hi { get; set; }
        public string Name_Hi { get; set; }
        public string State_Hi { get; set; }
        public string Po_Hi { get; set; }
        public string AuthModality { get; set; }
    }

    public class RootEsignModel
    {
        public InputEsignJson inputJson { get; set; }
        public string filetype { get; set; }
        public string transactionid { get; set; }
        public string docname { get; set; }
        public string designation { get; set; }
        public string status { get; set; }
        public string llx { get; set; }
        public string lly { get; set; }
        public string positionX { get; set; }
        public string positionY { get; set; }
        public string mode { get; set; }
        public string page { get; set; }
    }

    public class InputEsignJson
    {
        public string File { get; set; }
    }

    public class APIResponceWithTotalRec
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public int TotalRecords { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class APILoginRequest
    {
        public string SSOID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int LoginUserType { get; set; }
    }


    public class APIResponseViewModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        [JsonIgnore]
        public int TotalRecords { get; set; }
        [JsonIgnore]
        public int PageNumber { get; set; }
        [JsonIgnore]
        public int PageSize { get; set; }

        [JsonIgnore]
        public string WebLink { get; set; }
    }
}