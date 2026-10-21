using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace Cooperative.Models
{
    public class PagePath
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;
        public int Id { get; set; }
        public string MenuNameEn { get; set; }
        public string MenuNameHI { get; set; }
        public bool IsParent { get; set; }
        public int ParentId { get; set; }
        public string MenuUrl { get; set; }
        public string MenuIcon { get; set; }
        public int Priority { get; set; }
        public PagePath()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public DataTable GeneratePageLevelPath(int MenuId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SelectPageLevelPath", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MenuId", DBFactoryParameterType.Int, MenuId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PagePath", FunctionName = "GeneratePageLevelPath(int UserId)" }, ex);
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