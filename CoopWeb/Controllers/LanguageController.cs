using Cooperative.Filters;
using System;
using System.Web;
using System.Web.Mvc;

namespace Cooperative.Controllers
{
    [Error]
    [CustomAuthentication]
    public class LanguageController : BaseController
    {
        // Change Language ("English" to "Hindi" and Vice Versa)
        [HttpGet]
        public ActionResult ChangeLanguage(string id, string returnURL)
        {
            id = CultureHelper.GetImplementedCulture(id);

            HttpCookie cookie = Request.Cookies[SessionIdentity.Culture];

            if (cookie != null)
                cookie.Value = id;   // update cookie value
            else
            {
                cookie = new HttpCookie(SessionIdentity.Culture);
                cookie.Value = id;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            if(returnURL!=null)
               return Redirect(returnURL);
            else
                return Redirect(String.Format("/Error/General"));
        }
    }
}