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
using ExcelDataReader;


namespace Cooperative.Controllers
{
    public class textController : Controller
    {
        // GET: text
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ActionName("CertificateGenerate")]
        public string CertificateGenerate(string OTP, string TXN)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {


                    var otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                    ApplicationFlowModel AppFlowModel = new ApplicationFlowModel();
                    DataTable societyDt = AppFlowModel.GetSocietyListForCertificate();
                    if (otpResult.status != "1")
                    {
                        return "-1";
                    }
                    string societIds = "";
                    string Certdate = "";//CertDate
                    foreach (DataRow dr in societyDt.Rows)
                    {
                        societIds += (societIds != "" ? "," : "") + dr["SocietyIDEnc"].ToString();
                    }
                    foreach (DataRow dr in societyDt.Rows)
                    {
                        Certdate += (Certdate != "" ? "," : "") + dr["CertDate"].ToString();
                    }
                    int index = 0;
                    foreach (string Id in societIds.Split(','))
                    {

                        DataTable dt = AppFlowModel.GetScietyAdministrativeDetail(Id, 1, " ");
                        string fileNameCerti = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + ".pdf";
                        string fileNameMemList = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + ".pdf";

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

                        string Filename= dt.Rows[0]["RegistrationCertificateFile"].ToString(); 

                        //string Location = dt.Rows[0]["Location"].ToString();

                        if (lbl_OldRegistrationNo == "")
                        {
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            string actNameEn = lbl_ActId == "1" ? ("The Rajasthan Societies Registration Act, 1958 (Rajasthan Act No. 28, 1958)").ToUpper() : lbl_ActId == "3" ? ("The Rajasthan Co-operative Societies Act, 2001").ToUpper() : lbl_ActId == "7" ? ("The Rajasthan Societies Registration Act, 2005").ToUpper() : "";
                            string actNameHi = lbl_ActId == "1" ? ("राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 1958 (राजस्थान एक्ट नंबर 28, 1958) ").ToUpper() : lbl_ActId == "3" ? ("राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 2001").ToUpper() : lbl_ActId == "7" ? ("राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 2005").ToUpper() : "";
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
                            sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> दिनांक / DATE- " + Certdate.Split(',')[index] + " </td>");
                            sb.Append("</tr>");
                            sb.Append("</table>");
                            sb.Append("</td>");
                            sb.Append("</tr>");

                            //sb.Append("<td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'> यह प्रमाणित किया जाता है कि <b> " + lbl_InstitutionName.ToUpper() + " </b> जिला <b> " + lbl_AO_ADD_DistrictEn.ToUpper() + " </b>  का रजिस्ट्रेशन <b>' " + actNameHi + " '</b>  के अन्तर्गत आज किया गया है। यह प्रमाण पत्र मेरे डिजिटल हस्ताक्षरों से आज जारी किया गया है। </td>");

                            if (lbl_ActId == "1" || lbl_ActId == "7")
                            {
                                sb.Append("<tr><td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'><p style='text-align: justify;text-justify;'>  यह प्रमाणित किया जाता है कि <b> " + lbl_InstitutionName.ToUpper() + " </b> जिला <b> " + lbl_AO_ADD_DistrictEn.ToUpper() + " </b>  का रजिस्ट्रेशन <b>' " + actNameEn + " '</b>  के अन्तर्गत आज किया गया है। यह प्रमाण पत्र मेरे डिजिटल हस्ताक्षरों से आज जारी किया गया है।  </p></td></tr>");
                                sb.Append("<tr><td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'><p style='text-align: justify;text-justify;'>  IT IS CERTIFIED THAT <b>" + lbl_InstitutionName.ToUpper() + "</b> AT  DISTRICT  <b>" + lbl_AO_ADD_DistrictEn.ToUpper() + "</b> IS REGISTERED UNDER  <b>'" + actNameHi + "'</b>. THIS CERTIFICATE IS ISSUED TODAY UNDER MY DIGITAL SIGNATURE. </p></td></tr>");
                                sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/></td></tr>");
                            }
                            else if (lbl_ActId == "3")
                            {

                                sb.Append("<tr><td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'><p style='text-align: justify;text-justify;'> प्रमाणित किया जाता है कि  श्री " + lbl_Name.ToUpper() + "  द्वारा " + lbl_InstitutionName.ToUpper() + "  के नाम से एक सहकारी सोसाइटी  के रजिस्ट्रेशन हेतु प्रस्तुत आवेदन को स्वीकार करते हुए उक्त  सोसाइटी एवं  उसके द्वारा अंगीकृत उपविधियों का राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 2001  की धारा 6 के अंतर्गत  आज  दिनांक " +Certdate.Split(',')[index] + "  को क्रमांक  " + lbl_Registraionno + "  पर  रजिस्ट्रीकरण किया गया । यह  प्रमाण पत्र  मेरी  मुद्रा  एवं हस्ताक्षर सहित आज दिनांक "  +Certdate.Split(',')[index] + "  को प्रसारित  किया  जाता  है । </p></td></tr>");
                                sb.Append("<tr><td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'><p style='text-align: justify;text-justify;'> " + ("Certified that accepting the application made by Mr " + lbl_Name.ToUpper() + " for the registration of a co-operative society under the name " + lbl_InstitutionName.ToUpper() + ", the said society alongwith the bye - laws adopted by it, is registered at number " + lbl_Registraionno + " on dated " +Certdate.Split(',')[index] + " under section 6 of The Rajasthan Co-operative Societies Act, 2001.This registration certificate is issued under my seal and signatures on date " + Certdate.Split(',')[index]).ToUpper() + " </p></td></tr>");
                                sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'><br/><br/><br/><br/><br/><br/><br/></td></tr>");
                            }

                            //sb.Append("<tr><td style='padding: 15px; line-height: 30px; font-size: 12px; font-weight: normal; height: 100px;'>IT IS CERTIFIED THAT <b>" + lbl_InstitutionName.ToUpper() + "</b> AT  DISTRICT  <b>" + lbl_AO_ADD_DistrictEn.ToUpper() + "</b> IS REGISTERED UNDER  <b>'" + actNameEn + "'</b>. THIS CERTIFICATE IS ISSUED TODAY UNDER MY DIGITAL SIGNATURE.</td></tr>");
                            //sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'><br/><br/><br/><br/><br/><br/><br/></td></tr>");
                            sb.Append("</table>");
                            sb.Append("</div>");
                            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                            sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));


                            if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling_1")))
                            {
                                System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling_1"));
                            }

                            if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling_1/" + lbl_Registraionno.Replace("/", ""))))
                            {
                                System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling_1/" + lbl_Registraionno.Replace("/", "")));
                            }
                            string filepath = System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling_1/" + lbl_Registraionno.Replace("/", "") + "/") + Filename;
                            if (COMMON_FUNCTION.GeneratePDF(sb1, filepath, ""))
                            {
                                var result = COMMON_FUNCTION.EsinghPDF(otpResult.data, filepath, lbl_AO_ADD_DistrictEn, "Registrar", "350", "120", "150", "120");

                                if (result.status == "1")
                                {
                                    AppFlowModel.InsertSocietyTestCert(Id);

                                }
                            };

                        }

                        ++index;
                        //return ""; // lbl_SocietyDirectory + "/" + fileNameMemList + "#" + lbl_SocietyDirectory + "/" + BylawsStr;

                    }
                    return "";
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


        [HttpPost]
        public JsonResult GetGeneratedCertificate()
        {
            Models.MISModel objNew = new Models.MISModel();
            DataTable DT = objNew.GetGeneratedCertificate();
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [ActionName("GenerateAadhaarOTPForEsing")]
        public string GenerateAadhaarOTPForEsing()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser != null)
            {
                SSOWSSoap.SSOUserDetail usrdtl = new SSOWSSoap.SSOUserDetail();
                SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
                usrdtl = ssoclient.GetUserDetailXML(_LoggedInUser.SSOID, "RAJSAHKAR.TEST", "Rajasthan@12");

                // return COMMON_FUNCTION.GenerateOTPForEsingh("940396382715");
                string aadhaarId = usrdtl.aadhaarId;
             

                return COMMON_FUNCTION.GenerateOTPForEsingh("940396382715");

                //return COMMON_FUNCTION.GenerateOTPForEsingh(aadhaarId);

            }
            else
            {
                return "LOGOUT";
            }

            //return "RJVSD2343246566";
        }

        [AcceptVerbs("Post")]
        public string SaveFileData()
        {
            
            DataTable dataTable = new DataTable();
            var file = HttpContext.Request.Form["File"];
            string fileName = string.Empty;
            string pathString = "~/Content/SocietyProfiling/";
            int k = 1;
            try
            {
                foreach (string files in Request.Files)
                {
                    var httpPostedFile = Request.Files[files] as HttpPostedFileBase;

                    if (httpPostedFile != null)
                    {
                        if (k > 0)
                        {
                            fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(httpPostedFile.FileName);
                            var fileSave = System.Web.HttpContext.Current.Server.MapPath(pathString);
                            var fileSavePath = Path.Combine(fileSave, fileName);  // get the file path
                            if (!System.IO.File.Exists(fileSavePath))
                            {
                                httpPostedFile.SaveAs(fileSavePath); // save the file in the provided path
                                HttpResponse Response = System.Web.HttpContext.Current.Response;
                                Response.Clear();
                                Response.ContentType = "application/json; charset=utf-8";
                                Response.StatusDescription = "File uploaded succesfully#" + fileSavePath;
                                Response.End();
                                COMMON_FUNCTION.PDFResize(fileSavePath);
                            }
                            else
                            {
                                HttpResponse Response = System.Web.HttpContext.Current.Response;
                                Response.Clear();
                                Response.Status = "204 File already exists";
                                Response.StatusCode = 204;
                                Response.StatusDescription = "File already exists";
                                Response.End();
                            }
                        }
                        else
                        {

                            Stream stream = httpPostedFile.InputStream;
                            IExcelDataReader reader = null;
                            if (httpPostedFile.FileName.EndsWith(".xls"))
                            {
                                reader = ExcelReaderFactory.CreateBinaryReader(stream);
                            }
                            else if (httpPostedFile.FileName.EndsWith(".xlsx"))
                            {
                                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                            }
                            var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                                {
                                    UseHeaderRow = true
                                }
                            });
                            dataTable = result.Tables[0];
                            
                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                HttpResponse Response = System.Web.HttpContext.Current.Response;
                Response.Clear();
                Response.ContentType = "application/json; charset=utf-8";
                Response.StatusCode = 204;
                Response.Status = "204 No Content";
                Response.StatusDescription = e.Message;
                Response.End();
            }
            return Response.StatusDescription;
        }
    }
}