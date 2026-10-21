using Cooperative.Controllers;
using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CoopDAL;
using CoopDB;
using Models;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Web.Helpers;
using System.EnterpriseServices;
using Org.BouncyCastle.Asn1.Crmf;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Cooperative.Models
{
    public class RajNiveshModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;

        public RajNiveshModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
        }
        public void ViewApplication(Int32 SWSID, int ActID, int ActivityID, int IsNew, string RegNo = "")
        {
            try
            {

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.RajNiveshModel", FunctionName = "ViewApplication(Int32 SWSID, int ActID, int ActivityID, int IsNew, string RegNo = \"\")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
        }

        public RajNiveshResponse UpdateStatus(RajNiveshStatusRequest request)
        {
            string requestString = JsonConvert.SerializeObject(request);
            try
            {
                if (UpdatStatusInDB(request))
                {
                    using (HttpClient client = new HttpClient { BaseAddress = new Uri(GlobalConstant.RAJNIVESH_API_URL + "REGNUMUPDATE") })
                    {
                        string encoded = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(GlobalConstant.RAJNIVESH_APPNAME + ":" + GlobalConstant.RAJNIVESH_PASSWORD));
                        client.DefaultRequestHeaders.Add("XAPIKey", GlobalConstant.RAJNIVESH_XAPIKey);
                        client.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        using (var response = client.PostAsync(client.BaseAddress, new StringContent(requestString, Encoding.UTF8, "application/json")).Result)
                        {
                            var asyncResponse = response.Content.ReadAsStringAsync().Result;
                            return new JavaScriptSerializer().Deserialize<RajNiveshResponse>(asyncResponse);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.RajNiveshModel", FunctionName = "UpdateStatus(RajNiveshRequest request)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return new RajNiveshResponse();
        }
        public RajNiveshResponse UpdateRegistrationNo(RajNiveshStatusRequest request)
        {
            string requestString = JsonConvert.SerializeObject(request);
            try
            {
                if (UpdatStatusInDB(request))
                {
                    using (HttpClient client = new HttpClient { BaseAddress = new Uri(GlobalConstant.RAJNIVESH_API_URL + "REGNUMUPDATE") })
                    {
                        string encoded = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(GlobalConstant.RAJNIVESH_APPNAME + ":" + GlobalConstant.RAJNIVESH_PASSWORD));
                        client.DefaultRequestHeaders.Add("XAPIKey", GlobalConstant.RAJNIVESH_XAPIKey);
                        client.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        using (var response = client.PostAsync(client.BaseAddress, new StringContent(requestString, Encoding.UTF8, "application/json")).Result)
                        {
                            var asyncResponse = response.Content.ReadAsStringAsync().Result;
                            return new JavaScriptSerializer().Deserialize<RajNiveshResponse>(asyncResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.RajNiveshModel", FunctionName = "UpdateRegistrationNo(RajNiveshRequest request)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return new RajNiveshResponse();
        }
        public static string WSResponseSSO(string sURL, string sContentType, string sSSOToken)
        {
            string sResponse = "";
            try
            {
                string sUserName = GlobalConstant.RAJNIVESH_APPNAME;
                string sPWD = GlobalConstant.RAJNIVESH_PASSWORD;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = sContentType;
                    client.Headers[HttpRequestHeader.Accept] = sContentType;
                    client.Headers["cache-control"] = "no-cache";
                    client.Headers["SSO-TOKEN"] = sSSOToken;
                    client.Headers[HttpRequestHeader.Authorization] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(sUserName + ":" + sPWD));

                    // Disable certificate validation (not recommended for production)
                    ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                    try
                    {
                        sResponse = client.DownloadString(sURL);
                    }
                    catch (WebException ex)
                    {
                        // Additional error handling
                    }
                }
            }
            catch (Exception ex)
            {
                // ErrorLog(ex.Message + " " + ex.InnerException, System.Reflection.MethodBase.GetCurrentMethod().Name, System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Path));
            }
            return sResponse;
        }
        public string InsertSWCSData(RajNiveshRequest rajNivesh)
        {
            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("p_RajNiveshSWCS", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "INSERT");
                objDBAccess.AddParameterToCommand("@SWSID", DBFactoryParameterType.Int, rajNivesh.SWSID);
                objDBAccess.AddParameterToCommand("@ActID", DBFactoryParameterType.VARCHAR, rajNivesh.ActID);
                objDBAccess.AddParameterToCommand("@ActivityID", DBFactoryParameterType.VARCHAR, rajNivesh.ActivityID);
                objDBAccess.AddParameterToCommand("@IsNew", DBFactoryParameterType.VARCHAR, rajNivesh.IsNew);
                objDBAccess.AddParameterToCommand("@RegNo", DBFactoryParameterType.VARCHAR, rajNivesh.RegNo);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.VARCHAR, rajNivesh.SSO_ID);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.VARCHAR, rajNivesh.SocietyIdEnc);
                string regno = objDBAccess.ExecuteScalarCommand().ToString();
                return regno;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.RajNiveshModel", FunctionName = "UpdatStatusInDB(RajNiveshRequest request)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return "";
        }
        private bool UpdatStatusInDB(RajNiveshStatusRequest request)
        {
            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("p_RajNiveshSWCS", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "UPDATESTATUS");
                objDBAccess.AddParameterToCommand("@SWSID", DBFactoryParameterType.Int, request.TRACKERID);
                objDBAccess.AddParameterToCommand("@STATUS_CODE", DBFactoryParameterType.VARCHAR, request.STATUS_CODE);
                objDBAccess.AddParameterToCommand("@FEES", DBFactoryParameterType.VARCHAR, request.FEES);
                objDBAccess.AddParameterToCommand("@REMARKS", DBFactoryParameterType.VARCHAR, request.REMARKS);
                objDBAccess.AddParameterToCommand("@RegNo", DBFactoryParameterType.VARCHAR, request.REGISTRATION_NUMBER);
                var res = objDBAccess.ExecuteCommand();
                if (res > 0)
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.RajNiveshModel", FunctionName = "UpdatStatusInDB(RajNiveshRequest request)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return false;
        }
        public DataTable GetSwcData(string SWSID, string SSOID = "")
        {
            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("p_RajNiveshSWCS", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "GETDATA");
                objDBAccess.AddParameterToCommand("@SWSID", DBFactoryParameterType.Int, SWSID);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.VARCHAR, SSOID);
                DataTable res = objDBAccess.GetTable();
                return res;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.RajNiveshModel", FunctionName = $"GetSwcData(string SWSID={SWSID}, string SSOID = {SSOID})" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return new DataTable();
        }
        public DataTable GetSwcStatusDataBySocietyId(string SocietyIDEnc)
        {
            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("p_RajNiveshSWCS", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "IsApplicationFromSWCS");
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.VARCHAR, SocietyIDEnc);
                DataTable res = objDBAccess.GetTable();
                return res;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.RajNiveshModel", FunctionName = "GetSwcStatusData(string SWSID, string SSOID = \"\")" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return new DataTable();
        }
    }
    public class RajNiveshRequest
    {

        public Int32 SWSID { get; set; }
        public Int32 ActID { get; set; }
        public Int32 ActivityID { get; set; }
        public Int32 IsNew { get; set; }
        public string SocietyIdEnc { get; set; }
        public Int32 IsView { get; set; }
        public string RegNo { get; set; }
        public int Status { get; set; }
        public string SSO_ID { get; set; }
        public string Userdetails { get; set; }
    }
    public class RajNiveshStatusRequest
    {
        public Int32 TRACKERID { get; set; }
        public string REGISTRATION_NUMBER { get; set; }
        public Int32 STATUS_CODE { get; set; }
        public string REMARKS { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal FEES { get; set; }
    }
    public class RajNiveshResponse
    {
        public string ResMessage { get; set; }
        public Int32 ResCode { get; set; }
    }
}
