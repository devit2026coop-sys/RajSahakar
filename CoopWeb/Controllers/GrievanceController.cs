using com.sun.security.ntlm;
using CoopDB;
using Cooperative.Filters;
using Cooperative.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using sun.swing;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using static Cooperative.Models.GrievanceModel;


namespace Cooperative.Controllers
{
   
    public class GrievanceController : Controller
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
      
       csCommon Objsbm2 = new csCommon();

        public ActionResult SubmitGrievance()
        {
            return View();
        }

        [HttpPost]
        public string GetDepartmentList()
        {
            GrievanceModel ObjGrievanceModel = new GrievanceModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(ObjGrievanceModel.GetDepartmentList("",""));

            

        }

        [HttpPost]
        public string GetModuleList(string Department)
        {
            GrievanceModel ObjGrievanceModel = new GrievanceModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(ObjGrievanceModel.GetModuleList(Department, "", ""));

            

        }
       

        [HttpPost]
        public string GetCategoryList()
        {
            GrievanceModel ObjGrievanceModel = new GrievanceModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(ObjGrievanceModel.GetCategoryList());
        }

        public string GetDeveloperList()
        {
            GrievanceModel ObjGrievanceModel = new GrievanceModel(new DBLogger());

            DataTable dt = ObjGrievanceModel.GetDeveloperList();
            string ddHTML = "";
            foreach (DataRow dr in dt.Rows)
            {
                ddHTML += "<option value='" + dr["AgriLovValuesId"] + "'>" + dr["AgriLovEn"] + "</option>";
            }
            return (ddHTML.ToString());
        }
        [HttpPost]
        [ActionName("GetIssueRelatedList")]
        public string GetIssueRelatedList()
        {
            GrievanceModel GrievanceModel = new GrievanceModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(GrievanceModel.GetIssueRelatedList());
        }
        [HttpPost]
        [ActionName("GetGrivanceCategoryList")]
        public string GetGrivanceCategoryList()
        {
            GrievanceModel GrievanceModel = new GrievanceModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(GrievanceModel.GetGrivanceCategoryList());
        }
        [HttpPost]
        [ActionName("GetGrivanceSubCategoryList")]
        public string GetGrivanceSubCategoryList(int GriveanceCategoryId)
        {
            GrievanceModel GrievanceModel = new GrievanceModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(GrievanceModel.GetGrivanceSubCategoryList(GriveanceCategoryId));
        }
        [HttpPost]
        [ActionName("SaveGrievance")]
        public string SaveGrievance(GrievanceEntity Entity)
        {
            GrievanceModel GrievanceModel = new GrievanceModel(new DBLogger());
            
            int Result = 0;


            int postId = 0;// _LoggedInUser.PostId;

                string NewGuid = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "");
                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = Request.Files;
                    string fname = "";
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string filepath = System.Web.HttpContext.Current.Server.MapPath("~/ContentNew/Grievance/");

                        bool exists = System.IO.Directory.Exists(filepath);
                        if (!exists)
                        {
                            System.IO.Directory.CreateDirectory(filepath);
                        }
                        fname = filepath + NewGuid + System.IO.Path.GetExtension(file.FileName);

                        file.SaveAs(fname);

                        Entity.Attachment = NewGuid + System.IO.Path.GetExtension(file.FileName);
                    }
                }
                else
                {
                    Entity.Attachment = "";
                }


                Entity.UserId = Convert.ToInt32(_LoggedInUser.UserId);
                Entity.SSOId = _LoggedInUser.SSOID;
                Entity.MobileNo = _LoggedInUser.MobileNo;


                Result = GrievanceModel.SaveGrievance(Entity);
            

            if (Result > 0)
            {
                //string smstext = "";

                //if (Entity.MobileNo.Length > 9)
                //{
                //    List<string> Mobilelst = new List<string>();

                //    if (HttpContext.Request.Url.Host.ToLower().Contains("rajsahakar.rajasthan.gov.in"))
                //    {
                //       Mobilelst.Add(_LoggedInUser.MobileNo);
                //    }
                //    else
                //    {
                //        Mobilelst.Add("9828654214");
                //    }
                //    smstext = "आप की समस्या राजसहकार पोर्टल पर सफलतापूर्वक दर्ज कर दी गयी है समस्या क्रमांक : " + Result + " - सहकारी विभाग, राजस्थान सरकार";
                //    string sms = COMMON_FUNCTION.SendSMS_byListNewLatest("HIN", smstext, Mobilelst, "1407164137863406820", "1401650620000042602");
                //    if (Session[SessionIdentity.Admin] != null)
                //    {
                //        if (sms == "200")
                //        {

                //            Objsbm2.SMSlog(Result, _LoggedInUser.MobileNo, smstext, 1, "SMS sent" + sms, "GrievanceSubmission", "sms", "Grievance");

                //        }
                //        else
                //        {
                //            Objsbm2.SMSlog(Result, _LoggedInUser.MobileNo, smstext, 0, "SMS not sent" + sms, "GrievanceSubmission", "sms", "Grievance");
                //        }
                //    }
                //}

            }
            return Result.ToString();
        }

        public ActionResult GrievanceListIA()
        {
            ViewBag.RoleName = _LoggedInUser.RoleName_EN;
            return View();
        }

        public ActionResult GrievanceList()
        {
            ViewBag.RoleName = _LoggedInUser.RoleName_EN;
            return View();
        }


        [HttpPost]
        public string GetGrievanceList(string CategoryId, string Department, string ModuleId, string DeveloperId, string ComplainNo, string GrivanceCategoryid, string GrivanceSubCategoryid)
        {
            GrievanceModel obj = new GrievanceModel(new FileLogger());

            string WhereClause = "";

            if (ComplainNo != null && ComplainNo != "")
            {
                WhereClause += " and G.ComplainNo=" + ComplainNo;
            }
            else
            {
                if ((_LoggedInUser.RoleName_EN == "Developer") || (_LoggedInUser.RoleName_EN == "SYSTEM DEVELOPER"))
                {
                  
                    if ((DeveloperId != null && Convert.ToInt32(DeveloperId) > 0))
                    {
                        WhereClause += " and GD.AssignedTo=" + DeveloperId;
                    }
                    if ((ModuleId != null && Convert.ToInt32(ModuleId) > 0))
                    {
                        WhereClause += " and Module.ModuleId=" + ModuleId;
                    }
                    if ((GrivanceCategoryid != null && Convert.ToInt32(GrivanceCategoryid) > 0))
                    {
                        WhereClause += " And GX.GrivencecategoryId=" + GrivanceCategoryid;
                    }
                    if ((GrivanceSubCategoryid != null && Convert.ToInt32(GrivanceSubCategoryid) > 0))
                    {
                        WhereClause += " And GX.GrievanceSubCategoryMasterId=" + GrivanceSubCategoryid;
                    }
                    else
                    {
                        //WhereClause += " and GD.AssignedTo=" + DeveloperId;
                    }
                }
                else if (_LoggedInUser.RoleName_EN == "Informatics Assistant")
                {
                    WhereClause += "and Module.DeptEn='" + Department + "' OR Module.ModuleId='" + ModuleId + "' OR (GD1.AssignedTo=" + _LoggedInUser.UserId
                        + " OR (GD1.AssignedTo IN (SELECT UserId FROM GrievanceIALeave WHERE G.status = 1 and GETDATE() BETWEEN FromDate AND ToDate)))";
                }
                else
                {
                    if (CategoryId != null && Convert.ToInt32(CategoryId) > 0)
                    {
                        WhereClause += " and G.CategoryId=" + CategoryId;
                    }
                    if (Department != "" && Department != "0")
                    {
                        WhereClause += " and Module.DeptEn='" + Department + "'";
                    }
                    if (ModuleId != null && Convert.ToInt32(ModuleId) > 0)
                    {
                        WhereClause += " and Module.ModuleId=" + ModuleId;
                    }
                }
            }
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(obj.GrievanceList(WhereClause, _LoggedInUser.UserId));
        }

        [HttpPost]
        public string GetGrievanceListBySSO()
        {
            GrievanceModel obj = new GrievanceModel(new FileLogger());
           
                return COMMON_FUNCTION.DataTableToJSONWithJSONNet(obj.GetGrievanceListBySSO());
        }

        [HttpPost]
        public JsonResult AssignToDeveloper(string GrievanceId, string DeveloperId, string Priority, string Remark, string ActionType, string Attachment, string IssueRelatedTO = "")
        {
            string AttachmentName = AttachmentSave();
            GrievanceModel GrievanceModel = new GrievanceModel(new DBLogger());
            return Json(GrievanceModel.AssignToDeveloper(Convert.ToInt32(GrievanceId), Convert.ToInt32(DeveloperId), Priority, Remark, ActionType, AttachmentName, IssueRelatedTO));
        }

        [HttpPost]
        public JsonResult DisposeGrievance(string GrievanceId, string Remark, string Attachment)
        {
            string AttachmentName = AttachmentSave();
            GrievanceModel GrievanceModel = new GrievanceModel(new DBLogger());
            return Json(GrievanceModel.DisposeGrievance(Convert.ToInt32(GrievanceId), Remark, AttachmentName));
        }

        [HttpPost]
        public JsonResult FinalAction(string GrievanceId, string DeveloperId, string Priority, string Remark, string ActionType, string Attachment)
        {
            string AttachmentName = AttachmentSave();

            GrievanceModel GrievanceModel = new GrievanceModel(new DBLogger());
            return Json(GrievanceModel.FinalAction(Convert.ToInt32(GrievanceId), Convert.ToInt32(DeveloperId), Priority, Remark, ActionType, AttachmentName));
        }

        [HttpPost]
        public string ActionByIA(string GrievanceId, string DeveloperId, string Priority, string Remark, string ActionType, int GrivanceCategoryid, int GrivanceSubCategoryid, string Attachment, string IssueRelatedTO = "")
        {
            GrievanceModel GrievanceModel = new GrievanceModel(new DBLogger());

            string AttachmentName = AttachmentSave();

            DataTable Dt = new DataTable();

            Dt = GrievanceModel.ActionByIA(Convert.ToInt32(GrievanceId), Convert.ToInt32(DeveloperId), Priority, Remark, ActionType, GrivanceCategoryid, GrivanceSubCategoryid, AttachmentName, IssueRelatedTO);

            //if (ActionType == "Dispose" && Dt.Rows.Count > 0)
            //{
            //    string smstext = "";

            //    if (Dt.Rows[0]["Mobile"].ToString().Length > 9)
            //    {
            //        List<string> Mobilelst = new List<string>();
            //        if (HttpContext.Request.Url.Host.ToLower().Contains("rajkisan.rajasthan.gov.in"))
            //        {
            //            Mobilelst.Add(Dt.Rows[0]["Mobile"].ToString());
            //        }
            //        else
            //        {
            //            Mobilelst.Add("8955255544");
            //        }

            //        smstext = "आप की समस्या क्रमांक :" + Dt.Rows[0]["ComplainNo"].ToString() + " का निस्तारण सफलतापूर्वक कर दिया गया है - कृषि विभाग, राजस्थान सरकार";

                  
            //        string sms = COMMON_FUNCTION.SendSMS_byListNewLatest("HIN", smstext, Mobilelst, "1407164380431082759", "1401650620000042602");
            //        if (sms == "200")
            //        {
            //            Objsbm2.SMSlog(Dt.Rows[0]["ComplainNo"].ToString(), Dt.Rows[0]["Mobile"].ToString(), smstext, 1, "SMS sent" + sms, "DisposeGrievance", "sms", "Grievance");
            //        }
            //        else
            //        {
            //            Objsbm2.SMSlog(Dt.Rows[0]["ComplainNo"].ToString(), Dt.Rows[0]["Mobile"].ToString(), smstext, 0, "SMS not sent" + sms, "DisposeGrievance", "sms", "Grievance");
            //        }
            //    }
            //}
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(Dt);
        }

        [HttpPost]
        public JsonResult ReopenByUser(string GrievanceId, string Remark)
        {
            string Attachment = "";
            GrievanceModel GrievanceModel = new GrievanceModel(new DBLogger());

            string NewGuid = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "");
            if (Request.Files.Count > 0)
            {
                HttpFileCollectionBase files = Request.Files;
                string fname = "";
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    string filepath = System.Web.HttpContext.Current.Server.MapPath("~/ContentNew/Grievance/");

                    bool exists = System.IO.Directory.Exists(filepath);
                    if (!exists)
                    {
                        System.IO.Directory.CreateDirectory(filepath);
                    }
                    fname = filepath + NewGuid + System.IO.Path.GetExtension(file.FileName);

                    file.SaveAs(fname);

                    Attachment = NewGuid + System.IO.Path.GetExtension(file.FileName);
                }
            }


            return Json(GrievanceModel.ReopenByUser(Convert.ToInt32(GrievanceId), Remark, Attachment));
        }

        [HttpPost]
        public JsonResult RemarkByUser(string GrievanceId, string Remark, string Attachment)
        {

            GrievanceModel GrievanceModel = new GrievanceModel(new DBLogger());
            string NewGuid = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "");
            if (Request.Files.Count > 0)
            {
                HttpFileCollectionBase files = Request.Files;
                string fname = "";
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    string filepath = System.Web.HttpContext.Current.Server.MapPath("~/ContentNew/Grievance/");

                    bool exists = System.IO.Directory.Exists(filepath);
                    if (!exists)
                    {
                        System.IO.Directory.CreateDirectory(filepath);
                    }
                    fname = filepath + NewGuid + System.IO.Path.GetExtension(file.FileName);

                    file.SaveAs(fname);

                    Attachment = NewGuid + System.IO.Path.GetExtension(file.FileName);
                }
            }
            else
            {
                Attachment = "";
            }



            return Json(GrievanceModel.RemarkByUser(Convert.ToInt32(GrievanceId), Remark, Attachment));
        }

        public ActionResult GrievanceReport()
        {
            return View();
        }

        [HttpPost]
        [ActionName("GetGrievancetReport")]
        public string GetGrievancetReport(string Distid, string Department, string ModuleId, string FromDate, string ToDate, string GrivanceCategoryid, string GrivanceSubCategoryid, string ComplainNo = "", string Status = "")
        {
            GrievanceModel obj = new GrievanceModel(new FileLogger());
            string WhereClause = "";
            if (ComplainNo != null && ComplainNo != "")
            {
                WhereClause += " and G.ComplainNo=" + ComplainNo;
            }
            else
            {
                if (Distid != "" && Distid != "0")
                {
                    WhereClause += " and ISNULL(DS.DISTRICT_ID,-1)='" + Distid + "'";
                }
                if (Department != "" && Department != "0")
                {
                    WhereClause += " and Module.DeptEn='" + Department + "'";
                }
                if (ModuleId != null && Convert.ToInt32(ModuleId) > 0)
                {
                    WhereClause += " and Module.ModuleId=" + ModuleId;
                }
                if (Status != "")
                {
                    WhereClause += " and G.Status in (" + Status.Substring(0, Status.Length - 1) + ")";
                }
                if ((GrivanceCategoryid != null && Convert.ToInt32(GrivanceCategoryid) > 0))
                {
                    WhereClause += " And GX.GrivencecategoryId=" + GrivanceCategoryid;
                }
                if ((GrivanceSubCategoryid != null && Convert.ToInt32(GrivanceSubCategoryid) > 0))
                {
                    WhereClause += " And GX.GrievanceSubCategoryMasterId=" + GrivanceSubCategoryid;
                }

                WhereClause += " and CONVERT(DATE, G.CreateDate ,103) BETWEEN CONVERT(DATE,'" + FromDate + "',103) AND  CONVERT(DATE,'" + ToDate + "',103)";

            }

            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(obj.GetGrievancetReport(WhereClause));

        }

        [HttpGet]
        public string GetGrievanceTrail(string GrievanceId)
        {
            GrievanceModel obj = new GrievanceModel(new FileLogger());

            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(obj.GetGrievanceTrail(Convert.ToInt32(GrievanceId)));

        }
        [HttpPost]
        [ActionName("GetGrievanceTrailUser")]
        public string GetGrievanceTrailUser(string GrievanceId)
        {
            GrievanceModel obj = new GrievanceModel(new FileLogger());

            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(obj.GetGrievanceTrailUser(Convert.ToInt32(GrievanceId)));

        }
        [HttpPost]
        public string GetGrievanceCount(string FromDate = "", string ToDate = "")
        {
            GrievanceModel obj = new GrievanceModel(new FileLogger());

            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(obj.GetGrievanceCount(FromDate, ToDate));

        }

        public ActionResult SubmitLeave()
        {
            return View();
        }

        [HttpPost]
        [ActionName("SubmitLeaveByIA")]
        public int SubmitLeaveByIA(GrievanceEntity Entity)
        {
            GrievanceModel GrievanceModel = new GrievanceModel(new DBLogger());

            return GrievanceModel.SubmitLeaveByIA(Entity);
        }

        [HttpPost]
        public string GetLeaveList()
        {
            GrievanceModel obj = new GrievanceModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(obj.GetLeaveList());

        }

        public string AttachmentSave()
        {
            string AttachmentName = "";
            try
            {
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    foreach (string files in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[files];
                        var fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "") + Path.GetExtension(file.FileName);
                        var pathString = Server.MapPath("~/ContentNew/Grievance/");
                        bool exists = System.IO.Directory.Exists(pathString);

                        if (!exists)
                            System.IO.Directory.CreateDirectory(pathString);

                        if (System.IO.Directory.Exists(pathString))
                        {
                            file.SaveAs(Path.Combine(pathString, fileName));
                            AttachmentName = fileName.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return AttachmentName;
        }

        [HttpPost]
        public string DistrictState()
        {
            GrievanceModel ObjGrievanceModel = new GrievanceModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(ObjGrievanceModel.DistrictState());
        }
        [HttpPost]
        public string GetAllStatus()
        {
            GrievanceModel ObjGrievanceModel = new GrievanceModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(ObjGrievanceModel.GetAllStatus());
        }
        #region Amit
        public ActionResult GrievanceMaster()
        {
            return View();
        }
        [HttpPost]
        [ActionName("SaveGrivancemasterDepartment")]
        public string SaveGrivancemasterDepartment(GrivanceMaster obj)
        {
            GrievanceModel ObjGrievanceModel = new GrievanceModel(new DBLogger());
            return ObjGrievanceModel.SaveGrivancemasterDepartment(obj).ToString();
        }
        [HttpPost]
        [ActionName("DepartmentList")]
        public string DepartmentList(GrivanceMaster obj)
        {
            GrievanceModel ObjGrievanceModel = new GrievanceModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(ObjGrievanceModel.DepartmentList(obj));
        }
        [HttpPost]
        [ActionName("GetGrivanceMasterList")]
        public string GetGrivanceMasterList(GrivanceMaster obj)
        {
            GrievanceModel ObjGrievanceModel = new GrievanceModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(ObjGrievanceModel.GetGrivanceMasterList(obj));
        }
        [HttpPost]
        [ActionName("ChangeStatus")]
        public JsonResult ChangeStatus(GrivanceMaster obj, int ModuleId, int ActionId)
        {
            GrievanceModel ObjGrievanceModel = new GrievanceModel(new DBLogger());
            ActionId = (ActionId == 0 ? 1 : (ActionId == 1 ? 0 : ActionId));
            return Json(ObjGrievanceModel.ChangeStatus(obj, ModuleId, ActionId), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ActionName("DeleteGrivance")]
        public JsonResult DeleteGrivance(GrivanceMaster obj, int ModuleId, int isDeleted)
        {
            GrievanceModel ObjGrievanceModel = new GrievanceModel(new DBLogger());
            return Json(ObjGrievanceModel.DeleteGrivance(obj, ModuleId, isDeleted), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ActionName("UpdateGrivance")]
        public string UpdateGrivance(GrivanceMaster obj, int ModuleId)
        {
            GrievanceModel ObjGrievanceModel = new GrievanceModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(ObjGrievanceModel.UpdateGrivance(obj, ModuleId));
        }
        public ActionResult IALeaveMaster()
        {
            return View();
        }
        [HttpPost]
        [ActionName("GetIALeaveMaster")]
        public string GetIALeaveMaster(IAGrivanceLeave obj)
        {
            GrievanceModel ObjGrievanceModel = new GrievanceModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(ObjGrievanceModel.GetIALeaveMaster(obj));
        }
        [HttpPost]
        [ActionName("AddLeaveGrievanceIA")]
        public string AddLeaveGrievanceIA(IAGrivanceLeave obj, string SSOID, int RoleId, int OfficeId)
        {
            GrievanceModel ObjGrievanceModel = new GrievanceModel(new DBLogger());
            return ObjGrievanceModel.AddLeaveGrievanceIA(obj, SSOID, RoleId, OfficeId).ToString();
        }
        #endregion

        #region Mahesh 10012024
        public ActionResult GrievanceOICDetails()
        {
            if (_LoggedInUser != null)
            {
                ViewBag.hdnUserId = Convert.ToString(_LoggedInUser.UserId);
                return View();
            }
            else
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }

        }

        public string GetDepartmentListForOIC()
        {
            Int32 Userid = Convert.ToInt32(Request["UserID"].ToString());
            GrievanceModel ObjGrievanceModel = new GrievanceModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(ObjGrievanceModel.GetDepartmentListForOIC(Userid));
        }

        [HttpPost]
        [ActionName("GetGrievancetReportForOIC")]
        public string GetGrievancetReportForOIC(string Distid, string Department, string ModuleId, string FromDate, string ToDate, string ComplainNo = "", string Status = "")
        {
            GrievanceModel obj = new GrievanceModel(new FileLogger());
            string WhereClause = "";
            if (ComplainNo != null && ComplainNo != "")
            {
                WhereClause += " and G.ComplainNo=" + ComplainNo;
            }
            else
            {
                if (Distid != "" && Distid != "0")
                {
                    WhereClause += " and ISNULL(DS.DISTRICT_ID,-1)='" + Distid + "'";
                }
                if (Department != "" && Department != "0")
                {
                    WhereClause += " and Module.DeptEn='" + Department + "'";
                }
                if (ModuleId != null && Convert.ToInt32(ModuleId) > 0)
                {
                    WhereClause += " and Module.ModuleId=" + ModuleId;
                }
                if (Status != "")
                {
                    WhereClause += " and G.Status in (" + Status.Substring(0, Status.Length - 1) + ")";
                }

                //WhereClause += " and CONVERT(DATE, G.CreateDate ,103) BETWEEN CONVERT(DATE,'" + FromDate + "',103) AND  CONVERT(DATE,'" + ToDate + "',103)";

            }

            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(obj.GetGrievancetReportForOIC(WhereClause));

        }


        [HttpPost]
        [ActionName("UpdateRemarkByOIC")]
        public string UpdateRemarkByOIC()
        {
            GrievanceModel obj = new GrievanceModel(new FileLogger());
            string remark_OIC = Request.QueryString["Remark"].ToString();
            String Grievance_id = Request.QueryString["GrievanceID"].ToString();
            string pathString = "~/ContentNew/Grievance";
            string filepath = System.Web.HttpContext.Current.Server.MapPath(pathString);
            bool exists = System.IO.Directory.Exists(filepath);
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(filepath);
            }
            string fileName = "";
            foreach (string files in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[files];
                fileName = "OICAttachment_" + Grievance_id + Path.GetExtension(file.FileName);
                if (System.IO.Directory.Exists(Server.MapPath(pathString)))
                {
                    file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));
                }
            }
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(obj.UpdateRemarkByOIC(remark_OIC, Grievance_id, fileName));

        }
        #endregion



        [HttpPost]
        [ActionName("GetGrievanceCategoryId")]
        public string GetGrievanceCategoryIds(string ListCode)
        {
            GrievanceModel ObjGrievanceModel = new GrievanceModel(new DBLogger());

            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(ObjGrievanceModel.GetGrievancelCategoryId(ListCode));
        }

        [HttpPost]
        [ActionName("GetGrievanceModuleId")]
        public string GetGrievanceModuleId(string ListCode)
        {
            GrievanceModel ObjGrievanceModel = new GrievanceModel(new DBLogger());

            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(ObjGrievanceModel.GetGrievancelModuleId(ListCode));
        }


        [HttpGet]
        [ActionName("GetGrievanceSectionByModuleId")]
        public string GetGrievanceSectionByModuleId(int ModuleId)
        {
            GrievanceModel obj = new GrievanceModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(obj.GetGrievanceSectionByModuleId(ModuleId));
        }

        [HttpPost]
        public int SaveGrievanceDepartmentForm(GrievanceDepartmentEntity _Entity)
        {
            GrievanceModel obj = new GrievanceModel(new DBLogger());          

            int GrievanceDepartmentId = obj.SaveDepartmentGrievance(_Entity);        

            return GrievanceDepartmentId;
        }


        [HttpGet]
        [ActionName("GetMobileNoBySSOID")]
        public string GetMobileNoBySSOID(string SSOID)
        {
            GrievanceModel obj = new GrievanceModel(new DBLogger());

            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(obj.GetMobileNoBySSOID(SSOID));
        }


        [HttpPost]
        public JsonResult UploadGrievanceDocument(HttpPostedFileBase Attachment)
        {
            try
            {
                if (Attachment == null)
                    return Json(new { success = false, message = "File not received" });

                string folder = Server.MapPath("~/Content/Grievance/Document/");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(Attachment.FileName);
                string filePath = Path.Combine(folder, fileName);

                Attachment.SaveAs(filePath);

                return Json(new { success = true, fileName = fileName });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public string GetGrievanceList()
        {
            GrievanceModel obj = new GrievanceModel(new DBLogger());

            DataTable dt = obj.GetGrevianceList();

            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

    }
}