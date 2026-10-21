using System;
using Cooperative.DBConnector;
using System.Data;
using System.Web;
using System.Data.SqlClient;
using System.Collections.Generic;
using Cooperative.Controllers;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Web.Helpers;
using Cooperative.JanadhaarClasses;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

namespace Cooperative.Models
{
    public class CommonServicesModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public CommonServicesModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public int UpdatepermanentCaseRegNo(message objmessage)
        {
            dt = new DataTable();
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UpdateCaseRegno", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CaseID", DBFactoryParameterType.Int, 0);
                objDBAccess.AddParameterToCommand("@caseregno", DBFactoryParameterType.NVarchar, "");
                objDBAccess.AddParameterToCommand("@recordid", DBFactoryParameterType.NVarchar, objmessage.recordid);
                objDBAccess.AddParameterToCommand("@AppealNo", DBFactoryParameterType.NVarchar, objmessage.AppealNo);
                objDBAccess.AddParameterToCommand("@IsTemp", DBFactoryParameterType.NVarchar, 1);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.Int, objmessage.Staus);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServicesModel", FunctionName = "UpdatepermanentCaseRegNo(message objmessage)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataSet GetCaseDetailsByCaseId(int CaseRegId)
        {
            DataSet dset = new DataSet();

            objDBAccess.OpenConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = objDBAccess.CreateCommand("CaseDetailsByCaseId", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CaseRegID", DBFactoryParameterType.Int, CaseRegId);
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

        public DataTable GetAllCaseList(string SSOID)
        {
            dt.Clear();
            try
            {
                string WhereCluase = "Where SSOID='" + SSOID + "'  OR caseregno='" + SSOID + "'";

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetAllCaseDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereCluase);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServicesModel", FunctionName = "GetAllCaseList(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        //public DataTable SocietyListForAuditRPT(string SocietyName)
        //{
        //    dt.Clear();
        //    try
        //    {
        //        string WhereClause = "and SocietyName='" + SocietyName + "' OR BRN='" + SocietyName + "'  OR RegistrationNo='" + SocietyName + "'";

        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("SocietyListForAuditRPTWeb", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
        //        dt = objDBAccess.GetTable();
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CitizenModel", FunctionName = "SocietyListForAuditRPT(SocietyRegistrationEntity Obj)" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return dt;
        //}

        public DataTable GetAllTrainning(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetApprovedTrainningList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.TrainningModel", FunctionName = "GetAllTrainning(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetDataForMobileApp(int FIN_YEAR_ID)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("StLoneDataList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FID", DBFactoryParameterType.Int, FIN_YEAR_ID);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServicesModel", FunctionName = " public DataTable GetDataForMobileApp(int FIN_YEAR_ID)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable MobileAppFinancialYearList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("FinancialYearListMobile", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServicesModel", FunctionName = "FinancialYearList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


        public DataTable Mobile_GetFertiliserData(string StateWise, string FinancialYearId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("FertiliserCountForMobileApp", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@STATEWISE", DBFactoryParameterType.Int, int.Parse(StateWise));
                objDBAccess.AddParameterToCommand("@FinYearId", DBFactoryParameterType.Int, int.Parse(FinancialYearId));
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServicesModel", FunctionName = "Mobile_GetFertiliserData()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable Mobile_GetPMkisanDataForDashboard()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("PMkisanDataForDashboard", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServicesModel", FunctionName = "Mobile_GetPMkisanDataForDashboard()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable Mobile_GetOneFertiliserData(string FertiId, string FinancialYearId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("OneFertiliserCountForMobileApp", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FertiId", DBFactoryParameterType.Int, int.Parse(FertiId));
                objDBAccess.AddParameterToCommand("@FinYearId", DBFactoryParameterType.Int, int.Parse(FinancialYearId));
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServicesModel", FunctionName = "Mobile_GetFertiliserData()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable Mobile_GetDccbPacsMemberCountData()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Pacs_Member_Count_Report", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServicesModel", FunctionName = "Mobile_GetDccbPacsMemberCountData()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


        public int InsertPM_KIssanData(List<PM_KISAN_Entity> PM_KISAN_EntityList)
        {
            int Result = 0;
            try
            {

                if (PM_KISAN_EntityList.Count > 0)
                {
                    objDBAccess.OpenConnection();
                    foreach (PM_KISAN_Entity OBjlst in PM_KISAN_EntityList)
                    {
                        SqlCommand cmd1 = objDBAccess.CreateCommand("InsertPatwarServiceData", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@PATWARI_APPROVED", DBFactoryParameterType.Int, OBjlst.PATWARI_APPROVED);
                        objDBAccess.AddParameterToCommand("@PDF_DOWNLOAD", DBFactoryParameterType.Int, OBjlst.PDF_DOWNLOAD);
                        objDBAccess.AddParameterToCommand("@COLLECTER_TOTAL", DBFactoryParameterType.Int, OBjlst.COLLECTER_TOTAL);
                        objDBAccess.AddParameterToCommand("@TEHSILDAR_REJ", DBFactoryParameterType.Int, OBjlst.TEHSILDAR_REJ);
                        objDBAccess.AddParameterToCommand("@PATWAR", DBFactoryParameterType.Int, OBjlst.PATWAR);
                        objDBAccess.AddParameterToCommand("@TEH_ESIGN_STATUS", DBFactoryParameterType.Int, OBjlst.TEH_ESIGN_STATUS);
                        objDBAccess.AddParameterToCommand("@PATWARI_REJ", DBFactoryParameterType.Int, OBjlst.PATWARI_REJ);
                        objDBAccess.AddParameterToCommand("@PAT_ESIGN_STATUS", DBFactoryParameterType.Int, OBjlst.PAT_ESIGN_STATUS);
                        objDBAccess.AddParameterToCommand("@FAMILY_REG", DBFactoryParameterType.Int, OBjlst.FAMILY_REG);
                        objDBAccess.AddParameterToCommand("@TOTAL_PAT", DBFactoryParameterType.Int, OBjlst.TOTAL_PAT);
                        objDBAccess.AddParameterToCommand("@FINAL_UPLOAD", DBFactoryParameterType.Int, OBjlst.FINAL_UPLOAD);
                        objDBAccess.AddParameterToCommand("@DISTRICTNAME", DBFactoryParameterType.NVarchar, OBjlst.DISTRICTNAME);
                        objDBAccess.AddParameterToCommand("@COLLECTOR_ESIGN_STATUS", DBFactoryParameterType.Int, OBjlst.COLLECTOR_ESIGN_STATUS);
                        objDBAccess.AddParameterToCommand("@COLLECTER_APPROVED", DBFactoryParameterType.Int, OBjlst.COLLECTER_APPROVED);
                        objDBAccess.AddParameterToCommand("@TOTAL_TEH", DBFactoryParameterType.Int, OBjlst.TOTAL_TEH);
                        objDBAccess.AddParameterToCommand("@DISTRICTCODE", DBFactoryParameterType.Int, OBjlst.DISTRICTCODE);
                        objDBAccess.AddParameterToCommand("@TEHSILDAR_APPROVED", DBFactoryParameterType.Int, OBjlst.TEHSILDAR_APPROVED);
                        objDBAccess.AddParameterToCommand("@COLLECTER_REJ", DBFactoryParameterType.Int, OBjlst.COLLECTER_REJ);
                        var Result2 = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                    }
                }


            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServicesModel", FunctionName = "InsertPM_KIssanData(LIST)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public DataTable PM_Kisan_Application_Status()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("PM_Kisan_Application_Status", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServicesModel", FunctionName = "PM_Kisan_Application_Status()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable Mobile_AuditStatusYearList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AuditStatusYearList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServicesModel", FunctionName = "AuditStatusYearList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable Mobile_AuditStatusMonthList(int FYearID)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AuditStatusMonthList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@YearId", DBFactoryParameterType.Int, FYearID);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServicesModel", FunctionName = "AuditStatusMonthList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable Mobile_GetAuditDivisionReport(int FYearID, int MonthId)
        {
            dt.Clear();


            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AuditDivisionDataReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FYID", DBFactoryParameterType.Int, FYearID);
                objDBAccess.AddParameterToCommand("@MONTHID", DBFactoryParameterType.Int, MonthId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServicesModel", FunctionName = "GetDivisionReport(string FYearID, string MonthId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable Mobile_GetAuditDistrictReport(int FYearID, int MonthId, string DivisionName)
        {
            dt.Clear();


            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AuditDistrictDataReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@FYID", DBFactoryParameterType.Int, FYearID);
                objDBAccess.AddParameterToCommand("@MONTHID", DBFactoryParameterType.Int, MonthId);
                objDBAccess.AddParameterToCommand("@DivisionName", DBFactoryParameterType.NVarchar, DivisionName);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServicesModel", FunctionName = "Mobile_GetAuditDistrictReport(string FYearID, string MonthId,string DivisionName)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable Mobile_GetAudit_Status(int FYearID, int MonthId)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Audit_Status_For_MobileApp", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@YearId", DBFactoryParameterType.Int, FYearID);
                objDBAccess.AddParameterToCommand("@MonthId", DBFactoryParameterType.Int, MonthId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServicesModel", FunctionName = "Mobile_GetAudit_Status(string FYearID, string MonthId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable Mobile_GetFertilizerOpeningStockReport(string OnDate)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("FertilizerOpeningStockReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@OnDate", DBFactoryParameterType.NVarchar, OnDate);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServicesModel", FunctionName = "Mobile_GetFertilizerOpeningStockReport(string OnDate)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable Mobile_GetFertilizerOpeningStockForDashboard(string OnDate)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("[FertilizerOpeningStockReportForDeshboard]", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@OnDate", DBFactoryParameterType.NVarchar, OnDate);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServicesModel", FunctionName = "Mobile_GetFertilizerOpeningStockForDashboard(string OnDate)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable Mobile_GetFertilizerRetailerTransactionReport(string OnDate, string DistrictName)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("FertilizerRetailerTransactionReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@OnDate", DBFactoryParameterType.NVarchar, OnDate);
                objDBAccess.AddParameterToCommand("@DistrictName", DBFactoryParameterType.NVarchar, DistrictName);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServicesModel", FunctionName = "Mobile_GetFertilizerRetailerTransactionReport(string OnDate,string DistrictName)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable Mobile_GetAllFertilizerOpeningStockStateReport(string OnDate = "")
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("FertilizerOpeningStockStateReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@OnDate", DBFactoryParameterType.NVarchar, OnDate);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServicesModel", FunctionName = "Mobile_GetAllFertilizerStateReport(string OnDate)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable Mobile_GetAllFertilizerOpeningStockDistrictReportByFertilizerName(string FertilizerName, string OnDate = "")
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("FertilizerOpeningStockDistrictReportByFertilizerName", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@OnDate", DBFactoryParameterType.NVarchar, OnDate);
                objDBAccess.AddParameterToCommand("@FertilizerName", DBFactoryParameterType.NVarchar, FertilizerName);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServicesModel", FunctionName = " Mobile_GetAllFertilizerOpeningStockDistrictReportByFertilizerName(string FertilizerName,string OnDate)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable Mobile_GetSocietyMemberRegiStatusByApplicationNo(string ApplicationNo)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("MemberRegiStatusByApplicationNo", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ApplicationNo", DBFactoryParameterType.NVarchar, ApplicationNo);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.CommonServicesModel", FunctionName = "Mobile_GetAllFertilizerStateReport(string OnDate)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetSocietyAuditReport(string SocietyName, string FinancialYearId = "0")
        {
            dt.Clear();
            try
            {
                string WhereClause = "  ";

                if (SocietyName.Trim() != "")
                {
                    WhereClause = WhereClause + " and (SocietyName='" + SocietyName + "' OR BRN='" + SocietyName + "'  OR RegistrationNo='" + SocietyName + "')";
                }

                if (FinancialYearId != null && FinancialYearId.Trim() != "" && FinancialYearId.Trim() != "0")
                {
                    WhereClause = WhereClause + " and AuditRPT.FinancialYear=" + FinancialYearId + " ";
                }

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyListForAuditRPTWeb", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyListForAuditRPT", FunctionName = "SocietyListForAuditRPT(string SocietyName,  string FinancialYearId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable DistrictList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DistrictList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, 0);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "DistrictList(int DivisionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable YearMonthforFertilizerSale(string Month, string YearId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("YearMonthforFertilizer", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, int.Parse(Month));
                objDBAccess.AddParameterToCommand("@Year", DBFactoryParameterType.Int, int.Parse(YearId));
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServices", FunctionName = "YearMonthforFertilizerSale(string Month,string YearId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }
        public DataTable Mobile_GetYearMonthListForAuditReport(string Month, string YearId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("YearMonthforAuditReport", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Month", DBFactoryParameterType.Int, int.Parse(Month));
                objDBAccess.AddParameterToCommand("@Year", DBFactoryParameterType.Int, int.Parse(YearId));

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServices", FunctionName = "Mobile_GetYearMonthListForAuditReport(string Month,string YearId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }
        public DataTable MSP_PURCHASECENTER()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("RAJ01", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServices", FunctionName = "Mobile_GetYearMonthListForAuditReport(string Month,string YearId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }
        public DataSet GetComplaintReportForMobileApp(int District, int Society, int SearchType = 0)
        {
            DataSet ds = new DataSet();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CSS_Report_ForMobile", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DISTRICT", DBFactoryParameterType.Int, District);
                objDBAccess.AddParameterToCommand("@SOCIETY", DBFactoryParameterType.Int, Society);
                //objDBAccess.AddParameterToCommand("@SearchType", DBFactoryParameterType.Int, SearchType);
                ds = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MIS", FunctionName = "DataTable  GetComplaintReportData( int DistrictId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return ds;
        }
        public int Insert_RajFedData(DataTable Dt)
        {
            int Result = 0, i = 0;
            try
            {
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    objDBAccess.OpenConnection();
                    foreach (DataRow Dr in Dt.Rows)
                    {
                        i++;
                        SqlCommand cmd1 = objDBAccess.CreateCommand("InsertRajFedServiceData", DBFactoryCommandType.PROCEDURE_COMMAND);
                        objDBAccess.AddParameterToCommand("@Crop", DBFactoryParameterType.NVarchar, Dr["Crop"] != null ? Dr["Crop"].ToString() : "");
                        objDBAccess.AddParameterToCommand("@Rate", DBFactoryParameterType.Double, Dr["Rate"] != null ? Convert.ToDecimal(Dr["Rate"].ToString()) : 0);
                        objDBAccess.AddParameterToCommand("@PurchaseTarget", DBFactoryParameterType.Double, Dr["PurchaseTarget"] != null ? Convert.ToDecimal(Dr["PurchaseTarget"].ToString()) : 0);
                        objDBAccess.AddParameterToCommand("@RegTarget", DBFactoryParameterType.Double, Dr["Crop"] != null ? Convert.ToDecimal(Dr["RegTarget"].ToString()) : 0);
                        objDBAccess.AddParameterToCommand("@Registration", DBFactoryParameterType.Int, Dr["Crop"] != null ? Convert.ToInt32(Dr["Registration"].ToString()) : 0);
                        objDBAccess.AddParameterToCommand("@DateAlloted", DBFactoryParameterType.Int, Dr["Crop"] != null ? Convert.ToInt32(Dr["DateAlloted"].ToString()) : 0);
                        objDBAccess.AddParameterToCommand("@PurchaseQty", DBFactoryParameterType.Double, Dr["Crop"] != null ? Convert.ToDecimal(Dr["PurchaseQty"].ToString()) : 0);
                        objDBAccess.AddParameterToCommand("@PurchaseAmount", DBFactoryParameterType.Double, Dr["Crop"] != null ? Convert.ToDecimal(Dr["PurchaseAmount"].ToString()) : 0);
                        objDBAccess.AddParameterToCommand("@PurchaseFarmerCount", DBFactoryParameterType.Int, Dr["Crop"] != null ? Convert.ToInt32(Dr["PurchaseFarmerCount"].ToString()) : 0);
                        objDBAccess.AddParameterToCommand("@PaymentAmount", DBFactoryParameterType.Double, Dr["Crop"] != null ? Convert.ToDecimal(Dr["PaymentAmount"].ToString()) : 0);
                        objDBAccess.AddParameterToCommand("@RemaningPaymentFarmers", DBFactoryParameterType.Int, Dr["Crop"] != null ? Convert.ToInt32(Dr["RemaningPaymentFarmers"].ToString()) : 0);
                        objDBAccess.AddParameterToCommand("@RemaningPaymentRs", DBFactoryParameterType.Double, Dr["Crop"] != null ? Convert.ToDecimal(Dr["RemaningPaymentRs"].ToString()) : 0);
                        objDBAccess.AddParameterToCommand("@Date", DBFactoryParameterType.DateTime, Convert.ToDateTime(Dr["dt"].ToString()));
                        objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, 1);
                        objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, 0);
                        objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                        objDBAccess.AddParameterToCommand("@IsFirst", DBFactoryParameterType.Int, i);
                        Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServices", FunctionName = "Insert_RajFedData(DataTable Dt)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public int Insert_LWAData(LWA LWAOBJ)
        {
            int Result = 0, i = 0;
            try
            {
                if (LWAOBJ != null)
                {
                    objDBAccess.OpenConnection();
                    SqlCommand cmd1 = objDBAccess.CreateCommand("InsertLWAData", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@TotalActive_Head_Office", DBFactoryParameterType.NVarchar, LWAOBJ.TotalActive_Head_Office);
                    objDBAccess.AddParameterToCommand("@Total_Head_Office", DBFactoryParameterType.NVarchar, LWAOBJ.Total_Head_Office);
                    objDBAccess.AddParameterToCommand("@TotalActive_Branch_Office", DBFactoryParameterType.NVarchar, LWAOBJ.TotalActive_Branch_Office);
                    objDBAccess.AddParameterToCommand("@Total_Branch_Office", DBFactoryParameterType.NVarchar, LWAOBJ.Total_Branch_Office);
                    objDBAccess.AddParameterToCommand("@TotalActive_PACS", DBFactoryParameterType.NVarchar, LWAOBJ.TotalActive_PACS);
                    objDBAccess.AddParameterToCommand("@Total_PACS", DBFactoryParameterType.NVarchar, LWAOBJ.Total_PACS);
                    objDBAccess.AddParameterToCommand("@Total_Applications", DBFactoryParameterType.NVarchar, LWAOBJ.Total_Applications);
                    objDBAccess.AddParameterToCommand("@Total_UID_Authentication", DBFactoryParameterType.NVarchar, LWAOBJ.Total_UID_Authentication);
                    objDBAccess.AddParameterToCommand("@Total_Cert_Issued", DBFactoryParameterType.NVarchar, LWAOBJ.Total_Cert_Issued);
                    objDBAccess.AddParameterToCommand("@Total_Cert_Distributed", DBFactoryParameterType.NVarchar, LWAOBJ.Total_Cert_Distributed);
                    objDBAccess.AddParameterToCommand("@Total_Rechecking_Cases", DBFactoryParameterType.NVarchar, LWAOBJ.Total_Rechecking_Cases);
                    objDBAccess.AddParameterToCommand("@Total_GRA_Cases_Pending", DBFactoryParameterType.NVarchar, LWAOBJ.Total_GRA_Cases_Pending);
                    objDBAccess.AddParameterToCommand("@Total_Not_Eligible_Cases", DBFactoryParameterType.NVarchar, LWAOBJ.Total_Not_Eligible_Cases);
                    objDBAccess.AddParameterToCommand("@Total_Not_Intrested_Cases", DBFactoryParameterType.NVarchar, LWAOBJ.Total_Not_Intrested_Cases);
                    objDBAccess.AddParameterToCommand("@New_Beneficiaries_2019", DBFactoryParameterType.NVarchar, LWAOBJ.New_Beneficiaries_2019);
                    objDBAccess.AddParameterToCommand("@NOD_TOTAL_InCrores", DBFactoryParameterType.NVarchar, LWAOBJ.NOD_TOTAL_InCrores);
                    objDBAccess.AddParameterToCommand("@OD_TOTAL_InCrores", DBFactoryParameterType.NVarchar, LWAOBJ.OD_TOTAL_InCrores);
                    objDBAccess.AddParameterToCommand("@Avg_Loan_Waiver_Amount", DBFactoryParameterType.NVarchar, LWAOBJ.Avg_Loan_Waiver_Amount);
                    objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, 1);
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, 0);
                    objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());
                    Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServices", FunctionName = "Insert_RajFedData(DataTable Dt)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public void Insert_MemberOD_NOD_COUNT()
        {
            int Result = 0, i = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd1 = objDBAccess.CreateCommand("InsertMemberOD_NOD_COUNT", DBFactoryCommandType.PROCEDURE_COMMAND);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServices", FunctionName = "Insert_MemberOD_NOD_COUNT" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

        }

        public int CCSPropertyDetailInsert(CCSPropertDetailEntity entity)
        {
            int Result = 0;
            try
            {

                objDBAccess.OpenConnection();

                SqlCommand cmd1 = objDBAccess.CreateCommand("CCSPropertyDetailInsert", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, entity.DistrictId);
                objDBAccess.AddParameterToCommand("@CCSId", DBFactoryParameterType.Int, entity.CCSId);
                objDBAccess.AddParameterToCommand("@PropertyTypeId", DBFactoryParameterType.Int, entity.PropertyTypeId);
                objDBAccess.AddParameterToCommand("@LocationStateId", DBFactoryParameterType.Int, entity.LocationStateId);
                objDBAccess.AddParameterToCommand("@LocationDistrictId", DBFactoryParameterType.Int, entity.LocationDistrictId);
                objDBAccess.AddParameterToCommand("@IsRural", DBFactoryParameterType.Bit, entity.IsRural);
                objDBAccess.AddParameterToCommand("@LocationAddress", DBFactoryParameterType.NVarchar, entity.LocationAddress);
                objDBAccess.AddParameterToCommand("@LocationLandMark", DBFactoryParameterType.NVarchar, entity.LocationLandMark);
                objDBAccess.AddParameterToCommand("@Image1", DBFactoryParameterType.NVarchar, entity.ImageName1);
                objDBAccess.AddParameterToCommand("@Image2", DBFactoryParameterType.NVarchar, entity.ImageName2);
                objDBAccess.AddParameterToCommand("@Latitude", DBFactoryParameterType.NVarchar, entity.Latitude);
                objDBAccess.AddParameterToCommand("@Longitude", DBFactoryParameterType.NVarchar, entity.Longitude);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.NVarchar, entity.MobileNo);
                objDBAccess.AddParameterToCommand("@ReportByTypeId", DBFactoryParameterType.NVarchar, entity.subjectId);
                objDBAccess.AddParameterToCommand("@IsWeb", DBFactoryParameterType.NVarchar, entity.ISWeb);
                objDBAccess.AddParameterToCommand("@ReportByName", DBFactoryParameterType.NVarchar, entity.EnrantName);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServices", FunctionName = "CCSPropertyDetailInsert(CCSPropertDetailEntity entity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable Mobile_GetCCSPropertyTypeList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CCSPropertyTypeList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServices", FunctionName = "public DataTable Mobile_GetCCSPropertyTypeList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable Mobile_GetStateListCCSLocation()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("StateListCCSLocation", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServices", FunctionName = "public DataTable Mobile_StateListCCSLocation()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }


        public DataTable Mobile_GetDistrictListCCSLocation(int StateId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DistrictListCCSLocation", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@StateId", DBFactoryParameterType.Int, StateId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServices", FunctionName = "public DataTable Mobile_GetDistrictListCCSLocation(int StateId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable Mobile_GetEnrantListCCSLocation()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("[GetAllEnrantType]", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServices", FunctionName = "public DataTable Mobile_StateListCCSLocation()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }


        public DataTable GetTotalQuantityList(string DistrictId = "0", string Kvss = "", string purchase_center = "0", string commodity = "", string crop_year = "")
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetTotalQuantityListDatails", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@WhereHouseId", DBFactoryParameterType.Int, 0);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                objDBAccess.AddParameterToCommand("@Kvss", DBFactoryParameterType.NVarchar, Kvss);
                objDBAccess.AddParameterToCommand("@purchase_center", DBFactoryParameterType.NVarchar, purchase_center);
                objDBAccess.AddParameterToCommand("@commodity", DBFactoryParameterType.NVarchar, commodity);
                objDBAccess.AddParameterToCommand("@crop_year", DBFactoryParameterType.NVarchar, crop_year);

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

        public DataTable GetTotalQuantityListNew(string DistrictId = "0", string Kvss = "", string purchase_center = "0", string commodity = "", string crop_year = "", string month = "", string date = "", string Type = "", string Agency = "")
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetTotalQuantityListDatails_2024", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@WhereHouseId", DBFactoryParameterType.Int, 0);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                objDBAccess.AddParameterToCommand("@Kvss", DBFactoryParameterType.NVarchar, Kvss);
                objDBAccess.AddParameterToCommand("@purchase_center", DBFactoryParameterType.NVarchar, purchase_center);
                objDBAccess.AddParameterToCommand("@commodity", DBFactoryParameterType.NVarchar, commodity);
                objDBAccess.AddParameterToCommand("@crop_year", DBFactoryParameterType.NVarchar, crop_year);
                objDBAccess.AddParameterToCommand("@month", DBFactoryParameterType.NVarchar, month);
                objDBAccess.AddParameterToCommand("@date", DBFactoryParameterType.NVarchar, date);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, Type);
                objDBAccess.AddParameterToCommand("@Agency", DBFactoryParameterType.NVarchar, Agency);


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

        public DataTable GetWarehouseApiData(string DistrictId = "0", string Kvss = "", string purchase_center = "0", string commodity = "", string crop_year = "", string month = "", string date = "", string Type = "", string Agency = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetWarehouseApiData", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@WhereHouseId", DBFactoryParameterType.Int, 0);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                objDBAccess.AddParameterToCommand("@Kvss", DBFactoryParameterType.NVarchar, Kvss);
                objDBAccess.AddParameterToCommand("@purchase_center", DBFactoryParameterType.NVarchar, purchase_center);
                objDBAccess.AddParameterToCommand("@commodity", DBFactoryParameterType.NVarchar, commodity);
                objDBAccess.AddParameterToCommand("@crop_year", DBFactoryParameterType.NVarchar, crop_year);
                objDBAccess.AddParameterToCommand("@month", DBFactoryParameterType.NVarchar, month);
                objDBAccess.AddParameterToCommand("@date", DBFactoryParameterType.NVarchar, date);
                objDBAccess.AddParameterToCommand("@Type", DBFactoryParameterType.NVarchar, Type);
                objDBAccess.AddParameterToCommand("@Agency", DBFactoryParameterType.NVarchar, Agency);

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




        public DataSet GetSocietyBasicDetails(string RegistrationNo)
        {
            DataSet dt = new DataSet();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("GetSocietyBasicDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, @RegistrationNo);
                dt = objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahakar.Models.CitizenModel", FunctionName = "public DataTable GetSocietyBasicDetails(string RegistrationNo)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            //Setdatalocal(dt);
            return dt;

        }

        public int GetSocietyBasicDetails(DataTable sData)
        {
            int result = 0;

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                foreach (DataRow dr in sData.Rows)
                {
                    cmd = objDBAccess.CreateCommand("SaveSocietyData", DBFactoryCommandType.PROCEDURE_COMMAND);
                    string RegistrationNo = dr["RegistrationNo"].ToString();
                    string SocietyName = dr["SocietyName"].ToString();
                    string SocietyTypeId = dr["SocietyTypeId"].ToString();
                    string BusinessId = dr["BusinessId"].ToString();
                    string LiablityId = dr["LiablityId"].ToString();
                    string MemberLiablity = dr["MemberLiablity"].ToString();
                    string ClassId = dr["ClassId"].ToString();
                    string SubClassId = dr["SubClassId"].ToString();
                    string StatusId = dr["StatusId"].ToString();
                    string IsActive = dr["IsActive"].ToString();
                    string Remarks = dr["Remarks"].ToString();
                    string Certificate = dr["Certificate"].ToString();
                    string Request_Id = dr["Request_Id"].ToString();
                    string Amount = dr["Amount"].ToString();
                    string ApplicationFileBy = dr["ApplicationFileBy"].ToString();
                    string AssistantId = dr["AssistantId"].ToString();
                    string SeqId = dr["SeqId"].ToString();
                    string RehAct = dr["RehAct"].ToString();
                    string BankPassBook = dr["BankPassBook"].ToString();
                    string TotalShareAmount = dr["TotalShareAmount"].ToString();
                    string BankOpeningCertificate = dr["BankOpeningCertificate"].ToString();
                    string BRN = dr["BRN"].ToString();
                    string InspectorId = dr["InspectorId"].ToString();
                    string RegistrarId = dr["RegistrarId"].ToString();
                    string CreatedBy = dr["CreatedBy"].ToString();
                    string CreationDate = dr["CreationDate"].ToString();
                    string UpdatedBy = dr["UpdatedBy"].ToString();
                    string UpdationDate = dr["UpdationDate"].ToString();
                    string CreatedIP = dr["CreatedIP"].ToString();
                    string UpdatedIP = dr["UpdatedIP"].ToString();
                    string TotalMembersClassA = dr["TotalMembersClassA"].ToString();
                    string TotalMembersClassB = dr["TotalMembersClassB"].ToString();
                    string TotalMembersClassC = dr["TotalMembersClassC"].ToString();
                    string RegistrationCertificateFile = dr["RegistrationCertificateFile"].ToString();
                    string RegistrationDate = dr["RegistrationDate"].ToString();
                    string ActId = dr["ActId"].ToString();
                    string SSOID = dr["SSOID"].ToString();
                    string NoOfBranches = dr["NoOfBranches"].ToString();
                    string ElectionDate = dr["ElectionDate"].ToString();
                    string UserType = dr["UserType"].ToString();
                    string BylawsFile = dr["BylawsFile"].ToString();
                    string RulesRegulationFile = dr["RulesRegulationFile"].ToString();
                    string SocietyCategoryId = dr["SocietyCategoryId"].ToString();
                    string KIOSKCODE = dr["KIOSKCODE"].ToString();
                    string OldSocietyId = dr["OldSocietyId"].ToString();
                    string OTP = dr["OTP"].ToString();
                    string OTPDateTime = dr["OTPDateTime"].ToString();
                    string IsRSSC = dr["IsRSSC"].ToString();
                    string AGMDate = dr["AGMDate"].ToString();
                    string SportsAssociationId = dr["SportsAssociationId"].ToString();
                    string IsAffiliatedByRSSC = dr["IsAffiliatedByRSSC"].ToString();
                    string RSSCRegNo = dr["RSSCRegNo"].ToString();
                    string RSSCAffiliatedDate = dr["RSSCAffiliatedDate"].ToString();
                    string RSSCFileName = dr["RSSCFileName"].ToString();
                    string IsAffiliatedBySOC = dr["IsAffiliatedBySOC"].ToString();
                    string SOCRegNo = dr["SOCRegNo"].ToString();
                    string SOCAffiliatedDate = dr["SOCAffiliatedDate"].ToString();
                    string SOCFileName = dr["SOCFileName"].ToString();
                    string IsAffiliatedByDLSC = dr["IsAffiliatedByDLSC"].ToString();
                    string DLSCRegNo = dr["DLSCRegNo"].ToString();
                    string DLSCAffiliatedDate = dr["DLSCAffiliatedDate"].ToString();
                    string DLSCFileName = dr["DLSCFileName"].ToString();
                    string MemberListFileName = dr["MemberListFileName"].ToString();
                    string SubmitStatus = dr["SubmitStatus"].ToString();
                    string IsNewRegistration = dr["IsNewRegistration"].ToString();
                    string PaidUpShareCapital = dr["PaidUpShareCapital"].ToString();
                    string SingleShareValue = dr["SingleShareValue"].ToString();
                    string NoOfMemberInAGM = dr["NoOfMemberInAGM"].ToString();
                    string EntranceFeesCollected = dr["EntranceFeesCollected"].ToString();
                    string WorkPlanEcoViabilityFile = dr["WorkPlanEcoViabilityFile"].ToString();
                    string GeneralMeetingProcedureFile = dr["GeneralMeetingProcedureFile"].ToString();
                    string BalanceSheetExcelFile = dr["BalanceSheetExcelFile"].ToString();
                    string LiabilityOfMember = dr["LiabilityOfMember"].ToString();
                    string TotalNumberOfMembers = dr["TotalNumberOfMembers"].ToString();
                    string ObjectsFile = dr["ObjectsFile"].ToString();
                    string IsAffiliationBySLSA = dr["IsAffiliationBySLSA"].ToString();
                    string SLSARegNo = dr["SLSARegNo"].ToString();
                    string SLSAAffiliatedDate = dr["SLSAAffiliatedDate"].ToString();
                    string SLSAFileName = dr["SLSAFileName"].ToString();
                    string IsPermanentSave = dr["IsPermanentSave"].ToString();
                    string OldRegistrationNo = dr["OldRegistrationNo"].ToString();
                    string ISPROFILR = dr["ISPROFILR"].ToString();
                    string SOCIETIESAUTHORIZEDPERSONS = dr["SOCIETIESAUTHORIZEDPERSONS"].ToString();
                    string SocietyReportFile = dr["SocietyReportFile"].ToString();
                    string ExcelUploadId = dr["ExcelUploadId"].ToString();
                    string RejectionDate = dr["RejectionDate"].ToString();
                    string RejectionFileName = dr["RejectionFileName"].ToString();
                    string IsDev = dr["IsDev"].ToString();
                    string IsAmendment = dr["IsAmendment"].ToString();
                    string NameChangeFile = dr["NameChangeFile"].ToString();
                    string SocietyStatusId = dr["SocietyStatusId"].ToString();
                    string IsSWS = dr["IsSWS"].ToString();
                    string IsAmendmentAtProfiling = dr["IsAmendmentAtProfiling"].ToString();
                    string AmendmentType = dr["AmendmentType"].ToString();
                    string OldExecutiveCommittee = dr["OldExecutiveCommittee"].ToString();
                    string SocietyCode = dr["SocietyCode"].ToString();
                    string ApplicationStatus = dr["ApplicationStatus"].ToString();
                    string IsRural = dr["IsRural"].ToString();
                    string HouseNo = dr["HouseNo"].ToString();
                    string LaneStreetName = dr["LaneStreetName"].ToString();
                    string Locality = dr["Locality"].ToString();
                    string STATE_ID = dr["STATE_ID"].ToString();
                    string DISTRICT_ID = dr["DISTRICT_ID"].ToString();
                    string CITY_ID = dr["CITY_ID"].ToString();
                    string BLOCK_ID = dr["BLOCK_ID"].ToString();
                    string WARD_ID = dr["WARD_ID"].ToString();
                    string GP_ID = dr["GP_ID"].ToString();
                    string VILLAGE_ID = dr["VILLAGE_ID"].ToString();
                    string PinCode = dr["PinCode"].ToString();

                    objDBAccess.AddParameterToCommand("@RegistrationNo", DBFactoryParameterType.NVarchar, RegistrationNo);
                    objDBAccess.AddParameterToCommand("@SocietyName", DBFactoryParameterType.NVarchar, SocietyName);
                    objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.NVarchar, SocietyTypeId);
                    objDBAccess.AddParameterToCommand("@BusinessId", DBFactoryParameterType.NVarchar, BusinessId);
                    objDBAccess.AddParameterToCommand("@LiablityId", DBFactoryParameterType.NVarchar, LiablityId);
                    objDBAccess.AddParameterToCommand("@MemberLiablity", DBFactoryParameterType.NVarchar, MemberLiablity);
                    objDBAccess.AddParameterToCommand("@ClassId", DBFactoryParameterType.NVarchar, ClassId);
                    objDBAccess.AddParameterToCommand("@SubClassId", DBFactoryParameterType.NVarchar, SubClassId);
                    objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.NVarchar, StatusId);
                    objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.NVarchar, IsActive);
                    objDBAccess.AddParameterToCommand("@Remarks", DBFactoryParameterType.NVarchar, Remarks);
                    objDBAccess.AddParameterToCommand("@Certificate", DBFactoryParameterType.NVarchar, Certificate);
                    objDBAccess.AddParameterToCommand("@Request_Id", DBFactoryParameterType.NVarchar, Request_Id);
                    objDBAccess.AddParameterToCommand("@Amount", DBFactoryParameterType.NVarchar, Amount);
                    objDBAccess.AddParameterToCommand("@ApplicationFileBy", DBFactoryParameterType.NVarchar, ApplicationFileBy);
                    objDBAccess.AddParameterToCommand("@AssistantId", DBFactoryParameterType.NVarchar, AssistantId);
                    objDBAccess.AddParameterToCommand("@SeqId", DBFactoryParameterType.NVarchar, SeqId);
                    objDBAccess.AddParameterToCommand("@RehAct", DBFactoryParameterType.NVarchar, RehAct);
                    objDBAccess.AddParameterToCommand("@BankPassBook", DBFactoryParameterType.NVarchar, BankPassBook);
                    objDBAccess.AddParameterToCommand("@TotalShareAmount", DBFactoryParameterType.NVarchar, TotalShareAmount);
                    objDBAccess.AddParameterToCommand("@BankOpeningCertificate", DBFactoryParameterType.NVarchar, BankOpeningCertificate);
                    objDBAccess.AddParameterToCommand("@BRN", DBFactoryParameterType.NVarchar, BRN);
                    objDBAccess.AddParameterToCommand("@InspectorId", DBFactoryParameterType.NVarchar, InspectorId);
                    objDBAccess.AddParameterToCommand("@RegistrarId", DBFactoryParameterType.NVarchar, RegistrarId);
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.NVarchar, CreatedBy);
                    objDBAccess.AddParameterToCommand("@CreationDate", DBFactoryParameterType.NVarchar, CreationDate);
                    objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.NVarchar, UpdatedBy);
                    objDBAccess.AddParameterToCommand("@UpdationDate", DBFactoryParameterType.NVarchar, UpdationDate);
                    objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, CreatedIP);
                    objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, UpdatedIP);
                    objDBAccess.AddParameterToCommand("@TotalMembersClassA", DBFactoryParameterType.NVarchar, TotalMembersClassA);
                    objDBAccess.AddParameterToCommand("@TotalMembersClassB", DBFactoryParameterType.NVarchar, TotalMembersClassB);
                    objDBAccess.AddParameterToCommand("@TotalMembersClassC", DBFactoryParameterType.NVarchar, TotalMembersClassC);
                    objDBAccess.AddParameterToCommand("@RegistrationCertificateFile", DBFactoryParameterType.NVarchar, RegistrationCertificateFile);
                    objDBAccess.AddParameterToCommand("@RegistrationDate", DBFactoryParameterType.NVarchar, RegistrationDate);
                    objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, ActId);
                    objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, SSOID);
                    objDBAccess.AddParameterToCommand("@NoOfBranches", DBFactoryParameterType.NVarchar, NoOfBranches);
                    objDBAccess.AddParameterToCommand("@ElectionDate", DBFactoryParameterType.NVarchar, ElectionDate);
                    objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, UserType);
                    objDBAccess.AddParameterToCommand("@BylawsFile", DBFactoryParameterType.NVarchar, BylawsFile);
                    objDBAccess.AddParameterToCommand("@RulesRegulationFile", DBFactoryParameterType.NVarchar, RulesRegulationFile);
                    objDBAccess.AddParameterToCommand("@SocietyCategoryId", DBFactoryParameterType.NVarchar, SocietyCategoryId);
                    objDBAccess.AddParameterToCommand("@KIOSKCODE", DBFactoryParameterType.NVarchar, KIOSKCODE);
                    objDBAccess.AddParameterToCommand("@OldSocietyId", DBFactoryParameterType.NVarchar, OldSocietyId);
                    objDBAccess.AddParameterToCommand("@OTP", DBFactoryParameterType.NVarchar, OTP);
                    objDBAccess.AddParameterToCommand("@OTPDateTime", DBFactoryParameterType.NVarchar, OTPDateTime);
                    objDBAccess.AddParameterToCommand("@IsRSSC", DBFactoryParameterType.NVarchar, IsRSSC);
                    objDBAccess.AddParameterToCommand("@AGMDate", DBFactoryParameterType.NVarchar, AGMDate);
                    objDBAccess.AddParameterToCommand("@SportsAssociationId", DBFactoryParameterType.NVarchar, SportsAssociationId);
                    objDBAccess.AddParameterToCommand("@IsAffiliatedByRSSC", DBFactoryParameterType.NVarchar, IsAffiliatedByRSSC);
                    objDBAccess.AddParameterToCommand("@RSSCRegNo", DBFactoryParameterType.NVarchar, RSSCRegNo);
                    objDBAccess.AddParameterToCommand("@RSSCAffiliatedDate", DBFactoryParameterType.NVarchar, RSSCAffiliatedDate);
                    objDBAccess.AddParameterToCommand("@RSSCFileName", DBFactoryParameterType.NVarchar, RSSCFileName);
                    objDBAccess.AddParameterToCommand("@IsAffiliatedBySOC", DBFactoryParameterType.NVarchar, IsAffiliatedBySOC);
                    objDBAccess.AddParameterToCommand("@SOCRegNo", DBFactoryParameterType.NVarchar, SOCRegNo);
                    objDBAccess.AddParameterToCommand("@SOCAffiliatedDate", DBFactoryParameterType.NVarchar, SOCAffiliatedDate);
                    objDBAccess.AddParameterToCommand("@SOCFileName", DBFactoryParameterType.NVarchar, SOCFileName);
                    objDBAccess.AddParameterToCommand("@IsAffiliatedByDLSC", DBFactoryParameterType.NVarchar, IsAffiliatedByDLSC);
                    objDBAccess.AddParameterToCommand("@DLSCRegNo", DBFactoryParameterType.NVarchar, DLSCRegNo);
                    objDBAccess.AddParameterToCommand("@DLSCAffiliatedDate", DBFactoryParameterType.NVarchar, DLSCAffiliatedDate);
                    objDBAccess.AddParameterToCommand("@DLSCFileName", DBFactoryParameterType.NVarchar, DLSCFileName);
                    objDBAccess.AddParameterToCommand("@MemberListFileName", DBFactoryParameterType.NVarchar, MemberListFileName);
                    objDBAccess.AddParameterToCommand("@SubmitStatus", DBFactoryParameterType.NVarchar, SubmitStatus);
                    objDBAccess.AddParameterToCommand("@IsNewRegistration", DBFactoryParameterType.NVarchar, IsNewRegistration);
                    objDBAccess.AddParameterToCommand("@PaidUpShareCapital", DBFactoryParameterType.NVarchar, PaidUpShareCapital);
                    objDBAccess.AddParameterToCommand("@SingleShareValue", DBFactoryParameterType.NVarchar, SingleShareValue);
                    objDBAccess.AddParameterToCommand("@NoOfMemberInAGM", DBFactoryParameterType.NVarchar, NoOfMemberInAGM);
                    objDBAccess.AddParameterToCommand("@EntranceFeesCollected", DBFactoryParameterType.NVarchar, EntranceFeesCollected);
                    objDBAccess.AddParameterToCommand("@WorkPlanEcoViabilityFile", DBFactoryParameterType.NVarchar, WorkPlanEcoViabilityFile);
                    objDBAccess.AddParameterToCommand("@GeneralMeetingProcedureFile", DBFactoryParameterType.NVarchar, GeneralMeetingProcedureFile);
                    objDBAccess.AddParameterToCommand("@BalanceSheetExcelFile", DBFactoryParameterType.NVarchar, BalanceSheetExcelFile);
                    objDBAccess.AddParameterToCommand("@LiabilityOfMember", DBFactoryParameterType.NVarchar, LiabilityOfMember);
                    objDBAccess.AddParameterToCommand("@TotalNumberOfMembers", DBFactoryParameterType.NVarchar, TotalNumberOfMembers);
                    objDBAccess.AddParameterToCommand("@ObjectsFile", DBFactoryParameterType.NVarchar, ObjectsFile);
                    objDBAccess.AddParameterToCommand("@IsAffiliationBySLSA", DBFactoryParameterType.NVarchar, IsAffiliationBySLSA);
                    objDBAccess.AddParameterToCommand("@SLSARegNo", DBFactoryParameterType.NVarchar, SLSARegNo);
                    objDBAccess.AddParameterToCommand("@SLSAAffiliatedDate", DBFactoryParameterType.NVarchar, SLSAAffiliatedDate);
                    objDBAccess.AddParameterToCommand("@SLSAFileName", DBFactoryParameterType.NVarchar, SLSAFileName);
                    objDBAccess.AddParameterToCommand("@IsPermanentSave", DBFactoryParameterType.NVarchar, IsPermanentSave);
                    objDBAccess.AddParameterToCommand("@OldRegistrationNo", DBFactoryParameterType.NVarchar, OldRegistrationNo);
                    objDBAccess.AddParameterToCommand("@ISPROFILR", DBFactoryParameterType.NVarchar, ISPROFILR);
                    objDBAccess.AddParameterToCommand("@SOCIETIESAUTHORIZEDPERSONS", DBFactoryParameterType.NVarchar, SOCIETIESAUTHORIZEDPERSONS);
                    objDBAccess.AddParameterToCommand("@SocietyReportFile", DBFactoryParameterType.NVarchar, SocietyReportFile);
                    objDBAccess.AddParameterToCommand("@ExcelUploadId", DBFactoryParameterType.NVarchar, ExcelUploadId);
                    objDBAccess.AddParameterToCommand("@RejectionDate", DBFactoryParameterType.NVarchar, RejectionDate);
                    objDBAccess.AddParameterToCommand("@RejectionFileName", DBFactoryParameterType.NVarchar, RejectionFileName);
                    objDBAccess.AddParameterToCommand("@IsDev", DBFactoryParameterType.NVarchar, IsDev);
                    objDBAccess.AddParameterToCommand("@IsAmendment", DBFactoryParameterType.NVarchar, IsAmendment);
                    objDBAccess.AddParameterToCommand("@NameChangeFile", DBFactoryParameterType.NVarchar, NameChangeFile);
                    objDBAccess.AddParameterToCommand("@SocietyStatusId", DBFactoryParameterType.NVarchar, SocietyStatusId);
                    objDBAccess.AddParameterToCommand("@IsSWS", DBFactoryParameterType.NVarchar, IsSWS);
                    objDBAccess.AddParameterToCommand("@IsAmendmentAtProfiling", DBFactoryParameterType.NVarchar, IsAmendmentAtProfiling);
                    objDBAccess.AddParameterToCommand("@AmendmentType", DBFactoryParameterType.NVarchar, AmendmentType);
                    objDBAccess.AddParameterToCommand("@OldExecutiveCommittee", DBFactoryParameterType.NVarchar, OldExecutiveCommittee);
                    objDBAccess.AddParameterToCommand("@SocietyCode", DBFactoryParameterType.NVarchar, SocietyCode);
                    objDBAccess.AddParameterToCommand("@ApplicationStatus", DBFactoryParameterType.NVarchar, ApplicationStatus);
                    objDBAccess.AddParameterToCommand("@IsRural", DBFactoryParameterType.NVarchar, IsRural);
                    objDBAccess.AddParameterToCommand("@HouseNo", DBFactoryParameterType.NVarchar, HouseNo);
                    objDBAccess.AddParameterToCommand("@LaneStreetName", DBFactoryParameterType.NVarchar, LaneStreetName);
                    objDBAccess.AddParameterToCommand("@Locality", DBFactoryParameterType.NVarchar, Locality);
                    objDBAccess.AddParameterToCommand("@STATE_ID", DBFactoryParameterType.NVarchar, STATE_ID);
                    objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.NVarchar, DISTRICT_ID);
                    objDBAccess.AddParameterToCommand("@CITY_ID", DBFactoryParameterType.NVarchar, CITY_ID);
                    objDBAccess.AddParameterToCommand("@BLOCK_ID", DBFactoryParameterType.NVarchar, BLOCK_ID);
                    objDBAccess.AddParameterToCommand("@WARD_ID", DBFactoryParameterType.NVarchar, WARD_ID);
                    objDBAccess.AddParameterToCommand("@GP_ID", DBFactoryParameterType.NVarchar, GP_ID);
                    objDBAccess.AddParameterToCommand("@VILLAGE_ID", DBFactoryParameterType.NVarchar, VILLAGE_ID);
                    objDBAccess.AddParameterToCommand("@PinCode", DBFactoryParameterType.NVarchar, PinCode);
                    result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahakar.Models.CitizenModel", FunctionName = "public DataTable GetSocietyBasicDetails(string RegistrationNo)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            //Setdatalocal(dt);
            return result;

        }

        public int SaveSocietyApplicationFlow(DataTable sData, string SocietyId)
        {
            int result = 0;

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                foreach (DataRow dr in sData.Rows)
                {
                    cmd = objDBAccess.CreateCommand("SaveSocietyApplicationFlow", DBFactoryCommandType.PROCEDURE_COMMAND);
                    //string SocietyID = dr["SocietyID"].ToString();
                    string StatusId = dr["StatusId"].ToString();
                    string UserId = dr["UserId"].ToString();
                    string Remark = dr["Remark"].ToString();
                    string ActionDate = dr["ActionDate"].ToString();
                    string CreateBy = dr["CreateBy"].ToString();
                    string CreateIP = dr["CreateIP"].ToString();
                    string CreateBY_SSO_ID = dr["CreateBY_SSO_ID"].ToString();
                    string REGISTRATIONNO = dr["REGISTRATIONNO"].ToString();
                    string IsAmendment = dr["IsAmendment"].ToString();
                    string EAppRequestID = dr["EAppRequestID"].ToString();
                    string OldSocietyId = dr["OldSocietyId"].ToString();

                    objDBAccess.AddParameterToCommand("@SocietyID", DBFactoryParameterType.NVarchar, SocietyId);
                    objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.NVarchar, StatusId);
                    objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.NVarchar, UserId);
                    objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Remark);
                    objDBAccess.AddParameterToCommand("@ActionDate", DBFactoryParameterType.NVarchar, ActionDate);
                    objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.NVarchar, CreateBy);
                    objDBAccess.AddParameterToCommand("@CreateIP", DBFactoryParameterType.NVarchar, CreateIP);
                    objDBAccess.AddParameterToCommand("@CreateBY_SSO_ID", DBFactoryParameterType.NVarchar, CreateBY_SSO_ID);
                    objDBAccess.AddParameterToCommand("@REGISTRATIONNO", DBFactoryParameterType.NVarchar, REGISTRATIONNO);
                    objDBAccess.AddParameterToCommand("@IsAmendment", DBFactoryParameterType.NVarchar, IsAmendment);
                    objDBAccess.AddParameterToCommand("@EAppRequestID", DBFactoryParameterType.NVarchar, EAppRequestID);
                    objDBAccess.AddParameterToCommand("@OldSocietyId", DBFactoryParameterType.NVarchar, OldSocietyId);
                    result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahakar.Models.CitizenModel", FunctionName = "public DataTable GetSocietyBasicDetails(string RegistrationNo)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            //Setdatalocal(dt);
            return result;

        }

        public int SaveSocietyRegOfficeDetails(DataTable sData, string SocietyId)
        {
            int result = 0;

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                foreach (DataRow dr in sData.Rows)
                {
                    cmd = objDBAccess.CreateCommand("SaveSocietyRegOfficeDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                    //string SocietyId = dr["SocietyId"].ToString();
                    string IsRural = dr["IsRural"].ToString();
                    string HouseNo = dr["HouseNo"].ToString();
                    string LaneStreetName = dr["LaneStreetName"].ToString();
                    string Locality = dr["Locality"].ToString();
                    string STATE_ID = dr["STATE_ID"].ToString();
                    string DISTRICT_ID = dr["DISTRICT_ID"].ToString();
                    string CITY_ID = dr["CITY_ID"].ToString();
                    string BLOCK_ID = dr["BLOCK_ID"].ToString();
                    string WARD_ID = dr["WARD_ID"].ToString();
                    string GP_ID = dr["GP_ID"].ToString();
                    string VILLAGE_ID = dr["VILLAGE_ID"].ToString();
                    string PinCode = dr["PinCode"].ToString();
                    string AreaOfOperation = dr["AreaOfOperation"].ToString();
                    string CreatedBy = dr["CreatedBy"].ToString();
                    string CreationDate = dr["CreationDate"].ToString();
                    string UpdatedBy = dr["UpdatedBy"].ToString();
                    string UpdationDate = dr["UpdationDate"].ToString();
                    string CreatedIP = dr["CreatedIP"].ToString();
                    string UpdatedIP = dr["UpdatedIP"].ToString();
                    string IsActive = dr["IsActive"].ToString();
                    string TEHSIL_ID = dr["TEHSIL_ID"].ToString();
                    string DIVISION_ID = dr["DIVISION_ID"].ToString();
                    string IsSameCEOAddress = dr["IsSameCEOAddress"].ToString();
                    string CITYName = dr["CITYName"].ToString();
                    string BlockName = dr["BlockName"].ToString();
                    string WardName = dr["WardName"].ToString();
                    string GpName = dr["GpName"].ToString();
                    string VName = dr["VName"].ToString();
                    string CITY_IDD = dr["CITY_IDD"].ToString();
                    string CityTypes = dr["CityTypes"].ToString();
                    string AddressFor = dr["AddressFor"].ToString();
                    string IsPermanentSave = dr["IsPermanentSave"].ToString();
                    string oldSocietyId = dr["oldSocietyId"].ToString();

                    objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.NVarchar, SocietyId);
                    objDBAccess.AddParameterToCommand("@IsRural", DBFactoryParameterType.NVarchar, IsRural);
                    objDBAccess.AddParameterToCommand("@HouseNo", DBFactoryParameterType.NVarchar, HouseNo);
                    objDBAccess.AddParameterToCommand("@LaneStreetName", DBFactoryParameterType.NVarchar, LaneStreetName);
                    objDBAccess.AddParameterToCommand("@Locality", DBFactoryParameterType.NVarchar, Locality);
                    objDBAccess.AddParameterToCommand("@STATE_ID", DBFactoryParameterType.NVarchar, STATE_ID);
                    objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.NVarchar, DISTRICT_ID);
                    objDBAccess.AddParameterToCommand("@CITY_ID", DBFactoryParameterType.NVarchar, CITY_ID);
                    objDBAccess.AddParameterToCommand("@BLOCK_ID", DBFactoryParameterType.NVarchar, BLOCK_ID);
                    objDBAccess.AddParameterToCommand("@WARD_ID", DBFactoryParameterType.NVarchar, WARD_ID);
                    objDBAccess.AddParameterToCommand("@GP_ID", DBFactoryParameterType.NVarchar, GP_ID);
                    objDBAccess.AddParameterToCommand("@VILLAGE_ID", DBFactoryParameterType.NVarchar, VILLAGE_ID);
                    objDBAccess.AddParameterToCommand("@PinCode", DBFactoryParameterType.NVarchar, PinCode);
                    objDBAccess.AddParameterToCommand("@AreaOfOperation", DBFactoryParameterType.NVarchar, AreaOfOperation);
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.NVarchar, CreatedBy);
                    objDBAccess.AddParameterToCommand("@CreationDate", DBFactoryParameterType.NVarchar, CreationDate);
                    objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.NVarchar, UpdatedBy);
                    objDBAccess.AddParameterToCommand("@UpdationDate", DBFactoryParameterType.NVarchar, UpdationDate);
                    objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, CreatedIP);
                    objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, UpdatedIP);
                    objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.NVarchar, IsActive);
                    objDBAccess.AddParameterToCommand("@TEHSIL_ID", DBFactoryParameterType.NVarchar, TEHSIL_ID);
                    objDBAccess.AddParameterToCommand("@DIVISION_ID", DBFactoryParameterType.NVarchar, DIVISION_ID);
                    objDBAccess.AddParameterToCommand("@IsSameCEOAddress", DBFactoryParameterType.NVarchar, IsSameCEOAddress);
                    objDBAccess.AddParameterToCommand("@CITYName", DBFactoryParameterType.NVarchar, CITYName);
                    objDBAccess.AddParameterToCommand("@BlockName", DBFactoryParameterType.NVarchar, BlockName);
                    objDBAccess.AddParameterToCommand("@WardName", DBFactoryParameterType.NVarchar, WardName);
                    objDBAccess.AddParameterToCommand("@GpName", DBFactoryParameterType.NVarchar, GpName);
                    objDBAccess.AddParameterToCommand("@VName", DBFactoryParameterType.NVarchar, VName);
                    objDBAccess.AddParameterToCommand("@CITY_IDD", DBFactoryParameterType.NVarchar, CITY_IDD);
                    objDBAccess.AddParameterToCommand("@CityTypes", DBFactoryParameterType.NVarchar, CityTypes);
                    objDBAccess.AddParameterToCommand("@AddressFor", DBFactoryParameterType.NVarchar, AddressFor);
                    objDBAccess.AddParameterToCommand("@IsPermanentSave", DBFactoryParameterType.NVarchar, IsPermanentSave);
                    objDBAccess.AddParameterToCommand("@oldSocietyId", DBFactoryParameterType.NVarchar, oldSocietyId);
                    result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahakar.Models.CitizenModel", FunctionName = "public DataTable GetSocietyBasicDetails(string RegistrationNo)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            //Setdatalocal(dt);
            return result;

        }

        public int SaveAdministrativeDetails(DataTable sData, string SocietyId)
        {
            int result = 0;

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                foreach (DataRow dr in sData.Rows)
                {
                    cmd = objDBAccess.CreateCommand("SaveAdministrativeDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                    //string SocietyId = dr["SocietyId"].ToString();
                    string Name = dr["Name"].ToString();
                    string ContactNo = dr["ContactNo"].ToString();
                    string UserType = dr["UserType"].ToString();
                    string CreateBy = dr["CreateBy"].ToString();
                    string CreateDate = dr["CreateDate"].ToString();
                    string UpdateBy = dr["UpdateBy"].ToString();
                    string UpdateDate = dr["UpdateDate"].ToString();
                    string IsActive = dr["IsActive"].ToString();
                    string CreateIp = dr["CreateIp"].ToString();
                    string AadharNo = dr["AadharNo"].ToString();
                    string DOB = dr["DOB"].ToString();
                    string Gender = dr["Gender"].ToString();
                    string Photo = dr["Photo"].ToString();
                    string AdministratorDesignationId = dr["AdministratorDesignationId"].ToString();
                    string PostId = dr["PostId"].ToString();
                    string CategoryId = dr["CategoryId"].ToString();
                    string PhotoOld = dr["PhotoOld"].ToString();
                    string Aadh = dr["Aadh"].ToString();
                    string POSTNAME = dr["POSTNAME"].ToString();
                    string MEMBERTYPE = dr["MEMBERTYPE"].ToString();
                    string AppointedDate = dr["AppointedDate"].ToString();
                    string Address = dr["Address"].ToString();
                    string TotalMembersClassA = dr["TotalMembersClassA"].ToString();
                    string TotalMembersClassB = dr["TotalMembersClassB"].ToString();
                    string TotalMembersClassC = dr["TotalMembersClassC"].ToString();
                    string Occupation = dr["Occupation"].ToString();
                    string NominatedBy = dr["NominatedBy"].ToString();
                    string EMitraTransactionId = dr["EMitraTransactionId"].ToString();
                    string BoardType = dr["BoardType"].ToString();
                    string MembershipNo = dr["MembershipNo"].ToString();
                    string ShareAllotted = dr["ShareAllotted"].ToString();
                    string ShareCapitalAmount = dr["ShareCapitalAmount"].ToString();
                    string LWASTATUS_NOD_OD = dr["LWASTATUS_NOD_OD"].ToString();
                    string BhamashahId = dr["BhamashahId"].ToString();
                    string FatherName = dr["FatherName"].ToString();
                    string loaneeType = dr["loaneeType"].ToString();
                    string IsPermanentSave = dr["IsPermanentSave"].ToString();
                    string oldSocietyId = dr["oldSocietyId"].ToString();

                    objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.NVarchar, SocietyId);
                    objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, Name);
                    objDBAccess.AddParameterToCommand("@ContactNo", DBFactoryParameterType.NVarchar, ContactNo);
                    objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, UserType);
                    objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.NVarchar, CreateBy);
                    objDBAccess.AddParameterToCommand("@CreateDate", DBFactoryParameterType.NVarchar, CreateDate);
                    objDBAccess.AddParameterToCommand("@UpdateBy", DBFactoryParameterType.NVarchar, UpdateBy);
                    objDBAccess.AddParameterToCommand("@UpdateDate", DBFactoryParameterType.NVarchar, UpdateDate);
                    objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.NVarchar, IsActive);
                    objDBAccess.AddParameterToCommand("@CreateIp", DBFactoryParameterType.NVarchar, CreateIp);
                    objDBAccess.AddParameterToCommand("@AadharNo", DBFactoryParameterType.NVarchar, AadharNo);
                    objDBAccess.AddParameterToCommand("@DOB", DBFactoryParameterType.NVarchar, DOB);
                    objDBAccess.AddParameterToCommand("@Gender", DBFactoryParameterType.NVarchar, Gender);
                    objDBAccess.AddParameterToCommand("@Photo", DBFactoryParameterType.NVarchar, Photo);
                    objDBAccess.AddParameterToCommand("@AdministratorDesignationId", DBFactoryParameterType.NVarchar, AdministratorDesignationId);
                    objDBAccess.AddParameterToCommand("@PostId", DBFactoryParameterType.NVarchar, PostId);
                    objDBAccess.AddParameterToCommand("@CategoryId", DBFactoryParameterType.NVarchar, CategoryId);
                    objDBAccess.AddParameterToCommand("@PhotoOld", DBFactoryParameterType.NVarchar, PhotoOld);
                    objDBAccess.AddParameterToCommand("@Aadh", DBFactoryParameterType.NVarchar, Aadh);
                    objDBAccess.AddParameterToCommand("@POSTNAME", DBFactoryParameterType.NVarchar, POSTNAME);
                    objDBAccess.AddParameterToCommand("@MEMBERTYPE", DBFactoryParameterType.NVarchar, MEMBERTYPE);
                    objDBAccess.AddParameterToCommand("@AppointedDate", DBFactoryParameterType.NVarchar, AppointedDate);
                    objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, Address);
                    objDBAccess.AddParameterToCommand("@TotalMembersClassA", DBFactoryParameterType.NVarchar, TotalMembersClassA);
                    objDBAccess.AddParameterToCommand("@TotalMembersClassB", DBFactoryParameterType.NVarchar, TotalMembersClassB);
                    objDBAccess.AddParameterToCommand("@TotalMembersClassC", DBFactoryParameterType.NVarchar, TotalMembersClassC);
                    objDBAccess.AddParameterToCommand("@Occupation", DBFactoryParameterType.NVarchar, Occupation);
                    objDBAccess.AddParameterToCommand("@NominatedBy", DBFactoryParameterType.NVarchar, NominatedBy);
                    objDBAccess.AddParameterToCommand("@EMitraTransactionId", DBFactoryParameterType.NVarchar, EMitraTransactionId);
                    objDBAccess.AddParameterToCommand("@BoardType", DBFactoryParameterType.NVarchar, BoardType);
                    objDBAccess.AddParameterToCommand("@MembershipNo", DBFactoryParameterType.NVarchar, MembershipNo);
                    objDBAccess.AddParameterToCommand("@ShareAllotted", DBFactoryParameterType.NVarchar, ShareAllotted);
                    objDBAccess.AddParameterToCommand("@ShareCapitalAmount", DBFactoryParameterType.NVarchar, ShareCapitalAmount);
                    objDBAccess.AddParameterToCommand("@LWASTATUS_NOD_OD", DBFactoryParameterType.NVarchar, LWASTATUS_NOD_OD);
                    objDBAccess.AddParameterToCommand("@BhamashahId", DBFactoryParameterType.NVarchar, BhamashahId);
                    objDBAccess.AddParameterToCommand("@FatherName", DBFactoryParameterType.NVarchar, FatherName);
                    objDBAccess.AddParameterToCommand("@loaneeType", DBFactoryParameterType.NVarchar, loaneeType);
                    objDBAccess.AddParameterToCommand("@IsPermanentSave", DBFactoryParameterType.NVarchar, IsPermanentSave);
                    objDBAccess.AddParameterToCommand("@oldSocietyId", DBFactoryParameterType.NVarchar, oldSocietyId);
                    result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahakar.Models.CitizenModel", FunctionName = "public DataTable GetSocietyBasicDetails(string RegistrationNo)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            //Setdatalocal(dt);
            return result;

        }

        public int SaveSocietyAmendmentData(DataTable sData, string SocietyId)
        {
            int result = 0;

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                foreach (DataRow dr in sData.Rows)
                {
                    cmd = objDBAccess.CreateCommand("SaveSocietyAmendmentData", DBFactoryCommandType.PROCEDURE_COMMAND);
                    //string SocietyId = dr["SocietyId"].ToString();
                    string RegiYearId = dr["RegiYearId"].ToString();
                    string Name = dr["Name"].ToString();
                    string ByeLawsFile = dr["ByeLawsFile"].ToString();
                    string ResolutionNo = dr["ResolutionNo"].ToString();
                    string PaasedAGMDate = dr["PaasedAGMDate"].ToString();
                    string Remark = dr["Remark"].ToString();
                    string InsertDate = dr["InsertDate"].ToString();
                    string InsertBy = dr["InsertBy"].ToString();
                    string InsertIP = dr["InsertIP"].ToString();
                    string flAGMDeliveryProof = dr["flAGMDeliveryProof"].ToString();
                    string flSGMNotice = dr["flSGMNotice"].ToString();
                    string flSGMAttendanceSheet = dr["flSGMAttendanceSheet"].ToString();
                    string flByLowComparativeDocument = dr["flByLowComparativeDocument"].ToString();
                    string flAttendanceSheet = dr["flAttendanceSheet"].ToString();
                    string flResoloutionNotice = dr["flResoloutionNotice"].ToString();
                    string HouseBuildingNo = dr["HouseBuildingNo"].ToString();
                    string LaneStreetName = dr["LaneStreetName"].ToString();
                    string Locality = dr["Locality"].ToString();
                    string IsRural = dr["IsRural"].ToString();
                    string STATE_ID = dr["STATE_ID"].ToString();
                    string DIVISION_ID = dr["DIVISION_ID"].ToString();
                    string DISTRICT_ID = dr["DISTRICT_ID"].ToString();
                    string TEHSIL_ID = dr["TEHSIL_ID"].ToString();
                    string CityTypes = dr["CityTypes"].ToString();
                    string CITY_ID = dr["CITY_ID"].ToString();
                    string BLOCK_ID = dr["BLOCK_ID"].ToString();
                    string WARD_ID = dr["WARD_ID"].ToString();
                    string GP_ID = dr["GP_ID"].ToString();
                    string VILLAGE_ID = dr["VILLAGE_ID"].ToString();
                    string PinCode = dr["PinCode"].ToString();
                    string AmendmentType = dr["AmendmentType"].ToString();
                    string ChangeType = dr["ChangeType"].ToString();
                    string ChangeStatus = dr["ChangeStatus"].ToString();
                    string ProcessStatus = dr["ProcessStatus"].ToString();
                    string ComparisonFile = dr["ComparisonFile"].ToString();
                    string StatusId = dr["StatusId"].ToString();
                    string RegistrationCertificateFile = dr["RegistrationCertificateFile"].ToString();
                    string OldBylawsFile = dr["OldBylawsFile"].ToString();
                    string BRN = dr["BRN"].ToString();
                    string TotalMember = dr["TotalMember"].ToString();
                    string TotalPresantMember = dr["TotalPresantMember"].ToString();
                    string MeetingQuorum = dr["MeetingQuorum"].ToString();
                    string TotalVoteInSupport = dr["TotalVoteInSupport"].ToString();
                    string TotalVoteInOpposition = dr["TotalVoteInOpposition"].ToString();
                    string Section = dr["Section"].ToString();
                    string IsPermanentSave = dr["IsPermanentSave"].ToString();

                    objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.NVarchar, SocietyId);
                    objDBAccess.AddParameterToCommand("@RegiYearId", DBFactoryParameterType.NVarchar, RegiYearId);
                    objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, Name);
                    objDBAccess.AddParameterToCommand("@ByeLawsFile", DBFactoryParameterType.NVarchar, ByeLawsFile);
                    objDBAccess.AddParameterToCommand("@ResolutionNo", DBFactoryParameterType.NVarchar, ResolutionNo);
                    objDBAccess.AddParameterToCommand("@PaasedAGMDate", DBFactoryParameterType.NVarchar, PaasedAGMDate);
                    objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Remark);
                    objDBAccess.AddParameterToCommand("@InsertDate", DBFactoryParameterType.NVarchar, InsertDate);
                    objDBAccess.AddParameterToCommand("@InsertBy", DBFactoryParameterType.NVarchar, InsertBy);
                    objDBAccess.AddParameterToCommand("@InsertIP", DBFactoryParameterType.NVarchar, InsertIP);
                    objDBAccess.AddParameterToCommand("@flAGMDeliveryProof", DBFactoryParameterType.NVarchar, flAGMDeliveryProof);
                    objDBAccess.AddParameterToCommand("@flSGMNotice", DBFactoryParameterType.NVarchar, flSGMNotice);
                    objDBAccess.AddParameterToCommand("@flSGMAttendanceSheet", DBFactoryParameterType.NVarchar, flSGMAttendanceSheet);
                    objDBAccess.AddParameterToCommand("@flByLowComparativeDocument", DBFactoryParameterType.NVarchar, flByLowComparativeDocument);
                    objDBAccess.AddParameterToCommand("@flAttendanceSheet", DBFactoryParameterType.NVarchar, flAttendanceSheet);
                    objDBAccess.AddParameterToCommand("@flResoloutionNotice", DBFactoryParameterType.NVarchar, flResoloutionNotice);
                    objDBAccess.AddParameterToCommand("@HouseBuildingNo", DBFactoryParameterType.NVarchar, HouseBuildingNo);
                    objDBAccess.AddParameterToCommand("@LaneStreetName", DBFactoryParameterType.NVarchar, LaneStreetName);
                    objDBAccess.AddParameterToCommand("@Locality", DBFactoryParameterType.NVarchar, Locality);
                    objDBAccess.AddParameterToCommand("@IsRural", DBFactoryParameterType.NVarchar, IsRural);
                    objDBAccess.AddParameterToCommand("@STATE_ID", DBFactoryParameterType.NVarchar, STATE_ID);
                    objDBAccess.AddParameterToCommand("@DIVISION_ID", DBFactoryParameterType.NVarchar, DIVISION_ID);
                    objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.NVarchar, DISTRICT_ID);
                    objDBAccess.AddParameterToCommand("@TEHSIL_ID", DBFactoryParameterType.NVarchar, TEHSIL_ID);
                    objDBAccess.AddParameterToCommand("@CityTypes", DBFactoryParameterType.NVarchar, CityTypes);
                    objDBAccess.AddParameterToCommand("@CITY_ID", DBFactoryParameterType.NVarchar, CITY_ID);
                    objDBAccess.AddParameterToCommand("@BLOCK_ID", DBFactoryParameterType.NVarchar, BLOCK_ID);
                    objDBAccess.AddParameterToCommand("@WARD_ID", DBFactoryParameterType.NVarchar, WARD_ID);
                    objDBAccess.AddParameterToCommand("@GP_ID", DBFactoryParameterType.NVarchar, GP_ID);
                    objDBAccess.AddParameterToCommand("@VILLAGE_ID", DBFactoryParameterType.NVarchar, VILLAGE_ID);
                    objDBAccess.AddParameterToCommand("@PinCode", DBFactoryParameterType.NVarchar, PinCode);
                    objDBAccess.AddParameterToCommand("@AmendmentType", DBFactoryParameterType.NVarchar, AmendmentType);
                    objDBAccess.AddParameterToCommand("@ChangeType", DBFactoryParameterType.NVarchar, ChangeType);
                    objDBAccess.AddParameterToCommand("@ChangeStatus", DBFactoryParameterType.NVarchar, ChangeStatus);
                    objDBAccess.AddParameterToCommand("@ProcessStatus", DBFactoryParameterType.NVarchar, ProcessStatus);
                    objDBAccess.AddParameterToCommand("@ComparisonFile", DBFactoryParameterType.NVarchar, ComparisonFile);
                    objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.NVarchar, StatusId);
                    objDBAccess.AddParameterToCommand("@RegistrationCertificateFile", DBFactoryParameterType.NVarchar, RegistrationCertificateFile);
                    objDBAccess.AddParameterToCommand("@OldBylawsFile", DBFactoryParameterType.NVarchar, OldBylawsFile);
                    objDBAccess.AddParameterToCommand("@BRN", DBFactoryParameterType.NVarchar, BRN);
                    objDBAccess.AddParameterToCommand("@TotalMember", DBFactoryParameterType.NVarchar, TotalMember);
                    objDBAccess.AddParameterToCommand("@TotalPresantMember", DBFactoryParameterType.NVarchar, TotalPresantMember);
                    objDBAccess.AddParameterToCommand("@MeetingQuorum", DBFactoryParameterType.NVarchar, MeetingQuorum);
                    objDBAccess.AddParameterToCommand("@TotalVoteInSupport", DBFactoryParameterType.NVarchar, TotalVoteInSupport);
                    objDBAccess.AddParameterToCommand("@TotalVoteInOpposition", DBFactoryParameterType.NVarchar, TotalVoteInOpposition);
                    objDBAccess.AddParameterToCommand("@Section", DBFactoryParameterType.NVarchar, Section);
                    objDBAccess.AddParameterToCommand("@IsPermanentSave", DBFactoryParameterType.NVarchar, IsPermanentSave);
                    result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahakar.Models.CitizenModel", FunctionName = "public DataTable GetSocietyBasicDetails(string RegistrationNo)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            //Setdatalocal(dt);
            return result;

        }

        public int SocietyAmendmentDocuments(DataTable sData, string SocietyId, string AmendmentId)
        {
            int result = 0;

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                foreach (DataRow dr in sData.Rows)
                {
                    cmd = objDBAccess.CreateCommand("SocietyAmendmentDocuments", DBFactoryCommandType.PROCEDURE_COMMAND);
                    //string SocietyId = dr["SocietyId"].ToString();
                    //string SocietyId = dr["SocietyId"].ToString();
                    //string AmendmentId = dr["AmendmentId"].ToString();
                    string DocumentType = dr["DocumentType"].ToString();
                    string DocumentName = dr["DocumentName"].ToString();
                    string CreatedBy = dr["CreatedBy"].ToString();
                    string CreatedDate = dr["CreatedDate"].ToString();

                    objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.NVarchar, SocietyId);
                    objDBAccess.AddParameterToCommand("@AmendmentId", DBFactoryParameterType.NVarchar, AmendmentId);
                    objDBAccess.AddParameterToCommand("@DocumentType", DBFactoryParameterType.NVarchar, DocumentType);
                    objDBAccess.AddParameterToCommand("@DocumentName", DBFactoryParameterType.NVarchar, DocumentName);
                    objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.NVarchar, CreatedBy);
                    objDBAccess.AddParameterToCommand("@CreatedDate", DBFactoryParameterType.NVarchar, CreatedDate);
                    result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahakar.Models.CitizenModel", FunctionName = "public DataTable GetSocietyBasicDetails(string RegistrationNo)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            //Setdatalocal(dt);
            return result;

        }

        public int SaveAdministrativeDetailsAmendment(DataTable sData, string SocietyId, string AmendmentId)
        {
            int result = 0;

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                foreach (DataRow dr in sData.Rows)
                {
                    cmd = objDBAccess.CreateCommand("SaveAdministrativeDetailsAmendment", DBFactoryCommandType.PROCEDURE_COMMAND);
                    //string SocietyId = dr["SocietyId"].ToString();
                    string Name = dr["Name"].ToString();
                    string ContactNo = dr["ContactNo"].ToString();
                    string UserType = dr["UserType"].ToString();
                    string CreateBy = dr["CreateBy"].ToString();
                    string CreateDate = dr["CreateDate"].ToString();
                    string UpdateBy = dr["UpdateBy"].ToString();
                    string UpdateDate = dr["UpdateDate"].ToString();
                    string IsActive = dr["IsActive"].ToString();
                    string CreateIp = dr["CreateIp"].ToString();
                    string AadharNo = dr["AadharNo"].ToString();
                    string DOB = dr["DOB"].ToString();
                    string Gender = dr["Gender"].ToString();
                    string Photo = dr["Photo"].ToString();
                    string AdministratorDesignationId = dr["AdministratorDesignationId"].ToString();
                    string PostId = dr["PostId"].ToString();
                    string CategoryId = dr["CategoryId"].ToString();
                    string PhotoOld = dr["PhotoOld"].ToString();
                    string Aadh = dr["Aadh"].ToString();
                    string POSTNAME = dr["POSTNAME"].ToString();
                    string MEMBERTYPE = dr["MEMBERTYPE"].ToString();
                    string AppointedDate = dr["AppointedDate"].ToString();
                    string Address = dr["Address"].ToString();
                    string TotalMembersClassA = dr["TotalMembersClassA"].ToString();
                    string TotalMembersClassB = dr["TotalMembersClassB"].ToString();
                    string TotalMembersClassC = dr["TotalMembersClassC"].ToString();
                    string Occupation = dr["Occupation"].ToString();
                    string NominatedBy = dr["NominatedBy"].ToString();
                    string EMitraTransactionId = dr["EMitraTransactionId"].ToString();
                    string BoardType = dr["BoardType"].ToString();
                    string MembershipNo = dr["MembershipNo"].ToString();
                    string ShareAllotted = dr["ShareAllotted"].ToString();
                    string ShareCapitalAmount = dr["ShareCapitalAmount"].ToString();
                    string LWASTATUS_NOD_OD = dr["LWASTATUS_NOD_OD"].ToString();
                    string BhamashahId = dr["BhamashahId"].ToString();
                    string FatherName = dr["FatherName"].ToString();
                    string loaneeType = dr["loaneeType"].ToString();
                    string IsPermanentSave = dr["IsPermanentSave"].ToString();
                    string RequestType = dr["RequestType"].ToString();
                    string RequestStatus = dr["RequestStatus"].ToString();
                    string oldAdministrativeId = dr["oldAdministrativeId"].ToString();
                    string ProcessStatus = dr["ProcessStatus"].ToString();
                    string StatusId = dr["StatusId"].ToString();
                    string SOCIETIESAUTHORIZEDPERSONS = dr["SOCIETIESAUTHORIZEDPERSONS"].ToString();
                    //string AmendmentId = dr["AmendmentId"].ToString();
                    string Remark = dr["Remark"].ToString();
                    string RemovalType = dr["RemovalType"].ToString();
                    string RemovalFile = dr["RemovalFile"].ToString();

                    objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.NVarchar, SocietyId);
                    objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, Name);
                    objDBAccess.AddParameterToCommand("@ContactNo", DBFactoryParameterType.NVarchar, ContactNo);
                    objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.NVarchar, UserType);
                    objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.NVarchar, CreateBy);
                    objDBAccess.AddParameterToCommand("@CreateDate", DBFactoryParameterType.NVarchar, CreateDate);
                    objDBAccess.AddParameterToCommand("@UpdateBy", DBFactoryParameterType.NVarchar, UpdateBy);
                    objDBAccess.AddParameterToCommand("@UpdateDate", DBFactoryParameterType.NVarchar, UpdateDate);
                    objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.NVarchar, IsActive);
                    objDBAccess.AddParameterToCommand("@CreateIp", DBFactoryParameterType.NVarchar, CreateIp);
                    objDBAccess.AddParameterToCommand("@AadharNo", DBFactoryParameterType.NVarchar, AadharNo);
                    objDBAccess.AddParameterToCommand("@DOB", DBFactoryParameterType.NVarchar, DOB);
                    objDBAccess.AddParameterToCommand("@Gender", DBFactoryParameterType.NVarchar, Gender);
                    objDBAccess.AddParameterToCommand("@Photo", DBFactoryParameterType.NVarchar, Photo);
                    objDBAccess.AddParameterToCommand("@AdministratorDesignationId", DBFactoryParameterType.NVarchar, AdministratorDesignationId);
                    objDBAccess.AddParameterToCommand("@PostId", DBFactoryParameterType.NVarchar, PostId);
                    objDBAccess.AddParameterToCommand("@CategoryId", DBFactoryParameterType.NVarchar, CategoryId);
                    objDBAccess.AddParameterToCommand("@PhotoOld", DBFactoryParameterType.NVarchar, PhotoOld);
                    objDBAccess.AddParameterToCommand("@Aadh", DBFactoryParameterType.NVarchar, Aadh);
                    objDBAccess.AddParameterToCommand("@POSTNAME", DBFactoryParameterType.NVarchar, POSTNAME);
                    objDBAccess.AddParameterToCommand("@MEMBERTYPE", DBFactoryParameterType.NVarchar, MEMBERTYPE);
                    objDBAccess.AddParameterToCommand("@AppointedDate", DBFactoryParameterType.NVarchar, AppointedDate);
                    objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, Address);
                    objDBAccess.AddParameterToCommand("@TotalMembersClassA", DBFactoryParameterType.NVarchar, TotalMembersClassA);
                    objDBAccess.AddParameterToCommand("@TotalMembersClassB", DBFactoryParameterType.NVarchar, TotalMembersClassB);
                    objDBAccess.AddParameterToCommand("@TotalMembersClassC", DBFactoryParameterType.NVarchar, TotalMembersClassC);
                    objDBAccess.AddParameterToCommand("@Occupation", DBFactoryParameterType.NVarchar, Occupation);
                    objDBAccess.AddParameterToCommand("@NominatedBy", DBFactoryParameterType.NVarchar, NominatedBy);
                    objDBAccess.AddParameterToCommand("@EMitraTransactionId", DBFactoryParameterType.NVarchar, EMitraTransactionId);
                    objDBAccess.AddParameterToCommand("@BoardType", DBFactoryParameterType.NVarchar, BoardType);
                    objDBAccess.AddParameterToCommand("@MembershipNo", DBFactoryParameterType.NVarchar, MembershipNo);
                    objDBAccess.AddParameterToCommand("@ShareAllotted", DBFactoryParameterType.NVarchar, ShareAllotted);
                    objDBAccess.AddParameterToCommand("@ShareCapitalAmount", DBFactoryParameterType.NVarchar, ShareCapitalAmount);
                    objDBAccess.AddParameterToCommand("@LWASTATUS_NOD_OD", DBFactoryParameterType.NVarchar, LWASTATUS_NOD_OD);
                    objDBAccess.AddParameterToCommand("@BhamashahId", DBFactoryParameterType.NVarchar, BhamashahId);
                    objDBAccess.AddParameterToCommand("@FatherName", DBFactoryParameterType.NVarchar, FatherName);
                    objDBAccess.AddParameterToCommand("@loaneeType", DBFactoryParameterType.NVarchar, loaneeType);
                    objDBAccess.AddParameterToCommand("@IsPermanentSave", DBFactoryParameterType.NVarchar, IsPermanentSave);
                    objDBAccess.AddParameterToCommand("@RequestType", DBFactoryParameterType.NVarchar, RequestType);
                    objDBAccess.AddParameterToCommand("@RequestStatus", DBFactoryParameterType.NVarchar, RequestStatus);
                    objDBAccess.AddParameterToCommand("@oldAdministrativeId", DBFactoryParameterType.NVarchar, oldAdministrativeId);
                    objDBAccess.AddParameterToCommand("@ProcessStatus", DBFactoryParameterType.NVarchar, ProcessStatus);
                    objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.NVarchar, StatusId);
                    objDBAccess.AddParameterToCommand("@SOCIETIESAUTHORIZEDPERSONS", DBFactoryParameterType.NVarchar, SOCIETIESAUTHORIZEDPERSONS);
                    objDBAccess.AddParameterToCommand("@AmendmentId", DBFactoryParameterType.NVarchar, AmendmentId);
                    objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, Remark);
                    objDBAccess.AddParameterToCommand("@RemovalType", DBFactoryParameterType.NVarchar, RemovalType);
                    objDBAccess.AddParameterToCommand("@RemovalFile", DBFactoryParameterType.NVarchar, RemovalFile);
                    result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahakar.Models.CitizenModel", FunctionName = "  public int SaveAdministrativeDetailsAmendment(DataTable sData, string SocietyId, string AmendmentId)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            //Setdatalocal(dt);
            return result;

        }

        public int SaveSocietyApplicationFlowAmendment(DataTable sData, string SocietyId, string AmendmentId)
        {
            int result = 0;

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                foreach (DataRow dr in sData.Rows)
                {
                    cmd = objDBAccess.CreateCommand("SaveSocietyApplicationFlowAmendment", DBFactoryCommandType.PROCEDURE_COMMAND);
                    //string SocietyId = dr["SocietyId"].ToString();
                    //string SocietyId = dr["SocietyId"].ToString();
                    //string AmendmentId = dr["AmendmentId"].ToString();
                    //string SocietyID = dr["SocietyID"].ToString();
                    string StatusId = dr["StatusId"].ToString();
                    string UserId = dr["UserId"].ToString();
                    string Remark = dr["Remark"].ToString();
                    string ActionDate = dr["ActionDate"].ToString();
                    string CreateBy = dr["CreateBy"].ToString();
                    string CreateIP = dr["CreateIP"].ToString();
                    string CreateBY_SSO_ID = dr["CreateBY_SSO_ID"].ToString();
                    string REGISTRATIONNO = dr["REGISTRATIONNO"].ToString();
                    string IsAmendment = dr["IsAmendment"].ToString();
                    string EAppRequestID = dr["EAppRequestID"].ToString();
                    string OldSocietyId = dr["OldSocietyId"].ToString();
                    //string AmendmentId = dr["AmendmentId"].ToString();

                    objDBAccess.AddParameterToCommand("SocietyID", DBFactoryParameterType.NVarchar, SocietyId);
                    objDBAccess.AddParameterToCommand("StatusId", DBFactoryParameterType.NVarchar, StatusId);
                    objDBAccess.AddParameterToCommand("UserId", DBFactoryParameterType.NVarchar, UserId);
                    objDBAccess.AddParameterToCommand("Remark", DBFactoryParameterType.NVarchar, Remark);
                    objDBAccess.AddParameterToCommand("ActionDate", DBFactoryParameterType.NVarchar, ActionDate);
                    objDBAccess.AddParameterToCommand("CreateBy", DBFactoryParameterType.NVarchar, CreateBy);
                    objDBAccess.AddParameterToCommand("CreateIP", DBFactoryParameterType.NVarchar, CreateIP);
                    objDBAccess.AddParameterToCommand("CreateBY_SSO_ID", DBFactoryParameterType.NVarchar, CreateBY_SSO_ID);
                    objDBAccess.AddParameterToCommand("REGISTRATIONNO ", DBFactoryParameterType.NVarchar, REGISTRATIONNO);
                    objDBAccess.AddParameterToCommand("IsAmendment", DBFactoryParameterType.NVarchar, IsAmendment);
                    objDBAccess.AddParameterToCommand("EAppRequestID", DBFactoryParameterType.NVarchar, EAppRequestID);
                    objDBAccess.AddParameterToCommand("OldSocietyId", DBFactoryParameterType.NVarchar, OldSocietyId);
                    objDBAccess.AddParameterToCommand("AmendmentId", DBFactoryParameterType.NVarchar, AmendmentId);
                    result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahakar.Models.CitizenModel", FunctionName = "public DataTable GetSocietyBasicDetails(string RegistrationNo)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            //Setdatalocal(dt);
            return result;

        }


        public string Setdatalocal(DataSet ds)
        {
            string Result = "";
            DataTable dt0 = ds.Tables[0];
            DataTable dt1 = ds.Tables[1];
            DataTable dt2 = ds.Tables[2];
            DataTable dt3 = ds.Tables[3];
            DataTable dt4 = ds.Tables[4];
            DataTable dt5 = ds.Tables[5];
            DataTable dt6 = ds.Tables[6];
            try
            {
                int i = 0;
                foreach (DataTable dt in ds.Tables)
                {
                    try
                    {
                        objDBAccess.OpenConnection();
                        SqlCommand cmd = new SqlCommand();
                        cmd = objDBAccess.CreateCommand("InsertProdDataToLocal", DBFactoryCommandType.PROCEDURE_COMMAND);



                        if (i == 0)
                        {
                            cmd.Parameters.AddWithValue("@Society", dt0);
                            cmd.Parameters.AddWithValue("@Type", 0);
                        }
                        if (i == 1)
                        {
                            cmd.Parameters.AddWithValue("@ApplicationFlow", dt1);
                            cmd.Parameters.AddWithValue("@Type", 1);
                            cmd.Parameters.AddWithValue("@NewScoietyId", Result);
                        }
                        if (i == 2)
                        {
                            cmd.Parameters.AddWithValue("@SocietyRegOfficeDetails", dt2);
                            cmd.Parameters.AddWithValue("@Type", 2);
                            cmd.Parameters.AddWithValue("@NewScoietyId", Result);
                        }
                        if (i == 3)
                        {
                            cmd.Parameters.AddWithValue("@AdministrativeDetails", dt3);
                            cmd.Parameters.AddWithValue("@Type", 3);
                            cmd.Parameters.AddWithValue("@NewScoietyId", Result);
                        }

                        if (i == 4)
                        {
                            cmd.Parameters.AddWithValue("@SocietyAmendmentData", dt4);
                            cmd.Parameters.AddWithValue("@Type", 4);
                            cmd.Parameters.AddWithValue("@NewScoietyId", Result);
                        }
                        if (i == 5)
                        {
                            cmd.Parameters.AddWithValue("@SocietyAmendmentDocuments", dt5);
                            cmd.Parameters.AddWithValue("@Type", 6);
                            cmd.Parameters.AddWithValue("@NewScoietyId", Result);
                        }
                        if (i == 6)
                        {
                            cmd.Parameters.AddWithValue("@AdministrativeDetailsAmendment", dt6);
                            cmd.Parameters.AddWithValue("@Type", 5);
                            cmd.Parameters.AddWithValue("@NewScoietyId", Result);
                        }
                        Result = objDBAccess.ExecuteScalarCommand().ToString();
                    }
                    catch (Exception ex)
                    {

                    }
                    i = i + 1;
                }
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.SocietyModel", FunctionName = "SocietyRegistration(SocietyRegistrationEntity _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public string SaveSocietyData(DataTable dtt)
        {
            DataSet dt = new DataSet();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    cmd = objDBAccess.CreateCommand("GetSocietyData", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("SocietyID", DBFactoryParameterType.NVarchar, dtt.Rows[i]["SocietyID"]);
                    objDBAccess.AddParameterToCommand("RegistrationNo", DBFactoryParameterType.NVarchar, dtt.Rows[i]["RegistrationNo"]);
                    objDBAccess.AddParameterToCommand("SocietyName", DBFactoryParameterType.NVarchar, dtt.Rows[i]["SocietyName"]);
                    objDBAccess.AddParameterToCommand("SocietyTypeId", DBFactoryParameterType.NVarchar, dtt.Rows[i]["SocietyTypeId"]);
                    objDBAccess.AddParameterToCommand("BusinessId", DBFactoryParameterType.NVarchar, dtt.Rows[i]["BusinessId"]);
                    objDBAccess.AddParameterToCommand("LiablityId", DBFactoryParameterType.NVarchar, dtt.Rows[i]["LiablityId"]);
                    objDBAccess.AddParameterToCommand("MemberLiablity", DBFactoryParameterType.NVarchar, dtt.Rows[i]["MemberLiablity"]);
                    objDBAccess.AddParameterToCommand("ClassId", DBFactoryParameterType.NVarchar, dtt.Rows[i]["ClassId"]);
                    objDBAccess.AddParameterToCommand("SubClassId", DBFactoryParameterType.NVarchar, dtt.Rows[i]["SubClassId"]);
                    objDBAccess.AddParameterToCommand("StatusId", DBFactoryParameterType.NVarchar, dtt.Rows[i]["StatusId"]);
                    objDBAccess.AddParameterToCommand("IsActive", DBFactoryParameterType.NVarchar, dtt.Rows[i]["IsActive"]);
                    objDBAccess.AddParameterToCommand("Remarks", DBFactoryParameterType.NVarchar, dtt.Rows[i]["Remarks"]);
                    objDBAccess.AddParameterToCommand("Certificate", DBFactoryParameterType.NVarchar, dtt.Rows[i]["Certificate"]);
                    objDBAccess.AddParameterToCommand("Request_Id", DBFactoryParameterType.NVarchar, dtt.Rows[i]["Request_Id"]);
                    objDBAccess.AddParameterToCommand("Amount", DBFactoryParameterType.NVarchar, dtt.Rows[i]["Amount"]);
                    objDBAccess.AddParameterToCommand("ApplicationFileBy", DBFactoryParameterType.NVarchar, dtt.Rows[i]["ApplicationFileBy"]);
                    objDBAccess.AddParameterToCommand("AssistantId", DBFactoryParameterType.NVarchar, dtt.Rows[i]["AssistantId"]);
                    objDBAccess.AddParameterToCommand("SeqId", DBFactoryParameterType.NVarchar, dtt.Rows[i]["SeqId"]);
                    objDBAccess.AddParameterToCommand("RehAct", DBFactoryParameterType.NVarchar, dtt.Rows[i]["RehAct"]);
                    objDBAccess.AddParameterToCommand("BankPassBook", DBFactoryParameterType.NVarchar, dtt.Rows[i]["BankPassBook"]);
                    objDBAccess.AddParameterToCommand("TotalShareAmount", DBFactoryParameterType.NVarchar, dtt.Rows[i]["TotalShareAmount"]);
                    objDBAccess.AddParameterToCommand("BankOpeningCertificate", DBFactoryParameterType.NVarchar, dtt.Rows[i]["BankOpeningCertificate"]);
                    objDBAccess.AddParameterToCommand("BRN", DBFactoryParameterType.NVarchar, dtt.Rows[i]["BRN"]);
                    objDBAccess.AddParameterToCommand("InspectorId", DBFactoryParameterType.NVarchar, dtt.Rows[i]["InspectorId"]);
                    objDBAccess.AddParameterToCommand("RegistrarId", DBFactoryParameterType.NVarchar, dtt.Rows[i]["RegistrarId"]);
                    objDBAccess.AddParameterToCommand("CreatedBy", DBFactoryParameterType.NVarchar, dtt.Rows[i]["CreatedBy"]);
                    objDBAccess.AddParameterToCommand("CreationDate", DBFactoryParameterType.NVarchar, dtt.Rows[i]["CreationDate"]);
                    objDBAccess.AddParameterToCommand("UpdatedBy", DBFactoryParameterType.NVarchar, dtt.Rows[i]["UpdatedBy"]);
                    objDBAccess.AddParameterToCommand("UpdationDate", DBFactoryParameterType.NVarchar, dtt.Rows[i]["UpdationDate"]);
                    objDBAccess.AddParameterToCommand("CreatedIP", DBFactoryParameterType.NVarchar, dtt.Rows[i]["CreatedIP"]);
                    objDBAccess.AddParameterToCommand("UpdatedIP", DBFactoryParameterType.NVarchar, dtt.Rows[i]["UpdatedIP"]);
                    objDBAccess.AddParameterToCommand("TotalMembersClassA", DBFactoryParameterType.NVarchar, dtt.Rows[i]["TotalMembersClassA"]);
                    objDBAccess.AddParameterToCommand("TotalMembersClassB", DBFactoryParameterType.NVarchar, dtt.Rows[i]["TotalMembersClassB"]);
                    objDBAccess.AddParameterToCommand("TotalMembersClassC", DBFactoryParameterType.NVarchar, dtt.Rows[i]["TotalMembersClassC"]);
                    objDBAccess.AddParameterToCommand("RegistrationCertificateFile", DBFactoryParameterType.NVarchar, dtt.Rows[i]["RegistrationCertificateFile"]);
                    objDBAccess.AddParameterToCommand("RegistrationDate", DBFactoryParameterType.NVarchar, dtt.Rows[i]["RegistrationDate"]);
                    objDBAccess.AddParameterToCommand("ActId", DBFactoryParameterType.NVarchar, dtt.Rows[i]["ActId"]);
                    objDBAccess.AddParameterToCommand("SSOID", DBFactoryParameterType.NVarchar, dtt.Rows[i]["SSOID"]);
                    objDBAccess.AddParameterToCommand("NoOfBranches", DBFactoryParameterType.NVarchar, dtt.Rows[i]["NoOfBranches"]);
                    objDBAccess.AddParameterToCommand("ElectionDate", DBFactoryParameterType.NVarchar, dtt.Rows[i]["ElectionDate"]);
                    objDBAccess.AddParameterToCommand("UserType", DBFactoryParameterType.NVarchar, dtt.Rows[i]["UserType"]);
                    objDBAccess.AddParameterToCommand("BylawsFile", DBFactoryParameterType.NVarchar, dtt.Rows[i]["BylawsFile"]);
                    objDBAccess.AddParameterToCommand("RulesRegulationFile", DBFactoryParameterType.NVarchar, dtt.Rows[i]["RulesRegulationFile"]);
                    objDBAccess.AddParameterToCommand("SocietyCategoryId", DBFactoryParameterType.NVarchar, dtt.Rows[i]["SocietyCategoryId"]);
                    objDBAccess.AddParameterToCommand("KIOSKCODE", DBFactoryParameterType.NVarchar, dtt.Rows[i]["KIOSKCODE"]);
                    objDBAccess.AddParameterToCommand("OldSocietyId", DBFactoryParameterType.NVarchar, dtt.Rows[i]["OldSocietyId"]);
                    objDBAccess.AddParameterToCommand("OTP", DBFactoryParameterType.NVarchar, dtt.Rows[i]["OTP"]);
                    objDBAccess.AddParameterToCommand("OTPDateTime", DBFactoryParameterType.NVarchar, dtt.Rows[i]["OTPDateTime"]);
                    objDBAccess.AddParameterToCommand("IsRSSC", DBFactoryParameterType.NVarchar, dtt.Rows[i]["IsRSSC"]);
                    objDBAccess.AddParameterToCommand("AGMDate", DBFactoryParameterType.NVarchar, dtt.Rows[i]["AGMDate"]);
                    objDBAccess.AddParameterToCommand("SportsAssociationId", DBFactoryParameterType.NVarchar, dtt.Rows[i]["SportsAssociationId"]);
                    objDBAccess.AddParameterToCommand("IsAffiliatedByRSSC", DBFactoryParameterType.NVarchar, dtt.Rows[i]["IsAffiliatedByRSSC"]);
                    objDBAccess.AddParameterToCommand("RSSCRegNo", DBFactoryParameterType.NVarchar, dtt.Rows[i]["RSSCRegNo"]);
                    objDBAccess.AddParameterToCommand("RSSCAffiliatedDate", DBFactoryParameterType.NVarchar, dtt.Rows[i]["RSSCAffiliatedDate"]);
                    objDBAccess.AddParameterToCommand("RSSCFileName", DBFactoryParameterType.NVarchar, dtt.Rows[i]["RSSCFileName"]);
                    objDBAccess.AddParameterToCommand("IsAffiliatedBySOC", DBFactoryParameterType.NVarchar, dtt.Rows[i]["IsAffiliatedBySOC"]);
                    objDBAccess.AddParameterToCommand("SOCRegNo", DBFactoryParameterType.NVarchar, dtt.Rows[i]["SOCRegNo"]);
                    objDBAccess.AddParameterToCommand("SOCAffiliatedDate", DBFactoryParameterType.NVarchar, dtt.Rows[i]["SOCAffiliatedDate"]);
                    objDBAccess.AddParameterToCommand("SOCFileName", DBFactoryParameterType.NVarchar, dtt.Rows[i]["SOCFileName"]);
                    objDBAccess.AddParameterToCommand("IsAffiliatedByDLSC", DBFactoryParameterType.NVarchar, dtt.Rows[i]["IsAffiliatedByDLSC"]);
                    objDBAccess.AddParameterToCommand("DLSCRegNo", DBFactoryParameterType.NVarchar, dtt.Rows[i]["DLSCRegNo"]);
                    objDBAccess.AddParameterToCommand("DLSCAffiliatedDate", DBFactoryParameterType.NVarchar, dtt.Rows[i]["DLSCAffiliatedDate"]);
                    objDBAccess.AddParameterToCommand("DLSCFileName", DBFactoryParameterType.NVarchar, dtt.Rows[i]["DLSCFileName"]);
                    objDBAccess.AddParameterToCommand("MemberListFileName", DBFactoryParameterType.NVarchar, dtt.Rows[i]["MemberListFileName"]);
                    objDBAccess.AddParameterToCommand("SubmitStatus", DBFactoryParameterType.NVarchar, dtt.Rows[i]["SubmitStatus"]);
                    objDBAccess.AddParameterToCommand("IsNewRegistration", DBFactoryParameterType.NVarchar, dtt.Rows[i]["IsNewRegistration"]);
                    objDBAccess.AddParameterToCommand("PaidUpShareCapital", DBFactoryParameterType.NVarchar, dtt.Rows[i]["PaidUpShareCapital"]);
                    objDBAccess.AddParameterToCommand("SingleShareValue", DBFactoryParameterType.NVarchar, dtt.Rows[i]["SingleShareValue"]);
                    objDBAccess.AddParameterToCommand("NoOfMemberInAGM", DBFactoryParameterType.NVarchar, dtt.Rows[i]["NoOfMemberInAGM"]);
                    objDBAccess.AddParameterToCommand("EntranceFeesCollected", DBFactoryParameterType.NVarchar, dtt.Rows[i]["EntranceFeesCollected"]);
                    objDBAccess.AddParameterToCommand("WorkPlanEcoViabilityFile", DBFactoryParameterType.NVarchar, dtt.Rows[i]["WorkPlanEcoViabilityFile"]);
                    objDBAccess.AddParameterToCommand("GeneralMeetingProcedureFile", DBFactoryParameterType.NVarchar, dtt.Rows[i]["GeneralMeetingProcedureFile"]);
                    objDBAccess.AddParameterToCommand("BalanceSheetExcelFile", DBFactoryParameterType.NVarchar, dtt.Rows[i]["BalanceSheetExcelFile"]);
                    objDBAccess.AddParameterToCommand("LiabilityOfMember", DBFactoryParameterType.NVarchar, dtt.Rows[i]["LiabilityOfMember"]);
                    objDBAccess.AddParameterToCommand("TotalNumberOfMembers", DBFactoryParameterType.NVarchar, dtt.Rows[i]["TotalNumberOfMembers"]);
                    objDBAccess.AddParameterToCommand("ObjectsFile", DBFactoryParameterType.NVarchar, dtt.Rows[i]["ObjectsFile"]);
                    objDBAccess.AddParameterToCommand("IsAffiliationBySLSA", DBFactoryParameterType.NVarchar, dtt.Rows[i]["IsAffiliationBySLSA"]);
                    objDBAccess.AddParameterToCommand("SLSARegNo", DBFactoryParameterType.NVarchar, dtt.Rows[i]["SLSARegNo"]);
                    objDBAccess.AddParameterToCommand("SLSAAffiliatedDate", DBFactoryParameterType.NVarchar, dtt.Rows[i]["SLSAAffiliatedDate"]);
                    objDBAccess.AddParameterToCommand("SLSAFileName", DBFactoryParameterType.NVarchar, dtt.Rows[i]["SLSAFileName"]);
                    objDBAccess.AddParameterToCommand("IsPermanentSave", DBFactoryParameterType.NVarchar, dtt.Rows[i]["IsPermanentSave"]);
                    objDBAccess.AddParameterToCommand("OldRegistrationNo", DBFactoryParameterType.NVarchar, dtt.Rows[i]["OldRegistrationNo"]);
                    objDBAccess.AddParameterToCommand("ISPROFILR", DBFactoryParameterType.NVarchar, dtt.Rows[i]["ISPROFILR"]);
                    objDBAccess.AddParameterToCommand("SOCIETIESAUTHORIZEDPERSONS", DBFactoryParameterType.NVarchar, dtt.Rows[i]["SOCIETIESAUTHORIZEDPERSONS"]);
                    objDBAccess.AddParameterToCommand("SocietyReportFile", DBFactoryParameterType.NVarchar, dtt.Rows[i]["SocietyReportFile"]);
                    objDBAccess.AddParameterToCommand("ExcelUploadId", DBFactoryParameterType.NVarchar, dtt.Rows[i]["ExcelUploadId"]);
                    objDBAccess.AddParameterToCommand("RejectionDate", DBFactoryParameterType.NVarchar, dtt.Rows[i]["RejectionDate"]);
                    objDBAccess.AddParameterToCommand("RejectionFileName", DBFactoryParameterType.NVarchar, dtt.Rows[i]["RejectionFileName"]);
                    objDBAccess.AddParameterToCommand("IsDev", DBFactoryParameterType.NVarchar, dtt.Rows[i]["IsDev"]);
                    objDBAccess.AddParameterToCommand("IsAmendment", DBFactoryParameterType.NVarchar, dtt.Rows[i]["IsAmendment"]);
                    objDBAccess.AddParameterToCommand("NameChangeFile", DBFactoryParameterType.NVarchar, dtt.Rows[i]["NameChangeFile"]);
                    objDBAccess.AddParameterToCommand("SocietyStatusId", DBFactoryParameterType.NVarchar, dtt.Rows[i]["SocietyStatusId"]);
                    objDBAccess.AddParameterToCommand("IsSWS", DBFactoryParameterType.NVarchar, dtt.Rows[i]["IsSWS"]);
                    objDBAccess.AddParameterToCommand("IsAmendmentAtProfiling", DBFactoryParameterType.NVarchar, dtt.Rows[i]["IsAmendmentAtProfiling"]);
                    objDBAccess.AddParameterToCommand("AmendmentType", DBFactoryParameterType.NVarchar, dtt.Rows[i]["AmendmentType"]);
                    objDBAccess.AddParameterToCommand("OldExecutiveCommittee", DBFactoryParameterType.NVarchar, dtt.Rows[i]["OldExecutiveCommittee"]);
                    objDBAccess.AddParameterToCommand("SocietyCode", DBFactoryParameterType.NVarchar, dtt.Rows[i]["SocietyCode"]);
                    objDBAccess.AddParameterToCommand("IsRural", DBFactoryParameterType.NVarchar, dtt.Rows[i]["IsRural"]);
                    objDBAccess.AddParameterToCommand("HouseNo", DBFactoryParameterType.NVarchar, dtt.Rows[i]["HouseNo"]);
                    objDBAccess.AddParameterToCommand("LaneStreetName", DBFactoryParameterType.NVarchar, dtt.Rows[i]["LaneStreetName"]);
                    objDBAccess.AddParameterToCommand("Locality", DBFactoryParameterType.NVarchar, dtt.Rows[i]["Locality"]);
                    objDBAccess.AddParameterToCommand("STATE_ID", DBFactoryParameterType.NVarchar, dtt.Rows[i]["STATE_ID"]);
                    objDBAccess.AddParameterToCommand("DISTRICT_ID", DBFactoryParameterType.NVarchar, dtt.Rows[i]["DISTRICT_ID"]);
                    objDBAccess.AddParameterToCommand("CITY_ID", DBFactoryParameterType.NVarchar, dtt.Rows[i]["CITY_ID"]);
                    objDBAccess.AddParameterToCommand("BLOCK_ID", DBFactoryParameterType.NVarchar, dtt.Rows[i]["BLOCK_ID"]);
                    objDBAccess.AddParameterToCommand("WARD_ID", DBFactoryParameterType.NVarchar, dtt.Rows[i]["WARD_ID"]);
                    objDBAccess.AddParameterToCommand("GP_ID", DBFactoryParameterType.NVarchar, dtt.Rows[i]["GP_ID"]);
                    objDBAccess.AddParameterToCommand("VILLAGE_ID", DBFactoryParameterType.NVarchar, dtt.Rows[i]["VILLAGE_ID"]);
                    objDBAccess.AddParameterToCommand("PinCode", DBFactoryParameterType.NVarchar, dtt.Rows[i]["PinCode"]);
                    objDBAccess.AddParameterToCommand("ApplicationStatus", DBFactoryParameterType.NVarchar, dtt.Rows[i]["ApplicationStatus"]);
                }
                var data = objDBAccess.ExecuteScalarCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahakar.Models.CitizenModel", FunctionName = "public DataTable GetSocietyBasicDetails(string RegistrationNo)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            //Setdatalocal(dt);
            return "";


        }

        /// <summary>
        /// Added by #Pradeep @2025-07-01
        /// </summary>
        /// <param name="GID"></param>
        /// <returns></returns>
        public string GetJanaadharDataForPmKisan(string GID = "0")
        {
            try
            {
                for (int gid = Convert.ToInt32(GID); gid <= 85; gid++)
                {

                    DataTable dt = new PMKisanDashboardModel().Get_CMK_AadharInfo(gid.ToString());
                    foreach (DataRow dr in dt.Rows)
                    {

                        try
                        {
                            string plainInput = $"MEMBERID={dr["AadharNo"].ToString()}&TYPE=PMKISHAN";
                            string encryptedInput = AES_ECB_Encryption.Encrypt(plainInput, GlobalConstant.JanaadhaarCMKisanAesKey);

                            var requestData = new
                            {
                                ApiId = GlobalConstant.JanaadhaarCMKisanApiId,
                                Input = encryptedInput
                            };

                            string jsonPayload = JsonConvert.SerializeObject(requestData);

                            using (var client = new HttpClient())
                            {
                                var request = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                                HttpResponseMessage response = client.PostAsync(GlobalConstant.JanaadhaarCMKisanUrl, request).Result;

                                if (response.IsSuccessStatusCode)
                                {
                                    string resultEncrypted = response.Content.ReadAsStringAsync().Result;

                                    string decryptedOutput = AES_ECB_Encryption.Decrypt(resultEncrypted.Trim('"'), GlobalConstant.JanaadhaarCMKisanAesKey);
                                    List<CMK_JanaadharModel> data = JsonConvert.DeserializeObject<List<CMK_JanaadharModel>>(decryptedOutput);
                                    if (data.Count > 0)
                                    {
                                        data[0].AADHAAR_NO = dr["AadharNo"].ToString();
                                        data[0].BeneficiaryDataId = dr["BeneficiaryDataId"].ToString();
                                        new PMKisanDashboardModel().Update_CMK_JanaadharInfo(data[0]);
                                    }
                                }
                                else
                                {
                                    return "HTTP Error: " + response.StatusCode;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServices", FunctionName = "GetJanaadharDataForPmKisan(string GID)" }, ex);
                        }

                    }

                }
                return "Success";
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServices", FunctionName = "GetJanaadharDataForPmKisan(string GID)" }, ex);
            }
            return null;
        }

        /// <summary>
        /// Added by #Pradeep @2025-07-09
        /// </summary>
        /// <param name="installmentId"></param>
        /// <returns></returns>
        public async Task<string> GetPmKisanDbtTransaction(int installmentId, int pageNumber = 1, int pageSize = 500)
        {
            try
            {
                JanAadhaarGenericService _service = new JanAadhaarGenericService();
                DataTable dt = new PMKisanDashboardModel().Get_CMK_DbtTransactionByInstallmentId(installmentId, pageNumber, pageSize);
                foreach (DataRow dr in dt.Rows)
                {

                    PMKisanDashboardModel pMKisan = new PMKisanDashboardModel();
                    BeneficiaryTransactionInfo transactionInfo = new BeneficiaryTransactionInfo();
                    transactionInfo.TransactionId = Convert.ToInt64(dr["TransactionId"]);
                    transactionInfo.RegistrationNo = dr["RegistrationNo"].ToString();
                    transactionInfo.AadharNo = dr["AadharNo"].ToString();
                    transactionInfo.JanAadharMID = dr["JanAadharMID"].ToString();
                    transactionInfo.JanAadharNo = dr["JanAadharNo"].ToString();
                    transactionInfo.Amount = Convert.ToInt32(dr["Amount"]);
                    transactionInfo.TransactionDate = dr["TransactionDate"].ToString();
                    transactionInfo.BankAccountNo = dr["BankAccountNo"].ToString();
                    transactionInfo.BankIFSCCode = dr["BankIFSCCode"].ToString();
                    transactionInfo.CreditStatus = dr["CreditStatus"].ToString();
                    await pMKisan.PushPMKisanTransactionToJanaadhar(transactionInfo);
                    //await _service.PushPMKisanTransactionToJanaadhar(transactionInfo);

                }
                if (installmentId == -1)
                {
                    PMKisanDashboardModel pMKisan = new PMKisanDashboardModel();
                    BeneficiaryTransactionInfo transactionInfo = new BeneficiaryTransactionInfo();
                    transactionInfo.TransactionId = 2568599;
                    transactionInfo.RegistrationNo = "RJ116755302";
                    transactionInfo.AadharNo = "409032098170";
                    transactionInfo.JanAadharMID = "43924753271";
                    transactionInfo.JanAadharNo = "4703783330";
                    transactionInfo.Amount = 1000;
                    transactionInfo.TransactionDate = "05/07/2024";
                    transactionInfo.BankAccountNo = "663510110005237";
                    transactionInfo.BankIFSCCode = "BKID0006635";
                    transactionInfo.CreditStatus = "Success";
                    await pMKisan.PushPMKisanTransactionToJanaadhar(transactionInfo);
                    //await _service.PushPMKisanTransactionToJanaadhar(transactionInfo);
                }
                return "Success";
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServices", FunctionName = "GetJanaadharDataForPmKisan(string aadhaarNo)" }, ex);
            }
            return null;
        }

        public async Task<string> GetPmKisanDbtTransactionMultiThread(int installmentId, int pageNumber = 1, int pageSize = 200000)
        {
            try
            {
                DataTable dt = new PMKisanDashboardModel()
                    .Get_CMK_DbtTransactionByInstallmentId(installmentId, pageNumber, pageSize);

                var throttler = new SemaphoreSlim(15);

                var tasks = dt.AsEnumerable().Select(async row =>
                {
                    await throttler.WaitAsync();
                    try
                    {
                        await ProcessPmKisanTransactionAsync(row);
                    }
                    finally
                    {
                        throttler.Release();
                    }
                }).ToList();

                await Task.WhenAll(tasks);

                return "Success";
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(
                    new ErrorLogEntity()
                    {
                        PageName = "Cooperative.Models.CommonServices",
                        FunctionName = "GetPmKisanDbtTransaction"
                    }, ex);
            }

            return null;
        }
        private async Task ProcessPmKisanTransactionAsync(DataRow dr)
        {
            PMKisanDashboardModel pMKisan = new PMKisanDashboardModel();

            BeneficiaryTransactionInfo transactionInfo = new BeneficiaryTransactionInfo
            {
                TransactionId = Convert.ToInt64(dr["TransactionId"]),
                RegistrationNo = dr["RegistrationNo"].ToString(),
                AadharNo = dr["AadharNo"].ToString(),
                JanAadharMID = dr["JanAadharMID"].ToString(),
                JanAadharNo = dr["JanAadharNo"].ToString(),
                Amount = Convert.ToInt32(dr["Amount"]),
                TransactionDate = dr["TransactionDate"].ToString(),
                BankAccountNo = dr["BankAccountNo"].ToString(),
                BankIFSCCode = dr["BankIFSCCode"].ToString(),
                CreditStatus = dr["CreditStatus"].ToString()
            };

            await pMKisan.PushPMKisanTransactionToJanaadhar(transactionInfo);
        }

        public async Task<string> CMK_GenerateCsv(int installmentId, int pageNumber = 1, int pageSize = 500)
        {
            var pMKisan = new PMKisanDashboardModel();
            try
            {
                JanAadhaarGenericService _service = new JanAadhaarGenericService();
                DataTable dt = pMKisan.Get_CMK_JanAadhaarCsvDataByInstallmentId(installmentId, pageNumber, pageSize);
                var result = pMKisan.CMK_GenerateCsv(dt);
                return "Success";
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServices", FunctionName = "GetJanaadharDataForPmKisan(string aadhaarNo)" }, ex);
            }
            return null;
        }


        #region PDCS RCDF
        public PdcsResponseModel ValidatePdcsProfile(string RegNo)
        {
            var pdcsResponse = new PdcsResponseModel();
            try
            {
                var requestData = new
                {
                    Regno = RegNo
                };

                string jsonPayload = JsonConvert.SerializeObject(requestData);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                using (var client = new HttpClient())
                {
                    var request = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                    request.Headers.Add("X-API-KEY", GlobalConstant.PDCS_API_KEY);
                    HttpResponseMessage response = client.PostAsync(GlobalConstant.PDCS_PROFILE_URL, request).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        pdcsResponse = JsonConvert.DeserializeObject<PdcsResponseModel>(response.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        pdcsResponse.Status = Convert.ToInt32(response.StatusCode);
                        pdcsResponse.Message = "Error:-";
                    }
                }
            }
            catch (Exception ex)
            {
                pdcsResponse.Message = ex.Message;
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServices", FunctionName = "ValidatePdcsProfile(string RegNo)" }, ex);
            }

            return pdcsResponse;
        }
        public PdcsResponseModel UpdatePdcsProfile(string RegNo, string CoopRegNo)
        {
            var pdcsResponse = new PdcsResponseModel();
            try
            {
                var requestData = new
                {
                    Regno = RegNo,
                    RegId = CoopRegNo,
                };

                string jsonPayload = JsonConvert.SerializeObject(requestData);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                using (var client = new HttpClient())
                {
                    var request = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                    request.Headers.Add("X-API-KEY", GlobalConstant.PDCS_API_KEY);
                    HttpResponseMessage response = client.PostAsync(GlobalConstant.UPDATE_PDCS_PROFILE_URL, request).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        pdcsResponse = JsonConvert.DeserializeObject<PdcsResponseModel>(response.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        pdcsResponse.Status = Convert.ToInt32(response.StatusCode);
                        pdcsResponse.Message = "Error:-";
                    }
                }
            }
            catch (Exception ex)
            {
                pdcsResponse.Message = ex.Message;
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServices", FunctionName = "UpdatePdcsProfile(string RegNo, string CoopRegNo)" }, ex);
            }

            return pdcsResponse;
        }

        #endregion

        public DataTable BRNSocietyList(string Action = "", int SocietyId = 0, string BRN = "", string SocietyName = "")
        {
            //dt.Clear();
            DataTable datatable = new DataTable();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("chkIfSocietyBrnValidated", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@WhereHouseId", DBFactoryParameterType.Int, 0);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, Action);
                objDBAccess.AddParameterToCommand("@SocietyId", DBFactoryParameterType.Int, SocietyId);
                objDBAccess.AddParameterToCommand("@BRN", DBFactoryParameterType.NVarchar, BRN);
                objDBAccess.AddParameterToCommand("@SocietyName", DBFactoryParameterType.NVarchar, SocietyName);
                datatable = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Esahakar.Models.CitizenModel", FunctionName = "public DataTable GetCreditCooperativeSocieties(string WhereClause)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return datatable;
        }

        #region Master API - Use in Mobile App - Location DDL Fill - Society Details

        /// <summary>
        /// Created By :    Ritesh
        /// Created On :    2026-17-04
        /// Created For:    Use for Mobile App show Block List
        /// </summary>
        /// <param name="DistrictId"></param>
        /// <returns></returns>
        public DataTable BlockList(int DistrictId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("BlockList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.CommonServices", FunctionName = "public DataTable BlockList(int DistrictId = 0)" }, ex);
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
        /// Created For:    Use for Mobile App show Gram Panchayat List
        /// </summary>
        /// <param name="BlockId"></param>
        /// <returns></returns>
        public DataTable GramPanchayatList(int BlockId)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GramPanchayatList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, BlockId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.CommonServices", FunctionName = "GramPanchayatList(int BlockId)" }, ex);
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
        /// Created For:    Use for Mobile App show Village List
        /// </summary>
        /// <param name="BlockId"></param>
        /// <returns></returns>
        public DataTable VillageList(int GramPanchayatId)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("VillageList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GPId", DBFactoryParameterType.Int, GramPanchayatId);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Coopreative.Models.CommonServices", FunctionName = "VillageList(int GramPanchayatId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }
        #endregion Master API - Use in Mobile App - Location DDL Fill - Society Details


    }
    public class DomainChecker
    {
        public List<string> Domain { get; set; }
    }
}