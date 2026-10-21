using Cooperative.DBConnector;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using Cooperative.Controllers;
using Newtonsoft.Json;
using System.Linq;
namespace Cooperative.Models
{
    public class ApplicationFlowModel
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

        public ApplicationFlowModel(ILogger objErrorLogger)
        {
            objILogger = new DBLogger();
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public ApplicationFlowModel()
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
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "UpdateApplicationFlow()" }, ex);
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
                    whereCase += "and (s.SocietyName like '%" + Value + "%' or s.RegistrationNo  like '%" + Value + "%' or A.Remark  like '%" + Value + "%' or  us.OfficerNameEn like '%" + Value + "%' or us.OfficerNameHi like '%"+Value+ "%' or SPS.ProceesStatusTextEn like '%"+Value+ "%' or SPS.ProceesStatusTextHi like '%"+Value+"%')";
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
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                    objDBAccess.AddParameterToCommand("@IsAmendment", DBFactoryParameterType.NVarchar, IsAmendment);
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
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, whereCase);
                objDBAccess.AddParameterToCommand("@IsAmendment", DBFactoryParameterType.NVarchar, IsAmendment);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "GetSocietyAppliFlowList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "GetApplicationPdfdata(string SocietyIDEnc)" }, ex);
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "GetSocietyProceesStatusList(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable GetSocietyProceesStatusListByIds(int StatusId,int  WorkOnActId, string  SocietyIDEnc )
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "GetSocietyProceesStatusList(string WhereClause = '')" }, ex);
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = " GetSocietyDetailBySocietyIDEnc(string SocietyIDEnc )" }, ex);
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "GetRoleList(string SocietyIDEnc, int NextStatusId, int CurrentStatusId)" }, ex);
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = " GetOfficerBySocietyIDEnc(string SocietyIDEnc,int NextStatusId, int CurrentStatusId )" }, ex);
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = " GetApplicationFlowSociety(string SocietyIDEnc )" }, ex);
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
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = " GetApplicationFlowSocietyByRegiNo(string SocietyRegiNo)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public int AddApplicationFlow(string SocietyIDEnc, int StatusId, string Remark, long UserId, bool IsAmendment = false)
        {
            ApplicationFlowModel _Model;
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
                //Result = objDBAccess.ExecuteCommand();
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
                SendMSG(SocietyIDEnc, StatusId);

            }
            return Result;
        }


        public string InsertSocietyReportFile(string SocietyIDEnc,string SocietyReportFile )
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
                        if(ContactNo.Length>5 && SMS_MSG.Length>5)
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
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "GetSocietyBylawsFile(string "+ SocietyIDEnc + ")" }, ex);
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
                    else {

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


        public DataTable SendMessageOnActivity(int ProcessId,int ActionId)
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


        public DataTable GetAmendmentChanges(string SocietyID)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess.CreateCommand("GetAmendmentChanges", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyID);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "GetAmendmentChanges("+ SocietyID + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable GetAdministrativeDetailsAmendment(string SocietyID,string WhereClause)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = objDBAccess.CreateCommand("GetAdministrativeDetailsAmendment", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, SocietyID);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.ApplicationFlowModel", FunctionName = "GetAdministrativeDetailsAmendment(" + SocietyID + ","+ WhereClause + ")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public string SaveSocietyAmendmendData(string SocietyIDEnc, string fileNameCerti, string fileNameMemList, bool isNewRegistration,int StatusId)
        {
            ApplicationFlowModel _Model;
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

    }

}