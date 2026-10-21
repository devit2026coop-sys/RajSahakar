using System;
using System.Web;
using System.IO;
using System.Data;
using System.Text;
using Cooperative.DBConnector;
using System.Data.SqlClient;
using NReco.PdfGenerator;
using System.Web.Script.Serialization;
using System.Diagnostics;

namespace Cooperative.Models
{
    public static class PDFgenerator
    {
        public static string Generate(string html, int PageDirection)
        {
            if (string.IsNullOrEmpty(html))
                html = "<h1>Content not available for Document.</h1>";

            string appPath = HttpContext.Current.Request.PhysicalApplicationPath;

            //var htmlContent = InpuData.Replace("AppPath", appPath);
            var pdfDoc = new HtmlToPdfConverter(); //created an object of HtmlToPdfConverter class.


            pdfDoc.Orientation = PageOrientation.Default;  //setting orientation.

            if (PageDirection == 1)
                pdfDoc.Orientation = PageOrientation.Landscape;
            else
                pdfDoc.Orientation = PageOrientation.Portrait;

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
            var pdfBytes = pdfDoc.GeneratePdf(html);
            return Convert.ToBase64String(pdfBytes);
        }

        public static byte[] GeneratePdfFromHtml(string htmlContent, string wkhtmlPath)
        {
            string tempHtmlPath = Path.GetTempFileName() + ".html";
            string tempPdfPath = Path.GetTempFileName() + ".pdf";

            // Save HTML with UTF-8 encoding
            File.WriteAllText(tempHtmlPath, htmlContent, System.Text.Encoding.UTF8);

            var psi = new ProcessStartInfo
            {
                FileName = wkhtmlPath,
                Arguments = $"--encoding utf-8 --enable-local-file-access \"{tempHtmlPath}\" \"{tempPdfPath}\"",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(psi))
            {
                process.WaitForExit();
            }

            var pdfBytes = File.ReadAllBytes(tempPdfPath);

            // Optional cleanup
            File.Delete(tempHtmlPath);
            File.Delete(tempPdfPath);

            return pdfBytes;
        }

        public static byte[] GenerateFSPdfFromHtml(string htmlContent, string wkhtmlPath)
        {
            string tempHtmlPath = Path.GetTempFileName() + ".html";
            string tempPdfPath = Path.GetTempFileName() + ".pdf";

            // Save HTML with UTF-8 encoding
            File.WriteAllText(tempHtmlPath, htmlContent, System.Text.Encoding.UTF8);

            var psi = new ProcessStartInfo
            {
                FileName = wkhtmlPath,
                //Arguments = $"--encoding utf-8 --enable-local-file-access \"{tempHtmlPath}\" \"{tempPdfPath}\"",
                //Arguments = $"--encoding utf-8 --enable-local-file-access " + $"--page-size A4 --margin-bottom 45.23mm " + $"\"{tempHtmlPath}\" \"{tempPdfPath}\"",
                Arguments =
                $"--encoding utf-8 " +
                $"--enable-local-file-access " +
                $"--page-size A4 " +
                $"--margin-top 5.29mm " +
                $"--margin-bottom 45.23mm " +
                $"--margin-left 5.29mm " +
                $"--margin-right 5.29mm " +
                $"\"{tempHtmlPath}\" \"{tempPdfPath}\"",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(psi))
            {
                process.WaitForExit();
            }

            var pdfBytes = File.ReadAllBytes(tempPdfPath);

            // Optional cleanup
            File.Delete(tempHtmlPath);
            File.Delete(tempPdfPath);

            return pdfBytes;
        }
    }

    public static class GenerateCCSComplaintPDF
    {
        public static string GetPDF(string ComplaintIdEnc)
        {
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            DataTable dt = objCitizenModel.CCSComplaintListByAADHAR("", -1, 3, ComplaintIdEnc: ComplaintIdEnc);
            StringBuilder sb = new StringBuilder();
            string FileName = string.Empty;

            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    FileName = Convert.ToString(dt.Rows[0]["ReceiptNo"]);

                    if (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["ComplaintPDFfile"])))
                    {

                        sb.Append("<html><head><title></title><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head><body><div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;' ><center></center></div><br /><div id='divtoPrint'>");
                        sb.Append("<div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px; '>");
                        sb.Append("<table style='width: 100% ;font-size: 23px;' border='0' cellspacing='0px' cellpadding='5'>");


                        //sb.Append("<tr><td colspan='2'><div><img src=\"http://rajsahakar.rajasthan.gov.in/Content/images/Logo.jpg\"  alt=\"Sahakarita Vibhag\" /></div></td></tr>");
                        sb.Append("<tr><td colspan='2' style='text-align:center;'><label style='font-size: 35px!important;'> सहकारिता विभाग </label><br/></td></tr>");
                        sb.Append("<tr><td colspan='2' style='text-align:center;'><label style='font-size: 30px!important;'> नेहरू सहकार भवन, भवानी सिंह रोड, जयपुर, राजस्थान</label><br/></td></tr>");
                        sb.Append("<tr><td colspan='2' style='text-align:center;'><label style='font-size: 25px!important;'> फ़ोन नंबर: 0141-2740045, 2740737</label><br/></td></tr>");


                        sb.Append("<tr style='margin-top:5px;'><td colspan='2'>&nbsp;</td></tr>");
                        sb.Append("<tr><td colspan='2' style='text-align:center;'><label style='font-size: 35px!important;'><i><b> क्रेडिट को ऑपरेटिव सोसायटी के विरुद्ध दर्ज शिकायत का विवरण </b><i>  </label><br/></td></tr>");

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

                        string base64String = PDFgenerator.Generate(sb.ToString(), 2);

                        string DirctoryPath = System.Web.HttpContext.Current.Server.MapPath("~/CCSComplaintPDFfiles/");
                        string FilePath = System.Web.HttpContext.Current.Server.MapPath("~/CCSComplaintPDFfiles/" + FileName + ".pdf");


                        if (!Directory.Exists(DirctoryPath))
                        {
                            DirectoryInfo directory = Directory.CreateDirectory(DirctoryPath);
                        }

                        if (!System.IO.File.Exists(FilePath))
                        {
                            using (StreamWriter sw = System.IO.File.CreateText(FilePath))
                            {
                            }
                        }

                        byte[] bytes = Convert.FromBase64String(base64String);
                        File.WriteAllBytes(FilePath, bytes);

                        objCitizenModel.UpdateCCSComplaintFile(ComplaintIdEnc, FileName + ".pdf");
                    }

                    var t = new { Result = true, Message = "File Generated Successfully.", FileName = "http://rajsahakarapp.rajasthan.gov.in/CCSComplaintPDFfiles/" + FileName + ".pdf" };
                    return new JavaScriptSerializer().Serialize(t);
                }
                else
                {
                    sb.Append("<h1>Content not available for Document.</h1>");
                    var t = new { Result = false, Message = "No Data Available for PDF Generate" };
                    return new JavaScriptSerializer().Serialize(t);
                }
            }
            catch (Exception ex)
            {
                DBLogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PDFgenerator", FunctionName = "public static class GenerateCCSComplaintPDF >> public static string GetPDF(string AADHAREnc) " + Convert.ToString(ex.InnerException) + Convert.ToString(ex.StackTrace) }, ex);

                var t = new { Result = false, Message = "Error while Complaint Registration. pdf generate" };
                return new JavaScriptSerializer().Serialize(t);
            }
        }
    }

}