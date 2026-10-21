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
    public class FeedbacksModel
    {

        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;


        public long FeedbackId { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Menu Name (English) - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Name (English) - 100 characters allowed")]
        public string Name { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string Detail { get; set; }
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

        public FeedbacksModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public FeedbacksModel()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public int AddFeedback()
        {
            int Result = 0;
            try
            {
                var DOBConvert = Convert.ToDateTime(System.DateTime.Now);

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertTrainning", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@TrainningId", DBFactoryParameterType.Int, TrainningId);
                //objDBAccess.AddParameterToCommand("@ProgrammeNameEn", DBFactoryParameterType.NVarchar, ProgrammeNameEn);
                //objDBAccess.AddParameterToCommand("@ProgrammeNameHi", DBFactoryParameterType.NVarchar, ProgrammeNameHi);
                //objDBAccess.AddParameterToCommand("@StartDate", DBFactoryParameterType.DateTime, StartDate);
                //objDBAccess.AddParameterToCommand("@EndDate", DBFactoryParameterType.DateTime, EndDate);
                ////objDBAccess.AddParameterToCommand("@StartTime", DBFactoryParameterType.NVarchar, StartTime);
                ////objDBAccess.AddParameterToCommand("@EndTime", DBFactoryParameterType.NVarchar, EndTime);
                //objDBAccess.AddParameterToCommand("@StartTime", DBFactoryParameterType.NVarchar, "");
                //objDBAccess.AddParameterToCommand("@EndTime", DBFactoryParameterType.NVarchar, "");
                //objDBAccess.AddParameterToCommand("@ChannelId", DBFactoryParameterType.Int, ChannelId);
                //objDBAccess.AddParameterToCommand("@TargetGroupId", DBFactoryParameterType.Int, TargetGroupId);
                //objDBAccess.AddParameterToCommand("@IsAccomodation", DBFactoryParameterType.Int, IsAccomodation);
                //objDBAccess.AddParameterToCommand("@Location", DBFactoryParameterType.Int, Location);
                //objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, true);
                //objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                //objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                //objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, "");
                //objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, "");

                // Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Feedbacks", FunctionName = "AddFeedback()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataTable GetAllFeedback(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("FeefbackList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Feedbacks", FunctionName = "GetAllFeedback(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public int DeleteChangeStatusFeedback(int TrainningId, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteStautsTrainning", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TrainningId", DBFactoryParameterType.BigInt, TrainningId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);

                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Feedbacks", FunctionName = "DeleteChangeStatusFeedback(int TrainningId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

    }
}