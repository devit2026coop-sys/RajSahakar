using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cooperative.Controllers;
using Newtonsoft.Json;
using static iTextSharp.text.pdf.AcroFields;

namespace Cooperative.Models
{
    public class AuditorNewModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;
        [Key]
        
        public long Id { get; set; }

        // Financial Year
        public string FY { get; set; }

        // Month
        public int Month { get; set; }

        public int DistrictId { get; set; }
        // Office
        public int? OfficeId { get; set; }

        // 1. Committee Type
        public int CommitteeTypeId { get; set; }

        // 2. Total Committees
        public int? TotalCommittees { get; set; }

        // 3. Total Allotment Department
        public int? TotalAllotmentDepartment { get; set; }

        // 4. Total Allotment CA
        public int? TotalAllotmentCA { get; set; }

        // 5. Balance Allocation
        public int? balanceAllocation { get; set; }

        // 6. Last Month Department
        public int? AllotmentDepartmentLastMonth { get; set; }

        // 7. Last Month CA
        public int? AllotmentCALastMonth { get; set; }

        // 8. Current Month Department
        public int? AllotmentDepartmentInMonth { get; set; }

        // 9. Current Month CA
        public int? AllotmentCAInMonth { get; set; }
        // 21. KPI Indicators


        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        //
        public AuditorNewModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public AuditorNewModel()
        {
            objDBAccess = new DBFactoryMSSQL();

        }


        public DataTable GetAuditCommiteeDetails()
        {
            DataTable dt = new DataTable();

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_AuditMprCommitteeBackYear", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.VARCHAR, _LoggedInUser.DistrictId);


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





        public int SaveAuditResult(List<AuditorNewModel> model)
        {
            int Result = 0;

            UserEntity _LoggedInUser =
                (UserEntity)System.Web.HttpContext.Current
                .Session[SessionIdentity.Admin];

            if (_LoggedInUser == null ||
                model == null ||
                model.Count == 0)
            {
                return 0;
            }

            try
            {
                objDBAccess.OpenConnection();

                foreach (var item in model)
                {
                    // Financial Year Validation
                    if (string.IsNullOrWhiteSpace(item.FY))
                        throw new Exception("Financial Year is required.");

                    // Month Validation
                    if (item.Month <= 0 || item.Month > 12)
                        throw new Exception("Valid Month is required.");

                    // Committee Type Validation
                    if (item.CommitteeTypeId <= 0)
                        throw new Exception("Committee Type is required.");

                    SqlCommand cmd = objDBAccess.CreateCommand(
                        "USP_SaveAuditMprCommitteeBackYear",
                        DBFactoryCommandType.PROCEDURE_COMMAND
                    );

                    // 1. Financial Year
                    objDBAccess.AddParameterToCommand(
                        "@FinancialYear",
                        DBFactoryParameterType.VARCHAR,
                        item.FY
                    );

                    // 2. Month
                    objDBAccess.AddParameterToCommand(
                        "@Month",
                        DBFactoryParameterType.Int,
                        item.Month
                    );

                    objDBAccess.AddParameterToCommand(
                        "@DistrictId",
                        DBFactoryParameterType.Int,
                       _LoggedInUser.DistrictId
                    );

                    // 3. Office
                    objDBAccess.AddParameterToCommand(
                        "@OfficeId",
                        DBFactoryParameterType.Int,
                        0
                    );

                    // 4. Committee Type
                    objDBAccess.AddParameterToCommand(
                        "@CommitteeTypeId",
                        DBFactoryParameterType.Int,
                        item.CommitteeTypeId
                    );

                    // 5. Total Committees
                    objDBAccess.AddParameterToCommand(
                        "@TotalCommittees",
                        DBFactoryParameterType.Int,
                        (object)item.TotalCommittees ?? DBNull.Value
                    );

                    // 6. Total Allotment Department
                    objDBAccess.AddParameterToCommand(
                        "@TotalAllotmentDepartment",
                        DBFactoryParameterType.Int,
                        (object)item.TotalAllotmentDepartment ?? DBNull.Value
                    );

                    // 7. Total Allotment CA
                    objDBAccess.AddParameterToCommand(
                        "@TotalAllotmentCA",
                        DBFactoryParameterType.Int,
                        (object)item.TotalAllotmentCA ?? DBNull.Value
                    );

                    // 8. Balance Allocation
                    objDBAccess.AddParameterToCommand(
                        "@balanceAllocation",
                        DBFactoryParameterType.Int,
                        (object)item.balanceAllocation ?? DBNull.Value
                    );

                    // 9. Last Month Department
                    objDBAccess.AddParameterToCommand(
                        "@AllotmentDepartmentLastMonth",
                        DBFactoryParameterType.Int,
                        (object)item.AllotmentDepartmentLastMonth ?? DBNull.Value
                    );

                    // 10. Last Month CA
                    objDBAccess.AddParameterToCommand(
                        "@AllotmentCALastMonth",
                        DBFactoryParameterType.Int,
                        (object)item.AllotmentCALastMonth ?? DBNull.Value
                    );

                    // 11. Current Month Department
                    objDBAccess.AddParameterToCommand(
                        "@AllotmentDepartmentInMonth",
                        DBFactoryParameterType.Int,
                        (object)item.AllotmentDepartmentInMonth ?? DBNull.Value
                    );

                    // 12. Current Month CA
                    objDBAccess.AddParameterToCommand(
                        "@AllotmentCAInMonth",
                        DBFactoryParameterType.Int,
                        (object)item.AllotmentCAInMonth ?? DBNull.Value
                    );

                    // 13. Created By
                    objDBAccess.AddParameterToCommand(
                        "@CreatedBy",
                        DBFactoryParameterType.Int,
                        _LoggedInUser.UserId
                    );

                    // 14. User Role Department Mapping
                    objDBAccess.AddParameterToCommand(
                        "@UserRoleDeptMappingId",
                        DBFactoryParameterType.Int,
                        _LoggedInUser.UserRoleDeptMappingId
                    );

                    // 15. Role ID
                    objDBAccess.AddParameterToCommand(
                        "@RoleId",
                        DBFactoryParameterType.Int,
                        _LoggedInUser.RoleId
                    );

                    // Execute Stored Procedure
                    objDBAccess.ExecuteCommand();

                    Result++;
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(
                    new ErrorLogEntity()
                    {
                        PageName = "AuditorNewModel",
                        FunctionName = "SaveAuditResult"
                    },
                    ex
                );

                Result = 0;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public DataTable GetAuditAllDetails(int officeId,  int month, int DistrictId, bool isDevelopment = false, int resionId = 0, int DivisionId = 0)
        {
            DataTable dt = new DataTable();
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("USP_GetAuditMprCommitteeBackYear", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@OfficeId", DBFactoryParameterType.Int, officeId);
                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, month);
                //local
                //if (isDevelopment) { objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.Int, (_LoggedInUser.RoleId == 1 || _LoggedInUser.RoleId == 10235) ? pldbId : _LoggedInUser.PLDBId); }
                //else { objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.Int, (_LoggedInUser.RoleId == 10235) ? pldbId : _LoggedInUser.PLDBId); }
                //objDBAccess.AddParameterToCommand("@KPIFor", DBFactoryParameterType.NVarchar, kpiFor);
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



    }
}