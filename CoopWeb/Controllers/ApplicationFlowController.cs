using Cooperative.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
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
using java.security;
using Newtonsoft.Json;
//using java.nio.file;
using System.Web.Http.Results;
using static Cooperative.Controllers.COMMON_FUNCTION;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Web.UI.WebControls;
using static EsignAesEncryption;
using System.Linq;
using Microsoft.Ajax.Utilities;
using System.Web.Routing;
using com.sun.security.ntlm;
using static javax.net.ssl.SSLEngineResult;
using System.Runtime.InteropServices;
using javax.xml.transform;
using sun.swing;
using static Cooperative.Models.CoopModel;
using FuzzySharp;
using OfficeOpenXml.Drawing.Slicer.Style;

namespace Cooperative.Controllers
{
    [Error]
    [LogActionFilter]
    [CustomAuthentication]
    public class ApplicationFlowController : Controller
    {

        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        EsignModel eSignModel = new EsignModel(new DBLogger());
        // GET: ApplicationFlow
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        [ActionName("BankOpeningCertificateGenerate")]
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
            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());
            dt = objApplicationFlowModel.GetApplicationPdfdata(SocietyIDEnc);
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
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

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
        public ActionResult ApplicationFlowAmendment()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

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
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

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

            Users objApplicationFlowModel = new Users();
            return Json(new { DataList = objApplicationFlowModel.GetUserListForFileTransfer(ActId, DistrictId, UserId, RoleId) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetUserSocietyList(int ActId, int UserId)
        {

            Users objApplicationFlowModel = new Users();
            return Json(new { DataList = objApplicationFlowModel.GetUserSocietyList(UserId, ActId) }, JsonRequestBehavior.AllowGet);
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
            Users objApplicationFlowModel = new Users();
            return Json(new { DataList = objApplicationFlowModel.TransferSocerties(FromUserId, SocietyId, ToUserId, fileName, Remarks, COMMON_FUNCTION.GetIpAddress(), _LoggedInUser.SSOID, ActId) }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult ApplicationHistory()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

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

        [HttpGet]
        public ActionResult SimilarSocietyDetail()
        {
            return View();
        }


        //[HttpGet]
        //public ActionResult ApplicationFlowCitizen()
        //{
        //    UserModel OBJUser = new UserModel(new FileLogger());
        //    bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

        //    if (Result == false)
        //    {
        //        return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
        //    }
        //    else
        //    {
        //        ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.UserRegistrationRequestApprovable, (Request.Cookies[SessionIdentity.Culture]).Value);
        //    }
        //    return View();
        //}

        [HttpPost]
        [ActionName("GetSocietyAppliFlowList")]
        public JsonResult GetSocietyAppliFlowList(DataPagingModel Details, bool IsAmendment)
        {
            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());
            //return COMMON_FUNCTION.DataTableToJSONString(objApplicationFlowModel.GetSocietyAppliFlowList());
            // return COMMON_FUNCTION.DataTableToJSONString(objApplicationFlowModel.GetSocietyAppliFlowList(Details));
            DataTable DT = objApplicationFlowModel.GetSocietyAppliFlowList(ref Details, IsAmendment);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        [ActionName("GetSocietyAmendment1958FlowList")]
        public JsonResult GetSocietyAmendment1958FlowList(DataPagingModel Details, bool IsAmendment)
        {
            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());
            //return COMMON_FUNCTION.DataTableToJSONString(objApplicationFlowModel.GetSocietyAppliFlowList());
            // return COMMON_FUNCTION.DataTableToJSONString(objApplicationFlowModel.GetSocietyAppliFlowList(Details));
            DataTable DT = objApplicationFlowModel.GetSocietyAmendment1958FlowList(ref Details, IsAmendment);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        [ActionName("GetSocietyApplicationFlowListSearch")]
        public JsonResult GetSocietyApplicationFlowListSearch(DataPagingModel Details, bool IsAmendment)
        {

            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());
            //return COMMON_FUNCTION.DataTableToJSONString(objApplicationFlowModel.GetSocietyAppliFlowList());
            // return COMMON_FUNCTION.DataTableToJSONString(objApplicationFlowModel.GetSocietyAppliFlowList(Details));

            DataTable DT = objApplicationFlowModel.GetSocietyApplicationFlowListSearch(ref Details, IsAmendment);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public JsonResult GetUserForSocietyAssign(string SocietyID)
        {

            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());
            //return COMMON_FUNCTION.DataTableToJSONString(objApplicationFlowModel.GetSocietyAppliFlowList());
            // return COMMON_FUNCTION.DataTableToJSONString(objApplicationFlowModel.GetSocietyAppliFlowList(Details));

            DataTable DT = objApplicationFlowModel.GetUserForSocietyAssign(SocietyID);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);


        }


        [HttpPost]
        [ActionName("SocietyProceesStatusList")]
        public string GetSocietyProceesStatusList(string StatusId, string WorkOnActId, string SocietyIDEnc, int Flag = 0, bool IsAmendment = false, int AmendmentId = 0)
        {
            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());
            // string WhereClause = " and SocietyProceesStatusIdParent = " + StatusId + " AND WorkOnActId = " + WorkOnActId;
            int StatusIdInt;
            int.TryParse(StatusId, out StatusIdInt);
            int WorkOnActIdInt;
            int.TryParse(WorkOnActId, out WorkOnActIdInt);
            return COMMON_FUNCTION.DataTableToJSONString(objApplicationFlowModel.GetSocietyProceesStatusListByIds(StatusIdInt, WorkOnActIdInt, SocietyIDEnc, Flag, IsAmendment, AmendmentId));
        }

        [HttpGet]
        [ActionName("GetApplicationFlowSociety")]
        public string GetApplicationFlowSociety(string SocietyIDEnc)
        {
            // COMMON_FUNCTION.SSO_AADHAR_OTP_VALIDATION 
            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objApplicationFlowModel.GetApplicationFlowSociety(SocietyIDEnc));
        }

        [HttpGet]
        [ActionName("GetApplicationFlowSocietyByRegiNo")]
        public string GetApplicationFlowSocietyByRegiNo(string SocietyRegiNo)
        {
            // COMMON_FUNCTION.SSO_AADHAR_OTP_VALIDATION 
            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objApplicationFlowModel.GetApplicationFlowSocietyByRegiNo(SocietyRegiNo));
        }

        [HttpGet]
        [ActionName("GetApplicationFlowSocietyAmendment1958")]
        public string GetApplicationFlowSocietyAmendment1958(string SocietyIDEnc, int AmendmentId)
        {
            // COMMON_FUNCTION.SSO_AADHAR_OTP_VALIDATION 
            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objApplicationFlowModel.GetApplicationFlowSocietyAmendment1958(SocietyIDEnc, AmendmentId));
        }

        [HttpGet]
        [ActionName("GetCurrentUserId")]
        public string GetCurrentUserId()
        {
            //if ((UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
            //{
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            return _LoggedInUser.UserId.ToString();
            //}

        }

        [HttpGet]
        [ActionName("GenerateAadhaarOTP")]
        public string GenerateAadhaarOTP()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            return COMMON_FUNCTION.SendOtpByAadharNo(_LoggedInUser.AadharNo);
            //return COMMON_FUNCTION.SSO_AADHAR_OTP_VALIDATION("narottam.saini85");
            //return "RJVSD2343246566";
        }

        [HttpPost]
        [ActionName("VerifyAadhaarOTP")]
        public string VerifyAadhaarOTP(string OTP, string TXN)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            return COMMON_FUNCTION.DataTableToJSONString(COMMON_FUNCTION.ValidateOTP(TXN, _LoggedInUser.SSOID, OTP));
            //return COMMON_FUNCTION.DataTableToJSONString(COMMON_FUNCTION.ValidateOTP(TXN, "narottam.saini85", OTP));

        }

        [HttpPost]
        [ActionName("GetStatusListBySocietyRegiNo")]
        public string GetStatusListBySocietyRegiNo(string SocietyRegiNo)
        {
            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());
            string whereCondition = " and SocietyProceesStatusId != 17 AND WorkOnActId in (Select ActId from Societies where RegistrationNo='" + SocietyRegiNo + "')";
            return COMMON_FUNCTION.DataTableToJSONString(objApplicationFlowModel.GetSocietyProceesStatusList(whereCondition));
        }


        [HttpPost]
        [ActionName("GetSocietyDetailBySocietyIDEnc")]
        public string GetSocietyDetailBySocietyIDEnc(string SocietyIDEnc)
        {
            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objApplicationFlowModel.GetSocietyDetailBySocietyIDEnc(SocietyIDEnc));
        }
        [HttpPost]
        [ActionName("GetSocietyDetailBySocietyIDEncAmendment")]
        public string GetSocietyDetailBySocietyIDEncAmendment(string AmendmentId)
        {
            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objApplicationFlowModel.GetSocietyDetailBySocietyIDEncAmendment(AmendmentId));
        }
        public ActionResult SocietyActionView(string SocietyIDEnc)
        {
            // ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.SocietyApplicationView, (Request.Cookies[SessionIdentity.Culture]).Value);
            var response = TempData.Peek("Response")?.ToString();
            ViewBag.response = response;
            if (Request.Cookies[SessionIdentity.Culture] == null)
            {
                Response.Cookies.Add(new System.Web.HttpCookie(Cooperative.Controllers.SessionIdentity.Culture, "")
                { Value = "en-us", HttpOnly = true });
            }
            if (SocietyIDEnc != null) { System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc] = SocietyIDEnc; }
            return View();
        }



        public ActionResult SocietyActionViewAmendment1958(string SocietyIDEnc)
        {
            // ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.SocietyApplicationView, (Request.Cookies[SessionIdentity.Culture]).Value);
            var response = TempData.Peek("Response")?.ToString();
            ViewBag.response = response;
            if (Request.Cookies[SessionIdentity.Culture] == null)
            {
                Response.Cookies.Add(new System.Web.HttpCookie(Cooperative.Controllers.SessionIdentity.Culture, "")
                { Value = "en-us", HttpOnly = true });
            }

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
                string ext = "";
                if (RecieptPath != null)
                    ext = Path.GetExtension(RecieptPath.FileName);
                if (ext.Contains(".pdf"))
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
                else
                {
                    return Json(new { success = false, message = "File Extension not received" });
                }
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
        //[HttpPost]
        //[ActionName("SaveSocietyApplicationFlowStatus")]
        //public JsonResult SaveSocietyApplicationFlowStatus(String SocietyIDEnc, int StatusId, string Remark, int UserId, bool IsAmendment = false, string OTP = "", string TXN = "", string AmendmendAmount = "", string ReceiptNumber = "")
        //{
        //    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

        //    if (_LoggedInUser == null)
        //    {
        //        return Json("LOGOUT");
        //    }

        //    ApplicationFlowModel _Modal = new ApplicationFlowModel();
        //    if (UserId == 0)
        //    {
        //        //  UserId = Convert.ToInt32(_LoggedInUser.UserId);
        //    }
        //    ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
        //    if (!ModelState.IsValid)
        //        return Json(JsonModelError.Get(ModelState.Values));


        //    int result = 0;
        //    if (StatusId == 14)
        //    {
        //        ResultMessage otpResult = new ResultMessage();
        //        otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
        //        otpResult.status = "1";
        //        otpResult.data = TXN;
        //        if (otpResult.status != "1")
        //        {
        //            return Json(otpResult.data);
        //        }


        //        string cerResult = BankOpeningCertificateGenerate(SocietyIDEnc, otpResult);
        //        if (cerResult == "Success")
        //        {
        //            result = _Modal.AddApplicationFlow(SocietyIDEnc, StatusId, Remark, UserId, IsAmendment);

        //        }
        //        else
        //        {
        //            result = Convert.ToInt32(cerResult);
        //        }
        //        try
        //        {
        //            var a = PendingDocEsign(otpResult);
        //        }
        //        catch (Exception ex)
        //        {
        //            ILogger objILogger = new DBLogger();
        //            objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFLowController#" + ex.Message, FunctionName = "VerifyAadhaarOTPForEsinghAmendmen PendingDocEsign" }, ex);

        //        }

        //    }
        //    else
        //    {
        //        if (StatusId == 17 || StatusId == 26)
        //        {
        //            if (_Modal.GetActIdBySocietyId(SocietyIDEnc) == 3)
        //            {
        //                //NCD Integration START


        //                //NCD Integration END
        //            }

        //        }
        //        result = _Modal.AddApplicationFlow(SocietyIDEnc, StatusId, Remark, UserId, IsAmendment, AmendmendAmount, ReceiptNumber);
        //    }

        //    //Pradeep 01-08-2024
        //    if (result == 1)
        //    {
        //        RajNiveshModel rajNivesh = new RajNiveshModel(new FileLogger());
        //        DataTable dtr = rajNivesh.GetSwcStatusDataBySocietyId(SocietyIDEnc);

        //        if (dtr != null && dtr.Rows.Count > 0)
        //        {
        //            RajNiveshStatusRequest rajNiveshStatus = new RajNiveshStatusRequest();
        //            rajNiveshStatus.TRACKERID = Convert.ToInt32(dtr.Rows[0]["SWSID"].ToString());
        //            rajNiveshStatus.STATUS_CODE = Convert.ToInt32(dtr.Rows[0]["STATUS_CODE"].ToString());
        //            rajNiveshStatus.REGISTRATION_NUMBER = dtr.Rows[0]["RegNo"].ToString();
        //            rajNiveshStatus.REMARKS = dtr.Rows[0]["REMARKS"].ToString();
        //            rajNivesh.UpdateStatus(rajNiveshStatus);
        //        }
        //    }

        //    return Json(result);

        //}


        //new
        [HttpPost]
        [ActionName("SaveSocietyApplicationFlowStatus")]
        public JsonResult SaveSocietyApplicationFlowStatus(String SocietyIDEnc, int StatusId, string Remark, int UserId, bool IsAmendment = false, string OTP = "", string TXN = "", string AmendmendAmount = "", string ReceiptNumber = "", string CapitalAmount = null, string AccountNumber = "", string IFSCCode = "", string BankName = "", string RecieptPath = "", int Id = 0, int CCB_Id = 0, int Branch_Id = 0, string PACS_Code = "", DateTime? membershipAppDate = null, DateTime? membershipDate = null, string membershipRemark = "", bool? IsPacsMembershipReg = null)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            if (_LoggedInUser == null)
            {
                return Json("LOGOUT");
            }


            ApplicationFlowModel _Modal = new ApplicationFlowModel();
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
                    result = _Modal.AddApplicationFlow(SocietyIDEnc, StatusId, Remark, UserId, IsAmendment);

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
                    if (_Modal.GetActIdBySocietyId(SocietyIDEnc) == 3)
                    {
                        //NCD Integration START



                        //NCD Integration END
                    }

                }
                result = _Modal.AddApplicationFlow(SocietyIDEnc, StatusId, Remark, UserId, IsAmendment, AmendmendAmount, ReceiptNumber, CapitalAmount, AccountNumber, IFSCCode, BankName, RecieptPath, Id, CCB_Id, Branch_Id, PACS_Code, membershipAppDate, membershipDate, membershipRemark, IsPacsMembershipReg);
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
        [ActionName("SaveSocietyApplicationFlowAmendment1958Status")]
        public JsonResult SaveSocietyApplicationFlowAmendment1958Status(String SocietyIDEnc, int StatusId, string Remark, int UserId, int AmendmentId, string OTP = "", string TXN = "", string AmendmendAmount = "", string ReceiptNumber = "")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            if (_LoggedInUser == null)
            {
                return Json("LOGOUT");
            }

            ApplicationFlowModel _Modal = new ApplicationFlowModel();
            if (UserId == 0)
            {
                //  UserId = Convert.ToInt32(_LoggedInUser.UserId);
            }
            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
            if (!ModelState.IsValid)
                return Json(JsonModelError.Get(ModelState.Values));


            int result = 0;

            result = _Modal.AddApplicationFlowAmendment1958(SocietyIDEnc, StatusId, Remark, UserId, AmendmentId, AmendmendAmount, ReceiptNumber);

            return Json(result);

        }


        [HttpPost]
        [ActionName("SaveSocietyVerifyReportFile")]
        public string SaveSocietyVerifyReportFile(SocietyRegistrationEntity _Entity)
        {
            string ReturnRegiNo = "0";
            string fileName = string.Empty;
            string ReturnJSON = "";
            string pathString = "~/Content/SocietyProfiling/";

            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());

            foreach (string files in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[files];
                fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace(" ", "").Replace("_", "").ToUpper() + Path.GetExtension(file.FileName);
                if (files.ToString() == "SocietyReportFile")
                    _Entity.SocietyReportFile = fileName;
                ReturnRegiNo = objApplicationFlowModel.InsertSocietyReportFile(_Entity.SocietyIDEnc, _Entity.SocietyReportFile);
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
        [ActionName("CertificateGenerate")]
        public string CertificateGenerate(string SocietyIDEnc)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {

                    ApplicationFlowModel AppFlowModel = new ApplicationFlowModel();
                    DataTable dt = AppFlowModel.GetScietyAdministrativeDetail(SocietyIDEnc, 1, " ");
                    string fileNameCerti = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace("_", "").Replace(" ", "").ToUpper() + ".pdf";
                    string fileNameMemList = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace("_", "").Replace(" ", "").ToUpper() + ".pdf";
                    //remove this after check the condition (savecertificate)
                    //string savecertificate = SaveCertificate(SocietyIDEnc, fileNameCerti, fileNameMemList, true);
                    if (dt.Rows.Count > 0)
                    {

                        //DataTable tblFiltered = dt.AsEnumerable().Where(row => row.Field<string>("POSTNAME").ToUpper() == "SECRETARY").CopyToDataTable();
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

                        DataTable dt1 = AppFlowModel.GetScietyAdministrativeDetail(SocietyIDEnc, 2, "");//ad.UserType in (4
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

                        string BylawsStr = AppFlowModel.GetSocietyBylawsFile(SocietyIDEnc, 0, 0);

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
        [ActionName("CertificateGenerateAmendment")]
        public string CertificateGenerateAmendment(ref string CirtFile, ref string MemberFile, ref string bylawsFile, string SocietyIDEnc, int AmendmentId)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {

                    ApplicationFlowModel AppFlowModel = new ApplicationFlowModel();

                    DataTable dtAmendmentChanges = AppFlowModel.GetAmendmentChanges(SocietyIDEnc, AmendmentId);
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
                            //DataTable dt = AppFlowModel.GetScietyAdministrativeDetail(SocietyIDEnc, 1, " ");
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

                    DataTable dt1 = AppFlowModel.GetAdministrativeDetailsAmendment(SocietyIDEnc, "", AmendmentId);//ad.UserType in (4
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
            ApplicationFlowModel _Modal = new ApplicationFlowModel();

            return _Modal.Addcertificate(SocietyIDEnc, fileNameCerti, fileNameMemList, isNewRegistration).ToString();

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

                    //foreach (DataRow dr in dt.Rows) // search whole table
                    //{Name of society

                    //    if (dr["AadharNo"].ToString().Length > 10)
                    //    {
                    //        dr["AadharNo"] = COMMON_FUNCTION.Decrypt(dr["AadharNo"].ToString());
                    //        if (dr["AadharNo"].ToString().Length == 12)
                    //        {
                    //            var AadharNo = dr["AadharNo"].ToString();

                    //            var firstDigits = AadharNo.Substring(0, 8);
                    //            var lastDigits = AadharNo.Substring(AadharNo.Length - 4, 4);
                    //            var requiredMask = new String('X', AadharNo.Length - lastDigits.Length);
                    //            var maskedString = string.Concat(requiredMask, lastDigits);
                    //            var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0");
                    //            dr["AadharNo"] = maskedString;
                    //        }
                    //    }
                    //    //objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "inner aadhar loop " + dr["AadharNo"] }, httpException);
                    //}

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

                    //foreach (DataRow dr in dt.Rows) // search whole table
                    //{

                    //    if (dr["AadharNo"].ToString().Length > 10)
                    //    {
                    //        dr["AadharNo"] = COMMON_FUNCTION.Decrypt(dr["AadharNo"].ToString());
                    //        if (dr["AadharNo"].ToString().Length == 12)
                    //        {
                    //            var AadharNo = dr["AadharNo"].ToString();

                    //            var firstDigits = AadharNo.Substring(0, 8);
                    //            var lastDigits = AadharNo.Substring(AadharNo.Length - 4, 4);
                    //            var requiredMask = new String('X', AadharNo.Length - lastDigits.Length);
                    //            var maskedString = string.Concat(requiredMask, lastDigits);
                    //            var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0");
                    //            dr["AadharNo"] = maskedString;
                    //        }
                    //    }

                    //    //objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "inner aadhar loop " + dr["AadharNo"] }, httpException);

                    //}

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




                        //





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
                    //System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    // sb1.Append(sb.ToString().Replace("<br>", "<br/>"));
                    //string filepath = Server.MapPath("~/Content/RSWC");



                    //COMMON_FUNCTION.GeneratePDF(sb1, filepath, "");




                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath, ""))
                    {

                        return "Success";
                    }
                    else
                    {
                        return "Fail";
                    }



                    //sb.Append("<html><body><table style='width: 100%;height: 100%;border: solid 1px #ccc;'><tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'><img width='56' height='94' src='" + System.Web.HttpContext.Current.Server.MapPath("~/images/PDFLOGO.png") + "' alt='' /></td></tr><tr><td style='margin: 0px;padding: 0px 10px;font-weight:bold;font-family: Kruti Dev 010;font-size:16px;font-weight:bold;text-align:center;height:25px'>jktLFkku ljdkj</td></tr><tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px'><span style='margin:0px;padding:0px;font-family:Kruti Dev 010;font-size:19px;font-weight:bold'>lgdkfjrk foHkkx</span><span style='font-size:17px;font-weight:bold'>/ Cooperative Department</span></td></tr><tr><td style='padding: 8px 15px 0px 15px; text-align: center; border-top: solid 1px #cccccc; height: 20px;'><span style='margin:10px 0px;padding:0px;font-size:19px;font-family:Kruti Dev 010;font-weight:bold'>dk;Zdkfj.kh çek.k-i=</span><span style='font-size:15px;font-weight:bold;'>/ Executive Committee Certificate</span></td></tr>");
                    //sb.Append("<tr><td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><table style='width:100%'><tr><td style='width:49%;text-align:left'><span style='font-family: Kruti Dev 010; font-size: 17px;'>Øekad</span><span style='font-size: 14px;font-weight:normal;'>/ S.No. " + lbl_Registraionno + "</span></td><td style='width:49%;text-align:right'><span style='font-family:Kruti Dev 010;font-size:17px'>fnukad</span><span style='font-size: 14px;font-weight:normal;'>/ Date " + DateTime.Now.ToString("dd-MM-yyyy") + "</span></td></tr></table></td></tr>");

                    //if (actId == "1")
                    //{
                    //    sb.Append("<tr><td style='font-family: Kruti Dev 010; padding: 15px; line-height: 30px; height: 50px; font-size: 16px;'>;g çekf.kr fd;k tkrk gS fd <span style='font-family: open sans; font-weight: normal; font-size: 12px;'>" + lbl_InstitutionName + "</span> ftyk <span style='font-family: open sans; font-weight: normal; font-size: 12px;'> " + lbl_AO_ADD_DistrictEn + " </span> tks jktLFkku lkslk;Vh jftLVªs'ku vf/kfu;e]1958 ¼jktLFkku vf/kfu;e la[;kad 28½ ds vUrxZr iath—r gS] esa iathdj.k ds le; fuEukafdr dk;Zdkfj.kh gS</td></tr>");
                    //    sb.Append("<tr><td style='padding: 15px; line-height: 30px; font-size: 12px; font-weight: normal; height: 50px;'>It is certified that " + lbl_InstitutionName + " district " + lbl_AO_ADD_DistrictEn + " which is registered under The Rajasthan Societies Registration Act, 1958 (Rajasthan Act No.28) has the following executive committee at the time of registration</td></tr>");
                    //}
                    //else if (actId == "3")
                    //{
                    //    sb.Append("<tr><td style='font-family: Kruti Dev 010; padding: 15px; line-height: 30px; height: 50px; font-size: 16px;'>;g çekf.kr fd;k tkrk gS fd <span style='font-family: open sans; font-weight: normal; font-size: 12px;'>" + lbl_InstitutionName + "</span> ftyk <span style='font-family: open sans; font-weight: normal; font-size: 12px;'> " + lbl_AO_ADD_DistrictEn + " </span> jktLFkku lgdkjh lkslkbVh vfèkfu;e] 2001 ds vUrxZr iath—r gS] esa iathdj.k ds le; fuEukafdr dk;Zdkfj.kh gS</td></tr>");
                    //    sb.Append("<tr><td style='padding: 15px; line-height: 30px; font-size: 12px; font-weight: normal; height: 50px;'>It is certified that " + lbl_InstitutionName + " district " + lbl_AO_ADD_DistrictEn + " which is registered under The Rajasthan Societies Registration Act, 2001 has the following executive committee at the time of registration</td></tr>");
                    //}
                    //else if (actId == "7")
                    //{
                    //    sb.Append("<tr><td style='font-family: Kruti Dev 010; padding: 15px; line-height: 30px; height: 50px; font-size: 16px;'>;g çekf.kr fd;k tkrk gS fd <span style='font-family: open sans; font-weight: normal; font-size: 12px;'>" + lbl_InstitutionName + "</span> ftyk <span style='font-family: open sans; font-weight: normal; font-size: 12px;'> " + lbl_AO_ADD_DistrictEn + " </span> jktLFkku LiksV~lZ ,DV] 2005 ds vUrxZr iath—r gS] esa iathdj.k ds le; fuEukafdr dk;Zdkfj.kh gS</td></tr>");
                    //    sb.Append("<tr><td style='padding: 15px; line-height: 30px; font-size: 12px; font-weight: normal; height: 50px;'>It is certified that " + lbl_InstitutionName + " district " + lbl_AO_ADD_DistrictEn + " which is registered under The Rajasthan Societies Registration Act, 2005 has the following executive committee at the time of registration</td></tr>");
                    //}



                    //sb.Append("<tr><td valign='top' style = 'padding: 15px; line-height: 30px; font-size: 12px; font-weight: normal;'>");
                    //sb.Append("<table cellspacing='0' cellpadding='2'  style='width: 100%;height: 100%;font-size: 12px; font-weight: normal;border-collapse: collapse;'><tr><td style='padding:2px;border: solid 0.2px #ccc;'>Sl. No.</td><td style='padding:2px;border: solid 0.2px #ccc;'>Post</td><td style='padding:2px;border: solid 0.2px #ccc;'>Aadhaar No</td><td style='padding:2px;border: solid 0.2px #ccc;'>Photo</td><td style='padding:2px;border: solid 0.2px #ccc;'>Name</td><td style='padding:2px;border: solid 0.2px #ccc;'>Gender</td><td style='padding:2px;border: solid 0.2px #ccc;'>DOB</td></tr>");




                    //if ((dt.Rows.Count) < 7)
                    //{
                    //    sb.Append("<tr><td  height='400' valign='top' style = 'padding: 15px; line-height: 30px; font-size: 12px; font-weight: normal;'>");
                    //    sb.Append("<table cellspacing='0' cellpadding='2'  style='width: 100%;height: 100%;font-size: 12px; font-weight: normal;border-collapse: collapse;'><tr><td style='padding:2px;border: solid 0.2px #ccc;'>Sl. No.</td><td style='padding:2px;border: solid 0.2px #ccc;'>Post</td><td style='padding:2px;border: solid 0.2px #ccc;'>Aadhaar No</td><td style='padding:2px;border: solid 0.2px #ccc;'>Photo</td><td style='padding:2px;border: solid 0.2px #ccc;'>Name</td><td style='padding:2px;border: solid 0.2px #ccc;'>Gender</td><td style='padding:2px;border: solid 0.2px #ccc;'>DOB</td></tr>");
                    //}
                    //else if ((dt.Rows.Count) < 24)
                    //{
                    //    sb.Append("<tr><td  height='800' valign='top' style = 'padding: 15px; line-height: 30px; font-size: 12px; font-weight: normal;'>");
                    //    sb.Append("<table cellspacing='0' cellpadding='2'  style='width: 100%;height: 100%;font-size: 12px; font-weight: normal;border-collapse: collapse;'><tr><td style='padding:2px;border: solid 0.2px #ccc;'>Sl. No.</td><td style='padding:2px;border: solid 0.2px #ccc;'>Post</td><td style='padding:2px;border: solid 0.2px #ccc;'>Aadhaar No</td><td style='padding:2px;border: solid 0.2px #ccc;'>Photo</td><td style='padding:2px;border: solid 0.2px #ccc;'>Name</td><td style='padding:2px;border: solid 0.2px #ccc;'>Gender</td><td style='padding:2px;border: solid 0.2px #ccc;'>DOB</td></tr>");
                    //}
                    //else
                    //{
                    //    sb.Append("<tr><td  height='1200' valign='top' style = 'padding: 15px; line-height: 30px; font-size: 12px; font-weight: normal;'>");
                    //    sb.Append("<table cellspacing='0' cellpadding='2'  style='width: 100%;height: 100%;font-size: 12px; font-weight: normal;border-collapse: collapse;'><tr><td style='padding:2px;border: solid 0.2px #ccc;'>Sl. No.</td><td style='padding:2px;border: solid 0.2px #ccc;'>Post</td><td style='padding:2px;border: solid 0.2px #ccc;'>Aadhaar No</td><td style='padding:2px;border: solid 0.2px #ccc;'>Photo</td><td style='padding:2px;border: solid 0.2px #ccc;'>Name</td><td style='padding:2px;border: solid 0.2px #ccc;'>Gender</td><td style='padding:2px;border: solid 0.2px #ccc;'>DOB</td></tr>");
                    //}

                    //  objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "generatePDFformember 1 " }, httpException);

                    //foreach (DataRow row in dt.Rows)
                    //{
                    //    sb.Append("<tr><td style='padding:2px;border: solid 0.2px #ccc;'>" + row["RowNumber"].ToString() + "</td><td style='padding:2px;border: solid 0.2px #ccc;'>" + row["POST"].ToString().Split('/')[0] + "</td><td style='padding:2px;border: solid 0.2px #ccc;'>" + row["AADHAAR"].ToString() + "</td><td style='padding:2px;border: solid 0.2px #ccc;'><img width='55' height='55' src='" + row["PHOTO"].ToString() + "' /></td><td style='padding:2px;border: solid 0.2px #ccc;'>" + row["NAME"].ToString() + "</td><td style='padding:2px;border: solid 0.2px #ccc;'>" + row["GENDER"].ToString() + "</td><td style='padding:2px;border: solid 0.2px #ccc;'>" + row["DOB"].ToString() + "</td><td style='padding:2px;border: solid 0.2px #ccc;'>" + row["OCCUPATION"].ToString() + "</td></tr>");
                    //}
                    //foreach (DataRow dr in dt.Rows) // search whole table
                    //{

                    //    if (dr["AadharNo"].ToString().Length > 10)
                    //    {
                    //        dr["AadharNo"] = COMMON_FUNCTION.Decrypt(dr["AadharNo"].ToString());
                    //        if (dr["AadharNo"].ToString().Length == 12)
                    //        {
                    //            var AadharNo = dr["AadharNo"].ToString();

                    //            var firstDigits = AadharNo.Substring(0, 8);
                    //            var lastDigits = AadharNo.Substring(AadharNo.Length - 4, 4);
                    //            var requiredMask = new String('X', AadharNo.Length - lastDigits.Length);
                    //            var maskedString = string.Concat(requiredMask, lastDigits);
                    //            var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0");
                    //            dr["AadharNo"] = maskedString;
                    //        }
                    //    }

                    //    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "inner aadhar loop " + dr["AadharNo"] }, httpException);
                    //}
                    //foreach (DataRow row in dt.Rows)
                    //{
                    //    string Photostr = row["Photo"].ToString();
                    //    System.Drawing.Image ImgFile = null;
                    //    if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(Photostr)))
                    //    {
                    //        ImgFile = System.Drawing.Image.FromFile(System.Web.HttpContext.Current.Server.MapPath(Photostr));
                    //    }
                    //    else
                    //    {
                    //        ImgFile = System.Drawing.Image.FromFile(System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/Balnk.png"));
                    //    }
                    //    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "inner photo loop " + ImgFile }, httpException);

                    //    string fileImg = "data:image/png;base64," + COMMON_FUNCTION.ImageToBase64(ImgFile, ImgFile.RawFormat);
                    //    sb.Append("<tr><td style='padding:2px;border: solid 0.2px #ccc;'>" + RowNo.ToString() + "</td><td style='padding:2px;border: solid 0.2px #ccc;'>" + row["POSTNAME"].ToString().Split('/')[0] + " </td><td style='padding:2px;border: solid 0.2px #ccc;'>" + row["AadharNo"].ToString() + "</td><td style='padding:2px;border: solid 0.2px #ccc;'><img width='55' height='55' src='" + fileImg + "' /></td><td style='padding:2px;border: solid 0.2px #ccc;'>" + row["Name"].ToString() + "</td><td style='padding:2px;border: solid 0.2px #ccc;'>" + row["Gender"].ToString() + "</td><td style='padding:2px;border: solid 0.2px #ccc;'>" + row["MemberDOB"].ToString() + "</td></tr>");
                    //    RowNo++;
                    //}

                    //sb.Append("</table></td></tr>");
                    ////  sb.Append("<tr><td valign='bottom' style='padding-right:15px;padding-left:15px; text-align: right; padding-top:10px;height:150px '><span style='font-family: Kruti Dev 010; font-size: 16px;'>jftLVªkj] laLFkk,a</span><span style='font-size: 13px;font-weight:normal;'>/ Registrar, Institutions</span></td></tr><tr><td style='width: 100%;height:50px;font-size:10px;border-top:1px solid #ccc;padding:3px;'><b>Note:</b> This is a digitally signed certificate and does not required any physical signature. Also, this certificate can be validated using QR Code </td></tr></table></body></html>");
                    //sb.Append("</table></body></html>");
                    //Document pdfDoc = new Document(iTextSharp.text.PageSize.A4);
                    //objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "generatePDFformember 2 " }, httpException);
                    //string textVal = sb.ToString();
                    //System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    //sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(textVal));
                    //string testflag = "";
                    //objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "generatePDFformember 3 " }, httpException);
                    //if (COMMON_FUNCTION.GeneratePDF(sb1, filepath))
                    //{
                    //    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "generatePDFformember 4 " }, httpException);

                    //    // testflag = fileName;
                    //}

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
        [ActionName("GenerateAadhaarOTPForEsing")]
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
        [ActionName("GenerateAadhaarOTPForEsignGcc")]
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

        //[ActionName("GenerateAadhaarOTPForEsing_Test")]
        //public string GenerateAadhaarOTPForEsing_Test(string callback)
        //{
        //    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        //    if (_LoggedInUser != null)
        //    {
        //        System.Web.HttpContext.Current.Session["Esign_callback"] = callback;
        //       // COMMON_FUNCTION.ImportExcelToSql();
        //        return COMMON_FUNCTION.GetTxnId(_LoggedInUser.AadharNo);
        //       //return GetTxnId(_LoggedInUser.AadharNo);
        //    }
        //    else
        //    {
        //        return "LOGOUT";
        //    }
        //    //return "RJVSD2343246566";
        //}

        //static string ComputeSha256Hash(byte[] text)
        //{
        //    // Create a SHA256   
        //    using (SHA256 sha256Hash = SHA256.Create())
        //    {
        //        // ComputeHash - returns byte array  
        //        byte[] bytes = sha256Hash.ComputeHash(text);

        //        // Convert byte array to a string   
        //        StringBuilder builder = new StringBuilder();
        //        for (int i = 0; i < bytes.Length; i++)
        //        {
        //            builder.Append(bytes[i].ToString("x2"));
        //        }
        //        return builder.ToString();
        //    }
        //}

        //public static string GetTxnId(string hdn_aadhar)
        //{
        //    hdn_aadhar = "420928155572";

        //    string url = "https://aadhaarauthtest.rajasthan.gov.in/AadhaarAuth";
        //    BRNEntity result = new BRNEntity();
        //    string _txnid = "";
        //    string ResponseCode = "";
        //    string status = "";
        //    string errormsg = string.Empty;
        //    string succmsg = string.Empty;
        //    if (System.Web.HttpContext.Current.Request.Url.Host.Contains("localhost") || System.Web.HttpContext.Current.Request.Url.Host.Contains("devitsandbox"))
        //    {
        //        //hdn_aadhar = "9887356163";
        //        hdn_aadhar = "420928155572";
        //    }
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(url))
        //        {
        //            string AppCode = "PRAVL22868ABFAP021";
        //            string SSOToken = System.Web.HttpContext.Current.Session.SessionID;
        //            Random rnd1 = new Random();
        //            var iRnd1 = rnd1.Next(10000000, 99999999);
        //            string RequestId = "PRAVL22868" + iRnd1.ToString();
        //            string ReturnURL = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
        //            // string ReturnURL = "https://rajsahakarapp.rajasthan.gov.in/pldb/biokyc";


        //            //string Checksum = CreateMD5(RequestId);
        //            string encAadharNo = hdn_aadhar.ToString(); // EncryptWithKey(hdn_aadhar.ToString(), GlobalConstant.AadharAuthEsigEncKey_Test, GlobalConstant.AadharAuthEsigAppCode_Test);
        //            string checksumStr = AppCode + RequestId + encAadharNo + "3";


        //            // byte[] pidXmlBytes = Encoding.UTF8.GetBytes(checksumStr);


        //            byte[] inputBytes = Encoding.UTF8.GetBytes(checksumStr);
        //            string Checksum = ComputeSha256Hash(inputBytes);
        //            using (WebClient Client = new WebClient())
        //            {
        //                try
        //                {
        //                    string data = JsonConvert.SerializeObject(new
        //                    {
        //                        SSOToken = SSOToken,
        //                        AppCode = AppCode,
        //                        RequestId = RequestId,
        //                        AadhaarNo = hdn_aadhar.ToString(),
        //                        ReturnURL = ReturnURL,
        //                        Purpose = "Send OTP For Esign Authentication.",
        //                        RequestType = 3,
        //                        Checksum = Checksum,
        //                        VerificationType = 0,
        //                        OperatorIdType = 0,
        //                        OperatorId = "2",
        //                    });

        //                    string encData = EsignAesEncryption.Encrypt(data, GlobalConstant.AadharAuthEsigEncKey_Test);
        //                    Sp_callTrackingInsert(" GetTxnIdOtp(:- ", encData);


        //                    var responseString = Task.Run(() =>
        //                    {
        //                        return SubmitFormAsync(url, AppCode, encData);
        //                    }).Result;

        //                    Sp_callTrackingInsert("GetTxnIdOtp responseString :-", responseString);
        //                    //var responseObj = JsonConvert.DeserializeObject<AadhaarResponse>(responseString);
        //                    //string encryptedData = responseObj.Data;

        //                    //string encryptedData = JsonConvert.DeserializeObject(responseString).ToString();


        //                    // Decrypt and handle the response
        //                    string decryptedData = EsignAesEncryption.Decrypt(responseString, GlobalConstant.AadharAuthEsigEncKey_Test);
        //                    var parsedResponse = JsonConvert.DeserializeObject<DecryptedAadhaarResponse>(decryptedData);

        //                    Sp_callTrackingInsert("GetTxnIdOtp parsedResponse :-", parsedResponse.ToString());
        //                    if (parsedResponse.ResponseCode == "200")
        //                    {
        //                        _txnid = parsedResponse.RequestId;
        //                        _txnid.Replace("\"", "");
        //                    }
        //                    else
        //                    {
        //                        _txnid = "NO" + "#" + parsedResponse.Message;
        //                    }


        //                }
        //                catch (Exception ex)
        //                {
        //                    ILogger objILogger = new DBLogger();
        //                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.Coommonfunction", FunctionName = "GetTxnId" }, ex);
        //                    _txnid = "NO" + "#" + ex.Message;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ILogger objILogger = new DBLogger();
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.Coommonfunction", FunctionName = "GetTxnId" }, ex);
        //        _txnid = "NO" + "#" + ex.Message;

        //    }
        //    return _txnid;
        //}

        //public static string SubmitFormAsync(string authRequestEncrypted, string appCode, string data)
        //{

        //    try
        //    {
        //        WebProxy proxy = new WebProxy("http://proxyserver:8080");

        //        var externalServiceUrl = authRequestEncrypted;
        //        var postData = $"AppCode={WebUtility.UrlEncode(appCode)}&Data={WebUtility.UrlEncode(data)}";
        //        var request = (HttpWebRequest)WebRequest.Create(externalServiceUrl);

        //        request.Method = "POST";
        //        request.ContentType = "application/x-www-form-urlencoded";
        //        request.Timeout = 30000;  // Set a timeout
        //        request.Proxy = proxy;

        //        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        //        {
        //            streamWriter.Write(postData);
        //        }

        //        var response = (HttpWebResponse)request.GetResponse();
        //        string responseText;
        //        using (var streamReader = new StreamReader(response.GetResponseStream()))
        //        {
        //            responseText = streamReader.ReadToEnd();
        //        }

        //        return responseText;  // Return the response text

        //    }
        //    catch (WebException ex)
        //    {
        //        using (var response = ex.Response as HttpWebResponse)
        //        {
        //            if (response != null)
        //            {
        //                string errorResponse;
        //                using (var reader = new StreamReader(response.GetResponseStream()))
        //                {
        //                    errorResponse = reader.ReadToEnd();
        //                }

        //                // Log the error code and response
        //                return $"Error: {response.StatusCode}, Message: {errorResponse}";
        //            }
        //            else
        //            {
        //                return $"Request failed: {ex.Message}";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return $"Unexpected error: {ex.Message}";
        //    }


        //}
        //[ActionName("GenerateAadhaarOTPForEsingDec_Test")]
        //public string GenerateAadhaarOTPForEsingDec_Test(string encData="")
        //{

        //  //  System.Web.HttpContext.Current.Session["TxnIdDecData"] =ViewData["TxnIdDecData"].ToString();

        //    string _txnid = "";
        //    if (encData == "" && System.Web.HttpContext.Current.Session["TxnIdDecData"]!=null) {
        //        encData = System.Web.HttpContext.Current.Session["TxnIdDecData"].ToString();

        //        Sp_callTrackingInsert("GetTxnIdOtp  Session Value:-", System.Web.HttpContext.Current.Session["TxnIdDecData"].ToString());
        //    }


        //    if (encData == null || encData == "" || encData.Contains("NO")) 
        //    {
        //        Sp_callTrackingInsert("GetTxnIdOtp  encData1:-", encData);
        //        return _txnid = "NO" + "#";              
        //    }

        //    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        //    if (_LoggedInUser != null)
        //    {
        //        Sp_callTrackingInsert("GetTxnIdOtp  encData:-", encData);

        //        string decryptedData = EsignAesEncryption.Decrypt(encData, GlobalConstant.AadharAuthEsigEncKey_Test);

        //        Sp_callTrackingInsert("GetTxnIdOtp  decryptedData:-", decryptedData);

        //        var parsedResponse = JsonConvert.DeserializeObject<DecryptedAadhaarResponse>(decryptedData);


        //        Sp_callTrackingInsert("GetTxnIdOtp parsedResponse :-", parsedResponse.ToString());
        //        if (parsedResponse.ResponseCode == "200")
        //        {
        //            _txnid = parsedResponse.RequestId;
        //            _txnid.Replace("\"", "");
        //        }
        //        else
        //        {
        //            _txnid = "NO" + "#" + parsedResponse.Message;
        //        }
        //        System.Web.HttpContext.Current.Session["TxnIdDecData"] = "";
        //        return _txnid;               
        //    }
        //    else
        //    {
        //        System.Web.HttpContext.Current.Session["TxnIdDecData"] = "";
        //        return "LOGOUT";
        //    }
        //    //return "RJVSD2343246566";
        //}


        //[ActionName("GenerateAadhaarOTPForEsing_Test1")]
        //public string GenerateAadhaarOTPForEsing_Test1()
        //{
        //    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        //    if (_LoggedInUser != null)
        //    {
        //        return COMMON_FUNCTION.GenerateOTPForEsingh1(_LoggedInUser.AadharNo);

        //    }
        //    else
        //    {
        //        return "LOGOUT";
        //    }
        //    //return "RJVSD2343246566";
        //}

        [ActionName("VerifyAadhaarOTPForEsing_Test")]
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


        //public string VerifyAadhaarOTPForEsing(string OTP, string TXN, string SocietyIDEnc)
        //{
        //    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        //    return VerifyAadhaarOTPForEsingh(OTP, TXN, SocietyIDEnc);
        //}

        [HttpPost]
        [ActionName("VerifyAadhaarOTPForEsing")]
        public string VerifyAadhaarOTPForEsingh(string OTP, string TXN, string SocietyIDEnc, int StatusId, string Remark, bool IsAmendment)
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
        [ActionName("EsingDocument")]
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
                    var result = COMMON_FUNCTION.EsinghPDF(TXN, Server.MapPath("~") + document.Replace("..", ""), _LoggedInUser.DISTRICT_ENG, "REGISTRAR");
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
        [ActionName("VerifyAadhaarOTPForEsinghAmendment")]
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
                                    ApplicationFlowModel _Modal = new ApplicationFlowModel();
                                    string BylawsStr = _Modal.SaveSocietyAmendmendData(SocietyIDEnc, CirtFile, MemberFile, BylawsFile, true, StatusId, AmendmentId).ToString();

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
        [ActionName("GetAmendmentChanges")]
        public JsonResult GetAmendmentChanges(string SocietyIDEnc, int AmendmentId)
        {
            ApplicationFlowModel AppFlowModel = new ApplicationFlowModel();
            DataTable dtAmendmentChanges = AppFlowModel.GetAmendmentChanges(SocietyIDEnc, AmendmentId);
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
        [ActionName("GetSocietyFeesStatus")]
        public string GetSocietyFeesStatusList(string SocietyRegiNo)
        {
            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objApplicationFlowModel.SocietyFeesStatuslist(SocietyRegiNo));
        }

        [HttpPost]
        [ActionName("GetRoleListForApplicationStatus")]
        public string GetRoleListForApplicationStatus(string SocietyIDEnc, int NextStatusId, int CurrentStatusId)
        {
            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objApplicationFlowModel.GetRoleListForApplicationStatus(SocietyIDEnc, NextStatusId, CurrentStatusId));
        }

        [HttpPost]
        [ActionName("GetOfficerBySocietyIDEnc")]
        public string GetOfficerBySocietyIDEnc(string SocietyIDEnc, int NextStatusId, int CurrentStatusId, int RoleId, int LoanBankId = 0)
        {
            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objApplicationFlowModel.GetOfficerBySocietyIDEnc(SocietyIDEnc, NextStatusId, CurrentStatusId, RoleId, LoanBankId));
        }

        [HttpPost]
        public string SaveSocietyObjectionBelong(string SocietyIDEnc, bool ObjectionFlagBasic, bool ObjectionFlagAdmin, bool ObjectionFlagOffice)
        {
            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());
            return objApplicationFlowModel.SaveSocietyObjectionBelong(SocietyIDEnc, ObjectionFlagBasic, ObjectionFlagAdmin, ObjectionFlagOffice).ToString();
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
            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());
            return objApplicationFlowModel.SaveSocietyStatusAction(SocietyIDEnc, StatusId, RegistrationNo).ToString();
        }

        [HttpPost]
        [ActionName("GetSocietyAppliStatusList")]
        public JsonResult GetSocietyAppliFlowLists(DataPagingModel Details)
        {

            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());

            DataTable DT = objApplicationFlowModel.GetSocietyAppliStatus(ref Details);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public JsonResult GetSocietyPaymentTransectin(DataPagingModel Details, string SocietyIdENC)
        {

            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());
            //return COMMON_FUNCTION.DataTableToJSONString(objApplicationFlowModel.GetSocietyAppliFlowList());
            // return COMMON_FUNCTION.DataTableToJSONString(objApplicationFlowModel.GetSocietyAppliFlowList(Details));

            DataTable DT = objApplicationFlowModel.GetSocietyPaymentTransectin(ref Details, SocietyIdENC);
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

            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());
            return objApplicationFlowModel.SaveSocietystatusdata(StatusId, SocietyIDEnc, Remark, ApplicationFlowID, fileName).ToString();
        }

        [HttpPost]
        public string SaveStatusApplicationPayment(string SocietyIDEnc, string Remark, int ApplicationPaymentId, string ApplicationPayment)
        {
            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());
            return objApplicationFlowModel.SaveStatusApplicationPayment(SocietyIDEnc, Remark, ApplicationPaymentId, ApplicationPayment).ToString();
        }

        [HttpPost]
        public string SetApplicationUser(string SocietyIDEnc, string Remark, int OfficeId, int ApplicationFlowID)
        {
            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());
            return objApplicationFlowModel.SetApplicationUser(SocietyIDEnc, Remark, OfficeId, ApplicationFlowID).ToString();
        }

        public ViewResult CheckEsign()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetSocietyApplicantDetails(string SocietyIdENC, int AmendmentId = 0)
        {

            SocietyModel objModel = new SocietyModel(new DBLogger());
            //return COMMON_FUNCTION.DataTableToJSONString(objApplicationFlowModel.GetSocietyAppliFlowList());
            // return COMMON_FUNCTION.DataTableToJSONString(objApplicationFlowModel.GetSocietyAppliFlowList(Details));

            DataTable DT = objModel.GetSocietyApplicantDetails(SocietyIdENC, AmendmentId);
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
            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());
            DT = objApplicationFlowModel.GetSocietieCheckList(ApplicationNo);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }
        [HttpPost]
        public string DeleteSocietyAppliStatuschecklist(int Societyid)
        {
            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());
            return objApplicationFlowModel.DeleteSocietieCheckList(Societyid).ToString();
        }


        #region AadharEsignEncryption  Final 30 dec2024

        [ActionName("GenerateAadhaarOTPEsign_Test")]
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

        [ActionName("GenerateAadhaarOTPEsign_Test1")]
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


        [ActionName("GenerateAadhaarOTPDec_Test")]
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



        [ActionName("GenerateAadhaarOTPDec_Test1")]
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
                ApplicationFlowModel AppFlowModel = new ApplicationFlowModel();
                dt = AppFlowModel.GetPendingEsignDoc();

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
                                var status = AppFlowModel.PendingEsignDocStatusChange(Convert.ToInt32(row["SocietyId"]));
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
        [ActionName("GetSocietyBankDetails")]
        public JsonResult GetSocietyBankDetails(int societyId)
        {
            try
            {
                ApplicationFlowModel objModel = new ApplicationFlowModel(new DBLogger());
                DataSet ds = objModel.GetSocietyBankDetails(societyId);

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
                        Branch_Id = row["Branch_Id"]?.ToString(),
                        CCBNameEn = row["CCBNameEn"]?.ToString(),
                        BranchNameEn = row["BranchNameEn"]?.ToString()
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
        [ActionName("SavePacsCode")]
        public JsonResult SavePacsCode(string SocietyIDEnc, string PACS_Code)
        {
            try
            {
                ApplicationFlowModel obj = new ApplicationFlowModel(new DBLogger());

                int result = obj.UpdatePacsCode(SocietyIDEnc, PACS_Code);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }


        //Get Bank Details
        [HttpGet]
        [ActionName("GetSocietyBankAndBranch")]
        public JsonResult GetSocietyBankAndBranch(int societyId, int statusId)
        {
            try
            {
                ApplicationFlowModel objModel = new ApplicationFlowModel(new DBLogger());
                DataSet ds = objModel.GetSocietyBankAndBranchDetails(societyId, statusId);

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


        //Added By Narpat Kumawat
        /// <summary>
        /// Generate Bank Opening Latter 
        /// </summary>
        /// <param name="SocietyIDEnc"></param>
        /// <param name="StatusId"></param>
        /// <param name="Remark"></param>
        /// <param name="UserId"></param>
        /// <param name="IsAmendment"></param>
        /// <param name="AmendmendAmount"></param>
        /// <param name="ReceiptNumber"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveSocietyBankAccountLatter(String SocietyIDEnc, int StatusId, string Remark, int UserId, bool IsAmendment = false, string AmendmendAmount = "", string ReceiptNumber = "")
        {
            DBLogger objDBLogger = new DBLogger();
            objDBLogger.ErrorHandle(new ErrorLogEntity()
            {
                PageName = "ApplicationFlowController",
                FunctionName = "public JsonResult SaveSocietyBankAccountLatter() Step 1 start " + "Calling"
            }, new System.Exception("calling -1 "));
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            PacsResponseModel responseModel = new PacsResponseModel();
            string bankData = string.Empty;
            if (_LoggedInUser == null)
            { return Json("LOGOUT"); }
            ApplicationFlowModel _Modal = new ApplicationFlowModel();

            ModelState.Remove("UpdationDate");
            ModelState.Remove("Createdate");
            if (!ModelState.IsValid) { return Json(JsonModelError.Get(ModelState.Values)); }

            int result = 0;
            if (StatusId == 14)
            {
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "ApplicationFlowController",
                    FunctionName = "public JsonResult SaveSocietyBankAccountLatter() Step 2 start " + "Calling" + StatusId
                }, new System.Exception("calling -1 " + StatusId));
                bankData = GenerateBankAccountLatter(SocietyIDEnc);
                if (!string.IsNullOrEmpty(bankData))
                {
                    responseModel = JsonConvert.DeserializeObject<PacsResponseModel>(bankData);
                }
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "ApplicationFlowController",
                    FunctionName = "public JsonResult SaveSocietyBankAccountLatter() Step 3 start " + "Calling" + bankData
                }, new System.Exception("calling -1 " + bankData));
                if (responseModel != null && responseModel.Success == false)
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

                    DocPath = responseModel?.DocPath ?? "",
                    DocType = responseModel?.DocType ?? "",
                    DocName = responseModel?.DocName ?? "",
                    ReturnPath = responseModel?.ReturnPath ?? "",
                    Location = responseModel?.Location ?? "",
                    Designation = responseModel?.Designation ?? "",
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (StatusId == 17 || StatusId == 26)
                {
                    if (_Modal.GetActIdBySocietyId(SocietyIDEnc) == 3)
                    {

                    }
                }
                result = _Modal.AddApplicationFlow(SocietyIDEnc, StatusId, Remark, UserId, IsAmendment, AmendmendAmount, ReceiptNumber);
            }
            return Json(new
            {
                Success = true,
                DocPath = responseModel?.DocPath ?? "",
                DocType = responseModel?.DocType ?? "",
                DocName = responseModel?.DocName ?? "",
                ReturnPath = responseModel?.ReturnPath ?? "",
                Location = responseModel?.Location ?? "",
                Designation = responseModel?.Designation ?? "",
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Bank Opening Certificate 
        /// </summary>
        /// <param name="SocietyIDEnc"></param>
        /// <returns></returns>
        public string GenerateBankAccountLatter(string SocietyIDEnc)
        {
            int result = -1;
            string lbl_Registraion = "";
            string lbl_CEOName = "";
            string lbl_TodayDate = "";
            string lbl_Societyaddress = "";
            string lbl_RegistrationNo = "";
            string lbl_Amount = "";
            string lbl_AO_ADD_DistrictEn = "";
            string pdfPath = string.Empty;
            string folderPath = string.Empty;
            DataTable dt = new DataTable();
            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
            if (!ModelState.IsValid)
                return "0";
            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());
            dt = objApplicationFlowModel.GetApplicationPdfdata(SocietyIDEnc);
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

            if (result == 0)
            {
                StringBuilder sb = new StringBuilder();
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
                var fileName = Guid.NewGuid().ToString("N").ToUpper() + ".pdf";
                string QRCoder = COMMON_FUNCTION.GenerateQRCode("https://rajsahakarapp.rajasthan.gov.in/Content/SocietyProfiling/" + lbl_Registraion.Replace("-", "").Replace("/", "").Replace("_", "") + "/" + fileName);
                sb.Append("<img style='width:180px:height:180px;' src='" + QRCoder + "'/>");
                sb.Append("</td></tr>");
                sb.Append("</table>");
                sb.Append("</div>");

                StringBuilder sb1 = new StringBuilder();
                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));

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

                    folderPath = "~/Content/SocietyProfiling/";
                    pdfPath = folderPath + lbl_Registraion.ToString() + "/" + fileName;
                    if (pdfPath != "" && pdfPath != "Fail")
                    {
                        return JsonConvert.SerializeObject(new
                        {
                            Success = true,
                            DocPath = pdfPath,
                            DocType = "BankOpening Certificate",
                            DocName = fileName,
                            ReturnPath = Url.Action("ApplicationFlow", "Society"),
                            Location = lbl_AO_ADD_DistrictEn,
                            Designation = "REGISTRAR"
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
                        ReturnPath = "",
                        Location = "",
                        Designation = ""
                    });
                }
            }
            else
            {
                folderPath = "~/Content/SocietyProfiling/";
                pdfPath = folderPath + lbl_Registraion.ToString() + "/" + dt.Rows[0]["BankOpeningCertificate"].ToString();
                return JsonConvert.SerializeObject(new
                {
                    Success = true,
                    DocPath = pdfPath,
                    DocType = "BankOpening Certificate",
                    DocName = dt.Rows[0]["BankOpeningCertificate"].ToString(),
                    ReturnPath = Url.Action("ApplicationFlow", "Society"),
                    Location = lbl_AO_ADD_DistrictEn,
                    Designation = "REGISTRAR"
                });
            }
        }


        [HttpPost]
        [ActionName("GenerateCertificateLatter")]
        public JsonResult GenerateCertificateLatter(string SocietyIDEnc, int StatusId, string Remark, long UserId = 0, bool IsAmendment = false, int type = 0)
        {
            string resultData = string.Empty;
            PacsResponseModel responseModel = new PacsResponseModel();

            if (StatusId == 0 || string.IsNullOrWhiteSpace(Remark))
            {
                return Json(new
                {
                    Success = false,
                    Message = "Fail"
                });
            }

            try
            {
                switch (type)
                {
                    case 1:
                        resultData = GenerateCertificateLatter(SocietyIDEnc);
                        break;

                    case 2:
                        resultData = GenerateMemberCertificateLatter(SocietyIDEnc);
                        break;

                    case 3:
                        resultData = GenerateRulesReguCertificateLatter(SocietyIDEnc, 3);
                        break;

                    case 4:
                        resultData = GenerateObjectCertificateLatter(SocietyIDEnc, 4);
                        break;

                    default:
                        return Json(new
                        {
                            Success = false,
                            Message = "Invalid Type"
                        }, JsonRequestBehavior.AllowGet);
                }

                if (!string.IsNullOrWhiteSpace(resultData))
                {
                    responseModel = JsonConvert.DeserializeObject<PacsResponseModel>(resultData);
                }

                if (responseModel == null)
                {
                    return Json(new
                    {
                        Success = false,
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
                    Type = responseModel.Type ?? "0"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public string GenerateCertificateLatter(string SocietyIDEnc)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {

                    ApplicationFlowModel AppFlowModel = new ApplicationFlowModel();
                    DataTable dt = AppFlowModel.GetScietyAdministrativeDetail(SocietyIDEnc, 1, " ");
                    string fileNameCerti = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace("_", "").Replace(" ", "").ToUpper() + ".pdf";
                    string fileNameMemList = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace("_", "").Replace(" ", "").ToUpper() + ".pdf";
                    //remove this after check the condition (savecertificate)
                    //string savecertificate = SaveCertificate(SocietyIDEnc, fileNameCerti, fileNameMemList, true);
                    if (dt.Rows.Count > 0)
                    {

                        //DataTable tblFiltered = dt.AsEnumerable().Where(row => row.Field<string>("POSTNAME").ToUpper() == "SECRETARY").CopyToDataTable();
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
                            bool isPdfFile = COMMON_FUNCTION.GeneratePDF(sb1, filepath, "");
                            if (!isPdfFile)
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
                                    Type = ""
                                });
                            }
                            else
                            {
                                return JsonConvert.SerializeObject(new
                                {
                                    Success = true,
                                    DocPath = filepath,
                                    DocType = "Certificate Issue",
                                    DocName = fileNameCerti,
                                    ReturnPath = Url.Action("ApplicationFlow", "ApplicationFlow"),
                                    Location = lbl_AO_ADD_DistrictEn,
                                    Designation = "REGISTRAR",
                                    Type = "1"
                                });
                            }

                        }
                        else
                        {
                            fileNameCerti = dt.Rows[0]["RegistrationCertificateFile"].ToString();
                            return JsonConvert.SerializeObject(new
                            {
                                Success = true,
                                DocPath = System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraionno.Replace("/", "").Replace("_", "").Replace("-", "").Replace(" ", "") + "/" + fileNameCerti),
                                DocType = "Certificate Issue",
                                DocName = fileNameCerti,
                                ReturnPath = Url.Action("ApplicationFlow", "ApplicationFlow"),
                                Location = lbl_AO_ADD_DistrictEn,
                                Designation = "REGISTRAR",
                                Type = "1"
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
                            ReturnPath = "",
                            Location = "",
                            Designation = "",
                            Type = ""
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
                        ReturnPath = "",
                        Location = "",
                        Designation = "",
                        Type = ""
                    });
                }
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "CertificateGenerate catch " }, ex);
                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    Location = "",
                    Designation = "",
                    Type = ""
                });
            }
        }
        public string GenerateMemberCertificateLatter(string SocietyIDEnc)
        {
            string pdfPath = string.Empty;
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {
                    ApplicationFlowModel AppFlowModel = new ApplicationFlowModel();
                    string fileNameMemList = Guid.NewGuid().ToString("N").ToUpper() + ".pdf";
                    DataTable dt = AppFlowModel.GetScietyAdministrativeDetail(SocietyIDEnc, 1, " ");
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
                            DataTable dt1 = AppFlowModel.GetScietyAdministrativeDetail(SocietyIDEnc, 2, "");
                            if (dt1.Rows.Count > 0)
                            {
                                pdfPath = generatePDFformemberList(dt1, System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraionno.Replace("/", "").Replace("_", "").Replace("-", "").Replace(" ", "") + "/") + fileNameMemList, lbl_InstitutionName, lbl_AO_ADD_DistrictEn, lbl_AO_ADD_DistrictHi, lbl_Registraionno, lbl_ActId, IsStateLevel);
                                if (pdfPath != null && pdfPath != "Fail")
                                {
                                    return JsonConvert.SerializeObject(new
                                    {
                                        Success = true,
                                        DocPath = pdfPath,
                                        DocType = "Member Certificate",
                                        DocName = fileNameMemList,
                                        ReturnPath = Url.Action("ApplicationFlow", "ApplicationFlow"),
                                        Location = lbl_AO_ADD_DistrictEn,
                                        Designation = "REGISTRAR",
                                        Type = "2"
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
                                        Type = "0"
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
                                    ReturnPath = "",
                                    Location = "",
                                    Designation = "",
                                    Type = "0"
                                });
                            }

                        }
                        else
                        {
                            return pdfPath = "Fail";
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
                            ReturnPath = "",
                            Location = "",
                            Designation = "",
                            Type = "0"
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
                        ReturnPath = "",
                        Location = "",
                        Designation = "",
                        Type = "0"
                    });
                }
            }

            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity()
                { PageName = "ApplicationFlowController", FunctionName = "GenerateMemberCertificateLatter(string SocietyIDEnc)" }, ex);
                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    Location = "",
                    Designation = "",
                    Type = "0"
                });
            }
        }
        public string generatePDFformemberList(DataTable dt, string filepath, string lbl_InstitutionName, string lbl_AO_ADD_DistrictEn, string lbl_AO_ADD_DistrictHi, string lbl_Registraionno, string actId, string IsStateLevel)
        {
            ILogger objILogger = new DBLogger();
            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            string pdfPath = string.Empty;
            try
            {
                int RowNo = 1;
                if (dt.Rows.Count > 0)
                {
                    string fileNameMemList = Guid.NewGuid().ToString("N").ToUpper() + ".pdf";

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
                    sb.Append("</div>");
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));
                    if (Request.Url.Host.Contains("localhost") && COMMON_FUNCTION.GeneratePDF1(sb1, filepath, ""))
                    {
                        return filepath;
                    }
                    else if (COMMON_FUNCTION.GeneratePDF(sb1, filepath, ""))
                    {
                        return filepath;
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

                        return filepath = "Fail";
                    }
                }
                else
                {
                    return filepath = "Fail";
                }
            }
            catch (Exception ex)
            {

                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "generatePDFformember catch error. RegNo- " + lbl_Registraionno + "String:- " + sb1.ToString() + "String2:-" + sb.ToString() }, ex);
                return filepath = "Fail";
            }
            //string BylawsStr = AppFlowModel.GetSocietyBylawsFile(SocietyIDEnc);
        }
        public string GenerateRulesReguCertificateLatter(string SocietyIDEnc, int docType)
        {
            string fullPath = string.Empty;
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {

                    ApplicationFlowModel AppFlowModel = new ApplicationFlowModel();
                    DataTable dt = AppFlowModel.GetScietyAdministrativeDetail(SocietyIDEnc, 1, " ");
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

                        string BylawsStr = AppFlowModel.GetSocietyBylawsFile(SocietyIDEnc, 1, docType);
                        if (BylawsStr == "Fail" || BylawsStr == "-1")
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
                                Type = "0"
                            });
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(lbl_SocietyDirectory))
                            {
                                lbl_SocietyDirectory = lbl_SocietyDirectory + "/";
                            }
                            else { lbl_SocietyDirectory = lbl_Registraionno.Replace("/", "").Replace("_", "").Replace("-", "").Replace(" ", "") + "/"; }

                            fullPath = Path.Combine(Server.MapPath("~/Content/SocietyProfiling/"), lbl_SocietyDirectory, BylawsStr);
                            string fileName = Path.GetFileName(fullPath);
                            return JsonConvert.SerializeObject(new
                            {
                                Success = true,
                                DocPath = fullPath,
                                DocType = "Bylaws Certificate",
                                DocName = fileName,
                                ReturnPath = Url.Action("ApplicationFlow", "ApplicationFlow"),
                                Location = lbl_AO_ADD_DistrictEn,
                                Designation = "REGISTRAR",
                                Type = "3"
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
                            ReturnPath = "",
                            Location = "",
                            Designation = "",
                            Type = "0"
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
                        ReturnPath = "",
                        Location = "",
                        Designation = "",
                        Type = "0"
                    });
                }
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "public string GenerateBylawsCertificateLatter(string SocietyIDEnc)" }, ex);
                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    Location = "",
                    Designation = "",
                    Type = "0"
                });
            }
        }
        public string GenerateObjectCertificateLatter(string SocietyIDEnc, int docType)
        {
            string fullPath = string.Empty;
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {

                    ApplicationFlowModel AppFlowModel = new ApplicationFlowModel();
                    DataTable dt = AppFlowModel.GetScietyAdministrativeDetail(SocietyIDEnc, 1, " ");
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

                        string BylawsStr = AppFlowModel.GetSocietyBylawsFile(SocietyIDEnc, 1, docType);

                        if (BylawsStr == "Fail" || BylawsStr == "-1")
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
                                Type = "0"
                            });
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(lbl_SocietyDirectory))
                            {
                                lbl_SocietyDirectory = lbl_SocietyDirectory + "/";
                            }
                            else { lbl_SocietyDirectory = lbl_Registraionno.Replace("/", "").Replace("_", "").Replace("-", "").Replace(" ", "") + "/"; }
                            fullPath = Path.Combine(Server.MapPath("~/Content/SocietyProfiling/"), lbl_SocietyDirectory, BylawsStr);
                            string fileName = Path.GetFileName(fullPath);
                            return JsonConvert.SerializeObject(new
                            {
                                Success = true,
                                DocPath = fullPath,
                                DocType = "Objective Certificate",
                                DocName = fileName,
                                ReturnPath = Url.Action("ApplicationFlow", "ApplicationFlow"),
                                Location = lbl_AO_ADD_DistrictEn,
                                Designation = "REGISTRAR",
                                Type = "4"
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
                            ReturnPath = "",
                            Location = "",
                            Designation = "",
                            Type = "0"
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
                        ReturnPath = "",
                        Location = "",
                        Designation = "",
                        Type = "0"
                    });
                }
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "CertificateGenerate catch " }, ex);
                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    Location = "",
                    Designation = "",
                    Type = "0"
                });
            }
        }

        public string CertificateIssueEsign(string SocietyIDEnc, int StatusId, string Remark, bool IsAmendment)
        {
            string message = string.Empty;
            string succmsg = string.Empty;
            string pdfPath = "";
            string finalPath = string.Empty;

            if (StatusId == 0 || string.IsNullOrWhiteSpace(Remark))
            {
                return "Fail";
            }
            try
            {
                ApplicationFlowModel AppFlowModel = new ApplicationFlowModel();
                DataTable dt = AppFlowModel.GetScietyAdministrativeDetail(SocietyIDEnc, 1, " ");

                if (dt.Rows.Count > 0)
                {
                    string lbl_InstitutionName = dt.Rows[0]["SocietyName"].ToString();
                    string lbl_Name = dt.Rows[0]["Name"].ToString();
                    string lbl_AO_ADD_DistrictEn = dt.Rows[0]["DISTRICT_ENG"].ToString();
                    string lbl_AO_ADD_DistrictHi = dt.Rows[0]["DISTRICT_MANGAL"].ToString();
                    string lbl_Registraionno = dt.Rows[0]["RegistrationNo"].ToString();

                    string dirName = lbl_Registraionno.Replace("/", "").Replace("_", "").Replace("-", "").Replace(" ", "") + "/";

                    // DOC GEN
                    pdfPath = CertificateGenerate(SocietyIDEnc);
                    pdfPath = pdfPath.Split('$')[0];

                    finalPath = eSignModel.MergeMultiplePdfFiles(pdfPath, dirName);
                    string fileName = Path.GetFileName(finalPath);

                    if (!finalPath.Contains("Fail"))
                    {
                        return JsonConvert.SerializeObject(new
                        {
                            Success = true,
                            DocPath = finalPath,
                            DocType = "Certificate Issue",
                            DocName = fileName,
                            ReturnPath = Url.Action("ApplicationFlow", "Society", new { SocietyIDEnc = SocietyIDEnc }),
                            Location = lbl_AO_ADD_DistrictEn,
                            Designation = "REGISTRAR"
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
                            Designation = ""
                        });
                    }
                }

                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    Message = "No data found.",
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    Location = "",
                    Designation = ""
                });
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "public string CertificateIssueEsign(string SocietyIDEnc, int StatusId, string Remark, bool IsAmendment)" }, ex);

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
        [ActionName("GenerateAmendmentCertificate")]
        public string GenerateAmendmentCertificate(string SocietyIDEnc, int StatusId, string Remark, bool IsAmendment, int type, int AmendmentId)
        {
            string message = string.Empty;
            string succmsg = string.Empty;
            string pdfPath = "";
            string finalPath = string.Empty;

            if (StatusId == 0 || string.IsNullOrWhiteSpace(Remark))
                return "Fail";

            try
            {
                ApplicationFlowModel AppFlowModel = new ApplicationFlowModel();
                DataTable dt = AppFlowModel.GetAmendmentChanges(SocietyIDEnc, AmendmentId);

                if (dt.Rows.Count > 0)
                {
                    string lbl_AO_ADD_DistrictEn = dt.Rows[0]["DISTRICT_ENG"].ToString();
                    string lbl_Registraionno = dt.Rows[0]["RegistrationNo"].ToString();

                    string dirName = lbl_Registraionno.Replace("/", "").Replace("_", "").Replace("-", "").Replace(" ", "") + "/";

                    //DOC GEN
                    string CirtFile = ""; string MemberFile = ""; string BylawsFile = "";
                    pdfPath = CertificateGenerateAmendment(ref CirtFile, ref MemberFile, ref BylawsFile, SocietyIDEnc, AmendmentId);
                    pdfPath = pdfPath.Split('$')[0];

                    finalPath = eSignModel.MergeMultiplePdfFiles(pdfPath, dirName);
                    string fileName = Path.GetFileName(finalPath);
                    if (!finalPath.Contains("Fail"))
                    {
                        return JsonConvert.SerializeObject(new
                        {
                            Success = true,
                            DocPath = finalPath,
                            DocType = "Amendment Certificate",
                            DocName = fileName,
                            ReturnPath = Url.Action("ApplicationFlow", "SocietyActionViewAmendment1958", new { SocietyIDEnc = SocietyIDEnc }),
                            Location = lbl_AO_ADD_DistrictEn,
                            Designation = "REGISTRAR"
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
                            Designation = ""
                        });
                    }
                }

                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    Message = "No data found.",
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    Location = "",
                    Designation = ""
                });



            }
            catch (Exception ex)
            {
                DBLogger objDBLogger1 = new DBLogger();
                objDBLogger1.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "public string GenerateAmendmentCertificate(string SocietyIDEnc, int StatusId, string Remark, bool IsAmendment, int AmendmentId)" }, ex);


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
        [ActionName("SaveSocietyApplicationFlowStatusNew")]
        public JsonResult SaveSocietyApplicationFlowStatusNew(string SocietyIDEnc, int StatusId, string Remark, int UserId, bool IsAmendment = false, string AmendmendAmount = "", string ReceiptNumber = "", string CapitalAmount = null, string AccountNumber = "", string IFSCCode = "", string BankName = "", string RecieptPath = "", int Id = 0, int CCB_Id = 0, int Branch_Id = 0, string PACS_Code = "", DateTime? membershipAppDate = null, DateTime? membershipDate = null, string membershipRemark = "", bool? IsPacsMembershipReg = null)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            if (_LoggedInUser == null)
            {
                return Json("LOGOUT");
            }
            DBLogger objDBLogger = new DBLogger();
            objDBLogger.ErrorHandle(new ErrorLogEntity()
            {
                PageName = "ApplicationController with Status or Remarks",
                FunctionName = "SaveSocietyApplicationFlowStatusNew(responseModel :-) Step 1 start SaveSocietyApplicationFlowStatusNew StatusId " + StatusId
            }, new System.Exception("Remark:" + Remark));

            if (StatusId == 0 && string.IsNullOrWhiteSpace(Remark))
            {
                return Json(-3);
            }
            ApplicationFlowModel _Modal = new ApplicationFlowModel();

            if (UserId == 0)
            {
                // UserId = Convert.ToInt32(_LoggedInUser.UserId);
            }

            ModelState.Remove("UpdationDate");
            ModelState.Remove("Createdate");

            if (!ModelState.IsValid)
                return Json(JsonModelError.Get(ModelState.Values));

            int result = 0;

            // Existing logic for Status 17 & 26
            if (StatusId == 17 || StatusId == 26)
            {
                if (_Modal.GetActIdBySocietyId(SocietyIDEnc) == 3)
                {
                    // Keep your existing logic here if required
                }
            }

            // Existing AddApplicationFlow call
            result = _Modal.AddApplicationFlow(SocietyIDEnc, StatusId, Remark, UserId, IsAmendment, AmendmendAmount, ReceiptNumber, CapitalAmount, AccountNumber, IFSCCode, BankName, RecieptPath, Id, CCB_Id, Branch_Id, PACS_Code, membershipAppDate, membershipDate, membershipRemark, IsPacsMembershipReg);
            // Pradeep 01-08-2024
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

        //Amendment 01-07-2026
        [HttpPost]
        [ActionName("GenerateAmendmentCertificateIssue")]
        public JsonResult GenerateAmendmentCertificateIssue(string SocietyIDEnc, int StatusId, string Remark, bool IsAmendment, int AmendmentId, int type, bool AmedmentRequestType = false)
        {
            string resultData = string.Empty;
            PacsResponseModel responseModel = new PacsResponseModel();

            if (StatusId == 0 || string.IsNullOrWhiteSpace(Remark))
            {
                return Json(new
                {
                    Success = false,
                    Message = "Fail"
                });
            }

            try
            {
                switch (type)
                {
                    case 1:
                        resultData = GenerateAmendmentCertificateLatter(SocietyIDEnc, AmendmentId, AmedmentRequestType);
                        break;

                    case 2:
                        resultData = GenerateAmendmentMemberLatter(SocietyIDEnc, AmendmentId, AmedmentRequestType);
                        break;

                    case 3:
                        resultData = GenerateAmendmentBylawsLatter(SocietyIDEnc, AmendmentId, AmedmentRequestType);
                        break;

                    default:
                        return Json(new
                        {
                            Success = false,
                            Message = "Invalid Type"
                        }, JsonRequestBehavior.AllowGet);
                }

                if (!string.IsNullOrWhiteSpace(resultData))
                {
                    responseModel = JsonConvert.DeserializeObject<PacsResponseModel>(resultData);
                }

                if (responseModel == null)
                {
                    return Json(new
                    {
                        Success = false,
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
                    Type = responseModel.Type ?? "0"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public string GenerateAmendmentCertificateLatter(string SocietyIDEnc, int AmendmentId, bool AmedmentRequestType)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {
                    ApplicationFlowModel AppFlowModel = new ApplicationFlowModel();
                    DataTable dtAmendmentChanges = AppFlowModel.GetAmendmentChanges(SocietyIDEnc, AmendmentId);
                    string fileNameCerti = "";
                    string lbl_SocietyDirectory = "";
                    string lbl_AO_ADD_DistrictEn = "";

                    string lbl_InstitutionName = "";
                    string lbl_AO_ADD_DistrictHi = "";
                    string lbl_Registraionno = "";
                    string lbl_ActId = "";
                    string lbl_OldName = "";
                    string lbl_RegistrationDate = "";
                    string filepath = string.Empty;

                    if (dtAmendmentChanges.Rows.Count > 0)
                    {
                        lbl_Registraionno = dtAmendmentChanges.Rows[0]["RegistrationNo"].ToString();
                        try
                        {
                            if (lbl_Registraionno.Replace(" ", "").Replace("/", "").Replace("_", "") != lbl_Registraionno.Replace(" ", "").Replace("/", "").Replace("-", "").Replace("_", ""))
                            {
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

                        if (dtAmendmentChanges.Rows[0]["Name"] != null && dtAmendmentChanges.Rows[0]["Name"].ToString() != "")
                        {
                            lbl_AO_ADD_DistrictEn = dtAmendmentChanges.Rows[0]["DISTRICT_ENG"].ToString();
                            lbl_AO_ADD_DistrictHi = dtAmendmentChanges.Rows[0]["DISTRICT_MANGAL"].ToString();
                            lbl_Registraionno = dtAmendmentChanges.Rows[0]["RegistrationNo"].ToString();
                            lbl_ActId = dtAmendmentChanges.Rows[0]["ActId"].ToString();
                            fileNameCerti = Guid.NewGuid().ToString("N").ToUpper() + ".pdf";
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
                            sb.Append("<b><label style='font-size:22px;'> " + UnicodeToKrutidev.GetKrutidev("रजिस्ट्रीकरण प्रमाण - पत्र") + " </label></b>");
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

                            sb.Append("<tr><td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'><p style='text-align: justify;text-justify;'>  यह प्रमाणित किया जाता है कि <b> " + lbl_OldName.ToUpper() + " </b> जिला <b> " + lbl_AO_ADD_DistrictHi.ToUpper() + " </b>  का रजिस्ट्रेशन <b>' " + actNameHi + " '</b>  के अन्तर्गत दिनांक <b>" + lbl_RegistrationDate + "</b> को किया गया था। </p>");
                            sb.Append("<p style='text-align: justify;text-justify;'> अब इस संस्थान का नाम परिवर्तित कर <b> " + lbl_InstitutionName.ToUpper() + " </b> जिला <b> " + lbl_AO_ADD_DistrictHi.ToUpper() + " </b>  कर दिया गया है। यह परिवर्तन राजस्थान संस्था रजिस्ट्रीकरण अधिनियम ," + (lbl_ActId == "1" ? "1958" : lbl_ActId == "3" ? "2001" : "2005") + " की धारा -12 के प्रावधानान्तर्गत किया गया है। </p>");
                            sb.Append("<p style='text-align: justify;text-justify;'> यह प्रमाण पत्र मेरे हस्ताक्षर एवं कार्यालय की मुहर से दिनांक <b>" + DateTime.Now.ToString("dd-MM-yyyy") + "</b> को " + lbl_AO_ADD_DistrictHi + " में जारी किया गया। </p></td></tr>");
                            sb.Append("<tr><td style='text-align: left;'>");
                            string QRCoder = COMMON_FUNCTION.GenerateQRCode("https://rajsahakarapp.rajasthan.gov.in/Content/SocietyProfiling/" + lbl_Registraionno.Replace("-", "").Replace("/", "").Replace("_", "").Replace(" ", "") + "/" + fileNameCerti);
                            sb.Append("<img style='width:180px:height:180px;' src='" + QRCoder + "'/>");
                            sb.Append("</td></tr>");
                            sb.Append("</table>");
                            sb.Append("</div>");
                            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                            sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));

                            if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraionno.Replace("-", "").Replace("/", "").Replace("_", "").Replace(" ", "") + "/")))
                            {
                                System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraionno.Replace("-", "").Replace("/", "").Replace("_", "").Replace(" ", "") + "/"));
                            }
                            filepath = System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraionno.Replace("-", "").Replace("/", "") + "/").Replace("_", "").Replace(" ", "") + fileNameCerti;
                            if (!COMMON_FUNCTION.GeneratePDF(sb1, filepath, ""))
                            {
                                throw new Exception("Error in Generate Certificate");
                            }
                        }

                    }

                    lbl_SocietyDirectory = lbl_SocietyDirectory.Replace("-", "");
                    if (!string.IsNullOrWhiteSpace(lbl_SocietyDirectory))
                    { lbl_SocietyDirectory = System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_SocietyDirectory + "/" + fileNameCerti); }
                    else
                    { lbl_SocietyDirectory = filepath; }

                    if (!string.IsNullOrWhiteSpace(fileNameCerti))
                    {
                        return JsonConvert.SerializeObject(new
                        {
                            Success = true,
                            DocPath = lbl_SocietyDirectory,
                            DocType = "Amendment Certificate",
                            DocName = fileNameCerti,
                            ReturnPath = Url.Action("ApplicationFlowAmendment", "ApplicationFlow"),
                            Location = lbl_AO_ADD_DistrictEn,
                            Designation = "REGISTRAR",
                            Type = "1"
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
                            Type = "0"
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
                        ReturnPath = "",
                        Location = "",
                        Designation = "",
                        Type = "0"
                    });
                }
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "CertificateGenerate catch " }, ex);

                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    Location = "",
                    Designation = "",
                    Type = "0"
                });
            }
        }

        public string GenerateAmendmentMemberLatter(string SocietyIDEnc, int AmendmentId, bool AmedmentRequestType)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {

                    ApplicationFlowModel AppFlowModel = new ApplicationFlowModel();
                    DataTable dtAmendmentChanges = AppFlowModel.GetAmendmentChanges(SocietyIDEnc, AmendmentId);
                    string fileNameMemList = "";
                    string lbl_SocietyDirectory = "";
                    string lbl_AO_ADD_DistrictEn = "";

                    string lbl_InstitutionName = "";
                    string lbl_AO_ADD_DistrictHi = "";
                    string lbl_Registraionno = "";
                    string lbl_ActId = "";

                    DataTable dt1 = AppFlowModel.GetAdministrativeDetailsAmendment(SocietyIDEnc, "", AmendmentId);//ad.UserType in (4
                    string memberResult = "";
                    if (dt1.Rows.Count > 0)
                    {
                        lbl_SocietyDirectory = dt1.Rows[0]["SocietyDirectory"].ToString();
                        lbl_AO_ADD_DistrictEn = dt1.Rows[0]["DISTRICT_ENG"].ToString();
                        lbl_InstitutionName = dt1.Rows[0]["SocietyName"].ToString();
                        lbl_AO_ADD_DistrictHi = dt1.Rows[0]["DISTRICT_MANGAL"].ToString();
                        lbl_Registraionno = dt1.Rows[0]["RegistrationNo"].ToString();
                        lbl_ActId = dt1.Rows[0]["ActId"].ToString();
                        fileNameMemList = Guid.NewGuid().ToString("N").ToUpper() + ".pdf";
                        memberResult = generatePDFformemberAmendment(dt1, System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraionno.Replace("-", "").Replace("/", "").Replace("_", "").Replace(" ", "") + "/") + fileNameMemList, lbl_InstitutionName, lbl_AO_ADD_DistrictEn, lbl_AO_ADD_DistrictHi, lbl_Registraionno, lbl_ActId);
                        if (memberResult == "Fail")
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
                                Type = "0"
                            });
                        }
                        if (!string.IsNullOrWhiteSpace(fileNameMemList) && memberResult == "Success")
                        {
                            lbl_SocietyDirectory = lbl_SocietyDirectory.Replace("-", "");
                            if (!string.IsNullOrWhiteSpace(lbl_SocietyDirectory))
                            {
                                lbl_SocietyDirectory = System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_SocietyDirectory + "/" + fileNameMemList);
                            }
                            else
                            {
                                lbl_SocietyDirectory = System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraionno.Replace("-", "").Replace("/", "").Replace("_", "").Replace(" ", "") + "/" + fileNameMemList);
                            }
                            return JsonConvert.SerializeObject(new
                            {
                                Success = true,
                                DocPath = lbl_SocietyDirectory,
                                DocType = "Amendment Member Certificate",
                                DocName = fileNameMemList,
                                ReturnPath = Url.Action("ApplicationFlowAmendment", "ApplicationFlow"),
                                Location = lbl_AO_ADD_DistrictEn,
                                Designation = "REGISTRAR",
                                Type = "2"
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
                                Type = "0"
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
                            ReturnPath = "",
                            Location = "",
                            Designation = "",
                            Type = "0"
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
                        ReturnPath = "",
                        Location = "",
                        Designation = "",
                        Type = "0"
                    });
                }

            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "GenerateAmendmentMemberLatter(string SocietyIDEnc, int AmendmentId)" }, ex);
                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    Location = "",
                    Designation = "",
                    Type = "0"
                });
            }
        }

        public string GenerateAmendmentBylawsLatter(string SocietyIDEnc, int AmendmentId, bool AmedmentRequestType)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {

                    ApplicationFlowModel AppFlowModel = new ApplicationFlowModel();
                    DataTable dtAmendmentChanges = AppFlowModel.GetAmendmentChanges(SocietyIDEnc, AmendmentId);
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
                    }

                    lbl_SocietyDirectory = lbl_SocietyDirectory.Replace("-", "");
                    lbl_SocietyDirectory = System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_SocietyDirectory + "/" + BylawsStr);

                    if (!string.IsNullOrWhiteSpace(BylawsStr))
                    {
                        return JsonConvert.SerializeObject(new
                        {
                            Success = true,
                            DocPath = lbl_SocietyDirectory,
                            DocType = "Amendment Bylaws",
                            DocName = BylawsStr,
                            ReturnPath = Url.Action("ApplicationFlowAmendment", "ApplicationFlow"),
                            Location = lbl_AO_ADD_DistrictEn,
                            Designation = "REGISTRAR",
                            Type = "3"
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
                            Type = "0"
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
                        ReturnPath = "",
                        Location = "",
                        Designation = "",
                        Type = "0"
                    });
                }

            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "GenerateAmendmentBylawsLatter(string SocietyIDEnc, int AmendmentId)" }, ex);

                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    Location = "",
                    Designation = "",
                    Type = "0"
                });
            }
        }

        [HttpGet]
        [ActionName("GetSimilarSocietyDetails")]
        public string GetSocietySimilarDetails(string SocietyIDEnc)
        {
            ApplicationFlowModel objApplicationFlowModel =
                new ApplicationFlowModel(new DBLogger());

            DataTable dt = objApplicationFlowModel.GetSimilarSocietyDetails(SocietyIDEnc);

            if (dt == null || dt.Rows.Count == 0)
                return COMMON_FUNCTION.DataTableToJSONWithJSONNet(dt);

            // Assuming the search society name is in the first row.
            // Change the column name if needed.
            string searchName = dt.Rows[0]["SocietyName"].ToString();

            // Add Similarity column if not exists
            if (!dt.Columns.Contains("Similarity"))
                dt.Columns.Add("Similarity", typeof(int));

            foreach (DataRow row in dt.Rows)
            {
                string societyName = row["SocietyName"].ToString();

                row["Similarity"] = Fuzz.TokenSetRatio(searchName, societyName);
            }

            DataTable sortedTable = dt.AsEnumerable()
                                      .OrderByDescending(r => r.Field<int>("Similarity"))
                                      .CopyToDataTable();

            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(sortedTable);
        }


        [HttpGet]
        [ActionName("GetDeptPendingDaysDetails")]
        public string GetDeptPendingDaysDetails(string SocietyIDEnc)
        {
            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objApplicationFlowModel.GetDeptPendingDaysDetails(SocietyIDEnc));
        }


        [HttpGet]
        [ActionName("GetDeptAmendmentPendingDaysDetails")]
        public string GetDeptAmendmentPendingDaysDetails(string SocietyIDEnc)
        {
            ApplicationFlowModel objApplicationFlowModel = new ApplicationFlowModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objApplicationFlowModel.GetDeptAmendementPendingDaysDetails(SocietyIDEnc));
        }

        [HttpPost]
        [ActionName("SaveSocietyApplicationFlowAmendment1958StatusNew")]
        public JsonResult SaveSocietyApplicationFlowAmendment1958StatusNew(string SocietyIDEnc, int StatusId, string Remark, int UserId, int AmendmentId, string AmendmendAmount = "", string ReceiptNumber = "")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            if (_LoggedInUser == null)
            {
                return Json("LOGOUT");
            }

            ApplicationFlowModel _Modal = new ApplicationFlowModel();
            if (UserId == 0)
            {
                //  UserId = Convert.ToInt32(_LoggedInUser.UserId);
            }
            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
            if (!ModelState.IsValid)
                return Json(JsonModelError.Get(ModelState.Values));


            int result = 0;

            result = _Modal.AddApplicationFlowAmendment1958(SocietyIDEnc, StatusId, Remark, UserId, AmendmentId, AmendmendAmount, ReceiptNumber);

            return Json(result);

        }



    }
}