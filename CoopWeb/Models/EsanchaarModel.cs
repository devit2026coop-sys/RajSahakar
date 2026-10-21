using Cooperative.DBConnector;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Cooperative.Models
{
    public class EsanchaarModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public EsanchaarModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public DataTable GetSMSTempleteList(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("SmsTemplateList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Auditor", FunctionName = "GetAllAuditor(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public void InsertFailedSMSTrail(string MobileNo, string MSG)
        {
            string Result = "";
            try
            {
                
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertUpdateFailedSMSTrail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, MobileNo);
                objDBAccess.AddParameterToCommand("@MSG", DBFactoryParameterType.NVarchar, MSG);

                Result = (objDBAccess.ExecuteCommand().ToString());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EsanchaarModel", FunctionName = " InsertFailedSMSTrail(string MobileNo, string MSG)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
        }
    }
}