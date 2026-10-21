using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoopDAL;
namespace Cooperative.Controllers
{
    public class VerifyCertificateController : Controller
    {
        // GET: VerifyCertificate
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]      
        public JsonResult GetCertificate(string ApplicationNo)
        {          
            return Json(new { DataList = (new Society()).SearchSociety(ApplicationNo.Replace(" ", "")) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchSociety(string RegistrationNo)
        {
            return Json(new { DataList = (new Society()).SearchSociety(RegistrationNo) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSocietyDetails(string SocietyId)
        {
            return Json(new { DataList = (new Society()).GetSociety(SocietyId) }, JsonRequestBehavior.AllowGet);
        }

    }
}

