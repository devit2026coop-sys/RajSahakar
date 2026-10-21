using Cooperative.Filters;
using Cooperative.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Mvc;

namespace Cooperative.Controllers
{
    public class AdminDashboardController : Controller
    {
        // GET: AdminDashboard
        public ActionResult Dashboard(string Alfa, string Beta)
        {
            Beta = Beta.ToString().Replace(" ", "+");
            Beta = Beta.ToString().Replace(" ", "+");
            Beta = Beta.ToString().Replace(" ", "+");
            //string S = COMMON_FUNCTION.Encrypt("abhay.kumar");
            //string P = COMMON_FUNCTION.Encrypt("COOP_RAJASTHAN");
            string uid = COMMON_FUNCTION.Decrypt(Alfa);
            string passs = COMMON_FUNCTION.Decrypt(Beta);
            if (uid == "abhay.kumar" && passs == "COOP_RAJASTHAN")
                return View();
            else
                Response.Redirect("https://sso.rajasthan.gov.in");

            return View();
        }

        [HttpPost]
        [ActionName("Getlwadata")]
        public string lwadata()
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

            return result;

        }

        [HttpPost]
        public string BhandarMprDataForDashboard()
        {
            BhandarFormDataEntity _Model = new BhandarFormDataEntity();
            string WhereClause = " Where 1=1";

            if (DateTime.Now.Month < 4)
            {
                WhereClause += " FinancialYear.FIN_YEAR_END = " + DateTime.Now.Year;
            }
            if (DateTime.Now.Month > 3)
            {
                WhereClause += " FinancialYear.FIN_YEAR_START = " + DateTime.Now.Year;
            }
            DataTable DT = null;
            //if (_Model.BhandarId == _loggedInUSerDetails.DistrictId)
            //{
            EntryFormsModel objEntryForms = new EntryFormsModel(new DBLogger());
            DT = objEntryForms.BhandarMprDataForDashboard(_Model);
            //}
            return COMMON_FUNCTION.DataTableToJSONString(DT);

        }

        [HttpGet]
        [ActionName("GetConfedData")]
        public string GetConfedOrderDetails(string YearWiseOrAll)
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
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserControl", FunctionName = "GetConfedData" }, ex);
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

        [HttpGet]
        [ActionName("GetSMFApplicationStatusForDashboard")]
        public string GetSMFApplicationStatus()
        {
            UserRegistrationModel objUserRoleModel = new UserRegistrationModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objUserRoleModel.GetSMFApplicationStatus());
        }

        [HttpGet]
        [ActionName("GetUserCountForDashBoard")]
        public string GetUserCountForDashBoard()
        {
            UserModel objUserModel = new UserModel();
            return COMMON_FUNCTION.DataTableToJSONString(objUserModel.GetUserCountForDashBoard());
        }

        [HttpGet]
        [ActionName("GetRajFedData")]
        public string GetRajFedData(SocietyRegistrationEntity objSocietyRegistrationEntity)
        {
            var result = "";
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
                if (theDataSet.Tables.Count > 0)
                    result = COMMON_FUNCTION.DataTableToJSONString(theDataSet.Tables[0]);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.UserControl", FunctionName = "GetRajFedData" }, ex);
            }
            finally
            {

            }
            return result;
        }

        [HttpGet]
        [ActionName("GetDeshboardData")]
        public string GetDeshboard(SocietyRegistrationEntity objSocietyRegistrationEntity)
        {


            objSocietyRegistrationEntity.UserType = "D";
            objSocietyRegistrationEntity.SSOID = "abhay.kumar";

            objSocietyRegistrationEntity.LevelId = 1;
            objSocietyRegistrationEntity.DivisionId = 0;
            objSocietyRegistrationEntity.DistrictId = 0;
            objSocietyRegistrationEntity.CityId = 0;
            objSocietyRegistrationEntity.WardId = 0;
            objSocietyRegistrationEntity.PanchayatSamitiId = 0;
            objSocietyRegistrationEntity.GPId = 0;
            objSocietyRegistrationEntity.RoleId = 1;



            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            DataSet DT = objCitizenModel.getdeshboad(objSocietyRegistrationEntity);
            //return COMMON_FUNCTION.DataTableToJSONString(DT);
            return JsonConvert.SerializeObject(DT, Newtonsoft.Json.Formatting.Indented);


        }

        [HttpGet]
        [ActionName("GetPacsMemberCountReport")]
        public string GetPacsMemberCountReport()
        {
            UserModel objUserModel = new UserModel();
            return COMMON_FUNCTION.DataTableToJSONString(objUserModel.GetPacsMemberCountReport());
        }

        [HttpGet]
        [ActionName("GetPacsStatusReport")]
        public string GetPacsStatusReport()
        {
            UserModel objUserModel = new UserModel();
            return COMMON_FUNCTION.DataTableToJSONString(objUserModel.GetPacsStatusReport());
        }

        [HttpGet]
        [ActionName("GetDivisionReport")]
        public string GetDivisionReport(string FYearID, string MonthId)
        {
            UserModel objUserModel = new UserModel();
            return COMMON_FUNCTION.DataTableToJSONString(objUserModel.GetDivisionReport(FYearID, MonthId));
        }

        [HttpGet]
        [ActionName("GetDistrictReport")]
        public string AuditDistrictDataReport(string FYearID, string MonthId, string DivisionId)
        {
            UserModel objUserModel = new UserModel();
            return COMMON_FUNCTION.DataTableToJSONString(objUserModel.AuditDistrictDataReport(FYearID, MonthId, DivisionId));
        }

        [HttpPost]
        public string InspectionDataForDashboard(string FinancialYearId)
        {
            DataTable DT = null;
            DataTable DT2 = null;
            int _FIN_YEAR_ID = 0;
            int.TryParse(FinancialYearId, out _FIN_YEAR_ID);
            InspectionModel objAuditorModel = new InspectionModel(new DBLogger());
            DT = objAuditorModel.GetSocietyDataByLevelIdYearId(1, 3116, _FIN_YEAR_ID);

            InspectionModel objAuditorModel2 = new InspectionModel(new DBLogger());
            DT2 = objAuditorModel2.GetSocietyInspectionAuditStatus(1, 3116, _FIN_YEAR_ID);

            return "{\"data1\":" + COMMON_FUNCTION.DataTableToJSONString(DT) + ",\"data2\": " + COMMON_FUNCTION.DataTableToJSONString(DT2) + "}";
        }

        [ActionName("GetDistrictwiseSocietyByTypeStatus")]
        public string GetDistrictwiseSocietyByTypeStatus(string SocietyType, string SocietyStatus)
        {
            UserRegistrationModel objUserRoleModel = new UserRegistrationModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objUserRoleModel.GetDistrictwiseSocietyByTypeStatus(SocietyType, SocietyStatus));
        }

        [HttpGet]
        [ActionName("GetStLoanDataForDashBoard")]
        public string GetStLoanDataForDashBoard()
        {
            UserModel objUserModel = new UserModel();
            return COMMON_FUNCTION.DataTableToJSONString(objUserModel.GetStLoanDataForDashBoard());
        }

        [HttpPost]
        [ActionName("GetAuditReportRegionWise")]
        public string GetAuditReportRegionWise(int Year, int Month, int Type)
        {
            MISModel objMISModel = new MISModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objMISModel.GetAuditReportRegionWise(Year, Month, Type);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }

        [HttpGet]
        [ActionName("GetUserLevelWiseCount")]
        public string GetUserLevelWiseCount()
        {
            UserRegistrationModel objUserRoleModel = new UserRegistrationModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objUserRoleModel.GetUserLevelWiseCount());
        }

        [HttpGet]
        [ActionName("HousingSocietyAuditsCountYearWise")]
        public string GetHousingSocietyAuditsCountYearWise()
        {
            UserRegistrationModel objUserRoleModel = new UserRegistrationModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objUserRoleModel.GetHousingSocietyAuditsCountYearWise());
        }

       
    }
}