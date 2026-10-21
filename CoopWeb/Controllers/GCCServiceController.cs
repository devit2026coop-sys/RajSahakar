using Cooperative.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cooperative.Controllers
{
    public class GCCServiceController : BaseController
    {
        GCCServiceModel _gccServiceModel = new GCCServiceModel(new DBLogger());


        public ActionResult GCCDataSync()
        {
            return View();
        }


        [HttpGet]
        public JsonResult GetDccbcodeData()
        {
            var responce = new APIResponce();
            try
            {
                var ds = _gccServiceModel.GetccbDetails();
                if (ds != null)
                {
                    responce.Data = new
                    {
                        dccbData = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                    };
                    responce.Status = "200";
                    responce.Message = "Success";
                }
                else
                {
                    responce.Status = "404";
                    responce.Message = "Dccbcode not found.";
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
        public JsonResult GetRsobOdData(string type = "")
        {
            var responce = new APIResponce();
            try
            {
                var ds = _gccServiceModel.GetRSCBODData(type);
                if (ds != null)
                {
                    responce.Data = new
                    {
                        rscbData = ds.Tables.Count > 0 ? COMMON_FUNCTION.DataTableToJSONString(ds.Tables[0]) : null,
                    };
                    responce.Status = "200";
                    responce.Message = "Success";
                }
                else
                {
                    responce.Status = "404";
                    responce.Message = "Record not found.";
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