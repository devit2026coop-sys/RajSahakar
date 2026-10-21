using System;
using System.Collections.Generic;
using System.Data;
using Cooperative.DBConnector;
using System.Data.SqlClient;
using System.Web;

namespace Cooperative.Models
{
    public class UserRoleModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public Int64 RoleID { get; set; }
        public string RoleName_EN { get; set; }
        public string RoleName_HI { get; set; }
        public string RoleShortName_EN { get; set; }
        public string RoleShortName_HI { get; set; }
        public Int64 CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }
        public int LevelID { get; set; }
        public bool IsActive { get; set; }
        public string WhereClause { get; set; }
        public int MenuID { get; set; }

        //
        public Int64 RoleRightID { get; set; }
        public long MenuId { get; set; }

        public string ActionIds { get; set; }
        public int UserLevel { get; set; }
        //
        public UserRoleModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }
        public UserRoleModel()
        {
            objDBAccess = new DBFactoryMSSQL();
        }

        public int AddRole(UserRoleEntity objUserRole)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertMasterRole", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RoleID", DBFactoryParameterType.BigInt, objUserRole.RoleID);
                objDBAccess.AddParameterToCommand("@RoleName_EN", DBFactoryParameterType.NVarchar, objUserRole.RoleName_EN);
                objDBAccess.AddParameterToCommand("@RoleName_HI", DBFactoryParameterType.NVarchar, objUserRole.RoleName_HI);
                objDBAccess.AddParameterToCommand("@RoleShortName_EN", DBFactoryParameterType.NVarchar, objUserRole.RoleShortName_EN);
                objDBAccess.AddParameterToCommand("@RoleShortName_HI", DBFactoryParameterType.NVarchar, objUserRole.RoleShortName_HI);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, objUserRole.IsActive);
                objDBAccess.AddParameterToCommand("@LevelID", DBFactoryParameterType.Int, objUserRole.LevelID);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, objUserRole.CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, objUserRole.UpdatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, Controllers.COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, Controllers.COMMON_FUNCTION.GetIpAddress());

                //object o = 0;// 
                Result = int.Parse(objDBAccess.ExecuteScalarCommand().ToString());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRoleModel", FunctionName = " AddRole(UserRoleEntity obj)" }, ex);
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRoleModel", FunctionName = "GetBreadCrumbByMenuURL(string MenuURL)" }, ex);
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRoleModel", FunctionName = "GetBreadCrumb(string lang = 'en - US')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return _data;
        }

        //Bread creum end

        public DataTable GetAllRole(string WhereClause="")

        {
            try
            {
                dt.Clear();
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("RoleList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRoleModel", FunctionName = "GetAllRole(string WhereClause='')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable GetRolesDetailsByUserId(int WhereClause)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("RolesDetailsByUserId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.Int, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRoleModel", FunctionName = "GetMenuDetailsByUserId(int WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public int ChangeRoleStatus(int RoleId, int Action)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("RoleDeleteStatusChange", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, RoleId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.Int, Action);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRoleModel", FunctionName = "ChangeRoleStatus(int RoleId,int Action)" }, ex);
                Result = -1;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetAllMenu(UserRoleModel obj = null, int RoleId = 0)
        {
            dt.Clear();
            try
            {
                if (obj.MenuID > 0)
                {
                    obj.WhereClause = " and ParentId =" + obj.MenuID;
                }
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SelectAllMenu", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, obj != null ? obj.WhereClause : "");
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, RoleId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRoleModel", FunctionName = "GetAllMenu(UserRoleModel obj = null)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetRoleRights(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SelectAllRoleRights", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRoleModel", FunctionName = "GetRoleRights(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int AddAssignRights(List<RolemenuAction> list)
        {
            int Result = 0;

            objDBAccess.OpenConnection();
            if (list != null)
            {
                if (list.Count > 0)
                {
                    try
                    {
                        SqlCommand cmd2 = objDBAccess.CreateCommand("InActiveRoleRights", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.BigInt, list[0].RoleId);
                        Result = objDBAccess.ExecuteCommand();
                    }
                    catch (Exception ex)
                    {
                        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRoleModel", FunctionName = "AddAssignRights(List<RolemenuAction> list)" }, ex);
                    }
                    finally
                    {
                        // objDBAccess.CloseConnection();
                    }
                }
            }
            try
            {
                for (int i = 0; i < list.Count; i++)
                {
                    SqlCommand cmd = objDBAccess.CreateCommand("InsertRoleRights", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@RoleRightID", DBFactoryParameterType.BigInt, RoleRightID);
                    objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.BigInt, list[i].RoleId);
                    objDBAccess.AddParameterToCommand("@MenuId", DBFactoryParameterType.BigInt, list[i].MenuId);
                    objDBAccess.AddParameterToCommand("@ActionID", DBFactoryParameterType.NVarchar, list[i].MenuAction);
                    objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, list[i].IsActive);
                    objDBAccess.AddParameterToCommand("@AddedBy", DBFactoryParameterType.BigInt, CreatedBy);
                    objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                    objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, UpdatedIP);
                    objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, CreatedIP);
                    Result = objDBAccess.ExecuteCommand();


                }


                //object o = 0;// 

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRoleModel", FunctionName = "AddAssignRights(List<RolemenuAction> list)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public DataTable CivilOfficerTypeList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetBiodataOfOfficers", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@StatementType", DBFactoryParameterType.NVarchar, "GetCivilOfficerType");
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CivilListModel", FunctionName = "CivilOfficerTypeList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable DistrictList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetBiodataOfOfficers", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@StatementType", DBFactoryParameterType.NVarchar, "GetDistrict");
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CooperativesModel", FunctionName = "DistrictList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetOfficerCivilList()
        {
            List<NewsEntity> objNewsList = new List<NewsEntity>();
            DataTable dt = new DataTable();

            try
            {
                objDBAccess.CreateCommand("GetBiodataOfOfficers", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@StatementType", DBFactoryParameterType.NVarchar, "GetOfficerCivilList");
                objDBAccess.OpenConnection();
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "UserRoleModel", FunctionName = "GetOfficerCivilList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int InsertCivilList(List<TblCHisttoryDetails> objData, CiviLList _Model)
        {
            DataTable dt = new DataTable();
            var Result = 0;
            int i = 0;

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetBiodataOfOfficers", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@StatementType", DBFactoryParameterType.NVarchar, "insertCivilList");
                objDBAccess.AddParameterToCommand("@EditID", DBFactoryParameterType.Int, _Model.BiodataListId);
                objDBAccess.AddParameterToCommand("@OfficerType", DBFactoryParameterType.NVarchar, _Model.OfficerType);
                objDBAccess.AddParameterToCommand("@NameTitle", DBFactoryParameterType.NVarchar, _Model.NameTitle);
                objDBAccess.AddParameterToCommand("@FirstName", DBFactoryParameterType.NVarchar, _Model.FirstName);
                objDBAccess.AddParameterToCommand("@LastName", DBFactoryParameterType.NVarchar, _Model.LastName);
                objDBAccess.AddParameterToCommand("@OfficerImage", DBFactoryParameterType.VARCHAR, _Model.OfficerImage);
                objDBAccess.AddParameterToCommand("@DateOfBirth", DBFactoryParameterType.Date, _Model.DateOfBirth);
                objDBAccess.AddParameterToCommand("@DateOfJoining", DBFactoryParameterType.Date, _Model.DateOfJoining);
                objDBAccess.AddParameterToCommand("@DateOfRetirement", DBFactoryParameterType.Date, _Model.DateOfRetirement);
                objDBAccess.AddParameterToCommand("@Title", DBFactoryParameterType.NVarchar, _Model.Title);
                objDBAccess.AddParameterToCommand("@HomeDistrict", DBFactoryParameterType.NVarchar, _Model.HomeDistrict);
                objDBAccess.AddParameterToCommand("@Qualification", DBFactoryParameterType.NVarchar, _Model.Qualification);
                objDBAccess.AddParameterToCommand("@PostName", DBFactoryParameterType.NVarchar, _Model.PostName);
                objDBAccess.AddParameterToCommand("@PresentPosting", DBFactoryParameterType.NVarchar, _Model.PresentPosting);
                objDBAccess.AddParameterToCommand("@POSTINGOFF_NAME", DBFactoryParameterType.NVarchar, _Model.POSTINGOFF_NAME);
                objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.Date, _Model.FromDate);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                if (Result > 0)
                {
                    foreach (var obj in objData)
                    {
                        if (obj.District != null && obj.District != "" && obj.District != "0")
                        {
                            try
                            {
                                objDBAccess.OpenConnection();
                                SqlCommand cmd1 = objDBAccess.CreateCommand("GetCivilOfficersList", DBFactoryCommandType.PROCEDURE_COMMAND);
                                objDBAccess.AddParameterToCommand("@District", DBFactoryParameterType.NVarchar, obj.District);
                                objDBAccess.AddParameterToCommand("@ID", DBFactoryParameterType.NVarchar, obj.BiodateOfOffihistoryID);
                                objDBAccess.AddParameterToCommand("@BiodataListId", DBFactoryParameterType.NVarchar, Result);
                                objDBAccess.AddParameterToCommand("@JoingDateCivilList", DBFactoryParameterType.Date, obj.JoingDateCivilList);
                                objDBAccess.AddParameterToCommand("@RelivingDateCivilList", DBFactoryParameterType.Date, obj.RelivingDateCivilList);
                                objDBAccess.AddParameterToCommand("@PostName", DBFactoryParameterType.NVarchar, obj.PostName1);
                                objDBAccess.ExecuteScalarCommand();

                            }
                            catch (Exception ex)
                            {
                                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRoleModel", FunctionName = "InsertUpdateDataCivilList(CiviLListData _Model)" }, ex);
                            }
                            i++;

                        }
                    }
                }


            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRoleModel", FunctionName = "public int SaveCivilList(CiviLList _Entity)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable DeleteCivilList(int Id)
        {
            //List<NewsEntity> objNewsList = new List<NewsEntity>();
            DataTable dt = new DataTable();

            try
            {
                objDBAccess.CreateCommand("GetBiodataOfOfficers", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@StatementType", DBFactoryParameterType.NVarchar, "deleteCivilList");
                objDBAccess.AddParameterToCommand("@DltID", DBFactoryParameterType.NVarchar, Id);
                objDBAccess.OpenConnection();
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "UserRoleModel", FunctionName = "GetOfficerCivilList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataSet GetCivilListforedit(int Id)
        {
            DataSet appData = new DataSet();
            try
            {
                objDBAccess.CreateCommand("GetCivillistdit", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DltID", DBFactoryParameterType.NVarchar, Id);
                objDBAccess.OpenConnection();
                appData = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "UserRoleModel", FunctionName = "GetOfficerCivilList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return appData;
        }


    }
    public class RolemenuAction
    {
        public int RoleId { get; set; }
        public string MenuId { get; set; }
        public string MenuAction { get; set; }
        public int IsActive { get; set; }
    }
    public class CiviLList
    {
        //AddCiviLList
        public int BiodataListId { get; set; }

        public string OfficerType { get; set; }
        public string NameOfOfficer { get; set; }
        public string NameTitle { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OfficerImage { get; set; }
        public string PhoneNo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime DateOfRetirement { get; set; }
        public string Title { get; set; }
        public string HomeDistrict { get; set; }
        public string Qualification { get; set; }
        public string PostName { get; set; }
        public string PresentPosting { get; set; }
        public string POSTINGOFF_NAME { get; set; }
        public DateTime FromDate { get; set; }
        public List<TblCHisttoryDetails> TblCivilHisttoryDetails { get; set; }

    }
    public class TblCHisttoryDetails
    {
        public string District { get; set; }
        public int BiodateOfOffihistoryID { get; set; }
        public DateTime JoingDateCivilList { get; set; }
        public DateTime RelivingDateCivilList { get; set; }
        public string PostName1 { get; set; }
    }

    //added by Pradeep 2024-09-24
    public class UserTypeModel
    {
        public string sAMAccountName { get; set; }
        public List<string> Roles { get; set; }
        public string OldSSOIDs { get; set; }
        public string UserType { get; set; }
        public string DelegateBy { get; set; }
    }




}
