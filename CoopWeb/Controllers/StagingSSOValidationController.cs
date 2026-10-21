using Cooperative.Helper;
using Cooperative.Models;
using System;
using System.Linq;
using System.Web.Mvc;
namespace Cooperative.Controllers
{
    public class StagingSSOValidationController : Controller
    {
        public ActionResult ValidateUser()
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

                    result = SsoServiceHelper.GetSSOTokenDetails(userToken);
                    userdetails = SsoServiceHelper.GetSSOUserProfileDetail(result.Data.sAMAccountName, userToken);

                }
                catch (Exception ex)
                {
                    ILogger objILogger = new FileLogger();
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.StagingSSOValidationController", FunctionName = "ActionResult ValidateUser()" }, ex);
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
                            Value = userdetails.userType,
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
                        return RedirectToAction("StagingSSOMapping", "Login");
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


        public ActionResult RedirectToUser00AD()
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
            (new SocietyModel()).addhistory("valid-RedirectToUser007-" + Request.QueryString["sso"] + "- " +SsoServiceHelper.GetSSOUserDetail(Request.QueryString["sso"]).displayName);
            DeleteUnwantedCookies(new string[] { "RAJSSO", "SSOROLE", "SSO_ID", SessionIdentity.Culture });
            return RedirectToAction("SSOMapping", "Login");
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
    }
}

