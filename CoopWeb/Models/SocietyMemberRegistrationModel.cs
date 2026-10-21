using Cooperative.Controllers;
using Cooperative.DBConnector;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cooperative.Models
{
    public class SocietyMemberRegistrationModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public SocietyMemberRegistrationModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public DataTable TitleList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("TitleList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "TitleList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable DepartmentList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DepartmentList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "DepartmentList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable DesignationtList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DesignationtList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "DesignationtList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable PostList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("PostList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "PostList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable StateList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("StateList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "StateList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GenderList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GenderList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "GenderList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable ReligionList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ReligionList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "ReligionList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable CastList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CastList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "CastList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable CategoryList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CategoryList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "CategoryList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable DivisionList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DivisionList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "DivisionList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable DistrictList(int DivisionId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DistrictList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "DistrictList(int DivisionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable TehsilList(int DistrictId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("TehsilList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "TehsilList(int DistrictId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable PanchayatSamitiBlockList(int DistrictId)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("PanchayatSamitiList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "PanchayatSamitiBlockList(int DistrictId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable GramPanchayatList(int PanchayatSamitiId)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GramPanchayatList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, PanchayatSamitiId);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "GramPanchayatList(int PanchayatSamitiId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable VillageList(int GramPanchayatId)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("VillageList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GPId", DBFactoryParameterType.Int, GramPanchayatId);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "VillageList(int GramPanchayatId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable CityList(int DistrictId)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CityList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "CityList(int District)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }


        public DataTable CityListByTehsilId(int TehsilId)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CityList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TehsilId", DBFactoryParameterType.Int, TehsilId);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "CityListByTehsilId(int TehsilId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }


        public DataTable WardList(int CityId)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("WardList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CityId", DBFactoryParameterType.Int, CityId);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "WardList(int CityId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }


        public DataTable WidgetList(int WidgetTypeId)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("WidgetList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WidgetTypeId", DBFactoryParameterType.Int, WidgetTypeId);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "WidgetList(int WidgetTypeId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable FinancialYearList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("FinancialYearList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "FinancialYearList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable DepartmentListByFinancialYearId(int FinancialYearId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DepartmentListByFinancialYear", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FinancialYear", DBFactoryParameterType.Int, FinancialYearId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "public DataTable DepartmentListByFinancialYearId(int FinancialYearId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable RoleList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("RoleList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "DataTable RoleList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable WidgetTypeList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("WidgetTypeList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = " DataTable WidgetTypeList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable IsTableExists(string TableName)
        {
            objDBAccess.OpenConnection();
            SqlCommand cmd = objDBAccess.CreateCommand("SELECT ISNULL(OBJECT_ID('" + TableName + "', N'U'),'-1')", DBFactoryCommandType.TEXT_COMMAND);
            return objDBAccess.GetTable();
        }

        public int CreateTable(string query)
        {
            objDBAccess.OpenConnection();
            SqlCommand cmd = objDBAccess.CreateCommand(query, DBFactoryCommandType.TEXT_COMMAND);
            return objDBAccess.ExecuteCommand();
        }

        public void BulkInsertToDb(DataTable dt, string _TableName)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["wmsprod"].ConnectionString);
            SqlBulkCopy objbulk = new SqlBulkCopy(con);

            if (_TableName == "BDH_CITY") //1 
            {
                objbulk.DestinationTableName = "City";
                objbulk.ColumnMappings.Add("CITY_ID", "CITY_ID");
                objbulk.ColumnMappings.Add("DISTRICT_ID", "DISTRICT_ID");
                objbulk.ColumnMappings.Add("CITY_CODE", "CITY_CODE");
                objbulk.ColumnMappings.Add("CITY_ENG", "CITY_ENG");
                objbulk.ColumnMappings.Add("CITY_MANGAL", "CITY_MANGAL");
                objbulk.ColumnMappings.Add("CITY_TYPE", "CITY_TYPE");
                objbulk.ColumnMappings.Add("CREATION_DATE", "CREATION_DATE");
                objbulk.ColumnMappings.Add("MODIFICATION_DATE", "MODIFICATION_DATE");
                objbulk.ColumnMappings.Add("VERSION", "VERSION");
                objbulk.ColumnMappings.Add("IS_ACTIVE", "IS_ACTIVE");
                objbulk.ColumnMappings.Add("REMARKS", "REMARKS");
            }
            else if (_TableName == "BDH_WARD") //2
            {
                objbulk.DestinationTableName = "Ward";
                objbulk.ColumnMappings.Add("WARD_ID", "WARD_ID");
                objbulk.ColumnMappings.Add("WARD_CODE", "WARD_CODE");
                objbulk.ColumnMappings.Add("WARD_ENG", "WARD_ENG");
                objbulk.ColumnMappings.Add("WARD_MANGAL", "WARD_MANGAL");
                objbulk.ColumnMappings.Add("CREATION_DATE", "CREATION_DATE");
                objbulk.ColumnMappings.Add("MODIFICATION_DATE", "MODIFICATION_DATE");
                objbulk.ColumnMappings.Add("VERSION", "VERSION");
                objbulk.ColumnMappings.Add("IS_ACTIVE", "IS_ACTIVE");
                objbulk.ColumnMappings.Add("IS_DELETED", "IS_DELETED");
                objbulk.ColumnMappings.Add("CITY_ID", "CITY_ID");
                objbulk.ColumnMappings.Add("REMARKS", "REMARKS");
            }
            else if (_TableName == "EMITRA_STATE") //3
            {
                objbulk.DestinationTableName = "State";
                objbulk.ColumnMappings.Add("STATE_ID", "STATE_ID");
                objbulk.ColumnMappings.Add("STATE_CODE", "STATE_CODE");
                objbulk.ColumnMappings.Add("STATE_ENG", "STATE_ENG");
                objbulk.ColumnMappings.Add("STATE_MANGAL", "STATE_MANGAL");
                objbulk.ColumnMappings.Add("CREATION_DATE", "CREATION_DATE");
                objbulk.ColumnMappings.Add("MODIFICATION_DATE", "MODIFICATION_DATE");
                objbulk.ColumnMappings.Add("IS_ACTIVE", "IS_ACTIVE");
                objbulk.ColumnMappings.Add("IS_DELETED", "IS_DELETED");
                objbulk.ColumnMappings.Add("CAPITAL", "CAPITAL");
                objbulk.ColumnMappings.Add("REMARKS", "REMARKS");
            }
            else if (_TableName == "PRAJ_BLOCK") //4
            {
                objbulk.DestinationTableName = "Block";
                objbulk.ColumnMappings.Add("BLOCK_ID", "BLOCK_ID");
                objbulk.ColumnMappings.Add("BLOCK_CODE", "BLOCK_CODE");
                objbulk.ColumnMappings.Add("BLOCK_ENG", "BLOCK_ENG");
                objbulk.ColumnMappings.Add("BLOCK_MANGAL", "BLOCK_MANGAL");
                objbulk.ColumnMappings.Add("BLOCK_STATUS", "BLOCK_STATUS");
                objbulk.ColumnMappings.Add("CREATION_DATE", "CREATION_DATE");
                objbulk.ColumnMappings.Add("MODIFICATION_DATE", "MODIFICATION_DATE");
                objbulk.ColumnMappings.Add("VERSION", "VERSION");
                objbulk.ColumnMappings.Add("IS_ACTIVE", "IS_ACTIVE");
                objbulk.ColumnMappings.Add("IS_DELETED", "IS_DELETED");
                objbulk.ColumnMappings.Add("DISTRICT_ID", "DISTRICT_ID");
                objbulk.ColumnMappings.Add("REMARKS", "REMARKS");
            }
            else if (_TableName == "PRAJ_DISTRICT") //5
            {
                objbulk.DestinationTableName = "District";
                objbulk.ColumnMappings.Add("DISTRICT_ID", "DISTRICT_ID");
                objbulk.ColumnMappings.Add("DISTRICT_CODE", "DISTRICT_CODE");
                objbulk.ColumnMappings.Add("DISTRICT_ENG", "DISTRICT_ENG");
                objbulk.ColumnMappings.Add("DISTRICT_MANGAL", "DISTRICT_MANGAL");
                objbulk.ColumnMappings.Add("CREATION_DATE", "CREATION_DATE");
                objbulk.ColumnMappings.Add("MODIFICATION_DATE", "MODIFICATION_DATE");
                objbulk.ColumnMappings.Add("IS_ACTIVE", "IS_ACTIVE");
                objbulk.ColumnMappings.Add("IS_DELETED", "IS_DELETED");
                objbulk.ColumnMappings.Add("DIVISION_ID", "DIVISION_ID");
                objbulk.ColumnMappings.Add("REMARKS", "REMARKS");
                objbulk.ColumnMappings.Add("IS_TSP", "IS_TSP");
            }
            else if (_TableName == "PRAJ_DIVISION") //6
            {
                objbulk.DestinationTableName = "Division";
                objbulk.ColumnMappings.Add("DIVISION_ID", "DIVISION_ID");
                objbulk.ColumnMappings.Add("DIVISION_CODE", "DIVISION_CODE");
                objbulk.ColumnMappings.Add("DIVISION_ENG", "DIVISION_ENG");
                objbulk.ColumnMappings.Add("DIVISION_MANGAL", "DIVISION_MANGAL");
                objbulk.ColumnMappings.Add("CREATION_DATE", "CREATION_DATE");
                objbulk.ColumnMappings.Add("MODIFICATION_DATE", "MODIFICATION_DATE");
                objbulk.ColumnMappings.Add("VERSION", "VERSION");
                objbulk.ColumnMappings.Add("IS_ACTIVE", "IS_ACTIVE");
                objbulk.ColumnMappings.Add("IS_DELETED", "IS_DELETED");
                objbulk.ColumnMappings.Add("REMARKS", "REMARKS");
            }
            else if (_TableName == "PRAJ_GP") //7
            {
                objbulk.DestinationTableName = "GramPanchayat";
                objbulk.ColumnMappings.Add("GP_ID", "GP_ID");
                objbulk.ColumnMappings.Add("GP_CODE", "GP_CODE");
                objbulk.ColumnMappings.Add("GP_ENG", "GP_ENG");
                objbulk.ColumnMappings.Add("GP_MANGAL", "GP_MANGAL");
                objbulk.ColumnMappings.Add("CREATION_DATE", "CREATION_DATE");
                objbulk.ColumnMappings.Add("MODIFICATION_DATE", "MODIFICATION_DATE");
                objbulk.ColumnMappings.Add("VERSION", "VERSION");
                objbulk.ColumnMappings.Add("IS_ACTIVE", "IS_ACTIVE");
                objbulk.ColumnMappings.Add("IS_DELETED", "IS_DELETED");
                objbulk.ColumnMappings.Add("BLOCK_ID", "BLOCK_ID");
                objbulk.ColumnMappings.Add("REMARKS", "REMARKS");
            }
            else if (_TableName == "PRAJ_VILLAGE") //8
            {
                objbulk.DestinationTableName = "Village";
                objbulk.ColumnMappings.Add("VILLAGE_ID", "VILLAGE_ID");
                objbulk.ColumnMappings.Add("VILLAGE_CODE", "VILLAGE_CODE");
                objbulk.ColumnMappings.Add("VILLAGE_ENG", "VILLAGE_ENG");
                objbulk.ColumnMappings.Add("VILLAGE_MANGAL", "VILLAGE_MANGAL");
                objbulk.ColumnMappings.Add("CREATION_DATE", "CREATION_DATE");
                objbulk.ColumnMappings.Add("MODIFICATION_DATE", "MODIFICATION_DATE");
                objbulk.ColumnMappings.Add("VERSION", "VERSION");
                objbulk.ColumnMappings.Add("IS_ACTIVE", "IS_ACTIVE");
                objbulk.ColumnMappings.Add("IS_DELETED", "IS_DELETED");
                objbulk.ColumnMappings.Add("GP_ID", "GP_ID");
                objbulk.ColumnMappings.Add("PATWAR_ID", "PATWAR_ID");
                objbulk.ColumnMappings.Add("AC_ID", "AC_ID");
                objbulk.ColumnMappings.Add("PS_ID", "PS_ID");
                objbulk.ColumnMappings.Add("REMARKS", "REMARKS");
            }
            else if (_TableName == "RVNU_TEHSIL") //9
            {
                objbulk.DestinationTableName = "Tehsil";
                objbulk.ColumnMappings.Add("TEHSIL_ID", "TEHSIL_ID");
                objbulk.ColumnMappings.Add("TEHSIL_CODE", "TEHSIL_CODE");
                objbulk.ColumnMappings.Add("TEHSIL_ENG", "TEHSIL_ENG");
                objbulk.ColumnMappings.Add("TEHSIL_MANGAL", "TEHSIL_MANGAL");
                objbulk.ColumnMappings.Add("CREATION_DATE", "CREATION_DATE");
                objbulk.ColumnMappings.Add("MODIFICATION_DATE", "MODIFICATION_DATE");
                objbulk.ColumnMappings.Add("VERSION", "VERSION");
                objbulk.ColumnMappings.Add("IS_ACTIVE", "IS_ACTIVE");
                objbulk.ColumnMappings.Add("IS_DELETED", "IS_DELETED");
                objbulk.ColumnMappings.Add("DISTRICT_ID", "DISTRICT_ID");
                objbulk.ColumnMappings.Add("REMARKS", "REMARKS");
            }
            try
            {
                con.Open();
                objbulk.WriteToServer(dt);
            }

            finally { con.Close(); }

        }

        public DataTable GetBreadCrumbByMenuURL(string MenuURL)
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SelectPageLevelPath", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MenuURL", DBFactoryParameterType.NVarchar, MenuURL);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "GetBreadCrumbByMenuURL(string MenuURL)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public string GetBreadCrumb(string lang = "en-US")
        {
            string _data = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                dt = GetBreadCrumbByMenuURL(HttpContext.Current.Request.Url.AbsolutePath);

                if (dt != null && dt.Rows.Count > 0)
                {
                    _data = (lang.ToLower() == "en-us" ? Convert.ToString(dt.Rows[0]["PathEn"]) : Convert.ToString(dt.Rows[0]["PathHi"]));
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "GetBreadCrumb(string lang = 'en - US')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return _data;
        }

        public int SaveFinancialYear(FinancialYearEntity obj)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("FinancialYearInsert", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FIN_YEAR_START", DBFactoryParameterType.Int, obj.FIN_YEAR_START);
                objDBAccess.AddParameterToCommand("@FIN_YEAR_END", DBFactoryParameterType.Int, obj.FIN_YEAR_END);
                int.TryParse(objDBAccess.ExecuteScalarCommand().ToString(), out Result);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "int SaveFinancialYear(FinancialYearEntity obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int ChangeFinancialYearStatus(int Id)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("FinancialYearStatusChange", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, Id);
                int.TryParse(objDBAccess.ExecuteScalarCommand().ToString(), out Result);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "int ChangeFinancialYearStatus(int Id)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int InsertDepartment(DepartmentEntity obj)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DepartmentInsert", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DepartmentNameEn", DBFactoryParameterType.VARCHAR, obj.DEPTNAMEENGLISH);
                objDBAccess.AddParameterToCommand("@DepartmentNameHi", DBFactoryParameterType.NVarchar, obj.DEPTNAMEHINDI);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, obj.CreatedBy);
                int.TryParse(objDBAccess.ExecuteScalarCommand().ToString(), out Result);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "int InsertDepartment(DepartmentEntity obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int ChangeDepartmentStatus(int Id)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DepartmentStatusChange", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DepartmentId", DBFactoryParameterType.Int, Id);
                int.TryParse(objDBAccess.ExecuteScalarCommand().ToString(), out Result);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "int ChangeDepartmentStatus(int Id)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetOrganizationList()
        {
            dt = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetOrganizationList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = " public DataTable GetOrganizationList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int SaveOrganizationUserDetails(OrganizationUserRegistrationEntity _Entity, ref int RequestId)
        {
            int Result = 0;
            int PayDone = 0;
            JsonResult ResultJson;
            DataTable DT = null;
            string UniquerequestId = "0";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertSocietyMemberRegistrationDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RequestId", DBFactoryParameterType.Int, _Entity.UserId);
                objDBAccess.AddParameterToCommand("@ApplicationTypeId", DBFactoryParameterType.Int, _Entity.OrganizationtypeId);
                objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.Int, _Entity.DISTRICT_ID);
                objDBAccess.AddParameterToCommand("@BLOCK_ID", DBFactoryParameterType.Int, _Entity.BLOCK_ID);
                objDBAccess.AddParameterToCommand("@GP_ID", DBFactoryParameterType.Int, _Entity.GP_ID);
                objDBAccess.AddParameterToCommand("@VillageId", DBFactoryParameterType.Int, _Entity.VillageId);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, _Entity.OrganizationId);
                objDBAccess.AddParameterToCommand("@PatwarMandalName", DBFactoryParameterType.NVarchar, _Entity.PatwarMandalName);
                objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, _Entity.Name);
                objDBAccess.AddParameterToCommand("@FatherHusbandName", DBFactoryParameterType.NVarchar, _Entity.FatherHusbandName);
                objDBAccess.AddParameterToCommand("@Age", DBFactoryParameterType.Int, _Entity.Age);
                objDBAccess.AddParameterToCommand("@NomineeName", DBFactoryParameterType.NVarchar, _Entity.NomineeName);
                objDBAccess.AddParameterToCommand("@NomineeRelationId", DBFactoryParameterType.Int, _Entity.NomineeRelationId);
                objDBAccess.AddParameterToCommand("@EconomicSectionId", DBFactoryParameterType.Int, _Entity.EconomicSectionId);
                objDBAccess.AddParameterToCommand("@CategoryId", DBFactoryParameterType.Int, _Entity.CategoryId);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.VARCHAR, _Entity.MobileNo);
                objDBAccess.AddParameterToCommand("@AADHARNo", DBFactoryParameterType.NVarchar, _Entity.AADHARNo);
                objDBAccess.AddParameterToCommand("@SRDHNo", DBFactoryParameterType.NVarchar, _Entity.SRDHNo);
                objDBAccess.AddParameterToCommand("@PANNo", DBFactoryParameterType.VARCHAR, _Entity.PANNo);
                objDBAccess.AddParameterToCommand("@SahkariSavingAcNo", DBFactoryParameterType.VARCHAR, _Entity.SahkariSavingAcNo);
                objDBAccess.AddParameterToCommand("@SahkariSavingAcIFSC", DBFactoryParameterType.VARCHAR, _Entity.SahkariSavingAcIFSC);
                objDBAccess.AddParameterToCommand("@MembershipFee", DBFactoryParameterType.Int, _Entity.MembershipFee);
                objDBAccess.AddParameterToCommand("@ContributionAmt", DBFactoryParameterType.Int, _Entity.ContributionAmt);
                objDBAccess.AddParameterToCommand("@IsLandOwnership", DBFactoryParameterType.Bit, _Entity.IsLandOwnership);
                objDBAccess.AddParameterToCommand("@IsOtherBankLonee", DBFactoryParameterType.Bit, _Entity.IsOtherBankLonee);
                objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, _Entity.Address);
                objDBAccess.AddParameterToCommand("@Gender", DBFactoryParameterType.Int, _Entity.Gender);
                objDBAccess.AddParameterToCommand("@Business", DBFactoryParameterType.NVarchar, _Entity.Business);
                objDBAccess.AddParameterToCommand("@EmailId", DBFactoryParameterType.VARCHAR, _Entity.EmailId);
                objDBAccess.AddParameterToCommand("@AnimalCount", DBFactoryParameterType.Int, _Entity.AnimalCount);
                objDBAccess.AddParameterToCommand("@BuyCount", DBFactoryParameterType.Int, _Entity.BuyCount);
                objDBAccess.AddParameterToCommand("@DepositedAmount", DBFactoryParameterType.Int, _Entity.DepositedAmount);
                objDBAccess.AddParameterToCommand("@IsDisability", DBFactoryParameterType.Bit, _Entity.IsDisability);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _Entity.CreatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@KIOSKID", DBFactoryParameterType.NVarchar, _Entity.KIOSKID);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _Entity.SSOID);
                objDBAccess.AddParameterToCommand("@MembershipNo", DBFactoryParameterType.NVarchar, _Entity.MembershipNo);
                objDBAccess.AddParameterToCommand("@MembershipDate", DBFactoryParameterType.DateTime, _Entity.MembershipDate.ToString().Contains("0001") ? (DateTime.MaxValue) : _Entity.MembershipDate);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, _Entity.CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@CurrentAddress", DBFactoryParameterType.NVarchar, _Entity.CurrentAddress);
                objDBAccess.AddParameterToCommand("@EducationalQualificationId", DBFactoryParameterType.Int, _Entity.EducationalQualificationId);
                objDBAccess.AddParameterToCommand("@DOB", DBFactoryParameterType.DateTime, _Entity.DOB.ToString().Contains("0001") ? (DateTime.MaxValue) : _Entity.DOB.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                objDBAccess.AddParameterToCommand("@PhotoPath", DBFactoryParameterType.NVarchar, _Entity.PhotoPath);
                objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, _Entity.UserType);
                objDBAccess.AddParameterToCommand("@ActionStatus", DBFactoryParameterType.Int, 2);
                objDBAccess.AddParameterToCommand("@ActionRemark", DBFactoryParameterType.NVarchar, "Submited by Citizen");


                //bhupendra
                objDBAccess.AddParameterToCommand("@HouseBuildingNo", DBFactoryParameterType.NVarchar, _Entity.HouseBuildingNo);
                objDBAccess.AddParameterToCommand("@LaneStreetName", DBFactoryParameterType.NVarchar, _Entity.LaneStreetName);
                objDBAccess.AddParameterToCommand("@Locality", DBFactoryParameterType.NVarchar, _Entity.Locality);
                objDBAccess.AddParameterToCommand("@STATE_ID", DBFactoryParameterType.Int, _Entity.STATE_ID);
                objDBAccess.AddParameterToCommand("@DIVISION_ID", DBFactoryParameterType.Int, 0);
                objDBAccess.AddParameterToCommand("@Pincode", DBFactoryParameterType.Int, _Entity.pincode);
                objDBAccess.AddParameterToCommand("@WARD_ID", DBFactoryParameterType.Int, 0);
                objDBAccess.AddParameterToCommand("@CITY_ID", DBFactoryParameterType.Int, 0);


                int.TryParse(objDBAccess.ExecuteScalarCommand().ToString(), out Result);
                RequestId = Result;
                if (Result > 0)
                {
                    DataPagingModel Details = new DataPagingModel();

                    int Typeid = 0;
                    string Searchvalue = "";
                    DT = GetOrganizationUserRegistrationDetails( "AND A.IsActive = 0 and RequestId = " + Result, Typeid, Searchvalue);
                    if (_Entity.OrganizationtypeId == 1)
                    {
                        // Transaction for Land Details
                        if (_Entity.GSSLandDetails != null && _Entity.GSSLandDetails.Count > 0)
                        {
                            foreach (var item in _Entity.GSSLandDetails)
                            {
                                objDBAccess.OpenConnection();
                                SqlCommand cmd2 = objDBAccess.CreateCommand("InsertSocietyMemberLandDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                                objDBAccess.AddParameterToCommand("@VillageName", DBFactoryParameterType.NVarchar, item.VillageName);
                                objDBAccess.AddParameterToCommand("@AccountNo", DBFactoryParameterType.NVarchar, item.AccountNo);
                                objDBAccess.AddParameterToCommand("@KhasaraNo", DBFactoryParameterType.NVarchar, item.KhasaraNo);
                                objDBAccess.AddParameterToCommand("@TotalLandInHactor", DBFactoryParameterType.Int, item.TotalLandInHactor);
                                objDBAccess.AddParameterToCommand("@IrrigationLand", DBFactoryParameterType.Int, item.IrrigationLand);
                                objDBAccess.AddParameterToCommand("@NonIrrigationLand", DBFactoryParameterType.Int, item.NonIrrigationLand);
                                objDBAccess.AddParameterToCommand("@PartInIrrigationLand", DBFactoryParameterType.Int, item.PartInIrrigationLand);
                                objDBAccess.AddParameterToCommand("@PartInNonIrrigationLand", DBFactoryParameterType.Int, item.PartInNonIrrigationLand);
                                objDBAccess.AddParameterToCommand("@RequestId", DBFactoryParameterType.Int, Result);
                                var Result2 = objDBAccess.ExecuteScalarCommand().ToString();
                            }

                        }


                        // Transaction for Crop Detail
                        if (_Entity.SownCropDetails != null && _Entity.SownCropDetails.Count > 0)
                        {
                            foreach (var item in _Entity.SownCropDetails)
                            {
                                objDBAccess.OpenConnection();
                                SqlCommand cmd3 = objDBAccess.CreateCommand("InsertSocietyMemberCropDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                                objDBAccess.AddParameterToCommand("@LandInHactor", DBFactoryParameterType.Int, item.LandInHactor);
                                objDBAccess.AddParameterToCommand("@IsIrrigated", DBFactoryParameterType.Bit, item.IsIrrigated);
                                objDBAccess.AddParameterToCommand("@SownCropName", DBFactoryParameterType.NVarchar, item.SownCropName);
                                objDBAccess.AddParameterToCommand("@IsKharif", DBFactoryParameterType.Bit, item.IsKharif);
                                objDBAccess.AddParameterToCommand("@RequestId", DBFactoryParameterType.Int, Result);
                                var Result3 = objDBAccess.ExecuteScalarCommand().ToString();
                            }
                        }



                        // Transaction for OtherBank loan Details
                        if (_Entity.IsOtherBankLonee && (_Entity.OtherBankLoanDetails != null && _Entity.OtherBankLoanDetails.Count > 0))
                        {
                            foreach (var item in _Entity.OtherBankLoanDetails)
                            {
                                objDBAccess.OpenConnection();
                                SqlCommand cmd4 = objDBAccess.CreateCommand("InsertSocietyMemberOtherBankLoanDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                                objDBAccess.AddParameterToCommand("@InstitutionName", DBFactoryParameterType.NVarchar, item.InstitutionName);
                                objDBAccess.AddParameterToCommand("@ApprovedLoanAmt", DBFactoryParameterType.Int, item.ApprovedLoanAmt);
                                objDBAccess.AddParameterToCommand("@RemainingLoanAmtInTime", DBFactoryParameterType.Int, item.RemainingLoanAmtInTime);
                                objDBAccess.AddParameterToCommand("@RemainingLoanAmtTimeBarred", DBFactoryParameterType.Int, item.RemainingLoanAmtTimeBarred);
                                objDBAccess.AddParameterToCommand("@RequestId", DBFactoryParameterType.Int, Result);
                                var Result4 = objDBAccess.ExecuteScalarCommand().ToString();
                            }
                        }



                    }

                    if (_Entity.OrganizationtypeId == 1 || _Entity.OrganizationtypeId == 2)
                    {
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            var P = dt.Rows[0];
                            //string Message = "प्रिय सदस्य "+ Convert.ToString(P["Name"]) + " आपकी सदस्यता पंजीकरण प्रक्रिया प्रारम्भ हो चुकी है| आपका सदस्यता क्रमांक " + Convert.ToString(P["ApplicationNo"]) + " है.";
                            string Message = "Dear Member " + Convert.ToString(P["Name"]) + ", member registration process has been started| Your member registration Id is " + Convert.ToString(P["ApplicationNo"]);
                            EsanchaarController EC = new EsanchaarController();
                            string ReturnVal = EC.SendSocietySMS(Message, Convert.ToString(P["MobileNo"]));
                        }

                    }

                    if (Result > 0 && _Entity.UserType == "K")
                    {
                        //EmitraController obj = new EmitraController();
                        //JsonResult PayDoneLocal = obj.PayServicesForAll(Result.ToString(), "4690");
                        //if (PayDoneLocal.Data.ToString().Contains("Sucess"))
                        //{
                        //    DT = ChangeOrganizationUserStatus(Result, _Entity.UserType == "K" ? true : false);
                        //}
                    }
                    else if (_Entity.UserType == "C" && Result > 0)
                    {



                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "public int SaveOrganizationUserDetails(OrganizationUserRegistrationEntity _Entity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return RequestId;
        }


        public int SavePLDBUserDetails(OrganizationUserRegistrationEntity _Entity, ref int RequestId)
        {
            int Result = 0;
            int PayDone = 0;
            JsonResult ResultJson;
            DataTable DT = null;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertPLDBMemberRegistrationDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RequestId", DBFactoryParameterType.Int, _Entity.UserId);
                objDBAccess.AddParameterToCommand("@ApplicationTypeId", DBFactoryParameterType.Int, _Entity.OrganizationtypeId);
                objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.Int, _Entity.DISTRICT_ID);
                objDBAccess.AddParameterToCommand("@BLOCK_ID", DBFactoryParameterType.Int, _Entity.BLOCK_ID);
                objDBAccess.AddParameterToCommand("@GP_ID", DBFactoryParameterType.Int, _Entity.GP_ID);
                objDBAccess.AddParameterToCommand("@VillageId", DBFactoryParameterType.Int, _Entity.VillageId);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, _Entity.OrganizationId);
                objDBAccess.AddParameterToCommand("@PatwarMandalName", DBFactoryParameterType.NVarchar, _Entity.PatwarMandalName);
                objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, _Entity.Name);
                objDBAccess.AddParameterToCommand("@FatherHusbandName", DBFactoryParameterType.NVarchar, _Entity.FatherHusbandName);
                objDBAccess.AddParameterToCommand("@Age", DBFactoryParameterType.Int, _Entity.Age);
                objDBAccess.AddParameterToCommand("@NomineeName", DBFactoryParameterType.NVarchar, _Entity.NomineeName);
                objDBAccess.AddParameterToCommand("@NomineeRelationId", DBFactoryParameterType.Int, _Entity.NomineeRelationId);
                objDBAccess.AddParameterToCommand("@EconomicSectionId", DBFactoryParameterType.Int, _Entity.EconomicSectionId);
                objDBAccess.AddParameterToCommand("@CategoryId", DBFactoryParameterType.Int, _Entity.CategoryId);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.VARCHAR, _Entity.MobileNo);
                objDBAccess.AddParameterToCommand("@AADHARNo", DBFactoryParameterType.NVarchar, _Entity.AADHARNo);
                objDBAccess.AddParameterToCommand("@SRDHNo", DBFactoryParameterType.NVarchar, _Entity.SRDHNo);
                objDBAccess.AddParameterToCommand("@PANNo", DBFactoryParameterType.VARCHAR, _Entity.PANNo);
                objDBAccess.AddParameterToCommand("@SahkariSavingAcNo", DBFactoryParameterType.VARCHAR, _Entity.SahkariSavingAcNo);
                objDBAccess.AddParameterToCommand("@SahkariSavingAcIFSC", DBFactoryParameterType.VARCHAR, _Entity.SahkariSavingAcIFSC);
                objDBAccess.AddParameterToCommand("@MembershipFee", DBFactoryParameterType.Int, _Entity.MembershipFee);
                objDBAccess.AddParameterToCommand("@ContributionAmt", DBFactoryParameterType.Int, _Entity.ContributionAmt);
                objDBAccess.AddParameterToCommand("@IsLandOwnership", DBFactoryParameterType.Bit, _Entity.IsLandOwnership);
                objDBAccess.AddParameterToCommand("@IsOtherBankLonee", DBFactoryParameterType.Bit, _Entity.IsOtherBankLonee);
                objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, _Entity.Address);
                objDBAccess.AddParameterToCommand("@Gender", DBFactoryParameterType.Int, _Entity.Gender);
                objDBAccess.AddParameterToCommand("@Business", DBFactoryParameterType.NVarchar, _Entity.Business);
                objDBAccess.AddParameterToCommand("@EmailId", DBFactoryParameterType.VARCHAR, _Entity.EmailId);
                objDBAccess.AddParameterToCommand("@AnimalCount", DBFactoryParameterType.Int, _Entity.AnimalCount);
                objDBAccess.AddParameterToCommand("@BuyCount", DBFactoryParameterType.Int, _Entity.BuyCount);
                objDBAccess.AddParameterToCommand("@DepositedAmount", DBFactoryParameterType.Int, _Entity.DepositedAmount);
                objDBAccess.AddParameterToCommand("@IsDisability", DBFactoryParameterType.Bit, _Entity.IsDisability);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _Entity.CreatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@KIOSKID", DBFactoryParameterType.NVarchar, _Entity.KIOSKID);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _Entity.SSOID);
                //objDBAccess.AddParameterToCommand("@MembershipNo", DBFactoryParameterType.NVarchar, _Entity.MembershipNo);
                objDBAccess.AddParameterToCommand("@MembershipDate", DBFactoryParameterType.DateTime, _Entity.MembershipDate.ToString().Contains("0001") ? (DateTime.MaxValue) : _Entity.MembershipDate);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, _Entity.CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@CurrentAddress", DBFactoryParameterType.NVarchar, _Entity.CurrentAddress);
                objDBAccess.AddParameterToCommand("@EducationalQualificationId", DBFactoryParameterType.Int, _Entity.EducationalQualificationId);
                objDBAccess.AddParameterToCommand("@DOB", DBFactoryParameterType.DateTime, _Entity.DOB.ToString().Contains("0001") ? (DateTime.MaxValue) : _Entity.DOB.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                objDBAccess.AddParameterToCommand("@PhotoPath", DBFactoryParameterType.NVarchar, _Entity.PhotoPath);
                objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, _Entity.UserType);
                objDBAccess.AddParameterToCommand("@OldMembershipNo", DBFactoryParameterType.NVarchar, _Entity.OldMembershipNo);

                //add bhupendra
                objDBAccess.AddParameterToCommand("@PLDB_ID", DBFactoryParameterType.Int, _Entity.PLDB_ID);
                objDBAccess.AddParameterToCommand("@BRANCH_ID", DBFactoryParameterType.Int, _Entity.BRANCH_ID);
                objDBAccess.AddParameterToCommand("@HouseBuildingNo", DBFactoryParameterType.NVarchar, _Entity.HouseBuildingNo);
                objDBAccess.AddParameterToCommand("@LaneStreetName", DBFactoryParameterType.NVarchar, _Entity.LaneStreetName);
                objDBAccess.AddParameterToCommand("@Locality", DBFactoryParameterType.NVarchar, _Entity.Locality);
                objDBAccess.AddParameterToCommand("@STATE_ID", DBFactoryParameterType.Int, _Entity.STATE_ID);
                objDBAccess.AddParameterToCommand("@DIVISION_ID", DBFactoryParameterType.Int, _Entity.DIVISION_ID);
                objDBAccess.AddParameterToCommand("@Pincode", DBFactoryParameterType.Int, _Entity.pincode);
                objDBAccess.AddParameterToCommand("@WARD_ID", DBFactoryParameterType.Int, _Entity.WARD_ID);
                objDBAccess.AddParameterToCommand("@CITY_ID", DBFactoryParameterType.Int, _Entity.CITY_ID);



                int.TryParse(objDBAccess.ExecuteScalarCommand().ToString(), out Result);
                RequestId = Result;
                //if (Result > 0)
                //{
                //    //EmitraController obj = new EmitraController();
                //    //PayDone = obj.PayServicesForAll(Result.ToString(), "4690");
                //    //if (PayDone > 0)
                //    //{
                //    //    DT = ChangeOrganizationUserStatus(PayDone, _Entity.UserType == "K" ? true : false);
                //    //}



                //    DT = GetOrganizationUserRegistrationDetails("AND A.IsActive = 1 and RequestId = " + Result);
                //}



                //if (Result > 0 && _Entity.UserType == "K")
                //{
                //    EmitraController obj = new EmitraController();
                //    JsonResult PayDoneLocal = obj.PayServicesForAll(Result.ToString(), "4690");
                //    if (PayDoneLocal.Data.ToString().Contains("Sucess"))
                //    {
                //        DT = ChangeOrganizationUserStatus(Result, _Entity.UserType == "K" ? true : false);
                //    }
                //}
                //else if (_Entity.UserType == "C" && Result > 0)
                //{
                //    //DT = ChangeOrganizationUserStatus(Result, _Entity.UserType == "K" ? true : false);
                //    #region "insert into Emitrakiosktransaction"
                //    objDBAccess.CreateCommand("insertemitrakiosktransactionAll", DBFactoryCommandType.PROCEDURE_COMMAND);
                //    objDBAccess.AddParameterToCommand("SocietyId", DBFactoryParameterType.Int, Result);//
                //    objDBAccess.AddParameterToCommand("ApplicationNo", DBFactoryParameterType.VARCHAR, DT.Rows[0]["ApplicationNo"].ToString());//
                //    objDBAccess.AddParameterToCommand("KioskID", DBFactoryParameterType.VARCHAR, "SSO");//
                //    objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, "");//
                //    objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, "");
                //    objDBAccess.AddParameterToCommand("RequestStatus", DBFactoryParameterType.VARCHAR, "");
                //    objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, "Transaction Successfully Done");
                //    objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, "");
                //    objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, "");
                //    objDBAccess.AddParameterToCommand("ActId", DBFactoryParameterType.Int, 0);//                    
                //    objDBAccess.AddParameterToCommand("SSOID", DBFactoryParameterType.VARCHAR, _Entity.SSOID);
                //    objDBAccess.AddParameterToCommand("CreatedIP", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIpAddress());
                //    objDBAccess.AddParameterToCommand("SERICEID", DBFactoryParameterType.BigInt, 4690); //int.Parse(emitra.SERVICEID.ToString())
                //    objDBAccess.OpenConnection();
                //    object oi = objDBAccess.ExecuteScalarCommand();
                //    //UniquerequestId = Convert.ToString(oi);
                //    #endregion             
                //}
            }
            catch (Exception ex)
            {
                RequestId = -1;
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "public int SavePLDBUserDetails(OrganizationUserRegistrationEntity _Entity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return RequestId;
        }
        public DataTable SaveSocietyMemberRegiApproval(int RequestId, int ActionStatus, string ActionRemark, string ActionTakeBySSOId)
        {
            int Result = 0;
            DataTable DT = null;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SaveSocietyMemberRegiApproval", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RequestId", DBFactoryParameterType.Int, RequestId);
                objDBAccess.AddParameterToCommand("@ActionStatus", DBFactoryParameterType.Int, ActionStatus);
                objDBAccess.AddParameterToCommand("@ActionRemark", DBFactoryParameterType.NVarchar, ActionRemark);
                objDBAccess.AddParameterToCommand("@ActionTakeBySSOId", DBFactoryParameterType.NVarchar, ActionTakeBySSOId);
                objDBAccess.AddParameterToCommand("@ActionDate", DBFactoryParameterType.DateTime, DateTime.Now);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                int.TryParse(objDBAccess.ExecuteScalarCommand().ToString(), out Result);


            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "public int SaveOrganizationUserDetails(OrganizationUserRegistrationEntity _Entity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return DT;
        }
        public DataTable GetOrganizationUserRegistrationDetails(string WhereClause,int Typeid=0, string Searchvalue="" )
        {
            dt = new DataTable();




            if (Typeid != 0)
            {
                WhereClause += " and ApplicationTypeId='" + Typeid + "'";
            }



            if (Searchvalue != "")
            {
                WhereClause += " and A.Name  like '%" + Searchvalue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'  OR Societies.SocietyName  like '%" + Searchvalue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%' OR A.MobileNo  like '%" + Searchvalue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'  OR A.ApplicationNo  like '%" + Searchvalue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'  ";
            }

            try
            {



                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyMemberRegistrationDetailsList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);



                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = " public DataTable GetOrganizationUserRegistrationDetails()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable ChangeOrganizationUserStatus(int RequestId, bool IsActive)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ChangeSocietyMemberRegistrationStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RequestId", DBFactoryParameterType.Int, RequestId);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, IsActive);
                //int.TryParse(objDBAccess.ExecuteScalarCommand().ToString(), out Result);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = " public int ChangeOrganizationUserStatus(int UserId,bool IsActive)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetSocietyMemberRequestData(string SSOId)
        {
            dt = new DataTable();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyMemberRequestData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, SSOId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = " public DataTable GetSocietyMemberRequestData()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable AAAAAAA(string SSOId)
        {
            dt = new DataTable();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyMemberRequestData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, SSOId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = " public DataTable GetSocietyMemberRequestData()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }




        public DataTable GetSocietyRegisteredMeberDetailsByRequestId(int RequestId, int TypeId)
        {
            dt = new DataTable();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyRegisteredMeberDetailsByRequestId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RequestId", DBFactoryParameterType.Int, RequestId);
                objDBAccess.AddParameterToCommand("@TypeId", DBFactoryParameterType.Int, TypeId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "public DataTable GetSocietyRegisteredMeberDetailsByRequestId(int RequestId,int TypeId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetPLDBUserRegistrationDetails(string WhereClause)
        {
            dt = new DataTable();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetPLDBMemberRegistrationDetailsList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "public DataTable GetPLDBUserRegistrationDetails(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetObjectedFormData(string WhereClause)
        {
            dt = new DataTable();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetObjectedFormdata", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "public DataTable GetObjectedFormData(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }



        public DataTable GetPLDBMemberDetails(string MembershipNo, int DistrictId, int BranchId)
        {
            dt = new DataTable();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetPLDBMemberDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MembershipNo", DBFactoryParameterType.NVarchar, MembershipNo);
                objDBAccess.AddParameterToCommand("@PLDBId", DBFactoryParameterType.NVarchar, DistrictId);
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.NVarchar, BranchId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "public DataTable GetPLDBUserRegistrationDetails(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetPLDBUserListForApproval(int UserId, string SSOId)
        {
            dt = new DataTable();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetPLDBMemberApprovalRequestList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@LoggedInUserId", DBFactoryParameterType.Int, UserId);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "public DataTable GetPLDBUserListForApproval(int UserId,string SSOId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable GetPLDBMembersByUserRights(string UserSSOId, int RoleId, string UserType, string KioskId,int MappingID, int ActionId = 0)
        {
            dt = new DataTable();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetPLDBMembersByUserRights", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserSSOId", DBFactoryParameterType.NVarchar, UserSSOId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, RoleId);
                objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, UserType);
                objDBAccess.AddParameterToCommand("@KioskId", DBFactoryParameterType.NVarchar, KioskId);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);
                objDBAccess.AddParameterToCommand("@MappingID", DBFactoryParameterType.Int, MappingID);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "public DataTable GetPLDBMembersByUserRights(string UserSSOId,int RoleId,string UserType,string KioskId,int ActionId=0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable GetPLDBMembersByUser(ref DataPagingModel Details, string UserSSOId, int RoleId, string UserType, string KioskId, int ActionId = 0,int PLDBId=0,int BranchId=0)
        {
            dt = new DataTable();
            try
            {

                string whereCase = "";

                if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                {

                    var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                    whereCase += " and (REPLACE(lower(rtrim(ltrim(SMR.Name))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'  OR REPLACE(lower(rtrim(ltrim(PLDBData.PLDBName))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'OR REPLACE(lower(rtrim(ltrim(BranchName_En))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR REPLACE(lower(rtrim(ltrim(BranchNameHi))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR REPLACE(lower(rtrim(ltrim(SMR.MobileNo))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR REPLACE(lower(rtrim(ltrim(SMR.ApplicationNo))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR REPLACE(lower(rtrim(ltrim(SMR.CreatedDate))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR REPLACE(lower(rtrim(ltrim(SP.ProceesStatusTextHi))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR REPLACE(lower(rtrim(ltrim(AADHARNo))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR REPLACE(lower(rtrim(ltrim(MembershipNo))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%' )";
                }

                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();


                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetPLDBMembersByUser", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@UserSSOId", DBFactoryParameterType.NVarchar, UserSSOId);
                    objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, RoleId);
                    objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, UserType);
                    objDBAccess.AddParameterToCommand("@KioskId", DBFactoryParameterType.NVarchar, KioskId);
                    objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    objDBAccess.AddParameterToCommand("@PLDBId", DBFactoryParameterType.Int, PLDBId);
                    objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.NVarchar, BranchId);

                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                cmd = objDBAccess.CreateCommand("GetPLDBMembersByUser", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserSSOId", DBFactoryParameterType.NVarchar, UserSSOId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, RoleId);
                objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, UserType);
                objDBAccess.AddParameterToCommand("@KioskId", DBFactoryParameterType.NVarchar, KioskId);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                objDBAccess.AddParameterToCommand("@PLDBId", DBFactoryParameterType.Int, PLDBId);
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.NVarchar, BranchId);

                dt = objDBAccess.GetTable();










                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "public DataTable GetPLDBMembersByUserRights(string UserSSOId,int RoleId,string UserType,string KioskId,int ActionId=0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetPLDBMembersApprovalByUserRights(ref DataPagingModel Details, string UserSSOId, int RoleId, string UserType, string KioskId, int ActionId = 0)
        {
            dt = new DataTable();
            try
            {

                string whereCase = "";


                foreach (var x in Details.SearchParameter.Where(x => x.Key != "searchText")) {
                    whereCase += " and "+x.Key+"='"+x.Value+"'";
                }

                if (Details.SearchParameter.Where(x => x.Key == "searchText").Any())
                {

                    var searchValue = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                    whereCase += " and (REPLACE(lower(rtrim(ltrim(SMR.Name))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'  OR REPLACE(lower(rtrim(ltrim(PLDBData.PLDBName))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%'OR REPLACE(lower(rtrim(ltrim(BranchName_En))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR REPLACE(lower(rtrim(ltrim(BranchNameHi))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR REPLACE(lower(rtrim(ltrim(SMR.MobileNo))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR REPLACE(lower(rtrim(ltrim(SMR.ApplicationNo))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR REPLACE(lower(rtrim(ltrim(SMR.CreatedDate))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR REPLACE(lower(rtrim(ltrim(SP.ProceesStatusTextHi))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR REPLACE(lower(rtrim(ltrim(AADHARNo))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "% 'OR REPLACE(lower(rtrim(ltrim(MembershipNo))),'  ',' ')  like '%" + searchValue.ToLower().TrimStart().TrimEnd().Replace("\t", "").Replace("  ", " ") + "%' )";
                }

                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();


                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetPLDBMembersApprovalByUserRights", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@UserSSOId", DBFactoryParameterType.NVarchar, UserSSOId);
                    objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, RoleId);
                    objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, UserType);
                    objDBAccess.AddParameterToCommand("@KioskId", DBFactoryParameterType.NVarchar, KioskId);
                    objDBAccess.AddParameterToCommand("@MappingID", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                cmd = objDBAccess.CreateCommand("GetPLDBMembersApprovalByUserRights", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserSSOId", DBFactoryParameterType.NVarchar, UserSSOId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, RoleId);
                objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, UserType);
                objDBAccess.AddParameterToCommand("@KioskId", DBFactoryParameterType.NVarchar, KioskId);
                objDBAccess.AddParameterToCommand("@MappingID", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                dt = objDBAccess.GetTable();










                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "public DataTable GetPLDBMembersApprovalByUserRights(string UserSSOId,int RoleId,string UserType,string KioskId,int ActionId=0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable GetRegisteredMemberDetailByRequestId(int RequestId)
        {
            dt = new DataTable();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetRegisteredMemberDetailByRequestId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RequestId", DBFactoryParameterType.Int, RequestId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "public DataTable GetRegisteredMemberDetailByRequestId(int RequestId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int UpdateMemeberDetails(OrganizationUserRegistrationEntity _Entity)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateMemeberDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RequestId", DBFactoryParameterType.Int, _Entity.RequestId);
                objDBAccess.AddParameterToCommand("@EducationalQualificationId", DBFactoryParameterType.Int, _Entity.EducationalQualificationId);
                objDBAccess.AddParameterToCommand("@CategoryId", DBFactoryParameterType.Int, _Entity.CategoryId);
                objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, _Entity.Address);
                objDBAccess.AddParameterToCommand("@CurrentAddress", DBFactoryParameterType.NVarchar, _Entity.CurrentAddress);
                objDBAccess.AddParameterToCommand("@NomineeName", DBFactoryParameterType.NVarchar, _Entity.NomineeName);
                objDBAccess.AddParameterToCommand("@NomineeRelationId", DBFactoryParameterType.Int, _Entity.NomineeRelationId);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, _Entity.MobileNo);
                objDBAccess.AddParameterToCommand("@Business", DBFactoryParameterType.NVarchar, _Entity.Business);
                objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, _Entity.UserType);
                objDBAccess.AddParameterToCommand("@ActionTakenBySSOId", DBFactoryParameterType.NVarchar, _Entity.SSOID);
                objDBAccess.AddParameterToCommand("@ActionStatus", DBFactoryParameterType.NVarchar, _Entity.StatusId);
                objDBAccess.AddParameterToCommand("@KIOSKID", DBFactoryParameterType.NVarchar, _Entity.KIOSKID);
                int.TryParse(objDBAccess.ExecuteScalarCommand().ToString(), out Result);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.MemberRegistrationModel", FunctionName = "public int UpdateMemeberDetails(OrganizationUserRegistrationEntity _Entity)" }, ex);
                Result = -1;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetNextActionListOnCurrentAction(int ActionId)
        {
            dt = new DataTable();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetNextActionListOnCurrentAction", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "public DataTable GetNextActionListOnCurrentAction(int ActionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable SaveMembershipCertificate(int RequestId, string FileName)
        {
            dt = new DataTable();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertMembershipCertificate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RequestId", DBFactoryParameterType.Int, RequestId);
                objDBAccess.AddParameterToCommand("@CertificateFileName", DBFactoryParameterType.NVarchar, FileName);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = " public DataTable SaveMembershipCertificate(int RequestId,string FileName)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public string UpdateMembershipApplicationPayment(OrganizationUserRegistrationEntity _Entity, ref int RequestId)
        {
            string Result = "-1";
            int PayDone = 0;
            JsonResult ResultJson;
            DataTable DT = null;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateMembershipApplicationPayment", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RequestId", DBFactoryParameterType.Int, _Entity.RequestId);
                objDBAccess.AddParameterToCommand("@ReceiptNo", DBFactoryParameterType.NVarchar, _Entity.RequestIdEnc);
                objDBAccess.AddParameterToCommand("@OldMembershipNo", DBFactoryParameterType.NVarchar, _Entity.MembershipNo);
                Result = objDBAccess.ExecuteScalarCommand().ToString();

            }
            catch (Exception ex)
            {
                Result = "-1";
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "public int UpdateMembershipApplicationPayment(OrganizationUserRegistrationEntity _Entity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        #region WFH Balwant

        public DataTable GetPLDBMembershipStatus(int ActionId)
        {
            dt = new DataTable();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetPLDBMembershipStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);

                
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.SocietyMemberRegistrationModel", FunctionName = "public DataTable GetPLDBMembershipStatus(int ActionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        #endregion
        #region Member Upload by Branch Secetary

        public string InsertMemberByExcel(SocietyRegistrationEntity obj)
        {
            string result = "-1";
            string connString = "";
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                string strFileType = System.IO.Path.GetExtension(obj.ExcelUrl).ToLower();
                if (1 > 0)
                {
                    string path = string.Concat(System.Web.HttpContext.Current.Server.MapPath("~/Content/SocietyExcel/" + obj.ExcelUrl));

                    if (strFileType.Trim() == ".xls")
                    {
                        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    else if (strFileType.Trim() == ".xlsx")
                    {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }

                    OleDbConnection conn = new OleDbConnection(connString);
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    foreach (DataRow sheetRow in dtSheet.Rows)
                    {
                        string query = "SELECT * FROM [Members$]";
                        OleDbCommand cmd2 = new OleDbCommand(query, conn);
                        OleDbDataAdapter da = new OleDbDataAdapter(cmd2);
                        DataTable dt = new DataTable();
                        try
                        {
                            da.Fill(dt);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(" 'Sheet1' does not exists in upload Excel. OR Error-" + ex.Message.Replace("'", ""));
                        }

                        dt.Columns.Add("CEOAadharEnc", typeof(System.String));
                        dt.Columns.Add("ExcelUploadId", typeof(System.String));
                        try
                        {
                            int i = 0;
                            DateTime Dt;
                            foreach (DataRow Dr in dt.Rows)
                            {
                                int numberOfRecords = 0;
                                numberOfRecords = dt.Rows.Count;
                                if (numberOfRecords > 1000)
                                {
                                    return "Your Excel record greater than 1000. Please enter the maximum 1000 rows in a Excel sheet";
                                }

                                i++;
                                //string PLDBName = Dr["PLDBNAME"].ToString().Trim();                             
                                //string BranchName = Dr["BRANCHNAME"].ToString().Trim();    
                                string OldMembershipNo = Dr["MembershipNo"].ToString().Trim();
                                string MembershipDate = Dr["MembershipDate"].ToString().Trim();
                                string MemberName = Dr["MemberName"].ToString().Trim();
                                string Father_HusbandName = Dr["Father/HusbandName"].ToString().Trim();
                                string Relation = Dr["RELATION"].ToString().Trim();
                                string Age = Dr["Age"].ToString().Trim();
                                string NomineeName = Dr["NomineeName"].ToString().Trim();
                                string RelationWithNominee = Dr["RelationWithNominee"].ToString().Trim();
                                string Category = Dr["Category"].ToString().Trim();
                                string DOB = Dr["DOB"].ToString().Trim();
                                string MobileNo = Dr["MobileNo"].ToString().Trim();
                                string AADHARNo = Dr["AADHARNo"].ToString().Trim();
                                string MembershipFee = Dr["MembershipFee"].ToString().Trim();
                                string Address = Dr["Address"].ToString().Trim();
                                string Gender = Dr["Gender"].ToString().Trim();
                                string Business = Dr["Business"].ToString().Trim();
                                string CurrentAddress = Dr["CurrentAddress"].ToString().Trim();
                                string Educational = Dr["Educational"].ToString().Trim();
                                //string DistrictName = Dr["DistrictName"].ToString().Trim();
                                string CityName = Dr["CityName"].ToString().Trim();
                                string WardName = Dr["WardName"].ToString().Trim();
                                string BlockName = Dr["BlockName"].ToString().Trim();
                                string GPName = Dr["GPName"].ToString().Trim();
                                string VillageName = Dr["VillageName"].ToString().Trim();
                                string PinCode = Dr["PinCode"].ToString().Trim();

                                if (MemberName != "" || OldMembershipNo != "")
                                {
                                    //    if (PLDBName == "")
                                    //    {
                                    //        return "Please enter PLDB Name on line No " + i.ToString();
                                    //    }
                                    //    else if (MembershipDate == "")
                                    //    {
                                    //        return "Please enter Membership Date on line No " + i.ToString();
                                    //    }
                                    if (MembershipDate.Length < 10)
                                    {
                                        return "Please enter valid Membership Date(dd/mm/yyyy) on line No " + i.ToString();
                                    }
                                    //else if (!IsValidDateTime(RegistrationDate))
                                    //{
                                    //    return "Please enter Proper Registration Date '" + RegistrationDate + "'(dd/mm/yyyy) on line No " + (i + 1).ToString();
                                    //}
                                    //else if (MembershipDate.Length < 10)
                                    //{
                                    //    return "Please enter valid Membership Date(dd/mm/yyyy) on line No " + i.ToString();
                                    //}
                                    else if (OldMembershipNo == "")
                                    {
                                        return "Please enter Membership Number on line No " + i.ToString();
                                    }
                                    else if (MemberName == "")
                                    {
                                        return "Please enter Member Name on line No " + i.ToString();
                                    }
                                    else if (Father_HusbandName == "")
                                    {
                                        return "Please enter Father/Husband Name on line No " + i.ToString();
                                    }
                                    else if (Relation == "")
                                    {
                                        return "Please enter Relation on line No " + i.ToString();
                                    }
                                    else if (Age == "")
                                    {
                                        return "Please enter Age on line No " + i.ToString();
                                    }
                                    else if (NomineeName == "")
                                    {
                                        return "Please enter Nominee Name on line No " + i.ToString();
                                    }
                                    else if (RelationWithNominee == "")
                                    {
                                        return "Please enter Relation with Nominee on line No " + i.ToString();
                                    }
                                    else if (Category == "")
                                    {
                                        return "Please enter Category on line No " + i.ToString();
                                    }
                                    else if (DOB == "")
                                    {
                                        return "Please enter Date of Birth on line No " + i.ToString();
                                    }
                                    else if (RelationWithNominee == "")
                                    {
                                        return "Please enter Relation with Nominee on line No " + i.ToString();
                                    }
                                    else if (MobileNo == "")
                                    {
                                        return "Please enter Mobile Number on line No " + i.ToString();
                                    }
                                    else if (MobileNo.Length > 10 || MobileNo.Length < 10)
                                    {
                                        return "Please enter 10 digit Mobile Number on line No " + i.ToString();
                                    }
                                    else if (AADHARNo == "")
                                    {
                                        return "Please enter AADHAR Number on line No " + i.ToString();
                                    }
                                    else if (AADHARNo.Length > 12 || AADHARNo.Length < 12)
                                    {
                                        return "Please enter 12 digit AADHAR Number on line No " + i.ToString();
                                    }


                                    else if (MembershipFee == "")
                                    {
                                        return "Please enter Membership Fee on line No " + i.ToString();
                                    }
                                    else if (Address == "")
                                    {
                                        return "Please enter Address on line No " + i.ToString();
                                    }
                                    else if (Gender == "")
                                    {
                                        return "Please enter Gender on line No " + i.ToString();
                                    }
                                    else if (Business == "")
                                    {
                                        return "Please enter Business on line No " + i.ToString();
                                    }
                                    else if (CurrentAddress == "")
                                    {
                                        return "Please enter Current Address on line No " + i.ToString();
                                    }
                                    else if (Educational == "")
                                    {
                                        return "Please enter Educational on line No " + i.ToString();
                                    }
                                    else if (PinCode == "")
                                    {
                                        return "Please enter Pincode on line No " + i.ToString();
                                    }
                                    else if (PinCode.Length > 6 || PinCode.Length < 6)
                                    {
                                        return "Please enter 6 digit Pincode Number on line No " + i.ToString();
                                    }

                                    if (((CityName != "" || CityName != "NA") && (WardName != "" || WardName != "NA")) && ((BlockName == "" || BlockName == "NA") && (GPName == "" || GPName == "NA") && (VillageName == "" || VillageName == "NA")))
                                    {
                                        if (CityName == "" && CityName == "NA" || BlockName != "" && BlockName != "NA")
                                        {
                                            //return "Please City Name on line No " + i.ToString();
                                            break;
                                        }
                                        else if ((CityName == "" || CityName == "NA") && (BlockName == "" || BlockName == "NA"))
                                        {
                                            return "Please enter the value City and Ward / Block and Gram Panchayat and Village Name on line No " + i.ToString();
                                        }
                                        else if ((CityName != "" || CityName != "NA") && (WardName == "" || WardName == "NA"))
                                        {
                                            return "Please Ward Name on line No " + i.ToString();

                                        }
                                    }

                                    else if (((BlockName != "" || BlockName != "NA") && (GPName != "" || GPName != "NA") && (VillageName != "" || VillageName != "NA")) && ((CityName == "" || CityName == "NA") && (WardName == "" || WardName == "NA")))
                                    {
                                        if (BlockName == "" && BlockName == "NA" || CityName != "" && CityName != "NA")
                                        {
                                            //return "Please City Name on line No " + i.ToString();
                                            break;
                                        }
                                        else if ((BlockName == "" || BlockName == "NA") && (CityName == "" || CityName == "NA"))
                                        {
                                            return "Please enter the value City and Ward / Block and Gram Panchayat and Village Name on line No " + i.ToString();
                                        }
                                        else if ((BlockName != "" || BlockName != "NA") && (GPName == "" || GPName == "NA"))
                                        {
                                            return "Please enter the Gram Panchayat Name on line No " + i.ToString();

                                        }
                                        else if ((BlockName != "" || BlockName != "NA") && (VillageName == "" || VillageName == "NA"))
                                        {
                                            return "Please enter the Village Name on line No " + i.ToString();

                                        }
                                    }

                                    else if (((BlockName != "" || BlockName != "NA") && (GPName != "" || GPName != "NA") && (VillageName != "" || VillageName != "NA")) && ((CityName != "" || CityName != "NA") && (WardName != "" || WardName != "NA")))
                                    {
                                        return "Please enter the value City and Ward / Block and Gram Panchayat and Village Name on line No " + i.ToString();
                                    }
                                }
                                else
                                {
                                    return "Please enter value in PanchayatSamiti or City on line No " + i.ToString();
                                }
                            }
                            string ResultInner = ""; int Suc = 0, Unsuc = 0, Dupli = 0;

                            objDBAccess.OpenConnection();
                            SqlCommand cmd = objDBAccess.CreateCommand("InsertSocityExcelDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                            objDBAccess.AddParameterToCommand("@ExcelSheetName", DBFactoryParameterType.NVarchar, obj.ExcelUrl);
                            objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                            objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                            objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                            objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                            result = (objDBAccess.ExecuteScalarCommand().ToString());
                            i = 0;

                            foreach (DataRow Dr in dt.Rows)
                            {
                                i++;

                                //string PLDBName = Dr["PLDBNAME"].ToString().Trim();
                                //string BranchName = Dr["BRANCHNAME"].ToString().Trim();
                                string OldMembershipNo = Dr["MembershipNo"].ToString().Trim();
                                string MembershipDate = Dr["MembershipDate"].ToString().Trim();
                                string MemberName = Dr["MemberName"].ToString().Trim();
                                string Father_HusbandName = Dr["Father/HusbandName"].ToString().Trim();
                                string Relation = Dr["RELATION"].ToString().Trim();
                                string Age = Dr["Age"].ToString().Trim();
                                string NomineeName = Dr["NomineeName"].ToString().Trim();
                                string RelationWithNominee = Dr["RelationWithNominee"].ToString().Trim();
                                string Category = Dr["Category"].ToString().Trim();
                                string DOB = Dr["DOB"].ToString().Trim();
                                string MobileNo = Dr["MobileNo"].ToString().Trim();
                                string AADHARNo = Dr["AADHARNo"].ToString().Trim();
                                string MembershipFee = Dr["MembershipFee"].ToString().Trim();
                                string Address = Dr["Address"].ToString().Trim();
                                string Business = Dr["Business"].ToString().Trim();
                                string CurrentAddress = Dr["CurrentAddress"].ToString().Trim();
                                string Educational = Dr["Educational"].ToString().Trim();
                                string Gender = Dr["Gender"].ToString().Trim();
                                int GenderType = 0;
                                if (Gender == "M" || Gender == "Male") { GenderType = 1; }
                                else if (Gender == "F" || Gender == "Female") { GenderType = 2; }
                                else { GenderType = 3; }
                                //string DistrictName = Dr["DistrictName"].ToString().Trim();
                                string CityName = Dr["CityName"].ToString().Trim();
                                string WardName = Dr["WardName"].ToString().Trim();
                                string BlockName = Dr["BlockName"].ToString().Trim();
                                string GPName = Dr["GPName"].ToString().Trim();
                                string VillageName = Dr["VillageName"].ToString().Trim();
                                string PinCode = Dr["PinCode"].ToString().Trim();

                                if (MemberName != "" || OldMembershipNo != "")
                                {
                                    SqlCommand cmd1 = objDBAccess.CreateCommand("InsertMemberUploadByBranchS_ExcelDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                                    cmd1.Parameters.AddWithValue("@RequestId", 0);
                                    cmd1.Parameters.AddWithValue("@ApplicationTypeId", 1);
                                    cmd1.Parameters.AddWithValue("@PLDBId", _LoggedInUser.PLDBId);
                                    cmd1.Parameters.AddWithValue("@BranchId", _LoggedInUser.PLDBBrancchId);
                                    cmd1.Parameters.AddWithValue("@OldMembershipNo", OldMembershipNo);
                                    cmd1.Parameters.AddWithValue("@MembershipDate", MembershipDate);
                                    cmd1.Parameters.AddWithValue("@Name", MemberName);
                                    cmd1.Parameters.AddWithValue("@FatherHusbandName", Father_HusbandName);
                                    cmd1.Parameters.AddWithValue("@PatwarMandalName", "");
                                    cmd1.Parameters.AddWithValue("@NomineeName", NomineeName);
                                    cmd1.Parameters.AddWithValue("@NomineeRelationName", RelationWithNominee);
                                    cmd1.Parameters.AddWithValue("@Age", Age);
                                    cmd1.Parameters.AddWithValue("@CategoryName", Category);
                                    cmd1.Parameters.AddWithValue("@DOB", DOB);
                                    cmd1.Parameters.AddWithValue("@MobileNo", MobileNo);
                                    cmd1.Parameters.AddWithValue("@AADHARNo", AADHARNo);
                                    cmd1.Parameters.AddWithValue("@MembershipFee", MembershipFee);
                                    cmd1.Parameters.AddWithValue("@ContributionAmt", 0);
                                    cmd1.Parameters.AddWithValue("@Address", Address);
                                    cmd1.Parameters.AddWithValue("@Gender", GenderType);
                                    cmd1.Parameters.AddWithValue("@Business", Business);
                                    cmd1.Parameters.AddWithValue("@CurrentAddress", CurrentAddress);
                                    cmd1.Parameters.AddWithValue("@EducationalQualificationName", Educational);
                                    cmd1.Parameters.AddWithValue("@CreatedBy", _LoggedInUser.CreatedBy);
                                    cmd1.Parameters.AddWithValue("@CreatedIP", COMMON_FUNCTION.GetIpAddress());
                                    cmd1.Parameters.AddWithValue("@UpdatedBy", _LoggedInUser.UpdatedBy);
                                    cmd1.Parameters.AddWithValue("@UpdatedIP", COMMON_FUNCTION.GetIpAddress());
                                    cmd1.Parameters.AddWithValue("@DISTRICT_ID", _LoggedInUser.DistrictId);
                                    cmd1.Parameters.AddWithValue("@BLOCK_ID", BlockName);
                                    cmd1.Parameters.AddWithValue("@GP_ID", GPName);
                                    cmd1.Parameters.AddWithValue("@VillageId", VillageName);
                                    cmd1.Parameters.AddWithValue("@SocietyId", 0);
                                    cmd1.Parameters.AddWithValue("@UserType", _LoggedInUser.UserType);
                                    cmd1.Parameters.AddWithValue("@ActionStatus", 0);
                                    cmd1.Parameters.AddWithValue("@ActionRemark", "");
                                    cmd1.Parameters.AddWithValue("@SSOID", _LoggedInUser.SSOID);
                                    cmd1.Parameters.AddWithValue("@ExcelUploadId", result);
                                    cmd1.Parameters.AddWithValue("@Relation", Relation);
                                    cmd1.Parameters.AddWithValue("@CITY_Name", CityName);
                                    cmd1.Parameters.AddWithValue("@WARD_Name", WardName);
                                    cmd1.Parameters.AddWithValue("@PinCode", PinCode);

                                    ResultInner = (objDBAccess.ExecuteScalarCommand().ToString());
                                    if (ResultInner == "1")
                                    {
                                        Suc = Suc + 1;
                                    }
                                    else if (ResultInner == "2")
                                    {
                                        Dupli = Dupli + 1;
                                        SqlCommand cmd21 = objDBAccess.CreateCommand("InsertMemberBulkUploadFaildRecord", DBFactoryCommandType.PROCEDURE_COMMAND);
                                        cmd21.Parameters.AddWithValue("@Id", 0);
                                        cmd21.Parameters.AddWithValue("@PLDBId", _LoggedInUser.PLDBId);  //
                                        cmd21.Parameters.AddWithValue("@BranchId", _LoggedInUser.PLDBBrancchId);  //
                                        cmd21.Parameters.AddWithValue("@OldMembershipNo", OldMembershipNo);
                                        cmd21.Parameters.AddWithValue("@MembershipDate", MembershipDate);
                                        cmd21.Parameters.AddWithValue("@Name", MemberName);
                                        cmd21.Parameters.AddWithValue("@FatherHusbandName", Father_HusbandName);
                                        cmd21.Parameters.AddWithValue("@Relation", Relation);
                                        cmd21.Parameters.AddWithValue("@NomineeName", NomineeName);
                                        cmd21.Parameters.AddWithValue("@NomineeRelationName", RelationWithNominee);
                                        cmd21.Parameters.AddWithValue("@Age", Age);
                                        cmd21.Parameters.AddWithValue("@CategoryName", Category); //
                                        cmd21.Parameters.AddWithValue("@DOB", DOB);
                                        cmd21.Parameters.AddWithValue("@MobileNo", MobileNo);
                                        cmd21.Parameters.AddWithValue("@AADHARNo", AADHARNo);
                                        cmd21.Parameters.AddWithValue("@MembershipFee", MembershipFee);
                                        cmd21.Parameters.AddWithValue("@ContributionAmt", 0);
                                        cmd21.Parameters.AddWithValue("@Address", Address);
                                        cmd21.Parameters.AddWithValue("@Gender", GenderType);
                                        cmd21.Parameters.AddWithValue("@Business", Business);
                                        cmd21.Parameters.AddWithValue("@CurrentAddress", CurrentAddress);
                                        cmd21.Parameters.AddWithValue("@EducationalQualificationName", Educational);   ////
                                        cmd21.Parameters.AddWithValue("@CreatedBy", _LoggedInUser.CreatedBy);
                                        cmd21.Parameters.AddWithValue("@CreatedIP", COMMON_FUNCTION.GetIpAddress());
                                        cmd21.Parameters.AddWithValue("@DISTRICT_ID", _LoggedInUser.DistrictId);
                                        cmd21.Parameters.AddWithValue("@BLOCK_ID", BlockName);
                                        cmd21.Parameters.AddWithValue("@GP_ID", GPName);
                                        cmd21.Parameters.AddWithValue("@VillageId", VillageName);
                                        cmd21.Parameters.AddWithValue("@SocietyId", 0);
                                        cmd21.Parameters.AddWithValue("@SSOID", _LoggedInUser.SSOID);
                                        cmd21.Parameters.AddWithValue("@ExcelUploadId", result);
                                        cmd21.Parameters.AddWithValue("@Reasion", "Duplicate Data");
                                        cmd21.Parameters.AddWithValue("@CITY_Name", CityName);
                                        cmd21.Parameters.AddWithValue("@WARD_Name", WardName);
                                        cmd21.Parameters.AddWithValue("@PinCode", PinCode);

                                        ResultInner = (objDBAccess.ExecuteScalarCommand().ToString());
                                    }
                                    else if (ResultInner == "-1")
                                    {
                                        Unsuc = Unsuc + 1;
                                        SqlCommand cmd21 = objDBAccess.CreateCommand("InsertMemberBulkUploadFaildRecord", DBFactoryCommandType.PROCEDURE_COMMAND);
                                        cmd21.Parameters.AddWithValue("@Id", 0);
                                        cmd21.Parameters.AddWithValue("@PLDBId", _LoggedInUser.PLDBId);  //
                                        cmd21.Parameters.AddWithValue("@BranchId", _LoggedInUser.PLDBBrancchId);  //
                                        cmd21.Parameters.AddWithValue("@OldMembershipNo", OldMembershipNo);
                                        cmd21.Parameters.AddWithValue("@MembershipDate", MembershipDate);
                                        cmd21.Parameters.AddWithValue("@Name", MemberName);
                                        cmd21.Parameters.AddWithValue("@FatherHusbandName", Father_HusbandName);
                                        cmd21.Parameters.AddWithValue("@Relation", Relation);
                                        cmd21.Parameters.AddWithValue("@NomineeName", NomineeName);
                                        cmd21.Parameters.AddWithValue("@NomineeRelationName", RelationWithNominee);
                                        cmd21.Parameters.AddWithValue("@Age", Age);
                                        cmd21.Parameters.AddWithValue("@CategoryName", Category);  //
                                        cmd21.Parameters.AddWithValue("@DOB", DOB);
                                        cmd21.Parameters.AddWithValue("@MobileNo", MobileNo);
                                        cmd21.Parameters.AddWithValue("@AADHARNo", AADHARNo);
                                        cmd21.Parameters.AddWithValue("@MembershipFee", MembershipFee);
                                        cmd21.Parameters.AddWithValue("@ContributionAmt", 0);
                                        cmd21.Parameters.AddWithValue("@Address", Address);
                                        cmd21.Parameters.AddWithValue("@Gender", GenderType);
                                        cmd21.Parameters.AddWithValue("@Business", Business);
                                        cmd21.Parameters.AddWithValue("@CurrentAddress", CurrentAddress);
                                        cmd21.Parameters.AddWithValue("@EducationalQualificationName", Educational);   ////
                                        cmd21.Parameters.AddWithValue("@CreatedBy", _LoggedInUser.CreatedBy);
                                        cmd21.Parameters.AddWithValue("@CreatedIP", COMMON_FUNCTION.GetIpAddress());
                                        cmd21.Parameters.AddWithValue("@DISTRICT_ID", _LoggedInUser.DistrictId);
                                        cmd21.Parameters.AddWithValue("@BLOCK_ID", BlockName);
                                        cmd21.Parameters.AddWithValue("@GP_ID", GPName);
                                        cmd21.Parameters.AddWithValue("@VillageId", VillageName);
                                        cmd21.Parameters.AddWithValue("@SocietyId", 0);
                                        cmd21.Parameters.AddWithValue("@SSOID", _LoggedInUser.SSOID);
                                        cmd21.Parameters.AddWithValue("@ExcelUploadId", result);
                                        cmd21.Parameters.AddWithValue("@Reasion", "Error");
                                        cmd21.Parameters.AddWithValue("@CITY_Name", CityName);
                                        cmd21.Parameters.AddWithValue("@WARD_Name", WardName);
                                        cmd21.Parameters.AddWithValue("@PinCode", PinCode);
                                        ResultInner = (objDBAccess.ExecuteScalarCommand().ToString());
                                    }
                                }
                            }
                            result = "Successfully Saved=" + Suc.ToString() + ".  Duplicate Entry=" + Dupli.ToString() + ".  Error  Occurred=" + Unsuc.ToString();
                        }
                        catch (Exception ex)
                        {
                            objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "InsertSocityByExcel(SocietyRegistrationEntity obj)" }, ex);
                            result = ex.Message;
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "InsertSocityByExcel(SocietyRegistrationEntity obj)" }, ex);
                result = ex.Message;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }

        public DataTable GetMemberExcelDetails(long userId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetMemberExcelDetailsByCreatedBy", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, userId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable  GetMemberExcelDetails(int userId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetExlMemberDetails(int ExlId, int StatusId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetExcelMemberDetailsByExcelUploadId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ExcelUploadId", DBFactoryParameterType.Int, ExlId);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable   GetExlSocietyDetails(int ExlId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public string UpdateExcelMemberDetailsByExcelUpload(string RequestId, string MemberName, string FatherHusbandName, string MobileNo, string AADHARNo, string MembershipNo, string MembershipDate)
        {
            string result = "1";
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                string newDate = "";
                //if (!IsValidDateTime(RegistrationDate))
                //{
                //    return "-3";
                //}
                //else
                //{

                //newDate = MembershipDate.Split('/')[2] + "/" + MembershipDate.Split('/')[1] + "/" + MembershipDate.Split('/')[0];
                // }

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateExcelMemberDetailsByExcelUpload", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RequestId", DBFactoryParameterType.NVarchar, RequestId);
                //objDBAccess.AddParameterToCommand("@AdministrativeId", DBFactoryParameterType.NVarchar, AdministrativeId);
                objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, MemberName);
                objDBAccess.AddParameterToCommand("@FatherHusbandName", DBFactoryParameterType.NVarchar, FatherHusbandName);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, MobileNo);
                objDBAccess.AddParameterToCommand("@AADHARNo", DBFactoryParameterType.NVarchar, AADHARNo);
                objDBAccess.AddParameterToCommand("@MembershipNo", DBFactoryParameterType.NVarchar, MembershipNo);
                objDBAccess.AddParameterToCommand("@MembershipDate", DBFactoryParameterType.NVarchar, MembershipDate);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.NVarchar, _LoggedInUser.UpdatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                //objDBAccess.AddParameterToCommand("@Aadh", DBFactoryParameterType.NVarchar, Aadh);
                result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                result = "0";
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable   UpdateExcelMemberDetailsByExcelUpload(string SocietyId, string AdministrativeId, string SocietyName, string RegistrationNo, string RegistrationDate, string Name, string ContactNo, string Aadh)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }


        public int DeleteExlMemberDetails(int ExlId, int StatusId, int RequestId)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteMemberExcelDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ExcelUploadId", DBFactoryParameterType.Int, ExlId);
                objDBAccess.AddParameterToCommand("@RequestId", DBFactoryParameterType.Int, RequestId);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DeleteExlMemberDetails(int ExlId, int StatusId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        #endregion
    }
}