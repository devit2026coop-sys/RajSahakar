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

namespace Cooperative.Models
{
    public class AuditorModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;
        [Key]
        public int TemplateId { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Template Heading English - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Template Heading English - Max 100 characters allowed")]
        public string TemplateHeadingEN { get; set; }
        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Template Heading Hindi - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Template Heading Hindi - Max 100 characters allowed")]
        public string TemplateHeadingHI { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Template Body English - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string TemplateBodyEN { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Template Body Hindi - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string TemplateBodyHI { get; set; }

        public string TemplateType { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }

        public AuditorModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public AuditorModel()
        {
            objDBAccess = new DBFactoryMSSQL();

        }

        public int AddSmsFormat()
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertSmsTemplate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TemplateId", DBFactoryParameterType.BigInt, TemplateId);
                objDBAccess.AddParameterToCommand("@TemplateHeadingEN", DBFactoryParameterType.NVarchar, TemplateHeadingEN);
                objDBAccess.AddParameterToCommand("@TemplateHeadingHI", DBFactoryParameterType.NVarchar, TemplateHeadingHI);
                objDBAccess.AddParameterToCommand("@TemplateBodyEN", DBFactoryParameterType.NVarchar, TemplateBodyEN);
                objDBAccess.AddParameterToCommand("@TemplateBodyHI", DBFactoryParameterType.NVarchar, TemplateBodyHI);
                objDBAccess.AddParameterToCommand("@TemplateType", DBFactoryParameterType.NVarchar, "S");
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, IsActive);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, CreatedIP);
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, UpdatedIP);

                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SmsformatModel", FunctionName = "AddSmsFormat()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }




        public DataTable GetAllAuditor(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyAuditorAssigments", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Auditor", FunctionName = "GetAllAuditor(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


        public DataTable GetAuditUplodedDocument(string Id)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AuditDocumentList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, int.Parse(Id));
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Auditor", FunctionName = "AuditDocumentList(string Id = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }


        public DataSet GetSocietyAuditDataByFinYearId(string SocietyIdENC, string FinYearId)
        {
            dt.Clear();

            DataSet dset = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyAuditDataByFinYearId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdENC", DBFactoryParameterType.NVarchar, SocietyIdENC);
                objDBAccess.AddParameterToCommand("@FinYearId", DBFactoryParameterType.Int, int.Parse(FinYearId));
                dset = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Auditor", FunctionName = "GetSocietyAuditDataByFinYearId(string SocietyIdENC, string FinYearId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dset;
        }

        public DataTable CentralCategoryListForAudit(int LevelID)
        {
            dt.Clear();

            DataSet dset = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CentralCategoryListForAudit", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@LevelID", DBFactoryParameterType.Int, LevelID);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Auditor", FunctionName = "CentralCategoryListForAudit()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }

        public DataTable GetSocietyAuditDataByRoleFinYearId(ref DataPagingModel Details, int FinYearId)
        {
            dt.Clear();
            string whereCase = "";
            DataTable dset = new DataTable();
            try
            {

                string actId = Details.SearchParameter.Where(x => x.Key == "ActId").Select(x => x.Value).FirstOrDefault();

                whereCase += " and Societies.ActId in (select item from dbo.SplitString('" + actId + "',','))";

                string searchText = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    searchText = searchText.TrimEnd().TrimStart();
                    //whereCase += " and (Act.ActNameEn like N'%" + searchText + "%' or Act.ActNameHi like N'%" + searchText + "%' or Societies.[RegistrationNo] like N'%" + searchText + "%' or SocietyName like N'%" + searchText + "%' or ST.TypeameEng  like N'%" + searchText + "%' or ST.TypeNameHin  like N'%" + searchText + "%' or ClassNameEn  like N'%" + searchText + "%' or ClassNameHi  like N'%" + searchText + "%' or SubClassNameEn  like N'%" + searchText + "%' or SubClassNameHi  like N'%" + searchText + "%' or SubClassNameHi  like N'%" + searchText + "%' or SubmitStatus  like N'%" + searchText + "%' or SSOID  like N'%" + searchText + "%' or convert(nvarchar,[RegistrationDate], 103)  like N'%" + searchText + "%')";
                }



                if (Details.SearchParameter.Where(x => x.Key == "IsCCS" && x.Value == "1").Any())
                {

                    whereCase += " and Societies.ClassId= 9 And Societies.subclassid=20 ";
                }

                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];




                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("SocietyListForDepartmentAudit", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                    objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                    objDBAccess.AddParameterToCommand("@FinYearId", DBFactoryParameterType.Int, FinYearId);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, _LoggedInUser.DistrictId);
                    objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, _LoggedInUser.DivisionId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, whereCase);
                    objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, searchText);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                cmd = objDBAccess.CreateCommand("SocietyListForDepartmentAudit", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@FinYearId", DBFactoryParameterType.Int, FinYearId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, _LoggedInUser.DistrictId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, _LoggedInUser.DivisionId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, whereCase);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, searchText);
                dset = objDBAccess.GetTable();

                // objDBAccess.OpenConnection();
                // cmd = objDBAccess.CreateCommand("SocietyListForDepartmentAudit", DBFactoryCommandType.PROCEDURE_COMMAND);
                //// objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, UserId);
                // objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, LevelId);
                // objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, RoleId);              
                // objDBAccess.AddParameterToCommand("@FinYearId", DBFactoryParameterType.Int, FinYearId);
                // objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                // objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                // objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, "  ");
                // dset = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Auditor", FunctionName = "GetSocietyAuditDataByRoleFinYearId(int RoleId, long UserId, int LevelId, int FinYearId,int DistrictId, int DivisionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dset;
        }




        public int AddAuditorRequest(AuditorRequestEntity obj)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertAuditorRequestData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@AuditorId", DBFactoryParameterType.BigInt, obj.AuditorId);
                objDBAccess.AddParameterToCommand("@FirmName", DBFactoryParameterType.NVarchar, obj.FirmName);
                objDBAccess.AddParameterToCommand("@IsFirm", DBFactoryParameterType.Int, obj.IsFirm);
                objDBAccess.AddParameterToCommand("@FirmTypeId", DBFactoryParameterType.Int, obj.FirmType);
                objDBAccess.AddParameterToCommand("@FirmAddress", DBFactoryParameterType.NVarchar, obj.FirmAddress);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, obj.DistrictId);
                objDBAccess.AddParameterToCommand("@StateId", DBFactoryParameterType.Int, obj.StateId);
                objDBAccess.AddParameterToCommand("@PINCode", DBFactoryParameterType.NVarchar, obj.FirmPinCode);
                objDBAccess.AddParameterToCommand("@ICFAIRegistrationNo", DBFactoryParameterType.NVarchar, obj.ICFAIRegistrationNo);
                objDBAccess.AddParameterToCommand("@RegistrationDate", DBFactoryParameterType.Date, obj.RegistrationDate);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, obj.MobileNo);
                objDBAccess.AddParameterToCommand("@PhoneNo", DBFactoryParameterType.NVarchar, obj.PhoneNo);
                objDBAccess.AddParameterToCommand("@NoOfPartners", DBFactoryParameterType.Int, obj.NoOfPartners);
                objDBAccess.AddParameterToCommand("@BranchAddress", DBFactoryParameterType.NVarchar, obj.BranchAddress);
                objDBAccess.AddParameterToCommand("@RBIPanelUniqueCode", DBFactoryParameterType.NVarchar, obj.RBIPanelUniqueCode);
                objDBAccess.AddParameterToCommand("@EnrollmentNo", DBFactoryParameterType.NVarchar, obj.EnrollmentNo);
                objDBAccess.AddParameterToCommand("@BranchPinCode", DBFactoryParameterType.NVarchar, obj.BranchPinCode);
                objDBAccess.AddParameterToCommand("@SSO_ID", DBFactoryParameterType.NVarchar, obj.SSO_ID);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, Controllers.COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, Controllers.COMMON_FUNCTION.GetIpAddress());
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

        public DataTable GetAuditorList(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("GetAuditorList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Auditor", FunctionName = "GetAllAuditor(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetHouseAuditReport(string Year, string DistrictId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetHouseAuditList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FYearId", DBFactoryParameterType.Int, Year);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Auditor", FunctionName = "GetHouseAuditReport(string Year, string DistrictId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }
        public DataTable GetNotHouseAuditReport(string Year, string DistrictId, string SaveStatus)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetNotHouseAuditList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FYearId", DBFactoryParameterType.Int, Year);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@SaveStatus", DBFactoryParameterType.Int, SaveStatus);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Auditor", FunctionName = "GetNotHouseAuditReport(string Year, string DistrictId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }

        public DataTable GetAuditYearList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AuditYearList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Auditor", FunctionName = "GetAuditYearList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }
        public DataTable GetSocietyListAssignedToAuditor(string SSO_ID, string FYearId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyListAssignedToAuditor", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSO_ID", DBFactoryParameterType.NVarchar, SSO_ID);
                objDBAccess.AddParameterToCommand("@FYearId", DBFactoryParameterType.Int, FYearId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Auditor", FunctionName = "GetSocietyListAssignedToAuditor(string SSO_ID)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }


        public DataTable GetSocietyListAssignedToAuditorReport()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyListAssignedToAuditorReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Auditor", FunctionName = "GetSocietyListAssignedToAuditorReport(string SSO_ID)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }


        public DataTable SocietyListAssignedToAuditorReport(ref DataPagingModel Details, int? CountId, string FIN_YEAR_NAME, string DistrictId, bool isExport = false, int Month = 0, string IsEmpanelledCA = "", string AuditBy = "", int SocietyTypeId = 0)
        {

            DataTable resultDt = new DataTable();
            resultDt.Clear();
            try
            {
                string whereCase = " ";
                string searchstr = " ";
                if (Details.SearchParameter.Count > 0)
                {

                    //var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                    var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                    string whereDate = @" and   convert(date,created_date,111) ";
                    if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                    {
                        string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                        string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                        whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                    }
                    else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                    {
                        string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                        string tDate = fDate;
                        whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                    }
                    else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                    {
                        string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                        string fDate = tDate;
                        whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                    }


                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate" && x.Key != "serachText"))
                    {
                        if (x.Key == "SearchText")
                        {
                            searchstr = x.Value;
                        }
                        else
                        {
                            string key = x.Key;
                            whereCase += " and " + key + "='" + x.Value + "'";
                        }
                    }

                    if (Details.SearchParameter.Where(x => x.Key == "serachText").Any())
                    {
                        string Value = Details.SearchParameter.Where(x => x.Key == "serachText").Select(x => x.Value).FirstOrDefault();
                        whereCase += "and (FIN_YEAR_NAME like '%" + Value + "%' or DISTRICT_ENG like '%" + Value + "%' or DISTRICT_MANGAL  like '%" + Value + "%' or SocietyName  like '%" + Value + "%' or ActNameEn  like '%" + Value + "%' or  ActNameHi like '%" + Value + "%')";
                    }


                }
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();


                //if (Details.PageNumber == 100)
                //{
                //    cmd = objDBAccess.CreateCommand("SocietyListAssignedToAuditorReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                //    objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                //    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                //    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                //    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                //    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                //    objDBAccess.AddParameterToCommand("@CountId", DBFactoryParameterType.Int, CountId);
                //    objDBAccess.AddParameterToCommand("@FIN_YEAR_NAME", DBFactoryParameterType.NVarchar, FIN_YEAR_NAME);
                //    objDBAccess.AddParameterToCommand("@District_Id", DBFactoryParameterType.NVarchar, DistrictId == "undefined" || DistrictId == null ? "" : DistrictId);

                //    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                //}

                //if (Details.IsAllRecords)
                //{
                //    Details.PageSize = Details.TotalRecords;
                //}


                cmd = objDBAccess.CreateCommand("SocietyListAssignedToAuditorReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                //objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                //objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, isExport == true ? 5000000 : Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, isExport == true ? 1 : Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                objDBAccess.AddParameterToCommand("@searchstr", DBFactoryParameterType.NVarchar, searchstr);

                objDBAccess.AddParameterToCommand("@CountId", DBFactoryParameterType.Int, CountId);
                objDBAccess.AddParameterToCommand("@IsExport", DBFactoryParameterType.Bit, isExport);

                objDBAccess.AddParameterToCommand("@FIN_YEAR_NAME", DBFactoryParameterType.NVarchar, FIN_YEAR_NAME);
                objDBAccess.AddParameterToCommand("@District_Id", DBFactoryParameterType.NVarchar, DistrictId == "undefined" || DistrictId == null ? "" : DistrictId);

                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, Month); //20-6-2025
                objDBAccess.AddParameterToCommand("@IsEmpanelledCA", DBFactoryParameterType.NVarchar, IsEmpanelledCA);//20-6-2025
                objDBAccess.AddParameterToCommand("@AuditBy", DBFactoryParameterType.NVarchar, AuditBy);
                objDBAccess.AddParameterToCommand("@SocietyType", DBFactoryParameterType.Int, SocietyTypeId);

                resultDt = objDBAccess.GetTable();
                //if (resultDt != null)
                //    Details.TotalRecords = Convert.ToInt32(resultDt.Rows[0]["TotalRecords"]);
                //else Details.TotalRecords = 0;
                Details.TotalRecords = (resultDt != null && resultDt.Rows.Count > 0) ? Convert.ToInt32(resultDt.Rows[0]["TotalRecords"]) : 0;

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable SocietyListAssignedToAuditorReport(ref DataPagingModel Details)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable SocietyListAssignedToAuditorById(string SocietyIdEnc = "")
        {

            DataTable resultDt = new DataTable();
            resultDt.Clear();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();

                cmd = objDBAccess.CreateCommand("p_SocietyListAssignedToAuditorById", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.VARCHAR, SocietyIdEnc);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable SocietyListAssignedToAuditorReport(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }
        public DataTable SocietyListAssignedToAuditorReport_OLD(ref DataPagingModel Details, int? CountId, string FIN_YEAR_NAME, string DistrictId)
        {

            DataTable resultDt = new DataTable();
            resultDt.Clear();
            try
            {
                string whereCase = " ";

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();


                    var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                    var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                    string whereDate = @" and   convert(date,created_date,111) ";
                    if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                    {
                        string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                        string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                        whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                    }
                    else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                    {
                        string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                        string tDate = fDate;
                        whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                    }
                    else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                    {
                        string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                        string fDate = tDate;
                        whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                    }

                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate" && x.Key != "serachText"))
                    {
                        string key = x.Key;
                        whereCase += " and " + key + "='" + x.Value + "'";
                    }

                    if (Details.SearchParameter.Where(x => x.Key == "serachText").Any())
                    {
                        string Value = Details.SearchParameter.Where(x => x.Key == "serachText").Select(x => x.Value).FirstOrDefault();
                        whereCase += "and (FIN_YEAR_NAME like '%" + Value + "%' or DISTRICT_ENG like '%" + Value + "%' or DISTRICT_MANGAL  like '%" + Value + "%' or SocietyName  like '%" + Value + "%' or ActNameEn  like '%" + Value + "%' or  ActNameHi like '%" + Value + "%')";
                    }


                }
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 100)
                {
                    cmd = objDBAccess.CreateCommand("SocietyListAssignedToAuditorReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    objDBAccess.AddParameterToCommand("@CountId", DBFactoryParameterType.Int, CountId);
                    objDBAccess.AddParameterToCommand("@FIN_YEAR_NAME", DBFactoryParameterType.NVarchar, FIN_YEAR_NAME);
                    objDBAccess.AddParameterToCommand("@District_Id", DBFactoryParameterType.NVarchar, DistrictId == "undefined" || DistrictId == null ? "" : DistrictId);

                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("SocietyListAssignedToAuditorReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                objDBAccess.AddParameterToCommand("@CountId", DBFactoryParameterType.Int, CountId);
                objDBAccess.AddParameterToCommand("@FIN_YEAR_NAME", DBFactoryParameterType.NVarchar, FIN_YEAR_NAME);
                objDBAccess.AddParameterToCommand("@District_Id", DBFactoryParameterType.NVarchar, DistrictId == "undefined" || DistrictId == null ? "" : DistrictId);

                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable SocietyListAssignedToAuditorReport(ref DataPagingModel Details)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }


        public DataTable GetRegistartionAmountReport(ref DataPagingModel Details, int MapId)
        {

            DataTable resultDt = new DataTable();
            try
            {
                string whereCase = " ";

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                    var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                    string whereDate = @" and   convert(date,created_date,111) ";
                    if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                    {
                        string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                        string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                        whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                    }
                    else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                    {
                        string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                        string tDate = fDate;
                        whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                    }
                    else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                    {
                        string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                        string fDate = tDate;
                        whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                    }

                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate" && x.Key != "serachText"))
                    {
                        string key = x.Key;
                        whereCase += " and " + key + "='" + x.Value + "'";
                    }




                }
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                //if (Details.PageNumber == 1)
                //{
                //    cmd = objDBAccess.CreateCommand("GetRegistartionAmountReport", DBFactoryCommandType.PROCEDURE_COMMAND);                
                //    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                //    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                //    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                //    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                //    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                //}

                //if (Details.IsAllRecords)
                //{
                //    Details.PageSize = Details.TotalRecords;
                //}

                cmd = objDBAccess.CreateCommand("GetRegistartionAmountReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MapId", DBFactoryParameterType.Int, MapId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable GetRigistrationAmountReport(ref DataPagingModel Details)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }

        public DataTable GetRegistrationAmountSocietyDetails(ref DataPagingModel Details, int MapId)
        {

            DataTable resultDt = new DataTable();
            try
            {
                string whereCase = " ";

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();

                    var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                    var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                    string whereDate = @" and   convert(date,created_date,111) ";
                    if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                    {
                        string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                        string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                        whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                    }
                    else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                    {
                        string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                        string tDate = fDate;
                        whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                    }
                    else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                    {
                        string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                        string fDate = tDate;
                        whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                    }

                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate" && x.Key != "serachText"))
                    {
                        string key = x.Key;
                        whereCase += " and " + key + "='" + x.Value + "'";
                    }




                }
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                //if (Details.PageNumber == 1)
                //{
                //    cmd = objDBAccess.CreateCommand("GetRegistartionAmountReport", DBFactoryCommandType.PROCEDURE_COMMAND);                
                //    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                //    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                //    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                //    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                //    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                //}

                //if (Details.IsAllRecords)
                //{
                //    Details.PageSize = Details.TotalRecords;
                //}

                cmd = objDBAccess.CreateCommand("GetRegistrationAmountSocietyDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MapId", DBFactoryParameterType.Int, MapId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable GetRigistrationAmountReport(ref DataPagingModel Details)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }



        public DataTable GetSocietyLisSocietyAuditDocumentUploadReport()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyLisSocietyAuditDocumentUploadReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Auditor", FunctionName = "GetSocietyLisSocietyAuditDocumentUploadReport(string SSO_ID)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }

        public DataTable SocietyAuditDocumentUploadReport(ref DataPagingModel Details)
        {

            DataTable resultDt = new DataTable();
            try
            {
                string whereCase = " ";

                if (Details.SearchParameter.Count > 0)
                {

                    var type = Details.SearchParameter.Where(x => x.Key == "FilterType").Select(x => x.Value).FirstOrDefault();


                    var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                    var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();
                    string whereDate = @" and   convert(date,created_date,111) ";
                    if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                    {
                        string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                        string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                        whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                    }
                    else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                    {
                        string fDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                        string tDate = fDate;
                        whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                    }
                    else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                    {
                        string tDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                        string fDate = tDate;
                        whereCase += whereDate + " between '" + fDate + "' and '" + tDate + "'";
                    }

                    foreach (var x in Details.SearchParameter.Where(x => x.Key != "FromDate" && x.Key != "ToDate" && x.Key != "serachText"))
                    {
                        string key = x.Key;
                        whereCase += " and " + key + "=" + x.Value + "";
                    }

                    if (Details.SearchParameter.Where(x => x.Key == "serachText").Any())
                    {
                        string Value = Details.SearchParameter.Where(x => x.Key == "serachText").Select(x => x.Value).FirstOrDefault();
                        whereCase += "and (FIN_YEAR_NAME like '%" + Value + "%' or SocietyName  like '%" + Value + "%' or ActNameEn  like '%" + Value + "%' or  ActNameHi like '%" + Value + "%')";
                    }


                }
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("SocietyAuditDocumentUploadReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("SocietyAuditDocumentUploadReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable SocietyAuditDocumentUploadReport(ref DataPagingModel Details)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;

        }






        public int SaveAuditStartEndDate(DateTime AuditStartDate, DateTime AuditEndDate, int PKID, DateTime Feedate, SocietyAuditorAssigmentEntity _Model)
        {

            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SaveAuditStartEndDateById", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PKID", DBFactoryParameterType.Int, PKID);
                if (AuditStartDate != null && !AuditStartDate.ToString("dd/MM/yyyy").Contains("0001"))
                {
                    objDBAccess.AddParameterToCommand("@AuditStartDate", DBFactoryParameterType.DateTime, AuditStartDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                }
                if (AuditEndDate != null && !AuditEndDate.ToString("dd/MM/yyyy").Contains("0001"))
                {
                    objDBAccess.AddParameterToCommand("@AuditEndDate", DBFactoryParameterType.DateTime, AuditEndDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                }
                if (Feedate != null && !Feedate.ToString("dd/MM/yyyy").Contains("0001"))
                {
                    objDBAccess.AddParameterToCommand("@Feedate", DBFactoryParameterType.DateTime, Feedate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                }

                objDBAccess.AddParameterToCommand("@IsDeterminedFormat", DBFactoryParameterType.Bit, _Model.IsDeterminedFormat);
                objDBAccess.AddParameterToCommand("@IsApprovedByGeneralAssembly", DBFactoryParameterType.Bit, _Model.IsApprovedByGeneralAssembly);
                objDBAccess.AddParameterToCommand("@IsDesiredDocumentAttached", DBFactoryParameterType.Bit, _Model.IsDesiredDocumentAttached);
                objDBAccess.AddParameterToCommand("@IsAllAuditReportDocumentsAttached", DBFactoryParameterType.Bit, _Model.IsAllAuditReportDocumentsAttached);
                objDBAccess.AddParameterToCommand("@IsComplianceReportApprovedByGeneralAssembly", DBFactoryParameterType.Bit, _Model.IsComplianceReportApprovedByGeneralAssembly);
                objDBAccess.AddParameterToCommand("@ClassificationId", DBFactoryParameterType.Int, _Model.ClassificationId);
                objDBAccess.AddParameterToCommand("@TotalActivMembers", DBFactoryParameterType.NVarchar, _Model.TotalActivMembers);
                objDBAccess.AddParameterToCommand("@ACategory", DBFactoryParameterType.NVarchar, _Model.ACategory);
                objDBAccess.AddParameterToCommand("@BCategory", DBFactoryParameterType.NVarchar, _Model.BCategory);
                objDBAccess.AddParameterToCommand("@CCategory", DBFactoryParameterType.NVarchar, _Model.CCategory);
                objDBAccess.AddParameterToCommand("@CeoName", DBFactoryParameterType.NVarchar, _Model.CeoName);
                objDBAccess.AddParameterToCommand("@IsIrregularity", DBFactoryParameterType.Bit, _Model.IsIrregularity);
                objDBAccess.AddParameterToCommand("@IrregularityType", DBFactoryParameterType.Int, _Model.IrregularityType);//b
                objDBAccess.AddParameterToCommand("@DescriptionofIrregularity", DBFactoryParameterType.NVarchar, _Model.DescriptionofIrregularity);
                objDBAccess.AddParameterToCommand("@ResponsibleOfficerName", DBFactoryParameterType.NVarchar, _Model.ResponsibleOfficerName);
                objDBAccess.AddParameterToCommand("@IrregularityAmount", DBFactoryParameterType.NVarchar, _Model.IrregularityAmount);//b
                objDBAccess.AddParameterToCommand("@ActionDescription", DBFactoryParameterType.NVarchar, _Model.ActionDescription);
                objDBAccess.AddParameterToCommand("@IrregularityOfficerName", DBFactoryParameterType.NVarchar, _Model.IrregularityOfficerName);
                objDBAccess.AddParameterToCommand("@SectionId", DBFactoryParameterType.Int, _Model.SectionId);
                objDBAccess.AddParameterToCommand("@IsProfit", DBFactoryParameterType.Bit, _Model.IsProfit);
                objDBAccess.AddParameterToCommand("@TotalMembers", DBFactoryParameterType.NVarchar, _Model.TotalMembers);
                objDBAccess.AddParameterToCommand("@AccumulateProfitLoss", DBFactoryParameterType.NVarchar, _Model.AccumulateProfitLoss);
                objDBAccess.AddParameterToCommand("@Grossprofitor", DBFactoryParameterType.NVarchar, _Model.Grossprofitor);
                objDBAccess.AddParameterToCommand("@TotalAmonut", DBFactoryParameterType.Int, _Model.TotalAmonut);
                objDBAccess.AddParameterToCommand("@Membersshareamount", DBFactoryParameterType.NVarchar, _Model.Membersshareamount);
                objDBAccess.AddParameterToCommand("@StateGovtAmount", DBFactoryParameterType.NVarchar, _Model.StateGovtAmount);
                objDBAccess.AddParameterToCommand("@IsActionisrequired", DBFactoryParameterType.Bit, _Model.IsActionisrequired);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MenuModel", FunctionName = "SaveAuditStartEndDate(DateTime AuditStartDate, DateTime AuditEndDate,int PKID)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataTable GetChatFAQ(string SearchText, string LN = "en-us")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetChatFAQ", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@LN", DBFactoryParameterType.NVarchar, LN);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, SearchText);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Auditor", FunctionName = "GetChatFAQ()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }


        public DataTable GetCountSocietyAssignedToAuditor_OLD(string FIN_YEAR_NAME, string DistrictId, string WhareClause)
        {
            DataTable resultDt = new DataTable();
            try
            {
                if (FIN_YEAR_NAME == null)
                {
                    FIN_YEAR_NAME = "";
                }

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("GetCountSocietyAssignedToAuditor", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@FIN_YEAR_NAME", DBFactoryParameterType.NVarchar, FIN_YEAR_NAME == "undefined" ? "" : FIN_YEAR_NAME);
                objDBAccess.AddParameterToCommand("@District_Id", DBFactoryParameterType.NVarchar, DistrictId == "undefined" || DistrictId == null ? "" : DistrictId);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhareClause == "undefined" || WhareClause == null ? "" : WhareClause);

                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable SocietyAuditDocumentUploadReport(ref DataPagingModel Details)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;
        }
        [Obsolete]//Pradeep 23-05-2024
        public DataTable GetCountSocietyAssignedToAuditor(string FIN_YEAR_NAME, string DistrictId, string WhareClause)
        {
            DataTable resultDt = new DataTable();
            try
            {
                if (FIN_YEAR_NAME == null)
                {
                    FIN_YEAR_NAME = "";
                }

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("GetCountSocietyAssignedToAuditor", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@FIN_YEAR_NAME", DBFactoryParameterType.NVarchar, FIN_YEAR_NAME == "undefined" ? "" : FIN_YEAR_NAME);
                objDBAccess.AddParameterToCommand("@District_Id", DBFactoryParameterType.NVarchar, DistrictId == "undefined" || DistrictId == null ? "" : DistrictId);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhareClause == "undefined" || WhareClause == null ? "" : WhareClause);

                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable SocietyAuditDocumentUploadReport(ref DataPagingModel Details)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;
        }

        public DataTable GetCountSocietyAssignedToAuditor_v1(string FIN_YEAR_NAME, string DistrictId, string DivisionId, int Month = 0, string IsEmpanelledCA = "")
        {
            DataTable resultDt = new DataTable();
            try
            {
                if (FIN_YEAR_NAME == null)
                {
                    FIN_YEAR_NAME = "";
                }

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("GetCountSocietyAssignedToAuditor_v1", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@FIN_YEAR_NAME", DBFactoryParameterType.NVarchar, FIN_YEAR_NAME == "undefined" ? "" : FIN_YEAR_NAME);
                objDBAccess.AddParameterToCommand("@District_Id", DBFactoryParameterType.NVarchar, DistrictId == "undefined" || DistrictId == null ? "" : DistrictId);
                objDBAccess.AddParameterToCommand("@Division_Id", DBFactoryParameterType.NVarchar, DivisionId == "undefined" || DivisionId == null ? "" : DivisionId);
                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, Month);
                objDBAccess.AddParameterToCommand("@IsEmpanelledCA", DBFactoryParameterType.NVarchar, IsEmpanelledCA);

                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable GetCountSocietyAssignedToAuditor_v1(string FIN_YEAR_NAME, string DistrictId, string WhareClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;
        }

        public string SaveAuditOrder(AuditOrderEntity _Entity)
        {
            string Result = "ERROR";

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertAuditOrder", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, _Entity.LevelId);
                objDBAccess.AddParameterToCommand("@PlaceId", DBFactoryParameterType.Int, _Entity.PlaceId);
                objDBAccess.AddParameterToCommand("@FileName", DBFactoryParameterType.NVarchar, _Entity.FileName);
                objDBAccess.AddParameterToCommand("@OrderNo", DBFactoryParameterType.NVarchar, _Entity.OrderNo);
                objDBAccess.AddParameterToCommand("@SocietyCount", DBFactoryParameterType.Int, _Entity.SocietyCount);
                objDBAccess.AddParameterToCommand("@InActiveFlag", DBFactoryParameterType.Int, _Entity.InActiveFlag);
                objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, _Entity.SSOId);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _Entity.CreatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, Controllers.COMMON_FUNCTION.GetIpAddress());

                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());

            }
            catch (Exception ex)
            {

                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "SaveBankDetailmodel(SocietyRegistrationEntity _Entity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();

            }

            return Result;
        }
        public string InActiveAuditOrder(AuditOrderEntity _Entity)
        {
            string Result = "ERROR";

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertAuditOrder", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, _Entity.LevelId);
                objDBAccess.AddParameterToCommand("@PlaceId", DBFactoryParameterType.Int, _Entity.PlaceId);
                objDBAccess.AddParameterToCommand("@FileName", DBFactoryParameterType.NVarchar, _Entity.FileName);

                objDBAccess.AddParameterToCommand("@SocietyCount", DBFactoryParameterType.Int, _Entity.SocietyCount);
                objDBAccess.AddParameterToCommand("@SocietyCount", DBFactoryParameterType.Int, _Entity.SocietyCount);

                objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, _Entity.SSOId);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _Entity.CreatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, Controllers.COMMON_FUNCTION.GetIpAddress());

                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());

            }
            catch (Exception ex)
            {

                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "SaveBankDetailmodel(SocietyRegistrationEntity _Entity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();

            }

            return Result;
        }

        #region Order_Report_UMESH
        public DataTable GetOrderauditorListReport(string OrderNo, int Flag = 0)
        {
            DataTable resultDt = new DataTable();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("GetAuditorderlistReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                //objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _LoggedInUser.DistrictId);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@OrderNo", DBFactoryParameterType.NVarchar, OrderNo);
                objDBAccess.AddParameterToCommand("@Flag", DBFactoryParameterType.Int, Flag);
                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable SocietyAuditDocumentUploadReport(ref DataPagingModel Details)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;
        }

        public DataTable GetOrderauditorListReport_V1(ref DataPagingModel Details, string OrderNo, string fIYear, int Flag = 0)
        {
            dt.Clear();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();

                string ListType = "A";
                if (Details.SearchParameter.Where(x => x.Key == "ListType").Any())
                {
                    ListType = Details.SearchParameter.Where(x => x.Key == "ListType").Select(x => x.Value).FirstOrDefault();
                }

                string SearchText = "";
                if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                {
                    SearchText = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault();
                }

                string RequestType = "G";
                if (Details.SearchParameter.Where(x => x.Key == "RequestType").Any())
                {
                    RequestType = Details.SearchParameter.Where(x => x.Key == "RequestType").Select(x => x.Value).FirstOrDefault();
                }

                if (fIYear == null || fIYear == "0")
                {
                    fIYear = "and 1=1";

                }
                else
                {
                    fIYear = " and Fi.FIN_YEAR_MASTR_ID =" + fIYear;
                }

                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetAuditorderlistReport_v1", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                    objDBAccess.AddParameterToCommand("@Flag", DBFactoryParameterType.Int, Flag);
                    objDBAccess.AddParameterToCommand("@OrderNo", DBFactoryParameterType.NVarchar, OrderNo);
                    objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.NVarchar, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.NVarchar, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, true);
                    objDBAccess.AddParameterToCommand("@ListType", DBFactoryParameterType.NVarchar, ListType);
                    objDBAccess.AddParameterToCommand("@RequestType", DBFactoryParameterType.NVarchar, RequestType);
                    objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, SearchText);
                    objDBAccess.AddParameterToCommand("@fIYear", DBFactoryParameterType.NVarchar, fIYear);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }
                cmd = objDBAccess.CreateCommand("GetAuditorderlistReport_v1", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("@Flag", DBFactoryParameterType.Int, Flag);
                objDBAccess.AddParameterToCommand("@OrderNo", DBFactoryParameterType.NVarchar, OrderNo);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.NVarchar, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.NVarchar, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Bit, false);
                objDBAccess.AddParameterToCommand("@ListType", DBFactoryParameterType.VARCHAR, ListType);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, SearchText);
                objDBAccess.AddParameterToCommand("@fIYear", DBFactoryParameterType.NVarchar, fIYear);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "GetOrderauditorListReport_V1()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }


            return dt;

        }


        public string DeleteWithresionSave(string OrderNo, string DeleteResions)
        {
            string Result = "ERROR";
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ResionfordeleteDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@OrderNo", DBFactoryParameterType.NVarchar, OrderNo);
                objDBAccess.AddParameterToCommand("@DeleteResions", DBFactoryParameterType.NVarchar, DeleteResions);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);

                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());

            }
            catch (Exception ex)
            {

                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.UserRegistrationModel", FunctionName = "SaveBankDetailmodel(SocietyRegistrationEntity _Entity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();

            }

            return Result;
        }


        public string ReorderSocietiesDetails(string OrderNo, string DeleteResions, string id)
        {
            string Result = "ERROR";
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ReorderSocietiesDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@OrderNo", DBFactoryParameterType.NVarchar, OrderNo);
                objDBAccess.AddParameterToCommand("@DeleteResions", DBFactoryParameterType.NVarchar, DeleteResions);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.NVarchar, id);

                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());

            }
            catch (Exception ex)
            {

                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.AuditorModel", FunctionName = "ReorderSocietiesDetails(string OrderNo, string DeleteResions, string id)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();

            }

            return Result;
        }


        public DataTable GetCountSocietyauditprogesslist(string FIN_YEAR_NAME, string DistrictId, string WhareClause)
        {
            DataTable resultDt = new DataTable();
            try
            {
                if (FIN_YEAR_NAME == null)
                {
                    FIN_YEAR_NAME = "";
                }

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("GetCountSocietyAuditprogrss", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@FIN_YEAR_NAME", DBFactoryParameterType.NVarchar, FIN_YEAR_NAME == "undefined" ? "" : FIN_YEAR_NAME);
                objDBAccess.AddParameterToCommand("@District_Id", DBFactoryParameterType.NVarchar, DistrictId == "undefined" || DistrictId == null ? "" : DistrictId);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhareClause == "undefined" || WhareClause == null ? "" : WhareClause);

                resultDt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable SocietyAuditDocumentUploadReport(ref DataPagingModel Details)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return resultDt;
        }
        #endregion

        public string InsertAuditEmpanelled(AuditEmpanelledModel ApplicationModel)
        {
            string Result = "-1";
            int AuditEmpanelledId1 = 0;
            DBFactoryMSSQL conn = new DBFactoryMSSQL();
            SqlConnection sqlConnection = conn.GetConnection;
            SqlTransaction transaction;
            sqlConnection.Open();
            transaction = sqlConnection.BeginTransaction();

            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                string RBIRegistrationDate = "";
                if (!string.IsNullOrWhiteSpace(ApplicationModel.RBIRegistrationDate))
                {
                    ApplicationModel.RBIRegistrationDate = ApplicationModel.RBIRegistrationDate.Replace("-", "/");
                    RBIRegistrationDate = ApplicationModel.RBIRegistrationDate.Split('/')[2] + "/" + ApplicationModel.RBIRegistrationDate.Split('/')[1] + "/" + ApplicationModel.RBIRegistrationDate.Split('/')[0];
                }
                string NbardRegistrationDate = "";
                if (!string.IsNullOrWhiteSpace(ApplicationModel.NbardRegistrationDate))
                {
                    ApplicationModel.NbardRegistrationDate = ApplicationModel.NbardRegistrationDate.Replace("-", "/");
                    NbardRegistrationDate = ApplicationModel.NbardRegistrationDate.Split('/')[2] + "/" + ApplicationModel.NbardRegistrationDate.Split('/')[1] + "/" + ApplicationModel.NbardRegistrationDate.Split('/')[0];
                }
                string RegistrationDate = "";
                if (!string.IsNullOrWhiteSpace(ApplicationModel.RegistrationDate))
                {
                    ApplicationModel.RegistrationDate = ApplicationModel.RegistrationDate.Replace("-", "/");
                    RegistrationDate = ApplicationModel.RegistrationDate.Split('/')[2] + "/" + ApplicationModel.RegistrationDate.Split('/')[1] + "/" + ApplicationModel.RegistrationDate.Split('/')[0];
                }
                if (!string.IsNullOrWhiteSpace(RegistrationDate))
                {
                    if (Convert.ToDateTime(RegistrationDate) > DateTime.Now.AddYears(-3))
                    {
                        return "-5";
                    }
                }
                string FeeDepositDate = string.Empty;
                if (!string.IsNullOrWhiteSpace(ApplicationModel.FeeDepositDate))
                {
                    ApplicationModel.FeeDepositDate = ApplicationModel.FeeDepositDate.Replace("-", "/");
                    FeeDepositDate = ApplicationModel.FeeDepositDate.Split('/')[2] + "/" + ApplicationModel.FeeDepositDate.Split('/')[1] + "/" + ApplicationModel.FeeDepositDate.Split('/')[0];
                }

                ApplicationModel.CreatedBy = ApplicationModel.UpdatedBy = _LoggedInUser.UserId.ToString();
                ApplicationModel.CreatedIP = ApplicationModel.UpdatedIP = COMMON_FUNCTION.GetIpAddress();

                if (ApplicationModel.Status == "4" && ApplicationModel.IsSubmit && ApplicationModel.Action == null)
                {
                    ApplicationModel.Status = "5";
                }
                else if (ApplicationModel.Status == "0" && ApplicationModel.IsSubmit && ApplicationModel.Action == null)
                {
                    ApplicationModel.Status = "1";
                }
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = "Insert_AuditEmpanelled";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlConnection;
                cmd.Transaction = transaction;
                cmd.Parameters.AddWithValue("@AuditEmpanelledId", ApplicationModel.AuditEmpanelledId);
                cmd.Parameters.AddWithValue("@ApplicantNameEn", string.IsNullOrWhiteSpace(ApplicationModel.ApplicantNameEn) ? "" : ApplicationModel.ApplicantNameEn);//ApplicationModel.ApplicantNameEn); 
                cmd.Parameters.AddWithValue("@ApplicantNameHi", string.IsNullOrWhiteSpace(ApplicationModel.ApplicantNameHi) ? "" : ApplicationModel.ApplicantNameHi);//ApplicationModel.ApplicantNameHi);
                cmd.Parameters.AddWithValue("@RegistrationNo", ApplicationModel.RegistrationNo);
                cmd.Parameters.AddWithValue("@RBIRegistrationDate", RBIRegistrationDate == "" ? null : RBIRegistrationDate);//RegDate
                cmd.Parameters.AddWithValue("@AddressEn", ApplicationModel.AddressEn);
                cmd.Parameters.AddWithValue("@AddressHi", ApplicationModel.AddressHi);
                cmd.Parameters.AddWithValue("@PinCode", ApplicationModel.PinCode);
                cmd.Parameters.AddWithValue("@ContactNo1", ApplicationModel.ContactNo1);
                cmd.Parameters.AddWithValue("@ContactNo2", ApplicationModel.ContactNo2);
                cmd.Parameters.AddWithValue("@FaxNo", ApplicationModel.FaxNo);
                cmd.Parameters.AddWithValue("@MobileNo1", ApplicationModel.MobileNo);
                cmd.Parameters.AddWithValue("@MobileNo2", ApplicationModel.MobileNo2);
                cmd.Parameters.AddWithValue("@EmailId", ApplicationModel.EmailId);
                cmd.Parameters.AddWithValue("@ApplicantType", ApplicationModel.ApplicantType);
                cmd.Parameters.AddWithValue("@RBICode", ApplicationModel.RBICode);
                cmd.Parameters.AddWithValue("@RegistrationDate", RegistrationDate == "" ? null : RegistrationDate);
                cmd.Parameters.AddWithValue("@NabardCode", ApplicationModel.NabardCode);
                cmd.Parameters.AddWithValue("@NbardRegistrationDate", NbardRegistrationDate == "" ? null : NbardRegistrationDate);//NbardRegistrationDate
                cmd.Parameters.AddWithValue("@IsCompletecourse", ApplicationModel.IsCompletecourse);
                cmd.Parameters.AddWithValue("@CertificateName", ApplicationModel.CertificateName);//ApplicationModel.CertificateName
                cmd.Parameters.AddWithValue("@ExpieranceAreaRemark", ApplicationModel.ExpieranceAreaRemark);
                cmd.Parameters.AddWithValue("@Ammount", ApplicationModel.Ammount);
                cmd.Parameters.AddWithValue("@AmmountDetails", ApplicationModel.AmmountDetails);
                cmd.Parameters.AddWithValue("@CreatedBy", ApplicationModel.CreatedBy);
                cmd.Parameters.AddWithValue("@CreatedIP", ApplicationModel.CreatedIP);
                cmd.Parameters.AddWithValue("@IsSubmit", ApplicationModel.IsSubmit);
                cmd.Parameters.AddWithValue("@SSOID", _LoggedInUser.SSOID);
                cmd.Parameters.AddWithValue("@Action", ApplicationModel.Action);
                cmd.Parameters.AddWithValue("@Status", ApplicationModel.Status);
                cmd.Parameters.AddWithValue("@remark", ApplicationModel.remark == null ? "" : ApplicationModel.remark);

                cmd.Parameters.AddWithValue("@FeeChallan", ApplicationModel.FeeChallan);
                cmd.Parameters.AddWithValue("@FirmDiplomaCertificate", ApplicationModel.FirmDiplomaCertificate);
                cmd.Parameters.AddWithValue("@FirmMembershipCertificate", ApplicationModel.FirmMembershipCertificate);
                cmd.Parameters.AddWithValue("@FirmPracticeCertificate", ApplicationModel.FirmPracticeCertificate);
                cmd.Parameters.AddWithValue("@FirmRegistrationCertificate", ApplicationModel.FirmRegistrationCertificate);
                cmd.Parameters.AddWithValue("@FirmRegistrationNABARD", ApplicationModel.FirmRegistrationNABARD);
                cmd.Parameters.AddWithValue("@FirmTermsConditions", ApplicationModel.FirmTermsConditions);
                cmd.Parameters.AddWithValue("@FeeDepositDate", FeeDepositDate == "" ? null : FeeDepositDate);
                cmd.Parameters.AddWithValue("@IsRural", ApplicationModel.IsRural);
                cmd.Parameters.AddWithValue("@HouseNo", ApplicationModel.HouseNo);
                cmd.Parameters.AddWithValue("@LaneStreetName", ApplicationModel.LaneStreetName);
                cmd.Parameters.AddWithValue("@Locality", ApplicationModel.Locality);
                cmd.Parameters.AddWithValue("@STATE_ID", ApplicationModel.STATE_ID);
                cmd.Parameters.AddWithValue("@DISTRICT_ID", ApplicationModel.DISTRICT_ID);
                cmd.Parameters.AddWithValue("@CITY_ID", ApplicationModel.CITY_ID);
                cmd.Parameters.AddWithValue("@BLOCK_ID", ApplicationModel.BLOCK_ID);
                cmd.Parameters.AddWithValue("@WARD_ID", ApplicationModel.WARD_ID);
                cmd.Parameters.AddWithValue("@GP_ID", ApplicationModel.GP_ID);
                cmd.Parameters.AddWithValue("@VILLAGE_ID", ApplicationModel.VILLAGE_ID);
                cmd.Parameters.AddWithValue("@DIVISION_ID", ApplicationModel.DIVISION_ID);

                //cmd.Parameters.AddWithValue("@VILLAGE_ID", ApplicationModel.VILLAGE_ID);
                //cmd.Parameters.AddWithValue("@DIVISION_ID", ApplicationModel.DIVISION_ID);

                cmd.Parameters.AddWithValue("@ApplicantName", ApplicationModel.ApplicantName);
                cmd.Parameters.AddWithValue("@ApplicantAddress", ApplicationModel.ApplicantAddress);
                cmd.Parameters.AddWithValue("@ApplicantFatherName", ApplicationModel.ApplicantFatherName);
                cmd.Parameters.AddWithValue("@ApplicantAadharNo", ApplicationModel.ApplicantAadharNo);

                cmd.Parameters.AddWithValue("@IsFirm", ApplicationModel.IsFirm);
                cmd.Parameters.AddWithValue("@IsPartnerFirm", ApplicationModel.IsPartnerFirm);


                //inNum = Convert.ToInt32(cmd.ExecuteScalar());
                //;
                // var rr = cmd.ExecuteScalar();
                AuditEmpanelledId1 = Convert.ToInt32(cmd.ExecuteScalar());
                ApplicationModel.AuditEmpanelledId = AuditEmpanelledId1.ToString();
                Result = (AuditEmpanelledId1).ToString();

                //AuditEmpanelledId1 = cmd.ExecuteScalar().ToString();
                //ApplicationModel.AuditEmpanelledId = Convert.ToInt32(AuditEmpanelledId1);
                //Result = AuditEmpanelledId1;



                if (ApplicationModel.AuditformPartnership1 != null)
                {
                    string AuditformPartnershipIDS = string.Join(",", ApplicationModel.AuditformPartnership1.Select(x => x.AuditPartnershipId));
                    ApplicationModel.AuditformPartnership1.ForEach(AuditformPartnership1 =>
                    {

                        string NbardRegistrationDate1 = "";
                        if (!string.IsNullOrWhiteSpace(AuditformPartnership1.MembershipDate))
                        {
                            AuditformPartnership1.MembershipDate = AuditformPartnership1.MembershipDate.Replace("-", "/");
                            NbardRegistrationDate1 = AuditformPartnership1.MembershipDate.Split('/')[2] + "/" + AuditformPartnership1.MembershipDate.Split('/')[1] + "/" + AuditformPartnership1.MembershipDate.Split('/')[0];
                        }
                        if (!string.IsNullOrWhiteSpace(AuditformPartnership1.Name))
                        {
                            if (ApplicationModel.AuditformPartnership1 != null)
                            {
                                cmd = new SqlCommand();
                                cmd.CommandText = "Insert_AuditformPartnership";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Connection = sqlConnection;
                                cmd.Transaction = transaction;
                                cmd.Parameters.AddWithValue("@AuditEmpanelledId", AuditEmpanelledId1);
                                cmd.Parameters.AddWithValue("@AuditPartnershipId", AuditformPartnership1.AuditPartnershipId);
                                cmd.Parameters.AddWithValue("@Name", AuditformPartnership1.Name);
                                cmd.Parameters.AddWithValue("@PartnerName", AuditformPartnership1.PartnerName);
                                cmd.Parameters.AddWithValue("@MembershipNumber", AuditformPartnership1.MembershipNumber);
                                cmd.Parameters.AddWithValue("@MembershipDate", NbardRegistrationDate1);
                                cmd.Parameters.AddWithValue("@CreatedBy", _LoggedInUser.UserRoleDeptMappingId);
                                cmd.Parameters.AddWithValue("@CreatedIP", ApplicationModel.CreatedIP);
                                cmd.Parameters.AddWithValue("@Action", ApplicationModel.Action);
                                cmd.Parameters.AddWithValue("@AuditCertificateFileName", AuditformPartnership1.AuditCertificateFileName);
                                cmd.Parameters.AddWithValue("@MembershipCertificateFileName", AuditformPartnership1.MembershipCertificateFileName);
                                cmd.Parameters.AddWithValue("@PracticeCertificateFileName", AuditformPartnership1.PracticeCertificateFileName);
                                cmd.Parameters.AddWithValue("@Ids", AuditformPartnershipIDS);
                                cmd.ExecuteScalar();
                            }
                        }
                    });
                }
                else if (ApplicationModel.Action == null)
                {
                    cmd = new SqlCommand();
                    cmd.CommandText = "Insert_AuditformPartnership";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = sqlConnection;
                    cmd.Transaction = transaction;
                    cmd.Parameters.AddWithValue("@AuditEmpanelledId", AuditEmpanelledId1);
                    cmd.Parameters.AddWithValue("@AuditPartnershipId", 1);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@PartnerName", "");
                    cmd.Parameters.AddWithValue("@MembershipNumber", "");
                    cmd.Parameters.AddWithValue("@MembershipDate", "");
                    cmd.Parameters.AddWithValue("@CreatedBy", _LoggedInUser.UserRoleDeptMappingId);
                    cmd.Parameters.AddWithValue("@CreatedIP", ApplicationModel.CreatedIP);
                    cmd.Parameters.AddWithValue("@Action", 0);
                    cmd.Parameters.AddWithValue("@AuditCertificateFileName", "");
                    cmd.Parameters.AddWithValue("@MembershipCertificateFileName", "");
                    cmd.Parameters.AddWithValue("@PracticeCertificateFileName", "");
                    cmd.Parameters.AddWithValue("@Ids", "");
                    cmd.ExecuteScalar();
                }



                if (ApplicationModel.AuditfirmAddressdetails1 != null)
                {
                    string AuditformPartnershipIDS = string.Join(",", ApplicationModel.AuditfirmAddressdetails1.Select(x => x.AuditfirmAddressId));
                    ApplicationModel.AuditfirmAddressdetails1.ForEach(AuditfirmAddressdetails1 =>
                    {
                        if (!string.IsNullOrWhiteSpace(AuditfirmAddressdetails1.Name))
                        {

                            if (ApplicationModel.AuditfirmAddressdetails1 != null)
                            {
                                cmd = new SqlCommand();
                                cmd.CommandText = "Insert_AuditfirmAddressdetails";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Connection = sqlConnection;
                                cmd.Transaction = transaction;
                                cmd.Parameters.AddWithValue("@AuditEmpanelledId", AuditEmpanelledId1);
                                cmd.Parameters.AddWithValue("@AuditfirmAddressId", AuditfirmAddressdetails1.AuditfirmAddressId);
                                cmd.Parameters.AddWithValue("@Name", AuditfirmAddressdetails1.Name);
                                cmd.Parameters.AddWithValue("@MobileNo", AuditfirmAddressdetails1.MobileNo);
                                cmd.Parameters.AddWithValue("@CreatedBy", _LoggedInUser.UserRoleDeptMappingId);
                                cmd.Parameters.AddWithValue("@CreatedIP", ApplicationModel.CreatedIP);
                                cmd.Parameters.AddWithValue("@Action", ApplicationModel.Action);
                                cmd.Parameters.AddWithValue("@Ids", AuditformPartnershipIDS);
                                cmd.ExecuteScalar();
                            }
                        }
                    });
                }
                else if (ApplicationModel.Action == null)
                {
                    cmd = new SqlCommand();
                    cmd.CommandText = "Insert_AuditfirmAddressdetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = sqlConnection;
                    cmd.Transaction = transaction;
                    cmd.Parameters.AddWithValue("@AuditEmpanelledId", AuditEmpanelledId1);
                    cmd.Parameters.AddWithValue("@AuditfirmAddressId", 1);
                    cmd.Parameters.AddWithValue("@Name", "");
                    cmd.Parameters.AddWithValue("@MobileNo", "");
                    cmd.Parameters.AddWithValue("@CreatedBy", _LoggedInUser.UserRoleDeptMappingId);
                    cmd.Parameters.AddWithValue("@CreatedIP", ApplicationModel.CreatedIP);
                    cmd.Parameters.AddWithValue("@Action", ApplicationModel.Action);
                    cmd.Parameters.AddWithValue("@Ids", "");
                    cmd.ExecuteScalar();

                }


                if (ApplicationModel.FirmParemenentTraniningDetails1 != null)
                {
                    string AuditformPartnershipIDS = string.Join(",", ApplicationModel.FirmParemenentTraniningDetails1.Select(x => x.FirmParemenentTraniningId));
                    ApplicationModel.FirmParemenentTraniningDetails1.ForEach(FirmParemenentTraniningDetails1 =>
                    {
                        if (!string.IsNullOrWhiteSpace(FirmParemenentTraniningDetails1.AssistantName))
                        {
                            if (ApplicationModel.FirmParemenentTraniningDetails1 != null)
                            {
                                cmd = new SqlCommand();
                                cmd.CommandText = "Insert_FirmParemenentTraniningDetails";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Connection = sqlConnection;
                                cmd.Transaction = transaction;
                                cmd.Parameters.AddWithValue("@AuditEmpanelledId", AuditEmpanelledId1);
                                cmd.Parameters.AddWithValue("@FirmParemenentTraniningId", FirmParemenentTraniningDetails1.FirmParemenentTraniningId);
                                cmd.Parameters.AddWithValue("@AssistantName", FirmParemenentTraniningDetails1.AssistantName);
                                cmd.Parameters.AddWithValue("@Education", FirmParemenentTraniningDetails1.Education);
                                cmd.Parameters.AddWithValue("@Experiance", FirmParemenentTraniningDetails1.Experiance);
                                cmd.Parameters.AddWithValue("@CreatedBy", _LoggedInUser.UserRoleDeptMappingId);
                                cmd.Parameters.AddWithValue("@CreatedIP", ApplicationModel.CreatedIP);
                                cmd.Parameters.AddWithValue("@Action", ApplicationModel.Action);
                                cmd.Parameters.AddWithValue("@Ids", AuditformPartnershipIDS);
                                cmd.ExecuteScalar();
                            }
                        }
                    });
                }
                else if (ApplicationModel.Action == null)
                {
                    cmd = new SqlCommand();
                    cmd.CommandText = "Insert_FirmParemenentTraniningDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = sqlConnection;
                    cmd.Transaction = transaction;
                    cmd.Parameters.AddWithValue("@AuditEmpanelledId", AuditEmpanelledId1);
                    cmd.Parameters.AddWithValue("@FirmParemenentTraniningId", 1);
                    cmd.Parameters.AddWithValue("@AssistantName", "");
                    cmd.Parameters.AddWithValue("@Education", "");
                    cmd.Parameters.AddWithValue("@Experiance", "");
                    cmd.Parameters.AddWithValue("@CreatedBy", _LoggedInUser.UserRoleDeptMappingId);
                    cmd.Parameters.AddWithValue("@CreatedIP", ApplicationModel.CreatedIP);
                    cmd.Parameters.AddWithValue("@Action", ApplicationModel.Action);
                    cmd.Parameters.AddWithValue("@Ids", "");
                    cmd.ExecuteScalar();
                }
                if (Result != "-1" && Result != "0")
                {
                    //Result = "1";
                    transaction.Commit();
                }
                else
                {
                    Result = "-1";
                    transaction.Rollback();
                }
            }
            catch (Exception ex)
            {
                Result = "-1";
                transaction.Rollback();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ComplainUnderSectionModel", FunctionName = "  InsertUpdateComplainUnderSection(ComplainUnderSectionModel Applicationdatat)" }, ex);

            }
            finally
            {
                sqlConnection.Close();
            }

            return Result;

        }

        public string SetAuditEmpanelledFinalSubmit(string TransectionId)
        {
            DataTable resultDt = new DataTable();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("SetAuditEmpanelledFinalSubmit", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TransectionId", DBFactoryParameterType.NVarchar, TransectionId);

                objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable SetAuditEmpanelledFinalSubmit(ref DataPagingModel Details)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return "1";
        }

        public string UpdateAuditEmpanelledCertificate(string Id, string Name)
        {
            DataTable resultDt = new DataTable();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("UpdateAuditEmpanelledCertificate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.NVarchar, Id);
                objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, Name);
                objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable UpdateAuditEmpanelledCertificate(ref DataPagingModel Details)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return "1";
        }
        public string DeleteAuditEmpanelledDetails(int Id, string Type)
        {
            DataTable resultDt = new DataTable();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("DeleteAuditEmpanelledDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.NVarchar, Id);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, Type);
                objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MISModel", FunctionName = "public DataTable DeleteAuditEmpanelledDetails(ref DataPagingModel Details)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return "1";
        }
        public DataTable GetAuditEmpanelledDetails()
        {
            DataTable dt1 = new DataTable();
            try
            {
                // string WhereClause = "";
                int IsAdmin = 0;
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                if (_LoggedInUser.SSOID == "UMESHCJ.DOIT")
                    IsAdmin = 1;
                else
                    IsAdmin = 2;



                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("Get_AuditEmpanelledDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("@IsAdmin", DBFactoryParameterType.Int, IsAdmin);
                //objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                //objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, 0);

                //cmd.Parameters.AddWithValue("@SSOID", _LoggedInUser.SSOID);

                dt1 = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetworkscopedataList(int SocietyId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt1;
        }


        public DataTable GetAuditEmpanelledList(ref DataPagingModel Details)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                string whereCase = "";
                string ApplicationStatus = "";
                string StatusId = "0";
                string SearchText = "";
                string ApplicantType = "0";
                string FinancialYear = "0";
                if (Details.SearchParameter.Where(x => x.Key == "StatusId").Any())
                {
                    StatusId = Details.SearchParameter.Where(x => x.Key == "StatusId").Select(x => x.Value).FirstOrDefault().Replace(" ", "");
                }

                if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                {
                    SearchText = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault().Replace(" ", "");
                }

                if (Details.SearchParameter.Where(x => x.Key == "ApplicantType").Any())
                {
                    ApplicantType = Details.SearchParameter.Where(x => x.Key == "ApplicantType").Select(x => x.Value).FirstOrDefault().Replace(" ", "");
                }
                if (Details.SearchParameter.Where(x => x.Key == "FinancialYear").Any())
                {
                    FinancialYear = Details.SearchParameter.Where(x => x.Key == "FinancialYear").Select(x => x.Value).FirstOrDefault().Replace(" ", "");
                }


                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("Get_AuditEmpanelledList", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.NVarchar, StatusId);
                    objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, SearchText);
                    objDBAccess.AddParameterToCommand("@ApplicantType", DBFactoryParameterType.NVarchar, ApplicantType);
                    objDBAccess.AddParameterToCommand("@FinancialYear", DBFactoryParameterType.NVarchar, FinancialYear);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }
                cmd = objDBAccess.CreateCommand("Get_AuditEmpanelledList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.NVarchar, StatusId);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, SearchText);
                objDBAccess.AddParameterToCommand("@ApplicantType", DBFactoryParameterType.NVarchar, ApplicantType);
                objDBAccess.AddParameterToCommand("@FinancialYear", DBFactoryParameterType.NVarchar, FinancialYear);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "GetAuditEmpanelledList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }



            return dt;
        }



        public DataSet GetAuditEmpanelledDetailsByID(int AuditEmpanelledID)
        {
            DataSet dt1 = new DataSet();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                cmd = objDBAccess.CreateCommand("Get_AllAuditEmpanelledDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@AuditEmpanelledID", DBFactoryParameterType.Int, AuditEmpanelledID);
                //dt1 = objDBAccess.GetTable();
                dt1 = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetworkscopedataList(int SocietyId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt1;
        }

        #region Madhhuri chk If Society Previously Audit By CA or Department

        public string chkIfSocietyPreviouslyAuditByCAorDepartment(string SocietyIdEnc)
        {
            string result = "";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("sp_chkIfSocietyPreviouslyAuditByCAorDepartment", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                result = objDBAccess.ExecuteScalarCommand().ToString();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "chkIfSocietyPreviouslyAuditByCAorDepartment(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return result;
        }
        #endregion

        public DataSet GetFinYearFilterData(string actionName = "")
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_GetAuditorFinYear", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, actionName);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.AuditorModel", FunctionName = "GetFinYearFilterData(string actionName = '" + actionName + "')" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }




        public int AuditMprCommitteeCurrentYearInsert(
            AuditMprCommitteeCurrentYear obj)
        {
            int Result = -1;

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand(
                    "USP_Insert_AuditMprCommitteeCurrentYear",
                    DBFactoryCommandType.PROCEDURE_COMMAND);

                objDBAccess.AddParameterToCommand(
                    "@FinancialYear",
                    DBFactoryParameterType.NVarchar,
                    obj.FinancialYear);

                objDBAccess.AddParameterToCommand(
                    "@Month",
                    DBFactoryParameterType.Int,
                    obj.Month);

                objDBAccess.AddParameterToCommand(
                    "@DistrictId",
                    DBFactoryParameterType.Int,
                    obj.DistrictId);

                objDBAccess.AddParameterToCommand(
                    "@CommitteeTypeId",
                    DBFactoryParameterType.Int,
                    obj.CommitteeTypeId);

                objDBAccess.AddParameterToCommand(
                    "@TotalCommitteesCurrentYear",
                    DBFactoryParameterType.Int,
                    (object)obj.TotalCommitteesCurrentYear ?? DBNull.Value);

                objDBAccess.AddParameterToCommand(
                    "@ProposalDepartment",
                    DBFactoryParameterType.Int,
                    (object)obj.ProposalDepartment ?? DBNull.Value);

                objDBAccess.AddParameterToCommand(
                    "@ProposalCA",
                    DBFactoryParameterType.Int,
                    (object)obj.ProposalCA ?? DBNull.Value);

                objDBAccess.AddParameterToCommand(
                    "@AllocationDepartment",
                    DBFactoryParameterType.Int,
                    (object)obj.AllocationDepartment ?? DBNull.Value);

                objDBAccess.AddParameterToCommand(
                    "@AllocationCA",
                    DBFactoryParameterType.Int,
                    (object)obj.AllocationCA ?? DBNull.Value);

                objDBAccess.AddParameterToCommand(
                    "@AuditedProposalDepartment",
                    DBFactoryParameterType.Int,
                    (object)obj.AuditedProposalDepartment ?? DBNull.Value);

                objDBAccess.AddParameterToCommand(
                    "@AuditedProposalCA",
                    DBFactoryParameterType.Int,
                    (object)obj.AuditedProposalCA ?? DBNull.Value);

                objDBAccess.AddParameterToCommand(
                    "@AuditedAllocationDepartment",
                    DBFactoryParameterType.Int,
                    (object)obj.AuditedAllocationDepartment ?? DBNull.Value);

                objDBAccess.AddParameterToCommand(
                    "@AuditedAllocationCA",
                    DBFactoryParameterType.Int,
                    (object)obj.AuditedAllocationCA ?? DBNull.Value);

                objDBAccess.AddParameterToCommand(
                    "@CreatedBy",
                    DBFactoryParameterType.Int,
                    obj.CreatedBy);

                Result = Convert.ToInt32(
                    objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                Result = -1;

                objILogger.ErrorHandle(
                    new ErrorLogEntity()
                    {
                        PageName = "Cooperative.Models.SocietyModel",
                        FunctionName = "AuditMprCommitteeCurrentYearInsert"
                    },
                    ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }


    }
}