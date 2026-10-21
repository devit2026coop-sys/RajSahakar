using Cooperative.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;

namespace Cooperative.Controllers
{
    public static class AADHARSerice
    {
        /// <summary>
        /// Send OTP on AADHAR Registered Mobile No.
        /// </summary>
        /// <param name="_AadharNo">AADHAR No. (in 12 digits only)</param>
        /// <returns>Success:True/False with Message and Transaction No.</returns>
        /// //Aadhaar PROD BIO eKYC Authentication WAS 1.0.0-rev1
        public static string SendOTPOnAADHAR(string _AadharNo)
        {           
            _AadharNo = _AadharNo.ToString().Replace(" ","");
            string Msg = string.Empty;
            string regExpForAADHAR = @"^[0-9]{12}$";
            bool isValidAADHAAR = Regex.IsMatch(_AadharNo, regExpForAADHAR);

            if (!isValidAADHAAR)
            {
                var t = new { Result = false, Message = "AADHAR No. must be of 12 digits.", AadharNo = _AadharNo, txnid = string.Empty };
                return new JavaScriptSerializer().Serialize(t);
            }
            else
            {
                var temp = COMMON_FUNCTION.SendOtpByAadharNo(_AadharNo);
                var t = new { Result = !temp.Contains("DOITRJGOVAUTH") ? false : true, Message = !temp.Contains("DOITRJGOVAUTH") ? temp : "Success", AadharNo = COMMON_FUNCTION.Encrypt(_AadharNo), txnid = !temp.Contains("DOITRJGOVAUTH") ? string.Empty : temp };
                return new JavaScriptSerializer().Serialize(t);
            }
        }

        /// <summary>
        /// Verify AADHAR No on the basis of sent SMS over registered Mobile No.
        /// </summary>
        /// <param name="otp">OTP Received on Registered Mobuile no.</param>
        /// <param name="txnNo">System Genertaed txn No.</param>
        /// <param name="_EncryptedAadharNo">Encrypted AADHAR No. (Cryptographic Encryption)</param>
        /// <returns>Success:True/False with AADHAR Captured data of owner</returns>
        public static string VeryifyOTPsentOnAADHAR(string otp, string txnNo, string _EncryptedAadharNo)
        {
            string Msg = string.Empty;

            string regExpForOTP = @"^[0-9]{6}$";
            bool isValidOTP = Regex.IsMatch(otp, regExpForOTP);

            if (!isValidOTP)
                Msg += "OTP Must be of 6 digits. ";

            if (!txnNo.Contains("DOITRJGOVAUTH"))
                Msg += "Provided txn No. is not correct.";

            if (!string.IsNullOrEmpty(Msg))
            {
                var t = new { Result = false, Message = Msg, AadharNo = _EncryptedAadharNo, txnid = txnNo, AADHARDetails = string.Empty };
                return new JavaScriptSerializer().Serialize(t);
            }

            DataTable dt = COMMON_FUNCTION.ValidateOTP(txnNo, COMMON_FUNCTION.Decrypt(_EncryptedAadharNo), otp);
            if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["Column1"].ToString() == "Success")
            {
                List<Dictionary<string, object>> allRows = new List<Dictionary<string, object>>();
                Dictionary<string, object> singleRow;

                foreach (DataRow row in dt.Rows)
                {
                    singleRow = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                        singleRow.Add(col.ColumnName, row[col]);

                    allRows.Add(singleRow);
                }

                var t = new { Result = true, Message = "Success", AadharNo = _EncryptedAadharNo, txnid = string.Empty, AADHARDetails = allRows };
                return new JavaScriptSerializer().Serialize(t);
            }
            else
            {
                var t = new { Result = false, Message = "Not Valid Data", AadharNo = _EncryptedAadharNo, txnid = string.Empty, AADHARDetails = string.Empty };
                return new JavaScriptSerializer().Serialize(t);
            }

        }


        /// <summary>
        /// GetBioCode
        /// </summary>
        /// <returns></returns>
        public static KYCResponce BIO_KYC(string AadharNo)
        {
            string UID = string.Empty;
            var XMLDATA1 = "<PidOptions ver=\"1.0\"><Opts fCount=\"1\" fType=\"0\" format=\"0\" pidVer=\"2.0\" timeout=\"20000\" posh=\"\" env=\"P\" wadh=\"RZ+k4w9ySTzOibQdDHPzCFqrKScZ74b3EibKYy1WyGw=\" /></PidOptions>";
            var XMLDATA = "<PidOptions ver=\"1.0\"><Opts fCount=\"1\" fType=\"0\" iCount=\"\" iType=\"\" pCount=\"\" pType=\"\" format=\"0\" pidVer=\"2.0\" timeout=\"10000\" otp=\"\" wadh=\"RZ+k4w9ySTzOibQdDHPzCFqrKScZ74b3EibKYy1WyGw=\" posh=\"\"/></PidOptions>";
            KYCResponce result = new KYCResponce();
            string url = "http://127.0.0.1:11100/capture";

            System.Net.WebRequest req = null;
            WebResponse rsp = null;
            try
            {

                req = WebRequest.Create(url);
                //req.Proxy = WebProxy.GetDefaultProxy(); // Enable if using proxy
                req.Method = "CAPTURE";        // Post method
                req.ContentType = "text/xml";

                //req.Headers["appname"] = "CO-Operative";
                // content type
                // Wrap the request stream with a text-based writer
                StreamWriter writer = new StreamWriter(req.GetRequestStream());
                // Write the XML text into the stream
                writer.Write(XMLDATA);
                writer.Close();
                // Send the data to the webserver
                rsp = req.GetResponse(); //I am getting error over here
                StreamReader sr = new StreamReader(rsp.GetResponseStream());
                string results = sr.ReadToEnd();
                sr.Close();
                // UpdateIdentifyMessage(results, null);
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(results); // suppose that myXmlString contains "<Names>...</Names>"  
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "GenericCode", FunctionName = "BIO_KYC)" }, new Exception(results));
                result = AADHARSerice.VerifyBIO_KYCData(xml, AadharNo, 0);// "428434778643");


            }
            catch (Exception ex)
            {
                result.Status = "0";
                //_txnid = "DOITRJGOVAUTH" + "";

            }
            return result;
        }


        /// <summary>
        /// Verify BIO KYC.
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="Aadhar"></param>
        /// <returns></returns>
        public static KYCResponce VerifyBIO_KYCData(XmlDocument xml, string Aadhar, int logId)
        {
            DBLogger objDBLogger = new DBLogger();
            KYCResponce result = new KYCResponce();
            try
            {


                string biodata = xml.GetElementsByTagName("Data")[0].InnerXml;
                string auacode = GlobalConstant.AadharAuthSUBAUS;// "PNSCL22866";
                string licenseKey = GlobalConstant.AadharAuthLicenseKey;
                XmlNode nodeDeviceInfo = xml.SelectSingleNode("PidData/DeviceInfo");
                XmlNode rdsInfo = xml.SelectSingleNode("PidData/Resp");
                XmlNode nodeSkey = xml.SelectSingleNode("PidData/Skey");
                XmlNode nodeHmac = xml.SelectSingleNode("PidData/Hmac");
                XmlNode nodeData = xml.SelectSingleNode("PidData/Data");

                string fType = "FMR,FIR";// = rdsInfo.Attributes["fType"].Value;
                string errCode = rdsInfo.Attributes["errCode"].Value;
                string errInfo = rdsInfo.Attributes["errInfo"].Value;
                string fCount = rdsInfo.Attributes["fCount"].Value;
                string nmPoints = rdsInfo.Attributes["nmPoints"].Value;
                string qScore = rdsInfo.Attributes["qScore"].OuterXml.Split('"')[1];//rdsInfo.Attributes["qScore"].Value.Replace("\n","");// 
                //string qScore = rdsInfo.Attributes["Data"].Value;
                var tts = DateTime.UtcNow.ToString();
                string ip = COMMON_FUNCTION.GetIpAddress();
                string xmlString = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><authrequest uid=\"" + Aadhar + "\" tid=\"registered\" subaua=\"" + auacode + "\" bt=\"FMR,FIR\" lk=\"" + licenseKey + "\" rc=\"Y\" ra=\"F\" lr=\"Y\" de=\"N\" pfr=\"N\" mec=\"Y\" ver=\"2.5\"  dpID=\"" + nodeDeviceInfo.Attributes["dpId"].Value + "\" rdsID=\"" + nodeDeviceInfo.Attributes["rdsId"].Value + "\" rdsVer=\"" + nodeDeviceInfo.Attributes["rdsVer"].Value + "\" dc=\"" + nodeDeviceInfo.Attributes["dc"].Value + "\" mi=\"" + nodeDeviceInfo.Attributes["mi"].Value + "\" mc=\"" + nodeDeviceInfo.Attributes["mc"].Value + "\" deviceSrNO=\"NA\" deviceError=\"NA\"  ts='" + tts + "' ip=\"" + ip + "\" fdc=\"NC\" idc=\"NC\" macadd=\"78-2B-CB-8D-93-F1\" lot=\"P\" lov=\"110002\" ><deviceInfo fType=\"" + fType + "\" iCount=\"1\" pCount=\"1\" errCodeRDS=\"" + errCode + "\" errInfoRDS=\"" + errInfo + "\" fCount=\"1\" nmPoints=\"" + nmPoints + "\" qScore=\"" + qScore + "\" srno=\"NA\" deviceError=\"NA\" /><Skey ci=\"" + nodeSkey.Attributes["ci"].Value + "\">" + nodeSkey.InnerText + "</Skey><Hmac>" + nodeHmac.InnerText + "</Hmac><Data type=\"X\">" + biodata + "</Data></authrequest>";


                new CitizenModel(new DBLogger()).InserBioKYCLog("", "", xmlString, "", logId);


                System.Net.WebRequest req = null;
                WebResponse rsp = null;
                try
                {
                    string url = "https://api.sewadwaar.rajasthan.gov.in/app/live/was/kyc/bio/prod?client_id=02a79073-c2db-4c84-b810-885fdaa8c54a";
                    req = WebRequest.Create(url);
                    req.Method = "POST";
                    req.Headers["appname"] = "CO-Operative";
                    req.ContentType = "application/xml";
                    //req.Headers["appname"] = "CO-Operative";
                    StreamWriter writer = new StreamWriter(req.GetRequestStream());
                    writer.Write(xmlString);
                    writer.Close();
                    rsp = req.GetResponse();
                    StreamReader sr = new StreamReader(rsp.GetResponseStream());
                    string results = sr.ReadToEnd();







                    sr.Close();
                    XmlDocument xml11 = new XmlDocument();
                    xml11.LoadXml(results);

                    string errorStstusCode = "";
                    if (xml11.SelectNodes("//kycAuthResError") != null)
                    {
                        XmlNode kycAuthResError = xml11.SelectNodes("//kycAuthResError")[0];
                        if (kycAuthResError != null)
                        {
                            errorStstusCode = kycAuthResError.InnerText;
                        }
                    }
                    new CitizenModel(new DBLogger()).InserBioKYCLog(xml.ToString(), results, xmlString, errorStstusCode, logId);

                    string ret = string.Empty;
                    foreach (XmlNode node in xml11.SelectNodes("//auth[@status]"))
                    {
                        ret = node.Attributes["status"].Value;
                    }

                    //xml11.SelectNodes("kycAuthResError").Item.



                    if (ret.ToUpper() == "Y")
                    {
                        result.Status = "1";
                        XmlNode node2 = xml11.SelectSingleNode("authresponse/UidData/pht");
                        string base64BinaryStr = node2.InnerText;
                        byte[] byteArray = Convert.FromBase64String(base64BinaryStr);
                        string base64String = Convert.ToBase64String(byteArray, 0, byteArray.Length);


                        XmlNode node3 = xml11.SelectSingleNode("authresponse/UidData/pa");
                        XmlNode node4 = xml11.SelectSingleNode("authresponse/UidData/pi");

                        string house = node3.Attributes["house"].Value;
                        string street = node3.Attributes["street"].Value;
                        string lm = node3.Attributes["lm"].Value;
                        string loc = node3.Attributes["loc"].Value;
                        string vtc = node3.Attributes["vtc"].Value;
                        string dist = node3.Attributes["dist"].Value;
                        string state = node3.Attributes["state"].Value;
                        string pc = node3.Attributes["pc"].Value;

                        result.UserDetails = new UserDetails();
                        result.UserDetails.AadharNo = Aadhar;
                        result.UserDetails.Photo = "data:image/png;base64," + base64String; ;
                        result.UserDetails.Name = node4.Attributes["name"].Value;
                        result.UserDetails.FName = node3.Attributes["co"].Value;
                        result.UserDetails.DOB = node4.Attributes["dob"].Value;
                        result.UserDetails.Gender = node4.Attributes["gender"].Value;
                        result.UserDetails.Email = node4.Attributes["email"].Value;
                        result.UserDetails.Address = house + ", " + street + ", " + lm + ", " + loc + ", " + vtc + ", " + dist + ", " + state + ", " + pc;
                        result.UserDetails.Dist = node3.Attributes["dist"].Value;
                        result.UserDetails.House = node3.Attributes["house"].Value;
                        result.UserDetails.lm = node3.Attributes["lm"].Value;
                        result.UserDetails.loc = node3.Attributes["loc"].Value;
                        result.UserDetails.pc = node3.Attributes["pc"].Value;
                        result.UserDetails.Phone = node4.Attributes["phone"].Value;
                        result.UserDetails.State = node3.Attributes["state"].Value;
                        result.UserDetails.Street = node3.Attributes["street"].Value;
                        result.UserDetails.vtc = node3.Attributes["vtc"].Value;

                        string dob = result.UserDetails.DOB.Replace("-", "/");
                        dob = dob.Split('/')[2] + "/" + dob.Split('/')[1] + "/" + dob.Split('/')[0];
                        result.UserDetails.Age = (Convert.ToDateTime(dob) - Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"))).Days / 365;
                        result.UserDetails.Age = result.UserDetails.Age < 0 ? -1 * result.UserDetails.Age : result.UserDetails.Age;
                    }
                    else if (ret.ToUpper() == "N")
                    {
                        XmlNode kycAuthResError = xml11.SelectNodes("//kycAuthResError")[0];

                        if (kycAuthResError != null)
                        {
                            result.Status = kycAuthResError.InnerText;
                        }
                        else if (xml11.SelectNodes("//error")[0] != null)
                        {
                            XmlNode error = xml11.SelectNodes("//error")[0];
                            if (error.InnerText == "K-100")
                            {
                                result.Status = error.InnerText;
                            }
                            else { result.Status = "0"; }
                        }
                        else { result.Status = "0"; }

                        //Biometric data did not match with Aadhar No.                        
                    }
                    else
                    {
                        result.Status = "0";
                    }
                    //StringBuilder htmlTableString = new StringBuilder();
                    //htmlTableString.AppendLine("<table class='signup' cellpadding='5' cellspacing='5' style='border-collapse: collapse; width:730px'>");
                    //htmlTableString.AppendLine("<tr><td colspan='3'><h1>UID DETAILS</h1></td></tr>");
                    //htmlTableString.AppendLine("<tr><td><b>UID No.: </b></td><td style='padding-left: 10px';ForeColor='Red'>" + uidno + "</td><td style='padding-left: 10px';ForeColor='Red' rowspan='8'><img src='" + pht + "'></img></td></tr>");
                    //htmlTableString.AppendLine("<tr><td><b>Name:</b> </td><td style='padding-left: 10px';ForeColor='Red'>" + pname + "</td></tr>");
                    //htmlTableString.AppendLine("<tr><td><b>Father's Name: </b></td><td style='padding-left: 10px';ForeColor='Red'>" + fname + "</td></tr>");
                    //htmlTableString.AppendLine("<tr><td><b>DOB: </b></td><td style='padding-left: 10px';ForeColor='Red'>" + dobdate + "</td></tr>");
                    //htmlTableString.AppendLine("<tr><td><b>Gender: </b></td><td style='padding-left: 10px';ForeColor='Red'>" + pgender + "</td></tr>");
                    //htmlTableString.AppendLine("<tr><td><b>Phone No.:</b></td><td style='padding-left: 10px';ForeColor='Red'>" + phone1 + "</td></tr>");
                    //htmlTableString.AppendLine("<tr><td><b>Address: </b></td><td style='padding-left: 10px';ForeColor='Red'>" + Ads + "</td></tr>");
                    //htmlTableString.AppendLine("<tr><td><b>Email: </b></td><td style='padding-left: 10px';ForeColor='Red'>" + email1 + "</td></tr>");
                    //htmlTableString.AppendLine("</table>");
                    return result;
                    //btnQckviewPrint.Visible = true;
                }
                catch (Exception ex2)
                {
                    result.Status = "0";
                    //DBLogger objDBLogger = new DBLogger();
                    new CitizenModel(new DBLogger()).InserBioKYCLog(xml.ToString(), "", xmlString, ex2.Message + ",StackTrace:-" + ex2.StackTrace.ToString(), logId);

                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "VerifyBIO_KYCData", FunctionName = "Error in Aadhar Varify" }, ex2);
                    // result = "Error";
                    return result;

                }

            }
            catch (Exception ex3)
            {
                result.Status = "0";
                //DBLogger objDBLogger = new DBLogger();
                new CitizenModel(new DBLogger()).InserBioKYCLog(xml.ToString(), "", "", ex3.Message + ",StackTrace:-" + ex3.StackTrace.ToString(), logId);
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "VerifyBIO_KYCData", FunctionName = "Error in Bio Aadhar Varify" }, ex3);
                //result = "Error";
                return result;
            }

        }


        /// <summary>
        /// Get AADHAR No. By SSO Id
        /// </summary>
        /// <param name="ssoid">SSO Id of User</param>
        /// <returns>AADHAR No of User</returns>
        public static string GetAADHARNoBySSOId(string ssoid)
        {
            string logid = "";
            try
            {
                SSOWSSoap.SSOUserDetail userDetails = new SSOWSSoap.SSOUserDetail();
                SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
                userDetails = ssoclient.GetUserDetailXML(ssoid, "RAJSAHKAR.TEST", "Rajasthan@12");

                string aadhaarId = userDetails.aadhaarId;
                if (aadhaarId.Length > 12)
                {
                    aadhaarId = COMMON_FUNCTION.GetAadharFromDSMService(userDetails.aadhaarId);
                }

                logid = new csCommon().InsertTraceLogs("GenericCode.cs", "GetAADHARNoBySSOId", "aadhaarId", ssoid+'#'+aadhaarId, "W", "NA", "");
                return aadhaarId;
            }
            catch
            {
                return string.Empty;
            }

        }



    }


    /// <summary>
    /// Static class for SMS Sending
    /// </summary>
    public static class SMS
    {
        /// <summary>
        /// Send Message on behalf of provided Mobile No. and message body
        /// </summary>
        /// <param name="mobileNo">mobile no. exacly in 10 digits only</param>
        /// <param name="message">Non empty message (max length is 160 characters)</param>
        /// <returns>Response as json with response message and result: True/False</returns>
        public static string SendSMS(string mobileNo, string message)
        {
            string Msg = string.Empty;
            string regExpForMobileNo = @"^[0-9]{10}$";
            bool isValidMobileNo = Regex.IsMatch(mobileNo, regExpForMobileNo);

            if (!isValidMobileNo)
                return new JavaScriptSerializer().Serialize(new { Result = false, Message = "Mobile No. must be of 10 digits.", MobileNo = mobileNo });

            if (string.IsNullOrEmpty(message))
                return new JavaScriptSerializer().Serialize(new { Result = false, Message = "Message must not be blank.", MobileNo = mobileNo });

            if (message.Length > 160)
                return new JavaScriptSerializer().Serialize(new { Result = false, Message = "Message length is exceeding from max length 160 characters.", MobileNo = mobileNo });


            EsanchaarController EC = new EsanchaarController();
            string ReturnVal = EC.SendSocietySMS(message, mobileNo);
            return new JavaScriptSerializer().Serialize(new { Result = true, Message = "Message sent successfully.", MobileNo = mobileNo });
        }
    }

    public static class MessageOnActivity
    {
        public static string Message(int processId, int actionId)
        {
            string ReturnVal = string.Empty;
            try
            {
                ApplicationFlowModel model = new ApplicationFlowModel(new DBLogger());
                DataTable dt = model.SendMessageOnActivity(processId, actionId);

                if (dt != null && dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0][0]) != "-1")
                {
                    string message = Convert.ToString(dt.Rows[0]["Message"]);
                    string mobileNo = Convert.ToString(dt.Rows[0]["MobileNo"]);

                    EsanchaarController EC = new EsanchaarController();
                    ReturnVal = EC.SendSocietySMS(message, mobileNo);
                }
            }
            catch
            {
                return new JavaScriptSerializer().Serialize(new { Result = true, Message = "Error while Message sent.", MobileNo = string.Empty });
            }

            return ReturnVal;
        }

        public static DataTable GetMessageBodyActivityKey(string ActivityKey)
        {
            DataTable dt = new DataTable();
            try
            {
                ApplicationFlowModel model = new ApplicationFlowModel(new DBLogger());
                dt = model.GetMessageByActivityKey(ActivityKey);
            }
            catch
            {
                //return new JavaScriptSerializer().Serialize(new { Result = true, Message = "Error while Message sent.", MobileNo = string.Empty });
            }

            return dt;
        }
    }
}
public class KYCResponce
{
    public string Status { get; set; }
    public UserDetails UserDetails { get; set; }
}

public class BIOKYCRequest
{
    public string AadharNo { get; set; }
    public string XML { get; set; }
}



public class UserDetails
{
    public string AadharNo { get; set; }
    public string Name { get; set; }
    public string Photo { get; set; }
    public string FName { get; set; }
    public string DOB { get; set; }
    public string Gender { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string House { get; set; }
    public string Street { get; set; }
    public string lm { get; set; }
    public string loc { get; set; }
    public string vtc { get; set; }
    public string Dist { get; set; }
    public string State { get; set; }
    public string pc { get; set; }
    public int Age { get; set; }


}