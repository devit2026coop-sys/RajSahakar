using System;
using System.Collections.Generic;
using System.Data;
using Cooperative.DBConnector;
using System.Data.SqlClient;
using System.Web;

namespace Cooperative.Models
{
    public class WorkFlowModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public int MenuId { get; set; }
        public string MenuNameEn { get; set; }
        public string MenuUrl { get; set; }
        public int RoleId { get; set; }
        public string RoleName_EN { get; set; }

        public int MenuId1 { get; set; }
        public int NextRoleId { get; set; }
        public string nextroleen { get; set; }

        public int IsActive { get; set; }
        public int IsActive1 { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int DeletedBy { get; set; }

        public int LovValuesId { get; set; }
        public string LovCode { get; set; }
        public int LevelNo { get; set; }
        public int ActionID { get; set; }
        public string actionbyuser { get; set; }


        public WorkFlowModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public WorkFlowModel()
        {
            objDBAccess = new DBFactoryMSSQL();
        }

        public int SaveWorkFlowConfiguration()
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("WorkFlowConfiguration", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MenuId", DBFactoryParameterType.Int, MenuId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, RoleId);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, IsActive);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, CreatedBy);
                objDBAccess.AddParameterToCommand("@ModifiedBy", DBFactoryParameterType.Int, ModifiedBy);
                objDBAccess.AddParameterToCommand("@DeletedBy", DBFactoryParameterType.Int, DeletedBy);
                objDBAccess.AddParameterToCommand("@NextRoleId", DBFactoryParameterType.Int, NextRoleId);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionID);
                objDBAccess.AddParameterToCommand("@Level", DBFactoryParameterType.Int, LevelNo);
                objDBAccess.AddParameterToCommand("@IsActive1", DBFactoryParameterType.Int, IsActive1);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WorkFlowModel", FunctionName = "SaveWorkFlowConfiguration()" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        // List Of Actions For Execution Cycle in Work Flow
        public DataTable WorkFlowExecutionAction(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("WorkFlowAction", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WorkFlowModel", FunctionName = "WorkFlowExecutionAction(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        //Bread crum start
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WorkFlowModel", FunctionName = "GetBreadCrumbByMenuURL(string MenuURL)" }, ex);
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WorkFlowModel", FunctionName = "GetBreadCrumb(string lang = 'en - US')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return _data;
        }
        //bread crum end
        public DataTable GetAllWorkflow(int RoleId, string Menuurl)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAllWorkflowbyRoleId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, RoleId);
                objDBAccess.AddParameterToCommand("@Menuurl", DBFactoryParameterType.NVarchar, Menuurl);
                

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WorkflowModel", FunctionName = "GetAllWorkflow(int RoleId,string Menuurl)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


        //public DataTable URLList(int menuid)
        //{
        //    dt.Clear();
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("EO_GetURL_ByMenu", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@MenuId", DBFactoryParameterType.Int, menuid);
        //        dt = objDBAccess.GetTable();
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "URLList(int menuid)" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }

        //    return dt;
        //}

        //public DataTable MenuList()
        //{
        //    dt.Clear();
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("EO_getMenu", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        dt = objDBAccess.GetTable();
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "MenuList()" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return dt;
        //}

        //public DataTable RoleName()
        //{
        //    dt.Clear();
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("EO_GetRoleName", DBFactoryCommandType.PROCEDURE_COMMAND);

        //        dt = objDBAccess.GetTable();
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "RoleName()" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }

        //    return dt;
        //}

        //List of Work Flow Details
        public DataTable GetWorkFlowList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("WorkFlowList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WorkFlowModel", FunctionName = "GetWorkFlowList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int ChangeWorkFlowStatus(int Id, int Action)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("WorkFlowStatusChange", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, Id);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.Int, Action);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRoleModel", FunctionName = "ChangeWorkFlowStatus(int Id, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable WorkFlowActionList(int TypeId, int RequestId, int RoleId, string MenuURL)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("WorkFlowActionList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MenuURL", DBFactoryParameterType.NVarchar, MenuURL);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, RoleId);
                objDBAccess.AddParameterToCommand("@TypeId", DBFactoryParameterType.Int, TypeId);
                objDBAccess.AddParameterToCommand("@RequestId", DBFactoryParameterType.Int, RequestId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WorkFlowModel", FunctionName = "public DataTable WorkFlowActionList(int TypeId, int RequestId, int RoleId, string MenuURL)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable WorkFlowNextRoleList(int TypeId, int ActionId, int RoleId, string MenuURL)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("WorkFlowNextRoleList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MenuURL", DBFactoryParameterType.NVarchar, MenuURL);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, RoleId);
                objDBAccess.AddParameterToCommand("@TypeId", DBFactoryParameterType.Int, TypeId);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WorkFlowModel", FunctionName = "public DataTable WorkFlowNextRoleList(int TypeId, int ActionId, int RoleId, string MenuURL)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable WorkFlowNextUserList(int TypeId, int RoleId, int LoggdInUserId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("WorkFlowNextUserList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, RoleId);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, LoggdInUserId);
                objDBAccess.AddParameterToCommand("@TypeId", DBFactoryParameterType.Int, TypeId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WorkFlowModel", FunctionName = "public DataTable WorkFlowNextUserList(int TypeId, int RoleId, int LoggdInUserId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        //public IList<WorkFlowModel> WorkFlowList()
        //{
        //    IList<WorkFlowModel> workflowlist = new List<WorkFlowModel>();
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("EO_GetListMenuAction", DBFactoryCommandType.PROCEDURE_COMMAND);

        //        DataTable dt = objDBAccess.GetTable();
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            int RowCount = dt.Rows.Count;

        //            foreach (DataRow Dr in dt.Rows)
        //            {
        //                workflowlist.Add(new WorkFlowModel() { MenuNameEn = (Dr["Menu_NameEn"].ToString()), RoleName_EN = (Dr["Role_NameEn"].ToString()), LovCode = Dr["ActionBy_User"].ToString(), nextroleen = Dr["NextRoleEn"].ToString() });
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MenuModel", FunctionName = "WorkFlowList()" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //        dt.Dispose();
        //    }

        //    return workflowlist;
        //}
    }
}