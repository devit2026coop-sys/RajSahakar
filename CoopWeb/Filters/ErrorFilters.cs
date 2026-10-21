using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Cooperative.DBConnector;
using Cooperative.Models;

namespace Cooperative.Filters
{
    public class ErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            //if ((filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            //    || (new HttpException(null, filterContext.Exception).GetHttpCode() != 500)
            //    || (!ExceptionType.IsInstanceOfType(filterContext.Exception)))
            //    return;

            if (IsAjax(filterContext))  // if the request is AJAX then return JSON
            {
                filterContext.Result = new JsonResult()
                {
                    Data = filterContext.Exception.Message,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
            }
            else // Request is NOT AJAX so return view.
            {
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.Controller.ViewBag.OnExceptioError = "ErrorLog=>OnException Called.";

                //filterContext.HttpContext.Response.Write("ErrorLog=>OnException Called."); // No Need
                // filterContext.HttpContext.Response.StatusCode = 500; // No Need
                // filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;  // No Need

                var _ControllerName = (string)filterContext.RouteData.Values["controller"];
                var _ActionName = (string)filterContext.RouteData.Values["action"];
                var model = new HandleErrorInfo(filterContext.Exception, _ControllerName, _ActionName);
                var _ErrorCode = new HttpException(null, filterContext.Exception).GetHttpCode();
                if (_ErrorCode != 404)
                    Log(_ControllerName, _ActionName, filterContext.Exception.Message, model.Exception); // Save Error Log in Database

                if (filterContext.IsChildAction)
                {
                    // For child actions, return a simple message or nothing to avoid stack overflow or redirect error
                    //filterContext.Result = new ContentResult
                    //{
                    //    Content = "An error occurred while loading a component."
                    //};
                    //filterContext.Result = new EmptyResult();
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = "Erorr", action = "HttpError500" })
                    );
                    return;
                }
                else
                {
                    // Normal error page for full page requests
                    filterContext.Result = new ViewResult
                    {
                        ViewName = GetErrorViewPath(_ErrorCode),
                        MasterName = "~/Views/Shared/_Layout.cshtml",
                        ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                        TempData = filterContext.Controller.TempData
                    };
                }
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            }
        }

        private string GetErrorViewPath(int HttpErrorCode)
        {
            switch (HttpErrorCode)
            {
                case 401: return "~/Views/Error/UnAuthorizeAccess401.cshtml";
                case 404: return "~/Views/Error/HttpError404.cshtml";
                case 500: return "~/Views/Error/HttpError500.cshtml";
                default: return "~/Views/Error/General.cshtml";
            }
        }

        //If Request is Ajax or Normal. 0


        private bool IsAjax(ExceptionContext filterContext)
        {
            return filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        // Save Error Log into Database.
        private void Log(string _ControllerName, string _ActionName, string _Message, System.Exception ex)
        {
            DBFactoryMSSQL objDBAccess = new DBFactoryMSSQL();
            ILogger objILogger = new DBLogger();
            try
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "ErrorAttribute : HandleErrorAttribute", FunctionName = "OnException(ExceptionContext filterContext)" }, ex);
            }
            catch
            {

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
        }
    }
}