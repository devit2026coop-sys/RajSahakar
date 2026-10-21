using Cooperative.Controllers;
using Cooperative.DBConnector;
using Cooperative.JanadhaarClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;


namespace Cooperative.Models
{
    public class MMPBYFarmerDetailsModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public MMPBYFarmerDetailsModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public MMPBYFarmerDetailsModel()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();

        }

        public async Task<CommonAPIResponse<FarmerDetailsModel>> GetMMPBYFarmerDetailsByDistrict(int district_code = 0)
        {
            var responseData = new CommonAPIResponse<FarmerDetailsModel>();
            try
            {
                string baseUrl = ((System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("localhost") || (System.Web.HttpContext.Current?.Request?.Url?.Host ?? "").ToLower().Contains("10.70.234.208")) ? GlobalConstant.MMPBY_UAT_API_URL : GlobalConstant.MMPBY_PROD_API_URL;
                string url = $"{baseUrl}?client_id={JanAadhaarConfig.ClientId}";
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var formData = new MultipartFormDataContent();
                    formData.Add(new StringContent(district_code.ToString()), "DISTRICT_CODE");
                    var response = await client.PostAsync(url, formData);
                    var content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        responseData = JsonConvert.DeserializeObject<CommonAPIResponse<FarmerDetailsModel>>(content);
                        responseData.is_success = responseData.is_success;
                        responseData.message = responseData.message;
                    }
                    else
                    {
                        responseData.is_success = responseData.is_success;
                        responseData.message = responseData.message;
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.MMPBYFarmerDetailsModel",
                    FunctionName = $"GetMMPBYFarmerDetailsByDistrict({district_code})"
                }, ex);
                responseData.is_success = responseData.is_success;
                responseData.message = ex.Message;
            }
            return responseData;
        }

        public int SaveFarmerDetailDataWorking(List<FarmerDetailsModel> _farmerModelList)
        {
            int result = 0;
            UserEntity _loggedInUSerDetails = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            var farmerList = _farmerModelList.Select((x, i) => new
            {
                //farmer_id = i + 1,
                jan_aadhaar_no = x.jan_aadhaar_no,
                jan_aadhaar_member_id = x.jan_aadhaar_member_id,
                full_name = x.full_name,
                mobile_number = x.mobile_number,
                address = x.address,
                district_name = x.district_name,
                isRural = x.isRural == "Y" ? 1 : 0,
                city_block_name = x.city_block_name,
                IsLakpatiDidi = x.IsLakpatiDidi,
                IsGopalCard = x.IsGopalCard,
                policy_generated_date = x.policy_generated_date,
                policy_no = x.policy_no,
                start_date = x.start_date,
                end_date = x.end_date
            }).ToList();

            //List<insured_cattl_detailsModel> insuredDetailData = _farmerModelList.SelectMany(a => a.insured_cattl_details).ToList();
            //var insuredLstData = insuredDetailData.Select(x => new
            //{
            //    id = x.id,
            //    cattle_type_id = x.cattle_type_id,
            //    cattle_tag_no = x.cattle_tag_no,
            //    cattle_death_details_id = x.cattle_death_details?.id ?? 0,
            //    death_certificate_type = x.cattle_death_details?.death_certificate_type ?? 0,
            //}).ToList();
            var cattleList = _farmerModelList.SelectMany((farmer, index) => farmer.insured_cattl_details != null ? farmer.insured_cattl_details.Select((cattle, catIndex) => new
            {
                cattle_type_id = cattle.cattle_type_id ?? 0,
                cattle_tag_no = cattle.cattle_tag_no ?? "",
                cattle_death_details_id = cattle.cattle_death_details?.id ?? 0,
                death_certificate_type = cattle.cattle_death_details?.death_certificate_type ?? 0,
                farmer_id = index + 1
            }) : Enumerable.Empty<dynamic>()).ToList();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_MMPBYFarmerDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                cmd.Parameters.AddWithValue("@UDT_MMPBYFarmerDetails", BulkInsertToDataTable(farmerList));
                result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MMPBYFarmerDetailsModel", FunctionName = "saveMMPBYFarmerDetailData(List<FarmerDetailsModel> _farmerModelList)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }
        public int SaveFarmerDetailData(List<FarmerDetailsModel> _farmerModelList)
        {
            int result = 0;
            objDBAccess.OpenConnection();
            try
            {
                foreach (var farmer in _farmerModelList)
                {
                    SqlCommand cmd = objDBAccess.CreateCommand("p_MMPBYFarmerDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                    cmd.Parameters.AddWithValue("@jan_aadhaar_no", farmer.jan_aadhaar_no);
                    cmd.Parameters.AddWithValue("@jan_aadhaar_member_id", farmer.jan_aadhaar_member_id);
                    cmd.Parameters.AddWithValue("@full_name", farmer.full_name);
                    cmd.Parameters.AddWithValue("@mobile_number", farmer.mobile_number);
                    cmd.Parameters.AddWithValue("@address", farmer.address);
                    cmd.Parameters.AddWithValue("@district_name", farmer.district_name);
                    cmd.Parameters.AddWithValue("@isRural", farmer.isRural == "Y" ? 1 : 0);
                    cmd.Parameters.AddWithValue("@city_block_name", farmer.city_block_name);
                    cmd.Parameters.AddWithValue("@IsLakpatiDidi", farmer.IsLakpatiDidi);
                    cmd.Parameters.AddWithValue("@IsGopalCard", farmer.IsGopalCard);
                    cmd.Parameters.AddWithValue("@policy_generated_date", farmer.policy_generated_date);
                    cmd.Parameters.AddWithValue("@policy_no", farmer.policy_no);
                    cmd.Parameters.AddWithValue("@start_date", farmer.start_date);
                    cmd.Parameters.AddWithValue("@end_date", farmer.end_date);
                    long farmerId = Convert.ToInt64(cmd.ExecuteScalar());
                    if (farmer.insured_cattl_details != null && farmer.insured_cattl_details.Count > 0)
                    {
                        foreach (var cattle in farmer.insured_cattl_details)
                        {
                            SqlCommand command = objDBAccess.CreateCommand("p_MMPBYInsuredCattlDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                            command.Parameters.AddWithValue("@id", cattle.id ?? 0);
                            command.Parameters.AddWithValue("@cattle_type_id", cattle.cattle_type_id ?? 0);
                            command.Parameters.AddWithValue("@cattle_tag_no", cattle.cattle_tag_no ?? "");
                            command.Parameters.AddWithValue("@cattle_death_details_id", cattle.cattle_death_details?.id ?? 0);
                            command.Parameters.AddWithValue("@death_certificate_type", cattle.cattle_death_details?.death_certificate_type ?? 0);
                            command.Parameters.AddWithValue("@mmpby_farmer_id", farmerId);
                            long cattleId = Convert.ToInt64(command.ExecuteScalar());
                            result++;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.MMPBYFarmerDetailsModel",
                    FunctionName = "SaveFarmerDetailData(List<FarmerDetailsModel> _farmerModelList = '" + _farmerModelList.Count + "')"
                }, ex);

                throw;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return result;
        }

        public DataSet GetMMPBYFarmerDetails(string type = "")
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_GetMMPBYFarmerDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, type);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.MMPBYFarmerDetailsModel",
                    FunctionName = "GetMMPBYFarmerDetails(string type = '" + type + "')"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public class CommonAPIResponse<T>
        {
            public bool? is_success { get; set; }
            public string message { get; set; }
            public List<T> data { get; set; }
        }
        public class FarmerDetailsModel
        {
            public long? jan_aadhaar_no { get; set; }
            public string jan_aadhaar_member_id { get; set; }
            public string full_name { get; set; }
            public string mobile_number { get; set; }
            public string address { get; set; }
            public string district_name { get; set; }
            public string isRural { get; set; }
            public string city_block_name { get; set; }
            public bool? IsLakpatiDidi { get; set; }
            public bool? IsGopalCard { get; set; }
            public string policy_generated_date { get; set; }
            public string policy_no { get; set; }
            public string start_date { get; set; }
            public string end_date { get; set; }
            public List<insured_cattl_detailsModel> insured_cattl_details { get; set; }

        }

        public class insured_cattl_detailsModel
        {
            public int? id { get; set; }
            public int? cattle_type_id { get; set; }
            public string cattle_tag_no { get; set; }
            public cattle_death_detailsModel cattle_death_details { get; set; }
        }

        public class cattle_death_detailsModel
        {
            public int? id { get; set; }
            public int? cattle_id { get; set; }
            public string cattle_tag_no { get; set; }
            public int? death_certificate_type { get; set; }
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
