using Cooperative.Filters;
using Cooperative.Models;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Cooperative.Controllers
{
    [Error]
    [LogActionFilter]
    [CustomAuthentication]
    public class SocietyMemberRegistrationController : BaseController
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

        [HttpPost]
        public string GetOrganizationList()
        {
            SocietyMemberRegistrationModel objHomeModel = new SocietyMemberRegistrationModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objHomeModel.GetOrganizationList());
        }

        [HttpPost]
        public string SaveOrganizationUserRegistrationDetails(OrganizationUserRegistrationEntity _Entity)
        {
            SocietyMemberRegistrationModel objHomeModel = new SocietyMemberRegistrationModel(new DBLogger());
            _Entity.KIOSKID = _LoggedInUser.KIOSKCODE;
            _Entity.SSOID = _LoggedInUser.SSOID;
            _Entity.CreatedBy = _LoggedInUser.UserId;
            _Entity.UserType = _LoggedInUser.UserType;

            int _RequestId = -1;
            //DataTable dt = objHomeModel.SaveOrganizationUserDetails(_Entity, ref _RequestId);



            int Result = objHomeModel.SaveOrganizationUserDetails(_Entity, ref _RequestId);


            //return COMMON_FUNCTION.DataTableToJSONString(dt);
            return Result.ToString();

        }

        [HttpPost]
        public string SaveOrganizationUserRegistrationDetails_old(OrganizationUserRegistrationEntity _Entity)
        {
            SocietyMemberRegistrationModel objHomeModel = new SocietyMemberRegistrationModel(new DBLogger());
            _Entity.KIOSKID = _LoggedInUser.KIOSKCODE;
            _Entity.SSOID = _LoggedInUser.SSOID;
            _Entity.CreatedBy = _LoggedInUser.UserId;
            _Entity.UserType = _LoggedInUser.UserType;
            int _RequestId = -1;
            DataTable dt = new DataTable(); //objHomeModel.SaveOrganizationUserDetails(_Entity, ref _RequestId);

            if (_RequestId > 0)
            {
                SocietyMemberRegistrationModel obj = new SocietyMemberRegistrationModel(new DBLogger());
                DataTable RegisteredMemberData = new DataTable();
                RegisteredMemberData = obj.GetRegisteredMemberDetailByRequestId(_RequestId);

                if (RegisteredMemberData != null && RegisteredMemberData.Rows.Count > 0)
                {
                    var P = RegisteredMemberData.Rows[0];
                    //string Message = "प्रिय सदस्य "+ Convert.ToString(P["Name"]) + " आपकी सदस्यता पंजीकरण प्रक्रिया प्रारम्भ हो चुकी है| आपका सदस्यता क्रमांक " + Convert.ToString(P["ApplicationNo"]) + " है.";
                    string Message = "Dear Member " + Convert.ToString(P["Name"]) + ", member registration process has been started| Your member registration Id is " + Convert.ToString(P["ApplicationNo"]);
                    EsanchaarController EC = new EsanchaarController();
                    string ReturnVal = EC.SendSocietySMS(Message, Convert.ToString(P["MobileNo"]));
                }
            }



            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpPost]
        public string SavePLDBUserRegistrationDetails(OrganizationUserRegistrationEntity _Entity)
        {
            SocietyMemberRegistrationModel objHomeModel = new SocietyMemberRegistrationModel(new DBLogger());
            _Entity.KIOSKID = _LoggedInUser.KIOSKCODE;
            _Entity.SSOID = _LoggedInUser.SSOID;
            _Entity.CreatedBy = _LoggedInUser.UserId;
            _Entity.UserType = _LoggedInUser.UserType;
            _Entity.AADHARNo = csCommon.Decrypt_SA(Uri.UnescapeDataString(_Entity.AADHARNo));
            int _RequestId = -1;
            //DataTable dt = objHomeModel.SavePLDBUserDetails(_Entity, ref _RequestId);
            int Result = objHomeModel.SavePLDBUserDetails(_Entity, ref _RequestId);



            //string ApplicationNo = _RequestId.ToString();
            //string MembershipNo = _RequestId.ToString();



            //if (_RequestId > 0)
            //{
            //    SocietyMemberRegistrationModel obj = new SocietyMemberRegistrationModel(new DBLogger());
            //    DataTable RegisteredMemberData = new DataTable();
            //    RegisteredMemberData = obj.GetRegisteredMemberDetailByRequestId(_RequestId);



            //    if (RegisteredMemberData != null && RegisteredMemberData.Rows.Count > 0)
            //    {
            //        var P = RegisteredMemberData.Rows[0];
            //        //string Message = "प्रिय सदस्य "+ Convert.ToString(P["Name"]) + " आपकी सदस्यता पंजीकरण प्रक्रिया प्रारम्भ हो चुकी है| आपका सदस्यता क्रमांक " + Convert.ToString(P["ApplicationNo"]) + " है.";
            //        string Message = "Dear Member " + Convert.ToString(P["Name"]) + ", member registration process has been started| Your member registration Id is " + Convert.ToString(P["MembershipNo"]);
            //        ApplicationNo = Convert.ToString(P["ApplicationNo"]);
            //        MembershipNo = Convert.ToString(P["MembershipNo"]);
            //        EsanchaarController EC = new EsanchaarController();
            //        string ReturnVal = EC.SendSocietySMS(Message, Convert.ToString(P["MobileNo"]));
            //    }
            //}





            return Result.ToString();
        }

        [HttpPost]
        public string SaveSocietyMemberRegiApproval(int RequestId, int ActionStatus, string ActionRemark)
        {
            SocietyMemberRegistrationModel objHomeModel = new SocietyMemberRegistrationModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objHomeModel.SaveSocietyMemberRegiApproval(RequestId, ActionStatus, ActionRemark, _LoggedInUser.SSOID));
        }

        [HttpGet]
        public string GetOrganizationUserRegistrationDetails(int Typeid, string Searchvalue, string ApplicationIdEnc)
        {
            string WhereClause = "";
            DataTable DT = null;
            SocietyMemberRegistrationModel objHomeModel = new SocietyMemberRegistrationModel(new DBLogger());
            string finaldata = "";
            PGResponse objPGResponse = new PGResponse();
            DResponse objDResponse = new DResponse();



            if (ApplicationIdEnc != null && ApplicationIdEnc != "" && ApplicationIdEnc != "undefined")
            {
                ApplicationIdEnc = ApplicationIdEnc.Replace(' ', '+');
                ApplicationIdEnc = ApplicationIdEnc.Replace(' ', '+');
                ApplicationIdEnc = ApplicationIdEnc.Replace(' ', '+');



                WhereClause = " AND A.IsActive = 1 and RequestId = " + COMMON_FUNCTION.Decrypt(ApplicationIdEnc);
                DT = (objHomeModel.GetOrganizationUserRegistrationDetails(WhereClause, Typeid, Searchvalue));



                foreach (DataRow dr in DT.Rows) // search whole table
                {
                    if (dr["RequestId"].ToString().Length > 0)
                    {
                        dr["RequestIdEnc"] = COMMON_FUNCTION.Encrypt(dr["RequestId"].ToString());
                    }
                }



                if (Convert.ToInt32(DT.Rows[0]["ApplicationTypeId"]) == 2)
                {
                    if (_LoggedInUser.UserType == "C")
                        objPGResponse = JsonConvert.DeserializeObject<PGResponse>(DT.Rows[0]["ResponseString"].ToString());
                    else
                        objDResponse = JsonConvert.DeserializeObject<DResponse>(DT.Rows[0]["ResponseString"].ToString());



                    finaldata = "{";
                    finaldata += "\"SocietyMemberRegistration\":" + JsonConvert.SerializeObject(DT, Formatting.Indented);



                    if (_LoggedInUser.UserType == "C")
                        finaldata += ",\"PGResponse\":" + JsonConvert.SerializeObject(objPGResponse, Formatting.Indented);
                    else
                        finaldata += ",\"PGResponse\":" + JsonConvert.SerializeObject(objDResponse, Formatting.Indented);



                    finaldata += "}";
                }
                else
                {
                    int _requestId = Convert.ToInt32(DT.Rows[0]["RequestId"]);
                    finaldata = "{";



                    if (_LoggedInUser.UserType == "C")
                        objPGResponse = JsonConvert.DeserializeObject<PGResponse>(DT.Rows[0]["ResponseString"].ToString());
                    else
                        objDResponse = JsonConvert.DeserializeObject<DResponse>(DT.Rows[0]["ResponseString"].ToString());



                    DT.Rows[0]["ResponseString"] = "";
                    finaldata += "\"SocietyMemberRegistration\":" + COMMON_FUNCTION.DataTableToJSONString(DT);
                    finaldata += ",\"SocietyMemberLandDetail\":" + COMMON_FUNCTION.DataTableToJSONString(new SocietyMemberRegistrationModel(new DBLogger()).GetSocietyRegisteredMeberDetailsByRequestId(_requestId, 1));
                    finaldata += ",\"SocietyMemberSownCropDetail\":" + COMMON_FUNCTION.DataTableToJSONString(new SocietyMemberRegistrationModel(new DBLogger()).GetSocietyRegisteredMeberDetailsByRequestId(_requestId, 2));
                    finaldata += ",\"SocietyMemberOtherBankLoanDetail\":" + COMMON_FUNCTION.DataTableToJSONString(new SocietyMemberRegistrationModel(new DBLogger()).GetSocietyRegisteredMeberDetailsByRequestId(_requestId, 3));
                    if (_LoggedInUser.UserType == "C")
                        finaldata += ",\"PGResponse\":" + JsonConvert.SerializeObject(objPGResponse, Formatting.Indented);
                    else
                        finaldata += ",\"PGResponse\":" + JsonConvert.SerializeObject(objDResponse, Formatting.Indented);



                    finaldata += "}";
                }
            }
            else if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
            {
                if (Request.Cookies["SSOROLE"].Value == "CITIZEN")
                {
                    WhereClause = " AND A.SSOID = '" + Request.Cookies["SSO_ID"].Value + "'";
                }
                DT = (objHomeModel.GetOrganizationUserRegistrationDetails( WhereClause, Typeid, Searchvalue));




                foreach (DataRow dr in DT.Rows) // search whole table
                {
                    if (dr["RequestId"].ToString().Length > 0)
                    {
                        dr["RequestIdEnc"] = COMMON_FUNCTION.Encrypt(dr["RequestId"].ToString());
                    }
                }



                finaldata = JsonConvert.SerializeObject(DT, Formatting.Indented);
            }



            return finaldata;




            //return COMMON_FUNCTION.DataTableToJSONString(DT);





        }
        [HttpPost]
        public string ChangeOrganizationUserStatus(string UserId, string IsActive)
        {
            int _UserId = 0;
            bool _IsActive = false;

            if (!(int.TryParse(UserId, out _UserId) && bool.TryParse(IsActive, out _IsActive)))
                return "[]";

            SocietyMemberRegistrationModel objHomeModel = new SocietyMemberRegistrationModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(objHomeModel.ChangeOrganizationUserStatus(_UserId, _IsActive));

        }

        [HttpGet]
        public string GetSocietyMemberRequestData()
        {
            DataTable DT = null;
            SocietyMemberRegistrationModel objHomeModel = new SocietyMemberRegistrationModel(new DBLogger());

            DT = (objHomeModel.GetSocietyMemberRequestData(_LoggedInUser.SSOID));
            foreach (DataRow dr in DT.Rows)
            {
                if (dr["RequestId"].ToString().Length > 0)
                {
                    dr["RequestIdEnc"] = COMMON_FUNCTION.Encrypt(dr["RequestId"].ToString());
                }
            }
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }

        [HttpGet]
        public string GetPLDBUserRegistrationDetails(string ApplicationIdEnc)
        {
            string WhereClause = "";
            DataTable DT = null;
            SocietyMemberRegistrationModel objHomeModel = new SocietyMemberRegistrationModel(new DBLogger());
            string finaldata = "";
            if (ApplicationIdEnc != null && ApplicationIdEnc != "" && ApplicationIdEnc != "undefined")
            {
                ApplicationIdEnc = ApplicationIdEnc.Replace(' ', '+');
                ApplicationIdEnc = ApplicationIdEnc.Replace(' ', '+');
                ApplicationIdEnc = ApplicationIdEnc.Replace(' ', '+');

                WhereClause = " AND A.IsActive = 1 and RequestId = " + COMMON_FUNCTION.Decrypt(ApplicationIdEnc);
                DT = (objHomeModel.GetPLDBUserRegistrationDetails(WhereClause));

                foreach (DataRow dr in DT.Rows) // search whole table
                {
                    if (dr["RequestId"].ToString().Length > 0)
                    {
                        dr["RequestIdEnc"] = COMMON_FUNCTION.Encrypt(dr["RequestId"].ToString());
                    }
                }

                finaldata = JsonConvert.SerializeObject(DT, Formatting.Indented);
            }
            else if (Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null) //Request.Cookies["RAJSSO"] != null &&
            {
                if (Request.Cookies["SSOROLE"].Value == "CITIZEN")
                {
                    WhereClause = " AND A.SSOID = '" + Request.Cookies["SSO_ID"].Value + "'";
                }
                DT = (objHomeModel.GetPLDBUserRegistrationDetails(WhereClause));


                foreach (DataRow dr in DT.Rows) // search whole table
                {
                    if (dr["RequestId"].ToString().Length > 0)
                    {
                        dr["RequestIdEnc"] = COMMON_FUNCTION.Encrypt(dr["RequestId"].ToString());
                    }

                    if (dr["ApplicationNo"].ToString().Length > 0)
                    {
                        dr["ApplicationEnc"] = COMMON_FUNCTION.Encrypt(dr["ApplicationNo"].ToString());
                    }
                }

                finaldata = JsonConvert.SerializeObject(DT, Formatting.Indented);
            }

            return finaldata;
        }

        public ActionResult MemberRegObjectionResponse(string applicationNo = "")
        {
            ViewBag.Usertype = _LoggedInUser.UserType;
            //EsanchaarController EC = new EsanchaarController(); //string ReturnVal = EC.SendSocietySMS("Total Application For objection received at PLDB Admin : 1", "8058828693");
            return View();
        }

        [HttpPost]
        public string GetRegisteredUserDetailByApplicationNoCitizen(string ApplicationNo)
        {
            string WhereClause = " AND A.ApplicationNo='" + ApplicationNo + "'"; //COMMON_FUNCTION.Decrypt(ApplicationNo)
            return this.GetRegisteredUserDetailByApplicationNo(WhereClause);
        }

        [HttpPost]
        public string GetRegisteredUserDetailByApplicationNoKiosk(string ApplicationNo)
        {
            string WhereClause = " AND A.ApplicationNo='" + ApplicationNo + "'";
            return this.GetRegisteredUserDetailByApplicationNo(WhereClause);
        }

        public string GetRegisteredUserDetailByApplicationNo(string WhereClause)
        {
            SocietyMemberRegistrationModel objHomeModel = new SocietyMemberRegistrationModel(new DBLogger());
            return JsonConvert.SerializeObject((objHomeModel.GetObjectedFormData(WhereClause)), Formatting.Indented);
        }


        [HttpPost]
        public string SendResponseOnMembershipObjection(OrganizationUserRegistrationEntity _Entity)
        {
            //_Entity.RequestIdEnc = _Entity.RequestIdEnc.Replace(' ', '+');
            //_Entity.RequestIdEnc = _Entity.RequestIdEnc.Replace(' ', '+');
            //_Entity.RequestIdEnc = _Entity.RequestIdEnc.Replace(' ', '+');

            _Entity.KIOSKID = _LoggedInUser.KIOSKCODE;
            _Entity.SSOID = _LoggedInUser.SSOID;
            _Entity.CreatedBy = _LoggedInUser.UserId;
            _Entity.UserType = _LoggedInUser.UserType;
            // _Entity.RequestId = Convert.ToInt32(COMMON_FUNCTION.Decrypt(_Entity.RequestIdEnc));

            SocietyMemberRegistrationModel objMemberRegistrationModel = new SocietyMemberRegistrationModel(new DBLogger());
            int _Response = objMemberRegistrationModel.UpdateMemeberDetails(_Entity);

            string ReturnVal = string.Empty;
            if (_Response > 0)
            {
                SocietyMemberRegistrationModel obj = new SocietyMemberRegistrationModel(new DBLogger());
                DataTable RegisteredMemberData = new DataTable();
                RegisteredMemberData = obj.GetRegisteredMemberDetailByRequestId(_Entity.RequestId);

                if (RegisteredMemberData != null && RegisteredMemberData.Rows.Count > 0)
                {
                    var P = RegisteredMemberData.Rows[0];
                    //string Message = "प्रिय सदस्य "+ Convert.ToString(P["Name"]) + " आपकी सदस्यता पंजीकरण प्रक्रिया प्रारम्भ हो चुकी है| आपका सदस्यता क्रमांक " + Convert.ToString(P["ApplicationNo"]) + " है.";
                    string Message = "Dear Member " + Convert.ToString(P["Name"]) + ", your application response (Application No.: " + Convert.ToString(P["ApplicationNo"] + ")  has been sent successfully to the concerned officer.");
                    EsanchaarController EC = new EsanchaarController();
                    ReturnVal = EC.SendSocietySMS(Message, Convert.ToString(P["MobileNo"]));
                }
            }

            return "{\"Response\":" + _Response + ",\"SMSMessage\":\"" + ReturnVal + "\"}";
        }

        [HttpPost]
        public string GetTransactionLogByRequestId(string RequestIdEnc)
        {
            RequestIdEnc = RequestIdEnc.Replace(' ', '+');
            RequestIdEnc = RequestIdEnc.Replace(' ', '+');
            RequestIdEnc = RequestIdEnc.Replace(' ', '+');

            RequestIdEnc = COMMON_FUNCTION.Decrypt(RequestIdEnc);
            SocietyMemberRegistrationModel obj = new SocietyMemberRegistrationModel(new DBLogger());
            DataTable dt = new DataTable();
            dt = obj.GetRegisteredMemberDetailByRequestId(Convert.ToInt32(RequestIdEnc));

            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpPost]
        public string SendOTPForMembershipCertificate()
        {
            DataTable dt = null;
            if (_LoggedInUser.AadharNo.Length > 0)
            {
                dt = new DataTable();
                dt.Columns.Add("AadharNo", typeof(string));
                dt.Columns.Add("txnid", typeof(string));
                DataRow dtrow = dt.NewRow();
                dtrow["AadharNo"] = _LoggedInUser.AadharNo;
                dtrow["txnid"] = "";
                dt.Rows.Add(dtrow);
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                string _txnid = string.Empty;
                _txnid =  COMMON_FUNCTION.GenerateOTPForEsingh(_LoggedInUser.AadharNo);
                dt.Rows[0]["txnid"] = _txnid;
            }
            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpPost]
        public string ValidateOTPForMembershipGenerate(string otp, string txn, string _RequestIdEnc)
        {
            DataTable dt = null;

            dt = new DataTable();
            dt.Columns.Add("AadharNo", typeof(string));
            dt.Columns.Add("txnid", typeof(string));
            dt.Columns.Add("PhotoPath", typeof(string));
            DataRow dtrow = dt.NewRow();
            dtrow["AadharNo"] = _LoggedInUser.AadharNo;
            dtrow["txnid"] = "";
            dt.Rows.Add(dtrow);


            var otpResult =  COMMON_FUNCTION.VerifyAadhaarOTP(otp, txn);
            if (otpResult.status != "1")
            {
                return otpResult.data;
            }


            DataTable dtb = new DataTable();
            if (otp != null && txn != null && dt.Rows.Count > 0)
            {
                //_RequestIdEnc = _RequestIdEnc.Replace(' ', '+');
                //_RequestIdEnc = _RequestIdEnc.Replace(' ', '+');
                //_RequestIdEnc = _RequestIdEnc.Replace(' ', '+');

                string id = COMMON_FUNCTION.Decrypt(_RequestIdEnc);
                SocietyMemberRegistrationModel obj = new SocietyMemberRegistrationModel(new DBLogger());
                DataTable RegisteredMemberData = new DataTable();
                RegisteredMemberData = obj.GetRegisteredMemberDetailByRequestId(Convert.ToInt32(id));

                if (RegisteredMemberData != null && RegisteredMemberData.Rows.Count > 0)
                {
                    var firstRow = RegisteredMemberData.Rows[0];
                    var Result = new StatusReport() { };// GenerateMembershipCertificate(firstRow);
                    if (Result.Status)
                    {
                        // Update Membership Certificate in DB
                        //COMMON_FUNCTION.VerifyAadhaarOTPForEsingh(otp, txn, Server.MapPath("~/Content/SocietyMember/" + Result.FileName), true, Result.FileName);

                        var result = COMMON_FUNCTION.EsinghPDF(otpResult.data, Server.MapPath("~/Content/SocietyMember/" + Result.FileName));
                        //succmsg = result.data;


                        obj.SaveMembershipCertificate(Convert.ToInt32(id), Result.FileName);
                        string Message = "Dear Member " + Convert.ToString(firstRow["Name"]) + " Please find registration status as:  Request No.: " + Convert.ToString(firstRow["ApplicationNo"]) + " Status: " + Convert.ToString(firstRow["ApplicationStatus"]) + " Remark: " + Convert.ToString(firstRow["ActionRemark"]);
                        EsanchaarController EC = new EsanchaarController();
                        string ReturnVal = EC.SendSocietySMS(Message, Convert.ToString(firstRow["MobileNo"]));
                    }
                }

                //dtb = COMMON_FUNCTION.ValidateOTP(txn, _LoggedInUser.AadharNo, otp);
                //if (dtb.Rows.Count > 0)
                //{
                //    if (dtb.Rows[0]["Column1"].ToString() == "Success")
                //    {

                //        CitizenModel ObjCitizenModel = new CitizenModel(new DBLogger());
                //        if (Convert.ToInt32(id) > 0)
                //        {

                //        }
                //    }
                //    else
                //    {
                //        List<string[]> listx = new List<string[]>();
                //        listx.Add(new string[] { "Failed", "AADHAR No Verified", "" });
                //        dtb = ConvertListToDataTable(listx);
                //    }
                //}
            }
            else
            {
                List<string[]> listx = new List<string[]>();
                listx.Add(new string[] { "Failed", "Invalid OTP ", otp }); /*COMMON_FUNCTION.Encrypt(_AadharNo)*/
                dtb = ConvertListToDataTable(listx);
            }
            return COMMON_FUNCTION.DataTableToJSONString(dtb);
        }

        public StatusReport GenerateMembershipCertificate(DataTable dt,string TXN) //
        {
           

            StatusReport _StatusReport = new StatusReport() { FileName = string.Empty, Status = false, StatusDescription = string.Empty };
            try
            {
                //var P = dr;

                ElectionModel objElectionModel = new ElectionModel(new DBLogger());
                // StringBuilder sb = new StringBuilder();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                //sb.Append("<html><body><div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px;' ></div><br /><div id='divtoPrint'>");
                //sb.Append("<div style='padding-right:25px; margin-right:25px; padding-left:25px; margin-left:25px; '>");
                //sb.Append("<table style='width: 100% ;font-size: 23px;font-weight: normal;' border='0' cellspacing='0' cellpadding='0'>");

                //sb.Append("<tr>");
                //sb.Append("<td style='text-align:center;'>");
                //sb.Append(" यह प्रमाणित किया जाता है कि श्री " +  Convert.ToString(P["Name"]) + " "+ Convert.ToString(P["DISTRICT_MANGAL"]) + " भूमि विकास बैंक " + Convert.ToString(P["BLOCK_MANGAL"]) + " के सम्मानित सदस्य हैं । सदस्य पंजीकरण संख्या " + Convert.ToString(P["MembershipNo"]) +" है ");
                //sb.Append("</td></tr>");

                //sb.Append("</table></div><br /></div></body></html>");


                string _FileName = DateTime.Now.ToString().Replace("/", string.Empty).Replace("-", string.Empty) + ".pdf";
                string devFontSize = "17px";
                string branch = "";
                    string fontSize = "font-size: 13px;";
                for (int i = 0; i < 1; i++)
                {
                    _FileName = dt.Rows[0]["MembershipNo"].ToString()+".pdf";
                    branch = dt.Rows[0]["BLOCK_ENG"].ToString();

                    sb.Append("<div style='padding-left:20px;padding-right:20px;padding-top:50px'>");
                    sb.Append("<table style='width: 100%;border:1px solid;height: 100%; margin-left:50px;margin-top:50px;margin-right:50px'>");
                    sb.Append("<tr><td style='padding: 15px 15px 0px 15px; text-align: center; height: 109px'>");
                    sb.Append("<img style='width:100px;' src='" + Server.MapPath("~/Content/Images/cert_form_logo.png") + "'/>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td style='text-align:center;font-size:20px;'><b>" + UnicodeToKrutidev.GetKrutidev("राजस्थान सरकार") + "  </b></td></tr>");
                    sb.Append("<tr><td style='padding: 10px 5px 10px 5px; text-align: center; height: 30px;'><b><label style='font-size:25px;'> " + UnicodeToKrutidev.GetKrutidev("प्राथमिक सहकारी भूमि विकास बैंक  " + dt.Rows[0]["PLDBNameHi"].ToString()+ ", शाखा - " + dt.Rows[0]["BLOCK_MANGAL"].ToString()) + "</label></b>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td style='padding: 8px 35px 0px 15px; text-align: center;  height: 20px;'>");
                    sb.Append("<b><label style='font-size:22px;'>" + UnicodeToKrutidev.GetKrutidev(" प्रमाण पत्र ") + "</label></b>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align: left; padding: 10px 15px 10px 15px; text-align: center; height: 20px;'><br/>");
                    sb.Append("<table style='width:100%'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'> सदस्यता संख्या - <b> " + dt.Rows[0]["MembershipNo"].ToString() + "</b></td>");
                    sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> दिनांक - " + DateTime.Now.ToString("dd-MM-yyyy") + " </td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='width:49%;text-align:left;" + fontSize + "'> </td>");
                    sb.Append("<td style='width:49%;text-align:right;" + fontSize + "'> </td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    //sb.Append("<tr>");
                    //sb.Append("<td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'> यह प्रमाणित किया जाता है कि  <b> श्री / श्रीमती " + Convert.ToString(dt.Rows[0]["Name"].ToString()) + " </b>  पिता / पति   <b> श्री " + Convert.ToString(dt.Rows[0]["FatherHusbandName"].ToString()) + " </b> पता <b>" + Convert.ToString(dt.Rows[0]["Address"].ToString()) + "</b>  को इस बैंक द्वारा " + "100 /-रु (सौ रुपये )" + " प्रति हिस्सा मूल्य के.......................................... हिस्से  क्रम संख्या.............." + Convert.ToString(dt.Rows[0]["PldbId"].ToString()) + "......................... से.............................................. तक मूल्य पूर्णतया चूकते मूल्य के तत्सम्बन्धी उपनियमो के अधीन माह......................................  वर्ष..........................................  में  क्रम किये  गए हैं। हिस्सा  प्रमाण - पत्र  आज  दिनांक " + DateTime.Now.ToString("dd/MM/yyyy") + " को इस संस्था की सामान्य मुद्रा एवं हस्ताक्षर के जारी  किया गया।");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");


                    sb.Append("<tr>");
                    sb.Append("<td style='padding: 15px; line-height: 30px; height: 100px;" + fontSize + "'> यह प्रमाणित किया जाता है कि  <b> श्री / श्रीमती  " + Convert.ToString(dt.Rows[0]["Name"].ToString()) + " </b>  पुत्र   <b> श्री " + Convert.ToString(dt.Rows[0]["FatherHusbandName"].ToString()) + " </b>  पता <b>" + Convert.ToString(dt.Rows[0]["Address"].ToString()) + "</b> भूमि का खातेदार कास्तकार है और मेने इसकी राजस्व रिकॉर्ड  से जाँच कर ली है । <b>श्री " + Convert.ToString(dt.Rows[0]["Name"].ToString()) + "  </b> बैंक के उपनियमनुसार सदस्य बनने की योग्यता रखते है और इस हेतु इन्होने दिनांक <b> "+ dt.Rows[0]["MembershipDate"].ToString() + " </b>को <b>    </b>हिस्से का मूल्य<b> 100 </b>रु व प्रवेश शुल्क 10 रुपया जरिये रसीद संख्या<b> " + dt.Rows[0]["ApplicationNo"].ToString() + " </b>जमा करा दिए है।");
                    sb.Append("</td>");
                    sb.Append("</tr>");



                    //sb.Append("</table>");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");
                    sb.Append("<tr><td style='padding: 60px 15px 10px 35px; text-align: right;'><br/><br/><br/><br/><br/><br/><br/></td></tr>");
                    sb.Append("</table>");
                    sb.Append("</div>");

                    //sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<td colspan='2'>");
                    //sb.Append("<p style='text-align:left;padding-left:50px;padding-top:20px'>");
                    //sb.Append("प्रमाणित  किया  जाता  है की श्री ......................................... " + Convert.ToString(dt.Rows[0]["Name"].ToString()) + " .................................................... ");
                    //sb.Append("</p> ");
                    //sb.Append("<p style='text-align:left;padding-left:20px;'>");
                    //sb.Append("पिता श्री/श्रीमती................................................................... " + Convert.ToString(dt.Rows[0]["FatherHusbandName"].ToString())+" ................................................................................................................................ ");
                    //sb.Append("</p>");
                    //sb.Append("<p style='text-align:left;padding-left:20px;'>");
                    //sb.Append("पता " + Convert.ToString(dt.Rows[0]["Address"].ToString()) + "  को इस बैंक द्वारा " + "100 /-रु (सौ रुपये )" + " प्रति हिस्सा ");
                    //sb.Append("</p>");
                    //sb.Append("<p style='text-align:left;padding-left:20px;'>");
                    //sb.Append("मूल्य के.......................................... हिस्से  क्रम संख्या.............."+ Convert.ToString(dt.Rows[0]["PldbId"].ToString()) + "......................... से.............................................. तक मूल्य पूर्णतया ");
                    //sb.Append("</p>      ");
                    //sb.Append("<p style='text-align:left;padding-left:20px;'>");
                    //   sb.Append("चूकते मूल्य के तत्सम्बन्धी उपनियमो के अधीन माह......................................  वर्ष..........................................  में  क्रम किये  गए हैं |");
                    //sb.Append("</p>  ");
                    //sb.Append("<p style='text-align:left;padding-left:50px;'>");
                    //sb.Append("हिस्सा  प्रमाण - पत्र  आज  दिनांक " + DateTime.Now.ToString("dd") + " माह " + DateTime.Now.ToString("MM") + " सन " + DateTime.Now.ToString("yyyy") + " को इस संस्था की  सामान्य मुद्रा");
                    //sb.Append("</p>   ");
                    //sb.Append("<p colspan='2' style='text-align:left;padding-bottom:30px;padding-left:20px;'> एवं हस्ताक्षर के जारी  किया गया | ");
                    //sb.Append("</p>");
                    //sb.Append("</td>");
                    //sb.Append("</tr>");
                    ////sb.Append("<tr>");
                    ////sb.Append("<td style='text-align:right;padding-bottom:20px;'> सचिव</td>");
                    ////sb.Append("<td style='text-align:right;padding-bottom:20px;padding-right:20px;'>अध्यक्ष / प्रशासक</td>");
                    ////sb.Append("</tr>");
                    //sb.Append("</table>");
                    //sb.Append("</div>");
                    //sb.Append("</div>");
                    //sb.Append("</div> ");
                    //sb.Append("</body>");
                    //sb.Append("</html> ");
                }
                string InpuData = sb.ToString();

                //string PageType = "Portrait";
                //var pdfDoc = new NReco.PdfGenerator.HtmlToPdfConverter(); //created an object of HtmlToPdfConverter class.
                //if (string.IsNullOrEmpty(PageType))
                //    pdfDoc.Orientation = PageOrientation.Default;  //setting orientation.
                //else
                //{
                //    if (PageType == "Landscape")
                //        pdfDoc.Orientation = PageOrientation.Landscape;
                //    else
                //        pdfDoc.Orientation = PageOrientation.Portrait;
                //}

                //pdfDoc.Size = NReco.PdfGenerator.PageSize.A4;   //8.27 in × 11.02 in //Page Size
                //PageMargins pageMargins = new PageMargins();     //Margins in mm
                //pageMargins.Bottom = 05;
                //pageMargins.Left = 05;
                //pageMargins.Right = 05;
                //pageMargins.Top = 05;
                //pdfDoc.Margins = pageMargins;                      //margins added to PDF.

                //pdfDoc.PageHeaderHtml = " ";

                //var htmlContent = InpuData;
                //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                //UserModel OBJUser = new UserModel(new FileLogger());
                //pdfDoc.PageFooterHtml = "<div style='float:right;'>  Page <span class='page'></span> of  <span class='topage'></span></div>";
                //var pdfBytes = p1dfDoc.GeneratePdf(htmlContent);

               
                //string _FileName = Convert.ToString(P["ApplicationNo"]).Replace("/", string.Empty) + ".pdf";
                // System.IO.File.WriteAllBytes(Server.MapPath("~/Content/SocietyMember/" + _FileName), pdfBytes);
                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                //sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>")));
                sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(sb.ToString().Replace("<br>", "<br/>"), true, devFontSize));
                _FileName=_FileName.Replace("/", "");
                if (!System.IO.Directory.Exists(Server.MapPath("~/Content/SocietyMember")))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Content/SocietyMember"));
                }
               _StatusReport.Status = COMMON_FUNCTION.GeneratePDF(sb1, Server.MapPath("~/Content/SocietyMember/" + _FileName));
                if (_StatusReport.Status)
                {
                    _StatusReport.Status = false;
                   var esignResult = COMMON_FUNCTION.EsinghPDF(TXN, Server.MapPath("~/Content/SocietyMember/" + _FileName), branch, "Branch Secretary", "350", "220", "150", "220");
                    if (esignResult.status == "1")
                    {
                        _StatusReport.Status = true;
                    }
                }
                
                _StatusReport.FileName = _FileName;
            }
            catch(Exception ex)
            {
                _StatusReport.Status = false;
            }
            return _StatusReport;
        }

        public static DataTable ConvertListToDataTable(List<string[]> list)
        {
            // New table.
            DataTable table = new DataTable();
            // Get max columns.
            int columns = 0;
            foreach (var array in list)
            {
                if (array.Length > columns)
                {
                    columns = array.Length;
                }
            }
            // Add columns.
            for (int i = 0; i < columns; i++)
            {
                table.Columns.Add();
            }
            // Add rows.
            foreach (var array in list)
            {
                table.Rows.Add(array);
            }
            return table;
        }

        [HttpGet]
        public ActionResult PLDBMemberRegistration()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            ViewBag.usertype = _LoggedInUser.UserType;
            return View();
        }

        [HttpPost]
        public string GenerateOTPForPLDOTPBMemberRegistration(string _AadharNo)
        {
            _AadharNo = csCommon.Decrypt_SA(Uri.UnescapeDataString(_AadharNo));
            //_AadharNo = "428434778643";
            if ((UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin] == null)
                return "LOGOUT";

            string result = AADHARSerice.SendOTPOnAADHAR(_AadharNo);
            return result;
        }

        [HttpPost]
        public string ValidateOTPForPLDBMemberRegistration(string otp, string txn, string _AadharNo)
        {
            if ((UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin] == null)
                return "LOGOUT";

            var response = AADHARSerice.VeryifyOTPsentOnAADHAR(otp, txn, _AadharNo);
            dynamic ResponseData = new JavaScriptSerializer().Deserialize<dynamic>(response);
            bool IsVerified = Convert.ToBoolean(ResponseData["Result"]);

            if (IsVerified)
            {
                string folderpath = "../Content/NewRegistration";
                if (!Directory.Exists(Server.MapPath(folderpath)))
                    Directory.CreateDirectory(Server.MapPath(folderpath));


                string fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper();
                byte[] imageBytes = Convert.FromBase64String(ResponseData["AADHARDetails"][0]["Column5"]);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

                fileName = COMMON_FUNCTION.GetUniqueFileName("Member_" + fileName, System.Web.HttpContext.Current.Server.MapPath("../Content/NewRegistration/"), ".jpg").ToLower();
                FileStream fs = new FileStream(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/NewRegistration/"), fileName + ".jpg"), FileMode.CreateNew, FileAccess.Write);
                ms.WriteTo(fs);
                fs.Close();
                fs.Dispose();
                ms.Flush();
                ResponseData["AADHARDetails"][0]["Column5"] = "../Content/NewRegistration/" + fileName + ".jpg";
                response = new JavaScriptSerializer().Serialize(ResponseData);
            }

            return response;
        }

        [HttpGet]
        public ActionResult MemberApproval()
        {
            ViewBag.usertype = _LoggedInUser.UserType;
            return View();
        }

        [HttpPost]
        public string MemberListForApproval()
        {

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            DataTable dt = new DataTable();
            SocietyMemberRegistrationModel ObjSocietyMemberRegistrationModel = new SocietyMemberRegistrationModel(new DBLogger());
            dt = ObjSocietyMemberRegistrationModel.GetPLDBUserListForApproval(Convert.ToInt32(_LoggedInUser.UserId), _LoggedInUser.SSOID);

            foreach (var item in dt.AsEnumerable())
            {
                item.SetField("RequestIdEnc", COMMON_FUNCTION.Encrypt(Convert.ToString(item["RequestId"])));
            }

            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }


        [HttpPost]
        public string GetPLDBMembersByUserRights(string _ActionId = "0")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            DataTable dt = new DataTable();

            int ActionId = 0;
            int.TryParse(_ActionId, out ActionId);

            string _SSOId = _LoggedInUser.UserType.ToUpper() != "K" ? _LoggedInUser.SSOID : string.Empty; ;
            int _RoleId = _LoggedInUser.UserType.ToUpper() == "D" ? _LoggedInUser.RoleId : -1;
            string _UserType = _LoggedInUser.UserType;
            string _KioskId = _LoggedInUser.UserType.ToUpper() == "K" ? _LoggedInUser.KIOSKCODE : string.Empty;


            SocietyMemberRegistrationModel ObjSocietyMemberRegistrationModel = new SocietyMemberRegistrationModel(new DBLogger());
            dt = ObjSocietyMemberRegistrationModel.GetPLDBMembersByUserRights(_SSOId, _RoleId, _UserType, _KioskId, _LoggedInUser.UserRoleDeptMappingId, ActionId);

            foreach (var item in dt.AsEnumerable())
            {
                item.SetField("RequestIdEnc", COMMON_FUNCTION.Encrypt(Convert.ToString(item["RequestId"])));
                item.SetField("RequestId", -1);
            }

            return COMMON_FUNCTION.DataTableToJSONString(dt);
        }

        [HttpPost]
        public JsonResult GetPLDBMembersApprovalByUserRights(DataPagingModel Details, string _ActionId = "0")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            DataTable dt = new DataTable();

            int ActionId = 0;
            int.TryParse(_ActionId, out ActionId);

            string _SSOId = _LoggedInUser.UserType.ToUpper() != "K" ? _LoggedInUser.SSOID : string.Empty; ;
            int _RoleId = _LoggedInUser.UserType.ToUpper() == "D" ? _LoggedInUser.RoleId : -1;
            string _UserType = _LoggedInUser.UserType;
            string _KioskId = _LoggedInUser.UserType.ToUpper() == "K" ? _LoggedInUser.KIOSKCODE : string.Empty;


            SocietyMemberRegistrationModel ObjSocietyMemberRegistrationModel = new SocietyMemberRegistrationModel(new DBLogger());
            dt = ObjSocietyMemberRegistrationModel.GetPLDBMembersApprovalByUserRights(ref Details, _SSOId, _RoleId, _UserType, _KioskId, ActionId);

            foreach (var item in dt.AsEnumerable())
            {
                item.SetField("RequestIdEnc", COMMON_FUNCTION.Encrypt(Convert.ToString(item["RequestId"])));
                item.SetField("RequestId", -1);
            }

            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(dt), DataPaging = Details }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult GetPLDBMembersByUser(DataPagingModel Details,string _ActionId = "0")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            DataTable dt = new DataTable();

            int ActionId = 0;
            int.TryParse(_ActionId, out ActionId);

            string _SSOId = _LoggedInUser.UserType.ToUpper() != "K" ? _LoggedInUser.SSOID : string.Empty; ;
            int _RoleId = _LoggedInUser.UserType.ToUpper() == "D" ? _LoggedInUser.RoleId : -1;
            string _UserType = _LoggedInUser.UserType;
            string _KioskId = _LoggedInUser.UserType.ToUpper() == "K" ? _LoggedInUser.KIOSKCODE : string.Empty;


            SocietyMemberRegistrationModel ObjSocietyMemberRegistrationModel = new SocietyMemberRegistrationModel(new DBLogger());
            dt = ObjSocietyMemberRegistrationModel.GetPLDBMembersByUser(ref Details, _SSOId, _RoleId, _UserType, _KioskId, ActionId, _LoggedInUser.PLDBId, _LoggedInUser.PLDBBrancchId);

            foreach (var item in dt.AsEnumerable())
            {
                item.SetField("RequestIdEnc", COMMON_FUNCTION.Encrypt(Convert.ToString(item["RequestId"])));
                item.SetField("RequestId", -1);
            }           

            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(dt), DataPaging = Details }, JsonRequestBehavior.AllowGet);           
        }



        [HttpPost]
        public JsonResult ActionOnMemberApprovalRequest(string txn,string RequestId, int Status, string Remark)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            RequestId = RequestId.Replace(' ', '+');
            RequestId = RequestId.Replace(' ', '+');
            RequestId = RequestId.Replace(' ', '+');

            string id = COMMON_FUNCTION.Decrypt(RequestId);
            CitizenModel ObjCitizenModel = new CitizenModel(new DBLogger());
            StatusReport result = new StatusReport();
            result.Status = true;
            string MemberCertificate = "";
            if (Status == 1043)
            {
                result.Status = true;
                result = GeneratePLDBMembership(txn, RequestId);
                MemberCertificate = result.FileName;
            }
            int Result = 0;
            if (result.Status)
            {
                Result = ObjCitizenModel.SaveMemberRegistrationStatus(Convert.ToInt32(id), Status, Remark, _LoggedInUser.SSOID, _LoggedInUser.UserType, MemberCertificate);

                if (Convert.ToInt32(id) > 0)
                {
                    SocietyMemberRegistrationModel obj = new SocietyMemberRegistrationModel(new DBLogger());
                    DataTable RegisteredMemberData = new DataTable();
                    RegisteredMemberData = obj.GetRegisteredMemberDetailByRequestId(Convert.ToInt32(id));

                    if (RegisteredMemberData != null && RegisteredMemberData.Rows.Count > 0)
                    {
                        var P = RegisteredMemberData.Rows[0];
                        //string Message = "प्रिय सदस्य " + Convert.ToString(P["Name"]) + " आपकी सदस्यता पंजीकरण स्थिति इस प्रकार है| आपका सदस्यता क्रमांक: " + Convert.ToString(P["ApplicationNo"]) + " स्थिति: " + Convert.ToString(P["ApplicationStatus"]) + " टिप्पणी: " + Convert.ToString(P["ActionRemark"]);
                        string Message = "Dear " + Convert.ToString(P["Name"]) + " Please find registration status as:  Request No.: " + Convert.ToString(P["ApplicationNo"]) + " Status: " + Convert.ToString(P["ApplicationStatus"]) + " Remark: " + Convert.ToString(P["ActionRemark"]);
                        EsanchaarController EC = new EsanchaarController();
                        string ReturnVal = EC.SendSocietySMS(Message, Convert.ToString(P["MobileNo"]));



                        // Update Membership Certificate in DB
                    }
                }

            }
           
            return Json(Result > 0 ? 1 : Result);
        }

        [HttpPost]
        public string GetNextActionAsPerCurrentStatus(int StatusId)
        {
            SocietyMemberRegistrationModel ObjModel = new SocietyMemberRegistrationModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(ObjModel.GetNextActionListOnCurrentAction(StatusId));
        }

        [HttpPost]
        public string SaveCasePaymentDetails(OrganizationUserRegistrationEntity _Entity)
        {
            SocietyMemberRegistrationModel objHomeModel = new SocietyMemberRegistrationModel(new DBLogger());
            _Entity.KIOSKID = _LoggedInUser.KIOSKCODE;
            _Entity.SSOID = _LoggedInUser.SSOID;
            _Entity.CreatedBy = _LoggedInUser.UserId;
            _Entity.UserType = _LoggedInUser.UserType;
            int _RequestId = -1;
            //DataTable dt = objHomeModel.SavePLDBUserDetails(_Entity, ref _RequestId);
            string result = objHomeModel.UpdateMembershipApplicationPayment(_Entity, ref _RequestId);



            if (result != "-1")
            {
                string message = "Your membership application submitted successfully. Application No:-" + result;
                new EsanchaarController().SendSocietySMS(message, _Entity.MobileNo);
            }

            return result;
        }


        #region WFH Balwant
        [HttpPost]
        public string GetPLDBMembershipStatus(int StatusId)
        {           
            SocietyMemberRegistrationModel ObjModel = new SocietyMemberRegistrationModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONString(ObjModel.GetPLDBMembershipStatus(StatusId));
        }


        
        public StatusReport GeneratePLDBMembership(string txn, string _RequestIdEnc)
        {
            DataTable dt = null;
            StatusReport result = new StatusReport();
            result.Status = false;
            dt = new DataTable();
            dt.Columns.Add("AadharNo", typeof(string));
            dt.Columns.Add("txnid", typeof(string));
            dt.Columns.Add("PhotoPath", typeof(string));
            DataRow dtrow = dt.NewRow();
            dtrow["AadharNo"] = _LoggedInUser.AadharNo;
            dtrow["txnid"] = "";
            dt.Rows.Add(dtrow);
            //bool result = false;
            DataTable dtb = new DataTable();
            if (!string.IsNullOrWhiteSpace(txn)  && dt.Rows.Count > 0)
            {
                //_RequestIdEnc = _RequestIdEnc.Replace(' ', '+');
                //_RequestIdEnc = _RequestIdEnc.Replace(' ', '+');
                //_RequestIdEnc = _RequestIdEnc.Replace(' ', '+');

                string id = COMMON_FUNCTION.Decrypt(_RequestIdEnc);
                SocietyMemberRegistrationModel obj = new SocietyMemberRegistrationModel(new DBLogger());
                DataTable RegisteredMemberData = new DataTable();
                RegisteredMemberData = obj.GetRegisteredMemberDetailByRequestId(Convert.ToInt32(id));

                if (RegisteredMemberData != null && RegisteredMemberData.Rows.Count > 0)
                {
                    var firstRow = RegisteredMemberData.Rows[0];
                    result =  GenerateMembershipCertificate(RegisteredMemberData,txn);                    
                }                
            }
            else
            {
                //List<string[]> listx = new List<string[]>();
                //listx.Add(new string[] { "Failed", "Invalid OTP ", otp }); /*COMMON_FUNCTION.Encrypt(_AadharNo)*/
                //dtb = ConvertListToDataTable(listx);
            }
            return result;
        }



        #endregion
        #region Member Upload by Branch Secretary

        public ActionResult MemberUploadByBranchSecretary()
        {
            return View();
        }

        [HttpPost]
        [ActionName("InsertSocityByExcel")]
        public string InsertSocityByExcel(SocietyRegistrationEntity _Model)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            string fileName = string.Empty;
            _Model.ExcelUrl = "";
            if (Request.Files != null && Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];
                    fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    _Model.ExcelUrl = fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("~/Content/SocietyExcel"), fileName));

                }
                catch { }
            }
            _Model.CreatedBy = _Model.UpdatedBy = int.Parse(_LoggedInUser.UserId.ToString());
            _Model.CreatedIP = COMMON_FUNCTION.GetIpAddress();
            SocietyMemberRegistrationModel objLoan = new SocietyMemberRegistrationModel(new DBLogger());
            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            return objLoan.InsertMemberByExcel(_Model).ToString();
            //return CommonMessageFactory.Message(int.Parse(objSocietyModel.InsertSocityByExcel(_Model).ToString()), (Request.Cookies[SessionIdentity.Culture]).Value);
        }

        [HttpPost]
        [ActionName("GetMemberExcelDetails")]
        public string GetMemberExcelDetails()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            SocietyMemberRegistrationModel objModel = new SocietyMemberRegistrationModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objModel.GetMemberExcelDetails(_LoggedInUser.UserId);
            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }

        [HttpPost]
        [ActionName("GetExlMemberDetails")]
        public string GetExlMemberDetails(int ExlId, int StatusId)
        {
            SocietyMemberRegistrationModel objModel = new SocietyMemberRegistrationModel(new DBLogger());
            DataTable DT = new DataTable();
            DT = objModel.GetExlMemberDetails(ExlId, StatusId);
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
        }

        [HttpPost]
        [ActionName("UpdateExcelMemberDetailsByExcelUpload")]
        public string UpdateExcelMemberDetailsByExcelUpload(string RequestId, string MemberName, string FatherHusbandName, string MobileNo, string AADHARNo, string MembershipNo, string MembershipDate)
        {
            SocietyMemberRegistrationModel objModel = new SocietyMemberRegistrationModel(new DBLogger());
            return objModel.UpdateExcelMemberDetailsByExcelUpload(RequestId, MemberName, FatherHusbandName, MobileNo, AADHARNo, MembershipNo, MembershipDate);
        }

        [HttpPost]
        [ActionName("DeleteExlMemberDetails")]
        public string DeleteExlMemberDetails(int ExlId, int StatusId, int RequestId)
        {
            SocietyMemberRegistrationModel objModel = new SocietyMemberRegistrationModel(new DBLogger());
            int result = objModel.DeleteExlMemberDetails(ExlId, StatusId, RequestId);
            return result.ToString();
        }


        #endregion





    }
}

