using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Newtonsoft.Json.Linq;
using System.Web;
using Newtonsoft.Json;
using Cooperative.Controllers;
using Cooperative.DBConnector;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using Cooperative.JanadhaarClasses;
using System.Threading.Tasks;

namespace Cooperative.Models
{
    public class csCommon
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private string KeyValue;
        private DataTable dt;
        private readonly JanAadhaarGenericService _service = new JanAadhaarGenericService();

        public csCommon(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }
        public csCommon()
        {
            objDBAccess = new DBFactoryMSSQL();
            KeyValue = string.Empty;
        }
        //public string GetURLByKey(string KeyName)
        //{
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("GetURLDataByKey", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@KeyName", DBFactoryParameterType.NVarchar, KeyName);
        //        KeyValue = "";// objDBAccess.GetStringValue();
        //    }
        //    catch (Exception ex)
        //    {
        //        DBLogger objDBLogger = new DBLogger();
        //        objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.csCommon", FunctionName = "GetURLByKey(string KeyName)" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return KeyValue;
        //}
        public string GetURLByKey(string KeyName)
        {
            DataTable dt1 = new DataTable();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetURLDataByKey", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@KeyName", DBFactoryParameterType.NVarchar, KeyName);
                // objDBAccess.GetStringValue();
                dt1 = objDBAccess.GetTable();
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    KeyValue = dt1.Rows[0]["Kval"].ToString();
                }


            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.csCommon", FunctionName = "GetURLByKey(string KeyName)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return KeyValue;
        }
        public DataTable ConvertJSONToDataTable(string json)
        {
            DataTable dt1 = JsonConvert.DeserializeObject<DataTable>(json);
            return dt1;
        }

        public static string WebRequestinJson(string url, string postData, string contenttype)
        {
            string ret = string.Empty;
            try
            {
                StreamWriter requestWriter;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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
                ILogger objFileLogger = new DBLogger();
                objFileLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.csCommon", FunctionName = "WebRequestinJson(string url, string postData, string contenttype)" }, wex);
                return ret;
            }
        }


        #region Umesh sir Janaadhar Integration
        public DataSet JanAadhaarMembersList_old(string janAadharNo, string mid = "", string _SocietyDirectory = "")
        {
            DataSet ds = new DataSet();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                HttpClient client = new HttpClient();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var url = new csCommon().GetURLByKey("JanAadhaarByBhamashah").Replace("BhamashahNo", janAadharNo);
                HttpResponseMessage response = client.GetAsync(url).Result;
                try
                {
                    if (response.StatusCode.ToString() == "OK")
                    {
                        var JanAadharDetail = response.Content.ReadAsStringAsync().Result;
                        if (JanAadharDetail.Contains("JAN_MEMBER_ID"))
                        {
                            DataTable dt = ConvertJSONToDataTable(JanAadharDetail);
                            DataColumnCollection columns = dt.Columns;
                            if (columns.Contains("JAN_AADHAR"))
                                dt.Rows[0]["JAN_AADHAR"] = COMMON_FUNCTION.Encrypt(dt.Rows[0]["JAN_AADHAR"].ToString());

                            ds.Tables.Add(dt);
                        }

                    }
                    else
                    {
                        var JanAadharDetail = "{}";

                    }
                }
                catch (Exception ex)
                {
                    DataTable dt = new DataTable("Exception");
                    dt.Columns.Add("msg");
                    if (ex.Message.Contains("Unexpected"))
                    {
                        dt.Rows.Add((dt.NewRow())["msg"] = "JanAdhaar No. Not valid");
                    }
                    else
                    {
                        dt.Rows.Add((dt.NewRow())["msg"] = ex.Message);
                    }

                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.csCommonLN-185", FunctionName = "JanAadhaarMembersList(string janAadharNo)" + janAadharNo + " " + mid }, ex);
                    ds = new DataSet();
                    ds.Tables.Add(dt);
                }

            }
            catch (Exception ex)
            {
                DataTable dt = new DataTable("Exception");
                dt.Columns.Add("msg");
                dt.Rows.Add((dt.NewRow())["msg"] = ex.Message);
                ds = new DataSet();
                ds.Tables.Add(dt);
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.csCommon", FunctionName = "JanAadhaarMembersList(string janAadharNo)" }, ex);

            }
            return ds;
        }

        public DataSet JanAadhaarMembersList(string aadharNo, string mid = "", string _SocietyDirectory = "")
        {
            DataSet ds = new DataSet();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                HttpClient client = new HttpClient();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var url = "https://mahilanidhiapp.rajasthan.gov.in/api/Common/GetVIDByAadhar/" + aadharNo;
                HttpResponseMessage response = client.GetAsync(url).Result;
                try
                {
                    if (response.StatusCode.ToString() == "OK")
                    {
                        var JanAadharDetail = response.Content.ReadAsStringAsync().Result;
                        if (JanAadharDetail.Contains("Data"))
                        {
                            var result = JsonConvert.DeserializeObject<CommonDataModel>(JanAadharDetail);
                            DataTable dt = new DataTable("dt");
                            if (!dt.Columns.Contains("AADHAR_REF_NO"))
                                dt.Columns.Add("AADHAR_REF_NO", typeof(String));

                            if (!dt.Columns.Contains("ENC_AADHAR_REF_NO"))
                                dt.Columns.Add("ENC_AADHAR_REF_NO", typeof(String));
                            dt.Rows.Add(result.Data, COMMON_FUNCTION.Encrypt(result.Data));
                            ds.Tables.Add(dt);
                        }

                    }
                    else
                    {
                        var JanAadharDetail = "{}";

                    }
                }
                catch (Exception ex)
                {
                    DataTable dt = new DataTable("Exception");
                    dt.Columns.Add("msg");
                    if (ex.Message.Contains("Unexpected"))
                    {
                        dt.Rows.Add((dt.NewRow())["msg"] = "AadharNo No. Not valid");
                    }
                    else
                    {
                        dt.Rows.Add((dt.NewRow())["msg"] = ex.Message);
                    }

                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.csCommon", FunctionName = "JanAadhaarMembersList(string AadharNo)" + aadharNo + " " + mid }, ex);
                    ds = new DataSet();
                    ds.Tables.Add(dt);
                }

            }
            catch (Exception ex)
            {
                DataTable dt = new DataTable("Exception");
                dt.Columns.Add("msg");
                dt.Rows.Add((dt.NewRow())["msg"] = ex.Message);
                ds = new DataSet();
                ds.Tables.Add(dt);
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.csCommon", FunctionName = "JanAadhaarMembersList(string janAadharNo)" }, ex);

            }
            return ds;
        }

        public async Task<DataTable> GetJanAadhaarMIDByAadhaar(string aadhaarNo)
        {
            try
            {
                aadhaarNo = csCommon.Decrypt_SA(Uri.UnescapeDataString(aadhaarNo));
                string DecResponseString = await _service.getResponseData("COOP", "JanAadaarDetailByAadhaar", aadhaarNo, "", "", "");

                JanaadharByAadharRootResponse parsedResponse = JsonConvert.DeserializeObject<JanaadharByAadharRootResponse>(DecResponseString);
                JanAadhaarDetails janAadhaarDtl = new JanAadhaarDetails();
                if (parsedResponse != null && parsedResponse.response.status)
                {
                    string result = JsonConvert.SerializeObject(parsedResponse.response.data);
                    if (parsedResponse.response.data != null && parsedResponse.response.data.Count > 0)
                    {
                        DataTable dt = ConvertJSONToDataTable(result);
                        DataColumnCollection columns = dt.Columns;
                        //if (dt.Columns.Contains("JAN_AADHAR"))
                        //dt.Rows[0]["JAN_AADHAR"] = COMMON_FUNCTION.Encrypt(dt.Rows[0]["JAN_AADHAR"].ToString());
                        if (!dt.Columns.Contains("ENC_JAN_AADHAR"))
                        {
                            dt.Columns.Add("ENC_JAN_AADHAR", typeof(string));
                            dt.Rows[0]["ENC_JAN_AADHAR"] = COMMON_FUNCTION.Encrypt(dt.Rows[0]["JAN_AADHAR"].ToString());
                        }
                        if (!dt.Columns.Contains("JAN_MEMBER_ID"))
                        {
                            dt.Columns.Add("JAN_MEMBER_ID", typeof(string));
                            dt.Rows[0]["JAN_MEMBER_ID"] = dt.Rows[0]["MEMBER_ID"].ToString();
                        }
                        if (!dt.Columns.Contains("AADHAR_REF_NO"))
                        {
                            dt.Columns.Add("AADHAR_REF_NO", typeof(string));
                            dt.Rows[0]["AADHAR_REF_NO"] = dt.Rows[0]["MEMBER_ID"].ToString();
                        }

                        return dt;

                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.csCommon", FunctionName = "GetJanAadhaarMIDByAadhaar(string aadhaarNo)" }, ex);

            }
            return null;
        }
        public static DataSet SendOtpByAadharNo_KYC(string janId, string MemberID)
        {
            string JanAdhaarClientID = "b10d20ae-367c-4cab-8b14-49330d4a392d";
            DataSet objdsotp = new DataSet();
            if (janId.Length > 20)
                janId = COMMON_FUNCTION.Decrypt(janId);
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string Scheme = "CEA";
                var request = (HttpWebRequest)WebRequest.Create("https://api.sewadwaar.rajasthan.gov.in/app/live/Janaadhaar/Prod/Service/Gen/Otp/For?client_id=" + JanAdhaarClientID);
                request.Method = "POST";
                string postData = "<root><genOtpFor><janaadharId>" + janId + "</janaadharId><mid>" + MemberID + "</mid><schemeId>" + Scheme + "</schemeId></genOtpFor></root>";
                byte[] byteArray = Encoding.ASCII.GetBytes(postData);
                request.ContentType = "text/xml;charset=utf-8";
                request.ContentLength = byteArray.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(byteArray, 0, byteArray.Length);
                requestStream.Close();

                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();//<root><status>true</status><tid>174196469</tid></root>

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
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.csCommon", FunctionName = "SendOtpByAadharNo_KYC(string janId, string MemberID)" }, ex);
            }


            return objdsotp;
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
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.csCommon", FunctionName = "ConvertXMLResponceToDataSet(string xmlData)" }, ex);

                return null;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }
        public static class GlobleKeys
        {

            public static readonly string AadharAuthLicenseKey = "MDAW2xSJCuC-htWkTHcw9LTYXs1rl8kyfygbLztYldZocGJR6DDtE5A";//"MKmyGwbThaYG35Ahinwx35nLtBYXrNMP4ejWD7A9-x6InP7y4xLROXU";
                                                                                                                           //public static readonly string AadharAuthLicenseKey = "MJSazxO49Eh5vQ2BlcbUG--uNQ4tCpqKPF-OoFa0BZo0CE4CDBBtXVA";//"MKmyGwbThaYG35Ahinwx35nLtBYXrNMP4ejWD7A9-x6InP7y4xLROXU";
            public static readonly string AadharAuthClientId = "0c8fe990-3b61-406d-b185-0fafc80e73d7";
            public static readonly string AadharAuthSUBAUS = "PRCIS22869"; //"PNSCL22866";
            public static readonly string Certificatename = "uidai_auth_prod.cer";

            public static readonly string AadhaarDSMDETOKANIZEV2URL = "https://api.sewadwaar.rajasthan.gov.in/app/live/Aadhaar/Prod/detokenizeV2/doitAadhaar/encDec/demo/hsm/auth";
            public static readonly string SendOTPForAadharAuthURL = "https://api.sewadwaar.rajasthan.gov.in/app/live/was/otp/request/prod";
            public static readonly string VerifyOTPForAadharAuthURL = "https://api.sewadwaar.rajasthan.gov.in/app/live/was/kyc/otp/prod";
            public static readonly string AadharAuthEsignClientId = "02a79073-c2db-4c84-b810-885fdaa8c54a";
            public static readonly string SendOTPForAadharAuthEsignURL = "https://api.sewadwaar.rajasthan.gov.in/app/live/RajeSign/Prod/all/webresources/generic/sendOTP/Aadhar";
            public static readonly string VerifyOTPForAadharAuthEsignURL = "https://api.sewadwaar.rajasthan.gov.in/app/live/RajeSign/Prod/all/webresources/generic/authOTP/Aadhar";
            public static readonly string EsignPDFURL = "https://api.sewadwaar.rajasthan.gov.in/app/live/rajesign/Prod/Service/all/webresources/generic/esign/Doc";

            public static readonly string JaAdharMemberListURL = "https://api.sewadwaar.rajasthan.gov.in/app/live/Janaadhaar/Prod/Service/action/fetchJayFamily";
            public static readonly string JaAdharMemberDetailsURL = "https://api.sewadwaar.rajasthan.gov.in/app/live/Janaadhaar/Prod/Service/Info/Fetch";
            public static readonly string JanAadharAuthClientId = "f6de7747-60d3-4cf0-a0ae-71488abd6e95";
            public static readonly string JanAadhaarMemberPhotoURL = "f6de7747-60d3-4cf0-a0ae-71488abd6e95";
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

        public static DataTable VerifyAadhaarOTP(string OTP, string TXN)
        {
            ILogger objFileLogger = new DBLogger();
            DataTable apiResponce = new DataTable();
            string message = string.Empty;
            string status = "";
            string TransactionId = "";
            var decryOTP = OTP;
            var decryTXN = TXN;
            if (TXN != "")
            {
                try
                {
                    var KeyValue = "https://api.sewadwaar.rajasthan.gov.in/app/live/was/kyc/otp/prod?client_id=0c8fe990-3b61-406d-b185-0fafc80e73d7";
                    string json = "{\"otp\":\"" + decryOTP + "\",\"transactionid\":\"" + decryTXN + "\"}";
                    string ss = WebRequestinJson(KeyValue, json, "application/json");
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
                        message = "Success";
                        apiResponce.Rows[0]["Column1"] = "Success";
                    }
                    else
                    {
                        message = "Invalid OTP!";
                        apiResponce.Rows[0]["Column1"] = "Success";
                    }
                }
                catch (Exception ex)
                {
                    objFileLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooprative.Models.csCommon", FunctionName = "VerifyAadhaarOTPWithoutEncypt(string OTP, string TXN, string KeyName)" }, ex);
                    message = ex.Message;
                }

            }
            return apiResponce;
        }
        public static DataTable ValidateAadharOTP(string tid, string otp)
        {


            string JanAdhaarClientID = "b10d20ae-367c-4cab-8b14-49330d4a392d";
            DataSet objdsotp = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string Scheme = "CEA";
                var request = (HttpWebRequest)WebRequest.Create("https://api.sewadwaar.rajasthan.gov.in/app/live/Janaadhaar/Prod/Service/Validate/Otp?client_id=" + JanAdhaarClientID);
                request.Method = "POST";
                string postData = "<root><validateOtp><tid>" + tid + "</tid><otp>" + otp + "</otp><schemeId>" + Scheme + "</schemeId></validateOtp></root>";
                byte[] byteArray = Encoding.ASCII.GetBytes(postData);
                request.ContentType = "text/xml;charset=utf-8";
                request.ContentLength = byteArray.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(byteArray, 0, byteArray.Length);
                requestStream.Close();

                Sp_callTrackingInsert("ValidateAadharOTP(string tid, string otp) postData:- ", postData);

                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                Sp_callTrackingInsert("ValidateAadharOTP(string tid, string otp) :- response", responseString.ToString());

                int foundS1 = responseString.IndexOf("<root>");
                responseString = responseString.Substring(foundS1);

                objdsotp = ConvertXMLResponceToDataSet(responseString);
                dt = objdsotp.Tables[0];
                StreamReader respStream = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);
                string receivedResponse = respStream.ReadToEnd();
                respStream.Close();
                response.Close();

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.csCommon", FunctionName = "ValidateAadharOTP(string tid, string otp)" }, ex);
            }

            return dt;
            //DataTable apiResponce = new DataTable();
            //AadharResponce Responce = new AadharResponce();
            //Responce.UserAddress = new AadharAddressResponce();
            //Responce.UserInfo = new AadharUserInfoResponce();
            //Responce.UserLocation = new AadharLocationResponce();
            //try
            //{


            //    string auacode = GlobleKeys.AadharAuthSUBAUS;
            //    string lickey = GlobleKeys.AadharAuthLicenseKey;
            //    string client_id = GlobleKeys.AadharAuthClientId;

            //    string Certificatename = GlobleKeys.Certificatename;

            //    string url = GlobleKeys.VerifyOTPForAadharAuthURL + "?client_id=" + client_id;//new

            //    System.Web.HttpContext context = System.Web.HttpContext.Current;
            //    //string CertificatePath = context.Server.MapPath(Certificatename);
            //    string pathString = "~/Assests/";
            //    string CertificatePath = System.Web.HttpContext.Current.Server.MapPath(pathString + Certificatename);

            //    //Creating PID XML and Generate in UTF-8 Byte of that XML.
            //    var tts = DateTime.UtcNow.ToString("yyyy-MM-ddTHH\\:mm\\:ss");
            //    string msg = "<Pid ts=\"" + tts + "\" ver=\"2.0\" wadh=\"DEL9Vn2tNcxY/T3g9Jf6F/0Ne43ufdO6AaBClCWkCiQ=\"><Pv otp=\"" + otp + "\" /></Pid>";

            //    string expiryDate = string.Empty;
            //    string encSessionKey = string.Empty;
            //    string encryptedHmacBytes = string.Empty;
            //    string encXMLPIDDataNew = GetEncryptedPIDXmlNew(msg, CertificatePath, tts, ref expiryDate, ref encSessionKey, ref encryptedHmacBytes);

            //    //V.2.5
            //    string authXML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><authrequest uid=\"" + adhar + "\" tid=\"\" subaua=\"" + auacode + "\" bt=\"OTP\" lk=\"" + lickey + "\" rc=\"Y\" ra=\"O\" lr=\"Y\" de=\"N\" pfr=\"N\" mec=\"Y\" ver=\"2.5\" txn=\"" + tid + "\" dpID=\"\" rdsID=\"\" rdsVer=\"\" dc=\"\" mi=\"\" mc=\"\" deviceSrNO=\"NA\" deviceError=\"NA\" ip=\"NA\" fdc=\"NA\" idc=\"NC\" macadd=\"NA\" lot=\"P\" lov=\"302005\" ><deviceInfo fType=\"NA\" iCount=\"NA\" pCount=\"NA\" errCodeRDS=\"NA\" errInfoRDS=\"NA\" fCount=\"NA\" nmPoints=\"NA\" qScore=\"NA\" srno=\"NA\" deviceError=\"NA\" /><Skey ci=\"" + expiryDate + "\">" + encSessionKey + "</Skey><Hmac>" + encryptedHmacBytes + "</Hmac><Data type=\"X\">" + encXMLPIDDataNew + "</Data></authrequest>";


            //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //    byte[] bytes;
            //    bytes = System.Text.Encoding.ASCII.GetBytes(authXML);
            //    request.ContentType = "Application/xml";
            //    request.ContentLength = bytes.Length;
            //    request.Method = "POST";
            //    request.Headers["appname"] = "CO-Operative";

            //    Stream requestStream = request.GetRequestStream();
            //    requestStream.Write(bytes, 0, bytes.Length);
            //    requestStream.Close();
            //    HttpWebResponse response;
            //    response = (HttpWebResponse)request.GetResponse();
            //    if (response.StatusCode == HttpStatusCode.OK)
            //    {
            //        Stream responseStream = response.GetResponseStream();
            //        string results = new StreamReader(responseStream).ReadToEnd();
            //        // UpdateIdentifyMessage(results, null);

            //        //CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            //        //objCitizenModel.InsertAutharServeise(adhar, authXML, results);

            //        XmlDocument xml = new XmlDocument();
            //        xml.LoadXml(results); // suppose that myXmlString contains "<Names>...</Names>"
            //        XmlNodeList xnList = xml.SelectNodes("//authresponse/auth");
            //        string sa = "";
            //        string txnid = "";
            //        foreach (XmlNode xn in xnList)
            //        {
            //            sa = xn.Attributes["status"].Value;
            //            //txnid = xn.Attributes["txn"].Value;

            //        }
            //        if (sa.ToUpper() == "Y")
            //        {
            //            apiResponce.Rows[0]["Column1"] = "Success";
            //            string SSOIDORAADHAREnc = Encrypt(adhar);
            //            List<string[]> listx = new List<string[]>();
            //            XmlNodeList xDataList = xml.SelectNodes("//authresponse/UidData/pi");
            //            XmlNodeList xDataList1 = xml.SelectNodes("//authresponse/UidData/pht");
            //            XmlNodeList xDataList2 = xml.SelectNodes("//authresponse/UidData/LData");
            //            XmlNodeList xDataList3 = xml.SelectNodes("//authresponse/UidData/pa");
            //            string address = "House : " + xDataList3[0].Attributes["house"].Value + ", LandMark : " + xDataList3[0].Attributes["lm"].Value + ", Street : " + xDataList3[0].Attributes["street"].Value + ", District: " + xDataList3[0].Attributes["dist"].Value + ", PIN: " + xDataList3[0].Attributes["pc"].Value + ", State: " + xDataList3[0].Attributes["state"].Value;
            //            string address2 = "House : " + xDataList3[0].Attributes["house"].Value + "#LandMark : " + xDataList3[0].Attributes["lm"].Value + "#Street : " + xDataList3[0].Attributes["street"].Value + "#District: " + xDataList3[0].Attributes["dist"].Value + "#PIN: " + xDataList3[0].Attributes["pc"].Value + "#State: " + xDataList3[0].Attributes["state"].Value;
            //            listx.Add(new string[] { "Success", xDataList[0].Attributes["name"].Value, xDataList[0].Attributes["gender"].Value, xDataList[0].Attributes["dob"].Value, xDataList1[0].InnerText.ToString(), xDataList2[0].Attributes["name"].Value, SSOIDORAADHAREnc, xDataList3[0].Attributes["co"].Value, xDataList2[0].Attributes["co"].Value, address, address2 });


            //            Responce.UserInfo = new AadharUserInfoResponce() { name = xDataList[0].Attributes["name"].Value, gender = xDataList[0].Attributes["gender"].Value, photo = xDataList1[0].InnerText.ToString(), dob = xDataList[0].Attributes["dob"].Value, email = xDataList[0].Attributes["email"].Value, ms = xDataList[0].Attributes["ms"].Value, phone = xDataList[0].Attributes["phone"].Value };
            //            Responce.UserAddress = new AadharAddressResponce() { dist = xDataList3[0].Attributes["dist"].Value, house = xDataList3[0].Attributes["house"].Value, lm = xDataList3[0].Attributes["lm"].Value, pc = xDataList3[0].Attributes["pc"].Value, state = xDataList3[0].Attributes["state"].Value, street = xDataList3[0].Attributes["street"].Value, ms = xDataList3[0].Attributes["ms"].Value, co = xDataList3[0].Attributes["co"].Value, loc = xDataList3[0].Attributes["loc"].Value, vtc = xDataList3[0].Attributes["vtc"].Value };
            //            Responce.UserLocation = new AadharLocationResponce() { dist = xDataList2[0].Attributes["dist"].Value, house = xDataList2[0].Attributes["house"].Value, lm = xDataList2[0].Attributes["lm"].Value, pc = xDataList2[0].Attributes["pc"].Value, state = xDataList2[0].Attributes["state"].Value, street = xDataList2[0].Attributes["street"].Value, country = xDataList2[0].Attributes["country"].Value, co = xDataList2[0].Attributes["co"].Value, loc = xDataList2[0].Attributes["loc"].Value, vtc = xDataList2[0].Attributes["vtc"].Value, lang = xDataList2[0].Attributes["lang"].Value, name = xDataList2[0].Attributes["name"].Value, po = xDataList2[0].Attributes["po"].Value, subdist = xDataList2[0].Attributes["subdist"].Value };

            //            apiResponce.Rows[0]["Column2"] = Responce;

            //        }
            //        else
            //        {
            //            XmlNodeList xDataList = xml.SelectNodes("//authresponse/message");
            //            apiResponce.Rows[0]["Column1"] = "Failed";
            //            apiResponce.Rows[0]["Column3"] = xDataList[0].InnerText;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    apiResponce.Rows[0]["Column1"] = "Failed";
            //    apiResponce.Rows[0]["Column3"] = ex.ToString();
            //}
            //return apiResponce;
        }

        //Add umesh fun jan adhr details
        public DataTable GetJanAadharMembersDetail(string JanAadharid = "", string enrId = "", string janmemidselected = "", string SocietyDirectory = "")
        {
            if (JanAadharid.Length > 20)
                JanAadharid = COMMON_FUNCTION.Decrypt(JanAadharid);
            if (enrId.Length > 20)
                enrId = COMMON_FUNCTION.Decrypt(enrId);
            DataTable dt1 = new DataTable();
            try
            {
                csCommon data = new csCommon();
                string Message = "";
                string Status = "";
                string scheme = string.Empty;
                string client_id = "f6de7747-60d3-4cf0-a0ae-71488abd6e95";
                string Url = "https://api.sewadwaar.rajasthan.gov.in/app/live/Janaadhaar/Prod/Service/Info/Fetch" + "?client_id=" + client_id;

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

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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
                            DataRow dr = dt1.NewRow();
                            dt1.Columns.Add("Column1"); dt1.Columns.Add("Column2"); dt1.Columns.Add("Column3"); dt1.Columns.Add("Column4"); dt1.Columns.Add("Column5"); dt1.Columns.Add("Column6");
                            dt1.Columns.Add("Column7"); dt1.Columns.Add("Column8"); dt1.Columns.Add("Column9"); dt1.Columns.Add("Column10"); dt1.Columns.Add("Column11"); dt1.Columns.Add("Column12");
                            foreach (XmlNode node in nodeList)
                            {
                                janmemid = node.SelectSingleNode("jan_mid").InnerXml;
                                if (janmemid == "0")
                                {
                                    janmemid = node.SelectSingleNode("hof_jan_m_id").InnerXml;
                                }


                                Status = "OK";
                                if (janmemidselected == janmemid)
                                {
                                    dr["Column1"] = "Success";
                                    dr["Column2"] = node.SelectSingleNode("nameEng").InnerXml;
                                    dr["Column6"] = node.SelectSingleNode("nameHnd").InnerXml;
                                    dr["Column8"] = node.SelectSingleNode("fnameEng").InnerXml;
                                    dr["Column9"] = node.SelectSingleNode("fnameHnd").InnerXml;
                                    dr["Column4"] = node.SelectSingleNode("dob").InnerXml;
                                    dr["Column3"] = node.SelectSingleNode("gender").InnerXml == "Male" ? "M" : "F";
                                    dr["Column5"] = JanAadhaarMemberPhoto(JanAadharid, janmemid, SocietyDirectory);
                                    dr["Column7"] = Encrypt(JanAadharid);
                                }

                                //dt.Rows[0]["Column2"] = node.SelectSingleNode("nameEng").InnerXml;
                                //dt.Rows[0]["Column6"] = node.SelectSingleNode("nameHnd").InnerXml;
                                //dt.Rows[0]["Column8"] = node.SelectSingleNode("fnameEng").InnerXml;
                                //dt.Rows[0]["Column9"] = node.SelectSingleNode("fnameHnd").InnerXml;
                                //dt.Rows[0]["Column1"] = node.SelectSingleNode("dob").InnerXml;
                                //dt.Rows[0]["Column3"] = node.SelectSingleNode("gender").InnerXml;
                                //dt.Rows[0]["Column12"] = node.SelectSingleNode("mobile").InnerXml;


                            }
                            XmlNodeList nodeList1 = Dtlist2.SelectNodes("//family/familydetail");

                            foreach (XmlNode node1 in nodeList1)
                            {
                                //dt.Caste = node1.SelectSingleNode("caste").InnerXml;
                                //dt.Category = node1.SelectSingleNode("category").InnerXml;;
                                //dt.Rows[0]["Column10"] = node1.SelectSingleNode("addressEng").InnerXml;
                                //dt.Rows[0]["Column11"] = node1.SelectSingleNode("addressHnd").InnerXml;

                                dr["Column10"] = node1.SelectSingleNode("addressEng").InnerXml;
                                dr["Column11"] = node1.SelectSingleNode("addressHnd").InnerXml;



                            }
                            dt1.Rows.Add(dr);
                        }
                        else
                        {
                            Status = "";
                            Message = "Member details not found";
                        }
                    }

                }
                if (Status == "")
                {
                    Message = "Member details not found";
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.csCommon", FunctionName = "GetJanAadharMembersDetail(string JanAadharid = \"\", string enrId = \"\", string janmemidselected = \"\")" }, ex);
            }

            return dt1;
        }

        public static string JanAadhaarMemberPhoto(string JanaadharNo, string MemberId = "", string _SocietyDirectory = "")
        {
            string photo;
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string client_id = "0cbe6b41-c050-44a5-a3b7-3ec37f27b624";
                string Url = "https://api.sewadwaar.rajasthan.gov.in/app/live/Janaadhaar/Prod/Service/details/hofAndMemberPhoto/" + JanaadharNo + "/" + MemberId + "?client_id=" + client_id;
                var request = (HttpWebRequest)WebRequest.Create(Url);
                var response = (HttpWebResponse)request.GetResponse();
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseString);
                photo = ((Newtonsoft.Json.Linq.JContainer)(values["member_Photo"])).First.First.First.ToString();
                string fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper();

                string path = _SocietyDirectory.Length > 1 ? "../Content/SocietyProfiling/" + _SocietyDirectory : "../Content/NewRegistration/";
                fileName = COMMON_FUNCTION.GetUniqueFileName("Member_" + fileName, System.Web.HttpContext.Current.Server.MapPath(path), ".jpg").ToLower();

                if (!System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(path.Replace("..", "~"))))
                {
                    System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(path.Replace("..", "~")));
                }


                if (!System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(path.Replace("..", "~"))))
                {
                    System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(path.Replace("..", "~")));
                }

                FileStream fs = new FileStream(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(path.Replace("..", "~")), fileName + ".jpg"), FileMode.CreateNew, FileAccess.Write);
                byte[] imageBytes = Convert.FromBase64String(photo);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                ms.WriteTo(fs);
                fs.Close();
                fs.Dispose();
                ms.Flush();
                // ds.Tables[0].Rows[0]["Column5"] = _SocietyDirectory.Length > 1 ? "../Content/SocietyProfiling/" + _SocietyDirectory + "/" + fileName + ".jpg" : "../Content/NewRegistration/" + fileName + ".jpg";
                photo = _SocietyDirectory.Length > 1 ? "../Content/SocietyProfiling/" + _SocietyDirectory + "/" + fileName + ".jpg" : "../Content/NewRegistration/" + fileName + ".jpg";
                return photo;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.csCommon", FunctionName = "JanAadhaarMemberPhoto(string JanaadharNo, string MemberId = \"\", string _SocietyDirectory = \"\")" }, ex);

                return "";
            }
        }
        #endregion


        //pradeep 2024-11-14

        public static string Decrypt_SA(string encryptedText)
        {
            if (!encryptedText.Contains("Encrypt:"))
            {
                return encryptedText;
            }
            else
            {
                encryptedText = encryptedText.Replace("Encrypt:", "");
            }

            string secretKey = "W78K87BJ";
            byte[] keyArray;
            using (SHA256 sha256 = SHA256.Create())
            {
                keyArray = sha256.ComputeHash(Encoding.UTF8.GetBytes(secretKey));
            }

            // Decode the Base64 encrypted string to get the byte array
            byte[] fullCipher = Convert.FromBase64String(encryptedText);

            // Extract IV (first 16 bytes) and ciphertext (remaining bytes)
            byte[] iv = new byte[16];
            byte[] cipherText = new byte[fullCipher.Length - iv.Length];

            Array.Copy(fullCipher, iv, iv.Length);
            Array.Copy(fullCipher, iv.Length, cipherText, 0, cipherText.Length);

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyArray;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aes.CreateDecryptor();
                byte[] resultArray = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);

                return Encoding.UTF8.GetString(resultArray);
            }
        }
        public static string Encrypt_SA(string plainText)
        {
            if (plainText.Contains("Encrypt:"))
            {
                return plainText;
            }

            string secretKey = "W78K87BJ"; // Same key as used in Decrypt_SA
            byte[] keyArray;

            // Generate the SHA-256 hash of the key
            using (SHA256 sha256 = SHA256.Create())
            {
                keyArray = sha256.ComputeHash(Encoding.UTF8.GetBytes(secretKey));
            }

            // Create a new IV for encryption
            byte[] iv;
            using (Aes aes = Aes.Create())
            {
                aes.GenerateIV();
                iv = aes.IV;
            }

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyArray;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aes.CreateEncryptor();
                byte[] cipherText = encryptor.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);

                // Combine IV and cipherText
                byte[] result = new byte[iv.Length + cipherText.Length];
                Array.Copy(iv, 0, result, 0, iv.Length);
                Array.Copy(cipherText, 0, result, iv.Length, cipherText.Length);

                // Return Base64 encoded result
                return "Encrypt:" + Convert.ToBase64String(result);
            }

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
        public static string GetEncDataForEsign(string hdn_aadhar)
        {
            //hdn_aadhar = "409032098170";
            //hdn_aadhar = "420928155572";
            //string url = "https://aadhaarauthtest.rajasthan.gov.in/AadhaarAuth";
            //string url = "https://aadhaarauth.rajasthan.gov.in/AadhaarAuth";

            BRNEntity result = new BRNEntity();
            string _txnid = "";
            string ResponseCode = "";
            string status = "";
            string errormsg = string.Empty;
            string succmsg = string.Empty;
            if (HttpContext.Current.Request.Url.Host.Contains("localhost") || HttpContext.Current.Request.Url.Host.Contains("devitsandbox"))
            {
                //hdn_aadhar = "9887356163";
                hdn_aadhar = "420928155572";
            }
            try
            {
                if (!string.IsNullOrEmpty(hdn_aadhar))
                {
                    string AppCode = "PRAVL22868ABFAP012";
                    string SSOToken = System.Web.HttpContext.Current.Session.SessionID;
                    Random rnd1 = new Random();
                    var iRnd1 = rnd1.Next(10000000, 99999999);
                    string RequestId = "PRAVL22868" + iRnd1.ToString();
                    //string ReturnURL = HttpContext.Current.Request.UrlReferrer.ToString();
                    //string ReturnURL = "https://rajsahakarapp.rajasthan.gov.in/pldb/biokyc";
                    string ReturnURL = "https://rajsahakarapp.rajasthan.gov.in/AadhaarEsignAuth/AadharAuthLanding";

                    if (HttpContext.Current.Request.Url.Host.Contains("10.70.234.208"))
                    {
                        AppCode = "PRAVL22868ABFAP021";
                        ReturnURL = "http://10.70.234.208/rajsahakarapp/AadhaarEsignAuth/AadharAuthLanding";
                    }

                    string encAadharNo = hdn_aadhar.ToString(); // EncryptWithKey(hdn_aadhar.ToString(), GlobalConstant.AadharAuthEsigEncKey_Test, GlobalConstant.AadharAuthEsigAppCode_Test);
                    string checksumStr = AppCode + RequestId + encAadharNo + "3";
                    byte[] inputBytes = Encoding.UTF8.GetBytes(checksumStr);
                    string Checksum = ComputeSha256Hash(inputBytes);
                    using (WebClient Client = new WebClient())
                    {
                        try
                        {
                            string data_old = JsonConvert.SerializeObject(new
                            {
                                SSOToken = SSOToken,
                                AppCode = AppCode,
                                RequestId = RequestId,
                                AadhaarNo = hdn_aadhar.ToString(),
                                ReturnURL = ReturnURL,
                                Purpose = "For Esign Authentication.",
                                RequestType = 3,
                                Checksum = Checksum,
                                VerificationType = 0,
                                OperatorIdType = 0,
                                OperatorId = "2",
                            });

                            string Address = JsonConvert.SerializeObject(new
                            {
                                Co = "",
                                House = "",
                                Street = "",
                                Lm = "",
                                Loc = "",
                                Vtc = "",
                                Subdist = "",
                                Dist = "",
                                Pc = "",
                                Po = ""
                            });
                            string UserDetails = JsonConvert.SerializeObject(new
                            {

                                Name = "",
                                LastName = "",
                                Dob = "",
                                Gender = "",
                                Age = "",
                                Phone = "",
                                Email = "",
                            });


                            string data = JsonConvert.SerializeObject(new
                            {
                                SSOToken = SSOToken,
                                AppCode = AppCode,
                                RequestId = RequestId,
                                AadhaarNo = hdn_aadhar.ToString(),
                                RequestType = 3,
                                ReturnURL = ReturnURL,
                                Checksum = Checksum,
                                VerificationType = 0,
                                OperatorIdType = 0,
                                OperatorId = "2",
                                Purpose = "For Esign Authentication.",
                                AuthModality = "1",

                            });

                            //string encData = EsignAesEncryption.Encrypt(data, GlobalConstant.AadharAuthEsigEncKey_Test);

                            string encData = "";
                            EncryptionResult e = new EncryptionResult();

                            if (HttpContext.Current.Request.Url.Host.Contains("10.70.234.208"))
                                e = EsignAesEncryption.Encrypt_New(data, GlobalConstant.AadharAuthEsigEncKey_Test_stagging);
                            else
                                e = EsignAesEncryption.Encrypt_New(data, GlobalConstant.AadharAuthEsigEncKey_Test);


                            if (e.IsSuccess)
                            {
                                encData = Convert.ToBase64String(e.EncriptedData);
                            }
                            else
                            {
                                encData = "NO" + "#" + e.Message.ToString();
                            }

                            // Sp_callTrackingInsert("csCommon GetEncDataForEsign :-", encData);
                            var JsonData = new
                            {
                                EncData = encData,
                                RequestID = RequestId
                            };

                            // Convert object to JSON string
                            string jsonString = JsonConvert.SerializeObject(JsonData);


                            return jsonString;

                        }
                        catch (Exception ex)
                        {
                            ILogger objILogger = new DBLogger();
                            objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Model.csCommon", FunctionName = "GetEncDataForEsign(string hdn_aadhar)" }, ex);
                            _txnid = "NO" + "#" + ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Model.csCommon", FunctionName = "GetEncDataForEsign(string hdn_aadhar)" }, ex);
                _txnid = "NO" + "#" + ex.Message;

            }
            return _txnid;
        }

        public static void Sp_callTrackingInsert(string function_name, string msj)
        {
            if (msj == null) { msj = ""; }
            DBFactoryMSSQL conn = new DBFactoryMSSQL();
            SqlConnection sqlConnection = conn.GetConnection;
            sqlConnection.Open();
            try
            {
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

        public string InsertAadhaarServiceLogs(string aadhaar, string servicetype, string requestxml, string responsexml, string txnid, string status, string errorcode, string modulename)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertAadhaarServiceLogs", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@aadhaar", DBFactoryParameterType.NVarchar, aadhaar);
                objDBAccess.AddParameterToCommand("@servicetype", DBFactoryParameterType.NVarchar, servicetype);
                objDBAccess.AddParameterToCommand("@requestxml", DBFactoryParameterType.NVarchar, requestxml);
                objDBAccess.AddParameterToCommand("@responsexml", DBFactoryParameterType.NVarchar, responsexml);
                objDBAccess.AddParameterToCommand("@txnid", DBFactoryParameterType.NVarchar, txnid);
                objDBAccess.AddParameterToCommand("@status", DBFactoryParameterType.NVarchar, status);
                objDBAccess.AddParameterToCommand("@errorcode", DBFactoryParameterType.NVarchar, errorcode);
                objDBAccess.AddParameterToCommand("@modulename", DBFactoryParameterType.NVarchar, modulename);

                KeyValue = objDBAccess.GetStringValueNew();


            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                //objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.csCommon", FunctionName = "InsertAadhaarServiceLogs(string aadhaar, string servicetype, string requestxml, string responsexml, string txnid, string status) " }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return KeyValue;
        }

        public string InsertTraceLogs(string pageName, string functionName, string variableName, string errorMessage, string errorLocation, string stackTrace, string statusId)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertTraceLogging", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@pageName", DBFactoryParameterType.NVarchar, pageName);
                objDBAccess.AddParameterToCommand("@functionName", DBFactoryParameterType.NVarchar, functionName);
                objDBAccess.AddParameterToCommand("@variableName", DBFactoryParameterType.NVarchar, variableName);
                objDBAccess.AddParameterToCommand("@errorMessage", DBFactoryParameterType.NVarchar, errorMessage);
                objDBAccess.AddParameterToCommand("@errorLocation", DBFactoryParameterType.NVarchar, errorLocation);
                objDBAccess.AddParameterToCommand("@stackTrace", DBFactoryParameterType.NVarchar, stackTrace);
                objDBAccess.AddParameterToCommand("@statusId", DBFactoryParameterType.NVarchar, statusId);

                KeyValue = objDBAccess.GetStringValueNew();


            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return KeyValue;
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
        public string UpdateDcrypted(List<SaveDcrypted> objList)
        {
            try
            {
                objDBAccess.OpenConnection();
                foreach (SaveDcrypted obj in objList)
                {
                    SqlCommand cmd = objDBAccess.CreateCommand("sp_UpdateDecrptedJanNo", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@PK_ID", DBFactoryParameterType.Int, obj.PK_ID);
                    objDBAccess.AddParameterToCommand("@DecryptJanAadharNo", DBFactoryParameterType.NVarchar, obj.ColumnValueName);
                    objDBAccess.ExecuteScalarCommand();
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.csCommon", FunctionName = "UpdateReferenceDptUser(List<SaveReferencebyUID> objList)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return KeyValue;
        }

        #region --START-- Dyanamically_Convertion_Response_SP_Object_Convert_Into_API_Json_List_View
        /// <summary>
        /// Common Method for dynamically Convert Data Table to List type Variable as per column name match.
        /// Created By : RK
        /// Created On : 2026-04-09
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ConvertDataTableToList<T>(DataTable dt) where T : new()
        {
            List<T> result = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                T obj = new T();
                foreach (DataColumn column in dt.Columns)
                {
                    // Find a property in T that matches the column name
                    var property = typeof(T).GetProperty(column.ColumnName);
                    if (property != null && row[column] != DBNull.Value)
                    {
                        // Convert the value to the property type and set it
                        property.SetValue(obj, Convert.ChangeType(row[column], Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType));
                    }
                }
                result.Add(obj);
            }

            return result;
        }

        /// <summary>
        /// Common Method for dynamically Convert Data Table to any type list object as per column name match and use for API response send.
        /// Created By : RK
        /// Created On : 2026-04-20
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        // Convert DataTable → List<Dictionary<string, object>>
        public static List<Dictionary<string, object>> ConvertDataTableToDynamic(DataTable dt)
        {
            var list = new List<Dictionary<string, object>>();

            try
            {
                if (dt == null || dt.Rows.Count == 0)
                    return list;

                foreach (DataRow row in dt.Rows)
                {
                    var dict = new Dictionary<string, object>();

                    foreach (DataColumn col in dt.Columns)
                    {
                        dict[col.ColumnName] = row[col] == DBNull.Value ? null : row[col];
                    }

                    list.Add(dict);
                }
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(
                    new ErrorLogEntity()
                    {
                        PageName = "csCommon",
                        FunctionName = nameof(ConvertDataTableToDynamic)
                    }, ex);

                return new List<Dictionary<string, object>>(); // ✅ SAFE RETURN
            }

            return list;
        }

        /// <summary>
        /// Common Method for dynamically Convert Data SET (Multiple Tables) to any type list object as per Given Table name and column name match and use for API response send.
        /// Created By : RK
        /// Created On : 2026-04-28 
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        public static Dictionary<string, List<Dictionary<string, object>>> ConvertDataSetToDynamic(DataSet ds, List<string> tableNames)
        {
            var result = new Dictionary<string, List<Dictionary<string, object>>>();

            try
            {
                if (ds == null || ds.Tables.Count == 0)
                    return result;

                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    string tableName = (tableNames != null && tableNames.Count > i && !string.IsNullOrEmpty(tableNames[i]))
                                        ? tableNames[i]
                                        : "Table" + i;

                    var table = ds.Tables[i];
                    var tableData = new List<Dictionary<string, object>>();

                    foreach (DataRow row in table.Rows)
                    {
                        var dict = new Dictionary<string, object>();

                        foreach (DataColumn col in table.Columns)
                        {
                            dict[col.ColumnName] = row[col] == DBNull.Value ? null : row[col];
                        }

                        tableData.Add(dict);
                    }

                    result[tableName] = tableData;
                }
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(
                    new ErrorLogEntity()
                    {
                        PageName = "csCommon",
                        FunctionName = nameof(ConvertDataSetToDynamic)
                    }, ex);

                return new Dictionary<string, List<Dictionary<string, object>>>(); // ✅ SAFE RETURN
            }

            return result;
        }

        /// <summary>
        /// Common Method for dynamically get also Table Names From Data Set and use for API response send as Array or List view object name. 
        /// Dynamic Approach developed.
        /// Created By : RK
        /// Created On : 2026-04-30 
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static List<string> GetTableNamesFromDataSet(DataSet ds)
        {
            var tableNames = new List<string>();

            if (ds != null && ds.Tables.Count > 0)
            {
                var lastTable = ds.Tables[ds.Tables.Count - 1];

                if (lastTable.Columns.Contains("TableName"))
                {
                    foreach (DataRow row in lastTable.Rows)
                    {
                        if (row["TableName"] != DBNull.Value)
                        {
                            tableNames.Add(row["TableName"].ToString());
                        }
                    }
                }
            }

            return tableNames;
        }
        #endregion --END---- Dyanamically_Convertion_Response_SP_Object_Convert_Into_API_Json_List_View
    }
}