using CoopDB;
using Cooperative.Filters;
using Cooperative.Models;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Cooperative.Controllers
{
    [Error]
    public class MPRController : BaseController
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
         
        [HttpGet]
        public ActionResult MPRArticle25Form()
        {
            return View();
        }

        [HttpPost]
        public string SaveMPRArticle25Form(MPRArticleEntity _Entity)
        {
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();

            MPRModel objMPRModel = new MPRModel(new DBLogger());
            return objMPRModel.SaveMPRArticle25Data(_Entity).ToString();

        }

        [HttpGet]
        [ActionName("GetMPRArticle25ListDetails")]
        public JsonResult GetMPRArticle25ListDetails()
        {
            MPRModel objModel = new MPRModel();

            DataSet ds = objModel.GetMPRArticle25ListDetails();

            return Json(new { Result = new { Data = new { MPRArticle25List = JsonConvert.SerializeObject(ds.Tables[0]) } } }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult MPRArticle25bForm()
        {
            return View();
        }

        [HttpPost]
        public string SaveMPRArticle25bForm(MPRArticleEntity _Entity)
        {
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();

            MPRModel objMPRModel = new MPRModel(new DBLogger());
            return objMPRModel.SaveMPRArticle25bData(_Entity).ToString();

        }

        [HttpGet]
        [ActionName("GetMPRArticle25bListDetails")]
        public JsonResult GetMPRArticle25bListDetails()
        {
            MPRModel objModel = new MPRModel();

            DataSet ds = objModel.GetMPRArticle25bListDetails();

            return Json(new { Result = new { Data = new { MPRArticle25bList = JsonConvert.SerializeObject(ds.Tables[0]) } } }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult MPRArticle30Form()
        {
            return View();
        }

        [HttpPost]
        public string SaveMPRArticle30Form(MPRArticleEntity _Entity)
        {
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();

            MPRModel objMPRModel = new MPRModel(new DBLogger());
            return objMPRModel.SaveMPRArticle30Data(_Entity).ToString();

        }

        [HttpGet]
        [ActionName("GetMPRArticle30ListDetails")]
        public JsonResult GetMPRArticle30ListDetails()
        {
            MPRModel objModel = new MPRModel();

            DataSet ds = objModel.GetMPRArticle30ListDetails();

            return Json(new { Result = new { Data = new { MPRArticle30List = JsonConvert.SerializeObject(ds.Tables[0]) } } }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult MPRArticle125Form()
        {
            return View();
        }

        [HttpPost]
        public string SaveMPRArticle125Form(MPRArticleEntity _Entity)
        {
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();

            MPRModel objMPRModel = new MPRModel(new DBLogger());
            return objMPRModel.SaveMPRArticle125Data(_Entity).ToString();

        }

        [HttpGet]
        [ActionName("GetMPRArticle125ListDetails")]
        public JsonResult GetMPRArticle125ListDetails()
        {
            MPRModel objModel = new MPRModel();

            DataSet ds = objModel.GetMPRArticle125ListDetails();

            return Json(new { Result = new { Data = new { MPRArticle125List = JsonConvert.SerializeObject(ds.Tables[0]) } } }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult MPRArticle55Form()
        {
            return View();
        }

        [HttpPost]
        public string SaveMPRArticle55Form(MPRArticleEntity _Entity)
        {
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();

            MPRModel objMPRModel = new MPRModel(new DBLogger());
            return objMPRModel.SaveMPRArticle55Data(_Entity).ToString();

        }

        [HttpGet]
        [ActionName("GetMPRArticle55ListDetails")]
        public JsonResult GetMPRArticle55ListDetails()
        {
            MPRModel objModel = new MPRModel();

            DataSet ds = objModel.GetMPRArticle55ListDetails();

            return Json(new { Result = new { Data = new { MPRArticle55List = JsonConvert.SerializeObject(ds.Tables[0]) } } }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult MPRArticle57Form()
        {
            return View();
        }

        [HttpPost]
        public string SaveMPRArticle57Form(MPRArticleEntity _Entity)
        {
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();

            MPRModel objMPRModel = new MPRModel(new DBLogger());
            return objMPRModel.SaveMPRArticle57Data(_Entity).ToString();

        }

        [HttpGet]
        [ActionName("GetMPRArticle57ListDetails")]
        public JsonResult GetMPRArticle57ListDetails()
        {
            MPRModel objModel = new MPRModel();

            DataSet ds = objModel.GetMPRArticle57ListDetails();

            return Json(new { Result = new { Data = new { MPRArticle57List = JsonConvert.SerializeObject(ds.Tables[0]) } } }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult MPRArticle57AForm()
        {
            return View();
        }

        [HttpPost]
        public string SaveMPRArticle57AForm(MPRArticleEntity _Entity)
        {
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();

            MPRModel objMPRModel = new MPRModel(new DBLogger());
            return objMPRModel.SaveMPRArticle57AData(_Entity).ToString();

        }

        [HttpGet]
        [ActionName("GetMPRArticle57AListDetails")]
        public JsonResult GetMPRArticle57AListDetails()
        {
            MPRModel objModel = new MPRModel();

            DataSet ds = objModel.GetMPRArticle57AListDetails();

            return Json(new { Result = new { Data = new { MPRArticle57AList = JsonConvert.SerializeObject(ds.Tables[0]) } } }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult MPRArticle57RForm()
        {
            return View();
        }

        [HttpPost]
        public string SaveMPRArticle57RForm(MPRArticleEntity _Entity)
        {
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();

            MPRModel objMPRModel = new MPRModel(new DBLogger());
            return objMPRModel.SaveMPRArticle57RData(_Entity).ToString();

        }

        [HttpGet]
        [ActionName("GetMPRArticle57RListDetails")]
        public JsonResult GetMPRArticle57RListDetails()
        {
            MPRModel objModel = new MPRModel();

            DataSet ds = objModel.GetMPRArticle57RListDetails();

            return Json(new { Result = new { Data = new { MPRArticle57RList = JsonConvert.SerializeObject(ds.Tables[0]) } } }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult MPRArticle61Form()
        {
            return View();
        }

        [HttpPost]
        public string SaveMPRArticle61Form(MPRArticleEntity _Entity)
        {
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();

            MPRModel objMPRModel = new MPRModel(new DBLogger());
            return objMPRModel.SaveMPRArticle61Data(_Entity).ToString();

        }

        [HttpGet]
        [ActionName("GetMPRArticle61ListDetails")]
        public JsonResult GetMPRArticle61ListDetails()
        {
            MPRModel objModel = new MPRModel();

            DataSet ds = objModel.GetMPRArticle61ListDetails();

            return Json(new { Result = new { Data = new { MPRArticle61List = JsonConvert.SerializeObject(ds.Tables[0]) } } }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MPRArticle25Report()
        {
            return View();
        }

        [HttpGet]
        [ActionName("GetMPRArticle25Report")]
        public JsonResult GetMPRArticle25Report()
        {
            MPRModel objModel = new MPRModel();

            DataSet ds = objModel.GetMPRArticle25Report();

            return Json(new { Result = new { Data = new { MPRArticle25ReportList = JsonConvert.SerializeObject(ds.Tables[0]) } } }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MPRArticle125Report()
        {
            return View();
        }

        [HttpGet]
        [ActionName("GetMPRArticle125Report")]
        public JsonResult GetMPRArticle125Report()
        {
            MPRModel objModel = new MPRModel();

            DataSet ds = objModel.GetMPRArticle125Report();

            return Json(new { Result = new { Data = new { MPRArticle125ReportList = JsonConvert.SerializeObject(ds.Tables[0]) } } }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MPRArticle25bReport()
        {
            return View();
        }

        [HttpGet]
        [ActionName("GetMPRArticle25bReport")]
        public JsonResult GetMPRArticle25bReport()
        {
            MPRModel objModel = new MPRModel();

            DataSet ds = objModel.GetMPRArticle25bReport();

            return Json(new { Result = new { Data = new { MPRArticle25bReportList = JsonConvert.SerializeObject(ds.Tables[0]) } } }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MPRArticle30Report()
        {
            return View();
        }

        [HttpGet]
        [ActionName("GetMPRArticle30Report")]
        public JsonResult GetMPRArticle30Report()
        {
            MPRModel objModel = new MPRModel();

            DataSet ds = objModel.GetMPRArticle30Report();

            return Json(new { Result = new { Data = new { MPRArticle30ReportList = JsonConvert.SerializeObject(ds.Tables[0]) } } }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MPRArticle55Report()
        {
            return View();
        }

        [HttpGet]
        [ActionName("GetMPRArticle55Report")]
        public JsonResult GetMPRArticle55Report()
        {
            MPRModel objModel = new MPRModel();

            DataSet ds = objModel.GetMPRArticle55Report();

            return Json(new { Result = new { Data = new { MPRArticle55ReportList = JsonConvert.SerializeObject(ds.Tables[0]) } } }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MPRArticle57Report()
        {
            return View();
        }

        [HttpGet]
        [ActionName("GetMPRArticle57Report")]
        public JsonResult GetMPRArticle57Report()
        {
            MPRModel objModel = new MPRModel();

            DataSet ds = objModel.GetMPRArticle57Report();

            return Json(new { Result = new { Data = new { MPRArticle57ReportList = JsonConvert.SerializeObject(ds.Tables[0]) } } }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MPRArticle57AReport()
        {
            return View();
        }

        [HttpGet]
        [ActionName("GetMPRArticle57AReport")]
        public JsonResult GetMPRArticle57AReport()
        {
            MPRModel objModel = new MPRModel();

            DataSet ds = objModel.GetMPRArticle57AReport();

            return Json(new { Result = new { Data = new { MPRArticle57AReportList = JsonConvert.SerializeObject(ds.Tables[0]) } } }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MPRArticle57RReport()
        {
            return View();
        }

        [HttpGet]
        [ActionName("GetMPRArticle57RReport")]
        public JsonResult GetMPRArticle57RReport()
        {
            MPRModel objModel = new MPRModel();

            DataSet ds = objModel.GetMPRArticle57RReport();

            return Json(new { Result = new { Data = new { MPRArticle57RReportList = JsonConvert.SerializeObject(ds.Tables[0]) } } }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MPRArticle61Report()
        {
            return View();
        }

        [HttpGet]
        [ActionName("GetMPRArticle61Report")]
        public JsonResult GetMPRArticle61Report()
        {
            MPRModel objModel = new MPRModel();

            DataSet ds = objModel.GetMPRArticle61Report();

            return Json(new { Result = new { Data = new { MPRArticle61ReportList = JsonConvert.SerializeObject(ds.Tables[0]) } } }, JsonRequestBehavior.AllowGet);
        }

        #region "Other Code"

        [HttpGet]
        public ActionResult GccDistrictWiseSummary()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PLDBDistrictWiseReport()
        {
            return View();
        }

        [HttpGet]
        public ActionResult MPRSection25Report()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetComplaintReport(DataPagingModel Details, int DistrictId = 0, string SocietyId = "0", int SearchType = 1)
        {
            MISModel objMISModel = new MISModel(new DBLogger());
            string TotalAmount = "";
            string TotalCSS = "";
            DataTable DT = objMISModel.GetComplaintReport(ref Details, ref TotalAmount, ref TotalCSS, DistrictId, SocietyId, SearchType);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details, TotalAmount = TotalAmount, TotalCSS = TotalCSS }, JsonRequestBehavior.AllowGet);

        }

        #endregion

    }

}