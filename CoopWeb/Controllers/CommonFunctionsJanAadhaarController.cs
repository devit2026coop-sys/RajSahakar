using Cooperative.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static Cooperative.Models.GopalCreditCardModel;
using Cooperative.JanadhaarClasses;
using System.Linq;
using System.IO;
using WebGrease.Css.Ast;
using org.w3c.dom.ls;
using OfficeOpenXml.Drawing.Slicer.Style;


namespace Cooperative.Controllers
{
    public class CommonFunctionsJanAadhaarController : BaseController
    {
        private readonly CryptoHelper _crypto;
        private readonly HttpClient _httpClient;
        private readonly JanAadhaarGenericService _service = new JanAadhaarGenericService();


        public string PostServiceCallingMethod(string url = "", string encryptedRequestJson = "")
        {

            var responseString = "";
            responseString = Task.Run(() =>
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        var content = new StringContent(encryptedRequestJson, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();
                        response.EnsureSuccessStatusCode();
                        return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    }
                    catch (Exception iex)
                    {
                        ILogger objILogger = new DBLogger();
                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.CommonFunctionJanaadhaar", FunctionName = "PostServiceCallingMehod" }, iex);
                        return iex.Message;
                    }
                }
            }).Result;


            return responseString;
        }


        public CommonFunctionsJanAadhaarController()
        {
            //_service = new JanAadhaarGenericService();
        }

        [HttpPost]
        public async Task<string> FetchMemberList(string JanaadhaarNo, string SchemeName)
        {
            try
            {
                JanaadhaarNo = csCommon.Decrypt_SA(Uri.UnescapeDataString(JanaadhaarNo));
                string DecResponseString = await _service.getResponseData(SchemeName, "FetchMemberList", JanaadhaarNo, "", "", "");

                RootResponse parsedResponse = JsonConvert.DeserializeObject<RootResponse>(DecResponseString);
                List<JanAaadharFamilyMemberDetailsModel> resultData = new List<JanAaadharFamilyMemberDetailsModel>();
                List<JanAaadharFamilyMemberDetailsModel> memberList = new List<JanAaadharFamilyMemberDetailsModel>();
                DataTable DT = null;
                GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());

                if (parsedResponse != null && parsedResponse.response.status)
                {
                    string result = JsonConvert.SerializeObject(parsedResponse.response.data);
                    resultData = JsonConvert.DeserializeObject<List<JanAaadharFamilyMemberDetailsModel>>(result);
                    var JanMemberIds = "";
                    string JanAadharNo = "";

                    if (SchemeName == "GCC")
                    {
                        foreach (var item in resultData)
                        {
                            JanMemberIds += "," + item.MEMBER_ID;
                        }
                        DT = objGopalCreditCardDBModel.CheckGCCRegistrationStatus(JanMemberIds, COMMON_FUNCTION.Encrypt(JanAadharNo));
                    }

                    resultData.ForEach(x =>
                    {
                        x.Masked_JAN_MEMBER_ID = !String.IsNullOrEmpty(x.MEMBER_ID) == true ? COMMON_FUNCTION.Masked(x.MEMBER_ID) : "";
                        x.Encrypted_JAN_MEMBER_ID = !String.IsNullOrEmpty(x.MEMBER_ID) == true ? COMMON_FUNCTION.Encrypt(x.MEMBER_ID) : "";
                        x.MEMBERID = !String.IsNullOrEmpty(x.MEMBER_ID) == true ? x.MEMBER_ID : "";
                        x.NAME_EN = x.NAME_EN;
                        x.NAME_HINDI = x.NAME_LL;
                        x.NAME_LL = x.NAME_LL;
                        x.Masked_MOBILE_NO = !String.IsNullOrEmpty(x.MOBILE_NO) == true ? COMMON_FUNCTION.Masked(x.MOBILE_NO) : "";
                        if (DT == null || DT.Rows.Count == 0 || (DT.Rows.Count > 0 && DT.Columns.Contains("JanAadharMIDList") && !DT.Rows[0]["JanAadharMIDList"].ToString().Contains(x.MEMBER_ID)))
                        {
                            memberList.Add(x);
                        }
                    });
                    Session["JanAadharFamilyDataModel"] = memberList;
                    string resultjson = new JavaScriptSerializer().Serialize(resultData);
                    return resultjson;

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.CommonFunctionJanaadhaar",
                    FunctionName = "FetchMemberList"
                }, ex);
                return ex.Message;
            }

        }
        public async Task<string> GenerateOTP(string memberId, string SchemeName)
        {
            try
            {
                if (memberId.Contains("Encrypt:"))
                    memberId = csCommon.Decrypt_SA(Uri.UnescapeDataString(memberId));
                else if (!System.Text.RegularExpressions.Regex.IsMatch(memberId, @"^\d+$"))
                    memberId = COMMON_FUNCTION.Decrypt(Uri.UnescapeDataString(memberId));
                string result = await _service.GenerateOTP(memberId, SchemeName);
                return result;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.CommonFunctionJanaadhaar",
                    FunctionName = "GenerateOTP"
                }, ex);
                return ex.Message;
            }

        }
        public async Task<string> ValidateOTP_FetchRequestedData(string memberId, string SchemeName, string tid, string otp, string SocietyDirectory = "", string janAadharNo = "", string typeId = "0")
        {
            try
            {
                if (memberId.Contains("Encrypt:"))
                    memberId = csCommon.Decrypt_SA(Uri.UnescapeDataString(memberId));
                else if (!System.Text.RegularExpressions.Regex.IsMatch(memberId, @"^\d+$"))
                    memberId = COMMON_FUNCTION.Decrypt(Uri.UnescapeDataString(memberId));
                if (janAadharNo != null && janAadharNo != "")
                    janAadharNo = csCommon.Decrypt_SA(Uri.UnescapeDataString(janAadharNo));
                string DecResponseString = "";
                if (Request.Url.Host.Contains("localhost"))
                {
                    DecResponseString = "{\"response\":{\"status\":true,\"message\":\"OTP validated successfully\",\"responseCode\":\"JAN_200\",\"transactionId\":\"GCC20260710557610\",\"schemeCode\":\"RAJSAHAKAR\",\"appCode\":\"JAN6268201\",\"tid\":\"58227\",\"data\":[{\"NAME_EN\":\"GOVINDA KARYANI\",\"NAME_LL\":\"गोविंदा कारयानी\",\"MEM_TYPE\":\"MEM\",\"SRDR_MID\":78396255291,\"IS_DEATH\":\"N\",\"FATHER_NAME_EN\":\"HARISH KARYANI\",\"DOB\":\"25/06/1989\",\"MOTHER_NAME_EN\":\"KAVITA KARYANI\",\"MOTHER_NAME_LL\":\"कविता कारयानी\",\"CASTE_ID\":null,\"CAST\":null,\"CATEGORY_ID\":\"1\",\"CATEGORY_DESC_LL\":\"सामान्य\",\"GENDER_ID\":\"3\",\"GENDER\":\"MALE\",\"MARITAL_STATUS_ID\":\"7\",\"MARITAL_STATUS_CODE\":\"7\",\"MARITAL_STATUS\":\"MARRIED\",\"SPOUCE_NAME_EN\":\"VANDANA SINDHI\",\"SPOUCE_NAME_LL\":\"वंदना सिंधी\",\"MOBILE_NO\":9024054214,\"EMAIL\":\"govindkaryani@gmail.com\",\"BANK\":null,\"ACCOUNT_NO\":null,\"IFSC_CODE\":null,\"REL_WITH_HOF\":\"HUSBAND\",\"PAN_NO\":null,\"OCCUPATION\":null,\"GOVT_EMP_ID\":null,\"EDUCATION\":null,\"GAS_CONNECTION\":null,\"PIN_CODE\":302027,\"RGHS_NO\":null,\"IS_RURAL\":\"N\",\"BANK_BRANCH\":null,\"AADHAR_REF_ID\":444954817858835,\"ADDRESS\":\",Ward No- 13,Jar Heritage , Jaipur ,302027,Ward No. 133,Jaipur,Jaipur,302027\",\"BLOCK_CITY\":\"JAIPUR\",\"CATEGORY_DESC_ENG\":\"GEN\",\"DISTRICT\":\"JAIPUR\",\"ENR_ID\":\"0000-1111-86507\",\"GP_WARD\":\"WARD NO. 133\",\"JAN_AADHAR\":5121734638,\"MICR\":null,\"VILLAGE_NAME\":null,\"DISTRICT_CD\":\"110\",\"BLOCK_CITY_CD\":\"C340\",\"BLOCK_CITY_ID\":340,\"GP_WARD_CD\":\"C0340W0133\",\"GP_WARD_ID\":9605,\"VILLAGE_CD\":null,\"DISABILITY_PERCENTAGE\":null,\"ADDRESS_LL\":\", जयपुर हेरिटेज, जयपुर ,302027,वार्ड न. 133,जयपुर,जयपुर,302027\",\"DISTRICT_NAME_LL\":\"जयपुर\",\"BLOCK_CITY_LL\":\"जयपुर\",\"GP_LL\":null,\"VILLAGE_LL\":null,\"CATEGORY_CODE\":15,\"AGE\":37,\"IS_DISABILITY\":null,\"ECONOMIC_GROUP\":null}],\"janId\":null},\"signature\":\"Dv57B5CR+v5CStE4GwaIEC+bsrBXv3hxWr/vKsCCQx47z1a/ZLauntqiesPL1hQuE8+qeA/d2i+DtZvU1heOsVt4NjbDZ91WedqUhw9IGx5JRB0EBJDv6IiPZ1hsDIM1nddquJ1xqc6y7L0RNlQAwR6ZqoTEh7M1Xn5xom4h1PuUtdTBEwYUZRQKkawiv7kG7npzrP3JlTY4i0dc661kolIJQbwkjTag+7yRpZBVPLcjpl6kBhyiDFOSeEdMI1ssVj/gqDpcx0iLad1ZSBXDm/e9k8xPYDRR2H/r9iOhQi/CnSKnYNThRAG1pQFkztWaUce1LoHtXz9XleIcUAS31w==\"}";

                }
                else
                {
                    DecResponseString = await _service.getResponseData(SchemeName, "ValidateOTP_FetchRequestedData", "", memberId, tid, otp);
                }


                OTPResponse parsedResponse = JsonConvert.DeserializeObject<OTPResponse>(DecResponseString);
                if (parsedResponse != null && parsedResponse.response.status && parsedResponse.response.data?.Count > 0)
                {
                    DateTime dob = DateTime.ParseExact(parsedResponse.response.data[0].DOB, "dd/MM/yyyy", null);

                    DateTime today = DateTime.Today;
                    int age = today.Year - dob.Year;

                    if (dob.Date > today.AddYears(-age))
                        age--;

                    int calc_age = age;
                    parsedResponse.response.data[0].AGE = calc_age;
                    parsedResponse.response.data[0].ADDRESS = parsedResponse.response.data[0].ADDRESS?.Replace(",", ", ");
                    parsedResponse.response.data[0].GENDER = parsedResponse.response.data[0].GENDER == "MALE" ? "Male" : parsedResponse.response.data[0].GENDER == "FEMALE" ? "Female" : "Other";
                    parsedResponse.response.data[0].Encrypted_JAN_MEMBER_ID = !String.IsNullOrEmpty(parsedResponse.response.data[0].SRDR_MID) == true ? COMMON_FUNCTION.Encrypt(parsedResponse.response.data[0].SRDR_MID) : "";
                    parsedResponse.response.data[0].Masked_JAN_MEMBER_ID = !String.IsNullOrEmpty(parsedResponse.response.data[0].SRDR_MID) == true ? COMMON_FUNCTION.Masked(parsedResponse.response.data[0].SRDR_MID) : "";
                    parsedResponse.response.data[0].Masked_MOBILE_NO = !String.IsNullOrEmpty(parsedResponse.response.data[0].MOBILE_NO) == true ? COMMON_FUNCTION.Masked(parsedResponse.response.data[0].MOBILE_NO) : "";
                    parsedResponse.response.data[0].JanAadharNo = !String.IsNullOrEmpty(janAadharNo) == true ? COMMON_FUNCTION.Masked(janAadharNo) : "";
                    parsedResponse.response.data[0].Encrypted_JanAadharNo = !String.IsNullOrEmpty(janAadharNo) == true ? COMMON_FUNCTION.Encrypt(janAadharNo) : "";
                    if (!string.IsNullOrEmpty(parsedResponse.response.data[0].PHOTO))
                    {
                        if (SchemeName == "COOP")
                        {
                            string photo = parsedResponse.response.data[0].PHOTO;
                            string fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper();
                            string path = SocietyDirectory.Length > 1 ? "../Content/SocietyProfiling/" + SocietyDirectory : "../Content/NewRegistration/";
                            fileName = COMMON_FUNCTION.GetUniqueFileName("Member_" + fileName, System.Web.HttpContext.Current.Server.MapPath(path), ".jpg").ToLower();

                            if (!System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(path.Replace("..", "~"))))
                                System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(path.Replace("..", "~")));
                            if (!System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(path.Replace("..", "~"))))
                                System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(path.Replace("..", "~")));

                            FileStream fs = new FileStream(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(path.Replace("..", "~")), fileName + ".jpg"), FileMode.CreateNew, FileAccess.Write);
                            byte[] imageBytes = Convert.FromBase64String(photo);
                            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                            ms.WriteTo(fs);
                            fs.Close();
                            fs.Dispose();
                            ms.Flush();
                            parsedResponse.response.data[0].PHOTO = SocietyDirectory.Length > 1 ? "../Content/SocietyProfiling/" + SocietyDirectory + "/" + fileName + ".jpg" : "../Content/NewRegistration/" + fileName + ".jpg";
                        }
                        else if (SchemeName == "GCCG")
                        {
                            string photo = parsedResponse.response.data[0].PHOTO;
                            string fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper();
                            string path = "../Content/GopalCreditCard/";
                            fileName = COMMON_FUNCTION.GetUniqueFileName("Gurantor_" + fileName, System.Web.HttpContext.Current.Server.MapPath(path), ".jpg").ToLower();

                            if (!System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(path.Replace("..", "~"))))
                                System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(path.Replace("..", "~")));
                            if (!System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(path.Replace("..", "~"))))
                                System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(path.Replace("..", "~")));

                            FileStream fs = new FileStream(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(path.Replace("..", "~")), fileName + ".jpg"), FileMode.CreateNew, FileAccess.Write);
                            byte[] imageBytes = Convert.FromBase64String(photo);
                            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                            ms.WriteTo(fs);
                            fs.Close();
                            fs.Dispose();
                            ms.Flush();
                            parsedResponse.response.data[0].PHOTO = "../Content/GopalCreditCard/" + fileName + ".jpg";
                        }
                    }

                    if (SchemeName == "GCC")
                    {
                        //changes by govind 10-03-2026 start
                        DataTable DT = null;
                        GopalCreditCardDBModel objGopalCreditCardDBModel = new GopalCreditCardDBModel(new DBLogger());
                        DT = objGopalCreditCardDBModel.Get_CheckExistsInMMPBY(janAadharNo);
                        if (DT.Rows.Count > 0)
                        {
                            var familyData = (List<JanAaadharFamilyMemberDetailsModel>)Session["JanAadharFamilyDataModel"];
                            var familyDataModelNew = familyData.Where(x => x.Encrypted_JAN_MEMBER_ID != parsedResponse.response.data[0].Encrypted_JAN_MEMBER_ID).ToList();
                            var familyDataModel = new List<JanAadharModel.JanAaadharFamilyMemberModel>();
                            familyDataModelNew.ForEach(x =>
                            {
                                JanAadharModel.JanAaadharFamilyMemberModel janModel = new JanAadharModel.JanAaadharFamilyMemberModel();

                                janModel.NAME = x.NAME_EN;
                                janModel.NAME_HINDI = x.NAME_HINDI;
                                janModel.GENDER = x.GENDER;
                                janModel.MEMBER_TYPE = x.MEMBER_TYPE;
                                janModel.Masked_MOBILE_NO = x.Masked_MOBILE_NO;
                                janModel.Masked_JAN_MEMBER_ID = x.Masked_JAN_MEMBER_ID;
                                janModel.Encrypted_JAN_MEMBER_ID = x.Encrypted_JAN_MEMBER_ID;

                                familyDataModel.Add(janModel);
                            });
                            List<GopalCreditCardModel.JanAaadharFamilyMemberModel> gopalFamilyDataModel = new List<GopalCreditCardModel.JanAaadharFamilyMemberModel>();

                            familyDataModel.ForEach(x =>
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
                            parsedResponse.response.data[0].janAaadharFamilyMemberList = gopalFamilyDataModel;
                        }
                        else
                        {

                            parsedResponse.response.status = false;
                            parsedResponse.response.message = "Cattle is not Insured under MMPBY";

                        }
                    }
                    parsedResponse.response.data[0].tid = parsedResponse.response.tid;
                    parsedResponse.response.data[0].transactionId = parsedResponse.response.transactionId;
                }

                string result = JsonConvert.SerializeObject(parsedResponse.response);
                return result;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.CommonFunctionJanaadhaar",
                    FunctionName = "ValidateOTP_FetchRequestedData"
                }, ex);
                return ex.Message;
            }
        }


        public async Task<JsonResult> GetJanAadahrMIDByAadhaar(string aadhaarNo)
        {
            try
            {
                aadhaarNo = csCommon.Decrypt_SA(Uri.UnescapeDataString(aadhaarNo));
                string DecResponseString = await _service.getResponseData("COOP", "JanAadaarDetailByAadhaar", aadhaarNo, "", "", "");

                JanaadharByAadharRootResponse parsedResponse = JsonConvert.DeserializeObject<JanaadharByAadharRootResponse>(DecResponseString);
                JanAadhaarDetails janAadhaarDtl = new JanAadhaarDetails();
                if (parsedResponse != null && parsedResponse.response.status)
                {
                    string result = JsonConvert.SerializeObject(parsedResponse.response.data);
                    if (parsedResponse.response.data != null && parsedResponse.response.data.Count > 0)
                        janAadhaarDtl = parsedResponse.response.data[0];
                    return Json(janAadhaarDtl, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.CommonFunctionJanaadhaar",
                    FunctionName = "GetJanAadahrMIDByAadhaar"
                }, ex);
                return null;
            }

        }
    }

    public class RequestPayload
    {
        public string appCode { get; set; }
        public string schemShortCode { get; set; }
        public string transactionId { get; set; }
        public string janId { get; set; }
    }

    public class DecryptedData
    {
        public RequestPayload data { get; set; }
        public string signature { get; set; }
    }

    public class EncryptedRequest
    {
        public string data { get; set; }
    }

    // [CustomAuthentication]

    public class Member
    {
        public string NAME_EN { get; set; }
        public string MEMBERID { get; set; }
    }

    public class ResponseData
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string responseCode { get; set; }
        public string transactionId { get; set; }
        public string tid { get; set; }
        public string schemeCode { get; set; }
        public string appCode { get; set; }
        public string JanId { get; set; }

        public List<JanAaadharFamilyMemberDetailsModel> data { get; set; }
    }

    public class RootResponse
    {
        public ResponseData response { get; set; }
        public string signature { get; set; }
    }



    public class FetchFamilyResponse
    {
        public JanAaadharFamilyMemberDetailsModel response { get; set; }
        public string signature { get; set; }
    }
    public class OTPResponse
    {
        public OTPResponseData response { get; set; }
        public string signature { get; set; }
    }
    public class OTPResponseData
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string responseCode { get; set; }
        public string transactionId { get; set; }
        public string tid { get; set; }
        public string schemeCode { get; set; }
        public string appCode { get; set; }
        public string JanId { get; set; }
        public List<JanAaadharFamilyMemberDetailsModel> data { get; set; }
    }

    public class JanAaadharFamilyMemberDetailsModel
    {
        public string NAME_EN { get; set; }
        public string NAME_LL { get; set; }
        public string MEM_TYPE { get; set; }
        public string MEMBERID { get; set; }
        public string Masked_JAN_MEMBER_ID { get; set; }
        public string Encrypted_JAN_MEMBER_ID { get; set; }

        public string DOB { get; set; }
        public int AGE { get; set; } = 0;
        public string GENDER { get; set; }

        //new columns 
        public string MEMBER_ID { get; set; }
        public string AADHAR_REF_ID { get; set; }
        public string CAST { get; set; }
        public string CATEGORY_CODE { get; set; }
        public string MARITAL_STATUS { get; set; }
        public string MEMBER_TYPE { get; set; }
        public string Masked_MOBILE_NO { get; set; }
        public string NAME_HINDI { get; set; }
        public string MOBILE_NO { get; set; }
        public string IS_RURAL { get; set; }
        public string PHOTO { get; set; }
        public string ADDRESS { get; set; }
        public string BLOCK_CITY { get; set; }
        public string PIN_CODE { get; set; }
        public string CATEGORY_DESC_ENG { get; set; }
        public string DISTRICT { get; set; }
        public string ENR_ID { get; set; }
        public string GP_WARD { get; set; }
        public string IS_MINORITY { get; set; }
        public string VILLAGE_NAME { get; set; }
        public string DISTRICT_CD { get; set; }
        public string BLOCK_CITY_CD { get; set; }
        public string GP_WARD_CD { get; set; }
        public string VILLAGE_CD { get; set; }
        public string JanAadharNo { get; set; }
        public string Encrypted_JanAadharNo { get; set; }
        public string JanAadharEnrollmentId { get; set; }

        //otp response

        public string SRDR_MID { get; set; }
        public string IS_DEATH { get; set; }
        public string FATHER_NAME_EN { get; set; }
        public string MOTHER_NAME_EN { get; set; }
        public string BANK { get; set; }
        public string EDUCATION { get; set; }
        public string GAS_CONNECTION { get; set; }
        public string transactionId { get; set; }
        public string tid { get; set; }

        public List<JanAaadharFamilyMemberModel> janAaadharFamilyMemberList { get; set; }

    }

    public class JanaadharByAadharResponseData
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string responseCode { get; set; }
        public string transactionId { get; set; }
        public string tid { get; set; }
        public string schemeCode { get; set; }
        public string appCode { get; set; }
        public string JanId { get; set; }

        public List<JanAadhaarDetails> data { get; set; }
    }
    public class JanaadharByAadharRootResponse
    {
        public JanaadharByAadharResponseData response { get; set; }
        public string signature { get; set; }
    }
    public class JanAadhaarDetails
    {
        public string JAN_AADHAR { get; set; }
        public string MEMBER_ID { get; set; }
    }
    public class DBT_Transaction
    {
        public int Id { get; set; }
        public string transactionId { get; set; }
        public string schemeCode { get; set; }
        public string entitlementId { get; set; }
        public string entitlementMemId { get; set; }
        public string janaadhaarId { get; set; }
        public string janaadhaarMemId { get; set; }
        public string transactionMode { get; set; }
        public string aadharNo { get; set; }
        public string micr { get; set; }
        public string bankAccNo { get; set; }
        public string ifsc { get; set; }
        public string paymentAmount { get; set; }
        public string paymentDate { get; set; }

    }

}