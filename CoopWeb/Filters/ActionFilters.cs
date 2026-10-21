using System;
using System.Web;
using System.Web.Mvc;
using Cooperative.Controllers;
using Cooperative.Models;

namespace Cooperative.Filters
{
    public class CustomActionAttribute : ActionFilterAttribute,IResultFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string ExecutingActionName = filterContext.ActionDescriptor.ActionName;
            string ExecutingControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string ActionId = Convert.ToString(filterContext.RouteData.Values["id"]);
            int _ActionId = (int.TryParse(ActionId, out _ActionId) ? _ActionId : -1);

            UserRegistrationModel _objUserModel = new UserRegistrationModel(new DBLogger());

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[Cooperative.Controllers.SessionIdentity.Admin];

            //if (!_objUserModel.IsUserAuthorized("/" + ExecutingControllerName + "/" + ExecutingControllerName, _ActionId, _LoggedInUser))
            //{
            //    filterContext.Result = new ViewResult
            //    {
            //        ViewName = "~/Views/Error/UnAuthorizeAccess401.cshtml",
            //        MasterName = "~/Views/Shared/_Layout.cshtml",
            //        TempData = filterContext.Controller.TempData
            //    };
            //   // base.OnActionExecuting(filterContext);
            //}
            //else
            //{
            //    base.OnActionExecuting(filterContext);
            //}

            //base.OnActionExecuting(filterContext);
        }

        #region (Currently Not In Use) Action Filter Methods 
        //public override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //}

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            //UserEntity _LoggedInUser = (UserEntity)HttpContext.Current.Session[SessionIdentity.Admin];
            //(new DBLogger()).ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative", FunctionName = "public DataTable Error(" + _LoggedInUser.UserId + ")" }, new Exception());

            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Error/UnAuthorizeAccess401.cshtml",
               
               // MasterName = "~/Views/Shared/_Layout.cshtml",
                //TempData = filterContext.Controller.TempData
            };
        }

        
        #endregion
    }
}