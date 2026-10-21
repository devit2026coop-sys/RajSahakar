using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cooperative.Controllers;
using Newtonsoft.Json;
using CoopDAL;
using System.Web.Security;

namespace Cooperative.Models
{
    public class DashboardModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public DashboardModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public DashboardModel()
        {
            objDBAccess = new DBFactoryMSSQL();


        }

        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        public DataSet GetDashboardData(int FinancialYear = 0, int DistrictId = 0, int DivisionId = 0, int LevelId = 0, int ActId = 0, string CreatetionDate = "", int SocietyTypeId = 0, long UserRoleDeptMappingId = 0, int IsOnline = -1, string actionType = "", int type = 0)
        {
            try
            {
                DataSet ds = new DataSet();
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_GetDashboardByFinYear", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FinancialYear", DBFactoryParameterType.NVarchar, FinancialYear);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, LevelId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, ActId);
                objDBAccess.AddParameterToCommand("@CreatetionDate", DBFactoryParameterType.NVarchar, CreatetionDate);
                objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, SocietyTypeId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@IsOnline", DBFactoryParameterType.Int, IsOnline);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, actionType);
                objDBAccess.AddParameterToCommand("@type", DBFactoryParameterType.NVarchar, type);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.DashboardModel",
                    FunctionName = "GetDashboardData(int FinancialYear :" + FinancialYear + ", int DistrictId :" + DistrictId + ", int DivisionId:" + DivisionId + ", int LevelId:" + LevelId + ", int ActId :" + ActId + ", string CreatetionDate:" + CreatetionDate + ", int SocietyTypeId :" + SocietyTypeId + ", long UserRoleDeptMappingId :" + UserRoleDeptMappingId + ", int IsOnline :" + IsOnline + ", string actionType:'" + actionType + "')"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetSocietyActList()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_SocietyActList", DBFactoryCommandType.PROCEDURE_COMMAND);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.DashboardModel",
                    FunctionName = "GetSocietyActList()"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetPendencyData(int FinancialYear = 0, int DistrictId = 0, int DivisionId = 0, int LevelId = 0, int ActId = 0, string CreatetionDate = "", int SocietyTypeId = 0, long UserRoleDeptMappingId = 0, int IsOnline = -1, string actionType = "", int type = 0)
        {
            try
            {
                DataSet ds = new DataSet();
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_GetDashboardByFinYear", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FinancialYear", DBFactoryParameterType.NVarchar, FinancialYear);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, LevelId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, ActId);
                objDBAccess.AddParameterToCommand("@CreatetionDate", DBFactoryParameterType.NVarchar, CreatetionDate);
                objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, SocietyTypeId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@IsOnline", DBFactoryParameterType.Int, IsOnline);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, actionType);
                objDBAccess.AddParameterToCommand("@type", DBFactoryParameterType.NVarchar, type);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.DashboardModel",
                    FunctionName = "GetPendencyData(int FinancialYear :" + FinancialYear + ", int DistrictId :" + DistrictId + ", int DivisionId:" + DivisionId + ", int LevelId:" + LevelId + ", int ActId :" + ActId + ", string CreatetionDate:" + CreatetionDate + ", int SocietyTypeId :" + SocietyTypeId + ", long UserRoleDeptMappingId :" + UserRoleDeptMappingId + ", int IsOnline :" + IsOnline + ", string actionType:'" + actionType + "')"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetSocietyRegiAuditorData(int FinancialYear = 0, int DivisionId = 0, int DistrictId = 0, string actionType = "")
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_GetDashboardByChartData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FinancialYear", DBFactoryParameterType.NVarchar, FinancialYear);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.NVarchar, DivisionId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, actionType);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.DashboardModel",
                    FunctionName = "GetSocietyRegiAuditorData(int FinancialYear:'" + FinancialYear + "'', int DivisionId:'" + DivisionId + "', int DistrictId:'" + DistrictId + "', string actionType:'" + actionType + "')"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
    }
}