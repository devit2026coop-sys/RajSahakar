using Cooperative.DBConnector;
using System;
using System.Data;
using System.Data.SqlClient;
using Cooperative.Controllers;
using Newtonsoft.Json;
using System.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Net.Http;
using System.Web;
using CoopDAL;
using System.Web.Http.Results;
using Cooperative.Filters;

namespace Cooperative.Models
{
    public class MobileApplicationFlowModel
    {

        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public int ApplicationFlowID { get; set; }

        public int StatusId { get; set; }
        public int UserId { get; set; }
        public int SocietyID { get; set; }
        public string Remark { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ActionDate { get; set; }
        public string CreatedIP { get; set; }
        public string WhereClause { get; set; }

        public MobileApplicationFlowModel(ILogger objErrorLogger)
        {
            objILogger = new DBLogger();
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public MobileApplicationFlowModel()
        {
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }




        public int UpdateApplicationFlow(int ApplicationFlowID, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateApplicationFlow", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ApplicationFlowID", DBFactoryParameterType.BigInt, ApplicationFlowID);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);

                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileApplicationFlowModel", FunctionName = "UpdateApplicationFlow()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetSocietyAppliFlowList(ref DataPagingModel Details, bool IsAmendment)
        {
            dt.Clear();
            string qry = "";
            try
            {
                objDBAccess.OpenConnection();

                string whereCase = "";
                string ApplicationStatus = "";
                string StatusId = "0";
                string RegistrationType = "";
                string OverDue = "0";

                if (Details.SearchParameter.Where(x => x.Key == "NewRegistration").Any() && Details.SearchParameter.Where(x => x.Key == "Profiling").Any())
                {
                    RegistrationType = "";
                }
                else if (Details.SearchParameter.Where(x => x.Key == "NewRegistration").Any())
                {
                    RegistrationType = "N";
                }
                else if (Details.SearchParameter.Where(x => x.Key == "Profiling").Any())
                {
                    RegistrationType = "P";
                }

                if (Details.SearchParameter.Where(x => x.Key == "OverDue30days").Any())
                {
                    OverDue = "30";
                }
                else if (Details.SearchParameter.Where(x => x.Key == "OverDue10days").Any())
                {
                    OverDue = "10";
                }


                if (Details.SearchParameter.Where(x => x.Key == "ApplicationStatus").Any())
                {
                    ApplicationStatus = Details.SearchParameter.Where(x => x.Key == "ApplicationStatus").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "StatusId").Any())
                {
                    StatusId = Details.SearchParameter.Where(x => x.Key == "StatusId").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Any())
                {
                    whereCase += " and ROD.DISTRICT_ID= " + Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.SearchParameter.Where(x => x.Key == "ActId").Any())
                {
                    whereCase += " and s.ActId= " + Details.SearchParameter.Where(x => x.Key == "ActId").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                {
                    string Value = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault().Replace(" ", "");
                    whereCase += " and (replace(s.SocietyName,'',' ') like '%" + Value + "%' or replace(s.RegistrationNo,' ','')  like '%" + Value + "%' or replace(A.Remark,' ','')  like '%" + Value + "%' or  replace(us.OfficerNameEn,' ','') like '%" + Value + "%' or replace(us.OfficerNameHi,' ','') like '%" + Value + "%' or replace(SPS.ProceesStatusTextEn,' ','') like '%" + Value + "%' or replace(SPS.ProceesStatusTextHi,' ','') like '%" + Value + "%')";
                }


                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetSocietyApplicationFlowList", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@ApplicationStatus", DBFactoryParameterType.NVarchar, ApplicationStatus);
                    objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                    objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, _LoggedInUser.ActId);
                    objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _LoggedInUser.DistrictId);
                    objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.NVarchar, _LoggedInUser.DivisionId);
                    objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@IsAmendment", DBFactoryParameterType.NVarchar, IsAmendment);
                    objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.NVarchar, StatusId);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    objDBAccess.AddParameterToCommand("@RegistrationType", DBFactoryParameterType.NVarchar, RegistrationType);
                    objDBAccess.AddParameterToCommand("@OverDue", DBFactoryParameterType.NVarchar, OverDue);
                    qry += objDBAccess.getSqlCommandText();
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetSocietyApplicationFlowList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ApplicationStatus", DBFactoryParameterType.NVarchar, ApplicationStatus);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, _LoggedInUser.ActId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _LoggedInUser.DistrictId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.NVarchar, _LoggedInUser.DivisionId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@IsAmendment", DBFactoryParameterType.NVarchar, IsAmendment);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.NVarchar, StatusId);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                objDBAccess.AddParameterToCommand("@RegistrationType", DBFactoryParameterType.NVarchar, RegistrationType);
                objDBAccess.AddParameterToCommand("@OverDue", DBFactoryParameterType.NVarchar, OverDue);
                qry += objDBAccess.getSqlCommandText();
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileApplicationFlowModel", FunctionName = "GetSocietyAppliFlowList(ref DataPagingModel Details, bool IsAmendment)" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetSocietyAmendment1958FlowList(ref DataPagingModel Details, bool IsAmendment)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                string whereCase = "";
                string ApplicationStatus = "";
                string StatusId = "0";
                string RegistrationType = "";
                string OverDue = "0";

                if (Details.SearchParameter.Where(x => x.Key == "NewRegistration").Any() && Details.SearchParameter.Where(x => x.Key == "Profiling").Any())
                {
                    RegistrationType = "";
                }
                else if (Details.SearchParameter.Where(x => x.Key == "NewRegistration").Any())
                {
                    RegistrationType = "N";
                }
                else if (Details.SearchParameter.Where(x => x.Key == "Profiling").Any())
                {
                    RegistrationType = "P";
                }

                if (Details.SearchParameter.Where(x => x.Key == "OverDue30days").Any())
                {
                    OverDue = "30";
                }
                else if (Details.SearchParameter.Where(x => x.Key == "OverDue10days").Any())
                {
                    OverDue = "10";
                }


                if (Details.SearchParameter.Where(x => x.Key == "ApplicationStatus").Any())
                {
                    ApplicationStatus = Details.SearchParameter.Where(x => x.Key == "ApplicationStatus").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "StatusId").Any())
                {
                    StatusId = Details.SearchParameter.Where(x => x.Key == "StatusId").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Any())
                {
                    whereCase += " and ROD.DISTRICT_ID= " + Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                {
                    string Value = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault().Replace(" ", "");
                    whereCase += " and (replace(s.SocietyName,'',' ') like '%" + Value + "%' or replace(s.RegistrationNo,' ','')  like '%" + Value + "%' or replace(A.Remark,' ','')  like '%" + Value + "%' or  replace(us.OfficerNameEn,' ','') like '%" + Value + "%' or replace(us.OfficerNameHi,' ','') like '%" + Value + "%' or replace(SPS.ProceesStatusTextEn,' ','') like '%" + Value + "%' or replace(SPS.ProceesStatusTextHi,' ','') like '%" + Value + "%')";
                }


                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetSocietyAmendment1958FlowList", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@ApplicationStatus", DBFactoryParameterType.NVarchar, ApplicationStatus);
                    objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                    objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, _LoggedInUser.ActId);
                    objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _LoggedInUser.DistrictId);
                    objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.NVarchar, _LoggedInUser.DivisionId);
                    objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@IsAmendment", DBFactoryParameterType.NVarchar, IsAmendment);
                    objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.NVarchar, StatusId);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    objDBAccess.AddParameterToCommand("@RegistrationType", DBFactoryParameterType.NVarchar, RegistrationType);
                    objDBAccess.AddParameterToCommand("@OverDue", DBFactoryParameterType.NVarchar, OverDue);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetSocietyAmendment1958FlowList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ApplicationStatus", DBFactoryParameterType.NVarchar, ApplicationStatus);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, _LoggedInUser.ActId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _LoggedInUser.DistrictId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.NVarchar, _LoggedInUser.DivisionId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@IsAmendment", DBFactoryParameterType.NVarchar, IsAmendment);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.NVarchar, StatusId);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                objDBAccess.AddParameterToCommand("@RegistrationType", DBFactoryParameterType.NVarchar, RegistrationType);
                objDBAccess.AddParameterToCommand("@OverDue", DBFactoryParameterType.NVarchar, OverDue);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileApplicationFlowModel", FunctionName = "GetSocietyAmendment1958FlowList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetSocietyApplicationFlowListSearch(ref DataPagingModel Details, bool IsAmendment)
        {
            dt.Clear();
            string qry = "";
            try
            {
                objDBAccess.OpenConnection();

                string whereCase = "";
                string ApplicationStatus = "";
                string StatusId = "0";
                string RegistrationType = "";
                string OverDue = "0";

                if (Details.SearchParameter.Where(x => x.Key == "NewRegistration").Any() && Details.SearchParameter.Where(x => x.Key == "Profiling").Any())
                {
                    RegistrationType = "";
                }
                else if (Details.SearchParameter.Where(x => x.Key == "NewRegistration").Any())
                {
                    RegistrationType = "N";
                }
                else if (Details.SearchParameter.Where(x => x.Key == "Profiling").Any())
                {
                    RegistrationType = "P";
                }

                if (Details.SearchParameter.Where(x => x.Key == "OverDue30days").Any())
                {
                    OverDue = "30";
                }
                else if (Details.SearchParameter.Where(x => x.Key == "OverDue10days").Any())
                {
                    OverDue = "10";
                }


                if (Details.SearchParameter.Where(x => x.Key == "ApplicationStatus").Any())
                {
                    ApplicationStatus = Details.SearchParameter.Where(x => x.Key == "ApplicationStatus").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "StatusId").Any())
                {
                    StatusId = Details.SearchParameter.Where(x => x.Key == "StatusId").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Any())
                {
                    whereCase += " and ROD.DISTRICT_ID= " + Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                {
                    string Value = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault().Replace(" ", "");
                    whereCase += " and (replace(s.SocietyName,'',' ') like '%" + Value + "%' or replace(s.RegistrationNo,' ','')  like '%" + Value + "%' or replace(A.Remark,' ','')  like '%" + Value + "%' or  replace(us.OfficerNameEn,' ','') like '%" + Value + "%' or replace(us.OfficerNameHi,' ','') like '%" + Value + "%' or replace(SPS.ProceesStatusTextEn,' ','') like '%" + Value + "%' or replace(SPS.ProceesStatusTextHi,' ','') like '%" + Value + "%')";
                }


                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetSocietyApplicationFlowListSearch", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@ApplicationStatus", DBFactoryParameterType.NVarchar, ApplicationStatus);
                    objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                    objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, _LoggedInUser.ActId);
                    objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _LoggedInUser.DistrictId);
                    objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.NVarchar, _LoggedInUser.DivisionId);
                    objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@IsAmendment", DBFactoryParameterType.NVarchar, IsAmendment);
                    objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.NVarchar, StatusId);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    objDBAccess.AddParameterToCommand("@RegistrationType", DBFactoryParameterType.NVarchar, RegistrationType);
                    objDBAccess.AddParameterToCommand("@OverDue", DBFactoryParameterType.NVarchar, OverDue);
                    qry += objDBAccess.getSqlCommandText();
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetSocietyApplicationFlowListSearch", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ApplicationStatus", DBFactoryParameterType.NVarchar, ApplicationStatus);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, _LoggedInUser.ActId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _LoggedInUser.DistrictId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.NVarchar, _LoggedInUser.DivisionId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@IsAmendment", DBFactoryParameterType.NVarchar, IsAmendment);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.NVarchar, StatusId);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                objDBAccess.AddParameterToCommand("@RegistrationType", DBFactoryParameterType.NVarchar, RegistrationType);
                objDBAccess.AddParameterToCommand("@OverDue", DBFactoryParameterType.NVarchar, OverDue);
                qry += objDBAccess.getSqlCommandText();
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileApplicationFlowModel", FunctionName = " GetSocietyApplicationFlowListSearch(ref DataPagingModel Details, bool IsAmendment)" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


        public DataTable GetAmendmentChanges(string SocietyID, int AmendmentId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess.CreateCommand("GetAmendmentChanges", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyID);
                objDBAccess.AddParameterToCommand("@AmendmentId", DBFactoryParameterType.Int, AmendmentId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileApplicationFlowModel", FunctionName = "GetAmendmentChanges(" + SocietyID + ")" }, ex);
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileApplicationFlowModel", FunctionName = "GetUserForSocietyAssign(" + SocietyID + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }



        public DataTable GetAdministrativeDetailsAmendment(string SocietyID, string WhereClause, int AmendmentId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess.CreateCommand("GetAdministrativeDetailsAmendment", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyID);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                objDBAccess.AddParameterToCommand("@AmendmentId", DBFactoryParameterType.Int, AmendmentId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileApplicationFlowModel", FunctionName = "GetAdministrativeDetailsAmendment(" + SocietyID + "," + WhereClause + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public string SaveSocietyAmendmendData(string SocietyIDEnc, string fileNameCerti, string fileNameMemList, string BylawsFile, bool isNewRegistration, int StatusId, int AmendmentId)
        {
            MobileApplicationFlowModel _Model;
            string Result = "";
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SaveSocietyAmendmendData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.VARCHAR, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@MemberList", DBFactoryParameterType.NVarchar, fileNameMemList);
                objDBAccess.AddParameterToCommand("@RegistrationCertificateFile", DBFactoryParameterType.NVarchar, fileNameCerti);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.NVarchar, StatusId);
                objDBAccess.AddParameterToCommand("@BylawsFile", DBFactoryParameterType.NVarchar, BylawsFile);
                objDBAccess.AddParameterToCommand("@AmendmentId", DBFactoryParameterType.NVarchar, AmendmentId);
                //Result = objDBAccess.ExecuteCommand();
                Result = objDBAccess.ExecuteScalarCommand().ToString();
            }
            catch (Exception ex)
            {
                Result = "Fail";
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileApplicationFlowModel", FunctionName = "SaveSocietyAmendmendData(string SocietyIDEnc, string fileNameCerti, string fileNameMemList)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetApplicationPdfdata(string SocietyIDEnc)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyApplicationFlowPDF", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIDEnc);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileApplicationFlowModel", FunctionName = "GetApplicationPdfdata(string SocietyIDEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


        public DataTable GetSocietyProceesStatusList(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyProceesStatusList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileApplicationFlowModel", FunctionName = "GetSocietyProceesStatusList(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable GetSocietyProceesStatusListByIdsOld(int StatusId, int WorkOnActId, string SocietyIDEnc)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyProceesStatusListByIds", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, WorkOnActId);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileApplicationFlowModel", FunctionName = "GetSocietyProceesStatusList(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


        public DataTable GetSocietyDetailBySocietyIDEnc(string SocietyIDEnc)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyDetailBySocietyIDEnc", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyIDEnc);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileApplicationFlowModel", FunctionName = " GetSocietyDetailBySocietyIDEnc(string SocietyIDEnc )" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable GetSocietyDetailBySocietyIDEncAmendment(string AmendmentId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyDetailBySocietyIDEncAmendment", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@AmendmentId", DBFactoryParameterType.NVarchar, AmendmentId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileApplicationFlowModel", FunctionName = " GetSocietyDetailBySocietyIDEnc(string SocietyIDEnc )" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


        public DataTable GetRoleListForApplicationStatus(string SocietyIDEnc, int NextStatusId, int CurrentStatusId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetRoleListForApplicationStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@NextStatusId", DBFactoryParameterType.Int, NextStatusId);
                objDBAccess.AddParameterToCommand("@CurrentStatusId", DBFactoryParameterType.Int, CurrentStatusId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileApplicationFlowModel", FunctionName = "GetRoleList(string SocietyIDEnc, int NextStatusId, int CurrentStatusId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetOfficerBySocietyIDEnc(string SocietyIDEnc, int NextStatusId, int CurrentStatusId, int RoleId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetOfficeresNameOfSocietyDistrict", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, RoleId);
                objDBAccess.AddParameterToCommand("@NextStatusId", DBFactoryParameterType.Int, NextStatusId);
                objDBAccess.AddParameterToCommand("@CurrentStatusId", DBFactoryParameterType.Int, CurrentStatusId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileApplicationFlowModel", FunctionName = " GetOfficerBySocietyIDEnc(string SocietyIDEnc,int NextStatusId, int CurrentStatusId )" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetApplicationFlowSociety(string SocietyIDEnc)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess.CreateCommand("GetApplicationFlowSociety", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyRegiNoOrSocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@IsRegiNo", DBFactoryParameterType.Int, 0);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileApplicationFlowModel", FunctionName = " GetApplicationFlowSociety(string SocietyIDEnc )" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


        public DataTable GetApplicationFlowSocietyAmendment1958(string SocietyIDEnc, int AmendmentId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess.CreateCommand("GetApplicationFlowSocietyAmendment1958", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyRegiNoOrSocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@IsRegiNo", DBFactoryParameterType.Int, 0);
                objDBAccess.AddParameterToCommand("@AmendmentId", DBFactoryParameterType.Int, AmendmentId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileApplicationFlowModel", FunctionName = " GetApplicationFlowSociety(string SocietyIDEnc )" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetApplicationFlowSocietyByRegiNo(string SocietyRegiNo)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess.CreateCommand("GetApplicationFlowSociety", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyRegiNoOrSocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyRegiNo);
                objDBAccess.AddParameterToCommand("@IsRegiNo", DBFactoryParameterType.Int, 1);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileApplicationFlowModel", FunctionName = " GetApplicationFlowSocietyByRegiNo(string SocietyRegiNo)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }



        //old
        public int AddApplicationFlow(string SocietyIDEnc, int StatusId, string Remark, long UserId, bool IsAmendment = false, string AmendmendAmount = "", string ReceiptNumber = "")
        {
            MobileApplicationFlowModel _Model;
            int Result = 0;
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertApplicationFlow", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.VARCHAR, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Remark);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, UserId);
                objDBAccess.AddParameterToCommand("@IsAmendment", DBFactoryParameterType.Bit, IsAmendment);
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@CreateIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("@AmendmendAmount", DBFactoryParameterType.NVarchar, AmendmendAmount);
                objDBAccess.AddParameterToCommand("@ReceiptNumber", DBFactoryParameterType.NVarchar, ReceiptNumber);
                //Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                ////DataTable societyData = new SocietyModel(new DBLogger()).GetSocietyAppicationStatus(SocietyIDEnc);               
                ////if (societyData.Rows.Count > 0 && societyData.Rows[0]["IsFromSWC"].ToString() == "1")
                ////{
                ////    COMMON_FUNCTION.UpdateSWSStatus(societyData.Rows[0]["RegistrationNo"].ToString(), Convert.ToInt32(societyData.Rows[0]["SocietyStatus"]));
                ////}

            }
            catch (Exception ex)
            {
                Result = 0;
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "public int AddApplicationFlow(string SocietyIDEnc, int StatusId, string Remark, string UserSsoId = \"\", long UserId = 0, bool IsAmendment = false, string AmendmendAmount = \"\", string ReceiptNumber = \"\")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                SendMSG(SocietyIDEnc, StatusId);

            }
            return Result;
        }

        public int AddApplicationFlow(string SocietyIDEnc, int StatusId, string Remark, string UserSsoId = "", long UserId = 0, bool IsAmendment = false, string AmendmendAmount = "", string ReceiptNumber = "")
        {
            //MobileApplicationFlowModel _Model;
            int Result = 0;
            try
            {
                //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertApplicationFlow", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.VARCHAR, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Remark);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, UserId);
                objDBAccess.AddParameterToCommand("@IsAmendment", DBFactoryParameterType.Bit, IsAmendment);
                //objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.BigInt, UserId);
                objDBAccess.AddParameterToCommand("@CreateIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                //objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, UserSsoId);
                objDBAccess.AddParameterToCommand("@AmendmendAmount", DBFactoryParameterType.NVarchar, AmendmendAmount);
                objDBAccess.AddParameterToCommand("@ReceiptNumber", DBFactoryParameterType.NVarchar, ReceiptNumber);
                //Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                ////DataTable societyData = new SocietyModel(new DBLogger()).GetSocietyAppicationStatus(SocietyIDEnc);               
                ////if (societyData.Rows.Count > 0 && societyData.Rows[0]["IsFromSWC"].ToString() == "1")
                ////{
                ////    COMMON_FUNCTION.UpdateSWSStatus(societyData.Rows[0]["RegistrationNo"].ToString(), Convert.ToInt32(societyData.Rows[0]["SocietyStatus"]));
                ////}

            }
            catch (Exception ex)
            {
                Result = 0;
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "public int AddApplicationFlow(string SocietyIDEnc, int StatusId, string Remark, string UserSsoId = \"\", long UserId = 0, bool IsAmendment = false, string AmendmendAmount = \"\", string ReceiptNumber = \"\")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                SendMSG(SocietyIDEnc, StatusId);

            }
            return Result;
        }

        public int UpdatePacsCode(string SocietyIDEnc, string PACS_Code)
        {
            int Result = 0;

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("sp_UpdatePacsCode", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.VARCHAR, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@PACS_Code", DBFactoryParameterType.NVarchar, PACS_Code);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                Result = 0;
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "ApplicationFlowModel", FunctionName = "UpdatePacsCode" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public int GetActIdBySocietyId(string SocietyIDEnc)
        {
            int Result = 0;
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyActId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.VARCHAR, SocietyIDEnc);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

            }
            catch (Exception ex)
            {
                Result = 0;
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "GetActIdBySocietyId(string SocietyIDEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();

            }
            return Result;
        }


        public int AddApplicationFlowAmendment1958(string SocietyIDEnc, int StatusId, string Remark, long UserId, int AmendmentId, string AmendmendAmount = "", string ReceiptNumber = "")
        {
            ApplicationFlowModel _Model;
            int Result = 0;
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertApplicationFlowAmendment", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.VARCHAR, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Remark);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, UserId);
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@CreateIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("@AmendmendAmount", DBFactoryParameterType.NVarchar, AmendmendAmount);
                objDBAccess.AddParameterToCommand("@ReceiptNumber", DBFactoryParameterType.NVarchar, ReceiptNumber);
                objDBAccess.AddParameterToCommand("@AmendmentId", DBFactoryParameterType.Int, AmendmentId);
                //Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                ////DataTable societyData = new SocietyModel(new DBLogger()).GetSocietyAppicationStatus(SocietyIDEnc);               
                ////if (societyData.Rows.Count > 0 && societyData.Rows[0]["IsFromSWC"].ToString() == "1")
                ////{
                ////    COMMON_FUNCTION.UpdateSWSStatus(societyData.Rows[0]["RegistrationNo"].ToString(), Convert.ToInt32(societyData.Rows[0]["SocietyStatus"]));
                ////}

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
                SendMSG(SocietyIDEnc, StatusId);

            }
            return Result;
        }


        public int UpdateAudotorEmpaneledTransaction(string AuditEmpanelledId, string Remark)
        {
            ApplicationFlowModel _Model;
            int Result = 0;
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateAudotorEmpaneledTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@AuditEmpanelledId", DBFactoryParameterType.VARCHAR, AuditEmpanelledId);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Remark);
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                //Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                Result = 0;
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "UpdateAudotorEmpaneledTransaction(string SocietyIDEnc, int StatusId, string Remark,int UserId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();


            }
            return Result;
        }

        public string InsertSocietyReportFile(string SocietyIDEnc, string SocietyReportFile)
        {
            string Result = "";
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertSocietyReportFile", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.VARCHAR, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@SocietyReportFile", DBFactoryParameterType.NVarchar, SocietyReportFile);

                Result = (objDBAccess.ExecuteScalarCommand().ToString());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = " InsertSocietyReportFile(string SocietyIDEnc,string SocietyReportFile )" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public void SendMSG(string SocietyIDEnc, int StatusId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetStatusMSGDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);
                dt = objDBAccess.GetTable();
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        string SMS_MSG = dt.Rows[0]["SMS_MSG"].ToString().Trim();
                        string EMail_MSG = dt.Rows[0]["EMail_MSG"].ToString().Trim();
                        string ContactNo = dt.Rows[0]["ContactNo"].ToString().Trim();
                        if (ContactNo.Length > 5 && SMS_MSG.Length > 5)
                        {
                            EsanchaarController EC = new EsanchaarController();
                            string ReturnVal = EC.SendSocietySMS(SMS_MSG, ContactNo);
                            //string ReturnVal  = EC.SendSocietySMS(SMS_MSG, "9461569464");
                            //string ReturnVal9 = EC.SendSocietySMS(SMS_MSG, "9414038191");
                        }
                        //COMMON_FUNCTION.SendSMS("9414038191", SMS_MSG);  // 9461569464
                        //string EMail = dt.Rows[0]["EMail"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "SendMSG(string SocietyIDEnc ,int StatusId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

        }

        public DataTable GetScietyAdministrativeDetail(string SocietyIDEnc, int MemberType, string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess.CreateCommand("GetScietyAdministrativeDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@MemberType", DBFactoryParameterType.NVarchar, MemberType);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "GetScietyAdministrativeDetail(string SocietyIDEnc, string WhereClause='')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetSocietyListForCertificate()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyListForCertificate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "GetScietyAdministrativeDetail(string SocietyIDEnc, string WhereClause='')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public bool InsertSocietyTestCert(string SocietyIDEnc)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess.CreateCommand("InsertSocietyTestCert", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyIDEnc);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "InsertSocietyTestCert(string SocietyIDEnc, string WhereClause='')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return true;
        }

        public string Addcertificate(string SocietyIDEnc, string fileNameCerti, string fileNameMemList, bool isNewRegistration)
        {
            ApplicationFlowModel _Model;
            string Result = "";
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertApplicationFlowCertificate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.VARCHAR, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@SOCIETIESAUTHORIZEDPERSONS", DBFactoryParameterType.NVarchar, fileNameMemList);
                objDBAccess.AddParameterToCommand("@RegistrationCertificateFile", DBFactoryParameterType.NVarchar, fileNameCerti);
                objDBAccess.AddParameterToCommand("@isNewRegistration", DBFactoryParameterType.Bit, isNewRegistration);

                //Result = objDBAccess.ExecuteCommand();
                Result = objDBAccess.ExecuteScalarCommand().ToString();
            }
            catch (Exception ex)
            {
                Result = "Fail";
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "Addcertificate(string SocietyIDEnc, string fileNameCerti, string fileNameMemList)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public string GetSocietyBylawsFile(string SocietyIDEnc)
        {
            ApplicationFlowModel _Model;
            string Result = "";
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyBylawsFile", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.VARCHAR, SocietyIDEnc);

                //Result = objDBAccess.ExecuteCommand();
                Result = objDBAccess.ExecuteScalarCommand().ToString();
            }
            catch (Exception ex)
            {
                Result = "Fail";
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "GetSocietyBylawsFile(string " + SocietyIDEnc + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }



        public DataTable SocietyFeesStatuslist(string SocietyRegiNo)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyFeeStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, SocietyRegiNo);
                dt = objDBAccess.GetTable();
                DResponse objPGResponse = null;

                foreach (DataRow DR in dt.Rows)
                {
                    if (DR["ResponseString"].ToString() != "")
                    {

                        objPGResponse = JsonConvert.DeserializeObject<DResponse>(DR["ResponseString"].ToString());
                        DR["ResponseString"] = "";
                        DR["Status"] = objPGResponse.TRANSACTIONSTATUSCODE.ToString();
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "SocietyFeesStatuslist(string SocietyRegiNo)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int SaveSocietyObjectionBelong(string SocietyIDEnc, bool ObjectionFlagBasic, bool ObjectionFlagAdmin, bool ObjectionFlagOffice)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SaveSocietyObjectionBelongPermanentSave", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@ObjectionFlagBasic", DBFactoryParameterType.Bit, ObjectionFlagBasic);
                objDBAccess.AddParameterToCommand("@ObjectionFlagAdmin", DBFactoryParameterType.Bit, ObjectionFlagAdmin);
                objDBAccess.AddParameterToCommand("@ObjectionFlagOffice", DBFactoryParameterType.Bit, ObjectionFlagOffice);
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = " GetOfficerBySocietyIDEnc(string SocietyIDEnc )" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return Result;
        }


        public DataTable SendMessageOnActivity(int ProcessId, int ActionId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("MessageOnActivity", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RequestId", DBFactoryParameterType.Int, ProcessId);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "public DataTable SendMessageOnActivity(int ProcessId,int ActionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetMessageByActivityKey(string ActivityKey)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("MessageOnActivity", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RequestId", DBFactoryParameterType.Int, 0);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, 0);
                objDBAccess.AddParameterToCommand("@ActivityKey", DBFactoryParameterType.NVarchar, ActivityKey);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "public DataTable GetMessageByActivityKey(string ActivityKey)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int SaveSocietyStatusAction(string SocietyIDEnc, int StatusId, string RegistrationNo)
        {
            int Result = 0;
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ApplicationStatusSave", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, RegistrationNo);
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.NVarchar, _LoggedInUser.CreatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, _LoggedInUser.CreatedIP);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);

                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = " GetOfficerBySocietyIDEnc(string SocietyIDEnc )" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return Result;
        }

        public DataTable GetSocietyAppliStatus(ref DataPagingModel Details)
        {
            dt.Clear();
            string qry = "";
            try
            {
                objDBAccess.OpenConnection();

                string whereCase = "";
                string ApplicationStatus = "";
                if (Details.SearchParameter.Where(x => x.Key == "ApplicationStatus").Any())
                {
                    ApplicationStatus = Details.SearchParameter.Where(x => x.Key == "ApplicationStatus").Select(x => x.Value).FirstOrDefault();
                }


                if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                {
                    string Value = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault();
                    whereCase += "and (s.SocietyName like '%" + Value + "%' or s.RegistrationNo  like '%" + Value + "%' or A.Remark  like '%" + Value + "%' or  us.OfficerNameEn like '%" + Value + "%' or us.OfficerNameHi like '%" + Value + "%' or SPS.ProceesStatusTextEn like '%" + Value + "%' or SPS.ProceesStatusTextHi like '%" + Value + "%')";
                }


                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetSocietyApplicationListForStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ApplicationStatus", DBFactoryParameterType.NVarchar, ApplicationStatus);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, _LoggedInUser.ActId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _LoggedInUser.DistrictId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.NVarchar, _LoggedInUser.DivisionId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                qry += objDBAccess.getSqlCommandText();
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "GetSocietyAppliStatus(ref DataPagingModel Details)" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetSocietyPaymentTransectin(ref DataPagingModel Details, string SocietyIdENC)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                string whereCase = "";
                string ApplicationStatus = "";
                string StatusId = "0";
                if (Details.SearchParameter.Where(x => x.Key == "ApplicationStatus").Any())
                {
                    ApplicationStatus = Details.SearchParameter.Where(x => x.Key == "ApplicationStatus").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "StatusId").Any())
                {
                    StatusId = Details.SearchParameter.Where(x => x.Key == "StatusId").Select(x => x.Value).FirstOrDefault();
                }
                if (Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Any())
                {
                    whereCase += " and ROD.DISTRICT_ID= " + Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Select(x => x.Value).FirstOrDefault();
                }

                if (Details.SearchParameter.Where(x => x.Key == "SearchText").Any())
                {
                    string Value = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault().Replace(" ", "");
                    whereCase += " and (replace(S.SocietyName,'',' ') like '%" + Value + "%' or replace(S.RegistrationNo,' ','')  like '%" + Value + "%' or replace(F.Remark,' ','')  like '%" + Value + "%' or replace(P.ProceesStatusTextEn,' ','') like '%" + Value + "%' or replace(P.ProceesStatusTextHi,' ','') like '%" + Value + "%')";
                }


                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1 && string.IsNullOrWhiteSpace(SocietyIdENC))
                {
                    cmd = objDBAccess.CreateCommand("GetSocietyPaymentTransectin", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@SocietyIdENC", DBFactoryParameterType.NVarchar, SocietyIdENC);
                    objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
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

                cmd = objDBAccess.CreateCommand("GetSocietyPaymentTransectin", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdENC", DBFactoryParameterType.NVarchar, SocietyIdENC);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "GetSocietyPaymentTransectin()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetSocietyProceesStatusListByIds(int StatusId, int WorkOnActId, string SocietyIDEnc, int Flag = 0, bool IsAmendment = false, int AmendmentId = 0)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyProceesStatusListByIds", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, WorkOnActId);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, StatusId);
                objDBAccess.AddParameterToCommand("@Flag", DBFactoryParameterType.Int, Flag);
                objDBAccess.AddParameterToCommand("@IsAmendment", DBFactoryParameterType.Bit, IsAmendment);
                objDBAccess.AddParameterToCommand("@AmendmentId", DBFactoryParameterType.Int, AmendmentId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "GetSocietyProceesStatusList(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int SaveSocietystatusdata(string StatusId, string SocietyIDEnc, string Remark, int ApplicationFlowID, string Attachment)
        {
            int Result = 0;
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SaveSocietyStatusSave", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Remark);
                objDBAccess.AddParameterToCommand("@ApplicationFlowID", DBFactoryParameterType.Int, ApplicationFlowID);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.NVarchar, StatusId);
                objDBAccess.AddParameterToCommand("@Attachment", DBFactoryParameterType.NVarchar, Attachment);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = " GetOfficerBySocietyIDEnc(string SocietyIDEnc )" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return Result;
        }
        public int SaveStatusApplicationPayment(string SocietyIDEnc, string Remark, int ApplicationPaymentId, string ApplicationPayment)
        {
            int Result = 0;
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SaveStatusApplicationPayment", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Remark);
                objDBAccess.AddParameterToCommand("@ApplicationPayment", DBFactoryParameterType.NVarchar, ApplicationPayment);
                objDBAccess.AddParameterToCommand("@ApplicationPaymentId", DBFactoryParameterType.Int, ApplicationPaymentId);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = " SaveStatusApplicationPayment(string SocietyIDEnc )" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return Result;
        }

        public int SetApplicationUser(string SocietyIDEnc, string Remark, int OfficeId, int ApplicationFlowID)
        {
            int Result = 0;
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SetApplicationUser", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Remark);
                objDBAccess.AddParameterToCommand("@OfficeId", DBFactoryParameterType.NVarchar, OfficeId);
                objDBAccess.AddParameterToCommand("@ApplicationFlowID", DBFactoryParameterType.Int, ApplicationFlowID);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = " SaveStatusApplicationPayment(string SocietyIDEnc )" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return Result;
        }

        public DataTable GetSocietieCheckList(string ApplicationNo)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietystatuslistcheck", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, ApplicationNo);
                objDBAccess.AddParameterToCommand("@Districtid", DBFactoryParameterType.NVarchar, _LoggedInUser.DistrictId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "GetSocietyProceesStatusList(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int DeleteSocietieCheckList(int Societyid)
        {
            int Result = 0;
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteSocietydetailsstatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Societyid", DBFactoryParameterType.Int, Societyid);
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = " SaveStatusApplicationPayment(string SocietyIDEnc )" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return Result;
        }

        /// <summary>
        ///  GenerateMember_PDF   By Pradeep 2024-11-20
        /// </summary>
        /// <param name="societyIdEnc"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string GenerateMember_PDF(string societyIdEnc, string filePath)
        {
            DataSet ds = GetSocietyMemberDetails(societyIdEnc);
            Document document = new Document(PageSize.A4, 36, 36, 36, 36);
            try
            {
                if (ds != null && ds.Tables.Count > 1)
                {
                    DataTable dtSD = ds.Tables[0];
                    DataTable dtAD = ds.Tables[1];
                    PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                    document.Open();
                    //PdfFont unicodeFont = PdfFontFactory.CreateFont(fontPath, "Identity-H", true);
                    string ARIALUNI_TFF = Path.Combine(HttpContext.Current.Server.MapPath("~/fonts/"), "Arial Unicode Font.TTF");

                    //Create a base font object making sure to specify IDENTITY-H
                    BaseFont bf = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                    // Title and Header
                    Font titleFont = new Font(bf, 16, Font.BOLD);
                    Font subtitleFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                    Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                    Font regularFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                    Font italicFont = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 10);
                    Font underlineFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, Font.UNDERLINE);

                    // Title Section
                    Paragraph title = new Paragraph("राजस्थान सरकार / Government of Rajasthan", titleFont);
                    title.Alignment = Element.ALIGN_CENTER;
                    document.Add(title);

                    Paragraph subtitle = new Paragraph("सहकारीता विभाग / COOPERATIVE DEPARTMENT", subtitleFont);
                    subtitle.Alignment = Element.ALIGN_CENTER;
                    document.Add(subtitle);

                    Paragraph sanctionOrder = new Paragraph(new Chunk("कार्यकारिणी प्रमाण पत्र / EXECUTIVE COMMITTEE CERTIFICATE", underlineFont));
                    sanctionOrder.Alignment = Element.ALIGN_CENTER;
                    sanctionOrder.SpacingAfter = 20;
                    document.Add(sanctionOrder);
                    // Header Details
                    Chunk p1C1 = new Chunk("पं. सं./REG. NO.- ", regularFont);
                    Chunk p1C2 = new Chunk($"{dtSD.Rows[0]["RegistrationNo"].ToString()} ", boldFont);
                    Chunk p1C3 = new Chunk("दिनांक / DATE: ", regularFont);
                    Chunk p1C4 = new Chunk($"{DateTime.Now.ToString("dd-MM-yyyy")} ", boldFont);
                    Paragraph p1 = new Paragraph();
                    p1.SpacingAfter = 15;
                    p1.Add(p1C1);
                    p1.Add(p1C2);
                    p1.Add(p1C3);
                    p1.Add(p1C4);
                    document.Add(p1);

                    //Registration Details
                    if (dtSD.Rows[0]["IsAmendmentAtProfiling"].ToString() == "True" && dtSD.Rows[0]["AmendmentType"].ToString().IndexOf("4") > -1)
                    {
                        Paragraph p2 = new Paragraph();
                        Chunk p2C1 = new Chunk("यह प्रमाणित किया जाता है कि ", regularFont);
                        Chunk p2C2 = new Chunk($"{dtSD.Rows[0]["RegistrationNo"].ToString()} ", boldFont);
                        Chunk p2C3 = new Chunk("जिला ", regularFont);
                        Chunk p2C4 = new Chunk($"{dtSD.Rows[0]["DISTRICT_ENG"].ToString()} ", boldFont);
                        Chunk p2C5 = new Chunk($"का रजिस्ट्रेशन ", regularFont);
                        Chunk p2C6 = new Chunk($"{dtSD.Rows[0]["ActNameHi"].ToString()} ", boldFont);
                        Chunk p2C7 = new Chunk($"के अन्तर्गत पंजीकृत हैं, अब इस संस्थान की परिवर्तित कार्यकारिणी निम्नलिखित हैं। यह प्रमाण पत्र मेरे डिजिटल हस्ताक्षरों से आज जारी किया गया है। ", regularFont);
                        p2.SpacingAfter = 15;
                        p2.Add(p2C1);
                        p2.Add(p2C2);
                        p2.Add(p2C3);
                        p2.Add(p2C4);
                        p2.Add(p2C5);
                        p2.Add(p2C6);
                        p2.Add(p2C7);
                        document.Add(p2);

                        Paragraph p3 = new Paragraph();
                        Chunk p3C1 = new Chunk("IT IS CERTIFIED THAT ", regularFont);
                        Chunk p3C2 = new Chunk($"{dtSD.Rows[0]["RegistrationNo"].ToString()} ", boldFont);
                        Chunk p3C3 = new Chunk("AT  DISTRICT ", regularFont);
                        Chunk p3C4 = new Chunk($"{dtSD.Rows[0]["DISTRICT_ENG"].ToString()} ", boldFont);
                        Chunk p3C5 = new Chunk($"IS REGISTERED UNDER ", regularFont);
                        Chunk p3C6 = new Chunk($"{dtSD.Rows[0]["ActNameHi"].ToString()} ", boldFont);
                        Chunk p3C7 = new Chunk($"HAS THE FOLLOWING EXECUTIVE COMMITTEE AT THE TIME OF REGISTRATION. THIS CERTIFICATE IS ISSUED TODAY UNDER MY DIGITAL SIGNATURE. ", regularFont);
                        p3.SpacingAfter = 15;
                        p3.Add(p3C1);
                        p3.Add(p3C2);
                        p3.Add(p3C3);
                        p3.Add(p3C4);
                        p3.Add(p3C5);
                        p3.Add(p3C6);
                        p3.Add(p3C7);
                        document.Add(p3);
                    }
                    else
                    {
                        Paragraph p2 = new Paragraph();
                        Chunk p2C1 = new Chunk("यह प्रमाणित किया जाता है कि ", regularFont);
                        Chunk p2C2 = new Chunk($"{dtSD.Rows[0]["RegistrationNo"].ToString()} ", boldFont);
                        Chunk p2C3 = new Chunk("जिला ", regularFont);
                        Chunk p2C4 = new Chunk($"{dtSD.Rows[0]["DISTRICT_ENG"].ToString()} ", boldFont);
                        Chunk p2C5 = new Chunk($"का रजिस्ट्रेशन ", regularFont);
                        Chunk p2C6 = new Chunk($"{dtSD.Rows[0]["ActNameEn"].ToString()} ", boldFont);
                        Chunk p2C7 = new Chunk($"के अन्तर्गत पंजीकृत हैं, में पंजीकरण के समय निम्नलिखित कार्यकारिणी हैं। यह प्रमाण पत्र मेरे डिजिटल हस्ताक्षरों से आज जारी किया गया है। ", regularFont);
                        p2.SpacingAfter = 15;
                        p2.Add(p2C1);
                        p2.Add(p2C2);
                        p2.Add(p2C3);
                        p2.Add(p2C4);
                        p2.Add(p2C5);
                        p2.Add(p2C6);
                        p2.Add(p2C7);
                        document.Add(p2);

                        Paragraph p3 = new Paragraph();
                        Chunk p3C1 = new Chunk("IT IS CERTIFIED THAT ", regularFont);
                        Chunk p3C2 = new Chunk($"{dtSD.Rows[0]["RegistrationNo"].ToString()} ", boldFont);
                        Chunk p3C3 = new Chunk("AT  DISTRICT ", regularFont);
                        Chunk p3C4 = new Chunk($"{dtSD.Rows[0]["DISTRICT_ENG"].ToString()} ", boldFont);
                        Chunk p3C5 = new Chunk($"IS REGISTERED UNDER ", regularFont);
                        Chunk p3C6 = new Chunk($"{dtSD.Rows[0]["ActNameHi"].ToString()} ", boldFont);
                        Chunk p3C7 = new Chunk($"HAS THE FOLLOWING EXECUTIVE COMMITTEE AT THE TIME OF REGISTRATION. THIS CERTIFICATE IS ISSUED TODAY UNDER MY DIGITAL SIGNATURE. ", regularFont);
                        p3.SpacingAfter = 15;
                        p3.Add(p3C1);
                        p3.Add(p3C2);
                        p3.Add(p3C3);
                        p3.Add(p3C4);
                        p3.Add(p3C5);
                        p3.Add(p3C6);
                        p3.Add(p3C7);
                        document.Add(p3);

                    }

                    // Detailed Table for Beneficiaries
                    PdfPTable adT = null;
                    if (dtSD.Rows[0]["ActId"].ToString() == "3" && dtSD.Rows[0]["SocietyTypeId"].ToString() == "9")
                    {
                        adT = new PdfPTable(9);
                        adT.SetWidths(new float[] { 1, 1, 3, 3, 3, 2, 3, 3, 2 });
                    }
                    else
                    {
                        adT = new PdfPTable(7);
                        adT.SetWidths(new float[] { 1, 1, 3, 2, 3, 3, 2 });
                    }
                    adT.WidthPercentage = 100;
                    adT.SpacingBefore = 20;

                    // Detailed Table Header
                    adT.AddCell(new PdfPCell(new Phrase("Sr. No.", boldFont)) { BackgroundColor = BaseColor.LIGHT_GRAY, BorderWidth = 1 });
                    adT.AddCell(new PdfPCell(new Phrase("Photo", boldFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    if (dtSD.Rows[0]["ActId"].ToString() == "3" && dtSD.Rows[0]["SocietyTypeId"].ToString() == "9")
                    {
                        adT.AddCell(new PdfPCell(new Phrase("Name of Society", boldFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                        adT.AddCell(new PdfPCell(new Phrase("Post in Society", boldFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    }
                    adT.AddCell(new PdfPCell(new Phrase("Name", boldFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    adT.AddCell(new PdfPCell(new Phrase("Gender", boldFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    adT.AddCell(new PdfPCell(new Phrase("Post", boldFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    if (dtSD.Rows[0]["ActId"].ToString() != "3" && dtSD.Rows[0]["SocietyTypeId"].ToString() != "9")
                    {
                        adT.AddCell(new PdfPCell(new Phrase("Address", boldFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    }
                    adT.AddCell(new PdfPCell(new Phrase("DOB", boldFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });


                    if (dtAD != null && dtAD.Rows.Count > 0)
                    {
                        int counter = 0;
                        foreach (DataRow dr in dtAD.Rows)
                        {
                            counter++;
                            adT.AddCell(new PdfPCell(new Phrase(counter.ToString(), regularFont)));
                            iTextSharp.text.Image photo = iTextSharp.text.Image.GetInstance(dr["Photo"].ToString());
                            photo.ScaleToFit(45f, 40f);
                            photo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                            adT.AddCell(new PdfPCell(photo) { HorizontalAlignment = 1, VerticalAlignment = 5 });
                            if (dtSD.Rows[0]["ActId"].ToString() == "3" && dtSD.Rows[0]["SocietyTypeId"].ToString() == "9")
                            {
                                adT.AddCell(new PdfPCell(new Phrase(dr["NameOfSociety"].ToString(), regularFont)));
                                adT.AddCell(new PdfPCell(new Phrase(dr["PostinSociety"].ToString(), regularFont)));
                            }
                            adT.AddCell(new PdfPCell(new Phrase(dr["Name"].ToString(), regularFont)));
                            adT.AddCell(new PdfPCell(new Phrase(dr["Gender"].ToString(), regularFont)));
                            adT.AddCell(new PdfPCell(new Phrase(dr["POSTNAME"].ToString(), regularFont)));
                            if (dtSD.Rows[0]["ActId"].ToString() != "3" && dtSD.Rows[0]["SocietyTypeId"].ToString() != "9")
                            {
                                adT.AddCell(new PdfPCell(new Phrase(dr["Address"].ToString(), regularFont)));
                            }
                            adT.AddCell(new PdfPCell(new Phrase(Convert.ToDateTime(dr["DOB"]).ToString("dd-MM-yyyy"), regularFont)));
                        }

                        document.Add(adT);
                    }
                    document.Close();
                }
                return filePath;
            }
            catch (Exception ex)
            {
                document.Close();
                return "";
            }

        }

        private DataSet GetSocietyMemberDetails(string societyIdEnc)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("Test_GetSocietyMemberDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, societyIdEnc);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlow", FunctionName = "GetSocietyMemberDetails(string societyIdEnc))" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        #region Madhuri PendingEsignDoc
        [CustomAuthentication]
        public DataTable GetPendingEsignDoc()
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess.CreateCommand("sp_SocietyEsignTemp", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetSocietyFiles");

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = " GetPendingEsignDoc()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public int PendingEsignDocStatusChange(int SocietyId)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess.CreateCommand("sp_SocietyEsignTemp", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, SocietyId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "StatusChange");

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = " GetPendingEsignDoc()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return Result;
        }



        #endregion

        public DataSet GetSocietyBankDetails(int societyId)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetSocietyBankDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, societyId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlow", FunctionName = "GetSocietyMemberDetails(string societyIdEnc))" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataSet GetSocietyBankAndBranchDetails(int societyId, int statusId)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetSocietyBankAndBranchDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, societyId);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, statusId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlow", FunctionName = "GetSocietyMemberDetails(string societyIdEnc))" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

        public DataTable GetSocietyApplicantDetails(string SocietyId, int AmendmentId = 0)
        {
            DataTable appData = new DataTable();
            if (SocietyId == "") return appData;

            string qry = "";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmdAddress = new SqlCommand();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyApplicantDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdENC", DBFactoryParameterType.NVarchar, SocietyId);
                objDBAccess.AddParameterToCommand("@AmendmentId", DBFactoryParameterType.Int, AmendmentId);
                qry = objDBAccess.getSqlCommandText();
                appData = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "DataTable GetSocietyApplicantDetails(string SocietyId, int AmendmentId = 0)" + qry }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return appData;
        }

        #region     Without_Login_Session_DirectWebPage_Methods     ---START---

        public DataTable GetApplicationFlowSociety_DirectWebPage(string SocietyIDEnc)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetApplicationFlowSociety", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyRegiNoOrSocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@IsRegiNo", DBFactoryParameterType.Int, 0);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MobileApplicationFlowModel", FunctionName = "public DataTable GetApplicationFlowSociety_DirectWebPage(string SocietyIDEnc )" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public string Addcertificate_DirectWebPage(string SocietyIDEnc, string fileNameCerti, string fileNameMemList, bool isNewRegistration)
        {            
            string Result = "";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertApplicationFlowCertificate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.VARCHAR, SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@SOCIETIESAUTHORIZEDPERSONS", DBFactoryParameterType.NVarchar, fileNameMemList);
                objDBAccess.AddParameterToCommand("@RegistrationCertificateFile", DBFactoryParameterType.NVarchar, fileNameCerti);
                objDBAccess.AddParameterToCommand("@isNewRegistration", DBFactoryParameterType.Bit, isNewRegistration);

                //Result = objDBAccess.ExecuteCommand();
                Result = objDBAccess.ExecuteScalarCommand().ToString();
            }
            catch (Exception ex)
            {
                Result = "Fail";
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "public string Addcertificate_DirectWebPage(string SocietyIDEnc, string fileNameCerti, string fileNameMemList, bool isNewRegistration)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        #endregion  Without_Login_Session_DirectWebPage_Methods     -----END---

    }

}