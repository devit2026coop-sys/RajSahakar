using Cooperative.Controllers;
using Cooperative.DBConnector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace Cooperative.Models
{

    public class HTMLHelper
    {
        private static String PreparePOSTForm(string url, NameValueCollection data)
        {
            //Set a name for the form
            string formID = "PostForm";
            //Build the form using the specified data to be posted.
            StringBuilder strForm = new StringBuilder();
            strForm.Append("<form id=\"" + formID + "\" name=\"" +
                           formID + "\" action=\"" + url +
                           "\" method=\"POST\">");

            foreach (string key in data)
            {
                strForm.Append("<input type=\"hidden\" name=\"" + key +
                               "\" value=\"" + data[key] + "\">");
            }

            strForm.Append("</form>");
            //Build the JavaScript which will do the Posting operation.
            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language='javascript'>");
            strScript.Append("var v" + formID + " = document." +
                             formID + ";");
            strScript.Append("v" + formID + ".submit();");
            strScript.Append("</script>");
            //Return the form and the script concatenated.
            //(The order is important, Form then JavaScript)
            return strForm.ToString() + strScript.ToString();




        }

        public static string posttopage(bool IsMobileApp, string destinationUrl, string MERCHANTCODE, string ENCDATA, string SERVICEID)
        {
            string formID = "PostForm";

            StringBuilder sb = new StringBuilder();

            if (IsMobileApp == true)
            {
                sb.Append(destinationUrl + "?MERCHANTCODE=" + MERCHANTCODE + "&SERVICEID=" + SERVICEID + "&ENCDATA=" + ENCDATA);

            }
            else
            {

                //sb.Append("<html>");
                //sb.Append("<body>");
                sb.Append("<form id=\"" + formID + "\" name=\"" + formID + "\" action=\"" + destinationUrl + "\" method=\"POST\">");
                sb.AppendFormat("<input type='hidden' name='MERCHANTCODE' value='" + MERCHANTCODE + "'>");
                sb.AppendFormat("<input type='hidden' name='SERVICEID' value='" + SERVICEID + "'>");
                sb.AppendFormat("<input type='hidden' name='ENCDATA' value='" + ENCDATA + "'>");
                sb.Append("</form>");
                //sb.AppendFormat("<script language='javascript'>var " + formID + " = document.PostForm; " + formID + ".submit();</script>");
                //sb.Append("</body>");
                //sb.Append("</html>");
            }

            //HttpContext.Current.Response.Clear();
            //HttpContext.Current.Response.Write(sb.ToString());
            //HttpContext.Current.Response.End();


            return sb.ToString();
        }


        public static string RedirectAndPOST(bool IsMobileApp, string destinationUrl, string MERCHANTCODE, string ENCDATA, string SERVICEID)
        {
            //Prepare the Posting form

            string strForm = posttopage(IsMobileApp, destinationUrl, MERCHANTCODE, ENCDATA, SERVICEID);
            //Add a literal control the specified page holding 
            //the Post Form, this is to submit the Posting form with the request.
            // page.Controls.Add(new LiteralControl(strForm));
            return strForm;
        }
    }


    public class EmitraModelFactory
    {

        public string RegistrationNo { get; set; }


        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;
        public EmitraModelFactory(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }
        public static string Decrypt(string textToDecrypt, string EncryptionPassword)
        {
            //textToDecrypt = textToDecrypt.te;//.Replace(" ", "+");
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;
            rijndaelCipher.KeySize = 256;
            rijndaelCipher.BlockSize = 128;
            int mod4 = textToDecrypt.Length % 4;
            if (mod4 > 0)
            {
                textToDecrypt += new string('=', 4 - mod4);
            }

            byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
            byte[] pwdBytes = Encoding.UTF8.GetBytes(EncryptionPassword);
            pwdBytes = SHA256.Create().ComputeHash(pwdBytes);
            byte[] keyBytes = new byte[16];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }
            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;
            byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            return Encoding.UTF8.GetString(plainText);
        }
        public static string Encrypt(string textToEncrypt, string EncryptionPassword)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;
            rijndaelCipher.KeySize = 256;
            rijndaelCipher.BlockSize = 128;
            byte[] pwdBytes = Encoding.UTF8.GetBytes(EncryptionPassword);
            pwdBytes = SHA256.Create().ComputeHash(pwdBytes);
            byte[] keyBytes = new byte[16];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }
            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;
            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
            byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);
            return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
        }
        public string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    //sb.Append(hashBytes[i].ToString("X2"));
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
        public DataTable GetApplicationKey(string ServiceId, bool IsKiosk)
        
        {
            DataTable dt = new DataTable();
            try
            {
                string whereClause = " and  1=1";
                if (ServiceId.Length > 0)
                {
                    whereClause += "  and  SERICEID=" + ServiceId;
                    whereClause += "  and  IsKiosk=" + (IsKiosk ? "1" : "0");
                }
                objDBAccess.OpenConnection();
                objDBAccess.CreateCommand("EmitraServicesList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("WhereClause", DBFactoryParameterType.NVarchar, whereClause);
                objDBAccess.OpenConnection();
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModelFactory", FunctionName = "GetApplicationKey(string ServiceId, bool IsKiosk)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }
        public DataTable GetApplicationKeyByName(string ServiceName, bool IsKiosk)
        {
            DataTable dt = new DataTable();
            try
            {
                string whereClause = " where 1=1";
                if (ServiceName.Length > 0)
                {
                    whereClause += "  and  \"SERVICENAME\"='" + ServiceName + "'  ";
                    whereClause += "  and  \"IsKiosk\"='" + IsKiosk + "'  ";
                }

                objDBAccess.CreateCommand("selectapplicationkeyvalbyapplicationkey", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("WhereClause", DBFactoryParameterType.NVarchar, whereClause);
                objDBAccess.OpenConnection();
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModelFactory", FunctionName = "GetApplicationKeyByName(string ServiceName, bool IsKiosk)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }
        public String PreparePOSTForm(string url, NameValueCollection data)
        {
            //Set a name for the form
            string formID = "PostForm";
            //Build the form using the specified data to be posted.
            StringBuilder strForm = new StringBuilder();
            strForm.Append("<form id=\"" + formID + "\" name=\"" +
                           formID + "\" action=\"" + url +
                           "\" method=\"POST\">");

            foreach (string key in data)
            {
                strForm.Append("<input type=\"hidden\" name=\"" + key +
                               "\" value=\"" + data[key] + "\"/>");
            }

            strForm.Append("</form>");
            //Build the JavaScript which will do the Posting operation.
            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language='javascript'>");
            strScript.Append("var v" + formID + " = document." +
                             formID + ";");
            strScript.Append("v" + formID + ".submit();");
            strScript.Append("</script>");
            //Return the form and the script concatenated.
            //(The order is important, Form then JavaScript)
            return strForm.ToString() + strScript.ToString();
        }
        public int MakePayment(string UniquerequestId, string ConsumerName, string damnt, string RegistrationNo, string SocietyIdEnc, int IsPermanentSave)
        {

            DataTable Dt = null;
            int returnvalue = -1;
            string ServiceName = "";
            if (HttpContext.Current.Session["ARequest"] != null)
            {
                ARequest emitra = (ARequest)HttpContext.Current.Session["ARequest"];
                //ARequest emitra = new ARequest();
                //emitra.SSOID = "VINAYYDAVA329";
                //emitra.SERVICEID = "4209";
                //emitra.SSOTOKEN = "";
                if (emitra != null)
                {
                    EmitraModelFactory objEmitraModel = new EmitraModelFactory(new FileLogger());
                    Dt = objEmitraModel.GetApplicationKey(emitra.SERVICEID.ToString(), true);//emitra.SERVICEID.ToString()              
                    if (Dt.Rows.Count > 0)
                    {
                        double dAmt = Convert.ToDouble(damnt);
                        double cAmt = 0;
                        ServiceName = GerServiceName(emitra.SERVICEID.ToString());//// emitra.SERVICEID.ToString() == "3343" ? "RGKSSYAF" : "LAFP";

                        DRequest req = new DRequest();
                        req.MERCHANTCODE = Dt.Rows[0]["MERCHANTCODE"].ToString();
                        req.REQUESTID = UniquerequestId;
                        req.REQTIMESTAMP = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        req.SERVICEID = Dt.Rows[0]["SERICEID"].ToString();
                        cAmt = Convert.ToDouble(Dt.Rows[0]["COMTYPE"].ToString());
                        req.SUBSERVICEID = "";
                        req.REVENUEHEAD = Dt.Rows[0]["REVENUHEADKIOSKINTEGRATION"].ToString();
                        req.CONSUMERKEY = UniquerequestId;
                        req.CONSUMERNAME = ConsumerName.Replace("-","");
                        req.COMMTYPE = "3";
                        req.SSOID = emitra.SSOID;
                        req.OFFICECODE = Dt.Rows[0]["OFFICECODE"].ToString();
                        req.SSOTOKEN = emitra.SSOTOKEN;

                        DRequestChecksum reqChecksum = new DRequestChecksum(); //New line
                        req.CHECKSUM = reqChecksum.GetCheckSum(req); //New field

                        int i = -1;
                        //DBFactoryPGSQL objDBAccess = new DBFactoryPGSQL("connectionString");
                        objDBAccess.OpenConnection();
                        try
                        {
                            if (req != null)
                            {
                                //UPDATE REQUEST
                                objDBAccess.CreateCommand("UpdateEmitraKioskTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                                objDBAccess.AddParameterToCommand("Emitraapprequestid", DBFactoryParameterType.BigInt, int.Parse(UniquerequestId));
                                objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, JsonConvert.SerializeObject(req));
                                objDBAccess.AddParameterToCommand("updateflag", DBFactoryParameterType.BigInt, 1);
                                objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, "");
                                objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, "");
                                objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, "");
                                objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, "");
                                objDBAccess.AddParameterToCommand("RegistrationNo", DBFactoryParameterType.VARCHAR, RegistrationNo);
                                objDBAccess.AddParameterToCommand("ServiceId", DBFactoryParameterType.Int, emitra.SERVICEID);
                                objDBAccess.AddParameterToCommand("SocietyIdEnc", DBFactoryParameterType.VARCHAR, SocietyIdEnc);
                                objDBAccess.OpenConnection();
                                object o = objDBAccess.ExecuteScalarCommand();
                                i = Convert.ToInt32(o);
                            }
                        }
                        catch (Exception ex)
                        {
                            objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModelFactory", FunctionName = "MakePayment(string UniquerequestId, string ConsumerName, string damnt, string RegistrationNo, string SocietyIdEnc,int IsPermanentSave)" }, ex);
                        }
                        finally
                        {
                            objDBAccess.CloseConnection();
                            dt.Dispose();
                        }

                        if (i > 0)
                        {
                            try
                            {
                                string retVal = MakeTransactionsEncrypted(Dt.Rows[0]["T_Url"].ToString(), JsonConvert.SerializeObject(req), Dt.Rows[0]["ENCRYPTIONKEY"].ToString());
                                string decVal = Decrypt(retVal, Dt.Rows[0]["ENCRYPTIONKEY"].ToString());
                                DResponse resp = JsonConvert.DeserializeObject<DResponse>(decVal);
                                if (resp != null)
                                {
                                    //UPDATE RESPONCE
                                    objDBAccess.CreateCommand("UpdateEmitraKioskTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                                    objDBAccess.AddParameterToCommand("Emitraapprequestid", DBFactoryParameterType.BigInt, int.Parse(UniquerequestId));
                                    objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, "");
                                    objDBAccess.AddParameterToCommand("updateflag", DBFactoryParameterType.BigInt, 2);
                                    objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, resp.RECEIPTNO);
                                    objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, resp.TRANSACTIONID);
                                    objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, resp.MSG);
                                    objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, JsonConvert.SerializeObject(resp));
                                    objDBAccess.AddParameterToCommand("RegistrationNo", DBFactoryParameterType.VARCHAR, RegistrationNo);
                                    objDBAccess.AddParameterToCommand("ServiceId", DBFactoryParameterType.Int, emitra.SERVICEID);
                                    objDBAccess.AddParameterToCommand("SocietyIdEnc", DBFactoryParameterType.VARCHAR, SocietyIdEnc);
                                    objDBAccess.AddParameterToCommand("IsPermanentSave", DBFactoryParameterType.Int, IsPermanentSave);
                                    objDBAccess.OpenConnection();
                                    object o = objDBAccess.ExecuteScalarCommand();
                                    i = Convert.ToInt32(o);
                                    //returnvalue = resp.MSG;
                                    returnvalue = i;
                                }
                                else
                                {

                                }
                            }
                            catch (Exception ex)
                            {
                                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModelFactory", FunctionName = "MakePayment(string UniquerequestId, string ConsumerName, string damnt, string RegistrationNo, string SocietyIdEnc,int IsPermanentSave)" }, ex);
                            }
                            finally
                            {
                                objDBAccess.CloseConnection();
                                dt.Dispose();
                            }
                        }
                        if (i > 0 && (emitra.SERVICEID!= "9828" && emitra.SERVICEID != "13851"))
                        {
                            try
                            {

                                //UPDATE RESPONCE
                                objDBAccess.CreateCommand("SaveSocietyDetailsBySocietyIdFormTempTbl", DBFactoryCommandType.PROCEDURE_COMMAND);
                                objDBAccess.AddParameterToCommand("Emitraapprequestid", DBFactoryParameterType.BigInt, int.Parse(UniquerequestId));
                                objDBAccess.AddParameterToCommand("SocietyIdEnc", DBFactoryParameterType.VARCHAR, SocietyIdEnc);
                                objDBAccess.OpenConnection();
                                object o = objDBAccess.ExecuteScalarCommand();
                                i = Convert.ToInt32(o);
                                returnvalue = i;
                                if (i == 1)
                                {
                                    ApplicationFlowModel AppFlowModal = new ApplicationFlowModel();
                                    AppFlowModal.SendMSG(SocietyIdEnc, -2);
                                }

                            }
                            catch (Exception ex)
                            {
                                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModelFactory", FunctionName = "MakePayment(string UniquerequestId, string ConsumerName, string damnt, string RegistrationNo, string SocietyIdEnc,int IsPermanentSave)" }, ex);
                            }
                            finally
                            {
                                objDBAccess.CloseConnection();
                                dt.Dispose();
                            }
                        }

                    }
                    else
                    {
                        returnvalue = -1;
                    }
                }
            }


            //return returnvalue + "@" + ServiceName;
            return returnvalue;
        }
        public static string GerServiceName(string ServiceId)
        {
            switch (ServiceId)
            {
                case "3343":
                    return "RGKSSYAF";
                case "3363":
                    return "LAFP";
                case "3443":
                    return "FCRF";
                case "3463":
                    return "LFEE";
                default:
                    return "INVALID";
            }
        }
        public static string MakeTransactionsEncrypted(string URL, string data, string encryptionKey)
        {
            try
            {
                //LogErrorToLogFile error = new LogErrorToLogFile();

                //error.LogEmitra("Kiosk Verify " + data, "TestApp1");

                string encData = Encrypt(data, encryptionKey);
                //error.LogEmitra("Encrypted Request Data: Encrypt Data: " + encData, "TestApp2");
                //Base String
                string baseAddress = URL;
                //Post Parameters
                StringBuilder postData = new StringBuilder();
                postData.Append("encData=" + HttpUtility.UrlEncode(encData));

                //Create Web Request
                var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
                http.Method = "POST";
                http.Accept = "application/json";
                http.ContentType = "application/x-www-form-urlencoded";

                //Start Writing Post parameters to request object
                string parsedContent = postData.ToString();
                ASCIIEncoding encoding = new ASCIIEncoding();
                Byte[] bytes = encoding.GetBytes(parsedContent);
                Stream newStream = http.GetRequestStream();
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();

                //Read Response for posting done
                var response = http.GetResponse();
                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream);
                var content = sr.ReadToEnd();
                //return contents
                return content;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public string UpdateEmitrTransation(string UniquerequestId, string ConsumerName, string damnt, string RegistrationNo,string SocietyId, string SocietyIdEnc, string ServiceID, bool IsKiosk = true, string IsPermanentSave = "0", string Amount = "0")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            if (UniquerequestId.Length == 0)
            {
                #region "insert into Emitrakiosktransaction"
                objDBAccess.CreateCommand("InsertEmitraKioskTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("SocietyIdEnc", DBFactoryParameterType.VARCHAR, SocietyIdEnc);//
                objDBAccess.AddParameterToCommand("ApplicationNo", DBFactoryParameterType.VARCHAR, RegistrationNo);//
                objDBAccess.AddParameterToCommand("KioskID", DBFactoryParameterType.VARCHAR, _LoggedInUser.KIOSKCODE.Length > 0 ? _LoggedInUser.KIOSKCODE : "SSO");//
                objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, "");//
                objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("RequestStatus", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("ActId", DBFactoryParameterType.Int, 0);//                    
                objDBAccess.AddParameterToCommand("SSOID", DBFactoryParameterType.VARCHAR, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("CreatedIP", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("SERICEID", DBFactoryParameterType.BigInt, int.Parse(ServiceID));
                objDBAccess.OpenConnection();
                object oi = objDBAccess.ExecuteScalarCommand();
                UniquerequestId = Convert.ToString(oi);
                #endregion
            }
            DataTable Dt = null;
            string Result = "";

            if (_LoggedInUser != null)
            {
                EmitraModelFactory objEmitraModel = new EmitraModelFactory(new FileLogger());
                Dt = objEmitraModel.GetApplicationKey(ServiceID, IsKiosk);
                var tempRegistrationNo = RegistrationNo.Split('/');
                RegistrationNo = "";
                for (var k = 0; k < tempRegistrationNo.Length; k++)
                {
                    RegistrationNo += tempRegistrationNo[k];
                }
                if (Dt.Rows.Count > 0)
                {
                    string remoteUrl = Dt.Rows[0]["T_Url"].ToString();
                    string Checksumkey = Dt.Rows[0]["CHECKSUMKEY"].ToString();
                    string EncryptionKey = Dt.Rows[0]["ENCRYPTIONKEY"].ToString();

                    PGRequest data = new PGRequest();
                    // data.AMOUNT = Dt.Rows[0]["REVENUEHEADFORPG"].ToString().Split('-')[1].ToString();
                    data.AMOUNT = Amount;
                    data.COMMTYPE = Dt.Rows[0]["COMTYPE"].ToString();
                    data.MERCHANTCODE = Dt.Rows[0]["MERCHANTCODE"].ToString();
                    data.OFFICECODE = Dt.Rows[0]["OFFICECODE"].ToString();
                    data.PRN = UniquerequestId;
                    data.REQTIMESTAMP = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    data.REVENUEHEAD = Dt.Rows[0]["REVENUEHEADFORPG"].ToString().Split('-')[0].ToString() + "-" + Amount;
                    data.SERVICEID = Dt.Rows[0]["SERICEID"].ToString();
                    data.SUCCESSURL = Dt.Rows[0]["REDIRECTURL"].ToString() + "/?UniquerequestId=" + COMMON_FUNCTION.Encrypt(UniquerequestId) + "&SocietyIdEnc=" + SocietyIdEnc + "&ServiceID=" + ServiceID + "&IsFailed=" + COMMON_FUNCTION.Encrypt("NO") + "&IsPermanentSave=" + COMMON_FUNCTION.Encrypt(IsPermanentSave);
                    data.FAILUREURL = Dt.Rows[0]["REDIRECTURL"].ToString() + "/?UniquerequestId=" + COMMON_FUNCTION.Encrypt(UniquerequestId) + "&SocietyIdEnc=" + SocietyIdEnc + "&ServiceID=" + ServiceID + "&IsFailed=" + COMMON_FUNCTION.Encrypt("YES") + "&IsPermanentSave=" + COMMON_FUNCTION.Encrypt(IsPermanentSave);
                    //data.SUCCESSURL = "http://localhost:27466/Citizen/UpdateOnlinePaymentStatu_Post_ssuc";
                    //data.FAILUREURL = "http://localhost:27466/Citizen/UpdateOnlinePaymentStatu_Post_ssuc";
                    data.UDF1 = RegistrationNo.Replace("-", "");
                    data.UDF2 = _LoggedInUser.SSOID.ToUpper();
                    data.CONSUMERKEY = SocietyId + (!string.IsNullOrWhiteSpace(RegistrationNo.Replace("-", "")) ? "-" + RegistrationNo.Replace("-", "") : "");
                    data.CONSUMERNAME = ConsumerName.Replace("-","");
                    data.USEREMAIL = "";
                    data.USERMOBILE = "";

                    data.USERNAME = ConsumerName.Replace("-", "");


                    data.CHECKSUM = CreateMD5(data.PRN + "|" + data.AMOUNT + "|" + Checksumkey);
                    int M = -1;
                    try
                    {
                        if (data != null)
                        {
                            objDBAccess.CreateCommand("UpdateEmitraKioskTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                            objDBAccess.AddParameterToCommand("Emitraapprequestid", DBFactoryParameterType.BigInt, int.Parse(UniquerequestId));
                            objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, JsonConvert.SerializeObject(data));
                            objDBAccess.AddParameterToCommand("updateflag", DBFactoryParameterType.BigInt, 1);
                            objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, "");
                            objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, "");
                            objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, "");
                            objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, "");
                            objDBAccess.AddParameterToCommand("RegistrationNo", DBFactoryParameterType.VARCHAR, RegistrationNo);
                            objDBAccess.AddParameterToCommand("ServiceId", DBFactoryParameterType.Int, ServiceID);
                            objDBAccess.AddParameterToCommand("SocietyIdEnc", DBFactoryParameterType.VARCHAR, SocietyIdEnc);
                            //objDBAccess.AddParameterToCommand("PRN", DBFactoryParameterType.VARCHAR, data.PRN);

                            objDBAccess.OpenConnection();
                            object o = objDBAccess.ExecuteScalarCommand();
                            M = Convert.ToInt32(o);
                        }
                    }
                    catch (Exception ex)
                    {
                        //ILogger objILogger = new DBLogger();
                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModelFactory", FunctionName = "UpdateEmitrTransation(string UniquerequestId, string ConsumerName, string damnt, string RegistrationNo, string SocietyIdEnc, string ServiceID, bool IsKiosk = true)" }, ex);
                    }
                    finally
                    {
                        objDBAccess.CloseConnection();
                    }
                    StringBuilder sb = new StringBuilder();
                    EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES(EncryptionKey);
                    string encData = objEncryptDecrypt.encrypt(JsonConvert.SerializeObject(data));
                    string encDataSimple = JsonConvert.SerializeObject(data);
                    sb.Append("<form id=\"PostForm\" name=\"PostForm\" action=\"" + remoteUrl + "\" method=\"POST\">");
                    sb.AppendFormat("<input type='hidden' name='MERCHANTCODE' value='" + data.MERCHANTCODE + "'>");
                    sb.AppendFormat("<input type='hidden' name='SERVICEID' value='" + ServiceID + "'>");
                    sb.AppendFormat("<input type='hidden' name='ENCDATA' value='" + encData + "'>");
                    sb.Append("</form>");
                    // ILogger objILogger = new DBLogger();
                    //objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "UpdateEmitrTransation(string UniquerequestId, string ConsumerName, string damnt, string RegistrationNo, string SocietyIdEnc, string ServiceID, bool IsKiosk = true)" }, new Exception("IN form"));
                    return sb.ToString();

                }
            }
            return Result;
        }

        public string UpdateEmitrTransation_New(string UniquerequestId, string ConsumerName, string damnt, string RegistrationNo, string SocietyIdEnc, string ServiceID, bool IsKiosk = true, string IsPermanentSave = "0")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int i = -1;
            int Result = -1;
            //UPDATE RESPONCE
            objDBAccess.CreateCommand("UpdateEmitraKioskTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
            objDBAccess.AddParameterToCommand("Emitraapprequestid", DBFactoryParameterType.BigInt, int.Parse(UniquerequestId));
            objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, "BY CITYZEN oR DEPart");
            objDBAccess.AddParameterToCommand("updateflag", DBFactoryParameterType.BigInt, 2);
            objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, "");
            objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, "");
            objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, "Transaction Successfully Done");
            objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, "");
            objDBAccess.AddParameterToCommand("RegistrationNo", DBFactoryParameterType.VARCHAR, RegistrationNo);
            objDBAccess.AddParameterToCommand("ServiceId", DBFactoryParameterType.Int, ServiceID);
            objDBAccess.AddParameterToCommand("SocietyIdEnc", DBFactoryParameterType.VARCHAR, SocietyIdEnc);
            objDBAccess.AddParameterToCommand("IsPermanentSave", DBFactoryParameterType.Int, IsPermanentSave);
            objDBAccess.OpenConnection();
            object o = objDBAccess.ExecuteScalarCommand();
            string S = o.ToString();
            if (S.Contains("#"))
            {
                o = S.Split('#')[0];
                RegistrationNo = S.Split('#')[1];
            }
            i = Convert.ToInt32(o);
            //returnvalue = resp.MSG;
            if (i > 0)
            {
                try
                {
                    //UPDATE RESPONCE
                    objDBAccess.CreateCommand("SaveSocietyDetailsBySocietyIdFormTempTbl", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("Emitraapprequestid", DBFactoryParameterType.BigInt, int.Parse(UniquerequestId));
                    objDBAccess.AddParameterToCommand("SocietyIdEnc", DBFactoryParameterType.VARCHAR, SocietyIdEnc);
                    objDBAccess.OpenConnection();

                    object oo = objDBAccess.ExecuteScalarCommand();
                    Result = Convert.ToInt32(oo);
                    if (Result == 1 && RegistrationNo.Length > 0)
                    {
                        ApplicationFlowModel AppFlowModal = new ApplicationFlowModel();
                        AppFlowModal.SendMSG(SocietyIdEnc, -2);
                    }
                }
                catch (Exception ex)
                {
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModelFactory", FunctionName = "SaveSocietyDetailsBySocietyIdFormTempTbl()" }, ex);
                }
                finally
                {
                    objDBAccess.CloseConnection();
                    dt.Dispose();
                }
            }

            return Result.ToString();
        }

        public string UpdateEmitrTransation_SingleForm(string UniquerequestId, string ConsumerName, string damnt, string RegistrationNo, string SocietyIdEnc, string ServiceID, bool IsKiosk = true, string IsPermanentSave = "0")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int i = -1;
            int Result = -1;
            //UPDATE RESPONCE
            objDBAccess.CreateCommand("UpdateEmitraKioskTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
            objDBAccess.AddParameterToCommand("@Emitraapprequestid", DBFactoryParameterType.BigInt, int.Parse(UniquerequestId));
            objDBAccess.AddParameterToCommand("@RequestString", DBFactoryParameterType.VARCHAR, "BY CITYZEN oR DEPart");
            objDBAccess.AddParameterToCommand("@updateflag", DBFactoryParameterType.BigInt, 2);
            objDBAccess.AddParameterToCommand("@ReceiptNo", DBFactoryParameterType.VARCHAR, "");
            objDBAccess.AddParameterToCommand("@TokenNo", DBFactoryParameterType.VARCHAR, "");
            objDBAccess.AddParameterToCommand("@StatusMsg", DBFactoryParameterType.VARCHAR, "Transaction Successfully Done");
            objDBAccess.AddParameterToCommand("@ResponseString", DBFactoryParameterType.VARCHAR, "");
            objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.VARCHAR, RegistrationNo);
            objDBAccess.AddParameterToCommand("@ServiceId", DBFactoryParameterType.Int, ServiceID);
            objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.VARCHAR, SocietyIdEnc);
            objDBAccess.AddParameterToCommand("@IsPermanentSave", DBFactoryParameterType.Int, IsPermanentSave);
            objDBAccess.OpenConnection();
            object o = objDBAccess.ExecuteScalarCommand();
            string S = o.ToString();
            if (S.Contains("#"))
            {
                o = S.Split('#')[0];
                RegistrationNo = S.Split('#')[1];
            }
            i = Convert.ToInt32(o);
            //returnvalue = resp.MSG;
            if (i > 0)
            {
                try
                {
                    if (RegistrationNo.Length > 0)
                    {
                        ApplicationFlowModel AppFlowModal = new ApplicationFlowModel();
                        AppFlowModal.SendMSG(SocietyIdEnc, -2);
                    }
                }
                catch (Exception ex)
                {
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModelFactory", FunctionName = "SaveSocietyDetailsBySocietyIdFormTempTbl()" }, ex);
                }
                finally
                {
                    objDBAccess.CloseConnection();
                    dt.Dispose();
                }
            }

            return i.ToString();
        }

        public string UpdateEmitrTransationAll(string UniquerequestId, string ConsumerName, string damnt, string RegistrationNo, string SocietyId, string ServiceID, string KIOSKCODE, string SSOID, bool IsKiosk = true)
        {
            //ServiceID = "5531";
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ARequest emitra = (ARequest)HttpContext.Current.Session["ARequest"];
            //if (emitra != null)
            //{pppppppp
            if (UniquerequestId == "0")
            {
                #region "insert into Emitrakiosktransaction"
                objDBAccess.CreateCommand("insertemitrakiosktransactionAll", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("SocietyId", DBFactoryParameterType.Int, int.Parse(SocietyId));//
                objDBAccess.AddParameterToCommand("ApplicationNo", DBFactoryParameterType.VARCHAR, RegistrationNo);//
                objDBAccess.AddParameterToCommand("KioskID", DBFactoryParameterType.VARCHAR, KIOSKCODE.Length > 0 ? KIOSKCODE : "SSO");//
                objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, "");//
                objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("RequestStatus", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("ActId", DBFactoryParameterType.Int, 0);//                    
                objDBAccess.AddParameterToCommand("SSOID", DBFactoryParameterType.VARCHAR, SSOID);
                objDBAccess.AddParameterToCommand("CreatedIP", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("SERICEID", DBFactoryParameterType.BigInt, ServiceID); //int.Parse(emitra.SERVICEID.ToString())
                objDBAccess.OpenConnection();
                object oi = objDBAccess.ExecuteScalarCommand();
                UniquerequestId = Convert.ToString(oi);
                #endregion
            }
            //}
            DataTable Dt = null;
            string Result = "";

            if (_LoggedInUser != null)
            {
                EmitraModelFactory objEmitraModel = new EmitraModelFactory(new FileLogger());
                Dt = objEmitraModel.GetApplicationKey(ServiceID, IsKiosk);
                var tempRegistrationNo = RegistrationNo.Split('/');
                RegistrationNo = "";
                for (var k = 0; k < tempRegistrationNo.Length; k++)
                {
                    RegistrationNo += tempRegistrationNo[k];
                }
                if (Dt.Rows.Count > 0)
                {
                    string remoteUrl = Dt.Rows[0]["T_Url"].ToString();
                    string Checksumkey = Dt.Rows[0]["CHECKSUMKEY"].ToString();
                    string EncryptionKey = Dt.Rows[0]["ENCRYPTIONKEY"].ToString();

                    PGRequest data = new PGRequest();
                    data.AMOUNT = Dt.Rows[0]["REVENUEHEADFORPG"].ToString().Split('-')[1].ToString();
                    data.COMMTYPE = Dt.Rows[0]["COMTYPE"].ToString();
                    data.MERCHANTCODE = Dt.Rows[0]["MERCHANTCODE"].ToString();
                    data.OFFICECODE = Dt.Rows[0]["OFFICECODE"].ToString();
                    data.PRN = UniquerequestId;
                    data.REQTIMESTAMP = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    data.REVENUEHEAD = Dt.Rows[0]["REVENUEHEADFORPG"].ToString();
                    data.SERVICEID = Dt.Rows[0]["SERICEID"].ToString();
                    data.SUCCESSURL = Dt.Rows[0]["REDIRECTURL"].ToString() + "?UniquerequestId=" + COMMON_FUNCTION.Encrypt(UniquerequestId) + "&SocietyIdEnc=" + COMMON_FUNCTION.Encrypt(SocietyId) + "&ServiceID=" + ServiceID + "&IsFailed=" + COMMON_FUNCTION.Encrypt("NO"); ;
                    data.FAILUREURL = Dt.Rows[0]["REDIRECTURL"].ToString() + "?UniquerequestId=" + COMMON_FUNCTION.Encrypt(UniquerequestId) + "&SocietyIdEnc=" + COMMON_FUNCTION.Encrypt(SocietyId) + "&ServiceID=" + ServiceID + "&IsFailed=" + COMMON_FUNCTION.Encrypt("YES");
                    //data.SUCCESSURL = "http://localhost:27466/Citizen/UpdateOnlinePaymentStatu_Post_ssuc";
                    //data.FAILUREURL = "http://localhost:27466/Citizen/UpdateOnlinePaymentStatu_Post_ssuc";
                    data.UDF1 = RegistrationNo.Replace("-", "");
                    data.UDF2 = _LoggedInUser.SSOID.ToUpper();

                    data.USEREMAIL = "";
                    data.USERMOBILE = "";

                    data.USERNAME = ConsumerName.Replace("-", "");


                    data.CHECKSUM = CreateMD5(data.PRN + "|" + data.AMOUNT + "|" + Checksumkey);
                    int M = -1;
                    try
                    {
                        if (data != null)
                        {
                            objDBAccess.CreateCommand("UpdateEmitraKioskTransactionForAll", DBFactoryCommandType.PROCEDURE_COMMAND);
                            objDBAccess.AddParameterToCommand("Emitraapprequestid", DBFactoryParameterType.BigInt, int.Parse(UniquerequestId));
                            objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, JsonConvert.SerializeObject(data));
                            objDBAccess.AddParameterToCommand("updateflag", DBFactoryParameterType.BigInt, 1);
                            objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, "");
                            objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, "");
                            objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, "");
                            objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, "");
                            objDBAccess.AddParameterToCommand("RegistrationNo", DBFactoryParameterType.VARCHAR, RegistrationNo);
                            objDBAccess.AddParameterToCommand("ServiceId", DBFactoryParameterType.Int, ServiceID);
                            objDBAccess.AddParameterToCommand("SocietyId", DBFactoryParameterType.Int, SocietyId);

                            objDBAccess.OpenConnection();
                            object o = objDBAccess.ExecuteScalarCommand();
                            M = Convert.ToInt32(o);
                        }
                    }
                    catch (Exception ex)
                    {
                        //ILogger objILogger = new DBLogger();
                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModelFactory", FunctionName = "UpdateEmitrTransation(string UniquerequestId, string ConsumerName, string damnt, string RegistrationNo, string SocietyIdEnc, string ServiceID, bool IsKiosk = true)" }, ex);
                    }
                    finally
                    {
                        objDBAccess.CloseConnection();
                    }
                    StringBuilder sb = new StringBuilder();
                    EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES(EncryptionKey);
                    string encData = objEncryptDecrypt.encrypt(JsonConvert.SerializeObject(data));
                    string encDataSimple = JsonConvert.SerializeObject(data);
                    sb.Append("<form id=\"PostForm\" name=\"PostForm\" action=\"" + remoteUrl + "\" method=\"POST\">");
                    sb.AppendFormat("<input type='hidden' name='MERCHANTCODE' value='" + data.MERCHANTCODE + "'>");
                    sb.AppendFormat("<input type='hidden' name='SERVICEID' value='" + ServiceID + "'>");
                    sb.AppendFormat("<input type='hidden' name='ENCDATA' value='" + encData + "'>");
                    sb.Append("</form>");


                    // ILogger objILogger = new DBLogger();
                    //objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "UpdateEmitrTransation(string UniquerequestId, string ConsumerName, string damnt, string RegistrationNo, string SocietyIdEnc, string ServiceID, bool IsKiosk = true)" }, new Exception("IN form"));
                    return sb.ToString();

                }
            }
            return Result;
        }


        private string BuildCcAvenueRequestParameters(string MERCHANTCODE, string SERVICEID, string encData)
        {

            var queryParameters = new Dictionary<string, string>
             {
             {"MERCHANTCODE", MERCHANTCODE},
             {"SERVICEID", SERVICEID},
             {"ENCDATA", encData}
        }.Select(item => string.Format("{0}={1}", item.Key, item.Value));
            return string.Join("&", queryParameters);
        }


        public void RedirectAndPOST(Page page, string destinationUrl, NameValueCollection data)
        {
            string strForm = PreparePOSTForm(destinationUrl, data);
            page.Controls.Add(new LiteralControl(strForm));
        }


        public DataTable SelectDetailForEmitraPayment(string AppRequestID, string ServiceID = "")
        {
            DataTable dt = new DataTable();
            try
            {
                string whereClause = " ";
                if (AppRequestID.Length > 0)
                {
                    whereClause += " and EMitraAppRequestID=" + AppRequestID + "  ";
                }
                if (ServiceID == "4690")
                {
                    objDBAccess.CreateCommand("SelectDetailForEmitraPaymentForSocietyMemberRegistration", DBFactoryCommandType.PROCEDURE_COMMAND);
                }
                else
                {
                    objDBAccess.CreateCommand("SelectDetailForEmitraPayment", DBFactoryCommandType.PROCEDURE_COMMAND);
                }
                objDBAccess.AddParameterToCommand("WhereClause", DBFactoryParameterType.NVarchar, whereClause);
                objDBAccess.OpenConnection();
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModelFactory", FunctionName = "SelectDetailForEmitraPayment(AppRequestID)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }

        public DataTable GetSocietyPaymentList(string WhereClause)
        {

            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyPayment", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModel", FunctionName = "GetSocietyPaymentList(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetCAAuditorPaymentDetails(string AuditEmpanelledId)
        {

            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAuditEmpanelledDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@AuditEmpanelledId", DBFactoryParameterType.NVarchar, AuditEmpanelledId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModel", FunctionName = "GetSocietyPaymentList(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetPLDBMemberPaymentDetails(string MemberId)
        {

            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetPLDBMemberPaymentDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MemberId", DBFactoryParameterType.NVarchar, MemberId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModel", FunctionName = "GetPLDBMemberPaymentDetails(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable SocietyForUploadBankCertificate(string WhereClause, string SocietyIdEnc = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyForBankCertificate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModel", FunctionName = "SocietyForUploadBankCertificate(string WhereClause,string SocietyIdEnc=\"\")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public string FeeDeposit(string SocietyIdEnc, string ApplicationNo, string ConsumerName, string IsPermanentSave = "0", string Amount = "0")
        {
            string returnmsg = "";
            int K = -1, i = -1;
            string kioskCode = "";
            string ReceptNo = "";
            string KTimeStamp = "";
            ARequest emitra = (ARequest)HttpContext.Current.Session["ARequest"];
            try
            {
                #region "insert into Emitrakiosktransaction"
                objDBAccess.CreateCommand("InsertEmitraKioskTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("SocietyIdEnc", DBFactoryParameterType.VARCHAR, SocietyIdEnc);//
                objDBAccess.AddParameterToCommand("ApplicationNo", DBFactoryParameterType.VARCHAR, ApplicationNo);//
                objDBAccess.AddParameterToCommand("KioskID", DBFactoryParameterType.VARCHAR, emitra.KIOSKCODE.Length > 0 ? emitra.KIOSKCODE : "SSO");//
                objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, "");//
                objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("RequestStatus", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("ActId", DBFactoryParameterType.Int, 0);//                    
                objDBAccess.AddParameterToCommand("SSOID", DBFactoryParameterType.VARCHAR, emitra.KIOSKCODE.Length > 0 ? emitra.SSOID : emitra.SSOID);
                objDBAccess.AddParameterToCommand("CreatedIP", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("SERICEID", DBFactoryParameterType.BigInt, emitra.SERVICEID);
                objDBAccess.OpenConnection();
                object oi = objDBAccess.ExecuteScalarCommand();
                K = Convert.ToInt32(oi);
                #endregion

                DataTable Dt = null;
                Dt = GetApplicationKey(emitra.SERVICEID, true);
                if (Dt.Rows.Count > 0)
                {
                    var ONE = Dt.Rows[0]["REVENUHEADKIOSKINTEGRATION"].ToString().Split('|');
                    var one = ONE[0].Split('-');
                    var tw = ONE[0].Split('-');
                    string PAY = one[0].ToString() + "-" + Amount + "|" + ONE[1].ToString();
                    DRequest req = new DRequest();
                    req.MERCHANTCODE = Dt.Rows[0]["MERCHANTCODE"].ToString();
                    req.REQUESTID = K.ToString();
                    req.REQTIMESTAMP = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    req.SERVICEID = Dt.Rows[0]["SERICEID"].ToString();
                    req.SUBSERVICEID = "";
                    req.REVENUEHEAD = PAY;   //Dt.Rows[0]["REVENUHEADKIOSKINTEGRATION"].ToString();
                    req.CONSUMERKEY = K + "-" + ApplicationNo;
                    req.CONSUMERNAME = ConsumerName.Replace("-", "");
                    req.COMMTYPE = "3";
                    req.SSOID = emitra.SSOID;
                    req.OFFICECODE = Dt.Rows[0]["OFFICECODE"].ToString();// MandiID.ToString();
                    req.SSOTOKEN = emitra.SSOTOKEN;

                    DRequestChecksum reqChecksum = new DRequestChecksum();
                    req.CHECKSUM = reqChecksum.GetCheckSum(req);


                    #region "REQUEST UPDATE"
                    objDBAccess.CreateCommand("UpdateEmitraKioskTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("Emitraapprequestid", DBFactoryParameterType.BigInt, K);
                    objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, JsonConvert.SerializeObject(req));
                    objDBAccess.AddParameterToCommand("UpdateFlag", DBFactoryParameterType.BigInt, 1);
                    objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, "");
                    objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, "");
                    objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, "");
                    objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, "");
                    objDBAccess.AddParameterToCommand("RegistrationNo", DBFactoryParameterType.VARCHAR, ApplicationNo);
                    objDBAccess.AddParameterToCommand("ServiceId", DBFactoryParameterType.Int, emitra.SERVICEID);
                    objDBAccess.AddParameterToCommand("SocietyIdEnc", DBFactoryParameterType.VARCHAR, SocietyIdEnc);

                    objDBAccess.OpenConnection();
                    object o = objDBAccess.ExecuteScalarCommand();
                    i = Convert.ToInt32(o);

                    #endregion

                    string retVal = MakeTransactionsEncrypted(Dt.Rows[0]["T_Url"].ToString(), JsonConvert.SerializeObject(req), Dt.Rows[0]["encryptionkey"].ToString());

                    string decVal = Decrypt(retVal, Dt.Rows[0]["encryptionkey"].ToString());
                    DResponse resp = JsonConvert.DeserializeObject<DResponse>(decVal);

                    #region "Update into Emitrakiosktransaction"
                    try
                    {
                        if (resp != null)
                        {
                            //UPDATE RESPONCE
                            objDBAccess.CreateCommand("UpdateEmitraKioskTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                            objDBAccess.AddParameterToCommand("Emitraapprequestid", DBFactoryParameterType.BigInt, K);
                            objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, "");
                            objDBAccess.AddParameterToCommand("UpdateFlag", DBFactoryParameterType.BigInt, 2);
                            objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, resp.RECEIPTNO);
                            objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, resp.TRANSACTIONID);
                            objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, resp.MSG);
                            objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, JsonConvert.SerializeObject(resp));
                            objDBAccess.AddParameterToCommand("RegistrationNo", DBFactoryParameterType.VARCHAR, ApplicationNo);
                            objDBAccess.AddParameterToCommand("ServiceId", DBFactoryParameterType.Int, emitra.SERVICEID);
                            objDBAccess.AddParameterToCommand("SocietyIdEnc", DBFactoryParameterType.VARCHAR, SocietyIdEnc);
                            objDBAccess.AddParameterToCommand("IsPermanentSave", DBFactoryParameterType.Int, Convert.ToInt32(IsPermanentSave));

                            objDBAccess.OpenConnection();
                            object p = objDBAccess.ExecuteScalarCommand();
                            i = Convert.ToInt32(p);
                            returnmsg = resp.MSG;
                            ReceptNo = resp.RECEIPTNO;
                            kioskCode = emitra.KIOSKNAME + (emitra.KIOSKCODE);
                            KTimeStamp = resp.EMITRATIMESTAMP.ToString().Substring(0, 8);
                        }
                    }
                    catch (Exception ex)
                    {
                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModel", FunctionName = "FeeDeposit(string SocietyIdEnc, string ApplicationNo, string ConsumerName, string IsPermanentSave = '0')" }, ex);
                    }
                    finally
                    {
                        objDBAccess.CloseConnection();
                    }
                    #endregion
                }
                else
                {
                    returnmsg = "Error In Update Request";
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModel", FunctionName = "FeeDeposit(string SocietyIdEnc, string ApplicationNo, string ConsumerName, string IsPermanentSave = '0')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return returnmsg + "@" + ReceptNo + "@" + kioskCode + "@" + KTimeStamp + "@";
        }


        public string VerifyTransaction(SocietyRegistrationEntity ObjSocietyRegistration)
        {
            DataTable Dt = null;
            string returnval = "";
            VerifyResponse resp = null;
            PGResponse respg = null;
            int i = -1;

            try
            {
                Dt = new EmitraModelFactory(new FileLogger()).GetApplicationKey(ObjSocietyRegistration.ServicesId.ToString(), ObjSocietyRegistration.UserType == "K" ? true : false);

                if (Dt.Rows.Count > 0 && ObjSocietyRegistration.UserType == "K")
                {

                     VerifyRequest req = new VerifyRequest();
                    req.SSOTOKEN = "0";
                    req.MERCHANTCODE = Dt.Rows[0]["merchantcode"].ToString(); ;
                    req.REQUESTID = ObjSocietyRegistration.EMitraAppRequestID;
                    req.SERVICEID = Dt.Rows[0]["SERICEID"].ToString();
                    VerifyTransChecksum reqChecksum = new VerifyTransChecksum();
                    req.CHECKSUM = reqChecksum.GetCheckSum(req);
                    string retVal = MakeTransactionsEncrypted(Dt.Rows[0]["V_Url"].ToString(), JsonConvert.SerializeObject(req), Dt.Rows[0]["encryptionkey"].ToString());
                    string decVal = Decrypt(retVal, Dt.Rows[0]["encryptionkey"].ToString());
                    resp = JsonConvert.DeserializeObject<VerifyResponse>(decVal);
                }
                else
                {
                    string pgverifyurl = Dt.Rows[0]["V_Url"].ToString() + "?MERCHANTCODE=" + Dt.Rows[0]["merchantcode"].ToString() + "&SERVICEID=" + Dt.Rows[0]["SERICEID"].ToString() + "&PRN=" + ObjSocietyRegistration.EMitraAppRequestID + "";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(pgverifyurl);
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Method = "POST";
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Close();
                    HttpWebResponse response;
                    response = (HttpWebResponse)request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    string retVal = new StreamReader(responseStream).ReadToEnd();
                    respg = JsonConvert.DeserializeObject<PGResponse>(retVal);
                }

                if (resp != null || respg != null)
                {

                    //UPDATE RESPONCE
                    objDBAccess.CreateCommand("UpdateEmitraKioskTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("Emitraapprequestid", DBFactoryParameterType.BigInt, int.Parse(ObjSocietyRegistration.EMitraAppRequestID));
                    objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, "");
                    objDBAccess.AddParameterToCommand("updateflag", DBFactoryParameterType.BigInt, 2);
                    objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, resp == null ? respg.RECEIPTNO : resp.RECEIPTNO);
                    objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, resp == null ? respg.TRANSACTIONID : resp.TRANSACTIONID);
                    objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, resp == null ? respg.STATUS : resp.MSG);
                    objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, resp == null ? JsonConvert.SerializeObject(respg) : JsonConvert.SerializeObject(resp));
                    objDBAccess.AddParameterToCommand("RegistrationNo", DBFactoryParameterType.VARCHAR, RegistrationNo);
                    objDBAccess.AddParameterToCommand("ServiceId", DBFactoryParameterType.Int, ObjSocietyRegistration.ServicesId);
                    objDBAccess.AddParameterToCommand("SocietyIdEnc", DBFactoryParameterType.VARCHAR, ObjSocietyRegistration.SocietyIDEnc);
                    objDBAccess.AddParameterToCommand("IsPermanentSave", DBFactoryParameterType.Int, 1);//IsPermanentSave
                    objDBAccess.OpenConnection();
                    object o = objDBAccess.ExecuteScalarCommand();
                    i = Convert.ToInt32(o);
                    returnval = i.ToString();
                    //returnvalue = i;
                }
                if (ObjSocietyRegistration.ServicesId == 5530 && i > 0)
                {
                    try
                    {
                        //UPDATE RESPONCE
                        objDBAccess.CreateCommand("SaveSocietyDetailsBySocietyIdFormTempTbl", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("Emitraapprequestid", DBFactoryParameterType.BigInt, int.Parse(ObjSocietyRegistration.EMitraAppRequestID));
                        objDBAccess.AddParameterToCommand("SocietyIdEnc", DBFactoryParameterType.VARCHAR, ObjSocietyRegistration.SocietyIDEnc);
                        objDBAccess.OpenConnection();
                        object o = objDBAccess.ExecuteScalarCommand();
                        i = Convert.ToInt32(o);
                        returnval = i.ToString();
                        if (i == 1)
                        {
                            ApplicationFlowModel AppFlowModal = new ApplicationFlowModel();
                            AppFlowModal.SendMSG(ObjSocietyRegistration.SocietyIDEnc, -2);
                        }
                    }
                    catch (Exception ex)
                    {
                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModelFactory", FunctionName = "VerifyTransaction" }, ex);
                    }
                    finally
                    {
                        objDBAccess.CloseConnection();
                        dt.Dispose();
                    }
                }
                if ((ObjSocietyRegistration.ServicesId == 5691) && i > 0)
                {
                    try
                    {
                        SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                        returnval = objSocietyModel.SaveBankDetailForKioskmodel(ObjSocietyRegistration.UniqueId, ObjSocietyRegistration.SocietyIDEnc);
                    }
                    catch (Exception ex)
                    {
                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModelFactory", FunctionName = "VerifyTransaction" }, ex);
                    }
                    finally
                    {
                        objDBAccess.CloseConnection();
                        dt.Dispose();
                    }
                }
                if ((ObjSocietyRegistration.ServicesId == 5531) && i > 0)
                {
                    try
                    {
                        string PayStatus = (resp == null ? respg.STATUS : resp.MSG);
                        if (PayStatus.ToUpper().Contains("SUCCESS"))
                        {
                            ApplicationFlowModel AFM = new ApplicationFlowModel();
                            AFM.AddApplicationFlow(ObjSocietyRegistration.SocietyIDEnc, 15, "PAYMENT RECEIVED", 0);
                            AFM.SendMSG(ObjSocietyRegistration.SocietyIDEnc, 15);
                        }

                        if (resp == null)
                            returnval = JsonConvert.SerializeObject(respg);
                        else
                            returnval = JsonConvert.SerializeObject(resp);
                    }
                    catch (Exception ex)
                    {
                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModelFactory", FunctionName = "VerifyTransaction" }, ex);
                    }
                    finally
                    {
                        objDBAccess.CloseConnection();
                        dt.Dispose();
                    }
                }
                if (i > 0 && ObjSocietyRegistration.ServiceId == 4173)
                {
                    ApplicationFlowModel AFM = new ApplicationFlowModel();
                    AFM.SendMSG(ObjSocietyRegistration.SocietyIDEnc, 24);
                    returnval = JsonConvert.SerializeObject(resp);
                }

            }
            catch (Exception ex)
            {
                returnval = "-1";
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModel", FunctionName = "FeeDeposit(string SocietyIdEnc, string ApplicationNo, string ConsumerName, string IsPermanentSave = '0')" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return returnval;
        }

        public string GetPaymentTransaction(SocietyRegistrationEntity ObjSocietyRegistration)
        {
            DataTable Dt = null;
            string returnval = "-1";
            VerifyResponse resp = null;
            PGResponse respg = null;
            int i = -1;

            try
            {
                Dt = new EmitraModelFactory(new FileLogger()).GetApplicationKey(ObjSocietyRegistration.ServicesId.ToString(), ObjSocietyRegistration.UserType == "K" ? true : false);

                if (Dt.Rows.Count > 0 && ObjSocietyRegistration.UserType == "K")
                {

                    VerifyRequest req = new VerifyRequest();
                    req.SSOTOKEN = "0";
                    req.MERCHANTCODE = Dt.Rows[0]["merchantcode"].ToString(); ;
                    req.REQUESTID = ObjSocietyRegistration.EMitraAppRequestID;
                    req.SERVICEID = Dt.Rows[0]["SERICEID"].ToString();
                    VerifyTransChecksum reqChecksum = new VerifyTransChecksum();
                    req.CHECKSUM = reqChecksum.GetCheckSum(req);
                    string retVal = MakeTransactionsEncrypted(Dt.Rows[0]["V_Url"].ToString(), JsonConvert.SerializeObject(req), Dt.Rows[0]["encryptionkey"].ToString());
                    returnval = Decrypt(retVal, Dt.Rows[0]["encryptionkey"].ToString());
                    resp = JsonConvert.DeserializeObject<VerifyResponse>(returnval);
                }
                else
                {
                    string pgverifyurl = Dt.Rows[0]["V_Url"].ToString() + "?MERCHANTCODE=" + Dt.Rows[0]["merchantcode"].ToString() + "&SERVICEID=" + Dt.Rows[0]["SERICEID"].ToString() + "&PRN=" + ObjSocietyRegistration.EMitraAppRequestID + "";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(pgverifyurl);
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Method = "POST";
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Close();
                    HttpWebResponse response;
                    response = (HttpWebResponse)request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    returnval = new StreamReader(responseStream).ReadToEnd();
                    respg = JsonConvert.DeserializeObject<PGResponse>(returnval);
                }

                //if (resp != null || respg != null)
                //{
                //    returnval = (resp == null ? respg.STATUS : resp.MSG);
                //}
                
            }
            catch (Exception ex)
            {
                returnval = "-1";
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModel", FunctionName = "FeeDeposit(string SocietyIdEnc, string ApplicationNo, string ConsumerName, string IsPermanentSave = '0')" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return returnval;
        }

        public string RefundTransaction(SocietyRegistrationEntity ObjSocietyRegistration)
        {
            DataTable Dt = null;
            string returnval = "";
            VerifyResponse resp = null;
            PGResponse respg = null;
            int i = -1;

            try
            {
                Dt = new EmitraModelFactory(new FileLogger()).GetApplicationKey(ObjSocietyRegistration.ServicesId.ToString(), ObjSocietyRegistration.UserType == "K" ? true : false);



               

                if (Dt.Rows.Count > 0 && ObjSocietyRegistration.UserType == "K")
                {

                    VerifyRequest req = new VerifyRequest();
                    req.SSOTOKEN = "0";
                    req.MERCHANTCODE = Dt.Rows[0]["merchantcode"].ToString(); ;
                    req.REQUESTID = ObjSocietyRegistration.EMitraAppRequestID;
                    req.SERVICEID = Dt.Rows[0]["SERICEID"].ToString();
                    VerifyTransChecksum reqChecksum = new VerifyTransChecksum();
                    req.CHECKSUM = reqChecksum.GetCheckSum(req);
                    string retVal = MakeTransactionsEncrypted(Dt.Rows[0]["V_Url"].ToString(), JsonConvert.SerializeObject(req), Dt.Rows[0]["encryptionkey"].ToString());
                    string decVal = Decrypt(retVal, Dt.Rows[0]["encryptionkey"].ToString());
                    resp = JsonConvert.DeserializeObject<VerifyResponse>(decVal);
                }
                else
                {

                    var ONE = Dt.Rows[0]["REVENUHEADKIOSKINTEGRATION"].ToString().Split('|');
                    var one = ONE[0].Split('-');
                    var tw = ONE[0].Split('-');
                    //string PAY = one[0].ToString() + "-" + Amount + "|" + ONE[1].ToString();
                    DRequest req = new DRequest();
                    req.MERCHANTCODE = Dt.Rows[0]["merchantcode"].ToString();
                    //req.REQUESTID = K.ToString();
                    //req.REQTIMESTAMP = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    req.SERVICEID = Dt.Rows[0]["SERICEID"].ToString();
                    req.REQUESTID = ObjSocietyRegistration.EMitraAppRequestID;
                    //req.REVENUEHEAD = PAY;   //Dt.Rows[0]["REVENUHEADKIOSKINTEGRATION"].ToString();
                    //req.CONSUMERKEY = K + "-" + ApplicationNo;
                    //req.CONSUMERNAME = ConsumerName;
                    //req.COMMTYPE = "1";
                    //req.SSOID = emitra.SSOID;
                    //req.OFFICECODE = Dt.Rows[0]["OFFICECODE"].ToString();// MandiID.ToString();
                    //req.SSOTOKEN = emitra.SSOTOKEN;

                    string URL = "http://emitrauat.rajasthan.gov.in/webServicesRepositoryUat/rejectPGTransacti";//Dt.Rows[0]["V_Url"].ToString()
                    DRequestChecksum reqChecksum = new DRequestChecksum();
                    req.CHECKSUM = reqChecksum.GetCheckSum(req);


                    //LogErrorToLogFile error = new LogErrorToLogFile();

                    //error.LogEmitra("Kiosk Verify " + data, "TestApp1");

                   // string encData = Encrypt(data, encryptionKey);
                    //error.LogEmitra("Encrypted Request Data: Encrypt Data: " + encData, "TestApp2");
                    //Base String
                    string baseAddress = URL;
                    //Post Parameters
                    StringBuilder postData = new StringBuilder();
                    postData.Append("MERCHANT_CODE=" + HttpUtility.UrlEncode(Dt.Rows[0]["merchantcode"].ToString()));
                    postData.Append("SECRET_KEY =" + HttpUtility.UrlEncode("E-m!tr@2016"));
                    postData.Append("SERVICE_ID=" + HttpUtility.UrlEncode(Dt.Rows[0]["SERICEID"].ToString()));
                    postData.Append("REQUEST_ID=" + HttpUtility.UrlEncode(ObjSocietyRegistration.EMitraAppRequestID));

                    //Create Web Request
                    var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
                    http.Method = "POST";
                    http.Accept = "application/json";
                    http.ContentType = "application/x-www-form-urlencoded";

                    //Start Writing Post parameters to request object
                    string parsedContent = postData.ToString();
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    Byte[] bytes = encoding.GetBytes(parsedContent);
                    Stream newStream = http.GetRequestStream();
                    newStream.Write(bytes, 0, bytes.Length);
                    newStream.Close();

                    //Read Response for posting done
                    var response = http.GetResponse();
                    var stream = response.GetResponseStream();
                    var sr = new StreamReader(stream);
                    var content = sr.ReadToEnd();
                    //return contents
                    return content;







                    //string retVal = MakeTransactionsEncrypted(URL, JsonConvert.SerializeObject(req), Dt.Rows[0]["encryptionkey"].ToString());


                    //string pgverifyurl = URL + "?MERCHANT_CODE=" + Dt.Rows[0]["merchantcode"].ToString() + "&SERVICE_ID=" + Dt.Rows[0]["SERICEID"].ToString() + "&REQUEST_ID =" + ObjSocietyRegistration.EMitraAppRequestID + "";
                    //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(pgverifyurl);
                    //request.ContentType = "application/x-www-form-urlencoded";
                    //request.Method = "POST";
                    //Stream requestStream = request.GetRequestStream();
                    //requestStream.Close();
                    //HttpWebResponse response;
                    //response = (HttpWebResponse)request.GetResponse();
                    //Stream responseStream = response.GetResponseStream();
                    //string retVal = new StreamReader(responseStream).ReadToEnd();
                    //respg = JsonConvert.DeserializeObject<PGResponse>(retVal);
                }

                if (resp != null || respg != null)
                {

                    //UPDATE RESPONCE
                    objDBAccess.CreateCommand("UpdateEmitraKioskTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("Emitraapprequestid", DBFactoryParameterType.BigInt, int.Parse(ObjSocietyRegistration.EMitraAppRequestID));
                    objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, "");
                    objDBAccess.AddParameterToCommand("updateflag", DBFactoryParameterType.BigInt, 2);
                    objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, resp == null ? respg.RECEIPTNO : resp.RECEIPTNO);
                    objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, resp == null ? respg.TRANSACTIONID : resp.TRANSACTIONID);
                    objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, resp == null ? respg.STATUS : resp.MSG);
                    objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, resp == null ? JsonConvert.SerializeObject(respg) : JsonConvert.SerializeObject(resp));
                    objDBAccess.AddParameterToCommand("RegistrationNo", DBFactoryParameterType.VARCHAR, RegistrationNo);
                    objDBAccess.AddParameterToCommand("ServiceId", DBFactoryParameterType.Int, ObjSocietyRegistration.ServicesId);
                    objDBAccess.AddParameterToCommand("SocietyIdEnc", DBFactoryParameterType.VARCHAR, ObjSocietyRegistration.SocietyIDEnc);
                    objDBAccess.AddParameterToCommand("IsPermanentSave", DBFactoryParameterType.Int, 1);//IsPermanentSave
                    objDBAccess.OpenConnection();
                    object o = objDBAccess.ExecuteScalarCommand();
                    i = Convert.ToInt32(o);
                    returnval = i.ToString();
                    //returnvalue = i;
                }  
            }
            catch (Exception ex)
            {
                returnval = "-1";
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModel", FunctionName = "FeeDeposit(string SocietyIdEnc, string ApplicationNo, string ConsumerName, string IsPermanentSave = '0')" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return returnval;
        }



        public int MakePaymentForAll(string UniquerequestId, string ConsumerName, string damnt, string RegistrationNo, string SocietyId, string KIOSKCODE, string SSOID)
        {
            DataTable Dt = null;
            int returnvalue = -1;
            string ServiceName = "";
            if (HttpContext.Current.Session["ARequest"] != null)
            {
                ARequest emitra = (ARequest)HttpContext.Current.Session["ARequest"];
                if (emitra != null)
                {

                    if (UniquerequestId == "0")
                    {
                        #region "insert into Emitrakiosktransaction"
                        objDBAccess.CreateCommand("insertemitrakiosktransactionAll", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("SocietyId", DBFactoryParameterType.Int, int.Parse(SocietyId));//
                        objDBAccess.AddParameterToCommand("ApplicationNo", DBFactoryParameterType.VARCHAR, RegistrationNo);//
                        objDBAccess.AddParameterToCommand("KioskID", DBFactoryParameterType.VARCHAR, KIOSKCODE.Length > 0 ? KIOSKCODE : "SSO");//
                        objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, "");//
                        objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, "");
                        objDBAccess.AddParameterToCommand("RequestStatus", DBFactoryParameterType.VARCHAR, "");
                        objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, "");
                        objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, "");
                        objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, "");
                        objDBAccess.AddParameterToCommand("ActId", DBFactoryParameterType.Int, 0);//                    
                        objDBAccess.AddParameterToCommand("SSOID", DBFactoryParameterType.VARCHAR, SSOID);
                        objDBAccess.AddParameterToCommand("CreatedIP", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIpAddress());
                        objDBAccess.AddParameterToCommand("SERICEID", DBFactoryParameterType.BigInt, int.Parse(emitra.SERVICEID.ToString()));
                        objDBAccess.OpenConnection();
                        object oi = objDBAccess.ExecuteScalarCommand();
                        UniquerequestId = Convert.ToString(oi);
                        #endregion
                    }

                    EmitraModelFactory objEmitraModel = new EmitraModelFactory(new FileLogger());
                    Dt = objEmitraModel.GetApplicationKey(emitra.SERVICEID.ToString(), true);
                    if (Dt.Rows.Count > 0)
                    {
                        double dAmt = Convert.ToDouble(damnt);
                        double cAmt = 0;
                        ServiceName = GerServiceName(emitra.SERVICEID.ToString());

                        DRequest req = new DRequest();
                        req.MERCHANTCODE = Dt.Rows[0]["MERCHANTCODE"].ToString();
                        req.REQUESTID = UniquerequestId;
                        req.REQTIMESTAMP = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        req.SERVICEID = Dt.Rows[0]["SERICEID"].ToString();
                        cAmt = Convert.ToDouble(Dt.Rows[0]["COMTYPE"].ToString());
                        req.SUBSERVICEID = "";
                        req.REVENUEHEAD = Dt.Rows[0]["REVENUHEADKIOSKINTEGRATION"].ToString();
                        req.CONSUMERKEY = UniquerequestId;
                        req.CONSUMERNAME = ConsumerName;
                        req.COMMTYPE = "3";
                        req.SSOID = emitra.SSOID;
                        req.OFFICECODE = Dt.Rows[0]["OFFICECODE"].ToString();
                        req.SSOTOKEN = emitra.SSOTOKEN;

                        DRequestChecksum reqChecksum = new DRequestChecksum();
                        req.CHECKSUM = reqChecksum.GetCheckSum(req);

                        int i = -1;
                        objDBAccess.OpenConnection();
                        try
                        {
                            if (req != null)
                            {
                                //UPDATE REQUEST
                                objDBAccess.CreateCommand("UpdateEmitraKioskTransactionForAll", DBFactoryCommandType.PROCEDURE_COMMAND);
                                objDBAccess.AddParameterToCommand("Emitraapprequestid", DBFactoryParameterType.BigInt, int.Parse(UniquerequestId));
                                objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, JsonConvert.SerializeObject(req));
                                objDBAccess.AddParameterToCommand("updateflag", DBFactoryParameterType.BigInt, 1);
                                objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, "");
                                objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, "");
                                objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, "");
                                objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, "");
                                objDBAccess.AddParameterToCommand("RegistrationNo", DBFactoryParameterType.VARCHAR, RegistrationNo);
                                objDBAccess.AddParameterToCommand("ServiceId", DBFactoryParameterType.Int, emitra.SERVICEID);
                                objDBAccess.AddParameterToCommand("SocietyId", DBFactoryParameterType.Int, SocietyId);
                                objDBAccess.OpenConnection();
                                object o = objDBAccess.ExecuteScalarCommand();
                                i = Convert.ToInt32(o);
                            }
                        }
                        catch (Exception ex)
                        {
                            objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModelFactory", FunctionName = "MakePayment(string UniquerequestId, string ConsumerName, string damnt, string RegistrationNo, string SocietyIdEnc,int IsPermanentSave)" }, ex);
                        }
                        finally
                        {
                            objDBAccess.CloseConnection();
                            dt.Dispose();
                        }

                        if (i > 0)
                        {
                            try
                            {
                                string retVal = MakeTransactionsEncrypted(Dt.Rows[0]["T_Url"].ToString(), JsonConvert.SerializeObject(req), Dt.Rows[0]["ENCRYPTIONKEY"].ToString());
                                string decVal = Decrypt(retVal, Dt.Rows[0]["ENCRYPTIONKEY"].ToString());
                                DResponse resp = JsonConvert.DeserializeObject<DResponse>(decVal);
                                if (resp != null)
                                {
                                    //UPDATE RESPONCE
                                    objDBAccess.CreateCommand("UpdateEmitraKioskTransactionForAll", DBFactoryCommandType.PROCEDURE_COMMAND);
                                    objDBAccess.AddParameterToCommand("Emitraapprequestid", DBFactoryParameterType.BigInt, int.Parse(UniquerequestId));
                                    objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, "");
                                    objDBAccess.AddParameterToCommand("updateflag", DBFactoryParameterType.BigInt, 2);
                                    objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, resp.RECEIPTNO);
                                    objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, resp.TRANSACTIONID);
                                    objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, resp.MSG);
                                    objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, JsonConvert.SerializeObject(resp));
                                    objDBAccess.AddParameterToCommand("RegistrationNo", DBFactoryParameterType.VARCHAR, RegistrationNo);
                                    objDBAccess.AddParameterToCommand("ServiceId", DBFactoryParameterType.Int, emitra.SERVICEID);
                                    objDBAccess.AddParameterToCommand("SocietyId", DBFactoryParameterType.Int, SocietyId);
                                    objDBAccess.OpenConnection();
                                    object o = objDBAccess.ExecuteScalarCommand();
                                    i = Convert.ToInt32(o);
                                    //returnvalue = resp.MSG;
                                    returnvalue = i;
                                }
                                else
                                {

                                }
                            }
                            catch (Exception ex)
                            {
                                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModelFactory", FunctionName = "MakePayment(string UniquerequestId, string ConsumerName, string damnt, string RegistrationNo, string SocietyIdEnc,int IsPermanentSave)" }, ex);
                            }
                            finally
                            {
                                objDBAccess.CloseConnection();
                                dt.Dispose();
                            }
                        }
                    }
                    else
                    {
                        returnvalue = -1;
                    }
                }
            }
            return returnvalue;
        }

        public DataTable CCSComplaintRegisterData(int ComplaintId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CCSComplaintRegisterList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ComplaintId", DBFactoryParameterType.Int, ComplaintId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModel", FunctionName = "CCSComplaintRegisterData(int ComplaintId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable CCSComplaintRegisterDataByReceiptNo(string ReceiptNo)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CCSComplaintRegisterListByReceiptNo", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ReceiptNo", DBFactoryParameterType.NVarchar, ReceiptNo);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModel", FunctionName = "public DataTable CCSComplaintRegisterDataByReceiptNo(string ReceiptNo)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public string FeeDepositForCAAuditor(string SocietyIdEnc, string ApplicationNo, string ConsumerName, string IsPermanentSave = "0", string Amount = "0")
        {
            string returnmsg = "";
            int K = -1, i = -1;
            string kioskCode = "";
            string ReceptNo = "";
            string KTimeStamp = "";
            ARequest emitra = (ARequest)HttpContext.Current.Session["ARequest"];
            try
            {
                #region "insert into Emitrakiosktransaction"
                objDBAccess.CreateCommand("InsertEmitraKioskTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("SocietyIdEnc", DBFactoryParameterType.VARCHAR, SocietyIdEnc);//
                objDBAccess.AddParameterToCommand("ApplicationNo", DBFactoryParameterType.VARCHAR, ApplicationNo);//
                objDBAccess.AddParameterToCommand("KioskID", DBFactoryParameterType.VARCHAR, emitra.KIOSKCODE.Length > 0 ? emitra.KIOSKCODE : "SSO");//
                objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, "");//
                objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("RequestStatus", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("ActId", DBFactoryParameterType.Int, 0);//                    
                objDBAccess.AddParameterToCommand("SSOID", DBFactoryParameterType.VARCHAR, emitra.KIOSKCODE.Length > 0 ? emitra.SSOID : emitra.SSOID);
                objDBAccess.AddParameterToCommand("CreatedIP", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("SERICEID", DBFactoryParameterType.BigInt, emitra.SERVICEID);
                objDBAccess.OpenConnection();
                object oi = objDBAccess.ExecuteScalarCommand();
                K = Convert.ToInt32(oi);
                #endregion

                DataTable Dt = null;
                Dt = GetApplicationKey(emitra.SERVICEID, true);
                if (Dt.Rows.Count > 0)
                {
                    var ONE = Dt.Rows[0]["REVENUHEADKIOSKINTEGRATION"].ToString().Split('|');
                    var one = ONE[0].Split('-');
                    var tw = ONE[0].Split('-');
                    string PAY = one[0].ToString() + "-" + Amount + "|" + ONE[1].ToString();
                    DRequest req = new DRequest();
                    req.MERCHANTCODE = Dt.Rows[0]["MERCHANTCODE"].ToString();
                    req.REQUESTID = K.ToString();
                    req.REQTIMESTAMP = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    req.SERVICEID = Dt.Rows[0]["SERICEID"].ToString();
                    req.SUBSERVICEID = "";
                    req.REVENUEHEAD = PAY;   //Dt.Rows[0]["REVENUHEADKIOSKINTEGRATION"].ToString();
                    req.CONSUMERKEY = K + "-" + ApplicationNo;
                    req.CONSUMERNAME = ConsumerName;
                    req.COMMTYPE = "3";
                    req.SSOID = emitra.SSOID;
                    req.OFFICECODE = Dt.Rows[0]["OFFICECODE"].ToString();// MandiID.ToString();
                    req.SSOTOKEN = emitra.SSOTOKEN;

                    DRequestChecksum reqChecksum = new DRequestChecksum();
                    req.CHECKSUM = reqChecksum.GetCheckSum(req);


                    #region "REQUEST UPDATE"
                    objDBAccess.CreateCommand("UpdateEmitraKioskTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("Emitraapprequestid", DBFactoryParameterType.BigInt, K);
                    objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, JsonConvert.SerializeObject(req));
                    objDBAccess.AddParameterToCommand("UpdateFlag", DBFactoryParameterType.BigInt, 1);
                    objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, "");
                    objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, "");
                    objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, "");
                    objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, "");
                    objDBAccess.AddParameterToCommand("RegistrationNo", DBFactoryParameterType.VARCHAR, ApplicationNo);
                    objDBAccess.AddParameterToCommand("ServiceId", DBFactoryParameterType.Int, emitra.SERVICEID);
                    objDBAccess.AddParameterToCommand("SocietyIdEnc", DBFactoryParameterType.VARCHAR, SocietyIdEnc);

                    objDBAccess.OpenConnection();
                    object o = objDBAccess.ExecuteScalarCommand();
                    i = Convert.ToInt32(o);

                    #endregion

                    string retVal = MakeTransactionsEncrypted(Dt.Rows[0]["T_Url"].ToString(), JsonConvert.SerializeObject(req), Dt.Rows[0]["encryptionkey"].ToString());

                    string decVal = Decrypt(retVal, Dt.Rows[0]["encryptionkey"].ToString());
                    DResponse resp = JsonConvert.DeserializeObject<DResponse>(decVal);

                    #region "Update into Emitrakiosktransaction"
                    try
                    {
                        if (resp != null)
                        {
                            //UPDATE RESPONCE
                            objDBAccess.CreateCommand("UpdateEmitraKioskTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                            objDBAccess.AddParameterToCommand("Emitraapprequestid", DBFactoryParameterType.BigInt, K);
                            objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, "");
                            objDBAccess.AddParameterToCommand("UpdateFlag", DBFactoryParameterType.BigInt, 2);
                            objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, resp.RECEIPTNO);
                            objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, resp.TRANSACTIONID);
                            objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, resp.MSG);
                            objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, JsonConvert.SerializeObject(resp));
                            objDBAccess.AddParameterToCommand("RegistrationNo", DBFactoryParameterType.VARCHAR, ApplicationNo);
                            objDBAccess.AddParameterToCommand("ServiceId", DBFactoryParameterType.Int, emitra.SERVICEID);
                            objDBAccess.AddParameterToCommand("SocietyIdEnc", DBFactoryParameterType.VARCHAR, SocietyIdEnc);
                            objDBAccess.AddParameterToCommand("IsPermanentSave", DBFactoryParameterType.Int, Convert.ToInt32(IsPermanentSave));

                            objDBAccess.OpenConnection();
                            object p = objDBAccess.ExecuteScalarCommand();
                            i = Convert.ToInt32(p);
                            returnmsg = resp.MSG;
                            ReceptNo = resp.RECEIPTNO;
                            kioskCode = emitra.KIOSKNAME + (emitra.KIOSKCODE);
                            KTimeStamp = resp.EMITRATIMESTAMP.ToString().Substring(0, 8);
                            if (i == 1)
                            {
                                AuditorModel objEM = new AuditorModel(new FileLogger());
                                objEM.SetAuditEmpanelledFinalSubmit(K.ToString());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModel", FunctionName = "FeeDepositForCAAuditor(string SocietyIdEnc, string ApplicationNo, string ConsumerName, string IsPermanentSave = '0')" }, ex);
                    }
                    finally
                    {
                        objDBAccess.CloseConnection();
                    }
                    #endregion
                }
                else
                {
                    returnmsg = "Error In Update Request";
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModel", FunctionName = "FeeDepositForCAAuditor(string SocietyIdEnc, string ApplicationNo, string ConsumerName, string IsPermanentSave = '0')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return returnmsg + "@" + ReceptNo + "@" + kioskCode + "@" + KTimeStamp + "@";
        }

        public string FeeDepositForPLDBMembership(string SocietyIdEnc, string ApplicationNo, string ConsumerName,string OfficeCode, string IsPermanentSave = "0", string Amount = "0")
        {
            string returnmsg = "";
            int K = -1, i = -1;
            string kioskCode = "";
            string ReceptNo = "";
            string KTimeStamp = "";
            ARequest emitra = (ARequest)HttpContext.Current.Session["ARequest"];
            try
            {
                #region "insert into Emitrakiosktransaction"
                objDBAccess.CreateCommand("InsertEmitraKioskTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("SocietyIdEnc", DBFactoryParameterType.VARCHAR, SocietyIdEnc);//
                objDBAccess.AddParameterToCommand("ApplicationNo", DBFactoryParameterType.VARCHAR, ApplicationNo);//
                objDBAccess.AddParameterToCommand("KioskID", DBFactoryParameterType.VARCHAR, emitra.KIOSKCODE.Length > 0 ? emitra.KIOSKCODE : "SSO");//
                objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, "");//
                objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("RequestStatus", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("ActId", DBFactoryParameterType.Int, 0);//                    
                objDBAccess.AddParameterToCommand("SSOID", DBFactoryParameterType.VARCHAR, emitra.KIOSKCODE.Length > 0 ? emitra.SSOID : emitra.SSOID);
                objDBAccess.AddParameterToCommand("CreatedIP", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("SERICEID", DBFactoryParameterType.BigInt, emitra.SERVICEID);
                objDBAccess.OpenConnection();
                object oi = objDBAccess.ExecuteScalarCommand();
                K = Convert.ToInt32(oi);
                #endregion

                DataTable Dt = null;
                Dt = GetApplicationKey(emitra.SERVICEID, true);
                if (Dt.Rows.Count > 0)
                {
                    var ONE = Dt.Rows[0]["REVENUHEADKIOSKINTEGRATION"].ToString().Split('|');
                    var one = ONE[0].Split('-');
                    var tw = ONE[0].Split('-');
                    string PAY = one[0].ToString() + "-" + Amount + "|" + ONE[1].ToString();
                    DRequest req = new DRequest();
                    req.MERCHANTCODE = Dt.Rows[0]["MERCHANTCODE"].ToString();
                    req.REQUESTID = K.ToString();
                    req.REQTIMESTAMP = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    req.SERVICEID = Dt.Rows[0]["SERICEID"].ToString();
                    req.SUBSERVICEID = "";
                    req.REVENUEHEAD = PAY;   //Dt.Rows[0]["REVENUHEADKIOSKINTEGRATION"].ToString();
                    req.CONSUMERKEY = K + "-" + ApplicationNo;
                    req.CONSUMERNAME = ConsumerName;
                    req.COMMTYPE = "3";
                    req.SSOID = emitra.SSOID;
                    req.OFFICECODE = OfficeCode;// Dt.Rows[0]["OFFICECODE"].ToString();// MandiID.ToString();
                    req.SSOTOKEN = emitra.SSOTOKEN;

                    DRequestChecksum reqChecksum = new DRequestChecksum();
                    req.CHECKSUM = reqChecksum.GetCheckSum(req);


                    #region "REQUEST UPDATE"
                    objDBAccess.CreateCommand("UpdateEmitraKioskTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("Emitraapprequestid", DBFactoryParameterType.BigInt, K);
                    objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, JsonConvert.SerializeObject(req));
                    objDBAccess.AddParameterToCommand("UpdateFlag", DBFactoryParameterType.BigInt, 1);
                    objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, "");
                    objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, "");
                    objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, "");
                    objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, "");
                    objDBAccess.AddParameterToCommand("RegistrationNo", DBFactoryParameterType.VARCHAR, ApplicationNo);
                    objDBAccess.AddParameterToCommand("ServiceId", DBFactoryParameterType.Int, emitra.SERVICEID);
                    objDBAccess.AddParameterToCommand("SocietyIdEnc", DBFactoryParameterType.VARCHAR, SocietyIdEnc);

                    objDBAccess.OpenConnection();
                    object o = objDBAccess.ExecuteScalarCommand();
                    i = Convert.ToInt32(o);

                    #endregion

                    string retVal = MakeTransactionsEncrypted(Dt.Rows[0]["T_Url"].ToString(), JsonConvert.SerializeObject(req), Dt.Rows[0]["encryptionkey"].ToString());

                    string decVal = Decrypt(retVal, Dt.Rows[0]["encryptionkey"].ToString());
                    DResponse resp = JsonConvert.DeserializeObject<DResponse>(decVal);

                    #region "Update into Emitrakiosktransaction"
                    try
                    {
                        if (resp != null)
                        {
                            //UPDATE RESPONCE
                            objDBAccess.CreateCommand("UpdateEmitraKioskTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                            objDBAccess.AddParameterToCommand("Emitraapprequestid", DBFactoryParameterType.BigInt, K);
                            objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, "");
                            objDBAccess.AddParameterToCommand("UpdateFlag", DBFactoryParameterType.BigInt, 2);
                            objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, resp.RECEIPTNO);
                            objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, resp.TRANSACTIONID);
                            objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, resp.MSG);
                            objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, JsonConvert.SerializeObject(resp));
                            objDBAccess.AddParameterToCommand("RegistrationNo", DBFactoryParameterType.VARCHAR, ApplicationNo);
                            objDBAccess.AddParameterToCommand("ServiceId", DBFactoryParameterType.Int, emitra.SERVICEID);
                            objDBAccess.AddParameterToCommand("SocietyIdEnc", DBFactoryParameterType.VARCHAR, SocietyIdEnc);
                            objDBAccess.AddParameterToCommand("IsPermanentSave", DBFactoryParameterType.Int, Convert.ToInt32(IsPermanentSave));

                            objDBAccess.OpenConnection();
                            object p = objDBAccess.ExecuteScalarCommand();
                            i = Convert.ToInt32(p);
                            returnmsg = resp.MSG;
                            ReceptNo = resp.RECEIPTNO;
                            kioskCode = emitra.KIOSKNAME + (emitra.KIOSKCODE);
                            KTimeStamp = resp.EMITRATIMESTAMP.ToString().Substring(0, 8);
                            if (i == 1)
                            {
                                AuditorModel objEM = new AuditorModel(new FileLogger());
                                objEM.SetAuditEmpanelledFinalSubmit(K.ToString());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModel", FunctionName = "FeeDepositForCAAuditor(string SocietyIdEnc, string ApplicationNo, string ConsumerName, string IsPermanentSave = '0')" }, ex);
                    }
                    finally
                    {
                        objDBAccess.CloseConnection();
                    }
                    #endregion
                }
                else
                {
                    returnmsg = "Error In Update Request";
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModel", FunctionName = "FeeDepositForCAAuditor(string SocietyIdEnc, string ApplicationNo, string ConsumerName, string IsPermanentSave = '0')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return returnmsg + "@" + ReceptNo + "@" + kioskCode + "@" + KTimeStamp + "@";
        }


        public string UpdateEmitrTransationForCAAuditor(string UniquerequestId, string ConsumerName, string damnt, string RegistrationNo, string SocietyId, string SocietyIdEnc, string ServiceID, bool IsKiosk = true, string IsPermanentSave = "0", string Amount = "0")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            if (UniquerequestId.Length == 0)
            {
                #region "insert into Emitrakiosktransaction"
                objDBAccess.CreateCommand("InsertEmitraKioskTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("SocietyIdEnc", DBFactoryParameterType.VARCHAR, SocietyIdEnc);//
                objDBAccess.AddParameterToCommand("ApplicationNo", DBFactoryParameterType.VARCHAR, RegistrationNo);//
                objDBAccess.AddParameterToCommand("KioskID", DBFactoryParameterType.VARCHAR, _LoggedInUser.KIOSKCODE.Length > 0 ? _LoggedInUser.KIOSKCODE : "SSO");//
                objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, "");//
                objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("RequestStatus", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("ActId", DBFactoryParameterType.Int, 0);//                    
                objDBAccess.AddParameterToCommand("SSOID", DBFactoryParameterType.VARCHAR, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("CreatedIP", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("SERICEID", DBFactoryParameterType.BigInt, int.Parse(ServiceID));
                objDBAccess.OpenConnection();
                object oi = objDBAccess.ExecuteScalarCommand();
                UniquerequestId = Convert.ToString(oi);
                #endregion
            }
            DataTable Dt = null;
            string Result = "";

            ConsumerName = ConsumerName.Replace("&", "AND").Replace("/", " ").Replace("-", " ").Replace("_", " ");

            if (_LoggedInUser != null)
            {
                EmitraModelFactory objEmitraModel = new EmitraModelFactory(new FileLogger());
                Dt = objEmitraModel.GetApplicationKey(ServiceID, IsKiosk);
                var tempRegistrationNo = RegistrationNo.Split('/');
                RegistrationNo = "";
                for (var k = 0; k < tempRegistrationNo.Length; k++)
                {
                    RegistrationNo += tempRegistrationNo[k];
                }
                if (Dt.Rows.Count > 0)
                {
                    string remoteUrl = Dt.Rows[0]["T_Url"].ToString();
                    string Checksumkey = Dt.Rows[0]["CHECKSUMKEY"].ToString();
                    string EncryptionKey = Dt.Rows[0]["ENCRYPTIONKEY"].ToString();

                    PGRequest data = new PGRequest();
                    // data.AMOUNT = Dt.Rows[0]["REVENUEHEADFORPG"].ToString().Split('-')[1].ToString();
                    data.AMOUNT = Amount;
                    data.COMMTYPE = Dt.Rows[0]["COMTYPE"].ToString();
                    data.MERCHANTCODE = Dt.Rows[0]["MERCHANTCODE"].ToString();
                    data.OFFICECODE = Dt.Rows[0]["OFFICECODE"].ToString();
                    data.PRN = UniquerequestId;
                    data.REQTIMESTAMP = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    data.REVENUEHEAD = Dt.Rows[0]["REVENUEHEADFORPG"].ToString().Split('-')[0].ToString() + "-" + Amount;
                    data.SERVICEID = Dt.Rows[0]["SERICEID"].ToString();
                    data.SUCCESSURL = Dt.Rows[0]["REDIRECTURL"].ToString() + "/?UniquerequestId=" + COMMON_FUNCTION.Encrypt(UniquerequestId) + "&SocietyIdEnc=" + SocietyIdEnc + "&ServiceID=" + ServiceID + "&IsFailed=" + COMMON_FUNCTION.Encrypt("NO") + "&IsPermanentSave=" + COMMON_FUNCTION.Encrypt(IsPermanentSave);
                    data.FAILUREURL = Dt.Rows[0]["REDIRECTURL"].ToString() + "/?UniquerequestId=" + COMMON_FUNCTION.Encrypt(UniquerequestId) + "&SocietyIdEnc=" + SocietyIdEnc + "&ServiceID=" + ServiceID + "&IsFailed=" + COMMON_FUNCTION.Encrypt("YES") + "&IsPermanentSave=" + COMMON_FUNCTION.Encrypt(IsPermanentSave);
                    //data.SUCCESSURL = "http://localhost:27466/Citizen/UpdateOnlinePaymentStatu_Post_ssuc";
                    //data.FAILUREURL = "http://localhost:27466/Citizen/UpdateOnlinePaymentStatu_Post_ssuc";
                    data.UDF1 = RegistrationNo.Replace("-", "");
                    data.UDF2 = _LoggedInUser.SSOID.ToUpper();
                    data.CONSUMERKEY = SocietyId + (!string.IsNullOrWhiteSpace(RegistrationNo.Replace("-", "")) ? "-" + RegistrationNo.Replace("-", "") : "");
                    data.CONSUMERNAME = ConsumerName.Replace("-", "");
                    data.USEREMAIL = "";
                    data.USERMOBILE = "";

                    data.USERNAME = ConsumerName.Replace("-", "");


                    data.CHECKSUM = CreateMD5(data.PRN + "|" + data.AMOUNT + "|" + Checksumkey);
                    int M = -1;
                    try
                    {
                        if (data != null)
                        {
                            objDBAccess.CreateCommand("UpdateEmitraKioskTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                            objDBAccess.AddParameterToCommand("Emitraapprequestid", DBFactoryParameterType.BigInt, int.Parse(UniquerequestId));
                            objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, JsonConvert.SerializeObject(data));
                            objDBAccess.AddParameterToCommand("updateflag", DBFactoryParameterType.BigInt, 1);
                            objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, "");
                            objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, "");
                            objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, "");
                            objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, "");
                            objDBAccess.AddParameterToCommand("RegistrationNo", DBFactoryParameterType.VARCHAR, RegistrationNo);
                            objDBAccess.AddParameterToCommand("ServiceId", DBFactoryParameterType.Int, ServiceID);
                            objDBAccess.AddParameterToCommand("SocietyIdEnc", DBFactoryParameterType.VARCHAR, SocietyIdEnc);
                            //objDBAccess.AddParameterToCommand("PRN", DBFactoryParameterType.VARCHAR, data.PRN);

                            objDBAccess.OpenConnection();
                            object o = objDBAccess.ExecuteScalarCommand();
                            M = Convert.ToInt32(o);
                        }
                    }
                    catch (Exception ex)
                    {
                        //ILogger objILogger = new DBLogger();
                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModelFactory", FunctionName = "UpdateEmitrTransation(string UniquerequestId, string ConsumerName, string damnt, string RegistrationNo, string SocietyIdEnc, string ServiceID, bool IsKiosk = true)" }, ex);
                    }
                    finally
                    {
                        objDBAccess.CloseConnection();
                    }
                    StringBuilder sb = new StringBuilder();
                    EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES(EncryptionKey);
                    string encData = objEncryptDecrypt.encrypt(JsonConvert.SerializeObject(data));
                    string encDataSimple = JsonConvert.SerializeObject(data);
                    sb.Append("<form id=\"PostForm\" name=\"PostForm\" action=\"" + remoteUrl + "\" method=\"POST\">");
                    sb.AppendFormat("<input type='hidden' name='MERCHANTCODE' value='" + data.MERCHANTCODE + "'>");
                    sb.AppendFormat("<input type='hidden' name='SERVICEID' value='" + ServiceID + "'>");
                    sb.AppendFormat("<input type='hidden' name='ENCDATA' value='" + encData + "'>");
                    sb.Append("</form>");
                    // ILogger objILogger = new DBLogger();
                    //objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "UpdateEmitrTransation(string UniquerequestId, string ConsumerName, string damnt, string RegistrationNo, string SocietyIdEnc, string ServiceID, bool IsKiosk = true)" }, new Exception("IN form"));
                    return sb.ToString();

                }
            }
            return Result;
        }
        public string UpdateEmitrTransationForPLDBMembership(string UniquerequestId, string ConsumerName, string damnt, string RegistrationNo, string SocietyId, string SocietyIdEnc, string ServiceID,string OfficeCode, bool IsKiosk = true, string IsPermanentSave = "0", string Amount = "0")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            if (UniquerequestId.Length == 0)
            {
                #region "insert into Emitrakiosktransaction"
                objDBAccess.CreateCommand("InsertEmitraKioskTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("SocietyIdEnc", DBFactoryParameterType.VARCHAR, SocietyIdEnc);//
                objDBAccess.AddParameterToCommand("ApplicationNo", DBFactoryParameterType.VARCHAR, RegistrationNo);//
                objDBAccess.AddParameterToCommand("KioskID", DBFactoryParameterType.VARCHAR, _LoggedInUser.KIOSKCODE.Length > 0 ? _LoggedInUser.KIOSKCODE : "SSO");//
                objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, "");//
                objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("RequestStatus", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, "");
                objDBAccess.AddParameterToCommand("ActId", DBFactoryParameterType.Int, 0);//                    
                objDBAccess.AddParameterToCommand("SSOID", DBFactoryParameterType.VARCHAR, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("CreatedIP", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("SERICEID", DBFactoryParameterType.BigInt, int.Parse(ServiceID));
                objDBAccess.OpenConnection();
                object oi = objDBAccess.ExecuteScalarCommand();
                UniquerequestId = Convert.ToString(oi);
                #endregion
            }
            DataTable Dt = null;
            string Result = "";

            if (_LoggedInUser != null)
            {
                EmitraModelFactory objEmitraModel = new EmitraModelFactory(new FileLogger());
                Dt = objEmitraModel.GetApplicationKey(ServiceID, IsKiosk);
                var tempRegistrationNo = RegistrationNo.Split('/');
                RegistrationNo = "";
                for (var k = 0; k < tempRegistrationNo.Length; k++)
                {
                    RegistrationNo += tempRegistrationNo[k];
                }
                if (Dt.Rows.Count > 0)
                {
                    string remoteUrl = Dt.Rows[0]["T_Url"].ToString();
                    string Checksumkey = Dt.Rows[0]["CHECKSUMKEY"].ToString();
                    string EncryptionKey = Dt.Rows[0]["ENCRYPTIONKEY"].ToString();

                    PGRequest data = new PGRequest();
                    // data.AMOUNT = Dt.Rows[0]["REVENUEHEADFORPG"].ToString().Split('-')[1].ToString();
                    data.AMOUNT = Amount;
                    data.COMMTYPE = Dt.Rows[0]["COMTYPE"].ToString();
                    data.MERCHANTCODE = Dt.Rows[0]["MERCHANTCODE"].ToString();
                    data.OFFICECODE = OfficeCode;// Dt.Rows[0]["OFFICECODE"].ToString();
                    data.PRN = UniquerequestId;
                    data.REQTIMESTAMP = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    data.REVENUEHEAD = Dt.Rows[0]["REVENUEHEADFORPG"].ToString().Split('-')[0].ToString() + "-" + Amount;
                    data.SERVICEID = Dt.Rows[0]["SERICEID"].ToString();
                    data.SUCCESSURL = Dt.Rows[0]["REDIRECTURL"].ToString() + "/?UniquerequestId=" + COMMON_FUNCTION.Encrypt(UniquerequestId) + "&SocietyIdEnc=" + SocietyIdEnc + "&ServiceID=" + ServiceID + "&IsFailed=" + COMMON_FUNCTION.Encrypt("NO") + "&IsPermanentSave=" + COMMON_FUNCTION.Encrypt(IsPermanentSave);
                    data.FAILUREURL = Dt.Rows[0]["REDIRECTURL"].ToString() + "/?UniquerequestId=" + COMMON_FUNCTION.Encrypt(UniquerequestId) + "&SocietyIdEnc=" + SocietyIdEnc + "&ServiceID=" + ServiceID + "&IsFailed=" + COMMON_FUNCTION.Encrypt("YES") + "&IsPermanentSave=" + COMMON_FUNCTION.Encrypt(IsPermanentSave);
                    //data.SUCCESSURL = "http://localhost:27466/Citizen/UpdateOnlinePaymentStatu_Post_ssuc";
                    //data.FAILUREURL = "http://localhost:27466/Citizen/UpdateOnlinePaymentStatu_Post_ssuc";
                    data.UDF1 = RegistrationNo.Replace("-", "");
                    data.UDF2 = _LoggedInUser.SSOID.ToUpper();
                    data.CONSUMERKEY = SocietyId + (!string.IsNullOrWhiteSpace(RegistrationNo.Replace("-", "")) ? "-" + RegistrationNo.Replace("-", "") :"");                  
                    data.CONSUMERNAME = ConsumerName.Replace("-", "");
                    data.USEREMAIL = "";
                    data.USERMOBILE = "";

                    data.USERNAME = ConsumerName.Replace("-", "");


                    data.CHECKSUM = CreateMD5(data.PRN + "|" + data.AMOUNT + "|" + Checksumkey);
                    int M = -1;
                    try
                    {
                        if (data != null)
                        {
                            objDBAccess.CreateCommand("UpdateEmitraKioskTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                            objDBAccess.AddParameterToCommand("Emitraapprequestid", DBFactoryParameterType.BigInt, int.Parse(UniquerequestId));
                            objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, JsonConvert.SerializeObject(data));
                            objDBAccess.AddParameterToCommand("updateflag", DBFactoryParameterType.BigInt, 1);
                            objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, "");
                            objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, "");
                            objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, "");
                            objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, "");
                            objDBAccess.AddParameterToCommand("RegistrationNo", DBFactoryParameterType.VARCHAR, RegistrationNo);
                            objDBAccess.AddParameterToCommand("ServiceId", DBFactoryParameterType.Int, ServiceID);
                            objDBAccess.AddParameterToCommand("SocietyIdEnc", DBFactoryParameterType.VARCHAR, SocietyIdEnc);
                            //objDBAccess.AddParameterToCommand("PRN", DBFactoryParameterType.VARCHAR, data.PRN);

                            objDBAccess.OpenConnection();
                            object o = objDBAccess.ExecuteScalarCommand();
                            M = Convert.ToInt32(o);
                        }
                    }
                    catch (Exception ex)
                    {
                        //ILogger objILogger = new DBLogger();
                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModelFactory", FunctionName = "UpdateEmitrTransation(string UniquerequestId, string ConsumerName, string damnt, string RegistrationNo, string SocietyIdEnc, string ServiceID, bool IsKiosk = true)" }, ex);
                    }
                    finally
                    {
                        objDBAccess.CloseConnection();
                    }
                    StringBuilder sb = new StringBuilder();
                    EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES(EncryptionKey);
                    string encData = objEncryptDecrypt.encrypt(JsonConvert.SerializeObject(data));
                    string encDataSimple = JsonConvert.SerializeObject(data);
                    sb.Append("<form id=\"PostForm\" name=\"PostForm\" action=\"" + remoteUrl + "\" method=\"POST\">");
                    sb.AppendFormat("<input type='hidden' name='MERCHANTCODE' value='" + data.MERCHANTCODE + "'>");
                    sb.AppendFormat("<input type='hidden' name='SERVICEID' value='" + ServiceID + "'>");
                    sb.AppendFormat("<input type='hidden' name='ENCDATA' value='" + encData + "'>");
                    sb.Append("</form>");
                    // ILogger objILogger = new DBLogger();
                    //objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "UpdateEmitrTransation(string UniquerequestId, string ConsumerName, string damnt, string RegistrationNo, string SocietyIdEnc, string ServiceID, bool IsKiosk = true)" }, new Exception("IN form"));
                    return sb.ToString();

                }
            }
            return Result;
        }

        public string SavePaymentResponce(string PaymentResponce)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SavePaymentResponce", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PaymentResponce", DBFactoryParameterType.NVarchar, PaymentResponce);
                objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModel", FunctionName = "public DataTable SavePaymentResponce(string ReceiptNo)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return "1";
        }

    }

    [Serializable()]
    public class DRequestChecksum
    {
        public string SSOID { get; set; }
        public string REQUESTID { get; set; }
        public string REQTIMESTAMP { get; set; }
        public string SSOTOKEN { get; set; }
        public string GetCheckSum(DRequest req)
        {
            this.SSOID = req.SSOID;
            this.REQUESTID = req.REQUESTID;
            this.REQTIMESTAMP = req.REQTIMESTAMP;
            this.SSOTOKEN = req.SSOTOKEN;

            string retVal = new EmitraModelFactory(new FileLogger()).CreateMD5(JsonConvert.SerializeObject(this));
            return retVal;
        }
    }
    [Serializable()]
    public class ARequestChecksum
    {
        public string SSOID { get; set; }
        public string SERVICEID { get; set; }
        public string EMITRATIMESTAMP { get; set; }
        public string SSOTOKEN { get; set; }
        public string GetCheckSum(ARequest req)
        {
            this.SSOID = req.SSOID;
            this.SERVICEID = req.SERVICEID;
            this.EMITRATIMESTAMP = req.EMITRATIMESTAMP;
            this.SSOTOKEN = req.SSOTOKEN;
            string retVal = new EmitraModelFactory(new FileLogger()).CreateMD5(JsonConvert.SerializeObject(this));
            return retVal;
        }
    }
    [Serializable()]
    public class ARequest
    {
        public string SSOID { get; set; }
        public string SERVICEID { get; set; }
        public string EMSESSIONID { get; set; }
        public string KIOSKCODE { get; set; }
        public string KIOSKNAME { get; set; }
        public string ENTITYTYPE { get; set; }
        public string DISTRICT { get; set; }
        public string DISTRICTCD { get; set; }
        public string TEHSIL { get; set; }
        public string TEHSILCD { get; set; }
        public string VILLAGE { get; set; }
        public string VILLAGECD { get; set; }
        public string WARD { get; set; }
        public string WARDCD { get; set; }
        public string PINCODE { get; set; }

        public string MOBILE { get; set; }
        public string EMAIL { get; set; }
        public string LSPNAME { get; set; }
        public string PARAMETER1 { get; set; }
        public string PARAMETER2 { get; set; }
        public string PARAMETER3 { get; set; }
        public string PARAMETER4 { get; set; }
        public string PARAMETER5 { get; set; }
        public string RETURNURL { get; set; }
        public string EMITRATIMESTAMP { get; set; }
        public string SSOTOKEN { get; set; }
        public string CHECKSUM { get; set; }

    }
    [Serializable()]
    public class DRequest
    {
        public string MERCHANTCODE { get; set; }
        public string REQUESTID { get; set; }
        public string REQTIMESTAMP { get; set; }
        public string SERVICEID { get; set; }
        public string SUBSERVICEID { get; set; }
        public string REVENUEHEAD { get; set; }
        public string CONSUMERKEY { get; set; }
        public string CONSUMERNAME { get; set; }
        public string COMMTYPE { get; set; }
        public string SSOID { get; set; }
        public string OFFICECODE { get; set; }
        public string SSOTOKEN { get; set; }
        public string CHECKSUM { get; set; }
    }

    [Serializable()]
    public class DResponse
    {
        public string REQUESTID { get; set; }
        public string TRANSACTIONSTATUSCODE { get; set; }
        public string RECEIPTNO { get; set; }
        public string TRANSACTIONID { get; set; }
        public string TRANSAMT { get; set; }
        public string REMAININGWALLET { get; set; }
        public string EMITRATIMESTAMP { get; set; }
        public string TRANSACTIONSTATUS { get; set; }
        public string MSG { get; set; }
        //public string SSOTOKEN { get; set; }
        //public string CHECKSUM { get; set; }

    }
    [Serializable()]
    public class DResponseChecksum
    {
        public string SSOID { get; set; }
        public string TRANSACTIONID { get; set; }
        public string EMITRATIMESTAMP { get; set; }
        public string SSOTOKEN { get; set; }
    }
    [Serializable()]
    public class SSOAuth
    {
        public string sAMAccountName { get; set; }
        //public string Roles { get; set; }
        public List<string> Roles { get; set; }

    }
    [Serializable()]
    public class VerifyRequest
    {
        public string MERCHANTCODE { get; set; }
        public string SERVICEID { get; set; }
        public string REQUESTID { get; set; }
        public string SSOTOKEN { get; set; }
        public string CHECKSUM { get; set; }

    }
    [Serializable()]
    public class VerifyTransChecksum
    {

        public string MERCHANTCODE { get; set; }
        public string REQUESTID { get; set; }
        public string SSOTOKEN { get; set; }

        public string GetCheckSum(VerifyRequest req)
        {
            this.MERCHANTCODE = req.MERCHANTCODE;
            this.REQUESTID = req.REQUESTID;
            this.SSOTOKEN = req.SSOTOKEN;
            string retVal = new EmitraModelFactory(new FileLogger()).CreateMD5(JsonConvert.SerializeObject(this));
            return retVal;
        }
    }
    [Serializable()]
    public class VerifyResponse
    {
        public string TRANSACTIONDATE { get; set; }
        public string RECEIPTNO { get; set; }
        public string TRANSACTIONID { get; set; }
        public string AMT { get; set; }
        public string EMITRATIMESTAMP { get; set; }
        public string TRANSACTIONSTATUS { get; set; }
        public string VERIFYSTATUSCODE { get; set; }
        public string MSG { get; set; }
        public string SSOTOKEN { get; set; }
        public string CHECKSUM { get; set; }
    }
    [Serializable()]
    public class PGRequest
    {
        public string MERCHANTCODE { get; set; }
        public string PRN { get; set; }
        public string REQTIMESTAMP { get; set; }
        public string AMOUNT { get; set; }
        public string SUCCESSURL { get; set; }
        public string FAILUREURL { get; set; }
        public string USERNAME { get; set; }
        public string USERMOBILE { get; set; }
        public string USEREMAIL { get; set; }
        public string UDF1 { get; set; }
        public string UDF2 { get; set; }
        public string SERVICEID { get; set; }
        public string OFFICECODE { get; set; }
        public string REVENUEHEAD { get; set; }
        public string COMMTYPE { get; set; }
        public string CHECKSUM { get; set; }
        public string CONSUMERKEY { get; set; }
        public string CONSUMERNAME { get; set; }
    }
    [Serializable()]
    public class PGResponse
    {
        public string MERCHANTCODE { get; set; }
        public string REQTIMESTAMP { get; set; }
        public string PRN { get; set; }
        public string AMOUNT { get; set; }
        public string AMT { get; set; }
        public string PAIDAMOUNT { get; set; }
        public string SERVICEID { get; set; }
        public string TRANSACTIONID { get; set; }
        public string RECEIPTNO { get; set; }
        public string EMITRATIMESTAMP { get; set; }
        public string STATUS { get; set; }
        public string REQUESTID { get; set; }
        public string TRANSACTIONSTATUS { get; set; }
        public string PAYMENTMODE { get; set; }
        public string PAYMENTMODEBID { get; set; }
        public string PAYMENTMODETIMESTAMP { get; set; }
        public string RESPONSECODE { get; set; }
        public string RESPONSEMESSAGE { get; set; }
        public string MSG { get; set; }
        public string UDF1 { get; set; }
        public string UDF2 { get; set; }
        public string CHECKSUM { get; set; }
        public string UniqueTransactionID { get; set; }
        public string BANK_NAME { get; set; }
        public string BANK_ACC_NO { get; set; }
        public string BANK_BID_NO { get; set; }
        public string REFERENCE_ID { get; set; }
        public int INTERNAL_ID { get; set; }
        public string TRANSACTIONDATE { get; set; }
        public int VERIFYSTATUSCODE { get; set; }
   


        public void save()
        {
            //string ApplicationNo = (this.UDF1.Split('#'))[0].ToString();
            //string CollegeId = (this.UDF1.Split('#'))[1].ToString();
            //string CourseId = (this.UDF1.Split('#'))[2].ToString();
            //general gen = new general();
            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandText = "usp_Ins_ApplicationPaymentGatewayStatus";
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Connection = gen.con;
            //gen.con.Open();
            //cmd.Parameters.AddWithValue("@AppNo", ApplicationNo);
            //cmd.Parameters.AddWithValue("@CId", CollegeId);
            //cmd.Parameters.AddWithValue("@CSId", CourseId);
            //cmd.Parameters.AddWithValue("@SSOID", this.UDF2);
            //cmd.Parameters.AddWithValue("@merchantcode", this.MERCHANTCODE);
            //cmd.Parameters.AddWithValue("@reqtimestamp", this.REQTIMESTAMP);
            //cmd.Parameters.AddWithValue("@prn", this.PRN);
            //cmd.Parameters.AddWithValue("@amount", this.AMOUNT);
            //cmd.Parameters.AddWithValue("@paidamount", this.PAIDAMOUNT);
            //cmd.Parameters.AddWithValue("@serviceid", this.SERVICEID);
            //cmd.Parameters.AddWithValue("@transactionid", this.TRANSACTIONID);
            //cmd.Parameters.AddWithValue("@receiptno", this.RECEIPTNO);
            //cmd.Parameters.AddWithValue("@emitratimestamp", this.EMITRATIMESTAMP);
            //cmd.Parameters.AddWithValue("@status", this.STATUS);
            //cmd.Parameters.AddWithValue("@paymentmode", this.PAYMENTMODE);
            //cmd.Parameters.AddWithValue("@paymentmodebid", this.PAYMENTMODEBID);
            //cmd.Parameters.AddWithValue("@paymentmodetimestamp", this.PAYMENTMODETIMESTAMP);
            //cmd.Parameters.AddWithValue("@responsecode", this.RESPONSECODE);
            //cmd.Parameters.AddWithValue("@responsemessage", this.RESPONSEMESSAGE);
            //cmd.Parameters.AddWithValue("@checksum", this.CHECKSUM);
            //cmd.ExecuteNonQuery();
        }

        public void feeSave()
        {
            //string ApplicationNo = (this.UDF1.Split('#'))[0].ToString();
            //string CollegeId = (this.UDF1.Split('#'))[1].ToString();
            //string CourseId = (this.UDF1.Split('#'))[2].ToString();
            //general gen = new general();
            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandText = "USP_Ins_FeesPaymentGatewayStatus";
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Connection = gen.con;
            //gen.con.Open();
            //cmd.Parameters.AddWithValue("@UniqueTransactionID", UniqueTransactionID);
            //cmd.Parameters.AddWithValue("@AppNo", ApplicationNo);
            //cmd.Parameters.AddWithValue("@CId", CollegeId);
            //cmd.Parameters.AddWithValue("@CSId", CourseId);
            //cmd.Parameters.AddWithValue("@SSOID", this.UDF2);
            //cmd.Parameters.AddWithValue("@merchantcode", this.MERCHANTCODE);
            //cmd.Parameters.AddWithValue("@reqtimestamp", this.REQTIMESTAMP);
            //cmd.Parameters.AddWithValue("@prn", this.PRN);
            //cmd.Parameters.AddWithValue("@amount", this.AMOUNT);
            //cmd.Parameters.AddWithValue("@paidamount", this.PAIDAMOUNT);
            //cmd.Parameters.AddWithValue("@serviceid", this.SERVICEID);
            //cmd.Parameters.AddWithValue("@transactionid", this.TRANSACTIONID);
            //cmd.Parameters.AddWithValue("@receiptno", this.RECEIPTNO);
            //cmd.Parameters.AddWithValue("@emitratimestamp", this.EMITRATIMESTAMP);
            //cmd.Parameters.AddWithValue("@status", this.STATUS);
            //cmd.Parameters.AddWithValue("@paymentmode", this.PAYMENTMODE);
            //cmd.Parameters.AddWithValue("@paymentmodebid", this.PAYMENTMODEBID);
            //cmd.Parameters.AddWithValue("@paymentmodetimestamp", this.PAYMENTMODETIMESTAMP);
            //cmd.Parameters.AddWithValue("@responsecode", this.RESPONSECODE);
            //cmd.Parameters.AddWithValue("@responsemessage", this.RESPONSEMESSAGE);
            //cmd.Parameters.AddWithValue("@checksum", this.CHECKSUM);
            //cmd.ExecuteNonQuery();
        }


    }
}