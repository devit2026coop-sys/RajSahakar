using CoopDAL;
using Cooperative.DBConnector;
using Cooperative.Filters;
using Cooperative.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Cooperative.Controllers
{
    [Error]
    [LogActionFilter]
    [CustomAuthentication]
    public class AuditorController : BaseController
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        // GET: Auditor
        #region Auditor
        [HttpGet]
        public ActionResult Auditor()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Auditor, (Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }


        //[HttpPost]
        //[ActionName("SaveSmsFormat")]
        //public JsonResult MenuSmsActionInsert(AuditorModel _Model)
        //{
        //    ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
        //    if (!ModelState.IsValid)
        //        return Json(JsonModelError.Get(ModelState.Values));

        //    return Json(_Model.AddSmsFormat());
        //}

        [HttpGet]
        [ActionName("AuditorList")]
        public string GetAllAuditor(string WhereClause = "")
        {
            AuditorModel objAuditormaster = new AuditorModel(new FileLogger());

            return COMMON_FUNCTION.DataTableToJSONString(objAuditormaster.GetAllAuditor(WhereClause));
        }

        [HttpGet]
        [ActionName("GetAuditUplodedDocument")]
        public JsonResult GetAuditUplodedDocument(string Id)
        {
            AuditorModel objAuditormaster = new AuditorModel(new FileLogger());
            var data = objAuditormaster.GetAuditUplodedDocument(Id);

            DocumentListEntity finalData = new DocumentListEntity();

            if (data == null || data.Rows.Count == 1)
                return Json(finalData);

            var temp = data.AsEnumerable().Select(m => new
            {
                UploadTypeEnglish = m.Field<string>("UploadTypeEnglish"),
                UploadtypeHindi = m.Field<string>("UploadtypeHindi"),
                UploadFileName = m.Field<string>("UploadFileName"),
                DocumentType = m.Field<string>("UploadFileName").Split(new char[] { '.' }).LastOrDefault()
            });
            List<string> _documentTypes = null;
            if (temp.Count() > 0)
            {
                _documentTypes = temp.Select(s => s.DocumentType).Distinct().ToList();
            }
            finalData.DocumentTypes = _documentTypes;
            finalData.DocumentList = new List<DocumentDescriptionEntity>();

            if (_documentTypes != null && _documentTypes.Count() == 0)
                return Json(finalData);

            foreach (var item in temp)
            {
                if (finalData.DocumentList.Where(n => n.DocumentNameEnglish == item.UploadTypeEnglish).Count() == 0)
                {
                    DocumentDescriptionEntity obj = new DocumentDescriptionEntity();
                    obj.DocumentNameEnglish = item.UploadTypeEnglish;
                    obj.DocumentNameHindi = item.UploadtypeHindi;
                    List<DocumenTypeDetailEntity> _documenTypeDetail = new List<DocumenTypeDetailEntity>();

                    foreach (var item2 in _documentTypes)
                    {
                        _documenTypeDetail.Add(new DocumenTypeDetailEntity()
                        {
                            DocumentType = item2,
                            URL = item2.Equals(item.DocumentType) ? item.UploadFileName : string.Empty
                        });
                    }

                    finalData.DocumentList.Add(obj);
                    finalData.DocumentList.Where(n => n.DocumentNameEnglish.Equals(item.UploadTypeEnglish)).First().DocumenTypeDetails = _documenTypeDetail;
                }
                else if (finalData.DocumentList.Where(n => n.DocumentNameEnglish.Equals(item.UploadTypeEnglish) && n.DocumenTypeDetails.Where(s => s.DocumentType.Equals(item.DocumentType) && s.URL.Equals(string.Empty)).Count() == 1).Count() == 1)
                {
                    finalData.DocumentList.Where(n => n.DocumentNameEnglish.Equals(item.UploadTypeEnglish)).First().DocumenTypeDetails.Where(s => s.DocumentType.Equals(item.DocumentType)).First().URL = item.UploadFileName;
                }
            }


            return Json(finalData, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ActionName("GetSocietyAuditDataByFinYearId")]
        public string GetSocietyAuditDataByFinYearId(string SocietyIdENC, string FinYearId)
        {
            AuditorModel objAuditormaster = new AuditorModel(new FileLogger());
            DataSet DT = objAuditormaster.GetSocietyAuditDataByFinYearId(SocietyIdENC, FinYearId);
            return JsonConvert.SerializeObject(DT, Newtonsoft.Json.Formatting.Indented);

        }

        [HttpPost]
        [ActionName("GetCentralCategoryListForAudit")]
        public string CentralCategoryListForAudit()
        {
            AuditorModel objAuditormaster = new AuditorModel(new FileLogger());
            DataTable DT = objAuditormaster.CentralCategoryListForAudit(_LoggedInUser.LevelId);
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);

        }

        [HttpPost]
        [ActionName("GetSocietyAuditDataByRoleFinYearId")]
        public JsonResult GetSocietyAuditDataByRoleFinYearId(DataPagingModel Details, string FinYearId)
        {
            AuditorModel objAuditormaster = new AuditorModel(new FileLogger());
            DataTable DT = objAuditormaster.GetSocietyAuditDataByRoleFinYearId(ref Details, int.Parse(FinYearId));
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region AuditorRequest
        [HttpGet]
        public ActionResult AuditorRequest()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);
            Result = true;
            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.AuditorRequest, (Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpPost]
        [ActionName("SaveAuditorRequest")]
        public string SaveAuditor(AuditorRequestEntity _Model)
        {
            ModelState.Remove("CreationDate"); ModelState.Remove("UpdationDate"); ModelState.Remove("UpdatedBy");

            _Model.SSO_ID = _LoggedInUser.SSOID;
            if (!ModelState.IsValid)
                return JsonModelError.Get(ModelState.Values);

            _Model.OtpValEnc = _Model.OtpValEnc.ToString().Replace(' ', '+');
            _Model.OtpValEnc = _Model.OtpValEnc.ToString().Replace(' ', '+');
            _Model.OtpValEnc = _Model.OtpValEnc.ToString().Replace(' ', '+');

            if (_Model.OtpVal != COMMON_FUNCTION.Decrypt(_Model.OtpValEnc))
            {
                return "-5";
            }

            AuditorModel objAuditorModel = new AuditorModel(new DBLogger());


            return objAuditorModel.AddAuditorRequest(_Model).ToString();
        }

        [HttpPost]
        [ActionName("GetAuditorList")]
        public string GetAuditorList(string WhereClause = "")
        {
            if (WhereClause == "")
            {
                WhereClause = " AND SSO_ID='" + _LoggedInUser.SSOID + "' ";
            }

            //SSORoleUserMapping.SSOWSSoapClient ssoclient = new SSORoleUserMapping.SSOWSSoapClient();
            //ssoclient.RoleUserMapping("RAJMANDI.TEST", "Test@1234", objTraderBrokerEntity.SSOID.ToString().ToUpper().Trim(), "TD", "ADD");


            AuditorModel objAuditormaster = new AuditorModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objAuditormaster.GetAuditorList(WhereClause));
        }

        [HttpPost]
        [ActionName("SendOtp")]
        public string SendOtp(string MobNo)
        {
            Random random = new Random();
            var ran = random.Next(1000, 10000);
            string Responce = string.Empty;
            EsanchaarController Obj = new EsanchaarController();
            string MobileNo = System.Web.HttpContext.Current.Session[SessionIdentity.OditerMobile].ToString();
            if (MobileNo != "")
                Responce = Obj.SendSMS("", ran.ToString(), new List<string>() { MobileNo }, 1);
            else
                Responce = Obj.SendSMS("", ran.ToString(), new List<string>() { MobNo }, 1);
            if (Responce == "200")
            {
                Responce += "#" + COMMON_FUNCTION.Encrypt(ran.ToString());
            }
            else
                Responce = "OTP could not send please try after some time.";
            return Responce;
        }

        [HttpPost]
        [ActionName("GetHouseAuditReport")]
        public string GetHouseAuditReport(string Year, string DistrictId)
        {
            AuditorModel objAuditormaster = new AuditorModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objAuditormaster.GetHouseAuditReport(Year, DistrictId));
        }
        [HttpPost]
        [ActionName("GetNotHouseAuditReport")]
        public string GetNotHouseAuditReport(string Year, string DistrictId, string SaveStatus)
        {
            AuditorModel objAuditormaster = new AuditorModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objAuditormaster.GetNotHouseAuditReport(Year, DistrictId, SaveStatus));
        }

        [HttpPost]
        [ActionName("GetAuditYearList")]
        public string GetAuditYearList()
        {
            AuditorModel objAuditormaster = new AuditorModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objAuditormaster.GetAuditYearList());
        }

        [HttpGet]
        public ActionResult UploadAuditByAuditor()
        {
            return View();
        }


        [HttpPost]
        [ActionName("GetSocietyListAssignedToAuditor")]
        public string GetSocietyListAssignedToAuditor(string FYearId)
        {
            AuditorModel objAuditormaster = new AuditorModel(new FileLogger());
            string SSO_ID = _LoggedInUser.SSOID.ToString();
            return COMMON_FUNCTION.DataTableToJSONString(objAuditormaster.GetSocietyListAssignedToAuditor(SSO_ID, FYearId));
        }


        [HttpPost]
        [ActionName("AuditLiatByAuditor")]
        public string AuditLiatByAuditor(string FYearId)
        {
            AuditorModel objAuditormaster = new AuditorModel(new FileLogger());
            string SSO_ID = _LoggedInUser.SSOID.ToString();
            return COMMON_FUNCTION.DataTableToJSONString(objAuditormaster.GetSocietyListAssignedToAuditor(SSO_ID, FYearId));
        }

        [HttpPost]
        [ActionName("AuditorUploadAuditFile")]
        public int AuditorUploadAuditFile(SocietyAuditorAssigmentEntity _Model)
        {
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            AuditorModel AuditorModel = new AuditorModel(new DBLogger());

            int Result = 0;
            Result = _Model.Id;

            if (_Model.IsPermanentSave)
                objSocietyModel.PermanentsaveSocietyAuditorAssigments(Result);

            if (Result > 0)
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
                            obj.CreateBy = Convert.ToInt32(_LoggedInUser.UserId);
                            obj.DocumentType = stringSplitter[0];
                            lst.Add(obj);
                        }
                    }
                }//_Model.Id != 0 &&

                AuditorModel.SaveAuditStartEndDate(_Model.AuditStartDate, _Model.AuditEndDate, Result, _Model.FeeDate, _Model);

                if (lst.Count > 0)
                {
                    Result = objSocietyModel.AuditorAssignmentFilesInsert(lst);
                }


                return Result;
            }
            else
            {
                return Result;
            }

        }

        #endregion

        [HttpPost]
        [ActionName("SaveAuditOrder")]
        public string SaveAuditOrder(AuditOrderEntity _Entity)
        {
            AuditorModel obj = new AuditorModel();
            if (_LoggedInUser.LevelId == 3)
                _Entity.PlaceId = _LoggedInUser.DistrictId;
            else if (_LoggedInUser.LevelId == 2)
                _Entity.PlaceId = _LoggedInUser.DivisionId;
            else if (_LoggedInUser.LevelId == 1)
                _Entity.PlaceId = 6;


            _Entity.SSOId = _LoggedInUser.SSOID;
            _Entity.CreatedBy = _LoggedInUser.UserId;
            _Entity.LevelId = _LoggedInUser.LevelId;

            return obj.SaveAuditOrder(_Entity);



        }

        #region Order_Report_UMESH


        [HttpGet]
        public string GetOrderListReport(string OrderNo, int Flag = 0)
        {
            AuditorModel obj = new AuditorModel(new DBLogger());
            //PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(obj.GetOrderauditorListReport(OrderNo, Flag));
        }


        [HttpPost]
        public JsonResult GetOrderListReport_V1(DataPagingModel Details, string OrderNo, string fIYear, string ListType = "D", int Flag = 0)
        {
            AuditorModel obj = new AuditorModel(new DBLogger());
            DataTable DT = obj.GetOrderauditorListReport_V1(ref Details, OrderNo, fIYear, Flag);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public string GoForDeleteresion(string OrderNo, string DeleteResions)
        {
            string result;
            AuditorModel obj = new AuditorModel(new DBLogger());
            result = obj.DeleteWithresionSave(OrderNo, DeleteResions);
            return result;

        }
        [HttpGet]
        public ActionResult UploadAuditByDepartment()
        {
            return View();
        }
        [HttpPost]
        public string UserSocietyDataForAuditRPT()
        {
            CitizenModel objMenuActionModel = new CitizenModel(new FileLogger());

            DataTable DT = objMenuActionModel.SocietyListForAuditRPT(Convert.ToInt32(_LoggedInUser.UserId), _LoggedInUser.SSOID);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }
        #endregion
        [HttpGet]
        public ActionResult EmpanelledAudit()
        {
            var date = DateTime.Now.AddYears(-3).ToString("dd/MM/yyyy");
            ViewBag.RegstrationDate = date;
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Auditor, (Request.Cookies[SessionIdentity.Culture]).Value);
            ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        [HttpGet]
        public ActionResult EmpanelledAudit1()
        {
            var date = DateTime.Now.AddYears(-3).ToString("dd/MM/yyyy");
            ViewBag.RegstrationDate = date;
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Auditor, (Request.Cookies[SessionIdentity.Culture]).Value);
            ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        [HttpGet]
        public ActionResult EmpanelledAuditApprovalProcess()
        {
            var date = DateTime.Now.AddYears(-3).ToString("dd/MM/yyyy");
            ViewBag.RegstrationDate = date;
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Auditor, (Request.Cookies[SessionIdentity.Culture]).Value);
            ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }


        [HttpPost]
        public string UploadEmpanelledAuditFile()
        {
            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {

                    string fileDirectory = Server.MapPath("~/Content/EmpanelledAuditFile");

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
        // [ValidateInput(true)]
        [ValidateInputAttribute(true)]
        public JsonResult SaveAuditEmpanelled(AuditEmpanelledModel ApplicationModel)
        {
            string result = "";
            AuditorModel obj = new AuditorModel(new FileLogger());
            result = obj.InsertAuditEmpanelled(ApplicationModel);
            return Json(new { Result = result }, JsonRequestBehavior.AllowGet);
        }
        //DataPagingModel Details
        [HttpPost]
        [ActionName("GetAuditEmpanelledDetails")]
        public JsonResult GetAuditEmpanelledDetails()
        {
            DataTable Dt = null;
            AuditorModel obj = new AuditorModel(new FileLogger());
            Dt = obj.GetAuditEmpanelledDetails();
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(Dt) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("GetAuditEmpanelledDetailsById")]
        public JsonResult GetAuditEmpanelledDetailsById(int AuditEmpanelledID)
        {
            DataSet Dt = null;
            AuditorModel obj = new AuditorModel(new FileLogger());
            Dt = obj.GetAuditEmpanelledDetailsByID(AuditEmpanelledID);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(Dt) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAuditEmpanelledList(DataPagingModel Details)
        {
            AuditorModel obj = new AuditorModel(new FileLogger());
            DataTable DT = obj.GetAuditEmpanelledList(ref Details);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string GetTransactionFinalStatus(string AuditorId, string EMitraAppRequestID, string SERICEID, string UniqueId = "0")
        {

            //EMitraAppRequestID = "172413";  //12345678
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            SocietyRegistrationEntity objSocietyRegistrationEntity = new SocietyRegistrationEntity();
            if (_LoggedInUser.UserType == "K")
            {
                objSocietyRegistrationEntity.UserType = _LoggedInUser.UserType;
                objSocietyRegistrationEntity.SocietyIDEnc = AuditorId;
                objSocietyRegistrationEntity.EMitraAppRequestID = EMitraAppRequestID;
                objSocietyRegistrationEntity.ServicesId = int.Parse(SERICEID);
                objSocietyRegistrationEntity.UniqueId = int.Parse(UniqueId);
            }
            else
            {
                objSocietyRegistrationEntity.SSOID = _LoggedInUser.SSOID;
                objSocietyRegistrationEntity.UserType = _LoggedInUser.UserType;
                objSocietyRegistrationEntity.SocietyIDEnc = AuditorId;
                objSocietyRegistrationEntity.ServicesId = int.Parse(SERICEID);
                objSocietyRegistrationEntity.EMitraAppRequestID = EMitraAppRequestID;
            }
            EmitraModelFactory OBJ = new EmitraModelFactory(new DBLogger());
            var returnvalue = OBJ.VerifyTransaction(objSocietyRegistrationEntity);
            return "";
        }

        [HttpPost]
        public JsonResult DeleteAuditEmpanelledDetails(int Id, string Type)
        {
            AuditorModel objAuditormaster = new AuditorModel(new FileLogger());
            string result = objAuditormaster.DeleteAuditEmpanelledDetails(Id, Type);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string ExportAuditEmpanelled(DataPagingModel Details)

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
                AuditorModel obj = new AuditorModel(new FileLogger());
                DataTable DT = obj.GetAuditEmpanelledList(ref Details);


                System.Text.StringBuilder sb = new System.Text.StringBuilder();


                string devFontSize = "17px";
                string fontSize = "font-size: 13px;";

                sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:50px'>");
                sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px'>");
                //sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                //sb.Append("<img style='width:90px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                //sb.Append("</td></tr>");           
                sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");
                sb.Append("<label style='font-size:20px;'> कार्यालय- रजिस्ट्रार, सहकारी समितियॉं राजस्थान ,जयपुर </label>");
                sb.Append("</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td style='text-align:center;'> ");
                sb.Append(" भाग 'क' ");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='text-align:center;'> ");
                sb.Append(" चार्टर्ड अकाउंटेंट का पैनल 2021-24 ");
                sb.Append("</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td ><br/>");

                sb.Append("<table cellpadding='5' cellspacing='0' border='1''>");


                sb.Append("<tr>");
                sb.Append("<th style='text-align:left;font-size:10px;'> क्र सं.  </th>");
                sb.Append("<th style='text-align:left;font-size:10px;'> फर्म का नाम  </th>");
                sb.Append("<th style='text-align:left;font-size:10px;'> फर्म का पता  </th>");
                sb.Append("<th style='text-align:left;font-size:10px;'> ICAI संस्थान का पंजीयन क्रमांक  </th>");
                sb.Append("<th style='text-align:left;font-size:10px;'> ICAI संस्थान का पंजीयन दिनांक  </th>");
                sb.Append("<th style='text-align:left;font-size:10px;'> फोन नम्बर  </th>");
                //sb.Append("<th style='text-align:left;font-size:10px;'> पार्टनर्स की संख्या </th>");
                //sb.Append("<th style='text-align:left;font-size:10px;'> ब्रांच का पता  </th>");
                sb.Append("<th style='text-align:left;font-size:10px;'> RBI पैनल यूनिक कोड </th>");
                sb.Append("<th style='text-align:left;font-size:10px;'> एम्पैनलमेंट नम्बर </th>");
                sb.Append("</tr>");

                int i = 1;
                foreach (DataRow dr in DT.Rows) // search whole table
                {
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;font-size:10px;'> " + (i++) + " </td>");
                    sb.Append("<td  style='text-align:left;font-size:10px;'> " + dr["ApplicantNameEn"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left;font-size:10px;'> " + dr["FirmAddress"].ToString() + " </td>");
                    sb.Append("<td style='text-align:left;font-size:10px;'> " + dr["RegistrationNo"].ToString() + " </td>");
                    sb.Append("<td style='text-align:left;font-size:10px;'> " + dr["RegDate1"].ToString() + " </td>");
                    sb.Append("<td style='text-align:left;font-size:10px;'> " + dr["MobileNo"].ToString() + " </td>");
                    //sb.Append("<td style='text-align:left;font-size:10px;'> " + dr["TotalParter"].ToString() + " </td>");
                    //sb.Append("<td  style='text-align:left;font-size:10px;'> " + dr["BranchAddress"].ToString() + " </td>");
                    sb.Append("<td style='text-align:left;font-size:10px;'> " + dr["RBICode"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left;font-size:10px;'> " + dr["EmpanelledNo"].ToString() + " </td>");
                    sb.Append("</tr>");

                }

                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</div>");
                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                ////sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));

                //GenerateAuditEmpanelledCertificate(Details, "");

                //sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true));
                //var bytes = COMMON_FUNCTION.GePDFBytes(sb1, "Landscape");
                //string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                return sb.ToString().Replace("<br>", "<br/>");

            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Auditor", FunctionName = "ExportAuditEmpanelled(DataPagingModel Details)" }, ex);
                result = "Error";
            }
            return null;
        }

        [HttpPost]
        public void GenerateAuditEmpanelledCertificate(DataPagingModel Details, string Auditors, string TXN)
        {
            //= new DataPagingModel();
            AuditorModel obj = new AuditorModel(new FileLogger());
            DataTable DT = obj.GetAuditEmpanelledList(ref Details);


            DataTable dt = new DataTable();

            string actId = "1";
            string lbl_Registraionno = "COOP/2021/JAIPUR/201670";
            string devFontSize = "17px";
            string fontSize = "font-size: 13px;";
            string[] AuditorId = Auditors.Split(',');
            string sNoL = "फा. (45) सविरा / एम्पैनलमेंट / 2024-27";
            bool isGeneratePDF = false;
            string esignfiles = "";
            esignfiles = "";
            int count = 0;

            foreach (DataRow dr in DT.Rows) // search whole table
            {
                if (AuditorId.Contains(dr["AuditEmpanelledId"].ToString()))
                {
                    isGeneratePDF = false;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:20px'>");
                    sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:20px;margin-right:50px'>");
                    //sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                    //sb.Append("<img style='width:90px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                    //sb.Append("</td></tr>");
                    //sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                    sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:32px;'>" + UnicodeToKrutidev.GetKrutidev(" कार्यालय रजिस्ट्रार, सहकारी समितियाँ, राजस्थान ") + "</label></b>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");
                    //sb.Append("<b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("बाईस गोदाम सर्किल, नेहरू सहकार भवन, जयपुर") + "</label></b><br/><br/><br/>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
                    sb.Append("<table style='width:100%'>");
                    sb.Append("<tr>");
                    //sb.Append("<td style='text-align:left; " + fontSize + "'> क्रमांक :- <b>  फा. 8(45) सविरा @ ऑडिट @ एम्पैनलमेंट @ 2024-27  </b></td>");/// " + dr["EmpanelledNo"].ToString().Replace("CAI/2021/JAIPUR-","").Replace("CAF/2021/JAIPUR-", "") + "
                    //sb.Append("<td style='text-align:left; " + fontSize + "'> क्रमांक :- <b>" + UnicodeToKrutidev.GetKrutidev(" सविरा/एम्पैनलमेंट/ ") + "</b></td>");
                    sb.Append("<td style='text-align:left; " + fontSize + "'> क्रमांक :- <b>  " + sNoL + "  </b></td>");
                    //sb.Append("<td style='width:20%;text-align:right;" + fontSize + "'> दिनांक :- 07/10/2022 </td>");
                    //sb.Append("<td style='width:20%;text-align:right;" + fontSize + "'> दिनांक :- 28/03/2024 </td>");
                    sb.Append("<td style='width:20%;text-align:right;" + fontSize + "'> दिनांक :- " + DateTime.Now.ToString("dd/MM/yyyy") + " </td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' style='width:49%;text-align:left;" + fontSize + "'><br/><br/> नाम - " + dr["ApplicantNameEn"].ToString() + " <br/> पता - " + dr["FirmAddress"].ToString() + " <br/> फोन - " + dr["MobileNo"].ToString() + " <br/><br/><br/></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' style='width:49%;text-align:left;" + fontSize + "'> <p><br/> विषय : सहकारी सोसायटियों की लेखापरीक्षा हेतु एम्पैनलमेंट। </p> </td>");
                    sb.Append("</tr>");


                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");


                    sb.Append("<tr>");
                    //sb.Append("<td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'><p style='text-indent: 60px;'> विषयान्तर्गत सूचित किया जाता है की आपको राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 के प्रावधानों के तहत सहकारी सोसायटियों की सांविधिक लेखापरीक्षा हेतु गठित पैनल में क्रमांक " + dr["EmpanelledNo"].ToString() + " पर एम्पैनल किया गया है। उक्त एम्पैनलमेंट दिनांक 01.04.2021 से दिनांक 31.03.2024 तक की अवधि के लिये किया गया है। एम्पैनलमेंट आपके द्वारा आवेदन पत्र में उपलब्ध करायी गयी सूचनाओं एवं सलग्न प्रमाण पत्रों के आधार पर किया गया है। सूचनाओं के गलत पाये जाने की स्थिति में आप स्वयं उत्तरदायी होंगे। </p>");
                    //sb.Append("<p> आप सहकारी सोसायटियों की सांविधिक लेखापरीक्षा राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 व राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72 एवं 73 में वर्णित प्रावधानों के तहत पूर्ण करेंगे व आवेदन के समय स्वीकार किये गये समस्त नियम एवं शर्तो की अनुपालना सुनिश्चित करेंगे।  </p></td>");

                    sb.Append("<td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'><p style='text-indent: 60px;'> उपरोक्त विषयान्तर्गत सूचित किया जाता है की आपको राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 के प्रावधानों के तहत सहकारी सोसायटियों की सांविधिक लेखापरीक्षा हेतु गठित पैनल में क्रमांक " + dr["EmpanelledNo"].ToString() + " पर पूर्व में अधिसूचित" +
                        " एम्पैनलमेंट दिनांक 01.04.2024 से दिनांक 31.03.2027 तक की शेष अवधि (07.05.2026 से 31.03.2027 तक) के लिये एम्पैनल किया जाता है। एम्पैनलमेंट आपके द्वारा आवेदन पत्र में उपलब्ध करायी गयी सूचनाओं एवं सलग्न प्रमाण पत्रों के आधार पर किया गया है। सूचनाओं के गलत पाये जाने की स्थिति में आप स्वयं उत्तरदायी होंगे। </p>");
                    sb.Append("<p> आप सहकारी सोसायटियों की सांविधिक लेखापरीक्षा राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 व राजस्थान सहकारी सोसायटी नियम 2003 के नियम 72 एवं 73 में वर्णित प्रावधानों के तहत एवं विभाग द्वारा समय-समय पर जारी होने वाले आदशों/परिपत्रों की पूर्ण पालना करेंगे व आवेदन के समय स्वीकार किये गये समस्त नियम एवं शर्तो की अनुपालना सुनिश्चित करेंगे।  </p>");
                    sb.Append("<p style='text-align: right; font-size: 18px; line-height: 35px; padding-right: 60px;'><b>(शिवदयाल मीना)<br />संयुक्त मुख्य अंकेक्षक (जनरल)&ensp;&ensp;</b></p></td>");

                    sb.Append("</tr>");


                    //sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'><br/><br/><br/><br/><br/><br/><br/></td></tr>");
                    sb.Append("</table>");
                    sb.Append("</div>");


                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));

                    string fileNameCerti = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + ".pdf";
                    string filepath = System.Web.HttpContext.Current.Server.MapPath("~/Content/AuditReport/") + fileNameCerti;


                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath, ""))
                    {
                        //obj.UpdateAuditEmpanelledCertificate(dr["AuditEmpanelledId"].ToString(), fileNameCerti);
                        esignfiles += (esignfiles != "" ? "#" : "") + filepath;
                        isGeneratePDF = true;
                        dr["CertificateName"] = fileNameCerti;
                    }
                    else
                    {
                        //return "Fail";
                    }
                }
            }

            if (isGeneratePDF)
            {
                var esignResult = COMMON_FUNCTION.EsinghPDF(TXN, esignfiles, "Jaipur", "Joint chief auditor");
                var result = esignResult.data;
                if (esignResult.status == "1")
                {
                    foreach (DataRow dr in DT.Rows) // search whole table
                    {
                        if (AuditorId.Contains(dr["AuditEmpanelledId"].ToString()))
                        {
                            string mobileNo = (Request.Url.Host.Contains("localhost") ? "9460934860" : dr["MobileNo"].ToString()); ;
                            obj.UpdateAuditEmpanelledCertificate(dr["AuditEmpanelledId"].ToString(), dr["CertificateName"].ToString());
                            new EsanchaarController().SendSocietySMS("वर्ष 2024-27 एम्पेनलमेंट के क्रम में सहकारिता विभाग की साइट rajsahakar.rajasthan.gov.in का अवलोकन करें।", mobileNo);
                        }
                    }
                }
            }
        }

        #region Madhuri Payment Verify

        [HttpPost]
        public string GetEmitraTransaction(string SearchText, string ServiceType)
        {
            DataTable Dt = null;
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            CitizenController objCitizenModel = new CitizenController();
            int DistrictId = _LoggedInUser.DistrictId;
            Dt = objSocietyModel.GetEmitraTransaction(SearchText, ServiceType);

            string PayStatus = "";
            for (int i = 0; Dt.Rows.Count > i; i++)
            {
                PayStatus = UpdateAudotorEmpaneledTransaction(Dt.Rows[i]["AuditorIdENC"].ToString(), Dt.Rows[i]["EMitraAppRequestID"].ToString(), "7392", Dt.Rows[i]["SocietyId"].ToString());
                if (PayStatus.ToUpper().Contains("SUCCESS"))
                {
                    i = Dt.Rows.Count;
                }

            }

            return PayStatus;
        }

        [HttpPost]
        public string UpdateAudotorEmpaneledTransaction(string SocietyIdEnc, string EMitraAppRequestID, string SERICEID, string UniqueId = "0")
        {
            DataTable Dt = null;
            string returnval = "";
            VerifyResponse resp = null;
            PGResponse respg = null;
            string encData = "";
            string decryptData = "";
            PGResponse objPGResponse = null;

            string DivHtml = "";

            int i = -1;
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            SocietyRegistrationEntity objSocietyRegistrationEntity = new SocietyRegistrationEntity();

            if (_LoggedInUser.UserType == "K")
            {
                objSocietyRegistrationEntity.UserType = _LoggedInUser.UserType;
                objSocietyRegistrationEntity.SocietyIDEnc = SocietyIdEnc;
                objSocietyRegistrationEntity.EMitraAppRequestID = EMitraAppRequestID;
                objSocietyRegistrationEntity.ServicesId = int.Parse(SERICEID);
                objSocietyRegistrationEntity.UniqueId = int.Parse(UniqueId);

            }
            else
            {
                objSocietyRegistrationEntity.SSOID = _LoggedInUser.SSOID;
                objSocietyRegistrationEntity.UserType = "C";
                objSocietyRegistrationEntity.SocietyIDEnc = SocietyIdEnc;
                objSocietyRegistrationEntity.ServicesId = int.Parse(SERICEID);
                objSocietyRegistrationEntity.EMitraAppRequestID = EMitraAppRequestID;
            }

            EmitraModelFactory objEmitra = new EmitraModelFactory(new DBLogger());
            string PayStatus = objEmitra.GetPaymentTransaction(objSocietyRegistrationEntity);


            EmitraModelFactory objEmitraModel = new EmitraModelFactory(new FileLogger());

            encData = PayStatus;

            decryptData = encData;

            DivHtml = objCitizenModel.UpdateOnlinePaymentStatus("", SocietyIdEnc, EMitraAppRequestID, "NO", decryptData, "1");

            objPGResponse = JsonConvert.DeserializeObject<PGResponse>(DivHtml);
            if (objPGResponse.STATUS.ToString().Contains("SUCCESS"))
            {
                AuditorModel objEM = new AuditorModel(new FileLogger());
                objEM.SetAuditEmpanelledFinalSubmit(EMitraAppRequestID);
            }
            else
            {
            }
            //}
            return PayStatus;
        }

        #endregion



        #region Madhhuri chk If Society Previously Audit By CA or Department

        public string chkIfSocietyPreviouslyAuditByCAorDepartment(string SocietyIdEnc)
        {

            AuditorModel auditorModel = new AuditorModel();
            string a = auditorModel.chkIfSocietyPreviouslyAuditByCAorDepartment(SocietyIdEnc);
            return a;
        }
        #endregion



        #region Added New Filter FinYear on 13-04-2026
        [HttpGet]
        public JsonResult GetSectorDetailsByIsCredit(string actionName = "")
        {
            var responce = new APIResponce();
            AuditorModel _auditorModel = new AuditorModel(new FileLogger());
            var ds = _auditorModel.GetFinYearFilterData(actionName);
            if (ds != null)
            {
                responce.Data = new
                {
                    FinYearData = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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



        public ActionResult AuditorPurchaseSale()
        {
            return View();
        }


        public ActionResult AuditorCurrentYearForm()
        {
            return View();
        }


        public ActionResult AuditorEquityLiabilities()
        {
            return View();
        }


        public ActionResult AuditorSaleLiabilities()
        {
            return View();
        }



        #region GetAuditMPRCommiteeData On 27-10-2026
        [HttpGet]
        [ActionName("GetCurrentFinancialYearName")]
        public string GetCurrentFinancialYearName()
        {
            DataTable dt = new AuditorModel().GetCurrentFinancialYearName();
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpGet]
        public JsonResult GetAuditMPRMasterData()
        {
            AuditorModel objModal = new AuditorModel(new FileLogger());
            var responce = new APIResponce();
            try
            {
                var ds = objModal.GetAuditMPRMasterData();
                if (ds != null)
                {
                    responce.Data = new
                    {
                        MPRData = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                    };
                    responce.Status = "200";
                    responce.Message = "Success";
                }
                else
                {
                    responce.Status = "404";
                    responce.Message = "Record not founds.";
                }
            }
            catch (Exception ex)
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }

            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }


        #endregion




    }
}