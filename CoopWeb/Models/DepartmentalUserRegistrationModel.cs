using System;
using System.Collections.Generic;
using System.Data;
using Cooperative.DBConnector;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Cooperative.Models
{
    public class DepartmentalUserRegistrationModel 
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        //
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

        [Column(TypeName = "char"), Required(AllowEmptyStrings = false, ErrorMessage = "Gender - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(1, ErrorMessage = "Gender - Max 1 character allowed")]
        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Mobile No. - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        //[StringLength(15, MinimumLength = 15, ErrorMessage = "Mobile No. - 15 characters allowed")]
        public string MobileNo { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Phone No. - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        // [StringLength(15, MinimumLength = 15, ErrorMessage = "Phone No. - 15 characters allowed")]
        public string PhoneNo { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Email Address - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(50, ErrorMessage = "Email Address - 50 characters allowed")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email Address - Not valid")]
        public string EmailId { get; set; }

        [Column(TypeName = "nvarchar")] //Required(AllowEmptyStrings = false, ErrorMessage = "Image Url - Required")
        //[DisplayFormat(ConvertEmptyStringToNull = false)]
        //[StringLength(250, ErrorMessage = "Image Url - 250 characters allowed")]
        public string ImageUrl { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Password - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(50, ErrorMessage = "Password - 50 characters allowed")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[Range(1, Int32.MaxValue)]
        public int StateId { get; set; }

        //[Range(0, Int32.MaxValue)]
        public int DivisionId { get; set; }

        //[Range(0, Int32.MaxValue)]
        public int DistrictId { get; set; }

        //[Range(0, Int32.MaxValue)]
        public int TehsilId { get; set; }

        public int CityId { get; set; }

        public int WardId { get; set; }

        public int BlockId { get; set; }

        public int GPId { get; set; }

        public bool IsRuralUrban { get; set; }


        public int VillageId { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Address En - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(500, ErrorMessage = "Address En - 500 characters allowed")]
        public string AddressEn { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Address Hi - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(500, ErrorMessage = "Address Hi - 500 characters allowed")]
        public string AddressHi { get; set; }

        [Column(TypeName = "nvarchar")] //Required(AllowEmptyStrings = false, ErrorMessage = "PIN - Required")
        //[DisplayFormat(ConvertEmptyStringToNull = false)]
        //[StringLength(6, ErrorMessage = "PIN - 6 characters allowed")]
        //[DataType(DataType.PostalCode)]
        public string PIN { get; set; }

        public string SSOID { get; set; }
        //

        public Int64 DEPTCODE { get; set; }
        public Int64 DESIGNATIONCODE { get; set; }
        public Int64 PostId { get; set; }
        public Int64 RoleID { get; set; }
        public string RoleName_EN { get; set; }
        public string RoleName_HI { get; set; }
       
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

        public DepartmentalUserRegistrationModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public DepartmentalUserRegistrationModel()
        {
            objDBAccess = new DBFactoryMSSQL();

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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.DepartmentalUserRegistrationModel", FunctionName = "SelectDepartmentList()" }, ex);
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.DepartmentalUserRegistrationModel", FunctionName = "SelectDesignationList()" }, ex);
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.DepartmentalUserRegistrationModel", FunctionName = "SelectPostList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable SelectRoleList(string WhereClause = " and RoleName_EN = 'Departmental User'")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("RoleList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.DepartmentalUserRegistrationModel", FunctionName = "SelectRoleList(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int Add(DepartmentalUserRegistrationModel obj)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertDepartmenalUser", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, obj.UserId);
                objDBAccess.AddParameterToCommand("@UserTitle", DBFactoryParameterType.Int, obj.UserTitle);
                objDBAccess.AddParameterToCommand("@OfficerNameEn", DBFactoryParameterType.NVarchar, obj.OfficerNameEn);
                objDBAccess.AddParameterToCommand("@OfficerNameHi", DBFactoryParameterType.NVarchar, obj.OfficerNameHi);
                objDBAccess.AddParameterToCommand("@Gender", DBFactoryParameterType.VARCHAR, obj.Gender);
                objDBAccess.AddParameterToCommand("@DateOfBirth", DBFactoryParameterType.Date, DateTime.Now);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, obj.MobileNo);
                objDBAccess.AddParameterToCommand("@PhoneNo", DBFactoryParameterType.NVarchar, obj.PhoneNo);
                objDBAccess.AddParameterToCommand("@EmailId", DBFactoryParameterType.NVarchar, obj.EmailId);
                objDBAccess.AddParameterToCommand("@ImageUrl", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@Password", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@StateId", DBFactoryParameterType.Int, 1);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, obj.DivisionId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, obj.DistrictId);
                objDBAccess.AddParameterToCommand("@IsRuralUrban", DBFactoryParameterType.Bit, obj.IsRuralUrban);
                objDBAccess.AddParameterToCommand("@TehsilId", DBFactoryParameterType.Int, obj.TehsilId);
                objDBAccess.AddParameterToCommand("@CityId", DBFactoryParameterType.Int, obj.CityId);
                objDBAccess.AddParameterToCommand("@WardId", DBFactoryParameterType.Int, obj.WardId);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, obj.BlockId);
                objDBAccess.AddParameterToCommand("@GPId", DBFactoryParameterType.Int, obj.GPId);
                objDBAccess.AddParameterToCommand("@VillageId", DBFactoryParameterType.Int, obj.VillageId);
                objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, obj.AddressEn);
                objDBAccess.AddParameterToCommand("@AddressHi", DBFactoryParameterType.NVarchar, obj.AddressHi);
                objDBAccess.AddParameterToCommand("@Pincode", DBFactoryParameterType.NVarchar, obj.PIN);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, 1);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, 1);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, 1);
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, 1);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, 1);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, "Not Mapped");
                //
                objDBAccess.AddParameterToCommand("@DEPTCODE", DBFactoryParameterType.BigInt, obj.DEPTCODE);
                objDBAccess.AddParameterToCommand("@DESIGNATIONCODE", DBFactoryParameterType.BigInt, obj.DESIGNATIONCODE);
                objDBAccess.AddParameterToCommand("@PostId", DBFactoryParameterType.BigInt, obj.PostId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.BigInt, obj.RoleID);
                //
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.DepartmentalUserRegistrationModel", FunctionName = "Add(DepartmentalUserRegistrationModel obj)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetAll(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UserList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.DepartmentaluserRegistrationModel", FunctionName = " GetAll(string WhereClause)" }, ex);
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