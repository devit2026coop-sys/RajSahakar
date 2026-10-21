using System;
using System.Collections.Generic;
using Cooperative.Models;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Configuration;
using System.Web.Http;
using Cooperative.Controllers;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;
using System.Security.Policy;

namespace Cooperative
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();

            HttpException httpException = exception as HttpException;

            if (httpException != null)
            {
                string action;

                switch (httpException.GetHttpCode())
                {
                    case 404:
                        // page not found
                        action = "HttpError404";
                        break;
                    case 400:
                        action = "HttpError400";
                        break;
                    case 500:
                        // server error
                        action = "HttpError404";
                        break;
                    default:
                        action = "General";
                        break;
                }

                //clear error on server
                Server.ClearError();
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.MvcApplication", FunctionName = "Application_Error(object sender, EventArgs e)" }, httpException);
                Response.Redirect(String.Format("/Error/{0}", action), true);
            }
        }
        void Application_BeginRequest(object sender, EventArgs e)
        
        {
            //if (!Context.Request.IsSecureConnection)
            //{
            //    Response.Redirect(Context.Request.Url.ToString().Replace("http:", "https:"));
            //}
            string filePath = Server.MapPath("~/Scripts/AngularFramework/whitelistDomain.json");

            try
            {
               
                var referer = Request.Headers["Referer"]?.ToString();

                var origin = HttpContext.Current.Request.Headers["Origin"];

                if (!string.IsNullOrEmpty(referer))
                {
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
                    {
                        using (StreamReader reader = new StreamReader(fileStream, Encoding.UTF8, true, 1024, true))
                        {
                            string content = reader.ReadToEnd();
                            var domain = JsonConvert.DeserializeObject<DomainChecker>(content);
                            var isExists = domain.Domain.Exists(x => referer.Contains(x));
                            if (!isExists || referer.Contains("postman"))
                            {
                                Response.Redirect(String.Format("/Error/General"));
                            }


                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
            //HttpContext.Current.Response.Headers.Remove("Server");
        }

        protected void Application_EndRequest()
         {
            if (Response.Cookies.Count > 0)
            {
                foreach (string s in Response.Cookies.AllKeys)
                {
                    if (Request.Url.Host.Contains("10.70.234.208")|| Request.Url.Host.Contains("devitsandbox"))
                    {
                        //Response.Cookies[s].Secure = true;
                        //Response.Cookies[s].HttpOnly = true;
                        Response.Cookies[s].SameSite = SameSiteMode.Strict;
                    }
                    else
                    {
                        if (s != "_culture")
                        {
                            Response.Cookies[s].Secure = true;
                            Response.Cookies[s].HttpOnly = true;
                        }
                        Response.Cookies[s].SameSite = SameSiteMode.Strict;
                    }

                }
            }
        }
        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            if (Request.Url.Host.Contains("10.70.234.208"))
            {
                HttpContext.Current.Response.Headers.Remove("X-AspNetMvc-Version");
                HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
                HttpContext.Current.Response.Headers.Remove("Server");
            }

        }
    }
}
