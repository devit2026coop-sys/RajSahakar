using Cooperative.Helper;
using Cooperative.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RAJSSO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace Cooperative.Controllers
{
    public class SSOValidationController : Controller
    {
        /// <summary>
        /// For Live
        /// </summary>
        /// <returns></returns>
        public ActionResult ValidateUser()
        {

            //btnSelect_Click();

            //GetUserDetails();
            // Margepdf();
            //SaveOldSitePhoto();

            //string qrcode=  COMMON_FUNCTION.GenerateQRCode("COOP2021HANUMANGARHCOS200208/1C45059D0FDC4412AE2DCB396CB677A5.pdf");

            // COMMON_FUNCTION.AddStampOnPDF(System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/22233123/1B5553AB224B46E59352653E37ACB9F1.pdf"), System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/22233123/1B5553AB224B46E59352653E37ACB9F121.pdf"), qrcode);

            //(new ApplicationFlowController()).CertificateGenerate("0x02000000A8B95DA6040A0C82832F35C4110C3973BCD08DACAD30C43568AD3ECC7CB01FB186670B6CB6B00352D28EC2EDB4A6C7BB");

            //GetMembersCert(qrcode);

            //var otpdata=  COMMON_FUNCTION.GenerateOTPForEsingh("");
            // var otp = "";
            // var data = COMMON_FUNCTION.VerifyAadhaarOTP(otp, otpdata);


            //CertificateGenerateAmendment(data.data);

            //GetUserDetails();

            // UserDetailsEntity1 dtls2 = new UserDetailsEntity1();

            //GetData1();
            //(
            //new CommonServicesController()).GetDataFromProd("1323221");
            //GenerateAuditCompletePDF();
            //GetMembersCert();
            //  string _txnid = string.Empty;
            //_txnid = COMMON_FUNCTION.SendOtpByAadharNo("559944205715");

            //CertificateGenerateAmendment1111();
            // CertificateGenerateAmendment();
            ////SSOWSSoap.SSOUserDetail usrdtl = new SSOWSSoap.SSOUserDetail();
            ////SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
            ////usrdtl = ssoclient.GetUserDetailXML("GURJARRAJESH63323", "RAJSAHKAR.TEST", "Rajasthan@12");
            //string aadhaarId = usrdtl.aadhaarId;
            ////suroliasameer
            //var aadhar= COMMON_FUNCTION.GetAadharFromDSMService(aadhaarId);

            //HTMLtoPDF();

            // string[] arr = new string[]{   "tkfp1fpQmxDWxbUWGpnTjw=="
            //,"T1+Bhe8Ju9s4QjBaM66sAg=="
            //,"LSI04YPL1ZO2io+dCmo23A=="
            //,"5GxJU3NHc43mJYLG/XtYlQ=="
            //,"2Q6gTxXEZawquwd7/rVcLQ=="
            //,"T7p1DdoePvNLAOEY1uo3rQ=="
            //,"/knZbH2K9AyDqxyeh7QO7w=="
            //,"E6S/GHDCSfVbYITDBc3/wQ=="
            //,"6tVJNhQ7HeKhJm8ECs9eVw=="
            //,"qAEJJyF+6NuSLJBMGoK84w=="
            //,"7zoVkITe60tNEDUp9X0zRQ=="
            //,"2BgAqn6uld4VKsugFDTKkA=="
            //,"wDazbfmJJjFphAC3pTHM9w=="
            //,"0UYk2HBxGWWxuXmQ01vWkA=="
            //,"0Ua1/FOhjUw0F/wpetJuUg=="
            //,"mkrSW2Gx2U+NzUMGdE9dPA=="
            //,"3eMWzKly9oRo7ugIx16ZqA=="
            //,"de/rfZdWkDsbCYkkrAGGfA==" };
            //           string str = "";
            //            for (int i = 0; i<=arr.Length-1; i++) 
            //            {
            //              str+= ","+ COMMON_FUNCTION.Decrypt(arr[i]);
            //            }

            //string str = COMMON_FUNCTION.Decrypt(item.Field<string>("AADHAR"));

            // var data = UnicodeToKrutidev.FindAndReplaceKrutidev(("पंजीयन अनूपगढ़ आवेदन फॉर्म के साथ ऑनलाइन जमा होगा ").ToString().Replace("<br>", "<br/>"));

            //string SSOID = "938972367245";//dr["SSOID"].ToString();
            //SSOWSSoap.SSOUserDetail usrdtl = new SSOWSSoap.SSOUserDetail();
            //SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
            //usrdtl = ssoclient.GetUserDetailXML(SSOID, "RAJSAHKAR.TEST", "Rajasthan@12");

            //SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
            //string Get_Userdetails = ssoclient.GetTokenDetail("T2IzWEE3MnczbXp1SHEzeEdDSFMrU09NMHZ6OG1vdzV6MHJaOG5VV0U1dExpK2Z0dERHR09tZGpNR0xzR1gxRFlIeWZvdTdwV1hqdXZKQW1NUStQQXFpOFlNSGtUMnZDL2haQmJDWVFPc2pGMWFQbUJsSS9KczFTaUtzemJSMGV6SkhDSDdYZEdPQVRUYk1DQmY2MEpHVC9oamtsRVVTVzQ0NHJsVGpGZjdBPQ");
            ////GetData();
            //HTMLtoPDF();

            //FileTestLogger objILoggerTest = new FileTestLogger();
            //objILoggerTest.ErrorHandleTest(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SSOValidation", FunctionName = "ActionResult ValidateUser()" }, "SSO.CreateSSOSession working 1");


            //string SS = COMMON_FUNCTION.Encrypt("588954093818");//B
            //SS = COMMON_FUNCTION.Encrypt("550649276302");//V UG9KOICH5kOa+16pnDFnHg==
            //string  SS = COMMON_FUNCTION.Encrypt("428434778643");//R
            //SS = COMMON_FUNCTION.Encrypt("307691961366");//N  
            //SS = COMMON_FUNCTION.Encrypt("940396382715");//B
            // string SSs = COMMON_FUNCTION.Encrypt("939695846851");
            SSO SSO = new SSO();
            Response.ExpiresAbsolute = DateTime.Now;
            Response.Expires = -1441;
            Response.CacheControl = "no-cache";
            Response.AddHeader("Pragma", "no-cache");
            Response.AddHeader("Pragma", "no-store");
            Response.AddHeader("cache-control", "no-cache");
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetNoServerCaching();


            //OfficerNameEn = ABHAY KUMAR IAS    MobileNo = 9602367111
            //OfficerNameEn = NIRAJ KUMAR PAWAN  MobileNo = 9414119911
            //OfficerNameEn = Umesh chandra Joshi  MobileNo = 8085882869
            //OfficerNameEn = K N Sharma  MobileNo = 9414035286
            // COMMON_FUNCTION.SendMSGForAdmin("9602367111");  PS SIR
            //COMMON_FUNCTION.SendMSGForAdmin("9414119911"); //RCS  (NIRAJ KUMAR PAWAN)
            //var temp = "9950663082,9929272913,8290192623,9587487004,9587487004,9057578597,9887211868,8619274862,9414795534,9785526770,9782901297,8078628582,7691031280,9784543863,9571737829,7891300356,9660839747,9694313705,9680390152,9785640362,9414208662,9829161318,9667908342,9784289927,9950850106,7891204967,7742703019,9887561294,6350129131,9950570566,9950570566,9887211095,9887357266,9950913871,9468892142,9468892142,9468892142,9468892142,9468892142,9928577309,9829169121,9784327010,9950850014,9928922425,9928922425,9602994055,9587681635,9413529145,9694785968,9602181440,9887527344,6377099487,9829030648,9829030648,8290597615,8290597615,9667109293,7665479813,9950360992,9950360992,9928002000,8290948773,9462607260,9784012090,8764037299,8094195080,9829335152,7891034715,7891034715,9166128102,9001182811,9950925641,8058341591,9929176312,7240183020,9414594129,7597161522,7597161522,9785538085,9785538085,9414232490,9799221020,7568883277,9982737348,9571443549,9414419945".ToString().Split(',');
            //var temp = "9461617706,8114421898,9461569464".ToString().Split(',');
            //for (int i = 0; i < temp.Length; i++)
            //{
            //    COMMON_FUNCTION.SendMSGForAdmin(temp[0].ToString());
            //}
            //COMMON_FUNCTION.SendMSGForAdmin("9694418180");  //Umesh Sir
            //COMMON_FUNCTION.SendMSGForAdmin("9460934860");  //Rajeev
            //COMMON_FUNCTION.SendMSGForAdmin("9414035286"); // K N Sharma JI
            //COMMON_FUNCTION.SendMSGForAdmin("9782699967"); //Satish Ji
            //COMMON_FUNCTION.SendMSGForAdmin("9694418180");
            // COMMON_FUNCTION.SendMSGForAdmin("9461569464");
            //COMMON_FUNCTION.SendMSGForAdmin("9414038191"); //ns
            ////COMMON_FUNCTION.SendMSGForAdmin("9460934860"); //b
            // COMMON_FUNCTION.SendMSGForAdmin("9461569464"); //rk
            //COMMON_FUNCTION.SendMSGForAdmin("9782699967"); //rk
            //COMMON_FUNCTION.SendMSGForAdmin("8058828693"); //jk
            ////COMMON_FUNCTION.SendMSGForAdmin("8290019144"); //aj
            //COMMON_FUNCTION.JamaBandi("", "", "", "");


            //Session.Abandon();


            SSO.CreateSSOSession();
            try
            {
                string userdetails = "";
                string userToken = "";
                string ISRAJNIVESH = "0";


                System.Web.HttpContext.Current.Session[SessionIdentity.SWCSData] = null;

                SWSEntity objSWC = new SWSEntity();
                try
                {
                    System.Web.HttpContext.Current.Session[SessionIdentity.ISRAJNIVESH] = ISRAJNIVESH;
                    if (Request.QueryString["SWSID"] != null)
                    {
                        ISRAJNIVESH = "1";
                        System.Web.HttpContext.Current.Session[SessionIdentity.ISRAJNIVESH] = ISRAJNIVESH;
                        objSWC.SWSID = Request.QueryString["SWSID"];
                        objSWC.ActID = Request.QueryString["ActID"];
                        objSWC.IsNew = Request.QueryString["IsNew"];
                        if (Request.QueryString["ActivityID"] != null)
                            objSWC.ActivityID = Request.QueryString["ActivityID"];
                        if (Request.QueryString["RegNo"] != null)
                            objSWC.RegNo = Request.QueryString["RegNo"];
                        if (Request.QueryString["IsView"] != null)
                            objSWC.IsView = Request.QueryString["IsView"];
                        if (Request.Form["BRN"] != null)
                            objSWC.BRN = Request.Form["BRN"];
                        objSWC.Userdetails = Request.Form["userdetails"] != null ? Request.Form["Userdetails"] : "";
                        objSWC.Establishmentname = Request.Form["Establishmentname"];

                        userToken = Request.Form["userdetails"] != null ? Request.Form["Userdetails"] : "";

                        SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
                        userdetails = ssoclient.GetTokenDetail(userToken);

                        if (Request.Url.Host.Contains("localhost"))
                        {
                            objSWC.BRN = "8005710017000111";
                           // userdetails = $"sAMAccountName=PRADEEPKATARIYA7;Role=CITIZEN|";
                            //userdetails = "sAMAccountName=VINAYYADAV329;Role=CITIZEN|";
                        }

                    }
                    else
                    {
                        //if (System.Web.HttpContext.Current.Request.Headers["Referer"]?.ToString() != null && (System.Web.HttpContext.Current.Request.Headers["Referer"]?.ToString()).Contains("10.68.252.116"))
                        //{
                        //    userdetails = GetSSOuserdetails("https://ssotest.rajasthan.gov.in:4443/SSORESTNEW/TokenDetail", userToken);
                        //}
                        //else
                        //userToken = Request.Form["userdetails"] != null ? Request.Form["Userdetails"] : "";
                        //userdetails = GetSSOuserdetails("https://ssotest.rajasthan.gov.in:4443/SSORESTNEW/TokenDetail", userToken);
                        userdetails = SSO.GetSessionValue();
                        //ELECTION
                        //userdetails = "sAMAccountName=UmeshCj.doit;Role=HQ|"; //RAO POONAM
                        //userdetails = "sAMAccountName=RJPA200929007905;Role=RAO|"; //RAO UD
                        // userdetails = "sAMAccountName=RJAJ201101030068; Role =SA|"; // regi Abhishek pareek Special Auditor Ajmer
                        //userdetails = "sAMAccountName=RJRA201231008497; Role =admin|"; //citizen user
                        //userdetails = "sAMAccountName=Muktanand.agrawal; Role =RAO|";
                        if (Request.Url.Host.Contains("localhost") || Request.Url.Host.Contains("10.70.234.208") || Request.Url.Host.Contains("10.68.231.168"))
                        {
                            //   userdetails = "sAMAccountName=Monikac.doit;";
                           //userdetails = "sAMAccountName=VINAYYADAV329;Role=CITIZEN|";
                            //       userdetails = "sAMAccountName=PRIYA.BAJAJ1;Role=SA|";
                            // userdetails = "sAMAccountName=DAMYANTI.DEVI;Role=CITIZEN|";
                            //citizen user//VINAYYADAV329//suroliasameer/Churkheda2337
                            //userdetails = "sAMAccountName=UmeshCj.doit; Role =HQ|";
                            //  userdetails = "sAMAccountName=RJBU200810000722; Role =HQ|";
                            //  userdetails = "sAMAccountName=RJJP201119031502; Role =HQ|";


                            //userdetails = "sAMAccountName=RJAJ201606007079; Role =HQ|"; //inspector
                            //userdetails = "sAMAccountName=RJPG201130027916; Role =HQ|"; //inspector


                            // userdetails = "sAMAccountName=RJBP201107020057; Role =HQ|"; //dr

                            //userdetails = "sAMAccountName=balwant.chaudhary;Role=CITIZEN|"; //citizen user//VINAYYADAV329//suroliasameer/Churkheda2337
                            // userdetails = "sAMAccountName=RJAJ201101030068; Role =HQ|";
                            //userdetails = "sAMAccountName=UmeshCj.doit; Role =HQ|";
                            //  userdetails = "sAMAccountName=RJJP198719000398; Role =HQ|";


                            //  userdetails = "sAMAccountName=RJNA201128017691; Role =HQ|";
                            userdetails = "sAMAccountName= tailorravi68; Role =HQ|";

                        }
                    }

                }
                catch (Exception ex)
                {
                    ILogger objILogger = new FileLogger();
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SSOValidation", FunctionName = "ActionResult ValidateUser()" }, ex);
                }


                Response.Cookies.Add(new System.Web.HttpCookie("ASP.NET_SessionId", "")
                {
                    Expires = DateTime.Now.AddDays(-30),
                    HttpOnly = true
                });

                if (!string.IsNullOrEmpty(userdetails) && userdetails != "User Login Id or Password is incorrect")
                {
                    try
                    {
                        string key = userdetails.Split(';')[0].Split('=')[1];
                        string[] Role = userdetails.Split(';')[1].Split('=')[1].Split('|');
                        if (ISRAJNIVESH == "1")
                        {
                            objSWC.SSO_ID = key;

                            if (System.Web.HttpContext.Current.Session[SessionIdentity.ISRAJNIVESH].ToString() == "1")
                            {
                                SocietyController objSociety = new SocietyController();
                                objSWC.IsNewRequestFromSWCS = objSociety.CheckNewRequestFromSWCS(objSWC.SWSID, (objSWC.IsView == "1" ? objSWC.SSO_ID : ""));
                            }

                            objSWC.SWCSJsonData = JsonConvert.SerializeObject(objSWC);
                            System.Web.HttpContext.Current.Session[SessionIdentity.SWCSData] = objSWC;
                        }

                        Response.Cookies.Add(new System.Web.HttpCookie("SSO_ID", "")
                        {
                            Value = key,
                            HttpOnly = true
                        });

                        Response.Cookies.Add(new System.Web.HttpCookie("SSOROLE", "")
                        {
                            Value = Role[0],
                            HttpOnly = true
                        });

                        Response.Cookies.Add(new System.Web.HttpCookie("SWC", "")
                        {
                            Value = (ISRAJNIVESH == "1" ? "YES" : "NO"),
                            HttpOnly = true
                        });
                        Response.Cookies.Add(new System.Web.HttpCookie("SWC_DATA", "")
                        {
                            Value = (ISRAJNIVESH == "1" ? objSWC.SWCSJsonData : ""),
                            HttpOnly = true
                        });
                        Response.Cookies.Add(new System.Web.HttpCookie("SWC_TOKEN", "")
                        {
                            Value = (ISRAJNIVESH == "1" ? userToken : ""),
                            HttpOnly = true
                        });

                        Response.Cookies.Add(new System.Web.HttpCookie(SessionIdentity.Culture, "")
                        {
                            Value = "en-us",
                            HttpOnly = true
                        });
                        DeleteUnwantedCookies(new string[] { "RAJSSO", "SSOROLE", "SSO_ID", "SWC", "SWC_DATA", "SWC_TOKEN", SessionIdentity.Culture });
                        return RedirectToAction("SSOMapping", "Login");
                    }
                    catch (Exception ex)
                    {
                        ILogger objILogger = new FileLogger();
                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SSOValidation", FunctionName = "ActionResult ValidateUser()" }, ex);
                    }
                }
                else
                {
                    Session.Clear();
                    //Session.Abandon();

                    if (Response.Cookies != null && Response.Cookies.Count > 0)
                        foreach (var item in Response.Cookies.AllKeys)
                            Response.Cookies.Remove(item);

                    SSO.BackToSSO("");
                }
            }
            catch (Exception ex)
            {
                ILogger objILogger = new FileLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SSOValidation", FunctionName = "ActionResult ValidateUser()" }, ex);

            }
            COMMON_FUNCTION.ssoSignout("");
            //return View();
            //return Redirect("https://sso.rajasthan.gov.in/signin");

            return RedirectToAction("GotoSSO", "Login");
        }


        /// <summary>
        /// SSO Login via API
        /// <returns></returns>
        public ActionResult ValidateUser_New()
        {
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetNoServerCaching();
            string userToken = "";
            userToken = Request.Form["userdetails"] != null ? Request.Form["Userdetails"] : "";
            try
            {
                ApiResult<SSO_TokenDetailModel> result = new ApiResult<SSO_TokenDetailModel>();
                SSO_UserProfileDetailModel userdetails = new SSO_UserProfileDetailModel();
                string ISRAJNIVESH = "0";


                System.Web.HttpContext.Current.Session[SessionIdentity.SWCSData] = null;

                SWSEntity objSWC = new SWSEntity();
                try
                {
                    System.Web.HttpContext.Current.Session[SessionIdentity.ISRAJNIVESH] = ISRAJNIVESH;
                    if (Request.QueryString["SWSID"] != null)
                    {
                        ISRAJNIVESH = "1";
                        System.Web.HttpContext.Current.Session[SessionIdentity.ISRAJNIVESH] = ISRAJNIVESH;
                        objSWC.SWSID = Request.QueryString["SWSID"];
                        objSWC.ActID = Request.QueryString["ActID"];
                        objSWC.IsNew = Request.QueryString["IsNew"];
                        if (Request.QueryString["ActivityID"] != null)
                            objSWC.ActivityID = Request.QueryString["ActivityID"];
                        if (Request.QueryString["RegNo"] != null)
                            objSWC.RegNo = Request.QueryString["RegNo"];
                        if (Request.QueryString["IsView"] != null)
                            objSWC.IsView = Request.QueryString["IsView"];
                        if (Request.Form["BRN"] != null)
                            objSWC.BRN = Request.Form["BRN"];
                        objSWC.Userdetails = Request.Form["userdetails"] != null ? Request.Form["Userdetails"] : "";
                        objSWC.Establishmentname = Request.Form["Establishmentname"];
                        result = SsoServiceHelper.GetSSOTokenDetails(userToken);
                        userdetails = SsoServiceHelper.GetSSOUserProfileDetail(result.Data.sAMAccountName, objSWC.Userdetails);
                    }
                    else
                    {
                        if (Request.Url.Host.Contains("localhost"))
                        {
                            //ADMIN
                            userdetails.SSOID = "tailorravi68";
                            userdetails.userType = "HQ";

                            //CITIZEN
                            //userdetails.SSOID = "VINAYYADAV329";
                            //userdetails.userType = "CITIZEN";
                        }
                        else
                        {
                            result = SsoServiceHelper.GetSSOTokenDetails(userToken);
                            userdetails = SsoServiceHelper.GetSSOUserProfileDetail(result.Data.sAMAccountName, userToken);
                        }
                    }

                }
                catch (Exception ex)
                {
                    ILogger objILogger = new FileLogger();
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.SSOValidationController", FunctionName = "ActionResult ValidateUser()" }, ex);
                }


                Response.Cookies.Add(new System.Web.HttpCookie("ASP.NET_SessionId", "")
                {
                    Expires = DateTime.Now.AddDays(-30),
                    HttpOnly = true
                });

                if (userdetails.SSOID != null)
                {
                    try
                    {
                        if (ISRAJNIVESH == "1")
                        {
                            objSWC.SSO_ID = userdetails.SSOID;

                            if (System.Web.HttpContext.Current.Session[SessionIdentity.ISRAJNIVESH].ToString() == "1")
                            {
                                SocietyController objSociety = new SocietyController();
                                objSWC.IsNewRequestFromSWCS = objSociety.CheckNewRequestFromSWCS(objSWC.SWSID, (objSWC.IsView == "1" ? objSWC.SSO_ID : ""));
                            }

                            objSWC.SWCSJsonData = JsonConvert.SerializeObject(objSWC);
                            System.Web.HttpContext.Current.Session[SessionIdentity.SWCSData] = objSWC;
                        }
                        Response.Cookies.Add(new System.Web.HttpCookie("SSOTOKEN", "")
                        {
                            Value = userToken,
                            HttpOnly = true
                        });
                        Response.Cookies.Add(new System.Web.HttpCookie("RAJSSO", "")
                        {
                            Value = userdetails.SSOID,
                            HttpOnly = true
                        });
                        Response.Cookies.Add(new System.Web.HttpCookie("SSO_ID", "")
                        {
                            Value = userdetails.SSOID,
                            HttpOnly = true
                        });

                        Response.Cookies.Add(new System.Web.HttpCookie("SSOROLE", "")
                        {
                            Value = (result != null && result.Data?.Roles.Count > 0) ? result.Data?.Roles[0] : userdetails.userType,
                            HttpOnly = true
                        });

                        Response.Cookies.Add(new System.Web.HttpCookie("SWC", "")
                        {
                            Value = (ISRAJNIVESH == "1" ? "YES" : "NO"),
                            HttpOnly = true
                        });
                        Response.Cookies.Add(new System.Web.HttpCookie("SWC_DATA", "")
                        {
                            Value = (ISRAJNIVESH == "1" ? objSWC.SWCSJsonData : ""),
                            HttpOnly = true
                        });
                        Response.Cookies.Add(new System.Web.HttpCookie("SWC_TOKEN", "")
                        {
                            Value = (ISRAJNIVESH == "1" ? userToken : ""),
                            HttpOnly = true
                        });

                        Response.Cookies.Add(new System.Web.HttpCookie(SessionIdentity.Culture, "")
                        {
                            Value = "en-us",
                            HttpOnly = true
                        });
                        DeleteUnwantedCookies(new string[] { "SSOTOKEN", "RAJSSO", "SSOROLE", "SSO_ID", "SWC", "SWC_DATA", "SWC_TOKEN", SessionIdentity.Culture });
                        return RedirectToAction("SSOMapping", "Login");
                    }
                    catch (Exception ex)
                    {
                        ILogger objILogger = new FileLogger();
                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.StagingSSOValidationController", FunctionName = "ActionResult ValidateUser()" }, ex);
                    }
                }
                else
                {
                    Session.Clear();

                    if (Response.Cookies != null && Response.Cookies.Count > 0)
                        foreach (var item in Response.Cookies.AllKeys)
                            Response.Cookies.Remove(item);

                    SsoServiceHelper.BackToSSO(userToken);
                }
            }
            catch (Exception ex)
            {
                ILogger objILogger = new FileLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.StagingSSOValidationController", FunctionName = "ActionResult ValidateUser()" }, ex);

            }
            COMMON_FUNCTION.ssoSignout("");

            return RedirectToAction("GotoSSO", "Login");
        }
        public string CertificateGenerateAmendment(string otpResult)
        {
            try
            {
                ApplicationFlowModel AppFlowModel = new ApplicationFlowModel();

                //DataTable dtAmendmentChanges = AppFlowModel.GetAmendmentChanges(SocietyIDEnc, AmendmentId);
                string fileNameCerti = "";
                string fileNameMemList = "";
                string lbl_SocietyDirectory = "";
                string lbl_AO_ADD_DistrictEn = "";

                string lbl_InstitutionName = "";
                string lbl_AO_ADD_DistrictHi = "";
                string lbl_Registraionno = "";
                string lbl_ActId = "";
                string lbl_OldName = "";
                string BylawsStr = "";
                string lbl_RegistrationDate = "";

                lbl_Registraionno = "COOP/2021/JAIPUR/201221";
                try
                {
                    if (lbl_Registraionno.Replace(" ", "").Replace("/", "") != lbl_Registraionno.Replace(" ", "").Replace("/", "").Replace("-", ""))
                    {
                        //System.IO.File.Move(, Server.MapPath("/Content/SocietyProfiling/" + lbl_Registraionno.Replace(" ", "").Replace("/", "").Replace("-", "") + "/4B15AD7AF60C4130AEAA57BCA1ACFA75.pdf"));
                        string SourcePath = Server.MapPath("/Content/SocietyProfiling/" + lbl_Registraionno.Replace(" ", "").Replace("/", ""));
                        string DestinationPath = Server.MapPath("/Content/SocietyProfiling/" + lbl_Registraionno.Replace(" ", "").Replace("/", "").Replace("-", ""));

                        if (!System.IO.Directory.Exists(Server.MapPath("/Content/SocietyProfiling/" + lbl_Registraionno.Replace(" ", "").Replace("/", "").Replace("-", ""))))
                        {
                            System.IO.Directory.CreateDirectory(Server.MapPath("/Content/SocietyProfiling/" + lbl_Registraionno.Replace(" ", "").Replace("/", "").Replace("-", "")));
                        }

                        foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
                        {
                            System.IO.File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);
                        }

                    }
                }
                catch (Exception) { }



                lbl_SocietyDirectory = "COOP2021JAIPUR201221";// dtAmendmentChanges.Rows[0]["SocietyDirectory"].ToString();
                lbl_AO_ADD_DistrictEn = "Jaipur";// dtAmendmentChanges.Rows[0]["DISTRICT_MANGAL"].ToString();
                lbl_InstitutionName = "SAARTHI FOUNDATION CHIRAWA";// dtAmendmentChanges.Rows[0]["Name"].ToString();
                lbl_AO_ADD_DistrictHi = "Jaipur";// dtAmendmentChanges.Rows[0]["DISTRICT_MANGAL"].ToString();
                //lbl_Registraionno = lbl_Registraionno;// dtAmendmentChanges.Rows[0]["RegistrationNo"].ToString();
                lbl_ActId = "1";// dtAmendmentChanges.Rows[0]["ActId"].ToString();
                lbl_OldName = "";// dtAmendmentChanges.Rows[0]["SocietyName"].ToString();
                lbl_RegistrationDate = "20/01/2021";// dtAmendmentChanges.Rows[0]["RegistrationDateFormated"].ToString();


                //BylawsStr = dtAmendmentChanges.Rows[0]["ByeLawsFileURLForEsign"].ToString();

                //DataTable dt = AppFlowModel.GetScietyAdministrativeDetail(SocietyIDEnc, 1, " ");
                // if (dt.Rows.Count > 0)
                //{
                //DataTable tblFiltered = dt.AsEnumerable().Where(row => row.Field<string>("POSTNAME").ToUpper() == "SECRETARY").CopyToDataTable();
                //lbl_InstitutionName = dt.Rows[0]["SocietyName"].ToString();
                //string lbl_Name = dtAmendmentChanges.Rows[0]["Name"].ToString();
                //lbl_AO_ADD_DistrictEn = dtAmendmentChanges.Rows[0]["DISTRICT_ENG"].ToString();
                //lbl_AO_ADD_DistrictHi = dtAmendmentChanges.Rows[0]["DISTRICT_MANGAL"].ToString();
                //lbl_Registraionno = dtAmendmentChanges.Rows[0]["RegistrationNo"].ToString();
                //lbl_ActId = dtAmendmentChanges.Rows[0]["ActId"].ToString();
                //string lbl_SocietyDirectory = dt.Rows[0]["SocietyDirectory"].ToString();
                //string lbl_OldRegistrationNo = dtAmendmentChanges.Rows[0]["OldRegistrationNo"].ToString();
                //string lbl_ByLows = dtAmendmentChanges.Rows[0]["OldRegistrationNo"].ToString();
                //string Objects = dtAmendmentChanges.Rows[0]["OldRegistrationNo"].ToString();
                fileNameCerti = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + ".pdf";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                string actNameEn = lbl_ActId == "1" ? ("The Rajasthan Societies Registration Act, 1958 (Rajasthan Act No. 28, 1958)").ToUpper() : lbl_ActId == "3" ? ("The Rajasthan Co-operative Societies Act, 2001").ToUpper() : lbl_ActId == "7" ? ("The Rajasthan Societies Registration Act, 2005").ToUpper() : "";
                string actNameHi = lbl_ActId == "1" ? ("राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 1958 (राजस्थान अधिनियम संख्या 28, 1958) ").ToUpper() : lbl_ActId == "3" ? ("राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 2001").ToUpper() : lbl_ActId == "7" ? ("राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 2005").ToUpper() : "";
                string devFontSize = "17px";
                string fontSize = "font-size: 13px;";

                sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:50px'>");
                sb.Append("<table style='width: 100%;border:1px solid;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px'>");
                sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                sb.Append("<img style='width:90px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:32px;'>" + UnicodeToKrutidev.GetKrutidev("सहकारिता विभाग") + "</label></b>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");

                //if (lbl_ActId == "1" || lbl_ActId == "7")
                // {
                sb.Append("<b><label style='font-size:22px;'> " + UnicodeToKrutidev.GetKrutidev("रजिस्ट्रीकरण प्रमाण - पत्र") + " </label></b>");
                //}
                //else
                //{
                //    sb.Append("<b><label style='font-size:22px;'> " + UnicodeToKrutidev.GetKrutidev("पंजीयन प्रमाण - पत्र ") + " </label></b>");
                //}
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
                sb.Append("<table style='width:100%'>");
                sb.Append("<tr>");
                sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'><b> पं. संख्या " + lbl_Registraionno + "</b></td>");
                sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> दिनांक - " + DateTime.Now.ToString("dd-MM-yyyy") + " </td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");

                //sb.Append("<td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'> यह प्रमाणित किया जाता है कि <b> " + lbl_InstitutionName.ToUpper() + " </b> जिला <b> " + lbl_AO_ADD_DistrictEn.ToUpper() + " </b>  का रजिस्ट्रेशन <b>' " + actNameHi + " '</b>  के अन्तर्गत आज किया गया है। यह प्रमाण पत्र मेरे डिजिटल हस्ताक्षरों से आज जारी किया गया है। </td>");

                //if (lbl_ActId == "1" || lbl_ActId == "7")
                //{
                sb.Append("<tr><td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'><p style='text-align: justify;text-justify;'>  यह प्रमाणित किया जाता है कि <b> " + lbl_OldName.ToUpper() + " </b> जिला <b> " + lbl_AO_ADD_DistrictHi.ToUpper() + " </b>  का रजिस्ट्रेशन <b>' " + actNameHi + " '</b>  के अन्तर्गत दिनांक <b>" + lbl_RegistrationDate + "</b> को किया गया था। </p>");
                sb.Append("<p style='text-align: justify;text-justify;'> अब इस संस्थान का नाम परिवर्तित कर <b> " + lbl_InstitutionName.ToUpper() + " </b> जिला <b> " + lbl_AO_ADD_DistrictHi.ToUpper() + " </b>  कर दिया गया है। यह परिवर्तन राजस्थान संस्था रजिस्ट्रीकरण अधिनियम ," + (lbl_ActId == "1" ? "1958" : lbl_ActId == "3" ? "2001" : "2005") + " की धारा -12 के प्रावधानान्तर्गत किया गया है। </p>");
                sb.Append("<p style='text-align: justify;text-justify;'> यह प्रमाण पत्र मेरे हस्ताक्षर एवं कार्यालय की मुहर से दिनांक <b>" + DateTime.Now.ToString("dd-MM-yyyy") + "</b> को " + lbl_AO_ADD_DistrictHi + " में जारी किया गया। </p></td></tr>");
                //}
                //else if (lbl_ActId == "3")
                //{

                //    sb.Append("<tr><td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'><p style='text-align: justify;text-justify;'> प्रमाणित किया जाता है कि  श्री " + lbl_Name.ToUpper() + "  द्वारा " + lbl_InstitutionName.ToUpper() + "  के नाम से एक सहकारी सोसाइटी  के रजिस्ट्रेशन हेतु प्रस्तुत आवेदन को स्वीकार करते हुए उक्त  सोसाइटी एवं  उसके द्वारा अंगीकृत उपविधियों का राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 2001  की धारा 6 के अंतर्गत  आज  दिनांक " + DateTime.Now.ToString("dd-MM-yyyy") + "  को क्रमांक  " + lbl_Registraionno + "  पर  रजिस्ट्रीकरण किया गया । यह  प्रमाण पत्र  मेरी  मुद्रा  एवं हस्ताक्षर सहित आज दिनांक " + DateTime.Now.ToString("dd-MM-yyyy") + "  को प्रसारित  किया  जाता  है । </p></td></tr>");
                //    sb.Append("<tr><td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'><p style='text-align: justify;text-justify;'> " + ("Certified that accepting the application made by Mr " + lbl_Name.ToUpper() + " for the registration of a co-operative society under the name " + lbl_InstitutionName.ToUpper() + ", the said society alongwith the bye - laws adopted by it, is registered at number " + lbl_Registraionno + " on dated " + DateTime.Now.ToString("dd-MM-yyyy") + " under section 6 of The Rajasthan Co-operative Societies Act, 2001.This registration certificate is issued under my seal and signatures on date " + DateTime.Now.ToString("dd-MM-yyyy")).ToUpper() + " </p></td></tr>");
                //    sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'><br/><br/><br/><br/><br/><br/><br/></td></tr>");
                //}

                //sb.Append("<tr><td style='padding: 15px; line-height: 30px; font-size: 12px; font-weight: normal; height: 100px;'>IT IS CERTIFIED THAT <b>" + lbl_InstitutionName.ToUpper() + "</b> AT  DISTRICT  <b>" + lbl_AO_ADD_DistrictEn.ToUpper() + "</b> IS REGISTERED UNDER  <b>'" + actNameEn + "'</b>. THIS CERTIFICATE IS ISSUED TODAY UNDER MY DIGITAL SIGNATURE.</td></tr>");
                sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: center;'>");
                string QRCoder = COMMON_FUNCTION.GenerateQRCode(lbl_Registraionno.Replace("-", "").Replace("/", "") + "/" + fileNameCerti);
                sb.Append("<img style='width:180px:height:180px;' src='" + QRCoder + "'/>");
                sb.Append("</td></tr>");
                sb.Append("</table>");
                sb.Append("</div>");
                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));
                if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraionno.Replace("-", "").Replace("/", "") + "/")))
                {
                    System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraionno.Replace("-", "").Replace("/", "") + "/"));
                }
                string filepath = System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraionno.Replace("-", "").Replace("/", "") + "/") + fileNameCerti;
                if (!COMMON_FUNCTION.GeneratePDF(sb1, filepath, ""))
                {
                    throw new Exception("Error in Generate Certificate");
                };

                var result = COMMON_FUNCTION.EsinghPDF(otpResult, Server.MapPath("~/Content/SocietyProfiling/" + lbl_Registraionno.Replace("-", "").Replace("/", "") + "/") + fileNameCerti, lbl_AO_ADD_DistrictEn, "REGISTRAR");




                return "";



            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "CertificateGenerate catch " }, ex);

                return "Fail";
            }
        }

        public string GetAadhar(string ReferenceKey)
        {
            string aadharNo = "";
            try
            {
                //var url = "https://rajsahakarapp.rajasthan.gov.in/api/Services/Mobile_GetTotalQuantityDetails?DistrictId=0";
                //string xml = "<AuthRequest  UUID=\"" + ReferenceKey + "\" flagType=\"A\" subaua=\"" + auacode + "\" ver=\"2.5\"></ AuthRequest >";
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //byte[] bytes;
                //bytes = System.Text.Encoding.ASCII.GetBytes(xml);
                //request.ContentType = "Application/xml";
                //request.ContentLength = bytes.Length;
                //request.Method = "POST";
                //request.Headers["appname"] = "CO-Operative";
                //Stream requestStream = request.GetRequestStream();
                //requestStream.Write(bytes, 0, bytes.Length);
                //requestStream.Close();
                //HttpWebResponse response;
                //response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {

            }

            return aadharNo;
        }

        //public string YourMethod()
        //{
        //    //Create a list of your parameters
        //    var postParams = new List<KeyValuePair<string, object>>(){
        //        new KeyValuePair<string, object>("data", "First Value") ,
        //        new KeyValuePair<string, object>("SecondParameter", "Second Value")
        //    };

        //    //Join KVPs into a x-www-formurlencoded string
        //    var formString = string.Join("&", postParams.Select(x => string.Format("{0}={1}", x.Key, x.Value)));
        //    //output: FirstParamter=FirstValue&SecondParamter=SecondValue

        //    //Encode form string to bytes
        //    var bytes = Encoding.UTF8.GetBytes(formString);

        //    //Create a POST webrequest
        //    var request = WebRequest.CreateHttp("https://www.example.com");
        //    request.Method = "POST";
        //    request.ContentType = "application/x-www-form-urlencoded";

        //    //Open request stream
        //    using (var reqStream = await request.GetRequestStreamAsync())
        //    {
        //        //Write bytes to the request stream
        //        await reqStream.WriteAsync(bytes, 0, bytes.Length);
        //    }

        //    //Try to get response
        //    WebResponse response = null;
        //    try
        //    {
        //        response = await request.GetResponseAsync();
        //    }
        //    catch (WebException e)
        //    {
        //        //Something went wrong with our request. Return the exception message.
        //        return e.Message;
        //    }

        //    //Get response stream
        //    using (var respStream = response.GetResponseStream())
        //    {
        //        //Create a streamreader to read the website's response, then return it as a string
        //        using (var reader = new StreamReader(respStream))
        //        {
        //            return await reader.ReadToEndAsync();
        //        }
        //    }
        //}
        public void Margepdf()
        {
            string filepath = "C:\\Users\\balwant.chaudhary\\Documents\\TDS2021-22";// Server.MapPath("~/Content/Report");
            string[] files = new string[] { filepath + "/SchoolFees1.pdf", filepath + "/SchoolFees2.pdf" };
            COMMON_FUNCTION.MergePDFs(filepath + "/SchoolFees.pdf", files);
        }
        public string GetData1()
        {


            String message = "";
            using (WebClient Client = new WebClient())
            {
                try
                {
                    SocietyModel obj = new SocietyModel();

                    var data = COMMON_FUNCTION.DataTableToJSONWithJSONNet(obj.GetEmitraServiseData());
                    //data = "{data:" + data + "}";
                    string jsonString = string.Empty;
                    jsonString = JsonConvert.SerializeObject(data);

                    var postParams = new List<KeyValuePair<string, object>>(){
                    new KeyValuePair<string, object>("data", "{data:"+data+"}")
                     };

                    //Join KVPs into a x-www-formurlencoded string
                    var formString = string.Join("&", postParams.Select(x => string.Format("{0}={1}", x.Key, x.Value)));
                    //output: FirstParamter=FirstValue&SecondParamter=SecondValue

                    //Encode form string to bytes
                    var bytes = Encoding.UTF8.GetBytes(formString);

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;// | SecurityProtocolType.Tls11;
                    //var http = (HttpWebRequest)WebRequest.Create(new Uri("https://api.sewadwaar.rajasthan.gov.in/app/live/emitra/Prod/webServices/Repository/pushThirdPartyPendencyDataApi/service?client_id=02a79073-c2db-4c84-b810-885fdaa8c54a"));
                    var http = (HttpWebRequest)WebRequest.Create(new Uri("https://apitest.sewadwaar.rajasthan.gov.in/app/live/emitra/Prod/webServicesRepositoryUat/pushThirdPartyPendencyDataApi/service?client_id=76b3522f-117a-4797-b7fe-342e40ee9f29"));

                    http.Method = "POST";
                    http.ContentType = "application/x-www-form-urlencoded";

                    // string parsedContent = <<PUT HERE YOUR JSON PARSED CONTENT>>;
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    //Byte[] bytes = encoding.GetBytes(jsonString);

                    Stream newStream = http.GetRequestStream();
                    newStream.Write(bytes, 0, bytes.Length);
                    newStream.Close();

                    var response = http.GetResponse();

                    var stream = response.GetResponseStream();
                    var sr = new StreamReader(stream);
                    var content = sr.ReadToEnd();
                    //message = JsonConvert.DeserializeObject<ApplicationTransactionMessage>(content);
                    return content;
                }
                catch (Exception ex)
                {
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServices", FunctionName = "Mobile_GetSTLoanDetailByFinancialYear" }, ex);
                    //  ErrorLog.Write(ex);
                }
            }
            return message;

        }

        public DataSet GetDataFromProd(string registrationNo)
        {

            DataSet dr = new DataSet();
            //registrationNo = "121/JODHPUR/1992-93";

            // Create a request for the URL.
            WebRequest request = WebRequest.Create("https://api.sewadwaar.rajasthan.gov.in/app/live/emitra/Prod/webServices/Repository/pushThirdPartyPendencyDataApi/service?client_id=02a79073-c2db-4c84-b810-885fdaa8c54a");
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;

            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server.
            // The using block ensures the stream is automatically closed.
            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();

                var data = JsonConvert.DeserializeObject<Object>(responseFromServer);

                // Display the content.
                Console.WriteLine(responseFromServer);
            }

            // Close the response.
            response.Close();
            return dr;
        }


        public static DataTable jsonToDataTable(string jsonString)
        {
            var jsonLinq = JObject.Parse(jsonString);

            // Find the first array using Linq  
            var srcArray = jsonLinq.Descendants().Where(d => d is JArray).First();
            var trgArray = new JArray();
            foreach (JObject row in srcArray.Children<JObject>())
            {
                var cleanRow = new JObject();
                foreach (JProperty column in row.Properties())
                {
                    // Only include JValue types  
                    if (column.Value is JValue)
                    {
                        cleanRow.Add(column.Name, column.Value);
                    }
                }
                trgArray.Add(cleanRow);
            }

            return JsonConvert.DeserializeObject<DataTable>(trgArray.ToString());
        }

        public static DataTable JsonStringToDataTable(string jsonString)
        {
            DataTable dt = new DataTable();
            string[] jsonStringArray = Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");
            List<string> ColumnsName = new List<string>();
            foreach (string jSA in jsonStringArray)
            {
                string[] jsonStringData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                foreach (string ColumnsNameData in jsonStringData)
                {
                    try
                    {
                        int idx = ColumnsNameData.IndexOf(":");
                        string ColumnsNameString = ColumnsNameData.Substring(0, idx - 1).Replace("\"", "");
                        if (!ColumnsName.Contains(ColumnsNameString))
                        {
                            ColumnsName.Add(ColumnsNameString);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw new Exception(string.Format("Error Parsing Column Name : {0}", ColumnsNameData));
                    }
                }
                break;
            }
            foreach (string AddColumnName in ColumnsName)
            {
                dt.Columns.Add(AddColumnName);
            }
            foreach (string jSA in jsonStringArray)
            {
                string[] RowData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                DataRow nr = dt.NewRow();
                foreach (string rowData in RowData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string RowColumns = rowData.Substring(0, idx - 1).Replace("\"", "");
                        string RowDataString = rowData.Substring(idx + 1).Replace("\"", "");
                        nr[RowColumns] = RowDataString;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        continue;
                    }
                }
                dt.Rows.Add(nr);
            }
            return dt;
        }

        [HttpPost]
        public void GenerateAuditCompletePDF()
        {
            //= new DataPagingModel();
            string devFontSize = "17px";
            StringBuilder sb = new StringBuilder();
            sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:50px'>");
            sb.Append("<table  cellpadding='5' cellspacing='0' style='width:100%;'>");
            sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");
            sb.Append("<label style='font-size:15px;'> ACKNOWLEDGEMENT OF SOCIETY AUDIT 2021-21 </label>");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");
            sb.Append("<table  cellpadding='5' cellspacing='0' border='1' style='width:100%;'>");
            //sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
            //sb.Append("<img style='width:90px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
            //sb.Append("</td></tr>");           


            sb.Append("<tr>");
            sb.Append("<th style='text-align:left;font-size:10px;'> Auditor   </th>");
            sb.Append("<td style='text-align:left;font-size:10px;'> UDAI ASHOK AND ASSOCIATE </td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:left;font-size:10px;'> EmpanelledNo  </th>");
            sb.Append("<td style='text-align:left;font-size:10px;'> CAF/2021/JAIPUR-502 </td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:left;font-size:10px;'> Society Name  </th>");
            sb.Append("<td style='text-align:left;font-size:10px;'> The Krishna Co-operative Housing Society Ltd. </td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:left;font-size:10px;'> Society RegistrationNo  </th>");
            sb.Append("<td style='text-align:left;font-size:10px;'> 1599/L </td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:left;font-size:10px;'> Audit Start Date  </th>");
            sb.Append("<td style='text-align:left;font-size:10px;'> 01/05/2021 </td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<th style='text-align:left;font-size:10px;'> Audit End Date  </th>");
            sb.Append("<td style='text-align:left;font-size:10px;'> 01/05/2021 </td>");
            sb.Append("</tr>");
            sb.Append("</table>");
            sb.Append("</td>");
            sb.Append("</tr>");
            //sb.Append("<th style='text-align:left;font-size:10px;'> पार्टनर्स की संख्या </th>");
            //sb.Append("<th style='text-align:left;font-size:10px;'> ब्रांच का पता  </th>");                   
            //sb.Append("</tr>");

            //(This is computer generated Acknowledment Receipt)                    


            sb.Append("</table>");
            sb.Append("</div>");

            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));

            string fileNameCerti = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + ".pdf";
            string filepath = System.Web.HttpContext.Current.Server.MapPath("~/Content/") + fileNameCerti;


            if (COMMON_FUNCTION.GeneratePDF(sb1, filepath, ""))
            {
                //return "Success";
            }
            else
            {
                //return "Fail";
            }

        }


        public string GetMembersCert(string QRCoder1)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("AadharNo", typeof(string));
            dt.Columns.Add("Photo", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Gender", typeof(string));
            dt.Columns.Add("POSTNAME", typeof(string));
            dt.Columns.Add("MemberDOB", typeof(string));

            DataRow dr = dt.NewRow();
            dr["AadharNo"] = "	tkfp1fpQmxDWxbUWGpnTjw==";
            dr["Photo"] = "~/Content/SocietyProfiling/COOP2021JAIPUR201670/member_155f9d88ae4944b6a62a9b8e6a96d0d2.jpg";
            dr["Name"] = "Mukesh Kumar Meena";
            dr["Gender"] = "M";
            dr["POSTNAME"] = "CHAIRMAN/PRESIDENT";
            dr["MemberDOB"] = "21/11/1978";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["AadharNo"] = "	T1+Bhe8Ju9s4QjBaM66sAg==";
            dr["Photo"] = "~/Content/SocietyProfiling/COOP2021JAIPUR201670/member_9fa6267f011b4142825706663f33ad95.jpg";
            dr["Name"] = "	Balwant Chaudhary";
            dr["Gender"] = "M";
            dr["POSTNAME"] = "TREASURER";
            dr["MemberDOB"] = "20/09/1984";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["AadharNo"] = "ww86e317/Zw6p8n7TgkM+Q==";
            dr["Photo"] = "~/Content/SocietyProfiling/COOP2021JAIPUR201670/0140FD25314B4D968FCA117CAAF6F418.jpeg";
            dr["Name"] = "	Ganga Sharma";
            dr["Gender"] = "F";
            dr["POSTNAME"] = "VICE-PRESIDENTT";
            dr["MemberDOB"] = "24/05/1989";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["AadharNo"] = "de/rfZdWkDsbCYkkrAGGfA==";
            dr["Photo"] = "~/Content/SocietyProfiling/COOP2021JAIPUR201670/member_e5ffdbf1ae9a465b9d434304155ec0cb.jpg";
            dr["Name"] = "Shweta Chakravorty	";
            dr["Gender"] = "F";
            dr["POSTNAME"] = "GENERAL SECRETARY";
            dr["MemberDOB"] = "04/10/1988";
            dt.Rows.Add(dr);


            dr = dt.NewRow();
            dr["AadharNo"] = "	0UYk2HBxGWWxuXmQ01vWkA==";
            dr["Photo"] = "~/Content/SocietyProfiling/COOP2021JAIPUR201670/member_0e65986d83d34723b344f2b5fbc6571a.jpg	";
            dr["Name"] = "	Anjali Bansal";
            dr["Gender"] = "F";
            dr["POSTNAME"] = "Member";
            dr["MemberDOB"] = "30/11/1977";
            dt.Rows.Add(dr);



            string actId = "1";
            string lbl_Registraionno = "COOP/2021/JAIPUR/201670";
            string lbl_InstitutionName = "GANESH AANGAN SOCIETY";
            string lbl_AO_ADD_DistrictEn = "JAIPUR";
            string actNameEn = actId == "1" ? ("The Rajasthan Societies Registration Act, 1958 (Rajasthan Act No. 28, 1958)").ToUpper() : actId == "3" ? ("The Rajasthan Co-operative Societies Act, 2001").ToUpper() : actId == "7" ? ("The Rajasthan Societies Registration Act, 2005").ToUpper() : "";
            string actNameHi = actId == "1" ? ("राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 1958 (राजस्थान एक्ट नंबर 28, 1958) ").ToUpper() : actId == "3" ? ("राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 2001").ToUpper() : actId == "7" ? ("राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 2005").ToUpper() : "";
            string devFontSize = "17px";
            string fontSize = "font-size: 13px;";
            StringBuilder sb = new StringBuilder();
            sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:20px'>");
            sb.Append("<table style='width: 100%;height: 100%; margin-left:50px;margin-top:20px;margin-right:50px'>");
            sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
            sb.Append("<img style='width:90px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
            sb.Append("</td></tr>");
            sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
            sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:32px;'>" + UnicodeToKrutidev.GetKrutidev("सहकारिता विभाग") + "</label><label  style='font-size:25px;'>/ COOPERATIVE DEPARTMENT </label></b>");
            sb.Append("</td></tr>");
            sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");
            sb.Append("<b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev("कार्यकारिणी प्रमाण पत्र") + "</label><label  style='font-size:15px;'>/ EXECUTIVE COMMITTEE CERTIFICATE</label></b>");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
            sb.Append("<table style='width:100%'>");
            sb.Append("<tr>");
            sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'><b> पं. संख्या / REG. NO.- " + lbl_Registraionno + "</b></td>");
            sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> दिनांक / DATE- " + DateTime.Now.ToString("dd-MM-yyyy") + " </td>");
            sb.Append("</tr>");
            sb.Append("</table>");
            sb.Append("</td>");
            sb.Append("</tr>");


            sb.Append("<tr>");
            sb.Append("<td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'> यह प्रमाणित किया जाता है कि <b> " + lbl_InstitutionName.ToUpper() + " </b> जिला <b> " + lbl_AO_ADD_DistrictEn.ToUpper() + " </b>  का रजिस्ट्रेशन <b>' " + actNameHi + " '</b>  के अन्तर्गत पंजीकृत हैं, में पंजीकरण के समय निम्नलिखित कार्यकारिणी हैं। यह प्रमाण पत्र मेरे डिजिटल हस्ताक्षरों से आज जारी किया गया है।");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("<tr><td style='padding: 15px; line-height: 30px; font-size: 12px; font-weight: normal; height: 100px;'>IT IS CERTIFIED THAT <b>" + lbl_InstitutionName.ToUpper() + "</b> AT  DISTRICT  <b>" + lbl_AO_ADD_DistrictEn.ToUpper() + "</b> IS REGISTERED UNDER  <b>'" + actNameEn + "'</b> HAS THE FOLLOWING EXECUTIVE COMMITTEE AT THE TIME OF REGISTRATION. THIS CERTIFICATE IS ISSUED TODAY UNDER MY DIGITAL SIGNATURE.</td></tr>");

            sb.Append("<tr>");
            sb.Append("<td valign='top' style = 'padding: 15px; line-height: 30px; font-size: 12px; font-weight: normal;'>");
            sb.Append("<table cellspacing='0' cellpadding='5'  style='width: 100%;height: 100%;font-size: 12px; font-weight: normal;border-collapse: collapse; text-align:center;'>");
            sb.Append("<tr style=''>");
            sb.Append("<th style='padding:5px;border: solid 0.2px #ccc; padding:10px;'>Sl. No.</th>");
            sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;'>Photo</th>");
            sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;'>Name</th>");
            sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;'>Gender</th>");
            sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;'>Post</th>");
            sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;'>Aadhaar No</th>");
            sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;'>DOB</th>");
            sb.Append("</tr>");

            foreach (DataRow dr1 in dt.Rows) // search whole table
            {

                if (dr1["AadharNo"].ToString().Length > 10)
                {
                    dr1["AadharNo"] = COMMON_FUNCTION.Decrypt(dr1["AadharNo"].ToString());
                    if (dr1["AadharNo"].ToString().Length == 12)
                    {
                        var AadharNo = dr1["AadharNo"].ToString();

                        var firstDigits = AadharNo.Substring(0, 8);
                        var lastDigits = AadharNo.Substring(AadharNo.Length - 4, 4);
                        var requiredMask = new String('X', AadharNo.Length - lastDigits.Length);
                        var maskedString = string.Concat(requiredMask, lastDigits);
                        var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0");
                        dr1["AadharNo"] = maskedString;
                    }
                }

                //objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "inner aadhar loop " + dr["AadharNo"] }, httpException);

            }
            int RowNo = 1;
            foreach (DataRow row in dt.Rows)
            {
                string Photostr = row["Photo"].ToString();
                System.Drawing.Image ImgFile = null;
                if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(Photostr)))
                {
                    ImgFile = System.Drawing.Image.FromFile(System.Web.HttpContext.Current.Server.MapPath(Photostr));
                }
                else
                {
                    ImgFile = System.Drawing.Image.FromFile(System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/Balnk.png"));
                }
                //objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowController", FunctionName = "inner photo loop " + ImgFile }, httpException);
                string fileImg = "data:image/png;base64," + COMMON_FUNCTION.ImageToBase64(ImgFile, ImgFile.RawFormat);




                //



                if (RowNo == 8)
                {

                    sb.Append("<tr style=''>");
                    sb.Append("<th style='padding:5px;border: solid 0.2px #ccc; padding:10px;page-break-before:always;'>Sl. No.</th>");
                    sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;'>Photo</th>");
                    sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;'>Name</th>");
                    sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;'>Gender</th>");
                    sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;'>Post</th>");
                    sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;'>Aadhaar No</th>");
                    sb.Append("<th style='padding:5px;border: solid 0.2px #ccc;text-align:left;'>DOB</th>");
                    sb.Append("</tr>");
                }

                sb.Append("<tr>");
                sb.Append("<td style='padding:2px;border: solid 0.2px #ccc;'>" + RowNo.ToString() + "</td>");
                sb.Append("<td style='padding:2px;border: solid 0.2px #ccc; border-radius:30px;'><img width='45' height='45' src='" + fileImg + "' /></td>");
                sb.Append("<td style='padding:2px;border: solid 0.2px #ccc;text-align:left;'>" + row["Name"].ToString() + "</td>");
                sb.Append("<td style='padding:2px;border: solid 0.2px #ccc;text-align:left;'>" + row["Gender"].ToString() + "</td>");
                sb.Append("<td style='padding:2px;border: solid 0.2px #ccc;text-align:left;'>" + row["POSTNAME"].ToString() + " </td>");
                sb.Append("<td style='padding:2px;border: solid 0.2px #ccc;text-align:left;'>" + row["AadharNo"].ToString() + "</td>");
                sb.Append("<td style='padding:2px;border: solid 0.2px #ccc;text-align:left;'>" + row["MemberDOB"].ToString() + "</td>");
                sb.Append("</tr>");
                RowNo++;
            }

            sb.Append("<tr>");
            sb.Append("<td colspan='7' style='padding:2px;border: solid 0.2px #ccc;'></td>");
            sb.Append("</tr>");

            sb.Append("</table>");
            sb.Append("</td>");
            sb.Append("</tr>");
            //sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'><br/><br/><br/><br/><br/><br/><br/></td></tr>");
            sb.Append("</table>");
            sb.Append("<img style='width:150px:height:100px;' src='" + QRCoder1 + "'/>");
            sb.Append("</div>");


            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));

            string fileNameCerti = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + ".pdf";
            string filepath = System.Web.HttpContext.Current.Server.MapPath("~/Content/") + fileNameCerti;


            if (COMMON_FUNCTION.GeneratePDF(sb1, filepath, ""))
            {
                return "Success";
            }
            else
            {
                return "Fail";
            }
        }

        public string CertificateGenerateAmendment()
        {



            ApplicationFlowModel AppFlowModel = new ApplicationFlowModel();

            //DataTable dtAmendmentChanges = AppFlowModel.GetAmendmentChanges(SocietyIDEnc);
            string fileNameCerti = "";
            string fileNameMemList = "";
            string lbl_SocietyDirectory = "";
            string lbl_AO_ADD_DistrictEn = "";

            string lbl_InstitutionName = "AGARWAL SAMAJ SAMITI";
            string lbl_AO_ADD_DistrictHi = "";
            string lbl_Registraionno = "";
            string lbl_ActId = "";
            string lbl_OldName = "AGARWAL SAMAJ SEVA SAMITI";
            string BylawsStr = "";
            string lbl_RegistrationDate = "";
            //if (dtAmendmentChanges.Rows.Count > 0)

            //lbl_SocietyDirectory = dtAmendmentChanges.Rows[0]["SocietyDirectory"].ToString();
            //lbl_AO_ADD_DistrictEn = dtAmendmentChanges.Rows[0]["DISTRICT_MANGAL"].ToString();
            //lbl_InstitutionName = dtAmendmentChanges.Rows[0]["Name"].ToString();
            //lbl_AO_ADD_DistrictHi = dtAmendmentChanges.Rows[0]["DISTRICT_MANGAL"].ToString();
            //lbl_Registraionno = dtAmendmentChanges.Rows[0]["RegistrationNo"].ToString();
            //lbl_ActId = dtAmendmentChanges.Rows[0]["ActId"].ToString();
            //lbl_OldName = dtAmendmentChanges.Rows[0]["SocietyName"].ToString();
            //lbl_RegistrationDate = dtAmendmentChanges.Rows[0]["RegistrationDateFormated"].ToString();


            //BylawsStr = dtAmendmentChanges.Rows[0]["ByeLawsFile"].ToString();
            //if (dtAmendmentChanges.Rows[0]["Name"] != null && dtAmendmentChanges.Rows[0]["Name"].ToString() != "")
            //{
            //    DataTable dt = AppFlowModel.GetScietyAdministrativeDetail(SocietyIDEnc, 1, " ");
            //    if (dt.Rows.Count > 0)
            //    {
            //DataTable tblFiltered = dt.AsEnumerable().Where(row => row.Field<string>("POSTNAME").ToUpper() == "SECRETARY").CopyToDataTable();
            //lbl_InstitutionName = dt.Rows[0]["SocietyName"].ToString();
            string lbl_Name = "AGARWAL SAMAJ SEVA SAMITI";
            lbl_AO_ADD_DistrictEn = "Jaipur";
            lbl_AO_ADD_DistrictHi = "जयपुर";
            lbl_Registraionno = "COOP/2018/JAIPUR/102717";
            lbl_ActId = "1";
            //string lbl_SocietyDirectory = dt.Rows[0]["SocietyDirectory"].ToString();
            //string lbl_OldRegistrationNo = dt.Rows[0]["OldRegistrationNo"].ToString();
            //string lbl_ByLows = dt.Rows[0]["OldRegistrationNo"].ToString();
            // string Objects = dt.Rows[0]["OldRegistrationNo"].ToString();
            fileNameCerti = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + ".pdf";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string actNameEn = lbl_ActId == "1" ? ("The Rajasthan Societies Registration Act, 1958 (Rajasthan Act No. 28, 1958)").ToUpper() : lbl_ActId == "3" ? ("The Rajasthan Co-operative Societies Act, 2001").ToUpper() : lbl_ActId == "7" ? ("The Rajasthan Societies Registration Act, 2005").ToUpper() : "";
            string actNameHi = lbl_ActId == "1" ? ("राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 1958 (राजस्थान अधिनियम संख्या 28, 1958) ").ToUpper() : lbl_ActId == "3" ? ("राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 2001").ToUpper() : lbl_ActId == "7" ? ("राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 2005").ToUpper() : "";
            string devFontSize = "17px";
            string fontSize = "font-size: 13px;";

            sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:50px'>");
            sb.Append("<table style='width: 100%;border:1px solid;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px'>");
            sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
            sb.Append("<img style='width:90px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
            sb.Append("</td></tr>");
            sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
            sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:32px;'>" + UnicodeToKrutidev.GetKrutidev("सहकारिता विभाग") + "</label></b>");
            sb.Append("</td></tr>");
            sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");

            //if (lbl_ActId == "1" || lbl_ActId == "7")
            // {
            sb.Append("<b><label style='font-size:22px;'> " + UnicodeToKrutidev.GetKrutidev("रजिस्ट्रीकरण प्रमाण - पत्र") + " </label></b>");
            //}
            //else
            //{
            //    sb.Append("<b><label style='font-size:22px;'> " + UnicodeToKrutidev.GetKrutidev("पंजीयन प्रमाण - पत्र ") + " </label></b>");
            //}
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
            sb.Append("<table style='width:100%'>");
            sb.Append("<tr>");
            sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'><b> पं. संख्या " + lbl_Registraionno + "</b></td>");
            sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> दिनांक - " + DateTime.Now.ToString("dd-MM-yyyy") + " </td>");
            sb.Append("</tr>");
            sb.Append("</table>");
            sb.Append("</td>");
            sb.Append("</tr>");

            //sb.Append("<td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'> यह प्रमाणित किया जाता है कि <b> " + lbl_InstitutionName.ToUpper() + " </b> जिला <b> " + lbl_AO_ADD_DistrictEn.ToUpper() + " </b>  का रजिस्ट्रेशन <b>' " + actNameHi + " '</b>  के अन्तर्गत आज किया गया है। यह प्रमाण पत्र मेरे डिजिटल हस्ताक्षरों से आज जारी किया गया है। </td>");

            //if (lbl_ActId == "1" || lbl_ActId == "7")
            //{
            sb.Append("<tr><td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'><p style='text-align: justify;text-justify;'>  यह प्रमाणित किया जाता है कि <b> " + lbl_OldName.ToUpper() + " </b> जिला <b> " + lbl_AO_ADD_DistrictEn.ToUpper() + " </b>  का रजिस्ट्रेशन <b>' " + actNameHi + " '</b>  के अन्तर्गत दिनांक <b>" + lbl_RegistrationDate + "</b> को किया गया था। </p>");
            sb.Append("<p style='text-align: justify;text-justify;'> अब इस संस्थान का नाम परिवर्तित कर <b> " + lbl_InstitutionName.ToUpper() + " </b> जिला <b> " + lbl_AO_ADD_DistrictEn.ToUpper() + " </b>  कर दिया गया है। यह परिवर्तन राजस्थान संस्था रजिस्ट्रीकरण अधिनियम ," + (lbl_ActId == "1" ? "1958" : lbl_ActId == "3" ? "2001" : "2005") + " की धारा -12 के प्रावधानान्तर्गत किया गया है। </p>");
            sb.Append("<p style='text-align: justify;text-justify;'> यह प्रमाण पत्र मेरे हस्ताक्षर एवं कार्यालय की मुहर से दिनांक <b>" + DateTime.Now.ToString("dd-MM-yyyy") + "</b> को " + lbl_AO_ADD_DistrictHi + " में जारी किया गया। </p></td></tr>");
            //}
            //else if (lbl_ActId == "3")
            //{

            //    sb.Append("<tr><td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'><p style='text-align: justify;text-justify;'> प्रमाणित किया जाता है कि  श्री " + lbl_Name.ToUpper() + "  द्वारा " + lbl_InstitutionName.ToUpper() + "  के नाम से एक सहकारी सोसाइटी  के रजिस्ट्रेशन हेतु प्रस्तुत आवेदन को स्वीकार करते हुए उक्त  सोसाइटी एवं  उसके द्वारा अंगीकृत उपविधियों का राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 2001  की धारा 6 के अंतर्गत  आज  दिनांक " + DateTime.Now.ToString("dd-MM-yyyy") + "  को क्रमांक  " + lbl_Registraionno + "  पर  रजिस्ट्रीकरण किया गया । यह  प्रमाण पत्र  मेरी  मुद्रा  एवं हस्ताक्षर सहित आज दिनांक " + DateTime.Now.ToString("dd-MM-yyyy") + "  को प्रसारित  किया  जाता  है । </p></td></tr>");
            //    sb.Append("<tr><td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'><p style='text-align: justify;text-justify;'> " + ("Certified that accepting the application made by Mr " + lbl_Name.ToUpper() + " for the registration of a co-operative society under the name " + lbl_InstitutionName.ToUpper() + ", the said society alongwith the bye - laws adopted by it, is registered at number " + lbl_Registraionno + " on dated " + DateTime.Now.ToString("dd-MM-yyyy") + " under section 6 of The Rajasthan Co-operative Societies Act, 2001.This registration certificate is issued under my seal and signatures on date " + DateTime.Now.ToString("dd-MM-yyyy")).ToUpper() + " </p></td></tr>");
            //    sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'><br/><br/><br/><br/><br/><br/><br/></td></tr>");
            //}

            //sb.Append("<tr><td style='padding: 15px; line-height: 30px; font-size: 12px; font-weight: normal; height: 100px;'>IT IS CERTIFIED THAT <b>" + lbl_InstitutionName.ToUpper() + "</b> AT  DISTRICT  <b>" + lbl_AO_ADD_DistrictEn.ToUpper() + "</b> IS REGISTERED UNDER  <b>'" + actNameEn + "'</b>. THIS CERTIFICATE IS ISSUED TODAY UNDER MY DIGITAL SIGNATURE.</td></tr>");
            sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'><br/><br/><br/><br/><br/><br/><br/></td></tr>");
            sb.Append("</table>");
            sb.Append("</div>");
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));

            string filepath = System.Web.HttpContext.Current.Server.MapPath("~/Content/") + fileNameCerti;
            if (!COMMON_FUNCTION.GeneratePDF(sb1, filepath, ""))
            {
                throw new Exception("Error in Generate Certificate");
            };

            //DataTable dt1 = AppFlowModel.GetAdministrativeDetailsAmendment(SocietyIDEnc, "");//ad.UserType in (4
            // string memberResult = "";


            return "";
        }

        public void GetData()
        {

            //ApplicationTransactionMessage message = new ApplicationTransactionMessage();
            using (WebClient Client = new WebClient())
            {
                try
                {
                    RajfedData rajfedData = new RajfedData();
                    rajfedData.DistrictId = "099";
                    rajfedData.purchase_center = "";
                    rajfedData.Kvss = "";
                    rajfedData.commodityId = "1";
                    string jsonString = string.Empty;
                    jsonString = JsonConvert.SerializeObject(rajfedData);

                    var http = (HttpWebRequest)WebRequest.Create(new Uri("https://rajsahakarapp.rajasthan.gov.in/CommonServices/Mobile_GetTotalQuantityDetails"));
                    http.Accept = "application/json";
                    http.ContentType = "application/json";
                    http.Method = "POST";

                    // string parsedContent = <<PUT HERE YOUR JSON PARSED CONTENT>>;
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    Byte[] bytes = encoding.GetBytes(jsonString);

                    Stream newStream = http.GetRequestStream();
                    newStream.Write(bytes, 0, bytes.Length);
                    newStream.Close();

                    var response = http.GetResponse();

                    var stream = response.GetResponseStream();
                    var sr = new StreamReader(stream);
                    var content = sr.ReadToEnd();
                    //message = JsonConvert.DeserializeObject<ApplicationTransactionMessage>(content);
                }
                catch (Exception ex)
                {
                    //  ErrorLog.Write(ex);
                }
            }
            // return message;

        }
        public string CertificateGenerateAmendment1111()
        {



            ApplicationFlowModel AppFlowModel = new ApplicationFlowModel();

            //DataTable dtAmendmentChanges = AppFlowModel.GetAmendmentChanges(SocietyIDEnc);
            string fileNameCerti = "";
            string fileNameMemList = "";
            string lbl_SocietyDirectory = "";
            string lbl_AO_ADD_DistrictEn = "";

            string lbl_InstitutionName = "AGARWAL SAMAJ SAMITI";
            string lbl_AO_ADD_DistrictHi = "";
            string lbl_Registraionno = "";
            string lbl_ActId = "";
            string lbl_OldName = "AGARWAL SAMAJ SEVA SAMITI";
            string BylawsStr = "";
            string lbl_RegistrationDate = "";
            //if (dtAmendmentChanges.Rows.Count > 0)

            //lbl_SocietyDirectory = dtAmendmentChanges.Rows[0]["SocietyDirectory"].ToString();
            //lbl_AO_ADD_DistrictEn = dtAmendmentChanges.Rows[0]["DISTRICT_MANGAL"].ToString();
            //lbl_InstitutionName = dtAmendmentChanges.Rows[0]["Name"].ToString();
            //lbl_AO_ADD_DistrictHi = dtAmendmentChanges.Rows[0]["DISTRICT_MANGAL"].ToString();
            //lbl_Registraionno = dtAmendmentChanges.Rows[0]["RegistrationNo"].ToString();
            //lbl_ActId = dtAmendmentChanges.Rows[0]["ActId"].ToString();
            //lbl_OldName = dtAmendmentChanges.Rows[0]["SocietyName"].ToString();
            //lbl_RegistrationDate = dtAmendmentChanges.Rows[0]["RegistrationDateFormated"].ToString();


            //BylawsStr = dtAmendmentChanges.Rows[0]["ByeLawsFile"].ToString();
            //if (dtAmendmentChanges.Rows[0]["Name"] != null && dtAmendmentChanges.Rows[0]["Name"].ToString() != "")
            //{
            //    DataTable dt = AppFlowModel.GetScietyAdministrativeDetail(SocietyIDEnc, 1, " ");
            //    if (dt.Rows.Count > 0)
            //    {
            //DataTable tblFiltered = dt.AsEnumerable().Where(row => row.Field<string>("POSTNAME").ToUpper() == "SECRETARY").CopyToDataTable();
            //lbl_InstitutionName = dt.Rows[0]["SocietyName"].ToString();
            string lbl_Name = "AGARWAL SAMAJ SEVA SAMITI";
            lbl_AO_ADD_DistrictEn = "Jaipur";
            lbl_AO_ADD_DistrictHi = "जयपुर";
            lbl_Registraionno = "COOP/2018/JAIPUR/102717";
            lbl_ActId = "1";
            //string lbl_SocietyDirectory = dt.Rows[0]["SocietyDirectory"].ToString();
            //string lbl_OldRegistrationNo = dt.Rows[0]["OldRegistrationNo"].ToString();
            //string lbl_ByLows = dt.Rows[0]["OldRegistrationNo"].ToString();
            // string Objects = dt.Rows[0]["OldRegistrationNo"].ToString();
            fileNameCerti = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + ".pdf";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string actNameEn = lbl_ActId == "1" ? ("The Rajasthan Societies Registration Act, 1958 (Rajasthan Act No. 28, 1958)").ToUpper() : lbl_ActId == "3" ? ("The Rajasthan Co-operative Societies Act, 2001").ToUpper() : lbl_ActId == "7" ? ("The Rajasthan Societies Registration Act, 2005").ToUpper() : "";
            string actNameHi = lbl_ActId == "1" ? ("राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 1958 (राजस्थान अधिनियम संख्या 28, 1958) ").ToUpper() : lbl_ActId == "3" ? ("राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 2001").ToUpper() : lbl_ActId == "7" ? ("राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 2005").ToUpper() : "";
            string devFontSize = "17px";
            string fontSize = "font-size: 13px;";

            sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:50px'>");
            sb.Append("<table style='width: 100%;border:1px solid;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px'>");
            sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
            sb.Append("<img style='width:90px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
            sb.Append("</td></tr>");
            sb.Append("<tr><td style='text-align:center;font-size:30px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
            sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:25px;'>" + UnicodeToKrutidev.GetKrutidev("सहकारिता विभाग") + "</label></b>");
            sb.Append("</td></tr>");
            sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");

            //if (lbl_ActId == "1" || lbl_ActId == "7")
            // {
            sb.Append("<b><label style='font-size:22px;'> " + UnicodeToKrutidev.GetKrutidev("कार्यालय निर्वाचन अधिकारी") + " </label></b><br/><br/><br/>");
            sb.Append("<b><label style='font-size:20px;'> " + UnicodeToKrutidev.GetKrutidev("....................प्राथमिक दुग्ध उत्पादन सहकारी समिति लि. .........................") + " </label></b>");
            //}
            //else
            //{
            //    sb.Append("<b><label style='font-size:22px;'> " + UnicodeToKrutidev.GetKrutidev("पंजीयन प्रमाण - पत्र ") + " </label></b>");
            //}
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
            sb.Append("<table style='width:100%'>");
            sb.Append("<tr>");
            sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'><b> पं.स. " + lbl_Registraionno + "</b></td>");
            sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> कोड न " + DateTime.Now.ToString("dd-MM-yyyy") + " </td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'><b> नाम निर्वाचन अधिकारी ...................................................... </b></td>");
            sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'>  </td>");
            sb.Append("</tr>");
            sb.Append("</table>");
            sb.Append("</td>");
            sb.Append("</tr>");

            //sb.Append("<td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'> यह प्रमाणित किया जाता है कि <b> " + lbl_InstitutionName.ToUpper() + " </b> जिला <b> " + lbl_AO_ADD_DistrictEn.ToUpper() + " </b>  का रजिस्ट्रेशन <b>' " + actNameHi + " '</b>  के अन्तर्गत आज किया गया है। यह प्रमाण पत्र मेरे डिजिटल हस्ताक्षरों से आज जारी किया गया है। </td>");

            //if (lbl_ActId == "1" || lbl_ActId == "7")
            //{
            sb.Append("<tr><td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'><p style='text-align: justify;text-justify;'>  राजस्थान राज्य सहकारी निर्वाचन प्राधिकरण जयपुर के कार्यक्रमानुसार समिति का निर्वाचन दिनांक..........................से................................तक संपन्न किया, प्रकिरा में  निर्वाचित सञ्चालन मंडल एवं पदाधिकारियों का विवरण निम्न प्रकार है: - । </p>");
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("<tr>");
            sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
            sb.Append("<table style='width:100%' bordar='1'>");

            sb.Append("<tr>");
            sb.Append("<th>  क्र. </th>");
            sb.Append("<th>  नाम पदाधिकारी </th>");
            sb.Append("<th>  पद </th>");
            sb.Append("<th>  मोबईल नम्बर </th>");
            sb.Append("<th>  आधार नम्बर </th>");
            sb.Append("</tr>");


            sb.Append("<tr>");
            sb.Append("<td>  1 </td>");
            sb.Append("<td>  Balwant Chaudhary </td>");
            sb.Append("<td>  Secretory </td>");
            sb.Append("<td>  9460934860 </td>");
            sb.Append("<td>  23423423434 </td>");
            sb.Append("</tr>");

            sb.Append("<tr>");
            sb.Append("<td>  2. </td>");
            sb.Append("<td>  Rakesh kumar </td>");
            sb.Append("<td>  members </td>");
            sb.Append("<td>  2342344234 </td>");
            sb.Append("<td>  679875435435543 </td>");
            sb.Append("</tr>");



            sb.Append("</table>");
            sb.Append("</td>");
            sb.Append("</tr>");

            //}
            //else if (lbl_ActId == "3")
            //{

            //    sb.Append("<tr><td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'><p style='text-align: justify;text-justify;'> प्रमाणित किया जाता है कि  श्री " + lbl_Name.ToUpper() + "  द्वारा " + lbl_InstitutionName.ToUpper() + "  के नाम से एक सहकारी सोसाइटी  के रजिस्ट्रेशन हेतु प्रस्तुत आवेदन को स्वीकार करते हुए उक्त  सोसाइटी एवं  उसके द्वारा अंगीकृत उपविधियों का राजस्थान सोसाइटीज रजिस्ट्रेशन एक्ट, 2001  की धारा 6 के अंतर्गत  आज  दिनांक " + DateTime.Now.ToString("dd-MM-yyyy") + "  को क्रमांक  " + lbl_Registraionno + "  पर  रजिस्ट्रीकरण किया गया । यह  प्रमाण पत्र  मेरी  मुद्रा  एवं हस्ताक्षर सहित आज दिनांक " + DateTime.Now.ToString("dd-MM-yyyy") + "  को प्रसारित  किया  जाता  है । </p></td></tr>");
            //    sb.Append("<tr><td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'><p style='text-align: justify;text-justify;'> " + ("Certified that accepting the application made by Mr " + lbl_Name.ToUpper() + " for the registration of a co-operative society under the name " + lbl_InstitutionName.ToUpper() + ", the said society alongwith the bye - laws adopted by it, is registered at number " + lbl_Registraionno + " on dated " + DateTime.Now.ToString("dd-MM-yyyy") + " under section 6 of The Rajasthan Co-operative Societies Act, 2001.This registration certificate is issued under my seal and signatures on date " + DateTime.Now.ToString("dd-MM-yyyy")).ToUpper() + " </p></td></tr>");
            //    sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'><br/><br/><br/><br/><br/><br/><br/></td></tr>");
            //}

            //sb.Append("<tr><td style='padding: 15px; line-height: 30px; font-size: 12px; font-weight: normal; height: 100px;'>IT IS CERTIFIED THAT <b>" + lbl_InstitutionName.ToUpper() + "</b> AT  DISTRICT  <b>" + lbl_AO_ADD_DistrictEn.ToUpper() + "</b> IS REGISTERED UNDER  <b>'" + actNameEn + "'</b>. THIS CERTIFICATE IS ISSUED TODAY UNDER MY DIGITAL SIGNATURE.</td></tr>");

            sb.Append("</table>");
            sb.Append("</div>");
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));
            string filepath = System.Web.HttpContext.Current.Server.MapPath("~/Content/") + fileNameCerti;
            if (!COMMON_FUNCTION.GeneratePDF(sb1, filepath, ""))
            {
                throw new Exception("Error in Generate Certificate");
            };

            //DataTable dt1 = AppFlowModel.GetAdministrativeDetailsAmendment(SocietyIDEnc, "");//ad.UserType in (4
            // string memberResult = "";


            return "";
        }
        //public ActionResult RequestUser()
        //{
        //    string key = Request.QueryString["sso"];
        //    string[] Role = "HQ | ".Split('|');
        //    Response.Cookies.Add(new System.Web.HttpCookie("SSO_ID", "")
        //    {
        //        Value = key,
        //        HttpOnly = true
        //    });

        //    Response.Cookies.Add(new System.Web.HttpCookie("SSOROLE", "")
        //    {
        //        Value = Role[0],
        //        HttpOnly = true
        //    });
        //    Response.Cookies.Add(new System.Web.HttpCookie("RAJSSO", "")
        //    {
        //        Value = Role[0],
        //        HttpOnly = true
        //    });

        //    Response.Cookies.Add(new System.Web.HttpCookie(SessionIdentity.Culture, "")
        //    {
        //        Value = "en-us",
        //        HttpOnly = true
        //    });

        //    DeleteUnwantedCookies(new string[] { "RAJSSO", "SSOROLE", "SSO_ID", SessionIdentity.Culture });
        //    return RedirectToAction("SSOMapping", "Login");
        //}

        public List<LoginIP> LoadJson()
        {
            using (StreamReader r = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/Content/loginIP.json")))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<List<LoginIP>>(json);
            }
        }
        // create by rajeev kumawat 06042022 
        //public ActionResult RedirectToUser00AD()
        //{
        //    string key = Request.QueryString["sso"];
        //    string[] Role = "HQ | ".Split('|');
        //    Response.Cookies.Add(new System.Web.HttpCookie("SSOTOKEN", "")
        //    {
        //        Value = "",
        //        HttpOnly = true
        //    });
        //    Response.Cookies.Add(new System.Web.HttpCookie("SSO_ID", "")
        //    {
        //        Value = key,
        //        HttpOnly = true
        //    });
        //    Response.Cookies.Add(new System.Web.HttpCookie("SSOROLE", "")
        //    {
        //        Value = Role[0],
        //        HttpOnly = true
        //    });
        //    Response.Cookies.Add(new System.Web.HttpCookie("RAJSSO", "")
        //    {
        //        Value = Role[0],
        //        HttpOnly = true
        //    });
        //    Response.Cookies.Add(new System.Web.HttpCookie(SessionIdentity.Culture, "")
        //    {
        //        Value = "en-us",
        //        HttpOnly = true
        //    });
        //    (new SocietyModel()).addhistory("valid-RedirectToUser007-" + Request.QueryString["sso"] + "- " + GetUserDetails(Request.QueryString["sso"]).displayName);
        //    DeleteUnwantedCookies(new string[] { "SSOTOKEN", "RAJSSO", "SSOROLE", "SSO_ID", SessionIdentity.Culture });
        //    return RedirectToAction("SSOMapping", "Login");
        //}

        public ActionResult RedirectToDeptUser01()
        {
            string key = Request.QueryString["sso"];
            string[] Role = "HQ | ".Split('|');
            Response.Cookies.Add(new System.Web.HttpCookie("SSOTOKEN", "")
            {
                Value = "",
                HttpOnly = true
            });
            Response.Cookies.Add(new System.Web.HttpCookie("SSO_ID", "")
            {
                Value = key,
                HttpOnly = true
            });
            Response.Cookies.Add(new System.Web.HttpCookie("SSOROLE", "")
            {
                Value = Role[0],
                HttpOnly = true
            });
            Response.Cookies.Add(new System.Web.HttpCookie("RAJSSO", "")
            {
                Value = Role[0],
                HttpOnly = true
            });
            Response.Cookies.Add(new System.Web.HttpCookie(SessionIdentity.Culture, "")
            {
                Value = "en-us",
                HttpOnly = true
            });
            (new SocietyModel()).addhistory("valid-RedirectToUser007-" + Request.QueryString["sso"] + "- " + GetUserDetails(Request.QueryString["sso"]).displayName);
            DeleteUnwantedCookies(new string[] { "SSOTOKEN", "RAJSSO", "SSOROLE", "SSO_ID", SessionIdentity.Culture });
            return RedirectToAction("SSOMapping", "Login");
        }


        public ActionResult RedirectToCitiUser02()
        {
            string key = Request.QueryString["sso"];
            string[] Role = "CITIZEN | ".Split('|');
            Response.Cookies.Add(new System.Web.HttpCookie("SSOTOKEN", "")
            {
                Value = "",
                HttpOnly = true
            });
            Response.Cookies.Add(new System.Web.HttpCookie("SSO_ID", "")
            {
                Value = key,
                HttpOnly = true
            });
            Response.Cookies.Add(new System.Web.HttpCookie("SSOROLE", "")
            {
                Value = Role[0],
                HttpOnly = true
            });
            Response.Cookies.Add(new System.Web.HttpCookie("RAJSSO", "")
            {
                Value = Role[0],
                HttpOnly = true
            });
            Response.Cookies.Add(new System.Web.HttpCookie(SessionIdentity.Culture, "")
            {
                Value = "en-us",
                HttpOnly = true
            });
            (new SocietyModel()).addhistory("valid-RedirectToUser00C-" + Request.QueryString["sso"] + "- " + GetUserDetails(Request.QueryString["sso"]).displayName);
            DeleteUnwantedCookies(new string[] { "SSOTOKEN", "RAJSSO", "SSOROLE", "SSO_ID", SessionIdentity.Culture });
            return RedirectToAction("SSOMapping", "Login");
        }



        // create by madhuri For Citizen 27052024
        //public ActionResult RedirectToUser00CZ()
        //{
        //    string key = Request.QueryString["sso"];
        //    string[] Role = "CITIZEN | ".Split('|');
        //    Response.Cookies.Add(new System.Web.HttpCookie("SSOTOKEN", "")
        //    {
        //        Value = "",
        //        HttpOnly = true
        //    });
        //    Response.Cookies.Add(new System.Web.HttpCookie("SSO_ID", "")
        //    {
        //        Value = key,
        //        HttpOnly = true
        //    });
        //    Response.Cookies.Add(new System.Web.HttpCookie("SSOROLE", "")
        //    {
        //        Value = Role[0],
        //        HttpOnly = true
        //    });
        //    Response.Cookies.Add(new System.Web.HttpCookie("RAJSSO", "")
        //    {
        //        Value = Role[0],
        //        HttpOnly = true
        //    });
        //    Response.Cookies.Add(new System.Web.HttpCookie(SessionIdentity.Culture, "")
        //    {
        //        Value = "en-us",
        //        HttpOnly = true
        //    });
        //    (new SocietyModel()).addhistory("valid-RedirectToUser00C-" + Request.QueryString["sso"] + "- " + GetUserDetails(Request.QueryString["sso"]).displayName);
        //    DeleteUnwantedCookies(new string[] { "SSOTOKEN", "RAJSSO", "SSOROLE", "SSO_ID", SessionIdentity.Culture });
        //    return RedirectToAction("SSOMapping", "Login");
        //}


        public ActionResult RedirectToUser()
        {
            var IPSddress = LoadJson();


            //if (COMMON_FUNCTION.GetIpAddress() == "122.177.220.55" || COMMON_FUNCTION.GetIpAddress() == "::1" || COMMON_FUNCTION.GetIpAddress() == "45.119.8.245" || COMMON_FUNCTION.GetIpAddress() == "27.58.30.62")
            if (IPSddress.Where(x => x.IP == COMMON_FUNCTION.GetIpAddress()).Any())
            {
                string key = Request.QueryString["sso"];
                string[] Role = "HQ | ".Split('|');
                Response.Cookies.Add(new System.Web.HttpCookie("SSO_ID", "")
                {
                    Value = key,
                    HttpOnly = true
                });
                Response.Cookies.Add(new System.Web.HttpCookie("SSOROLE", "")
                {
                    Value = Role[0],
                    HttpOnly = true
                });
                Response.Cookies.Add(new System.Web.HttpCookie("RAJSSO", "")
                {
                    Value = Role[0],
                    HttpOnly = true
                });
                Response.Cookies.Add(new System.Web.HttpCookie(SessionIdentity.Culture, "")
                {
                    Value = "en-us",
                    HttpOnly = true
                });
                //(new SocietyModel()).addhistory("valid-" + Request.QueryString["sso"] + "- " + GetUserDetails(Request.QueryString["sso"]).displayName);
                DeleteUnwantedCookies(new string[] { "RAJSSO", "SSOROLE", "SSO_ID", SessionIdentity.Culture });
                return RedirectToAction("SSOMapping", "Login");
            }
            else
            {
                // (new SocietyModel()).addhistory("Invalid-" + Request.QueryString["sso"] + "- " + GetUserDetails(Request.QueryString["sso"]).displayName);
                DeleteUnwantedCookies(new string[] { "RAJSSO", "SSOROLE", "SSO_ID", SessionIdentity.Culture });
                return null;
            }
        }


        protected void btnSelect_Click()
        {

            string constr = System.Configuration.ConfigurationManager.ConnectionStrings["oldsite"].ConnectionString;
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(constr);
            DataTable dt = new DataTable();

            System.Data.SqlClient.SqlDataAdapter da = null;
            try
            {
                con.Open();
                da = new System.Data.SqlClient.SqlDataAdapter("select BANKPASSBOOK,BALANCESHEET,PROCEEDINGSGENERALMEETING,WORKPLANSHOWINGECONOMICVIABILITY,SOCIETYMEMBERSLIST,SOCIETYREGISTRATIONNO from TLBSOCIETYREGDETAILS where  SOCIETYREGISTRATIONNO like '%/cos/%'", con);
                da.Fill(dt);


                foreach (DataRow dr in dt.Rows)
                {
                    string SocietyDIR = dr["SOCIETYREGISTRATIONNO"].ToString().Replace("-", "").Replace("/", "").Replace(" ", "");
                    if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~/Content/OldSiteData1/" + SocietyDIR)))
                    {
                        System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("/Content/OldSiteData1/" + SocietyDIR));
                    }
                    if (dr["BANKPASSBOOK"] != null && dr["BANKPASSBOOK"].ToString() != "")
                    {
                        if (System.IO.File.Exists("E:\\Backup Cooperative HDD 1TB 04032022\\BackupCooperative\\Uploads Data\\" + dr["BANKPASSBOOK"].ToString()))
                        {
                            System.IO.File.Copy("E:\\Backup Cooperative HDD 1TB 04032022\\BackupCooperative\\Uploads Data\\" + dr["BANKPASSBOOK"].ToString(), Server.MapPath("/Content/OldSiteData1/" + SocietyDIR + "/" + dr["BANKPASSBOOK"].ToString()));
                        }
                    }

                    //if (dr["BALANCESHEET"] != null && dr["BALANCESHEET"].ToString() != "")
                    //{
                    //    if (System.IO.File.Exists("E:\\Backup Cooperative HDD 1TB 04032022\\BackupCooperative\\Uploads Data\\" + dr["BALANCESHEET"].ToString()))
                    //    {
                    //        System.IO.File.Copy("E:\\Backup Cooperative HDD 1TB 04032022\\BackupCooperative\\Uploads Data\\" + dr["BALANCESHEET"].ToString(), Server.MapPath("/Content/OldSiteData/" + SocietyDIR + "/" + dr["BALANCESHEET"].ToString()));
                    //    }
                    //}
                    //if (dr["PROCEEDINGSGENERALMEETING"] != null && dr["PROCEEDINGSGENERALMEETING"].ToString() != "")
                    //{
                    //    if (System.IO.File.Exists("E:\\Backup Cooperative HDD 1TB 04032022\\BackupCooperative\\Uploads Data\\" + dr["PROCEEDINGSGENERALMEETING"].ToString()))
                    //    {
                    //        System.IO.File.Copy("E:\\Backup Cooperative HDD 1TB 04032022\\BackupCooperative\\Uploads Data\\" + dr["PROCEEDINGSGENERALMEETING"].ToString(), Server.MapPath("/Content/OldSiteData/" + SocietyDIR + "/" + dr["PROCEEDINGSGENERALMEETING"].ToString()));
                    //    }
                    //}

                    //if (dr["WORKPLANSHOWINGECONOMICVIABILITY"] != null && dr["WORKPLANSHOWINGECONOMICVIABILITY"].ToString() != "")
                    //{
                    //    if (System.IO.File.Exists("E:\\Backup Cooperative HDD 1TB 04032022\\BackupCooperative\\Uploads Data\\" + dr["WORKPLANSHOWINGECONOMICVIABILITY"].ToString()))
                    //    {
                    //        System.IO.File.Copy("E:\\Backup Cooperative HDD 1TB 04032022\\BackupCooperative\\Uploads Data\\" + dr["WORKPLANSHOWINGECONOMICVIABILITY"].ToString(), Server.MapPath("/Content/OldSiteData/" + SocietyDIR + "/" + dr["WORKPLANSHOWINGECONOMICVIABILITY"].ToString()));
                    //    }
                    //}

                    //if (dr["SOCIETYMEMBERSLIST"] != null && dr["SOCIETYMEMBERSLIST"].ToString() != "")
                    //{
                    //    if (System.IO.File.Exists("E:\\Backup Cooperative HDD 1TB 04032022\\BackupCooperative\\Uploads Data\\" + dr["SOCIETYMEMBERSLIST"].ToString()))
                    //    {
                    //        System.IO.File.Copy("E:\\Backup Cooperative HDD 1TB 04032022\\BackupCooperative\\Uploads Data\\" + dr["SOCIETYMEMBERSLIST"].ToString(), Server.MapPath("/Content/OldSiteData/" + SocietyDIR + "/" + dr["SOCIETYMEMBERSLIST"].ToString()));
                    //    }
                    //}

                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
                //Response.Write("<script language='javascript'>alert(" + ex.ToString() + ")</script>");
            }
            finally
            {
                con.Close();
                da.Dispose();
                dt.Dispose();
            }
        }


        public void HTMLtoPDF()
        {
            var data = UnicodeToKrutidev.FindAndReplaceKrutidev(("पंजीयन अनूपगढ़ आवेदन फॉर्म के साथ ऑनलाइन जमा होगा ").ToString().Replace("<br>", "<br/>"));
            //string htmlText = System.IO.File.ReadAllText(Server.MapPath("~/Views/PldbLoan/WebForm1.aspx"));
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //sb.Append(htmlText);
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();

            string imgae = Server.MapPath("~/Content/Images/anupgarh.png");

            string fontSize = "font-size: 15px;";
            sb.Append("<div style='padding-left:50px;padding-right:50px;padding-top:20px'>");
            sb.Append("<html><body><table border='0' style='width:100%; " + fontSize + "' cellpadding='5' cellspacing='0'>");
            sb.Append("<tr><td colspan='5' style='text-align:center;font-size:32px;'><img src='" + imgae + "' /></td></tr>");
            sb.Append("");
            sb.Append("</table></body></html></div>");



            sb1.Append(data);
            if (COMMON_FUNCTION.GeneratePDF(sb1, Server.MapPath("~/Content/NFilfge6p995.pdf")))
            {
            }
        }


        public void DeleteUnwantedCookies(string[] CokkieKeyNotToDelete = null)
        {
            string[] Keys = System.Web.HttpContext.Current.Request.Cookies.AllKeys;
            foreach (var Key in Keys)
            {
                if (CokkieKeyNotToDelete == null || !CokkieKeyNotToDelete.Any(P => P == Key))
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

        public SSOWSSoap.SSOUserDetail GetUserDetails(string ssoid)
        {
            SSOWSSoap.SSOUserDetail usrdtl = new SSOWSSoap.SSOUserDetail();
            SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
            usrdtl = ssoclient.GetUserDetailXML(ssoid, "RAJSAHKAR.TEST", "Rajasthan@12");
            return usrdtl;
        }


        protected void SaveOldSitePhoto()
        {
            string constr = "Data Source=RPHO13\\SQL2019;Initial Catalog=Coop_Rajeev;Integrated Security=False;User ID=developer;Password=P@ssw0rd123;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;";// ConfigurationManager.ConnectionStrings["wmsprod"].ConnectionString;

            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(constr);

            DataTable dt = new DataTable();

            System.Data.SqlClient.SqlDataAdapter da = null;
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();
                foreach (string folderPath in System.IO.Directory.GetDirectories(Server.MapPath("/Content/OldSideMemberPhoto/")))
                {
                    string movePath = Server.MapPath("/Content/SocietyProfiling/" + folderPath.Split('\\')[folderPath.Split('\\').Length - 1]);
                    if (!System.IO.Directory.Exists(movePath))
                    {
                        System.IO.Directory.CreateDirectory(movePath);
                    }
                    foreach (string filePath in System.IO.Directory.GetFiles(folderPath))
                    {
                        string str = " update A set A.Photo='" + filePath.Split('\\')[filePath.Split('\\').Length - 1] + "' from AdministrativeDetails A inner join Societies S on S.societyid=A.SocietyId where A.Aadh='" + filePath.Split('\\')[filePath.Split('\\').Length - 1].Split('.')[0] + "' and REPLACE(REPLACE(S.RegistrationNo,'/',''),' ','')='" + folderPath.Split('\\')[folderPath.Split('\\').Length - 1] + "'";
                        var cmd = new System.Data.SqlClient.SqlCommand(str, con);//"insert into tbl_OLDMember(reg,auth)values('"+ folderPath.Split('\\')[folderPath.Split('\\').Length - 1] + "','"+ filePath.Split('\\')[filePath.Split('\\').Length - 1].Split('.')[0] +  "')", con);
                        cmd.ExecuteNonQuery();
                        System.IO.File.Move(folderPath + "/" + filePath.Split('\\')[filePath.Split('\\').Length - 1], movePath + "/" + filePath.Split('\\')[filePath.Split('\\').Length - 1]);

                    }
                }


                //for (int i =50; i <= 360; i++)
                //{
                //    int skip = (2000 * (i - 1));

                //        if (con.State == System.Data.ConnectionState.Closed)
                //            con.Open();
                //        da = new System.Data.SqlClient.SqlDataAdapter("select  A.PHOTO,B.SOCIETYREGISTRATIONNO,AADHAAR from TBLSOCIETYMEMBER A inner join TLBSOCIETYREGDETAILS B on B.ID=A.SOCIETYREGNO order by A.SOCIETYMEMBERID desc OFFSET " + skip + " ROWS FETCH NEXT 2000  ROWS ONLY  ", con);
                //        da.Fill(dt);

                //        if (!System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/OldSideMemberPhoto")))
                //        {
                //            System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/OldSideMemberPhoto"));

                //        }
                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        try
                //        {
                //            string fileName = dr["AADHAAR"].ToString() + ".png"; // + "_" + Guid.NewGuid().ToString()
                //            string folderName = dr["SOCIETYREGISTRATIONNO"].ToString().Replace(" ", "").Replace("-", "").Replace("/", "").Replace("_", "");
                //            if (!System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/OldSideMemberPhoto/" + folderName)))
                //            {
                //                System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/OldSideMemberPhoto/") + folderName);

                //            }


                //            if (!System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/OldSideMemberPhoto/" + folderName + "/" + fileName)))
                //            {
                //                System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/OldSideMemberPhoto"));
                //                System.IO.File.WriteAllBytes(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/OldSideMemberPhoto/") + folderName + "/" + fileName), Convert.FromBase64String(dr["PHOTO"].ToString().Split(',')[1]));

                //            }
                //        }
                //        catch (Exception ex)
                //        {

                //        }
                //    }

                //}
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
                //da.Dispose();
                //dt.Dispose();
            }

        }


        //KARAN.KUMAR335523

        public string GetSSOuserdetails(string methodURL, string ssoToken)
        {
            string wsUsername = "RAJSAHKAR.TEST";
            string wsPassword = "Rajasthan@12";
            var result = "";

            var responseString = Task.Run(() =>
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, methodURL);
                request.Headers.Add("SSO-TOKEN", ssoToken);
                request.Headers.Add("Authorization", "Basic " +
                Convert.ToBase64String(Encoding.Default.GetBytes(wsUsername + ":" + wsPassword)));
                var response = client.SendAsync(request).Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadAsStringAsync().Result;
            }).Result;
            if (responseString != null && !responseString.Contains("User Login Id or Password is incorrect"))
            {
                var userJson = JsonConvert.DeserializeObject<UserTypeModel>(responseString);
                result = $"sAMAccountName={userJson.sAMAccountName};Role={(userJson.UserType == "GOVT" ? "HQ" : userJson.UserType)}|";
            }
            return result;
        }
    }
}

public class RajfedData
{
    public string DistrictId { get; set; }
    public string purchase_center { get; set; }
    public string commodityId { get; set; }
    public string Kvss { get; set; }
}

public class LoginIP
{
    public string IP { get; set; }
}
