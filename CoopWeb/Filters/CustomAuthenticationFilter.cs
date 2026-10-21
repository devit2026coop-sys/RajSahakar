using System.Web.Mvc.Filters;
using System.Web.Routing;
using System.Web.Mvc;
using System.Web;
using Cooperative.Controllers;
using Cooperative.Models;
using System;

namespace Cooperative.Filters
{
    public class CustomAuthenticationAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (HttpContext.Current.Session[SessionIdentity.Admin] == null)
            {
                filterContext.Result = new HttpUnauthorizedResult();// new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Logout", area = "" }));
            }
            else
            {
              //  base.OnActionExecuting(filterContext);
            }
        }




        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                //if (filterContext.HttpContext.Request.IsAjaxRequest())
                //{
                //    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                //    {
                //        Action = "GotoSSO",
                //        Controller = "Login"
                //    }));
                //}
                //else

                if (HttpContext.Current.Session[SessionIdentity.Admin] == null)
                {
                    HttpContext.Current.Response.Headers.Add("Status Code", "401");

                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        Action = "GotoSSO",
                        Controller = "Login"
                    }));

                    // filterContext.Result =   new ViewResult { ViewName = "~/Login/GotoSSO" };
                }
                else
                {
                    HttpContext.Current.Response.Headers.Add("Status Code", "401");
                    //UserEntity _LoggedInUser = (UserEntity)HttpContext.Current.Session[SessionIdentity.Admin];

                    UserEntity _LoggedInUser = HttpContext.Current.Session[SessionIdentity.Admin] as UserEntity;

                    if (_LoggedInUser != null)
                    {
                        //if (!(new UserModel()).IsUserAuthenticate(System.Convert.ToInt32(_LoggedInUser.UserId)))
                        //    filterContext.Result = new ViewResult { ViewName = "~/Views/Error/UnAuthorizeAccess401.cshtml" };
                        //else
                        //    filterContext.Result = new ViewResult { ViewName = "~/Views/Error/General.cshtml" };
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                        {
                            Action = "UnAuthorizeAccess401",
                            Controller = "Error"
                        }));
                        //filterContext.Result = new ViewResult { ViewName = "~/Views/Error/UnAuthorizeAccess401" };
                    }
                }

            }
            
        }
    }

    public class LogActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //string actionName = (string)filterContext.RouteData.Values["action"];
            //string actionName = (string)filterContext.RouteData.Values["action"];

            string actionName = filterContext.ActionDescriptor.ActionName;
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            //if (HttpContext.Current.Request.Headers["Referer"] == null   && actionName!="index" && controllerName!="Home")
            //{
            //    HttpContext.Current.Response.Redirect(String.Format("/login/LoginPage"));
            //}

            if (HttpContext.Current.Session[SessionIdentity.Admin] != null)
            {
                UserEntity _LoggedInUser = (UserEntity)HttpContext.Current.Session[SessionIdentity.Admin];
                UserModel UM = new UserModel();
                bool Result = UM.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);
                if (Result == false)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        Action = "UnAuthorizeAccess401",
                        Controller = "Error"
                    }));
                    //filterContext.Result = new ViewResult { ViewName = "~/Views/Error/UnAuthorizeAccess401" };
                }
            }
        }

        //public void Log()
        //{

        //    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        //    UserModel UM = new UserModel();
        //    bool Result = UM.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);
        //    if (Result == false)
        //    {
        //        //return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
        //        filterContext.Result = new ViewResult { ViewName = "~/Views/Login/Login.cshtml" };
        //    }

        //}

    }

}