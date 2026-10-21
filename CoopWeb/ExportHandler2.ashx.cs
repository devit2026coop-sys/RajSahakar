using System;
using System.Web;
using System.IO;
using System.Data;
using System.Text;
using Cooperative.DBConnector;
using System.Data.SqlClient;
using NReco.PdfGenerator;//added this reference to DLL which is copied and pasted in bin folder. 
using Cooperative.Models;

namespace Cooperative
{
    /// <summary>
    /// Summary description for ExportHandler2
    /// </summary>
    public class ExportHandler2 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                DataTable dt;
                DBFactoryMSSQL objDBAccess;
                string FileNAme = DateTime.Now.ToString().Replace("/", "").Replace("-", "");


                StringBuilder sb = new StringBuilder();
                sb.Append("<html><head><title></title><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head><body><div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;' ><center></center></div><br /><div id='divtoPrint'>");
                sb.Append("<div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px; '>");
                sb.Append("<table style='width: 100% ;font-size: 23px;' border='0' cellspacing='0px' cellpadding='5'>");

                if (context.Request.Form["PdfId"] != null)
                {
                    objDBAccess = new DBFactoryMSSQL();
                    objDBAccess.OpenConnection();
                    if (context.Request.Form["PdfId"] == "2")
                    {
                        SqlCommand cmd = objDBAccess.CreateCommand("CCSComplaintListByAADHAR", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@AADHAREnc", DBFactoryParameterType.NVarchar, "");
                        objDBAccess.AddParameterToCommand("@ComplaintIdEnc", DBFactoryParameterType.NVarchar, Convert.ToString(context.Request.Form["data"]));
                        objDBAccess.AddParameterToCommand("@SearchType", DBFactoryParameterType.Int, 3);
                        dt = objDBAccess.GetTable();

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            //sb.Append("<html><head><style>.lable{font-size:30px!important;}</style></head><body>");

                            //sb.Append("<div id='divtoOrderPrint' ng-if='ShowOrderDetails' style='padding-left:17%; padding-right:17%;'>");
                            //sb.Append("<div style='margin-right:25px;    margin-left:25px;    font-size:23px;    padding:30px;'>");

                            //sb.Append("<table id='content' border='0' style='width:100%;' cellpadding='5' cellspacing='0'>");
                            FileNAme = Convert.ToString(dt.Rows[0]["ReceiptNo"]) + "_" + FileNAme;
                            //sb.Append("<tr><td colspan='2'><div><img src=\"http://rajsahakar.rajasthan.gov.in/Content/images/Logo.jpg\"  alt=\"Sahakarita Vibhag\" /></div></td></tr>");
                            sb.Append("<tr><td colspan='2' style='text-align:center;'><label style='font-size: 35px!important;'> सहकारिता विभाग </label><br/></td></tr>");
                            sb.Append("<tr><td colspan='2' style='text-align:center;'><label style='font-size: 30px!important;'> नेहरू सहकार भवन, भवानी सिंह रोड, जयपुर, राजस्थान</label><br/></td></tr>");
                            sb.Append("<tr><td colspan='2' style='text-align:center;'><label style='font-size: 25px!important;'> फ़ोन नंबर: 0141-2740045, 2740737</label><br/></td></tr>");


                            sb.Append("<tr style='margin-top:5px;'><td colspan='2'>&nbsp;</td></tr>");
                            sb.Append("<tr><td colspan='2' style='text-align:center;'><label style='font-size: 35px!important;'><i> क्रेडिट को ऑपरेटिव सोसायटी के विरुद्ध दर्ज शिकायत का विवरण <i>  </label><br/></td></tr>");

                            sb.Append("<tr style='margin-top:5px;'><td colspan='2'>&nbsp;</td></tr>");
                            sb.Append("<tr style='margin-top:5px;'><td colspan='2' style='text-align:left;'><label style='font-size: 35px!important;'> शिकायतकर्ता का विवरण</label><br/></td></tr>");
                            sb.Append("<tr style='margin-top:5px;'><td colspan='2' style='text-align:left;'><label style='font-size: 35px!important;'> -----------------------------------------------------------------------</label><br/></td></tr>");
                            sb.Append("<tr style='margin-top:5px;'><td colspan='2'><b>शिकायत संख्या : </b>" + Convert.ToString(dt.Rows[0]["ReceiptNo"]) + "</td></tr>");
                            sb.Append("<tr style='margin-top:5px;'><td><b>शिकायतकर्ता का नाम : </b>" + Convert.ToString(dt.Rows[0]["ComplainerName"]) + "</td><td style='margin-left:5px;'><b>पिता / पति का नाम : </b>" + Convert.ToString(dt.Rows[0]["ComplainerFatherName"]) + "</td></tr>");
                            sb.Append("<tr style='margin-top:5px;'><td colspan='2'><b>पता : </b>" + Convert.ToString(dt.Rows[0]["Address"]) + "</td></tr>");
                            sb.Append("<tr style='margin-top:5px;'><td><b>मोबाइल नंबर : </b>" + Convert.ToString(dt.Rows[0]["MobileNo"]) + "</td><td><b>ईमेल आईडी : </b>" + Convert.ToString(dt.Rows[0]["Email"]) + "</td></tr>");
                            sb.Append("<tr style='margin-top:5px;'><td><b>शिकायत दर्ज  दिनांक : </b>" + Convert.ToString(dt.Rows[0]["CreateDateTime"]) + "</td><td><b>फार्म भरने का स्रोत : </b>" + (Convert.ToString(dt.Rows[0]["UserType"]).ToUpper() == "K" ? "कियोस्क द्वारा " : "स्वयं सिटीजन द्वारा") + "</td></tr>");

                            sb.Append("<tr style='margin-top:5px;'><td colspan='2'>&nbsp;</td></tr>");
                            sb.Append("<tr><td colspan='2' style='text-align:left;'><label style='font-size: 35px!important;'> क्रेडिट कोऑपरेटिव सोसायटी का विवरण </label><br/></td></tr>");
                            sb.Append("<tr style='margin-top:5px;'><td colspan='2' style='text-align:left;'><label style='font-size: 35px!important;'> -----------------------------------------------------------------------</label><br/></td></tr>");
                            sb.Append("<tr style='margin-top:5px;'><td colspan='2'><b>ज़िला  : </b>" + Convert.ToString(dt.Rows[0]["DISTRICT_MANGAL"]) + "</td></tr>");
                            sb.Append("<tr style='margin-top:5px;'><td colspan='2'><b>क्रेडिट को  ऑपरेटिव सोसाइटी का नाम : </b>" + Convert.ToString(dt.Rows[0]["SocietyName"]) + "</td></tr>");
                            sb.Append("<tr style='margin-top:5px;'><td colspan='2'><b>ब्रांच  : </b>" + Convert.ToString(dt.Rows[0]["BranchName"]) + "</td></tr>");
                            sb.Append("<tr style='margin-top:5px;'><td><b>ग्राहक संख्या (बीपी नंबर) : </b>" + Convert.ToString(dt.Rows[0]["BPNo"]) + "</td><td><b>सदस्यता क्रमांक : </b>" + Convert.ToString(dt.Rows[0]["MembershipId"]) + "</td></tr>");
                            sb.Append("<tr style='margin-top:5px;'><td><b>क्रेडिट कोऑपरेटिव सोसायटी का खाता नम्बर : </b>" + Convert.ToString(dt.Rows[0]["AccountNo"]) + "</td><td><b>क्रेडिट कोऑपरेटिव सोसायटी का आई एफ एस सी नंबर : </b>" + Convert.ToString(dt.Rows[0]["IFSCCode"]) + "</td></tr>");
                            sb.Append("<tr style='margin-top:5px;'><td colspan='2'><b>कुल जमा राशि (&#8377; में)  : </b>" + Convert.ToDecimal(dt.Rows[0]["AmountDeposited"]) + "</td></tr>");
                            sb.Append("<tr style='margin-top:5px;'><td colspan='2'><b>टिप्पणी  : </b>" + Convert.ToString(dt.Rows[0]["Remarks"]) + "</td></tr>");

                            sb.Append("<tr style='margin-top:5px;'><td colspan='2'>&nbsp;</td></tr>");
                            sb.Append("<tr><td colspan='2' style='text-align:left;'><label style='font-size: 35px!important;'> रेमिटेंस हेतु बैंक खाते का विवरण </label><br/></td></tr>");
                            sb.Append("<tr style='margin-top:5px;'><td colspan='2' style='text-align:left;'><label style='font-size: 35px!important;'> -----------------------------------------------------------------------</label><br/></td></tr>");
                            sb.Append("<tr style='margin-top:5px;'><td> <b>बैंक खाता संख्या : </b>" + Convert.ToString(dt.Rows[0]["AccountNoInSociety"]) + "</td><td><b>बैंक खाता आई एफ एस सी कोड : </b>" + Convert.ToString(dt.Rows[0]["AccountIFSCInSociety"]) + "</td></tr>");
                            sb.Append("<tr style='margin-top:5px;'><td><b>खाता धारक का नाम : </b>" + Convert.ToString(dt.Rows[0]["AccountHolderName"]) + "</td><td>&nbsp;</td></tr>");

                            sb.Append("<tr style='margin-top:5px;'><td colspan='2'>&nbsp;</td></tr>");
                            sb.Append("<tr><td colspan='2' style='text-align:left;'><label style='font-size: 35px!important;'> आपके जमाओं से सम्बंधित दस्तावेजों का विवरण </label><br/></td></tr>");
                            sb.Append("<tr style='margin-top:5px;'><td colspan='2' style='text-align:left;'><label style='font-size: 35px!important;'> -----------------------------------------------------------------------</label><br/></td></tr>");

                            sb.Append("<tr>");
                            sb.Append("<td colspan='2'>");
                            sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;'>");
                            sb.Append("<tr><th style='text-align:center'> क्र. सं. </th><th style='text-align:center'> दस्तावेज का नाम </th><th  style='text-align:center'> खाता संख्या/दस्तावेज संख्या </th><th style='text-align:center'> जमा कराई गई राशि (&#8377; में)</th><th  style='text-align:center'> जमा करने की तिथि </th><th  style='text-align:center'> परिपक्वता राशि (&#8377; में)</th><th  style='text-align:center'> परिपक्वता तिथि </th></tr>");
                            int index = 1;

                            foreach (var item in dt.AsEnumerable())
                            {
                                sb.Append("<tr><td style='text-align:center'>" + (index) + "</td>");
                                sb.Append("<td style='text-align:center'>" + item.Field<string>("TypeNameHi") + "</td>");
                                sb.Append("<td style='text-align:center'>" + item.Field<string>("DetailAccountNo") + "</td>");
                                sb.Append("<td style='text-align:center'>" + item.Field<decimal>("PaidAmount") + "</td>");
                                sb.Append("<td style='text-align:center'>" + item.Field<string>("PayDate") + "</td>");
                                sb.Append("<td style='text-align:center'>" + item.Field<decimal>("MaturityAmount") + "</td>");
                                sb.Append("<td style='text-align:center'>" + item.Field<string>("MaturityDate") + "</td></tr>");
                                index++;
                            }
                            sb.Append("</table>");
                            sb.Append("</td>");
                            sb.Append("</tr>");

                            sb.Append("</table>");
                            sb.Append("</div>");
                            sb.Append("</div>");
                            sb.Append("</body></html>");

                        }
                    }

                }

                // string  printContents = '<html><head><title></title><meta http-equiv="Content-Type" content="text/html; charset=utf-8" /></head><body><div style="padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;" ><center></center></div><br />' + sb + '</body></html>';

                //string Data = context.Request.Form["data"];       //Div Content will be fetched from form data.
                string InpuData = sb.ToString();       //Div Content will be fetched from form data.
                                                       //If You find this error in above line 
                                                       //Error: A potentially dangerous Request.Form value was detected from the client (data="<html><head><title><...").
                                                       //then add this in your Web.config file.
                                                       // <httpRuntime requestValidationMode="2.0" /> inside   <system.web>  </system.web>

                string PageType = context.Request.Form["PageType"];  //here we will recieve Page Type sent from front end.
                if (string.IsNullOrEmpty(InpuData))
                    InpuData = "Some Error occured.Content not found.Please try again.";

                string appPath = HttpContext.Current.Request.PhysicalApplicationPath;

                var htmlContent = InpuData.Replace("AppPath", appPath);
                var pdfDoc = new NReco.PdfGenerator.HtmlToPdfConverter(); //created an object of HtmlToPdfConverter class.

                if (string.IsNullOrEmpty(PageType))
                    pdfDoc.Orientation = PageOrientation.Default;  //setting orientation.
                else
                {
                    if (PageType == "Landscape")
                        pdfDoc.Orientation = PageOrientation.Landscape;
                    else
                        pdfDoc.Orientation = PageOrientation.Portrait;
                }

                pdfDoc.Size = PageSize.A4;   //8.27 in × 11.02 in //Page Size
                PageMargins pageMargins = new PageMargins();     //Margins in mm
                pageMargins.Bottom = 05;
                pageMargins.Left = 05;
                pageMargins.Right = 05;
                pageMargins.Top = 05;
                pdfDoc.Margins = pageMargins;                      //margins added to PDF.

                pdfDoc.PageHeaderHtml = " ";
                //Why I am adding this to page footer?? So we can get paging in footer section of each PDF page.how its working?? A Javascript code is written inside the DLL which is handling  Div's class i.e. page and topage.
                pdfDoc.PageFooterHtml = "<div style='float:right;'>  Page <span class='page'></span> of  <span class='topage'></span></div>";
                var pdfBytes = pdfDoc.GeneratePdf(htmlContent);

                context.Response.ContentType = "application/pdf";
                context.Response.AddHeader("content-disposition", "attachment;filename=" + FileNAme + ".pdf");//Use inline in place of attachment If You wish to open PDF on Browser.
                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.BinaryWrite(pdfBytes);
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "ExportHandler2 : IHttpHandler", FunctionName = "public void ProcessRequest(HttpContext context)" }, ex);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}