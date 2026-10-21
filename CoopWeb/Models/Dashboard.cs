using System;
using Cooperative.DBConnector;
using System.Data;
using System.Web;
using System.Data.SqlClient;

namespace Cooperative.Models
{
    public class Dashboard
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public Dashboard(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public Dashboard()
        {
            objDBAccess = new DBFactoryMSSQL();

        }

        public DataTable GetSocietyCountTypeWiseForDashboard(string WhereClause = "")
        {

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("SocietyTypeGraphCount", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Dashboard", FunctionName = "GetSocietyCountTypeWiseForDashboard(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
    }
}