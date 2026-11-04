using CoopDAL;
using Cooperative.DBConnector;
using Cooperative.Filters;
using Cooperative.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Cooperative.Controllers
{
    [Error]
    [LogActionFilter]
    [CustomAuthentication]
    public class AuditorNewController : BaseController
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        // GET: Auditor
        
        [HttpGet]
        public ActionResult Auditor()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.Auditor, (Request.Cookies[SessionIdentity.Culture]).Value);
                ViewBag.BreadCrumbPath = new MasterModel(new DBLogger()).GetBreadCrumb((Request.Cookies[SessionIdentity.Culture]).Value);
            }
            return View();
        }

        public ActionResult AuditorCurrentYearForm()
        {
            return View();
        }

        public ActionResult AuditorCurrentYearFormNew()
        {
            return View();
        }

        [HttpGet]
        [ActionName("GetAuditCommiteeDetails")]
        public string GetAuditCommiteeDetails(string selectedDate, string financialYear, int districtId)
        {
            DataTable dt = new AuditorNewModel().GetAuditCommiteeDetails(selectedDate , financialYear,  districtId);
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpPost]
        public int SaveAuditResult(List<AuditorNewModel> model)
        {
            // bool isLocal = Request.Url.Host.Contains("localhost");
            int result = new AuditorNewModel().SaveAuditResult(model);

            return result;
        }

        [HttpGet]
        [ActionName("GetAuditAllDetails")]
        public string GetAuditAllDetails(int officeId, int month, int DistrictId, int resionId = 0, int DivisionId = 0)
        {
            bool isLocal = Request.Url.Host.Contains("localhost");
            DataTable dt = new AuditorNewModel().GetAuditAllDetails(officeId,  month,  DistrictId, isLocal, resionId, DivisionId);
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }



        [HttpGet]
        [ActionName("GetUserDetailsAuditBySSO")]
        public string GetUserDetailsAuditBySSO()
        {
            DataTable dt = new AuditorNewModel().GetUserDetailsAuditBySSO();

            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpGet]
        [ActionName("GetAuditorCurrentYearFormNew")]
        public string GetAuditorCurrentYearFormNew(string selectedDate, string financialYear, int districtId)
        {
            DataTable dt = new AuditorNewModel().GetAuditorCurrentYearFormNew(selectedDate, financialYear, districtId);

            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }


    }
}