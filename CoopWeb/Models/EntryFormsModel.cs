using Cooperative.Controllers;
using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Cooperative.Models
{
    public class EntryFormsModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public EntryFormsModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }
        #region BhandarDataEntity
        public int InsertBhandarData(BhandarFormDataEntity _Model)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertBhandarFormData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@BhandarDataId", DBFactoryParameterType.BigInt, _Model.BhandarDataId);
                objDBAccess.AddParameterToCommand("@BhandarId", DBFactoryParameterType.Int, _Model.BhandarId);
                objDBAccess.AddParameterToCommand("@FYearId", DBFactoryParameterType.Int, _Model.FYearId);
                objDBAccess.AddParameterToCommand("@MonthId", DBFactoryParameterType.Int, _Model.MonthId);
                objDBAccess.AddParameterToCommand("@ControlledBusinessTarget", DBFactoryParameterType.Money, _Model.ControlledBusinessTarget);
                objDBAccess.AddParameterToCommand("@UnControlledBusinessTarget", DBFactoryParameterType.Money, _Model.UnControlledBusinessTarget);
                objDBAccess.AddParameterToCommand("@ControlledBusinessfulfillment", DBFactoryParameterType.Money, _Model.ControlledBusinessfulfillment);
                objDBAccess.AddParameterToCommand("@UnControlledBusinessFulfillment", DBFactoryParameterType.Money, _Model.UnControlledBusinessFulfillment);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, _Model.CreatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, _Model.CreatedIP);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.BigInt, _Model.IsActive);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormsModel", FunctionName = "InsertBhandarData(BhandarFormDataEntity _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public DataTable BhandarMprDataList(BhandarFormDataEntity _Model)
        {
            dt.Clear();
            try
            {
                string WhereClause = " Where IsActive in (0,1) ";
                if (_Model.BhandarId > 0)
                {
                    WhereClause += " AND BhandarId = " + _Model.BhandarId;
                }
                if (_Model.FYearId > 0)
                {
                    WhereClause += " AND FYearId = " + _Model.FYearId;
                }
                if (_Model.MonthId > 0)
                {
                    WhereClause += " AND MonthId = " + _Model.MonthId;
                }
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("BhandarMprDataList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "BhandarMprDataList(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        //public int DeleteBhandarData(BhandarFormDataEntity _Model)
        //{
        //    int Result = 0;
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("DeleteStautsTrainning", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@BhandarDataId", DBFactoryParameterType.BigInt, _Model.BhandarDataId);
        //        objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.BigInt, _Model.IsActive);

        //        Result = objDBAccess.ExecuteCommand();
        //    }
        //    catch (Exception ex)
        //    {
        //        DBLogger objDBLogger = new DBLogger();
        //        objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.TrainningModel", FunctionName = "DeleteChangeStatusTrainning(int TrainningId, int Action)" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return Result;
        //}
        public DataTable BhandarMprDataForDashboard(BhandarFormDataEntity _Model)
        {
            dt.Clear();
            try
            {
                string WhereClause = " WHERE BhandarMprData.IsActive =1 ";
                if (_Model.BhandarId > 0)
                {
                    WhereClause += " AND BhandarId = " + _Model.BhandarId;
                }
                if (_Model.FYearId > 0)
                {
                    WhereClause += " AND FIN_YEAR_MASTR_ID = " + _Model.FYearId;
                }
                if (_Model.MonthId > 0)
                {
                    WhereClause += " AND MonthId = " + _Model.MonthId;
                }
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("BhandarMprDataForDashboard", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "BhandarMprDataForDashboard" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable BhandarDashboardData(int DistrictId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("BhandarDashBoardData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "BhandarDashboardData(int DistrictId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        #endregion

        #region KVSS

        public DataTable KVSSociety(int KVSSID = 0, int DistrictId = 0)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetKVSSociety", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@KVSSID", DBFactoryParameterType.Int, KVSSID);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "KVSSData(int KVSSID=0,int DistrictId=0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable KVSSData(KVSSDataEntity _Model)
        {
            dt.Clear();
            try
            {
                string WhereClause = " and  kd.IsActive in (0,1) ";
                if (_Model.KVSSId > 0)
                {
                    WhereClause += " AND  kd.KVSSId = " + _Model.KVSSId;
                }
                if (_Model.FYearId > 0)
                {
                    WhereClause += " AND  kd.FYearId = " + _Model.FYearId;
                }
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SelectKVSSData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "BhandarMprDataList(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int SaveKVSSFormsData(KVSSDataEntity _Model)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertKVSSData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@KVSSDataId", DBFactoryParameterType.Int, _Model.KVSSDataId);
                objDBAccess.AddParameterToCommand("@KVSSId", DBFactoryParameterType.Int, _Model.KVSSId);
                objDBAccess.AddParameterToCommand("@FYearId", DBFactoryParameterType.Int, _Model.FYearId);
                objDBAccess.AddParameterToCommand("@AgriProduction", DBFactoryParameterType.Money, _Model.AgriProduction);
                objDBAccess.AddParameterToCommand("@AgriAadan", DBFactoryParameterType.Money, _Model.AgriAadan);
                objDBAccess.AddParameterToCommand("@MaterialControl", DBFactoryParameterType.Money, _Model.MaterialControl);
                objDBAccess.AddParameterToCommand("@MaterialMedical", DBFactoryParameterType.Money, _Model.MaterialMedical);
                objDBAccess.AddParameterToCommand("@MaterialUncontrol", DBFactoryParameterType.Money, _Model.MaterialUncontrol);
                objDBAccess.AddParameterToCommand("@InsertIP", DBFactoryParameterType.NVarchar, _Model.InsertIP);
                objDBAccess.AddParameterToCommand("@InsertBy", DBFactoryParameterType.BigInt, _Model.InsertBy);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, _Model.IsActive);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormsModel", FunctionName = "InsertBhandarData(BhandarFormDataEntity _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataTable GetGSSocietyList(string WhereClause)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetGSSList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "KVSSData(int KVSSID=0,int DistrictId=0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        #endregion


        #region StLoanDataEntity
        public int SaveStLoanFormsData(StLoanFormDataEntity _Model)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertStLoanData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FID", DBFactoryParameterType.BigInt, _Model.FIN_YEAR_MASTR_ID);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, _Model.ApexLoanDisbursementFYearId);
                objDBAccess.AddParameterToCommand("@TotalTarget", DBFactoryParameterType.Int, _Model.TotalTarget);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, _Model.CreatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, _Model.CreatedIP);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.BigInt, 1);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                if (Result > 0 && _Model.StLoanFormDataTranlst.Count > 0)
                {
                    foreach (StLoanFormDataTranEntity OBjlst in _Model.StLoanFormDataTranlst)
                    {
                        SqlCommand cmd1 = objDBAccess.CreateCommand("InsertStLoanTranData", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@DccbId", DBFactoryParameterType.BigInt, OBjlst.CCBId);
                        objDBAccess.AddParameterToCommand("@TransactionId", DBFactoryParameterType.BigInt, OBjlst.ApexLoanDisbursementTranID);
                        objDBAccess.AddParameterToCommand("@Total_Application_Received", DBFactoryParameterType.Int, Result);
                        objDBAccess.AddParameterToCommand("@Approved_By_Pacs", DBFactoryParameterType.Int, OBjlst.Approved_By_Pacs);
                        objDBAccess.AddParameterToCommand("@Approved_By_Branch", DBFactoryParameterType.Int, OBjlst.Approved_By_Branch);
                        objDBAccess.AddParameterToCommand("@DMR_Credited_And_Loan_Disbursement", DBFactoryParameterType.Money, OBjlst.DMR_Credited_And_Loan_Disbursement);
                        objDBAccess.AddParameterToCommand("@TillDate", DBFactoryParameterType.Date, DateTime.Now);
                        objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, _Model.CreatedBy);
                        objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, _Model.CreatedIP);
                        objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.BigInt, 1);
                        Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                    }
                }


            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormsModel", FunctionName = "InsertBhandarData(BhandarFormDataEntity _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int SaveStLoanFormsDataNew(StLoanlstEntity _Model)
        {
            int Result = 0;
            try
            {

                if (_Model.Table.Count > 0)
                {
                    objDBAccess.OpenConnection();
                    foreach (StLoanEntity OBjlst in _Model.Table)
                    {
                        SqlCommand cmd1 = objDBAccess.CreateCommand("InsertStLoanTranData", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@DccbId", DBFactoryParameterType.BigInt, 0);
                        objDBAccess.AddParameterToCommand("@DisName", DBFactoryParameterType.NVarchar, OBjlst.DISTRICT);
                        objDBAccess.AddParameterToCommand("@Total_Application_Received", DBFactoryParameterType.Int, OBjlst.TotalRegistrations);
                        objDBAccess.AddParameterToCommand("@Approved_By_Pacs", DBFactoryParameterType.Int, int.Parse(OBjlst.ApprovedByPacs));
                        objDBAccess.AddParameterToCommand("@Approved_By_Branch", DBFactoryParameterType.Int, int.Parse(OBjlst.ApprovedByBranch));
                        objDBAccess.AddParameterToCommand("@DMR_Credited_And_Loan_Disbursement", DBFactoryParameterType.Money, decimal.Parse(OBjlst.DMRCreated_LoanDisbursed));
                        //objDBAccess.AddParameterToCommand("@TillDate", DBFactoryParameterType.Date, OBjlst.TillDate);
                        objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, 0);
                        objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, "");
                        objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.BigInt, 1);
                        objDBAccess.AddParameterToCommand("@MCLAMOUNT", DBFactoryParameterType.Money, decimal.Parse(OBjlst.MCLAMOUNT));
                        objDBAccess.AddParameterToCommand("@SANCTIONAMOUNT", DBFactoryParameterType.Money, decimal.Parse(OBjlst.SANCTIONAMOUNT));
                        var Result2 = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                    }
                }


            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormsModel", FunctionName = "InsertBhandarData(BhandarFormDataEntity _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public int SaveMSP(List<MSP> LSTMSP)
        {
            int Result = 0;
            try
            {


                objDBAccess.OpenConnection();
                foreach (MSP OBjlst in LSTMSP)
                {
                    SqlCommand cmd1 = objDBAccess.CreateCommand("InsertRAJ_NEW", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@PurchaseCenter_Kendra", DBFactoryParameterType.NVarchar, OBjlst.PurchaseCenter_Kendra);
                    objDBAccess.AddParameterToCommand("@KVSSSamiti_Name", DBFactoryParameterType.NVarchar, OBjlst.KVSSSamiti_Name);
                    objDBAccess.AddParameterToCommand("@KVSSSamiti_Name_EN", DBFactoryParameterType.NVarchar, OBjlst.KVSSSamiti_Name_EN);
                    objDBAccess.AddParameterToCommand("@District_Name_EN", DBFactoryParameterType.NVarchar, OBjlst.District_Name_EN);
                    objDBAccess.AddParameterToCommand("@Districtid", DBFactoryParameterType.Int, OBjlst.Districtid);
                    objDBAccess.AddParameterToCommand("@COMMODITY", DBFactoryParameterType.NVarchar, OBjlst.COMMODITY);
                    objDBAccess.AddParameterToCommand("@PurchaseCenterName_En", DBFactoryParameterType.NVarchar, OBjlst.PurchaseCenterName_En);
                    objDBAccess.AddParameterToCommand("@PurchaseCenterName_Hi", DBFactoryParameterType.NVarchar, OBjlst.PurchaseCenterName_Hi);
                    objDBAccess.AddParameterToCommand("@PurchaseCenter_ID", DBFactoryParameterType.Int, OBjlst.PurchaseCenter_ID);
                    var Result2 = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                }



            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormsModel", FunctionName = "InsertBhandarData(BhandarFormDataEntity _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable CCBData(int CCBId = 0, bool IsSelectActive = true)
        {
            dt.Clear();
            try
            {
                string WhereClause = "  ";
                if (CCBId > 0)
                {
                    WhereClause += " AND  CCB.CCBId = " + CCBId.ToString();
                }
                if (IsSelectActive)
                {
                    WhereClause += " AND  CCB.IsActive in (1) ";
                }
                else
                {
                    WhereClause += " AND  CCB.IsActive in (0,1) ";
                }
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SelectCCBData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "BhandarMprDataList(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        #endregion

        #region GCC Pankaj
        public DataTable CCBData_GCC(int CCBId = 0, int DistrictId = 0, bool IsSelectActive = true)
        {
            dt.Clear();
            try
            {
                string WhereClause = "  ";
                if (CCBId > 0)
                {
                    WhereClause += " AND  CCB.CCBId = " + CCBId.ToString();
                }
                if (DistrictId > 0)
                {
                    WhereClause += " AND  CCB.DistrictId = " + DistrictId.ToString();
                }
                if (IsSelectActive)
                {
                    WhereClause += " AND  CCB.IsActive in (1) ";
                }
                else
                {
                    WhereClause += " AND  CCB.IsActive in (0,1) ";
                }
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SelectCCBData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "CCBData_GCC(int CCBId = 0, int DistrictId = 0, bool IsSelectActive = true)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        #endregion

        #region KVSS

        //public DataTable KVSSociety(int KVSSID = 0, int DistrictId = 0)
        //{
        //    dt.Clear();
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("GetKVSSociety", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@KVSSID", DBFactoryParameterType.Int, KVSSID);
        //        objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
        //        dt = objDBAccess.GetTable();
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "KVSSData(int KVSSID=0,int DistrictId=0)" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return dt;
        //}

        public DataTable GetFertiMNGReportList(FertiMngReport _Model)
        {
            dt.Clear();
            try
            {
                string WhereClause = " and  FR.IsActive in (0,1) ";
                if (_Model.KVSSId > 0)
                {
                    WhereClause += " AND  FR.KVSSId = " + _Model.KVSSId;
                }
                if (_Model.FinYearId > 0)
                {
                    WhereClause += " AND  FR.FinYearId = " + _Model.FinYearId;
                }
                if (_Model.MonthId > 0)
                {
                    WhereClause += " AND  FR.MonthId = " + _Model.MonthId;
                }
                if (_Model.DistrictId > 0)
                {
                    WhereClause += " AND  FR.DistrictId = " + _Model.DistrictId;
                }
                if (_Model.FertiId > 0)
                {
                    WhereClause += " AND  FR.FertiId = " + _Model.FertiId;
                }
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetFertiMNGReportList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "BhandarMprDataList(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int SaveFertiMNGReport(FertiMngReport _Model)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertUpdateFertiMNGReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FertiMNGReportId", DBFactoryParameterType.Int, _Model.FertiMNGReportId);
                objDBAccess.AddParameterToCommand("@FinYearId", DBFactoryParameterType.Int, _Model.FinYearId);
                objDBAccess.AddParameterToCommand("@MonthId", DBFactoryParameterType.Int, _Model.MonthId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, _Model.DistrictId);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, _Model.BlockId);
                objDBAccess.AddParameterToCommand("@KVSSId", DBFactoryParameterType.Int, _Model.KVSSId);
                objDBAccess.AddParameterToCommand("@GPId", DBFactoryParameterType.Int, _Model.GPId);
                objDBAccess.AddParameterToCommand("@GSSId", DBFactoryParameterType.Int, _Model.GSSId);
                objDBAccess.AddParameterToCommand("@FertiId", DBFactoryParameterType.Int, _Model.FertiId);
                objDBAccess.AddParameterToCommand("@TotReceipt", DBFactoryParameterType.BigInt, _Model.TotReceipt);
                objDBAccess.AddParameterToCommand("@TotSale", DBFactoryParameterType.BigInt, _Model.TotSale);
                objDBAccess.AddParameterToCommand("@TotStockEnd", DBFactoryParameterType.BigInt, _Model.TotStockEnd);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, _Model.IsActive);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, _Model.CreatedBy);
                objDBAccess.AddParameterToCommand("@CreationDate", DBFactoryParameterType.DateTime, DateTime.Now);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, _Model.CreatedIP);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormsModel", FunctionName = "InsertBhandarData(BhandarFormDataEntity _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public int GetStockVal(FertiMngReport _Model)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetLastStockVal", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FinYearId", DBFactoryParameterType.Int, _Model.FinYearId);
                objDBAccess.AddParameterToCommand("@MonthId", DBFactoryParameterType.Int, _Model.MonthId);
                objDBAccess.AddParameterToCommand("@GSSId", DBFactoryParameterType.Int, _Model.GSSId);
                objDBAccess.AddParameterToCommand("@FertiId", DBFactoryParameterType.Int, _Model.FertiId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormsModel", FunctionName = "InsertBhandarData(BhandarFormDataEntity _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public int DeleteFertiMNGReport(int FertiMNGReportId)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteFertiMNGReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FertiMNGReportId", DBFactoryParameterType.Int, FertiMNGReportId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = "BhandarMprDataList(string SocietyIdEnc)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        #endregion

        #region Fertilizer Opening Stock

        public ResultMessage UploadFertilizerStock(DataTable Data, string Ondate)
        {
            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(objDBAccess.GetConnection))
            {
                try
                {

                    string date = Ondate.Split('/')[2] + "/" + Ondate.Split('/')[1] + "/" + Ondate.Split('/')[0];

                    System.Data.DataColumn CreateDate = new System.Data.DataColumn("Ondate", typeof(DateTime));
                    CreateDate.DefaultValue = Convert.ToDateTime(date);
                    Data.Columns.Add(CreateDate);


                    //DataTable orderedTable = new DataTable();
                    //orderedTable.Clear();
                    //orderedTable.Columns.Add("RetIdStr");
                    //orderedTable.Columns.Add("NatureName");
                    //orderedTable.Columns.Add("Fertilizer");
                    //orderedTable.Columns.Add("OpeningBalance");
                    //orderedTable.Columns.Add("Ondate");

                    //DataRow _row = orderedTable.NewRow();
                    //foreach (DataRow dr in Data.Rows)
                    //{
                    //    _row = orderedTable.NewRow();
                    //    _row["RetIdStr"] = dr["RetIdStr"].ToString().Trim(); ;
                    //    _row["NatureName"] = dr["NatureName"].ToString().Trim(); ;
                    //    _row["Fertilizer"] = dr["Fertilizer"].ToString().Trim(); ;
                    //    _row["OpeningBalance"] = dr["OpeningBalance"].ToString().Trim(); ;
                    //    _row["Ondate"] = dr["Ondate"].ToString().Trim();   
                    //    orderedTable.Rows.Add(_row);
                    //}


                    //DataTable dddd = new DataTable();
                    //int Result = 0;
                    //try
                    //{

                    //    objDBAccess.OpenConnection();
                    //    SqlCommand cmd = objDBAccess.CreateCommand("InsertSocietyMemberDetailsXL", DBFactoryCommandType.PROCEDURE_COMMAND);
                    //    //objDBAccess.AddParameterToCommand("@SocietyIdEnc", DBFactoryParameterType.NVarchar, _Model.AADHAARNo);
                    //    //objDBAccess.pa("@tbl",  data);
                    //   // cmd.Parameters.AddWithValue("@SocietyIdEnc", SocietyIdEnc);
                    //    cmd.Parameters.AddWithValue("@tbl", orderedTable);
                    //    cmd.ExecuteNonQuery();
                    //    //SqlDataAdapter _da = new SqlDataAdapter();
                    //    //_da = new SqlDataAdapter(cmd);
                    //    //_da.Fill(dddd);


                    //    //Result = Convert.ToInt32(cmd.ex());
                    //}
                    //catch (Exception ex)
                    //{
                    //    DBLogger objDBLogger = new DBLogger();
                    //    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "UploadMemberExcelData(DataTable data)" }, ex);
                    //}
                    //finally
                    //{
                    //    objDBAccess.CloseConnection();
                    //}


                    sqlBulkCopy.DestinationTableName = "dbo.RetailerOpeningStock";
                    objDBAccess.OpenConnection();
                    if (Data.Rows.Count > 0)
                    {
                        sqlBulkCopy.ColumnMappings.Add("RetIdStr", "RetIdStr");
                        sqlBulkCopy.ColumnMappings.Add("NatureName", "NatureName");
                        sqlBulkCopy.ColumnMappings.Add("Fertilizer", "Fertilizer");
                        sqlBulkCopy.ColumnMappings.Add("OpeningBalance", "OpeningBalance");
                        sqlBulkCopy.ColumnMappings.Add("Ondate", "Ondate");
                        sqlBulkCopy.WriteToServer(Data);



                        try
                        {
                            objDBAccess.OpenConnection();
                            SqlCommand cmd = objDBAccess.CreateCommand("UpdateRetailerOpeningStock", DBFactoryCommandType.PROCEDURE_COMMAND);
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            DBLogger objDBLogger = new DBLogger();
                            objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = " UploadFertilizerStock(DataTable Data, string Ondate)" }, ex);
                        }


                        objDBAccess.CloseConnection();





                        return CommonMessageFactory.GetJsonResponse("true", "Stock Data Successfully Uploaded");
                    }
                    return CommonMessageFactory.GetJsonResponse("false", "Data Not Uploaded");
                }
                catch (Exception ex)
                {
                    DBLogger objDBLogger = new DBLogger();
                    objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EntryFormModel", FunctionName = " UploadFertilizerStock(DataTable Data, string Ondate)" }, ex);
                    return CommonMessageFactory.GetJsonResponse("false", "Data Not Uploaded");
                }
            }

            //return "";
        }

        #endregion
    }
}