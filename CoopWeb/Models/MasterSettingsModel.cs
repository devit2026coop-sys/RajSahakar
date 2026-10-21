using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.UI;
using System.Web;
using Cooperative.Controllers;

namespace Cooperative.Models
{

    public class OrganizationLevelsMapping
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;
        public long MappingID { get; set; }
        public long DepartmentId { get; set; }
        public long OrganisationId { get; set; }
        public long LevelId { get; set; }
        public int IsActive { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public string WhereClause { get; set; }
        public List<OrganizationLevelsMapping> LstMasterSettingsModel { get; set; }

        public OrganizationLevelsMapping(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public OrganizationLevelsMapping()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public int AddOrganizationLevelMappings(List<OrganizationLevelsMapping> lst)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                foreach (OrganizationLevelsMapping obj in lst)
                {
                    SqlCommand cmd = objDBAccess.CreateCommand("InsertOrganizationLevelMapping", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@MappingID", DBFactoryParameterType.BigInt, obj.MappingID);
                    objDBAccess.AddParameterToCommand("@OrganizationId", DBFactoryParameterType.BigInt, obj.OrganisationId);
                    objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.BigInt, obj.LevelId);
                    objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, obj.IsActive);
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, Controllers.COMMON_FUNCTION.GetIpAddress());
                    objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, Controllers.COMMON_FUNCTION.GetIpAddress());
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterSettingsModel", FunctionName = "AddOrganizationLevelMappings(MasterSettingsModel obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int DeleteOrganizationLevelMapping(int MenuId)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteSportAssociation", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@AssociationId", DBFactoryParameterType.BigInt, MenuId);

                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterSettingsModel", FunctionName = "DeleteOrganizationLevelMapping(int MenuId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        //Bread creum start
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterSettingsModel", FunctionName = "GetBreadCrumbByMenuURL(string MenuURL)" }, ex);
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterSettingsModel", FunctionName = "GetBreadCrumb(string lang = 'en - US')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return _data;
        }

        //Bread creum end

        public int ChangeStatus(int MappingId, int Action)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("OrganizationLevelMappingDeleteStatusChange", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MappingID", DBFactoryParameterType.Int, MappingId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.Int, Action);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterSettingsModel", FunctionName = "ChangeStatus(int MappingId, int Action)" }, ex);
                Result = -1;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }



        public DataTable GetAllOrganizationLevels(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("OrganizationLevelList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterSettingsModel", FunctionName = "GetAllOrganizationLevels(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


    }


    public class DesignationLevelMappingModel
    {

        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public long DesignationLevelMappingID { get; set; }
        public long OrganizationLevelsMappingID { get; set; }
        public long DESIGNATIONCODE { get; set; }
        public int IsActive { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public string WhereClause { get; set; }


        public DesignationLevelMappingModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }


        public DesignationLevelMappingModel()
        {

            objDBAccess = new DBFactoryMSSQL();

        }




        public int AddDesignationLevelMapping(int DESIGNATIONCODE, string IsHOD, string IsActive, int DesignationLevelMappingID, int OrganizationLevelsMappingID,int OfficeId)
        {
            DesignationLevelMappingModel _Model;
            int Result = 0;
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertDesignationLevelMapping", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DesignationLevelMappingID", DBFactoryParameterType.BigInt, DesignationLevelMappingID);
                objDBAccess.AddParameterToCommand("@OrganizationLevelsMappingID", DBFactoryParameterType.Int, OrganizationLevelsMappingID);
                objDBAccess.AddParameterToCommand("@DESIGNATIONCODE", DBFactoryParameterType.Int, DESIGNATIONCODE);
                objDBAccess.AddParameterToCommand("@OfficeId", DBFactoryParameterType.Int, OfficeId);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, IsActive == "True" ? 1 : 0);
                objDBAccess.AddParameterToCommand("@IsHOD", DBFactoryParameterType.Int, IsHOD == "True" ? 1 : 0);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());

                //Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterSettingsModel", FunctionName = "AddDesignationLevelMapping(int DESIGNATIONCODE, string IsHOD, string IsActive, int DesignationLevelMappingID, int OrganizationLevelsMappingID)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetAllDesignationLevelMappings(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("GetDesignationLevelMappingList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterSettingsModel", FunctionName = "GetAllDesignationLevelMappings(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


        public int ChangeStatusDesignationLevelMapping(int DesignationLevelMappingId, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteStatusDesignationLevelMapping", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DesignationLevelMappingId", DBFactoryParameterType.BigInt, DesignationLevelMappingId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);

                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterSettingsModel", FunctionName = "ChangeStatusDesignationLevelMapping(int DesignationLevelMappingId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetAllDesignationByOrganizationLevelsMappingID(string OrganizationLevelsMappingID,string OfficeId)
        {
            return GetAllDesignationLevelMappings(" and o.isactive=1 and o.OrganizationLevelsMappingID=" + OrganizationLevelsMappingID.ToString() + " and o.OfficeId="+ OfficeId.ToString());
        }

    }
}