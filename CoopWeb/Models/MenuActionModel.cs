using Cooperative.DBConnector;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;


namespace Cooperative.Models
{
    public class MenuActionModel
    {

        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public long MenuActionId { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Menu Action Name (English) - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Menu Action Name (English) - 100 characters allowed")]
        public string MenuActionEn { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Menu Action Name (Hindi) - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Menu Action Name (Hindi) - 100 characters allowed")]
        public string MenuActionHi { get; set; }

        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }

        public MenuActionModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public MenuActionModel()
        {
            objDBAccess = new DBFactoryMSSQL();            
        }

        public int InsertMenuAction(MenuActionModel obj)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertMenuAction", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MenuActionId", DBFactoryParameterType.BigInt, obj.MenuActionId);
                objDBAccess.AddParameterToCommand("@MenuActionEn", DBFactoryParameterType.NVarchar, obj.MenuActionEn);
                objDBAccess.AddParameterToCommand("@MenuActionHi", DBFactoryParameterType.NVarchar, obj.MenuActionHi);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, obj.IsActive);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, UpdatedIP);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, CreatedIP);
                
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MenuActionModel", FunctionName = "InsertMenuAction(MenuActionEntity objMenuAction)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetAllMenuAction(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("MenuActionList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MenuActionModel", FunctionName = "GetAllMenuAction(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        //public IList<MenuActionEntity> GetAllMenuAction(MenuActionEntity obj = null)
        //{
        //    IList<MenuActionEntity> listMenuActionEntitylist = new List<MenuActionEntity>();
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("MenuActionList", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        if (obj != null)
        //        {
        //            objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, obj.WhereClause);
        //        };
        //        DataTable dt = objDBAccess.GetTable();                
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            int RowCount = dt.Rows.Count;

        //            foreach (DataRow Dr in dt.Rows)
        //            {
        //                listMenuActionEntitylist.Add(new MenuActionEntity() { MenuActionId = int.Parse(Dr["MenuActionId"].ToString()), MenuActionEn = Dr["MenuActionEn"].ToString(), MenuActionHi = Dr["MenuActionHi"].ToString(), IsActive = Convert.ToInt32(Dr["IsActive"].ToString()), CreationDate = Convert.ToDateTime(Dr["CreationDate"].ToString()) });
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRoleModel", FunctionName = "GetAllRole()" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //        dt.Dispose();
        //    }

        //    return listMenuActionEntitylist;
        //}
        public int ChangeMenuActionStatus(int MenuActionId, int Action)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("MenuActionDeleteStatusChange", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MenuActionId", DBFactoryParameterType.Int, MenuActionId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.Int, Action);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MenuActionModel", FunctionName = "ChangeMenuActionStatus(int MenuActionId,int Action)" }, ex);
                Result = -1;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

    }
}