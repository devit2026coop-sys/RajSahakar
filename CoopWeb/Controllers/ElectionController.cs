using Cooperative.Filters;
using Cooperative.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Cooperative.Controllers
{
    [Error]
    [LogActionFilter]
    [CustomAuthentication]
    public class ElectionController : BaseController
    {
        UserEntity _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

        #region Election

        [HttpGet]
        public ActionResult ElectionSocietyList()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Election, (Request.Cookies[SessionIdentity.Culture]).Value);

            return View();
        }

        [HttpPost]
        public string GetSocietyList(SocietyRegistrationEntity objSocietyRegistrationEntity)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ElectionModel objElectionModel = new ElectionModel();

            objSocietyRegistrationEntity.SSOID = _LoggedInUser.SSOID;
            objSocietyRegistrationEntity.UserType = _LoggedInUser.UserType;
            objSocietyRegistrationEntity.DistrictId = _LoggedInUser.DistrictId;
            objSocietyRegistrationEntity.DivisionId = _LoggedInUser.DivisionId;

            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objElectionModel.SocietyList(objSocietyRegistrationEntity));

        }

        [HttpPost]
        public JsonResult GetSocietyListElection(DataPagingModel Details, ElectionSocietyData objElectionSocietyData)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ElectionModel objElectionModel = new ElectionModel();

            objElectionSocietyData.SSOID = _LoggedInUser.SSOID;
            objElectionSocietyData.UserType = _LoggedInUser.UserType;
            DataTable DT = objElectionModel.SocietyListForElection(ref Details, objElectionSocietyData);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public string PrintElectionOrderPDF(string PDFData,string OrderId)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("<html><head><title></title><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head><body><div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;' ><center></center></div><br /><div id='divtoPrint'>");
        //    sb.Append("<div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px; '>");
        //    sb.append(PDFData);
        //    sb.Append("</div><br /></div></body></html>");
        //    // string  printContents = '<html><head><title></title><meta http-equiv="Content-Type" content="text/html; charset=utf-8" /></head><body><div style="padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;" ><center></center></div><br />' + sb + '</body></html>';

        //    string InpuData = sb.ToString();

        //    string PageType = "Portrait";
        //    var pdfDoc = new NReco.PdfGenerator.HtmlToPdfConverter(); //created an object of HtmlToPdfConverter class.
        //    if (string.IsNullOrEmpty(PageType))
        //        pdfDoc.Orientation = PageOrientation.Default;  //setting orientation.
        //    else
        //    {
        //        if (PageType == "Landscape")
        //            pdfDoc.Orientation = PageOrientation.Landscape;
        //        else
        //            pdfDoc.Orientation = PageOrientation.Portrait;
        //    }

        //    pdfDoc.Size = NReco.PdfGenerator.PageSize.A4;   //8.27 in × 11.02 in //Page Size
        //    PageMargins pageMargins = new PageMargins();     //Margins in mm
        //    pageMargins.Bottom = 05;
        //    pageMargins.Left = 05;
        //    pageMargins.Right = 05;
        //    pageMargins.Top = 05;
        //    pdfDoc.Margins = pageMargins;                      //margins added to PDF.

        //    pdfDoc.PageHeaderHtml = " ";

        //    var htmlContent = InpuData;
        //    //Why I am adding this to page footer?? So we can get paging in footer section of each PDF page.how its working?? A Javascript code is written inside the DLL which is handling  Div's class i.e. page and topage.
        //    pdfDoc.PageFooterHtml = "<div style='float:right;'>  Page <span class='page'></span> of  <span class='topage'></span></div>";
        //    var pdfBytes = p1dfDoc.GeneratePdf(htmlContent);

        //    //System.IO.File.WriteAllBytes(System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraion.Replace("/", "") + "/") + result.ToString() + ".pdf"), pdfBytes);
        //    //System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraion.Replace("/", "") + "/BankOpeningCertificate.pdf");
        //    System.IO.File.WriteAllBytes(Server.MapPath("~/Content/SocietyProfiling/Order"+ OrderId +".pdf"), pdfBytes);
        //    //FileStream fs = new FileStream(Server.MapPath("~/Content/Training"), FileMode.OpenOrCreate);
        //    //fs.Write(pdfBytes, 0, pdfBytes.Length);
        //    //fs.Close();
        //    return "~/Content/Election/Order" + OrderId + ".pdf";
        //}


        [HttpPost]
        public string GetElectionOrderList(string Todate = "", string FormDate = "", int StatusId = 0, string Datewise = "")
        {

            int IsReturingOfficer = 0;
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ElectionModel objElectionModel = new ElectionModel();
            DataTable dt = objElectionModel.GetElectionOrderList(Todate, FormDate, StatusId, Datewise, IsReturingOfficer);//Todate, FormDate
            foreach (DataRow dr in dt.Rows) // search whole table
            {
                dr["IdEnc"] = COMMON_FUNCTION.Encrypt(dr["Id"].ToString());
            }
            //return COMMON_FUNCTION.DataTableToJSONString(dt);
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(dt);
        }

        [HttpPost]
        public string UploadElectionFile()
        {
            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {

                    string fileDirectory = Server.MapPath("~/Content/ElectionDocument");

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
        public string UpdatedElectionStatus(ElectionSocietyData objElectionSocietyDataList)
        {
            ElectionModel objElectionModel = new ElectionModel();

            string Result = objElectionModel.UpdatedElectionStatus(objElectionSocietyDataList).ToString();
            if (Result != "-1")
            {
                return Result;
            }
            else
            {
                return Result;
            }

        }

        [HttpPost]
        public string GetElectionOrderListForRetirningOfficer()
        {
            string Todate = ""; string FormDate = ""; int StatusId = 0; string Datewise = "";
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ElectionModel objElectionModel = new ElectionModel();
            DataTable dt = objElectionModel.GetElectionOrderList(Todate, FormDate, StatusId, Datewise, 1);
            foreach (DataRow dr in dt.Rows) // search whole table
            {
                dr["IdEnc"] = COMMON_FUNCTION.Encrypt(dr["Id"].ToString());
            }

            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(dt);
        }



        [HttpPost]
        public string ElectionOfficerForSociety(string OredrId)
        {
            string DecOrderId;
            //if (OredrId.ToString().Contains("+"))
            //{
            //    DecOrderId = (COMMON_FUNCTION.Decrypt(OredrId.Replace('+', ' ')));
            //}
            //else 
            if (OredrId.Length < 8)
            {
                DecOrderId = OredrId;
            }
            else if (OredrId.ToString().Contains(" "))
            {
                DecOrderId = (COMMON_FUNCTION.Decrypt(OredrId.ToString().Replace(' ', '+')));
            }
            else
            {
                DecOrderId = (COMMON_FUNCTION.Decrypt(OredrId.ToString()));
            }
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ElectionModel objElectionModel = new ElectionModel();
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objElectionModel.ElectionOfficerForSociety(DecOrderId));
        }

        [HttpPost]
        public string UploadNominationForm()
        {
            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {

                    string fileDirectory = Server.MapPath("~/Content/SocietyProfiling/1377");

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
        public string GetSocietyListByOrderID(string OredrId)
        {
            //string DecOrderId = COMMON_FUNCTION.Decrypt(OredrId);
            string DecOrderId;
            //if (OredrId.ToString().Contains("+"))
            //{
            //    DecOrderId = (COMMON_FUNCTION.Decrypt(OredrId.Replace('+', ' ')));
            //}
            //else 
            if (OredrId.ToString().Contains(" "))
            {
                DecOrderId = (COMMON_FUNCTION.Decrypt(OredrId.ToString().Replace(' ', '+')));
            }
            else
            {
                DecOrderId = (COMMON_FUNCTION.Decrypt(OredrId.ToString()));
            }
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ElectionModel objElectionModel = new ElectionModel();
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objElectionModel.GetSocietyListByOrderID(DecOrderId));
        }

        [HttpPost]
        public string GetElectionQuestion(string OredrId)
        {

            //string DecOrderId = COMMON_FUNCTION.Decrypt(OredrId);
            string DecOrderId;
            //if (OredrId.ToString().Contains("+"))
            //{
            //    DecOrderId = (COMMON_FUNCTION.Decrypt(OredrId.Replace('+', ' ')));
            //}
            //else 
            if (OredrId.Length < 8)
            {
                DecOrderId = OredrId;
            }
            else if (OredrId.ToString().Contains(" "))
            {
                DecOrderId = (COMMON_FUNCTION.Decrypt(OredrId.ToString().Replace(' ', '+')));
            }
            else
            {
                DecOrderId = (COMMON_FUNCTION.Decrypt(OredrId.ToString()));
            }
            ElectionModel objElectionModel = new ElectionModel();
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objElectionModel.ElectionPublishList(DecOrderId));

        }

        [HttpPost]
        public string GetElectionDates(string OredrId)
        {
            string DecOrderId;
            if (OredrId.Length < 8)
            {
                DecOrderId = OredrId;
            }
            else if (OredrId.ToString().Contains(" "))
            {
                DecOrderId = (COMMON_FUNCTION.Decrypt(OredrId.ToString().Replace(' ', '+')));
            }
            else
            {
                DecOrderId = (COMMON_FUNCTION.Decrypt(OredrId.ToString()));
            }
            ElectionModel objElectionModel = new ElectionModel();
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objElectionModel.GetElectionDates(DecOrderId));

        }




        [HttpGet]
        public ActionResult ElectionImportantDates(string ElectionMasterId)
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.ElectionImportantDates, (Request.Cookies[SessionIdentity.Culture]).Value);
            ViewBag.ElectionMasterId = ElectionMasterId;

            return View();
        }

        [HttpGet]
        public ActionResult Publish()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Publish, (Request.Cookies[SessionIdentity.Culture]).Value);
            ViewBag.SocietyIDEnc = "";
            string strSocietyIDEnc = Request.QueryString["SocietyIDEnc"];
            if (strSocietyIDEnc != null)
            {
                ViewBag.SocietyIDEnc = strSocietyIDEnc;
            }
            return View();
        }
        [HttpPost]
        [ActionName("SaveElectionMasterdata")]
        public JsonResult SaveElectionMaster(ElectionSocietyData objElectionSocietyData)
        {
            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
            if (!ModelState.IsValid)
                return Json(JsonModelError.Get(ModelState.Values));
            if (objElectionSocietyData.ElectionFor == 3)
            {
                objElectionSocietyData.ReturningOfficerID = Convert.ToInt32(_loggedInUSerDetails.UserId);
            }
            ElectionModel objElectionModel = new ElectionModel();
            string Result = objElectionModel.InsertElectionSociety(objElectionSocietyData).ToString();
            if (Result != "-1")
            {
                return Json(COMMON_FUNCTION.Encrypt(Result));
            }
            else
            {
                return Json(Result);
            }
        }
        [HttpGet]
        [ActionName("ElecCurrentStatus")]
        public string ElecCurrentStatus(string id)
        {
            ElectionModel objElectionModel = new ElectionModel();
            string strResult = objElectionModel.ElecCurrentStatus(id).ToString();
            return strResult + "~" + COMMON_FUNCTION.Encrypt(id);
        }



        [HttpPost]
        [ActionName("GetSocietylistByElecionId")]
        public string GetSocietylistByElecionId(string OredrId)
        {

            ElectionModel objElectionModel = new ElectionModel();
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objElectionModel.GetSocietylist(OredrId));

        }




        [HttpPost]
        [ActionName("SaveElectionDatesdata")]
        public string SaveElectionDatesdata(List<Socitydates> objElectionSocietyData, string ElectionMasterIdEnc, string HeaderContantDetails)
        {
            string Result = "0";
            try
            {
                //,int CustomHeaderValue

                //if (CustomHeaderValue==0)
                //{
                //    HeaderContantDetails = "";
                //}

                int ElectionMasterId = -1;
                if (ElectionMasterIdEnc.ToString().Contains("+"))
                {
                    ElectionMasterId = int.Parse(COMMON_FUNCTION.Decrypt(ElectionMasterIdEnc.ToString().Replace('+', ' ')));
                }
                else
                if (ElectionMasterIdEnc.ToString().Contains(" "))
                {
                    ElectionMasterId = int.Parse(COMMON_FUNCTION.Decrypt(ElectionMasterIdEnc.ToString().Replace(' ', '+')));
                }
                else
                {
                    ElectionMasterId = int.Parse(COMMON_FUNCTION.Decrypt(ElectionMasterIdEnc.ToString()));
                }


                ElectionModel objElectionModel = new ElectionModel();
                int insertResult = objElectionModel.InsertElectionSocietyDate(objElectionSocietyData, ElectionMasterId, HeaderContantDetails);

                //var SocietyListOfOrder = objElectionModel.GetSocietyListByOrderID(ElectionMasterId.ToString());
                //var ElectionDates = objElectionModel.ElectionPublishList(ElectionMasterId.ToString());
                //return GenPDFHinForElectionProgram(SocietyListOfOrder, ElectionDates, ElectionMasterId.ToString(), otpResult);
                if (insertResult > 0)
                {
                    //objElectionModel.updateElectionOrderPDF("Order" + ElectionMasterId.ToString() + ".pdf", ElectionMasterId);
                    Result =  GenerateAadhaarOTPForEsing();
                }
                //else
                //{
                //    Result = "PDF Not Generate";
                //}

                // return Json(_ElectionFor.ToString());

            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Election", FunctionName = "SaveElectionDatesdata(List<Socitydates> objElectionSocietyData, string ElectionMasterIdEnc)" }, ex);
                Result = ex.StackTrace;
                return "0";
            }
            return Result.ToString();

            //return Json(Result);
        }


        [HttpPost]
        [ActionName("GET_DEFAULT_TEXT")]
        public string GET_DEFAULT_TEXT(List<Socitydates> objElectionSocietyData, string ElectionMasterIdEnc,  string HeaderContantDetails, string Snumber)
        {
            string Result = "";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                int ElectionMasterId = -1;
                if (ElectionMasterIdEnc.ToString().Contains("+"))
                {
                    ElectionMasterId = int.Parse(COMMON_FUNCTION.Decrypt(ElectionMasterIdEnc.ToString().Replace('+', ' ')));
                }
                else
                
                if (ElectionMasterIdEnc.ToString().Contains(" "))
                {
                    ElectionMasterId = int.Parse(COMMON_FUNCTION.Decrypt(ElectionMasterIdEnc.ToString().Replace(' ', '+')));
                }

                else
                {
                    ElectionMasterId = int.Parse(COMMON_FUNCTION.Decrypt(ElectionMasterIdEnc.ToString()));
                }

                ElectionModel objElectionModel = new ElectionModel();
                ElectionSocietyData _Model = new ElectionSocietyData();

                int _ElectionFor = 0;

                var SocietyListOfOrder = objElectionModel.GetSocietyListByOrderID(ElectionMasterId.ToString());
                var ElectionDates = objElectionModel.ElectionPublishList(ElectionMasterId.ToString());

                string _districtName = string.Empty;
                string _Soc_districtName = string.Empty;
                string _returningOfficerName = string.Empty;
                string _divisionName = string.Empty;
                string _Soc_divisionName = string.Empty;
                string _OfficerNameEn = string.Empty;
                string _HeaderContentDetails = string.Empty;
                string _serialnumber = Snumber;
                int _stepsInElection = _Model.StepInElection;
                int _ElectionStatus = 0;

                string[] _ElectionStepKey = new string[] { "प्रथम चरण", "द्वितीय चरण", "तृतीय चरण", "चतुर्थ चरण", "पंचम चरण", "षष्ठम चरण", "सप्तम चरण", "अष्टम चरण", "नवम चरण", "दशम चरण" };
                string[] _ElectionStepValue = new string[] { "Stap1Date", "Stap2Date", "Stap3Date", "Stap4Date", "Stap5Date", "Stap6Date", "Stap7Date", "Stap8Date", "Stap9Date", "Stap10Date" };

                if (SocietyListOfOrder != null && SocietyListOfOrder.Rows.Count > 0)
                {
                    _districtName = Convert.ToString(SocietyListOfOrder.Rows[0]["DISTRICT_MANGAL"]);
                    _Soc_districtName = Convert.ToString(SocietyListOfOrder.Rows[0]["Society_DISTRICT_MANGAL"]);
                    _returningOfficerName = Convert.ToString(SocietyListOfOrder.Rows[0]["OfficerNameHi"]);
                    _divisionName = Convert.ToString(SocietyListOfOrder.Rows[0]["DIVISION_MANGAL"]);
                    _Soc_divisionName = Convert.ToString(SocietyListOfOrder.Rows[0]["Society_DIVISION_MANGAL"]);
                    _stepsInElection = Convert.ToInt32(ElectionDates.Rows[0]["StepInElection"]);
                    _ElectionFor = Convert.ToInt32(SocietyListOfOrder.Rows[0]["ElectionFor"]);
                    _OfficerNameEn = Convert.ToString(SocietyListOfOrder.Rows[0]["ReturningOfficerNameEn"]);
                    _HeaderContentDetails = Convert.ToString(SocietyListOfOrder.Rows[0]["HeaderContentDetails"]);
                    _ElectionStatus = Convert.ToInt32(ElectionDates.Rows[0]["ElectionStatus"]);
                }
               
                string fontSize = "font-size: 13px;";
                sb.Append("<html><body><div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;' ><center></center></div><br /><div id='divtoPrint'>");
                sb.Append("<div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;" + fontSize + " '>");
                sb.Append("<table id='content'style='width:100%;" + fontSize + "'>");

                sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                //sb.Append("<img style='width:65px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                //sb.Append("</td></tr>");
                //sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                //sb.Append("<tr>");
                //sb.Append("<td style='text-align:center;'> कार्यालय राजस्थान राज्य सहकारी निर्वाचन प्राधिकरण </td>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td style='text-align:center;'> 10 - बी संस्थागत क्षेत्र ,रायसेम भवन ,झालाना डूंगरी ,जयपुर </td>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td style='text-align:center;'> फोन - 0141-2712544 फैक्स - 0141-2709018 </td>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td style='text-align:center;'><b> Email ID scea@rajasthan.gov.in </b> </td>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td style='text-align:center;'>");
                sb.Append("<p style='text-align:left;'> क्रमांक  " + _serialnumber + " </p> ");
              //  sb.Append("<p style=''> आदेश </p>");

                if (HeaderContantDetails != "")
                {
                    sb.Append(_HeaderContentDetails);  //Akshay
                }
                else
                {
                    sb.Append("<p style='text-align:justify;' > राजस्थान सहकारी सोसाइटी अधिनियम 2001 की धारा 33,35 एवं राजस्थान सहकारी सोसायटी ");
                    if (_ElectionFor != 3)
                    {
                        if (_ElectionFor == 1)
                        {
                            sb.Append(" नियम 2003 के नियम 45 व  46 के अंतर्गत प्रदत्त शक्तियों के अनुसरण में " + _divisionName + " संभाग की परिशिष्ठ -1 में ");
                        }
                        else if (_ElectionFor == 2)
                        {
                            sb.Append(" नियम 2003 के नियम 45 व  46 के अंतर्गत प्रदत्त शक्तियों के अनुसरण में " + _districtName + "संभाग की परिशिष्ठ -1 में ");
                        }

                        sb.Append(" अंकित सहकारी संस्थाओं के संचालक मंडल के सदस्यों एवं पदाधिकारियों के निर्वाचन करवाए जाने हेतु ");

                        if (_ElectionFor == 1)
                        {


                            sb.Append(" संभाग रिटर्निंग अधिकारी " + (!string.IsNullOrWhiteSpace(_OfficerNameEn) ? "(" + _OfficerNameEn + ")" : "") + " एवं अतिरिक्त / संयुक्त रजिस्ट्रार सहकारी समितियाँ ," + _divisionName + " आदेश ");
                            sb.Append(" अधिकृत किया जाता है। निर्वाचन कार्यक्रम परिशिष्ठ -2 पर संलग्न है । </p>");
                        }
                        else if (_ElectionFor == 2)
                        {
                            sb.Append(" इकाई रिटर्निंग अधिकारी " + (!string.IsNullOrWhiteSpace(_OfficerNameEn) ? "(" + _OfficerNameEn + ")" : "") + " एवं उप रजिस्ट्रार सहकारी समितियाँ , " + _districtName + " आदेश ");
                            sb.Append(" अधिकृत किया जाता है। निर्वाचन कार्यक्रम परिशिष्ठ -2 पर संलग्न है । </p>");
                        }

                    }
                    else
                    {
                        sb.Append(" नियम 2003 के नियम 45 व  46 के अंतर्गत प्रदत्त शक्तियों के अनुसरण परिशिष्ठ -1 में ");
                        sb.Append(" अंकित सहकारी संस्थाओं के संचालक मंडल के सदस्यों एवं पदाधिकारियों के निर्वाचन करवाए जाने हेतु ");
                        sb.Append(" निर्वाचन अधिकारी नियुक्त किया जाता है। निर्वाचन कार्यक्रम परिशिष्ठ -2 पर संलग्न है । </ p>");
                    }

                    sb.Append("<p style='text-align:justify;'>  निर्वाचन अधिकारी द्वारा राजस्थान सहकारी सोसायटी अधिनियम 2001 (यथा संशोथित), राजस्थान सोसायटी नियम 2003, (यथा संशोथित), संबंधित संस्थाओ के अद्यतन संशोधित पंजीकृत उपनियमों एवं राजस्थान राज्य सहकारी निर्वाचन प्राधिकरण द्वारा सहकारी संस्थाओ के निर्वाचन हेतु जारी सामान्य निर्देश क्रमांक फा. 3(1) रा . रा . सह . निर्वा . प्राधि / 16-35-43 दिनांक  20.02.2015 (http://rajsahakar.rajasthan.gov.in/)   वेबसाइट पर महत्वपूर्ण आदेशों में  Election Folder  पर उपलब्ध है) की पालना करते हुए निर्वाचन सम्पन्न कराये जाएंगे </p>");
                    sb.Append("<p style='text-align:justify;'>  यह आदेश आज दिनांक " + DateTime.Now.ToString("dd/MM/yyyy") + " को मेरे हस्ताक्षर एवं कार्यालय मुद्रा से अंकित कर जारी किया गया ।  </p>");

                }

                sb.Append("<p style='text-align:left;'> <b> संलग्न उपरोक्तानुसार </b> </p>");
                sb.Append("<p style='text-align:left;' > प्रतिलिपि निम्नलिखित को सूचनार्थ एवं आवश्यक कार्यवाही हेतु प्रेषित है :-  </p> ");
                sb.Append("<p style='text-align:left;' > 1. श्रीमान प्रमुख शासन सचिव, सहकारिता विभाग राजस्थान जयपुर । </p> ");
                sb.Append("<p style='text-align:left;' > 2. श्रीमान रजिस्ट्रार, सहकारी समितियां, राजस्थान जयपुर । </p> ");
                if (_ElectionFor == 1)
                {
                    sb.Append("<p style='text-align:left;' > 3. जोनल रिटर्निग अधिकारी एवं अतिरिक्त रजिस्ट्रार, सहकारी समितियां , " + _divisionName + " खंड " + _divisionName + " </p> ");
                    sb.Append("<p style='text-align:left;' > 4. गार्ड पत्रावली </p> ");
                }
                else if (_ElectionFor == 2)
                {
                    sb.Append("<p style='text-align:left;' > 3. जोनल रिटर्निग अधिकारी एवं अतिरिक्त रजिस्ट्रार, सहकारी समितियां," + _Soc_divisionName + " खंड " + _Soc_divisionName + " </p> ");
                    sb.Append("<p style='text-align:left;' > 4. इकाई रिटर्निंग अधिकारी एवं उप रजिस्ट्रार, सहकारी समितियां, " + _districtName + "।  </p> ");
                    sb.Append("<p style='text-align:left;' > 5. गार्ड पत्रावली </p> ");
                }
                else if (_ElectionFor == 3)
                {
                    sb.Append("<p style='text-align:left;' > 3. जोनल रिटर्निग अधिकारी एवं अतिरिक्त रजिस्ट्रार, सहकारी समितियां, " + _Soc_divisionName + " खंड " + _Soc_divisionName + " </p> ");
                    sb.Append("<p style='text-align:left;' > 4. इकाई रिटर्निंग अधिकारी एवं उप रजिस्ट्रार, सहकारी समितियां, " + _Soc_districtName + " । </p> ");
                    sb.Append("<p style='text-align:left;' > 5. गार्ड पत्रावली </p> ");
                }
              
                sb.Append("<br/></td></tr>");
                


           

                sb.Append("</table></div><br/></div></body></html>");

                string InpuData = sb.ToString();

                return InpuData;

            }

            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Election", FunctionName = "GET_DEFAULT_TEXT(List<Socitydates> objElectionSocietyData, string ElectionMasterIdEnc,  string HeaderContantDetails, string Snumber)" }, ex);
                Result = ex.StackTrace;
                return "0";
            }
            
        }




        [HttpPost]
        [ActionName("GenerateElectionInitiatPDF")]
        public string GenerateElectionInitiatPDF(List<Socitydates> objElectionSocietyData, string ElectionMasterIdEnc, string OTP, string TXN, string HeaderContantDetails, string Snumber)
        {
            string Result = "";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                var otpResult =  COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                if (otpResult.status != "1")
                {
                    return otpResult.data;
                }

                int ElectionMasterId = -1;
                if (ElectionMasterIdEnc.ToString().Contains("+"))
                {
                    ElectionMasterId = int.Parse(COMMON_FUNCTION.Decrypt(ElectionMasterIdEnc.ToString().Replace('+', ' ')));
                }
                else
                if (ElectionMasterIdEnc.ToString().Contains(" "))
                {
                    ElectionMasterId = int.Parse(COMMON_FUNCTION.Decrypt(ElectionMasterIdEnc.ToString().Replace(' ', '+')));
                }
                else
                {
                    ElectionMasterId = int.Parse(COMMON_FUNCTION.Decrypt(ElectionMasterIdEnc.ToString()));
                }
                ElectionModel objElectionModel = new ElectionModel();

                int _ElectionFor = 0;

                int insertResult = objElectionModel.InsertElectionSocietyDate(objElectionSocietyData, ElectionMasterId, HeaderContantDetails, true);

                var SocietyListOfOrder = objElectionModel.GetSocietyListByOrderID(ElectionMasterId.ToString());
                var ElectionDates = objElectionModel.ElectionPublishList(ElectionMasterId.ToString());

                string _districtName = string.Empty;
                string _Soc_districtName = string.Empty;
                string _returningOfficerName = string.Empty;
                string _divisionName = string.Empty;
                string _Soc_divisionName = string.Empty;
                string _OfficerNameEn = string.Empty;
                string _HeaderContentDetails = string.Empty;
                string _serialnumber = Snumber;
                int _stepsInElection = 8;
                int _ElectionStatus = 0;

                string[] _ElectionStepKey = new string[] { "प्रथम चरण", "द्वितीय चरण", "तृतीय चरण", "चतुर्थ चरण", "पंचम चरण", "षष्ठम चरण", "सप्तम चरण", "अष्टम चरण", "नवम चरण", "दशम चरण" };
                string[] _ElectionStepValue = new string[] { "Stap1Date", "Stap2Date", "Stap3Date", "Stap4Date", "Stap5Date", "Stap6Date", "Stap7Date", "Stap8Date", "Stap9Date", "Stap10Date" };

                if (SocietyListOfOrder != null && SocietyListOfOrder.Rows.Count > 0)
                {
                    _districtName = Convert.ToString(SocietyListOfOrder.Rows[0]["DISTRICT_MANGAL"]);
                    _Soc_districtName = Convert.ToString(SocietyListOfOrder.Rows[0]["Society_DISTRICT_MANGAL"]);
                    _returningOfficerName = Convert.ToString(SocietyListOfOrder.Rows[0]["OfficerNameHi"]);
                    _divisionName = Convert.ToString(SocietyListOfOrder.Rows[0]["DIVISION_MANGAL"]);
                    _Soc_divisionName = Convert.ToString(SocietyListOfOrder.Rows[0]["Society_DIVISION_MANGAL"]);
                    _stepsInElection = Convert.ToInt32(ElectionDates.Rows[0]["StepInElection"]);
                    _ElectionFor = Convert.ToInt32(SocietyListOfOrder.Rows[0]["ElectionFor"]);
                    _OfficerNameEn = Convert.ToString(SocietyListOfOrder.Rows[0]["ReturningOfficerNameEn"]);
                    _HeaderContentDetails = Convert.ToString(SocietyListOfOrder.Rows[0]["HeaderContentDetails"]);
                    _ElectionStatus = Convert.ToInt32(ElectionDates.Rows[0]["ElectionStatus"]);
                }
                // StringBuilder sb = new StringBuilder();
                string fontSize = "font-size: 13px;";
                //sb.Append("<html><head><title></title><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head><body><div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;' ><center></center></div><br /><div id='divtoPrint'>");
                sb.Append("<html><body><div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;' ><center></center></div><br /><div id='divtoPrint'>");
                sb.Append("<div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;" + fontSize + " '>");
                sb.Append("<table id='content'style='width:100%;" + fontSize + "'>");

                sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                sb.Append("<img style='width:65px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");

                //sb.Append("<tr>");
                //sb.Append("<td style='text-align:center'>");
                //sb.Append("<b><u style=''> राजस्थान सरकार </u></b>");
                //sb.Append("</td>");
                //sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='text-align:center;'> कार्यालय राजस्थान राज्य सहकारी निर्वाचन प्राधिकरण </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='text-align:center;'> 10 - बी संस्थागत क्षेत्र ,रायसेम भवन ,झालाना डूंगरी ,जयपुर </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='text-align:center;'> फोन - 0141-2712544 फैक्स - 0141-2709018 </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='text-align:center;'><b> Email ID scea@rajasthan.gov.in </b> </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='text-align:center;'>");
                sb.Append("<p style='text-align:left;'> क्रमांक  " + _serialnumber + " </p> ");
                sb.Append("<p style='text-align:right;'> दिनांक  " + DateTime.Now.ToString("dd/MM/yyyy") + " </p> ");
                sb.Append("<p style=''> आदेश </p>");
                if (HeaderContantDetails != "")
                {
                    sb.Append(_HeaderContentDetails);  //Akshay
                }
                else
                {
                    sb.Append("<p style='text-align:justify;' > राजस्थान सहकारी सोसाइटी अधिनियम 2001 की धारा 33,35 एवं राजस्थान सहकारी सोसायटी ");
                    if (_ElectionFor != 3)
                    {
                        if (_ElectionFor == 1)
                        {
                            sb.Append(" नियम 2003 के नियम 45 व  46 के अंतर्गत प्रदत्त शक्तियों के अनुसरण में " + _divisionName + " संभाग की परिशिष्ठ -1 में ");
                        }
                        else if (_ElectionFor == 2)
                        {
                            sb.Append(" नियम 2003 के नियम 45 व  46 के अंतर्गत प्रदत्त शक्तियों के अनुसरण में " + _districtName + "संभाग की परिशिष्ठ -1 में ");
                        }

                        sb.Append(" अंकित सहकारी संस्थाओं के संचालक मंडल के सदस्यों एवं पदाधिकारियों के निर्वाचन करवाए जाने हेतु ");

                        if (_ElectionFor == 1)
                        {


                            sb.Append(" संभाग रिटर्निंग अधिकारी " + (!string.IsNullOrWhiteSpace(_OfficerNameEn) ? "(" + _OfficerNameEn + ")" : "") + " एवं अतिरिक्त / संयुक्त रजिस्ट्रार सहकारी समितियाँ ," + _divisionName + " आदेश ");
                            sb.Append(" अधिकृत किया जाता है। निर्वाचन कार्यक्रम परिशिष्ठ -2 पर संलग्न है । </p>");
                        }
                        else if (_ElectionFor == 2)
                        {
                            sb.Append(" इकाई रिटर्निंग अधिकारी " + (!string.IsNullOrWhiteSpace(_OfficerNameEn) ? "(" + _OfficerNameEn + ")" : "") + " एवं उप रजिस्ट्रार सहकारी समितियाँ , " + _districtName + " आदेश ");
                            sb.Append(" अधिकृत किया जाता है। निर्वाचन कार्यक्रम परिशिष्ठ -2 पर संलग्न है । </p>");
                        }

                    }
                    else
                    {
                        sb.Append(" नियम 2003 के नियम 45 व  46 के अंतर्गत प्रदत्त शक्तियों के अनुसरण परिशिष्ठ -1 में ");
                        sb.Append(" अंकित सहकारी संस्थाओं के संचालक मंडल के सदस्यों एवं पदाधिकारियों के निर्वाचन करवाए जाने हेतु ");
                        sb.Append(" निर्वाचन अधिकारी नियुक्त किया जाता है। निर्वाचन कार्यक्रम परिशिष्ठ -2 पर संलग्न है । </ p>");
                    }

                    sb.Append("<p style='text-align:justify;'>  निर्वाचन अधिकारी द्वारा राजस्थान सहकारी सोसायटी अधिनियम 2001 (यथा संशोथित), राजस्थान सोसायटी नियम 2003, (यथा संशोथित), संबंधित संस्थाओ के अद्यतन संशोधित पंजीकृत उपनियमों एवं राजस्थान राज्य सहकारी निर्वाचन प्राधिकरण द्वारा सहकारी संस्थाओ के निर्वाचन हेतु जारी सामान्य निर्देश क्रमांक फा. 3(1) रा . रा . सह . निर्वा . प्राधि / 16-35-43 दिनांक  20.02.2015 (http://rajsahakar.rajasthan.gov.in/)   वेबसाइट पर महत्वपूर्ण आदेशों में  Election Folder  पर उपलब्ध है) की पालना करते हुए निर्वाचन सम्पन्न कराये जाएंगे </p>");
                    sb.Append("<p style='text-align:justify;'>  यह आदेश आज दिनांक " + DateTime.Now.ToString("dd/MM/yyyy") + " को मेरे हस्ताक्षर एवं कार्यालय मुद्रा से अंकित कर जारी किया गया ।  </p>");

                }

                sb.Append("<p style='text-align:left;'> <b> संलग्न उपरोक्तानुसार </b> </p>");
                //sb.Append("<p style='text-align:right;'><b> राज्य सहकारी निर्वाचन प्राधिकारी </b> </p>");
                sb.Append("<p style='text-align:left;' > प्रतिलिपि निम्नलिखित को सूचनार्थ एवं आवश्यक कार्यवाही हेतु प्रेषित है :-  </p> ");
                sb.Append("<p style='text-align:left;' > 1. श्रीमान प्रमुख शासन सचिव, सहकारिता विभाग राजस्थान जयपुर । </p> ");
                sb.Append("<p style='text-align:left;' > 2. श्रीमान रजिस्ट्रार, सहकारी समितियां, राजस्थान जयपुर । </p> ");
                if (_ElectionFor == 1)
                {
                    sb.Append("<p style='text-align:left;' > 3. जोनल रिटर्निग अधिकारी एवं अतिरिक्त रजिस्ट्रार, सहकारी समितियां , " + _divisionName + " खंड " + _divisionName + " </p> ");
                    sb.Append("<p style='text-align:left;' > 4. गार्ड पत्रावली </p> ");
                }
                else if (_ElectionFor == 2)
                {
                    sb.Append("<p style='text-align:left;' > 3. जोनल रिटर्निग अधिकारी एवं अतिरिक्त रजिस्ट्रार, सहकारी समितियां," + _Soc_divisionName + " खंड " + _Soc_divisionName + " </p> ");
                    sb.Append("<p style='text-align:left;' > 4. इकाई रिटर्निंग अधिकारी एवं उप रजिस्ट्रार, सहकारी समितियां, " + _districtName + "।  </p> ");
                    sb.Append("<p style='text-align:left;' > 5. गार्ड पत्रावली </p> ");
                }
                else if (_ElectionFor == 3)
                {
                    sb.Append("<p style='text-align:left;' > 3. जोनल रिटर्निग अधिकारी एवं अतिरिक्त रजिस्ट्रार, सहकारी समितियां, " + _Soc_divisionName + " खंड " + _Soc_divisionName + " </p> ");
                    sb.Append("<p style='text-align:left;' > 4. इकाई रिटर्निंग अधिकारी एवं उप रजिस्ट्रार, सहकारी समितियां, " + _Soc_districtName + " । </p> ");
                    sb.Append("<p style='text-align:left;' > 5. गार्ड पत्रावली </p> ");
                }
                //sb.Append("<p style='text-align:right;' > राज्य सहकारी निर्वाचन प्राधिकारी </p><br/>");
                sb.Append("<br/></td></tr>");
                int RowNo = 1;


                sb.Append("<tr><td style='text-align:center;'>");
                sb.Append("<p style='text-align: right;color:black;page-break-before:always;'> परिशिष्ट -1 </p>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                sb.Append("<img style='width:65px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                sb.Append("<tr>");
                sb.Append("<td style='text-align:center;'> कार्यालय राजस्थान राज्य सहकारी निर्वाचन प्राधिकरण </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='text-align:center;'> 10 - बी संस्थागत क्षेत्र ,रायसेम भवन ,झालाना डूंगरी ,जयपुर -302004 </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='text-align:center;'> फोन - 0141-2712544 फैक्स - 0141-2709018 </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='text-align:center;'> <b> Email ID scea@rajasthan.gov.in </b> </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='text-align:center; padding-top:15px;'>");
                sb.Append("<table id='content'  border='1'  cellpadding='5' cellspacing='0'  style='width:100%;" + fontSize + "'>");
                sb.Append("<thead> <tr> <th style='text-align:left;border:1px solid #000;width:40px;'> क्रमांक </th> <th style='text-align:left;'> संस्था का नाम </th>  <th style='text-align:left;width:150px;'> जिला </th>  </tr>  </thead>");
                sb.Append("<tbody>");


                foreach (DataRow row in SocietyListOfOrder.Rows)
                {
                    sb.Append(" <tr> <td style='text-align:left;'> " + RowNo.ToString() + " </td><td style='text-align:left;'> " + row["SocietyName"].ToString() + " </td><td style='text-align:left;'> " + row["DistrictNameEn"].ToString() + "</td> </tr>");
                    RowNo++;
                }


                sb.Append("</tbody></table>");
                sb.Append("</td></tr>");

                sb.Append("<tr><td style='text-align:center'>");
                sb.Append("<p style='text-align: right;color:black;page-break-before:always;'> परिशिष्ट -2 </p>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                sb.Append("<img style='width:65px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                sb.Append("<tr>");
                sb.Append("<td style='text-align:center;'> कार्यालय राजस्थान राज्य सहकारी निर्वाचन प्राधिकरण  </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='text-align:center;'> 10 - बी संस्थागत क्षेत्र ,रायसेम भवन ,झालाना डूंगरी ,जयपुर -302004 </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='text-align:center;'> फैक्स -0141-2709018 फोन-0141-2712544 </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                if (_ElectionFor == 1)
                {
                    sb.Append("<td style='text-align:center;'> <b> " + _divisionName + " संभाग की परिशिष्ठ -१  में अंकित सहकारी संस्थाओं के संचालन मंडल के सदस्यों एवं पदाधिकारियों के </b> </td>");
                }
                else if (_ElectionFor == 2)
                {
                    sb.Append("<td style='text-align:center;'> <b> " + _districtName + " इकाई की परिशिष्ठ -१  में अंकित सहकारी संस्थाओं के संचालन मंडल के सदस्यों एवं पदाधिकारियों के </b> </td>");
                }
                else if (_ElectionFor == 3)
                {
                    sb.Append("<td style='text-align:center;'> <b> परिशिष्ठ -१ में अंकित सहकारी संस्थाओं के संचालन मंडल के सदस्यों एवं पदाधिकारियों के  </b> </td>");
                }
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='text-align:center;'> <b> निर्वाचन हेतु निर्वाचन कार्यक्रम </b> </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='padding-top:15px;'>");
                sb.Append("<table id='content'  border='1'  cellpadding='5' cellspacing='0'  style='width:100%;" + fontSize + "'>");
                sb.Append("<thead>  <tr> <th style='text-align:left;width:50px;'> क्रमांक </th><th style='text-align:left;'> विवरण </th>");

                for (int i = 0; i < _stepsInElection; i++)
                {
                    sb.Append("<th style='text-align:left;width:90px;'> " + _ElectionStepKey[i] + " </th>");
                }

                sb.Append("</tr> </thead>");
                sb.Append("<tbody>");
                RowNo = 1;
                foreach (DataRow row in ElectionDates.Rows)
                {



                    sb.Append(" <tr> <td style='text-align:left;'> " + (RowNo < 9 ? RowNo.ToString() : "&nbsp;") + " </td><td style='text-align:left;'> " + row["QuestionHiWithDates"].ToString() + " </td>");



                    for (int i = 0; i < _stepsInElection; i++)
                    {

                        if (_ElectionStatus == 6)
                        {
                            bool Ispospone = (bool)row["Ispostpone"];
                            if (Ispospone != true)
                            {
                                sb.Append("<td style='text-align:left;' > " + (RowNo == 2 ? ElectionDates.Rows[1][_ElectionStepValue[i]] + " से <br>" : "") + "  " + Convert.ToString(row[_ElectionStepValue[i]]) + " " + (RowNo == 2 ? " " + objElectionSocietyData[1].EndTime + " तक " : "") + " </td>");
                            }
                        }
                        else
                        {
                            sb.Append("<td style='text-align:left;' > " + (RowNo == 2 ? ElectionDates.Rows[1][_ElectionStepValue[i]] + " से <br>" : "") + "  " + Convert.ToString(row[_ElectionStepValue[i]]) + " " + (RowNo == 2 ? " " + objElectionSocietyData[1].EndTime + " तक " : "") + " </td>");
                        }


                    }
                    sb.Append("</tr>");

                    RowNo++;
                }


                sb.Append("</tbody></table>");


                if (ElectionDates.Rows.Count > 11)
                {
                    sb.Append("<p style='text-align:justify;'> <b> नोट :- संचालन मंडल के समस्त सदस्यों के निर्विरोध निर्वाचन की स्थिति में पदाधिकारियों का चुनाव ");
                    DataRow drFrom = ElectionDates.Rows[7];
                    DataRow drTo = ElectionDates.Rows[12];
                    for (int j = 0; j < _stepsInElection; j++)
                    {

                        if (_ElectionStatus == 6)
                        {
                            bool Ispospone = (bool)drFrom["Ispostpone"];
                            if (Ispospone != true)
                            {
                                sb.Append(" " + _ElectionStepKey[j] + " में दिनांक " + Convert.ToString(drFrom[_ElectionStepValue[j]]) + " के स्थान पर दिनांक " + Convert.ToString(drTo[_ElectionStepValue[j]]) + " , ");
                            }
                        }
                        else
                        {
                            sb.Append(" " + _ElectionStepKey[j] + " में दिनांक " + Convert.ToString(drFrom[_ElectionStepValue[j]]) + " के स्थान पर दिनांक " + Convert.ToString(drTo[_ElectionStepValue[j]]) + " , ");
                        }


                    }

                    sb.Append(" को क्रम संख्या 8 में अंकित निर्वाचन कार्यक्रम अनुसार संपन्न करवाया जावेगा ।  </b> </p>");
                    //sb.Append("<p style='text-align:right;'> <b> राज्य सहकारी निर्वाचन प्राधिकारी </b></p>");
                }

                sb.Append("</td></tr>");

                sb.Append("</table></div><br/></div></body></html>");

                string InpuData = sb.ToString();

                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "17px"));


                if (!System.IO.Directory.Exists(Server.MapPath("~/Content/ElectionOrder")))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Content/ElectionOrder"));
                }

                if (COMMON_FUNCTION.GeneratePDF(sb1, Server.MapPath("~/Content/ElectionOrder/HinOrder" + ElectionMasterId.ToString() + ".pdf")))
                {

                    var esignResult = COMMON_FUNCTION.EsinghPDF(otpResult.data, Server.MapPath("~/Content/ElectionOrder/HinOrder" + ElectionMasterId.ToString() + ".pdf"), "", "Election Officer");
                    string result = esignResult.data;
                    if (result == "Success")
                    {

                        //objElectionModel.UpdateElectionMasterOrderPDFSign(ElectionMasterId.ToString(), true);


                        if (_ElectionStatus == 6)
                        {
                            objElectionModel.updateElectionOrderPDF("HinOrder" + ElectionMasterId.ToString() + ".pdf", Convert.ToInt32(ElectionMasterId), 7);
                        }
                        else
                        {
                            objElectionModel.updateElectionOrderPDF("HinOrder" + ElectionMasterId.ToString() + ".pdf", Convert.ToInt32(ElectionMasterId));
                        }
                        var DT = objElectionModel.GetElectionEmailSMSDetails(29, 1, Convert.ToInt32(ElectionMasterId), 0);

                        string link = (Request.Url.Host.Contains("localhost") ? "http://localhost:27466/" : "http://rajsahakarapp.rajasthan.gov.in/") + "Content/ElectionOrder/HinOrder" + ElectionMasterId.ToString() + ".pdf";


                        foreach (DataRow dr in DT.Rows) // search whole table
                        {
                            new EsanchaarController().SendSocietySMS(dr["sms"].ToString() + " " + link, dr["MobileNo"].ToString());
                        }

                        return "Success";
                    }
                    else
                    {
                        return "0";
                    }
                }
                else
                {
                    return "0";
                }




                //return GenPDFHinForElectionProgram(SocietyListOfOrder, ElectionDates, ElectionMasterId.ToString(), otpResult);
                //if (_ElectionFor > 0)
                //{
                //    //objElectionModel.updateElectionOrderPDF("Order" + ElectionMasterId.ToString() + ".pdf", ElectionMasterId);
                //    Result = GenerateAadhaarOTPForEsing();
                //}
                //else
                //{
                //    Result = "PDF Not Generate";
                //}

                // return Json(_ElectionFor.ToString());

            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Election", FunctionName = "GenerateElectionInitiatPDF(List<Socitydates> objElectionSocietyData, string ElectionMasterIdEnc, string OTP, string TXN, string HeaderContantDetails, string Snumber)" }, ex);
                Result = ex.StackTrace;
                return "0";
            }
            //return Json(Result);
        }


        [HttpPost]
        [ActionName("InsertSocietyElectionOfficer")]
        public JsonResult InsertSocietyElectionOfficer(ListSocietyElecOffi ObjListSocietyElecOffi)
        {
            ElectionModel objElectionModel = new ElectionModel(new DBLogger());
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            //List<SocietyElectionOfficer> objData = new List<SocietyElectionOfficer>();
            //try
            //{
            //    objData = Request.Form["DataArray"].ToString().Equals("[]") ? new List<SocietyElectionOfficer>() : (List<SocietyElectionOfficer>)s.Deserialize(Request.Form["DataArray"].ToString(), typeof(List<SocietyElectionOfficer>));
            //}
            //catch { }
            foreach (SocietyElectionOfficer item in ObjListSocietyElecOffi.DataArray)
            {
                try
                {
                    // item.ElectionMasterId = Convert.ToInt32( COMMON_FUNCTION.Decrypt(item.ElectionMasterIdEnc));
                    //if (item.ElectionMasterIdEnc.ToString().Contains("+"))
                    //{
                    //    item.ElectionMasterId = int.Parse(COMMON_FUNCTION.Decrypt(item.ElectionMasterIdEnc.ToString().Replace('+', ' ')));
                    //}
                    //else 
                    if (item.ElectionMasterIdEnc.ToString().Contains(" "))
                    {
                        item.ElectionMasterId = int.Parse(COMMON_FUNCTION.Decrypt(item.ElectionMasterIdEnc.ToString().Replace(' ', '+')));
                    }
                    else
                    {
                        item.ElectionMasterId = int.Parse(COMMON_FUNCTION.Decrypt(item.ElectionMasterIdEnc.ToString()));
                    }
                }
                catch
                {
                }
            }
            var result = objElectionModel.InsertSocietyElectionOfficer(ObjListSocietyElecOffi.DataArray);

            if (result > 0)
            {
                var SocietyListOfOrder = objElectionModel.GetSocietyListByOrderID(ObjListSocietyElecOffi.DataArray[0].ElectionMasterId.ToString());
                var ElectionDates = objElectionModel.ElectionPublishList(ObjListSocietyElecOffi.DataArray[0].ElectionMasterId.ToString());
                GenPDFEng(SocietyListOfOrder, ElectionDates, ObjListSocietyElecOffi.DataArray[0].ElectionMasterId.ToString());
                GenPDFHin(SocietyListOfOrder, ElectionDates, ObjListSocietyElecOffi.DataArray[0].ElectionMasterId.ToString());
                objElectionModel.updateElectionOrderPDF("Order" + ObjListSocietyElecOffi.DataArray[0].ElectionMasterId.ToString() + ".pdf", ObjListSocietyElecOffi.DataArray[0].ElectionMasterId);
            }

            return Json(result.ToString());

        }
        public int GenPDFEng(DataTable SocietyListOfOrder, DataTable ElectionDates, string ElectionMasterId)
        {
            ElectionModel objElectionModel = new ElectionModel(new DBLogger());
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<table style='text-align:center'><tr><td style='text-align:center'>");
            sb.Append("<h5><u>Govt of Rajasthan</u></h5>");
            sb.Append("<p >Office of Rajasthan State Co-operative Election Authority </p>");
            sb.Append("<p >10-B Institutional Sector, Raesem Bhawan, Jhana Dungari, Jaipur -302004 Fax-0141-2712544 Phone -0141-2709018 </p>");
            sb.Append("<p >Election program for election of members and office bearers of District Board of Cooperative Societies </p>");
            // sb.Append("<p style='text-align:left'>No. F. 3 (1) R.R. Nirva.Priday/General/2014/2305</p>");
            sb.Append("<p style='text-align:right' >Date</p> ");
            sb.Append("<p >Order </p>");
            sb.Append("<p style='text-align:justify' >Honorable Rajasthan High Court, Jodhpur, SBCWP No. 9315/2015 and SBCWP No. After passing the interim order ");
            sb.Append("dated 27.08.2015, 9316/2015, the effect of the election and implementation of the statutory order number 2010 dated 24.08.2015 for the election of the members ");
            sb.Append("and officials of the Board of Directors of the District Dairy Produce Cooperative Societies, Rejecting 24.08.2015 from the State on the addendum-1 attached to ");
            sb.Append("the election of the members and office-bearers of the Board of Directors of all District Dungi Sangha. Acn program is released, so the district Dugg producer is ");
            sb.Append("appointed Returning Officer to Face officer with the name of the respective Union in Appendix -2 for the election of cooperative associations for the election of the Union</p> ");
            sb.Append("<p style='text-align:justify' >Rajasthan Co-operative Societies Act, 2001 (as amended), Rajasthan Cooperative Society Rules, 2003, Regarding registered ");
            sb.Append("registered sub-rules of the concerned institution, registrar for use of membership rights, norms issued by cooperative society, Rajasthan and co-operative institutions by ");
            sb.Append("Rajasthan State Cooperative Election Authority For the election of members and office-bearers of the Board of Directors General instructions will be concluded on 20.02.2015, 02.03.2015 and 13.08.2015.</p>  ");
            sb.Append("<p style='text-align:justify' >This order was issued today on 31.08.2015, marked with my signature and office posture. </p> ");
            //sb.Append("<p style='text-align:left' > Appendix 1 and 2 attached  </p> ");
            //sb.Append("<p style='text-align:right' > State co-election officer </p> ");
            //sb.Append("<p style='text-align:left' > No. F. 3 (1) R.R. Nirva.Priday / General / 2014 / 2306-98</p> ");
            //sb.Append("<p style='text-align:left' >Copy is sent to the following for information and necessary action-</p> ");
            //sb.Append("<p style='text-align:left' >1. Mr. Sir, Chief Secretary, Cooperative Department, Rajasthan, Jaipur.</p> ");
            //sb.Append("<p style='text-align:left' >2. Sr. Registrar, Cooperative Societies, Rajasthan Jaipur</p> ");
            //sb.Append("<p style='text-align:left' >3. Mr. Managing Director, Rajasthan Co-operative Dairy Fundraiser Jaipur.</p> ");
            //sb.Append("<p style='text-align:left' >4. Institutional Development Officer, Rajasthan Co-operative Dairy Fundraisers Jaipur.</p> ");
            //sb.Append("<p style='text-align:left' >5. Zonal Training Officer and Additional Registrar, Co-operative Societies, Block All</p> ");
            //sb.Append("<p style='text-align:left' >6. Unit Returning Officer and Deputy Registrar, Co-operative Societies, all</p> ");
            //sb.Append("<p style='text-align:left' >7. Election Officer Shri</p> ");
            //sb.Append("<p style='text-align:left' >8. Managing Director, District Dairy Produce Co-operative Association Ltd.</p> ");
            sb.Append("<p style='text-align:right' >State Coop-Election Authority</p><br/>");

            sb.Append("</td></tr><tr><td style='text-align:center'>");

            //  sb.Append("<p style='text-align: right;color:black'>Appendix-1</p>");
            sb.Append("");
            sb.Append("<h5><u>Govt of Rajasthan</u></h5>");
            sb.Append("<p >Office of Rajasthan State Co-operative Election Authority </p>");
            sb.Append("<p>10-B Institutional Sector, Raesem Bhawan, Jhana Dungari, Jaipur -302004 Fax-0141-2712544 Phone -0141-2709018</p>");
            sb.Append("<p>Election program for election of members and office bearers of District Board of Cooperative Societies </p> ");
            sb.Append("<table style='border:1px solid black;'>");
            sb.Append("<thead> <tr> <th style='text-align:left'> Sr. No.</th><th style='text-align:left'>Event</th><th style='text-align:left'> Date</th> </tr> </thead>");
            sb.Append("<tbody>");
            int RowNo = 1;
            foreach (DataRow row in ElectionDates.Rows)
            {
                sb.Append(" <tr> <th style='text-align:left'> " + (RowNo < 9 ? RowNo.ToString() : "&nbsp;") + " </th><th style='text-align:left'>" + row["QuestionEnWithDates"].ToString() + "</th><th style='text-align:left'> " + row["StartDate"].ToString() + "</th> </tr>");
                RowNo++;
            }
            sb.Append("</tbody></table>");

            sb.Append("</td></tr><tr><td style='text-align:center'>");
            sb.Append("<table style='width:100%;border:1px solid black;'>");
            sb.Append("<thead> <tr> <th style='text-align:left'> Sr. No.  </th>  <th style='text-align:left'>  Name of Society </th>  <th style='text-align:left'> Name Of Election Officer  </th>  </tr>  </thead>");
            sb.Append("<tbody>");
            RowNo = 1;
            foreach (DataRow row in SocietyListOfOrder.Rows)
            {
                sb.Append(" <tr> <th style='text-align:left'> " + RowNo.ToString() + " </th><th style='text-align:left'>" + row["SocietyName"].ToString() + "</th><th style='text-align:left'> " + row["OfficerNameEn"].ToString() + "</th> </tr>");
                RowNo++;
            }
            sb.Append("</tbody></table>");
            sb.Append("</td></tr></table>");

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
            //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            //UserModel OBJUser = new UserModel(new FileLogger());
            ////Why I am adding this to page footer?? So we can get paging in footer section of each PDF page.how its working?? A Javascript code is written inside the DLL which is handling  Div's class i.e. page and topage.
            //pdfDoc.PageFooterHtml = "<div style='float:right;'>  Page <span class='page'></span> of  <span class='topage'></span></div>";
            //bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);
            //var pdfBytes = pd1fDoc.GeneratePdf(htmlContent);

            ////System.IO.File.WriteAllBytes(System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraion.Replace("/", "") + "/") + result.ToString() + ".pdf"), pdfBytes);
            ////System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraion.Replace("/", "") + "/BankOpeningCertificate.pdf");
            //// Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);
            //System.IO.File.WriteAllBytes(Server.MapPath("~/Content/ElectionOrder/EngOrder" + ElectionMasterId.ToString() + ".pdf"), pdfBytes);

            ////Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);
            ////objElectionModel.updateElectionOrderPDF("Eng" +"Order" +ElectionMasterId.ToString() + ".pdf", Convert.ToInt32(ElectionMasterId));
            ////Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>")));
            if (COMMON_FUNCTION.GeneratePDF(sb1, Server.MapPath("~/Content/ElectionOrder/EngOrder" + ElectionMasterId.ToString() + ".pdf")))
            {
                return 1;
            }
            else return 0;

        }
        public int GenPDFHin(DataTable SocietyListOfOrder, DataTable ElectionDates, string ElectionMasterId)
        {
            ElectionModel objElectionModel = new ElectionModel(new DBLogger());
            // StringBuilder sb = new StringBuilder();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<html><body><div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;' ><center></center></div><br /><div id='divtoPrint'>");
            sb.Append("<div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;font-size: 20px; '>");
            sb.Append("<table style='width: 100% ;font-weight: normal;' border='0' cellspacing='0' cellpadding='0'>");
            sb.Append("<tr><td style='text-align:center'>");
            sb.Append("<h5><u>राजस्थान सरकार</u></h5>");
            sb.Append("<p>कार्यालय राजस्थान राज सहकारी निर्वाचन प्राधिकारी</p>");
            sb.Append("<p>10-बी संस्थागत क्षेत्र ,रायसेम भवन ,झालाना डूंगरी ,जयपुर -302004 फैक्स-0141-2712544 फ़ोन -0141-2709018</p>");
            sb.Append("<p>जिला सहकारी संघो के संचालक मंडल के सदस्यों एवं पदाधिकारी के निर्वाचन हेतु निर्वाचन कार्यक्रम</p>");
            //sb.Append("<p style='text-align:left'>क्रमांक फा.3(1)रा.रा.सह.निर्वा.प्रधि/सामान्य/2014/2305</p>");
            sb.Append("<p style='text-align:right' >दिनांक</p> ");
            sb.Append("<p>आदेश</p>");
            sb.Append("<p style='text-align:justify' >माननीय राजस्थान उच्च न्यायलय जोधपुर ने SBCWP No. 9315/2015 एवं SBCWP No. 9316/2015 में अंतरिम आदेश दिनांक 27.08.2015 पारित कर जिला दुग्घ उत्पादक सहकारी संघो के संचालक मंडल के सदस्यों एवं ");
            sb.Append(" पदाधिकारियों के निर्वाचन हेतु इस कार्यालय के संसंख्यक आदेश क्रमांक 2010 दिनांक 24.08.2015 के प्रभाव एवं क्रयान्वन को स्टे कर दिया है अतः आदेश दिनांक 24.08.2015 को निरस्त करते हुए राज्य से समस्त जिला दुग्घ संघो के ");
            sb.Append("संचालक मंडल के सदस्यों एवं पदाधिकारियों के निर्वाचन हेतु सलग्न परिशिष्ट-1 पर निर्वाचन कार्यक्रम जारी किया जाता है अतः जिला दुग्घ उत्पादक सहकारी संघो के निर्वाचन हेतु परिशिष्ट-2 में संबंधित संघ के नाम के समक्ष अंकित ");
            sb.Append("अधिकारी को उस संघ के निर्वाचन हेतु निर्वाचन अधिकारी नियुक्त किया जाता है  ");
            //sb.Append("appointed Returning Officer to Face officer with the name of the respective Union in Appendix -2 for the election of cooperative associations for the election of the Union</p> ");
            sb.Append("<p style='text-align:justify' >निर्वाचन अधिकारी द्वारा राजस्थान सहकारी सोसाइटी अधिनियम 2001(यथा संसोधित) राजस्थान सहकारी सोसाइटी नियम 2003, संबंधित संस्था के अधतन संसोधित पंजीकृत उपनियम, सदस्यता संबंधी अधिकारों के प्रयोग हेतु रजिस्ट्रार, ");
            sb.Append("सहकारी समितिया, राजस्थान द्वारा जारी मानदंड एवं राजस्थान राज्य सहकारी निर्वाचन प्राधिकरण द्वारा सहकारी संस्थाओ के संचालक मंडल के सदस्यों एवं पदाधिकारयों के निर्वाचन हेतु जारी सामान्य निर्देश दिनांक 20.02.2015,  ");
            sb.Append("02.03.2015 एवं 13.08.2015 की पालना करते हुए सम्पन्न कराये जायेंगे। </p>  ");
            sb.Append("<p style='text-align:justify' > यह आदेश आज दिनांक 31.08.2015 को मेरे हस्ताक्षर एवं कार्यालय मुद्रा से अंकित कर जारी किया गया । </p> ");
            //sb.Append("<p style='text-align:left' > सलग्न परिशिष्ट 1 एवं 2  </p> ");
            //sb.Append("<p style='text-align:right' > राज्य सहकारी निर्वाचन अधिकारी  </p> ");
            //sb.Append("<p style='text-align:left' > क्रमांक फा.3(1)रा.रा.सह.निर्वा.प्रधि/सामान्य/2014/2306-98</p> ");
            //sb.Append("<p style='text-align:right' >दिनांक</p> ");
            //sb.Append("<p style='text-align:left' >प्रतिलिपि निम्नलिखित को सूचनार्थ एवं आवश्यक कार्यवाही हेतु प्रेषित है -</p> ");
            //sb.Append("<p style='text-align:left' >1. श्रीमान प्रमुख शासन सचिव, सहकारिता विभाग राजस्थान जयपुर।</p> ");
            //sb.Append("<p style='text-align:left' >2. श्रीमान रजिस्ट्रार, सहकारी समितियां, राजस्थान जयपुर।</p> ");
            //sb.Append("<p style='text-align:left' >3. श्रीमान प्रबंध निर्देशक, राजस्थान को-ऑपरेटिव डेयरी फंडरेसन जयपुर।</p> ");
            //sb.Append("<p style='text-align:left' >4. संस्थागत विकाश अधिकारी, राजस्थान को-ऑपरेटिव डेयरी फंडरेसन जयपुर।</p> ");
            //sb.Append("<p style='text-align:left' >5. जोनल रिटर्निग अधिकारी एवं अतिरिक्त रजिस्ट्रार, सहकारी समितियां, खंड समस्त।</p> ");
            //sb.Append("<p style='text-align:left' >6. इकाई रिटर्निंग अधिकारी एवं उप रजिस्ट्रार, सहकारी समितियां, समस्त।</p> ");
            //sb.Append("<p style='text-align:left' >7. निर्वाचन अधिकारी श्री</p> ");
            //sb.Append("<p style='text-align:left' >8. प्रबंध निर्देशक, जिला दुग्घ उत्पादक सहकारी संघ लि.</p> ");
            //sb.Append("<p style='text-align:right' >राज्य सहकारी निर्वाचन प्राधिकारी</p><br/>");

            sb.Append("</td></tr><tr><td style='text-align:center'>");

            sb.Append("<p style='text-align: right;color:black'> परिशिष्ट -1</p>");
            sb.Append("");
            sb.Append("<h5><u> राजस्थान सरकार </u></h5>");
            sb.Append("<p> कार्यालय राजस्थान राज सहकारी निर्वाचन प्राधिकारी </p>");
            sb.Append("<p> 10 - बी संस्थागत क्षेत्र ,रायसेम भवन ,झालाना डूंगरी ,जयपुर -302004 फैक्स-0141-2712544 फ़ोन - 0141-2709018</p>");
            sb.Append("<p> जिला सहकारी संघो के संचालक मंडल के सदस्यों एवं पदाधिकारी के निर्वाचन हेतु निर्वाचन कार्यक्रम </p> ");
            sb.Append("<table class='table table-responsive devtable' style='border:1px solid black;'>");
            sb.Append("<thead> <tr> <th style='text-align:left'> क्रमांक </th><th style='text-align:left'>विवरण </th><th style='text-align:left'> तिथि </th> </tr> </thead>");
            sb.Append("<tbody>");
            int RowNo = 1;
            foreach (DataRow row in ElectionDates.Rows)
            {
                sb.Append(" <tr> <th style='text-align:left'> " + (RowNo < 9 ? RowNo.ToString() : "&nbsp;") + " </th><th style='text-align:left'>" + row["QuestionHiWithDates"].ToString() + "</th><th style='text-align:left'> " + row["StartDate"].ToString() + "</th> </tr>");
                RowNo++;
            }
            sb.Append("</tbody></table>");

            sb.Append("</td></tr><tr><td style='text-align:center'>");
            sb.Append("<table style='width:100%'>");
            sb.Append("<thead> <tr> <th style='text-align:left'>  क्रमांक  </th>  <th style='text-align:left'>  सोसाइटी का नाम  </th>  <th style='text-align:left'> नाम निर्वाचन अधिकारी </th>  </tr>  </thead>");
            sb.Append("<tbody>");
            RowNo = 1;
            foreach (DataRow row in SocietyListOfOrder.Rows)
            {
                sb.Append(" <tr> <th style='text-align:left'> " + RowNo.ToString() + " </th><th style='text-align:left'>" + row["SocietyName"].ToString() + "</th><th style='text-align:left'> " + (row["OfficerNameHi"].ToString() == "null" ? row["OfficerNameEn"].ToString() : row["OfficerNameHi"].ToString()) + " </th> </tr>");
                RowNo++;
            }
            sb.Append("</tbody></table>");
            sb.Append("</td></tr>");
            sb.Append("</table></div><br /></div></body></html>");

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
            //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            //UserModel OBJUser = new UserModel(new FileLogger());
            ////Why I am adding this to page footer?? So we can get paging in footer section of each PDF page.how its working?? A Javascript code is written inside the DLL which is handling  Div's class i.e. page and topage.
            //pdfDoc.PageFooterHtml = "<div style='float:right;'>  Page <span class='page'></span> of  <span class='topage'></span></div>";
            //bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);
            //var pdfBytes = pd1fDoc.GeneratePdf(htmlContent);

            ////System.IO.File.WriteAllBytes(System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraion.Replace("/", "") + "/") + result.ToString() + ".pdf"), pdfBytes);
            ////System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraion.Replace("/", "") + "/BankOpeningCertificate.pdf");
            ////Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);
            //System.IO.File.WriteAllBytes(Server.MapPath("~/Content/ElectionOrder/HinOrder" + ElectionMasterId.ToString() + ".pdf"), pdfBytes);
            //Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);
            //objElectionModel.updateElectionOrderPDF("Order" +  ElectionMasterId.ToString() +"Hin"+ ".pdf",  ElectionMasterId);
            //Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>")));
            if (COMMON_FUNCTION.GeneratePDF(sb1, Server.MapPath("~/Content/ElectionOrder/HinOrder" + ElectionMasterId.ToString() + ".pdf")))
            {
                return 1;
            }
            else return 0;
        }


        [HttpPost]
        [ActionName("InsertSocietyElectionOfficerByReturningOfficer")]
        public string InsertSocietyElectionOfficerByReturningOfficer(string OTP, string TXN, ListSocietyElecOffi ObjListSocietyElecOffi)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser == null)
            {
                return "LOGOUT";
            }

            var otpResult =  COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
            if (otpResult.status != "1")
            {
                return otpResult.data;
            }



            ElectionModel objElectionModel = new ElectionModel(new DBLogger());
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();

            foreach (SocietyElectionOfficer item in ObjListSocietyElecOffi.DataArray)
            {
                try
                {

                    if (item.ElectionMasterIdEnc.ToString().Contains(" "))
                    {
                        item.ElectionMasterId = int.Parse(COMMON_FUNCTION.Decrypt(item.ElectionMasterIdEnc.ToString().Replace(' ', '+')));
                    }
                    else
                    {
                        item.ElectionMasterId = int.Parse(COMMON_FUNCTION.Decrypt(item.ElectionMasterIdEnc.ToString()));
                    }
                }
                catch
                {
                }
            }


            string Result = COMMON_FUNCTION.Encrypt(objElectionModel.InsertSocietyElectionOfficer(ObjListSocietyElecOffi.DataArray).ToString());

            string filePath = Server.MapPath("~/Content/ElectionOrder/HinOrderEO" + ObjListSocietyElecOffi.DataArray[0].ElectionMasterId + ".pdf");
            int _ElectionFor = GenElecOffiPDFHin(ObjListSocietyElecOffi.DataArray[0].ElectionMasterId, filePath);

            if (_ElectionFor > 0)
            {

                var result1 = COMMON_FUNCTION.EsinghPDF(otpResult.data, filePath);
                string result = result1.data;
                if (result == "Success")
                {
                    // ElectionModel objElectionModel = new ElectionModel();
                    //objElectionModel.UpdateElectionMasterOrderPDFSign(ElectionMasterId.ToString(), true);
                    objElectionModel.updateElectionOrderPDF("HinOrderEO" + ObjListSocietyElecOffi.DataArray[0].ElectionMasterId + ".pdf", ObjListSocietyElecOffi.DataArray[0].ElectionMasterId, 1);
                    var DT = objElectionModel.GetElectionEmailSMSDetails(30, 2, ObjListSocietyElecOffi.DataArray[0].ElectionMasterId, 0);
                    foreach (DataRow dr in DT.Rows) // search whole table
                    {
                        new EsanchaarController().SendSocietySMS(dr["sms"].ToString().Replace("APPLICANTNAME", dr["OfficerNameEn"].ToString()), dr["MobileNo"].ToString());
                    }
                    return "Success";
                }
                else
                {
                    return result;
                }

            }
            else
            {
                Result = "PDF Not Generate";
            }


            return (Result.ToString());

        }
        public void GenElecOffiPDFEng(int ElectionMasterId)
        {
            ElectionModel objElectionModel = new ElectionModel(new DBLogger());
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            DataTable SocietyListOfOrder = (objElectionModel.GetSocietiesWihEOByOrderId(ElectionMasterId.ToString()));
            sb.Append("<table style='text-align:center'><tr><td style='text-align:center'>");

            sb.Append("<h5><u>Govt of Rajasthan</u></h5>");
            sb.Append("<p >Office of Rajasthan State Co-operative Election Authority </p>");
            sb.Append("<p>10-B Institutional Sector, Raesem Bhawan, Jhana Dungari, Jaipur -302004 Fax-0141-2712544 Phone -0141-2709018</p>");
            sb.Append("<p>Election program for election of members and office bearers of District Board of Cooperative Societies </p> ");
            sb.Append("<table style='border:1px solid black;'>");
            sb.Append("<thead> <tr > <th style='text-align:left'> Sr. No.</th><th style='text-align:left'>Society Name</th><th style='text-align:left'> Election Officer</th> <th style='text-align:left'> Step</th> </tr> </thead>");
            sb.Append("<tbody>");
            int RowNo = 1;
            foreach (DataRow row in SocietyListOfOrder.Rows)
            {
                sb.Append(" <tr> <td style='text-align:left'> " + RowNo.ToString() + " </td><td style='text-align:left'>" + row["SocietyName"].ToString() + "</td><td style='text-align:left'> " + row["ElectionOfficerNameEng"].ToString() + "</td> <td style='text-align:left'> " + row["StepEng"].ToString() + "</td></tr>");
                RowNo++;
            }
            sb.Append("</tbody></table>");
            sb.Append("</td></tr></table>");

            string InpuData = sb.ToString();

            string PageType = "Portrait";
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

            var htmlContent = InpuData;
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            UserModel OBJUser = new UserModel(new FileLogger());
            //Why I am adding this to page footer?? So we can get paging in footer section of each PDF page.how its working?? A Javascript code is written inside the DLL which is handling  Div's class i.e. page and topage.
            //pdfDoc.PageFooterHtml = "<div style='float:right;'>  Page <span class='page'></span> of  <span class='topage'></span></div>";
            //bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);
            //var pdfBytes = pdfDoc.GeneratePdf(htmlContent);

            //System.IO.File.WriteAllBytes(System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraion.Replace("/", "") + "/") + result.ToString() + ".pdf"), pdfBytes);
            //System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraion.Replace("/", "") + "/BankOpeningCertificate.pdf");
            // Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);
            //System.IO.File.WriteAllBytes(Server.MapPath("~/Content/ElectionOrder/EngOrderEO" + ElectionMasterId.ToString() + ".pdf"), pdfBytes);
            // Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);
            //objElectionModel.updateElectionOrderPDF("Eng" +"Order" +ElectionMasterId.ToString() + ".pdf", Convert.ToInt32(ElectionMasterId));
            //Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);


        }
        public void GenElecOffiPDFHin123(int ElectionMasterId)
        {
            ElectionModel objElectionModel = new ElectionModel(new DBLogger());
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string[] _ElectionStepKey = new string[] { "çFke pj.k", "f}rh; pj.k", "r`rh; pj.k", "prqFkZ pj.k", "iape pj.k" };
            string[] _ElectionStepValue = new string[] { "Stap1Date", "Stap2Date", "Stap3Date", "Stap4Date", "Stap5Date", "Stap6Date", "Stap7Date", "Stap8Date", "Stap9Date", "Stap10Date" };
            string _districtName = string.Empty;
            string _returningOfficerName = string.Empty;
            string _divisionName = string.Empty;
            int _ElectionFor = 0;
            int _stepsInElection = 1;
            var SocietyListOfOrder = objElectionModel.GetSocietyListByOrderID(ElectionMasterId.ToString());
            var ElectionDates = objElectionModel.ElectionPublishList(ElectionMasterId.ToString());
            if (SocietyListOfOrder != null && SocietyListOfOrder.Rows.Count > 0)
            {
                _districtName = Convert.ToString(SocietyListOfOrder.Rows[0]["Society_DISTRICT_ENG"]);
                _returningOfficerName = Convert.ToString(SocietyListOfOrder.Rows[0]["OfficerNameEN"]);
                _divisionName = Convert.ToString(SocietyListOfOrder.Rows[0]["Society_DIVISION_ENG"]);
                _stepsInElection = Convert.ToInt32(ElectionDates.Rows[0]["StepInElection"]);
                _ElectionFor = Convert.ToInt32(SocietyListOfOrder.Rows[0]["ElectionFor"]);
            }

            DataTable SocietyListOfOrderAll = (objElectionModel.GetSocietiesWihEOByOrderId(ElectionMasterId.ToString()));

            if (SocietyListOfOrder.Rows.Count > 0)
            {
                if (_ElectionFor != 3)
                {
                    sb.Append("<html><head><title></title><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head><body><div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;' ><center></center></div><br /><div id='divtoPrint'>");
                    sb.Append("<div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px; '>");
                    sb.Append("<table style='width: 100% ;font-size: 23px;font-weight: normal;' border='0' cellspacing='0px' cellpadding='0px'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:center'>");
                    sb.Append("<h5><u>jktLFkku ljdkj</u></h5>");
                    sb.Append("<p>dk;kZy; jktLFkku jkT; lgdkjh fuokZpu çkf/kdj.k</p>");
                    sb.Append("<p>10&ch laLFkkxr {ks= ]jk;lse Hkou ]>kykuk Mwaxjh ]t;iqj</p>");
                    sb.Append("<p>Qksu &0141&2712544 QSDl&0141&2709018 </p>");
                    sb.Append("<p><b>Email ID scea@rajasthan.gov.in </b> </p>");
                    sb.Append("<p>lgdkjh lfefr;ksa ds lnL;ksa ,oa inkf/kdkjh ds fuokZpu gsrq fuokZpu dk;ZØe </p> ");
                    //sb.Append("<p style='text-align:left'>क्रमांक फा.3(1)रा.रा.सह.निर्वा.प्राधि/" + _districtName + "/2016</p>");
                    sb.Append("<p style='text-align:right'>fnukad " + DateTime.Now.ToString("dd/MM/yyyy") + "</p> ");
                    sb.Append("<p >आदेश</p>");

                    sb.Append("<table class='table table-responsive devtable' style='border:1px solid black;width: 100% ;'>");
                    sb.Append("<thead> <tr style='border:1px solid black;width: 100% ;'> <th style='border:1px solid black;text-align:left'> Øekad </th><th style='border:1px solid black;text-align:left'>lkslk;Vh</th><th style='border:1px solid black;text-align:left'>fuokZpu vf/kdkjh </th><th style='border:1px solid black;text-align:left'>pquko dk pj.k</th> </tr> </thead>");
                    sb.Append("<tbody>");
                    int RowNo = 1;
                    foreach (DataRow row in SocietyListOfOrderAll.Rows)
                    {
                        sb.Append(" <tr style='border:1px solid black;width: 100% ;'> <td style='border:1px solid black;text-align:left'> " + RowNo.ToString() + " </td><td style='border:1px solid black;text-align:left'>" + row["SocietyName"].ToString() + "</td><td style='border:1px solid black;text-align:left'> " + row["ElectionOfficerNameEng"].ToString() + "</td> <td style='border:1px solid black;text-align:left'> " + row["StepHin"].ToString() + "</td></tr>");
                        RowNo++;
                    }
                    sb.Append("</tbody></table>");
                    sb.Append("</td></tr>");
                    sb.Append("</table></div><br /></div></body></html>");

                    string InpuData = sb.ToString();

                    string PageType = "Portrait";
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

                    var htmlContent = InpuData;
                    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                    UserModel OBJUser = new UserModel(new FileLogger());
                    // Why I am adding this to page footer ?? So we can get paging in footer section of each PDF page.how its working ?? A Javascript code is written inside the DLL which is handling Div's class i.e. page and topage.
                    //pdfDoc.PageFooterHtml = "<div style='float:right;'>  Page <span class='page'></span> of  <span class='topage'></span></div>";
                    //var pdfBytes = pdfDoc.GeneratePdf(htmlContent);
                    // System.IO.File.WriteAllBytes(Server.MapPath("~/Content/ElectionOrder/HinOrderEO" + ElectionMasterId.ToString() + ".pdf"), pdfBytes);
                }
                else
                {
                    // StringBuilder sb = new StringBuilder();
                    sb.Append("<html><head><title></title><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head><body><div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;' ><center></center></div><br /><div id='divtoPrint'>");
                    sb.Append("<div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px; '>");
                    sb.Append("<table style='width: 100% ;font-size: 23px;font-weight: normal;' border='0' cellspacing='0px' cellpadding='0px'>");

                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:center'>");
                    sb.Append("<h5><u>राजस्थान सरकार</u></h5>");
                    sb.Append("<p>कार्यालय राजस्थान राज्य सहकारी निर्वाचन प्राधिकरण</p>");
                    sb.Append("<p> 10-बी संस्थागत क्षेत्र ,रायसेम भवन ,झालाना डूंगरी ,जयपुर</p>");
                    sb.Append("<p> फ़ोन -0141-2712544 फैक्स-0141-2709018 </p>");
                    sb.Append("<p> <b> Email ID scea@rajasthan.gov.in </b> </p>");
                    //sb.Append("<p style='text-align:left'>क्रमांक फा.3(1)रा.रा.सह.निर्वा.प्राधि/" + _districtName + "/2016</p>");
                    sb.Append("<p style='text-align:right'>दिनांक " + DateTime.Now.ToString("dd/MM/yyyy") + "</p> ");
                    sb.Append("<p >आदेश</p>");
                    sb.Append("<p style='text-align:justify' >राजस्थान सहकारी सोसाइटी अधिनियम 2001 की धारा 33,35 एवं राजस्थान सहकारी सोसायटी ");

                    sb.Append("नियम 2003 के नियम 45 व  46 के अंतर्गत प्रदत्त शक्तियों के अनुसरण परिशिष्ठ -1 में ");
                    sb.Append("अंकित सहकारी संस्थाओं के संचालक मंडल के सदस्यों एवं पदाधिकारियों के निर्वाचन करवाए जाने हेतु ");
                    sb.Append("निर्वाचन अधिकारी नियुक्त किया जाता है| निर्वाचन कार्यक्रम परिशिष्ठ-2 पर संलग्न है|</p>");

                    sb.Append("<p style='text-align:justify'>निर्वाचन अधिकारी द्वारा राजस्थान सहकारी सोसायटी अधिनियम 2001 (यथा संशोथित), राजस्थान");
                    sb.Append("राजस्थान सोसायटी नियम 2003, (यथा संशोथित), संबंधित संस्थाओ के अद्यतन संशोधित पंजीकृत उपनियमों एवं");
                    sb.Append("राजस्थान राज्य सहकारी निर्वाचन प्राधिकरण द्वारा सहकारी संस्थाओ के निर्वाचन हेतु जारी सामान्य निर्देश क्रमांक");
                    sb.Append("फा.3(1)रा.रा.सह.निर्वा.प्राधि/16-35-43 दिनांक 20.02.2015 (http://rajsahakar.rajasthan.gov.in/) वेबसाइट पर");
                    sb.Append("महत्वपूर्ण आदेशों में Election Folder पर उपलब्ध है) की पालना करते हुए निर्वाचन सम्पन्न कराये जाएंगे |</p>");
                    sb.Append("<p style='text-align:justify'>यह आदेश आज दिनांक " + DateTime.Now.ToString("dd/MM/yyyy") + "  को मेरे हस्ताक्षर एवं कार्यालय मुद्रा से अंकित कर जारी किया गया |</p>");

                    sb.Append("<p style='text-align:left'><b>संलग्न उपरोक्तानुसार</b></p>");
                    //sb.Append("<p style='text-align:right'><b>राज्य सहकारी निर्वाचन प्राधिकारी </b></p>");
                    //sb.Append("<p style='text-align:left' > क्रमांक फा.3(1)रा.रा.सह.निर्वा.प्रधि/" + _districtName + "/2016</p> ");
                    //sb.Append("<p style='text-align:right' >दिनांक" + DateTime.Now.ToString("dd/MM/yyyy") + "</p> ");
                    sb.Append("<p style='text-align:left' > प्रतिलिपि निम्नलिखित को सूचनार्थ एवं आवश्यक कार्यवाही हेतु प्रेषित है :-   </p> ");
                    sb.Append("<p style='text-align:left' >1. श्रीमान प्रमुख शासन सचिव, सहकारिता विभाग राजस्थान जयपुर।</p> ");
                    sb.Append("<p style='text-align:left' >2. श्रीमान रजिस्ट्रार, सहकारी समितियां, राजस्थान जयपुर।</p> ");
                    sb.Append("<p style='text-align:left' >3. जोनल रिटर्निग अधिकारी एवं अतिरिक्त रजिस्ट्रार, सहकारी समितियां," + _divisionName + " खंड " + _divisionName + "</p> ");
                    sb.Append("<p style='text-align:left' >4. इकाई रिटर्निंग अधिकारी एवं उप रजिस्ट्रार, सहकारी समितियां, " + _districtName + "|</p> ");
                    sb.Append("<p style='text-align:left' >5. गार्ड पत्रावली</p> ");

                    //sb.Append("<p style='text-align:right' >राज्य सहकारी निर्वाचन प्राधिकारी</p><br/>");
                    sb.Append("</td></tr>");

                    sb.Append("<tr><td style='text-align:center'>");
                    sb.Append("<p style='text-align: right;color:black'>परिशिष्ट-1</p>");
                    sb.Append("<h5><u>राजस्थान सरकार </u></h5>");
                    sb.Append("<p>कार्यालय राजस्थान राज्य सहकारी निर्वाचन प्राधिकरण </p>");
                    sb.Append("<p>10-बी संस्थागत क्षेत्र ,रायसेम भवन ,झालाना डूंगरी ,जयपुर -302004</p>");
                    sb.Append("<p> फ़ोन -0141-2712544 फैक्स-0141-2709018 </p>");
                    sb.Append("<p> <b> Email ID scea@rajasthan.gov.in </b> </p>");

                    sb.Append("<table style='width:100%;border:1px solid #000;'>");
                    sb.Append("<thead> <tr style='border:1px solid black;width: 100% ;'> <th style='border:1px solid black;text-align:left'> क्रमांक </th><th style='border:1px solid black;text-align:left'>सोसायटी</th><th style='border:1px solid black;text-align:left'> निर्वाचन अधिकारी </th><th style='border:1px solid black;text-align:left'> चुनाव का चरण </th> </tr> </thead>");
                    sb.Append("<tbody>");
                    int RowNo = 1;
                    foreach (DataRow row in SocietyListOfOrderAll.Rows)
                    {
                        sb.Append(" <tr style='border:1px solid black;width: 100% ;'> <td style='border:1px solid black;text-align:left'> " + RowNo.ToString() + " </td><td style='border:1px solid black;text-align:left'>" + row["SocietyName"].ToString() + "</td><td style='border:1px solid black;text-align:left'> " + row["ElectionOfficerNameEng"].ToString() + "</td> <td style='border:1px solid black;text-align:left'> " + row["StepHin"].ToString() + "</td></tr>");
                        RowNo++;
                    }
                    sb.Append("</tbody></table>");
                    sb.Append("</td></tr>");

                    sb.Append("<tr><td style='text-align:center'>");
                    sb.Append("<p style='text-align: right;color:black;page-break-before:always;'>परिशिष्ट-2</p>");
                    sb.Append("<h5><u>राजस्थान सरकार </u></h5>");
                    sb.Append("<p>कार्यालय राजस्थान राज्य सहकारी निर्वाचन प्राधिकरण </p>");
                    sb.Append("<p>10-बी संस्थागत क्षेत्र ,रायसेम भवन ,झालाना डूंगरी ,जयपुर -302004 फैक्स-0141-2709018 फ़ोन -0141-2712544  </p>");
                    sb.Append("<p> <b> परिशिष्ठ -१ में अंकित सहकारी संस्थाओं के संचालन मंडल के सदस्यों एवं पदाधिकारियों के </b> </p>");
                    sb.Append("<p> <b> निर्वाचन हेतु निर्वाचन कार्यक्रम </b> </p>");

                    sb.Append("<table class='table table-responsive devtable' style='border:1px solid #000;width:100%;'>");
                    sb.Append("<thead> <tr> <th style='text-align:left;border:1px solid #000;'> क्रमांक </th><th style='text-align:left;border:1px solid #000;'>विवरण </th>");

                    for (int i = 0; i < _stepsInElection; i++)
                    {
                        sb.Append("<th style ='text-align:left;border:1px solid #000;' > " + _ElectionStepKey[i] + " </ th >");
                    }

                    sb.Append("</tr> </thead>");
                    sb.Append("<tbody>");
                    RowNo = 1;

                    foreach (DataRow row in ElectionDates.Rows)
                    {
                        sb.Append(" <tr> <td style='text-align:left;border:1px solid #000;'> " + (RowNo < 9 ? RowNo.ToString() : "&nbsp;") + " </td><td style='text-align:left;border:1px solid #000;'>" + row["QuestionHiWithDates"].ToString() + "</td>");

                        for (int i = 0; i < _stepsInElection; i++)
                        {
                            sb.Append("<td style = 'text-align:left;border:1px solid #000;' > " + Convert.ToString(row[_ElectionStepValue[i]]) + " </td>");
                        }
                        sb.Append("</tr>");

                        RowNo++;
                    }


                    sb.Append("</tbody></table>");


                    if (ElectionDates.Rows.Count > 11)
                    {
                        sb.Append("<p style='text-align:justify'> <b>नोट :- संचालन मंडल के समस्त सदस्यों के निर्विरोध निर्वाचन की स्थिति में पदाधिकारियों का चुनाव ");
                        DataRow drFrom = ElectionDates.Rows[7];
                        DataRow drTo = ElectionDates.Rows[12];
                        for (int j = 0; j < _stepsInElection; j++)
                        {
                            sb.Append(_ElectionStepKey[j] + " में दिनांक " + Convert.ToString(drFrom[_ElectionStepValue[j]]) + " के स्थान पर दिनांक " + Convert.ToString(drTo[_ElectionStepValue[j]]) + ", ");
                        }

                        sb.Append("को क्रम संख्या 8 में अंकित निर्वाचन कार्यक्रम अनुसार संपन्न करवाया जावेगा| </b> </p>");
                        //sb.Append("<p style='text-align:right'><b>राज्य सहकारी निर्वाचन प्राधिकारी </b></p>");
                    }

                    sb.Append("</td></tr>");

                    sb.Append("</table></div><br /></div></body></html>");

                    string InpuData = sb.ToString();

                    string PageType = "Portrait";

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
                    //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                    //UserModel OBJUser = new UserModel(new FileLogger());
                    ////Why I am adding this to page footer?? So we can get paging in footer section of each PDF page.how its working?? A Javascript code is written inside the DLL which is handling  Div's class i.e. page and topage.
                    //pdfDoc.PageFooterHtml = "<div style='float:right;'>  Page <span class='page'></span> of  <span class='topage'></span></div>";
                    //bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);
                    //var pdfBytes = pdfDoc.GeneratePdf(htmlContent);

                    ////System.IO.File.WriteAllBytes(System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraion.Replace("/", "") + "/") + result.ToString() + ".pdf"), pdfBytes);
                    ////System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraion.Replace("/", "") + "/BankOpeningCertificate.pdf");
                    ////Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);
                    //System.IO.File.WriteAllBytes(Server.MapPath("~/Content/ElectionOrder/HinOrderEO" + ElectionMasterId.ToString() + ".pdf"), pdfBytes);
                    //Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);
                    ////objElectionModel.updateElectionOrderPDF("Order" +  ElectionMasterId.ToString() +"Hin"+ ".pdf",  ElectionMasterId);
                    ////Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);
                }
            }

        }

        public int GenElecOffiPDFHin(int ElectionMasterId, string FuilePath)
        {
            ElectionModel objElectionModel = new ElectionModel(new DBLogger());
            System.Text.StringBuilder sb = new System.Text.StringBuilder();


            string[] _ElectionStepKey = new string[] { "प्रथम चरण", "द्वितीय चरण", "तृतीय चरण", "चतुर्थ चरण", "पंचम चरण", "षष्ठम चरण", "सप्तम चरण", "अष्टम चरण", "नवम चरण", "दशम चरण" };

            string[] _ElectionStepValue = new string[] { "Stap1Date", "Stap2Date", "Stap3Date", "Stap4Date", "Stap5Date", "Stap6Date", "Stap7Date", "Stap8Date", "Stap9Date", "Stap10Date" };
            string _districtName = string.Empty;
            string _returningOfficerName = string.Empty;
            string _divisionName = string.Empty;
            int _ElectionFor = 0;
            int _stepsInElection = 1;
            var SocietyListOfOrder = objElectionModel.GetSocietyListByOrderID(ElectionMasterId.ToString());
            var ElectionDates = objElectionModel.ElectionPublishList(ElectionMasterId.ToString());
            if (SocietyListOfOrder != null && SocietyListOfOrder.Rows.Count > 0)
            {
                _districtName = Convert.ToString(SocietyListOfOrder.Rows[0]["Society_DISTRICT_MANGAL"]); ;
                _returningOfficerName = Convert.ToString(SocietyListOfOrder.Rows[0]["OfficerNameHi"]);
                _divisionName = Convert.ToString(SocietyListOfOrder.Rows[0]["Society_DIVISION_MANGAL"]);
                _stepsInElection = Convert.ToInt32(ElectionDates.Rows[0]["StepInElection"]);
                _ElectionFor = Convert.ToInt32(SocietyListOfOrder.Rows[0]["ElectionFor"]);

            }

            DataTable SocietyListOfOrderAll = (objElectionModel.GetSocietiesWihEOByOrderId(ElectionMasterId.ToString()));

            if (SocietyListOfOrder.Rows.Count > 0)
            {
                if (_ElectionFor != 3)
                {
                    sb.Append("<html><body><div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;' ><center></center></div><br /><div id='divtoPrint'>");
                    sb.Append("<div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;font-size:20px; '>");
                    sb.Append("<table id='content' style='width:100%;'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:center'>");
                    sb.Append("<b><u> राजस्थान सरकार </u></b>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td  style='text-align:center;'> कार्यालय राजस्थान राज्य सहकारी निर्वाचन प्राधिकरण </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td  style='text-align:center;'> 10 - बी संस्थागत क्षेत्र , रायसेम भवन, झालाना डूंगरी , जयपुर </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td  style='text-align:center;'> फोन -0141-2712544 फैक्स -0141-2709018  </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td  style='text-align:center;'><b> Email ID scea@rajasthan.gov.in </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td  style='text-align:center;padding-top:20px;'> सहकारी समितियों के सदस्यों एवं पदाधिकारी के निर्वाचन हेतु निर्वाचन कार्यक्रम  </td> ");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:right;padding-top:30px'> दिनांक " + DateTime.Now.ToString("dd/MM/yyyy") + " </td> ");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td  style='text-align:center;padding-top:10px;'> आदेश </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td>");
                    sb.Append("<table border='1'  cellpadding='5' cellspacing='0' style='width: 100%;'>");
                    sb.Append("<thead> <tr style='width: 100% ;'> <th style='text-align:left'> क्रमांक  </th><th style='text-align:left'> सोसायटी </th><th style='text-align:left'> निर्वाचन अधिकारी </th><th style='text-align:left'> चुनाव का चरण </th> </tr> </thead>");
                    sb.Append("<tbody>");
                    int RowNo = 1;
                    foreach (DataRow row in SocietyListOfOrderAll.Rows)
                    {
                        sb.Append("<tr style='width: 100% ;'> <td style='text-align:left'> " + RowNo.ToString() + " </td><td style='text-align:left'><label style='font-family: arial;font-size:17px;'>" + row["SocietyName"].ToString() + "</label></td><td style='text-align:left'> <label style='font-family: arial;font-size:17px;'>" + row["ElectionOfficerNameEng"].ToString() + " </label></td> <td style='text-align:left'> " + row["StepHin"].ToString() + " </td></tr>");
                        RowNo++;
                    }
                    sb.Append("</tbody></table>");
                    sb.Append("</td>"); sb.Append("</tr>");
                    //bhupendra

                    sb.Append(" <tr> ");
                    sb.Append(" <td style='text-align: right;color:black;page-break-before:always;'> परिशिष्ट -2 </td> ");
                    sb.Append(" </tr> ");
                    sb.Append(" <tr> ");
                    sb.Append(" <td style='text-align:center;'><u> राजस्थान सरकार </u></td> ");
                    sb.Append(" </tr> ");
                    sb.Append(" <tr> ");
                    sb.Append(" <td style='text-align:center;'> कार्यालय राजस्थान राज्य सहकारी निर्वाचन प्राधिकरण </td> ");
                    sb.Append(" </tr> ");
                    sb.Append(" <tr> ");
                    sb.Append(" <td style='text-align:center;'> 10 - बी संस्थागत क्षेत्र , रायसेम भवन , झालाना डूंगरी , जयपुर -302004 </td> ");
                    sb.Append(" </tr> ");
                    sb.Append(" <tr> ");
                    sb.Append(" <td style='text-align:center;'> फैक्स-0141-2709018 फ़ोन -0141-2712544 </td> ");
                    sb.Append(" </tr> ");
                    sb.Append(" <tr> ");
                    sb.Append(" <td style='text-align:center;'> <b> परिशिष्ठ -१ में अंकित सहकारी संस्थाओं के संचालन मंडल के सदस्यों एवं पदाधिकारियों के </b> </td> ");
                    sb.Append(" </tr> ");
                    sb.Append(" <tr> ");
                    sb.Append(" <td style='text-align:center;'> <b> निर्वाचन हेतु निर्वाचन कार्यक्रम </b> </td> ");
                    sb.Append(" </tr> ");
                    sb.Append(" <tr> ");
                    sb.Append("<td style='padding-top:15px;'>");
                    sb.Append("<table id='content'  border='1'  cellpadding='5' cellspacing='0'  style='width:100%;'>");
                    //sb.Append("<table class='table table-responsive devtable ' style='border:1px solid #000;width:100%;font-family: Kruti Dev 010'>");
                    sb.Append("<thead>  <tr> <th style='text-align:left;width:50px;'>  क्रमांक  </th><th style='text-align:left;'> विवरण </th>");

                    for (int i = 0; i < _stepsInElection; i++)
                    {
                        sb.Append("<th style='text-align:left;width:90px;'> " + _ElectionStepKey[i] + " </th>");
                    }

                    sb.Append("</tr> </thead>");
                    sb.Append("<tbody>");
                    RowNo = 1;

                    foreach (DataRow row in ElectionDates.Rows)
                    {
                        sb.Append(" <tr> <td style='text-align:left;'> " + (RowNo < 9 ? RowNo.ToString() : "&nbsp;") + " </td><td style='text-align:left;'> " + row["QuestionHiWithDates"].ToString() + " </td>");

                        for (int i = 0; i < _stepsInElection; i++)
                        {
                            sb.Append("<td style='text-align:left;' > " + Convert.ToString(row[_ElectionStepValue[i]]) + " </td>");
                        }
                        sb.Append("</tr>");

                        RowNo++;
                    }


                    sb.Append("</tbody></table>");
                    sb.Append(" </td> ");
                    sb.Append(" </tr> ");

                    if (ElectionDates.Rows.Count > 11)
                    {
                        sb.Append(" <tr> ");
                        sb.Append(" <td style='text-align:justify'> <b> नोट :- संचालन मंडल के समस्त सदस्यों के निर्विरोध निर्वाचन की स्थिति में पदाधिकारियों का चुनाव  ");
                        DataRow drFrom = ElectionDates.Rows[7];
                        DataRow drTo = ElectionDates.Rows[12];
                        for (int j = 0; j < _stepsInElection; j++)
                        {
                            sb.Append(" " + _ElectionStepKey[j] + " में दिनांक " + Convert.ToString(drFrom[_ElectionStepValue[j]]) + " के स्थान पर दिनांक " + Convert.ToString(drTo[_ElectionStepValue[j]]) + " , ");
                        }

                        sb.Append(" को क्रम संख्या 8 में अंकित निर्वाचन कार्यक्रम अनुसार संपन्न करवाया जावेगा ।  </b> </td> ");
                        sb.Append(" </tr> ");
                        sb.Append(" <tr> ");
                        //sb.Append(" <td style='text-align:right'><b> राज्य सहकारी निर्वाचन प्राधिकारी  </b></td> ");
                        sb.Append(" </tr> ");
                    }

                    //bhupendra


                    sb.Append("</table></div><br /></div></body></html>");

                    string InpuData = sb.ToString();
                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>")));
                    if (COMMON_FUNCTION.GeneratePDF(sb1, FuilePath))
                    {
                        return _ElectionFor;
                    }
                    else
                    {
                        return 0;
                    }
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
                    //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                    //UserModel OBJUser = new UserModel(new FileLogger());
                    //// Why I am adding this to page footer ?? So we can get paging in footer section of each PDF page.how its working ?? A Javascript code is written inside the DLL which is handling Div's class i.e. page and topage.
                    //pdfDoc.PageFooterHtml = "<div style='float:right;'>  Page <span class='page'></span> of  <span class='topage'></span></div>";
                    //var pdfBytes = p1dfDoc.GeneratePdf(htmlContent);
                    //System.IO.File.WriteAllBytes(Server.MapPath("~/Content/ElectionOrder/HinOrderEO" + ElectionMasterId.ToString() + ".pdf"), pdfBytes);




                }
                else
                {
                    // StringBuilder sb = new StringBuilder();
                    sb.Append(" <html><body><div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;' ><center></center></div><br /><div id='divtoPrint'> ");
                    sb.Append(" <div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;font-size:23px; '> ");
                    sb.Append(" <table id='content' style='width:100%;'> ");
                    sb.Append(" <tr> ");
                    sb.Append("<td style='text-align:center'>");
                    sb.Append("<b><u style=''> राजस्थान सरकार </u></b>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:center;'> कार्यालय राजस्थान राज्य सहकारी निर्वाचन प्राधिकरण </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:center;'> 10 - बी संस्थागत क्षेत्र , रायसेम भवन , झालाना डूंगरी , जयपुर </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:center;'> फोन -0141-2712544 फैक्स-0141-2709018 </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:center;font-size:15px;'><b> Email ID scea@rajasthan.gov.in </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:center;'>");
                    sb.Append("<p style='text-align:right;font-size:15px;'> दिनांक " + DateTime.Now.ToString("dd/MM/yyyy") + " </p> ");
                    sb.Append("<p style=''> आदेश </p>");
                    sb.Append("<p style='text-align:justify;' > राजस्थान सहकारी सोसाइटी अधिनियम 2001 की धारा 33,35 एवं राजस्थान सहकारी सोसायटी ");

                    sb.Append(" नियम 2003 के नियम 45 व  46 के अंतर्गत प्रदत्त शक्तियों के अनुसरण परिशिष्ठ -1 में ");
                    sb.Append(" अंकित सहकारी संस्थाओं के संचालक मंडल के सदस्यों एवं पदाधिकारियों के निर्वाचन करवाए जाने हेतु ");
                    sb.Append(" निर्वाचन अधिकारी नियुक्त किया जाता है। निर्वाचन कार्यक्रम परिशिष्ठ-2 पर संलग्न है । </ p>");


                    sb.Append("<p style='text-align:justify;'> निर्वाचन अधिकारी द्वारा राजस्थान सहकारी सोसायटी अधिनियम 2001 (यथा संशोथित) , राजस्थान सोसायटी नियम 2003, (यथा संशोथित), संबंधित संस्थाओ के अद्यतन संशोधित पंजीकृत उपनियमों एवं राजस्थान राज्य सहकारी निर्वाचन प्राधिकरण द्वारा सहकारी संस्थाओ के निर्वाचन हेतु जारी सामान्य निर्देश क्रमांक फा .3(1) रा . रा . सह . निर्वा . प्राधि /16-35-43 दिनांक 20.02.2015 (http://rajsahakar.rajasthan.gov.in/)  वेबसाइट पर महत्वपूर्ण आदेशों में Election Folder  पर उपलब्ध है) की पालना करते हुए निर्वाचन सम्पन्न कराये जाएंगे </p>");
                    sb.Append("<p style='text-align:justify;'> यह आदेश आज दिनांक " + DateTime.Now.ToString("dd/MM/yyyy") + " को मेरे हस्ताक्षर एवं कार्यालय मुद्रा से अंकित कर जारी किया गया ।  </p>");

                    sb.Append("<p style='text-align:left;'> <b> संलग्न उपरोक्तानुसार </b> </p>");
                    //sb.Append("<p style='text-align:right;'> <b> राज्य सहकारी निर्वाचन प्राधिकारी  </b> </p>");
                    sb.Append("<p style='text-align:left;' > प्रतिलिपि निम्नलिखित को सूचनार्थ एवं आवश्यक कार्यवाही हेतु प्रेषित है :- </p> ");
                    sb.Append("<p style='text-align:left;' > 1. श्रीमान प्रमुख शासन सचिव, सहकारिता विभाग राजस्थान जयपुर ।  </p> ");
                    sb.Append("<p style='text-align:left;' > 2. श्रीमान रजिस्ट्रार, सहकारी समितियां, राजस्थान जयपुर । </p> ");
                    sb.Append(" <p style='text-align:left' > 3. जोनल रिटर्निग अधिकारी एवं अतिरिक्त रजिस्ट्रार, सहकारी समितियां, " + _divisionName + "  खंड  " + _divisionName + " </p> ");
                    sb.Append(" <p style='text-align:left' > 4. इकाई रिटर्निंग अधिकारी एवं उप रजिस्ट्रार , सहकारी समितियां , " + _districtName + " । </p> ");
                    sb.Append("<p style='text-align:left;' > 5. गार्ड पत्रावली </p> ");

                    //sb.Append("<p style='text-align:right;' > राज्य सहकारी निर्वाचन प्राधिकारी </p><br/>");
                    sb.Append("</td></tr>");




                    sb.Append(" <tr> ");
                    sb.Append(" <td style='text-align: right;color:black'><p style='page-break-before:always;'></p> परिशिष्ट -1 </td> ");
                    sb.Append(" </tr> ");
                    sb.Append(" <tr> ");
                    sb.Append(" <td style='text-align:center;'><u> राजस्थान सरकार </u></td> ");
                    sb.Append(" </tr> ");
                    sb.Append(" <tr> ");
                    sb.Append(" <td style='text-align:center;'> कार्यालय राजस्थान राज्य सहकारी निर्वाचन प्राधिकरण </td> ");
                    sb.Append(" </tr> ");
                    sb.Append(" <tr> ");
                    sb.Append(" <td style='text-align:center;'> 10 - बी संस्थागत क्षेत्र , रायसेम भवन , झालाना डूंगरी , जयपुर -302004 </td> ");
                    sb.Append(" </tr> ");
                    sb.Append(" <tr> ");
                    sb.Append(" <td style='text-align:center;'>  फ़ोन -0141-2712544 फैक्स -0141-2709018 </td> ");
                    sb.Append(" </tr> ");
                    sb.Append(" <tr> ");
                    sb.Append(" <td style='text-align:center;'><b> Email ID scea@rajasthan.gov.in  </b> </td> ");
                    sb.Append(" </tr> ");
                    sb.Append(" <tr> ");
                    sb.Append(" <td> ");
                    sb.Append("<table border='1'  cellpadding='5' cellspacing='0' style='width: 100%;'>");
                    sb.Append("<thead> <tr style='width: 100% ;'> <th style='text-align:left'> क्रमांक </th><th style='text-align:left'> सोसायटी </th><th style='text-align:left'> निर्वाचन अधिकारी  </th><th style='text-align:left'> चुनाव का चरण </th> </tr> </thead>");
                    sb.Append("<tbody>");
                    int RowNo = 1;
                    foreach (DataRow row in SocietyListOfOrderAll.Rows)
                    {
                        sb.Append("<tr style='width: 100% ;'> <td style='text-align:left'> " + RowNo.ToString() + " </td><td style='text-align:left'> " + row["SocietyName"].ToString() + " </td><td style='text-align:left'>  " + row["ElectionOfficerNameEng"].ToString() + " </td> <td style='text-align:left'> " + row["StepHin"].ToString() + " </td></tr>");
                        RowNo++;
                    }
                    sb.Append("</tbody></table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append(" <tr> ");
                    sb.Append(" <td style='text-align: right;color:black;page-break-before:always;'> परिशिष्ट -2 </td> ");
                    sb.Append(" </tr> ");
                    sb.Append(" <tr> ");
                    sb.Append(" <td style='text-align:center;'><u> राजस्थान सरकार </u></td> ");
                    sb.Append(" </tr> ");
                    sb.Append(" <tr> ");
                    sb.Append(" <td style='text-align:center;'> कार्यालय राजस्थान राज्य सहकारी निर्वाचन प्राधिकरण </td> ");
                    sb.Append(" </tr> ");
                    sb.Append(" <tr> ");
                    sb.Append(" <td style='text-align:center;'> 10 - बी संस्थागत क्षेत्र , रायसेम भवन , झालाना डूंगरी , जयपुर -302004 </td> ");
                    sb.Append(" </tr> ");
                    sb.Append(" <tr> ");
                    sb.Append(" <td style='text-align:center;'> फैक्स-0141-2709018 फ़ोन -0141-2712544 </td> ");
                    sb.Append(" </tr> ");
                    sb.Append(" <tr> ");
                    sb.Append(" <td style='text-align:center;'> <b> परिशिष्ठ -१ में अंकित सहकारी संस्थाओं के संचालन मंडल के सदस्यों एवं पदाधिकारियों के </b> </td> ");
                    sb.Append(" </tr> ");
                    sb.Append(" <tr> ");
                    sb.Append(" <td style='text-align:center;'> <b> निर्वाचन हेतु निर्वाचन कार्यक्रम </b> </td> ");
                    sb.Append(" </tr> ");
                    sb.Append(" <tr> ");
                    sb.Append("<td style='padding-top:15px;'>");
                    sb.Append("<table id='content'  border='1'  cellpadding='5' cellspacing='0'  style='width:100%;'>");
                    //sb.Append("<table class='table table-responsive devtable ' style='border:1px solid #000;width:100%;font-family: Kruti Dev 010'>");
                    sb.Append("<thead>  <tr> <th style='text-align:left;width:50px;'>  क्रमांक  </th><th style='text-align:left;'> विवरण </th>");

                    for (int i = 0; i < _stepsInElection; i++)
                    {
                        sb.Append("<th style='text-align:left;width:90px;'> " + _ElectionStepKey[i] + " </th>");
                    }

                    sb.Append("</tr> </thead>");
                    sb.Append("<tbody>");
                    RowNo = 1;

                    foreach (DataRow row in ElectionDates.Rows)
                    {
                        sb.Append(" <tr> <td style='text-align:left;'> " + (RowNo < 9 ? RowNo.ToString() : "&nbsp;") + " </td><td style='text-align:left;'> " + row["QuestionHiWithDates"].ToString() + " </td>");

                        for (int i = 0; i < _stepsInElection; i++)
                        {
                            sb.Append("<td style='text-align:left;' > " + Convert.ToString(row[_ElectionStepValue[i]]) + " </td>");
                        }
                        sb.Append("</tr>");

                        RowNo++;
                    }


                    sb.Append("</tbody></table>");
                    sb.Append(" </td> ");
                    sb.Append(" </tr> ");

                    if (ElectionDates.Rows.Count > 11)
                    {
                        sb.Append(" <tr> ");
                        sb.Append(" <td style='text-align:justify'> <b> नोट :- संचालन मंडल के समस्त सदस्यों के निर्विरोध निर्वाचन की स्थिति में पदाधिकारियों का चुनाव  ");
                        DataRow drFrom = ElectionDates.Rows[7];
                        DataRow drTo = ElectionDates.Rows[12];
                        for (int j = 0; j < _stepsInElection; j++)
                        {
                            sb.Append(" " + _ElectionStepKey[j] + " में दिनांक " + Convert.ToString(drFrom[_ElectionStepValue[j]]) + " के स्थान पर दिनांक " + Convert.ToString(drTo[_ElectionStepValue[j]]) + " , ");
                        }

                        sb.Append(" को क्रम संख्या 8 में अंकित निर्वाचन कार्यक्रम अनुसार संपन्न करवाया जावेगा ।  </b> </td> ");
                        sb.Append(" </tr> ");
                        sb.Append(" <tr> ");
                        //sb.Append(" <td style='text-align:right'><b> राज्य सहकारी निर्वाचन प्राधिकारी  </b></td> ");
                        sb.Append(" </tr> ");
                    }



                    sb.Append(" </table></div><br/></div></body></html> ");

                    string InpuData = sb.ToString();

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
                    //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                    //UserModel OBJUser = new UserModel(new FileLogger());
                    ////Why I am adding this to page footer?? So we can get paging in footer section of each PDF page.how its working?? A Javascript code is written inside the DLL which is handling  Div's class i.e. page and topage.
                    //pdfDoc.PageFooterHtml = "<div style='float:right;'>  Page <span class='page'></span> of  <span class='topage'></span></div>";
                    //bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);
                    //var pdfBytes = p1dfDoc.GeneratePdf(htmlContent);

                    ////System.IO.File.WriteAllBytes(System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraion.Replace("/", "") + "/") + result.ToString() + ".pdf"), pdfBytes);
                    ////System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraion.Replace("/", "") + "/BankOpeningCertificate.pdf");
                    ////Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);
                    //System.IO.File.WriteAllBytes(Server.MapPath("~/Content/ElectionOrder/HinOrderEO" + ElectionMasterId.ToString() + ".pdf"), pdfBytes);
                    //Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);
                    ////objElectionModel.updateElectionOrderPDF("Order" +  ElectionMasterId.ToString() +"Hin"+ ".pdf",  ElectionMasterId);
                    ////Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>")));
                    if (COMMON_FUNCTION.GeneratePDF(sb1, FuilePath))
                    {
                        return _ElectionFor;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else return 0;

        }


        public class ListSocietyElecOffi
        {
            public List<SocietyElectionOfficer> DataArray { get; set; }
        }

        #endregion
        #region Voter And Candidate
        // GET: Election
        public ActionResult Voters()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Voters, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }
        public ActionResult VotersElection()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.VotersElection, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }
        public ActionResult CandidateList()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.CandidateList, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }
        public ActionResult ElectedMemberList()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.ElectedMemberList, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }
        [HttpGet]
        public string GetNominationActions()
        {

            DataTable DT = new SocietyModel().GetLovValuesByListId(4);

            COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);

            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT); ;
        }


        public ActionResult FinalCandidateList()
        {
            return View();
        }

        public ActionResult ElectionResult()
        {
            return View();
        }

        [HttpPost]
        [ActionName("InsertElectionResult")]
        public string InsertElectionResult(SocietyVoterMemberEntity SocietyRegistrationData)
        {
            string flag = "-1";
            List<SocietyVoterMemberEntity> obj = new List<SocietyVoterMemberEntity>();
            ElectionModel objElectionModel = new ElectionModel(new DBLogger());
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<ElectionResultDataData> objData = new List<ElectionResultDataData>();
            try
            {
                objData = Request.Form["DataArray"].ToString().Equals("[]") ? new List<ElectionResultDataData>() : (List<ElectionResultDataData>)s.Deserialize(Request.Form["DataArray"].ToString(), typeof(List<ElectionResultDataData>));
            }

            catch (System.Exception ex)
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

                        if (files.ToString().Contains("ResultPDFFile"))
                        {
                            string strid = files.ToString().Replace("ResultPDFFile", "");
                            if (strid != "")
                            {
                                int idx = Convert.ToInt32(files.Split('_')[1]);
                                objData[idx - 1].ResultDocument = fileName;

                            }
                        }

                    }


                }
                catch (System.Exception ex)
                {
                }
            }

            flag = objElectionModel.InsertElectionResult(objData, SocietyRegistrationData.IsSubmit);
            if (SocietyRegistrationData.IsSubmit && SocietyRegistrationData.StepType == 7)
            {
                DataTable dt = new DataTable();
                dt = GetDTVoterMemberDetailsBySocietyId(objData[0].SocietyIdEnc, false, objData[0].ElectionMasterId, SocietyRegistrationData.StepType);


                if (dt.Rows.Count > 0)
                {
                    ElectionPDF(dt, "ElectedCandidateList_" + dt.Rows[0]["SocietyId"] + "_" + dt.Rows[0]["ElectionMasterId"], "निर्वाचित सदस्य सूची");
                }
            }
            else if (SocietyRegistrationData.IsSubmit && SocietyRegistrationData.StepType == 12)
            {


                DataTable dt = new DataTable();
                dt = GetDTVoterMemberDetailsBySocietyId(objData[0].SocietyIdEnc, false, objData[0].ElectionMasterId, SocietyRegistrationData.StepType);

                DataTable tblFiltered = dt;// dt.AsEnumerable().Where(row => row.Field<bool>("IsWinner")).CopyToDataTable();


                if (tblFiltered.Rows.Count > 0)
                {
                    ElectionPDF(tblFiltered, "ElectionResultList_" + dt.Rows[0]["SocietyId"] + "_" + dt.Rows[0]["ElectionMasterId"], "चुनाव परिणाम");

                }

            }




            return flag;
            // return CommonMessageFactory.Response(CommonMessageFactory.Message(flag.Length > 10 ? 1 : 0, (Request.Cookies[SessionIdentity.Culture]).Value));
        }



        public ActionResult PublishByRO()
        {
            // ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Election, (Request.Cookies[SessionIdentity.Culture]).Value);

            return View();
        }
        [HttpPost]
        [ActionName("GetElectionSocietyListData")]
        public string GetElectionSocietyListData(int StepType = 0)
        {
            bool IsCEO = ((UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin]).UserType.ToUpper() == "C";
            if (StepType == 0)
            {
                StepType = IsCEO ? 1 : 2;
            }

            ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
            DataTable DT = new DataTable();


            if (((UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin]).UserType.ToUpper() == "C")
            {
                DT = objSocietyModel.GetSocietyListForElection(StepType, "0");
            }
            else
            {
                DT = objSocietyModel.GetSocietyListForElection(StepType, ((UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin]).UserId.ToString());
                // DT = objSocietyModel.GetElectionSocietyListData(StepType);

            }
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);

        }


        [HttpPost]
        [ActionName("GetElectionSocietyListData_New")]
        public JsonResult GetElectionSocietyListData_New(DataPagingModel Details, int StepType = 0)
        {
            bool IsCEO = ((UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin]).UserType.ToUpper() == "C";
            if (StepType == 0)
            {
                StepType = IsCEO ? 1 : 2;
            }

            ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
            DataTable DT = new DataTable();


            if (((UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin]).UserType.ToUpper() == "C")
            {
                DT = objSocietyModel.GetSocietyListForElection_New(ref Details, StepType, "0");
            }
            else
            {
                DT = objSocietyModel.GetSocietyListForElection_New(ref Details, StepType, ((UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin]).UserId.ToString());
                // DT = objSocietyModel.GetElectionSocietyListData(StepType);

            }

            ViewBag.totalRecords = Details.TotalRecords;
            //Details.TotalRecords = Details.TotalRecords;
            ViewBag.DataPagingModel = Details;
            ViewBag.StartIndex = ((Details.PageNumber - 1) * Details.PageSize) + 1;

            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

            //return COMMON_FUNCTION.DataTableToJSONString(DT);
        }





        [HttpPost]
        [ActionName("InsertSocietyMemberDetails")]
        public string InsertSocietyMemberDetails(SocietyVoterMemberEntity SocietyRegistrationData)
        {
            string flag = "-1";
            List<SocietyVoterMemberEntity> obj = new List<SocietyVoterMemberEntity>();
            ElectionModel objElectionModel = new ElectionModel(new DBLogger());
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<VoterMemberData> objData = new List<VoterMemberData>();
            try
            {

                objData = Request.Form["DataArray"].ToString().Equals("[]") ? new List<VoterMemberData>() : (List<VoterMemberData>)s.Deserialize(Request.Form["DataArray"].ToString(), typeof(List<VoterMemberData>));
            }

            catch (System.Exception ex)
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
                    int k = 0;
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
                        //objData[i].Photo = fileName;
                        //if (files.ToString().Contains("MemberFile"))
                        //    objData[i].Photo = fileName;
                        //else if (files.ToString().Contains("MemberPDFFile"))
                        //    objData[k].MemberPDFForm = fileName;
                        if (files.ToString().Contains("MemberFile"))
                        {
                            string strid = files.ToString().Replace("MemberFile", "");
                            if (strid != "")
                            {
                                int idx = Convert.ToInt32(files.Split('_')[1]);
                                objData[idx - 1].Photo = fileName;
                                //foreach (var item in objData)
                                //{
                                //    string strROw = item.RowNO.Replace("lblElAdministrativeFile", "");
                                //    if (strROw.Trim() == strid.Trim())
                                //    {
                                //        int id;
                                //        int.TryParse(strid, out id);
                                //        item.Photo = fileName;

                                //    }
                                //}
                            }
                        }
                        else if (files.ToString().Contains("MemberPDFFile"))
                        {
                            string strid = files.ToString().Replace("MemberPDFFile", "");
                            if (strid != "")
                            {
                                int idx = Convert.ToInt32(files.Split('_')[1]);
                                if (files.ToString().Contains("Add"))
                                {
                                    objData[idx - 1].MemberPDFForm = fileName;
                                }
                                else if (files.ToString().Contains("Delete"))
                                {
                                    objData[idx - 1].DeleteDocument = fileName;
                                }

                                //foreach (var item in objData)
                                //{
                                //    string strROw = item.RowNO.Replace("lblElAdministrativeFile", "");
                                //    if (strROw.Trim() == strid.Trim())
                                //    {
                                //        int id;
                                //        int.TryParse(strid, out id);
                                //        item.MemberPDFForm = fileName;
                                //    }
                                //}
                            }
                        }
                        //if (files.ToString() == "OtherAdministrativeFile11")
                        //    objData[i].Photo = fileName;
                        if (files.ToString().Contains("MemberFile") || files.ToString().Contains("MemberTempFile"))
                            i++;
                        else if (files.ToString().Contains("MemberPDFFile") || files.ToString().Contains("MemberTempPDFFile"))
                            k++;

                    }


                }
                catch (System.Exception ex)
                {



                }
            }
            SocietyRegistrationData.IsPublish = ((UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin]).UserType.ToUpper() == "C" ? false : SocietyRegistrationData.IsSubmit;
            flag = objElectionModel.InsertSocietyMemberDetails(objData, SocietyRegistrationData.IsSubmit, SocietyRegistrationData.IsPublish);

            if (SocietyRegistrationData.IsSubmit)
            {
                DataTable dt = new DataTable();

                if (((UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin]).UserType.ToUpper() == "C")
                {
                    dt = GetDTVoterMemberDetailsBySocietyId(objData[0].SocietyIdEnc, true, objData[0].ElectionMasterId, 1);
                }
                else
                {
                    dt = GetDTVoterMemberDetailsBySocietyId(objData[0].SocietyIdEnc, false, objData[0].ElectionMasterId, 2);
                }

                if (dt.Rows.Count > 0)
                {
                    ElectionPDF(dt, "MemberList_" + dt.Rows[0]["SocietyId"] + "_" + dt.Rows[0]["ElectionMasterId"], "सदस्य सूची");
                }
            }
            // ElectionPDF(DataTable List, string FileName)
            return flag;
            // return CommonMessageFactory.Response(CommonMessageFactory.Message(flag.Length > 10 ? 1 : 0, (Request.Cookies[SessionIdentity.Culture]).Value));
        }


        [HttpPost]
        [ActionName("UpdateSocietyMemberDetails")]
        public string UpdateSocietyMemberDetails(SocietyVoterMemberEntity SocietyRegistrationData)
        {
            string flag = "-1";
            List<SocietyVoterMemberEntity> obj = new List<SocietyVoterMemberEntity>();
            ElectionModel objElectionModel = new ElectionModel(new DBLogger());
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<VoterMemberData> objData = new List<VoterMemberData>();

            try
            {
                objData = Request.Form["DataArray"].ToString().Equals("[]") ? new List<VoterMemberData>() : (List<VoterMemberData>)s.Deserialize(Request.Form["DataArray"].ToString(), typeof(List<VoterMemberData>));
            }

            catch (System.Exception ex)
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
                    int k = 0;
                    bool Flag = false;
                    int ForOther = 0;

                    string pathString = "";
                    if (SocietyRegistrationData.SocietyDirectory == "undefined" || SocietyRegistrationData.SocietyDirectory == null)
                    {
                        pathString = "~/Content/SocietyProfiling";
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
                        //objData[i].Photo = fileName;
                        if (files.ToString().Contains("MemberFile"))
                        {
                            string strid = files.ToString().Replace("MemberFile", "");
                            if (strid != "")
                            {

                            }
                        }
                        else if (files.ToString().Contains("MemberPDFFile"))
                        {
                            string strid = files.ToString().Replace("MemberPDFFile", "");
                            if (strid != "")
                            {

                                int idx = Convert.ToInt32(files.Split('_')[1]);

                                objData[idx - 1].CandidatePDFForm = fileName;

                            }
                        }
                        else if (files.ToString().Contains("MemberActionPDFFile"))
                        {
                            string strid = files.ToString().Replace("MemberActionPDFFile", "");
                            if (strid != "")
                            {

                                int idx = Convert.ToInt32(files.Split('_')[1]);

                                objData[idx - 1].ActionDocument = fileName;

                            }
                        }
                        else if (files.ToString().Contains("BODPDFForm"))
                        {
                            string strid = files.ToString().Replace("BODPDFFile", "");
                            if (strid != "")
                            {

                                int idx = Convert.ToInt32(files.Split('_')[1]);

                                objData[idx - 1].BODPDFForm = fileName;

                            }
                        }



                        //if (files.ToString() == "OtherAdministrativeFile11")
                        //    objData[i].Photo = fileName;
                        //if (files.ToString().Contains("MemberFile") || files.ToString().Contains("MemberTempFile"))
                        //    i++;
                        //else if (files.ToString().Contains("MemberPDFFile") || files.ToString().Contains("MemberTempPDFFile"))
                        //    k++;

                    }


                }
                catch (System.Exception ex)
                {



                }
            }

            flag = objElectionModel.UpdateSocietyMemberDetails(objData, SocietyRegistrationData.IsSubmit, SocietyRegistrationData.IsPublish);





            if (SocietyRegistrationData.IsPublish && SocietyRegistrationData.StepType == 5)
            {
                DataTable dt = new DataTable();
                dt = GetDTVoterMemberDetailsBySocietyId(objData[0].SocietyIdEnc, false, objData[0].ElectionMasterId, SocietyRegistrationData.StepType);

                DataTable tblFiltered = dt.AsEnumerable().Where(row => row.Field<string>("ActionName") == "Candidate").CopyToDataTable();

                if (tblFiltered.Rows.Count > 0)
                {
                    ElectionPDF(tblFiltered, "CandidateList_" + tblFiltered.Rows[0]["SocietyId"] + "_" + tblFiltered.Rows[0]["ElectionMasterId"], "उम्मीदवार सूची");
                }
            }
            else if (objData.Count > 0 && objData[0].IsPostSubmited && SocietyRegistrationData.StepType == 8)
            {
                DataTable dt = new DataTable();
                dt = GetDTVoterMemberDetailsBySocietyId(objData[0].SocietyIdEnc, false, objData[0].ElectionMasterId, SocietyRegistrationData.StepType);

                if (dt.Rows.Count > 0)
                {
                    ElectionPDF(dt, "ElectedCandidateList_" + dt.Rows[0]["SocietyId"] + "_" + dt.Rows[0]["ElectionMasterId"], "निर्वाचित सदस्य सूची");

                }
            }
            return flag;
            // return CommonMessageFactory.Response(CommonMessageFactory.Message(flag.Length > 10 ? 1 : 0, (Request.Cookies[SessionIdentity.Culture]).Value));
        }


        [HttpPost]
        [ActionName("InsertUpdateElecionPost")]
        public string InsertUpdateElecionPost(SocietyVoterMemberEntity SocietyRegistrationData)
        {
            string flag = "-1";
            List<SocietyVoterMemberEntity> obj = new List<SocietyVoterMemberEntity>();
            ElectionModel objElectionModel = new ElectionModel(new DBLogger());
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<VoterMemberData> objData = new List<VoterMemberData>();

            try
            {
                objData = Request.Form["DataArray"].ToString().Equals("[]") ? new List<VoterMemberData>() : (List<VoterMemberData>)s.Deserialize(Request.Form["DataArray"].ToString(), typeof(List<VoterMemberData>));
            }

            catch (System.Exception ex)
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
                    int k = 0;
                    bool Flag = false;
                    int ForOther = 0;

                    string pathString = "";
                    if (SocietyRegistrationData.SocietyDirectory == "undefined" || SocietyRegistrationData.SocietyDirectory == null)
                    {
                        pathString = "~/Content/SocietyProfiling";
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
                        //objData[i].Photo = fileName;
                        if (files.ToString().Contains("MemberFile"))
                        {
                            string strid = files.ToString().Replace("MemberFile", "");
                            if (strid != "")
                            {

                            }
                        }
                        else if (files.ToString().Contains("MemberPDFFile"))
                        {
                            string strid = files.ToString().Replace("MemberPDFFile", "");
                            if (strid != "")
                            {

                                int idx = Convert.ToInt32(files.Split('_')[1]);

                                objData[idx - 1].CandidatePDFForm = fileName;

                            }
                        }
                        else if (files.ToString().Contains("MemberActionPDFFile"))
                        {
                            string strid = files.ToString().Replace("MemberActionPDFFile", "");
                            if (strid != "")
                            {

                                int idx = Convert.ToInt32(files.Split('_')[1]);

                                objData[idx - 1].ActionDocument = fileName;

                            }
                        }
                        else if (files.ToString().Contains("BODPDFForm"))
                        {
                            string strid = files.ToString().Replace("BODPDFFile", "");
                            if (strid != "")
                            {

                                int idx = Convert.ToInt32(files.Split('_')[1]);

                                objData[idx - 1].BODPDFForm = fileName;

                            }
                        }



                        //if (files.ToString() == "OtherAdministrativeFile11")
                        //    objData[i].Photo = fileName;
                        //if (files.ToString().Contains("MemberFile") || files.ToString().Contains("MemberTempFile"))
                        //    i++;
                        //else if (files.ToString().Contains("MemberPDFFile") || files.ToString().Contains("MemberTempPDFFile"))
                        //    k++;

                    }


                }
                catch (System.Exception ex)
                {



                }
            }
            flag = objElectionModel.InsertUpdateElecionPost(objData, SocietyRegistrationData.IsSubmit, SocietyRegistrationData.IsPublish);
            if (SocietyRegistrationData.IsPublish && SocietyRegistrationData.StepType == 5)
            {
                DataTable dt = new DataTable();
                dt = GetDTVoterMemberDetailsBySocietyId(objData[0].SocietyIdEnc, false, objData[0].ElectionMasterId, SocietyRegistrationData.StepType);

                DataTable tblFiltered = dt.AsEnumerable().Where(row => row.Field<string>("ActionName") == "Candidate").CopyToDataTable();

                if (tblFiltered.Rows.Count > 0)
                {
                    ElectionPDF(tblFiltered, "CandidateList_" + tblFiltered.Rows[0]["SocietyId"] + "_" + tblFiltered.Rows[0]["ElectionMasterId"], "उम्मीदवार सूची");
                }
            }
            else if (objData.Count > 0 && objData[0].IsPostSubmited && SocietyRegistrationData.StepType == 11)
            {
                DataTable dt = new DataTable();
                dt = GetDTVoterMemberDetailsBySocietyId(objData[0].SocietyIdEnc, false, objData[0].ElectionMasterId, SocietyRegistrationData.StepType);

                if (dt.Rows.Count > 0)
                {
                    ElectionPDF(dt, "PostCandidateList_" + dt.Rows[0]["SocietyId"] + "_" + dt.Rows[0]["ElectionMasterId"], "पद उम्मीदवार सूची");

                }
            }
            else if (objData.Count > 0 && objData[0].IsPostSubmited && SocietyRegistrationData.StepType == 15)
            {
                DataTable dt = new DataTable();
                dt = GetDTVoterMemberDetailsBySocietyId(objData[0].SocietyIdEnc, false, objData[0].ElectionMasterId, SocietyRegistrationData.StepType);

                if (dt.Rows.Count > 0)
                {
                    ElectionPDF(dt, "ElectedCandidateList_" + dt.Rows[0]["SocietyId"] + "_" + dt.Rows[0]["ElectionMasterId"], "निर्वाचित सदस्य सूची");

                }
            }
            return flag;
            // return CommonMessageFactory.Response(CommonMessageFactory.Message(flag.Length > 10 ? 1 : 0, (Request.Cookies[SessionIdentity.Culture]).Value));
        }





        [HttpPost]
        [ActionName("UpdateSocietyElectedMemberDetails")]
        public string UpdateSocietyElectedMemberDetails(SocietyVoterMemberEntity SocietyRegistrationData)
        {
            string flag = "-1";
            List<SocietyVoterMemberEntity> obj = new List<SocietyVoterMemberEntity>();
            ElectionModel objElectionModel = new ElectionModel(new DBLogger());
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<VoterMemberData> objData = new List<VoterMemberData>();

            try
            {
                objData = Request.Form["DataArray"].ToString().Equals("[]") ? new List<VoterMemberData>() : (List<VoterMemberData>)s.Deserialize(Request.Form["DataArray"].ToString(), typeof(List<VoterMemberData>));
            }

            catch (System.Exception ex)
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
                    int k = 0;
                    bool Flag = false;
                    int ForOther = 0;

                    string pathString = "";
                    if (SocietyRegistrationData.SocietyDirectory == "undefined" || SocietyRegistrationData.SocietyDirectory == null)
                    {
                        pathString = "~/Content/SocietyProfiling";
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
                        //objData[i].Photo = fileName;
                        if (files.ToString().Contains("BODFile"))
                        {
                            string strid = files.ToString().Replace("BODFile", "");
                            if (strid != "")
                            {
                                foreach (var item in objData)
                                {
                                    string strROw = item.RowNO.Replace("lblElAdministrativeFile", "");
                                    if (strROw.Trim() == strid.Trim())
                                    {
                                        int id;
                                        int.TryParse(strid, out id);
                                        item.Photo = fileName;

                                    }
                                }
                            }
                        }
                        else if (files.ToString().Contains("BODPDFFile"))
                        {
                            string strid = files.ToString().Replace("BODPDFFile", "");
                            if (strid != "")
                            {
                                foreach (var item in objData)
                                {
                                    string strROw = item.RowNO.Replace("lblElAdministrativeFile", "");
                                    if (strROw.Trim() == strid.Trim())
                                    {
                                        int id;
                                        int.TryParse(strid, out id);
                                        item.BODPDFForm = fileName;
                                    }
                                }
                            }
                        }
                        //if (files.ToString() == "OtherAdministrativeFile11")
                        //    objData[i].Photo = fileName;
                        if (files.ToString().Contains("BODFile") || files.ToString().Contains("BODTempFile"))
                            i++;
                        else if (files.ToString().Contains("BODPDFFile") || files.ToString().Contains("BODTempPDFFile"))
                            k++;

                    }


                }
                catch (System.Exception ex)
                {



                }
            }

            flag = objElectionModel.UpdateSocietyElectedMemberDetails(objData);

            return flag;
            // return CommonMessageFactory.Response(CommonMessageFactory.Message(flag.Length > 10 ? 1 : 0, (Request.Cookies[SessionIdentity.Culture]).Value));
        }

        [HttpPost]
        [ActionName("InsertSocityVoterMemberByExcel")]
        public JsonResult InsertSocityVoterMemberByExcel(SocietyVoterMemberEntity _Model)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            string fileName = string.Empty;
            _Model.ExcelUrl = "";
            ResultMessage excelResult = new ResultMessage() { status = "-1" };
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            VoterMemberData objData = new VoterMemberData();
            try
            {

                objData = Request.Form["DataArray"].ToString().Equals("[]") ? new VoterMemberData() : (VoterMemberData)s.Deserialize(Request.Form["DataArray"].ToString(), typeof(VoterMemberData));
            }

            catch (System.Exception ex)
            {
                return Json(excelResult);
            }


            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    _Model.ExcelUrl = fileName;
                    string pathString = Path.Combine(Server.MapPath("~/Content/SocietyExcel"), fileName);
                    file.SaveAs(pathString);
                    DataTable memberData = new DataTable();
                    var result = ValidateExcel(ref memberData, pathString);
                    if (result != "OK")
                    {
                        excelResult.status = "-2";
                        excelResult.data = "ErrorInMemberdata@" + result;
                        return Json(excelResult);
                    }
                    else
                    {

                        ElectionModel objElectionModel = new ElectionModel(new DBLogger());
                        DataTable DT = new DataTable();
                        excelResult.status = objElectionModel.InsertElectionCandidateMemberByExcel(objData, memberData);

                        return Json(excelResult);

                    }

                }
                catch (System.Exception ex)
                {
                    excelResult.status = "-1";
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Election", FunctionName = "  ActionResult InsertSocityVoterMemberByExcel(ElectionSymbol objData)" }, ex);

                }
            }
            _Model.CreatedBy = _Model.UpdatedBy = int.Parse(_LoggedInUser.UserId.ToString());
            // _Model.CreatedIP = COMMON_FUNCTION.GetIpAddress();
            return Json(excelResult); ;
            //return CommonMessageFactory.Message(int.Parse(objSocietyModel.InsertSocityByExcel(_Model).ToString()), (Request.Cookies[SessionIdentity.Culture]).Value);
        }

        public string ValidateExcel(ref DataTable Data, string excelPath)
        {
            string result = "";
            try
            {
                string extension = Path.GetExtension(excelPath).ToLower();
                if (extension != ".xlsx" && extension != ".xls")
                {
                    result = "Please upload xlsx format only";
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

                    string[] excelColumns = new string[] { "Name", "FatherName", "ContactNo", "AadharNo", "DOB", "Gender", "Category", "Address" };
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

                            if (column.ColumnName.ToUpper() == "DOB" && Data.Rows[i][column.ColumnName] != null)
                            {
                                if (!new SocietyModel(new DBLogger()).IsValidDateTime(Data.Rows[i][column.ColumnName].ToString()))
                                {
                                    //result += "Row.No-" + (i + 1) + ", Columns ''" + column.ColumnName + "'' is empty or incorrect. Please fill correct value.<br/>";
                                    result += "Row.No-" + (i + 1) + ", Columns ''" + column.ColumnName + "'', Please enter Proper DOB '" + Data.Rows[i][column.ColumnName].ToString().Split(' ')[0] + "' in formate (dd/mm/yyyy) .<br/> on line No " + (i + 1).ToString();
                                }

                            }

                            if (Data.Rows[i][column.ColumnName] == null || Data.Rows[i][column.ColumnName].ToString().Replace(" ", "") == "")
                            {
                                result += "Row.No-" + (i + 1) + ", Columns ''" + column.ColumnName + "'' is empty or incorrect. Please fill correct value.<br/>";
                            }
                            int n; float b;

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

                            if (column.ColumnName == "AadharNo" && Data.Rows[i][column.ColumnName] != null && Data.Rows[i][column.ColumnName].ToString().Replace(" ", "") != "")
                            {
                                aadharck = Data.Rows[i][column.ColumnName].ToString().Replace(" ", "");
                                if (Data.AsEnumerable().Where(row => row.Field<object>("AadharNo") != null && row.Field<dynamic>("AadharNo").ToString().Replace(" ", "") == aadharck).CopyToDataTable().Rows.Count > 1)
                                {
                                    result += "Row.No-" + (i + 1) + ", Columns ''" + column.ColumnName + "=" + aadharck + "' is duplicate'. Please fill correct value.<br/>";
                                }
                                else if (aadharck.Length != 12)
                                {
                                    result += "Row.No-" + (i + 1) + ", Columns ''" + column.ColumnName + "=" + aadharck + "' invalid ''" + column.ColumnName + "''. Please fill correct value.<br/>";
                                }


                            }

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
            catch (System.Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        [HttpPost]
        [ActionName("GetVoterMemberDetailsBySocietyId")]
        public string GetVoterMemberDetailsBySocietyId(string SocietyId, bool IsCEO, int ElectionMasterId, int StepType = 0)
        {
            IsCEO = ((UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin]).UserType.ToUpper() == "C";
            if (StepType == 0)
            {
                StepType = IsCEO ? 1 : 2;
            }

            if (string.IsNullOrEmpty(SocietyId))
                return "{}";

            ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objSocietyModel.GetVoterMemberDetailsBySocietyId(SocietyId, IsCEO, ElectionMasterId, StepType);
            //DT = objSocietyModel.GetSocietyDetailsBySocietyId(SocietyId);
            foreach (DataRow dr in DT.Rows) // search whole table
            {
                if (dr["AadharNo"].ToString().Length > 12)
                {
                    dr["AadharNo"] = COMMON_FUNCTION.Decrypt(dr["AadharNo"].ToString());
                    dr["AadharNo2"] = dr["AadharNo"].ToString();
                    var AadharNo = dr["AadharNo"].ToString();

                    var firstDigits = AadharNo.Substring(0, 8);
                    var lastDigits = AadharNo.Substring(AadharNo.Length - 4, 4);
                    var requiredMask = new System.String('X', AadharNo.Length - lastDigits.Length);
                    var maskedString = string.Concat(requiredMask, lastDigits);
                    var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0");
                    dr["AadharNo"] = maskedString;
                }

            }
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        }

        [HttpPost]
        [ActionName("GetDTVoterMemberDetailsBySocietyId")]
        public DataTable GetDTVoterMemberDetailsBySocietyId(string SocietyId, bool IsCEO, int ElectionMasterId, int StepType = 0)
        {
            IsCEO = ((UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin]).UserType.ToUpper() == "C";
            if (StepType == 0)
            {
                StepType = IsCEO ? 1 : 2;
            }

            if (string.IsNullOrEmpty(SocietyId))
                return new DataTable();

            ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objSocietyModel.GetVoterMemberDetailsBySocietyId(SocietyId, IsCEO, ElectionMasterId, StepType);
            //DT = objSocietyModel.GetSocietyDetailsBySocietyId(SocietyId);
            foreach (DataRow dr in DT.Rows) // search whole table
            {
                if (dr["AadharNo"].ToString().Length > 12)
                {
                    dr["AadharNo"] = COMMON_FUNCTION.Decrypt(dr["AadharNo"].ToString());
                    dr["AadharNo2"] = dr["AadharNo"].ToString();
                    var AadharNo = dr["AadharNo"].ToString();

                    var firstDigits = AadharNo.Substring(0, 8);
                    var lastDigits = AadharNo.Substring(AadharNo.Length - 4, 4);
                    var requiredMask = new System.String('X', AadharNo.Length - lastDigits.Length);
                    var maskedString = string.Concat(requiredMask, lastDigits);
                    var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0");
                    dr["AadharNo"] = maskedString;
                }
            }
            return DT;
        }

        [HttpPost]
        public string SocietyListForElection()
        {

            ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objSocietyModel.GetSocietyListForElection(0, "0");
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        }


        [HttpPost]
        public string GetSocietyElectionDetail(string ElectionMasterIdEnc)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ElectionModel objElectionModel = new ElectionModel();
            int ElectionMasterId = 0;
            if (ElectionMasterIdEnc.ToString().Contains(" "))
            {
                ElectionMasterId = int.Parse(COMMON_FUNCTION.Decrypt(ElectionMasterIdEnc.ToString().Replace(' ', '+')));
            }
            else
            {
                ElectionMasterId = int.Parse(COMMON_FUNCTION.Decrypt(ElectionMasterIdEnc.ToString()));
            }
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objElectionModel.GetSocietyElectionDetail(ElectionMasterId));

        }
        [HttpPost]
        public string GetCommentListbySociety(int SocietyId)
        {

            ElectionModel objElectionModel = new ElectionModel();
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objElectionModel.GetCommentListbySociety(SocietyId));

        }


        [HttpGet]
        [ActionName("GenerateAadhaarOTPForEsing")]
        public string GenerateAadhaarOTPForEsing()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser != null)
            {
                //SSOWSSoap.SSOUserDetail usrdtl = new SSOWSSoap.SSOUserDetail();
                //SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
                //usrdtl = ssoclient.GetUserDetailXML(_LoggedInUser.SSOID, "RAJSAHKAR.TEST", "Rajasthan@12");
                //string aadhaarId = usrdtl.aadhaarId;
                if (Request.Url.Host.Contains("localhost"))
                {
                    _LoggedInUser.AadharNo = "312334102405";
                    //return COMMON_FUNCTION.GenerateOTPForEsingh("940396382715");
                }
                //else
                //{
                //    //aadhaarId = "940396382715";
                //    aadhaarId = "922487890238";
                //}

                return  COMMON_FUNCTION.GenerateOTPForEsingh(_LoggedInUser.AadharNo);
                //return COMMON_FUNCTION.GenerateOTPForEsingh("940396382715");
            }
            else
            {
                return "LOGOUT";
            }

            //return "RJVSD2343246566";
        }



        [HttpPost]
        public string GenPDFHinForElectionInfo(string Id, string OTP, string TXN) // DataTable SocietyListOfOrder, DataTable ElectionDates, 
        {

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            if (_LoggedInUser == null)
            {
                return "LOGOUT";
            }
            var otpResult =  COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
            if (otpResult.status != "1")
            {
                return otpResult.data;
            }

            ElectionModel objElectionModel = new ElectionModel();

            DataTable DT = new DataTable();
            DT = objElectionModel.GetSocietiesByelectionOfficerId(_loggedInUSerDetails.UserId.ToString());

            int ElectionMasterId = -1;
            int StepId = -1;
            int SocietyId = -1;
            string OfficerName = string.Empty;
            string SocietyName = string.Empty;
            if (DT != null && DT.Rows.Count > 0)
            {
                DataRow[] drs = DT.Select("Id=" + Id + "");


                if (drs != null && drs.Length > 0)
                {
                    ElectionMasterId = Convert.ToInt32(drs[0]["ElectionMasterId"]);
                    StepId = Convert.ToInt32(drs[0]["StepId"]);
                    SocietyId = Convert.ToInt32(drs[0]["SocietyId"]);
                    OfficerName = Convert.ToString(drs[0]["ElectionOfficerNameEng"]);
                    SocietyName = Convert.ToString(drs[0]["SocietyName"]);
                }
            }
            var ElectionDates = objElectionModel.ElectionPublishList(ElectionMasterId.ToString());
            string[] stepdates = new string[] { "Stap1Date", "Stap2Date", "Stap3Date", "Stap4Date", "Stap5Date", "Stap6Date", "Stap7Date", "Stap8Date", "Stap9Date", "Stap10Date" };

            string result = string.Empty;

            DataRow voterPublicationRow = ElectionDates.Select("ElectionQustionID=1")[0];
            DataRow electionRow = ElectionDates.Select("ElectionQustionID=6")[0];
            DataRow objectionRow = ElectionDates.Select("ElectionQustionID=2")[0];
            DataRow nominationRow = ElectionDates.Select("ElectionQustionID=3")[0];
            DataRow publicationRow = ElectionDates.Select("ElectionQustionID=4")[0];
            DataRow nominationWithdrawRow = ElectionDates.Select("ElectionQustionID=5")[0];
            DataRow reElectionRow = ElectionDates.Select("ElectionQustionID=6")[0];
            DataRow voteCountRow = ElectionDates.Select("ElectionQustionID=7")[0];
            DataRow officialElectionRow = ElectionDates.Select("ElectionQustionID=8")[0];


            // StringBuilder sb = new StringBuilder();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //sb.Append("<html><head><title></title><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head><body><div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;' ><center></center></div><br /><div id='divtoPrint'>");
            //sb.Append("<div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px; '>");
            //sb.Append("<table style='width: 100% ;font-size: 23px;font-weight: normal;' border='0' cellspacing='0px' cellpadding='0px'>");


            sb.Append(" <html><body><div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;' ><center></center></div><br /><div id='divtoPrint'> ");
            sb.Append(" <div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;font-size:20px; '> ");
            sb.Append(" <table id='content' style='width:100%;'> ");

            sb.Append(" <tr> ");
            sb.Append(" <td style='text-align:center'> ");
            sb.Append(" <p><b> कार्यालय निर्वाचन अधिकारी </b></p> ");
            sb.Append(" <p><b> " + SocietyName + " सहकारी समिति </b></p> ");
            sb.Append(" <p><b> निर्वाचन की सुचना का प्रारूप </b></p> ");
            sb.Append(" <p><b> नियम 45  (5) (iii) </b></p> ");
            sb.Append(" <p style='text-align:right'> दिनांक : " + DateTime.Now.ToString("dd/MM/yyyy") + " </p> ");
            sb.Append(" <p style='text-align:left;'> " + SocietyName + " सहकारी समिति लिमिटेड की प्रबंधकारिणी के सदस्यों के निर्वाचन हेतु ");
            sb.Append(" दिनांक " + electionRow[stepdates[StepId - 1]] + "  को प्रातः " + electionRow["StartTime"] + " बजे से सांय " + electionRow["StartTime"] + " बजे तक ");
            sb.Append(" ………………………………….. पर मतदान होगा। अतः समस्त सदस्य सूचित रहे ।  </p> ");

            sb.Append(" <p style='text-align:left;'>&nbsp;&nbsp; प्रस्तावित मतदाता सूची का प्रकाशन समिति के कार्यालय एवं सभी शाखाओ पर दिनांक " + voterPublicationRow[stepdates[StepId - 1]] + " ");
            sb.Append(" को कर दिया गया है।  यदि इस सूची में किसी सदस्य को कोई एतराज हो तो वह अपनी ");
            sb.Append(" आपत्ति मय ठोस सबूत मुझे अथवा मेरे द्वारा अधिकृत व्यक्ति श्री " + OfficerName + " के समक्ष ");
            sb.Append(" समिति के कार्यालय में दिनांक " + objectionRow[stepdates[StepId - 1]] + " को दोपहर " + objectionRow["EndTime"] + " बजे तक प्रस्तुत ");
            sb.Append(" कर सकते है । जिनकी सुनवाई उसी दिन की जाकर मतदाताओं की सूचि का अंतिम प्रकाशन ");
            sb.Append(" उसी दिन कर दिया जाएगा । </p> ");

            sb.Append(" <p style='text-align:left;'>&nbsp;&nbsp; सोसायटी के संचालक मंडल में 12 (बारह) निर्वाचित सदस्य होंगे। परन्तु कोई सहकारी");
            sb.Append(" सोसायटी जो व्यक्तिगत सदस्यों से मिलकर बनी हो और जिसमे अनुसूचित जाती या ");
            sb.Append(" अनुसूचित जनजाति अथवा महिला वर्ग से सदस्य हो, एक स्थान अनुसूचित जातियों के लिए ");
            sb.Append(" एक स्थान अनुसूचित जनजातियों के लिए और दो स्थान महिलाओ के लिए आरक्षित होंगे । </p> ");


            sb.Append(" <p style='text-align:left;'>&nbsp;&nbsp; चुनाव लड़ने वाले उम्मीदवार को राजस्थान सहकारी सोसाइटी अधिनियम 2001, ");
            sb.Append(" राजस्थान सहकारी सोसाइटी नियम ,2003 एवं सोसाइटी के पंजीकृत उपनियमो में उल्लेखित ");
            sb.Append(" निर्योगिताओ से ग्रसित नहीं होना चाहिए । </p> ");

            sb.Append(" <p style='text-align:left;page-break-before:always;margin-top:20px;'><b> निर्वाचन कार्यक्रम - </b></p> ");

            sb.Append(" <p style='text-align:left;'>  1. नाम निर्देशन पत्र ………………………  स्थान पर निर्वाचन अधिकारी को ");
            sb.Append(" दिनांक " + nominationRow[stepdates[StepId - 1]] + " को प्रातः " + nominationRow["StartTime"] + " बजे से " + nominationRow["EndTime"] + " बजे तक स्वयं उम्मीदवार ");
            sb.Append(" अथवा उसके समर्थक अथवा उसके प्रस्तावक द्वारा निर्धारित प्रपत्र में दिया जायेगा । ");
            sb.Append(" तत्पश्चात  प्राप्त नामनिर्देशन पत्रों की सूची का प्रकाशन " + nominationRow["EndTime"] + " बजे तक किया ");
            sb.Append(" जाएगा ।  </p> ");

            sb.Append(" <p style='text-align:left;'> 2. नामनिर्देशन पत्रों की सूक्ष्म परीक्षा उसी स्थान पर दिनांक " + publicationRow[stepdates[StepId - 1]] + " को " + publicationRow["StartTime"] + " ");
            sb.Append(" से " + publicationRow["EndTime"] + " बजे की जाएगी। जिसमे उम्मीदवार या उसका प्रस्तावक अथवा ");
            sb.Append(" समर्थक उपस्थित रह सकता है। तत्पश्चात वैध नाम निर्देशन पत्रों की सूची का ");
            sb.Append(" प्रकाशन " + publicationRow["EndTime"] + " बजे तक किया जाएगा । </p> ");


            sb.Append(" <p style='text-align:left;'> 3. उम्मीदवारों के नाम वापसी लेने की सुचना निर्वाचन अधिकारी को " + nominationWithdrawRow[stepdates[StepId - 1]] + " " + nominationWithdrawRow["StartTime"] + " बजे ");
            sb.Append(" तक उसी दिनांक को स्वयं उम्मीदवार द्वारा उपस्थित होकर लिखित में दी जा सकेगी । ");
            sb.Append(" उम्मीदवार द्वारा प्रस्तुत नाम वापसी का आवेदन पत्र अंतिम होगा, जिसे उम्मीदवार द्वारा ");
            sb.Append(" वापस नहीं लिया जा सकेगा । </p> ");

            sb.Append(" <p style='text-align:left;'> 4. यदि आवश्यक हुआ तो मतदान दिनांक " + reElectionRow[stepdates[StepId - 1]] + " को प्रातः " + reElectionRow["StartTime"] + " बजे से " + reElectionRow["EndTime"] + " ");
            sb.Append(" बजे तक ऊपर उल्लेखित स्थान / स्थानों पर होगा तथा मतगणना दिनांक " + voteCountRow[stepdates[StepId - 1]] + " ");
            sb.Append(" को " + reElectionRow["EndTime"] + " बजे से निर्धारित स्थान पर होगी। निर्वाचन परिणाम की घोषणा ");
            sb.Append(" मतगणना के पश्चात की जाएगी । </p> ");

            sb.Append(" <p style='text-align:left;'> 5. सहकारी सोसाइटी के प्रबंधकारिणी के पदाधिकारियों का चुनाव दिनांक " + officialElectionRow[stepdates[StepId - 1]] + "  ");
            sb.Append(" स्थान …………………… पर निर्वाचन अधिकारी द्वारा कराया जाएगा । </p> ");

            sb.Append(" <p style='text-align:right;'> निर्वाचन अधिकारी </p> ");

            sb.Append(" <p style='text-align:left'><b> प्रतिलिपि सूचनार्थ एवं आवश्यक कार्यवाही हेतु - </b></p> ");
            sb.Append(" <p style='text-align:left' > 1. इकाई रिटर्निंग अधिकारी एवं उप / सहायक रजिस्ट्रार सहकारी समितियों ………………………………. </p> ");
            sb.Append(" <p style='text-align:left' > 2. समिति नोटिस बोर्ड । </p>");
            sb.Append(" <p style='text-align:left' > 3. ग्राम पंचायत या आम स्थल पर चस्पा । </p> ");
            sb.Append(" <p style='text-align:left' > 4. मुख्य कार्यकारी अधिकारी सम्बंधित सोसाइटी । </p>");
            sb.Append(" <p style='text-align:right' > निर्वाचन अधिकारी  </p> ");
            sb.Append(" </td></tr> ");



            sb.Append(" </table></div></div></body></html> ");

            string InpuData = sb.ToString();

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

            var htmlContent = InpuData;
            //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            //UserModel OBJUser = new UserModel(new FileLogger());
            //Why I am adding this to page footer?? So we can get paging in footer section of each PDF page.how its working?? A Javascript code is written inside the DLL which is handling  Div's class i.e. page and topage.
            // pdfDoc.PageFooterHtml = "<div style='float:right;'>  Page <span class='page'></span> of  <span class='topage'></span></div>";
            //bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);
            // var pdfBytes = p1dfDoc.GeneratePdf(htmlContent);

            //System.IO.File.WriteAllBytes(System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraion.Replace("/", "") + "/") + result.ToString() + ".pdf"), pdfBytes);
            //System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraion.Replace("/", "") + "/BankOpeningCertificate.pdf");
            //Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);
            string FileName = "ElectionOrderSociety_" + ElectionMasterId.ToString() + "_Step_" + StepId.ToString() + ".pdf";
            //System.IO.File.WriteAllBytes(Server.MapPath("~/Content/ElectionOrder/" + FileName), pdfBytes);
            //Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);
            //objElectionModel.updateElectionOrderForSocietyPDF(ElectionMasterId, SocietyId, FileName);
            //Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            string Result = "5t65465465654356465768";
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>")));
            if (COMMON_FUNCTION.GeneratePDF(sb1, Server.MapPath("~/Content/ElectionOrder/" + FileName)))
            {

                var result1 = COMMON_FUNCTION.EsinghPDF(otpResult.data, Server.MapPath("~/Content/ElectionOrder/" + FileName));
                result = result1.data;
                if (result == "Success")
                {

                    //objElectionModel.UpdateElectionMasterOrderPDFSign(ElectionMasterId.ToString(), true);
                    //objElectionModel.updateElectionOrderPDF(FileName, ElectionMasterId, 1);
                    objElectionModel.updateElectionOrderForSocietyPDF(ElectionMasterId, SocietyId, FileName);
                    var DT1 = objElectionModel.GetElectionEmailSMSDetails(31, 3, ElectionMasterId, SocietyId);//bhupendra
                    foreach (DataRow dr in DT1.Rows) // search whole table
                    {
                        new EsanchaarController().SendSocietySMS(dr["sms"].ToString().Replace("APPLICANTNAME", dr["OfficerNameEn"].ToString()), dr["MobileNo"].ToString());
                    }
                    return "Success";
                }
                else
                {
                    return result;
                }



                //return _ElectionFor;

            }
            else
            {
                Result = "PDF Not Generate";
            }
            return Result;
        }

        [HttpGet]
        public ActionResult ElectionProgrammeForSociety()
        {
            return View();
        }

        [HttpPost]
        public string GetElectionPrograameForSociety()
        {
            DataTable DT = new DataTable();
            ElectionModel objElectionModel = new ElectionModel();
            DT = objElectionModel.GetSocietiesByelectionOfficerId(_loggedInUSerDetails.UserId.ToString());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        }

        public ActionResult ElectionProgrammePublish(string Id)
        {
            ElectionModel objElectionModel = new ElectionModel();

            DataTable DT = new DataTable();
            DT = objElectionModel.GetSocietiesByelectionOfficerId(_loggedInUSerDetails.UserId.ToString());

            int ElectionMasterId = -1;

            if (DT != null && DT.Rows.Count > 0)
            {
                DataRow[] drs = DT.Select("Id=" + Id + "");


                if (drs != null && drs.Length > 0)
                {
                    ElectionMasterId = Convert.ToInt32(drs[0]["ElectionMasterId"]);

                }
            }

            ViewBag.ElectionMasterId = COMMON_FUNCTION.Encrypt(ElectionMasterId.ToString());
            return View();
        }

        [HttpPost]
        public string GetElectionProgrammeData(string Id)
        {
            ElectionModel objElectionModel = new ElectionModel();

            DataTable DT = new DataTable();
            DT = objElectionModel.GetSocietiesByelectionOfficerId(_loggedInUSerDetails.UserId.ToString());

            int ElectionMasterId = -1;
            int StepId = -1;
            string OfficerName = string.Empty;
            string SocietyName = string.Empty;
            if (DT != null && DT.Rows.Count > 0)
            {
                DataRow[] drs = DT.Select("Id=" + Id + "");


                if (drs != null && drs.Length > 0)
                {
                    ElectionMasterId = Convert.ToInt32(drs[0]["ElectionMasterId"]);
                    StepId = Convert.ToInt32(drs[0]["StepId"]);
                    OfficerName = Convert.ToString(drs[0]["ElectionOfficerNameEng"]);
                    SocietyName = Convert.ToString(drs[0]["SocietyName"]);
                }
            }
            var ElectionDates = objElectionModel.ElectionPublishList(ElectionMasterId.ToString());
            string[] stepdates = new string[] { "Stap1Date", "Stap2Date", "Stap3Date", "Stap4Date", "Stap5Date", "Stap6Date", "Stap7Date", "Stap8Date", "Stap9Date", "Stap10Date" };

            string result = string.Empty;

            DataRow voterPublicationRow = ElectionDates.Select("ElectionQustionID=1")[0];
            DataRow electionRow = ElectionDates.Select("ElectionQustionID=6")[0];
            DataRow objectionRow = ElectionDates.Select("ElectionQustionID=2")[0];
            DataRow nominationRow = ElectionDates.Select("ElectionQustionID=3")[0];
            DataRow publicationRow = ElectionDates.Select("ElectionQustionID=4")[0];
            DataRow nominationWithdrawRow = ElectionDates.Select("ElectionQustionID=5")[0];
            DataRow reElectionRow = ElectionDates.Select("ElectionQustionID=6")[0];
            DataRow voteCountRow = ElectionDates.Select("ElectionQustionID=7")[0];
            DataRow officialElectionRow = ElectionDates.Select("ElectionQustionID=8")[0];

            result = "{";

            result += "\"SocietyName\":\"" + SocietyName + "\",";
            result += "\"ElectionStartDate\":\"" + electionRow[stepdates[StepId - 1]] + "\",";
            result += "\"ElectionStartTime\":\"" + electionRow["StartTime"] + "\",";
            result += "\"ElectionEndDate\":\"" + electionRow["EndTime"] + "\",";
            result += "\"ElectionOfficerName\":\"" + OfficerName + "\",";

            result += "\"ObjectionDate\":\"" + objectionRow[stepdates[StepId - 1]] + "\",";
            result += "\"ObjectionTime\":\"" + objectionRow["EndTime"] + "\",";

            result += "\"NominationDate\":\"" + nominationRow[stepdates[StepId - 1]] + "\",";
            result += "\"NominationStartTime\":\"" + nominationRow["StartTime"] + "\",";
            result += "\"NominationEndTime\":\"" + nominationRow["EndTime"] + "\",";

            result += "\"PublicationDate\":\"" + publicationRow[stepdates[StepId - 1]] + "\",";
            result += "\"PublicationStartTime\":\"" + publicationRow["StartTime"] + "\",";
            result += "\"PublicationEndTime\":\"" + publicationRow["EndTime"] + "\",";

            result += "\"NominarionWithdrawDate\":\"" + nominationWithdrawRow[stepdates[StepId - 1]] + "\",";
            result += "\"NominarionWithdrawTime\":\"" + nominationWithdrawRow["StartTime"] + "\",";

            result += "\"ReElectionDate\":\"" + reElectionRow[stepdates[StepId - 1]] + "\",";
            result += "\"ReElectionStartTime\":\"" + reElectionRow["StartTime"] + "\",";
            result += "\"ReElectionEndTime\":\"" + reElectionRow["EndTime"] + "\",";

            result += "\"VotingCountDate\":\"" + voteCountRow[stepdates[StepId - 1]] + "\",";

            result += "\"OfficialElectionDate\":\"" + officialElectionRow[stepdates[StepId - 1]] + "\",";

            result += "\"VoterListPublicationDate\":\"" + voterPublicationRow[stepdates[StepId - 1]] + "\"";

            result += "}";


            return result;
        }

        #endregion

        [HttpGet]
        public string GetElectionDashboard()
        {

            ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objSocietyModel.GetElectionDashboard();
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        }
        [HttpGet]
        public string GetElectionOrdersForDashboard()
        {

            ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objSocietyModel.GetElectionOrdersForDashboard();
            //DT = objSocietyModel.GetSocietyDetailsBySocietyId(SocietyId);

            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        }

        [HttpGet]
        public string GetElectionSocietyDetails(int ElectionMasterId)
        {

            ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objSocietyModel.GetElectionSocietyDetails(ElectionMasterId);
            //DT = objSocietyModel.GetSocietyDetailsBySocietyId(SocietyId);

            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        }

        public void ElectionPDF(DataTable List, string FileName, string Titel)
        {
            try
            {

                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                sb.Append("<html><body><div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;' ><center></center></div><br /><div id='divtoPrint'>");
                sb.Append("<div style='text-align:center'>");
                sb.Append("<div class='table-responsive citizen-dash-head' id='tblCustomers' style=' padding-left:25px;padding-right:25px;font-size:20px;width:100%'>");
                sb.Append("<table id='content'  border='0'  cellpadding='0' cellspacing='0'  style='width:100%'>");
                sb.Append("<tr>");
                sb.Append("<th style=''><u> राजस्थान सरकार </u></th>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style=''> कार्यालय राजस्थान राज सहकारी निर्वाचन प्राधिकारी </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style=''> 10 - बी संस्थागत क्षेत्र ,रायसेम भवन ,झलना डूंगरी, जयपुर -302004</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style=''> फैक्स - 0141-2712544 फोन -0141-2709018 </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style=''> सहकारी संघो के संचालक मंडल के सदस्यों एवं पदाधिकारी के निर्वाचन हेतु निर्वाचन कार्यक्रम </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='text-align:left;padding-top:20px;'><br/><br/> क्रमांक फा. 3(1) रा.रा.सह.निर्वा.प्रधि / सामान्य / 2014/2305 </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='padding-top: 50px;'><br/>" + Titel + " ( " + List.Rows[0]["SocietyName"] + " )</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='padding-top: 15px;'>");
                sb.Append(" <br/><table id='content'  border='1'  cellpadding='5' cellspacing='0'  style='width:100%;'>");
                sb.Append("<thead>");
                sb.Append("<tr>");
                sb.Append("<th style=''>");
                sb.Append(" क्रमांक ");
                sb.Append("</th>");
                //sb.Append("<th style='width:50px'>");
                //sb.Append(" फोटो ");
                //sb.Append("</th>");
                sb.Append("<th style='text-align: left;'>");
                sb.Append(" नाम ");
                sb.Append("</th>");
                sb.Append("<th style='text-align: left;'> ");
                sb.Append("पिता / पति नाम ");
                sb.Append("</th> ");
                //sb.Append("<th style='text-align: left;'>");
                //sb.Append(" लिंग ");
                //sb.Append("</th>");
                //sb.Append("<th style=''> ");
                //sb.Append(" आधार ");
                //sb.Append("</th> ");
                //sb.Append("<th style=''> ");
                //sb.Append(" फोन नंबर ");
                //sb.Append("</th> ");
                //sb.Append("<th style=''> ");
                //sb.Append(" जन्म की तारीख ");
                //sb.Append("</th> ");
                sb.Append("<th style=''> ");
                sb.Append(" सदस्यता संख्या ");
                sb.Append("</th> ");
                sb.Append("<th style=''> ");
                sb.Append(" पता ");
                sb.Append("</th> ");





                if (Titel == "निर्वाचित सदस्य सूची")
                {
                    sb.Append("<th style=''> ");
                    sb.Append(" कुल वोट ");
                    sb.Append("</th> ");

                    sb.Append("<th style=''> ");
                    sb.Append(" परिणाम ");
                    sb.Append("</th> ");

                    sb.Append("<th> ");
                    sb.Append("");
                    sb.Append("</th> ");



                }
                if (Titel == "चुनाव परिणाम")
                {
                    sb.Append("<th style=''> ");
                    sb.Append(" पदनाम ");
                    sb.Append("</th> ");
                }
                if (Titel == "उम्मीदवार सूची" && List.Rows[0]["CandidateSymbol"] != null && List.Rows[0]["CandidateSymbol"].ToString() != "")
                {
                    sb.Append("<th style=''> ");
                    sb.Append(" चुनाव चिन्ह ");
                    sb.Append("</th> ");
                }
                else if (Titel == "पद उम्मीदवार सूची" && List.Rows[0]["OfficerSymbol"] != null && List.Rows[0]["OfficerSymbol"].ToString() != "")
                {
                    sb.Append("<th style=''> ");
                    sb.Append(" पद ");
                    sb.Append("</th> ");


                    sb.Append("<th style=''> ");
                    sb.Append(" चुनाव चिन्ह ");
                    sb.Append("</th> ");
                }
                sb.Append("</tr> ");
                sb.Append("</thead>");
                sb.Append("<tbody>");
                int index = 1;
                int ElectionMasterId = 0;
                int SocietyId = 0;
                foreach (DataRow dr in List.Rows) // search whole table
                {
                    //if (dr["AadharNo"].ToString().Length > 12)
                    //{
                    //    dr["AadharNo"] = COMMON_FUNCTION.Decrypt(dr["AadharNo"].ToString());
                    //    dr["AadharNo2"] = dr["AadharNo"].ToString();
                    //}
                    ElectionMasterId = Convert.ToInt32(dr["ElectionMasterId"].ToString());
                    SocietyId = Convert.ToInt32(dr["SocietyId"].ToString());
                    sb.Append("<tr>");
                    sb.Append("<td>" + (index++) + "</td> ");
                    //sb.Append("<td>");
                    //if (!string.IsNullOrWhiteSpace(dr["Photo"].ToString()))
                    //{
                    //    string path = Server.MapPath("~/Content/SocietyProfiling/" + dr["SocietyDirectory"].ToString() + "/" + dr["Photo"].ToString());

                    //    sb.Append("<img src='" + path + "' style='width:35px;' />");

                    //}       sb.Append("</td>");
                    sb.Append("<td style='text-align: left;'> ");
                    sb.Append("" + dr["Name"].ToString() + "");
                    sb.Append(" </td>");

                    sb.Append("<td style='text-align: left;'> ");
                    sb.Append("" + dr["FHName"].ToString() + "");
                    sb.Append("</td> ");
                    //sb.Append("<td style='text-align: left;'>");
                    //sb.Append("" + dr["Gender"].ToString() == "F" ? "Female" : "Male" + "");
                    //sb.Append("</td>");
                    //sb.Append("<td>");
                    //sb.Append("" + dr["AadharNo"].ToString() + "");
                    //sb.Append("</td>");
                    //sb.Append("<td>");
                    //sb.Append("" + dr["ContactNo"].ToString() + "");
                    //sb.Append("</td>");
                    //sb.Append("<td class='NewMemberdob'>");
                    //sb.Append("" + dr["DOB"].ToString() + "");
                    //sb.Append("</td>");
                    sb.Append("<td style=''> ");
                    sb.Append(" " + dr["MembershipNo"].ToString() + " ");
                    sb.Append("</td> ");
                    sb.Append("<td> ");
                    sb.Append("" + dr["Address"].ToString() + "");
                    sb.Append("</td> ");





                    if (Titel == "निर्वाचित सदस्य सूची")
                    {
                        sb.Append("<td> ");
                        sb.Append("" + dr["TotalVotes"].ToString() + "");
                        sb.Append("</td> ");


                        sb.Append("<td style=''> ");
                        sb.Append("" + (Convert.ToBoolean(dr["IsWinner"].ToString()) ? " विजेता " : " हार ") + "");
                        sb.Append("</td> ");


                        sb.Append("<td>");
                        sb.Append(" " + dr["ResultRemark"].ToString() + " ");
                        sb.Append("</td>");
                    }

                    if (Titel == "चुनाव परिणाम")
                    {

                        sb.Append("<td style=''> ");
                        sb.Append(" " + dr["Post_Hi"].ToString() + " ");
                        sb.Append("</td> ");
                    }


                    if (Titel == "उम्मीदवार सूची" && List.Rows[0]["CandidateSymbol"] != null && List.Rows[0]["CandidateSymbol"].ToString() != "")
                    {
                        string path = Server.MapPath("~/Content/ElectioSymbol/" + dr["CandidateSymbol"].ToString());
                        sb.Append("<td style=''> ");
                        sb.Append("<img src='" + path + "' style='width:50px;height:50px;' />");
                        sb.Append("</td> ");
                    }
                    else if (Titel == "पद उम्मीदवार सूची" && List.Rows[0]["OfficerSymbol"] != null && List.Rows[0]["OfficerSymbol"].ToString() != "")
                    {
                        sb.Append("<td style=''> ");
                        sb.Append(" " + dr["Post_Hi"].ToString() + " ");
                        sb.Append("</td> ");

                        string path = Server.MapPath("~/Content/ElectioSymbol/" + dr["OfficerSymbol"].ToString());
                        sb.Append("<td style=''> ");
                        sb.Append("<img src='" + path + "' style='width:50px;height:50px;' />");
                        sb.Append("</td> ");
                    }





                    sb.Append("</tr>");
                }
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</div></body></html>");

                if (System.IO.File.Exists(Server.MapPath("~/Content/ElectionOrder/" + FileName + ".pdf")))
                {
                    System.IO.File.Delete(Server.MapPath("~/Content/ElectionOrder/" + FileName + ".pdf"));
                }
                ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                DataTable DT = new DataTable();

                string fileType = Titel == "चुनाव परिणाम" ? "ElectionResultFile" : Titel == "निर्वाचित सदस्य सूची" ? "ElectedCandidate" : Titel == "उम्मीदवार सूची" ? "CandidateListFile" : Titel == "पद उम्मीदवार सूची" ? "PostCandidateFile" : "MemberListFile";

                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>")));
                if (COMMON_FUNCTION.GeneratePDF(sb1, Server.MapPath("~/Content/ElectionOrder/" + FileName + ".pdf")))
                {
                    objSocietyModel.UpdateElectionStatusFile("" + FileName + ".pdf", fileType, Convert.ToInt32(List.Rows[0]["SocietyId"].ToString()), Convert.ToInt32(List.Rows[0]["ElectionMasterId"].ToString()));
                    var DT1 = objSocietyModel.GetElectionEmailSMSDetails(32, 4, ElectionMasterId, SocietyId);
                    if (fileType == "MemberListFile")
                    {
                        foreach (DataRow dr in DT1.Rows) // search whole table
                        {
                            new EsanchaarController().SendSocietySMS(dr["sms"].ToString().Replace("APPLICANTNAME", dr["OfficerNameEn"].ToString()), dr["MobileNo"].ToString());
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Election", FunctionName = " ElectionPDF(DataTable List, string FileName, string Titel)" }, ex);
            }
        }

        [HttpPost]
        public string GenerateElectedCandidatePDF(string OTP, string TXN, string SocietyIdEnc, int ElectionMasterId, int MemberId, int StepType)
        {
            string result = "";
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return "LOGOUT";
                }


                var otpResult =  COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                if (otpResult.status != "1")
                {
                    return otpResult.data;
                }

                ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                DataTable DT = new DataTable();
                DT = objSocietyModel.GetVoterMemberDetailsBySocietyId(SocietyIdEnc, false, ElectionMasterId, StepType);
                DataTable tblFiltered = DT.AsEnumerable().Where(row => row.Field<int>("MemberId") == MemberId).CopyToDataTable();
                string pdfFileName = (StepType == 7 ? "" : "Officer") + "Certificate_" + ElectionMasterId.ToString() + "_" + MemberId + ".pdf";
                string filePath = Server.MapPath("~/Content/ElectionOrder/" + pdfFileName);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                int SocietyId = 0;
                foreach (DataRow dr in tblFiltered.Rows) // search whole table
                {
                    SocietyId = int.Parse(dr["SocietyId"].ToString());
                    //sb.Append(" <html> ");
                    //sb.Append(" <body> ");
                    //sb.Append(" <div style=''> <center> </center> </div> ");
                    //sb.Append(" <br /> <div id='divtoPrint'> ");
                    //sb.Append(" <div style='text-align:center'> ");
                    //sb.Append(" <div class='table-responsive citizen-dash-head' id='tblCustomers' style='text-align:center;font-size: 23px;border:1px solid black;padding-top:100px; padding-left:50px; padding-right:50px;'> ");
                    //sb.Append(" <table id='content' border='0' cellpadding='0' cellspacing='0' style='width:100%;padding:5px;'> ");
                    //sb.Append(" <tr> ");
                    //sb.Append(" <th colspan='2' style='text-align: center;padding-bottom: 12px;padding-top: 21px;'> कार्यालय निर्वाचन  अधिकारी </th> ");
                    //sb.Append(" </tr> ");
                    //sb.Append(" <tr><td colspan='2' style='text-align: center;padding-bottom: 12px;padding-top: 10px;padding-bottom: 50px;'> <br/> " + dr["SocietyName"].ToString() + " सहकारी  समिति  लिमिटेड </td></tr> ");
                    //sb.Append(" <tr> ");
                    //sb.Append(" <th colspan='2' style='text-align:center;padding-top:60px;'><br/><span style='padding: 15px 19px 15px 21px;background: black;color: #fff;font-size: 25px;'> निर्वाचन  का  प्रमाण पत्र </span></th> ");
                    //sb.Append(" </tr> ");
                    //sb.Append(" <tr> ");
                    //sb.Append(" <td colspan='2' style='padding-top:60px;'> ");
                    //sb.Append(" <br/><br/><p style='text-align:justify;padding-left:20px;padding-right:20px;'>   मैं... " + dr["ElectionOfficer"].ToString() + " (" + dr["SocietyName"].ToString() + ") .... सहकारी  समिति  लिमिटेड  के संचालन  मण्डल  के पदाधिकारियों के निर्वाचन के लिए ");
                    //sb.Append(" निर्वाचन  अधिकारी , एतद द्वारा  प्रमाणित  करता  हूँ  कि <br/> मैंने  दिनांक ... " + dr["ElectionDate"].ToString() + " ... को यह घोषित  कर दिया  है कि ");
                    //sb.Append(" श्री ... " + dr["Name"].ToString() + " ... सहकारी  समिति  लिमिटेड के संचालन  मण्डल ");
                    //sb.Append(" के  " + (StepType == 7 ? " सदस्य " : dr["Post_Hi"].ToString()) + "  के रूप में  निर्वाचित  हो गए हैं और इसके  साक्ष्य  स्वरूप  मैंने  उनको  यह  निर्वाचन  का  प्रमाण पत्र  जारी  किया  है | </p> ");
                    //sb.Append(" </td> ");
                    //sb.Append(" </tr> ");
                    //sb.Append(" <tr> ");
                    //sb.Append(" <td colspan='2'> ");
                    //sb.Append("<br/><br/> <p style='text-align:left;padding-left:20px;padding-top:20px'> स्थान ..................................  </p> ");
                    //sb.Append(" <p style='text-align:left;padding-left:20px;'> दिनांक ... " + DateTime.Now.ToString("dd/MM/yyyy") + " ... </p>");
                    //sb.Append(" </td> ");
                    //sb.Append(" </tr> ");
                    //sb.Append(" <tr><td></td><td style='text-align:right;padding-right:70px;padding-bottom:10px;'> निर्वाचन  अधिकारी </td></tr> ");
                    //sb.Append(" <tr><td></td><td style='text-align:right;padding-bottom:20px;padding-right:30px'> " + dr["SocietyName"].ToString() + "  सहकारी  समिति  लिमिटेड </td></tr> ");
                    //sb.Append(" </table> ");
                    //sb.Append(" </div> ");
                    //sb.Append(" </div> ");
                    //sb.Append(" </div> ");
                    //sb.Append(" </body> ");
                    //sb.Append(" </html> <p style='page-break-before:always;'></p> ");



                    string devFontSize = "17px";
                    string fontSize = "font-size: 13px;";
                    sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:50px'>");
                    sb.Append("<table style='width: 100%;border:1px solid;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px'>");
                    sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                    sb.Append("<img style='width:100px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                    sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:32px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय निर्वाचन  अधिकारी") + "</label> <br><br>  " + dr["SocietyName"].ToString() + " सहकारी  समिति  लिमिटेड  </b>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");
                    sb.Append("<b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("निर्वाचन का प्रमाण पत्र") + "</label></b>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
                    sb.Append("<table style='width:100%'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'><b> </b></td>");
                    sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> </td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'><p style='text-align:justify;'>  मैं  <b> " + dr["ElectionOfficer"].ToString() + " (" + dr["SocietyName"].ToString() + ") </b> सहकारी  समिति  लिमिटेड  के संचालन  मण्डल  के पदाधिकारियों के निर्वाचन के लिए  निर्वाचन  अधिकारी , एतद द्वारा  प्रमाणित  करता  हूँ  कि <br/> मैंने  दिनांक <b> " + dr["ElectionDate"].ToString() + " </b> को यह घोषित  कर दिया  है कि </p> ");
                    sb.Append("<p style='text-align:justify;'>  श्री <b> " + dr["Name"].ToString() + " </b>  सहकारी  समिति  लिमिटेड के संचालन  मण्डल  के  <b> " + (StepType == 7 ? " सदस्य " : dr["Post_Hi"].ToString()) + " </b>  के रूप में  निर्वाचित  हो गए हैं और इसके  साक्ष्य  स्वरूप  मैंने  उनको  यह  निर्वाचन  का  प्रमाण पत्र  जारी  किया  है। </p>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
                    sb.Append("<table style='width:100%'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'><b> स्थान </b>.................................. </td>");
                    sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> निर्वाचन  अधिकारी </td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'><b> दिनांक </b> " + DateTime.Now.ToString("dd/MM/yyyy") + " </td>");
                    sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> " + dr["SocietyName"].ToString() + "  सहकारी  समिति  लिमिटेड </td>");
                    sb.Append("</tr>");

                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'><br/><br/><br/><br/><br/><br/><br/></td></tr>");
                    sb.Append("</table>");
                    sb.Append("</div>");










                }
                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>")));
                if (COMMON_FUNCTION.GeneratePDF(sb1, filePath))
                {
                    //result = COMMON_FUNCTION.VerifyAadhaarOTPForEsingh(OTP, TXN, filePath, true, pdfFileName);
                    //var esignResult = COMMON_FUNCTION.EsinghPDF(otpResult.data, filePath );
                    var esignResult = COMMON_FUNCTION.EsinghPDF(otpResult.data, filePath, "", "Election officer", "350", "120", "150", "120");
                    result = esignResult.data;
                    if (result == "Success")
                    {
                        ElectionModel objElectionModel = new ElectionModel();
                        objElectionModel.updateElectionOrderPDF(pdfFileName, MemberId, StepType == 7 ? 5 : 6);
                        var DT1 = objElectionModel.GetElectionEmailSMSDetails(33, 4, ElectionMasterId, SocietyId);
                        foreach (DataRow dr in DT1.Rows) // search whole table
                        {
                            new EsanchaarController().SendSocietySMS(dr["sms"].ToString().Replace("APPLICANTNAME", dr["OfficerNameEn"].ToString()), dr["MobileNo"].ToString());
                        }
                    }
                }
                else
                {
                    result = "PDF Not Generate";
                }
            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Election", FunctionName = "GenerateElectedCandidatePDF(string OTP, string TXN, string SocietyIdEnc, int ElectionMasterId, int MemberId, int StepType)" }, ex);
                result = "PDF Not Generate";
            }
            return result;
        }





        public ActionResult ElectionSymbol()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ElectionSymbol(ElectionSymbol objData)
        {
            ElectionModel objElectionModel = new ElectionModel();

            try
            {
                string fileName = string.Empty;
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    try
                    {
                        string pathString = "";

                        pathString = "~/Content/ElectioSymbol/";
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
                            objData.symbol_url = fileName;
                        }

                    }
                    catch (System.Exception ex)
                    {
                        DBLogger objDBLogger = new DBLogger();
                        objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Election", FunctionName = "  ActionResult ElectionSymbol(ElectionSymbol objData)" }, ex);
                        return Json(new { Status = "-1" }, JsonRequestBehavior.AllowGet);
                    }
                }



                string result = objElectionModel.InsertUpdateElectionSymbol(objData);
                return Json(new { Status = result }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Election", FunctionName = "  ActionResult ElectionSymbol(ElectionSymbol objData)" }, ex);
                return Json(new { Status = "-1" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public string GetAllElectionSymbol(string WhereCase)
        {
            ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objSocietyModel.GetAllElectionSymbol(WhereCase);
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        }

        [HttpPost]
        public string GetCandidateElectionSymbol(int ElectionMasterId, int CandidateId, int Type)
        {
            ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objSocietyModel.GetCandidateElectionSymbol(ElectionMasterId, CandidateId, Type);
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        }

        [HttpPost]
        public string GetElectionForEdit(int ElectionMasterId)
        {
            ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objSocietyModel.GetElectionForEdit(ElectionMasterId);
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        }

        public ActionResult Members()
        {
            return View();
        }

        [HttpPost]
        [ActionName("InsertUpdateElectionMemberFile")]
        public JsonResult InsertUpdateElectionMemberFile(List<VoterMemberData> MemberData, int Status)
        {
            //string flag = "-1";
            ResultMessage result = new ResultMessage();
            List<SocietyVoterMemberEntity> obj = new List<SocietyVoterMemberEntity>();
            ElectionModel objElectionModel = new ElectionModel(new DBLogger());
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            //List<VoterMemberData> objData = new List<VoterMemberData>();
            try
            {

                MemberData = Request.Form["MemberData"].ToString().Equals("[]") ? new List<VoterMemberData>() : (List<VoterMemberData>)s.Deserialize(Request.Form["MemberData"].ToString(), typeof(List<VoterMemberData>));
            }
            catch (System.Exception ex)
            {
                result = new ResultMessage() { status = "-1", data = "Provided input data is not in correct format." };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            int StepType = 1;
            bool IsCEO = ((UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin]).UserType.ToUpper() == "C";
            StepType = IsCEO ? 1 : 2;

            if (Status == 1)
            {
                StepType += 1;
            }

            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {

                    string fileDirectory = Server.MapPath("~/Content/ElectionOrder");

                    if (!System.IO.Directory.Exists(fileDirectory))
                    {
                        System.IO.Directory.CreateDirectory(fileDirectory);
                    }
                    foreach (string files in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[files];
                        fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);

                        file.SaveAs(Path.Combine(fileDirectory, fileName));
                        MemberData[0].Photo = fileName;
                    }
                }
                catch (System.Exception ex)
                {

                }
            }
            result = objElectionModel.InsertUpdateElectionMemberFile(MemberData, Status, StepType);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ActionName("GetElectionMemberFile")]
        public JsonResult GetElectionMemberFile(string SocietyId, bool IsCEO, int ElectionMasterId, int StepType = 0)
        {
            IsCEO = ((UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin]).UserType.ToUpper() == "C";
            if (StepType == 0)
            {
                StepType = IsCEO ? 1 : 2;
            }



            ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
            // DataTable memberListDT = new DataTable();
            //memberListDT = objSocietyModel.GetElectionMemberFile(SocietyId, IsCEO, ElectionMasterId, StepType);
            //DT = objSocietyModel.GetSocietyDetailsBySocietyId(SocietyId);
            //DataTable dateDT = new DataTable();
            //dateDT = objSocietyModel.GetElectionMemberFileSubmitDate(SocietyId, IsCEO, ElectionMasterId, StepType);

            return Json(new { DateData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(objSocietyModel.GetElectionMemberFileSubmitDate(SocietyId, IsCEO, ElectionMasterId, StepType)), MemberList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(objSocietyModel.GetElectionMemberFile(SocietyId, IsCEO, ElectionMasterId, StepType)) }, JsonRequestBehavior.AllowGet);


        }


        [HttpPost]
        [ActionName("GetElectionCandidateDate")]
        public JsonResult GetElectionCandidateDate(string SocietyId, bool IsCEO, int ElectionMasterId, int StepType = 0)
        {
            IsCEO = ((UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin]).UserType.ToUpper() == "C";
            if (StepType == 0)
            {
                StepType = IsCEO ? 1 : 2;
            }



            ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
            // DataTable memberListDT = new DataTable();
            //memberListDT = objSocietyModel.GetElectionMemberFile(SocietyId, IsCEO, ElectionMasterId, StepType);
            //DT = objSocietyModel.GetSocietyDetailsBySocietyId(SocietyId);
            //DataTable dateDT = new DataTable();
            //dateDT = objSocietyModel.GetElectionMemberFileSubmitDate(SocietyId, IsCEO, ElectionMasterId, StepType);

            return Json(new { DateData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(objSocietyModel.GetElectionMemberFileSubmitDate(SocietyId, IsCEO, ElectionMasterId, 3)) }, JsonRequestBehavior.AllowGet);


        }



        [HttpPost]
        [ActionName("DeleteElectionMemberList")]
        public JsonResult DeleteElectionMemberList(VoterMemberData MemberData, int Status)
        {
            //string flag = "-1";
            ResultMessage result = new ResultMessage();
            List<SocietyVoterMemberEntity> obj = new List<SocietyVoterMemberEntity>();
            ElectionModel objElectionModel = new ElectionModel(new DBLogger());
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            //List<VoterMemberData> objData = new List<VoterMemberData>();
            try
            {

                MemberData = Request.Form["MemberData"].ToString().Equals("[]") ? new VoterMemberData() : (VoterMemberData)s.Deserialize(Request.Form["MemberData"].ToString(), typeof(VoterMemberData));
            }
            catch (System.Exception ex)
            {
                result = new ResultMessage() { status = "-1", data = "Provided input data is not in correct format." };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            int StepType = 1;
            bool IsCEO = ((UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin]).UserType.ToUpper() == "C";
            StepType = IsCEO ? 1 : 2;

            if (Status == 1)
            {
                StepType += 1;
            }

            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    int i = 0;
                    int k = 0;
                    string pathString = "";

                    string fileDirectory = Server.MapPath("~/Content/ElectionOrder");

                    if (!System.IO.Directory.Exists(fileDirectory))
                    {
                        System.IO.Directory.CreateDirectory(fileDirectory);
                    }
                    foreach (string files in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[files];
                        fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);

                        file.SaveAs(Path.Combine(fileDirectory, fileName));

                        if (files.ToString().Contains("MemberPDFFile"))
                        {
                            string strid = files.ToString().Replace("MemberPDFFile", "");
                            if (strid != "")
                            {
                                int idx = Convert.ToInt32(files.Split('_')[1]);
                                MemberData.DeleteDocument = fileName;
                            }
                        }
                        i++;
                    }


                }
                catch (System.Exception ex)
                {

                }
            }
            result = objElectionModel.DeleteElectionMemberList(MemberData.MemberId, MemberData.DeleteRemarks, MemberData.DeleteDocument);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ActionName("InsertElectionCandidateDetails")]
        public string InsertElectionCandidateDetails(SocietyVoterMemberEntity SocietyRegistrationData)
        {
            string flag = "-1";
            List<SocietyVoterMemberEntity> obj = new List<SocietyVoterMemberEntity>();
            ElectionModel objElectionModel = new ElectionModel(new DBLogger());
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<VoterMemberData> objData = new List<VoterMemberData>();
            try
            {

                objData = Request.Form["DataArray"].ToString().Equals("[]") ? new List<VoterMemberData>() : (List<VoterMemberData>)s.Deserialize(Request.Form["DataArray"].ToString(), typeof(List<VoterMemberData>));
            }

            catch (System.Exception ex)
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
                    int k = 0;

                    string pathString = Server.MapPath("~/Content/ElectionCandidate");

                    if (!System.IO.Directory.Exists(pathString))
                    {
                        System.IO.Directory.CreateDirectory(pathString);
                    }




                    foreach (string files in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[files];
                        fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                        if (System.IO.Directory.Exists(pathString))
                        {
                            file.SaveAs(Path.Combine(pathString, fileName));
                        }

                        if (files.ToString().Contains("MemberFile"))
                        {
                            string strid = files.ToString().Replace("MemberFile", "");
                            if (strid != "")
                            {
                                int idx = Convert.ToInt32(files.Split('_')[1]);
                                objData[idx - 1].Photo = fileName;
                            }
                        }
                        else if (files.ToString().Contains("MemberPDFFile"))
                        {
                            string strid = files.ToString().Replace("MemberPDFFile", "");
                            if (strid != "")
                            {
                                int idx = Convert.ToInt32(files.Split('_')[1]);

                                objData[idx - 1].MemberPDFForm = fileName;


                            }
                        }
                        if (files.ToString().Contains("MemberFile") || files.ToString().Contains("MemberTempFile"))
                            i++;
                        else if (files.ToString().Contains("MemberPDFFile") || files.ToString().Contains("MemberTempPDFFile"))
                            k++;

                    }


                }
                catch (System.Exception ex)
                {

                }
            }
            SocietyRegistrationData.IsPublish = ((UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin]).UserType.ToUpper() == "C" ? false : SocietyRegistrationData.IsSubmit;
            flag = objElectionModel.InsertElectionCandidateDetails(objData, SocietyRegistrationData.IsSubmit, SocietyRegistrationData.IsPublish);
            if (SocietyRegistrationData.IsSubmit)
            {
                DataTable dt = new DataTable();

                if (((UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin]).UserType.ToUpper() == "C")
                {
                    dt = GetDTVoterMemberDetailsBySocietyId(objData[0].SocietyIdEnc, true, objData[0].ElectionMasterId, 1);
                }
                else
                {
                    dt = GetDTVoterMemberDetailsBySocietyId(objData[0].SocietyIdEnc, false, objData[0].ElectionMasterId, 2);
                }

                if (dt.Rows.Count > 0)
                {
                    ElectionPDF(dt, "MemberList_" + dt.Rows[0]["SocietyId"] + "_" + dt.Rows[0]["ElectionMasterId"], "सदस्य सूची");
                }
            }
            return flag;
        }

        [HttpPost]
        [ActionName("DeleteElectionCandidate")]
        public JsonResult DeleteElectionCandidate(VoterMemberData CandidateData)
        {
            ElectionModel objElectionModel = new ElectionModel(new DBLogger());
            return Json(objElectionModel.DeleteElectionCandidate(CandidateData));
        }

        [HttpPost]
        [ActionName("InsertOfficerCommentBySociety")]
        public string InsertOfficerCommentBySociety(ElectionOfficerComment ElectionOfficerComment)
        {
            string flag = "-1";
            DataTable DT = new DataTable();

            ElectionModel objElectionModel = new ElectionModel(new DBLogger());
            DT = objElectionModel.InsertOfficerCommentBySociety(ElectionOfficerComment);

            return flag;
        }


        public ActionResult ElectionSocietyReport()
        {
            return View();
        }




        [HttpPost]
        public JsonResult GetElectionSocietyReportList(DataPagingModel Details, ElectionSocietyData objElectionSocietyData)
        {
            ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objSocietyModel.GetElectionSocietyReportList(ref Details, objElectionSocietyData);
            // return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadElectionData()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetElectionSocieties(int DistrictId, string SearchText)
        {
            ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objSocietyModel.GetElectionSocieties(DistrictId, SearchText);
            // return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetSocietiesForElectionUpdate(string SearchText)
        {
            ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objSocietyModel.GetSocietiesForElectionUpdate(SearchText);
            // return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSocietyDetailsWithMembers(string SocietyIdENC)
        {
            ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objSocietyModel.GetSocietyDetailsWithMembers(SocietyIdENC);

            if (DT.Rows.Count > 0)
            {                
                foreach (DataRow dr in DT.Rows) // search whole table
                {

                    try
                    {
                        if (dr["Aadh"] != null && string.IsNullOrWhiteSpace(dr["AadharNo"].ToString()) && dr["Aadh"].ToString().Length == 12)
                        {
                            dr["AadharNo"] = COMMON_FUNCTION.Encrypt(dr["Aadh"].ToString());
                        }
                    }
                    catch (Exception ex) { }


                    if (!string.IsNullOrEmpty(dr["AadharNo"].ToString()))
                    {
                        //dr["AadharNo"] = COMMON_FUNCTION.Decrypt(dr["AadharNo"].ToString());

                        if (dr["AadharNo"].ToString().Length > 12)
                        {
                            var aadth=COMMON_FUNCTION.Decrypt(dr["AadharNo"].ToString());
                            dr["AadharNo"] = aadth;
                            dr["Aadh"] = aadth;
                        }                       
                    }                   
                }
            }


            // return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ElectionofficerList()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Election, (Request.Cookies[SessionIdentity.Culture]).Value);

            return View();
        }


        [HttpPost]
        [ActionName("GetInspectorList")]
        public JsonResult GetInspectorList(DataPagingModel Details, int StepType = 0)
        {
            ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objSocietyModel.GetInspactorName(ref Details);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ActionName("SaveElectionUploaddata")]
        public string SaveSocietyProfilingNew(SocietyRegistrationEntity _Entity,string Transid="0")//, SocietyRegisteredOfficeDetailsEntity RegisteredOfficeDetailsEntity
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            _Entity.CreatedBy = _Entity.UpdatedBy = _LoggedInUser.UserId;
            string flag = "0";
            string fileName = string.Empty;
            string ReturnJSON = "";
            string pathString = "~/Content/NewRegistration/";
            DataTable memberData = new DataTable();
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            _Entity.lstSocietyWorkScope = new List<SocietyWorkScopeEntity>();// Request.Form["lstSocietyWorkScope"].ToString().Equals("[]") ? new List<SocietyWorkScopeEntity>() : (List<SocietyWorkScopeEntity>)s.Deserialize(Request.Form["lstSocietyWorkScope"].ToString(), typeof(List<SocietyWorkScopeEntity>));
            try
            {
                //_Entity.AddressDetails = Request.Form["AddressDetails"].ToString().Equals("[]") ? new SocietyRegisteredOfficeDetailsEntity() : (SocietyRegisteredOfficeDetailsEntity)s.Deserialize(Request.Form["AddressDetails"].ToString(), typeof(SocietyRegisteredOfficeDetailsEntity));
                _Entity.AddressDetails.IsSameCEOAddress = true;
                _Entity.IsNewRegistration = false;
                _Entity.SaveStatus = 0;
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

                //OBJnew.SSO_ID = _LoggedInUser.SSOID;
                //OBJnew.KIOSKCODE = _LoggedInUser.KIOSKCODE;
                //OBJnew.EMitraTransactionId = _Entity.AddressDetails.EMitraTransactionId;
                //_LoggedInUser.ServiceId = "0";
                //OBJnew.ServiceId = int.Parse(_LoggedInUser.ServiceId);
                //OBJnew.SubmitStatus = _Entity.SaveStatus;
                //OBJnew.UserType = _LoggedInUser.UserType;
                LstSocietyRegisteredOfficeDetails.Add(OBJnew);

                _Entity.IsSameCEOAddress = true;



                if (LstSocietyRegisteredOfficeDetails.Count > 0)
                    _Entity.SocietyRegisteredOfficeDetailsLst = LstSocietyRegisteredOfficeDetails;
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SocietyController", FunctionName = "SaveSocietyProfilingNew_AddressDetails#" + _LoggedInUser.SSOID }, ex);
                return CommonMessageFactory.Response("Provided input data is not in correct format.");
            }

            SocietyModel objSocietyModel1 = new SocietyModel(new DBLogger());

            try
            {
                _Entity.RegistrationNo = (_Entity.RegistrationNo == "undefined" || _Entity.RegistrationNo == null) ? "" : _Entity.RegistrationNo;

                if (_Entity.IsNewRegistration == false || _Entity.RegistrationNo.Length > 1)
                {
                    pathString = "~/Content/SocietyProfiling/";
                    pathString += Regex.Replace(_Entity.RegistrationNo.ToUpper().Trim(), "[^a-zA-Z0-9_]+", "");


                    if (_Entity.IsNewRegistration == false && !string.IsNullOrWhiteSpace(_Entity.OldRegistrationNo) && _Entity.RegistrationNo != _Entity.OldRegistrationNo)
                    {
                        flag = objSocietyModel1.ValidateOldSocietyIsExist(_Entity.SocietyIDEnc, _Entity.RegistrationNo);
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

                // if (Request.Files != null && Request.Files.Count > 0 || _Entity.SocietyIDEnc.Length > 15 || _Entity.SaveStatus == 0)
                // {
                // UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
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
                ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                flag = objSocietyModel.InsertSocietyProfileNew(_Entity);


                // }
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
                        // PayServicesForSingleForm(K[1], _Entity.SaveStatus.ToString(), "5530", K[0], _Entity.KIOSKCODE, _Entity.SocietyName, _Entity.RegistrationNo);

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
        public string GenerateElectionResultLetter(SocietyRegistrationEntity _Entity, string Transid = "0")//, SocietyRegisteredOfficeDetailsEntity RegisteredOfficeDetailsEntity
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            _Entity.CreatedBy = _Entity.UpdatedBy = _LoggedInUser.UserId;
            string flag = "0";
            string fileName = string.Empty;
            string ReturnJSON = "";
            string pathString = "~/Content/NewRegistration/";
            DataTable memberData = new DataTable();
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            _Entity.lstSocietyWorkScope = new List<SocietyWorkScopeEntity>();// Request.Form["lstSocietyWorkScope"].ToString().Equals("[]") ? new List<SocietyWorkScopeEntity>() : (List<SocietyWorkScopeEntity>)s.Deserialize(Request.Form["lstSocietyWorkScope"].ToString(), typeof(List<SocietyWorkScopeEntity>));
            try
            {
                //_Entity.AddressDetails = Request.Form["AddressDetails"].ToString().Equals("[]") ? new SocietyRegisteredOfficeDetailsEntity() : (SocietyRegisteredOfficeDetailsEntity)s.Deserialize(Request.Form["AddressDetails"].ToString(), typeof(SocietyRegisteredOfficeDetailsEntity));
                _Entity.AddressDetails.IsSameCEOAddress = true;
                _Entity.IsNewRegistration = false;
                _Entity.SaveStatus = 0;
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

                
                LstSocietyRegisteredOfficeDetails.Add(OBJnew);

                _Entity.IsSameCEOAddress = true;



                if (LstSocietyRegisteredOfficeDetails.Count > 0)
                    _Entity.SocietyRegisteredOfficeDetailsLst = LstSocietyRegisteredOfficeDetails;
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SocietyController", FunctionName = "SaveSocietyProfilingNew_AddressDetails#" + _LoggedInUser.SSOID }, ex);
                return CommonMessageFactory.Response("Provided input data is not in correct format.");
            }

            SocietyModel objSocietyModel1 = new SocietyModel(new DBLogger());

            try
            {
                _Entity.RegistrationNo = (_Entity.RegistrationNo == "undefined" || _Entity.RegistrationNo == null) ? "" : _Entity.RegistrationNo;

            
               

              
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
                ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                flag = objSocietyModel.InsertSocietyProfileNew(_Entity, Transid);
                
               
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
        public JsonResult AssignElectionOfficerRole(int UserId)
        {
            ElectionModel objSocietyModel = new ElectionModel(new DBLogger());     
            return Json(objSocietyModel.SetAssignElectionOfficerRole(UserId));
        }

        [HttpPost]       
        public string AddElectionOfficer(UserEntity _Model, List<RoleArray> RoleData)
        {
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            //List<RoleArray> RoleData = new List<RoleArray>();
            //try
            //{
            //    RoleData = Request.Form["RoleArray"].ToString().Equals("[]") ? new List<RoleArray>() : (List<RoleArray>)s.Deserialize(Request.Form["RoleArray"].ToString(), typeof(List<RoleArray>));
            //}
            //catch { return CommonMessageFactory.Response("Provided input data is not in correct format."); }

            _Model.CreatedBy = -1;
            _Model.CreatedIP = "";
            ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
            return CommonMessageFactory.Response(objSocietyModel.AddElectionOfficer(_Model, RoleData).ToString());
        }
        [HttpGet]    
        public string GetAllDesignation(string WhereCase)
        {
            DesignationLevelMappingModel objDesignationLevelMapping = new DesignationLevelMappingModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objDesignationLevelMapping.GetAllDesignationLevelMappings(WhereCase));
        }


        [HttpPost]
        public string UploadMemberExcel()
        {
            string fileName = string.Empty;
            string result = "-1";
            DataTable memberData = new DataTable();
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {

                    string fileDirectory = Server.MapPath("~/Content/SocietyExcel");
                   
                    if (!System.IO.Directory.Exists(fileDirectory))
                    {
                        System.IO.Directory.CreateDirectory(fileDirectory);
                    }
                    foreach (string files in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[files];
                        fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                        file.SaveAs(Path.Combine(fileDirectory, fileName));
                       
                        string pathString = Path.Combine(fileDirectory, fileName);
                     
                         result = ValidateMemberExcel(ref memberData, pathString);

                        //MemberData[0].Photo = fileName;
                    }
                }
                catch (System.Exception ex)
                {
                    result= "-1";
                }
            }
            if (result != "-1") {
                return COMMON_FUNCTION.DataTableToJSONWithJSONNet(memberData);
            } else {
                return result;
            }

        }

        public string ValidateMemberExcel(ref DataTable Data, string excelPath)
        {
            string result = "";
            try
            {
                string extension = Path.GetExtension(excelPath).ToLower();
                if (extension != ".xlsx" && extension != ".xls")
                {
                    result = "Please upload xlsx format only";
                }
                else
                if (System.IO.File.Exists(excelPath))
                {
                    Data = COMMON_FUNCTION.ReadExcelData(excelPath, "Sheet1");

                    result = "OK";

                }
                else
                {
                    result = "Execl File is not uploaded on server";
                }
            }
            catch (System.Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

    }
}

