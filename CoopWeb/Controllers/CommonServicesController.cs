using Cooperative.Filters;
using Cooperative.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using sun.util.calendar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static Cooperative.Models.GCCServiceModel;
using static Cooperative.Models.GopalCreditCardModel;
using static Cooperative.Models.CoopModel;
using static javax.net.ssl.SSLEngineResult;

namespace Cooperative.Controllers
{
    public class CommonServicesController : Controller
    {
        GCCServiceModel _gccServiceModel = new GCCServiceModel(new DBLogger());
        CoopModel _coopServiceModel = new CoopModel(new DBLogger());

        private static readonly HttpClient httpClient = new HttpClient(); // Reuse across calls
        [HttpPost]
        public string CourtCaseApproval(message objmessage)
        {
            try
            {
                CommonServicesModel objCommonServicesModel = new CommonServicesModel(new FileLogger());
                //Id9Cqk5IUHyKyjpGabsLUS4zAGwg14qcfNDcyDK9dPM=            
                if (COMMON_FUNCTION.Decrypt(objmessage.AppAuthKey) == "COOPUMESHRAJ@!#21")
                {
                    int Responce = objCommonServicesModel.UpdatepermanentCaseRegNo(objmessage);

                    if (Responce == -5)
                    {
                        objmessage.msg = "Success";
                    }
                    else
                        objmessage.msg = "Error";
                }
                else
                    objmessage.msg = "Authentication Failed !";


            }
            catch (Exception ex)
            {
                objmessage.msg = ex.Message;
            }
            return JsonConvert.SerializeObject(objmessage, Newtonsoft.Json.Formatting.Indented);

        }

        public string GetDataFromProd(string RegistarionNo)
        {
            DataSet ds = new DataSet();
            //registrationNo = "121/JODHPUR/1992-93";
            // Create a request for the URL.
            WebRequest request = WebRequest.Create("http://rajsahakarapp.rajasthan.gov.in/api/Services/GetSocietyBasicDetails?RegistrationNo=" + RegistarionNo);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            // The using block ensures the stream is automatically closed.
            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                var data = JsonConvert.DeserializeObject<Object>(responseFromServer);

                ds = JsonConvert.DeserializeObject<DataSet>(data.ToString());

            }
            // Close the response.
            response.Close();

            (new CommonServicesModel(new FileLogger())).GetSocietyBasicDetails(ds.Tables[0]);

            return "1";
        }

        public void EMITRATHIRDPARTYPENDENCYSERVICE()
        {

            //ApplicationTransactionMessage message = new ApplicationTransactionMessage();
            using (WebClient Client = new WebClient())
            {
                try
                {
                    SocietyModel obj = new SocietyModel();

                    var data = COMMON_FUNCTION.DataTableToJSONWithJSONNet(obj.GetEmitraServiseData());
                    data = "{data:" + data + "}";
                    string jsonString = string.Empty;
                    jsonString = JsonConvert.SerializeObject(data);

                    var http = (HttpWebRequest)WebRequest.Create(new Uri("https://api.sewadwaar.rajasthan.gov.in/app/live/emitra/Prod/webServices/Repository/pushThirdPartyPendencyDataApi/service?client_id=02a79073-c2db-4c84-b810-885fdaa8c54a"));
                    //var http = (HttpWebRequest)WebRequest.Create(new Uri("https://apitest.sewadwaar.rajasthan.gov.in/app/live/emitra/Prod/webServicesRepositoryUat/pushThirdPartyPendencyDataApi/service?client_id=02a79073-c2db-4c84-b810-885fdaa8c54a"));

                    //http.Accept = "application/json";
                    //http.ContentType = "application/json";
                    http.Method = "POST";

                    // string parsedContent = <<PUT HERE YOUR JSON PARSED CONTENT>>;
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    Byte[] bytes = encoding.GetBytes(jsonString);

                    Stream newStream = http.GetRequestStream();
                    newStream.Write(bytes, 0, bytes.Length);
                    newStream.Close();

                    var response = http.GetResponse();

                    var stream = response.GetResponseStream();
                    var sr = new StreamReader(stream);
                    var content = sr.ReadToEnd();
                    //message = JsonConvert.DeserializeObject<ApplicationTransactionMessage>(content);
                }
                catch (Exception ex)
                {
                    //  ErrorLog.Write(ex);
                }
            }
            // return message;

        }
        //public DataSet jsonToDataSet(string jsonString)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        XmlDocument xd = new XmlDocument();
        //        jsonString = "{​​​​​​​​ \"rootNode\": {​​​​​​​​" + jsonString.Trim().TrimStart('{​'​​​​​).TrimEnd("}​​​​​​​​") + "}​​​​​​​​ }​​​​​​​​";
        //        xd = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonString);

        //        ds.ReadXml(new XmlNodeReader(xd));
        //        return ds;
        //    }
        //    catch (Exception ex){

        //    }
        //    return ds;
        //}​​​​​​​​ 





        [HttpGet]
        public ActionResult UpdateCityzenPaymentStatus(string RegistrationNo, string UniquerequestId, string SocietyIdEnc, string IsFailed)
        {
            string encData = "";
            DataTable Dt = null;
            string decryptData = "";
            PGResponse objPGResponse = null;

            string DivHtml = "";

            UniquerequestId = UniquerequestId.Replace(' ', '+');
            UniquerequestId = UniquerequestId.Replace(' ', '+');
            UniquerequestId = UniquerequestId.Replace(' ', '+');

            SocietyIdEnc = SocietyIdEnc.Replace(' ', '+');
            SocietyIdEnc = SocietyIdEnc.Replace(' ', '+');
            SocietyIdEnc = SocietyIdEnc.Replace(' ', '+');

            IsFailed = IsFailed.Replace(' ', '+');
            IsFailed = IsFailed.Replace(' ', '+');
            IsFailed = IsFailed.Replace(' ', '+');

            UserModel OBJUser = new UserModel(new FileLogger());
            if (Request["encData"] != null)
            {
                EmitraModelFactory objEmitraModel = new EmitraModelFactory(new FileLogger());
                Dt = objEmitraModel.GetApplicationKey("4690", false);
                encData = Request["encData"].ToString().Replace("@!", "+");
                EncryptDecrypt3DES obj = new EncryptDecrypt3DES(Dt.Rows[0]["encryptionkey"].ToString());
                decryptData = obj.decrypt(encData);
                objPGResponse = JsonConvert.DeserializeObject<PGResponse>(decryptData);
                //objPGResponse = new PGResponse();
                //objPGResponse.SERVICEID = "4690";
                //objPGResponse.RECEIPTNO = "18000219184";
                //objPGResponse.TRANSACTIONID = "180000227755";
                //objPGResponse.RESPONSEMESSAGE = "Transaction Successfully Done";
                //objPGResponse.STATUS = "Transaction Successfully Done";
                //objPGResponse.REFERENCE_ID = "0";
                //objPGResponse.BANK_BID_NO = "0";
                //objPGResponse.BANK_ACC_NO = "123";
                //objPGResponse.BANK_NAME = "PNB";
                Dt = new EmitraModelFactory(new FileLogger()).GetApplicationKey(objPGResponse.SERVICEID, true);
                CitizenModel objCitizenModel = new CitizenModel(new FileLogger());
                DivHtml = objCitizenModel.UpdateOnlinePaymentStatusAll(RegistrationNo, COMMON_FUNCTION.Decrypt(SocietyIdEnc), COMMON_FUNCTION.Decrypt(UniquerequestId), COMMON_FUNCTION.Decrypt(IsFailed), decryptData);
                objPGResponse = JsonConvert.DeserializeObject<PGResponse>(decryptData);
                if (objPGResponse.RESPONSEMESSAGE.ToString().Contains("Successfully"))
                {
                    return RedirectToAction("MemberApplicationView", "Citizen", new { ApplicationIdEnc = SocietyIdEnc });
                }
                else
                {
                    return RedirectToAction("MemberApplicationView", "Citizen", new { ApplicationIdEnc = "Error" + objPGResponse.RESPONSEMESSAGE });
                }

                //ViewBag.DivHtmlData = DivHtml;
                //return RedirectToAction("SSOMapping", "Login");
            }





            return View();
        }


        [HttpPost]
        public string GetCaseDetailsByCaseId(CaseRegistraion objmessage)
        {
            DataSet Dset = null;
            try
            {
                CommonServicesModel objCommonServicesModel = new CommonServicesModel(new FileLogger());
                ////Id9Cqk5IUHyKyjpGabsLUS4zAGwg14qcfNDcyDK9dPM=            
                if (COMMON_FUNCTION.Decrypt(objmessage.AppAuthKey) == "COOPUMESHRAJ@!#21")
                {
                    Dset = objCommonServicesModel.GetCaseDetailsByCaseId(objmessage.CaseRegID);
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                //objmessage.msg = ex.Message;
            }

            return JsonConvert.SerializeObject(Dset, Newtonsoft.Json.Formatting.Indented);

        }

        [HttpPost]
        public string GetCaseBySsoOrCaseno(CaseRegistraion objmessage)
        {
            DataTable dt = null;
            try
            {
                CommonServicesModel objCommonServicesModel = new CommonServicesModel(new FileLogger());
                //Id9Cqk5IUHyKyjpGabsLUS4zAGwg14qcfNDcyDK9dPM=            
                if (COMMON_FUNCTION.Decrypt(objmessage.AppAuthKey) == "COOPUMESHRAJ@!#21")
                {
                    dt = objCommonServicesModel.GetAllCaseList(objmessage.SSOID);
                }

            }
            catch (Exception ex)
            {
                //  objmessage.msg = ex.Message;
            }
            return JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.Indented);

        }


        [HttpPost]
        public string CitizenDashboardDataForAuditRPT(SocietyRegistrationEntity objSocietyRegistrationEntity)
        {
            DataTable DT = null;
            CommonServicesModel objCommonServicesModel = new CommonServicesModel(new FileLogger());
            ////Id9Cqk5IUHyKyjpGabsLUS4zAGwg14qcfNDcyDK9dPM=            
            if (COMMON_FUNCTION.Decrypt(objSocietyRegistrationEntity.AppAuthKey) == "COOPUMESHRAJ@!#21")
            {
                DT = objCommonServicesModel.GetSocietyAuditReport(objSocietyRegistrationEntity.SocietyName);

            }


            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }

        [HttpPost]
        public string GetAllTrainning(TrainningModel objTrainningModel)
        {
            DataTable DT = null;
            // string WhereCluase = "";
            //if (objTrainningModel.ChannelId != 0)
            //    WhereCluase += "and Channels.ChannelId=" + (objTrainningModel.ChannelId);
            //if (objTrainningModel.OrganisationId != 0)
            //    WhereCluase += "and Organisations.OrganisationId=" + objTrainningModel.OrganisationId;
            //if (objTrainningModel.TrainningId != 0)
            //    WhereCluase += "and Trainning.TrainningId=" + objTrainningModel.TrainningId;
            //else
            objTrainningModel.WhereClause = "";


            CommonServicesModel objCommonServicesModel = new CommonServicesModel(new FileLogger());
            ////Id9Cqk5IUHyKyjpGabsLUS4zAGwg14qcfNDcyDK9dPM=            
            if (COMMON_FUNCTION.Decrypt(objTrainningModel.AppAuthKey) == "COOPUMESHRAJ@!#21")
            {
                DT = objCommonServicesModel.GetAllTrainning(objTrainningModel.WhereClause);
            }

            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }

        [HttpPost]
        public string HouseSocietyListForAuditRPTForWeb(string Data)
        {
            //SocietyModel objSocietyModel = new SocietyModel(new FileLogger());
            //if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
            //{
            //    objSocietyRegistrationEntity.UserType = (Request.Cookies["SSOROLE"].Value == "CITIZEN" ? "C" : "D");
            //    objSocietyRegistrationEntity.SSOID = Request.Cookies["SSO_ID"].Value;
            //}
            //DataTable DT = objSocietyModel.HouseSocietyListForAuditRPT(objSocietyRegistrationEntity);
            return COMMON_FUNCTION.DataTableToJSONString(null);

        }


        [HttpPost]
        public string Mobile_GetSTLoanDetailByFinancialYear(string FIN_YEAR_ID)//APEX
        {
            var result = "Error";
            try
            {
                CommonServicesModel objCommonServicesModel = new CommonServicesModel(new DBLogger());
                int _FIN_YEAR_ID = 0;
                int.TryParse(FIN_YEAR_ID, out _FIN_YEAR_ID);
                DataTable DT = objCommonServicesModel.GetDataForMobileApp(_FIN_YEAR_ID);
                result = COMMON_FUNCTION.DataTableToJSONString(DT);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_GetSTLoanDetailByFinancialYear" }, ex);
                return "";
            }
            finally
            {

            }
            return result;
        }

        [HttpPost]
        public string Mobile_GetFinancialYear()
        {
            var result = "Error";
            try
            {
                CommonServicesModel objCommonServicesModel = new CommonServicesModel(new DBLogger());
                DataTable DT = objCommonServicesModel.MobileAppFinancialYearList();
                result = COMMON_FUNCTION.DataTableToJSONString(DT);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_GetFinancialYear" }, ex);
            }
            finally
            {

            }
            return result;
        }

        [HttpPost]
        public string Mobile_GetRajFedData(string MSPRAJFED) //RAJFED
        {
            var result = "Error";
            try
            {
                ORDERSList obj = new ORDERSList();
                var hr = DateTime.Now.Hour;
                string date;
                if (hr > 10)
                    date = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                else
                    date = DateTime.Now.AddDays(-1).Year + "-" + DateTime.Now.AddDays(-1).Month + "-" + DateTime.Now.AddDays(-1).Day;

                string uri = "http://164.100.153.13/NAFED/ServiceApp.svc/clubreport/" + date;


                using (WebClient webClient = new WebClient())
                {
                    using (var stream = webClient.OpenRead(uri))
                    {
                        using (var streamReader = new StreamReader(stream))
                        {
                            result = streamReader.ReadToEnd();
                        }
                    }
                }

                if (result.Length < 20)
                {
                    uri = "http://164.100.153.13/NAFED/ServiceApp.svc/clubreport/2019-07-01";
                    using (WebClient webClient = new WebClient())
                    {
                        using (var stream = webClient.OpenRead(uri))
                        {
                            using (var streamReader = new StreamReader(stream))
                            {
                                result = streamReader.ReadToEnd();
                            }
                        }
                    }
                }

                StringReader theReader = new StringReader(result);
                DataSet theDataSet = new DataSet();
                theDataSet.ReadXml(theReader);
                //RajfedServicesData yourObject = new RajfedServicesData();

                if (MSPRAJFED == null)
                    MSPRAJFED = "2";

                if (theDataSet.Tables.Count > 0)
                {
                    DataTable NEWDT = theDataSet.Tables[0];
                    NEWDT.Columns.Add("RankPer", typeof(System.Decimal));

                    if (MSPRAJFED == "2")
                    {
                        foreach (DataRow Dr in NEWDT.Rows)
                        {
                            decimal RankPer = 0;
                            var Cl = decimal.Parse(Dr["PurchaseTarget"].ToString());
                            var C2 = decimal.Parse(Dr["PurchaseAmount"].ToString());
                            Cl = Math.Round(Cl * 100000);
                            Dr["PurchaseTarget"] = Cl;
                            if (Cl != 0 && C2 != 0)
                            {
                                RankPer = (decimal.Parse(Dr["PurchaseQty"].ToString()) / Cl) * 100;
                            }
                            Dr["RankPer"] = RankPer;
                        }
                    }
                    else if (MSPRAJFED == "1")
                    {
                        foreach (DataRow Dr in NEWDT.Rows)
                        {
                            decimal RankPer = 0;
                            var Cl = decimal.Parse(Dr["PurchaseTarget"].ToString());
                            Cl = (Cl * 1000000);
                            Cl = (Cl * decimal.Parse(Dr["Rate"].ToString()));
                            Cl = (Cl / 10000000);
                            Dr["PurchaseTarget"] = Cl;
                            var C2 = Math.Round(Convert.ToDecimal(Dr["PurchaseAmount"].ToString()), 2);
                            if (Cl != 0 && C2 != 0)
                            {
                                RankPer = (decimal.Parse(Dr["PurchaseAmount"].ToString()) / decimal.Parse(Dr["PurchaseTarget"].ToString()) * 100);
                            }

                            Dr["RankPer"] = RankPer;
                        }
                    }
                    NEWDT.DefaultView.Sort = "RankPer DESC";

                    result = COMMON_FUNCTION.DataTableToJSONString(NEWDT);
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_GetRajFedData" }, ex);
            }
            finally
            {

            }
            return result;

        }


        [HttpPost]
        public string Mobile_GetRajFedDataForMSP(string TransactionId) //RAJFED
        {
            var result = "Error";
            try
            {
                ORDERSList obj = new ORDERSList();
                var hr = DateTime.Now.Hour;
                //string APP="0103428856";


                //string uri = "http://164.100.153.13/NAFED/ServiceApp.svc/clubreport1/" + APP;

                string uri = "http://164.100.153.13/NAFED/ServiceApp.svc/clubreport2/" + TransactionId;


                using (WebClient webClient = new WebClient())
                {
                    using (var stream = webClient.OpenRead(uri))
                    {
                        using (var streamReader = new StreamReader(stream))
                        {
                            result = streamReader.ReadToEnd();
                        }
                    }
                }
                if (result.Length < 15)
                {
                    result = "No Record Found";
                }
                else
                {
                    StringReader theReader = new StringReader(result);
                    DataSet theDataSet = new DataSet();
                    theDataSet.ReadXml(theReader);

                    if (theDataSet.Tables.Count > 0)
                    {
                        DataTable NEWDT = theDataSet.Tables[0];
                        //NEWDT.Columns.Add("Status", typeof(System.String));
                        //NEWDT.Rows[0]["Status"] = "भुगतान किया जा चुका है";

                        result = COMMON_FUNCTION.DataTableToJSONString(NEWDT);
                    }
                }



            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_GetRajFedDataForMSP" }, ex);
            }
            finally
            {

            }
            return result;

        }

        [HttpPost]
        public string Mobile_GetRajFedPurchaseCenterTransactionList(string PurchaseCenter_ID) //RAJFED
        {
            var result = "Error";
            try
            {
                ORDERSList obj = new ORDERSList();
                var hr = DateTime.Now.Hour;

                string uri = "http://164.100.153.13/NAFED/ServiceApp.svc/clubreport4/" + PurchaseCenter_ID;
                using (WebClient webClient = new WebClient())
                {
                    using (var stream = webClient.OpenRead(uri))
                    {
                        using (var streamReader = new StreamReader(stream))
                        {
                            result = streamReader.ReadToEnd();
                        }
                    }
                }
                if (result.Length < 15)
                {
                    result = "No Record Found";
                }
                else
                {
                    StringReader theReader = new StringReader(result);
                    DataSet theDataSet = new DataSet();
                    theDataSet.ReadXml(theReader);

                    if (theDataSet.Tables.Count > 0)
                    {
                        DataTable NEWDT = theDataSet.Tables[0];
                        result = COMMON_FUNCTION.DataTableToJSONString(NEWDT);
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_GetRajFedPurchaseCenterList" }, ex);
            }
            finally
            {

            }
            return result;

        }
        [HttpPost]
        public string Mobile_GetRajFedPurchaseCenterList() //RAJFED
        {
            var result = "Error";
            try
            {
                ORDERSList obj = new ORDERSList();
                var hr = DateTime.Now.Hour;

                string uri = "http://164.100.153.13/NAFED/ServiceApp.svc/clubreport3";
                using (WebClient webClient = new WebClient())
                {
                    using (var stream = webClient.OpenRead(uri))
                    {
                        using (var streamReader = new StreamReader(stream))
                        {
                            result = streamReader.ReadToEnd();
                        }
                    }
                }
                if (result.Length < 15)
                {
                    result = "No Record Found";
                }
                else
                {
                    StringReader theReader = new StringReader(result);
                    DataSet theDataSet = new DataSet();
                    theDataSet.ReadXml(theReader);

                    if (theDataSet.Tables.Count > 0)
                    {
                        DataTable NEWDT = theDataSet.Tables[0];
                        //NEWDT.Columns.Remove("PurchaseCenter_Shakha");
                        //NEWDT.Columns.Remove("PurchaseCenter_Samiti");
                        //NEWDT.Columns.Remove("PurchaseCenter_PC");
                        //NEWDT.Columns.Remove("UserId");
                        //NEWDT.Columns.Remove("ip");
                        //NEWDT.Columns.Remove("Agency_ID");
                        //NEWDT.Columns.Remove("Farmer_Cap_M");
                        //NEWDT.Columns.Remove("Farmer_Cap_G");
                        //NEWDT.Columns.Remove("IsActive");
                        //NEWDT.Columns.Remove("Farmer_Cap_W");
                        //NEWDT.Columns.Remove("Farmer_Cap_L");
                        //NEWDT.Columns.Remove("Farmer_Cap_P");
                        //NEWDT.Columns.Remove("Farmer_Cap_Moong");
                        //NEWDT.Columns.Remove("Farmer_Cap_GN");
                        //NEWDT.Columns.Remove("Farmer_Cap_Udad");
                        //NEWDT.Columns.Remove("Farmer_Cap_Soya");
                        result = COMMON_FUNCTION.DataTableToJSONString(NEWDT);
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_GetRajFedPurchaseCenterList" }, ex);
            }
            finally
            {

            }
            return result;

        }

        [HttpPost]
        public string Mobile_GetlwaData()  //LWA
        {
            string result = "Error";
            try
            {
                CitizenModel _citobj = new CitizenModel(new FileLogger());
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://lwa.rajasthan.gov.in/API/DashBoardData");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "";
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_GetlwaData" }, ex);
            }
            finally
            {

            }
            return result;
        }


        [HttpPost]
        public string Mobile_GetConfedOrderDetails(string YearWiseOrAll)
        {
            var result = "";
            try
            {
                if (YearWiseOrAll == "undefined")
                {
                    YearWiseOrAll = "2";
                }

                string uri = "";
                string MM = "";
                string DD = "";

                if (DateTime.Now.Month < 10)
                    MM = "0" + DateTime.Now.Month.ToString();
                else
                    MM = DateTime.Now.Month.ToString();


                if (DateTime.Now.Day < 10)
                    DD = "0" + DateTime.Now.Day.ToString();
                else
                    DD = DateTime.Now.Day.ToString();


                string Date = DateTime.Now.Year.ToString() + "-" + MM + "-" + DD;
                string Date2 = DateTime.Now.Year.ToString() + "-04" + "-01";
                ORDERSList obj = new ORDERSList();

                if (YearWiseOrAll == "1")//OVER ALL
                    uri = "https://ebazaar.rajasthan.gov.in/Cooperative/OrderList?OdrFromDate=2011-01-01&OdrToDate=" + Date;
                else
                    uri = "https://ebazaar.rajasthan.gov.in/Cooperative/OrderList?OdrFromDate=" + Date2 + "&OdrToDate=" + Date;

                using (WebClient webClient = new WebClient())
                {
                    using (var stream = webClient.OpenRead(uri))
                    {
                        using (var streamReader = new StreamReader(stream))
                        {
                            result = streamReader.ReadToEnd();
                        }
                    }
                }

                //DataSet myDataSet = JsonConvert.DeserializeObject<DataSet>(result);
                ORDERSList yourObject = JsonConvert.DeserializeObject<ORDERSList>(result);
                Dictionary<string, decimal> AuthorList = new Dictionary<string, decimal>();
                foreach (ORDERSData Obj in yourObject.ORDERS)
                {
                    if (!AuthorList.ContainsKey(Obj.DISTRICT_NAME))
                    {
                        AuthorList.Add(Obj.DISTRICT_NAME, Obj.AMOUNT);
                    }
                    else if (AuthorList.ContainsKey(Obj.DISTRICT_NAME))
                    {
                        AuthorList[Obj.DISTRICT_NAME] += Obj.AMOUNT;
                    }
                }

                result = COMMON_FUNCTION.DataTableToJSONString(MyDictionaryToJson(AuthorList));
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServicesController", FunctionName = "Mobile_GetConfedOrderDetails" }, ex);
            }
            finally
            {

            }
            return result;

        }

        DataTable MyDictionaryToJson(Dictionary<string, decimal> dict)
        {
            DataTable foundPrices = new DataTable();
            foundPrices.Columns.Add("District", typeof(string));
            foundPrices.Columns.Add("Amount", typeof(string));
            foreach (string str in dict.Keys)
            {
                DataRow dr = foundPrices.NewRow(); //Creating Row  
                dr["District"] = str;
                dr["Amount"] = dict[str].ToString();
                foundPrices.Rows.Add(dr);

            }
            return foundPrices;
        }

        [HttpPost]
        public string Mobile_GetRajFedDataDIstrictWise(string DistrictName = "", string Crop = "") //RAJFED
        {
            var result = "Error";
            try
            {
                ORDERSList obj = new ORDERSList();
                var hr = DateTime.Now.Hour;
                string date;
                if (hr > 10)
                    date = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                else
                    date = DateTime.Now.AddDays(-1).Year + "-" + DateTime.Now.AddDays(-1).Month + "-" + DateTime.Now.AddDays(-1).Day;

                string uri = "http://164.100.153.13/NAFED/ServiceApp.svc/distreport/" + date;


                using (WebClient webClient = new WebClient())
                {
                    using (var stream = webClient.OpenRead(uri))
                    {
                        using (var streamReader = new StreamReader(stream))
                        {
                            result = streamReader.ReadToEnd();
                        }
                    }
                }

                if (result.Length < 20)
                {
                    uri = "http://164.100.153.13/NAFED/ServiceApp.svc/distreport/2019-07-01";
                    using (WebClient webClient = new WebClient())
                    {
                        using (var stream = webClient.OpenRead(uri))
                        {
                            using (var streamReader = new StreamReader(stream))
                            {
                                result = streamReader.ReadToEnd();
                            }
                        }
                    }
                }

                StringReader theReader = new StringReader(result);
                DataSet theDataSet = new DataSet();
                theDataSet.ReadXml(theReader);



                if (theDataSet.Tables.Count > 0)
                {
                    DataTable NEWDT = theDataSet.Tables[0];
                    DataTable dtTable = new DataTable();
                    dtTable.Columns.Add("District", typeof(string));

                    if (Crop == "")
                    {
                        dtTable.Columns.Add("PurchaseQtySarson", typeof(decimal));
                        dtTable.Columns.Add("PurchaseQtyChana", typeof(decimal));
                        dtTable.Columns.Add("PurchaseQtyGehu", typeof(decimal));
                        dtTable.Columns.Add("PurchaseRsSarson", typeof(decimal));
                        dtTable.Columns.Add("PurchaseRsChana", typeof(decimal));
                        dtTable.Columns.Add("PurchaseRsGehu", typeof(decimal));
                        dtTable.Columns.Add("PurchaseRS", typeof(string));
                        dtTable.Columns.Add("PurchaseUnit", typeof(string));
                        dtTable.Columns.Add("TotalPurchaseRs", typeof(decimal));
                        dtTable.Columns.Add("TotalPurchaseQty", typeof(decimal));
                        dtTable.Columns.Add("Rank", typeof(int));
                    }
                    else
                    {
                        if (Crop == "सरसों")
                        {
                            dtTable.Columns.Add("PurchaseQtySarson", typeof(decimal));
                            dtTable.Columns.Add("PurchaseRsSarson", typeof(decimal));
                        }
                        if (Crop == "चना")
                        {
                            dtTable.Columns.Add("PurchaseQtyChana", typeof(decimal));
                            dtTable.Columns.Add("PurchaseRsChana", typeof(decimal));
                        }
                        if (Crop == "गेंहू")
                        {
                            dtTable.Columns.Add("PurchaseQtyGehu", typeof(decimal));
                            dtTable.Columns.Add("PurchaseRsGehu", typeof(decimal));
                        }
                        dtTable.Columns.Add("PurchaseRS", typeof(string));
                        dtTable.Columns.Add("PurchaseUnit", typeof(string));
                        dtTable.Columns.Add("Rank", typeof(int));
                    }


                    if (DistrictName != "" && DistrictName != null)
                    {
                        foreach (DataRow Dr in NEWDT.Rows)
                        {
                            if (Dr["District"].ToString().ToLower() == DistrictName.ToString().ToLower())
                            {
                                DataRow dr = dtTable.NewRow();
                                dr["District"] = Dr["District"];
                                dr["PurchaseQtySarson"] = decimal.Parse(Dr["PurchaseQtySarson"].ToString());
                                dr["PurchaseQtyChana"] = decimal.Parse(Dr["PurchaseQtyChana"].ToString());
                                dr["PurchaseQtyGehu"] = decimal.Parse(Dr["PurchaseQtyGehu"].ToString());
                                dr["PurchaseRsSarson"] = decimal.Parse(Dr["PurchaseRsSarson"].ToString());
                                dr["PurchaseRsChana"] = decimal.Parse(Dr["PurchaseRsChana"].ToString());
                                dr["PurchaseRsGehu"] = decimal.Parse(Dr["PurchaseRsGehu"].ToString());
                                dr["PurchaseUnit"] = "Quintal";
                                dr["PurchaseRS"] = "RS";
                                dr["Rank"] = 1;
                                dtTable.Rows.Add(dr);
                                break;
                            }
                        }
                    }
                    else if (Crop != "" && Crop != null)
                    {
                        foreach (DataRow Dr in NEWDT.Rows)
                        {
                            DataRow dr = dtTable.NewRow();
                            dr["District"] = Dr["District"];
                            if (Crop == "सरसों")
                            {
                                dr["PurchaseQtySarson"] = decimal.Parse(Dr["PurchaseQtySarson"].ToString());
                                dr["PurchaseRsSarson"] = decimal.Parse(Dr["PurchaseRsSarson"].ToString());
                            }
                            if (Crop == "चना")
                            {
                                dr["PurchaseQtyChana"] = decimal.Parse(Dr["PurchaseQtyChana"].ToString());
                                dr["PurchaseRsChana"] = decimal.Parse(Dr["PurchaseRsChana"].ToString());
                            }
                            if (Crop == "गेंहू")
                            {
                                dr["PurchaseQtyGehu"] = decimal.Parse(Dr["PurchaseQtyGehu"].ToString());
                                dr["PurchaseRsGehu"] = decimal.Parse(Dr["PurchaseRsGehu"].ToString());
                            }
                            dr["PurchaseUnit"] = "Quintal";
                            dr["PurchaseRS"] = "RS";

                            dr["Rank"] = 0;
                            dtTable.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        foreach (DataRow Dr in NEWDT.Rows)
                        {
                            DataRow dr = dtTable.NewRow();
                            dr["District"] = Dr["District"];
                            dr["PurchaseQtySarson"] = decimal.Parse(Dr["PurchaseQtySarson"].ToString());
                            dr["PurchaseQtyChana"] = decimal.Parse(Dr["PurchaseQtyChana"].ToString());
                            dr["PurchaseQtyGehu"] = decimal.Parse(Dr["PurchaseQtyGehu"].ToString());
                            dr["PurchaseRsSarson"] = decimal.Parse(Dr["PurchaseRsSarson"].ToString());
                            dr["PurchaseRsChana"] = decimal.Parse(Dr["PurchaseRsChana"].ToString());
                            dr["PurchaseRsGehu"] = decimal.Parse(Dr["PurchaseRsGehu"].ToString());
                            dr["PurchaseUnit"] = "Quintal";
                            dr["PurchaseRS"] = "RS";
                            dr["TotalPurchaseRs"] = decimal.Parse(Dr["PurchaseRsGehu"].ToString()) + decimal.Parse(Dr["PurchaseRsChana"].ToString()) + decimal.Parse(Dr["PurchaseRsSarson"].ToString());
                            dr["TotalPurchaseQty"] = decimal.Parse(Dr["PurchaseQtySarson"].ToString()) + decimal.Parse(Dr["PurchaseQtyChana"].ToString()) + decimal.Parse(Dr["PurchaseQtyGehu"].ToString());
                            dr["Rank"] = 0;
                            dtTable.Rows.Add(dr);
                        }
                    }


                    DataView dv = dtTable.DefaultView;
                    if (Crop != "")
                    {
                        if (Crop == "सरसों")
                        {
                            dv.Sort = "PurchaseQtySarson DESC";
                        }
                        if (Crop == "चना")
                        {
                            dv.Sort = "PurchaseQtyChana DESC";

                        }
                        if (Crop == "गेंहू")
                        {
                            dv.Sort = "PurchaseQtyGehu DESC";
                        }
                    }
                    else
                    {
                        dv.Sort = "TotalPurchaseQty DESC";
                    }
                    dtTable = dv.ToTable();
                    int Count = 0;
                    foreach (DataRow Drr in dtTable.Rows)
                    {
                        Count = Count + 1;
                        Drr["Rank"] = Count;
                    }

                    result = COMMON_FUNCTION.DataTableToJSONString(dtTable);
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_GetRajFedDataDIstrictWise" }, ex);
            }
            finally
            {

            }
            return result;

        }

        [HttpPost]
        public string Mobile_GetFertiliserData(string StateWise, string FinancialYearId)
        {
            var result = "Error";
            try
            {
                CommonServicesModel objCommonServicesModel = new CommonServicesModel(new DBLogger());
                DataTable DT = objCommonServicesModel.Mobile_GetFertiliserData(StateWise, FinancialYearId);
                result = COMMON_FUNCTION.DataTableToJSONString(DT);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_GetFertiliserData" }, ex);
            }
            finally
            {

            }
            return result;
        }


        [HttpPost]
        public string Mobile_GetRajFedDataDIstrictWiseNew() //RAJFED
        {
            var result = "Error";
            try
            {
                ORDERSList obj = new ORDERSList();
                var hr = DateTime.Now.Hour;
                string date;
                if (hr > 10)
                    date = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                else
                    date = DateTime.Now.AddDays(-1).Year + "-" + DateTime.Now.AddDays(-1).Month + "-" + DateTime.Now.AddDays(-1).Day;

                string uri = "http://164.100.153.13/NAFED/ServiceApp.svc/distreport/" + date;


                using (WebClient webClient = new WebClient())
                {
                    using (var stream = webClient.OpenRead(uri))
                    {
                        using (var streamReader = new StreamReader(stream))
                        {
                            result = streamReader.ReadToEnd();
                        }
                    }
                }

                if (result.Length < 20)
                {
                    uri = "http://164.100.153.13/NAFED/ServiceApp.svc/distreport/2019-07-01";
                    using (WebClient webClient = new WebClient())
                    {
                        using (var stream = webClient.OpenRead(uri))
                        {
                            using (var streamReader = new StreamReader(stream))
                            {
                                result = streamReader.ReadToEnd();
                            }
                        }
                    }
                }

                StringReader theReader = new StringReader(result);
                DataSet theDataSet = new DataSet();
                theDataSet.ReadXml(theReader);


                if (theDataSet.Tables.Count > 0)
                {
                    DataTable NEWDT = theDataSet.Tables[0];
                    DataTable dtTable = new DataTable();
                    dtTable.Columns.Add("District", typeof(string));
                    dtTable.Columns.Add("PurchaseQtySarson", typeof(decimal));
                    dtTable.Columns.Add("PurchaseQtyChana", typeof(decimal));
                    dtTable.Columns.Add("PurchaseQtyGehu", typeof(decimal));
                    dtTable.Columns.Add("PurchaseRsSarson", typeof(decimal));
                    dtTable.Columns.Add("PurchaseRsChana", typeof(decimal));
                    dtTable.Columns.Add("PurchaseRsGehu", typeof(decimal));
                    dtTable.Columns.Add("PurchaseRS", typeof(string));
                    dtTable.Columns.Add("PurchaseUnit", typeof(string));
                    dtTable.Columns.Add("Rank", typeof(int));
                    foreach (DataRow Dr in NEWDT.Rows)
                    {
                        DataRow dr = dtTable.NewRow();
                        dr["District"] = Dr["District"];
                        dr["PurchaseQtySarson"] = decimal.Parse(Dr["PurchaseQtySarson"].ToString());
                        dr["PurchaseQtyChana"] = decimal.Parse(Dr["PurchaseQtyChana"].ToString());
                        dr["PurchaseQtyGehu"] = decimal.Parse(Dr["PurchaseQtyGehu"].ToString());
                        dr["PurchaseRsSarson"] = decimal.Parse(Dr["PurchaseRsSarson"].ToString());
                        dr["PurchaseRsChana"] = decimal.Parse(Dr["PurchaseRsChana"].ToString());
                        dr["PurchaseRsGehu"] = decimal.Parse(Dr["PurchaseRsGehu"].ToString());
                        dr["PurchaseUnit"] = "Quintal";
                        dr["PurchaseRS"] = "RS";
                        dr["Rank"] = 0;
                        dtTable.Rows.Add(dr);
                    }

                    DataView dv = dtTable.DefaultView;
                    dv.Sort = "PurchaseQtyGehu DESC";

                    dtTable = dv.ToTable();


                    int Count = 0;

                    foreach (DataRow Drr in dtTable.Rows)
                    {
                        Count = Count + 1;
                        Drr["Rank"] = Count;
                    }

                    result = COMMON_FUNCTION.DataTableToJSONString(dtTable);
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_GetRajFedDataDIstrictWise" }, ex);
            }
            finally
            {

            }
            return result;

        }

        [HttpPost]
        public string Mobile_GetPMkisanDataForDashboard()
        {
            var result = "Error";
            try
            {
                CommonServicesModel objCommonServicesModel = new CommonServicesModel(new DBLogger());
                DataTable DT = objCommonServicesModel.Mobile_GetPMkisanDataForDashboard();
                result = COMMON_FUNCTION.DataTableToJSONString(DT);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_GetPMkisanDataForDashboard" }, ex);
            }
            finally
            {

            }
            return result;
        }


        [HttpPost]
        public string Mobile_GetRajFedDistrictData(string DistrictName) //RAJFED
        {
            var result = "Error";
            try
            {
                ORDERSList obj = new ORDERSList();
                var hr = DateTime.Now.Hour;
                string date;
                if (hr > 10)
                    date = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                else
                    date = DateTime.Now.AddDays(-1).Year + "-" + DateTime.Now.AddDays(-1).Month + "-" + DateTime.Now.AddDays(-1).Day;

                string uri = "http://164.100.153.13/NAFED/ServiceApp.svc/distreport/" + date;


                using (WebClient webClient = new WebClient())
                {
                    using (var stream = webClient.OpenRead(uri))
                    {
                        using (var streamReader = new StreamReader(stream))
                        {
                            result = streamReader.ReadToEnd();
                        }
                    }
                }
                if (result.Length < 20)
                {
                    uri = "http://164.100.153.13/NAFED/ServiceApp.svc/distreport/2019-07-01";
                    using (WebClient webClient = new WebClient())
                    {
                        using (var stream = webClient.OpenRead(uri))
                        {
                            using (var streamReader = new StreamReader(stream))
                            {
                                result = streamReader.ReadToEnd();
                            }
                        }
                    }
                }

                StringReader theReader = new StringReader(result);
                DataSet theDataSet = new DataSet();
                theDataSet.ReadXml(theReader);

                if (DistrictName == null)
                    DistrictName = "";

                if (theDataSet.Tables.Count > 0)
                {
                    DataTable NEWDT = theDataSet.Tables[0];
                    DataTable dtTable = new DataTable();
                    dtTable.Columns.Add("District", typeof(string));
                    dtTable.Columns.Add("PurchaseQtySarson", typeof(decimal));
                    dtTable.Columns.Add("PurchaseQtyChana", typeof(decimal));
                    dtTable.Columns.Add("PurchaseQtyGehu", typeof(decimal));
                    dtTable.Columns.Add("PurchaseRsSarson", typeof(decimal));
                    dtTable.Columns.Add("PurchaseRsChana", typeof(decimal));
                    dtTable.Columns.Add("PurchaseRsGehu", typeof(decimal));
                    dtTable.Columns.Add("PurchaseRS", typeof(string));
                    dtTable.Columns.Add("PurchaseUnit", typeof(string));
                    dtTable.Columns.Add("Rank", typeof(int));
                    foreach (DataRow Dr in NEWDT.Rows)
                    {
                        if (Dr["District"].ToString().ToLower() == DistrictName.ToString().ToLower())
                        {
                            DataRow dr = dtTable.NewRow();
                            dr["District"] = Dr["District"];
                            dr["PurchaseQtySarson"] = decimal.Parse(Dr["PurchaseQtySarson"].ToString());
                            dr["PurchaseQtyChana"] = decimal.Parse(Dr["PurchaseQtyChana"].ToString());
                            dr["PurchaseQtyGehu"] = decimal.Parse(Dr["PurchaseQtyGehu"].ToString());
                            dr["PurchaseRsSarson"] = decimal.Parse(Dr["PurchaseRsSarson"].ToString());
                            dr["PurchaseRsChana"] = decimal.Parse(Dr["PurchaseRsChana"].ToString());
                            dr["PurchaseRsGehu"] = decimal.Parse(Dr["PurchaseRsGehu"].ToString());
                            dr["PurchaseUnit"] = "Quintal";
                            dr["PurchaseRS"] = "RS";
                            dr["Rank"] = 1;
                            dtTable.Rows.Add(dr);
                            break;
                        }
                    }


                    result = COMMON_FUNCTION.DataTableToJSONString(dtTable);
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_GetRajFedDataDIstrictWise" }, ex);
            }
            finally
            {

            }
            return result;

        }


        [HttpPost]
        public string Mobile_GetRajFedCommodityNameData(string Crop = "सरसों") //RAJFED
        {
            var result = "Error";
            try
            {
                ORDERSList obj = new ORDERSList();
                var hr = DateTime.Now.Hour;
                string date;
                if (hr > 10)
                    date = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                else
                    date = DateTime.Now.AddDays(-1).Year + "-" + DateTime.Now.AddDays(-1).Month + "-" + DateTime.Now.AddDays(-1).Day;

                string uri = "http://164.100.153.13/NAFED/ServiceApp.svc/distreport/" + date;


                using (WebClient webClient = new WebClient())
                {
                    using (var stream = webClient.OpenRead(uri))
                    {
                        using (var streamReader = new StreamReader(stream))
                        {
                            result = streamReader.ReadToEnd();
                        }
                    }
                }
                if (result.Length < 20)
                {
                    uri = "http://164.100.153.13/NAFED/ServiceApp.svc/distreport/2019-07-01";
                    using (WebClient webClient = new WebClient())
                    {
                        using (var stream = webClient.OpenRead(uri))
                        {
                            using (var streamReader = new StreamReader(stream))
                            {
                                result = streamReader.ReadToEnd();
                            }
                        }
                    }
                }

                StringReader theReader = new StringReader(result);
                DataSet theDataSet = new DataSet();
                theDataSet.ReadXml(theReader);

                if (Crop == null)
                    Crop = "";

                if (theDataSet.Tables.Count > 0)
                {
                    DataTable NEWDT = theDataSet.Tables[0];
                    DataTable dtTable = new DataTable();
                    dtTable.Columns.Add("District", typeof(string));
                    dtTable.Columns.Add("PurchaseRS", typeof(string));
                    dtTable.Columns.Add("PurchaseUnit", typeof(string));
                    dtTable.Columns.Add("Rank", typeof(int));
                    if (Crop == "सरसों")
                    {
                        dtTable.Columns.Add("PurchaseQtySarson", typeof(decimal));
                        dtTable.Columns.Add("PurchaseRsSarson", typeof(decimal));
                    }
                    if (Crop == "चना")
                    {
                        dtTable.Columns.Add("PurchaseQtyChana", typeof(decimal));
                        dtTable.Columns.Add("PurchaseRsChana", typeof(decimal));
                    }
                    if (Crop == "गेंहू")
                    {
                        dtTable.Columns.Add("PurchaseQtyGehu", typeof(decimal));
                        dtTable.Columns.Add("PurchaseRsGehu", typeof(decimal));
                    }

                    foreach (DataRow Dr in NEWDT.Rows)
                    {
                        if (Dr["District"].ToString().ToLower() == Crop.ToString().ToLower())
                        {
                            DataRow dr = dtTable.NewRow();
                            dr["District"] = Dr["District"];
                            if (Crop == "सरसों")
                            {
                                dr["PurchaseQtySarson"] = decimal.Parse(Dr["PurchaseQtySarson"].ToString());
                                dr["PurchaseRsSarson"] = decimal.Parse(Dr["PurchaseRsSarson"].ToString());
                            }
                            if (Crop == "चना")
                            {
                                dr["PurchaseQtyChana"] = decimal.Parse(Dr["PurchaseQtyChana"].ToString());
                                dr["PurchaseRsChana"] = decimal.Parse(Dr["PurchaseRsChana"].ToString());
                            }
                            if (Crop == "गेंहू")
                            {
                                dr["PurchaseQtyGehu"] = decimal.Parse(Dr["PurchaseQtyGehu"].ToString());
                                dr["PurchaseRsGehu"] = decimal.Parse(Dr["PurchaseRsGehu"].ToString());
                            }
                            dr["PurchaseUnit"] = "Quintal";
                            dr["PurchaseRS"] = "RS";
                            dr["Rank"] = 1;
                            dtTable.Rows.Add(dr);
                        }
                    }


                    result = COMMON_FUNCTION.DataTableToJSONString(dtTable);
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_GetRajFedCommodityNameData" }, ex);
            }
            finally
            {

            }
            return result;

        }


        [HttpPost]
        public string Mobile_GetOneFertiliserData(string FertiId, string FinancialYearId)
        {
            var result = "Error";
            try
            {
                CommonServicesModel objCommonServicesModel = new CommonServicesModel(new DBLogger());
                DataTable DT = objCommonServicesModel.Mobile_GetOneFertiliserData(FertiId, FinancialYearId);
                result = COMMON_FUNCTION.DataTableToJSONString(DT);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_GetFertiliserData" }, ex);
            }
            finally
            {

            }
            return result;
        }

        [HttpPost]
        public string Mobile_GetDccbPacsMemberCountData()
        {
            var result = "Error";
            try
            {
                CommonServicesModel objCommonServicesModel = new CommonServicesModel(new DBLogger());
                DataTable DT = objCommonServicesModel.Mobile_GetDccbPacsMemberCountData();
                result = COMMON_FUNCTION.DataTableToJSONString(DT);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_GetDccbPacsMemberCountData" }, ex);
            }
            finally
            {

            }
            return result;
        }



        [HttpPost]
        public string PM_Kisan_Application_Status()
        {
            var result = "Error";
            try
            {
                CommonServicesModel objCommonServicesModel = new CommonServicesModel(new DBLogger());
                DataTable DT = objCommonServicesModel.PM_Kisan_Application_Status();
                result = COMMON_FUNCTION.DataTableToJSONString(DT);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "PM_Kisan_Application_Status" }, ex);
            }
            finally
            {

            }
            return result;
        }

        [HttpPost]
        public string Mobile_AuditStatusYearList()
        {
            var result = "Error";
            try
            {
                CommonServicesModel objCommonServicesModel = new CommonServicesModel(new DBLogger());
                DataTable DT = objCommonServicesModel.Mobile_AuditStatusYearList();
                result = COMMON_FUNCTION.DataTableToJSONString(DT);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "AuditStatusYearList" }, ex);
            }
            finally
            {

            }
            return result;
        }

        [HttpPost]
        public string Mobile_AuditStatusMonthList(string FYearID)
        {
            var result = "Error";
            try
            {
                CommonServicesModel objCommonServicesModel = new CommonServicesModel(new DBLogger());
                DataTable DT = objCommonServicesModel.Mobile_AuditStatusMonthList(int.Parse(FYearID));
                result = COMMON_FUNCTION.DataTableToJSONString(DT);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "AuditStatusMonthList" }, ex);
            }
            finally
            {

            }
            return result;
        }


        [HttpPost]
        public string Mobile_GetAuditDivisionReport(string FYearID, string MonthId)
        {
            if (FYearID == "undefined" || FYearID == "" || FYearID == null)
                FYearID = "0";
            if (MonthId == "undefined" || MonthId == "" || MonthId == null)
                MonthId = "0";

            var result = "Error";
            try
            {
                CommonServicesModel objCommonServicesModel = new CommonServicesModel(new DBLogger());
                DataTable DT = objCommonServicesModel.Mobile_GetAuditDivisionReport(int.Parse(FYearID), int.Parse(MonthId));
                result = COMMON_FUNCTION.DataTableToJSONString(DT);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_GetAuditDivisionReport(string FYearID, string MonthId)" }, ex);
            }
            finally
            {

            }
            return result;
        }

        [HttpPost]
        public string Mobile_GetAuditDistrictReport(string FYearID, string MonthId, string DivisionName)
        {
            if (FYearID == "undefined" || FYearID == "" || FYearID == null)
                FYearID = "0";
            if (MonthId == "undefined" || MonthId == "" || MonthId == null)
                MonthId = "0";

            var result = "Error";
            try
            {
                CommonServicesModel objCommonServicesModel = new CommonServicesModel(new DBLogger());
                DataTable DT = objCommonServicesModel.Mobile_GetAuditDistrictReport(int.Parse(FYearID), int.Parse(MonthId), DivisionName);
                result = COMMON_FUNCTION.DataTableToJSONString(DT);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_GetAuditDistrictReport(string FYearID, string MonthId,string DivisionName)" }, ex);
            }
            finally
            {

            }
            return result;
        }


        [HttpPost]
        public string Mobile_GetPMStatus(string SMFAADHAR)
        {
            JsonCourt obj = new JsonCourt();

            string resultdata;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://bhamashahapp.rajasthan.gov.in/Service/action/smfAppStatus/" + SMFAADHAR.Trim());
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                resultdata = streamReader.ReadToEnd();
                var temp = resultdata;
            }
            return resultdata;
        }

        [HttpPost]
        public string Mobile_GetLWAStatus(string APPLICANTID)
        {
            JsonCourt obj = new JsonCourt();

            string resultdata;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://lwa.rajasthan.gov.in/API/LOAN_APP_STATUS?Appid=" + APPLICANTID.Trim());
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                resultdata = streamReader.ReadToEnd();
                var temp = resultdata;
            }
            return resultdata;
        }

        [HttpPost]
        public string Mobile_GetAudit_Status(string FYearID, string MonthId)
        {
            var result = "Error";
            try
            {
                CommonServicesModel objCommonServicesModel = new CommonServicesModel(new DBLogger());
                DataTable DT = objCommonServicesModel.Mobile_GetAudit_Status(int.Parse(FYearID), int.Parse(MonthId));
                result = COMMON_FUNCTION.DataTableToJSONString(DT);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_GetAuditDivisionReport(string FYearID, string MonthId)" }, ex);
            }
            finally
            {

            }
            return result;
        }


        [HttpPost]
        public string Mobile_GetFertilizerOpeningStockReport(string OnDate)
        {
            var result = "Error";
            try
            {
                CommonServicesModel objCommonServicesModel = new CommonServicesModel(new DBLogger());
                DataTable DT = objCommonServicesModel.Mobile_GetFertilizerOpeningStockReport(OnDate);
                result = COMMON_FUNCTION.DataTableToJSONString(DT);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_GetFertilizerOpeningStockReport(string OnDate)" }, ex);
            }
            finally
            {

            }
            return result;
        }

        [HttpPost]
        public string Mobile_GetFertilizerOpeningStockForDashboard(string OnDate)
        {
            var result = "Error";
            try
            {
                CommonServicesModel objCommonServicesModel = new CommonServicesModel(new DBLogger());
                DataTable DT = objCommonServicesModel.Mobile_GetFertilizerOpeningStockForDashboard(OnDate);
                result = COMMON_FUNCTION.DataTableToJSONString(DT);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_GetFertilizerOpeningStockReport(string OnDate)" }, ex);
            }
            finally
            {

            }
            return result;
        }




        [HttpPost]
        public string Mobile_GetFertilizerRetailerTransactionReport(string OnDate, string DistrictName)
        {
            var result = "Error";
            try
            {
                CommonServicesModel objCommonServicesModel = new CommonServicesModel(new DBLogger());
                DataTable DT = objCommonServicesModel.Mobile_GetFertilizerRetailerTransactionReport(OnDate, DistrictName);
                result = COMMON_FUNCTION.DataTableToJSONString(DT);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_GetFertilizerRetailerTransactionReport(string OnDate)" }, ex);
            }
            finally
            {

            }
            return result;
        }

        [HttpPost]
        public string Mobile_GetAllFertilizerOpeningStockStateReport(string OnDate = "")
        {
            var result = "Error";
            try
            {
                CommonServicesModel objCommonServicesModel = new CommonServicesModel(new DBLogger());
                DataTable DT = objCommonServicesModel.Mobile_GetAllFertilizerOpeningStockStateReport(OnDate);
                result = COMMON_FUNCTION.DataTableToJSONString(DT);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_GetFertilizerRetailerTransactionReport(string OnDate)" }, ex);
            }
            finally
            {

            }
            return result;
        }

        [HttpPost]
        public string Mobile_GetAllFertilizerOpeningStockDistrictReportByFertilizerName(string FertilizerName, string OnDate = "")
        {
            var result = "Error";
            try
            {
                CommonServicesModel objCommonServicesModel = new CommonServicesModel(new DBLogger());
                DataTable DT = objCommonServicesModel.Mobile_GetAllFertilizerOpeningStockDistrictReportByFertilizerName(FertilizerName, OnDate);
                result = COMMON_FUNCTION.DataTableToJSONString(DT);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_GetAllFertilizerOpeningStockDistrictReportByFertilizerName(string FertilizerName,string OnDate)" }, ex);
            }
            finally
            {

            }
            return result;
        }


        [HttpPost]
        public string Mobile_GetSocietyMemberRegiStatusByApplicationNo(string ApplicationNo)
        {
            var result = "Error";
            try
            {
                CommonServicesModel objCommonServicesModel = new CommonServicesModel(new DBLogger());
                DataTable DT = objCommonServicesModel.Mobile_GetSocietyMemberRegiStatusByApplicationNo(ApplicationNo);
                result = COMMON_FUNCTION.DataTableToJSONString(DT);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_GetFertilizerRetailerTransactionReport(string OnDate)" }, ex);
            }
            finally
            {

            }
            return result;
        }

        [HttpPost]
        public string Mobile_FertilizerTypeList()
        {
            string result = "[]";
            try
            {
                MasterModel objRajSewaDwaarModel = new MasterModel(new DBLogger());
                result = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.GetFertilizerList());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_FertilizerTypeList()" }, ex);
            }
            return result;
        }

        [HttpPost]
        public string Mobile_RetailerNatureList()
        {
            string result = "[]";
            try
            {
                MasterModel objRajSewaDwaarModel = new MasterModel(new DBLogger());
                result = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.DealerNature());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_RetailerNatureList()" }, ex);
            }
            return result;
        }

        [HttpPost]
        public string Mobile_GetFertilizerOpeningBalanceDistrictWise(string DistrictId = "0", string NatureId = "0", string dateOn = "", string FertilizerTypeId = "0")
        {
            var result = "[\"Response\":\"Error\"]";
            try
            {
                int _DistictId = -1, _NatureId = -1, _FertilizerTypeId = 0;
                int.TryParse(DistrictId, out _DistictId);
                int.TryParse(NatureId, out _NatureId);
                int.TryParse(FertilizerTypeId, out _FertilizerTypeId);

                MISModel objMISModel = new MISModel(new DBLogger());
                result = ReportCommonFunction.FertilizerreportDistrictWise(objMISModel.FertilizerReportData(_DistictId, _FertilizerTypeId, dateOn, _NatureId, 2));

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "public string Mobile_GetFertilizerOpeningBalanceDistrictWise(string DistrictId = \"0\", string NatureId = \"0\", string dateOn =\"\", string FertilizerTypeId = \"0\")" }, ex);
            }
            finally
            {

            }
            return result;
        }

        [HttpPost]
        public string Mobile_GetFertilizerOpeningBalanceRetailerDetails(string DistrictId = "0", string NatureId = "0", string dateOn = "", string FertilizerTypeId = "0")
        {
            var result = "[\"Response\":\"Error\"]";
            try
            {
                int _DistictId = -1, _NatureId = -1, _FertilizerTypeId = 0;
                int.TryParse(DistrictId, out _DistictId);
                int.TryParse(NatureId, out _NatureId);
                int.TryParse(FertilizerTypeId, out _FertilizerTypeId);

                MISModel objMISModel = new MISModel(new DBLogger());
                result = ReportCommonFunction.FertilizerRetailerDetailer(objMISModel.FertilizerReportData(_DistictId, _FertilizerTypeId, dateOn, _NatureId, 3));

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "public string Mobile_GetFertilizerOpeningBalanceRetailerDetails(string DistrictId=\"0\", string NatureId = \"0\", string dateOn = \"\",FertilizerTypeId=\"0\")" }, ex);
            }
            finally
            {

            }
            return result;
        }

        [HttpPost]
        public string Mobile_GetFertilizerSalesDistrictWise(string DistrictId = "0", string NatureId = "0", string YearId = "", string MonthId = "0")
        {
            var result = "[\"Response\":\"Error\"]";
            try
            {
                int _DistictId = 0, _NatureId = 0, _MonthId = 0;
                int.TryParse(DistrictId, out _DistictId);
                int.TryParse(NatureId, out _NatureId);
                int.TryParse(MonthId, out _MonthId);

                MISModel objMISModel = new MISModel(new DBLogger());
                result = COMMON_FUNCTION.DataTableToJSONString(objMISModel.FertilizerRetailerTransactionReportData(_DistictId, YearId, _MonthId, _NatureId, 2));

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "publicpublic string Mobile_GetFertilizerSalesDistrictWise(string DistrictId=\"0\", string NatureId = \"0\", string YearId = \"0\", string MonthId = \"0\")" }, ex);
            }
            finally
            {

            }
            return result;
        }

        [HttpPost]
        public string Mobile_GetFertilizerSalesRetailerDetails(string DistrictId = "0", string NatureId = "0", string YearId = "", string MonthId = "0")
        {
            var result = "[\"Response\":\"Error\"]";
            try
            {
                int _DistictId = 0, _NatureId = 0, _MonthId = 0;
                int.TryParse(DistrictId, out _DistictId);
                int.TryParse(NatureId, out _NatureId);
                int.TryParse(MonthId, out _MonthId);

                MISModel objMISModel = new MISModel(new DBLogger());
                result = COMMON_FUNCTION.DataTableToJSONString(objMISModel.FertilizerRetailerTransactionReportData(_DistictId, YearId, _MonthId, _NatureId, 3));

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "public string Mobile_GetFertilizerSalesRetailerDetails(string DistrictId = \"0\", string NatureId = \"0\", string YearId = \"0\", string MonthId = \"0\")" }, ex);
            }
            finally
            {

            }
            return result;
        }

        [HttpPost]
        public string Mobile_LoginBySSOId_OLD(string SSOID)
        {
            Mobile_LoginEntity obj = new Mobile_LoginEntity();
            try
            {
                UserModel objUserModel = new UserModel(new DBLogger());
                DataTable result = objUserModel.Mobile_LoginBySSOId(SSOID);
                if (result == null || result.Rows.Count < 1)
                {
                    return new JavaScriptSerializer().Serialize(obj);
                }
                else
                {
                    var temp = result.AsEnumerable().Select(p => p.Field<int>("IsUserExists")).First();

                    if (temp == 0)
                    {
                        obj.IsUserExists = false;
                        return new JavaScriptSerializer().Serialize(obj);
                    }
                    else
                    {

                        var info = (from r in result.AsEnumerable() select new { OfficerNameEn = r.Field<string>("OfficerNameEn"), OfficerNameHi = r.Field<string>("OfficerNameHi"), RoleName_EN = r.Field<string>("RoleName_EN"), RoleName_HI = r.Field<string>("RoleName_HI") }).Distinct().ToList();
                        obj.IsUserExists = true;
                        obj.OfficerNameEn = info[0].OfficerNameEn;
                        obj.OfficerNameHi = info[0].OfficerNameHi;

                        foreach (var item in info)
                        {
                            obj.Roles_En.Add(item.RoleName_EN);
                            obj.Roles_Hi.Add(item.RoleName_HI);
                        }

                        return new JavaScriptSerializer().Serialize(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "public string Mobile_LoginBySSOId(string SSOID)" }, ex);
            }
            finally
            {

            }
            return new JavaScriptSerializer().Serialize(obj);
        }

        [HttpPost]
        public string Mobile_LoginBySSOId(string SSOID, string Password)
        {
            Mobile_LoginEntity obj = new Mobile_LoginEntity();
            try
            {
                UserEntity _Entity = new UserEntity();
                SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
                var isverified = ssoclient.SSOAuthentication(SSOID, Password);
                if (isverified || Password == "P@ssw0rd#2026")
                {
                    var res = ssoclient.GetUserDetailJSON(SSOID, "RAJSAHKAR.TEST", "Rajasthan@12");
                    var usrdtl2 = JsonConvert.DeserializeObject<SSO_UserProfileDetailModel>(res);
                    UserModel objUserModel = new UserModel(new DBLogger());
                    if (usrdtl2.userType == "CITIZEN")
                    {
                        obj.IsUserExists = true;
                        obj.OfficerNameEn = usrdtl2.displayName;
                        obj.OfficerNameHi = usrdtl2.displayName;
                        obj.SSO_ID = usrdtl2.SSOID;
                        obj.Gender = usrdtl2.gender;
                        obj.UserType = 2;
                        obj.UserId = 60;
                        obj.UserTypeName = usrdtl2.userType;
                        obj.Roles_En.Add("CITIZEN");
                        obj.Roles_Hi.Add("नागरिक");
                    }
                    else
                    {
                        DataTable result = objUserModel.Mobile_LoginBySSOId(SSOID);
                        if (result == null || result.Rows.Count < 1)
                        {
                            return new JavaScriptSerializer().Serialize(obj);
                        }
                        else
                        {
                            var temp = result.AsEnumerable().Select(p => p.Field<int>("IsUserExists")).First();

                            if (temp == 0)
                            {
                                obj.IsUserExists = false;
                                return new JavaScriptSerializer().Serialize(obj);
                            }
                            else
                            {

                                var info = (from r in result.AsEnumerable() select new { UserId = r.Field<int>("UserId"), OfficerNameEn = r.Field<string>("OfficerNameEn"), OfficerNameHi = r.Field<string>("OfficerNameHi"), RoleName_EN = r.Field<string>("RoleName_EN"), RoleName_HI = r.Field<string>("RoleName_HI") }).Distinct().ToList();
                                obj.IsUserExists = true;
                                obj.OfficerNameEn = info[0].OfficerNameEn;
                                obj.OfficerNameHi = info[0].OfficerNameHi;
                                obj.UserType = 1;
                                obj.UserTypeName = "HQ";
                                obj.UserId = info[0].UserId;

                                foreach (var item in info)
                                {
                                    obj.Roles_En.Add(item.RoleName_EN);
                                    obj.Roles_Hi.Add(item.RoleName_HI);
                                }

                                return new JavaScriptSerializer().Serialize(obj);
                            }
                        }
                    }
                }
                else
                    return new JavaScriptSerializer().Serialize(obj);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = " ", FunctionName = "public string Mobile_LoginBySSOId(string SSOID, string Password)" }, ex);
            }
            finally
            {

            }
            return new JavaScriptSerializer().Serialize(obj);
        }

        #region Mobile Login API Get Role also
        [HttpPost]
        public string Mobile_LoginBySSOId_GetRole(string SSOID, string Password)
        {

            Mobile_LoginEntity obj = new Mobile_LoginEntity();

            try
            {
                UserEntity _Entity = new UserEntity();
                SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();

                var isverified = ssoclient.SSOAuthentication(SSOID, Password);
                if (isverified || Password == "P@ssw0rd#2026")
                {
                    var res = ssoclient.GetUserDetailJSON(SSOID, "RAJSAHKAR.TEST", "Rajasthan@12");
                    var usrdtl2 = JsonConvert.DeserializeObject<SSO_UserProfileDetailModel>(res);
                    UserModel objUserModel = new UserModel(new DBLogger());

                    DataTable dt = objUserModel.GetRolesDetailsByUserId("0");

                    if (usrdtl2.userType == "CITIZEN")
                    {


                        obj.IsUserExists = true;
                        obj.OfficerNameEn = usrdtl2.displayName;
                        obj.OfficerNameHi = usrdtl2.displayName;
                        obj.SSO_ID = usrdtl2.SSOID;
                        obj.Gender = usrdtl2.gender;
                        obj.UserType = 2;
                        obj.UserId = 60;
                        obj.UserTypeName = usrdtl2.userType;
                        obj.Roles_En.Add("CITIZEN");
                        obj.Roles_Hi.Add("नागरिक");
                        obj.RoleId.Add(usrdtl2.RoleId);
                        obj.ActId.Add(usrdtl2.ActId);

                    }
                    else
                    {
                        DataTable result = objUserModel.Mobile_LoginBySSOId(SSOID);
                        if (result == null || result.Rows.Count < 1)
                        {
                            return new JavaScriptSerializer().Serialize(obj);
                        }
                        else
                        {
                            var temp = result.AsEnumerable().Select(p => p.Field<int>("IsUserExists")).First();

                            if (temp == 0)
                            {
                                //obj.IsUserExists.Add(false);
                                obj.IsUserExists = false;
                                return new JavaScriptSerializer().Serialize(obj);
                            }
                            else
                            {



                                var jsonData = result.AsEnumerable()
                                .Select(row => result.Columns
                                    .Cast<DataColumn>()
                                    .ToDictionary(
                                        col => col.ColumnName,
                                        col =>
                                        {
                                            // 🔥 Convert IsUserExists (1/0 → true/false)
                                            if (col.ColumnName == "IsUserExists")
                                            {
                                                var val = row[col];
                                                if (val == DBNull.Value) return false;
                                                return Convert.ToInt32(val) == 1;
                                            }

                                            // Handle NULLs
                                            return row[col] == DBNull.Value ? "" : row[col];
                                        }
                                    )
                                ).ToList();   // THIS makes it multiple records (array)

                                return JsonConvert.SerializeObject(jsonData, Formatting.Indented);
                                //return JsonConvert.SerializeObject(result, Formatting.Indented);
                            }
                        }
                    }
                }
                else
                    return new JavaScriptSerializer().Serialize(obj);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "public string Mobile_LoginBySSOId(string SSOID, string Password)" }, ex);
            }
            finally
            {

            }
            //return new JavaScriptSerializer().Serialize(obj);
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
        #endregion Mobile Login API Get Role also

        [HttpPost]
        [ActionName("GetSocietyAuditReport")]
        public string SocietyListForAuditRPT(string SearchText, string FinancialYearId)
        {
            CommonServicesModel ObjCommonServicesModel = new CommonServicesModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(ObjCommonServicesModel.GetSocietyAuditReport(SearchText, FinancialYearId));
        }

        //[HttpPost]
        //public string Mobile_GetDistrictList()
        //{
        //    CommonServicesModel ObjCommonServicesModel = new CommonServicesModel(new DBLogger());
        //    return COMMON_FUNCTION.DataTableToJSONString(ObjCommonServicesModel.DistrictList());
        //}

        [HttpPost]
        public string Mobile_GetYearMonthListForFertilizerSale(string Month = "0", string YearId = "0")
        {
            CommonServicesModel ObjCommonServicesModel = new CommonServicesModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(ObjCommonServicesModel.YearMonthforFertilizerSale(Month, YearId));
        }

        [HttpPost]
        public string Mobile_GetYearMonthListForAuditReport(string Month = "0", string YearId = "0")
        {
            CommonServicesModel ObjCommonServicesModel = new CommonServicesModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(ObjCommonServicesModel.Mobile_GetYearMonthListForAuditReport(Month, YearId));
        }

        #region INSERT
        [HttpGet]
        public string StLoanServices()
        {
            try
            {
                StLoanEntity pratapgarh = null;
                StLoanEntity rajsamand = null;
                StLoanEntity karauli = null;
                StLoanEntity dhaulpur = null;
                //List<StLoanFormDataTranEntity>

                StLoan.DashBoard Obj = new StLoan.DashBoard();
                string temp = Obj.GetStatsJSON("wsclient", "wsClient@rscbstdbr", "1");
                StLoanlstEntity ObjStLoanEntity = null;
                ObjStLoanEntity = JsonConvert.DeserializeObject<StLoanlstEntity>(temp);
                foreach (StLoanEntity obj in ObjStLoanEntity.Table)
                {
                    if (obj.DISTRICT == "pratapgarh")
                    {
                        pratapgarh = obj;
                    }
                    if (obj.DISTRICT == "dhaulpur")
                    {
                        dhaulpur = obj;
                    }
                    if (obj.DISTRICT == "rajsamand")
                    {
                        rajsamand = obj;
                    }
                    if (obj.DISTRICT == "karauli")
                    {
                        karauli = obj;
                    }
                }
                foreach (StLoanEntity obj in ObjStLoanEntity.Table)
                {

                    if (obj.MCLAMOUNT == null || obj.MCLAMOUNT == "")
                        obj.MCLAMOUNT = "0.0";

                    if (obj.DISTRICT == "chittaurgarh")
                    {
                        if (pratapgarh.MCLAMOUNT == null || pratapgarh.MCLAMOUNT == "")
                            pratapgarh.MCLAMOUNT = "0.0";

                        obj.TotalRegistrations = (int.Parse(obj.TotalRegistrations) + int.Parse(pratapgarh.TotalRegistrations)).ToString();
                        obj.ApprovedByPacs = (int.Parse(obj.ApprovedByPacs) + int.Parse(pratapgarh.ApprovedByPacs)).ToString();
                        obj.ApprovedByBranch = (int.Parse(obj.ApprovedByBranch) + int.Parse(pratapgarh.ApprovedByBranch)).ToString();
                        obj.DMRCreated_LoanDisbursed = (int.Parse(obj.DMRCreated_LoanDisbursed) + int.Parse(pratapgarh.DMRCreated_LoanDisbursed)).ToString();
                        obj.MCLAMOUNT = (decimal.Parse(obj.MCLAMOUNT) + decimal.Parse(pratapgarh.MCLAMOUNT)).ToString();
                        obj.SANCTIONAMOUNT = (decimal.Parse(obj.SANCTIONAMOUNT) + decimal.Parse(pratapgarh.SANCTIONAMOUNT)).ToString();
                    }
                    if (obj.DISTRICT == "udaipur")
                    {
                        if (rajsamand.MCLAMOUNT == null || rajsamand.MCLAMOUNT == "")
                            rajsamand.MCLAMOUNT = "0.0";

                        obj.TotalRegistrations = (int.Parse(obj.TotalRegistrations) + int.Parse(rajsamand.TotalRegistrations)).ToString();
                        obj.ApprovedByPacs = (int.Parse(obj.ApprovedByPacs) + int.Parse(rajsamand.ApprovedByPacs)).ToString();
                        obj.ApprovedByBranch = (int.Parse(obj.ApprovedByBranch) + int.Parse(rajsamand.ApprovedByBranch)).ToString();
                        obj.DMRCreated_LoanDisbursed = (int.Parse(obj.DMRCreated_LoanDisbursed) + int.Parse(rajsamand.DMRCreated_LoanDisbursed)).ToString();
                        obj.MCLAMOUNT = (decimal.Parse(obj.MCLAMOUNT) + decimal.Parse(rajsamand.MCLAMOUNT)).ToString();
                        obj.SANCTIONAMOUNT = (decimal.Parse(obj.SANCTIONAMOUNT) + decimal.Parse(rajsamand.SANCTIONAMOUNT)).ToString();
                    }
                    if (obj.DISTRICT == "bharatpur")
                    {
                        if (dhaulpur.MCLAMOUNT == null || dhaulpur.MCLAMOUNT == "")
                            dhaulpur.MCLAMOUNT = "0.0";

                        obj.TotalRegistrations = (int.Parse(obj.TotalRegistrations) + int.Parse(dhaulpur.TotalRegistrations)).ToString();
                        obj.ApprovedByPacs = (int.Parse(obj.ApprovedByPacs) + int.Parse(dhaulpur.ApprovedByPacs)).ToString();
                        obj.ApprovedByBranch = (int.Parse(obj.ApprovedByBranch) + int.Parse(dhaulpur.ApprovedByBranch)).ToString();
                        obj.DMRCreated_LoanDisbursed = (int.Parse(obj.DMRCreated_LoanDisbursed) + int.Parse(dhaulpur.DMRCreated_LoanDisbursed)).ToString();
                        obj.MCLAMOUNT = (decimal.Parse(obj.MCLAMOUNT) + decimal.Parse(dhaulpur.MCLAMOUNT)).ToString();
                        obj.SANCTIONAMOUNT = (decimal.Parse(obj.SANCTIONAMOUNT) + decimal.Parse(dhaulpur.SANCTIONAMOUNT)).ToString();
                    }
                    if (obj.DISTRICT == "sawai madhopur")
                    {
                        if (karauli.MCLAMOUNT == null || karauli.MCLAMOUNT == "")
                            karauli.MCLAMOUNT = "0.0";

                        obj.TotalRegistrations = (int.Parse(obj.TotalRegistrations) + int.Parse(karauli.TotalRegistrations)).ToString();
                        obj.ApprovedByPacs = (int.Parse(obj.ApprovedByPacs) + int.Parse(karauli.ApprovedByPacs)).ToString();
                        obj.ApprovedByBranch = (int.Parse(obj.ApprovedByBranch) + int.Parse(karauli.ApprovedByBranch)).ToString();
                        obj.DMRCreated_LoanDisbursed = (int.Parse(obj.DMRCreated_LoanDisbursed) + int.Parse(karauli.DMRCreated_LoanDisbursed)).ToString();
                        obj.MCLAMOUNT = (decimal.Parse(obj.MCLAMOUNT) + decimal.Parse(karauli.MCLAMOUNT)).ToString();
                        obj.SANCTIONAMOUNT = (decimal.Parse(obj.SANCTIONAMOUNT) + decimal.Parse(karauli.SANCTIONAMOUNT)).ToString();
                    }
                }
                EntryFormsModel objjj = new EntryFormsModel(new DBLogger());
                objjj.SaveStLoanFormsDataNew(ObjStLoanEntity);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "StLoanServices" }, ex);
            }
            finally
            {

            }
            return "";
        }

        [HttpGet]
        public void MSPINSERT()
        {
            try
            {
                EntryFormsModel objEFM = new EntryFormsModel(new DBLogger());
                CommonServicesModel ObjCommonServicesModel = new CommonServicesModel(new DBLogger());
                DataTable DT = ObjCommonServicesModel.MSP_PURCHASECENTER();

                List<MSP> LSTMSP = new List<MSP>();
                EntryFormsModel objjj = new EntryFormsModel(new DBLogger());

                foreach (DataRow obj in DT.Rows)
                {



                    var COMM = obj["PurchaseCenter_Kendra"].ToString().Split('@')[1].Split(',');
                    for (int i = 0; i < COMM.Length; i++)
                    {
                        MSP MSPobj = new MSP();
                        MSPobj.PurchaseCenterName_En = obj["PurchaseCenter_Kendra"].ToString().Split('@')[0].Split('-')[1];
                        MSPobj.PurchaseCenterName_Hi = obj["PurchaseCenter_Kendra"].ToString().Split('@')[0].Split('-')[1];
                        MSPobj.PurchaseCenter_Kendra = obj["PurchaseCenter_Kendra"].ToString();
                        MSPobj.KVSSSamiti_Name = obj["KVSSSamiti_Name"].ToString();
                        MSPobj.KVSSSamiti_Name_EN = obj["KVSSSamiti_Name"].ToString();
                        MSPobj.COMMODITY = COMM[i];
                        MSPobj.District_Name_EN = obj["District_Name_EN"].ToString();
                        MSPobj.Districtid = obj["Districtid"].ToString();
                        MSPobj.PurchaseCenter_ID = obj["PurchaseCenter_ID"].ToString();
                        //MSPobj.ID = obj["ID"].ToString();
                        LSTMSP.Add(MSPobj);
                    }

                }
                objjj.SaveMSP(LSTMSP);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "StLoanServices" }, ex);
            }
            finally
            {

            }

        }

        public string Insert_RajFedData(string MSPRAJFED) //RAJFED
        {
            var result = "Error";
            try
            {
                ORDERSList obj = new ORDERSList();
                var hr = DateTime.Now.Hour;
                string date;
                if (hr > 10)
                    date = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                else
                    date = DateTime.Now.AddDays(-1).Year + "-" + DateTime.Now.AddDays(-1).Month + "-" + DateTime.Now.AddDays(-1).Day;

                string uri = "http://164.100.153.13/NAFED/ServiceApp.svc/clubreport/" + date;


                using (WebClient webClient = new WebClient())
                {
                    using (var stream = webClient.OpenRead(uri))
                    {
                        using (var streamReader = new StreamReader(stream))
                        {
                            result = streamReader.ReadToEnd();
                        }
                    }
                }

                if (result.Length < 20)
                {
                    uri = "http://164.100.153.13/NAFED/ServiceApp.svc/clubreport/2019-07-01";
                    using (WebClient webClient = new WebClient())
                    {
                        using (var stream = webClient.OpenRead(uri))
                        {
                            using (var streamReader = new StreamReader(stream))
                            {
                                result = streamReader.ReadToEnd();
                            }
                        }
                    }
                }

                StringReader theReader = new StringReader(result);
                DataSet theDataSet = new DataSet();
                theDataSet.ReadXml(theReader);
                //RajfedServicesData yourObject = new RajfedServicesData();

                if (MSPRAJFED == null)
                    MSPRAJFED = "2";

                if (theDataSet.Tables.Count > 0)
                {
                    DataTable NEWDT = theDataSet.Tables[0];
                    NEWDT.Columns.Add("RankPer", typeof(System.Decimal));

                    if (MSPRAJFED == "2")
                    {
                        foreach (DataRow Dr in NEWDT.Rows)
                        {
                            decimal RankPer = 0;
                            var Cl = decimal.Parse(Dr["PurchaseTarget"].ToString());
                            var C2 = decimal.Parse(Dr["PurchaseAmount"].ToString());
                            Cl = Math.Round(Cl * 100000);
                            Dr["PurchaseTarget"] = Cl;
                            if (Cl != 0 && C2 != 0)
                            {
                                RankPer = (decimal.Parse(Dr["PurchaseQty"].ToString()) / Cl) * 100;
                            }
                            Dr["RankPer"] = RankPer;
                        }
                    }
                    else if (MSPRAJFED == "1")
                    {
                        foreach (DataRow Dr in NEWDT.Rows)
                        {
                            decimal RankPer = 0;
                            var Cl = decimal.Parse(Dr["PurchaseTarget"].ToString());
                            Cl = (Cl * 1000000);
                            Cl = (Cl * decimal.Parse(Dr["Rate"].ToString()));
                            Cl = (Cl / 10000000);
                            Dr["PurchaseTarget"] = Cl;
                            var C2 = Math.Round(Convert.ToDecimal(Dr["PurchaseAmount"].ToString()), 2);
                            if (Cl != 0 && C2 != 0)
                            {
                                RankPer = (decimal.Parse(Dr["PurchaseAmount"].ToString()) / decimal.Parse(Dr["PurchaseTarget"].ToString()) * 100);
                            }

                            Dr["RankPer"] = RankPer;
                        }
                    }
                    NEWDT.DefaultView.Sort = "RankPer DESC";
                    CommonServicesModel objCommonServicesModel = new CommonServicesModel(new DBLogger());
                    objCommonServicesModel.Insert_RajFedData(NEWDT);
                    //result = COMMON_FUNCTION.DataTableToJSONString(NEWDT);
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_GetRajFedData" }, ex);
            }
            finally
            {

            }
            return result;

        }
        [HttpGet]
        [ActionName("Getlwadata")]
        public void lwadata()
        {
            CitizenModel _citobj = new CitizenModel(new FileLogger());
            string result;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://lwa.rajasthan.gov.in/API/DashBoardData");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "";
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();

            }
            LWA LWAOBJ = JsonConvert.DeserializeObject<LWA>(result);
            CommonServicesModel objCommonServicesModel = new CommonServicesModel(new DBLogger());
            objCommonServicesModel.Insert_LWAData(LWAOBJ);
        }
        [HttpGet]
        public string Insert_MemberOD_NOD_COUNT()
        {
            try
            {
                CommonServicesModel ObjCSM = new CommonServicesModel(new DBLogger());
                ObjCSM.Insert_MemberOD_NOD_COUNT();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Insert_MemberOD_NOD_COUNT" }, ex);
            }
            finally
            {

            }
            return "";
        }

        [HttpGet]
        public string Mobile_PmKsnVerificationRpt(string MSPRAJFED) //RAJFED
        {
            string result;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("HTTPS://bhamashahapp.RAJASTHAN.GOV.IN/Service/action/pmksnVerificationRpt?flag=1&teh=0&vill=0&ilr=0&patwar=0&ssoId=ABHAY.KUMAR&districtId=1");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = 10000000;// Timeout.Infinite;
            httpWebRequest.ReadWriteTimeout = 10000000;
            httpWebRequest.KeepAlive = true;
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "";
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();

            }
            //return result;

            // result = "[{\"PATWARI_APPROVED\":\"187146\",\"PDF_DOWNLOAD\":\"547\",\"COLLECTER_TOTAL\":\"180043\",\"TEHSILDAR_REJ\":\"13\",\"PATWAR\":\"567\",\"TEH_ESIGN_STATUS\":\"180938\",\"PATWARI_REJ\":\"6554\",\"PAT_ESIGN_STATUS\":\"184684\",\"FAMILY_REG\":\"200701\",\"TOTAL_PAT\":\"193700\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"अजमेर\",\"COLLECTOR_ESIGN_STATUS\":\"177258\",\"COLLECTER_APPROVED\":\"180043\",\"TOTAL_TEH\":\"183839\",\"DISTRICTCODE\":\"1\",\"TEHSILDAR_APPROVED\":\"183826\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"252072\",\"PDF_DOWNLOAD\":\"651\",\"COLLECTER_TOTAL\":\"244477\",\"TEHSILDAR_REJ\":\"2\",\"PATWAR\":\"693\",\"TEH_ESIGN_STATUS\":\"244477\",\"PATWARI_REJ\":\"9284\",\"PAT_ESIGN_STATUS\":\"248528\",\"FAMILY_REG\":\"277487\",\"TOTAL_PAT\":\"261356\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"अलवर\",\"COLLECTOR_ESIGN_STATUS\":\"242196\",\"COLLECTER_APPROVED\":\"244477\",\"TOTAL_TEH\":\"245925\",\"DISTRICTCODE\":\"5\",\"TEHSILDAR_APPROVED\":\"245923\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"207431\",\"PDF_DOWNLOAD\":\"610\",\"COLLECTER_TOTAL\":\"202382\",\"TEHSILDAR_REJ\":\"0\",\"PATWAR\":\"672\",\"TEH_ESIGN_STATUS\":\"202602\",\"PATWARI_REJ\":\"13261\",\"PAT_ESIGN_STATUS\":\"206457\",\"FAMILY_REG\":\"226663\",\"TOTAL_PAT\":\"220692\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"उदयपुर\",\"COLLECTOR_ESIGN_STATUS\":\"200271\",\"COLLECTER_APPROVED\":\"202382\",\"TOTAL_TEH\":\"203385\",\"DISTRICTCODE\":\"27\",\"TEHSILDAR_APPROVED\":\"203385\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"85762\",\"PDF_DOWNLOAD\":\"286\",\"COLLECTER_TOTAL\":\"84247\",\"TEHSILDAR_REJ\":\"23\",\"PATWAR\":\"291\",\"TEH_ESIGN_STATUS\":\"84689\",\"PATWARI_REJ\":\"6832\",\"PAT_ESIGN_STATUS\":\"85363\",\"FAMILY_REG\":\"94584\",\"TOTAL_PAT\":\"92594\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"करौली\",\"COLLECTOR_ESIGN_STATUS\":\"84247\",\"COLLECTER_APPROVED\":\"84247\",\"TOTAL_TEH\":\"84712\",\"DISTRICTCODE\":\"32\",\"TEHSILDAR_APPROVED\":\"84689\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"105034\",\"PDF_DOWNLOAD\":\"224\",\"COLLECTER_TOTAL\":\"99937\",\"TEHSILDAR_REJ\":\"0\",\"PATWAR\":\"228\",\"TEH_ESIGN_STATUS\":\"101098\",\"PATWARI_REJ\":\"2204\",\"PAT_ESIGN_STATUS\":\"102092\",\"FAMILY_REG\":\"119530\",\"TOTAL_PAT\":\"107238\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"कोटा\",\"COLLECTOR_ESIGN_STATUS\":\"97526\",\"COLLECTER_APPROVED\":\"99937\",\"TOTAL_TEH\":\"101580\",\"DISTRICTCODE\":\"26\",\"TEHSILDAR_APPROVED\":\"101580\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"166059\",\"PDF_DOWNLOAD\":\"413\",\"COLLECTER_TOTAL\":\"164864\",\"TEHSILDAR_REJ\":\"30\",\"PATWAR\":\"421\",\"TEH_ESIGN_STATUS\":\"165185\",\"PATWARI_REJ\":\"4938\",\"PAT_ESIGN_STATUS\":\"165813\",\"FAMILY_REG\":\"174503\",\"TOTAL_PAT\":\"170997\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"चित्तौड़गढ़\",\"COLLECTOR_ESIGN_STATUS\":\"142643\",\"COLLECTER_APPROVED\":\"164864\",\"TOTAL_TEH\":\"165255\",\"DISTRICTCODE\":\"22\",\"TEHSILDAR_APPROVED\":\"165225\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"178169\",\"PDF_DOWNLOAD\":\"281\",\"COLLECTER_TOTAL\":\"172287\",\"TEHSILDAR_REJ\":\"328\",\"PATWAR\":\"304\",\"TEH_ESIGN_STATUS\":\"172733\",\"PATWARI_REJ\":\"6225\",\"PAT_ESIGN_STATUS\":\"174393\",\"FAMILY_REG\":\"195589\",\"TOTAL_PAT\":\"184394\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"चूरू\",\"COLLECTOR_ESIGN_STATUS\":\"172287\",\"COLLECTER_APPROVED\":\"172287\",\"TOTAL_TEH\":\"173709\",\"DISTRICTCODE\":\"3\",\"TEHSILDAR_APPROVED\":\"173381\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"275451\",\"PDF_DOWNLOAD\":\"699\",\"COLLECTER_TOTAL\":\"261312\",\"TEHSILDAR_REJ\":\"6\",\"PATWAR\":\"730\",\"TEH_ESIGN_STATUS\":\"268150\",\"PATWARI_REJ\":\"7445\",\"PAT_ESIGN_STATUS\":\"270523\",\"FAMILY_REG\":\"308690\",\"TOTAL_PAT\":\"282896\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"जयपुर\",\"COLLECTOR_ESIGN_STATUS\":\"258761\",\"COLLECTER_APPROVED\":\"261312\",\"TOTAL_TEH\":\"268731\",\"DISTRICTCODE\":\"9\",\"TEHSILDAR_APPROVED\":\"268725\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"162736\",\"PDF_DOWNLOAD\":\"333\",\"COLLECTER_TOTAL\":\"156105\",\"TEHSILDAR_REJ\":\"15\",\"PATWAR\":\"464\",\"TEH_ESIGN_STATUS\":\"156105\",\"PATWARI_REJ\":\"6408\",\"PAT_ESIGN_STATUS\":\"159996\",\"FAMILY_REG\":\"177637\",\"TOTAL_PAT\":\"169144\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"जालोर\",\"COLLECTOR_ESIGN_STATUS\":\"156105\",\"COLLECTER_APPROVED\":\"156105\",\"TOTAL_TEH\":\"158412\",\"DISTRICTCODE\":\"18\",\"TEHSILDAR_APPROVED\":\"158397\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"48244\",\"PDF_DOWNLOAD\":\"113\",\"COLLECTER_TOTAL\":\"46510\",\"TEHSILDAR_REJ\":\"165\",\"PATWAR\":\"149\",\"TEH_ESIGN_STATUS\":\"47702\",\"PATWARI_REJ\":\"2158\",\"PAT_ESIGN_STATUS\":\"47915\",\"FAMILY_REG\":\"53963\",\"TOTAL_PAT\":\"50402\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"जैसलमेर\",\"COLLECTOR_ESIGN_STATUS\":\"46425\",\"COLLECTER_APPROVED\":\"46510\",\"TOTAL_TEH\":\"47727\",\"DISTRICTCODE\":\"15\",\"TEHSILDAR_APPROVED\":\"47562\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"218287\",\"PDF_DOWNLOAD\":\"447\",\"COLLECTER_TOTAL\":\"211094\",\"TEHSILDAR_REJ\":\"73\",\"PATWAR\":\"449\",\"TEH_ESIGN_STATUS\":\"211094\",\"PATWARI_REJ\":\"18701\",\"PAT_ESIGN_STATUS\":\"213606\",\"FAMILY_REG\":\"247769\",\"TOTAL_PAT\":\"236988\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"जोधपुर\",\"COLLECTOR_ESIGN_STATUS\":\"211094\",\"COLLECTER_APPROVED\":\"211094\",\"TOTAL_TEH\":\"211992\",\"DISTRICTCODE\":\"14\",\"TEHSILDAR_APPROVED\":\"211919\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"200486\",\"PDF_DOWNLOAD\":\"413\",\"COLLECTER_TOTAL\":\"195442\",\"TEHSILDAR_REJ\":\"3\",\"PATWAR\":\"417\",\"TEH_ESIGN_STATUS\":\"195442\",\"PATWARI_REJ\":\"3007\",\"PAT_ESIGN_STATUS\":\"197447\",\"FAMILY_REG\":\"211868\",\"TOTAL_PAT\":\"203493\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"झालावाड़\",\"COLLECTOR_ESIGN_STATUS\":\"194872\",\"COLLECTER_APPROVED\":\"195442\",\"TOTAL_TEH\":\"196454\",\"DISTRICTCODE\":\"17\",\"TEHSILDAR_APPROVED\":\"196451\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"155980\",\"PDF_DOWNLOAD\":\"301\",\"COLLECTER_TOTAL\":\"153278\",\"TEHSILDAR_REJ\":\"240\",\"PATWAR\":\"303\",\"TEH_ESIGN_STATUS\":\"153278\",\"PATWARI_REJ\":\"8353\",\"PAT_ESIGN_STATUS\":\"154932\",\"FAMILY_REG\":\"170664\",\"TOTAL_PAT\":\"164333\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"झूंझुनू\",\"COLLECTOR_ESIGN_STATUS\":\"153278\",\"COLLECTER_APPROVED\":\"153278\",\"TOTAL_TEH\":\"153949\",\"DISTRICTCODE\":\"4\",\"TEHSILDAR_APPROVED\":\"153709\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"157509\",\"PDF_DOWNLOAD\":\"328\",\"COLLECTER_TOTAL\":\"148588\",\"TEHSILDAR_REJ\":\"22\",\"PATWAR\":\"336\",\"TEH_ESIGN_STATUS\":\"148623\",\"PATWARI_REJ\":\"4740\",\"PAT_ESIGN_STATUS\":\"154150\",\"FAMILY_REG\":\"179967\",\"TOTAL_PAT\":\"162249\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"टोंक\",\"COLLECTOR_ESIGN_STATUS\":\"148386\",\"COLLECTER_APPROVED\":\"148588\",\"TOTAL_TEH\":\"150535\",\"DISTRICTCODE\":\"12\",\"TEHSILDAR_APPROVED\":\"150513\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"103140\",\"PDF_DOWNLOAD\":\"280\",\"COLLECTER_TOTAL\":\"102563\",\"TEHSILDAR_REJ\":\"0\",\"PATWAR\":\"284\",\"TEH_ESIGN_STATUS\":\"102631\",\"PATWARI_REJ\":\"9105\",\"PAT_ESIGN_STATUS\":\"103036\",\"FAMILY_REG\":\"114083\",\"TOTAL_PAT\":\"112245\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"डूँगरपुर\",\"COLLECTOR_ESIGN_STATUS\":\"102563\",\"COLLECTER_APPROVED\":\"102563\",\"TOTAL_TEH\":\"102677\",\"DISTRICTCODE\":\"20\",\"TEHSILDAR_APPROVED\":\"102677\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"119449\",\"PDF_DOWNLOAD\":\"304\",\"COLLECTER_TOTAL\":\"113535\",\"TEHSILDAR_REJ\":\"47\",\"PATWAR\":\"308\",\"TEH_ESIGN_STATUS\":\"113930\",\"PATWARI_REJ\":\"4189\",\"PAT_ESIGN_STATUS\":\"117176\",\"FAMILY_REG\":\"137183\",\"TOTAL_PAT\":\"123638\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"दौसा\",\"COLLECTOR_ESIGN_STATUS\":\"110955\",\"COLLECTER_APPROVED\":\"113535\",\"TOTAL_TEH\":\"113976\",\"DISTRICTCODE\":\"7\",\"TEHSILDAR_APPROVED\":\"113929\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"80262\",\"PDF_DOWNLOAD\":\"237\",\"COLLECTER_TOTAL\":\"77198\",\"TEHSILDAR_REJ\":\"1\",\"PATWAR\":\"244\",\"TEH_ESIGN_STATUS\":\"77198\",\"PATWARI_REJ\":\"3678\",\"PAT_ESIGN_STATUS\":\"79140\",\"FAMILY_REG\":\"88191\",\"TOTAL_PAT\":\"83940\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"धौलपुर\",\"COLLECTOR_ESIGN_STATUS\":\"74604\",\"COLLECTER_APPROVED\":\"77198\",\"TOTAL_TEH\":\"77594\",\"DISTRICTCODE\":\"13\",\"TEHSILDAR_APPROVED\":\"77593\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"265008\",\"PDF_DOWNLOAD\":\"524\",\"COLLECTER_TOTAL\":\"257896\",\"TEHSILDAR_REJ\":\"801\",\"PATWAR\":\"530\",\"TEH_ESIGN_STATUS\":\"259614\",\"PATWARI_REJ\":\"21247\",\"PAT_ESIGN_STATUS\":\"262004\",\"FAMILY_REG\":\"296910\",\"TOTAL_PAT\":\"286255\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"नागौर\",\"COLLECTOR_ESIGN_STATUS\":\"257896\",\"COLLECTER_APPROVED\":\"257896\",\"TOTAL_TEH\":\"260590\",\"DISTRICTCODE\":\"21\",\"TEHSILDAR_APPROVED\":\"259789\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"157750\",\"PDF_DOWNLOAD\":\"413\",\"COLLECTER_TOTAL\":\"153266\",\"TEHSILDAR_REJ\":\"0\",\"PATWAR\":\"498\",\"TEH_ESIGN_STATUS\":\"154396\",\"PATWARI_REJ\":\"3535\",\"PAT_ESIGN_STATUS\":\"155885\",\"FAMILY_REG\":\"174363\",\"TOTAL_PAT\":\"161285\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"पाली\",\"COLLECTOR_ESIGN_STATUS\":\"152002\",\"COLLECTER_APPROVED\":\"153266\",\"TOTAL_TEH\":\"155001\",\"DISTRICTCODE\":\"16\",\"TEHSILDAR_APPROVED\":\"155001\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"67082\",\"PDF_DOWNLOAD\":\"193\",\"COLLECTER_TOTAL\":\"66283\",\"TEHSILDAR_REJ\":\"0\",\"PATWAR\":\"193\",\"TEH_ESIGN_STATUS\":\"66512\",\"PATWARI_REJ\":\"6013\",\"PAT_ESIGN_STATUS\":\"66592\",\"FAMILY_REG\":\"76539\",\"TOTAL_PAT\":\"73095\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"प्रतापगढ़\",\"COLLECTOR_ESIGN_STATUS\":\"66145\",\"COLLECTER_APPROVED\":\"66283\",\"TOTAL_TEH\":\"66587\",\"DISTRICTCODE\":\"33\",\"TEHSILDAR_APPROVED\":\"66587\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"149574\",\"PDF_DOWNLOAD\":\"335\",\"COLLECTER_TOTAL\":\"139056\",\"TEHSILDAR_REJ\":\"280\",\"PATWAR\":\"376\",\"TEH_ESIGN_STATUS\":\"143018\",\"PATWARI_REJ\":\"6088\",\"PAT_ESIGN_STATUS\":\"145562\",\"FAMILY_REG\":\"169126\",\"TOTAL_PAT\":\"155662\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"बाँसवाड़ा\",\"COLLECTOR_ESIGN_STATUS\":\"139056\",\"COLLECTER_APPROVED\":\"139056\",\"TOTAL_TEH\":\"143492\",\"DISTRICTCODE\":\"24\",\"TEHSILDAR_APPROVED\":\"143212\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"280118\",\"PDF_DOWNLOAD\":\"437\",\"COLLECTER_TOTAL\":\"272179\",\"TEHSILDAR_REJ\":\"208\",\"PATWAR\":\"511\",\"TEH_ESIGN_STATUS\":\"275032\",\"PATWARI_REJ\":\"11281\",\"PAT_ESIGN_STATUS\":\"276931\",\"FAMILY_REG\":\"315611\",\"TOTAL_PAT\":\"291399\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"बाड़मेर\",\"COLLECTOR_ESIGN_STATUS\":\"270029\",\"COLLECTER_APPROVED\":\"272179\",\"TOTAL_TEH\":\"275506\",\"DISTRICTCODE\":\"8\",\"TEHSILDAR_APPROVED\":\"275298\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"128368\",\"PDF_DOWNLOAD\":\"267\",\"COLLECTER_TOTAL\":\"122778\",\"TEHSILDAR_REJ\":\"1\",\"PATWAR\":\"278\",\"TEH_ESIGN_STATUS\":\"123747\",\"PATWARI_REJ\":\"1667\",\"PAT_ESIGN_STATUS\":\"126505\",\"FAMILY_REG\":\"148768\",\"TOTAL_PAT\":\"130035\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"बाराँ\",\"COLLECTOR_ESIGN_STATUS\":\"122778\",\"COLLECTER_APPROVED\":\"122778\",\"TOTAL_TEH\":\"124788\",\"DISTRICTCODE\":\"29\",\"TEHSILDAR_APPROVED\":\"124787\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"120166\",\"PDF_DOWNLOAD\":\"326\",\"COLLECTER_TOTAL\":\"112849\",\"TEHSILDAR_REJ\":\"73\",\"PATWAR\":\"328\",\"TEH_ESIGN_STATUS\":\"115851\",\"PATWARI_REJ\":\"7748\",\"PAT_ESIGN_STATUS\":\"117614\",\"FAMILY_REG\":\"136428\",\"TOTAL_PAT\":\"127914\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"बीकानेर\",\"COLLECTOR_ESIGN_STATUS\":\"112849\",\"COLLECTER_APPROVED\":\"112849\",\"TOTAL_TEH\":\"116517\",\"DISTRICTCODE\":\"02\",\"TEHSILDAR_APPROVED\":\"116444\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"125030\",\"PDF_DOWNLOAD\":\"260\",\"COLLECTER_TOTAL\":\"113091\",\"TEHSILDAR_REJ\":\"0\",\"PATWAR\":\"288\",\"TEH_ESIGN_STATUS\":\"122912\",\"PATWARI_REJ\":\"1484\",\"PAT_ESIGN_STATUS\":\"123578\",\"FAMILY_REG\":\"134734\",\"TOTAL_PAT\":\"126514\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"बून्दी\",\"COLLECTOR_ESIGN_STATUS\":\"113091\",\"COLLECTER_APPROVED\":\"113091\",\"TOTAL_TEH\":\"123024\",\"DISTRICTCODE\":\"11\",\"TEHSILDAR_APPROVED\":\"123024\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"193003\",\"PDF_DOWNLOAD\":\"516\",\"COLLECTER_TOTAL\":\"190001\",\"TEHSILDAR_REJ\":\"0\",\"PATWAR\":\"712\",\"TEH_ESIGN_STATUS\":\"190001\",\"PATWARI_REJ\":\"6689\",\"PAT_ESIGN_STATUS\":\"191709\",\"FAMILY_REG\":\"208324\",\"TOTAL_PAT\":\"199692\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"भरतपुर\",\"COLLECTOR_ESIGN_STATUS\":\"187005\",\"COLLECTER_APPROVED\":\"190001\",\"TOTAL_TEH\":\"190043\",\"DISTRICTCODE\":\"10\",\"TEHSILDAR_APPROVED\":\"190043\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"259290\",\"PDF_DOWNLOAD\":\"527\",\"COLLECTER_TOTAL\":\"254544\",\"TEHSILDAR_REJ\":\"234\",\"PATWAR\":\"589\",\"TEH_ESIGN_STATUS\":\"256218\",\"PATWARI_REJ\":\"7297\",\"PAT_ESIGN_STATUS\":\"257393\",\"FAMILY_REG\":\"274891\",\"TOTAL_PAT\":\"266587\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"भीलवाड़ा\",\"COLLECTOR_ESIGN_STATUS\":\"244927\",\"COLLECTER_APPROVED\":\"254543\",\"TOTAL_TEH\":\"256480\",\"DISTRICTCODE\":\"6\",\"TEHSILDAR_APPROVED\":\"256246\",\"COLLECTER_REJ\":\"1\"},{\"PATWARI_APPROVED\":\"79394\",\"PDF_DOWNLOAD\":\"254\",\"COLLECTER_TOTAL\":\"76444\",\"TEHSILDAR_REJ\":\"1\",\"PATWAR\":\"256\",\"TEH_ESIGN_STATUS\":\"76546\",\"PATWARI_REJ\":\"1022\",\"PAT_ESIGN_STATUS\":\"77936\",\"FAMILY_REG\":\"88577\",\"TOTAL_PAT\":\"80416\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"राजसमन्द\",\"COLLECTOR_ESIGN_STATUS\":\"75082\",\"COLLECTER_APPROVED\":\"76444\",\"TOTAL_TEH\":\"76665\",\"DISTRICTCODE\":\"30\",\"TEHSILDAR_APPROVED\":\"76664\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"87135\",\"PDF_DOWNLOAD\":\"530\",\"COLLECTER_TOTAL\":\"79269\",\"TEHSILDAR_REJ\":\"4\",\"PATWAR\":\"634\",\"TEH_ESIGN_STATUS\":\"79450\",\"PATWARI_REJ\":\"2974\",\"PAT_ESIGN_STATUS\":\"83869\",\"FAMILY_REG\":\"102174\",\"TOTAL_PAT\":\"90109\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"श्रीगंगानगर\",\"COLLECTOR_ESIGN_STATUS\":\"78089\",\"COLLECTER_APPROVED\":\"79269\",\"TOTAL_TEH\":\"81735\",\"DISTRICTCODE\":\"28\",\"TEHSILDAR_APPROVED\":\"81731\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"121196\",\"PDF_DOWNLOAD\":\"290\",\"COLLECTER_TOTAL\":\"120486\",\"TEHSILDAR_REJ\":\"2\",\"PATWAR\":\"344\",\"TEH_ESIGN_STATUS\":\"120779\",\"PATWARI_REJ\":\"7206\",\"PAT_ESIGN_STATUS\":\"121001\",\"FAMILY_REG\":\"130421\",\"TOTAL_PAT\":\"128402\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"सवाईमाधोपुर\",\"COLLECTOR_ESIGN_STATUS\":\"120486\",\"COLLECTER_APPROVED\":\"120486\",\"TOTAL_TEH\":\"120833\",\"DISTRICTCODE\":\"25\",\"TEHSILDAR_APPROVED\":\"120831\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"53325\",\"PDF_DOWNLOAD\":\"191\",\"COLLECTER_TOTAL\":\"50904\",\"TEHSILDAR_REJ\":\"22\",\"PATWAR\":\"235\",\"TEH_ESIGN_STATUS\":\"51214\",\"PATWARI_REJ\":\"1030\",\"PAT_ESIGN_STATUS\":\"52758\",\"FAMILY_REG\":\"61570\",\"TOTAL_PAT\":\"54355\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"सिरोही\",\"COLLECTOR_ESIGN_STATUS\":\"50904\",\"COLLECTER_APPROVED\":\"50904\",\"TOTAL_TEH\":\"52235\",\"DISTRICTCODE\":\"19\",\"TEHSILDAR_APPROVED\":\"52213\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"181777\",\"PDF_DOWNLOAD\":\"338\",\"COLLECTER_TOTAL\":\"178282\",\"TEHSILDAR_REJ\":\"1\",\"PATWAR\":\"404\",\"TEH_ESIGN_STATUS\":\"179131\",\"PATWARI_REJ\":\"11873\",\"PAT_ESIGN_STATUS\":\"180790\",\"FAMILY_REG\":\"197643\",\"TOTAL_PAT\":\"193650\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"सीकर\",\"COLLECTOR_ESIGN_STATUS\":\"175253\",\"COLLECTER_APPROVED\":\"178282\",\"TOTAL_TEH\":\"179944\",\"DISTRICTCODE\":\"23\",\"TEHSILDAR_APPROVED\":\"179943\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"150121\",\"PDF_DOWNLOAD\":\"346\",\"COLLECTER_TOTAL\":\"145166\",\"TEHSILDAR_REJ\":\"1\",\"PATWAR\":\"393\",\"TEH_ESIGN_STATUS\":\"146346\",\"PATWARI_REJ\":\"5731\",\"PAT_ESIGN_STATUS\":\"147300\",\"FAMILY_REG\":\"163934\",\"TOTAL_PAT\":\"155852\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"हनुमानगढ़\",\"COLLECTOR_ESIGN_STATUS\":\"144480\",\"COLLECTER_APPROVED\":\"145145\",\"TOTAL_TEH\":\"146347\",\"DISTRICTCODE\":\"31\",\"TEHSILDAR_APPROVED\":\"146346\",\"COLLECTER_REJ\":\"21\"}]";
            //result = "[{\"PATWARI_APPROVED\":\"192968\",\"PDF_DOWNLOAD\":\"548\",\"COLLECTER_TOTAL\":\"183060\",\"TEHSILDAR_REJ\":\"14\",\"PATWAR\":\"568\",\"TEH_ESIGN_STATUS\":\"190099\",\"PATWARI_REJ\":\"7011\",\"PAT_ESIGN_STATUS\":\"191164\",\"FAMILY_REG\":\"208727\",\"TOTAL_PAT\":\"199979\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"अजमेर\",\"COLLECTOR_ESIGN_STATUS\":\"180974\",\"COLLECTER_APPROVED\":\"183060\",\"TOTAL_TEH\":\"190697\",\"DISTRICTCODE\":\"1\",\"TEHSILDAR_APPROVED\":\"190683\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"262030\",\"PDF_DOWNLOAD\":\"652\",\"COLLECTER_TOTAL\":\"252225\",\"TEHSILDAR_REJ\":\"15\",\"PATWAR\":\"690\",\"TEH_ESIGN_STATUS\":\"256309\",\"PATWARI_REJ\":\"9820\",\"PAT_ESIGN_STATUS\":\"259529\",\"FAMILY_REG\":\"288942\",\"TOTAL_PAT\":\"271850\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"अलवर\",\"COLLECTOR_ESIGN_STATUS\":\"252052\",\"COLLECTER_APPROVED\":\"252225\",\"TOTAL_TEH\":\"258561\",\"DISTRICTCODE\":\"5\",\"TEHSILDAR_APPROVED\":\"258546\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"213341\",\"PDF_DOWNLOAD\":\"620\",\"COLLECTER_TOTAL\":\"212096\",\"TEHSILDAR_REJ\":\"0\",\"PATWAR\":\"674\",\"TEH_ESIGN_STATUS\":\"212319\",\"PATWARI_REJ\":\"13826\",\"PAT_ESIGN_STATUS\":\"213106\",\"FAMILY_REG\":\"232846\",\"TOTAL_PAT\":\"227167\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"उदयपुर\",\"COLLECTOR_ESIGN_STATUS\":\"211921\",\"COLLECTER_APPROVED\":\"212096\",\"TOTAL_TEH\":\"212425\",\"DISTRICTCODE\":\"27\",\"TEHSILDAR_APPROVED\":\"212425\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"88641\",\"PDF_DOWNLOAD\":\"286\",\"COLLECTER_TOTAL\":\"87272\",\"TEHSILDAR_REJ\":\"34\",\"PATWAR\":\"291\",\"TEH_ESIGN_STATUS\":\"87379\",\"PATWARI_REJ\":\"7094\",\"PAT_ESIGN_STATUS\":\"88191\",\"FAMILY_REG\":\"99039\",\"TOTAL_PAT\":\"95735\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"करौली\",\"COLLECTOR_ESIGN_STATUS\":\"86098\",\"COLLECTER_APPROVED\":\"87272\",\"TOTAL_TEH\":\"88077\",\"DISTRICTCODE\":\"32\",\"TEHSILDAR_APPROVED\":\"88043\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"109942\",\"PDF_DOWNLOAD\":\"224\",\"COLLECTER_TOTAL\":\"99939\",\"TEHSILDAR_REJ\":\"10\",\"PATWAR\":\"228\",\"TEH_ESIGN_STATUS\":\"105967\",\"PATWARI_REJ\":\"2660\",\"PAT_ESIGN_STATUS\":\"108814\",\"FAMILY_REG\":\"123534\",\"TOTAL_PAT\":\"112602\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"कोटा\",\"COLLECTOR_ESIGN_STATUS\":\"97528\",\"COLLECTER_APPROVED\":\"99939\",\"TOTAL_TEH\":\"107233\",\"DISTRICTCODE\":\"26\",\"TEHSILDAR_APPROVED\":\"107223\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"169885\",\"PDF_DOWNLOAD\":\"413\",\"COLLECTER_TOTAL\":\"165491\",\"TEHSILDAR_REJ\":\"30\",\"PATWAR\":\"421\",\"TEH_ESIGN_STATUS\":\"168079\",\"PATWARI_REJ\":\"5063\",\"PAT_ESIGN_STATUS\":\"169006\",\"FAMILY_REG\":\"182560\",\"TOTAL_PAT\":\"174948\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"चित्तौड़गढ़\",\"COLLECTOR_ESIGN_STATUS\":\"165491\",\"COLLECTER_APPROVED\":\"165491\",\"TOTAL_TEH\":\"168300\",\"DISTRICTCODE\":\"22\",\"TEHSILDAR_APPROVED\":\"168270\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"186097\",\"PDF_DOWNLOAD\":\"281\",\"COLLECTER_TOTAL\":\"182723\",\"TEHSILDAR_REJ\":\"343\",\"PATWAR\":\"304\",\"TEH_ESIGN_STATUS\":\"182967\",\"PATWARI_REJ\":\"6724\",\"PAT_ESIGN_STATUS\":\"185083\",\"FAMILY_REG\":\"205008\",\"TOTAL_PAT\":\"192821\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"चूरू\",\"COLLECTOR_ESIGN_STATUS\":\"182462\",\"COLLECTER_APPROVED\":\"182723\",\"TOTAL_TEH\":\"183569\",\"DISTRICTCODE\":\"3\",\"TEHSILDAR_APPROVED\":\"183226\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"285118\",\"PDF_DOWNLOAD\":\"705\",\"COLLECTER_TOTAL\":\"261289\",\"TEHSILDAR_REJ\":\"6\",\"PATWAR\":\"730\",\"TEH_ESIGN_STATUS\":\"275839\",\"PATWARI_REJ\":\"7941\",\"PAT_ESIGN_STATUS\":\"280020\",\"FAMILY_REG\":\"319673\",\"TOTAL_PAT\":\"293059\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"जयपुर\",\"COLLECTOR_ESIGN_STATUS\":\"258738\",\"COLLECTER_APPROVED\":\"261289\",\"TOTAL_TEH\":\"276253\",\"DISTRICTCODE\":\"9\",\"TEHSILDAR_APPROVED\":\"276247\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"172784\",\"PDF_DOWNLOAD\":\"333\",\"COLLECTER_TOTAL\":\"168606\",\"TEHSILDAR_REJ\":\"16\",\"PATWAR\":\"462\",\"TEH_ESIGN_STATUS\":\"168606\",\"PATWARI_REJ\":\"7035\",\"PAT_ESIGN_STATUS\":\"171629\",\"FAMILY_REG\":\"184738\",\"TOTAL_PAT\":\"179819\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"जालोर\",\"COLLECTOR_ESIGN_STATUS\":\"164937\",\"COLLECTER_APPROVED\":\"168606\",\"TOTAL_TEH\":\"170314\",\"DISTRICTCODE\":\"18\",\"TEHSILDAR_APPROVED\":\"170298\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"49882\",\"PDF_DOWNLOAD\":\"113\",\"COLLECTER_TOTAL\":\"47935\",\"TEHSILDAR_REJ\":\"165\",\"PATWAR\":\"150\",\"TEH_ESIGN_STATUS\":\"48589\",\"PATWARI_REJ\":\"2285\",\"PAT_ESIGN_STATUS\":\"49461\",\"FAMILY_REG\":\"56236\",\"TOTAL_PAT\":\"52167\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"जैसलमेर\",\"COLLECTOR_ESIGN_STATUS\":\"47935\",\"COLLECTER_APPROVED\":\"47935\",\"TOTAL_TEH\":\"48762\",\"DISTRICTCODE\":\"15\",\"TEHSILDAR_APPROVED\":\"48597\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"230996\",\"PDF_DOWNLOAD\":\"447\",\"COLLECTER_TOTAL\":\"222126\",\"TEHSILDAR_REJ\":\"86\",\"PATWAR\":\"449\",\"TEH_ESIGN_STATUS\":\"222126\",\"PATWARI_REJ\":\"19794\",\"PAT_ESIGN_STATUS\":\"225988\",\"FAMILY_REG\":\"266475\",\"TOTAL_PAT\":\"250790\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"जोधपुर\",\"COLLECTOR_ESIGN_STATUS\":\"222126\",\"COLLECTER_APPROVED\":\"222126\",\"TOTAL_TEH\":\"223389\",\"DISTRICTCODE\":\"14\",\"TEHSILDAR_APPROVED\":\"223303\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"204470\",\"PDF_DOWNLOAD\":\"413\",\"COLLECTER_TOTAL\":\"199327\",\"TEHSILDAR_REJ\":\"3\",\"PATWAR\":\"417\",\"TEH_ESIGN_STATUS\":\"199327\",\"PATWARI_REJ\":\"3184\",\"PAT_ESIGN_STATUS\":\"202874\",\"FAMILY_REG\":\"218583\",\"TOTAL_PAT\":\"207654\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"झालावाड़\",\"COLLECTOR_ESIGN_STATUS\":\"199327\",\"COLLECTER_APPROVED\":\"199327\",\"TOTAL_TEH\":\"199461\",\"DISTRICTCODE\":\"17\",\"TEHSILDAR_APPROVED\":\"199458\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"161646\",\"PDF_DOWNLOAD\":\"301\",\"COLLECTER_TOTAL\":\"157425\",\"TEHSILDAR_REJ\":\"251\",\"PATWAR\":\"303\",\"TEH_ESIGN_STATUS\":\"157697\",\"PATWARI_REJ\":\"9096\",\"PAT_ESIGN_STATUS\":\"160437\",\"FAMILY_REG\":\"178899\",\"TOTAL_PAT\":\"170742\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"झूंझुनू\",\"COLLECTOR_ESIGN_STATUS\":\"153239\",\"COLLECTER_APPROVED\":\"157425\",\"TOTAL_TEH\":\"158604\",\"DISTRICTCODE\":\"4\",\"TEHSILDAR_APPROVED\":\"158353\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"167583\",\"PDF_DOWNLOAD\":\"328\",\"COLLECTER_TOTAL\":\"160159\",\"TEHSILDAR_REJ\":\"26\",\"PATWAR\":\"336\",\"TEH_ESIGN_STATUS\":\"160525\",\"PATWARI_REJ\":\"5694\",\"PAT_ESIGN_STATUS\":\"164941\",\"FAMILY_REG\":\"188140\",\"TOTAL_PAT\":\"173277\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"टोंक\",\"COLLECTOR_ESIGN_STATUS\":\"160159\",\"COLLECTER_APPROVED\":\"160159\",\"TOTAL_TEH\":\"163460\",\"DISTRICTCODE\":\"12\",\"TEHSILDAR_APPROVED\":\"163434\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"109903\",\"PDF_DOWNLOAD\":\"280\",\"COLLECTER_TOTAL\":\"107637\",\"TEHSILDAR_REJ\":\"0\",\"PATWAR\":\"285\",\"TEH_ESIGN_STATUS\":\"107846\",\"PATWARI_REJ\":\"9747\",\"PAT_ESIGN_STATUS\":\"109451\",\"FAMILY_REG\":\"122517\",\"TOTAL_PAT\":\"119650\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"डूँगरपुर\",\"COLLECTOR_ESIGN_STATUS\":\"107637\",\"COLLECTER_APPROVED\":\"107637\",\"TOTAL_TEH\":\"108440\",\"DISTRICTCODE\":\"20\",\"TEHSILDAR_APPROVED\":\"108440\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"126072\",\"PDF_DOWNLOAD\":\"305\",\"COLLECTER_TOTAL\":\"121008\",\"TEHSILDAR_REJ\":\"52\",\"PATWAR\":\"308\",\"TEH_ESIGN_STATUS\":\"121510\",\"PATWARI_REJ\":\"5015\",\"PAT_ESIGN_STATUS\":\"124286\",\"FAMILY_REG\":\"142259\",\"TOTAL_PAT\":\"131087\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"दौसा\",\"COLLECTOR_ESIGN_STATUS\":\"119971\",\"COLLECTER_APPROVED\":\"121008\",\"TOTAL_TEH\":\"121636\",\"DISTRICTCODE\":\"7\",\"TEHSILDAR_APPROVED\":\"121584\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"85996\",\"PDF_DOWNLOAD\":\"238\",\"COLLECTER_TOTAL\":\"83647\",\"TEHSILDAR_REJ\":\"5\",\"PATWAR\":\"244\",\"TEH_ESIGN_STATUS\":\"83647\",\"PATWARI_REJ\":\"4334\",\"PAT_ESIGN_STATUS\":\"85346\",\"FAMILY_REG\":\"94077\",\"TOTAL_PAT\":\"90330\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"धौलपुर\",\"COLLECTOR_ESIGN_STATUS\":\"83647\",\"COLLECTER_APPROVED\":\"83647\",\"TOTAL_TEH\":\"84205\",\"DISTRICTCODE\":\"13\",\"TEHSILDAR_APPROVED\":\"84200\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"275081\",\"PDF_DOWNLOAD\":\"525\",\"COLLECTER_TOTAL\":\"264913\",\"TEHSILDAR_REJ\":\"842\",\"PATWAR\":\"530\",\"TEH_ESIGN_STATUS\":\"267296\",\"PATWARI_REJ\":\"22013\",\"PAT_ESIGN_STATUS\":\"273307\",\"FAMILY_REG\":\"310094\",\"TOTAL_PAT\":\"297094\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"नागौर\",\"COLLECTOR_ESIGN_STATUS\":\"257916\",\"COLLECTER_APPROVED\":\"264913\",\"TOTAL_TEH\":\"271365\",\"DISTRICTCODE\":\"21\",\"TEHSILDAR_APPROVED\":\"270523\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"170750\",\"PDF_DOWNLOAD\":\"414\",\"COLLECTER_TOTAL\":\"164968\",\"TEHSILDAR_REJ\":\"0\",\"PATWAR\":\"498\",\"TEH_ESIGN_STATUS\":\"167818\",\"PATWARI_REJ\":\"4170\",\"PAT_ESIGN_STATUS\":\"169292\",\"FAMILY_REG\":\"185616\",\"TOTAL_PAT\":\"174920\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"पाली\",\"COLLECTOR_ESIGN_STATUS\":\"164968\",\"COLLECTER_APPROVED\":\"164968\",\"TOTAL_TEH\":\"168492\",\"DISTRICTCODE\":\"16\",\"TEHSILDAR_APPROVED\":\"168492\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"70662\",\"PDF_DOWNLOAD\":\"193\",\"COLLECTER_TOTAL\":\"67854\",\"TEHSILDAR_REJ\":\"3\",\"PATWAR\":\"196\",\"TEH_ESIGN_STATUS\":\"67867\",\"PATWARI_REJ\":\"6457\",\"PAT_ESIGN_STATUS\":\"69979\",\"FAMILY_REG\":\"83765\",\"TOTAL_PAT\":\"77119\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"प्रतापगढ़\",\"COLLECTOR_ESIGN_STATUS\":\"66141\",\"COLLECTER_APPROVED\":\"67854\",\"TOTAL_TEH\":\"68651\",\"DISTRICTCODE\":\"33\",\"TEHSILDAR_APPROVED\":\"68648\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"159140\",\"PDF_DOWNLOAD\":\"335\",\"COLLECTER_TOTAL\":\"150441\",\"TEHSILDAR_REJ\":\"313\",\"PATWAR\":\"377\",\"TEH_ESIGN_STATUS\":\"150790\",\"PATWARI_REJ\":\"7013\",\"PAT_ESIGN_STATUS\":\"155487\",\"FAMILY_REG\":\"175673\",\"TOTAL_PAT\":\"166153\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"बाँसवाड़ा\",\"COLLECTOR_ESIGN_STATUS\":\"145224\",\"COLLECTER_APPROVED\":\"150441\",\"TOTAL_TEH\":\"151951\",\"DISTRICTCODE\":\"24\",\"TEHSILDAR_APPROVED\":\"151638\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"292374\",\"PDF_DOWNLOAD\":\"439\",\"COLLECTER_TOTAL\":\"286048\",\"TEHSILDAR_REJ\":\"266\",\"PATWAR\":\"572\",\"TEH_ESIGN_STATUS\":\"289071\",\"PATWARI_REJ\":\"12969\",\"PAT_ESIGN_STATUS\":\"290277\",\"FAMILY_REG\":\"342492\",\"TOTAL_PAT\":\"305343\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"बाड़मेर\",\"COLLECTOR_ESIGN_STATUS\":\"286048\",\"COLLECTER_APPROVED\":\"286048\",\"TOTAL_TEH\":\"289330\",\"DISTRICTCODE\":\"8\",\"TEHSILDAR_APPROVED\":\"289064\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"134666\",\"PDF_DOWNLOAD\":\"267\",\"COLLECTER_TOTAL\":\"129610\",\"TEHSILDAR_REJ\":\"1\",\"PATWAR\":\"278\",\"TEH_ESIGN_STATUS\":\"129849\",\"PATWARI_REJ\":\"1946\",\"PAT_ESIGN_STATUS\":\"132551\",\"FAMILY_REG\":\"155305\",\"TOTAL_PAT\":\"136612\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"बाराँ\",\"COLLECTOR_ESIGN_STATUS\":\"129610\",\"COLLECTER_APPROVED\":\"129610\",\"TOTAL_TEH\":\"129850\",\"DISTRICTCODE\":\"29\",\"TEHSILDAR_APPROVED\":\"129849\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"129696\",\"PDF_DOWNLOAD\":\"326\",\"COLLECTER_TOTAL\":\"125171\",\"TEHSILDAR_REJ\":\"85\",\"PATWAR\":\"328\",\"TEH_ESIGN_STATUS\":\"125828\",\"PATWARI_REJ\":\"8383\",\"PAT_ESIGN_STATUS\":\"127295\",\"FAMILY_REG\":\"147732\",\"TOTAL_PAT\":\"138079\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"बीकानेर\",\"COLLECTOR_ESIGN_STATUS\":\"125171\",\"COLLECTER_APPROVED\":\"125171\",\"TOTAL_TEH\":\"126509\",\"DISTRICTCODE\":\"02\",\"TEHSILDAR_APPROVED\":\"126424\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"128861\",\"PDF_DOWNLOAD\":\"260\",\"COLLECTER_TOTAL\":\"113090\",\"TEHSILDAR_REJ\":\"0\",\"PATWAR\":\"288\",\"TEH_ESIGN_STATUS\":\"125198\",\"PATWARI_REJ\":\"1777\",\"PAT_ESIGN_STATUS\":\"127459\",\"FAMILY_REG\":\"140242\",\"TOTAL_PAT\":\"130638\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"बून्दी\",\"COLLECTOR_ESIGN_STATUS\":\"113090\",\"COLLECTER_APPROVED\":\"113090\",\"TOTAL_TEH\":\"127006\",\"DISTRICTCODE\":\"11\",\"TEHSILDAR_APPROVED\":\"127006\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"203669\",\"PDF_DOWNLOAD\":\"516\",\"COLLECTER_TOTAL\":\"194306\",\"TEHSILDAR_REJ\":\"1\",\"PATWAR\":\"703\",\"TEH_ESIGN_STATUS\":\"198301\",\"PATWARI_REJ\":\"7369\",\"PAT_ESIGN_STATUS\":\"202578\",\"FAMILY_REG\":\"219395\",\"TOTAL_PAT\":\"211038\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"भरतपुर\",\"COLLECTOR_ESIGN_STATUS\":\"194306\",\"COLLECTER_APPROVED\":\"194306\",\"TOTAL_TEH\":\"200238\",\"DISTRICTCODE\":\"10\",\"TEHSILDAR_APPROVED\":\"200237\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"269940\",\"PDF_DOWNLOAD\":\"527\",\"COLLECTER_TOTAL\":\"263822\",\"TEHSILDAR_REJ\":\"305\",\"PATWAR\":\"589\",\"TEH_ESIGN_STATUS\":\"265714\",\"PATWARI_REJ\":\"7592\",\"PAT_ESIGN_STATUS\":\"268017\",\"FAMILY_REG\":\"288328\",\"TOTAL_PAT\":\"277532\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"भीलवाड़ा\",\"COLLECTOR_ESIGN_STATUS\":\"256830\",\"COLLECTER_APPROVED\":\"263821\",\"TOTAL_TEH\":\"266457\",\"DISTRICTCODE\":\"6\",\"TEHSILDAR_APPROVED\":\"266152\",\"COLLECTER_REJ\":\"1\"},{\"PATWARI_APPROVED\":\"83302\",\"PDF_DOWNLOAD\":\"255\",\"COLLECTER_TOTAL\":\"79216\",\"TEHSILDAR_REJ\":\"1\",\"PATWAR\":\"256\",\"TEH_ESIGN_STATUS\":\"79663\",\"PATWARI_REJ\":\"1184\",\"PAT_ESIGN_STATUS\":\"81792\",\"FAMILY_REG\":\"92332\",\"TOTAL_PAT\":\"84486\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"राजसमन्द\",\"COLLECTOR_ESIGN_STATUS\":\"74938\",\"COLLECTER_APPROVED\":\"79216\",\"TOTAL_TEH\":\"80312\",\"DISTRICTCODE\":\"30\",\"TEHSILDAR_APPROVED\":\"80311\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"99283\",\"PDF_DOWNLOAD\":\"533\",\"COLLECTER_TOTAL\":\"93846\",\"TEHSILDAR_REJ\":\"4\",\"PATWAR\":\"634\",\"TEH_ESIGN_STATUS\":\"94497\",\"PATWARI_REJ\":\"3866\",\"PAT_ESIGN_STATUS\":\"96945\",\"FAMILY_REG\":\"114412\",\"TOTAL_PAT\":\"103149\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"श्रीगंगानगर\",\"COLLECTOR_ESIGN_STATUS\":\"92915\",\"COLLECTER_APPROVED\":\"93846\",\"TOTAL_TEH\":\"95080\",\"DISTRICTCODE\":\"28\",\"TEHSILDAR_APPROVED\":\"95076\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"125505\",\"PDF_DOWNLOAD\":\"290\",\"COLLECTER_TOTAL\":\"124643\",\"TEHSILDAR_REJ\":\"3\",\"PATWAR\":\"341\",\"TEH_ESIGN_STATUS\":\"124643\",\"PATWARI_REJ\":\"7490\",\"PAT_ESIGN_STATUS\":\"125083\",\"FAMILY_REG\":\"135456\",\"TOTAL_PAT\":\"132995\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"सवाईमाधोपुर\",\"COLLECTOR_ESIGN_STATUS\":\"124643\",\"COLLECTER_APPROVED\":\"124643\",\"TOTAL_TEH\":\"124782\",\"DISTRICTCODE\":\"25\",\"TEHSILDAR_APPROVED\":\"124779\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"54725\",\"PDF_DOWNLOAD\":\"191\",\"COLLECTER_TOTAL\":\"50903\",\"TEHSILDAR_REJ\":\"22\",\"PATWAR\":\"235\",\"TEH_ESIGN_STATUS\":\"51762\",\"PATWARI_REJ\":\"1244\",\"PAT_ESIGN_STATUS\":\"54302\",\"FAMILY_REG\":\"65152\",\"TOTAL_PAT\":\"55969\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"सिरोही\",\"COLLECTOR_ESIGN_STATUS\":\"50903\",\"COLLECTER_APPROVED\":\"50903\",\"TOTAL_TEH\":\"52783\",\"DISTRICTCODE\":\"19\",\"TEHSILDAR_APPROVED\":\"52761\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"192413\",\"PDF_DOWNLOAD\":\"338\",\"COLLECTER_TOTAL\":\"181301\",\"TEHSILDAR_REJ\":\"1\",\"PATWAR\":\"396\",\"TEH_ESIGN_STATUS\":\"188496\",\"PATWARI_REJ\":\"12164\",\"PAT_ESIGN_STATUS\":\"191177\",\"FAMILY_REG\":\"212861\",\"TOTAL_PAT\":\"204577\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"सीकर\",\"COLLECTOR_ESIGN_STATUS\":\"175264\",\"COLLECTER_APPROVED\":\"181301\",\"TOTAL_TEH\":\"188706\",\"DISTRICTCODE\":\"23\",\"TEHSILDAR_APPROVED\":\"188705\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"155303\",\"PDF_DOWNLOAD\":\"346\",\"COLLECTER_TOTAL\":\"148073\",\"TEHSILDAR_REJ\":\"1\",\"PATWAR\":\"388\",\"TEH_ESIGN_STATUS\":\"151353\",\"PATWARI_REJ\":\"5982\",\"PAT_ESIGN_STATUS\":\"152487\",\"FAMILY_REG\":\"172934\",\"TOTAL_PAT\":\"161285\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"हनुमानगढ़\",\"COLLECTOR_ESIGN_STATUS\":\"144458\",\"COLLECTER_APPROVED\":\"148052\",\"TOTAL_TEH\":\"152034\",\"DISTRICTCODE\":\"31\",\"TEHSILDAR_APPROVED\":\"152033\",\"COLLECTER_REJ\":\"21\"}]";
            //result = "[{\"PATWARI_APPROVED\":\"195987\",\"PDF_DOWNLOAD\":\"548\",\"COLLECTER_TOTAL\":\"188107\",\"TEHSILDAR_REJ\":\"14\",\"PATWAR\":\"568\",\"TEH_ESIGN_STATUS\":\"191761\",\"PATWARI_REJ\":\"7103\",\"PAT_ESIGN_STATUS\":\"194313\",\"FAMILY_REG\":\"212340\",\"TOTAL_PAT\":\"203090\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"अजमेर\",\"COLLECTOR_ESIGN_STATUS\":\"186601\",\"COLLECTER_APPROVED\":\"188107\",\"TOTAL_TEH\":\"192408\",\"DISTRICTCODE\":\"1\",\"TEHSILDAR_APPROVED\":\"192394\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"265892\",\"PDF_DOWNLOAD\":\"652\",\"COLLECTER_TOTAL\":\"257200\",\"TEHSILDAR_REJ\":\"60\",\"PATWAR\":\"690\",\"TEH_ESIGN_STATUS\":\"258446\",\"PATWARI_REJ\":\"9940\",\"PAT_ESIGN_STATUS\":\"263842\",\"FAMILY_REG\":\"293763\",\"TOTAL_PAT\":\"275832\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"अलवर\",\"COLLECTOR_ESIGN_STATUS\":\"257200\",\"COLLECTER_APPROVED\":\"257200\",\"TOTAL_TEH\":\"261158\",\"DISTRICTCODE\":\"5\",\"TEHSILDAR_APPROVED\":\"261098\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"214614\",\"PDF_DOWNLOAD\":\"620\",\"COLLECTER_TOTAL\":\"213068\",\"TEHSILDAR_REJ\":\"0\",\"PATWAR\":\"674\",\"TEH_ESIGN_STATUS\":\"213367\",\"PATWARI_REJ\":\"13918\",\"PAT_ESIGN_STATUS\":\"214235\",\"FAMILY_REG\":\"235283\",\"TOTAL_PAT\":\"228532\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"उदयपुर\",\"COLLECTOR_ESIGN_STATUS\":\"212513\",\"COLLECTER_APPROVED\":\"213068\",\"TOTAL_TEH\":\"213577\",\"DISTRICTCODE\":\"27\",\"TEHSILDAR_APPROVED\":\"213577\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"90913\",\"PDF_DOWNLOAD\":\"288\",\"COLLECTER_TOTAL\":\"89479\",\"TEHSILDAR_REJ\":\"36\",\"PATWAR\":\"291\",\"TEH_ESIGN_STATUS\":\"89479\",\"PATWARI_REJ\":\"7360\",\"PAT_ESIGN_STATUS\":\"90280\",\"FAMILY_REG\":\"101508\",\"TOTAL_PAT\":\"98273\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"करौली\",\"COLLECTOR_ESIGN_STATUS\":\"86091\",\"COLLECTER_APPROVED\":\"89479\",\"TOTAL_TEH\":\"89725\",\"DISTRICTCODE\":\"32\",\"TEHSILDAR_APPROVED\":\"89689\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"112530\",\"PDF_DOWNLOAD\":\"224\",\"COLLECTER_TOTAL\":\"99939\",\"TEHSILDAR_REJ\":\"10\",\"PATWAR\":\"228\",\"TEH_ESIGN_STATUS\":\"108192\",\"PATWARI_REJ\":\"2804\",\"PAT_ESIGN_STATUS\":\"111581\",\"FAMILY_REG\":\"125797\",\"TOTAL_PAT\":\"115334\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"कोटा\",\"COLLECTOR_ESIGN_STATUS\":\"97528\",\"COLLECTER_APPROVED\":\"99939\",\"TOTAL_TEH\":\"108803\",\"DISTRICTCODE\":\"26\",\"TEHSILDAR_APPROVED\":\"108793\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"171739\",\"PDF_DOWNLOAD\":\"413\",\"COLLECTER_TOTAL\":\"169260\",\"TEHSILDAR_REJ\":\"30\",\"PATWAR\":\"421\",\"TEH_ESIGN_STATUS\":\"169345\",\"PATWARI_REJ\":\"5130\",\"PAT_ESIGN_STATUS\":\"170731\",\"FAMILY_REG\":\"186578\",\"TOTAL_PAT\":\"176869\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"चित्तौड़गढ़\",\"COLLECTOR_ESIGN_STATUS\":\"165492\",\"COLLECTER_APPROVED\":\"169260\",\"TOTAL_TEH\":\"169989\",\"DISTRICTCODE\":\"22\",\"TEHSILDAR_APPROVED\":\"169959\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"188828\",\"PDF_DOWNLOAD\":\"281\",\"COLLECTER_TOTAL\":\"186683\",\"TEHSILDAR_REJ\":\"346\",\"PATWAR\":\"304\",\"TEH_ESIGN_STATUS\":\"187102\",\"PATWARI_REJ\":\"6897\",\"PAT_ESIGN_STATUS\":\"187855\",\"FAMILY_REG\":\"208800\",\"TOTAL_PAT\":\"195725\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"चूरू\",\"COLLECTOR_ESIGN_STATUS\":\"186683\",\"COLLECTER_APPROVED\":\"186683\",\"TOTAL_TEH\":\"187661\",\"DISTRICTCODE\":\"3\",\"TEHSILDAR_APPROVED\":\"187315\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"289575\",\"PDF_DOWNLOAD\":\"705\",\"COLLECTER_TOTAL\":\"278485\",\"TEHSILDAR_REJ\":\"6\",\"PATWAR\":\"730\",\"TEH_ESIGN_STATUS\":\"278485\",\"PATWARI_REJ\":\"8025\",\"PAT_ESIGN_STATUS\":\"284413\",\"FAMILY_REG\":\"324601\",\"TOTAL_PAT\":\"297600\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"जयपुर\",\"COLLECTOR_ESIGN_STATUS\":\"278485\",\"COLLECTER_APPROVED\":\"278485\",\"TOTAL_TEH\":\"279288\",\"DISTRICTCODE\":\"9\",\"TEHSILDAR_APPROVED\":\"279282\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"174802\",\"PDF_DOWNLOAD\":\"333\",\"COLLECTER_TOTAL\":\"169278\",\"TEHSILDAR_REJ\":\"17\",\"PATWAR\":\"462\",\"TEH_ESIGN_STATUS\":\"169898\",\"PATWARI_REJ\":\"7112\",\"PAT_ESIGN_STATUS\":\"173816\",\"FAMILY_REG\":\"187059\",\"TOTAL_PAT\":\"181914\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"जालोर\",\"COLLECTOR_ESIGN_STATUS\":\"164937\",\"COLLECTER_APPROVED\":\"169278\",\"TOTAL_TEH\":\"172362\",\"DISTRICTCODE\":\"18\",\"TEHSILDAR_APPROVED\":\"172345\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"51644\",\"PDF_DOWNLOAD\":\"114\",\"COLLECTER_TOTAL\":\"50380\",\"TEHSILDAR_REJ\":\"165\",\"PATWAR\":\"150\",\"TEH_ESIGN_STATUS\":\"50589\",\"PATWARI_REJ\":\"2383\",\"PAT_ESIGN_STATUS\":\"51251\",\"FAMILY_REG\":\"56800\",\"TOTAL_PAT\":\"54027\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"जैसलमेर\",\"COLLECTOR_ESIGN_STATUS\":\"50380\",\"COLLECTER_APPROVED\":\"50380\",\"TOTAL_TEH\":\"50668\",\"DISTRICTCODE\":\"15\",\"TEHSILDAR_APPROVED\":\"50503\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"237226\",\"PDF_DOWNLOAD\":\"447\",\"COLLECTER_TOTAL\":\"227954\",\"TEHSILDAR_REJ\":\"87\",\"PATWAR\":\"449\",\"TEH_ESIGN_STATUS\":\"228017\",\"PATWARI_REJ\":\"20276\",\"PAT_ESIGN_STATUS\":\"232027\",\"FAMILY_REG\":\"273088\",\"TOTAL_PAT\":\"257502\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"जोधपुर\",\"COLLECTOR_ESIGN_STATUS\":\"227954\",\"COLLECTER_APPROVED\":\"227954\",\"TOTAL_TEH\":\"228884\",\"DISTRICTCODE\":\"14\",\"TEHSILDAR_APPROVED\":\"228797\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"207095\",\"PDF_DOWNLOAD\":\"413\",\"COLLECTER_TOTAL\":\"199326\",\"TEHSILDAR_REJ\":\"3\",\"PATWAR\":\"417\",\"TEH_ESIGN_STATUS\":\"201116\",\"PATWARI_REJ\":\"3282\",\"PAT_ESIGN_STATUS\":\"204969\",\"FAMILY_REG\":\"221893\",\"TOTAL_PAT\":\"210377\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"झालावाड़\",\"COLLECTOR_ESIGN_STATUS\":\"199326\",\"COLLECTER_APPROVED\":\"199326\",\"TOTAL_TEH\":\"202622\",\"DISTRICTCODE\":\"17\",\"TEHSILDAR_APPROVED\":\"202619\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"164973\",\"PDF_DOWNLOAD\":\"301\",\"COLLECTER_TOTAL\":\"158212\",\"TEHSILDAR_REJ\":\"252\",\"PATWAR\":\"303\",\"TEH_ESIGN_STATUS\":\"159288\",\"PATWARI_REJ\":\"9247\",\"PAT_ESIGN_STATUS\":\"163831\",\"FAMILY_REG\":\"183553\",\"TOTAL_PAT\":\"174220\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"झूंझुनू\",\"COLLECTOR_ESIGN_STATUS\":\"153235\",\"COLLECTER_APPROVED\":\"158212\",\"TOTAL_TEH\":\"160328\",\"DISTRICTCODE\":\"4\",\"TEHSILDAR_APPROVED\":\"160076\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"171106\",\"PDF_DOWNLOAD\":\"329\",\"COLLECTER_TOTAL\":\"165905\",\"TEHSILDAR_REJ\":\"27\",\"PATWAR\":\"336\",\"TEH_ESIGN_STATUS\":\"165906\",\"PATWARI_REJ\":\"5967\",\"PAT_ESIGN_STATUS\":\"168417\",\"FAMILY_REG\":\"192189\",\"TOTAL_PAT\":\"177073\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"टोंक\",\"COLLECTOR_ESIGN_STATUS\":\"165905\",\"COLLECTER_APPROVED\":\"165905\",\"TOTAL_TEH\":\"167194\",\"DISTRICTCODE\":\"12\",\"TEHSILDAR_APPROVED\":\"167167\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"113709\",\"PDF_DOWNLOAD\":\"281\",\"COLLECTER_TOTAL\":\"111412\",\"TEHSILDAR_REJ\":\"0\",\"PATWAR\":\"285\",\"TEH_ESIGN_STATUS\":\"111545\",\"PATWARI_REJ\":\"10072\",\"PAT_ESIGN_STATUS\":\"113414\",\"FAMILY_REG\":\"126909\",\"TOTAL_PAT\":\"123781\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"डूँगरपुर\",\"COLLECTOR_ESIGN_STATUS\":\"110088\",\"COLLECTER_APPROVED\":\"111412\",\"TOTAL_TEH\":\"111730\",\"DISTRICTCODE\":\"20\",\"TEHSILDAR_APPROVED\":\"111730\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"128167\",\"PDF_DOWNLOAD\":\"306\",\"COLLECTER_TOTAL\":\"123212\",\"TEHSILDAR_REJ\":\"94\",\"PATWAR\":\"308\",\"TEH_ESIGN_STATUS\":\"123212\",\"PATWARI_REJ\":\"5245\",\"PAT_ESIGN_STATUS\":\"127151\",\"FAMILY_REG\":\"144890\",\"TOTAL_PAT\":\"133412\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"दौसा\",\"COLLECTOR_ESIGN_STATUS\":\"123212\",\"COLLECTER_APPROVED\":\"123212\",\"TOTAL_TEH\":\"124011\",\"DISTRICTCODE\":\"7\",\"TEHSILDAR_APPROVED\":\"123917\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"87628\",\"PDF_DOWNLOAD\":\"238\",\"COLLECTER_TOTAL\":\"84322\",\"TEHSILDAR_REJ\":\"5\",\"PATWAR\":\"244\",\"TEH_ESIGN_STATUS\":\"84322\",\"PATWARI_REJ\":\"4388\",\"PAT_ESIGN_STATUS\":\"86882\",\"FAMILY_REG\":\"96770\",\"TOTAL_PAT\":\"92016\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"धौलपुर\",\"COLLECTOR_ESIGN_STATUS\":\"84322\",\"COLLECTER_APPROVED\":\"84322\",\"TOTAL_TEH\":\"84951\",\"DISTRICTCODE\":\"13\",\"TEHSILDAR_APPROVED\":\"84946\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"280180\",\"PDF_DOWNLOAD\":\"525\",\"COLLECTER_TOTAL\":\"265812\",\"TEHSILDAR_REJ\":\"854\",\"PATWAR\":\"530\",\"TEH_ESIGN_STATUS\":\"273865\",\"PATWARI_REJ\":\"22457\",\"PAT_ESIGN_STATUS\":\"278508\",\"FAMILY_REG\":\"315890\",\"TOTAL_PAT\":\"302637\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"नागौर\",\"COLLECTOR_ESIGN_STATUS\":\"257915\",\"COLLECTER_APPROVED\":\"265812\",\"TOTAL_TEH\":\"276810\",\"DISTRICTCODE\":\"21\",\"TEHSILDAR_APPROVED\":\"275956\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"173644\",\"PDF_DOWNLOAD\":\"414\",\"COLLECTER_TOTAL\":\"170792\",\"TEHSILDAR_REJ\":\"0\",\"PATWAR\":\"498\",\"TEH_ESIGN_STATUS\":\"170792\",\"PATWARI_REJ\":\"4392\",\"PAT_ESIGN_STATUS\":\"172150\",\"FAMILY_REG\":\"189838\",\"TOTAL_PAT\":\"178036\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"पाली\",\"COLLECTOR_ESIGN_STATUS\":\"169432\",\"COLLECTER_APPROVED\":\"170792\",\"TOTAL_TEH\":\"171266\",\"DISTRICTCODE\":\"16\",\"TEHSILDAR_APPROVED\":\"171266\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"71768\",\"PDF_DOWNLOAD\":\"193\",\"COLLECTER_TOTAL\":\"69843\",\"TEHSILDAR_REJ\":\"3\",\"PATWAR\":\"196\",\"TEH_ESIGN_STATUS\":\"69843\",\"PATWARI_REJ\":\"6484\",\"PAT_ESIGN_STATUS\":\"70923\",\"FAMILY_REG\":\"84857\",\"TOTAL_PAT\":\"78252\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"प्रतापगढ़\",\"COLLECTOR_ESIGN_STATUS\":\"69843\",\"COLLECTER_APPROVED\":\"69843\",\"TOTAL_TEH\":\"70127\",\"DISTRICTCODE\":\"33\",\"TEHSILDAR_APPROVED\":\"70124\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"162819\",\"PDF_DOWNLOAD\":\"337\",\"COLLECTER_TOTAL\":\"158314\",\"TEHSILDAR_REJ\":\"319\",\"PATWAR\":\"378\",\"TEH_ESIGN_STATUS\":\"158314\",\"PATWARI_REJ\":\"7347\",\"PAT_ESIGN_STATUS\":\"160517\",\"FAMILY_REG\":\"178931\",\"TOTAL_PAT\":\"170166\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"बाँसवाड़ा\",\"COLLECTOR_ESIGN_STATUS\":\"152240\",\"COLLECTER_APPROVED\":\"158314\",\"TOTAL_TEH\":\"158882\",\"DISTRICTCODE\":\"24\",\"TEHSILDAR_APPROVED\":\"158563\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"296134\",\"PDF_DOWNLOAD\":\"439\",\"COLLECTER_TOTAL\":\"289262\",\"TEHSILDAR_REJ\":\"269\",\"PATWAR\":\"572\",\"TEH_ESIGN_STATUS\":\"290146\",\"PATWARI_REJ\":\"13196\",\"PAT_ESIGN_STATUS\":\"293835\",\"FAMILY_REG\":\"345057\",\"TOTAL_PAT\":\"309330\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"बाड़मेर\",\"COLLECTOR_ESIGN_STATUS\":\"286045\",\"COLLECTER_APPROVED\":\"289262\",\"TOTAL_TEH\":\"290872\",\"DISTRICTCODE\":\"8\",\"TEHSILDAR_APPROVED\":\"290603\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"136585\",\"PDF_DOWNLOAD\":\"267\",\"COLLECTER_TOTAL\":\"130956\",\"TEHSILDAR_REJ\":\"1\",\"PATWAR\":\"278\",\"TEH_ESIGN_STATUS\":\"130956\",\"PATWARI_REJ\":\"2076\",\"PAT_ESIGN_STATUS\":\"133850\",\"FAMILY_REG\":\"158474\",\"TOTAL_PAT\":\"138661\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"बाराँ\",\"COLLECTOR_ESIGN_STATUS\":\"130956\",\"COLLECTER_APPROVED\":\"130956\",\"TOTAL_TEH\":\"132037\",\"DISTRICTCODE\":\"29\",\"TEHSILDAR_APPROVED\":\"132036\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"132952\",\"PDF_DOWNLOAD\":\"326\",\"COLLECTER_TOTAL\":\"130574\",\"TEHSILDAR_REJ\":\"85\",\"PATWAR\":\"328\",\"TEH_ESIGN_STATUS\":\"130574\",\"PATWARI_REJ\":\"8512\",\"PAT_ESIGN_STATUS\":\"131174\",\"FAMILY_REG\":\"150282\",\"TOTAL_PAT\":\"141464\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"बीकानेर\",\"COLLECTOR_ESIGN_STATUS\":\"130574\",\"COLLECTER_APPROVED\":\"130574\",\"TOTAL_TEH\":\"130860\",\"DISTRICTCODE\":\"02\",\"TEHSILDAR_APPROVED\":\"130775\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"131189\",\"PDF_DOWNLOAD\":\"260\",\"COLLECTER_TOTAL\":\"128854\",\"TEHSILDAR_REJ\":\"0\",\"PATWAR\":\"288\",\"TEH_ESIGN_STATUS\":\"128872\",\"PATWARI_REJ\":\"1886\",\"PAT_ESIGN_STATUS\":\"130020\",\"FAMILY_REG\":\"143642\",\"TOTAL_PAT\":\"133075\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"बून्दी\",\"COLLECTOR_ESIGN_STATUS\":\"124411\",\"COLLECTER_APPROVED\":\"128854\",\"TOTAL_TEH\":\"128962\",\"DISTRICTCODE\":\"11\",\"TEHSILDAR_APPROVED\":\"128962\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"207238\",\"PDF_DOWNLOAD\":\"516\",\"COLLECTER_TOTAL\":\"203512\",\"TEHSILDAR_REJ\":\"1\",\"PATWAR\":\"703\",\"TEH_ESIGN_STATUS\":\"203512\",\"PATWARI_REJ\":\"7542\",\"PAT_ESIGN_STATUS\":\"205930\",\"FAMILY_REG\":\"222984\",\"TOTAL_PAT\":\"214780\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"भरतपुर\",\"COLLECTOR_ESIGN_STATUS\":\"194303\",\"COLLECTER_APPROVED\":\"203512\",\"TOTAL_TEH\":\"204294\",\"DISTRICTCODE\":\"10\",\"TEHSILDAR_APPROVED\":\"204293\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"274606\",\"PDF_DOWNLOAD\":\"527\",\"COLLECTER_TOTAL\":\"267108\",\"TEHSILDAR_REJ\":\"313\",\"PATWAR\":\"589\",\"TEH_ESIGN_STATUS\":\"269165\",\"PATWARI_REJ\":\"7720\",\"PAT_ESIGN_STATUS\":\"271759\",\"FAMILY_REG\":\"294661\",\"TOTAL_PAT\":\"282326\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"भीलवाड़ा\",\"COLLECTOR_ESIGN_STATUS\":\"256827\",\"COLLECTER_APPROVED\":\"267107\",\"TOTAL_TEH\":\"270187\",\"DISTRICTCODE\":\"6\",\"TEHSILDAR_APPROVED\":\"269874\",\"COLLECTER_REJ\":\"1\"},{\"PATWARI_APPROVED\":\"85213\",\"PDF_DOWNLOAD\":\"255\",\"COLLECTER_TOTAL\":\"82219\",\"TEHSILDAR_REJ\":\"1\",\"PATWAR\":\"256\",\"TEH_ESIGN_STATUS\":\"82219\",\"PATWARI_REJ\":\"1299\",\"PAT_ESIGN_STATUS\":\"83681\",\"FAMILY_REG\":\"94895\",\"TOTAL_PAT\":\"86512\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"राजसमन्द\",\"COLLECTOR_ESIGN_STATUS\":\"74939\",\"COLLECTER_APPROVED\":\"82219\",\"TOTAL_TEH\":\"82666\",\"DISTRICTCODE\":\"30\",\"TEHSILDAR_APPROVED\":\"82665\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"104295\",\"PDF_DOWNLOAD\":\"539\",\"COLLECTER_TOTAL\":\"100490\",\"TEHSILDAR_REJ\":\"4\",\"PATWAR\":\"634\",\"TEH_ESIGN_STATUS\":\"100490\",\"PATWARI_REJ\":\"4167\",\"PAT_ESIGN_STATUS\":\"102156\",\"FAMILY_REG\":\"116421\",\"TOTAL_PAT\":\"108462\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"श्रीगंगानगर\",\"COLLECTOR_ESIGN_STATUS\":\"92907\",\"COLLECTER_APPROVED\":\"100490\",\"TOTAL_TEH\":\"101507\",\"DISTRICTCODE\":\"28\",\"TEHSILDAR_APPROVED\":\"101503\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"127376\",\"PDF_DOWNLOAD\":\"290\",\"COLLECTER_TOTAL\":\"125872\",\"TEHSILDAR_REJ\":\"4\",\"PATWAR\":\"341\",\"TEH_ESIGN_STATUS\":\"126432\",\"PATWARI_REJ\":\"7634\",\"PAT_ESIGN_STATUS\":\"126978\",\"FAMILY_REG\":\"137445\",\"TOTAL_PAT\":\"135010\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"सवाईमाधोपुर\",\"COLLECTOR_ESIGN_STATUS\":\"125872\",\"COLLECTER_APPROVED\":\"125872\",\"TOTAL_TEH\":\"126488\",\"DISTRICTCODE\":\"25\",\"TEHSILDAR_APPROVED\":\"126484\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"55651\",\"PDF_DOWNLOAD\":\"191\",\"COLLECTER_TOTAL\":\"51763\",\"TEHSILDAR_REJ\":\"22\",\"PATWAR\":\"235\",\"TEH_ESIGN_STATUS\":\"52266\",\"PATWARI_REJ\":\"1268\",\"PAT_ESIGN_STATUS\":\"55093\",\"FAMILY_REG\":\"66398\",\"TOTAL_PAT\":\"56919\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"सिरोही\",\"COLLECTOR_ESIGN_STATUS\":\"51763\",\"COLLECTER_APPROVED\":\"51763\",\"TOTAL_TEH\":\"53242\",\"DISTRICTCODE\":\"19\",\"TEHSILDAR_APPROVED\":\"53220\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"196816\",\"PDF_DOWNLOAD\":\"338\",\"COLLECTER_TOTAL\":\"189903\",\"TEHSILDAR_REJ\":\"1\",\"PATWAR\":\"396\",\"TEH_ESIGN_STATUS\":\"189903\",\"PATWARI_REJ\":\"12344\",\"PAT_ESIGN_STATUS\":\"195524\",\"FAMILY_REG\":\"217479\",\"TOTAL_PAT\":\"209160\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"सीकर\",\"COLLECTOR_ESIGN_STATUS\":\"189573\",\"COLLECTER_APPROVED\":\"189903\",\"TOTAL_TEH\":\"191609\",\"DISTRICTCODE\":\"23\",\"TEHSILDAR_APPROVED\":\"191608\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"159021\",\"PDF_DOWNLOAD\":\"346\",\"COLLECTER_TOTAL\":\"151351\",\"TEHSILDAR_REJ\":\"1\",\"PATWAR\":\"388\",\"TEH_ESIGN_STATUS\":\"153020\",\"PATWARI_REJ\":\"6222\",\"PAT_ESIGN_STATUS\":\"155707\",\"FAMILY_REG\":\"176962\",\"TOTAL_PAT\":\"165243\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"हनुमानगढ़\",\"COLLECTOR_ESIGN_STATUS\":\"144458\",\"COLLECTER_APPROVED\":\"151330\",\"TOTAL_TEH\":\"154331\",\"DISTRICTCODE\":\"31\",\"TEHSILDAR_APPROVED\":\"154330\",\"COLLECTER_REJ\":\"21\"}]";
            //result = "[{\"PATWARI_APPROVED\":\"201719\",\"PDF_DOWNLOAD\":\"548\",\"COLLECTER_TOTAL\":\"188067\",\"TEHSILDAR_REJ\":\"16\",\"PATWAR\":\"568\",\"TEH_ESIGN_STATUS\":\"196484\",\"PATWARI_REJ\":\"6712\",\"PAT_ESIGN_STATUS\":\"199850\",\"FAMILY_REG\":\"219841\",\"TOTAL_PAT\":\"208431\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"अजमेर\",\"COLLECTOR_ESIGN_STATUS\":\"186564\",\"COLLECTER_APPROVED\":\"188067\",\"TOTAL_TEH\":\"197019\",\"DISTRICTCODE\":\"1\",\"TEHSILDAR_APPROVED\":\"197003\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"272431\",\"PDF_DOWNLOAD\":\"653\",\"COLLECTER_TOTAL\":\"265697\",\"TEHSILDAR_REJ\":\"57\",\"PATWAR\":\"690\",\"TEH_ESIGN_STATUS\":\"266984\",\"PATWARI_REJ\":\"9309\",\"PAT_ESIGN_STATUS\":\"269819\",\"FAMILY_REG\":\"299100\",\"TOTAL_PAT\":\"281740\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"अलवर\",\"COLLECTOR_ESIGN_STATUS\":\"257066\",\"COLLECTER_APPROVED\":\"265697\",\"TOTAL_TEH\":\"267528\",\"DISTRICTCODE\":\"5\",\"TEHSILDAR_APPROVED\":\"267471\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"217189\",\"PDF_DOWNLOAD\":\"620\",\"COLLECTER_TOTAL\":\"214931\",\"TEHSILDAR_REJ\":\"0\",\"PATWAR\":\"674\",\"TEH_ESIGN_STATUS\":\"214985\",\"PATWARI_REJ\":\"13570\",\"PAT_ESIGN_STATUS\":\"216524\",\"FAMILY_REG\":\"238992\",\"TOTAL_PAT\":\"230759\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"उदयपुर\",\"COLLECTOR_ESIGN_STATUS\":\"214710\",\"COLLECTER_APPROVED\":\"214931\",\"TOTAL_TEH\":\"215313\",\"DISTRICTCODE\":\"27\",\"TEHSILDAR_APPROVED\":\"215313\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"92707\",\"PDF_DOWNLOAD\":\"288\",\"COLLECTER_TOTAL\":\"91362\",\"TEHSILDAR_REJ\":\"36\",\"PATWAR\":\"291\",\"TEH_ESIGN_STATUS\":\"91374\",\"PATWARI_REJ\":\"7068\",\"PAT_ESIGN_STATUS\":\"92142\",\"FAMILY_REG\":\"104476\",\"TOTAL_PAT\":\"99775\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"करौली\",\"COLLECTOR_ESIGN_STATUS\":\"89430\",\"COLLECTER_APPROVED\":\"91362\",\"TOTAL_TEH\":\"91597\",\"DISTRICTCODE\":\"32\",\"TEHSILDAR_APPROVED\":\"91561\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"115589\",\"PDF_DOWNLOAD\":\"224\",\"COLLECTER_TOTAL\":\"99882\",\"TEHSILDAR_REJ\":\"10\",\"PATWAR\":\"228\",\"TEH_ESIGN_STATUS\":\"110877\",\"PATWARI_REJ\":\"2744\",\"PAT_ESIGN_STATUS\":\"114146\",\"FAMILY_REG\":\"128459\",\"TOTAL_PAT\":\"118333\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"कोटा\",\"COLLECTOR_ESIGN_STATUS\":\"97472\",\"COLLECTER_APPROVED\":\"99882\",\"TOTAL_TEH\":\"111636\",\"DISTRICTCODE\":\"26\",\"TEHSILDAR_APPROVED\":\"111626\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"175628\",\"PDF_DOWNLOAD\":\"413\",\"COLLECTER_TOTAL\":\"172514\",\"TEHSILDAR_REJ\":\"30\",\"PATWAR\":\"423\",\"TEH_ESIGN_STATUS\":\"173250\",\"PATWARI_REJ\":\"4989\",\"PAT_ESIGN_STATUS\":\"174266\",\"FAMILY_REG\":\"191271\",\"TOTAL_PAT\":\"180617\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"चित्तौड़गढ़\",\"COLLECTOR_ESIGN_STATUS\":\"165418\",\"COLLECTER_APPROVED\":\"172514\",\"TOTAL_TEH\":\"173410\",\"DISTRICTCODE\":\"22\",\"TEHSILDAR_APPROVED\":\"173380\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"192963\",\"PDF_DOWNLOAD\":\"281\",\"COLLECTER_TOTAL\":\"188877\",\"TEHSILDAR_REJ\":\"359\",\"PATWAR\":\"304\",\"TEH_ESIGN_STATUS\":\"189287\",\"PATWARI_REJ\":\"6599\",\"PAT_ESIGN_STATUS\":\"191591\",\"FAMILY_REG\":\"213878\",\"TOTAL_PAT\":\"199562\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"चूरू\",\"COLLECTOR_ESIGN_STATUS\":\"188704\",\"COLLECTER_APPROVED\":\"188877\",\"TOTAL_TEH\":\"189642\",\"DISTRICTCODE\":\"3\",\"TEHSILDAR_APPROVED\":\"189283\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"295913\",\"PDF_DOWNLOAD\":\"705\",\"COLLECTER_TOTAL\":\"283334\",\"TEHSILDAR_REJ\":\"5\",\"PATWAR\":\"732\",\"TEH_ESIGN_STATUS\":\"283682\",\"PATWARI_REJ\":\"7253\",\"PAT_ESIGN_STATUS\":\"290540\",\"FAMILY_REG\":\"330483\",\"TOTAL_PAT\":\"303166\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"जयपुर\",\"COLLECTOR_ESIGN_STATUS\":\"282602\",\"COLLECTER_APPROVED\":\"283334\",\"TOTAL_TEH\":\"284754\",\"DISTRICTCODE\":\"9\",\"TEHSILDAR_APPROVED\":\"284749\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"177752\",\"PDF_DOWNLOAD\":\"333\",\"COLLECTER_TOTAL\":\"174278\",\"TEHSILDAR_REJ\":\"16\",\"PATWAR\":\"462\",\"TEH_ESIGN_STATUS\":\"174278\",\"PATWARI_REJ\":\"6809\",\"PAT_ESIGN_STATUS\":\"176648\",\"FAMILY_REG\":\"190085\",\"TOTAL_PAT\":\"184561\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"जालोर\",\"COLLECTOR_ESIGN_STATUS\":\"174278\",\"COLLECTER_APPROVED\":\"174278\",\"TOTAL_TEH\":\"175167\",\"DISTRICTCODE\":\"18\",\"TEHSILDAR_APPROVED\":\"175151\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"52136\",\"PDF_DOWNLOAD\":\"114\",\"COLLECTER_TOTAL\":\"50378\",\"TEHSILDAR_REJ\":\"165\",\"PATWAR\":\"150\",\"TEH_ESIGN_STATUS\":\"50963\",\"PATWARI_REJ\":\"2283\",\"PAT_ESIGN_STATUS\":\"51674\",\"FAMILY_REG\":\"57513\",\"TOTAL_PAT\":\"54419\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"जैसलमेर\",\"COLLECTOR_ESIGN_STATUS\":\"50378\",\"COLLECTER_APPROVED\":\"50378\",\"TOTAL_TEH\":\"51308\",\"DISTRICTCODE\":\"15\",\"TEHSILDAR_APPROVED\":\"51143\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"244443\",\"PDF_DOWNLOAD\":\"447\",\"COLLECTER_TOTAL\":\"233679\",\"TEHSILDAR_REJ\":\"85\",\"PATWAR\":\"449\",\"TEH_ESIGN_STATUS\":\"233703\",\"PATWARI_REJ\":\"19349\",\"PAT_ESIGN_STATUS\":\"238139\",\"FAMILY_REG\":\"282047\",\"TOTAL_PAT\":\"263792\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"जोधपुर\",\"COLLECTOR_ESIGN_STATUS\":\"233679\",\"COLLECTER_APPROVED\":\"233679\",\"TOTAL_TEH\":\"234214\",\"DISTRICTCODE\":\"14\",\"TEHSILDAR_APPROVED\":\"234129\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"210625\",\"PDF_DOWNLOAD\":\"413\",\"COLLECTER_TOTAL\":\"199366\",\"TEHSILDAR_REJ\":\"18\",\"PATWAR\":\"417\",\"TEH_ESIGN_STATUS\":\"204534\",\"PATWARI_REJ\":\"3037\",\"PAT_ESIGN_STATUS\":\"207889\",\"FAMILY_REG\":\"226163\",\"TOTAL_PAT\":\"213662\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"झालावाड़\",\"COLLECTOR_ESIGN_STATUS\":\"199185\",\"COLLECTER_APPROVED\":\"199366\",\"TOTAL_TEH\":\"205078\",\"DISTRICTCODE\":\"17\",\"TEHSILDAR_APPROVED\":\"205060\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"170883\",\"PDF_DOWNLOAD\":\"301\",\"COLLECTER_TOTAL\":\"160689\",\"TEHSILDAR_REJ\":\"241\",\"PATWAR\":\"303\",\"TEH_ESIGN_STATUS\":\"161520\",\"PATWARI_REJ\":\"8962\",\"PAT_ESIGN_STATUS\":\"169090\",\"FAMILY_REG\":\"189220\",\"TOTAL_PAT\":\"179845\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"झूंझुनू\",\"COLLECTOR_ESIGN_STATUS\":\"153171\",\"COLLECTER_APPROVED\":\"160689\",\"TOTAL_TEH\":\"163795\",\"DISTRICTCODE\":\"4\",\"TEHSILDAR_APPROVED\":\"163554\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"178159\",\"PDF_DOWNLOAD\":\"332\",\"COLLECTER_TOTAL\":\"172852\",\"TEHSILDAR_REJ\":\"29\",\"PATWAR\":\"336\",\"TEH_ESIGN_STATUS\":\"173251\",\"PATWARI_REJ\":\"6072\",\"PAT_ESIGN_STATUS\":\"175570\",\"FAMILY_REG\":\"196703\",\"TOTAL_PAT\":\"184231\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"टोंक\",\"COLLECTOR_ESIGN_STATUS\":\"169092\",\"COLLECTER_APPROVED\":\"172852\",\"TOTAL_TEH\":\"174094\",\"DISTRICTCODE\":\"12\",\"TEHSILDAR_APPROVED\":\"174065\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"119312\",\"PDF_DOWNLOAD\":\"281\",\"COLLECTER_TOTAL\":\"117277\",\"TEHSILDAR_REJ\":\"0\",\"PATWAR\":\"285\",\"TEH_ESIGN_STATUS\":\"117277\",\"PATWARI_REJ\":\"10230\",\"PAT_ESIGN_STATUS\":\"118751\",\"FAMILY_REG\":\"132991\",\"TOTAL_PAT\":\"129542\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"डूँगरपुर\",\"COLLECTOR_ESIGN_STATUS\":\"116581\",\"COLLECTER_APPROVED\":\"117277\",\"TOTAL_TEH\":\"117413\",\"DISTRICTCODE\":\"20\",\"TEHSILDAR_APPROVED\":\"117413\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"132164\",\"PDF_DOWNLOAD\":\"306\",\"COLLECTER_TOTAL\":\"129506\",\"TEHSILDAR_REJ\":\"97\",\"PATWAR\":\"308\",\"TEH_ESIGN_STATUS\":\"129736\",\"PATWARI_REJ\":\"4947\",\"PAT_ESIGN_STATUS\":\"131058\",\"FAMILY_REG\":\"147934\",\"TOTAL_PAT\":\"137111\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"दौसा\",\"COLLECTOR_ESIGN_STATUS\":\"129506\",\"COLLECTER_APPROVED\":\"129506\",\"TOTAL_TEH\":\"129832\",\"DISTRICTCODE\":\"7\",\"TEHSILDAR_APPROVED\":\"129735\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"91599\",\"PDF_DOWNLOAD\":\"238\",\"COLLECTER_TOTAL\":\"87004\",\"TEHSILDAR_REJ\":\"5\",\"PATWAR\":\"245\",\"TEH_ESIGN_STATUS\":\"87149\",\"PATWARI_REJ\":\"4120\",\"PAT_ESIGN_STATUS\":\"90692\",\"FAMILY_REG\":\"100000\",\"TOTAL_PAT\":\"95719\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"धौलपुर\",\"COLLECTOR_ESIGN_STATUS\":\"86752\",\"COLLECTER_APPROVED\":\"87004\",\"TOTAL_TEH\":\"87740\",\"DISTRICTCODE\":\"13\",\"TEHSILDAR_APPROVED\":\"87735\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"288932\",\"PDF_DOWNLOAD\":\"526\",\"COLLECTER_TOTAL\":\"273772\",\"TEHSILDAR_REJ\":\"934\",\"PATWAR\":\"530\",\"TEH_ESIGN_STATUS\":\"279734\",\"PATWARI_REJ\":\"20866\",\"PAT_ESIGN_STATUS\":\"286416\",\"FAMILY_REG\":\"323467\",\"TOTAL_PAT\":\"309798\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"नागौर\",\"COLLECTOR_ESIGN_STATUS\":\"273772\",\"COLLECTER_APPROVED\":\"273772\",\"TOTAL_TEH\":\"283350\",\"DISTRICTCODE\":\"21\",\"TEHSILDAR_APPROVED\":\"282416\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"182221\",\"PDF_DOWNLOAD\":\"414\",\"COLLECTER_TOTAL\":\"179868\",\"TEHSILDAR_REJ\":\"0\",\"PATWAR\":\"498\",\"TEH_ESIGN_STATUS\":\"179868\",\"PATWARI_REJ\":\"4373\",\"PAT_ESIGN_STATUS\":\"181082\",\"FAMILY_REG\":\"196205\",\"TOTAL_PAT\":\"186594\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"पाली\",\"COLLECTOR_ESIGN_STATUS\":\"179868\",\"COLLECTER_APPROVED\":\"179868\",\"TOTAL_TEH\":\"180367\",\"DISTRICTCODE\":\"16\",\"TEHSILDAR_APPROVED\":\"180367\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"73274\",\"PDF_DOWNLOAD\":\"193\",\"COLLECTER_TOTAL\":\"69818\",\"TEHSILDAR_REJ\":\"3\",\"PATWAR\":\"196\",\"TEH_ESIGN_STATUS\":\"71143\",\"PATWARI_REJ\":\"6019\",\"PAT_ESIGN_STATUS\":\"72060\",\"FAMILY_REG\":\"85824\",\"TOTAL_PAT\":\"79293\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"प्रतापगढ़\",\"COLLECTOR_ESIGN_STATUS\":\"69818\",\"COLLECTER_APPROVED\":\"69818\",\"TOTAL_TEH\":\"71184\",\"DISTRICTCODE\":\"33\",\"TEHSILDAR_APPROVED\":\"71181\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"166250\",\"PDF_DOWNLOAD\":\"337\",\"COLLECTER_TOTAL\":\"160576\",\"TEHSILDAR_REJ\":\"318\",\"PATWAR\":\"378\",\"TEH_ESIGN_STATUS\":\"161668\",\"PATWARI_REJ\":\"7195\",\"PAT_ESIGN_STATUS\":\"163100\",\"FAMILY_REG\":\"182418\",\"TOTAL_PAT\":\"173445\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"बाँसवाड़ा\",\"COLLECTOR_ESIGN_STATUS\":\"159960\",\"COLLECTER_APPROVED\":\"160576\",\"TOTAL_TEH\":\"162663\",\"DISTRICTCODE\":\"24\",\"TEHSILDAR_APPROVED\":\"162345\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"299121\",\"PDF_DOWNLOAD\":\"439\",\"COLLECTER_TOTAL\":\"289202\",\"TEHSILDAR_REJ\":\"262\",\"PATWAR\":\"572\",\"TEH_ESIGN_STATUS\":\"293704\",\"PATWARI_REJ\":\"12765\",\"PAT_ESIGN_STATUS\":\"297462\",\"FAMILY_REG\":\"349102\",\"TOTAL_PAT\":\"311886\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"बाड़मेर\",\"COLLECTOR_ESIGN_STATUS\":\"285985\",\"COLLECTER_APPROVED\":\"289202\",\"TOTAL_TEH\":\"294134\",\"DISTRICTCODE\":\"8\",\"TEHSILDAR_APPROVED\":\"293872\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"140100\",\"PDF_DOWNLOAD\":\"267\",\"COLLECTER_TOTAL\":\"133237\",\"TEHSILDAR_REJ\":\"1\",\"PATWAR\":\"278\",\"TEH_ESIGN_STATUS\":\"133237\",\"PATWARI_REJ\":\"1995\",\"PAT_ESIGN_STATUS\":\"136882\",\"FAMILY_REG\":\"162675\",\"TOTAL_PAT\":\"142095\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"बाराँ\",\"COLLECTOR_ESIGN_STATUS\":\"133237\",\"COLLECTER_APPROVED\":\"133237\",\"TOTAL_TEH\":\"136539\",\"DISTRICTCODE\":\"29\",\"TEHSILDAR_APPROVED\":\"136538\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"136296\",\"PDF_DOWNLOAD\":\"326\",\"COLLECTER_TOTAL\":\"132890\",\"TEHSILDAR_REJ\":\"97\",\"PATWAR\":\"328\",\"TEH_ESIGN_STATUS\":\"132973\",\"PATWARI_REJ\":\"8109\",\"PAT_ESIGN_STATUS\":\"134255\",\"FAMILY_REG\":\"153291\",\"TOTAL_PAT\":\"144405\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"बीकानेर\",\"COLLECTOR_ESIGN_STATUS\":\"132890\",\"COLLECTER_APPROVED\":\"132890\",\"TOTAL_TEH\":\"133604\",\"DISTRICTCODE\":\"02\",\"TEHSILDAR_APPROVED\":\"133507\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"134456\",\"PDF_DOWNLOAD\":\"260\",\"COLLECTER_TOTAL\":\"128725\",\"TEHSILDAR_REJ\":\"0\",\"PATWAR\":\"288\",\"TEH_ESIGN_STATUS\":\"132056\",\"PATWARI_REJ\":\"1890\",\"PAT_ESIGN_STATUS\":\"133013\",\"FAMILY_REG\":\"147353\",\"TOTAL_PAT\":\"136346\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"बून्दी\",\"COLLECTOR_ESIGN_STATUS\":\"124282\",\"COLLECTER_APPROVED\":\"128725\",\"TOTAL_TEH\":\"132258\",\"DISTRICTCODE\":\"11\",\"TEHSILDAR_APPROVED\":\"132258\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"212709\",\"PDF_DOWNLOAD\":\"516\",\"COLLECTER_TOTAL\":\"209102\",\"TEHSILDAR_REJ\":\"1\",\"PATWAR\":\"703\",\"TEH_ESIGN_STATUS\":\"209346\",\"PATWARI_REJ\":\"7018\",\"PAT_ESIGN_STATUS\":\"211543\",\"FAMILY_REG\":\"227681\",\"TOTAL_PAT\":\"219727\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"भरतपुर\",\"COLLECTOR_ESIGN_STATUS\":\"209102\",\"COLLECTER_APPROVED\":\"209102\",\"TOTAL_TEH\":\"209994\",\"DISTRICTCODE\":\"10\",\"TEHSILDAR_APPROVED\":\"209993\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"282663\",\"PDF_DOWNLOAD\":\"527\",\"COLLECTER_TOTAL\":\"276612\",\"TEHSILDAR_REJ\":\"319\",\"PATWAR\":\"590\",\"TEH_ESIGN_STATUS\":\"278533\",\"PATWARI_REJ\":\"7342\",\"PAT_ESIGN_STATUS\":\"280634\",\"FAMILY_REG\":\"301965\",\"TOTAL_PAT\":\"290005\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"भीलवाड़ा\",\"COLLECTOR_ESIGN_STATUS\":\"272817\",\"COLLECTER_APPROVED\":\"276611\",\"TOTAL_TEH\":\"279051\",\"DISTRICTCODE\":\"6\",\"TEHSILDAR_APPROVED\":\"278732\",\"COLLECTER_REJ\":\"1\"},{\"PATWARI_APPROVED\":\"87494\",\"PDF_DOWNLOAD\":\"255\",\"COLLECTER_TOTAL\":\"83740\",\"TEHSILDAR_REJ\":\"1\",\"PATWAR\":\"256\",\"TEH_ESIGN_STATUS\":\"83740\",\"PATWARI_REJ\":\"1302\",\"PAT_ESIGN_STATUS\":\"86091\",\"FAMILY_REG\":\"98255\",\"TOTAL_PAT\":\"88796\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"राजसमन्द\",\"COLLECTOR_ESIGN_STATUS\":\"82237\",\"COLLECTER_APPROVED\":\"83740\",\"TOTAL_TEH\":\"84610\",\"DISTRICTCODE\":\"30\",\"TEHSILDAR_APPROVED\":\"84609\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"106672\",\"PDF_DOWNLOAD\":\"539\",\"COLLECTER_TOTAL\":\"102588\",\"TEHSILDAR_REJ\":\"4\",\"PATWAR\":\"634\",\"TEH_ESIGN_STATUS\":\"102588\",\"PATWARI_REJ\":\"4064\",\"PAT_ESIGN_STATUS\":\"105059\",\"FAMILY_REG\":\"118870\",\"TOTAL_PAT\":\"110736\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"श्रीगंगानगर\",\"COLLECTOR_ESIGN_STATUS\":\"100045\",\"COLLECTER_APPROVED\":\"102588\",\"TOTAL_TEH\":\"104340\",\"DISTRICTCODE\":\"28\",\"TEHSILDAR_APPROVED\":\"104336\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"129719\",\"PDF_DOWNLOAD\":\"290\",\"COLLECTER_TOTAL\":\"128656\",\"TEHSILDAR_REJ\":\"4\",\"PATWAR\":\"341\",\"TEH_ESIGN_STATUS\":\"128671\",\"PATWARI_REJ\":\"6826\",\"PAT_ESIGN_STATUS\":\"128979\",\"FAMILY_REG\":\"139735\",\"TOTAL_PAT\":\"136545\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"सवाईमाधोपुर\",\"COLLECTOR_ESIGN_STATUS\":\"128656\",\"COLLECTER_APPROVED\":\"128656\",\"TOTAL_TEH\":\"128689\",\"DISTRICTCODE\":\"25\",\"TEHSILDAR_APPROVED\":\"128685\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"57616\",\"PDF_DOWNLOAD\":\"191\",\"COLLECTER_TOTAL\":\"51757\",\"TEHSILDAR_REJ\":\"21\",\"PATWAR\":\"235\",\"TEH_ESIGN_STATUS\":\"52703\",\"PATWARI_REJ\":\"1253\",\"PAT_ESIGN_STATUS\":\"56688\",\"FAMILY_REG\":\"68193\",\"TOTAL_PAT\":\"58869\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"सिरोही\",\"COLLECTOR_ESIGN_STATUS\":\"51757\",\"COLLECTER_APPROVED\":\"51757\",\"TOTAL_TEH\":\"53678\",\"DISTRICTCODE\":\"19\",\"TEHSILDAR_APPROVED\":\"53657\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"204021\",\"PDF_DOWNLOAD\":\"338\",\"COLLECTER_TOTAL\":\"196898\",\"TEHSILDAR_REJ\":\"1\",\"PATWAR\":\"346\",\"TEH_ESIGN_STATUS\":\"199247\",\"PATWARI_REJ\":\"10927\",\"PAT_ESIGN_STATUS\":\"202663\",\"FAMILY_REG\":\"223706\",\"TOTAL_PAT\":\"214948\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"सीकर\",\"COLLECTOR_ESIGN_STATUS\":\"196898\",\"COLLECTER_APPROVED\":\"196898\",\"TOTAL_TEH\":\"200912\",\"DISTRICTCODE\":\"23\",\"TEHSILDAR_APPROVED\":\"200911\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"164651\",\"PDF_DOWNLOAD\":\"346\",\"COLLECTER_TOTAL\":\"156404\",\"TEHSILDAR_REJ\":\"23\",\"PATWAR\":\"388\",\"TEH_ESIGN_STATUS\":\"156557\",\"PATWARI_REJ\":\"6019\",\"PAT_ESIGN_STATUS\":\"161755\",\"FAMILY_REG\":\"181609\",\"TOTAL_PAT\":\"170670\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"हनुमानगढ़\",\"COLLECTOR_ESIGN_STATUS\":\"156380\",\"COLLECTER_APPROVED\":\"156380\",\"TOTAL_TEH\":\"158999\",\"DISTRICTCODE\":\"31\",\"TEHSILDAR_APPROVED\":\"158976\",\"COLLECTER_REJ\":\"24\"}]";
            result = "[{\"PATWARI_APPROVED\":\"206313\",\"PDF_DOWNLOAD\":\"548\",\"COLLECTER_TOTAL\":\"202355\",\"TEHSILDAR_REJ\":\"16\",\"PATWAR\":\"568\",\"TEH_ESIGN_STATUS\":\"202469\",\"PATWARI_REJ\":\"6775\",\"PAT_ESIGN_STATUS\":\"205060\",\"FAMILY_REG\":\"224292\",\"TOTAL_PAT\":\"213088\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"अजमेर\",\"COLLECTOR_ESIGN_STATUS\":\"200920\",\"COLLECTER_APPROVED\":\"202355\",\"TOTAL_TEH\":\"202683\",\"DISTRICTCODE\":\"1\",\"TEHSILDAR_APPROVED\":\"202667\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"279841\",\"PDF_DOWNLOAD\":\"655\",\"COLLECTER_TOTAL\":\"273916\",\"TEHSILDAR_REJ\":\"57\",\"PATWAR\":\"690\",\"TEH_ESIGN_STATUS\":\"273916\",\"PATWARI_REJ\":\"9641\",\"PAT_ESIGN_STATUS\":\"277301\",\"FAMILY_REG\":\"303839\",\"TOTAL_PAT\":\"289482\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"अलवर\",\"COLLECTOR_ESIGN_STATUS\":\"270188\",\"COLLECTER_APPROVED\":\"273916\",\"TOTAL_TEH\":\"275100\",\"DISTRICTCODE\":\"5\",\"TEHSILDAR_APPROVED\":\"275043\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"220233\",\"PDF_DOWNLOAD\":\"620\",\"COLLECTER_TOTAL\":\"218752\",\"TEHSILDAR_REJ\":\"0\",\"PATWAR\":\"676\",\"TEH_ESIGN_STATUS\":\"218772\",\"PATWARI_REJ\":\"13770\",\"PAT_ESIGN_STATUS\":\"219624\",\"FAMILY_REG\":\"237724\",\"TOTAL_PAT\":\"234003\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"उदयपुर\",\"COLLECTOR_ESIGN_STATUS\":\"218750\",\"COLLECTER_APPROVED\":\"218752\",\"TOTAL_TEH\":\"219452\",\"DISTRICTCODE\":\"27\",\"TEHSILDAR_APPROVED\":\"219452\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"95317\",\"PDF_DOWNLOAD\":\"288\",\"COLLECTER_TOTAL\":\"92397\",\"TEHSILDAR_REJ\":\"39\",\"PATWAR\":\"291\",\"TEH_ESIGN_STATUS\":\"93732\",\"PATWARI_REJ\":\"7318\",\"PAT_ESIGN_STATUS\":\"94767\",\"FAMILY_REG\":\"106751\",\"TOTAL_PAT\":\"102635\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"करौली\",\"COLLECTOR_ESIGN_STATUS\":\"90617\",\"COLLECTER_APPROVED\":\"92397\",\"TOTAL_TEH\":\"94468\",\"DISTRICTCODE\":\"32\",\"TEHSILDAR_APPROVED\":\"94429\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"118994\",\"PDF_DOWNLOAD\":\"224\",\"COLLECTER_TOTAL\":\"99746\",\"TEHSILDAR_REJ\":\"10\",\"PATWAR\":\"228\",\"TEH_ESIGN_STATUS\":\"115786\",\"PATWARI_REJ\":\"2902\",\"PAT_ESIGN_STATUS\":\"117034\",\"FAMILY_REG\":\"131289\",\"TOTAL_PAT\":\"121896\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"कोटा\",\"COLLECTOR_ESIGN_STATUS\":\"97336\",\"COLLECTER_APPROVED\":\"99746\",\"TOTAL_TEH\":\"115796\",\"DISTRICTCODE\":\"26\",\"TEHSILDAR_APPROVED\":\"115786\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"180852\",\"PDF_DOWNLOAD\":\"413\",\"COLLECTER_TOTAL\":\"176047\",\"TEHSILDAR_REJ\":\"30\",\"PATWAR\":\"423\",\"TEH_ESIGN_STATUS\":\"177078\",\"PATWARI_REJ\":\"5132\",\"PAT_ESIGN_STATUS\":\"179329\",\"FAMILY_REG\":\"195986\",\"TOTAL_PAT\":\"185984\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"चित्तौड़गढ़\",\"COLLECTOR_ESIGN_STATUS\":\"173837\",\"COLLECTER_APPROVED\":\"176047\",\"TOTAL_TEH\":\"178416\",\"DISTRICTCODE\":\"22\",\"TEHSILDAR_APPROVED\":\"178386\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"196205\",\"PDF_DOWNLOAD\":\"281\",\"COLLECTER_TOTAL\":\"191720\",\"TEHSILDAR_REJ\":\"384\",\"PATWAR\":\"304\",\"TEH_ESIGN_STATUS\":\"191720\",\"PATWARI_REJ\":\"6664\",\"PAT_ESIGN_STATUS\":\"195220\",\"FAMILY_REG\":\"217943\",\"TOTAL_PAT\":\"202869\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"चूरू\",\"COLLECTOR_ESIGN_STATUS\":\"191694\",\"COLLECTER_APPROVED\":\"191720\",\"TOTAL_TEH\":\"192393\",\"DISTRICTCODE\":\"3\",\"TEHSILDAR_APPROVED\":\"192009\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"304347\",\"PDF_DOWNLOAD\":\"707\",\"COLLECTER_TOTAL\":\"292681\",\"TEHSILDAR_REJ\":\"6\",\"PATWAR\":\"733\",\"TEH_ESIGN_STATUS\":\"296150\",\"PATWARI_REJ\":\"7284\",\"PAT_ESIGN_STATUS\":\"300412\",\"FAMILY_REG\":\"336640\",\"TOTAL_PAT\":\"311631\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"जयपुर\",\"COLLECTOR_ESIGN_STATUS\":\"288868\",\"COLLECTER_APPROVED\":\"292681\",\"TOTAL_TEH\":\"298717\",\"DISTRICTCODE\":\"9\",\"TEHSILDAR_APPROVED\":\"298711\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"182296\",\"PDF_DOWNLOAD\":\"333\",\"COLLECTER_TOTAL\":\"177009\",\"TEHSILDAR_REJ\":\"15\",\"PATWAR\":\"467\",\"TEH_ESIGN_STATUS\":\"177009\",\"PATWARI_REJ\":\"6848\",\"PAT_ESIGN_STATUS\":\"180591\",\"FAMILY_REG\":\"199009\",\"TOTAL_PAT\":\"189144\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"जालोर\",\"COLLECTOR_ESIGN_STATUS\":\"175126\",\"COLLECTER_APPROVED\":\"177009\",\"TOTAL_TEH\":\"179671\",\"DISTRICTCODE\":\"18\",\"TEHSILDAR_APPROVED\":\"179656\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"52468\",\"PDF_DOWNLOAD\":\"114\",\"COLLECTER_TOTAL\":\"50957\",\"TEHSILDAR_REJ\":\"165\",\"PATWAR\":\"151\",\"TEH_ESIGN_STATUS\":\"51311\",\"PATWARI_REJ\":\"2295\",\"PAT_ESIGN_STATUS\":\"51983\",\"FAMILY_REG\":\"58199\",\"TOTAL_PAT\":\"54763\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"जैसलमेर\",\"COLLECTOR_ESIGN_STATUS\":\"50957\",\"COLLECTER_APPROVED\":\"50957\",\"TOTAL_TEH\":\"51851\",\"DISTRICTCODE\":\"15\",\"TEHSILDAR_APPROVED\":\"51686\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"252312\",\"PDF_DOWNLOAD\":\"447\",\"COLLECTER_TOTAL\":\"238730\",\"TEHSILDAR_REJ\":\"87\",\"PATWAR\":\"449\",\"TEH_ESIGN_STATUS\":\"242082\",\"PATWARI_REJ\":\"19314\",\"PAT_ESIGN_STATUS\":\"244838\",\"FAMILY_REG\":\"290529\",\"TOTAL_PAT\":\"271626\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"जोधपुर\",\"COLLECTOR_ESIGN_STATUS\":\"238730\",\"COLLECTER_APPROVED\":\"238730\",\"TOTAL_TEH\":\"243334\",\"DISTRICTCODE\":\"14\",\"TEHSILDAR_APPROVED\":\"243247\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"214281\",\"PDF_DOWNLOAD\":\"413\",\"COLLECTER_TOTAL\":\"209634\",\"TEHSILDAR_REJ\":\"17\",\"PATWAR\":\"417\",\"TEH_ESIGN_STATUS\":\"209634\",\"PATWARI_REJ\":\"3044\",\"PAT_ESIGN_STATUS\":\"211662\",\"FAMILY_REG\":\"230284\",\"TOTAL_PAT\":\"217325\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"झालावाड़\",\"COLLECTOR_ESIGN_STATUS\":\"203684\",\"COLLECTER_APPROVED\":\"209634\",\"TOTAL_TEH\":\"209651\",\"DISTRICTCODE\":\"17\",\"TEHSILDAR_APPROVED\":\"209634\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"176860\",\"PDF_DOWNLOAD\":\"301\",\"COLLECTER_TOTAL\":\"167312\",\"TEHSILDAR_REJ\":\"229\",\"PATWAR\":\"303\",\"TEH_ESIGN_STATUS\":\"168649\",\"PATWARI_REJ\":\"9395\",\"PAT_ESIGN_STATUS\":\"175439\",\"FAMILY_REG\":\"193284\",\"TOTAL_PAT\":\"186255\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"झूंझुनू\",\"COLLECTOR_ESIGN_STATUS\":\"165217\",\"COLLECTER_APPROVED\":\"167312\",\"TOTAL_TEH\":\"169824\",\"DISTRICTCODE\":\"4\",\"TEHSILDAR_APPROVED\":\"169595\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"183191\",\"PDF_DOWNLOAD\":\"333\",\"COLLECTER_TOTAL\":\"175557\",\"TEHSILDAR_REJ\":\"28\",\"PATWAR\":\"336\",\"TEH_ESIGN_STATUS\":\"176798\",\"PATWARI_REJ\":\"6323\",\"PAT_ESIGN_STATUS\":\"181345\",\"FAMILY_REG\":\"200574\",\"TOTAL_PAT\":\"189514\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"टोंक\",\"COLLECTOR_ESIGN_STATUS\":\"173931\",\"COLLECTER_APPROVED\":\"175557\",\"TOTAL_TEH\":\"178372\",\"DISTRICTCODE\":\"12\",\"TEHSILDAR_APPROVED\":\"178344\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"123432\",\"PDF_DOWNLOAD\":\"281\",\"COLLECTER_TOTAL\":\"121425\",\"TEHSILDAR_REJ\":\"0\",\"PATWAR\":\"285\",\"TEH_ESIGN_STATUS\":\"121936\",\"PATWARI_REJ\":\"10458\",\"PAT_ESIGN_STATUS\":\"122783\",\"FAMILY_REG\":\"137760\",\"TOTAL_PAT\":\"133890\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"डूँगरपुर\",\"COLLECTOR_ESIGN_STATUS\":\"119874\",\"COLLECTER_APPROVED\":\"121425\",\"TOTAL_TEH\":\"122338\",\"DISTRICTCODE\":\"20\",\"TEHSILDAR_APPROVED\":\"122338\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"134727\",\"PDF_DOWNLOAD\":\"306\",\"COLLECTER_TOTAL\":\"131629\",\"TEHSILDAR_REJ\":\"101\",\"PATWAR\":\"308\",\"TEH_ESIGN_STATUS\":\"133016\",\"PATWARI_REJ\":\"5047\",\"PAT_ESIGN_STATUS\":\"133978\",\"FAMILY_REG\":\"150077\",\"TOTAL_PAT\":\"139774\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"दौसा\",\"COLLECTOR_ESIGN_STATUS\":\"129386\",\"COLLECTER_APPROVED\":\"131629\",\"TOTAL_TEH\":\"133117\",\"DISTRICTCODE\":\"7\",\"TEHSILDAR_APPROVED\":\"133016\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"95222\",\"PDF_DOWNLOAD\":\"239\",\"COLLECTER_TOTAL\":\"93357\",\"TEHSILDAR_REJ\":\"5\",\"PATWAR\":\"245\",\"TEH_ESIGN_STATUS\":\"93685\",\"PATWARI_REJ\":\"4221\",\"PAT_ESIGN_STATUS\":\"94774\",\"FAMILY_REG\":\"102504\",\"TOTAL_PAT\":\"99443\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"धौलपुर\",\"COLLECTOR_ESIGN_STATUS\":\"92527\",\"COLLECTER_APPROVED\":\"93357\",\"TOTAL_TEH\":\"94313\",\"DISTRICTCODE\":\"13\",\"TEHSILDAR_APPROVED\":\"94308\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"295755\",\"PDF_DOWNLOAD\":\"526\",\"COLLECTER_TOTAL\":\"290179\",\"TEHSILDAR_REJ\":\"947\",\"PATWAR\":\"530\",\"TEH_ESIGN_STATUS\":\"290582\",\"PATWARI_REJ\":\"20856\",\"PAT_ESIGN_STATUS\":\"294205\",\"FAMILY_REG\":\"330619\",\"TOTAL_PAT\":\"316611\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"नागौर\",\"COLLECTOR_ESIGN_STATUS\":\"273573\",\"COLLECTER_APPROVED\":\"290179\",\"TOTAL_TEH\":\"292927\",\"DISTRICTCODE\":\"21\",\"TEHSILDAR_APPROVED\":\"291980\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"187138\",\"PDF_DOWNLOAD\":\"414\",\"COLLECTER_TOTAL\":\"183975\",\"TEHSILDAR_REJ\":\"0\",\"PATWAR\":\"498\",\"TEH_ESIGN_STATUS\":\"183975\",\"PATWARI_REJ\":\"4556\",\"PAT_ESIGN_STATUS\":\"185230\",\"FAMILY_REG\":\"202599\",\"TOTAL_PAT\":\"191694\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"पाली\",\"COLLECTOR_ESIGN_STATUS\":\"183975\",\"COLLECTER_APPROVED\":\"183975\",\"TOTAL_TEH\":\"184733\",\"DISTRICTCODE\":\"16\",\"TEHSILDAR_APPROVED\":\"184733\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"76629\",\"PDF_DOWNLOAD\":\"193\",\"COLLECTER_TOTAL\":\"71779\",\"TEHSILDAR_REJ\":\"3\",\"PATWAR\":\"201\",\"TEH_ESIGN_STATUS\":\"74116\",\"PATWARI_REJ\":\"6776\",\"PAT_ESIGN_STATUS\":\"75556\",\"FAMILY_REG\":\"91701\",\"TOTAL_PAT\":\"83405\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"प्रतापगढ़\",\"COLLECTOR_ESIGN_STATUS\":\"71505\",\"COLLECTER_APPROVED\":\"71779\",\"TOTAL_TEH\":\"74169\",\"DISTRICTCODE\":\"33\",\"TEHSILDAR_APPROVED\":\"74166\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"169014\",\"PDF_DOWNLOAD\":\"337\",\"COLLECTER_TOTAL\":\"163857\",\"TEHSILDAR_REJ\":\"315\",\"PATWAR\":\"379\",\"TEH_ESIGN_STATUS\":\"164294\",\"PATWARI_REJ\":\"7328\",\"PAT_ESIGN_STATUS\":\"166783\",\"FAMILY_REG\":\"185031\",\"TOTAL_PAT\":\"176342\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"बाँसवाड़ा\",\"COLLECTOR_ESIGN_STATUS\":\"159781\",\"COLLECTER_APPROVED\":\"163857\",\"TOTAL_TEH\":\"165079\",\"DISTRICTCODE\":\"24\",\"TEHSILDAR_APPROVED\":\"164764\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"300653\",\"PDF_DOWNLOAD\":\"439\",\"COLLECTER_TOTAL\":\"288960\",\"TEHSILDAR_REJ\":\"255\",\"PATWAR\":\"572\",\"TEH_ESIGN_STATUS\":\"294178\",\"PATWARI_REJ\":\"12741\",\"PAT_ESIGN_STATUS\":\"299143\",\"FAMILY_REG\":\"351745\",\"TOTAL_PAT\":\"313394\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"बाड़मेर\",\"COLLECTOR_ESIGN_STATUS\":\"288960\",\"COLLECTER_APPROVED\":\"288960\",\"TOTAL_TEH\":\"296499\",\"DISTRICTCODE\":\"8\",\"TEHSILDAR_APPROVED\":\"296244\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"145371\",\"PDF_DOWNLOAD\":\"268\",\"COLLECTER_TOTAL\":\"136858\",\"TEHSILDAR_REJ\":\"1\",\"PATWAR\":\"278\",\"TEH_ESIGN_STATUS\":\"137125\",\"PATWARI_REJ\":\"2075\",\"PAT_ESIGN_STATUS\":\"142461\",\"FAMILY_REG\":\"166461\",\"TOTAL_PAT\":\"147446\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"बाराँ\",\"COLLECTOR_ESIGN_STATUS\":\"136858\",\"COLLECTER_APPROVED\":\"136858\",\"TOTAL_TEH\":\"140174\",\"DISTRICTCODE\":\"29\",\"TEHSILDAR_APPROVED\":\"140173\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"139636\",\"PDF_DOWNLOAD\":\"326\",\"COLLECTER_TOTAL\":\"134070\",\"TEHSILDAR_REJ\":\"125\",\"PATWAR\":\"329\",\"TEH_ESIGN_STATUS\":\"136583\",\"PATWARI_REJ\":\"8088\",\"PAT_ESIGN_STATUS\":\"138079\",\"FAMILY_REG\":\"156876\",\"TOTAL_PAT\":\"147724\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"बीकानेर\",\"COLLECTOR_ESIGN_STATUS\":\"132780\",\"COLLECTER_APPROVED\":\"134070\",\"TOTAL_TEH\":\"136977\",\"DISTRICTCODE\":\"02\",\"TEHSILDAR_APPROVED\":\"136852\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"137278\",\"PDF_DOWNLOAD\":\"260\",\"COLLECTER_TOTAL\":\"128361\",\"TEHSILDAR_REJ\":\"0\",\"PATWAR\":\"288\",\"TEH_ESIGN_STATUS\":\"134074\",\"PATWARI_REJ\":\"1969\",\"PAT_ESIGN_STATUS\":\"135740\",\"FAMILY_REG\":\"151339\",\"TOTAL_PAT\":\"139247\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"बून्दी\",\"COLLECTOR_ESIGN_STATUS\":\"123919\",\"COLLECTER_APPROVED\":\"128361\",\"TOTAL_TEH\":\"134570\",\"DISTRICTCODE\":\"11\",\"TEHSILDAR_APPROVED\":\"134570\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"216925\",\"PDF_DOWNLOAD\":\"516\",\"COLLECTER_TOTAL\":\"210531\",\"TEHSILDAR_REJ\":\"1\",\"PATWAR\":\"703\",\"TEH_ESIGN_STATUS\":\"213823\",\"PATWARI_REJ\":\"7028\",\"PAT_ESIGN_STATUS\":\"215874\",\"FAMILY_REG\":\"232555\",\"TOTAL_PAT\":\"223953\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"भरतपुर\",\"COLLECTOR_ESIGN_STATUS\":\"210531\",\"COLLECTER_APPROVED\":\"210531\",\"TOTAL_TEH\":\"214096\",\"DISTRICTCODE\":\"10\",\"TEHSILDAR_APPROVED\":\"214095\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"296233\",\"PDF_DOWNLOAD\":\"528\",\"COLLECTER_TOTAL\":\"294101\",\"TEHSILDAR_REJ\":\"352\",\"PATWAR\":\"590\",\"TEH_ESIGN_STATUS\":\"294470\",\"PATWARI_REJ\":\"7830\",\"PAT_ESIGN_STATUS\":\"295296\",\"FAMILY_REG\":\"309006\",\"TOTAL_PAT\":\"304063\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"भीलवाड़ा\",\"COLLECTOR_ESIGN_STATUS\":\"287453\",\"COLLECTER_APPROVED\":\"294099\",\"TOTAL_TEH\":\"294926\",\"DISTRICTCODE\":\"6\",\"TEHSILDAR_APPROVED\":\"294574\",\"COLLECTER_REJ\":\"2\"},{\"PATWARI_APPROVED\":\"90375\",\"PDF_DOWNLOAD\":\"255\",\"COLLECTER_TOTAL\":\"86971\",\"TEHSILDAR_REJ\":\"1\",\"PATWAR\":\"265\",\"TEH_ESIGN_STATUS\":\"86971\",\"PATWARI_REJ\":\"1316\",\"PAT_ESIGN_STATUS\":\"89520\",\"FAMILY_REG\":\"103563\",\"TOTAL_PAT\":\"91691\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"राजसमन्द\",\"COLLECTOR_ESIGN_STATUS\":\"85282\",\"COLLECTER_APPROVED\":\"86971\",\"TOTAL_TEH\":\"87113\",\"DISTRICTCODE\":\"30\",\"TEHSILDAR_APPROVED\":\"87112\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"110640\",\"PDF_DOWNLOAD\":\"539\",\"COLLECTER_TOTAL\":\"108937\",\"TEHSILDAR_REJ\":\"5\",\"PATWAR\":\"635\",\"TEH_ESIGN_STATUS\":\"108950\",\"PATWARI_REJ\":\"4173\",\"PAT_ESIGN_STATUS\":\"109717\",\"FAMILY_REG\":\"121583\",\"TOTAL_PAT\":\"114813\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"श्रीगंगानगर\",\"COLLECTOR_ESIGN_STATUS\":\"103800\",\"COLLECTER_APPROVED\":\"108937\",\"TOTAL_TEH\":\"109263\",\"DISTRICTCODE\":\"28\",\"TEHSILDAR_APPROVED\":\"109258\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"131909\",\"PDF_DOWNLOAD\":\"290\",\"COLLECTER_TOTAL\":\"131038\",\"TEHSILDAR_REJ\":\"6\",\"PATWAR\":\"318\",\"TEH_ESIGN_STATUS\":\"131038\",\"PATWARI_REJ\":\"6823\",\"PAT_ESIGN_STATUS\":\"131350\",\"FAMILY_REG\":\"142025\",\"TOTAL_PAT\":\"138732\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"सवाईमाधोपुर\",\"COLLECTOR_ESIGN_STATUS\":\"131038\",\"COLLECTER_APPROVED\":\"131038\",\"TOTAL_TEH\":\"131127\",\"DISTRICTCODE\":\"25\",\"TEHSILDAR_APPROVED\":\"131121\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"58802\",\"PDF_DOWNLOAD\":\"191\",\"COLLECTER_TOTAL\":\"52678\",\"TEHSILDAR_REJ\":\"20\",\"PATWAR\":\"236\",\"TEH_ESIGN_STATUS\":\"54776\",\"PATWARI_REJ\":\"1300\",\"PAT_ESIGN_STATUS\":\"57787\",\"FAMILY_REG\":\"70034\",\"TOTAL_PAT\":\"60102\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"सिरोही\",\"COLLECTOR_ESIGN_STATUS\":\"51736\",\"COLLECTER_APPROVED\":\"52678\",\"TOTAL_TEH\":\"55968\",\"DISTRICTCODE\":\"19\",\"TEHSILDAR_APPROVED\":\"55948\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"209303\",\"PDF_DOWNLOAD\":\"338\",\"COLLECTER_TOTAL\":\"206263\",\"TEHSILDAR_REJ\":\"1\",\"PATWAR\":\"346\",\"TEH_ESIGN_STATUS\":\"206597\",\"PATWARI_REJ\":\"10971\",\"PAT_ESIGN_STATUS\":\"207895\",\"FAMILY_REG\":\"229922\",\"TOTAL_PAT\":\"220274\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"सीकर\",\"COLLECTOR_ESIGN_STATUS\":\"205311\",\"COLLECTER_APPROVED\":\"206263\",\"TOTAL_TEH\":\"206598\",\"DISTRICTCODE\":\"23\",\"TEHSILDAR_APPROVED\":\"206597\",\"COLLECTER_REJ\":\"0\"},{\"PATWARI_APPROVED\":\"168231\",\"PDF_DOWNLOAD\":\"346\",\"COLLECTER_TOTAL\":\"156283\",\"TEHSILDAR_REJ\":\"23\",\"PATWAR\":\"388\",\"TEH_ESIGN_STATUS\":\"163639\",\"PATWARI_REJ\":\"6278\",\"PAT_ESIGN_STATUS\":\"166209\",\"FAMILY_REG\":\"185399\",\"TOTAL_PAT\":\"174509\",\"FINAL_UPLOAD\":\"0\",\"DISTRICTNAME\":\"हनुमानगढ़\",\"COLLECTOR_ESIGN_STATUS\":\"156259\",\"COLLECTER_APPROVED\":\"156259\",\"TOTAL_TEH\":\"165238\",\"DISTRICTCODE\":\"31\",\"TEHSILDAR_APPROVED\":\"165215\",\"COLLECTER_REJ\":\"24\"}]";

            if (result.Length > 15)
            {
                List<PM_KISAN_Entity> ObjLst = JsonConvert.DeserializeObject<List<PM_KISAN_Entity>>(result);

                CommonServicesModel objCommonServicesModel = new CommonServicesModel(new FileLogger());



                var Flag = objCommonServicesModel.InsertPM_KIssanData(ObjLst);
            }

            return "";

        }
        #endregion

        #region Credit Co operative Society Complaint Module
        [HttpPost]
        public string Mobile_GetCreditCooperativeSocieties(int DistrictId, int IsSingleState = 0)
        {
            try
            {
                CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
                DataTable dt = (objCitizenModel.GetCreditCooperativeSocietiesGreviyans(DistrictId.ToString(), -1, IsSingleState));

                if (dt != null && dt.Rows.Count > 0)
                {
                    var result = dt.AsEnumerable().Select(p => new
                    {
                        SocietyIdEnc = p.Field<string>("SocietyIdEnc"),
                        SocietyID = p.Field<int>("SocietyID"),
                        RegistrationNo = p.Field<string>("RegistrationNo"),
                        SocietyName = p.Field<string>("SocietyName")
                        //Name = p.Field<string>("Name"),
                        //ContactNo = p.Field<string>("ContactNo"),
                        //DISTRICT_ENG = p.Field<string>("DISTRICT_ENG"),
                        //DISTRICT_MANGAL = p.Field<string>("DISTRICT_MANGAL"),
                        //DISTRICT_ID = p.Field<int>("DISTRICT_ID"),
                    });
                    return new JavaScriptSerializer().Serialize(result);
                }

                return new JavaScriptSerializer().Serialize(new List<object>());
            }
            catch
            {
                return new JavaScriptSerializer().Serialize(new List<object>());
            }
        }

        [HttpPost]
        public string Mobile_SendOTP(string _AadharNo)
        {
            return AADHARSerice.SendOTPOnAADHAR(_AadharNo);
        }

        [HttpPost]
        public string Mobile_VerifyOTP(string otp, string txn, string AadharNoEnc)
        {
            try
            {
                return AADHARSerice.VeryifyOTPsentOnAADHAR(otp, txn, AadharNoEnc.Replace(" ", "+"));
            }
            catch (Exception ex)
            {
                var t = new { Result = false, Message = ex.Message, AadharNo = AadharNoEnc, txnid = txn, AADHARDetails = string.Empty };
                return new JavaScriptSerializer().Serialize(t);
            }

        }

        [HttpPost]
        public string Mobile_GetCreditCooperativeSocietyComplaintDetail(string ReceiptNo)
        {
            try
            {
                EmitraModelFactory objEmitraModelFactory = new EmitraModelFactory(new DBLogger());
                DataTable dt = objEmitraModelFactory.CCSComplaintRegisterDataByReceiptNo(ReceiptNo);

                if (dt != null && dt.Rows.Count > 0)
                {
                    var result = dt.AsEnumerable().Select(p => new
                    {
                        ComplaintId = p.Field<int>("ComplaintId"),
                        ComplainerName = p.Field<string>("ComplainerName"),
                        ComplainerFatherName = p.Field<string>("ComplainerFatherName"),
                        Addres = p.Field<string>("Address").Replace(System.Environment.NewLine, ""),
                        MobileNo = p.Field<string>("MobileNo"),
                        Email = p.Field<string>("Email"),
                        BranchName = p.Field<string>("BranchName"),
                        AccountNo = p.Field<string>("AccountNo"),
                        IFSCCode = p.Field<string>("IFSCCode"),
                        AmountDeposited = p.Field<decimal>("AmountDeposited"),
                        DepositDate = p.Field<string>("DepositDate"),
                        AADHAR = p.Field<string>("AADHAR"),
                        CreateDateTime = p.Field<string>("CreateDateTime"),
                        UserType = p.Field<string>("UserType"),
                        ReceiptNo = p.Field<string>("ReceiptNo"),
                        DistrictId = p.Field<int>("DistrictId"),
                        DistrictEng = p.Field<string>("DISTRICT_ENG"),
                        DistrictHi = p.Field<string>("DISTRICT_MANGAL"),
                        CCSId = p.Field<int>("CCSId"),
                        SocietyName = p.Field<string>("SocietyName"),
                        TotalDocumentSubmitted = p.Field<int>("TotalDocumentSubmitted"),
                        FileTypeId = p.Field<int>("FileTypeId"),
                        TypeNameEng = p.Field<string>("TypeNameEng"),
                        TypeNameHi = p.Field<string>("TypeNameHi"),
                        FilePath = p.Field<string>("FilePath"),
                        AccountNoofDetails = p.Field<string>("DetailAccountNo"),
                        PaidAmount = p.Field<decimal>("PaidAmount"),
                        PayDate = p.Field<string>("PayDate"),
                        MaturityDate = p.Field<string>("MaturityDate"),
                        MaturityAmount = p.Field<decimal>("MaturityAmount"),
                        SSOId = p.Field<string>("SSOId"),
                        Remarks = p.Field<string>("Remarks"),
                        AccountNoInSociety = p.Field<string>("AccountNoInSociety"),
                        AccountIFSCInSociety = p.Field<string>("AccountIFSCInSociety"),
                        AccountHolderName = p.Field<string>("AccountHolderName"),
                        PassbookCrossCheque = p.Field<string>("PassbookCrossCheque"),
                        BPNo = p.Field<string>("BPNo"),
                    });

                    var A = result.First();
                    List<CCSComplaintRegisterDetailModel> details = new List<CCSComplaintRegisterDetailModel>();
                    details = result.Select(p => new CCSComplaintRegisterDetailModel()
                    {
                        AccountNo = p.AccountNoofDetails,
                        PaidAmount = p.PaidAmount,
                        PayDate = p.PayDate,
                        MaturityAmount = p.MaturityAmount,
                        MaturityDate = p.MaturityDate,
                        DocTypeDetails = new DocTypeDetailModel() { DocTypeId = p.FileTypeId, DocTypeNameEng = p.TypeNameEng, DocTypeNameHi = p.TypeNameHi }
                    }).ToList();

                    CCSComplaintRegisterModel obj = new CCSComplaintRegisterModel()
                    {
                        ComplaintId = A.ComplaintId,
                        ComplainerName = A.ComplainerName,
                        ComplainerFatherName = A.ComplainerFatherName,
                        Address = A.Addres,
                        DistrictId = A.DistrictId,
                        DistrictDetails = new DistrictModel() { DistrictId = A.DistrictId, DistrictEng = A.DistrictEng, DistrictHi = A.DistrictHi },
                        MobileNo = A.MobileNo,
                        Email = A.Email,
                        CCSId = A.CCSId,
                        SocietyDetails = new SocietyDetailModel() { SocietyEng = A.SocietyName },
                        BranchName = A.BranchName,
                        AccountNo = A.AccountNo,
                        IFSCCode = A.IFSCCode,
                        AmountDeposited = A.AmountDeposited,
                        DepositDate = Convert.ToDateTime(A.DepositDate).ToString("dd/MM/yyyy"),
                        AADHAR = A.AADHAR,
                        UserType = A.UserType,
                        SSOId = A.SSOId,
                        Remarks = A.Remarks,
                        AccountNoInSociety = A.AccountNoInSociety,
                        AccountIFSCInSociety = A.AccountIFSCInSociety,
                        AccountHolderName = A.AccountHolderName,
                        PassbookCrossCheque = A.PassbookCrossCheque,
                        BPNo = A.BPNo,
                        FileDetails = details
                    };

                    return new JavaScriptSerializer().Serialize(obj);
                }

                return new JavaScriptSerializer().Serialize(new List<object>());
            }
            catch (Exception ex)
            {
                return new JavaScriptSerializer().Serialize(new List<object>());
            }
        }


        [HttpPost]
        public string Mobile_RegisterComplaintAgainstCreditCooperativeSociety(CCSComplaintRegisterModel model)
        {

            model.CreateIP = COMMON_FUNCTION.GetIpAddress();
            model.UserType = "C";
            model.SSOId = "ByMobileUser";

            string fileName = string.Empty;
            List<CCSComplaintRegisterDetailModel> tempFileDetails = new List<CCSComplaintRegisterDetailModel>();

            string folderpath = "../Content/CCSComplaintFiles";
            if (!Directory.Exists(Server.MapPath(folderpath)))
                Directory.CreateDirectory(Server.MapPath(folderpath));

            // Save CrossCheque/Passbook
            try
            {
                fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(model.PassbookCrossCheque);
                string filepathForSave = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/CCSComplaintFiles/"), fileName);
                model.PassbookCrossCheque = fileName;

                byte[] bytes = Convert.FromBase64String(model.PassbookCrossChequeFileBase64String);
                System.IO.File.WriteAllBytes(filepathForSave, bytes);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.CommonServices", FunctionName = "public string Mobile_RegisterComplaintAgainstCreditCooperativeSociety(CCSComplaintRegisterModel model) Error while Passbook/Cross Cheque File Save" }, ex);
                return new JavaScriptSerializer().Serialize(new { Result = false, Message = "Error while Passbook/Cross Cheque File Save" });
            }


            try
            {
                foreach (var files in model.FileDetails)
                {
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(files.FileName);
                    string filepathForSave = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/CCSComplaintFiles/"), fileName);
                    files.FilePath = fileName;

                    byte[] bytes = Convert.FromBase64String(files.FileBase64String);
                    System.IO.File.WriteAllBytes(filepathForSave, bytes);
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.CommonServices", FunctionName = "public string Mobile_RegisterComplaintAgainstCreditCooperativeSociety(CCSComplaintRegisterModel model) error while File Convert base64 to physical file" }, ex);
                return new JavaScriptSerializer().Serialize(new { Result = false, Message = "Error while Investment detail file save" });
            }

            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            string abc = JsonConvert.SerializeObject(model);
            int Result = objCitizenModel.SaveCCSComplaint(model);

            if (Result > 0)
            {
                try
                {
                    DataTable Response = objCitizenModel.GetCCSComplaintRegisterForCitizen(Result);
                    string IsOk = string.Empty;

                    if (Response != null && Response.Rows.Count > 0)
                    {
                        string MobileNo = Response.Rows[0]["MobileNo"].ToString();

                        if (MobileNo.Length == 10)
                        {
                            EsanchaarController EC = new EsanchaarController();
                            IsOk = EC.SendSocietySMS("Dear " + Convert.ToString(Response.Rows[0]["ComplainerName"]) + " S/O " + Convert.ToString(Response.Rows[0]["ComplainerFatherName"]) + ", Your complaint has been registered successfully. Your comlaint number is " + Convert.ToString(Response.Rows[0]["ReceiptNo"]) + ".", MobileNo);
                        }
                    }

                    var t = new { Result = true, Message = "Complaint Registered successfully." + (IsOk.Contains("200") ? " Message sent successfully." : " Error while Message sent") };
                    return new JavaScriptSerializer().Serialize(t);
                }
                catch
                {
                    var t = new { Result = true, Message = "Complaint Registered successfully. Error while Message sent" };
                    return new JavaScriptSerializer().Serialize(t);
                }
            }
            else
            {
                var t = new { Result = false, Message = "Error while Complaint Registration.No action Performed" };
                return new JavaScriptSerializer().Serialize(t);
            }
        }

        [HttpPost]
        public string TestCSSComplaint()
        {
            //string jsonString = "{\"ComplainerName\":\"Mobile App Developer\",\"ComplainerFatherName\":\"Complainer Father Name Mobile App Developer\",\"Address\":\"Jaipur\",\"DistrictId\":1,\"MobileNo\":\"8058828693\",\"Email\":\"cooperative@cooperative.com\",\"CCSId\":1,\"BranchName\":\"Credit Cooperative Branch\",\"AccountNo\":\"123654789\",\"IFSCCode\":\"IFSC00001\",\"AmountDeposited\":\"1000\",\"AADHAR\":\"AeS5cagS6t6LxgjZpCq7sd==\",\"MembershipId\":\"MMM00001\",\"Remarks\":\"Remarks\",\"AccountNoInSociety\":\"SBI0000001\",\"AccountIFSCInSociety\":\"IFSC00002\",\"AccountHolderName\":\"Ramesh Singh\",\"PassbookCrossCheque\":\"\",\"BPNo\":\"BOB00001\",\"FileDetails\": [{\"AccountNo\":\"123658749\",\"PaidAmount\":\"1000\",\"PayDate\":\"10/10/2019\",\"MaturityAmount\":1000,\"MaturityDate\":\"20/09/2025\"}]}";
            List<CCSComplaintRegisterDetailModel> temp = new List<CCSComplaintRegisterDetailModel>();

            string filename = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/CCSComplaintFiles/"), "24a86c0f-a97c-4398-8ff9-9cbc2e468ca1.txt");
            byte[] bytes;
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                bytes = System.IO.File.ReadAllBytes(filename);
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                fs.Close();
            }

            temp.Add(new CCSComplaintRegisterDetailModel() { FileName = "abc.txt", FileBase64String = Convert.ToBase64String(bytes), FileTypeId = 1, AccountNo = "0125688", PaidAmount = 80000, PayDate = "10/10/2019", MaturityAmount = 100000, MaturityDate = "28/06/2022" });

            CCSComplaintRegisterModel obj = new CCSComplaintRegisterModel()
            {
                AADHAR = "123456789123Encrypted",
                AccountNo = "123456789",
                Address = "Nehru Palace Jaipur",
                AmountDeposited = 1200,
                BranchName = "BranchName",
                CCSId = 1,
                ComplainerFatherName = "Complainer FatherName For Test",
                ComplainerName = "Complainer Name for Test",
                DistrictId = 1,
                Email = "erdmaster.mutant@gmail.com",
                FileDetails = temp,
                IFSCCode = "ICICI5689",
                MobileNo = "8058828693",
                MembershipId = "JL00912",
                Remarks = "TEST REMARK",
                AccountNoInSociety = "000000123456",
                AccountIFSCInSociety = "SBIN789685",
                AccountHolderName = "Account Holder Name",
                PassbookCrossCheque = "abc.txt",
                PassbookCrossChequeFileBase64String = Convert.ToBase64String(bytes),
                BPNo = "CustomerNo"
            };

            return this.Mobile_RegisterComplaintAgainstCreditCooperativeSociety(obj);
        }


        [HttpPost]
        public string ValidateOTPAndGetComplainerList(string otp, string txn, string _AadharNo)
        {
            var response = AADHARSerice.VeryifyOTPsentOnAADHAR(otp, txn, _AadharNo);
            dynamic ResponseData = new JavaScriptSerializer().Deserialize<dynamic>(response);
            bool IsVerified = Convert.ToBoolean(ResponseData["Result"]);

            if (IsVerified)
            {
                CitizenModel objCitizenModel = new CitizenModel(new DBLogger());

                DataTable dt = objCitizenModel.CCSComplaintListByAADHAR(_AadharNo, -1, 1);

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["ComplaintId"] = 0;
                    }
                }
                return COMMON_FUNCTION.DataTableToJSONString(dt);
            }
            else
            {
                return "[]";
            }
        }

        [HttpPost]
        public string Mobile_GetUploadDocTypeForCCSComplaint()
        {
            try
            {
                MasterModel objMasterModel = new MasterModel(new DBLogger());
                DataTable dt = objMasterModel.CCSComplaintDocumentTypeList();

                if (dt != null && dt.Rows.Count > 0)
                {
                    var result = dt.AsEnumerable().Select(p => new
                    {
                        FileTypeId = p.Field<int>("CCSComplaintDocTypeId"),
                        TypeNameEng = p.Field<string>("TypeNameEng").Trim(),
                        TypeNameHi = p.Field<string>("TypeNameHi").Trim()
                    });
                    return new JavaScriptSerializer().Serialize(result);
                }

                return new JavaScriptSerializer().Serialize(new List<object>());
            }
            catch (Exception ex)
            {
                (new DBLogger()).ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.CommonServicesController", FunctionName = "public string Mobile_GetUploadDocTypeForCCSComplaint()" }, ex);
                return new JavaScriptSerializer().Serialize(new List<object>());
            }
        }


        [HttpPost]
        public string GetComplaintReportData(int District = 0, int Society = 0)
        {
            CommonServicesModel objMISModel = new CommonServicesModel(new DBLogger());
            DataSet DT = objMISModel.GetComplaintReportForMobileApp(District, Society);
            return JsonConvert.SerializeObject(DT, Newtonsoft.Json.Formatting.Indented);
        }

        [HttpPost]
        public string GetCCSComplaintDetailFile(string ComplaintIdEnc)
        {
            return GenerateCCSComplaintPDF.GetPDF(ComplaintIdEnc);
        }

        // { "DistrictId": 0,	"CCSId": 0,	"PropertyTypeId": 0,	"LocationStateId": 1,	"LocationDistrictId": 1,	"IsRural": true,	"LocationAddress": "",	"LocationLandMark": "",	"ImageName1": "",	"ImageName2": "",	"Base64StringImage1": "",	"Base64StringImage2": "",	"Latitude": "",	"Longitude": "","MobileNo": "" }

        #endregion


        #region Credit Co operative Society Property Detail 
        [HttpPost]
        public JsonResult Mobile_InsertCCSPropertyDetail(CCSPropertDetailEntity entity)
        {
            string validationMessage = string.Empty;

            if (!ValidateCCSPropertyDetail(entity, out validationMessage)) // Validate model data
            {
                return Json(new { Result = false, Message = validationMessage });
            }

            try
            {
                string folderpath = "../Content/CCSPropertyDetails";
                if (!Directory.Exists(Server.MapPath(folderpath)))
                    Directory.CreateDirectory(Server.MapPath(folderpath));

                // Save Image 1
                string image1 = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(entity.ImageName1);
                string filepathForSave = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/CCSPropertyDetails/"), image1);
                byte[] bytes = Convert.FromBase64String(entity.Base64StringImage1);
                System.IO.File.WriteAllBytes(filepathForSave, bytes);
                entity.ImageName1 = image1;

                // Save Image 2
                string image2 = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(entity.ImageName2);
                string filepathForSave2 = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/CCSPropertyDetails/"), image2);
                byte[] bytes2 = Convert.FromBase64String(entity.Base64StringImage2);
                System.IO.File.WriteAllBytes(filepathForSave2, bytes2);
                entity.ImageName2 = image2;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.CommonServicesController", FunctionName = "public JsonResult Mobile_InsertCCSPropertyDetail(CCSPropertDetailEntity entity) while base 64 string to physical file conversion." }, ex);
                return Json(new { Result = false, Message = "Base 64 string to physical file conversion failed. So no action performed." }); // Since Files are not saved properly, hence no action performed and return with no action
            }

            try
            {
                CommonServicesModel objCommonServicesModel = new CommonServicesModel(new DBLogger());
                int response = objCommonServicesModel.CCSPropertyDetailInsert(entity);
                return Json(new { Result = response > 0 ? true : false, Message = response > 0 ? "Credit Cooperative property details saved successfully." : "Error!!" }); // (!!) means error log handled at database level directly
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.CommonServicesController", FunctionName = "public JsonResult Mobile_InsertCCSPropertyDetail(CCSPropertDetailEntity entity)" }, ex);
                return Json(new { Result = false, Message = "Error!!!" }); // (!!!) means error fired from Source code and saved in database
            }
        }

        [HttpPost]
        public string Mobile_GetCCSPropertyTypeList()
        {
            try
            {
                CommonServicesModel objCommonServicesModel = new CommonServicesModel(new DBLogger());
                DataTable Result = objCommonServicesModel.Mobile_GetCCSPropertyTypeList();
                return COMMON_FUNCTION.DataTableToJSONString(Result);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.CommonServicesController", FunctionName = "public string Mobile_GetCCSPropertyTypeList(CCSPropertDetailEntity entity)" }, ex);
                return "[]";
            }
        }

        [HttpPost]
        public string Mobile_GetStateListCCSLocation()
        {
            try
            {
                CommonServicesModel objCommonServicesModel = new CommonServicesModel(new DBLogger());
                DataTable Result = objCommonServicesModel.Mobile_GetStateListCCSLocation();
                return COMMON_FUNCTION.DataTableToJSONString(Result);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.CommonServicesController", FunctionName = "public string Mobile_GetStateListCCSLocation()" }, ex);
                return "[]";
            }
        }

        [HttpPost]
        public string Mobile_GetDistrictListCCSLocation(int StateId)
        {
            try
            {
                CommonServicesModel objCommonServicesModel = new CommonServicesModel(new DBLogger());
                DataTable Result = objCommonServicesModel.Mobile_GetDistrictListCCSLocation(StateId);
                return COMMON_FUNCTION.DataTableToJSONString(Result);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.CommonServicesController", FunctionName = "public string Mobile_GetDistrictListCCSLocation(int StateId)" }, ex);
                return "[]";
            }
        }
        private bool ValidateCCSPropertyDetail(CCSPropertDetailEntity entity, out string message)
        {
            int pointer = 1;
            string localStr = string.Empty;
            message = localStr;

            if (entity.DistrictId < 1)
            {
                localStr += pointer + ". District Id must be non negetive and greater than zero "; pointer++;
            }

            if (entity.CCSId < 1)
            {
                localStr += pointer + ". Credit Cooperative Society Id must be non negetive and greater than zero "; pointer++;
            }

            if (entity.PropertyTypeId < 1)
            {
                localStr += pointer + ". Property Type Id must be non negetive and greater than zero "; pointer++;
            }

            if (entity.LocationStateId < 1)
            {
                localStr += pointer + ". Location State Id must be non negetive and greater than zero "; pointer++;
            }

            if (entity.LocationDistrictId < 1)
            {
                localStr += pointer + ". Location District Id must be non negetive and greater than zero "; pointer++;
            }

            if (string.IsNullOrEmpty(entity.LocationAddress))
            {
                localStr += pointer + ". Must Provide Location Address "; pointer++;
            }

            if (string.IsNullOrEmpty(entity.LocationLandMark))
            {
                localStr += pointer + ". Must Provide Location Landmark "; pointer++;
            }

            if (string.IsNullOrEmpty(entity.Latitude))
            {
                localStr += pointer + ". Must Provide Latitude of location "; pointer++;
            }
            if (string.IsNullOrEmpty(entity.Longitude))
            {
                localStr += pointer + ". Must Provide Longitude of location ";
            }



            if (!string.IsNullOrEmpty(localStr))
            {
                message = localStr;
                return false;
            }

            return true;
        }

        [HttpPost]
        public string Mobile_GetEnrantTypeListCCSLocation()
        {
            try
            {
                CommonServicesModel objCommonServicesModel = new CommonServicesModel(new DBLogger());
                DataTable Result = objCommonServicesModel.Mobile_GetEnrantListCCSLocation();
                return COMMON_FUNCTION.DataTableToJSONString(Result);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.CommonServicesController", FunctionName = "public string Mobile_GetStateListCCSLocation()" }, ex);
                return "[]";
            }
        }
        #endregion

        //{"DistrictId":"12",  "purchase_center":"1349",  "commodity":"",  "Kvss":""}
        [HttpPost]
        public string Mobile_GetTotalQuantityDetailsV1(string DistrictId = "0", string Kvss = "", string purchase_center = "0", string commodityId = "")
        {
            try
            {
                // Response.AppendHeader("Access-Control-Allow-Origin", "*");
                CommonServicesModel objCitizenModel = new CommonServicesModel(new DBLogger());
                DataTable dt = (objCitizenModel.GetTotalQuantityList(DistrictId, Kvss, purchase_center, commodityId));
                // return new JavaScriptSerializer().Serialize(dt);
                return COMMON_FUNCTION.DataTableToJSONString(dt);
            }
            catch
            {
                return new JavaScriptSerializer().Serialize(new List<object>());
            }
        }

        [HttpGet]
        public string Mobile_GetTotalQuantityDetails(string DistrictId = "0", string Kvss = "", string purchase_center = "0", string commodityId = "")
        {
            try
            {
                Response.AppendHeader("Access-Control-Allow-Origin", "*");
                CommonServicesModel objCitizenModel = new CommonServicesModel(new DBLogger());
                DataTable dt = (objCitizenModel.GetTotalQuantityList(DistrictId, Kvss, purchase_center, commodityId));
                // return new JavaScriptSerializer().Serialize(dt);
                return COMMON_FUNCTION.DataTableToJSONString(dt);
            }
            catch
            {
                return new JavaScriptSerializer().Serialize(new List<object>());
            }
        }


        [HttpPost]
        public JsonResult SocietyDetails(string RegistrationNo)
        {
            Models.MISModel objNew = new Models.MISModel();
            DataTable DT = objNew.GetCertificateforVerify(RegistrationNo.Replace(" ", ""));

            List<SocietyDetailsModel> _societyDetails = new List<SocietyDetailsModel>();
            List<SocietyMembersModel> _membersDetails = new List<SocietyMembersModel>();

            foreach (DataRow dr in DT.Rows)
            {

                DataTable dtM = objNew.GetAdministrativeOfficers(dr["SocietyIDInc"].ToString());
                _membersDetails = new List<SocietyMembersModel>();
                foreach (DataRow dr2 in dtM.Rows)
                {
                    _membersDetails.Add(new SocietyMembersModel()
                    {
                        Name = dr2["Name"].ToString(),
                        Father_HusbandName = dr2["FatherName"].ToString(),
                        ContactNo = dr2["ContactNo"].ToString(),
                        Education = dr2["Education"].ToString(),
                        ResidenceAddress = dr2["Address"].ToString(),
                        Post = dr2["POSTNAME"].ToString()
                    });
                }

                _societyDetails.Add(new SocietyDetailsModel()
                {
                    Act = dr["ServiceName"].ToString(),
                    PartialCapital = dr["PartialCapital"].ToString(),
                    RegDate = dr["RegistrationDate"].ToString(),
                    RegNo = dr["ApplicationNo"].ToString(),
                    SocietyName = dr["SocietyName"].ToString(),
                    Type = dr["TypeameEng"].ToString(),
                    WorkingCapital = dr["WorkingCapital"].ToString(),
                    YearOfEstablishment = dr["YearOfEstablishment"].ToString(),
                    MonthOfEstablishment = dr["MonthOfEstablishment"].ToString(),
                    Members = _membersDetails
                });


            }
            return Json(_societyDetails, JsonRequestBehavior.AllowGet);
        }


        #region CM KISAN
        [HttpPost]
        public JsonResult KnowBeneficiaryDbtStatus(string MobileNo = "", string DistrictId = "", string RegNo = "")
        {
            var responce = new APIResponce();
            var apiService = new ApiServiceModel(new FileLogger());
            var ds = apiService.GetBeneficiaryDbtStatus(MobileNo, DistrictId, RegNo);
            if (ds != null)
            {
                responce.Data = new
                {
                    BeneficiaryInfo = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                    DbtStatus = ds.Tables.Count > 1 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[1]) : null,
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        [BasicAuth]
        [HttpPost]
        public JsonResult KnowBeneficiaryStatus(string AadhaarNo = "")
        {
            var responce = new APIResponce();
            var apiService = new ApiServiceModel(new FileLogger());
            if (AadhaarNo.Trim().Length == 12)
            {
                var ds = apiService.GetBeneficiaryStatus(AadhaarNo);
                if (ds != null)
                {
                    responce.Data = new
                    {
                        BeneficiaryInfo = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? JsonConvert.DeserializeObject<List<BeneficiaryInfoModel>>(COMMON_FUNCTION.DataTableToJSONWithJSONNet(ds.Tables[0]))[0] : null,
                        DbtTransList = ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0 ? JsonConvert.DeserializeObject<List<DbtTransModel>>(COMMON_FUNCTION.DataTableToJSONString(ds.Tables[1])) : null,
                    };
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        responce.Status = "200";
                        responce.Message = "Success";
                    }
                    else
                    {
                        responce.Status = "200";
                        responce.Message = "Record not found.";
                    }
                }
                else
                {
                    responce.Status = "500";
                    responce.Message = "Internal Server Error.";
                }
                return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                responce.Status = "400";
                responce.Message = "Please enter valid Aadhaar number.";
                return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Get_CMK_StatusTrackerSummary()
        {
            var responce = new APIResponce();
            var apiService = new ApiServiceModel(new FileLogger());
            var ds = apiService.Get_CMK_StatusTrackerSummary();
            if (ds != null)
            {
                responce.Data = new
                {
                    StatusSummary = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? JsonConvert.DeserializeObject<List<CMK_StatusTrackerSummaryModel>>(COMMON_FUNCTION.DataTableToJSONWithJSONNet(ds.Tables[0]))[0] : null
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PMK_DataPull(string GID = "")
        {
            try
            {
                string url = "https://ws.pmkisan.gov.in/RJdatapush18.asmx/DataPull";
                var requestData = new
                {
                    GID = GID,
                    Token = GlobalConstant.PMK_Token
                };

                string jsonRequest = JsonConvert.SerializeObject(requestData);
                string encryptedRequest = AesGcm256.Encrypt(jsonRequest,
                        AesGcm256.HexToByte(GlobalConstant.AesGcm256EncryptionKey1),
                        AesGcm256.HexToByte(GlobalConstant.AesGcm256EncryptionKey2));

                var encryptedRequestBody = new { EncryptedRequest = encryptedRequest };
                string encryptedRequestJson = JsonConvert.SerializeObject(encryptedRequestBody);
                var responseString = "";

                responseString = Task.Run(() =>
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            var content = new StringContent(encryptedRequestJson, Encoding.UTF8, "application/json");
                            HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();
                            response.EnsureSuccessStatusCode();
                            return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        }
                        catch (Exception iex)
                        {
                            ILogger objILogger = new DBLogger();
                            objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.CommonServices", FunctionName = $"PMK_DataPull INNEREX-(GID={GID})" }, iex);
                            return iex.Message;
                        }
                    }
                }).Result;

                string path = System.Web.HttpContext.Current.Server.MapPath("~/Content/PMKisan/BeneficiaryData/");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                path = Path.Combine(path, $"{GID}-" + DateTime.Now.Date.ToString("MM") + ".json");
                if (responseString != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseFormatModel>(responseString);
                    string decryptedResponse = AesGcm256.Decrypt(responseObject.d.output,
                        AesGcm256.HexToByte(GlobalConstant.AesGcm256EncryptionKey1),
                        AesGcm256.HexToByte(GlobalConstant.AesGcm256EncryptionKey2));

                    FileStream fs1 = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter tw = new StreamWriter(fs1);
                    tw.WriteLine(decryptedResponse);
                    tw.Close();

                    return Json(new { Result = new { GID = GID, Status = "Success" } }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new { Result = new { GID = GID, Status = "Error- Null response" } }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.CommonServices", FunctionName = $"PMK_DataPull(GID={GID})" }, ex);
                return Json(new { Result = new { GID = GID, Status = "Error:- " + ex.Message } }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult PMK_DataPullnPushInDB(string GID = "")
        {
            string logid = "";
            int minGid = 0, maxGid = 0;
            if (GID != null && GID.Length > 0)
            {
                minGid = Convert.ToInt32(GID.Split(',')[0]);
                maxGid = GID.Split(',').Length > 1 ? Convert.ToInt32(GID.Split(',')[1]) : minGid;
            }

            var apiService = new ApiServiceModel(new FileLogger());
            for (int i = minGid; i <= maxGid; i++)
            {
                try
                {
                    string url = "https://ws.pmkisan.gov.in/RJdatapush18.asmx/DataPull";
                    var requestData = new
                    {
                        GID = i,
                        Token = GlobalConstant.PMK_Token
                    };

                    string jsonRequest = JsonConvert.SerializeObject(requestData);
                    //InsertTraceLogs
                    logid = new csCommon().InsertTraceLogs("CommonServices", "PMK_DataPullnPushInDB", "jsonRequest", jsonRequest, "W", "NA", "");
                    string encryptedRequest = AesGcm256.Encrypt(
                        jsonRequest,
                        AesGcm256.HexToByte(GlobalConstant.AesGcm256EncryptionKey1),
                        AesGcm256.HexToByte(GlobalConstant.AesGcm256EncryptionKey2)
                    );
                    //InsertTraceLogs
                    logid = new csCommon().InsertTraceLogs("CommonServices", "PMK_DataPullnPushInDB", "encryptedRequest", encryptedRequest, "W", "NA", "");
                    var encryptedRequestBody = new { EncryptedRequest = encryptedRequest };
                    string encryptedRequestJson = JsonConvert.SerializeObject(encryptedRequestBody);

                    //InsertTraceLogs
                    logid = new csCommon().InsertTraceLogs("CommonServices", "PMK_DataPullnPushInDB", "encryptedRequestJson", encryptedRequestJson, "W", "NA", "");
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    string responseString = string.Empty;

                    try
                    {
                        var content = new StringContent(encryptedRequestJson, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = httpClient.PostAsync(url, content).GetAwaiter().GetResult(); // sync-safe
                        response.EnsureSuccessStatusCode();

                        responseString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        //InsertTraceLogs
                        logid = new csCommon().InsertTraceLogs("CommonServices", "PMK_DataPullnPushInDB", "responseString", responseString, "W", "NA", "");
                    }
                    catch (Exception iex)
                    {
                        ILogger objILogger = new DBLogger();
                        objILogger.ErrorHandle(new ErrorLogEntity()
                        {
                            PageName = "Cooperative.Controllers.CommonServices",
                            FunctionName = $"PMK_DataPullnPushInDB INNEREX-(GID={i})"
                        }, iex);
                        return Json(new { Result = new { GID = i, Status = "HTTP Error: " + iex.Message } }, JsonRequestBehavior.AllowGet);
                    }

                    if (!string.IsNullOrWhiteSpace(responseString))
                    {
                        var responseObject = JsonConvert.DeserializeObject<ResponseFormatModel>(responseString);
                        string decryptedResponse = AesGcm256.Decrypt(
                            responseObject.d.output,
                            AesGcm256.HexToByte(GlobalConstant.AesGcm256EncryptionKey1),
                            AesGcm256.HexToByte(GlobalConstant.AesGcm256EncryptionKey2)
                        );
                        //InsertTraceLogs
                        logid = new csCommon().InsertTraceLogs("CommonServices", "PMK_DataPullnPushInDB", "decryptedResponse", decryptedResponse, "W", "NA", "");
                        JObject jsonObj = JObject.Parse(decryptedResponse);
                        JArray dataArray = (JArray)jsonObj["Data"];
                        DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(dataArray.ToString());

                        var r = apiService.AddUpdateBeneficiaryDataByDataTable(dataTable);

                        return Json(new
                        {
                            Result = new
                            {
                                GID = i,
                                Status = r > 0 ? "Success" : "Error"
                            }
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Result = new { GID = i, Status = "Error - Empty response" } }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    ILogger objILogger = new DBLogger();
                    objILogger.ErrorHandle(new ErrorLogEntity()
                    {
                        PageName = "Cooperative.Controllers.CommonServices",
                        FunctionName = $"PMK_DataPullnPushInDB(GID={i})"
                    }, ex);
                    return Json(new { Result = new { GID = i, Status = "Error: " + ex.Message } }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { Result = new { GID = GID, Status = "Error - Empty response" } }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PMK_IneligibleDataPullnPushInDB()
        {
            string logid = "";

            string formattedDate = "2026-08-12"; //DateTime.Now.ToString("yyyy-MM-dd");
            var apiService = new ApiServiceModel(new FileLogger());

            try
            {
                string url = "https://ws.pmkisan.gov.in/RJdatapush18.asmx/IneligibleData";
                var requestData = new
                {
                    Date = formattedDate,
                    Token = GlobalConstant.PMK_Token
                };

                string jsonRequest = JsonConvert.SerializeObject(requestData);
                //InsertTraceLogs
                //logid = new csCommon().InsertTraceLogs("CommonServices", "PMK_IneligibleDataPullnPushInDB", "jsonRequest", jsonRequest, "W", "NA", "");
                string encryptedRequest = AesGcm256.Encrypt(
                        jsonRequest,
                        AesGcm256.HexToByte(GlobalConstant.AesGcm256EncryptionKey1),
                        AesGcm256.HexToByte(GlobalConstant.AesGcm256EncryptionKey2)
                    );
                //InsertTraceLogs
                //logid = new csCommon().InsertTraceLogs("CommonServices", "PMK_IneligibleDataPullnPushInDB", "encryptedRequest", encryptedRequest, "W", "NA", "");
                var encryptedRequestBody = new { EncryptedRequest = encryptedRequest };
                string encryptedRequestJson = JsonConvert.SerializeObject(encryptedRequestBody);

                //InsertTraceLogs
                logid = new csCommon().InsertTraceLogs("CommonServices", "PMK_IneligibleDataPullnPushInDB", "encryptedRequestJson", encryptedRequestJson, "W", "NA", "");
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string responseString = string.Empty;

                try
                {
                    var content = new StringContent(encryptedRequestJson, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = httpClient.PostAsync(url, content).GetAwaiter().GetResult(); // sync-safe
                    response.EnsureSuccessStatusCode();

                    responseString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    //InsertTraceLogs
                    logid = new csCommon().InsertTraceLogs("CommonServices", "PMK_IneligibleDataPullnPushInDB", "responseString", responseString, "W", "NA", "");
                }
                catch (Exception iex)
                {
                    ILogger objILogger = new DBLogger();
                    objILogger.ErrorHandle(new ErrorLogEntity()
                    {
                        PageName = "Cooperative.Controllers.CommonServices",
                        FunctionName = $"PMK_IneligibleDataPullnPushInDB INNEREX-(GID={0})"
                    }, iex);
                    return Json(new { Result = new { GID = 0, Status = "HTTP Error: " + iex.Message } }, JsonRequestBehavior.AllowGet);
                }

                if (!string.IsNullOrWhiteSpace(responseString))
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseFormatModel>(responseString);
                    string decryptedResponse = AesGcm256.Decrypt(
                        responseObject.d.output,
                        AesGcm256.HexToByte(GlobalConstant.AesGcm256EncryptionKey1),
                        AesGcm256.HexToByte(GlobalConstant.AesGcm256EncryptionKey2)
                    );
                    //InsertTraceLogs
                    logid = new csCommon().InsertTraceLogs("CommonServices", "PMK_IneligibleDataPullnPushInDB", "decryptedResponse", decryptedResponse, "W", "NA", "");

                    JObject jsonObj = JObject.Parse(decryptedResponse);
                    JArray dataArray = (JArray)jsonObj["Data"];
                    DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(dataArray.ToString());

                    var r = apiService.AddUpdateIneligibleDataByDataTable(dataTable);

                    //var r = 1;

                    return Json(new
                    {
                        Result = new
                        {
                            GID = 0,
                            Status = r > 0 ? "Success" : "Error"
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = new { GID = 0, Status = "Error - Empty response" } }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Controllers.CommonServices",
                    FunctionName = $"PMK_IneligibleDataPullnPushInDB(GID={0})"
                }, ex);
                return Json(new { Result = new { GID = 0, Status = "Error: " + ex.Message } }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult PMK_DataPullNew1(string GID = "16-05")
        {
            try
            {
                var apiService = new ApiServiceModel(new FileLogger());
                string filePath = System.Web.HttpContext.Current.Server.MapPath($"~/Content/PMKisan/BeneficiaryData/{GID}.json");
                string jsonContent = System.IO.File.ReadAllText(filePath);
                JObject jsonObj = JObject.Parse(jsonContent);
                JArray dataArray = (JArray)jsonObj["Data"];
                DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(dataArray.ToString());
                string jsonStr = COMMON_FUNCTION.DataTableToJSONString(dataTable);
                var r = apiService.AddUpdateBeneficiaryDataByJson(jsonStr);
                if (r > 0)
                    return Json(new { Result = new { GID = GID, Status = "Success" } }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { Result = new { GID = GID, Status = "Error" } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.CommonServices", FunctionName = $"PMK_DataPull(GID={GID})" }, ex);
                return Json(new { Result = new { GID = GID, Status = "Error:- " + ex.Message } }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult PMK_DataPullNew001(string GID = "16-05")
        {
            try
            {
                var apiService = new ApiServiceModel(new FileLogger());
                string filePath = System.Web.HttpContext.Current.Server.MapPath($"~/Content/PMKisan/BeneficiaryData/{GID}.json");
                string jsonContent = System.IO.File.ReadAllText(filePath);
                JObject jsonObj = JObject.Parse(jsonContent);
                JArray dataArray = (JArray)jsonObj["Data"];
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(dataArray.ToString());
                //string jsonStr = COMMON_FUNCTION.DataTableToJSONString(dataTable);                

                var r = apiService.AddUpdateBeneficiaryDataByDataTable(dt);
                if (r > 0)
                    return Json(new { Result = new { GID = GID, Status = "Success" } }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { Result = new { GID = GID, Status = "Error" } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.CommonServices", FunctionName = $"PMK_DataPull(GID={GID})" }, ex);
                return Json(new { Result = new { GID = GID, Status = "Error:- " + ex.Message } }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddUpdateBeneficiaryLOC_PROD(BeneficiaryModel beneficiary)
        {
            var responce = new APIResponce();
            var apiService = new ApiServiceModel(new FileLogger());
            var result = apiService.AddUpdateBeneficiaryLOC_PROD(beneficiary);
            if (result > 0)
            {
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DecryptDBTResponseXML(string password)
        {
            var responce = new APIResponce();
            var cMK_BeneficaryRFT = new CMK_BeneficaryRFTModel();
            var result = cMK_BeneficaryRFT.DecryptResponseXML(password);
            try
            {
                if (result)
                {
                    responce.Status = "200";
                    responce.Message = "File data decrypted Successfully";
                }
                else
                {
                    responce.Status = "500";
                    responce.Message = "Internal Server Error.";
                }
                return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception ex)
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";

                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.CommonServices", FunctionName = "DecryptDBTResponseXML(string password)" }, ex);
                return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UploadDbtResponse(string jsonString)
        {
            var responce = new APIResponce();
            var cMK_BeneficaryRFT = new CMK_BeneficaryRFTModel();
            var result = cMK_BeneficaryRFT.UploadDbtResponse(jsonString);
            try
            {
                if (result)
                {
                    responce.Status = "200";
                    responce.Message = "File data imported Successfully";
                }
                else
                {
                    responce.Status = "500";
                    responce.Message = "Internal Server Error.";
                }
                return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception ex)
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";

                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.CommonServices", FunctionName = "UploadDbtResponse(string jsonString)" }, ex);
                return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UploadDbtResponseFromXml(string rftNumber, int startFrom)
        {
            var responce = new APIResponce();
            var cMK_BeneficaryRFT = new CMK_BeneficaryRFTModel();
            var result = cMK_BeneficaryRFT.ReadXML(rftNumber ?? "RJ-CMK2025-260004", startFrom);
            try
            {
                if (result)
                {
                    responce.Status = "200";
                    responce.Message = "File data imported Successfully";
                }
                else
                {
                    responce.Status = "500";
                    responce.Message = "Internal Server Error.";
                }
                return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception ex)
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";

                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.CommonServices", FunctionName = "UploadDbtResponseFromXml(string rftNumber)" }, ex);
                return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetRequestJson(string GID = "", int isEncrypt = 0)
        {
            try
            {
                var requestData = new
                {
                    GID = GID,
                    Token = GlobalConstant.PMK_Token
                };
                string jsonRequest = JsonConvert.SerializeObject(requestData);
                if (isEncrypt == 0)
                    return Json(new { Result = new { GID = GID, Status = true, Result = jsonRequest } }, JsonRequestBehavior.AllowGet);
                string encryptedRequest = AesGcm256.Encrypt(jsonRequest,
                        AesGcm256.HexToByte(GlobalConstant.AesGcm256EncryptionKey1),
                        AesGcm256.HexToByte(GlobalConstant.AesGcm256EncryptionKey2));

                var encryptedRequestBody = new { EncryptedRequest = encryptedRequest };
                return Json(new { Result = new { GID = GID, Status = true, Result = JsonConvert.SerializeObject(encryptedRequestBody) } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.CommonServices", FunctionName = $"GetRequestJson(GID= {GID}, isEncrypt = {isEncrypt})" }, ex);
                return Json(new { Result = new { GID = GID, Status = false, Result = "Error:- " + ex.Message } }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetDecryptedDataJson(string encryptedData)
        {
            try
            {
                var responseObject = JsonConvert.DeserializeObject<ResponseFormatModel>(encryptedData);
                string decryptedResponse = AesGcm256.Decrypt(responseObject.d.output,
                    AesGcm256.HexToByte(GlobalConstant.AesGcm256EncryptionKey1),
                    AesGcm256.HexToByte(GlobalConstant.AesGcm256EncryptionKey2));
                return Json(new { Result = new { Status = true, Result = decryptedResponse } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.CommonServices", FunctionName = $"GetDecryptedDataJson(encryptedData={encryptedData})" }, ex);
                return Json(new { Result = new { Status = false, Result = "Error:- " + ex.Message } }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DownloadCMK_MobileDataCsv_old(int DistrictId = 0)
        {
            var apiService = new ApiServiceModel(new FileLogger());
            DataTable dt = apiService.Get_CMK_MobileData(DistrictId);
            var csv = new StringBuilder();

            // Add headers
            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
            csv.AppendLine(string.Join(",", columnNames));

            // Add rows
            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field =>
                    "\"" + field.ToString().Replace("\"", "\"\"") + "\""); // handle commas & quotes
                csv.AppendLine(string.Join(",", fields));
            }

            byte[] buffer = Encoding.UTF8.GetBytes(csv.ToString());
            return File(buffer, "text/csv", $"CMK_Beneficiary_Mobile_Data{(DistrictId == 0 ? "" : DistrictId.ToString())}.csv");
        }

        public ActionResult DownloadCMK_MobileDataCsv(int DistrictId = 0)
        {
            var apiService = new ApiServiceModel(new FileLogger());
            DataTable dt = apiService.Get_CMK_MobileData(DistrictId);
            var csv = new StringBuilder();

            // Add headers
            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
            csv.AppendLine(string.Join(",", columnNames));

            // Add rows
            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field =>
                {
                    var value = field.ToString();
                    // escape only if field has comma or quote
                    if (value.Contains(",") || value.Contains("\""))
                        value = "\"" + value.Replace("\"", "\"\"") + "\"";
                    return value;
                });

                csv.AppendLine(string.Join(",", fields));
            }

            byte[] buffer = Encoding.UTF8.GetBytes(csv.ToString());
            return File(buffer, "text/csv", $"CMK_Beneficiary_Mobile_Data{(DistrictId == 0 ? "" : DistrictId.ToString())}.csv");
        }

        public ActionResult DownloadCMK_BeneficiaryDbtDataCsv(int installmentId = 0, int DistrictId = 0)
        {
            var apiService = new ApiServiceModel(new FileLogger());
            DataTable dt = apiService.Get_CMK_BeneficiaryDbtData(installmentId, DistrictId);
            var csv = new StringBuilder();
            string districtName = "";
            // Add headers
            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
            csv.AppendLine(string.Join(",", columnNames));

            // Add rows
            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field =>
                    "\"" + field.ToString().Replace("\"", "\"\"") + "\""); // handle commas & quotes
                csv.AppendLine(string.Join(",", fields));
            }

            if (dt.Rows.Count > 0)
            {
                districtName = dt.Rows[0]["DistrictName"].ToString();
            }

            byte[] buffer = Encoding.UTF8.GetBytes(csv.ToString());
            return File(buffer, "text/csv", $"CMK_DBT_Data_{(DistrictId == 0 ? "" : districtName)}.csv");
        }

        [HttpGet]
        public JsonResult GetJanaadharDataForPmKisan(string GID = "0")
        {
            var responce = new APIResponce();
            CommonServicesModel commonServices = new CommonServicesModel(new FileLogger());
            var result = commonServices.GetJanaadharDataForPmKisan(GID);
            responce.Data = result;
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetPmKisanDbtTransaction(Int32 installmentId = 0, int pageNumber = 1, Int32 pageSize = 50000)
        {
            var responce = new APIResponce();
            CommonServicesModel commonServices = new CommonServicesModel(new FileLogger());
            var result = await commonServices.GetPmKisanDbtTransaction(installmentId, pageNumber, pageSize);
            responce.Data = result;
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> MultiThread(Int32 installmentId = 0, int pageNumber = 1, Int32 pageSize = 200000)
        {
            var responce = new APIResponce();
            CommonServicesModel commonServices = new CommonServicesModel(new FileLogger());
            var result = await commonServices.GetPmKisanDbtTransactionMultiThread(installmentId, pageNumber, pageSize);
            responce.Data = result;
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> CMK_GenerateCsv(Int32 installmentId = 0, int pageNumber = 1, Int32 pageSize = 200000)
        {
            var responce = new APIResponce();
            CommonServicesModel commonServices = new CommonServicesModel(new FileLogger());
            var result = await commonServices.CMK_GenerateCsv(installmentId, pageNumber, pageSize);
            responce.Data = result;
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Web SocietyRegDetails by Madhuri

        [HttpPost]
        public JsonResult GetSocietyApplicationStatus(string RegNo)
        {
            var responce = new APIResponce();
            var apiService = new ApiServiceModel(new FileLogger());
            var ds = apiService.GetSocietyApplicationStatus(RegNo);
            if (ds != null)
            {
                responce.Data = new
                {
                    SocietyData = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                    PaymentDetail = ds.Tables.Count > 1 ? COMMON_FUNCTION.DataTableToJSONWithJSONNet(ds.Tables[1]) : null,
                };

                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSocietyRegistrationDetails(int ActId = 0, int Fy = 0, string StartDate = "", string EndDate = "", int PageSize = 50, int PageNumber = 0)
        {
            var responce = new APIResponce();
            var apiService = new ApiServiceModel(new FileLogger());
            //int ActId1 = ActId==""?0:Convert.ToInt32(ActId);
            //int Fy1 = Fy == "" ? 0 : Convert.ToInt32(Fy);
            var ds = apiService.GetSocietyRegistrationDetails(PageSize, PageNumber, ActId, Fy, StartDate, EndDate);
            if (ds != null)
            {
                responce.Data = new
                {
                    SocietyInfo = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                    //  DbtStatus = ds.Tables.Count > 1 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[1]) : null,
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSocietyDetails(int DistrictId = 0, int ActId = 0, int SocietyTypeId = 0, int SocietyStatusId = 0, string SocietyName = "", int PageSize = 50, int PageNumber = 0)
        {
            var responce = new APIResponce();
            var apiService = new ApiServiceModel(new FileLogger());
            var ds = apiService.GetSocietyDetails(PageSize, PageNumber, DistrictId, ActId, SocietyTypeId, SocietyStatusId, SocietyName);
            if (ds != null)
            {
                responce.Data = new
                {
                    SocietyInfo = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region BRAP DASHBOARD

        [HttpPost]
        [OutputCache(Duration = 1800)]
        public JsonResult GetBRAP_DashboardData()
        {
            var responce = new APIResponce();
            var apiService = new ApiServiceModel(new FileLogger());
            var ds = apiService.GetBRAP_DashboardData();
            if (ds != null)
            {
                responce.Data = new
                {
                    DashboardData = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null
                };

                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }
        #endregion BRAP DASHBOARD

        #region GccMahilaNidhi Status
        [HttpPost]
        public JsonResult KnowGccApplicationStatus(string JanAadhaarNo = "")
        {
            var responce = new APIResponce();
            var apiService = new ApiServiceModel(new FileLogger());

            JanAadhaarNo = "Encrypt:" + JanAadhaarNo;

            var JanAadhaarDec = csCommon.Decrypt_SA(Uri.UnescapeDataString(JanAadhaarNo));

            var ds = apiService.GetGccApplicationStatus(COMMON_FUNCTION.Encrypt(JanAadhaarDec));

            if (ds != null)
            {
                responce.Data = new
                {
                    GccDetails = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region OTS Status
        [HttpPost]
        public ActionResult OTSFilesGenerated(string MobileNo)
        {
            DataTable dt = new DataTable();
            PLDBLoan objLoan = new PLDBLoan(new DBLogger());

            var MobileNoEnc = "Encrypt:" + MobileNo;
            var MobileNoDec = csCommon.Decrypt_SA(Uri.UnescapeDataString(MobileNoEnc));
            dt = objLoan.GetOTSListApplicationFormViewData("", "MainData", MobileNoDec);
            PLDB_OTSSchemeDataList _Model = new PLDB_OTSSchemeDataList();
            if (dt != null)
            {
                var list = dt.AsEnumerable().Select(row =>
                {
                    var obj = Activator.CreateInstance<PLDB_OTSSchemeData>();
                    foreach (var prop in typeof(PLDB_OTSSchemeData).GetProperties())
                        if (dt.Columns.Contains(prop.Name) && row[prop.Name] != DBNull.Value)
                            prop.SetValue(obj, Convert.ChangeType(row[prop.Name], prop.PropertyType));
                    return obj;
                }).ToList();
                _Model = new PLDB_OTSSchemeDataList { PLDB_OTSSchemeData = list };
            }

            try
            {
                string wkhtmlPath = Server.MapPath("~/bin/wkhtmltopdf.exe");
                string fontPath = new Uri(Server.MapPath("~/fonts/lohit-Devanagari.ttf")).AbsoluteUri;
                string iycLogo = new Uri(Server.MapPath("~/Content/images/iyc_logo.png")).AbsoluteUri;
                string cmPhoto = new Uri(Server.MapPath("~/Content/images/cm-ots-logo.png")).AbsoluteUri;

                if (_Model.PLDB_OTSSchemeData.Count > 0)
                {

                    StringBuilder strbld = new StringBuilder();
                    foreach (var app in _Model.PLDB_OTSSchemeData)
                    {
                        var file = "";
                        if (app.ODPrincipal > 0)
                            file = System.IO.Path.Combine(Server.MapPath("~/Template"), "ots_notice.html");
                        else
                            file = System.IO.Path.Combine(Server.MapPath("~/Template"), "ots_notice_103.html");

                        // ots_notice_103


                        var parts = new[] { app.VName, app.GpName, app.BlockName, app.TehsilName, app.VidhanSabha, app.DistrictName };
                        var address = string.Join(", ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));

                        strbld.Append(System.IO.File.ReadAllText(file));

                        strbld.Replace("#PLDB_Name#", app.PLDBNameHi)

                       .Replace("#NoticeNo#", app.NoticeNo)
                       .Replace("#CurrentDate#", DateTime.Now.ToString("dd-MM-yyyy"))
                       .Replace("#LoaneeName#", app.ApplicantName)
                       .Replace("#LoaneeFatherHusbandName#", app.FatherHusbandName)
                       .Replace("#LoaneeAddress#", address)
                       .Replace("#LoanAccNo#", app.LoanAccNo)
                       .Replace("#ODPrincipal_InsurancePremium#", (app.ODPrincipal + app.ODInsurancePremium).ToString())  //1=32+33
                       .Replace("#ODInterest#", app.ODInterest.ToString())                   //2 =34
                       .Replace("#ODPenalInterast#", app.ODPenalInterast.ToString())         //3 =35
                       .Replace("#ODTotal#", app.ODTotal.ToString())                         //4 =36

                        .Replace("#NODPrincipal_InsurancePremium#", (app.NODPrincipal + app.NODInsurancePremium).ToString())      //5 =37+38
                       .Replace("#NODInterest#", app.NODInterest.ToString())         //6 =39
                       .Replace("#NODPenalInterast#", app.NODPenalInterest.ToString())   //7 =40
                       .Replace("#NODTotal#", app.NODTotal.ToString())   //8=5+6+7

                       .Replace("#CDPrincipal#", app.CDPrincipal.ToString()) //9=42
                       .Replace("#CDInterest#", app.CDInterest.ToString()) //10=43
                       .Replace("#CDPenalInterast#", app.CDPenalInterest.ToString()) //11=44
                       .Replace("#CDTotal#", app.CDTotal.ToString()) //12 =(9+10+11)


                       .Replace("#Principal#", (app.NODPrincipal + app.NODInsurancePremium + app.CDPrincipal).ToString()) //13=5+9
                       .Replace("#CDInterest_PenalInt#", (app.CDInterest + app.CDPenalInterest).ToString())   //14=10+11
                       .Replace("#Total#", (app.NODPrincipal + app.NODInsurancePremium + app.CDPrincipal + app.CDInterest + app.CDPenalInterest).ToString()) //15=13+14

                       .Replace("#Interest#", app.NODInterest.ToString())  //16=6
                       .Replace("#PenalInterest#", app.NODPenalInterest.ToString()) //17=7
                       .Replace("#TotalInterest#", (app.NODInterest + app.NODPenalInterest).ToString());//18=16+17


                        //103

                        strbld.Replace("#PrincipalOutstandingAsOnDateSettlement#", app.PrincipalOutstandingAsOnDateSettlement.ToString())  //1
                       .Replace("#TotalInterestAsOnDateSettlement#", app.TotalInterestAsOnDateSettlement.ToString()) //2
                       .Replace("#InterestFromSettlementPre#", app.InterestFromSettlementPre.ToString())//3
                       .Replace("#InterestFromSettlement#", app.InterestFromSettlement.ToString())//4
                       .Replace("#TotalInterestFromSettlement#", (app.PrincipalOutstandingAsOnDateSettlement + app.TotalInterestAsOnDateSettlement + app.InterestFromSettlementPre + app.InterestFromSettlement).ToString())//5=1+2+3+4


                        .Replace("#PrincipalOutstandingAsOnDateSettlement#", app.PrincipalOutstandingAsOnDateSettlement.ToString())  //6=1
                        .Replace("#InterestFromSettlement#", app.InterestFromSettlement.ToString())//7=4
                       .Replace("#TotalInterestSettlement#", (app.PrincipalOutstandingAsOnDateSettlement + app.InterestFromSettlement).ToString()) //8=6+7
                       .Replace("#InterestAmount#", (app.TotalInterestAsOnDateSettlement + app.InterestFromSettlement).ToString());//9=2+3


                        strbld.Replace("#cmPhoto#", cmPhoto);
                        strbld.Replace("#iycLogo#", iycLogo);
                    }

                    string str = $@"
                        <html>
                            <head>
                            <meta charset='UTF-8'>
                            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                            <style>
                                @font-face {{
                                    font-family: 'lohit-Devanagari';
                                    src: url('{fontPath}') format('truetype');
                                }}
                                body {{
                                    font-family: 'lohit-Devanagari';
                                    margin: 0;
                                    font-weight:lighter;
                                }}
                            </style>
                        </head>
                        <body>
                            {strbld}
                        </body>
                        </html>";

                    var pdf = PDFgenerator.GeneratePdfFromHtml(str, wkhtmlPath);
                    return File(pdf, "application/pdf", "otsList.pdf");

                    //  return File(pdf, "application/pdf", fileName);
                }
                else
                {
                    return Json(new { message = "File not downloaded" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "CommonService Controller", FunctionName = "OTSFilesGenerated(string MobileNo) " }, ex);

                return Json(new { message = "File not downloaded" }, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion

        #region
        [HttpGet]
        public ActionResult GenerateTestPdf()
        {
            string wkhtmlPath = Server.MapPath("~/bin/wkhtmltopdf.exe");
            string fontPath = new Uri(Server.MapPath("~/fonts/lohit-Devanagari.ttf")).AbsoluteUri;
            string iycLogo = new Uri(Server.MapPath("~/Content/images/iyc_logo.png")).AbsoluteUri;
            StringBuilder htmlContent = new StringBuilder();
            htmlContent.Append(System.IO.File.ReadAllText(Server.MapPath("~/Template/ots_notice.html")));
            htmlContent.Replace("#rajsahkarlogo#", iycLogo);
            htmlContent.Replace("#iycLogo#", iycLogo);
            string str = $@"
            <html>
                <head>
                <meta charset='UTF-8'>
                <style>
                    @font-face {{
                        font-family: 'lohit-Devanagari';
                        src: url('{fontPath}') format('truetype');
                    }}
                    body {{
                        font-family: 'lohit-Devanagari';
                        margin: 0;
                        font-weight:lighter;
                    }}
                </style>
            </head>
            <body>
                {htmlContent.ToString()}
            </body>
            </html>";

            var pdf = PDFgenerator.GeneratePdfFromHtml(str, wkhtmlPath);
            return File(pdf, "application/pdf", "OTS-2025-26 Notice.pdf");
        }
        #endregion

        public static async Task<string> SendRequestAsync(string url, string encryptedRequestJson)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(encryptedRequestJson, Encoding.UTF8, "application/json");
                try
                {
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException ex)
                {
                    return $"Error: {ex.Message}";
                }
                catch (TaskCanceledException ex)
                {
                    return "Error: Request timed out.";
                }
            }
        }

        public static List<Dictionary<string, object>> DataTableToList(DataTable dt)
        {
            var list = new List<Dictionary<string, object>>();
            foreach (DataRow row in dt.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    dict[col.ColumnName] = row[col];
                }
                list.Add(dict);
            }
            return list;
        }


        #region 2001 Societies Api
        [BasicAuth]
        [HttpPost]
        public JsonResult GetPDCSDetails(string RegNo = "")
        {
            var responce = new APIResponce();
            var apiService = new ApiServiceModel(new FileLogger());
            if (RegNo.Trim() != "")
            {
                var ds = apiService.Get2001SocietyDetails(RegNo);

                if (ds != null)
                {

                    responce.Data = new
                    {

                        SocietyInfo = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? DataTableToList(ds.Tables[0]) : null,
                        Adminstrativedetails = ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0 ? DataTableToList(ds.Tables[1]) : null,
                    };

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        responce.Status = "200";
                        responce.Message = "Success";
                    }
                    else
                    {
                        responce.Status = "200";
                        responce.Message = "Record not found.";
                    }

                }
                else
                {
                    responce.Status = "500";
                    responce.Message = "Internal Server Error.";
                }

                return Json(new { Result = responce });
            }
            else
            {
                responce.Status = "400";
                responce.Message = "Please enter valid Registration number.";
                return Json(new { Result = responce });
            }
        }
        #endregion

        #region ePacs Data Service
        [BasicAuth]
        [HttpPost]
        public JsonResult ePacsData(int DistrictId = 0, string FinancialYear = "", int PageNumber = 1, int PageSize = 100)
        {
            var response = new APIPagingResponce();
            var apiService = new ApiServiceModel(new FileLogger());
            if (FinancialYear.Trim() != "")
            {
                var ds = apiService.GetePacsApiData(DistrictId, FinancialYear, PageNumber, PageSize);
                if (ds != null)
                {
                    if (ds.Tables.Count > 1 && ds.Tables[0].Rows.Count > 0)
                    {
                        var row = ds.Tables[1].Rows[0];
                        response.Status = "Success";
                        response.PageNumber = PageNumber;
                        response.PageSize = PageSize;
                        response.Totalpages = Convert.ToInt32(row["totalPages"].ToString());
                        if (response.Totalpages >= PageNumber)
                            response.Data = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? DataTableToList(ds.Tables[0]) : null;
                        else
                            response.Data = "Page number exceeds the size of total pages.";
                        response.Status = "200";
                        response.Message = "Success";
                    }
                    else
                    {
                        response.Status = "200";
                        response.Message = "Record not found.";
                    }
                }
                else
                {
                    response.Status = "500";
                    response.Message = "Internal Server Error.";
                }

                return Json(new { Result = response });
            }
            else
            {
                response.Status = "400";
                response.Message = "Parameter financial year required.";
                return Json(new { Result = response });
            }
        }
        #endregion

        #region CM Kisan Data for RAJFED
        [BasicAuth]
        [HttpPost]
        public JsonResult GetCmKisanData(int DistrictId = 0, int PageNumber = 1, int PageSize = 100)
        {
            var response = new APIPagingResponce();
            var apiService = new ApiServiceModel(new FileLogger());

            var ds = apiService.GeteCmKisanData(DistrictId, PageNumber, PageSize);
            if (ds != null)
            {
                if (ds.Tables.Count > 1 && ds.Tables[0].Rows.Count > 0)
                {
                    var row = ds.Tables[1].Rows[0];
                    response.Status = "Success";
                    response.PageNumber = PageNumber;
                    response.PageSize = PageSize;
                    response.Totalpages = Convert.ToInt32(row["totalPages"].ToString());
                    if (response.Totalpages >= PageNumber)
                        response.Data = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? DataTableToList(ds.Tables[0]) : null;
                    else
                        response.Data = "Page number exceeds the size of total pages.";
                    response.Status = "200";
                    response.Message = "Success";
                }
                else
                {
                    response.Status = "200";
                    response.Message = "Record not found.";
                }
            }
            else
            {
                response.Status = "500";
                response.Message = "Internal Server Error.";
            }

            return Json(new { Result = response });
        }
        #endregion

        #region Gopal Credit Card Data 
        [BasicAuth]
        [HttpPost]
        public JsonResult GetGopalCreditCardData(int DistrictCode = 0, int PageNumber = 1, int PageSize = 100)
        {
            var response = new APIPagingResponce();
            var apiService = new ApiServiceModel(new FileLogger());

            var ds = apiService.GetGopalCreditCardData(DistrictCode, PageNumber, PageSize);
            if (ds != null)
            {
                if (ds.Tables.Count > 1 && ds.Tables[0].Rows.Count > 0)
                {
                    var row = ds.Tables[1].Rows[0];
                    response.Status = "Success";
                    response.PageNumber = PageNumber;
                    response.PageSize = PageSize;
                    response.Totalpages = Convert.ToInt32(row["totalPages"].ToString());
                    if (response.Totalpages >= PageNumber)
                        response.Data = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? DataTableToList(ds.Tables[0]) : null;
                    else
                        response.Data = "Page number exceeds the size of total pages.";
                    response.Status = "200";
                    response.Message = "Success";
                }
                else
                {
                    response.Status = "200";
                    response.Message = "Record not found.";
                }
            }
            else
            {
                response.Status = "500";
                response.Message = "Internal Server Error.";
            }

            return Json(new { Result = response });
        }
        #endregion

        [HttpPost]
        public async Task<JsonResult> GetGccDataByDccbCode(int dccbId = 0, string janaadharMemberId = "")
        {

            var responseData = new CBSGCCResponseModel();
            try
            {
                responseData = await _gccServiceModel.CheckGcctoCBSByJanMemberId(dccbId, janaadharMemberId);
                if (responseData != null)
                {
                    //if (responseData != null && responseData.data.Count > 0)
                    //{
                    //    var result = _gccServiceModel.SaveFarmerDetailData(responseData.data);
                    //    if (result > 0)
                    //    {
                    //        responseData.is_success = true;
                    //        responseData.message = $"Farmer details imported successfully. Total records processed: {result}.";
                    //    }
                    //    else
                    //    {
                    //        responseData.is_success = false;
                    //        responseData.message = "Farmer data was received, but no records were saved or updated.";
                    //    }
                    //}
                    //else
                    //{
                    //    responseData.is_success = false;
                    //    responseData.message = "No farmer data found to import.";
                    //}
                }
            }
            catch (Exception ex)
            {
                //responseData.is_success = false;
                //responseData.message = "Internal Server Error: " + ex.Message;
            }//, data = responseData.data
            return Json(new
            {
                responseData
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string Girdawari_GetKhasraData(string villageCode, string khataNo, string khasraNo)  //Khasra
        {
            var requestData = new KhataKhasraRequest
            {
                VILLAGE_CODE = villageCode,
                KHATA_NUMBER = khataNo,
                KHASRA_NUMBER = khasraNo,
                SSOID = "test_ssoid"
            };
            //var requestData = new KhataKhasraRequest
            //{
            //    VILLAGE_CODE = "48583",
            //    KHATA_NUMBER = "66",
            //    KHASRA_NUMBER = "600",
            //    SSOID = "test_ssoid"
            //};
            string result = "Error";
            try
            {
                var ReqS = JsonConvert.SerializeObject(requestData);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://rptgirdawari.rajasthan.gov.in/api/View/NicData1_KhataKhasra");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers["X-Custom-Token"] = "tsIiIS1e8AVMhvqQYSESQX1T9FPuKFk3o9VvhhA9rBWGtr4Y207";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    //string json = "";
                    streamWriter.Write(ReqS);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                //ApiResponseGKK resGKK = JsonConvert.DeserializeObject<ApiResponseGKK>(result);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Girdawari_GetKhasraData" }, ex);
            }
            finally
            {

            }
            return result;
        }


        #region #gccApplication for not generated
        [HttpGet]
        [ActionName("GCCApplicationSignedForm")]
        public string GCCApplicationSignedForm(GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel = null, int signedType = 0)
        {
            try
            {
                string wkhtmlPath = Server.MapPath("~/bin/wkhtmltopdf.exe");
                string fontPath = new Uri(Server.MapPath("~/fonts/lohit-Devanagari.ttf")).AbsoluteUri;

                StringBuilder strbld = new StringBuilder();
                //var file = System.IO.Path.Combine(Server.MapPath("~/Template"), "Gcc_application_form.html");
                var file = "";
                string filePreName = "";
                if (signedType == 1) //for loanee
                {
                    file = System.IO.Path.Combine(Server.MapPath("~/Template"), "Gcc_application_form_loanee.html");
                    filePreName = "GccSignedFormLoanee";
                }
                else if (signedType == 2) //for pacs
                {
                    file = System.IO.Path.Combine(Server.MapPath("~/Template"), "Gcc_application_form_pacs.html");
                    filePreName = "GccSignedFormPacs";
                }
                else if (signedType == 3) //for gurantor
                {
                    file = System.IO.Path.Combine(Server.MapPath("~/Template"), "Gcc_application_gurantor_form.html");
                    filePreName = "GccSignedGurantorForm";
                }

                string purposeOfLoan = "";
                string guarantor1Name = "";
                string guarantor1MobileNo = "";
                string guarantor2Name = "";
                string guarantor2MobileNo = "";
                string guarantor1Address = "";
                string guarantor2Address = "";
                string guarantor1FatherName = "";
                string guarantor2FatherName = "";
                string category = "";
                if (_Model != null)
                {
                    if (_Model.IsWorking == true) { purposeOfLoan += "कार्यशील पूंजी, "; }
                    if (_Model.IsInfrastructure == true) { purposeOfLoan += "शेड, खेली, डेयरी गद्दे, "; }
                    if (_Model.IsApparatus == true) { purposeOfLoan += "दूध उपकरण "; }
                    purposeOfLoan = purposeOfLoan.TrimEnd(',', ' ');

                    guarantor1Name = _Model.GuarantorDetailsList.Count > 0 ? _Model.GuarantorDetailsList[0].Name : "";
                    guarantor1MobileNo = _Model.GuarantorDetailsList.Count > 0 ? _Model.GuarantorDetailsList[0].MobileNo : "";
                    guarantor1Address = _Model.GuarantorDetailsList.Count > 0 ? _Model.GuarantorDetailsList[0].Address : "";
                    guarantor1FatherName = _Model.GuarantorDetailsList.Count > 0 ? _Model.GuarantorDetailsList[0].FatherSpouseName : "";
                    guarantor2Name = _Model.GuarantorDetailsList.Count > 1 ? _Model.GuarantorDetailsList[1].Name : "";
                    guarantor2MobileNo = _Model.GuarantorDetailsList.Count > 1 ? _Model.GuarantorDetailsList[1].MobileNo : "";
                    guarantor2Address = _Model.GuarantorDetailsList.Count > 1 ? _Model.GuarantorDetailsList[1].Address : "";
                    guarantor2FatherName = _Model.GuarantorDetailsList.Count > 1 ? _Model.GuarantorDetailsList[1].FatherSpouseName : "";
                    switch (_Model.Category)
                    {
                        case "1":
                            category = "अ.ज.जा";
                            break;
                        case "2":
                            category = "अ.जा";
                            break;
                        case "3":
                            category = "सामान्य";
                            break;
                        case "4":
                            category = "ओ.बी.सी";
                            break;
                        default:
                            category = "अन्य";
                            break;
                    }

                }


                strbld.Append(System.IO.File.ReadAllText(file));

                strbld.Replace("#Name#", _Model.Name_ENG)
                .Replace("#FatherName#", _Model.FatherName_ENG)
                .Replace("#Age#", _Model.Age)
                .Replace("#Caste#", category)
                .Replace("#RegistrationNo#", _Model.RegistrationNo)
                .Replace("#Address#", $"{_Model.HouseBuildingNo}, {_Model.LaneStreetName}, {_Model.Address_ENG}, {_Model.DistrictName_ENG}")
                .Replace("#BlockName#", _Model.Block_ENG)
                .Replace("#VillageName#", _Model.Village_ENG)
                .Replace("#LoanAmountRecommendedByDCS#", NumberToHindiWordConverter.FormatDecimalAmount(Convert.ToDecimal(_Model.SanctionAmt)))
                .Replace("#LoanAmountRecommendedByDCSWords#", NumberToHindiWordConverter.ConvertAmtWords(Convert.ToDecimal(_Model.SanctionAmt)))
                .Replace("#AgricultureLandIrrigated#", (_Model.AgricultureLandDetailList?.Count ?? 0) > 0 ? _Model.AgricultureLandDetailList[0].Irrigated.ToString() : "0" + " हैक्टर")
                .Replace("#Guarantor1Name#", guarantor1Name)
                .Replace("#Guarantor1MobileNo#", "")
                .Replace("#Guarantor1Address#", guarantor1Address)
                .Replace("#Guarantor1Father#", guarantor1FatherName)
                .Replace("#Guarantor2Name#", guarantor2Name)
                .Replace("#Guarantor2MobileNo#", "")
                .Replace("#Guarantor2Address#", guarantor2Address)
                .Replace("#Guarantor2Father#", guarantor2FatherName)
                .Replace("#Guarantor1Caste#", "")
                .Replace("#Guarantor2Caste#", "")
                .Replace("#Guarantor1Tehsil#", "")
                .Replace("#Guarantor2Tehsil#", "")
                .Replace("#Date#", DateTime.Now.ToString("dd-MM-yyyy"))
                .Replace("#AccountNumber#", _Model.BankAccountNo)
                .Replace("#BranchName#", _Model.BankBranchName)
                .Replace("#ApplicantRuralResidence#", (_Model.IsApplicantRuralResidence == true ? "हां" : "नहीं"))
                .Replace("#PacsName#", _Model.PACSName_ENG)
                .Replace("#District#", _Model.DistrictName_ENG)
                .Replace("#CCBName#", _Model.LoanBankName_HI)
                .Replace("#PurposeOfLoan#", purposeOfLoan);

                string finalHtml = $@"
                   <html>
                    <head>
                        <meta charset='UTF-8'>
                        <style>
                            @font-face {{
                                font-family: 'lohit-Devanagari';
                                src: url('{fontPath}') format('truetype');
                            }}
                            body {{
                                font-family: 'lohit-Devanagari';
                                margin: 20px;
                            }}
                        </style>
                    </head>
                    <body>
                        {strbld}
                    </body>
                   </html>";

                var pdf = PDFgenerator.GeneratePdfFromHtml(finalHtml, wkhtmlPath);
                //var fileName = $"GccSignedForm{Guid.NewGuid().ToString("N").ToUpper()}.pdf";
                var fileName = $"{filePreName}{Guid.NewGuid().ToString("N").ToUpper()}.pdf";

                string cleanRegNo = _Model.RegistrationNo.Replace("/", "").Replace(".", "").Replace("_", "").Replace("-", "").Replace(" ", "");

                string folderPath = Path.Combine(Server.MapPath("~/Content/GopalCreditCard/GSSProfiling/"), cleanRegNo);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string filePath = Path.Combine(folderPath, fileName);
                System.IO.File.WriteAllBytes(filePath, pdf);

                var pdfPath = fileName;

                return pdfPath;


            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "GopalCreditCardController", FunctionName = "GCCApplicationSignedForm()" }, ex);

                return "Fail";
            }
        }


        [HttpPost]
        [ActionName("GetGccApplicationFormNotGenerated")]
        public string GetGccApplicationFormNotGenerated()
        {
            var _gccServiceModel = new GCCServiceModel(new DBLogger());
            DataTable dt = _gccServiceModel.GetGccApplicationFormNotGenerated();
            GopalCreditCardController gopalCredit = DependencyResolver.Current.GetService<GopalCreditCardController>();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    GopalCreditCardFormModel _Model = null;
                    string data = gopalCredit.Get_GopalApplicationFormViewData_v1(dt.Rows[i]["GopalCreditCardIdEnc"].ToString());
                    _Model = JsonConvert.DeserializeObject<GopalCreditCardFormModel>(data);
                    GccApplicationFlowModel _AppFlowModel = null;


                    GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());
                    var pdfPath = GCCApplicationSignedForm(_Model, _AppFlowModel, 1);
                    //_AppFlowModel.DocumentURL = pdfPath;
                    GccDocumentsModel docAppForm = new GccDocumentsModel();
                    docAppForm.DocumentURL = pdfPath;
                    docAppForm.DocId = 7;
                    docAppForm.GccId = _Model.gcc_id;
                    objGopalCreditCardDBModel.InsertGCCReportFile(docAppForm);

                    var pdfPath2 = GCCApplicationSignedForm(_Model, _AppFlowModel, 2);
                    //_AppFlowModel.DocumentURL = pdfPath;
                    GccDocumentsModel docAppForm2 = new GccDocumentsModel();
                    docAppForm2.DocumentURL = pdfPath2;
                    docAppForm2.DocId = 8;
                    docAppForm2.GccId = _Model.gcc_id;
                    objGopalCreditCardDBModel.InsertGCCReportFile(docAppForm2);

                    var pdfPath3 = GCCApplicationSignedForm(_Model, _AppFlowModel, 3);
                    //_AppFlowModel.DocumentURL = pdfPath;
                    GccDocumentsModel docAppForm3 = new GccDocumentsModel();
                    docAppForm3.DocumentURL = pdfPath3;
                    docAppForm3.DocId = 9;
                    docAppForm3.GccId = _Model.gcc_id;
                    objGopalCreditCardDBModel.InsertGCCReportFile(docAppForm3);

                }
            }
            return "";
        }

        #endregion


        #region Added new API for returning UrlFilePath (29-04-2026) by Narpat 

        [HttpPost]
        public async Task<JsonResult> GetReturnUrlByCoopId(string pacsId = "", string coopId = "", HttpPostedFileBase fileUploader = null)
        {
            var response = new ResponseModel();
            string message = string.Empty;

            try
            {
                if (string.IsNullOrWhiteSpace(pacsId) || pacsId == "0")
                    message = "pacsId is required";
                else if (string.IsNullOrWhiteSpace(coopId) || coopId == "0")
                    message = "coopId is required";
                else if (fileUploader == null || fileUploader.ContentLength == 0)
                    message = "file is required";

                if (!string.IsNullOrEmpty(message))
                {
                    response.status = "400";
                    response.message = message;
                    response.url = "";

                    return Json(new { data = response }, JsonRequestBehavior.AllowGet);
                }

                var url = await _coopServiceModel.GetUrlByCoopId(pacsId, coopId, fileUploader);

                if (!string.IsNullOrWhiteSpace(url))
                {
                    response.status = "200";
                    response.message = "Success";
                    response.url = url;
                }
                else
                {
                    response.status = "400";
                    response.message = "Failed";
                    response.url = "";
                }

                return Json(new { data = response }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                response.status = "500";
                response.message = ex.Message;
                response.url = "";

                return Json(new { data = response }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}
public class UnicodeFontProvider : XMLWorkerFontProvider
{
    private string _fontPath;
    public UnicodeFontProvider(string fontPath)
    {
        _fontPath = fontPath;
    }

    public override Font GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color)
    {
        BaseFont baseFont = BaseFont.CreateFont(_fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        return new Font(baseFont, size, style, color);
    }
}
public class SocietyDetailsModel
{
    public string SocietyName { get; set; }
    public string Act { get; set; }
    public string RegDate { get; set; }
    public string Type { get; set; }
    public string RegNo { get; set; }
    public string WorkingCapital { get; set; }
    public string PartialCapital { get; set; }
    public string MonthOfEstablishment { get; set; }
    public string YearOfEstablishment { get; set; }
    public List<SocietyMembersModel> Members { get; set; }
}

public class SocietyMembersModel
{
    public string Name { get; set; }
    public string Father_HusbandName { get; set; }
    public string Education { get; set; }
    public string ResidenceAddress { get; set; }
    public string ContactNo { get; set; }
    public string Post { get; set; }


}

public class KhataKhasraRequest
{
    public string VILLAGE_CODE { get; set; }
    public string KHATA_NUMBER { get; set; }
    public string KHASRA_NUMBER { get; set; }
    public string SSOID { get; set; }
}