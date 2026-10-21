using Cooperative.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cooperative.Models;
using System.Data;

namespace Cooperative.Controllers
{
    [LogActionFilter]
    public class CourtCaseController : Controller
    {
        // GET: CourtCase
        public ActionResult CaseStatus()
        {
            return View();
        }

        public ActionResult CourtCaseUpload()
        {
            return View();
        }

        public ActionResult CourtUserDashBoard()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UpdateLastHearingDate(int Id, string NextHearingDate)
        {
            Court objCourt = new Court();
            DataTable dt = objCourt.UpdateNextHearingDate(Id, Convert.ToDateTime(NextHearingDate));

            int Result = -1;
            if (dt != null && dt.Rows.Count > 0)
            {
                Result = Convert.ToInt32(dt.Rows[0][0]);
            }
            return Json(Result);
        }


    }
}