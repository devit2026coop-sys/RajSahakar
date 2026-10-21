using Cooperative.DBConnector;
using Cooperative.Filters;
using Cooperative.Models;
using java.util;
using Newtonsoft.Json;
using sun.security.timestamp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.Web.Mvc;
using static Cooperative.Models.NCDApiModel;
using static Cooperative.Models.NCDServiceModel;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Cooperative.Controllers
{
    public class NCDServiceController : BaseController
    {
        NCDServiceModel _nCDServiceModel = new NCDServiceModel(new DBLogger());
        public ActionResult NCD_Service_Report()
        {
            return View();
        }

        [CustomAuthentication]
        public ActionResult NcdServiceDataSync()
        {
            return View();
        }


        [HttpGet]
        public async Task<JsonResult> GetNCDStateData()
        {
            NCDStateApiResponse stateResponseData = new NCDStateApiResponse();
            try
            {
                stateResponseData = await _nCDServiceModel.GetStateDetails();
                if (stateResponseData != null && stateResponseData.status == "Success")
                {
                    if (stateResponseData.data != null)
                    {
                        var result = _nCDServiceModel.SaveStateDetails(stateResponseData.data);
                        stateResponseData.status = "200";
                        stateResponseData.message = "State data saved successfully.";
                    }
                }
            }
            catch (Exception ex)
            {
                stateResponseData.status = "500";
                stateResponseData.ErrorMessage = "Internal Server Error: " + ex.Message;
            }
            return Json(stateResponseData, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetNCDStateDetails()
        {
            var responce = new APIResponce();
            var ds = _nCDServiceModel.GetNCDStateDetails();
            if (ds != null)
            {
                responce.Data = new
                {
                    NCDStateDetail = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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

        /// <summary>
        /// GetNCDSectorDetails sector code
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public JsonResult GetNCDSectorDetails()
        {
            var responce = new APIResponce();
            var ds = _nCDServiceModel.GetNCDSectorDetails();
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


        [HttpGet]
        public async Task<JsonResult> GetNcdDistrictData()
        {
            NCDDistrictApiResponse districtResponseData = new NCDDistrictApiResponse();
            try
            {
                districtResponseData = await _nCDServiceModel.GetDistrictDetails();

                if (districtResponseData != null && districtResponseData.status == "Success")
                {
                    if (districtResponseData.result != null)
                    {
                        var result = _nCDServiceModel.SaveDistrictDetails(districtResponseData);
                        districtResponseData.status = "200";
                        districtResponseData.message = "District data saved successfully.";
                    }
                    else
                    {
                        districtResponseData.status = "204";
                        districtResponseData.message = "No district data found.";
                    }
                }
                else
                {
                    districtResponseData.status = "400";
                    districtResponseData.ErrorMessage = "API returned failure.";
                }
            }
            catch (Exception ex)
            {
                districtResponseData.status = "500";
                districtResponseData.ErrorMessage = "Internal Server Error: " + ex.Message;
            }
            return Json(districtResponseData, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public async Task<JsonResult> GetNcdBlockDetail()
        {
            NCDBlocksApiResponse blockResponseData = new NCDBlocksApiResponse();
            try
            {
                blockResponseData = await _nCDServiceModel.GetNcdBlockDetails();

                if (blockResponseData != null && blockResponseData.status == "success")
                {
                    if (blockResponseData.data != null && blockResponseData.data.blocks.Count > 0)
                    {
                        var result = _nCDServiceModel.SaveNcdBlockDetails(blockResponseData.data.blocks);
                        blockResponseData.status = "200";
                        blockResponseData.message = "Block data saved successfully.";
                    }
                    else
                    {
                        blockResponseData.status = "204";
                        blockResponseData.message = "No block data found.";
                    }
                }
                else
                {
                    blockResponseData.status = "400";
                    blockResponseData.ErrorMessage = "API returned failure.";
                }
            }
            catch (Exception ex)
            {
                blockResponseData.status = "500";
                blockResponseData.ErrorMessage = "Internal Server Error: " + ex.Message;
            }
            return Json(blockResponseData, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public async Task<JsonResult> GetNcdWardDetails()
        {
            NCDWardApiResponse wardResponseData = new NCDWardApiResponse();
            try
            {
                wardResponseData = await _nCDServiceModel.GetNcdWardDetails();

                if (wardResponseData != null && wardResponseData.status == "Success")
                {
                    if (wardResponseData.result != null)
                    {
                        var result = _nCDServiceModel.SaveNcdWardDetails(wardResponseData.result);
                        wardResponseData.status = "200";
                        wardResponseData.message = "Ward data saved successfully.";
                    }
                    else
                    {
                        wardResponseData.status = "204";
                        wardResponseData.message = "No ward data found.";
                    }
                }
                else
                {
                    wardResponseData.status = "400";
                    wardResponseData.ErrorMessage = "API returned failure.";
                }
            }
            catch (Exception ex)
            {
                wardResponseData.status = "500";
                wardResponseData.ErrorMessage = "Internal Server Error: " + ex.Message;
            }
            return new JsonResult
            {
                Data = wardResponseData,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }


        [HttpGet]
        public async Task<JsonResult> GetNcdVillageDetail()
        {
            NCDVillagesAPIResponse villagesResponseData = new NCDVillagesAPIResponse();
            try
            {
                villagesResponseData = await _nCDServiceModel.GetNcdVillageDetails();

                if (villagesResponseData != null && villagesResponseData.status == "Success")
                {
                    if (villagesResponseData.result != null)
                    {
                        var result = _nCDServiceModel.SaveVillageDetails(villagesResponseData.result);
                        villagesResponseData.status = "200";
                        villagesResponseData.message = "Village data saved successfully.";
                    }
                    else
                    {
                        villagesResponseData.status = "204";
                        villagesResponseData.message = "No village data found.";
                    }
                }
                else
                {
                    villagesResponseData.status = "400";
                    villagesResponseData.ErrorMessage = "API returned failure.";
                }
            }
            catch (Exception ex)
            {
                villagesResponseData.status = "500";
                villagesResponseData.ErrorMessage = "Internal Server Error: " + ex.Message;
            }
            return new JsonResult { Data = villagesResponseData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }


        [HttpGet]
        public async Task<JsonResult> GetNcdCitiesDetails()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            NCDCitiesAPIResponse citiesResponseData = new NCDCitiesAPIResponse();
            try
            {
                citiesResponseData = await _nCDServiceModel.GetNcdCitiesDetails();

                if (citiesResponseData != null && citiesResponseData.status == "Success")
                {
                    if (citiesResponseData.result != null)
                    {
                        var result = _nCDServiceModel.SaveCitiesDetails(citiesResponseData.result);
                        citiesResponseData.status = "200";
                        citiesResponseData.message = "Cities data saved successfully.";
                    }
                    else
                    {
                        citiesResponseData.status = "204";
                        citiesResponseData.message = "No cities data found.";
                    }
                }
                else
                {
                    citiesResponseData.status = "400";
                    citiesResponseData.ErrorMessage = "API returned failure.";
                }
            }
            catch (Exception ex)
            {
                citiesResponseData.status = "500";
                citiesResponseData.ErrorMessage = "Internal Server Error: " + ex.Message;
            }
            return new JsonResult { Data = citiesResponseData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }

        [HttpGet]
        public async Task<JsonResult> GetNcdSectorDetail()
        {
            NCDSectorAPIResponse sectorResponseData = new NCDSectorAPIResponse();
            try
            {
                sectorResponseData = await _nCDServiceModel.GetNcdSectorDetail();
                if (sectorResponseData != null && sectorResponseData.status == "Success")
                {
                    if (sectorResponseData.data != null)
                    {
                        var result = _nCDServiceModel.SaveSectorDetails(sectorResponseData.data);
                        sectorResponseData.status = "200";
                        sectorResponseData.message = "Sector data saved successfully.";
                    }
                    else
                    {
                        sectorResponseData.status = "204";
                        sectorResponseData.message = "No sector data found.";
                    }
                }
                else
                {
                    sectorResponseData.status = "400";
                    sectorResponseData.message = "API returned failure.";
                }
            }
            catch (Exception ex)
            {
                sectorResponseData.status = "500";
                sectorResponseData.message = "Internal Server Error: " + ex.Message;
            }
            return new JsonResult { Data = sectorResponseData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }

        [HttpGet]
        public async Task<JsonResult> GetLevelCodeMasterData(int levelcode = 0, int page = 1)
        {
            NCDLevelCodeCommonResponse levelCodeResponse = null;
            try
            {
                levelCodeResponse = await _nCDServiceModel.GetLevelCodeMasterData(levelcode, page);
                if (levelCodeResponse != null)
                { levelCodeResponse.status = "200"; levelCodeResponse.message = $"Data for level code {levelcode} imported successfully."; }
                else
                { levelCodeResponse = new NCDLevelCodeCommonResponse(); levelCodeResponse.status = "404"; levelCodeResponse.message = $"No data found for level code {levelcode}."; }
            }
            catch (Exception ex)
            {
                levelCodeResponse.status = "500";
                levelCodeResponse.ErrorMessage = "Internal Server Error: " + ex.Message;
            }
            return Json(levelCodeResponse, JsonRequestBehavior.AllowGet);
        }


        #region Cooperative Societies Registrations 23-06-2025

        [HttpGet]
        public async Task<JsonResult> GetStateWiseCooperativeRegistrationDetail()
        {
            int result = 0;
            NCDCooperativeRegistrationAPIResponse responseData = new NCDCooperativeRegistrationAPIResponse();
            try
            {
                responseData = await _nCDServiceModel.GetCooperativeRegistrationDetail();
                if (responseData != null && responseData.status == "Success")
                {
                    if (responseData.result != null)
                    {
                        result = _nCDServiceModel.SaveCooperativeRegistrations(responseData.result);
                        if (result > 0) { responseData.status = "200"; responseData.message = "Cooperative registration details data saved successfully."; }
                        else { responseData.status = "201"; responseData.message = "Data not saved."; }
                    }
                    else { responseData.status = "204"; responseData.message = "No cooperative registration detals data found."; }
                }
                else
                {
                    responseData.status = "400";
                    responseData.message = "API returned failure.";
                }
            }
            catch (Exception ex)
            {
                responseData.status = "500";
                responseData.ErrorMessage = "Internal Server Error: " + ex.Message;
            }
            return new JsonResult { Data = responseData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }

        [HttpGet]
        public JsonResult NcdSyncCooperativeData(NCDApiRequestModel reqModel)
        {
            var responseData = new NCDCommonResponse();
            try
            {
                responseData = _nCDServiceModel.NcdSaveCooperativeData(reqModel.cooperative_id);
                return new JsonResult
                {
                    Data = responseData,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch (Exception ex)
            {
                responseData.status = "500";
                responseData.message = "Internal Server Error: " + ex.Message;
            }
            return new JsonResult { Data = responseData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #endregion


        #region 2025-08-27 @Pradeep


        [HttpGet]
        public async Task<JsonResult> GetMemberDetailsByStatecode(int levelcode = 0)
        {
            var response = new NcdApiCommonResponse<MemberDetailsResultItem>();
            try
            {
                response = await _nCDServiceModel.GetMemberDetailByStatecode(levelcode);
                if (response != null && response.status == "Success")
                {
                    if (response.result != null)
                    {
                        var result = _nCDServiceModel.SaveMemberDetails(response.result);
                        response.status = "200";
                        response.message = "Member details saved successfully.";
                    }
                    else
                    {
                        response.status = "204";
                        response.message = "No member details detail data found.";
                    }
                }
                else
                {
                    response.status = "400";
                    response.ErrorMessage = "API returned failure.";
                }
            }
            catch (Exception ex)
            {
                response.status = "500";
                response.ErrorMessage = "Internal Server Error: " + ex.Message;
            }
            return new JsonResult { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }


        [HttpGet]
        public async Task<JsonResult> GetAreaofOperationRuralDetail(int page = 1, int limit = 100000)
        {
            var response = new NcdApiCommonResponse<AreaofOperationRuralResultItem>();
            try
            {
                response = await _nCDServiceModel.GetAreaofOperationRuralDetail(page, limit);
                if (response != null && response.status == "Success")
                {
                    if (response.result != null)
                    {
                        var result = _nCDServiceModel.SaveAreaofOperationRuralDetails(response.result);
                        response.status = "200";
                        response.message = "Details saved successfully.";
                    }
                    else
                    {
                        response.status = "204";
                        response.message = "No data found.";
                    }
                }
                else
                {
                    response.status = "400";
                    response.ErrorMessage = "API returned failure.";
                }
            }
            catch (Exception ex)
            {
                response.status = "500";
                response.ErrorMessage = "Internal Server Error: " + ex.Message;
            }
            return new JsonResult { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }


        [HttpGet]
        public async Task<JsonResult> GetAreaofOperationUrbanDetail(int page = 1, int limit = 100000)
        {
            var response = new NcdApiCommonResponse<AreaofOperationUrbanResultItem>();
            try
            {
                response = await _nCDServiceModel.GetAreaofOperationUrbanDetail(page, limit);
                if (response != null && response.status == "Success")
                {
                    if (response.result != null)
                    {
                        var result = _nCDServiceModel.SaveAreaofOperationUrbanDetails(response.result);
                        response.status = "200";
                        response.message = "Details saved successfully.";
                    }
                    else
                    {
                        response.status = "204";
                        response.message = "No data found.";
                    }
                }
                else
                {
                    response.status = "400";
                    response.ErrorMessage = "API returned failure.";
                }
            }
            catch (Exception ex)
            {
                response.status = "500";
                response.ErrorMessage = "Internal Server Error: " + ex.Message;
            }
            return new JsonResult { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }
        [HttpGet]
        public async Task<JsonResult> GetSocietyImplementingSchemesDetails(int page = 1, int limit = 100000)
        {
            var response = new NcdApiCommonResponse<SocietyImplementingSchemesDetailsResultItem>();
            try
            {
                response = await _nCDServiceModel.GetSocietyImplementingSchemesDetails(page, limit);
                if (response != null && response.status == "Success")
                {
                    if (response.result != null)
                    {
                        var result = _nCDServiceModel.SaveSocietyImplementingSchemesDetails(response.result);
                        response.status = "200";
                        response.message = "Details saved successfully.";
                    }
                    else
                    {
                        response.status = "204";
                        response.message = "No data found.";
                    }
                }
                else
                {
                    response.status = "400";
                    response.ErrorMessage = "API returned failure.";
                }
            }
            catch (Exception ex)
            {
                response.status = "500";
                response.ErrorMessage = "Internal Server Error: " + ex.Message;
            }
            return new JsonResult { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }
        [HttpGet]
        public async Task<JsonResult> GetCooperativeRegistrationsLandsDetails(int page = 1, int limit = 100000)
        {
            var response = new NcdApiCommonResponse<CooperativeRegistrationsLandsDetailsResultItem>();
            try
            {
                response = await _nCDServiceModel.GetCooperativeRegistrationsLandsDetails(page, limit);
                if (response != null && response.status == "Success")
                {
                    if (response.result != null)
                    {
                        var result = _nCDServiceModel.SaveCooperativeRegistrationsLandsDetails(response.result);
                        response.status = "200";
                        response.message = "Details saved successfully.";
                    }
                    else
                    {
                        response.status = "204";
                        response.message = "No data found.";
                    }
                }
                else
                {
                    response.status = "400";
                    response.ErrorMessage = "API returned failure.";
                }
            }
            catch (Exception ex)
            {
                response.status = "500";
                response.ErrorMessage = "Internal Server Error: " + ex.Message;
            }
            return new JsonResult
            {
                Data = response,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        [HttpGet]
        public async Task<JsonResult> GetSocietyAuditYearsDetails(int page = 1, int limit = 100000)
        {
            var response = new NcdApiCommonResponse<NCDSociatyAuditYearsResultItem>();
            try
            {
                response = await _nCDServiceModel.GetSocietyAuditYears(page, limit);
                if (response != null && response.status == "Success")
                {
                    if (response.result != null)
                    {
                        var result = _nCDServiceModel.SaveSocietyAuditYearsDetails(response.result);
                        response.status = "200";
                        response.message = "Details saved successfully.";
                    }
                    else
                    {
                        response.status = "204";
                        response.message = "No data found.";
                    }
                }
                else
                {
                    response.status = "400";
                    response.ErrorMessage = "API returned failure.";
                }
            }
            catch (Exception ex)
            {
                response.status = "500";
                response.ErrorMessage = "Internal Server Error: " + ex.Message;
            }
            return new JsonResult
            {
                Data = response,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        #endregion


        #region Sync NCD sector data
        [HttpGet]
        public async Task<JsonResult> SyncCooperativeChildTableDataBySector(int sectorcode, int page = 1, int limit = 100000)
        {
            var response = new NCDCommonResponse();
            try
            {
                response = await _nCDServiceModel.SyncCooperativeChildTableDataBySector(sectorcode, page, limit);
                if (response != null && response.status == "Success")
                {
                    response.status = "200";
                    response.message = "Details saved successfully.";
                }
                else
                {
                    response.status = "400";
                    response.message = "API returned failure.";
                }
            }
            catch (Exception ex)
            {
                response.status = "500";
                response.message = "Internal Server Error: " + ex.Message;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Affilation Union 04-12-2025
        [HttpGet]
        public async Task<JsonResult> GetAffilationUnionDetail()
        {
            NCDAffilationUnionAPIResponse responseData = new NCDAffilationUnionAPIResponse();
            try
            {
                responseData = await _nCDServiceModel.GetAffilationUnionDetail();
                if (responseData != null && responseData.status == "Success")
                {
                    if (responseData.result != null && responseData.result.Count > 0)
                    {
                        var result = _nCDServiceModel.SaveAffilationUnionDetail(responseData.result);
                        if (result > 0)
                        { responseData.status = "200"; responseData.message = $"Affiliation union data imported {result} successfully."; }
                        else
                        { responseData.status = "201"; responseData.message = "Data received from API, but no records were saved."; }
                    }
                }
                else
                {
                    responseData.status = "400";
                    responseData.ErrorMessage = "API returned failure.";
                }
            }
            catch (Exception ex)
            {
                responseData.status = "500";
                responseData.ErrorMessage = "Internal Server Error: " + ex.Message;
            }
            return Json(responseData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetUnionFederationDetail()
        {
            NCDUnionFederationAPIResponse responseData = new NCDUnionFederationAPIResponse();
            try
            {
                responseData = await _nCDServiceModel.GetUnionFederationDetail();
                if (responseData != null && responseData.status == "Success")
                {
                    if (responseData.data != null && responseData.data.Count > 0)
                    {
                        var result = _nCDServiceModel.SaveFederationUnionDetail(responseData.data);
                        if (result > 0)
                        { responseData.status = "200"; responseData.message = $"Federation union data imported {result} successfully."; }
                        else
                        { responseData.status = "201"; responseData.message = "Data received from API, but no records were saved."; }
                    }
                }
                else
                {
                    responseData.status = "400";
                    responseData.ErrorMessage = "API returned failure.";
                }

            }
            catch (Exception ex)
            {
                responseData.status = "500";
                responseData.ErrorMessage = "Internal Server Error: " + ex.Message;
            }
            return Json(responseData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetLevelAffiliatedUnionDetail()
        {
            NCDLevelAffiliatedUnionAPIResponse responseData = new NCDLevelAffiliatedUnionAPIResponse();
            try
            {
                responseData = await _nCDServiceModel.GetLevelAffiliatedUnionDetail();
                if (responseData != null && responseData.status == "Success")
                {
                    if (responseData.data != null && responseData.data.Count > 0)
                    {
                        var result = _nCDServiceModel.SaveLevelAffiliatedUnionDetail(responseData.data);
                        if (result > 0)
                        { responseData.status = "200"; responseData.message = $"Level affiliated union data imported {result} successfully."; }
                        else
                        { responseData.status = "201"; responseData.message = "Data received from API, but no records were saved."; }
                    }
                }
                else
                {
                    responseData.status = "400";
                    responseData.ErrorMessage = "API returned failure.";
                }

            }
            catch (Exception ex)
            {
                responseData.status = "500";
                responseData.ErrorMessage = "Internal Server Error: " + ex.Message;
            }
            return Json(responseData, JsonRequestBehavior.AllowGet);
        }

        #endregion



        #region Financial details 29-01-2026
        [HttpPost]
        public async Task<JsonResult> GetNcdFinancialDetails()
        {
            var responseData = new CommonFinancialResponse<NCDFinancialResponseModel>();
            try
            {
                responseData = await _nCDServiceModel.GetFinancialDetailByStatecode();
                if (responseData != null && responseData.status == "Success")
                {
                    if (responseData != null && responseData.result.Count > 0)
                    {
                        var result = _nCDServiceModel.SaveFinancialDetailsData(responseData.result);
                        if (result > 0)
                        {
                            responseData.statuscode = responseData.statuscode;
                            responseData.status = "Success";
                            responseData.message = $"NCD financial details were successfully imported : {result}";
                        }
                        else
                        {
                            responseData.statuscode = responseData.statuscode;
                            responseData.status = "Error";
                            responseData.message = "Financial data was received, but no records were saved or updated.";
                        }
                    }
                    else
                    {
                        responseData.statuscode = responseData.statuscode;
                        responseData.status = responseData.status;
                        responseData.message = "No NCD financial details available to import.";
                    }
                }
            }
            catch (Exception ex)
            {
                responseData.statuscode = 500;
                responseData.status = "Error";
                responseData.message = "Internal server error while processing NCD financial details: " + ex.Message;
            }
            return Json(new { statuscode = responseData.statuscode, status = responseData.status, message = responseData.message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete Society 17-08-2026
        [HttpPost]
        public async Task<JsonResult> DeleteSocietyDetails(SocietyRequestDataModel requestParams)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            var responseData = new SocietyDetailResponse();
            try
            {
                responseData = await _nCDServiceModel.DeleteSocietyDetail(requestParams);
                if (responseData != null && responseData.status == "success")
                {
                    if (!Request.Url.Host.Contains("localhost"))
                    {
                        var ds = _nCDServiceModel.SocietySoftDeleted(_LoggedInUser.UserId, requestParams);
                        COMMON_FUNCTION.Sp_callTrackingInsert("ncd_delete_response", requestParams.ncd_id.ToString() + "=" + JsonConvert.SerializeObject(responseData));
                        if (ds != null)
                        {
                            responseData.message = Convert.ToString(ds.Tables[0].Rows[0]["message"]);
                            responseData.status = Convert.ToString(ds.Tables[0].Rows[0]["status"]);
                            responseData.statuscode = 200;
                            responseData.deleted = Convert.ToString(ds.Tables[0].Rows[0]["cooperativeId"]);
                            responseData.name = Convert.ToString(ds.Tables[0].Rows[0]["name"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                responseData.statuscode = 500;
                responseData.status = "Error";
                responseData.message = "Failed to delete: " + ex.Message;
            }
            return Json(new
            {
                statuscode = responseData.statuscode,
                status = responseData.status,
                message = responseData.message
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}