using Cooperative.Controllers;
using Cooperative.DBConnector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Cooperative.Models
{
    public class LGDServiceModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;
        private string lgdapibaseUrl = "https://delhigw.napix.gov.in/nic/lgd/";
        private string clientId = "637145acf6ba7f195c84dd940f506394";
        private string clientSecret = "65dbc6e07e5d89b672988ebe6f1cdee3";

        public LGDServiceModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public LGDServiceModel()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();

        }

        #region LGD api integration
        /// <summary>
        /// GetActiveDistrictByState
        /// </summary>
        /// <param name="statecode"></param>
        /// <returns></returns>
        public async Task<LGDCommonAPIResponse<LGDDistrictModel>> GetActiveDistrictByState(int statecode = 0)
        {
            var responseData = new LGDCommonAPIResponse<LGDDistrictModel>();
            try
            {
                string url = $"{lgdapibaseUrl}state/{statecode}/districts/v1/status/active";
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("X-IBM-Client-Id", clientId);
                    client.DefaultRequestHeaders.Add("X-IBM-Client-Secret", clientSecret);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.PostAsync(url, null);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        responseData.data = JsonConvert.DeserializeObject<List<LGDDistrictModel>>(data);
                        responseData.statuscode = (int)response.StatusCode;
                        responseData.status = "Success";
                        responseData.message = "Districts fetched successfully";
                    }
                    else
                    {
                        responseData.status = "Failed";
                        responseData.statuscode = (int)response.StatusCode;
                        responseData.message = "Record not found";
                    }
                }

                return responseData;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.LGDServiceModel",
                    FunctionName = $"GetLgdDistrictBystateActive({statecode})"
                }, ex);

                responseData.status = "Error";
                responseData.message = ex.Message;
                return responseData;
            }
        }

        public int savedistrictData(List<LGDDistrictModel> _districtModel)
        {
            int result = 0;
            UserEntity _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                foreach (LGDDistrictModel model in _districtModel)
                {
                    SqlCommand cmd = objDBAccess.CreateCommand("M_NcdDistricts", DBFactoryCommandType.PROCEDURE_COMMAND);

                    objDBAccess.AddParameterToCommand("@DistrictCode", DBFactoryParameterType.VARCHAR, model.DistrictCode);
                    objDBAccess.AddParameterToCommand("@DistrictName", DBFactoryParameterType.VARCHAR, model.DistrictNameEnglish);
                    DataTable dtResult = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dtResult);
                    result = dtResult.Rows.Count;
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.LGDServiceModel", FunctionName = "savedistrictData(List<NCDDistrictModel> _districtModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statecode"></param>
        /// <returns></returns>
        public async Task<LGDCommonAPIResponse<LGDBlockModel>> GetActiveBlockByState(int statecode = 0)
        {
            var responseData = new LGDCommonAPIResponse<LGDBlockModel>();
            try
            {
                string url = $"{lgdapibaseUrl}state/{statecode}/blocks/v1/status/active";
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("X-IBM-Client-Id", clientId);
                    client.DefaultRequestHeaders.Add("X-IBM-Client-Secret", clientSecret);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.PostAsync(url, null);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        responseData.data = JsonConvert.DeserializeObject<List<LGDBlockModel>>(data);
                        responseData.statuscode = (int)response.StatusCode;
                        responseData.status = "Success";
                        responseData.message = "Blocks data fetched successfully.";
                    }
                    else
                    {
                        responseData.status = "Failed";
                        responseData.statuscode = (int)response.StatusCode;
                        responseData.message = "No response from LGD service.";
                    }
                }

                return responseData;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.LGDServiceModel",
                    FunctionName = $"GetLgdDistrictBystateActive({statecode})"
                }, ex);

                responseData.status = "Error";
                responseData.statuscode = 500;
                responseData.message = "No response from LGD service.";
                return responseData;
            }
        }

        public int SaveBlockDetails(List<LGDBlockModel> _blockModel)
        {
            int result = 0;
            var blockListData = _blockModel.Select(x => new
            {
                block_code = x.blockCode,
                block_name = x.blockNameEnglish,
                block_name_hindi = x.blockNameLocal == "" ? null : x.blockNameLocal,
                block_version = x.majorVersion,
                block_name_local = x.blockNameLocal == "" ? null : x.blockNameLocal,
                state_code = 8,
                district_code = x.districts.FirstOrDefault()?.districtCode
            }).ToList();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdBlock", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@M_NcdBlock", BulkInsertToDataTable(blockListData));
                result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NCDBlocksApiResponse", FunctionName = "SaveNcdBlockDetails(List<LGDBlockModel> _blockModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }
        public async Task<LGDCommonAPIResponse<LGDWardsModel>> GetActiveWardByState(int statecode = 0, int pageNo = 0)
        {
            var responseData = new LGDCommonAPIResponse<LGDWardsModel>();
            try
            {
                string url = $"{lgdapibaseUrl}state/{statecode}/wards/v1/status/active/category/R/page/{pageNo}";
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("X-IBM-Client-Id", clientId);
                    client.DefaultRequestHeaders.Add("X-IBM-Client-Secret", clientSecret);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.PostAsync(url, null);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        responseData.data = JsonConvert.DeserializeObject<List<LGDWardsModel>>(data);
                        responseData.statuscode = (int)response.StatusCode;
                        responseData.status = "Success";
                        responseData.message = "Wards data fetched successfully.";
                    }
                    else
                    {
                        responseData.status = "Failed";
                        responseData.statuscode = (int)response.StatusCode;
                        responseData.message = "No response from LGD service.";
                    }
                }

                return responseData;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.LGDServiceModel",
                    FunctionName = $"GetLgdDistrictBystateActive({statecode})"
                }, ex);

                responseData.status = "Error";
                responseData.statuscode = 500;
                responseData.message = "No response from LGD service.";
                return responseData;
            }
        }

        public int SaveWardDetails(List<LGDWardsModel> _wardModel)
        {
            int result = 0;
            try
            {
                var lstWardsData = _wardModel.Select(x => new
                {
                    local_body_code = x.localBodyCode,
                    local_body_name = x.localBodyNameEnglish,
                    local_body_hindi = x.localBodyNameEnglish,
                    ward_code = x.wardCode,
                    ward_name = x.wardNameEnglish,
                    ward_name_hindi = x.wardNameLocal,
                    state_code = 8,
                    state_name = "RAJASTHAN"
                }).ToList();
                objDBAccess.OpenConnection();
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
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.LGDWardsModel", FunctionName = "SaveWardDetails(List<NCDWardViewModel> _wardModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public async Task<LGDCommonAPIResponse<LGDCitiesModel>> GetActiveCitiesByState(int statecode = 0)
        {
            var responseData = new LGDCommonAPIResponse<LGDCitiesModel>();
            try
            {
                string url = $"{lgdapibaseUrl}state/{statecode}/villageGPMapping/v1/status/active";
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("X-IBM-Client-Id", clientId);
                    client.DefaultRequestHeaders.Add("X-IBM-Client-Secret", clientSecret);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.PostAsync(url, null);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        responseData.data = JsonConvert.DeserializeObject<List<LGDCitiesModel>>(data);
                        responseData.statuscode = (int)response.StatusCode;
                        responseData.status = "Success";
                        responseData.message = "Cities data fetched successfully.";
                    }
                    else
                    {
                        responseData.status = "Failed";
                        responseData.statuscode = (int)response.StatusCode;
                        responseData.message = "No response from LGD service.";
                    }
                }

                return responseData;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.LGDServiceModel",
                    FunctionName = $"GetLgdDistrictBystateActive({statecode})"
                }, ex);

                responseData.status = "Error";
                responseData.statuscode = 500;
                responseData.message = "No response from LGD service.";
                return responseData;
            }
        }

        public int SaveCitiesDetails(List<LGDCitiesModel> _CitiesModel)
        {
            int result = 0;
            try
            {
                var lstCitiesData = _CitiesModel.Select(x => new
                {
                    localbody_type_code = x.localBodyTypeCode,
                    localbody_type_name = x.localBodyTypeName,
                    localbody_type_name_hindi = x.localBodyTypeName,
                    localbody_code = x.localBodyCode,
                    local_body_name = x.localBodyNameEnglish,
                    local_body_name_hindi = x.localBodyNameEnglish,
                    state_code = x.stateCode,
                    state_name = x.stateNameEnglish,
                    district_code = x.districtCode,
                    district_name = x.districtNameEnglish
                }).GroupBy(a => new
                {
                    a.localbody_type_code,
                    a.localbody_type_name,
                    a.localbody_type_name_hindi,
                    a.localbody_code,
                    a.local_body_name,
                    a.local_body_name_hindi,
                    a.state_code,
                    a.state_name,
                    a.district_code,
                    a.district_name
                }).Select(g => g.First()).ToList();

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_NcdCity", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@M_NcdCities", BulkInsertToDataTable(lstCitiesData));
                result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.LGDCitiesModel", FunctionName = "SaveCitiesDataByStatecode(LGDCitiesModel _CitiesModel)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public async Task<LGDCommonAPIResponse<LGDVillageModel>> GetActiveVillageByState(int statecode = 0)
        {
            var responseData = new LGDCommonAPIResponse<LGDVillageModel>();
            try
            {
                string url = $"{lgdapibaseUrl}state/{statecode}/villages/v2/status/active";
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("X-IBM-Client-Id", clientId);
                    client.DefaultRequestHeaders.Add("X-IBM-Client-Secret", clientSecret);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.PostAsync(url, null);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        responseData.data = JsonConvert.DeserializeObject<List<LGDVillageModel>>(data);
                        responseData.statuscode = (int)response.StatusCode;
                        responseData.status = "Success";
                        responseData.message = "Village data fetched successfully.";
                    }
                    else
                    {
                        responseData.status = "Failed";
                        responseData.statuscode = (int)response.StatusCode;
                        responseData.message = "No response from LGD service.";
                    }
                }

                return responseData;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.LGDServiceModel",
                    FunctionName = $"GetLgdDistrictBystateActive({statecode})"
                }, ex);

                responseData.status = "Error";
                responseData.statuscode = 500;
                responseData.message = "No response from LGD service.";
                return responseData;
            }
        }

        public async Task<LGDCommonAPIResponse<LGDLocalbodiesViewModel>> GetActiveLocalbodiesByState(int statecode = 0, int pageNo = 0)
        {
            var responseData = new LGDCommonAPIResponse<LGDLocalbodiesViewModel>();
            try
            {
                string url = $"{lgdapibaseUrl}state/{statecode}/localbodies/v2/status/active/category/U/page/{pageNo}";
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("X-IBM-Client-Id", clientId);
                    client.DefaultRequestHeaders.Add("X-IBM-Client-Secret", clientSecret);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.PostAsync(url, null);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        if (data.TrimStart().StartsWith("["))
                        {
                            responseData.data = JsonConvert.DeserializeObject<List<LGDLocalbodiesViewModel>>(data);
                            responseData.statuscode = (int)response.StatusCode;
                            responseData.status = "Success";
                            responseData.message = "Localbodies data fetched successfully.";
                        }
                        else
                        {
                            var errorResponse = JsonConvert.DeserializeObject<ApiErrorResponse>(data);
                            responseData.statuscode = errorResponse.statusCode ?? (int)response.StatusCode;
                            responseData.status = errorResponse.status;
                            responseData.message = errorResponse?.message;
                            responseData.errors = errorResponse?.errors;
                        }
                    }
                    else
                    {
                        responseData.status = "Failed";
                        responseData.statuscode = (int)response.StatusCode;
                        responseData.message = "No response from LGD service.";
                    }
                }

                return responseData;
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.LGDServiceModel",
                    FunctionName = $"GetLgdDistrictBystateActive({statecode})"
                }, ex);

                responseData.status = "Error";
                responseData.statuscode = 500;
                responseData.message = "No response from LGD service.";
                return responseData;
            }
        }

        public class LGDCommonAPIResponse<T>
        {
            public string status { get; set; }
            public int statuscode { get; set; }
            public string message { get; set; }
            public List<string> errors { get; set; }
            [JsonIgnore]
            public List<T> data { get; set; }
        }
        public class ApiErrorResponse
        {
            public int? statusCode { get; set; }
            public string status { get; set; }
            public string message { get; set; }
            public List<string> errors { get; set; }
        }

        public class LGDDistrictModel
        {
            public int Statecode { get; set; }
            public int? DistrictCode { get; set; }
            public string DistrictNameEnglish { get; set; }
            public string DistrictNameLocal { get; set; }
            public string DistrictNameshort { get; set; }
            public string Census2011Code { get; set; }
            public string EffectiveDate { get; set; }
            public string LastUpdated { get; set; }
            public int? MajorVersion { get; set; }
            public int? MinorVersion { get; set; }
            public int? TransactionId { get; set; }
            public int? OperationCode { get; set; }
            public string TransactionDescription { get; set; }
            public bool IsActive { get; set; }

        }

        public class LGDBlockModel
        {
            public int? blockCode { get; set; }
            public string blockNameEnglish { get; set; }
            public string blockNameLocal { get; set; }
            public string effectiveDate { get; set; }
            public string lastUpdated { get; set; }
            public int? majorVersion { get; set; }
            public int? minorVersion { get; set; }
            public int? transactionId { get; set; }
            public int? operationCode { get; set; }
            public string transactionDescription { get; set; }
            public bool isActive { get; set; }
            public List<DistrictModel> districts { get; set; }
            public List<VillageModel> villageCoverage { get; set; }
        }

        public class DistrictModel
        {
            public int? districtCode { get; set; }
        }

        public class VillageModel
        {
            public int? villageCode { get; set; }
        }

        public class LGDWardsModel
        {
            public int? wardCode { get; set; }
            public string wardNameEnglish { get; set; }
            public string wardNameLocal { get; set; }
            public int? localBodyCode { get; set; }
            public string localBodyNameEnglish { get; set; }
            public string effectiveDate { get; set; }
            public string lastUpdated { get; set; }
            public int? majorVersion { get; set; }
            public int? minorVersion { get; set; }
            public int? transactionId { get; set; }
            public int? operationCode { get; set; }
            public string transactionDescription { get; set; }
            public bool isactive { get; set; }
        }

        public class LGDCitiesModel
        {
            public Int32? stateCode { get; set; }
            public string stateNameEnglish { get; set; }
            public Int32? districtCode { get; set; }
            public string districtNameEnglish { get; set; }
            public Int32? subdistrictCode { get; set; }
            public string subdistrictNameEnglish { get; set; }
            public Int32? villageCode { get; set; }
            public string villageNameEnglish { get; set; }
            public Int32? localBodyCode { get; set; }
            public string localBodyNameEnglish { get; set; }
            public Int32? localBodyTypeCode { get; set; }
            public string localBodyTypeName { get; set; }
            public Int32? parentLocalBodyCode { get; set; }
            public string parentLocalBodyName { get; set; }
            public Int32? parentLocalBodyTypeCode { get; set; }
            public string parentLocalBodyTypeName { get; set; }
        }


        public class LGDVillageModel
        {
            public Int32? districtCode { get; set; }
            public string districtNameEnglish { get; set; }
            public Int32? subDistrictCode { get; set; }
            public string subDistrictNameEnglish { get; set; }
            public Int32? villageCode { get; set; }
            public string villageNameEnglish { get; set; }
            public string villageNameLocal { get; set; }
            public Int32? stateCode { get; set; }
            public string census2011Code { get; set; }
            public string effectiveDate { get; set; }
            public string lastUpdated { get; set; }
            public Int32? majorVersion { get; set; }
            public Int32? minorVersion { get; set; }
            public Int32? transactionId { get; set; }
            public Int32? operationCode { get; set; }
            public string transactionDescription { get; set; }
            public bool? isactive { get; set; }
            public string villageStatus { get; set; }
            public string is_pesa { get; set; }

        }


        public class LGDLocalbodiesViewModel
        {
            public Int32? localBodyCode { get; set; }
            public string localBodyNameEnglish { get; set; }
            public string localBodyNameLocal { get; set; }
            public Int32? localBodyTypeCode { get; set; }
            public string localBodyTypeName { get; set; }
            public Int32? parentLocalBodyCode { get; set; }
            public Int32? districtCode { get; set; }
            public string districtNameEnglish { get; set; }
            public Int32? stateCode { get; set; }
            public string census2011Code { get; set; }
            public string effectiveDate { get; set; }
            public string lastUpdated { get; set; }
            public Int32? majorVersion { get; set; }
            public Int32? minorVersion { get; set; }
            public Int32? transactionId { get; set; }
            public Int32? operationCode { get; set; }
            public string transactionDescription { get; set; }
            public bool? isActive { get; set; }
            public List<CoverageViewModel> coverage { get; set; }
        }

        public class CoverageViewModel
        {
            public Int32 entity_code { get; set; }
            public string entity_type { get; set; }
            public string coverage_type { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
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

    }
}
