using Cooperative.Filters;
using Cooperative.Models;
using sun.security.timestamp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static Cooperative.Models.MMPBYFarmerDetailsModel;

namespace Cooperative.Controllers
{
    public class MMPBYFarmerDetailsController : BaseController
    {
        MMPBYFarmerDetailsModel _mmpbyFarmerDetailModel = new MMPBYFarmerDetailsModel(new DBLogger());


        public ActionResult MMPBYFarmerDataSync()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> MMPBYFarmerDetailsByDistrictcode(int DISTRICT_CODE = 0)
        {
            var responseData = new CommonAPIResponse<FarmerDetailsModel>();
            try
            {
                responseData = await _mmpbyFarmerDetailModel.GetMMPBYFarmerDetailsByDistrict(DISTRICT_CODE);

                if (responseData != null && responseData.is_success == true)
                {
                    if (responseData != null && responseData.data.Count > 0)
                    {
                        var result = _mmpbyFarmerDetailModel.SaveFarmerDetailData(responseData.data);
                        if (result > 0)
                        {
                            responseData.is_success = true;
                            responseData.message = $"Farmer details imported successfully. Total records processed: {result}.";
                        }
                        else
                        {
                            responseData.is_success = false;
                            responseData.message = "Farmer data was received, but no records were saved or updated.";
                        }
                    }
                    else
                    {
                        responseData.is_success = false;
                        responseData.message = "No farmer data found to import.";
                    }
                }
            }
            catch (Exception ex)
            {
                responseData.is_success = false;
                responseData.message = "Internal Server Error: " + ex.Message;
            }
            return Json(new { is_success = responseData.is_success, message = responseData.message }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMPBYFarmerDetailData(string type = "")
        {
            var responce = new APIResponce();
            try
            {
                var ds = _mmpbyFarmerDetailModel.GetMMPBYFarmerDetails(type);
                if (ds != null)
                {
                    responce.Data = new
                    {
                        farmerData = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
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

    }
}