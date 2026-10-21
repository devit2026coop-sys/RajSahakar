using Cooperative.Filters;
using Cooperative.Models;
using Ionic.Zip;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web.Mvc;
using static Cooperative.Controllers.COMMON_FUNCTION;

namespace Cooperative.Controllers
{
    public class AadhaarEsignAuthController : BaseController
    {
        ApplicationFlowController applicationFlow = new ApplicationFlowController();
        //public ActionResult AadharAuthLanding()
        //{
        //    //if (Request.Form["Data"] != null) { Sp_callTrackingInsert("GetTxnIdOtp  Request.Form[\"data\"] :-", Request.Form["Data"].ToString()); }
        //    //if (Request.Form["Data"] != null && Request.Form["Data"] != "")
        //    //{
        //    //    System.Web.HttpContext.Current.Session["TxnIdDecData"] = Request.Form["Data"];
        //    //    applicationFlow.GenerateAadhaarOTPDec_Test(Request.Form["Data"]);
        //    //    Sp_callTrackingInsert("GetTxnIdOtp  session.Form[\"data\"] :-", System.Web.HttpContext.Current.Session["TxnIdDecData"].ToString());
        //    //}
        //    //else
        //    //{
        //    //    ViewData["TxnIdDecData"] = "NO" + "#";
        //    //}
        //    return View();
        //}

        [ActionName("GetUpdatedEsignTxnId")]
        public  string GetUpdatedEsignTxnId()
        {
            // Get the updated session value
            return System.Web.HttpContext.Current.Session["EsignTxnId"]?.ToString() ?? "No";
        }
        public ActionResult AadharAuthLanding()
        {
            return View();
        }

    }
}