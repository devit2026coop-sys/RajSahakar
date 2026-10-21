using Cooperative.Filters;
using Cooperative.Models;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;

namespace Cooperative.Controllers
{
    [Error]
    [CustomAuthentication]
    public class GetSupportController : BaseController
    {
        // GET: GetSupport


        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        public ActionResult GetSupport()
        {
            //(new DBLogger()).ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative", FunctionName = "public DataTable Error(" + _LoggedInUser.UserId + ")" }, new Exception());

            FileTestLogger objILoggerTest = new FileTestLogger();
            if ("umeshcj.doit" != _LoggedInUser.SSOID.ToLower().Trim())
            {
                return new ViewResult { ViewName = "~/Views/Error/UnAuthorizeAccess401" };
            }
            return View();
        }

        public ActionResult GetSupporttt()
        {
            FileTestLogger objILoggerTest = new FileTestLogger();            
            return View();
        }

        [HttpPost]
        [ActionName("GetSupportData")]
        public string GetSupportData(MailFormat _Model,string Username,string Password)
        {
            //return COMMON_FUNCTION.DataTableToJSONString(_Model.GetSupportData()).Replace(System.Environment.NewLine, "");
            if (Username.ToLower().Trim() == "umeshcj.doit" && Password.ToLower().Trim() == "abcd@1234")
            {
                return JsonConvert.SerializeObject(_Model.GetSupportData(), Newtonsoft.Json.Formatting.Indented);
            }
            else
            {
                return "Error";
            }
        }
    }
}