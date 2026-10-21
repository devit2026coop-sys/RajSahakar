using System;
using System.Collections.Generic;
using System.Data;
using Cooperative.DBConnector;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;
using Cooperative.Controllers;
using System.IO;
using System.Net;
using sun.security.util;

namespace Cooperative.Models
{
    public class UserModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        [Key]
        public Int64 UserId { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "User Title - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(10, ErrorMessage = "User Title - Max 10 characters allowed")]
        public string UserTitle { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Officer Name En - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Officer Name En - Max 100 characters allowed")]
        public string OfficerNameEn { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Officer Name HI - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Officer Name Hi - Max 100 characters allowed")]
        public string OfficerNameHi { get; set; }

        //[Column(TypeName = "char"), Required(AllowEmptyStrings = false, ErrorMessage = "Gender - Required")]
        //[DisplayFormat(ConvertEmptyStringToNull = false)]
        //[StringLength(1, ErrorMessage = "Gender - Max 1 character allowed")]
        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        //[Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Mobile No. - Required")]
        //[DisplayFormat(ConvertEmptyStringToNull = false)]
        //[StringLength(15, MinimumLength = 15, ErrorMessage = "Mobile No. - 15 characters allowed")]
        public string MobileNo { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Phone No. - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(15, MinimumLength = 15, ErrorMessage = "Phone No. - 15 characters allowed")]
        public string PhoneNo { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Email Address - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(50, ErrorMessage = "Email Address - 50 characters allowed")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email Address - Not valid")]
        public string EmailId { get; set; }

        //[Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Image Url - Required")]
        //[DisplayFormat(ConvertEmptyStringToNull = false)]
        //[StringLength(250, ErrorMessage = "Image Url - 250 characters allowed")]
        public string ImageUrl { get; set; }

        //[Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Password - Required")]
        //[DisplayFormat(ConvertEmptyStringToNull = false)]
        //[StringLength(50, ErrorMessage = "Password - 50 characters allowed")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[Range(1, Int32.MaxValue)]
        public int StateId { get; set; }

        //[Range(1, Int32.MaxValue)]
        public int DivisionId { get; set; }

        // [Range(1, Int32.MaxValue)]
        public int DistrictId { get; set; }

        // [Range(1, Int32.MaxValue)]
        public int TehsilId { get; set; }

        public int CityId { get; set; }

        public int WardId { get; set; }

        public int BlockId { get; set; }

        public int GPId { get; set; }

        public bool IsRuralUrban { get; set; }

        // [Range(1, Int32.MaxValue)]
        public int VillageId { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Address En - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(500, ErrorMessage = "Address En - 500 characters allowed")]
        public string AddressEn { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Address Hi - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(500, ErrorMessage = "Address Hi - 500 characters allowed")]
        public string AddressHi { get; set; }

        //[Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "PIN - Required")]
        //[DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(6, ErrorMessage = "PIN - 6 characters allowed")]
        [DataType(DataType.PostalCode)]
        public string PIN { get; set; }

        public string CreatedIP { get; set; }

        // [Range(1, Int32.MaxValue)]
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }

        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string UpdatedIP { get; set; }
        public DateTime LastLoginDetail { get; set; }
        public DateTime LastLoginAttempt { get; set; }
        public int NoOfAttempts { get; set; }
        public bool IsFirstLogin { get; set; }
        public DateTime LastLoginTemp { get; set; }
        public int IsActive { get; set; }
        public DateTime DeactivatedDate { get; set; }

        public UserModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public UserModel()
        {

            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public int Add()
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertUser", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, UserId);
                objDBAccess.AddParameterToCommand("@UserTitle", DBFactoryParameterType.Int, UserTitle);
                objDBAccess.AddParameterToCommand("@OfficerNameEn", DBFactoryParameterType.NVarchar, OfficerNameEn);
                objDBAccess.AddParameterToCommand("@OfficerNameHi", DBFactoryParameterType.NVarchar, OfficerNameHi);
                objDBAccess.AddParameterToCommand("@Gender", DBFactoryParameterType.VARCHAR, Gender);
                objDBAccess.AddParameterToCommand("@DateOfBirth", DBFactoryParameterType.Date, DateTime.Now);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, MobileNo);
                objDBAccess.AddParameterToCommand("@PhoneNo", DBFactoryParameterType.NVarchar, PhoneNo);
                objDBAccess.AddParameterToCommand("@EmailId", DBFactoryParameterType.NVarchar, EmailId);
                objDBAccess.AddParameterToCommand("@ImageUrl", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@Password", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@StateId", DBFactoryParameterType.Int, 1);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@IsRuralUrban", DBFactoryParameterType.Bit, IsRuralUrban);
                objDBAccess.AddParameterToCommand("@TehsilId", DBFactoryParameterType.Int, TehsilId);
                objDBAccess.AddParameterToCommand("@CityId", DBFactoryParameterType.Int, CityId);
                objDBAccess.AddParameterToCommand("@WardId", DBFactoryParameterType.Int, WardId);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, BlockId);
                objDBAccess.AddParameterToCommand("@GPId", DBFactoryParameterType.Int, GPId);
                objDBAccess.AddParameterToCommand("@VillageId", DBFactoryParameterType.Int, VillageId);
                objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, AddressEn);
                objDBAccess.AddParameterToCommand("@AddressHi", DBFactoryParameterType.NVarchar, AddressHi);
                objDBAccess.AddParameterToCommand("@Pincode", DBFactoryParameterType.NVarchar, PIN);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, 1);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, 1);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, 1);
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, 1);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, false);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, "Not Mapped");
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserModel", FunctionName = "Add()" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int Update(UserEntity obj)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("inserttraderpaymentdetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, obj.Id);
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserModel", FunctionName = "Update(UserEntity obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return 1;
        }

        public int Delete(UserEntity obj)
        {
            try
            {
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserModel", FunctionName = "Delete(UserEntity obj)" }, ex);
                return 1;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return 1;
        }

        public IList<UserEntity> GetUserAll()
        {
            IList<UserEntity> list = new List<UserEntity>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UserList", DBFactoryCommandType.TEXT_COMMAND);
                //objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, UserID);
                DataTable dt = objDBAccess.GetTable();

                if (dt != null && dt.Rows.Count > 0)
                {
                    int RowCount = dt.Rows.Count;
                    foreach (DataRow Dr in dt.Rows)
                    {
                        list.Add(new UserEntity()
                        {
                            UserId = int.Parse(Dr["UserId"].ToString()),
                            OfficerNameEn = Dr["OfficerNameEn"].ToString(),
                            OfficerNameHi = Dr["OfficerNameEn"].ToString(),
                            UserTitle = Dr["UserTitle"].ToString(),
                            Gender = Dr["Gender"].ToString(),
                            MobileNo = Dr["MobileNo"].ToString(),
                            PhoneNo = Dr["PhoneNo"].ToString(),
                            EmailId = Dr["EmailId"].ToString(),
                            AddressEn = Dr["Address"].ToString(),
                            AddressHi = Dr["AddressHi"].ToString(),
                            IsActive = Convert.ToInt16(Dr["IsActive"]),
                            CreatedBy = Convert.ToInt32(Dr["CreatedBy"]),
                            CreatedIP = Dr["CreatedIP"].ToString(),
                            CreationDate = Convert.ToDateTime(Dr["CreationDate"]),
                            UpdatedBy = Convert.ToInt32(Dr["UpdatedBy"]),
                            UpdatedIP = Dr["UpdatedIP"].ToString(),
                            UpdatedDate = Convert.ToDateTime(Dr["UpdationDate"])

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserModel", FunctionName = "GetUserAll()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return list;
        }

        public UserEntity GetById(int Id)
        {
            UserEntity objUserEntity = new UserEntity();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("select phase phase,prjyear pyear from cm_phase order by prjyear", DBFactoryCommandType.TEXT_COMMAND);
                DataTable dt = objDBAccess.GetTable();
                foreach (DataRow Dr in dt.Rows)
                {
                    objUserEntity = new UserEntity() { UserId = int.Parse(Dr["UserId"].ToString()), OfficerNameEn = Convert.ToString(Dr["OfficerNameEn"]) };
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserModel", FunctionName = "GetById(int Id)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return objUserEntity;
        }

        public DataTable GetMenuDetailsByUserId(int UserId, int RoleId, int UserRoleDeptMappingId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("MenuDetailsByUserId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, RoleId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, UserRoleDeptMappingId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserModel", FunctionName = "GetMenuDetailsByUserId(int UserId,int RoleId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetRolesDetailsByUserId(string WhereClause)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("RolesDetailsByUserId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserModel", FunctionName = "GetRolesDetailsByUserId(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }



        public bool IsUserAuthenticate(long UserId, long RoleId)
        {
            bool Result = false;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UserAuthorizationCheck", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MenuURL", DBFactoryParameterType.VARCHAR, System.Web.HttpContext.Current.Request.Url.AbsolutePath);

                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, RoleId);

                if (Convert.ToInt32(objDBAccess.ExecuteScalarCommand()) == 1)
                    Result = true;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserModel", FunctionName = "bool IsUserAuthenticate(long UserId,long RoleId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public DataTable GetStLoanDataForDashBoard()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ApexLoanFOrDashboard", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.UserModels", FunctionName = "GetStLoanDataForDashBoard()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetUserCountForDashBoard()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UserCountForDashBoard", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.UserModels", FunctionName = "GetStLoanDataForDashBoard()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable GetPacsMemberCountReport()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Pacs_Member_Count_Report", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.UserModels", FunctionName = "GetPacsMemberCountReport()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable GetPacsStatusReport()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Pacs_Status_Report", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.UserModels", FunctionName = "GetPacsStatusReport()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable GetDivisionReport(string FYearID, string MonthId)
        {
            dt.Clear();
            if (FYearID == "undefined")
                FYearID = "0";
            if (MonthId == "undefined")
                MonthId = "0";

            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AuditDivisionDataReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FYID", DBFactoryParameterType.Int, int.Parse(FYearID));
                objDBAccess.AddParameterToCommand("@MONTHID", DBFactoryParameterType.Int, int.Parse(MonthId));
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.UserModels", FunctionName = "GetDivisionReport(string FYearID, string MonthId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable AuditDistrictDataReport(string FYearID, string MonthId, string DivisionId)
        {
            dt.Clear();
            if (FYearID == "undefined")
                FYearID = "48";
            if (MonthId == "undefined")
                MonthId = "6";

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AuditDistrictDataReportByDIVISION_ID", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FYID", DBFactoryParameterType.Int, int.Parse(FYearID));
                objDBAccess.AddParameterToCommand("@MONTHID", DBFactoryParameterType.Int, int.Parse(MonthId));
                objDBAccess.AddParameterToCommand("@DIVISION_ID", DBFactoryParameterType.Int, int.Parse(DivisionId));
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.UserModels", FunctionName = "AuditDistrictDataReport(string FYearID, string MonthId,string DivisionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetWebDashboardData()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetWebDashboardData", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.UserModels", FunctionName = "GetWebDashboardData()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetReturningOfficerList(int LevelId, int PlaceID = 0)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetReturningOfficerList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, LevelId);
                objDBAccess.AddParameterToCommand("@PlaceID", DBFactoryParameterType.Int, PlaceID);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.UserModels", FunctionName = "GetReturningOfficerList(int LevelId, int PlaceID = 0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable Mobile_LoginBySSOId(string SSOId)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("LoginForMobileApp", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.UserModels", FunctionName = "public DataTable Mobile_LoginBySSOId(string SSOId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataSet GetCommonFunctionForDashBoard()
        {
            DataSet dset = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DashboardDataCombo", DBFactoryCommandType.PROCEDURE_COMMAND);
                dset = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserModel", FunctionName = "GetCommonFunctionForDashBoard()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dset;
        }

        public DataTable GetSocietyTypeWiseAuditProgress(string FYearID, string MonthId)
        {
            dt.Clear();
            if (FYearID == "undefined")
                FYearID = "0";
            if (MonthId == "undefined")
                MonthId = "0";

            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyTypeWiseAuditProgress", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FYID", DBFactoryParameterType.Int, int.Parse(FYearID));
                objDBAccess.AddParameterToCommand("@MONTHID", DBFactoryParameterType.Int, int.Parse(MonthId));
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.UserModels", FunctionName = "GetSocietyTypeWiseAuditProgress(string FYearID, string MonthId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetSocietyStatusWiseDataList(string FYearID, string MonthId)
        {
            dt.Clear();
            if (FYearID == "undefined")
                FYearID = "0";
            if (MonthId == "undefined")
                MonthId = "0";

            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyStatusWiseDataList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FYID", DBFactoryParameterType.Int, int.Parse(FYearID));
                objDBAccess.AddParameterToCommand("@MONTHID", DBFactoryParameterType.Int, int.Parse(MonthId));
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.UserModels", FunctionName = "GetSocietyStatusWiseDataList(string FYearID, string MonthId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetDashboardCountSocietyAuditprogrss(string FYearID, string MonthId)
        {
            dt.Clear();
            if (FYearID == "undefined")
                FYearID = "0";
            if (MonthId == "undefined")
                MonthId = "0";

            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetDashboardCountSocietyAuditprogrss", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FYID", DBFactoryParameterType.Int, int.Parse(FYearID));
                objDBAccess.AddParameterToCommand("@MONTHID", DBFactoryParameterType.Int, int.Parse(MonthId));
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.UserModels", FunctionName = "GetDashboardCountSocietyAuditprogrss(string FYearID, string MonthId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        #region Send SMS to User

        public class SMSToUserModel
        {
            public int Id { get; set; }
            public string MessageBy { get; set; }
        }



        public string SaveAndSendSMS(string userId, string message)
        {
            UserEntity UserEntity = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            SMSToUserModel model = new SMSToUserModel();
            model.MessageBy = userId;
            string result = "-1";
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertUpdateMessageSentToUser", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, model.Id);
                objDBAccess.AddParameterToCommand("@MessageTo", DBFactoryParameterType.NVarchar, model.MessageBy);
                objDBAccess.AddParameterToCommand("@MessageBy", DBFactoryParameterType.Int, UserEntity.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@Message", DBFactoryParameterType.NVarchar, message);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, Controllers.COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.VARCHAR, UserEntity.CreatedBy);

                result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                objDBAccess.CloseConnection();


            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "Add(UserEntity obj,List<RoleArray> objRoleListData)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }



        #endregion

        #region Grievance
        public DataTable GrievanceHistoryList(int Grievance)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetGrievanceHistory", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GrievanceId", DBFactoryParameterType.Int, Grievance);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserModels", FunctionName = "GrievanceList(UserEntity U_Entity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int SaveGrievanceApproval(GrievanceApproval _Entity)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertGrievanceApprovalDetail", DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@GrievanceId", DBFactoryParameterType.NVarchar, _Entity.GrievanceId);
                objDBAccess.AddParameterToCommand("@ApprovalStatus", DBFactoryParameterType.NVarchar, _Entity.ApprovalStatus);
                objDBAccess.AddParameterToCommand("@Discription", DBFactoryParameterType.NVarchar, _Entity.Description);
                objDBAccess.AddParameterToCommand("@AttachmentFileName", DBFactoryParameterType.NVarchar, _Entity.AttachmentFileName);
                objDBAccess.AddParameterToCommand("@SSO_ID", DBFactoryParameterType.NVarchar, _Entity.SSO_ID);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, _Entity.CreatedIP);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.NVarchar, _Entity.CreatedBy);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.NVarchar, _Entity.IsActive);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserModels", FunctionName = "public int SaveGrievance(Grievance _Entity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        #endregion Grievance


        #region ThirdPartyServices

        public int IsEMITRATHIRDPARTYPENDENCYSERVICECall(bool IsLive)
        {
            var Result = 0;

            try
            {
                objDBAccess.CreateCommand("Select count(1) from Trn_ThirdPartyServices where ServiceType='EMITRATHIRDPARTYPENDENCYSERVICE' and cast(CreatedDate  as date)=cast(getdate() as date)", DBFactoryCommandType.TEXT_COMMAND);
                objDBAccess.OpenConnection();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                objDBAccess.CloseConnection();
                if (Result == 0)
                {

                    using (WebClient Client = new WebClient())
                    {
                        try
                        {
                            Cooperative.Models.SocietyModel obj = new Cooperative.Models.SocietyModel();

                            var data = Cooperative.Controllers.COMMON_FUNCTION.DataTableToJSONWithJSONNet(obj.GetEmitraServiseData());

                            var postParams = new List<KeyValuePair<string, object>>() { new KeyValuePair<string, object>("data", "{\"data\":" + data + "}") };

                            //Join KVPs into a x-www-formurlencoded string
                            var formString = string.Join("&", postParams.Select(x => string.Format("{0}={1}", x.Key, x.Value)));
                            //output: FirstParamter=FirstValue&SecondParamter=SecondValue

                            //Encode form string to bytes
                            var bytes = System.Text.Encoding.UTF8.GetBytes(formString);
                            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;// | SecurityProtocolType.Tls11;

                            var url = "";
                            if (IsLive)
                            {
                                url = "https://api.sewadwaar.rajasthan.gov.in/app/live/emitra/Prod/webServices/Repository/pushThirdPartyPendencyDataApi/service?client_id=02a79073-c2db-4c84-b810-885fdaa8c54a";
                            }
                            else
                            {
                                url = "https://apitest.sewadwaar.rajasthan.gov.in/app/live/emitra/Prod/webServicesRepositoryUat/pushThirdPartyPendencyDataApi/service?client_id=76b3522f-117a-4797-b7fe-342e40ee9f29";
                            }

                            var http = (HttpWebRequest)WebRequest.Create(new Uri(url));
                            http.Method = "POST";
                            http.ContentType = "application/x-www-form-urlencoded";


                            Stream newStream = http.GetRequestStream();
                            newStream.Write(bytes, 0, bytes.Length);
                            newStream.Close();

                            var response = http.GetResponse();

                            var stream = response.GetResponseStream();
                            var sr = new StreamReader(stream);
                            var content = sr.ReadToEnd();
                            if (content.ToString().Contains("Data uploaded successfully"))
                            {
                                objDBAccess.CreateCommand("Insert into Trn_ThirdPartyServices(ServiceType) values('EMITRATHIRDPARTYPENDENCYSERVICE')", DBFactoryCommandType.TEXT_COMMAND);
                                objDBAccess.OpenConnection();
                                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                                objDBAccess.CloseConnection();
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ILogger objILogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "UserRoleModel", FunctionName = "IsEMITRATHIRDPARTYPENDENCYSERVICECall()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return Result;
        }

        #endregion

        #region MyRegion
        public int objUserRegistrationModel(UserEntity obj)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateProfileData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, obj.UserId);                
                objDBAccess.AddParameterToCommand("@OfficerNameHi", DBFactoryParameterType.NVarchar, obj.OfficerNameHi);
                objDBAccess.AddParameterToCommand("@OfficerNameEn", DBFactoryParameterType.NVarchar, obj.OfficerNameEn);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, obj.MobileNo);
                objDBAccess.AddParameterToCommand("@EmailId", DBFactoryParameterType.NVarchar, obj.EmailId);
                objDBAccess.AddParameterToCommand("@ImageUrl", DBFactoryParameterType.NVarchar, obj.ImageUrl);
                objDBAccess.AddParameterToCommand("@AddressHi", DBFactoryParameterType.NVarchar, obj.AddressHi);
                objDBAccess.AddParameterToCommand("@AddressEn", DBFactoryParameterType.NVarchar, obj.AddressEn);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, _LoggedInUser.CreatedIP);
                
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                return Result > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.User", FunctionName = "objUserRegistrationModel(UserEntity obj)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        #endregion


        public DataTable GetAllDoc(string TableName)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("Get_EncryptedData", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.UserModel", FunctionName = "GetAllDoc()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

       

    }

    [DataContract]
    public class DataPoint
    {
        public DataPoint(string label, double y)
        {
            this.Label = label;
            this.Y = y;
        }
        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "label")]
        public string Label = "";

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public Nullable<double> Y = null;
    }

    public class WebAdminDashboardEntity
    {
        public string TitleEn { get; set; }
        public string TitleHi { get; set; }
        public int CountYearly { get; set; }
        public int CountMonthly { get; set; }
        public int CountToday { get; set; }
    }


    public class UserModelupdatenewssoid
    {
        public int UserId { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string SSOID { get; set; }
        public string Gender { get; set; }
        public string OfficerNameEn { get; set; }
    }

    public class SaveDcrypted
    {
        public Int64 PK_ID { get; set; }
        public string PKID_NAME { get; set; }
        public string ColumnValueName { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string ErrorMsg { get; set; }
        public int IsAbleToSave { get; set; }
    }


}