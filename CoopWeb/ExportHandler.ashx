<%@ WebHandler Language="C#" Class="ExportHandler" %>

using System;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Text;
using NReco.PdfGenerator;//added this reference to DLL which is copied and pasted in bin folder. 

public class ExportHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
             StringBuilder sb = new StringBuilder();
            sb.Append("<html><head><title></title><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head><body><div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;' ><center></center></div><br /><div id='divtoPrint'>");
            sb.Append("<div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px; '>");
            sb.Append("<table style='width: 100% ;font-size: 23px;' border='0' cellspacing='0px' cellpadding='0px'>");
            sb.Append("<tr><td colspan='4' align='center' style='width: 100%'><h3>कार्यालय.......................................सहकारी सोसायटी लि.</h3></td></tr>");
            sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
            sb.Append("<tr><td colspan='3' align='left' style='width: 75%'>क्रमांक फा. () </td><td colspan='1' align='left' style='width: 25%'>दिनांक </td></tr>");
            sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
            sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>मुख्य अंकेक्षक /</td></tr>");
            sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>क्षेत्रीय अंकेक्षण अधिकारी, खण्ड ............................../</td></tr>");
            sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>विशेष लेखा परीक्षक,</td></tr>");
            sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>सहकारी समितियाँ,</td></tr>");
            sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>..................................</td></tr>");
            sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
            sb.Append("<tr><td colspan='1' align='left' style='width: 25%'>&nbsp;</td><td colspan='3' align='left' style='width: 75%'>&nbsp; विषय: सांविधिक लेखापरीक्षा हेतु लेखापरीक्षक की नियुक्ति के क्रम में। </td></tr>");
            sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
            sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>महोदय,</td></tr>");
            sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
            sb.Append("<tr><td colspan='4' align='left' style='width: 100%'><p style='text-align: justify;text-justify: inter-word;'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; विषयान्तर्गत निवेदन है की राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 ");
            sb.Append("सपठित नियम 73 के प्रावधानों की अनुपालना में सोसायटी की समिति की बैठक दिनांक.......................................के संकल्प क्रमांक ...................................... द्वारा विभाग द्वारा वर्ष 2018 से 21 के लिये अधिसूचित चार्टर्ड अकाउंटेंट / चार्टर्ड अकाउंटेंट फर्म के पेनल में से क्रमांक");
            sb.Append(".............................................पर अधिसूचित चार्टर्ड अकाउंटेंट / चार्टर्ड अकाउंटेंट फर्म ................................................................. को सोसायटी की वर्ष .................. से वर्ष .................. तक की सांविधिक लेखापरीक्षा के लिये लेखापरीक्षक ");
            sb.Append("नियुक्त किये जाने का निर्णय लिया गया है। सोसायटी द्वारा नियुक्त लेखापरीक्षक को....................रु.लेखापरीक्षा शुल्क भुगतान किये जाने का निर्णय लिया गया है।</p></td></tr>");
            sb.Append("<tr><td colspan='4' align='left' style='width: 100%'><p style='text-align: justify;text-justify: inter-word;'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; नियुक्त लेखा परीक्षक को राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 54 एवं नियम 2003 ");
            sb.Append("के नियम 72 एवं 73 के प्रावधानों एवं नियुक्ति आदेश में वर्णित शर्तो के अनुसार लेखापरीक्षा पूर्ण कर लेखापरीक्षा रिपोर्ट की प्रतियाँ सोसायटी एवं रजिस्ट्रार को प्रेषित करने हेतु निर्देशित कर दिया गया है। सुलभ सन्दर्भ हेतु सोसायटी के संकल्प एवं लेखापरीक्षक नियुक्ति के आदेश की प्रति सलग्न है।</p></td></tr>");
            sb.Append("<tr><td colspan='4' align='left' style='width: 100%'>&nbsp;</td></tr>");
            sb.Append("<tr><td colspan='3' align='left' style='width: 75%'>&nbsp; </td><td colspan='1' align='left' style='width: 25%'>मुख्य कार्यकारी / सक्षम प्राधिकारी </td></tr>");
            sb.Append("<tr><td colspan='3' align='left' style='width: 75%'>&nbsp; </td><td colspan='1' align='left' style='width: 25%'>नाम.................................. </td></tr>");
            sb.Append("<tr><td colspan='3' align='left' style='width: 75%'>&nbsp; </td><td colspan='1' align='left' style='width: 25%'>पता..................................... </td></tr>");
            sb.Append("<tr><td colspan='3' align='left' style='width: 75%'>&nbsp; </td><td colspan='1' align='left' style='width: 25%'>फोन न. ................................ </td></tr>");
            sb.Append("</table></div><br /></div></body></html>");
            // string  printContents = '<html><head><title></title><meta http-equiv="Content-Type" content="text/html; charset=utf-8" /></head><body><div style="padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;" ><center></center></div><br />' + sb + '</body></html>';

        string Data = context.Request.Form["data"];       //Div Content will be fetched from form data.
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
            if(PageType=="Landscape")
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
        var pdfBytes=pdfDoc.GeneratePdf(htmlContent);
        context.Response.ContentType = "application/pdf";
        context.Response.AddHeader("content-disposition", "attachment;filename=ExportedFile.pdf");//Use inline in place of attachment If You wish to open PDF on Browser.
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        HttpContext.Current.Response.BinaryWrite(pdfBytes);
        //HttpContext.Current.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}