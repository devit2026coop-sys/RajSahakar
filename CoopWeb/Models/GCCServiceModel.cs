using com.sun.security.ntlm;
using Cooperative.Controllers;
using Cooperative.DBConnector;
using Cooperative.JanadhaarClasses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RAJSSO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Util;
using static Cooperative.Models.MMPBYFarmerDetailsModel;


namespace Cooperative.Models
{
    public class GCCServiceModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public GCCServiceModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public GCCServiceModel()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();

        }
        public async Task<CBSGCCResponseModel> CheckGcctoCBSByJanMemberId(int dccbId = 0, string janaadharMemberId = "")
        {
            string dccbName = "";
            string apiUrl = "";
            var ds = GetDccbcode(dccbId);
            if (ds != null && ds.Tables.Count>0 && ds.Tables[0].Rows.Count>0)
            {
                dccbName = ds.Tables[0].Rows[0]["DCCB_Code"]?.ToString();
                apiUrl = ds.Tables[0].Rows[0]["ApiUrl"]?.ToString();
            }
            var responseData = new CBSCommonAPIResponse<CBSGCCResponseModel>();
            try
            {
                //string url = $"https://164.100.222.250:4002/servlet/LoanServlet"; //test
                string url = $"{apiUrl}/servlet/LoanServlet";
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };

                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var requestBody = new
                    {
                        dccbName = dccbName,
                        janaadharMemberId = janaadharMemberId
                    };
                    var jsonData = JsonConvert.SerializeObject(requestBody);
                    var httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, httpContent);
                    var resData = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        resData = resData.ToString().Replace("\"{", "{").Replace("}\"", "}").Replace("\"null\"", "null");
                        var temp = JsonConvert.DeserializeObject<JObject>(resData);

                        var dataToken = temp["data"];
                        CBSGCCResponseModel model = dataToken.Type == JTokenType.String ? JsonConvert.DeserializeObject<CBSGCCResponseModel>(dataToken.ToString()) : dataToken.ToObject<CBSGCCResponseModel>();
                        return model;
                    }
                    else
                    {
                        return null;
                    }
                }


            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.GCCServiceModel",
                    FunctionName = $"CheckGcctoCBSByJanMemberId(string dccbName = {dccbName}, string janaadharMemberId = {janaadharMemberId})"
                }, ex);
            }
            return null;
        }

        public DataSet GetDccbcode(int dccbId = 0)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_GetDccbcodeData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "GetDccbcode");
                objDBAccess.AddParameterToCommand("@CCBId", DBFactoryParameterType.Int, dccbId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.GCCServiceModel",
                    FunctionName = "GetDccbcode(int dccbId = '" + dccbId + "')"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public DataTable GetGccApplicationFormNotGenerated()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_GetGccApplicationFormNotGenerated", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "GetData");

                DataSet ds = objDBAccess.GetDataSet();
                if (ds.Tables.Count > 0)

                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.GCCServiceModel",
                    FunctionName = "GetGccApplicationFormNotGenerated()"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public DataTable UpdateGccApplicationFormNotGenerated(int ApplicationFlowId, int DocId)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_GetGccApplicationFormNotGenerated", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "UpdateFile");
                objDBAccess.AddParameterToCommand("@ApplicationFlowId ", DBFactoryParameterType.Int, ApplicationFlowId);
                objDBAccess.AddParameterToCommand("@DocId ", DBFactoryParameterType.Int, DocId);
                DataSet ds = objDBAccess.GetDataSet();
                if (ds.Tables.Count > 0)

                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.GCCServiceModel",
                    FunctionName = $"UpdateGccApplicationFormNotGenerated({ApplicationFlowId} , {DocId})"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetccbDetails()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_GetDccbcodeData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "GetccbDetail");
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.GCCServiceModel",
                    FunctionName = "GetccbDetails()"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public DataSet GetRSCBODData(string type = "")
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_GetDccbcodeData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, type);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.GCCServiceModel",
                    FunctionName = "GetRSCBODData(string type = '" + type + "')"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public class CBSCommonAPIResponse<T>
        {
            public T data { get; set; }
        }
        public class CBSGCCResponseModel
        {
            public string Status { get; set; }
            public string DCCB { get; set; }
            public string Branch_Name { get; set; }
            public string SB_Account_Number { get; set; }
            public string PACS_ID { get; set; }
            public string PACS_Name { get; set; }
            public string ODFlag { get; set; }
            public string ODAmount { get; set; }
            public string Acc_Bank_Name { get; set; }
            public string Ifsc_Code { get; set; }
            public string Acc_Branch_Name { get; set; }
            public string Acc_Branch_Code { get; set; }
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
