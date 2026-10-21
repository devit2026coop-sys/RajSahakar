using Cooperative.Filters;
using Cooperative.Models;
using System.Web.Mvc;

namespace Cooperative.Controllers
{
    [Error]
    //[CustomAuthentication]
    public class ErrorController : Controller
    {
        public ActionResult HttpError404()
        {
            FileTestLogger objILoggerTest = new FileTestLogger();           
            return View("HttpError404");
        }
        public ActionResult HttpError500()
        {
            return View("HttpError405");
        }
        public ActionResult General()
        {
            return View();
        }
        public ActionResult UnAuthorize401()
        {
            return View();
        }

        public ActionResult UnAuthorizeAccess401()
        {
            return View();
        }
        public ActionResult HttpError400()
        {
            FileTestLogger objILoggerTest = new FileTestLogger();
            return View("HttpError400");
        }

    }
}