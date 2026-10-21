using System.IO;
using System.Web.Mvc;

namespace Cooperative.Controllers
{
    public class DynamicController : Controller
    {
        public ActionResult DynamicView(string url)
        {
            // split url into parts
            var parts = (url ?? "").Split('/');
            string controller = parts.Length > 0 ? parts[0] : "Home";
            string page = parts.Length > 1 ? parts[1] : "Index";

            // check if view exists
            string viewPath = $"~/Views/{controller}/{page}.cshtml";
            if (System.IO.File.Exists(Server.MapPath(viewPath)))
            {
                return View(viewPath);
            }
                     
            return RedirectToAction("HttpError404", "error");
        }
        //public ActionResult DynamicView(string controller, string page)
        //{
        //    // Path for view
        //    string viewPath = $"~/Views/{controller}/{page}.cshtml";

        //    if (System.IO.File.Exists(Server.MapPath(viewPath)))
        //    {
        //        return View(viewPath);
        //    }

        //    // return HttpNotFound($"View not found: {viewPath}");

        //    //var ssoController = new SSOValidationController();
        //    //return ssoController.ValidateUser();

        //    //return RedirectToAction("ValidateUser", "SSOValidationController");
        //    //Response.Redirect($"/SSOValidation/{"SSOValidationController"}");
        //}

    }
}