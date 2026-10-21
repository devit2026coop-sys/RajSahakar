using Cooperative.Filters;
using Cooperative.Models;
using Newtonsoft.Json;

using System;
using System.Data;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using static Cooperative.Models.GopalCreditCardModel;
using static Cooperative.Models.CoopModel;
using System.Linq;
using System.Web.Script.Serialization;
using System.Runtime.InteropServices;

namespace Cooperative.Controllers
{
    [Error]
    [LogActionFilter]
    [CustomAuthentication]
    public class TrainningController : BaseController
    {


        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

        [HttpPost]
        [ActionName("TrainningList")]
        public string GetAllTrainning(TrainningModel objTrainningModel)
        {

            string WhereCluase = "";
            if (objTrainningModel.ChannelId != 0)
                WhereCluase += "and Channels.ChannelId=" + (objTrainningModel.ChannelId);
            if (objTrainningModel.OrganisationId != 0)
                WhereCluase += "and Organisations.OrganisationId=" + objTrainningModel.OrganisationId;
            if (objTrainningModel.TrainningId != 0)
                WhereCluase += "and Trainning.TrainningId=" + objTrainningModel.TrainningId;
            else
                objTrainningModel = new TrainningModel(new FileLogger());

            // JObject json = JObject.Parse(COMMON_FUNCTION.DataTableToJSONWithJSONNet(objTrainningModel.GetAllTrainning(WhereCluase)));

            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objTrainningModel.GetAllTrainning(WhereCluase));
        }



        [HttpPost]
        [ActionName("ApproveAttendanceTrainningList")]
        public string GetApproveAttendanceTrainningList(TrainningModel objTrainningModel)
        {

            string WhereCluase = "";
            if (objTrainningModel.ChannelId != 0)
                WhereCluase += "and Channels.ChannelId=" + (objTrainningModel.ChannelId);
            if (objTrainningModel.OrganisationId != 0)
                WhereCluase += "and Organisations.OrganisationId=" + objTrainningModel.OrganisationId;
            if (objTrainningModel.TrainningId != 0)
                WhereCluase += "and Trainning.TrainningId=" + objTrainningModel.TrainningId;
            else
                objTrainningModel = new TrainningModel(new FileLogger());


            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objTrainningModel.ApproveAttendanceTrainningList(WhereCluase));
        }





        [HttpGet]
        public ActionResult DeptTrainningAcceptance()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.DeptTrainningAcceptance, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }
        [HttpGet]
        public ActionResult Trainning()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Trainning, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        [HttpGet]
        public ActionResult TrainingRequestByCoop()
        {
            //ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Trainning, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        [HttpGet]
        public ActionResult ApproveAttendance()
        {
            return View();
        }

        [HttpGet]
        public ActionResult RicemDashBoard()
        {
            //ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Trainning, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        [HttpPost]
        [ActionName("SaveTrainning")]
        public JsonResult TrainningInsert(TrainningModel _Model)
        {
            int result = -1;
            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
            if (!ModelState.IsValid)
                return Json(JsonModelError.Get(ModelState.Values));

            _Model.CreatedBy = _Model.UpdatedBy = _LoggedInUser.UserId;
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();
            result = _Model.AddTrainning(_Model);

            string WhereClause = "";
            WhereClause = " and o.isactive in (0,1) and OrganisationId= " + _Model.TargetGroupId;
            OrganisationMasterModel objOrganisationmaster = new OrganisationMasterModel(new FileLogger());
            var OrgItem = objOrganisationmaster.GetAllOrganisations(WhereClause);
            string OrgNameHi = "";
            if (OrgItem.Rows.Count > 0)
            {
                OrgNameHi = OrgItem.Rows[0]["OrgFullName_Hi"].ToString();
            }
            if (result > 0)
            {

                StringBuilder sb = new StringBuilder();
                sb.Append("<html><body><div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;' ><center></center></div><br /><div id='divtoPrint'>");
                sb.Append("<div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px; font-size: 20px;'>");
                sb.Append("<table style='width: 100%;font-weight: normal;' border='0' cellspacing='0' cellpadding='0'>");
                sb.Append("<tr><td colspan='4' align='center' style='width: 100%;'><h3> राजस्थान सहकारी शिक्षा एवं प्रबंधन संस्थान (राइसम ). </h3></td></tr>");
                sb.Append("<tr><td colspan='4' align='center' style='width: 100%; font-weight: normal !important;'><h4> 10-बी ,झालाना  संस्थागत क्षेत्र ,झालाना डूंगरी ,जयपुर </h4></td></tr>");
                sb.Append("<tr><td colspan='4' align='center' style='width: 100%;'><h4> फ़ोन नं-2709827,2710072 फैक्स नं-91-0141-2710072 </h4></td></tr>");
                sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
                sb.Append("<tr><td colspan='3' align='left' style='width: 75%;'> क्रमांक फा . ............ </td><td colspan='1' align='left' style='width: 25%;'> दिनांक" + (DateTime.Now.ToString("dd/MM/yyyy")) + "</td></tr>");
                sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
                sb.Append("<tr><td colspan='4' align='left' style='width: 100%;'> प्रबंध निदेशक </td></tr>");
                //  sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>.................केन्द्रीय सहकारी बैंक</td></tr>");
                sb.Append("<tr><td colspan='4' align='left' style='width: 100%;'> " + OrgNameHi + " </td></tr>");
                sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
                sb.Append("<tr><td colspan='1' align='left' style='width: 25%'>&nbsp;</td><td colspan='3' align='left' style='width: 75%;'>&nbsp; विषय : " + _Model.ProgrammeNameHi + " पर  प्रस्तावित प्रशिक्षण  कार्यक्रम दिनांक " + (_Model.StartDate.ToString("dd/MM/yyyy")) + " से " + _Model.EndDate.ToString("dd/MM/yyyy") + " तक मनोनयन करने क्रम में </td></tr>");
                sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
                sb.Append("<tr><td colspan='4' align='left' style='width: 100%;'> महोदय, </td></tr>");
                sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");



                sb.Append("<tr><td colspan='4' align='left' style='width: 100%'><p style='text-align: justify;text-justify: inter-word;'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; " + _Model.HtmlContent + " </p></td></tr>");
                //sb.Append(" प्रशिक्षण कार्यक्रम दिनांक " + _Model.StartDate + " तक किया जा रहा है इस कार्यक्रम मे पैक्स व्यवस्थापको को प्रशिक्षण दिया जाना है | ");
                //sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
                //sb.Append("<tr><td colspan='4' align='left' style='width: 100%'><p style='text-align: justify;text-justify: inter-word;'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;  अतः आप अपने क्षेत्र मे कार्यरत पैक्स /लैम्पस  के २ या ३ व्यवस्थापको नामांकित कर नामांकित व्यवस्थापको का तथा मोबाइल न. राइसेम   ईमेल  भिजवाये | </td></tr>");
                //sb.Append("<tr><td colspan='4' align='left' style='width: 100%'><p style='text-align: justify;text-justify: inter-word;'> उन्हें ये भी निर्देशित की दिनांक को " + _Model.StartDate + "  प्रातः" + _Model.StartTime + "बजे राइसेम  मे अपनी उपस्थिति सुनिश्चित करें | </td></tr>");
                //sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
                //sb.Append("<tr><td colspan='4' align='left' style='width: 100%'><p style='text-align: justify;text-justify: inter-word;'>नोट - कृपया प्रशिक्षण कार्यक्रम मे नामांकित प्रशिक्षणार्थियों को उनके पहचान पत्र (वोटर आईडी /आधार कार्ड /पेन कार्ड /ड्राइविंग लाइसेंस/कर्मचारी | ");
                //sb.Append("पहचान पत्र  ) आदि लाने हेतु भी सूचित करें जिससे  प्रशिक्षणार्थियों  की उपस्थिति को संस्था के डेटाबेस सॉफ्टवेयर मे संकलित किया जा सके |</p></td></tr>");
                sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
                sb.Append("<tr><td colspan='3' align='left' style='width: 65%'>&nbsp; </td><td colspan='1' align='left' style='width: 35%;'> भवदीय </td></tr>");
                sb.Append("<tr><td colspan='3' align='left' style='width: 65%'>&nbsp; </td><td colspan='1' align='left' style='width: 35%;'> प्रबंध निदेशक </td></tr>");
                sb.Append("<tr><td colspan='3' align='left' style='width: 65%'>&nbsp; </td><td colspan='1' align='left' style='width: 35%;'> राजस्थान सहकारी शिक्षा एवं प्रबंधन संस्थान (राइसम), जयपुर </td></tr>");
                sb.Append("<tr><td colspan='3' align='left' style='width: 65%'>&nbsp; </td><td colspan='1' align='left' style='width: 35%;'> दिनांक " + DateTime.Now.ToString("dd/MM/yyyy") + "  </td></tr>");

                // sb.Append("<tr><td colspan='3' align='left' style='width: 75%'>क्रमांक फा. (...............) </td></tr>");
                //sb.Append("<tr><td colspan='3' align='left' style='width: 75%'>प्रतिलिपि सूचनार्थ एवं आवश्यक कार्यवाही हेतु :-</td></tr>");
                //sb.Append("<tr><td colspan='3' align='left' style='width: 75%'>1. रजिस्ट्रार ,महोदय सहकारी समिति राजस्थान जयपुर |  </td></tr>");
                //sb.Append("<tr><td colspan='3' align='left' style='width: 75%'>2. मुख्य महाप्रबंधक नाबार्ड क्षेत्रीय कार्यालय जयपुर |  </td></tr>");
                //sb.Append("<tr><td colspan='3' align='left' style='width: 75%'>3. अतरिक्त निदेशक / प्रशाशनिक अधिकारी/ लेखा शाखा /प्रशिक्षण शाखा/ छात्रावास अधीक्षक/राइसेम  ,जयपुर |  </td></tr>");
                //sb.Append("<tr><td colspan='3' align='left' style='width: 75%'>4. गार्ड पत्रावली |  </td></tr>");
                sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
                sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
                //sb.Append("<tr><td colspan='3' align='left' style='width: 75%'>&nbsp; </td><td colspan='1' align='left' style='width: 25%'>सत्र संयोजक </td></tr>");
                sb.Append("</table></div><br /></div></body></html>");
                // string  printContents = '<html><head><title></title><meta http-equiv="Content-Type" content="text/html; charset=utf-8" /></head><body><div style="padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;" ><center></center></div><br />' + sb + '</body></html>';

                //string InpuData = sb.ToString();
                //TrainningController obj = new TrainningController();
                //TrainningModel objmodal = new TrainningModel();
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
                ////Why I am adding this to page footer?? So we can get paging in footer section of each PDF page.how its working?? A Javascript code is written inside the DLL which is handling  Div's class i.e. page and topage.
                //pdfDoc.PageFooterHtml = "<div style='float:right;'>  Page <span class='page'></span> of  <span class='topage'></span></div>";
                //var pdfBytes = p1dfDoc.GeneratePdf(htmlContent);

                //System.IO.File.WriteAllBytes(Server.MapPath("../Content/Training/" + result.ToString() + ".pdf"), pdfBytes);




                StringBuilder sb1 = new StringBuilder();
                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>")));

                COMMON_FUNCTION.GeneratePDF(sb1, Server.MapPath("../Content/Training/" + result.ToString() + ".pdf"));
                //FileStream fs = new FileStream(Server.MapPath("~/Content/Training"), FileMode.OpenOrCreate);
                //fs.Write(pdfBytes, 0, pdfBytes.Length);
                //fs.Close();


            }

            return Json(result);


        }


        [HttpPost]
        [ActionName("SaveTrainningSelectUser")]
        public JsonResult InsertTraningUserList(TrainningModel _Model)
        {
            DataTable dtUser;
            int result = -1;
            string pdfid = "";
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();
            string UserIds = "";
            foreach (var item in _Model.lstTrainningUsers)
            {
                if (item.UserId != 0)
                {
                    UserIds += item.UserId.ToString();
                    UserIds += ",";
                }

            }
            if (UserIds.Length > 0)
            {
                UserIds = UserIds.Substring(0, UserIds.Length - 1);
            }
            result = _Model.InsertTrainningUsersList(_Model);
            DataTable OrgItem = _Model.GetAllTrainning("and Trainning.TrainningId=" + _Model.TrainningId);
            string OrgNameHi = "";
            string StrDate = "";
            string EndDate = "";
            if (OrgItem.Rows.Count > 0)
            {
                OrgNameHi = OrgItem.Rows[0]["OrgFullName_Hi"].ToString();
                StrDate = OrgItem.Rows[0]["StartDate"].ToString();
                EndDate = OrgItem.Rows[0]["EndDate"].ToString();
            }
            dtUser = _Model.GetUserNameList(UserIds);
            if (result > 0)
            {
                pdfid = _Model.TrainningId.ToString();
                int SNNO = 1;
                StringBuilder sb = new StringBuilder();
                sb.Append(" <html> ");
                sb.Append(" <body> ");
                sb.Append(" <div style=''> <center> </center> </div> ");
                sb.Append(" <br /> ");
                sb.Append(" <div style='text-align:center'> ");
                sb.Append(" <div class='table-responsive citizen-dash-head' id='tblCustomers' style='text-align:center;font-size: 23px;border:1px solid black;padding-top:100px; padding-left:20px; padding-right:20px;'> ");
                sb.Append(" <table id='content' border='0' cellpadding='0' cellspacing='0' style='width:100%;padding:5px;'> ");
                sb.Append("<tr><td colspan='6' align='center' style='width: 100%'><h3> " + OrgNameHi + " </h3></td></tr>");
                //sb.Append("<tr><td colspan='4' align='center' style='width: 100%; font-weight: normal !important;'><h4>10-बी ,झालाना  संस्थागत क्षेत्र ,झेलना डूंगरी ,जयपुर</h4></td></tr>");

                sb.Append("<tr><td colspan='4' align='left' style='text-align:left'> क्रमांक फा. (.................) </td><td colspan='2'  style='text-align:right'> दिनांक " + (DateTime.Now.ToString("dd/MM/yyyy")) + " </td></tr>");
                sb.Append("<tr><td colspan='4' align='left' style=''>&nbsp;</td></tr>");
                sb.Append("<tr><td colspan='6' align='center' style=''> आदेश </td></tr>");
                sb.Append("<tr><td colspan='4' align='left' style=''>&nbsp;</td></tr>");
                //sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
                //sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>प्रबंध निदेशक /</td></tr>");
                //sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>.................केन्द्रीय सहकारी बैंक</td></tr>");
                //sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>.................|</td></tr>");
                sb.Append("<tr><td colspan='4' align='left' style=''>&nbsp;</td></tr>");
                //sb.Append("<tr><td colspan='4' align='left' style='width: 25%'>&nbsp;</td>" +"<td colspan='4' align='left' style='width: 100%'>&nbsp;निर्देशानुसार राजस्थान राज्य सहकारी प्रबंध संस्थान (राईसेम ) जयपुर द्वारा दिनांक @ से @ तक विषय पर आयोजित प्रशिक्षण कार्यक्रम म नामांकित अधिकारियों /निरक्षको किया जाकर निर्देश है की उक्त प्रशिक्षण कार्यक्रम मे  भाग लेने हेतु आप अपनी उपस्थिति दिनांक @ को प्रातः # बजे  उपरोक्त संस्थान  में आवश्यक रूप से प्रस्तुत करें | उक्त अधिकारिओ /निरक्षको  के नियंत्रक अधिकारी आवश्यक रूप से इन्हे समय पर उक्त प्रशिक्षण कार्यक्रम मे पहुंचने के लिए मुक्त करें   </td></tr>");
                //sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
                sb.Append("<tr><td colspan='6' align='left' style=''><p style='text-align: justify;text-justify: inter-word;'> निर्देशानुसार राजस्थान राज्य सहकारी प्रबंध संस्थान (राईसेम) जयपुर द्वारा दिनांक " + StrDate + "  से " + EndDate + "  तक विषय पर आयोजित प्रशिक्षण कार्यक्रम नामांकित अधिकारियों /निरीक्षकों किया जाकर निर्देश है की उक्त प्रशिक्षण कार्यक्रम मे  भाग लेने हेतु आप अपनी उपस्थिति दिनांक " + StrDate + "  को प्रातः 7.30 बजे  उपरोक्त संस्थान  में आवश्यक रूप से प्रस्तुत करें | उक्त अधिकारिओ /निरक्षको  के नियंत्रक अधिकारी आवश्यक रूप से इन्हे समय पर उक्त प्रशिक्षण कार्यक्रम मे पहुंचने के लिए मुक्त करें </p> </td></tr>");
                sb.Append("<tr><td colspan='4' align='left' style=''>  </td></tr>");
                //sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
                //sb.Append("<tr><td colspan='4' align='left' style='width: 100%'><p style='text-align: justify;text-justify: inter-word;'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; " + _Model.HtmlContent + " ");
                sb.Append("<tr><td colspan='6'> <br/><table id='content' border='1' cellpadding='5' cellspacing='0' style='width:100%;padding:5px;'> ");
                sb.Append("<tr><th style='text-align:left;padding:8px;'><b> क्रम सं. </b></th>" +
                    "<th  style='text-align:left;padding:8px;'><b> नाम </b></th>" +
                    "<th  style='text-align:left;padding:8px;'><b> एसएसओ आईडी </b></th>" +
                    "<th  style='text-align:left;padding:8px;'><b> विभाग </b></th>" +
                    "<th  style='text-align:left;padding:8px;'><b> पदनाम </b><br/></th></tr>");
                foreach (DataRow dr in dtUser.Rows) // search whole table
                {


                    sb.Append("<tr> <td  style='text-align:left;padding:8px;'> " + SNNO + " </td>");
                    sb.Append("<td  style='text-align:left;padding:8px;'> " + dr["OfficerNameEn"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left;padding:8px;'> " + dr["SSO_ID"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left;padding:8px;'> " + dr["DEPTNAMEENGLISH"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:left;padding:8px;'> " + dr["DESIGNATIONDESCENGLISH"].ToString() + " <br/></td></tr>");
                    SNNO = SNNO + 1;
                }
                sb.Append("</table></td></tr>");

                //sb.Append("<th colspan='4' align='left' style='width: 100%'>SSOID</th>");
                //sb.Append("<th colspan='4' align='left' style='width: 100%'>Department</th>");
                //sb.Append("<th colspan='4' align='left' style='width: 100%'>Designation</th></tr>");


                //sb.Append(" प्रशिक्षण कार्यक्रम दिनांक " + _Model.StartDate + " तक किया जा रहा है इस कार्यक्रम मे पैक्स व्यवस्थापको को प्रशिक्षण दिया जाना है | ");
                //sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
                //sb.Append("<tr><td colspan='4' align='left' style='width: 100%'><p style='text-align: justify;text-justify: inter-word;'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;  अतः आप अपने क्षेत्र मे कार्यरत पैक्स /लैम्पस  के २ या ३ व्यवस्थापको नामांकित कर नामांकित व्यवस्थापको का तथा मोबाइल न. राइसेम   ईमेल  भिजवाये | </td></tr>");
                //sb.Append("<tr><td colspan='4' align='left' style='width: 100%'><p style='text-align: justify;text-justify: inter-word;'> उन्हें ये भी निर्देशित की दिनांक को " + _Model.StartDate + "  प्रातः" + _Model.StartTime + "बजे राइसेम  मे अपनी उपस्थिति सुनिश्चित करें | </td></tr>");
                //sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
                //sb.Append("<tr><td colspan='4' align='left' style='width: 100%'><p style='text-align: justify;text-justify: inter-word;'>नोट - कृपया प्रशिक्षण कार्यक्रम मे नामांकित प्रशिक्षणार्थियों को उनके पहचान पत्र (वोटर आईडी /आधार कार्ड /पेन कार्ड /ड्राइविंग लाइसेंस/कर्मचारी | ");
                //sb.Append("पहचान पत्र  ) आदि लाने हेतु भी सूचित करें जिससे  प्रशिक्षणार्थियों  की उपस्थिति को संस्था के डेटाबेस सॉफ्टवेयर मे संकलित किया जा सके |</p></td></tr>");
                //sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
                //sb.Append("<tr><td colspan='3' align='left' style='width: 75%'>&nbsp; </td><td colspan='1' align='left' style='width: 25%'>भवदीय  </td></tr>");
                //sb.Append("<tr><td colspan='3' align='left' style='width: 75%'>&nbsp; </td><td colspan='1' align='left' style='width: 25%'>................................ </td></tr>");
                sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
                sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
                sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
                sb.Append("<tr><td colspan='3' align='right' style=''>&nbsp; </td><td colspan='4' align='right' style='text-align:right'> प्रबंध निदेशक </td></tr>");

                //sb.Append("<tr><td colspan='3' align='left' style='width: 75%'>क्रमांक फा. () </td></tr>");
                //sb.Append("<tr><td colspan='3' align='left' style='width: 75%'>प्रतिलिपि सूचनार्थ एवं आवश्यक कार्यवाही हेतु :-</td></tr>");
                //sb.Append("<tr><td colspan='3' align='left' style='width: 75%'>1. रजिस्ट्रार ,महोदय सहकारी समिति राजस्थान जयपुर |  </td></tr>");
                //sb.Append("<tr><td colspan='3' align='left' style='width: 75%'>2. मुख्य महाप्रबंधक नाबार्ड क्षेत्रीय कार्यालय जयपुर |  </td></tr>");
                //sb.Append("<tr><td colspan='3' align='left' style='width: 75%'>3. अतरिक्त निदेशक / प्रशाशनिक अधिकारी/ लेखा शाखा /प्रशिक्षण शाखा/ छात्रावास अधीक्षक/राइसेम  ,जयपुर |  </td></tr>");
                //sb.Append("<tr><td colspan='3' align='left' style='width: 75%'>4. गार्ड पत्रावली |  </td></tr>");
                sb.Append("<tr><td colspan='4' align='left' style=''>&nbsp;</td></tr>");
                sb.Append("<tr><td colspan='4' align='left' style=''>&nbsp;</td></tr>");
                //sb.Append("<tr><td colspan='3' align='left' style='width: 75%'>&nbsp; </td><td colspan='1' align='left' style='width: 25%'>सत्र संयोजक </td></tr>");
                sb.Append("</table></div></div></body></html>");
                // string  printContents = '<html><head><title></title><meta http-equiv="Content-Type" content="text/html; charset=utf-8" /></head><body><div style="padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;" ><center></center></div><br />' + sb + '</body></html>';

                string InpuData = sb.ToString();
                TrainningController obj = new TrainningController();
                TrainningModel objmodal = new TrainningModel();
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
                //Why I am adding this to page footer?? So we can get paging in footer section of each PDF page.how its working?? A Javascript code is written inside the DLL which is handling  Div's class i.e. page and topage.
                //pdfDoc.PageFooterHtml = "<div style='float:right;'>  Page <span class='page'></span> of  <span class='topage'></span></div>";
                // var pdfBytes = p1dfDoc.GeneratePdf(htmlContent);

                // System.IO.File.WriteAllBytes(Server.MapPath("../Content/TrainingSelectUserList/" + pdfid.ToString() + ".pdf"), pdfBytes);
                StringBuilder sb1 = new StringBuilder();
                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>")));
                COMMON_FUNCTION.GeneratePDF(sb1, Server.MapPath("../Content/TrainingSelectUserList/" + pdfid.ToString() + ".pdf"));

                //FileStream fs = new FileStream(Server.MapPath("~/Content/Training"), FileMode.OpenOrCreate);
                //fs.Write(pdfBytes, 0, pdfBytes.Length);
                //fs.Close();


            }

            return Json(result);
        }

        [HttpPost]
        [ActionName("TrainningReject")]
        public JsonResult TrainningRejectByDepartment(TrainningModel _Model)
        {
            _Model.CreatedBy = _LoggedInUser.UserId;
            TrainningModel objTrainningModel = new TrainningModel(new FileLogger());
            return Json(objTrainningModel.TrainningRejectByDepartment(_Model));

        }

        [HttpPost]
        [ActionName("DeleteandstatusTrainning")]
        public JsonResult delete_record(int id, int Action)
        {
            Action = (Action == 0 ? 1 : (Action == 1 ? 0 : Action));
            TrainningModel objTrainningModel = new TrainningModel(new FileLogger());
            return Json(objTrainningModel.DeleteChangeStatusTrainning(id, Action), JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [ActionName("TrainningAcceptUserList")]
        public string GetAllTrainningAcceptUser(TrainningModel objTrainningModel)
        {
            string WhereCluase = "";

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int District = _LoggedInUser.DistrictId;

            if (objTrainningModel.OrganizationLevelsMappingID != 0)
            {
                WhereCluase += " and URD.OrganizationLevelsMappingID= " + (objTrainningModel.OrganizationLevelsMappingID == 0 ? 0 : objTrainningModel.OrganizationLevelsMappingID);
            }
            if (objTrainningModel.DesignationLevelMappingId != 0)
            {
                WhereCluase += " and URD.DesignationLevelMappingId= " + (objTrainningModel.DesignationLevelMappingId == 0 ? 0 : objTrainningModel.DesignationLevelMappingId);
            }

            else
            {
                WhereCluase = " and 1=1";
            }
            objTrainningModel = new TrainningModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objTrainningModel.GetAllAcceptUser(WhereCluase));
        }

        [HttpGet]
        public ActionResult TrainningFeedBacks()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.FeedBacks, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        [HttpGet]
        [ActionName("FeedBacksList")]
        public string GetAllFeedBack(string WhereClause)
        {
            WhereClause += " and TrainningUser.UserId=" + _LoggedInUser.UserId;
            TrainningFeedbacksModel objFeedBackModel = new TrainningFeedbacksModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objFeedBackModel.GetAllFeedback(WhereClause));
        }

        [HttpPost]
        [ActionName("SaveTrainningFeedBack")]
        public JsonResult SaveTrainningFeedBack(TrainningFeedbacksModel _Model)
        {
            _Model.UserId = Convert.ToInt32(_LoggedInUser.UserId);

            //ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
            //if (!ModelState.IsValid)
            //    return Json(JsonModelError.Get(ModelState.Values));

            _Model.CreatedBy = _Model.UpdatedBy = _LoggedInUser.UserId;
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();

            return Json(_Model.AddFeedback(_Model));
            // return Json(_Model);
        }

        [HttpPost]
        [ActionName("GetUserTrainningList")]
        public string GetUserTrainningList()
        {

            TrainningFeedbacksModel objFeedbacksModel = new TrainningFeedbacksModel(new DBLogger());
            DataTable dt = new DataTable();

            dt = objFeedbacksModel.GetUserTrainningList(Convert.ToInt32(_LoggedInUser.UserId));

            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpPost]
        [ActionName("GetTrainningListById")]
        public string GetTrainningListById(string Trainningid)
        {
            if (string.IsNullOrEmpty(Trainningid))
                return "{}";
            string WhereCluase = "and Trainning.TrainningId=" + Trainningid + "";


            TrainningModel objTrainningModel = new TrainningModel(new DBLogger());

            DataTable dt = new DataTable();

            return COMMON_FUNCTION.DataTableToJSONString(objTrainningModel.GetAllTrainning(WhereCluase));

            // return COMMON_FUNCTION.DataTableToJSONString(dt);

        }

        [HttpPost]
        [ActionName("GetTrainningListByDate")]
        public string GetTrainningListByDate()
        {

            string WhereCluase = "and Trainning.StartDate <= Convert(Date,GETDATE()) and Trainning.EndDate >= Convert(Date,GETDATE())";


            TrainningModel objTrainningModel = new TrainningModel(new DBLogger());

            DataTable dt = new DataTable();

            return COMMON_FUNCTION.DataTableToJSONString(objTrainningModel.GetAllTrainning(WhereCluase));

            // return COMMON_FUNCTION.DataTableToJSONString(dt);

        }

        [HttpGet]
        public ActionResult TrainingAttendance()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.TrainningAttendance, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        [HttpPost]
        [ActionName("TrainningAttendanceUserList")]
        public string GetAllTrainningAttendanceUser(TrainningModel objTrainningModel)
        {
            string WhereCluase = "";

            if (objTrainningModel.TrainningId != 0)
            {
                WhereCluase += " and TrainningUser.TrainningId= " + (objTrainningModel.TrainningId == 0 ? 0 : objTrainningModel.TrainningId);
            }

            else
            {
                WhereCluase = " and 1=1";
            }
            objTrainningModel = new TrainningModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objTrainningModel.GetAllAttendanceUser(WhereCluase));
        }

        [HttpPost]
        [ActionName("SaveTrainningAttendanceUser")]
        public JsonResult InsertTraningAttendanceUserList(TrainningModel _Modal)
        {
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            try
            {
                _Modal.lstTrainningAttendanceUsers = Request.Form["lstTrainningAttendanceUsers"].ToString().Equals("[]") ? new List<TrainningAttendanceUser>() : (List<TrainningAttendanceUser>)s.Deserialize(Request.Form["lstTrainningAttendanceUsers"].ToString(), typeof(List<TrainningAttendanceUser>));
                //if (!ModelState.IsValid)
                //    return Json(ModelState.Values);
            }
            catch (Exception ex)
            {
                //return CommonMessageFactory.Response("Provided input data is not in correct format.");
            }


            return Json(_Modal.InsertTraningAttendanceUsersList(_Modal));

        }


        [HttpPost]
        [ActionName("SaveTrainningRequest")]
        public JsonResult TrainningRequestInsert(TrainningModel _Model)
        {
            int result = -1;
            ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
            ModelState.Remove("StartDate"); ModelState.Remove("Enddate");
            ModelState.Remove("StartTime"); ModelState.Remove("EndTime");
            ModelState.Remove("ChannelId"); ModelState.Remove("Location");

            _Model.CreatedBy = _Model.UpdatedBy = _LoggedInUser.UserId;
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();
            result = _Model.AddTrainningRequest(_Model);
            return Json(result);


        }


        [HttpPost]
        [ActionName("GetCurrentOrganisation")]
        public string GetCurrentOrganisation()
        {
            TrainningModel _Model = new TrainningModel();
            int userid = Convert.ToInt32(_LoggedInUser.UserId);
            int RoleId = Convert.ToInt32(_LoggedInUser.RoleId);
            return COMMON_FUNCTION.DataTableToJSONString(_Model.GetOrganisationOfUser(RoleId, userid));
        }

        [HttpPost]
        [ActionName("GetAllTrainningByUser")]
        public string GetAllTrainningByUser()
        {

            string WhereCluase = "";
            WhereCluase += "and Trainning.CreatedBy= " + (_LoggedInUser.UserId.ToString());
            TrainningModel _Model = new TrainningModel();
            return COMMON_FUNCTION.DataTableToJSONString(_Model.GetAllTrainning(WhereCluase));
        }

        [HttpPost]
        [ActionName("GetTrainningDashBoardData")]
        public string GetTrainningDashBoardData()
        {

            string WhereCluase = "";
            TrainningModel _Model = new TrainningModel();
            return COMMON_FUNCTION.DataTableToJSONString(_Model.GetTrainningDashBoardData());
        }

        [HttpGet]
        public ActionResult ResponseOnDepartmentTrainingRequest()
        {
            return View();
        }

        [HttpPost]
        public string TrainigRequestByDepartment(int ChannelId, int OrganizationId, int TrainingId)
        {

            TrainningModel _Model = new TrainningModel();
            int _UserId = Convert.ToInt32(_LoggedInUser.UserId);
            int _RoleId = _LoggedInUser.RoleId;
            return COMMON_FUNCTION.DataTableToJSONString(_Model.TrainigRequestByDepartment(ChannelId, OrganizationId, TrainingId, _UserId, _RoleId));
        }

        [HttpPost]
        public JsonResult SaveRICEMResponseOnTrainingRequest(TrainningModel _Model)
        {
            int result = -1;

            _Model.UpdatedBy = Convert.ToInt32(_LoggedInUser.UserId);
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();

            _Model.CreatedBy = _Model.UpdatedBy = _LoggedInUser.UserId;
            _Model.CreatedIP = _Model.UpdatedIP = COMMON_FUNCTION.GetIpAddress();
            result = _Model.SaveRICEMResponseOnTrainingRequest(_Model);
            return Json(result);
        }
        public DataTable GetTrainningAttendanceUserByTraining(int TrainningId = 0)
        {
            string WhereCluase = "";
            var dt = new DataTable();
            dt.Clear();
            if (TrainningId != 0)
            {
                WhereCluase += " and TrainningUser.TrainningId= " + (TrainningId == 0 ? 0 : TrainningId);
            }

            else
            {
                WhereCluase = " and 1=1";
            }
            var objTrainningModel = new TrainningModel(new FileLogger());
            dt = objTrainningModel.GetAllAttendanceUser(WhereCluase);
            return dt;
        }


        #region eSign process 2.1 on 27-07-2026
        [HttpPost]
        [ActionName("GenerateDocumentAttendance")]
        public JsonResult GenerateDocumentAttendance(int TrainingId = 0)
        {
            var responseModel = new PacsResponseModel();
            TrainningModel _Modal = new TrainningModel();
            try
            {
                var resultData = TrainingAttendanceFormList(TrainingId);
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
                    ViewPath = ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")) ? "http://localhost:27466/Content/Training/" + responseModel.DocName : "https://rajsahakarapp.rajasthan.gov.in/Content/Training/" + responseModel.DocName

                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.TrainningController", FunctionName = "GenerateDocumentAttendance(TrainningModel _Modal)" }, ex);
                return Json(new
                {
                    Success = false,
                    Result = 0,
                    Message = "An error occurred while generating the attendance document.",
                    Error = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [ActionName("TrainingAttendanceFormList")]
        public string TrainingAttendanceFormList(int TrainingId = 0)
        {
            DataTable resultData = new DataTable();
            DataTable dt = new DataTable();
            TrainningModel _Modal = new TrainningModel();

            string fileName = "";
            string folderPath = "";
            string filePath = "";

            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] == null)
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

                string wkhtmlPath = Server.MapPath("~/bin/wkhtmltopdf.exe");
                string fontPath = new Uri(Server.MapPath("~/fonts/lohit-Devanagari.ttf")).AbsoluteUri;

                dt = _Modal.GetProgramNameByTrainingId(TrainingId);
                string programTraningName = "";
                string trainingStartDate = "";
                string trainingEndDate = "";

                if (dt != null && dt.Rows.Count > 0)
                {
                    programTraningName = dt.Rows[0]["ProgrammeNameEn"]?.ToString() ?? "";
                    trainingStartDate = dt.Rows[0]["StartDate"]?.ToString() ?? "";
                    trainingEndDate = dt.Rows[0]["EndDate"]?.ToString() ?? "";
                    fileName = dt.Rows[0]["PDFPath"]?.ToString() ?? "";
                }

                folderPath = Server.MapPath("~/Content/Training/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    filePath = Path.Combine(folderPath, fileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        return JsonConvert.SerializeObject(new
                        {
                            Success = true,
                            DocPath = filePath,
                            DocType = "Trainning Certificate",
                            DocName = fileName,
                            ReturnPath = Url.Action("TrainingAttendance", "Trainning"),
                            Location = "",
                            Designation = ""
                        });
                    }
                }

                resultData = _Modal.TrainningAttendanceUsersList(TrainingId);
                StringBuilder traineeRows = new StringBuilder();
                int srNo = 1;

                var attendanceList = resultData.AsEnumerable().Where(r => r.Field<bool>("IsTraining") == false).ToList();
                foreach (DataRow row in attendanceList)
                {
                    string traineeName = row["OfficerNameEn"]?.ToString() ?? "";
                    string designation = row["DESIGNATIONDESCENGLISH"]?.ToString() ?? "";
                    string attendanceDate = row["TrainningDate"]?.ToString() ?? "";
                    string isPresent = (row["PresentStatus"]?.ToString() ?? "") == "Yes" ? "Present" : "Absent";
                    string statusColor = isPresent == "Absent" ? "color:red;font-weight:bold;" : "color:green;font-weight:bold;";
                    traineeRows.Append($@"
                <tr>
                    <td style='border:1px solid #2c2c2c;'>{srNo}</td>
                    <td style='border:1px solid #2c2c2c;'>{attendanceDate}</td>
                    <td style='border:1px solid #2c2c2c;'>{traineeName}</td>
                    <td style='border:1px solid #2c2c2c;'>{designation}</td>
                    <td style='border:1px solid #2c2c2c;{statusColor}'>{isPresent}</td>
                </tr>");

                    srNo++;
                }

                string templateName = Path.Combine(Server.MapPath("~/Template"), "trainning_attendance.html");
                StringBuilder strbld = new StringBuilder(System.IO.File.ReadAllText(templateName));
                strbld.Replace("#ProgramName#", programTraningName)
                    .Replace("#Date#", DateTime.Now.ToString("dd-MM-yyyy"))
                    .Replace("#StartDate#", trainingStartDate)
                    .Replace("#EndDate#", trainingEndDate)
                    .Replace("#AttendDate#", trainingEndDate)
                    .Replace("#TraineeRows#", traineeRows.ToString())
                    .Replace("#Img#", Server.MapPath("~/Images/TraningLatterLogo.png"))
                    .Replace("#BranchName#", "Jaipur");
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
                fileName = $"{Guid.NewGuid().ToString("N").ToUpper()}.pdf";
                filePath = Path.Combine(folderPath, fileName);
                System.IO.File.WriteAllBytes(filePath, pdf);
                _Modal.RICEMTrainingSaveFilePath(TrainingId, fileName, 1);
                return JsonConvert.SerializeObject(new
                {
                    Success = true,
                    DocPath = filePath,
                    DocType = "Trainning Certificate",
                    DocName = fileName,
                    ReturnPath = Url.Action("TrainingAttendance", "Trainning"),
                    Location = "",
                    Designation = ""
                });
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "TrainingController",
                    FunctionName = "TrainingAttendanceFormList()"
                }, ex);

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

        [HttpPost]
        [ActionName("GenerateTaxInvoice")]
        public JsonResult GenerateTaxInvoice(int TrainingId = 0)
        {
            var responseModel = new PacsResponseModel();
            try
            {
                var resultData = GenerateTrainingInvoice(TrainingId);
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
                    ViewPath = ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")) ? "http://localhost:27466/Content/Training/" + responseModel.DocName : "https://rajsahakarapp.rajasthan.gov.in/Content/Training/" + responseModel.DocName

                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.TrainningController", FunctionName = "GenerateTaxInvoice(TrainningModel _Modal)" }, ex);
                return Json(new
                {
                    Success = false,
                    Result = 0,
                    Message = "An error occurred while generating the attendance document.",
                    Error = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        [ActionName("GenerateTrainingInvoice")]
        public string GenerateTrainingInvoice(int TrainingId = 0)
        {
            var dt = new DataTable();
            var resultData = new DataTable();
            int attendenceDay = 0;
            string fileName = string.Empty;
            string folderPath = string.Empty;
            string filePath = string.Empty;
            TrainningModel _Modal = new TrainningModel();
            try
            {
                string wkhtmlPath = Server.MapPath("~/bin/wkhtmltopdf.exe");
                string fontPath = new Uri(Server.MapPath("~/fonts/lohit-Devanagari.ttf")).AbsoluteUri;
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {
                    StringBuilder strbld = new StringBuilder();
                    var templateName = System.IO.Path.Combine(Server.MapPath("~/Template"), "TaxInvoice.html");
                    strbld.Append(System.IO.File.ReadAllText(templateName));
                    resultData = _Modal.TrainningAttendanceInvoiceDetails(TrainingId);
                    string programTraningName = "";
                    string trainingStartDate = "";
                    string trainingEndDate = "";
                    string formattedStartDate = "";
                    string formattedEndDate = "";
                    string GrandTotal = "0.00";
                    string GSTRate = "0";
                    string Amount = "0";
                    string CGSTAmount = "0";
                    string SGSTAmount = "0";
                    string CGSTRate = "0";
                    string SGSTRate = "0";
                    string FeePerParticipantAmt = "0";

                    dt = _Modal.GetProgramNameByTrainingId(TrainingId);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        programTraningName = dt.Rows[0]["ProgrammeNameEn"].ToString();
                        trainingStartDate = dt.Rows[0]["StartDate"].ToString();
                        trainingEndDate = dt.Rows[0]["EndDate"].ToString();
                        trainingEndDate = dt.Rows[0]["EndDate"].ToString();
                        GrandTotal = Convert.ToDecimal(dt.Rows[0]["GrandTotal"]).ToString("0.00");
                        GSTRate = Convert.ToString(dt.Rows[0]["GSTRate"]);
                        Amount = Convert.ToDecimal(dt.Rows[0]["Amount"]).ToString("0.00");
                        CGSTAmount = Convert.ToDecimal(dt.Rows[0]["CGSTAmount"]).ToString("0.00");
                        SGSTAmount = Convert.ToDecimal(dt.Rows[0]["SGSTAmount"]).ToString("0.00");
                        CGSTRate = Convert.ToString(dt.Rows[0]["CGSTRate"]);
                        SGSTRate = Convert.ToString(dt.Rows[0]["SGSTRate"]);
                        FeePerParticipantAmt = Convert.ToDecimal(dt.Rows[0]["FeePerParticipantAmt"]).ToString("0.00");

                        DateTime startDate = Convert.ToDateTime(dt.Rows[0]["PreStartDate"]);
                        DateTime endDate = Convert.ToDateTime(dt.Rows[0]["PreEndDate"]);
                        attendenceDay = (endDate - startDate).Days + 1;
                        formattedStartDate = _Modal.FormatDateWithSuffix(startDate);
                        formattedEndDate = _Modal.FormatDateWithSuffix(endDate);
                        fileName = Convert.ToString(dt.Rows[0]["InvoicePath"]);
                    }

                    folderPath = Path.Combine(Server.MapPath("~/Content/Training/"));
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        filePath = Path.Combine(folderPath, fileName);
                        if (System.IO.File.Exists(filePath))
                        {
                            return JsonConvert.SerializeObject(new
                            {
                                Success = true,
                                DocPath = filePath,
                                DocType = "Trainning Invoice",
                                DocName = fileName,
                                ReturnPath = Url.Action("TrainingAttendance", "Trainning"),
                                Location = "",
                                Designation = ""
                            });
                        }
                    }

                    StringBuilder participantRows = new StringBuilder();
                    int rowSpan = resultData.Rows.Count;
                    for (int i = 0; i < resultData.Rows.Count; i++)
                    {
                        DataRow row = resultData.Rows[i];
                        if (i == 0)
                        {
                            //<td rowspan='{rowSpan}' style='text-align:center;'>{FeePerParticipantAmt}</td>
                            participantRows.Append($@"<tr>
                                        <td rowspan='{rowSpan}' style='text-align:center;'>1</td>
                                        <td rowspan='{rowSpan}'><strong>{programTraningName}</strong></td>
                                        <td style='text-align:center;'>{row["PresentMember"]}</td>
                                        <td style='text-align:center;'>{row["AmtPerTrainer"]}</td>
                                        <td style='text-align:center;'>{row["Days"]}</td>
                                        <td style='text-align:right;'>{Convert.ToDecimal(row["Amount"]).ToString("0.00")}</td>
                                        <td rowspan='{rowSpan}' style='text-align:center;'>{attendenceDay}</td>
                                        <td rowspan='{rowSpan}' style='text-align:center;'>{GSTRate}%</td>
                                        <td rowspan='{rowSpan}' style='text-align:right;'> {Amount}</td></tr>");
                        }
                        else
                        {
                            participantRows.Append($@"<tr>
                                        <td style='text-align:center;'>{row["PresentMember"]}</td>
                                        <td style='text-align:center;'>{row["AmtPerTrainer"]}</td>
                                        <td style='text-align:center;'>{row["Days"]}</td>
                                        <td style='text-align:right;'>{Convert.ToDecimal(row["Amount"]).ToString("0.00")}</td></tr>");
                        }
                    }

                    strbld.Replace("#NameofProgramInstitute#", programTraningName)
                            .Replace("#Date#", DateTime.Now.ToString("dd-MM-yyyy"))
                            .Replace("#FeePerParticipant#", FeePerParticipantAmt)
                            .Replace("#StartDate#", trainingStartDate)
                            .Replace("#EndDate#", trainingEndDate)
                            .Replace("#InvoiceNo#", "050")
                            .Replace("#ICEMLogo#", Server.MapPath("~/Images/RICEMLogo.jpg"))
                            .Replace("#CoopLogo#", Server.MapPath("~/Images/Cooprativelogo.jpg"))
                            .Replace("#GSTNo#", "08AAAAR0187J1ZE")
                            .Replace("#PANNo#", "AAAAR0187J")
                            .Replace("#GSTINo#", "08JPRR02550C1DE")
                            .Replace("#Taxable#", Amount)
                            .Replace("#CGST#", CGSTRate + "%")
                            .Replace("#CGSTAmt#", CGSTAmount)
                            .Replace("#SGST#", SGSTRate + "%")
                            .Replace("#SGSTAmt#", SGSTAmount)
                            .Replace("#GrandTotal#", GrandTotal)
                            .Replace("#InstitutePANNo#", "AAAAR0187J")
                            .Replace("#RateOfGST#", GSTRate + "%")
                            .Replace("#formattedStartDate#", formattedStartDate)
                            .Replace("#formattedEndDate#", formattedEndDate)
                            //.Replace("#TraningDays#", "0")
                            .Replace("#AmtWords#", _Modal.ConvertAmountToWords(GrandTotal))
                            .Replace("#AmtRs#", Amount)
                            .Replace("#ParticipantDetails#", participantRows.ToString())
                            .Replace("#NoOfParticipant#", resultData.Rows.Count.ToString());
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
                    fileName = $"{Guid.NewGuid().ToString("N").ToUpper()}.pdf";
                    filePath = Path.Combine(folderPath, fileName);
                    System.IO.File.WriteAllBytes(filePath, pdf);
                    _Modal.RICEMTrainingSaveFilePath(TrainingId, fileName, 2);
                    return JsonConvert.SerializeObject(new
                    {
                        Success = true,
                        DocPath = filePath,
                        DocType = "Trainning Invoice",
                        DocName = fileName,
                        ReturnPath = Url.Action("TrainingAttendance", "Trainning"),
                        Location = "",
                        Designation = ""
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
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "TrainingController", FunctionName = "GenerateTrainingInvoice(TrainningModel _Modal)" }, ex);
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



        [HttpPost]
        public JsonResult GetTrainingDataByTrainingId(int TrainningId = 0)
        {
            APIResponce responce = new APIResponce();
            try
            {
                TrainningModel trainningModel = new TrainningModel();
                var result = trainningModel.GetTrainingAttendanceByTrainingId(TrainningId);
                responce.Status = "Success";
                responce.Message = "Data Loaded";
                responce.Data = new
                {
                    data = JsonConvert.SerializeObject(result)
                };
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "TrainingController", FunctionName = "GetTrainingDataByTrainingId(int TrainningId = 0)" }, ex);
                responce.Status = "Error";
                responce.Message = "An error occurred while loading the training data.";
                responce.Data = null;
            }

            return Json(responce, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertTraningAttendanceUserApproved(TrainningAttendanceUser model)
        {
            try
            {
                TrainningModel trainningModel = new TrainningModel();
                int result = trainningModel.InsertTraningAttendanceUsersApproved(model);

                return Json(new
                {
                    Status = true,
                    Message = "Record saved successfully.",
                    Data = result
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                { PageName = "TrainningController", FunctionName = "InsertTraningAttendanceUserApproved" }, ex);

                return Json(new
                {
                    Status = false,
                    Message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [ActionName("TrainningAttendanceUsersList")]
        public string TrainningAttendanceUsersList(long TrainingId = 0)
        {
            var objTrainningModel = new TrainningModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objTrainningModel.TrainningAttendanceUsersList(TrainingId));
        }


        #endregion

    }
}