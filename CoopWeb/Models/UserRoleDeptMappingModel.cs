using System;
using System.Collections.Generic;
using System.Data;
using Cooperative.DBConnector;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cooperative.Controllers;
using RAJSSO.SSOWS;
using System.Linq;

namespace Cooperative.Models
{
    public class UserRoleDeptMappingModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public Int64 DEPTCODE { get; set; }
        public Int64 DESIGNATIONCODE { get; set; }
        public Int64 PostId { get; set; }
        public Int64 RoleID { get; set; }
        public string RoleName_EN { get; set; }
        public string RoleName_HI { get; set; }
        public Int64 UserId { get; set; }
        public string OfficerNameEn { get; set; }
        public string OfficerNameHi { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }
        public int MenuId { get; set; }
        public string MenuActionId { get; set; }
        public int MappingID { get; set; }

        public UserRoleDeptMappingModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public UserRoleDeptMappingModel()
        {
            objDBAccess = new DBFactoryMSSQL();

        }


        public DataTable SelectUserList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UserList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRoleDeptMappingModel", FunctionName = "SelectUserList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable SelectDepartmentList()
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRoleDeptMappingModel", FunctionName = "SelectDepartmentList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable SelectDesignationList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetDesignationList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRoleDeptMappingModel", FunctionName = "SelectDesignationList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


        public DataTable SelectPostList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetPostList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRoleDeptMappingModel", FunctionName = "SelectPostList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable SelectRoleList(int LevelId = 0)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("RoleList", DBFactoryCommandType.PROCEDURE_COMMAND);
                if (LevelId > 0)
                {
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.VARCHAR, " and Role.LevelID=" + LevelId);
                }
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRoleDeptMappingModel", FunctionName = "SelectRoleList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int UserRoleDeptMappingInsert(UserRoleDeptMappingModel obj)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertUserRoleDeptMapping", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.BigInt, obj.MappingID);
                objDBAccess.AddParameterToCommand("@DEPTCODE", DBFactoryParameterType.BigInt, obj.DEPTCODE);
                objDBAccess.AddParameterToCommand("@DESIGNATIONCODE", DBFactoryParameterType.BigInt, obj.DESIGNATIONCODE);
                objDBAccess.AddParameterToCommand("@PostId", DBFactoryParameterType.BigInt, obj.PostId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.BigInt, obj.RoleID);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.BigInt, obj.UserId);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, obj.IsActive);
                objDBAccess.AddParameterToCommand("@AddedBy", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                // objDBAccess.AddParameterToCommand("@IP", DBFactoryParameterType.NVarchar, Controllers.COMMON_FUNCTION.GetIpAddress());
                string resultVal = objDBAccess.ExecuteScalarCommand().ToString();
                int.TryParse(resultVal, out Result);
                //SSORoleUserMapping.SSOUserDetail usrdtl = new SSORoleUserMapping.SSOUserDetail();
                //SSORoleUserMapping.SSOWSSoapClient ssoclient = new SSORoleUserMapping.SSOWSSoapClient();
                //ssoclient.RoleUserMapping("RAJSAHKAR.TEST", "Rajasthan@12", _LoggedInUser.SSOID.ToString().ToUpper().Trim(), "RUMAP", "ADD");
                //return ssoclient.RoleUserMapping(rumapuser, rumappwd, ssoid, ssorole, activity);
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRoleDeptMappingModel", FunctionName = "UserRoleDeptMappingInsert(UserRoleDeptMappingModel obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetUserRoleDeptMapping(ref DataPagingModel Details)
        {
            dt.Clear();
            string whereCase = "";
            string Value = "";
            if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
            {
                 Value = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault();
                whereCase += "and (s.SocietyName like '%" + Value + "%' or s.RegistrationNo  like '%" + Value + "%' or A.Remark  like '%" + Value + "%' or  us.OfficerNameEn like '%" + Value + "%' or us.OfficerNameHi like '%" + Value + "%' or SPS.ProceesStatusTextEn like '%" + Value + "%' or SPS.ProceesStatusTextHi like '%" + Value + "%')";
            }
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetUserRoleDeptMapping", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, Value);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRoleDeptMappingModel", FunctionName = "GetUserRoleDeptMapping()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int ChangeStatusUserRoleDept(int MappingId, int Action)
        {
            int Result = -1;
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UserRoleDeptDeleteStatusChange", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.Int, MappingId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.Int, Action);
                objDBAccess.AddParameterToCommand("@UpdateBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRoleDeptMappingModel", FunctionName = "ChangeStatusUserRoleDept(int MappingId,int Action)" }, ex);
                Result = -1;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetAllMenu(UserRoleDeptMappingModel obj = null)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SelectAllMenu", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, obj != null ? obj.WhereClause : "");
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRoleDeptMappingModel", FunctionName = "GetAllMenu()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetUserRoleRights(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SelectAllUserRights", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRoleDeptMappingModel", FunctionName = "GetUserRoleRights(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int AddAssignMoreRights(List<UserRolemenuAction> list)
        {
            int Result = 0;
            try
            {
                int FirstEntryFlag = 0;
                objDBAccess.OpenConnection();
                for (int i = 0; i < list.Count; i++)
                {

                    SqlCommand cmd = objDBAccess.CreateCommand("InsertUserRights", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.BigInt, list[i].UserId);
                    objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.BigInt, list[i].RoleId);
                    objDBAccess.AddParameterToCommand("@MenuId", DBFactoryParameterType.BigInt, list[i].MenuId);
                    objDBAccess.AddParameterToCommand("@ActionID", DBFactoryParameterType.NVarchar, list[i].MenuAction);
                    objDBAccess.AddParameterToCommand("@DepartmentId", DBFactoryParameterType.BigInt, list[i].DepartmentId);
                    objDBAccess.AddParameterToCommand("@AddedBy", DBFactoryParameterType.BigInt, CreatedBy);
                    objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.BigInt, list[i].MappingId);
                    objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                    objDBAccess.AddParameterToCommand("@FirstEntryFlag", DBFactoryParameterType.BigInt, FirstEntryFlag);
                    objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.NVarchar, list[i].MenuActive);
                    FirstEntryFlag = 1;
                    Result = objDBAccess.ExecuteCommand();
                }


                //object o = 0;// 

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRoleDeptMappingModel", FunctionName = "AddAssignMoreRights(List<UserRolemenuAction> list)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetMenuActionWorkflow(long RoleId, string Menuurl)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetMenuActionWorkflow", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, RoleId);
                objDBAccess.AddParameterToCommand("@Menuurl", DBFactoryParameterType.NVarchar, Menuurl);


                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRoleDeptMappingModel", FunctionName = "GetMenuActionWorkflow(int UserId,string Menuurl)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

    }
    public class UserRolemenuAction
    {
        public int UserId { get; set; }
        public int DepartmentId { get; set; }
        public int RoleId { get; set; }
        public int MenuId { get; set; }
        public string MenuAction { get; set; }
        public int MenuActive { get; set; }
        public int MappingId { get; set; }
    }
}