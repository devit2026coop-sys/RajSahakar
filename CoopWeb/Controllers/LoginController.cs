using CoopDAL;
using Cooperative.Filters;
using Cooperative.Helper;
using Cooperative.Models;
using Cooperative.SSOWSSoap;
using java.security;
using javax.security.sasl;
using Newtonsoft.Json;
using RAJSSO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI.WebControls;

namespace Cooperative.Controllers
{
    public class LoginController : Controller
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        #region Login
        [HttpGet]
        public ActionResult MisDetailReport()
        {
            Response.Cookies.Add(new System.Web.HttpCookie("ASP.NET_SessionId", "")
            {
                Value = Guid.NewGuid().ToString(),
                HttpOnly = true
            });

            if (Request.Cookies[SessionIdentity.Culture] == null)
            {
                Response.Cookies.Add(new System.Web.HttpCookie(Cooperative.Controllers.SessionIdentity.Culture, "")
                {
                    Value = "en-us",
                    HttpOnly = true
                });
            }
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Home, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            Session.Abandon();
            Response.Cookies.Add(new System.Web.HttpCookie("ASP.NET_SessionId", "")
            {
                Value = Guid.NewGuid().ToString(),
                HttpOnly = true
            });


            if (Request.Cookies[SessionIdentity.Culture] == null)
            {
                Response.Cookies.Add(new System.Web.HttpCookie(Cooperative.Controllers.SessionIdentity.Culture, "")
                {
                    Value = "en-us",
                    HttpOnly = true
                });
            }
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Home, (Request.Cookies[SessionIdentity.Culture]).Value);


            if (Response.Cookies.Count > 0)
            {
                foreach (string s in Response.Cookies.AllKeys)
                {
                    if (s == FormsAuthentication.FormsCookieName || "ASP.NET_SessionId".Equals(s, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Response.Cookies[s].Secure = true;
                    }
                }
            }
            return View();
        }

        public ActionResult LoginPage()
        {

            return View();
        }

        //Added by Bhavesh
        [HttpGet]
        public ActionResult CitizenDetailList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Inauguration()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Home, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginCheck(LogInEntity _Model)
        {
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", "")
            {
                Expires = DateTime.Now.AddDays(-30),
                HttpOnly = true
            });

            if (string.IsNullOrEmpty(_Model.UserId) || string.IsNullOrEmpty(_Model.Password))
            {
                return View("Login");
            }

            string _USer = _Model.UserId, Pass = _Model.Password;
            UserRegistrationModel _db = new UserRegistrationModel(new DBLogger());
            UserEntity _User = _db.Login(_Model.UserId, _Model.Password);
            _User.UserType = "D";
            if (_User != null && _User.UserId > 0)
            {
                Session[SessionIdentity.Admin] = _User;
                regenerateId();
                return RedirectToAction("DashBoard", "User");
            }
            else
            {
                return View("Login");
            }

        }
        public void BacktoEmitra()
        {
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {
                    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                    if (_LoggedInUser.UserType == "K")
                        Response.Redirect(_LoggedInUser.EmitraReturnUrl);
                }
                else
                {
                    RAJSSO.SSO SSO = new RAJSSO.SSO();
                    Session.Clear();
                    Session.Abandon();
                    SSO.BackToSSO();
                }

            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.Login", FunctionName = "string BacktoEMITRA_Click()" }, ex);
            }


        }

        public void Logout()
        {
            if (Response != null)
            {
                Response.ExpiresAbsolute = DateTime.Now;
                Response.Expires = -1441;
                Response.CacheControl = "no-cache";
                Response.AddHeader("Pragma", "no-cache");
                Response.AddHeader("Pragma", "no-store");
                Response.AddHeader("cache-control", "no-cache");
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                Response.Cache.SetNoServerCaching();
                Session.Abandon();
                DeleteUnwantedCookies();
                Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", "")
                {
                    Value = Guid.NewGuid().ToString(),
                    HttpOnly = true
                });
            }

            SSO SSO = new SSO();
            if (Request != null && Request.Cookies["RAJSSO"] != null)
                SSO.SSOSignout(Request.Cookies["RAJSSO"].Value);
            else
                Response.Redirect("https://sso.rajasthan.gov.in");

            //String SSOID = Request.Cookies["RAJSSO"].Value;
            //RAJSSO.SSO SSO = new RAJSSO.SSO();
            //SSO.BackToSSO(SSOID);



            //  return RedirectToAction("Login", "Login");

        }



        public void GotoSSO()
        {
            Response.Redirect("https://sso.rajasthan.gov.in");

        }


        [HttpGet]
        public ActionResult SSOMapping()
        {
            //string sdsd = COMMON_FUNCTION.Encrypt("32999");
            Response.Cookies.Add(new System.Web.HttpCookie("ASP.NET_SessionId", "")
            {
                Expires = DateTime.Now.AddDays(-30),
                HttpOnly = true
            });

            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
            {
                try
                {
                    if (Request.Url.Host.Contains("10.70.234.208") || Request.Url.Host.Contains("10.68.231.168"))
                    {
                        UserEntity _Entity = new UserEntity();
                        SSOWSSoap.SSOUserDetail usrdtl = new SSOWSSoap.SSOUserDetail();
                        SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
                        usrdtl.SSOID = "VINAYYADAV329";
                        usrdtl.displayName = "VINAY YADAV";
                        usrdtl.aadhaarId = "111111111111";
                        usrdtl.designation = "CITIZEN";

                        string pathString = "../SSOUSER/" + usrdtl.SSOID + ".jpg";

                        _Entity.OfficerNameEn = usrdtl.displayName + "(" + usrdtl.designation + ")";
                        _Entity.ImageUrl = pathString;
                        _Entity.SSOID = usrdtl.SSOID;
                        _Entity.UserType = "C";
                        _Entity.UserId = 60;
                        _Entity.RoleId = 10044;
                        _Entity.UserRoleDeptMappingId = 60;
                        _Entity.KIOSKCODE = "";
                        System.Web.HttpContext.Current.Session[SessionIdentity.Admin] = _Entity;

                        regenerateId();
                        return RedirectToAction("Index", "Citizen");
                    }
                    else
                    {

                        if (Request.Cookies["SSOROLE"].Value != "CITIZEN")
                        {
                            try
                            {
                                SSOWSSoap.SSOUserDetail usrdtl = new SSOWSSoap.SSOUserDetail();
                                SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
                                usrdtl = ssoclient.GetUserDetailXML(Request.Cookies["SSO_ID"].Value, "RAJSAHKAR.TEST", "Rajasthan@12");
                                string pathString = "/SSOUSER/" + usrdtl.SSOID.Trim() + ".jpg";

                                string aadhaarId = usrdtl.aadhaarId;
                                string aadhaarRefId = usrdtl.aadhaarId;
                                if (usrdtl.aadhaarId.Length > 12)
                                {
                                    aadhaarId = COMMON_FUNCTION.GetAadharFromDSMService(usrdtl.aadhaarId);
                                }

                                if (usrdtl != null && usrdtl.jpegPhoto != null && usrdtl.jpegPhoto != "")
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
                                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.LoginController", FunctionName = "Inner Try in SSOMapping()" }, ex);
                                    }

                                }
                                else
                                {
                                    pathString = "../SSOUSER/Balnk.PNG";
                                }

                                string SSOID = Request.Cookies["SSO_ID"].Value;
                                UserRegistrationModel _db = new UserRegistrationModel();

                                UserEntity _User = _db.Login(SSOID);
                                _User.AadharNo = aadhaarId;
                                if (_User != null && _User.UserId > 0) // User Must exists here
                                {
                                    _User.KIOSKCODE = "";
                                    _User.UserType = "D";
                                    _User.ImageUrl = pathString;
                                    _User.AadharNo = aadhaarId;
                                    _User.AadharRefId = aadhaarRefId;
                                    System.Web.HttpContext.Current.Session[SessionIdentity.Admin] = _User;
                                    if (Request.Cookies[SessionIdentity.Culture] == null)
                                    {
                                        Response.Cookies.Add(new System.Web.HttpCookie(Cooperative.Controllers.SessionIdentity.Culture, "")
                                        {
                                            Value = "en-us",
                                            HttpOnly = true
                                        });
                                    }

                                    regenerateId();

                                    return RedirectToAction("DashBoard", "User");
                                }
                                else
                                {
                                    return RedirectToAction("userregistrationRequest", "Login");
                                }
                            }
                            catch (Exception ex)
                            {
                                ILogger objILogger = new DBLogger();
                                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.LoginController", FunctionName = "SSOMapping()" }, ex);
                            }
                        }
                        else
                        {
                            if (Request.Cookies["SSOROLE"].Value == "CITIZEN")
                            {
                                UserEntity _Entity = new UserEntity();
                                SSOWSSoap.SSOUserDetail usrdtl = new SSOWSSoap.SSOUserDetail();
                                SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
                                //usrdtl.SSOID = "VINAYYADAV329";
                                //usrdtl.displayName = "VINAYYADAV329";
                                //usrdtl.aadhaarId = "111111111111";

                                usrdtl = ssoclient.GetUserDetailXML(Request.Cookies["SSO_ID"].Value, "RAJSAHKAR.TEST", "Rajasthan@12");
                                _Entity.AadharNo = usrdtl.aadhaarId;
                                _Entity.AadharRefId = usrdtl.aadhaarId;
                                if (_Entity.AadharNo.Length > 12)
                                {
                                    _Entity.AadharNo = COMMON_FUNCTION.GetAadharFromDSMService(usrdtl.aadhaarId);
                                    //usrdtl.aadhaarId = _Entity.AadharNo;
                                }


                                string pathString = "../SSOUSER/" + usrdtl.SSOID + ".jpg";
                                if (usrdtl != null)
                                {
                                    _Entity.OfficerNameEn = usrdtl.displayName + "(" + usrdtl.designation + ")";
                                    ////Add By Bhavesh
                                    //_Entity.CitizenId = 0;
                                    //_Entity.DisplayNameEn  = usrdtl.displayName; //+ "(" + usrdtl.designation + ")";
                                    //_Entity.DisplayNameHi  = usrdtl.displayName; //+ "(" + usrdtl.designation + ")";
                                    //_Entity.SSOID = usrdtl.SSOID;
                                    //_Entity.Gender = usrdtl.gender;
                                    //_Entity.MobileNo = usrdtl.mobile;
                                    //_Entity.PhoneNo = usrdtl.telephoneNumber;
                                    //_Entity.EmailId = usrdtl.mailPersonal;
                                   
                                    //if (!string.IsNullOrEmpty(usrdtl.dateOfBirth))
                                    //{
                                    //    DateTime dob;
                                    //    if (DateTime.TryParse(usrdtl.dateOfBirth, out dob))
                                    //    {
                                    //        _Entity.DateOfBirth = dob;
                                    //    }
                                    //    else
                                    //    {
                                    //        _Entity.DateOfBirth = DateTime.MinValue; // or null if nullable
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    _Entity.DateOfBirth = DateTime.MinValue; // or null
                                    //}

                                    //_Entity.AadharNo = usrdtl.aadhaarId;
                                    //_Entity.IsActive = 1;
                                    //_Entity.CreatedBy = 0;
                                    //_Entity.CreatedIP = Request.UserHostAddress;

                                    // Insert by Bhavesh
                                    //UserRegistrationModel model = new UserRegistrationModel();
                                    //int citizenId = model.AddOrUpdateCitizen(_Entity);
                                   
                                    //    _Entity.CitizenId = citizenId;

                                    if (!System.IO.File.Exists(Server.MapPath(pathString)))
                                    {
                                        if (usrdtl.jpegPhoto != null)
                                        {
                                            byte[] imageBytes = Convert.FromBase64String(usrdtl.jpegPhoto);
                                            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                            string fileName = usrdtl.SSOID;
                                            FileStream fs = new FileStream(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/SSOUSER/"), fileName + ".jpg"), FileMode.CreateNew, FileAccess.Write);
                                            ms.WriteTo(fs);
                                            fs.Close();
                                            fs.Dispose();
                                            ms.Flush();
                                            _Entity.ImageUrl = "../SSOUSER/ " + fileName + ".jpg";
                                        }
                                    }
                                    else
                                        _Entity.ImageUrl = pathString.Replace("~", "..");
                                }
                                if (usrdtl != null && usrdtl.jpegPhoto != null && usrdtl.jpegPhoto != "")
                                {
                                    //if (!System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath(pathString)))
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
                                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.LoginController", FunctionName = "Inner Try in SSOMapping()" }, ex);
                                    }
                                }
                                else
                                {
                                    pathString = "../SSOUSER/Balnk.PNG";
                                }

                                if (usrdtl != null)
                                {
                                    //_Entity.AadharNo = usrdtl.aadhaarId;
                                    _Entity.AadharRefId = usrdtl.aadhaarId;
                                    _Entity.OfficerNameEn = usrdtl.displayName + "(" + usrdtl.designation + ")";
                                    //_Entity.DisplayNameEn = usrdtl.displayName + "(" + usrdtl.designation + ")";
                                }
                                else
                                {
                                    _Entity.AadharNo = "";
                                    _Entity.OfficerNameEn = "";
                                    _Entity.DisplayNameEn = "";
                                }

                                _Entity.ImageUrl = pathString;
                                _Entity.SSOID = Request.Cookies["SSO_ID"].Value;
                                _Entity.UserType = "C";
                                _Entity.UserId = 60;
                                _Entity.RoleId = 10044;
                                _Entity.UserRoleDeptMappingId = 60;
                                _Entity.KIOSKCODE = "";
                                if (Request.Cookies["SWC"] != null && Request.Cookies["SWC"].Value == "YES")
                                    _Entity.IsFromSWC = "YES";
                                else
                                    _Entity.IsFromSWC = "NO";

                                System.Web.HttpContext.Current.Session[SessionIdentity.Admin] = _Entity;
                                regenerateId();
                                if (_Entity.IsFromSWC == "YES")
                                {
                                    System.Web.HttpContext.Current.Session[SessionIdentity.SWCSData] = Request.Cookies["SWC_DATA"].Value;
                                    System.Web.HttpContext.Current.Session[SessionIdentity.ISRAJNIVESH] = "1";
                                    SWSEntity sWSEntity = JsonConvert.DeserializeObject<SWSEntity>(Request.Cookies["SWC_DATA"].Value);

                                    if (sWSEntity.IsView == "1")
                                    {
                                        RajNiveshModel rajNivesh = new RajNiveshModel(new FileLogger());
                                        DataTable dtr = rajNivesh.GetSwcData(sWSEntity.SWSID, sWSEntity.SSO_ID);
                                        if (dtr != null && dtr.Rows.Count > 0)
                                            return RedirectToAction("SocietyApplicationView", "Society", new { SocietyIDEnc = dtr.Rows[0]["SocietyIDEnc"].ToString() + "~_Citizen_SocietyProfile~undefined" });
                                    }
                                    if (sWSEntity.IsView == "2")
                                    {
                                        RajNiveshModel rajNivesh = new RajNiveshModel(new FileLogger());
                                        DataTable dtr = rajNivesh.GetSwcData(sWSEntity.SWSID);
                                        if (dtr != null && dtr.Rows.Count > 0)
                                            return RedirectToAction("SocietyApplicationView", "Society", new { SocietyIDEnc = dtr.Rows[0]["SocietyIDEnc"].ToString() + "~_Citizen_SocietyProfile~undefined" });
                                    }
                                    else
                                        return RedirectToAction("SocietyApplicationForm", "Citizen");
                                }
                                else
                                {
                                    System.Web.HttpContext.Current.Session[SessionIdentity.SWCSData] = null;
                                    return RedirectToAction("Index", "Citizen");
                                }
                            }
                        }
                    }
                    //}
                }
                catch (Exception ex)
                {
                    //ILogger objILogger = new DBLogger();
                    ILogger objILogger = new FileLogger();
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SSOValidation", FunctionName = "ActionResult ValidateUser()" }, ex);
                    return View();
                }

            }
            else
            {
                return View();
            }
            return View();
        }

        #region SSO Mapping using API by Pradeep

        [HttpGet]
        public ActionResult SSOMapping_New()
        {
            Response.Cookies.Add(new System.Web.HttpCookie("ASP.NET_SessionId", "")
            {
                Expires = DateTime.Now.AddDays(-30),
                HttpOnly = true
            });

            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
            {
                SSO_UserProfileDetailModel usrdtl = new SSO_UserProfileDetailModel();
                try
                {
                    if (Request.Cookies["SSOROLE"].Value != "CITIZEN")
                    {
                        try
                        {
                            if (Request.Url.Host.Contains("localhost"))
                            {
                                usrdtl.SSOID = Request.Cookies["SSO_ID"].Value;
                                SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
                                var usrdtllocal = ssoclient.GetUserDetailXML(Request.Cookies["SSO_ID"].Value, "RAJSAHKAR.TEST", "Rajasthan@12");
                                usrdtl.displayName = usrdtllocal.displayName;
                                usrdtl.designation = usrdtllocal.designation;
                                usrdtl.janaadhaarId = usrdtllocal.aadhaarId;

                            }
                            else
                                usrdtl = SsoServiceHelper.GetSSOUserProfileDetail(Request.Cookies["SSO_ID"].Value, Request.Cookies["SSOTOKEN"].Value);
                            string pathString = "/SSOUSER/" + usrdtl.SSOID.Trim() + ".jpg";

                            string aadhaarId = usrdtl.aadhaarId;
                            if (usrdtl.aadhaarId?.Length > 12)
                            {
                                aadhaarId = COMMON_FUNCTION.GetAadharFromDSMService(usrdtl.aadhaarId);
                            }

                            if (usrdtl != null && usrdtl.jpegPhoto != null && usrdtl.jpegPhoto != "")
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
                                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.LoginController", FunctionName = "Inner Try in SSOMapping()" }, ex);
                                }

                            }
                            else
                            {
                                pathString = "../SSOUSER/Balnk.PNG";
                            }

                            string SSOID = Request.Cookies["SSO_ID"].Value;
                            UserRegistrationModel _db = new UserRegistrationModel();

                            UserEntity _User = _db.Login(SSOID);
                            _User.AadharNo = aadhaarId;
                            if (_User != null && _User.UserId > 0) // User Must exists here
                            {
                                _User.KIOSKCODE = "";
                                _User.UserType = "D";
                                _User.ImageUrl = pathString;
                                _User.AadharNo = aadhaarId;
                                System.Web.HttpContext.Current.Session[SessionIdentity.Admin] = _User;
                                if (Request.Cookies[SessionIdentity.Culture] == null)
                                {
                                    Response.Cookies.Add(new System.Web.HttpCookie(Cooperative.Controllers.SessionIdentity.Culture, "")
                                    {
                                        Value = "en-us",
                                        HttpOnly = true
                                    });
                                }
                                regenerateId();

                                return RedirectToAction("DashBoard", "User");
                            }
                            else
                            {
                                return RedirectToAction("userregistrationRequest", "Login");
                            }
                        }
                        catch (Exception ex)
                        {
                            ILogger objILogger = new DBLogger();
                            objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.LoginController", FunctionName = "SSOMapping()" }, ex);
                        }
                    }
                    else
                    {
                        if (Request.Cookies["SSOROLE"].Value == "CITIZEN")
                        {
                            UserEntity _Entity = new UserEntity();
                            if (Request.Url.Host.Contains("localhost"))
                            {
                                usrdtl.SSOID = Request.Cookies["SSO_ID"].Value;
                                SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
                                var usrdtllocal = ssoclient.GetUserDetailXML(Request.Cookies["SSO_ID"].Value, "RAJSAHKAR.TEST", "Rajasthan@12");
                                usrdtl.displayName = usrdtllocal.displayName;
                                usrdtl.designation = usrdtllocal.designation;
                                usrdtl.janaadhaarId = usrdtllocal.aadhaarId;
                            }
                            else
                                usrdtl = SsoServiceHelper.GetSSOUserProfileDetail(Request.Cookies["SSO_ID"].Value, Request.Cookies["SSOTOKEN"].Value);
                            _Entity.AadharNo = usrdtl.aadhaarId;
                            if (_Entity.AadharNo?.Length > 12)
                            {
                                _Entity.AadharNo = COMMON_FUNCTION.GetAadharFromDSMService(usrdtl.aadhaarId);
                                usrdtl.aadhaarId = _Entity.AadharNo;
                            }


                            string pathString = "../SSOUSER/" + usrdtl.SSOID + ".jpg";
                            if (usrdtl != null)
                            {
                                _Entity.OfficerNameEn = usrdtl.displayName + "(" + usrdtl.designation + ")";
                                if (!System.IO.File.Exists(Server.MapPath(pathString)))
                                {
                                    if (usrdtl.jpegPhoto != null)
                                    {
                                        byte[] imageBytes = Convert.FromBase64String(usrdtl.jpegPhoto);
                                        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                        string fileName = usrdtl.SSOID;
                                        FileStream fs = new FileStream(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/SSOUSER/"), fileName + ".jpg"), FileMode.CreateNew, FileAccess.Write);
                                        ms.WriteTo(fs);
                                        fs.Close();
                                        fs.Dispose();
                                        ms.Flush();
                                        _Entity.ImageUrl = "../SSOUSER/ " + fileName + ".jpg";
                                    }
                                }
                                else
                                    _Entity.ImageUrl = pathString.Replace("~", "..");
                            }
                            if (usrdtl != null && usrdtl.jpegPhoto != null && usrdtl.jpegPhoto != "")
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
                                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.LoginController", FunctionName = "Inner Try in SSOMapping()" }, ex);
                                }
                            }
                            else
                            {
                                pathString = "../SSOUSER/Balnk.PNG";
                            }

                            if (usrdtl != null)
                            {
                                _Entity.AadharNo = usrdtl.aadhaarId;
                                _Entity.OfficerNameEn = usrdtl.displayName + "(" + usrdtl.designation + ")";
                            }
                            else
                            {
                                _Entity.AadharNo = "";
                                _Entity.OfficerNameEn = "";
                            }

                            _Entity.ImageUrl = pathString;
                            _Entity.SSOID = Request.Cookies["SSO_ID"].Value;
                            _Entity.UserType = "C";
                            _Entity.UserId = 60;
                            _Entity.RoleId = 10044;
                            _Entity.UserRoleDeptMappingId = 60;
                            _Entity.KIOSKCODE = "";
                            if (Request.Cookies["SWC"] != null && Request.Cookies["SWC"].Value == "YES")
                                _Entity.IsFromSWC = "YES";
                            else
                                _Entity.IsFromSWC = "NO";

                            System.Web.HttpContext.Current.Session[SessionIdentity.Admin] = _Entity;
                            regenerateId();
                            if (_Entity.IsFromSWC == "YES")
                            {
                                System.Web.HttpContext.Current.Session[SessionIdentity.SWCSData] = Request.Cookies["SWC_DATA"].Value;
                                System.Web.HttpContext.Current.Session[SessionIdentity.ISRAJNIVESH] = "1";
                                SWSEntity sWSEntity = JsonConvert.DeserializeObject<SWSEntity>(Request.Cookies["SWC_DATA"].Value);

                                if (sWSEntity.IsView == "1")
                                {
                                    RajNiveshModel rajNivesh = new RajNiveshModel(new FileLogger());
                                    DataTable dtr = rajNivesh.GetSwcData(sWSEntity.SWSID, sWSEntity.SSO_ID);
                                    if (dtr != null && dtr.Rows.Count > 0)
                                        return RedirectToAction("SocietyApplicationView", "Society", new { SocietyIDEnc = dtr.Rows[0]["SocietyIDEnc"].ToString() + "~_Citizen_SocietyProfile~undefined" });
                                }
                                if (sWSEntity.IsView == "2")
                                {
                                    RajNiveshModel rajNivesh = new RajNiveshModel(new FileLogger());
                                    DataTable dtr = rajNivesh.GetSwcData(sWSEntity.SWSID);
                                    if (dtr != null && dtr.Rows.Count > 0)
                                        return RedirectToAction("SocietyApplicationView", "Society", new { SocietyIDEnc = dtr.Rows[0]["SocietyIDEnc"].ToString() + "~_Citizen_SocietyProfile~undefined" });
                                }
                                else
                                    return RedirectToAction("SocietyApplicationForm", "Citizen");
                            }
                            else
                            {
                                System.Web.HttpContext.Current.Session[SessionIdentity.SWCSData] = null;
                                return RedirectToAction("Index", "Citizen");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ILogger objILogger = new FileLogger();
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.Login", FunctionName = "ActionResult SSOMapping()" }, ex);
                    return View();
                }

            }
            else
            {
                return View();
            }
            return View();
        }

        #endregion

        [CustomAuthentication]
        [HttpPost]
        public string UserProfileChangeByRoleId(string UserRoleDeptMappingId)
        {
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                string SSOID = _LoggedInUser.SSOID;
                string IMGUrl = _LoggedInUser.ImageUrl;
                string AadharNo = _LoggedInUser.AadharNo;
                string AadharRefId = _LoggedInUser.AadharRefId;
                UserRegistrationModel _db = new UserRegistrationModel();
                UserEntity _User = _db.LoginBySsoRole(SSOID, UserRoleDeptMappingId);

                if (_User != null && _User.UserId > 0) // User Must exists here
                {
                    //Session.Abandon();
                    _User.KIOSKCODE = "";
                    _User.UserType = "D";
                    _User.ImageUrl = IMGUrl;
                    _User.AadharNo = AadharNo;
                    _User.AadharRefId = AadharRefId;
                    Session[SessionIdentity.Admin] = _User;
                    if (Request.Cookies[SessionIdentity.Culture] == null)
                    {
                        //Request.Cookies.Add(new System.Web.HttpCookie(Cooperative.Controllers.SessionIdentity.Culture, "en-us"));
                        Response.Cookies.Add(new System.Web.HttpCookie(Cooperative.Controllers.SessionIdentity.Culture, "")
                        {
                            Value = "en-us",
                            HttpOnly = true
                        });
                    }

                    regenerateId();
                    if (_User.DashBoardURL.Contains("PMKisan/Dashboard"))
                        return "3";
                    return "1";
                }
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.Login", FunctionName = "string UserProfileChangeByRoleId(string UserRoleDeptMappingId)" }, ex);
                return "2";
            }

            return "2";
        }
        [HttpGet]
        public ActionResult MapSSOWithCredentials()
        {
            //if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
            //    return View();
            //else
            //    return Redirect("https://sso.rajasthan.gov.in/signin");
            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
                return View();
            else
            {

            }

            COMMON_FUNCTION.ssoSignout("");
            return View();
        }

        [HttpGet]
        public ActionResult MapSSOWithCredentials1()
        {
            return View();
        }


        public void BackToSSO()
        {
            //RAJSSO.SSO SSO = new RAJSSO.SSO();
            //if (Request.Cookies["SSO_ID"] != null)
            //    SSO.BackToSSO(Request.Cookies["SSO_ID"].Value);
            //else
            //    SSO.BackToSSO("");

            SSO SSO = new SSO();
            if (Request.Cookies["RAJSSO"] != null)
                SSO.BackToSSO(Request.Cookies["RAJSSO"].Value);
            else
                SSO.BackToSSO("");

            //if (Request.Cookies["SSOTOKEN"] != null)
            //    SsoServiceHelper.BackToSSO(Request.Cookies["SSOTOKEN"].Value);
            //else
            //    SsoServiceHelper.BackToSSO("");



        }

        public void regenerateId()
        {
            System.Web.SessionState.SessionIDManager manager = new System.Web.SessionState.SessionIDManager();
            string oldId = manager.GetSessionID(System.Web.HttpContext.Current);
            string newId = manager.CreateSessionID(System.Web.HttpContext.Current);
            bool isAdd = false, isRedir = false;
            manager.SaveSessionID(System.Web.HttpContext.Current, newId, out isRedir, out isAdd);
            System.Web.HttpApplication ctx = (HttpApplication)System.Web.HttpContext.Current.ApplicationInstance;
            HttpModuleCollection mods = ctx.Modules;
            System.Web.SessionState.SessionStateModule ssm = (SessionStateModule)mods.Get("Session");
            System.Reflection.FieldInfo[] fields = ssm.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            SessionStateStoreProviderBase store = null;
            System.Reflection.FieldInfo rqIdField = null, rqLockIdField = null, rqStateNotFoundField = null;
            foreach (System.Reflection.FieldInfo field in fields)
            {
                if (field.Name.Equals("_store")) store = (SessionStateStoreProviderBase)field.GetValue(ssm);
                if (field.Name.Equals("_rqId")) rqIdField = field;
                if (field.Name.Equals("_rqLockId")) rqLockIdField = field;
                if (field.Name.Equals("_rqSessionStateNotFound")) rqStateNotFoundField = field;
            }
            object lockId = rqLockIdField.GetValue(ssm);
            if ((lockId != null) && (oldId != null)) store.ReleaseItemExclusive(System.Web.HttpContext.Current, oldId, lockId);
            rqStateNotFoundField.SetValue(ssm, true);
            rqIdField.SetValue(ssm, newId);
        }


        [HttpPost]
        public ActionResult MapSSOWithCredentials(LogInEntity _Model)
        {

            //try
            //{
            //    UserRegistrationModel _db = new UserRegistrationModel();

            //    if ((_Model.UserId != null && _Model.UserId.Length > 0 && _Model.Password != null && _Model.Password.Length > 0) && Request.Cookies["SSO_ID"].Value != null)
            //    {
            //        string SSOID = Request.Cookies["SSO_ID"].Value;
            //        UserEntity _User = _db.Login(_Model.UserId, _Model.Password, SSOID); // with sso id

            //        if (_User != null && _User.UserId > 0)
            //        {
            //            //Session.Abandon();
            //            Session[SessionIdentity.Admin] = _User;
            //            return RedirectToAction("DashBoard", "User");
            //        }
            //    }
            //    else
            //    {
            //        return Redirect("https://sso.rajasthan.gov.in/signin");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ILogger objILogger = new DBLogger();
            //    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Evaluation.Controllers.Login", FunctionName = "string MapSSOWithCredentials(string UserId, string Password)" }, ex);
            //}

            //return RedirectToAction("MapSSOWithCredentials", "Login");
            Response.Cookies.Add(new System.Web.HttpCookie("ASP.NET_SessionId", "")
            {
                Expires = DateTime.Now.AddDays(-30),
                HttpOnly = true
            });
            try
            {
                UserRegistrationModel _db = new UserRegistrationModel();

                if ((_Model.UserId != null && _Model.UserId.Length > 0 && _Model.Password != null && _Model.Password.Length > 0) && Request.Cookies["SSO_ID"].Value != null)
                {
                    string SSOID = Request.Cookies["SSO_ID"].Value;
                    UserEntity _User = _db.Login(_Model.UserId, _Model.Password, SSOID); // with sso id

                    if (_User != null && _User.UserId > 0)
                    {
                        //Session.Abandon();
                        Session[SessionIdentity.Admin] = _User;
                        regenerateId();
                        return RedirectToAction("DashBoard", "User");
                    }
                }
                else
                {
                    COMMON_FUNCTION.ssoSignout("");
                    return View();
                }
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.Login", FunctionName = "string MapSSOWithCredentials(string UserId, string Password)" }, ex);
            }

            return RedirectToAction("MapSSOWithCredentials", "Login");
        }

        public void DeleteUnwantedCookies(string[] CokkieKeyNotToDelete = null)
        {
            string[] Keys = System.Web.HttpContext.Current.Request.Cookies.AllKeys;
            foreach (var Key in Keys)
            {
                //if (CokkieKeyNotToDelete == null || !CokkieKeyNotToDelete.Any(P => P == Key))
                if (CokkieKeyNotToDelete == null)
                {
                    System.Web.HttpContext.Current.Response.Cookies.Add(new System.Web.HttpCookie(Key)
                    {
                        Value = Guid.NewGuid().ToString(),
                        Expires = DateTime.Now.AddDays(-30),
                        HttpOnly = true
                    });
                }
            }
        }
        #endregion


        #region DepartmentalUserRegistration
        [HttpGet]
        public ActionResult DepartmentalUserRegistration()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Home, (Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        [HttpGet]
        [ActionName("SelectDepartment")]
        public string SelectDepartmentList()
        {
            DepartmentalUserRegistrationModel objDepartmentalUserRegistrationModel = new DepartmentalUserRegistrationModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objDepartmentalUserRegistrationModel.SelectDepartmentList());
        }

        [HttpGet]
        [ActionName("SelectDesignation")]
        public string SelectDesignationList()
        {
            DepartmentalUserRegistrationModel objDepartmentalUserRegistrationModel = new DepartmentalUserRegistrationModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objDepartmentalUserRegistrationModel.SelectDesignationList());
        }

        [HttpGet]
        [ActionName("SelectPost")]
        public string SelectPostList()
        {
            DepartmentalUserRegistrationModel objDepartmentalUserRegistrationModel = new DepartmentalUserRegistrationModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objDepartmentalUserRegistrationModel.SelectPostList());
        }

        [HttpGet]
        [ActionName("SelectRole")]
        public string SelectRoleList()
        {
            DepartmentalUserRegistrationModel objDepartmentalUserRegistrationModel = new DepartmentalUserRegistrationModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objDepartmentalUserRegistrationModel.SelectRoleList());
        }

        [HttpPost]
        [ActionName("SaveUser")]
        public string UserInsert(DepartmentalUserRegistrationModel _Model)
        {
            string[] keys = { };
            if (_Model.IsRuralUrban)
                keys = new[] { "Password", "TehsilId", "CityId", "WardId" };
            else
                keys = new[] { "Password", "BlockId", "GPId", "VillageId" };

            foreach (string str in keys)
                ModelState.Remove(str);

            if (!ModelState.IsValid)
                return JsonModelError.Get(ModelState.Values);

            DepartmentalUserRegistrationModel objDepartmentalUserRegistrationModel = new DepartmentalUserRegistrationModel(new DBLogger());
            return CommonMessageFactory.Response(CommonMessageFactory.Message(objDepartmentalUserRegistrationModel.Add(_Model), (Request.Cookies[SessionIdentity.Culture]).Value));
        }

        [HttpPost]
        [ActionName("ListUsers")]
        public string GetAllUsers(string WhereClause = "")
        {
            DepartmentalUserRegistrationModel objDepartmentalUserRegistrationModel = new DepartmentalUserRegistrationModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objDepartmentalUserRegistrationModel.GetAll(WhereClause));
        }
        #endregion

        [HttpGet]
        public ActionResult UserRegistrationRequest()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.UserRegistrationRequest, (Request.Cookies[SessionIdentity.Culture]).Value);
            return View();
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
                    if (Request.Cookies["SSOROLE"].Value != "CITIZEN")
                    {
                        string SSOID = Request.Cookies["SSO_ID"].Value;
                        SSOWSSoap.SSOUserDetail usrdtl = new SSOWSSoap.SSOUserDetail();
                        SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
                        usrdtl = ssoclient.GetUserDetailXML(SSOID, "RAJSAHKAR.TEST", "Rajasthan@12");
                        if (usrdtl != null && usrdtl.SSOID.Length > 0)
                        {
                            //  UserRegistration.DateOfBirth = usrdtl.dateOfBirth.Length > 3 ? Convert.ToDateTime(usrdtl.dateOfBirth) : System.DateTime.MaxValue;
                            UserRegistration.OfficerNameEn = usrdtl.displayName;
                            UserRegistration.Gender = (usrdtl.gender == "MALE" ? "M" : "F");
                            //UserRegistration.ImageUrl = usrdtl.jpegPhoto;
                            UserRegistration.SSOID = usrdtl.SSOID;
                            UserRegistration.AadharNo = usrdtl.aadhaarId;
                            UserRegistration.MobileNo = usrdtl.mobile;
                            UserRegistration.EmailId = usrdtl.mailPersonal;
                            UserRegistration.DepartmentEn = usrdtl.department;
                            UserRegistration.DesignationEn = usrdtl.designation;
                            UserRegistration.DepartmentId = int.Parse(usrdtl.departmentId == null ? "0" : usrdtl.departmentId);
                            UserRegistration.UnitId = 0;
                            string pathString = "~/SSOUSER/" + usrdtl.SSOID + ".jpg";
                            if (usrdtl.jpegPhoto != null)
                            {
                                //if (!System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath(pathString)) && usrdtl.jpegPhoto.Length > 10)
                                //{
                                //    byte[] imageBytes = Convert.FromBase64String(usrdtl.jpegPhoto);
                                //    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                //    string fileName = usrdtl.SSOID;
                                //    FileStream fs = new FileStream(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/SSOUSER/"), fileName + ".jpg"), FileMode.CreateNew, FileAccess.Write);
                                //    ms.WriteTo(fs);
                                //    fs.Close();
                                //    fs.Dispose();
                                //    ms.Flush();
                                //    UserRegistration.ImageUrl = "~/SSOUSER/ " + fileName + ".jpg";
                                //}
                                //else
                                //    UserRegistration.ImageUrl = pathString;
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

        [HttpPost]
        [ActionName("SaveUserRequest")]
        public string SaveUserRequest(UserEntity _Model)
        {
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<RoleArray> RoleData = new List<RoleArray>();
            try
            {
                RoleData = Request.Form["RoleArray"].ToString().Equals("[]") ? new List<RoleArray>() : (List<RoleArray>)s.Deserialize(Request.Form["RoleArray"].ToString(), typeof(List<RoleArray>));
            }
            catch { return CommonMessageFactory.Response("Provided input data is not in correct format."); }

            //string fileName = string.Empty;
            //_Model.ImageUrl = "";
            //if (Request.Files.Count > 0)
            //{
            //    try
            //    {
            //        var file = Request.Files[0];
            //        fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            //        _Model.ImageUrl = fileName;
            //        file.SaveAs(Path.Combine(Server.MapPath("~/Content/UserImages"), fileName));
            //    }
            //    catch { }
            //}
            _Model.CreatedBy = -1;
            _Model.CreatedIP = "";
            UserRegistrationModel objUserRegistrationModel = new UserRegistrationModel(new DBLogger());
            return CommonMessageFactory.Response(objUserRegistrationModel.Add(_Model, RoleData).ToString());
        }


        [HttpGet]
        [ActionName("BdateUpdate")]
        public void BdateUpdate()
        {
            TrainningModel obj = new TrainningModel();


            DataTable dtUser = obj.GetUserNameList("");

            foreach (DataRow Dr in dtUser.Rows)
            {
                SSOWSSoap.SSOUserDetail usrdtl = new SSOWSSoap.SSOUserDetail();
                SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
                if (Dr["SSO_ID"].ToString() != "")
                {
                    usrdtl = ssoclient.GetUserDetailXML(Dr["SSO_ID"].ToString(), "RAJSAHKAR.TEST", "Rajasthan@12");
                    if (usrdtl.dateOfBirth != "" && usrdtl.dateOfBirth != null)
                        obj.UpdateDob(int.Parse(Dr["USERID"].ToString()), Convert.ToDateTime(usrdtl.dateOfBirth));
                }
            }
        }

        [HttpGet]
        public ActionResult LoginForPS(string Alfa, string Beta)
        {
            Beta = Beta.ToString().Replace(" ", "+");
            Beta = Beta.ToString().Replace(" ", "+");
            Beta = Beta.ToString().Replace(" ", "+");
            string uid = COMMON_FUNCTION.Decrypt(Alfa);
            string passs = COMMON_FUNCTION.Decrypt(Beta);
            UserModel objUserModel = new UserModel();
            //return COMMON_FUNCTION.DataTableToJSONString(objUserModel.AuditDistrictDataReport(uid, passs));


            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", "")
            {
                Expires = DateTime.Now.AddDays(-30),
                HttpOnly = true
            });

            if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(passs))
            {
                return View("Login");
            }


            UserRegistrationModel _db = new UserRegistrationModel(new DBLogger());
            UserEntity _User = _db.Login(uid, passs);
            _User.UserType = "D";
            if (_User != null && _User.UserId > 0)
            {
                Session[SessionIdentity.Admin] = _User;
                regenerateId();
                return RedirectToAction("DashBoard", "User");
            }
            else
            {
                return View("Login");
            }
        }


        [HttpGet]
        public ActionResult LoginBySSO()
        {
            Response.ExpiresAbsolute = DateTime.Now;
            Response.Expires = -1441;
            Response.CacheControl = "no-cache";
            Response.AddHeader("Pragma", "no-cache");
            Response.AddHeader("Pragma", "no-store");
            Response.AddHeader("cache-control", "no-cache");
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetNoServerCaching();
            Session.Abandon();
            DeleteUnwantedCookies();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", "")
            {
                Value = Guid.NewGuid().ToString(),
                HttpOnly = true
            });
            UserRegistrationModel _db = new UserRegistrationModel(new DBLogger());
            DataTable dt = _db.GetSSOUserList();
            ViewBag.RoleList = dt.AsEnumerable().Select(F => new { OfficerNameEn = F.Field<string>("RoleName_EN"), UserId = F.Field<int>("RoleId") }).Distinct().Select(D => new UserEntity { OfficerNameEn = D.OfficerNameEn, UserId = D.UserId }).ToList();
            return View();
        }

        [HttpPost]
        public JsonResult SSOUserList(string RoleId)
        {
            UserRegistrationModel _db = new UserRegistrationModel(new DBLogger());
            DataTable dt = _db.GetSSOUserList();
            var UserList = dt.AsEnumerable().Where(s => s.Field<int>("RoleId") == Convert.ToInt32(RoleId)).Select(m => new { OfficerNameEn = m.Field<string>("OfficerNameEn"), SSOID = m.Field<string>("SSO_ID") }).Distinct().Select(m => new UserEntity { OfficerNameEn = m.OfficerNameEn, SSOID = m.SSOID }).ToList();
            return Json(UserList);
        }

        [HttpGet]
        [ActionName("UserSelfReqList")]
        public string UserSelfReqList(string SSOID = "")
        {
            UserRegistrationModel objUserRoleModel = new UserRegistrationModel(new DBLogger());

            if (SSOID == "")
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SSOID = _LoggedInUser.SSOID;
            }
            if (SSOID == "")
            {
                return "-1";
            }

            return COMMON_FUNCTION.DataTableToJSONString(objUserRoleModel.UserSelfReqList(SSOID));
        }

        //[HttpPost]
        public JsonResult CheckUserSession()
        {
            if (((Cooperative.Models.UserEntity)System.Web.HttpContext.Current.Session[Cooperative.Controllers.SessionIdentity.Admin]) == null)
                return Json(new { Status = false }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetChatFAQ(string SearchText, string LN = "en-us")
        {
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(new AuditorModel(new FileLogger()).GetChatFAQ(SearchText, LN)) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReturnToSWCS(int submitStatus = 3, string SocietyIdEnc = "")
        {
            if (submitStatus <= 3)
            {
                if (System.Web.HttpContext.Current.Session["ISRAJNIVESH"]?.ToString() == "1")
                {
                    SWSEntity sWSEntity = JsonConvert.DeserializeObject<SWSEntity>(System.Web.HttpContext.Current.Session[SessionIdentity.SWCSData].ToString());
                    RajNiveshModel rajNivesh = new RajNiveshModel(new FileLogger());

                    if (submitStatus == 3)
                    {
                        string poststring = COMMON_FUNCTION.posttopage(GlobalConstant.RAJNIVESH_URL + "servicelanding", sWSEntity.Userdetails, GlobalConstant.RAJNIVESH_APPNAME);
                        return Content(poststring);
                    }
                    else
                    {
                        DataTable dtr = rajNivesh.GetSwcStatusDataBySocietyId(SocietyIdEnc);

                        if (dtr != null && dtr.Rows.Count > 0)
                        {
                            RajNiveshStatusRequest rajNiveshStatus = new RajNiveshStatusRequest();
                            rajNiveshStatus.TRACKERID = Convert.ToInt32(dtr.Rows[0]["SWSID"].ToString());
                            rajNiveshStatus.STATUS_CODE = submitStatus == 0 ? 1 : 3;
                            rajNiveshStatus.REGISTRATION_NUMBER = dtr.Rows[0]["RegNo"].ToString();
                            rajNiveshStatus.REMARKS = "";
                            rajNivesh.UpdateStatus(rajNiveshStatus);
                        }
                        else
                        {
                            var rajNiveshRequest = new RajNiveshRequest();
                            rajNiveshRequest.SWSID = Convert.ToInt32(sWSEntity.SWSID);
                            rajNiveshRequest.ActID = Convert.ToInt32(sWSEntity.ActID);
                            rajNiveshRequest.ActivityID = Convert.ToInt32(sWSEntity.ActivityID);
                            rajNiveshRequest.IsNew = Convert.ToInt32(sWSEntity.IsNew);
                            rajNiveshRequest.SSO_ID = sWSEntity.SSO_ID;
                            rajNiveshRequest.SocietyIdEnc = SocietyIdEnc;
                            rajNiveshRequest.RegNo = sWSEntity.RegNo;
                            string regNo = rajNivesh.InsertSWCSData(rajNiveshRequest);
                            sWSEntity.RegNo = regNo;

                            System.Web.HttpContext.Current.Session[SessionIdentity.SWCSData] = sWSEntity;
                            string poststring = COMMON_FUNCTION.posttopage(GlobalConstant.RAJNIVESH_URL + "servicelanding" + $"?SWSID={sWSEntity.SWSID}&ActID={sWSEntity.ActID}&ActivityID={sWSEntity.ActivityID}&IsNew={sWSEntity.IsNew}&RegNo={sWSEntity.RegNo}&status=1", sWSEntity.Userdetails, GlobalConstant.RAJNIVESH_APPNAME);
                            return Content(poststring);
                        }
                    }
                }

            }
            return RedirectToAction("SocietyApplicationForm", "Citizen");

        }

        public string GetUserDetailsBySSO(string SSOID)
        {
            var jsonSerialiser = new JavaScriptSerializer();

            UserEntity UserRegistration = new UserEntity();

            try
            {

                //string SSOID = Request.Cookies["SSOID"].Value;
                SSOWSSoap.SSOUserDetail usrdtl = new SSOWSSoap.SSOUserDetail();
                SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
                usrdtl = ssoclient.GetUserDetailXML(SSOID, "RAJSAHKAR.TEST", "Rajasthan@12");
                if (usrdtl != null && usrdtl.SSOID.Length > 0)
                {

                    UserRegistrationModel objUserRoleModel = new UserRegistrationModel(new DBLogger());
                    DataTable userData = objUserRoleModel.GetUserDetailsBySSO(SSOID);
                    if (userData.Rows.Count > 0)
                    {
                        UserRegistration.EmailId = userData.Rows[0]["EmailId"] != null ? userData.Rows[0]["EmailId"].ToString() : "";
                        UserRegistration.UserId = userData.Rows[0]["UserId"] != null ? Convert.ToInt32(userData.Rows[0]["UserId"].ToString()) : 0;
                    }
                    //  UserRegistration.DateOfBirth = usrdtl.dateOfBirth.Length > 3 ? Convert.ToDateTime(usrdtl.dateOfBirth) : System.DateTime.MaxValue;
                    UserRegistration.OfficerNameEn = usrdtl.displayName;
                    UserRegistration.Gender = (usrdtl.gender == "MALE" ? "M" : "F");
                    //UserRegistration.ImageUrl = usrdtl.jpegPhoto;
                    UserRegistration.SSOID = usrdtl.SSOID;
                    UserRegistration.AadharNo = usrdtl.aadhaarId;
                    UserRegistration.MobileNo = usrdtl.mobile;
                    //UserRegistration.EmailId = usrdtl.mailPersonal;
                    UserRegistration.DepartmentEn = usrdtl.department;
                    UserRegistration.DesignationEn = usrdtl.designation;
                    UserRegistration.DepartmentId = int.Parse(usrdtl.departmentId == null ? "0" : usrdtl.departmentId);
                    UserRegistration.UnitId = 0;
                    string pathString = "~/SSOUSER/" + usrdtl.SSOID + ".jpg";
                    if (usrdtl.jpegPhoto != null)
                    {
                        //if (!System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath(pathString)) && usrdtl.jpegPhoto.Length > 10)
                        //{
                        //    byte[] imageBytes = Convert.FromBase64String(usrdtl.jpegPhoto);
                        //    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                        //    string fileName = usrdtl.SSOID;
                        //    FileStream fs = new FileStream(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/SSOUSER/"), fileName + ".jpg"), FileMode.CreateNew, FileAccess.Write);
                        //    ms.WriteTo(fs);
                        //    fs.Close();
                        //    fs.Dispose();
                        //    ms.Flush();
                        //    UserRegistration.ImageUrl = "~/SSOUSER/ " + fileName + ".jpg";
                        //}
                        //else
                        //    UserRegistration.ImageUrl = pathString;
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
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.LoginController", FunctionName = "GetSSOUserDetails()" }, ex);
            }

            return jsonSerialiser.Serialize(UserRegistration);

        }

        //Bhavesh Citizen List API


        [HttpGet]
      
        [ActionName("GetCitizenDetailList")]
        public string GetCitizenDetailList(string WhereClause = "")
        {

            UserRegistrationModel objUserRoleModel = new UserRegistrationModel(new DBLogger());

            return COMMON_FUNCTION.DataTableToJSONString(objUserRoleModel.GetAllCitizenUser(WhereClause));
        }

    }
}