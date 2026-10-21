using CoopDAL;
using Cooperative.Controllers;
using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq.Expressions;

namespace Cooperative.Models
{
    public class UserRegistrationModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public UserEntity Login(string _UserName, string _Password)
        {
            dt = new DataTable();
            UserEntity _UserEntity = new UserEntity();
            dt.Clear();
            try
            {
                objDBAccess = new DBFactoryMSSQL();
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("LoginCheck", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _UserName);
                //objDBAccess.AddParameterToCommand("@Password", DBFactoryParameterType.NVarchar, Encryption.Encrypt(_Password, EncryptionKey.PasswordHash));
                objDBAccess.AddParameterToCommand("@Password", DBFactoryParameterType.NVarchar, _Password);
                dt = objDBAccess.GetTable();

                if (dt != null && dt.Rows.Count > 0)
                {
                    // by n saini
                    ////////_UserEntity.UserId = Convert.ToInt32(dt.Rows[0]["UserId"]);
                    ////////_UserEntity.UserTitle = Convert.ToString(dt.Rows[0]["UserTitle"]);
                    ////////_UserEntity.OfficerNameEn = Convert.ToString(dt.Rows[0]["OfficerNameEn"]);
                    ////////_UserEntity.OfficerNameHi = Convert.ToString(dt.Rows[0]["OfficerNameHi"]);
                    ////////_UserEntity.Gender = Convert.ToString(dt.Rows[0]["Gender"]);
                    ////////_UserEntity.MobileNo = Convert.ToString(dt.Rows[0]["MobileNo"]);
                    ////////_UserEntity.PhoneNo = Convert.ToString(dt.Rows[0]["PhoneNo"]);
                    ////////_UserEntity.EmailId = Convert.ToString(dt.Rows[0]["EmailId"]);
                    ////////_UserEntity.ImageUrl = Convert.ToString(dt.Rows[0]["ImageUrl"]);
                    //////////_UserEntity.StateId = Convert.ToInt32(dt.Rows[0]["StateId"]);
                    //////////_UserEntity.DivisionId = Convert.ToInt32(dt.Rows[0]["DivisionId"]);
                    //////////_UserEntity.DistrictId = Convert.ToInt32(dt.Rows[0]["DistrictId"]);
                    //////////_UserEntity.TehsilId = Convert.ToInt32(dt.Rows[0]["TehsilId"]);
                    //////////_UserEntity.IsRuralUrban = Convert.ToBoolean(dt.Rows[0]["IsRuralArban"]);
                    //////////_UserEntity.VillageId = Convert.ToInt32(dt.Rows[0]["VillageId"]);
                    ////////_UserEntity.AddressEn = Convert.ToString(dt.Rows[0]["Address"]);
                    ////////_UserEntity.AddressHi = Convert.ToString(dt.Rows[0]["AddressHi"]);
                    ////////_UserEntity.PIN = Convert.ToString(dt.Rows[0]["Pincode"]);
                    ////////_UserEntity.IsActive = Convert.ToInt32(dt.Rows[0]["IsActive"]);
                    ////////_UserEntity.SSOID = Convert.ToString(dt.Rows[0]["SSO_ID"]);
                    //////////_UserEntity.CityId = Convert.ToInt32(dt.Rows[0]["CityId"]);
                    //////////_UserEntity.WardId = Convert.ToInt32(dt.Rows[0]["WardId"]);
                    //////////_UserEntity.BlockId = Convert.ToInt32(dt.Rows[0]["BlockId"]);
                    //////////_UserEntity.GPId = Convert.ToInt32(dt.Rows[0]["GPId"]);
                    ////////_UserEntity.RoleId = Convert.ToInt32(dt.Rows[0]["RoleID"]);
                    ////////_UserEntity.RoleName_EN = Convert.ToString(dt.Rows[0]["RoleName_EN"]);
                    ////////_UserEntity.RoleName_HI = Convert.ToString(dt.Rows[0]["RoleName_HI"]);
                    ////////_UserEntity.RoleShortName_EN = Convert.ToString(dt.Rows[0]["RoleShortName_EN"]);
                    ////////_UserEntity.RoleShortName_HI = Convert.ToString(dt.Rows[0]["RoleShortName_HI"]);
                    _UserEntity.UserId = Convert.ToInt32(dt.Rows[0]["UserId"]);
                    _UserEntity.UserTitle = Convert.ToString(dt.Rows[0]["UserTitle"]);
                    _UserEntity.OfficerNameEn = Convert.ToString(dt.Rows[0]["OfficerNameEn"]);
                    _UserEntity.OfficerNameHi = Convert.ToString(dt.Rows[0]["OfficerNameHi"]);
                    _UserEntity.Gender = Convert.ToString(dt.Rows[0]["Gender"]);
                    _UserEntity.MobileNo = Convert.ToString(dt.Rows[0]["MobileNo"]);
                    _UserEntity.PhoneNo = Convert.ToString(dt.Rows[0]["PhoneNo"]);
                    _UserEntity.EmailId = Convert.ToString(dt.Rows[0]["EmailId"]);
                    _UserEntity.ImageUrl = Convert.ToString(dt.Rows[0]["ImageUrl"]);
                    //_UserEntity.StateId = Convert.ToInt32(dt.Rows[0]["StateId"]);
                    _UserEntity.DivisionId = Convert.ToInt32(dt.Rows[0]["DivisionId"]);
                    _UserEntity.DistrictId = Convert.ToInt32(dt.Rows[0]["DistrictId"]);
                    //_UserEntity.TehsilId = Convert.ToInt32(dt.Rows[0]["TehsilId"]);
                    //_UserEntity.IsRuralUrban = Convert.ToBoolean(dt.Rows[0]["IsRuralArban"]);
                    _UserEntity.LevelId = Convert.ToInt32(dt.Rows[0]["LevelId"]);
                    _UserEntity.AddressEn = Convert.ToString(dt.Rows[0]["Address"]);
                    _UserEntity.AddressHi = Convert.ToString(dt.Rows[0]["AddressHi"]);
                    _UserEntity.PIN = Convert.ToString(dt.Rows[0]["Pincode"]);
                    _UserEntity.IsActive = Convert.ToInt32(dt.Rows[0]["IsActive"]);
                    _UserEntity.SSOID = Convert.ToString(dt.Rows[0]["SSO_ID"]);
                    _UserEntity.CityId = Convert.ToInt32(dt.Rows[0]["CityId"]);
                    _UserEntity.WardId = Convert.ToInt32(dt.Rows[0]["WardId"]);
                    _UserEntity.PanchayatSamitiId = Convert.ToInt32(dt.Rows[0]["PanchayatSamitiId"]);
                    _UserEntity.GPId = Convert.ToInt32(dt.Rows[0]["GPId"]);
                    _UserEntity.RoleId = Convert.ToInt32(dt.Rows[0]["RoleID"]);
                    _UserEntity.RoleName_EN = Convert.ToString(dt.Rows[0]["RoleName_EN"]);
                    _UserEntity.RoleName_HI = Convert.ToString(dt.Rows[0]["RoleName_HI"]);
                    _UserEntity.RoleShortName_EN = Convert.ToString(dt.Rows[0]["RoleShortName_EN"]);
                    _UserEntity.RoleShortName_HI = Convert.ToString(dt.Rows[0]["RoleShortName_HI"]);
                    _UserEntity.UserRoleDeptMappingId= Convert.ToInt32(dt.Rows[0]["MappingID"]);
                    _UserEntity.DashBoardURL = dt.Rows[0]["DashBoardURL"].ToString();
                    _UserEntity.WarehouseId = Convert.ToInt32(dt.Rows[0]["WarehouseId"].ToString());
                    _UserEntity.OrganizationId = Convert.ToInt32(dt.Rows[0]["OrganizationId"].ToString());
                    _UserEntity.PlantId = Convert.ToInt32(dt.Rows[0]["ProcessingUnitId"].ToString());
                    _UserEntity.DepotId = Convert.ToInt32(dt.Rows[0]["DepotId"].ToString());
                    _UserEntity.PLDBId = Convert.ToInt32(dt.Rows[0]["PLDBId"].ToString());
                    _UserEntity.PLDBBrancchId = Convert.ToInt32(dt.Rows[0]["PLDBBrancchId"].ToString());
                    _UserEntity.ActId = dt.Rows[0]["ActId"].ToString();
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "Login(string _UserName, string _Password)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return _UserEntity;
        }

        public UserEntity Login(string _UserName, string _Password, string SSOID)
        {
            dt = new DataTable();
            int Result = 0;
            DBFactoryMSSQL objDBAccess2 = new DBFactoryMSSQL();
            UserEntity _UserEntity = new UserEntity();
            dt.Clear();
            try
            {
                objDBAccess = new DBFactoryMSSQL();

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("LoginCheckWithSSO", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _UserName);
                objDBAccess.AddParameterToCommand("@Password", DBFactoryParameterType.NVarchar, Encryption.Encrypt(_Password, EncryptionKey.PasswordHash));
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOID);
                dt = objDBAccess.GetTable();

                if (dt != null && dt.Rows.Count > 0)
                {
                    _UserEntity.UserId = Convert.ToInt32(dt.Rows[0]["UserId"]);
                    _UserEntity.UserTitle = Convert.ToString(dt.Rows[0]["UserTitle"]);
                    _UserEntity.OfficerNameEn = Convert.ToString(dt.Rows[0]["OfficerNameEn"]);
                    _UserEntity.OfficerNameHi = Convert.ToString(dt.Rows[0]["OfficerNameHi"]);
                    _UserEntity.Gender = Convert.ToString(dt.Rows[0]["Gender"]);
                    _UserEntity.MobileNo = Convert.ToString(dt.Rows[0]["MobileNo"]);
                    _UserEntity.PhoneNo = Convert.ToString(dt.Rows[0]["PhoneNo"]);
                    _UserEntity.EmailId = Convert.ToString(dt.Rows[0]["EmailId"]);
                    _UserEntity.ImageUrl = Convert.ToString(dt.Rows[0]["ImageUrl"]);
                    //_UserEntity.StateId = Convert.ToInt32(dt.Rows[0]["StateId"]);
                    //_UserEntity.DivisionId = Convert.ToInt32(dt.Rows[0]["DivisionId"]);
                    //_UserEntity.DistrictId = Convert.ToInt32(dt.Rows[0]["DistrictId"]);
                    //_UserEntity.TehsilId = Convert.ToInt32(dt.Rows[0]["TehsilId"]);
                    //_UserEntity.IsRuralUrban = Convert.ToBoolean(dt.Rows[0]["IsRuralArban"]);
                    //_UserEntity.VillageId = Convert.ToInt32(dt.Rows[0]["VillageId"]);
                    _UserEntity.AddressEn = Convert.ToString(dt.Rows[0]["Address"]);
                    _UserEntity.AddressHi = Convert.ToString(dt.Rows[0]["AddressHi"]);
                    _UserEntity.PIN = Convert.ToString(dt.Rows[0]["Pincode"]);
                    _UserEntity.IsActive = Convert.ToInt32(dt.Rows[0]["IsActive"]);
                    _UserEntity.SSOID = Convert.ToString(dt.Rows[0]["SSO_ID"]);
                    //_UserEntity.CityId = Convert.ToInt32(dt.Rows[0]["CityId"]);
                    //_UserEntity.WardId = Convert.ToInt32(dt.Rows[0]["WardId"]);
                    //_UserEntity.BlockId = Convert.ToInt32(dt.Rows[0]["BlockId"]);
                    //_UserEntity.GPId = Convert.ToInt32(dt.Rows[0]["GPId"]);
                    //_UserEntity.RoleId = Convert.ToInt32(dt.Rows[0]["RoleID"]);
                    //_UserEntity.RoleName_EN = Convert.ToString(dt.Rows[0]["RoleName_EN"]);
                    //_UserEntity.RoleName_HI = Convert.ToString(dt.Rows[0]["RoleName_HI"]);
                    //_UserEntity.RoleShortName_EN = Convert.ToString(dt.Rows[0]["RoleShortName_EN"]);
                    //_UserEntity.RoleShortName_HI = Convert.ToString(dt.Rows[0]["RoleShortName_HI"]);
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    objDBAccess2.OpenConnection();
                    SqlCommand cmdForSSOUpdate = objDBAccess2.CreateCommand("SetSSOByUserCredentials", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess2.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _UserName);
                    objDBAccess2.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOID);
                    object ResultForSSOUpdate = objDBAccess2.ExecuteScalarCommand();
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                    SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();
                    ssoclient.RoleUserMapping("RAJSAHKAR.TEST", "Rajasthan@12", SSOID.ToUpper().Trim(), "Department", "ADD");
                }

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "Login(string _UserName, string _Password)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                objDBAccess2.CloseConnection();
                dt.Dispose();
            }
            return _UserEntity;
        }

        public UserEntity Login(string SSOID,int RoleId=0)
        {
            SSOID = SSOID ?? "x";

             dt = new DataTable();
            UserEntity _UserEntity = new UserEntity();
            dt.Clear();
            try
            {
                objDBAccess = new DBFactoryMSSQL();
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("LoginCheckBySSO", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOID);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, RoleId);


                dt = objDBAccess.GetTable();

                if (dt != null && dt.Rows.Count > 0)
                {
                    _UserEntity.UserId = Convert.ToInt32(dt.Rows[0]["UserId"]);
                    _UserEntity.UserTitle = Convert.ToString(dt.Rows[0]["UserTitle"]);
                    _UserEntity.OfficerNameEn = Convert.ToString(dt.Rows[0]["OfficerNameEn"]);
                    _UserEntity.OfficerNameHi = Convert.ToString(dt.Rows[0]["OfficerNameHi"]);
                    _UserEntity.Gender = Convert.ToString(dt.Rows[0]["Gender"]);
                    _UserEntity.MobileNo = Convert.ToString(dt.Rows[0]["MobileNo"]);
                    _UserEntity.PhoneNo = Convert.ToString(dt.Rows[0]["PhoneNo"]);
                    _UserEntity.EmailId = Convert.ToString(dt.Rows[0]["EmailId"]);
                    _UserEntity.ImageUrl = Convert.ToString(dt.Rows[0]["ImageUrl"]);
                    //_UserEntity.StateId = Convert.ToInt32(dt.Rows[0]["StateId"]);
                    _UserEntity.DivisionId = Convert.ToInt32(dt.Rows[0]["DivisionId"]);
                    _UserEntity.DistrictId = Convert.ToInt32(dt.Rows[0]["DistrictId"]);
                    //_UserEntity.TehsilId = Convert.ToInt32(dt.Rows[0]["TehsilId"]);
                    //_UserEntity.IsRuralUrban = Convert.ToBoolean(dt.Rows[0]["IsRuralArban"]);
                    _UserEntity.LevelId = Convert.ToInt32(dt.Rows[0]["LevelId"]);
                    _UserEntity.AddressEn = Convert.ToString(dt.Rows[0]["Address"]);
                    _UserEntity.AddressHi = Convert.ToString(dt.Rows[0]["AddressHi"]);
                    _UserEntity.PIN = Convert.ToString(dt.Rows[0]["Pincode"]);
                    _UserEntity.IsActive = Convert.ToInt32(dt.Rows[0]["IsActive"]);
                    _UserEntity.SSOID = Convert.ToString(dt.Rows[0]["SSO_ID"]);
                    _UserEntity.CityId = Convert.ToInt32(dt.Rows[0]["CityId"]);
                    _UserEntity.WardId = Convert.ToInt32(dt.Rows[0]["WardId"]);
                    _UserEntity.PanchayatSamitiId = Convert.ToInt32(dt.Rows[0]["PanchayatSamitiId"]);
                    _UserEntity.GPId = Convert.ToInt32(dt.Rows[0]["GPId"]);
                    _UserEntity.RoleId = Convert.ToInt32(dt.Rows[0]["RoleID"]);
                    _UserEntity.RoleName_EN = Convert.ToString(dt.Rows[0]["RoleName_EN"]);
                    _UserEntity.RoleName_HI = Convert.ToString(dt.Rows[0]["RoleName_HI"]);
                    _UserEntity.RoleShortName_EN = Convert.ToString(dt.Rows[0]["RoleShortName_EN"]);
                    _UserEntity.RoleShortName_HI = Convert.ToString(dt.Rows[0]["RoleShortName_HI"]);
                    _UserEntity.UserRoleDeptMappingId = Convert.ToInt32(dt.Rows[0]["MappingID"]);
                    _UserEntity.DashBoardURL = dt.Rows[0]["DashBoardURL"].ToString();
                    _UserEntity.WarehouseId = Convert.ToInt32(dt.Rows[0]["WarehouseId"].ToString());
                    _UserEntity.PlantId = Convert.ToInt32(dt.Rows[0]["ProcessingUnitId"].ToString());
                    _UserEntity.DepotId = Convert.ToInt32(dt.Rows[0]["DepotId"].ToString());
                    _UserEntity.OrganizationId = Convert.ToInt32(dt.Rows[0]["OrganizationId"].ToString());
                    _UserEntity.PLDBId = Convert.ToInt32(dt.Rows[0]["PLDBId"].ToString());
                    _UserEntity.PLDBBrancchId = Convert.ToInt32(dt.Rows[0]["PLDBBrancchId"].ToString());
                    _UserEntity.ActId = dt.Rows[0]["ActId"].ToString();
                    _UserEntity.DISTRICT_ENG = dt.Rows[0]["DISTRICT_ENG"].ToString();
                    _UserEntity.DISTRICT_MANGAL = dt.Rows[0]["DISTRICT_MANGAL"].ToString();
                    _UserEntity.DIVISION_ENG = dt.Rows[0]["DIVISION_ENG"].ToString();
                    _UserEntity.DIVISION_MANGAL = dt.Rows[0]["DIVISION_MANGAL"].ToString();
                    _UserEntity.ResionName = dt.Rows[0]["ResionName"].ToString();
                    _UserEntity.GccId = Convert.ToInt32(dt.Rows[0]["GccId"]);
                    _UserEntity.NccfId = Convert.ToInt32(dt.Rows[0]["NccfId"]);//madhuri

                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "Login(string _UserName, string _Password)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return _UserEntity;
        }

        public UserEntity LoginBySsoRole(string SSOID, string UserRoleDeptMappingId)
        {
            dt = new DataTable();
            UserEntity _UserEntity = new UserEntity();
            dt.Clear();
            try
            {
                objDBAccess = new DBFactoryMSSQL();
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("LoginCheckBySSORoleId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOID);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, UserRoleDeptMappingId);
                

                dt = objDBAccess.GetTable();

                if (dt != null && dt.Rows.Count > 0)
                {
                    _UserEntity.UserId = Convert.ToInt32(dt.Rows[0]["UserId"]);
                    _UserEntity.UserTitle = Convert.ToString(dt.Rows[0]["UserTitle"]);
                    _UserEntity.OfficerNameEn = Convert.ToString(dt.Rows[0]["OfficerNameEn"]);
                    _UserEntity.OfficerNameHi = Convert.ToString(dt.Rows[0]["OfficerNameHi"]);
                    _UserEntity.Gender = Convert.ToString(dt.Rows[0]["Gender"]);
                    _UserEntity.MobileNo = Convert.ToString(dt.Rows[0]["MobileNo"]);
                    _UserEntity.PhoneNo = Convert.ToString(dt.Rows[0]["PhoneNo"]);
                    _UserEntity.EmailId = Convert.ToString(dt.Rows[0]["EmailId"]);
                    _UserEntity.ImageUrl = Convert.ToString(dt.Rows[0]["ImageUrl"]);                    
                    _UserEntity.DivisionId = Convert.ToInt32(dt.Rows[0]["DivisionId"]);
                    _UserEntity.DistrictId = Convert.ToInt32(dt.Rows[0]["DistrictId"]);                    
                    _UserEntity.LevelId = Convert.ToInt32(dt.Rows[0]["LevelId"]);
                    _UserEntity.AddressEn = Convert.ToString(dt.Rows[0]["Address"]);
                    _UserEntity.AddressHi = Convert.ToString(dt.Rows[0]["AddressHi"]);
                    _UserEntity.PIN = Convert.ToString(dt.Rows[0]["Pincode"]);
                    _UserEntity.IsActive = Convert.ToInt32(dt.Rows[0]["IsActive"]);
                    _UserEntity.SSOID = Convert.ToString(dt.Rows[0]["SSO_ID"]);
                    _UserEntity.CityId = Convert.ToInt32(dt.Rows[0]["CityId"]);
                    _UserEntity.WardId = Convert.ToInt32(dt.Rows[0]["WardId"]);
                    _UserEntity.PanchayatSamitiId = Convert.ToInt32(dt.Rows[0]["PanchayatSamitiId"]);
                    _UserEntity.GPId = Convert.ToInt32(dt.Rows[0]["GPId"]);
                    _UserEntity.RoleId = Convert.ToInt32(dt.Rows[0]["RoleID"]);
                    _UserEntity.RoleName_EN = Convert.ToString(dt.Rows[0]["RoleName_EN"]);
                    _UserEntity.RoleName_HI = Convert.ToString(dt.Rows[0]["RoleName_HI"]);
                    _UserEntity.RoleShortName_EN = Convert.ToString(dt.Rows[0]["RoleShortName_EN"]);
                    _UserEntity.RoleShortName_HI = Convert.ToString(dt.Rows[0]["RoleShortName_HI"]);
                    _UserEntity.UserRoleDeptMappingId = Convert.ToInt32(dt.Rows[0]["MappingID"]);
                    _UserEntity.DashBoardURL = dt.Rows[0]["DashBoardURL"].ToString();
                    _UserEntity.WarehouseId = Convert.ToInt32(dt.Rows[0]["WarehouseId"].ToString());
                    _UserEntity.PlantId = Convert.ToInt32(dt.Rows[0]["ProcessingUnitId"].ToString());
                    _UserEntity.DepotId = Convert.ToInt32(dt.Rows[0]["DepotId"].ToString());
                    _UserEntity.OrganizationId = Convert.ToInt32(dt.Rows[0]["OrganizationId"].ToString());
                    _UserEntity.PLDBId = Convert.ToInt32(dt.Rows[0]["PLDBId"].ToString());
                    _UserEntity.PLDBBrancchId = Convert.ToInt32(dt.Rows[0]["PLDBBrancchId"].ToString());
                    _UserEntity.ActId = dt.Rows[0]["ActId"].ToString();
                    _UserEntity.DISTRICT_ENG = dt.Rows[0]["DISTRICT_ENG"].ToString();
                    _UserEntity.DISTRICT_MANGAL = dt.Rows[0]["DISTRICT_MANGAL"].ToString();
                    _UserEntity.DIVISION_ENG = dt.Rows[0]["DIVISION_ENG"].ToString();
                    _UserEntity.DIVISION_MANGAL = dt.Rows[0]["DIVISION_MANGAL"].ToString();
                    _UserEntity.ResionName = dt.Rows[0]["ResionName"].ToString();
                    _UserEntity.GccId= Convert.ToInt32(dt.Rows[0]["GccId"]);

                    _UserEntity.NccfId = Convert.ToInt32(dt.Rows[0]["NccfId"]);//madhuri

                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "LoginBySsoRole(string SSOID, string RoleId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return _UserEntity;
        }


        public UserRegistrationModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public UserRegistrationModel()
        {
        }

        //public int Add(UserEntity obj)
        //{
        //    int Result = -1;
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("InsertUser", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, obj.UserId);
        //        objDBAccess.AddParameterToCommand("@UserTitle", DBFactoryParameterType.Int, obj.UserTitle);
        //        objDBAccess.AddParameterToCommand("@OfficerNameEn", DBFactoryParameterType.NVarchar, obj.OfficerNameEn);
        //        objDBAccess.AddParameterToCommand("@OfficerNameHi", DBFactoryParameterType.NVarchar, obj.OfficerNameHi);
        //        objDBAccess.AddParameterToCommand("@Gender", DBFactoryParameterType.VARCHAR, obj.Gender);
        //        objDBAccess.AddParameterToCommand("@DateOfBirth", DBFactoryParameterType.Date, DateTime.Now);
        //        objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, obj.MobileNo);
        //        objDBAccess.AddParameterToCommand("@PhoneNo", DBFactoryParameterType.NVarchar, obj.PhoneNo);
        //        objDBAccess.AddParameterToCommand("@EmailId", DBFactoryParameterType.NVarchar, obj.EmailId);
        //        objDBAccess.AddParameterToCommand("@ImageUrl", DBFactoryParameterType.NVarchar, "");
        //        objDBAccess.AddParameterToCommand("@Password", DBFactoryParameterType.NVarchar, "");
        //        objDBAccess.AddParameterToCommand("@StateId", DBFactoryParameterType.Int, 1);
        //        objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, obj.DivisionId);
        //        objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, obj.DistrictId);
        //        objDBAccess.AddParameterToCommand("@IsRuralUrban", DBFactoryParameterType.Bit, obj.IsRuralUrban);
        //        objDBAccess.AddParameterToCommand("@TehsilId", DBFactoryParameterType.Int, obj.TehsilId);
        //        objDBAccess.AddParameterToCommand("@CityId", DBFactoryParameterType.Int, obj.CityId);
        //        objDBAccess.AddParameterToCommand("@WardId", DBFactoryParameterType.Int, obj.WardId);
        //        objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, obj.BlockId);
        //        objDBAccess.AddParameterToCommand("@GPId", DBFactoryParameterType.Int, obj.GPId);
        //        objDBAccess.AddParameterToCommand("@VillageId", DBFactoryParameterType.Int, obj.VillageId);
        //        objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, obj.AddressEn);
        //        objDBAccess.AddParameterToCommand("@AddressHi", DBFactoryParameterType.NVarchar, obj.AddressHi);
        //        objDBAccess.AddParameterToCommand("@Pincode", DBFactoryParameterType.NVarchar, obj.PIN);
        //        objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, 1);
        //        objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, 1);
        //        objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, 1);
        //        objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, 1);
        //        objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, 1);
        //        objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, "Not Mapped");
        //        Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "Add(UserEntity obj)" }, ex);

        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return Result;
        //}


        public int Add(UserEntity obj, List<RoleArray> objRoleListData)
        {
            int Result = -1;
            int ResultFinal = -1;
            try
            {
                obj.ActId = obj.ActId != null ? obj.ActId : "0";
                if (obj.UserId == 0) { 
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("InsertUser", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, obj.UserId);
                    objDBAccess.AddParameterToCommand("@UserTitle", DBFactoryParameterType.Int, obj.UserTitle);
                    objDBAccess.AddParameterToCommand("@OfficerNameEn", DBFactoryParameterType.NVarchar, obj.OfficerNameEn);
                    objDBAccess.AddParameterToCommand("@OfficerNameHi", DBFactoryParameterType.NVarchar, obj.OfficerNameHi);
                    objDBAccess.AddParameterToCommand("@Gender", DBFactoryParameterType.VARCHAR, obj.Gender);
                    objDBAccess.AddParameterToCommand("@DateOfBirth", DBFactoryParameterType.Date, DateTime.Now);
                    objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, obj.MobileNo);
                    objDBAccess.AddParameterToCommand("@PhoneNo", DBFactoryParameterType.NVarchar, obj.PhoneNo);
                    objDBAccess.AddParameterToCommand("@EmailId", DBFactoryParameterType.NVarchar, obj.EmailId);
                    objDBAccess.AddParameterToCommand("@ImageUrl", DBFactoryParameterType.NVarchar, obj.ImageUrl);
                    //objDBAccess.AddParameterToCommand("@Password", DBFactoryParameterType.NVarchar, Encryption.Encrypt(obj.Password, EncryptionKey.PasswordHash));
                    objDBAccess.AddParameterToCommand("@StateId", DBFactoryParameterType.Int, 6);
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
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, obj.CreatedBy);
                    objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, "S");
                    objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, obj.UpdatedBy);
                    objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, "A");
                    objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, obj.IsActive);
                    objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, obj.SSOID);
                    objDBAccess.AddParameterToCommand("@ResionId", DBFactoryParameterType.Int, obj.ResionId);
                    objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, obj.ActId.Length > 1 ? obj.ActId.Substring(0, obj.ActId.Length - 1) : "0");
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                    objDBAccess.CloseConnection();
                }
                else
                {
                    Result = Convert.ToInt32(obj.UserId);
                }
                if (Result > 0)
                {
                    foreach (var objD in objRoleListData)
                    {
                        obj.WorkScopResionIds = obj.WorkScopResionIds != null ? obj.WorkScopResionIds : "0";
                        objDBAccess.OpenConnection();
                        SqlCommand cmd1 = objDBAccess.CreateCommand("InsertUserRoleDeptMapping", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@DEPTCODE", DBFactoryParameterType.Int, objD.DepartmentId);
                        objDBAccess.AddParameterToCommand("@DESIGNATIONCODE", DBFactoryParameterType.Int, objD.DesignationId);
                        objDBAccess.AddParameterToCommand("@PostId", DBFactoryParameterType.Int, objD.PostId);
                        objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, objD.RoleId);
                        objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, Result);
                        objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, objD.IsActive);
                        objDBAccess.AddParameterToCommand("@AddedBy", DBFactoryParameterType.Int, obj.CreatedBy);
                        objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, 0);
                        objDBAccess.AddParameterToCommand("@UnitId", DBFactoryParameterType.Int, objD.UnitId);
                        objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, objD.LevelId);
                        objDBAccess.AddParameterToCommand("@DesignationLevelMappingId", DBFactoryParameterType.Int, objD.DesignationLevelMappingId);
                        objDBAccess.AddParameterToCommand("@OrganizationLevelsMappingID", DBFactoryParameterType.Int, objD.OrganizationLevelsMappingID);
                        objDBAccess.AddParameterToCommand("@OfficeId", DBFactoryParameterType.Int, objD.OfficeId);
                        objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, objD.DivisionId);
                        objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, objD.DistrictId);
                        objDBAccess.AddParameterToCommand("@TehsiId", DBFactoryParameterType.Int, objD.TehsilId);
                        objDBAccess.AddParameterToCommand("@CityId", DBFactoryParameterType.Int, objD.CityId);
                        objDBAccess.AddParameterToCommand("@WardId", DBFactoryParameterType.Int, objD.WardId);
                        objDBAccess.AddParameterToCommand("@PanchayatSamitiId", DBFactoryParameterType.Int, objD.PanchayatSamitiId);
                        objDBAccess.AddParameterToCommand("@GramPanchayatId", DBFactoryParameterType.Int, objD.GramPanchayatId);
                        objDBAccess.AddParameterToCommand("@VillageId", DBFactoryParameterType.Int, objD.VillageId);
                        objDBAccess.AddParameterToCommand("@IsRuralArban", DBFactoryParameterType.Bit, obj.IsRuralUrban);
                        objDBAccess.AddParameterToCommand("@MappingFromId", DBFactoryParameterType.BigInt, 0);
                        objDBAccess.AddParameterToCommand("@ResionId", DBFactoryParameterType.NVarchar, obj.ResionId);
                        objDBAccess.AddParameterToCommand("@WorkScopResionIds", DBFactoryParameterType.NVarchar, obj.WorkScopResionIds.Length > 1 ? obj.WorkScopResionIds.Substring(0, obj.WorkScopResionIds.Length - 1) : "0" );
                        objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, obj.ActId.Length>1 ? obj.ActId.Substring(0,obj.ActId.Length-1):"0");
                        objDBAccess.AddParameterToCommand("@ProcessingUnitId", DBFactoryParameterType.NVarchar, objD.ProcessingUnitId);
                        objDBAccess.AddParameterToCommand("@DepotId", DBFactoryParameterType.NVarchar, objD.DepotId);
                        objDBAccess.AddParameterToCommand("@WarehouseId", DBFactoryParameterType.NVarchar, objD.WarehouseId);
                        objDBAccess.AddParameterToCommand("@PLDBId", DBFactoryParameterType.NVarchar, objD.PLDBId);
                        objDBAccess.AddParameterToCommand("@PLDBBrancchId", DBFactoryParameterType.NVarchar, objD.PLDBBrancchId);
                        objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, Controllers.COMMON_FUNCTION.GetIpAddress());
                        objDBAccess.AddParameterToCommand("@ApexGccMappingId", DBFactoryParameterType.Int, objD.ApexGccMappingId);


                        
                        ResultFinal = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                        objDBAccess.CloseConnection();
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "Add(UserEntity obj,List<RoleArray> objRoleListData)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return ResultFinal;
        }

        public int AddUserReq(string Ids, string IdsRej, string SSO_ID)
        {
            int Result = -1;

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UserReqestApproved", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Ids", DBFactoryParameterType.VARCHAR, Ids);
                objDBAccess.AddParameterToCommand("@IdsRej", DBFactoryParameterType.VARCHAR, IdsRej);
                //objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSO_ID=null);



                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                objDBAccess.CloseConnection();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "AddUserReq(string Ids,string IdsRej,string SSO_ID)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int ChangeStatus(int UserId, int Action)
        {
            int Result = -1;
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UserDeleteStatusChange", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, UserId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.Int, Action);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "ChangeStatus(int UserId,int Action)" }, ex);
                Result = -1;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable UpdateUserRoleAppr(int MappingID, int IsApproved, int RoleID, string ActId, string PrimaryResionId, string DairyResionId, int UserId)
        {
            DataTable dt = null;
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateUserRoleApproval", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MappingID", DBFactoryParameterType.Int, MappingID);
                objDBAccess.AddParameterToCommand("@IsApproved", DBFactoryParameterType.Int, IsApproved);
                objDBAccess.AddParameterToCommand("@RoleID", DBFactoryParameterType.Int, RoleID);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, ActId);
                objDBAccess.AddParameterToCommand("@PrimaryResionId", DBFactoryParameterType.NVarchar, PrimaryResionId);
                objDBAccess.AddParameterToCommand("@DairyResionId", DBFactoryParameterType.NVarchar, DairyResionId);
                objDBAccess.AddParameterToCommand("@AddedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@ReplaceOfUserId", DBFactoryParameterType.Int, UserId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "UpdateUserRoleAppr(int MappingID, int IsApproved, int RoleID,string ActId, string PrimaryResionId , string DairyResionId )" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }

        public DataTable UpdateUserRole(int MappingID,  string ActId, string PrimaryResionId, string DairyResionId)
        {
            DataTable dt = null;
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateUserRole", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MappingID", DBFactoryParameterType.Int, MappingID);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, ActId);
                objDBAccess.AddParameterToCommand("@PrimaryResionId", DBFactoryParameterType.NVarchar, PrimaryResionId);
                objDBAccess.AddParameterToCommand("@DairyResionId", DBFactoryParameterType.NVarchar, DairyResionId);
                objDBAccess.AddParameterToCommand("@AddedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "UpdateUserRoleAppr(int MappingID, int IsApproved, int RoleID,string ActId, string PrimaryResionId , string DairyResionId )" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }
        public int Delete(int UserId)
        {
            return -1;
        }
        public DataTable GetAll(string WhereClause = "")
        {
            DataTable dt = null;
           
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UserList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = " GetAll(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public bool IsUserAuthorized(string Url, int ActionId, UserEntity _Model)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertUser", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, _Model.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, _Model.RoleId);
                objDBAccess.AddParameterToCommand("@MenuUrl", DBFactoryParameterType.NVarchar, Url);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.NVarchar, ActionId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "bool IsUserAuthorized(string Url, int ActionId, UserEntity _Model)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result > 0 ? true : false;
        }

        public int ApplyTransfer(RoleArray objD)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertUserRoleDeptMapping", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DEPTCODE", DBFactoryParameterType.Int, objD.DepartmentId);
                objDBAccess.AddParameterToCommand("@DESIGNATIONCODE", DBFactoryParameterType.Int, objD.DesignationId);
                objDBAccess.AddParameterToCommand("@PostId", DBFactoryParameterType.Int, objD.PostId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, objD.RoleId);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, objD.CreatedBy);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, objD.IsActive);
                objDBAccess.AddParameterToCommand("@AddedBy", DBFactoryParameterType.Int, objD.CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, 0);
                objDBAccess.AddParameterToCommand("@UnitId", DBFactoryParameterType.Int, objD.UnitId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, objD.LevelId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, objD.DivisionId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, objD.DistrictId);
                objDBAccess.AddParameterToCommand("@TehsiId", DBFactoryParameterType.Int, objD.TehsilId);
                objDBAccess.AddParameterToCommand("@CityId", DBFactoryParameterType.Int, objD.CityId);
                objDBAccess.AddParameterToCommand("@WardId", DBFactoryParameterType.Int, objD.WardId);
                objDBAccess.AddParameterToCommand("@PanchayatSamitiId", DBFactoryParameterType.Int, objD.PanchayatSamitiId);
                objDBAccess.AddParameterToCommand("@GramPanchayatId", DBFactoryParameterType.Int, objD.GramPanchayatId);
                objDBAccess.AddParameterToCommand("@VillageId", DBFactoryParameterType.Int, objD.VillageId);
                objDBAccess.AddParameterToCommand("@IsRuralArban", DBFactoryParameterType.Bit, objD.IsUrbanRural);
                objDBAccess.AddParameterToCommand("@MappingFromId", DBFactoryParameterType.BigInt, objD.MappingFromId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                objDBAccess.CloseConnection();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "ApplyTransfer(RoleArray obj)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;

        }

        public int ApplyTransAddiRole(RoleArray objD)
        {
            int Result = -1;
            try
            {
                objD.ActId = objD.ActId != null ? objD.ActId : "0";
                objD.WorkScopResionIds = objD.WorkScopResionIds != null ? objD.WorkScopResionIds : "0";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertUserTransAddiRoleDeptMapping", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DEPTCODE", DBFactoryParameterType.Int, objD.DepartmentId);
                objDBAccess.AddParameterToCommand("@DESIGNATIONCODE", DBFactoryParameterType.Int, objD.DesignationId);
                objDBAccess.AddParameterToCommand("@PostId", DBFactoryParameterType.Int, objD.PostId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, objD.RoleId);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, objD.USERID);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, objD.IsActive);
                objDBAccess.AddParameterToCommand("@AddedBy", DBFactoryParameterType.Int, objD.USERID);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, 0);
                objDBAccess.AddParameterToCommand("@UnitId", DBFactoryParameterType.Int, objD.UnitId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, objD.LevelId);
                objDBAccess.AddParameterToCommand("@DesignationLevelMappingId", DBFactoryParameterType.Int, objD.DesignationLevelMappingId);
                objDBAccess.AddParameterToCommand("@OrganizationLevelsMappingID", DBFactoryParameterType.Int, objD.OrganizationLevelsMappingID);
                objDBAccess.AddParameterToCommand("@OfficeId", DBFactoryParameterType.Int, objD.OfficeId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, objD.DivisionId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, objD.DistrictId);
                objDBAccess.AddParameterToCommand("@TehsiId", DBFactoryParameterType.Int, objD.TehsilId);
                objDBAccess.AddParameterToCommand("@CityId", DBFactoryParameterType.Int, objD.CityId);
                objDBAccess.AddParameterToCommand("@WardId", DBFactoryParameterType.Int, objD.WardId);
                objDBAccess.AddParameterToCommand("@PanchayatSamitiId", DBFactoryParameterType.Int, objD.PanchayatSamitiId);
                objDBAccess.AddParameterToCommand("@GramPanchayatId", DBFactoryParameterType.Int, objD.GramPanchayatId);
                objDBAccess.AddParameterToCommand("@VillageId", DBFactoryParameterType.Int, objD.VillageId);
                objDBAccess.AddParameterToCommand("@IsRuralArban", DBFactoryParameterType.Bit, objD.CityId==0?0:1);
                objDBAccess.AddParameterToCommand("@MappingFromId", DBFactoryParameterType.BigInt, 0);
                objDBAccess.AddParameterToCommand("@ResionId", DBFactoryParameterType.BigInt, objD.ResionId);
                objDBAccess.AddParameterToCommand("@WorkScopResionIds", DBFactoryParameterType.NVarchar, objD.WorkScopResionIds.Length > 1 ? objD.WorkScopResionIds.Substring(0, objD.WorkScopResionIds.Length - 1) : "0");
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, objD.ActId.Length > 1 ? objD.ActId.Substring(0, objD.ActId.Length - 1) : "0");
                objDBAccess.AddParameterToCommand("@ProcessingUnitId", DBFactoryParameterType.NVarchar, objD.ProcessingUnitId);
                objDBAccess.AddParameterToCommand("@DepotId", DBFactoryParameterType.NVarchar, objD.DepotId);
                objDBAccess.AddParameterToCommand("@WarehouseId", DBFactoryParameterType.NVarchar, objD.WarehouseId);
                objDBAccess.AddParameterToCommand("@PLDBId", DBFactoryParameterType.NVarchar, objD.PLDBId);
                objDBAccess.AddParameterToCommand("@PLDBBrancchId", DBFactoryParameterType.NVarchar, objD.PLDBBrancchId);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, Controllers.COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@ApexGccMappingId", DBFactoryParameterType.Int, objD.ApexGccMappingId);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                objDBAccess.CloseConnection();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "ApplyTransAddiRole(RoleArray objD)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;

        }

        public DataTable GetAllUserReq(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                SqlCommand cmd = objDBAccess.CreateCommand("GetUserrReqList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "GetAllUserReq(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable UserSelfReqList(string SSOID = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (SSOID == "")
                    SSOID = _LoggedInUser.SSOID;
                SqlCommand cmd = objDBAccess.CreateCommand("GetUserrSelfReqList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOID);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "UserSelfReqList(string SSOID ="+ SSOID  + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetUserReqDetail(int MappingID)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                SqlCommand cmd = objDBAccess.CreateCommand("GetUserReqDetailByMappingId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MappingID", DBFactoryParameterType.Int, MappingID);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "GetUserReqDetail(int MappingID)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


        public DataTable GetDistrictwiseSocietyByTypeStatus(string SocietyType, string SocietyStatus)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                SqlCommand cmd = objDBAccess.CreateCommand("GetDistrictwiseSocietyByTypeStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyType", DBFactoryParameterType.NVarchar, SocietyType);
                objDBAccess.AddParameterToCommand("@SocietyStatus", DBFactoryParameterType.NVarchar, SocietyStatus);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "GetDistrictwiseSocietyByTypeStatus(string SocietyType, string SocietyStatus)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


        public DataTable GetUserRoleAll(long UserID)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                SqlCommand cmd = objDBAccess.CreateCommand("SelectUserRoleProfile", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, UserID);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "GetUserRoleAll(int UserID)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable GetSMFApplicationStatus()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                SqlCommand cmd = objDBAccess.CreateCommand("SMFApplicationsStatusCountList", DBFactoryCommandType.PROCEDURE_COMMAND);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "GetDistrictwiseSocietyByTypeStatus(string SocietyType, string SocietyStatus)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetSSOUserList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SSOUserList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "public DataTable GetSSOUserList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


        public DataTable GetAllUser(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                SqlCommand cmd = objDBAccess.CreateCommand("GetUserList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "GetAllUser(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


        public DataTable GetHousingSocietyAuditsCountYearWise()
        {
            dt.Clear();
            int LevelID=0, ID = 0;
            
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser != null)
                {
                    LevelID = _LoggedInUser.LevelId;
                    if (LevelID == 1)
                        ID = 0;
                    else if (LevelID == 2)
                        ID = _LoggedInUser.DivisionId;
                    else if (LevelID == 3)
                        ID = _LoggedInUser.DistrictId;
                    else if (LevelID == 4)
                        ID = _LoggedInUser.BlockId;
                    else if (LevelID == 5)
                        ID = _LoggedInUser.GPId;

                }
                SqlCommand cmd = objDBAccess.CreateCommand("HousingSocietyAuditsStatusForDasboard", DBFactoryCommandType.PROCEDURE_COMMAND);               
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, LevelID);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, ID);
               

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "GetHousingSocietyAuditsCountYearWise()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetUserLevelWiseCount()
        {
            dt.Clear();            
            try
            {
                objDBAccess.OpenConnection();               
                SqlCommand cmd = objDBAccess.CreateCommand("UserLevelWiseCount", DBFactoryCommandType.PROCEDURE_COMMAND);               
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "GetHousingSocietyAuditsCountYearWise()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable CheckRoleAtCuttentLocation(int MappingID, string ActId, string PrimaryResionId, string DairyResionId)
        {
            DataTable dt = null;
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CheckRoleAtCuttentLocation", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MappingID", DBFactoryParameterType.Int, MappingID);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, ActId);
                objDBAccess.AddParameterToCommand("@PrimaryResionId", DBFactoryParameterType.NVarchar, PrimaryResionId);
                objDBAccess.AddParameterToCommand("@DairyResionId", DBFactoryParameterType.NVarchar, DairyResionId);
                objDBAccess.AddParameterToCommand("@AddedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "CheckRoleAtCuttentLocation(int MappingID, int IsApproved, int RoleID,string ActId, string PrimaryResionId , string DairyResionId )" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }

        public string MappingChangeStatus(int MappingID, int Action, int UserId, int Flag)
        {
            DataTable dt = new DataTable();
            string Result = "";
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UserRoleDeptMappingChangeStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MappingID", DBFactoryParameterType.BigInt, MappingID);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);
                objDBAccess.AddParameterToCommand("@UpdateBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, UserId);
                //  objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("@flag", DBFactoryParameterType.Int, Flag);

                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                //dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "MappingChangeStatus(int MappingId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
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

        public string ChangeUserDetailSSoidData(string SSOIDvalue)
        {
            var jsonSerialiser = new System.Web.Script.Serialization.JavaScriptSerializer();
            UserEntity UserRegistration = new UserEntity();

            string SSOID = SSOIDvalue;
            SSOWSSoap.SSOUserDetail usrdtl = new SSOWSSoap.SSOUserDetail();
            SSOWSSoap.SSOWSSoapClient ssoclient = new SSOWSSoap.SSOWSSoapClient();


            usrdtl = ssoclient.GetUserDetailXML(SSOID, "RAJSAHKAR.TEST", "Rajasthan@12");
            if (usrdtl != null && usrdtl.SSOID.Length > 0)
            {
                //  UserRegistration.DateOfBirth = usrdtl.dateOfBirth.Length > 3 ? Convert.ToDateTime(usrdtl.dateOfBirth) : System.DateTime.MaxValue;
                UserRegistration.OfficerNameEn = usrdtl.displayName;
                UserRegistration.Gender = (usrdtl.gender == "MALE" ? "M" : "F");
                //UserRegistration.ImageUrl = usrdtl.jpegPhoto;
                UserRegistration.SSOID = usrdtl.SSOID;
                UserRegistration.AadharNo = usrdtl.aadhaarId;
                UserRegistration.MobileNo = usrdtl.mobile;
                UserRegistration.EmailId = usrdtl.mailPersonal;
                UserRegistration.DepartmentEn = usrdtl.department;
                UserRegistration.DesignationEn = usrdtl.designation;
                UserRegistration.DepartmentId = int.Parse(usrdtl.departmentId == null ? "0" : usrdtl.departmentId);
                UserRegistration.UnitId = 0;
                //string pathString = "~/SSOUSER/" + usrdtl.SSOID + ".jpg";
                //if (usrdtl.jpegPhoto != null)
                //{

                //    try
                //    {
                //        if (!System.IO.File.Exists(Server.MapPath(pathString)))
                //        {
                //            byte[] imageBytes = Convert.FromBase64String(usrdtl.jpegPhoto);
                //            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                //            string fileName = usrdtl.SSOID;
                //            FileStream fs = new FileStream(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/SSOUSER/"), fileName + ".jpg"), FileMode.CreateNew, FileAccess.Write);
                //            ms.WriteTo(fs);
                //            fs.Close();
                //            fs.Dispose();
                //            ms.Flush();
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        ILogger objILogger = new DBLogger();
                //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Controllers.LoginController", FunctionName = "Inner Try of GetSSOUserDetails()" }, ex);
                //    }

                //}
            }
            else
            {
                UserRegistration.SSOID = "Data Could Not Be Retrieved From The Server Please Try After Some Time";
            }
            return jsonSerialiser.Serialize(UserRegistration);
        }




        public DataTable SetUserUpdateInfoNewSSOID(UserModelupdatenewssoid _Object)
        {
            DataTable dt = new DataTable();

            objDBAccess.OpenConnection();
       
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            SqlCommand cmd = objDBAccess.CreateCommand("Update_UserInfoSsoid", DBFactoryCommandType.PROCEDURE_COMMAND);
            objDBAccess.AddParameterToCommand("@OfficerNameEn", DBFactoryParameterType.NVarchar, _LoggedInUser.OfficerNameEn);
            objDBAccess.AddParameterToCommand("@Gender", DBFactoryParameterType.NVarchar, _LoggedInUser.Gender);
            objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
            objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, _Object.MobileNo);
            // objDBAccess.AddParameterToCommand("@EmailId", DBFactoryParameterType.NVarchar, _Object.EmailId);
            objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, _Object.UserId);
            objDBAccess.AddParameterToCommand("@UpdateBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
            dt = objDBAccess.GetTable();


            return dt;
        }

        public DataTable GetAllUserUpdateSSoid(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                SqlCommand cmd = objDBAccess.CreateCommand("GetUserList_UpdateSSoid", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);


                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "GetAllUser(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetUserDetailsBySSO(string SSOID = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (SSOID == "")
                    SSOID = _LoggedInUser.SSOID;
                SqlCommand cmd = objDBAccess.CreateCommand("GetUserDetailsBySSO", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOID);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "GetUserDetailsBySSO(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        //Bhavesh for Insert Citizen
        public int AddOrUpdateCitizen()
        {
           
            if (objDBAccess == null)
                objDBAccess = new DBFactoryMSSQL();

            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];           
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("InsertCitizen",DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("@CitizenId", DBFactoryParameterType.Int, _LoggedInUser.CitizenId);
                objDBAccess.AddParameterToCommand("@CitizenTitle",DBFactoryParameterType.NVarchar, _LoggedInUser.CitizenTitle );
                objDBAccess.AddParameterToCommand("@DisplayNameEn",DBFactoryParameterType.NVarchar, _LoggedInUser.DisplayNameEn);
                objDBAccess.AddParameterToCommand("@DisplayNameHi",DBFactoryParameterType.NVarchar, _LoggedInUser.DisplayNameHi );
                objDBAccess.AddParameterToCommand("@Gender",DBFactoryParameterType.VARCHAR, _LoggedInUser.Gender);
                objDBAccess.AddParameterToCommand("@DateOfBirth", DBFactoryParameterType.DateTime, _LoggedInUser.DateOfBirth);
                objDBAccess.AddParameterToCommand("@MobileNo",DBFactoryParameterType.NVarchar, _LoggedInUser.MobileNo );
                objDBAccess.AddParameterToCommand("@CreatedBy",DBFactoryParameterType.Int, _LoggedInUser.CreatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP",DBFactoryParameterType.NVarchar,_LoggedInUser.CreatedIP);
                objDBAccess.AddParameterToCommand("@IsActive",DBFactoryParameterType.Int, _LoggedInUser.IsActive);
                objDBAccess.AddParameterToCommand("@AadhaarId",DBFactoryParameterType.NVarchar, _LoggedInUser.AadharNo );
                int result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                if (result > 0)
                {
                    return result;
                }

                // Just return existing CitizenId or 0
                return 0;
            }
            finally
            {
                
                objDBAccess.CloseConnection();
            }
        }

        //Bhavesh for get list Citizen

        public DataTable GetAllCitizenUser(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess.CreateCommand("GetCitizenList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "GetAllUser(string WhereClause = '')" }, ex);
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