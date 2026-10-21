using CoopDAL;
using CoopDB;
using Cooperative.Filters;
using Cooperative.Models;
using javax.xml.stream;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml.Drawing.Slicer.Style;
using sun.swing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.UI;
using System.Xml;
using static iTextSharp.text.pdf.AcroFields;
using static iTextSharp.text.pdf.PdfDocument;
using static javax.net.ssl.SSLEngineResult;
using static QRCoder.PayloadGenerator;

namespace Cooperative.Controllers
{

    [Error]
    [CustomAuthentication]
    [LogActionFilter]
    public class SocietyController : BaseController
    {

        UserEntity _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];


        [HttpGet]
        public ActionResult AuditorData()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.AuditorData, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpGet]
        public ActionResult SocietyAuditReportUploadByDepartment()
        {
            return View();
        }

        [HttpGet]
        public ActionResult InspactorTargetsAction()
        {
            var response = TempData.Peek("Response")?.ToString();
            ViewBag.response = response;
            if (Request.Cookies[SessionIdentity.Culture] == null)
            {
                Response.Cookies.Add(new System.Web.HttpCookie(Cooperative.Controllers.SessionIdentity.Culture, "")
                { Value = "en-us", HttpOnly = true });
            }
            return View();
        }
        [HttpGet]
        public ActionResult SocietyLiquidationReg()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ObjecedSocieties()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ElectionForm()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FertilizerDemandRequest()
        {
            return View();
        }

        [HttpGet]
        public ActionResult TaggedItemDetails()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SocietyProfiling2001()
        {
            return View();
        }


        [HttpPost]
        [ActionName("SaveLiquidationFormsData")]
        public JsonResult SaveLiquidationFormsData(LiquidationFormDataEntity _Model)
        {
            int result = -1;
            if (_Model != null)
            {

                SocietyModel obj = new SocietyModel();
                ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
                _Model.CreatedBy = _Model.CreatedBy = _loggedInUSerDetails.UserId;
                //  _Model.CreatedIP = _Model.CreatedIP = COMMON_FUNCTION.GetIpAddress();

                result = obj.LiquidationRegistration(_Model);


            }
            return Json(result);
        }


        [HttpPost]
        [ActionName("GetLiquidationData1")]
        public JsonResult GetLiquidationData(int DistrictId, string Month)
        {
            DataTable DT = null;
            SocietyModel objEntryForms = new SocietyModel(new DBLogger());
            DT = objEntryForms.LiquidationMprDataList(DistrictId, Month);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult FileResize()
        {
            return View();
        }

        [HttpPost]
        public string GetFileListForResize(string DisId)
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            DataTable DT = objSocietyModel.GetSocietyListForFileResize(DisId);
            DT.Columns.Add("", typeof(int));

            foreach (DataRow Dr in DT.Rows)
            {
                string SourcePath = "../Content/SocietyProfiling/" + Dr["SocietyDirectory"].ToString() + "/" + Dr["RulesRegulationFile"].ToString();

                if (System.IO.File.Exists(Server.MapPath(SourcePath)))
                {
                    FileInfo info = new FileInfo(Server.MapPath(SourcePath));
                    long length = info.Length / 1024;
                    if (length > 2000)
                    {
                        Dr["IsFileResize"] = 1;
                    }
                    else
                    {
                        Dr["RulesRegulationFile"] = "";
                    }

                    Dr["RulesRegulationFileSize"] = length;
                }

                SourcePath = "../Content/SocietyProfiling/" + Dr["SocietyDirectory"].ToString() + "/" + Dr["BylawsFile"].ToString();

                if (System.IO.File.Exists(Server.MapPath(SourcePath)))
                {
                    FileInfo info = new FileInfo(Server.MapPath(SourcePath));
                    long length = info.Length / 1024;
                    if (length > 2000)
                    {
                        Dr["IsFileResize"] = 1;
                    }
                    else
                    {
                        Dr["BylawsFile"] = "";
                    }

                    Dr["BylawsFileSize"] = length;
                }

                SourcePath = "../Content/SocietyProfiling/" + Dr["SocietyDirectory"].ToString() + "/" + Dr["ObjectsFile"].ToString();

                if (System.IO.File.Exists(Server.MapPath(SourcePath)))
                {
                    FileInfo info = new FileInfo(Server.MapPath(SourcePath));
                    long length = info.Length / 1024;
                    if (length > 2000)
                    {
                        Dr["IsFileResize"] = 1;
                    }
                    else
                    {
                        Dr["ObjectsFile"] = "";
                    }

                    Dr["ObjectsFileSize"] = length;
                }
            }
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }
        //

        [HttpGet]
        public ActionResult AssignAuditor()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.AuditorData, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }


        [HttpGet]
        public ActionResult SocietyAuditReport()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.SocietyAuditReport, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            List<SocietyRegistrationEntity> _SocietyRegistrationData = new List<SocietyRegistrationEntity>();
            CitizenModel objMenuActionModel = new CitizenModel(new FileLogger());
            SocietyRegistrationEntity objSocietyRegistrationEntity = new SocietyRegistrationEntity();
            objSocietyRegistrationEntity.SSOID = "";

            List<SocietyListBySSOID_Result> list = objMenuActionModel.SocietyList(objSocietyRegistrationEntity, "", objSocietyRegistrationEntity.UserType);

            //foreach (var Dr in list)
            //{
            //    SocietyRegistrationEntity objEntity = new SocietyRegistrationEntity();
            //    objEntity.SocietyName = Dr.SocietyName;//["SocietyName"].ToString();
            //    objEntity.SocietyIDEnc = Dr.SocietyIdENC;//["SocietyIdENC"].ToString();
            //    objEntity.SocietyID = Dr.SocietyID;//["SocietyID"].ToString());
            //    objEntity.RegistrationNo = Dr.RegistrationNo;//["RegistrationNo"].ToString();
            //    objEntity.CreationDateStr = Dr.RegistrationDate;//["RegistrationDate"].ToString();
            //    objEntity.ActNameEn = Dr.ActNameEn;//["ActNameEn"].ToString();
            //    objEntity.ActNameHi = Dr.ActNameHi;//["ActNameHi"].ToString();
            //    objEntity.IsActive = Dr.IsActive;//["IsActive"].ToString());
            //    objEntity.IsAdministrativeDetailsAvailable = Dr.IsAdministrativeDetails;//["IsAdministrativeDetails"].ToString());

            //    objEntity.TypeNameEn = Dr.TypeameEng;//["TypeameEng"].ToString();
            //    objEntity.TypeNameHi = Dr.TypeNameHin;//["TypeNameHin"].ToString();
            //    objEntity.Status = Dr.Status;//["Status"].ToString();

            //    _SocietyRegistrationData.Add(objEntity);
            //}


            return View(list);
        }

        [HttpPost]
        public string GetUploadFileType()
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objSocietyModel.GetUploadFileType());
        }


        [HttpPost]
        public string GetAuditData(string IsAuditFirm, string DistrictId = "", int FinancialYear = 0, string WhereClause = "", string Search = "", string IsMapping = "0")
        {
            DataTable Dt = null;
            int _DistrictId = 0;
            if (Search.Length > 0)
            {
                WhereClause = Search.Length > 0 ? (WhereClause = " and replace(ICFAIRegistrationNo,' ','') Like '%" + Search.Replace(" ", "") + "%' OR replace(EnrollmentNo,' ','')  like '%" + Search.Replace(" ", "") + "%' ") : "";
            }
            else
            {
                bool IsAuditForFirm = false;

                if (!bool.TryParse(IsAuditFirm, out IsAuditForFirm))
                    return "{}";

                int.TryParse(DistrictId, out _DistrictId); // 

                WhereClause = _DistrictId > 0 ? (WhereClause = " and DistrictId =" + _DistrictId + "") : "";
                WhereClause += (WhereClause = " and IsFirm =" + (IsAuditForFirm ? "0" : "1") + "");


            }

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            if (_DistrictId > 0)
            {
                Dt = objSocietyModel.GetAuditDataForFirmOrIndividual(WhereClause, FinancialYear);
                if (Search.Length > 0 && Dt.Rows.Count > 0 && IsMapping == "1")
                {
                    System.Web.HttpContext.Current.Session[SessionIdentity.OditerMobile] = Dt.Rows[0]["MobileNo"].ToString();
                }
                else
                {
                    System.Web.HttpContext.Current.Session[SessionIdentity.OditerMobile] = "";
                }
                return COMMON_FUNCTION.DataTableToJSONWithJSONNet(Dt);
            }
            else
            {
                Dt = objSocietyModel.GetAuditDataForFirmOrIndividual(WhereClause, FinancialYear);
                if (Search.Length > 0 && Dt.Rows.Count > 0 && IsMapping == "1")
                {
                    System.Web.HttpContext.Current.Session[SessionIdentity.OditerMobile] = Dt.Rows[0]["MobileNo"].ToString();
                }
                else
                {
                    System.Web.HttpContext.Current.Session[SessionIdentity.OditerMobile] = "";
                }
                return COMMON_FUNCTION.DataTableToJSONWithJSONNet(Dt);
            }

        }



        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

        public ActionResult SocietyRegistration()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.SocietyRegistration, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        public ActionResult SocietyProfilingView()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.SocietyProfilingView, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }
        public ActionResult BankStatement()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.BankStatement, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        public ActionResult CC_Society_Status()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.BankStatement, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }



        public ActionResult BulkInsertByExcel()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.FileUpload, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }


        public ActionResult SocietyAuditReportByDepartment()
        {
            List<SocietyRegistrationEntity> _SocietyRegistrationData = new List<SocietyRegistrationEntity>();
            CitizenModel objMenuActionModel = new CitizenModel(new FileLogger());
            SocietyRegistrationEntity objSocietyRegistrationEntity = new SocietyRegistrationEntity();
            objSocietyRegistrationEntity.SSOID = "";

            List<SocietyListBySSOID_Result> list = objMenuActionModel.SocietyList(objSocietyRegistrationEntity, "", objSocietyRegistrationEntity.UserType);

            //foreach (DataRow Dr in DT.Rows)
            //{
            //    SocietyRegistrationEntity objEntity = new SocietyRegistrationEntity();
            //    objEntity.SocietyName = Dr["SocietyName"].ToString();
            //    objEntity.SocietyID = int.Parse(Dr["SocietyID"].ToString());
            //    objEntity.RegistrationNo = Dr["RegistrationNo"].ToString();
            //    objEntity.CreationDateStr = Dr["RegistrationDate"].ToString();
            //    objEntity.ActNameEn = Dr["ActNameEn"].ToString();
            //    objEntity.ActNameHi = Dr["ActNameHi"].ToString();
            //    objEntity.IsActive = int.Parse(Dr["IsActive"].ToString());
            //    objEntity.IsAdministrativeDetailsAvailable = int.Parse(Dr["IsAdministrativeDetails"].ToString());

            //    objEntity.TypeNameEn = Dr["TypeameEng"].ToString();
            //    objEntity.TypeNameHi = Dr["TypeNameHin"].ToString();
            //    objEntity.Status = Dr["Status"].ToString();

            //    _SocietyRegistrationData.Add(objEntity);
            //}


            return View(list);
        }

        public ActionResult SocietyAdministrativeDetails()
        {
            return View();
        }
        [HttpGet]
        public string GetSocietyProfilingForSocietyDetails()
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objSocietyModel.GetSocietyProfilingDetails());
        }

        [HttpGet]
        public string GetSocietyProfilingForPersonnelDetails()
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objSocietyModel.GetSocietyPersonnelProfilingDetails());
        }


        [HttpGet]
        public string GetSocietyProfilingForSocietyOfficeDetails()
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objSocietyModel.GetSocietyOfficeProfilingDetails());
        }


        [HttpGet]
        public string GetSocietyProfilingForSocietyMembersDetails()
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objSocietyModel.GetSocietyMembersProfilingDetails());
        }

        [HttpPost]
        [ActionName("SaveSocietyProfiling")]
        public string SaveSocietyProfiling(SocietyRegistrationEntity _Entity)//, SocietyRegisteredOfficeDetailsEntity RegisteredOfficeDetailsEntity
        {


            _Entity.CreatedBy = _Entity.UpdatedBy = _LoggedInUser.UserId;

            string flag = "0";
            string fileName = string.Empty;
            string ReturnJSON = "";
            string pathString = "~/Content/NewRegistration/";
            DataTable memberData = new DataTable();
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();

            try
            {
                _Entity.lstSocietyWorkScope = Request.Form["lstSocietyWorkScope"].ToString().Equals("[]") ? new List<SocietyWorkScopeEntity>() : (List<SocietyWorkScopeEntity>)s.Deserialize(Request.Form["lstSocietyWorkScope"].ToString(), typeof(List<SocietyWorkScopeEntity>));

            }
            catch (Exception ex)
            {
                {
                    ILogger objILogger = new DBLogger();
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SocietyController", FunctionName = "Save Society Proffile" }, ex);
                }
                return CommonMessageFactory.Response("Provided input data is not in correct format.");
            }


            try
            {
                //pathString = "~/Content/NewRegistration";
                //NEW CHANGES 16/11/2019
                _Entity.RegistrationNo = _Entity.RegistrationNo == "undefined" ? "" : _Entity.RegistrationNo;
                _Entity.RegistrationNo = _Entity.RegistrationNo == null ? "" : _Entity.RegistrationNo;

                if (_Entity.IsNewRegistration == false || _Entity.RegistrationNo.Length > 1)
                {
                    pathString = "~/Content/SocietyProfiling/";
                    pathString += Regex.Replace(_Entity.RegistrationNo.ToUpper().Trim(), "[^a-zA-Z0-9_]+", "");

                    if (!Directory.Exists(Server.MapPath(pathString)))
                    {
                        Directory.CreateDirectory(Server.MapPath(pathString));
                    }
                }
                //END
                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());

                foreach (string files in Request.Files)
                {

                    HttpPostedFileBase file = Request.Files[files];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));
                    if (files.ToString() == "RegistrationCertificateFile")
                        _Entity.RegistrationCertificateFile = fileName;
                    else if (files.ToString() == "ByeLawsSocietyFile")
                        _Entity.ByeLawsSocietyFile = fileName;
                    else if (files.ToString() == "RulesAndRegulationFile")
                        _Entity.RulesAndRegulationFile = fileName;
                    else if (files.ToString() == "RSSCFileName")
                        _Entity.RSSCFileName = fileName;
                    else if (files.ToString() == "DLSCFileName")
                        _Entity.DLSCFileName = fileName;
                    else if (files.ToString() == "SOCFileName")
                        _Entity.SOCFileName = fileName;
                    else if (files.ToString() == "SLSAFileName")
                        _Entity.SLSAFileName = fileName;

                    else if (files.ToString() == "MemberListExcelFile")
                    {
                        _Entity.MemberListExcelFile = fileName;
                        string result = "";
                        if (_Entity.ActId == 7)
                        {
                            result = ValidateExcelSportAct(ref memberData, Path.Combine(Server.MapPath(pathString), fileName));
                        }
                        else
                        {
                            result = ValidateExcel(ref memberData, Path.Combine(Server.MapPath(pathString), fileName));
                        }

                        if (result != "OK")
                        {
                            return CommonMessageFactory.GetJsonStringResponse("true", "ErrorInMemberdata@" + result);
                        }
                        // objSocietyModel.UploadMemberExcelData(Server.MapPath(pathString) + fileName, fileName, "0x0100000037FF234084D1F3E7A3BD37B9DBA002712733CB76513FA84E8D7367AF2F2EB771", _Entity.ActId);
                    }
                    else if (files.ToString() == "CooperativesListFile")
                        _Entity.DLSCFileName = fileName;  //COOPERATIVE UNIT LIST IN DLSCRegNo
                    else if (files.ToString() == "WorkPlanShowingEconomicViabilityFile")
                        _Entity.WorkPlanShowingEconomicViabilityFile = fileName;
                    else if (files.ToString() == "GeneralMeetingProceedingFile")
                        _Entity.GeneralMeetingProceedingFile = fileName;
                    else if (files.ToString() == "BalanceSheetFile")
                        _Entity.BalanceSheetFile = fileName;
                    else if (files.ToString() == "ObjectsFile")
                        _Entity.ObjectsFileName = fileName;

                }

                if (Request.Files != null && Request.Files.Count > 0 || _Entity.SocietyIDEnc.Length > 15 || _Entity.SaveStatus == 0)
                {
                    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                    _Entity.SSOID = _LoggedInUser.SSOID;
                    _Entity.UserType = _LoggedInUser.UserType;
                    _Entity.CreatedBy = _Entity.UpdatedBy = _LoggedInUser.UserId;
                    _Entity.KIOSKCODE = _LoggedInUser.KIOSKCODE;
                    _Entity.AADHARNO = _LoggedInUser.AadharNo;
                    if (_Entity.UserType == "K")
                    {
                        _Entity.ServicesId = int.Parse(_LoggedInUser.ServiceId);
                        _Entity.IsActive = 0;
                    }
                    else if (_Entity.UserType == "C" || _Entity.UserType == "D")
                    {
                        _Entity.ServicesId = 5530;//_Entity.IsNewRegistration == true ? 5530 : 5530;
                        _Entity.IsActive = 1;
                    }
                    _Entity.CreatedIP = _Entity.UpdatedIP = COMMON_FUNCTION.GetIpAddress();

                    flag = objSocietyModel.InsertSocietyProfile(_Entity);

                }
                if (flag == "-1" || flag == "0")
                {
                    //System.IO.Directory.Delete(Server.MapPath(pathString));
                }
                else
                {
                    var K = flag.Split('@');
                    if (K.Length > 1)
                    {
                        EmitraController objEC = new EmitraController();
                        //objEC.PayServices(K[1].ToString(), _Entity.SaveStatus.ToString(), "5530");
                    }
                }
            }
            catch (Exception ex)
            {

                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SocietyController", FunctionName = "SaveSocietyProfiling" }, ex);

            }

            ReturnJSON = "{\"data\":\"" + flag.ToString() + "\",\"societydirectory\":\"" + _Entity.RegistrationNo + "\"}";
            return ReturnJSON;

        }

        public string ValidateExcel(ref DataTable Data, string excelPath)
        {
            string result = "";
            try
            {
                string extension = Path.GetExtension(excelPath).ToLower();
                if (extension != ".xlsx" && extension != ".xls")
                {
                    result = "Please upload xlsx formate only";
                }
                else
                if (System.IO.File.Exists(excelPath))
                {
                    Data = COMMON_FUNCTION.ReadExcelData(excelPath, "MemberDetails");
                    string str = "";
                    foreach (DataColumn column in Data.Columns)
                    {
                        str += (str != "" ? "," : "") + column.ColumnName.Replace(" ", "");
                    }

                    string[] ckclm = str.Split(',');
                    bool isAllColomns = true;
                    string colomn = "";

                    string[] excelColumns = new string[] { "Name", "Father_Husband_Name", "Contact_No", "DOB", "Gender", "Category", "POSTNAME", "Occupation", "MembershipNo", "NOOfShareAllotted", "ShareCapitalAmount", "Address", "loaneeType", "MEMBERTYPE" };
                    string[] categories = new string[] { "ST", "SC", "GEN", "OBC" };

                    foreach (string clm in excelColumns)
                    {
                        if (!ckclm.Contains(clm))
                        {
                            isAllColomns = false;
                            colomn += (colomn != "" ? ", " : "") + clm;
                        }
                    }



                    if (!isAllColomns)
                    {
                        result += "Columns ''" + colomn + "'' is missing or incorrect. Please check and upload again.<br/>";
                    }
                    string aadharck = "";
                    for (int i = 0; i < Data.Rows.Count; i++)
                    {
                        foreach (DataColumn column in Data.Columns)
                        {

                            //if (column.ColumnName.ToUpper() == "DOB" && Data.Rows[i][column.ColumnName] != null)
                            //{
                            //    if (!new SocietyModel(new DBLogger()).IsValidDateTime(Data.Rows[i][column.ColumnName].ToString()))
                            //    {
                            //        //result += "Row.No-" + (i + 1) + ", Columns ''" + column.ColumnName + "'' is empty or incorrect. Please fill correct value.<br/>";
                            //        result += "Row.No-" + (i + 1) + ", Columns ''" + column.ColumnName + "'', Please enter Proper DOB '" + Data.Rows[i][column.ColumnName].ToString().Split(' ')[0] + "' in formate (dd/mm/yyyy) .<br/> on line No " + (i + 1).ToString();
                            //    }

                            //}

                            if (Data.Rows[i][column.ColumnName] == null || Data.Rows[i][column.ColumnName].ToString().Replace(" ", "") == "")
                            {
                                if ((Data.Rows[i]["Contact_No"].ToString().Replace(" ", "") == "" || Data.Rows[i]["DOB"].ToString().Replace(" ", "") == "") && Data.Rows.Count > 34)
                                {

                                }
                                else
                                {
                                    result += "Row.No-" + (i + 1) + ", Columns ''" + column.ColumnName + "'' is empty or incorrect. Please fill correct value.<br/>";
                                }
                            }

                            int n; float b;
                            if (column.ColumnName == "NOOfShareAllotted" && Data.Rows[i][column.ColumnName].ToString() != null && Data.Rows[i][column.ColumnName].ToString() != "" && !int.TryParse(Data.Rows[i][column.ColumnName].ToString(), out n))
                            {
                                result += "Row.No-" + (i + 1) + ", Columns ''" + column.ColumnName + "''. Please fill numeric value only.<br/>";
                            }
                            if (column.ColumnName == "ShareCapitalAmount" && Data.Rows[i][column.ColumnName].ToString() != null && Data.Rows[i][column.ColumnName].ToString() != "" && !float.TryParse(Data.Rows[i][column.ColumnName].ToString(), out b))
                            {
                                result += "Row.No-" + (i + 1) + ", Columns ''" + column.ColumnName + "''. Please fill numeric or decimal value only.<br/>";
                            }
                            if (column.ColumnName == "Category" && Data.Rows[i][column.ColumnName] != null)
                            {
                                if (Array.IndexOf(categories, Data.Rows[i][column.ColumnName].ToString().Trim().ToUpper()) < 0)
                                {
                                    result += "Row.No-" + (i + 1) + ", Columns ''" + column.ColumnName + "''. Please fill correct value from [ST,SC,GEN,OBC].<br/>";
                                }

                                Data.Rows[i][column.ColumnName] = Data.Rows[i][column.ColumnName].ToString().Trim().ToUpper() == "ST" ? 1 :
                                Data.Rows[i][column.ColumnName].ToString().Trim().ToUpper() == "SC" ? 2 :
                                Data.Rows[i][column.ColumnName].ToString().Trim().ToUpper() == "GEN" ? 3 :
                                Data.Rows[i][column.ColumnName].ToString().Trim().ToUpper() == "OBC" ? 4 : 0;
                            }

                            //if (column.ColumnName == "AadharNo" && Data.Rows[i][column.ColumnName] != null && Data.Rows[i][column.ColumnName].ToString().Replace(" ", "") != "")
                            //{
                            //    aadharck = Data.Rows[i][column.ColumnName].ToString().Replace(" ", "");
                            //    if (Data.AsEnumerable().Where(row => row.Field<object>("AadharNo") != null && row.Field<dynamic>("AadharNo").ToString().Replace(" ", "") == aadharck).CopyToDataTable().Rows.Count > 1)
                            //    {
                            //        result += "Row.No-" + (i + 1) + ", Columns ''" + column.ColumnName + "=" + aadharck + "' is duplicate'. Please fill correct value.<br/>";
                            //    }
                            //}

                        }
                    }

                    if (result == "")
                        result = "OK";

                }
                else
                {
                    result = "Execl File is not uploaded on server";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
        public string ValidateExcelSportAct(ref DataTable Data, string excelPath)
        {
            string result = "";
            try
            {
                string extension = Path.GetExtension(excelPath).ToLower();
                if (extension != ".xlsx")
                {
                    result = "Please upload xlsx formate only";
                }
                else
                if (System.IO.File.Exists(excelPath))
                {
                    Data = COMMON_FUNCTION.ReadExcelData(excelPath, "MemberDetails");
                    string str = "";
                    foreach (DataColumn column in Data.Columns)
                    {
                        str += (str != "" ? "," : "") + column.ColumnName.Replace(" ", "");
                    }

                    string[] ckclm = str.Split(',');
                    bool isAllColomns = true;
                    string colomn = "";

                    string[] excelColumns = new string[] { "Name", "Father_Husband_Name", "Contact_No", "DOB", "Gender", "Category", "POSTNAME", "Address", "MEMBERTYPE" };
                    string[] categories = new string[] { "ST", "SC", "GEN", "OBC" };

                    foreach (string clm in excelColumns)
                    {
                        if (!ckclm.Contains(clm))
                        {
                            isAllColomns = false;
                            colomn += (colomn != "" ? ", " : "") + clm;
                        }
                    }



                    if (!isAllColomns)
                    {
                        result += "Columns ''" + colomn + "'' is missing or incorrect. Please check and upload again.<br/>";
                    }
                    string aadharck = "";
                    for (int i = 0; i < Data.Rows.Count; i++)
                    {
                        foreach (DataColumn column in Data.Columns)
                        {
                            if (Data.Rows[i][column.ColumnName] == null || Data.Rows[i][column.ColumnName].ToString().Replace(" ", "") == "")
                            {
                                result += "Row.No-" + (i + 1) + ", Columns ''" + column.ColumnName + "'' is empty or incorrect. Please fill correct value.<br/>";
                            }

                            if (column.ColumnName == "Category" && Data.Rows[i][column.ColumnName] != null)
                            {
                                if (Array.IndexOf(categories, Data.Rows[i][column.ColumnName].ToString().Trim().ToUpper()) < 0)
                                {
                                    result += "Row.No-" + (i + 1) + ", Columns ''" + column.ColumnName + "''. Please fill correct value from [ST,SC,GEN,OBC].<br/>";
                                }

                                Data.Rows[i][column.ColumnName] = Data.Rows[i][column.ColumnName].ToString().Trim().ToUpper() == "ST" ? 1 :
                                Data.Rows[i][column.ColumnName].ToString().Trim().ToUpper() == "SC" ? 2 :
                                Data.Rows[i][column.ColumnName].ToString().Trim().ToUpper() == "GEN" ? 3 :
                                Data.Rows[i][column.ColumnName].ToString().Trim().ToUpper() == "OBC" ? 4 : 0;
                            }

                            //if (column.ColumnName == "AadharNo" && Data.Rows[i][column.ColumnName] != null && Data.Rows[i][column.ColumnName].ToString().Replace(" ", "") != "")
                            //{
                            //    aadharck = Data.Rows[i][column.ColumnName].ToString().Replace(" ", "");
                            //    if (Data.AsEnumerable().Where(row => row.Field<object>("AadharNo") != null && row.Field<dynamic>("AadharNo").ToString().Replace(" ", "") == aadharck).CopyToDataTable().Rows.Count > 1)
                            //    {
                            //        result += "Row.No-" + (i + 1) + ", Columns ''" + column.ColumnName + "=" + aadharck + "' is duplicate'. Please fill correct value.<br/>";
                            //    }
                            //}

                        }
                    }

                    if (result == "")
                        result = "OK";

                }
                else
                {
                    result = "Execl File is not uploaded on server";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }




        [HttpPost]
        [ActionName("SaveSocietyAuthorizedPersonnelDetail")]
        public int SaveSocietyAuthorizedPersonnelDetail(SocietyAuthorizedPersonEntity _Entity)
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return objSocietyModel.SocietyAuthorizedPersonnelDetailsInsert(_Entity);
        }

        [HttpPost]
        [ActionName("SaveSocietyRegisteredOfficeDetails")]
        public string SaveSocietyRegisteredOfficeDetail(SocietyRegisteredOfficeDetailsEntity _Entity)
        {
            string Result = "-1";

            string ReturnJSON = "";
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());


            _Entity.SSO_ID = _LoggedInUser.SSOID;
            _Entity.UserType = _LoggedInUser.UserType;
            _Entity.CreatedBy = _Entity.UpdatedBy = _LoggedInUser.UserId;
            _Entity.CreatedIP = _Entity.UpdatedIP = COMMON_FUNCTION.GetIpAddress();

            _Entity.IsSameCEOAddress = true;
            _Entity.DeleteFlag = 1;

            List<SocietyRegisteredOfficeDetailsEntity> LstSocietyRegisteredOfficeDetails = new List<SocietyRegisteredOfficeDetailsEntity>();

            SocietyRegisteredOfficeDetailsEntity OBJnew = new SocietyRegisteredOfficeDetailsEntity();
            OBJnew.SocietyRegOfficeDetailId = _Entity.SocietyRegOfficeDetailId;
            OBJnew.SocietyIdEnc = _Entity.SocietyIdEnc;
            OBJnew.IsRural = _Entity.IsRural;
            OBJnew.HouseBuildingNo = _Entity.HouseBuildingNo;
            OBJnew.LaneStreetName = _Entity.LaneStreetName;
            OBJnew.Locality = _Entity.Locality;
            OBJnew.STATE_ID = _Entity.STATE_ID; ;
            OBJnew.DIVISION_ID = _Entity.DIVISION_ID;
            OBJnew.DISTRICT_ID = _Entity.DISTRICT_ID;
            OBJnew.CityTypes = _Entity.CityTypes;
            OBJnew.CITY_ID = _Entity.CITY_ID;
            OBJnew.BLOCK_ID = _Entity.BLOCK_ID;
            OBJnew.WARD_ID = _Entity.WARD_ID;
            OBJnew.GP_ID = _Entity.GP_ID;
            OBJnew.VILLAGE_ID = _Entity.VILLAGE_ID;
            OBJnew.PinCode = _Entity.PinCode;
            OBJnew.CreatedBy = _Entity.CreatedBy;
            OBJnew.UpdatedBy = _Entity.UpdatedBy;
            OBJnew.CreatedIP = _Entity.CreatedIP;
            OBJnew.UpdatedIP = _Entity.UpdatedIP;
            OBJnew.IsSameCEOAddress = _Entity.IsSameCEOAddress;
            OBJnew.AddressFor = _Entity.AddressFor;
            OBJnew.SSO_ID = _LoggedInUser.SSOID;
            OBJnew.KIOSKCODE = _LoggedInUser.KIOSKCODE;
            OBJnew.EMitraTransactionId = _Entity.EMitraTransactionId;
            _LoggedInUser.ServiceId = "0";
            OBJnew.ServiceId = int.Parse(_LoggedInUser.ServiceId);
            OBJnew.SubmitStatus = _Entity.SubmitStatus;
            OBJnew.UserType = _LoggedInUser.UserType;
            LstSocietyRegisteredOfficeDetails.Add(OBJnew);




            _Entity.IsSameCEOAddress = true;


            //Resut = objSocietyModel.SocietyRegisteredOfficeDetailsInsert(_Entity);//same address then


            if (_Entity.IsSameCEOAddress == true)//if diffrent address
            {
                SocietyRegisteredOfficeDetailsEntity NewSocietyRegisteredOfficeDetailsEntity = new SocietyRegisteredOfficeDetailsEntity();
                NewSocietyRegisteredOfficeDetailsEntity.AddressFor = 9;
                NewSocietyRegisteredOfficeDetailsEntity.SocietyRegOfficeDetailId = _Entity.SocietyRegisrationID_copy;
                NewSocietyRegisteredOfficeDetailsEntity.IsSameCEOAddress = false;
                NewSocietyRegisteredOfficeDetailsEntity.SocietyIdEnc = _Entity.SocietyIdEnc;
                NewSocietyRegisteredOfficeDetailsEntity.HouseBuildingNo = _Entity.HouseBuildingNocopy;
                NewSocietyRegisteredOfficeDetailsEntity.LaneStreetName = _Entity.LaneStreetNamecopy;
                NewSocietyRegisteredOfficeDetailsEntity.Locality = _Entity.Localitycopy;
                NewSocietyRegisteredOfficeDetailsEntity.STATE_ID = _Entity.STATE_IDcopy;
                NewSocietyRegisteredOfficeDetailsEntity.DIVISION_ID = _Entity.DIVISION_ID_copy;
                NewSocietyRegisteredOfficeDetailsEntity.DISTRICT_ID = _Entity.DISTRICT_ID_copy;
                NewSocietyRegisteredOfficeDetailsEntity.IsRural = _Entity.IsRuralcopy;
                NewSocietyRegisteredOfficeDetailsEntity.CITY_ID = _Entity.CITY_IDcopy;
                NewSocietyRegisteredOfficeDetailsEntity.WARD_ID = _Entity.WARD_IDcopy;
                NewSocietyRegisteredOfficeDetailsEntity.GP_ID = _Entity.GP_IDcopy;
                NewSocietyRegisteredOfficeDetailsEntity.VILLAGE_ID = _Entity.VILLAGE_IDcopy;
                NewSocietyRegisteredOfficeDetailsEntity.PinCode = _Entity.PinCodecopy;
                NewSocietyRegisteredOfficeDetailsEntity.CityTypes = _Entity.CityTypes_copy;
                NewSocietyRegisteredOfficeDetailsEntity.BLOCK_ID = _Entity.BLOCK_ID_copy;
                NewSocietyRegisteredOfficeDetailsEntity.DeleteFlag = 2;
                NewSocietyRegisteredOfficeDetailsEntity.SSO_ID = _LoggedInUser.SSOID;
                NewSocietyRegisteredOfficeDetailsEntity.KIOSKCODE = _LoggedInUser.KIOSKCODE;
                NewSocietyRegisteredOfficeDetailsEntity.EMitraTransactionId = _Entity.EMitraTransactionId;
                NewSocietyRegisteredOfficeDetailsEntity.ServiceId = int.Parse(_LoggedInUser.ServiceId);
                NewSocietyRegisteredOfficeDetailsEntity.SubmitStatus = _Entity.SubmitStatus;
                NewSocietyRegisteredOfficeDetailsEntity.UserType = _Entity.UserType;
                LstSocietyRegisteredOfficeDetails.Add(NewSocietyRegisteredOfficeDetailsEntity);

                // int Result = objSocietyModel.SocietyRegisteredOfficeDetailsInsert(NewSocietyRegisteredOfficeDetailsEntity);

            }
            if (_Entity.IsSameCEOAddress == true && _Entity.ActId == 7)//if diffrent address
            {
                SocietyRegisteredOfficeDetailsEntity newobj = new SocietyRegisteredOfficeDetailsEntity();
                newobj.IsSameCEOAddress = true;
                newobj.AddressFor = 11;    //scretary id
                newobj.SocietyIdEnc = _Entity.SocietyIdEnc;
                newobj.HouseBuildingNo = _Entity.HouseBuildingNoSecretary;
                newobj.LaneStreetName = _Entity.LaneStreetNameSecretary;
                newobj.Locality = _Entity.LocalitySecretary;
                newobj.STATE_ID = _Entity.STATE_IDSecretary;
                newobj.DIVISION_ID = _Entity.DIVISION_IDSecretary;
                newobj.DISTRICT_ID = _Entity.DISTRICT_IDSecretary;
                newobj.IsRural = _Entity.IsRuralSecretary;
                newobj.CITY_ID = _Entity.CITY_IDSecretary;
                newobj.WARD_ID = _Entity.WARD_IDSecretary;
                newobj.GP_ID = _Entity.GP_IDSecretary;
                newobj.VILLAGE_ID = _Entity.VILLAGE_IDSecretary;
                newobj.PinCode = _Entity.PinCodeSecretary;
                newobj.CityTypes = _Entity.CityTypesSecretary;
                newobj.BLOCK_ID = _Entity.BLOCK_IDSecretary;

                newobj.DeleteFlag = 3;

                newobj.SSO_ID = _LoggedInUser.SSOID;
                newobj.KIOSKCODE = _LoggedInUser.KIOSKCODE;
                newobj.EMitraTransactionId = _Entity.EMitraTransactionId;
                newobj.ServiceId = int.Parse(_LoggedInUser.ServiceId);
                newobj.SubmitStatus = _Entity.SubmitStatus;
                newobj.UserType = _Entity.UserType;
                LstSocietyRegisteredOfficeDetails.Add(newobj);

                // int Result = objSocietyModel.SocietyRegisteredOfficeDetailsInsert(newobj);

            }
            if (_Entity.IsSameCEOAddress == true && _Entity.ActId == 7)//if diffrent address
            {
                SocietyRegisteredOfficeDetailsEntity newobjfortreasurer = new SocietyRegisteredOfficeDetailsEntity();
                newobjfortreasurer.IsSameCEOAddress = false;
                newobjfortreasurer.AddressFor = 12;    //treasurer id
                newobjfortreasurer.SocietyIdEnc = _Entity.SocietyIdEnc;
                newobjfortreasurer.HouseBuildingNo = _Entity.HouseBuildingNoTreasurer;
                newobjfortreasurer.LaneStreetName = _Entity.LaneStreetNameTreasurer;
                newobjfortreasurer.Locality = _Entity.LocalityTreasurer;
                newobjfortreasurer.STATE_ID = _Entity.STATE_IDTreasurer;
                newobjfortreasurer.DIVISION_ID = _Entity.DIVISION_IDTreasurer;
                newobjfortreasurer.DISTRICT_ID = _Entity.DISTRICT_IDTreasurer;
                newobjfortreasurer.IsRural = _Entity.IsRuralTreasurer;
                newobjfortreasurer.CITY_ID = _Entity.CITY_IDTreasurer;
                newobjfortreasurer.WARD_ID = _Entity.WARD_IDTreasurer;
                newobjfortreasurer.GP_ID = _Entity.GP_IDTreasurer;
                newobjfortreasurer.VILLAGE_ID = _Entity.VILLAGE_IDTreasurer;
                newobjfortreasurer.PinCode = _Entity.PinCodeTreasurer;
                newobjfortreasurer.CityTypes = _Entity.CityTypesTreasurer;
                newobjfortreasurer.BLOCK_ID = _Entity.BLOCK_IDTreasurer;
                newobjfortreasurer.UserType = _Entity.UserType;
                newobjfortreasurer.DeleteFlag = 4;

                newobjfortreasurer.SSO_ID = _LoggedInUser.SSOID;
                newobjfortreasurer.KIOSKCODE = _LoggedInUser.KIOSKCODE;
                newobjfortreasurer.EMitraTransactionId = _Entity.EMitraTransactionId;
                newobjfortreasurer.ServiceId = int.Parse(_LoggedInUser.ServiceId);

                newobjfortreasurer.SubmitStatus = _Entity.SubmitStatus;

                LstSocietyRegisteredOfficeDetails.Add(newobjfortreasurer);
                // int Result = objSocietyModel.SocietyRegisteredOfficeDetailsInsert(newobjfortreasurer);


            }



            Result = objSocietyModel.SocietyRegisteredOfficeDetailsInsert(LstSocietyRegisteredOfficeDetails);
            //_Entity.SSO_ID = _LoggedInUser.SSOID;
            //_Entity.UserType = _LoggedInUser.UserType;
            //_Entity.CreatedBy = _Entity.UpdatedBy = _LoggedInUser.UserId;
            //_Entity.CreatedIP = _Entity.UpdatedIP = COMMON_FUNCTION.GetIpAddress();
            //_Entity.IsSameCEOAddress = false;
            //int Resut1 = objSocietyModel.SocietyRegisteredOfficeDetailsInsert(_Entity);

            ReturnJSON = "{\"data\":\"" + Result.ToString() + "\",\"usertype\":\"" + _Entity.CreatedBy.ToString() + "\"}";
            return ReturnJSON;
        }

        [HttpPost]
        [ActionName("SocietyBearerInsert")]
        public int InsertSocietyBearer(SocietyBearerEntity _Model)
        {
            string fileName = string.Empty;
            if (Request.Files != null)
            {
                try
                {
                    foreach (string files in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[files];
                        fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                        if (files.ToString() == "Photo")
                        {
                            _Model.Photo = fileName;
                            file.SaveAs(Path.Combine(Server.MapPath("~/Content/SocietyProfiling"), fileName));
                        }
                    }

                }
                catch { }
            }

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return objSocietyModel.InsertSocietyBearer(_Model);
        }


        [HttpPost]
        [ActionName("InsertSocityByExcel")]
        public string InsertSocityByExcel(SocietyRegistrationEntity _Model)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            string fileName = string.Empty;
            _Model.ExcelUrl = "";
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    _Model.ExcelUrl = fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("~/Content/SocietyExcel"), fileName));
                }
                catch { }
            }
            _Model.CreatedBy = _Model.UpdatedBy = int.Parse(_LoggedInUser.UserId.ToString());
            _Model.CreatedIP = COMMON_FUNCTION.GetIpAddress();
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return objSocietyModel.InsertSocityByExcel(_Model).ToString();
            //return CommonMessageFactory.Message(int.Parse(objSocietyModel.InsertSocityByExcel(_Model).ToString()), (Request.Cookies[SessionIdentity.Culture]).Value);
        }

        public ActionResult SocietyProfile(SocietyRegistrationEntity obj)
        {
            List<SocietyRegistrationEntity> _SocietyRegistrationData = new List<SocietyRegistrationEntity>();
            CitizenModel objMenuActionModel = new CitizenModel(new FileLogger());

            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
            {
                if (Request.Cookies["SSOROLE"].Value == "CITIZEN")
                {
                    obj.UserType = "C";
                }
                else
                {
                    obj.UserType = "D";
                }
            }
            //SocietyRegistrationEntity objSocietyRegistrationEntity = new SocietyRegistrationEntity();
            //objSocietyRegistrationEntity.SSOID = "";
            //DataTable DT = objMenuActionModel.SocietyList(objSocietyRegistrationEntity);          


            obj.SSOID = _LoggedInUser.SSOID;
            DataTable DT = new DataTable();

            List<SocietyListBySSOID_Result> list = objMenuActionModel.SocietyList(obj, "", _LoggedInUser.UserType);

            //foreach (DataRow Dr in DT.Rows)
            //{
            //    SocietyRegistrationEntity objEntity = new SocietyRegistrationEntity();
            //    objEntity.SocietyName = Dr["SocietyName"].ToString();
            //    objEntity.SocietyIDEnc = Dr["SocietyIdENC"].ToString();
            //    objEntity.SocietyID = int.Parse(Dr["SocietyID"].ToString());
            //    objEntity.RegistrationNo = Dr["RegistrationNo"].ToString();
            //    objEntity.CreationDateStr = Dr["RegistrationDate"].ToString();
            //    objEntity.ActNameEn = Dr["ActNameEn"].ToString();
            //    objEntity.ActNameHi = Dr["ActNameHi"].ToString();
            //    objEntity.IsActive = int.Parse(Dr["IsActive"].ToString());
            //    //objEntity.Photo = Dr["Photo"].ToString();
            //    objEntity.FilledProfileLevel = int.Parse(Dr["IsAdministrativeDetails"].ToString());
            //    objEntity.TypeNameEn = Dr["TypeameEng"].ToString();
            //    objEntity.TypeNameHi = Dr["TypeNameHin"].ToString();
            //    objEntity.Status = Dr["Status"].ToString();
            //    objEntity.ActId = int.Parse(Dr["ActId"].ToString());
            //    _SocietyRegistrationData.Add(objEntity);
            //}
            return View(list);
        }


        [HttpPost]
        [ActionName("SaveSocietyAdministrativeDetails")]
        public string InsertSocietyAdministrativeDetails(SocietyAdministrativeEntity SocietyRegistrationData)
        {
            string flag = "-1";
            List<SocietyAdministrativeEntity> obj = new List<SocietyAdministrativeEntity>();
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<AdministrativeData> objData = new List<AdministrativeData>();

            try
            {
                objData = Request.Form["DataArray"].ToString().Equals("[]") ? new List<AdministrativeData>() : (List<AdministrativeData>)s.Deserialize(Request.Form["DataArray"].ToString(), typeof(List<AdministrativeData>));
            }
            catch (Exception ex)
            {
                return CommonMessageFactory.Response("Provided input data is not in correct format.");
            }

            if (!ModelState.IsValid)
                return JsonModelError.Get(ModelState.Values);

            string fileName = string.Empty;
            if (objData.Count > 0)
            {
                if (Request.Files != null && Request.Files.Count > 0 || SocietyRegistrationData.SubmitStatus == 0 || SocietyRegistrationData.BoardType == "Administrative")
                {
                    try
                    {
                        int i = 0;
                        bool Flag = false;
                        int ForOther = 0;

                        string pathString = "";
                        if (SocietyRegistrationData.SocietyDirectory == null || SocietyRegistrationData.SocietyDirectory == "" || SocietyRegistrationData.SocietyDirectory == "undefined")
                        {
                            pathString = "~/Content/NewRegistration";
                        }
                        else
                        {
                            pathString = "~/Content/SocietyProfiling/" + Regex.Replace(SocietyRegistrationData.SocietyDirectory.ToString(), "[^a-zA-Z0-9_]+", "");
                            if (!System.IO.Directory.Exists(Server.MapPath(pathString)))
                            {
                                System.IO.Directory.CreateDirectory(Server.MapPath(pathString));
                            }
                        }



                        foreach (string files in Request.Files)
                        {
                            HttpPostedFileBase file = Request.Files[files];
                            fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                            if (System.IO.Directory.Exists(Server.MapPath(pathString)))
                            {
                                file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));
                            }

                            if (files.ToString() == "CEOSocietyFile")
                            {
                                objData[0].Photo = fileName;
                                i = 0;
                            }
                            else if (files.ToString() == "ChairmanFile")
                            {
                                objData[1].Photo = fileName;
                                i = 1;
                            }
                            else if (files.ToString() == "AppointedFile")
                            {
                                objData[2].Photo = fileName;
                                i = 2;
                            }

                            if (files.ToString().Contains("OtherAdministrativeFile") && Flag == false)
                            {
                                for (; objData[i].UserType == "4" || objData[i].UserType == "1" || objData[i].UserType == "2" || objData[i].UserType == "3";)
                                {
                                    i++;
                                }
                                Flag = true;
                            }
                            //}

                            if (SocietyRegistrationData.ActId == 7)
                            {
                                if (files.ToString() == "OtherAdministrativeFile21")
                                    objData[i].Photo = fileName;
                                else if (files.ToString() == "OtherAppointedFile")
                                    objData[i].Photo = fileName;
                            }
                            else
                            {
                                i = i == 0 ? objData.Count - 1 : i;

                                if (files.ToString() == "OtherAdministrativeFile11")
                                    objData[i].Photo = fileName;
                                else if (files.ToString() == "OtherAppointedFile")
                                    objData[i].Photo = fileName;
                            }

                            i++;

                        }


                    }
                    catch (Exception ex)
                    {



                    }
                }


                flag = objSocietyModel.InsertSocietyAdministrativeDetails(objData, SocietyRegistrationData.ClassA, SocietyRegistrationData.ClassB, SocietyRegistrationData.ClassC, SocietyRegistrationData.EMitraTransactionId, _LoggedInUser.UserType.ToString());

                return flag;
            }
            else
                return flag = "-1";
            // return CommonMessageFactory.Response(CommonMessageFactory.Message(flag.Length > 10 ? 1 : 0, (Request.Cookies[SessionIdentity.Culture]).Value));
        }


        [HttpPost]
        [ActionName("SavePermanentSocietyData")]
        public string SavePermanentSocietyData(string SocietyIDEnc)
        {
            string flag = "-1";
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            flag = objSocietyModel.SavePermanentSocietyData(SocietyIDEnc);
            return flag;
        }


        [HttpPost]
        public string SocietyAssignOfficerSave(string SocietyIDEnc)
        {
            string flag = "-1";
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            flag = objSocietyModel.SocietyAssignOfficerSave(SocietyIDEnc);
            return flag;
        }

        [HttpPost]
        [ActionName("SAVEBranch")]
        public string SAVEBranch(int DistrictId, int BlockId, string BranchNameHi, string BranchName_En, int CityId)
        {
            string flag = "-1";
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            flag = objSocietyModel.SAVEBranch(DistrictId, BlockId, BranchNameHi, BranchName_En, CityId);
            return flag;
        }


        [HttpPost]
        [ActionName("SaveSocietyAdministrativeDetailsAmendment")]
        public string InsertSocietyAdministrativeDetailAmendments(SocietyAdministrativeEntity SocietyRegistrationData)
        {
            string flag = "-1";
            List<SocietyAdministrativeEntity> obj = new List<SocietyAdministrativeEntity>();
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<AdministrativeData> objData = new List<AdministrativeData>();

            try
            {
                objData = Request.Form["DataArray"].ToString().Equals("[]") ? new List<AdministrativeData>() : (List<AdministrativeData>)s.Deserialize(Request.Form["DataArray"].ToString(), typeof(List<AdministrativeData>));
            }
            catch (Exception ex)
            {
                return CommonMessageFactory.Response("Provided input data is not in correct format.");
            }

            if (!ModelState.IsValid)
                return JsonModelError.Get(ModelState.Values);

            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    int i = 0;
                    bool Flag = false;
                    int ForOther = 0;

                    string pathString = "";
                    if (SocietyRegistrationData.SocietyDirectory == "undefined")
                    {
                        pathString = "~/Content/NewRegistration";
                    }
                    else
                    {
                        pathString = "~/Content/SocietyProfiling/" + Regex.Replace(SocietyRegistrationData.SocietyDirectory.ToString(), "[^a-zA-Z0-9_]+", "");
                        if (!System.IO.Directory.Exists(pathString))
                        {
                            System.IO.Directory.CreateDirectory(Server.MapPath(pathString));
                        }
                    }



                    foreach (string files in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[files];
                        fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                        if (System.IO.Directory.Exists(Server.MapPath(pathString)))
                        {
                            file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));
                        }

                        if (files.ToString() == "CEOSocietyFile")
                        {
                            objData[0].Photo = fileName;
                            i = 0;
                        }
                        else if (files.ToString() == "ChairmanFile")
                        {
                            objData[1].Photo = fileName;
                            i = 1;
                        }
                        else if (files.ToString() == "AppointedFile")
                        {
                            objData[2].Photo = fileName;
                            i = 2;
                        }

                        else if (files.ToString() == "AdministrativeFile1")
                        {
                            objData[3].Photo = fileName;
                            i = 3;
                        }
                        else if (files.ToString() == "AdministrativeFile2")
                        {
                            objData[4].Photo = fileName;
                            i = 4;
                        }
                        else if (files.ToString() == "AdministrativeFile3")
                        {
                            objData[5].Photo = fileName;
                            i = 5;
                        }
                        else if (files.ToString() == "AdministrativeFile4")
                        {
                            objData[6].Photo = fileName;
                            i = 6;
                        }
                        else if (files.ToString() == "AdministrativeFile5")
                        {
                            objData[7].Photo = fileName;
                            i = 7;
                        }
                        else if (files.ToString() == "AdministrativeFile6")
                        {
                            objData[8].Photo = fileName;
                            i = 8;
                        }
                        else if (files.ToString() == "AdministrativeFile7")
                        {
                            objData[9].Photo = fileName;
                            i = 9;
                        }
                        else if (files.ToString() == "AdministrativeFile8")
                        {
                            objData[10].Photo = fileName;
                            i = 10;
                        }
                        else if (files.ToString() == "AdministrativeFile9")
                        {
                            objData[11].Photo = fileName;
                            i = 11;
                        }
                        else if (files.ToString() == "AdministrativeFile10")
                        {
                            objData[12].Photo = fileName;
                            i = 12;
                        }
                        else if (files.ToString() == "AdministrativeFile11")
                        {
                            objData[13].Photo = fileName;
                            i = 13;
                        }
                        else if (files.ToString() == "AdministrativeFile12")
                        {
                            objData[14].Photo = fileName;
                            i = 14;
                        }
                        else if (files.ToString() == "AdministrativeFile13")
                        {
                            objData[15].Photo = fileName;
                            i = 15;
                        }
                        else if (files.ToString() == "AdministrativeFile14")
                        {
                            objData[16].Photo = fileName;
                            i = 16;
                        }

                        else if (files.ToString() == "AdministrativeFile15")
                        {
                            objData[17].Photo = fileName;
                            i = 17;
                        }
                        else if (files.ToString() == "AdministrativeFile16")
                        {
                            objData[18].Photo = fileName;
                            i = 18;
                        }
                        else if (files.ToString() == "AdministrativeFile17")
                        {
                            objData[19].Photo = fileName;
                            i = 19;
                        }
                        else if (files.ToString() == "AdministrativeFile18")
                        {
                            objData[20].Photo = fileName;
                            i = 20;
                        }

                        //if (SocietyRegistrationData.ActId == 7)
                        //{
                        //    if (files.ToString().Contains("OtherAdministrativeFile") && Flag == false)
                        //    {
                        //        for (; objData[i].UserType != "4";)
                        //        {
                        //            i++;
                        //        }
                        //        Flag = true;
                        //    }
                        //}
                        //else
                        //{
                        if (files.ToString().Contains("OtherAdministrativeFile") && Flag == false)
                        {
                            for (; objData[i].UserType == "4" || objData[i].UserType == "1" || objData[i].UserType == "2" || objData[i].UserType == "3";)
                            {
                                i++;
                            }
                            Flag = true;
                        }
                        //}

                        if (SocietyRegistrationData.ActId == 7)
                        {
                            if (files.ToString() == "OtherAdministrativeFile21")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile22")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile23")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile24")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile25")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile26")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile27")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile28")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile29")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile30")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile31")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile32")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile33")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile34")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile35")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAppointedFile")
                                objData[i].Photo = fileName;
                        }
                        else
                        {
                            i = i == 0 ? objData.Count - 1 : i;

                            if (files.ToString() == "OtherAdministrativeFile11")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile12")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile13")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile14")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile15")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile16")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile17")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile18")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile19")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile20")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile21")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile22")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile23")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile24")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAdministrativeFile25")
                                objData[i].Photo = fileName;
                            else if (files.ToString() == "OtherAppointedFile")
                                objData[i].Photo = fileName;
                        }

                        i++;

                    }


                }
                catch (Exception ex)
                {



                }
            }

            flag = objSocietyModel.InsertSocietyAdministrativeDetailsAmendment(objData, SocietyRegistrationData.ClassA, SocietyRegistrationData.ClassB, SocietyRegistrationData.ClassC, SocietyRegistrationData.EMitraTransactionId);

            return flag;
            // return CommonMessageFactory.Response(CommonMessageFactory.Message(flag.Length > 10 ? 1 : 0, (Request.Cookies[SessionIdentity.Culture]).Value));
        }

        [HttpPost]
        [ActionName("SaveSocietyAuditReportDetails")]
        public int SaveSocietyAuditReportDetails(SocietyAuditReportEntity _Entity)
        {
            List<UploadFileDetailSocietyInspectorAssignmentEntity> lst = new List<UploadFileDetailSocietyInspectorAssignmentEntity>();
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int ReturnVal = 0;
            try
            {
                if (Request.Files != null && Request.Files.Count > 0) //&& Request.Files[0].ToString() == "AuditReport"
                {
                    string fileName = string.Empty;
                    string pathString = "~/Content/AuditReport/";
                    string FileList = string.Empty;
                    string ResolutionPDF = "";
                    string AuditorAssignFilePath = "";
                    foreach (string files in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[files];
                        fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                        file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));
                        UploadFileDetailSocietyInspectorAssignmentEntity obj = new UploadFileDetailSocietyInspectorAssignmentEntity();

                        string[] stringSplitter = files.Split('β');
                        if (stringSplitter.Length >= 3)
                        {
                            obj.SocietyInspectorAssignId = _Entity.SocietyAuditorAssigmentsId;
                            obj.UploadTypeId = stringSplitter[2].ToLower() == "na" ? Convert.ToInt32(stringSplitter[1]) : -1;
                            obj.UploadFileName = fileName;
                            obj.UploadExtraFileTitle = stringSplitter[2];
                            obj.IsActive = true;
                            obj.IsDelete = false;
                            obj.CreateBy = Convert.ToInt32(_loggedInUSerDetails.UserId);
                            obj.DocumentType = stringSplitter[0];
                            lst.Add(obj);
                        }
                        else
                        {
                            _Entity.AuditReport = fileName;
                        }
                    }
                }


                _Entity.CreatedBy = int.Parse(_LoggedInUser.UserId.ToString());
                if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
                {
                    if (Request.Cookies["SSOROLE"].Value == "CITIZEN")
                    {
                        _Entity.UserType = "C";
                    }
                    else
                    {
                        _Entity.UserType = "D";
                    }
                }

                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());


                ReturnVal = _Entity.SocietyAuditReportId;
                if (ReturnVal == 0)
                {
                    SocietyAuditReportEntity _Model = new SocietyAuditReportEntity();
                    _Model.SocietyIdEnc = _Entity.SocietyIdEnc;
                    _Model.IsApproved = _Entity.IsApproved;
                    _Model.IsSocietyWorking = _Entity.IsSocietyWorking;
                    _Model.FinancialYear = _Entity.FinancialYear;
                    _Model.AuditReport = _Entity.AuditReport;
                    _Model.CreatedBy = _Entity.CreatedBy;
                    _Model.UserType = _Entity.UserType;
                    _Model.ApprovedBy = _Entity.ApprovedBy;
                    ReturnVal = objSocietyModel.InsertSocietyAuditReportDetails(_Model);
                    _Entity.SocietyAuditReportId = ReturnVal;
                }

                if (_Entity.IsPermanentSave)
                {
                    objSocietyModel.SaveAuditorAssigmentsDate(_Entity.SocietyAuditReportId, _Entity.AuditStartDate, _Entity.AuditEndDate, _Entity);
                    objSocietyModel.PermanentsaveSocietyAuditorAssigments(_Entity.SocietyAuditorAssigmentsId);
                }
                if (lst.Count > 0 && ReturnVal > 0)
                {
                    objSocietyModel.AuditorAssignmentFilesInsert(lst);
                }
                return ReturnVal;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyController", FunctionName = "SaveSocietyAuditReportDetails(SocietyAuditReportEntity _Entity)" }, ex);
                return -1;
            }
            finally
            {

            }

        }



        //Umesh work for insert log in member details
        [HttpPost]
        [ActionName("Insertlogformemberdetails")]
        public string insertlogformemberdetails(SocietymemberdataEntity _Entity)
        {
            List<UploadFileDetailSocietyInspectorAssignmentEntity> lst = new List<UploadFileDetailSocietyInspectorAssignmentEntity>();
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            try
            {
                if ((_Entity.SocietyIdEnc != null && _Entity.SocietyIdEnc != "") || (_Entity.BRN != null && _Entity.BRN != ""))
                {
                    if (Request.Files != null && Request.Files.Count > 0) //&& Request.Files[0].ToString() == "AuditReport"
                    {
                        string fileName = string.Empty;

                        string pathString = "../Content/SocietyMember/";
                        string pathString1 = "../Content/NewRegistration/";
                        string FileList = string.Empty;
                        foreach (string files in Request.Files)
                        {
                            HttpPostedFileBase file = Request.Files[files];
                            fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                            if (files == "Adharfile")
                            {
                                _Entity.Adharfile = pathString1 + fileName;
                                ViewBag.AdhrFile = _Entity.Adharfile;
                            }
                            if (files == "photo")
                            {
                                _Entity.Photo = pathString1 + fileName;
                                ViewBag.Photo = _Entity.Photo;
                            }
                            file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));
                            file.SaveAs(Path.Combine(Server.MapPath(pathString1), fileName));
                        }
                    }

                    _Entity.CreatedBy = int.Parse(_LoggedInUser.UserId.ToString());
                    SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                    var data = objSocietyModel.InsertSocietyAuditReportDetails1(_Entity);
                    return COMMON_FUNCTION.DataTableToJSONString(data);

                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyController", FunctionName = "SaveSocietyAuditReportDetails(SocietyAuditReportEntity _Entity)" }, ex);
                return "-1";
            }
            finally
            {

            }

        }
        //End

        [HttpPost]
        [ActionName("GetSocietyDetailsBySocietyId")]
        public string GetSocietyDetailsBySocietyId(string SocietyId, int EMitraTransactionId = 0)
        {
            if (string.IsNullOrEmpty(SocietyId))
                return "{}";

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            DataTable DT = new DataTable();
            if (EMitraTransactionId == 0)
            {
                DT = objSocietyModel.GetSocietyDetailsBySocietyId(SocietyId);
            }
            else
            {
                DT = objSocietyModel.GetSocietyDetailsBySocietyIdFormTempTbl(SocietyId, EMitraTransactionId);
            }
            //DT = objSocietyModel.GetSocietyDetailsBySocietyId(SocietyId);
            foreach (DataRow dr in DT.Rows) // search whole table
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(dr["Photo"].ToString()))
                    {
                        string pathString = "~/Content/SocietyProfiling/" + dr["SocietyDirectory"].ToString();
                        if (Directory.Exists(pathString))
                        {
                            if (!System.IO.File.Exists(Server.MapPath(dr["Photo"].ToString())))
                            {
                                if (System.IO.File.Exists(Server.MapPath(pathString + "/" + dr["Aadh"].ToString() + ".png")))
                                {
                                    //Response.Write(Server.MapPath(pathString + "/" + dr["Aadh"].ToString() + ".png")+"\n");
                                    //Response.Write(Server.MapPath(dr["Photo"].ToString()));

                                    System.IO.File.Move(Server.MapPath(pathString + "/" + dr["Aadh"].ToString() + ".png"), Server.MapPath(dr["Photo"].ToString()));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) { }



                try
                {
                    if (dr["Aadh"] != null && string.IsNullOrWhiteSpace(dr["AadharNo"].ToString()) && dr["Aadh"].ToString().Length == 12)
                    {
                        dr["AadharNo"] = COMMON_FUNCTION.Encrypt(dr["Aadh"].ToString());
                    }
                }
                catch (Exception ex) { }





                try
                {
                    var lbl_Registraionno = dr["RegistrationNo"].ToString();
                    if (lbl_Registraionno.Replace(" ", "").Replace("/", "") != lbl_Registraionno.Replace(" ", "").Replace("/", "").Replace("-", ""))
                    {
                        //System.IO.File.Move(, Server.MapPath("/Content/SocietyProfiling/" + lbl_Registraionno.Replace(" ", "").Replace("/", "").Replace("-", "") + "/4B15AD7AF60C4130AEAA57BCA1ACFA75.pdf"));
                        string SourcePath = Server.MapPath("/Content/SocietyProfiling/" + lbl_Registraionno.Replace(" ", "").Replace("/", ""));
                        string DestinationPath = Server.MapPath("/Content/SocietyProfiling/" + lbl_Registraionno.Replace(" ", "").Replace("/", "").Replace("-", ""));

                        if (!System.IO.Directory.Exists(Server.MapPath("/Content/SocietyProfiling/" + lbl_Registraionno.Replace(" ", "").Replace("/", "").Replace("-", ""))))
                        {
                            System.IO.Directory.CreateDirectory(Server.MapPath("/Content/SocietyProfiling/" + lbl_Registraionno.Replace(" ", "").Replace("/", "").Replace("-", "")));
                        }
                        foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
                        {
                            System.IO.File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);
                        }

                    }
                }
                catch (Exception) { }

                try
                {

                    if (dr["AadharNo"].ToString().Length > 12)
                    {
                        dr["AadharNo"] = COMMON_FUNCTION.Decrypt(dr["AadharNo"].ToString());
                        dr["AadharNo2"] = dr["AadharNo"].ToString();
                        var AadharNo = dr["AadharNo"].ToString();

                        var firstDigits = AadharNo.Substring(0, 8);
                        var lastDigits = AadharNo.Substring(AadharNo.Length - 4, 4);
                        var requiredMask = new String('X', AadharNo.Length - lastDigits.Length);
                        var maskedString = string.Concat(requiredMask, lastDigits);
                        var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0");
                        dr["AadharNo"] = maskedString;
                    }
                    if (dr["Photo"].ToString().Length > 12)
                    {
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(dr["Photo"].ToString())))
                        {

                        }
                        else
                        {
                            var photo = dr["Photo"].ToString().Split('/');
                            var SocietyDirectory = dr["SocietyDirectory"].ToString();
                            string PhName = photo[(photo.Length - 1)].ToString();
                            if (dr["SocietyDirectory"].ToString() != null && dr["SocietyDirectory"].ToString() != "" && (dr["SocietyDirectory"].ToString()).Length > 1)
                            {
                                string pathString = "~/Content/SocietyProfiling/" + dr["SocietyDirectory"].ToString();
                                // if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                                // {
                                CitizenController obj = new CitizenController();
                                obj.UpdatePhotosAndDoc(int.Parse(dr["SocietyID"].ToString()));

                                // }

                            }

                            //if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("../Content/NewRegistration/" + PhName)))
                            //{
                            //    dr["Photo"] = "../Content/NewRegistration/" + PhName;
                            //    if (dr["SocietyDirectory"].ToString() != null && dr["SocietyDirectory"].ToString() != "" && (dr["SocietyDirectory"].ToString()).Length > 1)
                            //    {
                            //        string SourceFilePath = "~/Content/NewRegistration/" + PhName;
                            //        string pathString = "~/Content/SocietyProfiling/" + dr["SocietyDirectory"].ToString() + "/" + PhName;
                            //        if (System.IO.File.Exists(Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(Server.MapPath(pathString)))
                            //        {
                            //            System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(pathString));
                            //            dr["Photo"] = "../Content/SocietyProfiling/" + SocietyDirectory + "/" + PhName;
                            //        }

                            //    }
                            //}
                            else if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("../Content/SocietyProfiling/" + SocietyDirectory + "/" + PhName)) && (SocietyDirectory != null && SocietyDirectory != "" && SocietyDirectory.Length > 1))
                            {
                                dr["Photo"] = "../Content/SocietyProfiling/" + SocietyDirectory + "/" + PhName;
                            }
                            //else if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("../Content/SocietyProfiling/" + PhName)))
                            //{
                            //    string SourceFilePath = "~/Content/SocietyProfiling/" + PhName;
                            //    string pathString = "~/Content/SocietyProfiling/" + dr["SocietyDirectory"].ToString() + "/" + PhName;
                            //    if (dr["SocietyDirectory"].ToString() != null && dr["SocietyDirectory"].ToString() != "" && (dr["SocietyDirectory"].ToString()).Length > 1)
                            //    {
                            //        if (System.IO.File.Exists(Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(Server.MapPath(pathString)))
                            //        {
                            //            System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(pathString));
                            //            dr["Photo"] = "../Content/SocietyProfiling/" + SocietyDirectory + "/" + PhName;
                            //        }                               

                            //    }
                            //}
                            else
                                dr["Photo"] = "../Content/Images/Balnk.png";
                        }

                    }
                }
                catch (Exception ex)
                {

                }
            }
            //return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        }

        [HttpPost]
        public string GetSocietyDetailsBySocietyId_v1(string SocietyId, int EMitraTransactionId = 0)
        {
            if (string.IsNullOrEmpty(SocietyId))
                return "{}";

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            DataTable DT = new DataTable();
            if (EMitraTransactionId == 0)
            {
                DT = objSocietyModel.GetSocietyDetailsBySocietyId(SocietyId);
            }
            else
            {
                DT = objSocietyModel.GetSocietyDetailsBySocietyIdFormTempTbl(SocietyId, EMitraTransactionId);
            }
            //DT = objSocietyModel.GetSocietyDetailsBySocietyId(SocietyId);
            foreach (DataRow dr in DT.Rows) // search whole table
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(dr["Photo"].ToString()))
                    {
                        string pathString = "~/Content/SocietyProfiling/" + dr["SocietyDirectory"].ToString();
                        if (!System.IO.File.Exists(Server.MapPath(dr["Photo"].ToString())))
                        {
                            if (System.IO.File.Exists(Server.MapPath(pathString + "/" + dr["Aadh"].ToString() + ".png")))
                            {
                                //Response.Write(Server.MapPath(pathString + "/" + dr["Aadh"].ToString() + ".png")+"\n");
                                //Response.Write(Server.MapPath(dr["Photo"].ToString()));

                                System.IO.File.Move(Server.MapPath(pathString + "/" + dr["Aadh"].ToString() + ".png"), Server.MapPath(dr["Photo"].ToString()));
                            }
                        }
                    }
                }
                catch (Exception ex) { }


                try
                {
                    try
                    {
                        if (dr["Aadh"] != null && string.IsNullOrWhiteSpace(dr["AadharNo"].ToString()) && dr["Aadh"].ToString().Length == 12)
                        {
                            dr["AadharNo"] = COMMON_FUNCTION.Encrypt(dr["Aadh"].ToString());
                        }
                    }
                    catch (Exception ex) { }


                    if (dr["AadharNo"].ToString().Length > 12)
                    {
                        dr["AadharNo"] = COMMON_FUNCTION.Decrypt(dr["AadharNo"].ToString());
                        dr["AadharNo2"] = dr["AadharNo"].ToString();
                        var AadharNo = dr["AadharNo"].ToString();

                        var firstDigits = AadharNo.Substring(0, 8);
                        var lastDigits = AadharNo.Substring(AadharNo.Length - 4, 4);
                        var requiredMask = new String('X', AadharNo.Length - lastDigits.Length);
                        var maskedString = string.Concat(requiredMask, lastDigits);
                        var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0");
                        dr["AadharNo"] = maskedString;
                    }
                    if (dr["Photo"].ToString().Length > 12)
                    {
                        try
                        {
                            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(dr["Photo"].ToString())))
                            {

                            }
                            else
                            {
                                var photo = dr["Photo"].ToString().Split('/');
                                var SocietyDirectory = dr["SocietyDirectory"].ToString();
                                string PhName = photo[(photo.Length - 1)].ToString();
                                if (dr["SocietyDirectory"].ToString() != null && dr["SocietyDirectory"].ToString() != "" && (dr["SocietyDirectory"].ToString()).Length > 1)
                                {
                                    string pathString = "~/Content/SocietyProfiling/" + dr["SocietyDirectory"].ToString();

                                    CitizenController obj = new CitizenController();
                                    obj.UpdatePhotosAndDoc(int.Parse(dr["SocietyID"].ToString()));

                                }

                                else if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("../Content/SocietyProfiling/" + SocietyDirectory + "/" + PhName)) && (SocietyDirectory != null && SocietyDirectory != "" && SocietyDirectory.Length > 1))
                                {
                                    dr["Photo"] = "../Content/SocietyProfiling/" + SocietyDirectory + "/" + PhName;
                                }

                                else
                                    dr["Photo"] = "../Content/Images/Balnk.png";
                            }
                        }
                        catch { }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            //return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        }


        [HttpPost]
        public string SocietyDetailsByAmendmentSocietyId(string SocietyId, int EMitraTransactionId = 0)
        {
            if (string.IsNullOrEmpty(SocietyId))
                return "{}";

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            DataTable DT = new DataTable();
            if (EMitraTransactionId == 0)
            {
                DT = objSocietyModel.SocietyDetailsByAmendmentSocietyId(SocietyId, _LoggedInUser.SSOID);
            }
            else
            {
                DT = objSocietyModel.GetSocietyDetailsBySocietyIdFormTempTbl(SocietyId, EMitraTransactionId);
            }
            //DT = objSocietyModel.GetSocietyDetailsBySocietyId(SocietyId);
            foreach (DataRow dr in DT.Rows) // search whole table
            {
                try
                {
                    try
                    {
                        if (dr["Aadh"] != null && string.IsNullOrWhiteSpace(dr["AadharNo"].ToString()) && dr["Aadh"].ToString().Length == 12)
                        {
                            dr["AadharNo"] = COMMON_FUNCTION.Encrypt(dr["Aadh"].ToString());
                        }
                    }
                    catch (Exception ex) { }


                    if (dr["AadharNo"].ToString().Length > 12)
                    {
                        dr["AadharNo"] = COMMON_FUNCTION.Decrypt(dr["AadharNo"].ToString());
                        dr["AadharNo2"] = dr["AadharNo"].ToString();
                        var AadharNo = dr["AadharNo"].ToString();

                        var firstDigits = AadharNo.Substring(0, 8);
                        var lastDigits = AadharNo.Substring(AadharNo.Length - 4, 4);
                        var requiredMask = new String('X', AadharNo.Length - lastDigits.Length);
                        var maskedString = string.Concat(requiredMask, lastDigits);
                        var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0");
                        dr["AadharNo"] = maskedString;
                    }
                    if (dr["Photo"].ToString().Length > 12)
                    {
                        try
                        {
                            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(dr["Photo"].ToString())))
                            {

                            }
                            else
                            {
                                var photo = dr["Photo"].ToString().Split('/');
                                var SocietyDirectory = dr["SocietyDirectory"].ToString();
                                string PhName = photo[(photo.Length - 1)].ToString();
                                if (dr["SocietyDirectory"].ToString() != null && dr["SocietyDirectory"].ToString() != "" && (dr["SocietyDirectory"].ToString()).Length > 1)
                                {
                                    string pathString = "~/Content/SocietyProfiling/" + dr["SocietyDirectory"].ToString();

                                    CitizenController obj = new CitizenController();
                                    obj.UpdatePhotosAndDoc(int.Parse(dr["SocietyID"].ToString()));

                                }

                                else if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("../Content/SocietyProfiling/" + SocietyDirectory + "/" + PhName)) && (SocietyDirectory != null && SocietyDirectory != "" && SocietyDirectory.Length > 1))
                                {
                                    dr["Photo"] = "../Content/SocietyProfiling/" + SocietyDirectory + "/" + PhName;
                                }

                                else
                                    dr["Photo"] = "../Content/Images/Balnk.png";
                            }
                        }
                        catch { }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            //return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        }

        [HttpPost]
        public string GetSocietyDetailsByAmendmentId(string SocietyId, string AmendmentId, int EMitraTransactionId = 0)
        {
            if (string.IsNullOrEmpty(SocietyId))
                return "{}";

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            DataTable DT = new DataTable();
            if (EMitraTransactionId == 0)
            {
                DT = objSocietyModel.GetSocietyDetailsByAmendmentId(SocietyId, AmendmentId);
            }
            else
            {
                DT = objSocietyModel.GetSocietyDetailsBySocietyIdFormTempTbl(SocietyId, EMitraTransactionId);
            }
            //DT = objSocietyModel.GetSocietyDetailsBySocietyId(SocietyId);
            foreach (DataRow dr in DT.Rows) // search whole table
            {
                try
                {
                    try
                    {
                        if (dr["Aadh"] != null && string.IsNullOrWhiteSpace(dr["AadharNo"].ToString()) && dr["Aadh"].ToString().Length == 12)
                        {
                            dr["AadharNo"] = COMMON_FUNCTION.Encrypt(dr["Aadh"].ToString());
                        }
                    }
                    catch (Exception ex) { }


                    if (dr["AadharNo"].ToString().Length > 12)
                    {
                        dr["AadharNo"] = COMMON_FUNCTION.Decrypt(dr["AadharNo"].ToString());
                        dr["AadharNo2"] = dr["AadharNo"].ToString();
                        var AadharNo = dr["AadharNo"].ToString();

                        var firstDigits = AadharNo.Substring(0, 8);
                        var lastDigits = AadharNo.Substring(AadharNo.Length - 4, 4);
                        var requiredMask = new String('X', AadharNo.Length - lastDigits.Length);
                        var maskedString = string.Concat(requiredMask, lastDigits);
                        var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0");
                        dr["AadharNo"] = maskedString;
                    }
                    if (dr["Photo"].ToString().Length > 12)
                    {
                        try
                        {
                            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(dr["Photo"].ToString())))
                            {

                            }
                            else
                            {
                                var photo = dr["Photo"].ToString().Split('/');
                                var SocietyDirectory = dr["SocietyDirectory"].ToString();
                                string PhName = photo[(photo.Length - 1)].ToString();
                                if (dr["SocietyDirectory"].ToString() != null && dr["SocietyDirectory"].ToString() != "" && (dr["SocietyDirectory"].ToString()).Length > 1)
                                {
                                    string pathString = "~/Content/SocietyProfiling/" + dr["SocietyDirectory"].ToString();

                                    CitizenController obj = new CitizenController();
                                    obj.UpdatePhotosAndDoc(int.Parse(dr["SocietyID"].ToString()));

                                }

                                else if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("../Content/SocietyProfiling/" + SocietyDirectory + "/" + PhName)) && (SocietyDirectory != null && SocietyDirectory != "" && SocietyDirectory.Length > 1))
                                {
                                    dr["Photo"] = "../Content/SocietyProfiling/" + SocietyDirectory + "/" + PhName;
                                }

                                else
                                    dr["Photo"] = "../Content/Images/Balnk.png";
                            }
                        }
                        catch { }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            //return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        }


        [HttpPost]
        public string GetSocietyDetailsBySocietyIdForAmendmet(string SocietyId, int EMitraTransactionId = 0)
        {
            if (string.IsNullOrEmpty(SocietyId))
                return "{}";

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            DataTable DT = new DataTable();

            DT = objSocietyModel.SocietyDetailsBySocietyIdForAmendmet(SocietyId);
            foreach (DataRow dr in DT.Rows) // search whole table
            {
                if (dr["AadharNo"].ToString().Length > 12)
                {
                    dr["AadharNo"] = COMMON_FUNCTION.Decrypt(dr["AadharNo"].ToString());
                    dr["AadharNo2"] = dr["AadharNo"].ToString();
                    var AadharNo = dr["AadharNo"].ToString();

                    var firstDigits = AadharNo.Substring(0, 8);
                    var lastDigits = AadharNo.Substring(AadharNo.Length - 4, 4);
                    var requiredMask = new String('X', AadharNo.Length - lastDigits.Length);
                    var maskedString = string.Concat(requiredMask, lastDigits);
                    var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0");
                    dr["AadharNo"] = maskedString;
                }
            }
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }


        [HttpPost]
        public string GetWorkscopeBySocietyId_v1(string SocietyId, int EMitraTransactionId = 0)
        {
            if (string.IsNullOrEmpty(SocietyId))
                return "{}";

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            DataTable DT = new DataTable();
            if (EMitraTransactionId == 0)
            {
                DT = objSocietyModel.GetWorkscopeBySocietyId(SocietyId);
            }
            else
            {
                DT = objSocietyModel.GetWorkscopeBySocietyIdFormTempTbl(SocietyId, EMitraTransactionId);
            }

            return COMMON_FUNCTION.DataTableToJSONString(DT);
            //return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);

        }

        [HttpPost]
        [ActionName("GetWorkscopeBySocietyId")]
        public string GetWorkscopeBySocietyId(string SocietyId, int EMitraTransactionId = 0)
        {
            if (string.IsNullOrEmpty(SocietyId))
                return "{}";

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            DataTable DT = new DataTable();
            if (EMitraTransactionId == 0)
            {
                DT = objSocietyModel.GetWorkscopeBySocietyId(SocietyId);
            }
            else
            {
                DT = objSocietyModel.GetWorkscopeBySocietyIdFormTempTbl(SocietyId, EMitraTransactionId);
            }

            return COMMON_FUNCTION.DataTableToJSONString(DT);
            //return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);

        }

        [HttpPost]
        [ActionName("GetSocietyAuditDetailsBySocietyId")]
        public string GetSocietyAuditDetailsBySocietyId(string SocietyId)
        {
            int _SocietyId = 0;
            if (!int.TryParse(SocietyId, out _SocietyId))
                return "{}";

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objSocietyModel.SocietyAuditDetailsBySocietyId(_SocietyId));
        }


        [HttpPost]
        [ActionName("AuditorAssignmentInsertOld")]
        public int AuditorAssignmentInsertOld(SocietyAuditorAssigmentEntity _Model)
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            _Model.CreatedBy = Convert.ToInt32(_LoggedInUser.UserId);

            int FirmType = -1;

            if (_Model.IsEmpanelledCA && _Model.IsFirm == 0)
                FirmType = 0;
            else if (_Model.IsEmpanelledCA && _Model.IsFirm == 1)
                FirmType = 1;
            else if (!_Model.IsEmpanelledCA)
                FirmType = 2;

            _Model.IsFirm = FirmType;
            int Result = 0;
            if (_Model.Id == 0)
                Result = objSocietyModel.AuditorAssignmentInsertOldEntry(_Model);
            else
                Result = _Model.Id;

            if (_Model.IsPermanentSave && _Model.HaveAuditReport && Result > 0)
                objSocietyModel.PermanentsaveSocietyAuditorAssigments(Result);

            if (Result == -1 || Result == -2 || Result == 0 || Result == -3)
            {
                return Result;
            }
            else
            {
                string fileName = string.Empty;
                string pathString = "~/Content/AuditReport/";
                string FileList = string.Empty;
                List<UploadFileDetailSocietyInspectorAssignmentEntity> lst = new List<UploadFileDetailSocietyInspectorAssignmentEntity>();
                string ResolutionPDF = "";
                string AuditorAssignFilePath = "";
                foreach (string files in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[files];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));
                    UploadFileDetailSocietyInspectorAssignmentEntity obj = new UploadFileDetailSocietyInspectorAssignmentEntity();

                    string[] stringSplitter = files.Split('β');
                    if (stringSplitter.Length >= 3)
                    {
                        if (stringSplitter[1] == "-2")
                        {
                            ResolutionPDF = "../Content/AuditReport/" + fileName;
                        }
                        else if (stringSplitter[1] == "-3")
                        {
                            AuditorAssignFilePath = "../Content/AuditReport/" + fileName;
                        }
                        else
                        {
                            obj.SocietyInspectorAssignId = Result;
                            obj.UploadTypeId = stringSplitter[2].ToLower() == "na" ? Convert.ToInt32(stringSplitter[1]) : -1;
                            obj.UploadFileName = fileName;
                            obj.UploadExtraFileTitle = stringSplitter[2];
                            obj.IsActive = true;
                            obj.IsDelete = false;
                            obj.CreateBy = Convert.ToInt32(_loggedInUSerDetails.UserId);
                            obj.DocumentType = stringSplitter[0];
                            lst.Add(obj);
                        }
                    }
                }//_Model.Id != 0 &&
                if (AuditorAssignFilePath != "" && ResolutionPDF != "" && _Model.Id == 0)
                {
                    objSocietyModel.UpdateAuditorAssignmentFiles(AuditorAssignFilePath, ResolutionPDF, Result);
                }
                if (lst.Count > 0)
                {
                    Result = objSocietyModel.AuditorAssignmentFilesInsert(lst);
                }


                return Result;
            }

        }


        [HttpPost]
        [ActionName("AuditorAssignmentInsertNew")]
        public int AuditorAssignmentInsertNew(SocietyAuditorAssigmentEntity _Model)
        {
            string fileName = string.Empty;
            string pathString = "~/Content/AuditReport/";
            string FileList = string.Empty;
            string ResolutionPDF = "";

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            _Model.CreatedBy = Convert.ToInt32(_LoggedInUser.UserId);

            int FirmType = -1;

            if (_Model.IsEmpanelledCA && _Model.IsFirm == 0)
                FirmType = 0;
            else if (_Model.IsEmpanelledCA && _Model.IsFirm == 1)
                FirmType = 1;
            else if (!_Model.IsEmpanelledCA)
                FirmType = 2;

            _Model.IsFirm = FirmType;

            foreach (string files in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[files];
                fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));
                UploadFileDetailSocietyInspectorAssignmentEntity obj = new UploadFileDetailSocietyInspectorAssignmentEntity();

                string[] stringSplitter = files.Split('β');
                if (stringSplitter.Length >= 3)
                {
                    if (stringSplitter[1] == "-2")
                    {
                        _Model.ResolutionPDF = "../Content/AuditReport/" + fileName;
                    }
                    else if (stringSplitter[1] == "-3")
                    {
                        _Model.AuditorAssignFilePath = "../Content/AuditReport/" + fileName;
                    }

                }
            }
            int Result = 0;
            if (_Model.Id == 0)
                Result = objSocietyModel.AuditorAssignmentInsert(_Model);
            else
                Result = _Model.Id;

            return Result;
        }
        [HttpGet]
        public ActionResult Inspactor()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Inspactor, (Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        public ActionResult AuditFilesUplodByDepartment()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                //ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Inspactor, (Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.Title = "Upload Audit Reports";
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.UserType = _LoggedInUser.UserType;
            }
            return View();
        }

        [HttpPost]
        [ActionName("AuditorAssignmentListForcitizen")]
        public string AuditorAssignmentListForcitizen()
        {
            string WhereClause = " where 1=1 and SAA.IsActive=1 ";
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
            {
                WhereClause += " And Societies.SSOID='" + Request.Cookies["SSO_ID"].Value + "' ";
            }
            else
                WhereClause += " And 0=1 ";
            return COMMON_FUNCTION.DataTableToJSONString(objSocietyModel.GetAuditorAssignmentList(WhereClause));
        }



        [HttpPost]
        public JsonResult GetAuditorAssignmentList(DataPagingModel Details, string ID = "", string IsEmpanelled = "", string FYearId = "")

        {
            string WhereClause = " where 1=1 and SAA.IsActive=1 ";
            if (FYearId != "")
            {
                WhereClause += " AND SAA.FinancialYear= " + FYearId;
            }
            if (_LoggedInUser.LevelId == 1)
            {
                //WhereClause += " AND (Societies.SocietyTypeId=9 OR Isnull(SocietyCategory.CategoryId,0) IN (8,9,28,60))";
            }
            if (_LoggedInUser.LevelId == 2)
            {
                WhereClause += " AND ( Societies.SocietyTypeId=8 OR Isnull(SocietyCategory.CategoryId,0) in (42 ,61,3) ) AND ISNULL(SocietyCategory.CategoryId,0) NOT IN (8,9,28)  And D.DIVISION_ID=" + _LoggedInUser.DivisionId;
            }
            else if (_LoggedInUser.LevelId == 3)
            {
                WhereClause += " AND Societies.SocietyTypeId NOT IN (8,9) and Isnull(SocietyCategory.CategoryId,0) NOT IN (42 ,61,60,3) And SocietyRegOfficeDetails.DISTRICT_ID=" + _LoggedInUser.DistrictId;
            }


            if (ID == "1")
            {
                WhereClause += " And RequestStatus>0";
                if (IsEmpanelled == "" || IsEmpanelled == "True")
                {
                    WhereClause += " And IsEmpanelledCA=1 ";
                }
                else if (IsEmpanelled == "False")
                {
                    WhereClause += " And IsEmpanelledCA=0 ";
                }
            }
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());

            string SSO_ID = ""; ;

            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
            {
                if (Request.Cookies["SSOROLE"].Value == "CITIZEN")
                {
                    SSO_ID = Request.Cookies["SSO_ID"].Value;
                    WhereClause += " And SSOID='" + Request.Cookies["SSO_ID"].Value + "' ";
                }
                else
                {

                }
            }

            int DivisionId = 0;
            int DistrictId = 0;

            if (_LoggedInUser.LevelId == 3)
            {
                DistrictId = _LoggedInUser.DistrictId;
            }
            if (_LoggedInUser.LevelId == 2)
            {
                DivisionId = _LoggedInUser.DivisionId;
            }

            var data = (new Society()).GetAuditorAssignmentList(ref Details, DivisionId, DistrictId, SSO_ID, _LoggedInUser.UserRoleDeptMappingId, _LoggedInUser.LevelId);
            return Json(new { DataList = data, DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("AuditorAssignmentList")]
        public string AuditorAssignmentList(DataPagingModel Details, string ID = "", string IsEmpanelled = "", string FYearId = "")
        {
            string WhereClause = " where 1=1 and SAA.IsActive=1 ";
            if (FYearId != "")
            {
                WhereClause += " AND SAA.FinancialYear= " + FYearId;
            }
            if (_LoggedInUser.LevelId == 1)
            {
                //WhereClause += " AND (Societies.SocietyTypeId=9 OR Isnull(SocietyCategory.CategoryId,0) IN (8,9,28,60))";
            }
            if (_LoggedInUser.LevelId == 2)
            {
                WhereClause += " AND ( Societies.SocietyTypeId=8 OR Isnull(SocietyCategory.CategoryId,0) in (42 ,61,3) ) AND ISNULL(SocietyCategory.CategoryId,0) NOT IN (8,9,28)  And D.DIVISION_ID=" + _LoggedInUser.DivisionId;
            }
            else if (_LoggedInUser.LevelId == 3)
            {
                WhereClause += " AND Societies.SocietyTypeId NOT IN (8,9) and Isnull(SocietyCategory.CategoryId,0) NOT IN (42 ,61,60,3) And SocietyRegOfficeDetails.DISTRICT_ID=" + _LoggedInUser.DistrictId;
            }


            if (ID == "1")
            {
                WhereClause += " And RequestStatus>0";
                if (IsEmpanelled == "" || IsEmpanelled == "True")
                {
                    WhereClause += " And IsEmpanelledCA=1 ";
                }
                else if (IsEmpanelled == "False")
                {
                    WhereClause += " And IsEmpanelledCA=0 ";
                }
            }
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());

            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
            {
                if (Request.Cookies["SSOROLE"].Value == "CITIZEN")
                {
                    WhereClause += " And SSOID='" + Request.Cookies["SSO_ID"].Value + "' ";
                }
                else
                {

                }
            }


            //var data = (new Society()).GetAuditorAssignmentList(ref Details,WhereClause);
            // return Json(new { DataList = data, DataPaging = Details }, JsonRequestBehavior.AllowGet);
            return COMMON_FUNCTION.DataTableToJSONString(objSocietyModel.GetAuditorAssignmentList(WhereClause));
        }
        [HttpGet]
        [ActionName("GetInspactor")]
        public string GetAllInspactor(string WhereClause = "")
        {
            WhereClause = " Where 1=1 ";
            if (_loggedInUSerDetails.UserType == "C") // C => Citizen
            {
                WhereClause += " And Societies.SSOID=" + _loggedInUSerDetails.SSOID;
            }

            else if (_loggedInUSerDetails.RoleId == 10043) // INSPECTOR
            {
                WhereClause += " And SAA.AuditorId=" + _loggedInUSerDetails.UserId;
            }

            SocietyModel objSocietyModel = new SocietyModel();

            return COMMON_FUNCTION.DataTableToJSONString(objSocietyModel.GetAllInspactor(WhereClause));
        }
        [HttpGet]
        [ActionName("Getallworkscopedata")]
        public string GetAllWorkscope(int SocietyId)
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());


            return COMMON_FUNCTION.DataTableToJSONString(objSocietyModel.GetworkscopedataList((SocietyId)));
        }
        [HttpGet]
        public ActionResult AuditorAssignmentByDepartment()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.LevelId = _LoggedInUser.LevelId;
                ViewBag.DivisionId = _LoggedInUser.DivisionId;
                ViewBag.DistrictId = _LoggedInUser.DistrictId;
                ViewBag.UserRoleDeptMappingId = _LoggedInUser.UserRoleDeptMappingId;
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.AuditorAssignmentByDepartment, (Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpGet]
        public ActionResult AuditorAssignmentByDepartment1()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.LevelId = _LoggedInUser.LevelId;
                ViewBag.DivisionId = _LoggedInUser.DivisionId;
                ViewBag.DistrictId = _LoggedInUser.DistrictId;
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.AuditorAssignmentByDepartment, (Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [ActionName("GetAuditorAssignmentByDepartment")]
        public JsonResult GetAllAuditorAssignmentByDepartment(string WhereClause = "", string FYearId = "", string CategoryId = "0", int DistrictId = 0, int DivisionId = 0, string Resion = "")
        {
            int serachType = WhereClause == "undefined" || string.IsNullOrWhiteSpace(WhereClause) ? 0 : Convert.ToInt32(WhereClause);
            if (CategoryId == "undefined")
                CategoryId = "0";

            int IsForEmpanelledAgency = 0;
            if (WhereClause == "1")
            {
                WhereClause = " and (SocietyAuditorAssigments.IsEmpanelledCA = 1 or SocietyAuditorAssigments.IsEmpanelledCA is null) and RequestStatus = 0 and FinancialYear > 47  and ReqActionDate is null ";
                IsForEmpanelledAgency = 1;
            }
            else if (WhereClause == "0")
            {
                WhereClause = " and (SocietyAuditorAssigments.IsEmpanelledCA != 1 or SocietyAuditorAssigments.IsEmpanelledCA is null) and (RequestStatus = 0 or SocietyAuditorAssigments.AuditorId=0) and FinancialYear > 47  ";
                IsForEmpanelledAgency = 0;
            }
            else if (WhereClause == "2")
            {
                WhereClause = " and (SocietyAuditorAssigments.IsEmpanelledCA != 1 or SocietyAuditorAssigments.IsEmpanelledCA is null OR SocietyAuditorAssigments.RequestStatus=2)   ";
                IsForEmpanelledAgency = 2;
            }
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            int Division_District_Id = 0;
            if (_LoggedInUser.LevelId == 1)
            {
                WhereClause += " AND ( Societies.SocietyTypeId=9 OR Isnull(SocietyCategory.CategoryId,0) IN (8,9,28,60) ) ";
            }
            else if (_LoggedInUser.LevelId == 2)
            {
                Division_District_Id = _LoggedInUser.DivisionId;
                if (CategoryId == "0")
                    WhereClause += " AND ( Societies.SocietyTypeId=8 OR IsNULL(SocietyCategory.CategoryId,0) in (42 ,61) ) AND ISNULL(SocietyCategory.CategoryId,0) NOT IN (8,9,28)  ";
            }
            else if (_LoggedInUser.LevelId == 3)
            {
                Division_District_Id = _LoggedInUser.DistrictId;
                WhereClause += " AND Societies.SocietyTypeId NOT IN (8,9) and ISNULL(SocietyCategory.CategoryId,0) not in (42 ,61,60) ";
            }


            if (IsForEmpanelledAgency == 1)
            {
                return Json(new { DataList = (new Society()).SocietyListBydepartment_1(WhereClause, IsForEmpanelledAgency, _LoggedInUser.LevelId, Division_District_Id, Convert.ToInt32(FYearId.Replace("undefined", "0")), int.Parse(CategoryId), _LoggedInUser.UserRoleDeptMappingId, DistrictId, DivisionId, Resion) }, JsonRequestBehavior.AllowGet);

            }
            else if (IsForEmpanelledAgency == 2)
            {
                //return Json(new { DataList = (new Society()).SocietyListBydepartment_1(WhereClause, IsForEmpanelledAgency, _LoggedInUser.LevelId, Division_District_Id, Convert.ToInt32(FYearId.Replace("undefined", "0")), int.Parse(CategoryId), _LoggedInUser.UserRoleDeptMappingId, DistrictId, DivisionId, Resion) }, JsonRequestBehavior.AllowGet);

                return Json(new { DataList = (new Society()).SocietyListBydepartment_2_v2(WhereClause, IsForEmpanelledAgency, _LoggedInUser.LevelId, Division_District_Id, Convert.ToInt32(FYearId.Replace("undefined", "0")), int.Parse(CategoryId), _LoggedInUser.UserRoleDeptMappingId, DistrictId, DivisionId, Resion) }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new { DataList = (new Society()).SocietyListBydepartment_0(WhereClause, IsForEmpanelledAgency, _LoggedInUser.LevelId, Division_District_Id, Convert.ToInt32(FYearId.Replace("undefined", "0")), int.Parse(CategoryId), _LoggedInUser.UserRoleDeptMappingId, DistrictId, DivisionId, Resion) }, JsonRequestBehavior.AllowGet);
            }

        }



        [HttpPost]
        [ActionName("GetSocietyListForAuditorAssignment")]
        public JsonResult GetSocietyListForAuditorAssignment(DataPagingModel Details)
        {
            int Division_District_Id = 0;

            if (_LoggedInUser.LevelId == 2)
            {
                Division_District_Id = _LoggedInUser.DivisionId;
            }
            else if (_LoggedInUser.LevelId == 3)
            {
                Division_District_Id = _LoggedInUser.DistrictId;
            }
            //var data = (new Society()).SocietyListBydepartment_2(ref Details, _LoggedInUser.LevelId, _LoggedInUser.UserRoleDeptMappingId, Division_District_Id);
            //return Json(new { DataList = data, DataPaging = Details }, JsonRequestBehavior.AllowGet);


            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            DataTable dt = objSocietyModel.SocietyListBydepartment_2(ref Details, _LoggedInUser.LevelId, _LoggedInUser.UserRoleDeptMappingId, Division_District_Id);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(dt), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }




        [HttpGet]
        [ActionName("GetInspactorName")]
        public string GetInspactorName(string DistId, string OfficeID = "0")
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());

            return COMMON_FUNCTION.DataTableToJSONString(objSocietyModel.GetInspactorName(DistId, OfficeID));
        }

        [HttpPost]
        [ActionName("GetInspactorForRAO")]
        public string GetInspactorForRAO(string DivisionId = "0", string Office = "0")
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objSocietyModel.GetInspactorForRAO(_LoggedInUser.DistrictId, DivisionId, Office));
        }




        [HttpPost]
        [ActionName("saveAuditAssignbydeprt")]
        public string saveAuditAssignbydeprt(string AuditerId = "", string AuditBy = "", string Ids = "", string Status = "", string IsFirm = "", string Tran = "", string AuditTillDate = "")
        {
            string ResultIntStr = "Error";
            string OrderNo = "";
            AuditorController objAC = new AuditorController();
            AuditOrderEntity objAE = new AuditOrderEntity();

            try
            {
                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                DataTable Dt = null;
                var IdsArr = Ids.Split(',');

                string DistName = "";
                string DistNameHi = "";

                string saveIDs = "";
                if (_LoggedInUser.LevelId == 3)
                {
                    DistName = _LoggedInUser.DISTRICT_ENG;
                    DistNameHi = _LoggedInUser.DISTRICT_MANGAL;
                }
                else if (_LoggedInUser.LevelId == 2)
                {
                    DistName = _LoggedInUser.DIVISION_ENG;
                    DistNameHi = _LoggedInUser.DIVISION_MANGAL;
                }
                else if (_LoggedInUser.LevelId == 1)
                {
                    DistName = "Rajasthan";
                }

                #region 

                objAE.FileName = "";
                objAE.SocietyCount = 1;
                objAE.InActiveFlag = 0;
                OrderNo = objAC.SaveAuditOrder(objAE);
                objAE.OrderNo = OrderNo;
                #endregion

                string OfficerName = "";
                string CurrentFYear = "";
                string SocietyName = "";

                string fontSize = "font-size: 13px;";



                Dt = objSocietyModel.GetAuditorAndSocietyDetailByIds(Ids, AuditerId);



                StringBuilder sb = new StringBuilder();
                sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:10px'>");
                sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px; '>");
                sb.Append("<tr><td colspan=2 style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                sb.Append("<img style='width:100px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=2 style='text-align:center;" + fontSize + "'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");

                if (_LoggedInUser.LevelId == 3)
                    sb.Append("<tr><td colspan=2 style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय- विशेष लेखा परीक्षक सहकारी समितियॉं, DistrictNameHi") + "</label></b></td></tr>");
                else if (_LoggedInUser.LevelId == 2)
                    sb.Append("<tr><td colspan=2 style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय- रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड , DistrictNameHi") + "</label></b></td></tr>");
                else if (_LoggedInUser.LevelId == 1)
                    sb.Append("<tr><td colspan=2 style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय- रजिस्ट्रार, सहकारी समितियॉं राजस्थान ,जयपुर") + "</label></b></td></tr>");

                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                sb.Append("<tr><td align='left' style='margin-top:15px;'> क्रमांक फा. " + OrderNo + " </td><td align='right'> दिनांक " + DateTime.Now.ToString("dd/MM/yyyy") + "</td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><u><b> ऑडिट आवंटन आदेश </b></u></td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><b> (रा.स.सो.अधि.धारा 54(2) व नियम 73(4) के तहत) </b></td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                sb.Append("<tr><td align='justify'  colspan=2 style='margin-top:15px;'> <p>&nbsp;&nbsp;&nbsp; राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(4) के अनुसार निम्नलिखित सहकारी समितियों द्वारा विभागीय निरीक्षक से वर्ष FinYear की लेखा परीक्षा करवाने की अभ्यर्थना प्रस्तुत की है | </p></td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");

                if (_LoggedInUser.LevelId == 3)
                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, विशेष लेखा परीक्षक सहकारी समितियॉ DistrictNameHi , राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(4) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                else if (_LoggedInUser.LevelId == 2)
                {
                    if (AuditerId.ToString().Contains(","))
                    {
                        sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(4) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए SocietyName की वर्ष FinYear की लेखा परीक्षा हेतु निम्न लिखित लेखा निरीक्षकों की नियुक्ति करता / करती हूँ | </p></td></tr>");
                    }
                    else
                    {
                        sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(4) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                    }
                }
                else if (_LoggedInUser.LevelId == 1)
                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, संयुक्त मुख्य अंकेक्षक ( जनरल ) सहकारी समितियॉं, राजस्थान जयपुर, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(4) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");

                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                if (AuditerId.ToString().Contains(","))
                {
                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; उपरोक्त समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी |</p></td></tr>");
                }
                else
                {
                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; निम्न समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी |</p></td></tr>");
                }
                sb.Append("<tr><td align='center'  colspan=2 >");
                sb.Append("<table style='width: 100%; margin-top:20px;' border='1' cellspacing='0' cellpadding='5'>");
                if (AuditerId.ToString().Contains(","))
                {
                    sb.Append("<tr><td align='center'style='width:70px;'> क्र. स. </td><td align='center'> लेखा निरीक्षक </td><td align='center'> कार्यालय </td></tr>");
                }
                else
                {
                    sb.Append("<tr><td align='center'style='width:70px;'> क्र. स. </td><td align='center'> सहकारी समिति  </td><td align='center'> स्थान </td></tr>");/*<td align='center'>Officer/Firm</td>*/
                }
                int i1 = 0;
                foreach (DataRow dr in Dt.Rows)
                {
                    i1++;
                    sb.Append("<tr><td  align='left'  > " + i1.ToString() + "</td>");
                    if (AuditerId.ToString().Contains(","))
                    {
                        sb.Append("<td  align='left'  > " + dr["OfficerNameEn"].ToString() + " </td>");
                        sb.Append("<td  align='left'  > " + dr["OfficeNameEn"].ToString() + " </td></tr>");
                    }
                    else
                    {
                        sb.Append("<td  align='left'  > " + dr["SocietyName"].ToString() + " (" + dr["RegistrationNo"].ToString() + ") </td>");
                        sb.Append("<td  align='left'  > " + (dr["SocietyPlaceEn"].ToString() != "" ? (dr["SocietyPlaceEn"].ToString() + " , ") : " ") + dr["DISTRICT_ENG"].ToString() + "</td></tr>");
                    }
                    OfficerName = dr["OfficerNameEn"].ToString() != "" ? dr["OfficerNameEn"].ToString() : dr["FirmName"].ToString();
                    CurrentFYear = dr["CurrentFYear"].ToString();
                    saveIDs = saveIDs + dr["Id"].ToString() + ",";

                    SocietyName = dr["SocietyName"].ToString().ToUpper();


                }
                sb.Append("</table></td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                sb.Append("</table ></div>");


                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                string fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + ".pdf";
                string textVal = sb.ToString().Replace("<br>", "<br/>").Replace("DistrictNameHi", DistNameHi).Replace("SocietyName", SocietyName).Replace("FinYear", CurrentFYear).Replace("InspectorName", OfficerName).Replace("RegitrarName", _LoggedInUser.OfficerNameEn);
                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(textVal));

                string FilePath = "../Content/AuditReport/" + fileName;
                if (COMMON_FUNCTION.GeneratePDF(sb1, Server.MapPath(FilePath)))
                {
                    ResultMessage objRM = new ResultMessage();
                    objRM = COMMON_FUNCTION.EsinghPDF(Tran, Server.MapPath(FilePath), DistName);

                    if (objRM.data == "Success")
                    {
                        if (saveIDs.Length > 0)
                            saveIDs = saveIDs.Substring(0, saveIDs.Length - 1);

                        ResultIntStr = objSocietyModel.saveAuditAssignbydeprt(AuditerId, AuditBy, saveIDs, Status, IsFirm, FilePath, _LoggedInUser.UserRoleDeptMappingId, OrderNo).ToString();
                        if (ResultIntStr != "-1")
                            ResultIntStr = "Success";
                        else
                        {
                            objAE.InActiveFlag = 1;
                            objAC.SaveAuditOrder(objAE);
                        }
                    }
                }

            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Society", FunctionName = " saveAuditAssignbydeprt(string AuditerId = \"\", string AuditBy = \"\", string Ids = \"\", string Status = \"\", string IsFirm = \"\", string Tran = \"\", string AuditTillDate = \"\")" }, ex);

            }
            if (ResultIntStr == "Error")
            {
                objAE.InActiveFlag = 1;
                objAC.SaveAuditOrder(objAE);
            }
            return (ResultIntStr);


        }
        [HttpPost]
        [ActionName("saveAuditAssignbydeprtNotRequest")]
        public string saveAuditAssignbydeprtNotRequest(string AuditerId = "", string AuditBy = "", string Ids = "", string Status = "", string IsFirm = "", string Tran = "", int FinancialYearId = 0, string FinancialYearName = "", string AuditTillDate = "", string IsForced = "False", string AuditByDepartmentorCA = "")
        {
            string ResultIntStr = "Error";


            AuditorController objAC = new AuditorController();
            AuditOrderEntity objAE = new AuditOrderEntity();
            try
            {
                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());

                var IdsArr = Ids.Split(',');

                string DistName = "";
                string DistNameHi = "";
                string Location = "";
                string fontSize = "font-size: 13px;";
                string saveIDs = "";
                DataTable Dt = null;

                if (_LoggedInUser.LevelId == 3)
                {
                    DistName = _LoggedInUser.DISTRICT_ENG;
                    DistNameHi = _LoggedInUser.DISTRICT_MANGAL;
                    Location = "जिले";
                }
                else if (_LoggedInUser.LevelId == 2)
                {
                    DistName = _LoggedInUser.DIVISION_ENG;
                    DistNameHi = _LoggedInUser.DIVISION_MANGAL;
                    Location = "खण्ड";
                }
                else if (_LoggedInUser.LevelId == 1)
                {
                    Location = "राज्य";
                }


                string OfficerName = "";
                string CurrentFYear = "";
                string SocietyName = "";
                string OrderNo = "";
                //if (AuditerId.ToString().Contains(","))
                //{
                //Dt = objSocietyModel.GetAuditorsForMultipalAssigment(AuditerId);
                //}
                //else
                int CurrentFYId = 0;
                int PrevFYId = 0;

                // {
                int AuditByAD = (AuditByDepartmentorCA == "true" ? 1 : 2);

                //if(Dt.Rows.Count>0)
                //{
                //    CurrentFYId = Convert.ToInt32(Dt.Rows[0]["CurrentFYId"].ToString());
                //}
                //  }

                DataTable FYDdata = objSocietyModel.GetCurrentAssessmentYear();

                if (FYDdata.Rows.Count > 0)
                {
                    CurrentFYId = Convert.ToInt32(FYDdata.Rows[0]["CurrentFY"].ToString());
                    PrevFYId = Convert.ToInt32(FYDdata.Rows[0]["prevFY"].ToString());
                }
                #region 
                Dt = objSocietyModel.GetAuditorAndSocietyDetailByIds(Ids, AuditerId, AuditByAD, FinancialYearId);
                objAE.FileName = "";
                // objAE.SocietyCount = Dt.Rows.Count;

                objAE.SocietyCount = Dt.AsEnumerable().GroupBy(row => row["SocietyName"]).Count();

                objAE.InActiveFlag = 0;
                OrderNo = objAC.SaveAuditOrder(objAE);
                objAE.OrderNo = OrderNo;
                #endregion
                if (OrderNo != "ERROR")
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:10px'>");
                    sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px; '>");
                    sb.Append("<tr><td colspan=2 style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                    sb.Append("<img style='width:100px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td colspan=2 style='text-align:center;" + fontSize + "'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                    if (_LoggedInUser.LevelId == 3)
                        sb.Append("<tr><td colspan=2 style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय- विशेष लेखा परीक्षक सहकारी समितियॉं, DistrictNameHi") + "</label></b></td></tr>");
                    else if (_LoggedInUser.LevelId == 2)
                        sb.Append("<tr><td colspan=2 style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय- रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड , DistrictNameHi") + "</label></b></td></tr>");
                    else if (_LoggedInUser.LevelId == 1)
                        sb.Append("<tr><td colspan=2 style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय- रजिस्ट्रार, सहकारी समितियॉं राजस्थान ,जयपुर") + "</label></b></td></tr>");

                    sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");

                    sb.Append("<tr><td align='left' style='margin-top:15px;'> क्रमांक फा." + OrderNo + " </td><td align='right'> दिनांक " + DateTime.Now.ToString("dd/MM/yyyy") + "</td></tr>");
                    sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                    sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><u><b> ऑडिट आवंटन आदेश </b></u></td></tr>");
                    if (IsForced == "True")
                    {
                        sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><b> (रा.स.सो.अधि.धारा 54(2) व नियम 73(8) के तहत) </b></td></tr>");
                        sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><b> (सोसाइटियों के वर्ग / सोसायटी हेतु) </b></td></tr>");
                    }
                    else if (FinancialYearId > PrevFYId)//Current
                    {
                        sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><b> (रा.स.सो.अधि.धारा 54(2) व नियम 73(7) के तहत) </b></td></tr>");
                    }
                    else if (FinancialYearId < CurrentFYId)//FOR BACK LOG
                    {
                        sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><b> (रा.स.सो.अधि.धारा 54(2) व नियम 73(8) के तहत) </b></td></tr>");
                    }

                    sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");

                    if (IsForced == "True")
                    {
                        sb.Append("<tr><td align='justify'  colspan=2 style='margin-top:15px;'> <p>&nbsp;&nbsp;&nbsp; राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 (2) तथा राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(8) अनुसार नियम 73(4) से 73(7) में अंतर्विष्ट किसी बात के होने पर भी, रजिस्ट्रार आदेश द्वारा, किसी विशिस्ट कालावधि के लिए किसी सोसायटी या सोसाइटियों के किसी वर्ग के लेखाओं की लेखापरीक्षा कराने के लिए लेखापरीक्षक (लेखापरीक्षको) या लेखा परीक्षा फर्म (फर्मो ) को नियुक्त कर सकेगा | अधोहस्ताक्षरकर्ता को यह समाधान हो गया है , कि निम्नलिखित सहकारी समितियों की लेखा परीक्षा हेतु उक्त प्रावधानों के अंतर्गत विभाग स्तर से लेखा परीक्षक नियुक्त किया जाना आवश्यक है ' । </p></td></tr>");
                        sb.Append("<tr><td align='center'   colspan=2 >&nbsp; </td></tr>");
                        if (_LoggedInUser.LevelId == 3)
                            sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName विशेष लेखा परीक्षक सहकारी समितियॉ DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) तथा राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73 (8) के तहत प्रदत्त शक्तियों का प्रयोग करते हुए निम्नलिखित सहकारी समितियों के वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + " InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + "  को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                        else if (_LoggedInUser.LevelId == 2)
                        {
                            if (AuditerId.ToString().Contains(","))
                            {
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(8) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए SocietyName की वर्ष FinYear की लेखा परीक्षा हेतु निम्न लिखित लेखा निरीक्षकों की नियुक्ति करता / करती हूँ | </p></td></tr>");
                            }
                            else
                            {
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) तथा राजस्थान सहकारी सोसाइटी नियम 2003 के नियम 73(8) के तहत प्रदत्त शक्तियों का प्रयोग करते हुए निम्न सहकारी समितियों के वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + " InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + "  को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                            }

                        }

                        else if (_LoggedInUser.LevelId == 1)
                            sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, संयुक्त मुख्य अंकेक्षक ( जनरल ) सहकारी समितियॉं, राजस्थान जयपुर, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) तथा राजस्थान सहकारी सोसाइटी नियम 2003 के नियम 73 (8) के तहत प्रदत्त शक्तियों का प्रयोग करते हुए निम्न सहकारी समितियों के वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + " InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + "  को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");

                        sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");

                        if (AuditerId.ToString().Contains(","))
                        {
                            sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; उपरोक्त सहकारी समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी |</p></td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; निम्न सहकारी समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54  एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी |</p></td></tr>");
                        }


                    }
                    else
                    {
                        if (FinancialYearId > PrevFYId)
                        {
                            sb.Append("<tr><td align='justify'  colspan=2 style='margin-top:15px;'> <p>&nbsp;&nbsp;&nbsp; राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(4) के अनुसार निम्नलिखित सहकारी समितियॉं नियत समय में लेखा परीक्षक की नियुक्ति कर सुचना प्रस्तुत करने में विफल रही है ।  </p></td></tr>");
                            sb.Append("<tr><td align='center'   colspan=2 >&nbsp; </td></tr>");
                            if (_LoggedInUser.LevelId == 3)
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, विशेष लेखा परीक्षक सहकारी समितियॉ DistrictNameHi , राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(7) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                            else if (_LoggedInUser.LevelId == 2)
                            {
                                if (AuditerId.ToString().Contains(","))
                                {
                                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(7) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए SocietyName की वर्ष FinYear की लेखा परीक्षा हेतु निम्न लिखित लेखा निरीक्षकों की नियुक्ति करता / करती हूँ | </p></td></tr>");
                                }
                                else
                                {
                                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(7) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                                }
                            }
                            else if (_LoggedInUser.LevelId == 1)
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, संयुक्त मुख्य अंकेक्षक ( जनरल ) सहकारी समितियॉं, राजस्थान जयपुर, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(7) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");

                            sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");

                            if (AuditerId.ToString().Contains(","))
                            {
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; उपरोक्त सहकारी समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी |</p></td></tr>");
                            }
                            else
                            {
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; निम्न सहकारी समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी |</p></td></tr>");
                            }


                        }
                        else  //FOR BACK LOG
                        {
                            sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(3) के अनुसार प्रत्येक सोसाइटी के लेखाओ की लेखा परीक्षा उस वित्तीय वर्ष की जिससे ऐसे लेखे सम्बंधित है ,समाप्ति के छ माह के भीतर-भीतर की जाएगी | परन्तु  निम्नलिखित सहकारी समितियों द्वारा वर्ष " + FinancialYearName + " तक की ऑडिट करवाकर ऑडिट रिपोर्ट प्रस्तुत नहीं की गयी है | जिससे स्पष्ट है कि निम्नलिखित सहकारी समितियां अधिनियम के उक्त प्रावधानों की पालना करने में विफल रही है इसलिए इन सहकारी समितियों की वर्ष " + FinancialYearName + " तक बकाया समस्त लेखा परीक्षा हेतु विभाग स्तर से लेखा परीक्षक की नियुक्ति किया जाना आवश्यक हो गया है | </p></td></tr>");
                            sb.Append("<tr><td align='center'   colspan=2 >&nbsp; </td></tr>");

                            if (_LoggedInUser.LevelId == 3)
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, विशेष लेखा परीक्षक सहकारी समितियॉ DistrictNameHi , राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(8) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष " + FinancialYearName + " की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                            else if (_LoggedInUser.LevelId == 2)
                            {
                                if (AuditerId.ToString().Contains(","))
                                {
                                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(8) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए SocietyName की वर्ष FinYear की लेखा परीक्षा हेतु निम्न लिखित लेखा निरीक्षकों की नियुक्ति करता / करती हूँ | </p></td></tr>");
                                }
                                else
                                {
                                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(8) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष " + FinancialYearName + " की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                                }
                            }

                            else if (_LoggedInUser.LevelId == 1)
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, संयुक्त मुख्य अंकेक्षक ( जनरल ) सहकारी समितियॉं, राजस्थान जयपुर, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(8) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष " + FinancialYearName + " की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");


                            sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                            if (AuditerId.ToString().Contains(","))
                            {
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; उपरोक्त सहकारी समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी तथा लेखापरीक्षकगण आवंटीत समिति में अविलम्ब प्रारंभ कर दिनांक " + AuditTillDate + " तक अंकेक्षण कार्य सम्पादित करेंगे |</p></td></tr>");
                            }
                            else
                            {
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; निम्न सहकारी समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी तथा लेखापरीक्षकगण आवंटीत समिति में अविलम्ब प्रारंभ कर दिनांक " + AuditTillDate + " तक अंकेक्षण कार्य सम्पादित करेंगे |</p></td></tr>");
                            }


                        }
                    }

                    sb.Append("<tr><td align='center'  colspan=2 >");
                    sb.Append("<table style='width: 100%; margin-top:20px;' border='1' cellspacing='0' cellpadding='5'>");

                    if (AuditerId.ToString().Contains(","))
                    {
                        sb.Append("<tr><td align='center'style='width:70px;'> क्र. स. </td><td align='center'> लेखा निरीक्षक </td><td align='center'> सहकारी समिति  </td><td align='center'> कार्यालय </td></tr>");
                    }
                    else
                    {
                        sb.Append("<tr><td align='center'style='width:70px;'> क्र. स. </td><td align='center'> सहकारी समिति  </td><td align='center'> स्थान </td></tr>");
                    }


                    int i1 = 0;
                    foreach (DataRow dr in Dt.Rows)
                    {
                        i1++;
                        sb.Append("<tr><td  align='left'  > " + i1.ToString() + "</td>");

                        if (AuditerId.ToString().Contains(","))
                        {
                            sb.Append("<td  align='left'  > " + dr["OfficerNameEn"].ToString() + " </td>");
                            sb.Append("<td  align='left'  > " + dr["SocietyName"].ToString() + " (" + dr["RegistrationNo"].ToString() + ") </td>");
                            sb.Append("<td  align='left'  > " + dr["OfficeNameEn"].ToString() + " </td></tr>");
                        }
                        else
                        {
                            sb.Append("<td  align='left'  > " + dr["SocietyName"].ToString() + " (" + dr["RegistrationNo"].ToString() + ") </td>");
                            sb.Append("<td  align='left'  > " + (dr["SocietyPlaceEn"].ToString() != "" ? (dr["SocietyPlaceEn"].ToString() + " , ") : " ") + dr["DISTRICT_ENG"].ToString() + "</td></tr>");
                        }

                        if (AuditerId.ToString().Contains(","))
                        {
                            if (!OfficerName.Contains(dr["OfficerNameEn"].ToString()))
                                OfficerName = AuditBy == "1" ? dr["FirmName"].ToString() : OfficerName + ", " + dr["OfficerNameEn"].ToString();
                        }
                        else
                            OfficerName = AuditBy == "1" ? dr["FirmName"].ToString() : dr["OfficerNameEn"].ToString();
                        CurrentFYear = dr["CurrentFYear"].ToString() == "" ? FinancialYearName : dr["CurrentFYear"].ToString();
                        saveIDs = saveIDs + dr["Id"].ToString() + ",";
                        SocietyName = dr["SocietyName"].ToString().ToUpper();
                    }
                    if (OfficerName.Contains(", ") && OfficerName.StartsWith(","))
                    {
                        OfficerName = OfficerName.Substring(1);
                    }
                    sb.Append("</table></td></tr>");
                    sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                    sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                    sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                    sb.Append("</table ></div>");

                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    string fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + ".pdf";
                    string textVal = sb.ToString().Replace("<br>", "<br/>").Replace("DistrictNameHi", DistNameHi).Replace("SocietyName", SocietyName).Replace("DistrictNameHi", DistNameHi).Replace("FinYear", CurrentFYear).Replace("InspectorName", OfficerName).Replace("RegitrarName", _LoggedInUser.OfficerNameEn);
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(textVal));
                    string FilePath = "../Content/AuditReport/" + fileName;

                    if (COMMON_FUNCTION.GeneratePDF(sb1, Server.MapPath(FilePath)))
                    {
                        ResultMessage objRM = new ResultMessage();
                        objRM = COMMON_FUNCTION.EsinghPDF(Tran, Server.MapPath(FilePath), DistName);
                        if (objRM.data == "Success")
                        {
                            if (Ids.Length > 0)
                                saveIDs = saveIDs.Substring(0, saveIDs.Length - 1);

                            ResultIntStr = objSocietyModel.saveAuditAssignbydeprtNotRequest(AuditerId, AuditBy, Ids, Status, IsFirm, _LoggedInUser.UserId, FilePath, _LoggedInUser.UserRoleDeptMappingId, AuditTillDate, IsForced, _LoggedInUser.SSOID, FinancialYearId, OrderNo).ToString();
                            if (ResultIntStr != "-1" && ResultIntStr != "-2")
                                ResultIntStr = "Success";
                            else
                            {
                                objAE.InActiveFlag = 1;
                                objAC.SaveAuditOrder(objAE);
                            }

                        }
                    }
                    else
                    {
                        objAE.InActiveFlag = 1;
                        objAC.SaveAuditOrder(objAE);
                    }
                }

            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Society", FunctionName = " saveAuditAssignbydeprtNotRequest(string AuditerId = \"\", string AuditBy = \"\", string Ids = \"\", string Status = \"\", string IsFirm = \"\", string Tran = \"\", int FinancialYearId = 0, string FinancialYearName = \"\", string AuditTillDate = \"\", string IsForced =" }, ex);

            }
            if (ResultIntStr == "Error")
            {
                objAE.InActiveFlag = 1;
                objAC.SaveAuditOrder(objAE);
            }
            return ResultIntStr;
        }

        [HttpPost]
        [ActionName("SaveBankDetails")]
        public string SaveBankDetail(SocietyRegistrationEntity _Entity)
        {
            DataTable DT = null;
            int KioskResult = -1;
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            string fileName = string.Empty;
            string pathString = "~/Content/SocietyProfiling/";
            string Flags = "";
            string Result = "";
            foreach (string files in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[files];
                fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                if (files.ToString() == "BankPassbookFile")
                {
                    _Entity.BankPassbookFile = fileName;
                }
                _Entity.flag = 1;
                if (_LoggedInUser.UserType == "K")
                {
                    Flags = objSocietyModel.SaveBankDetailForKiosk(_Entity);
                    if (Flags != "")
                    {
                        EmitraModelFactory ObjEmitraModel = new EmitraModelFactory(new DBLogger());
                        EmitraController OBJ_EmitraController = new EmitraController();
                        if (_LoggedInUser.UserType == "K")
                        {
                            //SocietyId = DT.Rows[0]["SocietyId"].ToString();

                            EmitraModelFactory objEmitraModel = new EmitraModelFactory(new DBLogger());
                            DT = objEmitraModel.SocietyForUploadBankCertificate("", _Entity.SocietyIDEnc);

                            if (System.Web.HttpContext.Current.Session["ARequest"] != null && _LoggedInUser.KIOSKCODE.Length > 0 && DT != null && DT.Rows.Count > 0 && DT.Rows[0]["StatusId"].ToString() == "14")
                            {
                                KioskResult = ObjEmitraModel.MakePaymentForAll("0", DT.Rows[0]["SocietyName"].ToString(), "0", DT.Rows[0]["RegistrationNo"].ToString(), Flags, _LoggedInUser.KIOSKCODE, _LoggedInUser.SSOID);

                                if (KioskResult > 0)
                                {
                                    Flags = "Sucess";
                                    pathString += DT.Rows[0]["DirectoryName"].ToString();
                                    file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));

                                    Flags = objSocietyModel.SaveBankDetailForKioskmodel(KioskResult, _Entity.SocietyIDEnc);
                                    //MOVE TEMP DATA TO MAIN
                                    //ApplicationFlowModel AFM = new ApplicationFlowModel();
                                    //AFM.AddApplicationFlow(_Entity.SocietyIDEnc, 24, "BANK ACCOUNT DETAILS UPLOADED", 0);
                                    DResponse resp = JsonConvert.DeserializeObject<DResponse>(Flags);

                                    Flags = "<div class=\"col-lg-12 form-group citizen-dash-head\"><div class=\"col-lg-12 table-responsive\"><table class=\"table devtable\">" +
                                    "<thead><tr><th class=\"ng-binding\" colspan=\"4\">Society Fee Payment Details</th>" +
                                    //"<th class=\"ng-binding\">Column 2</th><th class=\"ng-binding\">Column 3</th> <th>Column 4</th>" +
                                    "</tr></thead><tbody>" +
                                    "<tr><td class=\"ng-binding\">TRANSACTIONID</td><td class=\"ng-binding\">" + resp.TRANSACTIONID + "</td><td class=\"ng-binding\">RECEIPTNO</td><td class=\"ng-binding\">" + resp.RECEIPTNO + "</td></tr>" +
                                    "<tr><td class=\"ng-binding\">PAIDAMOUNT</td><td class=\"ng-binding\">" + resp.TRANSAMT + "</td><td class=\"ng-binding\">Date</td><td class=\"ng-binding\">" + resp.EMITRATIMESTAMP.Substring(0, 8) + "</td></tr>" +
                                    "<tr><td class=\"ng-binding\">RESPONSEMESSAGE</td><td class=\"ng-binding\">" + resp.MSG + "</td><td class=\"ng-binding\"></td><td class=\"ng-binding\"></td></tr>" +
                                    "</tbody></table></div></div>";
                                }
                                else
                                {
                                    Flags = objSocietyModel.GetEmitraBankStatmentLastTran(int.Parse(Flags));
                                    DResponse resp = JsonConvert.DeserializeObject<DResponse>(Flags);
                                    Flags = "<div class=\"col-lg-12 form-group citizen-dash-head\"><div class=\"col-lg-12 table-responsive\"><table class=\"table devtable\">" +
                                    "<thead><tr><th class=\"ng-binding\" colspan=\"4\">Society Fee Payment Details</th>" +
                                    //"<th class=\"ng-binding\">Column 2</th><th class=\"ng-binding\">Column 3</th> <th>Column 4</th>" +
                                    "</tr></thead><tbody>" +
                                    "<tr><td class=\"ng-binding\">TRANSACTIONID</td><td class=\"ng-binding\">" + resp.TRANSACTIONID + "</td><td class=\"ng-binding\">RECEIPTNO</td><td class=\"ng-binding\">" + resp.RECEIPTNO + "</td></tr>" +
                                    "<tr><td class=\"ng-binding\">PAIDAMOUNT</td><td class=\"ng-binding\">" + resp.TRANSAMT + "</td><td class=\"ng-binding\">Date</td><td class=\"ng-binding\">" + resp.EMITRATIMESTAMP.Substring(0, 8) + "</td></tr>" +
                                    "<tr><td class=\"ng-binding\">RESPONSEMESSAGE</td><td class=\"ng-binding\">" + resp.MSG + "</td><td class=\"ng-binding\"></td><td class=\"ng-binding\"></td></tr>" +
                                    "</tbody></table></div></div>";
                                }

                                //SocietyId += COMMON_FUNCTION.Encrypt(result.ToString());
                            }
                        }

                    }
                }
                else
                {
                    Flags = objSocietyModel.SaveBankDetailmodel(_Entity);
                    if (Flags.Length > 5 && Flags != "-1")
                    {
                        pathString += Flags;
                        file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));
                    }
                }

            }
            return Flags;

        }


        [HttpGet]
        [ActionName("GetAuditorAssignReportDataShow")]
        // public string GetAuditorAssignReportDataShow(int SocietyId)
        public string GetAuditorAssignReportDataShow(string SocietyIDEnc)
        {
            SocietyAuditorAssigmentEntity _Model = new SocietyAuditorAssigmentEntity();
            _Model.SocietyIDEnc = SocietyIDEnc;
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());

            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
            {
                if (Request.Cookies["SSOROLE"].Value == "CITIZEN")
                {
                    _Model.SSOID = Request.Cookies["SSO_ID"].Value;
                }

            }

            return COMMON_FUNCTION.DataTableToJSONString(objSocietyModel.GetAuditorAssignReportDataShow(_Model));
        }

        [HttpGet]
        [ActionName("GetAllBasicDetails")]
        public string GetAllBasicDetails(int SocietyId)
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());


            return COMMON_FUNCTION.DataTableToJSONString(objSocietyModel.GetBasicdetailsData((SocietyId)));
        }

        [HttpPost]
        public string DeleteAuditReport(string Id, string Action)
        {
            int ClassId = 0, ActionId = 0;
            if (!int.TryParse(Id, out ClassId) && !int.TryParse(Action, out ActionId))
                return CommonMessageFactory.Message(401, (Request.Cookies[SessionIdentity.Culture]).Value);

            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;


            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return CommonMessageFactory.Response(CommonMessageFactory.Message(objSocietyModel.DeleteAuditreport(ClassId, ActionId), (Request.Cookies[SessionIdentity.Culture]).Value));

        }

        [HttpPost]
        public string DeleteSocietyAuditorAssigments(string Id, string Action)
        {
            int ClassId = 0, ActionId = 0;
            if (!int.TryParse(Id, out ClassId) && !int.TryParse(Action, out ActionId))
                return CommonMessageFactory.Message(401, (Request.Cookies[SessionIdentity.Culture]).Value);

            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;


            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return CommonMessageFactory.Response(CommonMessageFactory.Message(objSocietyModel.DeleteSocietyAuditorAssigments(ClassId, ActionId), (Request.Cookies[SessionIdentity.Culture]).Value));

        }

        [HttpPost]
        [ActionName("SaveSocietyFinalSubmition")]
        public string SaveSocietyFinalSubmition(string SocietyIdEncr)
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return objSocietyModel.SaveSocietyFinalSubmition(SocietyIdEncr).ToString();
        }

        [HttpGet]
        public ActionResult SocietyCertificateGeneration()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.SocietyCertificateGeneration, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpGet]
        [ActionName("SocietyCertificateData")]
        public string SocietyPaymentdata(string SocietyRegistrationNo)
        {
            SocietyRegistrationNo = "1";
            string WhereClause = " and 1='" + SocietyRegistrationNo + "'";
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());

            DataTable dt = objSocietyModel.GetSocietyCertificateDataList(WhereClause);
            return COMMON_FUNCTION.DataTableToJSONString(dt); ;
        }


        public ActionResult SocietyApplicationView(string SocietyIDEnc)
        {
            // ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.SocietyApplicationView, (Request.Cookies[SessionIdentity.Culture]).Value);
            if (SocietyIDEnc != null)
            {
                System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc] = SocietyIDEnc;
            }
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.SocietyApplicationView, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        //[HttpGet]
        //public string GetCurrentSocietyIDEncdata()
        //{
        //    string SocietyIDEnc = "";
        //    SocietyIDEnc = System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc].ToString();
        //    //    System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc] = null;
        //    //}
        //    return SocietyIDEnc;
        //}
        [HttpGet]
        [ActionName("GetCurrentSocietyIDEnc")]
        public string GetCurrentSocietyIDEnc()
        {
            string SocietyIDEnc = "";
            //if (System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc] != null)
            //{
            SocietyIDEnc = System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc].ToString();
            //    System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc] = null;
            //}
            return SocietyIDEnc;
        }


        public void GenerateOTPForEsing(string aadhar)
        {
            string errormsg = string.Empty;
            string succmsg = string.Empty;
            //hdn_aadhar.Value = "428434778643";
            if (aadhar.Length > 0)
            {
                //string url = ConfigurationManager.AppSettings["eSignGenerateOTP"].ToString();
                string url = "https://api.sewadwaar.rajasthan.gov.in/app/live/rajesignfly_v2/webresources/genericsendAadhar/OTP?client_id=f281c943-d1e8-4204-88e0-c26767fefaa0";
                string status = "";
                string TransactionId = "";
                string json = "{\"aadharid\":\"" + aadhar + "\", \"departmentname\" : \"RCMS\"}";
                try
                {
                    string ss = WebRequestinJson(url, json, "application/json");
                    JObject root = (JObject)JObject.Parse(ss);
                    foreach (var item in root)
                    {
                        if (item.Key == "Status")
                            status = item.Value.ToString().Replace("\"", ""); ;
                        if (item.Key == "TransactionId")
                            TransactionId = item.Value.ToString().Replace("\"", ""); ;
                    }
                    if (!status.Equals("1"))
                        errormsg = "status - " + status;
                    else if (status.Equals("1"))
                    {
                        Session["TransactionId"] = TransactionId.Replace("\"", "");
                        succmsg = "OTP has been sent on your registered Mobile No.";
                    }
                }
                catch (Exception ex)
                {
                    errormsg = ex.Message;
                }
            }
            else
            {
                errormsg = "Fill Aadhat ID!";
            }
            if (!String.IsNullOrEmpty(errormsg))
            {
                //displayMsg = "true";
                //msglbl.InnerText = errormsg;
                //msglbl.Visible = true;
                //msgDiv.Visible = true;
                ////"msgcss"
                //msgDiv.Attributes.Add("class", "msgcss");
            }
            else if (!String.IsNullOrEmpty(succmsg))
            {
                //displayMsg = "true";
                //msglbl.InnerText = succmsg;
                //msglbl.Visible = true;
                //msgDiv.Visible = true;
                //msgDiv.Attributes.Add("class", "succmsg");
            }

        }

        public string WebRequestinJson(string url, string postData, string contenttype)
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

        [HttpPost]
        [ActionName("GetBrnDetailsOld")]
        public string GetBrnDetailsOld(string BRN, string IsNewReg)
        {
            SocietyModel objSocietyModel = new SocietyModel();
            string ReturnJSON = "";
            //if (_loggedInUSerDetails.AadharNo != null && (_loggedInUSerDetails.AadharNo != "" && _loggedInUSerDetails.UserType == "C"))
            if (_loggedInUSerDetails.AadharNo != "" && _loggedInUSerDetails.AadharNo != null)
            {
                int IsPresent = (objSocietyModel.CheckSocietyByBRN(BRN, _loggedInUSerDetails.SSOID, _loggedInUSerDetails.UserType == "K" ? "" : (COMMON_FUNCTION.Encrypt(_loggedInUSerDetails.AadharNo)), (IsNewReg == "true" ? 1 : 0)));
                if (IsPresent == 1)
                {
                    ILogger objILogger = new DBLogger();
                    bool IsValid = true;
                    System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
                    try
                    {

                        BRNService.brwsSoapClient OBJBRN = new BRNService.brwsSoapClient();
                        string hashmd5, hashmd5salt, hashdoubelmd5, Bas64;
                        Random rnd = new Random();
                        int RandomNo = rnd.Next(10000000, 99999999);

                        hashmd5 = COMMON_FUNCTION.CalculateMD5Hash("Coop$3667");

                        hashmd5salt = RandomNo.ToString() + hashmd5;

                        hashdoubelmd5 = COMMON_FUNCTION.CalculateMD5Hash(hashmd5salt);

                        Bas64 = COMMON_FUNCTION.Base64Encode(hashdoubelmd5);

                        string Xmlstr = OBJBRN.GetBRNDetailXML("rajcoop", Bas64, RandomNo.ToString(), BRN).ToString();

                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(Xmlstr);
                        XmlNode str = doc.DocumentElement;

                        DataTable DT = new SocietyModel().GetLovValuesByListId(1);
                        foreach (DataRow Dr in DT.Rows)
                        {
                            if (str.SelectSingleNode("BO_Name").InnerText.Contains(Dr["LovCode"].ToString()) || str.SelectSingleNode("BO_Name").InnerText.Contains(Dr["Lovdescription"].ToString()))
                            {
                                str.SelectSingleNode("BO_Name").InnerText = str.SelectSingleNode("BO_Name").InnerText + " Due To this " + Dr["LovCode"].ToString() + " / " + Dr["Lovdescription"].ToString() + " Contains";
                                IsValid = false;
                                break;
                            }
                        }


                        if (str.SelectSingleNode("Status").InnerText == "1" && IsValid)
                        {
                            ReturnJSON = "{\"result\":\"" + "success" + "\",\"BRN\":\"" + str.SelectSingleNode("BRN").InnerText + "\",\"Applicant_No\":\"" + str.SelectSingleNode("Applicant_No").InnerText + "\",\"BO_Name\":\"" + str.SelectSingleNode("BO_Name").InnerText + "\"}";
                        }
                        else if (str.SelectSingleNode("Status").InnerText == "1" && !IsValid)
                        {
                            ReturnJSON = "{\"result\":\"" + "invalidname" + "\",\"BRN\":\"" + str.SelectSingleNode("BRN").InnerText + "\",\"Applicant_No\":\"" + str.SelectSingleNode("Applicant_No").InnerText + "\",\"BO_Name\":\"" + str.SelectSingleNode("BO_Name").InnerText + "\"}";
                        }
                        else
                        {
                            ReturnJSON = "{\"result\":\"" + "failed" + "\",\"BRN\":\"" + "Invalid BRN no." + "\"}";
                        }


                        //throw new Exception(Xmlstr);

                    }
                    catch (Exception ex)
                    {
                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SOCIETY", FunctionName = "GetBrnDetails()" }, ex);
                        ReturnJSON = "{\"result\":\"" + "failed" + "\",\"BRN\":\"" + "BRN Services Temporarily Down Please Try After Some Time." + "\"}";
                    }
                }
                else
                {
                    ReturnJSON = "{\"result\":\"" + "failed" + "\",\"BRN\":\"" + "BRN number is already exist." + "\"}";
                }
            }
            else
            {
                ReturnJSON = "{\"result\":\"" + "failed" + "\",\"BRN\":\"" + "Please update your aadhar no on sso portal first." + "\"}";
            }
            return ReturnJSON;
        }

        [HttpPost]
        [ActionName("GetBrnDetails")]
        public string GetBrnDetails(string BRN, string IsNewReg, string SocietyIdENC)
        {
            SocietyModel objSocietyModel = new SocietyModel();
            string sBRNPassword, sEncPwd, sPwdSalt, sEncPwdSalt, sBase64;
            int iRnd;
            string ReturnJSON = "";
            //if (_loggedInUSerDetails.AadharNo != null && (_loggedInUSerDetails.AadharNo != "" && _loggedInUSerDetails.UserType == "C"))
            if (_loggedInUSerDetails.UserType == "K" || (_loggedInUSerDetails.UserType == "C" && _loggedInUSerDetails.AadharNo != "" && _loggedInUSerDetails.AadharNo != null))
            {
                int IsPresent = (objSocietyModel.CheckSocietyByBRN(BRN, _loggedInUSerDetails.SSOID, _loggedInUSerDetails.UserType == "K" ? "" : (COMMON_FUNCTION.Encrypt(_loggedInUSerDetails.AadharNo)), (IsNewReg == "true" ? 1 : 0), SocietyIdENC));
                if (IsPresent == 1)
                {
                    ILogger objILogger = new DBLogger();
                    bool IsValid = true;
                    System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
                    try
                    {
                        BRNEntity obj = new BRNEntity();
                        string Jsonstr = "";
                        if (Request.Url.Host.Contains("http://10.68.252.116/rajsahakarapp"))
                        {
                            Jsonstr = "{\"Status\":\"1\",\"BRN\":\"" + BRN + "\",\"Area\":\"Rural\",\"District\":\"BHILWARA\",\"Tehsil\":\"Sahada\",\"Village\":\"Amli\",\"LocalBody\":\"\",\"Ward\":\"\",\"BO_Name\":\"Test Society Name\",\"BO_HouseNo\":\"123\",\"BO_Lane\":\"AMLI\",\"BO_Locality\":\"Amli Amli Sahada BHILWARA\",\"BO_PinCode\":\"311801\",\"BO_TelNo\":\"9772899513\",\"BO_Email\":\"\",\"BO_PanNo\":\"\",\"BO_TanNo\":\"\",\"HO_Name\":\"\",\"HO_HouseNo\":\"\",\"HO_Lane\":\"\",\"HO_Locality\":\"\",\"HO_PinCode\":\"\",\"HO_TelNo\":\"\",\"HO_EMail\":\"\",\"HO_PanNo\":\"\",\"HO_TanNo\":\"\",\"NIC_Code\":\"851\",\"Year\":\"2020\",\"Ownership\":\"Govt./PSU (1)\",\"Total_Person\":\"1\",\"ActAuthorityRegNo\":\"Societies Registration Act (na)\",\"Applicant_Name\":\"Anju Sharma\",\"Applicant_No\":\"9772899513\",\"Applicant_EMail\":\"anju17590@gmail.com\",\"Applicant_Address\":\"Labhesh Kumar Joshi Sahara ganw-gangapur th. sahada 3388-kund chowk Bhilwara Rajasthan 311806\"}";
                        }
                        else
                        {
                            //BRNService.brwsSoapClient OBJBRN = new BRNService.brwsSoapClient();
                            //sBRNPassword = "Coop$3667";
                            //Random rnd = new Random();
                            //iRnd = rnd.Next(10000000, 99999999);
                            //sEncPwd = CreateMD5(sBRNPassword);
                            //sPwdSalt = iRnd.ToString() + sEncPwd;
                            //sEncPwdSalt = CreateMD5(sPwdSalt);
                            //sBase64 = GetBase64Encode(sEncPwdSalt);
                            //string Xmlstr = OBJBRN.GetBRNDetailXML("rajcoop", Bas64, RandomNo.ToString(), BRN).ToString();
                            //Jsonstr = OBJBRN.GetBRNDetailJson("rajcoop", sBase64, iRnd.ToString(), BRN).ToString();

                            obj = COMMON_FUNCTION.GetBRN(BRN);

                        }

                        //Jsonstr= COMMON_FUNCTION.GetBRN(BRN);



                        //System.Web.Script.Serialization.JavaScriptSerializer sss = new System.Web.Script.Serialization.JavaScriptSerializer();

                        //try
                        //{
                        //    obj = Jsonstr.Equals("{}") ? new BRNEntity() : (BRNEntity)s.Deserialize(Jsonstr, typeof(BRNEntity));
                        //}
                        //catch (Exception ex)
                        //{
                        //    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SOCIETY", FunctionName = "When Convert JSON To BRNEntity" }, ex);
                        //    return CommonMessageFactory.Response("Provided input data is not in correct format.");
                        //}


                        //XmlDocument doc = new XmlDocument();
                        //doc.LoadXml(Xmlstr);
                        //XmlNode str = doc.DocumentElement;

                        //DataTable DT = new SocietyModel().GetLovValuesByListId(1);
                        //foreach (DataRow Dr in DT.Rows)
                        //{
                        //    if (obj.BO_Name.ToString().Contains(Dr["LovCode"].ToString()) || obj.BO_Name.ToString().Contains(Dr["Lovdescription"].ToString()))
                        //    {
                        //        obj.BO_Name = obj.BO_Name.ToString() + " Due To this " + Dr["LovCode"].ToString() + " / " + Dr["Lovdescription"].ToString() + " Contains";
                        //        IsValid = false;
                        //        break;
                        //    }
                        //}

                        if (obj != null)
                        {
                            if (obj.Status.ToString() == "1" && IsValid)
                            {
                                ReturnJSON = "{\"result\":\"" + "success" + "\",\"BRN\":\"" + obj.BRN + "\",\"Applicant_No\":\"" + obj.Applicant_No + "\",\"BO_Name\":\"" + obj.BO_Name + "\"}";
                            }
                            else if (obj.Status == "1" && !IsValid)
                            {
                                ReturnJSON = "{\"result\":\"" + "invalidname" + "\",\"BRN\":\"" + obj.BRN + "\",\"Applicant_No\":\"" + obj.Applicant_No + "\",\"BO_Name\":\"" + obj.BO_Name + "\"}";
                            }
                            else
                            {
                                ReturnJSON = "{\"result\":\"" + "failed" + "\",\"BRN\":\"" + "Invalid BRN no." + "\"}";
                            }
                        }
                        else
                            ReturnJSON = "{\"result\":\"" + "failed" + "\",\"BRN\":\"" + "BRN Services Temporarily Down Please Try After Some Time." + "\"}";

                    }
                    catch (Exception ex)
                    {
                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SOCIETY", FunctionName = "GetBrnDetails()" }, ex);
                        ReturnJSON = "{\"result\":\"" + "failed" + "\",\"BRN\":\"" + "BRN Services Temporarily Down Please Try After Some Time." + "\"}";
                    }
                }
                else
                {
                    ReturnJSON = "{\"result\":\"" + "failed" + "\",\"BRN\":\"" + "BRN number is already exist." + "\"}";
                }
            }
            else
            {
                ReturnJSON = "{\"result\":\"" + "failed" + "\",\"BRN\":\"" + "Please update your aadhar no on sso portal first." + "\"}";
            }
            return ReturnJSON;
        }



        [HttpPost]
        [ActionName("SocietyRegReport")]
        public string SocietyRegReport(SocietyRegistrationEntity objSocietyregModel)
        {

            string WhereCluase = "";
            if (objSocietyregModel.DISTRICT_ID != 0)
                WhereCluase += " and dis.DISTRICT_ID=" + (objSocietyregModel.DISTRICT_ID);
            if (objSocietyregModel.ActId != 0)
                WhereCluase += " and sact.ActId=" + objSocietyregModel.ActId;
            if (objSocietyregModel.SocietyTypeId != 0)
                WhereCluase += " and socty.SocietyTypeId=" + objSocietyregModel.SocietyTypeId;
            if (objSocietyregModel.YEAR != "0" && objSocietyregModel.YEAR != null)
                WhereCluase += " and YEAR(soc.RegistrationDate)='" + objSocietyregModel.YEAR + "'";
            if (objSocietyregModel.MONTH != "0" && objSocietyregModel.MONTH != null)
                WhereCluase += " and Format(soc.RegistrationDate,'MMMM')='" + objSocietyregModel.MONTH + "'";
            //if(objSocietyregModel.DISTRICT_ID = 0 && objSocietyregModel.ActId = 0 && objSocietyregModel.YEAR)
            //    WhereCluase += "1 = 1";


            SocietyModel objSocietyModel = new SocietyModel(new FileLogger());

            return COMMON_FUNCTION.DataTableToJSONString(objSocietyModel.GetSocietyRegReport(WhereCluase));
        }


        [HttpGet]
        [ActionName("GetSocietyDistrictList")]
        public string GetSocietyDistrictList()
        {

            SocietyModel objSocietyModel = new SocietyModel();

            return COMMON_FUNCTION.DataTableToJSONString(objSocietyModel.GetSocietyDistrictList());
        }

        [HttpGet]
        [ActionName("GetSocietyActList")]
        public string GetSocietyActList()
        {

            SocietyModel objSocietyModel = new SocietyModel();

            return COMMON_FUNCTION.DataTableToJSONString(objSocietyModel.GetSocietyActList());
        }

        [HttpGet]
        [ActionName("GetSocietyTypeList")]
        public string GetSocietyTypeList()
        {

            SocietyModel objSocietyModel = new SocietyModel();

            return COMMON_FUNCTION.DataTableToJSONString(objSocietyModel.GetSocietyTypeList());
        }
        [HttpGet]
        [ActionName("GetSocietyYearList")]
        public string GetSocietyYearList()
        {

            SocietyModel objSocietyModel = new SocietyModel();

            return COMMON_FUNCTION.DataTableToJSONString(objSocietyModel.GetSocietyYearList());
        }

        [HttpGet]
        [ActionName("GetSocietyMonthList")]
        public string GetSocietyMonthList()
        {

            SocietyModel objSocietyModel = new SocietyModel();

            return COMMON_FUNCTION.DataTableToJSONString(objSocietyModel.GetSocietyMonthList());
        }

        [HttpPost]
        [ActionName("GetSocietiesForDeshBoardBySocietyId")]
        public string GetSocietiesForDeshBoardBySocietyId(string SocietyId)
        {
            if (string.IsNullOrEmpty(SocietyId))
                return "{}";

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            DataTable DT = new DataTable();

            DT = objSocietyModel.GetSocietiesForDeshBoardBySocietyId(SocietyId);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }


        public string GetSocityExcelDetails()
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objSocietyModel.GetSocityExcelDetails(_LoggedInUser.UserId, _LoggedInUser.UserRoleDeptMappingId);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }


        [HttpPost]
        [ActionName("GetSocityExcelDetailsCourt")]
        public string GetSocityExcelDetailsCourt()
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objSocietyModel.GetSocityExcelDetailsCourt(_LoggedInUser.UserId);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }


        [HttpPost]
        [ActionName("GetExlSocietyDetails")]
        public string GetExlSocietyDetails(int TypeId, int ExlId, int StatusId)
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objSocietyModel.GetExlSocietyDetails(TypeId, ExlId, StatusId, _LoggedInUser.UserRoleDeptMappingId);
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        }

        [HttpPost]
        [ActionName("UpdateExcelSocityDetailsByExcelUpload")]
        public string UpdateExcelSocityDetailsByExcelUpload(string SocietyId, string AdministrativeId, string SocietyName, string RegistrationNo, string RegistrationDate, string Name, string ContactNo, string Aadh, string SocietyTypeId)
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return objSocietyModel.UpdateExcelSocityDetailsByExcelUpload(SocietyId, AdministrativeId, SocietyName, RegistrationNo, RegistrationDate, Name, ContactNo, Aadh, SocietyTypeId);
        }



        [HttpPost]
        [ActionName("GetExlSocietyDetailsCourt")]
        public string GetExlSocietyDetailsCourt(int ExlId, int StatusId)
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objSocietyModel.GetExlSocietyDetailsCourt(ExlId, StatusId);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }

        [HttpPost]
        [ActionName("InsertSocityByExcelCourt")]
        public string InsertSocityByExcelCourt(CaseRegistraion _Model)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            string fileName = string.Empty;
            _Model.ExcelUrl = "";
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    _Model.ExcelUrl = fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("~/Content/CourtExcel"), fileName));
                }
                catch { }
            }
            _Model.districtid = int.Parse(_LoggedInUser.DistrictId.ToString());
            _Model.CreatedBy = _Model.UpdatedBy = int.Parse(_LoggedInUser.UserId.ToString());
            _Model.CreatedIP = COMMON_FUNCTION.GetIpAddress();
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return objSocietyModel.InsertSocityByExcelCourt(_Model).ToString();
            //return CommonMessageFactory.Message(int.Parse(objSocietyModel.InsertSocityByExcel(_Model).ToString()), (Request.Cookies[SessionIdentity.Culture]).Value);
        }

        [HttpPost]
        public string HouseSocietyListForAuditRPT(SocietyRegistrationEntity objSocietyRegistrationEntity)
        {
            SocietyModel objSocietyModel = new SocietyModel(new FileLogger());
            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
            {
                objSocietyRegistrationEntity.UserType = (Request.Cookies["SSOROLE"].Value == "CITIZEN" ? "C" : "D");
                objSocietyRegistrationEntity.SSOID = Request.Cookies["SSO_ID"].Value;
            }
            DataTable DT = objSocietyModel.HouseSocietyListForAuditRPT(objSocietyRegistrationEntity);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }


        [HttpPost]
        [ActionName("GetUserDistrictId")]
        public string GetUserDistrictId()
        {
            return (_LoggedInUser.DistrictId.ToString());
        }


        [HttpPost]
        [ActionName("InsertSocityByExcelSTLoan")]
        public string InsertSocityByExcelSTLoan(SocietyRegistrationEntity _Model)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            string fileName = string.Empty;
            _Model.ExcelUrl = "";
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    _Model.ExcelUrl = fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("~/Content/SocietyExcel"), fileName));
                }
                catch { }
            }
            _Model.CreatedBy = _Model.UpdatedBy = int.Parse(_LoggedInUser.UserId.ToString());
            _Model.CreatedIP = COMMON_FUNCTION.GetIpAddress();
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return objSocietyModel.InsertSocityByExcelSTLoan(_Model).ToString();
            //return CommonMessageFactory.Message(int.Parse(objSocietyModel.InsertSocityByExcel(_Model).ToString()), (Request.Cookies[SessionIdentity.Culture]).Value);
        }

        #region BRN
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
        public static string ValidateBRN(string sBRN)
        {
            string BrnDetails, sBRNPassword, sEncPwd, sPwdSalt, sEncPwdSalt, sBase64;
            StringBuilder BrnStr = new StringBuilder();
            int iRnd;

            try
            {
                BRNService.brwsSoapClient objValidateBRN = new BRNService.brwsSoapClient();

                sBRNPassword = "Coop$3667";
                Random rnd = new Random();
                iRnd = rnd.Next(10000000, 99999999);
                sEncPwd = CreateMD5(sBRNPassword);
                sPwdSalt = iRnd.ToString() + sEncPwd;
                sEncPwdSalt = CreateMD5(sPwdSalt);
                sBase64 = GetBase64Encode(sEncPwdSalt);
                //XmlNode sXMLNode = objValidateBRN.GetBRNDetailXML("rajcoop", sBase64, iRnd.ToString(), sBRN);
                string Jsonstr = objValidateBRN.GetBRNDetailJson("rajcoop", sBase64, iRnd.ToString(), sBRN).ToString();
                //BrnStr.Append("<District>" + sXMLNode.SelectSingleNode("District").InnerText + "</District>");
                //BrnStr.Append("<Tehsil>" + sXMLNode.SelectSingleNode("Tehsil").InnerText + "</Tehsil>");
                //BrnStr.Append("<Village>" + sXMLNode.SelectSingleNode("Village").InnerText + "</Village>");
                //BrnStr.Append("<BO_Name>" + sXMLNode.SelectSingleNode("BO_Name").InnerText + "</BO_Name>");
                //BrnStr.Append("<BO_Lane>" + sXMLNode.SelectSingleNode("BO_Lane").InnerText + "</BO_Lane>");
                //BrnStr.Append("<BO_PinCode>" + sXMLNode.SelectSingleNode("BO_PinCode").InnerText + "</BO_PinCode>");
                //BrnStr.Append("<BO_TelNo>" + sXMLNode.SelectSingleNode("BO_TelNo").InnerText + "</BO_TelNo>");
                //BrnStr.Append("<BO_Email>" + sXMLNode.SelectSingleNode("BO_Email").InnerText + "</BO_Email>");
                //BrnStr.Append("<NIC_Code>" + sXMLNode.SelectSingleNode("NIC_Code").InnerText + "</NIC_Code>");
                //BrnStr.Append("<Year>" + sXMLNode.SelectSingleNode("Year").InnerText + "</Year>");
                //BrnStr.Append("<Status>" + sXMLNode.SelectSingleNode("Status").InnerText + "</Status>");
                //BrnStr.Append("<Ownership>" + sXMLNode.SelectSingleNode("Ownership").InnerText + "</Ownership>");
                //BrnStr.Append("<Total_Person>" + sXMLNode.SelectSingleNode("Total_Person").InnerText + "</Total_Person>");
                //BrnStr.Append("<ActAuthorityRegNo>" + sXMLNode.SelectSingleNode("ActAuthorityRegNo").InnerText + "</ActAuthorityRegNo>");
            }
            catch (Exception ex)
            {
                BrnDetails = ex.Message;
                //ErrorLog(ex.Message + " " + ex.InnerException + " BRN:- " + sBRN, System.Reflection.MethodBase.GetCurrentMethod().Name, System.IO.Path.GetFileName(HttpContext.Current.Request.Path));
            }
            return BrnDetails = "<BrnDetails>" + BrnStr.ToString() + "</BrnDetails>";

        }
        #endregion



        [HttpPost]
        [ActionName("SaveHousingSocietyAuditReportDetails")]
        public int SaveHousingSocietyAuditReportDetails(SocietyAuditReportEntity _Entity)
        {
            List<UploadFileDetailSocietyInspectorAssignmentEntity> lst = new List<UploadFileDetailSocietyInspectorAssignmentEntity>();
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                if (Request.Files != null && Request.Files.Count > 0) //&& Request.Files[0].ToString() == "AuditReport"
                {
                    //HttpPostedFileBase file = Request.Files[0];
                    //string fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    //file.SaveAs(Path.Combine(Server.MapPath("~/Content/AuditReport/"), fileName));
                    //_Entity.AuditReport = fileName;

                    string fileName = string.Empty;
                    string pathString = "~/Content/AuditReport/";
                    string FileList = string.Empty;
                    string ResolutionPDF = "";
                    string AuditorAssignFilePath = "";
                    foreach (string files in Request.Files)
                    {

                        HttpPostedFileBase file = Request.Files[files];

                        if (!COMMON_FUNCTION.CheckMimeType(file.InputStream, Path.GetExtension(file.FileName)))
                            return -10;
                        file.InputStream.Seek(0, SeekOrigin.Begin);

                        fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                        file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));
                        UploadFileDetailSocietyInspectorAssignmentEntity obj = new UploadFileDetailSocietyInspectorAssignmentEntity();

                        if (files == "HousingSocietyMember")
                            _Entity.HousingSocietyMember = fileName;
                        else
                            _Entity.AuditReport = fileName;
                    }
                }
            }
            catch { }

            _Entity.CreatedBy = int.Parse(_LoggedInUser.UserId.ToString());
            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
            {
                if (Request.Cookies["SSOROLE"].Value == "CITIZEN")
                {
                    _Entity.UserType = "C";
                }
                else
                {
                    _Entity.UserType = "D";
                }
            }
            _Entity.UserType = "D";

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            int ReturnVal = 0;


            ReturnVal = Convert.ToInt32(objSocietyModel.SaveHousingSocietyAudit(_Entity));

            return ReturnVal;
        }


        [HttpPost]
        [ActionName("VerifyAadhaarOTPForEsingAuditOrder")]
        public string VerifyAadhaarOTPForEsingAuditOrder(string OTP, string TXN, string AuditerId = "", string AuditBy = "", string Ids = "", string Status = "", string IsFirm = "", string IsRequested = "", int DistrictId = 0, int FinancialYearId = 0, string FinancialYearName = "", string AuditTillDate = "", string IsForced = "0", string AuditByDepartmentorCA = "0")//By Empanelled "true"    By Department  "false"
        {
            string message = string.Empty;
            string succmsg = "Some Error";
            if (TXN != "")
            {
                try
                {
                    ResultMessage ObjRM = new ResultMessage();
                    ObjRM.data = "";
                    ObjRM = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                    if (ObjRM.status == "1")
                    {
                        if (IsRequested == "0")
                            succmsg = saveAuditAssignbydeprtNotRequest(AuditerId, AuditBy, Ids, Status, IsFirm, ObjRM.data, FinancialYearId, FinancialYearName.Replace(" ", ""), AuditTillDate, IsForced, AuditByDepartmentorCA);
                        if (IsRequested == "1")
                            succmsg = saveAuditAssignbydeprt(AuditerId, AuditBy, Ids, Status, IsFirm, ObjRM.data, AuditTillDate);
                    }
                }
                catch (Exception ex)
                {
                    succmsg = ex.Message;
                }
            }
            return succmsg;
        }









        [HttpPost]
        public string EsingAuditReOrder(string OldOrderNo, string TXN, string AuditerId = "", string AuditBy = "", string Ids = "", string AssignedIds = "", string Status = "", string IsFirm = "", string IsRequested = "", int DistrictId = 0, int FinancialYearId = 0, string FinancialYearName = "", string AuditTillDate = "", string IsForced = "0", string AuditByDepartmentorCA = "0", string DeleteResions = "")//By Empanelled "true"    By Department  "false"
        {
            string message = string.Empty;
            string succmsg = "Some Error";
            if (TXN != "")
            {
                try
                {
                    if (IsRequested == "0")
                        succmsg = saveAuditAssignbydeprtNotRequestReOrder(OldOrderNo, AuditerId, AuditBy, Ids, Status, IsFirm, TXN, FinancialYearId, FinancialYearName.Replace(" ", ""), AuditTillDate, IsForced, AuditByDepartmentorCA, DeleteResions, AssignedIds);
                    if (IsRequested == "1")
                        succmsg = saveAuditAssignbydeprtReOrder(OldOrderNo, AuditerId, AuditBy, Ids, Status, IsFirm, TXN, AuditTillDate);
                }
                catch (Exception ex)
                {
                    succmsg = ex.Message;
                }
            }
            return succmsg;
        }

        [HttpPost]
        [ActionName("saveAuditAssignbydeprt")]
        public string saveAuditAssignbydeprtReOrder(string OldOrderNo, string AuditerId = "", string AuditBy = "", string Ids = "", string Status = "", string IsFirm = "", string Tran = "", string AuditTillDate = "", string DeleteResions = "", string AssignedIds = "")
        {
            string ResultIntStr = "Error";
            string OrderNo = "";
            AuditorController objAC = new AuditorController();
            AuditOrderEntity objAE = new AuditOrderEntity();

            try
            {
                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                DataTable Dt = null;
                var IdsArr = Ids.Split(',');

                string DistName = "";
                string DistNameHi = "";

                string saveIDs = "";
                if (_LoggedInUser.LevelId == 3)
                {
                    DistName = _LoggedInUser.DISTRICT_ENG;
                    DistNameHi = _LoggedInUser.DISTRICT_MANGAL;
                }
                else if (_LoggedInUser.LevelId == 2)
                {
                    DistName = _LoggedInUser.DIVISION_ENG;
                    DistNameHi = _LoggedInUser.DIVISION_MANGAL;
                }
                else if (_LoggedInUser.LevelId == 1)
                {
                    DistName = "Rajasthan";
                }

                #region 

                objAE.FileName = "";
                objAE.SocietyCount = 1;
                objAE.InActiveFlag = 0;
                OrderNo = objAC.SaveAuditOrder(objAE);
                objAE.OrderNo = OrderNo;
                #endregion

                string OfficerName = "";
                string CurrentFYear = "";
                string SocietyName = "";

                string fontSize = "font-size: 13px;";



                Dt = objSocietyModel.GetAuditorAndSocietyDetailByIds(Ids, AuditerId);



                StringBuilder sb = new StringBuilder();
                sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:10px'>");
                sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px; '>");
                sb.Append("<tr><td colspan=2 style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                sb.Append("<img style='width:100px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=2 style='text-align:center;" + fontSize + "'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");

                if (_LoggedInUser.LevelId == 3)
                    sb.Append("<tr><td colspan=2 style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय- विशेष लेखा परीक्षक सहकारी समितियॉं, DistrictNameHi") + "</label></b></td></tr>");
                else if (_LoggedInUser.LevelId == 2)
                    sb.Append("<tr><td colspan=2 style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय- रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड , DistrictNameHi") + "</label></b></td></tr>");
                else if (_LoggedInUser.LevelId == 1)
                    sb.Append("<tr><td colspan=2 style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय- रजिस्ट्रार, सहकारी समितियॉं राजस्थान ,जयपुर") + "</label></b></td></tr>");

                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                sb.Append("<tr><td align='left' style='margin-top:15px;'> क्रमांक फा. " + OrderNo + " </td><td align='right'> दिनांक " + DateTime.Now.ToString("dd/MM/yyyy") + "</td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><u><b> ऑडिट आवंटन आदेश </b></u></td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><b> (रा.स.सो.अधि.धारा 54(2) व नियम 73(4) के तहत) </b></td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                sb.Append("<tr><td align='justify'  colspan=2 style='margin-top:15px;'> <p>&nbsp;&nbsp;&nbsp; राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(4) के अनुसार निम्नलिखित सहकारी समितियों द्वारा विभागीय निरीक्षक से वर्ष FinYear की लेखा परीक्षा करवाने की अभ्यर्थना प्रस्तुत की है | </p></td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");

                if (_LoggedInUser.LevelId == 3)
                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, विशेष लेखा परीक्षक सहकारी समितियॉ DistrictNameHi , राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(4) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                else if (_LoggedInUser.LevelId == 2)
                {
                    if (AuditerId.ToString().Contains(","))
                    {
                        sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(4) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए SocietyName की वर्ष FinYear की लेखा परीक्षा हेतु निम्न लिखित लेखा निरीक्षकों की नियुक्ति करता / करती हूँ | </p></td></tr>");
                    }
                    else
                    {
                        sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(4) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                    }
                }
                else if (_LoggedInUser.LevelId == 1)
                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, संयुक्त मुख्य अंकेक्षक ( जनरल ) सहकारी समितियॉं, राजस्थान जयपुर, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(4) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");

                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                if (AuditerId.ToString().Contains(","))
                {
                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; उपरोक्त समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी |</p></td></tr>");
                }
                else
                {
                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; निम्न समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी |</p></td></tr>");
                }
                sb.Append("<tr><td align='center'  colspan=2 >");
                sb.Append("<table style='width: 100%; margin-top:20px;' border='1' cellspacing='0' cellpadding='5'>");
                if (AuditerId.ToString().Contains(","))
                {
                    sb.Append("<tr><td align='center'style='width:70px;'> क्र. स. </td><td align='center'> लेखा निरीक्षक </td><td align='center'> कार्यालय </td></tr>");
                }
                else
                {
                    sb.Append("<tr><td align='center'style='width:70px;'> क्र. स. </td><td align='center'> सहकारी समिति  </td><td align='center'> स्थान </td></tr>");/*<td align='center'>Officer/Firm</td>*/
                }
                int i1 = 0;
                foreach (DataRow dr in Dt.Rows)
                {
                    i1++;
                    sb.Append("<tr><td  align='left'  > " + i1.ToString() + "</td>");
                    if (AuditerId.ToString().Contains(","))
                    {
                        sb.Append("<td  align='left'  > " + dr["OfficerNameEn"].ToString() + " </td>");
                        sb.Append("<td  align='left'  > " + dr["OfficeNameEn"].ToString() + " </td></tr>");
                    }
                    else
                    {
                        sb.Append("<td  align='left'  > " + dr["SocietyName"].ToString() + " (" + dr["RegistrationNo"].ToString() + ") </td>");
                        sb.Append("<td  align='left'  > " + (dr["SocietyPlaceEn"].ToString() != "" ? (dr["SocietyPlaceEn"].ToString() + " , ") : " ") + dr["DISTRICT_ENG"].ToString() + "</td></tr>");
                    }
                    OfficerName = dr["OfficerNameEn"].ToString() != "" ? dr["OfficerNameEn"].ToString() : dr["FirmName"].ToString();
                    CurrentFYear = dr["CurrentFYear"].ToString();
                    saveIDs = saveIDs + dr["Id"].ToString() + ",";

                    SocietyName = dr["SocietyName"].ToString().ToUpper();


                }
                sb.Append("</table></td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                sb.Append("</table ></div>");


                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                string fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + ".pdf";
                string textVal = sb.ToString().Replace("<br>", "<br/>").Replace("DistrictNameHi", DistNameHi).Replace("SocietyName", SocietyName).Replace("FinYear", CurrentFYear).Replace("InspectorName", OfficerName).Replace("RegitrarName", _LoggedInUser.OfficerNameEn);
                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(textVal));

                string FilePath = "../Content/AuditReport/" + fileName;
                if (COMMON_FUNCTION.GeneratePDF(sb1, Server.MapPath(FilePath)))
                {
                    ResultMessage objRM = new ResultMessage();
                    objRM = COMMON_FUNCTION.EsinghPDF(Tran, Server.MapPath(FilePath), DistName);

                    if (objRM.data == "Success")
                    {
                        if (saveIDs.Length > 0)
                            saveIDs = saveIDs.Substring(0, saveIDs.Length - 1);

                        ResultIntStr = objSocietyModel.saveAuditAssignbydeprt(AuditerId, AuditBy, saveIDs, Status, IsFirm, FilePath, _LoggedInUser.UserRoleDeptMappingId, OrderNo).ToString();
                        if (ResultIntStr != "-1")
                            ResultIntStr = "Success";
                        else
                        {
                            objAE.InActiveFlag = 1;
                            objAC.SaveAuditOrder(objAE);
                        }
                    }
                }

            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Society", FunctionName = " saveAuditAssignbydeprtReOrder(string OldOrderNo, string AuditerId = \"\", string AuditBy = \"\", string Ids = \"\", string Status = \"\", string IsFirm = \"\", string Tran = \"\", string AuditTillDate = \"\", string DeleteResions = \"\", string AssignedIds = \"\")" }, ex);

            }
            if (ResultIntStr == "Error")
            {
                objAE.InActiveFlag = 1;
                objAC.SaveAuditOrder(objAE);
            }
            return (ResultIntStr);


        }



        [HttpPost]
        public string saveAuditAssignbydeprtNotRequestReOrder(string OldOrderNo, string AuditerId = "", string AuditBy = "", string Ids = "", string Status = "", string IsFirm = "", string Tran = "", int FinancialYearId = 0, string FinancialYearName = "", string AuditTillDate = "", string IsForced = "False", string AuditByDepartmentorCA = "", string DeleteResions = "", string AssignedIds = "")
        {
            string ResultIntStr = "Error";


            AuditorController objAC = new AuditorController();
            AuditOrderEntity objAE = new AuditOrderEntity();
            try
            {
                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());

                var IdsArr = Ids.Split(',');

                string DistName = "";
                string DistNameHi = "";
                string Location = "";
                string fontSize = "font-size: 13px;";
                string saveIDs = "";
                DataTable Dt = null;

                if (_LoggedInUser.LevelId == 3)
                {
                    DistName = _LoggedInUser.DISTRICT_ENG;
                    DistNameHi = _LoggedInUser.DISTRICT_MANGAL;
                    Location = "जिले";
                }
                else if (_LoggedInUser.LevelId == 2)
                {
                    DistName = _LoggedInUser.DIVISION_ENG;
                    DistNameHi = _LoggedInUser.DIVISION_MANGAL;
                    Location = "खण्ड";
                }
                else if (_LoggedInUser.LevelId == 1)
                {
                    Location = "राज्य";
                }


                string OfficerName = "";
                string CurrentFYear = "";
                string SocietyName = "";
                string OrderNo = "";
                //if (AuditerId.ToString().Contains(","))
                //{
                //Dt = objSocietyModel.GetAuditorsForMultipalAssigment(AuditerId);
                //}
                //else
                {
                    int AuditByAD = (AuditByDepartmentorCA == "true" ? 1 : 2);
                    Dt = objSocietyModel.GetAuditorAndSocietyDetailByIds(Ids, AuditerId, AuditByAD, FinancialYearId);
                }


                #region 

                objAE.FileName = "";
                objAE.SocietyCount = Dt.Rows.Count;
                objAE.SocietyCount = Dt.AsEnumerable().GroupBy(row => row["SocietyName"]).Count();

                objAE.InActiveFlag = 0;
                OrderNo = objAC.SaveAuditOrder(objAE);
                objAE.OrderNo = OrderNo;
                #endregion
                if (OrderNo != "ERROR")
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:10px'>");
                    sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px; '>");
                    sb.Append("<tr><td colspan=2 style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                    sb.Append("<img style='width:100px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td colspan=2 style='text-align:center;" + fontSize + "'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                    if (_LoggedInUser.LevelId == 3)
                        sb.Append("<tr><td colspan=2 style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय- विशेष लेखा परीक्षक सहकारी समितियॉं, DistrictNameHi") + "</label></b></td></tr>");
                    else if (_LoggedInUser.LevelId == 2)
                        sb.Append("<tr><td colspan=2 style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय- रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड , DistrictNameHi") + "</label></b></td></tr>");
                    else if (_LoggedInUser.LevelId == 1)
                        sb.Append("<tr><td colspan=2 style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय- रजिस्ट्रार, सहकारी समितियॉं राजस्थान ,जयपुर") + "</label></b></td></tr>");

                    sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");

                    sb.Append("<tr><td align='left' style='margin-top:15px;'> क्रमांक फा." + OrderNo + " </td><td align='right'> दिनांक " + DateTime.Now.ToString("dd/MM/yyyy") + "</td></tr>");
                    sb.Append("<tr><td align='Left'  colspan=2 > संदर्भ :- " + OldOrderNo + " </td></tr>");
                    sb.Append("<tr><td align='Left'  colspan=2 >&nbsp; </td></tr>");
                    sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><u><b> ऑडिट आवंटन आदेश (संसोधित)</b></u></td></tr>");
                    if (IsForced == "True")
                    {
                        sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><b> (रा.स.सो.अधि.धारा 54(2) व नियम 73(8) के तहत) </b></td></tr>");
                        sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><b> (सोसाइटियों के वर्ग / सोसायटी हेतु) </b></td></tr>");
                    }
                    else if (FinancialYearId > 48)//Current
                    {
                        sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><b> (रा.स.सो.अधि.धारा 54(2) व नियम 73(7) के तहत) </b></td></tr>");
                    }
                    else if (FinancialYearId < 49)//FOR BACK LOG
                    {
                        sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><b> (रा.स.सो.अधि.धारा 54(2) व नियम 73(8) के तहत) </b></td></tr>");
                    }

                    sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");

                    if (IsForced == "True")
                    {
                        sb.Append("<tr><td align='justify'  colspan=2 style='margin-top:15px;'> <p>&nbsp;&nbsp;&nbsp; राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 (2) तथा राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(8) अनुसार नियम 73(4) से 73(7) में अंतर्विष्ट किसी बात के होने पर भी, रजिस्ट्रार आदेश द्वारा, किसी विशिस्ट कालावधि के लिए किसी सोसायटी या सोसाइटियों के किसी वर्ग के लेखाओं की लेखापरीक्षा कराने के लिए लेखापरीक्षक (लेखापरीक्षको) या लेखा परीक्षा फर्म (फर्मो ) को नियुक्त कर सकेगा | अधोहस्ताक्षरकर्ता को यह समाधान हो गया है , कि निम्नलिखित सहकारी समितियों की लेखा परीक्षा हेतु उक्त प्रावधानों के अंतर्गत विभाग स्तर से लेखा परीक्षक नियुक्त किया जाना आवश्यक है ' । </p></td></tr>");
                        sb.Append("<tr><td align='center'   colspan=2 >&nbsp; </td></tr>");
                        if (_LoggedInUser.LevelId == 3)
                            sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName विशेष लेखा परीक्षक सहकारी समितियॉ DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) तथा राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73 (8) के तहत प्रदत्त शक्तियों का प्रयोग करते हुए निम्नलिखित सहकारी समितियों के वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + " InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + "  को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                        else if (_LoggedInUser.LevelId == 2)
                        {
                            if (AuditerId.ToString().Contains(","))
                            {
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(8) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए SocietyName की वर्ष FinYear की लेखा परीक्षा हेतु निम्न लिखित लेखा निरीक्षकों की नियुक्ति करता / करती हूँ | </p></td></tr>");
                            }
                            else
                            {
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) तथा राजस्थान सहकारी सोसाइटी नियम 2003 के नियम 73(8) के तहत प्रदत्त शक्तियों का प्रयोग करते हुए निम्न सहकारी समितियों के वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + " InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + "  को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                            }

                        }

                        else if (_LoggedInUser.LevelId == 1)
                            sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, संयुक्त मुख्य अंकेक्षक ( जनरल ) सहकारी समितियॉं, राजस्थान जयपुर, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) तथा राजस्थान सहकारी सोसाइटी नियम 2003 के नियम 73 (8) के तहत प्रदत्त शक्तियों का प्रयोग करते हुए निम्न सहकारी समितियों के वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + " InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + "  को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");

                        sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");

                        if (AuditerId.ToString().Contains(","))
                        {
                            sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; उपरोक्त सहकारी समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी |</p></td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; निम्न सहकारी समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54  एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी |</p></td></tr>");
                        }


                    }
                    else
                    {
                        if (FinancialYearId > 48)
                        {
                            sb.Append("<tr><td align='justify'  colspan=2 style='margin-top:15px;'> <p>&nbsp;&nbsp;&nbsp; राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(4) के अनुसार निम्नलिखित सहकारी समितियॉं नियत समय में लेखा परीक्षक की नियुक्ति कर सुचना प्रस्तुत करने में विफल रही है ।  </p></td></tr>");
                            sb.Append("<tr><td align='center'   colspan=2 >&nbsp; </td></tr>");
                            if (_LoggedInUser.LevelId == 3)
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, विशेष लेखा परीक्षक सहकारी समितियॉ DistrictNameHi , राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(7) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                            else if (_LoggedInUser.LevelId == 2)
                            {
                                if (AuditerId.ToString().Contains(","))
                                {
                                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(7) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए SocietyName की वर्ष FinYear की लेखा परीक्षा हेतु निम्न लिखित लेखा निरीक्षकों की नियुक्ति करता / करती हूँ | </p></td></tr>");
                                }
                                else
                                {
                                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(7) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                                }
                            }
                            else if (_LoggedInUser.LevelId == 1)
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, संयुक्त मुख्य अंकेक्षक ( जनरल ) सहकारी समितियॉं, राजस्थान जयपुर, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(7) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");

                            sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");

                            if (AuditerId.ToString().Contains(","))
                            {
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; उपरोक्त सहकारी समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी |</p></td></tr>");
                            }
                            else
                            {
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; निम्न सहकारी समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी |</p></td></tr>");
                            }


                        }
                        else  //FOR BACK LOG
                        {
                            sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(3) के अनुसार प्रत्येक सोसाइटी के लेखाओ की लेखा परीक्षा उस वित्तीय वर्ष की जिससे ऐसे लेखे सम्बंधित है ,समाप्ति के छ माह के भीतर-भीतर की जाएगी | परन्तु  निम्नलिखित सहकारी समितियों द्वारा वर्ष " + FinancialYearName + " तक की ऑडिट करवाकर ऑडिट रिपोर्ट प्रस्तुत नहीं की गयी है | जिससे स्पष्ट है कि निम्नलिखित सहकारी समितियां अधिनियम के उक्त प्रावधानों की पालना करने में विफल रही है इसलिए इन सहकारी समितियों की वर्ष " + FinancialYearName + " तक बकाया समस्त लेखा परीक्षा हेतु विभाग स्तर से लेखा परीक्षक की नियुक्ति किया जाना आवश्यक हो गया है | </p></td></tr>");
                            sb.Append("<tr><td align='center'   colspan=2 >&nbsp; </td></tr>");

                            if (_LoggedInUser.LevelId == 3)
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, विशेष लेखा परीक्षक सहकारी समितियॉ DistrictNameHi , राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(8) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष " + FinancialYearName + " की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                            else if (_LoggedInUser.LevelId == 2)
                            {
                                if (AuditerId.ToString().Contains(","))
                                {
                                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(8) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए SocietyName की वर्ष FinYear की लेखा परीक्षा हेतु निम्न लिखित लेखा निरीक्षकों की नियुक्ति करता / करती हूँ | </p></td></tr>");
                                }
                                else
                                {
                                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(8) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष " + FinancialYearName + " की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                                }
                            }

                            else if (_LoggedInUser.LevelId == 1)
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, संयुक्त मुख्य अंकेक्षक ( जनरल ) सहकारी समितियॉं, राजस्थान जयपुर, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(8) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष " + FinancialYearName + " की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");


                            sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                            if (AuditerId.ToString().Contains(","))
                            {
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; उपरोक्त सहकारी समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी तथा लेखापरीक्षकगण आवंटीत समिति में अविलम्ब प्रारंभ कर दिनांक " + AuditTillDate + " तक अंकेक्षण कार्य सम्पादित करेंगे |</p></td></tr>");
                            }
                            else
                            {
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; निम्न सहकारी समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी तथा लेखापरीक्षकगण आवंटीत समिति में अविलम्ब प्रारंभ कर दिनांक " + AuditTillDate + " तक अंकेक्षण कार्य सम्पादित करेंगे |</p></td></tr>");
                            }


                        }
                    }

                    sb.Append("<tr><td align='center'  colspan=2 >");
                    sb.Append("<table style='width: 100%; margin-top:20px;' border='1' cellspacing='0' cellpadding='5'>");

                    if (AuditerId.ToString().Contains(","))
                    {
                        sb.Append("<tr><td align='center'style='width:70px;'> क्र. स. </td><td align='center'> लेखा निरीक्षक </td><td align='center'> सहकारी समिति  </td><td align='center'> कार्यालय </td></tr>");
                    }
                    else
                    {
                        sb.Append("<tr><td align='center'style='width:70px;'> क्र. स. </td><td align='center'> सहकारी समिति  </td><td align='center'> स्थान </td></tr>");
                    }


                    int i1 = 0;
                    foreach (DataRow dr in Dt.Rows)
                    {
                        i1++;
                        sb.Append("<tr><td  align='left'  > " + i1.ToString() + "</td>");

                        if (AuditerId.ToString().Contains(","))
                        {
                            sb.Append("<td  align='left'  > " + dr["OfficerNameEn"].ToString() + " </td>");
                            sb.Append("<td  align='left'  > " + dr["SocietyName"].ToString() + " (" + dr["RegistrationNo"].ToString() + ") </td>");
                            sb.Append("<td  align='left'  > " + dr["OfficeNameEn"].ToString() + " </td></tr>");
                        }
                        else
                        {
                            sb.Append("<td  align='left'  > " + dr["SocietyName"].ToString() + " (" + dr["RegistrationNo"].ToString() + ") </td>");
                            sb.Append("<td  align='left'  > " + (dr["SocietyPlaceEn"].ToString() != "" ? (dr["SocietyPlaceEn"].ToString() + " , ") : " ") + dr["DISTRICT_ENG"].ToString() + "</td></tr>");
                        }
                        if (AuditerId.ToString().Contains(","))
                        {
                            if (!OfficerName.Contains(dr["OfficerNameEn"].ToString()))
                                OfficerName = AuditBy == "1" ? dr["FirmName"].ToString() : OfficerName + ", " + dr["OfficerNameEn"].ToString();
                        }
                        else
                            OfficerName = AuditBy == "1" ? dr["FirmName"].ToString() : dr["OfficerNameEn"].ToString();
                        CurrentFYear = dr["CurrentFYear"].ToString() == "" ? FinancialYearName : dr["CurrentFYear"].ToString();
                        saveIDs = saveIDs + dr["Id"].ToString() + ",";
                        SocietyName = dr["SocietyName"].ToString().ToUpper();
                    }
                    if (OfficerName.Contains(", ") && OfficerName.StartsWith(","))
                    {
                        OfficerName = OfficerName.Substring(1);
                    }
                    sb.Append("</table></td></tr>");
                    sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                    sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                    sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                    sb.Append("</table ></div>");

                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    string fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + ".pdf";
                    string textVal = sb.ToString().Replace("<br>", "<br/>").Replace("DistrictNameHi", DistNameHi).Replace("SocietyName", SocietyName).Replace("DistrictNameHi", DistNameHi).Replace("FinYear", CurrentFYear).Replace("InspectorName", OfficerName).Replace("RegitrarName", _LoggedInUser.OfficerNameEn);
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(textVal));
                    string FilePath = "../Content/AuditReport/" + fileName;

                    if (COMMON_FUNCTION.GeneratePDF(sb1, Server.MapPath(FilePath)))
                    {
                        ResultMessage objRM = new ResultMessage();
                        objRM = COMMON_FUNCTION.EsinghPDF(Tran, Server.MapPath(FilePath), DistName);
                        if (objRM.data == "Success")
                        {
                            if (Ids.Length > 0)
                                saveIDs = saveIDs.Substring(0, saveIDs.Length - 1);

                            ResultIntStr = objSocietyModel.saveAuditAssignbydeprtNotRequestReOrder(AuditerId, AuditBy, Ids, AssignedIds, Status, IsFirm, _LoggedInUser.UserId, FilePath, _LoggedInUser.UserRoleDeptMappingId, AuditTillDate, IsForced, _LoggedInUser.SSOID, FinancialYearId, OrderNo, DeleteResions).ToString();
                            if (ResultIntStr != "-1" && ResultIntStr != "-2")
                                ResultIntStr = "Success";
                            else
                            {
                                objAE.InActiveFlag = 1;
                                objAC.SaveAuditOrder(objAE);
                            }
                        }
                    }
                    else
                    {
                        objAE.InActiveFlag = 1;
                        objAC.SaveAuditOrder(objAE);
                    }
                }

            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Society", FunctionName = "saveAuditAssignbydeprtNotRequestReOrder(string OldOrderNo, string AuditerId = \"\", string AuditBy = \"\", string Ids = \"\", string Status = \"\", string IsFirm = \"\", string Tran = \"\", int FinancialYearId = 0, string FinancialYearName = \"\", string AuditTillDate = \"\", string IsForced = \"False\", string AuditByDepartmentorCA = \"\", string DeleteResions = \"\", string Assigne" }, ex);

            }
            if (ResultIntStr == "Error")
            {
                objAE.InActiveFlag = 1;
                objAC.SaveAuditOrder(objAE);
            }
            return ResultIntStr;
        }







        [HttpPost]
        [ActionName("RejectAssignAuditorRequest")]
        public string RejectAssignAuditorRequest(string Id = "", string DeleteResion = "")
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            objSocietyModel.RejectAuditorRequest(Id, DeleteResion, _LoggedInUser.SSOID).ToString();
            return "";
        }

        [HttpGet]
        [ActionName("GenerateAadhaarOTPForEsingAuditOrder")]
        public string GenerateAadhaarOTPForEsing()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser != null)
            {
                if (Request.Url.Host.Contains("localhost"))
                {
                    _LoggedInUser.AadharNo = "444394749686517";
                    return "999999";
                }
                return COMMON_FUNCTION.GenerateOTPForEsingh(_LoggedInUser.AadharNo);
            }
            else
            {
                return "LOGOUT";
            }
            //return "RJVSD2343246566";
        }

        [HttpPost]
        public string ApexBankSocietyDetailList()
        {
            SocietyModel objSocietyModel = new SocietyModel(new FileLogger());
            DataTable DT = objSocietyModel.ApexBankSocietyDetailList();
            return COMMON_FUNCTION.DataTableToJSONString(DT);

        }
        [HttpGet]
        public ActionResult ApexBankSocietyDetail()
        {
            return View();
        }
        [HttpPost]
        public string RajfedSocietyDetailList()
        {
            SocietyModel objSocietyModel = new SocietyModel(new FileLogger());
            DataTable DT = objSocietyModel.RajfedSocietyDetailList();
            return COMMON_FUNCTION.DataTableToJSONString(DT);

        }

        [HttpGet]
        public ActionResult RajfedSocietyDetail()
        {
            return View();
        }

        [HttpGet]
        public string SendOtpToAadhar()
        {
            return COMMON_FUNCTION.SSO_AADHAR_OTP_VALIDATION(_LoggedInUser.SSOID);
        }

        [HttpPost]
        public string VlidateOTPOfAadhar(CCSStatus ItemCCSStatus)
        {
            //SSOWSSoap.SSOUserDetail usrdtl = new SSOWSSoap.SSOUserDetail();
            //SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
            //usrdtl = ssoclient.GetUserDetailXML(_LoggedInUser.SSOID, "RAJSAHKAR.TEST", "Rajasthan@12");
            string _txnid = string.Empty;
            DataTable dtb = new DataTable();

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser != null)
            {
                dtb = COMMON_FUNCTION.ValidateOTP(ItemCCSStatus.TXN, _LoggedInUser.AadharNo, ItemCCSStatus.OTP);
                if (dtb.Rows.Count > 0)
                {
                    if (dtb.Rows[0]["Column1"].ToString() == "Success")
                    {
                        try
                        {
                            if (Request.Files != null && Request.Files.Count > 0) //&& Request.Files[0].ToString() == "AuditReport"
                            {
                                string fileName = string.Empty;
                                string pathString = "~/Content/SocietyProfiling/";
                                string FileList = string.Empty;
                                string ResolutionPDF = "";
                                string AuditorAssignFilePath = "";
                                pathString += Regex.Replace(ItemCCSStatus.SocietyRegiNo, "[^a-zA-Z0-9_]+", "");
                                if (!System.IO.File.Exists(pathString))
                                {
                                    System.IO.Directory.CreateDirectory(Server.MapPath(pathString));
                                }
                                foreach (string files in Request.Files)
                                {
                                    HttpPostedFileBase file = Request.Files[files];
                                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                                    file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));
                                    ItemCCSStatus.RejectFile = fileName;
                                }
                            }
                        }
                        catch { }
                        SocietyModel obj = new SocietyModel();
                        string str = "-1";
                        if (ItemCCSStatus.StatusId == "3")
                        {
                            str = obj.UpdateCCSocietyliquidator(ItemCCSStatus.SocietyIDEnc, ItemCCSStatus.liquidatorName, ItemCCSStatus.Statusdate, ItemCCSStatus.liquidatorPostName, ItemCCSStatus.liquidatorContactNo, _loggedInUSerDetails.UserId, ItemCCSStatus.liquidatorSSOID);
                        }
                        else if (ItemCCSStatus.StatusId == "4")
                        {
                            str = obj.UpdateCCSocietyReject(ItemCCSStatus.SocietyIDEnc, ItemCCSStatus.Statusdate, ItemCCSStatus.RejectFile, _loggedInUSerDetails.UserId);
                        }
                        else
                        {
                            str = obj.UpdateCCSociety(ItemCCSStatus.SocietyIDEnc, ItemCCSStatus.StatusId);
                        }
                        return "Success";
                    }
                    else
                        return "Failed";
                }
                else
                    return "Failed";
            }
            else
                return "Failed";
        }

        [HttpPost]
        public string UpdateCCSStatus(CCSStatus ItemCCSStatus)
        {
            try
            {
                if (Request.Files != null && Request.Files.Count > 0) //&& Request.Files[0].ToString() == "AuditReport"
                {
                    string fileName = string.Empty;
                    string pathString = "~/Content/SocietyProfiling/";
                    string FileList = string.Empty;
                    string ResolutionPDF = "";
                    string AuditorAssignFilePath = "";
                    pathString += Regex.Replace(ItemCCSStatus.SocietyRegiNo, "[^a-zA-Z0-9_]+", "");
                    if (!System.IO.File.Exists(pathString))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(pathString));
                    }
                    foreach (string files in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[files];
                        fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                        file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));
                        ItemCCSStatus.RejectFile = fileName;
                    }
                }
            }
            catch { }
            SocietyModel obj = new SocietyModel();
            string str = "-1";
            if (ItemCCSStatus.StatusId == "3")
            {
                str = obj.UpdateCCSocietyliquidator(ItemCCSStatus.SocietyIDEnc, ItemCCSStatus.liquidatorName, ItemCCSStatus.Statusdate, ItemCCSStatus.liquidatorPostName, ItemCCSStatus.liquidatorContactNo, _loggedInUSerDetails.UserId, ItemCCSStatus.liquidatorSSOID);
            }
            else if (ItemCCSStatus.StatusId == "4")
            {
                str = obj.UpdateCCSocietyReject(ItemCCSStatus.SocietyIDEnc, ItemCCSStatus.Statusdate, ItemCCSStatus.RejectFile, _loggedInUSerDetails.UserId);
            }
            else
            {
                str = obj.UpdateCCSociety(ItemCCSStatus.SocietyIDEnc, ItemCCSStatus.StatusId);
            }
            return "Success";

        }


        [HttpPost]
        public string GetCCSocietyList(int SearchType = 1, int DistrictId = 0)
        {
            CitizenModel objCModel = new CitizenModel(new FileLogger());
            DataTable DT = objCModel.GetCreditCooperativeSocietiesGreviyans(DistrictId.ToString());
            return COMMON_FUNCTION.DataTableToJSONString(DT);

        }

        [HttpPost]
        [ActionName("InsertSocietyAmendment")]
        public string InsertSocietyAmendment(SocietyAmendment SocietyAmendmentData)
        {
            DataTable memberData = new DataTable();
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            try
            {
                if (Request.Form["lstSocietyWorkScope"] != null)
                {
                    SocietyAmendmentData.lstSocietyWorkScope = Request.Form["lstSocietyWorkScope"].ToString().Equals("[]") ? new List<SocietyWorkScopeEntity>() : (List<SocietyWorkScopeEntity>)s.Deserialize(Request.Form["lstSocietyWorkScope"].ToString(), typeof(List<SocietyWorkScopeEntity>));
                }

            }
            catch (Exception ex)
            {
                return CommonMessageFactory.Response("Provided input data is not in correct format.");
            }

            List<AdministrativeData> objData = new List<AdministrativeData>();
            try
            {
                if (Request.Form["AdministrativeDataList"] != null)
                {
                    SocietyAmendmentData.AdministrativeDataList = Request.Form["AdministrativeDataList"].ToString().Equals("[]") ? new List<AdministrativeData>() : (List<AdministrativeData>)s.Deserialize(Request.Form["AdministrativeDataList"].ToString(), typeof(List<AdministrativeData>));
                }
            }
            catch (Exception ex)
            {
                return CommonMessageFactory.Response("Provided input data is not in correct format.");
            }


            return "";
        }



        //#region Credit Cooperative Society Complaint 


        //public ActionResult CCSComplaint()
        //{
        //    //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        //    ViewBag.LoggedInUserDistrictId = _LoggedInUser.DistrictId;
        //    return View();
        //}


        //[HttpPost]
        //public JsonResult GetCCSComplaintList(DataPagingModel Details)
        //{
        //    ViewBag.LoggedInUserDistrictId = _LoggedInUser.DistrictId;
        //    SocietyModel objNew = new SocietyModel(new DBLogger());
        //    int DistrictId = _LoggedInUser.DistrictId;
        //    DataTable DT = objNew.GetCCSComplaint(ref Details, DistrictId);
        //    return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

        //}

        //[HttpPost]
        //public string GenerateCCSLetter(string OTP, string TXN, CCSLetter LetterDetails)
        //{

        //    var otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
        //    if (otpResult.status != "1")
        //    {
        //        return otpResult.data;
        //    }



        //    string result = "";

        //    SocietyModel objNew = new SocietyModel(new DBLogger());
        //    string uniqeId = Guid.NewGuid().ToString();
        //    try
        //    {


        //        string RO = "";

        //        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        //        DataPagingModel Details = new DataPagingModel();
        //        Details.PageNumber = 1;
        //        Details.PageSize = 10;
        //        Details.SearchParameter = new Dictionary<string, string>();
        //        Details.SearchParameter.Add("ComplaintId", LetterDetails.ComplaintId);
        //        int DistrictId = _LoggedInUser.DistrictId;
        //        DataTable List = objNew.GetCCSComplaint(ref Details, DistrictId);

        //        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //        string filepath = Server.MapPath("~/Content/CCSLetter");
        //        string fontSize = "font-size: 15px;";

        //        if (!System.IO.Directory.Exists(filepath))
        //        {
        //            System.IO.Directory.CreateDirectory(filepath);
        //        }

        //        #region Complainant Letter
        //        bool isGeneratePDF = false;
        //        System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
        //        foreach (var items in LetterDetails.SocietyDetails)
        //        {
        //            sb = new StringBuilder();
        //            sb1 = new StringBuilder();
        //            string FileNameComplainant = "ccsc_" + items.ComplaintId;
        //            sb.Append("<div style='padding-left:50px;padding-right:50px;padding-top:20px'>");
        //            sb.Append("<table border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");

        //            sb.Append("<tr><td colspan='5' style='padding: 15px 15px 0px 15px; text-align: left; height: 109px'>");
        //            sb.Append("<img style='width:70px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");                   
        //            sb.Append("</td>");
        //            sb.Append("<td colspan='5' style='padding: 15px 15px 0px 15px; text-align: right; height: 109px'>");
        //            sb.Append("<img style='width:70px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");

        //            sb.Append("<td colspan='5' style='padding: 15px 15px 0px 15px; text-align: right; height: 109px'>");
        //            sb.Append("<img style='width:70px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
        //            sb.Append("</td></tr>");


        //            sb.Append("<tr><td colspan='5' style='text-align:center;font-size:32px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
        //            sb.Append("<tr><td colspan='5' style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:20px;'>" + UnicodeToKrutidev.GetKrutidev("सहकारिता विभाग") + "</label><label  style='font-size:25px;'> </label></b>");
        //            sb.Append("</td></tr>");

        //            sb.Append("<tr>");
        //            sb.Append("<td colspan='5' style='text-align:left;'> Doc No </td>");
        //            sb.Append("<td colspan='5' style='text-align:right;'> Date </td>");
        //            sb.Append("</tr>");

        //            sb.Append("<tr>");
        //            sb.Append("<td colspan='5' style='text-align:left;'>To</td>");
        //            sb.Append("</tr>");
        //            sb.Append("<tr>");
        //            sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'><br/>  " + items.ComplainerName + " , मोबाइल न. " + items.MobileNo + " </td>");                    
        //            sb.Append("</tr>");

        //            sb.Append("<tr>");
        //            sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'> " + items.Address + " </td>");
        //            sb.Append("</tr>");
        //            sb.Append("<tr>");
        //            sb.Append("<td colspan='5' style='text-align:left'> श्रीमान, </td>");
        //            sb.Append("</tr>");
        //            sb.Append("<tr>");
        //            sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'> आपने दिनांक " + List.Rows[0]["CreateDateTime"] + " को " + List.Rows[0]["SocietyName"] + " के खिलाफ राजसहकार पोर्टल पर एक शिकायत दर्ज करवाई है जिसका विवरण निम्न प्रकार है :- </td>");
        //            sb.Append("</tr>");
        //            sb.Append("<tr>");
        //            sb.Append("<td colspan='5'>");
        //            sb.Append("<table border='1' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0' >");
        //            sb.Append("<tr>");
        //            sb.Append("<th style ='border:1px solid black'> शिकायत न. </th>");
        //            sb.Append("<th style ='border:1px solid black'> के खिलाफ </th>");
        //            sb.Append("<th style ='border:1px solid black'> खाता नंबर </th>");
        //            sb.Append("<th style ='border:1px solid black;text-align:right;'> राशि </th>");
        //            sb.Append("</tr>");
        //            sb.Append("<tr>");
        //            sb.Append("<td style ='border:1px solid black'>" + List.Rows[0]["CCSId"] + " </td> ");
        //            sb.Append("<td style ='border:1px solid black'> " + List.Rows[0]["SocietyName"] + " </td> ");
        //            sb.Append("<td style ='border:1px solid black'>" + List.Rows[0]["AccountNo"] + "  </td> ");
        //            sb.Append("<td style ='border:1px solid black;text-align:right;'> " + List.Rows[0]["AmountDeposited"] + " </td>");
        //            sb.Append("</tr>");
        //            sb.Append("</table>");
        //            sb.Append("</td>");
        //            sb.Append("</tr>");

        //            sb.Append("<tr>");
        //            sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'><br/> आप दिनांक  " + LetterDetails.Date.Split(' ')[0] + " को  समय " + LetterDetails.Date.Split(' ')[1] + "" + LetterDetails.Date.Split(' ')[2] + " एवं " + LetterDetails.Place + " स्थान पर अधोहस्ताक्षरकर्ता के समक्ष सभी संबंधित दस्तावेजों की मूलप्रति के साथ उपस्थित होना सुनिश्चित करे |  </td>");
        //            sb.Append("</tr>");

        //            sb.Append("<tr>");
        //            //sb.Append("<td colspan='2' style='text-align:left;margin-left:100px;'><br/><br/><br/> दिनांक  " + DateTime.Now.ToString("dd/MM/yyyy") + "</td>");
        //            sb.Append("<td colspan='5' style='text-align:right;'> <br/><br/><br/> अधिकारी के हस्ताक्षर </td>");
        //            sb.Append("</tr>");
        //            sb.Append("</table>");
        //            sb.Append("</div>");
        //            LetterDetails.ComplaintId=LetterDetails.ComplaintId + 1;
        //           // bool isGeneratePDF = false;
        //           // System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
        //            sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "20px"));
        //            if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileNameComplainant + ".pdf", ""))
        //            {
        //                LetterDetails.complainant_letter = FileNameComplainant + ".pdf";
        //                isGeneratePDF = true;
        //            }
        //        }


        //        #endregion


        //        #region CEO Letter  
        //        string FileNameCEO = "ccsceo_" + LetterDetails.ComplaintId;
        //        if (isGeneratePDF)
        //        {
        //            isGeneratePDF = false;
        //            string FileNameComplainant = "ccsceo_" + LetterDetails.ComplaintId;
        //            sb = new System.Text.StringBuilder();
        //            sb1 = new System.Text.StringBuilder();
        //            sb.Append("<div style='padding-left:50px;padding-right:50px;padding-top:20px'>");
        //            sb.Append("<table border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");

        //            sb.Append("<tr><td colspan='5' style='padding: 15px 15px 0px 15px; text-align: left; height: 109px'>");
        //            sb.Append("<img style='width:70px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
        //            sb.Append("</td></tr>");


        //            sb.Append("<tr><td colspan='5' style='padding: 15px 15px 0px 15px; text-align: right; height: 109px'>");
        //            sb.Append("<img style='width:70px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
        //            sb.Append("</td></tr>");


        //            sb.Append("<tr><td colspan='5' style='text-align:center;font-size:32px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
        //            sb.Append("<tr><td colspan='5' style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:20px;'>" + UnicodeToKrutidev.GetKrutidev("सहकारिता विभाग") + "</label><label  style='font-size:25px;'> </label></b>");
        //            sb.Append("</td></tr>");

        //            sb.Append("<tr>");
        //            sb.Append("<td colspan='2' style='text-align:left;margin-left:100px;'><br/> Doc No : " + DateTime.Now.ToString("dd/MM/yyyy") + " </td>");
        //            sb.Append("<td colspan='3' style='text-align:right;'> Date  </td>");
        //            sb.Append("</tr>");
        //            sb.Append("<tr>");
        //            sb.Append("<td colspan='5' style='text-align:left;'>To</td>");
        //            sb.Append("</tr>");
        //            sb.Append("<tr>");
        //            sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'> <br/>   मुख्य कार्यकारी अधिकारी </td>");
        //            sb.Append("</tr>");
        //            sb.Append("<tr>");
        //            sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'> " + List.Rows[0]["SocietyName"] + " </ td >");
        //            sb.Append("</tr>");
        //            sb.Append("<tr>");
        //            sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'>" + List.Rows[0]["ScoietyAddress"] +" "+"( राजस्थान )" + "  <br/>  </ td >");
        //            sb.Append("</tr>");
        //            sb.Append("<tr>");
        //            sb.Append("<td colspan='5' style='text-align:left;'> <br/>  श्री मान, </td>");
        //            sb.Append("</tr>");
        //            sb.Append("<tr>");
        //            sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'> <br/>    राजसहकार पोर्टल पर आपके खिलाफ निम्न शिकायत प्राप्त हुई है:-  </td>");
        //            sb.Append("</tr>");
        //            sb.Append("<tr>");
        //            sb.Append("<td colspan='5'>");
        //            sb.Append("<table border='1' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0' >");
        //            sb.Append("<tr>");
        //            sb.Append("<th style ='border:1px solid black'> शिकायतकर्ता का नाम </th>");
        //            sb.Append("<th style ='border:1px solid black'> मोबाइल न . </th>");
        //            sb.Append("<th style ='border:1px solid black'> अकाउंट न. / कस्टमर आईडी </th>");
        //            sb.Append("<th style ='border:1px solid black;text-align:right;'> राशि </th>");
        //            sb.Append("</tr>");
        //            foreach (var item in LetterDetails.SocietyDetails)
        //            {
        //                sb.Append("<tr>");
        //                sb.Append("<td style ='border:1px solid black'>" + item.ComplainerName + " </td> ");
        //                sb.Append("<td style ='border:1px solid black'> " + item.MobileNo + " </td> ");
        //                sb.Append("<td style ='border:1px solid black'>" + item.AccountNo + "  </td> ");
        //                sb.Append("<td style ='border:1px solid black;text-align:right;'> " + item.AmountDeposited + " </td>");
        //                sb.Append("</tr>");
        //            }    
        //            sb.Append("</table>");
        //            sb.Append("</td>");
        //            sb.Append("</tr>");
        //            sb.Append("<tr>");
        //            sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'> उक्त शिकायतों के संबंध में आपकी समिति का  पक्ष रखने हेतु मय समित रिकॉर्ड अधोहस्ताक्षरकर्ता के समक्ष दिनांक " + LetterDetails.Date.Split(' ')[0] + " एवं समय " + LetterDetails.Date.Split(' ')[1] + " " + LetterDetails.Date.Split(' ')[2] + " एवं " + LetterDetails.Place + " स्थान पर उपस्थित होना सुनिश्चित करें ! </ td >");
        //            sb.Append("</tr>");


        //            sb.Append("<tr>");
        //            //sb.Append("<td colspan='2' style='text-align:left;margin-left:100px;'><br/><br/><br/> दिनांक : " + DateTime.Now.ToString("dd/MM/yyyy") + " </td>");
        //            sb.Append("<td colspan='3' style='text-align:right;'> <br/><br/><br/> अधिकारी के हस्ताक्षर </td>");
        //            sb.Append("</tr>");
        //            sb.Append("</table>");
        //            sb.Append("</div>");
        //           // sb1 = new System.Text.StringBuilder();
        //            sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "20px"));
        //            if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileNameCEO + ".pdf", ""))
        //            {
        //                LetterDetails.ceo_letter = FileNameCEO + ".pdf";
        //                isGeneratePDF = true;
        //            }

        //        }

        //        #endregion

        //        if (isGeneratePDF)
        //        {
        //            string FileNameComplainant = "ccsceo_" + LetterDetails.ComplaintId;
        //            string esignfiles = filepath + "/" + FileNameComplainant + ".pdf" + "#" + filepath + "/" + FileNameCEO + ".pdf";
        //            var esignResult = COMMON_FUNCTION.EsinghPDF(otpResult.data, esignfiles);
        //            result = esignResult.data;
        //            if (esignResult.status == "1")
        //            {

        //                result = objNew.InsertUpdateCCSComplaintLetter(LetterDetails);



        //                if (result != "-1")
        //                {
        //                    //send sms to Complainant
        //                    string link = (Request.Url.Host.Contains("localhost") ? "http://localhost:27466/" : "http://rajsahakarapp.rajasthan.gov.in/") + "/Content/CCSLetter/" + FileNameComplainant + "pdf";

        //                    string status = new EsanchaarController().SendSocietySMS("Regarding society complaint a letter is issued. click on " + link + " to open", List.Rows[0]["MobileNo"].ToString());
        //                    if (status == "200")
        //                    {
        //                        //send sms to ceo
        //                        link = (Request.Url.Host.Contains("localhost") ? "http://localhost:27466/" : "http://rajsahakarapp.rajasthan.gov.in/") + "/Content/CCSLetter/" + FileNameCEO + "pdf";
        //                        status = new EsanchaarController().SendSocietySMS("Regarding society complaint a letter is issued. click on " + link + " to open", LetterDetails.CEOMobile);
        //                    }
        //                    result = "Success";

        //                }
        //            }
        //        }



        //    }
        //    catch (System.Exception ex)
        //    {

        //        DBLogger objDBLogger = new DBLogger();
        //        objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Society", FunctionName = " ElectionPDF(DataTable List, string FileName, string Titel)" }, ex);
        //        result = "Error";
        //    }
        //    return result;

        //}

        //[HttpGet]
        //public ActionResult CCSConsolidatedReport()
        //{
        //    ViewBag.LoggedInUserDistrictId = _LoggedInUser.DistrictId;
        //    return View();
        //}

        //[HttpPost]
        //public string GetCCSocietyComplentForPrint(List<Params> paramlist)
        //{
        //    SocietyModel objCitizenModel = new SocietyModel(new DBLogger());
        //    // string whereclause = " and (Societies.RegistrationNo like 'MSCS%' or SocietyRegOfficeDetails.DISTRICT_ID=" + paramlist[0].Value + " ) ";
        //    return COMMON_FUNCTION.DataTableToJSONString(objCitizenModel.GetCCSocietyComplentForPrint(paramlist[0].Value, _LoggedInUser.DistrictId));
        //}

        //[HttpPost]
        //public string GetCCSData(List<Params> paramlist)
        //{
        //    SocietyModel objCitizenModel = new SocietyModel(new DBLogger());
        //    // string whereclause = " and (Societies.RegistrationNo like 'MSCS%' or SocietyRegOfficeDetails.DISTRICT_ID=" + paramlist[0].Value + " ) ";
        //    return COMMON_FUNCTION.DataTableToJSONString(objCitizenModel.GetCCSData(paramlist[0].Value, _LoggedInUser.DistrictId));
        //}

        //[HttpPost]
        //public JsonResult GetCCSConsolidatedReportData(string fromDate, string toDate, int districtId, int CCSId)
        //{
        //    string msg = string.Empty;

        //    if (!string.IsNullOrEmpty(fromDate) && !DataFormatter.IsValidDateTime(fromDate))
        //        msg += "date format in from date is not valid (required dd/mm/yyyy). ";

        //    if (!string.IsNullOrEmpty(toDate) && !DataFormatter.IsValidDateTime(toDate))
        //        msg += "date format in to date is not valid (required dd/mm/yyyy). ";

        //    if (!string.IsNullOrEmpty(msg))
        //        return Json(new { Result = false, data = new int[] { }, Message = msg }); // Validation fails no further action required

        //    CitizenModel objCitizenModel = new CitizenModel(new DBLogger());

        //    if (!string.IsNullOrEmpty(fromDate))
        //        fromDate = fromDate.Split('/')[1] + "/" + fromDate.Split('/')[0] + "/" + fromDate.Split('/')[2];

        //    if (!string.IsNullOrEmpty(toDate))
        //        toDate = toDate.Split('/')[1] + "/" + toDate.Split('/')[0] + "/" + toDate.Split('/')[2];

        //    DataTable dt = objCitizenModel.GetCCSConsolidatedReportData(fromDate, toDate, districtId, CCSId, _LoggedInUser.DistrictId, _LoggedInUser.UserRoleDeptMappingId);

        //    if (dt == null || dt.Rows.Count < 1)
        //        return Json(new { Result = false, data = new int[] { }, Message = "No Record found as per search criteria." });

        //    List<CCSComplaintRegisterModel> list = new List<CCSComplaintRegisterModel>();

        //    foreach (var item in dt.AsEnumerable())
        //    {
        //        CCSComplaintRegisterModel temp = new CCSComplaintRegisterModel();
        //        temp.ComplainerName = item.Field<string>("ComplainerName");
        //        temp.ComplainerFatherName = item.Field<string>("ComplainerFatherName");
        //        temp.MobileNo = item.Field<string>("MobileNo");
        //        temp.ReceiptNo = item.Field<string>("ReceiptNo");
        //        temp.AmountDeposited = item.Field<decimal>("AmountDeposited");
        //        temp.CreateDate = item.Field<string>("CreateDateTime");
        //        temp.ComplaintIdEnc = item.Field<string>("ComplaintIdEnc");
        //        temp.IsPermanentLocked = item.Field<bool>("IsPermanentLocked");
        //        try
        //        {
        //            string str = COMMON_FUNCTION.Decrypt(item.Field<string>("AADHAR"));
        //            temp.AADHARMasked = string.Concat("".PadLeft(8, 'X'), str.Substring(str.Length - 4));
        //        }
        //        catch (Exception ex)
        //        {
        //            temp.AADHARMasked = "XXXXXXXXXXXX";
        //            DBLogger objDBLogger = new DBLogger();
        //            objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahkar.Controller.Citizen", FunctionName = "public JsonResult GetCCSConsolidatedReportData(string fromDate, string toDate, int districtId, int CCSId) while AADHAR Decryption " + " Message: " + Convert.ToString(ex.Message) }, ex);
        //        }

        //        temp.District = new DistrictModel() { DistrictId = 1, DistrictEng = item.Field<string>("DISTRICT_ENG"), DistrictHi = item.Field<string>("DISTRICT_MANGAL") };
        //        temp.CCS = new CCSModel() { SocietyNameEng = item.Field<string>("SocietyName") };
        //        list.Add(temp);
        //    }

        //    return Json(new { Result = true, data = list, Message = "Success" });
        //}


        //[HttpPost]
        //[ActionName("GetUseDivisionId")]
        //public string GetUseDivisionId()
        //{
        //    return (_LoggedInUser.DivisionId.ToString());
        //}



        [HttpPost]
        [ActionName("DeleteFaildRecord")]
        public int DeleteSocietyBulkUploadFaildRecord(int SocietyID)
        {
            int result = 0;
            SocietyModel objNew = new SocietyModel(new DBLogger());
            result = objNew.DeleteSocietyRecord(SocietyID);
            return result;
        }
        [HttpPost]
        [ActionName("GetSocietyDeleteFaildRecord")]
        public string RajfedSocietyDetailList(string RegistrationNo)
        {
            SocietyModel objSocietyModel = new SocietyModel(new FileLogger());
            DataTable DT = objSocietyModel.DeleteSocietyRecordList(RegistrationNo);
            return COMMON_FUNCTION.DataTableToJSONString(DT);

        }
        //#endregion
        #region Credit Cooperative Society Complaint 


        public ActionResult CCSComplaint()
        {
            //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ViewBag.LoggedInUserDistrictId = _LoggedInUser.DistrictId;
            var response = TempData.Peek("Response")?.ToString();
            ViewBag.response = response;

            if (Request.Cookies[SessionIdentity.Culture] == null)
            {
                Response.Cookies.Add(new System.Web.HttpCookie(Cooperative.Controllers.SessionIdentity.Culture, "")
                { Value = "en-us", HttpOnly = true });
            }

            return View();
        }


        [HttpPost]
        public JsonResult GetCCSComplaintList(DataPagingModel Details)
        {
            ViewBag.LoggedInUserDistrictId = _LoggedInUser.DistrictId;
            SocietyModel objNew = new SocietyModel(new DBLogger());
            int DistrictId = _LoggedInUser.DistrictId;
            DataTable DT = objNew.GetCCSComplaint(ref Details, DistrictId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public string GenerateCCSLetter_OLD(string OTP, string TXN, CCSLetter LetterDetails)
        {

            var otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
            if (otpResult.status != "1")
            {
                return otpResult.data;
            }



            string result = "";

            SocietyModel objNew = new SocietyModel(new DBLogger());
            string uniqeId = Guid.NewGuid().ToString();
            try
            {


                string RO = "";

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                DataPagingModel Details = new DataPagingModel();
                Details.PageNumber = 1;
                Details.PageSize = 10;
                Details.SearchParameter = new Dictionary<string, string>();
                Details.SearchParameter.Add("ComplaintId", LetterDetails.ComplaintId);
                int DistrictId = _LoggedInUser.DistrictId;
                DataTable List = objNew.GetCCSComplaint(ref Details, DistrictId);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                string filepath = Server.MapPath("~/Content/CCSLetter");
                string fontSize = "font-size: 15px;";

                if (!System.IO.Directory.Exists(filepath))
                {
                    System.IO.Directory.CreateDirectory(filepath);
                }

                #region Complainant Letter
                bool isGeneratePDF = false;
                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                int count = 0;

                string esignfiles = "";

                foreach (var items in LetterDetails.SocietyDetails)
                {
                    sb = new StringBuilder();
                    sb1 = new StringBuilder();
                    string FileNameComplainant = "cc_sc_" + items.ComplaintId;
                    sb.Append("<div style='padding-left:50px;padding-right:50px;padding-top:20px'>");
                    sb.Append("<table border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");

                    sb.Append("<tr><td colspan='1' style='text-align: left; padding-top:20px'>");
                    sb.Append("<br/><br/><br/><br/><img style='width:75px;' src='" + Server.MapPath("~/Content/Images/cooperative_logo.png") + "'/>");
                    sb.Append("</td>");

                    sb.Append("<td colspan='3' style='text-align: center; height: 109px'>");
                    sb.Append("<img style='width:70px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                    sb.Append("</td>");

                    sb.Append("<td colspan='1' style='text-align: right;padding-top:20px'>");
                    sb.Append("<br/><br/><br/><br/><img style='width:75px;' src='" + Server.MapPath("~/Content/Images/logo.png") + "'/>");
                    sb.Append("</td></tr>");


                    sb.Append("<tr><td colspan='5' style='text-align:center;font-size:32px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                    sb.Append("<tr><td colspan='5' style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:20px;'>" + UnicodeToKrutidev.GetKrutidev("सहकारिता विभाग") + "</label><label  style='font-size:25px;'> </label></b>");
                    sb.Append("</td></tr>");

                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style='text-align:left;'> Doc. No :- " + items.ReceiptNo + "</td>");
                    sb.Append("<td colspan='2' style='text-align:right;'> दिनांक :" + DateTime.Now.ToString("dd/MM/yyyy") + " </td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left;'><br/>To</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'><br/>  " + items.ComplainerName + " , मोबाइल न. " + items.MobileNo + " </td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'> " + items.Address + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left'> श्रीमान, </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'> आपने दिनांक " + List.Rows[0]["CreateDateTime"] + " को " + List.Rows[0]["SocietyName"] + " के खिलाफ राजसहकार पोर्टल पर एक शिकायत दर्ज करवाई है जिसका विवरण निम्न प्रकार है :- </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5'>");
                    sb.Append("<table border='1' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0' >");
                    sb.Append("<tr>");
                    sb.Append("<th> शिकायत न. </th>");
                    sb.Append("<th> के खिलाफ </th>");
                    sb.Append("<th> खाता नंबर </th>");
                    sb.Append("<th> राशि </th>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td>" + List.Rows[0]["CCSId"] + " </td> ");
                    sb.Append("<td> " + List.Rows[0]["SocietyName"] + " </td> ");
                    sb.Append("<td>" + List.Rows[0]["AccountNo"] + "  </td> ");
                    sb.Append("<td> " + List.Rows[0]["AmountDeposited"] + " </td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'><br/> आप दिनांक  " + LetterDetails.Date.Split(' ')[0] + " को  समय " + LetterDetails.Date.Split(' ')[1] + "" + LetterDetails.Date.Split(' ')[2] + " एवं " + LetterDetails.Place + " स्थान पर अधोहस्ताक्षरकर्ता के समक्ष सभी संबंधित दस्तावेजों की मूलप्रति के साथ उपस्थित होना सुनिश्चित करे |  </td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    //sb.Append("<td colspan='2' style='text-align:left;margin-left:100px;'><br/><br/><br/> दिनांक  " + DateTime.Now.ToString("dd/MM/yyyy") + "</td>");
                    sb.Append("<td colspan='5' style='text-align:right;'> <br/><br/><br/> अधिकारी के हस्ताक्षर </td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</div>");
                    LetterDetails.ComplaintId = LetterDetails.ComplaintId;
                    // bool isGeneratePDF = false;
                    // System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "20px"));
                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileNameComplainant + ".pdf", ""))
                    {

                        LetterDetails.SocietyDetails[count].complainant_letter = FileNameComplainant + ".pdf";

                        esignfiles += (esignfiles != "" ? "#" : "") + filepath + "/" + FileNameComplainant + ".pdf";

                        isGeneratePDF = true;
                    }
                    count = count + 1;
                }


                #endregion


                #region CEO Letter  
                string FileNameCEO = "ccsceo_" + LetterDetails.ComplaintId;
                if (isGeneratePDF)
                {
                    isGeneratePDF = false;
                    string FileNameComplainant = "ccsceo_" + LetterDetails.ComplaintId;
                    sb = new System.Text.StringBuilder();
                    sb1 = new System.Text.StringBuilder();
                    sb.Append("<div style='padding-left:50px;padding-right:50px;padding-top:20px'>");
                    sb.Append("<table border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");
                    sb.Append("<tr><td colspan='1' style='text-align: left; padding-top:20px'>");
                    sb.Append("<br/><br/><br/><br/><img style='width:75px;' src='" + Server.MapPath("~/Content/Images/cooperative_logo.png") + "'/>");
                    sb.Append("</td>");
                    sb.Append("<td colspan='3' style='text-align: center; height: 109px'>");
                    sb.Append("<img style='width:70px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                    sb.Append("</td>");
                    sb.Append("<td colspan='1' style='text-align: right;padding-top:20px'>");
                    sb.Append("<br/><br/><br/><br/><img style='width:75px;' src='" + Server.MapPath("~/Content/Images/logo.png") + "'/>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td colspan='5' style='text-align:center;font-size:32px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                    sb.Append("<tr><td colspan='5' style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:20px;'>" + UnicodeToKrutidev.GetKrutidev("सहकारिता विभाग") + "</label><label  style='font-size:25px;'> </label></b>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' style='text-align:left;margin-left:100px;'><br/> Doc. No : " + LetterDetails.ReceiptNo + " </td>");
                    sb.Append("<td colspan='3' style='text-align:right;'> दिनांक :" + DateTime.Now.ToString("dd/MM/yyyy") + "  </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left;'><br/>To</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'> <br/>   मुख्य कार्यकारी अधिकारी </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'> " + List.Rows[0]["SocietyName"] + " </ td >");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'>" + List.Rows[0]["ScoietyAddress"] + " " + "( राजस्थान )" + "  <br/>  </ td >");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left;'> <br/>  श्री मान, </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'> <br/>    राजसहकार पोर्टल पर आपके खिलाफ निम्न शिकायत प्राप्त हुई है:-  </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5'>");
                    sb.Append("<table border='1' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0' >");
                    sb.Append("<tr>");
                    sb.Append("<th style ='border:1px solid black'> शिकायतकर्ता का नाम </th>");
                    sb.Append("<th style ='border:1px solid black'> मोबाइल न . </th>");
                    //sb.Append("<th style ='border:1px solid black'> अकाउंट न. / कस्टमर आईडी </th>");
                    sb.Append("<th style ='border:1px solid black;text-align:right;'> राशि </th>");
                    sb.Append("</tr>");
                    foreach (var item in LetterDetails.SocietyDetails)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style ='border:1px solid black'>" + item.ComplainerName + " </td> ");
                        sb.Append("<td style ='border:1px solid black'> " + item.MobileNo + " </td> ");
                        //sb.Append("<td style ='border:1px solid black'>" + item.AccountNo + "  </td> ");
                        sb.Append("<td style ='border:1px solid black;text-align:right;'> " + item.AmountDeposited + " </td>");
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'> उक्त शिकायतों के संबंध में आपकी समिति का  पक्ष रखने हेतु मय  रिकॉर्ड अधोहस्ताक्षरकर्ता के समक्ष दिनांक " + LetterDetails.Date.Split(' ')[0] + " एवं समय " + LetterDetails.Date.Split(' ')[1] + " " + LetterDetails.Date.Split(' ')[2] + " एवं " + LetterDetails.Place + " स्थान पर उपस्थित होना सुनिश्चित करें ! </ td >");
                    sb.Append("</tr>");


                    sb.Append("<tr>");
                    //sb.Append("<td colspan='2' style='text-align:left;margin-left:100px;'><br/><br/><br/> दिनांक : " + DateTime.Now.ToString("dd/MM/yyyy") + " </td>");
                    sb.Append("<td colspan='5' style='text-align:right;'> <br/><br/><br/> अधिकारी के हस्ताक्षर </td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</div>");
                    // sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "20px"));
                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileNameCEO + ".pdf", ""))
                    {
                        LetterDetails.ceo_letter = FileNameCEO + ".pdf";



                        esignfiles += (esignfiles != "" ? "#" : "") + filepath + "/" + FileNameCEO + ".pdf";


                        isGeneratePDF = true;
                    }

                }

                #endregion

                if (isGeneratePDF)
                {

                    string FileNameComplainant = "ccsceo_" + LetterDetails.ComplaintId;
                    //string esignfiles = filepath + "/" + FileNameComplainant + ".pdf" + "#" + filepath + "/" + FileNameCEO + ".pdf";
                    var esignResult = COMMON_FUNCTION.EsinghPDF(otpResult.data, esignfiles);
                    result = esignResult.data;
                    if (esignResult.status == "1")
                    {

                        foreach (var itemss in LetterDetails.SocietyDetails)
                        {
                            string Doc = itemss.complainant_letter;
                            LetterDetails.ComplaintId = itemss.ComplaintId;
                            LetterDetails.complainant_letter = Doc;
                            //result = objNew.InsertUpdateCCSComplaintLetter(LetterDetails);
                        }



                        if (result != "-1")
                        {
                            //send sms to Complainant
                            string link = (Request.Url.Host.Contains("localhost") ? "http://localhost:27466/" : "http://rajsahakarapp.rajasthan.gov.in/") + "/Content/CCSLetter/" + FileNameComplainant + ".pdf";

                            string status = new EsanchaarController().SendSocietySMS("Regarding society complaint a letter is issued. click on " + link + " to open", List.Rows[0]["MobileNo"].ToString());
                            if (status == "200")
                            {
                                //send sms to ceo
                                link = (Request.Url.Host.Contains("localhost") ? "http://localhost:27466/" : "http://rajsahakarapp.rajasthan.gov.in/") + "/Content/CCSLetter/" + FileNameCEO + ".pdf";
                                status = new EsanchaarController().SendSocietySMS("Regarding society complaint a letter is issued. click on " + link + " to open", LetterDetails.CEOMobile);
                            }
                            result = "Success";

                        }
                    }

                }



            }
            catch (System.Exception ex)
            {

                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Society", FunctionName = "GenerateCCSLetter_OLD(string OTP, string TXN, CCSLetter LetterDetails)" }, ex);
                result = "Error";
            }
            return result;

        }

        [HttpPost]
        public string GenerateCCSLetter_OLD1(string OTP, string TXN, CCSLetter LetterDetails)
        {

            var otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
            if (otpResult.status != "1")
            {
                return otpResult.data;
            }



            string result = "";

            SocietyModel objNew = new SocietyModel(new DBLogger());
            string uniqeId = Guid.NewGuid().ToString();
            try
            {


                string RO = "";

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                DataPagingModel Details = new DataPagingModel();
                Details.PageNumber = 1;
                Details.PageSize = 10;
                Details.SearchParameter = new Dictionary<string, string>();
                Details.SearchParameter.Add("ComplaintId", LetterDetails.ComplaintId);
                int DistrictId = _LoggedInUser.DistrictId;
                DataTable List = objNew.GetCCSComplaint(ref Details, DistrictId);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                string filepath = Server.MapPath("~/Content/CCSLetter");
                string fontSize = "font-size: 15px;";

                if (!System.IO.Directory.Exists(filepath))
                {
                    System.IO.Directory.CreateDirectory(filepath);
                }

                #region Complainant Letter
                bool isGeneratePDF = false;
                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                int count = 0;

                string esignfiles = "";

                foreach (var items in LetterDetails.SocietyDetails)
                {
                    sb = new StringBuilder();
                    sb1 = new StringBuilder();
                    string FileNameComplainant = "cc_sc_" + items.ComplaintId;
                    sb.Append("<div style='padding-left:50px;padding-right:50px;padding-top:20px'>");
                    sb.Append("<html><body><table border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");

                    sb.Append("<tr><td colspan='1' style='text-align: left; padding-top:20px'>");
                    sb.Append("<br/><br/><br/><br/><img style='width:75px;' src='" + Server.MapPath("~/Content/Images/cooperative_logo.png") + "'/>");
                    sb.Append("</td>");

                    sb.Append("<td colspan='3' style='text-align: center; height: 109px'>");
                    sb.Append("<img style='width:70px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                    sb.Append("</td>");

                    sb.Append("<td colspan='1' style='text-align: right;padding-top:20px'>");
                    sb.Append("<br/><br/><br/><br/><img style='width:75px;' src='" + Server.MapPath("~/Content/Images/logo.png") + "'/>");
                    sb.Append("</td></tr>");


                    sb.Append("<tr><td colspan='5' style='text-align:center;font-size:32px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                    sb.Append("<tr><td colspan='5' style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:20px;'>" + UnicodeToKrutidev.GetKrutidev("सहकारिता विभाग") + "</label><label  style='font-size:25px;'> </label></b>");
                    sb.Append("</td></tr>");

                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style='text-align:left;'> Doc. No :- " + items.ReceiptNo + "</td>");
                    sb.Append("<td colspan='2' style='text-align:right;'> दिनांक :" + DateTime.Now.ToString("dd/MM/yyyy") + " </td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left;'><br/>To</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'><br/>  " + items.ComplainerName + " , मोबाइल न. " + items.MobileNo + " </td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'> " + items.Address + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left'> श्रीमान, </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'> आपने दिनांक " + List.Rows[0]["CreateDateTime"] + " को " + List.Rows[0]["SocietyName"] + " के खिलाफ राजसहकार पोर्टल पर एक शिकायत दर्ज करवाई है जिसका विवरण निम्न प्रकार है :- </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5'>");
                    sb.Append("<table border='1' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0' >");
                    sb.Append("<tr>");
                    sb.Append("<th> शिकायत न. </th>");
                    sb.Append("<th> सोसायटी का नाम / पता </th>");
                    //sb.Append("<th> खाता नंबर </th>");
                    sb.Append("<th> राशि </th>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td>" + List.Rows[0]["CCSId"] + " </td> ");
                    sb.Append("<td> " + List.Rows[0]["SocietyName"] + " </td> ");
                    //sb.Append("<td>" + List.Rows[0]["AccountNo"] + "  </td> ");
                    sb.Append("<td> " + List.Rows[0]["AmountDeposited"] + " </td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'><br/> आप दिनांक  " + LetterDetails.Date.Split(' ')[0] + " को " + LetterDetails.Date.Split(' ')[1] + " " + LetterDetails.Date.Split(' ')[2] + " बजे " + LetterDetails.Place + " स्थान पर अधोहस्ताक्षरकर्ता के समक्ष सभी संबंधित दस्तावेजों की मूलप्रति के साथ उपस्थित होना सुनिश्चित करे |  </td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    //sb.Append("<td colspan='2' style='text-align:left;margin-left:100px;'><br/><br/><br/> दिनांक  " + DateTime.Now.ToString("dd/MM/yyyy") + "</td>");
                    // sb.Append("<td colspan='5' style='text-align:right;'> <br/><br/><br/> अधिकारी के हस्ताक्षर </td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left; width: 100%;height:50px;font-size:10px;border-top:1px solid #ccc;padding:3px;'><b>Note:</b> This is a digitally signed certificate and does not required any physical signature. </td>");
                    sb.Append("</tr>");
                    sb.Append("</table></body></html>");

                    sb.Append("</div>");
                    LetterDetails.ComplaintId = LetterDetails.ComplaintId;
                    // bool isGeneratePDF = false;
                    // System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "20px"));
                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileNameComplainant + ".pdf", ""))
                    {

                        LetterDetails.SocietyDetails[count].complainant_letter = FileNameComplainant + ".pdf";

                        esignfiles += (esignfiles != "" ? "#" : "") + filepath + "/" + FileNameComplainant + ".pdf";

                        isGeneratePDF = true;
                    }
                    count = count + 1;
                }


                #endregion


                #region CEO Letter  
                string FileNameCEO = "ccsceo_" + LetterDetails.ComplaintId;
                if (isGeneratePDF)
                {
                    isGeneratePDF = false;
                    string FileNameComplainant = "ccsceo_" + LetterDetails.ComplaintId;
                    sb = new System.Text.StringBuilder();
                    sb1 = new System.Text.StringBuilder();
                    sb.Append("<div style='padding-left:50px;padding-right:50px;padding-top:20px'>");
                    sb.Append("<table border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");
                    sb.Append("<tr><td colspan='1' style='text-align: left; padding-top:20px'>");
                    sb.Append("<br/><br/><br/><br/><img style='width:75px;' src='" + Server.MapPath("~/Content/Images/cooperative_logo.png") + "'/>");
                    sb.Append("</td>");
                    sb.Append("<td colspan='3' style='text-align: center; height: 109px'>");
                    sb.Append("<img style='width:70px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                    sb.Append("</td>");
                    sb.Append("<td colspan='1' style='text-align: right;padding-top:20px'>");
                    sb.Append("<br/><br/><br/><br/><img style='width:75px;' src='" + Server.MapPath("~/Content/Images/logo.png") + "'/>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td colspan='5' style='text-align:center;font-size:32px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                    sb.Append("<tr><td colspan='5' style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:20px;'>" + UnicodeToKrutidev.GetKrutidev("सहकारिता विभाग") + "</label><label  style='font-size:25px;'> </label></b>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' style='text-align:left;margin-left:100px;'><br/> Doc. No : " + LetterDetails.ReceiptNo + " </td>");
                    sb.Append("<td colspan='3' style='text-align:right;'> दिनांक :" + DateTime.Now.ToString("dd/MM/yyyy") + "  </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left;'><br/>To</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'> <br/>   मुख्य कार्यकारी अधिकारी </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'> " + List.Rows[0]["SocietyName"] + " </ td >");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'>" + List.Rows[0]["ScoietyAddress"] + " " + "( राजस्थान )" + "  <br/>  </ td >");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left;'> <br/>  श्री मान, </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'> <br/>    राजसहकार पोर्टल पर आपके खिलाफ निम्न शिकायत प्राप्त हुई है:-  </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5'>");
                    sb.Append("<table border='1' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0' >");
                    sb.Append("<tr>");
                    sb.Append("<th style ='border:1px solid black'> शिकायतकर्ता का नाम </th>");
                    sb.Append("<th style ='border:1px solid black'> मोबाइल न . </th>");
                    sb.Append("<th style ='border:1px solid black'> अकाउंट न. / कस्टमर आईडी </th>");
                    sb.Append("<th style ='border:1px solid black;text-align:right;'> राशि </th>");
                    sb.Append("</tr>");
                    foreach (var item in LetterDetails.SocietyDetails)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style ='border:1px solid black'>" + item.ComplainerName + " </td> ");
                        sb.Append("<td style ='border:1px solid black'> " + item.MobileNo + " </td> ");
                        sb.Append("<td style ='border:1px solid black'>" + item.AccountNo + "  </td> ");
                        sb.Append("<td style ='border:1px solid black;text-align:right;'> " + item.AmountDeposited + " </td>");
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'> उक्त शिकायतों के संबंध में आपकी समिति का  पक्ष रखने हेतु मय  रिकॉर्ड अधोहस्ताक्षरकर्ता के समक्ष दिनांक " + LetterDetails.Date.Split(' ')[0] + " को " + LetterDetails.Date.Split(' ')[1] + " " + LetterDetails.Date.Split(' ')[2] + " बजे " + LetterDetails.Place + " स्थान पर उपस्थित होना सुनिश्चित करें ! </ td >");
                    sb.Append("</tr>");


                    sb.Append("<tr>");
                    //sb.Append("<td colspan='2' style='text-align:left;margin-left:100px;'><br/><br/><br/> दिनांक : " + DateTime.Now.ToString("dd/MM/yyyy") + " </td>");
                    //sb.Append("<td colspan='5' style='text-align:right;'> <br/><br/><br/> अधिकारी के हस्ताक्षर </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left; width: 100%;height:50px;font-size:10px;border-top:1px solid #ccc;padding:3px;'><b>Note:</b> This is a digitally signed certificate and does not required any physical signature. </td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</div>");
                    // sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "20px"));
                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileNameCEO + ".pdf", ""))
                    {
                        LetterDetails.ceo_letter = FileNameCEO + ".pdf";



                        esignfiles += (esignfiles != "" ? "#" : "") + filepath + "/" + FileNameCEO + ".pdf";


                        isGeneratePDF = true;
                    }

                }

                #endregion

                if (isGeneratePDF)
                {

                    string FileNameComplainant = "ccsceo_" + LetterDetails.ComplaintId;
                    //string esignfiles = filepath + "/" + FileNameComplainant + ".pdf" + "#" + filepath + "/" + FileNameCEO + ".pdf";
                    var esignResult = COMMON_FUNCTION.EsinghPDF(otpResult.data, esignfiles);
                    result = esignResult.data;
                    if (esignResult.status == "1")
                    {
                        foreach (var itemss in LetterDetails.SocietyDetails)
                        {
                            string Doc = itemss.complainant_letter;
                            LetterDetails.ComplaintId = itemss.ComplaintId;
                            LetterDetails.complainant_letter = Doc;
                            //result = objNew.InsertUpdateCCSComplaintLetter(LetterDetails);
                        }

                        if (result != "-1")
                        {
                            //send sms to Complainant
                            string link = (Request.Url.Host.Contains("localhost") ? "http://localhost:27466/" : "http://rajsahakarapp.rajasthan.gov.in/") + "/Content/CCSLetter/" + FileNameComplainant + ".pdf";

                            string status = new EsanchaarController().SendSocietySMS("Regarding society complaint a letter is issued. click on " + link + " to open", List.Rows[0]["MobileNo"].ToString());
                            if (status == "200")
                            {
                                //send sms to ceo
                                link = (Request.Url.Host.Contains("localhost") ? "http://localhost:27466/" : "http://rajsahakarapp.rajasthan.gov.in/") + "/Content/CCSLetter/" + FileNameCEO + ".pdf";
                                status = new EsanchaarController().SendSocietySMS("Regarding society complaint a letter is issued. click on " + link + " to open", LetterDetails.CEOMobile);
                            }
                            result = "Success";

                        }
                    }

                }



            }
            catch (System.Exception ex)
            {

                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Society", FunctionName = " GenerateCCSLetter_OLD1(string OTP, string TXN, CCSLetter LetterDetails)" }, ex);
                result = "Error";
            }
            return result;

        }

        [HttpPost]
        public string GenerateCCSLetter(SocietyDetailsforAction model)
        {
            string result = "";
            SocietyModel objNew = new SocietyModel(new DBLogger());
            string uniqeId = Guid.NewGuid().ToString();
            try
            {
                string RO = "";
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                //DataPagingModel Details = new DataPagingModel();
                //Details.PageNumber = 1;
                //Details.PageSize = 10;
                //Details.SearchParameter = new Dictionary<string, string>();
                int DistrictId = _LoggedInUser.DistrictId;
                //DataTable List = objNew.GetCCSComplaint(ref Details, DistrictId);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                string filepath = Server.MapPath("~/Content/CCSLetter");
                string fontSize = "font-size: 15px;";

                if (!System.IO.Directory.Exists(filepath))
                {
                    System.IO.Directory.CreateDirectory(filepath);
                }

                #region Complainant Letter
                bool isGeneratePDF = false;
                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                int count = 0;

                string esignfiles = "";

                //foreach (var items in model.SocietyDetails)
                //{

                string imgae = Server.MapPath("~/Content/Images/anupgarh.png");

                sb = new StringBuilder();
                sb1 = new StringBuilder();
                string FileNameComplainant = "cc_sc_" + model.complainerId;
                sb.Append("<div style='padding-left:50px;padding-right:50px;padding-top:5px'>");
                sb.Append("<html><body><table border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");
                sb.Append("<tr><td colspan='5' style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                sb.Append("<img style='width:90px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                sb.Append("</td></tr>");
                if (!string.IsNullOrWhiteSpace(model.HeaderImage))
                {
                    sb.Append("<tr><td colspan='5' style='text-align:center;'><img src='" + Server.MapPath("~/Content/Images/" + model.HeaderImage) + "' /></td></tr><br>");
                }
                else
                {
                    sb.Append("<tr><td colspan='5' style='text-align:center;font-size:32px;'><b>" + UnicodeToKrutidev.GetKrutidev("कार्यालय उप रजिस्ट्रार, सहकारी समितियां, " + model.District + "") + "  </b></td></tr>");
                    sb.Append("<tr><td colspan='5' style='text-align:center;font-size:14px;'><b>" + model.OfficeAddress + "  </b><br><br></td></tr>");
                }


                sb.Append("<tr><td colspan='5' style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><br><br><b><label style='font-size:20px;'>" + UnicodeToKrutidev.GetKrutidev("नोटिस बाबत उपस्थिति परिवाद") + "<br><br><br><br></label><label  style='font-size:25px;'> </label></b>");
                sb.Append("</td></tr><br><br><br>");

                sb.Append("<tr>");
                sb.Append("<td colspan='3' style='text-align:left;'>  वास्ते :-<br> " + model.ComplainerName + ",<br>" + model.ComplainerFatherName + ",<br>" + model.Address + " <br/> Mobile Mo- " + model.MobileNo + (!string.IsNullOrWhiteSpace(model.Email) ? "<br/> Email- " + model.Email + "" : "") + "</td>");
                sb.Append("<td colspan='2' style='text-align:right;'> परिवाद संख्या  :" + model.ReceiptNo + "<br><br><br><br><br><br><br><br> </td>");
                sb.Append("</tr><br><br>");
                sb.Append("<br><tr>");
                sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'><br><br><p style='text-indent:50px;'> आप द्वारा The Banning of Unregulated Deposit Scheme Act-2019 के तहत शिकायत / परिवाद दोषी सोसाइटी " + model.SocietyName + " के विरुद्ध प्रस्तुत किया है, जिस पर सोसाइटी के विरुद्ध उचित विधिक कार्यवाही के अनुक्रम में अधिनियम की धारा - 4 सपठित धारा - 22 के तहत दंडनीय अपराध हेतु सम्बन्धित Designated court में परिवाद / इस्तागा प्रस्तुत किया जाना है</p></ td > ");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'><p style='text-indent:50px;'> अतः इस नोटिस के माध्यम से यह सूचित किया जाता है कि आप अपने परिवाद के समर्थन मे जिन दस्तावेजो पर निर्भर रहते हो यथा एफडीआर / पासबुक /निवेश का दस्तावेज /सोसाइटी में मेंबरशिप का का प्रमारपत्र आधार कार्ड की स्वप्रमारित प्रतिलिपियों मय निवेश की दो पासपोर्ट साइज फोटो के दिनांक  " + model.Presentdate + "को इस कार्यालय मे उपस्थित होवें |</p> </td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'><p style='text-indent:50px;'>आप निर्धारित तिथि को उपस्थित नहीं होने की सूरत में यह अवधारणा की जावेगी, की  आप परिवाद पर  कोई कार्यवाही नहीं चाहते है | यह नोटिस आज दिनांक। " + DateTime.Now.ToString("dd/MM/yyyy") + " को मेरे हस्ताक्षर एवं मोहर  से जारी किया गया है |</p></td><br>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td colspan='5' style='text-align:left; width: 100%;height:50px;font-size:10px;border-top:1px solid #ccc;padding:3px;'><b>Note:</b> This is a digitally signed certificate and does not required any physical signature. </td>");
                sb.Append("</tr>");
                sb.Append("</table></body></html>");

                sb.Append("</div>");

                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "20px"));
                model.Notice = sb.ToString().Replace("<br>", "<br/>");
                if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileNameComplainant + ".pdf", ""))
                {

                    model.complainant_letter = FileNameComplainant + ".pdf";

                    esignfiles += (esignfiles != "" ? "#" : "") + filepath + "/" + FileNameComplainant + ".pdf";

                    isGeneratePDF = true;
                }
                count = count + 1;
                // }


                #endregion

                if (isGeneratePDF)
                {
                    var esignResult = COMMON_FUNCTION.EsinghPDF(model.TXN, esignfiles);
                    result = esignResult.data;
                    if (esignResult.status == "1")
                    {

                        string Doc = model.complainant_letter;
                        model.ComplaintId = model.complainerId.ToString();
                        model.complainant_letter = Doc;
                        result = objNew.InsertUpdateCCSComplaintLetter(model);

                        if (result != "-1")
                        {
                            //send sms to Complainant
                            string link = (Request.Url.Host.Contains("localhost") ? "http://localhost:27466/" : "http://rajsahakarapp.rajasthan.gov.in/") + "/Content/CCSLetter/" + FileNameComplainant + ".pdf";
                            string status = new EsanchaarController().SendSocietySMS("Regarding society complaint a letter is issued. click on " + link + " to open", model.MobileNo);

                            string email = Request.Url.Host.Contains("localhost") ? "balwant.chaudhary@devitpl.com" : model.Email;
                            Cooperative.Controllers.COMMON_FUNCTION.SendEmail(email, "Rajsahakar:- Notice Issued Against Complaint No-" + model.ReceiptNo, model.Notice);
                            result = FileNameComplainant + ".pdf";

                        }
                    }

                }
            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.SocietyControler", FunctionName = " GenerateCCSLetter(DataTable List, string FileName, string Titel)" }, ex);
                result = "Error";
            }
            return result;

        }

        [HttpGet]
        public ActionResult CCSConsolidatedReport()
        {
            ViewBag.LoggedInUserDistrictId = _LoggedInUser.DistrictId;
            return View();
        }

        [HttpPost]
        public string GetCCSocietyComplentForPrint(List<Params> paramlist)
        {
            SocietyModel objCitizenModel = new SocietyModel(new DBLogger());
            // string whereclause = " and (Societies.RegistrationNo like 'MSCS%' or SocietyRegOfficeDetails.DISTRICT_ID=" + paramlist[0].Value + " ) ";
            return COMMON_FUNCTION.DataTableToJSONString(objCitizenModel.GetCCSocietyComplentForPrint(paramlist[0].Value, _LoggedInUser.DistrictId));
        }

        //[HttpPost]
        //public string GetCCSData(List<Params> paramlist, int Type = -1)
        //{
        //    SocietyModel objCitizenModel = new SocietyModel(new DBLogger());
        //    // string whereclause = " and (Societies.RegistrationNo like 'MSCS%' or SocietyRegOfficeDetails.DISTRICT_ID=" + paramlist[0].Value + " ) ";
        //    return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objCitizenModel.GetCCSData(paramlist[0].Value, _LoggedInUser.DistrictId, Type));
        //}



        [HttpPost]
        [ActionName("GetUseDivisionId")]
        public string GetUseDivisionId()
        {
            return (_LoggedInUser.DivisionId.ToString());
        }

        [HttpPost]
        public string saveActiondetails_10082020(SocietyDetailsforAction modal)
        {
            //= new SocietyDetailsforAction();
            string result = "0";
            string validationMessage = string.Empty;
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            try
            {

                modal = Request.Form["modal"].ToString().Equals("[]") ? new SocietyDetailsforAction() : (SocietyDetailsforAction)s.Deserialize(Request.Form["modal"].ToString(), typeof(SocietyDetailsforAction));
            }

            catch (System.Exception ex)
            {
                return "Provided input data is not in correct format.";
            }


            if (Request.Files != null && Request.Files.Count > 0)
            {
                string pathString = Server.MapPath("~/Content/CCSLetter");

                if (!System.IO.Directory.Exists(pathString))
                {
                    System.IO.Directory.CreateDirectory(pathString);
                }

                foreach (string files in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[files];
                    string fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);

                    modal.DisposeFile = fileName;

                    if (System.IO.Directory.Exists(pathString))
                    {
                        file.SaveAs(Path.Combine(pathString, fileName));
                    }


                }
            }


            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            result = objSocietyModel.saveActiondetail(modal);
            if (result == "1")
            {
                string MobileNo = modal.MobileNo;
                string Language = "HIN";
                string SMS_MSG = " आप दिनांक  " + modal.DateTime.Split(' ')[0] + " को  समय " + modal.DateTime.Split(' ')[1] + "" + modal.DateTime.Split(' ')[2] + " एवं " + modal.Place + " स्थान पर सभी संबंधित दस्तावेजों की मूलप्रति के साथ उपस्थित होना सुनिश्चित करे | ";  //08-08-2019
                if (MobileNo.Length > 5 && SMS_MSG.Length > 5)
                {
                    EsanchaarController EC = new EsanchaarController();
                    string ReturnVal = EC.SendSocietySMS(SMS_MSG, MobileNo, Language);
                }

            }
            else
            {
                return result;
            }
            return result;

        }
        [HttpPost]
        public string saveActiondetails(SocietyDetailsforAction modal)
        {
            string result1 = "Success";
            string result = "0";
            if (modal.actionId == 1 && modal.IsSystemGen == 1)
            {
                result1 = GenerateCCSLetter(modal);
            }

            if (result1 != "" && result1 != "Error")
            {

                if (Request.Files != null && Request.Files.Count > 0)
                {
                    string pathString = Server.MapPath("~/Content/CCSLetter");

                    if (!System.IO.Directory.Exists(pathString))
                    {
                        System.IO.Directory.CreateDirectory(pathString);
                    }

                    foreach (string files in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[files];
                        string fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                        result1 = fileName;
                        modal.DisposeFile = fileName;

                        if (System.IO.Directory.Exists(pathString))
                        {
                            file.SaveAs(Path.Combine(pathString, fileName));
                        }
                    }
                    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                    //DataPagingModel Details = new DataPagingModel();
                    //Details.PageNumber = 1;
                    //Details.PageSize = 10;
                    //Details.SearchParameter = new Dictionary<string, string>();
                    int DistrictId = _LoggedInUser.DistrictId;
                    //DataTable Listt = new SocietyModel(new DBLogger()).GetCCSComplaint(ref Details, DistrictId);
                    //string Doc = model.complainant_letter;
                    modal.ComplaintId = modal.complainerId.ToString();
                    modal.complainant_letter = modal.DisposeFile;
                    result = new SocietyModel(new DBLogger()).InsertUpdateCCSComplaintLetter(modal);

                    if (result != "-1")
                    {
                        //send sms to Complainant
                        if (Request.Url.Host.Contains("localhost"))
                        {

                        }
                        else
                        {
                            string link = (Request.Url.Host.Contains("localhost") ? "http://localhost:27466/" : "http://rajsahakarapp.rajasthan.gov.in/") + "/Content/CCSLetter/" + modal.complainant_letter + ".pdf";
                            string status = new EsanchaarController().SendSocietySMS("Regarding society complaint a letter is issued. click on " + link + " to open", modal.MobileNo);

                            //string email = Request.Url.Host.Contains("localhost") ? "balwant.chaudhary@devitpl.com" : model.Email;
                            //Cooperative.Controllers.COMMON_FUNCTION.SendEmail(email, "Rajsahakar:- Notice Issued Against Complaint No-" + model.ReceiptNo, model.Notice);
                        }
                        result = "1";

                    }

                }
                //else
                //{
                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                result = objSocietyModel.saveActiondetail(modal);
                //}
            }
            return result + "#" + result1;

        }
        [HttpPost]
        [ActionName("getAllActionList")]
        public string saveActiondetails(int CompId)
        {
            SocietyModel objCitizenModel = new SocietyModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objCitizenModel.getAllActionList(CompId));

        }


        [HttpPost]
        public string SaveBulkActiondetails(List<SocietyDetailsforAction> Details)
        {
            string result1 = "Success";
            string result = "0";
            SocietyModel objNew = new SocietyModel(new DBLogger());
            string uniqeId = Guid.NewGuid().ToString();
            try
            {
                string RO = "";
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                int DistrictId = _LoggedInUser.DistrictId;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                string filepath = Server.MapPath("~/Content/CCSLetter");
                string fontSize = "font-size: 15px;";

                if (!System.IO.Directory.Exists(filepath))
                {
                    System.IO.Directory.CreateDirectory(filepath);
                }

                #region Complainant Letter
                bool isGeneratePDF = false;
                string esignfiles = "";
                esignfiles = "";
                int count = 0;
                Details.ForEach(model =>
                {

                    isGeneratePDF = false;
                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();



                    string imgae = Server.MapPath("~/Content/Images/anupgarh.png");

                    sb = new StringBuilder();
                    sb1 = new StringBuilder();
                    string FileNameComplainant = "cc_sc_" + model.complainerId;
                    sb.Append("<div style='padding-left:50px;padding-right:50px;padding-top:5px'>");
                    sb.Append("<html><body><table border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");
                    sb.Append("<tr><td colspan='5' style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                    sb.Append("<img style='width:90px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                    sb.Append("</td></tr>");
                    if (!string.IsNullOrWhiteSpace(model.HeaderImage))
                    {
                        sb.Append("<tr><td colspan='5' style='text-align:center;'><img src='" + Server.MapPath("~/Content/Images/" + model.HeaderImage) + "' /></td></tr><br>");
                    }
                    else
                    {
                        sb.Append("<tr><td colspan='5' style='text-align:center;font-size:32px;'><b>" + UnicodeToKrutidev.GetKrutidev("कार्यालय उप रजिस्ट्रार, सहकारी समितियां, " + model.District + "") + "  </b></td></tr>");
                        sb.Append("<tr><td colspan='5' style='text-align:center;font-size:14px;'><b>" + model.OfficeAddress + "  </b><br><br></td></tr>");
                    }


                    sb.Append("<tr><td colspan='5' style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><br><br><b><label style='font-size:20px;'>" + UnicodeToKrutidev.GetKrutidev("नोटिस बाबत उपस्थिति परिवाद") + "<br><br><br><br></label><label  style='font-size:25px;'> </label></b>");
                    sb.Append("</td></tr><br><br><br>");

                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style='text-align:left;'>  वास्ते :-<br> " + model.ComplainerName + ",<br>" + model.ComplainerFatherName + ",<br>" + model.Address + " <br/> Mobile Mo- " + model.MobileNo + (!string.IsNullOrWhiteSpace(model.Email) ? "<br/> Email- " + model.Email + "" : "") + "</td>");
                    sb.Append("<td colspan='2' style='text-align:right;'> परिवाद संख्या  :" + model.ReceiptNo + "<br><br><br><br><br><br><br><br> </td>");
                    sb.Append("</tr><br><br>");
                    sb.Append("<br><tr>");
                    sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'><br><br><p style='text-indent:50px;'> आप द्वारा The Banning of Unregulated Deposit Scheme Act-2019 के तहत शिकायत / परिवाद दोषी सोसाइटी " + model.SocietyName + " के विरुद्ध प्रस्तुत किया है, जिस पर सोसाइटी के विरुद्ध उचित विधिक कार्यवाही के अनुक्रम में अधिनियम की धारा - 4 सपठित धारा - 22 के तहत दंडनीय अपराध हेतु सम्बन्धित Designated court में परिवाद / इस्तागा प्रस्तुत किया जाना है</p></ td > ");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'><p style='text-indent:50px;'> अतः इस नोटिस के माध्यम से यह सूचित किया जाता है कि आप अपने परिवाद के समर्थन मे जिन दस्तावेजो पर निर्भर रहते हो यथा एफडीआर / पासबुक /निवेश का दस्तावेज /सोसाइटी में मेंबरशिप का का प्रमारपत्र आधार कार्ड की स्वप्रमारित प्रतिलिपियों मय निवेश की दो पासपोर्ट साइज फोटो के दिनांक  " + model.Presentdate + "को इस कार्यालय मे उपस्थित होवें |</p> </td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'><p style='text-indent:50px;'>आप निर्धारित तिथि को उपस्थित नहीं होने की सूरत में यह अवधारणा की जावेगी, की  आप परिवाद पर  कोई कार्यवाही नहीं चाहते है | यह नोटिस आज दिनांक। " + DateTime.Now.ToString("dd/MM/yyyy") + " को मेरे हस्ताक्षर एवं मोहर  से जारी किया गया है |</p></td><br>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:left; width: 100%;height:50px;font-size:10px;border-top:1px solid #ccc;padding:3px;'><b>Note:</b> This is a digitally signed certificate and does not required any physical signature. </td>");
                    sb.Append("</tr>");
                    sb.Append("</table></body></html>");

                    sb.Append("</div>");
                    model.Notice = sb.ToString().Replace("<br>", "<br/>");
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "20px"));
                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileNameComplainant + ".pdf", ""))
                    {

                        model.complainant_letter = FileNameComplainant + ".pdf";

                        esignfiles += (esignfiles != "" ? "#" : "") + filepath + "/" + FileNameComplainant + ".pdf";

                        isGeneratePDF = true;
                    }
                    count = count + 1;
                });


                #endregion

                if (isGeneratePDF)
                {
                    var esignResult = COMMON_FUNCTION.EsinghPDF(Details[0].TXN, esignfiles);
                    result = esignResult.data;
                    if (esignResult.status == "1")
                    {
                        Details.ForEach(model =>
                        {
                            if (!string.IsNullOrWhiteSpace(model.complainant_letter))
                            {
                                string Doc = model.complainant_letter;
                                model.ComplaintId = model.complainerId.ToString();
                                model.complainant_letter = Doc;
                                result = objNew.InsertUpdateCCSComplaintLetter(model);

                                if (result != "-1")
                                {
                                    //send sms to Complainant
                                    string MobileNo = (Request.Url.Host.Contains("localhost") ? "9460934860" : model.MobileNo);
                                    string link = (Request.Url.Host.Contains("localhost") ? "http://localhost:27466/" : "http://rajsahakarapp.rajasthan.gov.in/") + "/Content/CCSLetter/" + model.complainant_letter + ".pdf";
                                    string status = new EsanchaarController().SendSocietySMS("Regarding society complaint a letter is issued. click on " + link + " to open", MobileNo);

                                    string email = Request.Url.Host.Contains("localhost") ? "balwant.chaudhary@devitpl.com" : model.Email;
                                    Cooperative.Controllers.COMMON_FUNCTION.SendEmail(email, "Rajsahakar:- Notice Issued Against Complaint No-" + model.ReceiptNo, model.Notice);

                                    SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                                    result = objSocietyModel.saveActiondetail(model);
                                    result = "1";

                                }
                            }
                        });
                    }

                }
            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.SocietyControler", FunctionName = " GenerateCCSLetter(DataTable List, string FileName, string Titel)" }, ex);
                result = "Error";
            }
            return result;


            //return result + "#" + result1;

        }



        [HttpPost]
        public string SendMSGFor()
        {
            string result = "";
            int CompId = 4;
            DataTable DT;
            SocietyModel objCitizenModel = new SocietyModel(new DBLogger());
            DT = objCitizenModel.getAllActionList(CompId);
            string MobileNo = "9887356163";
            try
            {

                string SMS_MSG = "Total 21368 Complaint (MSCS-24398, SSCS-58) of Rs 54831.94 Lacs. registered for ccs till 25/11/2019 6.30 PM Out of these : -\n 1 : Adarsh Credit : 8282 (27209.79 Lacs) \n2: Sanjivani : 12656 (21623.52 Lacs) \n 3: Navjeevan : 2606 (4433 Lacs)\nRegards Rajsahakar Team DoIT&C";  //08-08-2019
                if (MobileNo.Length > 5 && SMS_MSG.Length > 5)
                {
                    EsanchaarController EC = new EsanchaarController();
                    string ReturnVal = EC.SendSocietySMS(SMS_MSG, MobileNo);
                }

            }

            catch (Exception ex)
            {
            }
            finally
            {

            }
            return result;
        }


        [HttpPost]
        [ActionName("GetMaxEMitraAppRequestID")]
        public string GetMaxEMitraAppRequestID(string SocietyIdEnc)
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return objSocietyModel.GetMaxEMitraAppRequestID(SocietyIdEnc).ToString();

        }

        #endregion


        #region Map Society Admin 

        public ActionResult MapSociety()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetSocietyForMap(string RegistrationNo)
        {
            Models.MISModel objNew = new Models.MISModel();
            DataTable DT = objNew.GetCertificateforVerify(RegistrationNo.Replace(" ", ""));
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SubmitSocietyForMap(string SocietyId, string SSOId)
        {
            SocietyModel objSocietyModel = new SocietyModel(new FileLogger());
            return Json(objSocietyModel.SubmitSocietyForMap(SocietyId, SSOId), JsonRequestBehavior.AllowGet);
        }



        #endregion

        [HttpPost]
        public JsonResult GetSocietyAmendmentDocuments(string SocietyIdEnc, string AmendmentId)
        {
            SocietyModel objSocietyModel = new SocietyModel(new FileLogger());
            return Json(COMMON_FUNCTION.DataTableToJSONWithJSONNet(objSocietyModel.GetSocietyAmendmentDocuments(SocietyIdEnc, AmendmentId)), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("SaveSocietyProfilingNew")]
        public string SaveSocietyProfilingNew(SocietyRegistrationEntity _Entity)//, SocietyRegisteredOfficeDetailsEntity RegisteredOfficeDetailsEntity
        {
            _Entity.CreatedBy = _Entity.UpdatedBy = _LoggedInUser.UserId;
            string flag = "0";
            string fileName = string.Empty;
            string ReturnJSON = "";
            string pathString = "~/Content/NewRegistration/";
            DataTable memberData = new DataTable();
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();


            DataTable DT = new SocietyModel().GetLovValuesByListId(1);
            if (_Entity.IsNewRegistration == true)
            {
                foreach (DataRow Dr in DT.Rows)
                {

                    string[] validValue = new string[] { "government", "राजकीय", "state", "केंद्रीय", "central" };
                    string LovCode = Dr["LovCode"].ToString().Replace("\n", "").Replace("\r", "").ToUpper();
                    string Lovdescription = Dr["Lovdescription"].ToString().Replace("\n", "").Replace("\r", "").ToUpper();
                    string SocietyName = _Entity.SocietyName.ToUpper();
                    if ((_Entity.SocietyTypeId == 5 || _Entity.SocietyTypeId == 11) && (validValue.Contains(LovCode.ToLower()) || validValue.Contains(Lovdescription.ToLower())))
                    {

                    }
                    else if (SocietyName.ToString().Contains(LovCode) || SocietyName.ToString().Contains(Lovdescription))
                    {
                        return JsonConvert.SerializeObject(new { data = "NotValidName:-" + LovCode + "/" + Lovdescription });
                        //return CommonMessageFactory.Response("NotValidName:-" + LovCode + " , " + Lovdescription);
                    }
                }
            }


            _Entity.IsSWS = _LoggedInUser.IsFromSWC == "YES";
            if (_Entity.IsAmendment)
            {
                try
                {
                    _Entity.AmendmentDocuments = Request.Form["AmendmentDocuments"].ToString().Equals("[]") ? new List<AmendmentDocuments>() : (List<AmendmentDocuments>)s.Deserialize(Request.Form["AmendmentDocuments"].ToString(), typeof(List<AmendmentDocuments>));
                }
                catch (Exception ex)
                {
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen", FunctionName = " JsonResult SaveAmendmentDocumentsSocietyProfilingNew(SocietyAmendmentEntity SocietyAmendment)" }, ex);
                    //return CommonMessageFactory.Response("Provided input data is not in correct format.");                    
                    return JsonConvert.SerializeObject(new { data = "Provided input data is not in correct format." });
                }

            }

            try
            {
                _Entity.lstSocietyWorkScope = Request.Form["lstSocietyWorkScope"].ToString().Equals("[]") ? new List<SocietyWorkScopeEntity>() : (List<SocietyWorkScopeEntity>)s.Deserialize(Request.Form["lstSocietyWorkScope"].ToString(), typeof(List<SocietyWorkScopeEntity>));
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SocietyController", FunctionName = "SaveSocietyProfilingNew_lstSocietyWorkScope#" + _LoggedInUser.SSOID }, ex);
                //return CommonMessageFactory.Response("Provided input data is not in correct format.");
                return JsonConvert.SerializeObject(new { data = "Provided input data is not in correct format." });
            }

            try
            {
                _Entity.AdministrativeDataList = Request.Form["DataArray"].ToString().Equals("[]") ? new List<AdministrativeData>() : (List<AdministrativeData>)s.Deserialize(Request.Form["DataArray"].ToString(), typeof(List<AdministrativeData>));
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SocietyController", FunctionName = "SaveSocietyProfilingNew_AdministrativeDataList#" + _LoggedInUser.SSOID }, ex);
                //return CommonMessageFactory.Response("Provided input data is not in correct format.");
                return JsonConvert.SerializeObject(new { data = "Provided input data is not in correct format." });
            }



            try
            {
                _Entity.AddressDetails = Request.Form["AddressDetails"].ToString().Equals("[]") ? new SocietyRegisteredOfficeDetailsEntity() : (SocietyRegisteredOfficeDetailsEntity)s.Deserialize(Request.Form["AddressDetails"].ToString(), typeof(SocietyRegisteredOfficeDetailsEntity));
                _Entity.AddressDetails.IsSameCEOAddress = true;
                _Entity.DeleteFlag = 1;
                List<SocietyRegisteredOfficeDetailsEntity> LstSocietyRegisteredOfficeDetails = new List<SocietyRegisteredOfficeDetailsEntity>();
                SocietyRegisteredOfficeDetailsEntity OBJnew = new SocietyRegisteredOfficeDetailsEntity();
                OBJnew.SocietyRegOfficeDetailId = _Entity.AddressDetails.SocietyRegOfficeDetailId;
                OBJnew.SocietyIdEnc = _Entity.SocietyIDEnc;
                OBJnew.IsRural = _Entity.AddressDetails.IsRural;
                OBJnew.HouseBuildingNo = _Entity.AddressDetails.HouseBuildingNo;
                OBJnew.LaneStreetName = _Entity.AddressDetails.LaneStreetName;
                OBJnew.Locality = _Entity.AddressDetails.Locality;
                OBJnew.STATE_ID = _Entity.AddressDetails.STATE_ID; ;
                OBJnew.DIVISION_ID = _Entity.AddressDetails.DIVISION_ID;
                OBJnew.DISTRICT_ID = _Entity.AddressDetails.DISTRICT_ID;
                OBJnew.CityTypes = _Entity.AddressDetails.CityTypes;
                OBJnew.CITY_ID = _Entity.AddressDetails.CITY_ID;
                OBJnew.BLOCK_ID = _Entity.AddressDetails.BLOCK_ID;
                OBJnew.WARD_ID = _Entity.AddressDetails.WARD_ID;
                OBJnew.GP_ID = _Entity.AddressDetails.GP_ID;
                OBJnew.VILLAGE_ID = _Entity.AddressDetails.VILLAGE_ID;
                OBJnew.PinCode = _Entity.AddressDetails.PinCode;
                OBJnew.CreatedBy = _Entity.AddressDetails.CreatedBy;
                OBJnew.UpdatedBy = _Entity.AddressDetails.UpdatedBy;
                OBJnew.CreatedIP = _Entity.AddressDetails.CreatedIP;
                OBJnew.UpdatedIP = _Entity.AddressDetails.UpdatedIP;
                OBJnew.IsSameCEOAddress = _Entity.AddressDetails.IsSameCEOAddress;
                OBJnew.AddressFor = _Entity.AddressDetails.AddressFor;
                OBJnew.SSO_ID = _LoggedInUser.SSOID;
                OBJnew.KIOSKCODE = _LoggedInUser.KIOSKCODE;
                OBJnew.EMitraTransactionId = _Entity.AddressDetails.EMitraTransactionId;
                _LoggedInUser.ServiceId = "0";
                OBJnew.ServiceId = int.Parse(_LoggedInUser.ServiceId);
                OBJnew.SubmitStatus = _Entity.SaveStatus;
                OBJnew.UserType = _LoggedInUser.UserType;
                LstSocietyRegisteredOfficeDetails.Add(OBJnew);

                _Entity.IsSameCEOAddress = true;

                if (_Entity.IsSameCEOAddress == true)//if diffrent address
                {
                    SocietyRegisteredOfficeDetailsEntity NewSocietyRegisteredOfficeDetailsEntity = new SocietyRegisteredOfficeDetailsEntity();
                    NewSocietyRegisteredOfficeDetailsEntity.AddressFor = 9;
                    NewSocietyRegisteredOfficeDetailsEntity.SocietyRegOfficeDetailId = _Entity.AddressDetails.SocietyRegisrationID_copy;
                    NewSocietyRegisteredOfficeDetailsEntity.IsSameCEOAddress = false;
                    NewSocietyRegisteredOfficeDetailsEntity.SocietyIdEnc = _Entity.SocietyIDEnc;
                    NewSocietyRegisteredOfficeDetailsEntity.HouseBuildingNo = _Entity.AddressDetails.HouseBuildingNocopy;
                    NewSocietyRegisteredOfficeDetailsEntity.LaneStreetName = _Entity.AddressDetails.LaneStreetNamecopy;
                    NewSocietyRegisteredOfficeDetailsEntity.Locality = _Entity.AddressDetails.Localitycopy;
                    NewSocietyRegisteredOfficeDetailsEntity.STATE_ID = _Entity.AddressDetails.STATE_IDcopy;
                    NewSocietyRegisteredOfficeDetailsEntity.DIVISION_ID = _Entity.AddressDetails.DIVISION_ID_copy;
                    NewSocietyRegisteredOfficeDetailsEntity.DISTRICT_ID = _Entity.AddressDetails.DISTRICT_ID_copy;
                    NewSocietyRegisteredOfficeDetailsEntity.IsRural = _Entity.AddressDetails.IsRuralcopy;
                    NewSocietyRegisteredOfficeDetailsEntity.CITY_ID = _Entity.AddressDetails.CITY_IDcopy;
                    NewSocietyRegisteredOfficeDetailsEntity.WARD_ID = _Entity.AddressDetails.WARD_IDcopy;
                    NewSocietyRegisteredOfficeDetailsEntity.GP_ID = _Entity.AddressDetails.GP_IDcopy;
                    NewSocietyRegisteredOfficeDetailsEntity.VILLAGE_ID = _Entity.AddressDetails.VILLAGE_IDcopy;
                    NewSocietyRegisteredOfficeDetailsEntity.PinCode = _Entity.AddressDetails.PinCodecopy;
                    NewSocietyRegisteredOfficeDetailsEntity.CityTypes = _Entity.AddressDetails.CityTypes_copy;
                    NewSocietyRegisteredOfficeDetailsEntity.BLOCK_ID = _Entity.AddressDetails.BLOCK_ID_copy;
                    NewSocietyRegisteredOfficeDetailsEntity.DeleteFlag = 2;
                    NewSocietyRegisteredOfficeDetailsEntity.SSO_ID = _LoggedInUser.SSOID;
                    NewSocietyRegisteredOfficeDetailsEntity.KIOSKCODE = _LoggedInUser.KIOSKCODE;
                    NewSocietyRegisteredOfficeDetailsEntity.EMitraTransactionId = _Entity.EMitraTransactionId;
                    NewSocietyRegisteredOfficeDetailsEntity.ServiceId = int.Parse(_LoggedInUser.ServiceId);
                    NewSocietyRegisteredOfficeDetailsEntity.SubmitStatus = _Entity.SaveStatus;
                    NewSocietyRegisteredOfficeDetailsEntity.UserType = _Entity.UserType;
                    LstSocietyRegisteredOfficeDetails.Add(NewSocietyRegisteredOfficeDetailsEntity);

                }
                if (_Entity.IsSameCEOAddress == true && _Entity.ActId == 7)//if diffrent address
                {
                    SocietyRegisteredOfficeDetailsEntity newobj = new SocietyRegisteredOfficeDetailsEntity();
                    newobj.IsSameCEOAddress = true;
                    newobj.AddressFor = 11;    //scretary id
                    newobj.SocietyIdEnc = _Entity.SocietyIDEnc;
                    newobj.HouseBuildingNo = _Entity.AddressDetails.HouseBuildingNoSecretary;
                    newobj.LaneStreetName = _Entity.AddressDetails.LaneStreetNameSecretary;
                    newobj.Locality = _Entity.AddressDetails.LocalitySecretary;
                    newobj.STATE_ID = _Entity.AddressDetails.STATE_IDSecretary;
                    newobj.DIVISION_ID = _Entity.AddressDetails.DIVISION_IDSecretary;
                    newobj.DISTRICT_ID = _Entity.AddressDetails.DISTRICT_IDSecretary;
                    newobj.IsRural = _Entity.AddressDetails.IsRuralSecretary;
                    newobj.CITY_ID = _Entity.AddressDetails.CITY_IDSecretary;
                    newobj.WARD_ID = _Entity.AddressDetails.WARD_IDSecretary;
                    newobj.GP_ID = _Entity.AddressDetails.GP_IDSecretary;
                    newobj.VILLAGE_ID = _Entity.AddressDetails.VILLAGE_IDSecretary;
                    newobj.PinCode = _Entity.AddressDetails.PinCodeSecretary;
                    newobj.CityTypes = _Entity.AddressDetails.CityTypesSecretary;
                    newobj.BLOCK_ID = _Entity.AddressDetails.BLOCK_IDSecretary;

                    newobj.DeleteFlag = 3;

                    newobj.SSO_ID = _LoggedInUser.SSOID;
                    newobj.KIOSKCODE = _LoggedInUser.KIOSKCODE;
                    newobj.EMitraTransactionId = _Entity.EMitraTransactionId;
                    newobj.ServiceId = int.Parse(_LoggedInUser.ServiceId);
                    newobj.SubmitStatus = _Entity.SaveStatus;
                    newobj.UserType = _Entity.UserType;
                    LstSocietyRegisteredOfficeDetails.Add(newobj);

                }
                if (_Entity.IsSameCEOAddress == true && _Entity.ActId == 7)//if diffrent address
                {
                    SocietyRegisteredOfficeDetailsEntity newobjfortreasurer = new SocietyRegisteredOfficeDetailsEntity();
                    newobjfortreasurer.IsSameCEOAddress = false;
                    newobjfortreasurer.AddressFor = 12;    //treasurer id
                    newobjfortreasurer.SocietyIdEnc = _Entity.SocietyIDEnc;
                    newobjfortreasurer.HouseBuildingNo = _Entity.AddressDetails.HouseBuildingNoTreasurer;
                    newobjfortreasurer.LaneStreetName = _Entity.AddressDetails.LaneStreetNameTreasurer;
                    newobjfortreasurer.Locality = _Entity.AddressDetails.LocalityTreasurer;
                    newobjfortreasurer.STATE_ID = _Entity.AddressDetails.STATE_IDTreasurer;
                    newobjfortreasurer.DIVISION_ID = _Entity.AddressDetails.DIVISION_IDTreasurer;
                    newobjfortreasurer.DISTRICT_ID = _Entity.AddressDetails.DISTRICT_IDTreasurer;
                    newobjfortreasurer.IsRural = _Entity.AddressDetails.IsRuralTreasurer;
                    newobjfortreasurer.CITY_ID = _Entity.AddressDetails.CITY_IDTreasurer;
                    newobjfortreasurer.WARD_ID = _Entity.AddressDetails.WARD_IDTreasurer;
                    newobjfortreasurer.GP_ID = _Entity.AddressDetails.GP_IDTreasurer;
                    newobjfortreasurer.VILLAGE_ID = _Entity.AddressDetails.VILLAGE_IDTreasurer;
                    newobjfortreasurer.PinCode = _Entity.AddressDetails.PinCodeTreasurer;
                    newobjfortreasurer.CityTypes = _Entity.AddressDetails.CityTypesTreasurer;
                    newobjfortreasurer.BLOCK_ID = _Entity.AddressDetails.BLOCK_IDTreasurer;
                    newobjfortreasurer.UserType = _Entity.UserType;
                    newobjfortreasurer.DeleteFlag = 4;

                    newobjfortreasurer.SSO_ID = _LoggedInUser.SSOID;
                    newobjfortreasurer.KIOSKCODE = _LoggedInUser.KIOSKCODE;
                    newobjfortreasurer.EMitraTransactionId = _Entity.EMitraTransactionId;
                    newobjfortreasurer.ServiceId = int.Parse(_LoggedInUser.ServiceId);
                    newobjfortreasurer.SubmitStatus = _Entity.SaveStatus;
                    LstSocietyRegisteredOfficeDetails.Add(newobjfortreasurer);
                }

                if (LstSocietyRegisteredOfficeDetails.Count > 0)
                    _Entity.SocietyRegisteredOfficeDetailsLst = LstSocietyRegisteredOfficeDetails;
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SocietyController", FunctionName = "SaveSocietyProfilingNew_AddressDetails#" + _LoggedInUser.SSOID }, ex);
                return CommonMessageFactory.Response("Provided input data is not in correct format.");
            }

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());

            try
            {
                _Entity.RegistrationNo = (_Entity.RegistrationNo == "undefined" || _Entity.RegistrationNo == null) ? "" : _Entity.RegistrationNo;

                if (_Entity.IsNewRegistration == false || _Entity.RegistrationNo.Length > 1)
                {
                    pathString = "~/Content/SocietyProfiling/";
                    pathString += Regex.Replace(_Entity.RegistrationNo.ToUpper().Trim(), "[^a-zA-Z0-9_]+", "");


                    if (_Entity.IsNewRegistration == false && !string.IsNullOrWhiteSpace(_Entity.OldRegistrationNo) && _Entity.RegistrationNo != _Entity.OldRegistrationNo)
                    {
                        flag = objSocietyModel.ValidateOldSocietyIsExist(_Entity.SocietyIDEnc, _Entity.RegistrationNo);
                        var pathStringOld = "~/Content/SocietyProfiling/";
                        pathStringOld += Regex.Replace(_Entity.OldRegistrationNo.ToUpper().Trim(), "[^a-zA-Z0-9_]+", "");

                        if (flag != "-5" && Directory.Exists(Server.MapPath(pathStringOld)) && !Directory.Exists(Server.MapPath(pathString)))
                        {
                            Directory.Move(Server.MapPath(pathStringOld), Server.MapPath(pathString));
                        }
                        else if (flag != "-5" && !Directory.Exists(Server.MapPath(pathStringOld)) && !Directory.Exists(Server.MapPath(pathString)))
                        {
                            Directory.CreateDirectory(Server.MapPath(pathString));
                        }

                    }
                    else if (!Directory.Exists(Server.MapPath(pathString)))
                    {
                        Directory.CreateDirectory(Server.MapPath(pathString));
                    }

                }
                //END

                foreach (string files in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[files];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));
                    if (files.ToString() == "RegistrationCertificateFile")
                        _Entity.RegistrationCertificateFile = fileName;
                    else if (files.ToString() == "ByeLawsSocietyFile")
                        _Entity.ByeLawsSocietyFile = fileName;
                    else if (files.ToString() == "RulesAndRegulationFile")
                        _Entity.RulesAndRegulationFile = fileName;
                    else if (files.ToString() == "RSSCFileName")
                        _Entity.RSSCFileName = fileName;
                    else if (files.ToString() == "DLSCFileName")
                        _Entity.DLSCFileName = fileName;
                    else if (files.ToString() == "SOCFileName")
                        _Entity.SOCFileName = fileName;
                    else if (files.ToString() == "SLSAFileName")
                        _Entity.SLSAFileName = fileName;
                    else if (files.ToString() == "OldExecutiveCommittee")
                        _Entity.OldExecutiveCommittee = fileName;

                    else if (files.ToString() == "MemberListExcelFile")
                    {
                        _Entity.MemberListExcelFile = fileName;
                        string result = "";
                        if (_Entity.ActId == 7)
                        {
                            result = ValidateExcelSportAct(ref memberData, Path.Combine(Server.MapPath(pathString), fileName));
                        }
                        else
                        {
                            result = ValidateExcel(ref memberData, Path.Combine(Server.MapPath(pathString), fileName));
                        }

                        if (result != "OK")
                        {
                            return CommonMessageFactory.GetJsonStringResponse("true", "ErrorInMemberdata@" + result);
                        }

                    }
                    else if (files.ToString() == "CooperativesListFile")
                        _Entity.DLSCFileName = fileName;  //COOPERATIVE UNIT LIST IN DLSCRegNo
                    else if (files.ToString() == "WorkPlanShowingEconomicViabilityFile")
                        _Entity.WorkPlanShowingEconomicViabilityFile = fileName;
                    else if (files.ToString() == "GeneralMeetingProceedingFile")
                        _Entity.GeneralMeetingProceedingFile = fileName;
                    else if (files.ToString() == "BalanceSheetFile")
                        _Entity.BalanceSheetFile = fileName;
                    else if (files.ToString() == "ObjectsFile")
                        _Entity.ObjectsFileName = fileName;

                }

                if (Request.Files != null && Request.Files.Count > 0 || _Entity.SocietyIDEnc.Length > 15 || _Entity.SaveStatus == 0)
                {
                    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                    _Entity.SSOID = _LoggedInUser.SSOID;
                    _Entity.UserType = _LoggedInUser.UserType;
                    _Entity.CreatedBy = _Entity.UpdatedBy = _LoggedInUser.UserId;
                    _Entity.KIOSKCODE = _LoggedInUser.KIOSKCODE;
                    _Entity.AADHARNO = _LoggedInUser.AadharNo;
                    if (_Entity.UserType == "K")
                    {
                        _Entity.ServicesId = int.Parse(_LoggedInUser.ServiceId);
                        _Entity.IsActive = 0;
                    }
                    else if (_Entity.UserType == "C" || _Entity.UserType == "D")
                    {
                        _Entity.ServicesId = 5530;//_Entity.IsNewRegistration == true ? 5530 : 5530;
                        _Entity.IsActive = 1;
                    }
                    _Entity.CreatedIP = _Entity.UpdatedIP = COMMON_FUNCTION.GetIpAddress();

                    flag = objSocietyModel.InsertSocietyProfileNew(_Entity);


                }
                if (flag == "-1" || flag == "0")
                {
                    //System.IO.Directory.Delete(Server.MapPath(pathString));
                }
                else
                {
                    var K = flag.Split('@');
                    if (K.Length > 1)
                    {
                        EmitraController objEC = new EmitraController();
                        PayServicesForSingleForm(K[1], _Entity.SaveStatus.ToString(), "5530", K[0], _Entity.KIOSKCODE, _Entity.SocietyName, _Entity.RegistrationNo);

                        //DataTable societyData = objSocietyModel.GetSocietyAppicationStatus(K[0]);
                        //if (societyData.Rows.Count > 0 && societyData.Rows[0]["IsFromSWC"].ToString() == "1")
                        //{                            
                        //    if (societyData.Rows.Count > 0)
                        //    {
                        //        COMMON_FUNCTION.UpdateSWSPartialSave(societyData.Rows[0]["RegistrationNo"].ToString(), Convert.ToInt32(societyData.Rows[0]["SocietyStatus"]));
                        //        COMMON_FUNCTION.UpdateSWSStatus(societyData.Rows[0]["RegistrationNo"].ToString(), Convert.ToInt32(societyData.Rows[0]["SocietyStatus"]));
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {

                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SocietyController", FunctionName = "SaveSocietyProfiling#" + _LoggedInUser.SSOID }, ex);

            }

            ReturnJSON = "{\"data\":\"" + flag.ToString() + "\",\"societydirectory\":\"" + _Entity.RegistrationNo + "\"}";
            return ReturnJSON;

        }

        [HttpPost]
        public string SaveSocietyProfilingNew_v1(SocietyRegistrationEntity _Entity)//, SocietyRegisteredOfficeDetailsEntity RegisteredOfficeDetailsEntity
        {
            _Entity.CreatedBy = _Entity.UpdatedBy = _LoggedInUser.UserId;
            string flag = "0";
            string fileName = string.Empty;
            string ReturnJSON = "";
            string pathString = "~/Content/NewRegistration/";
            DataTable memberData = new DataTable();
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            _Entity.SocietyIDEnc = _Entity.SocietyIDEnc ?? "";
            _Entity.lstSocietyWorkScope = _Entity.lstSocietyWorkScope ?? new List<SocietyWorkScopeEntity>();
            _Entity.AdministrativeDataList = _Entity.AdministrativeDataList ?? new List<AdministrativeData>();
            DataTable DT = new SocietyModel().GetLovValuesByListId(1);

            if (_Entity.UserType == "K" && _Entity.SaveStatus == 1 && !_Entity.AdministrativeDataList.Where(x => x.SSOID != "" && x.SSOID != null && x.SSOID != "undefined").Any())
            {
                return "{\"data\":\"NotValidSSOID Please enter the SSOID of the CHAIRMAN/PRESIDENT of the Society\"}";
            }
            else if (_Entity.UserType == "K" && _Entity.SaveStatus == 1 && _Entity.AdministrativeDataList.Where(x => x.SSOID != "" && x.SSOID != null && x.SSOID != "undefined").Any())
            {
                string SSO_ID = _Entity.AdministrativeDataList.Where(x => x.SSOID != "" && x.SSOID != null && x.SSOID != "undefined").Select(x => x.SSOID).FirstOrDefault();

                UpdateDistrictEntity obk = new UpdateDistrictEntity();

                var dtls2 = COMMON_FUNCTION.GetUserDetailsBySSOId(SSO_ID);
                if (dtls2.DisplayName == null)
                {
                    return "{\"data\":\"NotValidSSOID Entered  SSOID of the CHAIRMAN/PRESIDENT of the Society is invalid. Please enter valid SSOID\"}";
                }
                else
                {
                    string AADHAARNo = AADHARSerice.GetAADHARNoBySSOId(SSO_ID);
                    if (!_Entity.AdministrativeDataList.Where(x => x.AADHAARNo == AADHAARNo).Any())
                    {
                        return "{\"data\":\"NotValidSSOID Entered  SSOID's details is not match with the CHAIRMAN/PRESIDENT of the Society. Please enter valid SSOID of the CHAIRMAN/PRESIDENT of the Society\"}";
                    }
                }

            }


            if (_Entity.IsNewRegistration == true)
            {
                /*Remove Special words check from system as request letter recieved dated 10-01-2022 from JR rules department*/
                //    var isValidate = COMMON_FUNCTION.ValidateEmblemsAndName(SocietyRegistrationData.Name.ToUpper(), SocietyAmendment.SocietyTypeId);
                //    if (isValidate != "")
                //    {
                //        return Json(new { data = isValidate });
                //    }                    

            }

            try
            {
                //_Entity.AddressDetails = Request.Form["AddressDetails"].ToString().Equals("[]") ? new SocietyRegisteredOfficeDetailsEntity() : (SocietyRegisteredOfficeDetailsEntity)s.Deserialize(Request.Form["AddressDetails"].ToString(), typeof(SocietyRegisteredOfficeDetailsEntity));
                _Entity.AddressDetails.IsSameCEOAddress = true;
                _Entity.DeleteFlag = 1;
                List<SocietyRegisteredOfficeDetailsEntity> LstSocietyRegisteredOfficeDetails = new List<SocietyRegisteredOfficeDetailsEntity>();
                SocietyRegisteredOfficeDetailsEntity OBJnew = new SocietyRegisteredOfficeDetailsEntity();
                OBJnew.SocietyRegOfficeDetailId = _Entity.AddressDetails.SocietyRegOfficeDetailId;
                OBJnew.SocietyIdEnc = _Entity.SocietyIDEnc;
                OBJnew.IsRural = _Entity.AddressDetails.IsRural;
                OBJnew.HouseBuildingNo = _Entity.AddressDetails.HouseBuildingNo;
                OBJnew.LaneStreetName = _Entity.AddressDetails.LaneStreetName;
                OBJnew.Locality = _Entity.AddressDetails.Locality;
                OBJnew.STATE_ID = _Entity.AddressDetails.STATE_ID; ;
                OBJnew.DIVISION_ID = _Entity.AddressDetails.DIVISION_ID;
                OBJnew.DISTRICT_ID = _Entity.AddressDetails.DISTRICT_ID;
                OBJnew.CityTypes = _Entity.AddressDetails.CityTypes;
                OBJnew.CITY_ID = _Entity.AddressDetails.CITY_ID;
                OBJnew.BLOCK_ID = _Entity.AddressDetails.BLOCK_ID;
                OBJnew.WARD_ID = _Entity.AddressDetails.WARD_ID;
                OBJnew.GP_ID = _Entity.AddressDetails.GP_ID;
                OBJnew.VILLAGE_ID = _Entity.AddressDetails.VILLAGE_ID;
                OBJnew.PinCode = _Entity.AddressDetails.PinCode;
                OBJnew.CreatedBy = _Entity.AddressDetails.CreatedBy;
                OBJnew.UpdatedBy = _Entity.AddressDetails.UpdatedBy;
                OBJnew.CreatedIP = _Entity.AddressDetails.CreatedIP;
                OBJnew.UpdatedIP = _Entity.AddressDetails.UpdatedIP;
                OBJnew.IsSameCEOAddress = _Entity.AddressDetails.IsSameCEOAddress;
                OBJnew.AddressFor = _Entity.AddressDetails.AddressFor;
                if (_Entity.UserType == "K")
                {
                    //_Entity.AdministrativeDataList.Where(x=>x.SSOID!="" && x.SSOID != null && x.SSOID != "undefined").Any()
                    OBJnew.SSO_ID = _Entity.AdministrativeDataList.Where(x => x.SSOID != "" && x.SSOID != null && x.SSOID != "undefined").Select(x => x.SSOID).FirstOrDefault();
                }
                else
                {
                    OBJnew.SSO_ID = _LoggedInUser.SSOID;
                }

                OBJnew.KIOSKCODE = _LoggedInUser.KIOSKCODE;
                OBJnew.EMitraTransactionId = _Entity.AddressDetails.EMitraTransactionId;
                _LoggedInUser.ServiceId = "0";
                OBJnew.ServiceId = int.Parse(_LoggedInUser.ServiceId);
                OBJnew.SubmitStatus = _Entity.SaveStatus;
                OBJnew.UserType = _LoggedInUser.UserType;
                LstSocietyRegisteredOfficeDetails.Add(OBJnew);

                _Entity.IsSameCEOAddress = true;

                if (_Entity.IsSameCEOAddress == true)//if diffrent address
                {
                    SocietyRegisteredOfficeDetailsEntity NewSocietyRegisteredOfficeDetailsEntity = new SocietyRegisteredOfficeDetailsEntity();
                    NewSocietyRegisteredOfficeDetailsEntity.AddressFor = 9;
                    NewSocietyRegisteredOfficeDetailsEntity.SocietyRegOfficeDetailId = _Entity.AddressDetails.SocietyRegisrationID_copy;
                    NewSocietyRegisteredOfficeDetailsEntity.IsSameCEOAddress = false;
                    NewSocietyRegisteredOfficeDetailsEntity.SocietyIdEnc = _Entity.SocietyIDEnc;
                    NewSocietyRegisteredOfficeDetailsEntity.HouseBuildingNo = _Entity.AddressDetails.HouseBuildingNocopy;
                    NewSocietyRegisteredOfficeDetailsEntity.LaneStreetName = _Entity.AddressDetails.LaneStreetNamecopy;
                    NewSocietyRegisteredOfficeDetailsEntity.Locality = _Entity.AddressDetails.Localitycopy;
                    NewSocietyRegisteredOfficeDetailsEntity.STATE_ID = _Entity.AddressDetails.STATE_IDcopy;
                    NewSocietyRegisteredOfficeDetailsEntity.DIVISION_ID = _Entity.AddressDetails.DIVISION_ID_copy;
                    NewSocietyRegisteredOfficeDetailsEntity.DISTRICT_ID = _Entity.AddressDetails.DISTRICT_ID_copy;
                    NewSocietyRegisteredOfficeDetailsEntity.IsRural = _Entity.AddressDetails.IsRuralcopy;
                    NewSocietyRegisteredOfficeDetailsEntity.CITY_ID = _Entity.AddressDetails.CITY_IDcopy;
                    NewSocietyRegisteredOfficeDetailsEntity.WARD_ID = _Entity.AddressDetails.WARD_IDcopy;
                    NewSocietyRegisteredOfficeDetailsEntity.GP_ID = _Entity.AddressDetails.GP_IDcopy;
                    NewSocietyRegisteredOfficeDetailsEntity.VILLAGE_ID = _Entity.AddressDetails.VILLAGE_IDcopy;
                    NewSocietyRegisteredOfficeDetailsEntity.PinCode = _Entity.AddressDetails.PinCodecopy;
                    NewSocietyRegisteredOfficeDetailsEntity.CityTypes = _Entity.AddressDetails.CityTypes_copy;
                    NewSocietyRegisteredOfficeDetailsEntity.BLOCK_ID = _Entity.AddressDetails.BLOCK_ID_copy;
                    NewSocietyRegisteredOfficeDetailsEntity.DeleteFlag = 2;
                    NewSocietyRegisteredOfficeDetailsEntity.SSO_ID = _LoggedInUser.SSOID;
                    NewSocietyRegisteredOfficeDetailsEntity.KIOSKCODE = _LoggedInUser.KIOSKCODE;
                    NewSocietyRegisteredOfficeDetailsEntity.EMitraTransactionId = _Entity.EMitraTransactionId;
                    NewSocietyRegisteredOfficeDetailsEntity.ServiceId = int.Parse(_LoggedInUser.ServiceId);
                    NewSocietyRegisteredOfficeDetailsEntity.SubmitStatus = _Entity.SaveStatus;
                    NewSocietyRegisteredOfficeDetailsEntity.UserType = _Entity.UserType;
                    LstSocietyRegisteredOfficeDetails.Add(NewSocietyRegisteredOfficeDetailsEntity);

                }
                if (_Entity.IsSameCEOAddress == true && _Entity.ActId == 7)//if diffrent address
                {
                    SocietyRegisteredOfficeDetailsEntity newobj = new SocietyRegisteredOfficeDetailsEntity();
                    newobj.IsSameCEOAddress = true;
                    newobj.AddressFor = 11;    //scretary id
                    newobj.SocietyIdEnc = _Entity.SocietyIDEnc;
                    newobj.HouseBuildingNo = _Entity.AddressDetails.HouseBuildingNoSecretary;
                    newobj.LaneStreetName = _Entity.AddressDetails.LaneStreetNameSecretary;
                    newobj.Locality = _Entity.AddressDetails.LocalitySecretary;
                    newobj.STATE_ID = _Entity.AddressDetails.STATE_IDSecretary;
                    newobj.DIVISION_ID = _Entity.AddressDetails.DIVISION_IDSecretary;
                    newobj.DISTRICT_ID = _Entity.AddressDetails.DISTRICT_IDSecretary;
                    newobj.IsRural = _Entity.AddressDetails.IsRuralSecretary;
                    newobj.CITY_ID = _Entity.AddressDetails.CITY_IDSecretary;
                    newobj.WARD_ID = _Entity.AddressDetails.WARD_IDSecretary;
                    newobj.GP_ID = _Entity.AddressDetails.GP_IDSecretary;
                    newobj.VILLAGE_ID = _Entity.AddressDetails.VILLAGE_IDSecretary;
                    newobj.PinCode = _Entity.AddressDetails.PinCodeSecretary;
                    newobj.CityTypes = _Entity.AddressDetails.CityTypesSecretary;
                    newobj.BLOCK_ID = _Entity.AddressDetails.BLOCK_IDSecretary;

                    newobj.DeleteFlag = 3;

                    newobj.SSO_ID = _LoggedInUser.SSOID;
                    newobj.KIOSKCODE = _LoggedInUser.KIOSKCODE;
                    newobj.EMitraTransactionId = _Entity.EMitraTransactionId;
                    newobj.ServiceId = int.Parse(_LoggedInUser.ServiceId);
                    newobj.SubmitStatus = _Entity.SaveStatus;
                    newobj.UserType = _Entity.UserType;
                    LstSocietyRegisteredOfficeDetails.Add(newobj);

                }
                if (_Entity.IsSameCEOAddress == true && _Entity.ActId == 7)//if diffrent address
                {
                    SocietyRegisteredOfficeDetailsEntity newobjfortreasurer = new SocietyRegisteredOfficeDetailsEntity();
                    newobjfortreasurer.IsSameCEOAddress = false;
                    newobjfortreasurer.AddressFor = 12;    //treasurer id
                    newobjfortreasurer.SocietyIdEnc = _Entity.SocietyIDEnc;
                    newobjfortreasurer.HouseBuildingNo = _Entity.AddressDetails.HouseBuildingNoTreasurer;
                    newobjfortreasurer.LaneStreetName = _Entity.AddressDetails.LaneStreetNameTreasurer;
                    newobjfortreasurer.Locality = _Entity.AddressDetails.LocalityTreasurer;
                    newobjfortreasurer.STATE_ID = _Entity.AddressDetails.STATE_IDTreasurer;
                    newobjfortreasurer.DIVISION_ID = _Entity.AddressDetails.DIVISION_IDTreasurer;
                    newobjfortreasurer.DISTRICT_ID = _Entity.AddressDetails.DISTRICT_IDTreasurer;
                    newobjfortreasurer.IsRural = _Entity.AddressDetails.IsRuralTreasurer;
                    newobjfortreasurer.CITY_ID = _Entity.AddressDetails.CITY_IDTreasurer;
                    newobjfortreasurer.WARD_ID = _Entity.AddressDetails.WARD_IDTreasurer;
                    newobjfortreasurer.GP_ID = _Entity.AddressDetails.GP_IDTreasurer;
                    newobjfortreasurer.VILLAGE_ID = _Entity.AddressDetails.VILLAGE_IDTreasurer;
                    newobjfortreasurer.PinCode = _Entity.AddressDetails.PinCodeTreasurer;
                    newobjfortreasurer.CityTypes = _Entity.AddressDetails.CityTypesTreasurer;
                    newobjfortreasurer.BLOCK_ID = _Entity.AddressDetails.BLOCK_IDTreasurer;
                    newobjfortreasurer.UserType = _Entity.UserType;
                    newobjfortreasurer.DeleteFlag = 4;

                    newobjfortreasurer.SSO_ID = _LoggedInUser.SSOID;
                    newobjfortreasurer.KIOSKCODE = _LoggedInUser.KIOSKCODE;
                    newobjfortreasurer.EMitraTransactionId = _Entity.EMitraTransactionId;
                    newobjfortreasurer.ServiceId = int.Parse(_LoggedInUser.ServiceId);
                    newobjfortreasurer.SubmitStatus = _Entity.SaveStatus;
                    LstSocietyRegisteredOfficeDetails.Add(newobjfortreasurer);
                }

                if (LstSocietyRegisteredOfficeDetails.Count > 0)
                    _Entity.SocietyRegisteredOfficeDetailsLst = LstSocietyRegisteredOfficeDetails;
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SocietyController", FunctionName = "SaveSocietyProfilingNew_AddressDetails#" + _LoggedInUser.SSOID }, ex);
                return CommonMessageFactory.Response("Provided input data is not in correct format.");
            }

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());

            try
            {
                _Entity.RegistrationNo = (_Entity.RegistrationNo == "undefined" || _Entity.RegistrationNo == null) ? "" : _Entity.RegistrationNo;

                if (_Entity.IsNewRegistration == false || _Entity.RegistrationNo.Length > 1)
                {
                    pathString = "~/Content/SocietyProfiling/";
                    pathString += Regex.Replace(_Entity.RegistrationNo.ToUpper().Trim(), "[^a-zA-Z0-9_]+", "");


                    if (_Entity.IsNewRegistration == false && !string.IsNullOrWhiteSpace(_Entity.OldRegistrationNo) && _Entity.RegistrationNo != _Entity.OldRegistrationNo)
                    {
                        flag = objSocietyModel.ValidateOldSocietyIsExist(_Entity.SocietyIDEnc, _Entity.RegistrationNo);
                        var pathStringOld = "~/Content/SocietyProfiling/";
                        pathStringOld += Regex.Replace(_Entity.OldRegistrationNo.ToUpper().Trim(), "[^a-zA-Z0-9_]+", "");

                        if (flag != "-5" && Directory.Exists(Server.MapPath(pathStringOld)) && !Directory.Exists(Server.MapPath(pathString)))
                        {
                            Directory.Move(Server.MapPath(pathStringOld), Server.MapPath(pathString));
                        }
                        else if (flag != "-5" && !Directory.Exists(Server.MapPath(pathStringOld)) && !Directory.Exists(Server.MapPath(pathString)))
                        {
                            Directory.CreateDirectory(Server.MapPath(pathString));
                        }

                    }
                    else if (!Directory.Exists(Server.MapPath(pathString)))
                    {
                        Directory.CreateDirectory(Server.MapPath(pathString));
                    }

                }
                //END



                //if (Request.Files != null && Request.Files.Count > 0 || _Entity.SocietyIDEnc.Length > 15 || _Entity.SaveStatus == 0)
                //{
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                _Entity.SSOID = _LoggedInUser.SSOID;
                _Entity.UserType = _LoggedInUser.UserType;
                _Entity.CreatedBy = _Entity.UpdatedBy = _LoggedInUser.UserId;
                _Entity.KIOSKCODE = _LoggedInUser.KIOSKCODE;
                _Entity.AADHARNO = _LoggedInUser.AadharNo;
                if (_Entity.UserType == "K")
                {
                    _Entity.ServicesId = int.Parse(_LoggedInUser.ServiceId);
                    _Entity.IsActive = 0;
                }
                else if (_Entity.UserType == "C" || _Entity.UserType == "D")
                {
                    _Entity.ServicesId = 5530;//_Entity.IsNewRegistration == true ? 5530 : 5530;
                    _Entity.IsActive = 1;
                }
                _Entity.CreatedIP = _Entity.UpdatedIP = COMMON_FUNCTION.GetIpAddress();

                flag = objSocietyModel.InsertSocietyProfileNew_v1(_Entity);


                // }
                if (flag == "-1" || flag == "0" || flag == "-5")
                {
                    //System.IO.Directory.Delete(Server.MapPath(pathString));
                }
                else
                {
                    var K = flag.Split('@');

                    if (K.Length > 1)
                    {
                        EmitraController objEC = new EmitraController();
                        PayServicesForSingleForm(K[1], _Entity.SaveStatus.ToString(), "5530", K[0], _Entity.KIOSKCODE, _Entity.SocietyName, _Entity.RegistrationNo);

                        //DataTable societyData = objSocietyModel.GetSocietyAppicationStatus(K[0]);
                        //if (societyData.Rows.Count > 0 && societyData.Rows[0]["IsFromSWC"].ToString() == "1")
                        //{                            
                        //    if (societyData.Rows.Count > 0)
                        //    {
                        //        COMMON_FUNCTION.UpdateSWSPartialSave(societyData.Rows[0]["RegistrationNo"].ToString(), Convert.ToInt32(societyData.Rows[0]["SocietyStatus"]));
                        //        COMMON_FUNCTION.UpdateSWSStatus(societyData.Rows[0]["RegistrationNo"].ToString(), Convert.ToInt32(societyData.Rows[0]["SocietyStatus"]));
                        //    }
                        //}
                    }

                    if (_LoggedInUser.IsFromSWC == "YES")
                    {

                        SWSEntity sWSEntity = JsonConvert.DeserializeObject<SWSEntity>(System.Web.HttpContext.Current.Session[SessionIdentity.SWCSData].ToString());
                        RajNiveshModel rajNivesh = new RajNiveshModel(new FileLogger());
                        if (!string.IsNullOrEmpty(sWSEntity.RegNo))
                        {
                            DataTable dtr = rajNivesh.GetSwcData(sWSEntity.SWSID, sWSEntity.SSO_ID);


                            var rajNiveshRequest = new RajNiveshRequest();
                            rajNiveshRequest.SWSID = Convert.ToInt32(sWSEntity.SWSID);
                            rajNiveshRequest.ActID = Convert.ToInt32(sWSEntity.ActID);
                            rajNiveshRequest.ActivityID = Convert.ToInt32(sWSEntity.ActivityID);
                            rajNiveshRequest.RegNo = sWSEntity.RegNo;
                            rajNiveshRequest.IsNew = Convert.ToInt32(sWSEntity.IsNew);
                            rajNiveshRequest.SSO_ID = sWSEntity.SSO_ID;
                            rajNiveshRequest.SocietyIdEnc = K[0];
                            if (dtr != null && dtr.Rows.Count > 0)
                            {
                                RajNiveshStatusRequest rajNiveshStatus = new RajNiveshStatusRequest();
                                rajNiveshStatus.TRACKERID = Convert.ToInt32(dtr.Rows[0]["SWSID"].ToString());
                                rajNiveshStatus.STATUS_CODE = (_Entity.SaveStatus == 0 ? 1 : 3);
                                rajNiveshStatus.REGISTRATION_NUMBER = dtr.Rows[0]["RegNo"].ToString();
                                rajNiveshStatus.REMARKS = "";
                                rajNivesh.UpdateStatus(rajNiveshStatus);
                            }
                        }

                    }


                    //if (_Entity.ActId == 3)
                    //{
                    //    string result_ncd_id = "";
                    //    try
                    //    {
                    //        NCDApiModel nCDApi = new NCDApiModel();
                    //        NCDServiceModel nCDService = new NCDServiceModel(new DBLogger());
                    //        result_ncd_id = nCDApi.SaveCooperativeDataIntoNCD(K[0]);

                    //        if (result_ncd_id.Length > 10)
                    //        {
                    //            var ncd_response = nCDService.NcdSaveCooperativeData(result_ncd_id);
                    //            COMMON_FUNCTION.Sp_callTrackingInsert("ncd_response", result_ncd_id + "=" + JsonConvert.SerializeObject(ncd_response));
                    //        }
                    //    }
                    //    catch (Exception exncd)
                    //    {

                    //        ILogger objILogger = new DBLogger();
                    //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SocietyController", FunctionName = "Society-SaveCooperativeDataIntoNCD=" + result_ncd_id }, exncd);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {

                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SocietyController", FunctionName = "SaveSocietyProfiling#" + _LoggedInUser.SSOID }, ex);

            }

            ReturnJSON = "{\"data\":\"" + flag.ToString() + "\",\"societydirectory\":\"" + _Entity.RegistrationNo + "\"}";
            return ReturnJSON;

        }


        [HttpPost]
        public JsonResult UploadSocietyProfilingRejectDocument(HttpPostedFileBase RejectFileSocietyProfilingPath)
        {
            try
            {
                if (RejectFileSocietyProfilingPath == null)
                    return Json(new { success = false, message = "File not received" });

                string folder = Server.MapPath("~/Content/SocietyProfilingRejectfile/");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(RejectFileSocietyProfilingPath.FileName);
                string filePath = Path.Combine(folder, fileName);

                RejectFileSocietyProfilingPath.SaveAs(filePath);

                return Json(new { success = true, fileName = fileName });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }



        [HttpPost]
        public string InsertOldSocietyProfilling(SocietyRegistrationEntity _Entity)//, SocietyRegisteredOfficeDetailsEntity RegisteredOfficeDetailsEntity
        {
            _Entity.CreatedBy = _Entity.UpdatedBy = _LoggedInUser.UserId;
            string flag = "0";
            string fileName = string.Empty;
            string ReturnJSON = "";
            string pathString = "~/Content/NewRegistration/";
            DataTable memberData = new DataTable();
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            _Entity.SocietyIDEnc = _Entity.SocietyIDEnc ?? "";
            _Entity.lstSocietyWorkScope = _Entity.lstSocietyWorkScope ?? new List<SocietyWorkScopeEntity>();
            _Entity.AdministrativeDataList = _Entity.AdministrativeDataList ?? new List<AdministrativeData>();
            DataTable DT = new SocietyModel().GetLovValuesByListId(1);

            _Entity.IsNewRegistration = false;

            if (_Entity.IsNewRegistration == true)
            {
                /*Remove Special words check from system as request letter recieved dated 10-01-2022 from JR rules department*/
                //    var isValidate = COMMON_FUNCTION.ValidateEmblemsAndName(SocietyRegistrationData.Name.ToUpper(), SocietyAmendment.SocietyTypeId);
                //    if (isValidate != "")
                //    {
                //        return Json(new { data = isValidate });
                //    }                    

            }
            string SSOID = "";
            try
            {

                if (_Entity.AdministrativeDataList.Where(x => x.SSOID != "" && x.SSOID != "undefined").Any())
                {
                    SSOID = _Entity.AdministrativeDataList.Where(x => x.SSOID != "" && x.SSOID != "undefined").Select(x => x.SSOID).FirstOrDefault();
                }
                //_Entity.AddressDetails = Request.Form["AddressDetails"].ToString().Equals("[]") ? new SocietyRegisteredOfficeDetailsEntity() : (SocietyRegisteredOfficeDetailsEntity)s.Deserialize(Request.Form["AddressDetails"].ToString(), typeof(SocietyRegisteredOfficeDetailsEntity));
                _Entity.AddressDetails.IsSameCEOAddress = true;
                _Entity.DeleteFlag = 1;
                List<SocietyRegisteredOfficeDetailsEntity> LstSocietyRegisteredOfficeDetails = new List<SocietyRegisteredOfficeDetailsEntity>();
                SocietyRegisteredOfficeDetailsEntity OBJnew = new SocietyRegisteredOfficeDetailsEntity();
                OBJnew.SocietyRegOfficeDetailId = _Entity.AddressDetails.SocietyRegOfficeDetailId;
                OBJnew.SocietyIdEnc = _Entity.SocietyIDEnc;
                OBJnew.IsRural = _Entity.AddressDetails.IsRural;
                OBJnew.HouseBuildingNo = _Entity.AddressDetails.HouseBuildingNo;
                OBJnew.LaneStreetName = _Entity.AddressDetails.LaneStreetName;
                OBJnew.Locality = _Entity.AddressDetails.Locality;
                OBJnew.STATE_ID = _Entity.AddressDetails.STATE_ID; ;
                OBJnew.DIVISION_ID = _Entity.AddressDetails.DIVISION_ID;
                OBJnew.DISTRICT_ID = _Entity.AddressDetails.DISTRICT_ID;
                OBJnew.CityTypes = _Entity.AddressDetails.CityTypes;
                OBJnew.CITY_ID = _Entity.AddressDetails.CITY_ID;
                OBJnew.BLOCK_ID = _Entity.AddressDetails.BLOCK_ID;
                OBJnew.WARD_ID = _Entity.AddressDetails.WARD_ID;
                OBJnew.GP_ID = _Entity.AddressDetails.GP_ID;
                OBJnew.VILLAGE_ID = _Entity.AddressDetails.VILLAGE_ID;
                OBJnew.PinCode = _Entity.AddressDetails.PinCode;
                OBJnew.CreatedBy = _Entity.AddressDetails.CreatedBy;
                OBJnew.UpdatedBy = _Entity.AddressDetails.UpdatedBy;
                OBJnew.CreatedIP = _Entity.AddressDetails.CreatedIP;
                OBJnew.UpdatedIP = _Entity.AddressDetails.UpdatedIP;
                OBJnew.IsSameCEOAddress = _Entity.AddressDetails.IsSameCEOAddress;
                OBJnew.AddressFor = _Entity.AddressDetails.AddressFor;
                OBJnew.SSO_ID = SSOID;
                OBJnew.KIOSKCODE = _LoggedInUser.KIOSKCODE;
                OBJnew.EMitraTransactionId = _Entity.AddressDetails.EMitraTransactionId;
                _LoggedInUser.ServiceId = "0";
                OBJnew.ServiceId = int.Parse(_LoggedInUser.ServiceId);
                OBJnew.SubmitStatus = _Entity.SaveStatus;
                OBJnew.UserType = _LoggedInUser.UserType;
                LstSocietyRegisteredOfficeDetails.Add(OBJnew);

                _Entity.IsSameCEOAddress = true;

                if (_Entity.IsSameCEOAddress == true)//if diffrent address
                {
                    SocietyRegisteredOfficeDetailsEntity NewSocietyRegisteredOfficeDetailsEntity = new SocietyRegisteredOfficeDetailsEntity();
                    NewSocietyRegisteredOfficeDetailsEntity.AddressFor = 9;
                    NewSocietyRegisteredOfficeDetailsEntity.SocietyRegOfficeDetailId = _Entity.AddressDetails.SocietyRegisrationID_copy;
                    NewSocietyRegisteredOfficeDetailsEntity.IsSameCEOAddress = false;
                    NewSocietyRegisteredOfficeDetailsEntity.SocietyIdEnc = _Entity.SocietyIDEnc;
                    NewSocietyRegisteredOfficeDetailsEntity.HouseBuildingNo = _Entity.AddressDetails.HouseBuildingNocopy;
                    NewSocietyRegisteredOfficeDetailsEntity.LaneStreetName = _Entity.AddressDetails.LaneStreetNamecopy;
                    NewSocietyRegisteredOfficeDetailsEntity.Locality = _Entity.AddressDetails.Localitycopy;
                    NewSocietyRegisteredOfficeDetailsEntity.STATE_ID = _Entity.AddressDetails.STATE_IDcopy;
                    NewSocietyRegisteredOfficeDetailsEntity.DIVISION_ID = _Entity.AddressDetails.DIVISION_ID_copy;
                    NewSocietyRegisteredOfficeDetailsEntity.DISTRICT_ID = _Entity.AddressDetails.DISTRICT_ID_copy;
                    NewSocietyRegisteredOfficeDetailsEntity.IsRural = _Entity.AddressDetails.IsRuralcopy;
                    NewSocietyRegisteredOfficeDetailsEntity.CITY_ID = _Entity.AddressDetails.CITY_IDcopy;
                    NewSocietyRegisteredOfficeDetailsEntity.WARD_ID = _Entity.AddressDetails.WARD_IDcopy;
                    NewSocietyRegisteredOfficeDetailsEntity.GP_ID = _Entity.AddressDetails.GP_IDcopy;
                    NewSocietyRegisteredOfficeDetailsEntity.VILLAGE_ID = _Entity.AddressDetails.VILLAGE_IDcopy;
                    NewSocietyRegisteredOfficeDetailsEntity.PinCode = _Entity.AddressDetails.PinCodecopy;
                    NewSocietyRegisteredOfficeDetailsEntity.CityTypes = _Entity.AddressDetails.CityTypes_copy;
                    NewSocietyRegisteredOfficeDetailsEntity.BLOCK_ID = _Entity.AddressDetails.BLOCK_ID_copy;
                    NewSocietyRegisteredOfficeDetailsEntity.DeleteFlag = 2;
                    NewSocietyRegisteredOfficeDetailsEntity.SSO_ID = SSOID;
                    NewSocietyRegisteredOfficeDetailsEntity.KIOSKCODE = _LoggedInUser.KIOSKCODE;
                    NewSocietyRegisteredOfficeDetailsEntity.EMitraTransactionId = _Entity.EMitraTransactionId;
                    NewSocietyRegisteredOfficeDetailsEntity.ServiceId = int.Parse(_LoggedInUser.ServiceId);
                    NewSocietyRegisteredOfficeDetailsEntity.SubmitStatus = _Entity.SaveStatus;
                    NewSocietyRegisteredOfficeDetailsEntity.UserType = _Entity.UserType;
                    LstSocietyRegisteredOfficeDetails.Add(NewSocietyRegisteredOfficeDetailsEntity);

                }
                if (_Entity.IsSameCEOAddress == true && _Entity.ActId == 7)//if diffrent address
                {
                    SocietyRegisteredOfficeDetailsEntity newobj = new SocietyRegisteredOfficeDetailsEntity();
                    newobj.IsSameCEOAddress = true;
                    newobj.AddressFor = 11;    //scretary id
                    newobj.SocietyIdEnc = _Entity.SocietyIDEnc;
                    newobj.HouseBuildingNo = _Entity.AddressDetails.HouseBuildingNoSecretary;
                    newobj.LaneStreetName = _Entity.AddressDetails.LaneStreetNameSecretary;
                    newobj.Locality = _Entity.AddressDetails.LocalitySecretary;
                    newobj.STATE_ID = _Entity.AddressDetails.STATE_IDSecretary;
                    newobj.DIVISION_ID = _Entity.AddressDetails.DIVISION_IDSecretary;
                    newobj.DISTRICT_ID = _Entity.AddressDetails.DISTRICT_IDSecretary;
                    newobj.IsRural = _Entity.AddressDetails.IsRuralSecretary;
                    newobj.CITY_ID = _Entity.AddressDetails.CITY_IDSecretary;
                    newobj.WARD_ID = _Entity.AddressDetails.WARD_IDSecretary;
                    newobj.GP_ID = _Entity.AddressDetails.GP_IDSecretary;
                    newobj.VILLAGE_ID = _Entity.AddressDetails.VILLAGE_IDSecretary;
                    newobj.PinCode = _Entity.AddressDetails.PinCodeSecretary;
                    newobj.CityTypes = _Entity.AddressDetails.CityTypesSecretary;
                    newobj.BLOCK_ID = _Entity.AddressDetails.BLOCK_IDSecretary;

                    newobj.DeleteFlag = 3;

                    newobj.SSO_ID = SSOID;
                    newobj.KIOSKCODE = _LoggedInUser.KIOSKCODE;
                    newobj.EMitraTransactionId = _Entity.EMitraTransactionId;
                    newobj.ServiceId = int.Parse(_LoggedInUser.ServiceId);
                    newobj.SubmitStatus = _Entity.SaveStatus;
                    newobj.UserType = _Entity.UserType;
                    LstSocietyRegisteredOfficeDetails.Add(newobj);

                }
                if (_Entity.IsSameCEOAddress == true && _Entity.ActId == 7)//if diffrent address
                {
                    SocietyRegisteredOfficeDetailsEntity newobjfortreasurer = new SocietyRegisteredOfficeDetailsEntity();
                    newobjfortreasurer.IsSameCEOAddress = false;
                    newobjfortreasurer.AddressFor = 12;    //treasurer id
                    newobjfortreasurer.SocietyIdEnc = _Entity.SocietyIDEnc;
                    newobjfortreasurer.HouseBuildingNo = _Entity.AddressDetails.HouseBuildingNoTreasurer;
                    newobjfortreasurer.LaneStreetName = _Entity.AddressDetails.LaneStreetNameTreasurer;
                    newobjfortreasurer.Locality = _Entity.AddressDetails.LocalityTreasurer;
                    newobjfortreasurer.STATE_ID = _Entity.AddressDetails.STATE_IDTreasurer;
                    newobjfortreasurer.DIVISION_ID = _Entity.AddressDetails.DIVISION_IDTreasurer;
                    newobjfortreasurer.DISTRICT_ID = _Entity.AddressDetails.DISTRICT_IDTreasurer;
                    newobjfortreasurer.IsRural = _Entity.AddressDetails.IsRuralTreasurer;
                    newobjfortreasurer.CITY_ID = _Entity.AddressDetails.CITY_IDTreasurer;
                    newobjfortreasurer.WARD_ID = _Entity.AddressDetails.WARD_IDTreasurer;
                    newobjfortreasurer.GP_ID = _Entity.AddressDetails.GP_IDTreasurer;
                    newobjfortreasurer.VILLAGE_ID = _Entity.AddressDetails.VILLAGE_IDTreasurer;
                    newobjfortreasurer.PinCode = _Entity.AddressDetails.PinCodeTreasurer;
                    newobjfortreasurer.CityTypes = _Entity.AddressDetails.CityTypesTreasurer;
                    newobjfortreasurer.BLOCK_ID = _Entity.AddressDetails.BLOCK_IDTreasurer;
                    newobjfortreasurer.UserType = _Entity.UserType;
                    newobjfortreasurer.DeleteFlag = 4;

                    newobjfortreasurer.SSO_ID = SSOID;
                    newobjfortreasurer.KIOSKCODE = _LoggedInUser.KIOSKCODE;
                    newobjfortreasurer.EMitraTransactionId = _Entity.EMitraTransactionId;
                    newobjfortreasurer.ServiceId = int.Parse(_LoggedInUser.ServiceId);
                    newobjfortreasurer.SubmitStatus = _Entity.SaveStatus;
                    LstSocietyRegisteredOfficeDetails.Add(newobjfortreasurer);
                }

                if (LstSocietyRegisteredOfficeDetails.Count > 0)
                    _Entity.SocietyRegisteredOfficeDetailsLst = LstSocietyRegisteredOfficeDetails;
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SocietyController", FunctionName = "SaveSocietyProfilingNew_AddressDetails#" + _LoggedInUser.SSOID }, ex);
                return CommonMessageFactory.Response("Provided input data is not in correct format.");
            }

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());

            try
            {
                _Entity.RegistrationNo = (_Entity.RegistrationNo == "undefined" || _Entity.RegistrationNo == null) ? "" : _Entity.RegistrationNo;

                if (_Entity.IsNewRegistration == false || _Entity.RegistrationNo.Length > 1)
                {
                    pathString = "~/Content/SocietyProfiling/";
                    pathString += Regex.Replace(_Entity.RegistrationNo.ToUpper().Trim(), "[^a-zA-Z0-9_]+", "");


                    if (_Entity.IsNewRegistration == false && !string.IsNullOrWhiteSpace(_Entity.OldRegistrationNo) && _Entity.RegistrationNo != _Entity.OldRegistrationNo)
                    {
                        flag = objSocietyModel.ValidateOldSocietyIsExist(_Entity.SocietyIDEnc, _Entity.RegistrationNo);
                        var pathStringOld = "~/Content/SocietyProfiling/";
                        pathStringOld += Regex.Replace(_Entity.OldRegistrationNo.ToUpper().Trim(), "[^a-zA-Z0-9_]+", "");

                        if (flag != "-5" && Directory.Exists(Server.MapPath(pathStringOld)) && !Directory.Exists(Server.MapPath(pathString)))
                        {
                            Directory.Move(Server.MapPath(pathStringOld), Server.MapPath(pathString));
                        }
                        else if (flag != "-5" && !Directory.Exists(Server.MapPath(pathStringOld)) && !Directory.Exists(Server.MapPath(pathString)))
                        {
                            Directory.CreateDirectory(Server.MapPath(pathString));
                        }

                    }
                    else if (!Directory.Exists(Server.MapPath(pathString)))
                    {
                        Directory.CreateDirectory(Server.MapPath(pathString));
                    }

                }
                //END



                //if (Request.Files != null && Request.Files.Count > 0 || _Entity.SocietyIDEnc.Length > 15 || _Entity.SaveStatus == 0)
                //{
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                _Entity.SSOID = SSOID;
                _Entity.UserType = _LoggedInUser.UserType;
                _Entity.CreatedBy = _Entity.UpdatedBy = _LoggedInUser.UserId;
                _Entity.KIOSKCODE = _LoggedInUser.KIOSKCODE;
                _Entity.AADHARNO = _LoggedInUser.AadharNo;
                if (_Entity.UserType == "K")
                {
                    _Entity.ServicesId = int.Parse(_LoggedInUser.ServiceId);
                    _Entity.IsActive = 0;
                }
                else if (_Entity.UserType == "C" || _Entity.UserType == "D")
                {
                    _Entity.ServicesId = 5530;//_Entity.IsNewRegistration == true ? 5530 : 5530;
                    _Entity.IsActive = 1;
                }
                _Entity.CreatedIP = _Entity.UpdatedIP = COMMON_FUNCTION.GetIpAddress();

                flag = objSocietyModel.InsertOldSocietyProfilling(_Entity);

                // }
                if (flag == "-1" || flag == "0")
                {
                    //System.IO.Directory.Delete(Server.MapPath(pathString));
                }
                else
                {
                    if (_Entity.IsMerge != "1")
                    {
                        if (_Entity.SaveStatus == 1)
                        {
                            var K = flag.Split('@');
                            ApplicationFlowModel _Modal = new ApplicationFlowModel();
                            _Modal.AddApplicationFlow(K[0], 26, "done profiling at DR office", 0);
                        }
                    }

                    //    if (K.Length > 1)
                    //    {
                    //        EmitraController objEC = new EmitraController();
                    //        PayServicesForSingleForm(K[1], _Entity.SaveStatus.ToString(), "5530", K[0], _Entity.KIOSKCODE, _Entity.SocietyName, _Entity.RegistrationNo);

                    //        //DataTable societyData = objSocietyModel.GetSocietyAppicationStatus(K[0]);
                    //        //if (societyData.Rows.Count > 0 && societyData.Rows[0]["IsFromSWC"].ToString() == "1")
                    //        //{                            
                    //        //    if (societyData.Rows.Count > 0)
                    //        //    {
                    //        //        COMMON_FUNCTION.UpdateSWSPartialSave(societyData.Rows[0]["RegistrationNo"].ToString(), Convert.ToInt32(societyData.Rows[0]["SocietyStatus"]));
                    //        //        COMMON_FUNCTION.UpdateSWSStatus(societyData.Rows[0]["RegistrationNo"].ToString(), Convert.ToInt32(societyData.Rows[0]["SocietyStatus"]));
                    //        //    }
                    //        //}
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {

                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SocietyController", FunctionName = "SaveSocietyProfiling#" + _LoggedInUser.SSOID }, ex);

            }

            ReturnJSON = "{\"data\":\"" + flag.ToString() + "\",\"societydirectory\":\"" + _Entity.RegistrationNo + "\"}";
            return ReturnJSON;

        }



        [HttpPost]
        public string UploadProfilingFiles_v1(bool IsNewRegistration, string RegistrationNo, string SocietyIDEnc, string OldRegistrationNo, int ActId, string filetype, string FileName)//, SocietyRegisteredOfficeDetailsEntity RegisteredOfficeDetailsEntity
        {
            string fileName = string.Empty;
            string SocietyDirectory = "NewRegistration";
            try
            {
                OldRegistrationNo = (OldRegistrationNo == "undefined" || OldRegistrationNo == null) ? "" : OldRegistrationNo;
                string flag = "0";
                string ReturnJSON = "";
                string pathString = "~/Content/NewRegistration/";
                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                RegistrationNo = (RegistrationNo == "undefined" || RegistrationNo == null) ? "" : RegistrationNo;

                if (IsNewRegistration == false || RegistrationNo.Length > 1)
                {
                    pathString = "~/Content/SocietyProfiling/";
                    pathString += Regex.Replace(RegistrationNo.ToUpper().Trim(), "[^a-zA-Z0-9_]+", "");
                    SocietyDirectory = Regex.Replace(RegistrationNo.ToUpper().Trim(), "[^a-zA-Z0-9_]+", "");

                    if (IsNewRegistration == false && !string.IsNullOrWhiteSpace(OldRegistrationNo) && RegistrationNo != OldRegistrationNo)
                    {
                        flag = objSocietyModel.ValidateOldSocietyIsExist(SocietyIDEnc, RegistrationNo);
                        var pathStringOld = "~/Content/SocietyProfiling/";
                        pathStringOld += Regex.Replace(OldRegistrationNo.ToUpper().Trim(), "[^a-zA-Z0-9_]+", "");
                        SocietyDirectory = Regex.Replace(RegistrationNo.ToUpper().Trim(), "[^a-zA-Z0-9_]+", "");
                        if (flag != "-5" && Directory.Exists(Server.MapPath(pathStringOld)) && !Directory.Exists(Server.MapPath(pathString)))
                        {
                            Directory.Move(Server.MapPath(pathStringOld), Server.MapPath(pathString));
                        }
                        else if (flag != "-5" && !Directory.Exists(Server.MapPath(pathStringOld)) && !Directory.Exists(Server.MapPath(pathString)))
                        {
                            Directory.CreateDirectory(Server.MapPath(pathString));
                        }

                    }
                    else if (!Directory.Exists(Server.MapPath(pathString)))
                    {
                        Directory.CreateDirectory(Server.MapPath(pathString));
                    }

                }
                //END

                foreach (string files in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[files];

                    //if (!COMMON_FUNCTION.CheckMimeType(file.InputStream, Path.GetExtension(file.FileName)))
                    //return "-10";
                    //file.InputStream.Seek(0, SeekOrigin.Begin);

                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    if (Path.GetExtension(file.FileName).ToUpper() == ".PDF")
                    {
                        string qrcode = COMMON_FUNCTION.GenerateQRCode(Path.Combine(Server.MapPath(pathString), fileName), 1);
                        COMMON_FUNCTION.AddStampOnPDF(file, Path.Combine(Server.MapPath(pathString), fileName), qrcode);
                    }
                    else
                    {
                        file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));
                    }

                    DataTable memberData = new DataTable();

                    if (files.ToString() == "flUploadMembers_List")
                    {

                        string result = "";
                        if (ActId == 7)
                        {
                            result = ValidateExcelSportAct(ref memberData, Path.Combine(Server.MapPath(pathString), fileName));
                        }
                        else
                        {
                            result = ValidateExcel(ref memberData, Path.Combine(Server.MapPath(pathString), fileName));
                        }

                        if (result != "OK")
                        {
                            return CommonMessageFactory.GetJsonStringResponse("true", "ErrorInMemberdata@" + result);
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        //(new Society()).AddEditSocietyDocument(new SocietyDocument() { DocumentURL = fileName, Isactive = true, SocietyId = SocietyId, DocumentName = FileName });
                    }
                }

            }
            catch (Exception ex)
            {

                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SocietyController", FunctionName = "UploadProfilingFiles_v1#" + _LoggedInUser.SSOID }, ex);

            }

            return SocietyDirectory + "/" + fileName;
        }


        [HttpPost]
        public string UploadSocietyOtherfiles(string RegistrationNo, string SocietyIDEnc, string OldRegistrationNo, int SocietyId, string FileName)//, SocietyRegisteredOfficeDetailsEntity RegisteredOfficeDetailsEntity
        {
            string fileName = string.Empty;
            string SocietyDirectory = "NewRegistration";
            try
            {
                OldRegistrationNo = (OldRegistrationNo == "undefined" || OldRegistrationNo == null) ? "" : OldRegistrationNo;
                string flag = "0";
                string ReturnJSON = "";
                string pathString = "~/Content/NewRegistration/";
                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                RegistrationNo = (RegistrationNo == "undefined" || RegistrationNo == null) ? "" : RegistrationNo;


                pathString = "~/Content/SocietyProfiling/";
                pathString += Regex.Replace(RegistrationNo.ToUpper().Trim(), "[^a-zA-Z0-9_]+", "");
                SocietyDirectory = Regex.Replace(RegistrationNo.ToUpper().Trim(), "[^a-zA-Z0-9_]+", "");

                if (!string.IsNullOrWhiteSpace(OldRegistrationNo) && RegistrationNo != OldRegistrationNo)
                {
                    flag = objSocietyModel.ValidateOldSocietyIsExist(SocietyIDEnc, RegistrationNo);
                    var pathStringOld = "~/Content/SocietyProfiling/";
                    pathStringOld += Regex.Replace(OldRegistrationNo.ToUpper().Trim(), "[^a-zA-Z0-9_]+", "");
                    SocietyDirectory = Regex.Replace(RegistrationNo.ToUpper().Trim(), "[^a-zA-Z0-9_]+", "");
                    if (flag != "-5" && Directory.Exists(Server.MapPath(pathStringOld)) && !Directory.Exists(Server.MapPath(pathString)))
                    {
                        Directory.Move(Server.MapPath(pathStringOld), Server.MapPath(pathString));
                    }
                    else if (flag != "-5" && !Directory.Exists(Server.MapPath(pathStringOld)) && !Directory.Exists(Server.MapPath(pathString)))
                    {
                        Directory.CreateDirectory(Server.MapPath(pathString));
                    }

                }
                else if (!Directory.Exists(Server.MapPath(pathString)))
                {
                    Directory.CreateDirectory(Server.MapPath(pathString));
                }


                //END

                foreach (string files in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[files];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);

                    if (Path.GetExtension(file.FileName).ToUpper() == ".PDF")
                    {
                        string qrcode = COMMON_FUNCTION.GenerateQRCode(Path.Combine(Server.MapPath(pathString), fileName), 1);
                        COMMON_FUNCTION.AddStampOnPDF(file, Path.Combine(Server.MapPath(pathString), fileName), qrcode);
                    }
                    else
                    {
                        file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));
                    }


                    (new Society()).AddEditSocietyDocument(new SocietyDocument() { DocumentURL = fileName, Isactive = true, SocietyId = SocietyId, DocumentType = "Other", DocumentName = FileName });

                }

            }
            catch (Exception ex)
            {

                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SocietyController", FunctionName = "UploadSocietyOtherfiles#" + _LoggedInUser.SSOID }, ex);

            }

            return SocietyDirectory + "/" + fileName;
        }



        [HttpPost]
        public string UploadProfilingFiles(string RegistrationNo, string OldRegistrationNo, string SocietyIDEnc, string filetype)
        {
            string fileName = "";
            try
            {
                string pathString = "";
                string flag = "";

                RegistrationNo = (RegistrationNo == "undefined" || RegistrationNo == null) ? "" : RegistrationNo;


                pathString = "~/Content/SocietyProfiling/";
                pathString += Regex.Replace(RegistrationNo.ToUpper().Trim(), "[^a-zA-Z0-9_]+", "").Replace("-", "");

                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                if (!string.IsNullOrWhiteSpace(OldRegistrationNo) && RegistrationNo != OldRegistrationNo)
                {
                    flag = objSocietyModel.ValidateOldSocietyIsExist(SocietyIDEnc, RegistrationNo);
                    var pathStringOld = "~/Content/SocietyProfiling/";
                    pathStringOld += Regex.Replace(OldRegistrationNo.ToUpper().Trim(), "[^a-zA-Z0-9_]+", "").Replace("-", ""); ;

                    if (flag != "-5" && Directory.Exists(Server.MapPath(pathStringOld)) && !Directory.Exists(Server.MapPath(pathString)))
                    {
                        Directory.Move(Server.MapPath(pathStringOld), Server.MapPath(pathString));
                    }
                    else if (flag != "-5" && !Directory.Exists(Server.MapPath(pathStringOld)) && !Directory.Exists(Server.MapPath(pathString)))
                    {
                        Directory.CreateDirectory(Server.MapPath(pathString));
                    }

                }
                else if (!Directory.Exists(Server.MapPath(pathString)))
                {
                    Directory.CreateDirectory(Server.MapPath(pathString));
                }


                //END

                foreach (string files in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[files];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    // file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));

                    if (Path.GetExtension(file.FileName).ToUpper() == ".PDF")
                    {
                        string qrcode = COMMON_FUNCTION.GenerateQRCode(Path.Combine(Server.MapPath(pathString), fileName), 1);
                        COMMON_FUNCTION.AddStampOnPDF(file, Path.Combine(Server.MapPath(pathString), fileName), qrcode);
                    }
                    else
                    {
                        file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));
                    }



                    DataTable memberData = new DataTable();
                    if (filetype == "excel")
                    {
                        var result = ValidateExcel(ref memberData, Path.Combine(Server.MapPath(pathString), fileName));
                        if (result != "OK")
                        {
                            return CommonMessageFactory.GetJsonStringResponse("true", "ErrorInMemberdata@" + result);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                fileName = "";
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SocietyController", FunctionName = "SaveSocietyProfiling#" + _LoggedInUser.SSOID }, ex);

            }

            return fileName;
        }


        [HttpPost]
        public string UploadSocietyAmendmentFile(string RegistrationNo, string OldRegistrationNo, string SocietyIDEnc)
        {

            string flag = "0";
            string fileName = string.Empty;
            string ReturnJSON = "";
            string pathString = "~/Content/NewRegistration/";
            DataTable memberData = new DataTable();
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());

            try
            {
                RegistrationNo = (RegistrationNo == "undefined" || RegistrationNo == null) ? "" : RegistrationNo;

                if (RegistrationNo.Length > 1)
                {
                    pathString = "~/Content/SocietyProfiling/";
                    pathString += Regex.Replace(RegistrationNo.ToUpper().Trim(), "[^a-zA-Z0-9_]+", "");


                    if (!string.IsNullOrWhiteSpace(OldRegistrationNo) && RegistrationNo != OldRegistrationNo)
                    {
                        flag = objSocietyModel.ValidateOldSocietyIsExist(SocietyIDEnc, RegistrationNo);
                        var pathStringOld = "~/Content/SocietyProfiling/";
                        pathStringOld += Regex.Replace(OldRegistrationNo.ToUpper().Trim(), "[^a-zA-Z0-9_]+", "");

                        if (flag != "-5" && Directory.Exists(Server.MapPath(pathStringOld)) && !Directory.Exists(Server.MapPath(pathString)))
                        {
                            Directory.Move(Server.MapPath(pathStringOld), Server.MapPath(pathString));
                        }
                        else if (flag != "-5" && !Directory.Exists(Server.MapPath(pathStringOld)) && !Directory.Exists(Server.MapPath(pathString)))
                        {
                            Directory.CreateDirectory(Server.MapPath(pathString));
                        }

                    }
                    else if (!Directory.Exists(Server.MapPath(pathString)))
                    {
                        Directory.CreateDirectory(Server.MapPath(pathString));
                    }

                }
                //END

                foreach (string files in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[files];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    //file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));

                    if (Path.GetExtension(file.FileName).ToUpper() == ".PDF")
                    {
                        string qrcode = COMMON_FUNCTION.GenerateQRCode(Path.Combine(Server.MapPath(pathString), fileName), 1);
                        COMMON_FUNCTION.AddStampOnPDF(file, Path.Combine(Server.MapPath(pathString), fileName), qrcode);
                    }
                    else
                    {
                        file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));
                    }



                    return fileName;
                }


            }
            catch (Exception ex)
            {

                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen", FunctionName = " JsonResult UploadSocietyAmendmentFile(string RegistrationNo, string OldRegistrationNo, string SocietyIDEnc)" }, ex);
                return "";
            }

            ReturnJSON = "{\"data\":\"" + flag.ToString() + "\",\"societydirectory\":\"" + RegistrationNo + "\"}";
            return ReturnJSON;

        }


        [HttpPost]
        public JsonResult GetSocietyOthersDocuments(int SocietyId)
        {
            return Json(new { DataList = (new Society()).GetSocietyDocument(SocietyId) }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeleteOtherDocument(int Id = 0)
        {
            return Json((new Society()).DeleteSocietyDocument(Id));
        }




        [HttpPost]
        public JsonResult PayServicesForSingleForm(string AppRequestID, string IsPermanentSave, string ServiceID, string SocietyIdEnc, string KioskCode, string SocietyName, string RegistrationNo)
        {
            int result = 0;
            string Result = "";
            EmitraModelFactory ObjEmitraModel = new EmitraModelFactory(new DBLogger());
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];


            if (_LoggedInUser.UserType == "K")
            {
                if (System.Web.HttpContext.Current.Session["ARequest"] != null && KioskCode.Length > 0)
                {
                    result = ObjEmitraModel.MakePayment(AppRequestID, SocietyName, "0", RegistrationNo, SocietyIdEnc, Convert.ToInt32(IsPermanentSave));

                    Result = result.ToString();
                    if (int.Parse(Result) > 0)
                    {
                        string FResult = "";
                        int Count = 0;
                        SocietyController obj = new SocietyController();

                        string str = obj.GetSocietyDetailsBySocietyId(SocietyIdEnc);
                        DataTable dt = (DataTable)JsonConvert.DeserializeObject(str, (typeof(DataTable)));
                        if (dt.Rows[0]["RegistrationNo"] == null || dt.Rows[0]["RegistrationNo"].ToString().Length == 0 || dt.Rows[0]["RegistrationNo"].ToString().Trim() == "")
                        {
                            FResult = obj.SaveSocietyFinalSubmition(SocietyIdEnc);
                            var REG = FResult.Split('#');
                            if (REG.Length > 1)
                            {
                                foreach (DataRow Dr in dt.Rows)
                                {
                                    Dr["SocietyDirectory"] = REG[1].ToString().Replace("/", "");
                                }
                            }
                        }


                        DataTable selectedTable = dt.AsEnumerable()
                        .Where(r => r.Field<string>("AddressFor") == "0" || r.Field<string>("AddressFor") == "")
                        .CopyToDataTable();


                        dt = selectedTable;
                        string pathString = "~/Content/SocietyProfiling/";
                        string SourceFilePath = "";
                        string TargetFilePath = "~/Content/SocietyProfiling/";
                        if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["SocietyDirectory"].ToString() != null && dt.Rows[0]["SocietyDirectory"].ToString() != "" && dt.Rows[0]["SocietyDirectory"].ToString() != "undefined")
                        {
                            foreach (DataRow Dr in dt.Rows)
                            {
                                if (Count == 0)
                                {
                                    pathString += Regex.Replace(dt.Rows[0]["SocietyDirectory"].ToString(), "[^a-zA-Z0-9_]+", "");
                                    if (!Directory.Exists(Server.MapPath(pathString)))
                                    {
                                        Directory.CreateDirectory(Server.MapPath(pathString));
                                    }

                                    if (Dr["RegistrationCertificateFile"] != null && !Dr["RegistrationCertificateFile"].ToString().Contains("undefined") && Dr["RegistrationCertificateFile"].ToString() != "")
                                    {
                                        if ((Dr["RegistrationCertificateFile"].ToString().Split('/')).Length > 3)
                                        {
                                            var temp = Dr["RegistrationCertificateFile"].ToString().Split('/');
                                            var TempRegistrationCertificateFile = temp[temp.Length - 1];
                                            SourceFilePath = "~/Content/NewRegistration/" + TempRegistrationCertificateFile;
                                            TargetFilePath = pathString + "/" + TempRegistrationCertificateFile;
                                            if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                                System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                        }
                                    }

                                    if (Dr["BylawsFile"] != null && !Dr["BylawsFile"].ToString().Contains("undefined") && Dr["BylawsFile"].ToString() != "")
                                    {
                                        if ((Dr["BylawsFile"].ToString().Split('/')).Length > 3)
                                        {
                                            var temp = Dr["BylawsFile"].ToString().Split('/');
                                            var TempBylawsFile = temp[temp.Length - 1];
                                            SourceFilePath = "~/Content/NewRegistration/" + TempBylawsFile;
                                            TargetFilePath = pathString + "/" + TempBylawsFile;
                                            if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                                System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                        }
                                    }

                                    if (Dr["RulesAndRegulationFile"] != null && !Dr["RulesAndRegulationFile"].ToString().Contains("undefined") && Dr["RulesAndRegulationFile"].ToString() != "")
                                    {
                                        if ((Dr["RulesAndRegulationFile"].ToString().Split('/')).Length > 3)
                                        {
                                            var temp = Dr["RulesAndRegulationFile"].ToString().Split('/');
                                            var TempRulesAndRegulationFile = temp[temp.Length - 1];
                                            SourceFilePath = "~/Content/NewRegistration/" + TempRulesAndRegulationFile;
                                            TargetFilePath = pathString + "/" + TempRulesAndRegulationFile;
                                            if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                                System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                        }
                                    }

                                    if (Dr["MemberListFile"] != null && !Dr["MemberListFile"].ToString().Contains("undefined") && Dr["MemberListFile"].ToString() != "")
                                    {
                                        if ((Dr["MemberListFile"].ToString().Split('/')).Length > 3)
                                        {
                                            var temp = Dr["MemberListFile"].ToString().Split('/');
                                            var TempMemberListFile = temp[temp.Length - 1];
                                            SourceFilePath = "~/Content/NewRegistration/" + TempMemberListFile;
                                            TargetFilePath = pathString + "/" + TempMemberListFile;
                                            if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                                System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                        }
                                    }


                                    if (Dr["WorkPlanEcoViabilityFile"] != null && !Dr["WorkPlanEcoViabilityFile"].ToString().Contains("undefined") && Dr["WorkPlanEcoViabilityFile"].ToString() != "")
                                    {
                                        if ((Dr["WorkPlanEcoViabilityFile"].ToString().Split('/')).Length > 3)
                                        {
                                            var temp = Dr["WorkPlanEcoViabilityFile"].ToString().Split('/');
                                            var TempWorkPlanEcoViabilityFile = temp[temp.Length - 1];
                                            SourceFilePath = "~/Content/NewRegistration/" + TempWorkPlanEcoViabilityFile;
                                            TargetFilePath = pathString + "/" + TempWorkPlanEcoViabilityFile;
                                            if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                                System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                        }
                                    }

                                    if (Dr["GeneralMeetingProcedureFile"] != null && !Dr["GeneralMeetingProcedureFile"].ToString().Contains("undefined") && Dr["GeneralMeetingProcedureFile"].ToString() != "")
                                    {
                                        if ((Dr["GeneralMeetingProcedureFile"].ToString().Split('/')).Length > 3)
                                        {
                                            var temp = Dr["GeneralMeetingProcedureFile"].ToString().Split('/');
                                            var TempGeneralMeetingProcedureFile = temp[temp.Length - 1];
                                            SourceFilePath = "~/Content/NewRegistration/" + TempGeneralMeetingProcedureFile;
                                            TargetFilePath = pathString + "/" + TempGeneralMeetingProcedureFile;
                                            if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                                System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                        }
                                    }

                                    if (Dr["BalanceSheetExcelFile"] != null && !Dr["BalanceSheetExcelFile"].ToString().Contains("undefined") && Dr["BalanceSheetExcelFile"].ToString() != "")
                                    {
                                        if ((Dr["BalanceSheetExcelFile"].ToString().Split('/')).Length > 3)
                                        {
                                            var temp = Dr["GeneralMeetingProcedureFile"].ToString().Split('/');
                                            var TempBalanceSheetExcelFile = temp[temp.Length - 1];
                                            SourceFilePath = "~/Content/NewRegistration/" + TempBalanceSheetExcelFile;
                                            TargetFilePath = pathString + "/" + TempBalanceSheetExcelFile;
                                            if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                                System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                        }
                                    }

                                    if (Dr["ObjectsFile"] != null && !Dr["ObjectsFile"].ToString().Contains("undefined") && Dr["ObjectsFile"].ToString() != "")
                                    {
                                        if ((Dr["ObjectsFile"].ToString().Split('/')).Length > 3)
                                        {
                                            var temp = Dr["ObjectsFile"].ToString().Split('/');
                                            var TempObjectsFile = temp[temp.Length - 1];
                                            SourceFilePath = "~/Content/NewRegistration/" + TempObjectsFile;
                                            TargetFilePath = pathString + "/" + TempObjectsFile;
                                            if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                                System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                        }
                                    }

                                    if (Dr["SLSAFileName"] != null && !Dr["SLSAFileName"].ToString().Contains("undefined") && Dr["SLSAFileName"].ToString() != "")
                                    {
                                        if ((Dr["SLSAFileName"].ToString().Split('/')).Length > 3)
                                        {
                                            var temp = Dr["SLSAFileName"].ToString().Split('/');
                                            var TempSLSAFileName = temp[temp.Length - 1];
                                            SourceFilePath = "~/Content/NewRegistration/" + TempSLSAFileName;
                                            TargetFilePath = pathString + "/" + TempSLSAFileName;
                                            if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                                System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                        }
                                    }
                                    if (Dr["RSSCFileName"] != null && !Dr["RSSCFileName"].ToString().Contains("undefined") && Dr["RSSCFileName"].ToString() != "")
                                    {
                                        if ((Dr["RSSCFileName"].ToString().Split('/')).Length > 3)
                                        {
                                            var temp = Dr["RSSCFileName"].ToString().Split('/');
                                            var TempRSSCFileName = temp[temp.Length - 1];
                                            SourceFilePath = "~/Content/NewRegistration/" + TempRSSCFileName;
                                            TargetFilePath = pathString + "/" + TempRSSCFileName;
                                            if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                                System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                        }
                                    }
                                    if (Dr["DLSCFileName"] != null && !Dr["DLSCFileName"].ToString().Contains("undefined") && Dr["DLSCFileName"].ToString() != "")
                                    {
                                        if ((Dr["DLSCFileName"].ToString().Split('/')).Length > 3)
                                        {
                                            var temp = Dr["DLSCFileName"].ToString().Split('/');
                                            var TempDLSCFileName = temp[temp.Length - 1];
                                            SourceFilePath = "~/Content/NewRegistration/" + TempDLSCFileName;
                                            TargetFilePath = pathString + "/" + TempDLSCFileName;
                                            if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                                System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                        }
                                    }
                                    if (Dr["SOCFileName"] != null && !Dr["SOCFileName"].ToString().Contains("undefined") && Dr["SOCFileName"].ToString() != "")
                                    {
                                        if ((Dr["SOCFileName"].ToString().Split('/')).Length > 3)
                                        {
                                            var temp = Dr["SOCFileName"].ToString().Split('/');
                                            var TempSOCFileName = temp[temp.Length - 1];
                                            SourceFilePath = "~/Content/NewRegistration/" + TempSOCFileName;
                                            TargetFilePath = pathString + "/" + TempSOCFileName;
                                            if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                                System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                        }
                                    }

                                }

                                if (Dr["Photo"] != null && Dr["Photo"].ToString() != "" && !Dr["Photo"].ToString().Contains("undefined") && !Dr["Photo"].ToString().Contains("Balnk"))
                                {
                                    var temp = Dr["Photo"].ToString().Split('/');
                                    if (temp.Length > 3)
                                    {
                                        var TempPhoto = temp[temp.Length - 1];
                                        SourceFilePath = "~/Content/NewRegistration/" + TempPhoto;
                                        TargetFilePath = pathString + "/" + TempPhoto;
                                        if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                            System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                    }
                                }
                                Count = Count + 1;
                            }
                        }
                    }

                }
            }
            else if (_LoggedInUser.UserType == "C" || _LoggedInUser.UserType == "D")
            {
                ILogger objILogger = new DBLogger();

                Result = ObjEmitraModel.UpdateEmitrTransation_SingleForm(AppRequestID, SocietyName, "0", RegistrationNo, SocietyIdEnc, ServiceID, false, IsPermanentSave);
                Result = Result.ToString();
                if (int.Parse(Result) > 0)
                {
                    string FResult = "";
                    int Count = 0;
                    SocietyController obj = new SocietyController();

                    string str = obj.GetSocietyDetailsBySocietyId(SocietyIdEnc);
                    DataTable dt = (DataTable)JsonConvert.DeserializeObject(str, (typeof(DataTable)));
                    if (dt.Rows[0]["RegistrationNo"] == null || dt.Rows[0]["RegistrationNo"].ToString().Length == 0 || dt.Rows[0]["RegistrationNo"].ToString().Trim() == "")
                    {
                        FResult = obj.SaveSocietyFinalSubmition(SocietyIdEnc);
                        var REG = FResult.Split('#');
                        if (REG.Length > 1)
                        {
                            foreach (DataRow Dr in dt.Rows)
                            {
                                Dr["SocietyDirectory"] = REG[1].ToString().Replace("/", "");
                            }
                        }
                    }

                    //  DataTable selectedTable = dt.AsEnumerable().Where(r => r.Field<string>("AddressFor") == "0" || r.Field<string>("AddressFor") == "").CopyToDataTable();

                    DataTable selectedTable = dt.AsEnumerable().Where(r =>
                    {
                        object addressFor = r["AddressFor"];
                        if (addressFor == null || addressFor == DBNull.Value) { return true; }
                        string addressForStr = addressFor.ToString();
                        return addressForStr == "0" || string.IsNullOrEmpty(addressForStr);
                    }).CopyToDataTable();



                    //DataTable selectedTable = dt.AsEnumerable()
                    //   .Where(r => r.Field<Int64?>("AddressFor") == 0 || r.Field<Int64?>("AddressFor") == null)
                    //   .CopyToDataTable();



                    dt = selectedTable;
                    string pathString = "~/Content/SocietyProfiling/";
                    string SourceFilePath = "";
                    string TargetFilePath = "~/Content/SocietyProfiling/";
                    if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["SocietyDirectory"].ToString() != null && dt.Rows[0]["SocietyDirectory"].ToString() != "" && dt.Rows[0]["SocietyDirectory"].ToString() != "undefined")
                    {
                        foreach (DataRow Dr in dt.Rows)
                        {
                            if (Count == 0)
                            {
                                pathString += Regex.Replace(dt.Rows[0]["SocietyDirectory"].ToString(), "[^a-zA-Z0-9_]+", "");
                                if (!Directory.Exists(Server.MapPath(pathString)))
                                {
                                    ApplicationFlowModel AppFlowModal = new ApplicationFlowModel();
                                    //AppFlowModal.SendMSG(SocietyIdEnc, -2);
                                    Directory.CreateDirectory(Server.MapPath(pathString));
                                }

                                if (Dr["RegistrationCertificateFile"] != null && !Dr["RegistrationCertificateFile"].ToString().Contains("undefined") && Dr["RegistrationCertificateFile"].ToString() != "")
                                {
                                    if ((Dr["RegistrationCertificateFile"].ToString().Split('/')).Length > 3)
                                    {
                                        var temp = Dr["RegistrationCertificateFile"].ToString().Split('/');
                                        var TempRegistrationCertificateFile = temp[temp.Length - 1];
                                        SourceFilePath = "~/Content/NewRegistration/" + TempRegistrationCertificateFile;
                                        TargetFilePath = pathString + "/" + TempRegistrationCertificateFile;
                                        if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                            System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));


                                        new SocietyModel(new DBLogger()).UploadMemberExcelData(TargetFilePath, TempRegistrationCertificateFile, SocietyIdEnc, Convert.ToInt32(Dr["ActId"].ToString()));

                                    }
                                }

                                if (Dr["BylawsFile"] != null && !Dr["BylawsFile"].ToString().Contains("undefined") && Dr["BylawsFile"].ToString() != "")
                                {
                                    if ((Dr["BylawsFile"].ToString().Split('/')).Length > 3)
                                    {
                                        var temp = Dr["BylawsFile"].ToString().Split('/');
                                        var TempBylawsFile = temp[temp.Length - 1];
                                        SourceFilePath = "~/Content/NewRegistration/" + TempBylawsFile;
                                        TargetFilePath = pathString + "/" + TempBylawsFile;
                                        if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                            System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                    }
                                }

                                if (Dr["RulesAndRegulationFile"] != null && !Dr["RulesAndRegulationFile"].ToString().Contains("undefined") && Dr["RulesAndRegulationFile"].ToString() != "")
                                {
                                    if ((Dr["RulesAndRegulationFile"].ToString().Split('/')).Length > 3)
                                    {
                                        var temp = Dr["RulesAndRegulationFile"].ToString().Split('/');
                                        var TempRulesAndRegulationFile = temp[temp.Length - 1];
                                        SourceFilePath = "~/Content/NewRegistration/" + TempRulesAndRegulationFile;
                                        TargetFilePath = pathString + "/" + TempRulesAndRegulationFile;
                                        if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                            System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                    }
                                }

                                if (Dr["MemberListFile"] != null && !Dr["MemberListFile"].ToString().Contains("undefined") && Dr["MemberListFile"].ToString() != "")
                                {
                                    if ((Dr["MemberListFile"].ToString().Split('/')).Length > 3)
                                    {
                                        var temp = Dr["MemberListFile"].ToString().Split('/');
                                        var TempMemberListFile = temp[temp.Length - 1];
                                        SourceFilePath = "~/Content/NewRegistration/" + TempMemberListFile;
                                        TargetFilePath = pathString + "/" + TempMemberListFile;
                                        if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                            System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                    }
                                }


                                if (Dr["WorkPlanEcoViabilityFile"] != null && !Dr["WorkPlanEcoViabilityFile"].ToString().Contains("undefined") && Dr["WorkPlanEcoViabilityFile"].ToString() != "")
                                {
                                    if ((Dr["WorkPlanEcoViabilityFile"].ToString().Split('/')).Length > 3)
                                    {
                                        var temp = Dr["WorkPlanEcoViabilityFile"].ToString().Split('/');
                                        var TempWorkPlanEcoViabilityFile = temp[temp.Length - 1];
                                        SourceFilePath = "~/Content/NewRegistration/" + TempWorkPlanEcoViabilityFile;
                                        TargetFilePath = pathString + "/" + TempWorkPlanEcoViabilityFile;
                                        if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                            System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                    }
                                }

                                if (Dr["GeneralMeetingProcedureFile"] != null && !Dr["GeneralMeetingProcedureFile"].ToString().Contains("undefined") && Dr["GeneralMeetingProcedureFile"].ToString() != "")
                                {
                                    if ((Dr["GeneralMeetingProcedureFile"].ToString().Split('/')).Length > 3)
                                    {
                                        var temp = Dr["GeneralMeetingProcedureFile"].ToString().Split('/');
                                        var TempGeneralMeetingProcedureFile = temp[temp.Length - 1];
                                        SourceFilePath = "~/Content/NewRegistration/" + TempGeneralMeetingProcedureFile;
                                        TargetFilePath = pathString + "/" + TempGeneralMeetingProcedureFile;
                                        if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                            System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                    }
                                }

                                if (Dr["BalanceSheetExcelFile"] != null && !Dr["BalanceSheetExcelFile"].ToString().Contains("undefined") && Dr["BalanceSheetExcelFile"].ToString() != "")
                                {
                                    if ((Dr["BalanceSheetExcelFile"].ToString().Split('/')).Length > 3)
                                    {
                                        var temp = Dr["BalanceSheetExcelFile"].ToString().Split('/');
                                        var TempBalanceSheetExcelFile = temp[temp.Length - 1];
                                        SourceFilePath = "~/Content/NewRegistration/" + TempBalanceSheetExcelFile;
                                        TargetFilePath = pathString + "/" + TempBalanceSheetExcelFile;
                                        if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                            System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                    }
                                }

                                if (Dr["ObjectsFile"] != null && !Dr["ObjectsFile"].ToString().Contains("undefined") && Dr["ObjectsFile"].ToString() != "")
                                {
                                    if ((Dr["ObjectsFile"].ToString().Split('/')).Length > 3)
                                    {
                                        var temp = Dr["ObjectsFile"].ToString().Split('/');
                                        var TempObjectsFile = temp[temp.Length - 1];
                                        SourceFilePath = "~/Content/NewRegistration/" + TempObjectsFile;
                                        TargetFilePath = pathString + "/" + TempObjectsFile;
                                        if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                            System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                    }
                                }

                                if (Dr["SLSAFileName"] != null && !Dr["SLSAFileName"].ToString().Contains("undefined") && Dr["SLSAFileName"].ToString() != "")
                                {
                                    if ((Dr["SLSAFileName"].ToString().Split('/')).Length > 3)
                                    {
                                        var temp = Dr["SLSAFileName"].ToString().Split('/');
                                        var TempSLSAFileName = temp[temp.Length - 1];
                                        SourceFilePath = "~/Content/NewRegistration/" + TempSLSAFileName;
                                        TargetFilePath = pathString + "/" + TempSLSAFileName;
                                        if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                            System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                    }
                                }
                                if (Dr["RSSCFileName"] != null && !Dr["RSSCFileName"].ToString().Contains("undefined") && Dr["RSSCFileName"].ToString() != "")
                                {
                                    if ((Dr["RSSCFileName"].ToString().Split('/')).Length > 3)
                                    {
                                        var temp = Dr["RSSCFileName"].ToString().Split('/');
                                        var TempRSSCFileName = temp[temp.Length - 1];
                                        SourceFilePath = "~/Content/NewRegistration/" + TempRSSCFileName;
                                        TargetFilePath = pathString + "/" + TempRSSCFileName;
                                        if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                            System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                    }
                                }
                                if (Dr["DLSCFileName"] != null && !Dr["DLSCFileName"].ToString().Contains("undefined") && Dr["DLSCFileName"].ToString() != "")
                                {
                                    if ((Dr["DLSCFileName"].ToString().Split('/')).Length > 3)
                                    {
                                        var temp = Dr["DLSCFileName"].ToString().Split('/');
                                        var TempDLSCFileName = temp[temp.Length - 1];
                                        SourceFilePath = "~/Content/NewRegistration/" + TempDLSCFileName;
                                        TargetFilePath = pathString + "/" + TempDLSCFileName;
                                        if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                            System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                    }
                                }
                                if (Dr["SOCFileName"] != null && !Dr["SOCFileName"].ToString().Contains("undefined") && Dr["SOCFileName"].ToString() != "")
                                {
                                    if ((Dr["SOCFileName"].ToString().Split('/')).Length > 3)
                                    {
                                        var temp = Dr["SOCFileName"].ToString().Split('/');
                                        var TempSOCFileName = temp[temp.Length - 1];
                                        SourceFilePath = "~/Content/NewRegistration/" + TempSOCFileName;
                                        TargetFilePath = pathString + "/" + TempSOCFileName;
                                        if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                            System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                    }
                                }

                            }

                            if (Dr["Photo"] != null && Dr["Photo"].ToString() != "" && !Dr["Photo"].ToString().Contains("undefined") && !Dr["Photo"].ToString().Contains("Balnk"))
                            {
                                var temp = Dr["Photo"].ToString().Split('/');
                                if (temp.Length > 3)
                                {
                                    var TempPhoto = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempPhoto;
                                    TargetFilePath = pathString + "/" + TempPhoto;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                    {
                                        if (!System.IO.File.Exists(Server.MapPath(TargetFilePath)))
                                            System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                    }

                                }
                            }
                            Count = Count + 1;
                        }
                    }
                }
            }

            return Json(new { status = "OK", UserType = _LoggedInUser.UserType, result = Result, SocietyIdEnc = SocietyIdEnc }, JsonRequestBehavior.AllowGet);

        }




        [HttpPost]
        [ActionName("GetMemberAmendmentBySocietyId")]
        public string GetMemberAmendmentBySocietyId(string SocietyId, int AmendmentId = 0)
        {
            if (string.IsNullOrEmpty(SocietyId))
                return "{}";

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            DataTable DT = new DataTable();
            //if (EMitraTransactionId == 0)
            //{
            DT = objSocietyModel.GetMemberAmendmentBySocietyId(SocietyId, AmendmentId);
            //  }
            // else
            //{
            //  DT = objSocietyModel.GetSocietyDetailsBySocietyIdFormTempTbl(SocietyId, EMitraTransactionId);
            //}
            //DT = objSocietyModel.GetSocietyDetailsBySocietyId(SocietyId);
            //return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
            //var encAadharNos = COMMON_FUNCTION.Encrypt("803026440586");
            //var decAadharNos = COMMON_FUNCTION.Decrypt(encAadharNos);


            foreach (DataRow dr in DT.Rows) // search whole table
            {

                if (dr["AadharNo"].ToString().Length > 12)
                {

                    dr["AadharNo"] = COMMON_FUNCTION.Decrypt(dr["AadharNo"].ToString());
                    dr["AadharNo2"] = dr["AadharNo"].ToString();
                    var AadharNo = dr["AadharNo"].ToString();

                    var firstDigits = AadharNo.Substring(0, 8);
                    var lastDigits = AadharNo.Substring(AadharNo.Length - 4, 4);
                    var requiredMask = new String('X', AadharNo.Length - lastDigits.Length);
                    var maskedString = string.Concat(requiredMask, lastDigits);
                    var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0");
                    dr["AadharNo"] = maskedString;
                }
                //if (dr["Photo"].ToString().Length > 12)
                //{
                //    if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(dr["Photo"].ToString())))
                //    {

                //    }
                //    else
                //    {
                //        var photo = dr["Photo"].ToString().Split('/');
                //        var SocietyDirectory = dr["SocietyDirectory"].ToString();
                //        string PhName = photo[(photo.Length - 1)].ToString();
                //        if (dr["SocietyDirectory"].ToString() != null && dr["SocietyDirectory"].ToString() != "" && (dr["SocietyDirectory"].ToString()).Length > 1)
                //        {
                //            string pathString = "~/Content/SocietyProfiling/" + dr["SocietyDirectory"].ToString();
                //            // if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                //            // {
                //            CitizenController obj = new CitizenController();
                //            obj.UpdatePhotosAndDoc(int.Parse(dr["SocietyID"].ToString()));

                //            // }

                //        }

                //        //if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("../Content/NewRegistration/" + PhName)))
                //        //{
                //        //    dr["Photo"] = "../Content/NewRegistration/" + PhName;
                //        //    if (dr["SocietyDirectory"].ToString() != null && dr["SocietyDirectory"].ToString() != "" && (dr["SocietyDirectory"].ToString()).Length > 1)
                //        //    {
                //        //        string SourceFilePath = "~/Content/NewRegistration/" + PhName;
                //        //        string pathString = "~/Content/SocietyProfiling/" + dr["SocietyDirectory"].ToString() + "/" + PhName;
                //        //        if (System.IO.File.Exists(Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(Server.MapPath(pathString)))
                //        //        {
                //        //            System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(pathString));
                //        //            dr["Photo"] = "../Content/SocietyProfiling/" + SocietyDirectory + "/" + PhName;
                //        //        }

                //        //    }
                //        //}
                //        else if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("../Content/SocietyProfiling/" + SocietyDirectory + "/" + PhName)) && (SocietyDirectory != null && SocietyDirectory != "" && SocietyDirectory.Length > 1))
                //        {
                //            dr["Photo"] = "../Content/SocietyProfiling/" + SocietyDirectory + "/" + PhName;
                //        }
                //        //else if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("../Content/SocietyProfiling/" + PhName)))
                //        //{
                //        //    string SourceFilePath = "~/Content/SocietyProfiling/" + PhName;
                //        //    string pathString = "~/Content/SocietyProfiling/" + dr["SocietyDirectory"].ToString() + "/" + PhName;
                //        //    if (dr["SocietyDirectory"].ToString() != null && dr["SocietyDirectory"].ToString() != "" && (dr["SocietyDirectory"].ToString()).Length > 1)
                //        //    {
                //        //        if (System.IO.File.Exists(Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(Server.MapPath(pathString)))
                //        //        {
                //        //            System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(pathString));
                //        //            dr["Photo"] = "../Content/SocietyProfiling/" + SocietyDirectory + "/" + PhName;
                //        //        }                               

                //        //    }
                //        //}
                //        else
                //            dr["Photo"] = "../Content/Images/Balnk.png";
                //    }

                //}

            }

            return COMMON_FUNCTION.DataTableToJSONString(DT);

        }

        [HttpPost]
        [ActionName("GetMemberListAmendmentBySocietyId")]
        public string GetMemberListAmendmentBySocietyId(string SocietyId)
        {
            if (string.IsNullOrEmpty(SocietyId))
                return "{}";

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            DataTable DT = new DataTable();
            //if (EMitraTransactionId == 0)
            //{
            DT = objSocietyModel.GetMemberListAmendmentBySocietyId(SocietyId);
            //  }
            // else
            //{
            //  DT = objSocietyModel.GetSocietyDetailsBySocietyIdFormTempTbl(SocietyId, EMitraTransactionId);
            //}
            //DT = objSocietyModel.GetSocietyDetailsBySocietyId(SocietyId);
            //return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
            foreach (DataRow dr in DT.Rows) // search whole table
            {
                if (dr["AadharNo"].ToString().Length > 12)
                {
                    dr["AadharNo"] = COMMON_FUNCTION.Decrypt(dr["AadharNo"].ToString());
                    dr["AadharNo2"] = dr["AadharNo"].ToString();
                    var AadharNo = dr["AadharNo"].ToString();

                    var firstDigits = AadharNo.Substring(0, 8);
                    var lastDigits = AadharNo.Substring(AadharNo.Length - 4, 4);
                    var requiredMask = new String('X', AadharNo.Length - lastDigits.Length);
                    var maskedString = string.Concat(requiredMask, lastDigits);
                    var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0");
                    dr["AadharNo"] = maskedString;
                }
                //if (dr["Photo"].ToString().Length > 12)
                //{
                //    if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(dr["Photo"].ToString())))
                //    {

                //    }
                //    else
                //    {
                //        var photo = dr["Photo"].ToString().Split('/');
                //        var SocietyDirectory = dr["SocietyDirectory"].ToString();
                //        string PhName = photo[(photo.Length - 1)].ToString();
                //        if (dr["SocietyDirectory"].ToString() != null && dr["SocietyDirectory"].ToString() != "" && (dr["SocietyDirectory"].ToString()).Length > 1)
                //        {
                //            string pathString = "~/Content/SocietyProfiling/" + dr["SocietyDirectory"].ToString();
                //            // if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                //            // {
                //            CitizenController obj = new CitizenController();
                //            obj.UpdatePhotosAndDoc(int.Parse(dr["SocietyID"].ToString()));

                //            // }

                //        }

                //        //if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("../Content/NewRegistration/" + PhName)))
                //        //{
                //        //    dr["Photo"] = "../Content/NewRegistration/" + PhName;
                //        //    if (dr["SocietyDirectory"].ToString() != null && dr["SocietyDirectory"].ToString() != "" && (dr["SocietyDirectory"].ToString()).Length > 1)
                //        //    {
                //        //        string SourceFilePath = "~/Content/NewRegistration/" + PhName;
                //        //        string pathString = "~/Content/SocietyProfiling/" + dr["SocietyDirectory"].ToString() + "/" + PhName;
                //        //        if (System.IO.File.Exists(Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(Server.MapPath(pathString)))
                //        //        {
                //        //            System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(pathString));
                //        //            dr["Photo"] = "../Content/SocietyProfiling/" + SocietyDirectory + "/" + PhName;
                //        //        }

                //        //    }
                //        //}
                //        else if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("../Content/SocietyProfiling/" + SocietyDirectory + "/" + PhName)) && (SocietyDirectory != null && SocietyDirectory != "" && SocietyDirectory.Length > 1))
                //        {
                //            dr["Photo"] = "../Content/SocietyProfiling/" + SocietyDirectory + "/" + PhName;
                //        }
                //        //else if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("../Content/SocietyProfiling/" + PhName)))
                //        //{
                //        //    string SourceFilePath = "~/Content/SocietyProfiling/" + PhName;
                //        //    string pathString = "~/Content/SocietyProfiling/" + dr["SocietyDirectory"].ToString() + "/" + PhName;
                //        //    if (dr["SocietyDirectory"].ToString() != null && dr["SocietyDirectory"].ToString() != "" && (dr["SocietyDirectory"].ToString()).Length > 1)
                //        //    {
                //        //        if (System.IO.File.Exists(Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(Server.MapPath(pathString)))
                //        //        {
                //        //            System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(pathString));
                //        //            dr["Photo"] = "../Content/SocietyProfiling/" + SocietyDirectory + "/" + PhName;
                //        //        }                               

                //        //    }
                //        //}
                //        else
                //            dr["Photo"] = "../Content/Images/Balnk.png";
                //    }

                //}

            }

            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }


        [HttpPost]
        [ActionName("SaveSocietyProfilingEdit")]
        public string SaveSocietyProfilingEdit(SocietyRegistrationEntity _Entity)//, SocietyRegisteredOfficeDetailsEntity RegisteredOfficeDetailsEntity
        {
            _Entity.CreatedBy = _Entity.UpdatedBy = _LoggedInUser.UserId;
            string flag = "0";
            string fileName = string.Empty;
            string ReturnJSON = "";
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            flag = objSocietyModel.UpdateSocietyProfileBy_SA_RAO_CAO(_Entity);
            ReturnJSON = "{\"data\":\"" + flag.ToString() + "\",\"societydirectory\":\"" + _Entity.RegistrationNo + "\"}";
            return ReturnJSON;

        }

        public ActionResult UserRatingList()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAllUserRatingList(DataPagingModel Details, string Rating, string ActId)
        {
            DataTable Dt = null;
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            Dt = objSocietyModel.GetAllUserRatingList(Details, Rating, ActId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(Dt), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ActionName("CheckNewRequestFromSWCS")]
        public int CheckNewRequestFromSWCS(string SWSID, string SSOID)
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return objSocietyModel.CheckNewRequestFromSWCS(SWSID);
        }
        [HttpPost]
        public int Updateverifydata(int BankDetailId)
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return objSocietyModel.Bankverification(BankDetailId);
        }








        public ActionResult InspactorTargets()
        {
            return View();
        }

        [HttpPost]
        public string UploadinInspactorFile()
        {
            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    string fileDirectory = Server.MapPath("~/Content/InspactorDocument");

                    if (!System.IO.Directory.Exists(fileDirectory))
                    {
                        System.IO.Directory.CreateDirectory(fileDirectory);
                    }
                    foreach (string files in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[files];
                        fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);

                        file.SaveAs(Path.Combine(fileDirectory, fileName));
                        //MemberData[0].Photo = fileName;
                    }
                }
                catch (System.Exception ex)
                {
                    return "";
                }
            }

            return fileName;

        }

        [HttpPost]
        [ActionName("SaveInspactorTargets")]
        public int SaveInspactorTargets(List<InspactorTargets> InspactorTargets)
        {
            int Result1 = 0;
            try
            {
                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                Result1 = objSocietyModel.InsertInspactorTargets(InspactorTargets);

                return Result1;
            }
            catch (Exception ex)
            {

            }
            return Result1;

        }

        public JsonResult GetInspactorTargetsCount(string InspactorId, string GSSSStatusId)
        {
            DataTable Dt = null;
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            Dt = objSocietyModel.GetInspactorTargetsCount(InspactorId, GSSSStatusId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(Dt) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("GetInspactorTargetsDetails")]
        public JsonResult GetInspactorTargetsDetails(DataPagingModel Details, string InspactorId, string StatusId, string GSSSStatusId, int Flag, int Month = -1)
        {
            DataTable Dt = null;
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            Dt = objSocietyModel.GetInspactorTargetsDetails(Details, InspactorId, StatusId, GSSSStatusId, Flag, Month);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(Dt), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InspactorGSS()
        {
            return View();
        }

        [HttpPost]
        [ActionName("SaveInspactorstausDetails")]
        public int SaveInspactorstausDetails(InspactorStatusDetails _InspactorStatusDetails)
        {
            int Result1 = 0;

            try
            {
                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                Result1 = objSocietyModel.SaveInspactorstausDetails(_InspactorStatusDetails);

                return Result1;
            }
            catch (Exception ex)
            {

            }
            return Result1;

        }

        public JsonResult GetInspectorStatusDetails(string InspactorTargetsId)
        {
            DataTable Dt = null;
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            Dt = objSocietyModel.GetInspectorStatusDetails(InspactorTargetsId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(Dt) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("SaveInspactorStatusDR")]
        public int SaveInspactorStatusDR(List<InspactorStatusDetails> InspactorStatusDetails, string Remarks, string Inspectorfile, string status, string TXNno, string SenctionDate, string OTP = "")
        {
            int Result1 = 0;
            string PdfPath = "";
            string fileName = "";
            string IDs = "";
            ResultMessage otpResults = new ResultMessage();
            try
            {
                if (status == "0")
                {
                    return Result1;
                }

                //PdfPath = GSSTargetSanctionLetter(TXNno, InspactorStatusDetails);
                if (status == "4")
                {
                    fileName = "GSSTargetSanction_" + Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace("_", "").ToUpper() + ".pdf";
                    Inspectorfile = fileName;

                    if (InspactorStatusDetails.Count > 1)
                    {
                        foreach (var item in InspactorStatusDetails)
                        {
                            IDs += item.InspactorTargetsId + ",";
                        }
                    }
                }

                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                Result1 = objSocietyModel.SaveInspactorStatusDR(InspactorStatusDetails, Remarks, Inspectorfile, status, TXNno, SenctionDate, IDs);



                if ((Result1 == 1 && status == "4" && (TXNno != "" || Request.Url.Host.Contains("localhost") || Request.Url.Host.Contains("devitsandbox"))))
                {
                    ResultMessage otpResult = new ResultMessage();


                    if (!Request.Url.Host.Contains("localhost") && !Request.Url.Host.Contains("devitsandbox"))
                    {
                        otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXNno);
                    }
                    else
                    {
                        otpResult.status = "1"; otpResult.data = TXNno;
                    }
                    if (otpResult.status == "1")
                    {

                        PdfPath = GSSTargetSanctionLetter(TXNno, fileName, InspactorStatusDetails);

                        if (PdfPath.Contains("Fail"))
                        {

                            string InspactorTargetsIds = (IDs == "") ? InspactorStatusDetails[0].InspactorTargetsId : IDs;
                            var Result2 = objSocietyModel.RevertLetter(InspactorTargetsIds);

                            return 0;
                        }

                        return Result1;

                    }
                    return Result1;
                }
            }
            catch (Exception ex)
            {

            }
            return Result1;
        }

        [HttpPost]
        [ActionName("SaveAGMStatusDR")]
        public int SaveAGMStatusDR(List<InspactorStatusDetails> InspactorStatusDetails, string Remarks, string Inspectorfile, string status, string SenctionDate, string MultiInspactorIds, int DistrictId, int DivisionId, int LevelId, int OfficeId)
        {
            int Result1 = 0;
            //string PdfPath = "";
            //string fileName = "";
            //string IDs = "";
            ResultMessage otpResults = new ResultMessage();
            try
            {
                if (status == "0")
                {
                    return Result1;
                }

                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                Result1 = objSocietyModel.SaveAGMStatusDR(InspactorStatusDetails, Remarks, Inspectorfile, status, SenctionDate, MultiInspactorIds, DistrictId, DivisionId, LevelId, OfficeId);

            }
            catch (Exception ex)
            {

            }
            return Result1;
        }

        //[HttpPost]
        //[ActionName("SaveGNStatus")]
        //public int SaveGNStatus(List<InspactorStatusDetails> InspactorStatusDetails, string Remarks, string agmTime, string status, string agmDate, string agmPlace)
        //{
        //    int Result1 = 0;
        //    //string PdfPath = "";
        //    string fileName = "";
        //    string IDs = "";
        //    ResultMessage otpResults = new ResultMessage();
        //    try
        //    {
        //        if (status == "0")
        //        {
        //            return Result1;
        //        }
        //        fileName = GenerateAGMNotice(agmTime, agmDate, agmPlace);
        //        SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
        //        Result1 = objSocietyModel.SaveGNStatus(InspactorStatusDetails, Remarks, agmTime, status, agmDate, agmPlace, fileName);

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return Result1;
        //}


        [HttpPost]
        [ActionName("SaveGNStatus")]
        public int SaveGNStatus(List<InspactorStatusDetails> InspactorStatusDetails, string Remarks, string status, string agmTime, string agmDate, string agmPlace, string TXNno, string OTP)
        {
            int Result1 = 0;

            try
            {
                if (status == "0")
                {
                    return 0;
                }

                // OTP VERIFY

                ResultMessage otpResult = new ResultMessage();

                if (!Request.Url.Host.Contains("localhost") && !Request.Url.Host.Contains("devitsandbox"))
                {
                    otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXNno);
                }
                else
                {
                    otpResult.status = "1"; // dev bypass
                }
                if (otpResult.status != "1")
                {
                    return -1;
                }

                var targetId = InspactorStatusDetails.FirstOrDefault()?.InspactorTargetsId;

                string pdfPath = GenerateAGMNotice(agmTime, agmDate, agmPlace, TXNno, targetId);

                // SAVE 
                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());

                Result1 = objSocietyModel.SaveGNStatus(InspactorStatusDetails, Remarks, agmTime, status, agmDate, agmPlace, pdfPath);

                if (pdfPath == "Fail")
                    return 0;

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        //new jaya
        [HttpGet]
        [ActionName("GenerateAGMNotice")]
        public string GenerateAGMNotice(string AGMTime = "", string AGMDate = "", string AGMPlace = "", string TXNno = "", string targetId = "")
        {
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {
                    string blockName = "";
                    string gpName = "";
                    string districtName = "";
                    string NewGSSName = "";
                    string PrecentNameGSS = "";
                    string formattedSanctionDate = "";
                    string CCBBranchName = "";
                    string RegNo = "";

                    DataTable Dt = null;
                    SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                    Dt = objSocietyModel.GetInspectorDetailsForCertificate(targetId);
                    if (Dt != null && Dt.Rows.Count > 0)
                    {
                        blockName = Dt.Rows[0]["BLOCK_ENG"]?.ToString();
                        gpName = Dt.Rows[0]["GP_ENG"]?.ToString();
                        districtName = Dt.Rows[0]["DISTRICT_ENG"]?.ToString();
                        NewGSSName = Dt.Rows[0]["NewGSSName"]?.ToString();
                        PrecentNameGSS = Dt.Rows[0]["PrecentNameGSS"]?.ToString();
                        CCBBranchName = Dt.Rows[0]["BranchNameEn"]?.ToString();

                        if (Dt.Rows[0]["SanctionDate"] != DBNull.Value)
                        {
                            DateTime dt = (DateTime)Dt.Rows[0]["SanctionDate"];
                            formattedSanctionDate = dt.ToString("dd/MM/yyyy");
                        }

                        RegNo = $"AGM/{districtName}/{blockName}/2026/00001";

                    }

                    StringBuilder strbld = new StringBuilder();

                    var file = Path.Combine(Server.MapPath("~/Template"), "pacs_generate_agm_notice.html");

                    strbld.Append(System.IO.File.ReadAllText(file));

                    strbld.Replace("#AGMTime#", AGMTime)
                          .Replace("#AGMDate#", AGMDate)
                          .Replace("#AGMPlace#", AGMPlace)
                          .Replace("#BlockName#", blockName)
                          .Replace("#GPName#", gpName)
                          .Replace("#DistrictName#", districtName)
                          .Replace("#NewGSSName#", NewGSSName)
                          .Replace("#SanctionDate#", formattedSanctionDate)
                          .Replace("#PrecentNameGSS#", PrecentNameGSS)
                          .Replace("#CCBBranchName#", CCBBranchName)
                          .Replace("#CurrentDate#", DateTime.Now.ToString("dd/MM/yyyy"))
                          .Replace("#RegNo#", RegNo);

                    var agmNoticePath = Server.MapPath("~/Content/AGMNotice/GSSProfiling/");

                    if (!Directory.Exists(agmNoticePath))
                    {
                        Directory.CreateDirectory(agmNoticePath);
                    }

                    var fileName = "AGMGenerateNLetter_" + Guid.NewGuid().ToString().Replace("-", "").ToUpper() + ".pdf";

                    var filePath = Path.Combine(agmNoticePath, fileName);

                    StringBuilder sb1 = new StringBuilder();
                    sb1.Append(strbld.ToString());

                    CommonPdfGenerate(sb1, filePath);

                    string pdfPath = fileName;

                    string result = "";

                    // eSIGN CALL (MAIN PART)
                    if (!Request.Url.Host.Contains("localhost") && !Request.Url.Host.Contains("devitsandbox"))
                    {
                        var esignResult = COMMON_FUNCTION.EsinghPDF(TXNno, pdfPath, "INS", "Inspactor");

                        result = esignResult.status;
                    }
                    else
                    {
                        result = pdfPath;
                    }

                    return result;
                }
                else
                {
                    return "Fail";
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "SocietyController", FunctionName = "GenerateAGMNotice catch" }, ex);
                return "Fail";
            }
        }

        //old ravi
        [HttpGet]
        [ActionName("GenerateAGMNotice1")]
        public string GenerateAGMNotice1(string AGMTime = "", string AGMDate = "", string AGMPlace = "", string TXNno = "")
        {
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {

                    StringBuilder strbld = new StringBuilder();
                    var file = System.IO.Path.Combine(Server.MapPath("~/Template"), "pacs_generate_agm_notice.html");

                    strbld.Append(System.IO.File.ReadAllText(file));
                    strbld.Replace("#AGMTime#", AGMTime)
                   .Replace("#AGMDate#", AGMDate)
                   .Replace("#AGMPlace#", AGMPlace);

                    var agmNoticePath = Server.MapPath("~/Content/AGMNotice/GSSProfiling/");

                    string devFontSize = "17px";

                    if (!System.IO.Directory.Exists(agmNoticePath))
                    {
                        System.IO.Directory.CreateDirectory(agmNoticePath);
                    }

                    var fileName = "AGMGenerateNLetter_" + Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace("_", "").ToUpper() + ".pdf";
                    var filePath = agmNoticePath + "/" + fileName;

                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(strbld.ToString());

                    CommonPdfGenerate(sb1, filePath);

                    var pdfPath = fileName;

                    return pdfPath;
                }
                else
                {
                    return "Fail";
                    // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "hideloading();popupSessionExpier();", true);
                }

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "SocietyController", FunctionName = "GenerateAGMNotice catch " }, ex);

                return "Fail";
            }
        }


        public string CommonPdfGenerate(System.Text.StringBuilder sb, string filePath)
        {
            string wkhtmlPath = HostingEnvironment.MapPath("~/bin/wkhtmltopdf.exe");
            string fontPath = new Uri(HostingEnvironment.MapPath("~/fonts/lohit-Devanagari.ttf")).AbsoluteUri;

            try
            {
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
                                    {sb}
                    </body>
                    </html>";

                var pdf = PDFgenerator.GeneratePdfFromHtml(finalHtml, wkhtmlPath);
                var fileName = $"AGMGenerateNLetter_{Guid.NewGuid().ToString("N").ToUpper()}.pdf";


                string folderPath = Path.Combine(HostingEnvironment.MapPath("~/Content/AGMNotice/GSSProfiling/"));

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                System.IO.File.WriteAllBytes(filePath, pdf);
                return filePath;
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpPost]
        [ActionName("SaveAGMFStatus")]
        public int SaveAGMFStatus(List<InspactorStatusDetails> InspactorStatusDetails, string Remarks, string IsAGMConduct, string status, string agmConductDate, string agmReasons, string fileNames)
        {
            int Result1 = 0;
            //string PdfPath = "";
            string fileName = "";
            string statusID = "";
            ResultMessage otpResults = new ResultMessage();
            try
            {
                if (IsAGMConduct == "0")
                {
                    statusID = "18";
                }
                else { statusID = status; }

                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                Result1 = objSocietyModel.SaveAGMFStatus(InspactorStatusDetails, Remarks, IsAGMConduct, statusID, agmConductDate, agmReasons, fileNames);

            }
            catch (Exception ex)
            {

            }
            return Result1;
        }

        //old
        //[HttpGet]
        //[ActionName("GSSTargetSanctionLetter")]
        //public string GSSTargetSanctionLetter(string txnId, string fileName, List<InspactorStatusDetails> InspactorStatusDetails = null)
        //{
        //    try
        //    {
        //        if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
        //        {
        //            GopalCreditCardDBModel _Modal = new GopalCreditCardDBModel();
        //            // var con = Get_GopalApplicationFormViewData_v1(_Model.GccIdDBEnc);                    

        //            var Details = new DataPagingModel
        //            {
        //                PageNumber = 10,
        //                PageSize = 100000
        //            };
        //            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
        //            DataTable Dt = objSocietyModel.GetInspactorTargetsDetails(Details, "0", "13", InspactorStatusDetails[0].InspactorTargetsId, 0, -1);

        //            //var conData = JsonConvert.DeserializeObject<InspactorStatusDetails>(Dt);
        //            //string json = JsonConvert.SerializeObject(Dt);
        //            //var conData = JsonConvert.DeserializeObject<InspactorStatusDetails>(json);
        //            bool isMahila = Convert.ToBoolean(Dt.Rows[0]["IsMahilaMpacs"]);
        //            bool isTsp = Convert.ToBoolean(Dt.Rows[0]["IsTspArea"]);

        //            var templatename = "";
        //            if (isMahila)
        //            {
        //                templatename = System.IO.Path.Combine(Server.MapPath("~/Template"), "gss_target_mahila_sanction_letter.html");
        //            }
        //            else if (isTsp)
        //            {
        //                templatename = System.IO.Path.Combine(Server.MapPath("~/Template"), "gss_target_sanction_letter.html");
        //            }
        //            else
        //            {
        //                templatename = System.IO.Path.Combine(Server.MapPath("~/Template"), "gss_target_sanction_letter.html");
        //            }

        //            StringBuilder strbld = new StringBuilder();

        //            var file = System.IO.Path.Combine(Server.MapPath("~/Template"), templatename);
        //            //var file = System.IO.Path.Combine(Server.MapPath("~/Template"), "ots_certificate.html");

        //            StringBuilder rowBuilder = new StringBuilder();
        //            int i = 0;
        //            string DR2JrPlanForwardedDate = "";
        //            strbld.Append(System.IO.File.ReadAllText(file));
        //            {

        //                foreach (var item in InspactorStatusDetails)
        //                {
        //                    rowBuilder.AppendFormat(
        //                                "<tr>" +
        //                                "<td>{0}</td>" +
        //                               "<td>{1}</td>" +
        //                               "<td>{2}</td>" +
        //                               "<td>{3}</td>" +
        //                               "</tr>",
        //                               i + 1, // Serial Number
        //                               item.GP_ENG?.ToString() ?? "",
        //                               item.GP_ENG?.ToString() ?? "",
        //                               item.BLOCK_ENG?.ToString() ?? ""
        //                           );

        //                    DR2JrPlanForwardedDate += Convert.ToDateTime(item.DR2JrPlanForwardedDate).ToString("dd-MM-yyyy") + ", ";
        //                }

        //                // remove last comma and space
        //                DR2JrPlanForwardedDate = DR2JrPlanForwardedDate.TrimEnd(',', ' ');

        //                foreach (DataRow row in Dt.Rows)
        //                {
        //                    //rowBuilder.AppendFormat(
        //                    //             "<tr>" +
        //                    //             "<td>{0}</td>" +
        //                    //            "<td>{1}</td>" +
        //                    //            "<td>{2}</td>" +
        //                    //            "<td>{3}</td>" +
        //                    //            "</tr>",
        //                    //            i + 1, // Serial Number
        //                    //            row["SocietyName"]?.ToString() ?? "",
        //                    //            row["GP_ENG"]?.ToString() ?? "",
        //                    //            row["BLOCK_ENG"]?.ToString() ?? ""
        //                    //        );

        //                    if (i == 0)
        //                    {
        //                        //strbld.Replace("##IsMahilaMpacs##", row["IsMahilaMpacs"]?.ToString() ?? "");
        //                        //strbld.Replace("##IsTspArea##", row["IsTspArea"]?.ToString() ?? "");
        //                        //strbld.Replace("##AreaType##", row["AreaType"]?.ToString() ?? "");
        //                        //strbld.Replace("##AssemblyId##", row["AssemblyId"]?.ToString() ?? "");
        //                        strbld.Replace("##ActionFlowId##", row["CurrentAppFlowId"]?.ToString() ?? "");
        //                        strbld.Replace("##SenctionNo##", row["SenctionNo"]?.ToString() ?? "");
        //                        strbld.Replace("##DistrictName##", row["DISTRICT_MANGAL"]?.ToString() ?? "");
        //                        strbld.Replace("##DivisionName##", row["DIVISION_MANGAL"]?.ToString() ?? "");
        //                        strbld.Replace("##BankName##", row["BankName"]?.ToString() ?? "");
        //                        strbld.Replace("##SanctionNo##", row["SenctionNo"]?.ToString() ?? "");
        //                        strbld.Replace("##SanctionDate##", DateTime.Now.ToString("dd-MM-yyyy"));
        //                        strbld.Replace("##CommitteeApprovalDate##", row["DateOfApproval"] != DBNull.Value
        //                           ? Convert.ToDateTime(row["DateOfApproval"]).ToString("dd-MM-yyyy")
        //                           : "");
        //                        strbld.Replace("##DR2JrPlanForwardedDate##", DR2JrPlanForwardedDate);
        //                        //strbld.Replace("##DR2JrPlanForwardedDate##", row["DR2JrPlanForwardedDate"] != DBNull.Value
        //                        // ? Convert.ToDateTime(row["DR2JrPlanForwardedDate"]).ToString("dd-MM-yyyy")
        //                        // : "");
        //                    }
        //                    i++;
        //                }
        //            }
        //            strbld.Replace("##SocietyData##", rowBuilder.ToString());


        //            //var gccRegNo = Server.MapPath("~/Content/GopalCreditCard/GSSProfiling/" + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", ""));
        //            var gccRegNo = Server.MapPath("~/Content/InspactorDocument/");
        //            string devFontSize = "17px";

        //            if (!System.IO.Directory.Exists(gccRegNo))
        //            {
        //                System.IO.Directory.CreateDirectory(gccRegNo);
        //            }

        //            // var fileName = "GSSTargetSanction_" + Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace("_", "").ToUpper() + ".pdf";
        //            var filePath = gccRegNo + "/" + fileName;

        //            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
        //            sb1.Append(strbld.ToString());
        //            //  sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(strbld.ToString().Replace("<br>", "<br/>"), true, devFontSize));
        //            //sb1.Replace("#SocietyName#", _Model.PACSName_ENG);
        //            string QRCoder = COMMON_FUNCTION.GenerateQRCode("https://rajsahakarapp.rajasthan.gov.in/Content/InspactorDocument/" + fileName);
        //            sb1.Replace("#QRCoder#", QRCoder);


        //            GopalCreditCardController gopalCreditCardController = new GopalCreditCardController();
        //            gopalCreditCardController.CommonPdfGenerate(sb1, filePath);
        //            var pdfPath = fileName;

        //            //if (_AppFlowModel.DocumentURL != null && _AppFlowModel.DocumentURL != "")
        //            //{
        //            //    pdfPath += "#" + _AppFlowModel.DocumentURL;
        //            //}


        //            string succmsg = string.Empty;

        //            if (!Request.Url.Host.Contains("localhost") && !Request.Url.Host.Contains("devitsandbox"))
        //            {
        //                var result1 = COMMON_FUNCTION.EsinghPDF(txnId, string.Join("#", pdfPath), "RCS", "Pacs Manager");
        //                succmsg = result1.status;
        //            }
        //            else
        //            {
        //                succmsg = "1";
        //            }

        //            return succmsg;

        //        }
        //        else
        //        {
        //            return "Fail";
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ILogger objILogger = new DBLogger();
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "SocietyController", FunctionName = "GSSTargetSanctionLetter catch " }, ex);

        //        return "Fail";
        //    }
        //}


        //New
        [HttpGet]
        [ActionName("GSSTargetSanctionLetter")]
        public string GSSTargetSanctionLetter(string txnId, string fileName, List<InspactorStatusDetails> InspactorStatusDetails = null)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {
                    GopalCreditCardDBModel _Modal = new GopalCreditCardDBModel();
                    // var con = Get_GopalApplicationFormViewData_v1(_Model.GccIdDBEnc);                    

                    var Details = new DataPagingModel
                    {
                        PageNumber = 10,
                        PageSize = 100000
                    };
                    SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                    DataTable Dt = objSocietyModel.GetInspactorTargetsDetails(Details, "0", "13", InspactorStatusDetails[0].InspactorTargetsId, 0, -1);

                    //var conData = JsonConvert.DeserializeObject<InspactorStatusDetails>(Dt);
                    //string json = JsonConvert.SerializeObject(Dt);
                    //var conData = JsonConvert.DeserializeObject<InspactorStatusDetails>(json);
                    bool isMahila = Convert.ToBoolean(Dt.Rows[0]["IsMahilaMpacs"]);
                    bool isTsp = Convert.ToBoolean(Dt.Rows[0]["IsTspArea"]);
                    int isAreaType = Convert.ToInt16(Dt.Rows[0]["AreaType"]);

                    var templatename = "";
                    if (isMahila)
                    {
                        templatename = System.IO.Path.Combine(Server.MapPath("~/Template"), "gss_target_mahila_sanction_letter.html");
                    }
                    else if (isTsp)
                    {
                        templatename = System.IO.Path.Combine(Server.MapPath("~/Template"), "gss_target_sanction_letter.html");
                    }
                    else
                    {
                        if (isAreaType == 3)
                        {
                            templatename = System.IO.Path.Combine(Server.MapPath("~/Template"), "gss_target_cad_sanction_letter.html");
                        }
                        else
                        {
                            templatename = System.IO.Path.Combine(Server.MapPath("~/Template"), "gss_target_sanction_letter.html");
                        }

                    }

                    StringBuilder strbld = new StringBuilder();

                    var file = System.IO.Path.Combine(Server.MapPath("~/Template"), templatename);
                    //var file = System.IO.Path.Combine(Server.MapPath("~/Template"), "ots_certificate.html");

                    StringBuilder rowBuilder = new StringBuilder();
                    int i = 0;
                    string DR2JrPlanForwardedDate = "";
                    strbld.Append(System.IO.File.ReadAllText(file));
                    {

                        foreach (DataRow row in Dt.Rows)
                        {
                            rowBuilder.AppendFormat(
                                "<tr>" +
                                "<td>{0}</td>" +
                                "<td>{1}</td>" +
                                "<td>{2}</td>" +
                                "<td>{3}</td>" +
                                "</tr>",
                                i + 1,
                                row["GP_ENG"]?.ToString() ?? "",
                                row["GP_ENG"]?.ToString() ?? "",
                                row["BLOCK_ENG"]?.ToString() ?? ""
                            );
                            DR2JrPlanForwardedDate += Convert.ToDateTime(row["DR2JrPlanForwardedDate"]).ToString("dd-MM-yyyy") + ", ";
                            i++;
                            //}


                            //foreach (var item in InspactorStatusDetails)
                            //{
                            //    rowBuilder.AppendFormat(
                            //                "<tr>" +
                            //                "<td>{0}</td>" +
                            //               "<td>{1}</td>" +
                            //               "<td>{2}</td>" +
                            //               "<td>{3}</td>" +
                            //               "</tr>",
                            //               i + 1, // Serial Number
                            //               item.GP_ENG?.ToString() ?? "",
                            //               item.GP_ENG?.ToString() ?? "",
                            //               item.BLOCK_ENG?.ToString() ?? ""
                            //           );

                            //    DR2JrPlanForwardedDate += Convert.ToDateTime(item.DR2JrPlanForwardedDate).ToString("dd-MM-yyyy") + ", ";
                            //}

                            // remove last comma and space
                            DR2JrPlanForwardedDate = DR2JrPlanForwardedDate.TrimEnd(',', ' ');

                            //foreach (DataRow row in Dt.Rows)
                            // {
                            //rowBuilder.AppendFormat(
                            //             "<tr>" +
                            //             "<td>{0}</td>" +
                            //            "<td>{1}</td>" +
                            //            "<td>{2}</td>" +
                            //            "<td>{3}</td>" +
                            //            "</tr>",
                            //            i + 1, // Serial Number
                            //            row["SocietyName"]?.ToString() ?? "",
                            //            row["GP_ENG"]?.ToString() ?? "",
                            //            row["BLOCK_ENG"]?.ToString() ?? ""
                            //        );

                            if (i > 0)
                            {
                                //strbld.Replace("##IsMahilaMpacs##", row["IsMahilaMpacs"]?.ToString() ?? "");
                                //strbld.Replace("##IsTspArea##", row["IsTspArea"]?.ToString() ?? "");
                                //strbld.Replace("##AreaType##", row["AreaType"]?.ToString() ?? "");
                                //strbld.Replace("##AssemblyId##", row["AssemblyId"]?.ToString() ?? "");
                                strbld.Replace("##ActionFlowId##", row["CurrentAppFlowId"]?.ToString() ?? "");
                                strbld.Replace("##SenctionNo##", row["SenctionNo"]?.ToString() ?? "");
                                strbld.Replace("##DistrictName##", row["DISTRICT_MANGAL"]?.ToString() ?? "");
                                strbld.Replace("##DivisionName##", row["DIVISION_MANGAL"]?.ToString() ?? "");
                                strbld.Replace("##BankName##", row["BankName"]?.ToString() ?? "");
                                strbld.Replace("##SanctionNo##", row["SenctionNo"]?.ToString() ?? "");
                                strbld.Replace("##SanctionDate##", DateTime.Now.ToString("dd-MM-yyyy"));
                                strbld.Replace("##CommitteeApprovalDate##", row["DateOfApproval"] != DBNull.Value
                                   ? Convert.ToDateTime(row["DateOfApproval"]).ToString("dd-MM-yyyy")
                                   : "");
                                strbld.Replace("##DR2JrPlanForwardedDate##", DR2JrPlanForwardedDate);
                                //strbld.Replace("##DR2JrPlanForwardedDate##", row["DR2JrPlanForwardedDate"] != DBNull.Value
                                // ? Convert.ToDateTime(row["DR2JrPlanForwardedDate"]).ToString("dd-MM-yyyy")
                                // : "");
                            }
                            i++;
                        }
                        // }
                    }
                    strbld.Replace("##SocietyData##", rowBuilder.ToString());


                    //var gccRegNo = Server.MapPath("~/Content/GopalCreditCard/GSSProfiling/" + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", ""));
                    var gccRegNo = Server.MapPath("~/Content/InspactorDocument/");
                    string devFontSize = "17px";

                    if (!System.IO.Directory.Exists(gccRegNo))
                    {
                        System.IO.Directory.CreateDirectory(gccRegNo);
                    }

                    // var fileName = "GSSTargetSanction_" + Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace("_", "").ToUpper() + ".pdf";
                    var filePath = gccRegNo + "/" + fileName;

                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(strbld.ToString());
                    //  sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(strbld.ToString().Replace("<br>", "<br/>"), true, devFontSize));
                    //sb1.Replace("#SocietyName#", _Model.PACSName_ENG);
                    string QRCoder = COMMON_FUNCTION.GenerateQRCode("https://rajsahakarapp.rajasthan.gov.in/Content/InspactorDocument/" + fileName);
                    sb1.Replace("#QRCoder#", QRCoder);


                    GopalCreditCardController gopalCreditCardController = new GopalCreditCardController();
                    gopalCreditCardController.CommonPdfGenerate(sb1, filePath);
                    var pdfPath = fileName;

                    //if (_AppFlowModel.DocumentURL != null && _AppFlowModel.DocumentURL != "")
                    //{
                    //    pdfPath += "#" + _AppFlowModel.DocumentURL;
                    //}


                    string succmsg = string.Empty;

                    if (!Request.Url.Host.Contains("localhost") && !Request.Url.Host.Contains("devitsandbox"))
                    {
                        var result1 = COMMON_FUNCTION.EsinghPDF(txnId, string.Join("#", pdfPath), "RCS", "Pacs Manager");
                        succmsg = result1.status;
                    }
                    else
                    {
                        succmsg = "1";
                    }

                    return succmsg;

                }
                else
                {
                    return "Fail";
                }

            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "SocietyController", FunctionName = "GSSTargetSanctionLetter catch " }, ex);

                return "Fail";
            }
        }
        public ActionResult InspactorTargetsReport()
        {
            return View();
        }

        public JsonResult GetInspactorTargetsReport(string InspactorId, string GSSSStatusId)
        {
            DataTable Dt = null;
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            Dt = objSocietyModel.GetInspactorTargetsReport(InspactorId, GSSSStatusId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(Dt) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("GetInspactorTargetsReportDetails")]
        public JsonResult GetInspactorTargetsReportDetails(DataPagingModel Details, string InspactorId, string StatusId, string GSSSStatusId, int Flag, int DistrictId)
        {
            DataTable Dt = null;
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            Dt = objSocietyModel.GetInspactorTargetsReportDetails(Details, InspactorId, StatusId, GSSSStatusId, Flag, DistrictId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(Dt), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string GetInspactorTargetsReportExportEn(DataPagingModel Details, string InspactorId, string StatusId, string GSSSStatusId, int Flag, int DistrictId)
        {
            string result = "";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return null;
                }
                Details.PageNumber = 1;
                Details.IsAllRecords = true;

                MISModel objNew = new MISModel(new DBLogger());
                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                DataTable List = objSocietyModel.GetInspactorTargetsReportDetails(Details, InspactorId, StatusId, GSSSStatusId, Flag, DistrictId);
                string ReportType = GSSSStatusId;
                string type = "";
                if (ReportType == "1")
                {
                    type = "Survay Report";
                }
                OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><body>");
                sb.Append("<div id='divtoOrderPrint' style='padding-left:5%; padding-right:5%;'>");
                sb.Append("<div style='margin-right:15px;    margin-left:15px;    font-size:20px;    padding:15px;'>");
                sb.Append("<table id='content' border='0' style='width:100%;' cellpadding='5' cellspacing='0'>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:center;font-size:20px;'>Inspector Target Report ");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:left;'> ");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style=''>");
                sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + (ReportType == "WR" ? "font-size:10px;" : "font-size:11px;") + "'>");


                sb.Append("<tr>");
                sb.Append("<th style='text-align:center'>Sr.No. </th>");
                sb.Append("<th style='text-align:center'>Inspector Name</th>");
                sb.Append("<th style='text-align:center'>Urban/Rural </th>");
                sb.Append("<th style='text-align:center'>District</th>");
                sb.Append("<th style='text-align:center'>Block</th>");
                sb.Append("<th style='text-align:center'>City</th>");
                sb.Append("<th style='text-align:center'>GramPanchayat</th>");
                sb.Append("<th style='text-align:center'>Remark</th>");
                sb.Append("<th style='text-align:center'>Status</th>");
                sb.Append("</tr>");
                int index = 1;


                string status1 = "";

                foreach (DataRow dr in List.Rows)
                {

                    string status = dr["InspectorStatusId"].ToString();
                    if (status == "1")
                    {
                        status1 = "Survey";
                    }
                    else if (status == "2")
                    {
                        status1 = " Not Flexible";
                    }
                    else if (status == "3")
                    {
                        status1 = "Proposal";
                    }
                    else if (status == "4")
                    {
                        status1 = "Sention";
                    }
                    else if (status == "5")
                    {
                        status1 = "Completed";
                    }
                    else if (status == "6")
                    {
                        status1 = "Objection";
                    }
                    else if (status == "7")
                    {
                        status1 = "Rejected";
                    }
                    else if (status == "8")
                    {
                        status1 = "Approved For Ragistration";
                    }
                    sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["InspacorName"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["Urban_Rural"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["DISTRICT_ENG"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["BLOCK_ENG"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["CITY_ENG"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["GP_ENG"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["Remark"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + status1 + " </td></tr>");

                }
                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</body></html>");
                // ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                DataTable DT = new DataTable();
                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true));
                var bytes = COMMON_FUNCTION.GePDFBytes(sb1, "Landscape");
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                return base64String;

            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Society", FunctionName = "  GetInspactorTargetsReportExportEn(DataPagingModel Details, string InspactorId, string StatusId, string GSSSStatusId, int Flag, int DistrictId)" }, ex);
                result = "Error";
            }
            return null;

        }

        [HttpPost]
        public string GetInspactorTargetsReportExportHi(DataPagingModel Details, string InspactorId, string StatusId, string GSSSStatusId, int Flag, int DistrictId)
        {
            string result = "";
            string OrderNo = "";
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return null;
                }
                Details.PageNumber = 1;
                Details.IsAllRecords = true;
                string ReportType = GSSSStatusId;
                string type = "";
                if (ReportType == "1")
                {
                    type = "Survay Report";
                }
                MISModel objNew = new MISModel(new DBLogger());
                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                DataTable List = objSocietyModel.GetInspactorTargetsReportDetails(Details, InspactorId, StatusId, GSSSStatusId, Flag, DistrictId);
                OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><body>");
                sb.Append("<div id='divtoOrderPrint' style='padding-left:5%; padding-right:5%;'>");
                sb.Append("<div style='margin-right:15px;    margin-left:15px;    font-size:20px;    padding:15px;'>");
                sb.Append("<table id='content' border='0' style='width:100%;' cellpadding='5' cellspacing='0'>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:center;font-size:20px;'>PLDB Membership Report ");
                sb.Append("  </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:left;'> ");
                sb.Append("  </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style=''>");
                sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + (ReportType == "WR" ? "font-size:10px;" : "font-size:11px;") + "'>");

                sb.Append("<tr>");
                sb.Append("<th style='text-align:center'>Sr.No. </th>");
                sb.Append("<th style='text-align:center'>Inspector Name</th>");
                sb.Append("<th style='text-align:center'>Urban/Rural </th>");
                sb.Append("<th style='text-align:center'>District</th>");
                sb.Append("<th style='text-align:center'>Block</th>");
                sb.Append("<th style='text-align:center'>City</th>");
                sb.Append("<th style='text-align:center'>GramPanchayat</th>");
                sb.Append("<th style='text-align:center'>Remark</th>");
                sb.Append("<th style='text-align:center'>Status</th>");
                sb.Append("</tr>");
                int index = 1;


                string status1 = "";

                foreach (DataRow dr in List.Rows)
                {
                    string status = dr["InspectorStatusId"].ToString();
                    if (status == "1")
                    {
                        status1 = "Survey";
                    }
                    else if (status == "2")
                    {
                        status1 = " Not Flexible";
                    }
                    else if (status == "3")
                    {
                        status1 = "Proposal";
                    }
                    else if (status == "4")
                    {
                        status1 = "Sention";
                    }
                    else if (status == "5")
                    {
                        status1 = "Completed";
                    }
                    else if (status == "6")
                    {
                        status1 = "Objection";
                    }
                    else if (status == "7")
                    {
                        status1 = "Rejected";
                    }
                    else if (status == "8")
                    {
                        status1 = "Approved For Ragistration";
                    }
                    sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["InspacorName"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["Urban_Rural"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["DISTRICT_ENG"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["BLOCK_ENG"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["CITY_ENG"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["GP_ENG"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + dr["Remark"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left'> " + status1 + " </td></tr>");

                }
                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</body></html>");
                // ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                DataTable DT = new DataTable();
                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true));
                var bytes = COMMON_FUNCTION.GePDFBytes(sb1, "Landscape");
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                return base64String;

            }
            catch (Exception ex)
            {

            }
            return null;
        }

        [HttpPost]
        public string SaveCCSCompaintTransferRequest(int ComplaintId, string TransferDistraict, string TransferFromDistraict, string TransferRemarks, string status, int TransferId, string ResionName)
        {
            ViewBag.LoggedInUserDistrictId = _LoggedInUser.DistrictId;
            SocietyModel objNew = new SocietyModel(new DBLogger());
            int DistrictId = _LoggedInUser.DistrictId;
            string result = objNew.SaveCCSCompaintTransferRequest(ComplaintId, TransferDistraict, TransferFromDistraict, TransferRemarks, status, TransferId, ResionName);
            return result;
        }

        [HttpPost]
        public string UpdateSocietyMAPRequest(string SocietyId, string SSOId, int RequestId, string Remarks, string Status)
        {

            ViewBag.LoggedInUserDistrictId = _LoggedInUser.DistrictId;
            SocietyModel objNew = new SocietyModel(new DBLogger());
            int DistrictId = _LoggedInUser.DistrictId;
            string result = objNew.UpdateSocietyMAPRequest(SocietyId, SSOId, RequestId, Remarks, Status);
            return result;
        }


        public ActionResult CCSComplaintTransfer()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetCCSComplaintTransfer(DataPagingModel Details)
        {
            DataTable Dt = null;
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            int DistrictId = _LoggedInUser.DistrictId;
            Dt = objSocietyModel.GetCCSComplaintTransfer(ref Details, DistrictId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(Dt), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetEmitraTransaction(string SearchText, string ServiceType)
        {
            DataTable Dt = null;
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            int DistrictId = _LoggedInUser.DistrictId;
            Dt = objSocietyModel.GetEmitraTransaction(SearchText, ServiceType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(Dt) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VerifyEmitraTransaction()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetCCSNoticIssueCount(int DistrictId1, string ActinDate)
        {
            DataTable Dt = null;
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            int DistrictId = _LoggedInUser.DistrictId;
            Dt = objSocietyModel.GetCCSNoticIssueCount(DistrictId, ActinDate);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(Dt) }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetResionByDistrict(int DistrictId)
        {
            DataTable Dt = null;
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            Dt = objSocietyModel.GetResionByDistrict(DistrictId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(Dt) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TransferSocietiesToUser()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SetSocietiesToUser(int FromUser, int ToUser, int ActId, int RoleId)
        {
            string result = "";
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            result = objSocietyModel.TransferSocietiesToUser(FromUser, ToUser, ActId, RoleId);
            return Json(result);
        }

        public ActionResult DataIntigration()
        {
            return View();
        }


        public ActionResult ApplicationErrorLog()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetErrorLog(DataPagingModel Details, int Id, int Type, int StatusId)
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            DataTable DT = objSocietyModel.GetErrorLog(ref Details, Id, Type, StatusId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddGodown()
        {
            return View();
        }

        [HttpPost]
        public int SaveGodown(GodownEntity _Entity)
        {
            string fileName = "";
            string fileName1 = "";


            if (_Entity.SanctionDate == "1/1/0001 12:00:00 AM" || _Entity.SanctionDate == "undefined")
            {
                _Entity.SanctionDate = string.Empty;
            }

            #region "Mange Undifined"

            if (_Entity.CurrentStatus == "undefined")
            {
                _Entity.CurrentStatus = string.Empty;
            }
            if (_Entity.Usegodown == "undefined")
            {
                _Entity.Usegodown = string.Empty;
            }
            if (_Entity.ComplitionYear == "undefined")
            {
                _Entity.ComplitionYear = string.Empty;
            }
            #endregion

            if (Request.Files != null && Request.Files.Count == 2)
            {

                if (Request.Files.AllKeys[0] == "SanctionDoc")
                {
                    if (Request.Files != null && Request.Files.Count > 0)
                    {
                        var file = Request.Files[0];
                        fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        _Entity.SanctionDoc = "~/Content/GodownFiles/" + fileName;
                        file.SaveAs(Path.Combine(Server.MapPath("~/Content/GodownFiles"), fileName));
                    }
                }

                if (Request.Files.AllKeys[1] == "file")
                {
                    if (Request.Files != null && Request.Files.Count > 0)
                    {
                        var file1 = Request.Files[1];
                        fileName1 = Guid.NewGuid() + Path.GetExtension(file1.FileName);
                        _Entity.Photo = "~/Content/GodownFiles/" + fileName1;
                        file1.SaveAs(Path.Combine(Server.MapPath("~/Content/GodownFiles"), fileName1));
                    }

                }
            }
            else
            {
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    if (_Entity.SanctionDoc != null && _Entity.SanctionDoc != "" && _Entity.SanctionDoc != "undefined")
                    {
                        if (Request.Files.AllKeys[0] == "SanctionDoc")
                        {
                            var file = Request.Files[0];
                            fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                            _Entity.SanctionDoc = "~/Content/GodownFiles/" + fileName;
                            file.SaveAs(Path.Combine(Server.MapPath("~/Content/GodownFiles"), fileName));
                        }
                    }
                    else if (Request.Files.AllKeys[0] == "file")
                    {
                        var file = Request.Files[0];
                        fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        _Entity.Photo = "~/Content/GodownFiles/" + fileName;
                        file.SaveAs(Path.Combine(Server.MapPath("~/Content/GodownFiles"), fileName));
                    }
                }

            }
            if (_Entity.Photo == "undefined")
            {
                _Entity.Photo = string.Empty;
            }
            if (_Entity.SanctionDoc == "undefined")
            {
                _Entity.SanctionDoc = string.Empty;
            }
            //if (Request.Files != null && Request.Files.Count > 0)
            //{
            //    var set = Request.Files.AllKeys[0];

            //    try
            //    {



            //        var file = Request.Files[0];

            //        var file1 = Request.Files[1];
            //        fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            //        fileName1 = Guid.NewGuid() + Path.GetExtension(file1.FileName);
            //        _Entity.SanctionDoc = fileName;

            //        _Entity.Photo = fileName1;
            //         file.SaveAs(Path.Combine(Server.MapPath("~/Content/SocietyExcel"), fileName));
            //        file1.SaveAs(Path.Combine(Server.MapPath("~/Content/SocietyExcel"), fileName1));
            //    }
            //    catch { }
            //}





            _Entity.CreatedBy = _LoggedInUser.UserId.ToString();
            //_Entity.SanctionDate = "1/1/0001 12:00:00 AM" ? DateTime.UtcNow.Date : _Entity.SanctionDate;

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return objSocietyModel.GodownDetailsInsert(_Entity);

        }

        [HttpPost]
        public string ChangeRoleStatus(string Id, string Action)
        {
            int GodownId = 0, ActionId = 0;
            if (!int.TryParse(Id, out GodownId) && !int.TryParse(Action, out ActionId))
                return CommonMessageFactory.Message(401, (Request.Cookies[SessionIdentity.Culture]).Value);

            ActionId = (Convert.ToInt32(Action));
            ActionId = (ActionId == 0 || ActionId == 1) ? (ActionId == 0 ? 1 : 0) : ActionId;


            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return CommonMessageFactory.Response(CommonMessageFactory.Message(objSocietyModel.ChangeGodownStatus(GodownId, ActionId), (Request.Cookies[SessionIdentity.Culture]).Value));

        }

        [HttpGet]
        public string Godownlists(int Status = 0)
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());

            DataPagingModel Details = new DataPagingModel();
            Details.IsAllRecords = true;
            Details.PageNumber = 1;
            return COMMON_FUNCTION.DataTableToJSONString(objSocietyModel.getGodownlist(ref Details, _LoggedInUser.UserRoleDeptMappingId, Status));
        }

        [HttpGet]
        public string Godownlistsdetails(int Gid = 0)
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());

            DataPagingModel Details = new DataPagingModel();
            Details.IsAllRecords = true;
            Details.PageNumber = 1;
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objSocietyModel.GetgodownDetails(Gid));
        }

        [HttpGet]
        public string GodownCCBlistsdetails(int districtid = 0)
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());

            DataPagingModel Details = new DataPagingModel();
            Details.IsAllRecords = true;
            Details.PageNumber = 1;
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objSocietyModel.GetCCBDetails(districtid));
        }


        [HttpGet]
        public string GetKVSSGSSData(string Type, int DistrictId = 0, int BlockId = 0, int CityId = 0, int GPId = 0)
        {
            SocietyModel objNew = new SocietyModel(new DBLogger());
            DataTable DT = objNew.GetGssKvssDetails(Type, DistrictId, BlockId, CityId, GPId);
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);

        }

        [HttpGet]
        public ActionResult GodownStatusUpdate()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UpdateDistrictDetails()
        {
            return View();
        }
        [HttpGet]
        public ActionResult UpdateRegistrationDetails()
        {
            return View();
        }

        [HttpPost]
        public int UpdateStatusGodown(GodownEntity _Entity)
        {

            string fileName = "";
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    _Entity.SanctionDoc = "~/Content/GodownFiles/" + fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("~/Content/GodownFiles"), fileName));
                    _Entity.Photo = "~/Content/GodownFiles/" + fileName;
                }
                catch { }
            }
            _Entity.CreatedBy = _LoggedInUser.UserId.ToString();
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return objSocietyModel.GodownDetailsupdate(_Entity);
        }


        [HttpPost]
        public int UpdateGodowndata(GodownEntity _Entity)
        {

            string fileName = "";
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    _Entity.SanctionDoc = "~/Content/GodownFiles/" + fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("~/Content/GodownFiles"), fileName));
                    _Entity.Photo = "~/Content/GodownFiles/" + fileName;
                }
                catch { }
            }
            _Entity.CreatedBy = _LoggedInUser.UserId.ToString();
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return objSocietyModel.Godownupdate(_Entity);
        }


        [HttpPost]
        public int UpdateDistrict(UpdateDistrictEntity _Entity)
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return objSocietyModel.SocietyDistrictupdate(_Entity);
        }
        [HttpPost]
        public int UpdateRegistration(UpdateDistrictEntity _Entity)
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return objSocietyModel.SocietyRegistrationupdate(_Entity);
        }

        [HttpPost]
        public JsonResult Godownlists(DataPagingModel Details, int Status = 0)
        {
            ViewBag.LoggedInUserDistrictId = _LoggedInUser.DistrictId;
            SocietyModel objNew = new SocietyModel(new DBLogger());
            int DistrictId = _LoggedInUser.DistrictId;
            DataTable DT = objNew.getGodownlist(ref Details, _LoggedInUser.UserRoleDeptMappingId, Status);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public string GetGodownStatus()
        {
            SocietyModel objNew = new SocietyModel(new DBLogger());
            DataTable DT = objNew.GetGodownStatus();
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);

        }

        [HttpPost]
        public JsonResult GetSocietyForRegList(DataPagingModel Details)
        {
            ViewBag.LoggedInUserDistrictId = _LoggedInUser.DistrictId;
            SocietyModel objNew = new SocietyModel(new DBLogger());
            int DistrictId = _LoggedInUser.DistrictId;
            DataTable DT = objNew.GetTotalSocietyforreg(ref Details, DistrictId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult GSocietyList(DataPagingModel Details)
        {
            ViewBag.LoggedInUserDistrictId = _LoggedInUser.DistrictId;
            SocietyModel objNew = new SocietyModel(new DBLogger());
            int DistrictId = _LoggedInUser.DistrictId;
            DataTable DT = objNew.GetTotalSociety(ref Details);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult GetOfflineSocietyList(DataPagingModel Details)
        {
            ViewBag.LoggedInUserDistrictId = _LoggedInUser.DistrictId;
            SocietyModel objNew = new SocietyModel(new DBLogger());
            int DistrictId = _LoggedInUser.DistrictId;
            DataTable DT = objNew.GetOfflineSocietyList(ref Details);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetOfflineSocietyList_V1(DataPagingModel Details)
        {
            ViewBag.LoggedInUserDistrictId = _LoggedInUser.DistrictId;
            SocietyModel objNew = new SocietyModel(new DBLogger());
            int DistrictId = _LoggedInUser.DistrictId;
            DataTable DT = objNew.GetOfflineSocietyList_V1(ref Details);

            //return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
            var result = new
            {
                DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT),
                DataPaging = Details
            };

            return new JsonResult
            {
                Data = result,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }



        [HttpPost]
        public JsonResult GetAmendmentHistory()
        {
            Society objNew = new Society();
            var data = objNew.GetAmendmentHistory(_LoggedInUser.SSOID);
            return Json(new { DataList = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAmendmentIssuedDocuments(int Amendmentid)
        {
            Society objNew = new Society();
            var data = objNew.GetAmendmentIssuedDocuments(Amendmentid);
            return Json(new { DataList = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAmendmentHistoryBySocietyId(string SocietyIdENC)
        {
            Society objNew = new Society();
            var data = objNew.GetAmendmentHistoryBySocietyId(SocietyIdENC);
            return Json(new { DataList = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CCSComplaintForm()
        {
            return View();
        }

        [HttpPost]
        public string InsertCCSComplaintForm(CCSComplaintDetails _Entity)
        {
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return objSocietyModel.InsertCCSComplaintData(_Entity).ToString();

        }

        #region Amit Kumar
        public ActionResult AssignauditorNew()
        {
            return View();
        }
        //[HttpPost]
        //[ActionName("GetDivisions")]
        //public string GetDivisions()
        //{
        //    SocietyModel objNew = new SocietyModel(new DBLogger());
        //    return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objNew.GetDivisions());
        //}
        //[HttpPost]
        //[ActionName("GetDistrictList")]
        //public string GetDistrictList(int DivisionId)
        //{
        //    SocietyModel objNew = new SocietyModel(new DBLogger());
        //    return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objNew.GetDistrictList(DivisionId));
        //}
        //[HttpPost]
        //[ActionName("GetFilterResion")]
        //public string GetFilterResion(int DistrictId)
        //{
        //    SocietyModel objNew = new SocietyModel(new DBLogger());
        //    return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objNew.GetFilterResion(DistrictId));
        //}
        //[HttpPost]
        //[ActionName("GetSocietyList")]
        //public string GetSocietyList(int ResionId)
        //{
        //    SocietyModel objNew = new SocietyModel(new DBLogger());
        //    return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objNew.GetSocietyList(ResionId));
        //}
        //[HttpPost]
        //[ActionName("GetFinancialYearList")]
        //public string GetFinancialYearList()
        //{
        //    SocietyModel objNew = new SocietyModel(new DBLogger());
        //    return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objNew.GetFinancialYearList());
        //}
        #endregion

        #region "Ravi Raj"
        [HttpPost]
        public string ValidateSocietybyId(string SocietyId)
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return objSocietyModel.ValidateSocietybyId(SocietyId).ToString();
        }
        #endregion

        //Ram
        [HttpGet]
        public ActionResult CCSSurveySubmitForm()
        {
            return View();
        }

        [HttpPost]
        public string insertCCSSurveySubmit(CCSSurveySubmitForm _Entity)
        {
            //UserEntity userName1 = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin]; , UserEntity userName  ,, userName

            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<CCSSurveySubmitFormInvestment> objData = new List<CCSSurveySubmitFormInvestment>();
            try
            {
                objData = Request.Form["DataArray"].ToString().Equals("[]") ? new List<CCSSurveySubmitFormInvestment>() : (List<CCSSurveySubmitFormInvestment>)s.Deserialize(Request.Form["DataArray"].ToString(), typeof(List<CCSSurveySubmitFormInvestment>));
            }
            catch (Exception ex)
            {
                return CommonMessageFactory.Response("Provided input data is not in correct format.");
            }
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return objSocietyModel.InsertCCSSurvey(objData, _Entity).ToString();
        }




        [HttpGet]
        public ActionResult CCSSurveyReportwork()
        {
            return View();
        }




        [HttpPost]
        public JsonResult GetCCSConsolidatedReportData(DataPagingModel Details, int Type = -1)
        {
            string msg = string.Empty;
            string fromDate = "";
            string toDate = "";
            if (!string.IsNullOrEmpty(fromDate) && !DataFormatter.IsValidDateTime(fromDate))
                msg += "date format in from date is not valid (required dd/mm/yyyy). ";

            if (!string.IsNullOrEmpty(toDate) && !DataFormatter.IsValidDateTime(toDate))
                msg += "date format in to date is not valid (required dd/mm/yyyy). ";

            if (!string.IsNullOrEmpty(msg))
                return Json(new { Result = false, data = new int[] { }, Message = msg }); // Validation fails no further action required

            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());

            if (!string.IsNullOrEmpty(fromDate))
                fromDate = fromDate.Split('/')[1] + "/" + fromDate.Split('/')[0] + "/" + fromDate.Split('/')[2];

            if (!string.IsNullOrEmpty(toDate))
                toDate = toDate.Split('/')[1] + "/" + toDate.Split('/')[0] + "/" + toDate.Split('/')[2];

            DataTable dt = objCitizenModel.GetCCSConsolidatedReportData(ref Details, fromDate, toDate, _LoggedInUser.DistrictId, _LoggedInUser.UserRoleDeptMappingId, Type);

            if (dt == null || dt.Rows.Count < 1)
                return Json(new { Result = false, data = new int[] { }, Message = "No Record found as per search criteria." });

            List<CCSComplaintRegisterModel> list = new List<CCSComplaintRegisterModel>();

            foreach (var item in dt.AsEnumerable())
            {
                CCSComplaintRegisterModel temp = new CCSComplaintRegisterModel();
                temp.ComplainerName = item.Field<string>("ComplainerName");
                temp.ComplainerFatherName = item.Field<string>("ComplainerFatherName");
                temp.MobileNo = item.Field<string>("MobileNo");
                temp.ReceiptNo = item.Field<string>("ReceiptNo");
                temp.AmountDeposited = item.Field<decimal>("AmountDeposited");
                temp.CreateDate = item.Field<string>("CreateDateTime");
                temp.ComplaintIdEnc = item.Field<string>("ComplaintIdEnc");
                temp.IsPermanentLocked = item.Field<bool>("IsPermanentLocked");
                try
                {
                    string str = COMMON_FUNCTION.Decrypt(item.Field<string>("AADHAR"));
                    temp.AADHARMasked = string.IsNullOrEmpty(str) ? string.Concat("".PadLeft(8, 'X'), str.Substring(str.Length - 4)) : "";
                }
                catch (Exception ex)
                {
                    temp.AADHARMasked = "XXXXXXXXXXXX";
                    //DBLogger objDBLogger = new DBLogger();
                    //objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Rajsahkar.Controller.society", FunctionName = "public JsonResult GetCCSConsolidatedReportData(string fromDate, string toDate, int districtId, int CCSId) while AADHAR Decryption " + " Message: " + item.Field<string>("AADHAR") + "fromDate=" + fromDate + ",toDate=" + toDate + ",DistrictId=" + _LoggedInUser.DistrictId + ",UserRoleDeptMappingId=" + _LoggedInUser.UserRoleDeptMappingId + ",Type=" + Type + ",Details.SearchParameter=" + Details.SearchParameter + Convert.ToString(ex.Message) }, ex);
                }

                temp.District = new DistrictModel() { DistrictId = 1, DistrictEng = item.Field<string>("DISTRICT_ENG"), DistrictHi = item.Field<string>("DISTRICT_MANGAL") };
                temp.CCS = new CCSModel() { SocietyNameEng = item.Field<string>("SocietyName") };
                list.Add(temp);
            }

            return Json(new { DataList = list, DataPaging = Details }, JsonRequestBehavior.AllowGet);

            //return Json(new { Result = true, data = list, Message = "Success" });
        }



        [HttpPost]
        [ActionName("getCCSSurveydetails")]
        public string getCCSSurveydetails()
        {
            string WhereClause = " where 1=1";
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());

            return COMMON_FUNCTION.DataTableToJSONString(objSocietyModel.getccssurveydetails(WhereClause));
        }



        [HttpPost]
        public string GetCCSData(List<Params> paramlist, int Type = -1)
        {
            SocietyModel objCitizenModel = new SocietyModel(new DBLogger());
            // string whereclause = " and (Societies.RegistrationNo like 'MSCS%' or SocietyRegOfficeDetails.DISTRICT_ID=" + paramlist[0].Value + " ) ";
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objCitizenModel.GetCCSData(paramlist[0].Value, _LoggedInUser.DistrictId, Type));
        }

        [HttpPost]
        [ActionName("getCCSSurveydetailsforwork")]
        public string getCCSSurveydetailsforwork()
        {
            string WhereClause = " where 1=1";
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());

            return COMMON_FUNCTION.DataTableToJSONString(objSocietyModel.getCCSSurveydetailsforwork(WhereClause));
        }

        [HttpPost]
        public string GetCCSSurveydetailsview(int Gid = 0)
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            DataPagingModel Details = new DataPagingModel();
            Details.IsAllRecords = true;
            Details.PageNumber = 1;
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objSocietyModel.getCCSSurveydetailsview(Gid));
        }
        [HttpPost]
        [ActionName("Investmentdetails")]
        public JsonResult Investmentdetails(int Gid = 0)
        {
            DataTable DT = new DataTable();

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            DataPagingModel Details = new DataPagingModel();

            Details.IsAllRecords = true;

            Details.PageNumber = 1;

            DT = objSocietyModel.Investmentdetails1(Gid);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string ActiononCCSSurvey(CCSSurveySubmitForm _Entity)
        {
            //UserEntity userName1 = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin]; , UserEntity userName  ,, userName           
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return objSocietyModel.InsertActiononCCSSurvey(_Entity).ToString();

        }
        //RAm



        #region ObjectedSocieties by madhuri 2 jan 2025
        [HttpGet]
        public string GetSocityDataForObjection(string searchText)
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            var dt = objSocietyModel.GetSocityDataForObjection(searchText);
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(dt);

        }

        [HttpPost]
        // public JsonResult insertObjectedSocieties(string Remark = "", int societyId = 0, string AmendmentIds = "", object doc = null , objecedSocietyEntity _entity=null)
        public JsonResult insertObjectedSocieties(objecedSocietyEntity _entity = null)
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());


            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    string fileName = string.Empty;
                    string pathString = "";

                    if (!System.IO.Directory.Exists(pathString))
                    {
                        pathString = "~/Content/ObjectedSocietyDoc/";
                        {
                            System.IO.Directory.CreateDirectory(Server.MapPath(pathString));
                        }
                    }
                    foreach (string files in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[files];
                        fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                        if (System.IO.Directory.Exists(Server.MapPath(pathString)))
                        {
                            file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));
                        }
                        if (files.ToString() == "ObjectionFile")
                        {
                            _entity.ObjectionFile = fileName;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _entity.ObjectionFile = "";
                }
            }

            var dt = objSocietyModel.insertObjectedSocieties(_entity.Remark, _entity.SocietyId, _entity.AmendmentIds, _entity.ObjectionFile.ToString());

            return Json(new { Result = true, Message = "", data = dt });
        }

        public string AllObjectedSocieties()
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            var dt = objSocietyModel.AllObjectedSocieties();
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(dt);

        }

        public JsonResult ChangeStatus(int id = 0)
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            var dt = objSocietyModel.ChangeStatus(id);
            return Json(new { Result = true, Message = "", data = dt });

        }


        #endregion

        #region RemoveUnwantedFilesOrDuplicateFiles in societyProfiling
        public static void RemoveDuplicateFiles()
        {
            DataTable dt = new DataTable();
            int SocietyId = 10081;
            string[] allowedFiles = { };
            string folderPath = "";

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            dt = objSocietyModel.GetAlldocumentsRelatedToSociety(SocietyId);

            if (dt != null)
            {
                allowedFiles = dt.AsEnumerable().Select(row => row.Field<string>("document")).ToArray();
                folderPath = "../Content/SocietyProfiling/" + dt.Rows[0]["SocietyDir"].ToString();

                if (Directory.Exists(folderPath))
                {
                    // Get all files in the folder
                    var files = Directory.GetFiles(folderPath);

                    foreach (var file in files)
                    {
                        string fileName = Path.GetFileName(file);

                        // Delete file if it's not in the allowed list
                        if (!allowedFiles.Contains(fileName, StringComparer.OrdinalIgnoreCase))
                        {
                            try
                            {
                                System.IO.File.Delete(file);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
            }


            // return COMMON_FUNCTION.DataTableToJSONWithJSONNet(dt);

            //  string folderPath = @"C:\Path\To\Your\Content"; // Update with your actual folder path



            // List of allowed files (only these will be kept)
            //string[] allowedFiles = {
            //"23BF78AC511C41A39AC7127AD1B9922C.pdf",
            //"2DA96FC8EB9E4D36BA74AF6BD3A861DF.pdf",
            //"DF836B3AB1D348FD817FCBF35A35524F.pdf",
            //"F2E58CC3507746519797E30F8FBF0E17.xlsx",
            //"0897F09A856C44C9A1EF3FA1E496F1D7.jpg",
            //"0989F1F30EB94F65B8AF234CC2A89848.jpg"
            //};



        }
        #endregion


        #region MergeSocieties for duplicate societies 
        [HttpPost]
        public int MergeSocieties(string OldSocietyIds, int NewSocietyId, string SocietyName = "", string Remark = "")
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return objSocietyModel.MergeSocieties(OldSocietyIds, NewSocietyId, SocietyName, Remark);
        }

        #endregion


        #region PDCS RCDF 

        [HttpPost]
        [ActionName("GetPDCSDetails")]
        public string GetPDCSDetails(string PdcsNo)
        {
            SocietyModel objSocietyModel = new SocietyModel();
            string ReturnJSON = "";
            int IsPresent = objSocietyModel.CheckSocietyByPdcsNo(PdcsNo);
            if (IsPresent == 0)
            {
                ILogger objILogger = new DBLogger();
                try
                {
                    PdcsResponseModel obj = new PdcsResponseModel();

                    CommonServicesModel objCommonServicesModel = new CommonServicesModel(new FileLogger());
                    obj = objCommonServicesModel.ValidatePdcsProfile(PdcsNo);
                    ReturnJSON = JsonConvert.SerializeObject(obj);

                }
                catch (Exception ex)
                {
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.Society", FunctionName = "GetPDCSDetails(string PdcsNo)" }, ex);
                }
            }
            else
            {
                ReturnJSON = "{\"Status\":\"" + "-2" + "\",\"Message\":\"" + "PdcsNo number is already exist." + "\"}";
            }
            return ReturnJSON;
        }

        [HttpPost]
        [ActionName("UpdatePDCSDetails")]
        public string UpdatePDCSDetails(PdcsModel pdcs)
        {
            string ReturnJSON = "";
            if (pdcs.SocietyTypeId == 10)
            {
                SocietyModel objSocietyModel = new SocietyModel();
                int IsPresent = objSocietyModel.CheckSocietyByPdcsNo(pdcs.RegNo);
                if (IsPresent == 0)
                {
                    ILogger objILogger = new DBLogger();
                    try
                    {
                        var result = objSocietyModel.UpdatePdcsDetails(pdcs);
                        if (result > 0)
                            ReturnJSON = "{\"Status\":\"" + "Success" + "\",\"Message\":\"" + "PDCS details updated successfully." + "\"}";

                    }
                    catch (Exception ex)
                    {
                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.Society", FunctionName = "UpdatePDCSDetails(PdcsModel pdcs)" }, ex);
                    }
                }
                else
                {
                    ReturnJSON = "{\"Status\":\"" + "Failed" + "\",\"Message\":\"" + "PdcsNo number is already exist." + "\"}";
                }
            }
            else
                ReturnJSON = "";
            return ReturnJSON;
        }


        #endregion

        [HttpPost]
        [ActionName("GSSTargetProceesStatusList")]
        public string GetGSSProceesStatusList(string StatusId, string InspactorTargetsId)
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            int StatusIdInt;
            int.TryParse(StatusId, out StatusIdInt);
            return COMMON_FUNCTION.DataTableToJSONString(objSocietyModel.GetGSSTargetProceesStatusListIdsByIds(StatusIdInt, InspactorTargetsId));
        }


        //Get Inspector Details
        [HttpGet]
        [ActionName("GetAGMMultiInspectorDetails")]
        public string GetAGMMultiInspectorDetails(int id)
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objSocietyModel.GetAGMMultiInspectorDetails(id));

        }

        public JsonResult GetInspectorDetailsForCertificate(string InspactorTargetsId)
        {
            DataTable Dt = null;
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            Dt = objSocietyModel.GetInspectorDetailsForCertificate(InspactorTargetsId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(Dt) }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult UploadDocument(HttpPostedFileBase UploadDocument)
        {
            try
            {
                if (UploadDocument == null)
                    return Json(new { success = false, message = "File not received" });

                string folder = Server.MapPath("~/Content/AGMDocument/UploadDocument/");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(UploadDocument.FileName);
                string filePath = Path.Combine(folder, fileName);

                UploadDocument.SaveAs(filePath);

                return Json(new { success = true, fileName = fileName });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public JsonResult DeleteLoanDocument(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    return Json(new { success = false, message = "Invalid file name" });
                }

                string folderPath = Server.MapPath("~/Content/AGMDocument/UploadDocument/");
                string filePath = Path.Combine(folderPath, fileName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        #region 'GetCSSComplaintOnlineDistrictWise'

        [HttpGet]
        [ActionName("GetCSSComplaintOnlineDistrictWise")]
        public JsonResult GetCSSComplaintOnlineDistrictWise(int districtId = 0)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            var responce = new APIResponce();

            var ds = objSocietyModel.GetCSSComplaintOnlineDistrictWise(districtId);

            if (ds != null)
            {
                responce.Data = new
                {
                    ComplaintOnlineDistrictWise = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null
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


        #region 'GetCCSComplaintList'

        [HttpGet]
        [ActionName("GetCCSComplaintListDetails")]
        public JsonResult GetCCSComplaintList()
        {
            SocietyModel objModel = new SocietyModel();

            DataSet ds = objModel.GetCCSComplaintList();

            return Json(new { Result = new { Data = new { CCSComplaintList = JsonConvert.SerializeObject(ds.Tables[0]) } } }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        [HttpPost]
        public string PacsUserLoginElectionDetails(SocietyRegistrationEntity objSocietyRegistrationEntity)
        {

            SocietyModel objMenuActionModel = new SocietyModel(new FileLogger());
            DataTable DT = null;
            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"].Value == "KIOSK" && Request.Cookies["SSO_ID"] != null && System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc] != null)
            {
                objSocietyRegistrationEntity.UserType = "K";
                objSocietyRegistrationEntity.SocietyIDEnc = System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc].ToString().Split('~')[0];
                DT = objMenuActionModel.SocietyListWithDepartmentUser(objSocietyRegistrationEntity);
                if (DT != null && DT.Rows.Count > 0)
                {
                    System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc] = null;
                }
            }
            else if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
            {
                if (_loggedInUSerDetails.UserType == "D")
                {
                    objSocietyRegistrationEntity.UserType = _loggedInUSerDetails.UserType;
                    objSocietyRegistrationEntity.SSOID = _loggedInUSerDetails.SSOID;
                    objSocietyRegistrationEntity.DistrictId = _loggedInUSerDetails.DistrictId;
                    objSocietyRegistrationEntity.UserRoleDeptMappingId = _loggedInUSerDetails.UserRoleDeptMappingId;

                    DT = objMenuActionModel.SocietyListWithDepartmentUser(objSocietyRegistrationEntity);
                }
                else
                {
                    objSocietyRegistrationEntity.UserType = _loggedInUSerDetails.UserType;
                    objSocietyRegistrationEntity.OrganizationId = _loggedInUSerDetails.UserRoleDeptMappingId;
                    objSocietyRegistrationEntity.SSOID = Request.Cookies["SSO_ID"].Value;
                    if (objSocietyRegistrationEntity.UserType != "K")
                        DT = objMenuActionModel.SocietyListWithDepartmentUser(objSocietyRegistrationEntity);
                }
            }

            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        }

        [HttpPost]
        public string KVSSGSSbyDRorMDCCB(SocietyRegistrationEntity objSocietyRegistrationEntity)
        {

            SocietyModel objMenuActionModel = new SocietyModel(new FileLogger());
            DataTable DT = null;
            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
            {
                if (_loggedInUSerDetails.UserType == "D")
                {
                    objSocietyRegistrationEntity.UserType = _loggedInUSerDetails.UserType;
                    objSocietyRegistrationEntity.SSOID = _loggedInUSerDetails.SSOID;
                    objSocietyRegistrationEntity.DistrictId = _loggedInUSerDetails.DistrictId;
                    objSocietyRegistrationEntity.UserRoleDeptMappingId = _loggedInUSerDetails.UserRoleDeptMappingId;
                    objSocietyRegistrationEntity.RoleId = _loggedInUSerDetails.RoleId;

                    DT = objMenuActionModel.KVSSGSSbyDRorMDCCB(objSocietyRegistrationEntity);
                }

            }

            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        }

        [HttpGet]
        [ActionName("GetReasonForVacancyByListId")]
        public string GetReasonForVacancyDetailsByListId(string ListCode)
        {
            DataTable DT = new SocietyModel().GetReasonForVacancyDetailByListId(ListCode);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }

        [HttpGet]
        [ActionName("GetSocietyMemberList")]
        public string GetElectionSocietyDetailsByListId(int SocietyId)
        {
            DataTable DT = new SocietyModel().GetSocietyDetailBySocietyId(SocietyId);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }



        [HttpPost]
        public int SaveElectionFormold(ElectionFormEntity _Entity)
        {
            _Entity.CreatedBy = _LoggedInUser.UserId.ToString();
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return objSocietyModel.ElectionDetailsInsert(_Entity);
        }


        [HttpPost]
        public int SaveElectionForm(ElectionFormEntity _Entity)
        {
            string vacancyListJson = Request.Form["VacancyList"];

            if (!string.IsNullOrWhiteSpace(vacancyListJson))
            {
                vacancyListJson = vacancyListJson.Replace("\"VacancyId\":null", "\"VacancyId\":0");
                _Entity.VacancyList = JsonConvert.DeserializeObject<List<SocietyElectionVacancyEntity>>(vacancyListJson);
            }

            _Entity.CreatedBy = _LoggedInUser.UserId.ToString();

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());

            return objSocietyModel.ElectionDetailsInsert(_Entity);
        }

        [HttpPost]
        public int SaveFertilizerForm(FertilizerFormEntity _Entity)
        {
            string demandListJson = Request.Form["DemandList"];
            if (!string.IsNullOrWhiteSpace(demandListJson))
            {
                demandListJson = demandListJson.Replace("\"DemandId\":null", "\"DemandId\":0");
                _Entity.DemandList = JsonConvert.DeserializeObject<List<FertilizerDemandEntity>>(demandListJson);
            }
            string creditFacilityListJson = Request.Form["CreditFacilityList"];
            if (!string.IsNullOrWhiteSpace(creditFacilityListJson))
            {
                creditFacilityListJson = creditFacilityListJson.Replace("\"CreditFacilityId\":null", "\"CreditFacilityId\":0");
                _Entity.CreditFacilityList = JsonConvert.DeserializeObject<List<FertilizerCreditFacilityEntity>>(creditFacilityListJson);
            }
            string purchaseListJson = Request.Form["PurchaseList"];
            if (!string.IsNullOrWhiteSpace(purchaseListJson))
            {
                purchaseListJson = purchaseListJson.Replace("\"PurchaseId\":null", "\"PurchaseId\":0");
                _Entity.PurchaseList = JsonConvert.DeserializeObject<List<FertilizerPurchaseEntity>>(purchaseListJson);
            }
            _Entity.CreatedBy = _LoggedInUser.UserId.ToString();

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());

            return objSocietyModel.InsertFertilizerDemandDetails(_Entity);
        }

        [HttpPost]
        public int SaveTaggingItemForm(TaggedItemFormEntity _Entity)
        { 
            _Entity.CreatedBy = _LoggedInUser.UserId.ToString();

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());

            return objSocietyModel.InsertTaggingItemDetails(_Entity);
        }

        [HttpGet]
        public JsonResult GetTaggingItemByUser()
        {
            var responce = new APIResponce();
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            var ds = objSocietyModel.GetTaggingItemByUser();
            if (ds != null)
            {
                responce.Data = new
                {
                    TaggedItemDetail = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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

        [HttpGet]
        public JsonResult GetTaggedItemDetailById(int TagId = 0)
        {
            var responce = new APIResponce();
            SocietyModel model = new SocietyModel(new DBLogger());
            var ds = model.GetTaggedItemDetailById(TagId);
            if (ds != null)
            {
                responce.Data = new
                {
                    TaggedItemDetailById = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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

        [HttpGet]
        public JsonResult DeleteTaggedItemById(int TagId = 0)
        {
            var responce = new APIResponce();
            SocietyModel model = new SocietyModel(new DBLogger());
            var dt = model.DeleteTaggedItemById(TagId);
            if (dt != null)
            {
                responce.Data = "1";
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

        [HttpGet]
        public string GetElectionList()
        {
            DataTable dt = new SocietyModel().GetElectionList();
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }


        [HttpPost]
        public JsonResult DeleteElection(int ElectionId)
        {
            int Result = new SocietyModel().DeleteElection(ElectionId);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UploadVoterDocument(HttpPostedFileBase ExcludedVoterListDocument)
        {
            try
            {
                if (ExcludedVoterListDocument == null)
                    return Json(new { success = false, message = "File not received" });

                string folder = Server.MapPath("~/Content/Election/VoterDocuments/");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(ExcludedVoterListDocument.FileName);
                string filePath = Path.Combine(folder, fileName);

                ExcludedVoterListDocument.SaveAs(filePath);

                return Json(new { success = true, fileName = fileName });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }




        [HttpPost]
        public JsonResult UploadBylawsDocument(HttpPostedFileBase BylawsDocument)
        {
            try
            {
                if (BylawsDocument == null)
                {
                    return Json(new { success = false, message = "File not received" });
                }

                string originalFileName = Path.GetFileNameWithoutExtension(BylawsDocument.FileName);
                string extension = Path.GetExtension(BylawsDocument.FileName).ToLower();

                // Allow only PDF
                if (extension != ".pdf")
                {
                    return Json(new { success = false, message = "Only PDF files are allowed." });
                }

                // Validate filename (letters, numbers, underscore and hyphen only)
                if (!System.Text.RegularExpressions.Regex.IsMatch(originalFileName, @"^[a-zA-Z0-9_-]+$"))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Invalid File (File name should not contain special characters)"
                    });
                }

                string folder = Server.MapPath("~/Content/Election/BylawsDocument/");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(folder, fileName);

                BylawsDocument.SaveAs(filePath);

                return Json(new
                {
                    success = true,
                    fileName = fileName
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }


        //[HttpPost]
        //public JsonResult UploadBylawsDocument(HttpPostedFileBase BylawsDocument)
        //{
        //    try
        //    {
        //        string ext = "";
        //        if (BylawsDocument != null)
        //            ext = Path.GetExtension(BylawsDocument.FileName);

        //        if (ext.Contains(".pdf"))
        //        {
        //            if (BylawsDocument == null)
        //                return Json(new { success = false, message = "File not received" });

        //            string folder = Server.MapPath("~/Content/Election/BylawsDocument/");
        //            if (!Directory.Exists(folder))
        //                Directory.CreateDirectory(folder);

        //            string fileName = Guid.NewGuid() + Path.GetExtension(BylawsDocument.FileName);
        //            string filePath = Path.Combine(folder, fileName);

        //            BylawsDocument.SaveAs(filePath);

        //            return Json(new { success = true, fileName = fileName });
        //        }
        //        else
        //        {
        //            return Json(new { success = false, message = "File Extension not received" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = ex.Message });
        //    }
        //}

        #region eSign 2.1 process
        [HttpPost]
        public string saveActiondetailsEsign(SocietyDetailsforAction modal)
        {
            string pdfPath = "Success";
            string result = "0";
            string result1 = "0";
            if (modal.actionId == 1 && modal.IsSystemGen == 1)
            {
                pdfPath = GenerateCCSComplainantCertificate(modal);
            }

            if (pdfPath != "" && pdfPath != "Error")
            {
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    string pathString = Server.MapPath("~/Content/CCSLetter");

                    if (!Directory.Exists(pathString))
                    {
                        Directory.CreateDirectory(pathString);
                    }

                    foreach (string files in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[files];
                        string fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                        pdfPath = fileName;
                        modal.DisposeFile = fileName;

                        if (Directory.Exists(pathString))
                        {
                            file.SaveAs(Path.Combine(pathString, fileName));
                        }
                    }
                    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                    int DistrictId = _LoggedInUser.DistrictId;

                    modal.ComplaintId = modal.complainerId.ToString();
                    modal.complainant_letter = modal.DisposeFile;
                    result = new SocietyModel(new DBLogger()).InsertUpdateCCSComplaintLetter(modal);

                    if (result != "-1")
                    {
                        if (Request.Url.Host.Contains("localhost"))
                        {

                        }
                        else
                        {
                            string link = (Request.Url.Host.Contains("localhost") ? "http://localhost:27466/" : "http://rajsahakarapp.rajasthan.gov.in/") + "/Content/CCSLetter/" + modal.complainant_letter + ".pdf";
                            string status = new EsanchaarController().SendSocietySMS("Regarding society complaint a letter is issued. click on " + link + " to open", modal.MobileNo);
                        }
                        result = "1";

                    }

                }

                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                result = objSocietyModel.saveActiondetail(modal);
            }
            return result + "#" + result1;

        }

        [HttpPost]
        public string GenerateCCSComplainantCertificate(SocietyDetailsforAction model)
        {
            string result = "";
            SocietyModel objNew = new SocietyModel(new DBLogger());
            string uniqeId = Guid.NewGuid().ToString();
            try
            {
                string RO = "";
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                int DistrictId = _LoggedInUser.DistrictId;

                StringBuilder sb = new StringBuilder();
                string filepath = Server.MapPath("~/Content/CCSLetter");
                string fontSize = "font-size: 15px;";

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                StringBuilder sb1 = new StringBuilder();
                int count = 0;

                string imgae = Server.MapPath("~/Content/Images/anupgarh.png");

                sb = new StringBuilder();
                sb1 = new StringBuilder();
                string FileNameComplainant = "cc_sc_" + model.complainerId;
                sb.Append("<div style='padding-left:50px;padding-right:50px;padding-top:5px'>");
                sb.Append("<html><body><table border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");
                sb.Append("<tr><td colspan='5' style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                sb.Append("<img style='width:90px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                sb.Append("</td></tr>");
                if (!string.IsNullOrWhiteSpace(model.HeaderImage))
                {
                    sb.Append("<tr><td colspan='5' style='text-align:center;'><img src='" + Server.MapPath("~/Content/Images/" + model.HeaderImage) + "' /></td></tr><br>");
                }
                else
                {
                    sb.Append("<tr><td colspan='5' style='text-align:center;font-size:32px;'><b>" + UnicodeToKrutidev.GetKrutidev("कार्यालय उप रजिस्ट्रार, सहकारी समितियां, " + model.District + "") + "  </b></td></tr>");
                    sb.Append("<tr><td colspan='5' style='text-align:center;font-size:14px;'><b>" + model.OfficeAddress + "  </b><br><br></td></tr>");
                }


                sb.Append("<tr><td colspan='5' style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><br><br><b><label style='font-size:20px;'>" + UnicodeToKrutidev.GetKrutidev("नोटिस बाबत उपस्थिति परिवाद") + "<br><br><br><br></label><label  style='font-size:25px;'> </label></b>");
                sb.Append("</td></tr><br><br><br>");

                sb.Append("<tr>");
                sb.Append("<td colspan='3' style='text-align:left;'>  वास्ते :-<br> " + model.ComplainerName + ",<br>" + model.ComplainerFatherName + ",<br>" + model.Address + " <br/> Mobile Mo- " + model.MobileNo + (!string.IsNullOrWhiteSpace(model.Email) ? "<br/> Email- " + model.Email + "" : "") + "</td>");
                sb.Append("<td colspan='2' style='text-align:right;'> परिवाद संख्या  :" + model.ReceiptNo + "<br><br><br><br><br><br><br><br> </td>");
                sb.Append("</tr><br><br>");
                sb.Append("<br><tr>");
                sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'><br><br><p style='text-indent:50px;'> आप द्वारा The Banning of Unregulated Deposit Scheme Act-2019 के तहत शिकायत / परिवाद दोषी सोसाइटी " + model.SocietyName + " के विरुद्ध प्रस्तुत किया है, जिस पर सोसाइटी के विरुद्ध उचित विधिक कार्यवाही के अनुक्रम में अधिनियम की धारा - 4 सपठित धारा - 22 के तहत दंडनीय अपराध हेतु सम्बन्धित Designated court में परिवाद / इस्तागा प्रस्तुत किया जाना है</p></ td > ");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'><p style='text-indent:50px;'> अतः इस नोटिस के माध्यम से यह सूचित किया जाता है कि आप अपने परिवाद के समर्थन मे जिन दस्तावेजो पर निर्भर रहते हो यथा एफडीआर / पासबुक /निवेश का दस्तावेज /सोसाइटी में मेंबरशिप का का प्रमारपत्र आधार कार्ड की स्वप्रमारित प्रतिलिपियों मय निवेश की दो पासपोर्ट साइज फोटो के दिनांक  " + model.Presentdate + "को इस कार्यालय मे उपस्थित होवें |</p> </td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'><p style='text-indent:50px;'>आप निर्धारित तिथि को उपस्थित नहीं होने की सूरत में यह अवधारणा की जावेगी, की  आप परिवाद पर  कोई कार्यवाही नहीं चाहते है | यह नोटिस आज दिनांक। " + DateTime.Now.ToString("dd/MM/yyyy") + " को मेरे हस्ताक्षर एवं मोहर  से जारी किया गया है |</p></td><br>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td colspan='5' style='text-align:left; width: 100%;height:50px;font-size:10px;border-top:1px solid #ccc;padding:3px;'><b>Note:</b> This is a digitally signed certificate and does not required any physical signature. </td>");
                sb.Append("</tr>");
                sb.Append("</table></body></html>");

                sb.Append("</div>");

                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "20px"));
                model.Notice = sb.ToString().Replace("<br>", "<br/>");
                if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileNameComplainant + ".pdf", ""))
                {
                    model.complainant_letter = FileNameComplainant + ".pdf";
                    result += (result != "" ? "#" : "") + filepath + "/" + FileNameComplainant + ".pdf";
                }
                count = count + 1;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.SocietyControler", FunctionName = " GenerateCCSLetter(DataTable List, string FileName, string Titel)" }, ex);
                result = "Error";
            }
            return result;
        }


        [HttpPost]
        public JsonResult GenerateCCSComplainantLetter(SocietyDetailsforAction requestModel)
        {
            try
            {
                var pdfPath = "";
                string fileName = "";
                string documentName = "";
                if (requestModel != null)
                {
                    pdfPath = GenerateCCSComplainantCertificate(requestModel);
                    documentName = Path.GetFileName(pdfPath);
                }
                else { pdfPath = "Fail"; }

                if (pdfPath != "Fail")
                {
                    if (Request.Files != null && Request.Files.Count > 0)
                    {
                        string pathString = Server.MapPath("~/Content/CCSLetter");
                        if (Directory.Exists(pathString)) { Directory.CreateDirectory(pathString); }
                        foreach (string files in Request.Files)
                        {
                            HttpPostedFileBase file = Request.Files[files];
                            fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                            if (Directory.Exists(pathString))
                            { file.SaveAs(Path.Combine(pathString, fileName)); }
                        }
                    }

                    return Json(new
                    {
                        Success = true,
                        DocPath = pdfPath,
                        DocType = "CCS Complainant",
                        DocName = documentName,
                        DisposeFile = fileName,
                        ReturnPath = Url.Action("CCSComplaint", "Society"),
                        Location = "",
                        Designation = ""
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Success = false,
                        Message = "PDF generation failed.",
                        DocPath = "",
                        DocType = "",
                        DocName = "",
                        ReturnPath = "",
                        DisposeFile = "",
                        Location = "",
                        Designation = ""
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Society", FunctionName = "GenerateCCSComplainantLetter(SocietyDetailsforAction requestModel)" }, ex);

                return Json(new
                {
                    Success = false,
                    Message = ex.Message,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    Location = "",
                    Designation = ""
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> StartEsign(string DocType, string docid, string SocietyIDEnc = "", string DocumentPath = "", string DocumentName = "", string ReturnPath = "", string Remark = "", int? StatusId = null, bool IsAmendment = false, string AmendmendAmount = "", string ReceiptNumber = "", SocietyDetailsforAction requestData = null, string Location = "", string Designation = "")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            EsignModel eSignModel = new EsignModel(new DBLogger());
            var detailsJson = JsonConvert.SerializeObject(requestData);
            string UAT_BaseUrl = "http://localhost:27466/eSign/EspResponse";
            string Prod_BaseUrl = "https://rajsahakarapp.rajasthan.gov.in/eSign/EspResponse";

            try
            {
                if (_LoggedInUser != null)
                {
                    string responseUrl = ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")) ? UAT_BaseUrl : Prod_BaseUrl;
                    //string pdfPath = Server.MapPath(DocumentPath);
                    string pdfPath = string.Empty;
                    switch (DocType)
                    {
                        case "CCS NoticeIssue":
                            pdfPath = DocumentPath;
                            break;
                        default:
                            pdfPath = Server.MapPath(DocumentPath);
                            break;
                    }


                    string prn = GeneratePRN();
                    string baseUrl = ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")) ? "http://localhost:27466/" : "https://rajsahakarapp.rajasthan.gov.in/";
                    ReturnPath = baseUrl + ReturnPath;
                    var existing = eSignModel.GetTransactionByPRN(prn);
                    if (existing.Rows.Count > 0)
                    {
                        return Json(new { success = false, message = "Transaction already exists." });
                    }

                    // INSERT TRANSACTION
                    eSignModel.InsertEsignTransaction(prn, pdfPath, _LoggedInUser.SSOID, _LoggedInUser.UserId, _LoggedInUser.RoleId, "INITIATED", DocType, ReturnPath, docid, SocietyIDEnc, Remark, DocumentName, StatusId, IsAmendment, AmendmendAmount, ReceiptNumber, detailsJson, "0", "0", "0", _LoggedInUser.DistrictId, "0", "0", "0", "0", "0", "0", _LoggedInUser.UserRoleDeptMappingId);

                    SignedXmlResponse signedXmlResp;
                    signedXmlResp = await eSignModel.GenerateSignedXmlAsync(pdfPath, prn, responseUrl, false, DocType, Location, Designation);

                    // LOG XML GENERATION API
                    eSignModel.InsertEsignApiLog(prn, "XML_GENERATE", "PDFPath: " + pdfPath, JsonConvert.SerializeObject(signedXmlResp), signedXmlResp?.data?.responseCode, signedXmlResp?.data?.responseMsg, signedXmlResp?.status);

                    if (signedXmlResp != null && signedXmlResp.status == "SUCCESS")
                    {
                        // UPDATE STATUS
                        eSignModel.UpdateEsignStatus(prn, "XML_GENERATED", signedXmlResp.data.txnId);

                        return Json(new
                        {
                            success = true,
                            xml = Encoding.UTF8.GetString(Convert.FromBase64String(signedXmlResp.data.signedXMLData))
                        });
                    }
                    else
                    {
                        eSignModel.UpdateEsignStatus(prn, "FAILED", null);

                        return Json(new
                        {
                            success = false,
                            message = "XML Generation Failed"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }

                return Json(new { success = false, message = "Session out !!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }




        //Added By 18-05-2026 

        [HttpPost]
        public string ESignAuditReOrder(string OldOrderNo, string AuditerId = "", string AuditBy = "", string Ids = "", string AssignedIds = "", string Status = "", string IsFirm = "", string IsRequested = "", int DistrictId = 0, int FinancialYearId = 0, string FinancialYearName = "", string AuditTillDate = "", string IsForced = "0", string AuditByDepartmentorCA = "0", string DeleteResions = "")//By Empanelled "true"    By Department  "false"
        {
            string message = string.Empty;
            string succmsg = "Some Error";

            try
            {
                if (IsRequested == "0")
                    succmsg = GenerateAuditAssignbydeprtOrderLatter(OldOrderNo, AuditerId, AuditBy, Ids, Status, IsFirm, FinancialYearId, FinancialYearName.Replace(" ", ""), AuditTillDate, IsForced, AuditByDepartmentorCA, DeleteResions, AssignedIds, IsRequested);
                if (IsRequested == "1")
                    succmsg = GenerateAuditAssignbydeprtOrderLatter(OldOrderNo, AuditerId, AuditBy, Ids, Status, IsFirm, AuditTillDate, IsRequested);
            }
            catch (Exception ex)
            {
                succmsg = ex.Message;
            }

            return succmsg;
        }

        [HttpPost]
        public string GenerateAuditAssignbydeprtOrderLatter(string OldOrderNo, string AuditerId = "", string AuditBy = "", string Ids = "", string Status = "", string IsFirm = "", int FinancialYearId = 0, string FinancialYearName = "", string AuditTillDate = "", string IsForced = "False", string AuditByDepartmentorCA = "", string DeleteResions = "", string AssignedIds = "", string IsRequested = "")
        {
            AuditorController objAC = new AuditorController();
            AuditOrderEntity objAE = new AuditOrderEntity();
            try
            {
                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                var IdsArr = Ids.Split(',');

                string DistName = "";
                string DistNameHi = "";
                string Location = "";
                string fontSize = "font-size: 13px;";
                string saveIDs = "";
                DataTable Dt = null;

                if (_LoggedInUser.LevelId == 3)
                {
                    DistName = _LoggedInUser.DISTRICT_ENG;
                    DistNameHi = _LoggedInUser.DISTRICT_MANGAL;
                    Location = "जिले";
                }
                else if (_LoggedInUser.LevelId == 2)
                {
                    DistName = _LoggedInUser.DIVISION_ENG;
                    DistNameHi = _LoggedInUser.DIVISION_MANGAL;
                    Location = "खण्ड";
                }
                else if (_LoggedInUser.LevelId == 1)
                {
                    Location = "राज्य";
                }


                string OfficerName = "";
                string CurrentFYear = "";
                string SocietyName = "";
                string OrderNo = "";

                {
                    int AuditByAD = (AuditByDepartmentorCA == "true" ? 1 : 2);
                    Dt = objSocietyModel.GetAuditorAndSocietyDetailByIds(Ids, AuditerId, AuditByAD, FinancialYearId);
                }
                objAE.FileName = "";
                objAE.SocietyCount = Dt.Rows.Count;
                objAE.SocietyCount = Dt.AsEnumerable().GroupBy(row => row["SocietyName"]).Count();

                objAE.InActiveFlag = 0;
                OrderNo = objAC.SaveAuditOrder(objAE);
                objAE.OrderNo = OrderNo;

                if (OrderNo != "ERROR")
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:10px'>");
                    sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px; '>");
                    sb.Append("<tr><td colspan=2 style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                    sb.Append("<img style='width:100px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td colspan=2 style='text-align:center;" + fontSize + "'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                    if (_LoggedInUser.LevelId == 3)
                        sb.Append("<tr><td colspan=2 style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय- विशेष लेखा परीक्षक सहकारी समितियॉं, DistrictNameHi") + "</label></b></td></tr>");
                    else if (_LoggedInUser.LevelId == 2)
                        sb.Append("<tr><td colspan=2 style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय- रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड , DistrictNameHi") + "</label></b></td></tr>");
                    else if (_LoggedInUser.LevelId == 1)
                        sb.Append("<tr><td colspan=2 style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय- रजिस्ट्रार, सहकारी समितियॉं राजस्थान ,जयपुर") + "</label></b></td></tr>");

                    sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");

                    sb.Append("<tr><td align='left' style='margin-top:15px;'> क्रमांक फा." + OrderNo + " </td><td align='right'> दिनांक " + DateTime.Now.ToString("dd/MM/yyyy") + "</td></tr>");
                    sb.Append("<tr><td align='Left'  colspan=2 > संदर्भ :- " + OldOrderNo + " </td></tr>");
                    sb.Append("<tr><td align='Left'  colspan=2 >&nbsp; </td></tr>");
                    sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><u><b> ऑडिट आवंटन आदेश (संसोधित)</b></u></td></tr>");
                    if (IsForced == "True")
                    {
                        sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><b> (रा.स.सो.अधि.धारा 54(2) व नियम 73(8) के तहत) </b></td></tr>");
                        sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><b> (सोसाइटियों के वर्ग / सोसायटी हेतु) </b></td></tr>");
                    }
                    else if (FinancialYearId > 48)//Current
                    {
                        sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><b> (रा.स.सो.अधि.धारा 54(2) व नियम 73(7) के तहत) </b></td></tr>");
                    }
                    else if (FinancialYearId < 49)//FOR BACK LOG
                    {
                        sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><b> (रा.स.सो.अधि.धारा 54(2) व नियम 73(8) के तहत) </b></td></tr>");
                    }

                    sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");

                    if (IsForced == "True")
                    {
                        sb.Append("<tr><td align='justify'  colspan=2 style='margin-top:15px;'> <p>&nbsp;&nbsp;&nbsp; राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 (2) तथा राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(8) अनुसार नियम 73(4) से 73(7) में अंतर्विष्ट किसी बात के होने पर भी, रजिस्ट्रार आदेश द्वारा, किसी विशिस्ट कालावधि के लिए किसी सोसायटी या सोसाइटियों के किसी वर्ग के लेखाओं की लेखापरीक्षा कराने के लिए लेखापरीक्षक (लेखापरीक्षको) या लेखा परीक्षा फर्म (फर्मो ) को नियुक्त कर सकेगा | अधोहस्ताक्षरकर्ता को यह समाधान हो गया है , कि निम्नलिखित सहकारी समितियों की लेखा परीक्षा हेतु उक्त प्रावधानों के अंतर्गत विभाग स्तर से लेखा परीक्षक नियुक्त किया जाना आवश्यक है ' । </p></td></tr>");
                        sb.Append("<tr><td align='center'   colspan=2 >&nbsp; </td></tr>");
                        if (_LoggedInUser.LevelId == 3)
                            sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName विशेष लेखा परीक्षक सहकारी समितियॉ DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) तथा राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73 (8) के तहत प्रदत्त शक्तियों का प्रयोग करते हुए निम्नलिखित सहकारी समितियों के वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + " InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + "  को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                        else if (_LoggedInUser.LevelId == 2)
                        {
                            if (AuditerId.ToString().Contains(","))
                            {
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(8) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए SocietyName की वर्ष FinYear की लेखा परीक्षा हेतु निम्न लिखित लेखा निरीक्षकों की नियुक्ति करता / करती हूँ | </p></td></tr>");
                            }
                            else
                            {
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) तथा राजस्थान सहकारी सोसाइटी नियम 2003 के नियम 73(8) के तहत प्रदत्त शक्तियों का प्रयोग करते हुए निम्न सहकारी समितियों के वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + " InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + "  को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                            }

                        }

                        else if (_LoggedInUser.LevelId == 1)
                            sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, संयुक्त मुख्य अंकेक्षक ( जनरल ) सहकारी समितियॉं, राजस्थान जयपुर, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) तथा राजस्थान सहकारी सोसाइटी नियम 2003 के नियम 73 (8) के तहत प्रदत्त शक्तियों का प्रयोग करते हुए निम्न सहकारी समितियों के वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + " InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + "  को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");

                        sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");

                        if (AuditerId.ToString().Contains(","))
                        {
                            sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; उपरोक्त सहकारी समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी |</p></td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; निम्न सहकारी समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54  एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी |</p></td></tr>");
                        }


                    }
                    else
                    {
                        if (FinancialYearId > 48)
                        {
                            sb.Append("<tr><td align='justify'  colspan=2 style='margin-top:15px;'> <p>&nbsp;&nbsp;&nbsp; राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(4) के अनुसार निम्नलिखित सहकारी समितियॉं नियत समय में लेखा परीक्षक की नियुक्ति कर सुचना प्रस्तुत करने में विफल रही है ।  </p></td></tr>");
                            sb.Append("<tr><td align='center'   colspan=2 >&nbsp; </td></tr>");
                            if (_LoggedInUser.LevelId == 3)
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, विशेष लेखा परीक्षक सहकारी समितियॉ DistrictNameHi , राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(7) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                            else if (_LoggedInUser.LevelId == 2)
                            {
                                if (AuditerId.ToString().Contains(","))
                                {
                                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(7) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए SocietyName की वर्ष FinYear की लेखा परीक्षा हेतु निम्न लिखित लेखा निरीक्षकों की नियुक्ति करता / करती हूँ | </p></td></tr>");
                                }
                                else
                                {
                                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(7) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                                }
                            }
                            else if (_LoggedInUser.LevelId == 1)
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, संयुक्त मुख्य अंकेक्षक ( जनरल ) सहकारी समितियॉं, राजस्थान जयपुर, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(7) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");

                            sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");

                            if (AuditerId.ToString().Contains(","))
                            {
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; उपरोक्त सहकारी समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी |</p></td></tr>");
                            }
                            else
                            {
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; निम्न सहकारी समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी |</p></td></tr>");
                            }


                        }
                        else  //FOR BACK LOG
                        {
                            sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(3) के अनुसार प्रत्येक सोसाइटी के लेखाओ की लेखा परीक्षा उस वित्तीय वर्ष की जिससे ऐसे लेखे सम्बंधित है ,समाप्ति के छ माह के भीतर-भीतर की जाएगी | परन्तु  निम्नलिखित सहकारी समितियों द्वारा वर्ष " + FinancialYearName + " तक की ऑडिट करवाकर ऑडिट रिपोर्ट प्रस्तुत नहीं की गयी है | जिससे स्पष्ट है कि निम्नलिखित सहकारी समितियां अधिनियम के उक्त प्रावधानों की पालना करने में विफल रही है इसलिए इन सहकारी समितियों की वर्ष " + FinancialYearName + " तक बकाया समस्त लेखा परीक्षा हेतु विभाग स्तर से लेखा परीक्षक की नियुक्ति किया जाना आवश्यक हो गया है | </p></td></tr>");
                            sb.Append("<tr><td align='center'   colspan=2 >&nbsp; </td></tr>");

                            if (_LoggedInUser.LevelId == 3)
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, विशेष लेखा परीक्षक सहकारी समितियॉ DistrictNameHi , राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(8) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष " + FinancialYearName + " की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                            else if (_LoggedInUser.LevelId == 2)
                            {
                                if (AuditerId.ToString().Contains(","))
                                {
                                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(8) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए SocietyName की वर्ष FinYear की लेखा परीक्षा हेतु निम्न लिखित लेखा निरीक्षकों की नियुक्ति करता / करती हूँ | </p></td></tr>");
                                }
                                else
                                {
                                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(8) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष " + FinancialYearName + " की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                                }
                            }

                            else if (_LoggedInUser.LevelId == 1)
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, संयुक्त मुख्य अंकेक्षक ( जनरल ) सहकारी समितियॉं, राजस्थान जयपुर, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(8) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष " + FinancialYearName + " की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");


                            sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                            if (AuditerId.ToString().Contains(","))
                            {
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; उपरोक्त सहकारी समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी तथा लेखापरीक्षकगण आवंटीत समिति में अविलम्ब प्रारंभ कर दिनांक " + AuditTillDate + " तक अंकेक्षण कार्य सम्पादित करेंगे |</p></td></tr>");
                            }
                            else
                            {
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; निम्न सहकारी समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी तथा लेखापरीक्षकगण आवंटीत समिति में अविलम्ब प्रारंभ कर दिनांक " + AuditTillDate + " तक अंकेक्षण कार्य सम्पादित करेंगे |</p></td></tr>");
                            }


                        }
                    }

                    sb.Append("<tr><td align='center'  colspan=2 >");
                    sb.Append("<table style='width: 100%; margin-top:20px;' border='1' cellspacing='0' cellpadding='5'>");

                    if (AuditerId.ToString().Contains(","))
                    {
                        sb.Append("<tr><td align='center'style='width:70px;'> क्र. स. </td><td align='center'> लेखा निरीक्षक </td><td align='center'> सहकारी समिति  </td><td align='center'> कार्यालय </td></tr>");
                    }
                    else
                    {
                        sb.Append("<tr><td align='center'style='width:70px;'> क्र. स. </td><td align='center'> सहकारी समिति  </td><td align='center'> स्थान </td></tr>");
                    }


                    int i1 = 0;
                    foreach (DataRow dr in Dt.Rows)
                    {
                        i1++;
                        sb.Append("<tr><td  align='left'  > " + i1.ToString() + "</td>");

                        if (AuditerId.ToString().Contains(","))
                        {
                            sb.Append("<td  align='left'  > " + dr["OfficerNameEn"].ToString() + " </td>");
                            sb.Append("<td  align='left'  > " + dr["SocietyName"].ToString() + " (" + dr["RegistrationNo"].ToString() + ") </td>");
                            sb.Append("<td  align='left'  > " + dr["OfficeNameEn"].ToString() + " </td></tr>");
                        }
                        else
                        {
                            sb.Append("<td  align='left'  > " + dr["SocietyName"].ToString() + " (" + dr["RegistrationNo"].ToString() + ") </td>");
                            sb.Append("<td  align='left'  > " + (dr["SocietyPlaceEn"].ToString() != "" ? (dr["SocietyPlaceEn"].ToString() + " , ") : " ") + dr["DISTRICT_ENG"].ToString() + "</td></tr>");
                        }
                        if (AuditerId.ToString().Contains(","))
                        {
                            if (!OfficerName.Contains(dr["OfficerNameEn"].ToString()))
                                OfficerName = AuditBy == "1" ? dr["FirmName"].ToString() : OfficerName + ", " + dr["OfficerNameEn"].ToString();
                        }
                        else
                            OfficerName = AuditBy == "1" ? dr["FirmName"].ToString() : dr["OfficerNameEn"].ToString();
                        CurrentFYear = dr["CurrentFYear"].ToString() == "" ? FinancialYearName : dr["CurrentFYear"].ToString();
                        saveIDs = saveIDs + dr["Id"].ToString() + ",";
                        SocietyName = dr["SocietyName"].ToString().ToUpper();
                    }
                    if (OfficerName.Contains(", ") && OfficerName.StartsWith(","))
                    {
                        OfficerName = OfficerName.Substring(1);
                    }
                    sb.Append("</table></td></tr>");
                    sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                    sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                    sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                    sb.Append("</table ></div>");

                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    string fileName = Guid.NewGuid().ToString("N").ToUpper() + ".pdf";
                    string textVal = sb.ToString().Replace("<br>", "<br/>").Replace("DistrictNameHi", DistNameHi).Replace("SocietyName", SocietyName).Replace("DistrictNameHi", DistNameHi).Replace("FinYear", CurrentFYear).Replace("InspectorName", OfficerName).Replace("RegitrarName", _LoggedInUser.OfficerNameEn);
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(textVal));
                    string FilePath = "../Content/AuditReport/" + fileName;
                    if (COMMON_FUNCTION.GeneratePDF(sb1, Server.MapPath(FilePath)))
                    {
                        return JsonConvert.SerializeObject(new
                        {
                            Success = true,
                            DocPath = FilePath,
                            DocType = "Audit Order",
                            DocName = fileName,
                            OrderNo = OrderNo,
                            IsRequested = IsRequested,
                            ReturnPath = Url.Action("AuditOrderListReport", "MIS"),
                            Location = "",
                            Designation = ""
                        });
                    }
                    else
                    {
                        objAE.InActiveFlag = 1;
                        objAC.SaveAuditOrder(objAE);
                        return JsonConvert.SerializeObject(new
                        {
                            Success = false,
                            DocPath = "",
                            DocType = "",
                            DocName = "",
                            OrderNo = "",
                            IsRequested = "",
                            ReturnPath = "",
                            Message = "PDF generation failed.",
                            Location = "",
                            Designation = ""
                        });
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(new
                    {
                        Success = false,
                        DocPath = "",
                        DocType = "",
                        DocName = "",
                        OrderNo = "",
                        IsRequested = "",
                        ReturnPath = "",
                        Message = "Order generation failed.",
                        Location = "",
                        Designation = ""
                    });
                }
            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Controller.Society",
                    FunctionName = " GenerateAuditAssignbydeprtOrderLatter(string OldOrderNo, string AuditerId = \"\", string AuditBy = \"\", string Ids = \"\", string Status = \"\", string IsFirm = \"\", string Tran = \"\", int FinancialYearId = 0, string FinancialYearName = \"\", string AuditTillDate = \"\", string IsForced = \"False\", string AuditByDepartmentorCA = \"\", string DeleteResions = \"\", string Assigne"
                }, ex);
                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    OrderNo = "",
                    IsRequested = "",
                    ReturnPath = "",
                    Message = ex.Message,
                    Location = "",
                    Designation = ""
                });
            }

        }

        [HttpPost]
        public string GenerateAuditAssignbydeprtOrderLatter(string OldOrderNo, string AuditerId = "", string AuditBy = "", string Ids = "", string Status = "", string IsFirm = "", string AuditTillDate = "", string DeleteResions = "", string AssignedIds = "", string IsRequested = "")
        {
            string OrderNo = "";
            AuditorController objAC = new AuditorController();
            AuditOrderEntity objAE = new AuditOrderEntity();

            try
            {
                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                DataTable Dt = null;
                var IdsArr = Ids.Split(',');

                string DistName = "";
                string DistNameHi = "";

                string saveIDs = "";
                if (_LoggedInUser.LevelId == 3)
                {
                    DistName = _LoggedInUser.DISTRICT_ENG;
                    DistNameHi = _LoggedInUser.DISTRICT_MANGAL;
                }
                else if (_LoggedInUser.LevelId == 2)
                {
                    DistName = _LoggedInUser.DIVISION_ENG;
                    DistNameHi = _LoggedInUser.DIVISION_MANGAL;
                }
                else if (_LoggedInUser.LevelId == 1)
                {
                    DistName = "Rajasthan";
                }

                #region 

                objAE.FileName = "";
                objAE.SocietyCount = 1;
                objAE.InActiveFlag = 0;
                OrderNo = objAC.SaveAuditOrder(objAE);
                objAE.OrderNo = OrderNo;
                #endregion

                string OfficerName = "";
                string CurrentFYear = "";
                string SocietyName = "";

                string fontSize = "font-size: 13px;";

                Dt = objSocietyModel.GetAuditorAndSocietyDetailByIds(Ids, AuditerId);

                StringBuilder sb = new StringBuilder();
                sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:10px'>");
                sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px; '>");
                sb.Append("<tr><td colspan=2 style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                sb.Append("<img style='width:100px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=2 style='text-align:center;" + fontSize + "'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");

                if (_LoggedInUser.LevelId == 3)
                    sb.Append("<tr><td colspan=2 style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय- विशेष लेखा परीक्षक सहकारी समितियॉं, DistrictNameHi") + "</label></b></td></tr>");
                else if (_LoggedInUser.LevelId == 2)
                    sb.Append("<tr><td colspan=2 style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय- रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड , DistrictNameHi") + "</label></b></td></tr>");
                else if (_LoggedInUser.LevelId == 1)
                    sb.Append("<tr><td colspan=2 style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय- रजिस्ट्रार, सहकारी समितियॉं राजस्थान ,जयपुर") + "</label></b></td></tr>");

                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                sb.Append("<tr><td align='left' style='margin-top:15px;'> क्रमांक फा. " + OrderNo + " </td><td align='right'> दिनांक " + DateTime.Now.ToString("dd/MM/yyyy") + "</td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><u><b> ऑडिट आवंटन आदेश </b></u></td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><b> (रा.स.सो.अधि.धारा 54(2) व नियम 73(4) के तहत) </b></td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                sb.Append("<tr><td align='justify'  colspan=2 style='margin-top:15px;'> <p>&nbsp;&nbsp;&nbsp; राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(4) के अनुसार निम्नलिखित सहकारी समितियों द्वारा विभागीय निरीक्षक से वर्ष FinYear की लेखा परीक्षा करवाने की अभ्यर्थना प्रस्तुत की है | </p></td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");

                if (_LoggedInUser.LevelId == 3)
                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, विशेष लेखा परीक्षक सहकारी समितियॉ DistrictNameHi , राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(4) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                else if (_LoggedInUser.LevelId == 2)
                {
                    if (AuditerId.ToString().Contains(","))
                    {
                        sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(4) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए SocietyName की वर्ष FinYear की लेखा परीक्षा हेतु निम्न लिखित लेखा निरीक्षकों की नियुक्ति करता / करती हूँ | </p></td></tr>");
                    }
                    else
                    {
                        sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(4) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                    }
                }
                else if (_LoggedInUser.LevelId == 1)
                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, संयुक्त मुख्य अंकेक्षक ( जनरल ) सहकारी समितियॉं, राजस्थान जयपुर, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(4) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");

                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                if (AuditerId.ToString().Contains(","))
                {
                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; उपरोक्त समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी |</p></td></tr>");
                }
                else
                {
                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; निम्न समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी |</p></td></tr>");
                }
                sb.Append("<tr><td align='center'  colspan=2 >");
                sb.Append("<table style='width: 100%; margin-top:20px;' border='1' cellspacing='0' cellpadding='5'>");
                if (AuditerId.ToString().Contains(","))
                {
                    sb.Append("<tr><td align='center'style='width:70px;'> क्र. स. </td><td align='center'> लेखा निरीक्षक </td><td align='center'> कार्यालय </td></tr>");
                }
                else
                {
                    sb.Append("<tr><td align='center'style='width:70px;'> क्र. स. </td><td align='center'> सहकारी समिति  </td><td align='center'> स्थान </td></tr>");/*<td align='center'>Officer/Firm</td>*/
                }
                int i1 = 0;
                foreach (DataRow dr in Dt.Rows)
                {
                    i1++;
                    sb.Append("<tr><td  align='left'  > " + i1.ToString() + "</td>");
                    if (AuditerId.ToString().Contains(","))
                    {
                        sb.Append("<td  align='left'  > " + dr["OfficerNameEn"].ToString() + " </td>");
                        sb.Append("<td  align='left'  > " + dr["OfficeNameEn"].ToString() + " </td></tr>");
                    }
                    else
                    {
                        sb.Append("<td  align='left'  > " + dr["SocietyName"].ToString() + " (" + dr["RegistrationNo"].ToString() + ") </td>");
                        sb.Append("<td  align='left'  > " + (dr["SocietyPlaceEn"].ToString() != "" ? (dr["SocietyPlaceEn"].ToString() + " , ") : " ") + dr["DISTRICT_ENG"].ToString() + "</td></tr>");
                    }
                    OfficerName = dr["OfficerNameEn"].ToString() != "" ? dr["OfficerNameEn"].ToString() : dr["FirmName"].ToString();
                    CurrentFYear = dr["CurrentFYear"].ToString();
                    saveIDs = saveIDs + dr["Id"].ToString() + ",";

                    SocietyName = dr["SocietyName"].ToString().ToUpper();


                }
                sb.Append("</table></td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                sb.Append("</table ></div>");


                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                string fileName = Guid.NewGuid().ToString("N").ToUpper() + ".pdf";
                string textVal = sb.ToString().Replace("<br>", "<br/>").Replace("DistrictNameHi", DistNameHi).Replace("SocietyName", SocietyName).Replace("FinYear", CurrentFYear).Replace("InspectorName", OfficerName).Replace("RegitrarName", _LoggedInUser.OfficerNameEn);
                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(textVal));

                string FilePath = "../Content/AuditReport/" + fileName;
                if (COMMON_FUNCTION.GeneratePDF(sb1, Server.MapPath(FilePath)))
                {

                    return JsonConvert.SerializeObject(new
                    {
                        Success = true,
                        DocPath = FilePath,
                        DocType = "Audit Order",
                        DocName = fileName,
                        OrderNo = OrderNo,
                        IsRequested = IsRequested,
                        ReturnPath = Url.Action("AuditOrderListReport", "MIS"),
                        Location = "",
                        Designation = ""

                    });

                    //objRM = COMMON_FUNCTION.EsinghPDF(Tran, Server.MapPath(FilePath), DistName);

                    //if (objRM.data == "Success")
                    //{
                    //    if (saveIDs.Length > 0)
                    //        saveIDs = saveIDs.Substring(0, saveIDs.Length - 1);

                    //    ResultIntStr = objSocietyModel.saveAuditAssignbydeprt(AuditerId, AuditBy, saveIDs, Status, IsFirm, FilePath, _LoggedInUser.UserRoleDeptMappingId, OrderNo).ToString();
                    //    if (ResultIntStr != "-1")
                    //        ResultIntStr = "Success";
                    //    else
                    //    {
                    //        objAE.InActiveFlag = 1;
                    //        objAC.SaveAuditOrder(objAE);
                    //    }
                    //}
                }
                else
                {
                    return JsonConvert.SerializeObject(new
                    {
                        Success = false,
                        DocPath = "",
                        DocType = "",
                        DocName = "",
                        OrderNo = "",
                        IsRequested = "",
                        ReturnPath = "",
                        Message = "PDF generation failed.",
                        Location = "",
                        Designation = ""
                    });
                }
            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Controller.Society",
                    FunctionName = "GenerateAuditAssignbydeprtOrderLatter(string OldOrderNo, string AuditerId = \"\", string AuditBy = \"\", string Ids = \"\", string Status = \"\", string IsFirm = \"\", string Tran = \"\", string AuditTillDate = \"\", string DeleteResions = \"\", string AssignedIds = \"\")"
                }, ex);
                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    OrderNo = "",
                    IsRequested = "",
                    ReturnPath = "",
                    Message = ex.Message,
                    Location = "",
                    Designation = ""
                });
            }
        }

        [HttpPost]
        public async Task<JsonResult> AuditStartEsign(string DocType, string docid, string SocietyIDEnc = "", string DocumentPath = "", string DocumentName = "", string ReturnPath = "", string Remark = "", int? StatusId = null, bool IsAmendment = false, string AmendmendAmount = "", string ReceiptNumber = "", string AuditBy = "", string AssignedIds = "", string IsFirm = "", string DistrictId = "", string FinancialYearId = "", string FinancialYearName = "", string AuditTillDate = "",
          string IsForced = "", string AuditByDepartmentorCA = "", string DeleteResions = "", string UserRoleDeptMappingId = "0", string PurposeOfLoanOther = "", string Location = "", string Designation = "", string DocIds = "")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            EsignModel eSignModel = new EsignModel(new DBLogger());
            string UAT_BaseUrl = "http://localhost:27466/eSign/EspResponse";
            string Prod_BaseUrl = "https://rajsahakarapp.rajasthan.gov.in/eSign/EspResponse";

            try
            {
                if (_LoggedInUser != null)
                {
                    string responseUrl = ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")) ? UAT_BaseUrl : Prod_BaseUrl;
                    string pdfPath = Server.MapPath(DocumentPath);

                    string prn = GeneratePRN();
                    string baseUrl = ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")) ? "http://localhost:27466/" : "https://rajsahakarapp.rajasthan.gov.in/";
                    ReturnPath = baseUrl + ReturnPath;
                    var existing = eSignModel.GetTransactionByPRN(prn);
                    if (existing.Rows.Count > 0)
                    {
                        return Json(new { success = false, message = "Transaction already exists." });
                    }

                    // INSERT TRANSACTION
                    eSignModel.InsertEsignTransaction(prn, pdfPath, _LoggedInUser.SSOID, _LoggedInUser.UserId, _LoggedInUser.RoleId, "INITIATED", DocType, ReturnPath, docid, SocietyIDEnc, Remark, DocumentName, StatusId, IsAmendment, AmendmendAmount, ReceiptNumber, "", AuditBy, AssignedIds, IsFirm, Convert.ToInt32(DistrictId), FinancialYearId, FinancialYearName, AuditTillDate, IsForced, AuditByDepartmentorCA, DeleteResions, Convert.ToInt32(UserRoleDeptMappingId), PurposeOfLoanOther, "", "", DocIds);

                    SignedXmlResponse signedXmlResp;
                    signedXmlResp = await eSignModel.GenerateSignedXmlAsync(pdfPath, prn, responseUrl, false, DocType, Location, Designation);

                    // LOG XML GENERATION API
                    eSignModel.InsertEsignApiLog(prn, "XML_GENERATE", "PDFPath: " + pdfPath, JsonConvert.SerializeObject(signedXmlResp), signedXmlResp?.data?.responseCode, signedXmlResp?.data?.responseMsg, signedXmlResp?.status);

                    if (signedXmlResp != null && signedXmlResp.status == "SUCCESS")
                    {
                        // UPDATE STATUS
                        eSignModel.UpdateEsignStatus(prn, "XML_GENERATED", signedXmlResp.data.txnId);

                        return Json(new
                        {
                            success = true,
                            xml = Encoding.UTF8.GetString(Convert.FromBase64String(signedXmlResp.data.signedXMLData))
                        });
                    }
                    else
                    {
                        eSignModel.UpdateEsignStatus(prn, "FAILED", null);

                        return Json(new
                        {
                            success = false,
                            message = "XML Generation Failed"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }

                return Json(new { success = false, message = "Session out !!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public static string GeneratePRN()
        {
            return Guid.NewGuid().ToString("N").ToUpper();
        }

        [HttpPost]
        [ActionName("EsignAuditOrderForDepartment")]
        public string EsignAuditOrderForDepartment(string AuditerId = "", string AuditBy = "", string Ids = "", string Status = "", string IsFirm = "", string IsRequested = "", int DistrictId = 0, int FinancialYearId = 0, string FinancialYearName = "", string AuditTillDate = "", string IsForced = "0", string AuditByDepartmentorCA = "0")
        {
            string message = string.Empty;
            string succmsg = "Some Error";

            try
            {
                if (IsRequested == "0")
                    succmsg = AssignBydeprtNotRequestLatter(AuditerId, AuditBy, Ids, Status, IsFirm, FinancialYearId, FinancialYearName.Replace(" ", ""), AuditTillDate, IsForced, AuditByDepartmentorCA, IsRequested);
                if (IsRequested == "1")
                    succmsg = AuditAssignbydeprtLatter(AuditerId, AuditBy, Ids, Status, IsFirm, AuditTillDate, IsRequested);

            }
            catch (Exception ex)
            {
                succmsg = ex.Message;
            }

            return succmsg;
        }


        [HttpPost]
        public string AssignBydeprtNotRequestLatter(string AuditerId = "", string AuditBy = "", string Ids = "", string Status = "", string IsFirm = "", int FinancialYearId = 0, string FinancialYearName = "", string AuditTillDate = "", string IsForced = "False", string AuditByDepartmentorCA = "", string IsRequested = "")
        {
            AuditorController objAC = new AuditorController();
            AuditOrderEntity objAE = new AuditOrderEntity();
            try
            {
                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());

                var IdsArr = Ids.Split(',');
                string DistName = "";
                string DistNameHi = "";
                string Location = "";
                string fontSize = "font-size: 13px;";
                string saveIDs = "";
                DataTable Dt = null;

                if (_LoggedInUser.LevelId == 3)
                {
                    DistName = _LoggedInUser.DISTRICT_ENG;
                    DistNameHi = _LoggedInUser.DISTRICT_MANGAL;
                    Location = "जिले";
                }
                else if (_LoggedInUser.LevelId == 2)
                {
                    DistName = _LoggedInUser.DIVISION_ENG;
                    DistNameHi = _LoggedInUser.DIVISION_MANGAL;
                    Location = "खण्ड";
                }
                else if (_LoggedInUser.LevelId == 1)
                {
                    Location = "राज्य";
                }


                string OfficerName = "";
                string CurrentFYear = "";
                string SocietyName = "";
                string OrderNo = "";
                //if (AuditerId.ToString().Contains(","))
                //{
                //Dt = objSocietyModel.GetAuditorsForMultipalAssigment(AuditerId);
                //}
                //else
                int CurrentFYId = 0;
                int PrevFYId = 0;

                // {
                int AuditByAD = (AuditByDepartmentorCA == "true" ? 1 : 2);

                //if(Dt.Rows.Count>0)
                //{
                //    CurrentFYId = Convert.ToInt32(Dt.Rows[0]["CurrentFYId"].ToString());
                //}
                //  }

                DataTable FYDdata = objSocietyModel.GetCurrentAssessmentYear();

                if (FYDdata.Rows.Count > 0)
                {
                    CurrentFYId = Convert.ToInt32(FYDdata.Rows[0]["CurrentFY"].ToString());
                    PrevFYId = Convert.ToInt32(FYDdata.Rows[0]["prevFY"].ToString());
                }
                #region 
                Dt = objSocietyModel.GetAuditorAndSocietyDetailByIds(Ids, AuditerId, AuditByAD, FinancialYearId);
                objAE.FileName = "";
                // objAE.SocietyCount = Dt.Rows.Count;

                objAE.SocietyCount = Dt.AsEnumerable().GroupBy(row => row["SocietyName"]).Count();

                objAE.InActiveFlag = 0;
                OrderNo = objAC.SaveAuditOrder(objAE);
                objAE.OrderNo = OrderNo;
                #endregion
                if (OrderNo != "ERROR")
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:10px'>");
                    sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px; '>");
                    sb.Append("<tr><td colspan=2 style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                    sb.Append("<img style='width:100px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td colspan=2 style='text-align:center;" + fontSize + "'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                    if (_LoggedInUser.LevelId == 3)
                        sb.Append("<tr><td colspan=2 style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय- विशेष लेखा परीक्षक सहकारी समितियॉं, DistrictNameHi") + "</label></b></td></tr>");
                    else if (_LoggedInUser.LevelId == 2)
                        sb.Append("<tr><td colspan=2 style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय- रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड , DistrictNameHi") + "</label></b></td></tr>");
                    else if (_LoggedInUser.LevelId == 1)
                        sb.Append("<tr><td colspan=2 style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय- रजिस्ट्रार, सहकारी समितियॉं राजस्थान ,जयपुर") + "</label></b></td></tr>");

                    sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");

                    sb.Append("<tr><td align='left' style='margin-top:15px;'> क्रमांक फा." + OrderNo + " </td><td align='right'> दिनांक " + DateTime.Now.ToString("dd/MM/yyyy") + "</td></tr>");
                    sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                    sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><u><b> ऑडिट आवंटन आदेश </b></u></td></tr>");
                    if (IsForced == "True")
                    {
                        sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><b> (रा.स.सो.अधि.धारा 54(2) व नियम 73(8) के तहत) </b></td></tr>");
                        sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><b> (सोसाइटियों के वर्ग / सोसायटी हेतु) </b></td></tr>");
                    }
                    else if (FinancialYearId > PrevFYId)//Current
                    {
                        sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><b> (रा.स.सो.अधि.धारा 54(2) व नियम 73(7) के तहत) </b></td></tr>");
                    }
                    else if (FinancialYearId < CurrentFYId)//FOR BACK LOG
                    {
                        sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><b> (रा.स.सो.अधि.धारा 54(2) व नियम 73(8) के तहत) </b></td></tr>");
                    }

                    sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");

                    if (IsForced == "True")
                    {
                        sb.Append("<tr><td align='justify'  colspan=2 style='margin-top:15px;'> <p>&nbsp;&nbsp;&nbsp; राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 (2) तथा राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(8) अनुसार नियम 73(4) से 73(7) में अंतर्विष्ट किसी बात के होने पर भी, रजिस्ट्रार आदेश द्वारा, किसी विशिस्ट कालावधि के लिए किसी सोसायटी या सोसाइटियों के किसी वर्ग के लेखाओं की लेखापरीक्षा कराने के लिए लेखापरीक्षक (लेखापरीक्षको) या लेखा परीक्षा फर्म (फर्मो ) को नियुक्त कर सकेगा | अधोहस्ताक्षरकर्ता को यह समाधान हो गया है , कि निम्नलिखित सहकारी समितियों की लेखा परीक्षा हेतु उक्त प्रावधानों के अंतर्गत विभाग स्तर से लेखा परीक्षक नियुक्त किया जाना आवश्यक है ' । </p></td></tr>");
                        sb.Append("<tr><td align='center'   colspan=2 >&nbsp; </td></tr>");
                        if (_LoggedInUser.LevelId == 3)
                            sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName विशेष लेखा परीक्षक सहकारी समितियॉ DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) तथा राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73 (8) के तहत प्रदत्त शक्तियों का प्रयोग करते हुए निम्नलिखित सहकारी समितियों के वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + " InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + "  को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                        else if (_LoggedInUser.LevelId == 2)
                        {
                            if (AuditerId.ToString().Contains(","))
                            {
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(8) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए SocietyName की वर्ष FinYear की लेखा परीक्षा हेतु निम्न लिखित लेखा निरीक्षकों की नियुक्ति करता / करती हूँ | </p></td></tr>");
                            }
                            else
                            {
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) तथा राजस्थान सहकारी सोसाइटी नियम 2003 के नियम 73(8) के तहत प्रदत्त शक्तियों का प्रयोग करते हुए निम्न सहकारी समितियों के वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + " InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + "  को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                            }

                        }

                        else if (_LoggedInUser.LevelId == 1)
                            sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, संयुक्त मुख्य अंकेक्षक ( जनरल ) सहकारी समितियॉं, राजस्थान जयपुर, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) तथा राजस्थान सहकारी सोसाइटी नियम 2003 के नियम 73 (8) के तहत प्रदत्त शक्तियों का प्रयोग करते हुए निम्न सहकारी समितियों के वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + " InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + "  को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");

                        sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");

                        if (AuditerId.ToString().Contains(","))
                        {
                            sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; उपरोक्त सहकारी समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी |</p></td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; निम्न सहकारी समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54  एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी |</p></td></tr>");
                        }


                    }
                    else
                    {
                        if (FinancialYearId > PrevFYId)
                        {
                            sb.Append("<tr><td align='justify'  colspan=2 style='margin-top:15px;'> <p>&nbsp;&nbsp;&nbsp; राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(4) के अनुसार निम्नलिखित सहकारी समितियॉं नियत समय में लेखा परीक्षक की नियुक्ति कर सुचना प्रस्तुत करने में विफल रही है ।  </p></td></tr>");
                            sb.Append("<tr><td align='center'   colspan=2 >&nbsp; </td></tr>");
                            if (_LoggedInUser.LevelId == 3)
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, विशेष लेखा परीक्षक सहकारी समितियॉ DistrictNameHi , राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(7) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                            else if (_LoggedInUser.LevelId == 2)
                            {
                                if (AuditerId.ToString().Contains(","))
                                {
                                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(7) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए SocietyName की वर्ष FinYear की लेखा परीक्षा हेतु निम्न लिखित लेखा निरीक्षकों की नियुक्ति करता / करती हूँ | </p></td></tr>");
                                }
                                else
                                {
                                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(7) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                                }
                            }
                            else if (_LoggedInUser.LevelId == 1)
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, संयुक्त मुख्य अंकेक्षक ( जनरल ) सहकारी समितियॉं, राजस्थान जयपुर, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(7) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");

                            sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");

                            if (AuditerId.ToString().Contains(","))
                            {
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; उपरोक्त सहकारी समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी |</p></td></tr>");
                            }
                            else
                            {
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; निम्न सहकारी समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी |</p></td></tr>");
                            }


                        }
                        else  //FOR BACK LOG
                        {
                            sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(3) के अनुसार प्रत्येक सोसाइटी के लेखाओ की लेखा परीक्षा उस वित्तीय वर्ष की जिससे ऐसे लेखे सम्बंधित है ,समाप्ति के छ माह के भीतर-भीतर की जाएगी | परन्तु  निम्नलिखित सहकारी समितियों द्वारा वर्ष " + FinancialYearName + " तक की ऑडिट करवाकर ऑडिट रिपोर्ट प्रस्तुत नहीं की गयी है | जिससे स्पष्ट है कि निम्नलिखित सहकारी समितियां अधिनियम के उक्त प्रावधानों की पालना करने में विफल रही है इसलिए इन सहकारी समितियों की वर्ष " + FinancialYearName + " तक बकाया समस्त लेखा परीक्षा हेतु विभाग स्तर से लेखा परीक्षक की नियुक्ति किया जाना आवश्यक हो गया है | </p></td></tr>");
                            sb.Append("<tr><td align='center'   colspan=2 >&nbsp; </td></tr>");

                            if (_LoggedInUser.LevelId == 3)
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, विशेष लेखा परीक्षक सहकारी समितियॉ DistrictNameHi , राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(8) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष " + FinancialYearName + " की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                            else if (_LoggedInUser.LevelId == 2)
                            {
                                if (AuditerId.ToString().Contains(","))
                                {
                                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(8) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए SocietyName की वर्ष FinYear की लेखा परीक्षा हेतु निम्न लिखित लेखा निरीक्षकों की नियुक्ति करता / करती हूँ | </p></td></tr>");
                                }
                                else
                                {
                                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(8) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष " + FinancialYearName + " की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                                }
                            }

                            else if (_LoggedInUser.LevelId == 1)
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, संयुक्त मुख्य अंकेक्षक ( जनरल ) सहकारी समितियॉं, राजस्थान जयपुर, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(8) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष " + FinancialYearName + " की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");


                            sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                            if (AuditerId.ToString().Contains(","))
                            {
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; उपरोक्त सहकारी समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी तथा लेखापरीक्षकगण आवंटीत समिति में अविलम्ब प्रारंभ कर दिनांक " + AuditTillDate + " तक अंकेक्षण कार्य सम्पादित करेंगे |</p></td></tr>");
                            }
                            else
                            {
                                sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; निम्न सहकारी समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी तथा लेखापरीक्षकगण आवंटीत समिति में अविलम्ब प्रारंभ कर दिनांक " + AuditTillDate + " तक अंकेक्षण कार्य सम्पादित करेंगे |</p></td></tr>");
                            }


                        }
                    }

                    sb.Append("<tr><td align='center'  colspan=2 >");
                    sb.Append("<table style='width: 100%; margin-top:20px;' border='1' cellspacing='0' cellpadding='5'>");

                    if (AuditerId.ToString().Contains(","))
                    {
                        sb.Append("<tr><td align='center'style='width:70px;'> क्र. स. </td><td align='center'> लेखा निरीक्षक </td><td align='center'> सहकारी समिति  </td><td align='center'> कार्यालय </td></tr>");
                    }
                    else
                    {
                        sb.Append("<tr><td align='center'style='width:70px;'> क्र. स. </td><td align='center'> सहकारी समिति  </td><td align='center'> स्थान </td></tr>");
                    }


                    int i1 = 0;
                    foreach (DataRow dr in Dt.Rows)
                    {
                        i1++;
                        sb.Append("<tr><td  align='left'  > " + i1.ToString() + "</td>");

                        if (AuditerId.ToString().Contains(","))
                        {
                            sb.Append("<td  align='left'  > " + dr["OfficerNameEn"].ToString() + " </td>");
                            sb.Append("<td  align='left'  > " + dr["SocietyName"].ToString() + " (" + dr["RegistrationNo"].ToString() + ") </td>");
                            sb.Append("<td  align='left'  > " + dr["OfficeNameEn"].ToString() + " </td></tr>");
                        }
                        else
                        {
                            sb.Append("<td  align='left'  > " + dr["SocietyName"].ToString() + " (" + dr["RegistrationNo"].ToString() + ") </td>");
                            sb.Append("<td  align='left'  > " + (dr["SocietyPlaceEn"].ToString() != "" ? (dr["SocietyPlaceEn"].ToString() + " , ") : " ") + dr["DISTRICT_ENG"].ToString() + "</td></tr>");
                        }

                        if (AuditerId.ToString().Contains(","))
                        {
                            if (!OfficerName.Contains(dr["OfficerNameEn"].ToString()))
                                OfficerName = AuditBy == "1" ? dr["FirmName"].ToString() : OfficerName + ", " + dr["OfficerNameEn"].ToString();
                        }
                        else
                            OfficerName = AuditBy == "1" ? dr["FirmName"].ToString() : dr["OfficerNameEn"].ToString();
                        CurrentFYear = dr["CurrentFYear"].ToString() == "" ? FinancialYearName : dr["CurrentFYear"].ToString();
                        saveIDs = saveIDs + dr["Id"].ToString() + ",";
                        SocietyName = dr["SocietyName"].ToString().ToUpper();
                    }
                    if (OfficerName.Contains(", ") && OfficerName.StartsWith(","))
                    {
                        OfficerName = OfficerName.Substring(1);
                    }
                    sb.Append("</table></td></tr>");
                    sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                    sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                    sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                    sb.Append("</table ></div>");

                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    string fileName = Guid.NewGuid().ToString("N").ToUpper() + ".pdf";
                    string textVal = sb.ToString().Replace("<br>", "<br/>").Replace("DistrictNameHi", DistNameHi).Replace("SocietyName", SocietyName).Replace("DistrictNameHi", DistNameHi).Replace("FinYear", CurrentFYear).Replace("InspectorName", OfficerName).Replace("RegitrarName", _LoggedInUser.OfficerNameEn);
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(textVal));
                    string FilePath = "../Content/AuditReport/" + fileName;
                    if (COMMON_FUNCTION.GeneratePDF(sb1, Server.MapPath(FilePath)))
                    {
                        return JsonConvert.SerializeObject(new
                        {
                            Success = true,
                            DocPath = FilePath,
                            DocType = "Auditor Department",
                            DocName = fileName,
                            OrderNo = OrderNo,
                            IsRequested = IsRequested,
                            ReturnPath = Url.Action("AuditorAssignmentByDepartment", "Society"),
                            Location = "",
                            Designation = ""
                        });
                    }
                    else
                    {
                        objAE.InActiveFlag = 1;
                        objAC.SaveAuditOrder(objAE);
                        return JsonConvert.SerializeObject(new
                        {
                            Success = false,
                            DocPath = "",
                            DocType = "",
                            DocName = "",
                            OrderNo = "",
                            IsRequested = "",
                            ReturnPath = "",
                            Message = "PDF generation failed.",
                            Location = "",
                            Designation = ""
                        });
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(new
                    {
                        Success = false,
                        DocPath = "",
                        DocType = "",
                        DocName = "",
                        OrderNo = "",
                        IsRequested = "",
                        ReturnPath = "",
                        Message = "Order generation failed.",
                        Location = "",
                        Designation = ""
                    });
                }
            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Society", FunctionName = " saveAuditAssignbydeprtNotRequest(string AuditerId = \"\", string AuditBy = \"\", string Ids = \"\", string Status = \"\", string IsFirm = \"\", string Tran = \"\", int FinancialYearId = 0, string FinancialYearName = \"\", string AuditTillDate = \"\", string IsForced =" }, ex);
                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    OrderNo = "",
                    IsRequested = "",
                    ReturnPath = "",
                    Message = ex.Message,
                    Location = "",
                    Designation = ""
                });
            }

        }

        [HttpPost]
        public string AuditAssignbydeprtLatter(string AuditerId = "", string AuditBy = "", string Ids = "", string Status = "", string IsFirm = "", string AuditTillDate = "", string IsRequested = "")
        {
            string OrderNo = "";
            AuditorController objAC = new AuditorController();
            AuditOrderEntity objAE = new AuditOrderEntity();

            try
            {
                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                DataTable Dt = null;
                var IdsArr = Ids.Split(',');

                string DistName = "";
                string DistNameHi = "";

                string saveIDs = "";
                if (_LoggedInUser.LevelId == 3)
                {
                    DistName = _LoggedInUser.DISTRICT_ENG;
                    DistNameHi = _LoggedInUser.DISTRICT_MANGAL;
                }
                else if (_LoggedInUser.LevelId == 2)
                {
                    DistName = _LoggedInUser.DIVISION_ENG;
                    DistNameHi = _LoggedInUser.DIVISION_MANGAL;
                }
                else if (_LoggedInUser.LevelId == 1)
                {
                    DistName = "Rajasthan";
                }

                #region 

                objAE.FileName = "";
                objAE.SocietyCount = 1;
                objAE.InActiveFlag = 0;
                OrderNo = objAC.SaveAuditOrder(objAE);
                objAE.OrderNo = OrderNo;
                #endregion

                string OfficerName = "";
                string CurrentFYear = "";
                string SocietyName = "";

                string fontSize = "font-size: 13px;";

                Dt = objSocietyModel.GetAuditorAndSocietyDetailByIds(Ids, AuditerId);

                StringBuilder sb = new StringBuilder();
                sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:10px'>");
                sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px; '>");
                sb.Append("<tr><td colspan=2 style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                sb.Append("<img style='width:100px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td colspan=2 style='text-align:center;" + fontSize + "'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");

                if (_LoggedInUser.LevelId == 3)
                    sb.Append("<tr><td colspan=2 style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय- विशेष लेखा परीक्षक सहकारी समितियॉं, DistrictNameHi") + "</label></b></td></tr>");
                else if (_LoggedInUser.LevelId == 2)
                    sb.Append("<tr><td colspan=2 style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय- रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड , DistrictNameHi") + "</label></b></td></tr>");
                else if (_LoggedInUser.LevelId == 1)
                    sb.Append("<tr><td colspan=2 style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय- रजिस्ट्रार, सहकारी समितियॉं राजस्थान ,जयपुर") + "</label></b></td></tr>");

                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                sb.Append("<tr><td align='left' style='margin-top:15px;'> क्रमांक फा. " + OrderNo + " </td><td align='right'> दिनांक " + DateTime.Now.ToString("dd/MM/yyyy") + "</td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><u><b> ऑडिट आवंटन आदेश </b></u></td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 style='margin-top:15px;'><b> (रा.स.सो.अधि.धारा 54(2) व नियम 73(4) के तहत) </b></td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                sb.Append("<tr><td align='justify'  colspan=2 style='margin-top:15px;'> <p>&nbsp;&nbsp;&nbsp; राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(4) के अनुसार निम्नलिखित सहकारी समितियों द्वारा विभागीय निरीक्षक से वर्ष FinYear की लेखा परीक्षा करवाने की अभ्यर्थना प्रस्तुत की है | </p></td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");

                if (_LoggedInUser.LevelId == 3)
                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, विशेष लेखा परीक्षक सहकारी समितियॉ DistrictNameHi , राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(4) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                else if (_LoggedInUser.LevelId == 2)
                {
                    if (AuditerId.ToString().Contains(","))
                    {
                        sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(4) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए SocietyName की वर्ष FinYear की लेखा परीक्षा हेतु निम्न लिखित लेखा निरीक्षकों की नियुक्ति करता / करती हूँ | </p></td></tr>");
                    }
                    else
                    {
                        sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, रीजनल आडिट आफिसर सहकारी समितियॉं DistrictNameHi खण्ड DistrictNameHi, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(4) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");
                    }
                }
                else if (_LoggedInUser.LevelId == 1)
                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; अतः मैं RegitrarName, संयुक्त मुख्य अंकेक्षक ( जनरल ) सहकारी समितियॉं, राजस्थान जयपुर, राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54(2) एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 73(4) के तहत प्रदत्त शक्तियो का प्रयोग करते हुए निम्नलिखित सहकारी समितियों की वर्ष FinYear की लेखा परीक्षा हेतु " + (AuditBy == "1" ? "" : " श्री / सुश्री / श्रीमती ") + "InspectorName " + (AuditBy == "1" ? "" : " निरीक्षक (आडिट) ") + " को लेखा परीक्षक नियुक्त करता / करती हूँ | </p></td></tr>");

                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                if (AuditerId.ToString().Contains(","))
                {
                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; उपरोक्त समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी |</p></td></tr>");
                }
                else
                {
                    sb.Append("<tr><td align='justify'  colspan=2 ><p>&nbsp;&nbsp;&nbsp; निम्न समितियाँ राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72,73 की पूर्ण पालना सुनिश्चित करेगी |</p></td></tr>");
                }
                sb.Append("<tr><td align='center'  colspan=2 >");
                sb.Append("<table style='width: 100%; margin-top:20px;' border='1' cellspacing='0' cellpadding='5'>");
                if (AuditerId.ToString().Contains(","))
                {
                    sb.Append("<tr><td align='center'style='width:70px;'> क्र. स. </td><td align='center'> लेखा निरीक्षक </td><td align='center'> कार्यालय </td></tr>");
                }
                else
                {
                    sb.Append("<tr><td align='center'style='width:70px;'> क्र. स. </td><td align='center'> सहकारी समिति  </td><td align='center'> स्थान </td></tr>");/*<td align='center'>Officer/Firm</td>*/
                }
                int i1 = 0;
                foreach (DataRow dr in Dt.Rows)
                {
                    i1++;
                    sb.Append("<tr><td  align='left'  > " + i1.ToString() + "</td>");
                    if (AuditerId.ToString().Contains(","))
                    {
                        sb.Append("<td  align='left'  > " + dr["OfficerNameEn"].ToString() + " </td>");
                        sb.Append("<td  align='left'  > " + dr["OfficeNameEn"].ToString() + " </td></tr>");
                    }
                    else
                    {
                        sb.Append("<td  align='left'  > " + dr["SocietyName"].ToString() + " (" + dr["RegistrationNo"].ToString() + ") </td>");
                        sb.Append("<td  align='left'  > " + (dr["SocietyPlaceEn"].ToString() != "" ? (dr["SocietyPlaceEn"].ToString() + " , ") : " ") + dr["DISTRICT_ENG"].ToString() + "</td></tr>");
                    }
                    OfficerName = dr["OfficerNameEn"].ToString() != "" ? dr["OfficerNameEn"].ToString() : dr["FirmName"].ToString();
                    CurrentFYear = dr["CurrentFYear"].ToString();
                    saveIDs = saveIDs + dr["Id"].ToString() + ",";

                    SocietyName = dr["SocietyName"].ToString().ToUpper();


                }
                sb.Append("</table></td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                sb.Append("<tr><td align='center'  colspan=2 >&nbsp; </td></tr>");
                sb.Append("</table ></div>");


                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                string fileName = Guid.NewGuid().ToString("N").ToUpper() + ".pdf";
                string textVal = sb.ToString().Replace("<br>", "<br/>").Replace("DistrictNameHi", DistNameHi).Replace("SocietyName", SocietyName).Replace("FinYear", CurrentFYear).Replace("InspectorName", OfficerName).Replace("RegitrarName", _LoggedInUser.OfficerNameEn);
                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(textVal));

                string FilePath = "../Content/AuditReport/" + fileName;
                if (COMMON_FUNCTION.GeneratePDF(sb1, Server.MapPath(FilePath)))
                {
                    return JsonConvert.SerializeObject(new
                    {
                        Success = true,
                        DocPath = FilePath,
                        DocType = "Auditor Department",
                        DocName = fileName,
                        OrderNo = OrderNo,
                        IsRequested = IsRequested,
                        ReturnPath = Url.Action("AuditorAssignmentByDepartment", "Society")
                    });
                    //ResultMessage objRM = new ResultMessage();
                    //objRM = COMMON_FUNCTION.EsinghPDF(Tran, Server.MapPath(FilePath), DistName);

                    //if (objRM.data == "Success")
                    //{
                    //    if (saveIDs.Length > 0)
                    //        saveIDs = saveIDs.Substring(0, saveIDs.Length - 1);

                    //    ResultIntStr = objSocietyModel.saveAuditAssignbydeprt(AuditerId, AuditBy, saveIDs, Status, IsFirm, FilePath, _LoggedInUser.UserRoleDeptMappingId, OrderNo).ToString();
                    //    if (ResultIntStr != "-1")
                    //        ResultIntStr = "Success";
                    //    else
                    //    {
                    //        objAE.InActiveFlag = 1;
                    //        objAC.SaveAuditOrder(objAE);
                    //    }
                    //}
                }
                else
                {
                    return JsonConvert.SerializeObject(new
                    {
                        Success = false,
                        DocPath = "",
                        DocType = "",
                        DocName = "",
                        OrderNo = "",
                        IsRequested = "",
                        ReturnPath = "",
                        Message = "PDF generation failed."
                    });
                }
            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Society", FunctionName = " saveAuditAssignbydeprt(string AuditerId = \"\", string AuditBy = \"\", string Ids = \"\", string Status = \"\", string IsFirm = \"\", string Tran = \"\", string AuditTillDate = \"\")" }, ex);
                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    OrderNo = "",
                    IsRequested = "",
                    ReturnPath = "",
                    Message = ex.Message
                });
            }

        }
        #endregion

        #region  eSign Process 2.1 
        [HttpPost]
        [ActionName("SaveInspactorStatusDRForEsign")]
        public JsonResult SaveInspactorStatusDRForEsign(List<InspactorStatusDetails> InspactorStatusDetails, string Remarks, string Inspectorfile, string status)
        {
            int Result = 0;
            string PdfPath = "";
            string IDs = "";
            var responseModel = new PacsResponseModel();

            try
            {
                if (status == "0")
                {
                    return Json(new
                    {
                        Success = false,
                        Result = 0
                    }, JsonRequestBehavior.AllowGet);
                }


                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                if (status != "4")
                {
                    Result = objSocietyModel.SaveInspactorStatusDRForNewEsign(InspactorStatusDetails, Remarks, Inspectorfile, status, IDs);
                }

                if (status == "4")
                {
                    PdfPath = GSSGenerateSanctionLetter(InspactorStatusDetails);

                    if (!string.IsNullOrEmpty(PdfPath))
                    {
                        responseModel = JsonConvert.DeserializeObject<PacsResponseModel>(PdfPath);
                    }

                    if (responseModel != null && responseModel.Success == false)
                    {
                        string InspactorTargetsIds = string.IsNullOrEmpty(IDs) ? InspactorStatusDetails[0].InspactorTargetsId : IDs;
                        var Result2 = objSocietyModel.RevertLetter(InspactorTargetsIds);
                        return Json(new
                        {
                            Success = false,
                            Result = 0,
                            Message = responseModel.Message
                        }, JsonRequestBehavior.AllowGet);
                    }

                    Result = objSocietyModel.SaveInspactorStatusDRForNewEsign(InspactorStatusDetails, Remarks, responseModel?.DocName, status, IDs);
                    return Json(new
                    {
                        Success = true,
                        Result = Result,
                        DocPath = responseModel?.DocPath ?? "",
                        DocType = responseModel?.DocType ?? "",
                        DocName = responseModel?.DocName ?? "",
                        ReturnPath = responseModel?.ReturnPath ?? "",
                        Location = "",
                        Designation = ""
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    Success = Result > 0,
                    Result = Result
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "SocietyController",
                    FunctionName = "SaveInspactorStatusDRForEsign"
                }, ex);
                return Json(new
                {
                    Success = false,
                    Result = 0,
                    Message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public string GSSGenerateSanctionLetter(List<InspactorStatusDetails> InspactorStatusDetails = null)
        {
            string PdfPath = string.Empty;
            string InspactorTargetsId = "";
            try
            {
                if (InspactorStatusDetails != null && InspactorStatusDetails.Any())
                {
                    InspactorTargetsId = string.Join(",", InspactorStatusDetails.Select(x => x.InspactorTargetsId));
                }
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {
                    GopalCreditCardDBModel _Modal = new GopalCreditCardDBModel();
                    var Details = new DataPagingModel { PageNumber = 10, PageSize = 100000 };
                    SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                    DataTable Dt = objSocietyModel.GetInspactorTargetsDetails(Details, "0", "13", InspactorTargetsId, 0, -1);

                    bool isMahila = Convert.ToBoolean(Dt.Rows[0]["IsMahilaMpacs"]);
                    bool isTsp = Convert.ToBoolean(Dt.Rows[0]["IsTspArea"]);
                    int isAreaType = Convert.ToInt16(Dt.Rows[0]["AreaType"]);

                    var templatename = "";
                    if (isMahila)
                    {
                        templatename = System.IO.Path.Combine(Server.MapPath("~/Template"), "gss_target_mahila_sanction_letter.html");
                    }
                    else if (isTsp)
                    {
                        templatename = System.IO.Path.Combine(Server.MapPath("~/Template"), "gss_target_sanction_letter.html");
                    }
                    else
                    {
                        if (isAreaType == 3)
                        {
                            templatename = System.IO.Path.Combine(Server.MapPath("~/Template"), "gss_target_cad_sanction_letter.html");
                        }
                        else
                        {
                            templatename = System.IO.Path.Combine(Server.MapPath("~/Template"), "gss_target_sanction_letter.html");
                        }

                    }

                    StringBuilder strbld = new StringBuilder();
                    var file = System.IO.Path.Combine(Server.MapPath("~/Template"), templatename);
                    StringBuilder rowBuilder = new StringBuilder();
                    int i = 0;
                    string DR2JrPlanForwardedDate = "";
                    strbld.Append(System.IO.File.ReadAllText(file));
                    {

                        foreach (DataRow row in Dt.Rows)
                        {
                            if (isAreaType == 3 && !string.IsNullOrWhiteSpace(row["ProposedNameMPACS"].ToString()) && !string.IsNullOrWhiteSpace(row["GP_ENG"].ToString()) && !string.IsNullOrWhiteSpace(row["BLOCK_ENG"].ToString()))
                            {
                                rowBuilder.AppendFormat("<tr>" +
                             "<td>{0}</td>" +
                             "<td>{1}</td>" +
                             "<td>{2}</td>" +
                             "<td>{3}</td>" +
                             "</tr>",
                             i + 1,
                             row["ProposedNameMPACS"].ToString(),
                             //row["GP_ENG"]?.ToString() ?? "",
                             row["GP_ENG"].ToString(),
                             row["BLOCK_ENG"].ToString());
                            }
                            else
                            {
                                rowBuilder.AppendFormat("<tr>" +
                           "<td>{0}</td>" +
                           "<td>{1}</td>" +
                           "<td>{2}</td>" +
                           "<td>{3}</td>" +
                           "</tr>",
                           i + 1,
                           //isAreaType == 3 ? row["ProposedNameMPACS"]?.ToString() ?? "" : row["GP_ENG"]?.ToString() ?? "",
                           row["GP_ENG"]?.ToString() ?? "",
                           row["GP_ENG"]?.ToString() ?? "",
                           row["BLOCK_ENG"]?.ToString() ?? "");
                            }

                            DR2JrPlanForwardedDate += Convert.ToDateTime(row["DR2JrPlanForwardedDate"]).ToString("dd-MM-yyyy") + ", ";
                            i++;

                            DR2JrPlanForwardedDate = DR2JrPlanForwardedDate.TrimEnd(',', ' ');
                            if (i > 0)
                            {
                                strbld.Replace("##ActionFlowId##", row["CurrentAppFlowId"]?.ToString() ?? "");
                                strbld.Replace("##SenctionNo##", row["SenctionNo"]?.ToString() ?? "");
                                strbld.Replace("##DistrictName##", row["DISTRICT_MANGAL"]?.ToString() ?? "");
                                strbld.Replace("##DivisionName##", row["DIVISION_MANGAL"]?.ToString() ?? "");
                                strbld.Replace("##BankName##", row["BankName"]?.ToString() ?? "");
                                strbld.Replace("##SanctionNo##", row["SenctionNo"]?.ToString() ?? "");
                                strbld.Replace("##SanctionDate##", DateTime.Now.ToString("dd-MM-yyyy"));
                                strbld.Replace("##CommitteeApprovalDate##", row["DateOfApproval"] != DBNull.Value
                                   ? Convert.ToDateTime(row["DateOfApproval"]).ToString("dd-MM-yyyy")
                                   : "");
                                strbld.Replace("##DR2JrPlanForwardedDate##", DR2JrPlanForwardedDate);
                            }
                            i++;
                        }
                    }
                    strbld.Replace("##SocietyData##", rowBuilder.ToString());


                    var gccRegNo = Server.MapPath("~/Content/InspactorDocument/");
                    string devFontSize = "17px";

                    if (!System.IO.Directory.Exists(gccRegNo))
                    {
                        System.IO.Directory.CreateDirectory(gccRegNo);
                    }
                    string fileName = "GSSTargetSanction_" + Guid.NewGuid().ToString("N").ToUpper() + ".pdf";
                    var filePath = gccRegNo + "/" + fileName;

                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(strbld.ToString());
                    string QRCoder = COMMON_FUNCTION.GenerateQRCode("https://rajsahakarapp.rajasthan.gov.in/Content/InspactorDocument/" + fileName);
                    sb1.Replace("#QRCoder#", QRCoder);
                    GopalCreditCardController gopalCreditCardController = new GopalCreditCardController();
                    PdfPath = gopalCreditCardController.CommonPdfGenerate(sb1, filePath);
                    if (PdfPath != "PdfPath")
                    {
                        PdfPath = "~/Content/InspactorDocument/";
                        return JsonConvert.SerializeObject(new
                        {
                            Success = true,
                            DocPath = PdfPath + fileName,
                            DocType = "MPacs",
                            DocName = fileName,
                            ReturnPath = Url.Action("InspactorTargetsAction", "Society"),
                            Location = "",
                            Designation = ""
                        });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new
                        {
                            Success = false,
                            Message = "PDF generation failed.",
                            DocPath = "",
                            DocType = "",
                            DocName = "",
                            ReturnPath = "",
                            Location = "",
                            Designation = ""
                        });
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(new
                    {
                        Success = false,
                        Message = "Session expired.",
                        DocPath = "",
                        DocType = "",
                        DocName = "",
                        ReturnPath = "",
                        Location = "",
                        Designation = ""
                    });
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Society", FunctionName = "GSSGenerateSanctionLetter(string fileName, List<InspactorStatusDetails> InspactorStatusDetails = null)" }, ex);
                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    Message = ex.Message,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    Location = "",
                    Designation = ""
                });
            }
        }


        [HttpPost]
        public async Task<JsonResult> MPacStartEsign(string DocType, string docid, string SocietyIDEnc = "", string DocumentPath = "", string DocumentName = "", string ReturnPath = "", string Remark = "", int? StatusId = null, bool IsAmendment = false, string AmendmendAmount = "", string ReceiptNumber = "", List<InspactorStatusDetails> Details = null, string Location = "", string Designation = "")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            EsignModel eSignModel = new EsignModel(new DBLogger());
            var detailsJson = JsonConvert.SerializeObject(Details);
            string UAT_BaseUrl = "http://localhost:27466/eSign/EspResponse";
            string Prod_BaseUrl = "https://rajsahakarapp.rajasthan.gov.in/eSign/EspResponse";

            try
            {
                if (_LoggedInUser != null)
                {
                    string responseUrl = ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")) ? UAT_BaseUrl : Prod_BaseUrl;
                    string pdfPath = "";
                    pdfPath = Server.MapPath(DocumentPath);
                    string prn = GeneratePRN();
                    string baseUrl = ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")) ? "http://localhost:27466/" : "https://rajsahakarapp.rajasthan.gov.in/";
                    ReturnPath = baseUrl + ReturnPath;
                    var existing = eSignModel.GetTransactionByPRN(prn);
                    if (existing.Rows.Count > 0)
                    {
                        return Json(new { success = false, message = "Transaction already exists." });
                    }

                    // INSERT TRANSACTION
                    eSignModel.InsertEsignTransaction(prn, pdfPath, _LoggedInUser.SSOID, _LoggedInUser.UserId, _LoggedInUser.RoleId, "INITIATED", DocType, ReturnPath, docid, SocietyIDEnc, Remark, DocumentName, StatusId, IsAmendment, AmendmendAmount, ReceiptNumber, detailsJson, "0", "0", "0", _LoggedInUser.DistrictId, "0", "0", "0", "0", "0", "0", _LoggedInUser.UserRoleDeptMappingId);

                    SignedXmlResponse signedXmlResp;
                    signedXmlResp = await eSignModel.GenerateSignedXmlAsync(pdfPath, prn, responseUrl, false, DocType, Location, Designation);

                    // LOG XML GENERATION API
                    eSignModel.InsertEsignApiLog(prn, "XML_GENERATE", "PDFPath: " + pdfPath, JsonConvert.SerializeObject(signedXmlResp), signedXmlResp?.data?.responseCode, signedXmlResp?.data?.responseMsg, signedXmlResp?.status);

                    if (signedXmlResp != null && signedXmlResp.status == "SUCCESS")
                    {
                        // UPDATE STATUS
                        eSignModel.UpdateEsignStatus(prn, "XML_GENERATED", signedXmlResp.data.txnId);

                        return Json(new
                        {
                            success = true,
                            xml = Encoding.UTF8.GetString(Convert.FromBase64String(signedXmlResp.data.signedXMLData))
                        });
                    }
                    else
                    {
                        eSignModel.UpdateEsignStatus(prn, "FAILED", null);

                        return Json(new
                        {
                            success = false,
                            message = "XML Generation Failed"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }

                return Json(new { success = false, message = "Session out !!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion


        [HttpGet]
        public string GetElectionSocietyCategoryList()
        {
            SocietyModel obj = new SocietyModel();

            return JsonConvert.SerializeObject(obj.GetElectionSocietyCategoryList());
        }
        // KPI For Pacs
        #region 
        [HttpGet]
        public ActionResult KPIForPacs()
        {
            return View();
        }


        [HttpPost]
        public JsonResult SaveKPIForPacs(KPIForPacsSaveModel model)
        {
            SocietyModel objNew = new SocietyModel(new DBLogger());

            DataTable DT = objNew.SaveKPIForPacs(model);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = model }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult KPIForPacsReport()
        {
            return View();
        }

        // GetKPIForPacs

        [HttpGet]
        public ActionResult GetKPIForPacsReport(int scoietyid)
        {
            SocietyModel objNew = new SocietyModel(new DBLogger());

            DataTable DT = objNew.GetKPIForPacsReport(scoietyid);
            return Json(
           COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), JsonRequestBehavior.AllowGet);
        }


        #endregion




        [HttpPost]
        public JsonResult UploadRecommendationDocument(HttpPostedFileBase RecommendationDocument)
        {
            try
            {
                if (RecommendationDocument == null)
                {
                    return Json(new { success = false, message = "File not received" });
                }

                string originalFileName = Path.GetFileNameWithoutExtension(RecommendationDocument.FileName);
                string extension = Path.GetExtension(RecommendationDocument.FileName).ToLower();

                // Allow only PDF
                if (extension != ".pdf")
                {
                    return Json(new { success = false, message = "Only PDF files are allowed." });
                }

                // Validate filename (letters, numbers, underscore and hyphen only)
                if (!System.Text.RegularExpressions.Regex.IsMatch(originalFileName, @"^[a-zA-Z0-9_-]+$"))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Invalid File (File name should not contain special characters)"
                    });
                }

                string folder = Server.MapPath("~/Content/Election/RecommendationDocument/");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(folder, fileName);

                RecommendationDocument.SaveAs(filePath);

                return Json(new
                {
                    success = true,
                    fileName = fileName
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }


        [HttpGet]
        [ActionName("GetElectionVacancyList")]
        public string GetElectionVacancyList(int ElectionId)
        {
            DataTable DT = new SocietyModel().GetElectionVacancyList(ElectionId);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }


        [HttpGet]
        [ActionName("GetSAKPIDetails")]
        public string GetSAKPIDetails()
        {
            DataTable dt = new SocietyModel().GetSAKPIDetails();
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpGet]
        [ActionName("GetRAKPIDetails")]
        public string GetRAKPIDetails()
        {
            DataTable dt = new SocietyModel().GetRAKPIDetails();
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpGet]
        [ActionName("GetUserDetailsBySSO")]
        public string GetUserDetailsBySSO()
        {
            DataTable dt = new SocietyModel().GetUserDetailsBySSO();
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpPost]
        public int SaveKPIResult(List<KPIResultModel> model)
        {
            int result = new SocietyModel().SaveKPIResult(model);
            return result;
        }


        // KPI For Bhandar
        #region 
        [HttpGet]
        public ActionResult KPIForBhandars()
        {
            return View();
        }
        #endregion

        [HttpGet]
        [ActionName("GetBhandarKPIDetails")]
        public string GetBhandarKPIDetails()
        {
            DataTable dt = new SocietyModel().GetBhandarKPIDetails();
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }


        #region NoticeIssue Esign Process on 10-08-2026

        [HttpPost]
        public JsonResult NoticeIssueLetter(List<SocietyDetailsforAction> Details)
        {
            var responseModel = new PacsResponseModel();
            var resultData = GenerateNoticeLetter(Details);
            if (!string.IsNullOrWhiteSpace(resultData))
            {
                responseModel = JsonConvert.DeserializeObject<PacsResponseModel>(resultData);
            }

            if (responseModel == null)
            {
                return Json(new
                {
                    Success = false,
                    Result = 0,
                    Message = "No response received."
                }, JsonRequestBehavior.AllowGet);
            }

            if (!responseModel.Success)
            {
                return Json(new
                {
                    Success = false,
                    Result = 0,
                    Message = responseModel.Message
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                Success = true,
                Result = 1,
                DocPath = responseModel.DocPath ?? "",
                DocType = responseModel.DocType ?? "",
                DocName = responseModel.DocName ?? "",
                ReturnPath = responseModel.ReturnPath ?? "",
                Location = responseModel.Location ?? "",
                Designation = responseModel.Designation ?? "",
            }, JsonRequestBehavior.AllowGet);
        }

        public string GenerateNoticeLetter(List<SocietyDetailsforAction> Details)
        {
            try
            {
                if (Details == null || Details.Count == 0)
                {
                    return JsonConvert.SerializeObject(new
                    {
                        Success = false,
                        DocPath = "",
                        DocType = "",
                        DocName = "",
                        ReturnPath = "",
                        Location = "",
                        Designation = "",
                        Message = "No data found"
                    });
                }

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                int DistrictId = _LoggedInUser.DistrictId;

                string filepath = Server.MapPath("~/Content/CCSLetter");
                string fontSize = "font-size: 15px;";

                if (!System.IO.Directory.Exists(filepath))
                {
                    System.IO.Directory.CreateDirectory(filepath);
                }

                // Only one complaint is selected
                SocietyDetailsforAction model = Details[0];

                StringBuilder sb = new StringBuilder();
                StringBuilder sb1 = new StringBuilder();

                string FileNameComplainant = "cc_sc_" + model.complainerId;

                sb.Append("<div style='padding-left:50px;padding-right:50px;padding-top:5px'>");

                sb.Append("<html><body>");
                sb.Append("<table border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");

                // Logo
                sb.Append("<tr>");
                sb.Append("<td colspan='5' style='padding:15px 15px 0px 15px; text-align:center; height:109px'>");
                sb.Append("<img style='width:90px;' src='" +
                          Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                sb.Append("</td>");
                sb.Append("</tr>");

                // Header
                if (!string.IsNullOrWhiteSpace(model.HeaderImage))
                {
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:center;'>");
                    sb.Append("<img src='" + Server.MapPath("~/Content/Images/" + model.HeaderImage) +
                              "' />");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                }
                else
                {
                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:center;font-size:32px;'>");
                    sb.Append("<b>");
                    sb.Append(UnicodeToKrutidev.GetKrutidev(
                        "कार्यालय उप रजिस्ट्रार, सहकारी समितियां, " + model.District));
                    sb.Append("</b>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td colspan='5' style='text-align:center;font-size:14px;'>");
                    sb.Append("<b>" + model.OfficeAddress + "</b>");
                    sb.Append("<br><br>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                }

                // Notice heading
                sb.Append("<tr>");
                sb.Append("<td colspan='5' style='padding:10px 5px 10px 5px; text-align:center; height:30px;'>");
                sb.Append("<br><br>");
                sb.Append("<b>");
                sb.Append("<label style='font-size:20px;'>");
                sb.Append(UnicodeToKrutidev.GetKrutidev("नोटिस बाबत उपस्थिति परिवाद"));
                sb.Append("<br><br><br><br>");
                sb.Append("</label>");
                sb.Append("</b>");
                sb.Append("</td>");
                sb.Append("</tr>");

                // Complainant details
                sb.Append("<tr>");

                sb.Append("<td colspan='3' style='text-align:left;'>");
                sb.Append("वास्ते :-<br>");
                sb.Append(model.ComplainerName + ",<br>");
                sb.Append(model.ComplainerFatherName + ",<br>");
                sb.Append(model.Address + "<br/>");
                sb.Append("Mobile Mo- " + model.MobileNo);

                if (!string.IsNullOrWhiteSpace(model.Email))
                {
                    sb.Append("<br/> Email- " + model.Email);
                }

                sb.Append("</td>");

                sb.Append("<td colspan='2' style='text-align:right;'>");
                sb.Append("परिवाद संख्या :" + model.ReceiptNo);
                sb.Append("<br><br><br><br><br><br><br><br>");
                sb.Append("</td>");

                sb.Append("</tr>");

                // Paragraph 1
                sb.Append("<tr>");
                sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'>");
                sb.Append("<br><br>");
                sb.Append("<p style='text-indent:50px;'>");

                sb.Append("आप द्वारा The Banning of Unregulated Deposit Scheme Act-2019 के तहत ");
                sb.Append("शिकायत / परिवाद दोषी सोसाइटी ");
                sb.Append(model.SocietyName);
                sb.Append(" के विरुद्ध प्रस्तुत किया है, जिस पर सोसाइटी के विरुद्ध उचित विधिक ");
                sb.Append("कार्यवाही के अनुक्रम में अधिनियम की धारा - 4 सपठित धारा - 22 के तहत ");
                sb.Append("दंडनीय अपराध हेतु सम्बन्धित Designated court में परिवाद / इस्तागा प्रस्तुत किया जाना है");

                sb.Append("</p>");
                sb.Append("</td>");
                sb.Append("</tr>");

                // Paragraph 2
                sb.Append("<tr>");
                sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'>");
                sb.Append("<p style='text-indent:50px;'>");

                sb.Append("अतः इस नोटिस के माध्यम से यह सूचित किया जाता है कि आप अपने परिवाद के समर्थन ");
                sb.Append("मे जिन दस्तावेजो पर निर्भर रहते हो यथा एफडीआर / पासबुक / निवेश का दस्तावेज / ");
                sb.Append("सोसाइटी में मेंबरशिप का प्रमाणपत्र / आधार कार्ड की स्वप्रमाणित प्रतिलिपियों ");
                sb.Append("मय निवेश की दो पासपोर्ट साइज फोटो के दिनांक ");

                sb.Append(model.Presentdate);

                sb.Append(" को इस कार्यालय में उपस्थित होवें |");

                sb.Append("</p>");
                sb.Append("</td>");
                sb.Append("</tr>");

                // Paragraph 3
                sb.Append("<tr>");
                sb.Append("<td colspan='5' style='text-align:left;padding-left:100px;'>");
                sb.Append("<p style='text-indent:50px;'>");

                sb.Append("आप निर्धारित तिथि को उपस्थित नहीं होने की सूरत में यह अवधारणा की जावेगी, ");
                sb.Append("की आप परिवाद पर कोई कार्यवाही नहीं चाहते है | यह नोटिस आज दिनांक ");

                sb.Append(DateTime.Now.ToString("dd/MM/yyyy"));

                sb.Append(" को मेरे हस्ताक्षर एवं मोहर से जारी किया गया है |");

                sb.Append("</p>");
                sb.Append("</td>");
                sb.Append("</tr>");

                // Digital signature note
                sb.Append("<tr>");
                sb.Append("<td colspan='5' style='text-align:left; width:100%; height:50px; ");
                sb.Append("font-size:10px; border-top:1px solid #ccc; padding:3px;'>");

                sb.Append("<b>Note:</b> This is a digitally signed certificate and does not require any physical signature.");

                sb.Append("</td>");
                sb.Append("</tr>");

                sb.Append("</table>");
                sb.Append("</body></html>");
                sb.Append("</div>");

                // Save HTML
                model.Notice = sb.ToString().Replace("<br>", "<br/>");

                // Convert Hindi text
                sb1.Append(
                    UnicodeToKrutidev.FindAndReplaceKrutidev(
                        sb.ToString().Replace("<br>", "<br/>"),
                        true,
                        "20px"
                    )
                );

                // Generate PDF
                string pdfPath = filepath + "/" + FileNameComplainant + ".pdf";

                if (COMMON_FUNCTION.GeneratePDF(sb1, pdfPath, ""))
                {
                    model.complainant_letter = FileNameComplainant + ".pdf";

                    return JsonConvert.SerializeObject(new
                    {
                        Success = true,
                        DocPath = pdfPath,
                        DocType = "CCS NoticeIssue",
                        DocName = FileNameComplainant + ".pdf",
                        DisposeFile = pdfPath,
                        ReturnPath = Url.Action("CCSComplaint", "Society"),
                        Location = "",
                        Designation = ""
                    });
                }

                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    Location = "",
                    Designation = "",
                    Message = "PDF generation failed"
                });
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();

                objDBLogger.ErrorHandle(
                    new ErrorLogEntity()
                    {
                        PageName = "Cooperative.Controller.SocietyControler",
                        FunctionName = "NoticeIssueLetter"
                    },
                    ex
                );

                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    Location = "",
                    Designation = "",
                    Message = ex.Message
                });
            }
        }


        #endregion


        [HttpGet]
        [ActionName("GetKPIDetails")]
        public string GetSAKPIDetails(int officeId, int month, string kpiFor)
        {
            DataTable dt = new SocietyModel().GetKPIAllDetails(officeId, month, kpiFor);
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }


        [HttpPost]
        [ActionName("AGMLetter")]
        public JsonResult AGMLetter(List<InspactorStatusDetails> InspactorStatusDetails, string Remarks, string status, string agmTime, string agmDate, string agmPlace)
        {
            string fy = new SocietyModel().GetKPICurrentFinancialYear();

            var responseModel = new PacsResponseModel();
            try
            {
                var targetId = InspactorStatusDetails.FirstOrDefault()?.InspactorTargetsId;
                var resultData = GenerateAGMNoticeLetter(agmTime, agmDate, agmPlace, targetId);
                if (!string.IsNullOrWhiteSpace(resultData))
                {
                    responseModel = JsonConvert.DeserializeObject<PacsResponseModel>(resultData);
                }

                if (responseModel == null)
                {
                    return Json(new
                    {
                        Success = false,
                        Result = 0,
                        Message = "No response received."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (!responseModel.Success)
                {
                    return Json(new
                    {
                        Success = false,
                        Result = 0,
                        Message = responseModel.Message
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    Success = true,
                    DocPath = responseModel.DocPath ?? "",
                    DocType = responseModel.DocType ?? "",
                    DocName = responseModel.DocName ?? "",
                    ReturnPath = responseModel.ReturnPath ?? "",
                    Location = responseModel.Location ?? "",
                    Designation = responseModel.Designation ?? "",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SocietyController", FunctionName = "AGMLatter(List<InspactorStatusDetails> InspactorStatusDetails, string Remarks, string status, string agmTime, string agmDate, string agmPlace)" }, ex);
                return Json(new
                {
                    Success = false,
                    Result = 0,
                    Message = "An error occurred while generating the attendance document.",
                    Error = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [ActionName("GenerateAGMNoticeLetter")]
        public string GenerateAGMNoticeLetter(string AGMTime = "", string AGMDate = "", string AGMPlace = "", string targetId = "")
        {
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {
                    string blockName = "";
                    string gpName = "";
                    string districtName = "";
                    string NewGSSName = "";
                    string PrecentNameGSS = "";
                    string formattedSanctionDate = "";
                    string CCBBranchName = "";
                    string RegNo = "";
                    string sanctionYear = "";

                    DataTable Dt = null;
                    SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                    Dt = objSocietyModel.GetInspectorDetailsForCertificate(targetId);
                    if (Dt != null && Dt.Rows.Count > 0)
                    {
                        blockName = Dt.Rows[0]["BLOCK_ENG"]?.ToString();
                        gpName = Dt.Rows[0]["GP_ENG"]?.ToString();
                        districtName = Dt.Rows[0]["DISTRICT_ENG"]?.ToString();
                        NewGSSName = Dt.Rows[0]["NewGSSName"]?.ToString();
                        PrecentNameGSS = Dt.Rows[0]["PrecentNameGSS"]?.ToString();
                        CCBBranchName = Dt.Rows[0]["BranchNameEn"]?.ToString();

                        if (Dt.Rows[0]["SanctionDate"] != DBNull.Value)
                        {
                            DateTime dt = (DateTime)Dt.Rows[0]["SanctionDate"];
                            formattedSanctionDate = dt.ToString("dd/MM/yyyy");
                            sanctionYear = dt.Year.ToString();
                        }

                        RegNo = $"AGM/{districtName}/{blockName}/{sanctionYear}/{Convert.ToInt32(Dt.Rows[0]["Id"])}";

                    }

                    StringBuilder strbld = new StringBuilder();
                    var file = Path.Combine(Server.MapPath("~/Template"), "pacs_generate_agm_notice.html");
                    strbld.Append(System.IO.File.ReadAllText(file));

                    strbld.Replace("#AGMTime#", AGMTime)
                          .Replace("#AGMDate#", AGMDate)
                          .Replace("#AGMPlace#", AGMPlace)
                          .Replace("#BlockName#", blockName)
                          .Replace("#GPName#", gpName)
                          .Replace("#DistrictName#", districtName)
                          .Replace("#NewGSSName#", NewGSSName)
                          .Replace("#SanctionDate#", formattedSanctionDate)
                          .Replace("#PrecentNameGSS#", PrecentNameGSS)
                          .Replace("#CCBBranchName#", CCBBranchName)
                          .Replace("#CurrentDate#", DateTime.Now.ToString("dd/MM/yyyy"))
                          .Replace("#RegNo#", RegNo);

                    var agmNoticePath = Server.MapPath("~/Content/AGMNotice/GSSProfiling/");
                    if (!Directory.Exists(agmNoticePath))
                    {
                        Directory.CreateDirectory(agmNoticePath);
                    }

                    var fileName = "AGMNotice_" + Guid.NewGuid().ToString("N").ToUpper() + ".pdf";
                    var filePath = Path.Combine(agmNoticePath, fileName);

                    StringBuilder sb = new StringBuilder();
                    sb.Append(strbld.ToString());
                    CommonPdfGenerate(sb, filePath);
                    return JsonConvert.SerializeObject(new
                    {
                        Success = true,
                        DocPath = filePath,
                        DocType = "AGM Certificate",
                        DocName = fileName,
                        ReturnPath = Url.Action("InspactorGSS", "society"),
                        Location = districtName,
                        Designation = "Inspactor"
                    });
                }
                else
                {
                    return JsonConvert.SerializeObject(new
                    {
                        Success = false,
                        DocPath = "",
                        DocType = "",
                        DocName = "",
                        ReturnPath = "",
                        Location = "",
                        Designation = "",
                        Message = "Fail"
                    });
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "SocietyController", FunctionName = "GenerateAGMNoticeLatter catch" }, ex);
                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    Location = "",
                    Designation = "",
                    Message = "Error in Generate files"
                });
            }
        }

        [HttpGet]
        [ActionName("GetFertilizerCategory")]
        public string GetFertilizerSubCategory()
        {
            DataTable dt = new SocietyModel().GetFertilizerCategory();
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpGet]
        public JsonResult GetFertilizerDetail(string actionType = "", int DemandRequestId = 0)
        {
            var responce = new APIResponce();
            SocietyModel model = new SocietyModel(new DBLogger());
            var ds = model.GetFertilizerDetailById(actionType, DemandRequestId);
            if (ds != null)
            {
                responce.Data = new
                {
                    FertilizerDetail = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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

    }
}
public class objecedSocietyEntity
{
    public string Remark { get; set; }
    public int SocietyId { get; set; }
    public string AmendmentIds { get; set; }
    public object ObjectionFile { get; set; }
}

public class AGMMultiInspectorModel
{
    public int Id { get; set; }
    public int InspactorStatusId { get; set; }
    public int Inspector_ID { get; set; }
    public string InspactorName { get; set; }

    public int InspactorTargetsId { get; set; }
    public string PrecentNameGSS { get; set; }
    public string NewGSSName { get; set; }

    public int IsPrimary { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }

    public int DistrictId { get; set; }
    public string DistrictName { get; set; }

    public int DivisionId { get; set; }
    public string DivisionName { get; set; }

    public int LevelId { get; set; }
    public string LevelName { get; set; }

    public int OfficeId { get; set; }
    public string OfficeName { get; set; }
}


public class PacsResponseModel
{
    public bool Success { get; set; }
    public string DocPath { get; set; }
    public string DocType { get; set; }
    public string DocName { get; set; }
    public string ReturnPath { get; set; }
    public string Message { get; set; }
    public string Location { get; set; }
    public string Designation { get; set; }
    public string Type { get; set; }
}

public class KPIResultModel
{
    public string FY { get; set; }
    public int Month { get; set; }
    public string KPIFor { get; set; }
    public int OfficeId { get; set; }
    public int ResionId { get; set; }
    public int KPIIndicatorsId { get; set; }
    public int CCBId { get; set; }
    public int KPI_MarksId { get; set; }
    public decimal KPIIndicatorsTarget { get; set; }
    public decimal KPIIndicatorsAchievement { get; set; }
    public decimal ProgressPercentage { get; set; }
    public decimal WeightedMarks { get; set; }
    public decimal MarksObtained { get; set; }
    public int PLDBId { get; set; }
    public int CreatedBy { get; set; }
    public int? CCBDistrictId { get; set; }
    public string CCBName { get; set; }
    public string CCBDistrictName { get; set; }
    public string CCBBranchName { get; set; }
    public string PacsManagerName { get; set; }
    public string PacsName { get; set; }
    public string PacsRegistrationNo { get; set; }
    public decimal? KCCLoanDisbursedAmt { get; set; }
    public decimal? AgricultureTrunOver { get; set; }
    public int? MembershipPrevious { get; set; }
    public int? MembershipCurrent { get; set; }
    public int? ShareCapitalPrevious { get; set; }
    public int? ShareCapitalCurrent { get; set; }
    public bool? selectOptionAnyOther1 { get; set; }
    public bool? selectOptionAnyOther2 { get; set; }


}