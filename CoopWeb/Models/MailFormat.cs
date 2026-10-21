using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Cooperative.Models
{
    public class MailFormat
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public long TemplateId { get; set; }
        public string TemplateHeadingEN { get; set; }
        public string TemplateHeadingHI { get; set; }
        public string TemplateBodyEN { get; set; }
        public string TemplateBodyHI { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }
        public DateTime CreateDate { get; set; }


        public MailFormat(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public MailFormat()
        {
            objDBAccess = new DBFactoryMSSQL();            
        }

        public DataTable GetSupportData()
        {

            FileTestLogger objILoggerTest = new FileTestLogger();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand(WhereClause, DBFactoryCommandType.TEXT_COMMAND);
                dt = objDBAccess.GetTable();               
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MailFormat", FunctionName = "GetSupportData()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }
    }

}