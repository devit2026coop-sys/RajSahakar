using Cooperative.Models;
using iTextSharp.text;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.html;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Cooperative.Filters;
using CoopDAL;
using Newtonsoft.Json;
using static Cooperative.Controllers.COMMON_FUNCTION;
using System.Linq;

namespace Cooperative.Controllers
{
    public class MobileApplicationFlowController : Controller
    {
        MobileApplicationFlowModel objMobileFlowModel = new MobileApplicationFlowModel(new DBLogger());
        int UserId = 0, RoleId = 0;
        string SSOID = "", DISTRICT_ENG = "Jaipur";
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public string BankOpeningCertificateGenerate(string SocietyIDEnc, ResultMessage OtpResult)
        {
            int result = -1;
            string lbl_Registraion = "";
            string lbl_CEOName = "";
            string lbl_TodayDate = "";
            string lbl_Societyaddress = "";
            string lbl_RegistrationNo = "";
            string lbl_Amount = "";
            string lbl_AO_ADD_DistrictEn = "";
            DataTable dt = new DataTable();
            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
            if (!ModelState.IsValid)
                return "0";
            dt = objMobileFlowModel.GetApplicationPdfdata(SocietyIDEnc);
            result = 0;
            if (dt.Rows.Count == 0)
            {
                return "-2";
            }


            if (dt.Rows.Count > 0)
            {
                lbl_Registraion = dt.Rows[0]["SocietyDirectory"].ToString();
                lbl_CEOName = dt.Rows[0]["Name"].ToString();
                lbl_TodayDate = dt.Rows[0]["TodayDate"].ToString();
                lbl_Societyaddress = dt.Rows[0]["Societyaddress"].ToString();
                lbl_RegistrationNo = dt.Rows[0]["RegistrationNo"].ToString();
                lbl_Amount = dt.Rows[0]["Amount"].ToString();

                lbl_AO_ADD_DistrictEn = dt.Rows[0]["DISTRICT_ENG"].ToString();


                result = dt.Rows[0]["BankOpeningCertificate"].ToString() == "" ? 0 : 1;
            }

            //SocietyRegistrationEntity _Entity = new SocietyRegistrationEntity();
            //_Entity.flag = 2;
            //_Entity.SocietyIDEnc = SocietyIDEnc;
            //_Entity.BankOpeningCertificate = lbl_Registraion + "BankOpeningCertificate";
            //SocietyModel objSocietyModel = new SocietyModel(new DBLogger());

            //objSocietyModel.SaveBankDetailmodel(_Entity);

            if (result == 0)
            {

                StringBuilder sb = new StringBuilder();
                //sb.Append("<html><body><div style='padding-right:25px; margin-top:50px; padding-left:25px; margin-left:25px;' ><center></center></div><br /><div id='divtoPrint'>");
                //sb.Append("<div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;font-size:20px; '>");
                //sb.Append("<table id='content'  border='0'  cellpadding='5' cellspacing='0'  style='width:100%'>");
                //sb.Append("<tr><td colspan='4' align='center' style='width: 100%'><h3> सहकारिता विभाग / Cooperative Department </h3></td></tr>");
                //// sb.Append("<tr><td colspan='4' align='center' style='width: 100%;'><h4> फ़ोन नं -2709827,2710072  फैक्स नं -91-0141-2710072 </h4></td></tr>");
                //sb.Append("<tr><td colspan='4' align='center' style='width: 100%; font-weight: normal !important;'><h4> बैंक खाता खोलने का प्रमाण / Certificate of bank opening </h4></td></tr>");
                //sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
                //sb.Append("<tr><td colspan='3' align='left' style='width: 75%;'> क्रमांक फा . (" + lbl_RegistrationNo + ") </td><td colspan='1' align='left' style='width: 25%;'> दिनांक " + DateTime.Now.ToString("dd/MM/yyyy") + " </td></tr>");
                //sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
                //sb.Append("<tr><td colspan='4' align='left' style='width: 100%'><p style='text-align: justify;text-justify: inter-word;'> श्री " + lbl_CEOName + " के आवेदन पत्र दिनांक " + DateTime.Now.ToString("dd/MM/yyyy") + " के क्रम मे प्रस्तावित सोसाइटी " + lbl_Societyaddress + "सहकारी समिति परिसिमिति अमाउंट " + lbl_Amount + " की राशि केंद्रीय सहकारी बैंक लि. में नॉमिनल खाते मे जमा करने की अनुमति दी जाती है । </p></td></tr>");
                //sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
                //sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
                //sb.Append("<tr><td colspan='4' align='left' style='width: 100%'><p style='text-align: justify;'> Application for the name of Mr." + lbl_CEOName + " Nomination  in the order of " + DateTime.Now.ToString("dd/MM/yyyy") + ", the name of the proposed " + lbl_Societyaddress + " Co-operative Committee, the amount of amounts in the Central Cooperative Bank Ltd. Are allowed to be deposited in the nominal account.</p></td></tr>");
                //sb.Append("<tr><td colspan='4' align='left' style='width: 100%'><p style='text-align: justify;border: 1px solid black;''><b>Note-</b> This is a digitally signed certificate and does not required any physical signature. Also, this certificate can be validated using QR Code </p></td></tr>");
                //sb.Append("</table></div></div></body></html>");




                string devFontSize = "17px";
                string fontSize = "font-size: 13px;";
                sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:50px'>");
                sb.Append("<table style='width: 100%;border:1px solid;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px'>");
                sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                sb.Append("<img style='width:90px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:32px;'>" + UnicodeToKrutidev.GetKrutidev("सहकारिता विभाग") + "</label><label  style='font-size:25px;'>/ COOPERATIVE DEPARTMENT </label></b>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");
                sb.Append("<b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("बैंक खाता खोलने का प्रमाण") + "</label><label  style='font-size:15px;'>/ CERTIFICATE OF BANK ACCOUNT OPENINGE</label></b>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
                sb.Append("<table style='width:100%'>");
                sb.Append("<tr>");
                sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'><b> पं. संख्या / REG. NO.- " + lbl_RegistrationNo + "</b></td>");
                sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> दिनांक / DATE- " + DateTime.Now.ToString("dd-MM-yyyy") + " </td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'><p style='text-align: justify;text-justify: inter-word;'> श्री <b> " + lbl_CEOName.ToUpper() + " </b> के आवेदन पत्र दिनांक <b> " + DateTime.Now.ToString("dd/MM/yyyy") + " </b> के क्रम मे प्रस्तावित सोसाइटी <b> " + lbl_Societyaddress.ToUpper() + "</b> सहकारी समिति परिसिमिति अमाउंट <b>" + lbl_Amount + "</b> की राशि केंद्रीय सहकारी बैंक लि. में नॉमिनल खाते मे जमा करने की अनुमति दी जाती है । </p>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr><td style='padding: 15px; line-height: 30px; font-size: 12px; font-weight: normal; height: 100px;'><p style='text-align: justify;'>" + ("Application for the name of Mr. <b> " + lbl_CEOName + "</b> Nomination  in the order of <b>" + DateTime.Now.ToString("dd/MM/yyyy") + "</b>, the name of the proposed <b>" + lbl_Societyaddress + "</b> Co-operative Committee, the amount <b>" + lbl_Amount + "</b> of amounts in the Central Cooperative Bank Ltd. Are allowed to be deposited in the nominal account").ToUpper() + ".</p></td></tr>");
                sb.Append("<tr><td style='text-align: left;'>");
                var fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace("_", "").ToUpper() + ".pdf";
                string QRCoder = COMMON_FUNCTION.GenerateQRCode("https://rajsahakarapp.rajasthan.gov.in/Content/SocietyProfiling/" + lbl_Registraion.Replace("-", "").Replace("/", "").Replace("_", "") + "/" + fileName);
                sb.Append("<img style='width:180px:height:180px;' src='" + QRCoder + "'/>");
                sb.Append("</td></tr>");
                sb.Append("</table>");
                sb.Append("</div>");





                // string  printContents = '<html><head><title></title><meta http-equiv="Content-Type" content="text/html; charset=utf-8" /></head><body><div style="padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;" ><center></center></div><br />' + sb + '</body></html>';

                //string InpuData = sb.ToString();

                //string PageType = "Portrait";
                //var pdfDoc = new NReco.PdfGenerator.HtmlToPdfConverter(); //created an object of HtmlToPdfConverter class.
                //if (string.IsNullOrEmpty(PageType))
                //    pdfDoc.Orientation = PageOrientation.Default;  //setting orientation.
                //else
                //{
                //    if (PageType == "Landscape")
                //        pdfDoc.Orientation = PageOrientation.Landscape;
                //    else
                //        pdfDoc.Orientation = PageOrientation.Portrait;
                //}

                //pdfDoc.Size = NReco.PdfGenerator.PageSize.A4;   //8.27 in × 11.02 in //Page Size
                //PageMargins pageMargins = new PageMargins();     //Margins in mm
                //pageMargins.Bottom = 05;
                //pageMargins.Left = 05;
                //pageMargins.Right = 05;
                //pageMargins.Top = 05;
                //pdfDoc.Margins = pageMargins;                      //margins added to PDF.

                //pdfDoc.PageHeaderHtml = " ";

                //var htmlContent = InpuData;
                //pdfDoc.PageFooterHtml = "<div style='float:right;'>  Page <span class='page'></span> of  <span class='topage'></span></div>";



                StringBuilder sb1 = new StringBuilder();
                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));

                //COMMON_FUNCTION.GeneratePDF(sb1, Server.MapPath("../Content/Training/" + result.ToString() + ".pdf"));

                if (!System.IO.Directory.Exists(Server.MapPath("~/Content/SocietyProfiling")))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Content/SocietyProfiling"));
                }

                if (!System.IO.Directory.Exists(Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraion.ToString())))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraion.ToString()));
                }

                if (COMMON_FUNCTION.GeneratePDF(sb1, Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraion.ToString() + "/" + fileName)))
                {
                    //  var result1 = COMMON_FUNCTION.EsignPDF_NEW(OtpResult.data, Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraion.ToString() + "/" + fileName), lbl_AO_ADD_DistrictEn, "REGISTRAR", "350", "170", "150", "170");
                    var result1 = COMMON_FUNCTION.EsignPDF_NEW(OtpResult.data, Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraion.ToString() + "/" + fileName), lbl_AO_ADD_DistrictEn, "REGISTRAR", "350", "170", "150", "170");


                    if (result1.data == "Success")
                    {

                        SocietyModel SM = new SocietyModel();
                        SocietyRegistrationEntity _EntitySRE = new SocietyRegistrationEntity();
                        _EntitySRE.SocietyIDEnc = SocietyIDEnc;
                        _EntitySRE.flag = 2;
                        _EntitySRE.BankOpeningCertificate = fileName;
                        result = Convert.ToInt32(SM.SaveBankDetailmodel(_EntitySRE));
                        if (result > 0)
                        {
                            return "Success";
                        }
                        else
                        {
                            return result.ToString();
                        }
                    }
                    else
                    {
                        return result1.data;
                    }

                }
            }
            else
            {
                return "Success";
            }

            return result.ToString();


        }

        [HttpGet]
        public ActionResult ApplicationFlow()
        {
            return View();
        }

        //[HttpGet]
        //public ActionResult ApplicationFlowStatus(int StatusId = 0, int ActId = 0, string SocietyIDEnc = "", int SocietyID = 0, int UserId = 0, bool IsAmendment = false, string RegistrationNo = "", string SocietyName = "", int DistrictId = 0, int UserRoleMappingId = 0)
        //{
        //    ViewBag.SkipLayout = true;
        //    return View();
        //}

        [HttpGet]
        public ActionResult ApplicationFlowStatus(string SocietyIDEnc = "", int SocietyID = 0, int StatusId = 0, int ActId = 0, int DistrictId = 0, string UserSsoId = "", int UserId = 0, int UserRoleMappingId = 0, bool IsAmendment = false, string RegistrationNo = "", string SocietyName = "", string ProceesStatusTextEn = "", string ProceesStatusTextHi = "")
        {
            ViewBag.SkipLayout = true;
            return View();
        }


        [HttpGet]
        public ActionResult ApplicationFlowAmendment()
        {

            UserModel OBJUser = new UserModel(new FileLogger());
            //bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);
            bool Result = OBJUser.IsUserAuthenticate(UserId, RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.UserRegistrationRequestApprovable, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpGet]
        public ActionResult ApplicationTransfer()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            //bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);
            bool Result = OBJUser.IsUserAuthenticate(UserId, RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.UserRegistrationRequestApprovable, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }


        [HttpPost]
        public JsonResult GetUserListForSocietyAssign(int ActId, int DistrictId, int UserId, int RoleId = 0)
        {

            Users objMobileFlowModel = new Users();
            return Json(new { DataList = objMobileFlowModel.GetUserListForFileTransfer(ActId, DistrictId, UserId, RoleId) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetUserSocietyList(int ActId, int UserId)
        {

            Users objMobileFlowModel = new Users();
            return Json(new { DataList = objMobileFlowModel.GetUserSocietyList(UserId, ActId) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TransferSocerties(int FromUserId, string SocietyId, int ToUserId, string Remarks, int ActId)
        {
            string fileName = "";
            foreach (string files in Request.Files)
            {
                string pathString = "~/Content/ChangeRequest/";
                if (!Directory.Exists(Server.MapPath(pathString)))
                {
                    Directory.CreateDirectory(Server.MapPath(pathString));
                }

                HttpPostedFileBase file = Request.Files[files];
                fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));


            }
            Users objMobileFlowModel = new Users();
            return Json(new { DataList = objMobileFlowModel.TransferSocerties(FromUserId, SocietyId, ToUserId, fileName, Remarks, COMMON_FUNCTION.GetIpAddress(), SSOID, ActId) }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult ApplicationHistory()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(UserId, RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.UserRegistrationRequestApprovable, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        public ActionResult UpdateSocietyApplicationFlow()
        {
            //UserModel OBJUser = new UserModel(new FileLogger());
            //bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            //if (Result == false)
            //{
            //    return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            //}
            //else
            //{
            //    ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.UserRegistrationRequestApprovable, (Request.Cookies[SessionIdentity.Culture]).Value);
            //}
            return View();

        }

        [HttpPost]
        public JsonResult GetSocietyAppliFlowList(DataPagingModel Details, bool IsAmendment)
        {
            DataTable DT = objMobileFlowModel.GetSocietyAppliFlowList(ref Details, IsAmendment);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult GetSocietyAmendment1958FlowList(DataPagingModel Details, bool IsAmendment)
        {
            DataTable DT = objMobileFlowModel.GetSocietyAmendment1958FlowList(ref Details, IsAmendment);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult GetSocietyApplicationFlowListSearch(DataPagingModel Details, bool IsAmendment)
        {

            DataTable DT = objMobileFlowModel.GetSocietyApplicationFlowListSearch(ref Details, IsAmendment);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public JsonResult GetUserForSocietyAssign(string SocietyID)
        {
            DataTable DT = objMobileFlowModel.GetUserForSocietyAssign(SocietyID);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);


        }


        [HttpPost]
        public string GetSocietyProceesStatusList(string StatusId, string WorkOnActId, string SocietyIDEnc, int Flag = 0, bool IsAmendment = false, int AmendmentId = 0)
        {
            int StatusIdInt;
            int.TryParse(StatusId, out StatusIdInt);
            int WorkOnActIdInt;
            int.TryParse(WorkOnActId, out WorkOnActIdInt);
            return COMMON_FUNCTION.DataTableToJSONString(objMobileFlowModel.GetSocietyProceesStatusListByIds(StatusIdInt, WorkOnActIdInt, SocietyIDEnc, Flag, IsAmendment, AmendmentId));
        }

        [HttpGet]
        public string GetApplicationFlowSociety(string SocietyIDEnc)
        {
            //return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objMobileFlowModel.GetApplicationFlowSociety(SocietyIDEnc));
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objMobileFlowModel.GetApplicationFlowSociety_DirectWebPage(SocietyIDEnc));
        }

        [HttpGet]

        public string GetApplicationFlowSocietyByRegiNo(string SocietyRegiNo)
        {
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objMobileFlowModel.GetApplicationFlowSocietyByRegiNo(SocietyRegiNo));
        }

        [HttpGet]
        public string GetApplicationFlowSocietyAmendment1958(string SocietyIDEnc, int AmendmentId)
        {
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objMobileFlowModel.GetApplicationFlowSocietyAmendment1958(SocietyIDEnc, AmendmentId));
        }

        [HttpGet]
        public string GetCurrentUserId(long userId)
        {
            //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            long UserId = userId;
            return UserId.ToString();

        }

        [HttpGet]
        public string GenerateAadhaarOTP()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            return COMMON_FUNCTION.SendOtpByAadharNo(_LoggedInUser.AadharNo);
        }

        [HttpPost]
        public string VerifyAadhaarOTP(string OTP, string TXN)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            return COMMON_FUNCTION.DataTableToJSONString(COMMON_FUNCTION.ValidateOTP(TXN, _LoggedInUser.SSOID, OTP));
            //return COMMON_FUNCTION.DataTableToJSONString(COMMON_FUNCTION.ValidateOTP(TXN, "narottam.saini85", OTP));

        }

        [HttpPost]
        public string GetStatusListBySocietyRegiNo(string SocietyRegiNo)
        {
            string whereCondition = " and SocietyProceesStatusId != 17 AND WorkOnActId in (Select ActId from Societies where RegistrationNo='" + SocietyRegiNo + "')";
            return COMMON_FUNCTION.DataTableToJSONString(objMobileFlowModel.GetSocietyProceesStatusList(whereCondition));
        }


        [HttpPost]
        public string GetSocietyDetailBySocietyIDEnc(string SocietyIDEnc)
        {
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objMobileFlowModel.GetSocietyDetailBySocietyIDEnc(SocietyIDEnc));
        }
        [HttpPost]
        public string GetSocietyDetailBySocietyIDEncAmendment(string AmendmentId)
        {
            return COMMON_FUNCTION.DataTableToJSONString(objMobileFlowModel.GetSocietyDetailBySocietyIDEncAmendment(AmendmentId));
        }
        public ActionResult SocietyActionView(string SocietyIDEnc)
        {
            ViewBag.Title = "";
            if (SocietyIDEnc != null)
            {

                System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc] = SocietyIDEnc;
            }
            return View();
        }



        public ActionResult SocietyActionViewAmendment1958(string SocietyIDEnc)
        {
            // ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.SocietyApplicationView, (Request.Cookies[SessionIdentity.Culture]).Value);
            ViewBag.Title = "";
            if (SocietyIDEnc != null)
            {
                System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc] = SocietyIDEnc;
                ViewBag.AmendmentId = SocietyIDEnc.Split('~')[SocietyIDEnc.Split('~').Length - 1];
            }
            return View();
        }


        [HttpPost]
        public JsonResult UploadLoanDocument(HttpPostedFileBase RecieptPath)
        {
            try
            {
                if (RecieptPath == null)
                    return Json(new { success = false, message = "File not received" });

                string folder = Server.MapPath("~/Content/SocietyBankReciept/");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(RecieptPath.FileName);
                string filePath = Path.Combine(folder, fileName);

                RecieptPath.SaveAs(filePath);

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

                string folderPath = Server.MapPath("~/Content/SocietyBankReciept/");
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

        //old
        [HttpPost]
        public JsonResult SaveSocietyApplicationFlowStatus(String SocietyIDEnc, int StatusId, string Remark, int UserId, bool IsAmendment = false, string OTP = "", string TXN = "", string AmendmendAmount = "", string ReceiptNumber = "")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            if (_LoggedInUser == null)
            {
                return Json("LOGOUT");
            }

            if (UserId == 0)
            {
                //  UserId = Convert.ToInt32(_LoggedInUser.UserId);
            }
            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
            if (!ModelState.IsValid)
                return Json(JsonModelError.Get(ModelState.Values));


            int result = 0;
            if (StatusId == 14)
            {
                ResultMessage otpResult = new ResultMessage();
                otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                otpResult.status = "1";
                otpResult.data = TXN;
                if (otpResult.status != "1")
                {
                    return Json(otpResult.data);
                }


                string cerResult = BankOpeningCertificateGenerate(SocietyIDEnc, otpResult);
                if (cerResult == "Success")
                {
                    result = objMobileFlowModel.AddApplicationFlow(SocietyIDEnc, StatusId, Remark, UserId, IsAmendment);
                }
                else
                {
                    result = Convert.ToInt32(cerResult);
                }
                try
                {
                    var a = PendingDocEsign(otpResult);
                }
                catch (Exception ex)
                {
                    ILogger objILogger = new DBLogger();
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFLowController#" + ex.Message, FunctionName = "VerifyAadhaarOTPForEsinghAmendmen PendingDocEsign" }, ex);

                }

            }
            else
            {
                if (StatusId == 17 || StatusId == 26)
                {
                    if (objMobileFlowModel.GetActIdBySocietyId(SocietyIDEnc) == 3)
                    {
                        //NCD Integration START


                        //NCD Integration END
                    }

                }
                result = objMobileFlowModel.AddApplicationFlow(SocietyIDEnc, StatusId, Remark, UserId, IsAmendment, AmendmendAmount, ReceiptNumber);
            }

            //Pradeep 01-08-2024
            if (result == 1)
            {
                RajNiveshModel rajNivesh = new RajNiveshModel(new FileLogger());
                DataTable dtr = rajNivesh.GetSwcStatusDataBySocietyId(SocietyIDEnc);

                if (dtr != null && dtr.Rows.Count > 0)
                {
                    RajNiveshStatusRequest rajNiveshStatus = new RajNiveshStatusRequest();
                    rajNiveshStatus.TRACKERID = Convert.ToInt32(dtr.Rows[0]["SWSID"].ToString());
                    rajNiveshStatus.STATUS_CODE = Convert.ToInt32(dtr.Rows[0]["STATUS_CODE"].ToString());
                    rajNiveshStatus.REGISTRATION_NUMBER = dtr.Rows[0]["RegNo"].ToString();
                    rajNiveshStatus.REMARKS = dtr.Rows[0]["REMARKS"].ToString();
                    rajNivesh.UpdateStatus(rajNiveshStatus);
                }
            }

            return Json(result);

        }



        [HttpPost]
        public JsonResult SaveSocietyApplicationFlowAmendment1958Status(String SocietyIDEnc, int StatusId, string Remark, int UserId, int AmendmentId, string OTP = "", string TXN = "", string AmendmendAmount = "", string ReceiptNumber = "")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            if (_LoggedInUser == null)
            {
                return Json("LOGOUT");
            }

            if (UserId == 0)
            {
                //  UserId = Convert.ToInt32(_LoggedInUser.UserId);
            }
            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
            if (!ModelState.IsValid)
                return Json(JsonModelError.Get(ModelState.Values));


            int result = 0;

            result = objMobileFlowModel.AddApplicationFlowAmendment1958(SocietyIDEnc, StatusId, Remark, UserId, AmendmentId, AmendmendAmount, ReceiptNumber);

            return Json(result);

        }


        [HttpPost]
        public string SaveSocietyVerifyReportFile(SocietyRegistrationEntity _Entity)
        {
            string ReturnRegiNo = "0";
            string fileName = string.Empty;
            string ReturnJSON = "";
            string pathString = "~/Content/SocietyProfiling/";


            foreach (string files in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[files];
                fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace(" ", "").Replace("_", "").ToUpper() + Path.GetExtension(file.FileName);
                if (files.ToString() == "SocietyReportFile")
                    _Entity.SocietyReportFile = fileName;
                ReturnRegiNo = objMobileFlowModel.InsertSocietyReportFile(_Entity.SocietyIDEnc, _Entity.SocietyReportFile);
                pathString += Regex.Replace(ReturnRegiNo, "[^a-zA-Z0-9_]+", "");
                if (!System.IO.File.Exists(pathString))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(pathString));
                }
                file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));
                break;
            }
            ReturnJSON = "{\"data\":\"" + fileName.ToString() + "\",\"societydirectory\":\"" + ReturnRegiNo + "\"}";

            return ReturnJSON;

        }



        [HttpGet]
        public string CertificateGenerate(string SocietyIDEnc)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {

                    DataTable dt = objMobileFlowModel.GetScietyAdministrativeDetail(SocietyIDEnc, 1, " ");
                    string fileNameCerti = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace("_", "").Replace(" ", "").ToUpper() + ".pdf";
                    string fileNameMemList = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace("_", "").Replace(" ", "").ToUpper() + ".pdf";
                    if (dt.Rows.Count > 0)
                    {

                        string lbl_InstitutionName = dt.Rows[0]["SocietyName"].ToString();
                        string lbl_Name = dt.Rows[0]["Name"].ToString();
                        string lbl_AO_ADD_DistrictEn = dt.Rows[0]["DISTRICT_ENG"].ToString();
                        string lbl_AO_ADD_DistrictHi = dt.Rows[0]["DISTRICT_MANGAL"].ToString();
                        string lbl_Registraionno = dt.Rows[0]["RegistrationNo"].ToString();
                        string lbl_ActId = dt.Rows[0]["ActId"].ToString();
                        string lbl_SocietyDirectory = dt.Rows[0]["SocietyDirectory"].ToString();
                        string lbl_OldRegistrationNo = dt.Rows[0]["OldRegistrationNo"].ToString();
                        string lbl_ByLows = dt.Rows[0]["OldRegistrationNo"].ToString();
                        string Objects = dt.Rows[0]["OldRegistrationNo"].ToString();
                        string lbl_RegistrationDate = dt.Rows[0]["RegistrationDateFormated"].ToString();
                        string lbl_OldName = dt.Rows[0]["OldRegistrationNo"].ToString();


                        string IsStateLevel = dt.Rows[0]["IsStateLevel"].ToString();

                        if (lbl_OldRegistrationNo == "" || (dt.Rows[0]["IsAmendmentAtProfiling"].ToString() == "True" && dt.Rows[0]["AmendmentType"].ToString().IndexOf("2") > -1))
                        {
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            string actNameEn = lbl_ActId == "1" ? ("The Rajasthan Societies Registration Act, 1958 (Rajasthan Act No. 28, 1958)").ToUpper() : lbl_ActId == "3" ? ("The Rajasthan Co-operative Societies Act, 2001").ToUpper() : lbl_ActId == "7" && IsStateLevel == "True" && lbl_Registraionno.Contains("RAJASTHAN") ? ("The Rajasthan Sports (Registration, Recognition and Regulation of Sports Associations) Act.2005").ToUpper() : lbl_ActId == "7" ? ("The Rajasthan(Registration, Recognition and Regulation of Sports Associations) Sports Act.2005").ToUpper() : "";
                            string actNameHi = lbl_ActId == "1" ? ("राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 1958 (राजस्थान एक्ट नंबर 28, 1958) ").ToUpper() : lbl_ActId == "3" ? ("राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 2001").ToUpper() : lbl_ActId == "7" && IsStateLevel == "True" && lbl_Registraionno.Contains("RAJASTHAN") ? ("राजस्थान क्रीड़ा ( संगमों का रजिस्ट्रीकरण मान्यता और विनियमन ) अधिनियम , 2005 ").ToUpper() : lbl_ActId == "7" ? ("राजस्थान ( क्रीड़ा संगमों का रजिस्ट्रीकरण मान्यता और विनियमन ) खेल अधिनियम , 2005 ").ToUpper() : "";



                            string devFontSize = "17px";
                            string fontSize = "font-size: 13px;";

                            sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:50px'>");
                            sb.Append("<table style='width: 100%;border:1px solid;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px'>");
                            sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                            sb.Append("<img style='width:90px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                            sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:32px;'>" + UnicodeToKrutidev.GetKrutidev("सहकारिता विभाग") + "</label><label  style='font-size:25px;'>/ COOPERATIVE DEPARTMENT </label></b>");
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");

                            if (lbl_ActId == "1" || lbl_ActId == "7")
                            {
                                sb.Append("<b><label style='font-size:22px;'> " + UnicodeToKrutidev.GetKrutidev("रजिस्ट्रीकरण प्रमाण - पत्र") + " </label><label  style='font-size:15px;'>/ REGISTRATION CERTIFICATE</label></b>");
                            }
                            else
                            {
                                sb.Append("<b><label style='font-size:22px;'> " + UnicodeToKrutidev.GetKrutidev("पंजीयन प्रमाण - पत्र ") + " </label><label  style='font-size:15px;'>/ CERTIFICATE OF REGISTRATION </label></b>");
                            }
                            sb.Append("</td>");
                            sb.Append("</tr>");
                            sb.Append("<tr>");
                            sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
                            sb.Append("<table style='width:100%'>");
                            sb.Append("<tr>");
                            sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'><b> पं. संख्या / REG. NO.- " + lbl_Registraionno + "</b></td>");
                            sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> दिनांक / DATE- " + DateTime.Now.ToString("dd-MM-yyyy") + " </td>");
                            sb.Append("</tr>");
                            sb.Append("</table>");
                            sb.Append("</td>");
                            sb.Append("</tr>");

                            //sb.Append("<td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'> यह प्रमाणित किया जाता है कि <b> " + lbl_InstitutionName.ToUpper() + " </b> जिला <b> " + lbl_AO_ADD_DistrictEn.ToUpper() + " </b>  का रजिस्ट्रेशन <b>' " + actNameHi + " '</b>  के अन्तर्गत आज किया गया है। यह प्रमाण पत्र मेरे डिजिटल हस्ताक्षरों से आज जारी किया गया है। </td>");

                            if (lbl_ActId == "1" || lbl_ActId == "7")
                            {


                                if ((dt.Rows[0]["IsAmendmentAtProfiling"].ToString() == "True" && dt.Rows[0]["AmendmentType"].ToString().IndexOf("2") > -1))
                                {
                                    sb.Append("<tr><td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'><p style='text-align: justify;text-justify;'>  यह प्रमाणित किया जाता है कि <b> " + lbl_OldName.ToUpper() + " </b> जिला <b> " + lbl_AO_ADD_DistrictEn.ToUpper() + " </b>  का रजिस्ट्रेशन <b>' " + actNameHi + " '</b>  के अन्तर्गत दिनांक <b>" + lbl_RegistrationDate + "</b> को किया गया था। </p>");
                                    sb.Append("<p style='text-align: justify;text-justify;'> अब इस संस्थान का नाम परिवर्तित कर <b> " + lbl_InstitutionName.ToUpper() + " </b> जिला <b> " + lbl_AO_ADD_DistrictEn.ToUpper() + " </b>  कर दिया गया है। यह परिवर्तन राजस्थान संस्था रजिस्ट्रीकरण अधिनियम ," + (lbl_ActId == "1" ? "1958" : lbl_ActId == "3" ? "2001" : "2005") + " की धारा -12 के प्रावधानान्तर्गत किया गया है। </p>");
                                    sb.Append("<p style='text-align: justify;text-justify;'> यह प्रमाण पत्र मेरे हस्ताक्षर एवं कार्यालय की मुहर से दिनांक <b>" + DateTime.Now.ToString("dd-MM-yyyy") + "</b> को " + lbl_AO_ADD_DistrictHi + " में जारी किया गया। </p></td></tr>");

                                }
                                else
                                {
                                    sb.Append("<tr><td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'><p style='text-align: justify;text-justify;'>  यह प्रमाणित किया जाता है कि <b> " + lbl_InstitutionName.ToUpper() + " </b> जिला <b> " + lbl_AO_ADD_DistrictEn.ToUpper() + " </b>  का रजिस्ट्रेशन <b>' " + actNameHi + " '</b>  के अन्तर्गत आज किया गया है। यह प्रमाण पत्र मेरे डिजिटल हस्ताक्षरों से आज जारी किया गया है।  </p></td></tr>");
                                    sb.Append("<tr><td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'><p style='text-align: justify;text-justify;'>  IT IS CERTIFIED THAT <b>" + lbl_InstitutionName.ToUpper() + "</b> AT  DISTRICT  <b>" + lbl_AO_ADD_DistrictEn.ToUpper() + "</b> IS REGISTERED UNDER  <b>'" + actNameEn + "'</b>. THIS CERTIFICATE IS ISSUED TODAY UNDER MY DIGITAL SIGNATURE. </p></td></tr>");
                                    //sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/></td></tr>");
                                }


                            }
                            else if (lbl_ActId == "3")
                            {

                                sb.Append("<tr><td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'><p style='text-align: justify;text-justify;'> प्रमाणित किया जाता है कि  श्री " + lbl_Name.ToUpper() + "  द्वारा " + lbl_InstitutionName.ToUpper() + "  के नाम से एक सहकारी सोसाइटी  के रजिस्ट्रेशन हेतु प्रस्तुत आवेदन को स्वीकार करते हुए उक्त  सोसाइटी एवं  उसके द्वारा अंगीकृत उपविधियों का राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 2001  की धारा 6 के अंतर्गत  आज  दिनांक " + DateTime.Now.ToString("dd-MM-yyyy") + "  को क्रमांक  " + lbl_Registraionno + "  पर  रजिस्ट्रीकरण किया गया । यह  प्रमाण पत्र  मेरी  मुद्रा  एवं हस्ताक्षर सहित आज दिनांक " + DateTime.Now.ToString("dd-MM-yyyy") + "  को प्रसारित  किया  जाता  है । </p></td></tr>");
                                sb.Append("<tr><td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'><p style='text-align: justify;text-justify;'> " + ("Certified that accepting the application made by Mr " + lbl_Name.ToUpper() + " for the registration of a co-operative society under the name " + lbl_InstitutionName.ToUpper() + ", the said society alongwith the bye - laws adopted by it, is registered at number " + lbl_Registraionno + " on dated " + DateTime.Now.ToString("dd-MM-yyyy") + " under section 6 of The Rajasthan Co-operative Societies Act, 2001.This registration certificate is issued under my seal and signatures on date " + DateTime.Now.ToString("dd-MM-yyyy")).ToUpper() + " </p></td></tr>");
                                //sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'><br/><br/><br/><br/><br/><br/><br/></td></tr>");
                            }

                            //sb.Append("<tr><td style='padding: 15px; line-height: 30px; font-size: 12px; font-weight: normal; height: 100px;'>IT IS CERTIFIED THAT <b>" + lbl_InstitutionName.ToUpper() + "</b> AT  DISTRICT  <b>" + lbl_AO_ADD_DistrictEn.ToUpper() + "</b> IS REGISTERED UNDER  <b>'" + actNameEn + "'</b>. THIS CERTIFICATE IS ISSUED TODAY UNDER MY DIGITAL SIGNATURE.</td></tr>");
                            //sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'><br/><br/><br/><br/><br/><br/><br/></td></tr>");
                            sb.Append("<tr><td style='text-align: left;'>");
                            string QRCoder = COMMON_FUNCTION.GenerateQRCode("https://rajsahakarapp.rajasthan.gov.in/Content/SocietyProfiling/" + lbl_Registraionno.Replace("-", "").Replace("/", "").Replace("_", "").Replace(" ", "") + "/" + fileNameCerti);
                            sb.Append("<img style='width:180px:height:180px;' src='" + QRCoder + "'/>");
                            sb.Append("</td></tr>");
                            sb.Append("</table>");
                            sb.Append("</div>");
                            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                            sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));
                            string filepath = System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraionno.Replace("/", "").Replace("_", "").Replace("-", "").Replace(" ", "") + "/") + fileNameCerti;

                            if (Request.Url.Host.Contains("localhost"))
                            {
                                if (!COMMON_FUNCTION.GeneratePDF1(sb1, filepath, ""))
                                {
                                    throw new Exception("Error in Generate Certificate");
                                }
                                ;
                            }
                            else
                            {
                                if (!COMMON_FUNCTION.GeneratePDF(sb1, filepath, ""))
                                {
                                    throw new Exception("Error in Generate Certificate");
                                }
                                ;
                            }


                        }
                        else
                            fileNameCerti = dt.Rows[0]["RegistrationCertificateFile"].ToString();

                        DataTable dt1 = objMobileFlowModel.GetScietyAdministrativeDetail(SocietyIDEnc, 2, "");//ad.UserType in (4
                        ILogger objILogger = new DBLogger();
                        //Exception httpException = new Exception();
                        //objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "CertificateGenerate 0 " }, httpException);

                        if (dt1.Rows.Count > 0)
                        {
                            //dt = dt1;
                            //objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "CertificateGenerate 1 " }, httpException);
                            string memberResult = generatePDFformember(dt1, System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraionno.Replace("/", "").Replace("_", "").Replace("-", "").Replace(" ", "") + "/") + fileNameMemList, lbl_InstitutionName, lbl_AO_ADD_DistrictEn, lbl_AO_ADD_DistrictHi, lbl_Registraionno, lbl_ActId, IsStateLevel);
                            if (memberResult == "Fail")
                            {
                                return "Fail";
                            }
                            //objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "CertificateGenerate 2 " }, httpException);
                        }
                        else
                        {
                            fileNameMemList = "";
                        }
                        //objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "CertificateGenerate 3 " }, httpException);

                        string BylawsStr = objMobileFlowModel.GetSocietyBylawsFile(SocietyIDEnc);

                        if (BylawsStr == "Fail" || BylawsStr == "-1")
                        {
                            return "Fail";
                        }


                        if (lbl_OldRegistrationNo == "")
                            if (fileNameMemList != "")
                                return lbl_SocietyDirectory + "/" + fileNameCerti + "#" + lbl_SocietyDirectory + "/" + fileNameMemList + "#" + lbl_SocietyDirectory + "/" + BylawsStr + "$" + lbl_AO_ADD_DistrictEn;
                            else
                                return lbl_SocietyDirectory + "/" + fileNameCerti + "#" + lbl_SocietyDirectory + "/" + BylawsStr + "$" + lbl_AO_ADD_DistrictEn;
                        else
                            return ""; // lbl_SocietyDirectory + "/" + fileNameMemList + "#" + lbl_SocietyDirectory + "/" + BylawsStr;

                    }
                    else
                    {
                        return "Fail,-2";
                    }
                }
                else
                {
                    return "Fail";
                    // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "hideloading();popupSessionExpier();", true);
                }
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "CertificateGenerate catch " }, ex);

                return "Fail";
            }
        }

        [HttpGet]
        public string CertificateGenerateAmendment(ref string CirtFile, ref string MemberFile, ref string bylawsFile, string SocietyIDEnc, int AmendmentId)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {


                    DataTable dtAmendmentChanges = objMobileFlowModel.GetAmendmentChanges(SocietyIDEnc, AmendmentId);
                    string fileNameCerti = "";
                    string fileNameMemList = "";
                    string lbl_SocietyDirectory = "";
                    string lbl_AO_ADD_DistrictEn = "";

                    string lbl_InstitutionName = "";
                    string lbl_AO_ADD_DistrictHi = "";
                    string lbl_Registraionno = "";
                    string lbl_ActId = "";
                    string lbl_OldName = "";
                    string BylawsStr = "";
                    string lbl_RegistrationDate = "";
                    if (dtAmendmentChanges.Rows.Count > 0)
                    {
                        lbl_Registraionno = dtAmendmentChanges.Rows[0]["RegistrationNo"].ToString();
                        try
                        {
                            if (lbl_Registraionno.Replace(" ", "").Replace("/", "").Replace("_", "") != lbl_Registraionno.Replace(" ", "").Replace("/", "").Replace("-", "").Replace("_", ""))
                            {
                                //System.IO.File.Move(, Server.MapPath("/Content/SocietyProfiling/" + lbl_Registraionno.Replace(" ", "").Replace("/", "").Replace("-", "") + "/4B15AD7AF60C4130AEAA57BCA1ACFA75.pdf"));
                                string SourcePath = Server.MapPath("/Content/SocietyProfiling/" + lbl_Registraionno.Replace(" ", "").Replace("/", "").Replace("-", "").Replace("_", ""));
                                string DestinationPath = Server.MapPath("/Content/SocietyProfiling/" + lbl_Registraionno.Replace(" ", "").Replace("/", "").Replace("-", "").Replace("_", ""));

                                if (!System.IO.Directory.Exists(Server.MapPath("/Content/SocietyProfiling/" + lbl_Registraionno.Replace(" ", "").Replace("/", "").Replace("-", "").Replace("_", ""))))
                                {
                                    System.IO.Directory.CreateDirectory(Server.MapPath("/Content/SocietyProfiling/" + lbl_Registraionno.Replace(" ", "").Replace("/", "").Replace("-", "").Replace("_", "")));
                                }

                                foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
                                {
                                    System.IO.File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);
                                }

                            }
                        }
                        catch (Exception) { }



                        lbl_SocietyDirectory = dtAmendmentChanges.Rows[0]["SocietyDirectory"].ToString();
                        lbl_AO_ADD_DistrictEn = dtAmendmentChanges.Rows[0]["DISTRICT_ENG"].ToString();
                        lbl_InstitutionName = dtAmendmentChanges.Rows[0]["Name"].ToString();
                        lbl_AO_ADD_DistrictHi = dtAmendmentChanges.Rows[0]["DISTRICT_MANGAL"].ToString();
                        lbl_Registraionno = dtAmendmentChanges.Rows[0]["RegistrationNo"].ToString();
                        lbl_ActId = dtAmendmentChanges.Rows[0]["ActId"].ToString();
                        lbl_OldName = dtAmendmentChanges.Rows[0]["SocietyName"].ToString();
                        lbl_RegistrationDate = dtAmendmentChanges.Rows[0]["RegistrationDateFormated"].ToString();


                        BylawsStr = dtAmendmentChanges.Rows[0]["ByeLawsFileURLForEsign"].ToString();
                        if (dtAmendmentChanges.Rows[0]["Name"] != null && dtAmendmentChanges.Rows[0]["Name"].ToString() != "")
                        {
                            //DataTable dt = objMobileFlowModel.GetScietyAdministrativeDetail(SocietyIDEnc, 1, " ");
                            // if (dt.Rows.Count > 0)
                            //{
                            //DataTable tblFiltered = dt.AsEnumerable().Where(row => row.Field<string>("POSTNAME").ToUpper() == "SECRETARY").CopyToDataTable();
                            //lbl_InstitutionName = dt.Rows[0]["SocietyName"].ToString();
                            //string lbl_Name = dtAmendmentChanges.Rows[0]["Name"].ToString();
                            lbl_AO_ADD_DistrictEn = dtAmendmentChanges.Rows[0]["DISTRICT_ENG"].ToString();
                            lbl_AO_ADD_DistrictHi = dtAmendmentChanges.Rows[0]["DISTRICT_MANGAL"].ToString();
                            lbl_Registraionno = dtAmendmentChanges.Rows[0]["RegistrationNo"].ToString();
                            lbl_ActId = dtAmendmentChanges.Rows[0]["ActId"].ToString();
                            //string lbl_SocietyDirectory = dt.Rows[0]["SocietyDirectory"].ToString();
                            //string lbl_OldRegistrationNo = dtAmendmentChanges.Rows[0]["OldRegistrationNo"].ToString();
                            //string lbl_ByLows = dtAmendmentChanges.Rows[0]["OldRegistrationNo"].ToString();
                            //string Objects = dtAmendmentChanges.Rows[0]["OldRegistrationNo"].ToString();
                            fileNameCerti = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace("_", "").ToUpper() + ".pdf";
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            string actNameEn = lbl_ActId == "1" ? ("The Rajasthan Societies Registration Act, 1958 (Rajasthan Act No. 28, 1958)").ToUpper() : lbl_ActId == "3" ? ("The Rajasthan Co-operative Societies Act, 2001").ToUpper() : lbl_ActId == "7" ? ("The Rajasthan Societies Registration Act, 2005").ToUpper() : "";
                            string actNameHi = lbl_ActId == "1" ? ("राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 1958 (राजस्थान अधिनियम संख्या 28, 1958) ").ToUpper() : lbl_ActId == "3" ? ("राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 2001").ToUpper() : lbl_ActId == "7" ? ("राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 2005").ToUpper() : "";
                            string devFontSize = "17px";
                            string fontSize = "font-size: 13px;";

                            sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:50px'>");
                            sb.Append("<table style='width: 100%;border:1px solid;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px'>");
                            sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                            sb.Append("<img style='width:90px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                            sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:32px;'>" + UnicodeToKrutidev.GetKrutidev("सहकारिता विभाग") + "</label></b>");
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");

                            //if (lbl_ActId == "1" || lbl_ActId == "7")
                            // {
                            sb.Append("<b><label style='font-size:22px;'> " + UnicodeToKrutidev.GetKrutidev("रजिस्ट्रीकरण प्रमाण - पत्र") + " </label></b>");
                            //}
                            //else
                            //{
                            //    sb.Append("<b><label style='font-size:22px;'> " + UnicodeToKrutidev.GetKrutidev("पंजीयन प्रमाण - पत्र ") + " </label></b>");
                            //}
                            sb.Append("</td>");
                            sb.Append("</tr>");
                            sb.Append("<tr>");
                            sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
                            sb.Append("<table style='width:100%'>");
                            sb.Append("<tr>");
                            sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'><b> पं. संख्या " + lbl_Registraionno + "</b></td>");
                            sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> दिनांक - " + DateTime.Now.ToString("dd-MM-yyyy") + " </td>");
                            sb.Append("</tr>");
                            sb.Append("</table>");
                            sb.Append("</td>");
                            sb.Append("</tr>");

                            //sb.Append("<td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'> यह प्रमाणित किया जाता है कि <b> " + lbl_InstitutionName.ToUpper() + " </b> जिला <b> " + lbl_AO_ADD_DistrictEn.ToUpper() + " </b>  का रजिस्ट्रेशन <b>' " + actNameHi + " '</b>  के अन्तर्गत आज किया गया है। यह प्रमाण पत्र मेरे डिजिटल हस्ताक्षरों से आज जारी किया गया है। </td>");

                            //if (lbl_ActId == "1" || lbl_ActId == "7")
                            //{
                            sb.Append("<tr><td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'><p style='text-align: justify;text-justify;'>  यह प्रमाणित किया जाता है कि <b> " + lbl_OldName.ToUpper() + " </b> जिला <b> " + lbl_AO_ADD_DistrictHi.ToUpper() + " </b>  का रजिस्ट्रेशन <b>' " + actNameHi + " '</b>  के अन्तर्गत दिनांक <b>" + lbl_RegistrationDate + "</b> को किया गया था। </p>");
                            sb.Append("<p style='text-align: justify;text-justify;'> अब इस संस्थान का नाम परिवर्तित कर <b> " + lbl_InstitutionName.ToUpper() + " </b> जिला <b> " + lbl_AO_ADD_DistrictHi.ToUpper() + " </b>  कर दिया गया है। यह परिवर्तन राजस्थान संस्था रजिस्ट्रीकरण अधिनियम ," + (lbl_ActId == "1" ? "1958" : lbl_ActId == "3" ? "2001" : "2005") + " की धारा -12 के प्रावधानान्तर्गत किया गया है। </p>");
                            sb.Append("<p style='text-align: justify;text-justify;'> यह प्रमाण पत्र मेरे हस्ताक्षर एवं कार्यालय की मुहर से दिनांक <b>" + DateTime.Now.ToString("dd-MM-yyyy") + "</b> को " + lbl_AO_ADD_DistrictHi + " में जारी किया गया। </p></td></tr>");
                            //}
                            //else if (lbl_ActId == "3")
                            //{

                            //    sb.Append("<tr><td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'><p style='text-align: justify;text-justify;'> प्रमाणित किया जाता है कि  श्री " + lbl_Name.ToUpper() + "  द्वारा " + lbl_InstitutionName.ToUpper() + "  के नाम से एक सहकारी सोसाइटी  के रजिस्ट्रेशन हेतु प्रस्तुत आवेदन को स्वीकार करते हुए उक्त  सोसाइटी एवं  उसके द्वारा अंगीकृत उपविधियों का राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 2001  की धारा 6 के अंतर्गत  आज  दिनांक " + DateTime.Now.ToString("dd-MM-yyyy") + "  को क्रमांक  " + lbl_Registraionno + "  पर  रजिस्ट्रीकरण किया गया । यह  प्रमाण पत्र  मेरी  मुद्रा  एवं हस्ताक्षर सहित आज दिनांक " + DateTime.Now.ToString("dd-MM-yyyy") + "  को प्रसारित  किया  जाता  है । </p></td></tr>");
                            //    sb.Append("<tr><td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'><p style='text-align: justify;text-justify;'> " + ("Certified that accepting the application made by Mr " + lbl_Name.ToUpper() + " for the registration of a co-operative society under the name " + lbl_InstitutionName.ToUpper() + ", the said society alongwith the bye - laws adopted by it, is registered at number " + lbl_Registraionno + " on dated " + DateTime.Now.ToString("dd-MM-yyyy") + " under section 6 of The Rajasthan Co-operative Societies Act, 2001.This registration certificate is issued under my seal and signatures on date " + DateTime.Now.ToString("dd-MM-yyyy")).ToUpper() + " </p></td></tr>");
                            //    sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'><br/><br/><br/><br/><br/><br/><br/></td></tr>");
                            //}

                            //sb.Append("<tr><td style='padding: 15px; line-height: 30px; font-size: 12px; font-weight: normal; height: 100px;'>IT IS CERTIFIED THAT <b>" + lbl_InstitutionName.ToUpper() + "</b> AT  DISTRICT  <b>" + lbl_AO_ADD_DistrictEn.ToUpper() + "</b> IS REGISTERED UNDER  <b>'" + actNameEn + "'</b>. THIS CERTIFICATE IS ISSUED TODAY UNDER MY DIGITAL SIGNATURE.</td></tr>");
                            sb.Append("<tr><td style='text-align: left;'>");
                            string QRCoder = COMMON_FUNCTION.GenerateQRCode("https://rajsahakarapp.rajasthan.gov.in/Content/SocietyProfiling/" + lbl_Registraionno.Replace("-", "").Replace("/", "").Replace("_", "").Replace(" ", "") + "/" + fileNameCerti);
                            sb.Append("<img style='width:180px:height:180px;' src='" + QRCoder + "'/>");
                            sb.Append("</td></tr>");
                            sb.Append("</table>");
                            sb.Append("</div>");
                            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                            sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));
                            //if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraionno.Replace("-", "").Replace("/", "").Replace("_", "") + "/")))
                            //{
                            //    System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraionno.Replace("-", "").Replace("/", "").Replace("_", "") + "/"));
                            //}
                            //string filepath = System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraionno.Replace("-", "").Replace("/", "") + "/").Replace("_", "") + fileNameCerti;
                            //if (!COMMON_FUNCTION.GeneratePDF(sb1, filepath, ""))
                            //{
                            //    throw new Exception("Error in Generate Certificate");
                            //};   
                            if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraionno.Replace("-", "").Replace("/", "").Replace("_", "").Replace(" ", "") + "/")))
                            {
                                System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraionno.Replace("-", "").Replace("/", "").Replace("_", "").Replace(" ", "") + "/"));
                            }
                            string filepath = System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraionno.Replace("-", "").Replace("/", "") + "/").Replace("_", "").Replace(" ", "") + fileNameCerti;
                            if (!COMMON_FUNCTION.GeneratePDF(sb1, filepath, ""))
                            {
                                throw new Exception("Error in Generate Certificate");
                            }
                            ;
                        }
                    }
                    DataTable dt1 = objMobileFlowModel.GetAdministrativeDetailsAmendment(SocietyIDEnc, "", AmendmentId);//ad.UserType in (4
                    string memberResult = "";
                    if (dt1.Rows.Count > 0)
                    {
                        lbl_SocietyDirectory = dt1.Rows[0]["SocietyDirectory"].ToString();
                        lbl_AO_ADD_DistrictEn = dt1.Rows[0]["DISTRICT_ENG"].ToString();
                        lbl_InstitutionName = dt1.Rows[0]["SocietyName"].ToString();
                        lbl_AO_ADD_DistrictHi = dt1.Rows[0]["DISTRICT_MANGAL"].ToString();
                        lbl_Registraionno = dt1.Rows[0]["RegistrationNo"].ToString();
                        lbl_ActId = dt1.Rows[0]["ActId"].ToString();
                        fileNameMemList = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace("_", "").ToUpper() + ".pdf";
                        //memberResult = generatePDFformemberAmendment(dt1, System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraionno.Replace("-", "").Replace("/", "").Replace("_", "") + "/") + fileNameMemList, lbl_InstitutionName, lbl_AO_ADD_DistrictEn, lbl_AO_ADD_DistrictHi, lbl_Registraionno, lbl_ActId);
                        memberResult = generatePDFformemberAmendment(dt1, System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraionno.Replace("-", "").Replace("/", "").Replace("_", "").Replace(" ", "") + "/") + fileNameMemList, lbl_InstitutionName, lbl_AO_ADD_DistrictEn, lbl_AO_ADD_DistrictHi, lbl_Registraionno, lbl_ActId);
                        if (memberResult == "Fail")
                        {
                            return "Fail";
                        }
                    }
                    else
                    {
                        fileNameMemList = "";
                    }



                    string result = "";
                    CirtFile = ""; MemberFile = "";
                    lbl_SocietyDirectory = lbl_SocietyDirectory.Replace("-", "");
                    if (!string.IsNullOrWhiteSpace(fileNameCerti))
                    {
                        CirtFile = fileNameCerti;
                        result = (result != "" ? "#" : "") + lbl_SocietyDirectory + "/" + fileNameCerti;
                    }
                    if (!string.IsNullOrWhiteSpace(BylawsStr))
                    {
                        result += (result != "" ? "#" : "") + lbl_SocietyDirectory + "/" + BylawsStr;
                        bylawsFile = BylawsStr;
                    }

                    if (!string.IsNullOrWhiteSpace(fileNameMemList) && memberResult == "Success")
                    {
                        MemberFile = fileNameMemList;
                        result += (result != "" ? "#" : "") + lbl_SocietyDirectory + "/" + fileNameMemList;
                    }
                    return result += "$" + lbl_AO_ADD_DistrictEn;

                }

                else
                {
                    return "Fail";
                }

            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "CertificateGenerate catch " }, ex);

                return "Fail";
            }
        }


        public string SaveCertificate(string SocietyIDEnc, string fileNameCerti, string fileNameMemList, bool isNewRegistration)
        {

            return objMobileFlowModel.Addcertificate(SocietyIDEnc, fileNameCerti, fileNameMemList, isNewRegistration).ToString();

        }

        public string generatePDFformember(DataTable dt, string filepath, string lbl_InstitutionName, string lbl_AO_ADD_DistrictEn, string lbl_AO_ADD_DistrictHi, string lbl_Registraionno, string actId, string IsStateLevel)
        {
            ILogger objILogger = new DBLogger();
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            StringBuilder sb = new StringBuilder();
            try
            {
                // Exception httpException = new Exception();
                //objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "generatePDFformember 0 " }, httpException);

                int RowNo = 1;
                if (dt.Rows.Count > 0)
                {


                    string actNameEn = actId == "1" ? ("The Rajasthan Societies Registration Act, 1958 (Rajasthan Act No. 28, 1958)").ToUpper() : actId == "3" ? ("The Rajasthan Co-operative Societies Act, 2001").ToUpper() : actId == "7" && IsStateLevel == "True" && lbl_Registraionno.Contains("RAJASTHAN") ? ("The Rajasthan Sports (Registration, Recognition and Regulation of Sports Associations) Act.2005").ToUpper() : actId == "7" ? ("The Rajasthan Societies Registration Act, 2005").ToUpper() : "";
                    string actNameHi = actId == "1" ? ("राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 1958 (राजस्थान एक्ट नंबर 28, 1958) ").ToUpper() : actId == "3" ? ("राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 2001").ToUpper() : actId == "7" && IsStateLevel == "True" && lbl_Registraionno.Contains("RAJASTHAN") ? ("राजस्थान क्रीड़ा ( संगमों का रजिस्ट्रीकरण मान्यता और विनियमन ) अधिनियम , 2005 ").ToUpper() : actId == "7" ? ("राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 2005").ToUpper() : "";
                    string devFontSize = "17px";
                    string fontSize = "font-size: 13px;";

                    sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:20px'>");
                    sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:20px;margin-right:50px'>");
                    sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                    sb.Append("<img style='width:90px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                    sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:32px;'>" + UnicodeToKrutidev.GetKrutidev("सहकारिता विभाग") + "</label><label  style='font-size:25px;'>/ COOPERATIVE DEPARTMENT </label></b>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");
                    sb.Append("<b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यकारिणी प्रमाण पत्र") + "</label><label  style='font-size:15px;'>/ EXECUTIVE COMMITTEE CERTIFICATE</label></b>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
                    sb.Append("<table style='width:100%'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'><b> पं. संख्या / REG. NO.- " + lbl_Registraionno + "</b></td>");
                    sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> दिनांक / DATE- " + DateTime.Now.ToString("dd-MM-yyyy") + " </td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    if (dt.Rows[0]["IsAmendmentAtProfiling"].ToString() == "True" && dt.Rows[0]["AmendmentType"].ToString().IndexOf("4") > -1)
                    {

                        sb.Append("<tr>");
                        sb.Append("<td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'> यह प्रमाणित किया जाता है कि <b> " + lbl_InstitutionName.ToUpper() + " </b> जिला <b> " + lbl_AO_ADD_DistrictEn.ToUpper() + " </b>  का रजिस्ट्रेशन <b>' " + actNameHi + " '</b>  के अन्तर्गत पंजीकृत हैं, अब इस संस्थान की परिवर्तित कार्यकारिणी निम्नलिखित हैं। यह प्रमाण पत्र मेरे डिजिटल हस्ताक्षरों से आज जारी किया गया है।");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td style='padding: 15px; line-height: 30px; font-size: 12px; font-weight: normal; height: 100px;'>IT IS CERTIFIED THAT <b>" + lbl_InstitutionName.ToUpper() + "</b> AT  DISTRICT  <b>" + lbl_AO_ADD_DistrictEn.ToUpper() + "</b> IS REGISTERED UNDER  <b>'" + actNameEn + "'</b> HAS THE FOLLOWING MODIFIED EXECUTIVE COMMITTEE. THIS CERTIFICATE IS ISSUED TODAY UNDER MY DIGITAL SIGNATURE.</td></tr>");

                    }
                    else
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'> यह प्रमाणित किया जाता है कि <b> " + lbl_InstitutionName.ToUpper() + " </b> जिला <b> " + lbl_AO_ADD_DistrictEn.ToUpper() + " </b>  का रजिस्ट्रेशन <b>' " + actNameHi + " '</b>  के अन्तर्गत पंजीकृत हैं, में पंजीकरण के समय निम्नलिखित कार्यकारिणी हैं। यह प्रमाण पत्र मेरे डिजिटल हस्ताक्षरों से आज जारी किया गया है।");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("<tr><td style='padding: 15px; line-height: 30px; font-size: 12px; font-weight: normal; height: 100px;'>IT IS CERTIFIED THAT <b>" + lbl_InstitutionName.ToUpper() + "</b> AT  DISTRICT  <b>" + lbl_AO_ADD_DistrictEn.ToUpper() + "</b> IS REGISTERED UNDER  <b>'" + actNameEn + "'</b> HAS THE FOLLOWING EXECUTIVE COMMITTEE AT THE TIME OF REGISTRATION. THIS CERTIFICATE IS ISSUED TODAY UNDER MY DIGITAL SIGNATURE.</td></tr>");
                    }
                    sb.Append("<tr>");
                    sb.Append("<td valign='top' style = 'padding: 15px; line-height: 30px; font-size: 12px; font-weight: normal;'>");
                    sb.Append("<table cellspacing='0' cellpadding='5'  style='width: 100%;height: 100%;font-size: 12px; font-weight: normal;border-collapse: collapse; text-align:center;'>");
                    sb.Append("<tr style=''>");
                    sb.Append("<th style='border: solid 0.2px #ccc; padding:10px;'>Sl. No.</th>");
                    sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;'>Photo</th>");
                    if (dt.Rows[0]["ActId"].ToString() == "3" && dt.Rows[0]["SocietyTypeId"].ToString() == "9")
                    {
                        sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;width:150px'>Name of society</th>");
                        sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;'>Post in Society</th>");
                    }

                    sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;width:130px;'>Name</th>");
                    sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:center;width:50px'>Gender</th>");
                    sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;width:80px;'>Post</th>");
                    if (dt.Rows[0]["ActId"].ToString() != "3" && dt.Rows[0]["SocietyTypeId"].ToString() != "9")
                    {
                        sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;width:200px'>Address</th>");
                    }
                    sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;width:70px;'>DOB</th>");
                    sb.Append("</tr>");


                    foreach (DataRow row in dt.Rows)
                    {
                        string Photostr = row["Photo"].ToString();
                        System.Drawing.Image ImgFile = null;
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(Photostr)))
                        {
                            ImgFile = System.Drawing.Image.FromFile(System.Web.HttpContext.Current.Server.MapPath(Photostr));
                        }
                        else
                        {
                            ImgFile = System.Drawing.Image.FromFile(System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/Balnk.png"));
                        }
                        //objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "inner photo loop " + ImgFile }, httpException);
                        string fileImg = "data:image/png;base64," + COMMON_FUNCTION.ImageToBase64(ImgFile, ImgFile.RawFormat);

                        if (RowNo == 6)
                        {

                            sb.Append("<tr style=''>");
                            sb.Append("<th style='border: solid 0.2px #ccc; padding:10px;page-break-before:always;'>Sl. No.</th>");
                            sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;'>Photo</th>");
                            if (row["ActId"].ToString() == "3" && row["SocietyTypeId"].ToString() == "9")
                            {
                                sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;;width:150px'>Name of society</th>");
                                sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;'>Post in Society</th>");
                            }
                            sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;width:130px;'>Name</th>");
                            sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:center;width:50px;'>Gender</th>");
                            sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;width:80px;'>Post</th>");
                            if (dt.Rows[0]["ActId"].ToString() != "3" && dt.Rows[0]["SocietyTypeId"].ToString() != "9")
                            {
                                sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;width:200px'>Address</th>");
                            }
                            sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;width:70px;'>DOB</th>");
                            sb.Append("</tr>");
                        }
                        string Address = "";
                        foreach (string x in row["Address"].ToString().Split(','))
                        {
                            if (!string.IsNullOrWhiteSpace(x))
                            {
                                Address += (Address != "" ? ", " : "") + x;
                            }
                        }
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;border: solid 0.2px #ccc;'>" + RowNo.ToString() + "</td>");
                        sb.Append("<td style='padding:2px;border: solid 0.2px #ccc; border-radius:30px;'><img width='45' height='45' src='" + fileImg + "' /></td>");
                        if (row["ActId"].ToString() == "3" && row["SocietyTypeId"].ToString() == "9")
                        {
                            sb.Append("<td style='padding:2px;border: solid 0.2px #ccc;text-align:left;'>" + row["NameOfSociety"].ToString() + "</td>");
                            sb.Append("<td style='padding:2px;border: solid 0.2px #ccc;text-align:left;'>" + row["PostinSociety"].ToString() + "</td>");
                        }
                        sb.Append("<td style='padding:2px;border: solid 0.2px #ccc;text-align:left;'>" + row["Name"].ToString() + "</td>");
                        sb.Append("<td style='padding:2px;border: solid 0.2px #ccc;text-align:center;'>" + row["Gender"].ToString() + "</td>");
                        sb.Append("<td style='padding:2px;border: solid 0.2px #ccc;text-align:left;'>" + row["POSTNAME"].ToString() + " </td>");
                        if (row["ActId"].ToString() != "3" && row["SocietyTypeId"].ToString() != "9")
                        {
                            sb.Append("<td style='padding:2px;border: solid 0.2px #ccc;text-align:left;'>" + Address + "</td>");
                        }
                        sb.Append("<td style='padding:2px;border: solid 0.2px #ccc;text-align:left;'>" + row["MemberDOB"].ToString() + "</td>");
                        sb.Append("</tr>");
                        RowNo++;
                    }


                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr><td style='text-align: center;'>");
                    string QRCoder = COMMON_FUNCTION.GenerateQRCode("https://rajsahakarapp.rajasthan.gov.in/Content/SocietyProfiling/" + lbl_Registraionno.Replace("-", "").Replace("/", "").Replace("_", "").Replace(" ", "") + "/" + filepath.Split('\\')[filepath.Split('\\').Length - 1], 1);
                    sb.Append("<img style='width:180px;height:180px;' src='" + QRCoder + "'/>");
                    sb.Append("</td></tr>");
                    sb.Append("</table>");
                    //string QRCoder = COMMON_FUNCTION.GenerateQRCode("https://rajsahakarapp.rajasthan.gov.in/Content/SocietyProfiling/" + lbl_Registraionno.Replace("-", "").Replace("/", "") + "/" + filepath.Split('\\')[filepath.Split('\\').Length-1],2);
                    //sb.Append("<img style='width:100px:height:100px;' src='" + QRCoder + "'/>");            
                    sb.Append("</div>");



                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));

                    if (Request.Url.Host.Contains("localhost") && COMMON_FUNCTION.GeneratePDF1(sb1, filepath, ""))
                    {
                        return "Success";
                    }
                    else if (COMMON_FUNCTION.GeneratePDF(sb1, filepath, ""))
                    {
                        return "Success";
                    }
                    else
                    {
                        try
                        {
                            Sp_callTrackingInsert("generatePDFformember  string ", sb1.ToString());
                        }
                        catch (Exception e)
                        {

                            objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "generatePDFformember catch error. RegNo- " + lbl_Registraionno }, e);
                        }

                        return "Fail";
                    }
                    //objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "generatePDFformember 1 " }, httpException);                

                }
                else
                {
                    return "Fail";
                }
            }
            catch (Exception ex)
            {

                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "generatePDFformember catch error. RegNo- " + lbl_Registraionno + "String:- " + sb1.ToString() + "String2:-" + sb.ToString() }, ex);
                return "Fail";
            }

        }

        public string generatePDFformemberAmendment(DataTable dt, string filepath, string lbl_InstitutionName, string lbl_AO_ADD_DistrictEn, string lbl_AO_ADD_DistrictHi, string lbl_Registraionno, string actId)
        {
            ILogger objILogger = new DBLogger();
            try
            {
                Exception httpException = new Exception();
                //objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "generatePDFformember 0 " }, httpException);

                int RowNo = 1;
                if (dt.Rows.Count > 0)
                {


                    string actNameEn = actId == "1" ? ("The Rajasthan Societies Registration Act, 1958 (Rajasthan Act No. 28, 1958)").ToUpper() : actId == "3" ? ("The Rajasthan Co-operative Societies Act, 2001").ToUpper() : actId == "7" ? ("The Rajasthan Societies Registration Act, 2005").ToUpper() : "";
                    string actNameHi = actId == "1" ? ("राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 1958 (राजस्थान एक्ट नंबर 28, 1958) ").ToUpper() : actId == "3" ? ("राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 2001").ToUpper() : actId == "7" ? ("राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 2005").ToUpper() : "";
                    string devFontSize = "17px";
                    string fontSize = "font-size: 13px;";
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:20px'>");
                    sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:20px;margin-right:50px'>");
                    sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                    sb.Append("<img style='width:90px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                    sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:32px;'>" + UnicodeToKrutidev.GetKrutidev("सहकारिता विभाग") + "</label><label  style='font-size:25px;'>/ COOPERATIVE DEPARTMENT </label></b>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");
                    sb.Append("<b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यकारिणी की सूची") + "</label><label  style='font-size:15px;'>/ EXECUTIVE COMMITTEE LIST</label></b>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
                    sb.Append("<table style='width:100%'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'><b> पं. संख्या / REG. NO.- " + lbl_Registraionno + "</b></td>");
                    sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> दिनांक / DATE- " + DateTime.Now.ToString("dd-MM-yyyy") + " </td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'> <b> " + lbl_InstitutionName.ToUpper() + " </b> जिला <b> " + lbl_AO_ADD_DistrictHi + " </b>  राजस्थान सोसाइटी रजिस्ट्रीकरण अधिनियम, 1958 (राजस्थान एक्ट नंबर 28, 1958) के अन्तर्गत पंजीकृत संस्था है। इस संस्था की परिवर्तित कार्यकारिणी अधोहस्ताक्षरकर्ता के कार्यालय मे राजस्थान सोसाइटी रजिस्ट्रीकरण अधिनियम, 1958 की धारा- 4 (1) के अन्तर्गत प्रस्तुत की गई है जिसे संस्था के रिकॉर्ड मे संधारित किया जाता है। ");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr><td style='padding: 15px; line-height: 30px; font-size: 12px; font-weight: normal; height: 100px;'>  <b> " + lbl_InstitutionName.ToUpper() + " </b> DISTRICT <b> " + lbl_AO_ADD_DistrictEn + " </b> IS A REGISTERED INSTITUTION UNDER THE RAJASTHAN SOCIETIES REGISTRATION ACT, 1958 ( RAJASTHAN ACT NUMBER 28, 1958 ). THE CHANGED EXECUTIVE COMMITTEE OF THE INSTITUTION IS SUBMITTEED BEFORE THE UNDERSIGNED UNDER THE RAJASTHAN SOCIETIES REGISTRATION ACT, 1958 <br/> SECTION- 4 (1) WHICH IS FILED IN THE SOCIETY'S RECORD. </td></tr>");
                    sb.Append("<tr>");
                    sb.Append("<td valign='top' style = 'padding: 15px; line-height: 30px; font-size: 12px; font-weight: normal;'>");
                    sb.Append("<table cellspacing='0' cellpadding='5'  style='width: 100%;height: 100%;font-size: 12px; font-weight: normal;border-collapse: collapse; text-align:center;'>");
                    sb.Append("<tr style=''>");
                    sb.Append("<th style='padding:5px;border: solid 0.2px #ccc; padding:10px;'>Sl. No.</th>");
                    sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;'>Photo</th>");
                    sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;'>Name</th>");
                    sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;'>Gender</th>");
                    sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;'>Post</th>");
                    sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;width:150px'>Address</th>");
                    sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;'>DOB</th>");
                    sb.Append("</tr>");


                    foreach (DataRow row in dt.Rows)
                    {
                        string Photostr = row["Photo"].ToString();
                        System.Drawing.Image ImgFile = null;
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(Photostr)))
                        {
                            ImgFile = System.Drawing.Image.FromFile(System.Web.HttpContext.Current.Server.MapPath(Photostr));
                        }
                        else
                        {
                            ImgFile = System.Drawing.Image.FromFile(System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/Balnk.png"));
                        }
                        //objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "inner photo loop " + ImgFile }, httpException);
                        string fileImg = "data:image/png;base64," + COMMON_FUNCTION.ImageToBase64(ImgFile, ImgFile.RawFormat);
                        if (RowNo == 6)
                        {

                            sb.Append("<tr style=''>");
                            sb.Append("<th style='padding:5px;border: solid 0.2px #ccc; padding:10px;page-break-before:always;'>Sl. No.</th>");
                            sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;'>Photo</th>");
                            sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;'>Name</th>");
                            sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;'>Gender</th>");
                            sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;'>Post</th>");
                            sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;width:150px'>Address</th>");
                            sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;'>DOB</th>");
                            sb.Append("</tr>");
                        }

                        string Address = "";
                        foreach (string x in row["Address"].ToString().Split(','))
                        {
                            if (!string.IsNullOrWhiteSpace(x))
                            {
                                Address += (Address != "" ? ", " : "") + x;
                            }
                        }

                        sb.Append("<tr>");
                        sb.Append("<td style='padding:2px;border: solid 0.2px #ccc;'>" + RowNo.ToString() + "</td>");
                        sb.Append("<td style='padding:2px;border: solid 0.2px #ccc; border-radius:30px;'><img width='45' height='45' src='" + fileImg + "' /></td>");
                        sb.Append("<td style='padding:2px;border: solid 0.2px #ccc;text-align:left;'>" + row["Name"].ToString() + "</td>");
                        sb.Append("<td style='padding:2px;border: solid 0.2px #ccc;text-align:left;'>" + (row["Gender"].ToString() == "F" ? "FEMALE" : row["Gender"].ToString() == "M" ? "MALE" : row["Gender"].ToString()) + "</td>");
                        sb.Append("<td style='padding:2px;border: solid 0.2px #ccc;text-align:left;'>" + row["POSTNAME"].ToString() + " </td>");
                        sb.Append("<td style='padding:2px;border: solid 0.2px #ccc;text-align:left;width:150px'>" + Address + "</td>");
                        sb.Append("<td style='padding:2px;border: solid 0.2px #ccc;text-align:left;'>" + row["MemberDOB"].ToString() + "</td>");
                        sb.Append("</tr>");
                        RowNo++;
                    }



                    sb.Append("<tr>");
                    sb.Append("<td colspan='7' style='text-align:left;'><br/> नोट-  इसे रजिस्ट्रार द्वारा शासी निकाय / GOVERNING BODY की वैधानिकता का अनुमोदन नहीं माना जावे । </td>");

                    sb.Append("</tr>");

                    //नोट - इसे रजिस्ट्रार द्वारा शासी  निकाय /GOVERNMENT  BODY की वैधानिकता का अनुमोदन नहीं माना जावे | 
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr><td style='text-align: center;'>");
                    string QRCoder = COMMON_FUNCTION.GenerateQRCode("https://rajsahakarapp.rajasthan.gov.in/Content/SocietyProfiling/" + lbl_Registraionno.Replace("-", "").Replace("/", "").Replace("_", "").Replace(" ", "") + "/" + filepath.Split('\\')[filepath.Split('\\').Length - 1], 1);
                    sb.Append("<img style='width:180px;height:180px;' src='" + QRCoder + "'/>");
                    sb.Append("</td></tr>");
                    sb.Append("</table>");
                    sb.Append("</div>");


                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));
                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath, ""))
                    {

                        return "Success";
                    }
                    else
                    {
                        return "Fail";
                    }



                }
                else
                {
                    return "Fail";
                }
            }
            catch (Exception ex)
            {

                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "generatePDFformemberAmendment catch error" + lbl_Registraionno }, ex);
                return "Fail";
            }

        }


        public class CustomImageTagProcessor : iTextSharp.tool.xml.html.Image
        {
            public override IList<IElement> End(IWorkerContext ctx, Tag tag, IList<IElement> currentContent)
            {
                IDictionary<string, string> attributes = tag.Attributes;
                string src;
                if (!attributes.TryGetValue(HTML.Attribute.SRC, out src))
                    return new List<IElement>(1);

                if (string.IsNullOrEmpty(src))
                    return new List<IElement>(1);

                if (src.StartsWith("data:image/", StringComparison.InvariantCultureIgnoreCase))
                {
                    var base64Data = src.Substring(src.IndexOf(",") + 1);
                    var imagedata = Convert.FromBase64String(base64Data);
                    var image = iTextSharp.text.Image.GetInstance(imagedata);

                    var list = new List<IElement>();
                    var htmlPipelineContext = GetHtmlPipelineContext(ctx);
                    list.Add(GetCssAppliers().Apply(new Chunk((iTextSharp.text.Image)GetCssAppliers().Apply(image, tag, htmlPipelineContext), 0, 0, true), tag, htmlPipelineContext));
                    return list;
                }
                else
                {
                    return base.End(ctx, tag, currentContent);
                }
            }
        }

        [HttpGet]
        public string GenerateAadhaarOTPForEsing()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
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

        [HttpGet]
        public string GenerateAadhaarOTPForEsignGcc(int userType = 0, string aadharNoEnc = "")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser != null)
            {
                if (Request.Url.Host.Contains("localhost"))
                {
                    _LoggedInUser.AadharNo = "444394749686517";
                    //return "999999";
                }

                if (!string.IsNullOrEmpty(aadharNoEnc))
                {
                    aadharNoEnc = COMMON_FUNCTION.Decrypt(aadharNoEnc);
                    return COMMON_FUNCTION.GenerateOTPForEsingh(aadharNoEnc);
                }
                return COMMON_FUNCTION.GenerateOTPForEsingh(_LoggedInUser.AadharNo);
            }
            else
            {
                return "LOGOUT";
            }

            //return "RJVSD2343246566";
        }



        public string VerifyAadhaarOTPForEsing_Test(string txnId, string pdfPath)
        {
            string finalPath = "";
            finalPath = Server.MapPath("~" + pdfPath);
            //finalPath = finalPath.Replace("rajsahakarapp", "");
            //var result1 = COMMON_FUNCTION.EsinghPDF_Test(txnId, finalPath, "Madhuri", "Test");
            // var result1 = COMMON_FUNCTION.EsinghPDF(txnId, finalPath, "Madhuri", "Test");
            var result1 = COMMON_FUNCTION.EsignPDF_NEW(txnId, finalPath, "Madhuri", "Test");

            return result1.status;
        }


        [HttpPost]
        public string VerifyAadhaarOTPForEsing(string OTP, string TXN, string SocietyIDEnc, int StatusId, string Remark, bool IsAmendment)
        {
            //if (IsAmendment)
            //{
            //    return VerifyAadhaarOTPForEsinghAmendment(OTP, TXN, SocietyIDEnc, StatusId, Remark, IsAmendment);
            //}
            string message = string.Empty;
            string status = "";
            string TransactionId = "";
            string final_status = "";
            string final_document = "";
            string succmsg = string.Empty;
            string pdfPath = "";
            string openFile = "";
            ResultMessage otpResult = new ResultMessage();
            if (StatusId == 0 || Remark == "")
            {
                return "Fail";
            }

            if (TXN != "")
            {
                try
                {

                    otpResult.status = "1";
                    otpResult.data = TXN;
                    // otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                    if (otpResult.status != "1")
                    {
                        return otpResult.data;
                    }
                    if (otpResult.status == "1")
                    {
                        //DOC GEN
                        pdfPath = CertificateGenerate(SocietyIDEnc);

                        try
                        {
                            Sp_callTrackingInsert("Society_CertificateEsign", SocietyIDEnc + ": " + pdfPath);
                        }
                        catch (Exception)
                        {

                        }

                        if (!pdfPath.Contains("Fail"))
                        {
                            Exception httpException = new Exception();
                            ILogger objILogger = new DBLogger();
                            //objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "NOT  Fail" }, httpException);

                            var Filepath = pdfPath.Split('#');
                            var lbl_AO_ADD_DistrictEn = Filepath[Filepath.Length - 1].Split('$')[1];
                            Filepath[Filepath.Length - 1] = Filepath[Filepath.Length - 1].Split('$')[0];

                            string[] files = new string[Filepath.Length - 1];
                            for (int x = 1; x < Filepath.Length; x++)
                            {
                                files[x - 1] = Server.MapPath("~/Content/SocietyProfiling/") + Filepath[x];
                            }

                            var result = COMMON_FUNCTION.EsinghPDF(otpResult.data, Server.MapPath("~/Content/SocietyProfiling/") + Filepath[0], lbl_AO_ADD_DistrictEn, "REGISTRAR", "350", "120", "150", "120");
                            //  var result = COMMON_FUNCTION.EsignPDF_NEW(otpResult.data, Server.MapPath("~/Content/SocietyProfiling/") + Filepath[0], lbl_AO_ADD_DistrictEn, "REGISTRAR", "350", "120", "150", "120");

                            if (result.status == "1")
                            {
                                //
                                result = COMMON_FUNCTION.EsinghPDF(otpResult.data, string.Join("#", files), lbl_AO_ADD_DistrictEn, "REGISTRAR");
                                // result = COMMON_FUNCTION.EsignPDF_NEW(otpResult.data, string.Join("#", files), lbl_AO_ADD_DistrictEn, "REGISTRAR");

                            }
                            succmsg = result.status;

                            if (succmsg == "1")
                            {
                                var result1 = SaveSocietyApplicationFlowStatus(SocietyIDEnc, StatusId, Remark, 0);
                                if (result1.Data.ToString() != "1")
                                {
                                    return "Fail";
                                }
                                else
                                {
                                    string BylawsStr = SaveCertificate(SocietyIDEnc, Filepath[0].Split('/')[1], Filepath[1].Split('/')[1], true);
                                    if (BylawsStr == "Fail" || BylawsStr == "-1")
                                    {
                                        return "Fail";
                                    }
                                }
                                succmsg = result.data;
                            }
                        }
                        else
                        {
                            succmsg = pdfPath;
                        }

                        try
                        {
                            var a = PendingDocEsign(otpResult);
                        }
                        catch (Exception ex)
                        {
                            ILogger objILogger = new DBLogger();
                            objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFLowController#" + ex.Message, FunctionName = "VerifyAadhaarOTPForEsingh PendingDocEsign" }, ex);

                        }

                    }
                    else
                    {
                        succmsg = "Invalid OTP!";
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
        public string EsingDocument(string TXN, string SocietyIDEnc, string document)
        {
            string succmsg = "";
            if (TXN != "")
            {
                try
                {
                    //var otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                    //if (otpResult.status != "1")
                    //{
                    //return otpResult.data;
                    //}
                    // if (otpResult.status == "1")
                    // {
                    var result = COMMON_FUNCTION.EsinghPDF(TXN, Server.MapPath("~") + document.Replace("..", ""), DISTRICT_ENG, "REGISTRAR");
                    //  var result = COMMON_FUNCTION.EsignPDF_NEW(TXN, Server.MapPath("~") + document.Replace("..", ""), _LoggedInUser.DISTRICT_ENG, "REGISTRAR");

                    succmsg = result.status;
                    //}
                    //else
                    //{
                    //succmsg = "Invalid OTP!";
                    //}

                }
                catch (Exception ex)
                {
                    succmsg = ex.Message;
                }

            }
            return succmsg;
        }



        [HttpPost]
        public string VerifyAadhaarOTPForEsinghAmendment(string OTP, string TXN, string SocietyIDEnc, int StatusId, string Remark, bool IsAmendment, int AmendmentId)
        {

            string message = string.Empty;
            string status = "";
            string TransactionId = "";
            string final_status = "";
            string final_document = "";
            string succmsg = string.Empty;
            string pdfPath = "";
            string openFile = "";

            if (StatusId == 0 || Remark == "")
            {
                return "Fail";
            }

            if (TXN != "")
            {
                try
                {
                    ResultMessage otpResult = new ResultMessage();
                    otpResult.status = "1";
                    otpResult.data = TXN;
                    //var otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                    if (otpResult.status != "1")
                    {
                        return otpResult.data;
                    }
                    if (otpResult.status == "1")
                    {
                        //DOC GEN
                        string CirtFile = ""; string MemberFile = ""; string BylawsFile = "";
                        pdfPath = CertificateGenerateAmendment(ref CirtFile, ref MemberFile, ref BylawsFile, SocietyIDEnc, AmendmentId);

                        if (!pdfPath.Contains("Fail"))
                        {

                            var Filepath = pdfPath.Split('#');

                            var lbl_AO_ADD_DistrictEn = Filepath[Filepath.Length - 1].Split('$')[1];
                            //Filepath[Filepath.Length - 1] = Filepath.Split('$')[0];

                            try
                            {
                                Sp_callTrackingInsert("Society_CertificateEsignAmendment", SocietyIDEnc + ": " + pdfPath);
                            }
                            catch (Exception)
                            {

                            }
                            string[] files = new string[pdfPath.Split('$')[0].Split('#').Length];
                            for (int x = 0; x <= pdfPath.Split('$')[0].Split('#').Length - 1; x++)
                            {
                                files[x] = Server.MapPath("~/Content/SocietyProfiling/") + pdfPath.Split('$')[0].Split('#')[x];
                            }

                            //var result = COMMON_FUNCTION.EsinghPDF(otpResult.data, Server.MapPath("~/Content/SocietyProfiling/") + Filepath[0], lbl_AO_ADD_DistrictEn, "REGISTRAR", "350", "120", "150", "120");
                            //if (result.status == "1")
                            //{
                            //



                            var result = COMMON_FUNCTION.EsinghPDF(otpResult.data, string.Join("#", files), lbl_AO_ADD_DistrictEn, "REGISTRAR");
                            // var result = COMMON_FUNCTION.EsignPDF_NEW(otpResult.data, string.Join("#", files), lbl_AO_ADD_DistrictEn, "REGISTRAR");

                            //}
                            succmsg = result.status;

                            if (succmsg == "1")
                            {
                                var result1 = SaveSocietyApplicationFlowAmendment1958Status(SocietyIDEnc, StatusId, Remark, 0, AmendmentId);
                                if (result1.Data.ToString() != "1")
                                {
                                    return "Fail";
                                }
                                else
                                {
                                    string BylawsStr = objMobileFlowModel.SaveSocietyAmendmendData(SocietyIDEnc, CirtFile, MemberFile, BylawsFile, true, StatusId, AmendmentId).ToString();

                                    Exception ex = new Exception();

                                    //ILogger objILogger = new DBLogger();
                                    //objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AmendmentRequest", FunctionName = "CirtFile="+ CirtFile + ",MemberFile= " + MemberFile + ", SocietyIDEnc= "+ SocietyIDEnc+ ", StatusId="+ StatusId }, ex);


                                    //string BylawsStr = SaveCertificate(SocietyIDEnc, Filepath[0].Split('/')[1], Filepath[1].Split('/')[1], true);
                                    if (BylawsStr == "Fail" || BylawsStr == "-1")
                                    {
                                        return "Fail";
                                    }
                                }
                                succmsg = result.data;
                            }
                            try
                            {
                                var a = PendingDocEsign(otpResult);
                            }
                            catch (Exception ex)
                            {
                                ILogger objILogger = new DBLogger();
                                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFLowController#" + ex.Message, FunctionName = "VerifyAadhaarOTPForEsinghAmendmen PendingDocEsign" }, ex);

                            }



                        }
                        else
                        {
                            succmsg = pdfPath;
                        }
                    }
                    else
                    {
                        succmsg = "Invalid OTP!";
                    }

                }
                catch (Exception ex)
                {
                    succmsg = ex.Message;
                }

            }
            return succmsg;
        }


        [HttpGet]
        public JsonResult GetAmendmentChanges(string SocietyIDEnc, int AmendmentId)
        {
            DataTable dtAmendmentChanges = objMobileFlowModel.GetAmendmentChanges(SocietyIDEnc, AmendmentId);
            return Json(COMMON_FUNCTION.DataTableToJSONString(dtAmendmentChanges), JsonRequestBehavior.AllowGet);

        }
        public ActionResult SocietyActionViewAmendment(string SocietyIDEnc)
        {
            // ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.SocietyApplicationView, (Request.Cookies[SessionIdentity.Culture]).Value);
            ViewBag.Title = "";
            if (SocietyIDEnc != null)
            {
                System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc] = SocietyIDEnc;
            }
            return View();
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

        [HttpGet]
        public string GetSocietyFeesStatusList(string SocietyRegiNo)
        {
            return COMMON_FUNCTION.DataTableToJSONString(objMobileFlowModel.SocietyFeesStatuslist(SocietyRegiNo));
        }

        [HttpPost]
        public string GetRoleListForApplicationStatus(string SocietyIDEnc, int NextStatusId, int CurrentStatusId)
        {
            return COMMON_FUNCTION.DataTableToJSONString(objMobileFlowModel.GetRoleListForApplicationStatus(SocietyIDEnc, NextStatusId, CurrentStatusId));
        }

        [HttpPost]
        public string GetOfficerBySocietyIDEnc(string SocietyIDEnc, int NextStatusId, int CurrentStatusId, int RoleId)
        {
            return COMMON_FUNCTION.DataTableToJSONString(objMobileFlowModel.GetOfficerBySocietyIDEnc(SocietyIDEnc, NextStatusId, CurrentStatusId, RoleId));
        }

        [HttpPost]
        public string SaveSocietyObjectionBelong(string SocietyIDEnc, bool ObjectionFlagBasic, bool ObjectionFlagAdmin, bool ObjectionFlagOffice)
        {
            return objMobileFlowModel.SaveSocietyObjectionBelong(SocietyIDEnc, ObjectionFlagBasic, ObjectionFlagAdmin, ObjectionFlagOffice).ToString();
        }


        public ViewResult ApplicationStatus()
        {
            return View();
        }
        public ViewResult GccApplicationStatus()
        {
            return View();
        }
        public ViewResult FindApplication()
        {
            return View();
        }

        public ViewResult ApplicationView()
        {
            return View();
        }

        [HttpPost]
        public string SaveSocietyStatus(string SocietyIDEnc, int StatusId, string RegistrationNo)
        {
            return objMobileFlowModel.SaveSocietyStatusAction(SocietyIDEnc, StatusId, RegistrationNo).ToString();
        }

        [HttpPost]
        public JsonResult GetSocietyAppliFlowLists(DataPagingModel Details)
        {


            DataTable DT = objMobileFlowModel.GetSocietyAppliStatus(ref Details);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public JsonResult GetSocietyPaymentTransectin(DataPagingModel Details, string SocietyIdENC)
        {
            DataTable DT = objMobileFlowModel.GetSocietyPaymentTransectin(ref Details, SocietyIdENC);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);


        }

        public ViewResult SocietyApplicationStatus(string SocietyIDEnc)
        {
            ViewBag.Title = "";
            if (SocietyIDEnc != null)
            {
                System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc] = SocietyIDEnc;
            }
            return View();
        }

        public ViewResult SocietyApplicationView(string SocietyIDEnc)
        {
            ViewBag.Title = "";
            if (SocietyIDEnc != null)
            {
                System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc] = SocietyIDEnc;
            }
            return View();
        }

        public ViewResult ChangeApplicationStatus(string SocietyIDEnc)
        {
            ViewBag.Title = "";
            if (SocietyIDEnc != null)
            {
                System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc] = SocietyIDEnc;
            }
            return View();
        }

        [HttpPost]
        public string SetStatusApplication(string StatusId, string SocietyIDEnc, string Remark, int ApplicationFlowID, string RegistrationNo)
        {
            string fileName = "";
            foreach (string files in Request.Files)
            {
                string pathString = "~/Content/SocietyProfiling/";
                RegistrationNo = RegistrationNo.ToUpper().Replace(" ", "").Replace("-", "").Replace("_", "");
                pathString += Regex.Replace(RegistrationNo.ToUpper().Trim(), "[^a-zA-Z0-9_]+", "");
                if (!Directory.Exists(Server.MapPath(pathString)))
                {
                    Directory.CreateDirectory(Server.MapPath(pathString));
                }

                HttpPostedFileBase file = Request.Files[files];
                fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace("_", "").ToUpper() + Path.GetExtension(file.FileName);
                file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));


            }

            return objMobileFlowModel.SaveSocietystatusdata(StatusId, SocietyIDEnc, Remark, ApplicationFlowID, fileName).ToString();
        }

        [HttpPost]
        public string SaveStatusApplicationPayment(string SocietyIDEnc, string Remark, int ApplicationPaymentId, string ApplicationPayment)
        {
            return objMobileFlowModel.SaveStatusApplicationPayment(SocietyIDEnc, Remark, ApplicationPaymentId, ApplicationPayment).ToString();
        }

        [HttpPost]
        public string SetApplicationUser(string SocietyIDEnc, string Remark, int OfficeId, int ApplicationFlowID)
        {
            return objMobileFlowModel.SetApplicationUser(SocietyIDEnc, Remark, OfficeId, ApplicationFlowID).ToString();
        }

        public ViewResult CheckEsign()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetSocietyApplicantDetails(string SocietyIdENC, int AmendmentId = 0)
        {
            DataTable DT = objMobileFlowModel.GetSocietyApplicantDetails(SocietyIdENC, AmendmentId);
            return Json(COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT));
        }

        public ViewResult ApplicationDocuments()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAllSocietyDocuments(string RegistrationNo)
        {
            var lbl_Registraionno = RegistrationNo.Replace(" ", "").Replace("-", "").Replace("_", "").Replace("/", ""); ; ;// "314JAIPUR201718";
            string[] filePaths = Directory.GetFiles(Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraionno));
            List<string> dir = new List<string>();
            foreach (string x in filePaths)
            {
                string nfile = x.Split('\\')[x.Split('\\').Length - 1];
                dir.Add(lbl_Registraionno + "/" + nfile);
            }
            return Json(dir);
        }

        [HttpPost]
        public JsonResult GetSocietyDocuments(int SocietyId)
        {

            return Json((new Society()).GetAllSocietyDocuments(SocietyId));
        }

        public ViewResult SocietyStatusCheckAndUpdation()
        {
            return View();
        }


        [HttpPost]
        public string GetSocietyAppliStatuschecklist(string ApplicationNo)
        {
            DataTable DT = null;
            DT = objMobileFlowModel.GetSocietieCheckList(ApplicationNo);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }
        [HttpPost]
        public string DeleteSocietyAppliStatuschecklist(int Societyid)
        {
            return objMobileFlowModel.DeleteSocietieCheckList(Societyid).ToString();
        }


        #region AadharEsignEncryption  Final 30 dec2024

        public string GenerateAadhaarOTPEsign_Test(string callback)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser != null)
            {
                //System.Web.HttpContext.Current.Session["EsignTxnId"] = null;
                return csCommon.GetEncDataForEsign(_LoggedInUser.AadharNo);
            }
            else
            {
                return "LOGOUT";
            }
        }
        public string GenerateAadhaarOTPEsign_Test1(string callback)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser != null)
            {
                return csCommon.GetEncDataForEsign("420928155572");
            }
            else
            {
                return "LOGOUT";
            }
        }

        public string GenerateAadhaarOTPDec_Test(string encData = "")
        {

            string _txnid = "";
            if (encData == "" || encData == "undefined")
            {
                return "NO" + "#";
            }

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser != null)
            {
                string decryptedData = "";
                EncryptionResult encryptionResult = new EncryptionResult();
                EncryptionResult e = EsignAesEncryption.Decrypt_New(encData, GlobalConstant.AadharAuthEsigEncKey_Test);

                if (Request.Url.Host.Contains("10.70.234.208") || Request.Url.Host.Contains("10.68.231.168"))
                    e = EsignAesEncryption.Decrypt_New(encData, GlobalConstant.AadharAuthEsigEncKey_Test_stagging);
                else
                    e = EsignAesEncryption.Decrypt_New(encData, GlobalConstant.AadharAuthEsigEncKey_Test);

                if (e.IsSuccess)
                {
                    decryptedData = e.DecriptedData.ToString();
                }
                else
                {
                    decryptedData = "NO" + "#" + e.Message.ToString();
                    return decryptedData;
                }
                var parsedResponse = JsonConvert.DeserializeObject<DecryptedAadhaarResponse>(decryptedData);
                Sp_callTrackingInsert("GetTxnIdOtp parsedResponse :-", decryptedData);
                if (parsedResponse.ResponseCode == "200")
                {
                    _txnid = parsedResponse.RequestId;
                    _txnid.Replace("\"", "");
                }
                else
                {
                    _txnid = "NO" + "#" + parsedResponse.Message;
                }
                //  System.Web.HttpContext.Current.Session["EsignTxnId"]= _txnid;
                return _txnid;
            }
            else
            {
                return "LOGOUT";
            }
        }

        public string GenerateAadhaarOTPDec_Test1(string encData = "")
        {
            string _txnid = "";
            if (encData == "" || encData == "undefined")
            {
                return "NO" + "#";
            }

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser != null)
            {
                string decryptedData = "";
                EncryptionResult encryptionResult = new EncryptionResult();
                EncryptionResult e = EsignAesEncryption.Decrypt_New(encData, GlobalConstant.AadharAuthEsigEncKey_Test);

                if (Request.Url.Host.Contains("10.70.234.208") || Request.Url.Host.Contains("10.68.231.168"))
                    e = EsignAesEncryption.Decrypt_New(encData, GlobalConstant.AadharAuthEsigEncKey_Test_stagging);
                else
                    e = EsignAesEncryption.Decrypt_New(encData, GlobalConstant.AadharAuthEsigEncKey_Test);

                if (e.IsSuccess)
                {
                    decryptedData = e.DecriptedData.ToString();
                }
                else
                {
                    decryptedData = "NO" + "#" + e.Message.ToString();
                    return decryptedData;
                }

                var parsedResponse = JsonConvert.DeserializeObject<DecryptedAadhaarResponse>(decryptedData);
                // Sp_callTrackingInsert("GetTxnIdOtp parsedResponse :-", decryptedData);
                if (parsedResponse.ResponseCode == "200")
                {
                    ResponseDataEsign Response = new ResponseDataEsign();
                    Response = COMMON_FUNCTION.IsValidEsignOtp(parsedResponse.RequestId);
                    // Sp_callTrackingInsert("GetTxnIdOtp  decryptedData:-", Response.RequestId);
                    if (Response != null)
                    {
                        _txnid = Response.RequestId;
                        _txnid.Replace("\"", "");
                    }
                    //_txnid = parsedResponse.RequestId;
                    //_txnid.Replace("\"", "");
                }
                else
                {
                    _txnid = "NO" + "#" + parsedResponse.Message;
                }
                System.Web.HttpContext.Current.Session["EsignTxnId"] = _txnid;
                return _txnid;
            }
            else
            {
                return "LOGOUT";
            }
        }
        #endregion



        #region PendingDocEsign   

        public string PendingDocEsign(ResultMessage otpResult)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = objMobileFlowModel.GetPendingEsignDoc();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row != null && otpResult.status == "1" && row["BylawsFile"] != null && row["BylawsFile"] != "")
                        {
                            string societyDir = row["SocietyDir"].ToString();
                            string targetDirectory = Server.MapPath($"~/Content/SocietyProfiling/{societyDir}/Esign/");
                            var lbl_AO_ADD_DistrictEn = "";
                            if (!Directory.Exists(targetDirectory))
                            {
                                Directory.CreateDirectory(targetDirectory);
                            }
                            //Sp_callTrackingInsert("PendingDocEsign" + ": " + row["SocietyDir"].ToString(), row["BylawsFile"].ToString());

                            string[] files = row["BylawsFile"].ToString().Split('#');

                            //if (files[files.Length - 1].Split('$').Length>1)
                            //  lbl_AO_ADD_DistrictEn = files[files.Length - 1].Split('$')[1];

                            lbl_AO_ADD_DistrictEn = row["DISTRICT"].ToString();


                            for (int x = 0; x < files.Count(); x++)
                            {
                                if (files[x] != null && files[x] != "")
                                {
                                    string TempFile = Server.MapPath(("~/Content/SocietyProfiling/") + societyDir + "/" + files[x]);

                                    string fileName = Path.GetFileName(TempFile);
                                    string targetPath = Path.Combine(targetDirectory, fileName);
                                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

                                    string fileExtension = Path.GetExtension(TempFile);
                                    string modifiedFileName = $"{fileNameWithoutExtension}_E{fileExtension}";
                                    string targetPath1 = Path.Combine(targetDirectory, modifiedFileName);

                                    // Copy the file to the target directory
                                    System.IO.File.Copy(TempFile, targetPath, overwrite: true);
                                    System.IO.File.Copy(TempFile, targetPath1, overwrite: true);
                                    //  files[x] =  targetDirectory + files[x];

                                    files[x] = TempFile;
                                }
                            }

                            var result = COMMON_FUNCTION.EsinghPDF(otpResult.data, string.Join("#", files), lbl_AO_ADD_DistrictEn, "REGISTRAR");

                            if (result.status == "1")
                            {
                                var status = objMobileFlowModel.PendingEsignDocStatusChange(Convert.ToInt32(row["SocietyId"]));
                            }
                            else
                            {
                                otpResult.status = "0";
                                otpResult.data = "";
                            }


                        }
                    }
                }


            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.CommonFunctions", FunctionName = " static ResponseDataEsign PendingDocEsign(otpResult)" + otpResult.data }, ex);
            }
            return "";

        }
        #endregion



        //Get Bank Details
        [HttpGet]
        public JsonResult GetSocietyBankDetails(int societyId)
        {
            try
            {
                DataSet ds = objMobileFlowModel.GetSocietyBankDetails(societyId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var row = ds.Tables[0].Rows[0];

                    var result = new
                    {
                        Id = row["Id"]?.ToString(),
                        ApplicationFlowId = row["ApplicationFlowId"]?.ToString(),
                        SocietyId = row["SocietyId"]?.ToString(),
                        CapitalAmount = row["CapitalAmount"]?.ToString(),
                        AccountNo = row["AccountNo"]?.ToString(),
                        IFSCCode = row["IFSCCode"]?.ToString(),
                        BankName = row["BankName"]?.ToString(),
                        RecieptPath = row["RecieptPath"]?.ToString(),
                        CCB_Id = row["CCB_Id"]?.ToString(),
                        Branch_Id = row["Branch_Id"]?.ToString()
                    };

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "GetSocietyBankDetails" }, ex);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SavePacsCode(string SocietyIDEnc, string PACS_Code)
        {
            try
            {
                int result = objMobileFlowModel.UpdatePacsCode(SocietyIDEnc, PACS_Code);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }


        //Get Bank Details
        [HttpGet]
        public JsonResult GetSocietyBankAndBranch(int societyId, int statusId)
        {
            try
            {
                DataSet ds = objMobileFlowModel.GetSocietyBankAndBranchDetails(societyId, statusId);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var row = ds.Tables[0].Rows[0];

                    var result = new
                    {
                        CCB_Id = row["CCB_Id"]?.ToString(),
                        Branch_Id = row["Branch_Id"]?.ToString()
                    };

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "GetSocietyBankAndBranch" }, ex);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }


        public string GetMasterDataDetails(string MasterType = "", string MasterCode = "", string MasterCode2 = "", int MappingId = 0, int DistrictId = 0)
        {

            string _MasterData = "{}";
            int Code = 0;

            if (string.IsNullOrEmpty(MasterType) || !int.TryParse(MasterCode, out Code))
            {
                // Response.Redirect("~/Views/Error/General.cshtml");
            }

            MasterModel master = new MasterModel(new DBLogger());


            switch (MasterType.ToUpper())
            {
                case "TITLE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.TitleList()); break; }
                case "DEPARTMENT": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.DepartmentList()); break; }
                case "DESIGNATION": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.DesignationtList(Code)); break; }
                case "POST": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.PostList(Code)); break; }
                case "GENDER": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GenderList()); break; }
                case "RELIGION": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.ReligionList()); break; }
                case "CAST": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.CastList()); break; }
                case "CATEGORY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.CategoryList()); break; }
                case "STATE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.StateList(Code)); break; }
                case "DIVISION": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.DivisionList()); break; }
                case "DISTRICT": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.DistrictList(Code)); break; }
                case "TEHSIL": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.TehsilList(Code)); break; }
                case "BLOCK": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.PanchayatSamitiBlockList(Code)); break; }
                case "PANCHAYATSAMITY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.PanchayatSamitiBlockList(Code)); break; }
                case "ASSEMBLY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.AssemblyList(Code)); break; }
                case "GRAMPANCHAYAT": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GramPanchayatList(Code)); break; }
                case "VILLAGE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.VillageList(Code)); break; }
                case "CITY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.CityList(Code, MasterCode2)); break; }
                case "CITYLISTBY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.CityByCityTypeList(Code, MasterCode2)); break; }

                case "WARD": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.WardList(Code)); break; }
                case "FINANCIALYEAR": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.FinancialYearList()); break; }
                case "SOCIETYTYPE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.SocietyTypeList(Code)); break; }
                case "BUSINESS": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.BusinessList(MasterCode)); break; }
                case "LIABILITY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.LiablityList()); break; }
                case "SOCIETYCLASS": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.SocietyClassList(Code)); break; }
                case "SOCIETYSUBCLASS": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.SocietySubClassList(Code, MasterCode2)); break; }

                case "UNITS": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.UnitListByDeptId(Code)); break; }
                case "LEVELS": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.LevelList()); break; }
                case "OCCUPATION": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.OccupationList()); break; }
                case "MEMBERTYPE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.MemberTypeList()); break; }
                case "ACT": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.ACTList()); break; }
                case "CLASSES": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.ClassesList()); break; }
                case "SubClasses": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.ClassesList()); break; }
                case "Buissness": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.BusinessList(MasterCode)); break; }
                // case "Unit": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.GetAllSubUnitList('')); break; }
                case "Organisation": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.OrganisationsList()); break; }
                // case "PurchaseCenter": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.PurchaseCenterList()); break; }                 
                case "Office": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.OfficesList()); break; }
                case "RESIONMAPPING": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.ResionMappingList(MasterCode)); break; }
                case "CHANNEL": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.ChannelList()); break; }
                case "TARGET": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.TargetList()); break; }
                case "USERS": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.UsersList()); break; }
                case "MAJORACTIVITY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetMajorActivityList("0")); break; }
                case "ACTIVITY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetActivityList(MasterCode)); break; }
                case "MONTH": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetMasterMonthList()); break; }
                case "RELATION": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetMasterRelationList()); break; }
                case "ECONOMICGROUP": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetMasterEconomicList()); break; }
                case "PLDB": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetMasterPLDBList(MasterCode)); break; }
                case "PLDBBRANCH": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetMasterPLDBBranchList(MasterCode)); break; }
                case "EDUCATIONQUALIFICATION": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetEducationQualificationList()); break; }
                case "FERTILIZER": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetFertilizerList()); break; }
                case "MENUCONFIGURATION": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.MenuListForConfiguration()); break; }
                case "CONFIGURATIONACTION": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.ActionListForConfiguration()); break; }
                case "DEALERNATURE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.DealerNature()); break; }
                case "LIABILITYASSET": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.LiabilityAssetMasterList(Convert.ToInt32(MasterCode))); break; }
                case "CITYTYPE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.CityTypeList(Convert.ToInt32(MasterCode))); break; }
                //case "COMMODITY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.RajfedCommodityMasterList(Convert.ToInt32(MasterCode))); break; }
                case "COMMODITY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.RajfedCommodityMasterList(Convert.ToInt32(MasterCode))); break; }

                case "PLANT": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.PlantList(Convert.ToInt32(MasterCode))); break; }
                case "DEPORT": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.DepotList(Convert.ToInt32(MasterCode))); break; }
                case "WAREHOUSE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.WarehouseList(Convert.ToInt32(MasterCode))); break; }

                case "CCSCOMPLAINTDOCTYPE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.CCSComplaintDocumentTypeList()); break; }
                case "RAJFEDRO": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.RajfedROList()); break; }
                case "DISTRICTBYRO": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetAllDistrictListByRO(Code)); break; }
                case "DISTRICTBYRODISTRICT": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetDistrictListByRODistrict(Code)); break; }
                case "WAREHOUSEBYRO": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetWarehousetByRODistrict(Code)); break; }
                case "BANKLIST": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetBankMasterList()); break; }
                case "CCB": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetCCBData(Code)); break; }
                case "GRIEVANCECATEGORY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GrievanceCategoryList()); break; }//bhupendra
                //case "GSS_DAIRY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetGSS_DAIRYList(Code, _LoggedInUser.SSOID)); break; }//bhupendra
                //case "INSPECTOR": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetInspector(_LoggedInUser.UserRoleDeptMappingId, _LoggedInUser.DistrictId)); break; }//bhupendra
                case "FLOWPROCESSSTATUS": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetSocietyFlowProcessStatus(MappingId, DistrictId)); break; }
                case "CMINSTALLMENTTYPE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.CMInstallmentType()); break; }
                case "CMKISAN": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetCMK_MasterData(MasterCode)); break; }

                // Pankaj 26-06-2024 Start
                case "PANCHAYATSAMITY_DISTRICTCODE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.PanchayatSamitiBlockList_DistrictCode(MasterCode)); break; }
                case "GRAMPANCHAYAT_BLOCKCODE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GramPanchayatList_BLOCKCODE(MasterCode)); break; }
                case "VILLAGE_GPCODE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.VillageList_GPCODE(MasterCode)); break; }
                case "CITY_DISTRICTCODE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.CityList_DISTRICTCODE(MasterCode, MasterCode2)); break; }
                case "WARD_CITYCODE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.WardList_CITYCODE(MasterCode)); break; }
                // Pankaj 26-06-2024 End

                case "CM_SUB_DISTRICT": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.Cm_SubDistrict(MasterCode)); break; }
                case "CM_VILLAGE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.Cm_Village(MasterCode, MasterCode2)); break; }
                //23-08-2024 # Pradeep
                case "LOANPURPOSE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.PurposeOfLoan()); break; }
                case "MN_DISTRICT": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.MN_DistrictList(Code)); break; }
                case "MN_BLOCK": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.MN_BlockList(Code)); break; }
                case "DISTRICT_WR": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.DISTRICT_WR(Code)); break; }
                case "PAYMENTMODE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetPaymentModeList()); break; } //21-04-2025 #Pradeep
                case "OTSPROCESSSTATUS": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetOtsProcessStatus()); break; } //29-04-2025 #Pradeep
                case "OTSPENDINGLEVEL": { _MasterData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(master.GetOtsPendingLevel(Code)); break; } //18-06-2025 #Pradeep
                case "FUNCTIONALSTATUS": { _MasterData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(master.FunctionalStatusList()); break; } //18-06-2025 #Pradeep

                case "CCB_LIST": { _MasterData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(master.GetCcbData(Convert.ToInt32(MasterCode))); break; } //23-09-2025 #Pradeep
                case "CCB_BANK_LIST": { _MasterData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(master.GetCcbBranchData(Convert.ToInt32(MasterCode))); break; } //23-09-2025 #Pradeep
                case "PACS_LIST": { _MasterData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(master.GetPacsData(Convert.ToInt32(MasterCode))); break; } //23-09-2025 #Pradeep
                case "MAPPING_GCC_ID": { _MasterData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(master.GetCcbIdsByMappingGccId()); break; } //07-10-2025 #Pradeep

                case "PLDBSECTORODNEW": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(master.GetPLDBSectorODNew()); break; } //16-12-2025 #Jaya


                default: break;

            }
            return _MasterData;
        }


        #region eSign New Process Testing Page on 29-05-2026 Added By #Narpat Kumawat

        [HttpGet]
        public ActionResult TestESign() { return View(); }

        #endregion

        #region     Without_Login_Session_DirectWebPage_Methods     ---START---

        [HttpPost]
        public JsonResult SaveSocietyApplicationFlowStatus_DirectWebPage(String SocietyIDEnc, int StatusId, string Remark, string UserSsoId = "", int UserRoleMappingId = 0, int UserId = 0, bool IsAmendment = false, string OTP = "", string TXN = "", string AmendmendAmount = "", string ReceiptNumber = "")
        {
            //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            //if (_LoggedInUser == null)
            //{
            //    return Json("LOGOUT");
            //}           

            if (UserId == 0)
            {
                //  UserId = Convert.ToInt32(_LoggedInUser.UserId);

                return Json("LOGOUT"); // Fro check direct web page request have user id or not // By ritesh 20260611 //
            }

            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
            if (!ModelState.IsValid)
                return Json(JsonModelError.Get(ModelState.Values));


            int result = 0;
            if (StatusId == 14)
            {
                ResultMessage otpResult = new ResultMessage();
                otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                otpResult.status = "1";
                otpResult.data = TXN;
                if (otpResult.status != "1")
                {
                    return Json(otpResult.data);
                }


                string cerResult = BankOpeningCertificateGenerate(SocietyIDEnc, otpResult);
                if (cerResult == "Success")
                {
                    //result = objMobileFlowModel.AddApplicationFlow(SocietyIDEnc, StatusId, Remark, UserId, IsAmendment);
                    result = objMobileFlowModel.AddApplicationFlow(SocietyIDEnc, StatusId, Remark, UserSsoId, UserId, IsAmendment);

                }
                else
                {
                    result = Convert.ToInt32(cerResult);
                }
                try
                {
                    var a = PendingDocEsign(otpResult);
                }
                catch (Exception ex)
                {
                    ILogger objILogger = new DBLogger();
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "MobileApplicationFlowController#" + ex.Message, FunctionName = "VerifyAadhaarOTPForEsinghAmendmen PendingDocEsign" }, ex);

                }

            }
            else
            {
                if (StatusId == 17 || StatusId == 26)
                {
                    if (objMobileFlowModel.GetActIdBySocietyId(SocietyIDEnc) == 3)
                    {
                        //NCD Integration START


                        //NCD Integration END
                    }

                }
                //result = objMobileFlowModel.AddApplicationFlow(SocietyIDEnc, StatusId, Remark, UserId, IsAmendment, AmendmendAmount, ReceiptNumber);
                result = objMobileFlowModel.AddApplicationFlow(SocietyIDEnc, StatusId, Remark, UserSsoId, UserId, IsAmendment, AmendmendAmount, ReceiptNumber);
            }

            //Pradeep 01-08-2024
            if (result == 1)
            {
                RajNiveshModel rajNivesh = new RajNiveshModel(new FileLogger());
                DataTable dtr = rajNivesh.GetSwcStatusDataBySocietyId(SocietyIDEnc);

                if (dtr != null && dtr.Rows.Count > 0)
                {
                    RajNiveshStatusRequest rajNiveshStatus = new RajNiveshStatusRequest();
                    rajNiveshStatus.TRACKERID = Convert.ToInt32(dtr.Rows[0]["SWSID"].ToString());
                    rajNiveshStatus.STATUS_CODE = Convert.ToInt32(dtr.Rows[0]["STATUS_CODE"].ToString());
                    rajNiveshStatus.REGISTRATION_NUMBER = dtr.Rows[0]["RegNo"].ToString();
                    rajNiveshStatus.REMARKS = dtr.Rows[0]["REMARKS"].ToString();
                    rajNivesh.UpdateStatus(rajNiveshStatus);
                }
            }

            return Json(result);

        }

        [HttpPost]
        public string VerifyAadhaarOTPForEsing_DirectWebPage(string OTP, string TXN, string SocietyIDEnc, int StatusId, string Remark, bool IsAmendment)
        {
            //if (IsAmendment)
            //{
            //    return VerifyAadhaarOTPForEsinghAmendment(OTP, TXN, SocietyIDEnc, StatusId, Remark, IsAmendment);
            //}
            string message = string.Empty;
            string status = "";
            string TransactionId = "";
            string final_status = "";
            string final_document = "";
            string succmsg = string.Empty;
            string pdfPath = "";
            string openFile = "";
            ResultMessage otpResult = new ResultMessage();
            if (StatusId == 0 || Remark == "")
            {
                return "Fail";
            }

            if (TXN != "")
            {
                try
                {
                    otpResult.status = "1";
                    otpResult.data = TXN;
                    // otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                    if (otpResult.status != "1")
                    {
                        return otpResult.data;
                    }
                    if (otpResult.status == "1")
                    {
                        //DOC GEN
                        pdfPath = CertificateGenerate(SocietyIDEnc);

                        try
                        {
                            Sp_callTrackingInsert("Society_CertificateEsign", SocietyIDEnc + ": " + pdfPath);
                        }
                        catch (Exception)
                        {

                        }

                        if (!pdfPath.Contains("Fail"))
                        {
                            Exception httpException = new Exception();
                            ILogger objILogger = new DBLogger();
                            //objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "NOT  Fail" }, httpException);

                            var Filepath = pdfPath.Split('#');
                            var lbl_AO_ADD_DistrictEn = Filepath[Filepath.Length - 1].Split('$')[1];
                            Filepath[Filepath.Length - 1] = Filepath[Filepath.Length - 1].Split('$')[0];

                            string[] files = new string[Filepath.Length - 1];
                            for (int x = 1; x < Filepath.Length; x++)
                            {
                                files[x - 1] = Server.MapPath("~/Content/SocietyProfiling/") + Filepath[x];
                            }

                            var result = COMMON_FUNCTION.EsinghPDF(otpResult.data, Server.MapPath("~/Content/SocietyProfiling/") + Filepath[0], lbl_AO_ADD_DistrictEn, "REGISTRAR", "350", "120", "150", "120");
                            //  var result = COMMON_FUNCTION.EsignPDF_NEW(otpResult.data, Server.MapPath("~/Content/SocietyProfiling/") + Filepath[0], lbl_AO_ADD_DistrictEn, "REGISTRAR", "350", "120", "150", "120");

                            if (result.status == "1")
                            {
                                //
                                result = COMMON_FUNCTION.EsinghPDF(otpResult.data, string.Join("#", files), lbl_AO_ADD_DistrictEn, "REGISTRAR");
                                // result = COMMON_FUNCTION.EsignPDF_NEW(otpResult.data, string.Join("#", files), lbl_AO_ADD_DistrictEn, "REGISTRAR");

                            }
                            succmsg = result.status;

                            if (succmsg == "1")
                            {
                                var result1 = SaveSocietyApplicationFlowStatus_DirectWebPage(SocietyIDEnc, StatusId, Remark, "");
                                if (result1.Data.ToString() != "1")
                                {
                                    return "Fail";
                                }
                                else
                                {
                                    //string BylawsStr = SaveCertificate(SocietyIDEnc, Filepath[0].Split('/')[1], Filepath[1].Split('/')[1], true);
                                    string BylawsStr = SaveCertificate_DirectWebPage(SocietyIDEnc, Filepath[0].Split('/')[1], Filepath[1].Split('/')[1], true);
                                    if (BylawsStr == "Fail" || BylawsStr == "-1")
                                    {
                                        return "Fail";
                                    }
                                }
                                succmsg = result.data;
                            }
                        }
                        else
                        {
                            succmsg = pdfPath;
                        }

                        try
                        {
                            var a = PendingDocEsign(otpResult);
                        }
                        catch (Exception ex)
                        {
                            ILogger objILogger = new DBLogger();
                            objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "MobileApplicationFlowController#" + ex.Message, FunctionName = " public string VerifyAadhaarOTPForEsing_DirectWebPage(string OTP, string TXN, string SocietyIDEnc, int StatusId, string Remark, bool IsAmendment)" }, ex);
                        }
                    }
                    else
                    {
                        succmsg = "Invalid OTP!";
                    }
                }
                catch (Exception ex)
                {
                    succmsg = ex.Message;
                }
            }
            return succmsg;
        }

        public string SaveCertificate_DirectWebPage(string SocietyIDEnc, string fileNameCerti, string fileNameMemList, bool isNewRegistration)
        {

            return objMobileFlowModel.Addcertificate_DirectWebPage(SocietyIDEnc, fileNameCerti, fileNameMemList, isNewRegistration).ToString();

        }
        #endregion  Without_Login_Session_DirectWebPage_Methods     -----END---
    }
}