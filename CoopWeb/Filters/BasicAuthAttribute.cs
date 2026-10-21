using System;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Cooperative.Controllers;
using Cooperative.Models;
using System.Web.Routing;

namespace Cooperative.Filters
{
    public class BasicAuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var req = filterContext.HttpContext.Request;

            if (req.Headers["Authorization"] != null)
            {
                var authHeader = AuthenticationHeaderValue.Parse(req.Headers["Authorization"]);
                if (authHeader.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) &&
                    authHeader.Parameter != null)
                {
                    var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter)).Split(':');
                    var username = credentials[0];
                    var password = credentials[1];

                    // Validate the credentials (replace with your own logic)
                    if (IsAuthorizedUser(username, password))
                    {
                        return;
                    }
                }
            }

            // Reject the request if authorization fails
            filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            filterContext.HttpContext.Response.Headers.Add("WWW-Authenticate", "Basic realm=\"RAJSAHAKARAPP\"");
            filterContext.Result = new RedirectResult("~/Error/UnAuthorize401");
        }

        private bool IsAuthorizedUser(string username, string password)
        {
            return username == "RAJSAHAKAR" && password == "COOP#321";
        }
    }
}