using Cooperative.Filters;
using Cooperative.Models;
using Newtonsoft.Json;
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
    public class NcdDataController : BaseController
    {
        UserEntity _loggedInUSerDetails = null;

        NcdDataModel _nCDDataModel = new NcdDataModel(new DBLogger());
        public NcdDataController()
        {
            _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        }

        [HttpGet]
        public ActionResult NcdBasicDetails(string cooperativeId = "")
        {
            return View();
        }

        [HttpGet]
        public ActionResult NcdBasicDetailsList()
        {
            return View();
        }


        [HttpGet]
        public ActionResult NcdDasboard()
        {
            return View();
        }

        [HttpGet]
        public ActionResult NcdDistrictDasboard()
        {
            return View();
        }


        [HttpGet]
        public ActionResult cooperativeregistrationsview()
        {
            return View();
        }
        [HttpGet]
        public ActionResult NcdBasicDetailsEdit()
        {
            return View();
        }

        public string GetNcdBasicDetailsList(DataPagingModel Details, bool isExport = false)
        {
            var ds = _nCDDataModel.GetNcdBasicDetailsList(ref Details, isExport);
            if (isExport)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Columns.Contains("TotalRecords"))
                    dt.Columns.Remove("TotalRecords");


                var filename = new ExcelGenerator(new FileLogger()).GenerateExcel(ds.Tables[0], "NCDListData.xlsx");
                return filename;
            }
            return COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]);

        }



        [HttpGet]
        public JsonResult GetDistrictwithStatecodeData(string statecode)
        {
            var responce = new APIResponce();
            try
            {
                var ds = _nCDDataModel.GetDistrictwithStatecode(statecode);

                if (ds != null)
                {
                    responce.Data = new
                    {
                        NCDDistrictDetail = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                    };
                    responce.Status = "200";
                    responce.Message = "Success";
                }
                else
                {
                    responce.Status = "404";
                    responce.Message = "Districts not found.";
                }
            }
            catch (Exception ex)
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
                // Log error
            }

            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetBlockByStateDistrict(string districtcode, string statecode = null)
        {
            var responce = new APIResponce();
            try
            {
                var ds = _nCDDataModel.GetBlockByStateDistrict(statecode, districtcode);

                if (ds != null)
                {
                    responce.Data = new
                    {
                        NCDBlockDetail = ds.Tables.Count > 0
                            ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0])
                            : null,
                    };
                    responce.Status = "200";
                    responce.Message = "Success";
                }
                else
                {
                    responce.Status = "404";
                    responce.Message = "Blocks not found.";
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
        public JsonResult GetGramPanchayatByStateDistrictBlock(string statecode, string districtcode, string blockcode)
        {
            var responce = new APIResponce();
            try
            {
                var ds = _nCDDataModel.GetGramPanchayatByStateDistrictBlock(statecode, districtcode, blockcode);

                if (ds != null)
                {
                    responce.Data = new
                    {
                        NCDGramPanchayatDetail = ds.Tables.Count > 0
                            ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0])
                            : null,
                    };
                    responce.Status = "200";
                    responce.Message = "Success";
                }
                else
                {
                    responce.Status = "404";
                    responce.Message = "Gram Panchayats not found.";
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
        public JsonResult GetVillageByStateDistrictBlockGramPanchayat(string statecode, string districtcode, string blockcode, string grampanchayatcode)
        {
            var responce = new APIResponce();
            try
            {
                var ds = _nCDDataModel.GetVillageByStateDistrictBlockGramPanchayat(statecode, districtcode, blockcode, grampanchayatcode);

                if (ds != null)
                {
                    responce.Data = new
                    {
                        NCDVillageDetail = ds.Tables.Count > 0
                            ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0])
                            : null,
                    };
                    responce.Status = "200";
                    responce.Message = "Success";
                }
                else
                {
                    responce.Status = "404";
                    responce.Message = "Villages not found.";
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
        public JsonResult GetNCDAuditCategories()
        {
            var responce = new APIResponce();
            try
            {
                var ds = _nCDDataModel.GetNCDAuditCategories();

                if (ds != null)
                {
                    responce.Data = new
                    {
                        NCDAuditCategoryDetail = ds.Tables.Count > 0
                            ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0])
                            : null,
                    };
                    responce.Status = "200";
                    responce.Message = "Success";
                }
                else
                {
                    responce.Status = "404";
                    responce.Message = "Audit categories not found.";
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
        public JsonResult GetNCDCooperativeSocietyBanks(string bankTypeId)
        {
            var responce = new APIResponce();
            try
            {
                var ds = _nCDDataModel.GetNCDCooperativeSocietyBanks(bankTypeId);
                if (ds != null)
                {
                    responce.Data = new
                    {
                        NCDCoopBankDetail = ds.Tables.Count > 0
                            ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0])
                            : null,
                    };
                    responce.Status = "200";
                    responce.Message = "Success";
                }
                else
                {
                    responce.Status = "404";
                    responce.Message = "Bank list not found.";
                }
            }
            catch (Exception ex)
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
                // log exception here if needed
            }

            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetNCDRegistrationAuthorities()
        {
            var responce = new APIResponce();
            try
            {
                var ds = _nCDDataModel.GetNCDRegistrationAuthorities();

                if (ds != null)
                {
                    responce.Data = new
                    {
                        NCDRegistrationAuthorityDetail = ds.Tables.Count > 0
                            ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0])
                            : null,
                    };
                    responce.Status = "200";
                    responce.Message = "Success";
                }
                else
                {
                    responce.Status = "404";
                    responce.Message = "Registration authorities not found.";
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
                var ds = _nCDDataModel.GetSocietyFunctionalStatus();

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
        public JsonResult GetNcdDesignations()
        {
            var responce = new APIResponce();
            try
            {
                var ds = _nCDDataModel.GetNcdDesignations();

                if (ds != null)
                {
                    responce.Data = new
                    {
                        NcdDesignationDetail = ds.Tables.Count > 0
                            ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0])
                            : null,
                    };
                    responce.Status = "200";
                    responce.Message = "Success";
                }
                else
                {
                    responce.Status = "404";
                    responce.Message = "Designations not found.";
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
        public JsonResult GetFinancialYears()
        {
            var responce = new APIResponce();
            try
            {
                var ds = _nCDDataModel.GetFinancialYears();

                if (ds != null)
                {
                    responce.Data = new
                    {
                        FinancialYearDetail = ds.Tables.Count > 0
                            ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0])
                            : null,
                    };
                    responce.Status = "200";
                    responce.Message = "Success";
                }
                else
                {
                    responce.Status = "404";
                    responce.Message = "Financial years not found.";
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
        public JsonResult GetNCDDistrictDetailsoo()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            var response = new APIResponce();
            var ds = _nCDDataModel.GetNCDDistrictDetailsoo();
            if (ds != null)
            {
                response.Data = new
                {
                    NCDDistrictDetailso = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                };
                response.Status = "200";
                response.Message = "Success";
            }
            else
            {
                response.Status = "500";
                response.Message = "Internal Server Error.";
            }
            return Json(new { Result = response }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetNCDBlockDetails(string districtcode)
        {
            var response = new APIResponce();
            var ds = _nCDDataModel.GetNCDBlockDetails(districtcode);

            if (ds != null)
            {
                response.Data = new
                {
                    NCDBlockDetailso = ds.Tables.Count > 0
                        ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0])
                        : null,
                };
                response.Status = "200";
                response.Message = "Success";
            }
            else
            {
                response.Status = "500";
                response.Message = "Internal Server Error.";
            }

            return Json(new { Result = response }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetNCDGramPanchayatDetails(string districtcode, string blockcode)
        {
            var response = new APIResponce();
            var ds = _nCDDataModel.GetNCDGramPanchayatDetails(districtcode, blockcode);

            if (ds != null)
            {
                response.Data = new
                {
                    NCDGramPanchayatDetails = ds.Tables.Count > 0
                        ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0])
                        : null,
                };
                response.Status = "200";
                response.Message = "Success";
            }
            else
            {
                response.Status = "500";
                response.Message = "Internal Server Error.";
            }

            return Json(new { Result = response }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetNCDVillageDetails(string districtcode, string blockcode, string grampanchayatcode)
        {
            var response = new APIResponce();
            var ds = _nCDDataModel.GetNCDVillageDetails(districtcode, blockcode, grampanchayatcode);

            if (ds != null)
            {
                response.Data = new
                {
                    NCDVillageDetails = ds.Tables.Count > 0
                        ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0])
                        : null,
                };
                response.Status = "200";
                response.Message = "Success";
            }
            else
            {
                response.Status = "500";
                response.Message = "Internal Server Error.";
            }

            return Json(new { Result = response }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetBeeBeehiveTypes()
        {
            var responce = new APIResponce();
            try
            {
                var ds = _nCDDataModel.GetBeeBeehiveTypes();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    responce.Data = new
                    {
                        BeeBeehiveTypes = COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0])
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


        public JsonResult GetFacilities(string type)
        {
            var response = new APIResponce();
            try
            {
                string action = string.Empty;

                if (type == "80")
                    action = "GetFacilities2";
                else if (type == "PrimaryHousing")
                    action = "GetFacilitiesFORPriamryHousing"; // your new case
                else
                    action = "GetFacilities";

                var ds = _nCDDataModel.GetFacilities(action);

                if (ds != null && ds.Tables.Count > 0)
                {
                    response.Data = new
                    {
                        Facilities = COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0])
                    };
                    response.Status = "200";
                    response.Message = "Success";
                }
                else
                {
                    response.Status = "404";
                    response.Message = "No records found.";
                }
            }
            catch
            {
                response.Status = "500";
                response.Message = "Internal Server Error.";
            }

            return Json(new { Result = response }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetNcdOfficeBuildingTypes()
        {
            var response = new APIResponce();
            try
            {
                var ds = _nCDDataModel.GetNcdOfficeBuildingTypes();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    response.Data = new
                    {
                        NCDOfficeBuildingTypes = COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0])
                    };
                    response.Status = "200";
                    response.Message = "Success";
                }
                else
                {
                    response.Status = "404";
                    response.Message = "No records found.";
                }
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Internal Server Error.";
            }

            return Json(new { Result = response }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAreaOperationDetails(int operation_area_location)
        {
            var response = new APIResponce();
            var ds = _nCDDataModel.GetAreaOperationDetails(operation_area_location);

            if (ds != null)
            {
                response.Data = new
                {
                    OperationDetails = ds.Tables.Count > 0
                        ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0])
                        : null,
                };
                response.Status = "200";
                response.Message = "Success";
            }
            else
            {
                response.Status = "500";
                response.Message = "Internal Server Error.";
            }

            return Json(new { Result = response }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetSectorDetailsByIsCredit(string actionName, int? sectorId)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetSectorDetailsByIsCredit(actionName, sectorId);
            if (ds != null)
            {
                responce.Data = new
                {
                    NCDSectorDetail = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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


        //Insert Data start from Here

        [HttpPost]
        public JsonResult SaveNcdData(string ncdJson, string cooperative_id = "")
        {
            string cooperativeId = string.Empty;
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            var response = new APIResponce();
            try
            {
                var ds = _nCDDataModel.SaveNCDData(ncdJson, Convert.ToInt32(_LoggedInUser.UserId), cooperative_id);
                if (ds != null)
                {
                    string result_ncd_id = "";
                    try
                    {
                        result_ncd_id = ds.Tables.Count > 0 ? ds.Tables[0].Rows[0]["ncdId"].ToString() : null;
                        NCDServiceModel nCDService = new NCDServiceModel(new DBLogger());

                        if (result_ncd_id.Length > 10)
                        {
                            if (!Request.Url.Host.Contains("localhost"))
                            {
                                var ncd_response = nCDService.NcdSaveCooperativeData(result_ncd_id);
                                COMMON_FUNCTION.Sp_callTrackingInsert("ncd_response", result_ncd_id + "=" + JsonConvert.SerializeObject(ncd_response));

                            }
                        }
                    }
                    catch (Exception exncd)
                    {

                        ILogger objILogger = new DBLogger();
                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.NcdDataController", FunctionName = "NcdData-NcdSaveCooperativeData=" + result_ncd_id }, exncd);
                    }
                    response.Data = new
                    {
                        ncdData = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                    };
                    response.Status = "200";
                    response.Message = "Success";
                }
                else
                { response.Status = "500"; response.Message = "Save returned no id."; }
            }
            catch (Exception ex)
            { response.Status = "500"; response.Message = ex.Message; }
            return Json(new { Result = response });
        }

        [HttpGet]
        [OutputCache(Duration = 60)]
        public JsonResult GetSectorDashboard(string fromDate = null, string toDate = null, int districtId = 0, int functionalStatusId = 0)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            var responce = new APIResponce();
            try
            {
                var ds = _nCDDataModel.GetSectorDashboard(fromDate, toDate, districtId, functionalStatusId);
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
        [OutputCache(Duration = 60)]
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
        public JsonResult GetWaterBodyTypeDetails()
        {
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetWaterBodyTypeDetail();
            if (ds != null)
            {
                responce.Data = new
                {
                    WaterBodyDetails = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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
        public JsonResult GetCooperativeDataByRegistrationNo(string RegistrationNo)
        {
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetCooperativeDataByRegistrationNo(RegistrationNo);
            if (ds != null)
            {
                responce.Data = new
                {
                    BasicDetails = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                    MemberDetails = ds.Tables.Count > 1 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[1]) : null,
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
        public JsonResult GetUrbanCategoryLocalBody()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetUrbanCategoryDetails();
            if (ds != null)
            {
                responce.Data = new
                {
                    urbanCategoryData = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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
        public JsonResult GetUrbanLocalBodyByTypecode(int? localbody_type_code)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetUrbanLocalBodyByTypecode(localbody_type_code);
            if (ds != null)
            {
                responce.Data = new
                {
                    urbanBodyTypeDetail = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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
        public JsonResult GetWardDetailsByCode(int? local_body_code)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetWardDetailsByCode(local_body_code);
            if (ds != null)
            {
                responce.Data = new
                {
                    WardDetails = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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

        public JsonResult GetDashboardData(string action, int? stateId = null, int? districtId = null, int? functionalStatusId = null, string fromDate = null, string toDate = null)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            var responce = new APIResponce();

            try
            {
                var ds = _nCDDataModel.GetDashboard(action, stateId, districtId, functionalStatusId, fromDate, toDate);

                if (ds != null)
                {
                    responce.Data = new
                    {
                        dashboardData = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null
                    };
                    responce.Status = "200";
                    responce.Message = "Success";
                }
                else
                {
                    responce.Status = "500";
                    responce.Message = "Internal Server Error.";
                }
            }
            catch (Exception ex)
            {
                responce.Status = "500";
                responce.Message = ex.Message;
            }

            return Json(new { Result = responce }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSocietyMemberDashboard(string fromDate = null, string toDate = null, int districtId = 0, int functionalStatusId = 0)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            var responce = new APIResponce();

            try
            {
                var ds = _nCDDataModel.GetSocietyMemberDashboard(fromDate, toDate, districtId, functionalStatusId);

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
        public JsonResult GetSocietyFacilityByPrimaryActivity(int? primaryId)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetSocietyFacilityByPrimaryActivity(primaryId);
            if (ds != null)
            {
                responce.Data = new
                {
                    facilitiesDetails = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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
        public JsonResult GetTypeOfWomenCooperative(int? primaryId)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetTypeOfWomenCooperative(primaryId);
            if (ds != null)
            {
                responce.Data = new
                {
                    womenCoopDetails = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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
        public JsonResult GetSecondryActivityBySectorOperation(int? sectorOperation)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetSecondryActivityBySectorOperation(sectorOperation);
            if (ds != null)
            {
                responce.Data = new
                {
                    sectorOperationDetails = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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
        public JsonResult GetTransportVehicleOwenership(int? sectorOperation)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetTransportVehicleOwenership(sectorOperation);
            if (ds != null)
            {
                responce.Data = new
                {
                    vehicleOwenership = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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
        public JsonResult GetKhadiGramSocietyActivity()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetKhadiGramSocietyActivity();
            if (ds != null)
            {
                responce.Data = new
                {
                    KhadiGramSocietyActivityDetails = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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
        public JsonResult GetNcdDetailsById(string cooperative_id = null, string ncd_id = null, string cooperative_society_name = null)
        {
            var response = new APIResponce();

            try
            {
                DataSet ds = _nCDDataModel.GetNcdDetailsById(cooperative_id, ncd_id, cooperative_society_name);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    response.Data = new
                    {
                        NCDEditdata = COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0])
                    };
                    response.Status = "200";
                    response.Message = "Success";
                }
                else
                {
                    response.Status = "404";
                    response.Message = "No records found.";
                }
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Internal Server Error.";
            }

            return Json(new { Result = response }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDetailsByCooperative(string cooperative = "")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetDetailsByCooperative(cooperative);
            if (ds != null)
            {
                responce.Data = new
                {
                    cooperativeData = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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
        public JsonResult GetNcdCooperativeData(string ncdId)
        {
            var responseData = new NCDRootResponse();
            try
            {
                responseData = _nCDDataModel.GetNcdCooperativeData(ncdId);
                return new JsonResult
                {
                    Data = responseData,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch (Exception ex)
            {
                responseData.status = "500";
            }
            return new JsonResult
            {
                Data = responseData,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        [HttpGet]
        public JsonResult GetNcdBasicDetailsView(string eid = null, int? FunctionalStatus = null, int? sectorOperation = null, string cooperative_society_name = null, string registrationNo = null)
        {
            try
            {
                //id,
                DataSet dsa = _nCDDataModel.GetNcdBasicDetailsView(eid, FunctionalStatus, sectorOperation, cooperative_society_name, registrationNo);

                List<NcdCooperativeDetail> list = new List<NcdCooperativeDetail>();

                // prepare default values for member details
                string m_general = null;
                string f_general = null;
                string t_general = null;
                string fsc = null;
                string f_obc = null;
                string m_obc = null;
                string msc = null;
                string fst = null;
                string tsc = null;
                string tst = null;
                string t_obc = null;
                string mst = null;

                if (dsa != null && dsa.Tables.Count > 1 && dsa.Tables[1].Rows.Count > 0)
                {
                    m_general = Convert.ToString(dsa.Tables[1].Rows[0]["m_general"]);
                    f_general = Convert.ToString(dsa.Tables[1].Rows[0]["f_general"]);
                    t_general = Convert.ToString(dsa.Tables[1].Rows[0]["t_general"]);
                    fsc = Convert.ToString(dsa.Tables[1].Rows[0]["fsc"]);
                    f_obc = Convert.ToString(dsa.Tables[1].Rows[0]["f_obc"]);
                    m_obc = Convert.ToString(dsa.Tables[1].Rows[0]["m_obc"]);
                    msc = Convert.ToString(dsa.Tables[1].Rows[0]["msc"]);
                    fst = Convert.ToString(dsa.Tables[1].Rows[0]["fst"]);
                    tsc = Convert.ToString(dsa.Tables[1].Rows[0]["tsc"]);
                    tst = Convert.ToString(dsa.Tables[1].Rows[0]["tst"]);
                    t_obc = Convert.ToString(dsa.Tables[1].Rows[0]["t_obc"]);
                    mst = Convert.ToString(dsa.Tables[1].Rows[0]["mst"]);
                }

                //if (dsa != null && dsa.Tables.Count > 0)
                if (dsa != null && dsa.Tables.Count > 0 && dsa.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dsa.Tables[0].Rows)
                    {
                        list.Add(new NcdCooperativeDetail
                        {
                            ncd_coop_reg_id = Convert.ToInt32(row["ncd_coop_reg_id"]),
                            cooperative_society_name = Convert.ToString(row["cooperative_society_name"]),
                            StateName = Convert.ToString(row["StateName"]),
                            DistrictName = Convert.ToString(row["DistrictName"]),
                            BlockName = Convert.ToString(row["BlockName"]),
                            //VillageName = Convert.ToString(row["VillageName"]),
                            GPName = Convert.ToString(row["GPName"]),
                            fulladdress = Convert.ToString(row["fulladdress"]),

                            registration_number = Convert.ToString(row["registration_number"]),
                            cooperative_id = Convert.ToString(row["cooperative_id"]),
                            reference_year = Convert.ToString(row["reference_year"]),
                            audityears = Convert.ToString(row["audityears"]),
                            contact_person = Convert.ToString(row["contact_person"]),
                            designation = Convert.ToString(row["designation"]),
                            Pincode = Convert.ToString(row["Pincode"]),

                            Mobile = Convert.ToString(row["Mobile"]),
                            landline = Convert.ToString(row["landline"]),
                            email = Convert.ToString(row["email"]),
                            sector_of_operation_Type = Convert.ToString(row["sector_of_operation_Type"]),
                            Sectorname = Convert.ToString(row["Sectorname"]),
                            IsCredit = Convert.ToString(row["IsCredit"]),
                            authorityname = Convert.ToString(row["authorityname"]),
                            LocationArea = Convert.ToString(row["LocationArea"]),
                            UrbanRuralArea = Convert.ToString(row["UrbanRuralArea"]),
                            waterbody = Convert.ToString(row["waterbody"]),
                            NumberofmemberofSociety = Convert.ToString(row["NumberofmemberofSociety"]),
                            Profitmaking = Convert.ToString(row["Profitmaking"]),
                            netloss = Convert.ToString(row["netloss"]),
                            isdevidentpaid = Convert.ToString(row["isdevidentpaid"]),
                            msme = Convert.ToString(row["msme"]),
                            banktype = Convert.ToString(row["banktype"]),
                            bankname = Convert.ToString(row["bankname"]),
                            affilated = Convert.ToString(row["affilated"]),
                            functionstatus = Convert.ToString(row["functionstatus"]),
                            date_registration = Convert.ToString(row["date_registration"]),
                            //registration_authoritie_id = Convert.ToString(row["registration_authoritie_id"]),

                            m_general = m_general,
                            f_general = f_general,
                            t_general = t_general,
                            fsc = fsc,
                            f_obc = f_obc,
                            m_obc = m_obc,
                            msc = msc,
                            fst = fst,
                            tsc = tsc,
                            tst = tst,
                            t_obc = t_obc,
                            mst = mst,
                        });
                    }
                }

                return Json(new
                {
                    Result = new { Status = "200", Data = new { NcdListData = list } }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = new { Status = "500", Message = ex.Message } }, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpGet]
        public JsonResult GetTypeofBeesCooperative()
        {
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetTypeofBeesCooperative();
            if (ds != null)
            {
                responce.Data = new
                {
                    BeesCoopDetails = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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
        public JsonResult GetLivestockProductProduce()
        {
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetLivestockProductProduce();
            if (ds != null)
            {
                responce.Data = new
                {
                    LivestockProduceDetails = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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
        public JsonResult GetJuteProductProduce()
        {
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetJuteProductProduce();
            if (ds != null)
            {
                responce.Data = new
                {
                    juteProduceDetails = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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
        public JsonResult GetJuteRawMeterialDetail()
        {
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetJuteRawMeterial();
            if (ds != null)
            {
                responce.Data = new
                {
                    juteRawMeterialDetail = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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
        public JsonResult GetTypeSocialCulturalActivities()
        {
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetTypeSocialCulturalActivitiesDetail();
            if (ds != null)
            {
                responce.Data = new
                {
                    typeSociaActivitiesDetails = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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
        public JsonResult GetHandicraftRawMeterialDetail()
        {
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetHandicraftRawMeterial();
            if (ds != null)
            {
                responce.Data = new
                {
                    handicraftRawMeterialDetail = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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
        public JsonResult GetHandicraftProductProduced()
        {
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetHandicraftProductProduced();
            if (ds != null)
            {
                responce.Data = new
                {
                    handicraftProductDetail = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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
        public JsonResult GetAgricultureFarmingMechanisms()
        {
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetAgricultureFarmingMechanisms();
            if (ds != null)
            {
                responce.Data = new
                {
                    FarmingMechanismDetail = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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
        public JsonResult GetEducationCourseOfferedBySector(int sectorId)
        {
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetEducationCourseOfferedBySector(sectorId);
            if (ds != null)
            {
                responce.Data = new
                {
                    EducationCourseDatail = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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
        public JsonResult GetSugarProductProduce()
        {
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetSugarProductProduce();
            if (ds != null)
            {
                responce.Data = new
                {
                    sugarProduceDetails = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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
        public JsonResult GetBeeFarmingProductProduced()
        {
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetBeeFarmingProductProduced();
            if (ds != null)
            {
                responce.Data = new
                {
                    beeFarmingProductDetail = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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
        public JsonResult GetAffiliatedUnionFeederationDetails()
        {
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetAffiliatedUnionFeederationDetail();
            if (ds != null)
            {
                responce.Data = new
                {
                    affiliatedFeederationDetails = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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


        #region Financial Report on 24-12-2025
        public ActionResult FinancialTrunOverReport()
        {
            return View();
        }


        [HttpPost]
        public string GetTrunoverProfitLoseDetails(int district_code = 0, string actionType = "", bool isExport = false)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetTrunoverProfitLossDetail(district_code, actionType, isExport);
            if (isExport)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                dt = ds.Tables[0];
                if (dt.Columns.Contains("TotalRecords"))
                    dt.Columns.Remove("TotalRecords");

                var filename = new ExcelGenerator(new FileLogger()).GenerateExcel(ds.Tables[0], "FinancialTrunOverReport.xlsx");
                return filename;
            }
            return COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]);
        }

        public ActionResult Financialdetails(string societyname = "", int location = 0, int district_code = 0, int sectorcode = 0, int financial_update = 0)
        {
            return View();
        }

        [HttpPost]
        public string GetFinancialTrunoverProfitLossDetail(DataPagingModel Details, string societyname = "", int location = 0, int district_code = 0, int sectorcode = 0, string actionType = "", bool isExport = false)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetFinancialTrunoverProfitLossDetail(ref Details, societyname, location, district_code, sectorcode, actionType, isExport);
            if (isExport)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Columns.Contains("TotalRecords"))
                    dt.Columns.Remove("TotalRecords");


                var filename = new ExcelGenerator(new FileLogger()).GenerateExcel(ds.Tables[0], "FinancialTrunoverProfitLossReport.xlsx");
                return filename;
            }
            return COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]);
        }

        public ActionResult NCDBoardOfDirector(string cooperativeId = "", int districtcode = 0, int sectorcode = 0)
        {
            return View();
        }


        [HttpPost]
        public JsonResult SaveDirectorData(string jsonData)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            var response = new APIResponce();
            try
            {
                CooperativeBoardofDirector director = JsonConvert.DeserializeObject<CooperativeBoardofDirector>(jsonData);
                var ds = _nCDDataModel.SaveCooperativeBoardDirector(director);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var row = ds.Tables[0].Rows[0];
                    int result = Convert.ToInt32(row["Result"]);
                    string message = row["Message"].ToString();
                    int status = Convert.ToInt32(row["Status"]);

                    response.Data = new
                    {
                        directorData = COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]),
                        result = result
                    };

                    response.Status = status.ToString();
                    response.Message = message;
                }
                else
                {
                    response.Status = "500";
                    response.Message = "No data returned from database.";
                }
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = ex.Message;
            }

            return Json(new { Result = response });
        }

        [HttpPost]
        public JsonResult GetBoardofDirectorByCooperativeId(string cooperativeId = "")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetBoardofDirectorByCooperativeId(cooperativeId);
            if (ds != null)
            {
                responce.Data = new
                {
                    directorDetails = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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
        public JsonResult GetCommonProductProduceBySociety(int sector_of_operation = 0)
        {
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetCommonProductProduceBySector(sector_of_operation);
            if (ds != null)
            {
                responce.Data = new
                {
                    produceDetails = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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
        public ActionResult NCDDistrictWiseReport()
        {
            return View();
        }


        [HttpGet]
        public JsonResult GetDistrictWiseNCDRegistrationCount(int type = 0, string actionType = "")
        {
            var responce = new APIResponce();
            var ds = _nCDDataModel.GetDistrictWiseNCDRegistrationCount(type, actionType);
            if (ds != null)
            {
                responce.Data = new
                {
                    ncdDataReport = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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
        #endregion
    }

}