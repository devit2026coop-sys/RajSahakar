using Cooperative.DBConnector;
using Cooperative.Filters;
using Cooperative.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Cooperative.Controllers
{
    [Error]
    [LogActionFilter]
    [CustomAuthentication]
    public class KPIController : BaseController
    {
        KPIModel kpiModel = new KPIModel(new DBLogger());
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

        [HttpGet]
        public ActionResult PacsFormForKPI()
        {
            return View();
        }

        [HttpGet]
        public ActionResult BhandarFormForKPI()
        {
            return View();
        }

        [HttpGet]
        public ActionResult KPIForPLDB()
        {
            return View();
        }

        // KPI For SAO
        #region 
        [HttpGet]
        public ActionResult SAOForKPI()
        {
            return View();
        }
        #endregion

        // KPI For RAO
        #region 
        [HttpGet]
        public ActionResult RAOForKPI()
        {
            return View();
        }
        #endregion

        [HttpGet]
        public ActionResult UnitOfficeForKPI()
        {
            return View();
        }

        //old
        [HttpPost]
        public int SaveKPIResult(List<KPIResultModel> model)
        {
            // bool isLocal = Request.Url.Host.Contains("localhost");
            int result = new KPIModel().SaveKPIResult(model);

            return result;
        }

        //new
        //[HttpPost]
        //public int SaveKPIResult(List<KPIResultModel> model)
        //{
        //    bool isLocal = Request.Url.Host.Contains("localhost");
        //    int result = new KPIModel().SaveKPIResult(model, isLocal);
        //    return result;
        //}


        [HttpGet]
        [ActionName("GetPacsKPIDetails")]
        public string GetPacsKPIDetails()
        {
            DataTable dt = new KPIModel().GetPacsKPIDetails();
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpGet]
        [ActionName("GetBhandarKPIDetails")]
        public string GetBhandarKPIDetails()
        {
            DataTable dt = new KPIModel().GetBhandarKPIDetails();
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpGet]
        [ActionName("GetPLDBKPIDetails")]
        public string GetPLDBKPIDetails()
        {
            DataTable dt = new KPIModel().GetPLDBKPIDetails();
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }



        [HttpPost]
        public string CitizenDashboardDataWithDepartmentUser(SocietyRegistrationEntity objSocietyRegistrationEntity)
        {

            KPIModel objMenuActionModel = new KPIModel(new FileLogger());
            DataTable DT = null;
            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"].Value == "KIOSK" && Request.Cookies["SSO_ID"] != null && System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc] != null)
            {
                objSocietyRegistrationEntity.UserType = "K";
                // objSocietyRegistrationEntity.SSOID = Request.Cookies["SSO_ID"].Value;
                objSocietyRegistrationEntity.SocietyIDEnc = System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc].ToString().Split('~')[0];
                DT = objMenuActionModel.SocietyListWithDepartmentUser(objSocietyRegistrationEntity);
                if (DT != null && DT.Rows.Count > 0)
                {
                    System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc] = null;
                }
            }
            else if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
            {
                if (_LoggedInUser.UserType == "D")
                {
                    objSocietyRegistrationEntity.UserType = _LoggedInUser.UserType;
                    objSocietyRegistrationEntity.SSOID = _LoggedInUser.SSOID;
                    objSocietyRegistrationEntity.DistrictId = _LoggedInUser.DistrictId;
                    objSocietyRegistrationEntity.UserRoleDeptMappingId = _LoggedInUser.UserRoleDeptMappingId;

                    DT = objMenuActionModel.SocietyListWithDepartmentUser(objSocietyRegistrationEntity);
                }
                else
                {
                    objSocietyRegistrationEntity.UserType = _LoggedInUser.UserType;
                    objSocietyRegistrationEntity.OrganizationId = _LoggedInUser.UserRoleDeptMappingId;
                    objSocietyRegistrationEntity.SSOID = Request.Cookies["SSO_ID"].Value;
                    if (objSocietyRegistrationEntity.UserType != "K")
                        DT = objMenuActionModel.SocietyListWithDepartmentUser(objSocietyRegistrationEntity);
                }
                // }
            }

            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        }


        [HttpGet]
        [ActionName("GetKPIDetails")]
        public string GetSAKPIDetails(int officeId, int? pldbId, int month, string kpiFor, int DistrictId, int resionId = 0, int DivisionId = 0)
        {
            bool isLocal = Request.Url.Host.Contains("localhost");
            DataTable dt = new KPIModel().GetKPIAllDetails(officeId, pldbId, month, kpiFor, DistrictId, isLocal, resionId, DivisionId);
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpGet]
        [ActionName("GetKPIValidationDate")]
        public string GetKPIValidationDate(string kpiFor)
        {

            DataTable dt = new KPIModel().GetKPIValidationDate(kpiFor);
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }


        [HttpGet]
        [ActionName("GetUserDetailsBySSO")]
        public string GetUserDetailsBySSO()
        {
            DataTable dt = new KPIModel().GetUserDetailsBySSO();

            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpGet]
        [ActionName("GetDivisionKPIStatus")]
        public string GetDivisionKPIStatus()
        {
            DataTable dt = new KPIModel().GetDivisionKPIStatus();
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }



        [HttpGet]
        [ActionName("GetKPIPLDBWiseDetails")]
        public string GetSAKPIPLDBDetails(int? pldbId, int month, string kpiFor)
        {
            DataTable dt = new KPIModel().GetPLDBWiseAllDetails(pldbId, month, kpiFor);
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpGet]
        [ActionName("CheckPLDBAdminLock")]
        public string CheckPLDBAdminLock(int pldbId, int month, string kpiFor = "PLDB")
        {
            DataTable dt = new KPIModel().CheckPLDBAdminLock(pldbId, month, kpiFor);
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        #region Added on 18-09-2026

        [HttpPost]
        [ActionName("PacsManagerDetails")]
        public JsonResult GetDetailsByPacs(string actionType = "")
        {
            var responce = new APIResponce();
            var ds = kpiModel.GetDetailsByPacs(actionType);
            if (ds != null)
            {
                responce.Data = new
                {
                    pacsDetails = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                };
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("GetPLDBKPIResult")]
        public string GetPLDBKPIResult(int pldbId, int month, string kpiFor = "PLDB")
        {
            DataTable dt = new KPIModel().GetPLDBKPIResult(pldbId, month, kpiFor);
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpGet]
        [ActionName("GetPLDBKPIArea")]
        public string GetPLDBKPIArea()
        {
            DataTable dt = new KPIModel().GetPLDBKPIArea();
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpGet]
        [ActionName("GetPLDBKPIAreaWiseResult")]
        public string GetPLDBKPIResult(string kpiFor, int month, string fy, int kpiAreaId)
        {
            DataTable dt = new KPIModel().GetPLDBKPIResult(kpiFor, month, fy, kpiAreaId);
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        #endregion


        [HttpGet]
        [ActionName("GetUnitOfficeKPIDetails")]
        public string GetUnitOfficeKPIDetails()
        {
            DataTable dt = new KPIModel().GetUnitOfficeKPIDetails();
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpGet]
        [ActionName("GetUnitOfficeKPIArea")]
        public string GetUnitOfficeKPIArea()
        {
            DataTable dt = new KPIModel().GetUnitOfficeKPIArea();
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpGet]
        [ActionName("GetUnitOfficeKPIReport")]
        public string GetUnitOfficeKPIReport(int month, string fy, int kpiAreaId)
        {
            DataTable dt = new KPIModel().GetUnitOfficeKPIReport(month, fy, kpiAreaId);

            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }


    }
}


