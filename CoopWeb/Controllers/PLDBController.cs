using CoopDB;
using Cooperative.Filters;
using Cooperative.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using java.util;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Cms;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using sun.reflect.generics.scope;
using sun.swing;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using static Cooperative.Controllers.COMMON_FUNCTION;
using static Cooperative.Controllers.GopalCreditCardController;
using static Cooperative.Controllers.ServicesController;
using static Cooperative.Models.GopalCreditCardModel;

namespace Cooperative.Controllers
{
    //[Error]
    [CustomAuthentication]
    [LogActionFilter]
    public class PLDBController : BaseController
    {
        // GET: PldbLoan
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OTSApplicationList()
        {
            return View();
        }
        public ActionResult ots_pr_receipt()
        {
            return View();
        }
        public ActionResult ots_fr_receipt()
        {
            return View();
        }
        public ActionResult PldbFinalLoanPayment()
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

        public ActionResult PldbTaamilNew()
        {
            return View();
        }

        public ActionResult PldbTaamil()
        {
            return View();
        }

        public ActionResult PldbOtsPRDetails()
        {
            return View();
        }


        public ActionResult PLDBSummaryODApplicationList()
        {
            return View();
        }


        public ActionResult PLDBSummaryInterestSubventionODCasesList()
        {
            return View();
        }

        public ActionResult PLDBSummaryODApplicationForm()
        {
            return View();
        }

        //Jaya 

        #region PLDB Loan Application Form

        public ActionResult PldbLoanApplicationDetailsList()
        {
            return View();
        }

        public ActionResult PldbLoanApplicationLoanDisbursementList()
        {
            return View();
        }

        public ActionResult PLDBLoanApplicationForm()
        {
            return View();
        }

        #endregion PLDB Loan Application Form


        #region PLDB Loan

        public ActionResult LoanApplication()
        {
            return View();
        }
        public ActionResult OTSApplicationForm(int sid = 0)
        {
            return View();
        }
        public ActionResult LoanApplicationFormV1(int sid = 0)
        {
            // ViewBag.schemeid = sid;
            return View();
        }

        public ActionResult LoanApplicationForm(int sid = 0)
        {
            ViewBag.schemeid = sid;
            return View();
        }

        public ActionResult LoanApplicationFormReport(int sid = 0)
        {
            ViewBag.schemeid = sid;
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

        #region PLDB SSI Loan Application Form

        public ActionResult SSILoanApplicationForm()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveSSILoanApplication()
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
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.PLDB", FunctionName = "SaveSSILoanApplication()" }, ex);
            }


            PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            result = objLoan.InsertUpdatePLDBLoanApplication(Applicationdata);
            if (result != "-1" && Applicationdata.IsSubmit)
            {
                string message = "Your PLDB Loan application has been submitted successfully. Application No : " + result;
                foreach (var mbl in Applicationdata.Members)
                {
                    new EsanchaarController().SendSocietySMS(message, mbl.MobileNo);
                }
            }
            return Json(new { Result = result }, JsonRequestBehavior.AllowGet);
        }

        #endregion


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
            MembershipNo = csCommon.Decrypt_SA(Uri.UnescapeDataString(MembershipNo));
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
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.PLDB", FunctionName = "public JsonResult SaveLoanApplication()" }, ex);
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
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.PLDB", FunctionName = "public JsonResult SaveLoanApplication()" }, ex);
                }
            }




            PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            result = objLoan.InsertUpdatePLDBLoanApplication(Applicationdata);
            if (result != "-1" && Applicationdata.IsSubmit)
            {
                string message = "Your PLDB Loan application has been submitted successfully. Application No : " + result;
                foreach (var mbl in Applicationdata.Members)
                {
                    new EsanchaarController().SendSocietySMS(message, mbl.TelephoneResidence);
                }
            }


            return Json(new { Result = result }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult UpdateLoanStatus(PLDBLoanMaster Details, string Level, List<PLDBLVODocuments> LVODocuments, List<PLDBLandAmount> PldbLandAmount)
        {
            string result = "";


            string fileName = string.Empty;
            PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            result = objLoan.UpdateLoanStatus(Details, Level, LVODocuments, PldbLandAmount);
            if (result != "-1")
            {
                string message = "Your PLDB loan " + Details.NameHi + " status is " + Details.NameEn.ToUpper();
                foreach (string mbl in Details.Mobile.Split('#'))
                {
                    new EsanchaarController().SendSocietySMS(message, mbl);
                }

            }
            return Json(new { Result = result }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult GetLoanApplicationList(DataPagingModel Details, string Level)
        {
            PLDBLoan objLoan = new PLDBLoan(new DBLogger());

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser == null)
            {
                return Json(new { DataList = "LOGOUT", DataPaging = Details }, JsonRequestBehavior.AllowGet);
            }
            DataTable DT = objLoan.GetLoanApplicationList(ref Details, _LoggedInUser.SSOID, Level);
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
        public JsonResult GetLoanProcessStatus(int ActionId)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            DataTable DT = objLoan.GetLoanProcessStatus(_LoggedInUser.LevelId, ActionId);
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




        #region PLDB Masters


        #region Scheme Masters

        public ActionResult SchemeMaster()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SchemeMaster(PLDBLoanMaster Details)
        {
            PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            string result = objLoan.InsertUpdateSchemeMaster(Details);
            return Json(new { Status = result }, JsonRequestBehavior.AllowGet);
        }

        public string GetSchemeMaster(string WhereCase = "", int SchemeId = 0, int SectorId = 0)
        {

            string whereClause = "";
            if (SchemeId > 0)
            {
                whereClause += "and PLS.scheme_id = '" + SchemeId + "'";
            }

            if (SectorId > 0)
            {
                whereClause += "and PLS.Sector_Id = '" + SectorId + "'";
            }

            if (WhereCase == "Farming" || WhereCase == "NonFarming")
            {

                whereClause += "and se.Farming_NonFarming = '" + WhereCase + "'";

            }



            PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            DataTable DT = objLoan.GetSchemeMaster(whereClause);
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        }

        [HttpPost]
        [ActionName("Deletescheme")]
        public JsonResult Deletescheme(int id, int Action)
        {
            Action = (Convert.ToInt32(Action));
            Action = (Action == 0 || Action == 1) ? (Action == 0 ? 1 : 0) : Action;

            PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            return Json(objLoan.DeletePLDBSchemerecord(id, Action), JsonRequestBehavior.AllowGet);
        }

        //public ActionResult SchemeMaster()
        //{
        //    PLDBLoanMaster Details = new PLDBLoanMaster();
        //    PLDBLoan objLoan = new PLDBLoan(new DBLogger());
        //    string result = objLoan.InsertUpdateSchemeMaster(Details);
        //    return Json(new { Status = result }, JsonRequestBehavior.AllowGet);

        //}

        #endregion Scheme Masters


        #region Sector Master

        public ActionResult SectorMaster()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SectorMaster(PLDBLoanMaster Details)
        {
            PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            string result = objLoan.InsertUpdateSectorMaster(Details);
            return Json(new { Status = result }, JsonRequestBehavior.AllowGet);
        }

        public string GetSectorMaster(string WhereCase = "")
        {
            string WhereClause = "";
            if (WhereCase != "")
            {
                WhereClause = " and se.is_active in (1,0) and Farming_NonFarming='" + WhereCase + "'";
            }

            PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            DataTable DT = objLoan.GetSectorMaster(WhereClause);
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        }

        [HttpPost]
        [ActionName("DeleteandstatusSector")]
        public JsonResult delete_record(int id = 0, int Action = 0)
        {
            Action = (Convert.ToInt32(Action));
            Action = (Action == 0 || Action == 1) ? (Action == 0 ? 1 : 0) : Action;

            PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            return Json(objLoan.DeletePLDBSectorrecord(id, Action), JsonRequestBehavior.AllowGet);
        }

        //public string GetSectorMaster(string WhereCase = "")
        //{
        //    PLDBLoan objLoan = new PLDBLoan(new DBLogger());
        //    DataTable DT = objLoan.GetSectorMaster(WhereCase);
        //    return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        //}

        #endregion Sector Master


        #region Purpose Master

        public ActionResult PurposeMaster()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PurposeMaster(PLDBLoanMaster Details)
        {
            PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            string result = objLoan.InsertUpdatePurposeMaster(Details);
            return Json(new { Status = result }, JsonRequestBehavior.AllowGet);
        }

        public string GetPurposeMaster(string WhereCase = "", int SchemeId = 0)
        {
            string WhereClause = "";


            if (SchemeId > 0)
            {
                WhereClause += "and pr.scheme_id = '" + SchemeId + "'";
            }

            if (WhereCase != "")
            {
                WhereClause += "  and se.is_active=1 and purpose_id = " + WhereCase;
            }
            PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            DataTable DT = objLoan.GetPurposeMaster(WhereClause);
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        }


        [HttpPost]
        [ActionName("ChangedeleteStatusPurposeMaster")]
        public JsonResult DeletePurpose(int Id, int ActionId)
        {
            ActionId = (ActionId == 0 ? 1 : (ActionId == 1 ? 0 : ActionId));

            PLDBLoan PLDBPurposeMasterModel = new PLDBLoan(new FileLogger());
            return Json(PLDBPurposeMasterModel.DeletePurpose(Id, ActionId), JsonRequestBehavior.AllowGet);
        }

        //public string GetPurposeMaster(string WhereCase = "")
        //{
        //    PLDBLoan objLoan = new PLDBLoan(new DBLogger());
        //    DataTable DT = objLoan.GetPurposeMaster(WhereCase);
        //    return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        //}

        //public string GetPurposeMasterSchemeMasterList(string WhereCase = "")
        //{
        //    PLDBLoan objLoan = new PLDBLoan(new DBLogger());
        //    DataTable DT = objLoan.GetPurposeMaster(WhereCase);
        //    return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        //}

        #endregion Purpose Master

        #endregion


        #region MemberShip

        #endregion

        public ActionResult BIOKYC()
        {
            if (Request.Form["Data"] != null) { Sp_callTrackingInsert("GetTxnIdOtp  Request.Form[\"data\"] :-", Request.Form["Data"].ToString()); }


            if (Request.Form["Data"] != null && Request.Form["Data"] != "")
            {
                // ViewData["TxnIdDecData"] = Request.Form["Data"];
                System.Web.HttpContext.Current.Session["TxnIdDecData"] = Request.Form["Data"];
                Sp_callTrackingInsert("GetTxnIdOtp  session.Form[\"data\"] :-", System.Web.HttpContext.Current.Session["TxnIdDecData"].ToString());
            }
            else
            {
                ViewData["TxnIdDecData"] = "NO" + "#";
            }

            return View();
        }




        public ActionResult TestEsignService()
        {
            return View();
        }

        public ActionResult PLDBschemeList()
        {
            return View();
        }

        // [HttpPost]
        // [ActionName("VerifyBIOKYC")]
        public JsonResult VerifyBIOKYC(string AadharNo)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser != null)
            {
                //SSOWSSoap.SSOUserDetail usrdtl = new SSOWSSoap.SSOUserDetail();
                //SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
                //usrdtl = ssoclient.GetUserDetailXML(_LoggedInUser.SSOID, "RAJSAHKAR.TEST", "Rajasthan@12");

                //// return COMMON_FUNCTION.GenerateOTPForEsingh("940396382715");
                //string aadhaarId = usrdtl.aadhaarId;
                //if (Request.Url.Host.Contains("localhost"))
                //{
                //    aadhaarId = "940396382715";


                //    //return COMMON_FUNCTION.GenerateOTPForEsingh("940396382715");
                //}
                //else
                //{
                //    //return COMMON_FUNCTION.GenerateOTPForEsingh(usrdtl.aadhaarId);
                //}



                KYCResponce result = AADHARSerice.BIO_KYC(AadharNo);

                return Json(new { Status = result.Status, UserDetails = result.UserDetails }, JsonRequestBehavior.AllowGet);


            }
            else
            {
                return Json(new { Status = "0" }, JsonRequestBehavior.AllowGet);
            }
            //return "RJVSD2343246566";
        }

        [HttpPost]
        public JsonResult GetApplicationDetails(string LoanId)
        {
            PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            DataSet DT = objLoan.GetApplicationDetails(LoanId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }

        #region Vehicle Loan ApplicationForm
        public ActionResult VehicleLoanApplication()
        {
            return View();
        }


        [HttpPost]
        public string UploadLoanFile()
        {
            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {

                    string fileDirectory = Server.MapPath("~/Content/PLDB");

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

        public bool SaveImageAsPDF(Stream FileStrime, string DirPath)
        {

            //int width = 600;
            using (var document = new PdfSharp.Pdf.PdfDocument())
            {
                PdfSharp.Pdf.PdfPage page = document.AddPage();
                System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)System.Drawing.Image.FromStream(FileStrime, true, true);
                XImage image = XImage.FromGdiPlusImage(bmp);
                // Calculate new height to keep image ratio
                var height = (int)(((double)image.PixelWidth / (double)image.PixelWidth) * image.PixelHeight);
                // Change PDF Page size to match image
                page.Width = image.PixelWidth;
                page.Height = image.PixelHeight;
                XGraphics gfx = XGraphics.FromPdfPage(page);
                gfx.DrawImage(image, 0, 0, page.Width, height);
                MemoryStream stream = new MemoryStream();
                document.Save(DirPath);
                return true;
            }
        }

        #endregion

        #region Personal Vehicle Loan Application
        public ActionResult PersonalVehicleLoanApplication()
        {
            return View();
        }
        #endregion
        [HttpPost]
        public string SchemebyTypeList(int id)
        {
            PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            DataTable DT = objLoan.GetSchemebyType(id);
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        }


        public string PurposebyTypeList(int id)
        {
            PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            DataTable DT = objLoan.GetPurposebyType(id);
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        }


        #endregion


        public JsonResult LoadMaster()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            string PLDB = (new MasterController()).GetMasterDataDetails("PLDB", "-1", "");
            string CATEGORY = (new MasterController()).GetMasterDataDetails("CATEGORY", "-1", "");

            string FarmerCategory = COMMON_FUNCTION.DataTableToJSONString(new SocietyModel().GetLovValuesByListId(5));
            string LoanTypeList = COMMON_FUNCTION.DataTableToJSONString(new SocietyModel().GetLovValuesByListId(7));
            string BANKLIST = (new MasterController()).GetMasterDataDetails("BANKLIST", "-1", "");

            string ProcessStatus = COMMON_FUNCTION.DataTableToJSONString((new PLDBLoan()).GetLoanProcessStatus(_LoggedInUser.LevelId, 1));

            string Master = "{\"PLDB\":" + PLDB + ",\"CATEGORY\":" + CATEGORY + ",\"FarmerCategory\":" + FarmerCategory + ",\"LoanTypeList\":" + LoanTypeList + ",\"BANKLIST\":" + BANKLIST + ",\"ProcessStatus\":" + ProcessStatus + "}";
            return Json(new { DataList = Master }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddBankDetails()
        {
            return View();
        }
        public ActionResult PLDBDetailsReport()
        {
            return View();
        }
        public ActionResult PldbPrLoanPayment()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetOtsApplicantDetails_old(string otsIdEnc, int summaryId, int type = 0, int isAccountCloseInSinglePayment = 0, string accountCloseDate = null)
        {
            var responce = new APIResponce();
            var pLDBLoan = new PLDBLoan(new FileLogger());
            var ds = pLDBLoan.GetOtsApplicantDetails(otsIdEnc, summaryId, type, isAccountCloseInSinglePayment, accountCloseDate);
            if (ds != null)
            {
                if (type == 0)
                {
                    responce.Data = new
                    {
                        ApplicantDetail = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                        PaymentSummary = ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[1]) : null,
                        QrData = COMMON_FUNCTION.GenerateQRCode(ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0 ? ds.Tables[2].Rows[0][0].ToString() : "Not available", 10)
                    };
                }
                else
                {
                    responce.Data = new
                    {
                        ApplicantDetail = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                        PaymentSummary = ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[1]) : null,
                        FinalPaymentSummaryList = ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[2]) : null,
                        QrData = COMMON_FUNCTION.GenerateQRCode(ds.Tables.Count > 3 && ds.Tables[3].Rows.Count > 0 ? ds.Tables[3].Rows[0][0].ToString() : "Not available", 10)
                    };
                }

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
        public JsonResult GetOtsApplicantDetails(string otsIdEnc, int summaryId, int type = 0, int isAccountCloseInSinglePayment = 0, string accountCloseDate = null, String NoticeNo = "")
        {
            var responce = new APIResponce();
            var pLDBLoan = new PLDBLoan(new FileLogger());
            var ds = pLDBLoan.GetOtsApplicantDetails(otsIdEnc, summaryId, type, isAccountCloseInSinglePayment, accountCloseDate, NoticeNo);
            if (ds != null)
            {
                if (type == 0)
                {
                    responce.Data = new
                    {
                        ApplicantDetail = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                        PaymentSummary = ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[1]) : null,
                        QrData = COMMON_FUNCTION.GenerateQRCode(ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0 ? ds.Tables[2].Rows[0][0].ToString() : "Not available", 10)
                    };
                }
                else
                {
                    responce.Data = new
                    {
                        ApplicantDetail = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                        PaymentSummary = ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[1]) : null,
                        FinalPaymentSummaryList = ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[2]) : null,
                        QrData = COMMON_FUNCTION.GenerateQRCode(ds.Tables.Count > 3 && ds.Tables[3].Rows.Count > 0 ? ds.Tables[3].Rows[0][0].ToString() : "Not available", 10)
                    };
                }

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

        /// <summary>
        /// Creaed by #Pradeep 29-04-2025
        /// To search Loanee application getails
        /// </summary>
        /// <param name="applicantDetails"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SearchOtsCitizenApplicantDetails(OtsCitizenApplicantDetails applicantDetails)
        {
            var responce = new APIResponce();
            var pLDBLoan = new PLDBLoan(new FileLogger());
            var ds = pLDBLoan.SearchOtsCitizenApplicantDetails(applicantDetails);
            if (ds != null)
            {
                responce.Data = new
                {
                    ApplicantDetail = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null
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

        /// <summary>
        /// Creaed by #Pradeep 09-05-2025
        /// To get Loanee application getails
        /// </summary>
        /// <param name="applicantDetails"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetOtsCitizenApplicantDetails(string EncPldbOtsSchemeId)
        {
            var responce = new APIResponce();
            var pLDBLoan = new PLDBLoan(new FileLogger());
            var ds = pLDBLoan.GetOtsCitizenApplicantDetails(EncPldbOtsSchemeId);
            if (ds != null)
            {
                responce.Data = new
                {
                    ApplicantDetail = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                    PaymentSummary = ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[1]) : null,
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

        public void SendPldbSms(PldbSmsParam pldbSmsParam)
        {
            GopalCreditCardDBModel _Modal = new GopalCreditCardDBModel();
            try
            {
                if (pldbSmsParam.MobileNo.Length == 10)
                {
                    DataTable MessageTable = new DataTable();


                    if (Request.Url.Host.Contains("localhost") || Request.Url.Host.Contains("devitsandbox") || Request.Url.Host.Contains("Devitsandbox"))
                    {
                        pldbSmsParam.MobileNo = "7568622727";
                    }

                    MessageTable = MessageOnActivity.GetMessageBodyActivityKey(pldbSmsParam.TemplateKey);

                    if (MessageTable != null && MessageTable.Rows.Count > 0)
                    {
                        string Message = Convert.ToString(MessageTable.Rows[0]["SMS_MSG"]);
                        string templateID = Convert.ToString(MessageTable.Rows[0]["TemplateID"]);

                        if (!string.IsNullOrEmpty(Message))
                        {
                            Message = Message.Replace("#LoanAccNo#", Convert.ToString(pldbSmsParam.AccNo));
                            Message = Message.Replace("#PaidAmount#", Convert.ToString(pldbSmsParam.Amount));
                            Message = Message.Replace("#PaymentDate#", Convert.ToString(pldbSmsParam.Date));
                            Message = Message.Replace("#DistrictName#", Convert.ToString(pldbSmsParam.DistrictName));
                            Message = Message.Replace("#PldbName#", Convert.ToString(pldbSmsParam.PldbName));
                            Message = Message.Replace("#BranchName#", Convert.ToString(pldbSmsParam.BranchName));
                            Message = Message.Replace("#LoaneeName#", Convert.ToString(pldbSmsParam.LoaneeName));
                            Message = Message.Replace("#ReceiptNo#", Convert.ToString(pldbSmsParam.ReceiptNo));

                            try
                            {
                                new EsanchaarController().SendSocietySMS(Message, pldbSmsParam.MobileNo, "HIN", templateID);
                                //new EsanchaarController().SendSocietySMS(Message, "7568622727", "HIN", templateID);//for testing
                            }
                            catch (Exception ex)
                            {
                                ILogger objILogger = new DBLogger();
                                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "PLDBController", FunctionName = "Message SendSms(int statusId, string MobileNo = \"\", string AccNo = \"\" ,string Amt = \"\", string Date = \"\")" }, ex);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "GopalCreditCardController", FunctionName = "SendSms(int statusId, string MobileNo = \"\", string RegistrationNO = \"\", int gccId = 0) " }, ex);
            }

        }




        [HttpPost]
        public JsonResult SaveRepaymentSummary(OtsRepaymentSummary otsRepayment)
        {
            var responce = new APIResponce();
            if (otsRepayment.PaidAmount <= 0)
            {
                responce.Status = "200";
                responce.Message = "The amount paid must be 1 or more.";
                return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
            }
            var pLDBLoan = new PLDBLoan();
            var result = pLDBLoan.SaveRepaymentSummary(otsRepayment);
            if (result > 0)
            {

                var ds = pLDBLoan.GetOtsApplicantDetails(otsRepayment.OtsIdEnc, result, 0);
                if (ds != null)
                {
                    var ApplicantDetail = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null;
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["MobileNo"] != "")
                        {
                            DateTime today = DateTime.Now;
                            string formattedDate = Convert.ToDateTime(otsRepayment.PaymentDate).ToString("dd-MM-yyyy");
                            PldbSmsParam pldbSmsParam = new PldbSmsParam();
                            pldbSmsParam.MobileNo = ds.Tables[0].Rows[0]["MobileNo"].ToString();
                            pldbSmsParam.AccNo = ds.Tables[0].Rows[0]["LoanAccNo"].ToString();
                            pldbSmsParam.ReceiptNo = ds.Tables[0].Rows[0]["CurrentReceiptNo"].ToString();
                            if (otsRepayment.IsFinalPayment)
                                pldbSmsParam.Amount = (otsRepayment.PaidAmount + otsRepayment.InterestAmount + otsRepayment.PenalInterestAmount + otsRepayment.PrincipalAmount + otsRepayment.OtherAmount).ToString();
                            else
                                pldbSmsParam.Amount = otsRepayment.PaidAmount.ToString();
                            pldbSmsParam.Date = formattedDate;
                            pldbSmsParam.BranchName = ds.Tables[0].Rows[0]["PLDBBranch"].ToString();
                            pldbSmsParam.DistrictName = ds.Tables[0].Rows[0]["DistrictName"].ToString();
                            pldbSmsParam.TemplateKey = "OTS_PaymentR";
                            SendPldbSms(pldbSmsParam);
                        }
                    }
                }

                responce.Status = "200";
                responce.Message = "Success";
            }
            else if (result == -1)
            {
                responce.Status = "200";
                responce.Message = "Loanee are not eligible to make payments.";
            }
            else if (result == -2)
            {
                responce.Status = "200";
                responce.Message = "Loanee are not eligible to make payments in back date.";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveFinalPaymentSummary(OtsFinalPaymentSummary otsFinalpayment)
        {
            var responce = new APIResponce();
            var pLDBLoan = new PLDBLoan();
            var result = pLDBLoan.SaveFinalPaymentSummary(otsFinalpayment);
            if (result > 0)
            {
                responce.Status = "200";
                responce.Message = "Success";
            }
            else if (result == -2)
            {
                responce.Status = "200";
                responce.Message = "Duplicate";

            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }



        public string OTSCertificateGenerated(PLDB_OTSSchemeDataList _Model = null, string PdfName = "ots_certificate", string TXN = "")
        {


            try
            {
                string wkhtmlPath = Server.MapPath("~/bin/wkhtmltopdf.exe");
                string fontPath = new Uri(Server.MapPath("~/fonts/lohit-Devanagari.ttf")).AbsoluteUri;
                string iycLogo = new Uri(Server.MapPath("~/Content/images/iyc_logo.png")).AbsoluteUri;
                string cmPhoto = new Uri(Server.MapPath("~/Content/images/cm-ots-logo.png")).AbsoluteUri;

                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {

                    StringBuilder strbld = new StringBuilder();
                    foreach (var app in _Model.PLDB_OTSSchemeData)
                    {
                        var file = "";
                        if (app.NODPrincipal > 0 || app.NODInterest > 0 || app.NODPenalInterest > 0)
                            file = System.IO.Path.Combine(Server.MapPath("~/Template"), "ots_notice.html");
                        else
                            file = System.IO.Path.Combine(Server.MapPath("~/Template"), "ots_notice_103.html");


                        if (PdfName == "ots_certificate")
                        {
                            file = System.IO.Path.Combine(Server.MapPath("~/Template"), "ots_certificate.html");
                        }

                        // ots_notice_103


                        //var parts = new[] { app.VName, app.GpName, app.BlockName, app.TehsilName, app.VidhanSabha, app.DistrictName };
                        var parts = new[] { app.VName, app.GpName, app.BlockName, app.DistrictName };
                        var address = string.Join(", ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));

                        strbld.Append(System.IO.File.ReadAllText(file));
                        strbld.Replace("#PLDB_Name#", app.PLDBNameHi)
                       .Replace("#LoaneeName#", app.ApplicantName)
                       .Replace("#LoaneeAddress#", address)
                       .Replace("#DistrictName#", app.DistrictName)
                       .Replace("#LoanAccNo#", app.LoanAccNo)
                       .Replace("#LoaneeFatherName#", app.FatherHusbandName);


                        if (file.Contains("ots_notice.html"))
                        {
                            strbld.Replace("#ODPrincipal_InsurancePremium#", (app.ODPrincipal + app.ODInsurancePremium).ToString())  //1=32+33
                          .Replace("#ODInterest#", app.ODInterest.ToString())                   //2 =34
                          .Replace("#ODPenalInterast#", app.ODPenalInterast.ToString())         //3 =35
                          .Replace("#ODTotal#", (app.ODPrincipal + app.ODInsurancePremium + app.ODInterest + app.ODPenalInterast).ToString())                         //4 =36

                           .Replace("#NODPrincipal_InsurancePremium#", (app.NODPrincipal + app.NODInsurancePremium).ToString())      //5 =37+38
                          .Replace("#NODInterest#", app.NODInterest.ToString())         //6 =39
                          .Replace("#NODPenalInterast#", app.NODPenalInterest.ToString())   //7 =40
                          .Replace("#NODTotal#", (app.NODPrincipal + app.NODInsurancePremium + app.NODInterest + app.NODPenalInterest).ToString())   //8=5+6+7

                          .Replace("#CDPrincipal#", app.CDPrincipal.ToString()) //9=42
                          .Replace("#CDInterest#", app.CDInterest.ToString()) //10=43
                          .Replace("#CDPenalInterast#", app.CDPenalInterest.ToString()) //11=44
                          .Replace("#CDTotal#", (app.CDPrincipal + app.CDInterest + app.CDPenalInterest).ToString()) //12 =(9+10+11)


                          .Replace("#Principal#", (app.NODPrincipal + app.NODInsurancePremium + app.CDPrincipal).ToString()) //13=5+9
                          .Replace("#CDInterest_PenalInt#", (app.CDInterest + app.CDPenalInterest).ToString())   //14=10+11
                          .Replace("#Total#", (app.NODPrincipal + app.NODInsurancePremium + app.CDPrincipal + app.CDInterest + app.CDPenalInterest).ToString()) //15=13+14

                          .Replace("#Interest#", app.NODInterest.ToString())  //16=6
                          .Replace("#PenalInterest#", app.NODPenalInterest.ToString()) //17=7
                          .Replace("#TotalInterest#", (app.NODInterest + app.NODPenalInterest).ToString());//18=16+17

                        }
                        //103
                        if (file.Contains("ots_notice_103.html"))
                        {
                            strbld.Append(System.IO.File.ReadAllText(file));
                            strbld.Replace("#PrincipalOutstandingAsOnDateSettlement#", app.PrincipalOutstandingAsOnDateSettlement.ToString())  //1
                           .Replace("#TotalInterestAsOnDateSettlement#", app.TotalInterestAsOnDateSettlement.ToString()) //2
                           .Replace("#InterestFromSettlementPre#", app.InterestFromSettlementPre.ToString())//3
                           .Replace("#InterestFromSettlement#", app.InterestFromSettlement.ToString())//4
                           .Replace("#TotalInterestFromSettlement#", (app.PrincipalOutstandingAsOnDateSettlement + app.TotalInterestAsOnDateSettlement + app.InterestFromSettlementPre + app.InterestFromSettlement).ToString())//5=1+2+3+4


                            .Replace("#PrincipalOutstandingAsOnDateSettlement#", app.PrincipalOutstandingAsOnDateSettlement.ToString())  //6=1
                            .Replace("#InterestFromSettlement#", app.InterestFromSettlement.ToString())//7=4
                           .Replace("#TotalInterestSettlement#", (app.PrincipalOutstandingAsOnDateSettlement + app.InterestFromSettlement).ToString()) //8=6+7
                           .Replace("#InterestAmount#", (app.TotalInterestAsOnDateSettlement + app.InterestFromSettlementPre).ToString());//9=2+3
                        }

                        if (file.Contains("ots_certificate.html"))
                        {

                            DateTime today = DateTime.Now;
                            string formattedDate = today.ToString("dd-MM-yyyy");

                            strbld.Replace("#RegistrationNo#", app.NoticeNo)
                            .Replace("#RegistrationDate#", formattedDate);
                            if (app.ODPrincipal > 0)
                                strbld.Replace("#TotalPaidAmount#", (app.NODInterest + app.NODPenalInterest).ToString());//18=16+17
                            else
                                strbld.Replace("#TotalPaidAmount#", (app.TotalInterestAsOnDateSettlement + app.InterestFromSettlementPre).ToString());//9=2+3


                        }


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
                            {strbld.ToString()}
                        </body>
                        </html>";

                    var pdf = PDFgenerator.GeneratePdfFromHtml(str, wkhtmlPath);
                    var fileName = "OTS_Certificate_" + Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace("_", "").ToUpper() + ".pdf";

                    //return fileName;


                    string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Content/OTS/" + _Model.PLDB_OTSSchemeData[0].LoanAccNo.Replace("/", "").Replace(".", "").Replace("_", "").Replace("-", "").Replace(" ", "") + "/");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Combine the directory path with the file name
                    var filePath = Path.Combine(folderPath, fileName);

                    // Save the PDF to the specified directory
                    System.IO.File.WriteAllBytes(filePath, pdf);

                    var result1 = COMMON_FUNCTION.EsignPDF_NEW(TXN, folderPath + "/" + fileName, _Model.PLDB_OTSSchemeData[0].PLDBName.ToUpper(), "Secretary", "350", "170", "150", "170");


                    if (result1.data == "Success")
                    {
                        return fileName;
                    }

                }
                else
                {
                    return "Fail";
                }

            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "PLDBController", FunctionName = "CertificateGenerate catch " }, ex);

                return "Fail";
            }

            return "Fail";


        }



        [HttpPost]
        public JsonResult FinalReceiptGenerate(string TXN = "", PLDB_OTSSchemeData _model = null)
        {
            var responce = new APIResponce();
            var pLDBLoan = new PLDBLoan();
            var pdf = "";
            var result = 0;
            if (_model.Status == 4)
            {

                PLDB_OTSSchemeDataList pLDB_OTSSchemeDataList = new PLDB_OTSSchemeDataList();
                pLDB_OTSSchemeDataList.PLDB_OTSSchemeData = new List<PLDB_OTSSchemeData>();
                pLDB_OTSSchemeDataList.PLDB_OTSSchemeData.Add(_model);
                // pLDB_OTSSchemeDataList.PLDB_OTSSchemeData[0] = _model;

                pdf = OTSCertificateGenerated(pLDB_OTSSchemeDataList, "ots_certificate", TXN);
                _model.CertificateGenerated = pdf;
                if (pdf != "Fail")
                {
                    result = pLDBLoan.SaveFinalReceipt(_model);
                }
            }
            else
            {

                result = pLDBLoan.SaveFinalReceipt(_model);
            }


            if (pdf != "Fail") { result = pLDBLoan.SaveFinalReceipt(_model); }
            else
            {
                result = 0;
            }

            if (result > 0)
            {
                responce.Status = "200";
                responce.Message = "Success";
            }
            else if (result == -2)
            {
                responce.Status = "200";
                responce.Message = "Duplicate";

            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }




        [HttpGet]
        [ActionName("GetPLDBData")]
        public string GetPLDBDatadetails(int Id)
        {

            //SocietyModel obj = new SocietyModel();

            PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            DataTable DT = objLoan.GetPLDBDetailsData(Id);
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);


        }

        public ActionResult PLDBDetailsReportByStatus()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            if (_LoggedInUser.PLDBId != 0)
            {
                string PLDBID = _LoggedInUser.PLDBId.ToString();
                ViewBag.PLDBID = PLDBID;
            }
            if (_LoggedInUser.PLDBBrancchId != 0)
            {
                string BranchPLDBID = _LoggedInUser.PLDBBrancchId.ToString();
                ViewBag.BranchPLDBID = BranchPLDBID;
            }
            return View();
        }

        public ActionResult PLDBLoanAppDetailsReportByStatus()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            if (_LoggedInUser.PLDBId != 0)
            {
                string PLDBID = _LoggedInUser.PLDBId.ToString();
                ViewBag.PLDBID = PLDBID;
            }
            if (_LoggedInUser.PLDBBrancchId != 0)
            {
                string BranchPLDBID = _LoggedInUser.PLDBBrancchId.ToString();
                ViewBag.BranchPLDBID = BranchPLDBID;
            }
            return View();
        }

        [HttpGet]
        [ActionName("GetPLDBDataByStatus")]
        public JsonResult GetPLDBDatadetailStatus(string StatusVal, int PLDBid = 0, int BranchId = 0, int Level = 1)
        {
            PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            DataTable DT = objLoan.GetPLDBDetailsStatusData(StatusVal, PLDBid, BranchId, Level);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        [ActionName("GetLoanApplicationListstatus")]
        public JsonResult GetLoanApplicationListstatus(string StatusVal, int PLDBid = 0, int BranchId = 0, int Level = 1)
        {
            PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            DataPagingModel Details = new DataPagingModel();
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser == null)
            {
                return Json(new { DataList = "LOGOUT", DataPaging = Details }, JsonRequestBehavior.AllowGet);
            }
            DataTable DT = objLoan.GetLoanApplicationListByStatus(ref Details, StatusVal, PLDBid, BranchId, Level);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [CustomAuthentication]
        //[ValidateInput(false)]
        public int Save_PldbApplicationForm(PLDB_OTSSchemeData _Model)
        {
            //var result = new OperationResult<List<UploadFileWithPath_WithUploadtedFileTypeDataModel>>();
            int result = 0;
            try
            {
                PLDBLoan objLoan = new PLDBLoan(new DBLogger());
                result = objLoan.Save_PldbApplicationForm(_Model);

                return result;

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = " Save_PldbApplicationForm(PLDB_OTSSchemeData _Model)" }, ex);
                //return null;
            }
            return result;
        }

        [HttpPost]
        [CustomAuthentication]
        public int SavePLDB_PendingLevelData(PLDB_OTSSchemeData _Model)
        {
            int result = 0;
            try
            {
                PLDBLoan objLoan = new PLDBLoan(new DBLogger());
                result = objLoan.SavePLDB_PendingLevelData(_Model);
                return result;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = " SavePLDB_PendingLevelData(PLDB_OTSSchemeData _Model)" }, ex);
            }
            return result;
        }


        [HttpPost]
        [ActionName("GetOTSList")]
        public JsonResult GetOTSList(DataPagingModel Details)
        {
            PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            DataTable DT = objLoan.GetOTSList(ref Details);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        [ActionName("CertificateGenerate")]
        public ActionResult CertificateGenerate(PLDB_OTSSchemeDataList _Model = null)
        {
            try
            {
                string wkhtmlPath = Server.MapPath("~/bin/wkhtmltopdf.exe");
                string fontPath = new Uri(Server.MapPath("~/fonts/lohit-Devanagari.ttf")).AbsoluteUri;
                string iycLogo = new Uri(Server.MapPath("~/Content/images/iyc_logo.png")).AbsoluteUri;
                string cmPhoto = new Uri(Server.MapPath("~/Content/images/cm-ots-logo.png")).AbsoluteUri;

                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {

                    StringBuilder strbld = new StringBuilder();
                    foreach (var app in _Model.PLDB_OTSSchemeData)
                    {
                        var file = "";
                        if (app.NODPrincipal > 0 || app.NODInterest > 0 || app.NODPenalInterest > 0)
                            file = System.IO.Path.Combine(Server.MapPath("~/Template"), "ots_notice.html");
                        else
                            file = System.IO.Path.Combine(Server.MapPath("~/Template"), "ots_notice_103.html");

                        // ots_notice_103


                        var parts = new[] { app.VName, app.GpName, app.BlockName, app.TehsilName, app.VidhanSabha, app.DistrictName };
                        var address = string.Join(", ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));

                        strbld.Append(System.IO.File.ReadAllText(file));
                        strbld.Replace("#PLDB_Name#", app.PLDBNameHi)
                       .Replace("#LoaneeName#", app.ApplicantName)
                       .Replace("#LoaneeAddress#", address)
                       .Replace("#LoanAccNo#", app.LoanAccNo)
                       .Replace("#ODPrincipal_InsurancePremium#", (app.ODPrincipal + app.ODInsurancePremium).ToString())  //1=32+33
                       .Replace("#ODInterest#", app.ODInterest.ToString())                   //2 =34
                       .Replace("#ODPenalInterast#", app.ODPenalInterast.ToString())         //3 =35
                       .Replace("#ODTotal#", (app.ODPrincipal + app.ODInsurancePremium + app.ODInterest + app.ODPenalInterast).ToString())                         //4 =36
                                                                                                                                                                   //.Replace("#ODTotal#", app.ODTotal.ToString())                         //4 =36

                        .Replace("#NODPrincipal_InsurancePremium#", (app.NODPrincipal + app.NODInsurancePremium).ToString())      //5 =37+38
                       .Replace("#NODInterest#", app.NODInterest.ToString())         //6 =39
                       .Replace("#NODPenalInterast#", app.NODPenalInterest.ToString())   //7 =40
                       .Replace("#NODTotal#", (app.NODPrincipal + app.NODInsurancePremium + app.NODInterest + app.NODPenalInterest).ToString())   //8=5+6+7
                                                                                                                                                  //.Replace("#NODTotal#", app.NODTotal.ToString())   //8=5+6+7

                       .Replace("#CDPrincipal#", app.CDPrincipal.ToString()) //9=42
                       .Replace("#CDInterest#", app.CDInterest.ToString()) //10=43
                       .Replace("#CDPenalInterast#", app.CDPenalInterest.ToString()) //11=44
                       .Replace("#CDTotal#", (app.CDPrincipal + app.CDInterest + app.CDPenalInterest).ToString()) //12 =(9+10+11)


                       .Replace("#Principal#", (app.NODPrincipal + app.NODInsurancePremium + app.CDPrincipal).ToString()) //13=5+9
                       .Replace("#CDInterest_PenalInt#", (app.CDInterest + app.CDPenalInterest).ToString())   //14=10+11
                       .Replace("#Total#", (app.NODPrincipal + app.NODInsurancePremium + app.CDPrincipal + app.CDInterest + app.CDPenalInterest).ToString()) //15=13+14

                       .Replace("#Interest#", app.NODInterest.ToString())  //16=6
                       .Replace("#PenalInterest#", app.NODPenalInterest.ToString()) //17=7
                       .Replace("#TotalInterest#", (app.NODInterest + app.NODPenalInterest).ToString());//18=16+17


                        //103
                        strbld.Append(System.IO.File.ReadAllText(file));
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
                            {strbld.ToString()}
                        </body>
                        </html>";

                    var pdf = PDFgenerator.GeneratePdfFromHtml(str, wkhtmlPath);
                    var fileName = "AllCombinedOTS_" + Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace("_", "").ToUpper() + ".pdf";

                    return File(pdf, "application/pdf", fileName);


                }
                else
                {
                    return Json(new { message = "File not downloaded" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "PLDBController", FunctionName = "CertificateGenerate catch " }, ex);

                return Json(new { message = "File not downloaded" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [ActionName("BulkOTSFilesGenerated")]
        public ActionResult BulkOTSFilesGenerated(PLDB_OTSSchemeDataList _Model = null)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            string pdfPath = "";
            string succmsg = string.Empty;
            if (_LoggedInUser == null)
            {
                return Json("LOGOUT");
            }

            //if (!ModelState.IsValid) return Json(JsonModelError.Get(ModelState.Values));

            try
            {
                string wkhtmlPath = Server.MapPath("~/bin/wkhtmltopdf.exe");
                string fontPath = new Uri(Server.MapPath("~/fonts/lohit-Devanagari.ttf")).AbsoluteUri;
                string iycLogo = new Uri(Server.MapPath("~/Content/images/iyc_logo.png")).AbsoluteUri;
                string cmPhoto = new Uri(Server.MapPath("~/Content/images/cm-ots-logo.png")).AbsoluteUri;

                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {

                    StringBuilder strbld = new StringBuilder();
                    foreach (var app in _Model.PLDB_OTSSchemeData)
                    {
                        var file = "";

                        //old
                        if (app.NODPrincipal > 0 || app.NODInterest > 0 || app.NODPenalInterest > 0)
                            if (app.IsShowForNewNotice == 0)
                            {
                                file = System.IO.Path.Combine(Server.MapPath("~/Template"), "ots_notice_old.html");
                            }
                            else
                            {
                                file = System.IO.Path.Combine(Server.MapPath("~/Template"), "ots_notice.html");
                            }

                        else
                        {
                            if (app.IsShowForNewNotice == 0)
                            {
                                file = System.IO.Path.Combine(Server.MapPath("~/Template"), "ots_notice_103_old.html");
                            }
                            else
                            {
                                file = System.IO.Path.Combine(Server.MapPath("~/Template"), "ots_notice_103.html");
                            }
                        }
                        // ots_notice_103


                        //var parts = new[] { app.VName, app.GpName, app.BlockName, app.TehsilName, app.VidhanSabha, app.DistrictName };
                        var parts = new[] { app.VName, app.GpName, app.BlockName, app.DistrictName };
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
                       .Replace("#ODTotal#", (app.ODPrincipal + app.ODInsurancePremium + app.ODInterest + app.ODPenalInterast).ToString())                         //4 =36

                        .Replace("#NODPrincipal_InsurancePremium#", (app.NODPrincipal + app.NODInsurancePremium).ToString())      //5 =37+38
                       .Replace("#NODInterest#", app.NODInterest.ToString())         //6 =39
                       .Replace("#NODPenalInterast#", app.NODPenalInterest.ToString())   //7 =40
                       .Replace("#NODTotal#", (app.NODPrincipal + app.NODInsurancePremium + app.NODInterest + app.NODPenalInterest).ToString())   //8=5+6+7

                       .Replace("#CDPrincipal#", app.CDPrincipal.ToString()) //9=42
                       .Replace("#CDInterest#", app.CDInterest.ToString()) //10=43
                       .Replace("#CDPenalInterast#", app.CDPenalInterest.ToString()) //11=44
                       .Replace("#CDTotal#", (app.CDPrincipal + app.CDInterest + app.CDPenalInterest).ToString()) //12 =(9+10+11)


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
                       .Replace("#InterestAmount#", (app.TotalInterestAsOnDateSettlement + app.InterestFromSettlementPre).ToString());//9=2+3


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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "PLDBController", FunctionName = "CertificateGenerate catch " }, ex);

                return Json(new { message = "File not downloaded" }, JsonRequestBehavior.AllowGet);
            }

        }


        //old
        //[HttpPost]
        //[ActionName("SaveOTSVerifyReportFile")]
        //public string SaveOTSVerifyReportFile(OTSDocumentsModel _Entity)
        //{
        //    int Result = 0;
        //    string fileName = string.Empty;
        //    string ReturnJSON = "";
        //    string pathString = "~/Content/OTS/AcceptanceLetter/";

        //    PLDBLoan objLoan = new PLDBLoan(new DBLogger());
        //    foreach (string files in Request.Files)
        //    {

        //        HttpPostedFileBase file = Request.Files[files];
        //        var prefix = "";


        //        fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace(" ", "").Replace("_", "").ToUpper() + System.IO.Path.GetExtension(file.FileName);


        //        if (files.ToString() == "DocSelfie")
        //        {
        //            prefix = "Selfie_";
        //            _Entity.OtherDoc = prefix + fileName;

        //        }

        //        else if (files.ToString() == "DocTaamil")
        //        {
        //            prefix = "Taamil_";
        //            _Entity.TaamilDoc = prefix + fileName;
        //        }

        //        if (!System.IO.File.Exists(pathString))
        //        {
        //            System.IO.Directory.CreateDirectory(Server.MapPath(pathString));
        //        }
        //        file.SaveAs(Path.Combine(Server.MapPath(pathString), prefix + fileName));


        //        // break;
        //    }

        //    Result = objLoan.InsertOTSReportFile(_Entity);
        //    if (Result == 0)
        //    {
        //        var ds = objLoan.GetOtsApplicantDetails(new CoopEntities().GetEncrypt(_Entity.OTSId.ToString()).FirstOrDefault().ToString(), 0, 0);
        //        if (ds != null)
        //        {
        //            var ApplicantDetail = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null;
        //            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //            {
        //                if (ds.Tables[0].Rows[0]["MobileNo"] != "")
        //                {
        //                    DateTime today = DateTime.Now;
        //                    string formattedDate = today.ToString("dd-MM-yyyy");
        //                    PldbSmsParam pldbSmsParam = new PldbSmsParam();
        //                    pldbSmsParam.MobileNo = ds.Tables[0].Rows[0]["MobileNo"].ToString();
        //                    pldbSmsParam.AccNo = ds.Tables[0].Rows[0]["LoanAccNo"].ToString();
        //                    pldbSmsParam.Date = formattedDate;
        //                    pldbSmsParam.BranchName = ds.Tables[0].Rows[0]["PLDBBranch"].ToString();
        //                    pldbSmsParam.DistrictName = ds.Tables[0].Rows[0]["DistrictName"].ToString();
        //                    pldbSmsParam.TemplateKey = "OTS_Notice";
        //                    SendPldbSms(pldbSmsParam);
        //                }
        //            }
        //        }
        //    }
        //    ReturnJSON = "{\"fileName\":\"" + fileName.ToString() + "\",\"docId\":\"" + Result + "\"}";

        //    return ReturnJSON;

        //}


        //New
        [HttpPost]
        [ActionName("SaveOTSVerifyReportFile")]
        public string SaveOTSVerifyReportFile(OTSDocumentsNewModel _Entity)
        {
            int Result = 0;
            string fileName = string.Empty;
            string ReturnJSON = "";
            string pathString = "~/Content/OTS/AcceptanceLetterNew/";

            PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            foreach (string files in Request.Files)
            {

                HttpPostedFileBase file = Request.Files[files];
                var prefix = "";


                fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace(" ", "").Replace("_", "").ToUpper() + System.IO.Path.GetExtension(file.FileName);


                if (files.ToString() == "DocSelfie")
                {
                    prefix = "Selfie_";
                    _Entity.OtherDoc_New = prefix + fileName;

                }

                else if (files.ToString() == "DocTaamil")
                {
                    prefix = "Taamil_";
                    _Entity.TaamilDoc_New = prefix + fileName;
                }

                if (!System.IO.File.Exists(pathString))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(pathString));
                }
                file.SaveAs(Path.Combine(Server.MapPath(pathString), prefix + fileName));


                // break;
            }

            Result = objLoan.InsertOTSReportFile(_Entity);
            if (Result == 0)
            {
                var ds = objLoan.GetOtsApplicantDetails(new CoopEntities().GetEncrypt(_Entity.OTSId.ToString()).FirstOrDefault().ToString(), 0, 0);
                if (ds != null)
                {
                    var ApplicantDetail = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null;
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["MobileNo"] != "")
                        {
                            DateTime today = DateTime.Now;
                            string formattedDate = today.ToString("dd-MM-yyyy");
                            PldbSmsParam pldbSmsParam = new PldbSmsParam();
                            pldbSmsParam.MobileNo = ds.Tables[0].Rows[0]["MobileNo"].ToString();
                            pldbSmsParam.AccNo = ds.Tables[0].Rows[0]["LoanAccNo"].ToString();
                            pldbSmsParam.Date = formattedDate;
                            pldbSmsParam.BranchName = ds.Tables[0].Rows[0]["PLDBBranch"].ToString();
                            pldbSmsParam.DistrictName = ds.Tables[0].Rows[0]["DistrictName"].ToString();
                            pldbSmsParam.TemplateKey = "OTS_Notice";
                            SendPldbSms(pldbSmsParam);
                        }
                    }
                }
            }
            ReturnJSON = "{\"fileName\":\"" + fileName.ToString() + "\",\"docId\":\"" + Result + "\"}";

            return ReturnJSON;

        }

        [HttpPost]
        [ActionName("SaveAadharOTSFile")]
        public string SaveAadharOTSFile(OTSAadharJanaadharModel _Entity)
        {
            int Result = 0;
            string fileName = string.Empty;
            string ReturnJSON = "";
            string pathString = "~/Content/OTS/LoaneeDoc/";

            PLDBLoan objLoan = new PLDBLoan(new DBLogger());

            var file = Request.Files[0];

            if (file != null)
            {
                fileName = "Aadhar_" + Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace(" ", "").Replace("_", "").ToUpper() + System.IO.Path.GetExtension(file.FileName);

                if (!System.IO.File.Exists(pathString))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(pathString));
                }
                file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));

                _Entity.Aadhar_Janaadhar_FileName = fileName;
                Result = objLoan.UploadOTSAadharFile(_Entity);
            }
            else
            {
                Result = 0;
            }

            ReturnJSON = "{\"fileName\":\"" + fileName.ToString() + "\",\"docId\":\"" + Result + "\"}";

            return ReturnJSON;

        }



        [HttpPost]
        public JsonResult GetOTSListApplicationFormViewData(string PldbOtsSchemeId)
        {
            try
            {
                string Ids = "";
                DataTable DT = null;
                if (!String.IsNullOrEmpty(PldbOtsSchemeId) && !String.IsNullOrEmpty(PldbOtsSchemeId))
                {
                    CoopEntities DB = new CoopEntities();
                    Ids = DB.GetDecrypt(PldbOtsSchemeId).FirstOrDefault().ToString();

                    PLDBLoan objLoan = new PLDBLoan(new DBLogger());
                    DT = objLoan.GetOTSListApplicationFormViewData(Ids, "MainData");
                    return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.PLDBController", FunctionName = "GetOTSListApplicationFormViewData(string PldbOtsSchemeId)" }, ex);
                return null;
            }
        }

        [HttpPost]
        public JsonResult UpdateRepaymentSummary(int Id, string Remark = "", string OTP = "", string TXN = "")
        {
            var responce = new APIResponce();
            ResultMessage otpResult = new ResultMessage();

            if (!Request.Url.Host.Contains("localhost"))
            {
                otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
            }
            else
            {
                otpResult.status = "1"; otpResult.data = TXN;
            }

            if (otpResult.status == "1")
            {
                var pLDBLoan = new PLDBLoan();
                var result = pLDBLoan.UpdateRepaymentSummary(Id, Remark);
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
            }
            else
            {

                responce.Status = "300";
                responce.Message = "Invalid OTP";

            }


            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetPldbRecoveryPaymentStatistics()
        {
            int districtId = 0, pldbId = 0, pldbBranchId = 0;
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser.DistrictId > 0)
                districtId = _LoggedInUser.DistrictId;
            if (_LoggedInUser.PLDBId > 0)
                pldbId = _LoggedInUser.PLDBId;
            if (_LoggedInUser.PLDBBrancchId > 0)
                pldbBranchId = _LoggedInUser.PLDBBrancchId;

            PLDBLoan objLoan = new PLDBLoan(new DBLogger());

            var responce = new APIResponce();
            var ds = objLoan.GetPldbPaymentRecoveryStatistics(districtId, pldbId, pldbBranchId);
            if (ds != null)
            {
                responce.Data = new
                {
                    PaymentRecoveryData = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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


        public ActionResult NIActDetail()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetNiActApplicantDetails(string otsIdEnc, int summaryId)
        {
            var responce = new APIResponce();
            var pLDBLoan = new PLDBLoan(new FileLogger());
            var ds = pLDBLoan.GetNiActApplicationDetail(otsIdEnc, summaryId);
            if (ds != null)
            {
                responce.Data = new
                {
                    ApplicantDetail = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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
        public JsonResult SaveNIActDetails(NIActDetailsModel nIActDetails)
        {
            var responce = new APIResponce();
            ResultMessage otpResult = new ResultMessage();

            var pLDBLoan = new PLDBLoan();
            var result = pLDBLoan.SavePLDBNIActDetails(nIActDetails);
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
        public JsonResult SaveCaseNotMortgageStatus(CaseNotMortageModel caseNotMortage)
        {
            var responce = new APIResponce();
            ResultMessage otpResult = new ResultMessage();

            var pLDBLoan = new PLDBLoan();
            var result = pLDBLoan.SaveCaseNotMortgageStatus(caseNotMortage);
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
        [CustomAuthentication]
        //[ValidateInput(false)]
        public int Save_PldbSummaryODApplicationForm(PLDB_SummaryODSubsidy _Model)
        {
            //var result = new OperationResult<List<UploadFileWithPath_WithUploadtedFileTypeDataModel>>();
            int result = 0;
            try
            {
                PLDBLoan objLoan = new PLDBLoan(new DBLogger());
                result = objLoan.Save_PldbSummaryODSubsidyApplicationForm(_Model);

                return result;

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.PLDBController", FunctionName = " Save_PldbSummaryODApplicationForm(PLDB_SummaryODSubsidy _Model)" }, ex);
                //return null;
            }
            return result;
        }



        [HttpPost]
        [ActionName("GetSubsidySummaryList")]
        public JsonResult GetSubsidySummaryList(DataPagingModel Details)
        {
            PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            DataTable DT = objLoan.GetSubsidySummaryList(ref Details);

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
        [ActionName("PldbLoanApplicationDetailslist")]
        public JsonResult PldbLoanApplicationDetailslist(DataPagingModel Details)
        {
            PLDBLoan objLoan = new PLDBLoan(new DBLogger());
            DataTable DT = objLoan.PldbLoanApplicationDetailsList(ref Details);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSubsidySummaryListApplicationFormViewData(string PldbOtsSchemeId)
        {
            try
            {
                string Ids = "";
                DataTable DT = null;
                if (!String.IsNullOrEmpty(PldbOtsSchemeId) && !String.IsNullOrEmpty(PldbOtsSchemeId))
                {
                    CoopEntities DB = new CoopEntities();
                    Ids = DB.GetDecrypt(PldbOtsSchemeId).FirstOrDefault().ToString();

                    PLDBLoan objLoan = new PLDBLoan(new DBLogger());
                    DT = objLoan.GetSubsidyListApplicationFormViewData(Ids, "MainData");
                    return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.PLDBController", FunctionName = "GetSubsidySummaryListApplicationFormViewData(string PldbOtsSchemeId)" }, ex);
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetPLDBLoanApplicationListFormViewData(string PldbOtsSchemeId)
        {
            try
            {
                if (!String.IsNullOrEmpty(PldbOtsSchemeId))
                {
                    CoopEntities DB = new CoopEntities();
                    string Ids = DB.GetDecrypt(PldbOtsSchemeId).FirstOrDefault().ToString();

                    PLDBLoan objLoan = new PLDBLoan(new DBLogger());
                    DataSet ds = objLoan.GetPLDBLoanApplicationListFormViewData(Ids);

                    return Json(new
                    {
                        Main = COMMON_FUNCTION.DataTableToJSONWithJSONNet(ds.Tables[0]),
                        Guarantor = COMMON_FUNCTION.DataTableToJSONWithJSONNet(ds.Tables[1]),
                        Disbursement = COMMON_FUNCTION.DataTableToJSONWithJSONNet(ds.Tables[2])
                    }, JsonRequestBehavior.AllowGet);
                }

                return null;
            }
            catch (Exception ex)
            {
                new DBLogger().ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "PLDBController",
                    FunctionName = "GetPLDBLoanApplicationListFormViewData"
                }, ex);

                return null;
            }
        }



        //[HttpPost]
        //[CustomAuthentication]
        //public int Save_PldbLoanApplicationDetails(PLDB_LoanApplicationDetailsModel _Model)
        //{
        //    int result = 0;
        //    try
        //    {
        //        PLDBLoan objLoan = new PLDBLoan(new DBLogger());
        //        result = objLoan.Save_PldbLoanApplicationDetails(_Model);
        //        return result;

        //    }
        //    catch (Exception ex)
        //    {
        //        DBLogger objDBLogger = new DBLogger();
        //        objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.PLDBController", FunctionName = " Save_PldbLoanApplicationDetails(PLDB_LoanApplicationDetailsModel _Model)" }, ex);

        //    }
        //    return result;
        //}

        [HttpPost]
        [CustomAuthentication]
        public JsonResult Save_PldbLoanApplicationDetails(PLDB_LoanApplicationDetailsModel _Model)
        {
            try
            {
                PLDBLoan objLoan = new PLDBLoan(new DBLogger());

                int result = objLoan.Save_PldbLoanApplicationDetails(_Model);

                return Json(new { Result = result, encGccid = _Model.encGccid }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();

                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.PLDBController", FunctionName = "Save_PldbLoanApplicationDetails(PLDB_LoanApplicationDetailsModel _Model)" }, ex);

                return Json(new { Result = 0, encGccid = "" }, JsonRequestBehavior.AllowGet);
            }
        }




        //[HttpPost]
        //public JsonResult UploadLoanDocument(HttpPostedFileBase LoanDocument, int DocumentNo, int ApplicationId)
        //{
        //    try
        //    {
        //        if (LoanDocument == null)
        //            return Json(new { success = false, message = "File not received" });

        //        string folder = Server.MapPath("~/Content/PLDB/LoanDocuments/");
        //        if (!Directory.Exists(folder))
        //            Directory.CreateDirectory(folder);

        //        string fileName = Guid.NewGuid() + Path.GetExtension(LoanDocument.FileName);
        //        string filePath = Path.Combine(folder, fileName);

        //        LoanDocument.SaveAs(filePath);

        //        //upload Function Call
        //        PLDBLoan objLoan = new PLDBLoan(new DBLogger());

        //        objLoan.UpdateLoanDocuments(ApplicationId, DocumentNo, fileName);

        //        return Json(new { success = true, fileName = fileName });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = ex.Message });
        //    }
        //}


        [HttpPost]
        public JsonResult UploadLoanDocument(HttpPostedFileBase LoanDocument)
        {
            try
            {
                if (LoanDocument == null)
                    return Json(new { success = false, message = "File not received" });

                string folder = Server.MapPath("~/Content/PLDB/LoanDocuments/");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(LoanDocument.FileName);
                string filePath = Path.Combine(folder, fileName);

                LoanDocument.SaveAs(filePath);

                return Json(new { success = true, fileName = fileName });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public JsonResult UploadLoanDocument2(HttpPostedFileBase LoanDocument2)
        {
            try
            {
                if (LoanDocument2 == null)
                    return Json(new { success = false, message = "File not received" });

                string folder = Server.MapPath("~/Content/PLDB/LoanDocument2/LoanDocuments");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(LoanDocument2.FileName);
                string filePath = Path.Combine(folder, fileName);

                LoanDocument2.SaveAs(filePath);

                return Json(new { success = true, fileName = fileName });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UploadLoanDocument3(HttpPostedFileBase LoanDocument3)
        {
            try
            {
                if (LoanDocument3 == null)
                    return Json(new { success = false, message = "File not received" });

                string folder = Server.MapPath("~/Content/PLDB/LoanDocument3/LoanDocuments");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(LoanDocument3.FileName);
                string filePath = Path.Combine(folder, fileName);

                LoanDocument3.SaveAs(filePath);

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

                string folderPath = Server.MapPath("~/Content/PLDB/LoanDocuments/");
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


        [HttpPost]
        public JsonResult DeleteLoanDocument2(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    return Json(new { success = false, message = "Invalid file name" });
                }

                string folderPath = Server.MapPath("~/Content/PLDB/LoanDocument2/LoanDocuments");
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


        [HttpPost]
        public JsonResult DeleteLoanDocument3(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    return Json(new { success = false, message = "Invalid file name" });
                }

                string folderPath = Server.MapPath("~/Content/PLDB/LoanDocument3/LoanDocuments");
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

        [HttpPost]
        [CustomAuthentication]
        public int DeletePldbLoanSubsidyApplication(int id)
        {
            int result = 0;

            try
            {
                PLDBLoan objLoan = new PLDBLoan(new DBLogger());
                result = objLoan.Delete_PldbLoanApplicationSubsidy(id);
                return result;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.PLDBController",
                    FunctionName = "Delete_PldbLoanApplication(int id)"
                }, ex);
            }

            return result;
        }


        //#region PldbLoanApplicationLoanDisbursementCasesList
        //[HttpPost]
        //[ActionName("PldbLoanApplicationLoanDisbursementCasesList")]
        //public JsonResult PldbLoanApplicationLoanDisbursementCasesList(DataPagingModel Details , bool isExport = false)
        //{
        //    PLDBLoan objLoan = new PLDBLoan(new DBLogger());
        //    DataTable DT = objLoan.PldbLoanApplicationLoanDisbursementCasesList(ref Details , isExport);
        //    return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        //}
        //#endregion PldbLoanApplicationLoanDisbursementCasesList


        //#region PLDBApplicationInterestSubventionODCasesList




        [HttpPost]
        [ActionName("PldbLoanApplicationLoanDisbursementCasesList")]
        public JsonResult PldbLoanApplicationLoanDisbursementCasesList(DataPagingModel Details, bool isExport = false)
        {
            PLDBLoan objLoan = new PLDBLoan(new DBLogger());

            DataTable DT = objLoan.PldbLoanApplicationLoanDisbursementCasesList(ref Details, isExport);

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


        //public JsonResult PLDBApplicationInterestSubventionODCasesList(DataPagingModel Details)
        //{
        //    PLDBLoan objLoan = new PLDBLoan(new DBLogger());

        //    DataTable DT = objLoan.GetSubsidySummaryODCasesList(ref Details);
        //    return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

        //}

        #region PLDBApplicationInterestSubventionODCasesList

        [HttpPost]
        [ActionName("PLDBApplicationInterestSubventionODCasesList")]
        public JsonResult PLDBApplicationInterestSubventionODCasesList(DataPagingModel Details)
        {
            PLDBLoan objLoan = new PLDBLoan(new DBLogger());

            DataTable DT = objLoan.GetSubsidySummaryODCasesList(ref Details);

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

        #endregion PLDBApplicationInterestSubventionODCasesList


        #region eSing process 2.1 11-06-2026
        /// <summary>
        /// OTS Certification Generate
        /// </summary>
        /// <param name="_Model"></param>
        /// <param name="PdfName"></param>
        /// <returns></returns>
        public string OTSGenerateCertificate(PLDB_OTSSchemeDataList _Model = null, string PdfName = "OTS_CERTIFICATE")
        {
            try
            {
                string wkhtmlPath = Server.MapPath("~/bin/wkhtmltopdf.exe");
                string fontPath = new Uri(Server.MapPath("~/fonts/lohit-Devanagari.ttf")).AbsoluteUri;
                string iycLogo = new Uri(Server.MapPath("~/Content/images/iyc_logo.png")).AbsoluteUri;
                string cmPhoto = new Uri(Server.MapPath("~/Content/images/cm-ots-logo.png")).AbsoluteUri;

                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var app in _Model.PLDB_OTSSchemeData)
                    {
                        var file = "";
                        if (app.NODPrincipal > 0 || app.NODInterest > 0 || app.NODPenalInterest > 0)
                            file = System.IO.Path.Combine(Server.MapPath("~/Template"), "ots_notice.html");
                        else
                            file = System.IO.Path.Combine(Server.MapPath("~/Template"), "ots_notice_103.html");


                        if (PdfName == "OTS_CERTIFICATE")
                        {
                            file = System.IO.Path.Combine(Server.MapPath("~/Template"), "ots_certificate.html");
                        }

                        // ots_notice_103


                        //var parts = new[] { app.VName, app.GpName, app.BlockName, app.TehsilName, app.VidhanSabha, app.DistrictName };
                        var parts = new[] { app.VName, app.GpName, app.BlockName, app.DistrictName };
                        var address = string.Join(", ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));

                        sb.Append(System.IO.File.ReadAllText(file));
                        sb.Replace("#PLDB_Name#", app.PLDBNameHi)
                       .Replace("#LoaneeName#", app.ApplicantName)
                       .Replace("#LoaneeAddress#", address)
                       .Replace("#DistrictName#", app.DistrictName)
                       .Replace("#LoanAccNo#", app.LoanAccNo)
                       .Replace("#LoaneeFatherName#", app.FatherHusbandName);
                        if (file.Contains("ots_notice.html"))
                        {
                            sb.Replace("#ODPrincipal_InsurancePremium#", (app.ODPrincipal + app.ODInsurancePremium).ToString())  //1=32+33
                          .Replace("#ODInterest#", app.ODInterest.ToString())                   //2 =34
                          .Replace("#ODPenalInterast#", app.ODPenalInterast.ToString())         //3 =35
                          .Replace("#ODTotal#", (app.ODPrincipal + app.ODInsurancePremium + app.ODInterest + app.ODPenalInterast).ToString())                         //4 =36

                           .Replace("#NODPrincipal_InsurancePremium#", (app.NODPrincipal + app.NODInsurancePremium).ToString())      //5 =37+38
                          .Replace("#NODInterest#", app.NODInterest.ToString())         //6 =39
                          .Replace("#NODPenalInterast#", app.NODPenalInterest.ToString())   //7 =40
                          .Replace("#NODTotal#", (app.NODPrincipal + app.NODInsurancePremium + app.NODInterest + app.NODPenalInterest).ToString())   //8=5+6+7

                          .Replace("#CDPrincipal#", app.CDPrincipal.ToString()) //9=42
                          .Replace("#CDInterest#", app.CDInterest.ToString()) //10=43
                          .Replace("#CDPenalInterast#", app.CDPenalInterest.ToString()) //11=44
                          .Replace("#CDTotal#", (app.CDPrincipal + app.CDInterest + app.CDPenalInterest).ToString()) //12 =(9+10+11)


                          .Replace("#Principal#", (app.NODPrincipal + app.NODInsurancePremium + app.CDPrincipal).ToString()) //13=5+9
                          .Replace("#CDInterest_PenalInt#", (app.CDInterest + app.CDPenalInterest).ToString())   //14=10+11
                          .Replace("#Total#", (app.NODPrincipal + app.NODInsurancePremium + app.CDPrincipal + app.CDInterest + app.CDPenalInterest).ToString()) //15=13+14

                          .Replace("#Interest#", app.NODInterest.ToString())  //16=6
                          .Replace("#PenalInterest#", app.NODPenalInterest.ToString()) //17=7
                          .Replace("#TotalInterest#", (app.NODInterest + app.NODPenalInterest).ToString());//18=16+17

                        }
                        //103
                        if (file.Contains("ots_notice_103.html"))
                        {
                            sb.Append(System.IO.File.ReadAllText(file));
                            sb.Replace("#PrincipalOutstandingAsOnDateSettlement#", app.PrincipalOutstandingAsOnDateSettlement.ToString())  //1
                           .Replace("#TotalInterestAsOnDateSettlement#", app.TotalInterestAsOnDateSettlement.ToString()) //2
                           .Replace("#InterestFromSettlementPre#", app.InterestFromSettlementPre.ToString())//3
                           .Replace("#InterestFromSettlement#", app.InterestFromSettlement.ToString())//4
                           .Replace("#TotalInterestFromSettlement#", (app.PrincipalOutstandingAsOnDateSettlement + app.TotalInterestAsOnDateSettlement + app.InterestFromSettlementPre + app.InterestFromSettlement).ToString())//5=1+2+3+4
                            .Replace("#PrincipalOutstandingAsOnDateSettlement#", app.PrincipalOutstandingAsOnDateSettlement.ToString())  //6=1
                            .Replace("#InterestFromSettlement#", app.InterestFromSettlement.ToString())//7=4
                           .Replace("#TotalInterestSettlement#", (app.PrincipalOutstandingAsOnDateSettlement + app.InterestFromSettlement).ToString()) //8=6+7
                           .Replace("#InterestAmount#", (app.TotalInterestAsOnDateSettlement + app.InterestFromSettlementPre).ToString());//9=2+3
                        }

                        if (file.Contains("ots_certificate.html"))
                        {

                            DateTime today = DateTime.Now;
                            string formattedDate = today.ToString("dd-MM-yyyy");

                            sb.Replace("#RegistrationNo#", app.NoticeNo)
                            .Replace("#RegistrationDate#", formattedDate);
                            if (app.ODPrincipal > 0)
                                sb.Replace("#TotalPaidAmount#", (app.NODInterest + app.NODPenalInterest).ToString());//18=16+17
                            else
                                sb.Replace("#TotalPaidAmount#", (app.TotalInterestAsOnDateSettlement + app.InterestFromSettlementPre).ToString());//9=2+3
                        }


                        sb.Replace("#cmPhoto#", cmPhoto);
                        sb.Replace("#iycLogo#", iycLogo);
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
                            {sb.ToString()}
                        </body>
                        </html>";

                    var pdf = PDFgenerator.GeneratePdfFromHtml(str, wkhtmlPath);
                    var fileName = "OTS_Certificate_" + Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace("_", "").ToUpper() + ".pdf";

                    //return fileName;


                    string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Content/OTS/" + _Model.PLDB_OTSSchemeData[0].LoanAccNo.Replace("/", "").Replace(".", "").Replace("_", "").Replace("-", "").Replace(" ", "") + "/");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    // Combine the directory path with the file name
                    var filePath = Path.Combine(folderPath, fileName);
                    // Save the PDF to the specified directory
                    System.IO.File.WriteAllBytes(filePath, pdf);
                    return filePath;
                }
                else
                {
                    return "Fail";
                }

            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "PLDBController", FunctionName = "CertificateGenerate catch " }, ex);

                return "Fail";
            }
        }

        [HttpPost]
        public JsonResult FinalReceiptGenerateNewEsign(PLDB_OTSSchemeData _model = null)
        {
            try
            {
                var pdfPath = "";
                string documentName = "";
                PLDB_OTSSchemeDataList pLDB_OTSSchemeDataList = new PLDB_OTSSchemeDataList();
                pLDB_OTSSchemeDataList.PLDB_OTSSchemeData = new List<PLDB_OTSSchemeData>();
                pLDB_OTSSchemeDataList.PLDB_OTSSchemeData.Add(_model);
                if (pLDB_OTSSchemeDataList != null)
                {
                    pdfPath = OTSGenerateCertificate(pLDB_OTSSchemeDataList, "OTS_CERTIFICATE");
                    documentName = Path.GetFileName(pdfPath);
                    _model.CertificateGenerated = documentName;
                }
                else { pdfPath = "Fail"; }

                if (pdfPath != "Fail")
                {
                    return Json(new
                    {
                        Success = true,
                        DocPath = pdfPath,
                        DocType = "OTS Certificate",
                        DocName = documentName,
                        Location = pLDB_OTSSchemeDataList.PLDB_OTSSchemeData[0].PLDBName.ToUpper(),
                        Designation = "Secretary",
                        ReturnPath = Url.Action("PldbFinalLoanPayment", "PLDB")
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
                        Location = "",
                        Designation = "",
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.PLDB", FunctionName = "FinalReceiptGenerateNewEsign(PLDB_OTSSchemeData _model)" }, ex);

                return Json(new
                {
                    Success = false,
                    Message = ex.Message,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    Location = "",
                    Designation = "",
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult FinalCetrificateGenerate(PLDB_OTSSchemeData _model = null)
        {
            var responce = new APIResponce();
            var pLDBLoan = new PLDBLoan();

            int result = pLDBLoan.SaveFinalReceipt(_model);

            if (result > 0)
            {
                responce.Status = "200";
                responce.Message = "Success";
            }
            else if (result == -2)
            {
                responce.Status = "200";
                responce.Message = "Duplicate";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }

            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region STEP 1 - START ESIGN
        [HttpPost]
        public async Task<JsonResult> StartEsign(string DocType, string docid, string SocietyIDEnc = "", string DocumentPath = "", string DocumentName = "", string ReturnPath = "", string Remark = "", int? StatusId = null, bool IsAmendment = false, string AmendmendAmount = "", string ReceiptNumber = "", PLDB_OTSSchemeData _model = null, string Location = "", string Designation = "")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            EsignModel eSignModel = new EsignModel(new DBLogger());
            var detailsJson = JsonConvert.SerializeObject(_model);
            string UAT_BaseUrl = "http://localhost:27466/eSign/EspResponse";
            string Prod_BaseUrl = "https://rajsahakarapp.rajasthan.gov.in/eSign/EspResponse";

            try
            {
                if (_LoggedInUser != null)
                {
                    string responseUrl = ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")) ? UAT_BaseUrl : Prod_BaseUrl;
                    string pdfPath = string.Empty;
                    switch (DocType)
                    {
                        case "Ware House":
                            pdfPath = Server.MapPath(DocumentPath) + "/" + DocumentName;
                            break;

                        case "OTS Certificate":
                            pdfPath = DocumentPath;
                            break;

                        default:
                            pdfPath = Server.MapPath(DocumentPath);
                            break;
                    }
                    string prn = eSignController.GeneratePRN();
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
    }
}

