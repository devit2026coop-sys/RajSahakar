using CoopDAL;
using Cooperative.Filters;
using Cooperative.JanadhaarClasses;
using Cooperative.Models;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using static Cooperative.Models.GopalCreditCardModel;
using static javax.net.ssl.SSLEngineResult;

namespace Cooperative.Controllers
{
    public class ServicesController : ApiController
    {

        [HttpGet]
        public List<RajfedData> Mobile_GetTotalQuantityDetails(string DistrictId = "0", string Kvss = "", string purchase_center = "0", string commodityId = "", string crop_year = "2021")
        {
            try
            {
                CommonServicesModel objCitizenModel = new CommonServicesModel(new DBLogger());
                DataTable dt = (objCitizenModel.GetTotalQuantityList(DistrictId, Kvss, purchase_center, commodityId, crop_year));
                // return new JavaScriptSerializer().Serialize(dt);


                List<RajfedData> data = JsonConvert.DeserializeObject<List<RajfedData>>(COMMON_FUNCTION.DataTableToJSONString(dt));
                return data;
            }
            catch
            {
                return new List<RajfedData>();
            }
        }

        [HttpGet]
        public List<Object> Mobile_GetTotalQuantityDetails_New(string DistrictId = "0", string Kvss = "", string purchase_center = "0", string commodityId = "", string crop_year = "2024", string month = "", string date = "", string Type = "", string Agency = "")
        {
            try
            {
                CommonServicesModel objCitizenModel = new CommonServicesModel(new DBLogger());
                DataTable dt = (objCitizenModel.GetTotalQuantityListNew(DistrictId, Kvss, purchase_center, commodityId, crop_year, month, date, Type, Agency));
                // return new JavaScriptSerializer().Serialize(dt);


                List<Object> data = JsonConvert.DeserializeObject<List<Object>>(COMMON_FUNCTION.DataTableToJSONString(dt));
                // string data = JsonConvert.SerializeObject(dt);
                return data;
            }
            catch
            {
                return new List<Object>();
            }
        }

        [HttpGet]
        public List<Object> Mobile_GetWarehouseApiData(string DistrictId = "0", string Kvss = "", string purchase_center = "0", string commodityId = "", string crop_year = "2024", string month = "", string date = "", string Type = "", string Agency = "")
        {
            try
            {
                CommonServicesModel objCitizenModel = new CommonServicesModel(new DBLogger());
                DataTable dt = (objCitizenModel.GetWarehouseApiData(DistrictId, Kvss, purchase_center, commodityId, crop_year, month, date, Type, Agency));
                // return new JavaScriptSerializer().Serialize(dt);


                List<Object> data = JsonConvert.DeserializeObject<List<Object>>(COMMON_FUNCTION.DataTableToJSONString(dt));
                // string data = JsonConvert.SerializeObject(dt);
                return data;
            }
            catch
            {
                return new List<Object>();
            }
        }


        [HttpGet]
        public string GetSocietyBasicDetails(string RegistrationNo)
        {
            try
            {
                CommonServicesModel objCitizenModel = new CommonServicesModel(new DBLogger());
                DataSet dt = (objCitizenModel.GetSocietyBasicDetails(RegistrationNo));

                return COMMON_FUNCTION.DataTableToJSONWithJSONNet(dt);
            }
            catch
            {
                return "";
            }
        }
        [HttpPost]
        public APIResponce GetSocietyApplication(APIRequest Data)
        {
            var responce = new APIResponce();
            List<object> collection = new List<object>();
            try
            {
                Models.MISModel objNew = new Models.MISModel();
                DataTable DT = objNew.GetCertificateforVerify(Data.ApplicationNo.Replace(" ", ""), "API");
                foreach (DataRow dr in DT.Rows)
                {
                    collection.Add(new
                    {
                        District = dr["District"].ToString(),
                        RuralUrban = dr["RuralUrban"].ToString(),
                        RegistrationNo = dr["ApplicationNo"].ToString(),
                        ApplicantName = dr["CEO"].ToString(),
                        CuurentStatus = dr["Status"].ToString(),
                        StatusChangeDate = dr["ActionDate"].ToString(),
                        CITYNAME = dr["CITY_ENG"].ToString(),
                        WARDNAME = dr["WARD_ENG"].ToString(),
                        VILLAGE = dr["VILLAGE_ENG"].ToString(),
                        GP_ENG = dr["GP_ENG"].ToString(),

                    });
                }
                if (DT.Rows.Count > 0)
                {
                    responce.Status = "200";
                    responce.Message = "Success";
                    responce.Data = collection;
                    //responce.Data = JsonSerializer.Serialize(collection);
                    //var json = JsonConvert.SerializeObject(collection);
                    //responce.Data = 
                }

                else
                {
                    responce.Status = "0";
                    responce.Message = "Data Not Found";
                }
            }
            catch (Exception e)
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return responce;

        }
        [HttpGet]
        public APIResponce GetSocietyDistWiseData(string ActId = "0", int SocietyTypeId = 0, int DIVISION_ID = 0, int IsOnline = -1, int DistrictId = 0)
        {
            var responce = new APIResponce();
            List<object> collection = new List<object>();
            try
            {
                MISModel objMISModel = new MISModel(new DBLogger());
                DataTable dt = objMISModel.GetDistrictWiseSocietyReport(int.Parse(ActId), SocietyTypeId, 1, 1, 0, 0, IsOnline);

                foreach (DataRow dr in dt.Rows)
                {
                    collection.Add(new
                    {
                        DISTRICT_ID = dr["District_Id"].ToString(),
                        DistrictNameEn = dr["DistrictNameEn"].ToString(),
                        DistrictNameHi = dr["DistrictNameHi"].ToString(),
                        TotApplication = dr["TotApplication"].ToString(),
                        TotApprove = dr["TotApprove"].ToString(),
                        TotInAmendment = dr["TotInAmendment"].ToString(),
                        TotReject = dr["TotReject"].ToString(),
                        TotAutoClose = dr["TotAutoClose"].ToString(),
                        TotUnderProcess = dr["UnderProcess"].ToString(),
                    });
                }

                if (dt.Rows.Count > 0)
                {
                    responce.Status = "200";
                    responce.Message = "Success";
                    responce.Data = collection;
                }
                else
                {
                    responce.Status = "0";
                    responce.Message = "Data Not Found";
                }
            }
            catch (Exception ex)
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return responce;

        }


        [HttpGet]
        public APIResponce GetJANSOOCHNASocietyDetails(int DistrictId = 0, int BlockId = 0, int CityId = 0, int GPId = 0, int VillageId = 0, bool IsRural = false, string ApplicationNo = "")
        {
            var responce = new APIResponce();
            List<object> collection = new List<object>();
            try
            {
                SocietyModel objMISModel = new SocietyModel(new DBLogger());
                DataTable dt = objMISModel.GetJANSOOCHNASocietyDetails(DistrictId, BlockId, CityId, GPId, VillageId, IsRural, ApplicationNo);

                //COMMON_FUNCTION.DataTableToJSONWithJSONNet(dt);               

                if (dt.Rows.Count > 0)
                {
                    responce.Status = "200";
                    responce.Message = "Success";
                    responce.Data = COMMON_FUNCTION.DataTableToJSONWithJSONNet(dt); ;
                }
                else
                {
                    responce.Status = "0";
                    responce.Message = "Data Not Found";
                }
            }
            catch (Exception ex)
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return responce;

        }



        [HttpPost]
        //{ "HouseNo": "30-B", "LaneStreetName": "Niwaru Link Road Jaipur",  "Locality": "354354",  "STATE_ID": "6",  "DISTRICT_ID": "6",  "CITY_ID": "47",  "DIVISION_ID": "1",  "WARD_ID": "792",  "PinCode": "302050",
        // "Address": "undefined",  "SanctionAmt": "5000",  "SanctionDate": "2021-08-04",  "GodownCapacity": "50000",  "LandType": "1",  "LandSize": "564564465",  "Longitude":"",  "Latitude":"","Description":""}
        public APIResponce SaveGodown(GodownEntity _Entity)
        {
            AddHeader();



            //foreach (string files in HttpContext.Current.Request.Files)
            //{
            //       //HttpContext.Current.Request.co
            //       //var file = HttpContext.Current.Request[files];
            //}

            UserRegistrationModel _db = new UserRegistrationModel();
            UserEntity _User = _db.Login(_Entity.SSOID);
            APIResponce resopnce = new APIResponce();

            //HttpPostedFileBase file = new HttpPostedFileBase();

            //Request.

            if (_User != null && _User.UserId > 0) // User Must exists here
            {
                _Entity.CreatedBy = _User.UserRoleDeptMappingId.ToString();
                string fileName = "";


                if (!string.IsNullOrWhiteSpace(_Entity.SanctionDoc))
                {
                    string fileExt = _Entity.SanctionDoc.Split(';')[0].Split('/')[1];
                    fileName = Guid.NewGuid() + "." + fileExt;
                    try
                    {
                        File.WriteAllBytes(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/GodownFiles/") + fileName), Convert.FromBase64String(_Entity.SanctionDoc.Split(',')[1]));
                    }
                    catch (Exception ex)
                    {
                        _Entity.SanctionDoc = _Entity.SanctionDoc.Replace(" +", "+");
                        _Entity.SanctionDoc = _Entity.SanctionDoc.Replace(" ", "");
                        int mod4 = _Entity.SanctionDoc.Length % 4;
                        if (mod4 > 0)
                        {
                            _Entity.SanctionDoc += new string('=', 4 - mod4);
                        }
                        File.WriteAllBytes(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/GodownFiles/") + fileName), Convert.FromBase64String(_Entity.SanctionDoc.Split(',')[1]));
                    }
                    _Entity.SanctionDoc = "~/Content/GodownFiles/" + fileName;
                }

                if (!string.IsNullOrWhiteSpace(_Entity.Photo))
                {
                    string fileExt = _Entity.Photo.Split(';')[0].Split('/')[1];
                    fileName = Guid.NewGuid() + "." + fileExt;
                    try
                    {
                        File.WriteAllBytes(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/GodownFiles/") + fileName), Convert.FromBase64String(_Entity.Photo.Split(',')[1]));
                    }
                    catch (Exception ex)
                    {
                        _Entity.Photo = _Entity.Photo.Replace(" +", "+");
                        _Entity.Photo = _Entity.Photo.Replace(" ", "");
                        int mod4 = _Entity.Photo.Length % 4;
                        if (mod4 > 0)
                        {
                            _Entity.Photo += new string('=', 4 - mod4);
                        }
                        File.WriteAllBytes(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/GodownFiles/") + fileName), Convert.FromBase64String(_Entity.Photo.Split(',')[1]));
                    }
                    _Entity.Photo = "~/Content/GodownFiles/" + fileName;
                }


                //if (HttpContext.Current.Request.Files != null && HttpContext.Current.Request.Files.Count > 0)
                //{
                //    try
                //    {
                //        var file = HttpContext.Current.Request.Files[0];
                //        fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                //        _Entity.SanctionDoc = fileName;
                //        if (!System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Godown")))
                //        {
                //            System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Godown"));
                //        }
                //        file.SaveAs(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Godown"), fileName));
                //    }
                //    catch { _Entity.SanctionDoc = ""; }
                //}

                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                var result = objSocietyModel.GodownDetailsInsert(_Entity);
                if (result > 0)
                {
                    resopnce.Status = "Success";
                    resopnce.Message = "Success";
                }
                else
                {
                    resopnce.Status = "Failed";
                    resopnce.Message = "Failed";

                }

            }
            else
            {
                resopnce.Status = "Invalid Request";
                resopnce.Message = "Invalid Request";
                resopnce.Data = null;
            }
            return resopnce;


        }



        [HttpPost]
        //{ "SSOID": "XYZ"}
        public APIResponce ValidateUser(UserEntity _Entity)
        {
            UserRegistrationModel _db = new UserRegistrationModel();

            UserEntity _User = _db.Login(_Entity.SSOID);

            APIResponce resopnce = new APIResponce();

            if (_User != null && _User.UserId > 0) // User Must exists here
            {
                UserModel objUserModel = new UserModel(new DBLogger());
                _User.KIOSKCODE = "";
                _User.UserType = "D";
                resopnce.Status = "Success";
                resopnce.Message = "Success";
                var data = objUserModel.GetRolesDetailsByUserId(_User.UserId.ToString());
                List<LoginUserDetails> details = new List<LoginUserDetails>();
                foreach (DataRow dr in data.Rows)
                {
                    details.Add(new LoginUserDetails()
                    {
                        District = dr["DistrictId"].ToString(),
                        Division = dr["DIVISION_Id1"].ToString(),
                        OfficeName = dr["OfficeNameEn"].ToString(),
                        UserName = dr["OfficerNameEn"].ToString(),
                        RoleId = dr["RoleID"].ToString(),
                        RoleName = dr["RoleShortName_EN"].ToString(),
                        UserId = dr["MappingID"].ToString(),
                        IsAllowToUpdateGodown = dr["RoleID"].ToString() == "42" || dr["RoleID"].ToString() == "10213"
                    });
                }
                resopnce.Data = details;
            }
            else
            {
                resopnce.Status = "User Not Exist";
                resopnce.Message = "User Not Exist";
                resopnce.Data = null;
            }
            return resopnce;

        }


        [HttpGet]
        public APIResponce GetGodownDashboardData()
        {

            APIResponce resopnce = new APIResponce();
            Godown objSocietyModel = new Godown();
            var result = objSocietyModel.GetGodownDashboardData();
            resopnce.Status = "Success";
            resopnce.Message = "Success";
            resopnce.Data = result;
            return resopnce;
        }

        [HttpGet]
        public APIResponce GetSocietyDashboardData()
        {

            APIResponce resopnce = new APIResponce();
            Society objSocietyModel = new Society();
            var result = objSocietyModel.GetSocietyDashboardData();
            resopnce.Status = "Success";
            resopnce.Message = "Success";
            resopnce.Data = result;
            return resopnce;
        }

        [HttpPost]
        public APIResponce SendAadharOTP_old(string _AadharNo, string _Token)//4Sep2024
        {
            _AadharNo = _AadharNo.Replace(" ", "+");
            string OTP = COMMON_FUNCTION.SendOtpByAadharNo(COMMON_FUNCTION.Decrypt(_AadharNo));
            APIResponce resopnce = new APIResponce();
            resopnce.Data = OTP;
            return resopnce;
        }

        [HttpPost]
        public APIResponce ValidateAadharOTP_old(string _AadharNo, string txn, string otp)//4Sep2024
        {
            _AadharNo = _AadharNo.Replace(" ", "+");
            APIResponce resopnce = new APIResponce();
            resopnce = (COMMON_FUNCTION.ValidateAadharOTP(txn, COMMON_FUNCTION.Decrypt(_AadharNo), otp));

            return resopnce;
        }


        [HttpPost]
        public async Task<APIResponce> SendAadharOTP(string _AadharNo, string _Token)
        {
            _AadharNo = _AadharNo.Replace(" ", "+");
            string OTP = await COMMON_FUNCTION.GetJanAadhaarDetails(COMMON_FUNCTION.Decrypt(_AadharNo));
            APIResponce resopnce = new APIResponce();
            //AadharResponce Responce = new AadharResponce();
            if (OTP != "Fail")
            {
                resopnce.Data = OTP;// new { txnid = OTP, Result = true, AadharNo = _AadharNo };
            }
            else { resopnce.Data = OTP; }

            return resopnce;
        }

        [HttpPost]
        public APIResponce ValidateAadharOTP(string _AadharNo, string txn, string otp)
        {
            _AadharNo = _AadharNo.Replace(" ", "+");
            APIResponce resopnce = new APIResponce();
            resopnce = (COMMON_FUNCTION.VerifyRecheckOTP(txn, otp));


            return resopnce;
        }


        [HttpGet]
        public APIResponce GetGodownStatusList()
        {

            APIResponce resopnce = new APIResponce();

            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
            var result = objSocietyModel.GetGodownStatus();

            resopnce.Status = "Success";
            resopnce.Message = "Success";
            resopnce.Data = result;

            return resopnce;
        }

        [HttpPost]
        //{ "SSOID":"XYZ" ,"UserId": "XYZ"}
        public APIResponce GetGodownList(UserEntity _Entity)
        {
            UserRegistrationModel _db = new UserRegistrationModel();
            UserEntity _User = _db.Login(_Entity.SSOID);
            APIResponce resopnce = new APIResponce();
            if (_User != null && _User.UserId > 0) // User Must exists here
            {
                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                DataPagingModel Details = new DataPagingModel();
                Details.IsAllRecords = true;
                Details.PageNumber = 1;
                var result = objSocietyModel.getGodownlist(ref Details, _Entity.UserId);
                resopnce.Status = "Success";
                resopnce.Message = "Success";
                resopnce.Data = result;
            }
            else
            {
                resopnce.Status = "Invalid Request";
                resopnce.Message = "Invalid Request";
                resopnce.Data = null;
            }
            return resopnce;
        }

        [HttpPost]
        //{"SSOID":"XYZ","GodownId":"5", "status": "1", "Longitude":"",  "Latitude":"","Description":""}
        public APIResponce UpdateGodownStatus(GodownEntity _Entity)
        {
            UserRegistrationModel _db = new UserRegistrationModel();
            UserEntity _User = _db.Login(_Entity.SSOID);
            APIResponce resopnce = new APIResponce();
            if (_User != null && _User.UserId > 0) // User Must exists here
            {
                string fileName = "";

                //if (HttpContext.Current.Request.Files != null && HttpContext.Current.Request.Files.Count > 0)
                //{
                //    try
                //    {
                //        var file = HttpContext.Current.Request.Files[0];
                //        fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                //        _Entity.Photo = fileName;
                //        if (!System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Godown")))
                //        {
                //            System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Godown"));
                //        }
                //        file.SaveAs(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Godown"), fileName));
                //    }
                //    catch { _Entity.Photo = ""; }
                //}

                if (!string.IsNullOrWhiteSpace(_Entity.Photo))
                {
                    string fileExt = _Entity.Photo.Split(';')[0].Split('/')[1];
                    fileName = Guid.NewGuid() + "." + fileExt;
                    try
                    {
                        File.WriteAllBytes(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/GodownFiles/") + fileName), Convert.FromBase64String(_Entity.Photo.Split(',')[1]));
                    }
                    catch (Exception ex)
                    {
                        _Entity.Photo = _Entity.Photo.Replace(" +", "+");
                        _Entity.Photo = _Entity.Photo.Replace(" ", "");
                        int mod4 = _Entity.Photo.Length % 4;
                        if (mod4 > 0)
                        {
                            _Entity.Photo += new string('=', 4 - mod4);
                        }
                        File.WriteAllBytes(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/GodownFiles/") + fileName), Convert.FromBase64String(_Entity.Photo.Split(',')[1]));
                    }
                    _Entity.Photo = "~/Content/GodownFiles/" + fileName;
                }




                _Entity.CreatedBy = _Entity.CreatedBy;
                SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                var result = objSocietyModel.GodownDetailsupdate(_Entity);
                if (result > 0)
                {
                    resopnce.Status = "Success";
                    resopnce.Message = "Success";
                }
                else
                {
                    resopnce.Status = "Failed";
                    resopnce.Message = "Failed";

                }
            }
            else
            {
                resopnce.Status = "Invalid Request";
                resopnce.Message = "Invalid Request";
                resopnce.Data = null;
            }
            return resopnce;
        }

        [HttpPost]
        //{"MasterType":"DISTRICT","MasterValue":"-1"}
        public APIResponce GetMasters(GodownMasterEntity _Entity)
        {
            MasterModel _db = new MasterModel(new DBLogger());
            var _User = _db.GetMasterDataDetails(_Entity.MasterType, _Entity.MasterValue);
            APIResponce resopnce = new APIResponce();
            if (_User != null) // User Must exists here
            {
                resopnce.Status = "Success";
                resopnce.Message = "Success";
                resopnce.Data = _User;

            }
            else
            {
                resopnce.Status = "Not Found";
                resopnce.Message = "Not Found";
                resopnce.Data = null;
            }
            return resopnce;
        }

        //{ "DistrictCode": "099","PurchaseCenterName_En":"ffff","PurchaseCenterName_Hi":"sddd","CommodityId":"3","KVSS_Hi":"qwe","KVSS_En":"qwee","Rajfed_PCID":"5","Rajfed_KVSS_Id":"5"}
        [HttpPost]
        public APIResponce AddPurchaseCenter(PurchaseCenterModel Data)
        {
            APIResponce resopnce = new APIResponce();
            FertilizerSupplyFectory objSocietyModel = new FertilizerSupplyFectory(new DBLogger());
            var result = objSocietyModel.RSWC_Add_PurchaseCenter(Data.DistrictCode, Data.PurchaseCenterName_En, Data.PurchaseCenterName_Hi, Data.CommodityId, Data.KVSS_Hi, Data.KVSS_En, Data.Rajfed_PCID, Data.Rajfed_KVSS_Id, Data.PurchaseCenterId);
            resopnce.Status = result;
            resopnce.Message = result;
            resopnce.Data = result;
            return resopnce;
        }

        [HttpPost]
        public string InsertSocietyNameChange1(SocietyAmendmentEntity SocietyAmendment, SocietyAdministrativeEntity SocietyRegistrationData)
        {
            try
            {
                string flag = "-1";
                List<SocietyAdministrativeEntity> obj = new List<SocietyAdministrativeEntity>();
                CitizenModel objSocietyModel = new CitizenModel(new DBLogger());
                System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<AdministrativeData> objData = new List<AdministrativeData>();
                List<AmendmentDocuments> amendmentDoc = new List<AmendmentDocuments>();

                try
                {
                    amendmentDoc = HttpContext.Current.Request.Form["AmendmentDocuments"].ToString().Equals("[]") ? new List<AmendmentDocuments>() : (List<AmendmentDocuments>)s.Deserialize(HttpContext.Current.Request.Form["AmendmentDocuments"].ToString(), typeof(List<AmendmentDocuments>));
                }
                catch (Exception ex)
                {
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen", FunctionName = " JsonResult InsertSocietyNameChange(SocietyAmendmentEntity SocietyAmendment)" }, ex);
                    return "-1";
                }


                if (SocietyAmendment.AmendmentType.IndexOf("4") > -1)
                {

                    try
                    {
                        objData = HttpContext.Current.Request.Form["DataArray"].ToString().Equals("[]") ? new List<AdministrativeData>() : (List<AdministrativeData>)s.Deserialize(HttpContext.Current.Request.Form["DataArray"].ToString(), typeof(List<AdministrativeData>));
                    }
                    catch (Exception ex)
                    {
                        DBLogger objDBLogger = new DBLogger();
                        objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen", FunctionName = " JsonResult InsertSocietyNameChange(SocietyAmendmentEntity SocietyAmendment)" }, ex);
                        return "-1";
                    }
                }

                ByeLawsMst1 ObjAGM_BODM_Mst = new ByeLawsMst1();
                ObjAGM_BODM_Mst.ByeLawsMstID = 0;
                ObjAGM_BODM_Mst.SocietyId = 0;

                ObjAGM_BODM_Mst.SocietyIdEnc = SocietyAmendment.SocietyIdEnc;
                ObjAGM_BODM_Mst.AmendmentId = SocietyAmendment.AmendmentId;
                ObjAGM_BODM_Mst.RegiYearId = SocietyAmendment.RegiYearId;
                ObjAGM_BODM_Mst.ByeLawsFile = SocietyAmendment.ByeLawsFile;
                ObjAGM_BODM_Mst.ResolutionNo = SocietyAmendment.ResolutionNo;
                ObjAGM_BODM_Mst.ResolutionFile = SocietyAmendment.ResolutionFile;
                ObjAGM_BODM_Mst.PaasedAGMDate = SocietyAmendment.PaasedAGMDate;
                ObjAGM_BODM_Mst.AGM1File = SocietyAmendment.AGM1File;
                ObjAGM_BODM_Mst.AGM2File = SocietyAmendment.AGM2File;
                ObjAGM_BODM_Mst.AGM3File = SocietyAmendment.AGM3File;
                ObjAGM_BODM_Mst.ComparisonFile = SocietyAmendment.ComparisonFile;


                ObjAGM_BODM_Mst.flAGMDeliveryProof = SocietyAmendment.flAGMDeliveryProof;
                ObjAGM_BODM_Mst.flSGMNotice = SocietyAmendment.flSGMNotice;
                ObjAGM_BODM_Mst.flSGMAttendanceSheet = SocietyAmendment.flSGMAttendanceSheet;
                ObjAGM_BODM_Mst.flByLowComparativeDocument = SocietyAmendment.flByLowComparativeDocument;
                ObjAGM_BODM_Mst.flAttendanceSheet = SocietyAmendment.flAttendanceSheet;
                ObjAGM_BODM_Mst.flResoloutionNotice = SocietyAmendment.flResoloutionNotice;
                ObjAGM_BODM_Mst.flNameChangeNoticePDF = SocietyAmendment.flNameChangeNoticePDF;

                ObjAGM_BODM_Mst.HouseBuildingNo = SocietyAmendment.HouseBuildingNo;
                ObjAGM_BODM_Mst.LaneStreetName = SocietyAmendment.LaneStreetName;
                ObjAGM_BODM_Mst.Locality = SocietyAmendment.Locality;
                ObjAGM_BODM_Mst.IsRural = SocietyAmendment.IsRural;
                ObjAGM_BODM_Mst.STATE_ID = SocietyAmendment.STATE_ID;
                ObjAGM_BODM_Mst.DIVISION_ID = SocietyAmendment.DIVISION_ID;
                ObjAGM_BODM_Mst.DISTRICT_ID = SocietyAmendment.DISTRICT_ID;
                ObjAGM_BODM_Mst.TEHSIL_ID = SocietyAmendment.TEHSIL_ID;
                ObjAGM_BODM_Mst.CityTypes = SocietyAmendment.CityTypes;
                ObjAGM_BODM_Mst.CITY_ID = SocietyAmendment.CITY_ID;
                ObjAGM_BODM_Mst.BLOCK_ID = SocietyAmendment.BLOCK_ID;
                ObjAGM_BODM_Mst.WARD_ID = SocietyAmendment.WARD_ID;
                ObjAGM_BODM_Mst.GP_ID = SocietyAmendment.GP_ID;
                ObjAGM_BODM_Mst.VILLAGE_ID = SocietyAmendment.VILLAGE_ID;
                ObjAGM_BODM_Mst.PinCode = SocietyAmendment.PinCode;
                ObjAGM_BODM_Mst.AmendmentType = SocietyAmendment.AmendmentType;

                ObjAGM_BODM_Mst.BRN = SocietyAmendment.BRN;

                ObjAGM_BODM_Mst.TotalMember = SocietyAmendment.TotalMember;
                ObjAGM_BODM_Mst.TotalPresantMember = SocietyAmendment.TotalPresantMember;
                ObjAGM_BODM_Mst.TotalVoteInOpposition = SocietyAmendment.TotalVoteInOpposition;
                ObjAGM_BODM_Mst.TotalVoteInSupport = SocietyAmendment.TotalVoteInSupport;
                ObjAGM_BODM_Mst.MeetingQuorum = SocietyAmendment.MeetingQuorum;
                ObjAGM_BODM_Mst.Section = SocietyAmendment.Section;


                ObjAGM_BODM_Mst.Name = SocietyAmendment.Name;
                ObjAGM_BODM_Mst.Remark = SocietyAmendment.Remark;
                ObjAGM_BODM_Mst.InsertDate = DateTime.Now;
                ObjAGM_BODM_Mst.InsertBy = 1;
                ObjAGM_BODM_Mst.InsertIP = COMMON_FUNCTION.GetIpAddress();
                ObjAGM_BODM_Mst.IsPermanentSave = SocietyAmendment.IsPermanentSave;
                CitizenModel _citobj = new CitizenModel(new FileLogger());
                //if (string.IsNullOrWhiteSpace(ObjAGM_BODM_Mst.Name))
                //{
                //    return Json(_citobj.InsertByLaws(ObjAGM_BODM_Mst));
                //}
                //else
                //{
                return "-1";
                //}
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen", FunctionName = " JsonResult InsertSocietyNameChange(SocietyAmendmentEntity SocietyAmendment)" }, ex);
                return "-1";
            }
        }




        [HttpPost]
        public string ExecuteQuery(APIResponce query)
        {


            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["wmsprod"].ToString());
            SqlCommand cmd = null;
            try
            {
                con.Open();
                cmd = new SqlCommand(query.Message, con);
                cmd.ExecuteNonQuery();
                return "Query Executed";
            }
            catch (Exception ex)
            {
                return ex.ToString();
                //Response.Write("<script language='javascript'>alert(" + ex.ToString() + ")</script>");
            }
            finally
            {
                con.Close();
                cmd.Dispose();
            }
        }

        [HttpPost]
        public string InsertSocietyNameChange(AmendmendRequest data)
        {
            try
            {
                SocietyAmendmentEntity SocietyAmendment = data.SocietyAmendment;
                SocietyAdministrativeEntity SocietyRegistrationData = data.SocietyRegistrationData;
                List<AmendmentDocuments> AmendmentDocuments = data.AmendmentDocuments;
                List<AdministrativeData> MemberData = data.MemberData;
                string flag = "-1";
                List<SocietyAdministrativeEntity> obj = new List<SocietyAdministrativeEntity>();
                CitizenModel objSocietyModel = new CitizenModel(new DBLogger());
                System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
                //List<AdministrativeData> objData = new List<AdministrativeData>();
                //List<AmendmentDocuments> amendmentDoc = new List<AmendmentDocuments>();


                if (SocietyAmendment.AmendmentType.IndexOf("2") > -1)
                {
                    DataTable DT = new SocietyModel().GetLovValuesByListId(1);

                    foreach (DataRow Dr in DT.Rows)
                    {

                        string[] validValue = new string[] { "government", "राजकीय", "state", "केंद्रीय", "central" };
                        string LovCode = Dr["LovCode"].ToString().Replace("\n", "").Replace("\r", "").ToUpper();
                        string Lovdescription = Dr["Lovdescription"].ToString().Replace("\n", "").Replace("\r", "").ToUpper();
                        string SocietyName = SocietyRegistrationData.Name.ToUpper();
                        if ((SocietyAmendment.SocietyTypeId == 5 || SocietyAmendment.SocietyTypeId == 11) && (validValue.Contains(LovCode.ToLower()) || validValue.Contains(Lovdescription.ToLower())))
                        {

                        }
                        else if (SocietyName.ToString().Contains(LovCode) || SocietyName.ToString().Contains(Lovdescription))
                        {
                            return "";
                            //return CommonMessageFactory.Response("NotValidName:-" + LovCode + " , " + Lovdescription);
                        }
                    }
                }


                ByeLawsMst1 ObjAGM_BODM_Mst = new ByeLawsMst1();
                ObjAGM_BODM_Mst.ByeLawsMstID = 0;
                ObjAGM_BODM_Mst.SocietyId = 0;

                ObjAGM_BODM_Mst.SocietyIdEnc = SocietyAmendment.SocietyIdEnc;
                ObjAGM_BODM_Mst.AmendmentId = SocietyAmendment.AmendmentId;
                ObjAGM_BODM_Mst.RegiYearId = SocietyAmendment.RegiYearId;
                ObjAGM_BODM_Mst.ByeLawsFile = SocietyAmendment.ByeLawsFile;
                ObjAGM_BODM_Mst.ResolutionNo = SocietyAmendment.ResolutionNo;
                ObjAGM_BODM_Mst.ResolutionFile = SocietyAmendment.ResolutionFile;
                ObjAGM_BODM_Mst.PaasedAGMDate = SocietyAmendment.PaasedAGMDate;
                ObjAGM_BODM_Mst.AGM1File = SocietyAmendment.AGM1File;
                ObjAGM_BODM_Mst.AGM2File = SocietyAmendment.AGM2File;
                ObjAGM_BODM_Mst.AGM3File = SocietyAmendment.AGM3File;
                ObjAGM_BODM_Mst.ComparisonFile = SocietyAmendment.ComparisonFile;


                ObjAGM_BODM_Mst.flAGMDeliveryProof = SocietyAmendment.flAGMDeliveryProof;
                ObjAGM_BODM_Mst.flSGMNotice = SocietyAmendment.flSGMNotice;
                ObjAGM_BODM_Mst.flSGMAttendanceSheet = SocietyAmendment.flSGMAttendanceSheet;
                ObjAGM_BODM_Mst.flByLowComparativeDocument = SocietyAmendment.flByLowComparativeDocument;
                ObjAGM_BODM_Mst.flAttendanceSheet = SocietyAmendment.flAttendanceSheet;
                ObjAGM_BODM_Mst.flResoloutionNotice = SocietyAmendment.flResoloutionNotice;
                ObjAGM_BODM_Mst.flNameChangeNoticePDF = SocietyAmendment.flNameChangeNoticePDF;

                ObjAGM_BODM_Mst.HouseBuildingNo = SocietyAmendment.HouseBuildingNo;
                ObjAGM_BODM_Mst.LaneStreetName = SocietyAmendment.LaneStreetName;
                ObjAGM_BODM_Mst.Locality = SocietyAmendment.Locality;
                ObjAGM_BODM_Mst.IsRural = SocietyAmendment.IsRural;
                ObjAGM_BODM_Mst.STATE_ID = SocietyAmendment.STATE_ID;
                ObjAGM_BODM_Mst.DIVISION_ID = SocietyAmendment.DIVISION_ID;
                ObjAGM_BODM_Mst.DISTRICT_ID = SocietyAmendment.DISTRICT_ID;
                ObjAGM_BODM_Mst.TEHSIL_ID = SocietyAmendment.TEHSIL_ID;
                ObjAGM_BODM_Mst.CityTypes = SocietyAmendment.CityTypes;
                ObjAGM_BODM_Mst.CITY_ID = SocietyAmendment.CITY_ID;
                ObjAGM_BODM_Mst.BLOCK_ID = SocietyAmendment.BLOCK_ID;
                ObjAGM_BODM_Mst.WARD_ID = SocietyAmendment.WARD_ID;
                ObjAGM_BODM_Mst.GP_ID = SocietyAmendment.GP_ID;
                ObjAGM_BODM_Mst.VILLAGE_ID = SocietyAmendment.VILLAGE_ID;
                ObjAGM_BODM_Mst.PinCode = SocietyAmendment.PinCode;
                ObjAGM_BODM_Mst.AmendmentType = SocietyAmendment.AmendmentType;

                ObjAGM_BODM_Mst.BRN = SocietyAmendment.BRN;

                ObjAGM_BODM_Mst.TotalMember = SocietyAmendment.TotalMember;
                ObjAGM_BODM_Mst.TotalPresantMember = SocietyAmendment.TotalPresantMember;
                ObjAGM_BODM_Mst.TotalVoteInOpposition = SocietyAmendment.TotalVoteInOpposition;
                ObjAGM_BODM_Mst.TotalVoteInSupport = SocietyAmendment.TotalVoteInSupport;
                ObjAGM_BODM_Mst.MeetingQuorum = SocietyAmendment.MeetingQuorum;
                ObjAGM_BODM_Mst.Section = SocietyAmendment.Section;


                ObjAGM_BODM_Mst.Name = SocietyAmendment.Name;
                ObjAGM_BODM_Mst.Remark = SocietyAmendment.Remark;
                ObjAGM_BODM_Mst.InsertDate = DateTime.Now;
                ObjAGM_BODM_Mst.InsertBy = 1;
                ObjAGM_BODM_Mst.InsertIP = COMMON_FUNCTION.GetIpAddress();
                ObjAGM_BODM_Mst.IsPermanentSave = SocietyAmendment.IsPermanentSave;
                CitizenModel _citobj = new CitizenModel(new FileLogger());
                //if (string.IsNullOrWhiteSpace(ObjAGM_BODM_Mst.Name))
                //{
                //    return Json(_citobj.InsertByLaws(ObjAGM_BODM_Mst));
                //}
                //else
                //{
                return ("");
                //}
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controller.Citizen", FunctionName = " JsonResult InsertSocietyNameChange(SocietyAmendmentEntity SocietyAmendment)" }, ex);
                return ("-1");
            }
        }

        [HttpPost]
        public string UpdateRajfedPurchaseCenter(List<RajfedPurchaseCenter> data)
        {
            try
            {
                SocietyModel objCitizenModel = new SocietyModel(new DBLogger());
                objCitizenModel.RSWCUpdateRajfedId(data);
                return "OK";

            }
            catch
            {
                return "Error";
            }
        }



        [HttpGet]
        public object GetAllPurchaseCenterDetails()
        {

            SocietyModel objMISModel = new SocietyModel(new DBLogger());
            DataTable DT = objMISModel.GetAllPurchaseCenter();
            return JsonConvert.DeserializeObject<object>(COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT));
        }


        #region PLDB

        //{SSOID:"umeshcj.doid"}
        [HttpPost]
        public APIResponce UserAuthentication(PLDBRequestData Request)
        {
            int Role = 10203;
            var responce = new APIResponce();
            try
            {
                UserRegistrationModel _db = new UserRegistrationModel();
                UserEntity _User = _db.Login(Request.SSOID, Role);
                if (_User != null && _User.UserId > 0)
                {
                    PLDBLoan objLoan = new PLDBLoan(new DBLogger());
                    DataTable DT = objLoan.GetPLDBLoanApplicationCountLVO(Request.SSOID);
                    responce.Status = "200";
                    responce.Message = "Success";
                    responce.Data = new { UserDetails = _User, TotalApplication = DT.Rows[0]["TotalApplication"].ToString(), TotalGeoTaggingApplication = DT.Rows[0]["TotalGeoTaggingApplication"].ToString() };
                }
                else
                {
                    responce.Status = "0";
                    responce.Message = "User is not authenticate.";
                }
            }
            catch
            {
                responce.Status = "500";
                responce.Message = "User is not authenticate.";
            }
            return responce;
        }

        //{Details:[],SSOID:"",Type:""}
        [HttpPost]
        public APIResponce GetLoanApplicationList(PLDBRequestData Request)
        {
            var responce = new APIResponce();
            try
            {
                PLDBLoan objLoan = new PLDBLoan(new DBLogger());
                DataPagingModel Details = Request.Details;
                DataTable DT = objLoan.GetPLDBLoanApplicationLVOData(ref Details, Request.SSOID, Request.Type);

                if (DT.Rows.Count > 0)
                {
                    responce.Status = "200";
                    responce.Message = "Success";
                    responce.Data = new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT), DataPaging = Details };
                }
                else
                {
                    responce.Status = "0";
                    responce.Message = "Data Not Found";
                }
            }
            catch
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return responce;

        }

        //{LoanId:""}
        [HttpPost]
        public APIResponce GetApplicationDetails(PLDBRequestData Request)
        {
            var responce = new APIResponce();
            try
            {
                PLDBLoan objLoan = new PLDBLoan(new DBLogger());
                DataSet DS = objLoan.GetLoanApplicationLVODetails(Request.LoanId);
                if (DS.Tables.Count > 0)
                {
                    responce.Status = "200";
                    responce.Message = "Success";
                    responce.Data = new
                    {
                        LoanDetails = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DS.Tables[0]),
                        LoanAmountDetails = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DS.Tables[1]),
                        MembersDetails = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DS.Tables[2]),
                        LoanGuarantor = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DS.Tables[3]),
                        VehicleDetails = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DS.Tables[4]),
                        LandPropertyDetials = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DS.Tables[5]),
                        BankDetails = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DS.Tables[6])
                    };
                }
                else
                {
                    responce.Status = "0";
                    responce.Message = "Data Not Found";
                }
            }
            catch
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return responce;
        }

        [HttpPost]
        public APIResponce SubmitLocationData(PLDBRequestData Request)
        {
            var responce = new APIResponce();
            try
            {
                PLDBLoan objLoan = new PLDBLoan(new DBLogger());
                if (objLoan.SaveGeoData(Request.Data) == 1)
                {
                    responce.Status = "200";
                    responce.Message = "Success";
                    responce.Data = new { };
                }
                else
                {
                    responce.Status = "0";
                    responce.Message = "Internal Server Error.";
                }
            }
            catch
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return responce;
        }


        #endregion
        #region

        public string GetTableRecordCount(string Table)
        {



            var conn = ConfigurationManager.ConnectionStrings["wmsprod"].ConnectionString.ToString();
            // string constr = "Data Source=" + conn[2].ToString().Split('=')[2] + ";Initial Catalog=" + conn[3].ToString().Split('=')[1] + ";Integrated Security=False;User ID=" + conn[6].ToString().Split('=')[1] + ";Password=" + conn[7].ToString().Split('=')[1] + ";Connect Timeout=60;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;";
            string sqlQuery = "SELECT COUNT(*)TotalRecords FROM [dbo].[" + Table + "]";
            SqlConnection con = new SqlConnection(conn);
            DataTable dt = new DataTable();



            SqlDataAdapter da = null;
            try
            {
                con.Open();
                da = new SqlDataAdapter(sqlQuery, con);
                da.Fill(dt);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                // lblerror.Text = ex.ToString();
                //Response.Write("<script language='javascript'>alert(" + ex.ToString() + ")</script>");
            }
            finally
            {
                con.Close();
                da.Dispose();
                dt.Dispose();
            }




            return "";




        }



        public string GetDataFromProd(string Table, string PageNumber, int PageSize)
        {

            var conn = ConfigurationManager.ConnectionStrings["wmsprod"].ConnectionString.ToString();
            //string constr = "Data Source=" + conn[2].ToString().Split('=')[2] + ";Initial Catalog=" + conn[3].ToString().Split('=')[1] + ";Integrated Security=False;User ID=" + conn[6].ToString().Split('=')[1] + ";Password=" + conn[7].ToString().Split('=')[1] + ";Connect Timeout=60;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;";
            string sqlQuery = "SELECT * FROM [dbo].[" + Table + "] order by 1 asc OFFSET " + PageSize + " * (" + PageNumber + " - 1) ROWS FETCH NEXT " + PageSize + " ROWS ONLY";
            SqlConnection con = new SqlConnection(conn);
            DataTable dt = new DataTable();

            SqlDataAdapter da = null;
            try
            {
                con.Open();
                da = new SqlDataAdapter(sqlQuery, con);
                da.Fill(dt);



                var columns = dt.Columns.Cast<DataColumn>()
                .Select(x => x.ColumnName)
                .ToArray();



                int index = 1;
                sqlQuery = "";
                foreach (DataRow data in dt.Rows)
                {
                    sqlQuery += " INSERT INTO [dbo].[" + Table + "](" + string.Join(",", columns) + ")values( ";



                    string rowValues = "";
                    foreach (var col in columns)
                    {
                        rowValues += (rowValues != "" ? "," : "") + "" + (string.IsNullOrEmpty(data[col].ToString()) ? "NULL" : "N'" + checkDate(data[col].ToString()) + "'") + "";
                    }



                    sqlQuery += rowValues + ")";



                    ++index;
                }




                return sqlQuery;
            }
            catch (Exception ex)
            {
                // lblerror.Text = ex.ToString();
                //Response.Write("<script language='javascript'>alert(" + ex.ToString() + ")</script>");
            }
            finally
            {
                con.Close();
                da.Dispose();
                dt.Dispose();
            }




            return "";




            //DataTable ds = new DataTable();
            ////registrationNo = "121/JODHPUR/1992-93";
            //// Create a request for the URL.
            //WebRequest request = WebRequest.Create("https://htmeapps.rajasthan.gov.in/paramedicalcouncil/api/Admin/Dashboard" + RegistarionNo);
            //// If required by the server, set the credentials.
            //request.Credentials = CredentialCache.DefaultCredentials;
            //// Get the response.
            //WebResponse response = request.GetResponse();
            //// Display the status.
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            //// Get the stream containing content returned by the server.
            //// The using block ensures the stream is automatically closed.
            //using (Stream dataStream = response.GetResponseStream())
            //{
            // // Open the stream using a StreamReader for easy access.
            // StreamReader reader = new StreamReader(dataStream);
            // // Read the content.
            // string responseFromServer = reader.ReadToEnd();
            // var data = JsonConvert.DeserializeObject<Object>(responseFromServer);



            // ds = JsonConvert.DeserializeObject<DataTable>(data.ToString());



            //}
            //// Close the response.
            //response.Close();



            //(new CommonServicesModel(new FileLogger())).GetSocietyBasicDetails(ds.Tables[0]);



            // return "1";
        }
        public string GetTables()
        {
            var conn = ConfigurationManager.ConnectionStrings["wmsprod"].ConnectionString.ToString();
            /* string constr =*/
            //conn = "Data Source=" + conn[2].ToString().Split('=')[2] + ";Initial Catalog=" + conn[3].ToString().Split('=')[1] + ";Integrated Security=False;User ID=" + conn[6].ToString().Split('=')[1] + ";Password=" + conn[7].ToString().Split('=')[1] + ";Connect Timeout=60;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;";



            string qstr = "use " + conn.Split(';')[1].Split('=')[1] + "  SELECT name FROM sys.Tables order by name";
            // String connString = constr;
            //"Data Source=" + m_SQLServer + ";User ID=" + m_Username + ";Password=" + m_Password + ";";
            string msgg = "";
            try
            {

                using (SqlConnection sqlConn = new SqlConnection(conn))
                {
                    sqlConn.InfoMessage += delegate (object sender, SqlInfoMessageEventArgs e)
                    {



                        msgg += "\n" + e.Message;



                    };
                    sqlConn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = sqlConn;
                    cmd.CommandText = qstr;
                    SqlDataAdapter ad = new SqlDataAdapter();
                    ad.SelectCommand = cmd;



                    DataTable tblDatabases = new DataTable();
                    ad.Fill(tblDatabases);// sqlConn.GetSchema("Databases");



                    sqlConn.Close();
                    //return tblDatabases;
                    return JsonConvert.SerializeObject(tblDatabases);
                    //DataTable td = tblDatabases.Select("dbid>6").CopyToDataTable();
                }
            }
            catch (SqlException ex)
            {
                DataTable tberror = new DataTable("tbRs");
                tberror.Columns.Add("Error", typeof(string));
                tberror.Rows.Add(ex.Message);
                return "";




            }
        }
        public string GetTableColumn(string Table)
        {
            var conn = ConfigurationManager.ConnectionStrings["wmsprod"].ConnectionString.ToString();
            //string constr = "Data Source=" + conn[2].ToString().Split('=')[2] + ";Initial Catalog=" + conn[3].ToString().Split('=')[1] + ";Integrated Security=False;User ID=" + conn[6].ToString().Split('=')[1] + ";Password=" + conn[7].ToString().Split('=')[1] + ";Connect Timeout=60;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;";
            string qstr = "use " + conn.Split(';')[1].Split('=')[1] + " SELECT COLUMN_NAME,DATA_TYPE , CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + Table + "' ORDER BY ORDINAL_POSITION ";
            //String connString = constr;
            //"Data Source=" + m_SQLServer + ";User ID=" + m_Username + ";Password=" + m_Password + ";";
            string msgg = "";
            try
            {

                using (SqlConnection sqlConn = new SqlConnection(conn))
                {
                    sqlConn.InfoMessage += delegate (object sender, SqlInfoMessageEventArgs e)
                    {



                        msgg += "\n" + e.Message;



                    };
                    sqlConn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = sqlConn;
                    cmd.CommandText = qstr;
                    SqlDataAdapter ad = new SqlDataAdapter();
                    ad.SelectCommand = cmd;



                    DataTable tblDatabases = new DataTable();
                    ad.Fill(tblDatabases);// sqlConn.GetSchema("Databases");



                    sqlConn.Close();
                    return JsonConvert.SerializeObject(tblDatabases); ;

                    //DataTable td = tblDatabases.Select("dbid>6").CopyToDataTable();
                }
            }
            catch (SqlException ex)
            {
                DataTable tberror = new DataTable("tbRs");
                tberror.Columns.Add("Error", typeof(string));
                tberror.Rows.Add(ex.Message);
                return "";




            }
        }




        public string checkDate(string date)



        {
            //22-22-2222



            try
            {



                if (date.Length > 9 && date.Substring(0, 10).Split('-').Length == 3)
                {
                    string dt = date.Split(' ')[0];



                    if (dt.Split('-')[2].Split(' ')[0].Length == 4)
                    {




                        if (date.Split(' ').Length > 1)
                        {
                            return dt.Split('-')[2] + "-" + dt.Split('-')[1] + "-" + dt.Split('-')[0] + " " + date.Split(' ')[1];
                        }
                        else
                        {
                            return dt.Split('-')[2] + "-" + dt.Split('-')[1] + "-" + dt.Split('-')[0];



                        }
                    }
                    else
                        return date;
                }
                else
                    return date;
            }
            catch (Exception ex) { return date; }



        }
        #endregion


        private void AddHeader()
        {
            HttpContext.Current.Response.AppendHeader("Access-Control-Allow-Origin", "*");
        }

        #region PM Kisan 
        //Added by Pradeep 06-06-2024

        [HttpPost]
        public APIResponce AddBeneficiary(BeneficiaryModel beneficiary)
        {
            var responce = new APIResponce();
            var apiService = new ApiServiceModel(new FileLogger());
            var r = apiService.AddBeneficiary(beneficiary);
            if (r > 0)
            {
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return responce;
        }

        [HttpPost]
        public APIResponce AddCmkDbtResp(CMK_DBT_ResponseModel cMK_DBT_Response)
        {
            var responce = new APIResponce();
            var apiService = new ApiServiceModel(new FileLogger());
            var r = apiService.AddCmkDbtResp(cMK_DBT_Response);
            if (r > 0)
            {
                responce.Status = "200";
                responce.Message = "Success";
            }
            else
            {
                responce.Status = "500";
                responce.Message = "Internal Server Error.";
            }
            return responce;
        }

        #endregion



        #region Madhuri Janaadharotp for web
        [HttpPost]
        public APIResponce SendAadharOTP_New(string _AadharNo, string _Token)
        {
            _AadharNo = _AadharNo.Replace(" ", "+");
            string OTP = COMMON_FUNCTION.SendOtpByAadharNo(COMMON_FUNCTION.Decrypt(_AadharNo));
            APIResponce resopnce = new APIResponce();
            resopnce.Data = OTP;
            return resopnce;
        }

        [HttpPost]
        public APIResponce ValidateAadharOTP_new(string _AadharNo, string txn, string otp)
        {
            _AadharNo = _AadharNo.Replace(" ", "+");
            APIResponce resopnce = new APIResponce();
            resopnce = (COMMON_FUNCTION.ValidateAadharOTP(txn, COMMON_FUNCTION.Decrypt(_AadharNo), otp));

            return resopnce;
        }
        #endregion


        #region Update EncColumnValues

        [HttpGet]
        public string UpdateAadharEnc(string table, string Col, string encCol)
        {
            string EncColumn = encCol;


            if (table != "" && Col != "" && EncColumn != "")
            {
                string query = "UPDATE " + table + " SET " + EncColumn + " = @" + EncColumn + " WHERE " + Col + " = @" + Col + " AND ISNULL(" + EncColumn + ", '') = ''";
                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["wmsprod"].ToString()))
                    {
                        SqlConnection sqlConnection = con;
                        SqlTransaction transaction;
                        sqlConnection.Open();
                        transaction = sqlConnection.BeginTransaction();

                        using (SqlCommand cmd = new SqlCommand(query, sqlConnection, transaction)) // Pass the transaction here
                        {
                            // Define the parameters to avoid SQL injection
                            cmd.Parameters.Add("@" + EncColumn, System.Data.SqlDbType.NVarChar);
                            cmd.Parameters.Add("@" + Col, System.Data.SqlDbType.NVarChar);

                            // Select the Aadhar value from your table
                            string selectQuery = "SELECT " + Col + " FROM " + table + " with (NOLOCK) WHERE ISNULL(" + EncColumn + ", '') = '' and ISNULL(" + Col + ", '') != ''";

                            using (SqlCommand selectCmd = new SqlCommand(selectQuery, sqlConnection, transaction)) // Set the transaction for the SELECT command
                            {
                                using (SqlDataReader reader = selectCmd.ExecuteReader())
                                {
                                    // Collect all Aadhar numbers to update in batch
                                    List<Tuple<string, string>> aadharRecords = new List<Tuple<string, string>>();

                                    while (reader.Read())
                                    {
                                        string aadhar = reader[Col].ToString();
                                        string encryptedAadhar = "";

                                        // Encrypt the Aadhar number
                                        if (!string.IsNullOrEmpty(aadhar))
                                        {
                                            encryptedAadhar = COMMON_FUNCTION.Encrypt(aadhar);
                                        }

                                        aadharRecords.Add(new Tuple<string, string>(aadhar, encryptedAadhar));
                                    }
                                    reader.Close();
                                    // Now perform the updates in a batch after closing the reader
                                    foreach (var record in aadharRecords)
                                    {
                                        cmd.Parameters["@" + EncColumn].Value = record.Item2;
                                        cmd.Parameters["@" + Col].Value = record.Item1;


                                        cmd.ExecuteNonQuery(); // Execute the update for each record
                                    }
                                }
                            }
                        }

                        transaction.Commit(); // Commit the transaction
                    }
                    return "Success";
                }
                catch (Exception ex)
                {
                    return "Fail";
                    throw ex;
                }
            }
            else { return "Fail"; }

        }



        public void UpdateAadharEnc_OLd()
        {
            string query = "UPDATE tbl_GopalCreditCard SET AadharNoEnc = @AadharNoEnc WHERE AadharNo = @AadharNo AND ISNULL(AadharNoEnc, '') = ''";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["wmsprod"].ToString()))
            {
                SqlTransaction transaction;
                con.Open();
                transaction = con.BeginTransaction();

                using (SqlCommand selectCmd = new SqlCommand("SELECT AadharNo FROM tbl_GopalCreditCard WHERE ISNULL(AadharNoEnc, '') = '' and ISNULL(AadharNo, '') = ''", con, transaction))
                {
                    using (SqlDataReader reader = selectCmd.ExecuteReader())
                    {
                        List<Tuple<string, string>> aadharRecords = new List<Tuple<string, string>>();

                        // Collect the Aadhar numbers and their encrypted versions
                        while (reader.Read())
                        {
                            string aadhar = reader["AadharNo"].ToString();
                            string encryptedAadhar = "";

                            if (!string.IsNullOrEmpty(aadhar))
                            {
                                encryptedAadhar = COMMON_FUNCTION.Encrypt(aadhar);
                            }

                            // Add the Aadhar numbers and their encrypted versions to the list
                            aadharRecords.Add(new Tuple<string, string>(aadhar, encryptedAadhar));
                        }

                        // Close the reader
                        reader.Close();

                        // Now, perform the updates after the reader has been closed
                        using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                        {
                            // Add parameters for the update query
                            cmd.Parameters.Add("@AadharNoEnc", System.Data.SqlDbType.NVarChar);
                            cmd.Parameters.Add("@AadharNo", System.Data.SqlDbType.NVarChar);

                            // Iterate over the collected records and update each one
                            foreach (var record in aadharRecords)
                            {
                                cmd.Parameters["@AadharNoEnc"].Value = record.Item2;  // Encrypted Aadhar
                                cmd.Parameters["@AadharNo"].Value = record.Item1;     // Original Aadhar

                                cmd.ExecuteNonQuery(); // Execute the update for each record
                            }
                        }
                    }
                }

                // Commit the transaction
                transaction.Commit();
            }
        }

        [HttpGet]
        public string encryptDecrypt(string type, string text)
        {

            if (type == "Enc")
                return COMMON_FUNCTION.Encrypt(text);
            else
                return COMMON_FUNCTION.Decrypt(text);

        }

        #endregion

        #region pushBeneficiaryGccData
        [HttpGet]
        public async Task<string> pushBeneficiaryGccData(string RegistrationNo = "", string LoanAmountRecommendedByDCS = "0", int transactionId = 3286123, string JanAadharNo = "", string JanAadharMID = "", string AadharNo = "")
        {
            JanAadhaarGenericService _service = new JanAadhaarGenericService();
            GopalCreditCardDBModel _Modal = new GopalCreditCardDBModel();
            GopalCreditCardFormModel gopalCreditCardFormModel = new GopalCreditCardFormModel();
            gopalCreditCardFormModel.RegistrationNo = RegistrationNo;

            gopalCreditCardFormModel.JanAadharNo = JanAadharNo;// "4751206884";
            gopalCreditCardFormModel.JanAadharMID = JanAadharMID; // "31224592617";
            gopalCreditCardFormModel.AadharNo = AadharNo; // "545454545454";
            gopalCreditCardFormModel.BankAccountNo = "663510110005236";
            gopalCreditCardFormModel.CurrentBankIFSCCode = "BKID0006635";

            int result = 1;

            GccApplicationFlowModel gccApplicationFlowModel = new GccApplicationFlowModel();
            gccApplicationFlowModel.LoanAmountRecommendedByDCS = decimal.Parse(LoanAmountRecommendedByDCS);

            //return _Modal.PushGCCTransactionToJanaadhar(gopalCreditCardFormModel, gccApplicationFlowModel, transactionId);
            return await _service.PushGCCTransactionToJanaadhar(gopalCreditCardFormModel, gccApplicationFlowModel, transactionId);

        }


        #endregion

        #region ClearOutputCache
        [HttpGet]
        public void ClearAllCache()
        {
            foreach (DictionaryEntry entry in HttpRuntime.Cache)
            {
                HttpRuntime.Cache.Remove(entry.Key.ToString());
            }

        }



        [HttpGet]
        public void ClearCache(string input)
        {
            HttpResponse.RemoveOutputCacheItem(input);

        }


        //public void RestartApplication()
        //{
        //    System.IO.File.SetLastWriteTimeUtc(
        //        Server.MapPath("~/web.config"), DateTime.UtcNow);
        //}
        #endregion


        #region GetAllBRNDetails

        [HttpGet]
        public string GetBRNSocietyList()
        {
            try
            {
                CommonServicesModel objCitizenModel = new CommonServicesModel(new DBLogger());
                DataTable dt1 = (objCitizenModel.BRNSocietyList("GetData", 0, "", ""));

                BRNEntity obj = new BRNEntity();
                int i = 0; int societyId = 0; string BRN = ""; string societyName = "";
                if (dt1 != null)
                    foreach (DataRow row in dt1.Rows)
                    {
                        // Access values using column names or indexes
                        //string SocietyID = row["SocietyID"].ToString();
                        societyId = Convert.ToInt32(row["SocietyID"]);
                        BRN = row["BRN"].ToString();

                        obj = COMMON_FUNCTION.GetBRN(BRN);
                        societyName = obj != null ? obj.BO_Name : "";
                        i = Convert.ToInt32(objCitizenModel.BRNSocietyList("InsertData", societyId, BRN, societyName).Rows[0]["Id"]);
                    }

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.Api",
                    FunctionName = "GetBRNSocietyList"
                }, ex);
                return ex.ToString();
            }


            return "true";



        }

        #endregion



        #region Ritesh_Mobile_App_ControllerMethods

        #region  SocietyDashboardData_AsPer_LoginUser   --START--

        [HttpPost]
        public APIResponce GetSocietyDashboardData_UserRoleWise(SocietyDashboard_UserRoleWiseRequestData Request)
        {
            APIResponce response = new APIResponce();
            MobileDashboardModel objSocietyModel = new MobileDashboardModel();

            try
            {
                var result = objSocietyModel.GetSocietyDashboardData_UserRoleWise(
                                Request.UserRoleMappingId,
                                Request.IsOnline,
                                Request.DistrictId,
                                 Request.DivisionId);

                ////if (result != null && result.Count > 0)
                ////{
                ////    response.Status = "Success";
                ////    response.Message = "Success";
                ////    response.Data = result;   // Dynamic JSON
                ////}
                ////else
                ////{
                ////    response.Status = "Success";
                ////    response.Message = "Data Not Found";
                ////    response.Data = null;
                ////} 

                ////// Issue resolved manage the null or 0 rows in Society Data result // by RK 2026-05-01 //
                if (result != null && result.Count > 0 && result.ContainsKey("SocietyData") && result["SocietyData"] != null && ((List<Dictionary<string, object>>)result["SocietyData"]).Count > 0)
                {
                    response.Status = "Success";
                    response.Message = "Success";
                    response.Data = result; // Dynamic JSON
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponce GetSocietyDashboardData_UserRoleWise(SocietyDashboard_UserRoleWiseRequestData Request) ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>               
        ///
        //// Use for API created by developer details manage given class by Dinesh ji.
        ////[DeveloperInfo("Dinesh Joshi", "07-Apr-2026", "Retrieves user list based on provided registration form filters.")]  .        
        [HttpPost]
        public APIResponceWithTotalRec GetSocietyDetails_ActDistrict_UserRoleWise(SocietyDashboard_UserRoleWiseRequestData Request)
        {
            APIResponceWithTotalRec response = new APIResponceWithTotalRec();
            MobileDashboardModel objModel = new MobileDashboardModel();

            try
            {
                if (Request == null)
                {
                    response.Status = "Error";
                    response.Message = "Invalid Request";
                    response.Data = null;
                    response.TotalRecords = 0;
                    return response;
                }

                var ds = objModel.GetSocietyDetailsActDistricUserRoleSearchByNameRegNo(
                            Request.ActId,
                            Request.IsOnline,
                            Request.Status,
                            Request.DistrictId,
                            Request.BlockId,
                            Request.GPId,
                            Request.VillageId,
                            Request.CityId,
                            Request.WardId,
                            Request.DivisionId,
                            Request.IsRuralNotUrban,
                            Request.SocietyTypeId,
                            Request.UserRoleMappingId,
                            Request.SocietyName_RegistrationNo,
                            Request.PageNo,
                            Request.Limit);

                ApiDataSetHelper.ApplyDataSetToResponse(ds, response, Request?.PageNo ?? 1, Request?.Limit ?? 10); // Data Set handle and response manage by Common Method // Ritesh 2026-05-21 //
            }
            catch (Exception ex)
            {
                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;
                response.TotalRecords = 0;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponceWithTotalRec GetSocietyDetails_ActDistrict_UserRoleWise(SocietyDashboard_UserRoleWiseRequestData Request) ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }

        [HttpPost]
        public APIResponceWithTotalRec GetSocietyDetailsActDistricUserRoleSearchByNameRegNo(SocietyDashboard_UserRoleWiseRequestData Request)
        {
            APIResponceWithTotalRec response = new APIResponceWithTotalRec();
            MobileDashboardModel objModel = new MobileDashboardModel();

            try
            {
                if (Request == null)
                {
                    response.Status = "Error";
                    response.Message = "Invalid Request";
                    response.Data = null;
                    response.TotalRecords = 0;
                    return response;
                }
                var ds = objModel.GetSocietyDetailsActDistricUserRoleSearchByNameRegNo(
                            Request.ActId,
                            Request.IsOnline,
                            Request.Status,
                            Request.DistrictId,
                            Request.BlockId,
                            Request.GPId,
                            Request.VillageId,
                            Request.CityId,
                            Request.WardId,
                            Request.DivisionId,
                            Request.IsRuralNotUrban,
                            Request.SocietyTypeId,
                            Request.UserRoleMappingId,
                            Request.SocietyName_RegistrationNo,
                            Request.PageNo,
                            Request.Limit);

                ApiDataSetHelper.ApplyDataSetToResponse(ds, response, Request?.PageNo ?? 1, Request?.Limit ?? 10); // Data Set handle and response manage by Common Method // Ritesh 2026-05-21 //

            }
            catch (Exception ex)
            {
                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;
                response.TotalRecords = 0;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponceWithTotalRec GetSocietyDetailsActDistricUserRoleSearchByNameRegNo(SocietyDashboard_UserRoleWiseRequestData Request) ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }

        [HttpPost]
        public APIResponceWithTotalRec GetSocietyDetails_UserRoleWise_MoreSummary(SocietyDashboard_UserRoleWiseRequestData Request)
        {
            APIResponceWithTotalRec response = new APIResponceWithTotalRec();
            MobileDashboardModel objModel = new MobileDashboardModel();

            try
            {
                var ds = objModel.GetSocietyDetails_UserRoleWise_MoreSummary(
                            Request.SocietyId,
                            Request.SocietySummaryType?.ToUpper(),
                            Request.PageNo,
                            Request.Limit);

                if (ds != null && ds.Tables.Count > 0)
                {
                    //-------------------------------------------------
                    // STEP 1: Get Table Names (Reusable Method)
                    //-------------------------------------------------
                    var tableNames = csCommon.GetTableNamesFromDataSet(ds);

                    //-------------------------------------------------
                    // STEP 2: Remove Last Table (Metadata)
                    //-------------------------------------------------
                    ds.Tables.RemoveAt(ds.Tables.Count - 1);

                    //-------------------------------------------------
                    // STEP 3: SAFETY CHECK (Important)
                    //-------------------------------------------------
                    if (tableNames.Count != ds.Tables.Count)
                    {
                        tableNames = ds.Tables
                                       .Cast<DataTable>()
                                       .Select((t, i) => "Table" + i)
                                       .ToList();
                    }

                    //-------------------------------------------------
                    // STEP 4: Convert DataSet → Dynamic
                    //-------------------------------------------------
                    var data = new Dictionary<string, List<Dictionary<string, object>>>();

                    if (ds.Tables.Count > 1)
                    {
                        data = csCommon.ConvertDataSetToDynamic(ds, tableNames);
                    }
                    else if (ds.Tables.Count == 1)
                    {
                        var tableData = csCommon.ConvertDataTableToDynamic(ds.Tables[0]);

                        var tableName = (tableNames.Count > 0 && !string.IsNullOrEmpty(tableNames[0]))
                                        ? tableNames[0]
                                        : "Data";

                        data.Add(tableName, tableData);
                    }

                    //-------------------------------------------------
                    // STEP 5: TotalRecords
                    //-------------------------------------------------
                    if (data.ContainsKey("TotalCount"))
                    {
                        var totalTable = data["TotalCount"];

                        if (totalTable?.Count > 0 &&
                            totalTable[0].ContainsKey("TotalRecords"))
                        {
                            response.TotalRecords = Convert.ToInt32(totalTable[0]["TotalRecords"] ?? 0);
                        }
                    }
                    else
                    {
                        response.TotalRecords = 0;
                    }

                    //-------------------------------------------------
                    // STEP 6: Default Response Check
                    //-------------------------------------------------
                    var firstTableName = tableNames.FirstOrDefault();

                    if (!string.IsNullOrEmpty(firstTableName) && data.ContainsKey(firstTableName))
                    {
                        var firstTable = data[firstTableName];

                        if (firstTable == null || firstTable.Count == 0)
                        {
                            response.Message = "Data Not Found";
                        }
                        else
                        {
                            response.Message = "Success";
                        }

                        response.Status = "Success";
                    }

                    //-------------------------------------------------
                    // FINAL RESPONSE
                    //-------------------------------------------------
                    response.Data = data;
                    response.PageNumber = Request.PageNo;
                    response.PageSize = Request.Limit;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                    response.TotalRecords = 0;
                }
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(
                    new ErrorLogEntity()
                    {
                        PageName = "ServicesController",
                        ////FunctionName = nameof(GetSocietyDetails_UserRoleWise_MoreSummary)
                        FunctionName = "public APIResponceWithTotalRec GetSocietyDetails_UserRoleWise_MoreSummary(SocietyDashboard_UserRoleWiseRequestData Request) ; Error : " + ex.Message.ToString()
                    }, ex);

                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;
                response.TotalRecords = 0;
            }

            return response;
        }

        [HttpPost]
        public APIResponceWithTotalRec GetSociety_OtherDetails_UserRoleWise(SocietyDashboard_UserRoleWiseRequestData Request)
        {
            APIResponceWithTotalRec response = new APIResponceWithTotalRec();
            MobileDashboardModel objModel = new MobileDashboardModel();

            int SocietyId = 0;
            string SocietyIdEnc = "";
            string OtherDetailsType = "";
            int AmendmentId = 0;
            DataSet ds = new DataSet();

            if (Request != null)
            {
                SocietyId = Request.SocietyId;
                SocietyIdEnc = Request.SocietyIdEnc ?? "";
                OtherDetailsType = Request.OtherDetailsType ?? "ALL";

                if (Request.IsSocietyAmendment)
                {
                    AmendmentId = Request.AmendmentId;
                }
            }
            else
            {
                response.Status = "Error";
                response.Message = "Invalid Request";
                response.Data = null;
                response.TotalRecords = 0;

                ILogger objILogger = new DBLogger();
                Exception exception = null;

                objILogger.ErrorHandle(
                    new ErrorLogEntity()
                    {
                        PageName = "ServicesController",
                        FunctionName = "public APIResponceWithTotalRec GetSociety_OtherDetails_UserRoleWise(SocietyDashboard_UserRoleWiseRequestData Request) ; Error : Request object is null or API parameters were not passed correctly."
                    }, exception);

                return response;
            }

            try
            {
                if (Request.IsSocietyAmendment)
                {
                    ds = objModel.Get_SocietyAmendment1958_OtherDetails_UserRoleWise(
                           SocietyId,
                           SocietyIdEnc,
                           AmendmentId,
                           OtherDetailsType,
                           Request.PageNo,
                           Request.Limit);
                }
                else
                {
                    ds = objModel.GetSociety_OtherDetails_UserRoleWise(
                            SocietyId,
                            SocietyIdEnc,
                            OtherDetailsType,
                            Request.PageNo,
                            Request.Limit);
                }

                if (ds != null && ds.Tables.Count > 0)
                {
                    //-------------------------------------------------
                    // STEP 1: Get Table Names (Reusable Method)
                    //-------------------------------------------------
                    var tableNames = csCommon.GetTableNamesFromDataSet(ds);

                    //-------------------------------------------------
                    // STEP 2: Remove Last Table (Metadata)
                    //-------------------------------------------------
                    ds.Tables.RemoveAt(ds.Tables.Count - 1);

                    //-------------------------------------------------
                    // STEP 3: SAFETY CHECK (Important)
                    //-------------------------------------------------
                    if (tableNames.Count != ds.Tables.Count)
                    {
                        tableNames = ds.Tables
                                       .Cast<DataTable>()
                                       .Select((t, i) => "Table" + i)
                                       .ToList();
                    }

                    //-------------------------------------------------
                    // STEP 4: Convert DataSet → Dynamic
                    //-------------------------------------------------
                    var data = new Dictionary<string, List<Dictionary<string, object>>>();

                    if (ds.Tables.Count > 1)
                    {
                        data = csCommon.ConvertDataSetToDynamic(ds, tableNames);
                    }
                    else if (ds.Tables.Count == 1)
                    {
                        var tableData = csCommon.ConvertDataTableToDynamic(ds.Tables[0]);

                        var tableName = (tableNames.Count > 0 && !string.IsNullOrEmpty(tableNames[0]))
                                        ? tableNames[0]
                                        : "Data";

                        data.Add(tableName, tableData);
                    }

                    //-------------------------------------------------
                    // STEP 5: TotalRecords
                    //-------------------------------------------------
                    if (data.ContainsKey("TotalCount"))
                    {
                        var totalTable = data["TotalCount"];

                        if (totalTable?.Count > 0 &&
                            totalTable[0].ContainsKey("TotalRecords"))
                        {
                            response.TotalRecords = Convert.ToInt32(totalTable[0]["TotalRecords"] ?? 0);
                        }
                    }
                    else
                    {
                        response.TotalRecords = 0;
                    }

                    //-------------------------------------------------
                    // STEP 6: Default Response Check
                    //-------------------------------------------------
                    var firstTableName = tableNames.FirstOrDefault();

                    if (!string.IsNullOrEmpty(firstTableName) && data.ContainsKey(firstTableName))
                    {
                        var firstTable = data[firstTableName];

                        if (firstTable == null || firstTable.Count == 0)
                        {
                            response.Message = "Data Not Found";
                        }
                        else
                        {
                            response.Message = "Success";
                        }

                        response.Status = "Success";
                    }

                    //-------------------------------------------------
                    // FINAL RESPONSE
                    //-------------------------------------------------
                    response.Data = data;
                    response.PageNumber = Request.PageNo;
                    response.PageSize = Request.Limit;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                    response.TotalRecords = 0;
                }
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(
                    new ErrorLogEntity()
                    {
                        PageName = "ServicesController",
                        FunctionName = "public APIResponceWithTotalRec GetSociety_OtherDetails_UserRoleWise(SocietyDashboard_UserRoleWiseRequestData Request) ; Error : " + ex.Message.ToString()
                    }, ex);

                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;
                response.TotalRecords = 0;
            }

            return response;
        }
        #endregion  SocietyDashboardData_AsPer_LoginUser --END----

        #region Master API - Use in Mobile App - Location DDL Fill - Society Details

        [HttpPost]
        public APIResponce Mobile_GetDivisionList(SocietyDashboard_UserRoleWiseRequestData Request)
        {
            MobileDashboardModel objSocietyModel = new MobileDashboardModel();
            APIResponce response = new APIResponce();

            try
            {
                string Status = "";
                int UserRoleMappingId = 0;
                int StateId = 0;

                if (Request != null)
                {
                    UserRoleMappingId = Request.UserRoleMappingId;
                    StateId = Request.StateId;
                    Status = Request.Status ?? "";
                }

                var result = objSocietyModel.DivisionList(UserRoleMappingId, StateId, Status);

                if (result != null && result.Rows.Count > 0)
                {
                    var data = csCommon.ConvertDataTableToDynamic(result);

                    response.Status = "Success";
                    response.Message = "Success";
                    response.Data = data;   // Dynamic JSON Creating for Reposne json type.
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Internal Server Error.";
                response.Data = null;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponce Mobile_GetDivisionList(SocietyDashboard_UserRoleWiseRequestData Request) ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }

        /// <summary>
        /// Created By :    Ritesh
        /// Created On :    2026-04-20
        /// Created For:    Use for Mobile App show District List 
        /// </summary>
        /// <param name="DistrictId"></param>
        /// <returns></returns>
        [HttpPost]
        public APIResponce Mobile_GetDistrictList(SocietyDashboard_UserRoleWiseRequestData Request)
        {
            MobileDashboardModel objSocietyModel = new MobileDashboardModel();
            APIResponce response = new APIResponce();

            try
            {
                string Status = "";
                int UserRoleMappingId = 0;
                int DivisionId = 0;

                if (Request != null)
                {
                    UserRoleMappingId = Request.UserRoleMappingId;
                    DivisionId = Request.DivisionId;
                    Status = Request.Status ?? "";
                }

                var result = objSocietyModel.DistrictList(UserRoleMappingId, DivisionId, Status);

                if (result != null && result.Rows.Count > 0)
                {
                    var data = csCommon.ConvertDataTableToDynamic(result);

                    response.Status = "Success";
                    response.Message = "Success";
                    response.Data = data;   // Dynamic JSON Creating for Reposne json type.
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Internal Server Error.";
                response.Data = null;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponce Mobile_GetDistrictList(SocietyDashboard_UserRoleWiseRequestData Request) ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }

        /// <summary>
        /// Created By :    Ritesh
        /// Created On :    2026-04-20
        /// Created For:    Use for Mobile App show Block List 
        /// </summary>
        /// <param name="DistrictId"></param>
        /// <returns></returns>
        [HttpPost]
        public APIResponce Mobile_GetBlockList(SocietyDashboard_UserRoleWiseRequestData Request)
        {
            MobileDashboardModel objSocietyModel = new MobileDashboardModel();
            APIResponce response = new APIResponce();
            try
            {
                string Status = "";
                int UserRoleMappingId = 0;
                int DistrictId = 0;
                int IsOnline = 0;
                int ActId = 0;

                if (Request != null)
                {
                    UserRoleMappingId = Request.UserRoleMappingId;
                    DistrictId = Request.DistrictId;
                    IsOnline = Request.IsOnline;
                    ActId = Request.SocietyTypeActId;
                    Status = Request.Status ?? "";
                }

                var result = objSocietyModel.BlockList(UserRoleMappingId, DistrictId, IsOnline, ActId, Status);

                if (result != null && result.Rows.Count > 0)
                {
                    var data = csCommon.ConvertDataTableToDynamic(result);
                    response.Status = "Success";
                    response.Message = "Success";
                    response.Data = data;   // Dynamic JSON Creating for Reposne json type.
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Internal Server Error.";
                response.Data = null;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponce Mobile_GetBlockList(SocietyDashboard_UserRoleWiseRequestData Request) ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }

        /// <summary>
        /// Created By :    Ritesh
        /// Created On :    2026-04-20
        /// Created For:    Use for Mobile App show Gram Panchayat List 
        /// </summary>
        /// <param name="BlockId"></param>
        /// <returns></returns>
        [HttpPost]
        public APIResponce Mobile_GramPanchayatList(SocietyDashboard_UserRoleWiseRequestData Request)
        {
            MobileDashboardModel objSocietyModel = new MobileDashboardModel();
            APIResponce response = new APIResponce();

            try
            {
                string Status = "";
                int UserRoleMappingId = 0;
                int BlockId = 0;
                int IsOnline = 0;
                int ActId = 0;

                if (Request != null)
                {
                    UserRoleMappingId = Request.UserRoleMappingId;
                    BlockId = Request.BlockId;
                    IsOnline = Request.IsOnline;
                    ActId = Request.SocietyTypeActId;
                    Status = Request.Status ?? "";
                }

                var result = objSocietyModel.GramPanchayatList(UserRoleMappingId, BlockId, IsOnline, ActId, Status);

                if (result != null && result.Rows.Count > 0)
                {
                    var data = csCommon.ConvertDataTableToDynamic(result);
                    response.Status = "Success";
                    response.Message = "Success";
                    response.Data = data;   // Dynamic JSON Creating for Reposne json type.
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Internal Server Error.";
                response.Data = null;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponce GramPanchayatList(SocietyDashboard_UserRoleWiseRequestData Request) ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }

        /// <summary>
        /// Created By :    Ritesh
        /// Created On :    2026-04-17
        /// Created For:    Use for Mobile App show Village List 
        /// </summary>
        /// <param name="GramPanchayatId"></param>
        /// <returns></returns>
        [HttpPost]
        public APIResponce Mobile_VillageList(SocietyDashboard_UserRoleWiseRequestData Request)
        {
            MobileDashboardModel objSocietyModel = new MobileDashboardModel();
            APIResponce response = new APIResponce();

            try
            {
                string Status = "";
                int UserRoleMappingId = 0;
                int GPId = 0;
                int IsOnline = 0;
                int ActId = 0;

                if (Request != null)
                {
                    UserRoleMappingId = Request.UserRoleMappingId;
                    GPId = Request.GPId;
                    IsOnline = Request.IsOnline;
                    ActId = Request.SocietyTypeActId;
                    Status = Request.Status ?? "";
                }

                var result = objSocietyModel.VillageList(UserRoleMappingId, GPId, IsOnline, ActId, Status);

                if (result != null && result.Rows.Count > 0)
                {
                    var data = csCommon.ConvertDataTableToDynamic(result);
                    response.Status = "Success";
                    response.Message = "Success";
                    response.Data = data;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponce Mobile_VillageList(SocietyDashboard_UserRoleWiseRequestData Request) ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }

        [HttpPost]
        public APIResponce Mobile_CityList(SocietyDashboard_UserRoleWiseRequestData Request)
        {
            MobileDashboardModel objSocietyModel = new MobileDashboardModel();
            APIResponce response = new APIResponce();

            try
            {
                string Status = "";
                int UserRoleMappingId = 0;
                int DistrictId = 0;
                int CityType = 0;
                int IsOnline = 0;
                int ActId = 0;

                if (Request != null)
                {
                    UserRoleMappingId = Request.UserRoleMappingId;
                    DistrictId = Request.DistrictId;
                    CityType = Request.CityType;
                    IsOnline = Request.IsOnline;
                    ActId = Request.SocietyTypeActId;
                    Status = Request.Status ?? "";
                }
                var result = objSocietyModel.CityList(UserRoleMappingId, DistrictId, CityType, IsOnline, ActId, Status);

                if (result != null && result.Rows.Count > 0)
                {
                    var data = csCommon.ConvertDataTableToDynamic(result);
                    response.Status = "Success";
                    response.Message = "Success";
                    response.Data = data;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponce Mobile_CityList(SocietyDashboard_UserRoleWiseRequestData Request) ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }

        [HttpPost]
        public APIResponce Mobile_WardList(SocietyDashboard_UserRoleWiseRequestData Request)
        {
            MobileDashboardModel objSocietyModel = new MobileDashboardModel();
            APIResponce response = new APIResponce();

            try
            {
                string Status = "";
                int UserRoleMappingId = 0;
                int CityId = 0;
                int IsOnline = 0;
                int ActId = 0;

                if (Request != null)
                {
                    UserRoleMappingId = Request.UserRoleMappingId;
                    CityId = Request.CityId;
                    IsOnline = Request.IsOnline;
                    ActId = Request.SocietyTypeActId;
                    Status = Request.Status ?? "";
                }
                var result = objSocietyModel.WardList(UserRoleMappingId, CityId, IsOnline, ActId, Status);

                if (result != null && result.Rows.Count > 0)
                {
                    var data = csCommon.ConvertDataTableToDynamic(result);
                    response.Status = "Success";
                    response.Message = "Success";
                    response.Data = data;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponce Mobile_WardList(SocietyDashboard_UserRoleWiseRequestData Request) ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }

        #endregion Master API - Use in Mobile App - Location DDL Fill - Society Details        

        #region     Master API - Filter Use in Mobile App
        [HttpGet]
        public APIResponce GetSocietyTypeList()
        {
            MobileDashboardModel objSocietyModel = new MobileDashboardModel();
            APIResponce response = new APIResponce();

            try
            {
                var result = objSocietyModel.GetSocietyTypeList();

                if (result != null && result.Rows.Count > 0)
                {
                    var data = csCommon.ConvertDataTableToDynamic(result);
                    response.Status = "Success";
                    response.Message = "Success";
                    response.Data = data;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Internal Server Error.";
                response.Data = null;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponce GetSocietyTypeList() ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }

        [HttpPost]
        public APIResponce GetSocietyTypeList_ActWise(SocietyDashboard_UserRoleWiseRequestData Request)
        {
            MobileDashboardModel objSocietyModel = new MobileDashboardModel();
            APIResponce response = new APIResponce();

            try
            {
                var result = objSocietyModel.GetSocietyTypeList_ActWise(Request.SocietyTypeActId);

                if (result != null && result.Rows.Count > 0)
                {
                    var data = csCommon.ConvertDataTableToDynamic(result);
                    response.Status = "Success";
                    response.Message = "Success";
                    response.Data = data;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Internal Server Error.";
                response.Data = null;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponce GetSocietyTypeList() ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }
        #endregion  Master API - Filter Use in Mobile App

        #region     Mobile APP Login API With Role details
        [HttpPost]
        public APIResponce Mobile_LoginBySSOId_GetRole(APILoginRequest Request)
        {
            //Mobile_LoginEntity obj = new Mobile_LoginEntity();
            Mobile_LoginEntity_Citizen obj = new Mobile_LoginEntity_Citizen();

            APIResponce response = new APIResponce();

            try
            {
                SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
                var isverified = ssoclient.SSOAuthentication(Request.SSOID, Request.Password);

                //// Invalid Login                
                //if (!isverified && Request.Password != "P@ssw0rd#2026")
                //{
                //    response.Status = "Error";
                //    response.Message = "Invalid Username / Password";
                //    response.Data = null;
                //    return response;
                //}

                if (Request.LoginUserType == 1)// DEPARTMENT USER // P@ssw0rd#2026 // Coop#2026dpt // 
                {
                    if (!isverified && Request.Password != "Coop#2026dpt")
                    {
                        response.Status = "Error";
                        response.Message = "Invalid Username / Password as Department Login";
                        response.Data = null;
                        return response;
                    }
                }
                else if (Request.LoginUserType == 2) // CITIZEN USER // Coop#2026ctz //
                {
                    if (!isverified && Request.Password != "Coop#2026ctz")
                    {
                        response.Status = "Error";
                        response.Message = "Invalid Username / Password  as Citizen Login";
                        response.Data = null;
                        return response;
                    }
                }
                else
                {
                    response.Status = "Error";
                    response.Message = "Invalid Login User Type";
                    response.Data = null;
                    return response;
                }
                //// Valid Login                
                var res = ssoclient.GetUserDetailJSON(Request.SSOID, "RAJSAHKAR.TEST", "Rajasthan@12");
                var usrdtl2 = JsonConvert.DeserializeObject<SSO_UserProfileDetailModel>(res);

                if (usrdtl2 == null)
                {
                    response.Status = "Error";
                    response.Message = "User Details Not Found";
                    response.Data = null;
                    return response;
                }

                UserModel objUserModel = new UserModel(new DBLogger());

                //// CITIZEN USER
                //if (usrdtl2.userType == "CITIZEN")
                //{
                //    obj.IsUserExists = true;
                //    obj.OfficerNameEn = usrdtl2.displayName;
                //    obj.OfficerNameHi = usrdtl2.displayName;
                //    obj.SSO_ID = usrdtl2.SSOID;
                //    obj.Gender = usrdtl2.gender;
                //    obj.UserType = 2;
                //    obj.UserId = 60;
                //    obj.UserTypeName = usrdtl2.userType;

                //    obj.Roles_En.Add("CITIZEN");
                //    obj.Roles_Hi.Add("नागरिक");
                //    obj.RoleId.Add(usrdtl2.RoleId);
                //    obj.ActId.Add(usrdtl2.ActId);

                //    response.Status = "Success";
                //    response.Message = "Login Successful";
                //    response.Data = obj;

                //    return response;
                //}

                if (Request.LoginUserType == 2)
                {
                    if (usrdtl2.userType == "CITIZEN")
                    {
                        obj.IsUserExists = true;
                        obj.UserId = 60;
                        obj.SSO_ID = usrdtl2.SSOID ?? string.Empty;

                        obj.Gender = usrdtl2.gender ?? string.Empty;

                        obj.OfficerNameEn = usrdtl2.displayName ?? string.Empty;
                        obj.OfficerNameHi = (usrdtl2.displayName ?? "").Replace("null", "").Trim();

                        obj.RoleId = usrdtl2.RoleId;
                        obj.RoleName_EN = "CITIZEN";
                        obj.RoleName_HI = "नागरिक";

                        //obj.ActId = usrdtl2.ActId ?? "1,3,7";
                        obj.ActId = usrdtl2.ActId ?? string.Empty;

                        obj.OfficeId = usrdtl2.OfficeId;
                        obj.UserRoleMappingId = 0;
                        obj.UserLevelId = 0;

                        obj.DistrictId = usrdtl2.DistrictId;
                        obj.DISTRICT_ENG = "";
                        obj.DISTRICT_MANGAL = "";

                        obj.UserType = 2; // CITIZEN

                        var lstdata = new List<Mobile_LoginEntity_Citizen>();
                        lstdata.Add(obj);

                        response.Status = "Success";
                        response.Message = "Login Successful";
                        response.Data = lstdata;

                        return response;
                    }
                    else {
                        MobileDashboardModel objMobileDashboardModel = new MobileDashboardModel(new DBLogger());
                        DataTable result = objMobileDashboardModel.Mobile_IsSocietywithSSOId(Request.SSOID);
                        if (result.Rows.Count > 0 && Convert.ToInt32(result.Rows[0][0]) > 0)
                        {
                            obj.IsUserExists = true;
                            obj.UserId = 60;
                            obj.SSO_ID = usrdtl2.SSOID ?? string.Empty;

                            obj.Gender = usrdtl2.gender ?? string.Empty;

                            obj.OfficerNameEn = usrdtl2.displayName ?? string.Empty;
                            obj.OfficerNameHi = (usrdtl2.displayName ?? "").Replace("null", "").Trim();

                            obj.RoleId = usrdtl2.RoleId;
                            obj.RoleName_EN = "CITIZEN";
                            obj.RoleName_HI = "नागरिक";

                            //obj.ActId = usrdtl2.ActId ?? "1,3,7";
                            obj.ActId = usrdtl2.ActId ?? string.Empty;

                            obj.OfficeId = usrdtl2.OfficeId;
                            obj.UserRoleMappingId = 0;
                            obj.UserLevelId = 0;

                            obj.DistrictId = usrdtl2.DistrictId;
                            obj.DISTRICT_ENG = "";
                            obj.DISTRICT_MANGAL = "";

                            obj.UserType = 2; // CITIZEN

                            var lstdata = new List<Mobile_LoginEntity_Citizen>();
                            lstdata.Add(obj);

                            response.Status = "Success";
                            response.Message = "Login Successful";
                            response.Data = lstdata;

                            return response;

                        }
                        else
                        {
                            response.Status = "Success";
                            response.Message = "User Not Found";
                            response.Data = null;
                            return response;
                        }
                        

                    }
                }

                else if (Request.LoginUserType == 1 && usrdtl2.userType != "CITIZEN")
                {
                    //// NON-CITIZEN USER                
                    //DataTable result = objUserModel.Mobile_LoginBySSOId(Request.SSOID);
                    MobileDashboardModel objMobileDashboardModel = new MobileDashboardModel(new DBLogger());
                    DataTable result = objMobileDashboardModel.Mobile_LoginBySSOId(Request.SSOID);

                    if (result == null || result.Rows.Count == 0)
                    {
                        response.Status = "Success";
                        response.Message = "User Not Found";
                        response.Data = null;
                        return response;
                    }

                    var isUserExists = result.AsEnumerable()
                                            .Select(p => p.Field<int>("IsUserExists"))
                                            .FirstOrDefault();

                    if (isUserExists == 0)
                    {
                        response.Status = "Success";
                        response.Message = "User Not Found";
                        response.Data = null;
                        return response;
                    }
                    var data = csCommon.ConvertDataTableToDynamic(result);

                    response.Status = "Success";
                    response.Message = "Login Successful";
                    response.Data = data;

                    return response;
                }
                else
                {
                    response.Status = "Error";
                    response.Message = "Login User Type Not Correct!";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.CommonServices",
                    FunctionName = "public APIResponce Mobile_LoginBySSOId_GetRole(APILoginRequest Request)"
                }, ex);

                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;
            }

            return response;
        }
        #endregion  Mobile APP Login API With Role details

        #region     SocietyDashboardData_AsPer_CitizenUserLogin    --START--

        [HttpPost]
        public APIResponce GetSocietyDashboardData_UserCitizenSSOIDWise(SocietyDashboard_UserCitizenSSOIDWiseRequestData Request)
        {
            APIResponce response = new APIResponce();
            MobileDashboardModel objSocietyModel = new MobileDashboardModel();

            try
            {
                var result = objSocietyModel.GetSocietyDashboardData_UserCitizenSSOIDWise(
                                Request.SSOID,
                                Request.IsOnline,
                                Request.DistrictId,
                                Request.DivisionId
                                );

                if (result != null && result.Count > 0 && result.ContainsKey("SocietyData") && result["SocietyData"] != null && ((List<Dictionary<string, object>>)result["SocietyData"]).Count > 0)
                {
                    response.Status = "Success";
                    response.Message = "Success";
                    response.Data = result; // Dynamic JSON
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;
                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponce GetSocietyDashboardData_UserCitizenSSOIDWise(SocietyDashboard_UserCitizenSSOIDWiseRequestData Request) ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }

        [HttpPost]
        public APIResponceWithTotalRec GetSocietyDetails_ActWise_UserCitizenSSOIDWise(SocietyDashboard_UserCitizenSSOIDWiseRequestData Request)
        {
            APIResponceWithTotalRec response = new APIResponceWithTotalRec();
            MobileDashboardModel objModel = new MobileDashboardModel();

            try
            {
                if (Request == null)
                {
                    response.Status = "Error";
                    response.Message = "Invalid Request";
                    response.Data = null;
                    response.TotalRecords = 0;
                    return response;
                }

                var ds = objModel.GetSocietyDetailsActWiseUserCitizenSSOIDSearchByNameRegNo(
                            Request.SSOID,
                            Request.ActId,
                            Request.IsOnline,
                            Request.Status,

                            Request.DivisionId,
                            Request.DistrictId,
                            Request.BlockId,
                            Request.GPId,
                            Request.VillageId,

                            Request.CityId,
                            Request.WardId,

                            Request.IsRuralNotUrban,
                            Request.SocietyTypeId,
                            Request.SocietyName_RegistrationNo,

                            Request.PageNo,
                            Request.Limit);

                ApiDataSetHelper.ApplyDataSetToResponse(ds, response, Request?.PageNo ?? 1, Request?.Limit ?? 10); // Data Set handle and response manage by Common Method // Ritesh 2026-05-21 //
            }
            catch (Exception ex)
            {
                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;
                response.TotalRecords = 0;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = " public APIResponceWithTotalRec GetSocietyDetails_ActWise_UserCitizenSSOIDWise(SocietyDashboard_UserCitizenSSOIDWiseRequestData Request) ; Error : " + ex.Message
                }, ex);
            }

            return response;
        }

        [HttpPost]
        public APIResponceWithTotalRec GetSocietyDetails_ActWise_UserCitizenSSOIDWise_SearchByNameRegNo(SocietyDashboard_UserCitizenSSOIDWiseRequestData Request)
        {
            APIResponceWithTotalRec response = new APIResponceWithTotalRec();
            MobileDashboardModel objModel = new MobileDashboardModel();

            try
            {
                if (Request == null)
                {
                    response.Status = "Error";
                    response.Message = "Invalid Request";
                    response.Data = null;
                    response.TotalRecords = 0;
                    return response;
                }

                var ds = objModel.GetSocietyDetailsActWiseUserCitizenSSOIDSearchByNameRegNo(
                            Request.SSOID,
                            Request.ActId,
                            Request.IsOnline,
                            Request.Status,

                            Request.DivisionId,
                            Request.DistrictId,
                            Request.BlockId,
                            Request.GPId,
                            Request.VillageId,

                            Request.CityId,
                            Request.WardId,

                            Request.IsRuralNotUrban,
                            Request.SocietyTypeId,
                            Request.SocietyName_RegistrationNo,

                            Request.PageNo,
                            Request.Limit);

                ApiDataSetHelper.ApplyDataSetToResponse(ds, response, Request?.PageNo ?? 1, Request?.Limit ?? 10); // Data Set handle and response manage by Common Method // Ritesh 2026-05-21 //
            }
            catch (Exception ex)
            {
                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;
                response.TotalRecords = 0;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponceWithTotalRec GetSocietyDetails_ActWise_UserCitizenSSOIDWise_SearchByNameRegNo(SocietyDashboard_UserCitizenSSOIDWiseRequestData Request) ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }

        [HttpPost]
        public APIResponceWithTotalRec GetSocietyDetails_UserCitizenSSOIDWise_MoreSummary(SocietyDashboard_UserRoleWiseRequestData Request)
        {
            APIResponceWithTotalRec response = new APIResponceWithTotalRec();
            MobileDashboardModel objModel = new MobileDashboardModel();

            try
            {
                var ds = objModel.GetSocietyDetails_UserRoleWise_MoreSummary(
                            Request.SocietyId,
                            Request.SocietySummaryType?.ToUpper(),
                            Request.PageNo,
                            Request.Limit);

                if (ds != null && ds.Tables.Count > 0)
                {
                    //-------------------------------------------------
                    // STEP 1: Get Table Names (Reusable Method)
                    //-------------------------------------------------
                    var tableNames = csCommon.GetTableNamesFromDataSet(ds);

                    //-------------------------------------------------
                    // STEP 2: Remove Last Table (Metadata)
                    //-------------------------------------------------
                    ds.Tables.RemoveAt(ds.Tables.Count - 1);

                    //-------------------------------------------------
                    // STEP 3: SAFETY CHECK (Important)
                    //-------------------------------------------------
                    if (tableNames.Count != ds.Tables.Count)
                    {
                        tableNames = ds.Tables
                                       .Cast<DataTable>()
                                       .Select((t, i) => "Table" + i)
                                       .ToList();
                    }

                    //-------------------------------------------------
                    // STEP 4: Convert DataSet → Dynamic
                    //-------------------------------------------------
                    var data = new Dictionary<string, List<Dictionary<string, object>>>();

                    if (ds.Tables.Count > 1)
                    {
                        data = csCommon.ConvertDataSetToDynamic(ds, tableNames);
                    }
                    else if (ds.Tables.Count == 1)
                    {
                        var tableData = csCommon.ConvertDataTableToDynamic(ds.Tables[0]);

                        var tableName = (tableNames.Count > 0 && !string.IsNullOrEmpty(tableNames[0]))
                                        ? tableNames[0]
                                        : "Data";

                        data.Add(tableName, tableData);
                    }

                    //-------------------------------------------------
                    // STEP 5: TotalRecords
                    //-------------------------------------------------
                    if (data.ContainsKey("TotalCount"))
                    {
                        var totalTable = data["TotalCount"];

                        if (totalTable?.Count > 0 &&
                            totalTable[0].ContainsKey("TotalRecords"))
                        {
                            response.TotalRecords = Convert.ToInt32(totalTable[0]["TotalRecords"] ?? 0);
                        }
                    }
                    else
                    {
                        response.TotalRecords = 0;
                    }

                    //-------------------------------------------------
                    // STEP 6: Default Response Check
                    //-------------------------------------------------
                    var firstTableName = tableNames.FirstOrDefault();

                    if (!string.IsNullOrEmpty(firstTableName) && data.ContainsKey(firstTableName))
                    {
                        var firstTable = data[firstTableName];

                        if (firstTable == null || firstTable.Count == 0)
                        {
                            response.Message = "Data Not Found";
                        }
                        else
                        {
                            response.Message = "Success";
                        }

                        response.Status = "Success";
                    }

                    //-------------------------------------------------
                    // FINAL RESPONSE
                    //-------------------------------------------------
                    response.Data = data;
                    response.PageNumber = Request.PageNo;
                    response.PageSize = Request.Limit;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                    response.TotalRecords = 0;
                }
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(
                    new ErrorLogEntity()
                    {
                        PageName = "ServicesController",
                        FunctionName = nameof(GetSocietyDetails_UserRoleWise_MoreSummary)
                    }, ex);

                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;
                response.TotalRecords = 0;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponceWithTotalRec GetSocietyDetails_UserCitizenSSOIDWise_MoreSummary(SocietyDashboard_UserRoleWiseRequestData Request) ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }

        #endregion  SocietyDashboardData_AsPer_CitizenUserLogin     --END----

        #region         CitizenCorner_AsPer_WebSite             --START--

        #region         CitizenCorner_Master_API_Mobile_App     --START--

        [HttpPost]
        public APIResponce CitizenCorner_TabsList()
        {
            MobileDashboardModel objModel = new MobileDashboardModel();
            APIResponce response = new APIResponce();

            try
            {
                var result = objModel.CitizenCorner_TabsList();

                if (result != null && result.Rows.Count > 0)
                {
                    var data = csCommon.ConvertDataTableToDynamic(result);
                    response.Status = "Success";
                    response.Message = "Success";
                    response.Data = data;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Internal Server Error.";
                response.Data = null;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponce CitizenCorner_TabsList() ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }

        [HttpPost]
        public APIResponce AuditFinancialYear()
        {
            MobileDashboardModel objModel = new MobileDashboardModel();
            APIResponce response = new APIResponce();

            //return COMMON_FUNCTION.DataTableToJSONString(objModel.AuditFinancialYear());

            try
            {
                var result = objModel.AuditFinancialYear();

                if (result != null && result.Rows.Count > 0)
                {
                    var data = csCommon.ConvertDataTableToDynamic(result);
                    response.Status = "Success";
                    response.Message = "Success";
                    response.Data = data;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Internal Server Error.";
                response.Data = null;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponce AuditFinancialYear() ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }
        #endregion     CitizenCorner_Master_API_Mobile_App     --END----

        #region         CitizenCorner_Tabs_Internal_Working     --START--        

        [HttpPost]
        public APIResponce Get_SOCIETY_REG_STATUS(CitizenCornerRequestData Request)
        {
            MobileDashboardModel objModel = new MobileDashboardModel();
            APIResponce response = new APIResponce();

            try
            {
                var result = objModel.SearchSociety(Request.SearchValue.Replace(" ", ""), Request.SearchType.ToString().Trim(), Request.PageNo, Request.Limit);

                if (result != null && result.Rows.Count > 0)
                {
                    var data = csCommon.ConvertDataTableToDynamic(result);

                    response.Status = "Success";
                    response.Message = "Success";
                    response.Data = data;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Internal Server Error.";
                response.Data = null;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponce Get_SOCIETY_REG_STATUS(CitizenCornerRequestData Request)"
                }, ex);
            }

            return response;
        }

        [HttpPost]
        public APIResponce Get_GCC_APPLICATION_STATUS(CitizenCornerRequestData Request)
        {
            MobileDashboardModel objModel = new MobileDashboardModel();
            APIResponce response = new APIResponce();

            try
            {
                string JanAadhaarNo = Request.SearchValue.Replace(" ", "");
                string JanAadhaarDec = "";

                //if (!string.IsNullOrEmpty(JanAadhaarNo))
                //{
                //    JanAadhaarNo = "Encrypt:" + JanAadhaarNo;
                //    JanAadhaarDec = csCommon.Decrypt_SA(Uri.UnescapeDataString(JanAadhaarNo));
                //}
                //////DataTable result = objModel.GetGccApplicationStatus(COMMON_FUNCTION.Encrypt(JanAadhaarDec));

                DataTable result = objModel.GetGccApplicationStatus(JanAadhaarDec, Request.SearchValue, Request.PageNo, Request.Limit);

                if (result != null && result.Rows.Count > 0)
                {
                    var data = csCommon.ConvertDataTableToDynamic(result);

                    response.Status = "Success";
                    response.Message = "Success";
                    response.Data = data;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Internal Server Error.";
                response.Data = null;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponce Get_GCC_APPLICATION_STATUS(CitizenCornerRequestData Request)"
                }, ex);
            }
            return response;
        }

        [HttpPost]
        public APIResponce Get_CM_KISAN_SAMMAN_NIDHI_SCHEME_STATUS(CitizenCornerRequestData Request)
        {
            APIResponce response = new APIResponce();
            MobileDashboardModel objModel = new MobileDashboardModel();

            try
            {
                //-------------------------------------------------
                // GET DATASET
                //-------------------------------------------------
                var ds = objModel.GetFarmerBeneficiaryDetails(
                            Request.DistrictId,
                            Request.SearchValue,
                            Request.PageNo,
                            Request.Limit);

                if (ds != null && ds.Tables.Count > 0)
                {
                    //-------------------------------------------------
                    // STEP 1: Get Table Names
                    //-------------------------------------------------
                    var tableNames = csCommon.GetTableNamesFromDataSet(ds);

                    //-------------------------------------------------
                    // STEP 2: Remove Last Table (Metadata/TotalCount)
                    //-------------------------------------------------
                    if (ds.Tables.Count > 1)
                    {
                        ds.Tables.RemoveAt(ds.Tables.Count - 1);
                    }

                    //-------------------------------------------------
                    // STEP 3: Safety Check
                    //-------------------------------------------------
                    if (tableNames.Count != ds.Tables.Count)
                    {
                        tableNames = ds.Tables
                                       .Cast<DataTable>()
                                       .Select((t, i) => "Table" + i)
                                       .ToList();
                    }

                    //-------------------------------------------------
                    // STEP 4: Convert DataSet → Dynamic
                    //-------------------------------------------------
                    var data = new Dictionary<string, List<Dictionary<string, object>>>();

                    if (ds.Tables.Count > 1)
                    {
                        data = csCommon.ConvertDataSetToDynamic(ds, tableNames);
                    }
                    else if (ds.Tables.Count == 1)
                    {
                        var tableData = csCommon.ConvertDataTableToDynamic(ds.Tables[0]);

                        var tableName = (tableNames.Count > 0 && !string.IsNullOrEmpty(tableNames[0]))
                                        ? tableNames[0]
                                        : "Data";

                        data.Add(tableName, tableData);
                    }

                    //-------------------------------------------------
                    // STEP 5: Data Validation
                    //-------------------------------------------------
                    var firstTableName = data.Keys.FirstOrDefault();

                    if (!string.IsNullOrEmpty(firstTableName) &&
                        data.ContainsKey(firstTableName))
                    {
                        var firstTable = data[firstTableName];

                        if (firstTable == null || firstTable.Count == 0)
                        {
                            response.Status = "Success";
                            response.Message = "Data Not Found";
                        }
                        else
                        {
                            response.Status = "Success";
                            response.Message = "Success";
                        }
                    }
                    else
                    {
                        response.Status = "Success";
                        response.Message = "Data Not Found";
                    }

                    //-------------------------------------------------
                    // FINAL RESPONSE
                    //-------------------------------------------------
                    response.Data = data;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponceWithTotalRec Get_CM_KISAN_SAMMAN_NIDHI_SCHEME_STATUS(CitizenCornerRequestData Request) ; Error : " + ex.Message
                }, ex);
            }

            return response;
        }

        [HttpPost]
        public APIResponce Get_SOCIETIES_AUDIT_REPORT(CitizenCornerRequestData Request)
        {
            APIResponce response = new APIResponce();

            MobileDashboardModel objModel = new MobileDashboardModel();

            try
            {
                //-------------------------------------------------
                // GET DATASET
                //-------------------------------------------------

                DataSet ds = objModel.GetSocietiesAuditReport(Request.SocietyTypeId, Request.IsRuralNotUrban, Request.DistrictId, Request.CityId, Request.BlockId, Request.GPId, Request.SearchValue, Request.FinancialYearId, Request.PageNo, Request.Limit);

                //-------------------------------------------------
                // NULL CHECK
                //-------------------------------------------------

                if (ds != null && ds.Tables.Count > 0)
                {
                    //-------------------------------------------------
                    // STEP 0 : SP ERROR CHECK
                    //-------------------------------------------------

                    if (ds.Tables[0].Columns.Contains("ErrorCode"))
                    {
                        response.Status = "Success";
                        response.Message = "Error";

                        response.Data =
                            csCommon.ConvertDataTableToDynamic(ds.Tables[0]);

                        return response;
                    }

                    //-------------------------------------------------
                    // STEP 1 : GET TABLE NAMES
                    //-------------------------------------------------

                    var tableNames = csCommon.GetTableNamesFromDataSet(ds);

                    //-------------------------------------------------
                    // STEP 2 : REMOVE METADATA TABLE
                    //-------------------------------------------------

                    for (int i = ds.Tables.Count - 1; i >= 0; i--)
                    {
                        if (ds.Tables[i].Columns.Contains("TableName"))
                        {
                            ds.Tables.RemoveAt(i);
                        }
                    }

                    //-------------------------------------------------
                    // STEP 3 : SAFETY CHECK
                    //-------------------------------------------------

                    if (tableNames.Count != ds.Tables.Count)
                    {
                        tableNames = ds.Tables
                                       .Cast<DataTable>()
                                       .Select((t, i) => "Table" + i)
                                       .ToList();
                    }

                    //-------------------------------------------------
                    // STEP 4 : CONVERT DATASET TO DYNAMIC
                    //-------------------------------------------------

                    var data = new Dictionary<string, object>();

                    //-------------------------------------------------
                    // MULTIPLE TABLES
                    //-------------------------------------------------

                    if (ds.Tables.Count > 1)
                    {
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            string tableName =
                                (tableNames.Count > i &&
                                 !string.IsNullOrEmpty(tableNames[i]))
                                ? tableNames[i]
                                : "Table" + i;

                            data.Add(
                                tableName,
                                csCommon.ConvertDataTableToDynamic(ds.Tables[i]));
                        }
                    }

                    //-------------------------------------------------
                    // SINGLE TABLE
                    //-------------------------------------------------

                    else if (ds.Tables.Count == 1)
                    {
                        string tableName =
                            (tableNames.Count > 0 &&
                             !string.IsNullOrEmpty(tableNames[0]))
                            ? tableNames[0]
                            : "Data";

                        data.Add(
                            tableName,
                            csCommon.ConvertDataTableToDynamic(ds.Tables[0]));
                    }

                    //-------------------------------------------------
                    // STEP 5 : DATA VALIDATION
                    //-------------------------------------------------

                    if (data.Count > 0)
                    {
                        var firstTable = data.FirstOrDefault().Value
                                         as List<Dictionary<string, object>>;

                        if (firstTable != null && firstTable.Count > 0)
                        {
                            response.Status = "Success";
                            response.Message = "Success";
                            response.Data = data;
                        }
                        else
                        {
                            response.Status = "Success";
                            response.Message = "Data Not Found";
                            response.Data = null;
                        }
                    }
                    else
                    {
                        response.Status = "Success";
                        response.Message = "Data Not Found";
                        response.Data = null;
                    }
                }
                else
                {
                    //-------------------------------------------------
                    // NO DATASET
                    //-------------------------------------------------

                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                //-------------------------------------------------
                // EXCEPTION HANDLE
                //-------------------------------------------------

                response.Status = "500";
                response.Message = "Internal Server Error.";
                response.Data = null;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",

                    FunctionName =
                    "public APIResponce Get_SOCIETIES_AUDIT_REPORT(CitizenCornerRequestData Request)"
                }, ex);
            }

            return response;
        }

        [HttpPost]
        public APIResponceWithTotalRec GetSocietyDetails_CitizenCorner_Search(CitizenCornerRequestData Request)
        {
            APIResponceWithTotalRec response = new APIResponceWithTotalRec();
            MobileDashboardModel objModel = new MobileDashboardModel();

            try
            {
                var ds = objModel.GetSocietyDetails_CitizenCorner_SearchByNameRegNo(

                            Request.SearchType,
                            Request.SearchValue,
                            Request.Status,

                            Request.SocietyTypeId,
                            Request.ActId,

                            Request.PageNo,
                            Request.Limit);

                // MAIN DATA
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var dt = ds.Tables[0];

                    var data = csCommon.ConvertDataTableToDynamic(ds.Tables[0]);

                    // Check default row (SocietyID = 0)
                    if (data.Count == 1 && data[0].ContainsKey("SocietyID") && data[0]["SocietyID"] != null && Convert.ToInt32(data[0]["SocietyID"]) == 0)
                    {
                        response.Status = "Success";
                        response.Message = "Data Not Found";
                        response.Data = data;
                    }
                    else if (data.Count == 1 && data[0].ContainsKey("ErrorCode") && data[0]["ErrorCode"] != null && Convert.ToString(data[0]["ErrorCode"]) != "") ////Check if SP returned error message instead of data
                    {
                        response.Status = "Success";
                        response.Message = "Data Not Found. Some Error Occured in records!";
                        response.Data = data;
                    }
                    else
                    {
                        response.Status = "Success";
                        response.Message = "Success";
                        response.Data = data;
                    }

                    // TOTAL RECORDS
                    if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                    {
                        response.TotalRecords = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalRecords"]);
                    }

                    response.PageNumber = Request.PageNo;
                    response.PageSize = Request.Limit;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                    response.TotalRecords = 0;
                }
            }
            catch (Exception ex)
            {
                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;
                response.TotalRecords = 0;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponceWithTotalRec GetSocietyDetails_CitizenCorner_Search(CitizenCornerRequestData Request) ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }
        #endregion      CitizenCorner_Tabs_Internal_Working     --END----

        #endregion     CitizenCorner_AsPer_WebSite             --END----

        #region     SocietyAmendment1958_DashboardData_AsPer_LoginUser   --START--

        [HttpPost]
        public APIResponce GetSocietyAmendment1958_DashboardCount_UserRoleWise(SocietyDashboard_UserRoleWiseRequestData Request)
        {
            APIResponce response = new APIResponce();
            MobileDashboardModel objSocietyModel = new MobileDashboardModel();

            try
            {
                var result = objSocietyModel.Get_SocietyAmendment1958_DashboardCount_StatusWise_UserRoleWise(
                                Request.UserRoleMappingId,
                                Request.DistrictId,
                                Request.DivisionId);

                if (result != null && result.Rows.Count > 0)
                {
                    var data = csCommon.ConvertDataTableToDynamic(result);
                    response.Status = "Success";
                    response.Message = "Success";
                    response.Data = data;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponce GetSocietyAmendment1958_DashboardCount_UserRoleWise(SocietyDashboard_UserRoleWiseRequestData Request) ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }

        [HttpPost]
        public APIResponceWithTotalRec GetSocietyAmendment1958_Details_UserRoleWise(SocietyDashboard_UserRoleWiseRequestData Request)
        {
            APIResponceWithTotalRec response = new APIResponceWithTotalRec();
            MobileDashboardModel objModel = new MobileDashboardModel();

            try
            {
                if (Request == null)
                {
                    response.Status = "Error";
                    response.Message = "Invalid Request";
                    response.Data = null;
                    response.TotalRecords = 0;
                    return response;
                }

                var ds = objModel.Get_SocietyAmendment1958_Details_UserRoleWise_SearchByNameRegNo(
                            Request.Status,
                            Request.DistrictId,
                            Request.BlockId,
                            Request.GPId,
                            Request.VillageId,
                            Request.CityId,
                            Request.WardId,
                            Request.DivisionId,
                            Request.IsRuralNotUrban,
                            Request.SocietyTypeId,
                            Request.UserRoleMappingId,
                            Request.SocietyName_RegistrationNo,
                            Request.PageNo,
                            Request.Limit);

                ApiDataSetHelper.ApplyDataSetToResponse(ds, response, Request?.PageNo ?? 1, Request?.Limit ?? 10); // Data Set handle and response manage by Common Method // Ritesh 2026-05-21 //
            }
            catch (Exception ex)
            {
                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;
                response.TotalRecords = 0;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = " public APIResponceWithTotalRec GetSocietyAmendment1958_Details_UserRoleWise(SocietyDashboard_UserRoleWiseRequestData Request) ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }

        [HttpPost]
        public APIResponceWithTotalRec GetSocietyAmendment1958_Details_UserRoleWise_SearchByNameRegNo(SocietyDashboard_UserRoleWiseRequestData Request)
        {
            APIResponceWithTotalRec response = new APIResponceWithTotalRec();
            MobileDashboardModel objModel = new MobileDashboardModel();

            try
            {
                if (Request == null)
                {
                    response.Status = "Error";
                    response.Message = "Invalid Request";
                    response.Data = null;
                    response.TotalRecords = 0;
                    return response;
                }
                var ds = objModel.Get_SocietyAmendment1958_Details_UserRoleWise_SearchByNameRegNo(
                            Request.Status,
                            Request.DistrictId,
                            Request.BlockId,
                            Request.GPId,
                            Request.VillageId,
                            Request.CityId,
                            Request.WardId,
                            Request.DivisionId,
                            Request.IsRuralNotUrban,
                            Request.SocietyTypeId,
                            Request.UserRoleMappingId,
                            Request.SocietyName_RegistrationNo,
                            Request.PageNo,
                            Request.Limit);

                ApiDataSetHelper.ApplyDataSetToResponse(ds, response, Request?.PageNo ?? 1, Request?.Limit ?? 10); // Data Set handle and response manage by Common Method // Ritesh 2026-05-21 //
            }
            catch (Exception ex)
            {
                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;
                response.TotalRecords = 0;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponceWithTotalRec GetSocietyAmendment1958_Details_UserRoleWise_SearchByNameRegNo(SocietyDashboard_UserRoleWiseRequestData Request) ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }

        [HttpPost]
        public APIResponceWithTotalRec GetSocietyAmendment1958_MoreSummary_UserRoleWise(SocietyDashboard_UserRoleWiseRequestData Request)
        {
            APIResponceWithTotalRec response = new APIResponceWithTotalRec();
            MobileDashboardModel objModel = new MobileDashboardModel();

            try
            {
                var ds = objModel.Get_SocietyAmendment1958_MoreSummary_UserRoleWise(
                            Request.SocietyId,
                            Request.SocietySummaryType?.ToUpper(),
                            Request.AmendmentId,
                            Request.PageNo,
                            Request.Limit);

                if (ds != null && ds.Tables.Count > 0)
                {
                    //-------------------------------------------------
                    // STEP 1: Get Table Names (Reusable Method)
                    //-------------------------------------------------
                    var tableNames = csCommon.GetTableNamesFromDataSet(ds);

                    //-------------------------------------------------
                    // STEP 2: Remove Last Table (Metadata)
                    //-------------------------------------------------
                    ds.Tables.RemoveAt(ds.Tables.Count - 1);

                    //-------------------------------------------------
                    // STEP 3: SAFETY CHECK (Important)
                    //-------------------------------------------------
                    if (tableNames.Count != ds.Tables.Count)
                    {
                        tableNames = ds.Tables
                                       .Cast<DataTable>()
                                       .Select((t, i) => "Table" + i)
                                       .ToList();
                    }

                    //-------------------------------------------------
                    // STEP 4: Convert DataSet → Dynamic
                    //-------------------------------------------------
                    var data = new Dictionary<string, List<Dictionary<string, object>>>();

                    if (ds.Tables.Count > 1)
                    {
                        data = csCommon.ConvertDataSetToDynamic(ds, tableNames);
                    }
                    else if (ds.Tables.Count == 1)
                    {
                        var tableData = csCommon.ConvertDataTableToDynamic(ds.Tables[0]);

                        var tableName = (tableNames.Count > 0 && !string.IsNullOrEmpty(tableNames[0]))
                                        ? tableNames[0]
                                        : "Data";

                        data.Add(tableName, tableData);
                    }

                    //-------------------------------------------------
                    // STEP 5: TotalRecords
                    //-------------------------------------------------
                    if (data.ContainsKey("TotalCount"))
                    {
                        var totalTable = data["TotalCount"];

                        if (totalTable?.Count > 0 &&
                            totalTable[0].ContainsKey("TotalRecords"))
                        {
                            response.TotalRecords = Convert.ToInt32(totalTable[0]["TotalRecords"] ?? 0);
                        }
                    }
                    else
                    {
                        response.TotalRecords = 0;
                    }

                    //-------------------------------------------------
                    // STEP 6: Default Response Check
                    //-------------------------------------------------
                    var firstTableName = tableNames.FirstOrDefault();

                    if (!string.IsNullOrEmpty(firstTableName) && data.ContainsKey(firstTableName))
                    {
                        var firstTable = data[firstTableName];

                        if (firstTable == null || firstTable.Count == 0)
                        {
                            response.Message = "Data Not Found";
                        }
                        else
                        {
                            response.Message = "Success";
                        }

                        response.Status = "Success";
                    }

                    //-------------------------------------------------
                    // FINAL RESPONSE
                    //-------------------------------------------------
                    response.Data = data;
                    response.PageNumber = Request.PageNo;
                    response.PageSize = Request.Limit;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                    response.TotalRecords = 0;
                }
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(
                    new ErrorLogEntity()
                    {
                        PageName = "ServicesController",
                        FunctionName = "public APIResponceWithTotalRec GetSocietyAmendment1958_MoreSummary_UserRoleWise(SocietyDashboard_UserRoleWiseRequestData Request) ; Error : " + ex.Message.ToString()
                    }, ex);

                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;
                response.TotalRecords = 0;
            }

            return response;
        }

        [HttpPost]
        public APIResponceWithTotalRec GetSocietyAmendment1958_OtherDetails_UserRoleWise(SocietyDashboard_UserRoleWiseRequestData Request)
        {
            APIResponceWithTotalRec response = new APIResponceWithTotalRec();
            MobileDashboardModel objModel = new MobileDashboardModel();

            int SocietyId = 0;
            string SocietyIdEnc = "";
            int AmendmentId = 0;
            string OtherDetailsType = "";

            if (Request != null)
            {
                SocietyId = Request.SocietyId;
                SocietyIdEnc = Request.SocietyIdEnc ?? "";
                AmendmentId = Request.AmendmentId;
                OtherDetailsType = Request.OtherDetailsType ?? "";
            }

            try
            {
                var ds = objModel.Get_SocietyAmendment1958_OtherDetails_UserRoleWise(
                            SocietyId,
                            SocietyIdEnc,
                            AmendmentId,
                            OtherDetailsType,
                            Request.PageNo,
                            Request.Limit);

                if (ds != null && ds.Tables.Count > 0)
                {
                    //-------------------------------------------------
                    // STEP 1: Get Table Names (Reusable Method)
                    //-------------------------------------------------
                    var tableNames = csCommon.GetTableNamesFromDataSet(ds);

                    //-------------------------------------------------
                    // STEP 2: Remove Last Table (Metadata)
                    //-------------------------------------------------
                    ds.Tables.RemoveAt(ds.Tables.Count - 1);

                    //-------------------------------------------------
                    // STEP 3: SAFETY CHECK (Important)
                    //-------------------------------------------------
                    if (tableNames.Count != ds.Tables.Count)
                    {
                        tableNames = ds.Tables
                                       .Cast<DataTable>()
                                       .Select((t, i) => "Table" + i)
                                       .ToList();
                    }

                    //-------------------------------------------------
                    // STEP 4: Convert DataSet → Dynamic
                    //-------------------------------------------------
                    var data = new Dictionary<string, List<Dictionary<string, object>>>();

                    if (ds.Tables.Count > 1)
                    {
                        data = csCommon.ConvertDataSetToDynamic(ds, tableNames);
                    }
                    else if (ds.Tables.Count == 1)
                    {
                        var tableData = csCommon.ConvertDataTableToDynamic(ds.Tables[0]);

                        var tableName = (tableNames.Count > 0 && !string.IsNullOrEmpty(tableNames[0]))
                                        ? tableNames[0]
                                        : "Data";

                        data.Add(tableName, tableData);
                    }

                    //-------------------------------------------------
                    // STEP 5: TotalRecords
                    //-------------------------------------------------
                    if (data.ContainsKey("TotalCount"))
                    {
                        var totalTable = data["TotalCount"];

                        if (totalTable?.Count > 0 &&
                            totalTable[0].ContainsKey("TotalRecords"))
                        {
                            response.TotalRecords = Convert.ToInt32(totalTable[0]["TotalRecords"] ?? 0);
                        }
                    }
                    else
                    {
                        response.TotalRecords = 0;
                    }

                    //-------------------------------------------------
                    // STEP 6: Default Response Check
                    //-------------------------------------------------
                    var firstTableName = tableNames.FirstOrDefault();

                    if (!string.IsNullOrEmpty(firstTableName) && data.ContainsKey(firstTableName))
                    {
                        var firstTable = data[firstTableName];

                        if (firstTable == null || firstTable.Count == 0)
                        {
                            response.Message = "Data Not Found";
                        }
                        else
                        {
                            response.Message = "Success";
                        }

                        response.Status = "Success";
                    }

                    //-------------------------------------------------
                    // FINAL RESPONSE
                    //-------------------------------------------------
                    response.Data = data;
                    response.PageNumber = Request.PageNo;
                    response.PageSize = Request.Limit;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                    response.TotalRecords = 0;
                }
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(
                    new ErrorLogEntity()
                    {
                        PageName = "ServicesController",
                        FunctionName = "public APIResponceWithTotalRec GetSocietyAmendment1958_MoreSummary_UserRoleWise(SocietyDashboard_UserRoleWiseRequestData Request) ; Error : " + ex.Message.ToString()
                    }, ex);

                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;
                response.TotalRecords = 0;
            }

            return response;
        }

        #endregion  SocietyAmendment1958_DashboardData_AsPer_LoginUser   --END----

        #region         MobileApp_ActionClick_WebPageURL  --START--

        [HttpPost]
        public APIResponce Get_ActionClick_WebPageURL(SocietyDashboard_UserRoleWiseRequestData Request)
        {
            MobileDashboardModel objSocietyModel = new MobileDashboardModel();
            APIResponce response = new APIResponce();
            string TabName = "", ActionName = "";

            try
            {
                TabName = Request?.TabName ?? "";// "Society_UnderProcess";
                ActionName = Request?.ActionName ?? "";//"Action";

                var result = objSocietyModel.Get_ActionClick_WebPageURL(TabName, ActionName);

                if (result != null && result.Rows.Count > 0)
                {
                    var data = csCommon.ConvertDataTableToDynamic(result);
                    response.Status = "Success";
                    response.Message = "Success";
                    response.Data = data;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Internal Server Error.";
                response.Data = null;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponce Get_Society_UnderProcess_ActionClick_WebPageURL() ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }

        [HttpPost]
        public APIResponce Get_ActionClick_WebPageURL_AsPerParams(WebPageURL_Request objWebPageURL_Request)
        {
            MobileDashboardModel objSocietyModel = new MobileDashboardModel();
            APIResponce response = new APIResponce();

            try
            {

                var result = objSocietyModel.Get_ActionClick_WebPageURL_AsPerParams(objWebPageURL_Request);

                if (result != null && result.Rows.Count > 0)
                {
                    var data = csCommon.ConvertDataTableToDynamic(result);
                    response.Status = "Success";
                    response.Message = "Success";
                    response.Data = data;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Internal Server Error.";
                response.Data = null;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponce Get_ActionClick_WebPageURL_AsPerParams(WebPageURL_Request objWebPageURL_Request) ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }

        #endregion      MobileApp_ActionClick_WebPageURL  ----END--

        #region     Society_UnderProcess_Pending_To_Assign  ---START--

        [HttpPost]
        public APIResponce GetApplicationFlowSociety(WebPageURL_Request Request)
        {
            MobileDashboardModel objSocietyModel = new MobileDashboardModel();
            APIResponce response = new APIResponce();
            string SocietyIDEnc = "";

            //return COMMON_FUNCTION.DataTableToJSONWithJSONNet(objSocietyModel.GetApplicationFlowSociety(SocietyIDEnc));
            try
            {
                SocietyIDEnc = Request?.SocietyIDEnc ?? "";//"EDIT-Action";
                var result = objSocietyModel.GetApplicationFlowSociety(SocietyIDEnc);

                if (result != null && result.Rows.Count > 0)
                {
                    var data = csCommon.ConvertDataTableToDynamic(result);
                    response.Status = "Success";
                    response.Message = "Success";
                    response.Data = data;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Internal Server Error.";
                response.Data = null;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponce GetApplicationFlowSociety(WebPageURL_Request Request) ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }

        [HttpPost]
        public APIResponce GetUserForSocietyAssign(WebPageURL_Request Request)
        {
            MobileDashboardModel objSocietyModel = new MobileDashboardModel();
            APIResponce response = new APIResponce();
            string SocietyID = "";

            //DataTable DT = objSocietyModel.GetUserForSocietyAssign(SocietyID);
            //return Json(new { DataList = COMMON_FUNCTION.DataTableToJSONWithJSONNet(DT) }, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            try
            {
                SocietyID = Request?.SocietyID ?? "";//"EDIT-Action";
                var result = objSocietyModel.GetUserForSocietyAssign(SocietyID);

                if (result != null && result.Rows.Count > 0)
                {
                    var data = csCommon.ConvertDataTableToDynamic(result);
                    response.Status = "Success";
                    response.Message = "Success";
                    response.Data = data;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Internal Server Error.";
                response.Data = null;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponce GetUserForSocietyAssign(WebPageURL_Request Request) ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }

        #endregion  Society_UnderProcess_Pending_To_Assign  _____END__

        #endregion Ritesh_Mobile_App_ControllerMethods



        #region Society Pending List API On 22-05-2026
        [HttpPost]
        public APIResponseViewModel GetUnderProcessSocietyDetailsByActUserWise(SocietyDashboard_UserRoleWiseRequestData Request)
        {
            APIResponseViewModel response = new APIResponseViewModel();
            MobileDashboardModel objModel = new MobileDashboardModel();

            try
            {
                var ds = objModel.GetUnderProcessSocietyDetailsByActUserWiseSearchByAct(Request.UserRoleMappingId, Request.ActId, Request.IsOnline, Request.Status, Request.SocietyTypeId = 0, Request.DistrictId, Request.DivisionId, Request.BlockId, Request.GPId, Request.VillageId, Request.CityId, Request.WardId, Request.IsRuralNotUrban, Request.PageNo, Request.Limit);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = csCommon.ConvertDataTableToDynamic(ds.Tables[0]);
                    response.Status = "Success";
                    response.Message = "Success";
                    response.Data = data;

                    // TOTAL RECORDS TABLE
                    //if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                    //{
                    //    response.TotalRecords = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalRecords"]);
                    //}

                    //response.PageNumber = Request.PageNo;
                    //response.PageSize = Request.Limit;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                    response.TotalRecords = 0;
                }
            }
            catch (Exception ex)
            {
                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;
                response.TotalRecords = 0;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponceWithTotalRec GetUnderProcessSocietyDetailsByActUserWise(SocietyDashboard_UserRoleWiseRequestData Request) ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }


        [HttpPost]
        public APIResponseViewModel GetSocietyProceesStatusList(SocietyStatusListModel requestModel)
        {
            APIResponseViewModel response = new APIResponseViewModel();
            MobileDashboardModel objModel = new MobileDashboardModel();
            try
            {
                var ds = objModel.GetUnderProcessSocietyProceesStatusList(requestModel);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var result = csCommon.ConvertDataTableToDynamic(ds.Tables[0]);
                    response.Status = "Success";
                    response.Message = "Success";
                    response.Data = result;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponceWithTotalRec GetUnderProcessSocietyDetailsByActUserWise(SocietyStatusListModel requestModel) ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;

        }

        [HttpPost]
        public APIResponseViewModel GetRoleListForApplicationStatus(SocietyStatusListModel requestModel)
        {
            APIResponseViewModel response = new APIResponseViewModel();
            MobileDashboardModel objModel = new MobileDashboardModel();

            try
            {
                var ds = objModel.GetRoleListForApplicationStatus(requestModel);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = csCommon.ConvertDataTableToDynamic(ds.Tables[0]);

                    response.Status = "Success";
                    response.Message = "Success";
                    response.Data = data;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponceWithTotalRec GetRoleListForApplicationStatus(SocietyStatusListModel requestModel) ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }


        [HttpPost]
        public APIResponseViewModel GetOfficerListBySocietyId(SocietyStatusListModel requestModel)
        {
            APIResponseViewModel response = new APIResponseViewModel();
            MobileDashboardModel objModel = new MobileDashboardModel();
            try
            {
                var ds = objModel.GetOfficerDetailBySocietyID(requestModel);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var result = csCommon.ConvertDataTableToDynamic(ds.Tables[0]);
                    response.Status = "Success";
                    response.Message = "Success";
                    response.Data = result;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponseViewModel GetOfficerListBySocietyId(SocietyStatusListModel requestModel) ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }




        [HttpPost]
        public APIResponseViewModel GetUnderProcessSocietyDetailsByActUserWiseNew(SocietyDashboard_UserRoleWiseRequestData Request)
        {
            APIResponseViewModel response = new APIResponseViewModel();
            MobileDashboardModel objModel = new MobileDashboardModel();

            try
            {
                var ds = objModel.GetUnderProcessSocietyDetailsByActUserWiseSearchByActNew(Request.UserRoleMappingId, Request.ActId, Request.IsOnline, Request.Status, Request.SocietyTypeId = 0, Request.DistrictId, Request.DivisionId, Request.BlockId, Request.GPId, Request.VillageId, Request.CityId, Request.WardId, Request.IsRuralNotUrban, Request.PageNo, Request.Limit);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var data = csCommon.ConvertDataTableToDynamic(ds.Tables[0]);

                    response.Status = "Success";
                    response.Message = "Success";
                    response.Data = data;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "Data Not Found";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;
                response.TotalRecords = 0;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponceWithTotalRec GetUnderProcessSocietyDetailsByActUserWiseSearchByActNew(SocietyDashboard_UserRoleWiseRequestData Request) ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }



        [HttpPost]
        public APIResponseViewModel SocietyApplicationFlowStatusRequest(SaveSocietyObjectionRequestModel requestModel)
        {
            APIResponseViewModel response = new APIResponseViewModel();
            try
            {
                var service = new MobileDashboardModel();
                if (requestModel.StatusId != 0 || !string.IsNullOrWhiteSpace(requestModel.Remark))
                {
                    if (requestModel.StatusId != 17 && requestModel.StatusId != 14)
                    {
                        if (requestModel.StatusId == 5 || requestModel.StatusId == 10 || requestModel.StatusId == 20)
                        {
                            if (requestModel.ObjectionFlagBasic == false && requestModel.ObjectionFlagAdmin == false && requestModel.ObjectionFlagOffice == false)
                            {
                                response.Status = "Fail";
                                response.Message = "Please Select Objection Belong To.";
                                response.Data = null;
                                return response;
                            }
                            else
                            {
                                response = SaveSocietyObjectionDetail(requestModel);
                                if (response.Status == "Success")
                                {
                                    response = SaveSocietyApplicationFlowStatus(requestModel);
                                    return response;
                                }
                            }
                        }
                        else
                        {
                            response = SaveSocietyApplicationFlowStatus(requestModel);
                            return response;
                        }
                    }
                    else
                    {
                        response = SaveSocietyApplicationFlowStatus(requestModel);
                        return response;
                    }
                }
                else
                {
                    response.Status = "Fail";
                    response.Message = "Please select Status and enter Remark";
                    response.Data = null;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;
                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponseViewModel SaveSocietyApplicationFlowStatus(SaveSocietyObjectionRequestModel RequestModel) ; Error : " + ex.Message.ToString()
                }, ex);
            }
            return response;
        }

        public APIResponseViewModel SaveSocietyObjectionDetail(SaveSocietyObjectionRequestModel RequestModel)
        {
            APIResponseViewModel response = new APIResponseViewModel();
            MobileDashboardModel objModel = new MobileDashboardModel();

            try
            {
                int result = objModel.SaveSocietyObjectionDetails(RequestModel);
                if (result > 0 && result != -1)
                {
                    response.Status = "Success";
                    response.Message = "Saved Successfully";
                    response.Data = result;
                }
                else if (result == -1)
                {
                    response.Status = "Fail";
                    response.Message = "Data Not Saved";
                    response.Data = result;
                }
                else
                {
                    response.Status = "Fail";
                    response.Message = "Data Not Saved";
                    response.Data = 0;
                }
            }
            catch (Exception ex)
            {
                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;

                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponseViewModel SaveSocietyObjectionDetail(string SocietyIDEnc, bool ObjectionFlagBasic, bool ObjectionFlagAdmin, bool ObjectionFlagOffice) ; Error : " + ex.Message.ToString()
                }, ex);
            }

            return response;
        }
        public APIResponseViewModel SaveSocietyApplicationFlowStatus(SaveSocietyObjectionRequestModel RequestModel)
        {
            int result = 0;
            APIResponseViewModel response = new APIResponseViewModel();
            MobileDashboardModel _model = new MobileDashboardModel();
            try
            {
                if (RequestModel.StatusId == 3 || RequestModel.StatusId == 9 || RequestModel.StatusId == 12 || RequestModel.StatusId == 19 || RequestModel.StatusId == 23 || RequestModel.StatusId == 35 || RequestModel.StatusId == 36 || RequestModel.StatusId == 37 || RequestModel.StatusId == 38 || RequestModel.StatusId == 39 || RequestModel.StatusId == 40 || RequestModel.StatusId == 41 || RequestModel.StatusId == 1058 || RequestModel.StatusId == 17 || RequestModel.StatusId == 43)
                {
                    if (RequestModel.OfficeId == 0)
                    {
                        response.Status = "Fail";
                        response.Message = "Please Select Officer.";
                        response.Data = null;
                        return response;
                    } //else { RequestModel.UserId = RequestModel.OfficeId; }

                }

                //if (RequestModel.StatusId == 13 || RequestModel.StatusId == 21 || RequestModel.StatusId == 6)
                //{
                result = _model.SocietyApplicationFlow(RequestModel.SocietyIDEnc, RequestModel.StatusId, RequestModel.Remark, RequestModel.UserId, RequestModel.LoggedUserId, RequestModel.SSOId, RequestModel.IsAmendment, RequestModel.AmendmendAmount, RequestModel.ReceiptNumber);
                if (result > 0)
                {
                    response.Status = "Success";
                    response.Message = "Saved Successfully";
                    response.Data = result;

                }
                else if (result == -1)
                {
                    response.Status = "Fail";
                    response.Message = "Data Not Saved";
                    response.Data = result;
                }
                else
                {
                    response.Status = "Fail";
                    response.Message = "Data Not Saved";
                    response.Data = result;
                }
                //}

            }
            catch (Exception ex)
            {
                response.Status = "Error";
                response.Message = "Internal Server Error";
                response.Data = null;
                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Controllers.ServicesController",
                    FunctionName = "public APIResponseViewModel SaveSocietyApplicationFlowStatus(SaveSocietyObjectionRequestModel RequestModel) ; Error : " + ex.Message.ToString()
                }, ex);

            }

            return response;

        }

        #endregion



        public class RajfedData
        {
            public string DISTRICT_CODE { get; set; }
            public string DISTRICT_ENG { get; set; }
            public string DISTRICT_MANGAL { get; set; }
            public string PurchaseCenterId { get; set; }
            public string Rajfed_PCID { get; set; }
            public string PurchaseCenterName_En { get; set; }
            public string PurchaseCenterName_Hi { get; set; }
            public string Commodity { get; set; }
            public string CommodityId { get; set; }
            public string KVSS { get; set; }
            public string TotalWR { get; set; }
            public string Quantity { get; set; }
            public string Unit { get; set; }
            public string crop_year { get; set; }
            public string crop_type { get; set; }
        }

        public class APIRequest
        {
            public string ApplicationNo { get; set; }
            public string Message { get; set; }
            public object Data { get; set; }
        }



        public class SocietyAmendment
        {
            public int SocietyID { get; set; }
            public int RegiYearId { get; set; }
            public string SocietyIDEnc { get; set; }
            public string SocietyDirectory { get; set; }
            public string ResolutionNo { get; set; }
            public string ByeLawsFile { get; set; }
            public string AGM1File { get; set; }
            public string AGM2File { get; set; }
            public string AGM3File { get; set; }
            public string Name { get; set; }
            public string MemberListExcelFile { get; set; }
            public string Remark { get; set; }
            public DateTime PaasedAGMDate { get; set; }
            public List<SocietyWorkScopeEntity> lstSocietyWorkScope { get; set; }
            public string HouseBuildingNo { get; set; }
            public string LaneStreetName { get; set; }
            public string Locality { get; set; }
            public bool IsRural { get; set; }
            public int STATE_ID { get; set; }
            public int DIVISION_ID { get; set; }
            public int DISTRICT_ID { get; set; }
            public int TEHSIL_ID { get; set; }
            public int CityTypes { get; set; }
            public int CITY_ID { get; set; }
            public int BLOCK_ID { get; set; }
            public int WARD_ID { get; set; }
            public int GP_ID { get; set; }
            public int VILLAGE_ID { get; set; }
            public int RoleId { get; set; }
            public int PinCode { get; set; }
            public int ClassA { get; set; }
            public int ClassB { get; set; }
            public int ClassC { get; set; }
            public string BoardType { get; set; }
            public List<AdministrativeData> AdministrativeDataList { get; set; }
        }


        //public class APIResponce
        //{
        //    public string Status { get; set; }
        //    public string Message { get; set; }
        //    public object Data { get; set; }
        //}        

        public class PLDBGeoData
        {
            public string LoanId { get; set; }
            public string PropartyId { get; set; }
            public string PropartyType { get; set; }
            public string SubmitType { get; set; }
            public string Latitude { get; set; }
            public string Longitude { get; set; }
            public string Photo { get; set; }
            public string SSOID { get; set; }
        }

        public class PLDBRequestData
        {
            public string SSOID { get; set; }
            public string Role { get; set; }
            public string LoanId { get; set; }
            public DataPagingModel Details { get; set; }
            public string Type { get; set; }
            public List<PLDBGeoData> Data { get; set; }
        }

        public class LoginUserDetails
        {
            public string RoleId { get; set; }
            public string UserId { get; set; }
            public string RoleName { get; set; }
            public string UserName { get; set; }
            public string OfficeName { get; set; }
            public string District { get; set; }
            public string Division { get; set; }
            public bool IsAllowToUpdateGodown { get; set; }
        }

        public class AmendmendRequest
        {
            public SocietyAmendmentEntity SocietyAmendment { get; set; }
            public SocietyAdministrativeEntity SocietyRegistrationData { get; set; }
            public List<AmendmentDocuments> AmendmentDocuments { get; set; }
            public List<AdministrativeData> MemberData { get; set; }
        }


        public class OTSDocumentsModel
        {
            public Int32 id { get; set; }
            public int OTSId { get; set; }
            public string OtsIdEnc { get; set; }
            public int DocId { get; set; }
            public string DocumentURL { get; set; }
            public string Remark { get; set; }
            public string OtherDoc { get; set; }
            public string TaamilDoc { get; set; }

        }

        public class OTSDocumentsNewModel
        {
            public Int32 id { get; set; }
            public int OTSId { get; set; }
            public string OtsIdEnc { get; set; }
            public int DocId { get; set; }
            public string DocumentURL { get; set; }
            public string RemarkNew { get; set; }
            public string OtherDoc_New { get; set; }
            public string TaamilDoc_New { get; set; }

        }

        public class OTSAadharJanaadharModel
        {
            public Int32 id { get; set; }
            public int PLDB_OTS_SchemeID { get; set; }
            public string OtsIdEnc { get; set; }

            public string Aadhar_Janaadhar_no { get; set; }
            public string Aadhar_Janaadhar_FileName { get; set; }

        }

        public class SocietyDashboard_UserRoleWiseRequestData
        {
            public string SSOID { get; set; }
            public int UserId { get; set; }
            public int RoleId { get; set; }
            public int OfficeId { get; set; }

            public int SocietyTypeId { get; set; }

            public int StateId { get; set; }
            public int DivisionId { get; set; } = 0;
            public int DistrictId { get; set; }
            public int BlockId { get; set; }
            public int GPId { get; set; }
            public int VillageId { get; set; }

            public int CityId { get; set; }
            public int CityType { get; set; }
            public int WardId { get; set; }

            public string ActId { get; set; }
            public string Status { get; set; }

            public int UserRoleMappingId { get; set; } = 0;
            public int IsOnline { get; set; }
            public int PageNo { get; set; }
            public int Limit { get; set; }
            public string SocietyName_RegistrationNo { get; set; }

            public int SocietyId { get; set; }
            public string SocietySummaryType { get; set; }
            public int IsRuralNotUrban { get; set; } = 1; // Default value set when object or variable create. By RK 2026-04-30 //

            public int SocietyTypeActId { get; set; }
            public int AmendmentId { get; set; }

            public string SocietyIdEnc { get; set; }
            public string OtherDetailsType { get; set; }
            public bool IsSocietyAmendment { get; set; } = false;

            public string TabName { get; set; }
            public string ActionName { get; set; }

        }

        public class SocietyDashboard_UserCitizenSSOIDWiseRequestData
        {
            public string SSOID { get; set; }
            public int UserId { get; set; }
            public int UserRoleMappingId { get; set; }
            public int RoleId { get; set; }
            public int OfficeId { get; set; }

            public string ActId { get; set; }
            public string Status { get; set; }

            public int SocietyId { get; set; }
            public int SocietyTypeId { get; set; } = 0;
            public string SocietyName_RegistrationNo { get; set; } = "";
            public string SocietySummaryType { get; set; }

            public int IsRuralNotUrban { get; set; } = 0; // Default value set when object or variable create. By RK 2026-04-30 //
            public int IsOnline { get; set; }

            public int StateId { get; set; } = 0;

            public int DivisionId { get; set; } = 0;
            public int DistrictId { get; set; } = 0;

            public int BlockId { get; set; } = 0;
            public int GPId { get; set; } = 0;
            public int VillageId { get; set; } = 0;

            public int CityId { get; set; } = 0;
            public int CityType { get; set; } = 0;
            public int WardId { get; set; } = 0;

            public int PageNo { get; set; } = 1;
            public int Limit { get; set; } = 10;
        }

        /// <summary>
        /// Use for Citizen Corner APIs Request model (Input params)
        /// </summary>
        public class CitizenCornerRequestData
        {
            // Basic Filter Keys
            public int SearchType { get; set; }
            public string SearchValue { get; set; }
            public string Status { get; set; } = "ALL";

            // Other Filters
            public int SocietyTypeId { get; set; } = 0; // Audit Report Filters
            public int ActId { get; set; } = 0;
            public string FinancialYear { get; set; } = "";
            public int FinancialYearId { get; set; } = 0; // Audit Report Filters

            // Location Filters (Audit Report Filters)
            public int IsRuralNotUrban { get; set; }  // 1 = Urban, 0 = Rural
            public int DistrictId { get; set; } = 0;
            public int CityId { get; set; } = 0;
            public int BlockId { get; set; } = 0;
            public int GPId { get; set; } = 0;

            // Pagination Filter
            public int PageNo { get; set; }
            public int Limit { get; set; }
        }

        public class WebPageURL_Request
        {
            public string TabName { get; set; }
            public string ActionName { get; set; }
            public string KeyName { get; set; }
            public string KeyValue { get; set; }

            public string SocietyIDEnc { get; set; }
            public string SocietyID { get; set; }
            public string RegistrationNo { get; set; }
            public string SocietyName { get; set; }

            public string StatusId { get; set; }
            public string ActId { get; set; }

            public string DistrictId { get; set; }
            public string UserRoleMappingId { get; set; }
            public string UserId { get; set; }
            public string IsAmendment { get; set; }

        }

    }
}
