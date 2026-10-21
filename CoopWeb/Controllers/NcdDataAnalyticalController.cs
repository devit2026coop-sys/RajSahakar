using Cooperative.Filters;
using Cooperative.Models;
using java.security.cert;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using static Cooperative.Models.NCDApiModel;

namespace Cooperative.Controllers
{
    [Error]
    [CustomAuthentication]
    [LogActionFilter]
    public class NcdDataAnalyticalController : BaseController
    {
        UserEntity _loggedInUSerDetails = null;

        NcdDataModel _nCDDataModel = new NcdDataModel(new DBLogger());

        NcdDataAnalyticalModel _nCDAnayticalDataModel = new NcdDataAnalyticalModel(new DBLogger());
        public NcdDataAnalyticalController()
        {
            _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        }

        #region 12/12/2025 BHAVESH
        [HttpGet]
        public ActionResult NcdAnalyticalDashboard()
        {
            return View();
        }
        #endregion

        [HttpGet]
        public JsonResult GetSectorDashboard(string fromDate = null, string toDate = null, int? districtId = null, int? functionalStatusId = null)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            var responce = new APIResponce();


            try
            {
                var ds = _nCDAnayticalDataModel.GetSectorDashboard(fromDate, toDate, districtId, functionalStatusId);

                if (ds != null && ds.Tables.Count > 0)
                {
                    responce.Data = new
                    {
                        SectorDashboard = COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0])
                    };
                    responce.Status = "200";
                    responce.Message = "Success";
                }
                else
                {
                    responce.Status = "404";
                    responce.Message = "No records found.";
                }
            }
            catch (Exception ex)
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }

            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetDistrictDashboard(string fromDate = null, string toDate = null, int districtId = 0, int functionalStatusId = 0)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            var responce = new APIResponce();

            try
            {
                var ds = _nCDDataModel.GetDistrictDashboard(fromDate, toDate, districtId, functionalStatusId);

                if (ds != null && ds.Tables.Count > 0)
                {
                    responce.Data = new
                    {
                        DistrictDashboard = COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0])
                    };
                    responce.Status = "200";
                    responce.Message = "Success";
                }
                else
                {
                    responce.Status = "404";
                    responce.Message = "No records found.";
                }
            }
            catch (Exception ex)
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }

            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSocietyMemberDashboard(string fromDate = null, string toDate = null, int? districtId = null, int? functionalStatusId = null)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            var responce = new APIResponce();

            try
            {
                var ds = _nCDAnayticalDataModel.GetSocietyMemberDashboard(fromDate, toDate, districtId, functionalStatusId);

                if (ds != null && ds.Tables.Count > 0)
                {
                    responce.Data = new
                    {
                        SectorWiseDistrictDashboard = COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0])
                    };
                    responce.Status = "200";
                    responce.Message = "Success";
                }
                else
                {
                    responce.Status = "404";
                    responce.Message = "No records found.";
                }
            }
            catch (Exception ex)
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }

            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSocietyFunctionalStatus()
        {
            var responce = new APIResponce();
            try
            {
                var ds = _nCDAnayticalDataModel.GetSocietyFunctionalStatus();

                if (ds != null)
                {
                    responce.Data = new
                    {
                        SocietyFunctionalStatusDetail = ds.Tables.Count > 0
                            ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0])
                            : null,
                    };
                    responce.Status = "200";
                    responce.Message = "Success";
                }
                else
                {
                    responce.Status = "404";
                    responce.Message = "Functional status not found.";
                }
            }
            catch (Exception ex)
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
                // log exception
            }

            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetBlockCoverageDashboard()
        {
            var response = new APIResponce();

            try
            {
                var ds = _nCDAnayticalDataModel.GetBlockCoverageDashboard();

                int fully = 0, partial = 0, notCovered = 0;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string status = row["CoverageStatus"].ToString();
                    int count = Convert.ToInt32(row["TotalGramPanchayat"]);

                    if (status == "Fully Covered") fully = count;
                    else if (status == "Partially Covered") partial = count;
                    else if (status == "Not Covered") notCovered = count;
                }

                response.Data = new
                {
                    Fully = fully,
                    Partial = partial,
                    Not = notCovered,
                    TotalGP = fully + partial + notCovered
                };

                response.Status = "200";
            }
            catch
            {
                response.Status = "500";
            }

            return Json(new { Result = response }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetRegisteredSocietiesTrend()
        {
            var responce = new APIResponce();
            try
            {
                var ds = _nCDAnayticalDataModel.GetRegisteredSocietiesTrend();

                if (ds != null)
                {
                    responce.Data = new
                    {
                        TrendData = ds.Tables.Count > 0
                            ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0])
                            : null,
                    };
                    responce.Status = "200";
                    responce.Message = "Success";
                }
                else
                {
                    responce.Status = "404";
                    responce.Message = "Functional status not found.";
                }
            }
            catch (Exception ex)
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
                // log exception
            }

            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetFunctionalStatusTrend()
        {
            var responce = new APIResponce();
            try
            {
                var ds = _nCDAnayticalDataModel.GetFunctionalStatusTrend();

                if (ds != null)
                {
                    responce.Data = new
                    {
                        TrendData = ds.Tables.Count > 0
                            ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0])
                            : null,
                    };
                    responce.Status = "200";
                    responce.Message = "Success";
                }
                else
                {
                    responce.Status = "404";
                    responce.Message = "Functional status not found.";
                }
            }
            catch (Exception ex)
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
                // log exception
            }

            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetFunctionalStatusTrendDairy()
        {
            var responce = new APIResponce();
            try
            {
                var ds = _nCDAnayticalDataModel.GetFunctionalStatusTrendDairy();

                if (ds != null)
                {
                    responce.Data = new
                    {
                        TrendData = ds.Tables.Count > 0
                            ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0])
                            : null,
                    };
                    responce.Status = "200";
                    responce.Message = "Success";
                }
                else
                {
                    responce.Status = "404";
                    responce.Message = "Functional status not found.";
                }
            }
            catch (Exception ex)
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
                // log exception
            }

            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetFunctionalFisheryStatusTrend()
        {
            var responce = new APIResponce();
            try
            {
                var ds = _nCDAnayticalDataModel.GetFunctionalFisheryStatusTrend();

                if (ds != null)
                {
                    responce.Data = new
                    {
                        TrendData = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                    };
                    responce.Status = "200";
                    responce.Message = "Success";
                }
                else
                {
                    responce.Status = "404";
                    responce.Message = "Functional status not found.";
                }
            }
            catch (Exception ex)
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
                // log exception
            }

            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }



    }

}