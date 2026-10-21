using Cooperative.Controllers;
using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using static Cooperative.Models.NCDApiModel;
using static Cooperative.Models.NCDServiceModel;

namespace Cooperative.Models
{
    public class NcdDataAnalyticalModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;
        public NcdDataAnalyticalModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public NcdDataAnalyticalModel()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();

        }



        public DataSet GetSectorDashboard(string fromDate = null, string toDate = null, int? districtId = null, int? functionalStatusId = null)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetSectorDashboard", DBFactoryCommandType.PROCEDURE_COMMAND);
                // Pass dates correctly
                objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.VARCHAR, fromDate);
                objDBAccess.AddParameterToCommand("@ToDate", DBFactoryParameterType.VARCHAR, toDate);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@FunctionalStatusId", DBFactoryParameterType.Int, functionalStatusId);

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "GetSectorDashboard()"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetDistrictDashboard(string fromDate = null, string toDate = null, int? districtId = null, int? functionalStatusId = null)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetDistrictDashboard", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.VARCHAR, fromDate);
                objDBAccess.AddParameterToCommand("@ToDate", DBFactoryParameterType.VARCHAR, toDate);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@FunctionalStatusId", DBFactoryParameterType.Int, functionalStatusId);

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "GetDistrictDashboard()"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetDashboard(string action, int? stateId = null, int? districtId = null, int? functionalStatusId = null, string fromDate = null, string toDate = null)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetDashboard", DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, action);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@FunctionalStatusId", DBFactoryParameterType.Int, functionalStatusId);
                objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.VARCHAR, fromDate);
                objDBAccess.AddParameterToCommand("@ToDate", DBFactoryParameterType.VARCHAR, toDate);

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "GetDashboard()"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetSocietyMemberDashboard(string fromDate = null, string toDate = null, int? districtId = null, int? functionalStatusId = null)
        {
            DataSet ds = new DataSet();
            List<string> sectorColumns = new List<string>();

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_SocietyMemberDashboard", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.VARCHAR, fromDate);
                objDBAccess.AddParameterToCommand("@ToDate", DBFactoryParameterType.VARCHAR, toDate);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@FunctionalStatusId", DBFactoryParameterType.Int, functionalStatusId);

                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "GetSocietyMemberDashboard()"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return (ds);
        }

        public DataSet GetSocietyFacilityByPrimaryActivity(int? primaryId)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetSocietyFacility", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetSocietyFacility");
                objDBAccess.AddParameterToCommand("@PrimaryKey", DBFactoryParameterType.Int, primaryId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.NcdDataModel", FunctionName = "GetSocietyFacilityByPrimaryActivity(string Action = 'Action')" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetSocietyFunctionalStatus()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("M_SocietyFunctionalStatusProc", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetSocietyFunctionalStatus");

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataModel",
                    FunctionName = "GetSocietyFunctionalStatus()"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }
        public DataSet GetBlockCoverageDashboard()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AnalyticsDashboard", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetGramPanchayatByDistrictBlock");
               

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataAnalyticalModel",
                    FunctionName = "GetBlockCoverageDashboard"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }


        public DataSet GetRegisteredSocietiesTrend()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AnalyticsDashboard", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "RegisteredPrimarySocietyDashboard");

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataAnalyticalModel",
                    FunctionName = "GetRegisteredSocietiesTrend()"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetFunctionalStatusTrend()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AnalyticsDashboard",DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action",DBFactoryParameterType.NVarchar, "FunctionalStatusSocietyDashboard");

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataAnalyticalModel",
                    FunctionName = "GetFunctionalStatusTrend()"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetFunctionalStatusTrendDairy()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AnalyticsDashboard",DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action",DBFactoryParameterType.NVarchar, "FunctionalDairyStatusSocietyDashboard");

                //objDBAccess.AddParameterToCommand(
                //    "@SectorId",
                //    DBFactoryParameterType.Int,
                //    sectorId
                //);

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataAnalyticalModel",
                    FunctionName = "GetFunctionalStatusTrend()"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetFunctionalFisheryStatusTrend()
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AnalyticsDashboard", DBFactoryCommandType.PROCEDURE_COMMAND );
                objDBAccess.AddParameterToCommand("@Action",DBFactoryParameterType.NVarchar, "FunctionalFisheryStatusSocietyDashboard");

                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity()
                {
                    PageName = "Cooperative.Models.NcdDataAnalyticalModel",
                    FunctionName = "GetFunctionalFisheryStatusTrend()"
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
