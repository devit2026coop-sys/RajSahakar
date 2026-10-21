using System;
using Cooperative.DBConnector;
using System.Data;
using System.Web;
using System.Data.SqlClient;
using CoopDAL;
using CoopDB;
using Models;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Web.Helpers;
using javax.xml.transform;
using static javax.net.ssl.SSLEngineResult;
using Cooperative.Controllers;
using static Cooperative.Controllers.ServicesController;

namespace Cooperative.Models
{
    public class MobileDashboardModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public MobileDashboardModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public MobileDashboardModel()
        {
            objDBAccess = new DBFactoryMSSQL();

        }

        #region Ritesh_Mobile_App_ModelMethods

        #region     --START--   SocietyDashboardData_AsPer_LoginUser   --START--
        public Dictionary<string, object> GetSocietyDashboardData_UserRoleWise(int UserRoleMappingId, int IsOnline, int DistId, int DivisionId = 0)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var dt1 = GetSocietyCountActWise(UserRoleMappingId, IsOnline, DistId, DivisionId);
                var dt2 = GetSocietyCountActDistricWise(UserRoleMappingId, IsOnline);
                var dt3 = GetSocietyCountActDistricWise(UserRoleMappingId, IsOnline);
                var dt4 = GetSocietyCountActDistricWise(UserRoleMappingId, IsOnline);

                result["SocietyData"] = csCommon.ConvertDataTableToDynamic(dt1);
                result["DistrictWiseSocietyData2001"] = csCommon.ConvertDataTableToDynamic(dt2);
                result["DistrictWiseSocietyData1958"] = csCommon.ConvertDataTableToDynamic(dt3);
                result["DistrictWiseSocietyData2005"] = csCommon.ConvertDataTableToDynamic(dt4);
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Models.MobileDashboardModel",
                    FunctionName = "public Dictionary<string, object> GetSocietyDashboardData_UserRoleWise(int UserRoleMappingId, int IsOnline, int DistId, int DivisionId = 0)"
                }, ex);
            }

            return result;
        }

        /// <summary>
        /// For show data on mobile app > Dashboard : Society counts as per Offline / Online filter 
        /// 
        /// </summary>
        /// <param name="UserRoleMappingId"></param>
        /// <param name="IsOnline"></param>
        /// <returns></returns>       

        public DataTable GetSocietyCountActWise(int UserRoleMappingId, int IsOnline, int DistId = 0, int DivisionId = 0)
        {
            DataTable dt = new DataTable();

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyCountActWise_UserRoleWise", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserRoleMappingId", DBFactoryParameterType.Int, UserRoleMappingId);
                objDBAccess.AddParameterToCommand("@IsOnline", DBFactoryParameterType.Int, IsOnline);
                objDBAccess.AddParameterToCommand("@DistId", DBFactoryParameterType.Int, DistId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Models.MobileDashboardModel",
                    FunctionName = "public DataTable GetSocietyCountActWise(int UserRoleMappingId, int IsOnline, int DistId = 0, int DivisionId = 0)"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable GetSocietyCountActDistricWise(int UserRoleMappingId, int IsOnline)
        {
            DataTable dt = new DataTable();

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand(
                    "GetSocietyCountActDistricWise_UserRoleWise",
                    DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@UserRoleMappingId", DBFactoryParameterType.Int, UserRoleMappingId);
                objDBAccess.AddParameterToCommand("@IsOnline", DBFactoryParameterType.Int, IsOnline);

                dt = objDBAccess.GetTable();   // ✅ Direct DataTable
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Models.MobileDashboardModel",
                    FunctionName = "public DataTable GetSocietyCountActDistricWise(int UserRoleMappingId, int IsOnline)"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataSet GetSocietyDetailsActDistricUserRoleSearchByNameRegNo(string ActId, int IsOnline, string Status, int DistrictId, int BlockId, int GramPanchayatId, int VillageId, int CityId, int WardId, int DivisionId = 0, int IsRuralNotUrban = 1, int SocietyTypeId = 0, int UserRoleMappingId = 0, string SocietyName_RegistrationNo = "", int PageNo = 1, int Limit = 10)
        {
            DataSet ds = new DataSet();

            try
            {
                objDBAccess.OpenConnection();

                objDBAccess.CreateCommand("GetSocietyDetails_ActDistricWise_UserRoleWise",
                    DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@UserRoleMappingId", DBFactoryParameterType.Int, UserRoleMappingId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int,
                    !string.IsNullOrEmpty(ActId) ? Convert.ToInt32(ActId) : 0);

                objDBAccess.AddParameterToCommand("@IsOnline", DBFactoryParameterType.Int, IsOnline);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, Status);
                objDBAccess.AddParameterToCommand("@SocietyName_RegistrationNo", DBFactoryParameterType.NVarchar, SocietyName_RegistrationNo);
                objDBAccess.AddParameterToCommand("@PageNo", DBFactoryParameterType.Int, PageNo);
                objDBAccess.AddParameterToCommand("@Limit", DBFactoryParameterType.Int, Limit);

                objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, SocietyTypeId);

                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, BlockId);
                objDBAccess.AddParameterToCommand("@GramPanchayatId", DBFactoryParameterType.Int, GramPanchayatId);
                objDBAccess.AddParameterToCommand("@VillageId", DBFactoryParameterType.Int, VillageId);

                objDBAccess.AddParameterToCommand("@CityId", DBFactoryParameterType.Int, CityId);
                objDBAccess.AddParameterToCommand("@WardId", DBFactoryParameterType.Int, WardId);
                objDBAccess.AddParameterToCommand("@IsRuralNotUrban", DBFactoryParameterType.Int, IsRuralNotUrban);  ///--- The Value of IsRuralNotUrban = 1 Then "Rural" Else if IsRuralNotUrban = 0 then "Urban". Use For Society Location Filter. //

                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Models.MobileDashboardModel",
                    FunctionName = "public DataSet GetSocietyDetailsActDistricUserRoleSearchByNameRegNo(string ActId, int IsOnline, string Status, int DistrictId, int BlockId, int GramPanchayatId, int VillageId, int CityId, int WardId, int DivisionId = 0, int IsRuralNotUrban = 1, int SocietyTypeId = 0, int UserRoleMappingId = 0, string SocietyName_RegistrationNo = '', int PageNo = 1, int Limit = 10)"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return ds;
        }

        public DataSet GetSocietyDetails_UserRoleWise_MoreSummary(int SocietyId, string SocietySummaryType = "DOCUMENTS", int PageNo = 1, int Limit = 10)
        {
            DataSet ds = new DataSet();

            try
            {
                objDBAccess.OpenConnection();

                objDBAccess.CreateCommand("GetSocietyDetails_UserRoleWise_MoreSummary",
                    DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, SocietyId);

                objDBAccess.AddParameterToCommand("@SocietySummaryType", DBFactoryParameterType.NVarchar, SocietySummaryType);
                objDBAccess.AddParameterToCommand("@PageNo", DBFactoryParameterType.Int, PageNo);
                objDBAccess.AddParameterToCommand("@Limit", DBFactoryParameterType.Int, Limit);

                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Models.MobileDashboardModel",
                    FunctionName = "public DataSet GetSocietyDetails_UserRoleWise_MoreSummary(int SocietyId, string SocietySummaryType = 'DOCUMENTS', int PageNo = 1, int Limit = 10)"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return ds;
        }

        public DataSet GetSociety_OtherDetails_UserRoleWise(int SocietyId, string SocietyIdEnc = "", string OtherDetailsType = "ALL", int PageNo = 1, int Limit = 10)
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();

                objDBAccess.CreateCommand("Get_Society_UserRoleWise_OtherDetails",
                    DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, SocietyId);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                objDBAccess.AddParameterToCommand("@OtherDetailsType", DBFactoryParameterType.NVarchar, OtherDetailsType);
                objDBAccess.AddParameterToCommand("@PageNo", DBFactoryParameterType.Int, PageNo);
                objDBAccess.AddParameterToCommand("@Limit", DBFactoryParameterType.Int, Limit);

                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Models.MobileDashboardModel",
                    FunctionName = "public DataSet GetSociety_OtherDetails_UserRoleWise(int SocietyId, string SocietyIdEnc = '', string OtherDetailsType = 'ALL', int PageNo = 1, int Limit = 10)"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return ds;
        }
        #endregion  --END----   SocietyDashboardData_AsPer_LoginUser   --END----

        #region     --START--   Master API - Use in Mobile App - Location DDL Fill - Society Details

        /// <summary>
        /// Created By  :   Ritesh Khandelwal
        /// Created On  :   2026-04-28
        /// Created For :   Use for Mobile App show Division List
        /// REMARKS     :   As discussed into meeting on dated 2026-04-27, Monday with Sunil (BA) and Developers Team.
        ///                 Also Filter DDL values as per login user role maping id based different location ids wise mapped Society data existance in records.
        /// </summary>
        /// <param name="UserRoleMappingId"></param>
        /// <param name="StateId"></param>
        /// <returns></returns>
        public DataTable DivisionList(int UserRoleMappingId = 0, int StateId = 0, string Status = "")
        {
            dt?.Clear(); // if dt not null then clear data //
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DivisionList_MobileApp", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserRoleMappingId", DBFactoryParameterType.Int, UserRoleMappingId);
                objDBAccess.AddParameterToCommand("@StateId", DBFactoryParameterType.Int, StateId);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, Status);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileDashboardModel", FunctionName = "public DataTable DivisionList(int UserRoleMappingId = 0, int StateId = 0, string Status = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        /// <summary>
        /// Created By  :   Ritesh
        /// Created On  :   2026-04-21
        /// Created For :   Use for Mobile App show District List
        /// 
        /// Updated By  :   Ritesh Khandelwal
        /// Updated On  :   2026-04-28
        /// Updated For :   Also get Division Id as input and as per filtered this Mobile App show District List.
        /// REMARKS     :   As discussed into meeting on dated 2026-04-27, Monday with Sunil (BA) and developers team.
        ///                 Also Filter DDL values as per login user role maping id based different location ids wise mapped Society data existance in records.
        /// </summary>
        /// <param name="UserRoleMappingId"></param>
        /// <param name="DivisionId"></param>
        /// <returns></returns>
        public DataTable DistrictList(int UserRoleMappingId = 0, int DivisionId = 0, string Status = "")
        {
            dt?.Clear(); // if dt not null then clear data //
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DistrictList_MobileApp", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserRoleMappingId", DBFactoryParameterType.Int, UserRoleMappingId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, Status);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileDashboardModel", FunctionName = "public DataTable DistrictList(int UserRoleMappingId = 0, int DivisionId = 0, string Status ='')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        /// <summary>
        /// Created By :    Ritesh
        /// Created On :    2026-04-17
        /// Created For :   Use for Mobile App show Block List
        /// REMARKS     :   As discussed into meeting on dated 2026-04-27, Monday with Sunil (BA) and Developers Team.
        ///                 Also Filter DDL values as per login user role maping id based different location ids wise mapped Society data existance in records.
        /// </summary>
        /// <param name="UserRoleMappingId"></param>
        /// <param name="DistrictId"></param>
        /// <returns></returns>
        public DataTable BlockList(int UserRoleMappingId = 0, int DistrictId = 0, int IsOnline = 0, int ActId = 0, string Status = "")
        {
            //if (dt != null)
            //{
            //    dt.Clear();
            //}

            dt?.Clear(); // if dt not null then clear data //
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("BlockList_MobileApp", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserRoleMappingId", DBFactoryParameterType.Int, UserRoleMappingId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@IsOnline", DBFactoryParameterType.Int, IsOnline);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, ActId);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, Status);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileDashboardModel", FunctionName = "public DataTable BlockList(int UserRoleMappingId = 0, int DistrictId = 0, int IsOnline = 0, int ActId = 0, string Status = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        /// <summary>
        /// Created By :    Ritesh
        /// Created On :    2026-17-04
        /// Created For :   Use for Mobile App show Gram Panchayat List
        /// REMARKS     :   As discussed into meeting on dated 2026-04-27, Monday with Sunil (BA) and Developers Team.
        ///                 Also Filter DDL values as per login user role maping id based different location ids wise mapped Society data existance in records.
        /// </summary>
        /// <param name="UserRoleMappingId"></param>
        /// <param name="BlockId"></param>
        /// <returns></returns>
        public DataTable GramPanchayatList(int UserRoleMappingId = 0, int BlockId = 0, int IsOnline = 0, int ActId = 0, string Status = "")
        {
            dt?.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GramPanchayatList_MobileApp", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserRoleMappingId", DBFactoryParameterType.Int, UserRoleMappingId);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, BlockId);
                objDBAccess.AddParameterToCommand("@IsOnline", DBFactoryParameterType.Int, IsOnline);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, ActId);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, Status);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.MobileDashboardModel", FunctionName = "public DataTable GramPanchayatList(int UserRoleMappingId = 0, int BlockId = 0, int IsOnline = 0, int ActId = 0, string Status = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        /// <summary>
        /// Created By :    Ritesh
        /// Created On :    2026-17-04
        /// Created For :   Use for Mobile App show Village List
        /// REMARKS     :   As discussed into meeting on dated 2026-04-27, Monday with Sunil (BA) and Developers Team.
        ///                 Also Filter DDL values as per login user role maping id based different location ids wise mapped Society data existance in records.
        /// </summary>
        /// <param name="UserRoleMappingId"></param>
        /// <param name="GramPanchayatId"></param>
        /// <returns></returns>
        public DataTable VillageList(int UserRoleMappingId = 0, int GramPanchayatId = 0, int IsOnline = 0, int ActId = 0, string Status = "")
        {
            dt?.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("VillageList_MobileApp", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserRoleMappingId", DBFactoryParameterType.Int, UserRoleMappingId);
                objDBAccess.AddParameterToCommand("@GPId", DBFactoryParameterType.Int, GramPanchayatId);
                objDBAccess.AddParameterToCommand("@IsOnline", DBFactoryParameterType.Int, IsOnline);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, ActId);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, Status);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.MobileDashboardModel", FunctionName = "public DataTable VillageList(int UserRoleMappingId = 0, int GramPanchayatId = 0, int IsOnline = 0, int ActId = 0, string Status = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        /// <summary>
        /// Created By :    Ritesh
        /// Created On :    2026-04-29
        /// Created For :   Use for Mobile App show City List
        /// REMARKS     :   As discussed into meeting on dated 2026-04-27, Monday with Sunil (BA) and Developers Team.
        ///                 Also Filter DDL values as per login user role maping id based different location ids wise mapped Society data existance in records.
        /// </summary>
        /// <param name="UserRoleMappingId"></param>
        /// <param name="DistrictId"></param>
        /// <returns></returns>
        public DataTable CityList(int UserRoleMappingId = 0, int DistrictId = 0, int CityType = 0, int IsOnline = 0, int ActId = 0, string Status = "")
        {
            //if (dt != null)
            //{
            //    dt.Clear();
            //}

            dt?.Clear(); // if dt not null then clear data //
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CityList_MobileApp", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserRoleMappingId", DBFactoryParameterType.Int, UserRoleMappingId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@CityType", DBFactoryParameterType.Int, CityType);
                objDBAccess.AddParameterToCommand("@IsOnline", DBFactoryParameterType.Int, IsOnline);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, ActId);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, Status);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileDashboardModel", FunctionName = "public DataTable CityList(int UserRoleMappingId = 0, int DistrictId = 0, int CityType = 0, int IsOnline = 0, int ActId = 0, string Status = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        /// <summary>
        /// Created By :    Ritesh
        /// Created On :    2026-04-29
        /// Created For :   Use for Mobile App show Ward List
        /// REMARKS     :   As discussed into meeting on dated 2026-04-27, Monday with Sunil (BA) and Developers Team.
        ///                 Also Filter DDL values as per login user role maping id based different location ids wise mapped Society data existance in records.
        /// </summary>
        /// <param name="UserRoleMappingId"></param>
        /// <param name="DistrictId"></param>
        /// <returns></returns>
        public DataTable WardList(int UserRoleMappingId = 0, int CityId = 0, int IsOnline = 0, int ActId = 0, string Status = "")
        {
            //if (dt != null)
            //{
            //    dt.Clear();
            //}

            dt?.Clear(); // if dt not null then clear data //
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("WardList_MobileApp", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserRoleMappingId", DBFactoryParameterType.Int, UserRoleMappingId);
                objDBAccess.AddParameterToCommand("@CityId", DBFactoryParameterType.Int, CityId);
                objDBAccess.AddParameterToCommand("@IsOnline", DBFactoryParameterType.Int, IsOnline);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, ActId);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, Status);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileDashboardModel", FunctionName = "public DataTable WardList(int UserRoleMappingId = 0, int CityId = 0, int IsOnline = 0, int ActId = 0, string Status = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }
        #endregion  --END----   Master API - Use in Mobile App - Location DDL Fill - Society Details

        #region     --START--   Master API - Filter Use in Mobile App
        public DataTable GetSocietyTypeList()
        {
            dt?.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyTypeList_MobileAPI", DBFactoryCommandType.PROCEDURE_COMMAND);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileDashboardModel", FunctionName = "public DataTable GetSocietyTypeList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetSocietyTypeList_ActWise(int ActId)
        {
            dt?.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyTypeList_MobileAPI_ActIdWise", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, ActId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileDashboardModel", FunctionName = "public DataTable GetSocietyTypeList_ActWise(int ActId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        #endregion  --END----   Master API - Filter Use in Mobile App

        #region     --START--   SocietyDashboardData_AsPer_CitizenUserLogin    
        public DataTable GetSocietyCountActWiseCitizenUser(string CitizenSSOID, int IsOnline, int DistId = 0, int DivisionId = 0)
        {
            DataTable dt = new DataTable();

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyCountActWise_UserCitizenSSOIDWise", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CitizenSSOID", DBFactoryParameterType.NVarchar, CitizenSSOID);
                objDBAccess.AddParameterToCommand("@IsOnline", DBFactoryParameterType.Int, IsOnline);
                objDBAccess.AddParameterToCommand("@DistId", DBFactoryParameterType.Int, DistId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Models.MobileDashboardModel",
                    FunctionName = "public DataTable GetSocietyCountActWiseCitizenUser(string CitizenSSOID, int IsOnline, int DistId = 0, int DivisionId = 0)"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }
        public Dictionary<string, object> GetSocietyDashboardData_UserCitizenSSOIDWise(string CitizenSSOID, int IsOnline, int DistId = 0, int DivisionId = 0)
        {
            var result = new Dictionary<string, object>();

            // Proper JSON Log //
            string jsonResult = "";
            string dtJson = "";

            try
            {
                var dt1 = GetSocietyCountActWiseCitizenUser(CitizenSSOID, IsOnline, DistId, DivisionId);

                // Log DataTable separately
                try
                {
                    dtJson = Newtonsoft.Json.JsonConvert.SerializeObject(dt1);
                }
                catch (Exception Ex1)
                {
                    dtJson = "DataTable Serialization Failed";
                }

                result["SocietyData"] = csCommon.ConvertDataTableToDynamic(dt1);
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Models.MobileDashboardModel",
                    FunctionName = "public Dictionary<string, object> GetSocietyDashboardData_UserCitizenSSOIDWise(string CitizenSSOID, int IsOnline, int DistId = 0,int DivisionId = 0)"
                }, ex);
            }

            try
            {
                jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception Ex2)
            {
                jsonResult = "Serialization Failed";
            }

            Exception ExObj = new Exception();

            ErrorLogs.LogError(new ErrorModel()
            {
                PageName = "Cooperative.Models.MobileDashboardModel",
                FunctionName = "public Dictionary<string, object> GetSocietyDashboardData_UserCitizenSSOIDWise(string CitizenSSOID, int IsOnline, int DistId = 0, int DivisionId = 0) ;" + " < dt > : " + dtJson + "; < result > : " + jsonResult
            }, ExObj);

            return result;
        }

        public DataSet GetSocietyDetailsActWiseUserCitizenSSOIDSearchByNameRegNo(string CitizenSSOID, string ActId, int IsOnline, string Status, int DivisionId, int DistrictId, int BlockId, int GramPanchayatId, int VillageId, int CityId, int WardId, int IsRuralNotUrban = 1, int SocietyTypeId = 0, string SocietyName_RegistrationNo = "", int PageNo = 1, int Limit = 10)
        {
            DataSet ds = new DataSet();
            // Proper JSON Log //
            string jsonResult = "";

            try
            {
                objDBAccess.OpenConnection();

                objDBAccess.CreateCommand("GetSocietyDetails_ActWise_UserCitizenSSOIDWise",
                    DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@CitizenSSOID", DBFactoryParameterType.NVarchar, CitizenSSOID);

                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int,
                    !string.IsNullOrEmpty(ActId) ? Convert.ToInt32(ActId) : 0);

                objDBAccess.AddParameterToCommand("@IsOnline", DBFactoryParameterType.Int, IsOnline);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, Status);
                objDBAccess.AddParameterToCommand("@SocietyName_RegistrationNo", DBFactoryParameterType.NVarchar, SocietyName_RegistrationNo);
                objDBAccess.AddParameterToCommand("@PageNo", DBFactoryParameterType.Int, PageNo);
                objDBAccess.AddParameterToCommand("@Limit", DBFactoryParameterType.Int, Limit);

                objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, SocietyTypeId);

                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, BlockId);
                objDBAccess.AddParameterToCommand("@GramPanchayatId", DBFactoryParameterType.Int, GramPanchayatId);
                objDBAccess.AddParameterToCommand("@VillageId", DBFactoryParameterType.Int, VillageId);

                objDBAccess.AddParameterToCommand("@CityId", DBFactoryParameterType.Int, CityId);
                objDBAccess.AddParameterToCommand("@WardId", DBFactoryParameterType.Int, WardId);

                objDBAccess.AddParameterToCommand("@IsRuralNotUrban", DBFactoryParameterType.Int, IsRuralNotUrban);  ///--- The Value of IsRuralNotUrban = 1 Then "Rural" Else if IsRuralNotUrban = 0 then "Urban". Use For Society Location Filter. //

                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Models.MobileDashboardModel",
                    FunctionName = "public DataSet GetSocietyDetailsActWiseUserCitizenSSOIDSearchByNameRegNo(string CitizenSSOID, string ActId, int IsOnline, string Status, int DivisionId, int DistrictId, int BlockId, int GramPanchayatId, int VillageId, int CityId, int WardId, int IsRuralNotUrban = 1, int SocietyTypeId = 0, string SocietyName_RegistrationNo = '', int PageNo = 1, int Limit = 10)"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
                }
                else
                {
                    jsonResult = "Table to Row data not found in ds";
                }
            }
            catch
            {
                jsonResult = "Serialization Failed";
            }

            Exception exObj = new Exception();

            ErrorLogs.LogError(new ErrorModel()
            {
                PageName = "Cooperative.Models.MobileDashboardModel",
                FunctionName = "public DataSet GetSocietyDetailsActWiseUserCitizenSSOIDSearchByNameRegNo(string CitizenSSOID, string ActId, int IsOnline, string Status, int DivisionId, int DistrictId, int BlockId, int GramPanchayatId, int VillageId, int CityId, int WardId, int IsRuralNotUrban = 1, int SocietyTypeId = 0, string SocietyName_RegistrationNo = '', int PageNo = 1, int Limit = 10) ;" + " < jsonResult > : " + jsonResult
            }, exObj);

            return ds;
        }

        public DataSet GetSocietyDetails_UserCitizenSSOIDWise_MoreSummary(int SocietyId, string SocietySummaryType = "DOCUMENTS", int PageNo = 1, int Limit = 10)
        {
            DataSet ds = new DataSet();
            string jsonResult = "";
            try
            {
                objDBAccess.OpenConnection();

                objDBAccess.CreateCommand("GetSocietyDetails_UserCitizenSSOIDWise_MoreSummary",
                    DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, SocietyId);

                objDBAccess.AddParameterToCommand("@SocietySummaryType", DBFactoryParameterType.NVarchar, SocietySummaryType);
                objDBAccess.AddParameterToCommand("@PageNo", DBFactoryParameterType.Int, PageNo);
                objDBAccess.AddParameterToCommand("@Limit", DBFactoryParameterType.Int, Limit);

                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Models.MobileDashboardModel",
                    FunctionName = "public DataSet GetSocietyDetails_UserCitizenSSOIDWise_MoreSummary(int SocietyId, string SocietySummaryType = 'DOCUMENTS', int PageNo = 1, int Limit = 10)"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
                }
                else
                {
                    jsonResult = "Table to Row data not dound in ds";
                }
            }
            catch
            {
                jsonResult = "Serialization Failed";
            }

            ErrorLogs.LogError(new ErrorModel()
            {
                PageName = "Cooperative.Models.MobileDashboardModel",
                FunctionName = "public DataSet GetSocietyDetails_UserCitizenSSOIDWise_MoreSummary(int SocietyId, string SocietySummaryType = 'DOCUMENTS', int PageNo = 1, int Limit = 10) ;" + " < result > : " + jsonResult
            }, null);

            return ds;
        }

        #endregion  --END----   SocietyDashboardData_AsPer_CitizenUserLogin --END----

        #region     --START--   Mobile_APP_Login_API_With_USERRoleDetails_UserCitizenSSOIDDetails
        public DataTable Mobile_LoginBySSOId(string SSOId)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("LoginForMobileApp_WithUserType", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileDashboardModel", FunctionName = "public DataTable Mobile_LoginBySSOId(string SSOId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable Mobile_IsSocietywithSSOId(string SSOId)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Mobile_IsSocietywithSSOId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileDashboardModel", FunctionName = "public DataTable Mobile_IsSocietywithSSOId(string SSOId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        #endregion  --END----   Mobile_APP_Login_API_With_USERRoleDetails_UserCitizenSSOIDDetails

        #region     --START--   CitizenCorner_AsPer_WebSite             

        public DataTable CitizenCorner_TabsList()
        {
            DataTable dt = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CitizenCorner_TabsList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahakar.Models.CooperativesModel", FunctionName = "public DataTable CitizenCorner_TabsList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable AuditFinancialYear()
        {
            DataTable dt = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AuditFinancialYearWeb_MobileApp", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahakar.Models.CooperativesModel", FunctionName = "public DataTable AuditFinancialYear()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        #region     --START--   CitizenCorner_Tabs_Internal_Working     
        // MODEL CLASS
        // File : Models/SocietyModel.cs
        public DataTable SearchSociety(string SearchValue = "", string SearchType = "", int PageNo = 1, int Limit = 10)
        {
            DataTable dt = new DataTable();

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyDetails_CitizenCorner_SOCIETY_REG_STATUS", DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@SearchValue", DBFactoryParameterType.NVarchar, SearchValue);
                objDBAccess.AddParameterToCommand("@SearchType", DBFactoryParameterType.NVarchar, SearchType);

                objDBAccess.AddParameterToCommand("@PageNo", DBFactoryParameterType.Int, PageNo);
                objDBAccess.AddParameterToCommand("@Limit", DBFactoryParameterType.Int, Limit);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(
                    new ErrorLogEntity()
                    {
                        PageName = "Cooperative.Models.MobileDashboardModel",
                        FunctionName = "public DataTable SearchSociety(string SearchValue = '', string SearchType = '', int PageNo = 1, int Limit = 10)"
                    }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }
        public DataTable GetGccApplicationStatus(string JanAadhaarNo = "", string SearchValue = "", int PageNo = 1, int Limit = 10)
        {
            DataTable dt = new DataTable();

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("GetGccApplicationStatus_CitizenCorner", DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@JanAadhaarNoEnc", DBFactoryParameterType.VARCHAR, JanAadhaarNo);
                objDBAccess.AddParameterToCommand("@SearchValue", DBFactoryParameterType.VARCHAR, SearchValue);

                objDBAccess.AddParameterToCommand("@PageNo", DBFactoryParameterType.Int, PageNo);
                objDBAccess.AddParameterToCommand("@Limit", DBFactoryParameterType.Int, Limit);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(
                    new ErrorLogEntity()
                    {
                        PageName = "Cooperative.Models.MobileDashboardModel",
                        FunctionName = "public DataTable GetGccApplicationStatus(string JanAadhaarNo = '', string SearchValue ='', int PageNo = 1, int Limit = 10)"
                    }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }
        public DataSet GetFarmerBeneficiaryDetails(int DistrictId = 0, string SearchValue = "", int PageNo = 1, int Limit = 10)
        {
            DataSet ds = new DataSet();

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("GetFarmerBeneficiaryDetails_CitizenCorner", DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@SearchValue", DBFactoryParameterType.VARCHAR, SearchValue);

                objDBAccess.AddParameterToCommand("@PageNo", DBFactoryParameterType.Int, PageNo);
                objDBAccess.AddParameterToCommand("@Limit", DBFactoryParameterType.Int, Limit);

                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(
                    new ErrorLogEntity()
                    {
                        PageName = "Cooperative.Models.MobileDashboardModel",
                        FunctionName = "public DataSet GetFarmerBeneficiaryDetails(int DistrictId = 0, string SearchValue = '', int PageNo = 1, int Limit = 10)"
                    }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return ds;
        }
        public DataSet GetSocietiesAuditReport(int SocietyTypeId, int IsRuralNotUrban, int DistrictId, int CityId = 0, int BlockId = 0, int GPId = 0, string SearchValue = "", int FinancialYearId = 0, int PageNo = 1, int Limit = 10)
        {
            DataSet ds = new DataSet();

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand(
                    "GetSocietiesAuditReport_CitizenCorner",
                    DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, SocietyTypeId);

                objDBAccess.AddParameterToCommand("@IsRuralNotUrban", DBFactoryParameterType.Int, IsRuralNotUrban);

                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);

                objDBAccess.AddParameterToCommand("@CityId", DBFactoryParameterType.Int, CityId);

                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, BlockId);

                objDBAccess.AddParameterToCommand("@GPId", DBFactoryParameterType.Int, GPId);

                objDBAccess.AddParameterToCommand("@SearchValue", DBFactoryParameterType.NVarchar, SearchValue);

                objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, FinancialYearId);

                objDBAccess.AddParameterToCommand("@PageNo", DBFactoryParameterType.Int, PageNo);

                objDBAccess.AddParameterToCommand("@Limit", DBFactoryParameterType.Int, Limit);

                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(
                    new ErrorLogEntity()
                    {
                        PageName = "Cooperative.Models.MobileDashboardModel",
                        FunctionName = "public DataSet GetSocietiesAuditReport(int SocietyTypeId, int IsRuralNotUrban, int DistrictId, int CityId = 0, int BlockId = 0, int GPId = 0, string SearchValue = '', int FinancialYearId = 0, int PageNo = 1, int Limit = 10)"
                    }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return ds;
        }
        public DataSet GetSocietyDetails_CitizenCorner_SearchByNameRegNo(int SearchType = 0, string SearchValue = "", string Status = "", int SocietyTypeId = 0, int ActId = 0, int PageNo = 1, int Limit = 10)
        {
            DataSet ds = new DataSet();
            // Proper JSON Log //
            string jsonResult = "";

            try
            {
                objDBAccess.OpenConnection();

                objDBAccess.CreateCommand("GetSocietyDetails_CitizenCorner_SearchByNameRegNo",
                    DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@SearchType", DBFactoryParameterType.Int, SearchType);
                objDBAccess.AddParameterToCommand("@SearchValue", DBFactoryParameterType.NVarchar, SearchValue);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, Status);

                objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.NVarchar, SocietyTypeId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, ActId);

                objDBAccess.AddParameterToCommand("@PageNo", DBFactoryParameterType.Int, PageNo);
                objDBAccess.AddParameterToCommand("@Limit", DBFactoryParameterType.Int, Limit);

                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Models.MobileDashboardModel",
                    FunctionName = "public DataSet GetSocietyDetails_CitizenCorner_SearchByNameRegNo(int SearchType = 0, string SearchValue = '', string Status = '', int SocietyTypeId = 0, int ActId = 0, int PageNo = 1, int Limit = 10)"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
                }
                else
                {
                    jsonResult = "Table to Row data not found in ds";
                }
            }
            catch
            {
                jsonResult = "Serialization Failed";
            }

            Exception exObj = new Exception();

            ErrorLogs.LogError(new ErrorModel()
            {
                PageName = "Cooperative.Models.MobileDashboardModel",
                FunctionName = "public DataSet GetSocietyDetails_CitizenCorner_SearchByNameRegNo(int SearchType = 0, string SearchValue = '', string Status = '', int SocietyTypeId = 0, int ActId = 0, int PageNo = 1, int Limit = 10) ;" + " < jsonResult > : " + jsonResult
            }, exObj);

            return ds;
        }

        #endregion  --END----   CitizenCorner_Tabs_Internal_Working

        #endregion  --END----   CitizenCorner_AsPer_WebSite

        #region     --START--   SocietyAmendment1958_DashboardData_AsPer_LoginUser
        public DataTable GetSocietyAmendment1958_Count_StatusWise_UserRoleWise(int UserRoleMappingId, int DistrictId = 0, int DivisionId = 0)
        {
            DataTable dt = new DataTable();

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("Get_SocietyAmendment1958_Count_StatusWise", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserRoleMappingId", DBFactoryParameterType.Int, UserRoleMappingId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Models.MobileDashboardModel",
                    FunctionName = "public DataTable GetSocietyAmendment1958_Count_StatusWise_UserRoleWise(int UserRoleMappingId, int DistrictId = 0, int DivisionId = 0)"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable Get_SocietyAmendment1958_DashboardCount_StatusWise_UserRoleWise(int UserRoleMappingId, int DistId, int DivisionId = 0)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = GetSocietyAmendment1958_Count_StatusWise_UserRoleWise(UserRoleMappingId, DistId, DivisionId);
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Models.MobileDashboardModel",
                    FunctionName = "public DataTable Get_SocietyAmendment1958_DashboardCount_StatusWise_UserRoleWise(int UserRoleMappingId, int DistId, int DivisionId = 0)"
                }, ex);
            }

            return dt;
        }

        public DataSet Get_SocietyAmendment1958_Details_UserRoleWise_SearchByNameRegNo(string Status, int DistrictId, int BlockId, int GramPanchayatId, int VillageId, int CityId, int WardId, int DivisionId = 0, int IsRuralNotUrban = 1, int SocietyTypeId = 0, int UserRoleMappingId = 0, string SocietyName_RegistrationNo = "", int PageNo = 1, int Limit = 10)
        {
            DataSet ds = new DataSet();

            try
            {
                objDBAccess.OpenConnection();

                objDBAccess.CreateCommand("Get_SocietyAmendment1958_Details_UserRoleWise",
                    DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@UserRoleMappingId", DBFactoryParameterType.Int, UserRoleMappingId);

                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, Status);
                objDBAccess.AddParameterToCommand("@SocietyName_RegistrationNo", DBFactoryParameterType.NVarchar, SocietyName_RegistrationNo);
                objDBAccess.AddParameterToCommand("@PageNo", DBFactoryParameterType.Int, PageNo);
                objDBAccess.AddParameterToCommand("@Limit", DBFactoryParameterType.Int, Limit);

                objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, SocietyTypeId);

                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);

                objDBAccess.AddParameterToCommand("@IsRuralNotUrban", DBFactoryParameterType.Int, IsRuralNotUrban);  ///--- The Value of IsRuralNotUrban = 1 Then "Rural" Else if IsRuralNotUrban = 0 then "Urban". Use For Society Location Filter. //

                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, BlockId);
                objDBAccess.AddParameterToCommand("@GramPanchayatId", DBFactoryParameterType.Int, GramPanchayatId);
                objDBAccess.AddParameterToCommand("@VillageId", DBFactoryParameterType.Int, VillageId);

                objDBAccess.AddParameterToCommand("@CityId", DBFactoryParameterType.Int, CityId);
                objDBAccess.AddParameterToCommand("@WardId", DBFactoryParameterType.Int, WardId);


                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Models.MobileDashboardModel",
                    FunctionName = "public DataSet Get_SocietyAmendment1958_Details_UserRoleWise_SearchByNameRegNo(string Status, int DistrictId, int BlockId, int GramPanchayatId, int VillageId, int CityId, int WardId, int DivisionId = 0, int IsRuralNotUrban = 1, int SocietyTypeId = 0, int UserRoleMappingId = 0, string SocietyName_RegistrationNo = '', int PageNo = 1, int Limit = 10)"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return ds;
        }

        public DataSet Get_SocietyAmendment1958_MoreSummary_UserRoleWise(int SocietyId, string SocietySummaryType = "DOCUMENTS", int AmendmentId = 0, int PageNo = 1, int Limit = 10)
        {
            DataSet ds = new DataSet();

            try
            {
                objDBAccess.OpenConnection();

                objDBAccess.CreateCommand("Get_SocietyAmendment1958_UserRoleWise_MoreSummary",
                    DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, SocietyId);
                objDBAccess.AddParameterToCommand("@SocietySummaryType", DBFactoryParameterType.NVarchar, SocietySummaryType);

                objDBAccess.AddParameterToCommand("@AmendmentId", DBFactoryParameterType.Int, AmendmentId);
                objDBAccess.AddParameterToCommand("@PageNo", DBFactoryParameterType.Int, PageNo);
                objDBAccess.AddParameterToCommand("@Limit", DBFactoryParameterType.Int, Limit);

                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Models.MobileDashboardModel",
                    FunctionName = "public DataSet Get_SocietyAmendment1958_MoreSummary_UserRoleWise(int SocietyId, string SocietySummaryType = 'DOCUMENTS', int AmendmentId = 0, int PageNo = 1, int Limit = 10)"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return ds;
        }

        public DataSet Get_SocietyAmendment1958_OtherDetails_UserRoleWise(int SocietyId, string SocietyIdEnc = "", int AmendmentId = 0, string OtherDetailsType = "ALL", int PageNo = 1, int Limit = 10)
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();

                objDBAccess.CreateCommand("Get_SocietyAmendment1958_UserRoleWise_OtherDetails",
                    DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, SocietyId);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                objDBAccess.AddParameterToCommand("@AmendmentId", DBFactoryParameterType.Int, AmendmentId);

                objDBAccess.AddParameterToCommand("@OtherDetailsType", DBFactoryParameterType.NVarchar, OtherDetailsType);
                objDBAccess.AddParameterToCommand("@PageNo", DBFactoryParameterType.Int, PageNo);
                objDBAccess.AddParameterToCommand("@Limit", DBFactoryParameterType.Int, Limit);

                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Models.MobileDashboardModel",
                    FunctionName = "public DataSet Get_SocietyAmendment1958_OtherDetails_UserRoleWise(int SocietyId, string SocietyIdEnc ='', int AmendmentId = 0, string OtherDetailsType = 'ALL', int PageNo = 1, int Limit = 10)"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return ds;
        }
        #endregion  --END----   SocietyAmendment1958_DashboardData_AsPer_LoginUser

        #region     MobileApp_ActionClick_WebPageURL  --START--
        public DataTable Get_ActionClick_WebPageURL(string TabName, string ActionName)
        {
            dt?.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Get_ActionClick_WebPageURL", DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@TabName", DBFactoryParameterType.NVarchar, TabName);
                objDBAccess.AddParameterToCommand("@ActionName", DBFactoryParameterType.NVarchar, ActionName);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileDashboardModel", FunctionName = "public DataTable Get_ActionClick_WebPageURL(string TabName, string ActionName)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable Get_ActionClick_WebPageURL_AsPerParams(WebPageURL_Request objWebPageURL_Request)
        {
            dt?.Clear();
            if (objWebPageURL_Request != null)
            {
                try
                {
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("Get_ActionClick_WebPageURL_AsPerParams", DBFactoryCommandType.PROCEDURE_COMMAND);

                    objDBAccess.AddParameterToCommand("@TabName", DBFactoryParameterType.NVarchar, objWebPageURL_Request.TabName);
                    objDBAccess.AddParameterToCommand("@ActionName", DBFactoryParameterType.NVarchar, objWebPageURL_Request.ActionName);

                    objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, objWebPageURL_Request.SocietyIDEnc);
                    objDBAccess.AddParameterToCommand("@SocietyID", DBFactoryParameterType.NVarchar, objWebPageURL_Request.SocietyID);
                    objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, objWebPageURL_Request.RegistrationNo);
                    objDBAccess.AddParameterToCommand("@SocietyName", DBFactoryParameterType.NVarchar, objWebPageURL_Request.SocietyName);

                    objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.NVarchar, objWebPageURL_Request.StatusId);
                    objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, objWebPageURL_Request.ActId);

                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, objWebPageURL_Request.DistrictId);
                    objDBAccess.AddParameterToCommand("@UserRoleMappingId", DBFactoryParameterType.NVarchar, objWebPageURL_Request.UserRoleMappingId);
                    objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, objWebPageURL_Request.UserId);
                    objDBAccess.AddParameterToCommand("@IsAmendment", DBFactoryParameterType.NVarchar, objWebPageURL_Request.IsAmendment);

                    dt = objDBAccess.GetTable();
                }
                catch (Exception ex)
                {
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileDashboardModel", FunctionName = "public DataTable Get_ActionClick_WebPageURL_AsPerParams(WebPageURL_Request objWebPageURL_Request)" }, ex);
                }
                finally
                {
                    objDBAccess.CloseConnection();
                }
            }
            return dt;
        }
        #endregion  MobileApp_ActionClick_WebPageURL  ----END--

        #region     Society_UnderProcess_Pending_To_Assign  ---START--
        public DataTable GetApplicationFlowSociety(string SocietyIDEnc)
        {
            dt?.Clear();
            try
            {
                objDBAccess.OpenConnection();
                //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess.CreateCommand("GetApplicationFlowSociety", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyRegiNoOrSocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@IsRegiNo", DBFactoryParameterType.Int, 0);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileDashboardModel", FunctionName = "public DataTable GetApplicationFlowSociety(string " + SocietyIDEnc + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetUserForSocietyAssign(string SocietyID)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess.CreateCommand("GetUserForSocietyAssign", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyID);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileDashboardModel", FunctionName = "public DataTable GetUserForSocietyAssign(string " + SocietyID + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        #endregion  Society_UnderProcess_Pending_To_Assign  _____END__

        #endregion Ritesh_Mobile_App_ModelMethods


        #region GetUnderProcessSociety On 11-05-2026

        public DataSet GetUnderProcessSocietyDetailsByActUserWiseSearchByAct(int UserRoleMappingId = 0, string ActId = "", int IsOnline = 0, string Status = "", int SocietyTypeId = 0, int DistrictId = 0, int DivisionId = 0, int BlockId = 0, int GPId = 0, int VillageId = 0, int CityId = 0, int WardId = 0, int IsRuralNotUrban = 1, int PageNo = 1, int Limit = 10)
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                objDBAccess.CreateCommand("GetUnderProcessSocietyDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserRoleMappingId", DBFactoryParameterType.Int, UserRoleMappingId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, !string.IsNullOrEmpty(ActId) ? Convert.ToInt32(ActId) : 0);
                objDBAccess.AddParameterToCommand("@IsOnline", DBFactoryParameterType.Int, IsOnline);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, Status);
                objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, SocietyTypeId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, BlockId);
                objDBAccess.AddParameterToCommand("@GPId", DBFactoryParameterType.Int, GPId);
                objDBAccess.AddParameterToCommand("@VillageId", DBFactoryParameterType.Int, VillageId);
                objDBAccess.AddParameterToCommand("@CityId", DBFactoryParameterType.Int, CityId);
                objDBAccess.AddParameterToCommand("@WardId", DBFactoryParameterType.Int, WardId);
                objDBAccess.AddParameterToCommand("@IsRuralNotUrban", DBFactoryParameterType.Int, IsRuralNotUrban);
                objDBAccess.AddParameterToCommand("@IsAmendment", DBFactoryParameterType.Bit, 0);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "GetSocietyDetails");
                objDBAccess.AddParameterToCommand("@PageNo", DBFactoryParameterType.Int, PageNo);
                objDBAccess.AddParameterToCommand("@Limit", DBFactoryParameterType.Int, Limit);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Models.MobileDashboardModel",
                    FunctionName = "public DataSet GetUnderProcessSocietyDetailsByActUserWiseSearchByAct(int UserRoleMappingId = 0, string ActId = " + ActId + ", int IsOnline = 0, string Status = " + Status + ", int SocietyTypeId = 0, int DistrictId = 0, int DivisionId = 0, int BlockId = 0, int GPId = 0, int VillageId = 0, int CityId = 0, int WardId = 0, int IsRuralNotUrban = 1, int PageNo = 1, int Limit = 10)"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return ds;
        }

        public DataSet GetUnderProcessSocietyProceesStatusList(SocietyStatusListModel requestModel)
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetUnderProcessSocietyProceesStatusList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, requestModel.SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, requestModel.ActId);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, requestModel.StatusId);
                objDBAccess.AddParameterToCommand("@Flag", DBFactoryParameterType.Int, requestModel.Flag);
                objDBAccess.AddParameterToCommand("@IsAmendment", DBFactoryParameterType.Bit, requestModel.IsAmendment);
                objDBAccess.AddParameterToCommand("@AmendmentId", DBFactoryParameterType.Int, requestModel.AmendmentId);

                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileDashboardModel", FunctionName = "GetSocietyProceesStatusListById(int StatusId, int WorkOnActId, string SocietyIDEnc, int Flag = 0, bool IsAmendment = false, int AmendmentId = 0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return ds;
        }

        public DataSet GetRoleListForApplicationStatus(SocietyStatusListModel requestModel)
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetRoleListForApplicationStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, requestModel.SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@NextStatusId", DBFactoryParameterType.Int, requestModel.NextStatusId);
                objDBAccess.AddParameterToCommand("@CurrentStatusId", DBFactoryParameterType.Int, requestModel.CurrentStatusId);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileDashboardModel", FunctionName = "GetRoleListForApplicationStatus(string SocietyIDEnc, int NextStatusId, int CurrentStatusId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();

            }

            return ds;
        }

        public DataSet GetOfficerDetailBySocietyID(SocietyStatusListModel requestModel)
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetOfficeresNameListOfSocietyDistrict", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, requestModel.SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, requestModel.RoleId);
                objDBAccess.AddParameterToCommand("@NextStatusId", DBFactoryParameterType.Int, requestModel.NextStatusId);
                objDBAccess.AddParameterToCommand("@CurrentStatusId", DBFactoryParameterType.Int, requestModel.CurrentStatusId);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileDashboardModel", FunctionName = " GetOfficerDetailBySocietyID(string SocietyIDEnc,int NextStatusId, int CurrentStatusId )" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return ds;
        }
        public int SaveSocietyObjectionDetails(SaveSocietyObjectionRequestModel RequestModel)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyObjectionDetailPermanentSave", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, RequestModel.SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@ObjectionFlagBasic", DBFactoryParameterType.Bit, RequestModel.ObjectionFlagBasic);
                objDBAccess.AddParameterToCommand("@ObjectionFlagAdmin", DBFactoryParameterType.Bit, RequestModel.ObjectionFlagAdmin);
                objDBAccess.AddParameterToCommand("@ObjectionFlagOffice", DBFactoryParameterType.Bit, RequestModel.ObjectionFlagOffice);
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileDashboardModel", FunctionName = " SaveSocietyObjectionDetails(string SocietyIDEnc )" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public DataSet GetUnderProcessSocietyDetailsByActUserWiseSearchByActNew(int UserRoleMappingId = 0, string ActId = "", int IsOnline = 0, string Status = "", int SocietyTypeId = 0, int DistrictId = 0, int DivisionId = 0, int BlockId = 0, int GPId = 0, int VillageId = 0, int CityId = 0, int WardId = 0, int IsRuralNotUrban = 1, int PageNo = 1, int Limit = 10)
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                objDBAccess.CreateCommand("GetSocietyApplicationFlowList_New", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, ActId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "GetSocietyDetails");
                objDBAccess.AddParameterToCommand("@IsAmendment", DBFactoryParameterType.Bit, 0);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, 0);
                objDBAccess.AddParameterToCommand("@UserRoleMappingId", DBFactoryParameterType.Int, UserRoleMappingId);
                objDBAccess.AddParameterToCommand("@IsOnline", DBFactoryParameterType.Int, IsOnline);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, Status);
                objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, SocietyTypeId);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, BlockId);
                objDBAccess.AddParameterToCommand("@GPId", DBFactoryParameterType.Int, GPId);
                objDBAccess.AddParameterToCommand("@VillageId", DBFactoryParameterType.Int, VillageId);
                objDBAccess.AddParameterToCommand("@IsRuralNotUrban", DBFactoryParameterType.Int, IsRuralNotUrban);
                objDBAccess.AddParameterToCommand("@CityId", DBFactoryParameterType.Int, CityId);
                objDBAccess.AddParameterToCommand("@WardId", DBFactoryParameterType.Int, WardId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                ErrorLogs.LogError(new ErrorModel()
                {
                    PageName = "Cooperative.Models.MobileDashboardModel",
                    FunctionName = "public DataSet GetUnderProcessSocietyDetailsByActUserWiseSearchByActNew(int UserRoleMappingId = 0, string ActId = " + ActId + ", int IsOnline = 0, string Status = " + Status + ", int SocietyTypeId = 0, int DistrictId = 0, int DivisionId = 0, int BlockId = 0, int GPId = 0, int VillageId = 0, int CityId = 0, int WardId = 0, int IsRuralNotUrban = 1, int PageNo = 1, int Limit = 10)"
                }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return ds;
        }

        public int SocietyApplicationFlow(string SocietyIDEnc, int StatusId, string Remark, long UserId, long LoggedInUserId, string SSOID, bool IsAmendment = false, string AmendmendAmount = "", string ReceiptNumber = "")
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertSocietyApplicationFlow", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.VARCHAR, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Remark);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, UserId);
                objDBAccess.AddParameterToCommand("@IsAmendment", DBFactoryParameterType.Bit, IsAmendment);
                objDBAccess.AddParameterToCommand("@CreateIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.BigInt, LoggedInUserId);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOID);
                objDBAccess.AddParameterToCommand("@AmendmendAmount", DBFactoryParameterType.NVarchar, AmendmendAmount);
                objDBAccess.AddParameterToCommand("@ReceiptNumber", DBFactoryParameterType.NVarchar, ReceiptNumber);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());



            }
            catch (Exception ex)
            {
                Result = 0;
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "AddApplicationFlow(string SocietyIDEnc, int StatusId, string Remark,int UserId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                //SendMSG(SocietyIDEnc, StatusId);

            }
            return Result;
        }


        #endregion


    }

    public class SocietyDashboardMobile
    {
        public List<GetSocietyCountActWise_Result> SocietyData { get; set; }
        public List<GetSocietyCountActDistricWise_Result> DistrictWiseSocietyData2001 { get; set; }
        public List<GetSocietyCountActDistricWise_Result> DistrictWiseSocietyData2005 { get; set; }
        public List<GetSocietyCountActDistricWise_Result> DistrictWiseSocietyData1958 { get; set; }
    }

    public partial class GetSocietyCountActWise_Result
    {
        public string ActNameEn { get; set; }
        public string ActNameHi { get; set; }
        public Nullable<int> ActId { get; set; }
        public Nullable<int> TotalSocieties { get; set; }
        public int? TotalAppli { get; set; }
        public int? RegiAppli { get; set; }
        public int? RejeAppli { get; set; }
        public int? PartialAppli { get; set; }
        public int? UnderProAppli { get; set; }
    }

    public partial class GetSocietyCountActDistricWise_Result
    {
        public string DISTRICT_ENG { get; set; }
        public string DISTRICT_MANGAL { get; set; }
        public Nullable<int> TotalSocieties { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class GetSocietyDetailsActDistricWiseUserRoleWise_Result
    {
        public int SrNo { get; set; }
        public int SocietyID { get; set; }
        public string SocietyName { get; set; }
        public string SocietyTypeNameEng { get; set; }
        public string SocietyTypeNameHin { get; set; }
        public string RegistrationNo { get; set; }
        public string RegistrationDate { get; set; }
        public string SocietyStatus { get; set; }
        //public int? StatusId { get; set; }

        public string OnlineOffline { get; set; }
        //public bool? IsOnline { get; set; }

        //public int? BlockId { get; set; }
        public string BLOCK_ENG { get; set; }
        public string BLOCK_MANGAL { get; set; }

        //public int? GramPanchayatId { get; set; }
        public string GP_ENG { get; set; }
        public string GP_MANGAL { get; set; }

        //public int? VillageId { get; set; }
        public string VILLAGE_ENG { get; set; }
        public string VILLAGE_MANGAL { get; set; }

        //public int? DistrictId { get; set; }
        public string DISTRICT_ENG { get; set; }
        public string DISTRICT_MANGAL { get; set; }
    }

    public class SocietyDetailsMobileResponse
    {
        public List<GetSocietyDetailsActDistricWiseUserRoleWise_Result> UserRoleActDistrictWiseSocietyDetails { get; set; }
        public int TotalRecords { get; set; }
    }

    public class Mobile_LoginEntity_Citizen
    {
        public bool IsUserExists { get; set; } = false;
        public int UserId { get; set; } = 0;
        public string SSO_ID { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;
        public string OfficerNameEn { get; set; } = string.Empty;
        public string OfficerNameHi { get; set; } = string.Empty;

        public int RoleId { get; set; } = 0;
        public string RoleName_EN { get; set; } = string.Empty;
        public string RoleName_HI { get; set; } = string.Empty;

        public string ActId { get; set; } = string.Empty;   // "1,3,7"
        public int OfficeId { get; set; } = 0;

        public int UserRoleMappingId { get; set; } = 0;
        public int UserLevelId { get; set; } = 0;

        public int? DistrictId { get; set; }   // nullable
        public string DISTRICT_ENG { get; set; }
        public string DISTRICT_MANGAL { get; set; }

        public int UserType { get; set; } = 0;
    }


    public class SocietyStatusListModel
    {
        public string StatusId { get; set; }
        public string ActId { get; set; }
        public string SocietyIDEnc { get; set; }
        public int Flag { get; set; } = 0;
        public bool IsAmendment { get; set; } = false;
        public int AmendmentId { get; set; } = 0;
        public int NextStatusId { get; set; }
        public int CurrentStatusId { get; set; }
        public int RoleId { get; set; }
    }


    public class SaveSocietyObjectionRequestModel
    {
        public string SocietyIDEnc { get; set; }
        public bool ObjectionFlagBasic { get; set; }
        public bool ObjectionFlagAdmin { get; set; }
        public bool ObjectionFlagOffice { get; set; }
        public int StatusId { get; set; }
        public string Remark { get; set; }
        public long UserId { get; set; } = 0;
        public bool IsAmendment { get; set; } = false;
        public string OTP { get; set; }
        public string TXN { get; set; }
        public string AmendmendAmount { get; set; }
        public string ReceiptNumber { get; set; }
        public long OfficeId { get; set; }
        public long LoggedUserId { get; set; }
        public string SSOId { get; set; }
    }
}