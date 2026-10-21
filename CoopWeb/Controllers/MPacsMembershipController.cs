using Cooperative.Filters;
using Cooperative.Models;
using java.nio.file;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using static Cooperative.Models.GopalCreditCardModel;


namespace Cooperative.Controllers
{
    [CustomAuthentication]
    [LogActionFilter]
    public class MPacsMembershipController : BaseController
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

        public ActionResult JanAadharApiTest()
        {
            return View();
        }
        public ActionResult MPacsMembershipApplicationForm()
        {
            return View();
        }

        public ActionResult MPacsMembershipApplicationData()
        {
            return View();
        }
        public ActionResult MPacsMembershipApplicationActionData()
        {
            return View();
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
                    //DT = objGopalCreditCardDBModel.CheckMPacsRegistrationStatus(JanMemberIds, COMMON_FUNCTION.Encrypt(JanAadharNo));

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

                        //if (DT.Rows.Count == 0 || (DT.Rows.Count > 0 && DT.Columns.Contains("JanAadharMIDList") && !DT.Rows[0]["JanAadharMIDList"].ToString().Contains(x.JAN_MEMBER_ID)))
                        //{
                        //}                       
                        familyDataModel.Add(model);

                    });
                    Session["MPACSJanAadharFamilyDataModel"] = familyDataModel;

                    var json = new JavaScriptSerializer().Serialize(familyDataModel);

                    return json;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.MPacsMembershipController", FunctionName = " GetJanAadhaarMembersList(string JanAadharNo)" + JanAadharNo }, ex);
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
                //if (Request.Url.Host.Contains("localhost") || Request.Url.Host.Contains("devitsandbox"))
                //{
                //    var responseString = "<root><status>true</status><tid>168232615</tid></root>";
                //    data = COMMON_FUNCTION.ConvertXMLResponceToDataSet(responseString);
                //}
                //else
                //{
                data = COMMON_FUNCTION.SendJanAadharOTP(_Model.Encrypted_JAN_AADHAR, _Model.Encrypted_JAN_MEMBER_ID);
                //}
                if (data != null && data.Tables.Count != 0 && data.Tables[0].Rows.Count != 0)
                {
                    if (data.Tables[0].Rows[0]["status"].ToString() != "true")
                    {
                        if (data.Tables[0].Rows[0]["status"].ToString() == "false")
                        {
                            return "";
                        }
                        else
                        {
                            return "";
                        }
                        //return Session["txn"].ToString();
                    }
                    else
                    {

                        string tid = data.Tables[0].Rows[0]["tid"].ToString();
                        return tid;
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.MPacsMembershipController", FunctionName = " GenrateJanAadharOTP(JanAadharModel.JanAaadharFamilyMemberModel _Model)" }, ex);
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
                //if (Request.Url.Host.Contains("localhost") || Request.Url.Host.Contains("devitsandbox"))
                //{
                //    var responseString = "<root><tid>169621548</tid><isvalidate>true</isvalidate></root>";
                //    data = COMMON_FUNCTION.ConvertXMLResponceToDataSet(responseString);
                //}
                //else
                //{
                data = COMMON_FUNCTION.ValidateJanAadharOTP(model.JanAadharOTP, model.tid);
                // }
                if (data != null)
                {
                    if (data.Tables[0].Rows[0]["isvalidate"].ToString() == "false")
                    {
                        operationResult.Status = false;
                        var json = new JavaScriptSerializer().Serialize(operationResult);
                        return "";
                    }
                    else
                    {
                        var dataList = COMMON_FUNCTION.GetJanAadharMembersDetail(COMMON_FUNCTION.Decrypt(model.Encrypted_JAN_AADHAR), model.ENR_ID, COMMON_FUNCTION.Decrypt(model.Encrypted_JAN_AADHAR), COMMON_FUNCTION.Decrypt(model.Encrypted_JAN_MEMBER_ID));

                        var mPacsMember = new MPacsMemberShipRegistrationJanaadharModel();

                        if (dataList.Status.ToUpper() == "OK")
                        {
                            mPacsMember.Status = true;
                            mPacsMember.JanAadharNo = dataList.UserDetails.JanAadharNo;
                            mPacsMember.Encrypted_JanAadharNo = dataList.UserDetails.Encrypted_JanAadharNo;
                            mPacsMember.Encrypted_JanAadharMID = dataList.UserDetails.Encrypted_JanAadharMID;
                            mPacsMember.JanAadharEnrollmentId = dataList.UserDetails.JanAadharEnrollmentId;
                            mPacsMember.Name = dataList.UserDetails.Name_ENG;
                            mPacsMember.FatherName = dataList.UserDetails.FatherName_ENG;
                            mPacsMember.MotherName = dataList.UserDetails.MotherName_ENG;
                            mPacsMember.SpouseName = dataList.UserDetails.SpouseName_ENG;
                            mPacsMember.DOB = dataList.UserDetails.DOB;
                            mPacsMember.Age = Convert.ToInt32(dataList.UserDetails.Age);
                            mPacsMember.Gender = dataList.UserDetails.Gender;
                            mPacsMember.MobileNo = dataList.UserDetails.MobileNo;
                            mPacsMember.Category = dataList.UserDetails.Category;
                            mPacsMember.Address = dataList.UserDetails.Address.Address_ENG;
                            mPacsMember.PinCode = dataList.UserDetails.Address.PinCode;
                            mPacsMember.District_Code = dataList.UserDetails.Address.District_Code;

                            mPacsMember.IsRural = dataList.UserDetails.Address.IsRural;
                            if (dataList.UserDetails.Address.IsRural == true)
                            {
                                mPacsMember.Block_Code = dataList.UserDetails.Address.Block_Code;
                                mPacsMember.GP_Code = dataList.UserDetails.Address.GP_Code;
                                mPacsMember.Village_Code = dataList.UserDetails.Address.Village_Code;
                            }
                            else
                            {
                                mPacsMember.City_Code = dataList.UserDetails.Address.City_Code;
                                mPacsMember.Ward_Code = dataList.UserDetails.Address.Ward_Code;
                            }

                            mPacsMember.Photo_Base64 = dataList.UserDetails.MemberPhoto_Base64;
                        }


                        var json = new JavaScriptSerializer().Serialize(mPacsMember);
                        return json;
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.MPacsMembershipController", FunctionName = " ValidateJanAadharOTP(JanAadharModel.JanAaadharFamilyMemberModel model)" }, ex);
                return null;
            }
            return "";
        }

        [HttpPost]
        public JsonResult SaveMPacsMembershipData(MPacsMemberShipRegistrationModel mPacsMember)
        {
            try
            {
                #region Save Member base 64 file
                string subPath = "/Content/MPacsMembership/";
                string dirPath = Server.MapPath(subPath);
                bool exists = System.IO.Directory.Exists(dirPath);
                if (!exists)
                    System.IO.Directory.CreateDirectory(dirPath);

                var fileName = "mPacsMem_" + System.DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_" + Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + ".jpg";

                string finalFilePath = dirPath + fileName;


                FileStream fs = new FileStream(finalFilePath, FileMode.CreateNew, FileAccess.Write);
                byte[] imageBytes = Convert.FromBase64String(mPacsMember.Photo_Base64);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                ms.WriteTo(fs);
                fs.Close();
                fs.Dispose();
                ms.Flush();

                mPacsMember.PhotoPath = fileName;
                #endregion

                mPacsMember.JanAadharMID = COMMON_FUNCTION.Decrypt(mPacsMember.Encrypted_JanAadharMID);
                mPacsMember.SSOID = _LoggedInUser.SSOID;
                mPacsMember.UserType = _LoggedInUser.UserType;
                mPacsMember.KIOSKCODE = _LoggedInUser.KIOSKCODE;
                mPacsMember.CreatedBy = _LoggedInUser.UserId;
                var mPacsMemberShip = new MPacsMemberShipRegistrationDBModel(new DBLogger());
                var result = mPacsMemberShip.SaveMPacsMembershipData(mPacsMember);
                return Json(new { Result = result, Message = "Success", Status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.MPacsMembershipController", FunctionName = "SaveMPacsMembershipData(MPacsMemberShipRegistrationModel mPacsMember)" }, ex);
            }
            return Json(new { Result = "", Message = "Error while submitting request.", Status = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetMPacsApplicationDetailBySSO(string searchText = "")
        {
            try
            {
                var mPacsMemberShip = new MPacsMemberShipRegistrationDBModel(new DBLogger());
                var dataSet = mPacsMemberShip.GetMPacsApplicationDetailBySSO(searchText);

                return Json(new { Result = COMMON_FUNCTION.DataTableToJSONWithJSONNet(dataSet.Tables[0]), Message = "Success", Status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.MPacsMembershipController", FunctionName = "GetMPacsApplicationDetailBySSO(string searchText)" }, ex);
            }
            return Json(new { Result = "", Message = "Error while getting data.", Status = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetMPacsApplicationDetailByUser(DataPagingModel Details, string searchText = "", int districtId = 0, int bankId = 0, int branchId = 0, string StatusType = "PS")
        {
            try
            {
                var mPacsMemberShip = new MPacsMemberShipRegistrationDBModel(new DBLogger());
                var dataSet = mPacsMemberShip.GetMPacsApplicationDetailByUser(ref Details, searchText, districtId, bankId, branchId, StatusType);

                return Json(new { Result = COMMON_FUNCTION.DataTableToJSONWithJSONNet(dataSet.Tables[0]), Message = "Success", Status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.MPacsMembershipController", FunctionName = "GetMPacsApplicationDetailByUser(string searchText, int districtId = 0, int bankId = 0, int branchId = 0)" }, ex);
            }
            return Json(new { Result = "", Message = "Error while getting data.", Status = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMPacsApplicationDetailByEncId(string encId = "")
        {
            try
            {
                var mPacsMemberShip = new MPacsMemberShipRegistrationDBModel(new DBLogger());
                var dataSet = mPacsMemberShip.GetMPacsApplicationDetailByEncId(encId);

                return Json(new { Result = COMMON_FUNCTION.DataTableToJSONWithJSONNet(dataSet.Tables[0]), Message = "Success", Status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.MPacsMembershipController", FunctionName = "GetMPacsApplicationDetailByEncId(string encId)" }, ex);
            }
            return Json(new { Result = "", Message = "Error while getting data.", Status = false }, JsonRequestBehavior.AllowGet);
        }


        #region madhuri
        [HttpPost]
        [ActionName("MembershipProceesStatusList")]
        public string GetMembershipProceesStatusList(string StatusId, string EncId)
        {
            MPacsMemberShipRegistrationDBModel objGopalCreditCardDBModel = new MPacsMemberShipRegistrationDBModel(new DBLogger());
            int StatusIdInt;
            int.TryParse(StatusId, out StatusIdInt);
            return COMMON_FUNCTION.DataTableToJSONString(objGopalCreditCardDBModel.GetMembershipProceesStatusList(StatusIdInt, EncId));
        }
        [HttpPost]
        [ActionName("SaveMpacsApplicationFlowStatus")]
        public JsonResult SaveMpacsApplicationFlowStatus(string OTP = "", string TXN = "", MpacsApplicationFlowDataModel _AppFlowModel = null, MPacsMemberShipRegistrationModel _Model = null)
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

                MPacsMemberShipRegistrationDBModel _Modal = new MPacsMemberShipRegistrationDBModel();
                var mPacsMemberShip = new MPacsMemberShipRegistrationDBModel(new DBLogger());
                var resultData = COMMON_FUNCTION.DataTableToJSONWithJSONNet(mPacsMemberShip.GetMPacsApplicationDetailByEncId(_AppFlowModel.EncId).Tables[0]);
                var memberData = JsonConvert.DeserializeObject<List<MPacsMemberShipRegistrationResponseModel>>(resultData).FirstOrDefault();
               
                if (!ModelState.IsValid)
                    return Json(JsonModelError.Get(ModelState.Values));

                int result = 0;

                if (TXN != "" || (Request.Url.Host.Contains("localhost") || Request.Url.Host.Contains("devitsandbox")))
                {
                    ResultMessage otpResult = new ResultMessage();

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
                        if (_AppFlowModel.StatusId == 5)
                        {
                            pdfPath = GenerateMembershipCertificate(memberData);
                            _AppFlowModel.MembershipCertificateFile = pdfPath;
                        }
                        var result1 = COMMON_FUNCTION.EsinghPDF(otpResult.data, string.Join("#", pdfPath), memberData.PacsName, "PACS Manager");

                        succmsg = result1.status;
                        result = _Modal.MpacsAddApplicationFlow(_AppFlowModel);
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
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.MpacsMembersController", FunctionName = " SaveMpacsApplicationFlowStatus(string OTP = \"\", string TXN = \"\", MpacsApplicationFlowDataModel _AppFlowModel = null, MPacsMemberShipRegistrationModel _Model = null)" }, ex);
                return null;
            }

        }


        public string GenerateMembershipCertificate(MPacsMemberShipRegistrationResponseModel _Model)
        {
            try
            {
                string wkhtmlPath = Server.MapPath("~/bin/wkhtmltopdf.exe");
                string fontPath = new Uri(Server.MapPath("~/fonts/lohit-Devanagari.ttf")).AbsoluteUri;
                string logo = new Uri(Server.MapPath("~/Content/images/Logo_1.png")).AbsoluteUri;
                string photo = new Uri(Server.MapPath($"~/Content/MPacsMembership/{_Model.PhotoPath}")).AbsoluteUri;
                if (System.Web.HttpContext.Current.Session[SessionIdentity.Admin] != null)
                {
                    StringBuilder strbld = new StringBuilder();
                    var file = System.IO.Path.Combine(Server.MapPath("~/Template"), "mpacs_membership.html");

                    strbld.Append(System.IO.File.ReadAllText(file));
                    strbld.Replace("#ReferenceNo#", _Model.ReferenceNo)
                   .Replace("#MembershipNo#", _Model.MembershipNo)
                   .Replace("#Name#", _Model.Name)
                   .Replace("#Gender#", _Model.Gender)
                   .Replace("#DOB#", _Model.DOB + "/" + _Model.Age)
                   .Replace("#FatherName#", _Model.FatherName)
                   .Replace("#Category#", _Model.Category)
                   .Replace("#MobileNo#", _Model.MobileNo)
                   .Replace("#EducationalQualification#", _Model.Qualification)
                   .Replace("#NomineeName#", _Model.NomineeName)
                   .Replace("#NomineeFatherName#", _Model.NomineeFatherName)
                   .Replace("#NomineeRelation#", _Model.NomineeRelation)
                   .Replace("#IsRural#", _Model.IsRural == true ? "Rural" : "Urban")
                   .Replace("#IsShowRural#", _Model.IsRural == true ? "table-row" : "none")
                   .Replace("#IsShowUrban#", _Model.IsRural == false ? "table-row" : "none")
                   .Replace("#HouseBuildingNo#", _Model.HouseBuildingNo)
                   .Replace("#LaneStreetName#", _Model.LaneStreetName)
                   .Replace("#Locality#", _Model.Locality)
                   .Replace("#Address#", _Model.Address)
                   .Replace("#DistrictName#", _Model.DistrictName)
                   .Replace("#BlockName#", _Model.BlockName)
                   .Replace("#GpName#", _Model.GpName)
                   .Replace("#VillageName#", _Model.VillageName)
                   .Replace("#PinCode#", _Model.PinCode)
                   .Replace("#CityName#", _Model.CityName)
                   .Replace("#WardName#", _Model.WardName)
                   .Replace("#PresentIsRural#", _Model.PresentIsRural == true ? "Rural" : "Urban")
                   .Replace("#PresentIsShowRural#", _Model.PresentIsRural == true ? "table-row" : "none")
                   .Replace("#PresentIsShowUrban#", _Model.IsRural == false ? "table-row" : "none")
                   .Replace("#PresentHouseBuildingNo#", _Model.HouseBuildingNo)
                   .Replace("#PresentLaneStreetName#", _Model.PresentLaneStreetName)
                   .Replace("#PresentLocality#", _Model.PresentLocality)
                   .Replace("#PresentAddress#", _Model.PresentAddress)
                   .Replace("#PresentDistrictName#", _Model.PresentDistrictName)
                   .Replace("#PresentBlockName#", _Model.PresentBlockName)
                   .Replace("#PresentGpName#", _Model.PresentGpName)
                   .Replace("#PresentVillageName#", _Model.PresentVillageName)
                   .Replace("#PresentPinCode#", _Model.PresentPinCode)
                   .Replace("#PresentCityName#", _Model.PresentCityName)
                   .Replace("#PresentWardName#", _Model.PresentWardName)
                   .Replace("#MPacsDistrictName#", _Model.MPacsDistrictName)
                   .Replace("#BankName#", _Model.BankName)
                   .Replace("#BranchName#", _Model.BranchName)
                   .Replace("#PacsName#", _Model.PacsName)
                   .Replace("#Logo#", logo)
                   .Replace("#Photo#", photo);


                    var filePath = Server.MapPath("~/Content/MPacsMembership/Certificate/" + _Model.ReferenceNo.ToString().Replace("/", "").Replace("-", "").Replace("_", ""));

                    if (!System.IO.Directory.Exists(filePath))
                        System.IO.Directory.CreateDirectory(filePath);

                    var fileName = "MembershipCertificate_" + Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").Replace("_", "").ToUpper() + ".pdf";
                    var fileFullPath = filePath + "/" + fileName;

                    string QRCoder = COMMON_FUNCTION.GenerateQRCode("https://rajsahakarapp.rajasthan.gov.in/MPacsMembership/Certificate/" + _Model.ReferenceNo.ToString().Replace("/", "").Replace("-", "").Replace("_", "") + "/" + fileName);
                    strbld.Replace("#QRCoder#", QRCoder);
                    string str = $@"
                        <html>
                            <head>
                            <meta charset='UTF-8'>
                            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                            <style>
                                @font-face {{
                                    font-family: 'lohit-Devanagari';
                                    src: url('{fontPath}') format('truetype');
                                }}
                                body {{
                                    font-family: 'lohit-Devanagari';
                                    margin: 0;
                                    font-weight:lighter;
                                }}
                            </style>
                        </head>
                        <body>
                            {strbld}
                        </body>
                        </html>";
                    var pdf = PDFgenerator.GeneratePdfFromHtml(str, wkhtmlPath);
                    System.IO.File.WriteAllBytes(fileFullPath, pdf);

                    var pdfPath = fileName;


                    return pdfPath;
                }
                else
                {
                    return "Fail";
                }

            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "MPacsMembershipController", FunctionName = "GenerateMembershipCertificate() " }, ex);

                return "Fail";
            }
        }

        #endregion


        public class OperationResult
        {
            public bool State { get; set; }
            public bool Status { get; set; }
            public string ErrorMessage { get; set; }
            public string SuccessMessage { get; set; }
            public string Content { get; set; }
        }

    }
}