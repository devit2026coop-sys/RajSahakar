using Cooperative.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Cooperative.Controllers
{
    public class VendorRegistrationPaymentController : Controller
    {
        private VendorRegistrationPaymentModel VendorRegistrationPaymentModel;
        public ActionResult Index()
        {
            return View();
        }

        public void StartSNA()
        {
            UsersVendorRegistration("Get_pending_registration_beneficiary");
            UsersVendorCodeVerification("Get_pending_vendorCode_verification");
            UsersVendorPaymentTransaction("Get_pendingPayment_transaction_bill");
            UsersVendorPaymentStatus("Get_pending_Payment_status");
            UsersVendorPaymentRejectStatus("Get_Bills_reject_status");

        }




        #region  VendorRegistration
        public string UsersVendorRegistration(string Name)
        {
            VendorRegistrationPaymentModel objVendorRegistrationPayment = new VendorRegistrationPaymentModel(new DBLogger());
            return (objVendorRegistrationPayment.UsersVendorRegistration(Name)).ToString();

        }
        #endregion

        #region  VendorCodeVerification
        public string UsersVendorCodeVerification(string Name)
        {
            VendorRegistrationPaymentModel objVendorRegistrationPayment = new VendorRegistrationPaymentModel(new FileLogger());
            return (objVendorRegistrationPayment.UsersVendorCodeVerification(Name)).ToString();
        }
        #endregion


        #region  VendorPaymentProcess
        public string UsersVendorPaymentTransaction(string Name)
        {
            VendorRegistrationPaymentModel objVendorRegistrationPayment = new VendorRegistrationPaymentModel(new FileLogger());
            return (objVendorRegistrationPayment.UsersVendorPaymentTransaction(Name)).ToString();
        }
        #endregion

        #region  VendorPaymentStatus
        public string UsersVendorPaymentStatus(string Name)
        {
            VendorRegistrationPaymentModel objVendorRegistrationPayment = new VendorRegistrationPaymentModel(new FileLogger());
            return (objVendorRegistrationPayment.UsersVendorPaymentStatus(Name)).ToString();
        }
        #endregion

        #region  VendorPaymentRejectStatus
        public string UsersVendorPaymentRejectStatus(string Name)
        {
            VendorRegistrationPaymentModel objVendorRegistrationPayment = new VendorRegistrationPaymentModel(new FileLogger());
            return (objVendorRegistrationPayment.UsersVendorPaymentRejectStatus(Name)).ToString();
        }
        #endregion


    }
}