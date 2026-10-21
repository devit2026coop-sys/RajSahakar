using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Cooperative.Models
{
    public class Court
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public Court(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public Court()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public DataTable UpdateNextHearingDate(int Id, DateTime NextHearingDate)
        {
            DBFactoryMSSQL objDBAccess;
            //objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
            // dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateNextHearingDate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, Id);
                objDBAccess.AddParameterToCommand("@NextHearingDate", DBFactoryParameterType.DateTime, @NextHearingDate);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Court", FunctionName = "public DataTable UpdateNextHearingDate(int Id, DateTime NextHearingDate)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
    }
}