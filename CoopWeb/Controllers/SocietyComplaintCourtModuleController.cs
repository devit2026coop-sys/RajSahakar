using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Cooperative.Filters;
using Cooperative.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;


namespace Cooperative.Controllers
{
    public class SocietyComplaintCourtModuleController : Controller
    {

        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        // GET: SocietyComplaintCourtModule


        #region Society Complaint Court Module


        public ActionResult SocietyComplaintCourtModule()
        {
            //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ViewBag.LoggedInUserDistrictId = _LoggedInUser.DistrictId;
            ViewBag.UserId = _LoggedInUser.UserRoleDeptMappingId;
            return View();
        }

        //[HttpPost]
        //// [ValidateInput(true)]
        //[ValidateInputAttribute(true)]
        //public JsonResult SaveComplainUnderSection(ComplainUnderSectionModel ApplicationModel)
        //{

        //    string result = "";
        //    ComplainUnderSectionModel ApplicationModel1 = new ComplainUnderSectionModel();
        //    try
        //    {
        //       // var resultLetter = GenerateComplainLetter(ApplicationModel);
        //       // if (resultLetter.status == "1")
        //       // {
        //       //   ApplicationModel.ComplainLetter = resultLetter.data;
        //            ComplainUnderSection obj1 = new ComplainUnderSection(new DBLogger());
        //            result = obj1.InsertUpdateComplainUnderSection(ApplicationModel);
        //       // }
        //        return Json(new { Result = result }, JsonRequestBehavior.AllowGet);

        //    }
        //    catch (System.Exception ex)
        //    {
        //        result = "-1";
        //        DBLogger objDBLogger = new DBLogger();
        //        objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.ComplinDetails", FunctionName = "public JsonResult SaveComplainUnderSection()" }, ex);
        //    }
        //    string fileName = string.Empty;
        //    if (Request.Files != null && Request.Files.Count > 0)
        //    {
        //        try
        //        {
        //            string pathString = "";

        //            pathString = "~/Content/PLDBLoanApplication";

        //            if (!System.IO.Directory.Exists(pathString))
        //            {
        //                System.IO.Directory.CreateDirectory(Server.MapPath(pathString));
        //            }


        //            foreach (string files in Request.Files)
        //            {
        //                HttpPostedFileBase file = Request.Files[files];
        //                fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
        //                if (System.IO.Directory.Exists(Server.MapPath(pathString)))
        //                {
        //                    file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));
        //                }

        //                if (files.ToString().Contains("MemberPhoto"))
        //                {
        //                    string strid = files.ToString().Replace("MemberPhoto", "");
        //                    if (strid != "")
        //                    {
        //                        int idx = Convert.ToInt32(files.Split('_')[1]);
        //                        //Applicationdata.Members[idx].Photo = pathString.Replace("~", "..") + "/" + fileName;

        //                    }
        //                }

        //            }

        //        }
        //        catch (System.Exception ex)
        //        {
        //            result = "-1";
        //            DBLogger objDBLogger = new DBLogger();
        //            objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.SocietyComplaintCourtModController", FunctionName = "public JsonResult SaveLoanApplication()" }, ex);
        //        }
        //    }


        //    //var resultLetter = GenerateComplainLetter(ApplicationModel);

        //    //if (resultLetter.status == "1")
        //    //{
        //    //    ApplicationModel.ComplainLetter = resultLetter.data;
        //    //    ComplainUnderSection obj = new ComplainUnderSection(new DBLogger());
        //    //    result = obj.InsertUpdateComplainUnderSection(ApplicationModel);
        //    //}

        //    return Json(new { Result = result }, JsonRequestBehavior.AllowGet);

        //}

        [HttpPost]
        // [ValidateInput(true)]
        [ValidateInputAttribute(true)]
        public JsonResult UpdateComplainUnderSection(ComplainUnderSectionModel ApplicationModel)
        {

            string result = "";
            ComplainUnderSectionModel ApplicationModel1 = new ComplainUnderSectionModel();
            try
            {
                // var resultLetter = GenerateComplainLetter(ApplicationModel);
                // if (resultLetter.status == "1")
                // {
                //   ApplicationModel.ComplainLetter = resultLetter.data;
                ComplainUnderSection obj1 = new ComplainUnderSection(new DBLogger());
                result = obj1.UpdateComplainUnderSection(ApplicationModel);
                // }
                return Json(new { Result = result }, JsonRequestBehavior.AllowGet);

            }
            catch (System.Exception ex)
            {
                result = "-1";
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.ComplinDetails", FunctionName = "public JsonResult SaveComplainUnderSection()" }, ex);
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
                                //Applicationdata.Members[idx].Photo = pathString.Replace("~", "..") + "/" + fileName;

                            }
                        }

                    }

                }
                catch (System.Exception ex)
                {
                    result = "-1";
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.SocietyComplaintCourtModController", FunctionName = "UpdateComplainUnderSection(ComplainUnderSectionModel ApplicationModel)" }, ex);
                }
            }


            //var resultLetter = GenerateComplainLetter(ApplicationModel);

            //if (resultLetter.status == "1")
            //{
            //    ApplicationModel.ComplainLetter = resultLetter.data;
            //    ComplainUnderSection obj = new ComplainUnderSection(new DBLogger());
            //    result = obj.InsertUpdateComplainUnderSection(ApplicationModel);
            //}

            return Json(new { Result = result }, JsonRequestBehavior.AllowGet);

        }



        public ResultMessage GenerateComplainLetter(ComplainUnderSectionModel ApplicationModel)
        {
            ResultMessage result = new ResultMessage();
            try
            {


                OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                string city = _LoggedInUser.DistrictId > 0 ? _LoggedInUser.DISTRICT_MANGAL : "राजस्थान";
                string cityEn = _LoggedInUser.DistrictId > 0 ? _LoggedInUser.DISTRICT_ENG : "Rajasthan";
                //string cityType = ApplicationModel.CityType;
                string designation = (_LoggedInUser.DistrictId > 0 ? "उप " : "") + "रजिस्ट्रार";
                string designationEn = (_LoggedInUser.DistrictId > 0 ? "Deputy " : "") + "Registrar";
                string societyName = ApplicationModel.SocietyName;
                string lbl_Kramank = "COOP/" + city + "/धारा 55/" + DateTime.Now.Year + "/200003";
                string period = ApplicationModel.IODetailList[0].Period;
                string IOUser = string.IsNullOrWhiteSpace(ApplicationModel.IODetailList[0].OfficerNameHi) || ApplicationModel.IODetailList[0].OfficerNameHi == "null" ? ApplicationModel.IODetailList[0].OfficerNameEn : ApplicationModel.IODetailList[0].OfficerNameHi;
                string drName = string.IsNullOrWhiteSpace(_LoggedInUser.OfficerNameHi) || _LoggedInUser.OfficerNameHi == "null" ? _LoggedInUser.OfficerNameEn : _LoggedInUser.OfficerNameHi;
                //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                string devFontSize = "17px";
                string fontSize = "font-size: 13px;";
                sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:10px'>");
                sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px'>");
                sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                sb.Append("<img style='width:100px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यालय-" + designation + ", सहकारी समितियॉं, " + city) + "</label></b>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");
                sb.Append("<b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("जाँच आदेश") + "</label></b><br/>");
                sb.Append("<b><label>(राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 55 के अंतर्गत)</label></b></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
                sb.Append("<table style='width:100%'>");
                sb.Append("<tr>");
                sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'><b> क्रमांक :- " + lbl_Kramank + "</b></td>");
                sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> दिनांक :- " + DateTime.Now.ToString("dd-MM-yyyy") + " </td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='" + fontSize + "'><p style='text-indent: 25px;'> " + societyName + " के सम्बन्ध में शिकायत प्राप्त होने पर राजस्थान सहकारी सोसाइटी  अधिनियम 2001 की धारा 55 के तहत निम्नानुसार बिंदुओं पर जाँच करवाया जाना उचित समझा गया हैं :- </p>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='" + fontSize + "'> " + ApplicationModel.ComplaintDetails + "");
                sb.Append("</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td style='" + fontSize + "'><p style='text-indent: 25px;'> अतः मैं " + drName + "    ,  " + designation + " , सहकारी समितियाँ, " + city + " मुझ में  निहित राजस्थान सहकारी सोसाइटी अधिनियम 2001 की धारा 55 के तहत्त प्रदत्त शक्तियों का प्रयोग करते हुए उक्त बिंदुंओ की जांच हेतु श्री / श्रीमती " + IOUser + ",  को जांच अधिकारी नियुक्त करता हूं। जांच अधिकारी जांच कार्यवाही पूर्ण करते हुए जांच रिपोर्ट " + period + " दिन की अवधि में  आवश्यक रूप से प्रस्तुत करें।  </p>");
                sb.Append("</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td style='" + fontSize + "'><p style='text-indent: 25px;'> यह जांच आदेश आज दिनांक <b>" + DateTime.Now.ToString("dd/MM/yyyy") + "</b> को मेरे हस्ताक्षर एवं कार्यालय मुद्रा से जारी किया गया हैं। </p>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='" + fontSize + ";text-align:right;'> " + designation + ",<br> सहकारी समितियॉं, " + city + " ");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'><br/><br/><br/></td></tr>");
                sb.Append("</table>");
                sb.Append("</div>");

                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));

                string filepath = Server.MapPath("~/Content/CCSComplaintFiles");

                string fileName = Guid.NewGuid().ToString() + ".pdf";



                if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + fileName, ""))
                {


                    result = COMMON_FUNCTION.EsinghPDF(ApplicationModel.TXN, filepath + "/" + fileName, cityEn, designationEn, "350", "30", "150", "30");
                }
                result.data = fileName;



            }
            catch (System.Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "SocietyComplaintCourtModController", FunctionName = "GenerateComplainLetter" }, ex);

            }
            return result;
            //return result;
        }

        public string GetApprovedSocietyList1()
        {
            ComplainUnderSection objModel = new ComplainUnderSection();


            DataTable DT = objModel.GetApprovedSocietyList();

            return JsonConvert.SerializeObject(DT, Newtonsoft.Json.Formatting.Indented);

        }
        public JsonResult GetApprovedSocietyList(DataPagingModel Details, string RequestType)
        {
            ComplainUnderSection objModel = new ComplainUnderSection();


            //DataTable DT = objModel.GetApprovedSocietyList();

            //return JsonConvert.SerializeObject(DT, Newtonsoft.Json.Formatting.Indented);


            DataTable DT = objModel.GetApprovedSocietyList(ref Details, RequestType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);


        }



        public ActionResult SocietyComplaintAssignIO()
        {
            //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ViewBag.LoggedInUserDistrictId = _LoggedInUser.DistrictId;
            return View();
        }

        public ActionResult SocietyComplaintAssignIO1()
        {
            //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ViewBag.LoggedInUserDistrictId = _LoggedInUser.DistrictId;
            return View();
        }

        [HttpPost]
        [ValidateInputAttribute(true)]
        public JsonResult SaveIODetails(ComplainUnderSectionModel ApplicationModel)
        {

            string result = "";
            ComplainUnderSectionModel ApplicationModel1 = new ComplainUnderSectionModel();
            try
            {

                //if (ApplicationModel.ComplainTrailList[0].ComplaintStatusId == "3")
                //{
                //    //ApplicationModel.IOComplainLetter = letterResult.data;
                //    ComplainUnderSection obj1 = new ComplainUnderSection(new DBLogger());
                //    result = obj1.InsertUpdateIODetail(ApplicationModel);
                //}
                //else {
                //    var letterResult = GenerateComplainLetterIO(ApplicationModel);
                //    if (letterResult.status == "1")
                //    {
                //ApplicationModel.IOComplainLetter = letterResult.data;
                ComplainUnderSection obj1 = new ComplainUnderSection(new DBLogger());
                result = obj1.InsertUpdateIODetail(ApplicationModel);
                //}
                //}
                return Json(new { Result = result }, JsonRequestBehavior.AllowGet);

            }
            catch (System.Exception ex)
            {
                result = "-1";
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.ComplinDetails", FunctionName = "public JsonResult SaveComplainUnderSection()" }, ex);
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
                                //Applicationdata.Members[idx].Photo = pathString.Replace("~", "..") + "/" + fileName;

                            }
                        }

                    }

                }
                catch (System.Exception ex)
                {
                    result = "-1";
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.SocietyComplaintCourtModController", FunctionName = " SaveIODetails(ComplainUnderSectionModel ApplicationModel)" }, ex);
                }
            }

            return Json(new { Result = result }, JsonRequestBehavior.AllowGet);

        }

        public ResultMessage GenerateComplainLetterIO(ComplainUnderSectionModel ApplicationModel)
        {
            ResultMessage result = new ResultMessage();
            try
            {


                OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                string city = "जयपुर";
                string cityEn = "Jaipur";
                string cityType = "शहर";
                string designation = "उप रजिस्ट्रार";
                string designationEn = "Deputy Registrar";
                string societyName = "गृह निर्माण सहकारी समिति लि. जयपुर की योजना...";
                string lbl_Kramank = "COOP/2019/KOTA/200003";
                string oldKramank = "COOP/2020/Jaipur/23244";
                string oldDinank = "08/04/2020";
                //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                string devFontSize = "17px";
                string fontSize = "font-size: 13px;";
                sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:30px'>");
                sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px'>");
                sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                sb.Append("<img style='width:100px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("जांच अधिकारी एवं कार्यालय-" + designation + ", सहकारी, समितियां, " + city + " (" + cityType + ")") + "</label></b>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");
                sb.Append("<b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("नोटिस") + "</label></b><br/>");
                sb.Append("<b><label style='" + fontSize + "'>(राजस्थान सहकारी सोसायटी अधिनियम 2001 की धारा 55(2) (क) के अंतर्गत)</label></b></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
                sb.Append("<table style='width:100%'>");
                sb.Append("<tr>");
                sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'> क्रमांक :- " + lbl_Kramank + "</td>");
                sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> दिनांक :- " + DateTime.Now.ToString("dd-MM-yyyy") + " </td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:left;" + fontSize + "'><br/><br/><b> अध्यक्ष / मंत्री<br/> " + societyName + "</b></td>");
                sb.Append("</tr>");

                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'>" + designation + " सहकारी समितियां, " + city + " (" + cityType + ") के जाँच आदेश क्रमांक <b>" + oldKramank + "</b> दिनांक <b>" + oldDinank + "</b> के द्वारा अधोहस्ताक्षरकर्ता को " + societyName + " के सम्बन्ध में राजस्थान सहकारी सोसाइटी अधिनियम 2001 की धारा 55 के अंतर्गत निम्न बिंदुओं पर जाँच हेतु जाँच अधिकारी नियुक्त किया गया हैं :-  <br/> <label style='width:35px;text-decoration: underline;'> जाँच बिंदु </label> :- ");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='padding: 15px; line-height: 30px;" + fontSize + "'> " + ApplicationModel.IODetailList[0].IODetails + "");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='padding: 15px; line-height: 30px;" + fontSize + "'> यह नोटिस आज दिनांक <b>" + DateTime.Now.ToString("dd/MM/yyyy") + "</b> को मेरे हस्ताक्षर से जारी किया गया। ");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='padding: 15px; line-height: 30px;" + fontSize + ";text-align:right;'> जांच अधिकारी एवं कार्यालय-" + designation + ",<br/> सहकारी, समितियां, " + city + "(" + cityType + ")");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'><br/><br/><br/></td></tr>");
                sb.Append("</table>");
                sb.Append("</div>");
                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));

                string filepath = Server.MapPath("~/Content/CCSComplaintFiles");

                string fileName = Guid.NewGuid().ToString() + ".pdf";



                if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + fileName, ""))
                {


                    result = COMMON_FUNCTION.EsinghPDF(ApplicationModel.TXN, filepath + "/" + fileName, cityEn, designationEn, "350", "30", "150", "30");
                }
                result.data = fileName;



            }
            catch (System.Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "SocietyController", FunctionName = "GenerateComplainLetter" }, ex);

            }
            return result;
            //return result;
        }


        //public string GetApprovedSocietyAssignIOList()
        //{
        //    ComplainUnderSection objModel = new ComplainUnderSection();


        //    DataSet DS = objModel.GetSocietyComplianCourtModuleDetailsForIO();

        //    return JsonConvert.SerializeObject(DS, Newtonsoft.Json.Formatting.Indented);

        //}

        public JsonResult GetApprovedSocietyAssignIOList(DataPagingModel Details, string RequestType)
        {
            ComplainUnderSection objModel = new ComplainUnderSection();


            //DataTable DT = objModel.GetApprovedSocietyList();

            //return JsonConvert.SerializeObject(DT, Newtonsoft.Json.Formatting.Indented);


            DataTable DT = objModel.GetSocietyComplianCourtModuleDetailsForIO(ref Details, RequestType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public string UploadComplainFile()
        {
            string fileName = string.Empty;

            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {

                    string fileDirectory = Server.MapPath("~/Content/CCSComplaintFiles");

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
        public string UploadComplainFile2()
        {
            string fileName = string.Empty;

            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {

                    string fileDirectory = Server.MapPath("~/Content/CCSComplaintFiles");

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

        public JsonResult GetComplainDetails(int ComplainID)
        {
            ComplainUnderSection objModel = new ComplainUnderSection();


            DataSet dt = objModel.GetComplainDetails(ComplainID);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(dt) }, JsonRequestBehavior.AllowGet);

        }


        public JsonResult GetComplainStatus(int ActionId)
        {
            ComplainUnderSection objModel = new ComplainUnderSection();


            DataTable dt = objModel.GetComplainStatus(ActionId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(dt) }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetComplainTrail(int ComplainID)
        {
            ComplainUnderSection objModel = new ComplainUnderSection();
            DataTable dt = objModel.GetComplainTrail(ComplainID);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(dt) }, JsonRequestBehavior.AllowGet);

        }



        [HttpPost]
        public JsonResult GetSocietydetailsData(DataPagingModel Details, string Districtid = "", string SocietyTypeid = "", string Classid = "", string Subclassid = "", string CategoryId = "", string Searchbytext = "", string Type="1")
        {
            DataTable Dt = null;
            ComplainUnderSection objSocietyModel = new ComplainUnderSection();

            Dt = objSocietyModel.GetSocietydetailsDatalist(Details, Districtid, SocietyTypeid, Classid, Subclassid,CategoryId, Searchbytext, Type);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(Dt), DataPaging = Details }, JsonRequestBehavior.AllowGet);

            // return COMMON_FUNCTION.DataTableToJSONWithJSONNet(Dt);
        }


        [HttpPost]
        // [ValidateInput(true)]
        [ValidateInputAttribute(true)]
        public JsonResult SaveComplainUnderSection(ComplainUnderSectionModel ApplicationModel)
        {

            string result = "";
            ComplainUnderSectionModel ApplicationModel1 = new ComplainUnderSectionModel();
            try
            {
                // var resultLetter = GenerateComplainLetter(ApplicationModel);
                // if (resultLetter.status == "1")
                // {
                //   ApplicationModel.ComplainLetter = resultLetter.data;
                ComplainUnderSection obj1 = new ComplainUnderSection(new DBLogger());
                result = obj1.InsertUpdateComplainUnderSection(ApplicationModel);
                // }
                return Json(new { Result = result }, JsonRequestBehavior.AllowGet);

            }
            catch (System.Exception ex)
            {
                result = "-1";
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.ComplinDetails", FunctionName = "public JsonResult SaveComplainUnderSection()" }, ex);
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
                                //Applicationdata.Members[idx].Photo = pathString.Replace("~", "..") + "/" + fileName;

                            }
                        }

                    }

                }
                catch (System.Exception ex)
                {
                    result = "-1";
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.SocietyComplaintCourtModController", FunctionName = "SaveComplainUnderSection(ComplainUnderSectionModel ApplicationModel)" }, ex);
                }
            }



            return Json(new { Result = result }, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        [ActionName("GetinveticationListUser")]
        public string GetinveticationListUser(string DesignationLevelMappingId, int UserId)
        {


            ComplainUnderSection ApplicationModel1 = new ComplainUnderSection();
            //UserRegistrationModel objUserRoleModel = new UserRegistrationModel(new DBLogger());



            return COMMON_FUNCTION.DataTableToJSONString(ApplicationModel1.GetInvestiUser(DesignationLevelMappingId, UserId));
        }




        #endregion


        #region Section55ComplaintList

        public ActionResult Section55ComplaintList()
        {
            return View();

        }

        public JsonResult GetSocietyComplaintList(DataPagingModel Details, string RequestType)
        {

            ComplainUnderSection _ObjCitizenModel = new ComplainUnderSection(new FileLogger());
            string SSOID = _LoggedInUser.SSOID;
            DataTable DT = _ObjCitizenModel.SocietyComplaintList1(ref Details, RequestType, SSOID);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);


        }

        #endregion Section55ComplaintList

        public JsonResult GetAdministrativeDetailsById(int SocietyId)
        {

            ComplainUnderSection _ObjCitizenModel = new ComplainUnderSection(new FileLogger());
            string SSOID = _LoggedInUser.SSOID;
            DataTable DT = _ObjCitizenModel.GetAdministrativeDetailsById(SocietyId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSection55Memberss(int ComplainID)
        {

            ComplainUnderSection _ObjCitizenModel = new ComplainUnderSection(new FileLogger());
            string SSOID = _LoggedInUser.SSOID;
            DataTable DT = _ObjCitizenModel.GetSection55Memberss(ComplainID);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }
    }
}