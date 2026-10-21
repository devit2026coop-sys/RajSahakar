using System;
using System.Collections.Generic;
using System.Linq;
using Cooperative.Filters;
using Cooperative.Models;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Cooperative.Controllers
{
    public class FAQController : Controller
    {
        // GET: FAQ



        #region FAQ


        public ActionResult FAQ()
        {
            return View();
        }


        [HttpPost]
        public ActionResult InsertUpdateFAQ(FAQModel Details)
        {
            FAQModel objLoan = new FAQModel(new DBLogger());
            string result = objLoan.InsertUpdateFAQ(Details);
            return Json(new { Status = result }, JsonRequestBehavior.AllowGet);
        }

        
        public string GetFAQList(string WhereCase = "")
        {
            string WhereClause = "";            
            //if (WhereCase != "")
            //{
            //    WhereClause += "  and IsActive=1 and Id = " + WhereCase;
            //}
            FAQModel objLoan = new FAQModel(new DBLogger());
            DataTable DT = objLoan.GetFAQList(WhereClause);
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        }


        [HttpPost]
        [ActionName("DeleteFAQ")]
        public JsonResult DeleteFAQ(int Id, int ActionId)
        {
            ActionId = (ActionId == 0 ? 1 : (ActionId == 1 ? 0 : ActionId));

            FAQModel faqModel = new FAQModel(new FileLogger());
            return Json(faqModel.DeleteFAQ(Id, ActionId), JsonRequestBehavior.AllowGet);
        }

        #endregion FAQ
    }
}