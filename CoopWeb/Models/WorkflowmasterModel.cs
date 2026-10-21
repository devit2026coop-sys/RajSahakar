using System;
using System.Collections.Generic;
using System.Data;
using Cooperative.DBConnector;
using System.Data.SqlClient;
using System.Web;
using System.IO;

namespace Cooperative.Models
{
    public class WorkflowmasterModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public int workflowmasterid { get; set; }
        public int workflowmasterdtlid { get; set; }
        public int actionid { get; set; }
        public int AgriLovValuesId { get; set; }
        public string AgriLovCode { get; set; }
        public string AgriLovEn { get; set; }
        public string AgriLovHi { get; set; }
        public int nextroleid { get; set; }
        public int actioncode { get; set; }
        public int actionpriority { get; set; }
        public int ModuleId { get; set; }
        public int SubModuleId { get; set; }
        public int ModuleNameEn { get; set; }
        public int ModuleNameHi { get; set; }
        public int SubModuleNameEn { get; set; }
        public int SubModuleNameHi { get; set; }
        public string RoleName_EN { get; set; }
        public string RoleName_HI { get; set; }
        public string LevelNameEn { get; set; }
        public string LevelNameHi { get; set; }
        public int roleid { get; set; }
        public int LevelId { get; set; }
        public DateTime createddate { get; set; }
        public int createdby { get; set; }
        public DateTime updateddate { get; set; }
        public int updatedby { get; set; }
        public int IsActive { get; set; }
        public int IsDelete { get; set; }
        public int SchemeNameId { get; set; }
        public int Stage { get; set; }





        public WorkflowmasterModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public WorkflowmasterModel()
        {
            objDBAccess = new DBFactoryMSSQL();
        }

        public int SaveWorkFlowConfiguration(WorkflowmasterModel Obj)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertWorkFlowMaster", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Workflowmasterid", DBFactoryParameterType.Int, Obj.workflowmasterid);
                objDBAccess.AddParameterToCommand("@SchemeNameId", DBFactoryParameterType.Int, Obj.SchemeNameId);
                objDBAccess.AddParameterToCommand("@ModuleId", DBFactoryParameterType.Int, Obj.ModuleId);
                objDBAccess.AddParameterToCommand("@SubModuleId", DBFactoryParameterType.Int, Obj.SubModuleId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, Obj.LevelId);
                objDBAccess.AddParameterToCommand("@Roleid", DBFactoryParameterType.Int, Obj.roleid);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, Obj.createdby);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, Obj.updatedby);
                objDBAccess.AddParameterToCommand("@Stage", DBFactoryParameterType.Int, Obj.Stage);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WorkflowmasterModel", FunctionName = "SaveWorkFlowConfiguration()" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int SaveWorkFlowDetailsConfiguration(WorkFlowDetails Obj)
        {

            int Result = 0;
            try
            {
                DataTable dtWF = new DataTable("WorFlowDetail");
                dtWF.Columns.Add("Workflowmasterdtlid");
                dtWF.Columns.Add("Workflowmasterid");
                dtWF.Columns.Add("Actionid");
                dtWF.Columns.Add("LevelId");
                dtWF.Columns.Add("Nextroleid");
                dtWF.Columns.Add("Actioncode");
                dtWF.Columns.Add("Actionpriority");
                dtWF.Columns.Add("CompletionDays");
                dtWF.Columns.Add("CreatedBy");
                dtWF.Columns.Add("UpdatedBy");
                dtWF.Columns.Add("actionheadid");
                dtWF.Columns.Add("OfficeId");
                foreach (WorkFlowScopeEntity objwrk in Obj.lstWorkFlowScope)
                {
                    dtWF.Rows.Add(objwrk.workflowmasterdtlid, objwrk.workflowmasterid, objwrk.actionid,objwrk.levelid, objwrk.nextroleid, objwrk.actioncode, objwrk.actionpriority,objwrk.completiondays, createdby, updatedby, objwrk.actionheadid, objwrk.OfficeId);
                    //objDBAccess.OpenConnection();
                    //SqlCommand cmd = objDBAccess.CreateCommand("Agri_InsertWorkFlowMasterDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                    //objDBAccess.AddParameterToCommand("@Workflowmasterdtlid", DBFactoryParameterType.Int, objwrk.workflowmasterdtlid);
                    //objDBAccess.AddParameterToCommand("@Workflowmasterid", DBFactoryParameterType.Int, objwrk.workflowmasterid);
                    //objDBAccess.AddParameterToCommand("@Actionid", DBFactoryParameterType.Int, objwrk.actionid);
                    //objDBAccess.AddParameterToCommand("@Nextroleid", DBFactoryParameterType.Int, objwrk.nextroleid);
                    //objDBAccess.AddParameterToCommand("@Actioncode", DBFactoryParameterType.Int, objwrk.actioncode);
                    //objDBAccess.AddParameterToCommand("@Actionpriority", DBFactoryParameterType.Int, objwrk.actionpriority);
                    //objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, createdby);
                    //objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, updatedby);
                    //Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());


                }

                string xmldata;
                using (StringWriter sw = new StringWriter())
                {
                    dtWF.WriteXml(sw);
                    xmldata = sw.ToString();
                }

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertWorkFlowMasterDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Listdata", DBFactoryParameterType.NVarchar, xmldata);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WorkflowmasterModel", FunctionName = "SaveWorkFlowDetailsConfiguration(WorkFlowDetails Obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable WorkFlowConfigList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetWorkFlowConfigList", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@productCategory", DBFactoryParameterType.NVarchar,"Productcategory");
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WorkflowmasterModel", FunctionName = "WorkFlowConfigList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable SpetModuleWorkFlowConfigList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSeptModuleWorkFlowConfigList", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@productCategory", DBFactoryParameterType.NVarchar,"Productcategory");
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WorkflowmasterModel", FunctionName = "SpetModuleWorkFlowConfigList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable WorkFlowDetails(int workflowmasterid)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetWorkFlowDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@workflowmasterid", DBFactoryParameterType.Int, workflowmasterid);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WorkflowmasterModel", FunctionName = "WorkFlowDetails(int workflowmasterid)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int ChangeStatusWorkflow(int ID, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteStatusworkflow", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ID", DBFactoryParameterType.BigInt, ID);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);

                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "ChangeStatusWorkflow(int ID, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable ModuleList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ModuleList", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@productCategory", DBFactoryParameterType.NVarchar,"Productcategory");
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WorkflowmasterModel", FunctionName = "ModuleList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable ModuleListBySchemeId(int SchemeId )
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ModuleListBySchemeId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SchemeId", DBFactoryParameterType.Int, SchemeId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WorkflowmasterModel", FunctionName = "ModuleList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable LevelList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("LevelList", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@productCategory", DBFactoryParameterType.NVarchar,"Productcategory");
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WorkflowmasterModel", FunctionName = "LevelList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable SeptSubModuleList(int DepartmentId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SeptSubModuleList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DepartmentId", DBFactoryParameterType.Int, DepartmentId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WorkflowmasterModel", FunctionName = "SeptSubModuleList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable SubModuleList(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SubModuleList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WorkflowmasterModel", FunctionName = "ModuleList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable CommonSubModuleList(int SchemeId, int ModuleId )
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CommonSubModuleList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SchemeId", DBFactoryParameterType.NVarchar, SchemeId);
                objDBAccess.AddParameterToCommand("@ModuleId", DBFactoryParameterType.NVarchar, ModuleId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WorkflowmasterModel", FunctionName = "ModuleList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable ActionList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("NewLovValuesByListId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ListCode", DBFactoryParameterType.NVarchar, "Action");
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WorkflowmasterModel", FunctionName = "ActionList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GodownActionList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("NewLovValuesByListId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ListCode", DBFactoryParameterType.NVarchar, "GodownAction");
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WorkflowmasterModel", FunctionName = "GodownActionList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable ActionHeadList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("LovListId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ListCode", DBFactoryParameterType.NVarchar, "Action");
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.WorkflowmasterModel", FunctionName = "ActionHeadList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

    }

    public class WorkFlowDetails
    {
        public List<WorkFlowScopeEntity> lstWorkFlowScope { get; set; }
    }

    #region Model :  WorkFlow scope Details
    public class WorkFlowScopeEntity
    {
        public int workflowmasterdtlid { get; set; }
        public int workflowmasterid { get; set; }
        public int actionid { get; set; }
        public int actioncode { get; set; }
        public int levelid { get; set; }
        public int nextroleid { get; set; }
        public int actionpriority { get; set; }
        public int completiondays { get; set; }
        public int actionheadid { get; set; }
        public int OfficeId { get; set; }
    }
    #endregion
}