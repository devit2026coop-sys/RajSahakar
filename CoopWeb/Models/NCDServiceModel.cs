using Cooperative.Controllers;
using Cooperative.DBConnector;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Cooperative.Models.MMPBYFarmerDetailsModel;
using static Cooperative.Models.NCDApiModel;

namespace Cooperative.Models
{
    public class NCDServiceModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;
        private string ncdApiBaseUrl = "https://api.cooperatives.gov.in/";
        private string RJkeyOld = "92b96b043c51494b309daf4de04766d07ed60934bb301b561808aef75c74a64b";
        private string RJkey = "jfh83423bsjahsad8888dsad$%";

        public NCDServiceModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public NCDServiceModel()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();

        }

        #region NCDStateAPI integration
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stateCode"></param>
        /// <returns></returns>
        public async Task<NCDStateApiResponse> GetStateDetails()
        {
            string url = $"{ncdApiBaseUrl}Api/findStateLgCode";
            NCDStateApiResponse ResponseData = new NCDStateApiResponse();
            try
            {
                List<NCDStateModel> lstState = new List<NCDStateModel>();
                using (HttpClient httpClient = new HttpClient())
                {
                    var payload = new
                    {
                        key = RJkey,
                        state_code = 8
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    var data = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        JObject result = JObject.Parse(data);
                        ResponseData.status = result["status"].ToString();
                        if (ResponseData.status == "Success")
                        {
                            var ncdStateData = result["message"].Value<JArray>();
                            lstState = ncdStateData.ToObject<List<NCDStateModel>>();
                            ResponseData.data = lstState;
                            return ResponseData;
                        }
                        else
                        {
                            ResponseData.message = result["message"].ToString();
                            return ResponseData;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDStateModel", FunctionName = "GetStateByCode()" }, ex);

            }
            return ResponseData;
        }
        public int SaveStateDetails(List<NCDStateModel> _stateModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdStates", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@Action", "InsertNCDState");
                cmd.Parameters.AddWithValue("@M_NcdState", BulkInsertToDataTable(_stateModel));
                result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDStateModel", FunctionName = "SaveStateData(List<NCDStateModel> _stateModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }
        #endregion

        public async Task<NCDDistrictApiResponse> GetDistrictDetails()
        {
            string url = $"{ncdApiBaseUrl}Api/districtdetailsbystate";
            NCDDistrictApiResponse ResponseData = new NCDDistrictApiResponse();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var payload = new
                    {
                        key = RJkey,
                        state_code = 8
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    string data = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        ResponseData = JsonConvert.DeserializeObject<NCDDistrictApiResponse>(data);
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDDistrictModel", FunctionName = "GetDistrictDetails()" }, ex);
            }
            return ResponseData;
        }

        public int SaveDistrictDetails(NCDDistrictApiResponse _districtModel)
        {
            int result = 0;
            UserEntity _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                foreach (NCDDistrictModel model in _districtModel.result)
                {
                    SqlCommand cmd = objDBAccess.CreateCommand("M_NcdDistricts", DBFactoryCommandType.PROCEDURE_COMMAND);

                    objDBAccess.AddParameterToCommand("@DistrictCode", DBFactoryParameterType.VARCHAR, model.district_code);
                    objDBAccess.AddParameterToCommand("@DistrictName", DBFactoryParameterType.VARCHAR, model.district_name);
                    DataTable dtResult = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dtResult);
                    result = dtResult.Rows.Count;
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDDistrictModel", FunctionName = "SaveDistrictData(List<NCDDistrictModel> _districtModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }


        public async Task<NCDBlocksApiResponse> GetNcdBlockDetails()
        {
            string url = $"{ncdApiBaseUrl}en/Api/apiforblocksdata";
            NCDBlocksApiResponse ResponseData = new NCDBlocksApiResponse();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var payload = new
                    {
                        key = RJkey,
                        state_code = 8
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    string data = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        ResponseData = JsonConvert.DeserializeObject<NCDBlocksApiResponse>(data);
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "GetNcdBlockDetails()" }, ex);
            }
            return ResponseData;
        }

        public int SaveNcdBlockDetails(List<NCDBlockViewModel> _blockModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstBlockData = _blockModel.Select(x => x.BlockDetails).ToList();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdBlock", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@M_NcdBlock", BulkInsertToDataTable(lstBlockData));
                result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDBlocksApiResponse", FunctionName = "SaveNcdBlockDetails(List<NCDBlockViewModel> _blockModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public async Task<NCDVillagesAPIResponse> GetNcdVillageDetails()
        {
            string url = $"{ncdApiBaseUrl}en/Api/apifordistrictsblocksgpvillagesdata";
            NCDVillagesAPIResponse ResponseData = new NCDVillagesAPIResponse();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var payload = new
                    {
                        key = RJkey,
                        state_code = 8
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    string data = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        ResponseData = JsonConvert.DeserializeObject<NCDVillagesAPIResponse>(data);
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "GetNcdVillageDetails()" }, ex);
            }
            return ResponseData;
        }

        public int SaveVillageDetails(List<NCDVillageViewModel> _VillageModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();

                var lstVillageData = _VillageModel.Select(x => x.VillageDetails).ToList();

                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdVillage", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@M_NcdVillage", BulkInsertToDataTable(lstVillageData));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDBlocksApiResponse", FunctionName = "SaveVillageDataByStatecode(NCDVillageViewModel _VillageModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public async Task<NCDCitiesAPIResponse> GetNcdCitiesDetails()
        {
            string url = $"{ncdApiBaseUrl}en/Api/apiforulbdata";
            NCDCitiesAPIResponse ResponseData = new NCDCitiesAPIResponse();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var payload = new
                    {
                        key = RJkey,
                        state_code = 8
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    string data = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        ResponseData = JsonConvert.DeserializeObject<NCDCitiesAPIResponse>(data);
                    }
                    return ResponseData;
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDCitiesAPIResponse", FunctionName = "GetNcdCitiesDetails()" }, ex);
            }
            return ResponseData;
        }


        public int SaveCitiesDetails(List<NCDCitiesViewModel> _CitiesModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();

                var lstCitiesData = _CitiesModel.Select(x => x.CitiesDetails).ToList();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdCity", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@M_NcdCities", BulkInsertToDataTable(lstCitiesData));
                result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDCitiesViewModel", FunctionName = "SaveCitiesDataByStatecode(NCDCitiesViewModel _CitiesModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public async Task<NCDWardApiResponse> GetNcdWardDetails()
        {
            string url = $"{ncdApiBaseUrl}en/Api/apiforulbwarddata";
            NCDWardApiResponse ResponseData = new NCDWardApiResponse();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var payload = new
                    {
                        key = RJkey,
                        state_code = 8
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    string data = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        ResponseData = JsonConvert.DeserializeObject<NCDWardApiResponse>(data);
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDWardApiResponse", FunctionName = "GetNcdWardDetails()" }, ex);
            }
            return ResponseData;
        }

        public int SaveNcdWardDetails(List<NCDWardViewModel> _wardModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstWardsData = _wardModel.Select(x => x.WardDetails).ToList();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdWard", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@M_NcdWard", BulkInsertToDataTable(lstWardsData));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDWardViewModel", FunctionName = "SaveNcdWardDetails(List<NCDWardViewModel> _wardModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public async Task<NCDSectorAPIResponse> GetNcdSectorDetail()
        {
            string url = $"{ncdApiBaseUrl}en/Api/findNcdSectorCode";
            NCDSectorAPIResponse ResponseData = new NCDSectorAPIResponse();
            List<NCDSectorModel> sectorLstData = new List<NCDSectorModel>();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var payload = new
                    {
                        key = RJkey,
                        state_code = 8
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    var data = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        JObject result = JObject.Parse(data);
                        ResponseData.status = result["status"].ToString();
                        if (ResponseData.status == "Success") { var ncdSectorData = result["message"].Value<JArray>(); sectorLstData = ncdSectorData.ToObject<List<NCDSectorModel>>(); ResponseData.data = sectorLstData; }
                        else { ResponseData.message = result["message"].ToString(); }
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDSectorAPIResponse", FunctionName = "GetNcdSectorDetail()" }, ex);
            }
            return ResponseData;
        }

        public int SaveSectorDetails(List<NCDSectorModel> _SectorModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdSectors", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "InsertNCDSector");
                cmd.Parameters.AddWithValue("@M_NcdSector", BulkInsertToDataTable(_SectorModel));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDBlocksApiResponse", FunctionName = "SaveSectorDataByStatecode(NCDSectorModel _SectorModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public async Task<NCDSubSectorAPIResponse> GetNcdSubSectorDetail(int statecode, int levelcode)
        {
            string url = $"{ncdApiBaseUrl}en/Api/apimiscellanousdata";
            NCDSubSectorAPIResponse ResponseData = new NCDSubSectorAPIResponse();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var payload = new
                    {
                        key = RJkey,
                        state_code = 8
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    string data = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        ResponseData = JsonConvert.DeserializeObject<NCDSubSectorAPIResponse>(data);
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDSubSectorAPIResponse", FunctionName = "GetNcdSubSectorDetail()" }, ex);
            }
            return ResponseData;
        }

        public int SaveNcdSubSectorDetails(List<NCDSubSectorModel> _SubsectorModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdSubSectors", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@M_NcdSubSector", BulkInsertToDataTable(_SubsectorModel));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDServiceModel", FunctionName = "SaveNcdSubSectorDetails(List<NCDSubSectorViewModel> _SubsectorModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }



        public async Task<NcdApiCommonResponse<MemberDetailsResultItem>> GetMemberDetailByStatecode(int page = 1, int limit = 100000)
        {
            NcdApiCommonResponse<MemberDetailsResultItem> ResponseData = new NcdApiCommonResponse<MemberDetailsResultItem>();
            string url = $"{ncdApiBaseUrl}en/Api/apimiscellanousdata";
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var payload = new
                    {
                        key = RJkey,
                        state_code = 8,
                        level_code = 6,
                        page = page,
                        limit = limit
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    string data = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        ResponseData = JsonConvert.DeserializeObject<NcdApiCommonResponse<MemberDetailsResultItem>>(data);
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDMiscellaneousAPIResponse", FunctionName = "GetMemberDetailByStatecode()" }, ex);
            }
            return ResponseData;
        }


        public async Task<NcdApiCommonResponse<AreaofOperationRuralResultItem>> GetAreaofOperationRuralDetail(int page = 1, int limit = 100000)
        {
            NcdApiCommonResponse<AreaofOperationRuralResultItem> ResponseData = new NcdApiCommonResponse<AreaofOperationRuralResultItem>();
            string url = $"{ncdApiBaseUrl}en/Api/apimiscellanousdata";
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var payload = new
                    {
                        key = RJkey,
                        state_code = 8,
                        level_code = 1,
                        page = page,
                        limit = limit
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    string data = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        ResponseData = JsonConvert.DeserializeObject<NcdApiCommonResponse<AreaofOperationRuralResultItem>>(data);
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDMiscellaneousAPIResponse", FunctionName = "GetAreaofOperationRuralDetail()" }, ex);
            }
            return ResponseData;
        }
        public async Task<NcdApiCommonResponse<AreaofOperationUrbanResultItem>> GetAreaofOperationUrbanDetail(int page = 1, int limit = 100000)
        {
            NcdApiCommonResponse<AreaofOperationUrbanResultItem> ResponseData = new NcdApiCommonResponse<AreaofOperationUrbanResultItem>();
            string url = $"{ncdApiBaseUrl}en/Api/apimiscellanousdata";
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var payload = new
                    {
                        key = RJkey,
                        state_code = 8,
                        level_code = 2,
                        page = page,
                        limit = limit
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);

                    string data = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        ResponseData = JsonConvert.DeserializeObject<NcdApiCommonResponse<AreaofOperationUrbanResultItem>>(data);
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDMiscellaneousAPIResponse", FunctionName = "GetAreaofOperationUrbanDetail()" }, ex);
            }
            return ResponseData;
        }
        public async Task<NcdApiCommonResponse<SocietyImplementingSchemesDetailsResultItem>> GetSocietyImplementingSchemesDetails(int page = 1, int limit = 100000)
        {
            NcdApiCommonResponse<SocietyImplementingSchemesDetailsResultItem> ResponseData = new NcdApiCommonResponse<SocietyImplementingSchemesDetailsResultItem>();
            string url = $"{ncdApiBaseUrl}en/Api/apimiscellanousdata";
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var payload = new
                    {
                        key = RJkey,
                        state_code = 8,
                        level_code = 3,
                        page = page,
                        limit = limit
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    string data = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        ResponseData = JsonConvert.DeserializeObject<NcdApiCommonResponse<SocietyImplementingSchemesDetailsResultItem>>(data);
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDMiscellaneousAPIResponse", FunctionName = "GetSocietyImplementingSchemesDetails()" }, ex);
            }
            return ResponseData;
        }
        public async Task<NcdApiCommonResponse<CooperativeRegistrationsLandsDetailsResultItem>> GetCooperativeRegistrationsLandsDetails(int page = 1, int limit = 100000)
        {
            NcdApiCommonResponse<CooperativeRegistrationsLandsDetailsResultItem> ResponseData = new NcdApiCommonResponse<CooperativeRegistrationsLandsDetailsResultItem>();
            string url = $"{ncdApiBaseUrl}en/Api/apimiscellanousdata";
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var payload = new
                    {
                        key = RJkey,
                        state_code = 8,
                        level_code = 4,
                        page = page,
                        limit = limit
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    string data = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        ResponseData = JsonConvert.DeserializeObject<NcdApiCommonResponse<CooperativeRegistrationsLandsDetailsResultItem>>(data);
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDMiscellaneousAPIResponse", FunctionName = "GetCooperativeRegistrationsLandsDetails()" }, ex);
            }
            return ResponseData;
        }

        public async Task<NcdApiCommonResponse<NCDSociatyAuditYearsResultItem>> GetSocietyAuditYears(int page = 1, int limit = 100000)
        {
            NcdApiCommonResponse<NCDSociatyAuditYearsResultItem> ResponseData = new NcdApiCommonResponse<NCDSociatyAuditYearsResultItem>();
            string url = $"{ncdApiBaseUrl}en/Api/apimiscellanousdata";
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var payload = new
                    {
                        key = RJkey,
                        state_code = 8,
                        level_code = 7,
                        page = page,
                        limit = limit
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    string data = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        ResponseData = JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDSociatyAuditYearsResultItem>>(data);
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDMiscellaneousAPIResponse", FunctionName = "GetSocietyAuditYears()" }, ex);
            }
            return ResponseData;
        }


        //Add & Update Cooperative Data


        public string GenerateNcdId(string sectorId)
        {
            string result = "";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_GenerateNcdId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@sectorId", DBFactoryParameterType.VARCHAR, sectorId);
                var datatable = objDBAccess.GetDataTable();
                if (datatable != null)
                {
                    result = datatable.Rows[0]["NCDID"].ToString();
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDServiceModel", FunctionName = "GenerateNcdId(string sectorId)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }
        public NCDCommonResponse NcdSaveCooperativeData(string cooperative_id)
        {

            NCDApiModel ncdApiModel = new NCDApiModel(new DBLogger());
            NCDApiRequestModel reqModel = new NCDApiRequestModel();
            reqModel.cooperative_id = cooperative_id;
            NCDRootResponse rootResponse = null;
            List<NCDCooperativeRegistrationResponseModel> lstCooperativeRegistration = new List<NCDCooperativeRegistrationResponseModel>();
            var ds = ncdApiModel.GetCooperativeRegistrationDetailsById(reqModel);
            if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {

                lstCooperativeRegistration = ncdApiModel.ConvertDTToList<NCDCooperativeRegistrationResponseModel>(ds.Tables[0]);

                if (ds.Tables.Count == 8)
                {
                    if (lstCooperativeRegistration[0].sector_of_operation == 1 || lstCooperativeRegistration[0].sector_of_operation == 20 || lstCooperativeRegistration[0].sector_of_operation == 22)
                    {
                        rootResponse = new NCDRootResponse<NCDPacsLampsFssResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDPacsLampsFssResponseModel>(ds.Tables[7])
                        };
                    }
                    else if (lstCooperativeRegistration[0].sector_of_operation == 7)
                    {
                        rootResponse = new NCDRootResponse<NCDUCBResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDUCBResponseModel>(ds.Tables[7])
                        };
                    }
                    else if (lstCooperativeRegistration[0].sector_of_operation == 9)
                    {
                        rootResponse = new NCDRootResponse<NCDDairyResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDDairyResponseModel>(ds.Tables[7])
                        };
                    }
                    else if (lstCooperativeRegistration[0].sector_of_operation == 10)
                    {
                        rootResponse = new NCDRootResponse<NCDFisheryResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDFisheryResponseModel>(ds.Tables[7])
                        };
                    }
                    else if (lstCooperativeRegistration[0].sector_of_operation == 11)
                    {
                        rootResponse = new NCDRootResponse<NCDSugarResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDSugarResponseModel>(ds.Tables[7])
                        };
                    }
                    else if (lstCooperativeRegistration[0].sector_of_operation == 13)
                    {
                        rootResponse = new NCDRootResponse<NCDHandloomResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDHandloomResponseModel>(ds.Tables[7])
                        };
                    }
                    else if (lstCooperativeRegistration[0].sector_of_operation == 14)
                    {
                        rootResponse = new NCDRootResponse<NCDHandicraftResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDHandicraftResponseModel>(ds.Tables[7])
                        };
                    }
                    else if (lstCooperativeRegistration[0].sector_of_operation == 15)
                    {
                        rootResponse = new NCDRootResponse<NCDWomenWelfareResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDWomenWelfareResponseModel>(ds.Tables[7])
                        };
                    }
                    else if (lstCooperativeRegistration[0].sector_of_operation == 16)
                    {
                        rootResponse = new NCDRootResponse<NCDMultipurposeResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDMultipurposeResponseModel>(ds.Tables[7])
                        };
                    }
                    else if (lstCooperativeRegistration[0].sector_of_operation == 18)
                    {
                        rootResponse = new NCDRootResponse<NCDCreditThriftResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDCreditThriftResponseModel>(ds.Tables[7])
                        };
                    }
                    else if (lstCooperativeRegistration[0].sector_of_operation == 29)
                    {
                        rootResponse = new NCDRootResponse<NCDMiscellaneousResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDMiscellaneousResponseModel>(ds.Tables[7])
                        };
                    }
                    else if (lstCooperativeRegistration[0].sector_of_operation == 31)
                    {
                        rootResponse = new NCDRootResponse<NCDAgroProcessingResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDAgroProcessingResponseModel>(ds.Tables[7])
                        };
                    }
                    else if (lstCooperativeRegistration[0].sector_of_operation == 35)
                    {
                        rootResponse = new NCDRootResponse<NCDCmiscellaneousResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDCmiscellaneousResponseModel>(ds.Tables[7])
                        };
                    }
                    else if (lstCooperativeRegistration[0].sector_of_operation == 47)
                    {
                        rootResponse = new NCDRootResponse<NCDHousingResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDHousingResponseModel>(ds.Tables[7])
                        };
                    }
                    else if (lstCooperativeRegistration[0].sector_of_operation == 51)
                    {
                        rootResponse = new NCDRootResponse<NCDLabourResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDLabourResponseModel>(ds.Tables[7])
                        };
                    }
                    else if (lstCooperativeRegistration[0].sector_of_operation == 54)
                    {
                        rootResponse = new NCDRootResponse<NCDLiveStockResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDLiveStockResponseModel>(ds.Tables[7])
                        };
                    }
                    else if (lstCooperativeRegistration[0].sector_of_operation == 68)
                    {
                        rootResponse = new NCDRootResponse<NCDTransportResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDTransportResponseModel>(ds.Tables[7])
                        };
                    }
                    else if (lstCooperativeRegistration[0].sector_of_operation == 77)
                    {
                        rootResponse = new NCDRootResponse<NCDAgricultureResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDAgricultureResponseModel>(ds.Tables[7])
                        };
                    }
                    else if (lstCooperativeRegistration[0].sector_of_operation == 79)
                    {
                        rootResponse = new NCDRootResponse<NCDBeeFarmingResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDBeeFarmingResponseModel>(ds.Tables[7])
                        };
                    }
                    else if (lstCooperativeRegistration[0].sector_of_operation == 80)
                    {
                        rootResponse = new NCDRootResponse<NCDConsumerResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDConsumerResponseModel>(ds.Tables[7])
                        };
                    }
                    else if (lstCooperativeRegistration[0].sector_of_operation == 82)
                    {
                        rootResponse = new NCDRootResponse<NCDMarketingResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDMarketingResponseModel>(ds.Tables[7])
                        };
                    }
                    else if (lstCooperativeRegistration[0].sector_of_operation == 84)
                    {
                        rootResponse = new NCDRootResponse<NCDEducationTraningResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDEducationTraningResponseModel>(ds.Tables[7])
                        };
                    }
                    else if (lstCooperativeRegistration[0].sector_of_operation == 90)
                    {
                        rootResponse = new NCDRootResponse<NCDJuteResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDJuteResponseModel>(ds.Tables[7])
                        };
                    }
                    else if (lstCooperativeRegistration[0].sector_of_operation == 96)
                    {
                        rootResponse = new NCDRootResponse<NCDSericultureResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDSericultureResponseModel>(ds.Tables[7])
                        };
                    }
                    else if (lstCooperativeRegistration[0].sector_of_operation == 98)
                    {
                        rootResponse = new NCDRootResponse<NCDSocialResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDSocialResponseModel>(ds.Tables[7])
                        };
                    }
                    else if (lstCooperativeRegistration[0].sector_of_operation == 99)
                    {
                        rootResponse = new NCDRootResponse<NCDTourismResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDTourismResponseModel>(ds.Tables[7])
                        };
                    }
                    else if (lstCooperativeRegistration[0].sector_of_operation == 102)
                    {
                        rootResponse = new NCDRootResponse<NCDTribalResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDTribalResponseModel>(ds.Tables[7])
                        };
                    }
                    else if (lstCooperativeRegistration[0].sector_of_operation == 117)
                    {
                        rootResponse = new NCDRootResponse<NCDKhadiGramResponseModel>
                        {
                            SectorData = ncdApiModel.ConvertDTToList<NCDKhadiGramResponseModel>(ds.Tables[7])
                        };
                    }
                }

                rootResponse.CooperativeRegistrations = lstCooperativeRegistration;
                rootResponse.status = "Success";
                rootResponse.state_code = 8;
                rootResponse.state_name = "RAJASTHAN";
                rootResponse.cooperative_id = rootResponse.CooperativeRegistrations[0].cooperative_id;
                rootResponse.secure_key = "jfh83423bsjahsad8888dsad$%";
                rootResponse.sector_of_operation = rootResponse.CooperativeRegistrations[0].sector_of_operation;
                rootResponse.MembersDetails = ncdApiModel.ConvertDTToList<NCDMemberDetailsModel>(ds.Tables[1]);
                rootResponse.AreaOfOperationLevel = ncdApiModel.ConvertDTToList<NCD_AreaofOperationRuralDetails>(ds.Tables[2]);
                rootResponse.AreaOfOperationLevelUrban = ncdApiModel.ConvertDTToList<NCD_AreaofOperationUrbanDetails>(ds.Tables[3]);
                rootResponse.SocietyImplementingSchemes = ncdApiModel.ConvertDTToList<NCD_SocietyImplementingSchemesDetails>(ds.Tables[4]);
                rootResponse.CooperativeRegistrationsLands = ncdApiModel.ConvertDTToList<NCD_CooperativeRegistrationsLandsDetails>(ds.Tables[5]);
                rootResponse.SocietyAuditYears = ncdApiModel.ConvertDTToList<NcdSocietyAuditYearsModel>(ds.Tables[6]);
                string url = $"{ncdApiBaseUrl}Rcs/saveCooperativeData?key={RJkey}&state=8";
                //string url = $"{ncdApiBaseUrl}securedncd/api/rcs/save?key={RJkey}&state=8";

                var responseData = new NCDCommonResponse();
                try
                {

                    var json = JsonConvert.SerializeObject(rootResponse, Formatting.Indented);
                    if (lstCooperativeRegistration[0].sector_of_operation == 1 || lstCooperativeRegistration[0].sector_of_operation == 20 || lstCooperativeRegistration[0].sector_of_operation == 22)
                        json = json.Replace("SectorData", "CooperativeRegistrationPacs");
                    else if (lstCooperativeRegistration[0].sector_of_operation == 7)
                        json = json.Replace("SectorData", "CooperativeRegistrationsUcb");
                    else if (lstCooperativeRegistration[0].sector_of_operation == 9)
                        json = json.Replace("SectorData", "CooperativeRegistrationDairy");
                    else if (lstCooperativeRegistration[0].sector_of_operation == 10)
                        json = json.Replace("SectorData", "CooperativeRegistrationFishery");
                    else if (lstCooperativeRegistration[0].sector_of_operation == 11)
                        json = json.Replace("SectorData", "CooperativeRegistrationsSugar");
                    else if (lstCooperativeRegistration[0].sector_of_operation == 13)
                        json = json.Replace("SectorData", "CooperativeRegistrationsHandloom");
                    else if (lstCooperativeRegistration[0].sector_of_operation == 14)
                        json = json.Replace("SectorData", "CooperativeRegistrationsHandicraft");
                    else if (lstCooperativeRegistration[0].sector_of_operation == 15)
                        json = json.Replace("SectorData", "CooperativeRegistrationsWocoop");
                    else if (lstCooperativeRegistration[0].sector_of_operation == 17)
                        json = json.Replace("SectorData", "CooperativeRegistrationsMulti");
                    else if (lstCooperativeRegistration[0].sector_of_operation == 18)
                        json = json.Replace("SectorData", "CooperativeRegistrationsCreditThrift");
                    else if (lstCooperativeRegistration[0].sector_of_operation == 29)
                        json = json.Replace("SectorData", "CooperativeRegistrationsMiscellaneous");
                    else if (lstCooperativeRegistration[0].sector_of_operation == 31)
                        json = json.Replace("SectorData", "CooperativeRegistrationsProcessing");
                    else if (lstCooperativeRegistration[0].sector_of_operation == 35)
                        json = json.Replace("SectorData", "CooperativeRegistrationsCmiscellaneous");
                    else if (lstCooperativeRegistration[0].sector_of_operation == 47)
                        json = json.Replace("SectorData", "CooperativeRegistrationsHousing");
                    else if (lstCooperativeRegistration[0].sector_of_operation == 51)
                        json = json.Replace("SectorData", "CooperativeRegistrationsLabour");
                    else if (lstCooperativeRegistration[0].sector_of_operation == 47)
                        json = json.Replace("SectorData", "CooperativeRegistrationsLivestock");
                    else if (lstCooperativeRegistration[0].sector_of_operation == 68)
                        json = json.Replace("SectorData", "CooperativeRegistrationsTransport");
                    else if (lstCooperativeRegistration[0].sector_of_operation == 77)
                        json = json.Replace("SectorData", "CooperativeRegistrationsAgriculture");
                    else if (lstCooperativeRegistration[0].sector_of_operation == 79)
                        json = json.Replace("SectorData", "CooperativeRegistrationsBee");
                    else if (lstCooperativeRegistration[0].sector_of_operation == 80)
                        json = json.Replace("SectorData", "CooperativeRegistrationsConsumer");
                    else if (lstCooperativeRegistration[0].sector_of_operation == 82)
                        json = json.Replace("SectorData", "CooperativeRegistrationsMarketing");
                    else if (lstCooperativeRegistration[0].sector_of_operation == 84)
                        json = json.Replace("SectorData", "CooperativeRegistrationsEducation");
                    else if (lstCooperativeRegistration[0].sector_of_operation == 90)
                        json = json.Replace("SectorData", "CooperativeRegistrationsJute");
                    else if (lstCooperativeRegistration[0].sector_of_operation == 96)
                        json = json.Replace("SectorData", "CooperativeRegistrationsSericulture");
                    else if (lstCooperativeRegistration[0].sector_of_operation == 98)
                        json = json.Replace("SectorData", "CooperativeRegistrationsSocial");
                    else if (lstCooperativeRegistration[0].sector_of_operation == 99)
                        json = json.Replace("SectorData", "CooperativeRegistrationsTourism");
                    else if (lstCooperativeRegistration[0].sector_of_operation == 102)
                        json = json.Replace("SectorData", "CooperativeRegistrationsTribal");
                    else if (lstCooperativeRegistration[0].sector_of_operation == 117)
                        json = json.Replace("SectorData", "CooperativeRegistrationsKhadiGram");
                    json = json.Replace("[]", "[{}]");
                    COMMON_FUNCTION.Sp_callTrackingInsert("NCDCooperativeData", json);

                    var responseString = Task.Run(() =>
                    {
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        using (HttpClient client = new HttpClient())
                        {
                            try
                            {
                                var content = new StringContent(json, Encoding.UTF8, "application/json");
                                HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();
                                response.EnsureSuccessStatusCode();
                                return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                            }
                            catch (Exception iex)
                            {
                                ILogger objILogger = new DBLogger();
                                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.NCDServiceModel", FunctionName = $"NcdSaveCooperativeData-Inner(string cooperative_id)" }, iex);
                                return iex.Message;
                            }
                        }
                    }).Result;
                    responseData = JsonConvert.DeserializeObject<NCDCommonResponse>(responseString);
                    COMMON_FUNCTION.Sp_callTrackingInsert("NCDCooperativeData-Response", responseString);

                }
                catch (Exception ex)
                {
                    DBLogger objDBLogger = new DBLogger();
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDServiceModel", FunctionName = "NcdSaveCooperativeData(string cooperative_id)" }, ex);

                }
                return responseData;
            }
            return null;
        }

        /// <summary>
        /// Common service for update and sync cooperative child data.
        /// </summary>
        /// <param name="sectorcode"></param>
        /// <returns></returns>
        public async Task<NCDCommonResponse> SyncCooperativeChildTableDataBySector(int sectorcode, int page = 1, int limit = 100000)
        {
            string url = $"{ncdApiBaseUrl}en/Api/sectorchildtablerawdatastatewise";
            var commonResponse = new NCDCommonResponse();
            try
            {
                int result = 0;
                using (HttpClient httpClient = new HttpClient())
                {
                    var payload = new
                    {
                        key = RJkey,
                        state_code = 8,
                        sector_code = sectorcode,
                        page = page,
                        limit = limit
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    string data = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        switch (sectorcode)
                        {
                            case 1:
                            case 20:
                            case 22:
                                result = SavePacsLampsFssDetails(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDPacsLampsFssViewModel>>(data).result);
                                break;
                            case 7:
                                result = SaveUCBDetail(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDUCBViewModel>>(data).result);
                                break;
                            case 9:
                                result = SaveDairyDetail(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDDairyViewModel>>(data).result);
                                break;
                            case 10:
                                result = SaveFisheryDetail(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDFisheryViewModel>>(data).result);
                                break;
                            case 11:
                                result = SaveSugarDetail(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDSugarViewModel>>(data).result);
                                break;
                            case 13:
                                result = SaveHadloomDetail(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDHandloomViewModel>>(data).result);
                                break;
                            case 14:
                                result = SaveHandicraftDetail(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDCooperativeRegistrationHandicraftViewModel>>(data).result);
                                break;
                            case 15:
                                result = SaveWomenWelfareDetail(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDWomenWelfareViewModel>>(data).result);
                                break;
                            case 16:
                                result = SaveMultipurposeDetail(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDMultipurposeViewModel>>(data).result);
                                break;
                            case 31:
                                result = SaveAgroProcessingDetail(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDAgroProcessingViewModel>>(data).result);
                                break;
                            case 79:
                                result = SaveBeeFarmingDetail(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDBeeFarmingViewModel>>(data).result);
                                break;
                            case 18:
                                result = SaveCreditThriftDetail(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDCreditThriftViewModel>>(data).result);
                                break;
                            case 29:
                                result = SaveMiscellaneousDetail(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDMiscellaneousViewModel>>(data).result);
                                break;
                            case 35:
                                result = SaveCmiscellaneousDetail(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDCmiscellaneousViewModel>>(data).result);
                                break;
                            case 47:
                                result = SaveHousingDetail(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDHousingViewModel>>(data).result);
                                break;
                            case 51:
                                result = SaveLabourDetail(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDLabourViewModel>>(data).result);
                                break;
                            case 54:
                                result = SaveLiveStockDetail(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDLiveStockViewModel>>(data).result);
                                break;
                            case 68:
                                result = SaveTransportDetail(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDTransportViewModel>>(data).result);
                                break;
                            case 77:
                                result = SaveAgricultureDetail(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDAgricultureViewModel>>(data).result);
                                break;
                            case 80:
                                result = SaveConsumerDetail(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDConsumerViewModel>>(data).result);
                                break;
                            case 82:
                                result = SaveMarketingDetail(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDMarketingViewModel>>(data).result);
                                break;
                            case 84:
                                result = SaveEducationTrainingDetail(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDEducationTraningViewModel>>(data).result);
                                break;
                            case 90:
                                result = SaveJuteDetail(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDJuteViewModel>>(data).result);
                                break;
                            case 96:
                                result = SaveSericultureDetail(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDSericultureViewModel>>(data).result);
                                break;
                            case 98:
                                result = SaveSocialDetail(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDSocialViewModel>>(data).result);
                                break;
                            case 99:
                                result = SaveTourismDetail(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDTourismViewModel>>(data).result);
                                break;
                            case 102:
                                result = SaveTribalDetail(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDTribalViewModel>>(data).result);
                                break;
                            case 117:
                                result = SaveKhadigramDetail(JsonConvert.DeserializeObject<NcdApiCommonResponse<NCDKhadiGramViewModel>>(data).result);
                                break;
                            default:
                                break;
                        }
                        if (result > 0)
                            commonResponse.status = "Success";
                        else
                            commonResponse.status = "Error";

                    }
                }
            }
            catch (Exception ex)
            {
                commonResponse.status = "Error";
                commonResponse.message = ex.Message;
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDServiceModel", FunctionName = "SyncCooperativeChildTableDataBySector(sectorcode = '" + sectorcode + "')" }, ex);

            }
            return commonResponse;
        }

        public async Task<NCDLevelCodeCommonResponse> GetLevelCodeMasterData(int levelcode, int page = 1, int limit = 100000)
        {
            NCDLevelCodeCommonResponse responseData = null;
            NCDApiModel ncdApiModel = new NCDApiModel(new DBLogger());
            string url = $"{ncdApiBaseUrl}en/Api/apimiscellanousdata";

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var payload = new
                    {
                        key = RJkey,
                        state_code = 8,
                        level_code = levelcode,
                        page = page,
                        limit = limit
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    string data = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        if (levelcode == 8)
                        {
                            var tempResponse = JsonConvert.DeserializeObject<NCDLevelCodeCommonResponse<NCDSociatyCategoryViewModel>>(data);

                            var flatList = tempResponse.result?
                                .Select(x => x.categoryDetails)
                                .ToList() ?? new List<NCDSociatyCategoryModel>();

                            var result = SaveSocietyAuditCategory(flatList);
                        }
                        else if (levelcode == 5)
                        {
                            var tempResponse = JsonConvert.DeserializeObject<NCDLevelCodeCommonResponse<NCDActivitiesKhadiGramDetailViewModel>>(data);

                            var flatList = tempResponse.result?
                                .Select(x => x.khadiGram)
                                .ToList() ?? new List<NCDActivitiesKhadiGramDetailModel>();
                            var result = SaveTypeOfActivitiesKhadiGrams(flatList);
                        }
                        else if (levelcode == 9)
                        {
                            var tempResponse = JsonConvert.DeserializeObject<NCDLevelCodeCommonResponse<NCDRegistrationAuthoritiesViewModel>>(data);

                            var flatList = tempResponse.result?
                                .Select(x => x.regAutho)
                                .ToList() ?? new List<NCDRegistrationAuthoritiesModel>();
                            var result = SaveRegistrationAuthorities(flatList);
                        }
                        else if (levelcode == 10)
                        {
                            var tempResponse = JsonConvert.DeserializeObject<NCDLevelCodeCommonResponse<NCDAreaOfOprationsViewModel>>(data);

                            var flatList = tempResponse.result?
                                .Select(x => x.areaOfOprations)
                                .ToList() ?? new List<NCDAreaOfOprationsModel>();
                            var result = SaveMasterAreaofOperationDetails(flatList);
                        }
                        else if (levelcode == 11)
                        {
                            var tempResponse = JsonConvert.DeserializeObject<NCDLevelCodeCommonResponse<NCDCooperativeSocietyBanksViewModel>>(data);

                            var flatList = tempResponse.result?
                                .Select(x => x.bank)
                                .ToList() ?? new List<NCDCooperativeSocietyBanksModel>();
                            var result = SaveCooperativeSocietyBanksDetails(flatList);
                        }
                        else if (levelcode == 12)
                        {
                            var tempResponse = JsonConvert.DeserializeObject<NCDLevelCodeCommonResponse<NCDDesignationsDetailsViewModel>>(data);

                            var flatList = tempResponse.result?
                                .Select(x => x.designations)
                                .ToList() ?? new List<NCDDesignationsDetailsModel>();
                            var result = SaveDesignationsDetails(flatList);
                        }
                        else if (levelcode == 13)
                        {
                            var tempResponse = JsonConvert.DeserializeObject<NCDLevelCodeCommonResponse<NCDCooperativeSocietyFacilitiesViewModel>>(data);

                            var flatList = tempResponse.result?
                                .Select(x => x.societyFacility)
                                .ToList() ?? new List<NCDCooperativeSocietyFacilitiesModel>();
                            var result = SaveSocietySocietyFacilities(flatList);
                        }
                        else if (levelcode == 14)
                        {
                            var tempResponse = JsonConvert.DeserializeObject<NCDLevelCodeCommonResponse<NCDCoverageAreasDetailsViewModel>>(data);

                            var flatList = tempResponse.result?
                                .Select(x => x.coverageArea)
                                .ToList() ?? new List<NCDCoverageAreasDetailsModel>();
                            var result = SaveCoverageAreasDetails(flatList);

                        }
                        else if (levelcode == 15)
                        {
                            var tempResponse = JsonConvert.DeserializeObject<NCDLevelCodeCommonResponse<NCDSubSectorViewModel>>(data);

                            var flatList = tempResponse.result?
                                .Select(x => x.SubDetails)
                                .ToList() ?? new List<NCDSubSectorModel>();
                            var result = SaveNcdSubSectorDetails(flatList);
                        }
                        else if (levelcode == 16)
                        {
                            var tempResponse = JsonConvert.DeserializeObject<NCDLevelCodeCommonResponse<NCDOfficeBuildingTypesViewModel>>(data);

                            var flatList = tempResponse.result?
                                .Select(x => x.buildingTypes)
                                .ToList() ?? new List<NCDOfficeBuildingTypesModel>();
                            var result = SaveOfficeBuildingTypes(flatList);
                        }
                        else if (levelcode == 17)
                        {
                            var tempResponse = JsonConvert.DeserializeObject<NCDLevelCodeCommonResponse<NCDWaterBodyTypesViewModel>>(data);

                            var flatList = tempResponse.result?
                                .Select(x => x.waterBodyType)
                                .ToList() ?? new List<NCDWaterBodyTypesModel>();
                            var result = SaveWaterBodyTypes(flatList);
                        }
                        else if (levelcode == 19)
                        {
                            var tempResponse = JsonConvert.DeserializeObject<NCDLevelCodeCommonResponse<NcdProductProduceViewModel>>(data);
                            var flatList = tempResponse.result?.Select(x => x.productProduceModel).ToList() ?? new List<NcdProductProduceModel>();
                            var result = SaveProductProduceBySociety(flatList);
                        }

                        responseData = JsonConvert.DeserializeObject<NCDLevelCodeCommonResponse>(data);
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NCDServiceModel",
                    FunctionName = $"GetLevelCodeMasterData(int levelcode = '{levelcode}')"
                }, ex);
            }

            return responseData;
        }
        public int SaveSocietyAuditCategory(List<NCDSociatyCategoryModel> nCDSociatyCategories)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdAuditCategory", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@M_NcdAuditcategory", BulkInsertToDataTable(nCDSociatyCategories));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDServiceModel", FunctionName = "SaveSocietyAuditCategory(List<NCDSociatyCategoryModel> nCDSociatyCategories)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }
        public int SaveTypeOfActivitiesKhadiGrams(List<NCDActivitiesKhadiGramDetailModel> nCDActivitiesKhadis)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdTypeOfActivitiesKhadiGrams", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@M_NcdKhadiGram", BulkInsertToDataTable(nCDActivitiesKhadis));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDServiceModel", FunctionName = "SaveTypeOfActivitiesKhadiGrams(List<NCDActivitiesKhadiGramDetailModel> nCDActivitiesKhadis)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }
        public int SaveRegistrationAuthorities(List<NCDRegistrationAuthoritiesModel> registrationAuthoritiesModels)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdRegistrationAuthoritiesDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@M_NcdAuthoritiesDetails", BulkInsertToDataTable(registrationAuthoritiesModels));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDServiceModel", FunctionName = "SaveRegistrationAuthorities(List<NCDRegistrationAuthoritiesModel> registrationAuthoritiesModels)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }
        public int SaveMasterAreaofOperationDetails(List<NCDAreaOfOprationsModel> nCDAreaOfOprations)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdAreaOfOperation", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@M_NcdAreaOperation", BulkInsertToDataTable(nCDAreaOfOprations));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDServiceModel", FunctionName = "SaveMasterAreaofOperationDetails(List<NCDAreaOfOprationsModel> nCDAreaOfOprations)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }
        public int SaveCooperativeSocietyBanksDetails(List<NCDCooperativeSocietyBanksModel> banksModels)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdCooperativeSocietyBank", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@M_NcdCooperativeBanks", BulkInsertToDataTable(banksModels));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDServiceModel", FunctionName = "SaveCooperativeSocietyBanksDetails(List<NCDCooperativeSocietyBanksModel> banksModels)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }
        public int SaveDesignationsDetails(List<NCDDesignationsDetailsModel> detailsModels)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdDesignation", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@M_NcdDesignation", BulkInsertToDataTable(detailsModels));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDServiceModel", FunctionName = "SaveDesignationsDetails(List<NCDDesignationsDetailsModel> detailsModels)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }
        public int SaveSocietySocietyFacilities(List<NCDCooperativeSocietyFacilitiesModel> societyFacilitiesModels)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdCooperativeSocietyFacility", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@M_NcdSocietyFacility", BulkInsertToDataTable(societyFacilitiesModels));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDServiceModel", FunctionName = "SaveSocietySocietyFacilities(List<NCDCooperativeSocietyFacilitiesModel> societyFacilitiesModels)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public int SaveCoverageAreasDetails(List<NCDCoverageAreasDetailsModel> nCDCoverageAreas)
        {
            nCDCoverageAreas = nCDCoverageAreas.GroupBy(x => x.ncd_id).Select(g => g.First()).ToList();
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdCoverageArea", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@M_NcdCoverageArea", BulkInsertToDataTable(nCDCoverageAreas));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDServiceModel", FunctionName = "SaveCoverageAreasDetails(List<NCDCoverageAreasDetailsModel> nCDCoverageAreas)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }
        public int SaveOfficeBuildingTypes(List<NCDOfficeBuildingTypesModel> buildingTypesModels)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdOfficeBuildingType", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@M_NcdOfficeBuilding", BulkInsertToDataTable(buildingTypesModels));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDServiceModel", FunctionName = "SaveOfficeBuildingTypes(List<NCDOfficeBuildingTypesModel> buildingTypesModels)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }
        public int SaveWaterBodyTypes(List<NCDWaterBodyTypesModel> nCDWaterBodies)
        {
            var lstData = nCDWaterBodies.Select(a => new
            {
                id = a.id,
                name = a.name,
                water_body_types_hindi = a.water_body_types_hindi
            }).ToList();
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdWaterBodyType", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@M_NcdWaterBody", BulkInsertToDataTable(lstData));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDServiceModel", FunctionName = "SaveWaterBodyTypes(List<NCDWaterBodyTypesModel> nCDWaterBodies)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }
        public int SaveProductProduceBySociety(List<NcdProductProduceModel> nCDProductProduce)
        {
            int result = 0;
            objDBAccess.OpenConnection();
            try
            {
                foreach (var items in nCDProductProduce)
                {
                    SqlCommand cmd = objDBAccess.CreateCommand("usp_ncdProductProduceBySociety", DBFactoryCommandType.PROCEDURE_COMMAND);
                    cmd.Parameters.AddWithValue("@id", items.id);
                    cmd.Parameters.AddWithValue("@name", items.name);
                    cmd.Parameters.AddWithValue("@primary_activity_id", items.primary_activity_id);
                    result = Convert.ToInt32(cmd.ExecuteScalar());
                }

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDServiceModel", FunctionName = "SaveProductProduceBySociety(List<NcdProductProduceModel> nCDProductProduce)" }, ex);

                throw;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return result;
        }


        //Added New For inserting master data 
        public async Task<NCDCooperativeRegistrationAPIResponse> GetCooperativeRegistrationDetail(int page = 1, int limit = 100000)
        {
            string url = $"{ncdApiBaseUrl}MasterApi/stateWiseBasicDetails";
            NCDCooperativeRegistrationAPIResponse responseData = new NCDCooperativeRegistrationAPIResponse();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var payload = new
                    {
                        key = RJkey,
                        state_code = 8,
                        page = page,
                        limit = limit
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    string data = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        responseData = JsonConvert.DeserializeObject<NCDCooperativeRegistrationAPIResponse>(data);
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDServiceModel", FunctionName = "GetStateWiseBasicDetails()" }, ex);

            }
            return responseData;
        }

        public int SaveCooperativeRegistrations(List<NCDCooperativeRegistrationResponseModel> _cooperativeRegiDetailModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstCooperativeRegistration = _cooperativeRegiDetailModel.Select(x => x).ToList();

                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistration", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@Action", "Save");
                cmd.Parameters.AddWithValue("@CooperativeRegistration", BulkInsertToDataTable(lstCooperativeRegistration));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveCooperativeRegistrations(List<NCDCooperativeRegistrationResponseModel> _cooperativeRegiDetailModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }


        public int SaveUCBDetail(List<NCDUCBViewModel> _cooperativeUCBDetailModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstCooperativeUCB = _cooperativeUCBDetailModel.Select(x => x.UCBDetails).ToList();

                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistrationUCB", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@Action", "Save");
                cmd.Parameters.AddWithValue("@CooperativeRegistrationUcb", BulkInsertToDataTable(lstCooperativeUCB));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveUCBDetail(List<NCDUCBViewModel> _cooperativeUCBDetailModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public int SavePacsLampsFssDetails(List<NCDPacsLampsFssViewModel> _cooperativePACSDetailModel)
        {
            int result = 0;
            try
            {
                _cooperativePACSDetailModel = _cooperativePACSDetailModel.GroupBy(x => x.PacsDetails.ncd_id).Select(a => a.First()).ToList();
                var lstCooperativePACS = _cooperativePACSDetailModel.Select(x => x.PacsDetails).ToList();
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistrationPACS", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@Action", "Save");
                cmd.Parameters.AddWithValue("@CooperativeRegistrationPacsLampsFss", BulkInsertToDataTable(lstCooperativePACS));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SavePacsLampsFssDetails(List<NCDPACSResponseModel> _cooperativePACSDetailModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }


        public int SaveFisheryDetail(List<NCDFisheryViewModel> _cooperativeFisheryDetailModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstCooperativeFishery = _cooperativeFisheryDetailModel.Select(x => x.FisheryDetail).ToList();

                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistrationFishery", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@Action", "Save");
                cmd.Parameters.AddWithValue("@CooperativeRegistrationFishery", BulkInsertToDataTable(lstCooperativeFishery));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveFisheryDetail(List<NCDFisheryViewModel> _cooperativeFisheryDetailModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }


        public int SaveSugarDetail(List<NCDSugarViewModel> _cooperativeSugarDetailModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstCooperativeSugar = _cooperativeSugarDetailModel.Select(x => x.SugarDetail).ToList();

                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistrationSugar", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@CooperativeRegistrationSugar", BulkInsertToDataTable(lstCooperativeSugar));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveSugarDetail(List<NCDSugarViewModel> _cooperativeSugarDetailModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }
        public int SaveHadloomDetail(List<NCDHandloomViewModel> _cooperativeHandloomDetailModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstCooperativeHandloom = _cooperativeHandloomDetailModel.Select(x => x.HandloomDetail).ToList();

                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistrationHandloom", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@Action", "Save");
                cmd.Parameters.AddWithValue("@CooperativeRegistrationHandloom", BulkInsertToDataTable(lstCooperativeHandloom));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveHadloomDetail(List<NCDHandloomViewModel> _cooperativeHandloomDetailModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public int SaveHandicraftDetail(List<NCDCooperativeRegistrationHandicraftViewModel> _cooperativeHandicraftDetailModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstCooperativeHandicraft = _cooperativeHandicraftDetailModel.Select(x => x.HandicraftDetail).ToList();

                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistrationHandicraft", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@CooperativeRegistrationHandicraft", BulkInsertToDataTable(lstCooperativeHandicraft));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveHandicraftDetail(List<NCDCooperativeRegistrationHandicraftViewModel> _cooperativeHandicraftDetailModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }


        public int SaveWomenWelfareDetail(List<NCDWomenWelfareViewModel> _WomenWelfareModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstWomenWelfare = _WomenWelfareModel.Select(x => x.WomenWelfareDetail).ToList();

                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistrationWomenWelfare", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@CooperativeRegistrationWomenWelfare", BulkInsertToDataTable(lstWomenWelfare));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveWomenWelfareDetail(List<NCDWomenWelfareViewModel> _WomenWelfareModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }


        public int SaveCreditThriftDetail(List<NCDCreditThriftViewModel> _CreditThriftModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstCreditThrift = _CreditThriftModel.Select(x => x.CreditDetail).ToList();
                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistrationCreditThrift", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@CooperativeRegistrationCreditThrift", BulkInsertToDataTable(lstCreditThrift));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveCreditThriftDetail(List<NCDCreditThriftViewModel> _CreditThriftModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public int SaveAgroProcessingDetail(List<NCDAgroProcessingViewModel> _AgroProcModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstAgroProcessing = _AgroProcModel.Select(x => x.AgroPDetail).ToList();

                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistrationAgroProcessing", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@CooperativeRegistrationAgroProcessing", BulkInsertToDataTable(lstAgroProcessing));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveAgroProcessingDetail(List<NCDAgroProcessingViewModel> _AgroProcModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }


        public int SaveCmiscellaneousDetail(List<NCDCmiscellaneousViewModel> _CmiscellModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstCmiscell = _CmiscellModel.Select(x => x.CmisceDetail).ToList();

                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistrationCmiscellaneous", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@CooperativeRegistrationCmiscellaneous", BulkInsertToDataTable(lstCmiscell));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveCmiscellaneousDetail(List<NCDCmiscellaneousViewModel> _CmiscellModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public int SaveHousingDetail(List<NCDHousingViewModel> _HousinglModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstHousing = _HousinglModel.Select(x => x.housingDetail).ToList();

                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistrationHousing", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@CooperativeRegistrationHousing", BulkInsertToDataTable(lstHousing));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveHousingDetail(List<NCDHousingViewModel> _HousinglModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }


        public int SaveLabourDetail(List<NCDLabourViewModel> _LabourlModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstLabour = _LabourlModel.Select(x => x.labourDetail).ToList();

                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistrationLabour", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@CooperativeRegistrationLabour", BulkInsertToDataTable(lstLabour));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveLabourDetail(List<NCDLabourViewModel> _LabourlModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }


        public int SaveLiveStockDetail(List<NCDLiveStockViewModel> _LiveStocklModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstLiveStock = _LiveStocklModel.Select(x => x.liveStDetail).ToList();

                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistrationLiveStock", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@CooperativeRegistrationLiveStock", BulkInsertToDataTable(lstLiveStock));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveLiveStockDetail(List<NCDLiveStockViewModel> _LiveStocklModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public int SaveTransportDetail(List<NCDTransportViewModel> _TranslModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstTransport = _TranslModel.Select(x => x.TransDetail).ToList();

                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistrationTransport", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@CooperativeRegistrationTransport", BulkInsertToDataTable(lstTransport));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveTransportDetail(List<NCDTransportViewModel> _TranslModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public int SaveAgricultureDetail(List<NCDAgricultureViewModel> _AgricultureModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstAgriculture = _AgricultureModel.Select(x => x.AgriDetail).ToList();

                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistrationAgriculture", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@CooperativeRegistrationAgriculture", BulkInsertToDataTable(lstAgriculture));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveAgricultureDetail(List<NCDAgricultureViewModel> _AgricultureModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }
        public int SaveBeeFarmingDetail(List<NCDBeeFarmingViewModel> _BeeFarmingModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstBeeFarming = _BeeFarmingModel.Select(x => x.BeeDetail).ToList();

                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistrationBeeFarming", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@CooperativeRegistrationBeeFarming", BulkInsertToDataTable(lstBeeFarming));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveBeeFarmingDetail(List<NCDBeeFarmingViewModel> _BeeFarmingModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public int SaveConsumerDetail(List<NCDConsumerViewModel> _CounsumerModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstConsumer = _CounsumerModel.Select(x => x.ConsDetail).ToList();

                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistrationConsumer", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@CooperativeRegistrationConsumer", BulkInsertToDataTable(lstConsumer));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveConsumerDetail(List<NCDConsumerViewModel> _CounsumerModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public int SaveMarketingDetail(List<NCDMarketingViewModel> _MarketingModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstMarketing = _MarketingModel.Select(x => x.MarkDetail).ToList();

                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistrationMarketing", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@CooperativeRegistrationMarketing", BulkInsertToDataTable(lstMarketing));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveMarketingDetail(List<NCDMarketingViewModel> _MarketingModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }


        public int SaveEducationTrainingDetail(List<NCDEducationTraningViewModel> _EducationTrainingModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstEducation = _EducationTrainingModel.Select(x => x.EduDetail).ToList();

                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistrationEducation", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@CooperativeRegistrationEducation", BulkInsertToDataTable(lstEducation));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveEducationTrainingDetail(List<NCDEducationTraningViewModel> _EducationTrainingModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public int SaveJuteDetail(List<NCDJuteViewModel> _JuteModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstJute = _JuteModel.Select(x => x.JuteDetail).ToList();

                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistrationJute", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@CooperativeRegistrationJute", BulkInsertToDataTable(lstJute));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveJuteDetail(List<NCDJuteViewModel> _JuteModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }


        public int SaveSericultureDetail(List<NCDSericultureViewModel> _SericultureModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstSericulture = _SericultureModel.Select(x => x.SeriDetail).ToList();

                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistrationSericulture", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@CooperativeRegistrationSericulture", BulkInsertToDataTable(lstSericulture));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveSericultureDetail(List<NCDSericultureViewModel> _SericultureModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }


        public int SaveSocialDetail(List<NCDSocialViewModel> _SocialModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstSocial = _SocialModel.Select(x => x.SocialDetail).ToList();

                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistrationSocial", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@CooperativeRegistrationSocial", BulkInsertToDataTable(lstSocial));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveSocialDetail(List<NCDSocialViewModel> _SocialModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public int SaveTourismDetail(List<NCDTourismViewModel> _TourismModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstTourism = _TourismModel.Select(x => x.TourDetail).ToList();

                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistrationTourism", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@CooperativeRegistrationTourism", BulkInsertToDataTable(lstTourism));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveTourismDetail(List<NCDTourismViewModel> _TourismModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }


        public int SaveTribalDetail(List<NCDTribalViewModel> _TribalModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstTribal = _TribalModel.Select(x => x.TribalDetail).ToList();

                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistrationTribal", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@CooperativeRegistrationTribal", BulkInsertToDataTable(lstTribal));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveTribalDetail(List<NCDTribalViewModel> _TribalModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }


        public int SaveKhadigramDetail(List<NCDKhadiGramViewModel> _KhadigramModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstKhadigram = _KhadigramModel.Select(x => x.KhadiDetail).ToList();
                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistrationKhadigram", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@CooperativeRegistrationKhadigram", BulkInsertToDataTable(lstKhadigram));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveKhadigramDetail(List<NCDKhadiGramViewModel> _KhadigramModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public int SaveDairyDetail(List<NCDDairyViewModel> _DairyModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                _DairyModel = _DairyModel.GroupBy(a => a.DairyDetail.ncd_id).Select(g => g.First()).ToList();
                var lstDairy = _DairyModel.Select(x => x.DairyDetail).ToList();

                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistrationDairy", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@Action", "Save");
                cmd.Parameters.AddWithValue("@CooperativeRegistrationDairy", BulkInsertToDataTable(lstDairy));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveDairyDetail(List<NCDDairyViewModel> _DairyModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public int SaveMultipurposeDetail(List<NCDMultipurposeViewModel> _MultippurposeModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstMultip = _MultippurposeModel.Select(x => x.MultiDetail).ToList();
                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_CooperativeRegistrationMultiPurpose", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@CooperativeRegistrationMultiPurpose", BulkInsertToDataTable(lstMultip));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveMultipurposeDetail(List<NCDMultipurposeViewModel> _MultippurposeModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }


        public int SaveMiscellaneousDetail(List<NCDMiscellaneousViewModel> _MiscellaneousModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstMiscellaneous = _MiscellaneousModel.Select(x => x.MisceDetail).ToList();
                SqlCommand cmd = objDBAccess.CreateCommand("ncd_cooperativeregistrationmiscellaneous", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@cooperativeregistrationmiscellaneous", BulkInsertToDataTable(lstMiscellaneous));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDService", FunctionName = "SaveMiscellaneousDetail(List<NCDMiscellaneousViewModel> _MiscellaneousModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        #region Save NCD Details 2025-08-26 @Pradeep

        public int SaveMemberDetails(List<MemberDetailsResultItem> nCDMembersDetails)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstMem = nCDMembersDetails.Select(x => x.MembersDetails).ToList();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdMemberDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@M_NcdMemberDetail", BulkInsertToDataTable(lstMem));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDServiceModel", FunctionName = "SaveMemberDetails(List<MemberDetailsResultItem> nCDMembersDetails)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public int SaveAreaofOperationRuralDetails(List<AreaofOperationRuralResultItem> resultItems)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstMem = resultItems.Select(x => x.Data).ToList();
                SqlCommand cmd = objDBAccess.CreateCommand("p_AreaofOperationRuralDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@AreaofOperationRuralDetailType", BulkInsertToDataTable(lstMem));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDServiceModel", FunctionName = "SaveAreaofOperationRuralDetails(NCD_AreaofOperationRuralDetails resultItems)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }


        public int SaveAreaofOperationUrbanDetails(List<AreaofOperationUrbanResultItem> resultItems)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstMem = resultItems.Select(x => x.Data).ToList();
                SqlCommand cmd = objDBAccess.CreateCommand("p_AreaofOperationUrbanDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@AreaofOperationUrbanDetailType", BulkInsertToDataTable(lstMem));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDServiceModel", FunctionName = "SaveAreaofOperationUrbanDetails(List<AreaofOperationUrbanResultItem> resultItems)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }
        public int SaveSocietyImplementingSchemesDetails(List<SocietyImplementingSchemesDetailsResultItem> resultItems)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstMem = resultItems.Select(x => new
                {
                    ncd_id = x.Data.ncd_id,
                    gov_scheme_name = x.Data.gov_scheme_name,
                    gov_scheme_type = x.Data.gov_scheme_type,
                    total_amount = x.Data.total_amount,
                    st_code = x.Data.st_code,
                    dist_code = x.Data.dist_code
                }).ToList();

                SqlCommand cmd = objDBAccess.CreateCommand("p_SocietyImplementingSchemesDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@SocietyImplementingSchemesDetailType", BulkInsertToDataTable(lstMem));
                result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDServiceModel", FunctionName = "SaveSocietyImplementingSchemesDetails(List<SocietyImplementingSchemesDetailsResultItem> resultItems)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public int SaveCooperativeRegistrationsLandsDetails(List<CooperativeRegistrationsLandsDetailsResultItem> resultItems)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstMem = resultItems.Select(x => x.Data).ToList();
                SqlCommand cmd = objDBAccess.CreateCommand("p_CooperativeRegistrationsLandsDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@CooperativeRegistrationsLandsDetailType", BulkInsertToDataTable(lstMem));
                result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDServiceModel", FunctionName = "SaveCooperativeRegistrationsLandsDetails(List<CooperativeRegistrationsLandsDetailsResultItem> resultItems)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }
        public int SaveSocietyAuditYearsDetails(List<NCDSociatyAuditYearsResultItem> resultItems)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                var lstMem = resultItems.Select(x => x.Data).ToList();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdSocietyAuditYearsDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@NcdSocietyAuditYearsDetailType", BulkInsertToDataTable(lstMem));
                result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDServiceModel", FunctionName = "SaveSocietyAuditYearsDetails(List<NCDSociatyAuditYearsResultItem> resultItems)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }


        public async Task<NCDAffilationUnionAPIResponse> GetAffilationUnionDetail()
        {
            string url = $"{ncdApiBaseUrl}Api/affilationunion";
            NCDAffilationUnionAPIResponse ResponseData = new NCDAffilationUnionAPIResponse();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var payload = new
                    {
                        key = RJkey,
                        state_code = 8
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    var data = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        ResponseData = JsonConvert.DeserializeObject<NCDAffilationUnionAPIResponse>(data);
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDAffilationUnionAPIResponse", FunctionName = "GetAffilationUnionDetail()" }, ex);
            }
            return ResponseData;
        }

        public int SaveAffilationUnionDetail(List<NCDAffilationUnionModel> _affilationUnionModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Ncd_AffiliatedUnions", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@M_NCDAffiliatedUnions", BulkInsertToDataTable(_affilationUnionModel));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDAffilationUnionModel", FunctionName = "SaveAffilationUnionDetail(List<NCDAffilationUnionModel> _affilationUnionModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }


        public async Task<NCDUnionFederationAPIResponse> GetUnionFederationDetail()
        {
            string url = $"{ncdApiBaseUrl}Api/fed/union-federation";
            NCDUnionFederationAPIResponse responseData = new NCDUnionFederationAPIResponse();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var payload = new
                    {
                        key = RJkey,
                        state_code = 8
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    var data = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        responseData = JsonConvert.DeserializeObject<NCDUnionFederationAPIResponse>(data);
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDUnionFederationAPIResponse", FunctionName = "GetUnionFederationDetail()" }, ex);
            }
            return responseData;
        }

        public int SaveFederationUnionDetail(List<NCDUnionFederationModel> _federationUnionModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("NCD_CooperativeBankFederationUnions", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@M_NCDFederationUnions", BulkInsertToDataTable(_federationUnionModel));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDUnionFederationModel", FunctionName = "SaveFederationUnionDetail(List<NCDUnionFederationModel> _federationUnionModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public async Task<NCDLevelAffiliatedUnionAPIResponse> GetLevelAffiliatedUnionDetail()
        {
            string url = $"{ncdApiBaseUrl}Api/fed/affiliated-union-levels";
            NCDLevelAffiliatedUnionAPIResponse responseData = new NCDLevelAffiliatedUnionAPIResponse();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var payload = new
                    {
                        key = RJkey,
                        state_code = 8
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    var data = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        responseData = JsonConvert.DeserializeObject<NCDLevelAffiliatedUnionAPIResponse>(data);
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDLevelAffiliatedUnionAPIResponse", FunctionName = "GetLevelAffiliatedUnionDetail()" }, ex);
            }
            return responseData;
        }

        public int SaveLevelAffiliatedUnionDetail(List<NCDLevelAffiliatedUnionModel> _levelAffiliateModel)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("NCD_levelAffiliatedUnions", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@M_NCDlevelAffiliatedUnion", BulkInsertToDataTable(_levelAffiliateModel));
                DataTable dtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                result = dtResult.Rows.Count;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDLevelAffiliatedUnionModel", FunctionName = "SaveLevelAffiliatedUnionDetail(List<NCDLevelAffiliatedUnionModel> _levelAffiliateModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }


        public async Task<CommonFinancialResponse<NCDFinancialResponseModel>> GetFinancialDetailByStatecode()
        {
            string url = $"{ncdApiBaseUrl}FinancialApi/financialDetails";
            var responseData = new CommonFinancialResponse<NCDFinancialResponseModel>();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var payload = new
                    {
                        key = RJkey,
                        state_code = 8
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    string data = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        responseData = JsonConvert.DeserializeObject<CommonFinancialResponse<NCDFinancialResponseModel>>(data);
                        responseData.statuscode = (int)response.StatusCode;
                        responseData.status = responseData.status;
                    }
                    else
                    {
                        responseData.statuscode = (int)response.StatusCode;
                        responseData.status = responseData.status;
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NCDServiceModel",
                    FunctionName = $"GetFinancialDetailByStatecode()"
                }, ex);
                responseData.statuscode = 500;
                responseData.status = responseData.status;

            }
            return responseData;
        }

        public int SaveFinancialDetailsData(List<NCDFinancialResponseModel> _financialList)
        {
            int result = 0;
            UserEntity _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_NcdFinancialDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@FinancialDetails", BulkInsertToDataTable(_financialList));
                result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SaveFinancialDetailsData", FunctionName = "SaveFinancialDetailsData(List<NCDFinancialResponseModel> _financialList)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public async Task<SocietyDetailResponse> DeleteSocietyDetail(SocietyRequestDataModel requestParams)
        {
            string url = $"{ncdApiBaseUrl}Rcs/deleteCooperative";
            var responseData = new SocietyDetailResponse();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var payload = new
                    {
                        key = RJkey,
                        state_code = 8,
                        ncd_id = requestParams.ncd_id,
                        cooperative_society_name = requestParams.cooperative_society_name,
                        registration_number = requestParams.registration_number,
                        date_registration = requestParams.date_registration,
                        sector_of_operation = requestParams.sector_of_operation,
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    string data = await response.Content.ReadAsStringAsync();
                    responseData = JsonConvert.DeserializeObject<SocietyDetailResponse>(data);
                    if (response.IsSuccessStatusCode)
                    {
                        responseData.statuscode = (int)response.StatusCode;
                        responseData.status = responseData.status;
                        responseData.message = responseData.message;
                        responseData.name = responseData.name;
                        responseData.deleted = responseData.deleted;
                    }
                    else
                    {
                        responseData.statuscode = (int)response.StatusCode;
                        responseData.status = responseData.status;
                        responseData.message = responseData.message;
                        responseData.name = responseData.name;
                        responseData.deleted = responseData.deleted;
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NCDServiceModel",
                    FunctionName = $"DeleteSocietyDetail(SocietyRequestDataModel requestParams)"
                }, ex);
                responseData.statuscode = 500;
                responseData.status = responseData.status;

            }
            return responseData;
        }

        public DataSet SocietySoftDeleted(long updatedBy, SocietyRequestDataModel model)
        {
            string result = string.Empty;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_SocietyDetailsDeleted", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@createdBy", DBFactoryParameterType.Int, updatedBy);
                objDBAccess.AddParameterToCommand("@cooperative_id", DBFactoryParameterType.VARCHAR, model.ncd_id);
                objDBAccess.AddParameterToCommand("@cooperative_society_name", DBFactoryParameterType.NVarchar, model.cooperative_society_name);
                objDBAccess.AddParameterToCommand("@registration_number", DBFactoryParameterType.NVarchar, model.registration_number);
                objDBAccess.AddParameterToCommand("@date_registration", DBFactoryParameterType.NVarchar, model.date_registration);
                objDBAccess.AddParameterToCommand("@sector_of_operation", DBFactoryParameterType.Int, model.sector_of_operation);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdServiceModel",
                    FunctionName = "SocietySoftDeleted(long updatedBy = '" + updatedBy + "', SocietyRequestDataModel model = '" + model + "')"
                }, ex);
                return null;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
        }


        #endregion

        public class NCDStateModel
        {
            public string state_code { get; set; }
            public string name { get; set; }
            public string hindi_name { get; set; }
            public string state_or_ut { get; set; }
        }

        public class NCDStateApiResponse
        {
            public string status { get; set; }
            public string message { get; set; }
            [JsonProperty("message")]
            public List<NCDStateModel> data { get; set; }

            [JsonProperty("Error Message")]
            public string ErrorMessage { get; set; }
        }

        public class NCDDistrictApiResponse
        {
            public string status { get; set; }
            public string state_or_ut { get; set; }
            public string state_or_ut_code { get; set; }
            public List<NCDDistrictModel> result { get; set; }
            public string message { get; set; }
            [JsonProperty("Error Message")]
            public string ErrorMessage { get; set; }
        }

        public class NCDDistrictModel
        {
            [JsonProperty("Sr No")]
            public int sr_no { get; set; }
            public int district_code { get; set; }
            public string district_name { get; set; }
        }

        public class NCDBlocksApiResponse
        {
            public string status { get; set; }
            public int code { get; set; }
            public NCDBlocksData data { get; set; }
            public string message { get; set; }
            [JsonProperty("Error Message")]
            public string ErrorMessage { get; set; }
        }

        public class NCDBlockModel
        {
            public int block_code { get; set; }
            [JsonProperty("name")]
            public string block_name { get; set; }
            public string block_name_hindi { get; set; }
            public int? block_version { get; set; }
            [JsonProperty("name_local")]
            public string block_name_local { get; set; }
            public int state_code { get; set; }
            public int district_code { get; set; }
        }

        public class NCDBlockViewModel
        {
            [JsonProperty("Blocks Details")]
            public NCDBlockModel BlockDetails { get; set; }
        }



        public class NCDBlocksData
        {
            public string state_name { get; set; }
            public int state_code { get; set; }
            public int total_records { get; set; }

            public List<NCDBlockViewModel> blocks { get; set; }
        }

        public class NCDVillagesAPIResponse
        {
            public string status { get; set; }
            public string state_name { get; set; }
            public int state_code { get; set; }
            public int records { get; set; }
            public List<NCDVillageViewModel> result { get; set; }
            public string message { get; set; }

            [JsonProperty("Error Message")]
            public string ErrorMessage { get; set; }
        }

        public class NCDVillageModel
        {

            public int gram_panchayat_code { get; set; }
            public string gram_panchayat_name { get; set; }
            public string gram_panchayat_name_hindi { get; set; }
            public int village_code { get; set; }
            public string village_name { get; set; }
            public int state_code { get; set; }
            public string state_name { get; set; }
            public int district_code { get; set; }
            public string district_name { get; set; }
            public int block_code { get; set; }
            public string block_name { get; set; }
        }

        public class NCDVillageViewModel
        {
            [JsonProperty("Districts - Blocks - GP - Villages Details")]
            public NCDVillageModel VillageDetails { get; set; }
        }

        public class NCDCitiesAPIResponse
        {
            public string status { get; set; }
            public string state_name { get; set; }
            public int state_code { get; set; }
            public int records { get; set; }
            public List<NCDCitiesViewModel> result { get; set; }
            public string message { get; set; }

            [JsonProperty("Error Message")]
            public string ErrorMessage { get; set; }
        }

        public class NCDCitiesModel
        {

            public int? localbody_type_code { get; set; }
            public string localbody_type_name { get; set; }
            public string localbody_type_name_hindi { get; set; }
            public int? localbody_code { get; set; }
            public string local_body_name { get; set; }
            public string local_body_name_hindi { get; set; }
            public int? state_code { get; set; }
            public string state_name { get; set; }
            public int? district_code { get; set; }
            public string district_name { get; set; }
        }

        public class NCDCitiesViewModel
        {
            [JsonProperty("Urban Local Bodies Details")]
            public NCDCitiesModel CitiesDetails { get; set; }
        }

        public class NCDWardApiResponse
        {
            public string status { get; set; }
            public string state_name { get; set; }
            public string state_code { get; set; }
            public int records { get; set; }
            public List<NCDWardViewModel> result { get; set; }
            public string message { get; set; }

            [JsonProperty("Error Message")]
            public string ErrorMessage { get; set; }
        }

        public class NCDWardModel
        {
            public int? local_body_code { get; set; }
            public string local_body_name { get; set; }
            public string local_body_hindi { get; set; }
            public int? ward_code { get; set; }
            public string ward_name { get; set; }
            public string ward_name_hindi { get; set; }
            public int? state_code { get; set; }
            public string state_name { get; set; }
        }
        public class NCDWardViewModel
        {

            [JsonProperty("Urban Local Bodies Ward Details")]
            public NCDWardModel WardDetails { get; set; }
        }

        public class NCDSectorAPIResponse
        {
            public string status { get; set; }
            public string message { get; set; }

            [JsonProperty("message")]
            public List<NCDSectorModel> data { get; set; }

            [JsonProperty("Error Message")]
            public string ErrorMessage { get; set; }
        }

        public class NCDSectorModel
        {
            [JsonProperty("id")]
            public int id { get; set; }

            [JsonProperty("name")]
            public string name { get; set; }

            [JsonProperty("hindi_name")]
            public string hindi_name { get; set; }
        }

        public class NCDLevelCodeCommonResponse
        {
            public string status { get; set; }
            public string state_name { get; set; }
            public int state_code { get; set; }
            public string api_type { get; set; }
            public int records { get; set; }
            public string message { get; set; }
            [JsonProperty("Error Message")]
            public string ErrorMessage { get; set; }
        }
        public class NCDLevelCodeCommonResponse<T> : NCDLevelCodeCommonResponse
        {
            public List<T> result { get; set; }
        }

        public class DynamicPropertyResolver : DefaultContractResolver
        {
            private readonly string _levelPropertyName;

            public DynamicPropertyResolver(string levelPropertyName)
            {
                _levelPropertyName = levelPropertyName;
            }

            protected override string ResolvePropertyName(string propertyName)
            {
                if (propertyName == "result")
                    return _levelPropertyName;   // 👈 change dynamically
                return base.ResolvePropertyName(propertyName);
            }
        }

        public class NCDSubSectorAPIResponse
        {
            public string status { get; set; }
            public string state_name { get; set; }
            public int state_code { get; set; }
            public string api_type { get; set; }
            public int records { get; set; }
            public List<NCDSubSectorViewModel> result { get; set; }
            public string message { get; set; }

            [JsonProperty("Error Message")]
            public string ErrorMessage { get; set; }
        }
        public class NCDSubSectorModel
        {
            public int? id { get; set; }
            public int? primary_activities_id { get; set; }
            public int? sub_sector_id { get; set; }
            public string sub_sector_name { get; set; }
            public string sub_sector_name_hindi { get; set; }
        }

        public class NCDSubSectorViewModel
        {
            [JsonProperty("Sub Sector Details")]
            public NCDSubSectorModel SubDetails { get; set; }
        }

        public class NCDSocietyCategoryAPIResponse
        {
            public string status { get; set; }
            public string state_name { get; set; }
            public int? state_code { get; set; }
            public string api_type { get; set; }
            public int records { get; set; }
            public List<NCDSociatyCategoryViewModel> result { get; set; }
            public string message { get; set; }

            [JsonProperty("Error Message")]
            public string ErrorMessage { get; set; }

        }

        public class NCDSociatyCategoryViewModel
        {
            [JsonProperty("Society Audit Years Details")]
            public NCDSociatyCategoryModel categoryDetails { get; set; }
        }

        public class NCDSociatyCategoryModel
        {
            public int? id { get; set; }
            public string name { get; set; }
            public string audit_categories_hindi { get; set; }
            public int? status { get; set; }
        }
        public class NCDActivitiesKhadiGramDetailViewModel
        {
            [JsonProperty("Type Of Activities Khadi Gram Details")]
            public NCDActivitiesKhadiGramDetailModel khadiGram { get; set; }
        }

        public class NCDActivitiesKhadiGramDetailModel
        {
            public int? id { get; set; }
            public string type_of_activities_name { get; set; }
            public string type_of_activities_name_hindi { get; set; }
            public int? status { get; set; }
        }


        public class NCDMembersDetailsModel
        {

            [JsonProperty("ncd_id")]
            public string ncd_id { get; set; }

            [JsonProperty("m_general")]
            public int? m_general { get; set; }

            [JsonProperty("m_sc")]
            public int? m_sc { get; set; }

            [JsonProperty("m_st")]
            public int? m_st { get; set; }

            [JsonProperty("m_obc")]
            public int? m_obc { get; set; }
            [JsonProperty("f_general")]
            public int? f_general { get; set; }

            [JsonProperty("f_sc")]
            public int? f_sc { get; set; }
            [JsonProperty("f_st")]
            public int? f_st { get; set; }

            [JsonProperty("f_obc")]
            public int? f_obc { get; set; }
            [JsonProperty("t_general")]
            public int? t_general { get; set; }

            [JsonProperty("t_sc")]
            public int? t_sc { get; set; }

            [JsonProperty("t_st")]
            public int? t_st { get; set; }
            [JsonProperty("t_obc")]
            public int? t_obc { get; set; }

            [JsonProperty("state_code")]
            public int? state_code { get; set; }

            [JsonProperty("dist_code")]
            public int? dist_code { get; set; }
        }

        public class NCDRegistrationAuthoritiesViewModel
        {
            [JsonProperty("Registration Authorities Details")]
            public NCDRegistrationAuthoritiesModel regAutho { get; set; }
        }
        public class NCDRegistrationAuthoritiesModel
        {
            public int? id { get; set; }
            public string authority_name { get; set; }
            public string authority_name_hindi { get; set; }
            public int? primary_activity { get; set; }
        }

        public class NCDAreaOfOprationsViewModel
        {
            [JsonProperty("Area Of Operations Details")]
            public NCDAreaOfOprationsModel areaOfOprations { get; set; }
        }
        public class NCDAreaOfOprationsModel
        {
            public Int32 id { get; set; }
            public Int32 urban_rural { get; set; }
            public string name { get; set; }
        }
        public class NCDCooperativeSocietyBanksViewModel
        {
            [JsonProperty("Area Of Operations Details")]
            public NCDCooperativeSocietyBanksModel bank { get; set; }
        }
        public class NCDCooperativeSocietyBanksModel
        {
            public int? id { get; set; }
            public string bank_name { get; set; }
            public int? bank_type { get; set; }
        }
        public class NCDDesignationsDetailsViewModel
        {
            [JsonProperty("Designations Details")]
            public NCDDesignationsDetailsModel designations { get; set; }
        }

        public class NCDDesignationsDetailsModel
        {
            public int? id { get; set; }
            public string name { get; set; }
            public string designations_hindi { get; set; }
        }
        public class NCDCooperativeLandDetailModel
        {
            public string ncd_id { get; set; }
            public decimal? land_owned { get; set; }
            public string land_leased { get; set; }
            public string land_allotted { get; set; }
            public decimal? land_total { get; set; }
            public int? st_code { get; set; }
            public int? dist_code { get; set; }

        }


        public class NCDCooperativeSocietyFacilitiesViewModel
        {
            [JsonProperty("Cooperative Society Facilities Details")]
            public NCDCooperativeSocietyFacilitiesModel societyFacility { get; set; }
        }
        public class NCDCooperativeSocietyFacilitiesModel
        {
            public int? id { get; set; }
            public string name { get; set; }
            public int? primary_activity_id { get; set; }
        }

        public class NCDOfficeBuildingTypesViewModel
        {
            [JsonProperty("Sub Sector Details")]
            public NCDOfficeBuildingTypesModel buildingTypes { get; set; }
        }
        public class NCDOfficeBuildingTypesModel
        {
            public int? id { get; set; }
            public string name { get; set; }
            public string office_building_types_hindi { get; set; }
        }
        public class NCDWaterBodyTypesViewModel
        {
            [JsonProperty("Sub Sector Details")]
            public NCDWaterBodyTypesModel waterBodyType { get; set; }
        }
        public class NCDWaterBodyTypesModel
        {
            public int? id { get; set; }
            public string name { get; set; }
            public string water_body_types_hindi { get; set; }
            public int? primary_activities_id { get; set; }
            public int? sub_sector_id { get; set; }
            public string sub_sector_name { get; set; }
            public string sub_sector_name_hindi { get; set; }
        }
        public class NCDCoverageAreasDetailsViewModel
        {
            [JsonProperty("Coverage Areas Details")]
            public NCDCoverageAreasDetailsModel coverageArea { get; set; }
        }

        public class NCDCoverageAreasDetailsModel
        {
            public string ncd_id { get; set; }
            public int? area_of_operation_id { get; set; }
            public int? state_code { get; set; }
            public int? district_code { get; set; }
            public int? block_code { get; set; }
            public int? panchayat_code { get; set; }
            public int? village_code { get; set; }
            public int? sector_of_operation { get; set; }
            public int? is_draft { get; set; }
            public int? is_approved { get; set; }
            public int? functional_status { get; set; }
            public int? location_of_head_quarter { get; set; }
            public string operation_area_location { get; set; }
            public DateTime? date_registration { get; set; }
        }

        public class NcdProductProduceViewModel
        {
            [JsonProperty("Product Produce By Society Details")]
            public NcdProductProduceModel productProduceModel { get; set; }
        }

        public class NcdProductProduceModel
        {
            public int? id { get; set; }
            public string name { get; set; }
            public int? primary_activity_id { get; set; }
        }
        public class NCDPagination
        {
            [JsonProperty("page")]
            public int? Page { get; set; }

            [JsonProperty("limit")]
            public int? Limit { get; set; }

            [JsonProperty("total_records")]
            public int? TotalRecords { get; set; }

            [JsonProperty("total_pages")]
            public int? TotalPages { get; set; }

        }


        public class NCDSectorChildWiseDetailAPIResponse
        {
            public string status { get; set; }

            public string state_name { get; set; }

            public int? state_code { get; set; }
            public string sector { get; set; }
            public int? sector_code { get; set; }
            public int? records { get; set; }
            public List<NCDSectorChildWiseDetailViewModel> result { get; set; }
            public string message { get; set; }

            [JsonProperty("Error Message")]
            public string ErrorMessage { get; set; }

        }

        public class NCDSectorChildWiseDetailViewModel
        {
            [JsonProperty("Women Welfare Details")]
            public NCDStateSectorChildDetailModel sectorChildDetail { get; set; }
        }

        public class NCDStateSectorChildDetailModel
        {
            public string ncd_id { get; set; }
            public int? type_society { get; set; }
            public int? has_building { get; set; }
            public string building_type { get; set; }
            public string authorised_share { get; set; }
            public string annual_turn_over { get; set; }
            public string is_raw_material_taken { get; set; }
            public string facilities { get; set; }
        }



        public class NCDUCBViewModel
        {
            [JsonProperty("UCB Details")]
            public NCDUCBResponseModel UCBDetails { get; set; }
        }
        public class NCDUCBResponseModel
        {
            public bool has_building { get; set; }
            public int? building_type { get; set; }
            public decimal? ucb_branch { get; set; }
            public string has_nafcub { get; set; }
            public decimal? authorised_share { get; set; }
            public decimal? annual_turn_over { get; set; }
            public decimal? annual_income { get; set; }
            public decimal? annual_ucb_expenditr { get; set; }
            public decimal? asset_ucb { get; set; }
            public decimal? liability_ucb { get; set; }
            public decimal? total_deposit { get; set; }
            public decimal? loan_outstanding { get; set; }
            public string is_gov_scheme_implemented { get; set; }
            public string is_computerized { get; set; }
            public int? no_computer_working { get; set; }
            public string have_ifsc { get; set; }
            public string have_corebanking { get; set; }
            public string have_doorstepservice { get; set; }
            public string is_aeps { get; set; }
            public string offer_debitcard { get; set; }
            public string have_internetbanking { get; set; }
            public string offer_creditcard { get; set; }
            public string cibil_membership { get; set; }
            public string conducting_gab { get; set; }
            public string cgtmsemli_member { get; set; }
            public string is_saf_to_cust { get; set; }
            public decimal? networth { get; set; }
            public string fswm_comp { get; set; }
            public string ncd_id { get; set; }
            public string paid_up_share_date { get; set; }
            public string Paid_up_share { get; set; }
        }
        public class NCDFisheryViewModel
        {
            [JsonProperty("Fishery Details")]
            public NCDFisheryResponseModel FisheryDetail { get; set; }
        }
        public class NCDFisheryResponseModel
        {
            public string ncd_id { get; set; }
            public decimal? annual_fish_catch { get; set; }
            public int? credit_facility { get; set; }
            public decimal? total_credit_provided { get; set; }
            public int? fuel_distribution { get; set; }
            public int? marketing { get; set; }
            public int? cold_storage { get; set; }
            public int? transportation { get; set; }
            public string other_facility { get; set; }
            public int? is_fpo_fisheries { get; set; }
            public string fpo_fisheries_details { get; set; }
            public string authorised_share { get; set; }
            public string paid_up_share { get; set; }
            public string total_deposit { get; set; }
            public string pack_total_outstanding_loan { get; set; }
            public string paid_up_share_date { get; set; }
        }
        public class NCDSugarViewModel
        {
            [JsonProperty("Sugar Details")]
            public NCDSugarResponseModel SugarDetail { get; set; }
        }

        public class NCDSugarResponseModel
        {
            public string ncd_id { get; set; }
            public int? has_building { get; set; }
            public int? building_type { get; set; }
            public decimal? authorised_share { get; set; }
            public decimal? paid_up_members { get; set; }
            public decimal? paid_up_government_bodies { get; set; }
            public decimal? paid_up_total { get; set; }
            public int? suger_mills_no { get; set; }
            public decimal? build_up_area { get; set; }
            public decimal? open_land_area { get; set; }
            public decimal? total_area { get; set; }
            public decimal? liecensed_capicity { get; set; }
            public decimal? installed_capicity { get; set; }
            public string crushing_period_start { get; set; }
            public string crushing_period_end { get; set; }
            public string product_produced { get; set; }
            public int? retail_shops { get; set; }
            public Int32? retail_shops_no { get; set; }
            public int? sugercane_input_provided { get; set; }
            public int? loan_facility { get; set; }
            public int? waste_management { get; set; }
            public int? central_government_benefits { get; set; }
            public int? state_government_benefits { get; set; }
            public decimal? annual_turn_over { get; set; }
            public string total_deposit { get; set; }
            public string pack_total_outstanding_loan { get; set; }
            public string paid_up_share_date { get; set; }
        }

        public class NCDHandloomViewModel
        {
            [JsonProperty("Handloom Details")]
            public NCDHandloomResponseModel HandloomDetail { get; set; }
        }

        public class NCDHandloomResponseModel
        {
            public string ncd_id { get; set; }
            public int? has_building { get; set; }
            public int? building_type { get; set; }
            public decimal? authorised_share { get; set; }
            public decimal? paid_up_members { get; set; }
            public decimal? paid_up_government_bodies { get; set; }
            public decimal? paid_up_total { get; set; }
            public decimal? annual_turn_over { get; set; }
            public int? individual_member { get; set; }
            public int? institutional_member { get; set; }
            public Int32? total_members { get; set; }
            public string power_loom_type { get; set; }
            public string hand_loom_type { get; set; }
            public int? no_of_loom { get; set; }
            public string raw_product_taken { get; set; }
            public string raw_material_available { get; set; }
            public string waste_generate { get; set; }
            public string waste_available { get; set; }
            public string operate_retail { get; set; }
            public int? no_of_retail { get; set; }
            public string product_sale_out { get; set; }
            public string operated_member_themself { get; set; }
            public string is_user_work_divide { get; set; }
            public string total_deposit { get; set; }
            public string pack_total_outstanding_loan { get; set; }
            public string paid_up_share_date { get; set; }

        }


        public class NCDCooperativeRegistrationHandicraftViewModel
        {
            [JsonProperty("Handicraft Details")]
            public NCDHandicraftResponseModel HandicraftDetail { get; set; }
        }
        public class NCDHandicraftResponseModel
        {
            public string ncd_id { get; set; }
            public int? has_building { get; set; }
            public int? building_type { get; set; }
            public decimal? authorised_share { get; set; }
            public decimal? paid_up_members { get; set; }
            public decimal? paid_up_government_bodies { get; set; }
            public decimal? paid_up_total { get; set; }
            public int? individual_member { get; set; }
            public int? institutional_member { get; set; }
            public Int32? total_members { get; set; }
            public decimal? annual_turn_over { get; set; }
            public string type_raw { get; set; }
            public string type_produce { get; set; }
            public int? common_work_place { get; set; }
            public int? workplace_operate { get; set; }
            public int? is_work_by_member { get; set; }
            public int? is_training_provide { get; set; }
            public int? is_raw_provide { get; set; }
            public int? is_raw_easy_avail { get; set; }
            public int? is_waste_generate { get; set; }
            public int? is_waste_facility { get; set; }
            public int? is_operate_retail { get; set; }
            public Int32? no_of_retail { get; set; }
            public int? is_product_sale_out { get; set; }
            public string facilities { get; set; }
            public string total_deposit { get; set; }
            public string pack_total_outstanding_loan { get; set; }
            public string paid_up_share_date { get; set; }
        }

        public class NCDWomenWelfareViewModel
        {
            [JsonProperty("Women Welfare Details")]
            public NCDWomenWelfareResponseModel WomenWelfareDetail { get; set; }
        }

        public class NCDWomenWelfareResponseModel
        {
            public string ncd_id { get; set; }
            public int? type_society { get; set; }
            public int? has_building { get; set; }
            public int? building_type { get; set; }
            public decimal? authorised_share { get; set; }
            public decimal? annual_turn_over { get; set; }
            public int? is_raw_material_taken { get; set; }
            public string facilities { get; set; }
            public string total_deposit { get; set; }
            public string pack_total_outstanding_loan { get; set; }
            public string paid_up_share_date { get; set; }
            public string Paid_up_share { get; set; }

        }

        public class NCDCreditThriftViewModel
        {
            [JsonProperty("Credit And Thrift Details")]
            public NCDCreditThriftResponseModel CreditDetail { get; set; }
        }

        public class NCDCreditThriftResponseModel
        {
            public bool? has_building { get; set; }
            public int? building_type { get; set; }
            public decimal? authorised_share { get; set; }
            public decimal? paid_up_share { get; set; }
            public decimal? total_deposit { get; set; }
            public decimal? pack_total_outstanding_loan { get; set; }
            public string facilities { get; set; }
            public int? st_code { get; set; }
            public int? dist_code { get; set; }
            public string ncd_id { get; set; }
            public string Paid_up_share_date { get; set; }
        }


        public class NCDAgroProcessingViewModel
        {
            [JsonProperty("Agro Processing / Industrial Cooperative Details")]
            public NCDAgroProcessingResponseModel AgroPDetail { get; set; }
        }

        public class NCDAgroProcessingResponseModel
        {
            public int? individual_member { get; set; }
            public int? institutional_member { get; set; }
            public int? total_member { get; set; }
            public int? type_society { get; set; }
            public int? has_building { get; set; }
            public int? building_type { get; set; }
            public decimal? authorised_share { get; set; }
            public decimal? paid_up_members { get; set; }
            public decimal? paid_up_government_bodies { get; set; }
            public decimal? paid_up_total { get; set; }
            public decimal? annual_turn_over { get; set; }
            public int? processing_unit { get; set; }
            public Int32? processing_unit_number { get; set; }
            public int? processing_by_members { get; set; }
            public int? work_divided { get; set; }
            public int? product_taken { get; set; }
            public int? material_available { get; set; }
            public int? wastes_generated { get; set; }
            public int? waste_management_facility { get; set; }
            public int? operate_shops { get; set; }
            public string operate_shops_number { get; set; }
            public int? product_sale_out_of_area { get; set; }
            public string ncd_id { get; set; }
            public int? st_code { get; set; }
            public int? dist_code { get; set; }
            public string total_deposit { get; set; }
            public string pack_total_outstanding_loan { get; set; }
            public string paid_up_share_date { get; set; }

        }


        public class NCDCmiscellaneousViewModel
        {
            [JsonProperty("Miscellaneous Credit Cooperative Details")]
            public NCDCmiscellaneousResponseModel CmisceDetail { get; set; }
        }

        public class NCDCmiscellaneousResponseModel
        {
            public int? has_building { get; set; }
            public int? building_type { get; set; }
            public decimal? authorised_share { get; set; }
            public decimal? paid_up_members { get; set; }
            public decimal? paid_up_government_bodies { get; set; }
            public decimal? paid_up_total { get; set; }
            public decimal? annual_turn_over { get; set; }
            public decimal? total_deposit { get; set; }
            public decimal? loan_outstanding { get; set; }
            public int? individual_member { get; set; }
            public int? institutional_member { get; set; }
            public string facilities { get; set; }
            public string ncd_id { get; set; }
            public int? st_code { get; set; }
            public int? dist_code { get; set; }
            public string Paid_up_share_date { get; set; }
        }

        public class NCDHousingViewModel
        {
            [JsonProperty("Miscellaneous Credit Cooperative Details")]
            public NCDHousingResponseModel housingDetail { get; set; }
        }


        public class NCDHousingResponseModel
        {
            public int? type_society { get; set; }
            public int? has_building { get; set; }
            public int? building_type { get; set; }
            public string has_land { get; set; }
            public decimal? authorised_share { get; set; }
            public decimal? paid_up_members { get; set; }
            public decimal? paid_up_government_bodies { get; set; }
            public decimal? paid_up_total { get; set; }
            public decimal? annual_turn_over { get; set; }
            public decimal? annual_expenses { get; set; }
            public string loan_facilities { get; set; }
            public int? number_of_houses_audit_year { get; set; }
            public int? number_of_houses_during_year { get; set; }
            public int? number_of_houses_construction { get; set; }
            public string facilities { get; set; }
            public string ncd_id { get; set; }
            public int? st_code { get; set; }
            public int? dist_code { get; set; }
            public string total_deposit { get; set; }
            public string pack_total_outstanding_loan { get; set; }
            public string paid_up_share_date { get; set; }

        }

        public class NCDLabourViewModel
        {
            [JsonProperty("Labour Cooperative Details")]
            public NCDLabourResponseModel labourDetail { get; set; }
        }
        public class NCDLabourResponseModel
        {
            public int? type_society { get; set; }
            public int? has_building { get; set; }
            public int? building_type { get; set; }
            public decimal? authorised_share { get; set; }
            public decimal? paid_up_members { get; set; }
            public decimal? paid_up_government_bodies { get; set; }
            public decimal? paid_up_total { get; set; }
            public decimal? annual_turn_over { get; set; }
            public decimal? annual_expenses { get; set; }
            public string work_allot_state_dist_federation { get; set; }
            public string work_guide_state_dist_federation { get; set; }
            public string concession_state_gov { get; set; }
            public string concession_centre_gov { get; set; }
            public string facilities { get; set; }
            public string ncd_id { get; set; }
            public int? st_code { get; set; }
            public int? dist_code { get; set; }
            public string total_deposit { get; set; }
            public string pack_total_outstanding_loan { get; set; }
            public string paid_up_share_date { get; set; }

        }

        public class NCDLiveStockViewModel
        {
            [JsonProperty("Livestock & Poultry Cooperative Details")]
            public NCDLiveStockResponseModel liveStDetail { get; set; }
        }
        public class NCDLiveStockResponseModel
        {
            public string type_society { get; set; }
            public int? has_building { get; set; }
            public int? building_type { get; set; }
            public decimal? authorised_share { get; set; }
            public decimal? paid_up_members { get; set; }
            public decimal? paid_up_government_bodies { get; set; }
            public decimal? paid_up_total { get; set; }
            public decimal? individual_member { get; set; }
            public decimal? institutional_member { get; set; }
            public decimal? annual_turn_over { get; set; }
            public string type_produce { get; set; }
            public string common_work_place { get; set; }
            public string is_work_by_member { get; set; }
            public string is_training_provide { get; set; }
            public string is_poultry_feed { get; set; }
            public string is_collected_from_member { get; set; }
            public string is_waste_facility { get; set; }
            public string is_operate_retail { get; set; }
            public int? no_of_retail { get; set; }
            public string is_product_sale_out { get; set; }
            public string facilities { get; set; }
            public string ncd_id { get; set; }
            public int? st_code { get; set; }
            public int? dist_code { get; set; }
            public string total_deposit { get; set; }
            public string pack_total_outstanding_loan { get; set; }
            public string paid_up_share_date { get; set; }
            public string Paid_up_share { get; set; }
        }

        public class NCDTransportViewModel
        {
            [JsonProperty("Transport Cooperative Details")]
            public NCDTransportResponseModel TransDetail { get; set; }
        }
        public class NCDTransportResponseModel
        {
            public int? type_society { get; set; }
            public int? has_building { get; set; }
            public int? building_type { get; set; }
            public decimal? authorised_share { get; set; }
            public decimal? paid_up_members { get; set; }
            public decimal? paid_up_government_bodies { get; set; }
            public decimal? paid_up_total { get; set; }
            public decimal? annual_turn_over { get; set; }
            public string individual_member { get; set; }
            public string institutional_member { get; set; }
            public int? type_owner { get; set; }
            public string type_owner_name { get; set; }
            public int? bus_type_detail { get; set; }
            public int? truck_type_detail { get; set; }
            public int? other_type_detail { get; set; }
            public int? no_passenger_vehicle { get; set; }
            public int? no_member_travel { get; set; }
            public int? no_freight_vehicle { get; set; }
            public int? quantity_good_transport { get; set; }
            public string member_themself { get; set; }
            public string is_user_transport_facility { get; set; }
            public string ncd_id { get; set; }
            public int? st_code { get; set; }
            public int? dist_code { get; set; }
            public string total_deposit { get; set; }
            public string pack_total_outstanding_loan { get; set; }
            public string paid_up_share_date { get; set; }
        }

        public class NCDAgricultureViewModel
        {
            [JsonProperty("Agriculture & Allied Cooperative Details")]
            public NCDAgricultureResponseModel AgriDetail { get; set; }
        }
        public class NCDAgricultureResponseModel
        {
            public string type_society { get; set; }
            public int? has_building { get; set; }
            public int? building_type { get; set; }
            public decimal? authorised_share { get; set; }
            public decimal? paid_up_members { get; set; }
            public decimal? paid_up_government_bodies { get; set; }
            public decimal? paid_up_total { get; set; }
            public decimal? annual_turn_over { get; set; }
            public string individual_member { get; set; }
            public int? institutional_member { get; set; }
            public string has_pool_land { get; set; }
            public string has_gov_land { get; set; }
            public string member_vested_right { get; set; }
            public string is_member_work { get; set; }
            public string society_common_pool { get; set; }
            public string is_utilize_pool { get; set; }
            public string harvesting { get; set; }
            public string farming_mech { get; set; }
            public string irrigation_means { get; set; }
            public string ncd_id { get; set; }
            public int? st_code { get; set; }
            public int? dist_code { get; set; }
            public string total_deposit { get; set; }
            public string pack_total_outstanding_loan { get; set; }
            public string paid_up_share_date { get; set; }
        }

        public class NCDBeeFarmingViewModel
        {
            [JsonProperty("Bee Cooperative Details")]
            public NCDBeeFarmingResponseModel BeeDetail { get; set; }
        }

        public class NCDBeeFarmingResponseModel
        {
            public string ncd_id { get; set; }
            public string type_bee { get; set; }
            public int? has_building { get; set; }
            public int? building_type { get; set; }
            public decimal? authorised_share { get; set; }
            public decimal? paid_up_members { get; set; }
            public decimal? paid_up_government_bodies { get; set; }
            public decimal? paid_up_total { get; set; }
            public decimal? annual_turn_over { get; set; }
            public int? common_yard { get; set; }
            public Int32? no_of_behives { get; set; }
            public string type_behives { get; set; }
            public int? rear_by_member { get; set; }
            public int? guidance_by_member { get; set; }
            public string type_product { get; set; }
            public int? is_bee_plant_grow { get; set; }
            public int? is_cleaning_process { get; set; }
            public int? is_waste_facility { get; set; }
            public int? own_brand_honey { get; set; }
            public int? is_operate_retail { get; set; }
            public Int32? no_of_retail { get; set; }
            public int? is_product_sale_out { get; set; }
            public string facilities { get; set; }
            public string total_deposit { get; set; }
            public string pack_total_outstanding_loan { get; set; }
            public string paid_up_share_date { get; set; }
        }


        public class NCDConsumerViewModel
        {
            [JsonProperty("Consumer Cooperative Details")]
            public NCDConsumerResponseModel ConsDetail { get; set; }
        }

        public class NCDConsumerResponseModel
        {
            public int? has_building { get; set; }
            public string has_store { get; set; }
            public int? no_of_outlets { get; set; }
            public int? building_type { get; set; }
            public decimal? authorised_share { get; set; }
            public decimal? paid_up_share { get; set; }
            public decimal? annual_turn_over { get; set; }
            public string facilities { get; set; }
            public int? st_code { get; set; }
            public int? dist_code { get; set; }
            public string ncd_id { get; set; }
            public string total_deposit { get; set; }
            public string pack_total_outstanding_loan { get; set; }
            public string paid_up_share_date { get; set; }
        }


        public class NCDMarketingViewModel
        {
            [JsonProperty("Marketing Cooperative Details")]
            public NCDMarketingResponseModel MarkDetail { get; set; }
        }

        public class NCDMarketingResponseModel
        {
            public int? has_building { get; set; }
            public int? building_type { get; set; }
            public string has_land { get; set; }
            public string has_warehouses { get; set; }
            public decimal? capacity_warehouses { get; set; }
            public decimal? authorised_share { get; set; }
            public decimal? paid_up_members { get; set; }
            public decimal? paid_up_government_bodies { get; set; }
            public decimal? paid_up_total { get; set; }
            public decimal? annual_turn_over { get; set; }
            public decimal? annual_expenses { get; set; }
            public string liecense_to_sell { get; set; }
            public string sell_the_item { get; set; }
            public string ncd_id { get; set; }
            public int? st_code { get; set; }
            public int? dist_code { get; set; }
            public string total_deposit { get; set; }
            public string pack_total_outstanding_loan { get; set; }
            public string paid_up_share_date { get; set; }

        }


        public class NCDEducationTraningViewModel
        {
            [JsonProperty("Education Cooperative Details")]
            public NCDEducationTraningResponseModel EduDetail { get; set; }
        }

        public class NCDEducationTraningResponseModel
        {
            public int? type_society { get; set; }
            public int? has_building { get; set; }
            public int? building_type { get; set; }
            public string has_land { get; set; }
            public decimal? authorised_share { get; set; }
            public decimal? paid_up_members { get; set; }
            public decimal? paid_up_government_bodies { get; set; }
            public decimal? paid_up_total { get; set; }
            public decimal? annual_turn_over { get; set; }
            public int? individual_member { get; set; }
            public int? institutional_member { get; set; }
            public int? level_of_edu { get; set; }
            public string level_of_edu_name { get; set; }
            public int? duration_of_course { get; set; }
            public string duration_of_course_name { get; set; }
            public string level_and_duration_of_course { get; set; }
            public decimal? course_in_audit { get; set; }
            public decimal? stu_in_audit { get; set; }
            public decimal? training_course_in_audit { get; set; }
            public decimal? participants_in_audit { get; set; }
            public string course_international_participant { get; set; }
            public int? no_of_training_course { get; set; }
            public int? attended_training { get; set; }
            public string society_recruit { get; set; }
            public int? no_regular_faculty { get; set; }
            public int? no_other_faculty { get; set; }
            public string facilities { get; set; }
            public int? st_code { get; set; }
            public int? dist_code { get; set; }
            public string ncd_id { get; set; }
            public string total_deposit { get; set; }
            public string pack_total_outstanding_loan { get; set; }
            public string paid_up_share_date { get; set; }

        }


        public class NCDJuteViewModel
        {
            [JsonProperty("Jute & Coir Cooperative Details")]
            public NCDJuteResponseModel JuteDetail { get; set; }
        }

        public class NCDJuteResponseModel
        {
            public int? has_building { get; set; }
            public int? building_type { get; set; }
            public decimal? authorised_share { get; set; }
            public decimal? annual_turn_over { get; set; }
            public string type_raw { get; set; }
            public string type_produce { get; set; }
            public string common_work_place { get; set; }
            public decimal? workplace_operate { get; set; }
            public string is_work_by_member { get; set; }
            public string is_training_provide { get; set; }
            public string is_raw_provide { get; set; }
            public string is_raw_easy_avail { get; set; }
            public string is_waste_generate { get; set; }
            public string is_waste_facility { get; set; }
            public string is_operate_retail { get; set; }
            public int? no_of_retail { get; set; }
            public string is_product_sale_out { get; set; }
            public string facilities { get; set; }
            public string ncd_id { get; set; }
            public int? st_code { get; set; }
            public int? dist_code { get; set; }
            public int? course_international_participant { get; set; }
            public string total_deposit { get; set; }
            public string pack_total_outstanding_loan { get; set; }
            public string paid_up_share_date { get; set; }
            public string Paid_up_share { get; set; }
        }


        public class NCDSericultureViewModel
        {
            [JsonProperty("Sericulture Cooperative Details")]
            public NCDSericultureResponseModel SeriDetail { get; set; }
        }

        public class NCDSericultureResponseModel
        {
            public string type_society { get; set; }
            public int? has_building { get; set; }
            public int? building_type { get; set; }
            public decimal? authorised_share { get; set; }
            public decimal? annual_turn_over { get; set; }
            public string common_work_place { get; set; }
            public int? no_rear_house { get; set; }
            public string is_work_by_member { get; set; }
            public string is_training_provide { get; set; }
            public string is_rear_appliance { get; set; }
            public string is_mulberry_easy_available { get; set; }
            public string is_cleaning_facility_cocoon { get; set; }
            public string is_spinning_weav { get; set; }
            public string is_waste_facility { get; set; }
            public string is_operate_retail { get; set; }
            public int? no_of_retail { get; set; }
            public string facilities { get; set; }
            public string ncd_id { get; set; }
            public int? st_code { get; set; }
            public int? dist_code { get; set; }
            public string total_deposit { get; set; }
            public string pack_total_outstanding_loan { get; set; }
            public string paid_up_share_date { get; set; }
            public string Paid_up_share { get; set; }

        }

        public class NCDSocialViewModel
        {
            [JsonProperty("Social Cooperative Details")]
            public NCDSocialResponseModel SocialDetail { get; set; }
        }

        public class NCDSocialResponseModel
        {
            public string type_society { get; set; }
            public int? has_building { get; set; }
            public int? building_type { get; set; }
            public decimal? authorised_share { get; set; }
            public decimal? paid_up_members { get; set; }
            public decimal? paid_up_government_bodies { get; set; }
            public decimal? paid_up_total { get; set; }
            public decimal? annual_turn_over { get; set; }
            public string type_social_culture_activity { get; set; }
            public string has_common { get; set; }
            public string is_operate_by_member { get; set; }
            public string guidance_by_member { get; set; }
            public string is_operate_vehicle { get; set; }
            public int? no_of_vehicle { get; set; }
            public string facilities { get; set; }
            public string ncd_id { get; set; }
            public int? st_code { get; set; }
            public int? dist_code { get; set; }
            public string total_deposit { get; set; }
            public string pack_total_outstanding_loan { get; set; }
            public string paid_up_share_date { get; set; }
        }


        public class NCDTourismViewModel
        {
            [JsonProperty("Tourism Cooperative Details")]
            public NCDTourismResponseModel TourDetail { get; set; }
        }

        public class NCDTourismResponseModel
        {
            public int? type_society { get; set; }
            public int? has_building { get; set; }
            public int? building_type { get; set; }
            public decimal? authorised_share { get; set; }
            public decimal? paid_up_members { get; set; }
            public decimal? paid_up_government_bodies { get; set; }
            public decimal? paid_up_total { get; set; }
            public int? individual_member { get; set; }
            public int? institutional_member { get; set; }
            public decimal? annual_turn_over { get; set; }
            public string pool_resource { get; set; }
            public string any_resource_taken { get; set; }
            public string is_right_vested { get; set; }
            public string facilities { get; set; }
            public string ncd_id { get; set; }
            public int? st_code { get; set; }
            public int? dist_code { get; set; }
            public string total_deposit { get; set; }
            public string pack_total_outstanding_loan { get; set; }
            public string paid_up_share_date { get; set; }
        }

        public class NCDTribalViewModel
        {
            [JsonProperty("Tribal-SC / ST Cooperative Details")]
            public NCDTribalResponseModel TribalDetail { get; set; }
        }
        public class NCDTribalResponseModel
        {
            public int? type_society { get; set; }
            public int? has_building { get; set; }
            public int? building_type { get; set; }
            public decimal? authorised_share { get; set; }
            public decimal? paid_up_members { get; set; }
            public decimal? paid_up_government_bodies { get; set; }
            public decimal? paid_up_total { get; set; }
            public decimal? annual_turn_over { get; set; }
            public string state_district_federation { get; set; }
            public string society_provide_raw_material { get; set; }
            public string facilities { get; set; }
            public string ncd_id { get; set; }
            public int? st_code { get; set; }
            public int? dist_code { get; set; }
            public string total_deposit { get; set; }
            public string pack_total_outstanding_loan { get; set; }
            public string paid_up_share_date { get; set; }
        }

        public class NCDKhadiGramViewModel
        {
            [JsonProperty("Khadi Gram Cooperative Details")]
            public NCDKhadiGramResponseModel KhadiDetail { get; set; }
        }

        public class NCDKhadiGramResponseModel
        {
            public int? has_building { get; set; }
            public int? building_type { get; set; }
            public decimal? authorised_share { get; set; }
            public decimal? paid_up_members { get; set; }
            public decimal? paid_up_government_bodies { get; set; }
            public decimal? paid_up_total { get; set; }
            public decimal? annual_turn_over { get; set; }
            public int? individual_member { get; set; }
            public int? institutional_member { get; set; }
            public string power_loom_type { get; set; }
            public string hand_loom_type { get; set; }
            public string hand_loom_other_type { get; set; }
            public int? no_of_loom { get; set; }
            public string raw_product_taken { get; set; }
            public string raw_material_available { get; set; }
            public string waste_generate { get; set; }
            public string waste_available { get; set; }
            public string operate_retail { get; set; }
            public int? no_of_retail { get; set; }
            public string product_sale_out { get; set; }
            public string operated_member_themself { get; set; }
            public string is_user_work_divide { get; set; }
            public string type_of_activities_khadi_gram { get; set; }
            public string do_you_want_to_enter_type_products_produced { get; set; }
            public string ncd_id { get; set; }
            public int? st_code { get; set; }
            public int? dist_code { get; set; }
            public string fswm_comp { get; set; }
            public string total_deposit { get; set; }
            public string pack_total_outstanding_loan { get; set; }
            public string paid_up_share_date { get; set; }
        }


        public class NCDDairyViewModel
        {
            [JsonProperty("Dairy Details")]
            public NCDDairyResponseModel DairyDetail { get; set; }
        }

        public class NCDDairyResponseModel
        {
            public string ncd_id { get; set; }
            public Int32? milk_collection { get; set; } = 0;
            public int? credit_facility { get; set; } = 0;
            public decimal? credit_provided { get; set; }
            public int? milk_collection_unit { get; set; }
            public string milk_collection_capicity { get; set; }
            public int? transport_milk { get; set; }
            public int? bulk_milk_unit { get; set; }
            public int? milk_testing { get; set; }
            public int? processing { get; set; }
            public int? is_bank_mitra { get; set; }
            public string bank_mitra_details { get; set; }
            public int? is_micro_atm { get; set; }
            public string micro_atm_details { get; set; }
            public string other_facility { get; set; }
            public string authorised_share { get; set; }
            public string paid_up_share { get; set; }
            public string total_deposit { get; set; }
            public string pack_total_outstanding_loan { get; set; }
            public string paid_up_share_date { get; set; }


        }


        public class NCDMultipurposeViewModel
        {
            [JsonProperty("Multipurpose Details")]
            public NCDMultipurposeResponseModel MultiDetail { get; set; }
        }

        public class NCDMultipurposeResponseModel
        {
            public string ncd_id { get; set; }
            public string sec_activity { get; set; }
            public int? has_building { get; set; }
            public int? building_type { get; set; }
            public decimal? authorised_share { get; set; }
            public decimal? paid_up_members { get; set; }
            public decimal? paid_up_government_bodies { get; set; }
            public decimal? paid_up_total { get; set; }
            public decimal? annual_turn_over { get; set; }
            public int? has_storage { get; set; }
            public decimal? storage_capacity { get; set; }
            public int? provide_raw { get; set; }
            public int? guidance_by_member { get; set; }
            public int? is_operate_retail { get; set; }
            public Int32? no_of_retail { get; set; }
            public int? is_product_sale_out { get; set; }
            public string facilities { get; set; }
            public string total_deposit { get; set; }
            public string pack_total_outstanding_loan { get; set; }
            public string paid_up_share_date { get; set; }
        }


        #region  NCD Common #Pradeep

        public class NCDCommonResponse
        {
            public string status { get; set; }
            public string message { get; set; }
        }

        public class NCDCooperativeRegistrationAPIResponse
        {
            public string status { get; set; }
            public string state_name { get; set; }
            public int? state_code { get; set; }
            public List<NCDCooperativeRegistrationResponseModel> result { get; set; }
            public string message { get; set; }
            [JsonProperty("Error Message")]
            public string ErrorMessage { get; set; }
            public int? total_societies { get; set; }
            public int? functional_societies { get; set; }
            public string non_functional_societies { get; set; }
            public string under_liquidation_societies { get; set; }
            public string total_members { get; set; }
        }

        public class NCDCooperativeRegistrationResponseModel
        {
            public string cooperative_id { get; set; }
            public int? registration_authoritie_id { get; set; }
            public string cooperative_society_name { get; set; }
            public string local_langauge_society_name { get; set; }
            public int? reference_year { get; set; }
            public string date_registration { get; set; }
            public string registration_number { get; set; }
            public int? cooperative_society_type_id { get; set; }
            public int? area_of_operation_id { get; set; }
            public int? water_body_type_id { get; set; } = 0;
            public int? is_approved { get; set; } = 0;
            public int? sector_of_operation_type { get; set; } = 0;
            public int? sector_of_operation { get; set; } = 0;
            public int? functional_status { get; set; } = 0;
            public string pacs_id { get; set; }
            public string pan_no { get; set; }
            public string gst_no { get; set; }
            public int? how_many_branches { get; set; }
            public string full_time_secretary { get; set; }
            public string mobile_number_of_secretary { get; set; }
            public string alternate_contact_no_for_pacs { get; set; }
            public int? location_of_head_quarter { get; set; }
            public int? state_code { get; set; } = 8;
            public int? district_code { get; set; }
            public int? block_code { get; set; }
            public int? gram_panchayat_code { get; set; }
            public int? village_code { get; set; }
            public int? urban_local_body_type_code { get; set; }
            public int? urban_local_body_code { get; set; }
            public int? locality_ward_code { get; set; }
            public int? subdistrict_code { get; set; }
            public string pincode { get; set; }
            public string full_address { get; set; }
            public string contact_person { get; set; }
            public int? designation { get; set; } = 0;
            public string mobile { get; set; }
            public string landline { get; set; }
            public string email { get; set; }
            public int? operation_area_location { get; set; } = 0;
            public int? is_coastal { get; set; } = 0;
            public int? is_affiliated_union_federation { get; set; } = 0;
            public int? members_of_society { get; set; }
            public int? financial_audit { get; set; } = 0;

            [JsonConverter(typeof(NullableIntConverter))]
            public int? audit_complete_year { get; set; }
            public int? category_audit { get; set; } = 0;
            public int? is_profit_making { get; set; } = 0;
            public decimal? annual_turnover { get; set; }
            public decimal? annual_loss { get; set; }
            public int? is_dividend_paid { get; set; } = 0;
            public string dividend_rate { get; set; }
            public string bank_type { get; set; }
            public string cooperative_society_bank_id { get; set; }
            public string other_bank { get; set; }
            public int? is_msme { get; set; } = 0;
            public string udyam_no { get; set; }
            public string updated { get; set; }
            public string annual_profit { get; set; }
            public string affiliated_union_federation_level { get; set; }
            public string affiliated_union_federation_name { get; set; }
            public string address_line { get; set; }
            public int? deregistered { get; set; } = 0;
            public string date_deregistered { get; set; }

        }


        public class NCDPacsLampsFssResponseModel
        {
            public int? has_building { get; set; } = 0;

            [JsonConverter(typeof(NullableIntConverter))]
            public int? building_type { get; set; }
            public string ncd_id { get; set; }
            public int? st_code { get; set; }
            public int? dist_code { get; set; }
            public int? fertilizer_distribution { get; set; } = 0;
            public decimal? fertilizer_distribution_qty { get; set; } = 0;
            public string fertilizer_distribution_details { get; set; }
            public int? pesticide_distribution { get; set; } = 0;
            public int? pesticide_distribution_qty { get; set; } = 0;
            public int? seed_distribution { get; set; } = 0;
            public decimal? seed_distribution_qty { get; set; } = 0;
            public Int32? fair_price { get; set; } = 0;
            public Int32? fair_price_qty { get; set; } = 0;
            public string fair_price_details { get; set; }
            public int? is_foodgrains { get; set; } = 0;
            public Int32? foodgrains_qty { get; set; } = 0;
            public int? agricultural_implements { get; set; } = 0;
            public string agricultural_implements_text { get; set; }
            public int? dry_storage { get; set; } = 0;
            public decimal? dry_storage_capicity { get; set; }
            public int? cold_storage { get; set; } = 0;
            public decimal? cold_storage_capicity { get; set; } = 0;
            public int? milk_unit { get; set; } = 0;
            public string milk_capicity_unit { get; set; }
            public int? food_processing { get; set; } = 0;
            public string food_processing_type { get; set; }
            public string other_facility { get; set; }
            public int? is_socitey_has_land { get; set; } = 0;
            public decimal? pack_involved_fish_catch { get; set; } = 0;
            public decimal? pack_annual_fish_catch { get; set; } = 0;
            public decimal? pack_total_outstanding_loan { get; set; } = 0;
            public decimal? pack_revenue_non_credit { get; set; } = 0;
            public int? is_lgs_program { get; set; } = 0;
            public decimal? lgs_capacity { get; set; } = 0;
            public int? is_csc { get; set; } = 0;
            public string csc_id { get; set; }
            public Int32? csc_revenue { get; set; } = 0;
            public string csc_details { get; set; }
            public int? is_fpo { get; set; } = 0;
            public string fpo_details { get; set; }
            public int? is_lpg_distributership { get; set; } = 0;
            public string lpg_distributership_details { get; set; }
            public int? is_bcp_pump { get; set; } = 0;
            public string bcp_pump_details { get; set; }
            public int? is_dpp_diesel { get; set; } = 0;
            public string dpp_diesel_details { get; set; }
            public int? is_jak { get; set; } = 0;
            public Int32? jak_qty { get; set; } = 0;
            public int? is_pmksk { get; set; } = 0;
            public string pmksk_details { get; set; }
            public int? is_paani_samity { get; set; } = 0;
            public string paani_samity_details { get; set; }
            public int? is_pm_kusum_scheme { get; set; } = 0;
            public string pm_kusum_scheme_details { get; set; }
            public int? is_consumer_business { get; set; } = 0;
            public string consumer_business_details { get; set; }
            public int? is_tourism { get; set; } = 0;
            public string tourism_details { get; set; }
            public int? is_cash_kendra_facilities { get; set; } = 0;
            public string cash_kendra_facilities_details { get; set; }
            public string authorised_share { get; set; }
            public string paid_up_share { get; set; }
            public string total_deposit { get; set; }
            public string paid_up_share_date { get; set; }
            public string lgs_program_details { get; set; }
        }
        public class NCDPacsLampsFssViewModel
        {
            [JsonProperty("PACS Details")]
            public NCDPacsLampsFssResponseModel PacsDetails { get; set; }
        }


        public class NcdApiCommonResponse<T>
        {
            public string status { get; set; }
            public string state_name { get; set; }
            public int? state_code { get; set; }
            public string sector { get; set; }
            public int? sector_code { get; set; }
            public int? records { get; set; }
            public List<T> result { get; set; }
            public string message { get; set; }
            [JsonProperty("Error Message")]
            public string ErrorMessage { get; set; }
        }

        public class NCDMemberDetailsModel
        {
            public string ncd_id { get; set; }
            public Int32? m_general { get; set; }
            public Int32? m_obc { get; set; }
            public Int32? m_sc { get; set; }
            public Int32? m_st { get; set; }
            public Int32? f_general { get; set; }
            public Int32? f_obc { get; set; }
            public Int32? f_sc { get; set; }
            public Int32? f_st { get; set; }
            public Int32? t_general { get; set; }
            public Int32? t_obc { get; set; }
            public Int32? t_sc { get; set; }
            public Int32? t_st { get; set; }
        }
        public class MemberDetailsResultItem
        {
            [JsonProperty("Members Details")]
            public NCDMemberDetailsModel MembersDetails { get; set; }
        }

        public class NCD_AreaofOperationRuralDetails
        {
            public int? AreaofOperationRuralDetailsId { get; set; }
            public string ncd_id { get; set; }
            public Int32? area_of_operation_id { get; set; } = 0;
            public Int32? state_code { get; set; } = 0;
            public Int32? district_code { get; set; } = 0;
            public Int32? block_code { get; set; } = 0;
            public Int32? panchayat_code { get; set; } = 0;
            public Int32? village_code { get; set; } = 0;
            public Int32? gp_village_all { get; set; } = 0;
            public bool? is_deleted { get; set; }
        }
        public class AreaofOperationRuralResultItem
        {
            [JsonProperty("Area of Operation Rural Details")]
            public NCD_AreaofOperationRuralDetails Data { get; set; }
        }

        public class NCD_AreaofOperationUrbanDetails
        {
            public int? AreaofOperationUrbanDetailsId { get; set; }
            public string ncd_id { get; set; }
            public Int32? area_of_operation_id { get; set; } = 0;
            public Int32? state_code { get; set; } = 0;
            public Int32? district_code { get; set; } = 0;
            public Int32? local_body_type_code { get; set; } = 0;
            public Int32? local_body_code { get; set; } = 0;
            public Int32? locality_ward_code { get; set; } = 0;
            public bool? is_deleted { get; set; }
        }
        public class AreaofOperationUrbanResultItem
        {
            [JsonProperty("Area of Operation Urban Details")]
            public NCD_AreaofOperationUrbanDetails Data { get; set; }
        }

        public class NCD_SocietyImplementingSchemesDetails
        {
            public Int32? ncd_society_id { get; set; }
            public string ncd_id { get; set; }
            public string gov_scheme_name { get; set; }
            public string gov_scheme_type { get; set; }
            public float? total_amount { get; set; } = 0;
            public Int32? st_code { get; set; } = 0;
            public Int32? dist_code { get; set; } = 0;
        }
        public class SocietyImplementingSchemesDetailsResultItem
        {
            [JsonProperty("Society Implementing Schemes Details")]
            public NCD_SocietyImplementingSchemesDetails Data { get; set; }
        }
        public class NCD_CooperativeRegistrationsLandsDetails
        {
            public string ncd_id { get; set; }
            public float? land_owned { get; set; } = 0;
            public float? land_leased { get; set; } = 0;
            public float? land_allotted { get; set; } = 0;
            public float? land_total { get; set; } = 0;
            public Int32? st_code { get; set; } = 0;
            public Int32? dist_code { get; set; } = 0;
        }
        public class CooperativeRegistrationsLandsDetailsResultItem
        {
            [JsonProperty("Cooperative Registrations Lands Details")]
            public NCD_CooperativeRegistrationsLandsDetails Data { get; set; }
        }

        public class NcdSocietyAuditYearsModel
        {
            public string ncd_id { get; set; }
            public Int32? audit_category { get; set; } = 0;
            public string audit_year { get; set; }
            public Int32? sector_type { get; set; } = 0;
            public Int32? sector { get; set; } = 0;
            public string annual_profit { get; set; }
            public string annual_loss { get; set; }
            public string share_capital { get; set; }
            public string reserve_fund { get; set; }
            public string revenue { get; set; }
            public string deposit { get; set; }
            public string loan_and_advance { get; set; }
            public string borrowings { get; set; }
            public string total_assets { get; set; }
            public string no_of_members { get; set; }
            public Int32? no_of_branches { get; set; } = 0;
            public string total_credit_provided { get; set; }
            public string paid_up_share { get; set; }
            public string annual_turnover { get; set; }
            public string annual_income { get; set; }
            public string annual_ucb_expenditr { get; set; }
            public string asset_ucb { get; set; }
            public string liability_ucb { get; set; }
            public string paid_up_members { get; set; }
            public string paid_up_government_bodies { get; set; }
            public string paid_up_total { get; set; }
            public string annual_expenses { get; set; }
            public int? state_code { get; set; } = 0;
            public Int32? district_code { get; set; } = 0;
            public string actual_annual_turnover { get; set; }
        }

        public class NCDSociatyAuditYearsResultItem
        {
            [JsonProperty("Society Audit Years Details")]
            public NcdSocietyAuditYearsModel Data { get; set; }
        }


        #endregion


        public class NCDMiscellaneousViewModel
        {
            [JsonProperty("Miscellaneous Non Credit Details")]
            public NCDMiscellaneousResponseModel MisceDetail { get; set; }
        }

        public class NCDMiscellaneousResponseModel
        {
            public int? has_building { get; set; }
            public int? building_type { get; set; }
            public decimal? authorised_share { get; set; }
            public decimal? paid_up_members { get; set; }
            public decimal? paid_up_government_bodies { get; set; }
            public decimal? paid_up_total { get; set; }
            public decimal? annual_turn_over { get; set; }
            public string ncd_id { get; set; }
            public int? st_code { get; set; }
            public int? dist_code { get; set; }
            public string total_deposit { get; set; }
            public string pack_total_outstanding_loan { get; set; }
            public string paid_up_share_date { get; set; }
        }
        public class NcdKhadiSectorOtherData
        {
            public Int32? ncd_coop_reg_khadi_gram_prod_id { get; set; }
            public string ncd_id { get; set; }
            public string item_name { get; set; }
            public Int32 yearly_production { get; set; }
            public Int32 unit { get; set; }
            public decimal value_in_rs { get; set; }
            public bool is_active { get; set; }
        }

        public class NCDAffilationUnionAPIResponse
        {
            public string status { get; set; }
            public string message { get; set; }
            public List<NCDAffilationUnionModel> result { get; set; }
            public string state_name { get; set; }
            public Int32? state_code { get; set; }
            public Int32? records { get; set; }
            public string ErrorMessage { get; set; }

        }
        public class NCDAffilationUnionModel
        {
            public Int32? affiliated_union_federation_level { get; set; }
            public Int32? affiliated_union_federation_name { get; set; }
            public string ncd_id { get; set; }
        }
        public class NCDUnionFederationAPIResponse
        {
            public string status { get; set; }
            public Int32? count { get; set; }
            public string message { get; set; }
            public List<NCDUnionFederationModel> data { get; set; }
            public string ErrorMessage { get; set; }
        }

        public class NCDUnionFederationModel
        {
            public Int32? id { get; set; }
            public string name { get; set; }
            public string entity_type { get; set; }
            public Int32? level_of_affiliated_union_id { get; set; }
            public Int32? dccb_flag { get; set; }
            public Int32? primary_activity_flag { get; set; }
        }


        public class NCDLevelAffiliatedUnionAPIResponse
        {
            public string status { get; set; }
            public Int32? count { get; set; }
            public string message { get; set; }
            public List<NCDLevelAffiliatedUnionModel> data { get; set; }
            public string ErrorMessage { get; set; }
        }

        public class NCDLevelAffiliatedUnionModel
        {
            public Int32? id { get; set; }
            public string name { get; set; }
            public string entity_type { get; set; }
            public Int32? level_of_affiliated_union_id { get; set; }
            public Int32? dccb_flag { get; set; }
            public Int32? primary_activity_flag { get; set; }
        }

        public class CommonFinancialResponse<T>
        {
            public int? statuscode { get; set; }
            public string status { get; set; }
            public string message { get; set; }
            public List<T> result { get; set; }
        }

        public class NCDFinancialResponseModel
        {
            public string cooperative_id { get; set; }
            public int? financial_audit { get; set; }
            public string audit_complete_year { get; set; }
            public int? category_audit { get; set; }
            public bool? is_profit_making { get; set; }
            public decimal? annual_turnover { get; set; }
            public decimal? annual_profit { get; set; }
            public decimal? annual_loss { get; set; }
        }
        public static DataTable BulkInsertToDataTable<T>(List<T> items)
        {
            var dataTable = new DataTable(typeof(T).Name);
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (var item in items)
            {
                var values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(item, null);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public DataSet GetNCDStateDetails()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("M_NcdStates", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetNCDState");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDServiceModel", FunctionName = "GetNCDStateDetails(string Action = 'Action')" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetNCDSectorDetails()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("M_NcdSectors", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetNCDSectorCode");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDServiceModel", FunctionName = "GetNCDStateDetails(string Action = 'Action')" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public class NullableIntConverter : JsonConverter<int?>
        {
            public override int? ReadJson(JsonReader reader, Type objectType, int? existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                if (reader.Value == null)
                    return null;

                var s = reader.Value.ToString();

                if (string.IsNullOrEmpty(s) || s.Equals("null", StringComparison.OrdinalIgnoreCase))
                    return null;

                int result;
                if (int.TryParse(s, out result))
                    return result;
                return null;
            }

            public override void WriteJson(JsonWriter writer, int? value, JsonSerializer serializer)
            {
                if (value.HasValue)
                    writer.WriteValue(value.Value);
                else
                    writer.WriteNull();
            }
        }

        public class NCDBoardofDirectorModel
        {
            public string bod_Member_name { get; set; }
            public int? gender { get; set; }
            public long mobile_number { get; set; }
            public string email_id { get; set; }
            public int? id_proof_type { get; set; }
            public string id_proof_no { get; set; }
            public int? bod_designation { get; set; }
            public string from_date { get; set; }
            public string to_date { get; set; }
            public int? is_any_other_cooperative { get; set; }
            public string other_cooperative_society_name { get; set; }
            public int? sector_code { get; set; }
            public int? st_code { get; set; }
            public int? dist_code { get; set; }
            public string ncd_id { get; set; }

        }


        public class SocietyRequestDataModel
        {
            public string ncd_id { get; set; }
            public string cooperative_society_name { get; set; }
            public string registration_number { get; set; }
            public string date_registration { get; set; }
            public int sector_of_operation { get; set; } = 0;
        }

        public class SocietyDetailResponse
        {
            public int statuscode { get; set; }
            public string status { get; set; }
            public string message { get; set; }
            public string deleted { get; set; }
            public string name { get; set; }
        }
    }
}
