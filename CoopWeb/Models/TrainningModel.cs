using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.UI;
using System.Web;
using System.Globalization;
using sun.swing;
using System.ServiceModel.Channels;

namespace Cooperative.Models
{

    public class TrainningModel
    {

        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;


        public long TrainningId { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Program Name (English) - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Programme Name (English) - 100 characters allowed")]
        public string ProgrammeNameEn { get; set; }


        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Menu Name (English) - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Programme Name (Hindi) - 100 characters allowed")]
        public string ProgrammeNameHi { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public int ChannelId { get; set; }
        public int TargetGroupId { get; set; }
        public string TargetGroupName { get; set; }

        public int IsAccomodation { get; set; }
        public int Location { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }
        public string UserIds { get; set; }

        public string HtmlContent { get; set; }
        public string HtmlContentRequest { get; set; }

        public int WorkScopeLevel_ID { get; set; }
        public int OrganizationLevelsMappingID { get; set; }
        public int DesignationLevelMappingId { get; set; }
        public List<TrainningUser> lstTrainningUsers { get; set; }
        public List<TrainningAttendanceUser> lstTrainningAttendanceUsers { get; set; }
        public int OrganisationId { get; set; }

        public string Remark { get; set; }
        public int IsAccepted { get; set; }
        public string AppAuthKey { get; set; }

        public class TrainningUser
        {
            //public int TrainningId { get; set; }
            public int UserId { get; set; }
            public int DepartMappingId { get; set; }

        }



        public TrainningModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public TrainningModel()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public int AddTrainning(TrainningModel _Model)
        {
            DBFactoryMSSQL objDBAccess;
            //objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            int Result = 0;
            try
            {
                var Dateformat = new string[] { "dd/MM/yyyy", "dd-MM-yyyy" };
                var DOBConvert = Convert.ToDateTime(System.DateTime.Now);

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertTrainning", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TrainningId", DBFactoryParameterType.Int, TrainningId);
                objDBAccess.AddParameterToCommand("@ProgrammeNameEn", DBFactoryParameterType.NVarchar, ProgrammeNameEn);
                objDBAccess.AddParameterToCommand("@ProgrammeNameHi", DBFactoryParameterType.NVarchar, ProgrammeNameHi);
                objDBAccess.AddParameterToCommand("@StartDate", DBFactoryParameterType.DateTime, StartDate.ToString("dd/MM/yyyy").Contains("0001") ? DateTime.MaxValue : DateTime.ParseExact(StartDate.ToString("dd/MM/yyyy"), Dateformat, new CultureInfo("en-GB"), DateTimeStyles.None));
                objDBAccess.AddParameterToCommand("@EndDate", DBFactoryParameterType.DateTime, EndDate.ToString("dd/MM/yyyy").Contains("0001") ? DateTime.MaxValue : DateTime.ParseExact(EndDate.ToString("dd/MM/yyyy"), Dateformat, new CultureInfo("en-GB"), DateTimeStyles.None));
                objDBAccess.AddParameterToCommand("@StartTime", DBFactoryParameterType.NVarchar, StartTime);
                objDBAccess.AddParameterToCommand("@EndTime", DBFactoryParameterType.NVarchar, EndTime);
                objDBAccess.AddParameterToCommand("@ChannelId", DBFactoryParameterType.Int, ChannelId);
                objDBAccess.AddParameterToCommand("@TargetGroupId", DBFactoryParameterType.Int, TargetGroupId);
                objDBAccess.AddParameterToCommand("@IsAccomodation", DBFactoryParameterType.Int, IsAccomodation);
                objDBAccess.AddParameterToCommand("@Location", DBFactoryParameterType.Int, Location);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, true);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@HtmlContent", DBFactoryParameterType.NVarchar, HtmlContent);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.TrainningModel", FunctionName = "AddTrainning(TrainningModel _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            //if (_Model.TrainningId == 0)
            //{
            //    foreach (TrainningUser item in _Model.lstTrainningUsers)
            //    {
            //        try
            //        {

            //            objDBAccess.OpenConnection();
            //            SqlCommand cmd = objDBAccess.CreateCommand("InsertTranningUsers", DBFactoryCommandType.PROCEDURE_COMMAND);
            //            objDBAccess.AddParameterToCommand("@TrainningId", DBFactoryParameterType.BigInt, Result);
            //            objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, item.UserId);
            //            objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.BigInt, 1);
            //            int Result2 = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            //        }
            //        catch (Exception ex)
            //        {
            //            DBLogger objDBLogger = new DBLogger();
            //            objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "int SocietyProfile(SocietyRegistrationEntity _Model)" }, ex);
            //        }
            //        finally
            //        {
            //            objDBAccess.CloseConnection();
            //        }


            //    }
            //}
            //else
            //{
            //    return Result;
            //}
            return Result;
        }


        public int InsertTrainningUsersList(TrainningModel _Model)
        {
            DBFactoryMSSQL objDBAccess;
            objDBAccess = new DBFactoryMSSQL();
            int Result = 0;
            if (_Model.TrainningId != 0)
            {
                foreach (TrainningUser item in _Model.lstTrainningUsers)
                {
                    try
                    {
                        objDBAccess.OpenConnection();
                        SqlCommand cmd = objDBAccess.CreateCommand("InsertTrainningUser", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@TrainningId", DBFactoryParameterType.Int, TrainningId);
                        objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, item.UserId);
                        objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.BigInt, item.DepartMappingId);
                        objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, 1);
                        Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                    }
                    catch (Exception ex)
                    {
                        DBLogger objDBLogger = new DBLogger();
                        objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.TrainningModel", FunctionName = "InsertTrainningUsersList(TrainningModel _Model)" }, ex);
                    }
                    finally
                    {
                        objDBAccess.CloseConnection();
                    }
                }
            }
            return Result;
        }


        public int TrainningRejectByDepartment(TrainningModel _Model)
        {
            DBFactoryMSSQL objDBAccess;
            objDBAccess = new DBFactoryMSSQL();
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("TrainningRejectByDepartment", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TrainningId", DBFactoryParameterType.Int, _Model.TrainningId);
                objDBAccess.AddParameterToCommand("@RejectedBy", DBFactoryParameterType.Int, _Model.CreatedBy);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, _Model.Remark);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.TrainningModel", FunctionName = "TrainningRejectByDepartment(TrainningModel _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }


            return Result;
        }

        public int InsertTraningAttendanceUsersList(TrainningModel _Model)
        {
            DBFactoryMSSQL objDBAccess;
            objDBAccess = new DBFactoryMSSQL();
            int Result = 0;

            foreach (TrainningAttendanceUser objapp in _Model.lstTrainningAttendanceUsers)
            {
                try
                {
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("InsertTrainningAttendanceUser", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@TrainningId", DBFactoryParameterType.Int, objapp.TrainningId);
                    objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, objapp.UserId);
                    objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, objapp.MappingId);
                    objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, objapp.IsActive);
                    objDBAccess.AddParameterToCommand("@IsApproveTraining", DBFactoryParameterType.Bit, objapp.IsApproveTraining);
                    if (objapp.TrainningDate.HasValue)
                    {
                        objDBAccess.AddParameterToCommand("@TrainningDate", DBFactoryParameterType.DateTime, objapp.TrainningDate.Value);
                    }
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                catch (Exception ex)
                {
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.TrainningModel", FunctionName = "InsertTraningAttendanceUsersList(TrainningModel _Model)" }, ex);
                }
                finally
                {
                    objDBAccess.CloseConnection();
                }
            }


            return Result;
        }


        public DataTable GetUserNameList(string UserIds)
        {
            string WhereClause = "";
            dt.Clear();
            try
            {
                WhereClause = " Where Users.UserId in (" + UserIds + ")";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetUserNameList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.TrainningModel", FunctionName = "DataTable GetUserNameList(string UserIds)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetAllTrainning(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("TrainningList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.TrainningModel", FunctionName = "GetAllTrainning(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


        public DataTable ApproveAttendanceTrainningList(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ApproveAttendanceTrainningList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.TrainningModel", FunctionName = "GetAllTrainning(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetAllAcceptUser(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("TrainningAcceptUserList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.TrainningModel", FunctionName = "GetAllAcceptUser(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetAllAttendanceUser(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("TrainningAttendanceUserList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.TrainningModel", FunctionName = "GetAllAttendanceUser(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int DeleteChangeStatusTrainning(int TrainningId, int Action)
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
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.TrainningModel", FunctionName = "DeleteChangeStatusTrainning(int TrainningId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public int AddTrainningRequest(TrainningModel _Model)
        {
            DBFactoryMSSQL objDBAccess;
            //objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            int Result = 0;
            try
            {
                var Dateformat = new string[] { "dd/MM/yyyy", "dd-MM-yyyy" };
                var DOBConvert = Convert.ToDateTime(System.DateTime.Now);

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertTrainningRequest", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TrainningId", DBFactoryParameterType.Int, TrainningId);
                objDBAccess.AddParameterToCommand("@ProgrammeNameEn", DBFactoryParameterType.NVarchar, ProgrammeNameEn);
                objDBAccess.AddParameterToCommand("@ProgrammeNameHi", DBFactoryParameterType.NVarchar, ProgrammeNameHi);
                objDBAccess.AddParameterToCommand("@TargetGroupId", DBFactoryParameterType.Int, TargetGroupId);
                objDBAccess.AddParameterToCommand("@IsAccomodation", DBFactoryParameterType.Int, IsAccomodation);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, true);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, UpdatedIP);
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, UpdatedIP);
                objDBAccess.AddParameterToCommand("@HtmlContentRequest", DBFactoryParameterType.NVarchar, HtmlContentRequest);
                objDBAccess.AddParameterToCommand("@IsRequestedTrainning", DBFactoryParameterType.Bit, true);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.TrainningModel", FunctionName = "AddTrainning(TrainningModel _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public DataTable GetOrganisationOfUser(int RoleId, int UserId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetOrganisationOfUserByRoleIdUserId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, RoleId);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, UserId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.TrainningModel", FunctionName = "GetOrganisationOfUser(int RoleId ,int UserId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public string UpdateDob(int UserId, DateTime Date)
        {
            string Result = "-1";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateDOB", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserID", DBFactoryParameterType.Int, UserId);
                objDBAccess.AddParameterToCommand("@Dob", DBFactoryParameterType.Date, Date);
                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "UpdateDob(int UserId , DateTime Date )" }, ex);
                Result = "-1";
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public DataTable GetTrainningDashBoardData()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("TrainningDashBoardData", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.TrainningModel", FunctionName = " GetTrainningDashBoardData()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }

        public DataTable TrainigRequestByDepartment(int ChannelId, int OrganizationId, int TrainingId, int UserId, int RoleId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("TrainigRequestByDepartment", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ChannelId", DBFactoryParameterType.BigInt, ChannelId);
                objDBAccess.AddParameterToCommand("@OrganisationId", DBFactoryParameterType.BigInt, OrganizationId);
                objDBAccess.AddParameterToCommand("@TrainningId", DBFactoryParameterType.BigInt, TrainingId);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.BigInt, UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.BigInt, RoleId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.TrainningModel", FunctionName = " public DataTable TrainigRequestByDepartment(int ChannelId, int OrganizationId,int TrainingId,int UserId,int RoleId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }

        public DataTable GetProgramNameByTrainingId(long TrainingId = 0)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetTrainingNameByTrainingId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TrainningId", DBFactoryParameterType.BigInt, TrainingId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.TrainningModel", FunctionName = " public DataTable GetProgramNameByTrainingId(int ChannelId, int OrganizationId,int TrainingId,int UserId,int RoleId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }

        public DataTable GetTrainingAttendanceByTrainingId(long TrainingId = 0)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetTrainingAttendanceByTrainingId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TrainningId", DBFactoryParameterType.BigInt, TrainingId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.TrainningModel", FunctionName = " public DataTable GetTrainingAttendanceByTrainingId(int ChannelId, int OrganizationId,int TrainingId,int UserId,int RoleId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }
        public int SaveRICEMResponseOnTrainingRequest(TrainningModel _Model)
        {
            DBFactoryMSSQL objDBAccess;
            //objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            int Result = 0;
            try
            {
                var Dateformat = new string[] { "dd/MM/yyyy", "dd-MM-yyyy" };
                var DOBConvert = Convert.ToDateTime(System.DateTime.Now);

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateTrainningStatusByRICEM ", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TrainningId", DBFactoryParameterType.Int, TrainningId);
                objDBAccess.AddParameterToCommand("@IsAccepted", DBFactoryParameterType.Int, IsAccepted);
                objDBAccess.AddParameterToCommand("@StartDate", DBFactoryParameterType.DateTime, StartDate.ToString("dd/MM/yyyy").Contains("0001") ? DateTime.MaxValue : DateTime.ParseExact(StartDate.ToString("dd/MM/yyyy"), Dateformat, new CultureInfo("en-GB"), DateTimeStyles.None));
                objDBAccess.AddParameterToCommand("@Enddate", DBFactoryParameterType.DateTime, EndDate.ToString("dd/MM/yyyy").Contains("0001") ? DateTime.MaxValue : DateTime.ParseExact(EndDate.ToString("dd/MM/yyyy"), Dateformat, new CultureInfo("en-GB"), DateTimeStyles.None));
                objDBAccess.AddParameterToCommand("@StartTime", DBFactoryParameterType.NVarchar, StartTime);
                objDBAccess.AddParameterToCommand("@EndTime", DBFactoryParameterType.NVarchar, EndTime);
                objDBAccess.AddParameterToCommand("@ChannelId", DBFactoryParameterType.Int, ChannelId);
                objDBAccess.AddParameterToCommand("@IsAccomodation", DBFactoryParameterType.Int, IsAccomodation);
                objDBAccess.AddParameterToCommand("@Location", DBFactoryParameterType.Int, Location);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, true);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@IP", DBFactoryParameterType.NVarchar, CreatedIP);
                objDBAccess.AddParameterToCommand("@HtmlContent", DBFactoryParameterType.NVarchar, HtmlContent);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Remark);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.TrainningModel", FunctionName = "public int SaveRICEMResponseOnTrainingRequest(TrainningModel _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public string FormatDateWithSuffix(DateTime date)
        {
            int day = date.Day;

            string suffix;
            if (day >= 11 && day <= 13)
                suffix = "th";
            else
            {
                switch (day % 10)
                {
                    case 1: suffix = "st"; break;
                    case 2: suffix = "nd"; break;
                    case 3: suffix = "rd"; break;
                    default: suffix = "th"; break;
                }
            }

            return $"{day}{suffix} {date:MMMM, yyyy}";
        }

        public string ConvertAmountToWords(string amount)
        {
            if (string.IsNullOrWhiteSpace(amount))
                return "(Rupees Zero Only)";

            decimal value = Convert.ToDecimal(amount);
            return $"(Rupees {NumberToWords((long)value)} Only)";
        }

        public static string NumberToWords(long number)
        {
            if (number == 0)
                return "Zero";

            if (number < 0)
                return "Minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 10000000) > 0)
            {
                words += NumberToWords(number / 10000000) + " Crore ";
                number %= 10000000;
            }

            if ((number / 100000) > 0)
            {
                words += NumberToWords(number / 100000) + " Lakh ";
                number %= 100000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                string[] unitsMap = {
            "Zero","One","Two","Three","Four","Five","Six","Seven","Eight","Nine",
            "Ten","Eleven","Twelve","Thirteen","Fourteen","Fifteen","Sixteen",
            "Seventeen","Eighteen","Nineteen" };
                string[] tensMap = { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
            }
            return words.Trim();
        }


        public int RICEMTrainingAttendance(int TrainningId = 0, string Filepath = "", int IsEsign = 0)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("USP_RICEMTrainingAttendance", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TrainningId", DBFactoryParameterType.Int, TrainningId);
                objDBAccess.AddParameterToCommand("@Filepath", DBFactoryParameterType.NVarchar, Filepath);
                objDBAccess.AddParameterToCommand("@IsEsign", DBFactoryParameterType.NVarchar, IsEsign);
                result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Training", FunctionName = "RICEMTrainingAttendance(int TrainningId = 0, string Filepath = \"\", int IsEsign = 0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return result;
        }
        public int RICEMTrainingSaveFilePath(int TrainningId = 0, string Filepath = "", int Type = 0)
        {
            int result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("USP_TrainingFilePath", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TrainningId", DBFactoryParameterType.Int, TrainningId);
                objDBAccess.AddParameterToCommand("@Filepath", DBFactoryParameterType.NVarchar, Filepath);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, Type);
                result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Training", FunctionName = "RICEMTrainingSaveFilePath(int TrainningId = 0, string Filepath = \"\", int Type = 0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return result;
        }

        public int InsertTraningAttendanceUsersApproved(TrainningAttendanceUser model)
        {
            DBFactoryMSSQL objDBAccess;
            objDBAccess = new DBFactoryMSSQL();
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertTrainningAttendanceUserApproved", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TrainningId", DBFactoryParameterType.Int, model.TrainningId);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, model.UserId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, model.MappingId);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, model.IsActive);
                objDBAccess.AddParameterToCommand("@IsApproveTraining", DBFactoryParameterType.Bit, model.IsApproveTraining);
                if (model.TrainningDate.HasValue)
                {
                    objDBAccess.AddParameterToCommand("@TrainningDate", DBFactoryParameterType.DateTime, model.TrainningDate.Value);
                }
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.TrainningModel", FunctionName = "InsertTraningAttendanceUsersApproved(TrainningModel _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }


            return Result;
        }



        public DataTable TrainningAttendanceUsersList(long TrainingId = 0)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("USP_TrainningAttendanceUser", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TrainingId", DBFactoryParameterType.BigInt, TrainingId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.TrainningModel", FunctionName = "TrainningAttendanceUsersList(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable TrainningAttendanceInvoiceDetails(long TrainingId = 0)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("USP_TrainningAttendanceInvoice", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TrainingId", DBFactoryParameterType.BigInt, TrainingId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.TrainningModel", FunctionName = "TrainningAttendanceInvoiceDetails(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

    }

    public class TrainningAttendanceUser
    {
        //public int TrainningId { get; set; }
        public int UserId { get; set; }
        public int TrainningId { get; set; }
        public int IsActive { get; set; }
        public long MappingId { get; set; }
        public DateTime? TrainningDate { get; set; }
        public bool IsApproveTraining { get; set; }

    }

    public class TrainningFeedbacksModel
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
        public string Feedback { get; set; }
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
        public int TrainningId { get; set; }
        public int UserId { get; set; }
        public List<TrainningUser> lstTrainningUsers { get; set; }

        public class TrainningUser
        {
            //public int TrainningId { get; set; }
            public int UserId { get; set; }

        }

        public TrainningFeedbacksModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public TrainningFeedbacksModel()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public int AddFeedback(TrainningFeedbacksModel _Model)
        {
            int Result = 0;
            try
            {
                var DOBConvert = Convert.ToDateTime(System.DateTime.Now);

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertTrainningFeedback", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TrainningId", DBFactoryParameterType.Int, _Model.TrainningId);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _Model.UserId);
                objDBAccess.AddParameterToCommand("@Feedback", DBFactoryParameterType.NVarchar, _Model.Feedback);
                objDBAccess.AddParameterToCommand("@StartDate", DBFactoryParameterType.DateTime, DOBConvert);
                objDBAccess.AddParameterToCommand("@EndDate", DBFactoryParameterType.DateTime, DOBConvert);
                objDBAccess.AddParameterToCommand("@StartTime", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@EndTime", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, true);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, _Model.CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, _Model.UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, _Model.CreatedIP);
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, _Model.UpdatedIP);

                // Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.TrainningModel", FunctionName = "AddFeedback(FeedbacksModel _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataTable GetAllFeedback(string WhereClause)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("FeedbackList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.TrainningModel", FunctionName = "GetAllFeedback(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetUserTrainningList(int UserId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UserTrainningList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, UserId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.TrainningModel", FunctionName = "GetUserTrainningList(string UserId)" }, ex);
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