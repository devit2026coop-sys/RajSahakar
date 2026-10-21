using Cooperative.Filters;
using Cooperative.Models;
using sun.security.timestamp;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static Cooperative.Models.LGDServiceModel;

namespace Cooperative.Controllers
{
    public class LGDServiceController : BaseController
    {
        LGDServiceModel _lgdServiceModel = new LGDServiceModel(new DBLogger());


        [CustomAuthentication]
        public ActionResult LGDServiceDataSync()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetLgdDistrictByState(LGDDistrictModel model)
        {
            var responseData = new LGDCommonAPIResponse<LGDDistrictModel>();
            try
            {
                responseData = await _lgdServiceModel.GetActiveDistrictByState(model.Statecode);
                if (responseData != null && responseData.status == "Success")
                {
                    if (responseData != null && responseData.data.Count > 0)
                    {
                        var result = _lgdServiceModel.savedistrictData(responseData.data);
                        if (result > 0)
                        {
                            responseData.statuscode = 200;
                            responseData.status = "Success";
                            responseData.message = $"Districts data imported successfully ({result} records).";
                        }
                        else
                        {
                            responseData.statuscode = 204;
                            responseData.status = "Failed";
                            responseData.message = "No district data was saved.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                responseData.status = "Error";
                responseData.statuscode = 500;
                responseData.message = "Internal Server Error: " + ex.Message;
            }
            return Json(new { status = responseData.status, statuscode = responseData.statuscode, message = responseData.message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GetLgdBlockDataByState(int statecode = 0)
        {
            var responseData = new LGDCommonAPIResponse<LGDBlockModel>();
            try
            {
                responseData = await _lgdServiceModel.GetActiveBlockByState(statecode);
                if (responseData != null && responseData.status == "Success")
                {
                    if (responseData != null && responseData.data.Count > 0)
                    {
                        var result = _lgdServiceModel.SaveBlockDetails(responseData.data);
                        if (result > 0)
                        {
                            responseData.statuscode = 200;
                            responseData.status = "Success";
                            responseData.message = $"Blocks data imported successfully ({result} records).";
                        }
                        else
                        {
                            responseData.statuscode = 204;
                            responseData.status = "Failed";
                            responseData.message = "No blocks data was saved.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                responseData.status = "Error";
                responseData.statuscode = 500;
                responseData.message = "Internal Server Error: " + ex.Message;
            }
            return Json(new { status = responseData.status, statuscode = responseData.statuscode, message = responseData.message }, JsonRequestBehavior.AllowGet);

        }

        public async Task<JsonResult> GetLgdWardDataByState(int statecode = 0, int pageNo = 0)
        {
            var responseData = new LGDCommonAPIResponse<LGDWardsModel>();
            try
            {
                responseData = await _lgdServiceModel.GetActiveWardByState(statecode, pageNo);
                //if (responseData != null && responseData.status == "Success")
                //{
                //    if (responseData != null && responseData.data.Count > 0)
                //    {
                //        var result = _lgdServiceModel.SaveWardDetails(responseData.data);
                //        if (result > 0)
                //        {
                //            responseData.statuscode = 200;
                //            responseData.status = "Success";
                //            responseData.message = $"Wards data imported successfully ({result} records).";
                //        }
                //        else
                //        {
                //            responseData.statuscode = 204;
                //            responseData.status = "Failed";
                //            responseData.message = "No wards data was saved.";
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                responseData.status = "Error";
                responseData.statuscode = 500;
                responseData.message = "Internal Server Error: " + ex.Message;
            }
            return Json(new { status = responseData.status, statuscode = responseData.statuscode, message = responseData.message }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetLgdCitiesDataByState(int statecode = 0)
        {
            var responseData = new LGDCommonAPIResponse<LGDCitiesModel>();
            try
            {
                responseData = await _lgdServiceModel.GetActiveCitiesByState(statecode);
                //if (responseData != null && responseData.status == "Success")
                //{
                //    if (responseData != null && responseData.data.Count > 0)
                //    {
                //        var result = _lgdServiceModel.SaveCitiesDetails(responseData.data);
                //        if (result > 0)
                //        {
                //            responseData.statuscode = 200;
                //            responseData.status = "Success";
                //            responseData.message = $"Cities data imported successfully ({result} records).";
                //        }
                //        else
                //        {
                //            responseData.statuscode = 204;
                //            responseData.status = "Failed";
                //            responseData.message = "No cities data was saved.";
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                responseData.status = "Error";
                responseData.statuscode = 500;
                responseData.message = "Internal Server Error: " + ex.Message;
            }
            return Json(new { status = responseData.status, statuscode = responseData.statuscode, message = responseData.message }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetLgdVillageDataByState(int statecode = 0)
        {
            var responseData = new LGDCommonAPIResponse<LGDVillageModel>();
            try
            {
                responseData = await _lgdServiceModel.GetActiveVillageByState(statecode);
                //if (responseData != null && responseData.status == "Success")
                //{
                //    if (responseData != null && responseData.data.Count > 0)
                //    {
                //        var result = _lgdServiceModel.SaveCitiesDetails(responseData.data);
                //        if (result > 0)
                //        {
                //            responseData.statuscode = 200;
                //            responseData.status = "Success";
                //            responseData.message = $"Cities data imported successfully ({result} records).";
                //        }
                //        else
                //        {
                //            responseData.statuscode = 204;
                //            responseData.status = "Failed";
                //            responseData.message = "No cities data was saved.";
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                responseData.status = "Error";
                responseData.statuscode = 500;
                responseData.message = "Internal Server Error: " + ex.Message;
            }
            //return Json(new { status = responseData.status, statuscode = responseData.statuscode, message = responseData.message }, JsonRequestBehavior.AllowGet);
            return new JsonResult
            {
                Data = new { status = responseData.status, statuscode = responseData.statuscode, message = responseData.message, data = responseData.data },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        public async Task<JsonResult> GetLgdLocalbodiesDataByState(int statecode = 0, int pageNo = 0)
        {
            var responseData = new LGDCommonAPIResponse<LGDLocalbodiesViewModel>();
            object returnResponseData = null;
            try
            {

                responseData = await _lgdServiceModel.GetActiveLocalbodiesByState(statecode, pageNo);
                //if (responseData != null && responseData.status == "Success")
                //{
                //    if (responseData != null && responseData.data.Count > 0)
                //    {
                //        var result = _lgdServiceModel.SaveCitiesDetails(responseData.data);
                //        if (result > 0)
                //        {
                //            json = new
                //            {
                //                status = responseData.status,
                //                statuscode = responseData.statuscode,
                //                message = $"Cities data imported successfully ({result} records).",
                //            };
                //        }
                //        else
                //        {
                //            json = new
                //            {
                //                status = responseData.status,
                //                statuscode = responseData.statuscode,
                //                message = responseData.message,
                //            };
                //        }
                //    }
                //}
                if (responseData.data != null)
                {
                    returnResponseData = new
                    {
                        status = responseData.status,
                        statuscode = responseData.statuscode,
                        message = responseData.message,
                        data = responseData.data
                    };
                }
                else
                {
                    returnResponseData = new
                    {
                        status = responseData.status,
                        statuscode = responseData.statuscode,
                        message = responseData.message,
                        errors = responseData.errors
                    };
                }
            }
            catch (Exception ex)
            {
                returnResponseData = new
                {
                    status = "Error",
                    statuscode = 500,
                    message = "Internal Server Error: " + ex.Message
                };
            }
            return new JsonResult
            {
                Data = returnResponseData,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }
    }
}