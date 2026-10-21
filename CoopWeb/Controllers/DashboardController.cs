using Cooperative.Filters;
using Cooperative.Models;
using System.Web.Mvc;
using System;
using sun.security.timestamp;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace Cooperative.Controllers
{
    [LogActionFilter]
    [CustomAuthentication]
    [Error]
    public class DashboardController : BaseController
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        DashboardModel _dashboardModel = new DashboardModel(new FileLogger());


        #region New Dashboard on 13-04-2026
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult DashBoard()
        {
            //string uid = COMMON_FUNCTION.Decrypt("MDNGzk93OX3MmQW/OLEsog==");
            return View();
        }

        [HttpGet]
        [OutputCache(Duration = 70)]
        public JsonResult GetDashboardData(int FinancialYear = 0, int DistrictId = 0, int DivisionId = 0, int LevelId = 0, int ActId = 0, string CreatetionDate = "", int SocietyTypeId = 0, long UserRoleDeptMappingId = 0, int IsOnline = 0, string ActionType = "", int type = 0)
        {
            var responce = new APIResponce();
            try
            {
                var ds = _dashboardModel.GetDashboardData(FinancialYear, DistrictId, DivisionId, LevelId, ActId, CreatetionDate, SocietyTypeId, UserRoleDeptMappingId, IsOnline, ActionType, type);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<string> columns = new List<string>();
                    var result = new List<Dictionary<string, object>>();
                    var dsData = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null;
                    if (ds != null && ds.Tables.Count > 1 && ds.Tables[1] != null)
                    {
                        DataTable dt = ds.Tables[1];
                        columns = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
                        result = dt.AsEnumerable().Select(row => dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => row[col])).ToList();
                    }
                    responce.Data = new
                    {
                        countData = dsData,
                        societyDetails = new
                        {
                            columns = columns,
                            result = result
                        },
                    };
                    responce.Status = "200";
                    responce.Message = "Success";
                }
                else
                {
                    responce.Status = "404";
                    responce.Message = "Dashboard data not found.";
                }
            }
            catch (Exception)
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }

            return new JsonResult
            {
                Data = responce,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        [HttpGet]
        public JsonResult GetSocietyActioList()
        {
            var responce = new APIResponce();
            try
            {
                var ds = _dashboardModel.GetSocietyActList();
                if (ds != null && ds.Tables.Count > 0)
                {
                    responce.Data = new
                    {
                        actList = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                    };
                    responce.Status = "200";
                    responce.Message = "Success";
                }
                else
                {
                    responce.Status = "404";
                    responce.Message = "data not found.";
                }
            }
            catch (Exception)
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }

            return new JsonResult
            {
                Data = responce,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }


        [HttpGet]
        [OutputCache(Duration = 70)]
        public JsonResult GetPendencyDetails(int FinancialYear = 0, int DistrictId = 0, int DivisionId = 0, int LevelId = 0, int ActId = 0, string CreatetionDate = "", int SocietyTypeId = 0, long UserRoleDeptMappingId = 0, int IsOnline = 0, string ActionType = "", int type = 0)
        {
            var responce = new APIResponce();
            try
            {
                var ds = _dashboardModel.GetPendencyData(FinancialYear, DistrictId, DivisionId, LevelId, ActId, CreatetionDate, SocietyTypeId, UserRoleDeptMappingId, IsOnline, ActionType, type);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    //var result = dt.AsEnumerable().Select(row => dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => row[col])).ToList();
                    var columns = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
                    var result = dt.AsEnumerable().Select(row => dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => row[col])).ToList();
                    responce.Data = new
                    {
                        column = columns,
                        pendencyData = result,
                    };
                    responce.Status = "200";
                    responce.Message = "Success";
                }
                else
                {
                    responce.Status = "404";
                    responce.Message = "Dashboard data not found.";
                }
            }
            catch (Exception)
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }

            return new JsonResult
            {
                Data = responce,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        [HttpGet]
        [OutputCache(Duration = 50)]
        public JsonResult GetSocietyRegAuditorData(int FinancialYear = 0, int DivisionId = 0, int DistrictId = 0, string ActionType = "")
        {
            var responce = new APIResponce();
            try
            {
                var ds = _dashboardModel.GetSocietyRegiAuditorData(FinancialYear, DivisionId, DistrictId, ActionType);
                if (ds != null && ds.Tables.Count > 0)
                {
                    responce.Data = new
                    {
                        chartData = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                    };
                    responce.Status = "200";
                    responce.Message = "Success";
                }
                else
                {
                    responce.Status = "404";
                    responce.Message = "Dashboard data not found.";
                }
            }
            catch (Exception)
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }

            return new JsonResult { Data = responce, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }


        #endregion


    }
}
