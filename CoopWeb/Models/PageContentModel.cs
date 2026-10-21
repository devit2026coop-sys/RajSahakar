using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.UI;
using System.Web;

namespace Cooperative.Models
{
    public class PageContentModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;


        public long PageId { get; set; }      
        public string PageName { get; set; }
        [DataType(DataType.Html)]
        public string PageDescEn { get; set; }
        [DataType(DataType.Html)]
        public string PageDescHi { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }
        public string UserIds { get; set; }
        public List<TrainningUser> lstTrainningUsers { get; set; }

        public class TrainningUser
        {
            //public int TrainningId { get; set; }
            public int UserId { get; set; }

        }

        public PageContentModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public PageContentModel()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public int AddPageContent()
        {
            int Result = 0;
            try
            {
                var DOBConvert = Convert.ToDateTime(System.DateTime.Now);

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertMasterPageContent", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageId", DBFactoryParameterType.Int, PageId);
                objDBAccess.AddParameterToCommand("@PageDescEn", DBFactoryParameterType.NVarchar, PageDescEn);
                objDBAccess.AddParameterToCommand("@PageDescHi", DBFactoryParameterType.NVarchar, PageDescHi);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, true);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, "");
//                Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PageContent", FunctionName = "AddPageContent()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataTable GetAllPageContent(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                WhereClause = " 1=1";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetPageContent", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PageContent", FunctionName = "GetAllPageContent(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public int DeleteChangeStatusPageContent(int PageId, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteStautsTrainning", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageId", DBFactoryParameterType.BigInt, PageId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);

                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.PageContent", FunctionName = "DeleteChangeStatusPageContent(int TrainningId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
    }
}