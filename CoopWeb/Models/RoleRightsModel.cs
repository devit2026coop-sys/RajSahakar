
using Cooperative.DBConnector;
using System;
using System.Data;

namespace Cooperative.Models
{
    public class RoleRightsModel 
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public Int64 RoleID { get; set; }
        public string RoleName_EN { get; set; }
        public string RoleName_HI { get; set; }
        public string RoleShortName_EN { get; set; }
        public string RoleShortName_HI { get; set; }
        public Int64 CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public bool IsActive { get; set; }
        public string WhereClause { get; set; }

        public RoleRightsModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }




       

        //
       
        //
    }
}