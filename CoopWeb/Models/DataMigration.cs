using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace Cooperative.Models
{
    public class DataMigration
    {
        DBFactoryMSSQL objDBAccess = new DBFactoryMSSQL();
        private DataTable dt;
        private ILogger objILogger;

        public int ID { get; set; }
        public string ObjectId { get; set; }
        public string TableName { get; set; }
        public string DataCount { get; set; }
        public string TableCreateDate { get; set; }
        public string TableUpdateDate { get; set; }



        public DataTable GetTableList()
        {
            DataTable ds = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Usp_GetTableSchemaList", DBFactoryCommandType.PROCEDURE_COMMAND);
                ds = objDBAccess.GetDataTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GetTableSchemaList", FunctionName = "GetTableSchemaList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return ds;
        }
        public DataTable GetTableData(string TableName)
        {
            DataTable ds = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Usp_GetTableData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TableName", DBFactoryParameterType.VARCHAR, TableName);
                ds = objDBAccess.GetDataTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GetTableSchemaList", FunctionName = "GetTableSchemaList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return ds;
        }

        public DataTable PushData(string TableName)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("USP_PushDataSourcetoDestinationTable", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TableName", DBFactoryParameterType.VARCHAR, TableName);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.DataMigration", FunctionName = "PushData(string TableName)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public string PushDataToLocalDB(List<InsertData> TableData, string TableName)
        {
            string MSG = "!Faild";
            Int32 Result = 0;
            //DBFactoryMSSQL conn = new DBFactoryMSSQL();
            //SqlConnection sqlConnection = conn.GetConnection;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["wmsprod"].ToString()))
            {
                SqlConnection sqlConnection = con;
                SqlTransaction transaction;
                sqlConnection.Open();
                transaction = sqlConnection.BeginTransaction();
                try
                {
                    DataTable DT = new DataTable();
                    SqlCommand cmd = new SqlCommand();
                    SqlDataAdapter sdr = new SqlDataAdapter(cmd);
                    cmd.CommandText = "InsertDataMigrationData";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = sqlConnection;
                    cmd.Transaction = transaction;
                    cmd.Parameters.AddWithValue("@Data", TableData[0].InsertDataValue.Replace("NNULL", "NULL"));
                    cmd.Parameters.AddWithValue("@IsValidate", 0);
                    sdr.Fill(DT);

                    if (DT.Rows.Count >= 0)
                    {
                        int Count = 0;
                        TableData.ForEach(obj =>
                        {
                            cmd = new SqlCommand();
                            cmd.CommandText = "InsertDataMigrationData";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = sqlConnection;
                            cmd.Transaction = transaction;
                            cmd.Parameters.AddWithValue("@Data", obj.InsertDataValue.Replace("NNULL", "NULL"));
                            cmd.Parameters.AddWithValue("@IsValidate", 1);
                            cmd.Parameters.AddWithValue("@TableName", TableName);
                            cmd.Parameters.AddWithValue("@IsTruncate", Count);
                            cmd.Parameters.AddWithValue("@IsLastRow", TableData.Count - 1 == Count ? 1 : 0);
                            Result = Convert.ToInt32(cmd.ExecuteScalar());
                            Count++;
                        });
                    }

                    if (Result > 0)
                    {
                        transaction.Commit();
                        MSG = "SUCESS";
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    Result = -1;
                    transaction.Rollback();
                    DBLogger objDBLogger = new DBLogger();
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.DataMigration", FunctionName = "PushDataToLocalDB" }, ex);
                    MSG = ex.Message;
                }
                finally
                {
                    objDBAccess.CloseConnection();
                    //dt.Dispose();
                }
            }

            return MSG;
        }



        public bool TurncateTable(string TableName)
        {
            try
            {

                string apiUrl = "http://localhost:63789/api/TableSchema/TruncateTable?TableName=" + TableName + "";
                object input = new
                {
                    Name = TableName,
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(input);
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string json = client.UploadString(apiUrl + "/TruncateTable", inputJson);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

}