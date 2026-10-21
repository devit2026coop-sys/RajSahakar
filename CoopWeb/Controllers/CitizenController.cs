using Cooperative.Filters;
using Cooperative.Models;
using Ionic.Zip;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using CoopDAL;
using CoopDB;
using System.Security.Cryptography;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Cooperative.Controllers
{

    //[Error]
    
    [LogActionFilter]
    public class CitizenController : BaseController
    {
        static String username = System.Configuration.ConfigurationManager.AppSettings["SMSLogin"].ToString();
        static String password = System.Configuration.ConfigurationManager.AppSettings["SMSPassword"].ToString();
        static String senderid = System.Configuration.ConfigurationManager.AppSettings["SMSSenderId"].ToString();
        static String secureKey = System.Configuration.ConfigurationManager.AppSettings["SMSKEY"].ToString();

        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        [CustomAuthentication]
        public ActionResult CitizenDashboard()
        {
            //List<SocietyRegistrationEntity> _SocietyRegistrationData = new List<SocietyRegistrationEntity>();
            //CitizenModel objMenuActionModel = new CitizenModel(new FileLogger());
            //SocietyRegistration.UserId = _LoggedInUser.UserId;
            //SocietyRegistration.SSOID = _LoggedInUser.SSOID;
            //SocietyRegistration.UserType = _LoggedInUser.UserType;
            //DataTable DT = objMenuActionModel.SocietyList(SocietyRegistration);



            //foreach (DataRow Dr in DT.Rows)
            //{
            //    SocietyRegistrationEntity objEntity = new SocietyRegistrationEntity();
            //    objEntity.SocietyName = Dr["SocietyName"].ToString();
            //    objEntity.SocietyID = int.Parse(Dr["SocietyID"].ToString());
            //    objEntity.SocietyIDEnc = Dr["SocietyIDEnc"].ToString();
            //    objEntity.RegistrationNo = Dr["RegistrationNo"].ToString();
            //    objEntity.CreationDateStr = Dr["RegistrationDate"].ToString();
            //    objEntity.ActNameEn = Dr["ActNameEn"].ToString();
            //    objEntity.ActNameHi = Dr["ActNameHi"].ToString();
            //    objEntity.IsActive = int.Parse(Dr["IsActive"].ToString());
            //    objEntity.FilledProfileLevel = int.Parse(Dr["IsAdministrativeDetails"].ToString());
            //    objEntity.TypeNameEn = Dr["TypeameEng"].ToString();
            //    objEntity.TypeNameHi = Dr["TypeNameHin"].ToString();
            //    objEntity.Status = Dr["Status"].ToString();
            //    objEntity.ActId = int.Parse(Dr["ActId"].ToString());
            //    objEntity.ActIdEnc = Dr["ActId"].ToString();
            //    _SocietyRegistrationData.Add(objEntity);
            //}


            //return View(_SocietyRegistrationData);
            return View();
        }
        public ActionResult MilkProductionCooperativeSociety()
        {
            return View();
        }
        public ActionResult AadharTest()
        {
            return View();
        }
        public ActionResult Esign()
        {
            return View();
        }
        public ActionResult SocietyAmendment()
        {
            //return View("Inedx");
            return View();
        }

        public ActionResult AmendmentSectionFour()
        {
            //return View("Inedx");
            return View();
        }

        public ActionResult FeeVerification()
        {
            return View();
        }
        public ActionResult CaseStatus()
        {
            return View();
        }
        public ActionResult OtsPrLoanPayment()
        {
            return View();
        }
        public ActionResult Amendment2001()
        {
            return View();
        }

        public ActionResult SocietyAuditReportByCitizen()
        {
            List<SocietyRegistrationEntity> _SocietyRegistrationData = new List<SocietyRegistrationEntity>();
            //CitizenModel objMenuActionModel = new CitizenModel(new FileLogger());
            //SocietyRegistrationEntity objSocietyRegistrationEntity = new SocietyRegistrationEntity();
            //if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
            //{
            //    if (Request.Cookies["SSOROLE"].Value == "CITIZEN")
            //    {
            //        objSocietyRegistrationEntity.UserType = "C";
            //    }
            //    else
            //    {
            //        objSocietyRegistrationEntity.UserType = "D";
            //    }

            //    objSocietyRegistrationEntity.SSOID = Request.Cookies["SSO_ID"].Value;
            //}
            //DataTable DT = objMenuActionModel.SocietyList(objSocietyRegistrationEntity);

            //foreach (DataRow Dr in DT.Rows)
            //{
            //    SocietyRegistrationEntity objEntity = new SocietyRegistrationEntity();
            //    objEntity.SocietyID = int.Parse(Dr["SocietyID"].ToString());
            //    objEntity.SocietyIDEnc = Dr["SocietyIDEnc"].ToString();
            //    objEntity.SocietyName = Dr["SocietyName"].ToString();
            //    objEntity.RegistrationNo = Dr["RegistrationNo"].ToString();
            //    objEntity.CreationDateStr = Dr["RegistrationDate"].ToString();
            //    objEntity.ActNameEn = Dr["ActNameEn"].ToString();
            //    objEntity.ActNameHi = Dr["ActNameHi"].ToString();
            //    objEntity.IsActive = int.Parse(Dr["IsActive"].ToString());
            //    _SocietyRegistrationData.Add(objEntity);
            //}

            return View(_SocietyRegistrationData);
        }

        [HttpGet]
        public string AAdhar(string AAdharENC)
        {
            string CHANNEL = "BOTH";
            string APP_NAME = "RajSahakar";
            string UID = "550649276302";
            string SSOID = "VINAYYADAV";
            string EKYC = "false";
            string CHECKSUM = APP_NAME.ToUpper() + "|" + UID + "|" + SSOID.ToUpper();
            string CHECKSUMMD5 = "";
            string CHECKSUMBase64 = "";
            string IS_SKIPABLE = "false";
            string IS_EDITABLE = "false";
            string RETURN_URL = "http://localhost:27466/Citizen/AadharTest";
            string Staging = "https://ssotest.rajasthan.gov.in/ssoauth/fpauthnew/";
            string Production = "https://ssoauth.rajasthan.gov.in/fpauthnew";
            string EncodedRequestXml = "";

            CHECKSUMMD5 = SocietyController.CreateMD5(CHECKSUM);
            CHECKSUMBase64 = SocietyController.GetBase64Encode(CHECKSUMMD5);

            string XMLDATA = "<?xml version='1.0' encoding='UTF-8' standalone='yes'?> <SSOAuthRequest> <CHANNEL>" + CHANNEL + " </CHANNEL> <APP_NAME>" + APP_NAME + " </APP_NAME><UID>" + UID + " </UID><SSOID>" + SSOID + " </SSOID><EKYC>" + EKYC + " </EKYC><CHECKSUM>" + CHECKSUMBase64 + " </CHECKSUM><IS_SKIPABLE>" + IS_SKIPABLE + " </IS_SKIPABLE><IS_EDITABLE>" + IS_EDITABLE + " </IS_EDITABLE><RETURN_URL>" + RETURN_URL + "</RETURN_URL></authrequest>";
            XMLDATA.Replace('\'', '\"');
            EncodedRequestXml = SocietyController.GetBase64Encode(XMLDATA.Replace('\'', '\"'));




            return Staging + EncodedRequestXml;

        }

        [HttpPost]
        public JsonResult CitizenDashboardData(SocietyRegistrationEntity objSocietyRegistrationEntity)
        {

            CitizenModel objMenuActionModel = new CitizenModel(new FileLogger());
            List<SocietyListBySSOID_Result> list = new List<SocietyListBySSOID_Result>();
            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"].Value == "KIOSK" && Request.Cookies["SSO_ID"] != null && System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc] != null)
            {
                objSocietyRegistrationEntity.UserType = "K";
                // objSocietyRegistrationEntity.SSOID = Request.Cookies["SSO_ID"].Value;
                objSocietyRegistrationEntity.SocietyIDEnc = System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc].ToString().Split('~')[0];
                list = objMenuActionModel.SocietyList(objSocietyRegistrationEntity, "", objSocietyRegistrationEntity.UserType);
            }
            else if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
            {
                objSocietyRegistrationEntity.UserType = Request.Cookies["SSOROLE"].Value == "KIOSK" ? "K" : (Request.Cookies["SSOROLE"].Value == "CITIZEN" ? "C" : "D");
                objSocietyRegistrationEntity.SSOID = Request.Cookies["SSO_ID"].Value;
                if (objSocietyRegistrationEntity.UserType != "K")
                    list = objMenuActionModel.SocietyList(objSocietyRegistrationEntity, "", objSocietyRegistrationEntity.UserType);

            }

            return Json(new { DataList = list }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string CitizenDashboardDataWithDepartmentUser(SocietyRegistrationEntity objSocietyRegistrationEntity)
        {

            CitizenModel objMenuActionModel = new CitizenModel(new FileLogger());
            DataTable DT = null;
            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"].Value == "KIOSK" && Request.Cookies["SSO_ID"] != null && System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc] != null)
            {
                objSocietyRegistrationEntity.UserType = "K";
                // objSocietyRegistrationEntity.SSOID = Request.Cookies["SSO_ID"].Value;
                objSocietyRegistrationEntity.SocietyIDEnc = System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc].ToString().Split('~')[0];
                DT = objMenuActionModel.SocietyListWithDepartmentUser(objSocietyRegistrationEntity);
                if (DT != null && DT.Rows.Count > 0)
                {
                    System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc] = null;
                }
            }
            else if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
            {
                if (_LoggedInUser.UserType == "D")
                {
                    objSocietyRegistrationEntity.UserType = _LoggedInUser.UserType;
                    objSocietyRegistrationEntity.SSOID = _LoggedInUser.SSOID;
                    objSocietyRegistrationEntity.DistrictId = _LoggedInUser.DistrictId;
                    objSocietyRegistrationEntity.UserRoleDeptMappingId = _LoggedInUser.UserRoleDeptMappingId;

                    DT = objMenuActionModel.SocietyListWithDepartmentUser(objSocietyRegistrationEntity);
                }
                else
                {
                    objSocietyRegistrationEntity.UserType = _LoggedInUser.UserType;
                    objSocietyRegistrationEntity.OrganizationId = _LoggedInUser.UserRoleDeptMappingId;
                    objSocietyRegistrationEntity.SSOID = Request.Cookies["SSO_ID"].Value;
                    if (objSocietyRegistrationEntity.UserType != "K")
                        DT = objMenuActionModel.SocietyListWithDepartmentUser(objSocietyRegistrationEntity);
                }
                // }
            }

            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        }

        [HttpPost]
        public string BankstamentData(string SocietyIDEnc)
        {

            CitizenModel objMenuActionModel = new CitizenModel(new FileLogger());
            DataTable DT = objMenuActionModel.BankstamentList(SocietyIDEnc);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }

        [HttpPost]
        public string CitizenDashboardDataForAuditRPT(SocietyRegistrationEntity objSocietyRegistrationEntity)
        {
            CitizenModel objMenuActionModel = new CitizenModel(new FileLogger());
            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
            {
                objSocietyRegistrationEntity.UserType = (Request.Cookies["SSOROLE"].Value == "CITIZEN" ? "C" : "D");
                objSocietyRegistrationEntity.SSOID = Request.Cookies["SSO_ID"].Value;
            }
            DataTable DT = objMenuActionModel.SocietyListForAuditRPT(objSocietyRegistrationEntity);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }

        public ActionResult SocietyProfile(string IsNewReg = "0", bool IsUpdate = false)

        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ViewBag.usertype = _LoggedInUser.UserType;
            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null && Request.Cookies["SSOROLE"].Value == "KIOSK")
            {
                if (IsUpdate)
                {
                    ViewBag.IsUpdateYes = 1;
                }
                else
                {
                    ViewBag.IsUpdateYes = 0;
                    if (IsNewReg == "1")
                    {
                        ViewBag.Serviceid = "4172";//NEW REG
                    }
                    else
                    {
                        ViewBag.Serviceid = "4209";//PROFILE
                    }


                    ////ViewBag.Serviceid = Request.Cookies["EmitraServiceid"].Value;

                    //if (IsNewReg == "1")
                    //{
                    //    ViewBag.Serviceid = "4172";
                    //}
                    //else
                    //{
                    //    ViewBag.Serviceid = "4209";
                    //}
                }

                // ViewBag.Serviceid = Request.Cookies["EmitraServiceid"].Value;

            }


            return View();
        }


        [HttpPost]
        [ActionName("SaveSocietyProfiling")]
        public string SaveSocietyProfiling(SocietyRegistrationEntity _Entity)
        {
            string flag = "0";
            string fileName = string.Empty;
            string pathString = "~/Content/SocietyProfiling/";

            pathString += Regex.Replace(_Entity.RegistrationNo, "[^a-zA-Z0-9_]+", "");
            if (!System.IO.File.Exists(pathString))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath(pathString));
            }
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());

            foreach (string files in Request.Files)
            {

                HttpPostedFileBase file = Request.Files[files];
                fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));
                if (files.ToString() == "RegistrationCertificateFile")
                    _Entity.RegistrationCertificateFile = fileName;
                else if (files.ToString() == "BylawsFile")
                    _Entity.ByeLawsSocietyFile = fileName;
                else if (files.ToString() == "RulesAndRegulation")
                    _Entity.RulesAndRegulationFile = fileName;
            }

            if (Request.Files != null && Request.Files.Count > 0)
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                _Entity.SSOID = _LoggedInUser.SSOID;
                _Entity.UserType = _LoggedInUser.UserType;
                _Entity.CreatedBy = _Entity.UpdatedBy = _LoggedInUser.UserId;
                _Entity.KIOSKCODE = _LoggedInUser.KIOSKCODE;
                _Entity.CreatedIP = _Entity.UpdatedIP = COMMON_FUNCTION.GetIpAddress();

                //flag = objSocietyModel.InsertSocietyProfile(_Entity);
                flag = "";
            }

            if (flag == "-1" || flag == "0")
            {
                //System.IO.Directory.Delete(Server.MapPath(pathString));
            }

            return flag;

        }

        [HttpPost]
        public string GetSSOMapRequestList(string SearchText)
        {
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objCitizenModel.GetSSOMapRequestList(SearchText));
        }

        [HttpPost]
        public JsonResult GetSocietyList(SocietyRegistrationEntity objSocietyRegistrationEntity)
        {
            //CitizenModel objCitizenModel = new CitizenModel(new DBLogger());

            if (_LoggedInUser.UserType == "C")
            {
                objSocietyRegistrationEntity.SSOID = _LoggedInUser.SSOID;
                objSocietyRegistrationEntity.UserType = _LoggedInUser.UserType;
            }
            CitizenModel objMenuActionModel = new CitizenModel(new FileLogger());
            List<SocietyListBySSOID_Result> list = objMenuActionModel.SocietyList(objSocietyRegistrationEntity, "", _LoggedInUser.UserType);

            return Json(new { DataList = list }, JsonRequestBehavior.AllowGet);
            //COMMON_FUNCTION.DataTableToJSONString(objCitizenModel.SocietyList(objSocietyRegistrationEntity));
        }

        [HttpPost]
        public JsonResult GetSocietyForFees(string SearchText)
        {
            //CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            SocietyRegistrationEntity objSocietyRegistrationEntity = new SocietyRegistrationEntity();

            objSocietyRegistrationEntity.SSOID = _LoggedInUser.SSOID;
            objSocietyRegistrationEntity.UserType = _LoggedInUser.UserType;

            CitizenModel objMenuActionModel = new CitizenModel(new FileLogger());
            List<GetSocietyForFees_Result> list = (new Society()).GetSocietyForFees(_LoggedInUser.SSOID, SearchText, _LoggedInUser.UserType);

            return Json(new { DataList = list }, JsonRequestBehavior.AllowGet);
            //COMMON_FUNCTION.DataTableToJSONString(objCitizenModel.SocietyList(objSocietyRegistrationEntity));
        }



        [HttpPost]
        public string GetSocietyListNotAssignedToInspector(SocietyRegistrationEntity objSocietyRegistrationEntity)
        {
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());

            if (_LoggedInUser.UserType == "C")
            {
                objSocietyRegistrationEntity.SSOID = _LoggedInUser.SSOID;
                objSocietyRegistrationEntity.UserType = _LoggedInUser.UserType;
            }
            else
            {
                objSocietyRegistrationEntity.UserRoleDeptMappingId = _LoggedInUser.UserRoleDeptMappingId;
            }
            return COMMON_FUNCTION.DataTableToJSONString(objCitizenModel.SocietyListNotAssignedToInspector(objSocietyRegistrationEntity));
        }
        [HttpPost]
        public string GetSocietyListNotAssignedToInspectorForCitizen(SocietyRegistrationEntity objSocietyRegistrationEntity)
        {
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());


            objSocietyRegistrationEntity.SSOID = _LoggedInUser.SSOID;
            objSocietyRegistrationEntity.UserType = _LoggedInUser.UserType;

            return COMMON_FUNCTION.DataTableToJSONString(objCitizenModel.SocietyListNotAssignedToInspectorForCitizen(objSocietyRegistrationEntity));
        }

        [HttpGet]
        public ActionResult UserSocietyMappingValid()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.UserSocietyMappingValid, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Index()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);
            ViewBag.currentSSOID = _LoggedInUser.SSOID;
            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Index, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpGet]
        public ActionResult ImpoDateAction()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            //bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            //if (Result == false)
            //{
            //    return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            //}
            //else
            //{
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.ImpoDateAction, (Request.Cookies[SessionIdentity.Culture]).Value);
            //}
            return View();
        }

        [HttpGet]
        [ActionName("GetImpoDateActionList")]
        public string GetImpoDateActionList(string SocietyIdENC)
        {
            DataTable dt = null;
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            dt = objCitizenModel.GetImpoDateActionList(SocietyIdENC);


            return COMMON_FUNCTION.DataTableToJSONString(dt); ;
        }
        [HttpPost]
        public string GetAllSocietyList()
        {
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objCitizenModel.GetAllSocietyList(_LoggedInUser.SSOID));
        }
        public ActionResult VeryFiedUser()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.VeryFiedUser, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }


        [HttpGet]
        [ActionName("GetSocityDataForOtp")]
        public string GetSocityDataForOtpSending(string SocietyRegistrationNo)
        {
            DataTable dt = null;
            string WhereClause = "";
            if (_LoggedInUser.UserType == "K")
                WhereClause = " and Societies.RegistrationNo like '%" + SocietyRegistrationNo + "%'";
            else
                WhereClause = " and Societies.RegistrationNo='" + SocietyRegistrationNo + "'";

            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            dt = objCitizenModel.GetSocityDataForOtpSending(WhereClause);
            var userAADHARId = AADHARSerice.GetAADHARNoBySSOId(_LoggedInUser.SSOID);

            string _loggedInUserSSOId = _LoggedInUser.SSOID;

            if (dt.Rows.Count > 0)
            {
                //dt.Rows[0]["AadharNo"] = "307691961366";
                //t.Rows[0]["AadharNo"] = "428434778643";
                foreach (DataRow dr in dt.Rows) // search whole table
                {
                    if (!string.IsNullOrEmpty(dr["AadharNo"].ToString()))
                    {
                        //dr["AadharNo"] = COMMON_FUNCTION.Decrypt(dr["AadharNo"].ToString());

                        if (dr["AadharNo"].ToString().Length > 12)
                        {
                            dr["AadharNo"] = COMMON_FUNCTION.Decrypt(dr["AadharNo"].ToString());
                        }


                        var AadharNo = dr["AadharNo"].ToString();

                        var firstDigits = AadharNo.Substring(0, 8);
                        var lastDigits = AadharNo.Substring(AadharNo.Length - 4, 4);
                        var requiredMask = new String('X', AadharNo.Length - lastDigits.Length);
                        var maskedString = string.Concat(requiredMask, lastDigits);
                        var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0");
                        dr["IsAuthorizedUser"] = dr["AadharNo"].ToString().Trim() == userAADHARId.Trim() ? true : true;
                        dr["AadharNo"] = maskedString;
                        dr["AdhaNoPure"] = AadharNo;
                    }
                    else
                    {

                        var AadharNo = userAADHARId.ToString();

                        var firstDigits = AadharNo.Substring(0, 8);
                        var lastDigits = AadharNo.Substring(AadharNo.Length - 4, 4);
                        var requiredMask = new String('X', AadharNo.Length - lastDigits.Length);
                        var maskedString = string.Concat(requiredMask, lastDigits);
                        var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0");
                        dr["IsAuthorizedUser"] = true;
                        dr["AadharNo"] = maskedString;
                        dr["AdhaNoPure"] = AadharNo;
                    }
                }
            }

            return COMMON_FUNCTION.DataTableToJSONString(dt); ;
        }

        [HttpPost]
        [ActionName("GetSocityDataForOtp2")]
        public JsonResult GetSocityDataForOtpSending2(string SocietyRegistrationNo, int DistrictId = 0, int ActId = 0)
        {
            string _UserType = _LoggedInUser.UserType.ToUpper();
            string _loggedInUserSSOId = _LoggedInUser.SSOID;
            string _loggedInUserAADHARNo = _LoggedInUser.AadharNo;// AADHARSerice.GetAADHARNoBySSOId(_LoggedInUser.SSOID);

            //DataTable dt = new DataTable();
            string WhereClause = "";
            if (_UserType == "K")
            {
                WhereClause = " and Societies.RegistrationNo like '%" + SocietyRegistrationNo + "%' or Societies.BRN like '%" + SocietyRegistrationNo + "%'"; DistrictId = 0;
            }
            else
            {
                WhereClause = " and ( Societies.RegistrationNo like '%" + SocietyRegistrationNo + "%'  or Societies.BRN like '%" + SocietyRegistrationNo + "%') and SocietyRegOfficeDetails.DISTRICT_ID=" + DistrictId;
            }



            //Citizen objCitizenModel = new Citizen();
            // var dt = objCitizenModel.GetSocityDataForOtpSending2(SocietyRegistrationNo, DistrictId, ActId);


            CitizenModel objCitizenModel1 = new CitizenModel(new DBLogger());
            DataTable dt1 = objCitizenModel1.GetSocityDataForOtpSending2(SocietyRegistrationNo, DistrictId, ActId);


            if (dt1 == null)
                return Json(new { Result = false, Message = "No data found", data = new List<object>() });

            var dt = dt1.AsEnumerable().Select(row => new GetDetailsForMapSociety_ResultData
            {
                SocietyID = row.Field<int>("SocietyID"),
                DISTRICT_ENG = row.Field<string>("DISTRICT_ENG"),
                IsAuthorizedUser = row.Field<int>("IsAuthorizedUser"),
                ActId = row.Field<int?>("ActId"),
                AdhaNoPure = row.Field<string>("AdhaNoPure"),
                ContactNo = row.Field<string>("ContactNo"),
                OTPDateTime = row.Field<DateTime?>("OTPDateTime"),
                SSOID = row.Field<string>("SSOID"),
                MaskingMobileNO = row.Field<string>("MaskingMobileNO"),
                AadharNo = row.Field<string>("AadharNo"),
                ActNameEn = row.Field<string>("ActNameEn"),
                OTP = row.Field<string>("OTP"),
                SocietyIDEnc = row.Field<string>("SocietyIDEnc"),
                RegistrationNo = row.Field<string>("RegistrationNo"),
                Name = row.Field<string>("Name"),
                TypeameEng = row.Field<string>("TypeameEng"),
                SocietyName = row.Field<string>("SocietyName"),
                txnid = row.Field<string>("txnid"),
                SocietyDirectory = row.Field<string>("SocietyDirectory"),
                TotalMember = row.Field<int>("TotalMember"),
                IsAmendment = row.Field<bool?>("IsAmendment"),
                IsAmendmentEdit = row.Field<bool?>("IsAmendmentEdit"),
                ApprovalStatusId = row.Field<int?>("ApprovalStatusId"),
                ProceesStatusTextEn = row.Field<string>("ProceesStatusTextEn"),
                MaxAppliFlowStatusID = row.Field<int?>("MaxAppliFlowStatusID"),
                IsObjection = row.Field<bool>("IsObjection")
            }).ToList();


            List<object> collection = new List<object>();

            // foreach (GetDetailsForMapSociety_Result dr in dt.Rows)
            foreach (GetDetailsForMapSociety_ResultData dr in dt)
            {
                string _AADHARNo = string.Empty, _maskedAADHARNo = string.Empty, _NativeAADHARNo = string.Empty, _maskedNativeAADHARNo = string.Empty;

                if (!string.IsNullOrEmpty(dr.SSOID))
                {
                    try
                    {
                        var loggedInUserAADHARNo = AADHARSerice.GetAADHARNoBySSOId(dr.SSOID);
                        _AADHARNo = loggedInUserAADHARNo;// COMMON_FUNCTION.Decrypt(dr["AadharNo"].ToString());
                        _maskedAADHARNo = "XXXXXXXX" + _AADHARNo.Substring(8, 4);
                    }
                    catch
                    {
                        _AADHARNo = _loggedInUserAADHARNo;
                        _maskedAADHARNo = "XXXXXXXX" + _AADHARNo.Substring(8, 4);
                    }
                }
                else
                {

                    try
                    {
                        _AADHARNo = _loggedInUserAADHARNo;
                        _maskedAADHARNo = "XXXXXXXX" + _loggedInUserAADHARNo.Substring(8, 4);// string.Concat("".PadLeft(8, 'X'), _NativeAADHARNo.Substring(_NativeAADHARNo.Length - 4));
                    }
                    catch
                    {
                    }
                }

                collection.Add(new
                {
                    AadharNo = _maskedAADHARNo,
                    AdhaNoPure = _AADHARNo,
                    IsSsoFound = !string.IsNullOrEmpty(dr.SSOID) ? true : false,
                    IsSameSSO = !string.IsNullOrEmpty(dr.SSOID) ? (string.Equals(dr.SSOID.ToLower(), _loggedInUserSSOId.ToLower()) ? true : false) : false,
                    LoggedInUserAADHAR = _NativeAADHARNo,
                    LoggedInUserMaskedAADHAR = _maskedNativeAADHARNo,
                    ActId = dr.ActId,
                    SocietyIDEnc = dr.SocietyIDEnc,
                    AADHARNoEnc = COMMON_FUNCTION.Encrypt(_NativeAADHARNo),
                    SocietyName = dr.SocietyName,
                    ActNameEn = dr.ActNameEn,
                    RegistrationNo = dr.RegistrationNo,
                    TypeameEng = dr.TypeameEng,
                    Name = dr.Name,
                    SocietyID = dr.SocietyID,
                    ApprovalStatusId = dr.ApprovalStatusId,
                    IsAmendment = dr.IsAmendment,
                    SocietyDirectory = dr.SocietyDirectory,
                    IsAmendmentEdit = dr.IsAmendmentEdit,
                    MaxAppliFlowStatusID = dr.MaxAppliFlowStatusID,
                    ProceesStatusTextEn = dr.ProceesStatusTextEn,
                    IsObjection = dr.IsObjection
                });
            }



            return Json(new { Result = true, Message = "", data = collection });
        }

        [HttpGet]
        [CustomAuthentication]
        [ActionName("GenrateOTPdata")]
        public string ChekvalidOTP(string SocietyRegistrationNo, string _AadharNo = "")
        {
            _AadharNo = csCommon.Decrypt_SA(Uri.UnescapeDataString(_AadharNo));


            if (Request.Url.Host.Contains("localhost"))
            {
                _AadharNo = "940396382715";
            }

            DataTable dt = null;
            if (SocietyRegistrationNo != null)
            {
                string WhereClause = " and Societies.RegistrationNo='" + SocietyRegistrationNo + "'";
                CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
                dt = objCitizenModel.GetSocityDataForOtpSending(WhereClause);
            }
            else if (_AadharNo.Length > 0)
            {
                dt = new DataTable();
                dt.Columns.Add("AadharNo", typeof(string));
                dt.Columns.Add("txnid", typeof(string));
                DataRow dtrow = dt.NewRow();
                dtrow["AadharNo"] = _AadharNo;
                dtrow["txnid"] = "";
                dt.Rows.Add(dtrow);
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                _AadharNo = dt.Rows[0]["AadharNo"].ToString();
                if (!string.IsNullOrEmpty(_AadharNo) && _AadharNo.Length > 12)
                    _AadharNo = COMMON_FUNCTION.Decrypt(dt.Rows[0]["AadharNo"].ToString());
                //Aadhar = "940396382715";
                string _txnid = string.Empty;
                _txnid = COMMON_FUNCTION.SendOtpByAadharNo(_AadharNo);
                foreach (DataRow dr in dt.Rows) // search whole table
                {
                    if (_AadharNo.Length == 12)
                    {
                        //dr["AadharNo"] = COMMON_FUNCTION.Decrypt(dr["AadharNo"].ToString());                      
                        //string AadharNo = dr["AadharNo"].ToString();
                        var firstDigits = _AadharNo.Substring(0, 8);
                        var lastDigits = _AadharNo.Substring(_AadharNo.Length - 4, 4);
                        var requiredMask = new String('X', _AadharNo.Length - lastDigits.Length);
                        var maskedString = string.Concat(requiredMask, lastDigits);
                        var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0");
                        dr["AadharNo"] = maskedString;
                        dr["txnid"] = _txnid;
                    }
                }
            }
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpGet]
        [ActionName("ValidateOTP")]
        public string ValidateOTP(string SocietyRegistrationNo, string otp, string txn, string _AadharNo = "", string _SocietyDirectory = "")
        {
            // _AadharNo = "432534469557";
            _AadharNo = csCommon.Decrypt_SA(Uri.UnescapeDataString(_AadharNo));
            if (Request.Url.Host.Contains("localhost"))
            {
                _AadharNo = "432534469557";
                return "[{\"Column1\":\"Success\",\"Column2\":\"Balwant Chaudhary\",\"Column3\":\"M\",\"Column4\":\"20-09-1984\",\"Column5\":\"../Content/NewRegistration/member_90b405c50c384ae5b3eb3ed25534594d.jpg\",\"Column6\":\"बलवंत चौधरी\",\"Column7\":\"T1+Bhe8Ju9s4QjBaM66sAg==\",\"Column8\":\"S/O: Mohan Lal Chaudhary\",\"Column9\":\"S/O: मोहन लाल चौधरी\",\"Column10\":\"House : , LandMark : Village raipur, Street : , District: Pratapgarh, PIN: 312619, State: Rajasthan\",\"Column11\":\"House : #LandMark : Village raipur#Street : #District: Pratapgarh#PIN: 312619#State: Rajasthan\"}]";
            }

            DataTable dt = null;
            if (!string.IsNullOrWhiteSpace(SocietyRegistrationNo))
            {
                string WhereClause = " and Societies.RegistrationNo='" + SocietyRegistrationNo + "'";
                CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
                dt = objCitizenModel.GetSocityDataForOtpSending(WhereClause);
            }
            else
            {
                dt = new DataTable();
                dt.Columns.Add("AadharNo", typeof(string));
                dt.Columns.Add("txnid", typeof(string));
                DataRow dtrow = dt.NewRow();
                dtrow["AadharNo"] = _AadharNo;
                dtrow["txnid"] = "";
            }
            DataTable dtb = new DataTable();
            if (!string.IsNullOrWhiteSpace(SocietyRegistrationNo) && otp != null && txn != null && dt.Rows.Count > 0)
            {
                //dt.Rows[0]["AadharNo"] = COMMON_FUNCTION.Decrypt(dt.Rows[0]["AadharNo"].ToString());
                _AadharNo = COMMON_FUNCTION.Decrypt(dt.Rows[0]["AadharNo"].ToString());
                //_AadharNo = "940396382715";//COMMON_FUNCTION.Decrypt(dt.Rows[0]["AadharNo"].ToString());
                dtb = COMMON_FUNCTION.ValidateOTP(txn, _AadharNo, otp);
                if (dtb.Rows.Count > 0)
                {
                    if (dtb.Rows[0]["Column1"].ToString() == "Success")
                    {
                        CitizenModel ObjCitizen = new CitizenModel(new DBLogger());
                        //if (_LoggedInUser.UserType.ToUpper() != "K")
                        //    ObjCitizen.UpdateUserSSOIDByRegNo(_LoggedInUser.SSOID, SocietyRegistrationNo);
                        string fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper();
                        byte[] imageBytes = Convert.FromBase64String(dtb.Rows[0]["Column5"].ToString());
                        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

                        fileName = COMMON_FUNCTION.GetUniqueFileName("Member_" + fileName, System.Web.HttpContext.Current.Server.MapPath("../Content/NewRegistration/"), ".jpg").ToLower();

                        FileStream fs = new FileStream(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/NewRegistration/"), fileName + ".jpg"), FileMode.CreateNew, FileAccess.Write);
                        ms.WriteTo(fs);
                        fs.Close();
                        fs.Dispose();
                        ms.Flush();
                        dtb.Rows[0]["Column5"] = "../Content/NewRegistration/" + fileName + ".jpg";
                    }
                }
            }
            else if (_AadharNo.Length > 0)
            {
                dtb = COMMON_FUNCTION.ValidateOTP(txn, _AadharNo, otp);
                if (dtb.Rows.Count > 0)
                {
                    if (dtb.Rows[0]["Column1"].ToString() == "Success")
                    {
                        _SocietyDirectory = _SocietyDirectory == "undefined" ? "" : _SocietyDirectory;

                        string path = _SocietyDirectory.Length > 1 ? "../Content/SocietyProfiling/" + _SocietyDirectory : "../Content/NewRegistration/";

                        string fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper();
                        byte[] imageBytes = Convert.FromBase64String(dtb.Rows[0]["Column5"].ToString());
                        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

                        fileName = COMMON_FUNCTION.GetUniqueFileName("Member_" + fileName, System.Web.HttpContext.Current.Server.MapPath(path), ".jpg").ToLower();

                        if (!System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(path.Replace("..", "~"))))
                        {
                            System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(path.Replace("..", "~")));
                        }


                        if (!System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(path.Replace("..", "~"))))
                        {
                            System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(path.Replace("..", "~")));
                        }

                        FileStream fs = new FileStream(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(path.Replace("..", "~")), fileName + ".jpg"), FileMode.CreateNew, FileAccess.Write);

                        ms.WriteTo(fs);
                        fs.Close();
                        fs.Dispose();
                        ms.Flush();
                        dtb.Rows[0]["Column5"] = _SocietyDirectory.Length > 1 ? "../Content/SocietyProfiling/" + _SocietyDirectory + "/" + fileName + ".jpg" : "../Content/NewRegistration/" + fileName + ".jpg";
                    }
                }
            }
            else
            {
                List<string[]> listx = new List<string[]>();
                listx.Add(new string[] { "Failed", "Invalid Society Data", COMMON_FUNCTION.Encrypt(_AadharNo) });
                dtb = ConvertListToDataTable(listx);
            }
            return COMMON_FUNCTION.DataTableToJSONString(dtb);
        }





        [HttpPost]
        public JsonResult ValidateBIOKYC(BIOKYCRequest Details)
        {

            return Json(new { Status = "5", Message = "Invalid request" }, JsonRequestBehavior.AllowGet); //Added by Pradeep 13-06-2024
            DBLogger objDBLogger = new DBLogger();
            CitizenModel ObjCitizen = new CitizenModel(new DBLogger());
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser != null)
                {
                    //KYCResponce result = AADHARSerice.BIO_KYC(AadharNo);

                    if (string.IsNullOrWhiteSpace(Details.XML))
                    {
                        return Json(new { Status = "5" }, JsonRequestBehavior.AllowGet);
                    }

                    int logId = ObjCitizen.InserBioKYCLog(Details.XML, "", "", "", 0);

                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(Details.XML); // suppose that myXmlString contains "<Names>...</Names>"  



                    foreach (XmlNode node in xml.SelectNodes("//Resp[@errCode]"))
                    {
                        string code = node.Attributes["errCode"].Value;
                        if (code != "0")
                        {
                            return Json(new { Status = "5" }, JsonRequestBehavior.AllowGet);
                        }
                    }



                    KYCResponce result = AADHARSerice.VerifyBIO_KYCData(xml, Details.AadharNo, logId);// "428434778643");



                    if (result.Status == "1")
                    {
                        string fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper();
                        byte[] imageBytes = Convert.FromBase64String(result.UserDetails.Photo.ToString().Replace("data:image/png;base64,", ""));
                        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

                        fileName = COMMON_FUNCTION.GetUniqueFileName("Member_" + fileName, System.Web.HttpContext.Current.Server.MapPath("../Content/NewRegistration/"), ".jpg").ToLower();

                        FileStream fs = new FileStream(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/NewRegistration/"), fileName + ".jpg"), FileMode.CreateNew, FileAccess.Write);
                        ms.WriteTo(fs);
                        fs.Close();
                        fs.Dispose();
                        ms.Flush();
                        result.UserDetails.Photo = "../Content/NewRegistration/" + fileName + ".jpg";
                    }

                    return Json(new { Status = result.Status, UserDetails = result.UserDetails }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = "0" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                int logId = ObjCitizen.InserBioKYCLog(Details.XML, "", "", ex.StackTrace.ToString(), 0);
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "VerifyBIOKYC", FunctionName = " public JsonResult ValidateBIOKYC(BIOKYCRequest Details)" }, ex);
                return Json(new { Status = "5" }, JsonRequestBehavior.AllowGet);
            }

            //return "RJVSD2343246566";
        }




        [HttpPost]
        public JsonResult VerifyBIOKYC_bacup(string AadharNo)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser != null)
            {
                KYCResponce result = AADHARSerice.BIO_KYC(AadharNo);


                if (result.Status == "1")
                {
                    string fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper();
                    byte[] imageBytes = Convert.FromBase64String(result.UserDetails.Photo.ToString().Replace("data:image/png;base64,", ""));
                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

                    fileName = COMMON_FUNCTION.GetUniqueFileName("Member_" + fileName, System.Web.HttpContext.Current.Server.MapPath("../Content/NewRegistration/"), ".jpg").ToLower();

                    FileStream fs = new FileStream(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/NewRegistration/"), fileName + ".jpg"), FileMode.CreateNew, FileAccess.Write);
                    ms.WriteTo(fs);
                    fs.Close();
                    fs.Dispose();
                    ms.Flush();
                    result.UserDetails.Photo = "../Content/NewRegistration/" + fileName + ".jpg";
                }

                return Json(new { Status = result.Status, UserDetails = result.UserDetails }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = "0" }, JsonRequestBehavior.AllowGet);
            }

            //return "RJVSD2343246566";
        }




        public static DataTable ConvertListToDataTable(List<string[]> list)
        {
            // New table.
            DataTable table = new DataTable();
            // Get max columns.
            int columns = 0;
            foreach (var array in list)
            {
                if (array.Length > columns)
                {
                    columns = array.Length;
                }
            }
            // Add columns.
            for (int i = 0; i < columns; i++)
            {
                table.Columns.Add();
            }
            // Add rows.
            foreach (var array in list)
            {
                table.Rows.Add(array);
            }
            return table;
        }

        public static string GetEncryptedPIDXmlNew(string pidXml, string certPath, string ts, ref string certExpiryDate, ref string encSessionKey, ref string encHmac)
        {
            ENCRYPTER enc = new ENCRYPTER(certPath);
            byte[] pidXmlBytes = Encoding.UTF8.GetBytes(pidXml);
            byte[] session_key = enc.generateSessionKey();
            byte[] encrypted_skey = enc.encryptUsingPublicKey(session_key);
            byte[] encPidXmlBytes = enc.encryptUsingSessionKeyNew(session_key, pidXmlBytes, ts, false);
            byte[] hmac = enc.generateSha256Hash(pidXmlBytes);
            byte[] encHmacBytes = enc.encryptUsingSessionKeyNew(session_key, hmac, ts, true);

            encSessionKey = Convert.ToBase64String(encrypted_skey);
            encHmac = Convert.ToBase64String(encHmacBytes);
            certExpiryDate = enc.getCertificateIdentifier();
            return Convert.ToBase64String(encPidXmlBytes);
        }

        [HttpGet]
        [ActionName("GetSocietyDownloadList")]
        public string GetSocietyDownloadList(string SocietyIdENC)
        {
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objCitizenModel.GetSocietyDownloadList(SocietyIdENC));
        }

        public ActionResult Downloads()
        {
            return View();
        }

        public class ENCRYPTER
        {
            private const string ASYMMETRIC_ALGO = "RSA/ECB/PKCS1Padding";
            private const int SYMMETRIC_KEY_SIZE = 256;
            private DateTime certExpiryDate;
            private RsaKeyParameters publicKey;
            public static int IV_SIZE_BITS = 96;
            // Additional authentication data - last 128 bits of ISO format timestamp
            public static int AAD_SIZE_BITS = 128;
            // Authentication tag length - in bits
            public static int AUTH_TAG_SIZE_BITS = 128;

            public ENCRYPTER(string publicKeyFilePath)
            {
                FileStream fileStream = null;
                try
                {
                    using (fileStream = new FileStream(publicKeyFilePath, FileMode.Open, FileAccess.Read))
                    {
                        Org.BouncyCastle.X509.X509Certificate x509Certificate = new Org.BouncyCastle.X509.X509CertificateParser().ReadCertificate(fileStream);
                        this.publicKey = (RsaKeyParameters)x509Certificate.GetPublicKey();
                        this.certExpiryDate = x509Certificate.NotAfter;
                        fileStream.Close();
                    }
                }
                catch (Exception innerException)
                {
                    throw new Exception("Could not intialize encryption module", innerException);
                }
                finally
                {
                    if (fileStream != null) { fileStream.Close(); }
                }
            }

            public ENCRYPTER(byte[] publicKeyBytes)
            {
                try
                {
                    Org.BouncyCastle.X509.X509Certificate x509Certificate = new X509CertificateParser().ReadCertificate(publicKeyBytes);
                    this.publicKey = (RsaKeyParameters)x509Certificate.GetPublicKey();
                    this.certExpiryDate = x509Certificate.NotAfter;
                }
                catch (Exception innerException)
                {
                    throw new Exception("Could not intialize encryption module", innerException);
                }
            }

            public byte[] encryptUsingPublicKey(byte[] data)
            {
                IBufferedCipher cipher = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
                cipher.Init(true, this.publicKey);
                return cipher.DoFinal(data);
            }

            public byte[] encryptUsingSessionKey(byte[] skey, byte[] data)
            {
                PaddedBufferedBlockCipher paddedBufferedBlockCipher = new PaddedBufferedBlockCipher(new AesEngine(), new Pkcs7Padding());
                paddedBufferedBlockCipher.Init(true, new KeyParameter(skey));
                byte[] array = new byte[paddedBufferedBlockCipher.GetOutputSize(data.Length)];
                int num = paddedBufferedBlockCipher.ProcessBytes(data, 0, data.Length, array, 0);
                int num2 = paddedBufferedBlockCipher.DoFinal(array, num);
                byte[] array2 = new byte[num + num2];
                Array.Copy(array, 0, array2, 0, array2.Length);
                return array2;
            }

            // AES/GCM ENCRYPTION
            public byte[] encryptUsingSessionKeyNew(byte[] skey, byte[] data, string ts, bool isHmac)
            {
                byte[] tsBytes = null;
                try
                {
                    tsBytes = Encoding.UTF8.GetBytes(ts);
                    byte[] ivBytes = new byte[IV_SIZE_BITS / 8];
                    byte[] aadBytes = new byte[AAD_SIZE_BITS / 8];
                    Array.Copy(tsBytes, tsBytes.Length - 12, ivBytes, 0, ivBytes.Length);
                    Array.Copy(tsBytes, tsBytes.Length - 16, aadBytes, 0, aadBytes.Length);

                    var cipher = new GcmBlockCipher(new AesFastEngine());
                    var parameters = new AeadParameters(new KeyParameter(skey), AUTH_TAG_SIZE_BITS, ivBytes, aadBytes);
                    cipher.Init(true, parameters);
                    byte[] result = new byte[cipher.GetOutputSize(data.Length)];
                    int processLen = cipher.ProcessBytes(data, 0, data.Length, result, 0);
                    cipher.DoFinal(result, processLen);
                    if (isHmac) return result;
                    // With Add for Session Key Encryption
                    byte[] packedCipherData = new byte[result.Length + tsBytes.Length];
                    Array.Copy(tsBytes, 0, packedCipherData, 0, tsBytes.Length);
                    Array.Copy(result, 0, packedCipherData, tsBytes.Length, result.Length);
                    return packedCipherData;
                }
                catch { throw; }
            }

            public byte[] generateSessionKey()
            {
                SecureRandom random = new SecureRandom();
                KeyGenerationParameters parameters = new KeyGenerationParameters(random, 256);
                CipherKeyGenerator keyGenerator = GeneratorUtilities.GetKeyGenerator("AES");
                keyGenerator.Init(parameters);
                return keyGenerator.GenerateKey();
            }

            public byte[] generateSha256Hash(byte[] message)
            {
                IDigest digest = new Sha256Digest();
                digest.Reset();
                byte[] array = new byte[digest.GetDigestSize()];
                digest.BlockUpdate(message, 0, message.Length);
                digest.DoFinal(array, 0);
                return array;
            }

            public string getCertificateIdentifier()
            {
                return this.certExpiryDate.ToString("yyyyMMdd");
            }
        }


        [HttpGet]
        [ActionName("GetDeshboardData")]
        public string GetDeshboard(SocietyRegistrationEntity objSocietyRegistrationEntity)
        {

            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
            {
                objSocietyRegistrationEntity.UserType = (Request.Cookies["SSOROLE"].Value == "CITIZEN" ? "C" : "D");
                objSocietyRegistrationEntity.SSOID = Request.Cookies["SSO_ID"].Value;
            }
            objSocietyRegistrationEntity.LevelId = _LoggedInUser.LevelId;
            objSocietyRegistrationEntity.DivisionId = _LoggedInUser.DivisionId;
            objSocietyRegistrationEntity.DistrictId = _LoggedInUser.DistrictId;
            objSocietyRegistrationEntity.CityId = _LoggedInUser.CityId;
            objSocietyRegistrationEntity.WardId = _LoggedInUser.WardId;
            objSocietyRegistrationEntity.PanchayatSamitiId = _LoggedInUser.PanchayatSamitiId;
            objSocietyRegistrationEntity.GPId = _LoggedInUser.GPId;



            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            DataSet DT = objCitizenModel.getdeshboad(objSocietyRegistrationEntity);
            //return COMMON_FUNCTION.DataTableToJSONString(DT);
            return JsonConvert.SerializeObject(DT, Newtonsoft.Json.Formatting.Indented);
        }
        [HttpGet]
        [ActionName("GetApexDeshboardData")]
        public string GetApexDeshboardData(SocietyRegistrationEntity objSocietyRegistrationEntity)
        {

            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
            {
                objSocietyRegistrationEntity.UserType = (Request.Cookies["SSOROLE"].Value == "CITIZEN" ? "C" : "D");
                objSocietyRegistrationEntity.SSOID = Request.Cookies["SSO_ID"].Value;
            }
            objSocietyRegistrationEntity.LevelId = _LoggedInUser.LevelId;
            objSocietyRegistrationEntity.DivisionId = _LoggedInUser.DivisionId;
            objSocietyRegistrationEntity.DistrictId = _LoggedInUser.DistrictId;
            objSocietyRegistrationEntity.CityId = _LoggedInUser.CityId;
            objSocietyRegistrationEntity.WardId = _LoggedInUser.WardId;
            objSocietyRegistrationEntity.PanchayatSamitiId = _LoggedInUser.PanchayatSamitiId;
            objSocietyRegistrationEntity.GPId = _LoggedInUser.GPId;



            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            DataSet DT = objCitizenModel.getApexdeshboad(objSocietyRegistrationEntity);
            //return COMMON_FUNCTION.DataTableToJSONString(DT);
            return JsonConvert.SerializeObject(DT, Newtonsoft.Json.Formatting.Indented);
        }
        [HttpGet]
        [ActionName("GetRajfedDashboardData")]
        public string GetRajfedDashboardData(SocietyRegistrationEntity objSocietyRegistrationEntity)
        {

            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
            {
                objSocietyRegistrationEntity.UserType = (Request.Cookies["SSOROLE"].Value == "CITIZEN" ? "C" : "D");
                objSocietyRegistrationEntity.SSOID = Request.Cookies["SSO_ID"].Value;
            }
            objSocietyRegistrationEntity.LevelId = _LoggedInUser.LevelId;
            objSocietyRegistrationEntity.DivisionId = _LoggedInUser.DivisionId;
            objSocietyRegistrationEntity.DistrictId = _LoggedInUser.DistrictId;
            objSocietyRegistrationEntity.CityId = _LoggedInUser.CityId;
            objSocietyRegistrationEntity.WardId = _LoggedInUser.WardId;
            objSocietyRegistrationEntity.PanchayatSamitiId = _LoggedInUser.PanchayatSamitiId;
            objSocietyRegistrationEntity.GPId = _LoggedInUser.GPId;



            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            DataSet DT = objCitizenModel.GetRajfedDashboardData(objSocietyRegistrationEntity);
            //return COMMON_FUNCTION.DataTableToJSONString(DT);
            return JsonConvert.SerializeObject(DT, Newtonsoft.Json.Formatting.Indented);
        }

        public ActionResult SocietyPayments()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.SocietyPayments, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpGet]
        [ActionName("SocietyPayment")]
        public string SocietyPaymentdata()
        {

            string WhereClause = "";

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            WhereClause = " and SSOID='" + _LoggedInUser.SSOID + "'";
            //string WhereClause = " and RegistrationNo='" + SocietyRegistrationNo + "'";
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            DataTable dt = objCitizenModel.GetSocietyPayment(WhereClause);


            return COMMON_FUNCTION.DataTableToJSONString(dt); ;
        }

        [HttpGet]
        [ActionName("UpdateOnlinePaymentStatu_get_ssuc")]
        public ActionResult UpdateOnlinePaymentStatu_get_ssuc()
        {
            return RedirectToAction("ShowResponce");
        }

        [HttpPost]
        [ActionName("UpdateOnlinePaymentStatu_Post_ssuc")]
        public ActionResult UpdateOnlinePaymentStatu_Post_ssuc()
        {
            return RedirectToAction("ShowResponce");
        }

        [HttpPost]
        [ActionName("UpdatePaymentStatus")]
        public ActionResult UpdateOnlinePaymentStatus(string UniquerequestId, string SocietyIdEnc = "", string ServiceID = "", string IsFailed = "")
        {
            DataTable Dt = null;
            string decryptData = "";
            int i = -1;
            PGResponse objPGResponse = null;
            UniquerequestId = UniquerequestId.Replace(' ', '+');
            UniquerequestId = UniquerequestId.Replace(' ', '+');
            UniquerequestId = UniquerequestId.Replace(' ', '+');
            string DivHtml = "";
            UniquerequestId = COMMON_FUNCTION.Decrypt(UniquerequestId);

            IsFailed = COMMON_FUNCTION.Decrypt(IsFailed.Replace(' ', '+'));
            EmitraModelFactory objEmitraModel = new EmitraModelFactory(new FileLogger());
            Dt = objEmitraModel.GetApplicationKey("4173", false);

            if (Request["encData"] != null && Dt.Rows.Count > 0)
            {
                EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES(Dt.Rows[0]["encryptionkey"].ToString());
                decryptData = objEncryptDecrypt.decrypt(Request["encData"].ToString());
                CitizenModel objCitizenModel = new CitizenModel(new FileLogger());
                DivHtml = objCitizenModel.UpdateOnlinePaymentStatus("", SocietyIdEnc, UniquerequestId, IsFailed, decryptData, "1");

            }
            //ShowResponce(DivHtml);
            objPGResponse = JsonConvert.DeserializeObject<PGResponse>(DivHtml);

            string Tbl = "";
            Tbl = "<div class=\"col-lg-12 table-responsive\"><table class=\"table devtable\">" +
            "<thead><tr><th class=\"ng-binding\">Column 1</th>" +
            "<th class=\"ng-binding\">Column 2</th><th class=\"ng-binding\">Column 3</th>" +
            "<th>Column 4</th></tr></thead><tbody>" +
            "<tr><td class=\"ng-binding\">TRANSACTIONID</td><td class=\"ng-binding\">" + objPGResponse.TRANSACTIONID + "</td><td class=\"ng-binding\">RECEIPTNO</td><td class=\"ng-binding\">" + objPGResponse.RECEIPTNO + "</td></tr>" +
            "<tr><td class=\"ng-binding\">PAIDAMOUNT</td><td class=\"ng-binding\">" + objPGResponse.PAIDAMOUNT + "</td><td class=\"ng-binding\">Date</td><td class=\"ng-binding\">" + objPGResponse.EMITRATIMESTAMP.Substring(0, 8) + "</td></tr>" +
            "<tr><td class=\"ng-binding\">BANK_NAME</td><td class=\"ng-binding\">" + objPGResponse.BANK_NAME + "</td><td class=\"ng-binding\">BANK ACC NO.</td><td class=\"ng-binding\">" + objPGResponse.BANK_ACC_NO + "</td></tr>" +
            "<tr><td class=\"ng-binding\">RESPONSEMESSAGE</td><td class=\"ng-binding\">" + objPGResponse.RESPONSEMESSAGE + "</td><td class=\"ng-binding\"></td><td class=\"ng-binding\"></td></tr>" +
            "</tbody></table></div>";

            TempData["MyData"] = Tbl;
            return RedirectToAction("ShowResponce");
        }

        [HttpGet]
        public ActionResult ShowResponce()
        {

            ViewBag.responce = TempData["MyData"];
            return View();
        }

        [HttpPost]
        [ActionName("FeePaymentByCitizen")]
        public JsonResult FeePaymentByEmitra(string RegistrationNo)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            DataTable DT = null;
            string str = string.Empty;
            string WhereClause = " and Societies.RegistrationNo='" + RegistrationNo + "'";
            EmitraModelFactory objEmitraModel = new EmitraModelFactory(new DBLogger());
            DT = objEmitraModel.GetSocietyPaymentList(WhereClause);

            if (_LoggedInUser.UserType == "K")
            {
                if (DT.Rows.Count > 0 && (DT.Rows[0]["StatusId"].ToString() == "33" || DT.Rows[0]["StatusId"].ToString() == "1081"))
                {
                    str = objEmitraModel.FeeDeposit(DT.Rows[0]["SocietyIdEnc"].ToString(), DT.Rows[0]["RegistrationNo"].ToString(), DT.Rows[0]["SocietyName"].ToString());
                }
            }
            else if (DT.Rows.Count > 0 && _LoggedInUser.UserType == "C" && (DT.Rows[0]["StatusId"].ToString() == "33" || DT.Rows[0]["StatusId"].ToString() == "1081"))
            {
                str = objEmitraModel.UpdateEmitrTransation("", DT.Rows[0]["SocietyName"].ToString(), "0", DT.Rows[0]["RegistrationNo"].ToString(), DT.Rows[0]["SocietyId"].ToString(), DT.Rows[0]["SocietyIdEnc"].ToString(), "5531", false, "1", DT.Rows[0]["Amount"].ToString());
                //UpdateEmitrTransation
                // Response.Write(str);
            }

            return Json(new { status = "OK", UserType = _LoggedInUser.UserType, result = str }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult ApplicationStatusByCitizen()
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



        public ActionResult UpdateCityzenPaymentStatus(string RegistrationNo = "", string UniquerequestId = "", string SocietyIdEnc = "", string IsFailed = "", string IsPermanentSave = "")
        {
            string encData = "";
            DataTable Dt = null;
            string decryptData = "";
            PGResponse objPGResponse = null;

            string DivHtml = "";

            UniquerequestId = UniquerequestId.Replace(' ', '+');
            UniquerequestId = UniquerequestId.Replace(' ', '+');
            UniquerequestId = UniquerequestId.Replace(' ', '+');

            SocietyIdEnc = SocietyIdEnc.Replace(' ', '+');
            SocietyIdEnc = SocietyIdEnc.Replace(' ', '+');
            SocietyIdEnc = SocietyIdEnc.Replace(' ', '+');

            IsFailed = IsFailed.Replace(' ', '+');
            IsFailed = IsFailed.Replace(' ', '+');
            IsFailed = IsFailed.Replace(' ', '+');


            IsPermanentSave = IsPermanentSave.Replace(' ', '+');
            IsPermanentSave = IsPermanentSave.Replace(' ', '+');
            IsPermanentSave = IsPermanentSave.Replace(' ', '+');

            UserModel OBJUser = new UserModel(new FileLogger());
            if (Request["encData"] != null)
            {
                EmitraModelFactory objEmitraModel = new EmitraModelFactory(new FileLogger());
                Dt = objEmitraModel.GetApplicationKey("5531", false);
                encData = Request["encData"].ToString().Replace("@!", "+");
                EncryptDecrypt3DES obj = new EncryptDecrypt3DES(Dt.Rows[0]["encryptionkey"].ToString());
                decryptData = obj.decrypt(encData);
                CitizenModel objCitizenModel = new CitizenModel(new FileLogger());
                (new EmitraModelFactory(new FileLogger())).SavePaymentResponce("Society Registration:  IsFail=" + COMMON_FUNCTION.Decrypt(IsFailed) + "--:-- " + decryptData);
                objPGResponse = JsonConvert.DeserializeObject<PGResponse>(decryptData);
                Dt = new EmitraModelFactory(new FileLogger()).GetApplicationKey(objPGResponse.SERVICEID, true);

                DivHtml = objCitizenModel.UpdateOnlinePaymentStatus(RegistrationNo, SocietyIdEnc, COMMON_FUNCTION.Decrypt(UniquerequestId), COMMON_FUNCTION.Decrypt(IsFailed), decryptData, COMMON_FUNCTION.Decrypt(IsPermanentSave));

                objPGResponse = JsonConvert.DeserializeObject<PGResponse>(DivHtml);
                if (objPGResponse.RESPONSEMESSAGE.ToString().Contains("Successfully") && objPGResponse.INTERNAL_ID > 0 && objPGResponse.SERVICEID == "5530")
                {
                    string FResult = "";
                    int Count = 0;
                    SocietyController objSociety = new SocietyController();

                    string str = objSociety.GetSocietyDetailsBySocietyId(SocietyIdEnc);
                    DataTable dt = (DataTable)JsonConvert.DeserializeObject(str, (typeof(DataTable)));
                    if (dt.Rows[0]["RegistrationNo"] == null || dt.Rows[0]["RegistrationNo"].ToString().Length == 0)
                    {
                        FResult = objSociety.SaveSocietyFinalSubmition(SocietyIdEnc);
                        var REG = FResult.Split('#');
                        if (REG.Length > 1)
                        {
                            foreach (DataRow Dr in dt.Rows)
                            {
                                Dr["SocietyDirectory"] = REG[1].ToString().Replace("/", "");
                            }
                        }
                    }
                    else if (!dt.Rows[0]["RegistrationNo"].ToString().Contains("COOP"))
                    {
                        FResult = objSociety.SaveSocietyFinalSubmition(SocietyIdEnc);
                        var REG = FResult.Split('#');
                        if (REG.Length > 1)
                        {
                            foreach (DataRow Dr in dt.Rows)
                            {
                                Dr["SocietyDirectory"] = REG[1].ToString().Replace("/", "");
                            }
                        }
                    }

                    DataTable selectedTable = dt.AsEnumerable()
                        .Where(r => r.Field<string>("AddressFor") == "0")
                        .CopyToDataTable();

                    dt = selectedTable;
                    string pathString = "~/Content/SocietyProfiling/";
                    string SourceFilePath = "";
                    string TargetFilePath = "~/Content/SocietyProfiling/";
                    foreach (DataRow Dr in dt.Rows)
                    {
                        if (Count == 0)
                        {
                            pathString += Regex.Replace(dt.Rows[0]["SocietyDirectory"].ToString(), "[^a-zA-Z0-9_]+", "");
                            if (!System.IO.Directory.Exists(Server.MapPath(pathString)))
                            {
                                System.IO.Directory.CreateDirectory(Server.MapPath(pathString));
                            }

                            if (Dr["RegistrationCertificateFile"] != null && !Dr["RegistrationCertificateFile"].ToString().Contains("undefined") && Dr["RegistrationCertificateFile"].ToString() != "")
                            {
                                if ((Dr["RegistrationCertificateFile"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["RegistrationCertificateFile"].ToString().Split('/');
                                    var TempRegistrationCertificateFile = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempRegistrationCertificateFile;
                                    TargetFilePath = pathString + "/" + TempRegistrationCertificateFile;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }

                            if (Dr["BylawsFile"] != null && !Dr["BylawsFile"].ToString().Contains("undefined") && Dr["BylawsFile"].ToString() != "")
                            {
                                if ((Dr["BylawsFile"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["BylawsFile"].ToString().Split('/');
                                    var TempBylawsFile = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempBylawsFile;
                                    TargetFilePath = pathString + "/" + TempBylawsFile;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }

                            if (Dr["RulesAndRegulationFile"] != null && !Dr["RulesAndRegulationFile"].ToString().Contains("undefined") && Dr["RulesAndRegulationFile"].ToString() != "")
                            {
                                if ((Dr["RulesAndRegulationFile"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["RulesAndRegulationFile"].ToString().Split('/');
                                    var TempRulesAndRegulationFile = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempRulesAndRegulationFile;
                                    TargetFilePath = pathString + "/" + TempRulesAndRegulationFile;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }


                            if (Dr["MemberListFile"] != null && !Dr["MemberListFile"].ToString().Contains("undefined") && Dr["MemberListFile"].ToString() != "")
                            {
                                if ((Dr["MemberListFile"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["MemberListFile"].ToString().Split('/');
                                    var TempMemberListFile = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempMemberListFile;
                                    TargetFilePath = pathString + "/" + TempMemberListFile;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }


                            if (Dr["WorkPlanEcoViabilityFile"] != null && !Dr["WorkPlanEcoViabilityFile"].ToString().Contains("undefined") && Dr["WorkPlanEcoViabilityFile"].ToString() != "")
                            {
                                if ((Dr["WorkPlanEcoViabilityFile"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["WorkPlanEcoViabilityFile"].ToString().Split('/');
                                    var TempWorkPlanEcoViabilityFile = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempWorkPlanEcoViabilityFile;
                                    TargetFilePath = pathString + "/" + TempWorkPlanEcoViabilityFile;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }

                            if (Dr["GeneralMeetingProcedureFile"] != null && !Dr["GeneralMeetingProcedureFile"].ToString().Contains("undefined") && Dr["GeneralMeetingProcedureFile"].ToString() != "")
                            {
                                if ((Dr["GeneralMeetingProcedureFile"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["GeneralMeetingProcedureFile"].ToString().Split('/');
                                    var TempGeneralMeetingProcedureFile = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempGeneralMeetingProcedureFile;
                                    TargetFilePath = pathString + "/" + TempGeneralMeetingProcedureFile;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }

                            if (Dr["BalanceSheetExcelFile"] != null && !Dr["BalanceSheetExcelFile"].ToString().Contains("undefined") && Dr["BalanceSheetExcelFile"].ToString() != "")
                            {
                                if ((Dr["BalanceSheetExcelFile"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["GeneralMeetingProcedureFile"].ToString().Split('/');
                                    var TempBalanceSheetExcelFile = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempBalanceSheetExcelFile;
                                    TargetFilePath = pathString + "/" + TempBalanceSheetExcelFile;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }

                            if (Dr["ObjectsFile"] != null && !Dr["ObjectsFile"].ToString().Contains("undefined") && Dr["ObjectsFile"].ToString() != "")
                            {
                                if ((Dr["ObjectsFile"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["ObjectsFile"].ToString().Split('/');
                                    var TempObjectsFile = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempObjectsFile;
                                    TargetFilePath = pathString + "/" + TempObjectsFile;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }

                            if (Dr["SLSAFileName"] != null && !Dr["SLSAFileName"].ToString().Contains("undefined") && Dr["SLSAFileName"].ToString() != "")
                            {
                                if ((Dr["SLSAFileName"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["SLSAFileName"].ToString().Split('/');
                                    var TempSLSAFileName = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempSLSAFileName;
                                    TargetFilePath = pathString + "/" + TempSLSAFileName;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }
                            if (Dr["RSSCFileName"] != null && !Dr["RSSCFileName"].ToString().Contains("undefined") && Dr["RSSCFileName"].ToString() != "")
                            {
                                if ((Dr["RSSCFileName"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["RSSCFileName"].ToString().Split('/');
                                    var TempRSSCFileName = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempRSSCFileName;
                                    TargetFilePath = pathString + "/" + TempRSSCFileName;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }
                            if (Dr["DLSCFileName"] != null && !Dr["DLSCFileName"].ToString().Contains("undefined") && Dr["DLSCFileName"].ToString() != "")
                            {
                                if ((Dr["DLSCFileName"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["DLSCFileName"].ToString().Split('/');
                                    var TempDLSCFileName = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempDLSCFileName;
                                    TargetFilePath = pathString + "/" + TempDLSCFileName;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }
                            if (Dr["SOCFileName"] != null && !Dr["SOCFileName"].ToString().Contains("undefined") && Dr["SOCFileName"].ToString() != "")
                            {
                                if ((Dr["SOCFileName"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["SOCFileName"].ToString().Split('/');
                                    var TempSOCFileName = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempSOCFileName;
                                    TargetFilePath = pathString + "/" + TempSOCFileName;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }

                        }

                        if (Dr["Photo"] != null && Dr["Photo"].ToString() != "" && !Dr["Photo"].ToString().Contains("undefined") && !Dr["Photo"].ToString().Contains("Balnk"))
                        {
                            var temp = Dr["Photo"].ToString().Split('/');
                            if (temp.Length > 3)
                            {
                                var TempPhoto = temp[temp.Length - 1];
                                SourceFilePath = "~/Content/NewRegistration/" + TempPhoto;
                                TargetFilePath = pathString + "/" + TempPhoto;
                                if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                    System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                            }
                        }
                        Count = Count + 1;
                    }

                    return RedirectToAction("SocietyApplicationView", "Society", new { SocietyIDEnc = SocietyIdEnc + "~Success^" + objPGResponse.RESPONSEMESSAGE });
                }
                else
                {
                    return RedirectToAction("SocietyApplicationView", "Society", new { SocietyIDEnc = SocietyIdEnc + "~Error^" + objPGResponse.RESPONSEMESSAGE });
                    //return RedirectToAction("MemberApplicationView", "Citizen", new { ApplicationIdEnc = "Error" + objPGResponse.RESPONSEMESSAGE });
                }
                //ViewBag.DivHtmlData = DivHtml;
            }
            return View();
            //return "OKAY";
        }

        [HttpPost]
        public ActionResult UpdateSocietyFeePaymentStatus(string RegistrationNo = "", string UniquerequestId = "", string SocietyIdEnc = "", string IsFailed = "", string IsPermanentSave = "")
        {
            string encData = "";
            DataTable Dt = null;
            string decryptData = "";
            PGResponse objPGResponse = null;

            string DivHtml = "";

            UniquerequestId = UniquerequestId.Replace(' ', '+');
            UniquerequestId = UniquerequestId.Replace(' ', '+');
            UniquerequestId = UniquerequestId.Replace(' ', '+');

            SocietyIdEnc = SocietyIdEnc.Replace(' ', '+');
            SocietyIdEnc = SocietyIdEnc.Replace(' ', '+');
            SocietyIdEnc = SocietyIdEnc.Replace(' ', '+');

            IsFailed = IsFailed.Replace(' ', '+');
            IsFailed = IsFailed.Replace(' ', '+');
            IsFailed = IsFailed.Replace(' ', '+');


            IsPermanentSave = IsPermanentSave.Replace(' ', '+');
            IsPermanentSave = IsPermanentSave.Replace(' ', '+');
            IsPermanentSave = IsPermanentSave.Replace(' ', '+');

            UserModel OBJUser = new UserModel(new FileLogger());
            if (Request["encData"] != null)
            {
                EmitraModelFactory objEmitraModel = new EmitraModelFactory(new FileLogger());
                Dt = objEmitraModel.GetApplicationKey("5531", false);
                encData = Request["encData"].ToString().Replace("@!", "+");
                EncryptDecrypt3DES obj = new EncryptDecrypt3DES(Dt.Rows[0]["encryptionkey"].ToString());
                decryptData = obj.decrypt(encData);
                CitizenModel objCitizenModel = new CitizenModel(new FileLogger());
                objPGResponse = JsonConvert.DeserializeObject<PGResponse>(decryptData);
                Dt = new EmitraModelFactory(new FileLogger()).GetApplicationKey(objPGResponse.SERVICEID, true);

                DivHtml = objCitizenModel.UpdateOnlinePaymentStatus(RegistrationNo, SocietyIdEnc, COMMON_FUNCTION.Decrypt(UniquerequestId), COMMON_FUNCTION.Decrypt(IsFailed), decryptData, COMMON_FUNCTION.Decrypt(IsPermanentSave));

                objPGResponse = JsonConvert.DeserializeObject<PGResponse>(DivHtml);
                if (objPGResponse.RESPONSEMESSAGE.ToString().Contains("Successfully") && objPGResponse.INTERNAL_ID > 0 && objPGResponse.SERVICEID == "5530")
                {
                    string FResult = "";
                    int Count = 0;
                    SocietyController objSociety = new SocietyController();

                    string str = objSociety.GetSocietyDetailsBySocietyId(SocietyIdEnc);
                    DataTable dt = (DataTable)JsonConvert.DeserializeObject(str, (typeof(DataTable)));
                    if (dt.Rows[0]["RegistrationNo"] == null || dt.Rows[0]["RegistrationNo"].ToString().Length == 0)
                    {
                        FResult = objSociety.SaveSocietyFinalSubmition(SocietyIdEnc);
                        var REG = FResult.Split('#');
                        if (REG.Length > 1)
                        {
                            foreach (DataRow Dr in dt.Rows)
                            {
                                Dr["SocietyDirectory"] = REG[1].ToString().Replace("/", "");
                            }
                        }
                    }
                    else if (!dt.Rows[0]["RegistrationNo"].ToString().Contains("COOP"))
                    {
                        FResult = objSociety.SaveSocietyFinalSubmition(SocietyIdEnc);
                        var REG = FResult.Split('#');
                        if (REG.Length > 1)
                        {
                            foreach (DataRow Dr in dt.Rows)
                            {
                                Dr["SocietyDirectory"] = REG[1].ToString().Replace("/", "");
                            }
                        }
                    }

                    DataTable selectedTable = dt.AsEnumerable()
                        .Where(r => r.Field<string>("AddressFor") == "0")
                        .CopyToDataTable();

                    dt = selectedTable;
                    string pathString = "~/Content/SocietyProfiling/";
                    string SourceFilePath = "";
                    string TargetFilePath = "~/Content/SocietyProfiling/";
                    foreach (DataRow Dr in dt.Rows)
                    {
                        if (Count == 0)
                        {
                            pathString += Regex.Replace(dt.Rows[0]["SocietyDirectory"].ToString(), "[^a-zA-Z0-9_]+", "");
                            if (!System.IO.Directory.Exists(Server.MapPath(pathString)))
                            {
                                System.IO.Directory.CreateDirectory(Server.MapPath(pathString));
                            }

                            if (Dr["RegistrationCertificateFile"] != null && !Dr["RegistrationCertificateFile"].ToString().Contains("undefined") && Dr["RegistrationCertificateFile"].ToString() != "")
                            {
                                if ((Dr["RegistrationCertificateFile"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["RegistrationCertificateFile"].ToString().Split('/');
                                    var TempRegistrationCertificateFile = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempRegistrationCertificateFile;
                                    TargetFilePath = pathString + "/" + TempRegistrationCertificateFile;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }

                            if (Dr["BylawsFile"] != null && !Dr["BylawsFile"].ToString().Contains("undefined") && Dr["BylawsFile"].ToString() != "")
                            {
                                if ((Dr["BylawsFile"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["BylawsFile"].ToString().Split('/');
                                    var TempBylawsFile = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempBylawsFile;
                                    TargetFilePath = pathString + "/" + TempBylawsFile;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }

                            if (Dr["RulesAndRegulationFile"] != null && !Dr["RulesAndRegulationFile"].ToString().Contains("undefined") && Dr["RulesAndRegulationFile"].ToString() != "")
                            {
                                if ((Dr["RulesAndRegulationFile"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["RulesAndRegulationFile"].ToString().Split('/');
                                    var TempRulesAndRegulationFile = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempRulesAndRegulationFile;
                                    TargetFilePath = pathString + "/" + TempRulesAndRegulationFile;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }


                            if (Dr["MemberListFile"] != null && !Dr["MemberListFile"].ToString().Contains("undefined") && Dr["MemberListFile"].ToString() != "")
                            {
                                if ((Dr["MemberListFile"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["MemberListFile"].ToString().Split('/');
                                    var TempMemberListFile = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempMemberListFile;
                                    TargetFilePath = pathString + "/" + TempMemberListFile;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }


                            if (Dr["WorkPlanEcoViabilityFile"] != null && !Dr["WorkPlanEcoViabilityFile"].ToString().Contains("undefined") && Dr["WorkPlanEcoViabilityFile"].ToString() != "")
                            {
                                if ((Dr["WorkPlanEcoViabilityFile"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["WorkPlanEcoViabilityFile"].ToString().Split('/');
                                    var TempWorkPlanEcoViabilityFile = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempWorkPlanEcoViabilityFile;
                                    TargetFilePath = pathString + "/" + TempWorkPlanEcoViabilityFile;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }

                            if (Dr["GeneralMeetingProcedureFile"] != null && !Dr["GeneralMeetingProcedureFile"].ToString().Contains("undefined") && Dr["GeneralMeetingProcedureFile"].ToString() != "")
                            {
                                if ((Dr["GeneralMeetingProcedureFile"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["GeneralMeetingProcedureFile"].ToString().Split('/');
                                    var TempGeneralMeetingProcedureFile = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempGeneralMeetingProcedureFile;
                                    TargetFilePath = pathString + "/" + TempGeneralMeetingProcedureFile;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }

                            if (Dr["BalanceSheetExcelFile"] != null && !Dr["BalanceSheetExcelFile"].ToString().Contains("undefined") && Dr["BalanceSheetExcelFile"].ToString() != "")
                            {
                                if ((Dr["BalanceSheetExcelFile"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["GeneralMeetingProcedureFile"].ToString().Split('/');
                                    var TempBalanceSheetExcelFile = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempBalanceSheetExcelFile;
                                    TargetFilePath = pathString + "/" + TempBalanceSheetExcelFile;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }

                            if (Dr["ObjectsFile"] != null && !Dr["ObjectsFile"].ToString().Contains("undefined") && Dr["ObjectsFile"].ToString() != "")
                            {
                                if ((Dr["ObjectsFile"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["ObjectsFile"].ToString().Split('/');
                                    var TempObjectsFile = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempObjectsFile;
                                    TargetFilePath = pathString + "/" + TempObjectsFile;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }

                            if (Dr["SLSAFileName"] != null && !Dr["SLSAFileName"].ToString().Contains("undefined") && Dr["SLSAFileName"].ToString() != "")
                            {
                                if ((Dr["SLSAFileName"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["SLSAFileName"].ToString().Split('/');
                                    var TempSLSAFileName = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempSLSAFileName;
                                    TargetFilePath = pathString + "/" + TempSLSAFileName;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }
                            if (Dr["RSSCFileName"] != null && !Dr["RSSCFileName"].ToString().Contains("undefined") && Dr["RSSCFileName"].ToString() != "")
                            {
                                if ((Dr["RSSCFileName"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["RSSCFileName"].ToString().Split('/');
                                    var TempRSSCFileName = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempRSSCFileName;
                                    TargetFilePath = pathString + "/" + TempRSSCFileName;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }
                            if (Dr["DLSCFileName"] != null && !Dr["DLSCFileName"].ToString().Contains("undefined") && Dr["DLSCFileName"].ToString() != "")
                            {
                                if ((Dr["DLSCFileName"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["DLSCFileName"].ToString().Split('/');
                                    var TempDLSCFileName = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempDLSCFileName;
                                    TargetFilePath = pathString + "/" + TempDLSCFileName;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }
                            if (Dr["SOCFileName"] != null && !Dr["SOCFileName"].ToString().Contains("undefined") && Dr["SOCFileName"].ToString() != "")
                            {
                                if ((Dr["SOCFileName"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["SOCFileName"].ToString().Split('/');
                                    var TempSOCFileName = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempSOCFileName;
                                    TargetFilePath = pathString + "/" + TempSOCFileName;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }

                        }

                        if (Dr["Photo"] != null && Dr["Photo"].ToString() != "" && !Dr["Photo"].ToString().Contains("undefined") && !Dr["Photo"].ToString().Contains("Balnk"))
                        {
                            var temp = Dr["Photo"].ToString().Split('/');
                            if (temp.Length > 3)
                            {
                                var TempPhoto = temp[temp.Length - 1];
                                SourceFilePath = "~/Content/NewRegistration/" + TempPhoto;
                                TargetFilePath = pathString + "/" + TempPhoto;
                                if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                    System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                            }
                        }
                        Count = Count + 1;
                    }

                    return RedirectToAction("SocietyApplicationView", "Society", new { SocietyIDEnc = SocietyIdEnc + "~Success^" + objPGResponse.RESPONSEMESSAGE });
                }
                else
                {
                    return RedirectToAction("SocietyApplicationView", "Society", new { SocietyIDEnc = SocietyIdEnc + "~Error^" + objPGResponse.RESPONSEMESSAGE });
                    //return RedirectToAction("MemberApplicationView", "Citizen", new { ApplicationIdEnc = "Error" + objPGResponse.RESPONSEMESSAGE });
                }
                //ViewBag.DivHtmlData = DivHtml;
            }
            return View();
            //return "OKAY";
        }

        [HttpPost]
        public string GetFailedTransactionList(string SocietyIdEnc, string SocietyRegno = "", string VerificationId = "")
        {
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            SocietyRegistrationEntity objSocietyRegistrationEntity = new SocietyRegistrationEntity();
            if (_LoggedInUser.UserType == "C")
            {
                objSocietyRegistrationEntity.SSOID = _LoggedInUser.SSOID;
                objSocietyRegistrationEntity.SocietyIDEnc = SocietyIdEnc;
                objSocietyRegistrationEntity.UserType = _LoggedInUser.UserType;
                objSocietyRegistrationEntity.RegistrationNo = SocietyRegno;

            }
            else
            {
                objSocietyRegistrationEntity.RegistrationNo = SocietyRegno;
                objSocietyRegistrationEntity.UserType = _LoggedInUser.UserType;
            }

            return COMMON_FUNCTION.DataTableToJSONString(objCitizenModel.GetFailedTransactionList(objSocietyRegistrationEntity, VerificationId));
        }



        #region Court 
        [HttpGet]
        public ActionResult CaseRegistraion()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.CaseRegistraion, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }
        [HttpPost]
        [ActionName("SaveCaseRegistraion")]
        public JsonResult InsertCaseRegistraion(CaseRegistraion _Modal)
        {
            _Modal.SSOID = _LoggedInUser.SSOID;
            if (_LoggedInUser.UserType == "C")
            { _Modal.UserId = 0; }
            else
            { _Modal.UserId = Convert.ToInt32(_LoggedInUser.UserId); }

            _Modal.UserType = _LoggedInUser.UserType;

            CitizenModel _citobj = new CitizenModel(new FileLogger());
            return Json(_citobj.AddCaseRegistraion(_Modal));

        }
        [HttpPost]
        [ActionName("SaveCaseDetails")]
        public JsonResult InsertCaseDetails(CaseDetails _Modal)
        {

            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();

            try
            {
                _Modal.lstAppliantScope = Request.Form["lstAppliantScope"].ToString().Equals("[]") ? new List<AppliantScopeEntity>() : (List<AppliantScopeEntity>)s.Deserialize(Request.Form["lstAppliantScope"].ToString(), typeof(List<AppliantScopeEntity>));
                //if (!ModelState.IsValid)
                //    return Json(ModelState.Values);
            }
            catch (Exception ex)
            {
                //return CommonMessageFactory.Response("Provided input data is not in correct format.");
            }

            CitizenModel _citobj = new CitizenModel(new FileLogger());
            return Json(_citobj.AddCaseDetails(_Modal));

        }

        [HttpPost]
        [ActionName("SaveRespCaseDetails")]
        public JsonResult InsertRespCaseDetails(CaseDetails _Modal)
        {

            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();

            try
            {
                _Modal.lstRespondScope = Request.Form["lstRespondScope"].ToString().Equals("[]") ? new List<RespondScopeEntity>() : (List<RespondScopeEntity>)s.Deserialize(Request.Form["lstRespondScope"].ToString(), typeof(List<RespondScopeEntity>));
                //if (!ModelState.IsValid)
                //    return Json(ModelState.Values);
            }
            catch (Exception ex)
            {
                //return CommonMessageFactory.Response("Provided input data is not in correct format.");
            }

            CitizenModel _citobj = new CitizenModel(new FileLogger());
            return Json(_citobj.AddRespCaseDetails(_Modal));

        }

        [HttpPost]
        [ActionName("DeleteEnclouser")]
        public JsonResult delete_record(int id)
        {
            CitizenModel objTrainningModel = new CitizenModel(new FileLogger());
            return Json(objTrainningModel.DeleteEnclouser(id), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("DeleteOIC")]
        public JsonResult delete_OICrecord(int id)
        {
            CitizenModel objTrainningModel = new CitizenModel(new FileLogger());
            return Json(objTrainningModel.DeleteOIC(id), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("DeleteAdvocateDetail")]
        public JsonResult DeleteAdvocateDetail(int id)
        {
            CitizenModel objTrainningModel = new CitizenModel(new FileLogger());
            return Json(objTrainningModel.DeleteAdvocateDetail(id), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ActionName("SaveAdvocateDetails")]
        public JsonResult InsertAdvocateDetails(AdvocateDetails _Modal)
        {

            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();

            try
            {
                _Modal.lstAdvocateScope = Request.Form["lstAdvocateScope"].ToString().Equals("[]") ? new List<AdvocateScopeEntity>() : (List<AdvocateScopeEntity>)s.Deserialize(Request.Form["lstAdvocateScope"].ToString(), typeof(List<AdvocateScopeEntity>));
                //if (!ModelState.IsValid)
                //    return Json(ModelState.Values);
            }
            catch (Exception ex)
            {
                //return CommonMessageFactory.Response("Provided input data is not in correct format.");
            }


            CitizenModel _citobj = new CitizenModel(new FileLogger());
            return Json(_citobj.AddAdvocateDetails(_Modal));

        }


        [HttpGet]
        public string CourtList()
        {
            CitizenModel _citobj = new CitizenModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(_citobj.CourtList());
        }
        [HttpGet]
        public string CourtActList()
        {
            CitizenModel _citobj = new CitizenModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(_citobj.CourtActList());
        }
        [HttpGet]
        public string CourtCaseTypeList()
        {
            CitizenModel _citobj = new CitizenModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(_citobj.CourtCaseTypeList());
        }
        [HttpGet]
        public string CourtPurposeList()
        {
            CitizenModel _citobj = new CitizenModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(_citobj.CourtPurposeList());
        }
        [HttpGet]
        public string AdvocateTypeList()
        {
            CitizenModel _citobj = new CitizenModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(_citobj.AdvocateTypeList());
        }
        [HttpGet]
        public string CourtAdvocateList()
        {
            CitizenModel _citobj = new CitizenModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(_citobj.AdvocateTypeList());
        }

        [HttpPost]
        [ActionName("SaveOICDetails")]
        public JsonResult InsertOICDetails(OICDetails _Modal)
        {
            CitizenModel _citobj = new CitizenModel(new FileLogger());
            return Json(_citobj.AddOICDetails(_Modal));
        }

        [HttpPost]
        [ActionName("SaveEnclouserDetails")]
        public JsonResult InsertEnclouserDetails(EnclouserDetails _Modal)
        {
            List<Enclouseruploadlst> lstDoc = new List<Enclouseruploadlst>();

            foreach (string files in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[files];
                string fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                file.SaveAs(Path.Combine(Server.MapPath("~/Content/CourtDoc/"), fileName));
                Enclouseruploadlst obj = new Enclouseruploadlst();
                obj.EnclouserName = fileName;
                obj.EnclouserType = files.ToString();
                lstDoc.Add(obj);
            }
            _Modal.lstEnclouserupload = lstDoc;
            CitizenModel _citobj = new CitizenModel(new FileLogger());
            return Json(_citobj.AddEnclouserDetails(_Modal));

        }

        [HttpGet]
        public string EnclouserDocList()
        {
            CitizenModel _citobj = new CitizenModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(_citobj.EnclouserDocList());
        }

        [HttpPost]
        [ActionName("CaseRegDetailList")]
        public string GetAllCaseDetails(string Date, string Status)
        {

            string WhereCluase = " Where ";
            if (_LoggedInUser.SSOID != "")
                WhereCluase += "SSOID ='" + _LoggedInUser.SSOID + "'";
            if (Date.ToString() != "")
                WhereCluase += " and creg.DateOfAffilation='" + Date + "'";
            if (Status != "")
                WhereCluase += " and creg.CaseStatus=" + Status;
            if (_LoggedInUser.UserType == "D")
                WhereCluase += " and creg.CaseStatus!=0";

            CitizenModel _citobj = new CitizenModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(_citobj.GetAllCaseDetails(WhereCluase));
        }

        [HttpGet]
        [ActionName("GetCaseDetails")]
        public int GetCaseDetails(string CaseRegID)
        {
            int result = 0;
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            result = objCitizenModel.GetCaseRegistrationAndUpdate(CaseRegID);

            return result;
        }



        public ActionResult casedetailview()
        {
            return View();
        }

        [HttpPost]
        [ActionName("GetCaseDetailsByCaseId")]
        public string GetCaseDetailsByCaseId(string CaseRegId)
        {
            if (string.IsNullOrEmpty(CaseRegId))
                return "{}";

            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            DataSet Dset = new DataSet();
            DataTable dt = new DataTable();

            Dset = objCitizenModel.GetCaseDetailsByCaseId(CaseRegId);

            return JsonConvert.SerializeObject(Dset, Newtonsoft.Json.Formatting.Indented);

        }

        [HttpGet]
        public string CourtSectionList()
        {
            CitizenModel _citobj = new CitizenModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(_citobj.CourtSectionList());
        }
        [HttpGet]
        [ActionName("GetCourtdata")]
        public string InsertCourtdata(result objcase)
        {
            CitizenModel _citobj = new CitizenModel(new FileLogger());
            string result;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://10.70.230.61/gcmsscripts/ASBIViewRest.dll/datasnap/rest/TASBIViewREST/getiview");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"_parameters\": [{\"getiview\": {\"name\": \"ivcmd\",\"axpapp\": \"gcms\",\"username\":\"mobileuser\",\"password\":\"827ccb0eea8a706c4c34a16891f84e7b\",\"seed\":\"\",\"s\": \"\",\"pageno\": \"1\",\"pagesize\": \"100\",\"sqlpagination\": \"true\",\"params\": {\"mastid\":\"8\",\"court_code\":\"10005\"}}}]}";
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
                var temp = result;
                JsonCourt obj = new JsonCourt();

                obj = (JsonCourt)s.Deserialize(result, typeof(JsonCourt));
                //_Entity.lstSocietyWorkScope = Request.Form["lstSocietyWorkScope"].ToString().Equals("[]") ? new List<SocietyWorkScopeEntity>() : (List<SocietyWorkScopeEntity>)s.Deserialize(Request.Form["lstSocietyWorkScope"].ToString(), typeof(List<SocietyWorkScopeEntity>));
                //  result = _citobj.SaveCourtdata(obj.result[0].row).ToString();
                //foreach (var item in obj.result[0].row)
                //{
                //    httpWebRequest = (HttpWebRequest)WebRequest.Create("http://10.70.230.61/gcmsscripts/ASBIViewRest.dll/datasnap/rest/TASBIViewREST/getiview");
                //    httpWebRequest.ContentType = "application/json";
                //    httpWebRequest.Method = "POST";
                //    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                //    {
                //        string json = "{\"_parameters\": [{\"getiview\": {\"name\": \"ivcmd\",\"axpapp\": \"gcms\",\"username\":\"mobileuser\",\"password\":\"827ccb0eea8a706c4c34a16891f84e7b\",\"seed\":\"\",\"s\": \"\",\"pageno\": \"1\",\"pagesize\": \"0\",\"sqlpagination\": \"true\",\"params\": {\"mastid\":\"2\",\"distid\":\""+ item.valueid + "\"}}}]}";
                //        streamWriter.Write(json);
                //        streamWriter.Flush();
                //        streamWriter.Close();
                //    }
                //    httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                //    System.Web.Script.Serialization.JavaScriptSerializer ss = new System.Web.Script.Serialization.JavaScriptSerializer();
                //    using (var streamReaderr = new StreamReader(httpResponse.GetResponseStream()))
                //    {
                //        result = streamReaderr.ReadToEnd();
                //        var tempp = result;
                //        JsonCourt objj = new JsonCourt();

                //        objj = (JsonCourt)ss.Deserialize(result, typeof(JsonCourt));
                //        result = _citobj.SaveCourtdata(objj.result[0].row, item.valueid).ToString();
                //    }
                //}
            }

            return result;

        }

        #endregion


        #region Society Amendment
        // UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        [HttpPost]
        [ActionName("InsertByLaws")]
        public JsonResult InsertByLaws(SocietyAmendmentEntity SocietyAmendment)
        {
            try
            {
                if (!System.IO.Directory.Exists(Server.MapPath("../Content/SocietyProfiling/" + SocietyAmendment.SocietyDirectory + "/")))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("../Content/SocietyProfiling/" + SocietyAmendment.SocietyDirectory + "/"));
                }
                foreach (string files in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[files];
                    string fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);

                    file.SaveAs(Path.Combine(Server.MapPath("../Content/SocietyProfiling/" + SocietyAmendment.SocietyDirectory + "/"), fileName));
                    if (files.ToString() == "ResolutionFile")
                        SocietyAmendment.ResolutionFile = fileName;
                    else if (files.ToString() == "ByeLawsFile")
                        SocietyAmendment.ByeLawsFile = fileName;
                    else if (files.ToString() == "AGM1File")
                        SocietyAmendment.AGM1File = fileName;
                    else if (files.ToString() == "AGM2File")
                        SocietyAmendment.AGM2File = fileName;
                    else if (files.ToString() == "AGM3File")
                        SocietyAmendment.AGM3File = fileName;
                }

                ByeLawsMst1 ObjAGM_BODM_Mst = new ByeLawsMst1();
                ObjAGM_BODM_Mst.ByeLawsMstID = 0;
                ObjAGM_BODM_Mst.SocietyId = 0;
                ObjAGM_BODM_Mst.SocietyIdEnc = SocietyAmendment.SocietyIdEnc;
                ObjAGM_BODM_Mst.RegiYearId = SocietyAmendment.RegiYearId;
                ObjAGM_BODM_Mst.ByeLawsFile = SocietyAmendment.ByeLawsFile;
                ObjAGM_BODM_Mst.ResolutionNo = SocietyAmendment.ResolutionNo;
                ObjAGM_BODM_Mst.ResolutionFile = SocietyAmendment.ResolutionFile;
                ObjAGM_BODM_Mst.PaasedAGMDate = SocietyAmendment.PaasedAGMDate;
                ObjAGM_BODM_Mst.AGM1File = SocietyAmendment.AGM1File;
                ObjAGM_BODM_Mst.AGM2File = SocietyAmendment.AGM2File;
                ObjAGM_BODM_Mst.AGM3File = SocietyAmendment.AGM3File;

                ObjAGM_BODM_Mst.Remark = SocietyAmendment.Remark;
                ObjAGM_BODM_Mst.InsertDate = DateTime.Now;
                ObjAGM_BODM_Mst.InsertBy = _LoggedInUser.UserId;
                ObjAGM_BODM_Mst.InsertIP = COMMON_FUNCTION.GetIpAddress();
                CitizenModel _citobj = new CitizenModel(new FileLogger());
                return Json(_citobj.InsertByLaws(ObjAGM_BODM_Mst));
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen", FunctionName = " JsonResult InsertByLaws(SocietyAmendmentEntity SocietyAmendment)" }, ex);
                return Json("-1");
            }
        }

        [HttpPost]
        [ActionName("InsertSocietyNameChange1")]
        public JsonResult InsertSocietyNameChange1(SocietyAmendmentEntity SocietyAmendment)
        {
            try
            {
                if (!System.IO.Directory.Exists(Server.MapPath("../Content/SocietyProfiling/" + SocietyAmendment.SocietyDirectory + "/")))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("../Content/SocietyProfiling/" + SocietyAmendment.SocietyDirectory + "/"));
                }
                foreach (string files in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[files];
                    string fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);

                    file.SaveAs(Path.Combine(Server.MapPath("../Content/SocietyProfiling/" + SocietyAmendment.SocietyDirectory + "/"), fileName));
                    if (files.ToString() == "ResolutionFile")
                        SocietyAmendment.ResolutionFile = fileName;
                    else if (files.ToString() == "ByeLawsFile")
                        SocietyAmendment.ByeLawsFile = fileName;
                    else if (files.ToString() == "AGM1File")
                        SocietyAmendment.AGM1File = fileName;
                    else if (files.ToString() == "AGM2File")
                        SocietyAmendment.AGM2File = fileName;
                    else if (files.ToString() == "AGM3File")
                        SocietyAmendment.AGM3File = fileName;
                }

                ByeLawsMst1 ObjAGM_BODM_Mst = new ByeLawsMst1();
                ObjAGM_BODM_Mst.ByeLawsMstID = 0;
                ObjAGM_BODM_Mst.SocietyId = 0;
                ObjAGM_BODM_Mst.SocietyIdEnc = SocietyAmendment.SocietyIdEnc;
                ObjAGM_BODM_Mst.RegiYearId = SocietyAmendment.RegiYearId;
                ObjAGM_BODM_Mst.ByeLawsFile = SocietyAmendment.ByeLawsFile;
                ObjAGM_BODM_Mst.ResolutionNo = SocietyAmendment.ResolutionNo;
                ObjAGM_BODM_Mst.ResolutionFile = SocietyAmendment.ResolutionFile;
                ObjAGM_BODM_Mst.PaasedAGMDate = SocietyAmendment.PaasedAGMDate;
                ObjAGM_BODM_Mst.AGM1File = SocietyAmendment.AGM1File;
                ObjAGM_BODM_Mst.AGM2File = SocietyAmendment.AGM2File;
                ObjAGM_BODM_Mst.AGM3File = SocietyAmendment.AGM3File;
                ObjAGM_BODM_Mst.Name = SocietyAmendment.Name;
                ObjAGM_BODM_Mst.Remark = SocietyAmendment.Remark;
                ObjAGM_BODM_Mst.InsertDate = DateTime.Now;
                ObjAGM_BODM_Mst.InsertBy = _LoggedInUser.UserId;
                ObjAGM_BODM_Mst.InsertIP = COMMON_FUNCTION.GetIpAddress();
                CitizenModel _citobj = new CitizenModel(new FileLogger());
                return Json("");//_citobj.InsertSocietyNameChange(ObjAGM_BODM_Mst));
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen", FunctionName = " JsonResult InsertSocietyNameChange(SocietyAmendmentEntity SocietyAmendment)" }, ex);
                return Json("-1");
            }
        }

        [HttpPost]
        [ActionName("SaveSocietyMemberExcel")]
        public string SaveSocietyMemberExcel(SocietyAmendmentEntity SocietyAmendment)
        {
            string ReturnJSON = "";
            try
            {

                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                string filePath = Server.MapPath("../Content/SocietyProfiling/" + SocietyAmendment.SocietyDirectory + "/");
                string pathString = "~/Content/NewRegistration/";
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                string fileName = "";
                foreach (string files in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[files];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));

                    DataTable td = new DataTable();
                    var result = new SocietyController().ValidateExcel(ref td, Path.Combine(Server.MapPath(pathString), fileName));
                    if (result != "OK")
                    {
                        return CommonMessageFactory.GetJsonStringResponse("true", "ErrorInMemberdata@" + result);
                    }

                }
                ByeLawsMst1 ObjAGM_BODM_Mst = new ByeLawsMst1();
                ObjAGM_BODM_Mst.SocietyId = 0;
                ObjAGM_BODM_Mst.SocietyIdEnc = SocietyAmendment.SocietyIdEnc;
                ObjAGM_BODM_Mst.RegiYearId = SocietyAmendment.RegiYearId;
                ObjAGM_BODM_Mst.Name = fileName;
                ObjAGM_BODM_Mst.Remark = SocietyAmendment.Remark;
                ObjAGM_BODM_Mst.InsertDate = DateTime.Now;
                ObjAGM_BODM_Mst.InsertBy = _LoggedInUser.UserId;
                ObjAGM_BODM_Mst.InsertIP = COMMON_FUNCTION.GetIpAddress();
                CitizenModel _citobj = new CitizenModel(new FileLogger());
                //return _citobj.InsertChangeSocietyMemberExcel(ObjAGM_BODM_Mst).ToString();
                return CommonMessageFactory.GetJsonStringResponse("true", _citobj.InsertChangeSocietyMemberExcel(ObjAGM_BODM_Mst).ToString());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen", FunctionName = " string SaveSocietyMemberExcel(SocietyAmendmentEntity SocietyAmendment)" }, ex);
                ReturnJSON = "-1";
            }
            return CommonMessageFactory.GetJsonStringResponse("true", ReturnJSON);

        }



        [HttpGet]
        public string GetByLawsList(string SocietyIdEnc = "")
        {
            CitizenModel _citobj = new CitizenModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(_citobj.GetByLawsList(SocietyIdEnc));
        }

        [HttpGet]
        public JsonResult GetSocietyNameChangeList(string SocietyIdEnc = "", int AmendmentId = 0, int AmendmentType = 0)
        {
            CitizenModel _citobj = new CitizenModel(new FileLogger());

            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(_citobj.GetSocietyNameChangeList(SocietyIdEnc, AmendmentId, AmendmentType)) }, JsonRequestBehavior.AllowGet);


        }

        [HttpGet]
        public string GetChangeSocietyMemberExcelList(string SocietyIdEnc = "")
        {
            CitizenModel _citobj = new CitizenModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(_citobj.GetChangeSocietyMemberExcelList(SocietyIdEnc));
        }


        [HttpPost]
        [ActionName("InsertAGM_BODM")]
        public JsonResult InsertAGM_BODM(SocietyAmendmentEntity SocietyAmendment)
        {
            foreach (string files in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[files];
                string fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                file.SaveAs(Path.Combine(Server.MapPath("../Content/SocietyProfiling/" + SocietyAmendment.SocietyDirectory + "/"), fileName));
                if (files.ToString() == "MeetingPDF")
                    SocietyAmendment.MeetingPDF = fileName;
            }
            AGM_BODM_Mst1 ObjAGM_BODM_Mst = new AGM_BODM_Mst1();
            ObjAGM_BODM_Mst.AGM_BODM_MstID = 0;
            ObjAGM_BODM_Mst.SocietyId = 0;
            ObjAGM_BODM_Mst.SocietyIdEnc = SocietyAmendment.SocietyIdEnc;
            ObjAGM_BODM_Mst.ByeLawsDate = SocietyAmendment.ByeLawsDate;
            ObjAGM_BODM_Mst.MeetingDate = SocietyAmendment.MeetingDate;
            ObjAGM_BODM_Mst.MeetingPDF = SocietyAmendment.MeetingPDF;
            ObjAGM_BODM_Mst.Is_AGM_BODM = SocietyAmendment.Is_AGM_BODM;
            ObjAGM_BODM_Mst.Remark = SocietyAmendment.Remark;
            ObjAGM_BODM_Mst.InsertDate = DateTime.Now;
            ObjAGM_BODM_Mst.InsertBy = _LoggedInUser.UserId;
            ObjAGM_BODM_Mst.InsertIP = COMMON_FUNCTION.GetIpAddress();
            CitizenModel _citobj = new CitizenModel(new FileLogger());
            return Json(_citobj.InsertAGM_BODM(ObjAGM_BODM_Mst));
        }

        [HttpGet]
        public string GetAGM_BODMList(string SocietyIdEnc = "")
        {
            CitizenModel _citobj = new CitizenModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(_citobj.GetAGM_BODMList(SocietyIdEnc));
        }

        [HttpGet]
        public string GetBalanceSheetCurrent(string SocietyIdEnc = "")
        {
            CitizenModel _citobj = new CitizenModel(new FileLogger());
            //DataTable DTEXL = _citobj.GetBalanceSheet(SocietyIdEnc);
            return COMMON_FUNCTION.DataTableToJSONString(_citobj.GetBalanceSheet(SocietyIdEnc));
        }

        [HttpGet]
        public string GetBalanceSheetAllList(string SocietyIdEnc = "")
        {
            CitizenModel _citobj = new CitizenModel(new FileLogger());
            //DataTable DTEXL = _citobj.GetBalanceSheet(SocietyIdEnc);
            return COMMON_FUNCTION.DataTableToJSONString(_citobj.GetBalanceSheetAllList(SocietyIdEnc));
        }


        [HttpPost]
        [ActionName("InsertBalanceSheetByExcel")]
        public string InsertBalanceSheetByExcel(SocietyAmendmentEntity SocietyAmendmen)
        {
            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    SocietyAmendmen.ExcelBalanceSheetFile = fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("../Content/SocietyProfiling/" + SocietyAmendmen.SocietyDirectory + "/"), fileName));
                }
                catch { }
            }
            // _Model.CreatedIP = COMMON_FUNCTION.GetIpAddress();
            CitizenModel objElectionModel = new CitizenModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objElectionModel.InsertBalanceSheetByExcel(SocietyAmendmen.ExcelBalanceSheetFile, SocietyAmendmen.SocietyIdEnc, SocietyAmendmen.SocietyDirectory, SocietyAmendmen.RegiYearId);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
            //return CommonMessageFactory.Message(int.Parse(objSocietyModel.InsertSocityByExcel(_Model).ToString()), (Request.Cookies[SessionIdentity.Culture]).Value);
        }

        #endregion

        [HttpPost]
        [ActionName("CaseList")]
        public string GetAllCaseList(CaseRegistraion objCaseModel)
        {

            string WhereCluase = "Where ";
            if (objCaseModel.Act != 0)
                WhereCluase += "cact.ID=" + (objCaseModel.Act);
            if (objCaseModel.section != 0)
                WhereCluase += " and csect.ID=" + objCaseModel.section;
            if (objCaseModel.TypeOfCase != 0)
                WhereCluase += " and ctype.ID=" + objCaseModel.TypeOfCase;


            CitizenModel _citobj = new CitizenModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(_citobj.GetAllCaseList(WhereCluase));
            //return COMMON_FUNCTION.DataTableToJSONString(objCaseModel.GetAllCaseList(WhereCluase));
        }

        [HttpPost]
        [ActionName("GetCaseHistory")]
        public string GetCaseHistory(string CaseRegNo)
        {

            JsonCourt obj = new JsonCourt();
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            // result = objCitizenModel.GetCaseRegistrationAndUpdate(CaseRegNo);
            string Str = "";
            Str = "{\"_parameters\": [{\"getiview\": {\"name\": \"ivcpch\",\"axpapp\": \"gcms\",\"username\":\"mobileuser\",\"password\":\"827ccb0eea8a706c4c34a16891f84e7b\",\"seed\":\"\",\"s\": \"\",\"pageno\": \"1\",\"pagesize\": \"100\",\"sqlpagination\": \"true\",\"params\": {\"cno\":\"" + CaseRegNo + "\"}}}]}";

            string resultdata;
            string recordid;
            //var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://192.168.10.74/gcmsscripts/ASBIViewRest.dll/datasnap/rest/TASBIViewREST/getiview");
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://10.70.230.61/gcmsscripts/ASBIViewRest.dll/datasnap/rest/TASBIViewREST/getiview");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = Str;
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                resultdata = streamReader.ReadToEnd();
                var temp = resultdata;
                obj = (JsonCourt)s.Deserialize(resultdata, typeof(JsonCourt));
                //  resultdata = obj.result[0].row[0].case_no.ToString();
                //   recordid = obj.result[0].message[0].recordid.ToString();
            }

            //  return COMMON_FUNCTION.DataTableToJSONString(obj);
            return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
        }

        [HttpGet]
        public string CityTypeList()
        {
            CitizenModel _citobj = new CitizenModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(_citobj.CityTypeList());
        }

        [HttpGet]
        public ActionResult SocietyMemberRegistration()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ViewBag.usertype = _LoggedInUser.UserType;
            return View();
        }

        [HttpGet]
        public ActionResult SocietyMemberApproval()
        {
            // UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ViewBag.usertype = _LoggedInUser.UserType;
            return View();
        }

        [HttpPost]
        [ActionName("GetSocietyFileByFinancialYear")]
        public string GetSocietyFileByFinancialYear(string SocietyIdENC, int FYearID)
        {

            // _Model.CreatedIP = COMMON_FUNCTION.GetIpAddress();
            CitizenModel objElectionModel = new CitizenModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objElectionModel.GetSocietyFileByFinancialYear(SocietyIdENC, FYearID);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
            //return CommonMessageFactory.Message(int.Parse(objSocietyModel.InsertSocityByExcel(_Model).ToString()), (Request.Cookies[SessionIdentity.Culture]).Value);
        }


        [HttpGet]
        public ActionResult OtherService()
        {
            return View();
        }

        [HttpGet]
        [ActionName("GetSocietyByGP_ID")]
        public string GetSocietyByGP_ID(int GP_ID, int SocietyCatyegoryId = 0)
        {
            // _Model.CreatedIP = COMMON_FUNCTION.GetIpAddress();
            CitizenModel objElectionModel = new CitizenModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objElectionModel.GetSocietyByGP_ID(GP_ID, SocietyCatyegoryId);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
            //return CommonMessageFactory.Message(int.Parse(objSocietyModel.InsertSocityByExcel(_Model).ToString()), (Request.Cookies[SessionIdentity.Culture]).Value);
        }

        [HttpPost]
        public string UserSocietyDataForAuditRPT()
        {
            CitizenModel objMenuActionModel = new CitizenModel(new FileLogger());

            DataTable DT = objMenuActionModel.SocietyListForAuditRPT(Convert.ToInt32(_LoggedInUser.UserId), _LoggedInUser.SSOID);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }


        public ActionResult MemberApplicationView(string ApplicationIdEnc = "")
        {
            return View();
        }

        [HttpGet]
        [ActionName("CreateDir")]
        public void GetAllRegno()
        {
            string flag = "0";
            string fileName = string.Empty;
            string pathString = "~/Content/SocietyProfiling/";

            CitizenModel objElectionModel = new CitizenModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objElectionModel.GetAllRegno();
            foreach (DataRow Dr in DT.Rows)
            {
                pathString = "~/Content/SocietyProfiling/" + Regex.Replace(Dr["RegistrationNo"].ToString(), "[^a-zA-Z0-9_]+", "");
                if (!System.IO.File.Exists(pathString))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(pathString));
                }
            }
        }

        [HttpGet]
        [ActionName("UpdatePhotos")]
        public void UpdatePhotos()
        {

            List<SocietyAdministrativeEntity> ObjLst = new List<SocietyAdministrativeEntity>();
            CitizenModel objElectionModel = new CitizenModel(new DBLogger());
            DataTable DT = new DataTable();

            DT = objElectionModel.GetAllPhoto();
            string pathString = "~/Content/SocietyProfiling/";
            foreach (DataRow Dr in DT.Rows)
            {
                SocietyAdministrativeEntity OBJ = new SocietyAdministrativeEntity();

                string fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper();
                string Photostr = Dr["PhotoOld"].ToString();
                pathString = pathString + Dr["RegistrationNo"].ToString() + "/";
                pathString = "~/Content/SocietyProfiling/" + Regex.Replace(Dr["RegistrationNo"].ToString(), "[^a-zA-Z0-9_]+", "");
                //if (Directory.Exists(Server.MapPath(pathString)))
                //{

                //    byte[] imageBytes = Convert.FromBase64String(Photostr);
                //    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

                //    fileName = COMMON_FUNCTION.GetUniqueFileName("Member_" + fileName, System.Web.HttpContext.Current.Server.MapPath("../Content/SocietyProfiling/" + Dr["RegistrationNo"].ToString()) + "/", ".jpg").ToLower();
                //    fileName = fileName.ToUpper();
                //    FileStream fs = new FileStream(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(pathString), fileName + ".jpg"), FileMode.CreateNew, FileAccess.Write);
                //    ms.WriteTo(fs);
                //    fs.Close();
                //    fs.Dispose();
                //    ms.Flush();
                string Aadh = Dr["Aadh"].ToString();

                if (Aadh.Length == 12)
                    OBJ.AADHAARNo = COMMON_FUNCTION.Encrypt(Aadh);

                //    OBJ.Photo = fileName + ".jpg";
                //    OBJ.AdministrativeId = Dr["AdministrativeId"].ToString();
                //    ObjLst.Add(OBJ);
                //}
            }

            objElectionModel.UpdatePhotos(ObjLst);
        }

        [HttpGet]
        [ActionName("UpdatePhotosforPartial")]
        public void UpdatePhotosforPartial()
        {
            List<SocietyAdministrativeEntity> ObjLst = new List<SocietyAdministrativeEntity>();
            CitizenModel objElectionModel = new CitizenModel(new DBLogger());
            DataTable DT = new DataTable();

            DT = objElectionModel.GetAllPhoto();
            string pathString = "~/Content/NewRegistration/";
            foreach (DataRow Dr in DT.Rows)
            {
                SocietyAdministrativeEntity OBJ = new SocietyAdministrativeEntity();

                string fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper();
                string Photostr = Dr["PhotoOld"].ToString();
                pathString = "~/Content/NewRegistration/";
                if (Dr["RegistrationNo"].ToString().Contains("COOP"))
                    pathString = "~/Content/SocietyProfiling/" + Dr["RegistrationNo"].ToString();

                if (System.IO.Directory.Exists(Server.MapPath(pathString)))
                {

                    byte[] imageBytes = Convert.FromBase64String(Photostr);
                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

                    if (Dr["RegistrationNo"].ToString().Contains("COOP"))
                        fileName = COMMON_FUNCTION.GetUniqueFileName("Member_" + fileName, System.Web.HttpContext.Current.Server.MapPath("../Content/SocietyProfiling/" + Dr["RegistrationNo"].ToString()) + "/", ".jpg").ToLower();
                    else
                        fileName = COMMON_FUNCTION.GetUniqueFileName("Member_" + fileName, System.Web.HttpContext.Current.Server.MapPath("../Content/NewRegistration/") + "/", ".jpg").ToLower();

                    fileName = fileName.ToUpper();
                    FileStream fs = new FileStream(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(pathString), fileName + ".jpg"), FileMode.CreateNew, FileAccess.Write);
                    ms.WriteTo(fs);
                    fs.Close();
                    fs.Dispose();
                    ms.Flush();
                    string Aadh = Dr["Aadh"].ToString();

                    if (Aadh.Length == 12)
                        OBJ.AADHAARNo = COMMON_FUNCTION.Encrypt(Aadh);

                    OBJ.Photo = fileName;
                    OBJ.AdministrativeId = Dr["AdministrativeId"].ToString();
                    ObjLst.Add(OBJ);
                }
            }

            objElectionModel.UpdatePhotos(ObjLst);
        }


        [HttpGet]
        [ActionName("UpdateDoc")]
        public void UpdateDoc()
        {

            List<SocietyRegistrationEntity> ObjLst = new List<SocietyRegistrationEntity>();
            CitizenModel objModel = new CitizenModel(new DBLogger());
            DataTable DT = new DataTable();

            DT = objModel.GetAllDoc();
            string pathString = "~/Content/SocietyProfiling/";
            string pathStringESigned = "~/Content/ESigned/";
            foreach (DataRow Dr in DT.Rows)
            {
                SocietyRegistrationEntity OBJ = new SocietyRegistrationEntity();

                // string RegistrationCertificatefileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper();
                string RegistrationCertificateFilestr = Dr["RegistrationCertificateFile"].ToString();
                string BylawsFilestr = Dr["BylawsFile"].ToString();
                string Certificatestr = Dr["Certificate"].ToString();
                string RulesRegulationFilestr = Dr["RulesRegulationFile"].ToString();
                string MemberListFileNamestr = Dr["MemberListFileName"].ToString();
                string BalanceSheetExcelFile = Dr["BalanceSheetExcelFile"].ToString();
                pathString = pathString + Dr["RegistrationNo"].ToString() + "/";
                pathString = "~/Content/SocietyProfiling/" + Regex.Replace(Dr["RegistrationNo"].ToString(), "[^a-zA-Z0-9_]+", "") + "";
                if (System.IO.Directory.Exists(Server.MapPath(pathString)))
                {
                    if (RegistrationCertificateFilestr != null && RegistrationCertificateFilestr != "")
                    {
                        System.IO.File.Copy(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("E:/Coop_Old_Data/UPLOADS/"), RegistrationCertificateFilestr), Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(pathString), RegistrationCertificateFilestr));
                        System.IO.File.Copy(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("E:/Coop_Old_Data/CERTIFICATES/"), RegistrationCertificateFilestr), Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(pathStringESigned), RegistrationCertificateFilestr));
                    }
                    else
                    {
                        OBJ.RegistrationCertificateFile = "";
                        OBJ.Certificate = "";
                    }
                    //if (Certificatestr != null && Certificatestr != "")
                    //{
                    //    System.IO.File.Copy(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/SocietyProfiling/"), Certificatestr), Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(pathString), Certificatestr));
                    //}
                    //else
                    //{
                    //    OBJ.Certificate = "";
                    //}
                    if (BylawsFilestr != null && BylawsFilestr != "")
                    {
                        System.IO.File.Copy(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("E:/Coop_Old_Data/UPLOADS/"), BylawsFilestr), Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(pathString), BylawsFilestr));
                        System.IO.File.Copy(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("E:/Coop_Old_Data/OBJECTIVECERTIFICATE/"), BylawsFilestr), Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(pathStringESigned), BylawsFilestr));
                    }
                    else
                    {
                        OBJ.ByeLawsSocietyFile = "";
                    }
                    if (RulesRegulationFilestr != null && RulesRegulationFilestr != "")
                    {
                        System.IO.File.Copy(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("E:/Coop_Old_Data/UPLOADS/"), RulesRegulationFilestr), Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(pathString), RulesRegulationFilestr));
                        System.IO.File.Copy(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("E:/Coop_Old_Data/RULECERTIFICATE/"), BylawsFilestr), Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(pathStringESigned), RulesRegulationFilestr));
                    }
                    else
                    {
                        OBJ.RulesAndRegulationFile = "";
                    }
                    if (MemberListFileNamestr != null && MemberListFileNamestr != "")
                    {
                        System.IO.File.Copy(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("E:/Coop_Old_Data/UPLOADS/"), MemberListFileNamestr), Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(pathString), MemberListFileNamestr));
                    }
                    else
                    {
                        OBJ.MemberListExcelFile = "";
                    }
                    if (BalanceSheetExcelFile != null && BalanceSheetExcelFile != "")
                    {
                        System.IO.File.Copy(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("E:/Coop_Old_Data/UPLOADS/"), BalanceSheetExcelFile), Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(pathString), BalanceSheetExcelFile));
                    }
                    else
                    {
                        OBJ.BalanceSheetFile = "";
                    }

                    OBJ.AdministrativeId = Dr["SocietyID"].ToString();
                    ObjLst.Add(OBJ);
                }
            }

            // objModel.UpdateDoc(ObjLst);
        }
        [HttpGet]
        [ActionName("GetActsOfUser")]
        public string GetActsOfUser()
        {
            return _LoggedInUser.ActId;
        }

        [HttpGet]
        [ActionName("GetUserListtoVerify")]
        public string GetUserListtoVerifys(string SocietyIdEnc)
        {
            // _AadharNo = "432534469557";
            DataTable dt = null;

            //string WhereClause = " and Societies.RegistrationNo='" + SocietyRegistrationNo + "'";
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            dt = objCitizenModel.GetCEODetails(SocietyIdEnc);
            List<object> collection = new List<object>();
            string AadharNo = "";
            foreach (DataRow dr in dt.Rows)
            {
                string _AADHARNo = string.Empty, _maskedAADHARNo = string.Empty, _NativeAADHARNo = string.Empty, _maskedNativeAADHARNo = string.Empty;
                // "XXXXXXXX" + _loggedInUserAADHARNo.Substring(8, 4);
                //COMMON_FUNCTION.Decrypt(dr["AadharNo"].ToString());
                AadharNo = "";
                string Aadh = "";
                if (string.IsNullOrWhiteSpace(dr["AadharNo"].ToString()) && dr["Aadh"].ToString().Length == 12)
                {
                    dr["AadharNo"] = COMMON_FUNCTION.Encrypt(dr["Aadh"].ToString());
                    Aadh = dr["Aadh"].ToString();
                }

                if (!string.IsNullOrWhiteSpace(dr["AadharNo"].ToString()))
                {
                    AadharNo = COMMON_FUNCTION.Decrypt(dr["AadharNo"].ToString());
                    Aadh = AadharNo;
                    AadharNo = "XXXXXXXX" + AadharNo.Substring(8, 4);

                }
                dr["Aadh"] = Aadh;
                dr["AadharNo"] = AadharNo;
            }
            return COMMON_FUNCTION.DataTableToJSONString(dt);

        }

        [HttpPost]
        public int SocietyMappingInsert(SocietyMappingEntity model, int Flag = 0, string FileName = "")
        {
            // model.SocietyId = model.SocietyId;
            string pathString = model.Photo;
            string SourceFilePath = "";
            string TargetFilePath = "";
            if (_LoggedInUser.UserType.ToUpper() == "K")
                model.RequestBySSOId = "";
            else
                model.RequestBySSOId = _LoggedInUser.SSOID;
            model.UserType = _LoggedInUser.UserType;
            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    string fileDirectory = Server.MapPath("~/Content/ScoietyAmenmentDoc");

                    if (!System.IO.Directory.Exists(fileDirectory))
                    {
                        System.IO.Directory.CreateDirectory(fileDirectory);
                    }
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    model.FileName = fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("../Content/ScoietyAmenmentDoc/"), fileName));
                }
                catch { }
            }
            model.Photo = model.Photo ?? "";
            var PhPath = model.Photo.Split('/');
            model.Photo = PhPath[PhPath.Length - 1];

            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            int Result = objCitizenModel.SocietyMappingInsert(model, Flag);

            if (Result > 0) // Send SMS to the requester [CEO as Citizen]
            {
                if (!System.IO.Directory.Exists(Server.MapPath(TargetFilePath)))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(TargetFilePath));
                }

                TargetFilePath = TargetFilePath + "/" + model.Photo;

                if (System.IO.File.Exists(Server.MapPath(pathString)) && !System.IO.File.Exists(Server.MapPath(TargetFilePath)))
                    System.IO.File.Copy(Server.MapPath(pathString), Server.MapPath(TargetFilePath));
                DataTable dt = (new MasterModel(new DBLogger())).GetMessageBodyByActivityKey("SEND_MESSAGE_ON_SOCIETY_MAPPING_REQUEST");
                if (dt != null && dt.Rows.Count > 0)
                {
                    string msg = Convert.ToString(dt.Rows[0]["SMS_MSG"]);
                    msg = msg.Replace("##Name##", model.Name);
                    SMS.SendSMS(model.MobileNo, msg);
                }
            }
            return Result;
        }

        [HttpPost]
        public int InsertAmendmentSenction4(SocietyMappingEntity model, int Flag = 0, string FileName = "")
        {
            // model.SocietyId = model.SocietyId;
            string pathString = model.Photo;
            string SourceFilePath = "";
            string TargetFilePath = "";
            if (_LoggedInUser.UserType.ToUpper() == "K")
                model.RequestBySSOId = "";
            else
                model.RequestBySSOId = _LoggedInUser.SSOID;
            model.UserType = _LoggedInUser.UserType;
            string fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    string fileDirectory = Server.MapPath("~/Content/ScoietyAmenmentDoc");

                    if (!System.IO.Directory.Exists(fileDirectory))
                    {
                        System.IO.Directory.CreateDirectory(fileDirectory);
                    }
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    model.FileName = fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("../Content/ScoietyAmenmentDoc/"), fileName));
                }
                catch { }
            }
            model.Photo = model.Photo ?? "";
            var PhPath = model.Photo.Split('/');
            model.Photo = PhPath[PhPath.Length - 1];

            Society objCitizenModel = new Society();
            int i;
            long l;
            decimal d;
            bool checkIsInNumber =
                int.TryParse(model.AADHARNo, out i) ||
                long.TryParse(model.AADHARNo, out l) ||
                decimal.TryParse(model.AADHARNo, out d);
            if (checkIsInNumber)
            {
                model.AADHARNo = COMMON_FUNCTION.Encrypt(model.AADHARNo);
                model.AADHARNoEnc = COMMON_FUNCTION.Encrypt(model.AADHARNo);
            }
            else if (string.IsNullOrEmpty(model.AADHARNoEnc) || model.AADHARNoEnc == "null" || model.AADHARNoEnc == "undefined")
            {
                if (model.AADHARNo?.Length > 0)
                    model.AADHARNoEnc = COMMON_FUNCTION.Encrypt(model.AADHARNo);
            }

            int Result = objCitizenModel.InsertAmendmentSenction4(Convert.ToInt32(model.SocietyId), model.Name, model.MobileNo, model.UserType, "", model.AADHARNoEnc, model.DOB, model.Gender, model.Photo, model.CategoryId.ToString(), model.PostName, "ELECTEDMEMBERS", model.Address, model.Occupation, model.FatherName, _LoggedInUser.SSOID);

            if (Result > 0) // Send SMS to the requester [CEO as Citizen]
            {
                if (!System.IO.Directory.Exists(Server.MapPath(TargetFilePath)))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(TargetFilePath));
                }

                TargetFilePath = TargetFilePath + "/" + model.Photo;

                if (System.IO.File.Exists(Server.MapPath(pathString)) && !System.IO.File.Exists(Server.MapPath(TargetFilePath)))
                    System.IO.File.Copy(Server.MapPath(pathString), Server.MapPath(TargetFilePath));
                DataTable dt = (new MasterModel(new DBLogger())).GetMessageBodyByActivityKey("SEND_MESSAGE_ON_SOCIETY_MAPPING_REQUEST");
                if (dt != null && dt.Rows.Count > 0)
                {
                    string msg = Convert.ToString(dt.Rows[0]["SMS_MSG"]);
                    msg = msg.Replace("##Name##", model.Name);
                    SMS.SendSMS(model.MobileNo, msg);
                }
            }
            return Result;
        }



        [HttpGet]
        public ActionResult CCSComplaint()
        {
            ViewBag.UserType = _LoggedInUser.UserType;
            return View();
        }



        [HttpPost]
        public string GetCreditCooperativeSocieties(List<Params> paramlist)
        {
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            // string whereclause = " and (Societies.RegistrationNo like 'MSCS%' or SocietyRegOfficeDetails.DISTRICT_ID=" + paramlist[0].Value + " ) ";
            return COMMON_FUNCTION.DataTableToJSONString(objCitizenModel.GetCreditCooperativeSocietiesGreviyans(paramlist[0].Value, _LoggedInUser.DistrictId));
        }

        [HttpPost]
        public string CCSComplaintDocumentUploadedList()
        {
            string fileName = string.Empty, connString = string.Empty;
            List<CCSComplaintRegisterDetailModel> collection = new List<CCSComplaintRegisterDetailModel>();
            MasterModel objMasterModel = new MasterModel(new DBLogger());
            DataTable dtDocType = objMasterModel.CCSComplaintDocumentTypeList();


            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    file.SaveAs(Path.Combine(Server.MapPath("~/Content/CCSComplaintFiles"), fileName));

                    string strFileType = System.IO.Path.GetExtension(fileName).ToLower();

                    string path = string.Concat(System.Web.HttpContext.Current.Server.MapPath("~/Content/CCSComplaintFiles/" + fileName));

                    if (strFileType.Trim() == ".xls")
                    {
                        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    else if (strFileType.Trim() == ".xlsx")
                    {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    OleDbConnection conn = new OleDbConnection(connString);

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    foreach (DataRow SheetRow in dtSheet.Rows)
                    {
                        string query = "SELECT * FROM [" + SheetRow.ItemArray[2] + "]";
                        OleDbCommand cmd2 = new OleDbCommand(query, conn);
                        OleDbDataAdapter da = new OleDbDataAdapter(cmd2);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        conn.Close();

                        try
                        {
                            if (dt == null || dt.Rows.Count < 1)
                                throw new Exception("Data Table is null since Excel has no record");

                            bool IsAllColContains = false;

                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Columns.Contains("Document Type") &&
                                    dt.Columns.Contains("Account No") &&
                                    dt.Columns.Contains("Paid Amount") &&
                                    dt.Columns.Contains("Issue Date") &&
                                    dt.Columns.Contains("Maturity Amount") &&
                                    dt.Columns.Contains("Maturity Date"))
                                {
                                    IsAllColContains = true;
                                }
                            }

                            if (!IsAllColContains)
                                throw new Exception("Excel has not all relevent column i.e. Document Type,Account No,Paid Amount,Issue Date,Maturity Amount,Maturity Date");


                            foreach (DataRow Dr in dt.Rows)
                            {
                                var FileTypeId = dtDocType.AsEnumerable()
                                    .Where(p => p.Field<string>("TypeNameEng").Trim() == Dr["Document Type"].ToString().Trim())
                                    .Select(q => new { Id = q.Field<int>("CCSComplaintDocTypeId") }).ToList().FirstOrDefault();

                                CCSComplaintRegisterDetailModel temp = new CCSComplaintRegisterDetailModel();
                                temp.FileName = Dr["Document Type"].ToString().Trim();
                                temp.AccountNo = Dr["Account No"].ToString().Trim();
                                temp.PaidAmount = Convert.ToDecimal(Dr["Paid Amount"].ToString().Trim());
                                temp.PayDate = Dr["Issue Date"].ToString().Trim();
                                temp.MaturityAmount = Convert.ToDecimal(Dr["Maturity Amount"].ToString().Trim());
                                temp.MaturityDate = Dr["Maturity Date"].ToString().Trim();
                                temp.FileTypeId = FileTypeId == null || Convert.ToInt32(FileTypeId.Id) == 0 ? 6 : Convert.ToInt32(FileTypeId.Id);
                                collection.Add(temp);
                            }

                            Session["CCSDocumentUploaded"] = collection;

                        }
                        catch (Exception ex)
                        {
                            DBLogger objDBLogger = new DBLogger();
                            objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citozen", FunctionName = "public string CCSComplaintDocumentUploadedList()" }, ex);
                        }
                    }

                }
                catch (Exception ex)
                {

                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citozen", FunctionName = "public string CCSComplaintDocumentUploadedList()" }, ex);
                }

            }
            return new JavaScriptSerializer().Serialize(collection);
        }

        [HttpPost]
        public string SaveCCSComplaint(CCSComplaintRegisterModel model)
        {
            if ((UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin] == null)
                return "LOGOUT";


            HttpCookie cookie = Request.Cookies[SessionIdentity.Culture];
            model.CreateIP = COMMON_FUNCTION.GetIpAddress();
            EmitraController objEC = new EmitraController();
            string fileName = string.Empty;
            DataTable DT = null;
            int KioskResult = -1;
            string Flags = "";
            //model.FileDetails = new List<CCSComplaintRegisterDetailModel>();
            model.SSOId = _LoggedInUser.SSOID;
            model.UserType = Convert.ToString(_LoggedInUser.UserType);
            // int index = 0;

            if (Request.Files != null && Request.Files.Count > 0)
            {
                string folderpath = "../Content/CCSComplaintFiles";
                if (!System.IO.Directory.Exists(Server.MapPath(folderpath)))
                    System.IO.Directory.CreateDirectory(Server.MapPath(folderpath));

                try
                {
                    foreach (string files in Request.Files)
                    {
                        HttpPostedFileBase tempfile = Request.Files[files];
                        fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(tempfile.FileName);
                        tempfile.SaveAs(Path.Combine(Server.MapPath("~/Content/CCSComplaintFiles/"), fileName));

                        if (files.ToString().ToLower() == "remittancepassbook")
                        {
                            model.PassbookCrossCheque = fileName;
                        }
                        else if (files.ToString().ToLower() == "fdaadhaar")
                        {
                            model.AttestedAadhaar = fileName;
                        }
                        else if (files.ToString().ToLower() == "fdaffidavit")
                        {
                            model.AffidavitRs = fileName;
                        }
                        else if (files.ToString().ToLower() == "fdinvestmentsfdr")
                        {
                            model.InvestmentFDR = fileName;
                        }
                        else if (files.ToString().ToLower() == "fdpancard")
                        {
                            model.AttestedPan = fileName;
                        }
                        else
                        {
                            string index = files.ToString().ToLower().Split('_')[1];
                            model.FileDetails[Convert.ToInt32(index)].FilePath = fileName;

                            //model.FileDetails[index].FileTypeId = Convert.ToInt32(model.FileDetails[index].FileTypeId);
                            //model.FileDetails[index].FilePath = fileName;
                            //index++;
                        }

                    }
                }
                catch (Exception ex)
                {
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen", FunctionName = "Function for Save Files of CCS Details" }, ex);
                }
            }
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            int Tranid = objCitizenModel.SaveCCSComplaint(model);
            if (Tranid > 0)
            {
                EmitraModelFactory ObjEmitraModel = new EmitraModelFactory(new DBLogger());
                EmitraController OBJ_EmitraController = new EmitraController();
                if (_LoggedInUser.UserType == "K")
                {
                    //SocietyId = DT.Rows[0]["SocietyId"].ToString();

                    EmitraModelFactory objEmitraModel = new EmitraModelFactory(new DBLogger());
                    DT = objEmitraModel.CCSComplaintRegisterData(Tranid);

                    if (System.Web.HttpContext.Current.Session["ARequest"] != null && _LoggedInUser.KIOSKCODE.Length > 0 && DT != null && DT.Rows.Count > 0)
                    {
                        KioskResult = ObjEmitraModel.MakePaymentForAll("0", DT.Rows[0]["ComplainerName"].ToString(), "0", DT.Rows[0]["ReceiptNo"].ToString(), Tranid.ToString(), _LoggedInUser.KIOSKCODE, _LoggedInUser.SSOID);

                        if (KioskResult > 0)
                        {
                            Flags = "Sucess";

                            Flags = objCitizenModel.ActiveCCSComplaintRegister(KioskResult, model.MobileNo, model.ComplainerName, model.ComplainerFatherName, DT.Rows[0]["ReceiptNo"].ToString());
                            //MOVE TEMP DATA TO MAIN
                            //ApplicationFlowModel AFM = new ApplicationFlowModel();
                            //AFM.AddApplicationFlow(_Entity.SocietyIDEnc, 24, "BANK ACCOUNT DETAILS UPLOADED", 0);
                            DResponse resp = JsonConvert.DeserializeObject<DResponse>(Flags);

                            DataTable Response = objCitizenModel.GetCCSComplaintRegisterForCitizen(Tranid);

                            if (Response != null && Response.Rows.Count > 0)
                            {
                                string MobileNo = Response.Rows[0]["MobileNo"].ToString();

                                if (MobileNo.Length == 10)
                                {
                                    EsanchaarController EC = new EsanchaarController();
                                    string IsOk = EC.SendSocietySMS("Dear " + Convert.ToString(Response.Rows[0]["ComplainerName"]) + " S/O " + Convert.ToString(Response.Rows[0]["ComplainerFatherName"]) + ", Your complaint has been registered successfully. Your complaint number is " + Convert.ToString(Response.Rows[0]["ReceiptNo"]) + ".", MobileNo, "HIN");
                                }
                            }

                            Flags = "<div class=\"form-group citizen-dash-head\"><div class=\"table-responsive\"><table class=\"table devtable\">" +
                            "<thead><tr><th class=\"ng-binding\" colspan=\"4\">Transaction Status</th>" +
                            //"<th class=\"ng-binding\">Column 2</th><th class=\"ng-binding\">Column 3</th> <th>Column 4</th>" +
                            "</tr></thead><tbody>" +
                            "<tr><td class=\"ng-binding\">TRANSACTIONID</td><td class=\"ng-binding\">" + resp.TRANSACTIONID + "</td><td class=\"ng-binding\">RECEIPTNO</td><td class=\"ng-binding\">" + resp.RECEIPTNO + "</td></tr>" +
                            "<tr><td class=\"ng-binding\">PAIDAMOUNT</td><td class=\"ng-binding\">" + resp.TRANSAMT + "</td><td class=\"ng-binding\">Date</td><td class=\"ng-binding\">" + resp.EMITRATIMESTAMP.Substring(0, 8) + "</td></tr>" +
                            "<tr><td class=\"ng-binding\">RESPONSEMESSAGE</td><td class=\"ng-binding\">" + resp.MSG + "</td><td class=\"ng-binding\"></td><td class=\"ng-binding\"></td></tr>" +
                            "<tr><td class=\"ng-binding\">Complaint No </td><td class=\"ng-binding\">" + Convert.ToString(Response.Rows[0]["ReceiptNo"]) + "</td><td class=\"ng-binding\"></td><td class=\"ng-binding\"></td></tr>" +
                            "</tbody></table></div></div>";
                        }
                        else
                        {
                            // Flags = objSocietyModel.GetEmitraBankStatmentLastTran(int.Parse(Flags));
                            DResponse resp = JsonConvert.DeserializeObject<DResponse>(Flags);
                            Flags = "<div class=\"form-group citizen-dash-head\"><div class=\"table-responsive\"><table class=\"table devtable\">" +
                            "<thead><tr><th class=\"ng-binding\" colspan=\"4\">Transaction Status</th>" +
                            //"<th class=\"ng-binding\">Column 2</th><th class=\"ng-binding\">Column 3</th> <th>Column 4</th>" +
                            "</tr></thead><tbody>" +
                            "<tr><td class=\"ng-binding\">TRANSACTIONID</td><td class=\"ng-binding\">" + resp.TRANSACTIONID + "</td><td class=\"ng-binding\">RECEIPTNO</td><td class=\"ng-binding\">" + resp.RECEIPTNO + "</td></tr>" +
                            "<tr><td class=\"ng-binding\">PAIDAMOUNT</td><td class=\"ng-binding\">" + resp.TRANSAMT + "</td><td class=\"ng-binding\">Date</td><td class=\"ng-binding\">" + resp.EMITRATIMESTAMP.Substring(0, 8) + "</td></tr>" +
                            "<tr><td class=\"ng-binding\">RESPONSEMESSAGE</td><td class=\"ng-binding\">" + resp.MSG + "</td><td class=\"ng-binding\"></td><td class=\"ng-binding\"></td></tr>" +

                            "</tbody></table></div></div>";
                        }

                        //SocietyId += COMMON_FUNCTION.Encrypt(result.ToString());
                    }
                }
                else if (_LoggedInUser.UserType.ToUpper() == "C" || _LoggedInUser.UserType.ToUpper() == "D")
                {
                    DataTable Response = objCitizenModel.GetCCSComplaintRegisterForCitizen(Tranid);

                    if (Response != null && Response.Rows.Count > 0)
                    {
                        string MobileNo = Response.Rows[0]["MobileNo"].ToString();

                        if (MobileNo.Length == 10)
                        {
                            EsanchaarController EC = new EsanchaarController();
                            string IsOk = EC.SendSocietySMS("Dear " + Convert.ToString(Response.Rows[0]["ComplainerName"]) + " S/O " + Convert.ToString(Response.Rows[0]["ComplainerFatherName"]) + ", Your complaint has been registered successfully. Your complaint number is " + Convert.ToString(Response.Rows[0]["ReceiptNo"]) + ".", MobileNo, "HIN");
                        }
                    }

                    string language = "hi-in";// (!string.IsNullOrEmpty(cookie.Value) ? cookie.Value : string.Empty).ToLower();

                    Flags = "<div class=\"form-group citizen-dash-head\">" +
                            "<div class=\"table-responsive\">" +
                            "<table class=\"table devtable\">" +
                            "<thead><tr><th class=\"ng-binding\">" + (string.Equals(language, "en-us") ? "Transaction Status" : "ट्रांजेक्शन स्थिति") + "</th></tr></thead>" +
                            "<tbody><tr><td class=\"ng-binding\">" + (string.Equals(language, "en-us") ? "Complaint has been successfully filed against Credit Co-operative Society. Your complaint number is " + Convert.ToString(Response.Rows[0]["ReceiptNo"]) + "." : "क्रेडिट को-ऑपरेटिव सोसायटी के खिलाफ शिकायत सफलतापूर्वक दर्ज कर दी गई है | आपकी  शिकायत संख्या " + Convert.ToString(Response.Rows[0]["ReceiptNo"]) + " है") + " </td></tr></tbody>" +
                            "</table>" +
                            "</div>" +
                            "</div>";
                }

            }

            return Flags;
        }


        [HttpGet]
        public ActionResult CCSComplaintEditView()
        {
            return View();
        }

        [HttpPost]
        public string ValidateOTPAndGetComplainerList(string otp, string txn, string _AadharNo)
        {
            var response = AADHARSerice.VeryifyOTPsentOnAADHAR(otp, txn, _AadharNo);
            dynamic ResponseData = new JavaScriptSerializer().Deserialize<dynamic>(response);
            bool IsVerified = Convert.ToBoolean(ResponseData["Result"]);

            if (IsVerified)
            {
                CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
                return COMMON_FUNCTION.DataTableToJSONString(objCitizenModel.GetCCSDetails(SearchType: 1, AADHAREnc: _AadharNo));
            }
            else
            {
                return "[]";
            }
        }


        [HttpPost]
        public JsonResult GetComplaintDetailsByComplaintId(string ComplaintIdEnc)
        {
            CCSComplaintRegisterModel objCCSComplaintRegisterModel = new CCSComplaintRegisterModel();
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            DataTable dt = objCitizenModel.GetCCSDetails(SearchType: 3, ComplaintIdEnc: ComplaintIdEnc);

            if (dt != null && dt.Rows.Count > 0)
            {
                try
                {
                    DataRow dr = dt.Rows[0];

                    objCCSComplaintRegisterModel.ComplaintIdEnc = ComplaintIdEnc;

                    objCCSComplaintRegisterModel.ComplainerName = dr.Field<string>("ComplainerName");
                    objCCSComplaintRegisterModel.ComplainerFatherName = dr.Field<string>("ComplainerFatherName");
                    objCCSComplaintRegisterModel.Address = dr.Field<string>("Address");
                    objCCSComplaintRegisterModel.DistrictId = dr.Field<int>("DistrictId");
                    objCCSComplaintRegisterModel.MobileNo = dr.Field<string>("MobileNo");
                    objCCSComplaintRegisterModel.Email = dr.Field<string>("Email");
                    objCCSComplaintRegisterModel.CCSId = dr.Field<int>("CCSId");
                    objCCSComplaintRegisterModel.BranchName = dr.Field<string>("BranchName");
                    objCCSComplaintRegisterModel.AccountNo = dr.Field<string>("AccountNo");
                    objCCSComplaintRegisterModel.IFSCCode = dr.Field<string>("IFSCCode");
                    objCCSComplaintRegisterModel.AmountDeposited = dr.Field<decimal>("AmountDeposited");
                    objCCSComplaintRegisterModel.MembershipId = dr.Field<string>("MembershipId");
                    objCCSComplaintRegisterModel.Remarks = dr.Field<string>("Remarks");
                    objCCSComplaintRegisterModel.AccountNoInSociety = dr.Field<string>("AccountNoInSociety");
                    objCCSComplaintRegisterModel.AccountIFSCInSociety = dr.Field<string>("AccountIFSCInSociety");
                    objCCSComplaintRegisterModel.AccountHolderName = dr.Field<string>("AccountHolderName");
                    objCCSComplaintRegisterModel.PassbookCrossChequeView = dr.Field<string>("PassbookCrossCheque");
                    objCCSComplaintRegisterModel.BPNo = dr.Field<string>("BPNo");
                    objCCSComplaintRegisterModel.ReceiptNo = dr.Field<string>("ReceiptNo");

                    objCCSComplaintRegisterModel.CCS = new CCSModel() { SocietyNameEng = dr.Field<string>("SocietyName") };
                    objCCSComplaintRegisterModel.CreateDate = dr.Field<string>("CreateDateTime");
                    objCCSComplaintRegisterModel.UserType = (Convert.ToString(dt.Rows[0]["UserType"]).ToUpper() == "K" ? "कियोस्क द्वारा " : "स्वयं सिटीजन द्वारा");
                    objCCSComplaintRegisterModel.District = new DistrictModel() { DistrictEng = dr.Field<string>("DISTRICT_ENG"), DistrictHi = dr.Field<string>("DISTRICT_MANGAL") };


                    objCCSComplaintRegisterModel.CreateDate = dr.Field<string>("CreateDateTime");
                    objCCSComplaintRegisterModel.CreateDate = dr.Field<string>("CreateDateTime");

                    objCCSComplaintRegisterModel.FileDetails = new List<CCSComplaintRegisterDetailModel>();

                    foreach (var item in dt.AsEnumerable())
                    {
                        CCSComplaintRegisterDetailModel temp = new CCSComplaintRegisterDetailModel();
                        temp.FileTypeEng = item.Field<string>("TypeNameEng");
                        temp.FileTypeHi = item.Field<string>("TypeNameHi");
                        temp.FileName = Convert.ToString(item.Field<int>("FileTypeId"));
                        temp.FilePathView = item.Field<string>("FilePath");
                        temp.FileTypeId = item.Field<int>("FileTypeId");
                        temp.AccountNo = item.Field<string>("DetailAccountNo");
                        temp.PaidAmount = item.Field<decimal>("PaidAmount");
                        temp.PayDate = item.Field<string>("PayDate");
                        temp.MaturityAmount = item.Field<decimal>("MaturityAmount");
                        temp.MaturityDate = item.Field<string>("MaturityDate");
                        objCCSComplaintRegisterModel.FileDetails.Add(temp);
                    }

                }
                catch (Exception ex)
                {
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen", FunctionName = "public JsonResult GetComplaintDetailsByComplaintId(string ComplaintIdEnc)" + " Message:" + Convert.ToString(ex.Message) + " StackTrace:" + Convert.ToString(ex.StackTrace) + " InnerException:" + Convert.ToString(ex.InnerException) }, ex);
                }
            }



            return Json(objCCSComplaintRegisterModel);
        }


        [HttpPost]
        public JsonResult IsDuplicateEdit(string ComplaintIdEnc)
        {
            CCSComplaintRegisterModel objCCSComplaintRegisterModel = new CCSComplaintRegisterModel();
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            int result = objCitizenModel.CCSComplaintAlreadyEdited(ComplaintIdEnc);

            if (result == 2)
            {
                return Json(new { Result = false, Response = "DuplicateEntry" });
            }
            else if (result == 1)
            {
                return Json(new { Result = true, Response = "Valid Data" });
            }
            else
            {
                return Json(new { Result = false, Response = "Error" });
            }
        }


        [HttpPost]
        public JsonResult EditCCSComplaint(CCSComplaintRegisterModel model)
        {
            HttpCookie cookie = Request.Cookies[SessionIdentity.Culture];
            model.CreateIP = COMMON_FUNCTION.GetIpAddress();
            EmitraController objEC = new EmitraController();
            string fileName = string.Empty;
            DataTable DT = null;
            int KioskResult = -1;
            string Flags = "";
            model.SSOId = _LoggedInUser.SSOID;
            model.UserType = Convert.ToString(_LoggedInUser.UserType);
            string language = cookie.Value;

            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());


            try
            {
                int AlreadyExistFlag = 0;
                AlreadyExistFlag = objCitizenModel.CCSComplaintAlreadyEdited(model.ComplaintIdEnc);

                if (AlreadyExistFlag == 2)
                {

                    string response = "<div class=\"form-group citizen-dash-head\">" +
                                               "<div class=\"table-responsive\">" +
                                               "<table class=\"table devtable\">" +
                                               "<thead><tr><th class=\"ng-binding\">" + (string.Equals(language, "en-us") ? "Transaction Status" : "ट्रांजेक्शन स्थिति") + "</th></tr></thead>" +
                                               "<tbody><tr><td class=\"ng-binding\">" + (string.Equals(language, "en-us") ? "Since the complaint has been edited earlier, this complaint is locked for re-edit (Complaint will not be re-edited)" : "चूँकि शिकायत पहले एडिट चुकी है | अतः यह शिकायत  पुनः एडिट के लिए लॉक है (शिकायत पुनः एडिट नहीं होगी) ") + " </td></tr></tbody>" +
                                               "</table>" +
                                               "</div>" +
                                               "</div>";

                    return Json(new { Result = false, Response = response });
                }
                else if (AlreadyExistFlag == -1)
                {
                    string response = "<div class=\"form-group citizen-dash-head\">" +
                            "<div class=\"table-responsive\">" +
                            "<table class=\"table devtable\">" +
                            "<thead><tr><th class=\"ng-binding\">" + (string.Equals(language, "en-us") ? "Transaction Status" : "ट्रांजेक्शन स्थिति") + "</th></tr></thead>" +
                            "<tbody><tr><td class=\"ng-binding\">" + (string.Equals(language, "en-us") ? "Some Error occured, please try sometime later." : "कुछ त्रुटि हुई, कृपया कुछ समय बाद प्रयास करें।") + " </td></tr></tbody>" +
                            "</table>" +
                            "</div>" +
                            "</div>";

                    return Json(new { Result = false, Response = response });
                }


                if (Request.Files != null && Request.Files.Count > 0)
                {
                    string folderpath = "../Content/CCSComplaintFiles";
                    if (!System.IO.Directory.Exists(Server.MapPath(folderpath)))
                        System.IO.Directory.CreateDirectory(Server.MapPath(folderpath));

                    try
                    {
                        foreach (string files in Request.Files)
                        {
                            HttpPostedFileBase tempfile = Request.Files[files];
                            fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(tempfile.FileName);
                            tempfile.SaveAs(Path.Combine(Server.MapPath("~/Content/CCSComplaintFiles/"), fileName));

                            if (files.ToString().ToLower() == "remittancepassbook")
                            {
                                model.PassbookCrossCheque = fileName;
                            }
                            else
                            {
                                string index = files.ToString().ToLower().Split('_')[1];
                                model.FileDetails[Convert.ToInt32(index)].FilePath = fileName;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        DBLogger objDBLogger = new DBLogger();
                        objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen", FunctionName = "Function for save Files of CCS edit Details" + " Message: " + Convert.ToString(ex.Message) + " StackTrace: " + Convert.ToString(ex.StackTrace) + " InnerException: " + Convert.ToString(ex.InnerException) }, ex);
                    }
                }



                string FileDetailsCheckMsg = string.Empty;
                int RowCounter = 1;

                foreach (var item in model.FileDetails)
                {
                    string temp = string.Empty;
                    string pointer = "•";

                    if (item.FileTypeId < 1)
                    {
                        temp += pointer + ". दस्तावेज का नाम ";
                    }

                    if (string.IsNullOrEmpty(item.AccountNo))
                    {
                        temp += pointer + ". बैंक खाता संख्या / दस्तावेज संख्या ";
                    }
                    if (!DataFormatter.IsValidIntOrDecimal(Convert.ToString(item.PaidAmount)))
                    {
                        temp += pointer + ". जमा कराई गई राशि ";
                    }
                    if (!DataFormatter.IsValidDateTime(item.PayDate))
                    {
                        temp += pointer + ". जमा करने की तिथि ";
                    }
                    if (!DataFormatter.IsValidIntOrDecimal(Convert.ToString(item.MaturityAmount)))
                    {
                        temp += pointer + ". परिपक्वता राशि ";
                    }
                    if (!DataFormatter.IsValidDateTime(item.MaturityDate))
                    {
                        temp += pointer + ". परिपक्वता तिथि ";
                    }

                    if (!(!string.IsNullOrEmpty(item.FilePathView) || !string.IsNullOrEmpty(item.FilePath)))
                    {
                        temp += pointer + ". अपलोड की गई फ़ाइल ";
                    }

                    if (!string.IsNullOrEmpty(temp))
                    {
                        FileDetailsCheckMsg += "<td class=\"ng-binding\"> तालिका क्रमांक " + RowCounter + ". " + temp + " </td></tr>";
                    }


                    RowCounter++;
                }

                if (!string.IsNullOrEmpty(FileDetailsCheckMsg))
                {
                    string response = "<div class=\"form-group citizen-dash-head\">" +
                           "<div class=\"table-responsive\">" +
                           "<table class=\"table devtable\">" +
                           "<thead><tr><th class=\"ng-binding\">" + (string.Equals(language, "en-us") ? "Transaction Status" : "ट्रांजेक्शन स्थिति") + "</th></tr></thead>" +
                           "<tbody><tr><td class=\"ng-binding\">" + (string.Equals(language, "en-us") ? "Please provide details of documents related to deposits." : "कृपया जमाओं से संबंधित दस्तावेजों का विवरण प्रदान करें।") + " </td></tr>" +
                           FileDetailsCheckMsg +
                           "</tbody>" +
                           "</table>" +
                           "</div>" +
                           "</div>";

                    return Json(new { Result = false, Response = response });
                }


                int Tranid = objCitizenModel.EditCCSComplaint(model);
                if (Tranid > 0)
                {
                    EmitraModelFactory ObjEmitraModel = new EmitraModelFactory(new DBLogger());
                    EmitraController OBJ_EmitraController = new EmitraController();

                    if (_LoggedInUser.UserType == "K")
                    {
                        EmitraModelFactory objEmitraModel = new EmitraModelFactory(new DBLogger());
                        DT = objEmitraModel.CCSComplaintRegisterData(Tranid);

                        if (System.Web.HttpContext.Current.Session["ARequest"] != null && _LoggedInUser.KIOSKCODE.Length > 0 && DT != null && DT.Rows.Count > 0)
                        {
                            KioskResult = ObjEmitraModel.MakePaymentForAll("0", DT.Rows[0]["ComplainerName"].ToString(), "0", DT.Rows[0]["ReceiptNo"].ToString(), Tranid.ToString(), _LoggedInUser.KIOSKCODE, _LoggedInUser.SSOID);

                            if (KioskResult > 0)
                            {
                                Flags = "Sucess";

                                Flags = objCitizenModel.ActiveCCSComplaintRegister(KioskResult, model.MobileNo, model.ComplainerName, model.ComplainerFatherName, DT.Rows[0]["ReceiptNo"].ToString());
                                //MOVE TEMP DATA TO MAIN
                                //ApplicationFlowModel AFM = new ApplicationFlowModel();
                                //AFM.AddApplicationFlow(_Entity.SocietyIDEnc, 24, "BANK ACCOUNT DETAILS UPLOADED", 0);
                                DResponse resp = JsonConvert.DeserializeObject<DResponse>(Flags);

                                DataTable Response = objCitizenModel.GetCCSComplaintRegisterForCitizen(Tranid);

                                if (Response != null && Response.Rows.Count > 0)
                                {
                                    string MobileNo = Response.Rows[0]["MobileNo"].ToString();

                                    if (MobileNo.Length == 10)
                                    {
                                        EsanchaarController EC = new EsanchaarController();
                                        string IsOk = EC.SendSocietySMS("Dear " + Convert.ToString(Response.Rows[0]["ComplainerName"]) + " " + Convert.ToString(Response.Rows[0]["ComplainerFatherName"]) + ", Your complaint ( No.: " + Convert.ToString(Response.Rows[0]["ReceiptNo"]) + ") has been edited successfully. ", MobileNo, "HIN");
                                    }
                                }

                                Flags = "<div class=\"form-group citizen-dash-head\"><div class=\"table-responsive\"><table class=\"table devtable\">" +
                                "<thead><tr><th class=\"ng-binding\" colspan=\"4\">Transaction Status</th>" +
                                //"<th class=\"ng-binding\">Column 2</th><th class=\"ng-binding\">Column 3</th> <th>Column 4</th>" +
                                "</tr></thead><tbody>" +
                                "<tr><td class=\"ng-binding\">TRANSACTIONID</td><td class=\"ng-binding\">" + resp.TRANSACTIONID + "</td><td class=\"ng-binding\">RECEIPTNO</td><td class=\"ng-binding\">" + resp.RECEIPTNO + "</td></tr>" +
                                "<tr><td class=\"ng-binding\">PAIDAMOUNT</td><td class=\"ng-binding\">" + resp.TRANSAMT + "</td><td class=\"ng-binding\">Date</td><td class=\"ng-binding\">" + resp.EMITRATIMESTAMP.Substring(0, 8) + "</td></tr>" +
                                "<tr><td class=\"ng-binding\">RESPONSEMESSAGE</td><td class=\"ng-binding\">" + resp.MSG + "</td><td class=\"ng-binding\"></td><td class=\"ng-binding\"></td></tr>" +
                                "<tr><td class=\"ng-binding\">Complaint No </td><td class=\"ng-binding\">" + Convert.ToString(Response.Rows[0]["ReceiptNo"]) + "</td><td class=\"ng-binding\"></td><td class=\"ng-binding\"></td></tr>" +
                                "</tbody></table></div></div>";
                            }
                            else
                            {
                                // Flags = objSocietyModel.GetEmitraBankStatmentLastTran(int.Parse(Flags));
                                DResponse resp = JsonConvert.DeserializeObject<DResponse>(Flags);
                                Flags = "<div class=\"form-group citizen-dash-head\"><div class=\"table-responsive\"><table class=\"table devtable\">" +
                                "<thead><tr><th class=\"ng-binding\" colspan=\"4\">Transaction Status</th>" +
                                //"<th class=\"ng-binding\">Column 2</th><th class=\"ng-binding\">Column 3</th> <th>Column 4</th>" +
                                "</tr></thead><tbody>" +
                                "<tr><td class=\"ng-binding\">TRANSACTIONID</td><td class=\"ng-binding\">" + resp.TRANSACTIONID + "</td><td class=\"ng-binding\">RECEIPTNO</td><td class=\"ng-binding\">" + resp.RECEIPTNO + "</td></tr>" +
                                "<tr><td class=\"ng-binding\">PAIDAMOUNT</td><td class=\"ng-binding\">" + resp.TRANSAMT + "</td><td class=\"ng-binding\">Date</td><td class=\"ng-binding\">" + resp.EMITRATIMESTAMP.Substring(0, 8) + "</td></tr>" +
                                "<tr><td class=\"ng-binding\">RESPONSEMESSAGE</td><td class=\"ng-binding\">" + resp.MSG + "</td><td class=\"ng-binding\"></td><td class=\"ng-binding\"></td></tr>" +

                                "</tbody></table></div></div>";
                            }
                        }


                        //Flags = "<div class=\"form-group citizen-dash-head\">" +
                        //        "<div class=\"table-responsive\">" +
                        //        "<table class=\"table devtable\">" +
                        //        "<thead><tr><th class=\"ng-binding\">" + (string.Equals(language, "en-us") ? "Transaction Status" : "ट्रांजेक्शन स्थिति") + "</th></tr></thead>" +
                        //        // "<tbody><tr><td class=\"ng-binding\">" + (string.Equals(language, "en-us") ? "Complaint has been successfully filed against Credit Co-operative Society. Your complaint number is " + Convert.ToString(Response.Rows[0]["ReceiptNo"]) + "." : "क्रेडिट को-ऑपरेटिव सोसायटी के खिलाफ शिकायत सफलतापूर्वक दर्ज कर दी गई है | आपकी  शिकायत संख्या " + Convert.ToString(Response.Rows[0]["ReceiptNo"]) + " है") + " </td></tr></tbody>" +
                        //        "<tbody><tr><td class=\"ng-binding\">" + (string.Equals(language, "en-us") ? "Complaint ( No.: " + Convert.ToString(Response.Rows[0]["ReceiptNo"]) + ") has been successfully edited." : "आपकी  शिकायत (संख्या: " + Convert.ToString(Response.Rows[0]["ReceiptNo"]) + ") सफलतापूर्वक सम्पादित (एडिट) कर दी गई है |") + " </td></tr></tbody>" +
                        //        "</table>" +
                        //        "</div>" +
                        //        "</div>";


                    }
                    else if (_LoggedInUser.UserType.ToUpper() == "C" || _LoggedInUser.UserType.ToUpper() == "D")
                    {
                        DataTable Response = objCitizenModel.GetCCSComplaintRegisterForCitizen(Tranid);

                        if (Response != null && Response.Rows.Count > 0)
                        {
                            string MobileNo = Response.Rows[0]["MobileNo"].ToString();

                            if (MobileNo.Length == 10)
                            {
                                EsanchaarController EC = new EsanchaarController();
                                string IsOk = EC.SendSocietySMS("Dear " + Convert.ToString(Response.Rows[0]["ComplainerName"]) + " S/O " + Convert.ToString(Response.Rows[0]["ComplainerFatherName"]) + ", Your complaint has been registered successfully. Your complaint number is " + Convert.ToString(Response.Rows[0]["ReceiptNo"]) + ".", MobileNo, "HIN");
                            }
                        }

                        language = "hi-in";// (!string.IsNullOrEmpty(cookie.Value) ? cookie.Value : string.Empty).ToLower();

                        Flags = "<div class=\"form-group citizen-dash-head\">" +
                                "<div class=\"table-responsive\">" +
                                "<table class=\"table devtable\">" +
                                "<thead><tr><th class=\"ng-binding\">" + (string.Equals(language, "en-us") ? "Transaction Status" : "ट्रांजेक्शन स्थिति") + "</th></tr></thead>" +
                                "<tbody><tr><td class=\"ng-binding\">" + (string.Equals(language, "en-us") ? "Your complaint has been registered successfully. Your complaint number is " + Convert.ToString(Response.Rows[0]["ReceiptNo"]) + "." + "." : "आपकी  शिकायत (संख्या: " + Convert.ToString(Response.Rows[0]["ReceiptNo"]) + ") सफलतापूर्वक सम्पादित (एडिट) कर दी गई है |") + " </td></tr></tbody>" +
                                "</table>" +
                                "</div>" +
                                "</div>";
                    }
                }
                else
                {
                    Flags = "<div class=\"form-group citizen-dash-head\">" +
                            "<div class=\"table-responsive\">" +
                            "<table class=\"table devtable\">" +
                            "<thead><tr><th class=\"ng-binding\">" + (string.Equals(language, "en-us") ? "Transaction Status" : "ट्रांजेक्शन स्थिति") + "</th></tr></thead>" +
                            "<tbody><tr><td class=\"ng-binding\">" + (string.Equals(language, "en-us") ? "Some Error occured, please try sometime later." : "कुछ त्रुटि हुई, कृपया कुछ समय बाद प्रयास करें।") + " </td></tr></tbody>" +
                            "</table>" +
                            "</div>" +
                            "</div>";
                }

                return Json(new { Result = true, Response = Flags });
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen", FunctionName = "Function for Edit CCS Details" + " Message: " + Convert.ToString(ex.Message) + " StackTrace: " + Convert.ToString(ex.StackTrace) + " InnerException: " + Convert.ToString(ex.InnerException) }, ex);

                string response = "<div class=\"form-group citizen-dash-head\">" +
                            "<div class=\"table-responsive\">" +
                            "<table class=\"table devtable\">" +
                            "<thead><tr><th class=\"ng-binding\">" + (string.Equals(language, "en-us") ? "Transaction Status" : "ट्रांजेक्शन स्थिति") + "</th></tr></thead>" +
                            "<tbody><tr><td class=\"ng-binding\">" + (string.Equals(language, "en-us") ? "Some Error occured, please try sometime later." : "कुछ त्रुटि हुई, कृपया कुछ समय बाद प्रयास करें।") + " </td></tr></tbody>" +
                            "</table>" +
                            "</div>" +
                            "</div>";

                return Json(new { Result = false, Response = response });
            }
        }

        [HttpGet]
        public ActionResult CCSConsolidatedReport()
        {
            ViewBag.LoggedInUserDistrictId = _LoggedInUser.DistrictId;
            return View();
        }

        [HttpPost]
        public JsonResult GetCCSConsolidatedReportData(DataPagingModel Details, string fromDate, string toDate, int districtId, int CCSId)
        {
            string msg = string.Empty;

            if (!string.IsNullOrEmpty(fromDate) && !DataFormatter.IsValidDateTime(fromDate))
                msg += "date format in from date is not valid (required dd/mm/yyyy). ";

            if (!string.IsNullOrEmpty(toDate) && !DataFormatter.IsValidDateTime(toDate))
                msg += "date format in to date is not valid (required dd/mm/yyyy). ";

            if (!string.IsNullOrEmpty(msg))
                return Json(new { Result = false, data = new int[] { }, Message = msg }); // Validation fails no further action required

            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());

            if (!string.IsNullOrEmpty(fromDate))
                fromDate = fromDate.Split('/')[1] + "/" + fromDate.Split('/')[0] + "/" + fromDate.Split('/')[2];

            if (!string.IsNullOrEmpty(toDate))
                toDate = toDate.Split('/')[1] + "/" + toDate.Split('/')[0] + "/" + toDate.Split('/')[2];

            DataTable dt = objCitizenModel.GetCCSConsolidatedReportData(ref Details, fromDate, toDate, _LoggedInUser.DistrictId, _LoggedInUser.UserRoleDeptMappingId);

            if (dt == null || dt.Rows.Count < 1)
                return Json(new { Result = false, data = new int[] { }, Message = "No Record found as per search criteria." });

            List<CCSComplaintRegisterModel> list = new List<CCSComplaintRegisterModel>();

            foreach (var item in dt.AsEnumerable())
            {
                CCSComplaintRegisterModel temp = new CCSComplaintRegisterModel();
                temp.ComplainerName = item.Field<string>("ComplainerName");
                temp.ComplainerFatherName = item.Field<string>("ComplainerFatherName");
                temp.MobileNo = item.Field<string>("MobileNo");
                temp.ReceiptNo = item.Field<string>("ReceiptNo");
                temp.AmountDeposited = item.Field<decimal>("AmountDeposited");
                temp.CreateDate = item.Field<string>("CreateDateTime");
                temp.ComplaintIdEnc = item.Field<string>("ComplaintIdEnc");
                temp.IsPermanentLocked = item.Field<bool>("IsPermanentLocked");
                try
                {
                    string str = COMMON_FUNCTION.Decrypt(item.Field<string>("AADHAR"));
                    temp.AADHARMasked = string.Concat("".PadLeft(8, 'X'), str.Substring(str.Length - 4));
                }
                catch (Exception ex)
                {
                    temp.AADHARMasked = "XXXXXXXXXXXX";
                    //DBLogger objDBLogger = new DBLogger();
                    //objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahkar.Controller.Citizen", FunctionName = "public JsonResult GetCCSConsolidatedReportData(string fromDate, string toDate, int districtId, int CCSId) while AADHAR Decryption " + " Message: " + Convert.ToString(ex.Message) }, ex);
                }

                temp.District = new DistrictModel() { DistrictId = 1, DistrictEng = item.Field<string>("DISTRICT_ENG"), DistrictHi = item.Field<string>("DISTRICT_MANGAL") };
                temp.CCS = new CCSModel() { SocietyNameEng = item.Field<string>("SocietyName") };
                list.Add(temp);
            }

            return Json(new { Result = true, data = list, Message = "Success" });
        }

        [HttpPost]
        public void DownloadFiles(string ComplaintIdEnc)
        {
            string ReceiptNo = "";
            var readMeContent = string.Format("\t*This ZIP file was created by Co operative Department at {0} and contains the following files:{0}", DateTime.Now, Environment.NewLine);

            try
            {
                CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
                objCitizenModel.SetFinalLockOnCCSComplaint(ComplaintIdEnc);
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.CitizenController", FunctionName = "DownloadFiles(string ComplaintIdEnc) While Final Lock Status Change" }, ex);
            }

            int SrNo = 1;
            int k = 0;

            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                zip.AddDirectoryByName("Files");

                CitizenModel obj = new CitizenModel(new DBLogger());
                DataTable dt = obj.GetCCSDetails(SearchType: 3, ComplaintIdEnc: ComplaintIdEnc);



                if (dt != null && dt.Rows.Count > 0)
                {

                    readMeContent += string.Format("\t* ----------------------------------------------------------------------------------------------------------------------------------{0}{0}", Environment.NewLine);
                    readMeContent += string.Format("\t* Passbook File : {0} {1}", dt.Rows[0]["PassbookCrossCheque"].ToString(), Environment.NewLine);

                    //zip.AddFile(Server.MapPath("~/Content/CCSComplaintFiles/" + dt.AsEnumerable().First().Field<string>("PassbookCrossCheque")), "Files");
                    //WebClient webClient = new WebClient();
                    //var temp = dt.Rows[0]["PassbookCrossCheque"].ToString().Split('/');

                    if (System.IO.File.Exists(Server.MapPath(dt.Rows[0]["PassbookCrossCheque"].ToString())))
                    {
                        zip.AddFile(Server.MapPath(dt.Rows[0]["PassbookCrossCheque"].ToString()), "Files");
                    }

                    //readMeContent += string.Format("\t* ----------------------------------------------------------------------------------------------------------------------------------{0}", Environment.NewLine);
                    //readMeContent += string.Format("\t* Sr. No.   File Type     Account No.    Deposited Amount   Deposit Date    Maturity Amount  Maturity Date   File Name {0}", Environment.NewLine);

                    foreach (var item in dt.AsEnumerable())
                    {
                        try
                        {
                            ReceiptNo = item.Field<string>("ReceiptNo");

                            //webClient = new WebClient();
                            // temp = dt.Rows[SrNo - 1]["FilePath"].ToString().Split('/');

                            //if (temp != null && temp.Length > 2)
                            //{
                            //webClient.DownloadFile(dt.AsEnumerable().First().Field<string>("FilePath"), Server.MapPath("~/Content/CCSComplaintFiles/" + temp[temp.Length - 1].ToString()));

                            string filePath = Server.MapPath(dt.Rows[SrNo - 1]["FilePath"].ToString());
                            if (System.IO.File.Exists(filePath))
                            {
                                zip.AddFile(filePath, "Files");
                            }

                            readMeContent += string.Format("\t* Sr. No. : #{0}  //  File Type : {1}  //  Account No. : {2}  //  Deposited Amount : {3} //  Deposit Date : {4} //   Maturity Amount : {5} // Maturity Date : {6}  // File Name : {7} {8}",
                                SrNo,
                                 item.Field<string>("TypeNameEng").ToString(),
                                item.Field<string>("DetailAccountNo"),
                                item.Field<decimal>("PaidAmount").ToString(),
                                item.Field<string>("PayDate"),
                                item.Field<decimal>("MaturityAmount").ToString(),
                                item.Field<string>("MaturityDate"),
                                item.Field<string>("FilePath"),
                                Environment.NewLine);
                            //}

                            // readMeContent += string.Format("\t* ----------------------------------------------------------------------------------------------------------------------------------{0}", Environment.NewLine);

                            SrNo++;
                        }
                        catch (Exception ex)
                        {
                            DBLogger objDBLogger = new DBLogger();
                            objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahkar.Controller.Citizen", FunctionName = "public void DownloadFiles(string ComplaintIdEnc) while add files in zip " + " Message: " + Convert.ToString(ex.Message) }, ex);
                        }
                    }
                }

                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format("Zip_{0}_{1}.zip", ReceiptNo, DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);

                zip.Password = null;
                zip.AddEntry("README.txt", readMeContent);

                zip.Save(Response.OutputStream);
                Response.End();
            }
        }



        public string ESING_New()
        {
            string ModifiedData = "";//COMMON_FUNCTION.NewFunctionForEsign("");
            string url = "http://59.145.20.75/esign/2.1/signdoc/";
            System.Net.WebRequest req = null;
            WebResponse rsp = null;

            req = WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/xml";
            //req.Headers["appname"] = "CO-Operative";
            StreamWriter writer = new StreamWriter(req.GetRequestStream());
            writer.Write(ModifiedData);
            writer.Close();
            rsp = req.GetResponse();
            StreamReader sr = new StreamReader(rsp.GetResponseStream());
            string results = sr.ReadToEnd();
            sr.Close();
            return results;
        }

        [HttpGet]
        public void GetEncAADHARNo()
        {

            string DirctoryPath = System.Web.HttpContext.Current.Server.MapPath("~/LogFiles1/" + string.Format("{0}_{1}", DateTime.Now.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture), DateTime.Now.Year));
            string FilePath = System.Web.HttpContext.Current.Server.MapPath("~/LogFiles1/" + string.Format("{0}_{1}", DateTime.Now.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture), DateTime.Now.Year) + "/" + string.Format("{0}_{1}_{2}.txt", DateTime.Now.ToString("dd", System.Globalization.CultureInfo.InvariantCulture), DateTime.Now.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture), DateTime.Now.Year));


            if (!System.IO.Directory.Exists(DirctoryPath))
            {
                DirectoryInfo directory = System.IO.Directory.CreateDirectory(DirctoryPath);
            }

            if (!System.IO.File.Exists(FilePath))
            {
                using (StreamWriter sw = System.IO.File.CreateText(FilePath))
                {
                }
            }




            string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("~/LogFiles1/ContactDetailsForCorrection_Updated.xlsx") + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";

            OleDbConnection conn = new OleDbConnection(connString);

            if (conn.State == ConnectionState.Closed)
                conn.Open();

            DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            foreach (DataRow SheetRow in dtSheet.Rows)
            {
                string query = "SELECT * FROM [" + SheetRow.ItemArray[2] + "]";
                OleDbCommand cmd2 = new OleDbCommand(query, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd2);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();

                try
                {
                    foreach (DataRow Dr in dt.Rows)
                    {
                        using (StreamWriter sw = System.IO.File.AppendText(FilePath))
                        {
                            sw.Write("AADHAR No:" + Dr.Field<string>("AADHAR No"));
                            sw.Write(" MobileNo : " + Dr.Field<double>("MobileNo"));
                            sw.Write(" ComplainantName : " + Dr.Field<string>("ComplainerName"));
                            sw.Write(" ComplainantFatherName : " + Dr.Field<string>("ComplainerFatherName"));
                            sw.Write(" AADHAR : " + Dr.Field<string>("AADHAR"));
                            sw.WriteLine(" Calculated AADHAR : " + COMMON_FUNCTION.Encrypt(Dr.Field<string>("AADHAR No").Replace(" ", "").Trim()) + "\n");
                        }
                    }
                }
                catch (Exception ex)
                {
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citozen", FunctionName = "public string CCSComplaintDocumentUploadedList()" }, ex);
                }
            }
        }

        [HttpPost]
        public string CheckUserType()
        {
            return _LoggedInUser.UserType.ToString();
        }

        [HttpGet]

        [OutputCache(Duration = 14400)]
        [ActionName("GetDeshboardData_scheduler")]
        public string GetDeshboardData_scheduler()
        {
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            DataSet DT = objCitizenModel.GetDeshboardData_scheduler();
            return JsonConvert.SerializeObject(DT, Newtonsoft.Json.Formatting.Indented);
        }

        [HttpGet]
        public string GetCCSCount()
        {

            CitizenModel objMenuActionModel = new CitizenModel(new FileLogger());
            DataTable DT = objMenuActionModel.GetCCSCountdata(_LoggedInUser.DistrictId);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }


        [HttpGet]
        [ActionName("UpdatePhotosAndDoc")]
        public void UpdatePhotosAndDoc(int SID = 0)
        {
            try
            {
                CitizenModel objElectionModel = new CitizenModel(new DBLogger());
                DataTable DT = new DataTable();

                DT = objElectionModel.GetAllPhoto(1, SID);
                string pathString = "~/Content/SocietyProfiling/";

                string SourceFilePath = "";

                int Count = 0;
                foreach (DataRow Dr in DT.Rows)
                {
                    SocietyAdministrativeEntity OBJ = new SocietyAdministrativeEntity();

                    //pathString = "~/Content/SocietyProfiling/";

                    pathString = "~/Content/SocietyProfiling/" + Dr["SocietyDirectory"].ToString();

                    if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                    {
                        System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(pathString));
                    }

                    if (Dr["RegistrationCertificateFile"] != null && !Dr["RegistrationCertificateFile"].ToString().Contains("undefined") && Dr["RegistrationCertificateFile"].ToString() != "")
                    {
                        SourceFilePath = "~/Content/SocietyProfiling/" + Dr["RegistrationCertificateFile"].ToString();
                        pathString = "~/Content/SocietyProfiling/" + Dr["SocietyDirectory"].ToString() + "/" + Dr["RegistrationCertificateFile"].ToString();

                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Copy(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));

                        if (Dr["OLdSocietyDirectory"].ToString().Length > 0)
                        {
                            SourceFilePath = "~/Content/SocietyProfiling/" + Dr["OLdSocietyDirectory"].ToString() + "/" + Dr["RegistrationCertificateFile"].ToString();
                            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                                System.IO.File.Copy(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));
                        }

                        SourceFilePath = "~/Content/NewRegistration/" + Dr["RegistrationCertificateFile"].ToString();
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Copy(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));
                    }

                    if (Dr["BylawsFile"] != null && !Dr["BylawsFile"].ToString().Contains("undefined") && Dr["BylawsFile"].ToString() != "")
                    {
                        SourceFilePath = "~/Content/SocietyProfiling/" + Dr["BylawsFile"].ToString();
                        pathString = "~/Content/SocietyProfiling/" + Dr["SocietyDirectory"].ToString() + "/" + Dr["BylawsFile"].ToString();
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));

                        if (Dr["OLdSocietyDirectory"].ToString().Length > 0)
                        {
                            SourceFilePath = "~/Content/SocietyProfiling/" + Dr["OLdSocietyDirectory"].ToString();
                            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                                System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));
                        }

                        SourceFilePath = "~/Content/NewRegistration/" + Dr["BylawsFile"].ToString();
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));

                    }

                    if (Dr["RulesRegulationFile"] != null && !Dr["RulesRegulationFile"].ToString().Contains("undefined") && Dr["RulesRegulationFile"].ToString() != "")
                    {
                        SourceFilePath = "~/Content/SocietyProfiling/" + Dr["RulesRegulationFile"].ToString();
                        pathString = "~/Content/SocietyProfiling/" + Dr["SocietyDirectory"].ToString() + "/" + Dr["RulesRegulationFile"].ToString();

                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));

                        if (Dr["OLdSocietyDirectory"].ToString().Length > 0)
                        {
                            SourceFilePath = "~/Content/SocietyProfiling/" + Dr["OLdSocietyDirectory"].ToString() + "/" + Dr["RulesRegulationFile"].ToString();
                            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                                System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));
                        }

                        SourceFilePath = "~/Content/NewRegistration/" + Dr["RulesRegulationFile"].ToString();
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));
                    }


                    if (Dr["MemberListFileName"] != null && !Dr["MemberListFileName"].ToString().Contains("undefined") && Dr["MemberListFileName"].ToString() != "")
                    {

                        SourceFilePath = "~/Content/SocietyProfiling/" + Dr["MemberListFileName"].ToString();
                        pathString = "~/Content/SocietyProfiling/" + Dr["SocietyDirectory"].ToString() + "/" + Dr["MemberListFileName"].ToString();

                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));

                        if (Dr["OLdSocietyDirectory"].ToString().Length > 0)
                        {
                            SourceFilePath = "~/Content/SocietyProfiling/" + Dr["OLdSocietyDirectory"].ToString() + "/" + Dr["MemberListFileName"].ToString();
                            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                                System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));
                        }

                        SourceFilePath = "~/Content/NewRegistration/" + Dr["MemberListFileName"].ToString();
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));
                    }


                    if (Dr["WorkPlanEcoViabilityFile"] != null && !Dr["WorkPlanEcoViabilityFile"].ToString().Contains("undefined") && Dr["WorkPlanEcoViabilityFile"].ToString() != "")
                    {

                        SourceFilePath = "~/Content/SocietyProfiling/" + Dr["WorkPlanEcoViabilityFile"].ToString();
                        pathString = "~/Content/SocietyProfiling/" + Dr["SocietyDirectory"].ToString() + "/" + Dr["WorkPlanEcoViabilityFile"].ToString();

                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));

                        if (Dr["OLdSocietyDirectory"].ToString().Length > 0)
                        {
                            SourceFilePath = "~/Content/SocietyProfiling/" + Dr["OLdSocietyDirectory"].ToString() + "/" + Dr["WorkPlanEcoViabilityFile"].ToString();
                            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                                System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));
                        }

                        SourceFilePath = "~/Content/NewRegistration/" + Dr["WorkPlanEcoViabilityFile"].ToString();
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));

                    }

                    if (Dr["GeneralMeetingProcedureFile"] != null && !Dr["GeneralMeetingProcedureFile"].ToString().Contains("undefined") && Dr["GeneralMeetingProcedureFile"].ToString() != "")
                    {
                        SourceFilePath = "~/Content/SocietyProfiling/" + Dr["GeneralMeetingProcedureFile"].ToString();
                        pathString = "~/Content/SocietyProfiling/" + Dr["SocietyDirectory"].ToString() + "/" + Dr["GeneralMeetingProcedureFile"].ToString();
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));

                        if (Dr["OLdSocietyDirectory"].ToString().Length > 0)
                        {
                            SourceFilePath = "~/Content/SocietyProfiling/" + Dr["OLdSocietyDirectory"].ToString() + "/" + Dr["GeneralMeetingProcedureFile"].ToString();
                            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                                System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));
                        }

                        SourceFilePath = "~/Content/NewRegistration/" + Dr["GeneralMeetingProcedureFile"].ToString();
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));
                    }

                    if (Dr["BalanceSheetExcelFile"] != null && !Dr["BalanceSheetExcelFile"].ToString().Contains("undefined") && Dr["BalanceSheetExcelFile"].ToString() != "")
                    {
                        SourceFilePath = "~/Content/SocietyProfiling/" + Dr["BalanceSheetExcelFile"].ToString();
                        pathString = "~/Content/SocietyProfiling/" + Dr["SocietyDirectory"].ToString() + "/" + Dr["BalanceSheetExcelFile"].ToString();
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));

                        if (Dr["OLdSocietyDirectory"].ToString().Length > 0)
                        {
                            SourceFilePath = "~/Content/SocietyProfiling/" + Dr["OLdSocietyDirectory"].ToString() + "/" + Dr["BalanceSheetExcelFile"].ToString();
                            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                                System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));
                        }

                        SourceFilePath = "~/Content/NewRegistration/" + Dr["BalanceSheetExcelFile"].ToString();
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));
                    }

                    if (Dr["ObjectsFile"] != null && !Dr["ObjectsFile"].ToString().Contains("undefined") && Dr["ObjectsFile"].ToString() != "")
                    {
                        SourceFilePath = "~/Content/SocietyProfiling/" + Dr["ObjectsFile"].ToString();
                        pathString = "~/Content/SocietyProfiling/" + Dr["SocietyDirectory"].ToString() + "/" + Dr["ObjectsFile"].ToString();
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));

                        if (Dr["OLdSocietyDirectory"].ToString().Length > 0)
                        {
                            SourceFilePath = "~/Content/SocietyProfiling/" + Dr["OLdSocietyDirectory"].ToString() + "/" + Dr["ObjectsFile"].ToString();
                            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                                System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));
                        }

                        SourceFilePath = "~/Content/NewRegistration/" + Dr["ObjectsFile"].ToString();
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));
                    }

                    if (Dr["SLSAFileName"] != null && !Dr["SLSAFileName"].ToString().Contains("undefined") && Dr["SLSAFileName"].ToString() != "")
                    {
                        SourceFilePath = "~/Content/SocietyProfiling/" + Dr["SLSAFileName"].ToString();
                        pathString = "~/Content/SocietyProfiling/" + Dr["SocietyDirectory"].ToString() + "/" + Dr["SLSAFileName"].ToString();
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));

                        if (Dr["OLdSocietyDirectory"].ToString().Length > 0)
                        {
                            SourceFilePath = "~/Content/SocietyProfiling/" + Dr["OLdSocietyDirectory"].ToString() + "/" + Dr["SLSAFileName"].ToString();
                            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                                System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));
                        }

                        SourceFilePath = "~/Content/NewRegistration/" + Dr["SLSAFileName"].ToString();
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));
                    }


                    if (Dr["RSSCFileName"] != null && !Dr["RSSCFileName"].ToString().Contains("undefined") && Dr["RSSCFileName"].ToString() != "")
                    {
                        SourceFilePath = "~/Content/SocietyProfiling/" + Dr["RSSCFileName"].ToString();
                        pathString = "~/Content/SocietyProfiling/" + Dr["SocietyDirectory"].ToString() + "/" + Dr["RSSCFileName"].ToString();
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));

                        if (Dr["OLdSocietyDirectory"].ToString().Length > 0)
                        {
                            SourceFilePath = "~/Content/SocietyProfiling/" + Dr["OLdSocietyDirectory"].ToString();
                            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                                System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));
                        }

                        SourceFilePath = "~/Content/NewRegistration/" + Dr["RSSCFileName"].ToString() + "/" + Dr["RSSCFileName"].ToString();
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));
                    }
                    if (Dr["DLSCFileName"] != null && !Dr["DLSCFileName"].ToString().Contains("undefined") && Dr["DLSCFileName"].ToString() != "")
                    {
                        SourceFilePath = "~/Content/SocietyProfiling/" + Dr["DLSCFileName"].ToString();
                        pathString = "~/Content/SocietyProfiling/" + Dr["SocietyDirectory"].ToString() + "/" + Dr["DLSCFileName"].ToString();
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));

                        if (Dr["OLdSocietyDirectory"].ToString().Length > 0)
                        {
                            SourceFilePath = "~/Content/SocietyProfiling/" + Dr["OLdSocietyDirectory"].ToString() + "/" + Dr["DLSCFileName"].ToString();
                            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                                System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));
                        }

                        SourceFilePath = "~/Content/NewRegistration/" + Dr["DLSCFileName"].ToString();
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));

                    }
                    if (Dr["SOCFileName"] != null && !Dr["SOCFileName"].ToString().Contains("undefined") && Dr["SOCFileName"].ToString() != "")
                    {
                        SourceFilePath = "~/Content/SocietyProfiling/" + Dr["SOCFileName"].ToString();
                        pathString = "~/Content/SocietyProfiling/" + Dr["SocietyDirectory"].ToString() + "/" + Dr["SOCFileName"].ToString();

                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));

                        if (Dr["OLdSocietyDirectory"].ToString().Length > 0)
                        {
                            SourceFilePath = "~/Content/SocietyProfiling/" + Dr["OLdSocietyDirectory"].ToString() + "/" + Dr["SOCFileName"].ToString();
                            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                                System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));
                        }

                        SourceFilePath = "~/Content/NewRegistration/" + Dr["SOCFileName"].ToString();
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));
                    }

                    if (Dr["BankOpeningCertificate"] != null && !Dr["BankOpeningCertificate"].ToString().Contains("undefined") && Dr["BankOpeningCertificate"].ToString() != "")
                    {
                        SourceFilePath = "~/Content/SocietyProfiling/" + Dr["BankOpeningCertificate"].ToString();
                        pathString = "~/Content/SocietyProfiling/" + Dr["SocietyDirectory"].ToString() + "/" + Dr["BankOpeningCertificate"].ToString();

                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));

                        if (Dr["OLdSocietyDirectory"].ToString().Length > 0)
                        {
                            SourceFilePath = "~/Content/SocietyProfiling/" + Dr["OLdSocietyDirectory"].ToString() + "/" + Dr["BankOpeningCertificate"].ToString();
                            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                                System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));
                        }

                        SourceFilePath = "~/Content/NewRegistration/" + Dr["BankOpeningCertificate"].ToString();
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));
                    }


                    if (Dr["Photo"] != null && Dr["Photo"].ToString() != "" && !Dr["Photo"].ToString().Contains("undefined") && !Dr["Photo"].ToString().Contains("Balnk"))
                    {

                        SourceFilePath = "~/Content/SocietyProfiling/" + Dr["Photo"].ToString();
                        pathString = "~/Content/SocietyProfiling/" + Dr["SocietyDirectory"].ToString() + "/" + Dr["Photo"].ToString();

                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));

                        if (Dr["OLdSocietyDirectory"].ToString().Length > 0)
                        {
                            SourceFilePath = "~/Content/SocietyProfiling/" + Dr["OLdSocietyDirectory"].ToString() + "/" + Dr["Photo"].ToString();
                            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                                System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));
                        }

                        SourceFilePath = "~/Content/NewRegistration/" + Dr["Photo"].ToString();
                        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)) && !System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(pathString));

                    }
                    Count = Count + 1;






                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen", FunctionName = "UpdatePhotosAndDoc" + " Message: " + Convert.ToString(ex.Message) + " StackTrace: " + Convert.ToString(ex.StackTrace) + " InnerException: " + Convert.ToString(ex.InnerException) }, ex);


            }

        }

        [HttpGet]
        [ActionName("GETNAME")]
        public void GETNAME()
        {
            CitizenModel objElectionModel = new CitizenModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objElectionModel.GETNAME();
            foreach (DataRow Dr in DT.Rows)
            {
                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                string PathString = "~/Content/SocietyProfiling/";
                string fileName = (Dr["RulesRegulationFile"].ToString().Length > 0 ? Dr["RulesRegulationFile"].ToString() : Dr["ObjectsFile"].ToString());
                string currentDirectory = Server.MapPath(PathString);
                string[] fullFilePath = System.IO.Directory.GetFiles(currentDirectory, fileName, SearchOption.AllDirectories);
                for (int i = 0; i < fullFilePath.Length; i++)
                {
                    if (fullFilePath[i].Contains("/200"))
                        objSocietyModel.InsertFilePathAndOther(fullFilePath[i].ToString(), Dr["RulesRegulationFile"].ToString(), Dr["ObjectsFile"].ToString(), Dr["SocietyID"].ToString());
                }


            }



        }
        [HttpGet]
        [ActionName("AAdharOne")]
        public string AAdharOne(string AAdharENC = "0")
        {
            var newdata = COMMON_FUNCTION.Decrypt(AAdharENC);
            return newdata;

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
                //    //aadhaarId = "940396382715";
                //    _LoggedInUser.AadharNo = "420928155572";
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
                //SSOWSSoap.SSOUserDetail usrdtl = new SSOWSSoap.SSOUserDetail();
                //SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
                //usrdtl = ssoclient.GetUserDetailXML(_LoggedInUser.SSOID, "RAJSAHKAR.TEST", "Rajasthan@12");
                //return COMMON_FUNCTION.GenerateOTPForEsingh(_LoggedInUser.AadharNo);
                return COMMON_FUNCTION.GenerateOTPForEsingh("940396382715");
            }
            else
            {
                return "LOGOUT";
            }
            //return "RJVSD2343246566";	
        }

        [HttpGet]
        public JsonResult VerifyAadhaarEsignOTP(string OTP, string TXN)
        {
            return Json(COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SocietyApplicationForm(string IsNewReg = "0", bool IsUpdate = false)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ViewBag.usertype = _LoggedInUser.UserType;
            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null && Request.Cookies["SSOROLE"].Value == "KIOSK")
            {
                if (IsUpdate)
                {
                    ViewBag.IsUpdateYes = 1;
                }
                else
                {
                    ViewBag.IsUpdateYes = 0;
                    if (IsNewReg == "1")
                    {
                        ViewBag.Serviceid = "4172";//NEW REG
                    }
                    else
                    {
                        ViewBag.Serviceid = "4209";//PROFILE
                    }
                }

            }
            if (_LoggedInUser.IsFromSWC == "YES")
            {
                SWSEntity sWSEntity = JsonConvert.DeserializeObject<SWSEntity>(System.Web.HttpContext.Current.Session[SessionIdentity.SWCSData].ToString());
                ViewBag.IsFromSWC = "Yes";
                ViewBag.ActId = sWSEntity.ActID;
                ViewBag.BRN = sWSEntity.BRN;
                ViewBag.SocietyName = sWSEntity.Establishmentname;
                ViewBag.SWCSData = System.Web.HttpContext.Current.Session[SessionIdentity.SWCSData];
            }
            else
            {

                ViewBag.ActId = "";
                ViewBag.BRN = "";
                ViewBag.SocietyName = "";
            }


            //return View("Inedx");
            return View();
        }

        public ActionResult SocietyApplicationForm1(string IsNewReg = "0", bool IsUpdate = false)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ViewBag.usertype = _LoggedInUser.UserType;
            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null && Request.Cookies["SSOROLE"].Value == "KIOSK")
            {
                if (IsUpdate)
                {
                    ViewBag.IsUpdateYes = 1;
                }
                else
                {
                    ViewBag.IsUpdateYes = 0;
                    if (IsNewReg == "1")
                    {
                        ViewBag.Serviceid = "4172";//NEW REG
                    }
                    else
                    {
                        ViewBag.Serviceid = "4209";//PROFILE
                    }

                }

            }
            if (_LoggedInUser.IsFromSWC == "YES")
            {
                ViewBag.IsFromSWC = "Yes";
                ViewBag.SWCSData = System.Web.HttpContext.Current.Session[SessionIdentity.SWCSData];
            }


            return View();
        }

        public ActionResult SocietyProfiling(string IsNewReg = "0", bool IsUpdate = false)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ViewBag.usertype = _LoggedInUser.UserType;
            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null && Request.Cookies["SSOROLE"].Value == "KIOSK")
            {
                if (IsUpdate)
                {
                    ViewBag.IsUpdateYes = 1;
                }
                else
                {
                    ViewBag.IsUpdateYes = 0;
                    if (IsNewReg == "1")
                    {
                        ViewBag.Serviceid = "4172";//NEW REG
                    }
                    else
                    {
                        ViewBag.Serviceid = "4209";//PROFILE
                    }

                }

            }


            return View();
        }

        public ActionResult SocietyApplicationForm2(string IsNewReg = "0", bool IsUpdate = false)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ViewBag.usertype = _LoggedInUser.UserType;
            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null && Request.Cookies["SSOROLE"].Value == "KIOSK")
            {
                if (IsUpdate)
                {
                    ViewBag.IsUpdateYes = 1;
                }
                else
                {
                    ViewBag.IsUpdateYes = 0;
                    if (IsNewReg == "1")
                    {
                        ViewBag.Serviceid = "4172";//NEW REG
                    }
                    else
                    {
                        ViewBag.Serviceid = "4209";//PROFILE
                    }

                }

            }
            if (_LoggedInUser.IsFromSWC == "YES")
            {
                ViewBag.IsFromSWC = "Yes";
                ViewBag.SWCSData = System.Web.HttpContext.Current.Session[SessionIdentity.SWCSData];
            }


            return View();
        }

        [HttpPost]
        public JsonResult BGetSWCSData()
        {
            string data = System.Web.HttpContext.Current.Session[SessionIdentity.SWCSData].ToString();// JsonConvert.SerializeObject(System.Web.HttpContext.Current.Session[SessionIdentity.SWCSData]) ;
            SWSEntity objSWC = JsonConvert.DeserializeObject<SWSEntity>(data);
            SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
            objSWC.Userdetails = ssoclient.GetTokenDetail(objSWC.Userdetails);
            return Json(objSWC);
        }

        [HttpPost]
        [ActionName("SaveSocietyAdministrativeDetailsAmendment")]
        public string InsertSocietyAdministrativeDetailAmendments(SocietyAdministrativeEntity SocietyRegistrationData)
        {
            string flag = "-1";
            List<SocietyAdministrativeEntity> obj = new List<SocietyAdministrativeEntity>();
            CitizenModel objSocietyModel = new CitizenModel(new DBLogger());
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<AdministrativeData> objData = new List<AdministrativeData>();

            try
            {
                objData = Request.Form["DataArray"].ToString().Equals("[]") ? new List<AdministrativeData>() : (List<AdministrativeData>)s.Deserialize(Request.Form["DataArray"].ToString(), typeof(List<AdministrativeData>));
            }
            catch (Exception ex)
            {
                return CommonMessageFactory.Response("Provided input data is not in correct format.");
            }

            if (!ModelState.IsValid)
                return JsonModelError.Get(ModelState.Values);

            string fileName = string.Empty;


            flag = objSocietyModel.InsertSocietyAdministrativeDetailsAmendment(objData, SocietyRegistrationData.ClassA, SocietyRegistrationData.ClassB, SocietyRegistrationData.ClassC, SocietyRegistrationData.EMitraTransactionId);

            return flag;
            // return CommonMessageFactory.Response(CommonMessageFactory.Message(flag.Length > 10 ? 1 : 0, (Request.Cookies[SessionIdentity.Culture]).Value));
        }

        [HttpPost]
        [ActionName("GetBrnDetails")]
        public string GetBrnDetails(string BRN, string SocietyIdENC)
        {
            CitizenModel objSocietyModel = new CitizenModel(new FileLogger());
            string ReturnJSON = "";

            int IsPresent = (objSocietyModel.CheckSocietyByBRNAmendment(BRN, SocietyIdENC));
            if (IsPresent == 1)
            {
                ILogger objILogger = new DBLogger();
                bool IsValid = true;
                System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
                try
                {

                    //BRNService.brwsSoapClient OBJBRN = new BRNService.brwsSoapClient();
                    //string hashmd5, hashmd5salt, hashdoubelmd5, Bas64;
                    //Random rnd = new Random();
                    //int RandomNo = rnd.Next(10000000, 99999999);

                    //hashmd5 = COMMON_FUNCTION.CalculateMD5Hash("Coop$3667");

                    //hashmd5salt = RandomNo.ToString() + hashmd5;

                    //hashdoubelmd5 = COMMON_FUNCTION.CalculateMD5Hash(hashmd5salt);

                    //Bas64 = COMMON_FUNCTION.Base64Encode(hashdoubelmd5);

                    //string Xmlstr = OBJBRN.GetBRNDetailXML("rajcoop", Bas64, RandomNo.ToString(), BRN).ToString();

                    //XmlDocument doc = new XmlDocument();
                    //doc.LoadXml(Xmlstr);
                    //XmlNode str = doc.DocumentElement;

                    BRNEntity obj = new BRNEntity();
                    obj = COMMON_FUNCTION.GetBRN(BRN);


                    //DataTable DT = new SocietyModel().GetLovValuesByListId(1);
                    //foreach (DataRow Dr in DT.Rows)
                    //{
                    //    if (str.SelectSingleNode("BO_Name").InnerText.Contains(Dr["LovCode"].ToString()) || str.SelectSingleNode("BO_Name").InnerText.Contains(Dr["Lovdescription"].ToString()))
                    //    {
                    //        str.SelectSingleNode("BO_Name").InnerText = str.SelectSingleNode("BO_Name").InnerText + " Due To this " + Dr["LovCode"].ToString() + " / " + Dr["Lovdescription"].ToString() + " Contains";
                    //        IsValid = false;
                    //        break;
                    //    }
                    //}


                    if (obj.Status.ToString() == "1" && IsValid)
                    {
                        ReturnJSON = "{\"result\":\"" + "success" + "\",\"BRN\":\"" + obj.BRN + "\",\"Applicant_No\":\"" + obj.Applicant_No + "\",\"BO_Name\":\"" + obj.BO_Name + "\"}";
                    }
                    else if (obj.Status == "1" && !IsValid)
                    {
                        ReturnJSON = "{\"result\":\"" + "invalidname" + "\",\"BRN\":\"" + obj.BRN + "\",\"Applicant_No\":\"" + obj.Applicant_No + "\",\"BO_Name\":\"" + obj.BO_Name + "\"}";
                    }
                    else
                    {
                        ReturnJSON = "{\"result\":\"" + "failed" + "\",\"BRN\":\"" + "Invalid BRN no." + "\"}";
                    }


                    //throw new Exception(Xmlstr);

                }
                catch (Exception ex)
                {
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SOCIETY", FunctionName = "GetBrnDetails()" }, ex);
                    ReturnJSON = "{\"result\":\"" + "failed" + "\",\"BRN\":\"" + "BRN Services Temporarily Down Please Try After Some Time." + "\"}";
                }
            }
            else
            {
                ReturnJSON = "{\"result\":\"" + "failed" + "\",\"BRN\":\"" + "BRN number is already exist." + "\"}";
            }

            return ReturnJSON;
        }

        [HttpPost]
        [ActionName("InsertSocietyNameChange")]
        public JsonResult InsertSocietyNameChange(SocietyAmendmentEntity SocietyAmendment, SocietyAdministrativeEntity SocietyRegistrationData, List<AmendmentDocuments> AmendmentDocuments, List<AdministrativeData> MemberData)
        {
            try
            {
                string flag = "-1";
                List<SocietyAdministrativeEntity> obj = new List<SocietyAdministrativeEntity>();
                CitizenModel objSocietyModel = new CitizenModel(new DBLogger());
                System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
                //List<AdministrativeData> objData = new List<AdministrativeData>();
                //List<AmendmentDocuments> amendmentDoc = new List<AmendmentDocuments>();


                if (SocietyAmendment.AmendmentType.IndexOf("2") > -1)
                {
                    DataTable DT = new SocietyModel().GetLovValuesByListId(1);

                    foreach (DataRow Dr in DT.Rows)
                    {

                        string[] validValue = new string[] { "government", "राजकीय", "state", "केंद्रीय", "central" };
                        string LovCode = Dr["LovCode"].ToString().Replace("\n", "").Replace("\r", "").ToUpper();
                        string Lovdescription = Dr["Lovdescription"].ToString().Replace("\n", "").Replace("\r", "").ToUpper();
                        string SocietyName = SocietyRegistrationData.Name.ToUpper();
                        if ((SocietyAmendment.SocietyTypeId == 5 || SocietyAmendment.SocietyTypeId == 11) && (validValue.Contains(LovCode.ToLower()) || validValue.Contains(Lovdescription.ToLower())))
                        {

                        }
                        else if (SocietyName.ToString().Contains(LovCode) || SocietyName.ToString().Contains(Lovdescription))
                        {
                            return Json(new { data = "NotValidName:-" + LovCode + "/" + Lovdescription });
                            //return CommonMessageFactory.Response("NotValidName:-" + LovCode + " , " + Lovdescription);
                        }
                    }
                }


                ByeLawsMst1 ObjAGM_BODM_Mst = new ByeLawsMst1();
                ObjAGM_BODM_Mst.ByeLawsMstID = 0;
                ObjAGM_BODM_Mst.SocietyId = 0;

                ObjAGM_BODM_Mst.SocietyIdEnc = SocietyAmendment.SocietyIdEnc;
                ObjAGM_BODM_Mst.AmendmentId = SocietyAmendment.AmendmentId;
                ObjAGM_BODM_Mst.RegiYearId = SocietyAmendment.RegiYearId;
                ObjAGM_BODM_Mst.ByeLawsFile = SocietyAmendment.ByeLawsFile;
                ObjAGM_BODM_Mst.ResolutionNo = SocietyAmendment.ResolutionNo;
                ObjAGM_BODM_Mst.ResolutionFile = SocietyAmendment.ResolutionFile;
                ObjAGM_BODM_Mst.PaasedAGMDate = SocietyAmendment.PaasedAGMDate;
                ObjAGM_BODM_Mst.AGM1File = SocietyAmendment.AGM1File;
                ObjAGM_BODM_Mst.AGM2File = SocietyAmendment.AGM2File;
                ObjAGM_BODM_Mst.AGM3File = SocietyAmendment.AGM3File;
                ObjAGM_BODM_Mst.ComparisonFile = SocietyAmendment.ComparisonFile;


                ObjAGM_BODM_Mst.flAGMDeliveryProof = SocietyAmendment.flAGMDeliveryProof;
                ObjAGM_BODM_Mst.flSGMNotice = SocietyAmendment.flSGMNotice;
                ObjAGM_BODM_Mst.flSGMAttendanceSheet = SocietyAmendment.flSGMAttendanceSheet;
                ObjAGM_BODM_Mst.flByLowComparativeDocument = SocietyAmendment.flByLowComparativeDocument;
                ObjAGM_BODM_Mst.flAttendanceSheet = SocietyAmendment.flAttendanceSheet;
                ObjAGM_BODM_Mst.flResoloutionNotice = SocietyAmendment.flResoloutionNotice;
                ObjAGM_BODM_Mst.flNameChangeNoticePDF = SocietyAmendment.flNameChangeNoticePDF;

                ObjAGM_BODM_Mst.HouseBuildingNo = SocietyAmendment.HouseBuildingNo;
                ObjAGM_BODM_Mst.LaneStreetName = SocietyAmendment.LaneStreetName;
                ObjAGM_BODM_Mst.Locality = SocietyAmendment.Locality;
                ObjAGM_BODM_Mst.IsRural = SocietyAmendment.IsRural;
                ObjAGM_BODM_Mst.STATE_ID = SocietyAmendment.STATE_ID;
                ObjAGM_BODM_Mst.DIVISION_ID = SocietyAmendment.DIVISION_ID;
                ObjAGM_BODM_Mst.DISTRICT_ID = SocietyAmendment.DISTRICT_ID;
                ObjAGM_BODM_Mst.TEHSIL_ID = SocietyAmendment.TEHSIL_ID;
                ObjAGM_BODM_Mst.CityTypes = SocietyAmendment.CityTypes;
                ObjAGM_BODM_Mst.CITY_ID = SocietyAmendment.CITY_ID;
                ObjAGM_BODM_Mst.BLOCK_ID = SocietyAmendment.BLOCK_ID;
                ObjAGM_BODM_Mst.WARD_ID = SocietyAmendment.WARD_ID;
                ObjAGM_BODM_Mst.GP_ID = SocietyAmendment.GP_ID;
                ObjAGM_BODM_Mst.VILLAGE_ID = SocietyAmendment.VILLAGE_ID;
                ObjAGM_BODM_Mst.PinCode = SocietyAmendment.PinCode;
                ObjAGM_BODM_Mst.AmendmentType = SocietyAmendment.AmendmentType;

                ObjAGM_BODM_Mst.BRN = SocietyAmendment.BRN;

                ObjAGM_BODM_Mst.TotalMember = SocietyAmendment.TotalMember;
                ObjAGM_BODM_Mst.TotalPresantMember = SocietyAmendment.TotalPresantMember;
                ObjAGM_BODM_Mst.TotalVoteInOpposition = SocietyAmendment.TotalVoteInOpposition;
                ObjAGM_BODM_Mst.TotalVoteInSupport = SocietyAmendment.TotalVoteInSupport;
                ObjAGM_BODM_Mst.MeetingQuorum = SocietyAmendment.MeetingQuorum;
                ObjAGM_BODM_Mst.Section = SocietyAmendment.Section;


                ObjAGM_BODM_Mst.Name = SocietyAmendment.Name;
                ObjAGM_BODM_Mst.Remark = SocietyAmendment.Remark;
                ObjAGM_BODM_Mst.InsertDate = DateTime.Now;
                ObjAGM_BODM_Mst.InsertBy = _LoggedInUser.UserId;
                ObjAGM_BODM_Mst.InsertIP = COMMON_FUNCTION.GetIpAddress();
                ObjAGM_BODM_Mst.IsPermanentSave = SocietyAmendment.IsPermanentSave;
                CitizenModel _citobj = new CitizenModel(new FileLogger());
                //if (string.IsNullOrWhiteSpace(ObjAGM_BODM_Mst.Name))
                //{
                //    return Json(_citobj.InsertByLaws(ObjAGM_BODM_Mst));
                //}
                //else
                //{
                return Json(_citobj.InsertSocietyNameChange(ObjAGM_BODM_Mst, MemberData, SocietyRegistrationData, AmendmentDocuments));
                //}
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen", FunctionName = " JsonResult InsertSocietyNameChange(SocietyAmendmentEntity SocietyAmendment)" }, ex);
                return Json("-1");
            }
        }
        [HttpPost]
        [ActionName("InsertSocietyNameChangeV1")]
        public JsonResult InsertSocietyNameChangeV1(SocietyAmendmentEntity SocietyAmendment, SocietyAdministrativeEntity SocietyRegistrationData, List<AmendmentDocuments> AmendmentDocuments, List<AdministrativeData> MemberData)
        {
            try
            {
                string flag = "-1";
                List<SocietyAdministrativeEntity> obj = new List<SocietyAdministrativeEntity>();
                CitizenModel objSocietyModel = new CitizenModel(new DBLogger());
                System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
                //List<AdministrativeData> objData = new List<AdministrativeData>();
                //List<AmendmentDocuments> amendmentDoc = new List<AmendmentDocuments>();

                /*Remove Special words check from system as request letter recieved dated 10-01-2022 from JR rules department*/
                //if (SocietyAmendment.AmendmentType.IndexOf("2") > -1)
                //{                    
                //    var isValidate = COMMON_FUNCTION.ValidateEmblemsAndName(SocietyRegistrationData.Name.ToUpper(), SocietyAmendment.SocietyTypeId);
                //    if (isValidate != "")
                //    {
                //        return Json(new { data = isValidate });
                //    }                    
                //}


                ByeLawsMst1 ObjAGM_BODM_Mst = new ByeLawsMst1();
                ObjAGM_BODM_Mst.ByeLawsMstID = 0;
                ObjAGM_BODM_Mst.SocietyId = 0;

                ObjAGM_BODM_Mst.SocietyIdEnc = SocietyAmendment.SocietyIdEnc;
                ObjAGM_BODM_Mst.AmendmentId = SocietyAmendment.AmendmentId;
                ObjAGM_BODM_Mst.RegiYearId = SocietyAmendment.RegiYearId;
                ObjAGM_BODM_Mst.ByeLawsFile = SocietyAmendment.ByeLawsFile;
                ObjAGM_BODM_Mst.ResolutionNo = SocietyAmendment.ResolutionNo;
                ObjAGM_BODM_Mst.ResolutionFile = SocietyAmendment.ResolutionFile;
                ObjAGM_BODM_Mst.PaasedAGMDate = SocietyAmendment.PaasedAGMDate;
                ObjAGM_BODM_Mst.AGM1File = SocietyAmendment.AGM1File;
                ObjAGM_BODM_Mst.AGM2File = SocietyAmendment.AGM2File;
                ObjAGM_BODM_Mst.AGM3File = SocietyAmendment.AGM3File;
                ObjAGM_BODM_Mst.ComparisonFile = SocietyAmendment.ComparisonFile;


                ObjAGM_BODM_Mst.flAGMDeliveryProof = SocietyAmendment.flAGMDeliveryProof;
                ObjAGM_BODM_Mst.flSGMNotice = SocietyAmendment.flSGMNotice;
                ObjAGM_BODM_Mst.flSGMAttendanceSheet = SocietyAmendment.flSGMAttendanceSheet;
                ObjAGM_BODM_Mst.flByLowComparativeDocument = SocietyAmendment.flByLowComparativeDocument;
                ObjAGM_BODM_Mst.flAttendanceSheet = SocietyAmendment.flAttendanceSheet;
                ObjAGM_BODM_Mst.flResoloutionNotice = SocietyAmendment.flResoloutionNotice;
                ObjAGM_BODM_Mst.flNameChangeNoticePDF = SocietyAmendment.flNameChangeNoticePDF;

                ObjAGM_BODM_Mst.HouseBuildingNo = SocietyAmendment.HouseBuildingNo;
                ObjAGM_BODM_Mst.LaneStreetName = SocietyAmendment.LaneStreetName;
                ObjAGM_BODM_Mst.Locality = SocietyAmendment.Locality;
                ObjAGM_BODM_Mst.IsRural = SocietyAmendment.IsRural;
                ObjAGM_BODM_Mst.STATE_ID = SocietyAmendment.STATE_ID;
                ObjAGM_BODM_Mst.DIVISION_ID = SocietyAmendment.DIVISION_ID;
                ObjAGM_BODM_Mst.DISTRICT_ID = SocietyAmendment.DISTRICT_ID;
                ObjAGM_BODM_Mst.TEHSIL_ID = SocietyAmendment.TEHSIL_ID;
                ObjAGM_BODM_Mst.CityTypes = SocietyAmendment.CityTypes;
                ObjAGM_BODM_Mst.CITY_ID = SocietyAmendment.CITY_ID;
                ObjAGM_BODM_Mst.BLOCK_ID = SocietyAmendment.BLOCK_ID;
                ObjAGM_BODM_Mst.WARD_ID = SocietyAmendment.WARD_ID;
                ObjAGM_BODM_Mst.GP_ID = SocietyAmendment.GP_ID;
                ObjAGM_BODM_Mst.VILLAGE_ID = SocietyAmendment.VILLAGE_ID;
                ObjAGM_BODM_Mst.PinCode = SocietyAmendment.PinCode;
                ObjAGM_BODM_Mst.AmendmentType = SocietyAmendment.AmendmentType;

                ObjAGM_BODM_Mst.BRN = SocietyAmendment.BRN;

                ObjAGM_BODM_Mst.TotalMember = SocietyAmendment.TotalMember;
                ObjAGM_BODM_Mst.TotalPresantMember = SocietyAmendment.TotalPresantMember;
                ObjAGM_BODM_Mst.TotalVoteInOpposition = SocietyAmendment.TotalVoteInOpposition;
                ObjAGM_BODM_Mst.TotalVoteInSupport = SocietyAmendment.TotalVoteInSupport;
                ObjAGM_BODM_Mst.MeetingQuorum = SocietyAmendment.MeetingQuorum;
                ObjAGM_BODM_Mst.Section = SocietyAmendment.Section;


                ObjAGM_BODM_Mst.Name = SocietyAmendment.Name;
                ObjAGM_BODM_Mst.Remark = SocietyAmendment.Remark;
                ObjAGM_BODM_Mst.InsertDate = DateTime.Now;
                ObjAGM_BODM_Mst.InsertBy = _LoggedInUser.UserId;
                ObjAGM_BODM_Mst.InsertIP = COMMON_FUNCTION.GetIpAddress();
                ObjAGM_BODM_Mst.IsPermanentSave = SocietyAmendment.IsPermanentSave;
                CitizenModel _citobj = new CitizenModel(new FileLogger());
                //if (string.IsNullOrWhiteSpace(ObjAGM_BODM_Mst.Name))
                //{
                //    return Json(_citobj.InsertByLaws(ObjAGM_BODM_Mst));
                //}
                //else
                //{
                if (MemberData != null)
                    foreach (var data in MemberData)
                    {
                        if (data.Photo.ToString().Contains("NewRegistration"))
                        {
                            var temp = data.Photo.ToString().Split('/');

                            if (!System.IO.Directory.Exists(Server.MapPath("../Content/SocietyProfiling/" + SocietyAmendment.SocietyDirectory + "/")))
                            {
                                System.IO.Directory.CreateDirectory(Server.MapPath("../Content/SocietyProfiling/" + SocietyAmendment.SocietyDirectory + "/"));
                            }
                            var newFilepath = Server.MapPath("../Content/SocietyProfiling/" + SocietyAmendment.SocietyDirectory + "/" + temp[temp.Length - 1].ToString());


                            try
                            {
                                if (temp.Length > 3 && !System.IO.File.Exists(newFilepath))
                                {
                                    System.IO.File.Copy(Server.MapPath(data.Photo), newFilepath, false);
                                    data.Photo = temp.Length > 3 ? temp[temp.Length - 1].ToString() : "";
                                }
                            }
                            catch (Exception ex)
                            {
                                DBLogger objDBLogger = new DBLogger();
                                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen.InsertSocietyNameChange", FunctionName = $"{data.Photo},new-{newFilepath}" }, ex);
                            }
                        }
                    }
                return Json(_citobj.InsertSocietyNameChangeV1(ObjAGM_BODM_Mst, MemberData, SocietyRegistrationData, AmendmentDocuments));
                //}
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen", FunctionName = " JsonResult InsertSocietyNameChange(SocietyAmendmentEntity SocietyAmendment)" }, ex);
                return Json("-1");
            }
        }

        [HttpPost]
        public string UploadAmendmentFiles(string SocietyDirectory )
        {
    
            try
            {
                //SocietyDirectory = "35BHARATREG.200001";
                SocietyDirectory = SocietyDirectory.Replace(" ", "").Replace("-", "").Replace("/", "").Replace("_", "");
                if (!System.IO.Directory.Exists(Server.MapPath("../Content/SocietyProfiling/" + SocietyDirectory + "/")))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("../Content/SocietyProfiling/" + SocietyDirectory + "/"));
                }
                string fileName = "";
                foreach (string files in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[files];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    if (Path.GetExtension(file.FileName).ToUpper() == ".PDF")
                    {
                        string qrcode = COMMON_FUNCTION.GenerateQRCode(Path.Combine(Server.MapPath("../Content/SocietyProfiling/" + SocietyDirectory + "/"), fileName), 1);
                        COMMON_FUNCTION.AddStampOnPDF(file, Path.Combine(Server.MapPath("../Content/SocietyProfiling/" + SocietyDirectory + "/"), fileName), qrcode);
                    }
                    else
                    {
                        file.SaveAs(Path.Combine(Server.MapPath("../Content/SocietyProfiling/" + SocietyDirectory + "/"), fileName));
                    }
                    //file.SaveAs(Path.Combine(Server.MapPath("../Content/SocietyProfiling/" + SocietyDirectory + "/"), fileName));
                }
                return fileName;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen", FunctionName = " JsonResult UploadAmendmentFiles(" + SocietyDirectory + ")" }, ex);
                return "";
            }
        }

        [HttpPost]
        public string UploadAmendmentOthersFiles(string SocietyDirectory, string DocumentTitle, string SocietyIDEnc, int AmendmentId)
        {
            try
            {
                SocietyDirectory = SocietyDirectory.Replace(" ", "").Replace("-", "").Replace("/", "").Replace("_", "");
                if (!System.IO.Directory.Exists(Server.MapPath("../Content/SocietyProfiling/" + SocietyDirectory + "/")))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("../Content/SocietyProfiling/" + SocietyDirectory + "/"));
                }
                string fileName = "";
                foreach (string files in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[files];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    if (Path.GetExtension(file.FileName).ToUpper() == ".PDF")
                    {
                        string qrcode = COMMON_FUNCTION.GenerateQRCode(Path.Combine(Server.MapPath("../Content/SocietyProfiling/" + SocietyDirectory + "/"), fileName), 1);
                        COMMON_FUNCTION.AddStampOnPDF(file, Path.Combine(Server.MapPath("../Content/SocietyProfiling/" + SocietyDirectory + "/"), fileName), qrcode);
                    }
                    else
                    {
                        file.SaveAs(Path.Combine(Server.MapPath("../Content/SocietyProfiling/" + SocietyDirectory + "/"), fileName));
                    }
                    (new Society()).AddEditSocietyAmendmentDocument(new SocietyAmendmentDocument() { DocumentType = DocumentTitle, DocumentName = fileName, AmendmentId = AmendmentId, CreatedBy = _LoggedInUser.SSOID, Type = "Other" }, SocietyIDEnc);
                }
                return fileName;



            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen", FunctionName = " JsonResult UploadAmendmentFiles(" + SocietyDirectory + ")" }, ex);
                return "";
            }
        }
        [HttpPost]
        public JsonResult DeleteSocietyAmendmentDocument(int Id)
        {
            return Json((new Society()).DeleteSocietyAmendmentDocument(Id));
        }

        #region Grievance

        public ActionResult Grievance()
        {
            SSOWSSoap.SSOUserDetail usrdtl = new SSOWSSoap.SSOUserDetail();
            SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
            usrdtl = ssoclient.GetUserDetailXML(Request.Cookies["SSO_ID"].Value, "RAJSAHKAR.TEST", "Rajasthan@12");
            ViewBag.MobileNo = usrdtl.mobile;
            return View();
        }

        [HttpPost]
        [ActionName("SaveGrievance")]
        public string SaveGrievance(Grievance _Entity)
        {
            _Entity.IsActive = 1;
            _Entity.CreatedBy = _LoggedInUser.UserRoleDeptMappingId;
            _Entity.CreatedIP = COMMON_FUNCTION.GetIpAddress();
            _Entity.SSO_ID = _LoggedInUser.SSOID;
            _Entity.OfficerNameEn = _LoggedInUser.OfficerNameEn;

            CitizenModel ObjCitizenModel = new CitizenModel(new FileLogger());
            return ObjCitizenModel.SaveGrievance(_Entity);

        }

        [HttpPost]
        [ActionName("GetGrievanceList")]
        public JsonResult GrievanceList(DataPagingModel Details)
        {
            CitizenModel _ObjCitizenModel = new CitizenModel(new FileLogger());
            DataTable DT = _ObjCitizenModel.GrievanceList(ref Details, _LoggedInUser);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }
        #endregion Grievance

        #region Rating
        [HttpPost]
        [ActionName("UserRatinginsert")]
        public JsonResult UserRatinginsert(postvarfname _postvarfname)
        {
            CitizenModel ObjCitizenModel = new CitizenModel(new DBLogger());
            int result = ObjCitizenModel.InserApplicationRating(_postvarfname);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [ActionName("GetUserRating")]
        public string GetUserRatingByid()
        {
            string _MasterData = "";
            CitizenModel ObjCitizenModel = new CitizenModel(new DBLogger());
            _MasterData = COMMON_FUNCTION.DataTableToJSONString(ObjCitizenModel.GetRatingUserby());
            return _MasterData;
        }
        #endregion Rating

        public ActionResult AddBankDetails()
        {
            return View();
        }

        public ActionResult SocietyAuditWorkSheet()
        {
            return View();
        }
        [HttpPost]
        public int SaveLetterDataDetails(LetterDataEntity objdata)
        {
            int result = 0;
            CitizenModel objMenuActionModel = new CitizenModel(new FileLogger());

            result = objMenuActionModel.SaveAuditInfomation(objdata);
            return result;
        }

        [HttpPost]
        public JsonResult FeePaymentCAAuditorRegistration(string AuditEmpanelledId)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            DataTable DT = null;
            string str = string.Empty;
            //string WhereClause = " and Societies.RegistrationNo='" + RegistrationNo + "'";
            EmitraModelFactory objEmitraModel = new EmitraModelFactory(new DBLogger());
            DT = objEmitraModel.GetCAAuditorPaymentDetails(AuditEmpanelledId);

            if (_LoggedInUser.UserType == "K")
            {

                str = objEmitraModel.FeeDepositForCAAuditor(DT.Rows[0]["AuditEmpanelledIdEnc"].ToString(), DT.Rows[0]["RegistrationNo"].ToString(), DT.Rows[0]["ApplicantNameEn"].ToString());

            }
            else
            {
                str = objEmitraModel.UpdateEmitrTransationForCAAuditor("", DT.Rows[0]["ApplicantNameEn"].ToString(), "0", DT.Rows[0]["RegistrationNo"].ToString(), DT.Rows[0]["AuditEmpanelledId"].ToString(), DT.Rows[0]["AuditEmpanelledIdEnc"].ToString(), "7392", false, "1", DT.Rows[0]["Amount"].ToString());
                //UpdateEmitrTransation
                //Response.Write(str);
            }

            return Json(new { status = "OK", UserType = _LoggedInUser.UserType, result = str }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult FeePaymentPLDBMembership(string MemberId)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            DataTable DT = null;
            string str = string.Empty;
            //string WhereClause = " and Societies.RegistrationNo='" + RegistrationNo + "'";
            EmitraModelFactory objEmitraModel = new EmitraModelFactory(new DBLogger());
            DT = objEmitraModel.GetPLDBMemberPaymentDetails(MemberId);

            if (_LoggedInUser.UserType == "K")
            {

                str = objEmitraModel.FeeDepositForPLDBMembership(DT.Rows[0]["RequestIdEnc"].ToString(), DT.Rows[0]["ApplicationNo"].ToString(), DT.Rows[0]["Name"].ToString(), DT.Rows[0]["OfficeCode"].ToString());

            }
            else
            {
                str = objEmitraModel.UpdateEmitrTransationForPLDBMembership("", DT.Rows[0]["Name"].ToString(), "0", DT.Rows[0]["ApplicationNo"].ToString(), DT.Rows[0]["RequestId"].ToString(), DT.Rows[0]["RequestIdEnc"].ToString(), "7670", DT.Rows[0]["OfficeCode"].ToString(), false, "1", DT.Rows[0]["Amount"].ToString());
                //UpdateEmitrTransation
                //Response.Write(str);
            }

            return Json(new { status = "OK", UserType = _LoggedInUser.UserType, result = str }, JsonRequestBehavior.AllowGet);

        }

        [AllowAnonymous]
        public ActionResult UpdatePaymentCAAuditorRegistration(string RegistrationNo = "", string UniquerequestId = "", string SocietyIdEnc = "", string IsFailed = "", string IsPermanentSave = "")
        {
            string encData = "";
            DataTable Dt = null;
            string decryptData = "";
            PGResponse objPGResponse = null;

            string DivHtml = "";

            UniquerequestId = UniquerequestId.Replace(' ', '+');
            UniquerequestId = UniquerequestId.Replace(' ', '+');
            UniquerequestId = UniquerequestId.Replace(' ', '+');

            SocietyIdEnc = SocietyIdEnc.Replace(' ', '+');
            SocietyIdEnc = SocietyIdEnc.Replace(' ', '+');
            SocietyIdEnc = SocietyIdEnc.Replace(' ', '+');

            IsFailed = IsFailed.Replace(' ', '+');
            IsFailed = IsFailed.Replace(' ', '+');
            IsFailed = IsFailed.Replace(' ', '+');


            IsPermanentSave = IsPermanentSave.Replace(' ', '+');
            IsPermanentSave = IsPermanentSave.Replace(' ', '+');
            IsPermanentSave = IsPermanentSave.Replace(' ', '+');

            UserModel OBJUser = new UserModel(new FileLogger());
            if (Request["encData"] != null)
            {
                EmitraModelFactory objEmitraModel = new EmitraModelFactory(new FileLogger());
                Dt = objEmitraModel.GetApplicationKey("7392", false);
                encData = Request["encData"].ToString().Replace("@!", "+");
                EncryptDecrypt3DES obj = new EncryptDecrypt3DES(Dt.Rows[0]["encryptionkey"].ToString());
                decryptData = obj.decrypt(encData);
                CitizenModel objCitizenModel = new CitizenModel(new FileLogger());
                objPGResponse = JsonConvert.DeserializeObject<PGResponse>(decryptData);
                Dt = new EmitraModelFactory(new FileLogger()).GetApplicationKey(objPGResponse.SERVICEID, true);

                DivHtml = objCitizenModel.UpdateOnlinePaymentStatus(RegistrationNo, SocietyIdEnc, COMMON_FUNCTION.Decrypt(UniquerequestId), COMMON_FUNCTION.Decrypt(IsFailed), decryptData, COMMON_FUNCTION.Decrypt(IsPermanentSave));

                objPGResponse = JsonConvert.DeserializeObject<PGResponse>(DivHtml);
                if (objPGResponse.RESPONSEMESSAGE.ToString().Contains("Successfully"))
                {

                    return RedirectToAction("SocietyApplicationView", "Society", new { SocietyIDEnc = SocietyIdEnc + "~Success^" + objPGResponse.RESPONSEMESSAGE });
                }
                else
                {
                    return RedirectToAction("SocietyApplicationView", "Society", new { SocietyIDEnc = SocietyIdEnc + "~Error^" + objPGResponse.RESPONSEMESSAGE });
                    //return RedirectToAction("MemberApplicationView", "Citizen", new { ApplicationIdEnc = "Error" + objPGResponse.RESPONSEMESSAGE });
                }
                //ViewBag.DivHtmlData = DivHtml;
            }
            return View();
            //return "OKAY";
        }

        [HttpGet]
        [ActionName("VerifyTransaction")]

        public string VerifyTransaction(SocietyRegistrationEntity ObjSocietyRegistration)
        {
            //ObjSocietyRegistration.EMitraAppRequestID = "12965";
            //ObjSocietyRegistration.ServicesId = 5570;

            string returnvalue = "";
            EmitraModelFactory OBJ = new EmitraModelFactory(new DBLogger());
            returnvalue = OBJ.VerifyTransaction(ObjSocietyRegistration);
            if (ObjSocietyRegistration.ServicesId == 5531 && returnvalue == "1")
            {
                string FResult = "";
                int Count = 0;
                SocietyController obj = new SocietyController();

                string str = obj.GetSocietyDetailsBySocietyId(ObjSocietyRegistration.SocietyIDEnc);
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(str, (typeof(DataTable)));
                if (dt.Rows[0]["RegistrationNo"] == null || dt.Rows[0]["RegistrationNo"].ToString().Length == 0)
                {
                    FResult = obj.SaveSocietyFinalSubmition(ObjSocietyRegistration.SocietyIDEnc);
                    var REG = FResult.Split('#');
                    if (REG.Length > 1)
                    {
                        foreach (DataRow Dr in dt.Rows)
                        {
                            Dr["SocietyDirectory"] = REG[1].ToString().Replace("/", "");
                        }
                    }
                }
                // else if (!dt.Rows[0]["RegistrationNo"].ToString().Contains("COOP"))
                else if (dt.Rows[0]["RegistrationNo"].ToString().Trim() == "")
                {
                    FResult = obj.SaveSocietyFinalSubmition(ObjSocietyRegistration.SocietyIDEnc);
                    var REG = FResult.Split('#');
                    if (REG.Length > 1)
                    {
                        foreach (DataRow Dr in dt.Rows)
                        {
                            Dr["SocietyDirectory"] = REG[1].ToString().Replace("/", "");
                        }
                    }
                }

                DataTable selectedTable = dt.AsEnumerable()
                    .Where(r => r.Field<string>("AddressFor") == "0")
                    .CopyToDataTable();

                dt = selectedTable;
                string pathString = "~/Content/SocietyProfiling/";
                string SourceFilePath = "";
                string TargetFilePath = "~/Content/SocietyProfiling/";
                foreach (DataRow Dr in dt.Rows)
                {
                    if (Count == 0)
                    {
                        pathString += Regex.Replace(dt.Rows[0]["SocietyDirectory"].ToString(), "[^a-zA-Z0-9_]+", "");

                        if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                        {
                            System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(pathString));
                        }

                        if (Dr["RegistrationCertificateFile"] != null && !Dr["RegistrationCertificateFile"].ToString().Contains("undefined") && Dr["RegistrationCertificateFile"].ToString() != "")
                        {
                            if ((Dr["RegistrationCertificateFile"].ToString().Split('/')).Length > 3)
                            {
                                var temp = Dr["RegistrationCertificateFile"].ToString().Split('/');
                                var TempRegistrationCertificateFile = temp[temp.Length - 1];
                                SourceFilePath = "~/Content/NewRegistration/" + TempRegistrationCertificateFile;
                                TargetFilePath = pathString + "/" + TempRegistrationCertificateFile;
                                if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)))
                                    System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(TargetFilePath));
                            }
                        }

                        if (Dr["BylawsFile"] != null && !Dr["BylawsFile"].ToString().Contains("undefined") && Dr["BylawsFile"].ToString() != "")
                        {
                            if ((Dr["BylawsFile"].ToString().Split('/')).Length > 3)
                            {
                                var temp = Dr["BylawsFile"].ToString().Split('/');
                                var TempBylawsFile = temp[temp.Length - 1];
                                SourceFilePath = "~/Content/NewRegistration/" + TempBylawsFile;
                                TargetFilePath = pathString + "/" + TempBylawsFile;
                                if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)))
                                    System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(TargetFilePath));
                            }
                        }

                        if (Dr["RulesAndRegulationFile"] != null && !Dr["RulesAndRegulationFile"].ToString().Contains("undefined") && Dr["RulesAndRegulationFile"].ToString() != "")
                        {
                            if ((Dr["RulesAndRegulationFile"].ToString().Split('/')).Length > 3)
                            {
                                var temp = Dr["RulesAndRegulationFile"].ToString().Split('/');
                                var TempRulesAndRegulationFile = temp[temp.Length - 1];
                                SourceFilePath = "~/Content/NewRegistration/" + TempRulesAndRegulationFile;
                                TargetFilePath = pathString + "/" + TempRulesAndRegulationFile;
                                if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)))
                                    System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(TargetFilePath));
                            }
                        }

                        if (Dr["MemberListFile"] != null && !Dr["MemberListFile"].ToString().Contains("undefined") && Dr["MemberListFile"].ToString() != "")
                        {
                            if ((Dr["MemberListFile"].ToString().Split('/')).Length > 3)
                            {
                                var temp = Dr["MemberListFile"].ToString().Split('/');
                                var TempMemberListFile = temp[temp.Length - 1];
                                SourceFilePath = "~/Content/NewRegistration/" + TempMemberListFile;
                                TargetFilePath = pathString + "/" + TempMemberListFile;
                                if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)))
                                    System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(TargetFilePath));
                            }
                        }

                        if (Dr["WorkPlanEcoViabilityFile"] != null && !Dr["WorkPlanEcoViabilityFile"].ToString().Contains("undefined") && Dr["WorkPlanEcoViabilityFile"].ToString() != "")
                        {
                            if ((Dr["WorkPlanEcoViabilityFile"].ToString().Split('/')).Length > 3)
                            {
                                var temp = Dr["WorkPlanEcoViabilityFile"].ToString().Split('/');
                                var TempWorkPlanEcoViabilityFile = temp[temp.Length - 1];
                                SourceFilePath = "~/Content/NewRegistration/" + TempWorkPlanEcoViabilityFile;
                                TargetFilePath = pathString + "/" + TempWorkPlanEcoViabilityFile;
                                if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)))
                                    System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(TargetFilePath));
                            }
                        }

                        if (Dr["GeneralMeetingProcedureFile"] != null && !Dr["GeneralMeetingProcedureFile"].ToString().Contains("undefined") && Dr["GeneralMeetingProcedureFile"].ToString() != "")
                        {
                            if ((Dr["GeneralMeetingProcedureFile"].ToString().Split('/')).Length > 3)
                            {
                                var temp = Dr["GeneralMeetingProcedureFile"].ToString().Split('/');
                                var TempGeneralMeetingProcedureFile = temp[temp.Length - 1];
                                SourceFilePath = "~/Content/NewRegistration/" + TempGeneralMeetingProcedureFile;
                                TargetFilePath = pathString + "/" + TempGeneralMeetingProcedureFile;
                                if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)))
                                    System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(TargetFilePath));
                            }
                        }

                        if (Dr["BalanceSheetExcelFile"] != null && !Dr["BalanceSheetExcelFile"].ToString().Contains("undefined") && Dr["BalanceSheetExcelFile"].ToString() != "")
                        {
                            if ((Dr["BalanceSheetExcelFile"].ToString().Split('/')).Length > 3)
                            {
                                var temp = Dr["GeneralMeetingProcedureFile"].ToString().Split('/');
                                var TempBalanceSheetExcelFile = temp[temp.Length - 1];
                                SourceFilePath = "~/Content/NewRegistration/" + TempBalanceSheetExcelFile;
                                TargetFilePath = pathString + "/" + TempBalanceSheetExcelFile;
                                if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)))
                                    System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(TargetFilePath));
                            }
                        }

                        if (Dr["ObjectsFile"] != null && !Dr["ObjectsFile"].ToString().Contains("undefined") && Dr["ObjectsFile"].ToString() != "")
                        {
                            if ((Dr["ObjectsFile"].ToString().Split('/')).Length > 3)
                            {
                                var temp = Dr["ObjectsFile"].ToString().Split('/');
                                var TempObjectsFile = temp[temp.Length - 1];
                                SourceFilePath = "~/Content/NewRegistration/" + TempObjectsFile;
                                TargetFilePath = pathString + "/" + TempObjectsFile;
                                if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)))
                                    System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(TargetFilePath));
                            }
                        }

                        if (Dr["SLSAFileName"] != null && !Dr["SLSAFileName"].ToString().Contains("undefined") && Dr["SLSAFileName"].ToString() != "")
                        {
                            if ((Dr["SLSAFileName"].ToString().Split('/')).Length > 3)
                            {
                                var temp = Dr["SLSAFileName"].ToString().Split('/');
                                var TempSLSAFileName = temp[temp.Length - 1];
                                SourceFilePath = "~/Content/NewRegistration/" + TempSLSAFileName;
                                TargetFilePath = pathString + "/" + TempSLSAFileName;
                                if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)))
                                    System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(TargetFilePath));
                            }
                        }

                        if (Dr["RSSCFileName"] != null && !Dr["RSSCFileName"].ToString().Contains("undefined") && Dr["RSSCFileName"].ToString() != "")
                        {
                            if ((Dr["RSSCFileName"].ToString().Split('/')).Length > 3)
                            {
                                var temp = Dr["RSSCFileName"].ToString().Split('/');
                                var TempRSSCFileName = temp[temp.Length - 1];
                                SourceFilePath = "~/Content/NewRegistration/" + TempRSSCFileName;
                                TargetFilePath = pathString + "/" + TempRSSCFileName;
                                if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)))
                                    System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(TargetFilePath));
                            }
                        }

                        if (Dr["DLSCFileName"] != null && !Dr["DLSCFileName"].ToString().Contains("undefined") && Dr["DLSCFileName"].ToString() != "")
                        {
                            if ((Dr["DLSCFileName"].ToString().Split('/')).Length > 3)
                            {
                                var temp = Dr["DLSCFileName"].ToString().Split('/');
                                var TempDLSCFileName = temp[temp.Length - 1];
                                SourceFilePath = "~/Content/NewRegistration/" + TempDLSCFileName;
                                TargetFilePath = pathString + "/" + TempDLSCFileName;
                                if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)))
                                    System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(TargetFilePath));
                            }
                        }

                        if (Dr["SOCFileName"] != null && !Dr["SOCFileName"].ToString().Contains("undefined") && Dr["SOCFileName"].ToString() != "")
                        {
                            if ((Dr["SOCFileName"].ToString().Split('/')).Length > 3)
                            {
                                var temp = Dr["SOCFileName"].ToString().Split('/');
                                var TempSOCFileName = temp[temp.Length - 1];
                                SourceFilePath = "~/Content/NewRegistration/" + TempSOCFileName;
                                TargetFilePath = pathString + "/" + TempSOCFileName;
                                if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)))
                                    System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(TargetFilePath));
                            }
                        }

                    }

                    if (Dr["Photo"] != null && Dr["Photo"].ToString() != "" && !Dr["Photo"].ToString().Contains("undefined") && !Dr["Photo"].ToString().Contains("Balnk"))
                    {
                        var temp = Dr["Photo"].ToString().Split('/');
                        if (temp.Length > 3)
                        {
                            var TempPhoto = temp[temp.Length - 1];
                            SourceFilePath = "~/Content/NewRegistration/" + TempPhoto;
                            TargetFilePath = pathString + "/" + TempPhoto;
                            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath)))
                                System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(SourceFilePath), System.Web.HttpContext.Current.Server.MapPath(TargetFilePath));
                        }
                    }
                    Count = Count + 1;
                }

            }

            return returnvalue;
        }

        public string GetTransactionFinalStatus(string SocietyIdEnc, string EMitraAppRequestID, string SERICEID, string UserType, string UniqueId = "0")
        {

            //EMitraAppRequestID = "172413";  //12345678
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            SocietyRegistrationEntity objSocietyRegistrationEntity = new SocietyRegistrationEntity();
            if (UserType == "C")
            {
                objSocietyRegistrationEntity.SSOID = "umeshcj.doit";
                objSocietyRegistrationEntity.UserType = "C";
                objSocietyRegistrationEntity.SocietyIDEnc = SocietyIdEnc;
                objSocietyRegistrationEntity.ServicesId = int.Parse(SERICEID);
                objSocietyRegistrationEntity.EMitraAppRequestID = EMitraAppRequestID;
            }
            if (UserType == "K")
            {
                objSocietyRegistrationEntity.UserType = "K";
                objSocietyRegistrationEntity.SocietyIDEnc = SocietyIdEnc;
                objSocietyRegistrationEntity.EMitraAppRequestID = EMitraAppRequestID;
                objSocietyRegistrationEntity.ServicesId = int.Parse(SERICEID);
                objSocietyRegistrationEntity.UniqueId = int.Parse(UniqueId);

            }

            //UserEntity _LoggedInUser = new UserEntity();
            //_LoggedInUser.SSOID= "umeshcj.doit";
            //_LoggedInUser.UserType = "C";
            //System.Web.HttpContext.Current.Session[SessionIdentity.Admin] = _LoggedInUser;
            CitizenController OBJ = new CitizenController();
            return OBJ.VerifyTransaction(objSocietyRegistrationEntity);
            //string Tbl = "";
            //Tbl = "<div class=\"col-lg-12 table-responsive\"><table class=\"table devtable\">" +
            //"<thead><tr><th class=\"ng-binding\">Column 1</th>" +
            //"<th class=\"ng-binding\">Column 2</th><th class=\"ng-binding\">Column 3</th>" +
            //"<th>Column 4</th></tr></thead><tbody>" +
            //"<tr><td class=\"ng-binding\">TRANSACTIONID</td><td class=\"ng-binding\">" + objPGResponse.TRANSACTIONID + "</td><td class=\"ng-binding\">RECEIPTNO</td><td class=\"ng-binding\">" + objPGResponse.RECEIPTNO + "</td></tr>" +
            //"<tr><td class=\"ng-binding\">PAIDAMOUNT</td><td class=\"ng-binding\">" + objPGResponse.PAIDAMOUNT + "</td><td class=\"ng-binding\">Date</td><td class=\"ng-binding\">" + objPGResponse.EMITRATIMESTAMP.Substring(0, 8) + "</td></tr>" +
            //"<tr><td class=\"ng-binding\">BANK_NAME</td><td class=\"ng-binding\">" + objPGResponse.BANK_NAME + "</td><td class=\"ng-binding\">BANK ACC NO.</td><td class=\"ng-binding\">" + objPGResponse.BANK_ACC_NO + "</td></tr>" +
            //"<tr><td class=\"ng-binding\">RESPONSEMESSAGE</td><td class=\"ng-binding\">" + objPGResponse.RESPONSEMESSAGE + "</td><td class=\"ng-binding\"></td><td class=\"ng-binding\"></td></tr>" +
            //"</tbody></table></div>";
            //return COMMON_FUNCTION.DataTableToJSONString(objCitizenModel.SocietyList(objSocietyRegistrationEntity));
        }

        public string RefundTransaction(string EMitraAppRequestID, string SERICEID, string UserType)
        {
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            SocietyRegistrationEntity objSocietyRegistrationEntity = new SocietyRegistrationEntity();
            if (UserType == "C")
            {
                objSocietyRegistrationEntity.SSOID = "umeshcj.doit";
                objSocietyRegistrationEntity.UserType = "C";
                //objSocietyRegistrationEntity.SocietyIDEnc = SocietyIdEnc;
                objSocietyRegistrationEntity.ServicesId = int.Parse(SERICEID);
                objSocietyRegistrationEntity.EMitraAppRequestID = EMitraAppRequestID;
            }
            if (UserType == "K")
            {
                objSocietyRegistrationEntity.UserType = "K";
                //objSocietyRegistrationEntity.SocietyIDEnc = SocietyIdEnc;
                //objSocietyRegistrationEntity.EMitraAppRequestID = EMitraAppRequestID;
                objSocietyRegistrationEntity.ServicesId = int.Parse(SERICEID);
                // objSocietyRegistrationEntity.UniqueId = int.Parse(UniqueId);

            }

            //UserEntity _LoggedInUser = new UserEntity();
            //_LoggedInUser.SSOID= "umeshcj.doit";
            //_LoggedInUser.UserType = "C";
            //System.Web.HttpContext.Current.Session[SessionIdentity.Admin] = _LoggedInUser;
            EmitraModelFactory OBJ = new EmitraModelFactory(new DBLogger());
            return OBJ.RefundTransaction(objSocietyRegistrationEntity);
        }



        public string GetTransaction(string SocietyIdEnc, string EMitraAppRequestID, string SERICEID, string RegistrationNo, string UniqueId = "0")
        {
            DataTable Dt = null;
            string returnval = "";
            VerifyResponse resp = null;
            PGResponse respg = null;
            string encData = "";
            string decryptData = "";
            PGResponse objPGResponse = null;

            string DivHtml = "";

            int i = -1;
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            SocietyRegistrationEntity objSocietyRegistrationEntity = new SocietyRegistrationEntity();
            if (_LoggedInUser.UserType == "C")
            {
                objSocietyRegistrationEntity.SSOID = _LoggedInUser.SSOID;
                objSocietyRegistrationEntity.UserType = _LoggedInUser.UserType;
                objSocietyRegistrationEntity.SocietyIDEnc = SocietyIdEnc;
                objSocietyRegistrationEntity.ServicesId = int.Parse(SERICEID);
                objSocietyRegistrationEntity.EMitraAppRequestID = EMitraAppRequestID;
            }
            if (_LoggedInUser.UserType == "K")
            {
                objSocietyRegistrationEntity.UserType = _LoggedInUser.UserType;
                objSocietyRegistrationEntity.SocietyIDEnc = SocietyIdEnc;
                objSocietyRegistrationEntity.EMitraAppRequestID = EMitraAppRequestID;
                objSocietyRegistrationEntity.ServicesId = int.Parse(SERICEID);
                objSocietyRegistrationEntity.UniqueId = int.Parse(UniqueId);

            }

            EmitraModelFactory objEmitra = new EmitraModelFactory(new DBLogger());
            string PayStatus = objEmitra.GetPaymentTransaction(objSocietyRegistrationEntity);

            if (PayStatus.ToUpper().Contains("SUCCESS"))
            {
                EmitraModelFactory objEmitraModel = new EmitraModelFactory(new FileLogger());
                Dt = objEmitraModel.GetApplicationKey("5531", false);
                encData = PayStatus;
                //EncryptDecrypt3DES obj = new EncryptDecrypt3DES(Dt.Rows[0]["encryptionkey"].ToString());
                decryptData = encData;// obj.decrypt(encData);
                                      // CitizenModel objCitizenModel = new CitizenModel(new FileLogger());
                objPGResponse = JsonConvert.DeserializeObject<PGResponse>(decryptData);
                Dt = new EmitraModelFactory(new FileLogger()).GetApplicationKey(objPGResponse.SERVICEID, true);

                DivHtml = objCitizenModel.UpdateOnlinePaymentStatus(RegistrationNo, SocietyIdEnc, EMitraAppRequestID, "NO", decryptData, "1");

                objPGResponse = JsonConvert.DeserializeObject<PGResponse>(DivHtml);
                if (objPGResponse.RESPONSEMESSAGE.ToString().Contains("Successfully") && objPGResponse.INTERNAL_ID > 0 && objPGResponse.SERVICEID == "5530")
                {
                    string FResult = "";
                    int Count = 0;
                    SocietyController objSociety = new SocietyController();

                    string str = objSociety.GetSocietyDetailsBySocietyId(SocietyIdEnc);
                    DataTable dt = (DataTable)JsonConvert.DeserializeObject(str, (typeof(DataTable)));
                    if (dt.Rows[0]["RegistrationNo"] == null || dt.Rows[0]["RegistrationNo"].ToString().Length == 0)
                    {
                        FResult = objSociety.SaveSocietyFinalSubmition(SocietyIdEnc);
                        var REG = FResult.Split('#');
                        if (REG.Length > 1)
                        {
                            foreach (DataRow Dr in dt.Rows)
                            {
                                Dr["SocietyDirectory"] = REG[1].ToString().Replace("/", "");
                            }
                        }
                    }
                    else if (!dt.Rows[0]["RegistrationNo"].ToString().Contains("COOP"))
                    {
                        FResult = objSociety.SaveSocietyFinalSubmition(SocietyIdEnc);
                        var REG = FResult.Split('#');
                        if (REG.Length > 1)
                        {
                            foreach (DataRow Dr in dt.Rows)
                            {
                                Dr["SocietyDirectory"] = REG[1].ToString().Replace("/", "");
                            }
                        }
                    }

                    DataTable selectedTable = dt.AsEnumerable()
                        .Where(r => r.Field<string>("AddressFor") == "0")
                        .CopyToDataTable();

                    dt = selectedTable;
                    string pathString = "~/Content/SocietyProfiling/";
                    string SourceFilePath = "";
                    string TargetFilePath = "~/Content/SocietyProfiling/";
                    foreach (DataRow Dr in dt.Rows)
                    {
                        if (Count == 0)
                        {
                            pathString += Regex.Replace(dt.Rows[0]["SocietyDirectory"].ToString(), "[^a-zA-Z0-9_]+", "");
                            if (!System.IO.Directory.Exists(Server.MapPath(pathString)))
                            {
                                System.IO.Directory.CreateDirectory(Server.MapPath(pathString));
                            }

                            if (Dr["RegistrationCertificateFile"] != null && !Dr["RegistrationCertificateFile"].ToString().Contains("undefined") && Dr["RegistrationCertificateFile"].ToString() != "")
                            {
                                if ((Dr["RegistrationCertificateFile"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["RegistrationCertificateFile"].ToString().Split('/');
                                    var TempRegistrationCertificateFile = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempRegistrationCertificateFile;
                                    TargetFilePath = pathString + "/" + TempRegistrationCertificateFile;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }

                            if (Dr["BylawsFile"] != null && !Dr["BylawsFile"].ToString().Contains("undefined") && Dr["BylawsFile"].ToString() != "")
                            {
                                if ((Dr["BylawsFile"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["BylawsFile"].ToString().Split('/');
                                    var TempBylawsFile = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempBylawsFile;
                                    TargetFilePath = pathString + "/" + TempBylawsFile;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }

                            if (Dr["RulesAndRegulationFile"] != null && !Dr["RulesAndRegulationFile"].ToString().Contains("undefined") && Dr["RulesAndRegulationFile"].ToString() != "")
                            {
                                if ((Dr["RulesAndRegulationFile"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["RulesAndRegulationFile"].ToString().Split('/');
                                    var TempRulesAndRegulationFile = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempRulesAndRegulationFile;
                                    TargetFilePath = pathString + "/" + TempRulesAndRegulationFile;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }


                            if (Dr["MemberListFile"] != null && !Dr["MemberListFile"].ToString().Contains("undefined") && Dr["MemberListFile"].ToString() != "")
                            {
                                if ((Dr["MemberListFile"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["MemberListFile"].ToString().Split('/');
                                    var TempMemberListFile = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempMemberListFile;
                                    TargetFilePath = pathString + "/" + TempMemberListFile;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }


                            if (Dr["WorkPlanEcoViabilityFile"] != null && !Dr["WorkPlanEcoViabilityFile"].ToString().Contains("undefined") && Dr["WorkPlanEcoViabilityFile"].ToString() != "")
                            {
                                if ((Dr["WorkPlanEcoViabilityFile"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["WorkPlanEcoViabilityFile"].ToString().Split('/');
                                    var TempWorkPlanEcoViabilityFile = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempWorkPlanEcoViabilityFile;
                                    TargetFilePath = pathString + "/" + TempWorkPlanEcoViabilityFile;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }

                            if (Dr["GeneralMeetingProcedureFile"] != null && !Dr["GeneralMeetingProcedureFile"].ToString().Contains("undefined") && Dr["GeneralMeetingProcedureFile"].ToString() != "")
                            {
                                if ((Dr["GeneralMeetingProcedureFile"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["GeneralMeetingProcedureFile"].ToString().Split('/');
                                    var TempGeneralMeetingProcedureFile = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempGeneralMeetingProcedureFile;
                                    TargetFilePath = pathString + "/" + TempGeneralMeetingProcedureFile;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }

                            if (Dr["BalanceSheetExcelFile"] != null && !Dr["BalanceSheetExcelFile"].ToString().Contains("undefined") && Dr["BalanceSheetExcelFile"].ToString() != "")
                            {
                                if ((Dr["BalanceSheetExcelFile"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["GeneralMeetingProcedureFile"].ToString().Split('/');
                                    var TempBalanceSheetExcelFile = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempBalanceSheetExcelFile;
                                    TargetFilePath = pathString + "/" + TempBalanceSheetExcelFile;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }

                            if (Dr["ObjectsFile"] != null && !Dr["ObjectsFile"].ToString().Contains("undefined") && Dr["ObjectsFile"].ToString() != "")
                            {
                                if ((Dr["ObjectsFile"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["ObjectsFile"].ToString().Split('/');
                                    var TempObjectsFile = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempObjectsFile;
                                    TargetFilePath = pathString + "/" + TempObjectsFile;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }

                            if (Dr["SLSAFileName"] != null && !Dr["SLSAFileName"].ToString().Contains("undefined") && Dr["SLSAFileName"].ToString() != "")
                            {
                                if ((Dr["SLSAFileName"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["SLSAFileName"].ToString().Split('/');
                                    var TempSLSAFileName = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempSLSAFileName;
                                    TargetFilePath = pathString + "/" + TempSLSAFileName;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }
                            if (Dr["RSSCFileName"] != null && !Dr["RSSCFileName"].ToString().Contains("undefined") && Dr["RSSCFileName"].ToString() != "")
                            {
                                if ((Dr["RSSCFileName"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["RSSCFileName"].ToString().Split('/');
                                    var TempRSSCFileName = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempRSSCFileName;
                                    TargetFilePath = pathString + "/" + TempRSSCFileName;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }
                            if (Dr["DLSCFileName"] != null && !Dr["DLSCFileName"].ToString().Contains("undefined") && Dr["DLSCFileName"].ToString() != "")
                            {
                                if ((Dr["DLSCFileName"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["DLSCFileName"].ToString().Split('/');
                                    var TempDLSCFileName = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempDLSCFileName;
                                    TargetFilePath = pathString + "/" + TempDLSCFileName;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }
                            if (Dr["SOCFileName"] != null && !Dr["SOCFileName"].ToString().Contains("undefined") && Dr["SOCFileName"].ToString() != "")
                            {
                                if ((Dr["SOCFileName"].ToString().Split('/')).Length > 3)
                                {
                                    var temp = Dr["SOCFileName"].ToString().Split('/');
                                    var TempSOCFileName = temp[temp.Length - 1];
                                    SourceFilePath = "~/Content/NewRegistration/" + TempSOCFileName;
                                    TargetFilePath = pathString + "/" + TempSOCFileName;
                                    if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                        System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                }
                            }

                        }

                        if (Dr["Photo"] != null && Dr["Photo"].ToString() != "" && !Dr["Photo"].ToString().Contains("undefined") && !Dr["Photo"].ToString().Contains("Balnk"))
                        {
                            var temp = Dr["Photo"].ToString().Split('/');
                            if (temp.Length > 3)
                            {
                                var TempPhoto = temp[temp.Length - 1];
                                SourceFilePath = "~/Content/NewRegistration/" + TempPhoto;
                                TargetFilePath = pathString + "/" + TempPhoto;
                                if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                    System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                            }
                        }
                        Count = Count + 1;
                    }

                    //return RedirectToAction("SocietyApplicationView", "Society", new { SocietyIDEnc = SocietyIdEnc + "~Success^" + objPGResponse.RESPONSEMESSAGE });
                }
                else
                {
                    //return RedirectToAction("SocietyApplicationView", "Society", new { SocietyIDEnc = SocietyIdEnc + "~Error^" + objPGResponse.RESPONSEMESSAGE });
                    //return RedirectToAction("MemberApplicationView", "Citizen", new { ApplicationIdEnc = "Error" + objPGResponse.RESPONSEMESSAGE });
                }
            }
            return PayStatus;
        }

        [HttpPost]
        public string GetPaymentTransaction(string SocietyIdEnc, string EMitraAppRequestID, string SERICEID, string RegistrationNo, int ApplicationStatus = 0, string UniqueId = "0", string UserType = "C")
        {

            DataTable Dt = null;
            string returnval = "";
            VerifyResponse resp = null;
            PGResponse respg = null;
            string encData = "";
            string decryptData = "";
            PGResponse objPGResponse = null;

            string DivHtml = "";


            int i = -1;
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            SocietyRegistrationEntity objSocietyRegistrationEntity = new SocietyRegistrationEntity();

            //if (_LoggedInUser.UserType == "K")
            //{
            objSocietyRegistrationEntity.UserType = UserType;
            objSocietyRegistrationEntity.SocietyIDEnc = SocietyIdEnc;
            objSocietyRegistrationEntity.EMitraAppRequestID = EMitraAppRequestID;
            objSocietyRegistrationEntity.ServicesId = int.Parse(SERICEID);
            objSocietyRegistrationEntity.UniqueId = int.Parse(UniqueId);

            //}
            //else
            //{
            //    objSocietyRegistrationEntity.SSOID = _LoggedInUser.SSOID;
            //    objSocietyRegistrationEntity.UserType = "C";
            //    objSocietyRegistrationEntity.SocietyIDEnc = SocietyIdEnc;
            //    objSocietyRegistrationEntity.ServicesId = int.Parse(SERICEID);
            //    objSocietyRegistrationEntity.EMitraAppRequestID = EMitraAppRequestID;
            //}

            EmitraModelFactory objEmitra = new EmitraModelFactory(new DBLogger());
            string PayStatus = objEmitra.GetPaymentTransaction(objSocietyRegistrationEntity);
            //{ "SocietyIdENC": '0x02000000CE78F2A1DDC20B632D4D0E97E9F2255A90F69E72E99553DD4530CD9C802DCB57EA805BA6BF128AB43D056E6F4F7B438B', "EMitraAppRequestID": '400136', "SERICEID": 5531, "UniqueId": 402301, "RegistrationNo": 'COOP/2021/DAUSA/200679' }
            //if (PayStatus.ToUpper().Contains("SUCCESS"))
            //{
            EmitraModelFactory objEmitraModel = new EmitraModelFactory(new FileLogger());
            //Dt = objEmitraModel.GetApplicationKey("5531", false);
            encData = PayStatus;
            //EncryptDecrypt3DES obj = new EncryptDecrypt3DES(Dt.Rows[0]["encryptionkey"].ToString());
            decryptData = encData;// obj.decrypt(encData);
                                  // CitizenModel objCitizenModel = new CitizenModel(new FileLogger());
                                  //objPGResponse = JsonConvert.DeserializeObject<PGResponse>(decryptData);
                                  //Dt = new EmitraModelFactory(new FileLogger()).GetApplicationKey(objPGResponse.SERVICEID, true);

            DivHtml = objCitizenModel.UpdateOnlinePaymentStatus(RegistrationNo, SocietyIdEnc, EMitraAppRequestID, "NO", decryptData, "1");

            objPGResponse = JsonConvert.DeserializeObject<PGResponse>(DivHtml);
            if (objPGResponse.STATUS != null)
            {
                if (objPGResponse.STATUS.ToString().Contains("SUCCESS"))
                {
                    if (ApplicationStatus == 33 || ApplicationStatus == 1081)//for sports state level -1081
                    {
                        ApplicationFlowModel AFM = new ApplicationFlowModel();
                        AFM.AddApplicationFlow(SocietyIdEnc, 15, "PAYMENT RECEIVED", 0);
                        AFM.SendMSG(SocietyIdEnc, 15);
                    }

                }
            }
            else
            {
                //return RedirectToAction("SocietyApplicationView", "Society", new { SocietyIDEnc = SocietyIdEnc + "~Error^" + objPGResponse.RESPONSEMESSAGE });
                //return RedirectToAction("MemberApplicationView", "Citizen", new { ApplicationIdEnc = "Error" + objPGResponse.RESPONSEMESSAGE });
            }
            //}
            return PayStatus;
        }


        [HttpPost]
        public string UpdateAudotorEmpaneledTransaction(string SocietyIdEnc, string EMitraAppRequestID, string SERICEID, string UniqueId = "0")
        {
            DataTable Dt = null;
            string returnval = "";
            VerifyResponse resp = null;
            PGResponse respg = null;
            string encData = "";
            string decryptData = "";
            PGResponse objPGResponse = null;

            string DivHtml = "";

            int i = -1;
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            SocietyRegistrationEntity objSocietyRegistrationEntity = new SocietyRegistrationEntity();

            if (_LoggedInUser.UserType == "K")
            {
                objSocietyRegistrationEntity.UserType = _LoggedInUser.UserType;
                objSocietyRegistrationEntity.SocietyIDEnc = SocietyIdEnc;
                objSocietyRegistrationEntity.EMitraAppRequestID = EMitraAppRequestID;
                objSocietyRegistrationEntity.ServicesId = int.Parse(SERICEID);
                objSocietyRegistrationEntity.UniqueId = int.Parse(UniqueId);

            }
            else
            {
                objSocietyRegistrationEntity.SSOID = _LoggedInUser.SSOID;
                objSocietyRegistrationEntity.UserType = "C";
                objSocietyRegistrationEntity.SocietyIDEnc = SocietyIdEnc;
                objSocietyRegistrationEntity.ServicesId = int.Parse(SERICEID);
                objSocietyRegistrationEntity.EMitraAppRequestID = EMitraAppRequestID;
            }

            EmitraModelFactory objEmitra = new EmitraModelFactory(new DBLogger());
            string PayStatus = objEmitra.GetPaymentTransaction(objSocietyRegistrationEntity);

            //if (PayStatus.ToUpper().Contains("SUCCESS"))
            //{
            EmitraModelFactory objEmitraModel = new EmitraModelFactory(new FileLogger());
            //Dt = objEmitraModel.GetApplicationKey("5531", false);
            encData = PayStatus;
            //EncryptDecrypt3DES obj = new EncryptDecrypt3DES(Dt.Rows[0]["encryptionkey"].ToString());
            decryptData = encData;// obj.decrypt(encData);
                                  // CitizenModel objCitizenModel = new CitizenModel(new FileLogger());
                                  //objPGResponse = JsonConvert.DeserializeObject<PGResponse>(decryptData);
                                  //Dt = new EmitraModelFactory(new FileLogger()).GetApplicationKey(objPGResponse.SERVICEID, true);

            DivHtml = objCitizenModel.UpdateOnlinePaymentStatus("", SocietyIdEnc, EMitraAppRequestID, "NO", decryptData, "1");

            objPGResponse = JsonConvert.DeserializeObject<PGResponse>(DivHtml);
            if (objPGResponse.STATUS.ToString().Contains("SUCCESS"))
            {
                ApplicationFlowModel AFM = new ApplicationFlowModel();
                AFM.UpdateAudotorEmpaneledTransaction(SocietyIdEnc, "PAYMENT RECEIVED");
                //AFM.SendMSG(SocietyIdEnc, 15);
            }
            else
            {
                //return RedirectToAction("SocietyApplicationView", "Society", new { SocietyIDEnc = SocietyIdEnc + "~Error^" + objPGResponse.RESPONSEMESSAGE });
                //return RedirectToAction("MemberApplicationView", "Citizen", new { ApplicationIdEnc = "Error" + objPGResponse.RESPONSEMESSAGE });
            }
            //}
            return PayStatus;
        }

        [HttpPost]
        public string GetFeeVerification7670(string RequestId, string RequestIdENC, string EMitraAppRequestID, string SERICEID, string UniqueId = "0")
        {
            DataTable Dt = null;
            string returnval = "";
            VerifyResponse resp = null;
            PGResponse respg = null;
            string encData = "";
            string decryptData = "";
            PGResponse objPGResponse = null;

            string DivHtml = "";

            int i = -1;
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            SocietyRegistrationEntity objSocietyRegistrationEntity = new SocietyRegistrationEntity();

            if (_LoggedInUser.UserType == "K")
            {
                objSocietyRegistrationEntity.UserType = _LoggedInUser.UserType;
                objSocietyRegistrationEntity.SocietyIDEnc = RequestIdENC;
                objSocietyRegistrationEntity.EMitraAppRequestID = EMitraAppRequestID;
                objSocietyRegistrationEntity.ServicesId = int.Parse(SERICEID);
                objSocietyRegistrationEntity.UniqueId = int.Parse(UniqueId);

            }
            else
            {
                objSocietyRegistrationEntity.SSOID = _LoggedInUser.SSOID;
                objSocietyRegistrationEntity.UserType = "C";
                objSocietyRegistrationEntity.SocietyIDEnc = RequestIdENC;
                objSocietyRegistrationEntity.ServicesId = int.Parse(SERICEID);
                objSocietyRegistrationEntity.EMitraAppRequestID = EMitraAppRequestID;
            }

            EmitraModelFactory objEmitra = new EmitraModelFactory(new DBLogger());
            string PayStatus = objEmitra.GetPaymentTransaction(objSocietyRegistrationEntity);

            //if (PayStatus.ToUpper().Contains("SUCCESS"))
            //{
            EmitraModelFactory objEmitraModel = new EmitraModelFactory(new FileLogger());
            //Dt = objEmitraModel.GetApplicationKey("5531", false);
            encData = PayStatus;
            //EncryptDecrypt3DES obj = new EncryptDecrypt3DES(Dt.Rows[0]["encryptionkey"].ToString());
            decryptData = encData;// obj.decrypt(encData);
                                  // CitizenModel objCitizenModel = new CitizenModel(new FileLogger());
                                  //objPGResponse = JsonConvert.DeserializeObject<PGResponse>(decryptData);
                                  //Dt = new EmitraModelFactory(new FileLogger()).GetApplicationKey(objPGResponse.SERVICEID, true);

            DivHtml = objCitizenModel.UpdateOnlinePaymentStatus("", RequestIdENC, EMitraAppRequestID, "NO", decryptData, "1");

            objPGResponse = JsonConvert.DeserializeObject<PGResponse>(DivHtml);
            if (objPGResponse.STATUS.ToString().Contains("SUCCESS"))
            {
                var result = objCitizenModel.SetSocietyMemberRegistrationFinalSubmit(RequestId);
                if (result != "-1")
                {
                    string message = "Your membership application submitted successfully. Application No:-" + result.Split('#')[1];
                    new EsanchaarController().SendSocietySMS(message, result.Split('#')[0]);
                }

                //ApplicationFlowModel AFM = new ApplicationFlowModel();
                //AFM.UpdateAudotorEmpaneledTransaction(RequestIdENC, "PAYMENT RECEIVED");
                //AFM.SendMSG(SocietyIdEnc, 15);
            }
            else
            {
                //return RedirectToAction("SocietyApplicationView", "Society", new { SocietyIDEnc = SocietyIdEnc + "~Error^" + objPGResponse.RESPONSEMESSAGE });
                //return RedirectToAction("MemberApplicationView", "Citizen", new { ApplicationIdEnc = "Error" + objPGResponse.RESPONSEMESSAGE });
            }
            //}
            return PayStatus;
        }


        public string GetTransactionFinalStatus1(string SocietyIdEnc, string EMitraAppRequestID, string SERICEID, string UniqueId = "0")
        {

            //EMitraAppRequestID = "172413";  //12345678
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            SocietyRegistrationEntity objSocietyRegistrationEntity = new SocietyRegistrationEntity();

            if (_LoggedInUser.UserType == "K")
            {
                objSocietyRegistrationEntity.UserType = _LoggedInUser.UserType;
                objSocietyRegistrationEntity.SocietyIDEnc = SocietyIdEnc;
                objSocietyRegistrationEntity.EMitraAppRequestID = EMitraAppRequestID;
                objSocietyRegistrationEntity.ServicesId = int.Parse(SERICEID);
                objSocietyRegistrationEntity.UniqueId = int.Parse(UniqueId);

            }
            else
            {
                objSocietyRegistrationEntity.SSOID = _LoggedInUser.SSOID;
                objSocietyRegistrationEntity.UserType = _LoggedInUser.UserType;
                objSocietyRegistrationEntity.SocietyIDEnc = SocietyIdEnc;
                objSocietyRegistrationEntity.ServicesId = int.Parse(SERICEID);
                objSocietyRegistrationEntity.EMitraAppRequestID = EMitraAppRequestID;
            }

            EmitraController OBJ = new EmitraController();
            return OBJ.VerifyTransaction(objSocietyRegistrationEntity);
            //string Tbl = "";
            //Tbl = "<div class=\"col-lg-12 table-responsive\"><table class=\"table devtable\">" +
            //"<thead><tr><th class=\"ng-binding\">Column 1</th>" +
            //"<th class=\"ng-binding\">Column 2</th><th class=\"ng-binding\">Column 3</th>" +
            //"<th>Column 4</th></tr></thead><tbody>" +
            //"<tr><td class=\"ng-binding\">TRANSACTIONID</td><td class=\"ng-binding\">" + objPGResponse.TRANSACTIONID + "</td><td class=\"ng-binding\">RECEIPTNO</td><td class=\"ng-binding\">" + objPGResponse.RECEIPTNO + "</td></tr>" +
            //"<tr><td class=\"ng-binding\">PAIDAMOUNT</td><td class=\"ng-binding\">" + objPGResponse.PAIDAMOUNT + "</td><td class=\"ng-binding\">Date</td><td class=\"ng-binding\">" + objPGResponse.EMITRATIMESTAMP.Substring(0, 8) + "</td></tr>" +
            //"<tr><td class=\"ng-binding\">BANK_NAME</td><td class=\"ng-binding\">" + objPGResponse.BANK_NAME + "</td><td class=\"ng-binding\">BANK ACC NO.</td><td class=\"ng-binding\">" + objPGResponse.BANK_ACC_NO + "</td></tr>" +
            //"<tr><td class=\"ng-binding\">RESPONSEMESSAGE</td><td class=\"ng-binding\">" + objPGResponse.RESPONSEMESSAGE + "</td><td class=\"ng-binding\"></td><td class=\"ng-binding\"></td></tr>" +
            //"</tbody></table></div>";
            //return COMMON_FUNCTION.DataTableToJSONString(objCitizenModel.SocietyList(objSocietyRegistrationEntity));
        }

        public string GetPaymentStatus(string EMitraAppRequestID, string SERICEID, string UniqueId = "0", string SocietyIdEnc = "")
        {

            //EMitraAppRequestID = "172413";  //12345678
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            SocietyRegistrationEntity objSocietyRegistrationEntity = new SocietyRegistrationEntity();

            if (_LoggedInUser.UserType == "K")
            {
                objSocietyRegistrationEntity.UserType = _LoggedInUser.UserType;
                objSocietyRegistrationEntity.SocietyIDEnc = SocietyIdEnc;
                objSocietyRegistrationEntity.EMitraAppRequestID = EMitraAppRequestID;
                objSocietyRegistrationEntity.ServicesId = int.Parse(SERICEID);
                objSocietyRegistrationEntity.UniqueId = int.Parse(UniqueId);

            }
            else
            {
                objSocietyRegistrationEntity.SSOID = _LoggedInUser.SSOID;
                objSocietyRegistrationEntity.UserType = _LoggedInUser.UserType;
                objSocietyRegistrationEntity.SocietyIDEnc = SocietyIdEnc;
                objSocietyRegistrationEntity.ServicesId = int.Parse(SERICEID);
                objSocietyRegistrationEntity.EMitraAppRequestID = EMitraAppRequestID;
            }

            EmitraModelFactory objEmitraModel = new EmitraModelFactory(new FileLogger());
            return objEmitraModel.GetPaymentTransaction(objSocietyRegistrationEntity);
        }


        public ActionResult AmendmentSociety()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetSocietieForAmendment(int ActId, int AmendmentType)
        {
            SocietyModel objModel = new SocietyModel(new DBLogger());
            var data = objModel.GetSocietieForAmendment(ActId, _LoggedInUser.SSOID, AmendmentType);
            return Json(COMMON_FUNCTION.DataTableToJSONWithJSONNet(data), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOldSiteDirectories(string RegistrationNO)
        {

            string[] filePaths = System.IO.Directory.GetFiles(Server.MapPath("~//Content/SocietyProfiling/" + RegistrationNO + "_old"));
            List<string> Member = new List<string>();
            foreach (string x in filePaths)
            {

                string nfile = x.Split('\\')[x.Split('\\').Length - 1];
                Member.Add("/Content/SocietyProfiling/" + RegistrationNO + "/" + nfile);
            }

            return Json(Member);
        }


        [HttpGet]
        [ActionName("GetSSOUserDetails")]
        public string GetSSOUserDetails()
        {
            var jsonSerialiser = new JavaScriptSerializer();

            UserEntity UserRegistration = new UserEntity();
            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
            {
                try
                {
                    if (Request.Cookies["SSOROLE"].Value == "CITIZEN")
                    {
                        string SSOID = Request.Cookies["SSO_ID"].Value;
                        SSOWSSoap.SSOUserDetail usrdtl = new SSOWSSoap.SSOUserDetail();
                        SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
                        usrdtl = ssoclient.GetUserDetailXML(SSOID, "RAJSAHKAR.TEST", "Rajasthan@12");
                        if (usrdtl != null && usrdtl.SSOID.Length > 0)
                        {
                            //  UserRegistration.DateOfBirth = usrdtl.dateOfBirth.Length > 3 ? Convert.ToDateTime(usrdtl.dateOfBirth) : System.DateTime.MaxValue;
                            UserRegistration.OfficerNameEn = usrdtl.displayName;
                            UserRegistration.Gender = usrdtl.gender;
                            //UserRegistration.ImageUrl = usrdtl.jpegPhoto;
                            UserRegistration.SSOID = usrdtl.SSOID;
                            UserRegistration.AadharNo = usrdtl.aadhaarId;
                            UserRegistration.MobileNo = usrdtl.mobile;
                            UserRegistration.EmailId = usrdtl.mailPersonal;
                            UserRegistration.DepartmentEn = usrdtl.department;
                            UserRegistration.DesignationEn = usrdtl.designation;
                            UserRegistration.DepartmentId = int.Parse(usrdtl.departmentId == null ? "0" : usrdtl.departmentId);
                            UserRegistration.AddressEn = usrdtl.postalAddress;
                            UserRegistration.UnitId = 0;

                            string pathString = "~/SSOUSER/" + usrdtl.SSOID + ".jpg";
                            if (usrdtl.jpegPhoto != null)
                            {
                                try
                                {
                                    if (!System.IO.File.Exists(Server.MapPath(pathString)))
                                    {
                                        byte[] imageBytes = Convert.FromBase64String(usrdtl.jpegPhoto);
                                        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                        string fileName = usrdtl.SSOID;
                                        FileStream fs = new FileStream(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/SSOUSER/"), fileName + ".jpg"), FileMode.CreateNew, FileAccess.Write);
                                        ms.WriteTo(fs);
                                        fs.Close();
                                        fs.Dispose();
                                        ms.Flush();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ILogger objILogger = new DBLogger();
                                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.LoginController", FunctionName = "Inner Try of GetSSOUserDetails()" }, ex);
                                }

                            }
                        }
                        else
                        {
                            UserRegistration.SSOID = "Data Could Not Be Retrieved From The Server Please Try After Some Time";
                        }
                    }
                }
                catch (Exception ex)
                {
                    ILogger objILogger = new DBLogger();
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.LoginController", FunctionName = "GetSSOUserDetails()" }, ex);
                }
            }
            return jsonSerialiser.Serialize(UserRegistration);

        }

        public ActionResult GSSRelatedComplaints()
        {
            return View();
        }

        //[HttpPost]
        //[ActionName("SaveGssComplaint")]
        //public string SaveGssComplaint(GSSComplaint GssComplaint)
        //{

        //    CitizenModel obj = new CitizenModel(new DBLogger());
        //    int Result = obj.SaveGssComplaint(GssComplaint);
        //    return Result.ToString();
        //}


        [HttpPost]
        [ActionName("SaveGssComplaint")]
        public string SaveGssComplaint(GSSComplaintNew GssComplaint)
        {

            CitizenModel obj = new CitizenModel(new DBLogger());
            int Result = obj.SaveGssComplaint(GssComplaint);
            return Result.ToString();
        }


        [HttpPost]
        [ActionName("GetGssComplaintDetails")]
        public JsonResult GetGssComplaintDetails()
        {
            DataTable Dt = null;
            CitizenModel obj = new CitizenModel(new DBLogger());
            if (Request.Cookies["SSO_ID"] != null) { Dt = obj.GetGssComplaintDetails(Request.Cookies["SSO_ID"].Value); }

            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(Dt) }, JsonRequestBehavior.AllowGet);
        }



        #region Umesh sir Janaadhar Integration

        [HttpPost]
        public string GetJanAadhaarDetails_old(string JanaadhaarNo)
        {
            JanaadhaarNo = csCommon.Decrypt_SA(Uri.UnescapeDataString(JanaadhaarNo));
            var mid = "";
            if (Request.Url.Host.Contains("localhost1"))
            {
                mid = "31224592617";
                JanaadhaarNo = "557975711583";

            }

            //Bhamasah objBhamasah = new Bhamasah();
            csCommon objComm = new csCommon();

            DataSet dS = new DataSet();
            DataTable dt = new DataTable();
            dS = objComm.JanAadhaarMembersList(JanaadhaarNo, mid);
            // dS = objBhamasah.JanAadhaarMembersDetails(Request.Form["JanaadhaarNo"].ToString());
            try
            {
                if (dS != null && dS.Tables.Count > 0)
                {
                    dt = dS.Tables[0];
                    if (!dt.Columns.Contains("MaskedMid"))
                        dt.Columns.Add("MaskedMid", typeof(string));
                    if (!dt.Columns.Contains("ENC_AADHAR_REF_NO"))
                        dt.Columns.Add("ENC_AADHAR_REF_NO", typeof(string));

                    if (dt.Columns.Contains("JAN_MEMBER_ID"))
                        foreach (DataRow dr in dt.Rows) // search whole table
                        {
                            dr["MaskedMid"] = dr["JAN_MEMBER_ID"].ToString();
                            dr["ENC_AADHAR_REF_NO"] = COMMON_FUNCTION.Encrypt(dr["AADHAR_REF_NO"].ToString());
                            if (dr["JAN_MEMBER_ID"].ToString().Length > 8)
                            {
                                var Mid = dr["JAN_MEMBER_ID"].ToString();
                                Session["JAN_MEMBER_ID"] = dr["JAN_MEMBER_ID"].ToString();

                                var firstDigits = Mid.Substring(0, 5);
                                var lastDigits = Mid.Substring(Mid.Length - 4, 4);
                                var requiredMask = new String('X', Mid.Length - lastDigits.Length);
                                var maskedString = string.Concat(requiredMask, lastDigits);
                                var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0");
                                dr["MaskedMid"] = maskedString;
                                Session["JanaadhaarNo"] = COMMON_FUNCTION.Decrypt(dr["JAN_AADHAR"].ToString());   //JanaadhaarNo;---madhuri
                            }
                            if (dr["SRDR_MID"].ToString() == "0")
                            {
                                Session["HeadAadhar"] = dr["AADHAR_ID"];
                            }

                        }
                }

            }
            catch (Exception ex)
            {

                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen", FunctionName = "GetJanAadhaarDetails(string JanaadhaarNo) JanaadhaarNo:-" + JanaadhaarNo }, ex);
            }
            if (dt.Rows.Count > 0)
            {
                return COMMON_FUNCTION.DataTableToJSONString(dt);

            }
            else
            {
                dt = null;
                return COMMON_FUNCTION.DataTableToJSONString(dt);
            }


        }

        [HttpPost]
        public async Task<string> GetJanAadhaarDetails(string JanaadhaarNo)
        {
            JanaadhaarNo = csCommon.Decrypt_SA(Uri.UnescapeDataString(JanaadhaarNo));
            var mid = "";
            if (Request.Url.Host.Contains("localhost"))
            {
                mid = "31224592617";
                JanaadhaarNo = "557975711583";
            }

            csCommon objComm = new csCommon();

            DataTable dt = new DataTable();
            dt = await objComm.GetJanAadhaarMIDByAadhaar(JanaadhaarNo);
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (!dt.Columns.Contains("MaskedMid"))
                        dt.Columns.Add("MaskedMid", typeof(string));
                    if (!dt.Columns.Contains("ENC_AADHAR_REF_NO"))
                        dt.Columns.Add("ENC_AADHAR_REF_NO", typeof(string));
                    if (dt.Columns.Contains("JAN_AADHAR"))
                        dt.Columns.Remove("JAN_AADHAR");

                    if (dt.Columns.Contains("JAN_MEMBER_ID"))
                        foreach (DataRow dr in dt.Rows) // search whole table
                        {
                            dr["MaskedMid"] = dr["JAN_MEMBER_ID"].ToString();
                            dr["ENC_AADHAR_REF_NO"] = COMMON_FUNCTION.Encrypt(dr["AADHAR_REF_NO"].ToString());
                            if (dr["JAN_MEMBER_ID"].ToString().Length > 8)
                            {
                                var Mid = dr["JAN_MEMBER_ID"].ToString();
                                Session["JAN_MEMBER_ID"] = dr["JAN_MEMBER_ID"].ToString();

                                var firstDigits = Mid.Substring(0, 5);
                                var lastDigits = Mid.Substring(Mid.Length - 4, 4);
                                var requiredMask = new String('X', Mid.Length - lastDigits.Length);
                                var maskedString = string.Concat(requiredMask, lastDigits);
                                var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0");
                                dr["MaskedMid"] = maskedString;
                                Session["JanaadhaarNo"] = COMMON_FUNCTION.Decrypt(dr["ENC_JAN_AADHAR"].ToString());
                            }
                            //if (dr["SRDR_MID"].ToString() == "0")
                            //{
                            //    Session["HeadAadhar"] = dr["AADHAR_ID"];
                            //}

                        }
                }

            }
            catch (Exception ex)
            {

                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen", FunctionName = "GetJanAadhaarDetails(string JanaadhaarNo) JanaadhaarNo:-" + JanaadhaarNo }, ex);
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                return COMMON_FUNCTION.DataTableToJSONString(dt);

            }
            else
            {
                dt = null;
                return COMMON_FUNCTION.DataTableToJSONString(dt);
            }


        }

        [HttpPost]
        public string SendRecheckOTP_old(string AadharNo, string JanAadharId, string MemberId, string ENR_Id)
        {
            try
            {
                if (AadharNo != null && AadharNo != "")
                    AadharNo = csCommon.Decrypt_SA(Uri.UnescapeDataString(AadharNo));
                if (JanAadharId != null && JanAadharId != "")
                {
                    JanAadharId = csCommon.Decrypt_SA(Uri.UnescapeDataString(JanAadharId));
                    JanAadharId = COMMON_FUNCTION.Decrypt(JanAadharId);
                }
                DataSet dtts = new DataSet();

                if (Request.Url.Host.Contains("localhost"))
                {

                    string jsonString = @"[{ 'status': true, 'tid': '173999578' }]";

                    DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(jsonString);
                    dtts.Tables.Add(dataTable);

                }
                else
                {
                    dtts = csCommon.SendOtpByAadharNo_KYC(JanAadharId, MemberId);
                }

                DataTable dtt = new DataTable();
                if (dtts != null && dtts.Tables.Count != 0 && dtts.Tables[0].Rows.Count != 0)
                    dtt = dtts.Tables[0];



                if (dtt != null && dtt.Rows.Count > 0 && dtt.Columns.Contains("tid"))
                {
                    if (dtt.Rows[0]["tid"] == null)
                    {
                        return "Fail";
                    }
                    else if (dtt.Rows[0]["tid"].ToString().Contains("DOIT"))
                    {
                        Session["Aadhar"] = AadharNo;
                        Session["JanAadharId"] = JanAadharId;
                        Session["MemberId"] = MemberId;
                        //Session["ENR_Id"] = ENR_Id;

                        Session["mAadhar"] = AadharNo;
                        Session["mJanAadharId"] = JanAadharId;
                        Session["mMemberId"] = MemberId;
                        Session["mENR_Id"] = ENR_Id;


                        Session["tid"] = dtt.Rows[0]["tid"].ToString();
                        return "Success";

                    }
                    else
                    {
                        Session["tid"] = dtt.Rows[0]["tid"].ToString();
                        Session["mENR_Id"] = ENR_Id;
                        return Session["tid"].ToString();
                    }

                }
                else
                {
                    return "Fail";
                }
            }
            catch (Exception ex)
            {

                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen", FunctionName = "SendRecheckOTP(string \"" + AadharNo + "\", string \"" + JanAadharId + "\", string MemberId, string ENR_Id)" }, ex);
                return "Fail";
            }
        }

        public string VerifyRecheckOTP_old(string OTP, string SocietyDirectory = "")
        {
            try
            {
                //string _SocietyDirectory = "";
                if (Request.Url.Host.Contains("localhost"))
                {

                    return "[{\"Column1\":\"Success\",\"Column2\":\"Balwant Chaudhary\",\"Column3\":\"M\",\"Column4\":\"20-09-1984\",\"Column5\":\"../Content/NewRegistration/member_90b405c50c384ae5b3eb3ed25534594d.jpg\",\"Column6\":\"बलवंत चौधरी\",\"Column7\":\"T1+Bhe8Ju9s4QjBaM66sAg==\",\"Column8\":\"S/O: Mohan Lal Chaudhary\",\"Column9\":\"S/O: मोहन लाल चौधरी\",\"Column10\":\"House : , LandMark : Village raipur, Street : , District: Pratapgarh, PIN: 312619, State: Rajasthan\",\"Column11\":\"House : #LandMark : Village raipur#Street : #District: Pratapgarh#PIN: 312619#State: Rajasthan\"}]";
                }

                string message = string.Empty;

                Session["IsVerified"] = "False";
                DataTable dt = new DataTable();

                if (Session["tid"] != null)
                {
                    dt = csCommon.ValidateAadharOTP(Session["tid"].ToString(), OTP); //	
                    if (dt.Rows.Count > 0)
                    {
                        if (Request.Url.Host.Contains("localhost"))
                        {
                            dt.Rows[0]["isvalidate"] = "true";
                            Session["JanaadhaarNo"] = "4751206885";
                            Session["JAN_MEMBER_ID"] = "31224592617";
                            Session["ENR_Id"] = "1067-S692-15214";

                        }

                        if (dt.Rows[0]["isvalidate"].ToString() == "true")
                        {

                            csCommon objComm = new csCommon();
                            DataSet dS = new DataSet();
                            dt = objComm.GetJanAadharMembersDetail(Session["JanaadhaarNo"].ToString(), Session["mENR_Id"].ToString(), Session["JAN_MEMBER_ID"].ToString(), SocietyDirectory); //Session["JanaadhaarNo"] as aadhar



                            //dS = objComm.JanAadhaarMembersList(Session["JanaadhaarNo"].ToString(), Session["JAN_MEMBER_ID"].ToString()); //Session["JanaadhaarNo"] as aadhar
                            //dt = dS.Tables[0];
                            //dt.Columns["AADHAR_ID"].ColumnName = "Column1"; dt.Columns["NAME"].ColumnName = "Column2";
                            //dt.Columns["SRDR_MID"].ColumnName = "Column3";
                            //dt.Columns["MOBILE_NO"].ColumnName = "Column6";
                            //dt.Columns["FATHER_NAME_ENG"].ColumnName = "Column8";
                            //dt.Columns["FATHER_NAME_ENG"].ColumnName = "Column10";
                            //dt.Rows[0]["Column1"] = "Success";

                            Session.Remove("tid");
                            Session.Remove("JanaadhaarNo");
                            return COMMON_FUNCTION.DataTableToJSONString(dt);
                        }

                    }
                    else
                    {
                        dt.Columns.Add("Column1");
                        DataRow dr = dt.NewRow();
                        dr["Column1"] = "Invalid OTP!";
                        dt.Rows.Add(dr);
                        return COMMON_FUNCTION.DataTableToJSONString(dt);
                    }
                    return COMMON_FUNCTION.DataTableToJSONString(dt);
                }
                else
                {
                    dt.Columns.Add("Column1");
                    DataRow dr = dt.NewRow();
                    dr["Column1"] = "Session out , Please login again!";
                    dt.Rows.Add(dr);
                    return COMMON_FUNCTION.DataTableToJSONString(dt);
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen", FunctionName = "VerifyRecheckOTP(string OTP)" }, ex);

                return "Some error occured.";
            }

        }



        [HttpPost]
        public string SendRecheckOTP(string AadharNo, string JanAadharId, string MemberId, string ENR_Id)
        {
            try
            {
                if (AadharNo != null && AadharNo != "")
                    AadharNo = csCommon.Decrypt_SA(Uri.UnescapeDataString(AadharNo));
                if (JanAadharId != null && JanAadharId != "")
                {
                    JanAadharId = csCommon.Decrypt_SA(Uri.UnescapeDataString(JanAadharId));
                    JanAadharId = COMMON_FUNCTION.Decrypt(JanAadharId);
                }
                DataSet dtts = new DataSet();

                if (Request.Url.Host.Contains("localhost"))
                {

                    string jsonString = @"[{ 'status': true, 'tid': '173999578' }]";

                    DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(jsonString);
                    dtts.Tables.Add(dataTable);

                }
                else
                {
                    dtts = csCommon.SendOtpByAadharNo_KYC(JanAadharId, MemberId);
                }

                DataTable dtt = new DataTable();
                if (dtts != null && dtts.Tables.Count != 0 && dtts.Tables[0].Rows.Count != 0)
                    dtt = dtts.Tables[0];



                if (dtt != null && dtt.Rows.Count > 0 && dtt.Columns.Contains("tid"))
                {
                    if (dtt.Rows[0]["tid"] == null)
                    {
                        return "Fail";
                    }
                    else if (dtt.Rows[0]["tid"].ToString().Contains("DOIT"))
                    {
                        Session["Aadhar"] = AadharNo;
                        Session["JanAadharId"] = JanAadharId;
                        Session["MemberId"] = MemberId;
                        //Session["ENR_Id"] = ENR_Id;

                        Session["mAadhar"] = AadharNo;
                        Session["mJanAadharId"] = JanAadharId;
                        Session["mMemberId"] = MemberId;
                        Session["mENR_Id"] = ENR_Id;


                        Session["tid"] = dtt.Rows[0]["tid"].ToString();
                        return "Success";

                    }
                    else
                    {
                        Session["tid"] = dtt.Rows[0]["tid"].ToString();
                        Session["mENR_Id"] = ENR_Id;
                        return Session["tid"].ToString();
                    }

                }
                else
                {
                    return "Fail";
                }
            }
            catch (Exception ex)
            {

                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen", FunctionName = "SendRecheckOTP(string \"" + AadharNo + "\", string \"" + JanAadharId + "\", string MemberId, string ENR_Id)" }, ex);
                return "Fail";
            }
        }

        public string VerifyRecheckOTP(string OTP, string SocietyDirectory = "")
        {
            try
            {
                //string _SocietyDirectory = "";
                if (Request.Url.Host.Contains("localhost1"))
                {

                    return "[{\"Column1\":\"Success\",\"Column2\":\"Balwant Chaudhary\",\"Column3\":\"M\",\"Column4\":\"20-09-1984\",\"Column5\":\"../Content/NewRegistration/member_90b405c50c384ae5b3eb3ed25534594d.jpg\",\"Column6\":\"बलवंत चौधरी\",\"Column7\":\"T1+Bhe8Ju9s4QjBaM66sAg==\",\"Column8\":\"S/O: Mohan Lal Chaudhary\",\"Column9\":\"S/O: मोहन लाल चौधरी\",\"Column10\":\"House : , LandMark : Village raipur, Street : , District: Pratapgarh, PIN: 312619, State: Rajasthan\",\"Column11\":\"House : #LandMark : Village raipur#Street : #District: Pratapgarh#PIN: 312619#State: Rajasthan\"}]";
                }

                string message = string.Empty;

                Session["IsVerified"] = "False";
                DataTable dt = new DataTable();

                if (Session["tid"] != null)
                {
                    dt = csCommon.ValidateAadharOTP(Session["tid"].ToString(), OTP); //	
                    if (dt.Rows.Count > 0)
                    {
                        if (Request.Url.Host.Contains("localhost"))
                        {
                            dt.Rows[0]["isvalidate"] = "true";
                            Session["JanaadhaarNo"] = "4751206885";
                            Session["JAN_MEMBER_ID"] = "31224592617";
                            Session["ENR_Id"] = "1067-S692-15214";

                        }

                        if (dt.Rows[0]["isvalidate"].ToString() == "true")
                        {

                            csCommon objComm = new csCommon();
                            DataSet dS = new DataSet();
                            dt = objComm.GetJanAadharMembersDetail(Session["JanaadhaarNo"].ToString(), Session["mENR_Id"].ToString(), Session["JAN_MEMBER_ID"].ToString(), SocietyDirectory); //Session["JanaadhaarNo"] as aadhar



                            //dS = objComm.JanAadhaarMembersList(Session["JanaadhaarNo"].ToString(), Session["JAN_MEMBER_ID"].ToString()); //Session["JanaadhaarNo"] as aadhar
                            //dt = dS.Tables[0];
                            //dt.Columns["AADHAR_ID"].ColumnName = "Column1"; dt.Columns["NAME"].ColumnName = "Column2";
                            //dt.Columns["SRDR_MID"].ColumnName = "Column3";
                            //dt.Columns["MOBILE_NO"].ColumnName = "Column6";
                            //dt.Columns["FATHER_NAME_ENG"].ColumnName = "Column8";
                            //dt.Columns["FATHER_NAME_ENG"].ColumnName = "Column10";
                            //dt.Rows[0]["Column1"] = "Success";

                            Session.Remove("tid");
                            Session.Remove("JanaadhaarNo");
                            return COMMON_FUNCTION.DataTableToJSONString(dt);
                        }

                    }
                    else
                    {
                        dt.Columns.Add("Column1");
                        DataRow dr = dt.NewRow();
                        dr["Column1"] = "Invalid OTP!";
                        dt.Rows.Add(dr);
                        return COMMON_FUNCTION.DataTableToJSONString(dt);
                    }
                    return COMMON_FUNCTION.DataTableToJSONString(dt);
                }
                else
                {
                    dt.Columns.Add("Column1");
                    DataRow dr = dt.NewRow();
                    dr["Column1"] = "Session out , Please login again!";
                    dt.Rows.Add(dr);
                    return COMMON_FUNCTION.DataTableToJSONString(dt);
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen", FunctionName = "VerifyRecheckOTP(string OTP)" }, ex);

                return "Some error occured.";
            }

        }

        //End JanAdhar intigration
        public static string Encrypt(string Text)
        {
            string Key = "681D392DB4FD7B8C6562712DFEEF1";
            string initVector = "tu89geji340t89u2";
            int keysize = 256;

            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(Text);
            PasswordDeriveBytes password = new PasswordDeriveBytes(Key, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] Encrypted = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(Encrypted);
        }
        public static string Decrypt(string EncryptedText)
        {
            string Key = "681D392DB4FD7B8C6562712DFEEF1";
            string initVector = "tu89geji340t89u2";
            int keysize = 256;
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] DeEncryptedText = Convert.FromBase64String(EncryptedText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(Key, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(DeEncryptedText);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[DeEncryptedText.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }
        #endregion

        #region Pldb
        public ActionResult OTSApplication()
        {
            return View();
        }


        [HttpPost]
        [ActionName("GetOtsData")]
        public JsonResult GetOtsData(string SearchParameter)
        {
            DataTable Dt = null;
            CitizenModel obj = new CitizenModel(new DBLogger());
            if (Request.Cookies["SSO_ID"] != null) { Dt = obj.GetGssComplaintDetails(Request.Cookies["SSO_ID"].Value); }


            PLDBLoan objLoan = new PLDBLoan(new DBLogger());

            var SearchParameterEnc = "Encrypt:" + SearchParameter;
            var SearchParameterDec = csCommon.Decrypt_SA(Uri.UnescapeDataString(SearchParameterEnc));
            Dt = objLoan.GetOTSListApplicationFormViewData("", "MainData", SearchParameterDec);


            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(Dt) }, JsonRequestBehavior.AllowGet);
        }

        #endregion


        [HttpPost]
        public JsonResult UploadComplainDocument(HttpPostedFileBase UploadDocument)
        {
            try
            {
                if (UploadDocument == null)
                    return Json(new { success = false, message = "File not received" });

                string folder = Server.MapPath("~/Content/Citizen/ComplainDocument/");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(UploadDocument.FileName);
                string filePath = Path.Combine(folder, fileName);

                UploadDocument.SaveAs(filePath);

                return Json(new { success = true, fileName = fileName });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}