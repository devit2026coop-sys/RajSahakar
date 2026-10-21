using Cooperative.Controllers;
using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Cooperative.Models
{
    public class InspectionModel
    {
        SqlTransaction Xsaction;
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

        public InspectionModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public DataTable InspectionData(SocietyRegistrationEntity Obj)
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
                WhereClause += Obj.InspectionID > 0 ? (WhereClause = " and Inspections.[InspectionID] =" + Obj.InspectionID + "") : "";

                if (Obj.SocietyIDEnc != null)
                {
                    WhereClause += Obj.SocietyIDEnc.Length > 0 ? (WhereClause = " and Societies.[SocietyID] =cast( Dbo.Decrypt('" + Obj.SocietyIDEnc + "')as int) ") : "";
                }

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyListForInspection", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.InspectionModel", FunctionName = "DataTable InspectionData(SocietyRegistrationEntity Obj)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public string AddInspection(InspectiondataEntity _ObjEntity)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertInspections", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, _ObjEntity.SocietyIdEnc);
                objDBAccess.AddParameterToCommand("@Year", DBFactoryParameterType.Int, _ObjEntity.Year);
                objDBAccess.AddParameterToCommand("@Tenure", DBFactoryParameterType.Int, _ObjEntity.Tenure);
                objDBAccess.AddParameterToCommand("@StartDate", DBFactoryParameterType.DateTime, _ObjEntity.DateOfStartInspection.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                objDBAccess.AddParameterToCommand("@EndDate", DBFactoryParameterType.DateTime, _ObjEntity.DateOfEndInspection.ToString("dd/MM/yyyy").ConvertString_ToValidDate());
                objDBAccess.AddParameterToCommand("@Admin", DBFactoryParameterType.NVarchar, _ObjEntity.Admin);
                objDBAccess.AddParameterToCommand("@Fincial", DBFactoryParameterType.NVarchar, _ObjEntity.Financial);
                objDBAccess.AddParameterToCommand("@TrandBusiness", DBFactoryParameterType.NVarchar, _ObjEntity.TradeBusiness);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, _ObjEntity.Remark);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, _ObjEntity.IsActive);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _ObjEntity.CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, _ObjEntity.UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@InspOfficerSocietyId", DBFactoryParameterType.NVarchar, _ObjEntity.InspOfficerSocietyId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.InspectionModel", FunctionName = "AddInspection(InspectiondataEntity _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return COMMON_FUNCTION.Encrypt(Result.ToString());
        }

        public int InsertInspectionReply(InspectionAnsSave ObjInspectionAnsSave)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                //Xsaction = objDBAccess.StartTransaction();
                //SqlTransaction Tran = objDBAccess.GetTransaction; 
                foreach (InspectionAns objInspectionAns in ObjInspectionAnsSave.InspectionAnsList)
                {
                    objDBAccess.CreateCommand("InsertInspectionAns", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@AnswerId", DBFactoryParameterType.BigInt, objInspectionAns.AnswerId);
                    objDBAccess.AddParameterToCommand("@InspectionId", DBFactoryParameterType.NVarchar, objInspectionAns.InspectionId);
                    objDBAccess.AddParameterToCommand("@QuestionId", DBFactoryParameterType.NVarchar, objInspectionAns.QuestionId);
                    objDBAccess.AddParameterToCommand("@Answear", DBFactoryParameterType.NVarchar, objInspectionAns.Answear);
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.NVarchar, ObjInspectionAnsSave.InspectionAnsList[0].CreatedBy);
                    objDBAccess.AddParameterToCommand("@IsObjection", DBFactoryParameterType.Int, objInspectionAns.IsObjection);
                    objDBAccess.AddParameterToCommand("@SSO_ID", DBFactoryParameterType.NVarchar, objInspectionAns.SSO_ID);
                    objDBAccess.AddParameterToCommand("@CEORemark", DBFactoryParameterType.NVarchar, objInspectionAns.CEORemark);
                    objDBAccess.AddParameterToCommand("@InspectorRemark", DBFactoryParameterType.NVarchar, objInspectionAns.InspectorRemark);
                    objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                    objDBAccess.AddParameterToCommand("@Isliquideter", DBFactoryParameterType.Int, objInspectionAns.Isliquideter);
                    objDBAccess.AddParameterToCommand("@Isliquideterremark", DBFactoryParameterType.NVarchar, objInspectionAns.Isliquideterremark);
                    objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.Int, objInspectionAns.UserType);



                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }
                if (ObjInspectionAnsSave.TableHeaderAnsList != null)
                {
                    int QuestionId = 0;
                    foreach (TableHeaderAns objTableHeaderAns in ObjInspectionAnsSave.TableHeaderAnsList)
                    {
                        if (QuestionId != objTableHeaderAns.QuestionId)
                        {
                            QuestionId = objTableHeaderAns.QuestionId;
                            objTableHeaderAns.IsDeleted = 1;
                        }
                        else
                            objTableHeaderAns.IsDeleted = 0;

                        SqlCommand cmd = objDBAccess.CreateCommand("InsertTableHeaderAns", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@AnswerId", DBFactoryParameterType.BigInt, objTableHeaderAns.AnswerId);
                        objDBAccess.AddParameterToCommand("@QuestionId", DBFactoryParameterType.NVarchar, objTableHeaderAns.QuestionId);
                        objDBAccess.AddParameterToCommand("@TableId", DBFactoryParameterType.BigInt, objTableHeaderAns.TableId);
                        objDBAccess.AddParameterToCommand("@Answear", DBFactoryParameterType.NVarchar, objTableHeaderAns.Answear);
                        objDBAccess.AddParameterToCommand("@InspectionId", DBFactoryParameterType.NVarchar, objTableHeaderAns.InspectionId);
                        objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.NVarchar, ObjInspectionAnsSave.InspectionAnsList[0].CreatedBy);
                        objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                        objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.Int, objTableHeaderAns.UserType);
                        objDBAccess.AddParameterToCommand("@IsDeleted", DBFactoryParameterType.Int, objTableHeaderAns.IsDeleted);
                        objDBAccess.AddParameterToCommand("@CurrentUserType", DBFactoryParameterType.Int, ObjInspectionAnsSave.InspectionAnsList[0].CurrentUserType);

                        //@CurrentUserType
                        Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                    }
                }

                //Xsaction.Commit();

            }
            catch (Exception ex)
            {
                //Xsaction.Rollback();
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.InspectionModel", FunctionName = "InsertInspectionReply(InspectionAnsSave ObjInspectionAnsSave)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable InspectionQuestionData(int Inspectionid)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("QuestionsForInspectionBySocietyTypeId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@InspectionID", DBFactoryParameterType.Int, Inspectionid);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.InspectionModel", FunctionName = "InspectionQuestionData(int Inspectionid)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetTableHeaderDataByQuestionId(string QuestionId, int Inspectionid)
        {
            DataTable TableHeaserDT = null;

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("TableheaderByQuestionId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@QuestionId", DBFactoryParameterType.Int, int.Parse(QuestionId));
                objDBAccess.AddParameterToCommand("@InspectionID", DBFactoryParameterType.Int, Inspectionid);
                TableHeaserDT = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.InspectionModel", FunctionName = "GetTableHeaderDataByQuestionId(string QuestionId, int Inspectionid)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return TableHeaserDT;
        }

        #region Inspection Init

        public DataTable InspectionForSocietyOfUser(int SocietyTypeId, int ClassId, int FYID = 0)
        {
            dt.Clear();
            try
            {
                string WhereClause = "";
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyListForInspectionByUserId", DBFactoryCommandType.PROCEDURE_COMMAND);
                if (_LoggedInUser.UserType == "C")
                    WhereClause += " AND Societies.SSOID='" + _LoggedInUser.SSOID + "' ";
                else
                    WhereClause = " AND IOS.OfficerId=" + _LoggedInUser.UserId;


                if (SocietyTypeId != 0)
                    WhereClause += " AND Societies.SocietyTypeId='" + SocietyTypeId.ToString() + "' ";

                if (ClassId != 0)
                    WhereClause += " AND Societies.SSOID='" + ClassId.ToString() + "' ";
                //  Societies.ClassId,Societies.SubClassId


                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                objDBAccess.AddParameterToCommand("@FYID", DBFactoryParameterType.NVarchar, FYID.ToString());


                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.InspectionModel", FunctionName = "DataTable InspectionForSocietyOfUser()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetSocietyListPrimary(string FYID = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetPrimarySocietyListByRoleIdUserID", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@FYearId", DBFactoryParameterType.Int, int.Parse(FYID));
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.InspectionModel", FunctionName = "GetSocietyListPrimary()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetInspectionOfficerForSociety()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetInspectionOfficerForSocietyByRoleIdUserID", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "GetInspectionOfficerForSociety()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public string SaveInspectionOfficerSocietyData(List<InspectionBasicData> onjInspectionBasicData)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertInspectionMst", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TotSocietyNo", DBFactoryParameterType.Int, onjInspectionBasicData.Count);
                objDBAccess.AddParameterToCommand("@FYearId", DBFactoryParameterType.NVarchar, onjInspectionBasicData[0].FYearId);
                objDBAccess.AddParameterToCommand("@InsertBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@InsertIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@Mappingid", DBFactoryParameterType.Int, _LoggedInUser.UserRoleDeptMappingId);
                int InspectionMstId = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                if (InspectionMstId > 0)
                {
                    foreach (var Obj in onjInspectionBasicData)
                    {
                        cmd = objDBAccess.CreateCommand("InsertInspOfficerSociety", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@InspectionMstId", DBFactoryParameterType.Int, InspectionMstId);
                        objDBAccess.AddParameterToCommand("@OfficerId", DBFactoryParameterType.Int, Obj.OfficerId);
                        objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, Obj.SocietyIDEnc);
                        objDBAccess.AddParameterToCommand("@InsertBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                        objDBAccess.AddParameterToCommand("@InsertIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                        Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                    }
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "SaveInspectionOfficerSocietyData(List<InspectionBasicData> onjInspectionBasicData)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return COMMON_FUNCTION.Encrypt(Result.ToString());
        }
        #endregion

        public int UploadInspectionFile(InspectiondataEntity _ObjEntity)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UploadInspectionFile", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@InspectionID", DBFactoryParameterType.NVarchar, _ObjEntity.InspectionID);
                objDBAccess.AddParameterToCommand("@FilePath", DBFactoryParameterType.NVarchar, _ObjEntity.InspectionFile);
                objDBAccess.AddParameterToCommand("@FileRemark", DBFactoryParameterType.NVarchar, _ObjEntity.InspectionFileRemark);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.InspectionModel", FunctionName = "UploadInspectionFile(InspectiondataEntity _ObjEntity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetSocietyInspectionAuditStatus(int LevelId, int UserId, int YearId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAuditStatusReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, LevelId);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, UserId);
                objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, YearId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Auditor", FunctionName = "DataTable GetSocietyInspectionAuditStatus(int LevelId, int UserId, int YearId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }

        public DataTable GetSocietyDataByLevelIdYearId(int LevelId, int UserId, int YearId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetInspectionReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, LevelId);
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, UserId);
                objDBAccess.AddParameterToCommand("@FinancialYearId", DBFactoryParameterType.Int, YearId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Auditor", FunctionName = "DataTable GetSocietyDataByLevelIdYearId(int LevelId,int UserId,int YearId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }

        public DataSet GetAuditorDashBoardData(int _UserLevel, int _Id, int _ActionId,int _ActId=0)
        {
            DataSet dset = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAuditorDashBoardData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UserLevel", DBFactoryParameterType.Int, _UserLevel);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, _Id);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, _ActionId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, _ActId);
                dset = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.InspectionModel", FunctionName = "public DataSet GetAuditorDashBoardData(int _UserLevel, int _Id,int _ActionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dset.Dispose();
            }
            return dset;
        }

        public DataSet GetAuditorDashBoardDataNew(int UserRoleMappingId,int ActId)
        {
            DataSet dset = new DataSet();
            string query = "";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAuditorDashBoardDataNew", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.Int, UserRoleMappingId);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, ActId);
                query = objDBAccess.getSqlCommandText();
                dset = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.InspectionModel", FunctionName = "public DataSet GetAuditorDashBoardDataNew(int UserRoleMappingId,int ActId)" + query }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dset.Dispose();
            }
            return dset;
        }
    }
}