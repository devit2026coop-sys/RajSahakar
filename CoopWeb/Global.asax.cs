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
            Exception ex = Server.GetLastError();

            Server.ClearError();

            Response.Clear();
            Response.StatusCode = 500;
            Response.TrySkipIisCustomErrors = true;
            Response.ContentType = "text/plain";

            Response.Write(ex?.ToString() ?? "Unknown application error");
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
                    if (Request.Url.Host.Contains("10.70.234.208")|| Request.Url.Host.Contains("devitsandbox") || Request.Url.Host.Contains("164.100.222.195"))
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
