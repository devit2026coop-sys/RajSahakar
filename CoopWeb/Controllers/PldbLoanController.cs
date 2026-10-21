using Cooperative.Filters;
using Cooperative.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cooperative.Controllers
{
    [Error]
    [CustomAuthentication]
    [LogActionFilter]
    public class PldbLoanController : BaseController
    {
        // GET: PldbLoan
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoanApplication()
        {
            return View();
        }


        public ActionResult LoanApplicationForm()
        {
            
            return View();
        }

        public ActionResult LoanApplicationList()
        {
            return View();
        }


        public ActionResult LoanValuation()
        {
            return View();
        }

        public ActionResult LoanApproval()
        {
            return View();
        }


        [HttpGet]
        public string GetLovMaster(int Id)
        {

            DataTable DT = new SocietyModel().GetLovValuesByListId(Id);

            COMMON_FUNCTION.DataTableToJSONString(DT);

            return COMMON_FUNCTION.DataTableToJSONString(DT); ;
        }

        [HttpGet]
        public JsonResult GetMemberDetails(string MembershipNo, int DistrictId, int BranchId)
        {
            string WhereClause = "";
            DataTable DT = new DataTable();
            PLDBLoan objModel = new PLDBLoan();
            if (!string.IsNullOrWhiteSpace(MembershipNo))
            {
                DT = objModel.GetPLDBMemberDetails(MembershipNo, DistrictId, BranchId);
            }
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
            //return COMMON_FUNCTION.DataTableToJSONString(DT);

        }


        [HttpPost]
        public JsonResult SaveLoanApplication()
        {
            string result = "";
            PLDPLoanApplication Applicationdata = new PLDPLoanApplication();
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
                Applicationdata = Request.Form["Applicationdata"].ToString().Equals("[]") ? new PLDPLoanApplication() : (PLDPLoanApplication)s.Deserialize(Request.Form["Applicationdata"].ToString(), typeof(PLDPLoanApplication));
            }
            catch (System.Exception ex)
            {
                result = "-1";
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.PLDBLoan", FunctionName = "public JsonResult SaveLoanApplication()" }, ex);
            }

            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    string pathString = "";

                    pathString = "~/Content/PLDBLoanApplication";

                    if (!System.IO.Directory.Exists(pathString))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(pathString));
                    }


                    foreach (string files in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[files];
                        fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                        if (System.IO.Directory.Exists(Server.MapPath(pathString)))
                        {
                            file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));
                        }

                        if (files.ToString().Contains("MemberPhoto"))
                        {
                            string strid = files.ToString().Replace("MemberPhoto", "");
                            if (strid != "")
                            {
                                int idx = Convert.ToInt32(files.Split('_')[1]);
                                Applicationdata.Members[idx].Photo = pathString.Replace("~", "..") + "/" + fileName;

                            }
                        }

                    }

                }
                catch (System.Exception ex)
                {
                    result = "-1";
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.PLDBLoan", FunctionName = "public JsonResult SaveLoanApplication()" }, ex);
                }
            }




            PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            result = objLoan.InsertUpdatePLDBLoanApplication(Applicationdata);
            return Json(new { Result = result }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult UpdateLoanStatus(PLDBLoanMaster Details,string Level, List<PLDBLVODocuments> LVODocuments)
        {
            string result = ""; 

            string fileName = string.Empty;      
            PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            result = "";//objLoan.UpdateLoanStatus(Details, Level,  LVODocuments);
            return Json(new { Result = result }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult GetLoanApplicationList(DataPagingModel Details,string Level)
        {
            PLDBLoan objLoan = new PLDBLoan(new DBLogger());

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser == null)
            {
                return Json(new { DataList = "LOGOUT", DataPaging = Details }, JsonRequestBehavior.AllowGet);
            }
            DataTable DT = objLoan.GetLoanApplicationList(ref Details, _LoggedInUser.SSOID,  Level);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetLoanApplicationDetails(string Table, int LoanId)
        {
            PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            DataTable DT = objLoan.GetLoanApplicationDetails(Table, LoanId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetLoanProcessStatus()
        {
            PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            DataTable DT = objLoan.GetLoanProcessStatus(1,1);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult PLDBDashboard()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetPLDBDashboard()
        {

            PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            DataSet ds = objLoan.GetPLDBDashboard();
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(ds) }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult LoanProcessBranch()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetLoanApplicationBranchList(DataPagingModel Details)
        {
            PLDBLoan objLoan = new PLDBLoan(new DBLogger());

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser == null)
            {
                return Json(new { DataList = "LOGOUT", DataPaging = Details }, JsonRequestBehavior.AllowGet);
            }
            DataTable DT = objLoan.GetLoanApplicationList(ref Details, _LoggedInUser.SSOID, "2");
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }
                
    }
}