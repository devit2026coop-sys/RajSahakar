using Cooperative.Models;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Cooperative.Controllers
{
    public class EmitraController : BaseController
    {
        UserEntity _LoggedInUser = new UserEntity();

        public EmitraController()
        {
            if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null && System.Web.HttpContext.Current.Session[SessionIdentity.Admin] is UserEntity)
                _LoggedInUser = System.Web.HttpContext.Current.Session[SessionIdentity.Admin] as UserEntity;


            //if (Request == null || Request.Cookies[SessionIdentity.Culture] == null)
            //{
            //    System.Web.HttpContext.Current.Response.Cookies.Add(new System.Web.HttpCookie(SessionIdentity.Culture, "")
            //    {
            //        Value = "hi-in",
            //        HttpOnly = true
            //    });
            //}
        }
        // GET: Emitra
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UploadAccountOpeningCertificate()
        {
            return View();
        }



        [HttpGet]
        public ActionResult GetSocietyByEmitra()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.GetSocietyByEmitra, "hi-in");
            return View();
        }

        [HttpGet]
        public ActionResult SocietyFeePayment()
        {
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.GetSocietyByEmitra, "hi-in");
            return View();
        }

        public ActionResult Emitra()
        {
            ARequest emitra = new ARequest();
            DataTable Dt = null;

            //emitra.SERVICEID = "4690";
            //emitra.SSOID = "Narottam.saini85";
            //emitra.SSOTOKEN = "Narottam.saini.SSOTOKEN";
            //emitra.KIOSKCODE = "Narottam.saini.KIOSKCODE";
            try
            {
                if (Request["encData"] != null)
                {
                    string encData = Request["encData"]?.ToString();
                    if (Request.Url.Host.Contains("localhost"))
                    {
                        encData = "j+FzxBgFGwEITRjyjs7iCOr2SF4EUKBjmnEr0JOiBZx9tztByxAeDWKH8t3SjKWQzslrC3cDx2Yqa+0j9jnyD9RH2d6qtM3qie5od9RbL6FdxcEhpnrtdyVpIDULWSFcgcs9kv6b5SmIo6yGW0XuJCoJHYMdiMs1NlE7nLy2jyLS5tE+GdSHfy0KfHxyuGqSvzx6OCV/SAs8q9dIfF5nTTW7Bw5YAMwCFIr8UHaEPelQaQKt2J4lsTSMgSUS9gGzfZhsObiQGxqgMKON7ZCDzFL4Pg3hYyfMEC2H90mDSqOJv1Oy1NT3aUvy3UreNQ7kWdgcklCSRrzGXWbTvyNVGBB9mPjQwc3TYlW2JvNwOO9maghUc9Ej/I/hLL2e17T3Xn0FB1NWNz1II4HPjLyJ2sZ63aLyXrOM28KqDk8O8VCunctXVNRATjasMXbzw8c6+5YzmJONWJI0AGZqLNFS27I5soB0B2Ys8G7RpOsxf99v+g7RRmNCqnW2eqpBaYbd9JbXYXDqkNlrPZtA7htMtymqH1JtxK/6Iyf+f1d5QNxPuYQ8OzcNS2P9GagKGb5trHA5C6ICDb5hGs42ihNOmiIWKcs0eJAv4bozLpz1PGreF3451bb4wGyrkne49gtWSwxy0FhMrMVb4A6ioZpONRUYeJ3omwMK+XfzY8g9MIf8UUlsMEU+npoWlQdBNpxYDjpT/AxVjaupOvWfEwq4bHapsdXHSeY/UUtQ52iS/FO0lIiBMeLxCf66Pm5g3XQMghZK77pDh/4O9JM66OO4k4cJQsqeQr4llHBn3F0Au2nsziyToUGpuZNZsiR4Dv5UCAMEclsaNzM/UI9TCUbd+g==";
                    }
                    string decryptData = EmitraModelFactory.Decrypt(encData, "E-m!tr@2016");

                    emitra = JsonConvert.DeserializeObject<ARequest>(decryptData);

                    EmitraModelFactory objEmitraModel = new EmitraModelFactory(new FileLogger());
                    Dt = objEmitraModel.GetApplicationKey(emitra.SERVICEID, true);
                    //Emitra.Decrypt(encData, ConfigurationManager.AppSettings["EmitraKey"].ToString());//SP


                    Dt = new EmitraModelFactory(new FileLogger()).GetApplicationKey(emitra.SERVICEID, true);//
                                                                                                            //Dt = new EmitraModelFactory(new FileLogger()).GetApplicationKey("1111", true);
                                                                                                            //decryptData = EmitraModelFactory.Decrypt(encData, Dt.Rows[0]["encryptionkey"].ToString()); //Emitra.Decrypt(encData, ConfigurationManager.AppSettings["EmitraKey"].ToString());//SP
                                                                                                            //emitra = JsonConvert.DeserializeObject<ARequest>(decryptData);
                    if (Dt.Rows.Count > 0)
                    {
                        Session.Add("ARequest", emitra);
                        Response.Cookies.Add(new HttpCookie("SSO_ID")
                        {
                            Value = emitra.SSOID,
                            HttpOnly = true
                        });
                        Response.Cookies.Add(new HttpCookie("SSOROLE")
                        {
                            Value = "KIOSK",
                            HttpOnly = true
                        });
                        Response.Cookies.Add(new HttpCookie("RAJSSO")
                        {
                            Value = emitra.SSOTOKEN,
                            HttpOnly = true
                        });
                        Response.Cookies.Add(new HttpCookie("EmitraServiceid")
                        {
                            Value = emitra.SERVICEID,
                            HttpOnly = true
                        });

                        if (Dt.Rows[0]["redirecturl"].ToString() != "")
                        {

                            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
                            {
                                if (Response.Cookies["SSOROLE"].Value == "KIOSK")
                                {

                                    SSOWSSoap.SSOUserDetail usrdtl = new SSOWSSoap.SSOUserDetail();
                                    SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
                                    usrdtl = ssoclient.GetUserDetailXML(Request.Cookies["SSO_ID"].Value, "RAJSAHKAR.TEST", "Rajasthan@12");
                                    UserEntity _Entity = new UserEntity();


                                    string pathString = "~/SSOUSER/" + usrdtl.SSOID + ".jpg";
                                    //if (!System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath(pathString)))
                                    //{
                                    //    byte[] imageBytes = Convert.FromBase64String(usrdtl.jpegPhoto);
                                    //    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                    //    string fileName = usrdtl.SSOID;
                                    //    FileStream fs = new FileStream(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/SSOUSER/"), fileName + ".jpg"), FileMode.CreateNew, FileAccess.Write);
                                    //    ms.WriteTo(fs);
                                    //    fs.Close();
                                    //    fs.Dispose();
                                    //    ms.Flush();
                                    //    _Entity.ImageUrl = "../SSOUSER/" + fileName + ".jpg";
                                    //}
                                    //else
                                    _Entity.ImageUrl = pathString.ToString().Replace("~", "..");


                                    _Entity.SSOID = Request.Cookies["SSO_ID"].Value;
                                    _Entity.UserType = "K";
                                    _Entity.KIOSKCODE = emitra.KIOSKCODE;
                                    _Entity.UserId = 3111;
                                    _Entity.RoleId = 10045;
                                    _Entity.UserRoleDeptMappingId = 2193;
                                    _Entity.ServiceId = emitra.SERVICEID;
                                    _Entity.EmitraReturnUrl = emitra.RETURNURL;
                                    _Entity.OfficerNameHi = _Entity.OfficerNameEn = emitra.KIOSKNAME == "" ? emitra.KIOSKCODE : emitra.KIOSKCODE;
                                    System.Web.HttpContext.Current.Session[SessionIdentity.Admin] = _Entity;


                                }
                                else
                                {
                                    UserEntity _Entity = new UserEntity();
                                    _Entity.SSOID = Request.Cookies["SSO_ID"].Value;
                                    _Entity.UserType = "C";
                                    _Entity.UserId = 1030;
                                    System.Web.HttpContext.Current.Session[SessionIdentity.Admin] = _Entity;
                                    return RedirectToAction("ValidateUser", "SSOValidation");

                                }
                            }

                            return RedirectToAction(Dt.Rows[0]["redirecturl"].ToString().Split('/')[1], Dt.Rows[0]["redirecturl"].ToString().Split('/')[0]);
                            // return RedirectToAction("SocietyMemberRegistration", "Citizen");
                        }
                        else
                        {
                            Response.Redirect(emitra.RETURNURL, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.Emitra", FunctionName = "Emitra()" }, ex);
                return View();
            }
            return Redirect("https://sso.rajasthan.gov.in/signin");

        }



        [HttpPost]
        public ActionResult OrderSummary(string Result)
        {

            return View(Result);
        }

        [HttpGet]
        public ActionResult FeeVerification()
        {
            return View();
        }


        [HttpPost]
        public JsonResult PayServices(string AppRequestID, string IsPermanentSave, string ServiceID)
        {

            DataTable DT = null;
            int result = 0;
            string Result = "";
            EmitraModelFactory ObjEmitraModel = new EmitraModelFactory(new DBLogger());
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            DT = ObjEmitraModel.SelectDetailForEmitraPayment(AppRequestID);
            string SocietyIdEnc = "";
            if (DT.Rows.Count > 0 && _LoggedInUser.UserType == "K")
            {
                SocietyIdEnc = DT.Rows[0]["SocietyIdEnc"].ToString();
                if (System.Web.HttpContext.Current.Session["ARequest"] != null && DT.Rows[0]["KioskID"].ToString().Length > 0)
                {
                    result = ObjEmitraModel.MakePayment(DT.Rows[0]["EMitraAppRequestID"].ToString(), DT.Rows[0]["SocietyName"].ToString(), "0", DT.Rows[0]["RegistrationNo"].ToString(), DT.Rows[0]["SocietyIdEnc"].ToString(), Convert.ToInt32(IsPermanentSave));

                    Result = result.ToString();
                    if (int.Parse(Result) > 0)
                    {
                        string FResult = "";
                        int Count = 0;
                        SocietyController obj = new SocietyController();

                        string str = obj.GetSocietyDetailsBySocietyId(DT.Rows[0]["SocietyIdEnc"].ToString());
                        DataTable dt = (DataTable)JsonConvert.DeserializeObject(str, (typeof(DataTable)));
                        if (dt.Rows[0]["RegistrationNo"] == null || dt.Rows[0]["RegistrationNo"].ToString().Length == 0 || dt.Rows[0]["RegistrationNo"].ToString().Trim() == "")
                        {
                            FResult = obj.SaveSocietyFinalSubmition(DT.Rows[0]["SocietyIdEnc"].ToString());
                            var REG = FResult.Split('#');
                            if (REG.Length > 1)
                            {
                                foreach (DataRow Dr in dt.Rows)
                                {
                                    Dr["SocietyDirectory"] = REG[1].ToString().Replace("/", "");
                                }
                            }
                        }
                        //else if (!dt.Rows[0]["RegistrationNo"].ToString().Contains("COOP"))
                        //else if ()
                        //{
                        //    FResult = obj.SaveSocietyFinalSubmition(DT.Rows[0]["SocietyIdEnc"].ToString());
                        //    var REG = FResult.Split('#');
                        //    if (REG.Length > 1)
                        //    {
                        //        foreach (DataRow Dr in dt.Rows)
                        //        {
                        //            Dr["SocietyDirectory"] = REG[1].ToString().Replace("/", "");
                        //        }
                        //    }
                        //}

                        //DataTable selectedTable = dt.AsEnumerable()
                        //    .Where(r => r.Field<string>("AddressFor") == "0")
                        //    .CopyToDataTable();

                        DataTable selectedTable = dt.AsEnumerable()
                        .Where(r => r.Field<string>("AddressFor") == "0" || r.Field<string>("AddressFor") == "")
                        .CopyToDataTable();


                        dt = selectedTable;
                        string pathString = "~/Content/SocietyProfiling/";
                        string SourceFilePath = "";
                        string TargetFilePath = "~/Content/SocietyProfiling/";
                        if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["SocietyDirectory"].ToString() != null && dt.Rows[0]["SocietyDirectory"].ToString() != "" && dt.Rows[0]["SocietyDirectory"].ToString() != "undefined")
                        {
                            foreach (DataRow Dr in dt.Rows)
                            {
                                if (Count == 0)
                                {
                                    pathString += Regex.Replace(dt.Rows[0]["SocietyDirectory"].ToString(), "[^a-zA-Z0-9_]+", "");
                                    if (!Directory.Exists(Server.MapPath(pathString)))
                                    {
                                        Directory.CreateDirectory(Server.MapPath(pathString));
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



                                    //if (Dr["RSSCFileName"] != null && !Dr["RSSCFileName"].ToString().Contains("undefined") && Dr["RSSCFileName"].ToString() != "")
                                    //{
                                    //    if ((Dr["RSSCFileName"].ToString().Split('/')).Length > 3)
                                    //    {
                                    //        var temp = Dr["RSSCFileName"].ToString().Split('/');
                                    //        var TempRSSCFileName = Dr["RSSCFileName"].ToString().Split('/')[3];
                                    //        SourceFilePath = "~/Content/NewRegistration/" + TempRSSCFileName;
                                    //        TargetFilePath = pathString + "/" + TempRSSCFileName;
                                    //        if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                    //            System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                    //    }
                                    //}


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
                        }
                    }

                }
            }
            else if (DT.Rows.Count > 0 && _LoggedInUser.UserType == "C" || _LoggedInUser.UserType == "D")
            {
                ILogger objILogger = new DBLogger();
                SocietyIdEnc = DT.Rows[0]["SocietyIdEnc"].ToString();
                Result = ObjEmitraModel.UpdateEmitrTransation_New(DT.Rows[0]["EMitraAppRequestID"].ToString(), DT.Rows[0]["SocietyName"].ToString(), "0", DT.Rows[0]["RegistrationNo"].ToString(), DT.Rows[0]["SocietyIdEnc"].ToString(), DT.Rows[0]["SERICEID"].ToString(), false, IsPermanentSave);
                Result = Result.ToString();
                if (int.Parse(Result) > 0)
                {
                    string FResult = "";
                    int Count = 0;
                    SocietyController obj = new SocietyController();

                    string str = obj.GetSocietyDetailsBySocietyId(DT.Rows[0]["SocietyIdEnc"].ToString());
                    DataTable dt = (DataTable)JsonConvert.DeserializeObject(str, (typeof(DataTable)));
                    if (dt.Rows[0]["RegistrationNo"] == null || dt.Rows[0]["RegistrationNo"].ToString().Length == 0 || dt.Rows[0]["RegistrationNo"].ToString().Trim() == "")
                    {
                        FResult = obj.SaveSocietyFinalSubmition(DT.Rows[0]["SocietyIdEnc"].ToString());
                        var REG = FResult.Split('#');
                        if (REG.Length > 1)
                        {
                            foreach (DataRow Dr in dt.Rows)
                            {
                                Dr["SocietyDirectory"] = REG[1].ToString().Replace("/", "");
                            }
                        }
                    }
                    ////else if (!dt.Rows[0]["RegistrationNo"].ToString().Contains("COOP"))
                    //else if (dt.Rows[0]["RegistrationNo"].ToString().Trim() == "")
                    //{
                    //    FResult = obj.SaveSocietyFinalSubmition(DT.Rows[0]["SocietyIdEnc"].ToString());
                    //    var REG = FResult.Split('#');
                    //    if (REG.Length > 1)
                    //    {
                    //        foreach (DataRow Dr in dt.Rows)
                    //        {
                    //            Dr["SocietyDirectory"] = REG[1].ToString().Replace("/", "");
                    //        }
                    //    }
                    //}

                    DataTable selectedTable = dt.AsEnumerable()
                        .Where(r => r.Field<string>("AddressFor") == "0" || r.Field<string>("AddressFor") == "")
                        .CopyToDataTable();

                    dt = selectedTable;
                    string pathString = "~/Content/SocietyProfiling/";
                    string SourceFilePath = "";
                    string TargetFilePath = "~/Content/SocietyProfiling/";
                    if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["SocietyDirectory"].ToString() != null && dt.Rows[0]["SocietyDirectory"].ToString() != "" && dt.Rows[0]["SocietyDirectory"].ToString() != "undefined")
                    {
                        foreach (DataRow Dr in dt.Rows)
                        {
                            if (Count == 0)
                            {
                                pathString += Regex.Replace(dt.Rows[0]["SocietyDirectory"].ToString(), "[^a-zA-Z0-9_]+", "");
                                if (!Directory.Exists(Server.MapPath(pathString)))
                                {
                                    ApplicationFlowModel AppFlowModal = new ApplicationFlowModel();
                                    //AppFlowModal.SendMSG(SocietyIdEnc, -2);
                                    Directory.CreateDirectory(Server.MapPath(pathString));
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


                                        new SocietyModel(new DBLogger()).UploadMemberExcelData(TargetFilePath, TempRegistrationCertificateFile, DT.Rows[0]["SocietyIdEnc"].ToString(), Convert.ToInt32(DT.Rows[0]["ActId"].ToString()));

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



                                //if (Dr["RSSCFileName"] != null && !Dr["RSSCFileName"].ToString().Contains("undefined") && Dr["RSSCFileName"].ToString() != "")
                                //{
                                //    if ((Dr["RSSCFileName"].ToString().Split('/')).Length > 3)
                                //    {
                                //        var temp = Dr["RSSCFileName"].ToString().Split('/');
                                //        var TempRSSCFileName = Dr["RSSCFileName"].ToString().Split('/')[3];
                                //        SourceFilePath = "~/Content/NewRegistration/" + TempRSSCFileName;
                                //        TargetFilePath = pathString + "/" + TempRSSCFileName;
                                //        if (System.IO.File.Exists(Server.MapPath(SourceFilePath)))
                                //            System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                //    }
                                //}


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
                                        var temp = Dr["BalanceSheetExcelFile"].ToString().Split('/');
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
                                    {
                                        if (!System.IO.File.Exists(Server.MapPath(TargetFilePath)))
                                            System.IO.File.Move(Server.MapPath(SourceFilePath), Server.MapPath(TargetFilePath));
                                    }

                                }
                            }
                            Count = Count + 1;
                        }
                    }
                }
            }

            return Json(new { status = "OK", UserType = _LoggedInUser.UserType, result = Result, SocietyIdEnc = SocietyIdEnc }, JsonRequestBehavior.AllowGet);

        }




        [HttpGet]
        [ActionName("EmitraFeePayment")]
        public string SocietyPaymentdata(string SocietyRegistrationNo)
        {
            string WhereClause = " and Societies.RegistrationNo='" + SocietyRegistrationNo + "'";
            EmitraModelFactory objEmitraModel = new EmitraModelFactory(new DBLogger());
            DataTable dt = objEmitraModel.GetSocietyPaymentList(WhereClause);
            return COMMON_FUNCTION.DataTableToJSONString(dt); ;
        }

        [HttpGet]
        [ActionName("EmitraUploadBankCertificate")]
        public string SocietyForUploadBankCertificate(string SocietyRegistrationNo)
        {
            string WhereClause = " and Societies.RegistrationNo='" + SocietyRegistrationNo + "'";
            EmitraModelFactory objEmitraModel = new EmitraModelFactory(new DBLogger());
            DataTable dt = objEmitraModel.SocietyForUploadBankCertificate(WhereClause);
            return COMMON_FUNCTION.DataTableToJSONString(dt); ;
        }

        [HttpPost]
        [ActionName("FeePaymentByEmitra")]
        public string FeePaymentByEmitra(string RegistrationNo, string IsPermanentSave = "0")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            DataTable DT = null;
            string RetunMsg = "";
            string WhereClause = " and Societies.RegistrationNo='" + RegistrationNo + "'";
            EmitraModelFactory objEmitraModel = new EmitraModelFactory(new DBLogger());
            DT = objEmitraModel.GetSocietyPaymentList(WhereClause);
            if (_LoggedInUser.UserType == "K")
            {
                if (DT.Rows.Count > 0)
                {
                    RetunMsg = objEmitraModel.FeeDeposit(DT.Rows[0]["SocietyIdEnc"].ToString(), DT.Rows[0]["RegistrationNo"].ToString(), DT.Rows[0]["SocietyName"].ToString(), IsPermanentSave, DT.Rows[0]["Amount"].ToString());
                }
            }
            else if (DT.Rows.Count > 0 && _LoggedInUser.UserType == "C")
            {
                RetunMsg = objEmitraModel.UpdateEmitrTransation(DT.Rows[0]["UniqueRequestID"].ToString(), DT.Rows[0]["SocietyName"].ToString(), "0", DT.Rows[0]["RegistrationNo"].ToString(), DT.Rows[0]["SocietyId"].ToString(), DT.Rows[0]["SocietyIdEnc"].ToString(), "4173");
                //UpdateEmitrTransation
            }
            return RetunMsg;
            // return Json(new { status = "OK", UserType = _LoggedInUser.UserType, result = RetunMsg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("RedirectToProfilePage")]
        public string RedirectToProfilePage(string SocietyRegistrationNo, string Identity)
        {
            Identity = Identity.Replace(" ", "+");
            Identity = Identity.Replace(" ", "+");
            Identity = Identity.Replace(" ", "+");
            if (SocietyRegistrationNo != null)
            {
                string WhereClause = "";
                WhereClause = " and Societies.RegistrationNo='" + SocietyRegistrationNo + "'";

                CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
                DataTable dt = objCitizenModel.GetSocityDataForOtpSending(WhereClause);
                System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc] = dt.Rows[0]["SocietyIDEnc"].ToString();
                System.Web.HttpContext.Current.Session[SessionIdentity.Identity] = Identity.ToString();
            }
            return "1";
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
            if (ObjSocietyRegistration.ServicesId == 5530 && returnvalue == "1")
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

                        if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(pathString)))
                        {
                            Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(pathString));
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


        [HttpPost]
        public JsonResult PayServicesForAll(string AppRequestID, string ServiceID)
        {
            DataTable DT = null;
            int result = 0;
            string Result = "";
            EmitraModelFactory ObjEmitraModel = new EmitraModelFactory(new DBLogger());
            SocietyMemberRegistrationModel objmodel = new SocietyMemberRegistrationModel(new DBLogger());
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            //DT = ObjEmitraModel.SelectDetailForEmitraPayment(AppRequestID, ServiceID);
            DT = objmodel.GetOrganizationUserRegistrationDetails(" and A.[RequestId]=" + AppRequestID);
            string SocietyId = "";
            if (_LoggedInUser.UserType == "K")
            {
                //SocietyId = DT.Rows[0]["SocietyId"].ToString();
                if (System.Web.HttpContext.Current.Session["ARequest"] != null && _LoggedInUser.KIOSKCODE.Length > 0)
                {
                    result = ObjEmitraModel.MakePaymentForAll("0", DT.Rows[0]["Name"].ToString(), "0", DT.Rows[0]["ApplicationNo"].ToString(), AppRequestID, _LoggedInUser.KIOSKCODE, _LoggedInUser.SSOID);

                    Result = result.ToString();
                    if (result > 0)
                    {
                        Result = "Sucess";
                    }
                    else
                    {
                        Result = "Faild";
                    }

                    SocietyId += COMMON_FUNCTION.Encrypt(result.ToString());
                }
            }
            else if (_LoggedInUser.UserType == "C")
            {
                //ILogger objILogger = new DBLogger();

                //objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "SocietyList(SocietyRegistrationEntity Obj)" }, new Exception("IN PayServices"));

                Result = ObjEmitraModel.UpdateEmitrTransationAll("0", DT.Rows[0]["Name"].ToString(), "0", DT.Rows[0]["ApplicationNo"].ToString(), AppRequestID, "4690".ToString(), _LoggedInUser.KIOSKCODE, _LoggedInUser.SSOID, false);
                //Result = AppRequestID;
            }

            return Json(new { status = "OK", UserType = _LoggedInUser.UserType, result = Result, IdEnc = SocietyId }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult UserSocietyMappingValid()
        {
            //UserModel OBJUser = new UserModel(new FileLogger());
            //bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            //if (Result == false)
            //{
            //    return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            //}
            //else
            //{
            ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.UserSocietyMappingValid, "hi-in");
            //}
            return View();
        }

        [HttpPost]
        public int SocietyMappingInsertByEmitra(SocietyMappingEntity model)
        {
            // model.SocietyId = model.SocietyId;

            if (_LoggedInUser.UserType.ToUpper() == "K")
                model.RequestBySSOId = "";
            else
                model.RequestBySSOId = _LoggedInUser.SSOID;

            model.UserType = _LoggedInUser.UserType;

            //model.AADHARNoEnc = model.AADHARNoEnc;
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            int Result = objCitizenModel.SocietyMappingInsertByEmitra(model);

            if (Result > 0) // Send SMS to the requester [CEO as Citizen]
            {
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


        [AllowAnonymous]
        public ActionResult UpdatePaymentCAAuditorRegistration(string UniquerequestId = "", string IsFailed = "")
        {
            string encData = "";
            DataTable Dt = null;
            string decryptData = "";
            PGResponse objPGResponse = null;

            string DivHtml = "";

            UniquerequestId = UniquerequestId.Replace(' ', '+');
            UniquerequestId = UniquerequestId.Replace(' ', '+');
            UniquerequestId = UniquerequestId.Replace(' ', '+');

            // SocietyIdEnc = SocietyIdEnc.Replace(' ', '+');
            // SocietyIdEnc = SocietyIdEnc.Replace(' ', '+');
            //  SocietyIdEnc = SocietyIdEnc.Replace(' ', '+');

            IsFailed = IsFailed.Replace(' ', '+');
            IsFailed = IsFailed.Replace(' ', '+');
            IsFailed = IsFailed.Replace(' ', '+');


            // IsPermanentSave = IsPermanentSave.Replace(' ', '+');
            //  IsPermanentSave = IsPermanentSave.Replace(' ', '+');
            //  IsPermanentSave = IsPermanentSave.Replace(' ', '+');

            UserModel OBJUser = new UserModel(new FileLogger());
            if (Request["encData"] != null)
            {
                EmitraModelFactory objEmitraModel = new EmitraModelFactory(new FileLogger());
                Dt = objEmitraModel.GetApplicationKey("7392", false);
                encData = Request["encData"].ToString().Replace("@!", "+");
                EncryptDecrypt3DES obj = new EncryptDecrypt3DES(Dt.Rows[0]["encryptionkey"].ToString());
                decryptData = obj.decrypt(encData);
                CitizenModel objCitizenModel = new CitizenModel(new FileLogger());
                (new EmitraModelFactory(new FileLogger())).SavePaymentResponce("Auditor Registration :  IsFail=" + COMMON_FUNCTION.Decrypt(IsFailed) + "--:-- " + decryptData);
                objPGResponse = JsonConvert.DeserializeObject<PGResponse>(decryptData);
                Dt = new EmitraModelFactory(new FileLogger()).GetApplicationKey(objPGResponse.SERVICEID, true);

                DivHtml = objCitizenModel.UpdateOnlinePaymentStatus("", "", COMMON_FUNCTION.Decrypt(UniquerequestId), COMMON_FUNCTION.Decrypt(IsFailed), decryptData, "1");

                objPGResponse = JsonConvert.DeserializeObject<PGResponse>(DivHtml);
                if (objPGResponse.RESPONSEMESSAGE.ToString().Contains("Successfully"))
                {
                    SetCitizonSesion(objPGResponse.UDF2);
                    if (COMMON_FUNCTION.Decrypt(IsFailed) == "NO")
                    {
                        AuditorModel objEM = new AuditorModel(new FileLogger());
                        objEM.SetAuditEmpanelledFinalSubmit(COMMON_FUNCTION.Decrypt(UniquerequestId));
                    }
                    return RedirectToAction("EmpanelledAudit", "Auditor");

                }
                else
                {
                    SetCitizonSesion(objPGResponse.UDF2);
                    return RedirectToAction("EmpanelledAudit", "Auditor");

                }
                //ViewBag.DivHtmlData = DivHtml;
            }
            return View();
            //return "OKAY";
        }


        [AllowAnonymous]
        public ActionResult UpdatePaymentPLDBMembership(string UniquerequestId = "", string IsFailed = "")
        {
            string encData = "";
            DataTable Dt = null;
            string decryptData = "";
            PGResponse objPGResponse = null;

            string DivHtml = "";

            UniquerequestId = UniquerequestId.Replace(' ', '+');
            UniquerequestId = UniquerequestId.Replace(' ', '+');
            UniquerequestId = UniquerequestId.Replace(' ', '+');

            // SocietyIdEnc = SocietyIdEnc.Replace(' ', '+');
            // SocietyIdEnc = SocietyIdEnc.Replace(' ', '+');
            //  SocietyIdEnc = SocietyIdEnc.Replace(' ', '+');

            IsFailed = IsFailed.Replace(' ', '+');
            IsFailed = IsFailed.Replace(' ', '+');
            IsFailed = IsFailed.Replace(' ', '+');


            // IsPermanentSave = IsPermanentSave.Replace(' ', '+');
            //  IsPermanentSave = IsPermanentSave.Replace(' ', '+');
            //  IsPermanentSave = IsPermanentSave.Replace(' ', '+');

            UserModel OBJUser = new UserModel(new FileLogger());
            if (Request["encData"] != null)
            {
                EmitraModelFactory objEmitraModel = new EmitraModelFactory(new FileLogger());
                Dt = objEmitraModel.GetApplicationKey("7392", false);
                encData = Request["encData"].ToString().Replace("@!", "+");
                EncryptDecrypt3DES obj = new EncryptDecrypt3DES(Dt.Rows[0]["encryptionkey"].ToString());
                decryptData = obj.decrypt(encData);
                CitizenModel objCitizenModel = new CitizenModel(new FileLogger());
                (new EmitraModelFactory(new FileLogger())).SavePaymentResponce("Auditor Registration :  IsFail=" + COMMON_FUNCTION.Decrypt(IsFailed) + "--:-- " + decryptData);
                objPGResponse = JsonConvert.DeserializeObject<PGResponse>(decryptData);
                Dt = new EmitraModelFactory(new FileLogger()).GetApplicationKey(objPGResponse.SERVICEID, true);

                DivHtml = objCitizenModel.UpdateOnlinePaymentStatus("", "", COMMON_FUNCTION.Decrypt(UniquerequestId), COMMON_FUNCTION.Decrypt(IsFailed), decryptData, "1");

                objPGResponse = JsonConvert.DeserializeObject<PGResponse>(DivHtml);
                if (objPGResponse.RESPONSEMESSAGE.ToString().Contains("Successfully"))
                {
                    SetCitizonSesion(objPGResponse.UDF2);
                    if (COMMON_FUNCTION.Decrypt(IsFailed) == "NO")
                    {
                        var result = objCitizenModel.SetSocietyMemberRegistrationFinalSubmit(COMMON_FUNCTION.Decrypt(UniquerequestId));
                        if (result != "-1")
                        {
                            string message = "Your membership application submitted successfully. Application No:-" + result.Split('#')[1];
                            new EsanchaarController().SendSocietySMS(message, result.Split('#')[0]);
                        }
                    }
                    return RedirectToAction("SocietyMemberRegistration", "PLDBMemberRegistration");

                }
                else
                {
                    SetCitizonSesion(objPGResponse.UDF2);
                    return RedirectToAction("SocietyMemberRegistration", "PLDBMemberRegistration");
                    (new ApplicationFlowController()).CertificateGenerate("");
                }
                //ViewBag.DivHtmlData = DivHtml;
            }
            return View();
            //return "OKAY";
        }


        public void SetCitizonSesion(string SSO_ID)
        {
            UserEntity _Entity = new UserEntity();
            SSOWSSoap.SSOUserDetail usrdtl = new SSOWSSoap.SSOUserDetail();
            SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
            usrdtl = ssoclient.GetUserDetailXML(SSO_ID, "RAJSAHKAR.TEST", "Rajasthan@12");
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
                _Entity.AadharNo = usrdtl.aadhaarId;
                _Entity.OfficerNameEn = usrdtl.displayName + "(" + usrdtl.designation + ")";
            }
            else
            {
                _Entity.AadharNo = "";
                _Entity.OfficerNameEn = "";
            }

            _Entity.ImageUrl = pathString;
            _Entity.SSOID = SSO_ID;
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
            (new LoginController()).regenerateId();
            if (_Entity.IsFromSWC == "YES")
            {
                System.Web.HttpContext.Current.Session[SessionIdentity.SWCSData] = Request.Cookies["SWC_DATA"].Value;
                if (Request.Url.Host.Contains("localhost"))
                {

                    //  return RedirectToAction(RedirectURL);
                }
                else
                {
                    //return RedirectToAction(RedirectURL);
                }
            }
            else
            {
                System.Web.HttpContext.Current.Session[SessionIdentity.SWCSData] = null;
                // return RedirectToAction(RedirectURL);
            }
        }

    }
}