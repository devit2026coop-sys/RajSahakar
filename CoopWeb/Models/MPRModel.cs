using Cooperative.Controllers;
using Cooperative.DBConnector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;

namespace Cooperative.Models
{
    public class MPRModel
    { 
        DBLogger objDBLogger = new DBLogger();
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;
         
        public MPRModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public MPRModel()
        {
            objILogger = new DBLogger();
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable(); 
        }

        #region "MPR Module"
        
        public int SaveMPRArticle25Data(MPRArticleEntity _Entity)
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand Cmd = objDBAccess.CreateCommand("SP_SaveMPRArticle25", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, _Entity.Id);
                objDBAccess.AddParameterToCommand("@FinYearId", DBFactoryParameterType.NVarchar, _Entity.FinYearId);
                objDBAccess.AddParameterToCommand("@MonthId", DBFactoryParameterType.NVarchar, _Entity.MonthId);
                objDBAccess.AddParameterToCommand("@TsActive", DBFactoryParameterType.Int, _Entity.TsActive);
                objDBAccess.AddParameterToCommand("@TsInactive", DBFactoryParameterType.Int, _Entity.TsInactive);
                objDBAccess.AddParameterToCommand("@TsUnderLiquidation", DBFactoryParameterType.Int, _Entity.TsUnderLiquidation);
                objDBAccess.AddParameterToCommand("@PmgmActive", DBFactoryParameterType.Int, _Entity.PmgmActive);
                objDBAccess.AddParameterToCommand("@PmgmInactive", DBFactoryParameterType.Int, _Entity.PmgmInactive);
                objDBAccess.AddParameterToCommand("@PmgmUnderLiquidation", DBFactoryParameterType.Int, _Entity.PmgmUnderLiquidation);
                objDBAccess.AddParameterToCommand("@CmgmActive", DBFactoryParameterType.Int, _Entity.CmgmActive);
                objDBAccess.AddParameterToCommand("@CmgmInactive", DBFactoryParameterType.Int, _Entity.CmgmInactive);
                objDBAccess.AddParameterToCommand("@CmgmUnderLiquidation", DBFactoryParameterType.Int, _Entity.CmgmUnderLiquidation);
                objDBAccess.AddParameterToCommand("@EmgmActive", DBFactoryParameterType.Int, _Entity.EmgmActive);
                objDBAccess.AddParameterToCommand("@EmgmInactive", DBFactoryParameterType.Int, _Entity.EmgmInactive);
                objDBAccess.AddParameterToCommand("@EmgmUnderLiquidation", DBFactoryParameterType.Int, _Entity.EmgmUnderLiquidation);
                objDBAccess.AddParameterToCommand("@TmgmCount", DBFactoryParameterType.Int, _Entity.TmgmCount);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, loginuserName.UserId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, loginuserName.RoleId);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPRModel", FunctionName = "SaveMPRArticle25Data(MPRArticleEntity _Entity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public DataSet GetMPRArticle25ListDetails()
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_MPRArticle25Details", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, loginuserName.RoleId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPRModel", FunctionName = "GetMPRArticle25ListDetails" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }

        public int SaveMPRArticle25bData(MPRArticleEntity _Entity)
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand Cmd = objDBAccess.CreateCommand("SP_SaveMPRArticle25b", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, _Entity.Id);
                objDBAccess.AddParameterToCommand("@FinYearId", DBFactoryParameterType.NVarchar, _Entity.FinYearId);
                objDBAccess.AddParameterToCommand("@MonthId", DBFactoryParameterType.NVarchar, _Entity.MonthId);
                objDBAccess.AddParameterToCommand("@PyActive", DBFactoryParameterType.Int, _Entity.PyActive);
                objDBAccess.AddParameterToCommand("@PyInactive", DBFactoryParameterType.Int, _Entity.PyInactive);
                objDBAccess.AddParameterToCommand("@PyUnderLiquidation", DBFactoryParameterType.Int, _Entity.PyUnderLiquidation);
                objDBAccess.AddParameterToCommand("@CmgmActive", DBFactoryParameterType.Int, _Entity.CmgmActive);
                objDBAccess.AddParameterToCommand("@CmgmInactive", DBFactoryParameterType.Int, _Entity.CmgmInactive);
                objDBAccess.AddParameterToCommand("@CmgmUnderLiquidation", DBFactoryParameterType.Int, _Entity.CmgmUnderLiquidation);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, loginuserName.UserId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, loginuserName.RoleId);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPRModel", FunctionName = "SaveMPRArticle25bData(MPRArticleEntity _Entity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public DataSet GetMPRArticle25bListDetails()
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_MPRArticle25bDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, loginuserName.RoleId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPRModel", FunctionName = "GetMPRArticle25bListDetails" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }

        public int SaveMPRArticle30Data(MPRArticleEntity _Entity)
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand Cmd = objDBAccess.CreateCommand("SP_SaveMPRArticle30", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, _Entity.Id);
                objDBAccess.AddParameterToCommand("@FinYearId", DBFactoryParameterType.NVarchar, _Entity.FinYearId);
                objDBAccess.AddParameterToCommand("@MonthId", DBFactoryParameterType.NVarchar, _Entity.MonthId);
                objDBAccess.AddParameterToCommand("@InYearPendingCase", DBFactoryParameterType.Int, _Entity.InYearPendingCase);
                objDBAccess.AddParameterToCommand("@LastMonthCaseReceived", DBFactoryParameterType.Int, _Entity.LastMonthCaseReceived);
                objDBAccess.AddParameterToCommand("@InMonthCaseReceived", DBFactoryParameterType.Int, _Entity.InMonthCaseReceived);
                objDBAccess.AddParameterToCommand("@LastMonthCaseDisposed", DBFactoryParameterType.Int, _Entity.LastMonthCaseDisposed);
                objDBAccess.AddParameterToCommand("@InMonthCaseDisposed", DBFactoryParameterType.Int, _Entity.InMonthCaseDisposed);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, loginuserName.UserId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, loginuserName.RoleId);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPRModel", FunctionName = "SaveMPRArticle30Data(MPRArticleEntity _Entity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public DataSet GetMPRArticle30ListDetails()
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_MPRArticle30Details", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, loginuserName.RoleId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPRModel", FunctionName = "GetMPRArticle30ListDetails" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }

        public int SaveMPRArticle125Data(MPRArticleEntity _Entity)
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand Cmd = objDBAccess.CreateCommand("SP_SaveMPRArticle125", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, _Entity.Id);
                objDBAccess.AddParameterToCommand("@FinYearId", DBFactoryParameterType.NVarchar, _Entity.FinYearId);
                objDBAccess.AddParameterToCommand("@MonthId", DBFactoryParameterType.NVarchar, _Entity.MonthId);
                objDBAccess.AddParameterToCommand("@InYearPendingCase", DBFactoryParameterType.Int, _Entity.InYearPendingCase);
                objDBAccess.AddParameterToCommand("@LastMonthCaseReceived", DBFactoryParameterType.Int, _Entity.LastMonthCaseReceived);
                objDBAccess.AddParameterToCommand("@InMonthCaseReceived", DBFactoryParameterType.Int, _Entity.InMonthCaseReceived);
                objDBAccess.AddParameterToCommand("@LastMonthCaseDisposed", DBFactoryParameterType.Int, _Entity.LastMonthCaseDisposed);
                objDBAccess.AddParameterToCommand("@InMonthCaseDisposed", DBFactoryParameterType.Int, _Entity.InMonthCaseDisposed);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, loginuserName.UserId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, loginuserName.RoleId);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPRModel", FunctionName = "SaveMPRArticle125Data(MPRArticleEntity _Entity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public DataSet GetMPRArticle125ListDetails()
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_MPRArticle125Details", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, loginuserName.RoleId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPRModel", FunctionName = "GetMPRArticle125ListDetails" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }

        public int SaveMPRArticle55Data(MPRArticleEntity _Entity)
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand Cmd = objDBAccess.CreateCommand("SP_SaveMPRArticle55", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, _Entity.Id);
                objDBAccess.AddParameterToCommand("@FinYearId", DBFactoryParameterType.NVarchar, _Entity.FinYearId);
                objDBAccess.AddParameterToCommand("@MonthId", DBFactoryParameterType.NVarchar, _Entity.MonthId);
                objDBAccess.AddParameterToCommand("@InYearPendingCase", DBFactoryParameterType.Int, _Entity.InYearPendingCase);
                objDBAccess.AddParameterToCommand("@LastMonthCaseReceived", DBFactoryParameterType.Int, _Entity.LastMonthCaseReceived);
                objDBAccess.AddParameterToCommand("@InMonthCaseReceived", DBFactoryParameterType.Int, _Entity.InMonthCaseReceived);
                objDBAccess.AddParameterToCommand("@LastMonthCaseDisposed", DBFactoryParameterType.Int, _Entity.LastMonthCaseDisposed);
                objDBAccess.AddParameterToCommand("@InMonthCaseDisposed", DBFactoryParameterType.Int, _Entity.InMonthCaseDisposed);
                objDBAccess.AddParameterToCommand("@NoCaseReport", DBFactoryParameterType.Int, _Entity.NoCaseReport);
                objDBAccess.AddParameterToCommand("@NoPendingCaseReport", DBFactoryParameterType.Int, _Entity.NoPendingCaseReport);
                objDBAccess.AddParameterToCommand("@No1YearPending", DBFactoryParameterType.Int, _Entity.No1YearPending);
                objDBAccess.AddParameterToCommand("@No1to3YearPending", DBFactoryParameterType.Int, _Entity.No1to3YearPending);
                objDBAccess.AddParameterToCommand("@No3to5YearPending", DBFactoryParameterType.Int, _Entity.No3to5YearPending);
                objDBAccess.AddParameterToCommand("@No5YearPending", DBFactoryParameterType.Int, _Entity.No5YearPending);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, loginuserName.UserId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, loginuserName.RoleId);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPRModel", FunctionName = "SaveMPRArticle55Data(MPRArticleEntity _Entity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public DataSet GetMPRArticle55ListDetails()
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_MPRArticle55Details", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, loginuserName.RoleId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPRModel", FunctionName = "GetMPRArticle55ListDetails" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }

        public int SaveMPRArticle57Data(MPRArticleEntity _Entity)
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand Cmd = objDBAccess.CreateCommand("SP_SaveMPRArticle57", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, _Entity.Id);
                objDBAccess.AddParameterToCommand("@FinYearId", DBFactoryParameterType.NVarchar, _Entity.FinYearId);
                objDBAccess.AddParameterToCommand("@MonthId", DBFactoryParameterType.NVarchar, _Entity.MonthId);
                objDBAccess.AddParameterToCommand("@InYearPendingCase", DBFactoryParameterType.Int, _Entity.InYearPendingCase);
                objDBAccess.AddParameterToCommand("@LastMonthCaseReceived", DBFactoryParameterType.Int, _Entity.LastMonthCaseReceived);
                objDBAccess.AddParameterToCommand("@InMonthCaseReceived", DBFactoryParameterType.Int, _Entity.InMonthCaseReceived);
                objDBAccess.AddParameterToCommand("@LastMonthCaseDisposed", DBFactoryParameterType.Int, _Entity.LastMonthCaseDisposed);
                objDBAccess.AddParameterToCommand("@InMonthCaseDisposed", DBFactoryParameterType.Int, _Entity.InMonthCaseDisposed);
                objDBAccess.AddParameterToCommand("@NoCaseReport", DBFactoryParameterType.Int, _Entity.NoCaseReport);
                objDBAccess.AddParameterToCommand("@NoPendingCaseReport", DBFactoryParameterType.Int, _Entity.NoPendingCaseReport);
                objDBAccess.AddParameterToCommand("@No1YearPending", DBFactoryParameterType.Int, _Entity.No1YearPending);
                objDBAccess.AddParameterToCommand("@No1to3YearPending", DBFactoryParameterType.Int, _Entity.No1to3YearPending);
                objDBAccess.AddParameterToCommand("@No3to5YearPending", DBFactoryParameterType.Int, _Entity.No3to5YearPending);
                objDBAccess.AddParameterToCommand("@No5YearPending", DBFactoryParameterType.Int, _Entity.No5YearPending);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, loginuserName.UserId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, loginuserName.RoleId);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPRModel", FunctionName = "SaveMPRArticle57Data(MPRArticleEntity _Entity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public DataSet GetMPRArticle57ListDetails()
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_MPRArticle57Details", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, loginuserName.RoleId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPRModel", FunctionName = "GetMPRArticle57ListDetails" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }

        public int SaveMPRArticle57AData(MPRArticleEntity _Entity)
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand Cmd = objDBAccess.CreateCommand("SP_SaveMPRArticle57A", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, _Entity.Id);
                objDBAccess.AddParameterToCommand("@FinYearId", DBFactoryParameterType.NVarchar, _Entity.FinYearId);
                objDBAccess.AddParameterToCommand("@MonthId", DBFactoryParameterType.NVarchar, _Entity.MonthId);
                objDBAccess.AddParameterToCommand("@InYearPendingCase", DBFactoryParameterType.Int, _Entity.InYearPendingCase);
                objDBAccess.AddParameterToCommand("@InYearPendingCaseRs", DBFactoryParameterType.Float, _Entity.InYearPendingCaseRs);
                objDBAccess.AddParameterToCommand("@LastMonthCaseReceived", DBFactoryParameterType.Int, _Entity.LastMonthCaseReceived);
                objDBAccess.AddParameterToCommand("@LastMonthCaseReceivedRs", DBFactoryParameterType.Float, _Entity.LastMonthCaseReceivedRs);
                objDBAccess.AddParameterToCommand("@InMonthCaseReceived", DBFactoryParameterType.Int, _Entity.InMonthCaseReceived);
                objDBAccess.AddParameterToCommand("@InMonthCaseReceivedRs", DBFactoryParameterType.Float, _Entity.InMonthCaseReceivedRs);
                objDBAccess.AddParameterToCommand("@LastMonthCaseDisposed", DBFactoryParameterType.Int, _Entity.LastMonthCaseDisposed);
                objDBAccess.AddParameterToCommand("@LastMonthCaseDisposedRs", DBFactoryParameterType.Float, _Entity.LastMonthCaseDisposedRs);
                objDBAccess.AddParameterToCommand("@InMonthCaseDisposed", DBFactoryParameterType.Int, _Entity.InMonthCaseDisposed);
                objDBAccess.AddParameterToCommand("@InMonthCaseDisposedRs", DBFactoryParameterType.Float, _Entity.InMonthCaseDisposedRs);
                objDBAccess.AddParameterToCommand("@No1YearPending", DBFactoryParameterType.Int, _Entity.No1YearPending);
                objDBAccess.AddParameterToCommand("@No1to3YearPending", DBFactoryParameterType.Int, _Entity.No1to3YearPending);
                objDBAccess.AddParameterToCommand("@No3to5YearPending", DBFactoryParameterType.Int, _Entity.No3to5YearPending);
                objDBAccess.AddParameterToCommand("@No5YearPending", DBFactoryParameterType.Int, _Entity.No5YearPending);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, loginuserName.UserId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, loginuserName.RoleId);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPRModel", FunctionName = "SaveMPRArticle57AData(MPRArticleEntity _Entity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public DataSet GetMPRArticle57AListDetails()
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_MPRArticle57ADetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, loginuserName.RoleId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPRModel", FunctionName = "GetMPRArticle57AListDetails" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }

        public int SaveMPRArticle57RData(MPRArticleEntity _Entity)
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand Cmd = objDBAccess.CreateCommand("SP_SaveMPRArticle57R", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, _Entity.Id);
                objDBAccess.AddParameterToCommand("@FinYearId", DBFactoryParameterType.NVarchar, _Entity.FinYearId);
                objDBAccess.AddParameterToCommand("@MonthId", DBFactoryParameterType.NVarchar, _Entity.MonthId);
                objDBAccess.AddParameterToCommand("@InYearPendingCase", DBFactoryParameterType.Int, _Entity.InYearPendingCase);
                objDBAccess.AddParameterToCommand("@InYearPendingCaseRs", DBFactoryParameterType.Float, _Entity.InYearPendingCaseRs);
                objDBAccess.AddParameterToCommand("@LastMonthCaseReceived", DBFactoryParameterType.Int, _Entity.LastMonthCaseReceived);
                objDBAccess.AddParameterToCommand("@LastMonthCaseReceivedRs", DBFactoryParameterType.Float, _Entity.LastMonthCaseReceivedRs);
                objDBAccess.AddParameterToCommand("@InMonthCaseReceived", DBFactoryParameterType.Int, _Entity.InMonthCaseReceived);
                objDBAccess.AddParameterToCommand("@InMonthCaseReceivedRs", DBFactoryParameterType.Float, _Entity.InMonthCaseReceivedRs);
                objDBAccess.AddParameterToCommand("@LastMonthCaseDisposed", DBFactoryParameterType.Int, _Entity.LastMonthCaseDisposed);
                objDBAccess.AddParameterToCommand("@LastMonthCaseDisposedRs", DBFactoryParameterType.Float, _Entity.LastMonthCaseDisposedRs);
                objDBAccess.AddParameterToCommand("@InMonthCaseDisposed", DBFactoryParameterType.Int, _Entity.InMonthCaseDisposed);
                objDBAccess.AddParameterToCommand("@InMonthCaseDisposedRs", DBFactoryParameterType.Float, _Entity.InMonthCaseDisposedRs);
                objDBAccess.AddParameterToCommand("@No1YearPending", DBFactoryParameterType.Int, _Entity.No1YearPending);
                objDBAccess.AddParameterToCommand("@No1to3YearPending", DBFactoryParameterType.Int, _Entity.No1to3YearPending);
                objDBAccess.AddParameterToCommand("@No3to5YearPending", DBFactoryParameterType.Int, _Entity.No3to5YearPending);
                objDBAccess.AddParameterToCommand("@No5YearPending", DBFactoryParameterType.Int, _Entity.No5YearPending);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, loginuserName.UserId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, loginuserName.RoleId);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPRModel", FunctionName = "SaveMPRArticle57RData(MPRArticleEntity _Entity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public DataSet GetMPRArticle57RListDetails()
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_MPRArticle57RDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, loginuserName.RoleId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPRModel", FunctionName = "GetMPRArticle57RListDetails" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }

        public int SaveMPRArticle61Data(MPRArticleEntity _Entity)
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand Cmd = objDBAccess.CreateCommand("SP_SaveMPRArticle61", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, _Entity.Id);
                objDBAccess.AddParameterToCommand("@FinYearId", DBFactoryParameterType.NVarchar, _Entity.FinYearId);
                objDBAccess.AddParameterToCommand("@MonthId", DBFactoryParameterType.NVarchar, _Entity.MonthId);
                objDBAccess.AddParameterToCommand("@InYearPendingCase", DBFactoryParameterType.Int, _Entity.InYearPendingCase);
                objDBAccess.AddParameterToCommand("@LastMonthCaseReceived", DBFactoryParameterType.Int, _Entity.LastMonthCaseReceived);
                objDBAccess.AddParameterToCommand("@InMonthCaseReceived", DBFactoryParameterType.Int, _Entity.InMonthCaseReceived);
                objDBAccess.AddParameterToCommand("@LastMonthCaseDisposed", DBFactoryParameterType.Int, _Entity.LastMonthCaseDisposed);
                objDBAccess.AddParameterToCommand("@InMonthCaseDisposed", DBFactoryParameterType.Int, _Entity.InMonthCaseDisposed);
                objDBAccess.AddParameterToCommand("@No1YearPending", DBFactoryParameterType.Int, _Entity.No1YearPending);
                objDBAccess.AddParameterToCommand("@No1to3YearPending", DBFactoryParameterType.Int, _Entity.No1to3YearPending);
                objDBAccess.AddParameterToCommand("@No3to5YearPending", DBFactoryParameterType.Int, _Entity.No3to5YearPending);
                objDBAccess.AddParameterToCommand("@No5YearPending", DBFactoryParameterType.Int, _Entity.No5YearPending);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, loginuserName.UserId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, loginuserName.RoleId);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPRModel", FunctionName = "SaveMPRArticle61Data(MPRArticleEntity _Entity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public DataSet GetMPRArticle61ListDetails()
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_MPRArticle61Details", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, loginuserName.RoleId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPRModel", FunctionName = "GetMPRArticle61ListDetails" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }

        public DataSet GetMPRArticle25Report()
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_MPRArticle25DetailList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId); 
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPRModel", FunctionName = "GetMPRArticle25Report" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }

        public DataSet GetMPRArticle125Report()
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_MPRArticle125DetailList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPRModel", FunctionName = "GetMPRArticle125Report" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }

        public DataSet GetMPRArticle25bReport()
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_MPRArticle25BDetailList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPRModel", FunctionName = "GetMPRArticle25bReport" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }

        public DataSet GetMPRArticle30Report()
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_MPRArticle30DetailList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPRModel", FunctionName = "GetMPRArticle30ListDetails" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }

        public DataSet GetMPRArticle55Report()
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_MPRArticle55DetailList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPRModel", FunctionName = "GetMPRArticle55Report" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }

        public DataSet GetMPRArticle57Report()
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_MPRArticle57DetailList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPRModel", FunctionName = "GetMPRArticle57Report" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }

        public DataSet GetMPRArticle57AReport()
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_MPRArticle57ADetailList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPRModel", FunctionName = "GetMPRArticle57AReport" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }

        public DataSet GetMPRArticle57RReport()
        {
            UserEntity _loggedUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_MPRArticle57RDetailList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _loggedUser.DistrictId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPRModel", FunctionName = "GetMPRArticle57RReport" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }

        public DataSet GetMPRArticle61Report()
        {
            UserEntity loginuserName = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Get_MPRArticle61DetailList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, loginuserName.DistrictId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPRModel", FunctionName = "GetMPRArticle61Report" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }

        #endregion

        #region "Reports"

        public DataTable GetAnswerofQuestionForGraph(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetChartData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "GetAnswerofQuestionForGraph(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable GetComplaintReport(ref DataPagingModel Details, ref string TotalAmount, ref string TotalCSS, int DistrictId, string SocietyId = "0", int SearchType = 1)
        {
            dt.Clear();
            SqlCommand cmd = new SqlCommand();
            objDBAccess.OpenConnection();
            try
            {
                string searchText = "";
                if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                {
                    searchText = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetComplaintReportNew", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                    objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyId);
                    objDBAccess.AddParameterToCommand("@SearchType", DBFactoryParameterType.Int, SearchType);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, true);
                    objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, searchText);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetComplaintReportNew", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyId);
                objDBAccess.AddParameterToCommand("@SearchType", DBFactoryParameterType.Int, SearchType);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, false);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, searchText);
                dt = objDBAccess.GetTable();
                if (SocietyId == "0")
                {
                    TotalAmount = dt.Compute("Sum(toamt)", string.Empty).ToString(); ;
                    TotalCSS = dt.Compute("Sum(Total)", string.Empty).ToString(); ;
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable  GetComplaintReportData( int DistrictId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }

        public DataTable GetOICDashbordData(int DistrictId, int ActId)
        {
            dt.Clear();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("OICDashbordData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, ActId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = " GetOICDashbordData(int DistrictId, int ActId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }


        public DataTable GetInspectionReportData(int FinancialYearId, int ActId, int DistrictId, int FilterId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAuditInspectionReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, FinancialYearId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, ActId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@FilterId", DBFactoryParameterType.Int, FilterId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable GetInspectionReportData(int FinancialYearId, int ActId,int DistrictId,int FilterId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }

        #endregion

        #region Get CCSComplaint Report
         
        public DataTable GetActionIdCCSComplaintReportCount(int ActionId, string WhereClause)
        {
            DataTable ds = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetCCSComplaintReportCount", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                ds = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable  GetActionIdCCSComplaintReport(int ActionId, string WhereClause)" }, ex);
            }
            return ds;
        }

        public DataTable GetCCSComplaintListSocieties(int ActionId, string WhereClause)
        {
            DataTable ds = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetCCSComplaintListSocieties", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereClause);
                ds = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable  GetCCSComplaintListSocieties(int ActionId, string WhereClause)" }, ex);
            }
            return ds;
        }

        public DataTable GetActionIdCCSComplaintReport(ref DataPagingModel Details, int ActionId, string WhereClause)
        {
            DataTable ds = new DataTable();
            try
            {


                var CCSId = Details.SearchParameter.Where(x => x.Key == "CCSId").Select(x => x.Value).FirstOrDefault();
                var SearchText = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault();

                if (CCSId != null)
                {
                    WhereClause += " and CCSId=" + CCSId;
                }

                if (SearchText != null)
                {
                    WhereClause += " and ComplainerName like '%" + SearchText + "%'" + "or Address like '%" + SearchText + "%'" + "or MobileNo like'%" + SearchText + "%'" + "or AmountDeposited like'%" + SearchText + "%'";
                }

                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetCCSComplaintList", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, ActionId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereClause);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetCCSComplaintList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, ActionId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereClause);
                ds = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable  GetActionIdCCSComplaintReport(int ActionId, string WhereClause)" }, ex);
            }
            return ds;
        }
         
        public DataTable GetCCSComplaintStatusReportDetails(ref DataPagingModel Details, int ActionId, string WhereClause, int MultiState, int DistrictId)
        {
            DataTable ds = new DataTable();
            try
            {

                WhereClause = "";
                var CCSId = Details.SearchParameter.Where(x => x.Key == "CCSId").Select(x => x.Value).FirstOrDefault();
                var SearchText = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault();

                if (CCSId != null)
                {
                    WhereClause += " and CCSId=" + CCSId;
                }

                //if (SearchText != null)
                //{
                //    WhereClause += " and ComplainerName like '%" + SearchText + "%'" + "or Address like '%" + SearchText + "%'" + "or MobileNo like'%" + SearchText + "%'" + "or AmountDeposited like'%" + SearchText + "%'";
                //}

                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetCCSComplaintStatusReportDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, ActionId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereClause);
                    objDBAccess.AddParameterToCommand("@MultiState", DBFactoryParameterType.Int, MultiState);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                    objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, SearchText);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetCCSComplaintStatusReportDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, ActionId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereClause);
                objDBAccess.AddParameterToCommand("@MultiState", DBFactoryParameterType.Int, MultiState);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, SearchText);
                ds = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable  GetCCSComplaintStatusReportDetails(int ActionId, string WhereClause)" }, ex);
            }
            return ds;
        }
         
        public DataSet GetComplaintReportData(int DistrictId, string SocietyId, int SearchType)
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetComplaintReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyId);
                objDBAccess.AddParameterToCommand("@SearchType", DBFactoryParameterType.Int, SearchType);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable  GetComplaintReportData( int DistrictId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return ds;
        }
         
        #endregion

        #region Code Written By Jitendra Start Date 18-July-2019

        public string InsertAuditReportDashboardByExcel(AuditReportDashboardEntity obj)
        {
            string Result = "-1";
            string ExcelUploadId = "";
            string connString = "";
            try
            {
                string conString = string.Empty;
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertAuditReportDashboardExcelDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ExcelSheetName", DBFactoryParameterType.NVarchar, obj.ExcelUrl);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, obj.CreatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                Result = ExcelUploadId = (objDBAccess.ExecuteScalarCommand().ToString());

                string strFileType = System.IO.Path.GetExtension(obj.ExcelUrl).ToLower();
                if (1 > 0)
                {
                    string path = string.Concat(System.Web.HttpContext.Current.Server.MapPath("~/Content/AuditReportExcelForDashboard/" + obj.ExcelUrl));

                    if (strFileType.Trim() == ".xls")
                    {
                        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    else if (strFileType.Trim() == ".xlsx")
                    {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    OleDbConnection conn = new OleDbConnection(connString);

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    int ResultInner = 0; int Suc = 0, Unsuc = 0, Dupli = 0;
                    foreach (DataRow SheetRow in dtSheet.Rows)
                    {
                        string query = "SELECT * FROM [" + SheetRow.ItemArray[2] + "]";
                        OleDbCommand cmd2 = new OleDbCommand(query, conn);
                        OleDbDataAdapter da = new OleDbDataAdapter(cmd2);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dt.Columns.Add("ExcelUploadId", typeof(System.String));

                        try
                        {
                            foreach (DataRow Dr in dt.Rows)
                            {
                                int YearId = obj.YearId;
                                int MonthId = obj.MonthId;
                                int DistrictId = obj.DistrictId;
                                string DivisionName = "";// Dr["संभाग"].ToString().Trim();
                                string DistrictName = Dr["नाम इकाई"].ToString().Trim();
                                string AuditableSocieties = Dr["आडिट योग्य समितियाँ"].ToString().Trim();
                                string AuditedSocietiesTillLastMonth = Dr["गत माह तक आडिटेड समितियाँ"].ToString().Trim();
                                string AuditedSocietiesCurrentMonth = Dr["माह में आडिटेड समितियाँ"].ToString().Trim();

                                SqlCommand cmd1 = objDBAccess.CreateCommand("InsertAuditReportForDashboardByExcelSheet", DBFactoryCommandType.PROCEDURE_COMMAND);

                                cmd1.Parameters.AddWithValue("@YearId", YearId);
                                cmd1.Parameters.AddWithValue("@MonthId", MonthId);
                                cmd1.Parameters.AddWithValue("@DivisionName", DivisionName);
                                cmd1.Parameters.AddWithValue("@DistrictId", DistrictId);
                                cmd1.Parameters.AddWithValue("@DistrictName", DistrictName);
                                cmd1.Parameters.AddWithValue("@AuditableSocieties", AuditableSocieties);
                                cmd1.Parameters.AddWithValue("@AuditedSocietiesTillLastMonth", AuditedSocietiesTillLastMonth);
                                cmd1.Parameters.AddWithValue("@AuditedSocietiesCurrentMonth", AuditedSocietiesCurrentMonth);
                                cmd1.Parameters.AddWithValue("@ExcelUploadId", ExcelUploadId);
                                cmd1.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                                cmd1.Parameters.AddWithValue("@CreatedIP", COMMON_FUNCTION.GetIpAddress());
                                cmd1.Parameters.AddWithValue("@SSOID", obj.SSOID);
                                ResultInner = Convert.ToInt32(objDBAccess.ExecuteScalarCommand().ToString());

                                if (ResultInner > 0)
                                    Suc = Suc + 1;
                                else if (ResultInner == -2)
                                    Dupli = Dupli + 1;
                                else if (ResultInner == -1)
                                    Unsuc = Unsuc + 1;
                            }

                            Result = "Successfully Saved=" + Suc.ToString() + ".  Duplicate Entry=" + Dupli.ToString() + ".  Error  Occurred=" + Unsuc.ToString();
                        }
                        catch (Exception ex)
                        {
                            objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "InsertSocityByExcel(SocietyRegistrationEntity obj)" }, ex);
                            Result = ex.Message;
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public string InsertAuditReportDashboardByExcel(AuditReportDashboardEntity obj)" }, ex);
                Result = ex.Message;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetAuditReportDashboardExcelDetails(long userId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAuditReportDashboardExcelDataDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, userId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable GetAuditReportDashboardExcelDetails(long userId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetExlAuditReportDashboardDetails(int ExlId, int StatusId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetExcelAuditReportDashboardDetailsByExcelUploadId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ExcelUploadId", DBFactoryParameterType.Int, ExlId);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable GetExlAuditReportDashboardDetails(int ExlId, int StatusId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public string InsertAuditReportHeadWiseDashboardByExcel(AuditReportDashboardEntity obj)
        {
            string Result = "-1";
            string ExcelUploadId = "";
            string connString = "";
            try
            {
                string conString = string.Empty;
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertAuditReportDashboardHeadWiseExcelDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ExcelSheetName", DBFactoryParameterType.NVarchar, obj.ExcelUrl);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, obj.CreatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                Result = ExcelUploadId = (objDBAccess.ExecuteScalarCommand().ToString());

                string strFileType = System.IO.Path.GetExtension(obj.ExcelUrl).ToLower();
                if (1 > 0)
                {
                    string path = string.Concat(System.Web.HttpContext.Current.Server.MapPath("~/Content/AuditReportExcelForDashboard/" + obj.ExcelUrl));

                    if (strFileType.Trim() == ".xls")
                    {
                        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    else if (strFileType.Trim() == ".xlsx")
                    {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    OleDbConnection conn = new OleDbConnection(connString);

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    int ResultInner = 0; int Suc = 0, Unsuc = 0, Dupli = 0;
                    foreach (DataRow SheetRow in dtSheet.Rows)
                    {
                        string query = "SELECT * FROM [" + SheetRow.ItemArray[2] + "]";
                        OleDbCommand cmd2 = new OleDbCommand(query, conn);
                        OleDbDataAdapter da = new OleDbDataAdapter(cmd2);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dt.Columns.Add("ExcelUploadId", typeof(System.String));

                        //  objDBAccess.OpenConnection();
                        try
                        {
                            foreach (DataRow Dr in dt.Rows)
                            {
                                int YearId = obj.YearId;
                                int MonthId = obj.MonthId;
                                int DistrictId = obj.DistrictId;

                                string DistrictName = Dr["नाम खण्ड/इकाई"].ToString().Trim();
                                string TypeName = Dr["प्रकार"].ToString().Trim();// Dr["नाम खण्ड/इकाई"].ToString().Trim();
                                string Apex = Dr["अपेक्स"].ToString().Trim();
                                string CCB = Dr["सीसीबी"].ToString().Trim();
                                string MilkFederatrion = Dr["दुग्ध संघ"].ToString().Trim();

                                string PLDB = Dr["पीएलडीबी"].ToString().Trim();
                                string UrbanBank = Dr["अरबन बैंक"].ToString().Trim();
                                string WholeSale = Dr["होलसेल भंडार"].ToString().Trim();
                                string BuySell = Dr["क्रय विक्रय"].ToString().Trim();


                                string DistrictUnion = Dr["जिला स संघ"].ToString().Trim();
                                string HouseDevelopment = Dr["गृह निर्माण"].ToString().Trim();
                                string Village = Dr["ग्राम सेवा"].ToString().Trim();
                                string PrimaryDairy = Dr["प्राथ डेयरी"].ToString().Trim();


                                string OilSeed = Dr["तिलहन"].ToString().Trim();
                                string Salary = Dr["सैलरी"].ToString().Trim();
                                string Credit = Dr["क्रेडिट"].ToString().Trim();
                                string Women = Dr["महिला"].ToString().Trim();


                                string PrimaryStorage = Dr["प्राथ भंडार"].ToString().Trim();
                                string Industrial = Dr["औधोगिक"].ToString().Trim();
                                string Khadi = Dr["खादी"].ToString().Trim();
                                string Bunkar = Dr["बुनकर"].ToString().Trim();


                                string Agriculture = Dr["कृषि"].ToString().Trim();
                                string MultiplePurpose = Dr["बहु उद्देशीय"].ToString().Trim();
                                string Other = Dr["अन्य"].ToString().Trim();
                                string GrandTotal = "0"; //Dr["कुल योग"].ToString().Trim();

                                int _typeId = TypeName == "कुल समितियाँ" ? 1 : (TypeName == "माह तक आडिटेड समितियाँ" ? 2 : (TypeName == "माह के पश्चात आडिट से शेष समितियाँ" ? 3 : -1));

                                SqlCommand cmd1 = objDBAccess.CreateCommand("InsertAuditReportForDashboardHeadWiseByExcelSheet", DBFactoryCommandType.PROCEDURE_COMMAND);

                                cmd1.Parameters.AddWithValue("@YearId", YearId);
                                cmd1.Parameters.AddWithValue("@MonthId", MonthId);
                                cmd1.Parameters.AddWithValue("@DistrictId", DistrictId);
                                cmd1.Parameters.AddWithValue("@DistrictName", DistrictName);
                                cmd1.Parameters.AddWithValue("@Apex", Apex);
                                cmd1.Parameters.AddWithValue("@CCB", CCB);
                                cmd1.Parameters.AddWithValue("@MilkFederation", MilkFederatrion);
                                cmd1.Parameters.AddWithValue("@PLDB", PLDB);
                                cmd1.Parameters.AddWithValue("@Urban", UrbanBank);
                                cmd1.Parameters.AddWithValue("@WholeSale", WholeSale);
                                cmd1.Parameters.AddWithValue("@BuySell", BuySell);
                                cmd1.Parameters.AddWithValue("@DistrictUnion", DistrictUnion);
                                cmd1.Parameters.AddWithValue("@HouseDevelopment", HouseDevelopment);
                                cmd1.Parameters.AddWithValue("@Village", Village);
                                cmd1.Parameters.AddWithValue("@PrimaryDairy", PrimaryDairy);
                                cmd1.Parameters.AddWithValue("@OilSeed", OilSeed);
                                cmd1.Parameters.AddWithValue("@Salary", Salary);
                                cmd1.Parameters.AddWithValue("@Credit", Credit);
                                cmd1.Parameters.AddWithValue("@Women", Women);
                                cmd1.Parameters.AddWithValue("@PrimaryStorage", PrimaryStorage);
                                cmd1.Parameters.AddWithValue("@Industrial", Industrial);
                                cmd1.Parameters.AddWithValue("@Khadi", Khadi);
                                cmd1.Parameters.AddWithValue("@Bunkar", Bunkar);
                                cmd1.Parameters.AddWithValue("@Agriculture", Agriculture);
                                cmd1.Parameters.AddWithValue("@Multipurpose", MultiplePurpose);
                                cmd1.Parameters.AddWithValue("@Others", Other);
                                cmd1.Parameters.AddWithValue("@ExcelUploadId", ExcelUploadId);
                                cmd1.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                                cmd1.Parameters.AddWithValue("@CreatedIP", COMMON_FUNCTION.GetIpAddress());
                                cmd1.Parameters.AddWithValue("@SSOID", obj.SSOID);
                                cmd1.Parameters.AddWithValue("@Total", GrandTotal);
                                cmd1.Parameters.AddWithValue("@TypeId", _typeId);
                                ResultInner = Convert.ToInt32(objDBAccess.ExecuteScalarCommand().ToString());

                                if (ResultInner > 0)
                                    Suc = Suc + 1;
                                else if (ResultInner == -2)
                                    Dupli = Dupli + 1;
                                else if (ResultInner == -1)
                                    Unsuc = Unsuc + 1;

                            }

                            Result = "Successfully Saved=" + Suc.ToString() + ".  Duplicate Entry=" + Dupli.ToString() + ".  Error  Occurred=" + Unsuc.ToString();
                        }
                        catch (Exception ex)
                        {
                            objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public string InsertAuditReportHeadWiseDashboardByExcel(AuditReportDashboardEntity obj)" }, ex);
                            Result = ex.Message;
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public string InsertAuditReportHeadWiseDashboardByExcel(AuditReportDashboardEntity obj)" }, ex);
                Result = ex.Message;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetAuditReportHeadWiseDashboardExcelDetails(long userId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAuditReportDashboardHeadWiseExcelDataDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, userId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable GetAuditReportHeadWiseDashboardExcelDetails(long userId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetExlAuditReportHeadWiseDashboardDetails(int ExlId, int StatusId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetExcelAuditReportDashboardHeadWiseDetailsByExcelUploadId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ExcelUploadId", DBFactoryParameterType.Int, ExlId);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable GetExlAuditReportHeadWiseDashboardDetails(int ExlId, int StatusId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetAuditReportRegionWise(int YearId, int MonthId, int Type)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAuditReportRegionWise", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Year", DBFactoryParameterType.Int, YearId);
                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, MonthId);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.Int, Type);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable GetAuditReportRegionWise(int YearId, int MonthId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable PACSReport(int LevelId, int DistrictId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("PACSReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, LevelId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable PACSReport(int LevelId, int DistrictId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable FertilizerReportData(int DistrictId, int FertilizerType, string dateOn, int RetailerNatureId, int ReportType)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("FertilizerOpeningStockReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@OnDate", DBFactoryParameterType.VARCHAR, dateOn);
                objDBAccess.AddParameterToCommand("@IsReport", DBFactoryParameterType.Int, ReportType);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@FertilizerType", DBFactoryParameterType.Int, FertilizerType);
                objDBAccess.AddParameterToCommand("@NatureId", DBFactoryParameterType.Int, RetailerNatureId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable FertilizerReportData(int DistrictId, int FertilizerType,string dateOn,int RetailerNatureId,int ReportType)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        /*getReport*/
        public DataTable GodownFilterData(string key, int DistrictId, int StatusId = 0)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("districtwiseGodown", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@key", DBFactoryParameterType.NVarchar, key);
                objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.NVarchar, DistrictId);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.NVarchar, StatusId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable FertilizerReportData(int DistrictId, int FertilizerType,string dateOn,int RetailerNatureId,int ReportType)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable GodownFilterDistrictData(string key, int DistrictId, int StatusId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("districtwiseGodown", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@key", DBFactoryParameterType.NVarchar, key);
                objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable FertilizerReportData(int DistrictId, int FertilizerType,string dateOn,int RetailerNatureId,int ReportType)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }



        public DataTable RetailerTransactionYearMonthList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("RetailerTransactionYearMonthList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable RetailerTransactionYearMonthList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable FertilizerRetailerTransactionReportData(int DistrictId, string YearId, int Month, int NatureId, int ReportType)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("FertilizerRetailerTransactionReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.Int, ReportType);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@YearId", DBFactoryParameterType.VARCHAR, YearId);
                objDBAccess.AddParameterToCommand("@MonthId", DBFactoryParameterType.Int, Month);
                objDBAccess.AddParameterToCommand("@NatureId", DBFactoryParameterType.Int, NatureId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable FertilizerRetailerTransactionReportData(int DistrictId, string YearId, int Month,int NatureId,int ReportType)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetRSWC_MISReportData123(string WhereClause)
        {
            DataTable resultDt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("RajfedGetWarehouseRSWCList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, "0");
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereClause);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable GetRSWC_MISReportData(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }


        #endregion

        #region "Reports Other"
        public DataSet GetWarehouseManagerDashboardData(int WarehouseId)
        {
            DataSet ds = new DataSet();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("WR_Dashboard", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WarehouseId", DBFactoryParameterType.Int, WarehouseId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, _LoggedInUser.DivisionId);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataSet GetWarehouseManagerDashboardData(int WarehouseId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return ds;
        }

        public DataTable GetWarehouseManagerDashboardData(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("WR_CommodityWiseDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataTable GetWarehouseManagerDashboardData(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }
        // public DataSet GetSocietyDistWiseData(string ActId, int SocietyTypeId, string CreatetionDate, int LevelId = 1, int DistrictId = 0, int DivisionId = 0, int IsOnline = -1)

        public DataSet GetSocietyDistWiseData(int ActId, int SocietyTypeId, long UserRoleDeptMappingId, int LevelId = 1, int DistrictId = 0, int DivisionId = 0, int IsOnline = -1)
        {
            //DataSet ds = new DataSet();
            //try
            //{
            //    objDBAccess.OpenConnection();
            //    SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyDistWiseData", DBFactoryCommandType.PROCEDURE_COMMAND);
            //    objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.VARCHAR, ActId);
            //    objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, SocietyTypeId);
            //    objDBAccess.AddParameterToCommand("@CreatetionDate", DBFactoryParameterType.VARCHAR, CreatetionDate);
            //    objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, LevelId);
            //    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
            //    objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);

            //    ds = objDBAccess.GetDataSet();
            //}
            //catch (Exception ex)
            //{
            //    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable  GetSocietyCombineData(int ActId, int SocietyTypeId)" }, ex);
            //}
            //finally
            //{
            //    objDBAccess.CloseConnection();
            //    dt.Dispose();
            //}
            //return ds;

            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyDistWiseData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, ActId);
                objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, SocietyTypeId);

                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, LevelId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@IsOnline", DBFactoryParameterType.Int, IsOnline);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable  GetSocietyCombineData(int ActId, int SocietyTypeId,,int LevelId = 1,  int DistrictId =0, int DivisionId=0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return ds;

        }


        public DataTable GetDistrictWiseSocietyReport(int ActId, int SocietyTypeId, long UserRoleDeptMappingId, int LevelId = 1, int DistrictId = 0, int DivisionId = 0, int IsOnline = -1)
        {

            DataTable dt = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetDistrictWiseSocietyReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, ActId);
                objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, SocietyTypeId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, LevelId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@IsOnline", DBFactoryParameterType.Int, IsOnline);
                dt = objDBAccess.GetTable();


                //TotalAmount = dt.Compute("Sum(toamt)", string.Empty).ToString(); ;
                //TotalCSS = dt.Compute("Sum(Total)", string.Empty).ToString(); ;


            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable  GetSocietyCombineData(int ActId, int SocietyTypeId,,int LevelId = 1,  int DistrictId =0, int DivisionId=0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;

        }

        public DataTable GetSocietyReportData(ref DataPagingModel Details, int StatusId, long UserRoleDeptMappingId, int ActId, int SocietyTypeId, int DistrictId = 0, int IsOnline = -1)
        {
            DataTable ds = new DataTable();
            try
            {
                string whereCase = "";
                var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                string whereDate = @" and   convert(date,A.ActionDate,111) ";
                if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                {
                    string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                    string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                    whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                }
                else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                {
                    string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                    string tDate = fDate;
                    whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                }
                else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                {
                    string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                    string fDate = tDate;
                    whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                }

                //if (IsOnline > 0)
                //{
                //    whereCase += " and isnull(so.IsNewRegistration,0)=" + IsOnline;

                //}

                if (DistrictId > 0)
                {
                    whereCase += " and ROD.DISTRICT_ID=" + DistrictId;

                }
                if (ActId > 0)
                {
                    whereCase += " and so.ActId=" + ActId;

                }
                if (SocietyTypeId > 0)
                {
                    whereCase += " and so.SocietyTypeId=" + SocietyTypeId;

                }

                if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                {
                    string searchText = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault();
                    whereCase += " and ( so.RegistrationNo like '%" + searchText + "%' or so.SocietyName like '%" + searchText + "%' or  ProceesStatusTextEn like '%" + searchText + "%' or ProceesStatusTextHi like '%" + searchText + "%' or act.ActNameEn_Short like '%" + searchText + "%' or act.ActNameHi_Short like '%" + searchText + "%'  or SocietyType.TypeameEng like '%" + searchText + "%' or SocietyType.TypeNameHin like '%" + searchText + "%' or District.DISTRICT_ENG like '%" + searchText + "%' or District.DISTRICT_MANGAL like '%" + searchText + "%')";
                }
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetSocietyReportData", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);
                    objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@IsNewRegistration", DBFactoryParameterType.Int, IsOnline);
                    objDBAccess.AddParameterToCommand("@WhareCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetSocietyReportData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@IsNewRegistration", DBFactoryParameterType.Int, IsOnline);
                objDBAccess.AddParameterToCommand("@WhareCase", DBFactoryParameterType.NVarchar, whereCase);
                ds = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable  GetSocietyCombineData(int ActId, int SocietyTypeId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return ds;
        }

        public DataSet GetWarehouseList(string WhereClause)
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetWarehouseList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "public DataSet GetWarehouseList(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return ds;
        }

        public DataTable GetWarehouseRSWCList(string WhereClause)
        {
            dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();

                cmd = objDBAccess.CreateCommand("RajfedGetWarehouseRSWCList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Wharehouse_Id", DBFactoryParameterType.NVarchar, 1);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, 100000);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, 1);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable GetWarehouseRSWCList(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;

        }

        public DataTable GetWRROReport(ref DataPagingModel Details, string ReportType, int DivisionId)
        {
            string qry = "";
            DataTable resultDt = new DataTable();
            try
            {
                string whereCase = " ";

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();


                    var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                    var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                    string whereDate = @" and   convert(date,created_date,111) ";
                    if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                    {
                        string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                        string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                        whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                    }
                    else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                    {
                        string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                        string tDate = fDate;
                        whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                    }
                    else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                    {
                        string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                        string fDate = tDate;
                        whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                    }

                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate"))
                    {
                        string key = x.Key;
                        if (x.Key == "fy")
                        {
                            key = "case when cast(rs.valid_from_date as date)>cast(cast(year(rs.valid_from_date) as varchar(5))+'-03-31' as date) and cast(rs.valid_from_date as date)<cast(cast(year(rs.valid_from_date)+1 as varchar(5))+'-04-01' as date)		then cast( year(rs.valid_from_date) as varchar(5))+'-'+right(cast( year(rs.valid_from_date)+1 as varchar(5)),2) else cast(year(rs.valid_from_date)-1 as varchar(5))+'-'+right(cast(year(rs.valid_from_date) as varchar(5)),2) end";
                        }
                        if (x.Key == "DISTRICT_ID")
                        {

                            whereCase += " and ( dst.DISTRICT_ID='" + x.Value + "'" + " or dst1.DISTRICT_ID='" + x.Value + "') ";
                        }

                        if (x.Key != "DISTRICT_ID")
                            whereCase += " and " + key + "='" + x.Value.ToString() + "'";
                    }
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedWRROReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.NVarchar, DivisionId);
                    objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.NVarchar, ReportType);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    qry += objDBAccess.getSqlCommandText();
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedWRROReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.NVarchar, DivisionId);
                objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.NVarchar, ReportType);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                qry += objDBAccess.getSqlCommandText();
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable GetWRROReport(ref DataPagingModel Details, string ReportType)" + qry }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetWRReport(ref DataPagingModel Details, string ReportType)
        {

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            DataTable resultDt = new DataTable();
            try
            {
                string whereCase = " ";



                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    if (Details.SearchParameter.Where(x => x.Key == "commoditytype").Any())
                    {
                        string val = Details.SearchParameter.Where(x => x.Key == "commoditytype").Select(x => x.Value).FirstOrDefault();
                        whereCase += " and IsKharif=" + val;
                    }

                    var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                    var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                    string whereDate = @" and   convert(date,created_date,111) ";
                    if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                    {
                        string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                        string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                        whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                    }
                    else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                    {
                        string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                        string tDate = fDate;
                        whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                    }
                    else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                    {
                        string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                        string fDate = tDate;
                        whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                    }

                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate" && x.Key != "commoditytype"))
                    {
                        string key = x.Key;
                        if (x.Key == "fy")
                        {
                            key = "cast(FIN_YEAR_START as varchar(5))+'-'+cast(right(FIN_YEAR_END,2) as varchar(5))";
                        }

                        whereCase += " and " + key + "='" + x.Value + "'";
                    }
                }
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("RajfedWRReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.NVarchar, ReportType);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("RajfedWRReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ReportType", DBFactoryParameterType.NVarchar, ReportType);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);

                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable GetWRReport(ref DataPagingModel Details, string ReportType)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }
        
        #endregion

    }
}