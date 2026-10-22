using Cooperative.Controllers;
using Cooperative.DBConnector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using PdfSharp.Pdf.Content.Objects;
namespace Cooperative.Models
{
    public class KPIModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public KPIModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public KPIModel()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

        //old
        public int SaveKPIResult(List<KPIResultModel> model)
        {
            int Result = 0;
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            try
            {
                objDBAccess.OpenConnection();

                foreach (var item in model)
                {
                    SqlCommand cmd = objDBAccess.CreateCommand("USP_SaveSAKPIResult", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@KPIFor", DBFactoryParameterType.VARCHAR, item.KPIFor);
                    //objDBAccess.AddParameterToCommand("@FY", DBFactoryParameterType.VARCHAR, item.FY);
                    objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, item.Month);
                    //old
                    //objDBAccess.AddParameterToCommand("@OfficeId", DBFactoryParameterType.Int, item.OfficeId);
                    objDBAccess.AddParameterToCommand("@OfficeId", DBFactoryParameterType.Int, item.KPIFor == "PACS" ? _LoggedInUser.GccId : item.OfficeId);
                    objDBAccess.AddParameterToCommand("@ResionId", DBFactoryParameterType.Int, item.ResionId);
                    objDBAccess.AddParameterToCommand("@KPIIndicatorsId", DBFactoryParameterType.Int, item.KPIIndicatorsId);
                    objDBAccess.AddParameterToCommand("@Target", DBFactoryParameterType.Float, item.KPIIndicatorsTarget);
                    objDBAccess.AddParameterToCommand("@CCBId", DBFactoryParameterType.Int, item.CCBId);
                    objDBAccess.AddParameterToCommand("@KPI_MarksId", DBFactoryParameterType.Int, item.KPI_MarksId);
                    objDBAccess.AddParameterToCommand("@Achievement", DBFactoryParameterType.Float, item.KPIIndicatorsAchievement);
                    objDBAccess.AddParameterToCommand("@ProgressPercentage", DBFactoryParameterType.Float, item.ProgressPercentage);
                    objDBAccess.AddParameterToCommand("@WeightedMarks", DBFactoryParameterType.Float, item.WeightedMarks);
                    objDBAccess.AddParameterToCommand("@MarksObtained", DBFactoryParameterType.Float, item.MarksObtained);
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@UserRoleMappingDepId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                    //local
                    objDBAccess.AddParameterToCommand("@PLDBId", DBFactoryParameterType.Int, (_LoggedInUser.RoleId == 1 || _LoggedInUser.RoleId == 10235) ? item.PLDBId : _LoggedInUser.PLDBId);
                    //live
                    //objDBAccess.AddParameterToCommand("@PLDBId", DBFactoryParameterType.Int, (_LoggedInUser.RoleId == 10235) ? item.PLDBId : _LoggedInUser.PLDBId);

                    objDBAccess.ExecuteCommand();
                    Result++;
                }
                //Result = 1;   
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "KPIModel", FunctionName = "SaveKPIResult" }, ex);
                Result = 0;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }


        //New
        //public int SaveKPIResult(List<KPIResultModel> model, bool isDevelopment = false)
        //{
        //    int Result = 0;
        //    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        foreach (var item in model)
        //        {
        //            SqlCommand cmd = objDBAccess.CreateCommand("USP_SaveSAKPIResult", DBFactoryCommandType.PROCEDURE_COMMAND);
        //            objDBAccess.AddParameterToCommand("@KPIFor", DBFactoryParameterType.VARCHAR, item.KPIFor);
        //            //objDBAccess.AddParameterToCommand("@FY", DBFactoryParameterType.VARCHAR, item.FY);
        //            objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, item.Month);
        //            //old
        //            //objDBAccess.AddParameterToCommand("@OfficeId", DBFactoryParameterType.Int, item.OfficeId);
        //            objDBAccess.AddParameterToCommand("@OfficeId", DBFactoryParameterType.Int, item.KPIFor == "PACS" ? _LoggedInUser.GccId : item.OfficeId);
        //            objDBAccess.AddParameterToCommand("@KPIIndicatorsId", DBFactoryParameterType.Int, item.KPIIndicatorsId);
        //            objDBAccess.AddParameterToCommand("@Target", DBFactoryParameterType.Float, item.KPIIndicatorsTarget);
        //            objDBAccess.AddParameterToCommand("@CCBId", DBFactoryParameterType.Int, item.CCBId);
        //            objDBAccess.AddParameterToCommand("@KPI_MarksId", DBFactoryParameterType.Int, item.KPI_MarksId);
        //            objDBAccess.AddParameterToCommand("@Achievement", DBFactoryParameterType.Float, item.KPIIndicatorsAchievement);
        //            objDBAccess.AddParameterToCommand("@ProgressPercentage", DBFactoryParameterType.Float, item.ProgressPercentage);
        //            objDBAccess.AddParameterToCommand("@WeightedMarks", DBFactoryParameterType.Float, item.WeightedMarks);
        //            objDBAccess.AddParameterToCommand("@MarksObtained", DBFactoryParameterType.Float, item.MarksObtained);
        //            objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
        //            objDBAccess.AddParameterToCommand("@UserRoleMappingDepId", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
        //            //local
        //            if (isDevelopment) { objDBAccess.AddParameterToCommand("@PLDBId", DBFactoryParameterType.Int, (_LoggedInUser.RoleId == 1 || _LoggedInUser.RoleId == 10235) ? item.PLDBId : _LoggedInUser.PLDBId); }
        //            else
        //            { objDBAccess.AddParameterToCommand("@PLDBId", DBFactoryParameterType.Int, (_LoggedInUser.RoleId == 10235) ? item.PLDBId : _LoggedInUser.PLDBId); }

        //            if (item.KPIFor == "PACS")
        //            {
        //                objDBAccess.AddParameterToCommand("@CCBDistrictId", DBFactoryParameterType.Int, item.CCBDistrictId);
        //                objDBAccess.AddParameterToCommand("@CCBName", DBFactoryParameterType.NVarchar, item.CCBName);
        //                objDBAccess.AddParameterToCommand("@CCBDistrictName", DBFactoryParameterType.NVarchar, item.CCBDistrictName);
        //                objDBAccess.AddParameterToCommand("@CCBBranchName", DBFactoryParameterType.NVarchar, item.CCBBranchName);
        //                objDBAccess.AddParameterToCommand("@PacsManagerName", DBFactoryParameterType.NVarchar, item.PacsManagerName);
        //                objDBAccess.AddParameterToCommand("@PacsName", DBFactoryParameterType.NVarchar, item.PacsName);
        //                objDBAccess.AddParameterToCommand("@PacsRegistrationNo", DBFactoryParameterType.NVarchar, item.PacsRegistrationNo);
        //                objDBAccess.AddParameterToCommand("@KCCLoanDisbursedAmt", DBFactoryParameterType.VARCHAR, item.KCCLoanDisbursedAmt);
        //                objDBAccess.AddParameterToCommand("@AgricultureTrunOver", DBFactoryParameterType.VARCHAR, item.AgricultureTrunOver);
        //                objDBAccess.AddParameterToCommand("@MembershipPreviousInd", DBFactoryParameterType.Int, item.MembershipPrevious);
        //                objDBAccess.AddParameterToCommand("@MembershipCurrentInd", DBFactoryParameterType.Int, item.MembershipCurrent);
        //                objDBAccess.AddParameterToCommand("@SharePreviousInd", DBFactoryParameterType.Int, item.ShareCapitalPrevious);
        //                objDBAccess.AddParameterToCommand("@ShareCurrentInd", DBFactoryParameterType.Int, item.ShareCapitalCurrent);
        //                objDBAccess.AddParameterToCommand("@selectOptionAnyOther1", DBFactoryParameterType.Bit, item.selectOptionAnyOther1);
        //                objDBAccess.AddParameterToCommand("@selectOptionAnyOther2", DBFactoryParameterType.Bit, item.selectOptionAnyOther2);

        //            }

        //            objDBAccess.ExecuteCommand();
        //            Result++;
        //        }
        //        //Result = 1;   
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "KPIModel", FunctionName = "SaveKPIResult" }, ex);
        //        Result = 0;
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return Result;
        //}


        public DataTable GetPacsKPIDetails()
        {
            DataTable dt = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetPacsTable", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.KPIModel", FunctionName = "GetPacsKPIDetails()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }


        public DataTable GetBhandarKPIDetails()
        {
            DataTable dt = new DataTable();

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetBhandarTable", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.KPIModel", FunctionName = "GetBhandarKPIDetails()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable GetPLDBKPIDetails()
        {
            DataTable dt = new DataTable();

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetpldbTable", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.KPIModel", FunctionName = "GetPLDBKPIDetails()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }



        public DataTable SocietyListWithDepartmentUser(SocietyRegistrationEntity Obj)
        {
            dt.Clear();
            try
            {
                string WhereClause = " and 1=1 " + Obj.WhereClause;

                if (Obj != null && Obj.SSOID != null)
                    WhereClause += Obj.SSOID.Length > 0 ? (WhereClause = " and SSOID ='" + Obj.SSOID + "'") : "";

                if (Obj.SocietyIDEnc != null && Obj.UserType == "K")
                {
                    WhereClause += Obj.SocietyIDEnc.Length > 0 ? (WhereClause = " and Societies.[SocietyID] =cast( Dbo.Decrypt('" + Obj.SocietyIDEnc + "')as int) ") : "";
                }

                WhereClause += Obj.ActId > 0 ? (WhereClause = " and [Societies].ActId =" + Obj.ActId + "") : "";
                WhereClause += Obj.SocietyTypeId > 0 ? (WhereClause = " and Societies.[SocietyTypeId] =" + Obj.SocietyTypeId + "") : "";
                WhereClause += Obj.ClassId > 0 ? (WhereClause = " and Societies.[ClassId] =" + Obj.ClassId + "") : "";

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("KPI_SocietyList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, Obj.SSOID);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.KPIModel", FunctionName = "SocietyListWithDepartmentUser(SocietyRegistrationEntity Obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;

        }

        public DataTable GetKPIAllDetails(int officeId, int? pldbId, int month, string kpiFor, int DistrictId, bool isDevelopment = false, int resionId = 0, int DivisionId = 0)
        {
            DataTable dt = new DataTable();
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetKPIResult", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@OfficeId", DBFactoryParameterType.Int, officeId);
                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, month);
                //local
                if (isDevelopment) { objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.Int, (_LoggedInUser.RoleId == 1 || _LoggedInUser.RoleId == 10235) ? pldbId : _LoggedInUser.PLDBId); }
                else { objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.Int, (_LoggedInUser.RoleId == 10235) ? pldbId : _LoggedInUser.PLDBId); }
                objDBAccess.AddParameterToCommand("@KPIFor", DBFactoryParameterType.NVarchar, kpiFor);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                objDBAccess.AddParameterToCommand("@ResionId", DBFactoryParameterType.NVarchar, resionId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.NVarchar, DivisionId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.KPIModel", FunctionName = "GetKPIDetails(int officeId, int month, string kpiFor)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable GetKPIValidationDate(string kpiFor)
        {
            DataTable dt = new DataTable();
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetKPIValidationDate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@KPIFor", DBFactoryParameterType.NVarchar, kpiFor);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.KPIModel", FunctionName = "GetKPIValidationDate(string kpiFor)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable GetUserDetailsBySSO()
        {
            DataTable dt = new DataTable();
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetUserDetailsPLDBBySSO", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSO_ID", DBFactoryParameterType.VARCHAR, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@PLDBId", DBFactoryParameterType.Int, _LoggedInUser.PLDBId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.KPIModel", FunctionName = "GetUserDetailsBySSO(string SSOID)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable GetDivisionKPIStatus()
        {
            DataTable dt = new DataTable();
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetDivisionKPIStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserRoleMappingDepId", DBFactoryParameterType.BigInt, _LoggedInUser.UserRoleDeptMappingId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.KPIModel", FunctionName = "GetDivisionKPIStatus()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }


        public DataTable GetPLDBWiseAllDetails(int? pldbId, int month, string kpiFor)
        {
            DataTable dt = new DataTable();
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetKPIResultByPLDB", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, month);
                objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.Int, pldbId);
                //  objDBAccess.AddParameterToCommand("@OfficeId", DBFactoryParameterType.Int, officeId);
                objDBAccess.AddParameterToCommand("@KPIFor", DBFactoryParameterType.NVarchar, kpiFor);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.KPIModel", FunctionName = "GetPLDBWiseAllDetails(int month, string kpiFor)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable CheckPLDBAdminLock(int pldbId, int month, string kpiFor)
        {
            DataTable dt = new DataTable();
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_CheckPLDBAdminLock", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PLDBId", DBFactoryParameterType.Int, pldbId);
                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, month);
                objDBAccess.AddParameterToCommand("@KPIFor", DBFactoryParameterType.NVarchar, kpiFor);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.KPIModel", FunctionName = "CheckPLDBAdminLock(int pldbId, int month, string kpiFor)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable GetPLDBKPIResult(int pldbId, int month, string kpiFor)
        {
            DataTable dt = new DataTable();
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetPLDBKPIResult", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PLDBId", DBFactoryParameterType.Int, pldbId);
                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, month);
                objDBAccess.AddParameterToCommand("@KPIFor", DBFactoryParameterType.NVarchar, kpiFor);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.KPIModel", FunctionName = "GetPLDBKPIResult(int pldbId, int month, string kpiFor)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable GetPLDBKPIArea()
        {
            DataTable dt = new DataTable();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetPLDBKPIArea", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.KPIModel", FunctionName = "GetPLDBKPIArea()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }


        public DataTable GetPLDBKPIResult(string kpiFor, int month, string fy, int kpiAreaId)
        {
            DataTable dt = new DataTable();

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetPLDBKPIAresWiseResult", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@KPIFor", DBFactoryParameterType.NVarchar, kpiFor);
                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, month);
                //objDBAccess.AddParameterToCommand("@PLDBId", DBFactoryParameterType.Int, pldbId);
                objDBAccess.AddParameterToCommand("@FY", DBFactoryParameterType.NVarchar, fy);
                objDBAccess.AddParameterToCommand("@KPIAreaId", DBFactoryParameterType.Int, kpiAreaId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.KPIModel", FunctionName = "GetPLDBKPIResult(string kpiFor, int month, int pldbId, string fy, int kpiAreaId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }
        public DataSet GetDetailsByPacs(string actionType = "")
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetDetailsByPacs", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, actionType);
                objDBAccess.AddParameterToCommand("@GCCId", DBFactoryParameterType.BigInt, _LoggedInUser.GccId);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.BigInt, _LoggedInUser.UserRoleDeptMappingId);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.KPIModel", FunctionName = "GetDetailsByPacs(string actionType)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return ds;
        }

        public DataTable GetUnitOfficeKPIDetails()
        {
            DataTable dt = new DataTable();

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetUnitOfficeKPITable", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.KPIModel", FunctionName = "GetUnitOfficeKPIDetails()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }


        public DataTable GetUnitOfficeKPIArea()
        {
            DataTable dt = new DataTable();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetUnitOfficeKPIArea", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.KPIModel", FunctionName = "GetUnitOfficeKPIArea()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable GetUnitOfficeKPIReport(int month, string fy, int kpiAreaId)
        {
            DataTable dt = new DataTable();

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetUnitOfficeKPIReport", DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, month);
                objDBAccess.AddParameterToCommand("@FY", DBFactoryParameterType.NVarchar, fy);
                objDBAccess.AddParameterToCommand("@KPIAreaId", DBFactoryParameterType.Int, kpiAreaId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.KPIModel", FunctionName = "GetUnitOfficeKPIReport(int month, string fy, int kpiAreaId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }



    }
}