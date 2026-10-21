using Cooperative.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using static Cooperative.Models.NCDApiModel;
using static Cooperative.Models.NCDServiceModel;

namespace Cooperative.Controllers
{
    public class NCDApiController : BaseController
    {
        NCDApiModel ncdApiModel = new NCDApiModel(new DBLogger());
        NCDApiCommonResponse response = new NCDApiCommonResponse();
        NCDApiCommonResponse2 response2 = new NCDApiCommonResponse2();
        string stateKey = "28aGOXtPqgScSOpLaxVm5iwNlRP3aLYi";

        [HttpPost]
        public ActionResult GetCooperativeRegDetails(NCDApiRequestModel reqModel)
        {
            var response = new NCDApiResponse();
            List<NCDCooperativeRegistrationResponseModel> modelList = new List<NCDCooperativeRegistrationResponseModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }

                var ds = ncdApiModel.GetCooperativeRegistrationDetails(reqModel);
                if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    modelList = ncdApiModel.ConvertDTToList<NCDCooperativeRegistrationResponseModel>(ds.Tables[0]);
                    var row = ds.Tables[1].Rows[0];

                    response.status = "Success";
                    response.state_code = Convert.ToInt32(row["state_code"]);
                    response.state_name = row["state_name"].ToString();
                    response.total_societies = Convert.ToInt32(row["total_societies"]);
                    response.functional_societies = Convert.ToInt32(row["functional_societies"]);
                    response.non_functional_societies = Convert.ToInt32(row["non_functional_societies"]);
                    response.under_liquidation_societies = Convert.ToInt32(row["under_liquidation_societies"]);
                    response.total_members = Convert.ToInt32(row["total_members"]);
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    response.totalPages = Convert.ToInt32(row["totalPages"].ToString());
                    if (response.totalPages >= reqModel.pageNumber)
                        response.result = modelList;
                    else
                        response.result = "Page number exceeds the size of total pages.";
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "Data not found."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse
                {
                    status = "Failed",
                    error_message = "Internal Server Error." + ex.Message,
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetAlliedAgriculturDetails(NCDApiRequestModel reqModel)
        {

            List<NCDAgricultureResponseModel> modelList = new List<NCDAgricultureResponseModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }

                var ds = ncdApiModel.GetNCDAgricultureDetailsByState(reqModel);
                if (ds != null)
                {
                    DataTable dataTable = ds.Tables[0];
                    modelList = ncdApiModel.ConvertDTToList<NCDAgricultureResponseModel>(dataTable);
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Agriculture & Allied Cooperative";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "data not found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse
                {
                    status = "Failed",
                    error_message = "Internal Server Error." + ex.Message,
                }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult GetAgroProcessingDetails(NCDApiRequestModel reqModel)
        {

            List<NCDAgroProcessingResponseModel> modelList = new List<NCDAgroProcessingResponseModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }

                modelList = ncdApiModel.GetNCDAgroProcessingDetailsByState(reqModel);
                if (modelList != null)
                {
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Agro Processing / Industrial Cooperative";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "data not found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetBeeFarmingDetails(NCDApiRequestModel reqModel)
        {
            List<NCDBeeFarmingResponseModel> modelList = new List<NCDBeeFarmingResponseModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }

                modelList = ncdApiModel.GetNCDBeeFarmingDetailsByState(reqModel);
                if (modelList != null)
                {
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Bee Farming Cooperative";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "data not found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetConsumerDetails(NCDApiRequestModel reqModel)
        {
            List<NCDConsumerResponseModel> modelList = new List<NCDConsumerResponseModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }

                modelList = ncdApiModel.GetNCDConsumerDetailsByState(reqModel);
                if (modelList != null)
                {
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Consumer Cooperative";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "data not found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetCreditThriftSocietyDetails(NCDApiRequestModel reqModel)
        {
            List<NCDCreditThriftResponseModel> modelList = new List<NCDCreditThriftResponseModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }

                modelList = ncdApiModel.GetNCDCreditThriftDetailsByState(reqModel);
                if (modelList != null)
                {
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Credit & Thrift Society";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "data not found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetDairyCooperativeDetails(NCDApiRequestModel reqModel)
        {
            List<NCDDairyResponseModel> modelList = new List<NCDDairyResponseModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }

                modelList = ncdApiModel.GetNCDDairyCooperativeDetailsByState(reqModel);
                if (modelList != null)
                {
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Dairy Cooperative";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "data not found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetEducationTrainingDetails(NCDApiRequestModel reqModel)
        {
            List<NCDEducationTraningResponseModel> modelList = new List<NCDEducationTraningResponseModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }

                modelList = ncdApiModel.GetNCDEducationTrainingDetailsByState(reqModel);
                if (modelList != null)
                {
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Educational & Training Cooperatives";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "data not found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetPacsDetails(NCDApiRequestModel reqModel)
        {
            List<NCDPacsLampsFssResponseModel> modelList = new List<NCDPacsLampsFssResponseModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }

                modelList = ncdApiModel.GetNCDPacsDetailsByState(reqModel);
                if (modelList != null)
                {
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Primary Agricultural Credit Society (PACS)";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "data not found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetFisheryCooperativeDetails(NCDApiRequestModel reqModel)
        {
            List<NCDFisheryResponseModel> modelList = new List<NCDFisheryResponseModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }

                modelList = ncdApiModel.GetNCDFisheryCooperativeDetailsByState(reqModel);
                if (modelList != null)
                {
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Fishery Cooperative";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "data not found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetHandicraftDetails(NCDApiRequestModel reqModel)
        {
            List<NCDHandicraftResponseModel> modelList = new List<NCDHandicraftResponseModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }

                modelList = ncdApiModel.GetNCDHandicraftDetailsByState(reqModel);
                if (modelList != null)
                {
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Handicraft Cooperative";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "data not found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetHandloomDetails(NCDApiRequestModel reqModel)
        {
            List<NCDHandloomResponseModel> modelList = new List<NCDHandloomResponseModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }

                modelList = ncdApiModel.GetNCDHandloomDetailsByState(reqModel);
                if (modelList != null)
                {
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Handloom Textile & Weavers Cooperative";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "data not found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetHousingCooperativeDetails(NCDApiRequestModel reqModel)
        {
            List<NCDHousingResponseModel> modelList = new List<NCDHousingResponseModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }

                modelList = ncdApiModel.GetNCDHousingDetailsByState(reqModel);
                if (modelList != null)
                {
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Housing Cooperative Society";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "data not found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetJuteCoirDetails(NCDApiRequestModel reqModel)
        {
            List<NCDJuteResponseModel> modelList = new List<NCDJuteResponseModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }

                modelList = ncdApiModel.GetNCDJuteCoirDetailsByState(reqModel);
                if (modelList != null)
                {
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Jute and Coir Cooperative";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "data not found."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse
                {
                    status = "Failed",
                    error_message = "Internal Server Error."
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetLabourCooperativeDetails(NCDApiRequestModel reqModel)
        {
            List<NCDLabourResponseModel> modelList = new List<NCDLabourResponseModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }

                modelList = ncdApiModel.GetNCDLabourDetailsByState(reqModel);
                if (modelList != null)
                {
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Labour Cooperative";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "data not found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetLiveStockDetails(NCDApiRequestModel reqModel)
        {
            try
            {
                List<NCDLiveStockResponseModel> modelList = new List<NCDLiveStockResponseModel>();
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }

                modelList = ncdApiModel.GetNCDLiveStockDetailsByState(reqModel);
                if (modelList != null)
                {
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Livestock & Poultry Cooperative";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "data not found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetMarketingDetails(NCDApiRequestModel reqModel)
        {
            try
            {
                List<NCDMarketingResponseModel> modelList = new List<NCDMarketingResponseModel>();
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }

                modelList = ncdApiModel.GetNCDMarketingDetailsByState(reqModel);
                if (modelList != null)
                {
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Marketing Cooperative Society";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "data not found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetMiscellaneousCreditDetails(NCDApiRequestModel reqModel)
        {
            try
            {
                List<NCDCmiscellaneousResponseModel> modelList = new List<NCDCmiscellaneousResponseModel>();

                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }

                modelList = ncdApiModel.GetNCDMiscellaneousCreditDetailsByState(reqModel);
                if (modelList != null)
                {
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Miscellaneous Credit Cooperative Society";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "data not found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetMiscellaneousNonCreditDetails(NCDApiRequestModel reqModel)
        {
            try
            {
                List<NCDMiscellaneousResponseModel> modelList = new List<NCDMiscellaneousResponseModel>();

                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }

                modelList = ncdApiModel.GetNCDMiscellaneousNonCreditDetailsByState(reqModel);
                if (modelList != null)
                {
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Miscellaneous Non Credit";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "data not found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetMultipurposeDetails(NCDApiRequestModel reqModel)
        {
            try
            {
                List<NCDMultipurposeResponseModel> modelList = new List<NCDMultipurposeResponseModel>();

                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }

                modelList = ncdApiModel.GetNCDMultipurposeDetailsByState(reqModel);
                if (modelList != null)
                {
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Multipurpose Cooperative";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "data not found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetSericultureDetails(NCDApiRequestModel reqModel)
        {
            try
            {
                List<NCDSericultureResponseModel> modelList = new List<NCDSericultureResponseModel>();
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }

                modelList = ncdApiModel.GetNCDSericultureDetailsByState(reqModel);
                if (modelList != null)
                {
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Sericulture Cooperative";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "data not found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetSocialWelfareDetails(NCDApiRequestModel reqModel)
        {
            try
            {
                List<NCDSocialResponseModel> modelList = new List<NCDSocialResponseModel>();
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }

                modelList = ncdApiModel.GetNCDSocialWelfareDetailsByState(reqModel);
                if (modelList != null)
                {
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Social Welfare & Cultural Cooperative";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "data not found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetSugarMillsDetails(NCDApiRequestModel reqModel)
        {
            try
            {
                List<NCDSugarResponseModel> modelList = new List<NCDSugarResponseModel>();
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }

                modelList = ncdApiModel.GetNCDSugarDetailsByState(reqModel);
                if (modelList != null)
                {
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Sugar Mills Cooperative";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "data not found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetTourismDetails(NCDApiRequestModel reqModel)
        {
            try
            {
                List<NCDTourismResponseModel> modelList = new List<NCDTourismResponseModel>();
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }

                modelList = ncdApiModel.GetNCDTourismDetailsByState(reqModel);
                if (modelList != null)
                {
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Tourism Cooperative";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "data not found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetTransportDetails(NCDApiRequestModel reqModel)
        {
            List<NCDTransportResponseModel> modelList = new List<NCDTransportResponseModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }

                modelList = ncdApiModel.GetNCDTransportDetailsByState(reqModel);
                if (modelList != null)
                {
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Transport Cooperative";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "No data found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult GetTribalDetails(NCDApiRequestModel reqModel)
        {
            List<NCDTribalResponseModel> modelList = new List<NCDTribalResponseModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }

                modelList = ncdApiModel.GetNCDTribalDetailsByState(reqModel);
                if (modelList != null)
                {
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Tribal-SC/ST Cooperative";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "No data found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult GetUcbDetails(NCDApiRequestModel reqModel)
        {
            List<NCDUCBResponseModel> modelList = new List<NCDUCBResponseModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }

                modelList = ncdApiModel.GetNCDUCBDetailsByState(reqModel);
                if (modelList != null)
                {
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Urban Cooperative Bank (UCB)";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "No data found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetWomenWelfareDetails(NCDApiRequestModel reqModel)
        {
            List<NCDWomenWelfareResponseModel> modelList = new List<NCDWomenWelfareResponseModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }
                modelList = ncdApiModel.GetNCDWomenWelfareDetailsByState(reqModel);
                if (modelList != null)
                {
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Women Welfare Cooperative Society";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "No data found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult GetKhadiGramDetails(NCDApiRequestModel reqModel)
        {
            List<NCDKhadiGramResponseModel> modelList = new List<NCDKhadiGramResponseModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }
                modelList = ncdApiModel.GetNCDKhadiGramDetailsByState(reqModel);
                if (modelList != null)
                {
                    response.status = "Success";
                    response.state_code = 8;
                    response.state_name = "RAJASTHAN";
                    response.sector = "Khadi Gramodyog";
                    response.result = modelList;
                    response.pageNumber = reqModel.pageNumber;
                    response.pageSize = reqModel.pageSize;
                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "No data found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult GetMemberDetails(NCDApiRequestModel reqModel)
        {
            List<NCDMemberDetailsModel> modelList = new List<NCDMemberDetailsModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }
                var ds = ncdApiModel.GetNCDMemberDetailsByState(reqModel);
                if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    modelList = ncdApiModel.ConvertDTToList<NCDMemberDetailsModel>(ds.Tables[0]);
                    var row = ds.Tables[1].Rows[0];

                    response2.status = "Success";
                    response2.state_code = 8;
                    response2.state_name = "RAJASTHAN";
                    response2.api_type = "Members Details";
                    response2.pageNumber = reqModel.pageNumber;
                    response2.pageSize = reqModel.pageSize;
                    response2.records = modelList.Count();
                    response2.totalPages = Convert.ToInt32(row["totalPages"].ToString());
                    if (response2.totalPages >= reqModel.pageNumber)
                        response2.result = modelList;
                    else
                        response2.result = "Page number exceeds the size of total pages.";
                    return new JsonResult
                    {
                        Data = response2,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "No data found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult GetAreaOfOperationRuralDetails(NCDApiRequestModel reqModel)
        {
            List<NCD_AreaofOperationRuralDetails> modelList = new List<NCD_AreaofOperationRuralDetails>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }
                var ds = ncdApiModel.GetNCDAreaOfOprationsRuralDetails(reqModel);
                if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    modelList = ncdApiModel.ConvertDTToList<NCD_AreaofOperationRuralDetails>(ds.Tables[0]);
                    var row = ds.Tables[1].Rows[0];
                    response2.status = "Success";
                    response2.state_code = 8;
                    response2.state_name = "RAJASTHAN";
                    response2.api_type = "Area of Operation - Rural";
                    response2.pageNumber = reqModel.pageNumber;
                    response2.pageSize = reqModel.pageSize;
                    response2.records = modelList.Count();
                    response2.totalPages = Convert.ToInt32(row["totalPages"].ToString());
                    if (response2.totalPages >= reqModel.pageNumber)
                        response2.result = modelList;
                    else
                        response2.result = "Page number exceeds the size of total pages.";
                    return new JsonResult
                    {
                        Data = response2,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "No data found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetAreaOfOperationUrbanDetails(NCDApiRequestModel reqModel)
        {
            List<NCD_AreaofOperationUrbanDetails> modelList = new List<NCD_AreaofOperationUrbanDetails>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }
                var ds = ncdApiModel.GetNCDAreaOfOprationsUrbanDetails(reqModel);
                if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    modelList = ncdApiModel.ConvertDTToList<NCD_AreaofOperationUrbanDetails>(ds.Tables[0]);
                    var row = ds.Tables[1].Rows[0];
                    response2.status = "Success";
                    response2.state_code = 8;
                    response2.state_name = "RAJASTHAN";
                    response2.api_type = "Area of Operation - Urban";
                    response2.pageNumber = reqModel.pageNumber;
                    response2.pageSize = reqModel.pageSize;
                    response2.records = modelList.Count();
                    response2.totalPages = Convert.ToInt32(row["totalPages"].ToString());
                    if (response2.totalPages >= reqModel.pageNumber)
                        response2.result = modelList;
                    else
                        response2.result = "Page number exceeds the size of total pages.";
                    return new JsonResult
                    {
                        Data = response2,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "No data found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetSocietyImplementingSchemesDetails(NCDApiRequestModel reqModel)
        {
            List<NCD_SocietyImplementingSchemesDetails> modelList = new List<NCD_SocietyImplementingSchemesDetails>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }
                var ds = ncdApiModel.GetNCDSocietyImplementingSchemesDetails(reqModel);
                if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    modelList = ncdApiModel.ConvertDTToList<NCD_SocietyImplementingSchemesDetails>(ds.Tables[0]);
                    var row = ds.Tables[1].Rows[0];
                    response2.status = "Success";
                    response2.state_code = 8;
                    response2.state_name = "RAJASTHAN";
                    response2.api_type = "Society Implementing Schemes";
                    response2.pageNumber = reqModel.pageNumber;
                    response2.pageSize = reqModel.pageSize;
                    response2.records = modelList.Count();
                    response2.totalPages = Convert.ToInt32(row["totalPages"].ToString());
                    if (response2.totalPages >= reqModel.pageNumber)
                        response2.result = modelList;
                    else
                        response2.result = "Page number exceeds the size of total pages.";
                    return new JsonResult
                    {
                        Data = response2,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "No data found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetCooperativeRegistrationsLandsDetails(NCDApiRequestModel reqModel)
        {
            List<NCD_CooperativeRegistrationsLandsDetails> modelList = new List<NCD_CooperativeRegistrationsLandsDetails>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }
                var ds = ncdApiModel.GetNCDCooperativeRegistrationsLandsDetails(reqModel);
                if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    modelList = ncdApiModel.ConvertDTToList<NCD_CooperativeRegistrationsLandsDetails>(ds.Tables[0]);
                    var row = ds.Tables[1].Rows[0];
                    response2.status = "Success";
                    response2.state_code = 8;
                    response2.state_name = "RAJASTHAN";
                    response2.api_type = "Cooperative Registrations Lands";
                    response2.pageNumber = reqModel.pageNumber;
                    response2.pageSize = reqModel.pageSize;
                    response2.records = modelList.Count();
                    response2.totalPages = Convert.ToInt32(row["totalPages"].ToString());
                    if (response2.totalPages >= reqModel.pageNumber)
                        response2.result = modelList;
                    else
                        response2.result = "Page number exceeds the size of total pages.";
                    return new JsonResult
                    {
                        Data = response2,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "No data found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]
        public ActionResult GetAuditYearDetails(NCDApiRequestModel reqModel)
        {
            List<NcdSocietyAuditYearsModel> modelList = new List<NcdSocietyAuditYearsModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }
                var ds = ncdApiModel.GetNCDAuditYearDetails(reqModel);
                if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    modelList = ncdApiModel.ConvertDTToList<NcdSocietyAuditYearsModel>(ds.Tables[0]);
                    var row = ds.Tables[1].Rows[0];
                    response2.status = "Success";
                    response2.state_code = 8;
                    response2.state_name = "RAJASTHAN";
                    response2.api_type = "Society Audit Years";
                    response2.pageNumber = reqModel.pageNumber;
                    response2.pageSize = reqModel.pageSize;
                    response2.records = modelList.Count();
                    response2.totalPages = Convert.ToInt32(row["totalPages"].ToString());
                    if (response2.totalPages >= reqModel.pageNumber)
                        response2.result = modelList;
                    else
                        response2.result = "Page number exceeds the size of total pages.";
                    return new JsonResult
                    {
                        Data = response2,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "No data found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }




        [HttpPost]
        public ActionResult GetCooperativeDetailsById(NCDApiRequestModel reqModel)
        {
            NCDRootResponse rootResponse = null;
            List<NCDCooperativeRegistrationResponseModel> lstCooperativeRegistration = new List<NCDCooperativeRegistrationResponseModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }
                var ds = ncdApiModel.GetCooperativeRegistrationDetailsById(reqModel);
                if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {

                    lstCooperativeRegistration = ncdApiModel.ConvertDTToList<NCDCooperativeRegistrationResponseModel>(ds.Tables[0]);

                    if (ds.Tables.Count == 8)
                    {
                        if (lstCooperativeRegistration[0].sector_of_operation == 1 || lstCooperativeRegistration[0].sector_of_operation == 20 || lstCooperativeRegistration[0].sector_of_operation == 22)
                        {
                            rootResponse = new NCDRootResponse<NCDPacsLampsFssResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDPacsLampsFssResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 20)
                        {
                            rootResponse = new NCDRootResponse<NCDPacsLampsFssResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDPacsLampsFssResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 22)
                        {
                            rootResponse = new NCDRootResponse<NCDPacsLampsFssResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDPacsLampsFssResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 2)
                        {
                            rootResponse = new NCDRootResponse<NCDUCBResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDUCBResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 9)
                        {
                            rootResponse = new NCDRootResponse<NCDDairyResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDDairyResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 10)
                        {
                            rootResponse = new NCDRootResponse<NCDFisheryResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDFisheryResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 11)
                        {
                            rootResponse = new NCDRootResponse<NCDSugarResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDSugarResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 13)
                        {
                            rootResponse = new NCDRootResponse<NCDHandloomResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDHandloomResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 14)
                        {
                            rootResponse = new NCDRootResponse<NCDHandicraftResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDHandicraftResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 15)
                        {
                            rootResponse = new NCDRootResponse<NCDWomenWelfareResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDWomenWelfareResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 16)
                        {
                            rootResponse = new NCDRootResponse<NCDMultipurposeResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDMultipurposeResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 18)
                        {
                            rootResponse = new NCDRootResponse<NCDCreditThriftResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDCreditThriftResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 29)
                        {
                            rootResponse = new NCDRootResponse<NCDMiscellaneousResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDMiscellaneousResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 31)
                        {
                            rootResponse = new NCDRootResponse<NCDAgroProcessingResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDAgroProcessingResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 35)
                        {
                            rootResponse = new NCDRootResponse<NCDCmiscellaneousResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDCmiscellaneousResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 47)
                        {
                            rootResponse = new NCDRootResponse<NCDHousingResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDHousingResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 51)
                        {
                            rootResponse = new NCDRootResponse<NCDLabourResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDLabourResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 54)
                        {
                            rootResponse = new NCDRootResponse<NCDLiveStockResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDLiveStockResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 68)
                        {
                            rootResponse = new NCDRootResponse<NCDTransportResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDTransportResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 77)
                        {
                            rootResponse = new NCDRootResponse<NCDAgricultureResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDAgricultureResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 79)
                        {
                            rootResponse = new NCDRootResponse<NCDBeeFarmingResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDBeeFarmingResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 80)
                        {
                            rootResponse = new NCDRootResponse<NCDConsumerResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDConsumerResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 82)
                        {
                            rootResponse = new NCDRootResponse<NCDMarketingResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDMarketingResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 84)
                        {
                            rootResponse = new NCDRootResponse<NCDEducationTraningResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDEducationTraningResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 90)
                        {
                            rootResponse = new NCDRootResponse<NCDJuteResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDJuteResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 96)
                        {
                            rootResponse = new NCDRootResponse<NCDSericultureResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDSericultureResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 98)
                        {
                            rootResponse = new NCDRootResponse<NCDSocialResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDSocialResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 99)
                        {
                            rootResponse = new NCDRootResponse<NCDTourismResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDTourismResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 102)
                        {
                            rootResponse = new NCDRootResponse<NCDTribalResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDTribalResponseModel>(ds.Tables[7])
                            };
                        }
                        else if (lstCooperativeRegistration[0].sector_of_operation == 117)
                        {
                            rootResponse = new NCDRootResponse<NCDKhadiGramResponseModel>
                            {
                                SectorData = ncdApiModel.ConvertDTToList<NCDKhadiGramResponseModel>(ds.Tables[7])
                            };
                        }
                    }

                    rootResponse.CooperativeRegistrations = lstCooperativeRegistration;
                    rootResponse.status = "Success";
                    rootResponse.state_code = 8;
                    rootResponse.state_name = "RAJASTHAN";
                    rootResponse.cooperative_id = rootResponse.CooperativeRegistrations[0].cooperative_id;
                    rootResponse.sector_of_operation = rootResponse.CooperativeRegistrations[0].sector_of_operation;
                    rootResponse.MembersDetails = ncdApiModel.ConvertDTToList<NCDMemberDetailsModel>(ds.Tables[1]);
                    rootResponse.AreaOfOperationLevel = ncdApiModel.ConvertDTToList<NCD_AreaofOperationRuralDetails>(ds.Tables[2]);
                    rootResponse.AreaOfOperationLevelUrban = ncdApiModel.ConvertDTToList<NCD_AreaofOperationUrbanDetails>(ds.Tables[3]);
                    rootResponse.SocietyImplementingSchemes = ncdApiModel.ConvertDTToList<NCD_SocietyImplementingSchemesDetails>(ds.Tables[4]);
                    rootResponse.CooperativeRegistrationsLands = ncdApiModel.ConvertDTToList<NCD_CooperativeRegistrationsLandsDetails>(ds.Tables[5]);
                    rootResponse.SocietyAuditYears = ncdApiModel.ConvertDTToList<NcdSocietyAuditYearsModel>(ds.Tables[6]);
                    return new JsonResult
                    {
                        Data = rootResponse,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "No data found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }




        [HttpPost]
        public ActionResult GetBoardofDirectorDetails(NCDApiRequestModel reqModel)
        {
            List<NCDBoardofDirectorModel> modelList = new List<NCDBoardofDirectorModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(reqModel.apiKey))
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "API key required."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (reqModel.apiKey != stateKey)
                {
                    return Json(new NCDApiErrorResponse
                    {
                        status = "Failed",
                        error_message = "The API key is invalid."
                    }, JsonRequestBehavior.AllowGet);
                }
                var ds = ncdApiModel.GetNCDBoardofDirectorDetails(reqModel);
                if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    modelList = ncdApiModel.ConvertDTToList<NCDBoardofDirectorModel>(ds.Tables[0]);
                    var row = ds.Tables[1].Rows[0];
                    response2.status = "Success";
                    response2.state_code = 8;
                    response2.state_name = "RAJASTHAN";
                    response2.api_type = "Board Of Director";
                    response2.pageNumber = reqModel.pageNumber;
                    response2.pageSize = reqModel.pageSize;
                    response2.records = modelList.Count();
                    response2.totalPages = Convert.ToInt32(row["totalPages"].ToString());
                    if (response2.totalPages >= reqModel.pageNumber)
                        response2.result = modelList;
                    else
                        response2.result = "Page number exceeds the size of total pages.";
                    return new JsonResult
                    {
                        Data = response2,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                {
                    return Json(new NCDApiErrorResponse { status = "Failed", error_message = "No data found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new NCDApiErrorResponse { status = "Failed", error_message = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}