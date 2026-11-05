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
        public string GetAuditCommiteeDetails()
        {
            DataTable dt = new AuditorNewModel().GetAuditCommiteeDetails();
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }



        //[HttpPost]
        //public JsonResult SaveAuditResult(List<AuditorNewModel> model)
        //{
        //    // BREAKPOINT 1 - Yahan F9 lagayein

        //    try
        //    {
        //        if (model == null)
        //        {
        //            return Json(new
        //            {
        //                Success = false,
        //                Result = 0,
        //                Message = "Controller called but model is NULL."
        //            });
        //        }

        //        if (model.Count == 0)
        //        {
        //            return Json(new
        //            {
        //                Success = false,
        //                Result = 0,
        //                Message = "Controller called but list is EMPTY."
        //            });
        //        }

        //        if (!ModelState.IsValid)
        //        {
        //            var errors = ModelState
        //                .Where(x => x.Value.Errors.Count > 0)
        //                .Select(x => new
        //                {
        //                    Field = x.Key,
        //                    Errors = x.Value.Errors.Select(e => e.ErrorMessage)
        //                })
        //                .ToList();

        //            System.Diagnostics.Debug.WriteLine(
        //                Newtonsoft.Json.JsonConvert.SerializeObject(errors)
        //            );

        //            return Json(new
        //            {
        //                Success = false,
        //                Result = 0,
        //                Message = "Model binding validation failed."
        //            });
        //        }

        //        // BREAKPOINT 2
        //        int result = new AuditorNewModel()
        //            .SaveAuditResult(model);

        //        // BREAKPOINT 3
        //        return Json(new
        //        {
        //            Success = result > 0,
        //            Result = result,
        //            Message = result > 0
        //                ? "Audit data saved successfully."
        //                : "Database method returned 0."
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Trace.TraceError(ex.ToString());

        //        Response.StatusCode = 500;

        //        return Json(new
        //        {
        //            Success = false,
        //            Result = 0,
        //            Message = "Server error during save."
        //        });
        //    }
        //}


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

        


        //[HttpPost]
        //[ActionName("SaveSmsFormat")]
        //public JsonResult MenuSmsActionInsert(AuditorModel _Model)
        //{
        //    ModelState.Remove("UpdationDate"); ModelState.Remove("Createdate");
        //    if (!ModelState.IsValid)
        //        return Json(JsonModelError.Get(ModelState.Values));

        //    return Json(_Model.AddSmsFormat());
        //}


    }
}