using Cooperative.Models;
using javax.xml.stream;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static iTextSharp.text.pdf.PdfDocument;

namespace Cooperative.Controllers
{
    public class FertilizerSupplyController : Controller
    {
        // GET: FertilizerSupply
        public ActionResult Index()
        {
            return View();
        }

        public string GetAllOrganisations()
        {
            OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(obj.GetAllOrganisations(" and OrganisationId=2"));
        }

        #region GSS Demand

        [HttpPost]
        public string GetFertilizerList()
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objNew.GetFertilizerList());
        }


        public ActionResult GSSDemand()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GSSDemand(GSSDemandModel Details)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            Details.RequestType = 1;
            Details.KVSSId = 0;
            int result = objNew.InsertGSSDemand(Details);
            return Json(new { Status = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GSSDEmandList(DataPagingModel Details)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            string SSOID = "0";
            if (_LoggedInUser.UserType == "C")
            {
                SSOID = _LoggedInUser.SSOID;
            }

            DataTable DT = objNew.GSSDemandList(ref Details, SSOID);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
            //return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objNew.GSSDEmandList(_LoggedInUser.SSOID));
        }
        public ActionResult GSSReport()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetGSSDemandReport(DataPagingModel Details, string Type)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            string SSOID = "0";
            if (_LoggedInUser.UserType == "C")
            {
                SSOID = _LoggedInUser.SSOID;
            }
            DataTable DT = objNew.GetGSSDemandReport(ref Details, SSOID, Type);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
            //return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objNew.GSSDEmandList(_LoggedInUser.SSOID));
        }

        [HttpPost]
        public string GetGSSReport(DataPagingModel Details, string Type)
        {
            string result = "";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return null;
                }
                FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());


                string SSOID = "0";
                if (_LoggedInUser.UserType == "C")
                {
                    SSOID = _LoggedInUser.SSOID;
                }


                Details.PageNumber = 1;
                Details.IsAllRecords = true;

                DataTable List = objNew.GetGSSDemandReport(ref Details, SSOID, Type);

                //DataTable List = objNew.OrderDetails(ref Details, OrderNo);
                //DataTable TransportList = objNew.GetTransportList(ref Details).AsEnumerable().Where(row => row.Field<int>("transport_id") == TransportId).CopyToDataTable();


                object sumOrderQty;
                sumOrderQty = List.Compute("Sum(quantity)", string.Empty);
                OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                var orgLig = obj.GetAllOrganisations(" and OrganisationId=2");
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><head><style>.lable{font-size:30px!important;}</style></head><body>");
                sb.Append("<div id='divtoOrderPrint' ng-if='ShowOrderDetails' style='padding-left:17%; padding-right:17%;'>");
                sb.Append("<div style='margin-right:25px;    margin-left:25px;    font-size:23px;    padding:30px;'>");
                sb.Append("<table id='content' border='0' style='width:100%;' cellpadding='5' cellspacing='0'>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:center;'>");
                sb.Append("<label style='font-size: 20px;'> " + List.Rows[0]["SocietyName"] + ", " + List.Rows[0]["Address"] + " </label>");
                //sb.Append("<label style='font-size: 45px!important;'> राजस्थान राज्य  सहकारी  क्रय-विक्रय  संघ  लि </label><br/>");
                sb.Append("</td>");
                sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td colspan='2' style='text-align:center;border-bottom: 2px solid black;'>");
                ////sb.Append(" " + orgLig.Rows[0]["Address_Hi"] + " ");
                //sb.Append("</td>");
                //sb.Append("</tr>");                   
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style=''>");
                sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;'>");
                sb.Append("<tr><th style='text-align:center'> क्र. सं. </th><th style='text-align:center'> " + (Type == "FY" ? "वित्तीय वर्ष" : Type == "Yearly" ? "वर्ष" : Type == "Month" ? "माह" : "दिनांक") + " </th><th  style='text-align:center'> उर्वरक </th><th style='text-align:center'> मात्रा </th><th  style='text-align:center'> यूनिट </th></tr>");
                int index = 1;

                foreach (DataRow dr in List.Rows) // search whole table
                {
                    sb.Append("<tr><td style='text-align:center'>" + (index++) + "</td><td  style='text-align:center'> " + dr["FY"].ToString() + " </td><td style='text-align:center'> " + dr["name_hi"].ToString() + " </td><td style='text-align:center'> " + dr["quantity"].ToString() + " </td><td style='text-align:center'> " + dr["unit_hi"].ToString() + " </td></tr>");
                }

                sb.Append("<tr><td></td><td></td><td style='text-align:center;'><b> योग </b></td><td style='text-align:center'><b>" + sumOrderQty.ToString() + "</b></td> <td> </td></tr>");

                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                //sb.Append("<td colspan='2' style=''> अतः उपरोक्तानुसार माँग अनुसार " + List.Rows[0]["fertilizer_name_hi"] + " आपूर्ति करवाने का कष्ट  करें। </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                //sb.Append("<td colspan='2' style='text-align:left;'>संलग्नानुसार |</td>");
                sb.Append("</tr>");
                //sb.Append("<tr><td colspan='2' style='text-align:right;'><br /><br /> भवदीय </td></tr>");
                //sb.Append("<tr><td colspan='2' style='text-align:right;'><br />सहा. प्रबन्धक (कृषि आदान) </td></tr>");
                //sb.Append("<tr><td colspan='2' style='text-align:left;'>प्रतिलिपि:- </td></tr>");
                //sb.Append("<tr>");
                //sb.Append("<td colspan='2' style='text-align:left;padding-left: 45px;'>");
                //sb.Append("<ol>");
                //sb.Append("<li>संयुक्त  निदेशक ,(रसायन ) कृषि  आयुक्तालय ,राजस्थान , जयपुर</li>");
                //sb.Append("<li>उपप्रबंधक ,आरएसएमएमएल, प्लाट न. 02  गांधीनगर  स्कीम ,बीकानेर को  भेजकर निवेदन है  कि  उक्तानुसार  राजफैड  के  ट्रांसपोर्टर  को {{GSSList[0].fertilizer_name_hi}} उपलब्ध  करवाने का कष्ट  करें |</li>");
                //sb.Append("<li>उपनिदेशक कृषि (विस्तार), जिला  परिषद , सीकर  / बाड़मेर  |</li>");
                //sb.Append("<li>क्षेत्रीय  कार्यालय , अधिकारी , जयपुर /जोधपुर |</li>");
                //sb.Append("</ol>");
                //sb.Append("</td>");
                //sb.Append("</tr>");
                //sb.Append("<tr><td colspan='2' style='text-align:right;'>भवदीय</td></tr>");
                //sb.Append("<tr><td colspan='2' style='text-align:right;'>सहाO  प्रबन्धक (कृषि  आदान) </td></tr>");
                sb.Append("</table>");
                sb.Append("</div>");
                sb.Append("<div class='col-md-12' style='margin-top: 10px;' ng-if='ShowGenarateOrder'>");
                sb.Append("<div class='dev-menu-btn'>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</body></html>");


                //if (!System.IO.Directory.Exists(filepath))
                //{
                //    System.IO.Directory.CreateDirectory(filepath);
                //}
                ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                DataTable DT = new DataTable();

                string FileName = "RequestSupply_" + OrderNo;

                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();




                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true));
                var bytes = COMMON_FUNCTION.GePDFBytes(sb1, "");
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);

                //string result1 = System.Text.Encoding.UTF8.GetString(COMMON_FUNCTION.GePDFBytes(sb1, "", filepath));

                return base64String;



            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.FertilizerSupply", FunctionName = "GetGSSReport(DataPagingModel Details, string Type)" }, ex);
                result = "Error";
            }
            return null;
        }


        #endregion

        #region KVSS Request

        public ActionResult KVSSRequest()
        {
            return View();
        }



        [HttpPost]
        public ActionResult KVSSRequest(GSSDemandModel Details)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser.UserType == "C")
            {
                Details.SSOId = _LoggedInUser.SSOID;
            }
            int result = objNew.InsertGSSDemand(Details);
            return Json(new { Status = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult KVSSRequestList(DataPagingModel Details, string RequestType)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            string SSOID = "0";
            if (_LoggedInUser.UserType == "C")
            {
                SSOID = _LoggedInUser.SSOID;
            }
            //DataPagingModel Details = new DataPagingModel();
            // Details.SearchParameter.Add("SSOID", SSOID);

            DataTable DT = objNew.KVSSRequestList(ref Details, SSOID, RequestType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

            //return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objNew.KVSSRequestList(_LoggedInUser.SSOID));
        }
        [HttpPost]
        public JsonResult GetKVSSData(DataPagingModel Details, string RequestType)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            string SSOID = "0";
            if (_LoggedInUser.UserType == "C")
            {
                SSOID = _LoggedInUser.SSOID;
            }
            //DataPagingModel Details = new DataPagingModel();
            // Details.SearchParameter.Add("SSOID", SSOID);
            string ROId = "4";
            DataTable DT = objNew.GetKVSSData(ref Details, ROId, RequestType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

            //return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objNew.KVSSRequestList(_LoggedInUser.SSOID));
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

                //if (Request.Url.Host.Contains("localhost"))
                //{
                //    aadhaarId = "940396382715";


                //    //return COMMON_FUNCTION.GenerateOTPForEsingh("940396382715");
                //}


                return COMMON_FUNCTION.GenerateOTPForEsingh(_LoggedInUser.AadharNo);
            }
            else
            {
                return "LOGOUT";
            }
        }



        [HttpGet]
        [ActionName("GenerateAadhaarOTPForEsingDemo")]
        public string GenerateAadhaarOTPForEsingDemo()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser != null)
            {
                SSOWSSoap.SSOUserDetail usrdtl = new SSOWSSoap.SSOUserDetail();
                SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
                usrdtl = ssoclient.GetUserDetailXML(_LoggedInUser.SSOID, "RAJSAHKAR.TEST", "Rajasthan@12");
                return COMMON_FUNCTION.GenerateOTPForEsingh("428434778643");
            }
            else
            {
                return "LOGOUT";
            }

            //return "RJVSD2343246566";
        }



        public ActionResult RegionalOfficeRequest()
        {
            return View();
        }


        [HttpPost]
        public string SentRORequest(string OTP, string TXN, List<GSSDemandModel> ReqestData)
        {
            string result = "";
            try
            {



                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return "LOGOUT";
                }
                FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
                // List<GSSDemandModel> _Model = new List<GSSDemandModel>();
                //try
                //{
                //    System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
                //    _Model = Request.Form["ReqestData"].ToString().Equals("[]") ? new List<GSSDemandModel>() : (List<GSSDemandModel>)s.Deserialize(Request.Form["ReqestData"].ToString(), typeof(GSSDemandModel));
                //}
                //catch (System.Exception ex)
                //{
                //    //return CommonMessageFactory.Response("Provided input data is not in correct format.");
                //}
                var otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                if (otpResult.status != "1")
                {
                    return otpResult.data;
                }
                string requestNo = objNew.SendRORequest(ReqestData);
                if (result == "-1" && result == "0")
                {
                    result = "Error";
                }
                else
                {

                    DataPagingModel Details = new DataPagingModel();
                    Details.PageNumber = 1;
                    Details.IsAllRecords = true;
                    DataTable List = objNew.KVSSRequestDetails(ref Details, requestNo);

                    object sumOrderQty;
                    sumOrderQty = List.Compute("Sum(quantity)", string.Empty);
                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                    var orgLig = obj.GetAllOrganisations(" and OrganisationId=2");
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<html><body>");
                    sb.Append("<div id='divtoOrderPrint' ng-if='ShowOrderDetails' style='padding-left:17%; padding-right:17%;'>");
                    sb.Append("<div style='margin-right:25px;    margin-left:25px;    font-size:12px;    padding:30px;'>");
                    sb.Append("<table id='content' border='0' style='width:100%;font-size:12px;' cellpadding='0' cellspacing='0'>");
                    sb.Append("<tr>");
                    sb.Append("<th colspan='2' style='text-align:center;font-size:35px!important;'><b> " + UnicodeToKrutidev.GetKrutidev("क्षेत्रीय कार्यालय") + " , " + UnicodeToKrutidev.GetKrutidev(ReqestData[0].ROName) + " </b></th>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' style='text-align:left;border-bottom:1px solid black;padding-top:10px;padding-bottom:10px;'>");
                    //sb.Append(" " + orgLig.Rows[0]["Address_Hi"] + " ");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    //sb.Append("<tr><td colspan='2' style=''><br/> प्रधान कार्यालय : " + orgLig.Rows[0]["OrgFullName_Hi"] + " </td></tr>");
                    sb.Append("<tr><td style=''> क्रमांक  :  क्षे. का. रा.  / " + requestNo + "</td><td style='text-align:right;'>दिनांक : " + DateTime.Now.ToString("dd/MM/yyyy") + "</td></tr>");
                    sb.Append("<tr><td colspan='2' style=''> श्रीमान महाप्रबंधक (कृषि )<br /> राजफैड <br /> जयपुर </td></tr>");
                    sb.Append("<tr><td colspan='2' style='padding-left:65px;padding-top:25px;'><br/> विषय :- " + ReqestData[0].FertilizerName + " की  आपूर्ति  करने बाबत </td></tr>");
                    sb.Append("<tr><td colspan='2' style=''><br/> महोदय, </td></tr>");
                    sb.Append("<tr><td colspan='2' style=''><p style='text-indent: 65px;'> उपरोक्त विषयान्तर्गत निवेदन है कि निम्नलिखित ग्राम सेवा  सहकारी  समितियों द्वारा " + sumOrderQty.ToString() + "  मैं.टन " + ReqestData[0].FertilizerName + " की मांग दी है। </p></td></tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' style=''>");
                    sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;font-size:12px;'>");
                    sb.Append("<tr><th style='text-align:center'> क्र. सं. </th><th> समिति का नाम </th><th> मात्रा (में. टन) </th></tr>");
                    int index = 1;
                    foreach (var dr in ReqestData) // search whole table
                    {
                        sb.Append("<tr ng-repeat='item in OrderList'><td  style='text-align:center'>" + (index++) + "</td><td> " + dr.KVSSName.ToString() + " </td><td> " + dr.Quantity + "</td></tr>");
                    }
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' style=''><br/> कृपया उपरोक्तानुसार " + ReqestData[0].FertilizerName + " की आपूर्ति करवाने का कष्ट  करें। </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr><td colspan='2' style='text-align:right;'><br /><br /> भवदीय </td></tr>");
                    sb.Append("<tr><td colspan='2' style='text-align:right;'><br /> क्षेत्रीय अधिकारी </td></tr>");

                    //sb.Append("<tr><td colspan='2' style='text-align:right;'> भवदीय </td></tr>");
                    //sb.Append("<tr><td colspan='2' style='text-align:right;'> क्षेत्रीय अधिकारी </td></tr>");
                    sb.Append("</table>");
                    sb.Append("</div>");
                    sb.Append("<div class='col-md-12' style='margin-top: 10px;' ng-if='ShowGenarateOrder'>");
                    sb.Append("<div class='dev-menu-btn'>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</body></html>");


                    if (!System.IO.Directory.Exists(Server.MapPath("~/Content/FertilizerSupply")))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Content/FertilizerSupply"));
                    }
                    ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                    DataTable DT = new DataTable();

                    string FileName = "RORequest_" + requestNo;

                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "17px"));
                    if (COMMON_FUNCTION.GeneratePDF(sb1, Server.MapPath("~/Content/FertilizerSupply/" + FileName + ".pdf")))
                    {
                        //result = COMMON_FUNCTION.VerifyAadhaarOTPForEsingh(OTP, TXN, Server.MapPath("~/Content/FertilizerSupply/" + FileName + ".pdf"), true, FileName + ".pdf");
                        //filepath + "/" + FileName + ".pdf",
                        var esignResult = COMMON_FUNCTION.EsinghPDF(otpResult.data, Server.MapPath("~/Content/FertilizerSupply/") + "/" + FileName + ".pdf");
                        result = esignResult.data;


                        if (result == "Success")
                        {
                            var updateData = new List<GSSDemandModel>();
                            updateData.Add(new GSSDemandModel() { Request_No = requestNo, EntryFile = FileName + ".pdf" });
                            objNew.SendRORequest(updateData);
                        }


                        //objSocietyModel.UpdateElectionStatusFile("" + FileName + ".pdf", fileType, Convert.ToInt32(List.Rows[0]["SocietyId"].ToString()), Convert.ToInt32(List.Rows[0]["ElectionMasterId"].ToString()));
                        //var DT1 = objSocietyModel.GetElectionEmailSMSDetails(32, 4, ElectionMasterId, SocietyId);
                        //if (fileType == "MemberListFile")
                        //{
                        //    foreach (DataRow dr in DT1.Rows) // search whole table
                        //    {
                        //        new EsanchaarController().SendSocietySMS(dr["sms"].ToString().Replace("APPLICANTNAME", dr["OfficerNameEn"].ToString()), dr["MobileNo"].ToString());
                        //    }
                        //}

                    }
                    else
                    {
                        result = "PDF Not Generate";
                    }
                }

            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.FertilizerSupply", FunctionName = " SentRORequest(string OTP, string TXN, List<GSSDemandModel> ReqestData)" }, ex);
                result = "Error";
            }
            return result;
        }



        [HttpPost]
        public string GenerateRequest(string OTP, string TXN, List<GSSDemandModel> ReqestData)
        {
            string result = "";
            try
            {



                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return "LOGOUT";
                }
                FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
                // List<GSSDemandModel> _Model = new List<GSSDemandModel>();
                //try
                //{
                //    System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
                //    _Model = Request.Form["ReqestData"].ToString().Equals("[]") ? new List<GSSDemandModel>() : (List<GSSDemandModel>)s.Deserialize(Request.Form["ReqestData"].ToString(), typeof(GSSDemandModel));
                //}
                //catch (System.Exception ex)
                //{
                //    //return CommonMessageFactory.Response("Provided input data is not in correct format.");
                //}


                var otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                if (otpResult.status != "1")
                {
                    return otpResult.data;
                }


                string requestNo = objNew.InsertKVSSRequest(ReqestData);
                if (result == "-1" && result == "0")
                {
                    result = "Error";
                }
                else
                {


                    DataPagingModel Details = new DataPagingModel();
                    Details.PageNumber = 1;
                    Details.IsAllRecords = true;
                    DataTable List = objNew.KVSSRequestDetails(ref Details, requestNo);

                    object sumOrderQty;
                    sumOrderQty = List.Compute("Sum(quantity)", string.Empty);
                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                    var orgLig = obj.GetAllOrganisations(" and OrganisationId=2");
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<html><body>");
                    sb.Append("<div id='divtoOrderPrint' ng-if='ShowOrderDetails' style='padding-left:17%; padding-right:17%;'>");
                    sb.Append("<div style='margin-right:25px;    margin-left:25px;    font-size:12px;    padding:30px;'>");
                    sb.Append("<table id='content' border='0' style='width:100%;font-size:12px;' cellpadding='0' cellspacing='0'>");
                    sb.Append("<tr>");
                    sb.Append("<th colspan='2' style='text-align:center;font-size:35px!important;'><b> " + UnicodeToKrutidev.GetKrutidev(List.Rows[0]["KVSSName"].ToString()) + " , " + UnicodeToKrutidev.GetKrutidev(List.Rows[0]["DISTRICT_MANGAL"].ToString()) + " </b></th>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' style='text-align:left;border-bottom:1px solid black;padding-top:10px;padding-bottom:10px;'>");
                    //sb.Append(" " + orgLig.Rows[0]["Address_Hi"] + " ");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    //sb.Append("<tr><td colspan='2' style=''><br/> प्रधान कार्यालय : " + orgLig.Rows[0]["OrgFullName_Hi"] + " </td></tr>");
                    sb.Append("<tr><td style=''> क्रमांक  :  क्षे. का. रा.  / " + requestNo + "</td><td style='text-align:right;'>दिनांक : " + DateTime.Now.ToString("dd/MM/yyyy") + "</td></tr>");
                    sb.Append("<tr><td colspan='2' style=''> श्रीमान महाप्रबंधक (कृषि )<br /> राजफैड <br /> जयपुर </td></tr>");
                    sb.Append("<tr><td colspan='2' style='padding-left:65px;padding-top:25px;'><br/> विषय :- " + List.Rows[0]["fertilizer_name_hi"] + " की  आपूर्ति  करने बाबत </td></tr>");
                    sb.Append("<tr><td colspan='2' style=''><br/> महोदय, </td></tr>");
                    sb.Append("<tr><td colspan='2' style=''><p style='text-indent: 65px;'> उपरोक्त विषयान्तर्गत निवेदन है कि " + List.Rows[0]["KVSSName"] + " ने " + sumOrderQty.ToString() + "  मैं.टन " + List.Rows[0]["fertilizer_name_hi"] + " की मांग दी है। </p></td></tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' style=''>");
                    sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;font-size:12px;'>");
                    sb.Append("<tr><th style='text-align:center'> क्र. सं. </th><th> समिति का नाम </th><th> मात्रा <br/" +
                        "> (" + List.Rows[0]["fertilizer_unit_hi"] + ") </th><th> आपूर्ति स्थान </th><th> रुट चार्ट </th></tr>");
                    int index = 1;
                    foreach (DataRow dr in List.Rows) // search whole table
                    {
                        sb.Append("<tr ng-repeat='item in OrderList'><td  style='text-align:center'>" + (index++) + "</td><td> " + dr["SocietyName"].ToString() + " </td><td> " + dr["quantity"].ToString() + "</td><td> " + dr["AddressFor"].ToString() + " </td><td> " + dr["rout_chart"].ToString() + " </td></tr>");

                    }
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' style=''><br/> कृपया उपरोक्तानुसार " + List.Rows[0]["fertilizer_name_hi"] + " की आपूर्ति करवाने का कष्ट  करें। </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr><td colspan='2' style='text-align:right;'><br /><br /> भवदीय </td></tr>");
                    sb.Append("<tr><td colspan='2' style='text-align:right;'><br /> क्षेत्रीय अधिकारी </td></tr>");

                    //sb.Append("<tr><td colspan='2' style='text-align:right;'> भवदीय </td></tr>");
                    //sb.Append("<tr><td colspan='2' style='text-align:right;'> क्षेत्रीय अधिकारी </td></tr>");
                    sb.Append("</table>");
                    sb.Append("</div>");
                    sb.Append("<div class='col-md-12' style='margin-top: 10px;' ng-if='ShowGenarateOrder'>");
                    sb.Append("<div class='dev-menu-btn'>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</body></html>");


                    if (!System.IO.Directory.Exists(Server.MapPath("~/Content/FertilizerSupply")))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Content/FertilizerSupply"));
                    }
                    ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                    DataTable DT = new DataTable();

                    string FileName = "Request_" + requestNo;

                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "17px"));
                    if (COMMON_FUNCTION.GeneratePDF(sb1, Server.MapPath("~/Content/FertilizerSupply/" + FileName + ".pdf")))
                    {
                        //result = COMMON_FUNCTION.VerifyAadhaarOTPForEsingh(OTP, TXN, Server.MapPath("~/Content/FertilizerSupply/" + FileName + ".pdf"), true, FileName + ".pdf");
                        //filepath + "/" + FileName + ".pdf",
                        var esignResult = COMMON_FUNCTION.EsinghPDF(otpResult.data, Server.MapPath("~/Content/FertilizerSupply/") + "/" + FileName + ".pdf");
                        result = esignResult.data;


                        if (result == "Success")
                        {
                            objNew.UpdateKVSSRequest(requestNo, FileName + ".pdf", 1, 0);
                        }


                        //objSocietyModel.UpdateElectionStatusFile("" + FileName + ".pdf", fileType, Convert.ToInt32(List.Rows[0]["SocietyId"].ToString()), Convert.ToInt32(List.Rows[0]["ElectionMasterId"].ToString()));
                        //var DT1 = objSocietyModel.GetElectionEmailSMSDetails(32, 4, ElectionMasterId, SocietyId);
                        //if (fileType == "MemberListFile")
                        //{
                        //    foreach (DataRow dr in DT1.Rows) // search whole table
                        //    {
                        //        new EsanchaarController().SendSocietySMS(dr["sms"].ToString().Replace("APPLICANTNAME", dr["OfficerNameEn"].ToString()), dr["MobileNo"].ToString());
                        //    }
                        //}

                    }
                    else
                    {
                        result = "PDF Not Generate";
                    }
                }
            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.FertilizerSupply", FunctionName = " GenerateRequest(string OTP, string TXN, List<GSSDemandModel> ReqestData)" }, ex);
                result = "Error";
            }
            return result;
        }


        public ActionResult KVSSReport()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetKVSSReport(DataPagingModel Details, string Type, string ReportType)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            string SSOID = "0";
            if (_LoggedInUser.UserType == "C")
            {
                SSOID = _LoggedInUser.SSOID;
            }
            DataTable DT = objNew.GetKVSSReport(ref Details, SSOID, Type, ReportType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
            //return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objNew.GSSDEmandList(_LoggedInUser.SSOID));
        }

        [HttpPost]
        public string GetKVSSExportReport(DataPagingModel Details, string Type, string ReportType)
        {
            string result = "";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return null;
                }
                FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());


                string SSOID = "0";
                if (_LoggedInUser.UserType == "C")
                {
                    SSOID = _LoggedInUser.SSOID;
                }


                Details.PageNumber = 1;
                Details.IsAllRecords = true;

                DataTable List = objNew.GetKVSSReport(ref Details, SSOID, Type, ReportType);

                //DataTable List = objNew.OrderDetails(ref Details, OrderNo);
                //DataTable TransportList = objNew.GetTransportList(ref Details).AsEnumerable().Where(row => row.Field<int>("transport_id") == TransportId).CopyToDataTable();


                object sumOrderQty;
                sumOrderQty = List.Compute("Sum(quantity)", string.Empty);
                OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                var orgLig = obj.GetAllOrganisations(" and OrganisationId=2");
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><head><style>.lable{font-size:30px!important;}</style></head><body>");
                sb.Append("<div id='divtoOrderPrint' ng-if='ShowOrderDetails' style='padding-left:17%; padding-right:17%;'>");
                sb.Append("<div style='margin-right:25px;    margin-left:25px;    font-size:23px;    padding:30px;'>");
                sb.Append("<table id='content' border='0' style='width:100%;' cellpadding='5' cellspacing='0'>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:center;'>");
                sb.Append("<label style='font-size: 20px;'> " + List.Rows[0]["SocietyName"] + ", " + List.Rows[0]["Address"] + " </label>");
                //sb.Append("<label style='font-size: 45px!important;'> राजस्थान राज्य  सहकारी  क्रय-विक्रय  संघ  लि </label><br/>");
                sb.Append("</td>");
                sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td colspan='2' style='text-align:center;border-bottom: 2px solid black;'>");
                ////sb.Append(" " + orgLig.Rows[0]["Address_Hi"] + " ");
                //sb.Append("</td>");
                //sb.Append("</tr>");                   
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style=''>");
                sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;'>");
                sb.Append("<tr><th style='text-align:center'> क्र. सं. </th>");
                if (ReportType == "Quantity")
                {
                    sb.Append("<th style='text-align:left'> " + (Type == "FY" ? "वित्तीय वर्ष" : Type == "Yearly" ? "वर्ष" : Type == "Month" ? "माह" : "दिनांक") + " </th>");
                }
                if (ReportType != "Quantity" && Type == "Date")
                {
                    sb.Append("<th style='text-align:left'> दिनांक </th>");
                }
                if (ReportType != "Quantity")
                {
                    sb.Append("<th style='text-align:left'> जीएसएस का नाम </th>");
                }
                sb.Append("<th  style='text-align:center'> उर्वरक </th>");
                sb.Append("<th style='text-align:center'> मात्रा </th>");
                sb.Append("<th  style='text-align:center'> यूनिट </th></tr>");
                int index = 1;

                foreach (DataRow dr in List.Rows) // search whole table
                {
                    //sb.Append("<tr><td style='text-align:center'>" + (index++) + "</td><td  style='text-align:center;" + (ReportType != "GSSWise" ? "" : "display:none") + "'> " + dr["FY"].ToString() + " </td>" + (ReportType == "GSSWise" ? "<th style='text-align:left'> " + dr["SocietyName"].ToString() + " </th>" : "") + "<td style='text-align:center'> " + dr["name_hi"].ToString() + " </td><td style='text-align:center'> " + dr["quantity"].ToString() + " </td><td style='text-align:center'> " + dr["unit_hi"].ToString() + " </td></tr>");

                    sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                    if (ReportType == "Quantity")
                    {
                        sb.Append("<td style='text-align:left'> " + dr["FY"].ToString() + " </td>");
                    }
                    if (ReportType != "Quantity" && Type == "Date")
                    {
                        sb.Append("<td style='text-align:left'> " + dr["FY"].ToString() + " </td>");
                    }
                    if (ReportType != "Quantity")
                    {
                        sb.Append("<td style='text-align:left'> " + dr["SocietyName"].ToString() + " </td>");
                    }
                    sb.Append("<td  style='text-align:center'> " + dr["name_hi"].ToString() + " </td>");
                    sb.Append("<td style='text-align:center'> " + dr["quantity"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:center'> " + dr["unit_hi"].ToString() + " </td></tr>");

                }


                sb.Append("<tr><td style='text-align:center'>  </td>");
                if (ReportType == "Quantity")
                {
                    sb.Append("<td style='text-align:left'> </td>");
                }
                if (ReportType != "Quantity" && Type == "Date")
                {
                    sb.Append("<td style='text-align:left'>  </td>");
                }
                if (ReportType != "Quantity")
                {
                    sb.Append("<td style='text-align:left'>  </td>");
                }
                sb.Append("<td  style='text-align:center'> <b> योग </b> </td>");
                sb.Append("<td style='text-align:center'> <b>" + sumOrderQty.ToString() + "</b> </td>");
                sb.Append("<td  style='text-align:center'>  </td></tr>");











                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                //sb.Append("<td colspan='2' style=''> अतः उपरोक्तानुसार माँग अनुसार " + List.Rows[0]["fertilizer_name_hi"] + " आपूर्ति करवाने का कष्ट  करें। </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                //sb.Append("<td colspan='2' style='text-align:left;'>संलग्नानुसार |</td>");
                sb.Append("</tr>");
                //sb.Append("<tr><td colspan='2' style='text-align:right;'><br /><br /> भवदीय </td></tr>");
                //sb.Append("<tr><td colspan='2' style='text-align:right;'><br />सहा. प्रबन्धक (कृषि आदान) </td></tr>");
                //sb.Append("<tr><td colspan='2' style='text-align:left;'>प्रतिलिपि:- </td></tr>");
                //sb.Append("<tr>");
                //sb.Append("<td colspan='2' style='text-align:left;padding-left: 45px;'>");
                //sb.Append("<ol>");
                //sb.Append("<li>संयुक्त  निदेशक ,(रसायन ) कृषि  आयुक्तालय ,राजस्थान , जयपुर</li>");
                //sb.Append("<li>उपप्रबंधक ,आरएसएमएमएल, प्लाट न. 02  गांधीनगर  स्कीम ,बीकानेर को  भेजकर निवेदन है  कि  उक्तानुसार  राजफैड  के  ट्रांसपोर्टर  को {{GSSList[0].fertilizer_name_hi}} उपलब्ध  करवाने का कष्ट  करें |</li>");
                //sb.Append("<li>उपनिदेशक कृषि (विस्तार), जिला  परिषद , सीकर  / बाड़मेर  |</li>");
                //sb.Append("<li>क्षेत्रीय  कार्यालय , अधिकारी , जयपुर /जोधपुर |</li>");
                //sb.Append("</ol>");
                //sb.Append("</td>");
                //sb.Append("</tr>");
                //sb.Append("<tr><td colspan='2' style='text-align:right;'>भवदीय</td></tr>");
                //sb.Append("<tr><td colspan='2' style='text-align:right;'>सहाO  प्रबन्धक (कृषि  आदान) </td></tr>");
                sb.Append("</table>");
                sb.Append("</div>");
                sb.Append("<div class='col-md-12' style='margin-top: 10px;' ng-if='ShowGenarateOrder'>");
                sb.Append("<div class='dev-menu-btn'>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</body></html>");


                //if (!System.IO.Directory.Exists(filepath))
                //{
                //    System.IO.Directory.CreateDirectory(filepath);
                //}
                ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                DataTable DT = new DataTable();

                string FileName = "RequestSupply_" + OrderNo;

                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();




                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true));
                var bytes = COMMON_FUNCTION.GePDFBytes(sb1, "");
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);

                //string result1 = System.Text.Encoding.UTF8.GetString(COMMON_FUNCTION.GePDFBytes(sb1, "", filepath));

                return base64String;



            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.FertilizerSupply", FunctionName = "GetKVSSExportReport(DataPagingModel Details, string Type, string ReportType)" }, ex);
                result = "Error";
            }
            return null;
        }

        #endregion

        #region Director Agriculture
        public ActionResult AgricultureDirector()
        {
            return View();
        }

        [HttpPost]
        public string GetKVSSByDistrictId()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser == null)
            {
                return "";
            }
            int DistrictId = 22;//  _LoggedInUser.DistrictId;
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objNew.GetKVSSByDistrictId(DistrictId));
        }

        [HttpPost]
        public string GetGSSByBlockId(int BlockId)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objNew.GetGSSByBlockId(BlockId));
        }
        [HttpPost]
        public string GetGSSBySSOId()
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            string SSOID = "0";
            if (_LoggedInUser.UserType == "C")
            {
                SSOID = _LoggedInUser.SSOID;
            }
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objNew.GetGSSBySSOId(SSOID));
        }
        // New Add 01-03-2022 
        [HttpPost]
        public string GetGSSORKVSSBySSOId(string TypeName)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            string SSOID = "0";
            if (_LoggedInUser.UserType == "C")
            {
                SSOID = _LoggedInUser.SSOID;
            }
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objNew.GetGSSKVSSBySSOId(SSOID, TypeName));
        }

        [HttpPost]
        public string GetKVSSList()
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objNew.GetKVSSList());
        }

        [HttpPost]
        public string GetConstituencyListByDistrictId(string DistrictId)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objNew.GetConstituencyListByDistrictId(Convert.ToInt32(DistrictId)));
        }
        [HttpPost]
        public string GenerateDirectorDemand(string OTP, string TXN, List<GSSDemandModel> ReqestData)
        {
            string result = "";
            string requestNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return "LOGOUT";
                }
                FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());


                // string requestNo = objNew.InsertKVSSRequest(ReqestData);
                var otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                if (otpResult.status != "1")
                {
                    return otpResult.data;
                }

                List<GSSDemandModel> request = new List<GSSDemandModel>();

                ReqestData.ForEach(x =>
                {
                    x.SSOId = "";
                    int demandId = objNew.InsertGSSDemand(x);
                    request.Add(new GSSDemandModel { DemandId = demandId, RequestId = 2 });
                });

                requestNo = objNew.InsertKVSSRequest(request);


                if (result == "-1" && result == "0")
                {
                    result = "Error";
                }
                else
                {

                    DataPagingModel Details = new DataPagingModel();
                    Details.PageNumber = 1;
                    Details.IsAllRecords = true;
                    DataTable List = objNew.GetDirectorDemandDetails(ref Details, requestNo, "New");
                    //DataTable TransportList = objNew.GetTransportList().AsEnumerable().Where(row => row.Field<int>("transport_id") == TransportId).CopyToDataTable();


                    object sumOrderQty;
                    sumOrderQty = List.Compute("Sum(quantity)", string.Empty);
                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                    var orgLig = obj.GetAllOrganisations(" and OrganisationId=2");
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<html><head><style>.lable{font-size:30px!important;}</style></head><body>");
                    sb.Append("<div id='divtoOrderPrint' ng-if='ShowOrderDetails' style='padding-left:17%; padding-right:17%;'>");
                    sb.Append("<div style='margin-right:25px;    margin-left:25px;    font-size:23px;    padding:30px;'>");
                    sb.Append("<table id='content' border='0' style='width:100%;' cellpadding='5' cellspacing='0'>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' style='text-align:center;font-size: 20px;'> राजस्थान सरकार </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' style='text-align:center;font-size: 30px;'> कार्यालय सहायक  निदेशक कृषि (विस्तार) " + List.Rows[0]["BLOCK_MANGAL"] + ",  " + List.Rows[0]["DISTRICT_MANGAL"] + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' style='text-align:left;border-bottom: 2px solid black;'>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr><td style=''> क्रमांक  :-  " + (DateTime.Now.ToString("yyyy") + "-" + (Convert.ToInt16(DateTime.Now.ToString("yy")) + 1)) + " / </td><td style='text-align:right;'> दिनांक : " + DateTime.Now.ToString("dd/MM/yyy") + "</td></tr>");
                    sb.Append("<tr><td colspan='2' style=''> श्रीमान प्रबन्धक महोदय, राजफैड, <br />" + orgLig.Rows[0]["Address_Hi"] + " </td></tr>");

                    sb.Append("<tr><td colspan='2' ><br/><p style='padding-left:65px;padding-top:25px;'>विषय :-   " + List.Rows[0]["fertilizer_name_hi"] + " की माँग बाबत </p></td></tr>");

                    sb.Append("<tr><td colspan='2' style=''>महोदय</td></tr>");
                    sb.Append("<tr><td colspan='2' style=''><p style='text-indent: 65px;'>उपरोक्त विषयान्तर्गत निवेदन है कि वर्ष " + (DateTime.Now.ToString("yyyy") + "-" + (Convert.ToInt16(DateTime.Now.ToString("yy")) + 1)) + " में विभिन्न योजनान्तर्गत " + List.Rows[0]["fertilizer_name_hi"] + " के उपयोग हेतु  निम्नानुसार  ग्राम सेवा  सहकारी  समितियों को उपलब्ध करवाने का कष्ट करावें । </p></td></tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' style=''>");
                    sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;'>");
                    sb.Append("<tr><th style='text-align:center'> क्र. सं. </th><th> समिति का नाम </th><th style='text-align:center;'>मात्रा <br/> (" + List.Rows[0]["fertilizer_unit_hi"] + ") </th><th  style='text-align:center;'> सहायक कृषि अधिकारी क्षेत्र </th><th  style='text-align:center;'> रुट चार्ट </th></tr>");

                    int index = 1;
                    foreach (DataRow dr in List.Rows) // search whole table
                    {
                        sb.Append("<tr><td style='text-align:center'>" + (index++) + "</td><td style='text-align:left;'> " + dr["SocietyName"].ToString() + " </td><td style='text-align:center;'>" + dr["quantity"].ToString() + "</td><td style='text-align:center;'> " + dr["GP_MANGAL"].ToString() + " </td><td style='text-align:center;'> " + dr["rout_chart"].ToString() + " </td></tr>");
                    }

                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' style=''>कृपया उपरोक्तानुसार " + List.Rows[0]["fertilizer_name_hi"] + " की आपूर्ति करवाने का कष्ट  करें। </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr><td colspan='2' style='text-align:right;'> भवदीय </td></tr>");
                    sb.Append("<tr><td colspan='2' style='text-align:right;'>सहायक निदेशक कृषि (विस्तार) </td></tr>");

                    sb.Append("</table>");
                    sb.Append("</div>");
                    sb.Append("<div class='col-md-12' style='margin-top: 10px;' ng-if='ShowGenarateOrder'>");
                    sb.Append("<div class='dev-menu-btn'>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</body></html>");

                    string filepath = Server.MapPath("~/Content/FertilizerSupply");
                    if (!System.IO.Directory.Exists(filepath))
                    {
                        System.IO.Directory.CreateDirectory(filepath);
                    }
                    ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                    DataTable DT = new DataTable();

                    string FileName = "DirectorRequest_" + requestNo;

                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true));
                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName + ".pdf"))
                    {

                        //result = COMMON_FUNCTION.VerifyAadhaarOTPForEsingh(OTP, TXN, filepath + "/" + FileName + ".pdf", true, FileName + ".pdf");


                        var result1 = COMMON_FUNCTION.EsinghPDF(otpResult.data, filepath + "/" + FileName + ".pdf");
                        result = result1.data;
                        if (result == "Success")
                        {
                            objNew.UpdateKVSSRequest(requestNo, FileName + ".pdf", 1, 0);
                        }


                        //objSocietyModel.UpdateElectionStatusFile("" + FileName + ".pdf", fileType, Convert.ToInt32(List.Rows[0]["SocietyId"].ToString()), Convert.ToInt32(List.Rows[0]["ElectionMasterId"].ToString()));
                        //var DT1 = objSocietyModel.GetElectionEmailSMSDetails(32, 4, ElectionMasterId, SocietyId);
                        //if (fileType == "MemberListFile")
                        //{
                        //    foreach (DataRow dr in DT1.Rows) // search whole table
                        //    {
                        //        new EsanchaarController().SendSocietySMS(dr["sms"].ToString().Replace("APPLICANTNAME", dr["OfficerNameEn"].ToString()), dr["MobileNo"].ToString());
                        //    }
                        //}

                    }
                    else
                    {
                        result = "PDF Not Generate";
                    }
                }
            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.FertilizerSupply", FunctionName = " GenerateDirectorDemand(string OTP, string TXN, List<GSSDemandModel> ReqestData)" }, ex);
                result = "Error";
            }
            return result;
        }
        [HttpPost]
        public JsonResult GetDirectorDemandList(DataPagingModel Details, string RequestType)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            int DistrictId = 22;
            DataTable DT = objNew.GetDirectorDemandList(ref Details, DistrictId, RequestType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
            //return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objNew.GSSDEmandList(_LoggedInUser.SSOID));
        }

        public ActionResult AgricultureDirectorReport()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAgricultureDirectorReport(DataPagingModel Details, string Type, string ReportType)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            int DistrictId = 22;//  _LoggedInUser.DistrictId;
            DataTable List = objNew.GetADReport(ref Details, Type, ReportType, DistrictId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(List), DataPaging = Details }, JsonRequestBehavior.AllowGet);
            //return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objNew.GSSDEmandList(_LoggedInUser.SSOID));
        }

        [HttpPost]
        public string GetAgricultureDirectorReportExport(DataPagingModel Details, string Type, string ReportType)
        {
            string result = "";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return null;
                }
                FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());

                int DistrictId = 22;//  _LoggedInUser.DistrictId;


                Details.PageNumber = 1;
                Details.IsAllRecords = true;

                DataTable List = objNew.GetADReport(ref Details, Type, ReportType, DistrictId);

                //DataTable List = objNew.OrderDetails(ref Details, OrderNo);
                //DataTable TransportList = objNew.GetTransportList(ref Details).AsEnumerable().Where(row => row.Field<int>("transport_id") == TransportId).CopyToDataTable();


                object sumOrderQty;
                sumOrderQty = List.Compute("Sum(quantity)", string.Empty);
                OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                var orgLig = obj.GetAllOrganisations(" and OrganisationId=2");
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><head><style>.lable{font-size:30px!important;}</style></head><body>");
                sb.Append("<div id='divtoOrderPrint' ng-if='ShowOrderDetails' style='padding-left:17%; padding-right:17%;'>");
                sb.Append("<div style='margin-right:25px;    margin-left:25px;    font-size:23px;    padding:30px;'>");
                sb.Append("<table id='content' border='0' style='width:100%;' cellpadding='5' cellspacing='0'>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:center;'>");
                //sb.Append("<label style='font-size: 20px;'> " + List.Rows[0]["SocietyName"] + ", " + List.Rows[0]["Address"] + " </label>");
                //sb.Append("<label style='font-size: 45px!important;'> राजस्थान राज्य  सहकारी  क्रय-विक्रय  संघ  लि </label><br/>");
                sb.Append("</td>");
                sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td colspan='2' style='text-align:center;border-bottom: 2px solid black;'>");
                ////sb.Append(" " + orgLig.Rows[0]["Address_Hi"] + " ");
                //sb.Append("</td>");
                //sb.Append("</tr>");                   
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style=''>");
                sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;'>");
                sb.Append("<tr><th style='text-align:center'> क्र. सं. </th>");
                if (ReportType == "Quantity")
                {
                    sb.Append("<th style='text-align:left'> " + (Type == "FY" ? "वित्तीय वर्ष" : Type == "Yearly" ? "वर्ष" : Type == "Month" ? "माह" : "दिनांक") + " </th>");
                }
                if (ReportType != "Quantity" && Type == "Date")
                {
                    sb.Append("<th style='text-align:left'> दिनांक </th>");
                }
                if (ReportType != "Quantity")
                {
                    sb.Append("<th style='text-align:left'> जीएसएस का नाम </th>");
                }
                sb.Append("<th  style='text-align:center'> उर्वरक </th>");
                sb.Append("<th style='text-align:center'> मात्रा </th>");
                sb.Append("<th  style='text-align:center'> यूनिट </th></tr>");
                int index = 1;

                foreach (DataRow dr in List.Rows) // search whole table
                {
                    //sb.Append("<tr><td style='text-align:center'>" + (index++) + "</td><td  style='text-align:center;" + (ReportType != "GSSWise" ? "" : "display:none") + "'> " + dr["FY"].ToString() + " </td>" + (ReportType == "GSSWise" ? "<th style='text-align:left'> " + dr["SocietyName"].ToString() + " </th>" : "") + "<td style='text-align:center'> " + dr["name_hi"].ToString() + " </td><td style='text-align:center'> " + dr["quantity"].ToString() + " </td><td style='text-align:center'> " + dr["unit_hi"].ToString() + " </td></tr>");

                    sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                    if (ReportType == "Quantity")
                    {
                        sb.Append("<td style='text-align:left'> " + dr["FY"].ToString() + " </td>");
                    }
                    if (ReportType != "Quantity" && Type == "Date")
                    {
                        sb.Append("<td style='text-align:left'> " + dr["FY"].ToString() + " </td>");
                    }
                    if (ReportType != "Quantity")
                    {
                        sb.Append("<td style='text-align:left'> " + dr["SocietyName"].ToString() + " </td>");
                    }
                    sb.Append("<td  style='text-align:center'> " + dr["name_hi"].ToString() + " </td>");
                    sb.Append("<td style='text-align:center'> " + dr["quantity"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:center'> " + dr["unit_hi"].ToString() + " </td></tr>");

                }


                sb.Append("<tr><td style='text-align:center'>  </td>");
                if (ReportType == "Quantity")
                {
                    sb.Append("<td style='text-align:left'> </td>");
                }
                if (ReportType != "Quantity" && Type == "Date")
                {
                    sb.Append("<td style='text-align:left'>  </td>");
                }
                if (ReportType != "Quantity")
                {
                    sb.Append("<td style='text-align:left'>  </td>");
                }
                sb.Append("<td  style='text-align:center'> <b> योग </b> </td>");
                sb.Append("<td style='text-align:center'> <b>" + sumOrderQty.ToString() + "</b> </td>");
                sb.Append("<td  style='text-align:center'>  </td></tr>");











                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                //sb.Append("<td colspan='2' style=''> अतः उपरोक्तानुसार माँग अनुसार " + List.Rows[0]["fertilizer_name_hi"] + " आपूर्ति करवाने का कष्ट  करें। </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                //sb.Append("<td colspan='2' style='text-align:left;'>संलग्नानुसार |</td>");
                sb.Append("</tr>");
                //sb.Append("<tr><td colspan='2' style='text-align:right;'><br /><br /> भवदीय </td></tr>");
                //sb.Append("<tr><td colspan='2' style='text-align:right;'><br />सहा. प्रबन्धक (कृषि आदान) </td></tr>");
                //sb.Append("<tr><td colspan='2' style='text-align:left;'>प्रतिलिपि:- </td></tr>");
                //sb.Append("<tr>");
                //sb.Append("<td colspan='2' style='text-align:left;padding-left: 45px;'>");
                //sb.Append("<ol>");
                //sb.Append("<li>संयुक्त  निदेशक ,(रसायन ) कृषि  आयुक्तालय ,राजस्थान , जयपुर</li>");
                //sb.Append("<li>उपप्रबंधक ,आरएसएमएमएल, प्लाट न. 02  गांधीनगर  स्कीम ,बीकानेर को  भेजकर निवेदन है  कि  उक्तानुसार  राजफैड  के  ट्रांसपोर्टर  को {{GSSList[0].fertilizer_name_hi}} उपलब्ध  करवाने का कष्ट  करें |</li>");
                //sb.Append("<li>उपनिदेशक कृषि (विस्तार), जिला  परिषद , सीकर  / बाड़मेर  |</li>");
                //sb.Append("<li>क्षेत्रीय  कार्यालय , अधिकारी , जयपुर /जोधपुर |</li>");
                //sb.Append("</ol>");
                //sb.Append("</td>");
                //sb.Append("</tr>");
                //sb.Append("<tr><td colspan='2' style='text-align:right;'>भवदीय</td></tr>");
                //sb.Append("<tr><td colspan='2' style='text-align:right;'>सहाO  प्रबन्धक (कृषि  आदान) </td></tr>");
                sb.Append("</table>");
                sb.Append("</div>");
                sb.Append("<div class='col-md-12' style='margin-top: 10px;' ng-if='ShowGenarateOrder'>");
                sb.Append("<div class='dev-menu-btn'>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</body></html>");


                //if (!System.IO.Directory.Exists(filepath))
                //{
                //    System.IO.Directory.CreateDirectory(filepath);
                //}
                ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                DataTable DT = new DataTable();

                string FileName = "RequestSupply_" + OrderNo;

                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();




                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true));
                var bytes = COMMON_FUNCTION.GePDFBytes(sb1, "");
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);

                //string result1 = System.Text.Encoding.UTF8.GetString(COMMON_FUNCTION.GePDFBytes(sb1, "", filepath));

                return base64String;



            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.FertilizerSupply", FunctionName = "  GetAgricultureDirectorReportExport(DataPagingModel Details, string Type, string ReportType)" }, ex);
                result = "Error";
            }
            return null;
        }


        #endregion

        #region DemandSupply
        public ActionResult DemandSupply()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetDemandSupplyList(DataPagingModel Details, string RequestType, int ListType)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());

            DataTable DT = objNew.GetDemandSupplyList(ref Details, RequestType, ListType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
            //return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objNew.GSSDEmandList(_LoggedInUser.SSOID));
        }
        [HttpPost]
        public JsonResult GetTransportList()
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataPagingModel Details = new DataPagingModel();
            Details.PageNumber = 1;
            Details.IsAllRecords = true;
            DataTable DT = objNew.GetTransportList(ref Details);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
            //return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objNew.GSSDEmandList(_LoggedInUser.SSOID));
        }

        [HttpPost]
        public JsonResult GetKVSSRequestDetailList(string RequestNo)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataPagingModel Details = new DataPagingModel();
            Details.IsAllRecords = true;
            Details.PageNumber = 1;
            DataTable DT = objNew.KVSSRequestDetails(ref Details, RequestNo);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
            //return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objNew.GSSDEmandList(_LoggedInUser.SSOID));
        }
        [HttpPost]
        public string GenerateSupplyOrder(string OTP, string TXN, int TransportId, List<GSSDemandModel> ReqestData)
        {
            string result = "";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return "LOGOUT";
                }
                FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());



                var otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                if (otpResult.status != "1")
                {
                    return otpResult.data;
                }


                // string requestNo = objNew.InsertKVSSRequest(ReqestData);

                ReqestData.ForEach(x =>
                {
                    OrderNo = objNew.UpdateKVSSRequest(x.Request_No.ToString(), OrderNo, 3, 0);
                });



                //List<GSSDemandModel> request = new List<GSSDemandModel>();

                //ReqestData.ForEach(x =>
                //{                    
                //   int demandId= objNew.InsertGSSDemand(x);
                //    request.Add(new GSSDemandModel { DemandId = demandId, RequestId = 2 });
                //});

                //OrderNo= objNew.InsertKVSSRequest(request);


                if (result == "-1" && result == "0")
                {
                    result = "Error";
                }
                else
                {

                    DataPagingModel Details = new DataPagingModel();
                    Details.PageNumber = 1;
                    Details.IsAllRecords = true;
                    DataTable List = objNew.OrderDetails(ref Details, OrderNo);
                    DataTable TransportList = objNew.GetTransportList(ref Details).AsEnumerable().Where(row => row.Field<int>("transport_id") == TransportId).CopyToDataTable();


                    object sumOrderQty;
                    sumOrderQty = List.Compute("Sum(quantity)", string.Empty);
                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                    var orgLig = obj.GetAllOrganisations(" and OrganisationId=2");
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<html><head><style>.lable{font-size:30px!important;}</style></head><body>");
                    sb.Append("<div id='divtoOrderPrint' ng-if='ShowOrderDetails' style='padding-left:17%; padding-right:17%;'>");
                    sb.Append("<div style='margin-right:25px;    margin-left:25px;font-size:12px;padding:30px;'>");
                    sb.Append("<table id='content' border='0' style='width:100%;font-size:12px;' cellpadding='5' cellspacing='0'>");
                    sb.Append("<tr>");
                    sb.Append("<td  style='text-align:left;font-size:10px; vertical-align: top;'>GST No. :  " + orgLig.Rows[0]["GSTNo"] + " </td>");
                    //sb.Append("<td style='vertical-align: top;'>");
                    //sb.Append("<table><tr><td style='width:66px;vertical-align:top;'> फोन </td><td style='width:5px;vertical-align: top;'>:</td><td>" + orgLig.Rows[0]["Phone"] + "</td></tr>");
                    //sb.Append("<tr><td style='vertical-align:top;'> फैक्स </td><td style='width:5px;vertical-align: top;'>:</td><td>" + orgLig.Rows[0]["Fax"] + "</td></tr>");
                    //sb.Append("<tr><td style='vertical-align:top;'> E-mail </td><td style='width:5px;vertical-align: top;'>:</td><td>" + orgLig.Rows[0]["Email"] + " </td></tr></table>");
                    // sb.Append("</td>");
                    //sb.Append("</tr>");sb.Append("<tr>");

                    sb.Append("<td  style='text-align:right;'>");
                    sb.Append("<p><label style='font-size:15px'>" + UnicodeToKrutidev.GetKrutidev("फोन") + "    </label>:<label style='padding-left:20px;font-size:10px'> " + orgLig.Rows[0]["Phone"] + " </label><br/>");
                    sb.Append("<label style='font-size:15px'>" + UnicodeToKrutidev.GetKrutidev("फैक्स") + "   </label>:<label style='padding-left:20px;font-size:10px'> " + orgLig.Rows[0]["Fax"] + " </label><br/> ");
                    sb.Append("<label  style='font-size:10px'> E-mail </label>:<label style='padding-left:20px;font-size:10px'> " + orgLig.Rows[0]["Email"] + " </label></p> ");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align:left; font-size:16px; '>फैक्स </td>");
                    //sb.Append("<td style='text-align:left;font-size:16px;word-break:break-all;'>");
                    //sb.Append(": " + orgLig.Rows[0]["Fax"] + " ");

                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align:left; font-size:16px; vertical-align: top;'>E-mail </td>");
                    //sb.Append("<td style='text-align:left;vertical-align: top;font-size:16px;word-break:break-all'>");
                    //sb.Append(": " + orgLig.Rows[0]["Email"] + " ");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' style='text-align:center;'>");
                    sb.Append("<label style='font-size: 21px;'><b>RAJASTHAN STATE CO-OPERATIVE MARKETING FEDERATION LTD</b></label><br/>");
                    sb.Append("<label style='font-size: 35px'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान राज्य  सहकारी  क्रय-विक्रय  संघ  लि") + "</b></label>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' style='text-align:center;border-bottom: 2px solid black;'>");
                    sb.Append(" " + orgLig.Rows[0]["Address_Hi"] + " ");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr><td style=''  >क्रमांक  :  एफ / कृषि / " + TransportList.Rows[0]["FinancialYear"] + " /" + OrderNo + "</td><td style='text-align:right;'>दिनांक : " + DateTime.Now.ToString("dd/MM/yyy") + "</td></tr>");
                    sb.Append("<tr><td colspan='2' style=''>" + TransportList.Rows[0]["transport_name_hi"] + " </td></tr>");
                    sb.Append("<tr><td style='' > " + TransportList.Rows[0]["address_hi"] + " </td><td style=''></td></tr>");
                    sb.Append("<tr><td colspan='2' style=''>E-mail Id:- " + TransportList.Rows[0]["email"] + " </td></tr>");
                    sb.Append("<tr><td colspan='2' ><p style='padding-left:65px;padding-top:25px;'>विषय :-   " + List.Rows[0]["fertilizer_name_hi"] + " की  आपूर्ति  करने बाबत। </p></td></tr>");
                    sb.Append("<tr><td colspan='2' style=''> महोदय, </td></tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' style=''>");
                    sb.Append("<p style='text-indent: 65px;'>");
                    sb.Append("वर्ष " + TransportList.Rows[0]["FinancialYear"] + "  में " + List.Rows[0]["fertilizer_name_hi"] + " परिवहन  कार्य  हेतु राजफैड  द्वारा स्वीकृत  दरों  पर  निष्पादित  अनुबंध  अनुसार " + List.Rows[0]["fertilizer_name_hi"] + "  परिवहन  हेतु आपको  पत्र  के साथ निम्नानुसार उप / सहा. निदेशक /समितियों की " + List.Rows[0]["fertilizer_name_hi"] + " की  स्थानवार  माँग  एवं  रुटचार्ट  निम्नानुसार /संलग्नानुसार  भिजवाया  जा रहा है |  कृपया गन्तव्य स्थान   पर निम्नानुसार " + List.Rows[0]["fertilizer_name_hi"] + "  पहुँचाये :-");
                    sb.Append("</p>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' style=''>");
                    sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;;font-size:12px'>");
                    sb.Append("<tr><th style='text-align:center'> क्र. सं. </th><th style='text-align:center'> समिति का नाम </th><th style='text-align:center'> मात्रा (मै. टन) </th><th style='text-align:center'> स्थान  एवं रुटचार्ट  </th></tr>");
                    int index = 1;
                    string requestFiles = "";
                    string filepath = Server.MapPath("~/Content/FertilizerSupply");
                    foreach (DataRow dr in List.Rows) // search whole table
                    {
                        requestFiles += (requestFiles != "" ? "," : "") + filepath + "/" + dr["request_file"].ToString();

                        if (dr["request_type"].ToString() == "2")
                        {
                            sb.Append("<tr ng-repeat='item in GSSList'><td style='text-align:center'>" + (index++) + "</td><td> " + ("सहायक निदेशक कृषि (विस्तार) " + dr["BLOCK_MANGAL"].ToString() + ", जिला " + dr["DISTRICT_MANGAL"].ToString()) + " </td><td style='text-align:center'> " + dr["quantity"].ToString() + " </td><td  style='text-align:center'> संलग्नानुसार आपूर्ति करवाये </td></tr>");

                        }
                        else
                        {
                            sb.Append("<tr ng-repeat='item in GSSList'><td style='text-align:center'>" + (index++) + "</td><td> " + dr["SocietyName"].ToString() + " </td><td style='text-align:center'> " + dr["quantity"].ToString() + " </td><td  style='text-align:center'> संलग्नानुसार आपूर्ति करवाये </td></tr>");

                        }
                    }

                    sb.Append("<tr><td></td><td style='text-align:center;'><b> योग </b></td><td style='text-align:center'><b>" + sumOrderQty.ToString() + "</b></td> <td> </td></tr>");

                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='2' style=''> अतः उपरोक्तानुसार माँग अनुसार " + List.Rows[0]["fertilizer_name_hi"] + " आपूर्ति करवाने का कष्ट  करें। </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    //sb.Append("<td colspan='2' style='text-align:left;'>संलग्नानुसार |</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr><td colspan='2' style='text-align:right;'><br /><br /> भवदीय </td></tr>");
                    sb.Append("<tr><td colspan='2' style='text-align:right;'><br />सहा. प्रबन्धक (कृषि आदान) </td></tr>");
                    //sb.Append("<tr><td colspan='2' style='text-align:left;'>प्रतिलिपि:- </td></tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td colspan='2' style='text-align:left;padding-left: 45px;'>");
                    //sb.Append("<ol>");
                    //sb.Append("<li>संयुक्त  निदेशक ,(रसायन ) कृषि  आयुक्तालय ,राजस्थान , जयपुर</li>");
                    //sb.Append("<li>उपप्रबंधक ,आरएसएमएमएल, प्लाट न. 02  गांधीनगर  स्कीम ,बीकानेर को  भेजकर निवेदन है  कि  उक्तानुसार  राजफैड  के  ट्रांसपोर्टर  को {{GSSList[0].fertilizer_name_hi}} उपलब्ध  करवाने का कष्ट  करें |</li>");
                    //sb.Append("<li>उपनिदेशक कृषि (विस्तार), जिला  परिषद , सीकर  / बाड़मेर  |</li>");
                    //sb.Append("<li>क्षेत्रीय  कार्यालय , अधिकारी , जयपुर /जोधपुर |</li>");
                    //sb.Append("</ol>");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");
                    //sb.Append("<tr><td colspan='2' style='text-align:right;'>भवदीय</td></tr>");
                    //sb.Append("<tr><td colspan='2' style='text-align:right;'>सहाO  प्रबन्धक (कृषि  आदान) </td></tr>");
                    sb.Append("</table>");
                    sb.Append("</div>");
                    sb.Append("<div class='col-md-12' style='margin-top: 10px;' ng-if='ShowGenarateOrder'>");
                    sb.Append("<div class='dev-menu-btn'>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</body></html>");


                    if (!System.IO.Directory.Exists(filepath))
                    {
                        System.IO.Directory.CreateDirectory(filepath);
                    }
                    ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                    DataTable DT = new DataTable();

                    string FileName = "RequestSupply_" + OrderNo;

                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "17px"));
                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName + ".pdf"))
                    {
                        requestFiles = filepath + "/" + FileName + ".pdf" + "," + requestFiles;
                        FileName = "OrderSupply_" + OrderNo;

                        COMMON_FUNCTION.MergePDFs(filepath + "/" + FileName + ".pdf", requestFiles.Split(','));

                        var esignResult = COMMON_FUNCTION.EsinghPDF(otpResult.data, filepath + "/" + FileName + ".pdf");
                        result = esignResult.data;
                        //result = COMMON_FUNCTION.VerifyAadhaarOTPForEsingh(OTP, TXN, filepath + "/" + FileName + ".pdf", true, FileName + ".pdf");
                        if (result == "Success")
                        {
                            //requestFiles = filepath + "/" + FileName + ".pdf" + "," + requestFiles;


                            objNew.UpdateKVSSRequest(OrderNo, FileName + ".pdf", 2, TransportId);
                        }


                        //objSocietyModel.UpdateElectionStatusFile("" + FileName + ".pdf", fileType, Convert.ToInt32(List.Rows[0]["SocietyId"].ToString()), Convert.ToInt32(List.Rows[0]["ElectionMasterId"].ToString()));
                        //var DT1 = objSocietyModel.GetElectionEmailSMSDetails(32, 4, ElectionMasterId, SocietyId);
                        //if (fileType == "MemberListFile")
                        //{
                        //    foreach (DataRow dr in DT1.Rows) // search whole table
                        //    {
                        //        new EsanchaarController().SendSocietySMS(dr["sms"].ToString().Replace("APPLICANTNAME", dr["OfficerNameEn"].ToString()), dr["MobileNo"].ToString());
                        //    }
                        //}

                    }
                    else
                    {
                        result = "PDF Not Generate";
                    }
                }
            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.FertilizerSupply", FunctionName = " GenerateSupplyOrder(string OTP, string TXN, int TransportId, List<GSSDemandModel> ReqestData)" }, ex);
                result = "Error";
            }
            return result;
        }

        public ActionResult SupplyReport()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetSupplyReport(DataPagingModel Details, string Type, string ReportType)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());

            DataTable DT = objNew.GetSupplyReport(ref Details, Type, ReportType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
            //return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objNew.GSSDEmandList(_LoggedInUser.SSOID));
        }

        [HttpPost]
        public string GetSupplyReportExport(DataPagingModel Details, string Type, string ReportType)
        {
            string result = "";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return null;
                }
                FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());




                Details.PageNumber = 1;
                Details.IsAllRecords = true;

                DataTable List = objNew.GetSupplyReport(ref Details, Type, ReportType);

                //DataTable List = objNew.OrderDetails(ref Details, OrderNo);
                //DataTable TransportList = objNew.GetTransportList(ref Details).AsEnumerable().Where(row => row.Field<int>("transport_id") == TransportId).CopyToDataTable();


                object sumOrderQty;
                sumOrderQty = List.Compute("Sum(quantity)", string.Empty);
                OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                var orgLig = obj.GetAllOrganisations(" and OrganisationId=2");
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<html><head><style>.lable{font-size:30px!important;}</style></head><body>");
                sb.Append("<div id='divtoOrderPrint' ng-if='ShowOrderDetails' style='padding-left:17%; padding-right:17%;'>");
                sb.Append("<div style='margin-right:25px;    margin-left:25px;    font-size:23px;    padding:30px;'>");
                sb.Append("<table id='content' border='0' style='width:100%;' cellpadding='5' cellspacing='0'>");
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style='text-align:center;'>");
                //sb.Append("<label style='font-size: 20px;'> " + List.Rows[0]["SocietyName"] + ", " + List.Rows[0]["Address"] + " </label>");
                //sb.Append("<label style='font-size: 45px!important;'> राजस्थान राज्य  सहकारी  क्रय-विक्रय  संघ  लि </label><br/>");
                sb.Append("</td>");
                sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td colspan='2' style='text-align:center;border-bottom: 2px solid black;'>");
                ////sb.Append(" " + orgLig.Rows[0]["Address_Hi"] + " ");
                //sb.Append("</td>");
                //sb.Append("</tr>");                   
                sb.Append("<tr>");
                sb.Append("<td colspan='2' style=''>");
                sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;'>");
                sb.Append("<tr><th style='text-align:center'> क्र. सं. </th>");
                if (ReportType == "Quantity")
                {
                    sb.Append("<th style='text-align:left'> " + (Type == "FY" ? "वित्तीय वर्ष" : Type == "Yearly" ? "वर्ष" : Type == "Month" ? "माह" : "दिनांक") + " </th>");
                }
                if (ReportType != "Quantity" && Type == "Date")
                {
                    sb.Append("<th style='text-align:left'> दिनांक </th>");
                }
                if (ReportType != "Quantity")
                {
                    sb.Append("<th style='text-align:left'> जीएसएस का नाम </th>");
                }
                sb.Append("<th  style='text-align:center'> उर्वरक </th>");
                sb.Append("<th style='text-align:center'> मात्रा </th>");
                sb.Append("<th  style='text-align:center'> यूनिट </th></tr>");
                int index = 1;

                foreach (DataRow dr in List.Rows) // search whole table
                {
                    //sb.Append("<tr><td style='text-align:center'>" + (index++) + "</td><td  style='text-align:center;" + (ReportType != "GSSWise" ? "" : "display:none") + "'> " + dr["FY"].ToString() + " </td>" + (ReportType == "GSSWise" ? "<th style='text-align:left'> " + dr["SocietyName"].ToString() + " </th>" : "") + "<td style='text-align:center'> " + dr["name_hi"].ToString() + " </td><td style='text-align:center'> " + dr["quantity"].ToString() + " </td><td style='text-align:center'> " + dr["unit_hi"].ToString() + " </td></tr>");

                    sb.Append("<tr><td style='text-align:center'> " + (index++) + " </td>");
                    if (ReportType == "Quantity")
                    {
                        sb.Append("<td style='text-align:left'> " + dr["FY"].ToString() + " </td>");
                    }
                    if (ReportType != "Quantity" && Type == "Date")
                    {
                        sb.Append("<td style='text-align:left'> " + dr["FY"].ToString() + " </td>");
                    }
                    if (ReportType != "Quantity")
                    {
                        sb.Append("<td style='text-align:left'> " + dr["SocietyName"].ToString() + " </td>");
                    }
                    sb.Append("<td  style='text-align:center'> " + dr["name_hi"].ToString() + " </td>");
                    sb.Append("<td style='text-align:center'> " + dr["quantity"].ToString() + " </td>");
                    sb.Append("<td  style='text-align:center'> " + dr["unit_hi"].ToString() + " </td></tr>");

                }


                sb.Append("<tr><td style='text-align:center'>  </td>");
                if (ReportType == "Quantity")
                {
                    sb.Append("<td style='text-align:left'> </td>");
                }
                if (ReportType != "Quantity" && Type == "Date")
                {
                    sb.Append("<td style='text-align:left'>  </td>");
                }
                if (ReportType != "Quantity")
                {
                    sb.Append("<td style='text-align:left'>  </td>");
                }
                sb.Append("<td  style='text-align:center'> <b> योग </b> </td>");
                sb.Append("<td style='text-align:center'> <b>" + sumOrderQty.ToString() + "</b> </td>");
                sb.Append("<td  style='text-align:center'>  </td></tr>");











                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                //sb.Append("<td colspan='2' style=''> अतः उपरोक्तानुसार माँग अनुसार " + List.Rows[0]["fertilizer_name_hi"] + " आपूर्ति करवाने का कष्ट  करें। </td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                //sb.Append("<td colspan='2' style='text-align:left;'>संलग्नानुसार |</td>");
                sb.Append("</tr>");
                //sb.Append("<tr><td colspan='2' style='text-align:right;'><br /><br /> भवदीय </td></tr>");
                //sb.Append("<tr><td colspan='2' style='text-align:right;'><br />सहा. प्रबन्धक (कृषि आदान) </td></tr>");
                //sb.Append("<tr><td colspan='2' style='text-align:left;'>प्रतिलिपि:- </td></tr>");
                //sb.Append("<tr>");
                //sb.Append("<td colspan='2' style='text-align:left;padding-left: 45px;'>");
                //sb.Append("<ol>");
                //sb.Append("<li>संयुक्त  निदेशक ,(रसायन ) कृषि  आयुक्तालय ,राजस्थान , जयपुर</li>");
                //sb.Append("<li>उपप्रबंधक ,आरएसएमएमएल, प्लाट न. 02  गांधीनगर  स्कीम ,बीकानेर को  भेजकर निवेदन है  कि  उक्तानुसार  राजफैड  के  ट्रांसपोर्टर  को {{GSSList[0].fertilizer_name_hi}} उपलब्ध  करवाने का कष्ट  करें |</li>");
                //sb.Append("<li>उपनिदेशक कृषि (विस्तार), जिला  परिषद , सीकर  / बाड़मेर  |</li>");
                //sb.Append("<li>क्षेत्रीय  कार्यालय , अधिकारी , जयपुर /जोधपुर |</li>");
                //sb.Append("</ol>");
                //sb.Append("</td>");
                //sb.Append("</tr>");
                //sb.Append("<tr><td colspan='2' style='text-align:right;'>भवदीय</td></tr>");
                //sb.Append("<tr><td colspan='2' style='text-align:right;'>सहाO  प्रबन्धक (कृषि  आदान) </td></tr>");
                sb.Append("</table>");
                sb.Append("</div>");
                sb.Append("<div class='col-md-12' style='margin-top: 10px;' ng-if='ShowGenarateOrder'>");
                sb.Append("<div class='dev-menu-btn'>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</body></html>");


                //if (!System.IO.Directory.Exists(filepath))
                //{
                //    System.IO.Directory.CreateDirectory(filepath);
                //}
                ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                DataTable DT = new DataTable();

                string FileName = "RequestSupply_" + OrderNo;

                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();




                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true));
                var bytes = COMMON_FUNCTION.GePDFBytes(sb1, "");
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);

                //string result1 = System.Text.Encoding.UTF8.GetString(COMMON_FUNCTION.GePDFBytes(sb1, "", filepath));

                return base64String;



            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.FertilizerSupply", FunctionName = " GetSupplyReportExport(DataPagingModel Details, string Type, string ReportType)" }, ex);
                result = "Error";
            }
            return null;
        }



        #endregion

        #region Transport Master
        public ActionResult TransportMaster()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TransportMaster(TransportMasterModel Details)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            int result = objNew.InsertTransport(Details);
            return Json(new { Status = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTransportMasterList(DataPagingModel Details)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());

            DataTable DT = objNew.GetTransportList(ref Details);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Warehouse


        public ActionResult Warehouse()
        {
            string Todate = "";
            int Day = -1;
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());

            var response = TempData.Peek("Response")?.ToString();
            ViewBag.response = response;
            if (Request.Cookies[SessionIdentity.Culture] == null)
            {
                Response.Cookies.Add(new System.Web.HttpCookie(Cooperative.Controllers.SessionIdentity.Culture, "")
                { Value = "en-us", HttpOnly = true });
            }

            ViewBag.FromDate = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
            ViewBag.TodateDate = DateTime.Now.AddDays(-1).AddMonths(3).ToString("dd/MM/yyyy").Replace("-", "/");
            Todate = ViewBag.MinDate = "2019/11/01"; //DateTime.Now.AddDays(-1).ToString("yyy/MM/dd"); 
                                                     //Todate = ViewBag.MinDate = DateTime.Now.AddDays(-1).ToString("yyy/MM/dd"); 
                                                     //while (objNew.CheckIsItHoliday(Todate)>0)
                                                     //{
                                                     //    Day = Day - 1;
                                                     //    Todate = ViewBag.MinDate =DateTime.Now.AddDays(Day).ToString("yyy/MM/dd");

            //}

            ViewBag.MinDate = Todate;


            //int Result = objNew.CheckIsItHoliday(Todate);


            return View();
        }

        [HttpPost]
        public ActionResult Warehouse(List<WarehouseRSWCModel> Details)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            return Json(new { status = objNew.InsertUpdateRSWC(Details) }, JsonRequestBehavior.AllowGet);
        }


        public string GenerateWarehouseRSWCEng(string TXN, List<WarehouseRSWCModel> DetailsList)
        {
            string result = "";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return "LOGOUT";
                }
                FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
                if (!DetailsList[0].IsSubmit)
                {
                    if (DetailsList[0].Id == "0")
                    {
                        result = objNew.InsertUpdateRSWC(DetailsList).ToString();

                    }
                    else
                    {
                        result = objNew.UpdateRSWC(DetailsList).ToString();
                        //string requestNo = objNew.InsertKVSSRequest(ReqestData);
                    }
                    if (result == "-1" || result == "0")
                    {
                        return result;
                    }
                    else
                    {
                        result = "Success";
                    }
                    return result;
                }

                if (result == "-1" || result == "0")
                {
                    return result;
                }
                else
                {

                    // DataTable List = objNew.GetDirectorDemandDetails(result);

                    //////var otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                    //////if (otpResult.status != "1")
                    //////{
                    //////    return otpResult.data;
                    //////}

                    object sumOrderQty;

                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<html><head><style>.lable{font-size:30px!important;}</style></head><body>");

                    sb.Append("<div style='margin-right:15px;    margin-left:15px; padding:5px;'>");
                    int index = 1;
                    string requestFiles = "";
                    string filepath = Server.MapPath("~/Content/RSWC");
                    string devFontSize = "";
                    string WRHTML = GenerateWRHTML(DetailsList);
                    sb.Append(WRHTML);
                    sb.Append("</div>");
                    sb.Append("</body></html>");



                    string fileId = Guid.NewGuid().ToString();

                    if (!System.IO.Directory.Exists(filepath))
                    {
                        System.IO.Directory.CreateDirectory(filepath);
                    }
                    ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                    DataTable DT = new DataTable();

                    string FileName = fileId;// result.Replace("/", "_");
                    string Receipt_No = result;
                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "15px"));
                    //System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    // sb1.Append(sb.ToString().Replace("<br>", "<br/>"));
                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName + ".pdf", ""))
                    {
                        var esignResult = COMMON_FUNCTION.EsinghPDF(TXN, filepath + "/" + FileName + ".pdf");
                        //var esignResult = COMMON_FUNCTION.EsignPDF_NEW(TXN, filepath + "/" + FileName + ".pdf");

                        result = esignResult.data;
                        if (esignResult.status == "1")
                        {
                            if (DetailsList[0].Id == "0")
                            {
                                result = objNew.InsertUpdateRSWC(DetailsList).ToString();

                            }
                            else
                            {
                                result = objNew.UpdateRSWC(DetailsList).ToString();
                                //string requestNo = objNew.InsertKVSSRequest(ReqestData);
                            }
                            Receipt_No = result;

                            if (System.IO.File.Exists(filepath + "/" + Receipt_No.Replace("/", "_") + ".pdf"))
                            {
                                System.IO.File.Delete(filepath + "/" + Receipt_No.Replace("/", "_") + ".pdf");
                            }

                            System.IO.File.Move(filepath + "/" + FileName + ".pdf", filepath + "/" + Receipt_No.Replace("/", "_") + ".pdf");
                            if (result == "-1" || result == "0")
                            {
                                return result;
                            }
                            else
                            {
                                result = "Success";
                            }
                            //return result;


                            FileName = Receipt_No.Replace("/", "_");
                            DetailsList.ForEach(x =>
                            {
                                x.Id = "1";
                                x.EntryFile = FileName + ".pdf";
                                x.Receipt_No = Receipt_No;
                            });
                            objNew.InsertUpdateRSWC(DetailsList).ToString();


                        }
                    }
                    else
                    {
                        result = "PDF Not Generate";
                    }
                }
            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.FertilizerSupply", FunctionName = "GenerateWarehouseRSWCEng(string TXN, List<WarehouseRSWCModel> DetailsList)" }, ex);
                result = "Error";
            }
            return result;
        }

        public string GenerateWRHTML(List<WarehouseRSWCModel> DetailsList)
        {
            string result = "";
            try
            {
                if (DetailsList.Count > 0)
                {

                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    int index = 1;
                    string devFontSize = "";
                    string fontSize = "font-size: 13px;";
                    sb.Append("<table id='content' border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");
                    sb.Append("<tr>");
                    sb.Append("<th colspan='3' style=''><div style='text-align:left;width:60px;float:left;'><img style='width:60px;height:60px' src='" + Server.MapPath("~/Content/Images/rswc_logo.png") + "' /></div>");
                    sb.Append("<div style='text-align:center;float:left;'><label style='font-size: 17px;'>RAJASTHAN STATE WAREHOUSING CORPORATION</label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 13px;'>(A GOVERNMENT UNDERTAKING)</label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 25px;'> " + UnicodeToKrutidev.GetKrutidev("राजस्थान राज्य भण्डार व्यवस्था निगम") + " </label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 20px;'> (सरकार का प्रतिष्ठान) </label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 13px;'>Warehouse Receipt- Nagotiable/Non-negotiable</label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 13px;'>" + UnicodeToKrutidev.GetKrutidev("भण्डार रसीद परक्राम्य") + "<label style='font-size: 13px;'>/</label>" + UnicodeToKrutidev.GetKrutidev("अपरक्राम्य") + "</label>");
                    sb.Append("</div></th>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style='text-align:center;border-bottom: 2px solid black;'>");
                    sb.Append("  ");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style='text-align:left;'>");
                    sb.Append("<table id='content' border='0' style='width:100%;" + fontSize + "' cellpadding='3' cellspacing='0'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;width:35%;'>Sr.No. Of Physical Receipt (रसीद की क्रम संख्या) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> RSWC/" + DetailsList[0].OldReceipt_No + " </b> </td>");//List.Rows[0]["receipt_serial_no"]
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Name & Location of Warehouse (भण्डार गृह का नाम तथा स्थान) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> S/W " + DetailsList[0].WarehousName + " </b> </td>");//List.Rows[0]["WarehousNameEn"]
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Warehousemans license No. (भण्डार प्रबंधन का लाइसेंस नंबर) </td>");
                    sb.Append("<td style='text-align:left;'><b> " + DetailsList[0].Lisence_No + " </b> </td>");//List.Rows[0]["lisence_no"]
                    sb.Append("<td>Valid up to (तक मान्य) </td>");
                    sb.Append("<td>" + (string.IsNullOrWhiteSpace(DetailsList[0].LicenceValidUpTo) ? "N.A." : DetailsList[0].LicenceValidUpTo) + "</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Received From (Name and address of depositor)<br /> प्राप्त किया (जमा कर्ता का नाम और पता) </td>");
                    //sb.Append("<td colspan='3' style='text-align:left;'><b> NAFED Through KVSS " + DetailsList[0].KvssName + " , Centre " + DetailsList[0].PurchaseCenter + "  </b> </td>");//List.Rows[0]["DISTRICT_ENG"] + ", " + List.Rows[0]["SocietyName"] + " , " + List.Rows[0]["PurchaseCenterEng"]
                    //sb.Append("<td colspan='3' style='text-align:left;'><b> NAFED Through KVSS " + DetailsList[0].KvssName + " , Centre " + DetailsList[0].PurchaseCenter + "  </b> </td>");//List.Rows[0]["DISTRICT_ENG"] + ", " + List.Rows[0]["SocietyName"] + " , " + List.Rows[0]["PurchaseCenterEng"]

                    sb.Append("<td colspan='3' style='text-align:left;'><b> " + DetailsList[0].AgencyName + " Through KVSS " + DetailsList[0].KvssName + " , Centre " + DetailsList[0].PurchaseCenter + "  </b> </td>");//List.Rows[0]["DISTRICT_ENG"] + ", " + List.Rows[0]["SocietyName"] + " , " + List.Rows[0]["PurchaseCenterEng"]
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Commodity (जिंस) </td>");

                    string fyYear = "";

                    if (DateTime.Now.Month < 4)
                        fyYear = (DateTime.Now.Year - 1).ToString();
                    else
                        fyYear = DateTime.Now.Year.ToString();


                    sb.Append("<td  style='text-align:left;'><b> " + DetailsList[0].CommodityName + " </b> / " + DetailsList[0].Kharif + " " + DetailsList[0].Crop_year + "</td>");//List.Rows[0]["CommodityNameEn"]
                    //sb.Append("<td  style='text-align:left;'><b> " + DetailsList[0].CommodityName + " </b> / " + DetailsList[0].Kharif + " " + DetailsList[0].Valid_From_Date.Split('/')[2] + "</td>");//List.Rows[0]["CommodityNameEn"]
                    sb.Append("<td colspan='2' style='text-align:left;'>Moisture (नमी) : <b> " + (string.IsNullOrWhiteSpace(DetailsList[0].Moisture) ? "0" : DetailsList[0].Moisture) + " %</b></td>");//List.Rows[0]["CommodityNameEn"]
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr><td style='" + devFontSize + "' colspan='3' >Goods of the following description (निम्नलिखित वर्णन का माल ) </td></tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style=''>");
                    sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + fontSize + "'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:center;font-size:10px;'>Sr.No<br> क्र.सं. </td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>Class or Standared Quality and/or Grade<br/> श्रेणी या मान्य गुण प्रकार तथा / अथवा मान </td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>No. of Packages or Lots <br/> बंडलों की संख्या </td>");
                    sb.Append("<td style='text-align:center;font-size:10px;width:70px;'>Unit<br/> यूनिट </td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>Net Quantity Measured in Qtls<br> नापी या तोली गयी शुद्ध मात्रा क्वि. में </td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>Name and/or Licence No. of the Weighter/Classifier/Sampler<br/> तोला / श्रेणी कर्ता / नमूना कर्ता का नाम तथा / अथवा लाइसेंस नंबर </td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>GNO/Stack No.<br/> गोदाम / स्टैक नंबर </td>");
                    sb.Append("</tr>");
                    decimal totalPrice = 0;
                    int totalLots = 0;
                    decimal totalNetQty = 0;
                    foreach (var dr in DetailsList)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;font-size:10px;'>" + (index++) + "</td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + (string.IsNullOrWhiteSpace(dr.Quality) ? "FAQ" : dr.Quality.ToUpper()) + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.Packages_Lots + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.Packages_Quantity_Unit + " KG </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.NetQuantity + "  </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.SamplerName + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + (string.IsNullOrWhiteSpace(dr.GoDownName) ? dr.GoDownName : dr.GoDownName) + "/" + dr.Stack_No + " </td>");
                        sb.Append("</tr>");
                        totalPrice += (Convert.ToDecimal(dr.NetQuantity)) * Convert.ToDecimal(dr.Price);//+ ((Convert.ToDecimal(dr.NetQuantity)) * Convert.ToDecimal(dr.StorageRateCharge) ;
                        totalLots += Convert.ToInt32(dr.Packages_Lots);
                        totalNetQty += Convert.ToDecimal(dr.NetQuantity);
                    }

                    if (DetailsList.Count > 1)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2'style='text-align:right'>Total (कुल) </td>");
                        sb.Append("<td style='text-align:center'>" + totalLots + "</td>");
                        sb.Append("<td></td>");
                        sb.Append("<td style='text-align:center'>" + totalNetQty + "</td>");
                        sb.Append("<td></td>");
                        sb.Append("<td></td>");
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("<table id='content' border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;width:60%;'>Condition of Goods -(1) Good (2) Fair (3) Average<br/> माल की स्थिति - (1) उत्तम (2) सुंदर (3) साधारण  </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> " + (DetailsList[0].ConditionOfGoods == "1" ? "Good" : DetailsList[0].ConditionOfGoods == "2" ? "Fair" : "Average") + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Dispatch ID (प्रेषण आईडी) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> " + DetailsList[0].DispatchNo + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Private marks of the depositor on the packages if any<br/> यदि हो तो बंडलों  पर जमाकर्ता  के निजी चिन्ह </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> " + DetailsList[0].PrivateMarkofDepositor + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Rate of storage and other charges (भंडारण की दर और अन्य शुल्क) </td>");
                    sb.Append("<td colspan='3'  style='text-align:left;'><b> " + DetailsList[0].StorageRateCharge + " Per katta Per Month </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Indemnified / Insured for Fire /Theft/Riots / Other Contingency with<br/> आग / चोरी / दंगों / आदि अन्य  आपत्तियों  के लिये बीमा </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> " + DetailsList[0].IndemnifiedInsurance + "  </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>If Insured (Name Of Insurance Co.) (साथ यदि बीमा कराया  गया हो (बीमा समवाय का नाम)) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> " + DetailsList[0].InsuredName + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Remarks (टिप्पणी) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> " + DetailsList[0].QuantityNote + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Done for an amount of Rs. (रुपये की राशि के लिए किया गया) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> Rs. " + String.Format("{0:n}", totalPrice) + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>The goods are accepted for storage from (भण्डारण  के लिए  माल स्वीकार किया  जाता  हैं)  </td>");
                    sb.Append("<td style='text-align:left;'><b> " + DetailsList[0].Valid_From_Date + " </b> </td>");
                    sb.Append("<td style='text-align:left;'>to (से)    </td>");
                    sb.Append("<td style='text-align:left;'><b>" + DetailsList[0].Valid_To_Date + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>MSP rate at the time of deposit (भण्डारण के समय समर्थन मूल्य भाव का भाव) </td>");
                    sb.Append("<td style='text-align:left;'><b> Rs. " + String.Format("{0:n}", DetailsList[0].Price) + " </b> </td>");
                    sb.Append("<td style='text-align:left;'>Valuation<br> मूल्यांकन </td>");
                    sb.Append("<td style='text-align:left;'><b>Rs. " + String.Format("{0:n}", totalPrice) + " </b></td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("<br/><table cellpadding='5' cellspacing='0' border='0' style='width:100%;margin-bottom: 10px;" + fontSize + "'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Date (दिनांक)  </td>");
                    sb.Append("<td style='text-align:left;'><p style='margin-left:5px;'><b>" + DateTime.Now.ToString("dd/MM/yyyy") + "</b></p></td>");
                    sb.Append("<td colspan='2' style='text-align:right;'><br> <b>Signature of the Warehouseman or his Agent<br/> भंडार प्रबंधक या उसके अभिकर्ता के हस्ताक्षर </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");


                    return sb.ToString();

                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return result;
        }


        [HttpPost]
        public string GetKVSS(int DistrictId)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            // return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objNew.GetPurchaseCenterKVSS(DistrictId));
            return COMMON_FUNCTION.DataTableToJSONString(objNew.GetPurchaseCenterKVSS(DistrictId));
        }

        [HttpPost]
        public string GetPurchaseCenterList(int DistrictId, int CommodityId)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            //return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objNew.GetPurchaseCenterList(DistrictId, CommodityId));
            return COMMON_FUNCTION.DataTableToJSONString(objNew.GetPurchaseCenterList(DistrictId, CommodityId));
        }
        [HttpPost]
        public string GetWarehouseList(bool IsCWC = false)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            return new MasterController().RajfedWarehouseDetails(_LoggedInUser.WarehouseId, IsCWC);
        }

        [HttpPost]
        public JsonResult GetWarehouseRSWCList(DataPagingModel Details, string RequestType)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int WarehouseId = _LoggedInUser.WarehouseId;
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.GetWarehouseRSWCList(ref Details, WarehouseId, RequestType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string GetFertilizerSupplierDashboardData()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            return JsonConvert.SerializeObject(objNew.GetFertilizerSupplierDashboardData(_LoggedInUser.UserRoleDeptMappingId), Formatting.Indented);
        }

        public ActionResult WarehouseLoanList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateRSWC(string ReceiptNo, string Type, string Value)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            return Json(new { status = objNew.UpdateRSWC(ReceiptNo, Type, Value) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WarehouseCommodityReleased()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetWarehouseRSWCReceiptDetails(string ReceiptNo)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.GetWarehouseRSWCDetails(ReceiptNo); ;
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetWarehouseReleaseOrder(DataPagingModel Details, string RequestType)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int WarehouseId = _LoggedInUser.WarehouseId;
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.GetWarehouseReleaseOrder(ref Details, RequestType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetWarehouseDeliveryOrder(DataPagingModel Details, string RequestType)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int WarehouseId = _LoggedInUser.WarehouseId;
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.GetWarehouseDeliveryOrder(ref Details, WarehouseId, RequestType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetWarehouseDeliveryOrderWRList(DataPagingModel Details, string RequestType)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int WarehouseId = _LoggedInUser.WarehouseId;
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.GetWarehouseDeliveryOrderWRList(ref Details, WarehouseId, RequestType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDeliveryOrder(DataPagingModel Details, string RequestType)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int WarehouseId = _LoggedInUser.WarehouseId;
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.GetDeliveryOrder(ref Details, RequestType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDeliveryOrderWRList(DataPagingModel Details, string RequestType)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int WarehouseId = _LoggedInUser.WarehouseId;
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.GetDeliveryOrderWRList(ref Details, RequestType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult InsertCommodityReleased(string OTP, string TXN, List<WarehouseRSWCModel> DetailsList)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            string result = objNew.InsertCommodityReleased(DetailsList);

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }


        public string WRCommodityReleased(string OTP, string TXN, List<WarehouseRSWCModel> DetailsList, string ReceiptNo)
        {
            string result = "1";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return "LOGOUT";
                }

                FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());

                var otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                if (otpResult.status != "1")
                {
                    return otpResult.data;
                }

                //result = objNew.InsertCommodityReleased(DetailsList);
                string requestNo = result;

                if (result == "-1" && result == "0")
                {
                    result = "Error";
                }
                else
                {

                    DataTable List = objNew.GetWarehouseRSWCDetails(ReceiptNo);
                    DataTable ReleasedCommodity = objNew.GetReleasedCommodity(ReceiptNo);
                    object sumOrderQty;

                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<html><head><style>.lable{font-size:30px!important;}</style></head><body>");

                    sb.Append("<div style='margin-right:15px;    margin-left:15px; padding:5px;'>");
                    int index = 1;
                    string requestFiles = "";
                    string filepath = Server.MapPath("~/Content/RSWC");
                    string devFontSize = "";
                    string fontSize = "font-size: 13px;";
                    sb.Append("<table id='content' border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");
                    sb.Append("<tr>");

                    sb.Append("<th colspan='3' style=''><div style='text-align:left;width:80px;float:left;'><img style='width:80px;height:80px' src='" + Server.MapPath("~/Content/Images/rswc_logo.png") + "' /></div>");

                    sb.Append("<div style='text-align:center;float:left;'><label style='font-size: 22px;'>RAJASTHAN STATE WAREHOUSING CORPORATION</label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 15px;'>(A GOVERNMENT UNDERTAKING)</label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 32px;'> " + UnicodeToKrutidev.GetKrutidev("राजस्थान राज्य भण्डार व्यवस्था निगम") + " </label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 30px;'> (सरकार का प्रतिष्ठान) </label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 18px;'>Warehouse Receipt- Nagotiable/Non-negotiable</label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 20px;'>" + UnicodeToKrutidev.GetKrutidev("भण्डार रसीद परक्राम्य") + "<label style='font-size: 18px;'>/</label>" + UnicodeToKrutidev.GetKrutidev("अपरक्राम्य") + "</label>");
                    sb.Append("</div></th>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style='text-align:center;border-bottom: 2px solid black;'>");
                    sb.Append("  ");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style='text-align:left;'>");
                    sb.Append("<table id='content' border='0' style='width:100%;" + fontSize + "' cellpadding='3' cellspacing='0'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;width:35%;'>Serial No. of Receipt (रसीद की क्रम संख्या) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> RSWC/" + List.Rows[0]["receipt_serial_no"] + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Name & Location of Warehouse (भण्डार गृह का नाम तथा स्थान) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> S/W " + List.Rows[0]["WarehousNameEn"] + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Warehousemans license No. (भण्डार प्रबंधन का लाइसेंस नंबर) </td>");
                    sb.Append("<td style='text-align:left;'><b> " + List.Rows[0]["lisence_no"] + " </b> </td>");
                    sb.Append("<td>Valid up to (तक मान्य) </td>");
                    sb.Append("<td>N.A.</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Received From (Name and address of depositor)<br /> प्राप्त किया (जमा कर्ता का नाम और पता) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> NAFED Through " + List.Rows[0]["SocietyName"] + " , " + List.Rows[0]["PurchaseCenterEng"] + "  </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Commodity (जिंस) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> " + List.Rows[0]["CommodityNameEn"] + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");



                    sb.Append("<tr><td style='" + devFontSize + "' colspan='3' >Goods of the following description (निम्नलिखित वर्णन का माल ) </td></tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style=''>");
                    sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + fontSize + "'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:center;font-size:10px;'>Sr.No<br> क्र.सं. </td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>Class or Standared Quality and/or Grade<br/> श्रेणी या मान्य गुण प्रकार तथा / अथवा मान </td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>No. of Packages or Lots <br/> बंडलों की संख्या </td>");
                    sb.Append("<td style='text-align:center;font-size:10px;width:70px;'>Unit<br/> यूनिट </td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>Net Quantity Measured in Qtls<br> नापी या तोली गयी शुद्ध मात्रा क्वि. में </td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>Name and/or Licence No. of the Weighter/Classifier/Sampler<br/> तोला / श्रेणी कर्ता / नमूना कर्ता का नाम तथा / अथवा लाइसेंस नंबर </td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>GNO/Stack No.<br/> गोदाम / स्टैक नंबर </td>");
                    sb.Append("</tr>");
                    decimal totalPrice = 0;
                    int totalLots = 0;
                    decimal totalNetQty = 0;
                    foreach (DataRow dr in List.Rows) // search whole table
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;font-size:10px;'>" + (index++) + "</td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["quality"] + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["packages_lots"] + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["UnitNameEng"] + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["net_quantity"] + "  </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["sampler_name"] + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + (string.IsNullOrWhiteSpace(dr["GoDownNameEng"].ToString()) ? dr["GoDownNameEng"] : dr["GoDownNameEng"]) + "/" + dr["stack_no"] + " </td>");
                        sb.Append("</tr>");
                        totalPrice += (Convert.ToDecimal(dr["net_quantity"])) * Convert.ToDecimal(dr["Price"]);
                        totalLots += Convert.ToInt32(dr["packages_lots"]);
                        totalNetQty += Convert.ToDecimal(dr["net_quantity"]);
                    }

                    if (List.Rows.Count > 1)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2'style='text-align:right'>Total<br> कुल </td>");
                        sb.Append("<td style='text-align:center'>" + totalLots + "</td>");
                        sb.Append("<td></td>");
                        sb.Append("<td style='text-align:center'>" + totalNetQty + "</td>");
                        sb.Append("<td></td>");
                        sb.Append("<td></td>");
                        sb.Append("</tr>");
                    }


                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");







                    sb.Append("<table id='content' border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;width:60%;'>Condition of Goods -(1) Good (2) Fair (3) Average<br/> माल की स्थिति - (1) उत्तम (2) सुंदर (3) साधारण  </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> " + (List.Rows[0]["goods_condition"].ToString() == "1" ? "Good" : List.Rows[0]["goods_condition"].ToString() == "2" ? "Fair" : "Average") + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Private marks of the depositor on the packages if any<br/> यदि हो तो बंडलों  पर जमाकर्ता  के निजी चिन्ह </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> " + List.Rows[0]["PrivateMarkofDepositor"] + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Rate of storage and other charges (भंडारण की दर और अन्य शुल्क) </td>");
                    sb.Append("<td colspan='3'  style='text-align:left;'><b> " + List.Rows[0]["StorageRateCharge"] + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Indemnified / Insured for Fire /Theft/Riots / Other Contingency with<br/> आग / चोरी / दंगों / आदि अन्य  आपत्तियों  के लिये बीमा </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> " + List.Rows[0]["IndemnifiedInsurance"] + "  </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>If Insured (Name Of Insurance Co.) (साथ यदि बीमा कराया  गया हो (बीमा समवाय का नाम)) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> " + List.Rows[0]["InsuredName"] + " </b> </td>");
                    sb.Append("</tr>");


                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Done for an amount of Rs. (रुपये की राशि के लिए किया गया) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> Rs. " + String.Format("{0:n}", totalPrice) + " </b> </td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>The goods are accepted for storage from (भण्डारण  के लिए  माल स्वीकार किया  जाता  हैं)  </td>");
                    sb.Append("<td style='text-align:left;'><b> " + List.Rows[0]["from_date"] + " </b> </td>");
                    sb.Append("<td style='text-align:left;'>to (से)    </td>");
                    sb.Append("<td style='text-align:left;'><b>" + List.Rows[0]["to_date"] + " </b> </td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Market rate at the time of deposit (भण्डारण के समय मंडी का भाव) </td>");
                    sb.Append("<td style='text-align:left;'><b> Rs. " + String.Format("{0:n}", List.Rows[0]["Price"]) + " </b> </td>");
                    sb.Append("<td style='text-align:left;'>Valuation<br> मूल्यांकन </td>");
                    sb.Append("<td style='text-align:left;'><b>Rs. " + String.Format("{0:n}", totalPrice) + " </b></td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");

                    sb.Append("<br/><table cellpadding='5' cellspacing='0' border='0' style='width:100%;margin-bottom: 10px;" + fontSize + "'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Date (दिनांक)  </td>");
                    sb.Append("<td style='text-align:left;'><p style='margin-left:5px;'><b>" + DateTime.Now.ToString("dd/MM/yyyy") + "</b></p></td>");
                    sb.Append("<td colspan='2' style='text-align:right;'><br> <b>Signature of the Warehouseman or his Agent<br/> भंडार प्रबंधक या उसके अभिकर्ता के हस्ताक्षर </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");

                    sb.Append("<br/><table cellpadding='5' cellspacing='0' border='0' style='width:100%;margin-bottom: 10px;" + fontSize + "'>");
                    sb.Append("<tr>");
                    sb.Append("<td  colspan='4' style='text-align:left;'><p style='text-indent: 20px;'>The goods mentioned below are hereby released from the receipt for delivery from Warehouse. Any unreleased balance of goods is subject to a lien for unpaid charges and advances on the released portion</p><p style='text-indent: 20px;'> निम्नलिखित वस्तुएं भण्डार गृह से निकालने के लिए एतदद्वार इस रसीद से हटाई जाती है। निकाले गये माल पर ली गई तथा वापिस न दी गई अग्रिम राशि तथा बकाया शुल्कों को न छोडे गये माल से पूरा किया जा सकेगा। </p></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='4' style=''>");
                    sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + fontSize + "'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:center;font-size:10px;'>Sr.No<br> क्र.सं. </td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>Date<br/> दिनांक </td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>Quantity Released<br/> निकाले  गये  माल की मात्रा </td>");
                    sb.Append("<td style='text-align:center;font-size:10px;width:70px;'>Unit<br/> यूनिट </td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>Signature<br/> हस्ताक्षर </td>");
                    sb.Append("<td style='text-align:center;font-size:10px;width:70px;'>Quantity due on Receipt<br/> रसीद पर बकाया माल की मात्रा  </td>");
                    sb.Append("</tr>");
                    index = 1;





                    int releasedQuantity = 0;

                    foreach (DataRow dr in ReleasedCommodity.Rows) // search whole table
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;font-size:10px;'>" + (index++) + "</td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["RealeaseDated"] + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["released_quantity"] + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["UnitNameEng"] + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'>  </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["RemainingQuantity"] + " </td>");
                        sb.Append("</tr>");
                        releasedQuantity += Convert.ToInt32(dr["released_quantity"]);
                    }





                    foreach (var dr in DetailsList) // search whole table
                    {

                        string fromDate = "";
                        if (!string.IsNullOrWhiteSpace(dr.Valid_From_Date))
                        {
                            fromDate = dr.Valid_From_Date.Split('/')[2] + "/" + dr.Valid_From_Date.Split('/')[1] + "/" + dr.Valid_From_Date.Split('/')[0];
                        }
                        int remaingQty = (Convert.ToInt32(dr.Packages_Lots) - Convert.ToInt32(dr.ReleasedQuantity) - Convert.ToInt32(dr.ReleaseQuantity));
                        if (remaingQty < 0)
                            remaingQty = 0;
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;font-size:10px;'>" + (index++) + "</td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.Valid_From_Date + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.ReleaseQuantity + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.Packages_Quantity_Unit + "  </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'>  </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + remaingQty + " </td>");
                        sb.Append("</tr>");
                        //releasedQuantity += Convert.ToInt32(dr.ReleaseQuantity);
                    }

                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");






                    sb.Append("</table>");



                    //sb.Append("<footer>");
                    //sb.Append("<br/><br/><table cellpadding='5' cellspacing='0' border='0' style='width:100%;margin-bottom: 10px;" + fontSize + "'>");

                    //sb.Append("</table>");
                    //sb.Append("</footer>");

                    //sb.Append("<br/><table cellpadding='5' cellspacing='0' border='0' style='width:100%;margin-bottom: 10px;" + fontSize + "'>");
                    //sb.Append("<tr>");
                    //sb.Append("<td  colspan='4' style='text-align:left;'>The goods mentioned below are hereby released from the receipt for delivery from Warehouse. Any unreleased balance of goods is subject to a lien for unpaid charges and advances on the released portion<br/> निम्नलिखित वस्तुएं भण्डार गृह से निकालने के लिए एतदद्वार इस रसीद से हटाई जाती है| निकाले गये माल पर ली गई तथा वापिस न दी गई अग्रिम राशि तथा बकाया शुल्कों को न छोड़े गये माल से पूरा किया जा सकेगा |   </td>");
                    //sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align:left;'>Date<br/> दिनांक  </td>");
                    //sb.Append("<td style='text-align:left;'><p style='margin-left:5px;'><b>" + DateTime.Now.ToString("dd/MM/yyyy") + "</b></p></td>");
                    //sb.Append("<td style='text-align:right;'><br> <b>Signature of the Warehouseman or his Agent<br/> भंडार प्रबंधक या उसके अभिकर्ता के हस्ताक्षर </b> </td>");
                    //sb.Append("</tr>");
                    //sb.Append("</table>");


                    sb.Append("</div>");
                    sb.Append("</body></html>");




                    if (!System.IO.Directory.Exists(filepath))
                    {
                        System.IO.Directory.CreateDirectory(filepath);
                    }
                    ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                    DataTable DT = new DataTable();

                    string FileName = "Relese_" + requestNo + "_" + ReceiptNo.Replace("/", "_");
                    string Receipt_No = ReceiptNo;
                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "15px"));
                    //System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    // sb1.Append(sb.ToString().Replace("<br>", "<br/>"));
                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName + ".pdf", ""))
                    {
                        var esignResult = COMMON_FUNCTION.EsinghPDF(otpResult.data, filepath + "/" + FileName + ".pdf");
                        result = esignResult.data;
                        if (esignResult.data == "1")
                        {

                            string requestNo1 = objNew.InsertCommodityReleased(DetailsList);

                            if (result == "-1" && result == "0")
                            {
                                result = "Error";
                            }
                            else
                            {
                                result = "Success";
                            }



                            DetailsList.ForEach(x =>
                            {
                                x.Id = "0";
                                x.EntryFile = FileName + ".pdf";
                                x.Receipt_No = Receipt_No;
                            });


                            objNew.InsertCommodityReleased(DetailsList, requestNo1).ToString();


                        }
                    }
                    else
                    {
                        result = "PDF Not Generate";
                    }
                }
            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.FertilizerSupply", FunctionName = " WRCommodityReleased(string OTP, string TXN, List<WarehouseRSWCModel> DetailsList, string ReceiptNo)" }, ex);
                result = "Error";
            }
            return result;
        }


        public string DOCommodityReleased(string OTP, string TXN, List<WarehouseRSWCModel> DataList)
        {
            string result = "1";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return "LOGOUT";
                }

                var otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                if (otpResult.status != "1")
                {
                    return otpResult.data;
                }

                FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());

                //result = objNew.InsertCommodityReleased(DetailsList);
                string requestNo = result;

                if (result == "-1" && result == "0")
                {
                    result = "Error";
                }
                else
                {
                    var reciept = DataList.Select(x => x.Receipt_No).Distinct().ToList();
                    string esignPdf = "";

                    foreach (var recpt in reciept)
                    {
                        List<WarehouseRSWCModel> DetailsList = DataList.Where(a => a.Receipt_No == recpt).ToList();
                        string ReceiptNo = DetailsList[0].Receipt_No;






                        DataTable List = objNew.GetWarehouseRSWCDetails(ReceiptNo);
                        List<WarehouseRSWCModel> model1 = new List<WarehouseRSWCModel>();

                        int releasedPackagesLots = DetailsList.Sum(x => Convert.ToInt32(x.ReleasedPackages_Lots)); ;
                        int totalPackagesLots = 0;
                        foreach (DataRow dr in List.Rows)
                        {
                            WarehouseRSWCModel model = new WarehouseRSWCModel();
                            model.OldReceipt_No = dr["receipt_serial_no"].ToString();
                            model.WarehousName = dr["WarehousNameEn"].ToString();
                            model.Lisence_No = dr["lisence_no"].ToString();
                            model.KvssName = dr["SocietyName"].ToString();
                            model.PurchaseCenter = dr["PurchaseCenterEng"].ToString();
                            model.CommodityName = dr["CommodityNameEn"].ToString();
                            model.Quality = dr["quality"].ToString();
                            model.Packages_Lots = dr["packages_lots"].ToString();
                            model.Packages_Quantity_Unit = dr["UnitNameEng"].ToString();
                            model.NetQuantity = dr["net_quantity"].ToString();
                            model.SamplerName = dr["sampler_name"].ToString();
                            model.SamplerName = dr["Price"].ToString();
                            model.GoDownName = dr["GoDownNameEng"].ToString();
                            model.ConditionOfGoods = dr["goods_condition"].ToString();
                            model.PrivateMarkofDepositor = dr["PrivateMarkofDepositor"].ToString();
                            model.StorageRateCharge = dr["StorageRateCharge"].ToString();
                            model.IndemnifiedInsurance = dr["IndemnifiedInsurance"].ToString();
                            model.InsuredName = dr["InsuredName"].ToString();
                            model.Valid_From_Date = dr["from_date"].ToString();
                            model.Valid_To_Date = dr["to_date"].ToString();
                            model.Kharif = dr["Kharif"].ToString();
                            model.Stack_No = dr["Stack_No"].ToString();
                            model.Price = dr["Price"].ToString();
                            model.QuantityNote = dr["QuantityNote"].ToString();
                            model.ReleasedPackages_Lots = dr["ReleasedPackagesLots"].ToString();
                            model.Crop_year = Convert.ToInt32(dr["crop_year"].ToString());
                            releasedPackagesLots += Convert.ToInt32(dr["ReleasedPackagesLots"].ToString());
                            totalPackagesLots += Convert.ToInt32(dr["Packages_Lots"].ToString());
                            model1.Add(model);
                        }

                        DataTable ReleasedCommodity = objNew.GetReleasedCommodity(ReceiptNo);

                        OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("<html><head><style>.lable{font-size:30px!important;}</style></head><body>");

                        sb.Append("<div style='margin-right:15px;    margin-left:15px; padding:5px;'>");
                        int index = 1;
                        string filepath = Server.MapPath("~/Content/RSWC");
                        string fontSize = "font-size: 13px;";
                        string WRHTML = GenerateWRHTML(model1);
                        sb.Append(WRHTML);
                        sb.Append("<br/><table cellpadding='5' cellspacing='0' border='0' style='width:100%;margin-bottom: 10px;" + fontSize + "'>");
                        sb.Append("<tr>");
                        sb.Append("<td  colspan='4' style='text-align:left;'><p style='text-indent: 20px;'>The goods mentioned below are hereby released from the receipt for delivery from Warehouse. Any unreleased balance of goods is subject to a lien for unpaid charges and advances on the released portion</p><p style='text-indent: 20px;'> निम्नलिखित वस्तुएं भण्डार गृह से निकालने के लिए एतदद्वार इस रसीद से हटाई जाती है। निकाले गये माल पर ली गई तथा वापिस न दी गई अग्रिम राशि तथा बकाया शुल्कों को न छोडे गये माल से पूरा किया जा सकेगा। </p></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='4' style=''>");
                        sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + fontSize + "'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;font-size:10px;'>Sr.No<br> क्र.सं. </td>");
                        sb.Append("<td style='text-align:center;font-size:10px'>Date<br/> दिनांक </td>");
                        sb.Append("<td style='text-align:center;font-size:10px'>Quantity Released<br/> निकाले  गये  माल की मात्रा </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;width:70px;'>Unit<br/> यूनिट </td>");
                        sb.Append("<td style='text-align:center;font-size:10px'>Signature<br/> हस्ताक्षर </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;width:70px;'>Quantity due on Receipt<br/> रसीद पर बकाया माल की मात्रा  </td>");
                        sb.Append("</tr>");
                        index = 1;

                        decimal releasedQuantity = DetailsList.Sum(x => Convert.ToDecimal(x.NetQuantity));

                        foreach (DataRow dr in ReleasedCommodity.Rows) // search whole table
                        {
                            sb.Append("<tr>");
                            sb.Append("<td style='text-align:center;font-size:10px;'>" + (index++) + "</td>");
                            sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["RealeaseDated"] + " </td>");
                            sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["released_quantity"] + " </td>");
                            sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["UnitNameEng"] + " </td>");
                            sb.Append("<td style='text-align:center;font-size:10px;'>  </td>");
                            sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["RemainingQuantity"] + " </td>");
                            sb.Append("</tr>");
                            releasedQuantity = Convert.ToDecimal(dr["RemainingQuantity"]);
                        }
                        var relesedQty = DetailsList.Sum(x => Convert.ToDecimal(x.ReleaseQuantity));
                        //foreach (var dr in DetailsList) // search whole table
                        //{

                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;font-size:10px;'>" + (index++) + "</td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + DetailsList[0].Valid_From_Date + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + relesedQty + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + DetailsList[0].Packages_Quantity_Unit + "  </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'>  </td>");

                        if (releasedPackagesLots >= totalPackagesLots)
                        {
                            sb.Append("<td style='text-align:center;font-size:10px;'>0</td>");
                        }
                        else
                        {
                            sb.Append("<td style='text-align:center;font-size:10px;'> " + (releasedQuantity - relesedQty) + " </td>");
                        }


                        sb.Append("</tr>");
                        //}

                        sb.Append("</table>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");
                        sb.Append("</div>");
                        sb.Append("</body></html>");




                        if (!System.IO.Directory.Exists(filepath))
                        {
                            System.IO.Directory.CreateDirectory(filepath);
                        }
                        ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                        DataTable DT = new DataTable();

                        string FileName = "Relese_" + (ReleasedCommodity.Rows.Count + 1) + "_" + ReceiptNo.Replace("/", "_");
                        string Receipt_No = ReceiptNo;
                        System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                        sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "15px"));
                        //System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                        // sb1.Append(sb.ToString().Replace("<br>", "<br/>"));
                        if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName + ".pdf", ""))
                        {

                            esignPdf += (string.IsNullOrWhiteSpace(esignPdf) ? "" : "#") + filepath + "/" + FileName + ".pdf";

                        }
                        else
                        {
                            return "PDF Not Generate";

                        }
                    }

                    var esignResult = COMMON_FUNCTION.EsinghPDF(TXN, esignPdf);
                    result = esignResult.data;
                    if (esignResult.status == "1")
                    {
                        int index = 0;


                        result = objNew.InsertReleasedCommodity(DataList, esignPdf.Split('#'));


                        //foreach (var recpt in reciept)
                        //{
                        //    List<WarehouseRSWCModel> DetailsList = DataList.Where(a => a.Receipt_No == recpt).ToList();
                        //    string ReceiptNo = DetailsList[0].Receipt_No;
                        //    DataTable ReleasedCommodity = objNew.GetReleasedCommodity(ReceiptNo);
                        //    string requestNo1 = objNew.InsertCommodityReleased(DetailsList);

                        //    if (result == "-1" && result == "0")
                        //    {
                        //        result = "Error";
                        //        return result;
                        //    }
                        //    else
                        //    {
                        //        result = "Success";
                        //    }
                        //    DetailsList.ForEach(x =>
                        //    {
                        //        x.Id = "0";
                        //        x.EntryFile = "Relese_" + (ReleasedCommodity.Rows.Count+1) + "_" + ReceiptNo.Replace("/", "_") + ".pdf";
                        //        x.Receipt_No = ReceiptNo;
                        //    });
                        //    objNew.InsertCommodityReleased(DetailsList, requestNo1).ToString();    
                        //}
                    }


                }
            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.FertilizerSupply", FunctionName = " DOCommodityReleased(string OTP, string TXN, List<WarehouseRSWCModel> DataList)" }, ex);
                result = "Error";
            }
            return result;
        }




        public ActionResult CommodityReleaseOrder()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ViewBag.CurrentData = DateTime.Now.ToString("yyyy/MM/dd");
            int WarehouseId = _LoggedInUser.WarehouseId;
            return View();
        }
        [HttpPost]
        public JsonResult GetCommodityReleaseOrderList(DataPagingModel Details, string RequestType)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = new DataTable();
            if (RequestType == "1" || RequestType == "0" || RequestType == "3")
                DT = objNew.GetCommodityReleaseOrderList(ref Details, RequestType);
            else
                DT = objNew.GetWarehouseReleaseOrder(ref Details, RequestType);

            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCommodityReleaseOrderDetails(string RequestNo)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = new DataTable();
            DT = objNew.GetCommodityReleaseOrderDetails(RequestNo);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public string InsertUpdateCommodityReleaseOrder(string OTP, string TXN, List<WarehouseRSWCModel> DetailsList)
        {
            string result = "";
            string OrderNo = "";
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            string uniqeId = Guid.NewGuid().ToString();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return "LOGOUT";

                }

                var otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                if (otpResult.status != "1")
                {
                    return otpResult.data;
                }


                result = objNew.InsertUpdateCommodityReleaseOrder(DetailsList, "", uniqeId).ToString();
                //string requestNo = objNew.InsertKVSSRequest(ReqestData);



                if (result == "-1" || result == "0")
                {
                    objNew.DeleteReleaseOrder(uniqeId);
                    result = "Error";
                }
                else
                {

                    DataTable List = objNew.GetCommodityReleaseOrderDetails(result);

                    object sumOrderQty;

                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<html><head><style>.lable{font-size:30px!important;}</style></head><body>");

                    sb.Append("<div style='margin-right:15px;    margin-left:15px; padding:5px;'>");
                    int index = 1;
                    string requestFiles = "";
                    string filepath = Server.MapPath("~/Content/RSWC");
                    string devFontSize = "";
                    string fontSize = "font-size: 13px;";
                    sb.Append("<table id='content' border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");
                    sb.Append("<tr>");

                    sb.Append("<th colspan='3' style='text-align:center;'>");

                    sb.Append("<br><label style='font-size: 22px;'>National Agricultural Cooperative Marketing Federation of India Limited</label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 15px;'> (NAFED) </label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 12px;'><b>Branch Office :</b> 3rd Floor, Nehru Sahkar Bhavan , Bhawani Singh Road, Jaipur-302001 (Rajasthan) </label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 18px;'>DELIVERY ORDER</label>");
                    //sb.Append("<br>");
                    //sb.Append("<label style='font-size: 32px;'> " + UnicodeToKrutidev.GetKrutidev("राजस्थान राज्य भण्डार व्यवस्था निगम") + " </label>");

                    //sb.Append("<br>");
                    //sb.Append("<label style='font-size: 18px;'>Warehouse Receipt- Nagotiable/Non-negotiable</label>");
                    //sb.Append("<br>");
                    //sb.Append("<label style='font-size: 20px;'>" + UnicodeToKrutidev.GetKrutidev("भण्डार रसीद परक्राम्य") + "<label style='font-size: 18px;'>/</label>" + UnicodeToKrutidev.GetKrutidev("अपरक्राम्य") + "</label>");
                    sb.Append("</th>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style='text-align:center;border-bottom: 2px solid black;'>");
                    sb.Append("  ");
                    sb.Append("</td>");
                    sb.Append("</tr>");


                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style='text-align:left;'>");
                    sb.Append("<table id='content' border='0' style='width:100%;" + fontSize + "' cellpadding='3' cellspacing='0'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;width:25%;'><b>DO. NO.</b> </td>");
                    sb.Append("<td  style='text-align:left;'>" + result + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'><b>DO. Date</b></td>");

                    // DetailsList[0].CompanyName
                    string fromDate = "";
                    if (!string.IsNullOrWhiteSpace(DetailsList[0].DateOfIssue))
                    {
                        fromDate = DetailsList[0].DateOfIssue;//.Split('/')[2] + "/" + DetailsList[0].DateOfIssue.Split('/')[1] + "/" + DetailsList[0].DateOfIssue.Split('/')[0];
                    }
                    else
                    {
                        fromDate = DateTime.Now.ToString("dd/MM/yyyy");
                    }

                    sb.Append("<td  style='text-align:left;'>" + fromDate + " </td>");

                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'><b>Warehouse</b></td>");
                    sb.Append("<td  style='text-align:left;'>" + List.Rows[0]["WarehousNameEn"] + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;vertical-align: top;'><b>Deliver To</b></td>");
                    sb.Append("<td style='text-align:left;'><b>Company Name :</b> " + DetailsList[0].CompanyName + "<br><b>Receiver Name :</b> " + DetailsList[0].ReceiverName + " S/O " + DetailsList[0].ReceiverFName.Replace("S/O", "") + "<br><b>Mobile :</b> " + DetailsList[0].MobileNo + "    <b>Email :</b> " + DetailsList[0].Email + "<br><b>Address :</b> " + DetailsList[0].Address + " </td>");

                    sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align:left;'>Received From (Name and address of depositor)<br /> प्राप्त किया (जमा कर्ता का नाम और पता) </td>");
                    //sb.Append("<td colspan='3' style='text-align:left;'><b> NAFED Through " + List.Rows[0]["DISTRICT_ENG"] + ", " + List.Rows[0]["SocietyName"] + " , " + List.Rows[0]["PurchaseCenterEng"] + "  </b> </td>");
                    //sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align:left;'>Commodity (जिंस) </td>");
                    //sb.Append("<td colspan='3' style='text-align:left;'><b> " + List.Rows[0]["CommodityNameEn"] + " </b> </td>");
                    //sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");







                    sb.Append("<tr><td style='" + devFontSize + "' colspan='3' >Goods of the following description (निम्नलिखित वर्णन का माल ) </td></tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style=''>");
                    sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + fontSize + "'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:center;font-size:10px;'>Sr.No<br> क्र.सं. </td>");
                    //sb.Append("<td style='text-align:center;font-size:10px;'>Warehouse<br> भण्डार गृह </td>");
                    sb.Append("<td style='text-align:center;font-size:10px;'>WR No<br> डब्ल्यू.आर. संख्या </td>");
                    sb.Append("<td style='text-align:center;font-size:10px;'>Commodity<br> जिंस </td>");

                    //sb.Append("<td style='text-align:center;font-size:10px;'>Physical Receipt Sr.No. <br> फिसिकल रसीद नंबर</td>");
                    sb.Append("<td style='text-align:center;font-size:10px;'>No of Packages or Lots<br> कुल बंडलों की संख्या </td>");
                    //sb.Append("<td style='text-align:center;font-size:10px;'>Released No of <br> Packages or Lots<br> जारी किये गये <br> बंडलों की संख्या </td>");
                    sb.Append("<td style='text-align:center;font-size:10px;'>Release Packages or Lots<br> जारी करने के लिए <br> बंडलों की संख्या </td>");
                    sb.Append("</tr>");
                    decimal totalPrice = 0;
                    int totalLots = 0;
                    decimal totalNetQty = 0;
                    foreach (DataRow dr in List.Rows) // search whole table
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;font-size:10px;'>" + (index++) + "</td>");
                        //sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["WarehousNameEn"] + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["receipt_no"] + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["CommodityNameEn"] + " </td>");
                        //sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["receipt_serial_no"] + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["packages_lots"] + " </td>");
                        //sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["ReleasedPackagesLots"] + "  </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["ReleaseOrderQuantity"] + " </td>");
                        sb.Append("</tr>");
                        totalPrice += (Convert.ToDecimal(dr["net_quantity"])) * Convert.ToDecimal(dr["Price"]);
                        totalLots += Convert.ToInt32(dr["packages_lots"]);
                        totalNetQty += Convert.ToDecimal(dr["net_quantity"]);
                    }

                    //if (List.Rows.Count > 1)
                    //{
                    //    sb.Append("<tr>");
                    //    sb.Append("<td colspan='2'style='text-align:right'>Total<br> कुल </td>");
                    //    sb.Append("<td style='text-align:center'>" + totalLots + "</td>");
                    //    sb.Append("<td></td>");
                    //    sb.Append("<td style='text-align:center'>" + totalNetQty + "</td>");
                    //    sb.Append("<td></td>");
                    //    sb.Append("<td></td>");
                    //    sb.Append("</tr>");
                    //}


                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");







                    //sb.Append("<table id='content' border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align:left;width:60%;'>Condition of Goods -(1) Good (2) Fair (3) Average<br/> माल की स्थिति - (1) उत्तम (2) सुंदर (3) साधारण  </td>");
                    //sb.Append("<td colspan='3' style='text-align:left;'><b> " + (List.Rows[0]["goods_condition"].ToString() == "1" ? "Good" : List.Rows[0]["goods_condition"].ToString() == "2" ? "Fair" : "Average") + " </b> </td>");
                    //sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align:left;'>Private marks of the depositor on the packages if any<br/> यदि हो तो बंडलों  पर जमाकर्ता  के निजी चिन्ह </td>");
                    //sb.Append("<td colspan='3' style='text-align:left;'><b> " + List.Rows[0]["PrivateMarkofDepositor"] + " </b> </td>");
                    //sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align:left;'>Rate of storage and other charges (भंडारण की दर और अन्य शुल्क) </td>");
                    //sb.Append("<td colspan='3'  style='text-align:left;'><b> " + List.Rows[0]["StorageRateCharge"] + " </b> </td>");
                    //sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align:left;'>Indemnified / Insured for Fire /Theft/Riots / Other Contingency with<br/> आग / चोरी / दंगों / आदि अन्य  आपत्तियों  के लिये बीमा </td>");
                    //sb.Append("<td colspan='3' style='text-align:left;'><b> " + List.Rows[0]["IndemnifiedInsurance"] + "  </b> </td>");
                    //sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align:left;'>If Insured (Name Of Insurance Co.) (साथ यदि बीमा कराया  गया हो (बीमा समवाय का नाम)) </td>");
                    //sb.Append("<td colspan='3' style='text-align:left;'><b> " + List.Rows[0]["InsuredName"] + " </b> </td>");
                    //sb.Append("</tr>");


                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align:left;'>Done for an amount of Rs. (रुपये की राशि के लिए किया गया) </td>");
                    //sb.Append("<td colspan='3' style='text-align:left;'><b> Rs. " + String.Format("{0:n}", totalPrice) + " </b> </td>");
                    //sb.Append("</tr>");

                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align:left;'>The goods are accepted for storage from (भण्डारण  के लिए  माल स्वीकार किया  जाता  हैं)  </td>");
                    //sb.Append("<td style='text-align:left;'><b> " + List.Rows[0]["from_date"] + " </b> </td>");
                    //sb.Append("<td style='text-align:left;'>to (से)    </td>");
                    //sb.Append("<td style='text-align:left;'><b>" + List.Rows[0]["to_date"] + " </b> </td>");
                    //sb.Append("</tr>");

                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align:left;'>Market rate at the time of deposit (भण्डारण के समय मंडी का भाव) </td>");
                    //sb.Append("<td style='text-align:left;'><b> Rs. " + String.Format("{0:n}", List.Rows[0]["Price"]) + " </b> </td>");
                    //sb.Append("<td style='text-align:left;'>Valuation<br> मूल्यांकन </td>");
                    //sb.Append("<td style='text-align:left;'><b>Rs. " + String.Format("{0:n}", totalPrice) + " </b></td>");
                    //sb.Append("</tr>");
                    //sb.Append("</table>");

                    sb.Append("<br/><table cellpadding='5' cellspacing='0' border='0' style='width:100%;margin-bottom: 10px;" + fontSize + "'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Date (दिनांक)  </td>");
                    sb.Append("<td style='text-align:left;'><p style='margin-left:5px;'><b>" + DateTime.Now.ToString("dd/MM/yyyy") + "</b></p></td>");
                    sb.Append("<td colspan='2' style='text-align:right;'><br> <b>Authority Signature<br/> अधिकारी हस्ताक्षर </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");

                    sb.Append("</div>");
                    sb.Append("</body></html>");





                    if (!System.IO.Directory.Exists(filepath))
                    {
                        System.IO.Directory.CreateDirectory(filepath);
                    }
                    ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                    DataTable DT = new DataTable();

                    string FileName = "DeliveryOrder_" + result;
                    string Receipt_No = result;
                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "15px"));
                    //System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    // sb1.Append(sb.ToString().Replace("<br>", "<br/>"));
                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName + ".pdf", ""))
                    {
                        var esignResult = COMMON_FUNCTION.EsinghPDF(otpResult.data, filepath + "/" + FileName + ".pdf");
                        result = esignResult.data;
                        if (esignResult.status == "1")
                        {


                            DetailsList.ForEach(x =>
                            {
                                x.Id = "1";
                                x.EntryFile = FileName + ".pdf";
                                //x.Receipt_No = Receipt_No;
                            });
                            objNew.InsertUpdateCommodityReleaseOrder(DetailsList, Receipt_No, uniqeId).ToString();
                        }
                        else { objNew.DeleteReleaseOrder(uniqeId); }
                    }
                    else
                    {
                        objNew.DeleteReleaseOrder(uniqeId);
                        result = "PDF Not Generate";
                    }
                }
            }
            catch (System.Exception ex)
            {
                objNew.DeleteReleaseOrder(uniqeId);
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.FertilizerSupply", FunctionName = "InsertUpdateCommodityReleaseOrder(string OTP, string TXN, List<WarehouseRSWCModel> DetailsList)" }, ex);
                result = "Error";
            }
            return result;
        }


        [HttpPost]
        public string InsertUpdateReleaseOrder(string OTP, string TXN, List<WarehouseRSWCModel> DetailsList)
        {
            string result = "";
            string OrderNo = "";
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            string uniqeId = Guid.NewGuid().ToString();
            try
            {


                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return "LOGOUT";

                }

                var otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                if (otpResult.status != "1")
                {
                    return otpResult.data;
                }

                result = objNew.InsertUpdateCommodityReleaseOrder(DetailsList, "", uniqeId).ToString();

                if (result == "-1" || result == "0")
                {
                    objNew.DeleteReleaseOrder(uniqeId);
                    result = "Error";
                }
                else
                {

                    //DataTable List = objNew.GetCommodityReleaseOrderDetails(result);                    

                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<html><head><style>.lable{font-size:30px!important;}</style></head><body>");

                    sb.Append("<div style='margin-right:15px;    margin-left:15px; padding:5px;'>");
                    int index = 1;
                    string filepath = Server.MapPath("~/Content/RSWC");
                    string devFontSize = "";
                    string fontSize = "font-size: 13px;";
                    sb.Append("<table id='content' border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");
                    sb.Append("<tr>");

                    sb.Append("<th colspan='3' style='text-align:center;'>");

                    sb.Append("<br><label style='font-size: 22px;'>National Agricultural Cooperative Marketing Federation of India Limited</label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 15px;'> (NAFED) </label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 12px;'><b>Branch Office :</b> 3rd Floor, Nehru Sahkar Bhavan , Bhawani Singh Road, Jaipur-302001 (Rajasthan) </label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 18px;'>DELIVERY ORDER</label>");

                    sb.Append("</th>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style='text-align:center;border-bottom: 2px solid black;'>");
                    sb.Append("  ");
                    sb.Append("</td>");
                    sb.Append("</tr>");


                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style='text-align:left;'>");
                    sb.Append("<table id='content' border='0' style='width:100%;" + fontSize + "' cellpadding='3' cellspacing='0'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;width:25%;'><b>DO. NO.</b> </td>");
                    sb.Append("<td  style='text-align:left;'>" + result + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'><b>DO. Date</b></td>");

                    string fromDate = "";
                    if (!string.IsNullOrWhiteSpace(DetailsList[0].DateOfIssue))
                    {
                        fromDate = DetailsList[0].DateOfIssue;//.Split('/')[2] + "/" + DetailsList[0].DateOfIssue.Split('/')[1] + "/" + DetailsList[0].DateOfIssue.Split('/')[0];
                    }
                    else
                    {
                        fromDate = DateTime.Now.ToString("dd/MM/yyyy");
                    }


                    sb.Append("<td  style='text-align:left;'>" + fromDate + " </td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'><b>Warehouse</b></td>");
                    sb.Append("<td  style='text-align:left;'>" + DetailsList[0].WarehousName + " </td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'><b>Commodity</b></td>");
                    sb.Append("<td  style='text-align:left;'>" + DetailsList[0].CommodityName + " / " + DetailsList[0].FY + " </td>");
                    sb.Append("</tr>");


                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;vertical-align: top;'><b>Deliver To</b></td>");
                    sb.Append("<td style='text-align:left;'><b>Company Name :</b> " + DetailsList[0].CompanyName + "<br><b>Receiver Name :</b> " + DetailsList[0].ReceiverName + " S/O " + (string.IsNullOrWhiteSpace(DetailsList[0].ReceiverFName) ? "" : DetailsList[0].ReceiverFName.Replace("S/O", "")) + "<br><b>Mobile :</b> " + DetailsList[0].MobileNo + "    <b>Email :</b> " + DetailsList[0].Email + "<br><b>Address :</b> " + DetailsList[0].Address + " </td>");
                    sb.Append("</tr>");




                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");


                    sb.Append("<tr><td style='" + devFontSize + "' colspan='3' >Goods of the following description (निम्नलिखित वर्णन का माल ) </td></tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style=''>");
                    sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + fontSize + "'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:center;font-size:10px;'>Sr.No<br> क्र.सं. </td>");

                    //sb.Append("<td style='text-align:center;font-size:10px;'>Warehouse<br> वेयरहाउस  </td>");
                    //sb.Append("<td style='text-align:center;font-size:10px;'>Commodity<br> जिंस </td>");
                    //sb.Append("<td style='text-align:center;font-size:10px;'>Financial Year <br> वित्तीय वर्ष </td>");                
                    sb.Append("<td style='text-align:center;font-size:10px;'>Total Quantity(Quintal)<br> कुल मात्रा (क्विंटल) </td>");
                    sb.Append("<td style='text-align:center;font-size:10px;'>Total Released Quantity(Quintal)<br> कुल रिलीज की गयी मात्रा (क्विंटल) </td>");
                    sb.Append("<td style='text-align:center;font-size:10px;'>Total Quantity to be Release (Quintal)<br> रिलीज होने के लिए जारी कुल मात्रा (क्विंटल) </td>");
                    sb.Append("</tr>");
                    decimal totalPrice = 0;
                    int totalLots = 0;
                    decimal totalNetQty = 0;
                    foreach (var dr in DetailsList) // search whole table
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;font-size:10px;'>" + (index++) + "</td>");
                        //sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["WarehousNameEn"] + " </td>");
                        //sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.WarehousName + " </td>");
                        //sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.CommodityName + " </td>");
                        //sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.FY + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.NetQuantity + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.ReleasedQuantity + "  </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.ReleaseQuantity + " </td>");
                        sb.Append("</tr>");
                        //totalPrice += (Convert.ToDecimal(dr["net_quantity"])) * Convert.ToDecimal(dr["Price"]);
                        //totalLots += Convert.ToInt32(dr["packages_lots"]);
                        //totalNetQty += Convert.ToDecimal(dr["net_quantity"]);
                    }


                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");


                    sb.Append("<br/><table cellpadding='5' cellspacing='0' border='0' style='width:100%;margin-bottom: 10px;" + fontSize + "'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Date (दिनांक)  </td>");
                    sb.Append("<td style='text-align:left;'><p style='margin-left:5px;'><b>" + DateTime.Now.ToString("dd/MM/yyyy") + "</b></p></td>");
                    sb.Append("<td colspan='2' style='text-align:right;'><br> <b>Authority Signature<br/> अधिकारी हस्ताक्षर </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");

                    sb.Append("</div>");
                    sb.Append("</body></html>");





                    if (!System.IO.Directory.Exists(filepath))
                    {
                        System.IO.Directory.CreateDirectory(filepath);
                    }
                    ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                    DataTable DT = new DataTable();

                    string FileName = "DeliveryOrder_" + result;
                    string Receipt_No = result;
                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "15px"));

                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName + ".pdf", ""))
                    {
                        var esignResult = COMMON_FUNCTION.EsinghPDF(otpResult.data, filepath + "/" + FileName + ".pdf");
                        result = esignResult.data;
                        if (esignResult.status == "1")
                        {


                            DetailsList.ForEach(x =>
                            {
                                x.Id = "1";
                                x.EntryFile = FileName + ".pdf";
                                //x.Receipt_No = Receipt_No;
                            });
                            objNew.InsertUpdateCommodityReleaseOrder(DetailsList, Receipt_No, uniqeId).ToString();
                        }
                        else { objNew.DeleteReleaseOrder(uniqeId); }
                    }
                    else
                    {
                        objNew.DeleteReleaseOrder(uniqeId);
                        result = "PDF Not Generate";
                    }
                }
            }
            catch (System.Exception ex)
            {
                objNew.DeleteReleaseOrder(uniqeId);
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.FertilizerSupply", FunctionName = " InsertUpdateReleaseOrder(string OTP, string TXN, List<WarehouseRSWCModel> DetailsList)" }, ex);
                result = "Error";
            }
            return result;
        }


        [HttpPost]
        public string SendOTPOnAADHAR(string _AadharNo)
        {
            return AADHARSerice.SendOTPOnAADHAR(_AadharNo);
        }

        [HttpPost]
        public string VeryifyOTPsentOnAADHAR(string OTP, string TXN, string _EncryptedAadharNo)
        {
            return AADHARSerice.VeryifyOTPsentOnAADHAR(OTP, TXN, _EncryptedAadharNo);
        }





        #endregion

        public ActionResult WRSummary()
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

        [HttpPost]
        public JsonResult GetWRSummaryList(DataPagingModel Details, string RequestType)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int WarehouseId = _LoggedInUser.WarehouseId;
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.GetWRSummaryList(ref Details, RequestType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWRSummaryDetailsList(string RequestNo)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.GetWRSummaryDetailsList(RequestNo);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string InsertWRSummary(string OTP, string TXN, List<WarehouseRSWCModel> DetailsList)
        {
            string result = "";
            string OrderNo = "";
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            string uniqeId = Guid.NewGuid().ToString();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return "LOGOUT";
                }

                var otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                if (otpResult.status != "1")
                {
                    return otpResult.data;
                }


                result = objNew.InsertUpdateWRSummary(DetailsList, "", uniqeId).ToString();
                //string requestNo = objNew.InsertKVSSRequest(ReqestData);



                if (result == "-1" && result == "0")
                {
                    objNew.DeleteWRSummary(uniqeId);
                    result = "Error";
                }
                else
                {

                    string RO = "";

                    DataTable List = objNew.GetWRSummaryDetailsList(result);

                    //foreach (DataRow wr in List.Rows)
                    //{

                    //    if (Array.IndexOf(RO.Split(','), wr["RONameEn"].ToString()) < 0)
                    //        RO += (RO != "" ? "," : "") + wr["RONameEn"];
                    //}


                    object sumOrderQty;

                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<html><head><style>.lable{font-size:30px!important;}</style></head><body>");

                    sb.Append("<div style='margin-right:15px;    margin-left:15px; padding:5px;'>");
                    int index = 1;
                    string requestFiles = "";
                    string filepath = Server.MapPath("~/Content/RSWC");
                    string devFontSize = "";
                    string fontSize = "font-size: 15px;";
                    sb.Append("<table id='content' border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");
                    sb.Append("<tr>");

                    sb.Append("<th colspan='3' style=''><div style='text-align:left;width:80px;float:left;'><img style='width:80px;height:80px' src='" + Server.MapPath("~/Content/Images/rajfed_logo.jpg") + "' /></div>");

                    sb.Append("<div style='text-align:center;float:left;'><label style='font-size: 28px;'>" + UnicodeToKrutidev.GetKrutidev("राजस्थान राज्य सहकारी क्रय-विक्रय संघ लि.") + "  </label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 15px;'>RAJASTHAN STATE CO-OPERATIVE MARKETING FEDERATION LTD.</label>");
                    //sb.Append("<br>");
                    //sb.Append("<label style='font-size: 32px;'> " + UnicodeToKrutidev.GetKrutidev("राजस्थान राज्य भण्डार व्यवस्था निगम") + " </label>");
                    //sb.Append("<br>");
                    //sb.Append("<label style='font-size: 30px;'> (सरकार का प्रतिष्ठान) </label>");
                    //sb.Append("<br>");
                    //sb.Append("<label style='font-size: 18px;'>Warehouse Receipt- Nagotiable/Non-negotiable</label>");
                    //sb.Append("<br>");
                    //sb.Append("<label style='font-size: 20px;'>" + UnicodeToKrutidev.GetKrutidev("भण्डार रसीद परक्राम्य") + "<label style='font-size: 18px;'>/</label>" + UnicodeToKrutidev.GetKrutidev("अपरक्राम्य") + "</label>");
                    sb.Append("</div></th>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style='text-align:center;border-bottom: 2px solid black;'>");
                    sb.Append("  ");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    string commodityName = "";
                    //string comodity=""
                    foreach (DataRow dr in List.Rows) // search whole table
                    {
                        commodityName += (commodityName == "" ? "" : ",") + dr["CommodityNameEn"];
                    }
                    commodityName = string.Join(",", commodityName.Split(',').Distinct().ToArray());

                    sb.Append("<tr><td style='text-align:left;" + devFontSize + "' colspan='3' > क्रमांक : " + commodityName + " / WR / " + List.Rows[0]["Kharif"] + " / " + List.Rows[0]["FY"] + "/ " + result + " </td></tr>");
                    //sb.Append("<tr><td style='text-align:left;" + devFontSize + "' colspan='3' > शाखा प्रबंधक <br /> नेफैड <br /> जयपुर । </td></tr>");
                    sb.Append("<tr><td style='text-align:left;" + devFontSize + "' colspan='3' > शाखा प्रबंधक <br /> " + DetailsList[0].AgencyName + "<br /> जयपुर । </td></tr>");
                    sb.Append("<tr><td style='text-align:left;" + devFontSize + "' colspan='3' ><p style='margin-left:20px'> विषय :- समर्थन मूल्य पर " + commodityName + " " + List.Rows[0]["Kharif"] + " " + List.Rows[0]["FY"] + " की  WR भिजवाने  बाबत । </p></td></tr>");
                    sb.Append("<tr><td style='text-align:left;" + devFontSize + "' colspan='3' > महोदय, </td></tr>");
                    sb.Append("<tr><td style='text-align:left;" + devFontSize + "' colspan='3' ><p style='margin-left:10px'> विषयान्तर्गत निवेदन हैं कि  समर्थन मूल्य  पर " + commodityName + " " + List.Rows[0]["Kharif"] + " " + List.Rows[0]["FY"] + " की WR सलग्न कर निम्नानुसार भिजवाई जा रही  हैं :- </p></td></tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style=''>");
                    sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + fontSize + "'>");
                    sb.Append("<tr>");
                    sb.Append("<th style='text-align:center;font-size:10px;'>Sr.No</th>");
                    sb.Append("<th style='text-align:left;font-size:10px;'>R.O.</th>");
                    sb.Append("<th style='text-align:left;font-size:10px;'>Warehous </th>");
                    sb.Append("<th style='text-align:left;font-size:10px;'>KVSS </th>");
                    sb.Append("<th style='text-align:left;font-size:10px;'>Purchase Cente </th>");
                    sb.Append("<th style='text-align:left;font-size:10px;'>WR No.</th>");
                    sb.Append("<th style='text-align:center;font-size:10px;'>Date</th>");
                    sb.Append("<th style='text-align:right;font-size:10px;'>No. of Lots </th>");
                    sb.Append("<th style='text-align:right;font-size:10px;'>Weight(Quintal)</th>");
                    //sb.Append("<td style='text-align:center;font-size:10px;'>Total Value</td>");
                    sb.Append("</tr>");
                    decimal totalPrice = 0;
                    int totalLots = 0;
                    decimal totalNetQty = 0;
                    decimal rowTotalNetQty = 0;
                    int rowTotalLots = 0;
                    string prevRO = "A";
                    string prevKVSS = "A";
                    string pCenter = "A";
                    foreach (DataRow dr in List.Rows) // search whole table
                    {

                        if (pCenter != dr["PurchaseCenterEng"].ToString() && pCenter != "A")
                        {
                            sb.Append("<tr>");
                            sb.Append("<th  colspan='7' style='text-align:right;font-size:10px;'> Total </th>");
                            sb.Append("<th style='text-align:right;font-size:10px;'> " + rowTotalLots + " </th>");
                            sb.Append("<th style='text-align:right;font-size:10px;'> " + rowTotalNetQty + "  </th>");
                            sb.Append("</tr>");
                            rowTotalNetQty = 0;
                            rowTotalLots = 0;
                        }
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;font-size:10px;'>" + (index) + "</td>");
                        sb.Append("<td style='text-align:left;font-size:10px;'> " + UnicodeToKrutidev.GetKrutidev(dr["RONameEn"].ToString()) + " </td>");
                        sb.Append("<td style='text-align:left;font-size:10px;'> " + UnicodeToKrutidev.GetKrutidev(dr["WarehousNameEn"].ToString()) + " </td>");
                        sb.Append("<td style='text-align:left;font-size:15px;'> " + UnicodeToKrutidev.GetKrutidev(dr["SocietyName"].ToString()) + "    </td>");
                        //sb.Append("<td style='text-align:left;font-size:10px;'> " + (prevRO == dr["RONameEn"].ToString() ? "" : UnicodeToKrutidev.GetKrutidev(dr["RONameEn"].ToString())) + " </td>");
                        //sb.Append("<td style='text-align:left;font-size:15px;'> " + (prevKVSS == dr["SocietyName"].ToString() ? "" : UnicodeToKrutidev.GetKrutidev(dr["SocietyName"].ToString())) + "    </td>");
                        sb.Append("<td style='text-align:left;font-size:10px;'> " + UnicodeToKrutidev.GetKrutidev(dr["PurchaseCenterEng"].ToString()) + " </td>");
                        sb.Append("<td style='text-align:left;font-size:10px;'> " + dr["receipt_serial_no"] + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["WRGenerateDate"] + " </td>");
                        sb.Append("<td style='text-align:right;font-size:10px;'> " + dr["packages_lots"] + " </td>");
                        sb.Append("<td style='text-align:right;font-size:10px;'> " + dr["net_quantity"] + "  </td>");
                        //sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["TotalProce"] + " </td>");
                        sb.Append("</tr>");
                        rowTotalNetQty += Convert.ToDecimal(dr["net_quantity"]);
                        rowTotalLots += Convert.ToInt32(dr["packages_lots"]);


                        if (index >= List.Rows.Count)
                        {
                            //if (prevKVSS != dr["SocietyName"].ToString() && prevKVSS != "A")
                            {
                                sb.Append("<tr>");
                                sb.Append("<th  colspan='7' style='text-align:right;font-size:10px;'> Total </th>");
                                sb.Append("<th style='text-align:right;font-size:10px;'> " + rowTotalLots + " </th>");
                                sb.Append("<th style='text-align:right;font-size:10px;'> " + rowTotalNetQty + "  </th>");
                                sb.Append("</tr>");
                                rowTotalNetQty = 0;
                                rowTotalLots = 0;
                            }
                        }


                        prevKVSS = dr["SocietyName"].ToString();
                        prevRO = dr["RONameEn"].ToString();
                        pCenter = dr["PurchaseCenterEng"].ToString();


                        totalPrice += (Convert.ToDecimal(dr["net_quantity"])) * Convert.ToDecimal(dr["Price"]);
                        totalLots += Convert.ToInt32(dr["packages_lots"]);
                        totalNetQty += Convert.ToDecimal(dr["net_quantity"]);
                        ++index;
                    }

                    if (List.Rows.Count > 1)
                    {
                        sb.Append("<tr>");
                        sb.Append("<th   colspan='7' style='text-align:right;font-size:10px;'>G. Total</th>");
                        sb.Append("<th  style='text-align:right;font-size:10px;'>" + totalLots + "</th>");
                        sb.Append("<th  style='text-align:right;font-size:10px;'>" + totalNetQty + "</th>");
                        sb.Append("</tr>");
                    }


                    if (List.Rows.Count > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<th   colspan='7' style='text-align:right;font-size:10px;'>Previous Total of WR's</th>");
                        sb.Append("<th  style='text-align:right;font-size:10px;'>" + List.Rows[0]["PrevTotalPackages_Lots"] + "</th>");
                        sb.Append("<th  style='text-align:right;font-size:10px;'>" + List.Rows[0]["PrevTotalNet_quantity"] + "</th>");
                        sb.Append("</tr>");
                    }


                    if (List.Rows.Count > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<th   colspan='7' style='text-align:right;font-size:10px;'>G. Total</th>");
                        sb.Append("<th  style='text-align:right;font-size:10px;'>" + (totalLots + Convert.ToDecimal(List.Rows[0]["PrevTotalPackages_Lots"])) + "</th>");
                        sb.Append("<th  style='text-align:right;font-size:10px;'>" + (totalNetQty + Convert.ToDecimal(List.Rows[0]["PrevTotalNet_quantity"])) + "</th>");
                        sb.Append("</tr>");
                    }



                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");






                    sb.Append("<br/><table cellpadding='5' cellspacing='0' border='0' style='width:100%;margin-bottom: 10px;" + fontSize + "'>");

                    sb.Append("<tr>");
                    sb.Append("<td colspan='3'  style='text-align:left;'>Enclosed:-" + List.Rows.Count + " WR </td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Date (दिनांक) :- " + DateTime.Now.ToString("dd/MM/yyyy") + "  </td>");
                    //sb.Append("<td colspan='2' style='text-align:right;'><br> <b> भवदीय <br/><br/> सहायक रजिस्ट्रार  </b> </td>");
                    sb.Append("<td colspan='2' style='text-align:right;'><br> <b> भवदीय <br/><br/> अतिरिक्त रजिस्ट्रार  </b> </td>");

                    sb.Append("</tr>");
                    sb.Append("</table>");

                    sb.Append("</div>");
                    sb.Append("</body></html>");





                    if (!System.IO.Directory.Exists(filepath))
                    {
                        System.IO.Directory.CreateDirectory(filepath);
                    }
                    ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                    DataTable DT = new DataTable();

                    string FileName = "WRSummary_" + result;
                    string Receipt_No = result;
                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "20px"));
                    //System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    // sb1.Append(sb.ToString().Replace("<br>", "<br/>"));
                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName + ".pdf", ""))
                    {
                        var esignResult = COMMON_FUNCTION.EsinghPDF(otpResult.data, filepath + "/" + FileName + ".pdf");
                        result = esignResult.data;
                        if (result == "Success")
                        {


                            DetailsList.ForEach(x =>
                            {
                                x.Id = "1";
                                x.EntryFile = FileName + ".pdf";
                                //x.Receipt_No = Receipt_No;
                            });
                            objNew.InsertUpdateWRSummary(DetailsList, Receipt_No, uniqeId).ToString();


                        }
                        else
                        {
                            objNew.DeleteWRSummary(uniqeId);
                        }
                    }
                    else
                    {
                        objNew.DeleteWRSummary(uniqeId);
                        result = "PDF Not Generate";
                    }
                }
            }
            catch (System.Exception ex)
            {
                objNew.DeleteWRSummary(uniqeId);
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.FertilizerSupply", FunctionName = "InsertWRSummary(string OTP, string TXN, List<WarehouseRSWCModel> DetailsList)" }, ex);
                result = "Error";
            }
            return result;
        }


        public ActionResult WRPayment()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetWRPaymentList(DataPagingModel Details, string RequestType)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.GetRajfedWRSummary(ref Details, RequestType); //objNew.GetRajfedWRSummaryPayment(ref Details);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public string InsertWRPayment(List<WarehouseRSWCModel> DetailsList)
        {
            string result = "";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return "LOGOUT";
                }
                FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());

                result = objNew.InsertUpdateWRPayment(DetailsList).ToString();

                if (result == "1")
                {
                    result = "Success";
                }
                else
                {
                    result = "Faild";
                }

                //if (result == "-1" && result == "0")
                //{
                //    result = "Error";
                //}
                //else
                //{
                //    DataTable List = objNew.GetWRSummaryDetailsList(result);
                //    object sumOrderQty;
                //    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                //    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                //    sb.Append("<html><head><style>.lable{font-size:30px!important;}</style></head><body>");

                //    sb.Append("<div style='margin-right:15px;    margin-left:15px; padding:5px;'>");
                //    int index = 1;
                //    string requestFiles = "";
                //    string filepath = Server.MapPath("~/Content/RSWC");
                //    string devFontSize = "";
                //    string fontSize = "font-size: 13px;";
                //    sb.Append("<table id='content' border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");
                //    sb.Append("<tr>");

                //    sb.Append("<th colspan='3' style=''><div style='text-align:left;width:80px;float:left;'><img style='width:80px;height:80px' src='" + Server.MapPath("~/Content/Images/rswc_logo.png") + "' /></div>");

                //    sb.Append("<div style='text-align:center;float:left;'><label style='font-size: 22px;'>RAJASTHAN STATE WAREHOUSING CORPORATION</label>");
                //    sb.Append("<br>");
                //    sb.Append("<label style='font-size: 15px;'>(A GOVERNMENT UNDERTAKING)</label>");
                //    sb.Append("<br>");
                //    sb.Append("<label style='font-size: 32px;'> " + UnicodeToKrutidev.GetKrutidev("राजस्थान राज्य भण्डार व्यवस्था निगम") + " </label>");
                //    sb.Append("<br>");
                //    sb.Append("<label style='font-size: 30px;'> (सरकार का प्रतिष्ठान) </label>");
                //    sb.Append("<br>");
                //    sb.Append("<label style='font-size: 18px;'>Warehouse Receipt- Nagotiable/Non-negotiable</label>");
                //    sb.Append("<br>");
                //    sb.Append("<label style='font-size: 20px;'>" + UnicodeToKrutidev.GetKrutidev("भण्डार रसीद परक्राम्य") + "<label style='font-size: 18px;'>/</label>" + UnicodeToKrutidev.GetKrutidev("अपरक्राम्य") + "</label>");
                //    sb.Append("</div></th>");
                //    sb.Append("</tr>");
                //    sb.Append("<tr>");
                //    sb.Append("<td colspan='3' style='text-align:center;border-bottom: 2px solid black;'>");
                //    sb.Append("  ");
                //    sb.Append("</td>");
                //    sb.Append("</tr>");
                //    sb.Append("<tr><td style='" + devFontSize + "' colspan='3' >Goods of the following description (निम्नलिखित वर्णन का माल ) </td></tr>");
                //    sb.Append("<tr>");
                //    sb.Append("<td colspan='3' style=''>");
                //    sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + fontSize + "'>");
                //    sb.Append("<tr>");
                //    sb.Append("<td style='text-align:center;font-size:10px;'>Sr.No<br> क्र.सं. </td>");
                //    sb.Append("<td style='text-align:center;font-size:10px;'>Warehouse<br> भण्डार गृह </td>");
                //    sb.Append("<td style='text-align:center;font-size:10px;'>Receipt No<br> रसीद संख्या </td>");
                //    sb.Append("<td style='text-align:center;font-size:10px;'>Physical Receipt Sr.No. <br> फिसिकल रसीद नंबर</td>");
                //    sb.Append("<td style='text-align:center;font-size:10px;'>No of Packages or Lots<br> बंडलों की संख्या </td>");
                //    sb.Append("<td style='text-align:center;font-size:10px;'>Quantity<br> मात्रा </td>");
                //    sb.Append("<td style='text-align:center;font-size:10px;'>Total Value<br> कुल मूल्य </td>");
                //    sb.Append("</tr>");
                //    decimal totalPrice = 0;
                //    int totalLots = 0;
                //    decimal totalNetQty = 0;
                //    foreach (DataRow dr in List.Rows) // search whole table
                //    {
                //        sb.Append("<tr>");
                //        sb.Append("<td style='text-align:center;font-size:10px;'>" + (index++) + "</td>");
                //        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["WarehousNameEn"] + " </td>");
                //        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["receipt_no"] + " </td>");
                //        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["receipt_serial_no"] + " </td>");
                //        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["packages_lots"] + " </td>");
                //        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["net_quantity"] + "  </td>");
                //        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["TotalProce"] + " </td>");
                //        sb.Append("</tr>");
                //        totalPrice += (Convert.ToDecimal(dr["net_quantity"])) * Convert.ToDecimal(dr["Price"]);
                //        totalLots += Convert.ToInt32(dr["packages_lots"]);
                //        totalNetQty += Convert.ToDecimal(dr["net_quantity"]);
                //    }

                //    sb.Append("</table>");
                //    sb.Append("</td>");
                //    sb.Append("</tr>");
                //    sb.Append("</table>");
                //    sb.Append("<br/><table cellpadding='5' cellspacing='0' border='0' style='width:100%;margin-bottom: 10px;" + fontSize + "'>");
                //    sb.Append("<tr>");
                //    sb.Append("<td style='text-align:left;'>Date (दिनांक)  </td>");
                //    sb.Append("<td style='text-align:left;'><p style='margin-left:5px;'><b>" + DateTime.Now.ToString("dd/MM/yyyy") + "</b></p></td>");
                //    sb.Append("<td colspan='2' style='text-align:right;'><br> <b>Signature<br/> हस्ताक्षर </b> </td>");
                //    sb.Append("</tr>");
                //    sb.Append("</table>");

                //    sb.Append("</div>");
                //    sb.Append("</body></html>");





                //    if (!System.IO.Directory.Exists(filepath))
                //    {
                //        System.IO.Directory.CreateDirectory(filepath);
                //    }
                //    ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                //    DataTable DT = new DataTable();

                //    string FileName = "WRSummary_" + result;
                //    string Receipt_No = result;
                //    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                //    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "15px"));
                //    //System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                //    // sb1.Append(sb.ToString().Replace("<br>", "<br/>"));
                //    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName + ".pdf", ""))
                //    {
                //        result = COMMON_FUNCTION.VerifyAadhaarOTPForEsingh(OTP, TXN, filepath + "/" + FileName + ".pdf", true, FileName + ".pdf");
                //        if (result == "Success")
                //        {


                //            DetailsList.ForEach(x =>
                //            {
                //                x.Id = "1";
                //                x.EntryFile = FileName + ".pdf";
                //                //x.Receipt_No = Receipt_No;
                //            });
                //            objNew.InsertUpdateWRSummary(DetailsList, Receipt_No).ToString();


                //        }
                //    }
                //    else
                //    {
                //        result = "PDF Not Generate";
                //    }
                //}
            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.FertilizerSupply", FunctionName = "InsertWRPayment(List<WarehouseRSWCModel> DetailsList)" }, ex);
                result = "Error";
            }
            return result;
        }


        public ActionResult ReleaseOrder()
        {
            ViewBag.CurrentData = DateTime.Now.ToString("yyyy/MM/dd");
            return View();
        }

        //[HttpPost]
        public JsonResult GetAvailableCommodity(int Wharehouse_id, int CommodityId, int FinancialYear)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.GetAvailableCommodity(Wharehouse_id, CommodityId, FinancialYear);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDODetailsWR(int DelivberyNo)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.GetDODetailsWR(DelivberyNo);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeleteWR(string Receipt_No)
        {
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            return Json(new { Status = objNew.DeleteWR(Receipt_No) }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult CommodityReleased()
        {
            return View();
        }


        public ActionResult FertilizerExcelDataUpload()
        {
            return View();
        }


        [HttpPost]
        [ActionName("InsertfertilizerByExcel")]
        public string InsertSocityByExcel(SocietyRegistrationEntity _Model)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            string fileName = string.Empty;
            _Model.ExcelUrl = "";
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    _Model.ExcelUrl = fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("~/Content/SocietyExcel"), fileName));
                }
                catch { }
            }
            _Model.CreatedBy = _Model.UpdatedBy = int.Parse(_LoggedInUser.UserId.ToString());
            _Model.CreatedIP = COMMON_FUNCTION.GetIpAddress();
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            return objNew.InsertFertilizerByExcel(_Model).ToString();
            //return CommonMessageFactory.Message(int.Parse(objSocietyModel.InsertSocityByExcel(_Model).ToString()), (Request.Cookies[SessionIdentity.Culture]).Value);
        }



        #region Central Warehousing Corporation(CWC)

        public ActionResult GenerateCWC_WR()
        {
            string Todate = "";
            int Day = -1;
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());


            ViewBag.FromDate = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
            ViewBag.TodateDate = DateTime.Now.AddDays(-1).AddMonths(3).ToString("dd/MM/yyyy").Replace("-", "/");
            Todate = ViewBag.MinDate = "2019/11/01"; //DateTime.Now.AddDays(-1).ToString("yyy/MM/dd"); 
                                                     //Todate = ViewBag.MinDate = DateTime.Now.AddDays(-1).ToString("yyy/MM/dd"); 
                                                     //while (objNew.CheckIsItHoliday(Todate)>0)
                                                     //{
                                                     //    Day = Day - 1;
                                                     //    Todate = ViewBag.MinDate =DateTime.Now.AddDays(Day).ToString("yyy/MM/dd");

            //}

            ViewBag.MinDate = Todate;


            //int Result = objNew.CheckIsItHoliday(Todate);


            return View();

        }

        public string GenerateWarehouseCWCEng(string OTP, string TXN, List<WarehouseRSWCModel> DetailsList)
        {
            string result = "";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return "LOGOUT";
                }
                FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());




                if (!DetailsList[0].IsSubmit)
                {
                    if (DetailsList[0].Id == "0")
                    {
                        result = objNew.InsertUpdateRSWC(DetailsList).ToString();

                    }
                    else
                    {
                        result = objNew.UpdateRSWC(DetailsList).ToString();
                        //string requestNo = objNew.InsertKVSSRequest(ReqestData);
                    }
                    if (result == "-1" || result == "0")
                    {
                        return result;
                    }
                    else
                    {
                        result = "Success";
                    }
                    return result;
                }

                if (result == "-1" || result == "0")
                {
                    return result;
                }
                else
                {

                    // DataTable List = objNew.GetDirectorDemandDetails(result);

                    //////////var otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                    //////////if (otpResult.status != "1")
                    //////////{
                    //////////    return otpResult.data;
                    //////////}

                    object sumOrderQty;

                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<html><head><style>.lable{font-size:30px!important;}</style></head><body>");

                    sb.Append("<div style='margin-right:15px;    margin-left:15px; padding:5px;'>");
                    int index = 1;
                    string requestFiles = "";
                    string filepath = Server.MapPath("~/Content/RSWC");
                    string devFontSize = "";
                    string WRHTML = GenerateWRHTMLCWC(DetailsList);
                    sb.Append(WRHTML);
                    sb.Append("</div>");
                    sb.Append("</body></html>");



                    string fileId = Guid.NewGuid().ToString();

                    if (!System.IO.Directory.Exists(filepath))
                    {
                        System.IO.Directory.CreateDirectory(filepath);
                    }
                    ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                    DataTable DT = new DataTable();

                    string FileName = fileId;// result.Replace("/", "_");
                    string Receipt_No = result;
                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "15px"));
                    //System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    // sb1.Append(sb.ToString().Replace("<br>", "<br/>"));
                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName + ".pdf", ""))
                    {
                        //var esignResult = COMMON_FUNCTION.EsinghPDF(otpResult.data, filepath + "/" + FileName + ".pdf");
                        //result = esignResult.data;
                        //if (esignResult.status == "1")
                        //{

                        if (DetailsList[0].Id == "0")
                        {
                            result = objNew.InsertUpdateRSWC(DetailsList).ToString();
                        }
                        else
                        {
                            result = objNew.UpdateRSWC(DetailsList).ToString();
                            //string requestNo = objNew.InsertKVSSRequest(ReqestData);
                        }
                        Receipt_No = result;

                        if (System.IO.File.Exists(filepath + "/" + Receipt_No.Replace("/", "_") + ".pdf"))
                        {
                            System.IO.File.Delete(filepath + "/" + Receipt_No.Replace("/", "_") + ".pdf");
                        }

                        System.IO.File.Move(filepath + "/" + FileName + ".pdf", filepath + "/" + Receipt_No.Replace("/", "_") + ".pdf");
                        if (result == "-1" || result == "0")
                        {
                            return result;
                        }
                        else
                        {
                            result = "Success";
                        }
                        //return result;


                        FileName = Receipt_No.Replace("/", "_");



                        DetailsList.ForEach(x =>
                        {
                            x.Id = "1";
                            x.EntryFile = FileName + ".pdf";
                            x.Receipt_No = Receipt_No;
                        });
                        objNew.InsertUpdateRSWC(DetailsList).ToString();


                        // }
                    }
                    else
                    {
                        result = "PDF Not Generate";
                    }
                }
            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.FertilizerSupply", FunctionName = "GenerateWarehouseCWCEng(string OTP, string TXN, List<WarehouseRSWCModel> DetailsList)" }, ex);
                result = "Error";
            }
            return result;
        }

        public string GenerateWRHTMLCWC(List<WarehouseRSWCModel> DetailsList)
        {
            string result = "";
            try
            {
                if (DetailsList.Count > 0)
                {

                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    int index = 1;
                    string devFontSize = "";
                    string fontSize = "font-size: 13px;";
                    sb.Append("<table id='content' border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");
                    sb.Append("<tr>");
                    sb.Append("<th colspan='3' style=''><div style='text-align:left;width:80px;float:left;'><img style='width:80px;height:80px' src='" + Server.MapPath("~/Content/Images/cwc_logo.png") + "' /></div>");
                    sb.Append("<div style='text-align:center;float:left;'><label style='font-size: 22px;'>CENTRAL WAREHOUSING CORPORATION</label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 15px;'>(A GOVERNMENT OF INDIA UNDERTAKING)</label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 32px;'> " + UnicodeToKrutidev.GetKrutidev("केन्द्रीय भंडारण निगम") + " </label>");
                    //sb.Append("<br>");
                    //sb.Append("<label style='font-size: 30px;'> (सरकार का प्रतिष्ठान) </label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 18px;'>Non-Negotiable Receipt</label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 20px;'>" + UnicodeToKrutidev.GetKrutidev("अपरक्राम्य रसीद") + "</label>");
                    sb.Append("</div></th>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style='text-align:center;border-bottom: 2px solid black;'>");
                    sb.Append("  ");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style='text-align:left;'>");
                    sb.Append("<table id='content' border='0' style='width:100%;" + fontSize + "' cellpadding='3' cellspacing='0'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;width:35%;'>Serial No of Receipt (रसीद की क्रम संख्या) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> WHRNN/" + DetailsList[0].OldReceipt_No + " </b> </td>");//List.Rows[0]["receipt_serial_no"]
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Name of Warehouse (वेयरहाउस का नाम) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> CW- " + DetailsList[0].WarehousName + " </b> </td>");//List.Rows[0]["WarehousNameEn"]
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Address of Warehouse (वेयरहाउस का पता) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> " + DetailsList[0].Address + " </b> </td>");//List.Rows[0]["WarehousNameEn"]
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Warehouse license No. (वेयरहाउस का लाइसेंस नंबर) </td>");
                    sb.Append("<td style='text-align:left;'><b> " + DetailsList[0].Lisence_No + " </b> </td>");//List.Rows[0]["lisence_no"]
                    sb.Append("<td>Valid up to (तक मान्य) </td>");
                    sb.Append("<td>" + (string.IsNullOrWhiteSpace(DetailsList[0].LicenceValidUpTo) ? "N.A." : DetailsList[0].LicenceValidUpTo) + "</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Received From (Name and address of depositor)<br /> प्राप्त किया (जमा कर्ता का नाम और पता) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> NAFED Through KVSS " + DetailsList[0].KvssName + "   </b> </td>");//List.Rows[0]["DISTRICT_ENG"] + ", " + List.Rows[0]["SocietyName"] + " , " + List.Rows[0]["PurchaseCenterEng"]
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Commodity (जिंस) </td>");

                    string fyYear = "";

                    if (DateTime.Now.Month < 4)
                        fyYear = (DateTime.Now.Year - 1).ToString();
                    else
                        fyYear = DateTime.Now.Year.ToString();


                    sb.Append("<td  colspan='3' style='text-align:left;'><b> " + DetailsList[0].CommodityName + " </b> / " + DetailsList[0].Kharif + " " + DetailsList[0].Crop_year + "</td>");//List.Rows[0]["CommodityNameEn"]
                    //sb.Append("<td  style='text-align:left;'><b> " + DetailsList[0].CommodityName + " </b> / " + DetailsList[0].Kharif + " " + DetailsList[0].Valid_From_Date.Split('/')[2] + "</td>");//List.Rows[0]["CommodityNameEn"]
                    //sb.Append("<td colspan='2' style='text-align:left;'>Moisture (नमी) : <b> " + (string.IsNullOrWhiteSpace(DetailsList[0].Moisture) ? "0" : DetailsList[0].Moisture) + " %</b></td>");//List.Rows[0]["CommodityNameEn"]
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr><td style='" + devFontSize + "' colspan='3' >Goods of the following description (निम्नलिखित वर्णन का माल ) </td></tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style=''>");
                    sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + fontSize + "'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:center;font-size:10px;'>Sr.No</td>");
                    sb.Append("<td style='text-align:center;font-size:10px;'>Purchase Center</td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>Class or Standared Quality and/or Grade</td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>No. of Packages or Lots</td>");
                    sb.Append("<td style='text-align:center;font-size:10px;width:70px;'>Unit</td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>Net Quantity Measured in Qtls</td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>Name and/or Licence No. of the Weighter/Classifier/Sampler</td>");
                    sb.Append("<td style='text-align:center;font-size:10px'>GNO/Stack No.</td>");
                    sb.Append("</tr>");
                    decimal totalPrice = 0;
                    int totalLots = 0;
                    decimal totalNetQty = 0;
                    foreach (var dr in DetailsList)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;font-size:10px;'>" + (index++) + "</td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'>" + dr.PurchaseCenter + "</td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + (string.IsNullOrWhiteSpace(dr.Quality) ? "FAQ" : dr.Quality.ToUpper()) + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.Packages_Lots + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.Packages_Quantity_Unit + " KG </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.NetQuantity + "  </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr.SamplerName + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + (string.IsNullOrWhiteSpace(dr.GoDownName) ? dr.GoDownName : dr.GoDownName) + "/" + dr.Stack_No + " </td>");
                        sb.Append("</tr>");
                        totalPrice += (Convert.ToDecimal(dr.NetQuantity)) * Convert.ToDecimal(dr.Price);//+ ((Convert.ToDecimal(dr.NetQuantity)) * Convert.ToDecimal(dr.StorageRateCharge) ;
                        totalLots += Convert.ToInt32(dr.Packages_Lots);
                        totalNetQty += Convert.ToDecimal(dr.NetQuantity);
                    }

                    if (DetailsList.Count > 1)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td colspan='3'style='text-align:right'>Total (कुल) </td>");
                        sb.Append("<td style='text-align:center'>" + totalLots + "</td>");
                        sb.Append("<td></td>");
                        sb.Append("<td style='text-align:center'>" + totalNetQty + "</td>");
                        sb.Append("<td></td>");
                        sb.Append("<td></td>");
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("<table id='content' border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;width:60%;'>Condition of Goods -(1) Good (2) Fair (3) Average<br/> माल की स्थिति - (1) उत्तम (2) सुंदर (3) साधारण  </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> " + (DetailsList[0].ConditionOfGoods == "1" ? "Good" : DetailsList[0].ConditionOfGoods == "2" ? "Fair" : "Average") + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Private marks of the depositor on the packages if any<br/> यदि हो तो बंडलों  पर जमाकर्ता  के निजी चिन्ह </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> " + DetailsList[0].PrivateMarkofDepositor + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Rate of storage and other charges (भंडारण की दर और अन्य शुल्क) </td>");
                    sb.Append("<td colspan='3'  style='text-align:left;'><b> " + DetailsList[0].StorageRateCharge + " </b> </td>");
                    sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align:left;'>Indemnified / Insured for Fire /Theft/Riots / Other Contingency with<br/> आग / चोरी / दंगों / आदि अन्य  आपत्तियों  के लिये बीमा </td>");
                    //sb.Append("<td colspan='3' style='text-align:left;'><b> " + DetailsList[0].IndemnifiedInsurance + "  </b> </td>");
                    //sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>If Insured (Name Of Insurance Co.) (साथ यदि बीमा कराया  गया हो (बीमा समवाय का नाम)) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> " + DetailsList[0].InsuredName + " </b> </td>");
                    sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td style='text-align:left;'>Done for an amount of Rs. (रुपये की राशि के लिए किया गया) </td>");
                    //sb.Append("<td colspan='3' style='text-align:left;'><b> Rs. " + String.Format("{0:n}", totalPrice) + " </b> </td>");
                    //sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>The goods are accepted for storage from (भण्डारण  के लिए  माल स्वीकार किया  जाता  हैं)  </td>");
                    sb.Append("<td style='text-align:left;'><b> " + DetailsList[0].Valid_From_Date + " </b> </td>");
                    sb.Append("<td style='text-align:left;'>to (से)    </td>");
                    sb.Append("<td style='text-align:left;'><b>" + DetailsList[0].Valid_To_Date + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Market rate at the time of deposit (भण्डारण के समय मार्केट मूल्य भाव का भाव) </td>");
                    sb.Append("<td style='text-align:left;'  colspan='3'><b> Rs. " + String.Format("{0:n}", DetailsList[0].Price) + " </b> </td>");

                    sb.Append("</tr>");


                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Valuation<br> मूल्यांकन </td>");
                    sb.Append("<td style='text-align:left;'  colspan='3'><b>Rs. " + String.Format("{0:n}", totalPrice) + " </b></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Date of issue (जारी करने की तारिख) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b>" + DetailsList[0].DateOfIssue + "</b> </td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Remarks (टिप्पणी) </td>");
                    sb.Append("<td colspan='3' style='text-align:left;'><b> " + DetailsList[0].QuantityNote + " </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("<br/><table cellpadding='5' cellspacing='0' border='0' style='width:100%;margin-bottom: 10px;" + fontSize + "'>");


                    sb.Append("<tr>");
                    sb.Append("<td colspan='4' style='text-align:left;'>WHR NON Negotiable DATE : <b>" + DateTime.Now.ToString("dd/MM/yyyy") + "</b></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'> </td>");
                    sb.Append("<td colspan='3' style='text-align:right;'><br> <b>Signature of the warehouse Manager or authorised person<br/> वेयरहाउस प्रबंधक या अधिकृत व्यक्ति का हस्ताक्षर </b> </td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");

                    return sb.ToString();

                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return result;
        }

        [HttpPost]
        public JsonResult GetWarehouseCWCList(DataPagingModel Details, string RequestType)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int WarehouseId = _LoggedInUser.WarehouseId;
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.GetWarehouseCWCList(ref Details, WarehouseId, RequestType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult WRPhysical()
        {
            string Todate = "";
            int Day = -1;
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());


            ViewBag.FromDate = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
            ViewBag.TodateDate = DateTime.Now.AddDays(-1).AddMonths(3).ToString("dd/MM/yyyy").Replace("-", "/");
            Todate = ViewBag.MinDate = "2019/11/01"; //DateTime.Now.AddDays(-1).ToString("yyy/MM/dd"); 
                                                     //Todate = ViewBag.MinDate = DateTime.Now.AddDays(-1).ToString("yyy/MM/dd"); 
                                                     //while (objNew.CheckIsItHoliday(Todate)>0)
                                                     //{
                                                     //    Day = Day - 1;
                                                     //    Todate = ViewBag.MinDate =DateTime.Now.AddDays(Day).ToString("yyy/MM/dd");

            //}

            ViewBag.MinDate = Todate;


            //int Result = objNew.CheckIsItHoliday(Todate);


            return View();
        }


        [HttpPost]
        public JsonResult RajfedGetWRPhysicalEntry(DataPagingModel Details, string RequestType)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int WarehouseId = _LoggedInUser.WarehouseId;
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            DataTable DT = objNew.RajfedGetWRPhysicalEntry(ref Details, WarehouseId, RequestType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RajfedInsertUpdatedWRPhysicalEntry(List<WarehouseRSWCModel> _Model, string request_No = "")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int WarehouseId = _LoggedInUser.WarehouseId;
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            return Json(objNew.RajfedInsertUpdatedWRPhysicalEntry(_Model, request_No));
        }



        /// <summary>
        /// Added on New esign process 2.1
        /// </summary>
        /// <param name="DetailsList"></param>
        /// <returns></returns>
        public string GenerateWarehouseRSWCNew(List<WarehouseRSWCModel> DetailsList)
        {
            string result = "";
            string OrderNo = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return "LOGOUT";
                }
                FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
                if (!DetailsList[0].IsSubmit)
                {
                    if (DetailsList[0].Id == "0")
                    {
                        result = objNew.InsertUpdateRSWC(DetailsList).ToString();

                    }
                    else
                    {
                        result = objNew.UpdateRSWC(DetailsList).ToString();
                    }
                    if (result == "-1" || result == "0")
                    {
                        return result;
                    }
                    else
                    {
                        result = "Success";
                    }
                    return result;
                }

                if (result == "-1" || result == "0")
                {
                    return result;
                }
                else
                {
                    object sumOrderQty;

                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<html><head><style>.lable{font-size:30px!important;}</style></head><body>");

                    sb.Append("<div style='margin-right:15px;    margin-left:15px; padding:5px;'>");
                    int index = 1;
                    string requestFiles = "";
                    string filepath = Server.MapPath("~/Content/RSWC");
                    string devFontSize = "";
                    string WRHTML = GenerateWRHTML(DetailsList);
                    sb.Append(WRHTML);
                    sb.Append("</div>");
                    sb.Append("</body></html>");
                    string fileId = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace("_", "").ToUpper();
                    if (!Directory.Exists(filepath))
                    {
                        Directory.CreateDirectory(filepath);
                    }
                    ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                    DataTable DT = new DataTable();

                    string FileName = fileId;// result.Replace("/", "_");
                    string Receipt_No = result;
                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "15px"));
                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName + ".pdf", ""))
                    {
                        filepath = "~/Content/RSWC";
                        return JsonConvert.SerializeObject(new
                        {
                            Success = true,
                            DocPath = filepath,
                            DocType = "Ware House",
                            DocFullPath = filepath + "/" + FileName + ".pdf",
                            DocName = FileName + ".pdf",
                            ReturnPath = Url.Action("Warehouse", "FertilizerSupply")
                        });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new
                        {
                            Success = false,
                            Message = "PDF generation failed.",
                            DocPath = "",
                            DocType = "",
                            DocName = "",
                            ReturnPath = "",
                            DocFullPath = ""
                        });
                    }
                }
            }
            catch (System.Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.FertilizerSupply", FunctionName = "GenerateWarehouseRSWCEng(string TXN, List<WarehouseRSWCModel> DetailsList)" }, ex);
                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    Message = ex.Message,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    DocFullPath = ""
                });
            }
        }


        #region STEP 1 - START ESIGN
        [HttpPost]
        public async Task<JsonResult> StartEsign(string DocType, string docid, string SocietyIDEnc = "", string DocumentPath = "", string DocumentName = "", string ReturnPath = "", string Remark = "", int? StatusId = null, bool IsAmendment = false, string AmendmendAmount = "", string ReceiptNumber = "", List<WarehouseRSWCModel> Details = null, string Location = "", string Designation = "")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            EsignModel eSignModel = new EsignModel(new DBLogger());
            var detailsJson = JsonConvert.SerializeObject(Details);
            string UAT_BaseUrl = "http://localhost:27466/eSign/EspResponse";
            string Prod_BaseUrl = "https://rajsahakarapp.rajasthan.gov.in/eSign/EspResponse";

            try
            {
                if (_LoggedInUser != null)
                {
                    string responseUrl = ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")) ? UAT_BaseUrl : Prod_BaseUrl;
                    string pdfPath = ""; //Path.Combine(DocumentPath);
                    if (DocType == "Ware House")
                    {
                        pdfPath = Server.MapPath(DocumentPath) + "/" + DocumentName;
                    }
                    else { pdfPath = Server.MapPath(DocumentPath); }

                    string prn = GeneratePRN();
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


        [HttpPost]
        public string GenerateWRSummary(List<WarehouseRSWCModel> Details)
        {
            string result = "";
            string OrderNo = "";
            FertilizerSupplyFectory objNew = new FertilizerSupplyFectory(new DBLogger());
            string uniqeId = Guid.NewGuid().ToString("N").ToUpper();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                {
                    return "LOGOUT";
                }

                result = objNew.InsertUpdateWRSummary(Details, "", uniqeId).ToString();

                if (result == "-1" && result == "0")
                {
                    objNew.DeleteWRSummary(uniqeId);
                    return JsonConvert.SerializeObject(new
                    {
                        Success = false,
                        Message = "Unable to generate WR Summary.",
                        DocPath = "",
                        DocType = "",
                        DocName = "",
                        ReturnPath = "",
                        UniqeId = uniqeId,
                        Location = "",
                        Designation = "",
                    });
                }
                else
                {

                    string RO = "";
                    DataTable List = objNew.GetWRSummaryDetailsList(result);

                    object sumOrderQty;
                    OrganisationMasterModel obj = new OrganisationMasterModel(new DBLogger());

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<html><head><style>.lable{font-size:30px!important;}</style></head><body>");

                    sb.Append("<div style='margin-right:15px;    margin-left:15px; padding:5px;'>");
                    int index = 1;
                    string requestFiles = "";
                    string filepath = Server.MapPath("~/Content/RSWC");
                    string devFontSize = "";
                    string fontSize = "font-size: 15px;";
                    sb.Append("<table id='content' border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");
                    sb.Append("<tr>");

                    sb.Append("<th colspan='3' style=''><div style='text-align:left;width:80px;float:left;'><img style='width:80px;height:80px' src='" + Server.MapPath("~/Content/Images/rajfed_logo.jpg") + "' /></div>");

                    sb.Append("<div style='text-align:center;float:left;'><label style='font-size: 28px;'>" + UnicodeToKrutidev.GetKrutidev("राजस्थान राज्य सहकारी क्रय-विक्रय संघ लि.") + "  </label>");
                    sb.Append("<br>");
                    sb.Append("<label style='font-size: 15px;'>RAJASTHAN STATE CO-OPERATIVE MARKETING FEDERATION LTD.</label>");
                    //sb.Append("<br>");
                    //sb.Append("<label style='font-size: 32px;'> " + UnicodeToKrutidev.GetKrutidev("राजस्थान राज्य भण्डार व्यवस्था निगम") + " </label>");
                    //sb.Append("<br>");
                    //sb.Append("<label style='font-size: 30px;'> (सरकार का प्रतिष्ठान) </label>");
                    //sb.Append("<br>");
                    //sb.Append("<label style='font-size: 18px;'>Warehouse Receipt- Nagotiable/Non-negotiable</label>");
                    //sb.Append("<br>");
                    //sb.Append("<label style='font-size: 20px;'>" + UnicodeToKrutidev.GetKrutidev("भण्डार रसीद परक्राम्य") + "<label style='font-size: 18px;'>/</label>" + UnicodeToKrutidev.GetKrutidev("अपरक्राम्य") + "</label>");
                    sb.Append("</div></th>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style='text-align:center;border-bottom: 2px solid black;'>");
                    sb.Append("  ");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    string commodityName = "";
                    //string comodity=""
                    foreach (DataRow dr in List.Rows) // search whole table
                    {
                        commodityName += (commodityName == "" ? "" : ",") + dr["CommodityNameEn"];
                    }
                    commodityName = string.Join(",", commodityName.Split(',').Distinct().ToArray());

                    sb.Append("<tr><td style='text-align:left;" + devFontSize + "' colspan='3' > क्रमांक : " + commodityName + " / WR / " + List.Rows[0]["Kharif"] + " / " + List.Rows[0]["FY"] + "/ " + result + " </td></tr>");
                    //sb.Append("<tr><td style='text-align:left;" + devFontSize + "' colspan='3' > शाखा प्रबंधक <br /> नेफैड <br /> जयपुर । </td></tr>");
                    sb.Append("<tr><td style='text-align:left;" + devFontSize + "' colspan='3' > शाखा प्रबंधक <br /> " + Details[0].AgencyName + "<br /> जयपुर । </td></tr>");
                    sb.Append("<tr><td style='text-align:left;" + devFontSize + "' colspan='3' ><p style='margin-left:20px'> विषय :- समर्थन मूल्य पर " + commodityName + " " + List.Rows[0]["Kharif"] + " " + List.Rows[0]["FY"] + " की  WR भिजवाने  बाबत । </p></td></tr>");
                    sb.Append("<tr><td style='text-align:left;" + devFontSize + "' colspan='3' > महोदय, </td></tr>");
                    sb.Append("<tr><td style='text-align:left;" + devFontSize + "' colspan='3' ><p style='margin-left:10px'> विषयान्तर्गत निवेदन हैं कि  समर्थन मूल्य  पर " + commodityName + " " + List.Rows[0]["Kharif"] + " " + List.Rows[0]["FY"] + " की WR सलग्न कर निम्नानुसार भिजवाई जा रही  हैं :- </p></td></tr>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='3' style=''>");
                    sb.Append("<table cellpadding='5' cellspacing='0' border='1' style='width:100%;" + fontSize + "'>");
                    sb.Append("<tr>");
                    sb.Append("<th style='text-align:center;font-size:10px;'>Sr.No</th>");
                    sb.Append("<th style='text-align:left;font-size:10px;'>R.O.</th>");
                    sb.Append("<th style='text-align:left;font-size:10px;'>Warehous </th>");
                    sb.Append("<th style='text-align:left;font-size:10px;'>KVSS </th>");
                    sb.Append("<th style='text-align:left;font-size:10px;'>Purchase Cente </th>");
                    sb.Append("<th style='text-align:left;font-size:10px;'>WR No.</th>");
                    sb.Append("<th style='text-align:center;font-size:10px;'>Date</th>");
                    sb.Append("<th style='text-align:right;font-size:10px;'>No. of Lots </th>");
                    sb.Append("<th style='text-align:right;font-size:10px;'>Weight(Quintal)</th>");
                    //sb.Append("<td style='text-align:center;font-size:10px;'>Total Value</td>");
                    sb.Append("</tr>");
                    decimal totalPrice = 0;
                    int totalLots = 0;
                    decimal totalNetQty = 0;
                    decimal rowTotalNetQty = 0;
                    int rowTotalLots = 0;
                    string prevRO = "A";
                    string prevKVSS = "A";
                    string pCenter = "A";
                    foreach (DataRow dr in List.Rows) // search whole table
                    {

                        if (pCenter != dr["PurchaseCenterEng"].ToString() && pCenter != "A")
                        {
                            sb.Append("<tr>");
                            sb.Append("<th  colspan='7' style='text-align:right;font-size:10px;'> Total </th>");
                            sb.Append("<th style='text-align:right;font-size:10px;'> " + rowTotalLots + " </th>");
                            sb.Append("<th style='text-align:right;font-size:10px;'> " + rowTotalNetQty + "  </th>");
                            sb.Append("</tr>");
                            rowTotalNetQty = 0;
                            rowTotalLots = 0;
                        }
                        sb.Append("<tr>");
                        sb.Append("<td style='text-align:center;font-size:10px;'>" + (index) + "</td>");
                        sb.Append("<td style='text-align:left;font-size:10px;'> " + UnicodeToKrutidev.GetKrutidev(dr["RONameEn"].ToString()) + " </td>");
                        sb.Append("<td style='text-align:left;font-size:10px;'> " + UnicodeToKrutidev.GetKrutidev(dr["WarehousNameEn"].ToString()) + " </td>");
                        sb.Append("<td style='text-align:left;font-size:15px;'> " + UnicodeToKrutidev.GetKrutidev(dr["SocietyName"].ToString()) + "    </td>");
                        //sb.Append("<td style='text-align:left;font-size:10px;'> " + (prevRO == dr["RONameEn"].ToString() ? "" : UnicodeToKrutidev.GetKrutidev(dr["RONameEn"].ToString())) + " </td>");
                        //sb.Append("<td style='text-align:left;font-size:15px;'> " + (prevKVSS == dr["SocietyName"].ToString() ? "" : UnicodeToKrutidev.GetKrutidev(dr["SocietyName"].ToString())) + "    </td>");
                        sb.Append("<td style='text-align:left;font-size:10px;'> " + UnicodeToKrutidev.GetKrutidev(dr["PurchaseCenterEng"].ToString()) + " </td>");
                        sb.Append("<td style='text-align:left;font-size:10px;'> " + dr["receipt_serial_no"] + " </td>");
                        sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["WRGenerateDate"] + " </td>");
                        sb.Append("<td style='text-align:right;font-size:10px;'> " + dr["packages_lots"] + " </td>");
                        sb.Append("<td style='text-align:right;font-size:10px;'> " + dr["net_quantity"] + "  </td>");
                        //sb.Append("<td style='text-align:center;font-size:10px;'> " + dr["TotalProce"] + " </td>");
                        sb.Append("</tr>");
                        rowTotalNetQty += Convert.ToDecimal(dr["net_quantity"]);
                        rowTotalLots += Convert.ToInt32(dr["packages_lots"]);


                        if (index >= List.Rows.Count)
                        {
                            //if (prevKVSS != dr["SocietyName"].ToString() && prevKVSS != "A")
                            {
                                sb.Append("<tr>");
                                sb.Append("<th  colspan='7' style='text-align:right;font-size:10px;'> Total </th>");
                                sb.Append("<th style='text-align:right;font-size:10px;'> " + rowTotalLots + " </th>");
                                sb.Append("<th style='text-align:right;font-size:10px;'> " + rowTotalNetQty + "  </th>");
                                sb.Append("</tr>");
                                rowTotalNetQty = 0;
                                rowTotalLots = 0;
                            }
                        }


                        prevKVSS = dr["SocietyName"].ToString();
                        prevRO = dr["RONameEn"].ToString();
                        pCenter = dr["PurchaseCenterEng"].ToString();


                        totalPrice += (Convert.ToDecimal(dr["net_quantity"])) * Convert.ToDecimal(dr["Price"]);
                        totalLots += Convert.ToInt32(dr["packages_lots"]);
                        totalNetQty += Convert.ToDecimal(dr["net_quantity"]);
                        ++index;
                    }

                    if (List.Rows.Count > 1)
                    {
                        sb.Append("<tr>");
                        sb.Append("<th   colspan='7' style='text-align:right;font-size:10px;'>G. Total</th>");
                        sb.Append("<th  style='text-align:right;font-size:10px;'>" + totalLots + "</th>");
                        sb.Append("<th  style='text-align:right;font-size:10px;'>" + totalNetQty + "</th>");
                        sb.Append("</tr>");
                    }


                    if (List.Rows.Count > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<th   colspan='7' style='text-align:right;font-size:10px;'>Previous Total of WR's</th>");
                        sb.Append("<th  style='text-align:right;font-size:10px;'>" + List.Rows[0]["PrevTotalPackages_Lots"] + "</th>");
                        sb.Append("<th  style='text-align:right;font-size:10px;'>" + List.Rows[0]["PrevTotalNet_quantity"] + "</th>");
                        sb.Append("</tr>");
                    }


                    if (List.Rows.Count > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<th   colspan='7' style='text-align:right;font-size:10px;'>G. Total</th>");
                        sb.Append("<th  style='text-align:right;font-size:10px;'>" + (totalLots + Convert.ToDecimal(List.Rows[0]["PrevTotalPackages_Lots"])) + "</th>");
                        sb.Append("<th  style='text-align:right;font-size:10px;'>" + (totalNetQty + Convert.ToDecimal(List.Rows[0]["PrevTotalNet_quantity"])) + "</th>");
                        sb.Append("</tr>");
                    }



                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");

                    sb.Append("<br/><table cellpadding='5' cellspacing='0' border='0' style='width:100%;margin-bottom: 10px;" + fontSize + "'>");

                    sb.Append("<tr>");
                    sb.Append("<td colspan='3'  style='text-align:left;'>Enclosed:-" + List.Rows.Count + " WR </td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:left;'>Date (दिनांक) :- " + DateTime.Now.ToString("dd/MM/yyyy") + "  </td>");
                    //sb.Append("<td colspan='2' style='text-align:right;'><br> <b> भवदीय <br/><br/> सहायक रजिस्ट्रार  </b> </td>");
                    sb.Append("<td colspan='2' style='text-align:right;'><br> <b> भवदीय <br/><br/> अतिरिक्त रजिस्ट्रार  </b> </td>");

                    sb.Append("</tr>");
                    sb.Append("</table>");

                    sb.Append("</div>");
                    sb.Append("</body></html>");

                    if (!Directory.Exists(filepath))
                    {
                        Directory.CreateDirectory(filepath);
                    }
                    ElectionModel objSocietyModel = new ElectionModel(new DBLogger());
                    DataTable DT = new DataTable();

                    string FileName = "WRSummary_" + result;
                    string Receipt_No = result;
                    StringBuilder sb1 = new StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, "20px"));
                    if (COMMON_FUNCTION.GeneratePDF(sb1, filepath + "/" + FileName + ".pdf", ""))
                    {
                        filepath = "~/Content/RSWC";
                        return JsonConvert.SerializeObject(new
                        {
                            Success = true,
                            DocPath = filepath,
                            DocType = "WRSummary",
                            DocName = FileName + ".pdf",
                            ReturnPath = Url.Action("WRSummary", "FertilizerSupply"),
                            UniqeId = uniqeId,
                            Receipt_No = Receipt_No,
                            Location = "",
                            Designation = "",
                        });
                    }
                    else
                    {
                        objNew.DeleteWRSummary(uniqeId);
                        return JsonConvert.SerializeObject(new
                        {
                            Success = false,
                            Message = "PDF generation failed.",
                            DocPath = "",
                            DocType = "",
                            DocName = "",
                            ReturnPath = "",
                            UniqeId = "",
                            Location = "",
                            Designation = "",
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                objNew.DeleteWRSummary(uniqeId);
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.FertilizerSupply", FunctionName = "GenerateWRSummary(List<WarehouseRSWCModel> DetailsList)" }, ex);
                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    Message = ex.Message,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    UniqeId = "",
                    Location = "",
                    Designation = "",
                });
            }

        }
        public static string GeneratePRN()
        {
            return Guid.NewGuid().ToString("N").ToUpper();
        }
        #endregion
    }
}
