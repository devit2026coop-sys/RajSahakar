using Cooperative.Controllers;
using Cooperative.DBConnector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Net;


namespace Cooperative.Models
{
    public class CitizenModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;


        public CitizenModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public DataTable SocietyList(SocietyRegistrationEntity Obj)
        {
            dt.Clear();
            try
            {
                string WhereClause = " and 1=1 " + Obj.WhereClause;
                if (Obj.UserType == "C")
                {
                    if (Obj != null && Obj.SSOID != null)
                        WhereClause += Obj.SSOID.Length > 0 ? (WhereClause = " and SSOID ='" + Obj.SSOID + "'") : "";
                }
                else if (Obj.SocietyIDEnc != null && Obj.UserType == "K")
                {
                    WhereClause += Obj.SocietyIDEnc.Length > 0 ? (WhereClause = " and Societies.[SocietyID] =cast( Dbo.Decrypt('" + Obj.SocietyIDEnc + "')as int) ") : "";
                }


                WhereClause += Obj.ActId > 0 ? (WhereClause = " and [Societies].ActId =" + Obj.ActId + "") : "";
                WhereClause += Obj.SocietyTypeId > 0 ? (WhereClause = " and Societies.[SocietyTypeId] =" + Obj.SocietyTypeId + "") : "";
                WhereClause += Obj.ClassId > 0 ? (WhereClause = " and Societies.[ClassId] =" + Obj.ClassId + "") : "";


                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "SocietyList(SocietyRegistrationEntity Obj)" }, ex);
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
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                //objDBAccess.AddParameterToCommand("@DepartmentSSO_ID", DBFactoryParameterType.NVarchar, Obj.SSOID);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "SocietyList(SocietyRegistrationEntity Obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;

        }
        public DataTable BankstamentList(string SocietyIdEnc)
        {
            dt.Clear();
            try
            {
                string WhereClause = " and 1=1";



                if (SocietyIdEnc != null)
                {
                    WhereClause += SocietyIdEnc.Length > 0 ? (WhereClause = " and Societies.[SocietyID] =cast( Dbo.Decrypt('" + SocietyIdEnc + "')as int) ") : "";
                }

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "BankstamentList(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable SocietyListForAuditRPT(SocietyRegistrationEntity Obj)
        {
            dt.Clear();
            try
            {
                string WhereClause = " and 1=1 " + Obj.WhereClause;
                if (Obj.UserType == "C" || Obj.UserType == "K")
                {
                    if (Obj != null && Obj.SSOID != null)
                        WhereClause += Obj.SSOID.Length > 0 ? (WhereClause = " and SSOID ='" + Obj.SSOID + "'") : "";
                }


                WhereClause += Obj.ActId > 0 ? (WhereClause = " and [Societies].ActId =" + Obj.ActId + "") : "";
                WhereClause += Obj.SocietyTypeId > 0 ? (WhereClause = " and Societies.[SocietyTypeId] =" + Obj.SocietyTypeId + "") : "";
                WhereClause += Obj.ClassId > 0 ? (WhereClause = " and Societies.[ClassId] =" + Obj.ClassId + "") : "";


                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyListForAuditRPT", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "SocietyListForAuditRPT(SocietyRegistrationEntity Obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        //public DataTable GetAllPhotos()
        //{
        //    dt.Clear();
        //    try
        //    {                
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("getAadhar", DBFactoryCommandType.PROCEDURE_COMMAND);               
        //        dt = objDBAccess.GetTable();
        //        objDBAccess.CloseConnection();
        //        foreach (DataRow DR in dt.Rows )
        //        {
        //            objDBAccess.OpenConnection();
        //            SqlCommand cmdd = objDBAccess.CreateCommand("encAadhar", DBFactoryCommandType.PROCEDURE_COMMAND);
        //            objDBAccess.AddParameterToCommand("@AdministrativeId", DBFactoryParameterType.Int, int.Parse(DR["AdministrativeId"].ToString()));
        //            objDBAccess.AddParameterToCommand("@AadharNoEnc", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.Encrypt(DR["AadharNo"].ToString()));
        //            int Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "SocietyList()" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return dt;
        //}

        public void UpdatePhotos(List<SocietyAdministrativeEntity> LstDdata)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                foreach (SocietyAdministrativeEntity _Model in LstDdata)
                {
                    SqlCommand cmd = objDBAccess.CreateCommand("UpdatePhoto", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@AdministrativeId", DBFactoryParameterType.Int, _Model.AdministrativeId);
                    objDBAccess.AddParameterToCommand("@Photo", DBFactoryParameterType.NVarchar, _Model.Photo);
                    objDBAccess.AddParameterToCommand("@AAdhar", DBFactoryParameterType.NVarchar, _Model.AADHAARNo);
                    int Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "UpdatePhotos(List<SocietyAdministrativeEntity> LstDdata)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

        }
        public void UpdateDoc(List<SocietyRegistrationEntity> LstDdata)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                foreach (SocietyRegistrationEntity _Model in LstDdata)
                {
                    SqlCommand cmd = objDBAccess.CreateCommand("UpdateDoc", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@SocietyID", DBFactoryParameterType.Int, _Model.SocietyID);
                    objDBAccess.AddParameterToCommand("@RegistrationCertificateFile", DBFactoryParameterType.NVarchar, _Model.RegistrationCertificateFile);
                    objDBAccess.AddParameterToCommand("@ByeLawsSocietyFile", DBFactoryParameterType.NVarchar, _Model.ByeLawsSocietyFile);
                    objDBAccess.AddParameterToCommand("@RulesAndRegulationFile", DBFactoryParameterType.NVarchar, _Model.RulesAndRegulationFile);
                    objDBAccess.AddParameterToCommand("@MemberListExcelFile", DBFactoryParameterType.NVarchar, _Model.MemberListExcelFile);
                    int Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "UpdateDoc(List<SocietyRegistrationEntity> LstDdata)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

        }
        public DataTable SocietyListNotAssignedToInspector(SocietyRegistrationEntity Obj)
        {
            dt.Clear();
            try
            {
                string WhereClause = " and 1=1 " + Obj.WhereClause;
                if (Obj.UserType == "C")
                {
                    if (Obj != null && Obj.SSOID != null)
                        WhereClause += Obj.SSOID.Length > 0 ? (WhereClause = " and SSOID ='" + Obj.SSOID + "'") : "";
                }


                WhereClause += Obj.ActId > 0 ? (WhereClause = " and [Societies].ActId =" + Obj.ActId + "") : "";
                WhereClause += Obj.SocietyTypeId > 0 ? (WhereClause = " and Societies.[SocietyTypeId] =" + Obj.SocietyTypeId + "") : "";
                WhereClause += Obj.ClassId > 0 ? (WhereClause = " and Societies.[ClassId] =" + Obj.ClassId + "") : "";


                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyListNotAssignedToInspector", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                objDBAccess.AddParameterToCommand("@UserRoldptmap", DBFactoryParameterType.Int, Obj.UserRoleDeptMappingId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "SocietyListNotAssignedToInspector(SocietyRegistrationEntity Obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable SocietyListNotAssignedToInspectorForCitizen(SocietyRegistrationEntity Obj)
        {
            dt.Clear();
            try
            {
                string WhereClause = " and 1=1 " + Obj.WhereClause;

                if (Obj != null && Obj.SSOID != null)
                    WhereClause += Obj.SSOID.Length > 0 ? (WhereClause = " and SSOID ='" + Obj.SSOID + "'") : "";
                else
                    WhereClause += " and 0=1 ";

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyListNotAssignedToInspector", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "SocietyListNotAssignedToInspector(SocietyRegistrationEntity Obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int InsertSocietyProfile(SocietyRegistrationEntity _Model)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertSocietyProfile", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyName", DBFactoryParameterType.NVarchar, _Model.SocietyName);
                objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, _Model.SocietyTypeId);
                objDBAccess.AddParameterToCommand("@SocietyClassId", DBFactoryParameterType.Int, _Model.ClassId);
                objDBAccess.AddParameterToCommand("@SocietySubClassId", DBFactoryParameterType.Int, _Model.SubClassId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, _Model.ActId);
                objDBAccess.AddParameterToCommand("@RegistrationCertificateFile", DBFactoryParameterType.NVarchar, _Model.RegistrationCertificateFile);
                objDBAccess.AddParameterToCommand("@ByeLawsSocietyFile", DBFactoryParameterType.NVarchar, _Model.ByeLawsSocietyFile);
                objDBAccess.AddParameterToCommand("@RulesAndRegulationFile", DBFactoryParameterType.NVarchar, _Model.RulesAndRegulationFile);
                objDBAccess.AddParameterToCommand("@BRN", DBFactoryParameterType.NVarchar, _Model.BRN);
                objDBAccess.AddParameterToCommand("@RegistrationDate", DBFactoryParameterType.DateTime, _Model.RegistrationDate.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, _Model.RegistrationNo == "undefined" ? "" : _Model.RegistrationNo);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _Model.SSOID);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _Model.UserType);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "InsertSocietyProfile(SocietyRegistrationEntity _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetImpoDateActionList(string SocietyIdENC)
        {

            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyImpoDate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdENC", DBFactoryParameterType.NVarchar, SocietyIdENC);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetImpoDateActionList(string SocietyIdENC)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable GetAllSocietyList(string SSOID)
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyListBySSOID", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOID);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetAllSocietyList(string SSOID)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetSocityDataForOtpSending(string WhereClause)
        {

            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetContactNoOTP", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetSocityDataForOtpSending(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int insertotp(string txnid, string SocietyRegistrationNo)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertOTPuservalid", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@OTP", DBFactoryParameterType.NVarchar, txnid);
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, SocietyRegistrationNo);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "insertotp(string txnid, string SocietyRegistrationNo)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetMatchdataOPT(string WhereClause)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetMatchOTP", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetMatchdataOPT(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int UpdateSocietySSO(string SSOID, int SocietyID)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateUserSSOID", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOID);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, SocietyID);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "UpdateSocietySSO(string SSOID, int SocietyID)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetSocietyDownloadList(String SocietyIdENC)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyDownloadList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdENC", DBFactoryParameterType.NVarchar, SocietyIdENC);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetSocietyDownloadList(String SocietyIdENC)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataSet getdeshboad(SocietyRegistrationEntity objSocietyRegistrationEntity)
        {

            dt.Clear();

            DataSet dset = new DataSet();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                string WhereClause = " where 1=1 " + objSocietyRegistrationEntity.WhereClause;
                if (objSocietyRegistrationEntity.UserType == "C" || objSocietyRegistrationEntity.UserType == "K")
                {
                    if (objSocietyRegistrationEntity != null && objSocietyRegistrationEntity.SSOID != null)
                        WhereClause += objSocietyRegistrationEntity.SSOID.Length > 0 ? (WhereClause = " and SSOID ='" + objSocietyRegistrationEntity.SSOID + "'") : "";
                }

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Getdeshboarddata", DBFactoryCommandType.PROCEDURE_COMMAND);
                if (objSocietyRegistrationEntity.SSOID != "abhay.kumar")
                {
                    objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                    objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                    objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, _LoggedInUser.ActId);
                    objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _LoggedInUser.DistrictId);
                    objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.NVarchar, _LoggedInUser.DivisionId);
                    objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                    //dt = objDBAccess.GetTable();
                }
                else
                {
                    objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, objSocietyRegistrationEntity.UserId);
                    objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, objSocietyRegistrationEntity.RoleId);
                    objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, objSocietyRegistrationEntity.ActId);
                    objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, objSocietyRegistrationEntity.LevelId);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, objSocietyRegistrationEntity.DistrictId);
                    objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.NVarchar, objSocietyRegistrationEntity.DivisionId);
                    objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, "");
                    //dt = objDBAccess.GetTable();
                }
                dset = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "getdeshboad(SocietyRegistrationEntity objSocietyRegistrationEntity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dset;
        }

        public DataSet getApexdeshboad(SocietyRegistrationEntity objSocietyRegistrationEntity)
        {

            dt.Clear();

            DataSet dset = new DataSet();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetApexdeshboarddata", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, _LoggedInUser.ActId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _LoggedInUser.DistrictId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.NVarchar, _LoggedInUser.DivisionId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                dset = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "getApexdeshboad(SocietyRegistrationEntity objSocietyRegistrationEntity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dset;
        }

        public DataSet GetRajfedDashboardData(SocietyRegistrationEntity objSocietyRegistrationEntity)
        {

            dt.Clear();

            DataSet dset = new DataSet();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetRajfedDeshboardData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.NVarchar, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, _LoggedInUser.ActId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.NVarchar, _LoggedInUser.LevelId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, _LoggedInUser.DistrictId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.NVarchar, _LoggedInUser.DivisionId);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                dset = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "getApexdeshboad(SocietyRegistrationEntity objSocietyRegistrationEntity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dset;
        }
        public DataTable GetSocietyPayment(string WhereClause)
        {

            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyPayment", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetSocietyPayment(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public string UpdateOnlinePaymentStatus(string RegistrationNo, string SocietyIdEnc, string UniquerequestId, string IsFailed, string decryptData, string IsPermanentSave = "0")
        {
            int Result = -1;
            string DivHtml = "";
            try
            {
                PGResponse objPGResponse = null;
                objPGResponse = JsonConvert.DeserializeObject<PGResponse>(decryptData);

                if (objPGResponse != null)
                {
                    //UPDATE REQUEST
                    objDBAccess.CreateCommand("UpdateEmitraKioskTransaction", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("Emitraapprequestid", DBFactoryParameterType.BigInt, int.Parse(UniquerequestId));
                    objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, "");
                    objDBAccess.AddParameterToCommand("updateflag", DBFactoryParameterType.BigInt, 2);
                    objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, objPGResponse.RECEIPTNO);
                    objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, objPGResponse.TRANSACTIONID);
                    objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, objPGResponse.RESPONSEMESSAGE);
                    objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, JsonConvert.SerializeObject(objPGResponse));
                    objDBAccess.AddParameterToCommand("RegistrationNo", DBFactoryParameterType.VARCHAR, RegistrationNo);
                    objDBAccess.AddParameterToCommand("ServiceId", DBFactoryParameterType.Int, objPGResponse.SERVICEID);
                    objDBAccess.AddParameterToCommand("SocietyIdEnc", DBFactoryParameterType.VARCHAR, SocietyIdEnc);
                    objDBAccess.AddParameterToCommand("BANK_NAME", DBFactoryParameterType.VARCHAR, objPGResponse.BANK_NAME);
                    objDBAccess.AddParameterToCommand("BANK_ACC_NO", DBFactoryParameterType.VARCHAR, objPGResponse.BANK_ACC_NO);
                    objDBAccess.AddParameterToCommand("BANK_BID_NO", DBFactoryParameterType.VARCHAR, objPGResponse.BANK_BID_NO);
                    objDBAccess.AddParameterToCommand("BREFERENCE_ID", DBFactoryParameterType.VARCHAR, objPGResponse.REFERENCE_ID);
                    objDBAccess.AddParameterToCommand("IsPermanentSave", DBFactoryParameterType.VARCHAR, IsPermanentSave);
                    objDBAccess.OpenConnection();
                    object o = objDBAccess.ExecuteScalarCommand();
                    Result = Convert.ToInt32(o);
                    if (Result > 0 && objPGResponse.SERVICEID == "5530")
                    {
                        try
                        {

                            //UPDATE RESPONCE
                            objDBAccess.CreateCommand("SaveSocietyDetailsBySocietyIdFormTempTbl", DBFactoryCommandType.PROCEDURE_COMMAND);
                            objDBAccess.AddParameterToCommand("Emitraapprequestid", DBFactoryParameterType.BigInt, int.Parse(UniquerequestId));
                            objDBAccess.AddParameterToCommand("SocietyIdEnc", DBFactoryParameterType.VARCHAR, SocietyIdEnc);
                            objDBAccess.OpenConnection();
                            object oo = objDBAccess.ExecuteScalarCommand();
                            objPGResponse.INTERNAL_ID = Convert.ToInt32(oo);
                            decryptData = JsonConvert.SerializeObject(objPGResponse);
                        }
                        catch (Exception ex)
                        {
                            objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModelFactory", FunctionName = "MakePayment(string UniquerequestId, string ConsumerName, string damnt, string RegistrationNo, string SocietyIdEnc,int IsPermanentSave)" }, ex);
                        }
                        finally
                        {
                            objDBAccess.CloseConnection();
                            dt.Dispose();
                        }
                    }
                    if ((objPGResponse.SERVICEID == "5691") && Result > 0)
                    {
                        try
                        {
                            SocietyModel objSocietyModel = new SocietyModel(new DBLogger());
                            objSocietyModel.SaveBankDetailForKioskmodel(int.Parse(UniquerequestId), SocietyIdEnc);
                        }
                        catch (Exception ex)
                        {
                            objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModelFactory", FunctionName = "VerifyTransaction" }, ex);
                        }
                        finally
                        {
                            objDBAccess.CloseConnection();
                            dt.Dispose();
                        }
                    }
                    if ((objPGResponse.SERVICEID == "5531") && Result > 0)
                    {
                        try
                        {
                            ApplicationFlowModel AFM = new ApplicationFlowModel();
                            AFM.AddApplicationFlow(SocietyIdEnc, 15, "PAYMENT RECEIVED", 0);
                            AFM.SendMSG(SocietyIdEnc, 15);
                        }
                        catch (Exception ex)
                        {
                            objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EmitraModelFactory", FunctionName = "VerifyTransaction" }, ex);
                        }
                        finally
                        {
                            objDBAccess.CloseConnection();
                            dt.Dispose();
                        }
                    }
                    //if (i > 0 && ObjSocietyRegistration.ServiceId == 4173)
                    //{
                    //    ApplicationFlowModel AFM = new ApplicationFlowModel();
                    //    AFM.SendMSG(ObjSocietyRegistration.SocietyIDEnc, 24);
                    //    returnval = JsonConvert.SerializeObject(resp);
                    //}
                    DivHtml = decryptData;
                    //DivHtml += "<table class=\"table table-responsive devtable\"><thead><tr><th>Receipt Number</<th><th>Status</<th> </tr></ thead >";
                    //DivHtml += "<tbody><tr><td>" + objPGResponse.RECEIPTNO + "</td><td>" + objPGResponse.RESPONSEMESSAGE + "</td>";
                    //DivHtml += "</ tr ></ tbody ></ table >";

                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "UpdateOnlinePaymentStatus(string RegistrationNo, string SocietyIdEnc, string UniquerequestId, string IsFailed, string decryptData)" }, ex);
            }
            finally
            {
                //objDBAccess.CloseConnection();
            }
            return DivHtml;
        }

        public string UpdateOnlinePaymentStatusAll(string RegistrationNo, string SocietyId, string UniquerequestId, string IsFailed, string decryptData)
        {
            int Result = -1;
            string DivHtml = "";
            try
            {
                PGResponse objPGResponse = null;
                objPGResponse = JsonConvert.DeserializeObject<PGResponse>(decryptData);

                if (objPGResponse != null)
                {
                    //UPDATE REQUEST
                    objDBAccess.CreateCommand("UpdateEmitraKioskTransactionForAll", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("Emitraapprequestid", DBFactoryParameterType.BigInt, int.Parse(UniquerequestId));
                    objDBAccess.AddParameterToCommand("RequestString", DBFactoryParameterType.VARCHAR, "");
                    objDBAccess.AddParameterToCommand("updateflag", DBFactoryParameterType.BigInt, 2);
                    objDBAccess.AddParameterToCommand("ReceiptNo", DBFactoryParameterType.VARCHAR, objPGResponse.RECEIPTNO);
                    objDBAccess.AddParameterToCommand("TokenNo", DBFactoryParameterType.VARCHAR, objPGResponse.TRANSACTIONID);
                    objDBAccess.AddParameterToCommand("StatusMsg", DBFactoryParameterType.VARCHAR, objPGResponse.RESPONSEMESSAGE);
                    objDBAccess.AddParameterToCommand("ResponseString", DBFactoryParameterType.VARCHAR, JsonConvert.SerializeObject(objPGResponse));
                    objDBAccess.AddParameterToCommand("RegistrationNo", DBFactoryParameterType.VARCHAR, RegistrationNo);
                    objDBAccess.AddParameterToCommand("ServiceId", DBFactoryParameterType.Int, objPGResponse.SERVICEID);
                    objDBAccess.AddParameterToCommand("SocietyId", DBFactoryParameterType.Int, SocietyId);
                    objDBAccess.AddParameterToCommand("BANK_NAME", DBFactoryParameterType.VARCHAR, objPGResponse.BANK_NAME);
                    objDBAccess.AddParameterToCommand("BANK_ACC_NO", DBFactoryParameterType.VARCHAR, objPGResponse.BANK_ACC_NO);
                    objDBAccess.AddParameterToCommand("BANK_BID_NO", DBFactoryParameterType.VARCHAR, objPGResponse.BANK_BID_NO);
                    objDBAccess.AddParameterToCommand("REFERENCE_ID", DBFactoryParameterType.VARCHAR, objPGResponse.REFERENCE_ID);
                    objDBAccess.OpenConnection();
                    object o = objDBAccess.ExecuteScalarCommand();
                    Result = Convert.ToInt32(o);
                    DivHtml = decryptData;
                    //if (objPGResponse.RESPONSEMESSAGE.ToString().Contains("Successfully"))
                    //{

                    //}
                    //else
                    //{
                    //    DivHtml += "<table class=\"table table-responsive devtable\"><thead><tr><th>Receipt Number</<th><th>Status</<th> </tr></ thead >";
                    //    DivHtml += "<tbody><tr><td>" + objPGResponse.RECEIPTNO + "</td><td>" + objPGResponse.RESPONSEMESSAGE + "</td>";
                    //    DivHtml += "</ tr ></ tbody ></ table >";
                    //}

                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "UpdateOnlinePaymentStatus(string RegistrationNo, string SocietyIdEnc, string UniquerequestId, string IsFailed, string decryptData)" }, ex);
            }
            finally
            {
                //objDBAccess.CloseConnection();
            }
            return DivHtml;
        }

        public DataTable GetFailedTransactionList(SocietyRegistrationEntity Obj, string GetFailedTransactionList)
        {
            dt.Clear();
            try
            {
                string WhereClause = " and 1=1 " + Obj.WhereClause;
                if (Obj.UserType == "C")//|| Obj.UserType == "K"
                {
                    if (Obj != null && Obj.SSOID != null)
                        WhereClause += Obj.SSOID.Length > 0 ? (WhereClause = " and Societies.SSOID ='" + Obj.SSOID + "'") : "";
                }
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("TransactionForVerificationList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                objDBAccess.AddParameterToCommand("@SocietyIDEnc", DBFactoryParameterType.NVarchar, Obj.SocietyIDEnc);
                objDBAccess.AddParameterToCommand("@SocietyRegistrationNo", DBFactoryParameterType.NVarchar, Obj.RegistrationNo);
                objDBAccess.AddParameterToCommand("@ID", DBFactoryParameterType.Int, int.Parse(GetFailedTransactionList));
                objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, Obj.UserType == "C" ? "0" : "1");

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetFailedTransactionList(SocietyRegistrationEntity Obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        #region Court 

        public int AddCaseRegistraion(CaseRegistraion Obj)
        {
            int Result = 0;
            try
            {
                DateTime dt = new DateTime();
                dt = DateTime.Now;
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertCaseRegistration", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ID", DBFactoryParameterType.Int, Obj.CaseRegID);
                objDBAccess.AddParameterToCommand("@Court", DBFactoryParameterType.Int, Obj.Court);
                objDBAccess.AddParameterToCommand("@Act", DBFactoryParameterType.Int, Obj.Act);
                objDBAccess.AddParameterToCommand("@Section", DBFactoryParameterType.Int, Obj.section);
                objDBAccess.AddParameterToCommand("@Case", DBFactoryParameterType.Int, Obj.Case);
                objDBAccess.AddParameterToCommand("@TypeOfCase", DBFactoryParameterType.Int, Obj.TypeOfCase);
                objDBAccess.AddParameterToCommand("@DateOfAffiliation", DBFactoryParameterType.DateTime, dt);
                objDBAccess.AddParameterToCommand("@CaseDescription", DBFactoryParameterType.NVarchar, Obj.CaseDescription);
                objDBAccess.AddParameterToCommand("@isruralurban", DBFactoryParameterType.Int, Obj.isruralurban);
                objDBAccess.AddParameterToCommand("@districtid", DBFactoryParameterType.Int, Obj.districtid);
                objDBAccess.AddParameterToCommand("@citytypeid", DBFactoryParameterType.Int, Obj.citytypeid);
                objDBAccess.AddParameterToCommand("@cityblockid", DBFactoryParameterType.Int, Obj.cityblockid);
                objDBAccess.AddParameterToCommand("@wardgpid", DBFactoryParameterType.Int, Obj.wardgpid);
                objDBAccess.AddParameterToCommand("@villageid", DBFactoryParameterType.Int, Obj.villageid);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, Obj.SSOID);
                objDBAccess.AddParameterToCommand("@Userid", DBFactoryParameterType.Int, Obj.UserId);
                objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, Obj.UserType);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "AddCaseRegistraion(CaseRegistraion Obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int AddCaseDetails(CaseDetails Obj)
        {
            int Result = 0;
            try
            {
                foreach (AppliantScopeEntity objapp in Obj.lstAppliantScope)
                {
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("InsertCaseDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@CaseRegID", DBFactoryParameterType.NVarchar, objapp.CaseRegID);
                    objDBAccess.AddParameterToCommand("@AppellantName", DBFactoryParameterType.NVarchar, objapp.AppellantName);
                    objDBAccess.AddParameterToCommand("@FatherHasbandName", DBFactoryParameterType.NVarchar, objapp.FatherHasbandName);
                    objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, objapp.MobileNo);
                    objDBAccess.AddParameterToCommand("@EmailID", DBFactoryParameterType.NVarchar, objapp.EmailID);
                    objDBAccess.AddParameterToCommand("@AddharID", DBFactoryParameterType.NVarchar, objapp.AddharID);
                    objDBAccess.AddParameterToCommand("@Gender", DBFactoryParameterType.Int, objapp.GenderID);
                    objDBAccess.AddParameterToCommand("@Category", DBFactoryParameterType.Int, objapp.CategoryID);
                    objDBAccess.AddParameterToCommand("@AppellantType", DBFactoryParameterType.NVarchar, objapp.AppellantTypeId);
                    objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, objapp.Address);
                    objDBAccess.AddParameterToCommand("@State", DBFactoryParameterType.NVarchar, objapp.State);
                    objDBAccess.AddParameterToCommand("@District", DBFactoryParameterType.NVarchar, objapp.DistrictID);
                    objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, 1);
                    objDBAccess.AddParameterToCommand("@IsPrimery", DBFactoryParameterType.Int, objapp.IsPrimeryID);
                    objDBAccess.AddParameterToCommand("@flag", DBFactoryParameterType.Int, objapp.flag);
                    objDBAccess.AddParameterToCommand("@Isdelete", DBFactoryParameterType.Int, Result);
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "AddCaseDetails(CaseDetails Obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int AddRespCaseDetails(CaseDetails Obj)
        {
            int Result = 0;
            try
            {
                foreach (RespondScopeEntity objapp in Obj.lstRespondScope)
                {
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("InsertCaseDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@CaseRegID", DBFactoryParameterType.NVarchar, objapp.CaseRegID);
                    objDBAccess.AddParameterToCommand("@AppellantName", DBFactoryParameterType.NVarchar, objapp.AppellantName);
                    objDBAccess.AddParameterToCommand("@FatherHasbandName", DBFactoryParameterType.NVarchar, objapp.FatherHasbandName);
                    objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, objapp.MobileNo);
                    objDBAccess.AddParameterToCommand("@EmailID", DBFactoryParameterType.NVarchar, objapp.EmailID);
                    objDBAccess.AddParameterToCommand("@AddharID", DBFactoryParameterType.NVarchar, objapp.AddharID);
                    objDBAccess.AddParameterToCommand("@Gender", DBFactoryParameterType.Int, objapp.GenderID);
                    objDBAccess.AddParameterToCommand("@Category", DBFactoryParameterType.Int, objapp.CategoryID);
                    objDBAccess.AddParameterToCommand("@AppellantType", DBFactoryParameterType.NVarchar, objapp.AppellantTypeId);
                    objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, objapp.Address);
                    objDBAccess.AddParameterToCommand("@State", DBFactoryParameterType.NVarchar, objapp.State);
                    objDBAccess.AddParameterToCommand("@District", DBFactoryParameterType.NVarchar, objapp.DistrictID);
                    objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, 1);
                    objDBAccess.AddParameterToCommand("@IsPrimery", DBFactoryParameterType.Int, objapp.IsPrimeryID);
                    objDBAccess.AddParameterToCommand("@flag", DBFactoryParameterType.Int, objapp.flag);
                    objDBAccess.AddParameterToCommand("@Isdelete", DBFactoryParameterType.Int, Result);
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "AddRespCaseDetails(CaseDetails Obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int DeleteEnclouser(int caseid)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteEnclouser", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CaseRegID", DBFactoryParameterType.BigInt, caseid);

                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "DeleteEnclouser(int caseid)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public int DeleteOIC(int caseid)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteOicDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CaseRegID", DBFactoryParameterType.BigInt, caseid);

                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "DeleteOIC(int caseid)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int DeleteAdvocateDetail(int caseid)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteAdvocateDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CaseRegID", DBFactoryParameterType.BigInt, caseid);

                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "DeleteAdvocateDetail(int caseid)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public int GetCaseRegistrationAndUpdate(string caseid)
        {
            DataSet dset = null;
            DataTable dtcase = null;
            DataTable dtencl = null;
            DataTable dtappl = null;
            DataTable dtresp = null;
            DataTable dtadv = null;
            DataTable dtoic = null;
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                string WhereClause = "creg.ID = " + caseid + "";
                SqlCommand cmd = objDBAccess.CreateCommand("GetCaseDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dset = objDBAccess.GetDataSet();
                if (dset.Tables.Count > 0 && dset != null)
                {
                    string Str = "";
                    string StrCase = "";
                    dtcase = dset.Tables[0];
                    dtencl = dset.Tables[3];
                    dtappl = dset.Tables[1];
                    dtresp = dset.Tables[2];
                    dtadv = dset.Tables[4];
                    dtoic = dset.Tables[5];
                    if (dtcase != null && dtcase.Rows.Count > 0)
                    {
                        int index = 0;
                        Str = "{\"_parameters\":[{\"savedata\":{\"axpapp\":\"gcms\",\"username\":\"mobileuser\",\"password\":\"827ccb0eea8a706c4c34a16891f84e7b\",\"seed\":\"\",\"s\":\"\",\"transid\":\"tscpr\",\"recordid\":\"0\",\"recdata\":[";
                        Str += "{\"axp_recid1\":[{ \"rowno\":\"001\",\"text\":\"0\",\"columns\":";
                        StrCase = (COMMON_FUNCTION.DataTableToJSONString(dtcase)).Replace("[", "");
                        StrCase = StrCase.Replace("]", "");
                        //DataSet ONE
                        Str += StrCase + "}]}";
                        //DataSet Doc
                        Str += ",{\"axp_recid2\":[";
                        for (int i = 0; i < dtencl.Rows.Count; i++)
                        {
                            if (dtencl.Rows.Count > 1)
                            {
                                Str += "{\"rowno\":\"00" + (i + 1) + "\",\"text\":\"0\",\"columns\":{\"doc_name\":\"" + dtencl.Rows[i]["doc_name"].ToString() + "\",\"doc_type\":\"" + dtencl.Rows[i]["doc_type"].ToString() + "\",\"docbase64\":\"" + dtencl.Rows[i]["docbase64"].ToString() + "\"}},";
                            }
                            else
                            {
                                Str += "{\"rowno\":\"00" + (i + 1) + "\",\"text\":\"0\",\"columns\":{\"doc_name\":\"" + dtencl.Rows[i]["doc_name"].ToString() + "\",\"doc_type\":\"" + dtencl.Rows[i]["doc_type"].ToString() + "\",\"docbase64\":\"" + dtencl.Rows[i]["docbase64"].ToString() + "\"}}";
                            }
                        }

                        index = Str.LastIndexOf(',');
                        Str = Str.Remove(index, 1);
                        Str += "]}";
                        //DataSet Appliant
                        Str += ",{\"axp_recid3\":[";
                        for (int i = 0; i < dtappl.Rows.Count; i++)
                        {
                            if (dtappl.Rows.Count > 1)
                            {
                                Str += "{\"rowno\":\"00" + (i + 1) + "\",\"text\":\"0\",\"columns\":{\"appellant_name\":\"" + dtappl.Rows[i]["appellant_name"].ToString() + "\",\"father_husband_name\":\"" + dtappl.Rows[i]["father_husband_name"].ToString() + "\",\"phone1\":\"" + dtappl.Rows[i]["phone1"].ToString() + "\",\"appellant_email\":\"" + dtappl.Rows[i]["appellant_email"].ToString() + "\",\"appellant_otherid\":\"" + dtappl.Rows[i]["appellant_otherid"].ToString() + "\",\"appellant_gender\":\"" + dtappl.Rows[i]["appellant_gender"].ToString() + "\",\"acategory\":\"" + dtappl.Rows[i]["acategory"].ToString() + "\",\"appellant_type\":\"" + dtappl.Rows[i]["appellant_type"].ToString() + "\",\"appellant_address\":\"" + dtappl.Rows[i]["appellant_address"].ToString() + "\",\"astate_name\":\"" + dtappl.Rows[i]["astate_name"].ToString() + "\",\"adistrict_name\":\"" + dtappl.Rows[i]["adistrict_name"].ToString() + "\",\"aprimarycheck\":\"" + dtappl.Rows[i]["aprimarycheck"].ToString() + "\",\"app_isactive\":\"" + dtappl.Rows[i]["app_isactive"].ToString() + "\"}},";
                            }
                            else
                            {
                                Str += "{\"rowno\":\"00" + (i + 1) + "\",\"text\":\"0\",\"columns\":{\"appellant_name\":\"" + dtappl.Rows[i]["appellant_name"].ToString() + "\",\"father_husband_name\":\"" + dtappl.Rows[i]["father_husband_name"].ToString() + "\",\"phone1\":\"" + dtappl.Rows[i]["phone1"].ToString() + "\",\"appellant_email\":\"" + dtappl.Rows[i]["appellant_email"].ToString() + "\",\"appellant_otherid\":\"" + dtappl.Rows[i]["appellant_otherid"].ToString() + "\",\"appellant_gender\":\"" + dtappl.Rows[i]["appellant_gender"].ToString() + "\",\"acategory\":\"" + dtappl.Rows[i]["acategory"].ToString() + "\",\"appellant_type\":\"" + dtappl.Rows[i]["appellant_type"].ToString() + "\",\"appellant_address\":\"" + dtappl.Rows[i]["appellant_address"].ToString() + "\",\"astate_name\":\"" + dtappl.Rows[i]["astate_name"].ToString() + "\",\"adistrict_name\":\"" + dtappl.Rows[i]["adistrict_name"].ToString() + "\",\"aprimarycheck\":\"" + dtappl.Rows[i]["aprimarycheck"].ToString() + "\",\"app_isactive\":\"" + dtappl.Rows[i]["app_isactive"].ToString() + "\"}}";
                            }
                        }
                        index = Str.LastIndexOf(',');
                        Str = Str.Remove(index, 1);
                        Str += "]}";
                        //DataSet Respondent
                        Str += ",{\"axp_recid4\":[";
                        for (int i = 0; i < dtresp.Rows.Count; i++)
                        {
                            if (dtresp.Rows.Count > 1)
                            {
                                Str += "{\"rowno\":\"00" + (i + 1) + "\",\"text\":\"0\",\"columns\":{\"respondent_name\":\"" + dtresp.Rows[i]["respondent_name"].ToString() + "\",\"rfather_husband_name\":\"" + dtresp.Rows[i]["rfather_husband_name"].ToString() + "\",\"respondent_phone1\":\"" + dtresp.Rows[i]["respondent_phone1"].ToString() + "\",\"respondent_email\":\"" + dtresp.Rows[i]["respondent_email"].ToString() + "\",\"respondent_otherid\":\"" + dtresp.Rows[i]["respondent_otherid"].ToString() + "\",\"respondent_gender\":\"" + dtresp.Rows[i]["respondent_gender"].ToString() + "\",\"rcategory\":\"" + dtresp.Rows[i]["rcategory"].ToString() + "\",\"respondent_type\":\"" + dtresp.Rows[i]["respondent_type"].ToString() + "\",\"respondent_address\":\"" + dtresp.Rows[i]["respondent_address"].ToString() + "\",\"rstate_name\":\"" + dtresp.Rows[i]["rstate_name"].ToString() + "\",\"rdistrict_name\":\"" + dtresp.Rows[i]["rdistrict_name"].ToString() + "\",\"rprimarycheck\":\"" + dtresp.Rows[i]["rprimarycheck"].ToString() + "\",\"res_isactive\":\"" + dtresp.Rows[i]["res_isactive"].ToString() + "\"}},";
                            }
                            else
                            {
                                Str += "{\"rowno\":\"00" + (i + 1) + "\",\"text\":\"0\",\"columns\":{\"respondent_name\":\"" + dtresp.Rows[i]["respondent_name"].ToString() + "\",\"rfather_husband_name\":\"" + dtresp.Rows[i]["rfather_husband_name"].ToString() + "\",\"respondent_phone1\":\"" + dtresp.Rows[i]["respondent_phone1"].ToString() + "\",\"respondent_email\":\"" + dtresp.Rows[i]["respondent_email"].ToString() + "\",\"respondent_otherid\":\"" + dtresp.Rows[i]["respondent_otherid"].ToString() + "\",\"respondent_gender\":\"" + dtresp.Rows[i]["respondent_gender"].ToString() + "\",\"rcategory\":\"" + dtresp.Rows[i]["rcategory"].ToString() + "\",\"respondent_type\":\"" + dtresp.Rows[i]["respondent_type"].ToString() + "\",\"respondent_address\":\"" + dtresp.Rows[i]["respondent_address"].ToString() + "\",\"rstate_name\":\"" + dtresp.Rows[i]["rstate_name"].ToString() + "\",\"rdistrict_name\":\"" + dtresp.Rows[i]["rdistrict_name"].ToString() + "\",\"rprimarycheck\":\"" + dtresp.Rows[i]["rprimarycheck"].ToString() + "\",\"res_isactive\":\"" + dtresp.Rows[i]["res_isactive"].ToString() + "\"}}";
                            }
                        }
                        index = Str.LastIndexOf(',');
                        Str = Str.Remove(index, 1);
                        Str += "]}";
                        //DataSet Advocate
                        Str += ",{\"axp_recid5\":[";
                        for (int i = 0; i < dtadv.Rows.Count; i++)
                        {
                            if (dtadv.Rows.Count > 1)
                            {
                                Str += "{\"rowno\":\"00" + (i + 1) + "\",\"text\":\"0\",\"columns\":{\"advocate_name\":\"" + dtadv.Rows[i]["advocate_name"].ToString() + "\",\"advocate_type\":\"" + dtadv.Rows[i]["advocate_type"].ToString() + "\",\"advcontactcr\":\"" + dtadv.Rows[i]["advcontactcr"].ToString() + "\",\"primarycheck\":\"" + dtadv.Rows[i]["primarycheck"].ToString() + "\",\"adv_isactive\":\"" + dtadv.Rows[i]["adv_isactive"].ToString() + "\"}},";
                            }
                            else
                            {
                                Str += "{\"rowno\":\"00" + (i + 1) + "\",\"text\":\"0\",\"columns\":{\"advocate_name\":\"" + dtadv.Rows[i]["advocate_name"].ToString() + "\",\"advocate_type\":\"" + dtadv.Rows[i]["advocate_type"].ToString() + "\",\"advcontactcr\":\"" + dtadv.Rows[i]["advcontactcr"].ToString() + "\",\"primarycheck\":\"" + dtadv.Rows[i]["primarycheck"].ToString() + "\",\"adv_isactive\":\"" + dtadv.Rows[i]["adv_isactive"].ToString() + "\"}}";
                            }
                        }
                        index = Str.LastIndexOf(',');
                        Str = Str.Remove(index, 1);
                        Str += "]}";
                        //DataSet Oic
                        Str += ",{\"axp_recid7\":[";
                        for (int i = 0; i < dtoic.Rows.Count; i++)
                        {
                            if (dtoic.Rows.Count > 1)
                            {
                                Str += "{\"rowno\":\"00" + (i + 1) + "\",\"text\":\"0\",\"columns\":{\"oicname\":\"" + dtoic.Rows[i]["OICName"].ToString() + "\",\"oictype\":\"" + dtoic.Rows[i]["OICType"].ToString() + "\",\"oicmobno\":\"" + dtoic.Rows[i]["OICMobileNo"].ToString() + "\"}},";
                            }
                            else
                            {
                                Str += "{\"rowno\":\"00" + (i + 1) + "\",\"text\":\"0\",\"columns\":{\"oicname\":\"" + dtoic.Rows[i]["OICName"].ToString() + "\",\"oictype\":\"" + dtoic.Rows[i]["OICType"].ToString() + "\",\"oicmobno\":\"" + dtoic.Rows[i]["OICMobileNo"].ToString() + "\"}}";
                            }
                        }
                        index = Str.LastIndexOf(',');
                        Str = Str.Remove(index, 1);
                        Str += "]}]}}]}";

                        string result;
                        string recordid;
                        //  var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://192.168.10.74/gcmsscripts//ASBTStructRest.dll/datasnap/rest/TASBTStruct/savedata");
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://10.70.230.61/gcmsscripts//ASBTStructRest.dll/datasnap/rest/TASBTStruct/savedata");

                        cmd = objDBAccess.CreateCommand("UpdateCaseJson", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@CaseID", DBFactoryParameterType.Int, Convert.ToInt32(caseid));
                        objDBAccess.AddParameterToCommand("@casejson", DBFactoryParameterType.NVarchar, Str.ToString());
                        Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";
                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string json = Str;
                            streamWriter.Write(json);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            result = streamReader.ReadToEnd();
                            var temp = result;
                            JsonCourt obj = new JsonCourt();
                            obj = (JsonCourt)s.Deserialize(result, typeof(JsonCourt));
                            result = obj.result[0].message[0].PortalRegNo.ToString();
                            recordid = obj.result[0].message[0].recordid.ToString();
                        }

                        cmd = objDBAccess.CreateCommand("UpdateCaseRegno", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@CaseID", DBFactoryParameterType.Int, Convert.ToInt32(caseid));
                        objDBAccess.AddParameterToCommand("@caseregno", DBFactoryParameterType.NVarchar, result.ToString());
                        objDBAccess.AddParameterToCommand("@recordid", DBFactoryParameterType.NVarchar, recordid.ToString());
                        Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetCaseRegistrationAndUpdate(string caseid)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public int AddOICDetails(OICDetails Obj)
        {
            DataSet dset = null;
            DataTable dtcase = null;
            DataTable dtencl = null;
            DataTable dtappl = null;
            DataTable dtresp = null;
            DataTable dtadv = null;
            DataTable dtoic = null;
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertOICDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CaseRegID", DBFactoryParameterType.NVarchar, Obj.CaseRegID);
                objDBAccess.AddParameterToCommand("@OICName", DBFactoryParameterType.NVarchar, Obj.OICName);
                objDBAccess.AddParameterToCommand("@OICType", DBFactoryParameterType.NVarchar, Obj.OICType);
                objDBAccess.AddParameterToCommand("@OICMobileNo", DBFactoryParameterType.NVarchar, Obj.OICMobileNo);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                if (Result > 0)
                {
                    string WhereClause = "creg.ID = " + Obj.CaseRegID + "";
                    cmd = objDBAccess.CreateCommand("GetCaseDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                    dset = objDBAccess.GetDataSet();
                    if (dset.Tables.Count > 0 && dset != null)
                    {
                        string Str = "";
                        string StrCase = "";
                        dtcase = dset.Tables[0];
                        dtencl = dset.Tables[3];
                        dtappl = dset.Tables[1];
                        dtresp = dset.Tables[2];
                        dtadv = dset.Tables[4];
                        dtoic = dset.Tables[5];
                        if (dtcase != null && dtcase.Rows.Count > 0)
                        {
                            int index = 0;
                            Str = "{\"_parameters\":[{\"savedata\":{\"axpapp\":\"gcms\",\"username\":\"mobileuser\",\"password\":\"827ccb0eea8a706c4c34a16891f84e7b\",\"seed\":\"\",\"s\":\"\",\"transid\":\"tscpr\",\"recordid\":\"0\",\"recdata\":[";
                            Str += "{\"axp_recid1\":[{ \"rowno\":\"001\",\"text\":\"0\",\"columns\":";
                            StrCase = (COMMON_FUNCTION.DataTableToJSONString(dtcase)).Replace("[", "");
                            StrCase = StrCase.Replace("]", "");
                            //DataSet ONE
                            Str += StrCase + "}]}";
                            //StrDoc = (COMMON_FUNCTION.DataTableToJSONString(dtencl)).Replace("[", "");
                            //StrDoc = StrDoc.Replace("]", "");
                            //StrApp = (COMMON_FUNCTION.DataTableToJSONString(dtappl)).Replace("[", "");
                            //StrApp = StrApp.Replace("]", "");
                            //StrRes = (COMMON_FUNCTION.DataTableToJSONString(dtresp)).Replace("[", "");
                            //StrRes = StrRes.Replace("]", "");
                            //StrAdv = (COMMON_FUNCTION.DataTableToJSONString(dtadv)).Replace("[", "");
                            //StrAdv = StrAdv.Replace("]", "");
                            //DataSet Doc
                            Str += ",{\"axp_recid2\":[";
                            for (int i = 0; i < dtencl.Rows.Count; i++)
                            {
                                if (dtencl.Rows.Count > 1)
                                {
                                    Str += "{\"rowno\":\"00" + (i + 1) + "\",\"text\":\"0\",\"columns\":{\"doc_name\":\"" + dtencl.Rows[i]["doc_name"].ToString() + "\",\"doc_type\":\"" + dtencl.Rows[i]["doc_type"].ToString() + "\",\"docbase64\":\"" + dtencl.Rows[i]["docbase64"].ToString() + "\"}},";
                                }
                                else
                                {
                                    Str += "{\"rowno\":\"00" + (i + 1) + "\",\"text\":\"0\",\"columns\":{\"doc_name\":\"" + dtencl.Rows[i]["doc_name"].ToString() + "\",\"doc_type\":\"" + dtencl.Rows[i]["doc_type"].ToString() + "\",\"docbase64\":\"" + dtencl.Rows[i]["docbase64"].ToString() + "\"}}";
                                }
                            }

                            index = Str.LastIndexOf(',');
                            Str = Str.Remove(index, 1);
                            Str += "]}";
                            //DataSet Appliant
                            Str += ",{\"axp_recid3\":[";
                            for (int i = 0; i < dtappl.Rows.Count; i++)
                            {
                                if (dtappl.Rows.Count > 1)
                                {
                                    Str += "{\"rowno\":\"00" + (i + 1) + "\",\"text\":\"0\",\"columns\":{\"appellant_name\":\"" + dtappl.Rows[i]["appellant_name"].ToString() + "\",\"father_husband_name\":\"" + dtappl.Rows[i]["father_husband_name"].ToString() + "\",\"phone1\":\"" + dtappl.Rows[i]["phone1"].ToString() + "\",\"appellant_email\":\"" + dtappl.Rows[i]["appellant_email"].ToString() + "\",\"appellant_otherid\":\"" + dtappl.Rows[i]["appellant_otherid"].ToString() + "\",\"appellant_gender\":\"" + dtappl.Rows[i]["appellant_gender"].ToString() + "\",\"acategory\":\"" + dtappl.Rows[i]["acategory"].ToString() + "\",\"appellant_type\":\"" + dtappl.Rows[i]["appellant_type"].ToString() + "\",\"appellant_address\":\"" + dtappl.Rows[i]["appellant_address"].ToString() + "\",\"astate_name\":\"" + dtappl.Rows[i]["astate_name"].ToString() + "\",\"adistrict_name\":\"" + dtappl.Rows[i]["adistrict_name"].ToString() + "\",\"aprimarycheck\":\"" + dtappl.Rows[i]["aprimarycheck"].ToString() + "\",\"app_isactive\":\"" + dtappl.Rows[i]["app_isactive"].ToString() + "\"}},";
                                }
                                else
                                {
                                    Str += "{\"rowno\":\"00" + (i + 1) + "\",\"text\":\"0\",\"columns\":{\"appellant_name\":\"" + dtappl.Rows[i]["appellant_name"].ToString() + "\",\"father_husband_name\":\"" + dtappl.Rows[i]["father_husband_name"].ToString() + "\",\"phone1\":\"" + dtappl.Rows[i]["phone1"].ToString() + "\",\"appellant_email\":\"" + dtappl.Rows[i]["appellant_email"].ToString() + "\",\"appellant_otherid\":\"" + dtappl.Rows[i]["appellant_otherid"].ToString() + "\",\"appellant_gender\":\"" + dtappl.Rows[i]["appellant_gender"].ToString() + "\",\"acategory\":\"" + dtappl.Rows[i]["acategory"].ToString() + "\",\"appellant_type\":\"" + dtappl.Rows[i]["appellant_type"].ToString() + "\",\"appellant_address\":\"" + dtappl.Rows[i]["appellant_address"].ToString() + "\",\"astate_name\":\"" + dtappl.Rows[i]["astate_name"].ToString() + "\",\"adistrict_name\":\"" + dtappl.Rows[i]["adistrict_name"].ToString() + "\",\"aprimarycheck\":\"" + dtappl.Rows[i]["aprimarycheck"].ToString() + "\",\"app_isactive\":\"" + dtappl.Rows[i]["app_isactive"].ToString() + "\"}}";
                                }
                            }
                            index = Str.LastIndexOf(',');
                            Str = Str.Remove(index, 1);
                            Str += "]}";
                            //DataSet Respondent
                            Str += ",{\"axp_recid4\":[";
                            for (int i = 0; i < dtresp.Rows.Count; i++)
                            {
                                if (dtresp.Rows.Count > 1)
                                {
                                    Str += "{\"rowno\":\"00" + (i + 1) + "\",\"text\":\"0\",\"columns\":{\"respondent_name\":\"" + dtresp.Rows[i]["respondent_name"].ToString() + "\",\"rfather_husband_name\":\"" + dtresp.Rows[i]["rfather_husband_name"].ToString() + "\",\"respondent_phone1\":\"" + dtresp.Rows[i]["respondent_phone1"].ToString() + "\",\"respondent_email\":\"" + dtresp.Rows[i]["respondent_email"].ToString() + "\",\"respondent_otherid\":\"" + dtresp.Rows[i]["respondent_otherid"].ToString() + "\",\"respondent_gender\":\"" + dtresp.Rows[i]["respondent_gender"].ToString() + "\",\"rcategory\":\"" + dtresp.Rows[i]["rcategory"].ToString() + "\",\"respondent_type\":\"" + dtresp.Rows[i]["respondent_type"].ToString() + "\",\"respondent_address\":\"" + dtresp.Rows[i]["respondent_address"].ToString() + "\",\"rstate_name\":\"" + dtresp.Rows[i]["rstate_name"].ToString() + "\",\"rdistrict_name\":\"" + dtresp.Rows[i]["rdistrict_name"].ToString() + "\",\"rprimarycheck\":\"" + dtresp.Rows[i]["rprimarycheck"].ToString() + "\",\"res_isactive\":\"" + dtresp.Rows[i]["res_isactive"].ToString() + "\"}},";
                                }
                                else
                                {
                                    Str += "{\"rowno\":\"00" + (i + 1) + "\",\"text\":\"0\",\"columns\":{\"respondent_name\":\"" + dtresp.Rows[i]["respondent_name"].ToString() + "\",\"rfather_husband_name\":\"" + dtresp.Rows[i]["rfather_husband_name"].ToString() + "\",\"respondent_phone1\":\"" + dtresp.Rows[i]["respondent_phone1"].ToString() + "\",\"respondent_email\":\"" + dtresp.Rows[i]["respondent_email"].ToString() + "\",\"respondent_otherid\":\"" + dtresp.Rows[i]["respondent_otherid"].ToString() + "\",\"respondent_gender\":\"" + dtresp.Rows[i]["respondent_gender"].ToString() + "\",\"rcategory\":\"" + dtresp.Rows[i]["rcategory"].ToString() + "\",\"respondent_type\":\"" + dtresp.Rows[i]["respondent_type"].ToString() + "\",\"respondent_address\":\"" + dtresp.Rows[i]["respondent_address"].ToString() + "\",\"rstate_name\":\"" + dtresp.Rows[i]["rstate_name"].ToString() + "\",\"rdistrict_name\":\"" + dtresp.Rows[i]["rdistrict_name"].ToString() + "\",\"rprimarycheck\":\"" + dtresp.Rows[i]["rprimarycheck"].ToString() + "\",\"res_isactive\":\"" + dtresp.Rows[i]["res_isactive"].ToString() + "\"}}";
                                }
                            }
                            index = Str.LastIndexOf(',');
                            Str = Str.Remove(index, 1);
                            Str += "]}";
                            //DataSet Advocate
                            Str += ",{\"axp_recid5\":[";
                            for (int i = 0; i < dtadv.Rows.Count; i++)
                            {
                                if (dtadv.Rows.Count > 1)
                                {
                                    Str += "{\"rowno\":\"00" + (i + 1) + "\",\"text\":\"0\",\"columns\":{\"advocate_name\":\"" + dtadv.Rows[i]["advocate_name"].ToString() + "\",\"advocate_type\":\"" + dtadv.Rows[i]["advocate_type"].ToString() + "\",\"advcontactcr\":\"" + dtadv.Rows[i]["advcontactcr"].ToString() + "\",\"primarycheck\":\"" + dtadv.Rows[i]["primarycheck"].ToString() + "\",\"adv_isactive\":\"" + dtadv.Rows[i]["adv_isactive"].ToString() + "\"}},";
                                }
                                else
                                {
                                    Str += "{\"rowno\":\"00" + (i + 1) + "\",\"text\":\"0\",\"columns\":{\"advocate_name\":\"" + dtadv.Rows[i]["advocate_name"].ToString() + "\",\"advocate_type\":\"" + dtadv.Rows[i]["advocate_type"].ToString() + "\",\"advcontactcr\":\"" + dtadv.Rows[i]["advcontactcr"].ToString() + "\",\"primarycheck\":\"" + dtadv.Rows[i]["primarycheck"].ToString() + "\",\"adv_isactive\":\"" + dtadv.Rows[i]["adv_isactive"].ToString() + "\"}}";
                                }
                            }
                            index = Str.LastIndexOf(',');
                            Str = Str.Remove(index, 1);
                            Str += "]}";
                            //DataSet Oic
                            Str += ",{\"axp_recid7\":[";
                            for (int i = 0; i < dtoic.Rows.Count; i++)
                            {
                                if (dtoic.Rows.Count > 1)
                                {
                                    Str += "{\"rowno\":\"00" + (i + 1) + "\",\"text\":\"0\",\"columns\":{\"oicname\":\"" + dtoic.Rows[i]["OICName"].ToString() + "\",\"oictype\":\"" + dtoic.Rows[i]["OICType"].ToString() + "\",\"oicmobno\":\"" + dtoic.Rows[i]["OICMobileNo"].ToString() + "\"}},";
                                }
                                else
                                {
                                    Str += "{\"rowno\":\"00" + (i + 1) + "\",\"text\":\"0\",\"columns\":{\"oicname\":\"" + dtoic.Rows[i]["OICName"].ToString() + "\",\"oictype\":\"" + dtoic.Rows[i]["OICType"].ToString() + "\",\"oicmobno\":\"" + dtoic.Rows[i]["OICMobileNo"].ToString() + "\"}}";
                                }
                            }
                            index = Str.LastIndexOf(',');
                            Str = Str.Remove(index, 1);
                            Str += "]}]}}]}";

                            string result;
                            string recordid;
                            //var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://192.168.10.74/gcmsscripts//ASBTStructRest.dll/datasnap/rest/TASBTStruct/savedata");
                            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://10.70.230.61/gcmsscripts//ASBTStructRest.dll/datasnap/rest/TASBTStruct/savedata");
                            cmd = objDBAccess.CreateCommand("UpdateCaseJson", DBFactoryCommandType.PROCEDURE_COMMAND);
                            objDBAccess.AddParameterToCommand("@CaseID", DBFactoryParameterType.Int, Convert.ToInt32(Obj.CaseRegID));
                            objDBAccess.AddParameterToCommand("@casejson", DBFactoryParameterType.NVarchar, Str.ToString());
                            Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                            httpWebRequest.ContentType = "application/json";
                            httpWebRequest.Method = "POST";
                            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                            {
                                string json = Str;
                                streamWriter.Write(json);
                                streamWriter.Flush();
                                streamWriter.Close();
                            }
                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                result = streamReader.ReadToEnd();
                                var temp = result;
                                JsonCourt obj = new JsonCourt();
                                obj = (JsonCourt)s.Deserialize(result, typeof(JsonCourt));
                                result = obj.result[0].message[0].PortalRegNo.ToString();
                                recordid = obj.result[0].message[0].recordid.ToString();
                            }

                            cmd = objDBAccess.CreateCommand("UpdateCaseRegno", DBFactoryCommandType.PROCEDURE_COMMAND);
                            objDBAccess.AddParameterToCommand("@CaseID", DBFactoryParameterType.Int, Obj.CaseRegID);
                            objDBAccess.AddParameterToCommand("@caseregno", DBFactoryParameterType.NVarchar, result.ToString());
                            objDBAccess.AddParameterToCommand("@recordid", DBFactoryParameterType.NVarchar, recordid.ToString());
                            Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "AddOICDetails(OICDetails Obj)" }, ex);
            }
            return Result;
        }


        public int AddAdvocateDetails(AdvocateDetails Obj)
        {

            int Result = 0;
            try
            {
                foreach (AdvocateScopeEntity objadv in Obj.lstAdvocateScope)
                {
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("InsertAdvocateDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@CaseRegID", DBFactoryParameterType.NVarchar, objadv.CaseRegID);
                    objDBAccess.AddParameterToCommand("@AdvocateName", DBFactoryParameterType.NVarchar, objadv.AdvocateName);
                    objDBAccess.AddParameterToCommand("@AdvocateType", DBFactoryParameterType.NVarchar, objadv.AdvocateTypeID);
                    objDBAccess.AddParameterToCommand("@AdvocateMobileNo", DBFactoryParameterType.NVarchar, objadv.AdvocateMobileNo);
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "AddAdvocateDetails(AdvocateDetails Obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int AddEnclouserDetails(EnclouserDetails ObjEnc)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                foreach (Enclouseruploadlst obj in ObjEnc.lstEnclouserupload)
                {
                    SqlCommand cmd = objDBAccess.CreateCommand("InsertEnclouserDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@CaseRegID", DBFactoryParameterType.NVarchar, ObjEnc.CaseRegID);
                    objDBAccess.AddParameterToCommand("@NoofEnclouser", DBFactoryParameterType.Int, ObjEnc.NoofEnclouser);
                    objDBAccess.AddParameterToCommand("@Enclouserupload", DBFactoryParameterType.NVarchar, obj.EnclouserName);
                    objDBAccess.AddParameterToCommand("@Enclousertype", DBFactoryParameterType.NVarchar, obj.EnclouserType);
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "AddEnclouserDetails(EnclouserDetails ObjEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }






        public DataTable CourtList()
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CourtList", DBFactoryCommandType.PROCEDURE_COMMAND);
                // objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOID);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "CourtList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable CourtActList()
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CourtActList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "CourtActList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable CourtSectionList()
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CourtSectionList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "CourtSectionList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable CourtCaseTypeList()
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CourtCaseTypeList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "CourtCaseTypeList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable CourtPurposeList()
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CourtPurposeList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "CourtPurposeList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable EnclouserDocList()
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("EnclouserDocList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "EnclouserDocList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable AdvocateTypeList()
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AdvocateTypeList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "AdvocateTypeList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable CourtAdvocateList()
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CourtAdvocateList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "CourtAdvocateList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;

        }

        public DataTable GetAllCaseDetails(string WhereClause)
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAllCaseDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetAllCaseDetails(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataSet GetCaseDetails(string WhereClause)
        {
            DataSet dset = new DataSet();
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetCaseDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dset = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetCaseDetails(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dset;

        }

        public DataSet GetCaseDetailsByCaseId(string CaseRegId)
        {
            DataSet dset = new DataSet();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("CaseDetailsByCaseId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CaseRegID", DBFactoryParameterType.NVarchar, CaseRegId);
                dset = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetCaseDetailsByCaseId(string CaseRegId)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dset;
        }

        public DataTable GetAllCaseList(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CaseList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetAllCaseList(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        #endregion



        public DataTable CityTypeList()
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CityTypeList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "CityTypeList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        #region Society Amendment

        public int InsertByLaws(ByeLawsMst Obj)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertByLaws", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ByeLawsMstID", DBFactoryParameterType.Int, Obj.ByeLawsMstID);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, Obj.SocietyIdEnc);
                objDBAccess.AddParameterToCommand("@RegiYearId", DBFactoryParameterType.Int, Obj.RegiYearId);
                objDBAccess.AddParameterToCommand("@ByeLawsFile", DBFactoryParameterType.NVarchar, Obj.ByeLawsFile);
                objDBAccess.AddParameterToCommand("@ResolutionNo", DBFactoryParameterType.NVarchar, Obj.ResolutionNo);
                objDBAccess.AddParameterToCommand("@ResolutionFile", DBFactoryParameterType.NVarchar, Obj.ResolutionFile);
                objDBAccess.AddParameterToCommand("@PaasedAGMDate", DBFactoryParameterType.Date, Obj.PaasedAGMDate);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Obj.Remark);
                objDBAccess.AddParameterToCommand("@InsertDate", DBFactoryParameterType.DateTime, Obj.InsertDate);
                objDBAccess.AddParameterToCommand("@InsertBy", DBFactoryParameterType.Int, Obj.InsertBy);
                objDBAccess.AddParameterToCommand("@InsertIP", DBFactoryParameterType.NVarchar, Obj.InsertIP);
                objDBAccess.AddParameterToCommand("@AGM1File", DBFactoryParameterType.NVarchar, Obj.AGM1File);
                objDBAccess.AddParameterToCommand("@AGM2File", DBFactoryParameterType.NVarchar, Obj.AGM2File);
                objDBAccess.AddParameterToCommand("@AGM3File", DBFactoryParameterType.NVarchar, Obj.AGM3File);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                //if (Result == 1)
                //{
                //    cmd = objDBAccess.CreateCommand("InsertSocietyByelawsFile", DBFactoryCommandType.PROCEDURE_COMMAND);
                //    objDBAccess.AddParameterToCommand("@FileName", DBFactoryParameterType.NVarchar, Obj.ByeLawsFile);
                //    objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, Obj.SocietyIdEnc);
                //    objDBAccess.AddParameterToCommand("@RegiYearId", DBFactoryParameterType.Int, Obj.RegiYearId);
                //    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                //    if (Result == 1)
                //    {
                //        //ApplicationFlowModel _Modal = new ApplicationFlowModel();
                //        //_Modal.AddApplicationFlow(Obj.SocietyIdEnc, 2, "Submit Amendments of ByeLaws File.", Obj.InsertBy, true);
                //        //_Modal.AddApplicationFlow(Obj.SocietyIdEnc, 3, "Asign CLerk for Amendments.", Obj.InsertBy, true);

                //        cmd = objDBAccess.CreateCommand("InsertSocietyApplicationFlowForAmendment", DBFactoryCommandType.PROCEDURE_COMMAND);
                //        objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, Obj.SocietyIdEnc);
                //        objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Int, Obj.InsertBy);
                //        objDBAccess.AddParameterToCommand("@CreateIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                //        Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                //    }
                //}
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "InsertByLaws(ByeLawsMst Obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public DataTable GetByLawsList(string SocietyIdEnc)
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetByLawsList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetByLawsList(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int InsertSocietyNameChange(ByeLawsMst Obj)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertSocietyNameChangeMst", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ByeLawsMstID", DBFactoryParameterType.Int, Obj.ByeLawsMstID);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, Obj.SocietyIdEnc);
                objDBAccess.AddParameterToCommand("@RegiYearId", DBFactoryParameterType.Int, Obj.RegiYearId);
                objDBAccess.AddParameterToCommand("@ByeLawsFile", DBFactoryParameterType.NVarchar, Obj.ByeLawsFile);
                objDBAccess.AddParameterToCommand("@ResolutionNo", DBFactoryParameterType.NVarchar, Obj.ResolutionNo);
                objDBAccess.AddParameterToCommand("@ResolutionFile", DBFactoryParameterType.NVarchar, Obj.ResolutionFile);
                objDBAccess.AddParameterToCommand("@PaasedAGMDate", DBFactoryParameterType.Date, Obj.PaasedAGMDate);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Obj.Remark);
                objDBAccess.AddParameterToCommand("@InsertDate", DBFactoryParameterType.DateTime, Obj.InsertDate);
                objDBAccess.AddParameterToCommand("@InsertBy", DBFactoryParameterType.Int, Obj.InsertBy);
                objDBAccess.AddParameterToCommand("@InsertIP", DBFactoryParameterType.NVarchar, Obj.InsertIP);
                objDBAccess.AddParameterToCommand("@AGM1File", DBFactoryParameterType.NVarchar, Obj.AGM1File);
                objDBAccess.AddParameterToCommand("@AGM2File", DBFactoryParameterType.NVarchar, Obj.AGM2File);
                objDBAccess.AddParameterToCommand("@AGM3File", DBFactoryParameterType.NVarchar, Obj.AGM3File);
                objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, string.IsNullOrWhiteSpace(Obj.Name) ? "" : Obj.Name);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                //if (Result == 1)
                //{
                //    cmd = objDBAccess.CreateCommand("InsertSocietyNameChange", DBFactoryCommandType.PROCEDURE_COMMAND);
                //    objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, Obj.Name);
                //    objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, Obj.SocietyIdEnc);
                //    objDBAccess.AddParameterToCommand("@RegiYearId", DBFactoryParameterType.Int, Obj.RegiYearId);
                //    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                //    if (Result == 1)
                //    {
                //        //ApplicationFlowModel _Modal = new ApplicationFlowModel();
                //        //_Modal.AddApplicationFlow(Obj.SocietyIdEnc, 2, "Submit Amendments of ByeLaws File.", Obj.InsertBy, true);
                //        //_Modal.AddApplicationFlow(Obj.SocietyIdEnc, 3, "Asign CLerk for Amendments.", Obj.InsertBy, true);

                //        cmd = objDBAccess.CreateCommand("InsertSocietyApplicationFlowForAmendment", DBFactoryCommandType.PROCEDURE_COMMAND);
                //        objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, Obj.SocietyIdEnc);
                //        objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Int, Obj.InsertBy);
                //        objDBAccess.AddParameterToCommand("@CreateIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                //        Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                //    }
                //}
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "InsertSocietyNameChange(ByeLawsMst Obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public DataTable GetSocietyNameChangeList(string SocietyIdEnc)
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyNameChangeList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetSocietyNameChangeList(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }



        public DataTable GetChangeSocietyMemberExcelList(string SocietyIdEnc)
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetChangeSocietyMemberExcelList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetChangeSocietyMemberExcelList(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int InsertChangeSocietyMemberExcel(ByeLawsMst Obj)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertSocietyMemberExcel", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MemberExcel_Name", DBFactoryParameterType.NVarchar, Obj.Name);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, Obj.SocietyIdEnc);
                objDBAccess.AddParameterToCommand("@RegiYearId", DBFactoryParameterType.Int, Obj.RegiYearId);
                objDBAccess.AddParameterToCommand("@Remarks", DBFactoryParameterType.NVarchar, Obj.Remark);
                objDBAccess.AddParameterToCommand("@InsertBy", DBFactoryParameterType.Int, Obj.InsertBy);
                objDBAccess.AddParameterToCommand("@InsertIP", DBFactoryParameterType.NVarchar, Obj.InsertIP);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                if (Result == 1)
                {
                    cmd = objDBAccess.CreateCommand("[InsertSocietyNMemberListFile]", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@MemberExcel_Name", DBFactoryParameterType.NVarchar, Obj.Name);
                    objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, Obj.SocietyIdEnc);
                    objDBAccess.AddParameterToCommand("@RegiYearId", DBFactoryParameterType.Int, Obj.RegiYearId);
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                    if (Result == 1)
                    {


                        cmd = objDBAccess.CreateCommand("InsertSocietyApplicationFlowForAmendment", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, Obj.SocietyIdEnc);
                        objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Int, Obj.InsertBy);
                        objDBAccess.AddParameterToCommand("@CreateIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                        Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "InsertSocietyNameChange(ByeLawsMst Obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public string InsertSocietyAdministrativeDetailsAmendment(List<AdministrativeData> objData, string ClassA, string ClassB, string ClassC, int EMitraTransactionId)
        {
            string Result = "-1";

            try
            {
                int i = 0;
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                int CheckInsertUpdate = 0;

                foreach (var obj in objData)
                {

                    if (obj.Photo != null)
                    {
                        if (obj.Photo.ToString().Contains("SocietyProfiling"))
                        {
                            var temp = obj.Photo.ToString().Split('/');
                            obj.Photo = temp.Length > 3 ? temp[temp.Length - 1].ToString() : "";
                        }
                        if (obj.Photo.ToString().Contains("NewRegistration"))
                        {
                            var temp = obj.Photo.ToString().Split('/');
                            obj.Photo = temp.Length > 3 ? temp[temp.Length - 1].ToString() : "";
                        }
                        if (obj.Photo.ToString().Contains("Balnk"))
                        {

                            obj.Photo = "";
                        }
                    }

                    if (obj.Name.Length > 0)
                    {
                        var DOBConvert = obj.DOB.Length > 3 ? Convert.ToDateTime(obj.DOB) : DateTime.MaxValue;
                        var DOJConvert = obj.AppointedDate == null ? DateTime.MaxValue : (obj.AppointedDate.Length > 3 ? Convert.ToDateTime(obj.AppointedDate) : DateTime.MaxValue);
                        objDBAccess.OpenConnection();
                        SqlCommand cmd = new SqlCommand();

                        cmd = objDBAccess.CreateCommand("InsertSocietyMemberAmendment", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@EMitraTransactionId", DBFactoryParameterType.Int, EMitraTransactionId);
                        objDBAccess.AddParameterToCommand("@AdministrativeId", DBFactoryParameterType.Int, obj.AdministrativeId);
                        objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.NVarchar, obj.SocietyIdEnc);
                        //objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, obj.SocietyId);
                        objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, obj.Name);
                        objDBAccess.AddParameterToCommand("@ContactNo", DBFactoryParameterType.NVarchar, obj.ContactNo);
                        objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.Int, obj.UserType);
                        objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                        // objDBAccess.AddParameterToCommand("@CreateDate", DBFactoryParameterType.DateTime,m.DateTime.Now);
                        objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, 1);
                        objDBAccess.AddParameterToCommand("@AppointedDate", DBFactoryParameterType.DateTime, DOJConvert);
                        objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, obj.IsActive);
                        objDBAccess.AddParameterToCommand("@ClassA", DBFactoryParameterType.Int, ClassA.ToString() == "undefined" ? 0 : int.Parse(ClassA));
                        objDBAccess.AddParameterToCommand("@ClassB", DBFactoryParameterType.Int, ClassB.ToString() == "undefined" ? 0 : int.Parse(ClassB));
                        objDBAccess.AddParameterToCommand("@ClassC", DBFactoryParameterType.Int, ClassC.ToString() == "undefined" ? 0 : int.Parse(ClassC));
                        objDBAccess.AddParameterToCommand("@flag", DBFactoryParameterType.Int, i);
                        objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                        objDBAccess.AddParameterToCommand("@DOB", DBFactoryParameterType.DateTime, DOBConvert);
                        objDBAccess.AddParameterToCommand("@AADHARENO", DBFactoryParameterType.NVarchar, (!string.IsNullOrEmpty(obj.AADHAARNo) ? COMMON_FUNCTION.Encrypt(obj.AADHAARNo) : ""));
                        objDBAccess.AddParameterToCommand("@GENDER", DBFactoryParameterType.NVarchar, obj.Gender);
                        objDBAccess.AddParameterToCommand("@Photo", DBFactoryParameterType.NVarchar, obj.Photo != null ? (obj.Photo.Contains("fakepath") ? "" : obj.Photo) : "");
                        objDBAccess.AddParameterToCommand("@AdministratorDesignationId", DBFactoryParameterType.Int, obj.AdministratorDesignationId);
                        objDBAccess.AddParameterToCommand("@PostId", DBFactoryParameterType.Int, obj.PostId);
                        objDBAccess.AddParameterToCommand("@CategoryId", DBFactoryParameterType.Int, obj.CategoryId);
                        objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, obj.Address);
                        objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                        objDBAccess.AddParameterToCommand("@KIOSKCODE", DBFactoryParameterType.NVarchar, _LoggedInUser.KIOSKCODE);
                        objDBAccess.AddParameterToCommand("@MEMBERTYPE", DBFactoryParameterType.NVarchar, obj.MEMBERTYPE);
                        objDBAccess.AddParameterToCommand("@POSTNAME", DBFactoryParameterType.NVarchar, obj.PostName);
                        objDBAccess.AddParameterToCommand("@Occupation", DBFactoryParameterType.NVarchar, obj.Occupation);
                        objDBAccess.AddParameterToCommand("@NominatedBy", DBFactoryParameterType.NVarchar, obj.NominatedBy);
                        objDBAccess.AddParameterToCommand("@RequestType", DBFactoryParameterType.NVarchar, obj.RequestType);
                        objDBAccess.AddParameterToCommand("@RequestStatus", DBFactoryParameterType.NVarchar, obj.RequestStatus);
                        objDBAccess.AddParameterToCommand("@oldAdministrativeId", DBFactoryParameterType.NVarchar, obj.AdministrativeId);
                        if (_LoggedInUser.KIOSKCODE.Length > 0)
                        {
                            objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.NVarchar, _LoggedInUser.ServiceId);
                        }
                        else
                        {
                            if (obj.SocietyIdEnc.Length > 0)
                                objDBAccess.AddParameterToCommand("@ServicesId", DBFactoryParameterType.Int, 4210);// Update Profile
                        }




                        //Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                        Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                        //if ((Result.Split('@').Length > 1))
                        //    EMitraTransactionId = int.Parse(Result.Split('@')[1]);

                    }
                    i++;
                }

                //if (objData.Count > 0)
                //{
                //    // ApplicationFlowModel _Modal = new ApplicationFlowModel();
                //    // _Modal.AddApplicationFlow(objData[0].SocietyIdEnc, 2, "Submit Amendments of Administrative Details(Members).", _LoggedInUser.UserId, true);

                //    SqlCommand cmd = objDBAccess.CreateCommand("InsertSocietyApplicationFlowForAmendment", DBFactoryCommandType.PROCEDURE_COMMAND);
                //    objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, objData[0].SocietyIdEnc);
                //    objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                //    objDBAccess.AddParameterToCommand("@CreateIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                //    objDBAccess.ExecuteScalarCommand();
                //}
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "InsertSocietyAdministrativeDetailsAmendment(List<AdministrativeData> objData, string ClassA, string ClassB, string ClassC, int EMitraTransactionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }




        public int InsertAGM_BODM(AGM_BODM_Mst Obj)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertAGM_BODM", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@AGM_BODM_MstID", DBFactoryParameterType.Int, Obj.AGM_BODM_MstID);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, Obj.SocietyIdEnc);
                objDBAccess.AddParameterToCommand("@ByeLawsDate", DBFactoryParameterType.Date, Obj.ByeLawsDate);
                objDBAccess.AddParameterToCommand("@MeetingDate", DBFactoryParameterType.Date, Obj.MeetingDate);
                objDBAccess.AddParameterToCommand("@MeetingPDF", DBFactoryParameterType.NVarchar, Obj.MeetingPDF);
                objDBAccess.AddParameterToCommand("@Is_AGM_BODM", DBFactoryParameterType.Bit, Obj.Is_AGM_BODM);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Obj.Remark);
                objDBAccess.AddParameterToCommand("@InsertDate", DBFactoryParameterType.DateTime, Obj.InsertDate);
                objDBAccess.AddParameterToCommand("@InsertBy", DBFactoryParameterType.Int, Obj.InsertBy);
                objDBAccess.AddParameterToCommand("@InsertIP", DBFactoryParameterType.NVarchar, Obj.InsertIP);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "InsertAGM_BODM(AGM_BODM_Mst Obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public DataTable GetAGM_BODMList(string SocietyIdEnc)
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAGM_BODMList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetAGM_BODMList(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetBalanceSheetAllList(string SocietyIdEnc)
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetBalanceSheetFileList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetBalanceSheetAllList(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetBalanceSheet(string SocietyIdEnc)
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetBalanceSheet", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                DataTable dtFromDB = new DataTable();
                string connString = "";
                dtFromDB = objDBAccess.GetTable();
                if (dtFromDB != null && dtFromDB.Rows.Count > 0)
                {
                    var EXLPath = dtFromDB.Rows[0][0].ToString();
                    if (EXLPath != null && EXLPath != "" && EXLPath != "-1")
                    {
                        string strFileType = System.IO.Path.GetExtension(EXLPath).ToLower();

                        string path = string.Concat(System.Web.HttpContext.Current.Server.MapPath(EXLPath));

                        if (strFileType.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        }
                        else if (strFileType.Trim() == ".xlsx")
                        {
                            connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        }

                        OleDbConnection conn = new OleDbConnection(connString);

                        if (conn.State == ConnectionState.Closed)
                            conn.Open();

                        DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                        foreach (DataRow SheetRow in dtSheet.Rows)
                        {
                            //if (SheetRow.ItemArray[2].ToString() == "MemberDetail$")
                            //{
                            string query = "SELECT * FROM [" + SheetRow.ItemArray[2] + "]";
                            OleDbCommand cmd2 = new OleDbCommand(query, conn);
                            OleDbDataAdapter da = new OleDbDataAdapter(cmd2);
                            da.Fill(dt);
                            break;
                            //}

                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetBalanceSheet(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable InsertBalanceSheetByExcel(string ExcelBalanceSheetFile, string SocietyIdEnc, string SocietyDirectory, int RegiYearId)
        {
            int Result = -1;
            DataTable dt = new DataTable();
            string connString = "";
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertSocietyDocumentTrans", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FileName", DBFactoryParameterType.NVarchar, ExcelBalanceSheetFile);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                string strFileType = System.IO.Path.GetExtension(ExcelBalanceSheetFile).ToLower();
                if (Result > 0)
                {
                    string path = string.Concat(System.Web.HttpContext.Current.Server.MapPath("../Content/SocietyProfiling/" + SocietyDirectory + "/" + ExcelBalanceSheetFile));

                    //Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();
                    //Microsoft.Office.Interop.Excel.Workbook workbook = application.Workbooks.Open(path);
                    //Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.ActiveSheet;
                    //Microsoft.Office.Interop.Excel.Range range = worksheet.UsedRange;
                    //if (range.Columns.Count < 6)
                    //{
                    //    dt.Columns.Add("Sr. No.");
                    //    dt.Columns.Add("Liability_Head");
                    //    dt.Columns.Add("Liability_Amount");
                    //    dt.Columns.Add("Asset_Head");
                    //    dt.Columns.Add("Asset_Amount");
                    //    for (int row = 2; row <= range.Rows.Count; row++)
                    //    {
                    //        var Liability_Head = ((Microsoft.Office.Interop.Excel.Range)range.Cells[row, 2]).Text;
                    //        var Liability_Amount = ((Microsoft.Office.Interop.Excel.Range)range.Cells[row, 3]).Text;
                    //        var Asset_Head = ((Microsoft.Office.Interop.Excel.Range)range.Cells[row, 4]).Text;
                    //        var Asset_Amount = ((Microsoft.Office.Interop.Excel.Range)range.Cells[row, 5]).Text;
                    //        if ((Liability_Head != null && Liability_Head.ToString().Trim().Length > 0) || (Asset_Head != null && Asset_Head.ToString().Trim().Length > 0))
                    //        {
                    //            double Liability_Amount_Local = 0;
                    //            if (Liability_Amount != null && Liability_Amount.ToString().Trim().Length > 0)
                    //            {
                    //                double.TryParse(Liability_Amount.ToString(), out Liability_Amount_Local);
                    //            }
                    //            double Asset_Amount_Local = 0;
                    //            if (Asset_Amount != null && Asset_Amount.ToString().Trim().Length > 0)
                    //            {
                    //                double.TryParse(Asset_Amount.ToString(), out Asset_Amount_Local);
                    //            }
                    //            dt.Rows.Add(new object[] { row - 1, Liability_Head, Liability_Amount, Asset_Head, Asset_Amount });
                    //            SaveBalanceSheetRow(RegiYearId, SocietyIdEnc, Liability_Head.ToString().Trim(), Liability_Amount_Local == 0 ? "" : Liability_Amount_Local.ToString(), Asset_Head.ToString().Trim(), Asset_Amount_Local == 0 ? "" : Asset_Amount_Local.ToString(), Result);
                    //        }
                    //    }
                    //    workbook.Close(0);
                    //    application.Quit();
                    //}

                    //if (objDBAccess.GetConnection.State == ConnectionState.Closed)
                    //    objDBAccess.OpenConnection();

                    //cmd = objDBAccess.CreateCommand("InsertSocietyBalanceSheetFile", DBFactoryCommandType.PROCEDURE_COMMAND);
                    //objDBAccess.AddParameterToCommand("@FileName", DBFactoryParameterType.NVarchar, ExcelBalanceSheetFile);
                    //objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                    //objDBAccess.AddParameterToCommand("@RegiYearId", DBFactoryParameterType.Int, RegiYearId);
                    //Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                    //if (Result == 1)
                    //{
                    //    cmd = objDBAccess.CreateCommand("InsertSocietyApplicationFlowForAmendment", DBFactoryCommandType.PROCEDURE_COMMAND);
                    //    objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                    //    objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                    //    objDBAccess.AddParameterToCommand("@CreateIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                    //    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                    //}
                    Result = -1;

                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.InsertSocityByExcel", FunctionName = "InsertBalanceSheetByExcel(string ExcelBalanceSheetFile, string SocietyIdEnc, string SocietyDirectory, int RegiYearId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable SaveBalanceSheetRow(int RegiYearId, string SocietyIdEnc, string LiabilityName, string LiabilityValue, string AssetName, string AssetValue, int SocietyDocumentTransId)
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertBalanceSheet", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RegiYearId", DBFactoryParameterType.Int, RegiYearId);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdEnc);
                objDBAccess.AddParameterToCommand("@LiabilityName", DBFactoryParameterType.NVarchar, LiabilityName);
                objDBAccess.AddParameterToCommand("@LiabilityValue", DBFactoryParameterType.NVarchar, LiabilityValue);
                objDBAccess.AddParameterToCommand("@AssetName", DBFactoryParameterType.NVarchar, AssetName);
                objDBAccess.AddParameterToCommand("@AssetValue", DBFactoryParameterType.NVarchar, AssetValue);
                objDBAccess.AddParameterToCommand("@SocietyDocumentTransId", DBFactoryParameterType.Int, SocietyDocumentTransId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "SaveBalanceSheetRow(int RegiYearId, string SocietyIdEnc, string LiabilityName, string LiabilityValue, string AssetName, string AssetValue, int SocietyDocumentTransId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        #endregion


        public DataTable GetSocietyFileByFinancialYear(string SocietyIdENC, int FYearID)
        {
            int Result = -1;
            DataTable dt = new DataTable();
            string connString = "";
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyFileByFinancialYear", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, SocietyIdENC);
                objDBAccess.AddParameterToCommand("@FYearID", DBFactoryParameterType.Int, FYearID);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.InsertSocityByExcel", FunctionName = "DataTable InsertSocityVoterMemberByExcel(SocietyVoterMemberEntity obj)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetSocietyByGP_ID(int GP_ID, int SocietyCatyegoryId = 0)
        {
            int Result = -1;
            DataTable dt = new DataTable();
            string connString = "";
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyByGPID", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GPID", DBFactoryParameterType.Int, GP_ID);
                objDBAccess.AddParameterToCommand("@SocietyCatyegoryId", DBFactoryParameterType.Int, SocietyCatyegoryId);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.InsertSocityByExcel", FunctionName = "DataTable GetSocietyByGP_ID(  int GP_ID)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable SocietyListForAuditRPT(int UserId, string SSOID)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyListForAuditRPT", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@AuditorId", DBFactoryParameterType.Int, UserId);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOID);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "SocietyListForAuditRPT(int UserId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int UpdateUserSSOIDByRegNo(string SSOID, string @RegistrationNo)
        {
            int Result = -1;
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateUserSSOIDByRegNo", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOID);
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, RegistrationNo);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "UpdateUserSSOIDByRegNo(string SSOID,string @RegistrationNo)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataTable GetAllRegno()
        {
            DataTable dt = new DataTable();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAllSocietyForDir", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CITIZEN", FunctionName = "DataTable GetAllRegno()" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetAllPhoto(int flag=0,int SID=0)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand(flag == 0 ? "GetAllPhoto" : "DocumentSet", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SID", DBFactoryParameterType.Int, SID);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetAllPhoto" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GETNAME()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("GetAllSocietyes", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GETNAME" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetAllDoc()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAllDoc", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetAllDoc" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int SaveMemberRegistrationStatus(int RequestId, int Status, string Remark, string ActionTakenSSOId, string UserType,string MemberCertificate)
        {
            int Result = -1;
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("MemberRegistrationApproval", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RequestId", DBFactoryParameterType.Int, RequestId);
                objDBAccess.AddParameterToCommand("@ActionStatus", DBFactoryParameterType.Int, Status);
                objDBAccess.AddParameterToCommand("@ActionRemark", DBFactoryParameterType.NVarchar, Remark);
                objDBAccess.AddParameterToCommand("@ActionTakenBySSOId", DBFactoryParameterType.NVarchar, ActionTakenSSOId);
                objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, UserType);
                objDBAccess.AddParameterToCommand("@MemberCertificate", DBFactoryParameterType.NVarchar, MemberCertificate);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "public int SaveMemberRegistrationStatus(int RequestId, int Status, string Remark,string ActionTakenSSOId,string UserType)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int SocietyMappingInsert(SocietyMappingEntity model)
        {
            int Result = -1;
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyMappingRequestInsert", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, model.Name);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, model.ActId);
                objDBAccess.AddParameterToCommand("@FatherName", DBFactoryParameterType.NVarchar, model.FatherName);
                objDBAccess.AddParameterToCommand("@AADHARNo", DBFactoryParameterType.NVarchar, model.AADHARNo);
                objDBAccess.AddParameterToCommand("@DOB", DBFactoryParameterType.NVarchar, model.DOB);
                objDBAccess.AddParameterToCommand("@Gender", DBFactoryParameterType.NVarchar, model.Gender);
                objDBAccess.AddParameterToCommand("@Photo", DBFactoryParameterType.NVarchar, model.Photo);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.VARCHAR, model.SocietyId);
                objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, model.Address);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.VARCHAR, model.MobileNo);
                objDBAccess.AddParameterToCommand("@CategoryId", DBFactoryParameterType.Int, model.CategoryId);
                objDBAccess.AddParameterToCommand("@PostName", DBFactoryParameterType.NVarchar, model.PostName);
                objDBAccess.AddParameterToCommand("@Occupation", DBFactoryParameterType.NVarchar, model.Occupation);
                objDBAccess.AddParameterToCommand("@RequestBySSOId", DBFactoryParameterType.NVarchar, model.RequestBySSOId);
                objDBAccess.AddParameterToCommand("@AADHAREnc", DBFactoryParameterType.NVarchar, model.AADHARNoEnc);
                objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, model.UserType);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, model.ActionId);


                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "public int SocietyMappingInsert(SocietyMappingEntity model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int SocietyMappingInsertByEmitra(SocietyMappingEntity model)
        {
            int Result = -1;
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyMappingRequestInsertByEmitra", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, model.Name);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, model.ActId);
                objDBAccess.AddParameterToCommand("@FatherName", DBFactoryParameterType.NVarchar, model.FatherName);
                objDBAccess.AddParameterToCommand("@AADHARNo", DBFactoryParameterType.NVarchar, model.AADHARNo);
                objDBAccess.AddParameterToCommand("@DOB", DBFactoryParameterType.NVarchar, model.DOB);
                objDBAccess.AddParameterToCommand("@Gender", DBFactoryParameterType.NVarchar, model.Gender);
                objDBAccess.AddParameterToCommand("@Photo", DBFactoryParameterType.NVarchar, model.Photo);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.VARCHAR, model.SocietyId);
                objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, model.Address);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.VARCHAR, model.MobileNo);
                objDBAccess.AddParameterToCommand("@CategoryId", DBFactoryParameterType.Int, model.CategoryId);
                objDBAccess.AddParameterToCommand("@PostName", DBFactoryParameterType.NVarchar, model.PostName);
                objDBAccess.AddParameterToCommand("@Occupation", DBFactoryParameterType.NVarchar, model.Occupation);
                objDBAccess.AddParameterToCommand("@RequestBySSOId", DBFactoryParameterType.NVarchar, model.RequestBySSOId);
                objDBAccess.AddParameterToCommand("@AADHAREnc", DBFactoryParameterType.NVarchar, model.AADHARNoEnc);
                objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, model.UserType);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, model.ActionId);


                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "public int SocietyMappingInsertByEmitra(SocietyMappingEntity model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetSocietyMappingRequestList(int loggedInUserRoleId, int loggedInUserId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyMappingRequestList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@LoggedInUserId", DBFactoryParameterType.Int, loggedInUserId);
                objDBAccess.AddParameterToCommand("@LoggedInUserRoleId", DBFactoryParameterType.Int, loggedInUserRoleId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "public DataTable GetSocietyMappingRequestList(int loggedInUserRoleId,int loggedInUserId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int SaveSocietyMappingProcessRequest(int RequestId, int NextActionId, int NextRoleId, int NextUserId, string Remark, string SSO, int loggedInUserId, int loggedinUserRole)
        {
            int Result = -1;
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyMappingProcessRequestInsert", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RequestId", DBFactoryParameterType.Int, RequestId);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, NextActionId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, loggedinUserRole);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, loggedInUserId);
                objDBAccess.AddParameterToCommand("@NextRoleId", DBFactoryParameterType.Int, NextRoleId);
                objDBAccess.AddParameterToCommand("@NextUserId", DBFactoryParameterType.Int, NextUserId);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Remark);
                objDBAccess.AddParameterToCommand("@ActionBySSOId", DBFactoryParameterType.NVarchar, SSO);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "public int SaveSocietyMappingProcessRequest(int RequestId, int NextActionId, int NextRoleId, int NextUserId, string Remark,string SSO)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable SocietyMappingRequestActivityLog(int RequestId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyMappingRequestActivityLog", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RequestId", DBFactoryParameterType.Int, RequestId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "public DataTable SocietyMappingRequestActivityLog(int RequestId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetCreditCooperativeSocieties(string WhereClause)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetSocietyBasicDetail", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, 100000);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, 1);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "public DataTable GetCreditCooperativeSocieties(string WhereClause)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetCreditCooperativeSocietiesGreviyans(string WhereClause, int LoggedInUserDistrictId = -1, int IsSingleState=0)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetSocietyBasicDetailForWeb", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistID", DBFactoryParameterType.Int, WhereClause);
                objDBAccess.AddParameterToCommand("@LoggedInUserDistrictId", DBFactoryParameterType.Int, LoggedInUserDistrictId);
                objDBAccess.AddParameterToCommand("@IsSingleState", DBFactoryParameterType.Bit, Convert.ToBoolean(IsSingleState));
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahakar.Models.CitizenModel", FunctionName = "public DataTable GetCreditCooperativeSocieties(string WhereClause)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public int SaveCCSComplaint(CCSComplaintRegisterModel model)
        {
            int Result = -1;

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CCSComplaintInsert", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ComplainerName", DBFactoryParameterType.NVarchar, model.ComplainerName);
                objDBAccess.AddParameterToCommand("@ComplainerFatherName", DBFactoryParameterType.NVarchar, model.ComplainerFatherName);
                objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, model.Address);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, model.DistrictId);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, model.MobileNo);
                objDBAccess.AddParameterToCommand("@Email", DBFactoryParameterType.NVarchar, model.Email);
                objDBAccess.AddParameterToCommand("@CCSId", DBFactoryParameterType.Int, model.CCSId);
                objDBAccess.AddParameterToCommand("@BranchName", DBFactoryParameterType.NVarchar, model.BranchName);
                objDBAccess.AddParameterToCommand("@AccountNo", DBFactoryParameterType.NVarchar, model.AccountNo);
                objDBAccess.AddParameterToCommand("@IFSCCode", DBFactoryParameterType.NVarchar, model.IFSCCode);
                objDBAccess.AddParameterToCommand("@AmountDeposited", DBFactoryParameterType.Money, model.AmountDeposited);
                objDBAccess.AddParameterToCommand("@DepositDate", DBFactoryParameterType.NVarchar, model.DepositDate);
                objDBAccess.AddParameterToCommand("@AADHAR", DBFactoryParameterType.NVarchar, model.AADHAR);
                objDBAccess.AddParameterToCommand("@CreateByAADHAR", DBFactoryParameterType.NVarchar, model.AADHAR);
                objDBAccess.AddParameterToCommand("@CreateIP", DBFactoryParameterType.NVarchar, model.CreateIP);
                objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, model.UserType);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, (string.Equals(model.UserType.ToUpper(), "C") ? true : false));
                objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, model.SSOId);
                objDBAccess.AddParameterToCommand("@MembershipId", DBFactoryParameterType.NVarchar, model.MembershipId);
                objDBAccess.AddParameterToCommand("@Remarks", DBFactoryParameterType.NVarchar, model.Remarks);
                objDBAccess.AddParameterToCommand("@TotalUploadedDocument", DBFactoryParameterType.Int, model.FileDetails.Count);

                objDBAccess.AddParameterToCommand("@AccountNoInSociety", DBFactoryParameterType.NVarchar, model.AccountNoInSociety);
                objDBAccess.AddParameterToCommand("@AccountIFSCInSociety", DBFactoryParameterType.NVarchar, model.AccountIFSCInSociety);
                objDBAccess.AddParameterToCommand("@AccountHolderName", DBFactoryParameterType.NVarchar, model.AccountHolderName);
                objDBAccess.AddParameterToCommand("@PassbookCrossCheque", DBFactoryParameterType.NVarchar, model.PassbookCrossCheque);
                objDBAccess.AddParameterToCommand("@BPNo", DBFactoryParameterType.NVarchar, model.BPNo);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                if (Result > 0)
                {
                    foreach (var item in model.FileDetails)
                    {
                        SqlCommand cmd2 = objDBAccess.CreateCommand("CCSComplaintDetailInsert", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@CCSComplaintRegisterId", DBFactoryParameterType.Int, Result);
                        objDBAccess.AddParameterToCommand("@FileTypeId", DBFactoryParameterType.Int, item.FileTypeId);
                        objDBAccess.AddParameterToCommand("@FilePath", DBFactoryParameterType.NVarchar, item.FilePath);
                        objDBAccess.AddParameterToCommand("@AccountNo", DBFactoryParameterType.NVarchar, item.AccountNo);
                        objDBAccess.AddParameterToCommand("@PaidAmount", DBFactoryParameterType.Money, item.PaidAmount);
                        objDBAccess.AddParameterToCommand("@PayDate", DBFactoryParameterType.NVarchar, item.PayDate);
                        objDBAccess.AddParameterToCommand("@MaturityAmount", DBFactoryParameterType.Money, item.MaturityAmount);
                        objDBAccess.AddParameterToCommand("@MaturityDate", DBFactoryParameterType.NVarchar, item.MaturityDate);

                        int Result2 = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                    }

                    //if(model.MobileNo.Length == 10) { 
                    //    EsanchaarController EC = new EsanchaarController();
                    //    string ReturnVal = EC.SendSocietySMS("Dear "+ model.ComplainerName.ToString()+" S/O "+ model.ComplainerFatherName.ToString() + ", Your complaint has been registered successfully. Your comlaint number is "+ Result.ToString() + ".", model.MobileNo);
                    //}


                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "public int SaveCCSComplaint(CCSComplaintRegisterModel model)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public string ActiveCCSComplaintRegister(int ComplaintId, string MobileNo, string ComplainerName, string ComplainerFatherName, string ReceiptNo)
        {

            string Result = "-1";

            if (ComplaintId > 0)
            {
                try
                {
                    objDBAccess.OpenConnection();
                    SqlCommand cmd = objDBAccess.CreateCommand("UpdateCCSComplaintRegisterByKiosk", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@ComplaintId", DBFactoryParameterType.Int, ComplaintId);
                    dt = objDBAccess.GetTable();
                    objDBAccess.CloseConnection();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DResponse resp = JsonConvert.DeserializeObject<DResponse>(dt.Rows[0]["Result"].ToString());
                        Result = dt.Rows[0]["Result"].ToString();

                        if (MobileNo.Length == 10)
                        {
                            EsanchaarController EC = new EsanchaarController();
                            string ReturnVal = EC.SendSocietySMS("Dear " + ComplainerName.ToString() + " S/O " + ComplainerFatherName.ToString() + ", Your complaint has been registered successfully. Your comlaint number is " + ReceiptNo + ".", MobileNo);
                        }
                    }

                    Result = dt.Rows[0]["Result"].ToString();

                }
                catch (Exception ex)
                {
                    objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "ActiveCCSComplaintRegister(ComplaintId)" }, ex);

                }
                finally
                {
                    objDBAccess.CloseConnection();

                }
            }

            return Result;
        }

        public DataTable GetCCSComplaintRegisterForCitizen(int ComplaintId)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetCCSComplaintRegisterForCitizen", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ComplaintId", DBFactoryParameterType.Int, ComplaintId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "public DataTable GetCCSComplaintRegisterForCitizen(int ComplaintId)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable CCSComplaintListByAADHAR(string AADHAREnc, int ComplaintId, int SearchType, string ComplaintIdEnc = "")
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("CCSComplaintListByAADHAR", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@AADHAREnc", DBFactoryParameterType.NVarchar, AADHAREnc);
                objDBAccess.AddParameterToCommand("@ComplaintId", DBFactoryParameterType.Int, ComplaintId);
                objDBAccess.AddParameterToCommand("@SearchType", DBFactoryParameterType.Int, SearchType);
                objDBAccess.AddParameterToCommand("@ComplaintIdEnc", DBFactoryParameterType.NVarchar, ComplaintIdEnc);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "publicpublic DataTable CCSComplaintListByAADHAR(string AADHAREnc,int ComplaintId,int SearchType,string ComplaintIdEnc)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int UpdateCCSComplaintFile(string ComplaintIdEnc, string FilePath)
        {
            int Result = -1;
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateCCSComplaintFile", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ComplaintIdEnc", DBFactoryParameterType.NVarchar, ComplaintIdEnc);
                objDBAccess.AddParameterToCommand("@FilePath", DBFactoryParameterType.NVarchar, FilePath);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "public int UpdateCCSComplaintFile(string ComplaintIdEnc,string FilePath)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetCCSDetails(int SearchType, string AADHAREnc = "", int ComplaintId = 0, string ComplaintIdEnc = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CCSComplaintListByAADHAR", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@AADHAREnc", DBFactoryParameterType.NVarchar, AADHAREnc);
                objDBAccess.AddParameterToCommand("@ComplaintId", DBFactoryParameterType.Int, ComplaintId);
                objDBAccess.AddParameterToCommand("@SearchType", DBFactoryParameterType.Int, SearchType);
                objDBAccess.AddParameterToCommand("@ComplaintIdEnc", DBFactoryParameterType.NVarchar, ComplaintIdEnc);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahkaar.Models.CitizenModel", FunctionName = "public DataTable GetCCSDetails(int SearchType, string AADHAREnc=\"\",int ComplaintId=0, string ComplaintIdEnc=\"\")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public int CCSComplaintAlreadyEdited(string ComplaintIdEnc)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CCSComplaintAlreadyEdited", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ComplaintIdEnc", DBFactoryParameterType.NVarchar, ComplaintIdEnc);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahkaar.Models.CitizenModel", FunctionName = "public int CCSComplaintAlreadyEdited(string ComplaintIdEnc)" }, ex);
                Result = -1;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public int EditCCSComplaint(CCSComplaintRegisterModel model)
        {
            int Result = -1;

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CCSComplaintEdit", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ComplaintIdEnc", DBFactoryParameterType.NVarchar, model.ComplaintIdEnc);
                objDBAccess.AddParameterToCommand("@ComplainerName", DBFactoryParameterType.NVarchar, model.ComplainerName);
                objDBAccess.AddParameterToCommand("@ComplainerFatherName", DBFactoryParameterType.NVarchar, model.ComplainerFatherName);
                objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, model.Address);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, model.DistrictId);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, model.MobileNo);
                objDBAccess.AddParameterToCommand("@Email", DBFactoryParameterType.NVarchar, model.Email);
                objDBAccess.AddParameterToCommand("@CCSId", DBFactoryParameterType.Int, model.CCSId);
                objDBAccess.AddParameterToCommand("@BranchName", DBFactoryParameterType.NVarchar, model.BranchName);
                objDBAccess.AddParameterToCommand("@AccountNo", DBFactoryParameterType.NVarchar, model.AccountNo);
                objDBAccess.AddParameterToCommand("@IFSCCode", DBFactoryParameterType.NVarchar, model.IFSCCode);
                objDBAccess.AddParameterToCommand("@AmountDeposited", DBFactoryParameterType.Money, model.AmountDeposited);
                objDBAccess.AddParameterToCommand("@DepositDate", DBFactoryParameterType.NVarchar, model.DepositDate);
                objDBAccess.AddParameterToCommand("@AADHAR", DBFactoryParameterType.NVarchar, model.AADHAR);
                objDBAccess.AddParameterToCommand("@CreateByAADHAR", DBFactoryParameterType.NVarchar, model.AADHAR);
                objDBAccess.AddParameterToCommand("@CreateIP", DBFactoryParameterType.NVarchar, model.CreateIP);
                objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, model.UserType);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, (string.Equals(model.UserType.ToUpper(), "C") ? true : false));
                objDBAccess.AddParameterToCommand("@SSOId", DBFactoryParameterType.NVarchar, model.SSOId);
                objDBAccess.AddParameterToCommand("@MembershipId", DBFactoryParameterType.NVarchar, model.MembershipId);
                objDBAccess.AddParameterToCommand("@Remarks", DBFactoryParameterType.NVarchar, model.Remarks);
                objDBAccess.AddParameterToCommand("@TotalUploadedDocument", DBFactoryParameterType.Int, model.FileDetails.Count);
                objDBAccess.AddParameterToCommand("@AccountNoInSociety", DBFactoryParameterType.NVarchar, model.AccountNoInSociety);
                objDBAccess.AddParameterToCommand("@AccountIFSCInSociety", DBFactoryParameterType.NVarchar, model.AccountIFSCInSociety);
                objDBAccess.AddParameterToCommand("@AccountHolderName", DBFactoryParameterType.NVarchar, model.AccountHolderName);
                objDBAccess.AddParameterToCommand("@PassbookCrossCheque", DBFactoryParameterType.NVarchar, model.PassbookCrossCheque);
                objDBAccess.AddParameterToCommand("@BPNo", DBFactoryParameterType.NVarchar, model.BPNo);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                if (Result > 0)
                {
                    foreach (var item in model.FileDetails)
                    {
                        SqlCommand cmd2 = objDBAccess.CreateCommand("CCSComplaintDetailInsert", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@CCSComplaintRegisterId", DBFactoryParameterType.Int, Result);
                        objDBAccess.AddParameterToCommand("@FileTypeId", DBFactoryParameterType.Int, item.FileTypeId);
                        objDBAccess.AddParameterToCommand("@FilePath", DBFactoryParameterType.NVarchar, item.FilePath);
                        objDBAccess.AddParameterToCommand("@AccountNo", DBFactoryParameterType.NVarchar, item.AccountNo);
                        objDBAccess.AddParameterToCommand("@PaidAmount", DBFactoryParameterType.Money, item.PaidAmount);
                        objDBAccess.AddParameterToCommand("@PayDate", DBFactoryParameterType.NVarchar, item.PayDate);
                        objDBAccess.AddParameterToCommand("@MaturityAmount", DBFactoryParameterType.Money, item.MaturityAmount);
                        objDBAccess.AddParameterToCommand("@MaturityDate", DBFactoryParameterType.NVarchar, item.MaturityDate);
                        objDBAccess.AddParameterToCommand("@OldComolaintIdEnc", DBFactoryParameterType.NVarchar, model.ComplaintIdEnc);

                        int Result2 = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahakar.Models.CitizenModel", FunctionName = "public int EditCCSComplaint(CCSComplaintRegisterModel model)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetSocityDataForOtpSending2(string WhereClause)
        {

            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetDetailsForMapSociety", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetSocityDataForOtpSending2(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable GetCCSConsolidatedReportData(ref DataPagingModel Details, string fromDate, string toDate, int districtId, int CCSId, int LoggedInUserDistrictId, int LoggedInUserMappingId, int Type=-1)
        {
            dt.Clear();
            try
            {
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();

                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("CCSConsolidatedReportData", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.NVarchar, fromDate);
                    objDBAccess.AddParameterToCommand("@Todate", DBFactoryParameterType.NVarchar, toDate);
                    objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                    objDBAccess.AddParameterToCommand("@CCSId", DBFactoryParameterType.Int, CCSId);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@LoggedInUserDistrictId", DBFactoryParameterType.Int, LoggedInUserDistrictId);
                    objDBAccess.AddParameterToCommand("@LoggedInUserMappingId", DBFactoryParameterType.Int, LoggedInUserMappingId);
                    objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.VARCHAR, Type);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }

                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                objDBAccess.OpenConnection();
                objDBAccess.CreateCommand("CCSConsolidatedReportData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FromDate", DBFactoryParameterType.NVarchar, fromDate);
                objDBAccess.AddParameterToCommand("@Todate", DBFactoryParameterType.NVarchar, toDate);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@CCSId", DBFactoryParameterType.Int, CCSId);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@LoggedInUserDistrictId", DBFactoryParameterType.Int, LoggedInUserDistrictId);
                objDBAccess.AddParameterToCommand("@LoggedInUserMappingId", DBFactoryParameterType.Int, LoggedInUserMappingId);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.VARCHAR, Type);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahkaar.Models.CitizenModel", FunctionName = "public DataTable GetCCSConsolidatedReportData(string fromDate, string toDate, int districtId, int CCSId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public int SetFinalLockOnCCSComplaint(string ComplaintIdEnc)
        {
            int Result = -1;

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CCSComplaintFinalLock", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ComplaintIdEnc", DBFactoryParameterType.NVarchar, ComplaintIdEnc);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahakar.Models.CitizenModel", FunctionName = "SetFinalLockOnCCSComplaint(string ComplaintIdEnc)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public int InserBioKYCLog(string RequestXml, string ResponceXml,string aadhar_validate_xml,string error, int logId)
        {
            int Result = 0;
            dt.Clear();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                string status = "";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertBIOKYCLog", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@log_id", DBFactoryParameterType.NVarchar, logId);
                objDBAccess.AddParameterToCommand("@request_xml", DBFactoryParameterType.NVarchar, RequestXml);
                objDBAccess.AddParameterToCommand("@aadhar_validate_xml", DBFactoryParameterType.NVarchar, aadhar_validate_xml);
                objDBAccess.AddParameterToCommand("@responce_xml", DBFactoryParameterType.NVarchar, ResponceXml);
                objDBAccess.AddParameterToCommand("@error", DBFactoryParameterType.NVarchar, error);
                objDBAccess.AddParameterToCommand("@status", DBFactoryParameterType.NVarchar, status);
                objDBAccess.AddParameterToCommand("@sso_id", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                Result =Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "public bool InserBioKYCLOg(string RequestXml, string ResponceXml)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        //public DataTable GetCCSDetails(int SearchType, string AADHAREnc = "", int ComplaintId = -1)
        //{
        //    dt.Clear();
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("CCSComplaintListByAADHAR", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@AADHAREnc", DBFactoryParameterType.NVarchar, AADHAREnc);
        //        objDBAccess.AddParameterToCommand("@ComplaintId", DBFactoryParameterType.Int, ComplaintId);
        //        objDBAccess.AddParameterToCommand("@SearchType", DBFactoryParameterType.Int, SearchType);
        //        objDBAccess.AddParameterToCommand("@ComplaintIdEnc", DBFactoryParameterType.NVarchar);
        //        dt = objDBAccess.GetTable();
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahkaar.Models.CitizenModel", FunctionName = "public DataTable GetCCSDetails(int SearchType, string AADHAREnc=\"\", int ComplaintId=-1)" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }

        //    return dt;
        //}

        public DataSet GetDeshboardData_scheduler()
        {

            dt.Clear();

            DataSet dset = new DataSet();
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetDeshboardData_scheduler", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserRoleDeptMappingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                dset = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetDeshboardData_scheduler()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dset;
        }

        public DataTable GetCCSCountdata(int DistrictId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetCompaintcount", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "BankstamentList(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

    }


}
