using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.UI;
using System.Web;

namespace Cooperative.Models
{
    public class MenuModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public long MenuId { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Menu Name (English) - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Menu Name (English) - 100 characters allowed")]
        public string MenuNameEn { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Menu Name (Hindi) - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Menu Name (Hindi) - 100 characters allowed")]
        public string MenuNameHi { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Menu Name (Hindi) - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Menu Name (Hindi) - 100 characters allowed")]
        public string MenuUrl { get; set; }

        public string ActionIds { get; set; }
        public string ParentMenuNameEn { get; set; }
        public string ParentMenuNameHi { get; set; }

        public int SubMenuId { get; set; }


        public int ParentId { get; set; }
        public string MenuIcon { get; set; }

        public int Priority { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }

        public MenuModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public MenuModel()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }



        public int AddMenu(MenuModel obj)
        {
            int Result = 0;
            try
            {
                int level = 1;
                if (obj.ParentId > 0 && obj.SubMenuId == 0)
                {
                    level = 2;
                }
                else if (obj.ParentId > 0 && obj.SubMenuId > 0)
                {
                    obj.ParentId = obj.SubMenuId;
                    level = 3;
                }

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertMenu", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MenuId", DBFactoryParameterType.BigInt, obj.MenuId);
                objDBAccess.AddParameterToCommand("@ParentId", DBFactoryParameterType.Int, obj.ParentId);
                objDBAccess.AddParameterToCommand("@MenuNameEn", DBFactoryParameterType.NVarchar, obj.MenuNameEn);
                objDBAccess.AddParameterToCommand("@MenuNameHi", DBFactoryParameterType.NVarchar, obj.MenuNameHi);
                objDBAccess.AddParameterToCommand("@MenuUrl", DBFactoryParameterType.NVarchar, obj.MenuUrl);
                objDBAccess.AddParameterToCommand("@MenuIcon", DBFactoryParameterType.NVarchar, obj.MenuIcon);
                objDBAccess.AddParameterToCommand("@MenuActionId", DBFactoryParameterType.NVarchar, obj.ActionIds);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, obj.IsActive);
                objDBAccess.AddParameterToCommand("@Priority", DBFactoryParameterType.Int, 1);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, obj.CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, obj.UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, Controllers.COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, Controllers.COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@Level", DBFactoryParameterType.Int, level);


                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MenuModel", FunctionName = "AddMenu(MenuEntity objMenu)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int DeleteMenu(int MenuId)
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
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MenuModel", FunctionName = "DeleteMenu(int MenuId)" }, ex);
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MenuModel", FunctionName = "GetBreadCrumbByMenuURL(string MenuURL)" }, ex);
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MenuModel", FunctionName = "GetBreadCrumb(string lang = 'en - US')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return _data;
        }

        //Bread creum end










        public int ChangeStatus(int MenuId, int Action)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("MenuDeleteStatusChange", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MenuId", DBFactoryParameterType.Int, MenuId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.Int, Action);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MenuModel", FunctionName = "ChangeStatus(int MenuId,int Action)" }, ex);
                Result = -1;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetAllMenuParent(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("MenuListParentId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MenuModel", FunctionName = "GetAllMenuParent(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetAllMenu(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("MenuList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MenuModel", FunctionName = "GetAllMenu(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable GetAllSubMenuList(int ParentId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SubMenuList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ParentId", DBFactoryParameterType.Int, ParentId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MenuModel", FunctionName = "GetAllSubMenuList(" + ParentId + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

    }

}