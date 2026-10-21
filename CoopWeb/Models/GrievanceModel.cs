using Cooperative.DBConnector;
using java.math;
using java.util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using Cooperative.Controllers;


namespace Cooperative.Models
{
    public class GrievanceModel
    {
        SqlTransaction Xsaction;
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public GrievanceModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }



        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        public DataTable GetDepartmentList(string Ssoid, string moduleType = "")
        {
            try
            {

                SqlCommand cmd = objDBAccess.CreateCommand("GetBMSDeptList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSoid", DBFactoryParameterType.NVarchar, Ssoid);
                objDBAccess.AddParameterToCommand("@ModuleType", DBFactoryParameterType.NVarchar, moduleType);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "GetDepartmentList()" }, ex);
            }
            finally
            {
                //objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }

        public DataTable GetModuleList(string Department, string Ssoid, string moduleType = "")
        {
            try
            {

                SqlCommand cmd = objDBAccess.CreateCommand("GetModuleListByDepartment", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Department", DBFactoryParameterType.NVarchar, Department);
                objDBAccess.AddParameterToCommand("@Ssoid", DBFactoryParameterType.NVarchar, Ssoid);
                objDBAccess.AddParameterToCommand("@ModuleType", DBFactoryParameterType.NVarchar, moduleType);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "GetModuleList(string Deaprtment)" }, ex);
            }
            finally
            {
                //objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }
        public DataTable GetSubModuleList(int ModuleId, int DepartmentId)
        {
            try
            {

                SqlCommand cmd = objDBAccess.CreateCommand("GetSubModuleList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ModuleId", DBFactoryParameterType.Int, ModuleId);
                objDBAccess.AddParameterToCommand("@DepartmentId", DBFactoryParameterType.Int, DepartmentId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "GetModuleList(int DeaprtmentId)" }, ex);
            }
            finally
            {
                //objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }

        public DataTable GetCategoryList()
        {
            try
            {

                SqlCommand cmd = objDBAccess.CreateCommand("GrievanceManagement", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetCategoryList");
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "GetCategoryList()" }, ex);
            }
            finally
            {
                //objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }

        public DataTable GetDeveloperList()
        {
            try
            {

                SqlCommand cmd = objDBAccess.CreateCommand("GrievanceManagement", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetDeveloperList");
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "GetDeveloperList()" }, ex);
            }
            finally
            {
                //objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }
        public DataTable GetIssueRelatedList()
        {
            try
            {

                SqlCommand cmd = objDBAccess.CreateCommand("GrievanceManagement", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GrievanceIssueRelatedTo");
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "GetIssueRelatedList()" }, ex);
            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }
        public DataTable GetGrivanceCategoryList()
        {
            try
            {

                SqlCommand cmd = objDBAccess.CreateCommand("GrievanceManagement", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GrievanceCategoryList");
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "GetGrivanceCategoryList()" }, ex);
            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }
        public DataTable GetGrivanceSubCategoryList(int GriveanceCategoryId)
        {
            try
            {

                SqlCommand cmd = objDBAccess.CreateCommand("GrievanceManagement", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GrievanceSubCategoryList");
                objDBAccess.AddParameterToCommand("@GriveanceCategoryId", DBFactoryParameterType.Int, GriveanceCategoryId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "GetGrivanceSubCategoryList(int GriveanceCategoryId)" }, ex);
            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }
        public int SaveGrievance(GrievanceEntity obj)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GrievanceManagement", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GrievanceRegister");
                objDBAccess.AddParameterToCommand("@IssueFrom", DBFactoryParameterType.Int, obj.IssueFrom);
                objDBAccess.AddParameterToCommand("@ModuleId", DBFactoryParameterType.Int, obj.ModuleId);
                objDBAccess.AddParameterToCommand("@CategoryId", DBFactoryParameterType.Int, obj.CategoryId);
                objDBAccess.AddParameterToCommand("@Subject", DBFactoryParameterType.NVarchar, obj.Subject);
                objDBAccess.AddParameterToCommand("@Description", DBFactoryParameterType.NVarchar, obj.Description);
                objDBAccess.AddParameterToCommand("@Attachment", DBFactoryParameterType.NVarchar, obj.Attachment);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, obj.MobileNo);
                objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, obj.SSOId);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.BigInt, obj.UserId);
                objDBAccess.AddParameterToCommand("@UserMobileNo", DBFactoryParameterType.NVarchar, obj.UserMobileNo);
                objDBAccess.AddParameterToCommand("@ApplicationId", DBFactoryParameterType.NVarchar, obj.ApplicationId);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "SaveGrievance(GrievanceModel Entity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GrievanceList(string WhereClause, Int64 userid = 0)
        {
            dt.Clear();
            try
            {
                SqlCommand cmd = objDBAccess.CreateCommand("GrievanceManagement", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetGrievanceList");
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                objDBAccess.AddParameterToCommand("@login_userid", DBFactoryParameterType.BigInt, userid);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "GrievanceList(int ModuleId)" }, ex);
            }
            finally
            {
                //objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }

        public DataTable GetGrievanceListBySSO()
        {
            dt.Clear();
            try
            {

                SqlCommand cmd = objDBAccess.CreateCommand("GrievanceManagement", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetGrievanceListBySSO");
                objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "GetGrievanceListBySSO()" }, ex);
            }
            finally
            {
                //objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }



        public int AssignToDeveloper(int GrievanceId, int DeveloperId, string Priority, string Remark, string Action, string Attachment = "", string IssueRelatedTO = "")
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GrievanceManagement", DBFactoryCommandType.PROCEDURE_COMMAND);
                if (Action == "Close")
                {
                    objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "DisposedByHelpdesk");
                }
                else if (Action == "AssignToDeveloper")
                {
                    objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "AssignToDeveloper");
                }
                objDBAccess.AddParameterToCommand("@GrievanceId", DBFactoryParameterType.Int, GrievanceId);
                objDBAccess.AddParameterToCommand("@AssignedTo", DBFactoryParameterType.Int, DeveloperId);
                objDBAccess.AddParameterToCommand("@Priority", DBFactoryParameterType.NVarchar, Priority);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Remark);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@Attachment", DBFactoryParameterType.NVarchar, Attachment);
                objDBAccess.AddParameterToCommand("@IssueRelatedTO", DBFactoryParameterType.NVarchar, IssueRelatedTO);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "AssignToDeveloper(int GrievanceId, int DeveloperId,string Priority, string Remark)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int DisposeGrievance(int GrievanceId, string Remark, string Attachment = "")
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GrievanceManagement", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "DisposedByDeveloper");
                objDBAccess.AddParameterToCommand("@GrievanceId", DBFactoryParameterType.Int, GrievanceId);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Remark);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@Attachment", DBFactoryParameterType.NVarchar, Attachment);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "DisposeGrievance(int GrievanceId, string Remark)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int FinalAction(int GrievanceId, int DeveloperId, string Priority, string Remark, string Action, string Attachment = "")
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GrievanceManagement", DBFactoryCommandType.PROCEDURE_COMMAND);
                if (Action == "Close")
                {
                    objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "DisposedByHelpdesk");
                }
                else if (Action == "Reopen")
                {
                    objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "ReassignToDeveloper");
                }
                objDBAccess.AddParameterToCommand("@GrievanceId", DBFactoryParameterType.Int, GrievanceId);
                objDBAccess.AddParameterToCommand("@AssignedTo", DBFactoryParameterType.Int, DeveloperId);
                objDBAccess.AddParameterToCommand("@Priority", DBFactoryParameterType.NVarchar, Priority);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Remark);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@Attachment", DBFactoryParameterType.NVarchar, Attachment);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "FinalAction(int GrievanceId, int DeveloperId, string Remark,string Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public int ReopenByUser(int GrievanceId, string Remark, string Attachment = "")
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GrievanceManagement", DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "ReopenByUser");
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, _LoggedInUser.UserId);



                //objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "ReopenByUser");
                objDBAccess.AddParameterToCommand("@GrievanceId", DBFactoryParameterType.Int, GrievanceId);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Remark);
                objDBAccess.AddParameterToCommand("@Attachment", DBFactoryParameterType.NVarchar, Attachment);
                //objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, _LoggedInUser.UserId);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "ReopenByUser(int GrievanceId, string Remark, string Attachment)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int RemarkByUser(int GrievanceId, string Remark, string Attachment)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GrievanceManagement", DBFactoryCommandType.PROCEDURE_COMMAND);


                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "RemarkByUser");
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, _LoggedInUser.UserId);

                //objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "RemarkByUser");
                objDBAccess.AddParameterToCommand("@GrievanceId", DBFactoryParameterType.Int, GrievanceId);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Remark);
                objDBAccess.AddParameterToCommand("@Attachment", DBFactoryParameterType.NVarchar, Attachment);
                //objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, _LoggedInUser.UserId);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "RemarkByUser(int GrievanceId, string Remark, string Attachment)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable ActionByIA(int GrievanceId, int DeveloperId, string Priority, string Remark, string Action, int GrivanceCategoryid, int GrivanceSubCategoryid, string Attachment = "", string IssueRelatedTO = "")
        {

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GrievanceManagement", DBFactoryCommandType.PROCEDURE_COMMAND);
                if (Action == "Dispose")
                {
                    objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "DisposedByIA");
                }
                else if (Action == "AssignToBA")
                {
                    objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "AssignToBA");
                }
                else if (Action == "AssignToDeveloper")
                {
                    objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "AssignToDeveloper");
                }
                else if (Action == "MoreInformation")
                {
                    objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "MoreInformation");
                }
                else if (Action == "Reopen")
                {
                    objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "ReassignToBA");
                }
                objDBAccess.AddParameterToCommand("@GrievanceId", DBFactoryParameterType.Int, GrievanceId);
                objDBAccess.AddParameterToCommand("@AssignedTo", DBFactoryParameterType.Int, DeveloperId);
                objDBAccess.AddParameterToCommand("@Priority", DBFactoryParameterType.NVarchar, Priority);
                objDBAccess.AddParameterToCommand("@GriveanceCategoryId", DBFactoryParameterType.Int, GrivanceCategoryid);
                objDBAccess.AddParameterToCommand("@GriveanceSubCategoryId", DBFactoryParameterType.Int, GrivanceSubCategoryid);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Remark);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@Attachment", DBFactoryParameterType.NVarchar, Attachment);
                objDBAccess.AddParameterToCommand("@IssueRelatedTO", DBFactoryParameterType.NVarchar, IssueRelatedTO);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "ActionByIA(int GrievanceId, string Remark, string Action)" }, ex);
            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }

        public DataTable GetGrievancetReport(string WhereClause)
        {
            try
            {

                SqlCommand cmd = objDBAccess.CreateCommand("GrievanceManagement", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetGrievanceReport");
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "GetGrievancetReport(int ModuleId,string Priority,int Status,string FromDate,string ToDate)" }, ex);
            }
            finally
            {
                //objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }

        public DataTable GetGrievanceTrail(int GrievanceId)
        {
            try
            {

                SqlCommand cmd = objDBAccess.CreateCommand("GrievanceManagement", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetGrievanceTrail");
                objDBAccess.AddParameterToCommand("@GrievanceId", DBFactoryParameterType.Int, GrievanceId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "GetGrievanceTrail(int GrievanceId)" }, ex);
            }
            finally
            {
                //objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }
        public DataTable GetGrievanceTrailUser(int GrievanceId)
        {
            try
            {

                SqlCommand cmd = objDBAccess.CreateCommand("GrievanceManagement", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "TrailUserLevel");
                objDBAccess.AddParameterToCommand("@GrievanceId", DBFactoryParameterType.Int, GrievanceId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "GetGrievanceTrail(int GrievanceId)" }, ex);
            }
            finally
            {
                //objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }

        public DataTable GetGrievanceCount(string FromDate, string ToDate)
        {
            try
            {
                SqlCommand cmd = objDBAccess.CreateCommand("GrievanceManagement", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetGrievanceCount");
                objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.NVarchar, FromDate);
                objDBAccess.AddParameterToCommand("@ToDate", DBFactoryParameterType.NVarchar, ToDate);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "GetGrievanceCount(string FromDate,string ToDate)" }, ex);
            }
            finally
            {
                //objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }
        public DataTable GetLeaveList()
        {
            try
            {
                SqlCommand cmd = objDBAccess.CreateCommand("GrievanceIALeaveManagement", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetLeaveList");
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "GetGrievanceCount(string FromDate,string ToDate)" }, ex);
            }
            finally
            {
                //objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }
        public int SubmitLeaveByIA(GrievanceEntity obj)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GrievanceIALeaveManagement", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "SubmitLeave");
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.NVarchar, obj.FromDate);
                objDBAccess.AddParameterToCommand("@ToDate", DBFactoryParameterType.NVarchar, obj.ToDate);
                objDBAccess.AddParameterToCommand("@Reason", DBFactoryParameterType.NVarchar, obj.Reason);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "SubmitLeaveByIA(GrievanceModel Entity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public DataTable DistrictState()
        {
            try
            {
                SqlCommand cmd = objDBAccess.CreateCommand("DistrictList", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetCategoryList");
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = " DistrictState()" }, ex);
            }
            finally
            {
                //objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }
        public DataTable GetAllStatus()
        {
            try
            {
                SqlCommand cmd = objDBAccess.CreateCommand("Statuslist", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = " GetAllStatus()" }, ex);
            }
            finally
            {
                //objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }
        #region Amit
        public int SaveGrivancemasterDepartment(GrivanceMaster obj)
        {
            int Result = 0;
            try
            {
                if (obj.ModuleId == 0)
                {
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("GrievanceMaster", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, 1);
                    objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, obj.ModuleId > 0 ? "RadioDepartment" : "RadioModule");
                    objDBAccess.AddParameterToCommand("@DeptEn", DBFactoryParameterType.NVarchar, obj.DeptEn);
                    objDBAccess.AddParameterToCommand("@DeptHi", DBFactoryParameterType.NVarchar, obj.DeptHi);
                    objDBAccess.AddParameterToCommand("@ModuleEn", DBFactoryParameterType.NVarchar, obj.ModuleEn);
                    objDBAccess.AddParameterToCommand("@ModuleHi", DBFactoryParameterType.NVarchar, obj.ModuleHi);
                    objDBAccess.AddParameterToCommand("@isActive", DBFactoryParameterType.Int, obj.isActive);
                    objDBAccess.AddParameterToCommand("@isDeleted", DBFactoryParameterType.Int, obj.isDeleted);
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                else
                {
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("GrievanceMaster", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, 2);
                    objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, "RadioModule");
                    objDBAccess.AddParameterToCommand("@ModuleId", DBFactoryParameterType.NVarchar, obj.ModuleId);
                    objDBAccess.AddParameterToCommand("@DeptEn", DBFactoryParameterType.NVarchar, obj.DeptEn);
                    objDBAccess.AddParameterToCommand("@DeptHi", DBFactoryParameterType.NVarchar, obj.DeptHi);
                    objDBAccess.AddParameterToCommand("@ModuleEn", DBFactoryParameterType.NVarchar, obj.ModuleEn);
                    objDBAccess.AddParameterToCommand("@ModuleHi", DBFactoryParameterType.NVarchar, obj.ModuleHi);
                    objDBAccess.AddParameterToCommand("@isActive", DBFactoryParameterType.Int, obj.isActive);
                    objDBAccess.AddParameterToCommand("@isDeleted", DBFactoryParameterType.Int, obj.isDeleted);
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "SaveGrivancemasterDepartment(GrivanceMaster obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public DataTable DepartmentList(GrivanceMaster obj)
        {
            try
            {
                SqlCommand cmd = objDBAccess.CreateCommand("GrievanceMaster", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, 3);
                objDBAccess.AddParameterToCommand("@isActive", DBFactoryParameterType.Int, obj.isActive);
                objDBAccess.AddParameterToCommand("@isDeleted", DBFactoryParameterType.Int, obj.isDeleted);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = " DepartmentList(GrivanceMaster obj)" }, ex);
            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }
        public DataTable GetGrivanceMasterList(GrivanceMaster obj)
        {
            try
            {
                SqlCommand cmd = objDBAccess.CreateCommand("GrievanceMaster", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, 4);
                objDBAccess.AddParameterToCommand("@isActive", DBFactoryParameterType.Int, obj.isActive);
                objDBAccess.AddParameterToCommand("@isDeleted", DBFactoryParameterType.Int, obj.isDeleted);
                //objDBAccess.AddParameterToCommand("@ActionId1", DBFactoryParameterType.Bit, obj.ActionId1);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = " GetGrivanceMasterList(GrivanceMaster obj)" }, ex);
            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }
        public int ChangeStatus(GrivanceMaster obj, int ModuleId, int ActionId)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GrievanceMaster", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, 5);
                objDBAccess.AddParameterToCommand("@ModuleId", DBFactoryParameterType.Int, ModuleId);
                objDBAccess.AddParameterToCommand("@isActive", DBFactoryParameterType.Int, ActionId);
                objDBAccess.AddParameterToCommand("@isDeleted", DBFactoryParameterType.Int, obj.isDeleted);
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "ChangeStatus(GrivanceMaster obj,int ModuleId, int ActionId)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public int DeleteGrivance(GrivanceMaster obj, int ModuleId, int isDeleted)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GrievanceMaster", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, 6);
                objDBAccess.AddParameterToCommand("@ModuleId", DBFactoryParameterType.Int, ModuleId);
                objDBAccess.AddParameterToCommand("@isActive", DBFactoryParameterType.Int, isDeleted);
                objDBAccess.AddParameterToCommand("@isDeleted", DBFactoryParameterType.Int, obj.isDeleted);
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "DeleteGrivance(GrivanceMaster obj,int ModuleId, int ActionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public DataTable UpdateGrivance(GrivanceMaster obj, int ModuleId)
        {
            try
            {
                SqlCommand cmd = objDBAccess.CreateCommand("GrievanceMaster", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, 4);
                objDBAccess.AddParameterToCommand("@ModuleId", DBFactoryParameterType.Int, ModuleId);
                objDBAccess.AddParameterToCommand("@isActive", DBFactoryParameterType.Int, obj.isActive);
                objDBAccess.AddParameterToCommand("@isDeleted", DBFactoryParameterType.Int, obj.isDeleted);
                //objDBAccess.AddParameterToCommand("@ActionId1", DBFactoryParameterType.Bit, obj.ActionId1);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = " UpdateGrivance(GrivanceMaster obj)" }, ex);
            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }
        public DataTable GetIALeaveMaster(IAGrivanceLeave obj)
        {
            try
            {
                SqlCommand cmd = objDBAccess.CreateCommand("IAleaveMaster", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, "SelectIA");
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = " GetIALeaveMaster(IAGrivanceLeave obj)" }, ex);
            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }
        public int AddLeaveGrievanceIA(IAGrivanceLeave obj, string SSOID, int RoleId, int OfficeId)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertLeaveGrievanceIA", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, 1);
                objDBAccess.AddParameterToCommand("@LeaveId", DBFactoryParameterType.Int, obj.LeaveId);
                objDBAccess.AddParameterToCommand("@StartDate", DBFactoryParameterType.DateTime, obj.StartDate);
                objDBAccess.AddParameterToCommand("@EndDate", DBFactoryParameterType.DateTime, obj.EndDate);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOID);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, RoleId);
                objDBAccess.AddParameterToCommand("@OfficeId", DBFactoryParameterType.Int, OfficeId);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, obj.IsActive);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = " AddLeaveGrievanceIA(IAGrivanceLeave obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        #region mahesh 10012004
        public DataTable GetDepartmentListForOIC(Int32 Userid)
        {
            try
            {
                SqlCommand cmd = objDBAccess.CreateCommand("Grievance_OIC", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@action", DBFactoryParameterType.NVarchar, "DepartmentNameByUserID");
                objDBAccess.AddParameterToCommand("@LoginUserid", DBFactoryParameterType.NVarchar, Userid);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "GetDepartmentListForOIC()" }, ex);
            }
            finally
            {
                //objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }
        public DataTable GetGrievancetReportForOIC(string WhereClause)
        {
            try
            {

                SqlCommand cmd = objDBAccess.CreateCommand("Grievance_OIC", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetGrievanceReport_OIC");
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "GetGrievancetReportForOIC(int ModuleId,string Priority,int Status,string FromDate,string ToDate)" }, ex);
            }
            finally
            {
                //objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }
        public DataTable UpdateRemarkByOIC(string remark_OIC, string Grievance_id, string fileName)
        {
            try
            {

                SqlCommand cmd = objDBAccess.CreateCommand("Grievance_OIC", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "UpdateRemarkOIC");
                objDBAccess.AddParameterToCommand("@RemarkOIC", DBFactoryParameterType.NVarchar, remark_OIC);
                objDBAccess.AddParameterToCommand("@GID", DBFactoryParameterType.Int, Convert.ToInt32(Grievance_id));
                objDBAccess.AddParameterToCommand("@AttachmentOIC", DBFactoryParameterType.NVarchar, fileName);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "AgriIfarms.Models.GrievanceModel", FunctionName = "UpdateRemarkByOIC(string remark_OIC , string Grievance_id , string fileName)" }, ex);
            }
            finally
            {
                //objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }
        #endregion

        public DataTable GetGrievancelCategoryId(string ListCode)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("NewLovValuesByListId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ListCode", DBFactoryParameterType.NVarchar, ListCode);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GrievanceModel", FunctionName = "GetGrievancelCategoryId(string ListCode)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetGrievancelModuleId(string ListCode)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("NewLovValuesByListId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ListCode", DBFactoryParameterType.NVarchar, ListCode);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GrievanceModel", FunctionName = "GetGrievancelCategoryId(string ListCode)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }



        public DataTable GetGrievanceSectionByModuleId(int ModuleId)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetGrievanceSectionByModuleId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ModuleId", DBFactoryParameterType.Int, ModuleId);
                dt = objDBAccess.GetTable();
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }



        public int SaveDepartmentGrievance(GrievanceDepartmentEntity obj)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SaveGrievanceDepartmentDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GrievanceId", DBFactoryParameterType.Int, obj.GrievanceId);
                objDBAccess.AddParameterToCommand("@IssueFrom", DBFactoryParameterType.Int, obj.IssueFrom);
                objDBAccess.AddParameterToCommand("@DepartmentId", DBFactoryParameterType.Int, obj.DepartmentId);
                objDBAccess.AddParameterToCommand("@ModuleId", DBFactoryParameterType.Int, obj.ModuleId);
                objDBAccess.AddParameterToCommand("@SectionId", DBFactoryParameterType.Int, obj.SectionId);
                objDBAccess.AddParameterToCommand("@CategoryId", DBFactoryParameterType.Int, obj.CategoryId);
                objDBAccess.AddParameterToCommand("@Subject", DBFactoryParameterType.NVarchar, obj.Subject);
                objDBAccess.AddParameterToCommand("@Description", DBFactoryParameterType.NVarchar, obj.Description);
                objDBAccess.AddParameterToCommand("@Attachment", DBFactoryParameterType.NVarchar, obj.Attachment);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, obj.MobileNo);
                objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@UserMobileNo", DBFactoryParameterType.NVarchar, obj.UserMobileNo);
                objDBAccess.AddParameterToCommand("@ApplicationId", DBFactoryParameterType.NVarchar, obj.ApplicationId);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GrievanceModel", FunctionName = "SaveDepartmentGrievance(GrievanceDepartmentEntity Entity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataTable GetMobileNoBySSOID(string SSOID)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetMobileNoBySSOID", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOID);
                dt = objDBAccess.GetTable();
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }


        public DataTable GetGrevianceList()
        {
            DataTable dt = new DataTable();

            objDBAccess.OpenConnection();

            SqlCommand cmd = objDBAccess.CreateCommand("GetGrevienceList", DBFactoryCommandType.PROCEDURE_COMMAND);

            dt = objDBAccess.GetTable();

            objDBAccess.CloseConnection();

            return dt;
        }



    }
    public class GrivanceMaster
    {
        public int ModuleId { get; set; }
        //public int DepartmentId { get; set;}
        public string DeptEn { get; set; }
        public string DeptHi { get; set; }
        public string ModuleEn { get; set; }
        public string ModuleHi { get; set; }
        public int isActive { get; set; }
        public int isDeleted { get; set; }
        //public bool ActionId1 { get; set; } 
    }
    public class IAGrivanceLeave
    {
        public int LeaveId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int IsActive { get; set; }
        public string SSOID { get; set; }
        public int OfficeId { get; set; }
        public int RoleId { get; set; }
    }
    #endregion
    public class GrievanceEntity
    {
        public int GrievanceId { get; set; }
        public int IssueFrom { get; set; }
        public int ModuleId { get; set; }
        public int CategoryId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Attachment { get; set; }
        public string MobileNo { get; set; }
        public string SSOId { get; set; }
        // public string URL { get; set; }
        public int AssignedTo { get; set; }
        public string Remark { get; set; }
        public int UserId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Reason { get; set; }
        public string DISTRICTNameEN { get; set; }
        public string DISTRICTNameHI { get; set; }
        public string UserMobileNo { get; set; }
        public string ApplicationId { get; set; }
    }


    public class GrievanceDepartmentEntity
    {
        public int GrievanceId { get; set; }
        public int IssueFrom { get; set; }
        public int DepartmentId { get; set; }
        public int ModuleId { get; set; }
        public int SectionId { get; set; }
        public int CategoryId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Attachment { get; set; }
        public string MobileNo { get; set; }
        public string SSOId { get; set; }
        // public string URL { get; set; }
        public int AssignedTo { get; set; }
        public string Remark { get; set; }
        public int UserId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Reason { get; set; }
        public string DISTRICTNameEN { get; set; }
        public string DISTRICTNameHI { get; set; }
        public string UserMobileNo { get; set; }
        public string ApplicationId { get; set; }
    }

}