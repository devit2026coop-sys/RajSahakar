using com.sun.security.ntlm;
using CoopDB;
using Cooperative.Filters;
using Cooperative.Models;
using javax.xml.stream;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using sun.swing;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using static Cooperative.Models.CoopModel;
using static Cooperative.Models.GCCServiceModel;
using static Cooperative.Models.GopalCreditCardModel;


namespace Cooperative.Controllers
{
    [LogActionFilter]
    public class GopalCreditCardController : BaseController
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];


        [CustomAuthentication]
        public ActionResult GopalApplicationForm()
        {
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.GopalCreditCardRegistration, (Request.Cookies[SessionIdentity.Culture]).Value);
            }

            return View();
        }
        public ActionResult EsignTest() //12-02-2026 #Pradeep
        {
            return View();
        }
        public ActionResult GopalApplicationFormPart2() //17-07-2024 #Pradeep
        {
            return View();
        }

        public ActionResult ApplicationFlowGCC()
        {
            return View();
        }
        public ActionResult Moka_Report() //27-Aug-2024 #madhuri
        {
            return View();
        }
        public ActionResult BranchLevel_Report() //6-Sep-2024 #madhuri
        {
            return View();
        }

        public ActionResult MapGcc() //6-Sep-2024 #madhuri
        {
            return View();
        }

        public ActionResult GccApplicationTransfer() //6-Sep-2024 #madhuri
        {
            return View();
        }

        public ActionResult GccRecoveryApplicationList() //26-Aug-2025 #madhuri
        {
            return View();
        }

        [CustomAuthentication]
        public ActionResult GopalApplicationFormActionView(string eId, string eRno)

        {

            if (!String.IsNullOrEmpty(eId))
            {
                eId = eId.Replace(' ', '+');
            }
            if (!String.IsNullOrEmpty(eRno))
            {
                eRno = eRno.Replace(' ', '+');
            }
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                if (!String.IsNullOrEmpty(eId) && !String.IsNullOrEmpty(eRno))
                {
                    System.Web.HttpContext.Current.Session[SessionIdentity.GCCEId] = eId;
                    System.Web.HttpContext.Current.Session[SessionIdentity.GCCERNO] = eRno;
                }
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.GopalCreditCardRegistration, (Request.Cookies[SessionIdentity.Culture]).Value);
            }

            return View();
        }

        [HttpPost]
        public int UpdateAadharData(string aadharNo, int GCCEId)
        {
            try
            {

                GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());
                int result = objGopalCreditCardDBModel.UpdateGCCApplicantAadhar(GCCEId, aadharNo);
                return result;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = " UpdateAadharData(string aadharNo, int GCCId)" }, ex);
                return 0;
            }

        }

        [HttpPost]
        public string GetDashBoardData(GopalCreditCardModel.DashBoardDataModel model)
        {
            model.SSOID = _LoggedInUser.SSOID;
            model.UserType = _LoggedInUser.UserType;
            model.KIOSKCODE = _LoggedInUser.KIOSKCODE;

            DataTable DT = null;
            GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());
            DT = objGopalCreditCardDBModel.Get_DashBoardData(model);

            //return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);

            if (DT != null)
            {
                if (DT != null && DT.Rows.Count > 0)
                {
                    DT.Columns.Add("Encrypted_RegistrationNo");
                    DT.Columns.Add("Encrypted_GopalCreditCardId");
                }
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    DT.Rows[i]["Encrypted_RegistrationNo"] = !String.IsNullOrEmpty(DT.Rows[i]["RegistrationNo"].ToString()) == true ? COMMON_FUNCTION.Encrypt(DT.Rows[i]["RegistrationNo"].ToString()) : "";
                    DT.Rows[i]["Encrypted_GopalCreditCardId"] = !String.IsNullOrEmpty(DT.Rows[i]["GopalCreditCardId"].ToString()) == true ? COMMON_FUNCTION.Encrypt(DT.Rows[i]["GopalCreditCardId"].ToString()) : "";

                }

                var jsonData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
                var resultDataModel = JsonConvert.DeserializeObject<List<GopalCreditCardModel.DashBoardDataModel>>(jsonData);
                List<GopalCreditCardModel.DashBoardDataModel> listData = new List<DashBoardDataModel>();
                listData = resultDataModel;
                var json = new JavaScriptSerializer().Serialize(listData);
                return json; ;
            }
            else
            {
                return null;
            }


        }

        [HttpPost]
        public string GetJanAadhaarMembersList(string JanAadharNo)
        {
            JanAadharNo = csCommon.Decrypt_SA(Uri.UnescapeDataString(JanAadharNo));
            try
            {
                DataTable DT = null;
                List<JanAadharModel.JanAaadharFamilyMemberModel> memberLists = COMMON_FUNCTION.GetJanAadhaarMembersList(JanAadharNo);
                List<JanAadharModel.JanAaadharFamilyMemberModel> familyDataModel = new List<JanAadharModel.JanAaadharFamilyMemberModel>();

                GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());


                if (memberLists != null)
                {
                    var JanMemberIds = "";
                    foreach (var item in memberLists)
                    {
                        JanMemberIds += "," + item.JAN_MEMBER_ID;
                    }
                    //  DT = objGopalCreditCardDBModel.CheckGCCRegistrationStatus(JanMemberIds);
                    DT = objGopalCreditCardDBModel.CheckGCCRegistrationStatus(JanMemberIds, COMMON_FUNCTION.Encrypt(JanAadharNo));


                    memberLists.ForEach(x =>
                    {
                        JanAadharModel.JanAaadharFamilyMemberModel model = new JanAadharModel.JanAaadharFamilyMemberModel();

                        model.NAME = x.NAME;
                        model.NAME_HINDI = x.NAME_HINDI;
                        model.FATHER_NAME_ENG = x.FATHER_NAME_ENG;
                        model.FATHER_NAME_HND = x.FATHER_NAME_HND;
                        model.GENDER = x.GENDER;
                        model.MEMBER_TYPE = x.MEMBER_TYPE;

                        model.Masked_MOBILE_NO = x.Masked_MOBILE_NO;
                        model.Encrypted_MOBILE_NO = x.Encrypted_MOBILE_NO;
                        model.JAN_AADHAR = x.JAN_AADHAR;
                        model.Encrypted_JAN_AADHAR = x.Encrypted_JAN_AADHAR;
                        model.Masked_JAN_MEMBER_ID = x.Masked_JAN_MEMBER_ID;
                        model.Encrypted_ENR_ID = x.Encrypted_ENR_ID;
                        model.Encrypted_JAN_MEMBER_ID = x.Encrypted_JAN_MEMBER_ID;
                        model.Encrypted_AADHAR_REF_NO = x.Encrypted_AADHAR_REF_NO;
                        model.Age = x.Age;

                        //if (DT.Rows.Count == 0 || (DT.Rows.Count > 0 && DT.Columns.Contains("JanAadharMIDList") && string.IsNullOrEmpty(DT.Rows[0]["JanAadharMIDList"]?.ToString()) || !DT.Rows[0]["JanAadharMIDList"].ToString().Contains(x.JAN_MEMBER_ID)))
                        //{
                        //    familyDataModel.Add(model);
                        //}

                        if (DT.Rows.Count == 0 || (DT.Rows.Count > 0 && DT.Columns.Contains("JanAadharMIDList") || (string.IsNullOrWhiteSpace(DT.Rows[0]["JanAadharMIDList"]?.ToString()) || !("," + DT.Rows[0]["JanAadharMIDList"].ToString().Trim() + ",").Contains("," + x.JAN_MEMBER_ID + ","))))
                        {
                            familyDataModel.Add(model);
                        }

                        //if (DT.Rows.Count == 0 || (DT.Rows.Count > 0 && DT.Columns.Contains("JanAadharMIDList") && (string.IsNullOrEmpty(DT.Rows[0]["JanAadharMIDList"]?.ToString()) || !DT.Rows[0]["JanAadharMIDList"].ToString().Contains(x.JAN_MEMBER_ID))))
                        //{
                        //    familyDataModel.Add(model);
                        //}

                        //if (DT.Rows.Count == 0)
                        //{
                        //    familyDataModel.Add(model);
                        //}                        

                    });
                    Session["JanAadharFamilyDataModel"] = familyDataModel;

                    var json = new JavaScriptSerializer().Serialize(familyDataModel);
                    //Session["MemberDataTable"] = "";

                    //if (DT.Rows.Count != 0 && memberLists != null)
                    //    return "-1";

                    return json;
                }
                else
                {
                    //Session["MemberDataTable"] = "";
                    return "";
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = " GetJanAadhaarMembersList(string JanAadharNo)" + JanAadharNo }, ex);
                return null;
            }
        }

        [HttpPost]
        public string GenrateJanAadharOTP(JanAadharModel.JanAaadharFamilyMemberModel _Model)
        {
            try
            {
                DataSet data = new DataSet();
                string eJanId = "";
                string eJanMID = "";
                //var data = "";
                if (Request.Url.Host.Contains("localhost") || Request.Url.Host.Contains("devitsandbox"))
                {
                    var responseString = "<root><status>true</status><tid>168232615</tid></root>";
                    data = COMMON_FUNCTION.ConvertXMLResponceToDataSet(responseString);
                }
                else
                {
                    data = COMMON_FUNCTION.SendJanAadharOTP(_Model.Encrypted_JAN_AADHAR, _Model.Encrypted_JAN_MEMBER_ID);
                }
                //if (data.Tables[0].Rows[0]["status"].ToString() != null)
                if (data != null && data.Tables.Count != 0 && data.Tables[0].Rows.Count != 0)
                {
                    if (data.Tables[0].Rows[0]["status"].ToString() != "true")
                    {
                        if (data.Tables[0].Rows[0]["status"].ToString() == "false")
                        {
                            //AlertMessage("Selected Janaadhaar member details not updated with Janaadhaar!");
                            return "";
                        }
                        else
                        {
                            //AlertMessage(data.Tables[0].Rows[0]["status"].ToString());
                            return "";
                        }

                        //return Session["txn"].ToString();
                    }
                    else
                    {

                        string tid = data.Tables[0].Rows[0]["tid"].ToString();
                        //var jason=COMMON_FUNCTION.DataTableToJSONString(data.Tables[0]);                  
                        //AlertMessage("OTP send to your mobile no.");
                        return tid;
                    }
                }
                else
                {
                    //AlertMessage("Fail");
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = " GenrateJanAadharOTP(JanAadharModel.JanAaadharFamilyMemberModel _Model)" }, ex);
                return null;
            }
            return "";
        }
        [HttpPost]
        public string ValidateJanAadharOTP(JanAadharModel.JanAaadharFamilyMemberModel model)
        {
            try
            {
                OperationResult operationResult = new OperationResult();
                DataSet data = new DataSet();
                if (Request.Url.Host.Contains("localhost") || Request.Url.Host.Contains("devitsandbox"))
                {
                    var responseString = "<root><tid>169621548</tid><isvalidate>true</isvalidate></root>";
                    data = COMMON_FUNCTION.ConvertXMLResponceToDataSet(responseString);
                }
                else
                {
                    data = COMMON_FUNCTION.ValidateJanAadharOTP(model.JanAadharOTP, model.tid);
                }
                if (data != null)
                {
                    if (data.Tables[0].Rows[0]["isvalidate"].ToString() == "false")
                    {
                        //AlertMessage("Selected Janaadhaar member details not updated with Janaadhaar!");
                        operationResult.Status = false;
                        var json = new JavaScriptSerializer().Serialize(operationResult);
                        return "";
                    }
                    else
                    {
                        var dataList = COMMON_FUNCTION.GetJanAadharMembersDetail(COMMON_FUNCTION.Decrypt(model.Encrypted_JAN_AADHAR), model.ENR_ID, COMMON_FUNCTION.Decrypt(model.Encrypted_JAN_AADHAR), COMMON_FUNCTION.Decrypt(model.Encrypted_JAN_MEMBER_ID));

                        GopalCreditCardModel.GopalCreditCardFormModel gopalCreditCardFormModel = new GopalCreditCardModel.GopalCreditCardFormModel();

                        if (dataList.Status.ToUpper() == "OK")
                        {
                            gopalCreditCardFormModel.Status = true;
                            gopalCreditCardFormModel.JanAadharNo = dataList.UserDetails.JanAadharNo;
                            gopalCreditCardFormModel.Encrypted_JanAadharNo = dataList.UserDetails.Encrypted_JanAadharNo;
                            //gopalCreditCardFormModel.JanAadharMID = dataList.UserDetails.JanAadharMID;
                            gopalCreditCardFormModel.Encrypted_JanAadharMID = dataList.UserDetails.Encrypted_JanAadharMID;
                            gopalCreditCardFormModel.JanAadharEnrollmentId = dataList.UserDetails.JanAadharEnrollmentId;
                            gopalCreditCardFormModel.AadharNo = dataList.UserDetails.AadharNo;
                            gopalCreditCardFormModel.Name_ENG = dataList.UserDetails.Name_ENG;
                            gopalCreditCardFormModel.Name_HI = dataList.UserDetails.Name_HI;
                            gopalCreditCardFormModel.FatherName_ENG = dataList.UserDetails.FatherName_ENG;
                            gopalCreditCardFormModel.FatherName_HI = dataList.UserDetails.FatherName_HI;
                            gopalCreditCardFormModel.MotherName_ENG = dataList.UserDetails.MotherName_ENG;
                            gopalCreditCardFormModel.MotherName_HI = dataList.UserDetails.MotherName_HI;
                            gopalCreditCardFormModel.SpouseName_ENG = dataList.UserDetails.SpouseName_ENG;
                            gopalCreditCardFormModel.SpouseName_HI = dataList.UserDetails.SpouseName_HI;
                            gopalCreditCardFormModel.MaritalStatus = dataList.UserDetails.MaritalStatus;
                            gopalCreditCardFormModel.RelationType = dataList.UserDetails.RelationType;
                            gopalCreditCardFormModel.DOB = dataList.UserDetails.DOB;
                            gopalCreditCardFormModel.Age = dataList.UserDetails.Age;
                            gopalCreditCardFormModel.Gender = dataList.UserDetails.Gender;
                            gopalCreditCardFormModel.MobileNo = dataList.UserDetails.MobileNo;
                            gopalCreditCardFormModel.BankAccountNo = ""; //dataList.UserDetails.BankAccountNo;
                            gopalCreditCardFormModel.BankName = ""; //dataList.UserDetails.BankName;
                            gopalCreditCardFormModel.BankIFSCCode = ""; //dataList.UserDetails.BankIFSCCode;
                            gopalCreditCardFormModel.BankBranchName = ""; //dataList.UserDetails.BankBranchName;
                            gopalCreditCardFormModel.Income = dataList.UserDetails.Income;
                            gopalCreditCardFormModel.Occupation = dataList.UserDetails.Occupation;
                            gopalCreditCardFormModel.PANCardNo = dataList.UserDetails.PANCardNo;
                            gopalCreditCardFormModel.Caste = dataList.UserDetails.Caste;
                            gopalCreditCardFormModel.Category = dataList.UserDetails.Category;
                            gopalCreditCardFormModel.Address_ENG = dataList.UserDetails.Address.Address_ENG;
                            gopalCreditCardFormModel.Original_Address_ENG = dataList.UserDetails.Address.Address_ENG;
                            gopalCreditCardFormModel.Address_HI = dataList.UserDetails.Address.Address_HI;
                            gopalCreditCardFormModel.Original_Address_HI = dataList.UserDetails.Address.Address_HI;
                            gopalCreditCardFormModel.PinCode = dataList.UserDetails.Address.PinCode;
                            gopalCreditCardFormModel.Original_PinCode = dataList.UserDetails.Address.PinCode;
                            gopalCreditCardFormModel.DistrictName_ENG = dataList.UserDetails.Address.DistrictName_ENG;
                            gopalCreditCardFormModel.DistrictName_HI = dataList.UserDetails.Address.DistrictName_HI;
                            gopalCreditCardFormModel.District_Code = dataList.UserDetails.Address.District_Code;
                            gopalCreditCardFormModel.Original_District_Code = dataList.UserDetails.Address.District_Code;

                            gopalCreditCardFormModel.IsRural = dataList.UserDetails.Address.IsRural;
                            gopalCreditCardFormModel.Original_IsRural = dataList.UserDetails.Address.IsRural;
                            if (dataList.UserDetails.Address.IsRural == true)
                            {
                                gopalCreditCardFormModel.Block_ENG = dataList.UserDetails.Address.Block_ENG;
                                gopalCreditCardFormModel.Block_HI = dataList.UserDetails.Address.Block_HI;
                                gopalCreditCardFormModel.Block_Code = dataList.UserDetails.Address.Block_Code;
                                gopalCreditCardFormModel.Original_Block_Code = dataList.UserDetails.Address.Block_Code;
                                gopalCreditCardFormModel.GP_ENG = dataList.UserDetails.Address.GP_ENG;
                                gopalCreditCardFormModel.GP_HI = dataList.UserDetails.Address.GP_HI;
                                gopalCreditCardFormModel.GP_Code = dataList.UserDetails.Address.GP_Code;
                                gopalCreditCardFormModel.Original_GP_Code = dataList.UserDetails.Address.GP_Code;
                                gopalCreditCardFormModel.Village_ENG = dataList.UserDetails.Address.Village_ENG;
                                gopalCreditCardFormModel.Village_HI = dataList.UserDetails.Address.Village_HI;
                                gopalCreditCardFormModel.Village_Code = dataList.UserDetails.Address.Village_Code;
                                gopalCreditCardFormModel.Original_Village_Code = dataList.UserDetails.Address.Village_Code;
                            }
                            else
                            {
                                gopalCreditCardFormModel.City_ENG = dataList.UserDetails.Address.City_ENG;
                                gopalCreditCardFormModel.City_HI = dataList.UserDetails.Address.City_HI;
                                gopalCreditCardFormModel.City_Code = dataList.UserDetails.Address.City_Code;
                                gopalCreditCardFormModel.Original_City_Code = dataList.UserDetails.Address.City_Code;
                            }

                            gopalCreditCardFormModel.MemberPhoto_Base64 = dataList.UserDetails.MemberPhoto_Base64;

                            gopalCreditCardFormModel.LoanDistrictId = "0";
                            gopalCreditCardFormModel.LoanBankID = "0";
                            gopalCreditCardFormModel.LoanBranchId = "0";
                            gopalCreditCardFormModel.PACS_ID = "0";


                            /*MahilaNidhi*/
                            //gopalCreditCardFormModel.Mn_DistrictId = 0;
                            //gopalCreditCardFormModel.Mn_BlockId =0;
                            //gopalCreditCardFormModel.Mn_ClusterId = "0";
                            //gopalCreditCardFormModel.Mn_VoId = "0";
                            //gopalCreditCardFormModel.Mn_ShgId = "0";
                            //gopalCreditCardFormModel.Mn_DistrictName = "";
                            //gopalCreditCardFormModel.Mn_BlockName = "";                          
                            //gopalCreditCardFormModel.Mn_ClusterName = "";
                            //gopalCreditCardFormModel.Mn_VoName = "";
                            //gopalCreditCardFormModel.Mn_ShgName = "";


                            //List<JanAadharModel.JanAaadharFamilyMemberModel> familyDataModel = new List<JanAadharModel.JanAaadharFamilyMemberModel>();
                            var familyDataModel = (List<JanAadharModel.JanAaadharFamilyMemberModel>)Session["JanAadharFamilyDataModel"];
                            var familyDataModelNew = familyDataModel.Where(x => x.Encrypted_JAN_MEMBER_ID != gopalCreditCardFormModel.Encrypted_JanAadharMID).ToList();


                            List<GopalCreditCardModel.JanAaadharFamilyMemberModel> gopalFamilyDataModel = new List<GopalCreditCardModel.JanAaadharFamilyMemberModel>();
                            //for (int i = 0; i < familyDataModelNew.Count; i++)
                            //{
                            //    GopalCreditCardModel.JanAaadharFamilyMemberModel janModel = new GopalCreditCardModel.JanAaadharFamilyMemberModel();

                            //    janModel.NAME = familyDataModelNew[i].NAME;
                            //    janModel.NAME_HINDI = familyDataModelNew[i].NAME_HINDI;

                            //    gopalFamilyDataModel.Add(janModel);
                            //}
                            //gopalCreditCardFormModel.janAaadharFamilyMemberModels = gopalFamilyDataModel;


                            familyDataModelNew.ForEach(x =>
                            {
                                GopalCreditCardModel.JanAaadharFamilyMemberModel janModel = new GopalCreditCardModel.JanAaadharFamilyMemberModel();

                                janModel.NAME = x.NAME;
                                janModel.NAME_HI = x.NAME_HINDI;
                                janModel.FATHER_NAME_ENG = x.FATHER_NAME_ENG;
                                janModel.FATHER_NAME_HND = x.FATHER_NAME_HND;
                                janModel.GENDER = x.GENDER;
                                janModel.MEMBER_TYPE = x.MEMBER_TYPE;
                                janModel.Masked_MOBILE_NO = x.Masked_MOBILE_NO;
                                janModel.Encrypted_MOBILE_NO = x.Encrypted_MOBILE_NO;
                                janModel.JAN_AADHAR = x.JAN_AADHAR;
                                janModel.Encrypted_JAN_AADHAR = x.Encrypted_JAN_AADHAR;
                                janModel.Masked_JAN_MEMBER_ID = x.Masked_JAN_MEMBER_ID;
                                janModel.Encrypted_JAN_MEMBER_ID = x.Encrypted_JAN_MEMBER_ID;
                                janModel.Encrypted_AADHAR_REF_NO = x.Encrypted_AADHAR_REF_NO;
                                janModel.Age = x.Age ?? 0;

                                gopalFamilyDataModel.Add(janModel);
                            });
                            gopalCreditCardFormModel.janAaadharFamilyMemberList = gopalFamilyDataModel;
                        }


                        var json = new JavaScriptSerializer().Serialize(gopalCreditCardFormModel);
                        return json;
                    }
                }
                else
                {
                    //AlertMessage("Fail");
                }
                //var json = new JavaScriptSerializer().Serialize(operationResult);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = " ValidateJanAadharOTP(JanAadharModel.JanAaadharFamilyMemberModel model)" }, ex);
                return null;
            }
            return "";
        }

        [HttpPost]
        public string ValidateJanAadharOTP_V1(JanAadharModel.JanAaadharFamilyMemberModel model)
        {
            try
            {
                OperationResult operationResult = new OperationResult();
                DataSet data = new DataSet();
                if (Request.Url.Host.Contains("localhost") || Request.Url.Host.Contains("devitsandbox"))
                {
                    var responseString = "<root><tid>169621548</tid><isvalidate>true</isvalidate></root>";
                    data = COMMON_FUNCTION.ConvertXMLResponceToDataSet(responseString);
                }
                else
                {
                    data = COMMON_FUNCTION.ValidateJanAadharOTP(model.JanAadharOTP, model.tid);
                }
                if (data != null)
                {
                    if (data.Tables[0].Rows[0]["isvalidate"].ToString() == "false")
                    {
                        //AlertMessage("Selected Janaadhaar member details not updated with Janaadhaar!");
                        operationResult.Status = false;
                        var json = new JavaScriptSerializer().Serialize(operationResult);
                        return "";
                    }
                    else
                    {




                        var dataList = COMMON_FUNCTION.GetJanAadharMembersDetail(COMMON_FUNCTION.Decrypt(model.Encrypted_JAN_AADHAR), model.ENR_ID, COMMON_FUNCTION.Decrypt(model.Encrypted_JAN_AADHAR), COMMON_FUNCTION.Decrypt(model.Encrypted_JAN_MEMBER_ID));
                        GopalCreditCardModel.GopalCreditCardFormModel gopalCreditCardFormModel = new GopalCreditCardModel.GopalCreditCardFormModel();

                        if (dataList.Status.ToUpper() == "OK")
                        {
                            gopalCreditCardFormModel.Status = true;
                            gopalCreditCardFormModel.JanAadharNo = dataList.UserDetails.JanAadharNo;
                            gopalCreditCardFormModel.Encrypted_JanAadharNo = dataList.UserDetails.Encrypted_JanAadharNo;
                            //gopalCreditCardFormModel.JanAadharMID = dataList.UserDetails.JanAadharMID;
                            gopalCreditCardFormModel.Encrypted_JanAadharMID = dataList.UserDetails.Encrypted_JanAadharMID;
                            gopalCreditCardFormModel.JanAadharEnrollmentId = dataList.UserDetails.JanAadharEnrollmentId;
                            gopalCreditCardFormModel.AadharNo = dataList.UserDetails.AadharNo;
                            gopalCreditCardFormModel.Name_ENG = dataList.UserDetails.Name_ENG;
                            gopalCreditCardFormModel.Name_HI = dataList.UserDetails.Name_HI;
                            gopalCreditCardFormModel.FatherName_ENG = dataList.UserDetails.FatherName_ENG;
                            gopalCreditCardFormModel.FatherName_HI = dataList.UserDetails.FatherName_HI;
                            gopalCreditCardFormModel.MotherName_ENG = dataList.UserDetails.MotherName_ENG;
                            gopalCreditCardFormModel.MotherName_HI = dataList.UserDetails.MotherName_HI;
                            gopalCreditCardFormModel.SpouseName_ENG = dataList.UserDetails.SpouseName_ENG;
                            gopalCreditCardFormModel.SpouseName_HI = dataList.UserDetails.SpouseName_HI;
                            gopalCreditCardFormModel.MaritalStatus = dataList.UserDetails.MaritalStatus;
                            gopalCreditCardFormModel.RelationType = dataList.UserDetails.RelationType;
                            gopalCreditCardFormModel.DOB = dataList.UserDetails.DOB;
                            gopalCreditCardFormModel.Age = dataList.UserDetails.Age;
                            gopalCreditCardFormModel.Gender = dataList.UserDetails.Gender;
                            gopalCreditCardFormModel.MobileNo = dataList.UserDetails.MobileNo;
                            gopalCreditCardFormModel.BankAccountNo = ""; //dataList.UserDetails.BankAccountNo;
                            gopalCreditCardFormModel.BankName = ""; //dataList.UserDetails.BankName;
                            gopalCreditCardFormModel.BankIFSCCode = ""; //dataList.UserDetails.BankIFSCCode;
                            gopalCreditCardFormModel.BankBranchName = ""; //dataList.UserDetails.BankBranchName;
                            gopalCreditCardFormModel.Income = dataList.UserDetails.Income;
                            gopalCreditCardFormModel.Occupation = dataList.UserDetails.Occupation;
                            gopalCreditCardFormModel.PANCardNo = dataList.UserDetails.PANCardNo;
                            gopalCreditCardFormModel.Caste = dataList.UserDetails.Caste;
                            gopalCreditCardFormModel.Category = dataList.UserDetails.Category;
                            gopalCreditCardFormModel.Address_ENG = dataList.UserDetails.Address.Address_ENG;
                            gopalCreditCardFormModel.Original_Address_ENG = dataList.UserDetails.Address.Address_ENG;
                            gopalCreditCardFormModel.Address_HI = dataList.UserDetails.Address.Address_HI;
                            gopalCreditCardFormModel.Original_Address_HI = dataList.UserDetails.Address.Address_HI;
                            gopalCreditCardFormModel.PinCode = dataList.UserDetails.Address.PinCode;
                            gopalCreditCardFormModel.Original_PinCode = dataList.UserDetails.Address.PinCode;
                            gopalCreditCardFormModel.DistrictName_ENG = dataList.UserDetails.Address.DistrictName_ENG;
                            gopalCreditCardFormModel.DistrictName_HI = dataList.UserDetails.Address.DistrictName_HI;
                            gopalCreditCardFormModel.District_Code = dataList.UserDetails.Address.District_Code;
                            gopalCreditCardFormModel.Original_District_Code = dataList.UserDetails.Address.District_Code;

                            gopalCreditCardFormModel.IsRural = dataList.UserDetails.Address.IsRural;
                            gopalCreditCardFormModel.Original_IsRural = dataList.UserDetails.Address.IsRural;
                            if (dataList.UserDetails.Address.IsRural == true)
                            {
                                gopalCreditCardFormModel.Block_ENG = dataList.UserDetails.Address.Block_ENG;
                                gopalCreditCardFormModel.Block_HI = dataList.UserDetails.Address.Block_HI;
                                gopalCreditCardFormModel.Block_Code = dataList.UserDetails.Address.Block_Code;
                                gopalCreditCardFormModel.Original_Block_Code = dataList.UserDetails.Address.Block_Code;
                                gopalCreditCardFormModel.GP_ENG = dataList.UserDetails.Address.GP_ENG;
                                gopalCreditCardFormModel.GP_HI = dataList.UserDetails.Address.GP_HI;
                                gopalCreditCardFormModel.GP_Code = dataList.UserDetails.Address.GP_Code;
                                gopalCreditCardFormModel.Original_GP_Code = dataList.UserDetails.Address.GP_Code;
                                gopalCreditCardFormModel.Village_ENG = dataList.UserDetails.Address.Village_ENG;
                                gopalCreditCardFormModel.Village_HI = dataList.UserDetails.Address.Village_HI;
                                gopalCreditCardFormModel.Village_Code = dataList.UserDetails.Address.Village_Code;
                                gopalCreditCardFormModel.Original_Village_Code = dataList.UserDetails.Address.Village_Code;
                            }
                            else
                            {
                                gopalCreditCardFormModel.City_ENG = dataList.UserDetails.Address.City_ENG;
                                gopalCreditCardFormModel.City_HI = dataList.UserDetails.Address.City_HI;
                                gopalCreditCardFormModel.City_Code = dataList.UserDetails.Address.City_Code;
                                gopalCreditCardFormModel.Original_City_Code = dataList.UserDetails.Address.City_Code;
                            }

                            gopalCreditCardFormModel.MemberPhoto_Base64 = dataList.UserDetails.MemberPhoto_Base64;

                            gopalCreditCardFormModel.LoanDistrictId = "0";
                            gopalCreditCardFormModel.LoanBankID = "0";
                            gopalCreditCardFormModel.LoanBranchId = "0";
                            gopalCreditCardFormModel.PACS_ID = "0";

                            /*MahilaNidhi*/
                            //gopalCreditCardFormModel.Mn_DistrictId = 0;
                            //gopalCreditCardFormModel.Mn_BlockId =0;
                            //gopalCreditCardFormModel.Mn_ClusterId = "0";
                            //gopalCreditCardFormModel.Mn_VoId = "0";
                            //gopalCreditCardFormModel.Mn_ShgId = "0";
                            //gopalCreditCardFormModel.Mn_DistrictName = "";
                            //gopalCreditCardFormModel.Mn_BlockName = "";                          
                            //gopalCreditCardFormModel.Mn_ClusterName = "";
                            //gopalCreditCardFormModel.Mn_VoName = "";
                            //gopalCreditCardFormModel.Mn_ShgName = "";

                            //List<JanAadharModel.JanAaadharFamilyMemberModel> familyDataModel = new List<JanAadharModel.JanAaadharFamilyMemberModel>();
                            var familyDataModel = (List<JanAadharModel.JanAaadharFamilyMemberModel>)Session["JanAadharFamilyDataModel"];
                            var familyDataModelNew = familyDataModel.Where(x => x.Encrypted_JAN_MEMBER_ID != gopalCreditCardFormModel.Encrypted_JanAadharMID).ToList();


                            List<GopalCreditCardModel.JanAaadharFamilyMemberModel> gopalFamilyDataModel = new List<GopalCreditCardModel.JanAaadharFamilyMemberModel>();
                            //for (int i = 0; i < familyDataModelNew.Count; i++)
                            //{
                            //    GopalCreditCardModel.JanAaadharFamilyMemberModel janModel = new GopalCreditCardModel.JanAaadharFamilyMemberModel();

                            //    janModel.NAME = familyDataModelNew[i].NAME;
                            //    janModel.NAME_HINDI = familyDataModelNew[i].NAME_HINDI;

                            //    gopalFamilyDataModel.Add(janModel);
                            //}
                            //gopalCreditCardFormModel.janAaadharFamilyMemberModels = gopalFamilyDataModel;


                            familyDataModelNew.ForEach(x =>
                            {
                                GopalCreditCardModel.JanAaadharFamilyMemberModel janModel = new GopalCreditCardModel.JanAaadharFamilyMemberModel();

                                janModel.NAME = x.NAME;
                                janModel.NAME_HI = x.NAME_HINDI;
                                janModel.FATHER_NAME_ENG = x.FATHER_NAME_ENG;
                                janModel.FATHER_NAME_HND = x.FATHER_NAME_HND;
                                janModel.GENDER = x.GENDER;
                                janModel.MEMBER_TYPE = x.MEMBER_TYPE;
                                janModel.Masked_MOBILE_NO = x.Masked_MOBILE_NO;
                                janModel.Encrypted_MOBILE_NO = x.Encrypted_MOBILE_NO;
                                janModel.JAN_AADHAR = x.JAN_AADHAR;
                                janModel.Encrypted_JAN_AADHAR = x.Encrypted_JAN_AADHAR;
                                janModel.Masked_JAN_MEMBER_ID = x.Masked_JAN_MEMBER_ID;
                                janModel.Encrypted_JAN_MEMBER_ID = x.Encrypted_JAN_MEMBER_ID;
                                janModel.Encrypted_AADHAR_REF_NO = x.Encrypted_AADHAR_REF_NO;
                                janModel.Age = x.Age ?? 0;

                                gopalFamilyDataModel.Add(janModel);
                            });
                            gopalCreditCardFormModel.janAaadharFamilyMemberList = gopalFamilyDataModel;
                        }
                        var json = new JavaScriptSerializer().Serialize(gopalCreditCardFormModel);
                        return json;
                    }
                }
                else
                {
                    //AlertMessage("Fail");
                }
                //var json = new JavaScriptSerializer().Serialize(operationResult);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = " ValidateJanAadharOTP(JanAadharModel.JanAaadharFamilyMemberModel model)" }, ex);
                return null;
            }
            return "";
        }



        [HttpPost]
        public string PassValidateJanAadharOTP(JanAadharModel.JanAaadharFamilyMemberModel model)
        {
            try
            {
                //OperationResult operationResult = new OperationResult();
                DataSet data = new DataSet();
                //if (Request.Url.Host.Contains("localhost"))
                //{
                //    var responseString = "<root><tid>169621548</tid><isvalidate>true</isvalidate></root>";
                //    data = COMMON_FUNCTION.ConvertXMLResponceToDataSet(responseString);
                //}
                //else
                //{
                //    data = COMMON_FUNCTION.ValidateJanAadharOTP(model.JanAadharOTP, model.tid);
                //}
                //if (data != null)
                //{
                //if (data.Tables[0].Rows[0]["isvalidate"].ToString() == "false")
                //{
                //    //AlertMessage("Selected Janaadhaar member details not updated with Janaadhaar!");
                //    operationResult.Status = false;
                //    var json = new JavaScriptSerializer().Serialize(operationResult);
                //    return "";
                //}
                //else
                {
                    var dataList = COMMON_FUNCTION.GetJanAadharMembersDetail(COMMON_FUNCTION.Decrypt(model.Encrypted_JAN_AADHAR), model.ENR_ID, COMMON_FUNCTION.Decrypt(model.Encrypted_JAN_AADHAR), COMMON_FUNCTION.Decrypt(model.Encrypted_JAN_MEMBER_ID));

                    GopalCreditCardModel.GopalCreditCardFormModel gopalCreditCardFormModel = new GopalCreditCardModel.GopalCreditCardFormModel();

                    if (dataList.Status.ToUpper() == "OK")
                    {
                        gopalCreditCardFormModel.Status = true;
                        gopalCreditCardFormModel.JanAadharNo = dataList.UserDetails.JanAadharNo;
                        gopalCreditCardFormModel.Encrypted_JanAadharNo = dataList.UserDetails.Encrypted_JanAadharNo;
                        //gopalCreditCardFormModel.JanAadharMID = dataList.UserDetails.JanAadharMID;
                        gopalCreditCardFormModel.Encrypted_JanAadharMID = dataList.UserDetails.Encrypted_JanAadharMID;
                        gopalCreditCardFormModel.JanAadharEnrollmentId = dataList.UserDetails.JanAadharEnrollmentId;
                        gopalCreditCardFormModel.AadharNo = dataList.UserDetails.AadharNo;
                        gopalCreditCardFormModel.Name_ENG = dataList.UserDetails.Name_ENG;
                        gopalCreditCardFormModel.Name_HI = dataList.UserDetails.Name_HI;
                        gopalCreditCardFormModel.FatherName_ENG = dataList.UserDetails.FatherName_ENG;
                        gopalCreditCardFormModel.FatherName_HI = dataList.UserDetails.FatherName_HI;
                        gopalCreditCardFormModel.MotherName_ENG = dataList.UserDetails.MotherName_ENG;
                        gopalCreditCardFormModel.MotherName_HI = dataList.UserDetails.MotherName_HI;
                        gopalCreditCardFormModel.SpouseName_ENG = dataList.UserDetails.SpouseName_ENG;
                        gopalCreditCardFormModel.SpouseName_HI = dataList.UserDetails.SpouseName_HI;
                        gopalCreditCardFormModel.MaritalStatus = dataList.UserDetails.MaritalStatus;
                        gopalCreditCardFormModel.RelationType = dataList.UserDetails.RelationType;
                        gopalCreditCardFormModel.DOB = dataList.UserDetails.DOB;
                        gopalCreditCardFormModel.Age = dataList.UserDetails.Age;
                        gopalCreditCardFormModel.Gender = dataList.UserDetails.Gender;
                        gopalCreditCardFormModel.MobileNo = dataList.UserDetails.MobileNo;
                        gopalCreditCardFormModel.BankAccountNo = ""; //dataList.UserDetails.BankAccountNo;
                        gopalCreditCardFormModel.BankName = ""; //dataList.UserDetails.BankName;
                        gopalCreditCardFormModel.BankIFSCCode = ""; //dataList.UserDetails.BankIFSCCode;
                        gopalCreditCardFormModel.BankBranchName = ""; //dataList.UserDetails.BankBranchName;
                        gopalCreditCardFormModel.Income = dataList.UserDetails.Income;
                        gopalCreditCardFormModel.Occupation = dataList.UserDetails.Occupation;
                        gopalCreditCardFormModel.PANCardNo = dataList.UserDetails.PANCardNo;
                        gopalCreditCardFormModel.Caste = dataList.UserDetails.Caste;
                        gopalCreditCardFormModel.Category = dataList.UserDetails.Category;
                        gopalCreditCardFormModel.Address_ENG = dataList.UserDetails.Address.Address_ENG;
                        gopalCreditCardFormModel.Original_Address_ENG = dataList.UserDetails.Address.Address_ENG;
                        gopalCreditCardFormModel.Address_HI = dataList.UserDetails.Address.Address_HI;
                        gopalCreditCardFormModel.Original_Address_HI = dataList.UserDetails.Address.Address_HI;
                        gopalCreditCardFormModel.PinCode = dataList.UserDetails.Address.PinCode;
                        gopalCreditCardFormModel.Original_PinCode = dataList.UserDetails.Address.PinCode;
                        gopalCreditCardFormModel.DistrictName_ENG = dataList.UserDetails.Address.DistrictName_ENG;
                        gopalCreditCardFormModel.DistrictName_HI = dataList.UserDetails.Address.DistrictName_HI;
                        gopalCreditCardFormModel.District_Code = dataList.UserDetails.Address.District_Code;
                        gopalCreditCardFormModel.Original_District_Code = dataList.UserDetails.Address.District_Code;

                        gopalCreditCardFormModel.IsRural = dataList.UserDetails.Address.IsRural;
                        gopalCreditCardFormModel.Original_IsRural = dataList.UserDetails.Address.IsRural;
                        if (dataList.UserDetails.Address.IsRural == true)
                        {
                            gopalCreditCardFormModel.Block_ENG = dataList.UserDetails.Address.Block_ENG;
                            gopalCreditCardFormModel.Block_HI = dataList.UserDetails.Address.Block_HI;
                            gopalCreditCardFormModel.Block_Code = dataList.UserDetails.Address.Block_Code;
                            gopalCreditCardFormModel.Original_Block_Code = dataList.UserDetails.Address.Block_Code;
                            gopalCreditCardFormModel.GP_ENG = dataList.UserDetails.Address.GP_ENG;
                            gopalCreditCardFormModel.GP_HI = dataList.UserDetails.Address.GP_HI;
                            gopalCreditCardFormModel.GP_Code = dataList.UserDetails.Address.GP_Code;
                            gopalCreditCardFormModel.Original_GP_Code = dataList.UserDetails.Address.GP_Code;
                            gopalCreditCardFormModel.Village_ENG = dataList.UserDetails.Address.Village_ENG;
                            gopalCreditCardFormModel.Village_HI = dataList.UserDetails.Address.Village_HI;
                            gopalCreditCardFormModel.Village_Code = dataList.UserDetails.Address.Village_Code;
                            gopalCreditCardFormModel.Original_Village_Code = dataList.UserDetails.Address.Village_Code;
                        }
                        else
                        {
                            gopalCreditCardFormModel.City_ENG = dataList.UserDetails.Address.City_ENG;
                            gopalCreditCardFormModel.City_HI = dataList.UserDetails.Address.City_HI;
                            gopalCreditCardFormModel.City_Code = dataList.UserDetails.Address.City_Code;
                            gopalCreditCardFormModel.Original_City_Code = dataList.UserDetails.Address.City_Code;
                        }

                        gopalCreditCardFormModel.MemberPhoto_Base64 = dataList.UserDetails.MemberPhoto_Base64;

                        gopalCreditCardFormModel.LoanDistrictId = "0";
                        gopalCreditCardFormModel.LoanBankID = "0";
                        gopalCreditCardFormModel.LoanBranchId = "0";
                        gopalCreditCardFormModel.PACS_ID = "0";


                        /*MahilaNidhi*/
                        //gopalCreditCardFormModel.Mn_DistrictId = 0;
                        //gopalCreditCardFormModel.Mn_BlockId =0;
                        //gopalCreditCardFormModel.Mn_ClusterId = "0";
                        //gopalCreditCardFormModel.Mn_VoId = "0";
                        //gopalCreditCardFormModel.Mn_ShgId = "0";
                        //gopalCreditCardFormModel.Mn_DistrictName = "";
                        //gopalCreditCardFormModel.Mn_BlockName = "";                          
                        //gopalCreditCardFormModel.Mn_ClusterName = "";
                        //gopalCreditCardFormModel.Mn_VoName = "";
                        //gopalCreditCardFormModel.Mn_ShgName = "";


                        //List<JanAadharModel.JanAaadharFamilyMemberModel> familyDataModel = new List<JanAadharModel.JanAaadharFamilyMemberModel>();
                        var familyDataModel = (List<JanAadharModel.JanAaadharFamilyMemberModel>)Session["JanAadharFamilyDataModel"];
                        var familyDataModelNew = familyDataModel.Where(x => x.Encrypted_JAN_MEMBER_ID != gopalCreditCardFormModel.Encrypted_JanAadharMID).ToList();


                        List<GopalCreditCardModel.JanAaadharFamilyMemberModel> gopalFamilyDataModel = new List<GopalCreditCardModel.JanAaadharFamilyMemberModel>();
                        //for (int i = 0; i < familyDataModelNew.Count; i++)
                        //{
                        //    GopalCreditCardModel.JanAaadharFamilyMemberModel janModel = new GopalCreditCardModel.JanAaadharFamilyMemberModel();

                        //    janModel.NAME = familyDataModelNew[i].NAME;
                        //    janModel.NAME_HINDI = familyDataModelNew[i].NAME_HINDI;

                        //    gopalFamilyDataModel.Add(janModel);
                        //}
                        //gopalCreditCardFormModel.janAaadharFamilyMemberModels = gopalFamilyDataModel;


                        familyDataModelNew.ForEach(x =>
                        {
                            GopalCreditCardModel.JanAaadharFamilyMemberModel janModel = new GopalCreditCardModel.JanAaadharFamilyMemberModel();

                            janModel.NAME = x.NAME;
                            janModel.NAME_HI = x.NAME_HINDI;
                            janModel.FATHER_NAME_ENG = x.FATHER_NAME_ENG;
                            janModel.FATHER_NAME_HND = x.FATHER_NAME_HND;
                            janModel.GENDER = x.GENDER;
                            janModel.MEMBER_TYPE = x.MEMBER_TYPE;
                            janModel.Masked_MOBILE_NO = x.Masked_MOBILE_NO;
                            janModel.Encrypted_MOBILE_NO = x.Encrypted_MOBILE_NO;
                            janModel.JAN_AADHAR = x.JAN_AADHAR;
                            janModel.Encrypted_JAN_AADHAR = x.Encrypted_JAN_AADHAR;
                            janModel.Masked_JAN_MEMBER_ID = x.Masked_JAN_MEMBER_ID;
                            janModel.Encrypted_JAN_MEMBER_ID = x.Encrypted_JAN_MEMBER_ID;
                            janModel.Encrypted_AADHAR_REF_NO = x.Encrypted_AADHAR_REF_NO;
                            janModel.Age = x.Age ?? 0;

                            gopalFamilyDataModel.Add(janModel);
                        });
                        gopalCreditCardFormModel.janAaadharFamilyMemberList = gopalFamilyDataModel;
                    }


                    var json = new JavaScriptSerializer().Serialize(gopalCreditCardFormModel);
                    return json;
                }
                //}
                //else
                //{
                //    //AlertMessage("Fail");
                //}
                //var json = new JavaScriptSerializer().Serialize(operationResult);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = " PassValidateJanAadharOTP(JanAadharModel.JanAaadharFamilyMemberModel model)" }, ex);
                return null;
            }
            return "";
        }




        public class OperationResult
        {
            public bool State { get; set; }
            public bool Status { get; set; }
            public string ErrorMessage { get; set; }
            public string SuccessMessage { get; set; }
            public string Content { get; set; }
        }
        private static string GetValidFileName(string fileName)
        {
            // remove any invalid character from the filename.
            String ret = Regex.Replace(fileName.Trim(), "[^A-Za-z0-9_. ]+", "");
            return ret.Replace(" ", String.Empty);
        }
        [HttpPost]
        [ActionName("UploadDocument_WithUploadtedFileType")]
        public string UploadDocument_WithUploadtedFileType(string FileTypeName)
        {
            OperationResult result = new OperationResult();
            //var result = new OperationResult<List<UploadFileWithPath_WithUploadtedFileTypeDataModel>>();
            try
            {
                //HttpRequestMessage request = this.Request;
                var httpPostedFile = HttpContext.Request.Files["file"];
                foreach (string files in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[files];


                    var size = file.InputStream.Length;
                    if (size > (1 * 1024 * 1024))
                    {
                        result.State = false;
                        result.ErrorMessage = "File size is bigger than 1 MB.!";
                    }
                    // 2. set the file uploaded folder
                    string subPath = "/Content/GopalCreditCard/";
                    // 3. check for the file length, if it is more than 0 the save it
                    if (file.InputStream.Length > 0)
                    {
                        //if (headerValues.Contains("UploadtedFileType"))
                        //{
                        //    var myValue = Request.Form.FirstOrDefault(x => x.Key == "UploadtedFileType").Value.FirstOrDefault();
                        //    if (!String.IsNullOrEmpty(myValue))
                        //    {
                        //        subPath = subPath + "/" + myValue;
                        //    }
                        //}
                        //else
                        //{
                        //    subPath = subPath + "/DefaultPath";
                        //}

                        // create directory if not exists
                        string DirPath = Server.MapPath(subPath);
                        bool exists = System.IO.Directory.Exists(DirPath);
                        if (!exists)
                            System.IO.Directory.CreateDirectory(DirPath);


                        string uploadFolder = System.IO.Path.Combine(Directory.GetCurrentDirectory(), subPath);



                        // 3a. read the file name of the received file
                        //var Dis_fileName = ContentDispositionHeaderValue.Parse(postedFile.ContentDisposition).FileName.Trim('"');

                        string Dis_fileName = httpPostedFile.FileName;
                        Dis_fileName = GetValidFileName(Dis_fileName);

                        //string fileWithOutExt = Path.GetFileNameWithoutExtension(file.FileName);


                        var filePath = FileTypeName + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_" + Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + ".pdf";
                        var tempFilePath = FileTypeName + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_" + Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + "_temp.pdf";

                        string finalFilePath = DirPath + filePath;
                        if (Request.Files != null)
                        {
                            try
                            {
                                HttpPostedFileBase file1 = Request.Files[0];
                                if (Path.GetExtension(tempFilePath) == ".pdf")
                                {
                                    file1.SaveAs(tempFilePath);
                                    filePath = WatermarkService.AddWatermarkToPdf(tempFilePath, filePath, "www.rajsahakarapp.rajasthan.gov.in");
                                    if (System.IO.File.Exists(tempFilePath))
                                    {
                                        System.IO.File.Delete(tempFilePath);
                                    }
                                }
                                //foreach (string files1 in Request.Files)
                                //{
                                //    HttpPostedFileBase file1 = Request.Files[files1];
                                //    //fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                                //    //if (files.ToString() == "Photo")
                                //    //{
                                //    //_Model.Photo = fileName;
                                //    file1.SaveAs(finalFilePath);
                                //    //}
                                //}

                            }
                            catch { }
                        }


                        //var finalPath = Path.Combine(uploadFolder, FileName);
                        //using (var fileStream = new FileStream(finalPath, FileMode.Create))
                        //{
                        //    HttpPostedFile.CopyTo(fileStream);
                        //}
                        //using (FileStream pngStream = new FileStream(finalPath, FileMode.OpenOrCreate))
                        //{
                        //    ResizeImage(pngStream, finalPathSave);
                        //}
                        result.State = true;
                        result.SuccessMessage = "File is uploaded Successfully.!";
                        result.Content = filePath;
                        var json = new JavaScriptSerializer().Serialize(result);
                        return json;
                    }
                    else
                    {
                        //result.State = OperationState.Error;
                        //result.ErrorMessage = "The File is not received.!";
                    }

                }



            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = " UploadDocument_WithUploadtedFileType(string FileTypeName)" }, ex);
                return null;
            }
            return "";
        }


        [HttpPost]
        [ActionName("UploadDocument_WithUploadtedFileType_Insameformat")]
        public string UploadDocument_WithUploadtedFileType_Insameformat(string FileTypeName)
        {
            OperationResult result = new OperationResult();
            try
            {

                var httpPostedFile = HttpContext.Request.Files["file"];


                if (Request.Files.Count > 0)
                {
                    try
                    {
                        HttpPostedFileBase file = Request.Files[0];


                        if (file != null && file.ContentLength > 0)
                        {
                            string fileExtension = Path.GetExtension(file.FileName);

                            var fileName = FileTypeName + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_" + Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + fileExtension;
                            var tempFileName = FileTypeName + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_" + Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + "_temp" + fileExtension;

                            var filePath = Path.Combine(Server.MapPath("~/Content/GopalCreditCard/"), fileName);
                            var tempFilePath = Path.Combine(Server.MapPath("~/Content/GopalCreditCard/"), tempFileName);

                            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                            }
                            if (fileExtension.ToLower() == ".pdf")
                            {
                                file.SaveAs(tempFilePath);
                                filePath = WatermarkService.AddWatermarkToPdf(tempFilePath, filePath, "rajsahakarapp.rajasthan.gov.in ");
                                if (System.IO.File.Exists(tempFilePath))
                                {
                                    System.IO.File.Delete(tempFilePath);
                                }
                            }
                            else
                                using (var image = System.Drawing.Image.FromStream(httpPostedFile.InputStream))
                                {
                                    filePath = WatermarkService.AddWatermarkToImage(image, filePath, "rajsahakarapp.rajasthan.gov.in");
                                }

                            result.State = true;
                            result.SuccessMessage = "File is uploaded Successfully.!";
                            result.Content = fileName;
                            var json = new JavaScriptSerializer().Serialize(result);
                            return json;
                        }
                    }
                    catch (Exception ex)
                    {
                        result.State = false;
                        result.SuccessMessage = "The File is not received.!";
                        result.Content = "";
                        var json = new JavaScriptSerializer().Serialize(result);
                        return json;
                    }
                }

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = " UploadDocument_WithUploadtedFileType_Insameformat(string FileTypeName)" }, ex);
                result.State = false;
                result.SuccessMessage = "The File is not received.!";
                result.Content = "";
                var json = new JavaScriptSerializer().Serialize(result);
                return json;
            }
            return "";
        }
        private bool CheckIfImageWithPDFFile(string FileName)
        {
            var extension = "." + FileName.Split('.');
            return (extension.ToLower() == ".pdf" || extension.ToLower() == ".bmp" || extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".gif" || extension.ToLower() == ".png" || extension.ToLower() == ".xlsx" || extension.ToLower() == ".xls" || extension.ToLower() == ".doc" || extension.ToLower() == ".docx" || extension.ToLower() == ".dot" || extension.ToLower() == ".wps" || extension.ToLower() == ".docm" || extension.ToLower() == ".dotm"); // Change the extension based on your need .xlsx, .xls
        }

        [HttpPost]
        [CustomAuthentication]
        [ValidateInput(false)]
        public async Task<string> Save_GopalCreditCardForm(GopalCreditCardFormModel gopalCreditCard)
        {
            GCCServiceModel _gccServiceModel = new GCCServiceModel(new DBLogger());
            var resOdData = new CBSGCCResponseModel();
            if (gopalCreditCard.GuarantorDetailsList.Count > 1)
            {
                if (gopalCreditCard.GuarantorDetailsList[0].JanMemberId != null && gopalCreditCard.GuarantorDetailsList[0].JanMemberId != "" &&
                    gopalCreditCard.GuarantorDetailsList[1].JanMemberId != null && gopalCreditCard.GuarantorDetailsList[1].JanMemberId != "" && gopalCreditCard.GuarantorDetailsList[0].JanMemberId != gopalCreditCard.GuarantorDetailsList[1].JanMemberId)
                {
                    if (gopalCreditCard.PANCardNo?.Length > 13)
                        gopalCreditCard.PANCardNo = csCommon.Decrypt_SA(Uri.UnescapeDataString(gopalCreditCard.PANCardNo));

                    resOdData = await _gccServiceModel.CheckGcctoCBSByJanMemberId(Convert.ToInt32(gopalCreditCard.LoanBankID), gopalCreditCard.JanAadharMID);
                    int cbsOdResult = 0;
                    if (resOdData != null)
                    {
                        cbsOdResult = resOdData.ODFlag == "N" ? 0 : (resOdData.ODFlag == null || resOdData.ODFlag == "Null") ? 2 : 1;
                    }
                    if (cbsOdResult == 0)
                    {
                        //var result = new OperationResult<List<UploadFileWithPath_WithUploadtedFileTypeDataModel>>();
                        try
                        {
                            DataTable DT = null;
                            OperationResult operationResult = new OperationResult();
                            GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());

                            //DT = objGopalCreditCardDBModel.CheckGCCRegistrationStatus(GopalCreditCardModel.JanAadharNo);
                            //if (GopalCreditCardModel.JanAadharMID!=null && GopalCreditCardModel.JanAadharMID!="")

                            //    if (String.IsNullOrEmpty(GopalCreditCardModel.Encrypted_JanAadharMID) && GopalCreditCardModel.gcc_id==0 && !String.IsNullOrEmpty(GopalCreditCardModel.JanAadharMID))
                            //    {
                            //        GopalCreditCardModel.Encrypted_JanAadharMID = COMMON_FUNCTION.Encrypt(GopalCreditCardModel.JanAadharMID);
                            //        if (String.IsNullOrEmpty(GopalCreditCardModel.Encrypted_JanAadharNo))
                            //            GopalCreditCardModel.Encrypted_JanAadharNo = !String.IsNullOrEmpty(GopalCreditCardModel.JanAadharNoEnc) == true ? GopalCreditCardModel.JanAadharNoEnc : "";

                            //    }

                            var JanAadharMID = !String.IsNullOrEmpty(gopalCreditCard.Encrypted_JanAadharMID) == true ? COMMON_FUNCTION.Decrypt(gopalCreditCard.Encrypted_JanAadharMID) : "";



                            if (JanAadharMID != null && JanAadharMID != "")
                                DT = objGopalCreditCardDBModel.CheckGCCRegistrationStatus(JanAadharMID, gopalCreditCard.Encrypted_JanAadharNo);

                            if (DT != null || gopalCreditCard.gcc_id > 0)
                            {
                                if (DT != null && DT.Rows.Count > 0 && JanAadharMID != "" && (DT.Rows[0]["JanAadharMIDList"].ToString().Contains(COMMON_FUNCTION.Decrypt(gopalCreditCard.Encrypted_JanAadharMID))) && gopalCreditCard.gcc_id != 0)
                                {
                                    operationResult.Status = false;
                                    operationResult.ErrorMessage = "GCC_APPLICATIONALREADYEXIST";
                                }
                                else
                                {
                                    if (gopalCreditCard.IsRural == false)
                                    {
                                        operationResult.Status = false;
                                        operationResult.ErrorMessage = "GCC_IsRural_False";
                                    }
                                    //if (GopalCreditCardModel.DoesTheMilkSaleToDCS == false)
                                    //{
                                    //    operationResult.Status = false;
                                    //    operationResult.ErrorMessage = "GCC_DoesTheMilkSaleToDCS_False";
                                    //}

                                    //if (GopalCreditCardModel.IsRecommendationOfDCSUploaded == false)
                                    //{
                                    //    operationResult.Status = false;
                                    //    operationResult.ErrorMessage = "GCC_IsRecommendationOfDCSUploaded_False";
                                    //}

                                    if (gopalCreditCard.IsPACSBankAccountAvailable == false)
                                    {
                                        operationResult.Status = false;
                                        operationResult.ErrorMessage = "GCC_IsPACSBankAccountAvailable_False";
                                    }

                                    // Renewal condition
                                    if (gopalCreditCard.IsEligibleForRenewable == true)
                                    {
                                        operationResult.Status = true;
                                        operationResult.ErrorMessage = "GCC_APPLICATION_RENEWAL_ALLOWED";
                                    }
                                    else if (gopalCreditCard.IsSatisfiedWithInttCalcByPacs == true)// Closure condition (stop saving)
                                    {
                                        operationResult.Status = false;
                                        operationResult.ErrorMessage = "GCC_Closer_Application";
                                        var jsons = new JavaScriptSerializer().Serialize(operationResult);
                                        return jsons;
                                    }

                                    #region Code to Save form
                                    gopalCreditCard.SSOID = _LoggedInUser.SSOID;
                                    gopalCreditCard.UserType = _LoggedInUser.UserType;
                                    gopalCreditCard.KIOSKCODE = _LoggedInUser.KIOSKCODE;
                                    gopalCreditCard.CreatedBy = gopalCreditCard.UpdatedBy = _LoggedInUser.UserId;
                                    gopalCreditCard.CreatedByIP = gopalCreditCard.UpdatedByIP = COMMON_FUNCTION.GetIpAddress();

                                    if (gopalCreditCard.gcc_id == 0 && !gopalCreditCard.IsEligibleForRenewable)
                                    {
                                        #region Save Member base 64 file
                                        string subPath = "/Content/GopalCreditCard/";
                                        string DirPath = Server.MapPath(subPath);
                                        bool exists = System.IO.Directory.Exists(DirPath);
                                        if (!exists)
                                            System.IO.Directory.CreateDirectory(DirPath);

                                        var FileName = "GopalakPhoto_" + System.DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_" + Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + ".jpg";

                                        string finalFilePath = DirPath + FileName;


                                        FileStream fs = new FileStream(finalFilePath, FileMode.CreateNew, FileAccess.Write);
                                        byte[] imageBytes = Convert.FromBase64String(gopalCreditCard.MemberPhoto_Base64);
                                        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                        ms.WriteTo(fs);
                                        fs.Close();
                                        fs.Dispose();
                                        ms.Flush();

                                        gopalCreditCard.MemberPhoto = FileName;
                                        #endregion

                                    }
                                    string Result = "-1";
                                    string ReturnJSON = "";

                                    //GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());
                                    string jsonStr = JsonConvert.SerializeObject(gopalCreditCard);
                                    COMMON_FUNCTION.Sp_callTrackingInsert("Save_GopalCreditCardForm", jsonStr);
                                    Result = objGopalCreditCardDBModel.Save_GopalCreditCardForm(gopalCreditCard);


                                    COMMON_FUNCTION.Sp_callTrackingInsert("Save_GopalCreditCardForm_Result", Result);
                                    string[] arr = Result.Split('@');
                                    if (arr != null && arr.Length > 1 && !String.IsNullOrEmpty(arr[1]))
                                    {
                                        if (Convert.ToInt32(arr[1].ToString()) > 0)
                                        {
                                            operationResult.Status = true;
                                            operationResult.Content = arr[2].ToString();

                                        }
                                        else { operationResult.Status = false; }
                                    }
                                    else
                                    {
                                        operationResult.Status = false;
                                        //operationResult.ErrorMessage = string.IsNullOrEmpty(arr[0].ToString()) ? "Something went wrong, please try again" : arr[0].ToString();
                                        operationResult.ErrorMessage = string.IsNullOrEmpty(arr[0].ToString()) ? "Registered Detail Already Exists" : arr[0].ToString();
                                    }



                                    //ReturnJSON = "{\"data\":\"" + Result.Replace("@","").ToString() + "\",\"usertype\":\"" + GopalCreditCardModel.CreatedBy.ToString() + "\"}";
                                    #endregion
                                }
                            }
                            else
                            {
                                operationResult.Status = false;
                                operationResult.ErrorMessage = "Some Error Occured!";
                            }

                            var json = new JavaScriptSerializer().Serialize(operationResult);
                            return json;

                        }
                        catch (Exception ex)
                        {
                            DBLogger objDBLogger = new DBLogger();
                            objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = " Save_GopalCreditCardForm(GopalCreditCardModel.GopalCreditCardFormModel GopalCreditCardModel)" }, ex);
                            //return null;
                        }
                        return "";
                    }
                    else
                    {
                        if (cbsOdResult == 1)
                        {
                            return "GCC_APPLICATION_OD";
                        }
                        else
                        {
                            return "GCC_APPLICATION_RETRY";
                        }
                    }
                }
                else
                {
                    return "GCC_APPLICATION_GURANTOR";
                }
            }
            else
            {
                return "GCC_APPLICATION_GURANTOR";
            }
        }

        public ActionResult GopalApplicationFormView(string eId, string eRno)
        {
            //var currentUri = new Uri(HttpContext.Request.Url.Scheme + "://" +
            //       HttpContext.Request.Url.Authority +
            //       HttpContext.Request.RawUrl);
            //var queryStringCollection = HttpUtility.ParseQueryString((currentUri).Query);
            //var a= queryStringCollection.GetValues("eId");

            if (!String.IsNullOrEmpty(eId))
            {
                eId = eId.Replace(' ', '+');
                eId = eId.Replace(' ', '+');
                eId = eId.Replace(' ', '+');
            }
            if (!String.IsNullOrEmpty(eRno))
            {
                eRno = eRno.Replace(' ', '+');
                eRno = eRno.Replace(' ', '+');
                eRno = eRno.Replace(' ', '+');
            }
            UserModel OBJUser = new UserModel(new FileLogger());
            bool Result = OBJUser.IsUserAuthenticate(_LoggedInUser.UserId, _LoggedInUser.RoleId);

            if (Result == false)
            {
                return Redirect(String.Format("/Error/UnAuthorizeAccess401"));
            }
            else
            {
                if (!String.IsNullOrEmpty(eId) && !String.IsNullOrEmpty(eRno))
                {
                    System.Web.HttpContext.Current.Session[SessionIdentity.GCCEId] = eId;
                    System.Web.HttpContext.Current.Session[SessionIdentity.GCCERNO] = eRno;
                }
                ViewBag.Title = PageTitleMetaSetting.SetPageTitle(PageTitleFactory.GopalCreditCardRegistration, (Request.Cookies[SessionIdentity.Culture]).Value);
            }

            return View();
        }
        [HttpPost]
        public string Get_GopalApplicationFormViewData(string eid)
        {
            try
            {
                DataTable DT = null;

                string GCCEId = System.Web.HttpContext.Current.Session[SessionIdentity.GCCEId].ToString();
                string GCCERNO = System.Web.HttpContext.Current.Session[SessionIdentity.GCCERNO].ToString();
                if (!String.IsNullOrEmpty(GCCEId) && !String.IsNullOrEmpty(GCCERNO))
                {
                    //int id = Convert.ToInt32(COMMON_FUNCTION.Decrypt(GCCEId));
                    string RNO = COMMON_FUNCTION.Decrypt(GCCERNO);

                    GopalCreditCardModel.GopalCreditCardFormModel gopalCreditCardFormModel = new GopalCreditCardModel.GopalCreditCardFormModel();

                    GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());
                    DT = objGopalCreditCardDBModel.Get_GopalApplicationFormViewData(RNO, "MainData");

                    //return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);

                    if (DT != null)
                    {
                        var jsonData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);

                        var resultDataModel = JsonConvert.DeserializeObject<List<GopalCreditCardModel.GopalCreditCardFormModel>>(jsonData);

                        var dataList = resultDataModel.FirstOrDefault();
                        GopalCreditCardModel.JanAaadharFamilyMemberModel familyModel = new GopalCreditCardModel.JanAaadharFamilyMemberModel();
                        DataTable DTMember = null;
                        DTMember = objGopalCreditCardDBModel.Get_GopalApplicationFormViewData(RNO, "MemberData");

                        var jsonDataMember = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DTMember);

                        var resultDataModelMember = JsonConvert.DeserializeObject<List<GopalCreditCardModel.JanAaadharFamilyMemberModel>>(jsonDataMember);

                        dataList.janAaadharFamilyMemberList = resultDataModelMember;

                        var json = new JavaScriptSerializer().Serialize(dataList);

                        return json; ;


                    }
                    else
                    {
                        //Session["MemberDataTable"] = "";
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = " Get_GopalApplicationFormViewData(string eid)" }, ex);
                return null;
            }
        }
        [HttpPost]
        public string Get_DCSDetailByRegistrationNo(string dscRno)
        {
            try
            {
                DataTable DT = null;

                GopalCreditCardModel.GopalCreditCardFormModel gopalCreditCardFormModel = new GopalCreditCardModel.GopalCreditCardFormModel();

                GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());
                DT = objGopalCreditCardDBModel.Get_DCSDetailByRegistrationNo(dscRno);

                //return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);

                if (DT != null)
                {
                    var jsonData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);

                    var resultDataModel = JsonConvert.DeserializeObject<List<GopalCreditCardModel.GopalCreditCardFormModel>>(jsonData);

                    var dataList = resultDataModel.FirstOrDefault();
                    if (dataList != null)
                    {
                        gopalCreditCardFormModel.DCSName = dataList.DCSName;
                        gopalCreditCardFormModel.DCSOwnerName = dataList.DCSOwnerName;
                        gopalCreditCardFormModel.DCSOwnerMobileNo = dataList.DCSOwnerMobileNo;
                        gopalCreditCardFormModel.DCSAddress = dataList.DCSAddress;
                    }

                    var json = new JavaScriptSerializer().Serialize(gopalCreditCardFormModel);
                    return json;
                }
                else
                {
                    //Session["MemberDataTable"] = "";
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = " Get_DCSDetailByRegistrationNo(string dscRno)" }, ex);
                return null;
            }
        }

        [HttpPost]
        public string Get_CCBList(string id)
        {
            try
            {
                int DistrictId = 0;
                if (!String.IsNullOrEmpty(id))
                {
                    DistrictId = Convert.ToInt32(id);
                }
                EntryFormsModel objEntryForms = new EntryFormsModel(new DBLogger());
                DataTable dtCCB = objEntryForms.CCBData_GCC(0, DistrictId);
                if (dtCCB != null)
                {
                    var jsonData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(dtCCB);

                    return jsonData;
                }
                else
                {
                    //Session["MemberDataTable"] = "";
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = " Get_CCBList(string id)" }, ex);
                return null;
            }
        }
        [HttpPost]
        public string Get_LoanBankBranch(string LoanDistrictId, string LoanBankId)
        {
            try
            {
                int DistrictId = 0, BankId = 0;
                if (!String.IsNullOrEmpty(LoanDistrictId))
                {
                    DistrictId = Convert.ToInt32(LoanDistrictId);
                }
                if (!String.IsNullOrEmpty(LoanBankId))
                {
                    BankId = Convert.ToInt32(LoanBankId);
                }

                GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());
                DataTable dtCCB = objGopalCreditCardDBModel.Get_LoanBankBranch(DistrictId, BankId);

                if (dtCCB != null)
                {
                    var jsonData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(dtCCB);

                    return jsonData;
                }
                else
                {
                    //Session["MemberDataTable"] = "";
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = " Get_LoanBankBranch(string LoanDistrictId, string LoanBankId)" }, ex);
                return null;
            }
        }
        [HttpPost]
        public string Get_LoanBranch_PACS(string LoanBranchId)
        {
            try
            {
                int BranchId = 0;
                if (!String.IsNullOrEmpty(LoanBranchId))
                {
                    BranchId = Convert.ToInt32(LoanBranchId);
                }

                GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());
                DataTable dtCCB = objGopalCreditCardDBModel.Get_LoanBranch_PACS(BranchId);

                if (dtCCB != null)
                {
                    var jsonData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(dtCCB);

                    return jsonData;
                }
                else
                {
                    //Session["MemberDataTable"] = "";
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = " Get_LoanBranch_PACS(string LoanBranchId)" }, ex);
                return null;
            }
        }
        [HttpPost]
        public string CheckGCCRegistrationStatus(string JanAadharNo)
        {
            JanAadharNo = csCommon.Decrypt_SA(Uri.UnescapeDataString(JanAadharNo));

            try
            {
                DataTable DT = null;
                OperationResult operationResult = new OperationResult();
                GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());
                DT = objGopalCreditCardDBModel.CheckGCCRegistrationStatus("", COMMON_FUNCTION.Encrypt(JanAadharNo));

                if (DT != null)
                {
                    if (DT.Rows.Count > 0)
                    {
                        operationResult.State = true;
                        operationResult.Status = false;
                    }
                    else
                    {
                        operationResult.State = true;
                        operationResult.Status = true;
                    }
                }
                else
                {
                    operationResult.State = false;
                    operationResult.Status = false;
                }
                var json = new JavaScriptSerializer().Serialize(operationResult);
                return json;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = " CheckGCCRegistrationStatus(string JanAadharNo)" }, ex);
                return null;
            }
        }


        #region Madhuri

        [CustomAuthentication]
        [HttpPost]
        [ActionName("GetGCCFlowList")]
        public JsonResult GetGCCFlowList(DataPagingModel Details, bool isExport = false)
        {
            GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());
            DataTable DT = objGopalCreditCardDBModel.GetGCCFlowList(ref Details, isExport);
            if (isExport)
            {
                var filename = new ExcelGenerator(new FileLogger()).GenerateExcel(DT, "Gopal-Credit-Card-Application-List.xlsx");
                return Json(new { DataList = filename, DataPaging = Details }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details }, JsonRequestBehavior.AllowGet);

        }

        //old
        //[HttpGet]
        //[ActionName("GetApplicationFlowGCC")]
        //public string GetApplicationFlowGCC(string GCC_Id = "")
        //{
        //    // COMMON_FUNCTION.SSO_AADHAR_OTP_VALIDATION 

        //    GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());
        //    return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objGopalCreditCardDBModel.GetApplicationFlowGCC(GCC_Id));
        //}


        [HttpGet]
        [ActionName("GetApplicationFlowGCC")]
        public string GetApplicationFlowGCC(string GCC_Id = "")
        {
            GopalCreditCardDBModel obj = new GopalCreditCardDBModel(new DBLogger());
            var ds = obj.GetApplicationFlowGCC(GCC_Id);
            var result = new
            {
                MainFlow = COMMON_FUNCTION.DataTableToJSONWithJSONNet(ds.Tables[0]),
                OldFlow = COMMON_FUNCTION.DataTableToJSONWithJSONNet(ds.Tables[1])
            };


            return JsonConvert.SerializeObject(result);

        }

        [HttpPost]
        public string Get_GopalApplicationFormViewData1(string GCCEId, string GCCERNO)
        {
            try
            {

                DataTable DT = null;
                if (!String.IsNullOrEmpty(GCCEId) && !String.IsNullOrEmpty(GCCERNO))
                {
                    GCCEId = GCCEId.Replace(' ', '+');
                    GCCERNO = GCCERNO.Replace(' ', '+');
                    //int id = Convert.ToInt32(COMMON_FUNCTION.Decrypt(GCCEId));

                    //Gccid encryption from db

                    //CoopEntities DB = new CoopEntities();
                    //int GccId = Convert.ToInt32(DB.GetDecrypt(GCCEId).FirstOrDefault());


                    string RNO = COMMON_FUNCTION.Decrypt(GCCERNO);
                    GopalCreditCardModel.GopalCreditCardFormModel gopalCreditCardFormModel = new GopalCreditCardModel.GopalCreditCardFormModel();

                    GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());
                    DT = objGopalCreditCardDBModel.Get_GopalApplicationFormViewData(RNO, "MainData");

                    if (DT != null)
                    {
                        var jsonData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
                        var resultDataModel = JsonConvert.DeserializeObject<List<GopalCreditCardModel.GopalCreditCardFormModel>>(jsonData);

                        var dataList = resultDataModel.FirstOrDefault();
                        gopalCreditCardFormModel = dataList;

                        GopalCreditCardModel.JanAaadharFamilyMemberModel familyModel = new GopalCreditCardModel.JanAaadharFamilyMemberModel();
                        DataTable DTMember = null;
                        DTMember = objGopalCreditCardDBModel.Get_GopalApplicationFormViewData(RNO, "MemberData");
                        var jsonDataMember = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DTMember);
                        var resultDataModelMember = JsonConvert.DeserializeObject<List<GopalCreditCardModel.JanAaadharFamilyMemberModel>>(jsonDataMember);
                        gopalCreditCardFormModel.janAaadharFamilyMemberList = resultDataModelMember;

                        //GccCropLoanData
                        CropLoanDetailsModel gccld = new CropLoanDetailsModel();
                        DataTable dtgccld = null;
                        dtgccld = objGopalCreditCardDBModel.Get_GopalApplicationFormViewData(RNO, "GccCropLoanData");
                        gopalCreditCardFormModel.CropLoanDetailsList = JsonConvert.DeserializeObject<List<CropLoanDetailsModel>>(COMMON_FUNCTION.DataTableToJSONWithJSONNet(dtgccld));

                        //GccCropLoanData
                        AgricultureLandDetailModel gccald = new AgricultureLandDetailModel();
                        DataTable dtgccald = null;
                        dtgccald = objGopalCreditCardDBModel.Get_GopalApplicationFormViewData(RNO, "GccAgricultureLand");
                        gopalCreditCardFormModel.AgricultureLandDetailList = JsonConvert.DeserializeObject<List<AgricultureLandDetailModel>>(COMMON_FUNCTION.DataTableToJSONWithJSONNet(dtgccald));

                        //GccCropLoanData
                        GuarantorDetailsModel gccgt = new GuarantorDetailsModel();
                        DataTable dtgccgt = null;
                        dtgccgt = objGopalCreditCardDBModel.Get_GopalApplicationFormViewData(RNO, "GccGuarantorData");
                        gopalCreditCardFormModel.GuarantorDetailsList = JsonConvert.DeserializeObject<List<GuarantorDetailsModel>>(COMMON_FUNCTION.DataTableToJSONWithJSONNet(dtgccgt));

                        var json = new JavaScriptSerializer().Serialize(gopalCreditCardFormModel);
                        return json;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = " Get_GopalApplicationFormViewData1(string GCCEId, string GCCERNO)" }, ex);
                return null;
            }
        }



        [HttpPost]
        public string Get_GopalApplicationFormViewData_v1(string GCCEId)
        {
            try
            {
                DataTable DT = null;

                if (!String.IsNullOrEmpty(GCCEId))
                {

                    //Gccid encryption from db

                    CoopEntities DB = new CoopEntities();
                    int GccId = Convert.ToInt32(DB.GetDecrypt(GCCEId).FirstOrDefault());


                    //  string RNO = COMMON_FUNCTION.Decrypt(GCCERNO);
                    GopalCreditCardModel.GopalCreditCardFormModel gopalCreditCardFormModel = new GopalCreditCardModel.GopalCreditCardFormModel();

                    GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());
                    DT = objGopalCreditCardDBModel.Get_GopalApplicationFormViewData_v1("MainData", GccId);

                    if (DT != null)
                    {
                        var jsonData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
                        var resultDataModel = JsonConvert.DeserializeObject<List<GopalCreditCardModel.GopalCreditCardFormModel>>(jsonData);

                        var dataList = resultDataModel.FirstOrDefault();


                        //if (dataList != null && dataList.JanAadharNoEnc != "")
                        //{
                        //    dataList.JanAadharNo = csCommon.Encrypt_SA(Uri.UnescapeDataString(COMMON_FUNCTION.Decrypt(dataList.JanAadharNoEnc)));
                        //    dataList.JanAadharNo = csCommon.Encrypt_SA(COMMON_FUNCTION.Masked(Uri.UnescapeDataString(COMMON_FUNCTION.Decrypt(dataList.JanAadharNoEnc))));
                        //}
                        //dataList.Encrypted_JanAadharNo = dataList.JanAadharNoEnc;

                        if (dataList != null && dataList.AadharNoEnc != "")
                            dataList.AadharNo = csCommon.Encrypt_SA(Uri.UnescapeDataString(COMMON_FUNCTION.Decrypt(dataList.AadharNoEnc)));

                        if (dataList != null && dataList.PANCardNoEnc != "")
                            dataList.PANCardNo = csCommon.Encrypt_SA(Uri.UnescapeDataString(COMMON_FUNCTION.Decrypt(dataList.PANCardNoEnc)));

                        if (dataList != null && dataList.Encrypted_JanAadharMID != "")
                            dataList.Encrypted_JanAadharMID = (COMMON_FUNCTION.Encrypt(dataList.JanAadharMID));


                        gopalCreditCardFormModel = dataList;

                        GopalCreditCardModel.JanAaadharFamilyMemberModel familyModel = new GopalCreditCardModel.JanAaadharFamilyMemberModel();
                        DataTable DTMember = null;
                        DTMember = objGopalCreditCardDBModel.Get_GopalApplicationFormViewData_v1("MemberData", GccId);
                        var jsonDataMember = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DTMember);
                        var resultDataModelMember = JsonConvert.DeserializeObject<List<GopalCreditCardModel.JanAaadharFamilyMemberModel>>(jsonDataMember);
                        gopalCreditCardFormModel.janAaadharFamilyMemberList = resultDataModelMember;

                        //GccCropLoanData
                        CropLoanDetailsModel gccld = new CropLoanDetailsModel();
                        DataTable dtgccld = null;
                        dtgccld = objGopalCreditCardDBModel.Get_GopalApplicationFormViewData_v1("GccCropLoanData", GccId);
                        gopalCreditCardFormModel.CropLoanDetailsList = JsonConvert.DeserializeObject<List<CropLoanDetailsModel>>(COMMON_FUNCTION.DataTableToJSONWithJSONNet(dtgccld));

                        //GccAgricultureLandData
                        AgricultureLandDetailModel gccald = new AgricultureLandDetailModel();
                        DataTable dtgccald = null;
                        dtgccald = objGopalCreditCardDBModel.Get_GopalApplicationFormViewData_v1("GccAgricultureLand", GccId);
                        gopalCreditCardFormModel.AgricultureLandDetailList = JsonConvert.DeserializeObject<List<AgricultureLandDetailModel>>(COMMON_FUNCTION.DataTableToJSONWithJSONNet(dtgccald));

                        //GccGuarantorData
                        GuarantorDetailsModel gccgt = new GuarantorDetailsModel();
                        DataTable dtgccgt = null;
                        dtgccgt = objGopalCreditCardDBModel.Get_GopalApplicationFormViewData_v1("GccGuarantorData", GccId);
                        gopalCreditCardFormModel.GuarantorDetailsList = JsonConvert.DeserializeObject<List<GuarantorDetailsModel>>(COMMON_FUNCTION.DataTableToJSONWithJSONNet(dtgccgt));

                        foreach (var i in gopalCreditCardFormModel.GuarantorDetailsList)
                        {
                            if (i.AadharNoEnc != "")
                            {
                                i.AadharNo = csCommon.Encrypt_SA(Uri.UnescapeDataString(COMMON_FUNCTION.Decrypt(i.AadharNoEnc)));
                                i.AadharNoMasked = "XXXX-XXXX-XX" + Uri.UnescapeDataString(COMMON_FUNCTION.Decrypt(i.AadharNoEnc)).Substring(10, 2);
                            }
                            if (i.JanAadharNoEnc != "")
                                i.JanAadharNumber = csCommon.Encrypt_SA(Uri.UnescapeDataString(COMMON_FUNCTION.Decrypt(i.JanAadharNoEnc)));
                            if (i.PANCardNoEnc != "")
                            {
                                i.PANCardNo = csCommon.Encrypt_SA(Uri.UnescapeDataString(COMMON_FUNCTION.Decrypt(i.PANCardNoEnc)));
                                i.PANCardNoMasked = "XXXXXXXX" + Uri.UnescapeDataString(COMMON_FUNCTION.Decrypt(i.PANCardNoEnc)).Substring(8, 2);
                            }
                        }


                        var json = new JavaScriptSerializer().Serialize(gopalCreditCardFormModel);
                        return json;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = " Get_GopalApplicationFormViewData_v1(string" + GCCEId + ")" }, ex);
                return null;
            }
        }

        [HttpPost]
        [ActionName("GSSProceesStatusList")]
        public string GetGSSProceesStatusList(string StatusId, string GCC_Id)
        {
            GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());
            int StatusIdInt;
            int.TryParse(StatusId, out StatusIdInt);
            return COMMON_FUNCTION.DataTableToJSONString(objGopalCreditCardDBModel.GetGCCProceesStatusListIdsByIds(StatusIdInt, GCC_Id));
        }


        public bool ContainsUnicodeCharacter(string input)
        {
            const int MaxAnsiCode = 255;
            return input.Any(c => c > MaxAnsiCode);
        }

        [HttpPost]
        [ActionName("SaveGSSApplicationFlowStatus_V1")]
        public JsonResult SaveGSSApplicationFlowStatus_V1(string OTP = "", string TXN = "", GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel = null)
        {
            string pdfPath = "";
            string succmsg = string.Empty;
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                if (_LoggedInUser == null)
                {
                    return Json("LOGOUT");
                }

                GopalCreditCardDBModel _Modal = new GopalCreditCardDBModel();
                if (!ModelState.IsValid)
                    return Json(JsonModelError.Get(ModelState.Values));

                int result = 0;

                if (TXN != "" || (Request.Url.Host.Contains("localhost") || Request.Url.Host.Contains("devitsandbox")))
                {
                    ResultMessage otpResult = new ResultMessage();

                    //if (!Request.Url.Host.Contains("localhost") && _AppFlowModel.StatusId == 4)
                    //{
                    //    otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                    //}

                    if (!Request.Url.Host.Contains("localhost") && !Request.Url.Host.Contains("devitsandbox"))
                    {
                        otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                    }
                    else
                    {
                        otpResult.status = "1"; otpResult.data = TXN;
                    }

                    if (otpResult.status == "1")
                    {
                        if (_AppFlowModel.StatusId == 19 || _AppFlowModel.StatusId == 4)
                        {
                            if (_AppFlowModel.StatusId == 19)
                                pdfPath = AllDocumentEsign(_Model, _AppFlowModel);

                            //changes by jaya
                            if (_AppFlowModel.StatusId == 4)
                            {
                                if (!string.IsNullOrEmpty(_Model.RegistrationNo) && new GopalCreditCardDBModel().CheckRegistrationNoExists(_Model.RegistrationNo))
                                {
                                    pdfPath = ReSanctionCertificateGenerate(_Model, _AppFlowModel);
                                }
                                else
                                {
                                    pdfPath = CertificateGenerate(_Model, _AppFlowModel);
                                }

                            }

                            if (!pdfPath.Contains("Fail"))
                            {
                                //var Filepath = pdfPath.Split('#');
                                string[] files = pdfPath.Split('#');
                                for (int x = 0; x < files.Count(); x++)
                                {
                                    if (files[x] != null && files[x] != "")
                                        files[x] = Server.MapPath("~/Content/GopalCreditCard/GssProfiling/") + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", "") + "/" + files[x];
                                }


                                if (!Request.Url.Host.Contains("localhost") && !Request.Url.Host.Contains("devitsandbox"))
                                {

                                    if (_AppFlowModel.StatusId == 19)
                                    {
                                        // var result1 = COMMON_FUNCTION.EsignPDF_NEW(otpResult.data, string.Join("#", files), _Model.LoanBankName_ENG, "Managing Director");
                                        //var result1 = COMMON_FUNCTION.EsinghPDF(otpResult.data, string.Join("#", files), _Model.LoanBankName_ENG, "Managing Director");

                                        //succmsg = result1.status;

                                        GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());

                                        pdfPath = GCCApplicationSignedForm(_Model, _AppFlowModel, 1);
                                        //_AppFlowModel.DocumentURL = pdfPath;
                                        GccDocumentsModel docAppForm = new GccDocumentsModel();
                                        docAppForm.DocumentURL = pdfPath;
                                        docAppForm.DocId = 7;
                                        docAppForm.GccId = _AppFlowModel.GccId;
                                        _AppFlowModel.DocId = objGopalCreditCardDBModel.InsertGCCReportFile(docAppForm);

                                        pdfPath = GCCApplicationSignedForm(_Model, _AppFlowModel, 2);
                                        //_AppFlowModel.DocumentURL = pdfPath;
                                        GccDocumentsModel docAppForm2 = new GccDocumentsModel();
                                        docAppForm2.DocumentURL = pdfPath;
                                        docAppForm2.DocId = 8;
                                        docAppForm2.GccId = _AppFlowModel.GccId;
                                        _AppFlowModel.DocId = objGopalCreditCardDBModel.InsertGCCReportFile(docAppForm2);

                                        pdfPath = GCCApplicationSignedForm(_Model, _AppFlowModel, 3);
                                        //_AppFlowModel.DocumentURL = pdfPath;
                                        GccDocumentsModel docAppForm3 = new GccDocumentsModel();
                                        docAppForm3.DocumentURL = pdfPath;
                                        docAppForm3.DocId = 9;
                                        docAppForm3.GccId = _AppFlowModel.GccId;
                                        _AppFlowModel.DocId = objGopalCreditCardDBModel.InsertGCCReportFile(docAppForm3);


                                        // succmsg = "1";
                                    }
                                    if (_AppFlowModel.StatusId == 4)
                                    {
                                        //var result1 = COMMON_FUNCTION.EsignPDF_NEW(otpResult.data, string.Join("#", files), _Model.PACSName_ENG, "Pacs Manager");
                                        var result1 = COMMON_FUNCTION.EsinghPDF(otpResult.data, string.Join("#", files), _Model.PACSName_ENG, "Pacs Manager");

                                        succmsg = result1.status;
                                    }

                                }

                                if (succmsg == "1" || (Request.Url.Host.Contains("localhost") || Request.Url.Host.Contains("devitsandbox")))
                                {
                                    result = _Modal.AddApplicationFlow_V1(_AppFlowModel);
                                    if (result > 0)
                                    {
                                        if (!Request.Url.Host.Contains("localhost") && !Request.Url.Host.Contains("devitsandbox"))
                                        {
                                            SendSms(_AppFlowModel.StatusId, _Model.MobileNo, _Model.RegistrationNo, _Model.gcc_id);
                                        }
                                        if (_AppFlowModel.StatusId == 19)
                                        {
                                            var status = _Modal.PushGCCTransactionToJanaadhar(_Model, _AppFlowModel, result);

                                            //JanAadhaarGenericService _service = new JanAadhaarGenericService();
                                            //var status = _service.PushGCCTransactionToJanaadhar(_Model, _AppFlowModel, result);
                                            //if (status != "")
                                            //{
                                            //    _Modal.InsertPushDataStatus(_Model.gcc_id, status);
                                            //}


                                        }
                                    }
                                }

                            }
                            else
                            {
                                result = 0;
                                succmsg = pdfPath;
                            }
                        }
                        else
                        {
                            string fullPath = "";
                            if (_AppFlowModel.DocumentURL != null && _AppFlowModel.DocumentURL != "")
                                fullPath = Server.MapPath("~/Content/GopalCreditCard/GSSProfiling/") + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", "") + "/" + _AppFlowModel.DocumentURL;


                            if (_AppFlowModel.StatusId == 7)
                            {
                                if (!Request.Url.Host.Contains("localhost") && !Request.Url.Host.Contains("devitsandbox"))
                                {
                                    pdfPath = GenerateMokaReport(_Model, _AppFlowModel);
                                    _AppFlowModel.DocumentURL = pdfPath;
                                    fullPath = Server.MapPath("~/Content/GopalCreditCard/GSSProfiling/") + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", "") + "/" + _AppFlowModel.DocumentURL;
                                    GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());
                                    GccDocumentsModel docMoka = new GccDocumentsModel();
                                    docMoka.DocumentURL = pdfPath;
                                    docMoka.DocId = 1;
                                    docMoka.GccId = _AppFlowModel.GccId;
                                    _AppFlowModel.DocId = objGopalCreditCardDBModel.InsertGCCReportFile(docMoka);

                                    // var result1 = COMMON_FUNCTION.EsignPDF_NEW(otpResult.data, fullPath, _Model.PACSName_ENG, "Pacs Manager");
                                    var result1 = COMMON_FUNCTION.EsinghPDF(otpResult.data, fullPath, _Model.PACSName_ENG, "Pacs Manager");

                                    succmsg = result1.status;
                                    // succmsg = "1";
                                }

                                if (succmsg == "1" || (Request.Url.Host.Contains("localhost") || Request.Url.Host.Contains("devitsandbox")))
                                {
                                    result = _Modal.AddApplicationFlow_V1(_AppFlowModel);
                                    _Modal.UpdateMokaReportData(_Model);
                                }
                            }
                            else if (_AppFlowModel.StatusId == 8 || _AppFlowModel.StatusId == 24)
                            {
                                //if (!Request.Url.Host.Contains("localhost") && !Request.Url.Host.Contains("devitsandbox"))
                                //{
                                //    if (fullPath != "")
                                //    {
                                //        //  var result1 = COMMON_FUNCTION.EsignPDF_NEW(otpResult.data, fullPath, _Model.LoanBranchName_ENG, "Branch Manager");
                                //        var result1 = COMMON_FUNCTION.EsinghPDF(otpResult.data, fullPath, _Model.LoanBranchName_ENG, "Branch Manager");

                                //        succmsg = result1.status;
                                //    }
                                //    // succmsg = "1";

                                //}
                                if (succmsg == "1" || (Request.Url.Host.Contains("localhost") || Request.Url.Host.Contains("devitsandbox")) || (_AppFlowModel.StatusId == 8 || _AppFlowModel.StatusId == 24))
                                {
                                    result = _Modal.AddApplicationFlow_V1(_AppFlowModel);
                                }

                            }

                            else
                            {
                                result = _Modal.AddApplicationFlow_V1(_AppFlowModel);
                            }

                            if (result > 0 && (_AppFlowModel.StatusId == 16 || _AppFlowModel.StatusId == 17 || _AppFlowModel.StatusId == 13 || _AppFlowModel.StatusId == 19 || _AppFlowModel.StatusId == 4))
                            {
                                SendSms(_AppFlowModel.StatusId, _Model.MobileNo, _Model.RegistrationNo, _Model.gcc_id);
                            }

                        }

                    }
                    else
                    {
                        result = -2;
                    }
                }
                else
                {
                    result = 0;
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = "SaveGSSApplicationFlowStatus_V1( string OTP = \"\", string TXN = \"\",  GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel=null)" }, ex);
                return null;
            }

        }


        public void SendSms(int statusId, string MobileNo = "", string RegistrationNO = "", int gccId = 0)
        {
            GopalCreditCardDBModel _Modal = new GopalCreditCardDBModel();
            try
            {
                if (MobileNo != null && MobileNo.Length == 10)
                {
                    DataTable MessageTable = new DataTable();
                    int MessageFlag = statusId;

                    if (Request.Url.Host.Contains("localhost") || Request.Url.Host.Contains("devitsandbox") || Request.Url.Host.Contains("Devitsandbox"))
                    {
                        MobileNo = "7568622727";
                    }

                    if (statusId == 16 || statusId == 17)
                    {
                        MessageTable = MessageOnActivity.GetMessageBodyActivityKey("SMS_ON_GccApplicationRejection");
                    }
                    else if (statusId == 13)
                    {
                        MessageTable = MessageOnActivity.GetMessageBodyActivityKey("SMS_ON_GccApplicationObjection");
                    }
                    else if (statusId == 4)
                    {
                        MessageTable = MessageOnActivity.GetMessageBodyActivityKey("SMS_ON_GccApplicationSanction");
                    }

                    if (MessageTable != null && MessageTable.Rows.Count > 0)
                    {
                        string Message = Convert.ToString(MessageTable.Rows[0]["SMS_MSG"]);
                        string templateID = Convert.ToString(MessageTable.Rows[0]["TemplateID"]);

                        if (!string.IsNullOrEmpty(Message))
                        {
                            Message = Message.Replace("##RegNo##", Convert.ToString(RegistrationNO));
                            try
                            {
                                new EsanchaarController().SendSocietySMS(Message, MobileNo, "HIN", templateID);
                                //new EsanchaarController().SendSocietySMS(Message, "7568622727", "HIN", templateID);
                            }
                            catch (Exception ex)
                            {
                                DBLogger objDBLogger = new DBLogger();
                                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "GopalCreditCardController", FunctionName = "Message SendSms(int statusId, string MobileNo = \"\", string RegistrationNO = \"\", int gccId = 0) " }, ex);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "GopalCreditCardController", FunctionName = "SendSms(int statusId, string MobileNo = \"\", string RegistrationNO = \"\", int gccId = 0) " }, ex);
            }

        }


        [HttpGet]
        [ActionName("CertificateGenerate_old")]
        public string CertificateGenerate_old(GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel = null)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {
                    int TotalInstallment = 12;
                    decimal LateInterestRate = 2;
                    decimal InterestRate = 10.25m;
                    decimal TotalInterestRate = LateInterestRate + InterestRate;


                    StringBuilder strbld = new StringBuilder();
                    var file = System.IO.Path.Combine(Server.MapPath("~/Template"), "Gcc-sanction-letter-format.html");


                    strbld.Append(System.IO.File.ReadAllText(file));
                    strbld.Replace("#Bank#", _Model.LoanBankName_HI)
                   .Replace("#Branch#", _Model.LoanBranchName_HI)
                   .Replace("#UserName#", _LoggedInUser.OfficerNameEn);

                    if (_Model.DCSName != null && ContainsUnicodeCharacter(_Model.PACSName_ENG))
                    {
                        strbld.Replace("#SocietyName#", _Model.DCSName);
                    }


                    strbld.Replace("#SocietyRegistrationNo#", _Model.RegistrationNo)
                        .Replace("#SanctionDate#", DateTime.Now.ToString("dd-MM-yyyy"))
                        .Replace("#LoaneeName#", UnicodeToKrutidev.GetKrutidev(_Model.Name_HI))
                        .Replace("#LoaneeFatherName#", UnicodeToKrutidev.GetKrutidev(_Model.FatherName_HI))
                        .Replace("#LoaneeAddress#", UnicodeToKrutidev.GetKrutidev(_Model.Address_HI))
                        .Replace("#SanctionAmount#", _Model.SanctionAmt.ToString())
                        .Replace("#TotalInstallment#", TotalInstallment.ToString())
                        .Replace("#LateInterestRate#", LateInterestRate.ToString())
                        .Replace("#InterestRate#", InterestRate.ToString())
                        .Replace("#TotalInterestRate#", TotalInterestRate.ToString());



                    //strbld.Replace("#SocietyRegistrationNo#", _Model.DCSRegistrationNo)
                    //    .Replace("#SanctionDate#", DateTime.Now.ToString("dd-MM-yyyy"))
                    //    .Replace("#LoaneeName#", _Model.Name_ENG != null && _Model.Name_ENG != "" ? _Model.Name_ENG : _Model.Name_HI)
                    //    .Replace("#LoaneeFatherName#", _Model.FatherName_ENG != null && _Model.FatherName_ENG != "" ? _Model.FatherName_ENG : _Model.FatherName_HI)
                    //    .Replace("#LoaneeAddress#", _Model.Address_ENG != null && _Model.Address_ENG != "" ? _Model.Address_ENG : _Model.Address_HI)
                    //    .Replace("#SanctionAmount#", _Model.SanctionAmt.ToString())
                    //    .Replace("#TotalInstallment#", TotalInstallment.ToString())
                    //    .Replace("#LateInterestRate#", LateInterestRate.ToString())
                    //    .Replace("#InterestRate#", InterestRate.ToString())
                    //    .Replace("#TotalInterestRate#", TotalInterestRate.ToString());



                    var gccRegNo = Server.MapPath("~/Content/GopalCreditCard/GSSProfiling/" + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", ""));

                    string devFontSize = "17px";

                    if (!System.IO.Directory.Exists(gccRegNo))
                    {
                        System.IO.Directory.CreateDirectory(gccRegNo);
                    }

                    var fileName = "SanctionLetter_" + Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace("_", "").ToUpper() + ".pdf";
                    var filePath = gccRegNo + "/" + fileName;

                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(strbld.ToString().Replace("<br>", "<br/>"), true, devFontSize));
                    sb1.Replace("#SocietyName#", _Model.PACSName_ENG);
                    string QRCoder = COMMON_FUNCTION.GenerateQRCode("https://rajsahakarapp.rajasthan.gov.in/GopalCreditCard/GSSProfiling/" + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", "") + "/" + fileName);
                    sb1.Replace("#QRCoder#", QRCoder);

                    GopalCreditCardDBModel _Modal = new GopalCreditCardDBModel();

                    if (!COMMON_FUNCTION.GeneratePDF(sb1, filePath))
                    {
                        throw new Exception("Error in Generate Certificate");
                    }
                    else
                    {
                        GccDocumentsModel _Entity = new GccDocumentsModel { DocId = 3, DocumentURL = fileName, GccId = _AppFlowModel.GccId };
                        var ReturnRegiNo = _Modal.InsertGCCReportFile(_Entity);
                    }



                    DataSet ds = _Modal.GetGccDoc(_Model.gcc_id);

                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    var prefixPath = "GSSProfiling/" + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", "") + "/";
                    //var pdfPath = "GSSProfiling/" + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", "") + "/" + fileName;
                    var pdfPath = fileName;
                    pdfPath += "#" + _AppFlowModel.DocumentURL;// "";


                    var targetDir = Server.MapPath("~/Content/GopalCreditCard/GssProfiling/" + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", ""));
                    var ChangedDocName = "";

                    //DataSet otherDoc = _Modal.GetGccOtherDoc(_Model.gcc_id);
                    //DataTable dt_otherDoc = new DataTable();
                    //dt_otherDoc = otherDoc.Tables[0];


                    DataRow newRow = dt.NewRow();
                    newRow["DocumentUrl"] = _AppFlowModel.DocumentURL;
                    dt.Rows.Add(newRow);

                    dt = dt.AsEnumerable().Where(r => r.Field<string>("DocumentUrl") != "").CopyToDataTable();

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["DocumentUrl"].ToString() != "")
                            {
                                ChangedDocName = "";
                                // pdfPath += "#" + dt.Rows[i]["DocumentUrl"].ToString();

                                if (!Directory.Exists(targetDir))
                                {
                                    Directory.CreateDirectory(targetDir);
                                }

                                //Start file copy in esign directory
                                string documentUrl = dt.Rows[i]["DocumentUrl"].ToString();
                                string documentPath = Server.MapPath("~/Content/GopalCreditCard/" + dt.Rows[i]["DocumentUrl"].ToString());
                                if (!string.IsNullOrEmpty(documentUrl) && System.IO.File.Exists(documentPath))
                                {
                                    // Copy the file to eSignDirectory
                                    string destinationPath = Path.Combine(targetDir, Path.GetFileName(documentPath));
                                    System.IO.File.Copy(documentPath, destinationPath, true); // Overwrite if exists

                                    // Check the file extension and convert to PDF if needed
                                    string fileExtension = Path.GetExtension(documentUrl).ToLower();
                                    if (fileExtension == ".jpg" || fileExtension == ".png" || fileExtension == ".jpeg")
                                    {
                                        ChangedDocName = GopalCreditCardDBModel.ConvertImageToPdf(destinationPath);

                                    }
                                    else if (fileExtension == ".docx")
                                    {
                                        ChangedDocName = GopalCreditCardDBModel.ConvertTextToPdf(destinationPath);
                                    }
                                    else if (fileExtension == ".pdf")
                                    {
                                        pdfPath += "#" + documentUrl;
                                    }
                                    if (ChangedDocName != "")
                                    {

                                        pdfPath += "#" + Path.ChangeExtension(documentUrl, ".pdf");
                                    }

                                }
                                //End file copy in esign directory

                            }


                        }

                    }




                    else
                    {
                        return "Fail,-2";
                    }
                    return pdfPath;
                }
                else
                {
                    return "Fail";
                    // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "hideloading();popupSessionExpier();", true);
                }

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "GopalCreditCardController", FunctionName = "CertificateGenerate catch " }, ex);

                return "Fail";
            }
        }

        [HttpGet]
        [ActionName("AllDocumentEsign_old")]
        public string AllDocumentEsign_old(GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel = null)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {

                    GopalCreditCardDBModel _Modal = new GopalCreditCardDBModel();
                    DataSet ds = _Modal.GetGccDoc(_Model.gcc_id);

                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    var pdfPath = "";

                    var targetDir = Server.MapPath("~/Content/GopalCreditCard/GssProfiling/" + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", ""));
                    var ChangedDocName = "";

                    if (_AppFlowModel.DocumentURL != null && _AppFlowModel.DocumentURL != "")
                    {
                        DataRow newRow = dt.NewRow();
                        pdfPath += _AppFlowModel.DocumentURL;
                        newRow["DocumentUrl"] = _AppFlowModel.DocumentURL;
                        dt.Rows.Add(newRow);
                    }


                    dt = dt.AsEnumerable().Where(r => r.Field<string>("DocumentUrl") != "").CopyToDataTable();

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["DocumentUrl"].ToString() != "")
                            {
                                ChangedDocName = "";
                                // pdfPath += "#" + dt.Rows[i]["DocumentUrl"].ToString();

                                if (!Directory.Exists(targetDir))
                                {
                                    Directory.CreateDirectory(targetDir);
                                }

                                //Start file copy in esign directory
                                string documentUrl = dt.Rows[i]["DocumentUrl"].ToString();
                                string documentPath = Server.MapPath("~/Content/GopalCreditCard/" + dt.Rows[i]["DocumentUrl"].ToString());
                                if (!string.IsNullOrEmpty(documentUrl) && System.IO.File.Exists(documentPath))
                                {
                                    // Copy the file to eSignDirectory
                                    string destinationPath = Path.Combine(targetDir, Path.GetFileName(documentPath));
                                    System.IO.File.Copy(documentPath, destinationPath, true); // Overwrite if exists

                                    // Check the file extension and convert to PDF if needed
                                    string fileExtension = Path.GetExtension(documentUrl).ToLower();
                                    if (fileExtension == ".jpg" || fileExtension == ".png" || fileExtension == ".jpeg")
                                    {
                                        ChangedDocName = GopalCreditCardDBModel.ConvertImageToPdf(destinationPath);

                                    }
                                    else if (fileExtension == ".docx")
                                    {
                                        ChangedDocName = GopalCreditCardDBModel.ConvertTextToPdf(destinationPath);
                                    }
                                    else if (fileExtension == ".pdf")
                                    {
                                        pdfPath += "#" + documentUrl;
                                    }
                                    if (ChangedDocName != "")
                                    {

                                        pdfPath += "#" + Path.ChangeExtension(documentUrl, ".pdf");
                                    }
                                }
                                //End file copy in esign directory
                            }
                        }
                    }
                    else
                    {
                        return "Fail,-2";
                    }

                    return pdfPath.TrimStart('#');
                }
                else
                {
                    return "Fail";
                    // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "hideloading();popupSessionExpier();", true);
                }

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "GopalCreditCardController", FunctionName = "CertificateGenerate catch " }, ex);

                return "Fail";
            }
        }




        [HttpGet]
        [ActionName("AllDocumentEsign")]
        public string AllDocumentEsign(GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel = null)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {

                    GopalCreditCardDBModel _Modal = new GopalCreditCardDBModel();
                    DataSet ds = _Modal.GetGccDoc(_Model.gcc_id);

                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    var pdfPath = "";

                    var targetDir = Server.MapPath("~/Content/GopalCreditCard/GssProfiling/" + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", ""));
                    var ChangedDocName = "";

                    if (_AppFlowModel.DocumentURL != null && _AppFlowModel.DocumentURL != "")
                    {
                        DataRow newRow = dt.NewRow();
                        pdfPath += _AppFlowModel.DocumentURL;
                        newRow["DocumentUrl"] = _AppFlowModel.DocumentURL;
                        dt.Rows.Add(newRow);
                    }
                    if (dt.AsEnumerable().Where(r => r.Field<string>("DocumentUrl") != "").Count() > 0)
                    {
                        dt = dt.AsEnumerable().Where(r => r.Field<string>("DocumentUrl") != "").CopyToDataTable();

                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Rows[i]["DocumentUrl"].ToString() != "")
                                {
                                    ChangedDocName = "";
                                    // pdfPath += "#" + dt.Rows[i]["DocumentUrl"].ToString();

                                    if (!Directory.Exists(targetDir))
                                    {
                                        Directory.CreateDirectory(targetDir);
                                    }

                                    //Start file copy in esign directory
                                    string documentUrl = dt.Rows[i]["DocumentUrl"].ToString();
                                    string documentPath = Server.MapPath("~/Content/GopalCreditCard/" + dt.Rows[i]["DocumentUrl"].ToString());
                                    if (!string.IsNullOrEmpty(documentUrl) && System.IO.File.Exists(documentPath))
                                    {
                                        // Copy the file to eSignDirectory
                                        string destinationPath = Path.Combine(targetDir, Path.GetFileName(documentPath));
                                        System.IO.File.Copy(documentPath, destinationPath, true); // Overwrite if exists  //24Feb2025
                                                                                                  //System.IO.File.Move(documentPath, destinationPath); // Overwrite existing files if necessary

                                        // Check the file extension and convert to PDF if needed
                                        string fileExtension = Path.GetExtension(documentUrl).ToLower();
                                        if (fileExtension == ".jpg" || fileExtension == ".png" || fileExtension == ".jpeg")
                                        {
                                            try
                                            {
                                                ChangedDocName = GopalCreditCardDBModel.ConvertImageToPdf(destinationPath);
                                            }
                                            catch (Exception)
                                            {
                                                return "";
                                            }

                                        }
                                        else if (fileExtension == ".docx")
                                        {
                                            ChangedDocName = GopalCreditCardDBModel.ConvertTextToPdf(destinationPath);
                                        }
                                        else if (fileExtension == ".pdf")
                                        {
                                            pdfPath += "#" + documentUrl;
                                        }
                                        if (ChangedDocName != "")
                                        {

                                            pdfPath += "#" + Path.ChangeExtension(documentUrl, ".pdf");
                                        }


                                    }
                                    //End file copy in esign directory
                                }
                            }
                        }
                        else
                        {
                            return "Fail,-2";
                        }

                        pdfPath = pdfPath.TrimStart('#');

                        //var filteredRows = dt.AsEnumerable().Where(r => !string.IsNullOrEmpty(r.Field<string>("DocumentUrl"))).ToList();

                        // Extract paths for G1Photo, G2Photo, G1Signature, and G2Signature  
                        //string g1PhotoPath = filteredRows.FirstOrDefault(r => r.Field<string>("DocumentUrl").StartsWith("G1Photo"))?.Field<string>("DocumentUrl");
                        //string g2PhotoPath = filteredRows.FirstOrDefault(r => r.Field<string>("DocumentUrl").StartsWith("G2Photo"))?.Field<string>("DocumentUrl");
                        //string g1SignaturePath = filteredRows.FirstOrDefault(r => r.Field<string>("DocumentUrl").StartsWith("G1Signature"))?.Field<string>("DocumentUrl");
                        //string g2SignaturePath = filteredRows.FirstOrDefault(r => r.Field<string>("DocumentUrl").StartsWith("G2Signature"))?.Field<string>("DocumentUrl");

                        //string mergedPdf = Path.Combine(targetDir, "GuarantorMerge.pdf");
                        //string[] pathArray = pdfPath.Split('#');
                        //string[] updatedPaths;

                        //string[] filterKeywords = { "G1Photo", "G2Photo" };
                        //string[] filteredPaths = pathArray.Where(path => filterKeywords.Any(keyword => path.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)).Select(path => Path.Combine(targetDir, path)).ToArray();

                        //if (g1PhotoPath != null && g2PhotoPath != null /*&& g1SignaturePath != null && g2SignaturePath != null*/)
                        //{

                        //    g1PhotoPath = Server.MapPath("~/Content/GopalCreditCard/" + g1PhotoPath);
                        //    g2PhotoPath = Server.MapPath("~/Content/GopalCreditCard/" + g2PhotoPath);
                        //    //g1SignaturePath = Server.MapPath("~/Content/GopalCreditCard/" + g1SignaturePath);
                        //    // g2SignaturePath = Server.MapPath("~/Content/GopalCreditCard/" + g2SignaturePath);

                        //    GopalCreditCardDBModel.MergePDF(mergedPdf, g1PhotoPath + "#" + g2PhotoPath /*+ "#" + g1SignaturePath +"#" + g2SignaturePath*/);
                        //    updatedPaths = pathArray.Select(path => filterKeywords.Any(keyword => path.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ? "" : path).ToArray();
                        //    pdfPath = string.Join("#", updatedPaths) + "#GuarantorMerge.pdf";

                        //}

                        return pdfPath.TrimStart('#').Replace("##", "");
                    }
                    else
                        return "";
                }
                else
                {
                    return "Fail";
                    // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "hideloading();popupSessionExpier();", true);
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "GopalCreditCardController", FunctionName = "CertificateGenerate catch " }, ex);

                return "Fail";
            }
        }


        [HttpGet]
        [ActionName("CertificateGenerate")]
        public string CertificateGenerate(GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel = null)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {
                    int TotalInstallment = 12;
                    decimal LateInterestRate = 2;
                    decimal InterestRate = 10.25m;
                    decimal TotalInterestRate = LateInterestRate + InterestRate;


                    StringBuilder strbld = new StringBuilder();
                    var file = System.IO.Path.Combine(Server.MapPath("~/Template"), "Gcc_sanction_letter_format.html");


                    strbld.Append(System.IO.File.ReadAllText(file));
                    strbld.Replace("#Bank#", _Model.LoanBankName_HI)
                   .Replace("#Branch#", _Model.LoanBranchName_HI)
                   .Replace("#UserName#", _LoggedInUser.OfficerNameEn);

                    if (_Model.DCSName != null && ContainsUnicodeCharacter(_Model.PACSName_ENG))
                    {
                        strbld.Replace("#SocietyName#", _Model.DCSName);
                    }


                    strbld.Replace("#SocietyRegistrationNo#", _Model.RegistrationNo)
                        .Replace("#SanctionDate#", DateTime.Now.ToString("dd-MM-yyyy"))
                        .Replace("#LoaneeName#", UnicodeToKrutidev.GetKrutidev(_Model.Name_HI))
                        .Replace("#LoaneeFatherName#", UnicodeToKrutidev.GetKrutidev(_Model.FatherName_HI))
                        .Replace("#LoaneeAddress#", UnicodeToKrutidev.GetKrutidev(_Model.Address_HI))
                        .Replace("#SanctionAmount#", _Model.SanctionAmt.ToString())
                        .Replace("#TotalInstallment#", TotalInstallment.ToString())
                        .Replace("#LateInterestRate#", LateInterestRate.ToString())
                        .Replace("#InterestRate#", InterestRate.ToString())
                        .Replace("#TotalInterestRate#", TotalInterestRate.ToString());


                    var gccRegNo = Server.MapPath("~/Content/GopalCreditCard/GSSProfiling/" + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", ""));

                    string devFontSize = "17px";

                    if (!System.IO.Directory.Exists(gccRegNo))
                    {
                        System.IO.Directory.CreateDirectory(gccRegNo);
                    }

                    var fileName = "SanctionLetter_" + Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace("_", "").ToUpper() + ".pdf";
                    var filePath = gccRegNo + "/" + fileName;

                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(strbld.ToString().Replace("<br>", "<br/>"), true, devFontSize));
                    sb1.Replace("#SocietyName#", _Model.PACSName_ENG);
                    string QRCoder = COMMON_FUNCTION.GenerateQRCode("https://rajsahakarapp.rajasthan.gov.in/GopalCreditCard/GSSProfiling/" + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", "") + "/" + fileName);
                    sb1.Replace("#QRCoder#", QRCoder);

                    GopalCreditCardDBModel _Modal = new GopalCreditCardDBModel();


                    //if (Request.Url.Host.Contains("localhost") && !COMMON_FUNCTION.GeneratePDF1(sb1, filePath)) 
                    //{
                    //    throw new Exception("Error in Generate Certificate");
                    //}
                    //else 
                    if (!COMMON_FUNCTION.GeneratePDF(sb1, filePath))
                    {
                        throw new Exception("Error in Generate Certificate");
                    }
                    else
                    {
                        GccDocumentsModel _Entity = new GccDocumentsModel { DocId = 3, DocumentURL = fileName, GccId = _AppFlowModel.GccId };
                        var ReturnRegiNo = _Modal.InsertGCCReportFile(_Entity);
                    }

                    var pdfPath = fileName;


                    if (_AppFlowModel.DocumentURL != null && _AppFlowModel.DocumentURL != "")
                    {
                        pdfPath += "#" + _AppFlowModel.DocumentURL;
                    }


                    return pdfPath;
                }
                else
                {
                    return "Fail";
                    // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "hideloading();popupSessionExpier();", true);
                }

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "GopalCreditCardController", FunctionName = "CertificateGenerate catch " }, ex);

                return "Fail";
            }
        }


        //changes by jaya
        [HttpGet]
        [ActionName("ReSanctionCertificateGenerate")]
        public string ReSanctionCertificateGenerate(GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel = null)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {
                    int TotalInstallment = 12;
                    decimal LateInterestRate = 2;
                    decimal InterestRate = 10.25m;
                    decimal TotalInterestRate = LateInterestRate + InterestRate;


                    StringBuilder strbld = new StringBuilder();
                    var file = System.IO.Path.Combine(Server.MapPath("~/Template"), "Gcc_resanction_letter_format.html");


                    strbld.Append(System.IO.File.ReadAllText(file));
                    strbld.Replace("#Bank#", _Model.LoanBankName_HI)
                   .Replace("#Branch#", _Model.LoanBranchName_HI)
                   .Replace("#UserName#", _LoggedInUser.OfficerNameEn);

                    if (_Model.DCSName != null && ContainsUnicodeCharacter(_Model.PACSName_ENG))
                    {
                        strbld.Replace("#SocietyName#", _Model.DCSName);
                    }

                    strbld.Replace("#SocietyRegistrationNo#", _Model.RegistrationNo)
                        .Replace("#SanctionDate#", DateTime.Now.ToString("dd-MM-yyyy"))
                        .Replace("#LoaneeName#", UnicodeToKrutidev.GetKrutidev(_Model.Name_HI))
                        .Replace("#LoaneeFatherName#", UnicodeToKrutidev.GetKrutidev(_Model.FatherName_HI))
                        .Replace("#LoaneeAddress#", UnicodeToKrutidev.GetKrutidev(_Model.Address_HI))
                        .Replace("#SanctionAmount#", _Model.SanctionAmt.ToString())
                        .Replace("#TotalInstallment#", TotalInstallment.ToString())
                        .Replace("#LateInterestRate#", LateInterestRate.ToString())
                        .Replace("#InterestRate#", InterestRate.ToString())
                        .Replace("#TotalInterestRate#", TotalInterestRate.ToString());


                    var gccRegNo = Server.MapPath("~/Content/GopalCreditCard/GSSProfiling/" + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", ""));

                    string devFontSize = "17px";

                    if (!System.IO.Directory.Exists(gccRegNo))
                    {
                        System.IO.Directory.CreateDirectory(gccRegNo);
                    }

                    var fileName = "ReSanctionLetter_" + Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace("_", "").ToUpper() + ".pdf";
                    var filePath = gccRegNo + "/" + fileName;

                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(strbld.ToString().Replace("<br>", "<br/>"), true, devFontSize));
                    sb1.Replace("#SocietyName#", _Model.PACSName_ENG);
                    string QRCoder = COMMON_FUNCTION.GenerateQRCode("https://rajsahakarapp.rajasthan.gov.in/GopalCreditCard/GSSProfiling/" + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", "") + "/" + fileName);
                    sb1.Replace("#QRCoder#", QRCoder);

                    GopalCreditCardDBModel _Modal = new GopalCreditCardDBModel();

                    if (!COMMON_FUNCTION.GeneratePDF(sb1, filePath))
                    {
                        throw new Exception("Error in Generate Certificate");
                    }
                    else
                    {
                        GccDocumentsModel _Entity = new GccDocumentsModel { DocId = 3, DocumentURL = fileName, GccId = _AppFlowModel.GccId };
                        var ReturnRegiNo = _Modal.InsertGCCReportFile(_Entity);
                    }

                    var pdfPath = fileName;


                    if (_AppFlowModel.DocumentURL != null && _AppFlowModel.DocumentURL != "")
                    {
                        pdfPath += "#" + _AppFlowModel.DocumentURL;
                    }


                    return pdfPath;
                }
                else
                {
                    return "Fail";

                }

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "GopalCreditCardController", FunctionName = "CertificateGenerate catch " }, ex);

                return "Fail";
            }
        }


        [HttpGet]
        [ActionName("NOCDocumnetGenerate")]
        public string NOCDocumnetGenerate(GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel = null)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {
                    int TotalInstallment = 12;
                    decimal LateInterestRate = 2;
                    decimal InterestRate = 10.25m;
                    decimal TotalInterestRate = LateInterestRate + InterestRate;


                    var con = GetGopalCreditById(_AppFlowModel.GccId);

                    var conDataList = JsonConvert.DeserializeObject<List<GopalCreditCardFormModel>>(con);

                    if (conDataList == null || !conDataList.Any())
                        throw new Exception("No data found for GccId");

                    var conData = conDataList.FirstOrDefault();
                    Console.WriteLine("Condata", conData);

                    StringBuilder strbld = new StringBuilder();
                    var file = System.IO.Path.Combine(Server.MapPath("~/Template"), "Gcc_Noc_letter_format.html");
                    //var file = System.IO.Path.Combine(Server.MapPath("~/Template"), "ots_certificate.html");

                    strbld.Append(System.IO.File.ReadAllText(file));
                    strbld.Replace("#Bank#", _Model.LoanBankName_ENG)
                   .Replace("#Branch#", _Model.LoanBranchName_ENG)
                   .Replace("#PacsName#", _Model.PACSName_ENG)
                   .Replace("#UserName#", _LoggedInUser.OfficerNameEn);

                    if (_Model.DCSName != null && ContainsUnicodeCharacter(_Model.PACSName_ENG))
                    {
                        strbld.Replace("#SocietyName#", _Model.DCSName);
                    }

                    strbld.Replace("#SocietyRegistrationNo#", conData.RegistrationNo)
                   .Replace("#NocDate#", DateTime.Now.ToString("dd-MM-yyyy"))
                   .Replace("#LoaneeName#", UnicodeToKrutidev.GetKrutidev(conData.Name_ENG))
                   .Replace("#LoaneeFatherName#", UnicodeToKrutidev.GetKrutidev(conData.FatherName_ENG))
                   .Replace("#LoaneeAddress#", UnicodeToKrutidev.GetKrutidev(_Model.Address_ENG))
                   .Replace("#SanctionAmount#", _Model.SanctionAmt.ToString())
                   .Replace("#DisbursedDate#", _Model.DisbursedDate?.ToString("dd/MM/yyyy"))
                   .Replace("#PaymentDate#", _Model.PaymentDate?.ToString("dd/MM/yyyy"))
                   .Replace("#TotalInstallment#", TotalInstallment.ToString())
                   .Replace("#LateInterestRate#", LateInterestRate.ToString())
                   .Replace("#InterestRate#", InterestRate.ToString())
                   .Replace("#TotalInterestRate#", TotalInterestRate.ToString());

                    //strbld.Replace("#SocietyRegistrationNo#", _Model.RegistrationNo)
                    //    .Replace("#NocDate#", DateTime.Now.ToString("dd-MM-yyyy"))
                    //    .Replace("#LoaneeName#", UnicodeToKrutidev.GetKrutidev(_Model.Name_HI))
                    //    .Replace("#LoaneeFatherName#", UnicodeToKrutidev.GetKrutidev(_Model.FatherName_HI))
                    //    .Replace("#LoaneeAddress#", UnicodeToKrutidev.GetKrutidev(_Model.Address_HI))
                    //    .Replace("#SanctionAmount#", _Model.SanctionAmt.ToString())
                    //    .Replace("#TotalInstallment#", TotalInstallment.ToString())
                    //    .Replace("#LateInterestRate#", LateInterestRate.ToString())
                    //    .Replace("#InterestRate#", InterestRate.ToString())
                    //    .Replace("#TotalInterestRate#", TotalInterestRate.ToString());


                    var gccRegNo = Server.MapPath("~/Content/GopalCreditCard/GSSProfiling/" + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", ""));

                    string devFontSize = "17px";

                    if (!System.IO.Directory.Exists(gccRegNo))
                    {
                        System.IO.Directory.CreateDirectory(gccRegNo);
                    }

                    var fileName = "NOCGenerateLetter_" + Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace("_", "").ToUpper() + ".pdf";
                    var filePath = gccRegNo + "/" + fileName;

                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(strbld.ToString());
                    //  sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(strbld.ToString().Replace("<br>", "<br/>"), true, devFontSize));
                    sb1.Replace("#SocietyName#", _Model.PACSName_ENG);
                    string QRCoder = COMMON_FUNCTION.GenerateQRCode("https://rajsahakarapp.rajasthan.gov.in/GopalCreditCard/GSSProfiling/" + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", "") + "/" + fileName);
                    sb1.Replace("#QRCoder#", QRCoder);

                    GopalCreditCardDBModel _Modal = new GopalCreditCardDBModel();

                    CommonPdfGenerate(sb1, filePath);


                    //if (Request.Url.Host.Contains("localhost") && !COMMON_FUNCTION.GeneratePDF1(sb1, filePath))
                    //{
                    //    throw new Exception("Error in Generate Certificate");
                    //}
                    //else if (Request.Url.Host.Contains("localhost") && !COMMON_FUNCTION.GeneratePDF(sb1, filePath))
                    //{
                    //    throw new Exception("Error in Generate Certificate");
                    //}
                    //else
                    //{
                    GccDocumentsModel _Entity = new GccDocumentsModel { DocId = 7, DocumentURL = fileName, GccId = _AppFlowModel.GccId };
                    var ReturnRegiNo = _Modal.InsertGCCReportFile(_Entity);
                    //}

                    var pdfPath = fileName;


                    if (_AppFlowModel.DocumentURL != null && _AppFlowModel.DocumentURL != "")
                    {
                        pdfPath += "#" + _AppFlowModel.DocumentURL;
                    }


                    return pdfPath;
                }
                else
                {
                    return "Fail";
                    // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "hideloading();popupSessionExpier();", true);
                }

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "GopalCreditCardController", FunctionName = "NOCDocumnetGenerate catch " }, ex);

                return "Fail";
            }
        }



        [HttpGet]
        [ActionName("GenerateMokaReport")]
        public string GenerateMokaReport(GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel = null)
        {
            try
            {
                string wkhtmlPath = Server.MapPath("~/bin/wkhtmltopdf.exe");
                string fontPath = new Uri(Server.MapPath("~/fonts/lohit-Devanagari.ttf")).AbsoluteUri;
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {
                    StringBuilder strbld = new StringBuilder();
                    var file = System.IO.Path.Combine(Server.MapPath("~/Template"), "Moka_report.html");


                    strbld.Append(System.IO.File.ReadAllText(file));
                    strbld.Replace("#RegistrationNo#", _Model.RegistrationNo)
                    .Replace("#PACSName_ENG#", _Model.PACSName_ENG)
                    .Replace("#Date#", DateTime.Now.ToString("dd-MM-yyyy"))
                    .Replace("#Name#", _Model.Name_ENG)
                    .Replace("#FatherName#", _Model.FatherName_ENG)
                    .Replace("#Address#", $"{_Model.HouseBuildingNo}, {_Model.LaneStreetName}, {_Model.Address_ENG}, {_Model.PinCode}, {_Model.Village_ENG}, {_Model.DistrictName_ENG}")
                    .Replace("#ApplicantRuralResidence#", _Model.IsApplicantRuralResidence == true ? "हां" : "नहीं")
                    .Replace("#DCSName#", _Model.DCSName)
                    .Replace("#LoanAmountRecommendedByDCS#", NumberToHindiWordConverter.FormatDecimalAmount(Convert.ToDecimal(_Model.LoanAmountRecommendedByDCS)))
                    .Replace("#NoOfbuffaloesOnMoka#", _Model.NoOfbuffaloesOnMoka?.ToString() ?? "0")
                    .Replace("#NoOfCowsOnMoka#", _Model.NoOfCowsOnMoka?.ToString() ?? "0")
                    .Replace("#LandValueOnMoka#", _Model.LandValueOnMoka?.ToString() ?? "N/A")
                    .Replace("#Guarantor1Name#", _Model.GuarantorDetailsList[0].Name)
                    .Replace("#Guarantor1FatherName#", _Model.GuarantorDetailsList[0].FatherSpouseName)
                    .Replace("#Guarantor1Address#", _Model.GuarantorDetailsList[0].Address)
                    .Replace("#Guarantor1LandProperty#", (_Model.GuarantorDetailsList[0].GuarranterLandProperty == "0" ? "N/A" : _Model.GuarantorDetailsList[0].GuarranterLandProperty))
                    .Replace("#Guarantor2Name#", _Model.GuarantorDetailsList[1].Name)
                    .Replace("#Guarantor2FatherName#", _Model.GuarantorDetailsList[1].FatherSpouseName)
                    .Replace("#Guarantor2Address#", _Model.GuarantorDetailsList[1].Address)
                    .Replace("#Guarantor2LandProperty#", (_Model.GuarantorDetailsList[1].GuarranterLandProperty == "0" ? "N/A" : _Model.GuarantorDetailsList[1].GuarranterLandProperty))
                    .Replace("#AgricultureLandIrrigated#", (_Model.AgricultureLandDetailList?.Count ?? 0) > 0 ? _Model.AgricultureLandDetailList[0].Irrigated.ToString() : "N/A")
                    .Replace("#AgricultureLandNonIrrigated#", (_Model.AgricultureLandDetailList?.Count ?? 0) > 0 ? _Model.AgricultureLandDetailList[0].NonIrrigated.ToString() : "N/A")
                    .Replace("#MokaRemark#", _Model.RemarkMoka);

                    string finalHtml = $@"
                   <html>
                    <head>
                        <meta charset='UTF-8'>
                        <style>
                            @font-face {{
                                font-family: 'lohit-Devanagari';
                                src: url('{fontPath}') format('truetype');
                            }}
                            body {{
                                font-family: 'lohit-Devanagari';
                                margin: 20px;
                            }}
                        </style>
                    </head>
                    <body>
                        {strbld}
                    </body>
                   </html>";


                    var pdf = PDFgenerator.GeneratePdfFromHtml(finalHtml, wkhtmlPath);
                    var fileName = $"MokaRpt{Guid.NewGuid().ToString("N").ToUpper()}.pdf";

                    string cleanRegNo = _Model.RegistrationNo.Replace("/", "").Replace(".", "").Replace("_", "").Replace("-", "").Replace(" ", "");

                    string folderPath = Path.Combine(Server.MapPath("~/Content/GopalCreditCard/GSSProfiling/"), cleanRegNo);

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    string filePath = Path.Combine(folderPath, fileName);
                    System.IO.File.WriteAllBytes(filePath, pdf);

                    var pdfPath = fileName;
                    if (_AppFlowModel.DocumentURL != null && _AppFlowModel.DocumentURL != "")
                    {
                        pdfPath += "#" + _AppFlowModel.DocumentURL;
                    }
                    return pdfPath;
                }
                else
                {
                    return "Fail";
                }

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "GopalCreditCardController", FunctionName = "GenerateMokaReport catch " }, ex);

                return "Fail";
            }
        }


        public string CommonPdfGenerate(System.Text.StringBuilder sb, string filePath)
        {
            //StringBuilder str = sb.ToString();

            string wkhtmlPath = HostingEnvironment.MapPath("~/bin/wkhtmltopdf.exe");
            string fontPath = new Uri(HostingEnvironment.MapPath("~/fonts/lohit-Devanagari.ttf")).AbsoluteUri;
            //string iycLogo = new Uri(Server.MapPath("~/Content/images/iyc_logo.png")).AbsoluteUri;
            //string cmPhoto = new Uri(Server.MapPath("~/Content/images/cm-ots-logo.png")).AbsoluteUri;

            try
            {
                string finalHtml = $@"
                   <html>
                    <head>
                        <meta charset='UTF-8'>
                        <style>
                            @font-face {{
                                font-family: 'lohit-Devanagari';
                                src: url('{fontPath}') format('truetype');
                            }}
                            body {{
                                font-family: 'lohit-Devanagari';
                                margin: 20px;
                            }}
                        </style>
                    </head>
                    <body>
                        {sb}
                    </body>
                   </html>";

                var pdf = PDFgenerator.GeneratePdfFromHtml(finalHtml, wkhtmlPath);
                var fileName = $"NOCGenerateLetter_{Guid.NewGuid().ToString("N").ToUpper()}.pdf";


                string folderPath = Path.Combine(HostingEnvironment.MapPath("~/Content/GopalCreditCard/GSSProfiling/"));

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }


                System.IO.File.WriteAllBytes(filePath, pdf);
                return filePath;
            }
            catch (Exception)
            {

                throw;
            }

        }

        [CustomAuthentication]
        [HttpPost]
        public string GetGCCDashboardData()
        {
            GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());
            DataSet DT = objGopalCreditCardDBModel.GetGCCDashboardData();
            return JsonConvert.SerializeObject(DT, Newtonsoft.Json.Formatting.Indented);
        }


        [HttpPost]
        public string GetCurrentUserCCBDetail()
        {
            GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objGopalCreditCardDBModel.GetCurrentUserCCBDetail());
        }


        [HttpGet]
        [ActionName("GetCurrentRoleId")]
        public string GetCurrentRoleId()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            return _LoggedInUser?.RoleId.ToString();
        }

        [HttpPost]
        [ActionName("SaveGCCVerifyReportFile")]
        public string SaveGCCVerifyReportFile(GopalCreditCardModel.GccDocumentsModel _Entity)
        {
            int ReturnRegiNo = 0;
            string fileName = string.Empty;
            string ReturnJSON = "";
            string pathString = "~/Content/GopalCreditCard/GSSProfiling/";

            GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());

            foreach (string files in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[files];
                var prefix = "";

                if (_Entity.DocId == 1) { prefix = "MokaRpt"; }
                else if (_Entity.DocId == 2) { prefix = "BranchLevelRpt_"; }
                else if (_Entity.DocId == 5) { prefix = "OriginalDoc_"; }
                else if (_Entity.DocId == 4) { prefix = "OtherDoc_"; }


                fileName = prefix + Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace(" ", "").Replace("_", "").ToUpper() + System.IO.Path.GetExtension(file.FileName);
                if (files.ToString() == "GccReportFile")
                    _Entity.DocumentURL = fileName;

                ReturnRegiNo = objGopalCreditCardDBModel.InsertGCCReportFile(_Entity);


                if (ReturnRegiNo > 0)
                {
                    pathString += Regex.Replace(_Entity.RegistrationNo, "[^a-zA-Z0-9_]+", "");
                    if (!System.IO.File.Exists(pathString))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(pathString));
                    }
                    file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));
                }
                break;
            }
            ReturnJSON = "{\"fileName\":\"" + fileName.ToString() + "\",\"docId\":\"" + ReturnRegiNo + "\"}";

            return ReturnJSON;

        }


        //[HttpPost]
        //[ActionName("EsingDocument")]
        //public string EsingDocument(string TXN, string SocietyIDEnc, string document)
        //{
        //    string succmsg = "";
        //    if (TXN != "")
        //    {
        //        try
        //        {
        //            var result = COMMON_FUNCTION.EsignPDF_NEW(TXN, Server.MapPath("~") + document.Replace("..", ""), _LoggedInUser.DISTRICT_ENG, "REGISTRAR");
        //            succmsg = result.status;
        //        }
        //        catch (Exception ex)
        //        {
        //            succmsg = ex.Message;
        //        }
        //    }
        //    return succmsg;
        //}


        [HttpGet]
        [ActionName("GetGccDoc")]
        public string GetGccDoc(int GccId = 0)
        {
            var gccModel = new GopalCreditCardDBModel(new DBLogger());
            DataSet ds = gccModel.GetGccDoc(GccId);

            if (ds != null)
            {
                return JsonConvert.SerializeObject(ds, Newtonsoft.Json.Formatting.Indented);
            }
            else
                return "";
        }


        [HttpGet]
        [ActionName("GetGccOtherDoc")]
        public string GetGccOtherDoc(int GccId = 0)
        {
            var gccModel = new GopalCreditCardDBModel(new DBLogger());
            DataSet ds = gccModel.GetGccOtherDoc(GccId);

            if (ds != null)
            {
                return JsonConvert.SerializeObject(ds, Newtonsoft.Json.Formatting.Indented);
            }
            else
                return "";
        }



        [HttpPost]
        [ActionName("SaveGccEditableField")]
        public string SaveGccEditableField(GccEditableDocumentslist _Entity = null)
        {
            var gccModel = new GopalCreditCardDBModel(new DBLogger());
            var str = "";
            foreach (var item in _Entity.GccEditableDocuments)
            {
                if (item.FieldId != 0)
                    str += "#" + item.FieldId + "," + item.Status + ":";
            }
            var a = gccModel.SaveGccEditableField(str, _Entity.GccEditableDocuments[0].GccId);
            return "Success";
        }


        #endregion

        #region Pradeep
        [HttpGet]
        [ActionName("GetApplicationDetail")]
        public string GetApplicationDetail(string GccIdEnc = "")
        {
            var gccModel = new GopalCreditCardDBModel(new DBLogger());
            DataSet ds = gccModel.GetApplicationDetail(GccIdEnc);

            if (ds != null)
            {
                return JsonConvert.SerializeObject(ds, Newtonsoft.Json.Formatting.Indented);
            }
            else
                return "";
        }

        [HttpPost]
        [ActionName("Save_GopalCreditCardFormPart2")]
        public JsonResult Save_GopalCreditCardFormPart2(GCC_RequiredPointsModel gCC_RequiredPoints)
        {
            try
            {
                var gccModel = new GopalCreditCardDBModel(new DBLogger());
                var s = gccModel.Save_GopalCreditCardFormPart2(gCC_RequiredPoints);
                if (s == "-1")
                {
                    return Json(new { Status = false, Msg = "Error" }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { Status = true, Msg = "Application saved successfuly." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = "Save_GopalCreditCardFormPart2(GCC_RequiredPointsModel gCC_RequiredPoints)" }, ex);
                return Json(new { Status = false, Msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region madhuri #start gcc emitra kiosk payment#


        [HttpPost]
        public string GetEmitraTransaction(string SearchText, string ServiceType)
        {
            string PayStatus = "";
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            if (_LoggedInUser.UserType == "K")
            {
                DataTable Dt = null;
                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                CitizenController objCitizenModel = new CitizenController();
                int DistrictId = _LoggedInUser.DistrictId;
                Dt = objSocietyModel.GetEmitraTransaction(SearchText, ServiceType);


                if (Dt != null)
                    for (int i = 0; Dt.Rows.Count > i; i++)
                    {
                        PayStatus = UpdateGccTransaction(Dt.Rows[i]["GccId"].ToString(), Dt.Rows[i]["EMitraAppRequestID"].ToString(), ServiceType);
                        if (PayStatus.ToUpper().Contains("SUCCESS"))
                        {
                            i = Dt.Rows.Count;
                        }
                    }
            }
            return PayStatus;
        }

        [HttpPost]
        public string UpdateGccTransaction(string SocietyIdEnc, string EMitraAppRequestID, string SERICEID, string UniqueId = "0")
        {
            DataTable Dt = null;
            string returnval = "";
            VerifyResponse resp = null;
            PGResponse respg = null;
            string encData = "";
            string decryptData = "";
            PGResponse objPGResponse = null;

            string DivHtml = "";

            int i = -1;
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            CitizenModel objCitizenModel = new CitizenModel(new DBLogger());
            SocietyRegistrationEntity objSocietyRegistrationEntity = new SocietyRegistrationEntity();

            if (_LoggedInUser.UserType == "K")
            {
                objSocietyRegistrationEntity.UserType = _LoggedInUser.UserType;
                objSocietyRegistrationEntity.SocietyIDEnc = SocietyIdEnc;
                objSocietyRegistrationEntity.EMitraAppRequestID = EMitraAppRequestID;
                objSocietyRegistrationEntity.ServicesId = int.Parse(SERICEID);
                objSocietyRegistrationEntity.UniqueId = int.Parse(UniqueId);

            }
            //else
            //{
            //    objSocietyRegistrationEntity.SSOID = _LoggedInUser.SSOID;
            //    objSocietyRegistrationEntity.UserType = "C";
            //    objSocietyRegistrationEntity.SocietyIDEnc = SocietyIdEnc;
            //    objSocietyRegistrationEntity.ServicesId = int.Parse(SERICEID);
            //    objSocietyRegistrationEntity.EMitraAppRequestID = EMitraAppRequestID;
            //}

            EmitraModelFactory objEmitra = new EmitraModelFactory(new DBLogger());

            string PayStatus = objEmitra.GetPaymentTransaction(objSocietyRegistrationEntity);


            EmitraModelFactory objEmitraModel = new EmitraModelFactory(new FileLogger());

            encData = PayStatus;

            decryptData = encData;

            objCitizenModel.UpdateOnlinePaymentStatus("", SocietyIdEnc, EMitraAppRequestID, "NO", decryptData, "1");

            //objPGResponse = JsonConvert.DeserializeObject<PGResponse>(DivHtml);
            //if (objPGResponse.STATUS!=null && objPGResponse.STATUS.ToString().Contains("SUCCESS"))
            //{
            //    AuditorModel objEM = new AuditorModel(new FileLogger());
            //    objEM.SetAuditEmpanelledFinalSubmit(EMitraAppRequestID);
            //}
            //else
            //{
            //}
            //}
            return PayStatus;
        }

        #endregion

        #region BulkUpdateStatus(CCB-md)
        [HttpPost]
        [ActionName("BulkUploadApplicationFlowStatus_V1")]
        public JsonResult BulkUploadApplicationFlowStatus_V1(string OTP = "", string TXN = "", GopalCreditCardFormModelList _Model = null, GccApplicationFlowModel _AppFlowModel = null)
        {
            string pdfPath = "";
            string succmsg = string.Empty;

            if (_LoggedInUser.LevelId != 7 && (_AppFlowModel.StatusId == 19 || _AppFlowModel.StatusId == 17)) return Json(0);
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                if (_LoggedInUser == null)
                {
                    return Json("LOGOUT");
                }

                GopalCreditCardDBModel _Modal = new GopalCreditCardDBModel();
                if (!ModelState.IsValid)
                    return Json(JsonModelError.Get(ModelState.Values));

                int result = 0;

                if (TXN != "" || (Request.Url.Host.Contains("localhost") || Request.Url.Host.Contains("devitsandbox")))
                {
                    ResultMessage otpResult = new ResultMessage();


                    if (!Request.Url.Host.Contains("localhost") && !Request.Url.Host.Contains("devitsandbox")) { otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN); }
                    else { otpResult.status = "1"; otpResult.data = TXN; }

                    if (otpResult.status == "1")
                    {
                        foreach (var app in _Model.GopalCreditCardFormModel)
                        {
                            _AppFlowModel.LoanAmountRecommendedByDCS = app.SanctionAmt;
                            _AppFlowModel.GccId = app.gcc_id;
                            if (_AppFlowModel.StatusId == 19)
                                pdfPath = AllDocumentEsign(app, _AppFlowModel);

                            result = _Modal.AddApplicationFlow_V1(_AppFlowModel);
                            SendSms(_AppFlowModel.StatusId, app.MobileNo, app.RegistrationNo, app.gcc_id);

                        }

                    }
                    else { result = -2; }


                }
                else
                {
                    result = 0;
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = "SaveGSSApplicationFlowStatus_V1( string OTP = \"\", string TXN = \"\",  GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel=null)" }, ex);
                return null;
            }

        }

        #endregion

        #region UserAssign
        [HttpPost]
        public JsonResult GetUserForGccAssign(int GccId)
        {

            GopalCreditCardDBModel objApplicationFlowModel = new GopalCreditCardDBModel(new DBLogger());

            DataTable DT = objApplicationFlowModel.GetUserForGccAssign(GccId);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public string GccAssignOfficerSave(int GccId, int UserId)
        {
            string flag = "-1";
            GopalCreditCardDBModel objModel = new GopalCreditCardDBModel(new DBLogger());
            flag = objModel.GccAssignOfficerSave(GccId, UserId);
            return flag;
        }


        [HttpPost]
        public JsonResult GetUserListForGccAssign(int CCB_BankID = 0, int CCB_BranchID = 0, int CCB_PacsID = 0)
        {
            GopalCreditCardDBModel objApplicationFlowModel = new GopalCreditCardDBModel();
            DataTable DT = objApplicationFlowModel.GetUserListForFileTransfer(CCB_BankID, CCB_BranchID, CCB_PacsID);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetUserGccList(int UserId = 0, int MappingID = 0)
        {
            GopalCreditCardDBModel objApplicationFlowModel = new GopalCreditCardDBModel();
            DataTable DT = objApplicationFlowModel.GetUserGccList(UserId, MappingID);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTransferToUserList(int UserId = 0, int MappingID = 0)
        {
            GopalCreditCardDBModel objApplicationFlowModel = new GopalCreditCardDBModel();
            DataTable DT = objApplicationFlowModel.GetTransferToUserList(UserId, MappingID);
            return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, JsonRequestBehavior.AllowGet);
        }


        //[HttpPost]
        //public int TransferGcc_old(int UserId,int OldUserId, string GccIDS)
        //{
        //    GopalCreditCardDBModel objApplicationFlowModel = new GopalCreditCardDBModel();
        //    int a = objApplicationFlowModel.TransferGcc(GccIDS,UserId, OldUserId);
        //    return a;
        //  }


        [HttpPost]
        public int TransferGcc(TransferGccRequest request)
        {
            GopalCreditCardDBModel objApplicationFlowModel = new GopalCreditCardDBModel();
            int result = objApplicationFlowModel.TransferGcc(request.GccIDS, request.UserId, request.OldUserId);
            return result;
        }


        #endregion

        #region MahilaNidhi


        public ActionResult GccMahilaNidhiApplicationForm()
        {
            return View();
        }
        public ActionResult GccMahilaNidhiApplicationFormView()
        {
            return View();
        }


        #endregion

        #region MapGccSSO
        [HttpPost]
        public string GetSSOMapRequestList(string registrationNo, int pacsId = 0)
        {
            GopalCreditCardDBModel GopalCreditCardDBModel = new GopalCreditCardDBModel();
            return COMMON_FUNCTION.DataTableToJSONWithJSONNet(GopalCreditCardDBModel.GetSSOMapRequestList(registrationNo, pacsId));
        }


        [HttpPost]
        public string UpdateGccMAPRequest(string GccIdEnc, string SSOId, string Remarks)
        {

            ViewBag.LoggedInUserDistrictId = _LoggedInUser.DistrictId;
            GopalCreditCardDBModel GopalCreditCardDBModel = new GopalCreditCardDBModel();
            int DistrictId = _LoggedInUser.DistrictId;
            string result = GopalCreditCardDBModel.UpdateGccMAPRequest(GccIdEnc, SSOId, Remarks);
            return result;
        }
        #endregion

        #region RecoveryGcc       

        //[HttpPost]
        //[ActionName("SetGccRecoverablePayment")]
        //public int SetGccRecoverablePayment(string json)
        //{
        //    int result = -1;
        //    GopalCreditCardDBModel GopalCreditCardDBModel = new GopalCreditCardDBModel();
        //    result = GopalCreditCardDBModel.SetGccRecoverablePayment(json);
        //    return result;
        //}


        [HttpPost]
        [ActionName("SetGccRecoverablePayment")]
        public int SetGccRecoverablePayment(string json)
        {
            HttpPostedFileBase file = Request.Files["ReceiptFile"];
            string savedFileName = null;

            if (file != null && file.ContentLength > 0)
            {
                string folder = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/GCC/Receipts/");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                savedFileName = Guid.NewGuid().ToString() + ".pdf";
                string fullPath = Path.Combine(folder, savedFileName);

                file.SaveAs(fullPath);

                //json = json.TrimEnd('}');
                //json = json + ",\"ReceiptFileName\":\"" + savedFileName + "\"}";
                JObject obj = JObject.Parse(json);
                obj["ReceiptFileName"] = savedFileName;
                json = obj.ToString();
            }


            int result = -1;
            GopalCreditCardDBModel GopalCreditCardDBModel = new GopalCreditCardDBModel();
            result = GopalCreditCardDBModel.SetGccRecoverablePayment(json);
            return result;
        }


        [HttpPost]
        public string GetJanMemberData(GopalCreditCardModel.DashBoardDataModel model)
        {
            model.SSOID = _LoggedInUser.SSOID;
            model.UserType = _LoggedInUser.UserType;
            model.KIOSKCODE = _LoggedInUser.KIOSKCODE;

            DataTable DT = null;
            GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());
            DT = objGopalCreditCardDBModel.Get_DashBoardData(model);
            if (DT != null)
            {
                if (DT != null && DT.Rows.Count > 0)
                {
                    DT.Columns.Add("Encrypted_RegistrationNo");
                    DT.Columns.Add("Encrypted_GopalCreditCardId");
                }
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    DT.Rows[i]["Encrypted_RegistrationNo"] = !String.IsNullOrEmpty(DT.Rows[i]["RegistrationNo"].ToString()) == true ? COMMON_FUNCTION.Encrypt(DT.Rows[i]["RegistrationNo"].ToString()) : "";
                    DT.Rows[i]["Encrypted_GopalCreditCardId"] = !String.IsNullOrEmpty(DT.Rows[i]["GopalCreditCardId"].ToString()) == true ? COMMON_FUNCTION.Encrypt(DT.Rows[i]["GopalCreditCardId"].ToString()) : "";

                }
                var jsonData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT);
                var resultDataModel = JsonConvert.DeserializeObject<List<GopalCreditCardModel.DashBoardDataModel>>(jsonData);
                List<GopalCreditCardModel.DashBoardDataModel> listData = new List<DashBoardDataModel>();
                listData = resultDataModel;
                var json = new JavaScriptSerializer().Serialize(listData);
                return json; ;
            }
            else
            {
                return null;
            }


        }


        #endregion


        [HttpPost]
        [ActionName("SaveGSSApplicationFlowStatus_Recovery")]
        public JsonResult SaveGSSApplicationFlowStatus_Recovery(string OTP = "", string TXN = "", GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel = null, string generalNumber = "")
        {
            string pdfPath = "";
            string succmsg = string.Empty;
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                if (_LoggedInUser == null)
                {
                    return Json("LOGOUT");
                }

                GopalCreditCardDBModel _Modal = new GopalCreditCardDBModel();
                // if (!ModelState.IsValid)    return Json(JsonModelError.Get(ModelState.Values));

                int result = 0;

                if (TXN != "" || (Request.Url.Host.Contains("localhost") || Request.Url.Host.Contains("devitsandbox")))
                {
                    ResultMessage otpResult = new ResultMessage();

                    //if (!Request.Url.Host.Contains("localhost") && _AppFlowModel.StatusId == 4)
                    //{
                    //    otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                    //}

                    if (!Request.Url.Host.Contains("localhost") && !Request.Url.Host.Contains("devitsandbox"))
                    {
                        otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);
                    }
                    else
                    {
                        otpResult.status = "1"; otpResult.data = TXN;
                    }



                    if (otpResult.status == "1")
                    {
                        if (_AppFlowModel.StatusId == 34)
                        {

                            if (_AppFlowModel.StatusId == 34) //(NOC Generate)
                            {
                                pdfPath = NOCDocumnetGenerate(_Model, _AppFlowModel);
                            }

                            if (!pdfPath.Contains("Fail"))
                            {
                                //var Filepath = pdfPath.Split('#');
                                string[] files = pdfPath.Split('#');
                                for (int x = 0; x < files.Count(); x++)
                                {
                                    if (files[x] != null && files[x] != "")
                                        files[x] = Server.MapPath("~/Content/GopalCreditCard/GssProfiling/") + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", "") + "/" + files[x];
                                }


                                if (!Request.Url.Host.Contains("localhost") && !Request.Url.Host.Contains("devitsandbox"))
                                {

                                    if (_AppFlowModel.StatusId == 19)
                                    {
                                        // var result1 = COMMON_FUNCTION.EsignPDF_NEW(otpResult.data, string.Join("#", files), _Model.LoanBankName_ENG, "Managing Director");
                                        var result1 = COMMON_FUNCTION.EsinghPDF(otpResult.data, string.Join("#", files), _Model.LoanBankName_ENG, "Managing Director");

                                        succmsg = result1.status;


                                        // succmsg = "1";
                                    }
                                    if (_AppFlowModel.StatusId == 34)
                                    {
                                        //var result1 = COMMON_FUNCTION.EsignPDF_NEW(otpResult.data, string.Join("#", files), _Model.PACSName_ENG, "Pacs Manager");
                                        var result1 = COMMON_FUNCTION.EsinghPDF(otpResult.data, string.Join("#", files), _Model.PACSName_ENG, "Pacs Manager");

                                        succmsg = result1.status;

                                        //if (succmsg == "1") {

                                        //    _AppFlowModel.DocumentURL =  ;
                                        //}
                                    }

                                }

                                if (succmsg == "1" || (Request.Url.Host.Contains("localhost") || Request.Url.Host.Contains("devitsandbox")))
                                {

                                    _AppFlowModel.DocumentURL = pdfPath;
                                    result = _Modal.AddApplicationFlow_Recovery(_AppFlowModel);
                                    if (result > 0)
                                    {
                                        SendSms(_AppFlowModel.StatusId, _Model.MobileNo, _Model.RegistrationNo, _Model.gcc_id);
                                        if (_AppFlowModel.StatusId == 19)
                                        {
                                            //  var status = _Modal.PushGCCTransactionToJanaadhar(_Model, _AppFlowModel, result);

                                        }
                                    }
                                }

                            }
                            else
                            {
                                result = 0;
                                succmsg = pdfPath;
                            }
                        }
                        else
                        {
                            string fullPath = "";
                            if (_AppFlowModel.DocumentURL != null && _AppFlowModel.DocumentURL != "")
                                fullPath = Server.MapPath("~/Content/GopalCreditCard/GSSProfiling/") + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", "") + "/" + _AppFlowModel.DocumentURL;


                            result = _Modal.AddApplicationFlow_Recovery(_AppFlowModel);

                            // Update General Number and StatusId only when StatusId = 32(PAYMENT APPROVED)
                            if (_AppFlowModel.StatusId == 32)
                            {
                                int updateResult = _Modal.UpdateGeneralNumber(_AppFlowModel.GccId, _AppFlowModel.PaymentSummaryId, generalNumber, _AppFlowModel.StatusId);
                            }

                            if (_Model.MobileNo != null && result > 0 && (_AppFlowModel.StatusId == 16 || _AppFlowModel.StatusId == 17 || _AppFlowModel.StatusId == 13 || _AppFlowModel.StatusId == 19 || _AppFlowModel.StatusId == 4))
                            {
                                SendSms(_AppFlowModel.StatusId, _Model.MobileNo, _Model.RegistrationNo, _Model.gcc_id);
                            }

                        }

                    }
                    else
                    {
                        result = -2;
                    }
                }
                else
                {
                    result = 0;
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = "SaveGSSApplicationFlowStatus_V1( string OTP = \"\", string TXN = \"\",  GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel=null)" }, ex);
                return null;
            }

        }



        public string GenerateNOCCertificate(GopalCreditCardFormModel _Model, string PdfName = "NOCGenerateLetter", string TXN = "")
        {
            try
            {
                string wkhtmlPath = Server.MapPath("~/bin/wkhtmltopdf.exe");
                string fontPath = new Uri(Server.MapPath("~/fonts/lohit-Devanagari.ttf")).AbsoluteUri;
                string iycLogo = new Uri(Server.MapPath("~/Content/images/iyc_logo.png")).AbsoluteUri;
                string cmPhoto = new Uri(Server.MapPath("~/Content/images/cm-ots-logo.png")).AbsoluteUri;

                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {

                    var con = Get_GopalApplicationFormViewData_v1(_Model.GccIdDBEnc);
                    var conData = JsonConvert.DeserializeObject<GopalCreditCardFormModel>(con);


                    var file = System.IO.Path.Combine(Server.MapPath("~/Template"), "Gcc_Noc_letter_format.html");
                    StringBuilder strbld = new StringBuilder(System.IO.File.ReadAllText(file));

                    strbld.Replace("#Bank#", _Model.LoanBankName_ENG ?? "")
                          .Replace("#Branch#", _Model.LoanBranchName_ENG ?? "")
                          .Replace("#PacsName#", _Model.PACSName_ENG ?? "")
                          .Replace("#UserName#", _LoggedInUser?.OfficerNameEn ?? "")
                          .Replace("#SocietyRegistrationNo#", conData?.RegistrationNo ?? "")
                          .Replace("#NocDate#", DateTime.Now.ToString("dd-MM-yyyy"))
                          .Replace("#SanctionAmount#", _Model.SanctionAmt.ToString())
                          .Replace("#DisbursedDate#", _Model.DisbursedDate.HasValue ? _Model.DisbursedDate.Value.ToString("dd-MM-yyyy") : "")
                          .Replace("#PaymentDate#", _Model.PaymentDate.HasValue ? _Model.PaymentDate.Value.ToString("dd-MM-yyyy") : "");
                    //.Replace("#TotalInstallment#", TotalInstallment.ToString())
                    //.Replace("#LateInterestRate#", LateInterestRate.ToString())
                    //.Replace("#InterestRate#", InterestRate.ToString())
                    //.Replace("#TotalInterestRate#", TotalInterestRate.ToString());
                    if (!string.IsNullOrEmpty(_Model.DCSName) && ContainsUnicodeCharacter(_Model.PACSName_ENG))
                    {
                        strbld.Replace("#SocietyName#", _Model.DCSName);
                    }


                    strbld.Replace("#LoaneeName#", UnicodeToKrutidev.GetKrutidev(conData?.Name_ENG ?? ""))
                          .Replace("#LoaneeFatherName#", UnicodeToKrutidev.GetKrutidev(conData?.FatherName_ENG ?? ""))
                          .Replace("#LoaneeAddress#", UnicodeToKrutidev.GetKrutidev(_Model.Address_ENG ?? ""));


                    string finalHtml = $@"
                   <html>
                    <head>
                        <meta charset='UTF-8'>
                        <style>
                            @font-face {{
                                font-family: 'lohit-Devanagari';
                                src: url('{fontPath}') format('truetype');
                            }}
                            body {{
                                font-family: 'lohit-Devanagari';
                                margin: 20px;
                            }}
                        </style>
                    </head>
                    <body>
                        {strbld}
                    </body>
                   </html>";

                    var pdf = PDFgenerator.GeneratePdfFromHtml(finalHtml, wkhtmlPath);
                    var fileName = $"NOCGenerateLetter_{Guid.NewGuid().ToString("N").ToUpper()}.pdf";

                    string cleanRegNo = _Model.RegistrationNo.Replace("/", "").Replace(".", "").Replace("_", "").Replace("-", "").Replace(" ", "");

                    string folderPath = Path.Combine(Server.MapPath("~/Content/GopalCreditCard/GSSProfiling/"), cleanRegNo);

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string filePath = Path.Combine(folderPath, fileName);

                    System.IO.File.WriteAllBytes(filePath, pdf);

                    var result1 = COMMON_FUNCTION.EsignPDF_NEW(TXN, filePath, _Model.LoanBankName_ENG?.ToUpper() ?? "BANK", "Secretary", "350", "170", "150", "170");

                    if (result1.data == "Success")
                        return Path.Combine(cleanRegNo, fileName).Replace("\\", "/");
                }
                else
                {
                    return "Fail";
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "GopalCreditCardController", FunctionName = "GenerateNOCCertificate" }, ex);
                return "Fail";
            }

            return "Fail";
        }





        //public string NOCCertificateGenerated(GopalCreditCardFormModel _Model = null, string PdfName = "NOCGenerateLetter", string TXN = "")
        //{


        //    try
        //    {
        //        string wkhtmlPath = Server.MapPath("~/bin/wkhtmltopdf.exe");
        //        string fontPath = new Uri(Server.MapPath("~/fonts/lohit-Devanagari.ttf")).AbsoluteUri;
        //        string iycLogo = new Uri(Server.MapPath("~/Content/images/iyc_logo.png")).AbsoluteUri;
        //        string cmPhoto = new Uri(Server.MapPath("~/Content/images/cm-ots-logo.png")).AbsoluteUri;

        //        if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
        //        {

        //            StringBuilder strbld = new StringBuilder();
        //            foreach (var app in _Model.PLDB_OTSSchemeData)
        //            {
        //                var file = "";
        //                if (app.NODPrincipal > 0 || app.NODInterest > 0 || app.NODPenalInterest > 0)
        //                    file = System.IO.Path.Combine(Server.MapPath("~/Template"), "ots_notice.html");
        //                else
        //                    file = System.IO.Path.Combine(Server.MapPath("~/Template"), "ots_notice_103.html");


        //                if (PdfName == "NOCGenerateLetter")
        //                {
        //                    file = System.IO.Path.Combine(Server.MapPath("~/Template"), "ots_certificate.html");
        //                }

        //                // ots_notice_103


        //                //var parts = new[] { app.VName, app.GpName, app.BlockName, app.TehsilName, app.VidhanSabha, app.DistrictName };
        //                var parts = new[] { app.VName, app.GpName, app.BlockName, app.DistrictName };
        //                var address = string.Join(", ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));

        //                strbld.Append(System.IO.File.ReadAllText(file));
        //                strbld.Replace("#PLDB_Name#", app.PLDBNameHi)
        //               .Replace("#LoaneeName#", app.ApplicantName)
        //               .Replace("#LoaneeAddress#", address)
        //               .Replace("#DistrictName#", app.DistrictName)
        //               .Replace("#LoanAccNo#", app.LoanAccNo)
        //               .Replace("#LoaneeFatherName#", app.FatherHusbandName);


        //                if (file.Contains("ots_certificate.html"))
        //                {

        //                    DateTime today = DateTime.Now;
        //                    string formattedDate = today.ToString("dd-MM-yyyy");

        //                    strbld.Replace("#RegistrationNo#", app.NoticeNo)
        //                    .Replace("#RegistrationDate#", formattedDate);
        //                    if (app.ODPrincipal > 0)
        //                        strbld.Replace("#TotalPaidAmount#", (app.NODInterest + app.NODPenalInterest).ToString());//18=16+17
        //                    else
        //                        strbld.Replace("#TotalPaidAmount#", (app.TotalInterestAsOnDateSettlement + app.InterestFromSettlementPre).ToString());//9=2+3

        //                }


        //                strbld.Replace("#cmPhoto#", cmPhoto);
        //                strbld.Replace("#iycLogo#", iycLogo);
        //            }

        //            string str = $@"
        //                <html>
        //                    <head>
        //                    <meta charset='UTF-8'>
        //                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        //                    <style>
        //                        @font-face {{
        //                            font-family: 'lohit-Devanagari';
        //                            src: url('{fontPath}') format('truetype');
        //                        }}
        //                        body {{
        //                            font-family: 'lohit-Devanagari';
        //                            margin: 0;
        //                            font-weight:lighter;
        //                        }}
        //                    </style>
        //                </head>
        //                <body>
        //                    {strbld.ToString()}
        //                </body>
        //                </html>";

        //            var pdf = PDFgenerator.GeneratePdfFromHtml(str, wkhtmlPath);
        //            var fileName = "OTS_Certificate_" + Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace("_", "").ToUpper() + ".pdf";

        //            //return fileName;


        //            string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Content/OTS/" + _Model.PLDB_OTSSchemeData[0].LoanAccNo.Replace("/", "").Replace(".", "").Replace("_", "").Replace("-", "").Replace(" ", "") + "/");

        //            if (!Directory.Exists(folderPath))
        //            {
        //                Directory.CreateDirectory(folderPath);
        //            }

        //            // Combine the directory path with the file name
        //            var filePath = Path.Combine(folderPath, fileName);

        //            // Save the PDF to the specified directory
        //            System.IO.File.WriteAllBytes(filePath, pdf);

        //            var result1 = COMMON_FUNCTION.EsignPDF_NEW(TXN, folderPath + "/" + fileName, _Model.PLDB_OTSSchemeData[0].PLDBName.ToUpper(), "Secretary", "350", "170", "150", "170");


        //            if (result1.data == "Success")
        //            {
        //                return fileName;
        //            }

        //        }
        //        else
        //        {
        //            return "Fail";
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ILogger objILogger = new DBLogger();
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "PLDBController", FunctionName = "CertificateGenerate catch " }, ex);

        //        return "Fail";
        //    }

        //    return "Fail";


        //}


        [HttpGet]
        [ActionName("GetGopalCreditById")]
        public string GetGopalCreditById(int GopalCreditCardId)
        {
            var gccModel = new GopalCreditCardDBModel(new DBLogger());
            DataSet ds = gccModel.GetGopalCreditById(GopalCreditCardId);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return JsonConvert.SerializeObject(ds.Tables[0], Newtonsoft.Json.Formatting.Indented);
            }
            else
            {
                return JsonConvert.SerializeObject(new { message = "No record found for given GccId." });
            }
        }

        [HttpPost]
        public async Task<string> GetGuarantorJanAadhaarDetails(int loanBankID, string JanaadhaarNo)
        {
            JanaadhaarNo = csCommon.Decrypt_SA(Uri.UnescapeDataString(JanaadhaarNo));
            var mid = "";
            csCommon objComm = new csCommon();

            DataTable dt = new DataTable();
            dt = await objComm.GetJanAadhaarMIDByAadhaar(JanaadhaarNo);
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (!dt.Columns.Contains("MaskedMid"))
                        dt.Columns.Add("MaskedMid", typeof(string));
                    if (!dt.Columns.Contains("ENC_AADHAR_REF_NO"))
                        dt.Columns.Add("ENC_AADHAR_REF_NO", typeof(string));
                    if (!dt.Columns.Contains("Message"))
                    {
                        dt.Columns.Add("Message", typeof(string));
                        dt.Rows[0]["Message"] = "Success";
                    }

                    if (dt.Columns.Contains("JAN_MEMBER_ID"))
                        foreach (DataRow dr in dt.Rows) // search whole table
                        {
                            dr["MaskedMid"] = dr["JAN_MEMBER_ID"].ToString();
                            dr["ENC_AADHAR_REF_NO"] = COMMON_FUNCTION.Encrypt(dr["AADHAR_REF_NO"].ToString());
                            if (dr["JAN_MEMBER_ID"].ToString().Length > 8)
                            {
                                var Mid = dr["JAN_MEMBER_ID"].ToString();
                                Session["JAN_MEMBER_ID"] = dr["JAN_MEMBER_ID"].ToString();

                                var firstDigits = Mid.Substring(0, 5);
                                var lastDigits = Mid.Substring(Mid.Length - 4, 4);
                                var requiredMask = new String('X', Mid.Length - lastDigits.Length);
                                var maskedString = string.Concat(requiredMask, lastDigits);
                                var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0");
                                dr["MaskedMid"] = maskedString;
                                Session["JanaadhaarNo"] = COMMON_FUNCTION.Decrypt(dr["ENC_JAN_AADHAR"].ToString());
                            }

                            GCCServiceModel _gccServiceModel = new GCCServiceModel(new DBLogger());
                            var resOdData = await _gccServiceModel.CheckGcctoCBSByJanMemberId(loanBankID, dr["JAN_MEMBER_ID"].ToString());
                            if (resOdData != null)
                            {
                                int cbsOdResult = resOdData.ODFlag == "N" ? 0 : (resOdData.ODFlag == null || resOdData.ODFlag == "Null") ? 2 : 1;
                                if (cbsOdResult == 2)
                                {
                                    dr["Message"] = "The service is down, please try again.";
                                    return COMMON_FUNCTION.DataTableToJSONString(dt);
                                }
                                if (cbsOdResult == 0)
                                {
                                    var gccModel = new GopalCreditCardDBModel(new DBLogger());
                                    int result = gccModel.ValidateGurantor(dr["JAN_AADHAR"].ToString(), dr["AADHAR_REF_NO"].ToString());
                                    if (result == 0)
                                    {
                                        dr["Message"] = "The guarantor has already provided guarantees for two applications and cannot provide additional guarantees.";
                                        return COMMON_FUNCTION.DataTableToJSONString(dt);
                                    }
                                    else if (result == 1)
                                    {
                                        dr["Message"] = "Success";
                                        return COMMON_FUNCTION.DataTableToJSONString(dt);
                                    }
                                }
                                else
                                {
                                    dr["Message"] = "This guarantor is not eligible to provide a guarantee due to an existing overdraft(OD).";
                                    return COMMON_FUNCTION.DataTableToJSONString(dt);
                                }
                            }
                            else
                            {
                                dr["Message"] = "Success";
                                return COMMON_FUNCTION.DataTableToJSONString(dt);
                            }
                        }
                    // check gurantor is duplicate or OD

                }

            }
            catch (Exception ex)
            {

                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen", FunctionName = "GetJanAadhaarDetails(string JanaadhaarNo) JanaadhaarNo:-" + JanaadhaarNo }, ex);
            }
            if (dt.Rows.Count > 0)
            {
                return COMMON_FUNCTION.DataTableToJSONString(dt);

            }
            else
            {
                dt = null;
                return COMMON_FUNCTION.DataTableToJSONString(dt);
            }


        }

        [HttpPost]
        [ActionName("GCCApplicationEsign")]
        public JsonResult GCCApplicationEsign(string OTP = "", string TXN = "", int signedBy = 0, string location = "", string designation = "", string fileName = "", string gccid = "0")
        {
            string pdfPath = "";
            string succmsg = string.Empty;
            int result = 0;

            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                if (_LoggedInUser == null)
                    return Json("LOGOUT");

                if (!ModelState.IsValid)
                    return Json(JsonModelError.Get(ModelState.Values));

                GopalCreditCardDBModel _Modal = new GopalCreditCardDBModel();
                bool isLocal = Request.Url.Host.Contains("localhost") || Request.Url.Host.Contains("devitsandbox");
                if (TXN != "" || isLocal)
                {
                    ResultMessage otpResult = new ResultMessage();

                    if (!isLocal)
                        otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);

                    otpResult.status = "1";
                    otpResult.data = TXN;

                    if (otpResult.status == "1")
                    {

                        foreach (string file in fileName.Split('#'))
                        {
                            pdfPath += Server.MapPath("~/Content/GopalCreditCard/GSSProfiling/") + file + "#";
                        }
                        pdfPath = pdfPath.Substring(0, pdfPath.Length - 1);

                        if (!pdfPath.Contains("Fail"))
                        {
                            ResultMessage resultResponse = new ResultMessage();
                            if (signedBy == 0 || signedBy == 1)
                            {
                                resultResponse = COMMON_FUNCTION.EsignPDF_GCC(otpResult.data, pdfPath, location, "PACS Manager", 0, "380", "-50");
                                succmsg = resultResponse.status;
                            }
                            else if (signedBy == 2)
                            {
                                resultResponse = COMMON_FUNCTION.EsignPDF_GCC(otpResult.data, pdfPath, location, "Guarantor 1", 0, "50", "200");
                                succmsg = resultResponse.status;
                            }
                            else if (signedBy == 3)
                            {
                                resultResponse = COMMON_FUNCTION.EsignPDF_GCC(otpResult.data, pdfPath, location, "Guarantor 2", 0, "300", "200");
                                succmsg = resultResponse.status;
                            }
                            else if (signedBy == 4)
                            {
                                resultResponse = COMMON_FUNCTION.EsignPDF_GCC(otpResult.data, pdfPath, location, "CITIZEN", 0, "50", "-50");
                                succmsg = resultResponse.status;
                            }

                            if (resultResponse.status == "1")
                            {
                                GopalCreditCardFormModel creditCardFormModel = new GopalCreditCardFormModel();
                                creditCardFormModel.gcc_id = Convert.ToInt32(gccid);
                                if (signedBy == 0 || signedBy == 1)
                                    creditCardFormModel.IsPacsManagerSigned = true;
                                else if (signedBy == 2)
                                    creditCardFormModel.IsGurantor1Signed = true;
                                else if (signedBy == 3)
                                    creditCardFormModel.IsGurantor2Signed = true;
                                else if (signedBy == 4)
                                    creditCardFormModel.IsApplicantSigned = true;
                                var gccModel = new GopalCreditCardDBModel(new DBLogger());
                                int r = gccModel.UpdateEsingInfoOfGCC(creditCardFormModel);
                            }
                            return Json(resultResponse);
                        }
                        else
                        {
                            result = 0;
                            succmsg = pdfPath;
                        }
                    }
                    else
                    {
                        result = -2;
                    }
                }
                else
                {
                    result = 0;
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = "GCCApplicationEsign(string OTP, string TXN, GopalCreditCardFormModel model, GccApplicationFlowModel appFlowModel)" }, ex);
                return null;
            }
        }


        [HttpPost]
        [ActionName("GCCApplicationEsignTest")]
        public JsonResult GCCApplicationEsignTest(string OTP = "", string TXN = "", int signedBy = 0, string location = "", string designation = "", string llx = "200", string lly = "-100")
        {
            string pdfPath = "";
            string succmsg = string.Empty;
            int result = 0;

            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                if (_LoggedInUser == null)
                    return Json("LOGOUT");

                if (!ModelState.IsValid)
                    return Json(JsonModelError.Get(ModelState.Values));

                GopalCreditCardDBModel _Modal = new GopalCreditCardDBModel();
                bool isLocal = Request.Url.Host.Contains("localhost") || Request.Url.Host.Contains("devitsandbox");
                if (TXN != "" || isLocal)
                {
                    ResultMessage otpResult = new ResultMessage();

                    if (!isLocal)
                        otpResult = COMMON_FUNCTION.VerifyAadhaarOTP(OTP, TXN);

                    otpResult.status = "1";
                    otpResult.data = TXN;

                    if (otpResult.status == "1")
                    {
                        pdfPath = Server.MapPath("~/Content/GopalCreditCard/") + "GccSignedForm003A2261B0E04F96827EAA5B28D5017C.pdf";

                        if (!pdfPath.Contains("Fail"))
                        {
                            ResultMessage resultResponse = new ResultMessage();
                            if (signedBy == 0 || signedBy == 1)
                            {
                                resultResponse = COMMON_FUNCTION.EsignPDF_GCC(otpResult.data, pdfPath, location, designation, 0, llx, lly);
                                succmsg = resultResponse.status;
                            }
                            else if (signedBy == 2)
                            {
                                resultResponse = COMMON_FUNCTION.EsignPDF_GCC(otpResult.data, pdfPath, location, designation, 8, llx, lly);
                                succmsg = resultResponse.status;
                            }
                            else if (signedBy == 3)
                            {
                                resultResponse = COMMON_FUNCTION.EsignPDF_GCC(otpResult.data, pdfPath, location, designation, 8, llx, lly);
                                succmsg = resultResponse.status;
                            }
                            else if (signedBy == 4)
                            {
                                resultResponse = COMMON_FUNCTION.EsignPDF_GCC(otpResult.data, pdfPath, location, designation, 0, llx, lly);
                                succmsg = resultResponse.status;
                            }

                            return Json(resultResponse);
                        }
                        else
                        {
                            result = 0;
                            succmsg = pdfPath;
                        }
                    }
                    else
                    {
                        result = -2;
                    }
                }
                else
                {
                    result = 0;
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = "GCCApplicationEsign(string OTP, string TXN, GopalCreditCardFormModel model, GccApplicationFlowModel appFlowModel)" }, ex);
                return null;
            }
        }


        [HttpGet]
        [ActionName("GCCApplicationSignedForm")]
        public string GCCApplicationSignedForm(GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel = null, int signedType = 0)
        {
            try
            {
                string wkhtmlPath = Server.MapPath("~/bin/wkhtmltopdf.exe");
                string fontPath = new Uri(Server.MapPath("~/fonts/lohit-Devanagari.ttf")).AbsoluteUri;
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {
                    StringBuilder strbld = new StringBuilder();
                    //var file = System.IO.Path.Combine(Server.MapPath("~/Template"), "Gcc_application_form.html");
                    var file = "";
                    string filePreName = "";
                    if (signedType == 1) //for loanee
                    {
                        file = System.IO.Path.Combine(Server.MapPath("~/Template"), "Gcc_application_form_loanee.html");
                        filePreName = "GccSignedFormLoanee";
                    }
                    else if (signedType == 2) //for pacs
                    {
                        file = System.IO.Path.Combine(Server.MapPath("~/Template"), "Gcc_application_form_pacs.html");
                        filePreName = "GccSignedFormPacs";
                    }
                    else if (signedType == 3) //for gurantor
                    {
                        file = System.IO.Path.Combine(Server.MapPath("~/Template"), "Gcc_application_gurantor_form.html");
                        filePreName = "GccSignedGurantorForm";
                    }

                    string purposeOfLoan = "";
                    string guarantor1Name = "";
                    string guarantor1MobileNo = "";
                    string guarantor2Name = "";
                    string guarantor2MobileNo = "";
                    string guarantor1Address = "";
                    string guarantor2Address = "";
                    string guarantor1FatherName = "";
                    string guarantor2FatherName = "";
                    string category = "";
                    if (_Model != null)
                    {
                        if (_Model.IsWorking == true) { purposeOfLoan += "कार्यशील पूंजी, "; }
                        if (_Model.IsInfrastructure == true) { purposeOfLoan += "शेड, खेली, डेयरी गद्दे, "; }
                        if (_Model.IsApparatus == true) { purposeOfLoan += "दूध उपकरण "; }
                        purposeOfLoan = purposeOfLoan.TrimEnd(',', ' ');

                        guarantor1Name = _Model.GuarantorDetailsList.Count > 0 ? _Model.GuarantorDetailsList[0].Name : "";
                        guarantor1MobileNo = _Model.GuarantorDetailsList.Count > 0 ? _Model.GuarantorDetailsList[0].MobileNo : "";
                        guarantor1Address = _Model.GuarantorDetailsList.Count > 0 ? _Model.GuarantorDetailsList[0].Address : "";
                        guarantor1FatherName = _Model.GuarantorDetailsList.Count > 0 ? _Model.GuarantorDetailsList[0].FatherSpouseName : "";
                        guarantor2Name = _Model.GuarantorDetailsList.Count > 1 ? _Model.GuarantorDetailsList[1].Name : "";
                        guarantor2MobileNo = _Model.GuarantorDetailsList.Count > 1 ? _Model.GuarantorDetailsList[1].MobileNo : "";
                        guarantor2Address = _Model.GuarantorDetailsList.Count > 1 ? _Model.GuarantorDetailsList[1].Address : "";
                        guarantor2FatherName = _Model.GuarantorDetailsList.Count > 1 ? _Model.GuarantorDetailsList[1].FatherSpouseName : "";
                        switch (_Model.Category)
                        {
                            case "1":
                                category = "अ.ज.जा";
                                break;
                            case "2":
                                category = "अ.जा";
                                break;
                            case "3":
                                category = "सामान्य";
                                break;
                            case "4":
                                category = "ओ.बी.सी";
                                break;
                            default:
                                category = "अन्य";
                                break;
                        }

                    }


                    strbld.Append(System.IO.File.ReadAllText(file));

                    strbld.Replace("#Name#", _Model.Name_ENG)
                    .Replace("#FatherName#", _Model.FatherName_ENG)
                    .Replace("#Age#", _Model.Age)
                    .Replace("#Caste#", category)
                    .Replace("#RegistrationNo#", _Model.RegistrationNo)
                    .Replace("#Address#", $"{_Model.HouseBuildingNo}, {_Model.LaneStreetName}, {_Model.Address_ENG}, {_Model.DistrictName_ENG}")
                    .Replace("#BlockName#", _Model.Block_ENG)
                    .Replace("#VillageName#", _Model.Village_ENG)
                    .Replace("#LoanAmountRecommendedByDCS#", NumberToHindiWordConverter.FormatDecimalAmount(Convert.ToDecimal(_Model.SanctionAmt)))
                    .Replace("#LoanAmountRecommendedByDCSWords#", NumberToHindiWordConverter.ConvertAmtWords(Convert.ToDecimal(_Model.SanctionAmt)))
                    .Replace("#AgricultureLandIrrigated#", (_Model.AgricultureLandDetailList?.Count ?? 0) > 0 ? _Model.AgricultureLandDetailList[0].Irrigated.ToString() : "0" + " हैक्टर")
                    .Replace("#Guarantor1Name#", guarantor1Name)
                    .Replace("#Guarantor1MobileNo#", "")
                    .Replace("#Guarantor1Address#", guarantor1Address)
                    .Replace("#Guarantor1Father#", guarantor1FatherName)
                    .Replace("#Guarantor2Name#", guarantor2Name)
                    .Replace("#Guarantor2MobileNo#", "")
                    .Replace("#Guarantor2Address#", guarantor2Address)
                    .Replace("#Guarantor2Father#", guarantor2FatherName)
                    .Replace("#Guarantor1Caste#", "")
                    .Replace("#Guarantor2Caste#", "")
                    .Replace("#Guarantor1Tehsil#", "")
                    .Replace("#Guarantor2Tehsil#", "")
                    .Replace("#Date#", DateTime.Now.ToString("dd-MM-yyyy"))
                    .Replace("#AccountNumber#", _Model.BankAccountNo)
                    .Replace("#BranchName#", _Model.BankBranchName)
                    .Replace("#ApplicantRuralResidence#", (_Model.IsApplicantRuralResidence == true ? "हां" : "नहीं"))
                    .Replace("#PacsName#", _Model.PACSName_ENG)
                    .Replace("#District#", _Model.DistrictName_ENG)
                    .Replace("#CCBName#", _Model.LoanBankName_HI)
                    .Replace("#PurposeOfLoan#", purposeOfLoan);

                    string finalHtml = $@"
                   <html>
                    <head>
                        <meta charset='UTF-8'>
                        <style>
                            @font-face {{
                                font-family: 'lohit-Devanagari';
                                src: url('{fontPath}') format('truetype');
                            }}
                            body {{
                                font-family: 'lohit-Devanagari';
                                margin: 20px;
                            }}
                        </style>
                    </head>
                    <body>
                        {strbld}
                    </body>
                   </html>";

                    var pdf = PDFgenerator.GeneratePdfFromHtml(finalHtml, wkhtmlPath);
                    //var fileName = $"GccSignedForm{Guid.NewGuid().ToString("N").ToUpper()}.pdf";
                    var fileName = $"{filePreName}{Guid.NewGuid().ToString("N").ToUpper()}.pdf";

                    string cleanRegNo = _Model.RegistrationNo.Replace("/", "").Replace(".", "").Replace("_", "").Replace("-", "").Replace(" ", "");

                    string folderPath = Path.Combine(Server.MapPath("~/Content/GopalCreditCard/GSSProfiling/"), cleanRegNo);

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    string filePath = Path.Combine(folderPath, fileName);
                    System.IO.File.WriteAllBytes(filePath, pdf);

                    var pdfPath = fileName;
                    if (_AppFlowModel.DocumentURL != null && _AppFlowModel.DocumentURL != "")
                    {
                        pdfPath += "#" + _AppFlowModel.DocumentURL;
                    }
                    return pdfPath;
                }
                else
                {
                    return "Fail";
                }

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "GopalCreditCardController", FunctionName = "GCCApplicationSignedForm()" }, ex);

                return "Fail";
            }
        }



        [HttpPost]
        [ActionName("SaveGCCRecoveryApplication")]
        public JsonResult SaveGCCRecoveryApplication(GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel = null, string generalNumber = "")
        {
            string pdfPath = "";
            string message = string.Empty;
            int result = 0;

            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null) { return Json("LOGOUT"); }

                GopalCreditCardDBModel _Modal = new GopalCreditCardDBModel();
                if (_AppFlowModel.StatusId == 34)
                {
                    // NOC Generate
                    pdfPath = NOCDocumnetGenerate(_Model, _AppFlowModel);
                    if (!pdfPath.Contains("Fail"))
                    {
                        string[] files = pdfPath.Split('#');
                        for (int x = 0; x < files.Count(); x++)
                        {
                            if (!string.IsNullOrEmpty(files[x]))
                            {
                                files[x] = Server.MapPath("~/Content/GopalCreditCard/GssProfiling/") + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", "") + "/" + files[x];
                            }
                        }

                        if (!Request.Url.Host.Contains("localhost") && !Request.Url.Host.Contains("devitsandbox"))
                        {
                            if (_AppFlowModel.StatusId == 19)
                            {
                                // var result1 = COMMON_FUNCTION.EsignPDF_NEW(
                                //     otpResult.data,
                                //     string.Join("#", files),
                                //     _Model.LoanBankName_ENG,
                                //     "Managing Director");

                                // succmsg = result1.status;
                                // succmsg = "1";
                            }

                            if (Request.Url.Host.Contains("localhost") || Request.Url.Host.Contains("devitsandbox"))
                            {
                                _AppFlowModel.DocumentURL = pdfPath;
                                result = _Modal.AddApplicationFlow_Recovery(_AppFlowModel);

                                if (result > 0)
                                {
                                    SendSms(_AppFlowModel.StatusId, _Model.MobileNo, _Model.RegistrationNo, _Model.gcc_id);
                                    if (_AppFlowModel.StatusId == 19)
                                    {
                                        // Existing empty block retained
                                    }
                                }
                            }
                        }
                        else
                        {
                            result = 0;
                            message = pdfPath;
                        }
                    }
                    else
                    {
                        string fullPath = "";
                        if (!string.IsNullOrEmpty(_AppFlowModel.DocumentURL))
                        {
                            fullPath = Server.MapPath("~/Content/GopalCreditCard/GSSProfiling/") + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", "") + "/" + _AppFlowModel.DocumentURL;
                        }
                        result = _Modal.AddApplicationFlow_Recovery(_AppFlowModel);
                        if (_AppFlowModel.StatusId == 32)
                        {
                            int updateResult = _Modal.UpdateGeneralNumber(_AppFlowModel.GccId, _AppFlowModel.PaymentSummaryId, generalNumber, _AppFlowModel.StatusId);
                        }
                        if (_Model.MobileNo != null && result > 0 && (_AppFlowModel.StatusId == 16 || _AppFlowModel.StatusId == 17 || _AppFlowModel.StatusId == 13 || _AppFlowModel.StatusId == 19 || _AppFlowModel.StatusId == 4))
                        {
                            SendSms(_AppFlowModel.StatusId, _Model.MobileNo, _Model.RegistrationNo, _Model.gcc_id);
                        }
                    }
                }

                return Json(new
                {
                    Result = result,
                    Message = message
                });
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();

                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.GopalCreditCardController",
                    FunctionName = "SaveGCCRecoveryApplication(GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel = null)"
                }, ex);

                return null;
            }
        }


        [HttpPost]
        [ActionName("generategccnoc")]
        public JsonResult GenerateGCCNOC(GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel = null, string generalNumber = "")
        {
            string pdfPath = "";
            string message = string.Empty;
            int result = 0;
            string virtualPath = string.Empty;

            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null) { return Json("LOGOUT"); }

                GopalCreditCardDBModel _Modal = new GopalCreditCardDBModel();
                if (_AppFlowModel.StatusId == 34)
                {
                    if (_AppFlowModel.StatusId == 34) //(NOC Generate)
                    {
                        pdfPath = NOCDocumnetGenerate(_Model, _AppFlowModel);
                        virtualPath = Server.MapPath("~/Content/GopalCreditCard/GssProfiling/") + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", "") + "/" + pdfPath;
                    }

                    if (!pdfPath.Contains("Fail"))
                    {
                        //string[] files = pdfPath.Split('#');
                        //for (int x = 0; x < files.Count(); x++)
                        //{
                        //    if (!string.IsNullOrEmpty(files[x]))
                        //    {
                        //        files[x] = Server.MapPath("~/Content/GopalCreditCard/GssProfiling/") + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", "") + "/" + files[x];
                        //    }
                        //}

                        if (Request.Url.Host.Contains("localhost") || Request.Url.Host.Contains("devitsandbox"))
                        {
                            //_AppFlowModel.DocumentURL = pdfPath;
                            //result = _Modal.AddApplicationFlow_Recovery(_AppFlowModel);
                            //if (result > 0)
                            //{
                            //    SendSms(_AppFlowModel.StatusId, _Model.MobileNo, _Model.RegistrationNo, _Model.gcc_id);
                            //    if (_AppFlowModel.StatusId == 19)
                            //    {
                            //        // var status = _Modal.PushGCCTransactionToJanaadhar(_Model, _AppFlowModel, result);
                            //    }
                            //}
                        }
                        return Json(new
                        {
                            Success = true,
                            DocPath = virtualPath,
                            Message = "Success",
                            DocType = "NOC Certificate",
                            DocName = pdfPath,
                            ReturnPath = Url.Action("GccRecoveryApplicationList", "gopalCreditCard"),
                            Location = _Model.PACSName_ENG,
                            Designation = "Pacs Manager"
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Success = false,
                            DocPath = "",
                            Message = "",
                            DocType = "",
                            DocName = "",
                            ReturnPath = "",
                            Location = "",
                            Designation = ""
                        });
                    }
                }
                else
                {
                    string fullPath = "";
                    if (!string.IsNullOrEmpty(_AppFlowModel.DocumentURL))
                    {
                        fullPath = Server.MapPath("~/Content/GopalCreditCard/GSSProfiling/") + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", "") + "/" + _AppFlowModel.DocumentURL;
                    }

                    result = _Modal.AddApplicationFlow_Recovery(_AppFlowModel);
                    // Update General Number and StatusId only when StatusId = 32(PAYMENT APPROVED)
                    if (_AppFlowModel.StatusId == 32)
                    {
                        int updateResult = _Modal.UpdateGeneralNumber(_AppFlowModel.GccId, _AppFlowModel.PaymentSummaryId, generalNumber, _AppFlowModel.StatusId);
                    }
                    if (_Model.MobileNo != null && result > 0 && (_AppFlowModel.StatusId == 16 || _AppFlowModel.StatusId == 17 || _AppFlowModel.StatusId == 13 || _AppFlowModel.StatusId == 19 || _AppFlowModel.StatusId == 4))
                    {
                        SendSms(_AppFlowModel.StatusId, _Model.MobileNo, _Model.RegistrationNo, _Model.gcc_id);
                    }

                    return Json(new
                    {
                        Success = (result > 0),
                        Result = result
                    });
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();

                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.GopalCreditCardController",
                    FunctionName = "GenerateGCCNOC(GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel = null)"
                }, ex);
                return Json(new { Success = false, Message = ex.Message });
            }
        }

        [HttpPost]
        [ActionName("GCCESignApplication")]
        public JsonResult GCCESignApplication(int signedBy = 0, string fileName = "", string gccid = "0")
        {
            string pdfPath = "";
            string documentName = "";

            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                if (_LoggedInUser == null)
                    return Json("LOGOUT");

                if (!ModelState.IsValid)
                    return Json(JsonModelError.Get(ModelState.Values));

                string[] files = fileName.Split('#');

                if (files.Length == 0 || string.IsNullOrEmpty(files[0]))
                {
                    return Json(new
                    {
                        Success = false,
                        Message = "File not found"
                    }, JsonRequestBehavior.AllowGet);
                }

                pdfPath = "~/Content/GopalCreditCard/GSSProfiling/" + files[0];
                if (!pdfPath.Contains("Fail"))
                {
                    if (signedBy == 4)
                    {
                        documentName = files[0];
                        string docPath = pdfPath;
                        return Json(new
                        {
                            Success = true,
                            DocPath = docPath,
                            DocType = "Citizen Certificate",
                            DocName = documentName,
                            ReturnPath = Url.Action("GopalApplicationForm", "GopalCreditCard"),
                            docid = gccid,
                            Location = "",
                            Designation = ""
                        }, JsonRequestBehavior.AllowGet);
                    }
                }

                return Json(new
                {
                    Success = false,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    docid = gccid,
                    Location = "",
                    Designation = ""
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.GopalCreditCardController",
                    FunctionName = "GCCESignApplication"
                }, ex);

                return Json(new
                {
                    Success = false,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    docid = "",
                    Message = ex.Message,
                    Location = "",
                    Designation = ""
                }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [ActionName("EsignForPacsGuarantor")]
        public JsonResult EsignForPacsGuarantor(int signedBy = 0, string fileName = "", string gccid = "0")
        {
            string pdfPath = "";
            GopalCreditCardDBModel _Modal = new GopalCreditCardDBModel();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser == null)
                    return Json("LOGOUT");

                if (!ModelState.IsValid)
                    return Json(JsonModelError.Get(ModelState.Values));

                //foreach (string file in fileName.Split('#'))
                //{
                //    pdfPath += Server.MapPath("~/Content/GopalCreditCard/GSSProfiling/") + file + "#";
                //}
                //pdfPath = pdfPath.Substring(0, pdfPath.Length - 1);
                pdfPath = Server.MapPath("~/Content/GopalCreditCard/GSSProfiling/") + fileName;
                if (!pdfPath.Contains("Fail"))
                {
                    if (signedBy == 0 || signedBy == 1)
                    {
                        return Json(new
                        {
                            Success = true,
                            DocPath = pdfPath,
                            DocType = "GCCPACS Certificate",
                            DocName = fileName,
                            ReturnPath = Url.Action("ApplicationFlowGCC", "GOPALCREDITCARD"),
                            Location = "",
                            Designation = "PACS Manager"
                        });
                    }
                    else if (signedBy == 2)
                    {
                        return Json(new
                        {
                            Success = true,
                            DocPath = pdfPath,
                            DocType = "Guarantor Certificate",
                            DocName = fileName,
                            ReturnPath = Url.Action("ApplicationFlowGCC", "GOPALCREDITCARD"),
                            Location = "",
                            Designation = "Guarantor 1"
                        });
                    }
                    else if (signedBy == 3)
                    {
                        return Json(new
                        {
                            Success = true,
                            DocPath = pdfPath,
                            DocType = "Guarantor1 Certificate",
                            DocName = fileName,
                            ReturnPath = Url.Action("ApplicationFlowGCC", "GOPALCREDITCARD"),
                            Location = "",
                            Designation = "Guarantor 2"
                        });
                    }
                    return Json(new
                    {
                        Success = false,
                        DocPath = "",
                        Message = "Invalid signedBy value.",
                        DocType = "",
                        DocName = "",
                        ReturnPath = "",
                        Location = "",
                        Designation = ""
                    });

                }
                else
                {
                    return Json(new
                    {
                        Success = false,
                        DocPath = "",
                        Message = "",
                        DocType = "",
                        DocName = "",
                        ReturnPath = "",
                        Location = "",
                        Designation = ""
                    });
                }

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = "GCCApplicationEsign(string OTP, string TXN, GopalCreditCardFormModel model, GccApplicationFlowModel appFlowModel)" }, ex);
                return Json(new
                {
                    Success = false,
                    DocPath = "",
                    Message = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    Location = "",
                    Designation = ""
                });
            }
        }


        [HttpPost]
        [ActionName("gccapplicationflowstatus")]
        public JsonResult GccApplicationFlowStatus(GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel = null)
        {
            string pdfPath = "";
            int result = 0;
            string succmsg = string.Empty;
            var responseModel = new PacsResponseModel();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                if (_LoggedInUser == null)
                {
                    return Json("LOGOUT");
                }

                GopalCreditCardDBModel _Modal = new GopalCreditCardDBModel();
                if (!ModelState.IsValid)
                    return Json(JsonModelError.Get(ModelState.Values));


                if (_AppFlowModel.StatusId == 19 || _AppFlowModel.StatusId == 4)
                {
                    if (_AppFlowModel.StatusId == 4)
                    {
                        if (!string.IsNullOrEmpty(_Model.RegistrationNo) && new GopalCreditCardDBModel().CheckRegistrationNoExists(_Model.RegistrationNo))
                        {
                            var resultData = GenerateReSanctionLatter(_Model, _AppFlowModel);
                            if (!string.IsNullOrWhiteSpace(resultData))
                            {
                                responseModel = JsonConvert.DeserializeObject<PacsResponseModel>(resultData);
                            }

                            if (responseModel == null)
                            {
                                return Json(new
                                {
                                    Success = false,
                                    Message = "No response received."
                                }, JsonRequestBehavior.AllowGet);
                            }

                            if (!responseModel.Success)
                            {
                                return Json(new
                                {
                                    Success = false,
                                    Result = 0,
                                    Message = responseModel.Message
                                }, JsonRequestBehavior.AllowGet);
                            }

                            return Json(new
                            {
                                Success = true,
                                DocPath = responseModel.DocPath ?? "",
                                DocType = responseModel.DocType ?? "",
                                DocName = responseModel.DocName ?? "",
                                ReturnPath = responseModel.ReturnPath ?? "",
                                Location = responseModel.Location ?? "",
                                Designation = responseModel.Designation ?? ""
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var resultData = GenerateCertificate(_Model, _AppFlowModel);
                            if (!string.IsNullOrWhiteSpace(resultData))
                            {
                                responseModel = JsonConvert.DeserializeObject<PacsResponseModel>(resultData);
                            }

                            if (responseModel == null)
                            {
                                return Json(new
                                {
                                    Success = false,
                                    Message = "No response received."
                                }, JsonRequestBehavior.AllowGet);
                            }

                            if (!responseModel.Success)
                            {
                                return Json(new
                                {
                                    Success = false,
                                    Result = 0,
                                    Message = responseModel.Message
                                }, JsonRequestBehavior.AllowGet);
                            }

                            return Json(new
                            {
                                Success = true,
                                DocPath = responseModel.DocPath ?? "",
                                DocType = responseModel.DocType ?? "",
                                DocName = responseModel.DocName ?? "",
                                ReturnPath = responseModel.ReturnPath ?? "",
                                Location = responseModel.Location ?? "",
                                Designation = responseModel.Designation ?? ""
                            }, JsonRequestBehavior.AllowGet);
                        }

                    }


                    if (_AppFlowModel.StatusId == 19)
                    {
                        GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());
                        pdfPath = GCCApplicationSignedForm(_Model, _AppFlowModel, 1);
                        GccDocumentsModel docAppForm = new GccDocumentsModel();
                        docAppForm.DocumentURL = pdfPath;
                        docAppForm.DocId = 7;
                        docAppForm.GccId = _AppFlowModel.GccId;
                        _AppFlowModel.DocId = objGopalCreditCardDBModel.InsertGCCReportFile(docAppForm);
                        var GopalCreditCardData = JsonConvert.SerializeObject(_Model);
                        var GccApplicationFlowData = JsonConvert.SerializeObject(_AppFlowModel);
                        int gId = objGopalCreditCardDBModel.InsertGCCApplicationSigned(_AppFlowModel.GccId, GopalCreditCardData, GccApplicationFlowData);


                        pdfPath = GCCApplicationSignedForm(_Model, _AppFlowModel, 2);
                        GccDocumentsModel docAppForm2 = new GccDocumentsModel();
                        docAppForm2.DocumentURL = pdfPath;
                        docAppForm2.DocId = 8;
                        docAppForm2.GccId = _AppFlowModel.GccId;
                        _AppFlowModel.DocId = objGopalCreditCardDBModel.InsertGCCReportFile(docAppForm2);

                        pdfPath = GCCApplicationSignedForm(_Model, _AppFlowModel, 3);
                        GccDocumentsModel docAppForm3 = new GccDocumentsModel();
                        docAppForm3.DocumentURL = pdfPath;
                        docAppForm3.DocId = 9;
                        docAppForm3.GccId = _AppFlowModel.GccId;
                        _AppFlowModel.DocId = objGopalCreditCardDBModel.InsertGCCReportFile(docAppForm3);
                    }

                    result = _Modal.AddApplicationFlow_V1(_AppFlowModel);
                    if (result > 0)
                    {
                        if (!Request.Url.Host.Contains("localhost") && !Request.Url.Host.Contains("devitsandbox"))
                        {
                            SendSms(_AppFlowModel.StatusId, _Model.MobileNo, _Model.RegistrationNo, _Model.gcc_id);
                        }
                        if (_AppFlowModel.StatusId == 19)
                        {
                            var status = _Modal.PushGCCTransactionToJanaadhar(_Model, _AppFlowModel, result);
                        }
                    }
                }
                else
                {
                    if (_AppFlowModel.StatusId == 7)
                    {
                        var resultData = GenerateMokaReportLatter(_Model, _AppFlowModel);

                        if (!string.IsNullOrWhiteSpace(resultData))
                        {
                            responseModel = JsonConvert.DeserializeObject<PacsResponseModel>(resultData);
                        }

                        if (responseModel == null)
                        {
                            return Json(new
                            {
                                Success = false,
                                Message = "No response received."
                            }, JsonRequestBehavior.AllowGet);
                        }

                        if (!responseModel.Success)
                        {
                            return Json(new
                            {
                                Success = false,
                                Result = 0,
                                Message = responseModel.Message
                            }, JsonRequestBehavior.AllowGet);
                        }

                        return Json(new
                        {
                            Success = true,
                            DocPath = responseModel.DocPath ?? "",
                            DocType = responseModel.DocType ?? "",
                            DocName = responseModel.DocName ?? "",
                            ReturnPath = responseModel.ReturnPath ?? "",
                            Location = responseModel.Location ?? "",
                            Designation = responseModel.Designation ?? ""
                        }, JsonRequestBehavior.AllowGet);
                    }

                    else if (_AppFlowModel.StatusId == 8 || _AppFlowModel.StatusId == 24)
                    {
                        result = _Modal.AddApplicationFlow_V1(_AppFlowModel);
                    }
                    else
                    {
                        result = _Modal.AddApplicationFlow_V1(_AppFlowModel);
                    }

                    if (result > 0 && (_AppFlowModel.StatusId == 16 || _AppFlowModel.StatusId == 17 || _AppFlowModel.StatusId == 13 || _AppFlowModel.StatusId == 19 || _AppFlowModel.StatusId == 4))
                    {
                        SendSms(_AppFlowModel.StatusId, _Model.MobileNo, _Model.RegistrationNo, _Model.gcc_id);
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.GopalCreditCardController", FunctionName = "GccApplicationFlowStatus(GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel=null)" }, ex);
                return null;
            }
            return Json(result);
        }


        [HttpGet]
        [ActionName("GenerateMokaReportLatter")]
        public string GenerateMokaReportLatter(GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel = null)
        {
            try
            {
                string wkhtmlPath = Server.MapPath("~/bin/wkhtmltopdf.exe");
                string fontPath = new Uri(Server.MapPath("~/fonts/lohit-Devanagari.ttf")).AbsoluteUri;
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {
                    StringBuilder strbld = new StringBuilder();
                    var file = System.IO.Path.Combine(Server.MapPath("~/Template"), "Moka_report.html");


                    strbld.Append(System.IO.File.ReadAllText(file));
                    strbld.Replace("#RegistrationNo#", _Model.RegistrationNo)
                    .Replace("#PACSName_ENG#", _Model.PACSName_ENG)
                    .Replace("#Date#", DateTime.Now.ToString("dd-MM-yyyy"))
                    .Replace("#Name#", _Model.Name_ENG)
                    .Replace("#FatherName#", _Model.FatherName_ENG)
                    .Replace("#Address#", $"{_Model.HouseBuildingNo}, {_Model.LaneStreetName}, {_Model.Address_ENG}, {_Model.PinCode}, {_Model.Village_ENG}, {_Model.DistrictName_ENG}")
                    .Replace("#ApplicantRuralResidence#", _Model.IsApplicantRuralResidence == true ? "हां" : "नहीं")
                    .Replace("#DCSName#", _Model.DCSName)
                    .Replace("#LoanAmountRecommendedByDCS#", NumberToHindiWordConverter.FormatDecimalAmount(Convert.ToDecimal(_Model.LoanAmountRecommendedByDCS)))
                    .Replace("#NoOfbuffaloesOnMoka#", _Model.NoOfbuffaloesOnMoka?.ToString() ?? "0")
                    .Replace("#NoOfCowsOnMoka#", _Model.NoOfCowsOnMoka?.ToString() ?? "0")
                    .Replace("#LandValueOnMoka#", _Model.LandValueOnMoka?.ToString() ?? "N/A")
                    .Replace("#Guarantor1Name#", _Model.GuarantorDetailsList[0].Name)
                    .Replace("#Guarantor1FatherName#", _Model.GuarantorDetailsList[0].FatherSpouseName)
                    .Replace("#Guarantor1Address#", _Model.GuarantorDetailsList[0].Address)
                    .Replace("#Guarantor1LandProperty#", (_Model.GuarantorDetailsList[0].GuarranterLandProperty == "0" ? "N/A" : _Model.GuarantorDetailsList[0].GuarranterLandProperty))
                    .Replace("#Guarantor2Name#", _Model.GuarantorDetailsList[1].Name)
                    .Replace("#Guarantor2FatherName#", _Model.GuarantorDetailsList[1].FatherSpouseName)
                    .Replace("#Guarantor2Address#", _Model.GuarantorDetailsList[1].Address)
                    .Replace("#Guarantor2LandProperty#", (_Model.GuarantorDetailsList[1].GuarranterLandProperty == "0" ? "N/A" : _Model.GuarantorDetailsList[1].GuarranterLandProperty))
                    .Replace("#AgricultureLandIrrigated#", (_Model.AgricultureLandDetailList?.Count ?? 0) > 0 ? _Model.AgricultureLandDetailList[0].Irrigated.ToString() : "N/A")
                    .Replace("#AgricultureLandNonIrrigated#", (_Model.AgricultureLandDetailList?.Count ?? 0) > 0 ? _Model.AgricultureLandDetailList[0].NonIrrigated.ToString() : "N/A")
                    .Replace("#MokaRemark#", _Model.RemarkMoka);

                    string finalHtml = $@"
                   <html>
                    <head>
                        <meta charset='UTF-8'>
                        <style>
                            @font-face {{
                                font-family: 'lohit-Devanagari';
                                src: url('{fontPath}') format('truetype');
                            }}
                            body {{
                                font-family: 'lohit-Devanagari';
                                margin: 20px;
                            }}
                        </style>
                    </head>
                    <body>
                        {strbld}
                    </body>
                   </html>";


                    var pdf = PDFgenerator.GeneratePdfFromHtml(finalHtml, wkhtmlPath);
                    var fileName = $"MokaRpt{Guid.NewGuid().ToString("N").ToUpper()}.pdf";

                    string cleanRegNo = _Model.RegistrationNo.Replace("/", "").Replace(".", "").Replace("_", "").Replace("-", "").Replace(" ", "");

                    string folderPath = Path.Combine(Server.MapPath("~/Content/GopalCreditCard/GSSProfiling/"), cleanRegNo);


                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    string filePath = Path.Combine(folderPath, fileName);
                    System.IO.File.WriteAllBytes(filePath, pdf);

                    if (_AppFlowModel.DocumentURL != null && _AppFlowModel.DocumentURL != "")
                    {
                        //pdfPath += "#" + _AppFlowModel.DocumentURL;
                        return JsonConvert.SerializeObject(new
                        {
                            Success = true,
                            DocPath = folderPath + _AppFlowModel.DocumentURL,
                            DocType = "MokaReport Certificate",
                            DocName = _AppFlowModel.DocumentURL,
                            ReturnPath = Url.Action("ApplicationFlowGCC", "GopalCreditCard"),
                            Location = _Model.PACSName_ENG,
                            Designation = "Pacs Manager"
                        });
                    }
                    return JsonConvert.SerializeObject(new
                    {
                        Success = true,
                        DocPath = filePath,
                        DocType = "MokaReport Certificate",
                        DocName = fileName,
                        ReturnPath = Url.Action("ApplicationFlowGCC", "GopalCreditCard"),
                        Location = _Model.PACSName_ENG,
                        Designation = "Pacs Manager"
                    });
                }
                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    Location = "",
                    Designation = ""
                });
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "GopalCreditCardController", FunctionName = "GenerateMokaReportLatter(GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel = null)" }, ex);
                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    Location = "",
                    Designation = ""
                });
            }
        }

        [HttpGet]
        [ActionName("GenerateReSanctionLatter")]
        public string GenerateReSanctionLatter(GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel = null)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {
                    int TotalInstallment = 12;
                    decimal LateInterestRate = 2;
                    decimal InterestRate = 10.25m;
                    decimal TotalInterestRate = LateInterestRate + InterestRate;


                    StringBuilder strbld = new StringBuilder();
                    var file = System.IO.Path.Combine(Server.MapPath("~/Template"), "Gcc_resanction_letter_format.html");


                    strbld.Append(System.IO.File.ReadAllText(file));
                    strbld.Replace("#Bank#", _Model.LoanBankName_HI)
                   .Replace("#Branch#", _Model.LoanBranchName_HI)
                   .Replace("#UserName#", _LoggedInUser.OfficerNameEn);

                    if (_Model.DCSName != null && ContainsUnicodeCharacter(_Model.PACSName_ENG))
                    {
                        strbld.Replace("#SocietyName#", _Model.DCSName);
                    }

                    strbld.Replace("#SocietyRegistrationNo#", _Model.RegistrationNo)
                        .Replace("#SanctionDate#", DateTime.Now.ToString("dd-MM-yyyy"))
                        .Replace("#LoaneeName#", UnicodeToKrutidev.GetKrutidev(_Model.Name_HI))
                        .Replace("#LoaneeFatherName#", UnicodeToKrutidev.GetKrutidev(_Model.FatherName_HI))
                        .Replace("#LoaneeAddress#", UnicodeToKrutidev.GetKrutidev(_Model.Address_HI))
                        .Replace("#SanctionAmount#", _Model.SanctionAmt.ToString())
                        .Replace("#TotalInstallment#", TotalInstallment.ToString())
                        .Replace("#LateInterestRate#", LateInterestRate.ToString())
                        .Replace("#InterestRate#", InterestRate.ToString())
                        .Replace("#TotalInterestRate#", TotalInterestRate.ToString());


                    var gccRegNo = Server.MapPath("~/Content/GopalCreditCard/GSSProfiling/" + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", ""));
                    string devFontSize = "17px";
                    if (!System.IO.Directory.Exists(gccRegNo))
                    {
                        System.IO.Directory.CreateDirectory(gccRegNo);
                    }

                    var fileName = "ReSanctionLetter_" + Guid.NewGuid().ToString("N").ToUpper() + ".pdf";
                    var filePath = gccRegNo + "/" + fileName;

                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(strbld.ToString().Replace("<br>", "<br/>"), true, devFontSize));
                    sb1.Replace("#SocietyName#", _Model.PACSName_ENG);
                    string QRCoder = COMMON_FUNCTION.GenerateQRCode("https://rajsahakarapp.rajasthan.gov.in/GopalCreditCard/GSSProfiling/" + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", "") + "/" + fileName);
                    sb1.Replace("#QRCoder#", QRCoder);

                    GopalCreditCardDBModel _Modal = new GopalCreditCardDBModel();

                    if (!COMMON_FUNCTION.GeneratePDF(sb1, filePath))
                    {
                        return JsonConvert.SerializeObject(new
                        {
                            Success = false,
                            DocPath = "",
                            DocType = "",
                            DocName = "",
                            ReturnPath = "",
                            Location = "",
                            Designation = "",
                            Message = "Error in Generate Certificate"
                        });
                    }


                    var pdfPath = fileName;
                    if (_AppFlowModel.DocumentURL != null && _AppFlowModel.DocumentURL != "")
                    {
                        return JsonConvert.SerializeObject(new
                        {
                            Success = true,
                            DocPath = gccRegNo + "/" + _AppFlowModel.DocumentURL,
                            DocType = "ReSanction Certificate",
                            DocName = _AppFlowModel.DocumentURL,
                            ReturnPath = Url.Action("ApplicationFlowGCC", "GopalCreditCard"),
                            Location = _Model.PACSName_ENG,
                            Designation = "Pacs Manager"
                        });
                    }
                    return JsonConvert.SerializeObject(new
                    {
                        Success = true,
                        DocPath = filePath,
                        DocType = "ReSanction Certificate",
                        DocName = fileName,
                        ReturnPath = Url.Action("ApplicationFlowGCC", "GopalCreditCard"),
                        Location = _Model.PACSName_ENG,
                        Designation = "Pacs Manager"
                    });
                }
                else
                {
                    return JsonConvert.SerializeObject(new
                    {
                        Success = false,
                        DocPath = "",
                        DocType = "",
                        DocName = "",
                        ReturnPath = "",
                        Location = "",
                        Designation = ""
                    });
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "GopalCreditCardController", FunctionName = "CertificateGenerate catch " }, ex);
                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    Location = "",
                    Designation = "",
                    Message = ex.Message
                });
            }
        }


        [HttpGet]
        [ActionName("GenerateCertificate")]
        public string GenerateCertificate(GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel = null)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {
                    int TotalInstallment = 12;
                    decimal LateInterestRate = 2;
                    decimal InterestRate = 10.25m;
                    decimal TotalInterestRate = LateInterestRate + InterestRate;


                    StringBuilder strbld = new StringBuilder();
                    var file = System.IO.Path.Combine(Server.MapPath("~/Template"), "Gcc_sanction_letter_format.html");


                    strbld.Append(System.IO.File.ReadAllText(file));
                    strbld.Replace("#Bank#", _Model.LoanBankName_HI)
                   .Replace("#Branch#", _Model.LoanBranchName_HI)
                   .Replace("#UserName#", _LoggedInUser.OfficerNameEn);

                    if (_Model.DCSName != null && ContainsUnicodeCharacter(_Model.PACSName_ENG))
                    {
                        strbld.Replace("#SocietyName#", _Model.DCSName);
                    }


                    strbld.Replace("#SocietyRegistrationNo#", _Model.RegistrationNo)
                        .Replace("#SanctionDate#", DateTime.Now.ToString("dd-MM-yyyy"))
                        .Replace("#LoaneeName#", UnicodeToKrutidev.GetKrutidev(_Model.Name_HI))
                        .Replace("#LoaneeFatherName#", UnicodeToKrutidev.GetKrutidev(_Model.FatherName_HI))
                        .Replace("#LoaneeAddress#", UnicodeToKrutidev.GetKrutidev(_Model.Address_HI))
                        .Replace("#SanctionAmount#", _Model.SanctionAmt.ToString())
                        .Replace("#TotalInstallment#", TotalInstallment.ToString())
                        .Replace("#LateInterestRate#", LateInterestRate.ToString())
                        .Replace("#InterestRate#", InterestRate.ToString())
                        .Replace("#TotalInterestRate#", TotalInterestRate.ToString());


                    var gccRegNo = Server.MapPath("~/Content/GopalCreditCard/GSSProfiling/" + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", ""));

                    string devFontSize = "17px";

                    if (!System.IO.Directory.Exists(gccRegNo))
                    {
                        System.IO.Directory.CreateDirectory(gccRegNo);
                    }

                    var fileName = "SanctionLetter_" + Guid.NewGuid().ToString("N").ToUpper() + ".pdf";
                    var filePath = gccRegNo + "/" + fileName;

                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.Append(UnicodeToKrutidev.FindAndReplaceKrutidev(strbld.ToString().Replace("<br>", "<br/>"), true, devFontSize));
                    sb1.Replace("#SocietyName#", _Model.PACSName_ENG);
                    string QRCoder = COMMON_FUNCTION.GenerateQRCode("https://rajsahakarapp.rajasthan.gov.in/GopalCreditCard/GSSProfiling/" + _Model.RegistrationNo.ToString().Replace("/", "").Replace("-", "").Replace("_", "") + "/" + fileName);
                    sb1.Replace("#QRCoder#", QRCoder);
                    GopalCreditCardDBModel _Modal = new GopalCreditCardDBModel();

                    if (!COMMON_FUNCTION.GeneratePDF(sb1, filePath))
                    {
                        return JsonConvert.SerializeObject(new
                        {
                            Success = false,
                            DocPath = "",
                            DocType = "",
                            DocName = "",
                            ReturnPath = "",
                            Location = "",
                            Designation = "",
                            Message = "Error in Generate Certificate"
                        });
                    }

                    var pdfPath = fileName;
                    if (_AppFlowModel.DocumentURL != null && _AppFlowModel.DocumentURL != "")
                    {
                        return JsonConvert.SerializeObject(new
                        {
                            Success = true,
                            DocPath = gccRegNo + "/" + pdfPath,
                            DocType = "GCC Certificate",
                            DocName = fileName,
                            ReturnPath = Url.Action("ApplicationFlowGCC", "GopalCreditCard"),
                            Location = _Model.PACSName_ENG,
                            Designation = "Pacs Manager"
                        });
                    }
                    return JsonConvert.SerializeObject(new
                    {
                        Success = true,
                        DocPath = filePath,
                        DocType = "GCC Certificate",
                        DocName = fileName,
                        ReturnPath = Url.Action("ApplicationFlowGCC", "GopalCreditCard"),
                        Location = _Model.PACSName_ENG,
                        Designation = "Pacs Manager"
                    });
                }
                else
                {
                    return JsonConvert.SerializeObject(new
                    {
                        Success = false,
                        DocPath = "",
                        DocType = "",
                        DocName = "",
                        ReturnPath = "",
                        Location = "",
                        Designation = ""
                    });
                }

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "GopalCreditCardController", FunctionName = "CertificateGenerate catch " }, ex);
                return JsonConvert.SerializeObject(new
                {
                    Success = false,
                    DocPath = "",
                    DocType = "",
                    DocName = "",
                    ReturnPath = "",
                    Location = "",
                    Designation = "",
                    Message = ex.Message
                });
            }
        }



        [HttpPost]
        public async Task<JsonResult> GCCStartEsign(string DocType, string docid, string SocietyIDEnc = "", string DocumentPath = "", string DocumentName = "", string ReturnPath = "", string Remark = "", int? StatusId = null, bool IsAmendment = false, string AmendmendAmount = "", string ReceiptNumber = "", GopalCreditCardFormModel _Model = null, GccApplicationFlowModel _AppFlowModel = null, string Location = "", string Designation = "")
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            EsignModel eSignModel = new EsignModel(new DBLogger());
            var GopalCreditCardData = JsonConvert.SerializeObject(_Model);
            var GccApplicationFlowData = JsonConvert.SerializeObject(_AppFlowModel);

            string UAT_BaseUrl = "http://localhost:27466/eSign/EspResponse";
            string Prod_BaseUrl = "https://rajsahakarapp.rajasthan.gov.in/eSign/EspResponse";

            try
            {
                if (_LoggedInUser != null)
                {
                    string responseUrl = ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")) ? UAT_BaseUrl : Prod_BaseUrl;
                    string pdfPath = "";
                    switch (DocType)
                    {
                        case "NOC Certificate":
                            pdfPath = DocumentPath;
                            break;
                        case "MokaReport Certificate":
                            pdfPath = DocumentPath;
                            break;
                        case "ReSanction Certificate":
                            pdfPath = DocumentPath;
                            break;
                        case "GCC Certificate":
                            pdfPath = DocumentPath;
                            break;
                        default:
                            pdfPath = Server.MapPath(DocumentPath);
                            break;
                    }

                    string prn = eSignController.GeneratePRN();
                    string baseUrl = ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")) ? "http://localhost:27466/" : "https://rajsahakarapp.rajasthan.gov.in/";
                    ReturnPath = baseUrl + ReturnPath;
                    var existing = eSignModel.GetTransactionByPRN(prn);
                    if (existing.Rows.Count > 0)
                    {
                        return Json(new { success = false, message = "Transaction already exists." });
                    }

                    // INSERT TRANSACTION
                    eSignModel.InsertEsignTransaction(prn, pdfPath, _LoggedInUser.SSOID, _LoggedInUser.UserId, _LoggedInUser.RoleId, "INITIATED", DocType, ReturnPath, docid, SocietyIDEnc, Remark, DocumentName, StatusId, IsAmendment, AmendmendAmount, ReceiptNumber, GopalCreditCardData, "0", "0", "0", _LoggedInUser.DistrictId, "0", "0", "0", "0", "0", "0", _LoggedInUser.UserRoleDeptMappingId, GccApplicationFlowData);

                    SignedXmlResponse signedXmlResp;
                    signedXmlResp = await eSignModel.GenerateSignedXmlAsync(pdfPath, prn, responseUrl, false, DocType, Location, Designation);

                    // LOG XML GENERATION API
                    eSignModel.InsertEsignApiLog(prn, "XML_GENERATE", "PDFPath: " + pdfPath, JsonConvert.SerializeObject(signedXmlResp), signedXmlResp?.data?.responseCode, signedXmlResp?.data?.responseMsg, signedXmlResp?.status);

                    if (signedXmlResp != null && signedXmlResp.status == "SUCCESS")
                    {
                        // UPDATE STATUS
                        eSignModel.UpdateEsignStatus(prn, "XML_GENERATED", signedXmlResp.data.txnId);

                        return Json(new
                        {
                            success = true,
                            xml = Encoding.UTF8.GetString(Convert.FromBase64String(signedXmlResp.data.signedXMLData))
                        });
                    }
                    else
                    {
                        eSignModel.UpdateEsignStatus(prn, "FAILED", null);

                        return Json(new
                        {
                            success = false,
                            message = "XML Generation Failed"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }

                return Json(new { success = false, message = "Session out !!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }

    public class GopalCreditCardFormModelList
    {
        public List<GopalCreditCardFormModel> GopalCreditCardFormModel { get; set; }
    }

    public class TransferGccRequest
    {
        public int UserId { get; set; }
        public int OldUserId { get; set; }
        public string GccIDS { get; set; }
    }
}